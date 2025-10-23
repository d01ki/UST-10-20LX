using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class LiDARTest : MonoBehaviour
{
    [Header("LiDAR Settings")]
    public string lidarIP = "192.168.0.10";
    public int lidarPort = 10940;
    
    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;
    bool isRunning = false;
    
    Queue<string> dataQueue = new Queue<string>();
    object queueLock = new object();
    
    void Start()
    {
        Debug.Log("=== LiDAR通信開始 ===");
        
        try
        {
            client = new TcpClient();
            client.Connect(lidarIP, lidarPort);
            stream = client.GetStream();
            
            Debug.Log("TCP接続成功！");
            
            SendCommand("SCIP2.0\n");
            Thread.Sleep(300);
            
            SendCommand("BM\n");
            Thread.Sleep(300);
            
            SendCommand("MD0000072500001\n"); // 1回だけスキャン
            
            isRunning = true;
            receiveThread = new Thread(ReceiveData);
            receiveThread.Start();
            
            Debug.Log("受信開始");
        }
        catch (Exception e)
        {
            Debug.LogError($"接続エラー: {e.Message}");
        }
    }
    
    void Update()
    {
        lock (queueLock)
        {
            while (dataQueue.Count > 0)
            {
                string data = dataQueue.Dequeue();
                ProcessLiDARData(data);
            }
        }
    }
    
    void SendCommand(string cmd)
    {
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(cmd);
            stream.Write(data, 0, data.Length);
            Debug.Log($"送信: {cmd.Trim()}");
        }
        catch (Exception e)
        {
            Debug.LogError($"送信エラー: {e.Message}");
        }
    }
    
    void ReceiveData()
    {
        byte[] buffer = new byte[8192];
        StringBuilder messageBuilder = new StringBuilder();
        
        while (isRunning)
        {
            try
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(chunk);
                    
                    // 改行で分割
                    string fullMessage = messageBuilder.ToString();
                    string[] lines = fullMessage.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    // 最後の行が不完全かもしれないので保持
                    if (!fullMessage.EndsWith("\n"))
                    {
                        messageBuilder.Clear();
                        messageBuilder.Append(lines[lines.Length - 1]);
                        Array.Resize(ref lines, lines.Length - 1);
                    }
                    else
                    {
                        messageBuilder.Clear();
                    }
                    
                    // 完全な行をキューに追加
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            lock (queueLock)
                            {
                                dataQueue.Enqueue(line.Trim());
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
            catch (Exception e)
            {
                Debug.LogError($"受信エラー: {e.Message}");
            }
        }
    }
    
    void ProcessLiDARData(string line)
    {
        Debug.Log($"受信行: {line}");
        
        // MDで始まる行（コマンドエコー）
        if (line.StartsWith("MD"))
        {
            Debug.Log("→ コマンドエコー");
            return;
        }
        
        // ステータスコード（00, 99など）
        if (line.Length <= 5)
        {
            Debug.Log($"→ ステータス: {line}");
            return;
        }
        
        // タイムスタンプ（大文字で始まる短い行）
        if (line.Length < 20 && char.IsUpper(line[0]))
        {
            Debug.Log($"→ タイムスタンプ: {line}");
            return;
        }
        
        // 距離データ（長い行）
        if (line.Length > 50)
        {
            Debug.Log($"→ 距離データ検出 ({line.Length}文字)");
            List<int> distances = DecodeSCIP2Data(line);
            
            Debug.Log($"====================");
            Debug.Log($"測定点数: {distances.Count}");
            Debug.Log($"最初の10点の距離 (mm):");
            for (int i = 0; i < Math.Min(10, distances.Count); i++)
            {
                Debug.Log($"  点{i}: {distances[i]} mm");
            }
            Debug.Log($"====================");
        }
    }
    
    List<int> DecodeSCIP2Data(string encodedData)
    {
        List<int> distances = new List<int>();
        
        // 3文字ずつ処理
        for (int i = 0; i <= encodedData.Length - 3; i += 3)
        {
            try
            {
                char c1 = encodedData[i];
                char c2 = encodedData[i + 1];
                char c3 = encodedData[i + 2];
                
                // SCIP2.0デコード（0x30を引く）
                int val = ((c1 - 0x30) << 12) | ((c2 - 0x30) << 6) | (c3 - 0x30);
                
                distances.Add(val);
            }
            catch (Exception e)
            {
                Debug.LogError($"デコードエラー at {i}: {e.Message}");
                break;
            }
        }
        
        return distances;
    }
    
    void OnApplicationQuit()
    {
        Debug.Log("終了処理");
        isRunning = false;
        
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(1000);
        }
        
        if (stream != null)
        {
            SendCommand("QT\n");
            Thread.Sleep(100);
            stream.Close();
        }
        
        if (client != null)
        {
            client.Close();
        }
        
        Debug.Log("終了完了");
    }
}
