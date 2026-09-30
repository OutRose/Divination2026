# CLAUDE.md — Divination2026入口

詳細規約のcanonical sourceは`PROJECT_GUIDE.md`である。全体を先読みせず、変更対象に応じて指定節だけを読む。

## 常時規律

- 学習目的の段階的refactoringであり、挙動保存と警告0を維持する。
- code・project fileとMarkdownは`.gitattributes`どおりUTF-8（BOMなし）+ LFで保存する。
- 常時参照する指示文書は必要最小限に保ち、詳細は必要時に読む文書へ移す。
- generated Designer fileと未所有変更を不用意に書き換えず、秘密情報・build生成物をcommitしない。
- `main`へ直接pushせず作業branchを使い、force push・履歴改変・branch削除はowner確認なしで行わない。

## 条件付きrouting

- encodingやfile生成前に`PROJECT_GUIDE.md` §2、build・test変更前に§3と付録Bを読む。
- 構造・命名・C#実装変更前に該当する§4〜§6を読む。
- Git・phase・履歴・既知課題を扱う前に該当する§7〜§10と付録Aを読む。
- code変更では`dotnet test`と§3のMSBuildを実行し、docs-only変更ではlink・encoding・diffを検証する。

## Stop rules

正本文書と実装が衝突する、警告0またはtestを維持できない、未所有資産の変更が必要、または新たな設計判断が必要なら停止してpathと論点を返す。
