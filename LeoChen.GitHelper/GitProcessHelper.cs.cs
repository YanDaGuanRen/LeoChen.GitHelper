using System.Diagnostics;
using System.Text;

namespace LeoChen.GitHelper;

/// <summary>
/// Git进程进程操作帮助类，用于执行git命令并处理结果
/// </summary>
public partial class GitProcessHelper : IDisposable
{
    private readonly string _gitPath;
    private string _workingDirectory;
    private bool _disposed = false; // 用于跟踪是否已释放资源


    public string WorkingDirectory
    {
        get => _workingDirectory;
        set => _workingDirectory = value;
    }

    /// <summary>
    /// 命令执行超时时间(毫秒)
    /// </summary>
    public int Timeout { get; set; } = 2 * 60 * 1000; // 默认1分钟

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workingDirectory">Git仓库工作目录</param>
    /// <param name="gitPath">git.exe的路径，如果已添加到环境变量可以直接使用"git"</param>
    public GitProcessHelper(string workingDirectory, string gitPath = "git")
    {
        _gitPath = gitPath;
        _workingDirectory = workingDirectory ?? throw new ArgumentNullException(nameof(workingDirectory));
    }

    #region Core

    /// <summary>
    /// 执行git命令
    /// </summary>
    /// <param name="arguments">git命令参数</param>
    /// <returns>命令执行结果</returns>
    public GitResult ExecuteCommand(string arguments)
    {
        var result = new GitResult();

        using (var process = new Process())
        {
            try
            {
                // 配置进程启动信息
                var startInfo = new ProcessStartInfo
                {
                    FileName = _gitPath,
                    Arguments = arguments,
                    WorkingDirectory = _workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                process.StartInfo = startInfo;

                // 订阅输出事件
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputBuilder.AppendLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        errorBuilder.AppendLine(e.Data);
                    }
                };

                // 启动进程
                process.Start();

                // 开始异步读取输出
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // 等待进程完成，带有超时
                var completed = process.WaitForExit(Timeout);

                if (!completed)
                {
                    process.Kill();
                    result.ExitCode = 9998;
                    result.Error = $"命令执行超时（超过{Timeout}毫秒）";
                    return result;
                }

                // 等待所有异步输出事件处理完毕
                process.WaitForExit();

                var ou = outputBuilder.ToString();
                result.Output = ou.TrimEnd();
                result.Error = errorBuilder.ToString().TrimEnd();
                result.ExitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                result.ExitCode = 9999;
                result.Error = $"执行命令时发生异常: {ex.Message}";
            }
        }

        return result;
    }

    /// <summary>
    /// 转义字符串中的引号，避免命令解析错误
    /// </summary>
    private string EscapeQuotes(string? input)
    {
        return input?.Replace("\"", "\\\"") ?? string.Empty;
    }

    #endregion

    #region 常用Git命令封装

    /// <summary>
    /// 执行git clone命令
    /// </summary>
    /// <param name="repositoryUrl">仓库URL</param>
    /// <param name="directory">目标目录</param>
    /// <returns>命令执行结果</returns>
    public GitResult Clone(string repositoryUrl, string? directory = null)
    {
        var args = $"clone \"{repositoryUrl}\"";
        if (!string.IsNullOrEmpty(directory))
        {
            args += $" \"{directory}\"";
        }

        return ExecuteCommand(args);
    }

    /// <summary>
    /// 执行git status命令
    /// </summary>
    /// <returns>命令执行结果</returns>
    public GitResult Status()
    {
        return ExecuteCommand("status");
    }

    /// <summary>
    /// 执行git log命令
    /// </summary>
    /// <param name="limit">限制显示的提交数量</param>
    /// <returns>命令执行结果</returns>
    public GitResult Log(int limit = 10)
    {
        return ExecuteCommand($"log -n {limit}");
    }

    /// <summary>
    /// 执行git commit命令
    /// </summary>
    /// <param name="message">提交信息</param>
    /// <returns>命令执行结果</returns>
    public GitResult Commit(string message)
    {
        return ExecuteCommand($"commit -m \"{EscapeQuotes(message)}\"");
    }

    /// <summary>
    /// 执行git add命令
    /// </summary>
    /// <param name="path">要添加的文件路径，默认为添加所有</param>
    /// <param name="args"></param>
    /// <returns>命令执行结果</returns>
    public GitResult Add(params string[] args)
    {
        if (args.Length < 1)
        {
            args = new[] { "." };
        }

        return ExecuteCommand($"add {args.Join(" ")}");
    }




    public GitResult Reset(params string[] args)
    {
        return ExecuteCommand($"reset {args.Join(" ")}");
    }

    public GitResult Status(params string[] args)
    {
        return ExecuteCommand($"status {args.Join(" ")}");
    }

    public GitResult Tag(params string[] args)
    {
        return ExecuteCommand($"tag {args.Join(" ")}");
    }

    public GitResult Update(params string[] args)
    {
        return ExecuteCommand($"update {args.Join(" ")}");
    }

    public GitResult UpdateSubmodule(params string[] args)
    {
        return ExecuteCommand($"update-submodule {args.Join(" ")}");
    }

    #endregion

    #region 扩展操作

    #region Branch
    /// <summary>
    /// 执行git branch命令
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public GitResult Branch(params string[] args)
    {
        return ExecuteCommand($"branch {args.Join(" ")}");
    }

    /// <summary>
    /// 获取所有本地分支
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetLocalBranch()
    {
        var rs = Branch("-l");
        return rs.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Replace("*", "").Trim());
    }

    /// <summary>
    /// 获取所有远程分支
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetRemoteBranch()
    {
        var rs = Branch("-r");
        return rs.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Replace("*", "").Trim());
    }

    /// <summary>
    /// 获取所有分支
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetAllBranch()
    {
        var rs = Branch("-a");
        return rs.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Replace("*", "").Trim());
    }

    public string GetCurrentBranch()
    {
        var rs = Branch("--show-current");
        return rs.Output.Trim();
    }

    // 1. 列出分支相关参数及使用示例
    // - 无参数：列出本地所有分支（当前分支前有 *）
    //   示例：git branch
    // - -r / --remotes：列出所有远程跟踪分支
    //   示例：git branch -r
    // - -a / --all：列出本地和远程所有分支
    //   示例：git branch -a
    // - --show-current：仅显示当前分支名称
    //   示例：git branch --show-current
    // - -v / --verbose：显示分支最新提交的哈希值和提交信息
    //   示例：git branch -v
    // - --merged <commit>：列出已合并到指定提交（默认 HEAD）的分支
    //   示例：git branch --merged main（列出已合并到 main 的分支）
    // - --no-merged <commit>：列出未合并到指定提交的分支
    //   示例：git branch --no-merged main（列出未合并到 main 的分支）
    // - --contains <commit>：列出包含指定提交的分支
    //   示例：git branch --contains a1b2c3d（列出包含提交 a1b2c3d 的分支）

    // 2. 创建分支相关参数及使用示例
    // - <branchname>：基于当前 HEAD 创建新分支
    //   示例：git branch feature/login（创建 feature/login 分支）
    // - <branchname> <start-point>：基于指定起点（提交、分支、标签）创建分支
    //   示例：git branch bugfix/v1.0 dev（基于 dev 分支创建 bugfix/v1.0）
    // - --track：创建分支时关联远程跟踪分支（默认行为，可省略）
    //   示例：git branch --track feature/remote origin/feature/remote（关联远程分支）
    // - --no-track：创建分支时不关联远程跟踪分支
    //   示例：git branch --no-track feature/independent
    // - -f / --force：强制重置现有分支到指定起点
    //   示例：git branch -f main origin/main（强制将 main 分支重置为远程 main 的状态）

    // 3. 重命名/移动分支相关参数及使用示例
    // - -m / --move：重命名分支（若新分支存在则失败）
    //   示例：git branch -m old-branch new-branch
    // - -M：强制重命名分支（覆盖现有分支）
    //   示例：git branch -M old-branch existing-branch

    // 4. 复制分支相关参数及使用示例
    // - -c / --copy：复制分支（包括配置和引用日志）
    //   示例：git branch -c feature/v1 feature/v2（复制 feature/v1 到 feature/v2）
    // - -C：强制复制分支（覆盖现有分支）
    //   示例：git branch -C feature/v1 existing-v2

    // 5. 删除分支相关参数及使用示例
    // - -d / --delete：删除已合并的分支（未合并则失败）
    //   示例：git branch -d feature/old（删除已合并的 feature/old）
    // - -D：强制删除分支（无论是否合并）
    //   示例：git branch -D feature/unused（强制删除未合并的分支）
    // - -r + -d：删除远程跟踪分支（本地记录，不影响远程仓库）
    //   示例：git branch -rd origin/old-branch

    // 6. 配置分支跟踪关系相关参数及使用示例
    // - -u <upstream> / --set-upstream-to=<upstream>：为分支设置上游跟踪分支
    //   示例：git branch -u origin/main main（让本地 main 跟踪远程 origin/main）
    // - --unset-upstream：取消分支的上游跟踪配置
    //   示例：git branch --unset-upstream feature/independent

    // 7. 其他参数及使用示例
    // - --color[=<when>]：为分支输出着色（always/auto/never）
    //   示例：git branch --color=always（始终着色显示）
    // - --sort=<key>：按指定键排序分支（如 refname 名称、committerdate 提交时间）
    //   示例：git branch --sort=-committerdate（按提交时间倒序排列）
    // - --format=<format>：自定义输出格式（支持占位符如 %(refname)、%(objectname)）
    //   示例：git branch --format="%(refname:short) - %(committerdate)"（显示分支名和提交时间）
    // - --edit-description：编辑分支描述（用于 git request-pull 等场景）
    //   示例：git branch --edit-description feature/docs

    #endregion

    #region Remote
    /// <summary>
    /// 获取远程仓库
    /// </summary>
    /// <param name="args">参数</param>
    /// <returns></returns>
    public GitResult Remote(params string[] args)
    {
        return ExecuteCommand($"remote {args.Join(" ")}");
    }
    // 1. 基础查看操作
    //- 无参数：显示现有远程仓库列表
    // 示例：git remote
    public IEnumerable<string> GetAllRemotes()
    {
        var rs = Remote();
        return rs.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim());
    }


    //- -v /--verbose：显示远程仓库名称及对应的 URL
    // 示例：git remote -v
    // 2. 添加远程仓库
    //- add <name> <URL>：为 URL 对应的仓库添加名为 name 的远程仓库
    // 示例：git remote add origin https://github.com/example/repo.git
    // - add -f <name> <URL>：添加远程仓库后立即执行 fetch
    // 示例：git remote add -f upstream https://github.com/upstream/repo.git
    // - add --tags <name> <URL>：添加远程仓库并导入所有标签
    // 示例：git remote add --tags origin https://github.com/example/repo.git
    // - add -t <branch> <name> <URL>：仅跟踪指定分支
    // 示例：git remote add -t main origin https://github.com/example/repo.git
    // - add -m <master> <name> <URL>：设置远程默认分支
    // 示例：git remote add -m main origin https://github.com/example/repo.git
    // - add --mirror=fetch <name> <URL>：创建获取镜像（裸仓库适用）
    // 示例：git remote add --mirror=fetch mirror https://github.com/example/repo.git
    // 3. 重命名远程仓库
    //- rename <old> <new>：将远程仓库 old 重命名为 new
    // 示例：git remote rename origin origin-old
    //- rename --progress <old> <new>：显示重命名进度
    // 示例：git remote rename --progress old-remote new-remote
    // 4. 删除远程仓库
    //- remove <name> / rm <name>：移除名为 name 的远程仓库
    // 示例：git remote remove upstream
    // 5. 设置远程仓库默认分支
    //- set-head <name> -a /--auto：自动设置远程默认分支（根据远程 HEAD）
    // 示例：git remote set-head origin -a
    //- set-head <name> -d /--delete：删除远程默认分支设置
    // 示例：git remote set-head origin -d
    //- set-head <name> <branch>：显式设置远程默认分支
    // 示例：git remote set-head origin develop
    // 6. 配置远程仓库跟踪分支
    //- set-branches <name> <branch>...：设置远程仓库跟踪的分支列表
    // 示例：git remote set-branches origin main develop
    //- set-branches --add <name> <branch>：向跟踪列表添加分支
    // 示例：git remote set-branches --add origin feature/new
    // 7. 查看 / 修改远程仓库 URL
    //- get-url <name>：获取远程仓库的 URL
    // 示例：git remote get-url origin
    //- get-url --push <name>：获取远程仓库的推送 URL
    // 示例：git remote get-url --push origin
    //- set-url <name> <newurl>：修改远程仓库的 URL
    // 示例：git remote set-url origin https://github.com/new/location.git
    // - set-url --push <name> <newurl>：修改远程仓库的推送 URL
    // 示例：git remote set-url --push origin https://github.com/push/location.git
    // - set-url --add <name> <newurl>：为远程仓库添加额外 URL
    // 示例：git remote set-url --add origin https://github.com/another/location.git
    // - set-url --delete <name> <URL>：删除远程仓库的指定 URL
    // 示例：git remote set-url --delete origin https://github.com/old/location.git
    // 8. 显示远程仓库详细信息
    //- show <name>：显示远程仓库的详细信息
    // 示例：git remote show origin
    //- show -n <name>：显示详细信息但不查询远程状态
    // 示例：git remote show -n upstream
    // 9. 清理远程跟踪分支
    //- prune <name>：删除已在远程删除的本地跟踪分支
    // 示例：git remote prune origin
    //- prune -n /--dry-run <name>：预览要删除的分支（不实际删除）
    // 示例：git remote prune --dry-run origin
    // 10. 更新远程跟踪分支
    //- update <name>：更新指定远程仓库的跟踪分支
    // 示例：git remote update origin
    //- update -p /--prune <name>：更新时同时清理已删除的远程分支
    // 示例：git remote update -p upstream

    #endregion


    
    #region Checkout
    /// <summary>
    /// git-checkout 命令用于切换分支或恢复工作树文件
    /// </summary>
    /// <param name="args">参数</param>
    /// <returns>命令执行结果 <see cref="GitResult"/></returns>
    public GitResult Checkout(params string[] args)
    {
        return ExecuteCommand($"checkout {args.Join(" ")}");
    }

    /// <summary>
    /// 命令：git checkout <paramref name="branch"/>
    /// <para>说明：切换到指定分支。更新索引和工作树以匹配该分支，HEAD 指向该分支，保留本地修改以便提交到该分支。</para>
    /// <para> 若<paramref name="branch"/>不存在但在某个远程有唯一匹配的跟踪分支，且未指定 --no-guess，则自动创建并跟踪该远程分支</para>
    /// </summary>
    /// <param name="branch">分支名</param>
    /// <returns>成功</returns>
    public bool CheckoutBranch(string branch)
    {
      return  Checkout(branch).Success ;
    }
    
    
    /// <example>
    /// 命令：git checkout -q <branch>
    /// 说明：安静模式下切换到指定分支，不输出反馈消息
    /// </example>

    /// <example>
    /// 命令：git checkout --progress <branch>
    /// 说明：切换到指定分支时，强制显示进度状态，即使未连接到终端
    /// </example>

    /// <example>
    /// 命令：git checkout --no-progress <branch>
    /// 说明：切换到指定分支时，不显示进度状态
    /// </example>

    /// <example>
    /// 命令：git checkout -f <branch>
    /// 说明：强制切换到指定分支，忽略索引或工作树与 HEAD 的差异，丢弃本地更改和阻碍的未跟踪文件/目录
    /// </example>

    /// <example>
    /// 命令：git checkout -b <new-branch>
    /// 说明：创建名为 <new-branch> 的新分支（以当前 HEAD 为起点），并切换到该新分支
    /// </example>

    /// <example>
    /// 命令：git checkout -b <new-branch> <start-point>
    /// 说明：以 <start-point> 为起点创建名为 <new-branch> 的新分支，并切换到该分支
    /// </example>

    /// <example>
    /// 命令：git checkout -B <new-branch>
    /// 说明：若 <new-branch> 不存在则创建；若存在则重置该分支到当前 HEAD，然后切换到该分支
    /// </example>

    /// <example>
    /// 命令：git checkout -B <new-branch> <start-point>
    /// 说明：若 <new-branch> 不存在则以 <start-point> 为起点创建；若存在则重置到 <start-point>，然后切换到该分支
    /// </example>

    /// <example>
    /// 命令：git checkout --orphan <new-branch>
    /// 说明：创建一个没有历史记录的新分支 <new-branch>，并切换到该分支
    /// </example>

    /// <example>
    /// 命令：git checkout --detach
    /// 说明：分离 HEAD 指针，使其指向当前分支的尖端，进入"分离 HEAD"状态，保留本地修改
    /// </example>

    /// <example>
    /// 命令：git checkout --detach <branch>
    /// 说明：分离 HEAD 指针，使其指向 <branch> 的尖端，更新索引和工作树，保留本地修改
    /// </example>

    /// <example>
    /// 命令：git checkout <commit>
    /// 说明：分离 HEAD 指针到指定提交 <commit>，更新索引和工作树，保留本地修改，以便在该提交基础上工作
    /// </example>

    /// <example>
    /// 命令：git checkout <tree-ish> -- <pathspec>...
    /// 说明：用 <tree-ish>（通常是提交）中的内容覆盖匹配 <pathspec> 的文件，同时更新索引和工作树，将文件恢复到 <tree-ish> 中的版本
    /// </example>

    /// <example>
    /// 命令：git checkout -- <pathspec>...
    /// 说明：未指定 <tree-ish> 时，用索引中的内容覆盖工作树中匹配 <pathspec> 的文件，丢弃本地修改
    /// </example>

    /// <example>
    /// 命令：git checkout --pathspec-from-file=<file>
    /// 说明：从 <file> 中读取路径规范，用索引中的内容覆盖工作树中对应文件
    /// </example>

    /// <example>
    /// 命令：git checkout --pathspec-from-file=<file> --pathspec-file-nul
    /// 说明：从 <file> 中读取以 NUL 分隔的路径规范，用索引中的内容覆盖工作树中对应文件
    /// </example>

    /// <example>
    /// 命令：git checkout --ours <pathspec>...
    /// 说明：在合并冲突时，从索引中检出"我们的"版本的文件（当前分支的版本）到工作树
    /// </example>

    /// <example>
    /// 命令：git checkout --theirs <pathspec>...
    /// 说明：在合并冲突时，从索引中检出"他们的"版本的文件（待合并分支的版本）到工作树
    /// </example>

    /// <example>
    /// 命令：git checkout -m <pathspec>...
    /// 说明：在合并冲突时，丢弃工作树中 <pathspec> 对应文件的更改，重新创建原始冲突的合并结果
    /// </example>

    /// <example>
    /// 命令：git checkout --conflict=<style> <pathspec>...
    /// 说明：指定合并冲突的解决风格 <style>，并检出 <pathspec> 对应文件
    /// </example>

    /// <example>
    /// 命令：git checkout -p
    /// 说明：交互式模式，显示索引与工作树的差异，允许选择要应用的代码块，以恢复或更新文件
    /// </example>

    /// <example>
    /// 命令：git checkout -p <tree-ish> <pathspec>...
    /// 说明：交互式模式，显示 <tree-ish> 与工作树中 <pathspec> 对应文件的差异，选择要应用的代码块
    /// </example>
    

    #endregion
    #endregion

    #region IDisposable 实现

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源的核心方法
    /// </summary>
    /// <param name="disposing">是否从Dispose方法调用</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        // 如果是手动调用Dispose，释放托管资源
        if (disposing)
        {
            // 这里可以释放托管资源，当前类没有需要释放的托管资源
            // 例如：如果未来有Process、Stream等成员，在这里释放
        }

        // 释放非托管资源（当前类没有直接使用非托管资源）
        // 例如：如果有非托管句柄等，在这里释放

        _disposed = true;
    }

    /// <summary>
    /// 析构函数，用于释放非托管资源
    /// </summary>
    ~GitProcessHelper()
    {
        Dispose(false);
    }

    #endregion
}