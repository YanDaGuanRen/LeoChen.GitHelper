namespace LeoChen.GitHelper;

public partial  class GitProcessHelper
{
    /// <summary>
    /// 执行 git pull 命令，从另一个仓库或本地分支获取并整合更改。
    /// <para>如果当前分支落后于远程，默认会快进当前分支以匹配远程；如果当前分支与远程有分歧，需通过--rebase或--no-rebase指定如何协调。</para>
    /// <para>该命令先运行git fetch，再根据配置或命令行标志调用git rebase或git merge来协调分歧分支。</para>
    /// </summary>
    /// <param name="args">git pull 命令的参数，如仓库、引用规范、选项等</param>
    /// <returns>执行 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git pull</para>
    /// <para>说明：从当前分支配置的远程仓库和分支获取并整合更改，使用默认的合并或变基方式。</para>
    /// <para>2. 示例：git pull origin master</para>
    /// <para>说明：从远程仓库origin的master分支获取更改并整合到当前分支。</para>
    /// <para>3. 示例：git pull -q</para>
    /// <para>说明：安静模式执行pull，抑制git-fetch和git-merge的输出。</para>
    /// <para>4. 示例：git pull -v</para>
    /// <para>说明：详细模式执行pull，向git-fetch和git-merge传递--verbose参数，输出更多信息。</para>
    /// <para>5. 示例：git pull --recurse-submodules</para>
    /// <para>说明：获取子模块的新提交，并更新活跃子模块的工作树。</para>
    /// <para>6. 示例：git pull --no-recurse-submodules</para>
    /// <para>说明：不获取子模块的新提交，也不更新子模块工作树。</para>
    /// <para>7. 示例：git pull --recurse-submodules=on-demand</para>
    /// <para>说明：按需获取子模块的新提交并更新工作树（具体行为参考git-fetch等文档）。</para>
    /// <para>8. 示例：git pull --commit</para>
    /// <para>说明：执行合并后提交结果，可覆盖--no-commit选项，仅在合并时有效。</para>
    /// <para>9. 示例：git pull --no-commit</para>
    /// <para>说明：执行合并但在创建合并提交前停止，让用户检查和调整合并结果后再手动提交。注意快进更新不会创建合并提交，因此--no-commit对其无效；若要确保分支不被更新，需结合--no-ff使用。</para>
    /// <para>10. 示例：git pull --edit</para>
    /// <para>说明：合并成功后调用编辑器，让用户进一步编辑自动生成的合并消息。</para>
    /// <para>11. 示例：git pull -e</para>
    /// <para>说明：与--edit功能相同，合并成功后调用编辑器编辑合并消息。</para>
    /// <para>12. 示例：git pull --no-edit</para>
    /// <para>说明：接受自动生成的合并消息，不调用编辑器（通常不推荐）。</para>
    /// <para>13. 示例：git pull --cleanup=scissors</para>
    /// <para>说明：设置合并消息的清理模式为scissors，在合并冲突时会在MERGE_MSG后附加分隔线。其他模式参考git-commit文档。</para>
    /// <para>14. 示例：git pull --ff-only</para>
    /// <para>说明：仅在没有本地分歧历史时更新到新历史，这是未指定分歧历史协调方法时的默认行为。</para>
    /// </remarks>
    public GitResult Pull(params string[] args)
    {
        return ExecuteCommand($"pull {string.Join(" ", args)}");
    }

    /// <summary>
    /// 执行 git pull 命令的安静模式，抑制git-fetch和git-merge的输出。
    /// </summary>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行 git pull 安静模式命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git pull -q</para>
    /// <para>说明：安静执行pull，不输出获取和合并过程的信息。</para>
    /// <para>2. 示例：git pull -q origin develop</para>
    /// <para>说明：从远程origin的develop分支安静获取并整合更改，不输出过程信息。</para>
    /// </remarks>
    public GitResult PullQuiet(params string[] args)
    {
        return Pull("-q", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令的详细模式，向git-fetch和git-merge传递--verbose参数。
    /// </summary>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行 git pull 详细模式命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git pull -v</para>
    /// <para>说明：详细执行pull，输出获取和合并过程的详细信息。</para>
    /// <para>2. 示例：git pull -v upstream feature-branch</para>
    /// <para>说明：从远程upstream的feature-branch分支详细获取并整合更改，输出详细过程信息。</para>
    /// </remarks>
    public GitResult PullVerbose(params string[] args)
    {
        return Pull("-v", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令并控制子模块的处理方式。
    /// </summary>
    /// <param name="recurseOption">子模块处理选项，可为"yes"、"on-demand"、"no"，若为null则仅使用--recurse-submodules</param>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行带指定子模块选项的 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git pull --recurse-submodules</para>
    /// <para>说明：获取子模块的新提交，并更新活跃子模块的工作树。</para>
    /// <para>2. 示例：git pull --recurse-submodules=yes</para>
    /// <para>说明：同--recurse-submodules，获取子模块新提交并更新工作树。</para>
    /// <para>3. 示例：git pull --recurse-submodules=on-demand</para>
    /// <para>说明：按需处理子模块，具体行为参考git-fetch等相关文档。</para>
    /// <para>4. 示例：git pull --no-recurse-submodules</para>
    /// <para>说明：不处理子模块，不获取其新提交也不更新工作树。</para>
    /// </remarks>
    public GitResult PullRecurseSubmodules(string recurseOption = null, params string[] args)
    {
        string option = recurseOption == null ? "--recurse-submodules" : $"--recurse-submodules={recurseOption}";
        return Pull(option, string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令并指定是否在合并后自动提交。
    /// </summary>
    /// <param name="commit">true使用--commit，false使用--no-commit</param>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行带提交选项的 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git pull --commit</para>
    /// <para>说明：执行合并后自动提交结果，可覆盖--no-commit。</para>
    /// <para>2. 示例：git pull --no-commit</para>
    /// <para>说明：执行合并后不自动提交，让用户检查调整后手动提交（快进更新无效）。</para>
    /// </remarks>
    public GitResult PullCommit(bool commit, params string[] args)
    {
        string option = commit ? "--commit" : "--no-commit";
        return Pull(option, string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令并指定是否编辑合并消息。
    /// </summary>
    /// <param name="edit">true使用--edit，false使用--no-edit，null使用-e（同--edit）</param>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行带编辑消息选项的 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git pull --edit</para>
    /// <para>说明：合并成功后调用编辑器编辑合并消息。</para>
    /// <para>2. 示例：git pull -e</para>
    /// <para>说明：同--edit，合并成功后调用编辑器编辑合并消息。</para>
    /// <para>3. 示例：git pull --no-edit</para>
    /// <para>说明：不编辑合并消息，直接使用自动生成的消息。</para>
    /// </remarks>
    public GitResult PullEdit(bool? edit, params string[] args)
    {
        string option = edit == true ? "--edit" : edit == false ? "--no-edit" : "-e";
        return Pull(option, string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令并指定合并消息的清理模式。
    /// </summary>
    /// <param name="mode">清理模式，如"scissors"等（参考git-commit文档）</param>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行带指定清理模式的 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git pull --cleanup=scissors</para>
    /// <para>说明：设置合并消息清理模式为scissors，合并冲突时在MERGE_MSG后附加分隔线。</para>
    /// <para>2. 示例：git pull --cleanup=strip</para>
    /// <para>说明：设置合并消息清理模式为strip，去除消息前后的空白（具体行为参考git-commit）。</para>
    /// </remarks>
    public GitResult PullCleanup(string mode, params string[] args)
    {
        return Pull($"--cleanup={mode}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git pull 命令，仅在无本地分歧历史时更新。
    /// </summary>
    /// <param name="args">其他可选参数，如仓库、引用规范等</param>
    /// <returns>执行 --ff-only 选项的 git pull 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git pull --ff-only</para>
    /// <para>说明：仅当本地分支与远程无分歧时，更新到远程的新历史。</para>
    /// <para>2. 示例：git pull --ff-only origin main</para>
    /// <para>说明：仅当本地分支与origin/main无分歧时，从该分支获取并更新本地分支。</para>
    /// </remarks>
    public GitResult PullFfOnly(params string[] args)
    {
        return Pull("--ff-only", string.Join(" ", args));
    }
}