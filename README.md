# RemoveComponentsByRegex

## 概要

これは正規表現でマッチする、構造が同じ場所にあるコンポーネントを一括で削除するUnityエディタ拡張です。

## インストール

[このリポジトリのzipファイル](https://github.com/Taremin/RemoveComponentsByRegex/archive/master.zip)をダウンロードして、解凍したものをアセットの `Plugins` フォルダにコピーします。


## 使い方

1. ヒエラルキーで削除対象の一番根本のオブジェクトを選択
2. ヒエラルキーで右クリックしてコンテキストメニューから `Remove Components By Regex` をクリック
3. `Remove Components By Regex` ウィンドウが開くので `コンポーネント正規表現` に削除したいコンポーネントにマッチする正規表現を書く
   (例: `Dynamic Bone` と `Dynamic Bone Collider` を削除したいなら `Dynamic` など)
4. `Remove Components By Regex` ウィンドウの `Remove` ボタンを押す

削除する前にどのオブジェクトからどのコンポーネントが削除されるか知りたい場合、「削除対象をConsoleで確認(DryRun)」にチェックを入れて「Remove」をすると、実際には削除されずコンソールで削除対象が確認できます。

## ライセンス

[MIT](./LICENSE)
