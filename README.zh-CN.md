# DesignPatterns.Samples

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md)

[![CI](https://github.com/Skymly/DesignPatterns.Samples/actions/workflows/ci.yml/badge.svg)](https://github.com/Skymly/DesignPatterns.Samples/actions/workflows/ci.yml)

**[Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns)** 的可运行控制台示例集 —— 涵盖 Strategy、Chain of Responsibility、Command Router、Composite、Factory Registry、Decorator、Event Aggregator、State 状态转换表、Step Builder、Fork–Join Work Graph、Singleton 以及 MSDI 集成。

## 环境要求

- [.NET SDK 8](https://dotnet.microsoft.com/download)（见 [`global.json`](global.json)）

## 克隆布局

示例默认与生成器主仓**同级并列**放置以便本地开发：

```
<workspace>/
  DesignPatterns/
  DesignPatterns.Samples/    ← 本仓库
  DesignPatterns.Docs/
```

```powershell
git clone https://github.com/Skymly/DesignPatterns.git
git clone https://github.com/Skymly/DesignPatterns.Samples.git
cd DesignPatterns.Samples
```

当 `../DesignPatterns/DesignPatterns.slnx` 存在时，`UseLocalDesignPatterns` 默认设为 **true**（见 `Directory.Build.props`）。

## 运行单个示例

```powershell
dotnet run --project DesignPatterns.Samples.Strategy -c Release
dotnet run --project DesignPatterns.Samples.DependencyInjection -c Release
```

## 运行全部（CI）

```powershell
./build.ps1 --target Ci --configuration Release
```

等价命令：`dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release`

## 示例项目列表

| 示例项目 | 演示内容 |
|----------|----------|
| **DesignPatterns.Samples.Strategy** | `[RegisterStrategy]` → 键常量 + 静态 `Instance` 注册表；同步 `Pay` + 异步 `ExecuteAsync`；守卫谓词（`TryGetWithGuard`）；执行追踪（`ExecuteTracedAsync`） |
| **DesignPatterns.Samples.Chain** | `[HandlerOrder]` → 生成处理器管线；追踪调用（`InvokeTracedAsync`）；异常可观测性（`IHandlerExceptionObserver`） |
| **DesignPatterns.Samples.CommandRouter** | 手动 `CommandRouterBuilder` + `[RegisterCommandHandler]` → `SendAsync` / `TrySendAsync`；通过 `RegisterDi` + `AddCommandRouter` 接入 DI |
| **DesignPatterns.Samples.Composite** | `[CompositePart]` → `BuildForest()` / `TraverseForest`（+ `BuildRoot`、手动构建器） |
| **DesignPatterns.Samples.Factory** | `[RegisterFactory]` 工厂注册表；异步工厂（`IAsyncFactoryRegistry`）；对象池工厂（`IPooledFactoryRegistry` 的 `RentAsync`/`Return`） |
| **DesignPatterns.Samples.RegisterFactory** | 手动 `FactoryRegistryBuilder` 注册 |
| **DesignPatterns.Samples.Decorator** | `[Decorator]` → 装饰栈 + `DecoratorOrder` + 条件 `Add` |
| **DesignPatterns.Samples.EventAggregator** | `IEventAggregator` 发布/订阅；错误隔离（`ContinueOnError`）；发布追踪（`PublishTracedAsync`） |
| **DesignPatterns.Samples.GenerateSingleton** | `[GenerateSingleton]` 延迟初始化单例 |
| **DesignPatterns.Samples.DependencyInjection** | Strategy / Factory / Handler 的 `RegisterDi` |
| **DesignPatterns.Samples.State** | 手动 `TransitionTableBuilder` + `[StateMachine]` / `[Transition]` 订单生命周期；守卫谓词；entry/exit 动作；`IStateMachine` 包装器；`TransitionTrace` |
| **DesignPatterns.Samples.StepBuilder** | `[GenerateBuilder]` HTTP 请求：必填 Url+Method 类型状态；可选 Header/Body；BearerToken 与 BasicAuth 互斥 |
| **DesignPatterns.Samples.WorkGraph** | 请求准备分叉-汇聚图：`Auth` ∥ `LoadConfig` → `BuildPrincipal` → `Authorize`；手动 `WorkGraphBuilder` + 生成的 `RequestPrepWorkGraph.Create` / `RequestPrepWorkStepKeys`（在发布含 Work Graph 的 NuGet 包前需同级主仓源码） |
| **DesignPatterns.Samples.PluginAssemblies** | 多程序集 `[RegisterStrategy]` + `RegisterAutofac` + `RegistryConfiguration`（`IConfiguration`；见子目录 [README](DesignPatterns.Samples.PluginAssemblies/README.md)） |

## 使用已发布的 NuGet 包

`Directory.Build.props` 中固定了已发布的 `0.2.3-preview2` 包版本。如需以 NuGet 包方式运行示例，请设置 `UseLocalDesignPatterns=false`：

```powershell
dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release --use-local-design-patterns false
```

当 `UseLocalDesignPatterns=false` 时，`PluginAssemblies` 也可完整基于 NuGet（`Skymly.DesignPatterns` + Autofac + Configuration 扩展）运行。

## 相关链接

| 链接 | 说明 |
|------|------|
| [DesignPatterns](https://github.com/Skymly/DesignPatterns) | 运行时、源生成器与测试 |
| [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs) | 用户文档站（VitePress） |

## 许可证

MIT — 见 [LICENSE](LICENSE)。
