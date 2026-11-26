# DesignPatterns.Samples

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md)

**[Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns)** 的可執行主控台範例集 —— 涵蓋 Strategy、Chain of Responsibility、Command Router、Composite、Factory Registry、Decorator、Event Aggregator、State 狀態轉換表、Step Builder、Fork–Join Work Graph、Singleton 與 MSDI 整合。

## 環境需求

- [.NET SDK 8](https://dotnet.microsoft.com/download)（請參閱 [`global.json`](global.json)）

## 複製目錄版面

範例預設放置於產生器主儲存庫的**同層目錄**以便本機開發：

```
<workspace-root>/
  Skymly/
    DesignPatterns/
      DesignPatterns/
      DesignPatterns.Samples/    ← 本儲存庫
      DesignPatterns.Docs/
```

```powershell
git clone https://github.com/Skymly/DesignPatterns.git
git clone https://github.com/Skymly/DesignPatterns.Samples.git
cd DesignPatterns.Samples
```

當 `../DesignPatterns/DesignPatterns.slnx` 存在時，`UseLocalDesignPatterns` 預設為 **true**（見 `Directory.Build.props`）。

## 執行單一範例

```powershell
dotnet run --project DesignPatterns.Samples.Strategy -c Release
dotnet run --project DesignPatterns.Samples.DependencyInjection -c Release
```

## 執行全部（CI）

```powershell
./build.ps1 --target Ci --configuration Release
```

等價命令：`dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release`

## 範例專案清單

| 範例專案 | 示範內容 |
|----------|----------|
| **DesignPatterns.Samples.Strategy** | `[RegisterStrategy]` → 鍵常數 + 靜態 `Instance` 登錄表；同步 `Pay` + 非同步 `ExecuteAsync`；守衛述詞（`TryGetWithGuard`）；執行追蹤（`ExecuteTracedAsync`） |
| **DesignPatterns.Samples.Chain** | `[HandlerOrder]` → 產生處理常式管線；追蹤呼叫（`InvokeTracedAsync`）；例外可觀測性（`IHandlerExceptionObserver`） |
| **DesignPatterns.Samples.CommandRouter** | 手動 `CommandRouterBuilder` + `[RegisterCommandHandler]` → `SendAsync` / `TrySendAsync`；透過 `RegisterDi` + `AddCommandRouter` 整合 DI |
| **DesignPatterns.Samples.Composite** | `[CompositePart]` → `BuildForest()` / `TraverseForest`（+ `BuildRoot`、手動建構器） |
| **DesignPatterns.Samples.Factory** | `[RegisterFactory]` 工廠登錄表；非同步工廠（`IAsyncFactoryRegistry`）；物件集區工廠（`IPooledFactoryRegistry` 的 `RentAsync`/`Return`） |
| **DesignPatterns.Samples.RegisterFactory** | 手動 `FactoryRegistryBuilder` 註冊 |
| **DesignPatterns.Samples.Decorator** | `[Decorator]` → 裝飾堆疊 + `DecoratorOrder` + 條件式 `Add` |
| **DesignPatterns.Samples.EventAggregator** | `IEventAggregator` 發布/訂閱；錯誤隔離（`ContinueOnError`）；發布追蹤（`PublishTracedAsync`） |
| **DesignPatterns.Samples.GenerateSingleton** | `[GenerateSingleton]` 延遲初始化單例 |
| **DesignPatterns.Samples.DependencyInjection** | Strategy / Factory / Handler 的 `RegisterDi` |
| **DesignPatterns.Samples.State** | 手動 `TransitionTableBuilder` + `[StateMachine]` / `[Transition]` 訂單生命週期；守衛述詞；entry/exit 動作；`IStateMachine` 包裝器；`TransitionTrace` |
| **DesignPatterns.Samples.StepBuilder** | `[GenerateBuilder]` HTTP 請求：必填 Url+Method 型別狀態；選填 Header/Body；BearerToken 與 BasicAuth 互斥 |
| **DesignPatterns.Samples.WorkGraph** | 請求準備分叉-彙整圖：`Auth` ∥ `LoadConfig` → `BuildPrincipal` → `Authorize`；手動 `WorkGraphBuilder` + 產生的 `RequestPrepWorkGraph.Create` / `RequestPrepWorkStepKeys`（在含 Work Graph 的 NuGet 套件發布前需同層主儲存庫） |
| **DesignPatterns.Samples.PluginAssemblies** | 多組件 `[RegisterStrategy]` + `RegisterAutofac` + `RegistryConfiguration`（`IConfiguration`；請參閱子目錄 [README](DesignPatterns.Samples.PluginAssemblies/README.md)） |

## 使用已發布的 NuGet 套件

`Directory.Build.props` 中固定了已發布的 `0.2.3-preview2` 套件版本。若要以 NuGet 套件執行範例，請設定 `UseLocalDesignPatterns=false`：

```powershell
dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release --use-local-design-patterns false
```

當 `UseLocalDesignPatterns=false` 時，`PluginAssemblies` 亦可完整以 NuGet（`Skymly.DesignPatterns` + Autofac + Configuration 擴充套件）執行。

## 相關連結

| 連結 | 說明 |
|------|------|
| [DesignPatterns](https://github.com/Skymly/DesignPatterns) | 執行階段、原始碼產生器與測試 |
| [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs) | 使用者文件網站（VitePress） |

## 授權條款

MIT — 請參閱 [LICENSE](LICENSE)。
