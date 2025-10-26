using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class LiDARVisualizer : MonoBehaviour
{
    [Header("LiDAR Settings")]
    public string lidarIP = "192.168.0.10";
    public int lidarPort = 10940;
    
    [Header("Visualization Settings")]
    public float visualScale = 0.01f; // mm to Unity units (大きめに調整)
    public float maxDistance = 5000f; // 最大表示距離 (mm)
    public Color lineColor = Color.green;
    public Color sensorCenterColor = Color.red;
    public float lineWidth = 0.05f;
    
    [Header("Camera Settings")]
    public bool autoSetupCamera = true; // 自動でカメラを真上に配置
    public float cameraHeight = 30f; // カメラの高さ
    
    // LiDAR通信
    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;
    bool isRunning = false;
    
    // データ処理
    Queue<string> dataQueue = new Queue<string>();
    object queueLock = new object();
    List<int> latestDistances = new List<int>();
    
    // 可視化
    LineRenderer lineRenderer;
    GameObject sensorCenter; // センサー中心点の表示
    
    void Start()
    {
        // カメラを真上に配置
        if (autoSetupCamera)
        {
            SetupTopDownCamera();
        }
        
        // 可視化設定
        SetupVisualization();
        
        Debug.Log("=== LiDAR可視化システム開始 ===");
        
        try
        {
            // TCP接続
            client = new TcpClient();
            client.Connect(lidarIP, lidarPort);
            stream = client.GetStream();
            
            Debug.Log("TCP接続成功！");
            
            // LiDAR初期化
            SendCommand("SCIP2.0\n");
            Thread.Sleep(300);
            
            SendCommand("BM\n");
            Thread.Sleep(300);
            
            // 連続スキャンモード（00は無限ループ）
            SendCommand("MD0000072500000\n");
            
            // 受信スレッド開始
            isRunning = true;
            receiveThread = new Thread(ReceiveData);
            receiveThread.Start();
            
            Debug.Log("連続スキャン開始 - 真上ビューで表示中");
        }
        catch (Exception e)
        {
            Debug.LogError($"接続エラー: {e.Message}");
        }
    }
    
    void SetupTopDownCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Cameraが見つかりません。手動でカメラを配置してください。");
            return;
        }
        
        // カメラを真上に配置
        mainCamera.transform.position = new Vector3(0, cameraHeight, 0);
        mainCamera.transform.rotation = Quaternion.Euler(90f, 0, 0); // 真下を向く
        
        // Orthographic（平行投影）にすると更に見やすい
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 10f; // 表示範囲
        
        Debug.Log("カメラを真上ビューに設定しました");
    }
    
    void SetupVisualization()
    {
        // LineRenderer設定
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0;
        
        // センサー中心点を作成
        sensorCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sensorCenter.transform.position = Vector3.zero;
        sensorCenter.transform.localScale = Vector3.one * 0.3f;
        sensorCenter.name = "LiDAR Sensor Center";
        
        // センサーの色を設定
        Renderer renderer = sensorCenter.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = sensorCenterColor;
        
        Debug.Log("可視化オブジェクトを作成しました");
    }
    
    void Update()
    {
        // キューからデータを取り出して処理
        lock (queueLock)
        {
            while (dataQueue.Count > 0)
            {
                string data = dataQueue.Dequeue();
                ProcessLiDARData(data);
            }
        }
        
        // 最新データを可視化
        if (latestDistances.Count > 0)
        {
            VisualizeData();
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
        // コマンドエコー
        if (line.StartsWith("MD"))
        {
            return;
        }
        
        // ステータスコード
        if (line.Length <= 5)
        {
            return;
        }
        
        // タイムスタンプ
        if (line.Length < 20 && char.IsUpper(line[0]))
        {
            return;
        }
        
        // 距離データ（長い行）
        if (line.Length > 50)
        {
            List<int> distances = DecodeSCIP2Data(line);
            
            lock (queueLock)
            {
                latestDistances = distances;
            }
            
            // コンソールに最初の10点を表示
            Debug.Log($"========== 測定点数: {distances.Count} ==========");
            for (int i = 0; i < Math.Min(10, distances.Count); i++)
            {
                Debug.Log($"  点{i}: {distances[i]} mm");
            }
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
                
                // SCIP2.0デコード
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
    
    void VisualizeData()
    {
        List<int> distances;
        lock (queueLock)
        {
            distances = new List<int>(latestDistances);
        }
        
        if (distances.Count == 0) return;
        
        // LineRendererのポイント数を設定
        lineRenderer.positionCount = distances.Count + 1; // +1で始点に戻る
        
        // センサーの角度範囲を計算（UST-10LXは270度）
        float angleRange = 270f;
        float angleStep = angleRange / (distances.Count - 1);
        float startAngle = -135f; // -135度から+135度
        
        // 各点の座標を計算（Y=0の平面上に描画）
        for (int i = 0; i < distances.Count; i++)
        {
            float distance = distances[i];
            
            // 距離が0または最大値を超える場合は最大値に設定
            if (distance <= 0 || distance > maxDistance)
            {
                distance = maxDistance;
            }
            
            // 角度を計算（度からラジアンへ）
            float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
            
            // 極座標からデカルト座標へ変換（XZ平面上に描画、Y=0）
            float x = distance * Mathf.Cos(angle) * visualScale;
            float z = distance * Mathf.Sin(angle) * visualScale;
            
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
        
        // 最後の点を最初の点に接続して閉じる
        lineRenderer.SetPosition(distances.Count, lineRenderer.GetPosition(0));
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
