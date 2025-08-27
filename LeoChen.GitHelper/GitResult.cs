namespace LeoChen.GitHelper;

public class GitResult
{
    /// <summary>
    /// 执行的命令
    /// </summary>
    public string? Command { get; set; }

    /// <summary>
    /// 命令输出结果
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 退出代码
    /// </summary>
    public int ExitCode { get; set; }

    /// <summary>
    /// 是否执行成功
    /// </summary>
    public bool Success => ExitCode == 0;
}
