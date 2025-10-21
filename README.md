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

<img src="https://github.com/user-attachments/assets/a34e060d-fa74-4d5f-a937-8f664b2673a0" alt="正常に電源が入ると青ランプがつく" width="350">

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
3. 有線LAN（イーサネット）を右クリック → **プロパティ**

<img width="451" height="256" alt="ネットワーク接続" src="https://github.com/user-attachments/assets/1e1939d5-68d9-43ee-ae38-bae7af58be6a" />

4. 「インターネット プロトコル バージョン4（TCP/IPv4）」を選択し **プロパティ** をクリック  
5. 以下のように設定します：

| 項目 | 設定値 |
|------|--------|
| IPアドレス | `192.168.0.100` |
| サブネットマスク | `255.255.255.0` |
| デフォルトゲートウェイ | （空欄でOK） |

<img width="466" height="658" alt="IPv4設定" src="https://github.com/user-attachments/assets/ebe420f5-bc60-4b95-9f76-0ab8c8a00f7d" />

設定を保存し、ウィンドウを閉じます。

---

### UrgBenriPlus（北陽公式ツール）の設定

1. UrgBenriPlus を起動します。  
2. 右上の「Connectors」から **Ethernet1** を選択。  
3. 以下のように入力します：

| 項目 | 値 |
|------|----|
| IPアドレス | `192.168.0.10` |
| ポート番号 | `10940` |

4. 「▶」ボタンで接続を開始します。

<img width="973" height="626" alt="UrgBenriPlus設定" src="https://github.com/user-attachments/assets/92faf08a-b498-428b-a3d8-e09395ee630e" />

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
