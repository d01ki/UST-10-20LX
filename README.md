# 測域センサ「UST-20LX」接続ガイド

## 概要

「測域センサ」は北陽電機（Hokuyo）独自の呼称で、一般的には **LiDAR（Light Detection and Ranging）** と呼ばれています。

このガイドでは、UST-20LXの基本的な接続手順から、データ取得、Unityとの通信方法まで、段階的に解説します。

---

## 目次

1. [必要な機材](#必要な機材)
2. [接続手順](#接続手順)
   - [電源とセンサの接続](#電源とセンサの接続)
   - [PCのIPアドレス設定](#pcのipアドレス設定)
   - [UrgBenriPlusの設定](#urgbenriplusの設定)
3. [UrgBenriPlusの活用](#urgbenriplusの活用)
   - [ステータス情報の取得](#ステータス情報の取得)
   - [距離・強度データの表示](#距離強度データの表示)
4. [Unityとの連携](#unityとの連携)
5. [トラブルシューティング](#トラブルシューティング)
6. [参考資料](#参考資料)

---

## 必要な機材

| 項目 | 内容 |
|------|------|
| センサ本体 | UST-20LX（デフォルトIP: `192.168.0.10`） |
| 電源アダプタ | DC **12V または 24V** 対応（安定した出力が必要） |
| ワニ口クリップ | センサと電源の接続に使用 |

### センサの状態確認

<img src="https://github.com/user-attachments/assets/a34e060d-fa74-4d5f-a937-8f664b2673a0" width="350">

> 正常に電源が入ると青ランプが点灯します

<img src="https://github.com/user-attachments/assets/e6710584-faa5-4ed6-ab92-7d16e791cccb" width="400">

---

## 接続手順

### 電源とセンサの接続

1. 電源アダプター（12Vまたは24V）を用意します
2. センサの電源線を接続します：
   - **赤線** = プラス（＋）
   - **黒線** = マイナス（−）
   - ワニ口クリップなどで確実に接続してください
 
  
  
  ![IMG_4925 (1)](https://github.com/user-attachments/assets/798be07a-b703-4b78-bc59-7fb252c91502)


3. 電源を入れるとセンサが起動し、緑ランプが点灯します

### PCのIPアドレス設定

USTセンサは初期状態で **`192.168.0.10`** に設定されています。PC側のIPアドレスを同一セグメントに設定する必要があります。

#### 設定手順

1. `Windows + R` を押して「ファイル名を指定して実行」を開きます
2. `ncpa.cpl` と入力して **ネットワーク接続** 画面を開きます

<img width="451" height="256" alt="ネットワーク接続" src="https://github.com/user-attachments/assets/1e1939d5-68d9-43ee-ae38-bae7af58be6a" />

3. 有線LAN（イーサネット）を右クリック → **プロパティ** を選択します

<img width="476" height="680" alt="{977BF823-9214-4410-BB30-28CF213857EB}" src="https://github.com/user-attachments/assets/19666ca6-73c5-4ddd-b615-cd381d57afd0" />

4. 「インターネット プロトコル バージョン4（TCP/IPv4）」を選択し **プロパティ** をクリックします

<img width="389" height="375" alt="{A730788B-2FE0-4FAB-9CBE-90917B703898}" src="https://github.com/user-attachments/assets/183a175c-58b5-4eae-af7d-79d079f00bb2" />

5. 以下のように設定します：

| 項目 | 設定値 |
|------|--------|
| IPアドレス | `192.168.0.100` |
| サブネットマスク | `255.255.255.0` |
| デフォルトゲートウェイ | （空欄でOK） |

<img width="467" height="528" alt="{A24F080A-ED88-4CBE-8098-0EB36834D6A4}" src="https://github.com/user-attachments/assets/7dfdd09f-f4f6-4ded-a090-2d24f912a6f4" />

6. 設定を保存し、ウィンドウを閉じます

7. 疎通確認


<img width="714" height="309" alt="{95BA340E-BD8D-4A20-A79F-595AA73BF4AD}" src="https://github.com/user-attachments/assets/c2bbc48e-9ec2-4f16-a3c6-5cbe65c70f29" />



### UrgBenriPlusの設定

#### URGセンサとは

URGは北陽電機製のレーザ距離センサです。広範囲の距離データを高精度で取得できます。

詳細：https://sourceforge.net/p/urgnetwork/wiki/whatis_jp/

#### インストール

以下のリンクからUrgBenriPlusをダウンロードしてインストールします：

https://www.hokuyo-aut.co.jp/search/single.php?serial=16#download

<img width="847" height="92" alt="{876E3120-9397-404F-AE52-D1D1584B853D}" src="https://github.com/user-attachments/assets/0cb6096e-d897-4378-b757-026f50af8194" />

#### 接続設定

1. UrgBenriPlus を起動します
2. 右上の「Connectors」から **Ethernet1** を選択します
3. 以下のように入力します：

| 項目 | 値 |
|------|-------|
| IPアドレス | `192.168.0.10` |
| ポート番号 | `10940` |

4. 「▶」ボタンで接続を開始します

<img width="973" height="626" alt="UrgBenriPlus設定" src="https://github.com/user-attachments/assets/92faf08a-b498-428b-a3d8-e09395ee630e" />

<img width="1228" height="275" alt="{D5F0F390-7E6C-4BA5-B1C9-9995652CD2A3}" src="https://github.com/user-attachments/assets/d83b3eee-7407-416a-8d24-2a034f679aa9" />

---

## UrgBenriPlusの活用

### ステータス情報の取得

画面右の「コンソール」を押して **`PP`** と入力すると、センサのステータス情報が取得できます。

#### 取得できる情報

| 項目 | 意味 | 値 |
|------|------|-----|
| **MODL** | モデル名 | `UST-20LX` |
| **DMIN** | 測定可能な最短距離（mm） | `20` |
| **DMAX** | 測定可能な最長距離（mm） | `60000` |
| **ARES** | 角度分解能（ステップ数） | `1440`（≈0.25°/step） |
| **AMIN** | 最小角度（ステップ） | `0` |
| **AMAX** | 最大角度（ステップ） | `1080`（≈270°） |
| **AFRT** | 中央角度（ステップ） | `540` |
| **SCAN** | スキャン周期（ms） | `2400`（≈25Hz） |

**わかること：** 距離の計測範囲は 2cm～60m

#### その他のコマンド

- **`VV`** - バージョン情報
- **`II`** - センサ情報

参考：https://sourceforge.net/p/urgnetwork/wiki/scip_status_jp/

### 距離・強度データの表示

**Urg Viewer** は距離・強度データを表示するアプリケーションです。記録したデータの再生も可能です。

#### ダウンロード

以下からダウンロードして .exe ファイルを実行します：

https://sourceforge.net/p/urgnetwork/wiki/urg_viewer_jp/

<img width="1916" height="1011" alt="{677D7963-E4C8-4077-A1CF-A2E4CB3B5E39}" src="https://github.com/user-attachments/assets/bb6b904a-d936-4333-b23f-c7a53340d35a" />

#### 表示内容

- **オレンジの線** - センサに対して手が近いほど線が広がります
- **青の線** - レーザーが届いている部分を示します

#### トラブルシューティング

接続できない場合は **FW（ファイアウォール）を ON** にしてください。

<img width="1922" height="1004" alt="{49552A43-3182-4AB7-8BB1-7FC8C2445E67}" src="https://github.com/user-attachments/assets/aa0185e4-576f-415d-9968-81b04613081d" />

---

## Unityとの連携

UnityからUST-20LXと直接通信し、LiDARデータを取得する方法を説明します。

### 1. スクリプトの作成

#### 手順

1. **Project** ウィンドウで **Assets/Scripts** フォルダを右クリック
2. **Create > C# Script** を選択
3. 名前を **LiDARTest** と入力（拡張子 .cs は自動で付きます）
4. **Enter** キーを押して確定
5. **LiDARTest** をダブルクリックして開く
6. 全ての内容を削除
7. 以下のコードを貼り付けて **Ctrl+S** で保存

```csharp
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

8. Unityに戻る
9. Visual Studio / VSCodeを閉じる
10. Unityエディタに戻る
11. Consoleウィンドウでコンパイルエラーがないか確認

### 2. GameObjectにアタッチ

1. **Hierarchy** で右クリック > **Create Empty**
2. 名前を **「LiDARReceiver」** に変更
3. **Project** ウィンドウから **LiDARTest** スクリプトをドラッグして **「LiDARReceiver」** にドロップ

#### 確認

- **Inspector** で **「LiDAR Test (Script)」** と表示されていればOK
- **「Missing Script」** と表示されている場合は、ファイル名が間違っています

### 3. 受信データの確認

正常に接続できると、以下のデータが受信されます：

<img width="1897" height="990" alt="image" src="https://github.com/user-attachments/assets/d03cb4b8-2ace-495b-bf09-0fab241c4036" />

#### 受信できるデータ

- **SCIP2.0の応答**（21 bytes）
- **MDコマンドの応答**（2275 bytes）
- **距離データ**（連続して届く、99b で始まるデータ）

画面下部に見える16進数のような文字列がLiDARの測定データです。

#### 受信内容の例

```
受信内容: MD0000072500100
99b
C?i1L
06m02e0KD0KD0K>0K70K60Jl0Ji0Jf0Jd0J\0JQ0JG0JE0JD0J@0J60J40Ij0J10`
Id0Ie0I`0I_0IY0IT0IV0IT0IT0IR0IF0IN0IA0IA0I?0I;0I80Ho0Hl0Hl0Hj0H_
h0Hh0Hd0Hh0Ho0H^0H]0Hf0Ha0Ha0I:1GD1Ga1Gi1G
```

#### データ構造

```
MD0000072500100    ← コマンドエコー（送信したコマンドの確認）
99b                ← ステータスコード（99b = 正常）
C?i1L              ← タイムスタンプ
06m02e0K...        ← 距離データ（SCIP2.0エンコード）
```

### 4. SCIP2.0エンコーディングの解読

LiDARから送られてくるデータは、SCIP2.0という独自のエンコーディング形式で圧縮されています。

#### エンコーディングの特徴

- **3文字で1つの距離値**を表現
- 各文字は6ビットの情報を持つ（0x30を引いてデコード）
- 例：`06m` → 距離1つ、`02e` → 距離1つ

#### デコード式

```
距離(mm) = ((char1 - 0x30) << 12) | ((char2 - 0x30) << 6) | (char3 - 0x30)
```

#### デコード実装例

<img width="1916" height="993" alt="{5562BDC6-90C8-4286-A27B-AE5CECFF19F4}" src="https://github.com/user-attachments/assets/b8e40a91-5102-4d95-8eea-cac57658730a" />

> **注意：** 本来のファイル名は LiDARTest.cs のままですが、検証段階では一時的に decode.cs として使用しています

### 5. ファイアウォール設定（受信できない場合）

#### 原因

Windowsのファイアウォールが通信をブロックしている可能性があります。

#### 対処法：Windowsファイアウォールの設定

1. **Windowsキー + R** → **`wf.msc`** と入力して **Enter**
2. 左側のメニューから **「受信の規則」** をクリック
3. 右側のメニューから **「新しい規則」** をクリック
4. **「ポート」** を選択 → **次へ**
5. **「UDP」** を選択、**「特定のローカルポート」** に **`10940`** を入力
6. **「接続を許可する」** を選択
7. すべてのプロファイル（ドメイン、プライベート、パブリック）にチェック
8. 名前を **「Unity LiDAR」** として完了

---

## トラブルシューティング

| 症状 | 原因 | 対処法 |
|------|------|--------|
| 接続できない | IPアドレス設定ミス | PCとセンサが同じネットワーク帯（`192.168.0.x`）にあるか確認 |
| ゲージが出ない | 電源供給不足 | 12V / 24V の電源が安定しているかチェック |
| データが途切れる | LANケーブルの品質 | シールドケーブルを使用する |

---

## 参考資料

### 公式ドキュメント

- **測域センサの原理と使い方**  
  https://www.roboken.iit.tsukuba.ac.jp/lectures/software_science_experiment/2019/document/sokuiki_sensor.pdf

- **URG Network 公式Wiki**  
  https://sourceforge.net/p/urgnetwork/wiki/top_jp/

- **VSSP Capture（データ取得ツール）**  
  https://sourceforge.net/p/urgnetwork/wiki/vssp_capture_jp/

- **仕様書（SCIP2.0通信仕様）**  
  https://img.atwiki.jp/kanazawa2robocar/attach/20/2/URG-Series_SCIP2_Compatible_Communication_Specification_JPN.pdf

### 解説記事

- **測域センサの使い方（Qiita）**  
  https://qiita.com/atsonic/items/be9e15f528e34e370d32

- **電源の入れ方**  
  https://qiita.com/atsonic/items/be9e15f528e34e370d32

- **北陽電機製品情報**  
  https://www.hokuyo-aut.co.jp/products/data.php?id=1

- **センサーの設定（note）**  
  https://note.com/bunkeidatte/n/n23ca5181a009

- **URG Benriに測域センサ(UST-20LX)が繋がらないときの対処法**  
  https://qiita.com/MOSO1409/items/bfe4d3baecffde94a172

### 便利なツール

- **測域センサの汎用ツール "HokuyoUtil"**  
  https://zenn.dev/kjkmr/articles/7a170492293090  
  https://github.com/STARRYWORKS-inc/HokuyoUtil

---

## 必要なソフトウェア

このセンサを使用するには、以下2点が必要です：

1. **HOKUYO公式ツール「UrgBenriPlus」のインストール**  
   https://www.hokuyo-aut.co.jp/search/single.php?serial=16#download

2. **パソコンのIPアドレス設定**
   - センサのIPアドレス（デフォルト）：`192.168.0.10:10940`
   - 自分のPCのIPアドレス：`192.168.0.100` に設定

---

## まとめ

このガイドでは、UST-20LXの接続から、データ取得、Unityとの連携まで一通りの手順を説明しました。トラブルが発生した場合は、トラブルシューティングのセクションや参考資料を確認してください。

ご不明点がありましたら、参考資料のリンクもご活用ください。
