# GitHelper

一个用于简化 Git 命令操作的 .NET 工具库，提供了对 Git 命令的封装，使在 .NET 应用中执行 Git 操作变得更加简单和直观。

## 特性

- 简化 Git 命令执行
- 提供面向对象的 API
- 支持常用 Git 操作（clone、branch、checkout、merge、fetch、pull、push 等）
- 支持超时控制
- 支持异步操作
- MIT 开源许可证

## 安装

目前可以通过以下方式使用本项目：

1. 克隆项目源代码
2. 将项目添加到你的解决方案中
3. 添加项目引用

## 使用方法

### 基本用法

```csharp
using LeoChen.GitHelper;

// 创建 GitProcessHelper 实例
var gitHelper = new GitProcessHelper(@"C:\path\to\your\repository");

// 执行 Git 命令
var result = gitHelper.Status();
if (result.Success)
{
    Console.WriteLine("Git 状态:");
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"执行失败: {result.Error}");
}
```

### 克隆仓库

```csharp
// 克隆远程仓库
var cloneResult = gitHelper.Clone("https://github.com/user/repo.git", @"C:\path\to\local\directory");
if (cloneResult.Success)
{
    Console.WriteLine("仓库克隆成功");
}
```

### 分支操作

```csharp
// 获取所有本地分支
var localBranches = gitHelper.GetLocalBranch();
foreach (var branch in localBranches)
{
    Console.WriteLine($"本地分支: {branch}");
}

// 获取所有远程分支
var remoteBranches = gitHelper.GetRemoteBranch();
foreach (var branch in remoteBranches)
{
    Console.WriteLine($"远程分支: {branch}");
}

// 获取当前分支
var currentBranch = gitHelper.GetCurrentBranch();
Console.WriteLine($"当前分支: {currentBranch}");
```

### 提交操作

```csharp
// 添加文件到暂存区
gitHelper.Add(".");

// 提交更改
var commitResult = gitHelper.Commit("提交信息");
if (commitResult.Success)
{
    Console.WriteLine("提交成功");
}
```

### 远程操作

```csharp
// 获取所有远程仓库
var remotes = gitHelper.GetAllRemotes();
foreach (var remote in remotes)
{
    Console.WriteLine($"远程仓库: {remote}");
}
```

### 推送和拉取

项目还提供了专门的 GitPushHelper、GitPullHelper、GitFetchHelper 和 GitMergeHelper 类来处理特定的 Git 操作。

```csharp
// 推送到远程仓库
var pushHelper = new GitPushHelper(gitHelper);
var pushResult = pushHelper.Push("origin", "main");
```

## API 参考

### GitProcessHelper 类

主要的 Git 操作类，提供以下方法：

- `Clone()` - 克隆仓库
- `Status()` - 查看状态
- `Log()` - 查看日志
- `Commit()` - 提交更改
- `Add()` - 添加文件到暂存区
- `Branch()` - 分支操作
- `GetLocalBranch()` - 获取本地分支
- `GetRemoteBranch()` - 获取远程分支
- `GetCurrentBranch()` - 获取当前分支
- `Remote()` - 远程仓库操作
- `GetAllRemotes()` - 获取所有远程仓库
- `Checkout()` - 切换分支或恢复文件

### GitResult 类

表示 Git 命令执行结果：

- `Command` - 执行的命令
- `Output` - 命令输出结果
- `Error` - 错误信息
- `ExitCode` - 退出代码
- `Success` - 是否执行成功

## 许可证

本项目采用 MIT 许可证，详见 [LICENSE](LICENSE.txt) 文件。
