
namespace LeoChen.GitHelper;

public static class GitCommandHelper
{
    /// <summary>
    /// 克隆
    /// </summary>
    /// <param name="cmd">Cmd帮助类</param>
    /// <param name="url">Git地址</param>
    /// <returns></returns>
    public static void GitClone(this CmdExecute cmd, string url)
    {
        cmd.Execute("git clone " + url);
    }
    /// <summary>
    /// 克隆
    /// </summary>
    /// <param name="path">Git路径</param>
    /// <param name="url">Git地址</param>
    /// <returns></returns>
    public static void GitClone(this string path, string url)
    {
        path = path.GetFullPath().EnsureDirectory();
        CmdExecute.Execute("git clone " + url, path);
    }

    /// <summary>
    /// 获取所有分支
    /// </summary>
    /// <param name="cmd">Cmd帮助类</param>
    /// <param name="command">参数</param>
    /// <returns></returns>
    public static string[] GitBranch(this CmdExecute cmd,string command="")
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = cmd.Execute("git branch" + com);
        return result.Output.Replace("*", "")
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .Select(branch => branch.Trim())
        .Where(branch => !string.IsNullOrEmpty(branch))
        .ToArray();
    }
    /// <summary>
    /// 获取所有分支
    /// </summary>
    /// <param name="path">Git路径</param>
    /// <param name="command">参数</param>
    /// <returns></returns>
    public static string[] GitBranch(this string path, string command = "")
    {
        var com = "";
        path = path.GetFullPath().EnsureDirectory();
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = CmdExecute.Execute("git branch" + com, path);
        return result.Output.Replace("*", "")
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .Select(branch => branch.Trim())
        .Where(branch => !string.IsNullOrEmpty(branch))
        .ToArray();
    }

    /// <summary>
    /// 获取所有远程仓库
    /// </summary>
    /// <param name="cmd">Cmd帮助类</param>
    /// <param name="command">参数</param>
    /// <returns></returns>
    public static string[] GitRemote(this CmdExecute cmd, string command = "")
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = cmd.Execute("git remote" + com);
        return result.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); ;
    }

    /// <summary>
    /// 获取所有远程仓库
    /// </summary>
    /// <param name="path">Git路径</param>
    /// <param name="command">参数</param>
    /// <returns></returns>
    public static string[] GitRemote(this string path, string command = "")
    {
        var com = "";
        path = path.GetFullPath().EnsureDirectory();
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = CmdExecute.Execute("git remote" + com, path);
        return result.Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); ;
    }


    /// <summary>
    /// 获取远程仓库
    /// </summary>
    /// <param name="cmd">Cmd帮助类</param>
    /// <param name="command">仓库名称 也可以后面加空格再加分支 </param>
    /// <param name="output">输出信息</param>
    /// <returns></returns>
    public static bool GitFetch(this CmdExecute cmd, string command,out string output)
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = cmd.Execute("git fetch" + com);
        output = result.Output;
        return true;

    }
    /// <summary>
    /// 获取远程仓库
    /// </summary>
    /// <param name="path">Git路径</param>
    /// <param name="command">仓库名称 也可以后面加空格再加分支 </param>
    /// <returns></returns>
    public static CmdResult GitFetch(this string path, string command)
    {
        path = path.GetFullPath().EnsureDirectory();
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }

        return CmdExecute.Execute("git fetch" + com, path);

    }

    /// <summary>
    /// 切换分支
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static bool GitCheckOut(this CmdExecute cmd, string command)
    {

        var result = cmd.Execute("git checkout " + command);
        return true;

    }

    /// <summary>
    /// 切换分支
    /// </summary>
    /// <param name="path">Git路径</param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitCheckOut(this string path, string command)
    {

        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git checkout " + command, path);

    }
    //git merge %UPSTREAM%/%UPSTREAM_BRANCH% --no-edit

    /// <summary>
    /// 合并分支
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static bool GitMerge(this CmdExecute cmd, string command)
    {

        var result = cmd.Execute("git merge " + command);
        return true;

    }


    /// <summary>
    /// 合并分支
    /// </summary>
    /// <param name="path"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitMerge(this string path, string command)
    {

        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git merge " + command, path);

    }

    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static bool GitCommit(this CmdExecute cmd, string command)
    {

        var result = cmd.Execute("git commit " + command);
        return true;

    }
    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="path"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitCommit(this string path, string command)
    {

        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git commit " + command, path);

    }

    /// <summary>
    /// 推送
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitPush(this CmdExecute cmd, string command="")
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        return cmd.Execute("git push " + com);

    }
    /// <summary>
    /// 推送
    /// </summary>
    /// <param name="path"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitPush(this string path, string command="")
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git push" + com, path);
    }

    /// <summary>
    /// 拉取
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitPull(this CmdExecute cmd, string command = "")
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        var result = cmd.Execute("git pull" + com);

        return result;
    }

    /// <summary>
    /// 拉取
    /// </summary>
    /// <param name="path"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CmdResult GitPull(this string path, string command)
    {
        var com = "";
        if (!string.IsNullOrEmpty(command))
        {
            com = " " + command;
        }
        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git pull" + com, path);
    }

    /// <summary>
    /// 获取当前分支
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static CmdResult GitCurrentBranch(this string path)
    {

        path = path.GetFullPath().EnsureDirectory();
        return CmdExecute.Execute("git rev-parse --abbrev-ref HEAD",path);
    }

    /// <summary>
    /// 获取当前分支
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public static CmdResult GitCurrentBranch(this CmdExecute cmd)
    {
        return cmd.Execute("git rev-parse --abbrev-ref HEAD");
    }

}

