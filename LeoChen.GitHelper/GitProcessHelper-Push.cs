namespace LeoChen.GitHelper;

public partial class GitProcessHelper
{
    /// <summary>
    /// 执行 git push 命令，用于更新远程引用（refs）以及相关的对象。
    /// <para>通过本地引用更新远程引用，并发送完成给定引用所需的对象。</para>
    /// <para>若命令行未指定推送目标（&lt;repository&gt;），则会参考当前分支的 branch.*.remote 配置，默认为 origin。</para>
    /// <para>若未指定推送内容（&lt;refspec&gt; 或相关选项），则会参考 remote.*.push 配置，若无则使用 push.default 配置。</para>
    /// </summary>
    /// <param name="args">git push 命令的参数列表，包括选项、仓库地址、引用规范等</param>
    /// <returns>执行 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git push</para>
    /// <para>说明：根据配置推送当前分支到默认远程仓库的对应上游分支（默认对应 push.default 的 simple 行为，即仅当本地分支与上游分支同名时推送）。</para>
    /// <para>2. 示例：git push origin</para>
    /// <para>说明：推送当前分支到名为 origin 的远程仓库的对应上游分支。</para>
    /// <para>3. 示例：git push origin master</para>
    /// <para>说明：将本地 master 分支推送到 origin 远程仓库的 master 分支（若远程无 master 则创建）。</para>
    /// <para>4. 示例：git push origin feature:dev</para>
    /// <para>说明：将本地 feature 分支推送到 origin 远程仓库的 dev 分支。</para>
    /// <para>5. 示例：git push --all origin</para>
    /// <para>说明：推送所有本地分支到 origin 远程仓库。</para>
    /// <para>6. 示例：git push --tags</para>
    /// <para>说明：推送所有本地标签到默认远程仓库。</para>
    /// <para>7. 示例：git push -n origin master</para>
    /// <para>说明：模拟推送本地 master 分支到 origin 远程仓库，不实际执行推送操作，用于查看推送效果。</para>
    /// <para>8. 示例：git push -f origin master</para>
    /// <para>说明：强制推送本地 master 分支到 origin 远程仓库，可能会覆盖远程分支的历史，慎用。</para>
    /// <para>9. 示例：git push --delete origin feature</para>
    /// <para>说明：删除 origin 远程仓库的 feature 分支。</para>
    /// <para>10. 示例：git push -u origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并将 origin/master 设置为本地 master 分支的上游分支。</para>
    /// <para>11. 示例：git push --push-option="ci-skip" origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并传递推送选项 "ci-skip" 给远程仓库的钩子程序。</para>
    /// <para>12. 示例：git push --signed origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并对推送进行签名。</para>
    /// <para>13. 示例：git push --force-with-lease=master:oldcommit origin</para>
    /// <para>说明：在确保远程 master 分支当前指向 oldcommit 时，强制推送本地 master 分支到 origin 远程仓库，用于安全强制推送。</para>
    /// <para>14. 示例：git push --no-verify origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，跳过预推送钩子的验证。</para>
    /// <para>15. 示例：git push --mirror origin</para>
    /// <para>说明：将本地仓库的所有引用（分支、标签等）镜像推送到 origin 远程仓库，会删除远程仓库中本地不存在的引用。</para>
    /// <para>16. 示例：git push --follow-tags origin</para>
    /// <para>说明：推送当前分支到 origin 远程仓库，并同时推送所有可访问的标签（即指向已推送提交的标签）。</para>
    /// <para>17. 示例：git push --atomic origin master dev</para>
    /// <para>说明：原子化推送本地 master 和 dev 分支到 origin 远程仓库，确保所有分支都推送成功，若有一个失败则全部回滚。</para>
    /// <para>18. 示例：git push --prune origin</para>
    /// <para>说明：推送当前分支到 origin 远程仓库，并删除远程仓库中本地已不存在的对应引用。</para>
    /// <para>19. 示例：git push --quiet origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，减少输出信息，仅显示必要内容。</para>
    /// <para>20. 示例：git push --verbose origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，显示详细的推送过程信息。</para>
    /// <para>21. 示例：git push --receive-pack=/path/to/git-receive-pack origin</para>
    /// <para>说明：指定远程仓库的 git-receive-pack 程序路径，推送当前分支到 origin 远程仓库。</para>
    /// <para>22. 示例：git push --repo=https://github.com/example/repo.git master</para>
    /// <para>说明：推送本地 master 分支到指定 URL 的远程仓库（https://github.com/example/repo.git）。</para>
    /// <para>23. 示例：git push --no-signed origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，不对推送进行签名。</para>
    /// <para>24. 示例：git push --signed=if-asked origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，仅当远程仓库要求时才对推送进行签名。</para>
    /// <para>25. 示例：git push --force-if-includes origin master</para>
    /// <para>说明：在强制推送本地 master 分支到 origin 远程仓库时，确保本地包含远程分支的所有提交，增加安全性。</para>
    /// </remarks>
    public GitResult Push(params string[] args)
    {
        return ExecuteCommand($"push {string.Join(" ", args)}");
    }

    /// <summary>
    /// 执行 git push --all 命令，推送所有本地分支到指定远程仓库。
    /// <para>该选项会将本地所有分支推送到目标远程仓库，每个分支对应远程的同名分支。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <returns>执行 git push --all 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --all</para>
    /// <para>说明：推送所有本地分支到默认远程仓库（通常为 origin）。</para>
    /// <para>2. 示例：git push --all origin</para>
    /// <para>说明：推送所有本地分支到名为 origin 的远程仓库。</para>
    /// <para>3. 示例：git push --all https://github.com/example/repo.git</para>
    /// <para>说明：推送所有本地分支到指定 URL 的远程仓库。</para>
    /// </remarks>
    public GitResult PushAll(string repository = "")
    {
        var args = new List<string> { "--all" };
        if (!string.IsNullOrEmpty(repository))
            args.Add(repository);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --branches 命令，推送所有匹配的分支到指定远程仓库（具体行为受配置影响）。
    /// <para>通常用于推送所有本地分支，类似 --all，但可能受远程配置的分支模式影响。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <returns>执行 git push --branches 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --branches</para>
    /// <para>说明：推送所有本地分支到默认远程仓库（通常为 origin）。</para>
    /// <para>2. 示例：git push --branches origin</para>
    /// <para>说明：推送所有本地分支到名为 origin 的远程仓库。</para>
    /// </remarks>
    public GitResult PushBranches(string repository = "")
    {
        var args = new List<string> { "--branches" };
        if (!string.IsNullOrEmpty(repository))
            args.Add(repository);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --mirror 命令，镜像推送本地所有引用到指定远程仓库。
    /// <para>会推送所有分支、标签等引用，并删除远程仓库中本地不存在的引用，适用于备份仓库。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <returns>执行 git push --mirror 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --mirror</para>
    /// <para>说明：将本地所有引用镜像推送到默认远程仓库，远程会删除本地没有的引用。</para>
    /// <para>2. 示例：git push --mirror backup-remote</para>
    /// <para>说明：将本地所有引用镜像推送到名为 backup-remote 的远程仓库，用于备份。</para>
    /// </remarks>
    public GitResult PushMirror(string repository = "")
    {
        var args = new List<string> { "--mirror" };
        if (!string.IsNullOrEmpty(repository))
            args.Add(repository);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --tags 命令，推送所有本地标签到指定远程仓库。
    /// <para>该选项仅推送标签，不推送分支，所有本地标签会被推送到远程仓库。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <returns>执行 git push --tags 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --tags</para>
    /// <para>说明：推送所有本地标签到默认远程仓库。</para>
    /// <para>2. 示例：git push --tags origin</para>
    /// <para>说明：推送所有本地标签到名为 origin 的远程仓库。</para>
    /// </remarks>
    public GitResult PushTags(string repository = "")
    {
        var args = new List<string> { "--tags" };
        if (!string.IsNullOrEmpty(repository))
            args.Add(repository);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --follow-tags 命令，推送当前分支及可访问的标签到指定远程仓库。
    /// <para>仅推送指向已推送提交的标签，即能通过当前推送的分支访问到的标签。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <param name="refspec">引用规范（如分支名，可选）</param>
    /// <returns>执行 git push --follow-tags 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --follow-tags</para>
    /// <para>说明：推送当前分支及可访问的标签到默认远程仓库。</para>
    /// <para>2. 示例：git push --follow-tags origin master</para>
    /// <para>说明：推送本地 master 分支及可访问的标签到 origin 远程仓库。</para>
    /// </remarks>
    public GitResult PushFollowTags(string repository = "", string refspec = "")
    {
        var args = new List<string> { "--follow-tags" };
        if (!string.IsNullOrEmpty(repository))
            args.Add(repository);
        if (!string.IsNullOrEmpty(refspec))
            args.Add(refspec);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --atomic 命令，原子化推送多个引用到指定远程仓库。
    /// <para>确保所有指定的引用都推送成功，若有一个失败则全部回滚，保证远程仓库状态一致性。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <param name="refspecs">多个引用规范（如分支名列表）</param>
    /// <returns>执行 git push --atomic 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --atomic origin master dev</para>
    /// <para>说明：原子化推送本地 master 和 dev 分支到 origin 远程仓库，确保两者同时成功或同时失败。</para>
    /// <para>2. 示例：git push --atomic https://github.com/example/repo.git feature1 feature2</para>
    /// <para>说明：原子化推送本地 feature1 和 feature2 分支到指定 URL 的远程仓库。</para>
    /// </remarks>
    public GitResult PushAtomic(string repository, params string[] refspecs)
    {
        var args = new List<string> { "--atomic", repository };
        args.AddRange(refspecs);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push -n 或 git push --dry-run 命令，模拟推送操作而不实际执行。
    /// <para>用于查看推送会执行的操作（如哪些引用会被更新），但不会真正修改远程仓库。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行 git push --dry-run 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --dry-run</para>
    /// <para>说明：模拟推送当前分支到默认远程仓库，显示推送操作详情但不实际执行。</para>
    /// <para>2. 示例：git push -n origin master</para>
    /// <para>说明：模拟推送本地 master 分支到 origin 远程仓库，显示可能的推送结果。</para>
    /// </remarks>
    public GitResult PushDryRun(params string[] args)
    {
        return Push(new[] { "--dry-run" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --receive-pack=&lt;git-receive-pack&gt; 命令，指定远程的 git-receive-pack 程序路径进行推送。
    /// <para>git-receive-pack 是远程仓库用于接收推送的程序，该选项用于指定其路径。</para>
    /// </summary>
    /// <param name="receivePackPath">远程 git-receive-pack 程序的路径</param>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行指定 receive-pack 的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --receive-pack=/usr/local/git/bin/git-receive-pack origin master</para>
    /// <para>说明：指定远程 origin 的 git-receive-pack 程序路径为 /usr/local/git/bin/git-receive-pack，推送本地 master 分支。</para>
    /// </remarks>
    public GitResult PushWithReceivePack(string receivePackPath, params string[] args)
    {
        return Push(new[] { $"--receive-pack={receivePackPath}" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --repo=&lt;repository&gt; 命令，指定推送的目标远程仓库。
    /// <para>用于在命令中直接指定远程仓库地址或名称，覆盖默认配置。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称</param>
    /// <param name="refspecs">引用规范（如分支名等，可选）</param>
    /// <returns>执行指定仓库的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --repo=origin master</para>
    /// <para>说明：推送本地 master 分支到名为 origin 的远程仓库。</para>
    /// <para>2. 示例：git push --repo=https://github.com/example/repo.git dev</para>
    /// <para>说明：推送本地 dev 分支到指定 URL 的远程仓库。</para>
    /// </remarks>
    public GitResult PushToRepo(string repository, params string[] refspecs)
    {
        var args = new List<string> { $"--repo={repository}" };
        args.AddRange(refspecs);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push -f 或 git push --force 命令，强制推送本地引用到远程仓库。
    /// <para>允许覆盖远程仓库的引用历史，可能导致数据丢失，慎用。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行强制推送的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --force origin master</para>
    /// <para>说明：强制推送本地 master 分支到 origin 远程仓库，覆盖远程的 master 分支历史。</para>
    /// <para>2. 示例：git push -f feature:dev</para>
    /// <para>说明：强制推送本地 feature 分支到默认远程仓库的 dev 分支。</para>
    /// </remarks>
    public GitResult PushForce(params string[] args)
    {
        return Push(new[] { "--force" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push -d 或 git push --delete 命令，删除远程仓库的指定引用。
    /// <para>可用于删除远程分支、标签等引用。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <param name="refspecs">要删除的远程引用（如分支名、标签名）</param>
    /// <returns>执行删除远程引用的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --delete origin feature</para>
    /// <para>说明：删除 origin 远程仓库的 feature 分支。</para>
    /// <para>2. 示例：git push -d origin v1.0</para>
    /// <para>说明：删除 origin 远程仓库的 v1.0 标签。</para>
    /// </remarks>
    public GitResult PushDelete(string repository, params string[] refspecs)
    {
        var args = new List<string> { "--delete", repository };
        args.AddRange(refspecs);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push --prune 命令，推送时删除远程仓库中本地不存在的对应引用。
    /// <para>保持远程仓库的引用与本地同步，删除本地已删除的引用。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <param name="refspecs">引用规范（如分支名等，可选）</param>
    /// <returns>执行带修剪的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --prune origin</para>
    /// <para>说明：推送当前分支到 origin 远程仓库，并删除 origin 中本地已不存在的对应分支。</para>
    /// <para>2. 示例：git push --prune origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并删除 origin 中与 master 相关的本地已不存在的引用。</para>
    /// </remarks>
    public GitResult PushPrune(string repository, params string[] refspecs)
    {
        var args = new List<string> { "--prune", repository };
        args.AddRange(refspecs);
        return Push(args.ToArray());
    }

    /// <summary>
    /// 执行 git push -q 或 git push --quiet 命令，安静地执行推送操作，减少输出信息。
    /// <para>仅显示错误信息，不显示详细的推送过程。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行安静推送的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --quiet origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，仅在出错时显示信息。</para>
    /// <para>2. 示例：git push -q</para>
    /// <para>说明：安静地推送当前分支到默认远程仓库，减少输出。</para>
    /// </remarks>
    public GitResult PushQuiet(params string[] args)
    {
        return Push(new[] { "--quiet" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push -v 或 git push --verbose 命令，详细显示推送操作过程。
    /// <para>输出更多推送相关的信息，便于调试和了解推送细节。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行详细推送的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --verbose origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，显示详细的推送过程信息。</para>
    /// <para>2. 示例：git push -v --tags</para>
    /// <para>说明：推送所有标签到默认远程仓库，并显示详细信息。</para>
    /// </remarks>
    public GitResult PushVerbose(params string[] args)
    {
        return Push(new[] { "--verbose" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push -u 或 git push --set-upstream 命令，推送并设置上游分支。
    /// <para>将本地分支与远程分支关联，后续可直接使用 git pull 拉取，git push 推送。</para>
    /// </summary>
    /// <param name="repository">远程仓库地址或名称（如 origin）</param>
    /// <param name="refspec">引用规范（如分支名，格式可以是本地分支:远程分支）</param>
    /// <returns>执行设置上游的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --set-upstream origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并将 origin/master 设置为本地 master 的上游分支。</para>
    /// <para>2. 示例：git push -u origin feature:dev</para>
    /// <para>说明：推送本地 feature 分支到 origin 远程仓库的 dev 分支，并将 origin/dev 设置为本地 feature 的上游分支。</para>
    /// </remarks>
    public GitResult PushSetUpstream(string repository, string refspec)
    {
        return Push("--set-upstream", repository, refspec);
    }

    /// <summary>
    /// 执行 git push -o 或 git push --push-option=&lt;string&gt; 命令，推送时传递选项给远程仓库的钩子程序。
    /// <para>远程仓库的 pre-receive 等钩子可以获取这些选项并执行相应操作（如跳过CI）。</para>
    /// </summary>
    /// <param name="pushOption">要传递的推送选项字符串</param>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行带推送选项的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --push-option="ci-skip" origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，传递选项 "ci-skip" 给远程钩子，可能用于跳过CI构建。</para>
    /// <para>2. 示例：git push -o "deploy=staging" origin dev</para>
    /// <para>说明：推送本地 dev 分支到 origin 远程仓库，传递选项 "deploy=staging" 给远程钩子，可能用于触发 staging 环境部署。</para>
    /// </remarks>
    public GitResult PushWithOption(string pushOption, params string[] args)
    {
        return Push(new[] { $"--push-option={pushOption}" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --signed 或 git push --no-signed 命令，控制推送是否进行签名。
    /// <para>--signed 会对推送进行签名，用于验证推送者身份；--no-signed 则不签名。</para>
    /// </summary>
    /// <param name="signed">是否签名，true 表示 --signed，false 表示 --no-signed</param>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行带签名控制的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --signed origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，并对推送进行签名。</para>
    /// <para>2. 示例：git push --no-signed origin dev</para>
    /// <para>说明：推送本地 dev 分支到 origin 远程仓库，不进行签名。</para>
    /// </remarks>
    public GitResult PushSigned(bool signed, params string[] args)
    {
        return Push(new[] { signed ? "--signed" : "--no-signed" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --signed=&lt;mode&gt; 命令，指定推送签名模式。
    /// <para>mode 可以是 true（强制签名）、false（不签名）、if-asked（仅当远程要求时签名）。</para>
    /// </summary>
    /// <param name="mode">签名模式，可选值为 "true"、"false"、"if-asked"</param>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行指定签名模式的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --signed=true origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，强制进行签名。</para>
    /// <para>2. 示例：git push --signed=if-asked origin dev</para>
    /// <para>说明：推送本地 dev 分支到 origin 远程仓库，仅当远程仓库要求时才进行签名。</para>
    /// </remarks>
    public GitResult PushSignedWithMode(string mode, params string[] args)
    {
        return Push(new[] { $"--signed={mode}" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --force-with-lease 命令，安全地强制推送，避免覆盖他人提交。
    /// <para>仅当远程引用当前指向预期值时才强制推送，防止意外覆盖未同步的远程提交。</para>
    /// </summary>
    /// <param name="refname">可选，指定要保护的远程引用名称（如 master）</param>
    /// <param name="expect">可选，指定远程引用的预期值（ commit ID），若 refname 存在则有效</param>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行带 lease 的强制推送的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --force-with-lease origin master</para>
    /// <para>说明：仅当 origin/master 指向本地已知的最新提交时，强制推送本地 master 分支到 origin。</para>
    /// <para>2. 示例：git push --force-with-lease=master:abc123 origin</para>
    /// <para>说明：仅当 origin/master 指向 commit abc123 时，强制推送本地 master 分支到 origin。</para>
    /// </remarks>
    public GitResult PushForceWithLease(string refname = "", string expect = "", params string[] args)
    {
        string leaseArg = "--force-with-lease";
        if (!string.IsNullOrEmpty(refname))
        {
            leaseArg += $"={refname}";
            if (!string.IsNullOrEmpty(expect))
                leaseArg += $":{expect}";
        }
        return Push(new[] { leaseArg }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --force-if-includes 命令，增强强制推送的安全性，确保本地包含远程所有提交。
    /// <para>强制推送前验证本地分支包含远程分支的所有提交，防止丢失远程提交。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行带 includes 检查的强制推送的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --force-if-includes origin master</para>
    /// <para>说明：强制推送本地 master 分支到 origin 远程仓库，前提是本地 master 包含 origin/master 的所有提交。</para>
    /// </remarks>
    public GitResult PushForceIfIncludes(params string[] args)
    {
        return Push(new[] { "--force-if-includes" }.Concat(args).ToArray());
    }

    /// <summary>
    /// 执行 git push --no-verify 命令，推送时跳过预推送钩子（pre-push）的验证。
    /// <para>预推送钩子通常用于执行代码检查等操作，该选项可临时跳过这些检查。</para>
    /// </summary>
    /// <param name="args">其他参数（如仓库地址、引用规范等）</param>
    /// <returns>执行跳过验证的 git push 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git push --no-verify origin master</para>
    /// <para>说明：推送本地 master 分支到 origin 远程仓库，跳过预推送钩子的验证操作。</para>
    /// <para>2. 示例：git push --no-verify</para>
    /// <para>说明：推送当前分支到默认远程仓库，跳过预推送钩子验证。</para>
    /// </remarks>
    public GitResult PushNoVerify(params string[] args)
    {
        return Push(new[] { "--no-verify" }.Concat(args).ToArray());
    }
}