namespace LeoChen.GitHelper;

public partial class GitProcessHelper
{
    /// <summary>
    /// 执行 git fetch 命令，从一个或多个其他仓库下载对象和引用（分支、标签等），并更新远程跟踪分支。
    /// <para>默认情况下，会获取指向所感兴趣分支历史的标签，获取的引用及其指向的对象信息会写入.git/FETCH_HEAD。</para>
    /// <para>若未指定远程仓库，默认使用"origin"远程，除非当前分支配置了上游分支。</para>
    /// </summary>
    /// <param name="args">git fetch 命令的参数，包括选项、仓库、引用规范等</param>
    /// <returns>执行 git fetch 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git fetch</para>
    /// <para>说明：从默认的"origin"远程仓库获取数据，更新远程跟踪分支，默认获取指向感兴趣分支的标签，覆盖.git/FETCH_HEAD中的旧数据。</para>
    /// <para>2. 示例：git fetch origin</para>
    /// <para>说明：从名为"origin"的远程仓库获取数据，更新远程跟踪分支，默认行为同上。</para>
    /// <para>3. 示例：git fetch origin main</para>
    /// <para>说明：从"origin"远程仓库获取main分支的最新数据，更新对应的远程跟踪分支。</para>
    /// <para>4. 示例：git fetch --all</para>
    /// <para>说明：获取所有远程仓库的数据（除了配置了remote.&lt;name&gt;.skipFetchAll的远程），覆盖fetch.all配置。</para>
    /// <para>5. 示例：git fetch -a</para>
    /// <para>说明：使用--append选项，将获取的引用名称和对象名称追加到.git/FETCH_HEAD，而不是覆盖旧数据。</para>
    /// <para>6. 示例：git fetch --atomic</para>
    /// <para>说明：使用原子事务更新本地引用，要么所有引用都更新，要么出错时 none 引用更新。</para>
    /// <para>7. 示例：git fetch --depth=5</para>
    /// <para>说明：限制获取每个远程分支历史中从尖端开始的5个提交，若本地是浅仓库（通过git clone --depth创建），则调整历史深度为5个提交，不获取加深提交的标签。</para>
    /// <para>8. 示例：git fetch --deepen=3</para>
    /// <para>说明：与--depth类似，但从当前浅边界开始增加3个提交的历史深度，而非从远程分支尖端开始计算。</para>
    /// <para>9. 示例：git fetch --shallow-since=2024-01-01</para>
    /// <para>说明：调整浅仓库的历史，包含所有在2024-01-01之后可访问的提交。</para>
    /// <para>10. 示例：git fetch --shallow-exclude=origin/old-branch</para>
    /// <para>说明：调整浅仓库的历史，排除可从远程分支origin/old-branch访问的提交，该选项可多次指定。</para>
    /// <para>11. 示例：git fetch --unshallow</para>
    /// <para>说明：若源仓库是完整的，将浅仓库转换为完整仓库，移除浅仓库的限制；若源仓库是浅的，则尽可能获取数据，使本地仓库与源仓库历史一致。</para>
    /// <para>12. 示例：git fetch --update-shallow</para>
    /// <para>说明：默认情况下，从浅仓库获取时，git fetch 拒绝需要更新.git/shallow的引用，此选项允许更新.git/shallow并接受此类引用。</para>
    /// <para>13. 示例：git fetch --negotiation-tip=main</para>
    /// <para>说明：默认情况下，Git会向服务器报告所有本地引用可访问的提交以查找共同提交，此选项指定仅报告从main引用可访问的提交，以加快获取速度，可多次指定。</para>
    /// <para>14. 示例：git fetch --negotiate-only --negotiation-tip=dev</para>
    /// <para>说明：不从服务器获取任何数据，而是打印与服务器共同拥有的、由--negotiation-tip=dev指定的提交的祖先，与--recurse-submodules=yes/on-demand不兼容。</para>
    /// <para>15. 示例：git fetch --dry-run</para>
    /// <para>说明：显示将要执行的操作，但不实际进行任何更改。</para>
    /// <para>16. 示例：git fetch --porcelain</para>
    /// <para>说明：以易于解析的格式向标准输出打印结果，供脚本使用，与--recurse-submodules=yes/on-demand不兼容，且优先于fetch.output配置。</para>
    /// <para>17. 示例：git fetch --no-write-fetch-head</para>
    /// <para>说明：不将获取的远程引用列表写入$GIT_DIR下的FETCH_HEAD文件，默认是写入的。</para>
    /// <para>18. 示例：git fetch mygroup</para>
    /// <para>说明：从配置文件中remotes.mygroup条目指定的多个仓库获取数据。</para>
    /// <para>19. 示例：git fetch --multiple origin mygroup</para>
    /// <para>说明：从指定的多个仓库（origin和mygroup）获取数据。</para>
    /// </remarks>
    public GitResult Fetch(params string[] args)
    {
        return ExecuteCommand($"fetch {string.Join(" ", args)}");
    }

    /// <summary>
    /// 执行 git fetch --all 命令，获取所有远程仓库的数据（除了配置了remote.&lt;name&gt;.skipFetchAll的远程）。
    /// <para>此选项会覆盖fetch.all配置变量。</para>
    /// </summary>
    /// <returns>执行 git fetch --all 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --all</para>
    /// <para>说明：获取所有远程仓库的数据，忽略配置了skipFetchAll的远程，更新各远程对应的跟踪分支。</para>
    /// </remarks>
    public GitResult FetchAll()
    {
        return Fetch("--all");
    }

    /// <summary>
    /// 执行 git fetch --append 命令，将获取的引用名称和对象名称追加到.git/FETCH_HEAD，而非覆盖旧数据。
    /// <para>等效于使用 -a 选项。</para>
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --append 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --append</para>
    /// <para>说明：从默认远程获取数据，将结果追加到.git/FETCH_HEAD，保留之前的记录。</para>
    /// <para>2. 示例：git fetch --append origin feature-branch</para>
    /// <para>说明：从origin远程获取feature-branch分支数据，将结果追加到.git/FETCH_HEAD。</para>
    /// </remarks>
    public GitResult FetchAppend(params string[] args)
    {
        return Fetch("--append", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --atomic 命令，使用原子事务更新本地引用，确保要么所有引用都更新，要么出错时 none 引用更新。
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --atomic 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --atomic</para>
    /// <para>说明：从默认远程获取数据，以原子方式更新本地引用，保证数据一致性。</para>
    /// <para>2. 示例：git fetch --atomic origin main</para>
    /// <para>说明：从origin远程获取main分支数据，原子化更新对应的本地引用。</para>
    /// </remarks>
    public GitResult FetchAtomic(params string[] args)
    {
        return Fetch("--atomic", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --depth=&lt;depth&gt; 命令，限制获取远程分支历史中从尖端开始的指定数量的提交。
    /// <para>适用于浅仓库时，可加深或缩短历史深度，不获取加深提交的标签。</para>
    /// </summary>
    /// <param name="depth">要限制的提交数量</param>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --depth=&lt;depth&gt; 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --depth=10</para>
    /// <para>说明：从默认远程获取各分支历史中从尖端开始的10个提交，若为浅仓库则调整深度为10。</para>
    /// <para>2. 示例：git fetch --depth=5 origin dev</para>
    /// <para>说明：从origin远程获取dev分支历史中从尖端开始的5个提交。</para>
    /// </remarks>
    public GitResult FetchDepth(int depth, params string[] args)
    {
        return Fetch($"--depth={depth}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --deepen=&lt;depth&gt; 命令，从当前浅边界开始增加指定数量的提交历史深度。
    /// </summary>
    /// <param name="depth">从浅边界开始增加的提交数量</param>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --deepen=&lt;depth&gt; 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --deepen=3</para>
    /// <para>说明：从当前浅边界开始，增加3个提交的历史深度，扩展浅仓库的历史范围。</para>
    /// <para>2. 示例：git fetch --deepen=2 origin test</para>
    /// <para>说明：从origin远程的test分支，从当前浅边界开始增加2个提交的历史深度。</para>
    /// </remarks>
    public GitResult FetchDeepen(int depth, params string[] args)
    {
        return Fetch($"--deepen={depth}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --shallow-since=&lt;date&gt; 命令，调整浅仓库历史，包含所有指定日期之后的可访问提交。
    /// </summary>
    /// <param name="date">日期字符串，如"2024-01-01"</param>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --shallow-since=&lt;date&gt; 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --shallow-since=2023-06-01</para>
    /// <para>说明：调整浅仓库历史，包含2023年6月1日之后的所有可访问提交。</para>
    /// <para>2. 示例：git fetch --shallow-since=2024-03-15 origin main</para>
    /// <para>说明：从origin远程的main分支，获取2024年3月15日之后的提交，调整浅仓库历史。</para>
    /// </remarks>
    public GitResult FetchShallowSince(string date, params string[] args)
    {
        return Fetch($"--shallow-since={date}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --shallow-exclude=&lt;ref&gt; 命令，调整浅仓库历史，排除可从指定远程分支或标签访问的提交。
    /// <para>该选项可多次指定以排除多个引用。</para>
    /// </summary>
    /// <param name="reference">要排除的远程分支或标签，如"origin/old-tag"</param>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --shallow-exclude=&lt;ref&gt; 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --shallow-exclude=origin/legacy</para>
    /// <para>说明：调整浅仓库历史，排除可从origin/legacy分支访问的提交。</para>
    /// <para>2. 示例：git fetch --shallow-exclude=v1.0 --shallow-exclude=v2.0 origin</para>
    /// <para>说明：从origin远程获取数据，排除可从v1.0和v2.0标签访问的提交，调整浅仓库历史。</para>
    /// </remarks>
    public GitResult FetchShallowExclude(string reference, params string[] args)
    {
        return Fetch($"--shallow-exclude={reference}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --unshallow 命令，将浅仓库转换为完整仓库（若源仓库完整）或获取尽可能多的数据使本地与源仓库历史一致（若源仓库为浅仓库）。
    /// </summary>
    /// <param name="args">其他参数，如仓库等</param>
    /// <returns>执行 git fetch --unshallow 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --unshallow</para>
    /// <para>说明：若源仓库完整，将当前浅仓库转换为完整仓库，移除浅仓库限制。</para>
    /// <para>2. 示例：git fetch --unshallow origin</para>
    /// <para>说明：从origin远程获取数据，若origin是完整仓库，将本地浅仓库转为完整；若origin是浅仓库，使本地与origin历史一致。</para>
    /// </remarks>
    public GitResult FetchUnshallow(params string[] args)
    {
        return Fetch("--unshallow", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --update-shallow 命令，允许更新.git/shallow文件，接受需要更新该文件的引用。
    /// <para>默认情况下，从浅仓库获取时会拒绝此类引用。</para>
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --update-shallow 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --update-shallow</para>
    /// <para>说明：从默认远程获取数据，允许更新.git/shallow文件，接受需要该操作的引用。</para>
    /// <para>2. 示例：git fetch --update-shallow origin new-branch</para>
    /// <para>说明：从origin远程获取new-branch分支数据，允许更新.git/shallow以接受相关引用。</para>
    /// </remarks>
    public GitResult FetchUpdateShallow(params string[] args)
    {
        return Fetch("--update-shallow", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --negotiation-tip=&lt;commit|glob&gt; 命令，指定向服务器报告的提交范围以加快获取速度。
    /// <para>仅报告从指定引用（提交、分支等）可访问的提交，可多次指定。</para>
    /// </summary>
    /// <param name="tip">提交的SHA-1、引用或引用glob，如"main"、"abc123"、"refs/heads/*"</param>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --negotiation-tip=&lt;commit|glob&gt; 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --negotiation-tip=main</para>
    /// <para>说明：向服务器仅报告从main分支可访问的提交，加快从默认远程的获取速度。</para>
    /// <para>2. 示例：git fetch --negotiation-tip=dev --negotiation-tip=abc123 origin</para>
    /// <para>说明：从origin远程获取数据，向服务器报告从dev分支和abc123提交可访问的提交，优化数据传输。</para>
    /// </remarks>
    public GitResult FetchNegotiationTip(string tip, params string[] args)
    {
        return Fetch($"--negotiation-tip={tip}", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --negotiate-only 命令，不获取数据，仅打印与服务器共同拥有的、由--negotiation-tip指定的提交的祖先。
    /// <para>与--recurse-submodules=yes/on-demand不兼容。</para>
    /// </summary>
    /// <param name="tips">--negotiation-tip指定的提交或引用列表</param>
    /// <returns>执行 git fetch --negotiate-only 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --negotiate-only --negotiation-tip=main</para>
    /// <para>说明：不获取数据，仅打印与服务器共同拥有的、从main分支可访问的提交的祖先。</para>
    /// <para>2. 示例：git fetch --negotiate-only --negotiation-tip=dev --negotiation-tip=test origin</para>
    /// <para>说明：针对origin远程，不获取数据，打印与服务器共同拥有的、从dev和test分支可访问的提交的祖先。</para>
    /// </remarks>
    public GitResult FetchNegotiateOnly(params string[] tips)
    {
        List<string> args = new List<string> { "--negotiate-only" };
        args.AddRange(tips);
        return Fetch(args.ToArray());
    }

    /// <summary>
    /// 执行 git fetch --dry-run 命令，显示将要执行的操作，但不实际修改任何数据。
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --dry-run 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --dry-run</para>
    /// <para>说明：显示从默认远程获取数据时将要执行的操作，不实际获取或更新任何内容。</para>
    /// <para>2. 示例：git fetch --dry-run origin feature</para>
    /// <para>说明：显示从origin远程获取feature分支时的操作计划，不实际执行。</para>
    /// </remarks>
    public GitResult FetchDryRun(params string[] args)
    {
        return Fetch("--dry-run", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --porcelain 命令，以易于脚本解析的格式输出结果。
    /// <para>与--recurse-submodules=yes/on-demand不兼容，优先于fetch.output配置。</para>
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --porcelain 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --porcelain</para>
    /// <para>说明：从默认远程获取数据，以易于解析的格式输出结果，适合脚本处理。</para>
    /// <para>2. 示例：git fetch --porcelain origin main</para>
    /// <para>说明：从origin远程获取main分支数据，以结构化格式输出结果，便于脚本解析。</para>
    /// </remarks>
    public GitResult FetchPorcelain(params string[] args)
    {
        return Fetch("--porcelain", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --no-write-fetch-head 命令，不将获取的远程引用列表写入FETCH_HEAD文件。
    /// <para>默认情况下会写入该文件。</para>
    /// </summary>
    /// <param name="args">其他参数，如仓库、引用规范等</param>
    /// <returns>执行 git fetch --no-write-fetch-head 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无额外参数示例：git fetch --no-write-fetch-head</para>
    /// <para>说明：从默认远程获取数据，但不更新.git/FETCH_HEAD文件。</para>
    /// <para>2. 示例：git fetch --no-write-fetch-head origin test</para>
    /// <para>说明：从origin远程获取test分支数据，不将结果写入FETCH_HEAD文件。</para>
    /// </remarks>
    public GitResult FetchNoWriteFetchHead(params string[] args)
    {
        return Fetch("--no-write-fetch-head", string.Join(" ", args));
    }

    /// <summary>
    /// 执行 git fetch --multiple 命令，从多个指定的仓库或仓库组获取数据。
    /// </summary>
    /// <param name="repositoriesOrGroups">要获取数据的仓库或仓库组列表</param>
    /// <returns>执行 git fetch --multiple 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git fetch --multiple origin mygroup</para>
    /// <para>说明：从origin仓库和mygroup仓库组获取数据。</para>
    /// <para>2. 示例：git fetch --multiple repo1 repo2</para>
    /// <para>说明：从repo1和repo2两个远程仓库获取数据。</para>
    /// </remarks>
    public GitResult FetchMultiple(params string[] repositoriesOrGroups)
    {
        return Fetch("--multiple", string.Join(" ", repositoriesOrGroups));
    }
}