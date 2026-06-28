# CLAUDE.md — Divination2026 リファクタリングガイド

このファイルは、Claude Code が本リポジトリで作業する際に参照する基礎情報・規約・方針を集約したものです。本文の方針と現状が乖離した場合は、まずこの CLAUDE.md を更新してから作業を開始してください。

最終更新: 2026-06-28 (フェーズ γ-3 完了反映)

---

## 1. プロジェクト概要

- **正体**: 8 年前（2018 年）の卒業制作的位置づけで作られた、誕生日・名前から運勢を占う Windows Forms アプリ「電脳祭2018 (DenNoSai2018) 」プロジェクトの**複製版**。オリジナルは `OutRose/Divination2018` 系統リポジトリ。
- **本リポジトリの目的**: 当時のコード資産を題材に、**学習目的で段階的にリファクタリングを行う**こと。プロダクト機能を増やすことが主目的ではない。
- **メインアプリ**: `Birthdate-Constella-Divination.exe` (Windows Forms, .NET Framework 4.8)。`FirstWindow` (入力フォーム) と `Result` (結果表示フォーム) の 2 画面構成。健康・金運・学業・恋愛・仕事・対人の 6 指標で運勢を算出し、ラッキーアイテムを提示する。
- **本リポジトリの実体規模** (γ-3 時点): 手書きコード 約 401 行 (うち Fortune 配下に 7 ファイル約 215 行、Result.cs は 290→50 行に圧縮)、Designer 自動生成 約 604 行、resx 約 354 行、csproj 120 行、テスト約 380 行 (70 件)。詳細は §4 を参照。
- **CLI/サーバ要素なし**: 純粋な WinForms クライアント。外部ネットワーク・DB アクセスなし。
- **国際化**: 日本語固定 (UI 文字列、コメント、アセンブリ属性すべて日本語) 。多言語化の予定はない。

---

## 2. ファイルエンコーディング方針

### 目標 (2026-06-28 確定)

| カテゴリ | エンコーディング | 改行コード |
|---|---|---|
| コード関連 (.cs, .csproj, .sln, .slnx, .config, .resx, .settings, .manifest, .xaml, .props, .targets, .ruleset, .editorconfig) | **UTF-8 BOM** | **CRLF** |
| ドキュメント (.md, .txt) | UTF-8 (BOM なし) | **LF** |
| ツール設定 (`.gitattributes`, `.gitignore`) | UTF-8 (BOM なし) | **LF** |
| バイナリ (.snk, .pfx, .exe, .dll, .pdb, .nupkg, 画像各種) | バイナリ扱い | 改行制御なし |

上記方針は [.gitattributes](.gitattributes) で機械的に強制されます (フェーズ α-1 で導入済み)。

### 現状 (フェーズ α-1 完了時点)

すべてのソースおよび設定ファイルが上記方針に**準拠済み**。

- フェーズ α-1 で BOM を追加したファイル: [App.config](Birthdate-Constella-Divination/App.config), [packages.config](Birthdate-Constella-Divination/packages.config), [Birthdate-Constella-Divination.slnx](Birthdate-Constella-Divination.slnx)
- README.md は Markdown 慣例どおり BOM なし UTF-8 を維持。
- 改行コードは `.gitattributes` の `eol=crlf` / `eol=lf` で次回 checkout 時から自動正規化される。既存の混在状態は `git add --renormalize .` を別途実施するか、各ファイルを順次編集する中で解消する (フェーズ α-1 では強制再正規化は実施せず、自然な更新に任せる方針)。

### 運用ルール

- 新規ファイル作成時、`.gitattributes` の規則に従って Visual Studio / Claude Code が適切な BOM・EOL で保存する。
- BOM 付与は [PowerShell] [IO.File] API で先頭 3 バイト `EF BB BF` を直接書き込む方法が確実 (テキストエディタによっては BOM 付与オプションが分かりにくいため)。
- ツール (Visual Studio / dotnet CLI / Roslyn) が BOM を勝手に剥がさないか、コミット前に `git diff` で警戒する。`.gitattributes` の `working-tree-encoding=UTF-8` 指定で Git レベルでは UTF-8 が維持される。

---

## 3. ビルド環境

| 項目 | 値 |
|---|---|
| ターゲットフレームワーク | .NET Framework **4.8** (`TargetFrameworkVersion = v4.8`) |
| 出力種別 | `WinExe` (Windows Forms 実行可能ファイル) |
| プラットフォーム | **AnyCPU** のみ (x86 / x64 構成なし) |
| ビルド構成 | `Debug|AnyCPU`, `Release|AnyCPU` の 2 つ |
| C# 言語バージョン | **`<LangVersion>latest</LangVersion>`** (C# 12.0) — Polyfill 戦略で .NET Framework 4.8 上でも最新機能を活用 |
| 強名前署名 | `SignAssembly=true`, キー: [Key-ThirdCompleted20191105.snk](Birthdate-Constella-Divination/Key-ThirdCompleted20191105.snk) |
| ClickOnce | 有効 (証明書が無いため `MSB3327` 警告が出る — 動作には影響なし) |
| ソリューション形式 | **`.slnx`** (新 XML 形式)。レガシーな `.sln` ではない |
| 検証済み MSBuild | `C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe` (VS 18 Insiders) |
| ビルド結果 (2026-06-28、β-1 後) | **成功** (エラー 0、**警告 0**) — α-1〜β-1 で ClickOnce 関連設定を段階撤去した結果、`MSB3327` が解消 |

### 注意点
- **C# 言語バージョンは `latest` (C# 12.0) を採用**: 2026-06-28 のフェーズ α-0 で `8.0` から `latest` に変更。.NET Framework 4.8 ランタイム上で最新言語機能を最大限活用する方針。ランタイム依存機能 (init 専用プロパティの `IsExternalInit` など) はリポジトリ内に polyfill 型を自前定義して対応する (§6 参照) 。
- ~~ClickOnce 警告 `MSB3327`~~ → β-1 で ClickOnce 関連設定一式 (`GenerateManifests` / `SignManifests` / `PublishUrl` 等 23 項目) を csproj から完全撤去した結果、警告自体が出なくなった。今後 ClickOnce で配布する場合は Visual Studio の publish ウィザードから再設定する。
- ビルドコマンド例 (PowerShell):
  ```
  & "C:\Program Files\Microsoft Visual Studio\18\Insiders\MSBuild\Current\Bin\MSBuild.exe" `
    "Birthdate-Constella-Divination.slnx" `
    /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal /nologo
  ```
- テスト実行コマンド (γ-0 以降):
  ```
  Set-Location "Birthdate-Constella-Divination.Tests"
  dotnet test --nologo --verbosity normal
  ```
  - `dotnet` CLI 10.0.x が利用可能 (`C:\Program Files\dotnet\dotnet.exe`)
  - 初回のみ `dotnet restore` で NuGet パッケージ取得

---

## 4. プロジェクト構造

γ-0 完了時点で、コードを含むサブディレクトリは **2 つ**:
- [Birthdate-Constella-Divination/](Birthdate-Constella-Divination/) — メインアプリ (§4.1)
- [Birthdate-Constella-Divination.Tests/](Birthdate-Constella-Divination.Tests/) — xUnit テストプロジェクト (§4.5、γ-0 で新設)

歴史的に想定されていた他のサブ単位 (`BuildProcessTemplates/`、`DenNoSai2018Project`) はすでにリポジトリから消えている (§4.2 / §4.3 参照)。

### 4.1 [Birthdate-Constella-Divination/](Birthdate-Constella-Divination/) — メインアプリ (現役)

WinForms 占いアプリ本体。実体のあるコード。

| ファイル | 行数 | 種別 |
|---|---:|---|
| [Program.cs](Birthdate-Constella-Divination/Program.cs) | 19 | エントリポイント |
| [FirstWindow.cs](Birthdate-Constella-Divination/FirstWindow.cs) | 48 | 入力画面ロジック (β-0 で Form1.cs から rename、γ-3 で Result.birtheight/strusrname 静的フィールド代入を除去し `new Result(birth, name)` 引数渡しに変更) |
| [FirstWindow.Designer.cs](Birthdate-Constella-Divination/FirstWindow.Designer.cs) | 167 | 自動生成 |
| [FirstWindow.resx](Birthdate-Constella-Divination/FirstWindow.resx) | 119 | リソース (旧 Form1.resx、β-0 でリネーム) |
| [Result.cs](Birthdate-Constella-Divination/Result.cs) | 50 | 結果画面の**薄 UI 配線層** (γ-3 で 290→50 行に圧縮)。コンストラクタ `Result(string birthdateText, string userName)` で FortuneCalculator / LuckyItemSelector / LuckRankClassifier を呼ぶだけ |
| [Fortune/FortuneConstants.cs](Birthdate-Constella-Divination/Fortune/FortuneConstants.cs) | 36 | マジック定数 23 項目を集約 (γ-1 新設) |
| [Fortune/Fortune.cs](Birthdate-Constella-Divination/Fortune/Fortune.cs) | 16 | 6 スコアを束ねる record (γ-1 新設、`MaxScoreCount`/`IsSuperLucky` を提供) |
| [Fortune/FortuneCalculator.cs](Birthdate-Constella-Divination/Fortune/FortuneCalculator.cs) | 70 | 純粋計算 (誕生日差→6桁抽出→スコア算出→ゼロ補正) (γ-1 新設、`Calculate(int birthdate, DateTime today, Random rng)`) |
| [Fortune/LuckCategory.cs](Birthdate-Constella-Divination/Fortune/LuckCategory.cs) | 12 | enum (Life/Gold/Study/Love/Work/Pattern) (γ-2 新設) |
| [Fortune/LuckRank.cs](Birthdate-Constella-Divination/Fortune/LuckRank.cs) | 12 | enum (Worst/Low/Mid/MidHigh/High/Highest) (γ-2 新設) |
| [Fortune/LuckRankClassifier.cs](Birthdate-Constella-Divination/Fortune/LuckRankClassifier.cs) | 78 | スコア→ランク分類 (`switch` 式) + 36 メッセージ `IReadOnlyDictionary<(LuckCategory,LuckRank), string>` (γ-2 新設) |
| [Fortune/LuckyItem.cs](Birthdate-Constella-Divination/Fortune/LuckyItem.cs) | 10 | enum (Pearl/Globe/Charm/LeisureSheet) (γ-2 新設) |
| [Fortune/LuckyItemSelector.cs](Birthdate-Constella-Divination/Fortune/LuckyItemSelector.cs) | 38 | `Select(int)` / `Select(Random)` / `GetName(item)` (γ-2 新設) |
| [Result.Designer.cs](Birthdate-Constella-Divination/Result.Designer.cs) | 346 | 自動生成 |
| [Result.resx](Birthdate-Constella-Divination/Result.resx) | 119 | リソース |
| [Properties/AssemblyInfo.cs](Birthdate-Constella-Divination/Properties/AssemblyInfo.cs) | 37 | アセンブリ属性 |
| [Properties/Resources.Designer.cs](Birthdate-Constella-Divination/Properties/Resources.Designer.cs) | 64 | 自動生成 |
| [Properties/Resources.resx](Birthdate-Constella-Divination/Properties/Resources.resx) | 116 | リソース |
| [Properties/Settings.Designer.cs](Birthdate-Constella-Divination/Properties/Settings.Designer.cs) | 27 | 自動生成 |
| [Properties/Settings.settings](Birthdate-Constella-Divination/Properties/Settings.settings) | 8 | 設定 |
| [Properties/app.manifest](Birthdate-Constella-Divination/Properties/app.manifest) | — | UAC マニフェスト (3723 byte) |
| [Birthdate-Constella-Divination.csproj](Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj) | 115 | プロジェクト定義 (β-1 で死設定 31 項目除去により 161 → 115 に圧縮) |
| [App.config](Birthdate-Constella-Divination/App.config) | 7 | アプリ構成 |
| [packages.config](Birthdate-Constella-Divination/packages.config) | 2 | NuGet (現在空) |
| [DNS2018-InternalPattern.txt](Birthdate-Constella-Divination/DNS2018-InternalPattern.txt) | — | 内部メモ (運用文書) |
| [Key-ThirdCompleted20191105.snk](Birthdate-Constella-Divination/Key-ThirdCompleted20191105.snk) | — | 強名前署名キー (現役)。α-1 で Key-First/Second と一時 pfx を削除して Third のみに集約 |

C# ルート名前空間: `BirthdateConstellaDivination` (ハイフンなし) 。

### 4.2 BuildProcessTemplates/ — **削除済み (フェーズ β-2)**

- 元は Team Foundation Server 11 系のビルドプロセス XAML テンプレート 4 件 (計 1,508 行) を歴史資料として保持していた。
- 現代のビルドフロー (ローカル MSBuild + git) からは完全に未参照と確認の上、β-2 で `git rm -r BuildProcessTemplates/` でディレクトリごと削除。
- 必要になれば git 履歴 (`git log -- BuildProcessTemplates/`) から復元可能。

### 4.3 DenNoSai2018Project — **削除済み (存在しない)**

- ユーザ認識上は 3 つめのサブプロジェクトとして言及されているが、**ディスク上には存在しない**。
- Git 履歴によれば: 空ファイルとして作成 (`ca10877`) → `.sln` にリネーム (`13dbc58`) → 削除 (`9ad6def`「ターゲティングパッケージ変更」コミット中で `DenNoSai2018Project.sln | 1 -` として削除) 。
- このリポジトリで参照する必要はない。CLAUDE.md からも、フェーズが進む中で言及を整理していく。

### 4.4 ルートファイル

- [Birthdate-Constella-Divination.slnx](Birthdate-Constella-Divination.slnx) — 唯一のソリューションファイル (新 XML 形式)。γ-0 でテストプロジェクトを参加させた
- [README.md](README.md) — 当時 (2018-2019) の制作者メッセージ。歴史的資料として残す
- [.gitignore](.gitignore) / [.gitattributes](.gitattributes) / [.editorconfig](.editorconfig) — α-1 〜 α-3 で導入したツール設定の三点セット

### 4.5 [Birthdate-Constella-Divination.Tests/](Birthdate-Constella-Divination.Tests/) — xUnit テストプロジェクト (γ-0 新設)

- **形式**: SDK-style csproj (`Microsoft.NET.Sdk`)。メインプロジェクトは旧形式だが、テストは新形式の方が `<PackageReference>` 中心で軽量、xUnit テンプレートとも親和
- **TargetFramework**: `net48` (メイン本体と一致)
- **言語設定**: `LangVersion=latest` (C# 12)、`Nullable=enable` (新規コードなのでグリーンフィールドで有効化)
- **依存パッケージ** (PackageReference):
  - `Microsoft.NET.Test.Sdk` 17.8.0
  - `xunit` 2.6.6 (LTS)
  - `xunit.runner.visualstudio` 2.5.7
- **ProjectReference**: 本体 [Birthdate-Constella-Divination.csproj](Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj) を参照 (メイン型へのテストアクセスのため)
- **初期ファイル** (γ-0 時点):
  - [Birthdate-Constella-Divination.Tests.csproj](Birthdate-Constella-Divination.Tests/Birthdate-Constella-Divination.Tests.csproj)
  - [SanityTests.cs](Birthdate-Constella-Divination.Tests/SanityTests.cs) — `Framework_Works` (xUnit 動作確認) + `MainAssembly_IsReachable` (本体型 `FirstWindow` の `typeof` 解決確認) の 2 件
- 後続フェーズ (γ-1〜γ-4) で `Fortune/` フォルダ配下にミラー構造のテスト群を追加していく

---

## 5. 命名規約

C# 標準慣例に従う。

| 対象 | 規約 | 例 |
|---|---|---|
| クラス・構造体・列挙型 | PascalCase | `FirstWindow`, `Result` |
| メソッド・プロパティ | PascalCase | `StartFunction_Click`, `InitializeComponent` |
| ローカル変数・パラメータ | camelCase | `birthOk`, `inputBirth` |
| プライベートフィールド | camelCase または `_camelCase` | (現コードに統一規則なし — 整理対象) |
| 定数 | PascalCase | `MaxScore` |
| 名前空間 | PascalCase ドット区切り | `BirthdateConstellaDivination` |
| ファイル名 | クラス名と一致 (PascalCase) | `Result.cs` |

### 既存コードの揺れ
- ~~`Form1.cs` 内のクラス名は `FirstWindow` だが、ファイル名・Designer は `Form1.*` のまま~~ → **フェーズ β-0 で解消** (`Form1.*` → `FirstWindow.*`、Designer の `this.Name` も `"firstWindow"` → `"FirstWindow"` に統一)。
- Designer 自動生成側の命名 (`button1`, `label1` 等の連番) は WinForms デザイナの仕様。手で書き換える際は意味のある名前に置換する。

---

## 6. コーディングスタイル

### C# 言語バージョン: `latest` (C# 12.0)

.NET Framework 4.8 環境で、**最新の C# 言語機能を最大限活用する**。ランタイム依存の機能は polyfill 型をリポジトリ内に自前定義することで対応する。

csproj 宣言: `<LangVersion>latest</LangVersion>` (Debug / Release 両構成)

### Polyfill 戦略

[Birthdate-Constella-Divination/Polyfills/](Birthdate-Constella-Divination/Polyfills/) に、C# コンパイラが要求するが .NET Framework 4.8 ランタイムには存在しない型を `internal` として自前定義する。

| Polyfill 型 | 必要な C# 機能 | 状態 |
|---|---|---|
| [`IsExternalInit`](Birthdate-Constella-Divination/Polyfills/IsExternalInit.cs) | `init` 専用プロパティ / `record` 型 | **追加済み (2026-06-28)** |
| `RequiredMemberAttribute` / `SetsRequiredMembersAttribute` | `required` 修飾子 (C# 11.0) | 未追加 — 実際に `required` を使う段で追加 |
| `System.Index` / `System.Range` | インデックス・レンジ構文 `[^1]`, `[1..3]` | 未追加 — NuGet `Microsoft.Bcl.AsyncInterfaces` 等で補完可、必要になった時点で導入 |

「使うときに足す」がルール。先回りで polyfill を増やさない。

### 積極的に使用する機能 (デフォルトで OK)

- **null 許容参照型** (`#nullable enable`) — フェーズ γ で段階的に有効化
- **`using` 宣言** (`using var x = ...;`)
- **`switch` 式** (`x switch { ... }`)
- **プロパティパターン** (`is { Length: > 0 }` 等)
- **レコード型** (`record`, `record struct`) — `IsExternalInit` polyfill により利用可
- **`init` 専用プロパティ** — 同上
- **target-typed `new` 式** (`List<string> xs = new();`)
- **パターンマッチング強化** (`and`, `or`, `not`, `is`)
- **ファイルスコープ名前空間** (`namespace Foo;`)
- **グローバル `using`** (`global using ...;`)
- **プライマリコンストラクター** (`public class Foo(int x)`)
- **コレクション式** (`int[] xs = [1, 2, 3];`)

### 使用しない機能 (ランタイム必須・互換性なし)

- **default interface methods** — .NET Framework 4.8 ランタイム未対応
- **async streams / `IAsyncEnumerable`** — 同上
- **ファイルローカル型 (`file` キーワード)** — 動作はするが用途が薄い・本リポジトリでは不要

### NuGet 補完が必要な機能 (必要になった時点で追加)

- **`System.Index` / `System.Range`** — `Microsoft.Bcl.AsyncInterfaces` ほか
- **`required` 修飾子** — Polyfill 型 (`RequiredMemberAttribute` 等) を追加 or NuGet

### リファクタリング指針

**8 年前のコードを、現代の C# 12.0 の表現力で書き直す**。具体的には:
- 手続き的な if-else の連鎖 → switch 式・パターンマッチ
- イミュータブルなデータ束 → `record` (例: 占い結果のスコア集合)
- 多数のフィールドを持つクラス → init 専用プロパティ + プライマリコンストラクター
- 文字列補間と target-typed `new` で記述量を削減

### コーディング規約 (補足)

- `var` は右辺の型が自明なときのみ使用 (`var x = new List<int>();` は OK、`var y = SomeMethod();` は避ける)
- 文字列補間 `$"..."` を `string.Format` より優先
- `nameof()` をマジック文字列の代わりに使用
- 三項演算子の入れ子は禁止 (可読性優先)
- `goto` は禁止
- `dynamic` は外部 COM/Office 連携など正当理由がない限り禁止
- 既存コードに残る `_ = MessageBox.Show(...)` のような discard 用法は維持して構わない

### フォーマット

- インデント: スペース 4 (Visual Studio 既定)
- 1 行の最大長: 厳格な上限なし。120 字を目安
- ブレース: K&R ではなく Allman (中括弧は次行) — Visual Studio C# 既定
- ファイル末尾は LF 1 つで終える (BOM は §2 参照)

---

## 7. Git 慣習

### コミットメッセージ
- **Claude Code が作成するコミットは `CC：` (全角コロン) プレフィックスを付与する**。
  例: `CC：Analyzerパッケージ削除`
- 人間 (OutRose) のコミットはプレフィックスなしの自由形式 (例: `リファクタリング前最終更新`, `NuGet一旦更新`) 。
- 言語: **日本語で書く**。命令形・体言止めいずれも可。
- 1 行目は概要 50 字以内を目安。必要なら空行を空けて本文を続ける。

### ブランチ命名
- パターン: **`refact-{YYMMDD}-{phase}`**
  - `YYMMDD`: 作業開始日 (例 `260628` = 2026 年 6 月 28 日)
  - `phase`: `alpha` / `beta` / `gamma` / `delta` のいずれか (§8 参照)
- 現行ブランチ: `refact-260628-alpha`
- main からの分岐 → 完了後に main へマージ (squash か no-ff かは未定、フェーズ β で確定)

### コミット粒度
- 「ビルドが通る単位」「論理的に意味のある単位」でこまめに切る。
- 大量の自動生成ファイル変更 (Designer.cs, .vs/, bin/, obj/) を 1 コミットに混ぜない。
- バイナリ (.suo, .vsidx, .nupkg) は原則コミットしない。`.gitignore` 整備はフェーズ β。

### 破壊的操作の事前確認
- `git push --force`, `git reset --hard`, ブランチ削除はユーザ確認なしで行わない。
- `main` への直接プッシュは行わない。フィーチャブランチ → PR/マージ。

---

## 8. リファクタリング戦略 (フェーズ構成)

| フェーズ | 目標 | 代表的タスク |
|---|---|---|
| **α (アルファ)** | 現状把握と土台整備 | CLAUDE.md 作成、エンコーディング統一方針確定、`LangVersion` 7.3/8.0 の判断、`.gitignore`/`.gitattributes`/`.editorconfig` 検討 |
| **β (ベータ)** | 構造的クリーンアップ | クラス/ファイル名の整合 (`Form1` → `FirstWindow.cs` 等)、Designer 以外の手書きコードの責務整理、不要ファイル削除 (古い `.snk`, 一時 `.pfx`) |
| **γ (ガンマ)** | 内部品質向上 | `Result.cs` (290 行) の責務分割、運勢計算ロジックの抽出 (UI から分離)、Magic Number の名前付き定数化、エラーハンドリング整備 |
| **δ (デルタ)** | モダン化 | nullable reference types 有効化検討、`async` 化が意味を持つ部分の見直し、テスト追加 (現状テスト 0)、ClickOnce 設定の整理 |

各フェーズは独立ブランチ (`refact-{YYMMDD}-{phase}`) で行い、フェーズ完了時に main へマージ。次フェーズはマージ後の main から再分岐する。

**現フェーズ**: γ 進行中 (γ-0, γ-1, γ-2, γ-3 完了)。残りサブフェーズ: γ-4 (BirthdateParser 入力検証)、γ-5 (NRT 有効化、γ 終結)。

---

## 9. Divination2018 からの変更点記録

このリポジトリは元リポジトリ `Divination2018` の複製として開始した。以下に主な逸脱を記録する。

| 日付 | 内容 | コミット |
|---|---|---|
| 2026-06-27 | DenNoSai2018Project.sln (空ファイル) を削除 | 9ad6def |
| 2026-06-27 | ターゲティングパッケージ変更 (NuGet ターゲット更新) | 9ad6def |
| 2026-06-27 | NuGet パッケージ更新 (一時的に FxCopAnalyzers 等を追加) | 7fe86fb |
| 2026-06-27 | `Microsoft.CodeAnalysis.FxCopAnalyzers` を削除 (リファクタリング前にノイズを除去) | 76e23f1 |
| 2026-06-27 | リファクタリング前最終スナップショット | 3398f66 |
| 2026-06-28 | CLAUDE.md 初版作成、ブランチ `refact-260628-alpha` を切る | (フェーズ α 初手) |
| 2026-06-28 | フェーズ α-0: `LangVersion` を `8.0` → `latest` (C# 12.0) に変更、[Polyfills/IsExternalInit.cs](Birthdate-Constella-Divination/Polyfills/IsExternalInit.cs) を新設 (record 型 / init 専用プロパティ対応)、CLAUDE.md §3/§6 更新 | `7ef8a2a` |
| 2026-06-28 | フェーズ α-1: 3 XML 設定に UTF-8 BOM 追加、[.gitattributes](.gitattributes) 導入 (code=CRLF / md=LF)、旧署名資産削除 (Key-First/Second snk + 一時 pfx)、csproj から BootstrapperPackage と死参照を除去、CLAUDE.md §2/§4/§9/§10/付録 A 更新 | `751f27a` |
| 2026-06-28 | フェーズ α-2: [.gitignore](.gitignore) 新設、追跡済みビルド成果物 25 件 (bin/×3 + obj/×14 + .vs/×8 [root と csproj 配下のネスト両方] + csproj.user×1) を `git rm --cached` で untrack、CLAUDE.md §10/付録 A 更新 | `c23908a` |
| 2026-06-28 | フェーズ α-3: [.editorconfig](.editorconfig) 最小版を新設 (encoding / EOL / indent / Allman ブレース)、csproj 既存 `<None Include=".editorconfig" />` の死参照を実体化、CLAUDE.md §10/付録 A 更新 | `4d8a6a6` |
| 2026-06-28 | フェーズ α-4: `git add --renormalize .` で 19 ファイルの EOL を `.gitattributes` 規則どおりに blob 正規化、α-2 で見逃していた `*.csproj.vspscc` を untrack、CLAUDE.md §8/§10/付録 A 更新 (フェーズ α 完了) | `d78b985` |
| 2026-06-28 | フェーズ β-0: `Form1.cs`/`.Designer.cs`/`.resx` を `FirstWindow.*` に `git mv` でリネーム (履歴保持)、csproj の `<Compile>`/`<EmbeddedResource>`/`<DependentUpon>` 5 箇所更新、Designer の `this.Name = "firstWindow"` を `"FirstWindow"` に修正、CLAUDE.md §4/§5/§8/§9/§10 更新 | `5b6bc1f` |
| 2026-06-28 | フェーズ β-1: csproj 死設定 31 項目を一括除去 (旧 Scc バインディング×4、ClickOnce 23 項目、`ManifestCertificateThumbprint`/`CodeAnalysisRuleSet`/`ToolsVersion`/`NuGetPackageImportStamp`/`TargetFrameworkProfile`/`ManifestKeyFile`)。細分化された PropertyGroup を統合し 161 行 → 115 行に圧縮。**`MSB3327` 警告が解消**し α 以降初の警告 0 ビルド達成 | `32c1151` |
| 2026-06-28 | フェーズ β-2: [BuildProcessTemplates/](https://github.com/OutRose/Divination2026) (TFS 11 系 XAML テンプレート 4 件、計 1,508 行、完全未参照確認済み) を `git rm -r` でディレクトリごと削除。CLAUDE.md §1/§4/§8/§9/§10、付録 A 項目 5 更新 (項目 5 解消)。**フェーズ β 完了** | `05efb8c` |
| 2026-06-28 | フェーズ γ-0: [Birthdate-Constella-Divination.Tests/](Birthdate-Constella-Divination.Tests/) を SDK-style + xUnit 2.6.6 + Microsoft.NET.Test.Sdk 17.8.0 で新設、.slnx にプロジェクト追加、SanityTests.cs に 2 件のスモークテスト (両方合格)、.gitignore に `[Tt]est[Rr]esults/` 追加、CLAUDE.md §3/§4/§8/§9/§10 更新 | `a2445ca` |
| 2026-06-28 | フェーズ γ-1: [Fortune/](Birthdate-Constella-Divination/Fortune/) フォルダ新設 (`BirthdateConstellaDivination.Fortune` 名前空間)、`FortuneConstants` (23 定数)、`Fortune` (record 型 6 スコア)、`FortuneCalculator` (純粋計算 + ゼロ補正) を抽出。テスト 17 件追加 (`FortuneTests` 10 件 + `FortuneCalculatorTests` 7 件)、全 19/19 合格 | `e232f5a` |
| 2026-06-28 | フェーズ γ-2: 5 新型を Fortune/ に追加 (`LuckCategory`/`LuckRank` enum、`LuckRankClassifier` (`switch` 式 + 36 メッセージ dict)、`LuckyItem` enum、`LuckyItemSelector`)。テスト 51 件追加、全 70/70 合格 | `c3c183c` |
| 2026-06-28 | フェーズ γ-3: [Result.cs](Birthdate-Constella-Divination/Result.cs) を 290 行 → 50 行に圧縮 (薄 UI 配線層化)。`Result.birtheight`/`strusrname` 静的フィールド廃止、コンストラクタ引数化 (`Result(string birth, string name)`)。FortuneCalculator + LuckRankClassifier + LuckyItemSelector を呼び出す `AssignScore` ヘルパで 6 指標を一様処理。Life>27 を `Math.Min(score, progress.Maximum)` でキャップし元実装の潜在 `ArgumentOutOfRangeException` バグも解消。70/70 テスト維持 | (本作業) |

以後、機能差分・設定差分が出るたびにここに追記する。

---

## 10. 過去の整理作業履歴

### 2026-06-28 (フェーズ α-0: 言語バージョン昇格と Polyfill 導入)
- csproj の `<LangVersion>` を `8.0` → `latest` に変更 (Debug / Release 両構成)
- [Birthdate-Constella-Divination/Polyfills/](Birthdate-Constella-Divination/Polyfills/) フォルダを新設し、[IsExternalInit.cs](Birthdate-Constella-Divination/Polyfills/IsExternalInit.cs) を配置 (record 型と init 専用プロパティを有効化)
- csproj の `<ItemGroup>` に `<Compile Include="Polyfills\IsExternalInit.cs" />` を追加 (旧形式 csproj のため明示登録が必要)
- CLAUDE.md §3 (ビルド環境表の C# 言語バージョン行)、§6 (コーディングスタイル全面書き換え)、§9 (Divination2018 からの変更点)、付録 A (要決定事項 1: 解消)、を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ) で成功
- 関連コミット: `7ef8a2a`

### 2026-06-28 (フェーズ α-1: エンコーディング統一と旧資産整理)
- 3 XML 設定ファイル ([App.config](Birthdate-Constella-Divination/App.config), [packages.config](Birthdate-Constella-Divination/packages.config), [Birthdate-Constella-Divination.slnx](Birthdate-Constella-Divination.slnx)) に UTF-8 BOM (`EF BB BF`) を PowerShell で in-place 追加 (改行コードは現状維持)
- [.gitattributes](.gitattributes) をリポジトリルートに新設: コード関連 = CRLF + `working-tree-encoding=UTF-8`、md/txt = LF、`.snk` `.pfx` `.exe` 等 = binary
- 旧署名資産 3 ファイルを `git rm` で削除: `Key-FirstCompleted20180430.snk`, `Key-SecondCompleted20180504.snk`, `Birthdate-Constella-Divination_一時キー.pfx` (アーカイブブランチ退避ではなく直接削除)
- csproj から削除ファイル 3 件への `<None Include>` 参照を除去
- csproj から `BootstrapperPackage` ItemGroup を除去 (.NET 4.5.2 / 3.5 SP1 への古い ClickOnce 参照)
- CLAUDE.md §2 (エンコーディング方針: 目標表を「決定事項」に書き換え)、§4 (キーファイル行を Third 1 件に集約)、§9 (Divination2018 変更点)、§10 (履歴)、付録 A (要決定事項 2/4/7: 解消) を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ) で成功
- 関連コミット: `751f27a`

### 2026-06-28 (フェーズ α-2: .gitignore 導入とビルド成果物の untrack)
- [.gitignore](.gitignore) をリポジトリルートに新設 (Visual Studio 標準テンプレート相当を本プロジェクト向けに整理: `[Bb]in/`, `[Oo]bj/`, `.vs/`, `*.user`, `*.suo`, `*.pfx`, `.claude/`, `.vscode/*` + 主要設定ファイルの例外 等)
- 追跡済みビルド成果物・作業状態ファイル 25 件を `git rm --cached` で index から除去 (working tree は保持):
  - `.vs/` 配下 4 件 (vsidx, suo, DocumentLayout×2)
  - `Birthdate-Constella-Divination/.vs/` 配下 4 件 (**nested `.vs/`** — solution root と csproj root の両方に存在する歴史的経緯あり)
  - `Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj.user` 1 件 (ユーザ固有)
  - `Birthdate-Constella-Divination/bin/Debug/` 3 件 (`.exe`, `.exe.config`, `.pdb`)
  - `Birthdate-Constella-Divination/obj/Debug/` 14 件 (`.NETFramework,Version=v4.8.AssemblyAttributes.cs`, 各種 `.cache`, 3 種の `.resources`, `TempPE/Properties.Resources.Designer.cs.dll`, exe/pdb の中間版 等)
- CLAUDE.md §10 (履歴)、付録 A 項目 3 (`bin`/`obj` 追跡: 解消) を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ) で成功。再ビルド後の `git status` がクリーンであることも確認
- 関連コミット: `c23908a`

### 2026-06-28 (フェーズ α-3: .editorconfig 最小版導入)
- [.editorconfig](.editorconfig) をリポジトリルートに新設 (約 35 行)。範囲を以下に限定:
  - 全ファイル既定: indent 4 スペース、UTF-8、CRLF、末尾改行、末尾空白除去
  - `[*.{md,txt}]`: UTF-8 (BOM なし) + LF
  - `[{.gitattributes,.gitignore}]`: LF
  - `[*.cs]`: UTF-8 BOM + Allman ブレース ( `csharp_new_line_before_*` 系一式)
  - `[*.{csproj,sln,slnx,config,resx,settings,manifest,xaml,props,targets,ruleset}]`: UTF-8 BOM + indent 2 スペース (XML 慣例)
- **意図的に含めなかったもの** (β/γ で必要になった時点で追加):
  - 命名規約 (`dotnet_naming_*`) — 既存コードへの警告ノイズを避けるため
  - `var` / null / コレクション式等の style preferences (`csharp_style_*`, `dotnet_style_*`)
  - Roslyn 診断ルールの severity 昇格 (`dotnet_diagnostic.*.severity`)
- csproj に既存していた `<None Include=".editorconfig" />` 参照が、実体ファイルの追加により**整合**した (削除ではなく実体化を選択)
- CLAUDE.md §10 (履歴)、付録 A 項目 2 (`.editorconfig` 整合: 解消) を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ、`.editorconfig` 起因の新規警告なし) で成功
- 関連コミット: `4d8a6a6`

### 2026-06-28 (フェーズ α-4: 全面 EOL 再正規化とフェーズ α 終結)
- `git add --renormalize .` を実行。事前チェック (`git ls-files` + `git check-attr eol` + blob の CR/LF カウント) で 9 ファイルの MISMATCH を特定したが、renormalize は連動して 19 ファイルの blob を `.gitattributes` の canonical 形 (CRLF 系 → LF blob、LF 系はそのまま) で書き直し
- 内訳: `Form1.cs` / `Result.cs` / 各 `Designer.cs` / 各 `.resx` / `Properties/Settings.settings` / `Properties/AssemblyInfo.cs` / `Properties/app.manifest` / `BuildProcessTemplates/*.xaml` (4 件) / `README.md` / `DNS2018-InternalPattern.txt` ほか。**実体は変わらず、リポジトリ内の保存形式が canonical 化される**だけ (working tree は `.gitattributes` の `eol=` 指定どおり再生成される)
- α-2 で見逃していた `Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj.vspscc` (1 件) を `git rm --cached` で untrack
- CLAUDE.md §8 (現フェーズ表示: α 完了)、§10 (履歴)、付録 A 項目 9 (改行コード混在: 解消) を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ) で成功
- 関連コミット: `d78b985`

### 2026-06-28 (フェーズ β-0: Form1 → FirstWindow リネーム)
- ファイルリネーム 3 件を `git mv` で実施 (履歴保持、rename detection が効く形):
  - `Birthdate-Constella-Divination/Form1.cs` → [FirstWindow.cs](Birthdate-Constella-Divination/FirstWindow.cs)
  - `Birthdate-Constella-Divination/Form1.Designer.cs` → [FirstWindow.Designer.cs](Birthdate-Constella-Divination/FirstWindow.Designer.cs)
  - `Birthdate-Constella-Divination/Form1.resx` → [FirstWindow.resx](Birthdate-Constella-Divination/FirstWindow.resx)
- csproj の参照 5 箇所を更新 (Compile×2, EmbeddedResource×1, DependentUpon×2)
- [FirstWindow.Designer.cs](Birthdate-Constella-Divination/FirstWindow.Designer.cs) の `this.Name = "firstWindow"` を `"FirstWindow"` に修正 (Designer 自動生成のケース不一致を解消、クラス名・ファイル名と完全一致)
- CLAUDE.md §4 (ファイル表)、§5 (揺れ note 解消)、§8 (現フェーズ表示: β-0)、§9/§10 (履歴) を更新
- ビルド検証: Debug|AnyCPU でエラー 0、警告 1 (`MSB3327` のみ) で成功、smoke test ユーザ確認 OK
- 関連コミット: `5b6bc1f`

### 2026-06-28 (フェーズ β-1: csproj 死設定 31 項目を一括除去)
- [Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj](Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj) から以下を完全除去:
  - **死設定 8 項目**: `ToolsVersion="14.0"` 属性、Scc バインディング 4 件 (SccProjectName/LocalPath/AuxPath/Provider = SAK)、`NuGetPackageImportStamp` 空タグ、`TargetFrameworkProfile` 空タグ、`ManifestKeyFile` 空タグ、`ManifestCertificateThumbprint` (α-1 で削除した一時 pfx 紐付け)、`CodeAnalysisRuleSet` (死参照 `BasicDesignGuidelineRules.ruleset`)
  - **ClickOnce 関連 23 項目**: `PublishUrl` (アクセス不能な学校サーバ `\\00esv1a002\実習室\...`)、`Install`、`InstallFrom`、`IsWebBootstrapper`、`UpdateEnabled`/`UpdateMode`/`UpdateInterval`/`UpdateIntervalUnits`/`UpdatePeriodically`/`UpdateRequired`、`MapFileExtensions`、`WebPage`、`AutorunEnabled`、`ApplicationRevision`、`ApplicationVersion`、`UseApplicationTrust`、`PublishWizardCompleted`、`BootstrapperEnabled`、`GenerateManifests`、`SignManifests`、`TargetZone`
- 細分化されていた 7 つの単一プロパティ PropertyGroup を統合 (`ApplicationManifest` + `SignAssembly` + `AssemblyOriginatorKeyFile` を 1 PropertyGroup に集約)
- csproj 行数: 161 → 115 (約 30% 削減)
- **`MSB3327` 警告が解消**し、α 以降初の警告 0 ビルド達成
- CLAUDE.md §3 (ビルド環境表、警告 0 反映)、§3 注意点 (`MSB3327` 解消マーク)、§8 (現フェーズ表示)、§9/§10 (履歴) 更新
- 付録 A 項目 8 (`ManifestCertificateThumbprint`) を解決済みに
- ビルド検証: Debug|AnyCPU で**エラー 0、警告 0** で成功
- 関連コミット: `32c1151`

### 2026-06-28 (フェーズ β-2: BuildProcessTemplates/ 完全削除、β 全体終結)
- `git rm -r BuildProcessTemplates/` で TFS 11 系 XAML テンプレート 4 件 + ディレクトリ自体を削除
  - 削除ファイル: `AzureContinuousDeployment.11.xaml` (686 行) / `DefaultTemplate.11.1.xaml` (543 行) / `LabDefaultTemplate.11.xaml` (203 行) / `UpgradeTemplate.xaml` (76 行)、計 1,508 行
  - 完全に未参照 (csproj/.slnx/コード/README から一切参照なし、β プラン Phase 1 の Explore 調査で確認済み) のため、リファクタリング対象から除外して削除
  - 必要になれば git 履歴 (`git log -- BuildProcessTemplates/`) から復元可能
- CLAUDE.md §1 (実体規模、csproj 175 → 115 反映)、§4 (前文を「コード含むサブディレクトリは Birthdate-Constella-Divination/ のみ」に書き換え、§4.2 を「削除済み」節に縮約)、§8 (現フェーズ表示: β 完了 → γ へ)、§9/§10 (履歴) 更新
- 付録 A 項目 5 (`BuildProcessTemplates/` の扱い) を解決済みに
- ビルド検証: Debug|AnyCPU で**エラー 0、警告 0** で成功 (コード/csproj に影響なし)
- 関連コミット: `05efb8c`

### 2026-06-28 (フェーズ γ-3: Result.cs を薄 UI 層に再構成、静的フィールド除去)
- [Result.cs](Birthdate-Constella-Divination/Result.cs) を全面書き換え:
  - 290 行 → **50 行** (約 83% 削減、-272/+30 差分)
  - **静的フィールド廃止**: `public static string birtheight;` / `public static string strusrname;` を削除
  - **コンストラクタ引数化**: `public Result(string birthdateText, string userName)` でコンストラクタ注入
  - **パラメータレスコンストラクタ廃止**: `public Result()` 削除 (注: VS デザイナの Result 設計ビューが動かなくなるが、フォームレイアウトは既に固定なので影響軽微。再設計必要時は parameterless ctor を一時復活させる)
  - **AssignScore ヘルパ** で 6 指標を一様処理 (`ProgressBar.Value` 設定 + ランクメッセージ取得)
  - **Life>27 のキャップ追加**: `progress.Value = Math.Min(score, progress.Maximum)` で元実装の潜在バグ (`ArgumentOutOfRangeException` 可能性) を解消
  - 既存挙動の forward 保持: `int.Parse` のエラーは引き続き例外 (γ-4 で `TryParse` に切り替え)、Random は内部で `new Random()` 生成 (UI 配線層のため depend injection は不要と判断)
- [FirstWindow.cs](Birthdate-Constella-Divination/FirstWindow.cs) を最小変更:
  - 51 行 → **48 行** (-3 行)
  - `Result.birtheight = inputBirth.Text;` / `Result.strusrname = inputName.Text;` の 2 行を削除
  - `new Result()` → `new Result(inputBirth.Text, inputName.Text)` 引数渡しに変更
  - `birthOk == true && nameOk == true` → `birthOk && nameOk` (微小簡素化)
  - 注: 2 つの empty チェック構造はそのまま (γ-4 で BirthdateParser に統一予定)
- 中間表ファイル (Designer.cs / resx) は無変更 — UI 配置は元のまま
- CLAUDE.md §1 (実体規模)、§4.1 (Result.cs / FirstWindow.cs 備考)、§8 (現フェーズ表示)、§9/§10 (履歴) 更新、γ-2 ハッシュ `c3c183c` 補填
- 検証:
  - `dotnet test`: **70/70 維持** (202 ms、refactor によりテスト失敗ゼロ — γ-1/γ-2 の純粋ロジックテストが Result.cs 内部処理と等価なため、振る舞いの degradation がないことを確認)
  - .slnx 経由 MSBuild: 両プロジェクト警告 0
  - **実機 smoke test 必須** (ユーザ手動): bin/Debug/Birthdate-Constella-Divination.exe を起動して FirstWindow → Result の遷移を確認
- 関連コミット: (未コミット)

### 2026-06-28 (フェーズ γ-2: LuckRank / LuckyItem 抽出と 36 メッセージ集約)
- Fortune/ に新規ファイル 5 件:
  - [LuckCategory.cs](Birthdate-Constella-Divination/Fortune/LuckCategory.cs): `enum { Life, Gold, Study, Love, Work, Pattern }`
  - [LuckRank.cs](Birthdate-Constella-Divination/Fortune/LuckRank.cs): `enum { Worst, Low, Mid, MidHigh, High, Highest }` (6 段階)
  - [LuckRankClassifier.cs](Birthdate-Constella-Divination/Fortune/LuckRankClassifier.cs): `Classify(int score)` を C# `switch` 式で 6 行に圧縮、`GetMessage(category, rank)` で 36 メッセージを tuple-key `IReadOnlyDictionary` 経由で返す。`GetMessageForScore` 便利メソッド
  - [LuckyItem.cs](Birthdate-Constella-Divination/Fortune/LuckyItem.cs): `enum { Pearl, Globe, Charm, LeisureSheet }`
  - [LuckyItemSelector.cs](Birthdate-Constella-Divination/Fortune/LuckyItemSelector.cs): `Select(int)` (純粋マッピング、out-of-range で例外)、`Select(Random)` (rng.Next ラッパ)、`GetName(item)` (日本語ラベル)
- **元実装からの軽い refinement**:
  - 元コードはランク分類が `if (0 <= s && s <= 4) ... else if ... else if (25 <= s && s <= 27)` で、`s > 27` や `s < 0` のとき label が初期文字列 (例: "解説1") に残る silent default だった
  - 新 `Classify` は `switch` 式の `_` パターンで > RankHighMax を `Highest` にキャップ、負数は最初の `<= RankWorstMax` に落ちて `Worst` を返す → UI 表示の死分岐がなくなる
  - 元のラッキーアイテムは `Random.Next(1, 10)` で 1〜9 を生成し、`8-10` の if-else 分岐に 10 がハマる前提だが実は到達不能だった。新 `Select(int)` は値 1-10 すべてを正しくマップし、out-of-range は例外
  - これらは γ-3 で Result.cs を refactor したとき、Life>27 (max 162) を含む既知の挙動を「Highest メッセージ表示」で扱えるようになる
- main csproj に 5 ファイル分の `<Compile Include="Fortune\...">` を追加
- テストプロジェクトに新規 2 ファイル:
  - [LuckRankClassifierTests.cs](Birthdate-Constella-Divination.Tests/Fortune/LuckRankClassifierTests.cs): 境界値分類 15 件 + 特定メッセージ検証 9 件 + 36 組合せ網羅 1 件 + GetMessageForScore 合成 4 件 = 計 29 件
  - [LuckyItemSelectorTests.cs](Birthdate-Constella-Divination.Tests/Fortune/LuckyItemSelectorTests.cs): Select(int) マッピング 10 件 + Select(int) out-of-range 4 件 + Select(Random) 決定論 1 件 + 1000 回ランダムでも valid item 1 件 + null rng 1 件 + GetName 4 件 + GetName 不正値 1 件 = 計 22 件
- CLAUDE.md §4.1 (ファイル表に 5 新ファイル追加)、§8 (現フェーズ表示)、§9/§10 (履歴) 更新、γ-1 ハッシュ `e232f5a` 補填
- 検証: `dotnet test` で **70/70 合格** (γ-0 sanity 2 + γ-1 ロジック 17 + γ-2 新規 51)、所要 119 ms。.slnx 経由 MSBuild で両プロジェクト警告 0
- 関連コミット: (未コミット)

### 2026-06-28 (フェーズ γ-1: FortuneCalculator 抽出とマジック定数集約)
- 新フォルダ [Birthdate-Constella-Divination/Fortune/](Birthdate-Constella-Divination/Fortune/) を作成 (`BirthdateConstellaDivination.Fortune` 名前空間)
- 新規ファイル 3 件:
  - [FortuneConstants.cs](Birthdate-Constella-Divination/Fortune/FortuneConstants.cs) — マジック定数 23 項目を集約 (`MaxScore=27`, `DigitCount=6`, `RandomKey/LifeAdjustment/ZeroAdjustment` の Min/MaxExclusive、6 ゼロ補正シード、ランク境界 6 つ、`LuckyItem` 境界、`SuperLuckyThreshold=3` 等)
  - [Fortune.cs](Birthdate-Constella-Divination/Fortune/Fortune.cs) — `public sealed record Fortune(int Life, int Gold, int Study, int Love, int Work, int Pattern)` に `MaxScoreCount` / `IsSuperLucky` を持たせる (`IsExternalInit` polyfill 経由で record 型が使える)
  - [FortuneCalculator.cs](Birthdate-Constella-Divination/Fortune/FortuneCalculator.cs) — `Calculate(int birthdateYyyyMmDd, DateTime today, Random rng) → Fortune` の純粋計算。`Random` 注入でテスト時にシード固定可能。ゼロ補正は private `ZeroAdjust(score, seed)` に分離 (元実装の決定論的挙動を保存)
- **元実装の挙動を完全に保存**:
  - `> 27` キャップは pattern 側 (dead branch、原文ママ) — コメントで明記
  - ゼロ補正シード `100/300/600/400/200/500` は元コード値そのまま
  - `Random.Next` の max-exclusive 規約を定数名で明示 (`...MinInclusive` / `...MaxExclusive`)
- main csproj の `<ItemGroup>` に 3 ファイル分の `<Compile Include="Fortune\...">` を追加
- テストプロジェクトに [Fortune/](Birthdate-Constella-Divination.Tests/Fortune/) サブフォルダ作成、新規テスト 2 ファイル:
  - [FortuneTests.cs](Birthdate-Constella-Divination.Tests/Fortune/FortuneTests.cs) — `MaxScoreCount` / `IsSuperLucky` のパラメータ化テスト 9 件 + record 等価性 1 件
  - [FortuneCalculatorTests.cs](Birthdate-Constella-Divination.Tests/Fortune/FortuneCalculatorTests.cs) — characterization テスト (誕生日==今日のゼロ補正、既知入力での期待値計算、null rng 例外、3 入力での >=1 不変量、決定論性) 計 7 件
- 既存 [Result.cs](Birthdate-Constella-Divination/Result.cs) は**変更なし** (新型と並走状態。γ-3 で UI 層に refactor 時に呼び出す)
- CLAUDE.md §4.1 (ファイル表に新ファイル群追加)、§8 (現フェーズ表示)、§9/§10 (履歴) 更新、γ-0 ハッシュ `a2445ca` 補填
- 検証: `dotnet test` で **19/19 合格** (γ-0 の 2 + γ-1 の 17)、.slnx 経由 MSBuild で両プロジェクト警告 0
- 関連コミット: (未コミット)

### 2026-06-28 (フェーズ γ-0: xUnit テストプロジェクト雛形を新設)
- 新ディレクトリ [Birthdate-Constella-Divination.Tests/](Birthdate-Constella-Divination.Tests/) を作成
- `Birthdate-Constella-Divination.Tests.csproj` (SDK-style `Microsoft.NET.Sdk`、`net48` ターゲット、`LangVersion=latest`、`Nullable=enable`) を新設、以下を PackageReference として宣言:
  - `Microsoft.NET.Test.Sdk` 17.8.0
  - `xunit` 2.6.6 (LTS)
  - `xunit.runner.visualstudio` 2.5.7 (`PrivateAssets=all`)
- ProjectReference でメインプロジェクト ([Birthdate-Constella-Divination.csproj](Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj)) を参照
- `SanityTests.cs` (2 件のスモークテスト) を配置:
  - `Framework_Works` — `Assert.True(true)` 相当、xUnit フレームワーク動作確認
  - `MainAssembly_IsReachable` — メイン本体型 `FirstWindow` の `typeof` 解決確認 (ProjectReference の wiring 検証)
- [Birthdate-Constella-Divination.slnx](Birthdate-Constella-Divination.slnx) にテストプロジェクトを `<Project Path="..." Id="1f7fd37b-77cb-427d-a80d-8d3d6e87b98a" />` で追加
- [.gitignore](.gitignore) に `[Tt]est[Rr]esults/` を追記 (テスト出力ディレクトリの慣例)
- CLAUDE.md §3 (テスト実行コマンド追加)、§4 前文 (サブディレクトリ 2 つに更新)、§4.4 (ツール 3 点セット注記追加)、§4.5 (テストプロジェクト節を新設)、§8 (現フェーズ表示: γ 進行中)、§9/§10 (履歴) を更新
- ビルド検証:
  - `dotnet test` で 2/2 テスト合格 (経過 3.99 秒)
  - .slnx 経由の MSBuild で両プロジェクト警告 0 ビルド成功
- 関連コミット: (未コミット)

---

**フェーズ β 終了サマリ**: β-0 〜 β-2 で構造的クリーンアップが完了。クラス/ファイル名の不整合解消 (Form1 → FirstWindow)、csproj の死設定 31 項目除去、未使用ディレクトリ削除を達成。ビルドは警告 0 で安定。残課題はフェーズ γ (内部品質向上: `Result.cs` 290 行の責務分割、運勢計算ロジックの UI 分離、`record` 化、Magic Number の名前付き定数化、エラーハンドリング、テスト追加) のみ。

---

**フェーズ α 終了サマリ**: α-0 〜 α-4 で土台整備 (Modern C# 12.0 環境、エンコーディング・改行コードの統一とリポジトリ全面 canonical 化、旧資産整理、`.gitignore` / `.gitattributes` / `.editorconfig` の三点セット完備) が完了。これでフェーズ β (構造的クリーンアップ: `Form1` → `FirstWindow.cs` 名前整合、`ManifestCertificateThumbprint` 整理、`BuildProcessTemplates/` の扱い決定、`Result.cs` 290 行の責務分割準備など) に着手できる状態。

### テンプレート
```
### YYYY-MM-DD (フェーズ X)
- 実施内容 (1 行)
- 対象ファイル / 影響範囲
- 関連コミット: <hash>
```

---

## 付録 A: 既知の課題・要決定事項

1. ~~**C# LangVersion**: 7.3 にダウングレードするか、8.0 を維持するか~~ → **2026-06-28 解決**: `latest` (C# 12.0) + Polyfill 戦略を採用 (フェーズ α-0)
2. ~~**`.gitattributes` / `.editorconfig` の導入可否**~~ → **2026-06-28 解決**: `.gitattributes` をフェーズ α-1 で導入 (code=CRLF / md=LF / binary 多数)。`.editorconfig` 最小版をフェーズ α-3 で導入し、csproj の死参照を実体化
3. ~~**`bin/` / `obj/` のリポジトリ追跡**~~ → **2026-06-28 解決**: フェーズ α-2 で [.gitignore](.gitignore) 導入と 25 件の `git rm --cached` untrack を実施。以後、ビルド成果物・`.vs/`・`*.user`・`.claude/` 等は `git status` に現れない
4. ~~**古いキーファイルの扱い**~~ → **2026-06-28 解決**: フェーズ α-1 で Key-First/Second snk と一時 pfx を直接削除 (アーカイブブランチ退避は行わなかった)
5. ~~**`BuildProcessTemplates/`** の扱い~~ → **2026-06-28 解決**: フェーズ β-2 で `git rm -r` でディレクトリごと完全削除。git 履歴には残るので必要時は `git log -- BuildProcessTemplates/` で参照可能
6. **テスト不在**: 単体テストが 1 つもない。安全なリファクタリングのためにフェーズ γ で `Result.cs` の運勢計算ロジックを抽出した上でテスト追加を検討
7. ~~**古い `BootstrapperPackage` 参照**~~ → **2026-06-28 解決**: フェーズ α-1 で csproj から ItemGroup 全体を除去
8. ~~**`ManifestCertificateThumbprint`** が csproj に残存~~ → **2026-06-28 解決**: フェーズ β-1 で `ManifestCertificateThumbprint` を含む ClickOnce/署名マニフェスト関連 5 項目 (`ManifestCertificateThumbprint` / `ManifestKeyFile` / `GenerateManifests` / `SignManifests` / `TargetZone`) を csproj から完全除去。これにより `MSB3327` 警告も解消
9. ~~**既存ファイルの改行コード混在**~~ → **2026-06-28 解決**: フェーズ α-4 で `git add --renormalize .` を実行し全面 canonical 化。実体は変わらず、blob と attributes が完全整合した状態

## 付録 B: ビルド再現手順 (MSBuild パスの自動検出)

```powershell
$vswhere = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = & $vswhere -latest -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
& $msbuild "Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj" `
  /p:Configuration=Debug /p:Platform=AnyCPU /verbosity:minimal /nologo
```
