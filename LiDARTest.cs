using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class LiDARTest : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;
    bool isRunning = false;
    
    void Start()
    {
        Debug.Log("=== LiDAR通信開始 (TCP版) ===");
        
        try
        {
            // TCPで接続
            client = new TcpClient();
            client.Connect("192.168.0.10", 10940);
            stream = client.GetStream();
            
            Debug.Log("TCP接続成功！");
            
            // 接続確認
            SendCommand("SCIP2.0\n");
            Thread.Sleep(300);
            
            // 計測開始
            SendCommand("BM\n");
            Thread.Sleep(300);
            
            // データ送信要求
            SendCommand("MD0000072500101\n");
            
            // 受信スレッド開始
            isRunning = true;
            receiveThread = new Thread(ReceiveData);
            receiveThread.Start();
            
            Debug.Log("受信スレッド開始");
        }
        catch (Exception e)
        {
            Debug.LogError($"接続エラー: {e.Message}");
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
        byte[] buffer = new byte[4096];
        
        while (isRunning)
        {
            try
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    
                    Debug.Log("!!!!!!!! 受信成功 (TCP) !!!!!!!!");
                    Debug.Log($"受信データサイズ: {bytesRead} bytes");
                    Debug.Log($"受信内容: {msg.Substring(0, Math.Min(200, msg.Length))}");
                    Debug.Log("========================================");
                }
                Thread.Sleep(10);
            }
            catch (Exception e)
            {
                Debug.LogError($"受信エラー: {e.Message}");
            }
        }
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
