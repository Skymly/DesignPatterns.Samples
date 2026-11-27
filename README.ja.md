# DesignPatterns.Samples

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語**

[![CI](https://github.com/Skymly/DesignPatterns.Samples/actions/workflows/ci.yml/badge.svg)](https://github.com/Skymly/DesignPatterns.Samples/actions/workflows/ci.yml)

**[Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns)** 向けの実行可能なコンソールサンプル集です — Strategy、Chain of Responsibility、Command Router、Composite、Factory Registry、Decorator、Event Aggregator、State 遷移テーブル、Step Builder、Fork–Join Work Graph、Singleton、MSDI 統合を網羅しています。

## 前提条件

- [.NET SDK 8](https://dotnet.microsoft.com/download)（[`global.json`](global.json) を参照）

## クローン構成

開発時は生成元リポジトリの**兄弟ディレクトリ**に配置する構成を既定としています：

```
<workspace>/
  DesignPatterns/
  DesignPatterns.Samples/    ← 本リポジトリ
  DesignPatterns.Docs/
```

```powershell
git clone https://github.com/Skymly/DesignPatterns.git
git clone https://github.com/Skymly/DesignPatterns.Samples.git
cd DesignPatterns.Samples
```

`../DesignPatterns/DesignPatterns.slnx` が存在する場合、`Directory.Build.props` により `UseLocalDesignPatterns` は既定で **true** になります。

## 単体サンプルの実行

```powershell
dotnet run --project DesignPatterns.Samples.Strategy -c Release
dotnet run --project DesignPatterns.Samples.DependencyInjection -c Release
```

## 全実行（CI）

```powershell
./build.ps1 --target Ci --configuration Release
```

同等コマンド：`dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release`

## プロジェクト一覧

| サンプル | 内容 |
|----------|------|
| **DesignPatterns.Samples.Strategy** | `[RegisterStrategy]` → キー定数 + 静的 `Instance` レジストリ；同期 `Pay` + 非同期 `ExecuteAsync`；ガード述語（`TryGetWithGuard`）；実行トレース（`ExecuteTracedAsync`） |
| **DesignPatterns.Samples.Chain** | `[HandlerOrder]` → 生成ハンドラーパイプライン；トレース呼び出し（`InvokeTracedAsync`）；例外可観測性（`IHandlerExceptionObserver`） |
| **DesignPatterns.Samples.CommandRouter** | 手動 `CommandRouterBuilder` + `[RegisterCommandHandler]` → `SendAsync` / `TrySendAsync`；`RegisterDi` + `AddCommandRouter` による DI 連携 |
| **DesignPatterns.Samples.Composite** | `[CompositePart]` → `BuildForest()` / `TraverseForest`（+ `BuildRoot`、手動ビルダー） |
| **DesignPatterns.Samples.Factory** | `[RegisterFactory]` ファクトリーレジストリ；非同期ファクトリー（`IAsyncFactoryRegistry`）；プールファクトリー（`IPooledFactoryRegistry` の `RentAsync`/`Return`） |
| **DesignPatterns.Samples.RegisterFactory** | 手動 `FactoryRegistryBuilder` 登録 |
| **DesignPatterns.Samples.Decorator** | `[Decorator]` → スタック + `DecoratorOrder` + 条件付き `Add` |
| **DesignPatterns.Samples.EventAggregator** | `IEventAggregator` の publish/subscribe；エラー分離（`ContinueOnError`）；発行トレース（`PublishTracedAsync`） |
| **DesignPatterns.Samples.GenerateSingleton** | `[GenerateSingleton]` 遅延シングルトン |
| **DesignPatterns.Samples.DependencyInjection** | Strategy / Factory / Handler 向け `RegisterDi` |
| **DesignPatterns.Samples.State** | 手動 `TransitionTableBuilder` + `[StateMachine]` / `[Transition]` 注文ライフサイクル；ガード述語；entry/exit アクション；`IStateMachine` ラッパー；`TransitionTrace` |
| **DesignPatterns.Samples.StepBuilder** | `[GenerateBuilder]` HTTP リクエスト：必須 Url+Method 型状態；任意 Header/Body；BearerToken と BasicAuth の排他 |
| **DesignPatterns.Samples.WorkGraph** | リクエスト準備フォーク・ジョイングラフ：`Auth` ∥ `LoadConfig` → `BuildPrincipal` → `Authorize`；手動 `WorkGraphBuilder` + 生成済み `RequestPrepWorkGraph.Create` / `RequestPrepWorkStepKeys`（Work Graph を含む NuGet が公開されるまでローカル兄弟リポジトリが必要） |
| **DesignPatterns.Samples.PluginAssemblies** | マルチアセンブリ `[RegisterStrategy]` + `RegisterAutofac` + `RegistryConfiguration`（`IConfiguration`；サブディレクトリの [README](DesignPatterns.Samples.PluginAssemblies/README.md) を参照） |

## 公開済み NuGet パッケージの利用

`Directory.Build.props` には公開済みの `0.2.3-preview2` が固定されています。NuGet パッケージ経由でサンプルを実行するには `UseLocalDesignPatterns=false` を指定してください：

```powershell
dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release --use-local-design-patterns false
```

`UseLocalDesignPatterns=false` 時でも `PluginAssemblies` は NuGet（`Skymly.DesignPatterns` + Autofac + Configuration 拡張）のみで完全に動作します。

## 関連リンク

| リンク | 説明 |
|--------|------|
| [DesignPatterns](https://github.com/Skymly/DesignPatterns) | ランタイム、ソースジェネレーター、テスト |
| [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs) | ユーザードキュメント（VitePress） |

## ライセンス

MIT — [LICENSE](LICENSE) を参照してください。
