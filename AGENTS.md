## プロジェクトについて

- パズルとローグライクを合わせたゲーム
- 詳しい仕様は仕様書についてを参照
- ゲームを作りながら，Unityの操作を覚え，C#を使えるようにし，ゲームを開発することを目的としている
- あなたの役割は操作方法や考え方を教えることである
- ドキュメントなどを参照し，引用を示しながら説明して
- プロンプト内で「実装して」と指示があった時のみ実装をする

## git

- ブランチの切り方，コミットメッセージなどはREADME.mdを参照する
- 名前はKaito
- prefixは適宜判断して
- mainブランチでの作業は行わず，ブランチを切ってから作業することを徹底する
- やりたいことを受け取った際はその内容に応じたブランチを切ること
- remoteはSSH
- 新しい要素を実装するプロンプトが入力されたとき，それまでの作業内容を確認してコミットをする
- 成果物の修正の場合はまだコミットしない

## 導入済みMCP

- Unity MCP, Google Drive, Githubを導入済み
- 必要に応じてツールを使って

## Unity MCPについて

- Unity MCPは `isuzu-unity-mcp` v3 を使用する。Unity MCPを使う作業では、まず Codex Skill `isuzu-unity-mcp` を利用すること。
- Unity Editor、Scene、GameObject、Component、Build Settings、Play Modeなど、Unityを操作する場合はUnity MCPを利用したときのみに制限する。
- Unity MCPを利用できない状態で、Unityのバッチモード、Scene YAMLの直接編集、別のGUI操作などへ迂回してUnityを操作しない。
- Unity MCPに接続できない場合は実装を進めず、原因を調査してUnity MCPへ接続できる状態に戻すことを第一目標とする。
- 接続確認では、`isuzu-unity-mcp projects`、`isuzu-unity-mcp health`、`isuzu-unity-mcp tools` を使い、対象Unityプロジェクトが検出できることと接続状態がhealthyであることを確認する。
- 接続できない場合は、UnityとUnity MCPの起動状態、descriptor、認証token、残留しているUnity MCPのNodeプロセス、ポート競合を調査する。残留プロセスを終了する場合は、対象をpuzlog用Unity MCPに限定し、事前に許可を得る。
- Unity MCPプロセスを終了すると現在のMCP接続も切れるため、終了後はCodexとUnityを再起動してから再接続する。

## Unity MCPでの実装・検証

- C#スニペットを実行する場合は `execute_code` を使い、大きな処理を一度に渡さず、Scene作成、UI配置、参照設定、保存などの小さな単位に分割して実行する。
- `execute_code` は原則として `isuzu-unity-mcp call execute_code --file <snippet.cs>` のようにファイル経由で渡し、シェルやJSONエンコードによる文字列破損を避ける。
- Unityを変更する前に、Edit ModeかPlay Modeか、現在開いているSceneが対象Sceneかを確認する。対象Sceneは明示的に開いてから変更する。
- Sceneを変更した後は、`MarkSceneDirty`と`SaveScene`を明示的に実行し、保存後にSceneファイルまたはHierarchyで対象要素が存在することを確認する。
- Play Mode中にSceneやSerializedObjectを保存しない。Play Modeを終了し、Edit Modeへ戻ったことを確認してから変更・保存する。
- 動作確認前にConsoleをクリアし、確認後にエラー数と警告数を確認する。ログ確認は `console_read_logs`、件数確認は `console_get_count`、必要に応じて `editor_log_tail` を使用する。
- `ScreenCapture.CaptureScreenshot`は非同期なので、必要に応じてPlayer Loop更新とView再描画を行い、ファイルの生成を確認してから画像を読む。
- TextMeshProの日本語は既存の日本語フォントアセットを利用し、必要な字形を`TryAddCharacters`で追加する。実行中に新しい動的フォントアセットを生成しない。

## 仕様書について

- 仕様書はGoogle Driveの共有アイテム内にある。
- 仕様書の場所は `共有アイテム/crumpdott/パズログ/仕様書`。
- 実装・設計・用語確認を行う前に、まずGoogle Driveで `crumpdott` → `パズログ` → `仕様書` を探して読むこと。
- 仕様書ドキュメント名は `仕様書`。
- 仕様書は以下のURL
  - https://docs.google.com/document/d/13Qp8JkCf4r218XS7efw5sgBNLFtF9ZmLr-DhBtmc9sU
- アーティファクト一覧は以下のURL
  - https://docs.google.com/spreadsheets/d/1Yn3C11AORIbSthBGBoge3ShjOCoc-tTrD3-zM7YXGRM/
- ゲーム内の専門用語、アーティファクト、エレメント、特殊効果、お金、ショップに関する判断は、まず仕様書の記述を確認すること。
- 仕様書に記述がある場合は、その内容を引用して説明すること。
- 仕様書に記述がない、または曖昧な場合は、勝手に決めずに質問して仕様を改良すること。
- 質問の回答があった場合は，仕様書の該当箇所にコメントとして残せるか試すこと．いずれ正式な仕様として本文に反映する時に参照するためである．
- Google Drive API経由のコメントが本文に正しく紐づかない，または削除済みコンテンツ扱いになる場合は，無理に本文へメモを追記せず，コメント作成ができなかったことと手動で該当箇所にコメントしてほしいことを伝えること．
- 実装したいものがあった時には、それについての記述がないかまず確認すること。
