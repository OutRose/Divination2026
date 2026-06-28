# CLAUDE.md — Divination2026 リファクタリングガイド

このファイルは、Claude Code が本リポジトリで作業する際に参照する基礎情報・規約・方針を集約したものです。本文の方針と現状が乖離した場合は、まずこの CLAUDE.md を更新してから作業を開始してください。

最終更新: 2026-06-28 (フェーズ β-1 完了反映)

---

## 1. プロジェクト概要

- **正体**: 8 年前（2018 年）の卒業制作的位置づけで作られた、誕生日・名前から運勢を占う Windows Forms アプリ「電脳祭2018 (DenNoSai2018) 」プロジェクトの**複製版**。オリジナルは `OutRose/Divination2018` 系統リポジトリ。
- **本リポジトリの目的**: 当時のコード資産を題材に、**学習目的で段階的にリファクタリングを行う**こと。プロダクト機能を増やすことが主目的ではない。
- **メインアプリ**: `Birthdate-Constella-Divination.exe` (Windows Forms, .NET Framework 4.8)。`FirstWindow` (入力フォーム) と `Result` (結果表示フォーム) の 2 画面構成。健康・金運・学業・恋愛・仕事・対人の 6 指標で運勢を算出し、ラッキーアイテムを提示する。
- **本リポジトリの実体規模**: 手書きコード 約 397 行、Designer 自動生成 約 604 行、resx 約 354 行、csproj 175 行。詳細は §4 を参照。
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
    "Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj" `
    /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal
  ```

---

## 4. プロジェクト構造

リポジトリには**論理的に 3 つ**のサブ単位が想定されていますが、現実には **2 つ**しか存在しません。

### 4.1 [Birthdate-Constella-Divination/](Birthdate-Constella-Divination/) — メインアプリ (現役)

WinForms 占いアプリ本体。実体のあるコード。

| ファイル | 行数 | 種別 |
|---|---:|---|
| [Program.cs](Birthdate-Constella-Divination/Program.cs) | 19 | エントリポイント |
| [FirstWindow.cs](Birthdate-Constella-Divination/FirstWindow.cs) | 51 | 入力画面ロジック (旧 Form1.cs、β-0 でリネーム) |
| [FirstWindow.Designer.cs](Birthdate-Constella-Divination/FirstWindow.Designer.cs) | 167 | 自動生成 |
| [FirstWindow.resx](Birthdate-Constella-Divination/FirstWindow.resx) | 119 | リソース (旧 Form1.resx、β-0 でリネーム) |
| [Result.cs](Birthdate-Constella-Divination/Result.cs) | 290 | 結果計算・表示ロジック (本リポジトリで最大の手書きクラス) |
| [Result.Designer.cs](Birthdate-Constella-Divination/Result.Designer.cs) | 346 | 自動生成 |
| [Result.resx](Birthdate-Constella-Divination/Result.resx) | 119 | リソース |
| [Properties/AssemblyInfo.cs](Birthdate-Constella-Divination/Properties/AssemblyInfo.cs) | 37 | アセンブリ属性 |
| [Properties/Resources.Designer.cs](Birthdate-Constella-Divination/Properties/Resources.Designer.cs) | 64 | 自動生成 |
| [Properties/Resources.resx](Birthdate-Constella-Divination/Properties/Resources.resx) | 116 | リソース |
| [Properties/Settings.Designer.cs](Birthdate-Constella-Divination/Properties/Settings.Designer.cs) | 27 | 自動生成 |
| [Properties/Settings.settings](Birthdate-Constella-Divination/Properties/Settings.settings) | 8 | 設定 |
| [Properties/app.manifest](Birthdate-Constella-Divination/Properties/app.manifest) | — | UAC マニフェスト (3723 byte) |
| [Birthdate-Constella-Divination.csproj](Birthdate-Constella-Divination/Birthdate-Constella-Divination.csproj) | 175 | プロジェクト定義 |
| [App.config](Birthdate-Constella-Divination/App.config) | 7 | アプリ構成 |
| [packages.config](Birthdate-Constella-Divination/packages.config) | 2 | NuGet (現在空) |
| [DNS2018-InternalPattern.txt](Birthdate-Constella-Divination/DNS2018-InternalPattern.txt) | — | 内部メモ (運用文書) |
| [Key-ThirdCompleted20191105.snk](Birthdate-Constella-Divination/Key-ThirdCompleted20191105.snk) | — | 強名前署名キー (現役)。α-1 で Key-First/Second と一時 pfx を削除して Third のみに集約 |

C# ルート名前空間: `BirthdateConstellaDivination` (ハイフンなし) 。

### 4.2 [BuildProcessTemplates/](BuildProcessTemplates/) — TFS XAML ビルドテンプレート (歴史的資料)

| ファイル | 行数 | 種別 |
|---|---:|---|
| [AzureContinuousDeployment.11.xaml](BuildProcessTemplates/AzureContinuousDeployment.11.xaml) | 686 | TFS 用 |
| [DefaultTemplate.11.1.xaml](BuildProcessTemplates/DefaultTemplate.11.1.xaml) | 543 | TFS 用 |
| [LabDefaultTemplate.11.xaml](BuildProcessTemplates/LabDefaultTemplate.11.xaml) | 203 | TFS 用 |
| [UpgradeTemplate.xaml](BuildProcessTemplates/UpgradeTemplate.xaml) | 76 | TFS 用 |

- 中身は Team Foundation Server 11 系のビルドプロセス XAML テンプレートのみ。`.csproj` も `.cs` もない。
- **現在のビルドフローでは未使用**。歴史的・記念的に保持されている。リファクタリング対象外。

### 4.3 DenNoSai2018Project — **削除済み (存在しない)**

- ユーザ認識上は 3 つめのサブプロジェクトとして言及されているが、**ディスク上には存在しない**。
- Git 履歴によれば: 空ファイルとして作成 (`ca10877`) → `.sln` にリネーム (`13dbc58`) → 削除 (`9ad6def`「ターゲティングパッケージ変更」コミット中で `DenNoSai2018Project.sln | 1 -` として削除) 。
- このリポジトリで参照する必要はない。CLAUDE.md からも、フェーズが進む中で言及を整理していく。

### 4.4 ルートファイル

- [Birthdate-Constella-Divination.slnx](Birthdate-Constella-Divination.slnx) — 唯一のソリューションファイル (新 XML 形式)
- [README.md](README.md) — 当時 (2018-2019) の制作者メッセージ。歴史的資料として残す

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

**現フェーズ**: β 進行中 (β-0, β-1 完了)。残りは β-2 (BuildProcessTemplates/ 削除)。

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
| 2026-06-28 | フェーズ β-1: csproj 死設定 31 項目を一括除去 (旧 Scc バインディング×4、ClickOnce 23 項目、`ManifestCertificateThumbprint`/`CodeAnalysisRuleSet`/`ToolsVersion`/`NuGetPackageImportStamp`/`TargetFrameworkProfile`/`ManifestKeyFile`)。細分化された PropertyGroup を統合し 161 行 → 115 行に圧縮。**`MSB3327` 警告が解消**し α 以降初の警告 0 ビルド達成 | (本作業) |

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
- 関連コミット: (未コミット)

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
5. **`BuildProcessTemplates/`** の扱い: 未使用 XAML 群を残すか削除するか — フェーズ β
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
