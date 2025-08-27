using System.Collections;
using System.Text;

namespace LeoChen.GitHelper;

public static class Utils
{
    /// <summary>把一个列表组合成为一个字符串，默认逗号分隔</summary>
    /// <param name="value"></param>
    /// <param name="separator">组合分隔符，默认逗号</param>
    /// <returns></returns>
    public static String Join(this IEnumerable value, String separator = ",")
    {
        if (value is null) return "";

        var sb = new StringBuilder();

            foreach (var item in value)
            {
                sb.Append(item + "");
            }
        
        return sb.ToString();
    }
}