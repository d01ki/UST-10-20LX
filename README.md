# 測域センサ「UST-20LX」 接続ガイド

「測域センサ」という言葉は北陽電機（Hokuyo）独自の呼称で、  
一般的には **LiDAR（Light Detection and Ranging）** と呼ばれます。  
ここでは、UST-20LX の基本的な接続手順をまとめる。

---

## 必要なもの

| 項目 | 内容 |
|------|------|
| センサ本体 | UST-20LX（デフォルトIP: `192.168.0.10`） |
| 電源アダプタ | DC **12V または 24V** 対応（安定した出力が必要） |
| ワニ口クリップ | センサと電源の接続に使用 |

<img src="https://github.com/user-attachments/assets/a34e060d-fa74-4d5f-a937-8f664b2673a0" width="350">
>正常に電源が入ると青ランプがつく


<img src="https://github.com/user-attachments/assets/e6710584-faa5-4ed6-ab92-7d16e791cccb" width="400">




---

## 接続方法

###  電源とセンサの接続

1. 電源アダプター（12Vまたは24V）を用意します。  
2. センサの電源線（赤＝＋、黒＝−）をワニ口クリップなどで接続します。  
3. 電源を入れるとセンサが起動し、緑ランプが点灯します。

---

###  PCのIPアドレス設定

USTセンサは初期状態で **`192.168.0.10`** に設定されています。  
PC側のIPを同一セグメントに設定しましょう。

1. `Windows + R` を押して「ファイル名を指定して実行」を開く  
2. `ncpa.cpl` と入力して **ネットワーク接続** 画面を開く  


<img width="451" height="256" alt="ネットワーク接続" src="https://github.com/user-attachments/assets/1e1939d5-68d9-43ee-ae38-bae7af58be6a" />


3. 有線LAN（イーサネット）を右クリック → **プロパティ**

<img width="476" height="680" alt="{977BF823-9214-4410-BB30-28CF213857EB}" src="https://github.com/user-attachments/assets/19666ca6-73c5-4ddd-b615-cd381d57afd0" />


4. 「インターネット プロトコル バージョン4（TCP/IPv4）」を選択し **プロパティ** をクリック  

<img width="389" height="375" alt="{A730788B-2FE0-4FAB-9CBE-90917B703898}" src="https://github.com/user-attachments/assets/183a175c-58b5-4eae-af7d-79d079f00bb2" />


5. 以下のように設定します：

| 項目 | 設定値 |
|------|--------|
| IPアドレス | `192.168.0.100` |
| サブネットマスク | `255.255.255.0` |
| デフォルトゲートウェイ | （空欄でOK） |

<img width="467" height="528" alt="{A24F080A-ED88-4CBE-8098-0EB36834D6A4}" src="https://github.com/user-attachments/assets/7dfdd09f-f4f6-4ded-a090-2d24f912a6f4" />

設定を保存し、ウィンドウを閉じます。

---

### UrgBenriPlus（北陽公式ツール）の設定

URG は、北陽電機製のレーザ距離センサです。広範囲の距離データを高精度で取得できます。

https://sourceforge.net/p/urgnetwork/wiki/whatis_jp/

UrgBenriPlusを以下のリンクからインストールする

https://www.hokuyo-aut.co.jp/search/single.php?serial=16#download

<img width="847" height="92" alt="{876E3120-9397-404F-AE52-D1D1584B853D}" src="https://github.com/user-attachments/assets/0cb6096e-d897-4378-b757-026f50af8194" />

順番に沿って進めていく

1. UrgBenriPlus を起動します。  
2. 右上の「Connectors」から **Ethernet1** を選択。  
3. 以下のように入力します：

| 項目 | 値 |
|------|----|
| IPアドレス | `192.168.0.10` |
| ポート番号 | `10940` |

4. 「▶」ボタンで接続を開始します。

<img width="973" height="626" alt="UrgBenriPlus設定" src="https://github.com/user-attachments/assets/92faf08a-b498-428b-a3d8-e09395ee630e" />





<img width="1228" height="275" alt="{D5F0F390-7E6C-4BA5-B1C9-9995652CD2A3}" src="https://github.com/user-attachments/assets/d83b3eee-7407-416a-8d24-2a034f679aa9" />

# UrgBenriPlusを少しいじってみた

## ステータス情報取得コマンド

画面右の「コンソール」を押して「PP」と入力すると写真のようなデータが返ってくる
分かりやすくすると以下の表になる

| 項目       | 意味            | 値                    |
| -------- | ------------- | -------------------- |
| **MODL** | モデル名          | `UST-20LX`           |
| **DMIN** | 測定可能な最短距離（mm） | `20`                 |
| **DMAX** | 測定可能な最長距離（mm） | `60000`              |
| **ARES** | 角度分解能（ステップ数）  | `1440`（＝約0.25°/step） |
| **AMIN** | 最小角度（ステップ）    | `0`                  |
| **AMAX** | 最大角度（ステップ）    | `1080`（＝約270°）       |
| **AFRT** | 中央角度（ステップ）    | `540`                |
| **SCAN** | スキャン周期（ms）    | `2400`（＝約25Hz）       |

→分かること距離の計測範囲 2cm~60m（ほんとかよ～）

他にも「VV」、「II」コマンドがあるらしい

参考
https://sourceforge.net/p/urgnetwork/wiki/scip_status_jp/

## 距離・強度データを表示するツールを使ってみる

Urg Viewer -- 距離・強度データを表示するアプリケーションです。 記録したデータを再生することが可能です。

以下からダウンロードして .exeをクリックして開く

https://sourceforge.net/p/urgnetwork/wiki/urg_viewer_jp/


<img width="1916" height="1011" alt="{677D7963-E4C8-4077-A1CF-A2E4CB3B5E39}" src="https://github.com/user-attachments/assets/bb6b904a-d936-4333-b23f-c7a53340d35a" />

オレンジの線

- センサに対して手が近いほどオレンジの線が広がる

青の線

- レーザーが届いている部分

トラブルシューティング

- FW　ONにしたら接続できた

<img width="1922" height="1004" alt="{49552A43-3182-4AB7-8BB1-7FC8C2445E67}" src="https://github.com/user-attachments/assets/aa0185e4-576f-415d-9968-81b04613081d" />



## Unityで直接UST-20LXと通信する方法



```
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
```


受信できているデータ
- SCIP2.0の応答（21 bytes）
- MDコマンドの応答（2275 bytes）
- 距離データが連続で届いている（99b で始まるデータ）
画面下部に見える16進数のような文字列がLiDARの測定データ


<img width="1897" height="990" alt="image" src="https://github.com/user-attachments/assets/d03cb4b8-2ace-495b-bf09-0fab241c4036" />


受信内容

```
受信内容: MD0000072500100
99b
C?i1L
06m02e0KD0KD0K>0K70K60Jl0Ji0Jf0Jd0J\0JQ0JG0JE0JD0J@0J60J40Ij0J10`
Id0Ie0I`0I_0IY0IT0IV0IT0IT0IR0IF0IN0IA0IA0I?0I;0I80Ho0Hl0Hl0Hj0H_
h0Hh0Hd0Hh0Ho0H^0H]0Hf0Ha0Ha0I:1GD1Ga1Gi1G
UnityEngine.Debug:Log (object)
LiDARTest:ReceiveData () (at Assets/Scripts/LiDARTest.cs:81)
System.Threading.ThreadHelper:ThreadStart ()

```

データ構造

```
MD0000072500100    ← コマンドエコー（送信したコマンドの確認）
99b                ← ステータスコード（99b = 正常）
C?i1L              ← タイムスタンプ
06m02e0K...        ← 距離データ（SCIP2.0エンコード）
```

SCIP2.0エンコーディングの解読

- **3文字で1つの距離値**を表現
- 各文字は6ビット（0x30を引いてデコード）
- 例：`06m` → 距離1つ、`02e` → 距離1つ

デコード式
距離(mm) = ((char1 - 0x30) << 12) | ((char2 - 0x30) << 6) | (char3 - 0x30)


decode.cs（本来の命名はLiDARTest.csのまま、検証段階なので一時的にdecode.cs）

<img width="1916" height="993" alt="{5562BDC6-90C8-4286-A27B-AE5CECFF19F4}" src="https://github.com/user-attachments/assets/b8e40a91-5102-4d95-8eea-cac57658730a" />


---

## トラブルシューティング

| 症状 | 原因 | 対処法 |
|------|------|--------|
| 接続できない | IPアドレス設定ミス | PCとセンサが同じネットワーク帯 (`192.168.0.x`) にあるか確認 |
| ゲージが出ない | 電源供給不足 | 12V / 24V の電源が安定しているかチェック |
| データが途切れる | LANケーブルの品質 | シールドケーブルを使用する |

## 参考になるかもしれないページ集

測域センサの原理と使い方
https://www.roboken.iit.tsukuba.ac.jp/lectures/software_science_experiment/2019/document/sokuiki_sensor.pdf

https://sourceforge.net/p/urgnetwork/wiki/top_jp/

https://sourceforge.net/p/urgnetwork/wiki/vssp_capture_jp/

測域センサ

https://qiita.com/atsonic/items/be9e15f528e34e370d32

電源の入れ方

https://qiita.com/atsonic/items/be9e15f528e34e370d32

https://www.hokuyo-aut.co.jp/products/data.php?id=1

センサーの設定

https://note.com/bunkeidatte/n/n23ca5181a009

この測域センサーを使用するには、以下2点が必要
・HOKUYO　ホームページから【RS2】データ確認ツール　UrgBenriPlusのインストール

https://www.hokuyo-aut.co.jp/search/single.php?serial=16#download

・パソコンのIPアドレスの設定が必要

センサのIPアドレス（デフォルト192.168.0.10:10940）/自分のPCは192.168.0.100に設定

URG Benriに測域センサ(UST-20LX)が繋がらないときの対処法＆接続方法

https://qiita.com/MOSO1409/items/bfe4d3baecffde94a172
測域センサの汎用ツール "HokuyoUtil"
https://zenn.dev/kjkmr/articles/7a170492293090
https://github.com/STARRYWORKS-inc/HokuyoUtil

仕様書
https://img.atwiki.jp/kanazawa2robocar/attach/20/2/URG-Series_SCIP2_Compatible_Communication_Specification_JPN.pdf
