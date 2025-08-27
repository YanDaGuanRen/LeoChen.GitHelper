namespace LeoChen.GitHelper;

public partial class GitProcessHelper
{
    /// <summary>
    /// 执行 git merge 命令，用于将两个或多个开发历史合并在一起。
    /// <para>该命令可将指定提交（自与当前分支历史分叉以来的提交）的更改合并到当前分支。</para>
    /// <para>常用于将其他分支的更改整合到当前工作分支，也被 git pull 用于合并远程仓库的更改。</para>
    /// </summary>
    /// <param name="args">git merge 命令的参数列表，如提交、分支名或各种选项</param>
    /// <returns>执行 git merge 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git merge</para>
    /// <para>说明：通常不直接使用无参数形式，需指定要合并的提交或分支，否则可能报错。</para>
    /// <para>2. 示例：git merge topic</para>
    /// <para>说明：将 topic 分支自与当前分支（如 master）分叉以来的更改（如 E 到 C 的提交）合并到当前分支，并创建合并提交。</para>
    /// <para>3. 示例：git merge --no-commit topic</para>
    /// <para>说明：执行合并但不自动创建合并提交，让用户检查和调整合并结果后再手动提交。</para>
    /// <para>4. 示例：git merge --commit topic</para>
    /// <para>说明：覆盖 --no-commit，执行合并并自动创建合并提交（默认行为）。</para>
    /// <para>5. 示例：git merge -e topic</para>
    /// <para>说明：合并 topic 分支后，调用编辑器让用户进一步编辑自动生成的合并消息。</para>
    /// <para>6. 示例：git merge --no-edit topic</para>
    /// <para>说明：合并 topic 分支时，接受自动生成的合并消息，不打开编辑器。</para>
    /// <para>7. 示例：git merge --cleanup=scissors topic</para>
    /// <para>说明：合并时，在合并消息（MERGE_MSG）后附加分隔线，用于清理提交消息，在合并冲突时生效。</para>
    /// <para>8. 示例：git merge --ff topic</para>
    /// <para>说明：当 topic 分支是当前分支的后代时，以快进方式合并（仅更新分支指针，不创建合并提交）；否则创建合并提交。</para>
    /// <para>9. 示例：git merge --no-ff topic</para>
    /// <para>说明：无论是否可快进，都创建合并提交，保留合并历史。</para>
    /// <para>10. 示例：git merge --ff-only topic</para>
    /// <para>说明：仅当可以快进合并时执行合并，否则拒绝合并并返回非零状态。</para>
    /// <para>11. 示例：git merge -S topic</para>
    /// <para>说明：使用默认提交者身份的 GPG 密钥对合并提交进行签名。</para>
    /// <para>12. 示例：git merge --gpg-sign=ABC123 topic</para>
    /// <para>说明：使用指定密钥 ID（ABC123）对合并提交进行 GPG 签名。</para>
    /// <para>13. 示例：git merge --no-gpg-sign topic</para>
    /// <para>说明：不对合并提交进行 GPG 签名，覆盖配置中的 commit.gpgSign 或之前的 --gpg-sign 选项。</para>
    /// <para>14. 示例：git merge --log=5 topic</para>
    /// <para>说明：在合并消息中除分支名外，添加最多 5 个被合并提交的单行描述。</para>
    /// <para>15. 示例：git merge --no-log topic</para>
    /// <para>说明：不在合并消息中包含被合并提交的描述。</para>
    /// <para>16. 示例：git merge -n topic</para>
    /// <para>说明：合并时不显示合并的提交信息摘要。</para>
    /// <para>17. 示例：git merge --stat topic</para>
    /// <para>说明：合并后显示文件更改的统计信息。</para>
    /// <para>18. 示例：git merge --no-verify topic</para>
    /// <para>说明：合并时跳过 pre-commit 和 commit-msg 钩子的验证。</para>
    /// <para>19. 示例：git merge -s recursive topic</para>
    /// <para>说明：使用 recursive 合并策略进行合并（适用于合并两个分支）。</para>
    /// <para>20. 示例：git merge -X ours topic</para>
    /// <para>说明：使用合并策略选项 ours，在冲突时优先采用当前分支的更改。</para>
    /// <para>21. 示例：git merge --allow-unrelated-histories topic</para>
    /// <para>说明：允许合并历史不相关的分支（如两个独立创建的仓库的分支）。</para>
    /// <para>22. 示例：git merge --no-allow-unrelated-histories topic</para>
    /// <para>说明：拒绝合并历史不相关的分支（默认行为）。</para>
    /// <para>23. 示例：git merge --rerere-autoupdate topic</para>
    /// <para>说明：在合并时，自动应用 rerere（重用记录的冲突解决方案）的结果来更新工作树文件。</para>
    /// <para>24. 示例：git merge --no-rerere-autoupdate topic</para>
    /// <para>说明：合并时不自动应用 rerere 的结果，即使有记录的冲突解决方案。</para>
    /// <para>25. 示例：git merge -m "Merge topic into master" topic</para>
    /// <para>说明：合并 topic 分支，使用指定的消息 "Merge topic into master" 作为合并提交消息。</para>
    /// <para>26. 示例：git merge -F merge-message.txt topic</para>
    /// <para>说明：从文件 merge-message.txt 中读取合并提交消息。</para>
    /// <para>27. 示例：git merge --into-name new-branch topic</para>
    /// <para>说明：将合并结果放入新分支 new-branch 而非当前分支（较少使用）。</para>
    /// <para>28. 示例：git merge --continue</para>
    /// <para>说明：在解决合并冲突后，继续完成合并过程并创建合并提交。</para>
    /// <para>29. 示例：git merge --abort</para>
    /// <para>说明：中止当前合并过程，尝试恢复到合并前的状态（注意：有未提交更改时可能无法完全恢复）。</para>
    /// <para>30. 示例：git merge --quit</para>
    /// <para>说明：退出合并状态，但不恢复工作树，用于放弃合并但保留已做的更改。</para>
    /// </remarks>
    public GitResult Merge(params string[] args)
    {
        return ExecuteCommand($"merge {string.Join(" ", args)}");
    }

    /// <summary>
    /// 执行 git merge 命令并指定要合并的提交或分支
    /// <para>将指定的提交（自与当前分支分叉以来）的更改合并到当前分支。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge topic</para>
    /// <para>说明：将 topic 分支的更改合并到当前分支，自动创建合并提交（若需）。</para>
    /// <para>2. 示例：git merge abc1234</para>
    /// <para>说明：将提交 abc1234 及其以来的更改合并到当前分支。</para>
    /// </remarks>
    public GitResult Merge(string commit)
    {
        return Merge(commit);
    }

    /// <summary>
    /// 执行 git merge 命令，合并后不自动提交
    /// <para>执行合并操作，但在合并完成后不创建合并提交，允许用户检查和调整结果。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --no-commit 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --no-commit topic</para>
    /// <para>说明：合并 topic 分支到当前分支，不自动提交，用户可检查后手动提交。</para>
    /// </remarks>
    public GitResult MergeWithNoCommit(string commit)
    {
        return Merge("--no-commit", commit);
    }

    /// <summary>
    /// 执行 git merge 命令，强制创建合并提交（不使用快进）
    /// <para>无论是否可以快进合并，都创建合并提交，保留合并历史记录。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --no-ff 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --no-ff topic</para>
    /// <para>说明：合并 topic 分支，即使可快进也创建合并提交，清晰保留合并历史。</para>
    /// </remarks>
    public GitResult MergeWithNoFastForward(string commit)
    {
        return Merge("--no-ff", commit);
    }

    /// <summary>
    /// 执行 git merge 命令，仅允许快进合并
    /// <para>仅当被合并的历史是当前分支的后代时才执行快进合并，否则拒绝合并。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --ff-only 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --ff-only topic</para>
    /// <para>说明：若 topic 分支是当前分支的后代，则快进合并；否则拒绝合并。</para>
    /// </remarks>
    public GitResult MergeWithFastForwardOnly(string commit)
    {
        return Merge("--ff-only", commit);
    }

    /// <summary>
    /// 执行 git merge 命令并编辑合并提交消息
    /// <para>合并完成后，调用编辑器让用户修改自动生成的合并提交消息。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --edit 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --edit topic</para>
    /// <para>说明：合并 topic 分支后，打开编辑器让用户编辑合并提交消息。</para>
    /// </remarks>
    public GitResult MergeWithEdit(string commit)
    {
        return Merge("--edit", commit);
    }

    /// <summary>
    /// 执行 git merge 命令，不编辑合并提交消息
    /// <para>合并完成后，直接使用自动生成的合并提交消息，不打开编辑器。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --no-edit 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --no-edit topic</para>
    /// <para>说明：合并 topic 分支，使用自动生成的消息作为合并提交消息，不打开编辑器。</para>
    /// </remarks>
    public GitResult MergeWithNoEdit(string commit)
    {
        return Merge("--no-edit", commit);
    }

    /// <summary>
    /// 执行 git merge --continue 命令，继续完成合并
    /// <para>在解决合并冲突后，继续合并过程并创建合并提交。</para>
    /// </summary>
    /// <returns>执行 git merge --continue 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git merge --continue</para>
    /// <para>说明：解决合并冲突并提交更改后，执行此命令完成合并。</para>
    /// </remarks>
    public GitResult MergeContinue()
    {
        return Merge("--continue");
    }

    /// <summary>
    /// 执行 git merge --abort 命令，中止合并
    /// <para>中止当前合并过程，尝试恢复到合并前的状态。</para>
    /// </summary>
    /// <returns>执行 git merge --abort 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git merge --abort</para>
    /// <para>说明：放弃当前合并，尝试恢复到合并开始前的状态（注意：未提交更改可能影响恢复）。</para>
    /// </remarks>
    public GitResult MergeAbort()
    {
        return Merge("--abort");
    }

    /// <summary>
    /// 执行 git merge --quit 命令，退出合并状态
    /// <para>退出合并状态，但不恢复工作树，保留已做的更改。</para>
    /// </summary>
    /// <returns>执行 git merge --quit 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 无参数示例：git merge --quit</para>
    /// <para>说明：退出合并状态，不恢复工作树，适用于想保留更改但放弃继续合并的场景。</para>
    /// </remarks>
    public GitResult MergeQuit()
    {
        return Merge("--quit");
    }

    /// <summary>
    /// 执行 git merge 命令，允许合并不相关历史
    /// <para>强制合并历史不相关的分支（如两个独立仓库的分支）。</para>
    /// </summary>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge --allow-unrelated-histories 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge --allow-unrelated-histories other-repo-branch</para>
    /// <para>说明：合并历史不相关的 other-repo-branch 分支到当前分支。</para>
    /// </remarks>
    public GitResult MergeAllowUnrelatedHistories(string commit)
    {
        return Merge("--allow-unrelated-histories", commit);
    }

    /// <summary>
    /// 执行 git merge 命令并指定合并消息
    /// <para>合并分支时，使用指定的消息作为合并提交消息。</para>
    /// </summary>
    /// <param name="message">合并提交的消息</param>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge -m 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge -m "Merge feature branch" feature</para>
    /// <para>说明：合并 feature 分支，使用 "Merge feature branch" 作为合并提交消息。</para>
    /// </remarks>
    public GitResult MergeWithMessage(string message, string commit)
    {
        return Merge("-m", message, commit);
    }

    /// <summary>
    /// 执行 git merge 命令，从文件读取合并消息
    /// <para>合并分支时，从指定文件中读取内容作为合并提交消息。</para>
    /// </summary>
    /// <param name="filePath">包含合并消息的文件路径</param>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge -F 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge -F merge-msg.txt topic</para>
    /// <para>说明：合并 topic 分支，从 merge-msg.txt 文件中读取合并提交消息。</para>
    /// </remarks>
    public GitResult MergeWithMessageFile(string filePath, string commit)
    {
        return Merge("-F", filePath, commit);
    }

    /// <summary>
    /// 执行 git merge 命令，使用指定的合并策略
    /// <para>指定合并时采用的策略（如 recursive、octopus 等）进行合并操作。</para>
    /// </summary>
    /// <param name="strategy">合并策略，如 "recursive"、"octopus" 等</param>
    /// <param name="commits">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge -s 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge -s recursive topic</para>
    /// <para>说明：使用 recursive 策略合并 topic 分支（适用于两个分支的合并）。</para>
    /// <para>2. 示例：git merge -s octopus branch1 branch2</para>
    /// <para>说明：使用 octopus 策略合并多个分支（branch1、branch2）到当前分支。</para>
    /// </remarks>
    public GitResult MergeWithStrategy(string strategy, params string[] commits)
    {
        var args = new List<string> { "-s", strategy };
        args.AddRange(commits);
        return Merge(args.ToArray());
    }

    /// <summary>
    /// 执行 git merge 命令，使用指定的策略选项
    /// <para>为合并策略指定额外的选项（如 ours、theirs 等），处理冲突时使用。</para>
    /// </summary>
    /// <param name="strategyOption">策略选项，如 "ours"、"theirs" 等</param>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge -X 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge -X ours topic</para>
    /// <para>说明：合并 topic 分支时，冲突部分采用当前分支（our）的更改。</para>
    /// <para>2. 示例：git merge -X theirs topic</para>
    /// <para>说明：合并 topic 分支时，冲突部分采用 topic 分支（their）的更改。</para>
    /// </remarks>
    public GitResult MergeWithStrategyOption(string strategyOption, string commit)
    {
        return Merge("-X", strategyOption, commit);
    }

    /// <summary>
    /// 执行 git merge 命令，对合并提交进行 GPG 签名
    /// <para>使用指定的 GPG 密钥对合并提交进行签名，确保提交的真实性。</para>
    /// </summary>
    /// <param name="keyId">GPG 密钥 ID（可选，默认使用提交者身份的密钥）</param>
    /// <param name="commit">要合并的提交哈希或分支名</param>
    /// <returns>执行 git merge -S 或 --gpg-sign 命令的结果 <seealso cref="GitResult"/></returns>
    /// <remarks>
    /// <para>使用示例及说明：</para>
    /// <para>1. 示例：git merge -S topic</para>
    /// <para>说明：合并 topic 分支，使用默认 GPG 密钥对合并提交签名。</para>
    /// <para>2. 示例：git merge --gpg-sign=XYZ789 topic</para>
    /// <para>说明：合并 topic 分支，使用密钥 ID XYZ789 对合并提交签名。</para>
    /// </remarks>
    public GitResult MergeWithGpgSign(string commit, string keyId = "")
    {
        if (string.IsNullOrEmpty(keyId))
        {
            return Merge("-S", commit);
        }
        return Merge($"--gpg-sign={keyId}", commit);
    }
}