using System.Text;

public class AlipayUtil
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Obscure(string content)
    {
        var buf = new StringBuilder(content);
        if (buf.Length > 10)
        {
            buf.Insert(10, 'M');
        }

        if (buf.Length > 20)
        {
            buf.Insert(20, 'N');
        }
        return buf.ToString();
    }
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Restore(string content)
    {
        var buf = new StringBuilder(content);

        if (buf.Length > 20)
        {
            buf.Remove(20, 1);
        }
        if (buf.Length > 10)
        {
            buf.Remove(10, 1);
        }
        return buf.ToString();
    }
}
