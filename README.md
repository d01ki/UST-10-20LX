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



## Unityで直接UST-20LXと通信する方法

Unityプロジェクトのセットアップ
1. 新規Unityプロジェクトを作成

Unity 2021.3 LTS以降を推奨
3D (URP) テンプレートを選択

2. センサー通信用のC#スクリプト作成

3. タッチ検出スクリプトの作成
壁に触れた部分を検出するスクリプト：

4. 水面エフェクトのシェーダー作成

5. 水面エフェクト用のスクリプト

   Unity でのセットアップ手順
ステップ1: スクリプトの配置

Unityで Assets/Scripts フォルダを作成
上記の4つのスクリプトをコピー＆ペースト：

HokuyoUST20LX.cs
WallTouchDetector.cs
WaterRippleEffect.cs


Assets/Shaders フォルダを作成
WaterRipple.shader を配置

ステップ2: シーンのセットアップ

空のGameObjectを作成（名前: SensorManager）

HokuyoUST20LX スクリプトをアタッチ
WallTouchDetector スクリプトをアタッチ


水面エフェクトのPrefabを作成

Hierarchy で 3D Object → Quad を作成
名前を WaterRipple に変更
Rotation を X:-90 に設定（床に平行に）
Scale を (0.5, 0.5, 0.5) に設定


マテリアルの作成

Assets/Materials フォルダを作成
右クリック → Create → Material （名前: WaterRippleMat）
Shader を Custom/WaterRipple に変更
このマテリアルを WaterRipple Quadにアタッチ


Prefab化

WaterRipple に WaterRippleEffect.cs をアタッチ
WaterRipple を Project ウィンドウにドラッグしてPrefab化
Hierarchyから削除


設定

SensorManager の WallTouchDetector コンポーネント：

Sensor に SensorManager 自身をドラッグ
Water Ripple Prefab に作成した WaterRipple Prefabをドラッグ


ステップ3: テスト実行

Playボタンを押す
Consoleを確認

"✓ センサーに接続しました" と表示されればOK


Scene ビューで確認

緑色の点（センサーデータ）が表示される
黄色の円（基準距離）が表示される


壁に手を近づける

赤い球（タッチポイント）が表示される
水面エフェクトが発生する
---

## 動作確認

- 緑のゲージが表示されていれば、センサが正常に距離データを取得しています。  
- 物体を前に置くと、ゲージの長さが変化します。  
- UST-10LX は最大 10m、UST-20LX は最大 20m まで測定可能です。  

---

## トラブルシューティング

| 症状 | 原因 | 対処法 |
|------|------|--------|
| 接続できない | IPアドレス設定ミス | PCとセンサが同じネットワーク帯 (`192.168.0.x`) にあるか確認 |
| ゲージが出ない | 電源供給不足 | 12V / 24V の電源が安定しているかチェック |
| データが途切れる | LANケーブルの品質 | シールドケーブルを使用する |

## 参考になるかもしれないページ集

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
