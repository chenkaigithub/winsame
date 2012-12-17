using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winsame
{
    #region StringFunction
    /// <summary>
    /// 字符串处理类
    /// </summary>
    public static class StringFunctions
    {
        /// <summary>
        /// 将形如[][][]的文件名切割成字符串数组
        /// </summary>
        /// <param name="input">输入的完整文件名</param>
        /// <returns></returns>
        public static string[] mysplit(string input)
        {
            string[] now = new string[7];
            int pos = 0;
            for (int i = 0; i < 7; ++i)
            {
                pos++;
                while (input[pos] != ']') now[i] = now[i] + input[pos++];
                pos++;
            }
            return now;
        }
        /// <summary>
        /// 将形如"xxx-yyy"的字符串分割成两个字符串
        /// </summary>
        /// <param name="input">输入的待分割字符串</param>
        /// <returns></returns>
        public static string[] splitagain(string input)
        {
            return input.Split('-');
        }
        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="time">输入的字符串</param>
        /// <returns></returns>
        public static int stringtoint(string time)
        {
            int pos = 0, ans = 0;
            while (Char.IsNumber(time[pos]))
            {
                ans = ans * 10 + time[pos++];
            }
            return ans;
        }
        /// <summary>
        /// 删除字符串空白字符
        /// 有利于【修改缩进】类雷同的识别
        /// </summary>
        /// <param name="text">输入的待删除文本</param>
        /// <returns>删除后的结果</returns>
        public static string deleteEmpty(string text)
        {
            string temp = text;
            for (char c = (char)0; c < (char)256; ++c)
                if (Char.IsWhiteSpace(c)) temp = temp.Replace(c + "", "");
            return temp;
        }
        /// <summary>
        /// 计算两个文本的相似度
        /// 使用编辑距离算法
        /// 更利于找出修改变量名的雷同方法
        /// </summary>
        /// <param name="text1">第一个文本</param>
        /// <param name="text2">第二个文本</param>
        /// <returns>计算后的相似度，在[0..1]范围内</returns>
        public static double calstringsim(string text1, string text2)
        {
            int len1 = text1.Length;
            int len2 = text2.Length;
            int[,] dp = new int[len1, len2];
            for (int i = 0; i < len1; ++i)
                for (int j = 0; j < len2; ++j)
                {
                    dp[i, j] = len1 + len2;
                    if (i == 0 && j == 0)
                    {
                        dp[i, j] = 0;
                    }
                    else
                    {
                        if (i > 0) dp[i, j] = Math.Min(dp[i - 1, j] + 1, dp[i, j]);
                        if (j > 0) dp[i, j] = Math.Min(dp[i, j - 1] + 1, dp[i, j]);
                        if (i > 0 && j > 0)
                        {
                            if (text1[i] != text2[j]) dp[i, j] = Math.Min(dp[i - 1, j - 1] + 1, dp[i, j]);
                            else dp[i, j] = Math.Min(dp[i - 1, j - 1], dp[i, j]);
                        }
                    }
                }
            double dif = 2 * dp[len1 - 1, len2 - 1];
            dif /= (double)(len1 + len2);
            return 1 - dif;
        }
        /// <summary>
        /// 散列Hash方法
        /// </summary>
        /// <param name="text">待hash的文本</param>
        /// <param name="q">步长</param>
        /// <returns>散列hash数组</returns>
        private static long[] gethash(string text, int q)
        {
            int len = text.Length;
            long[] res = new long[len - q + 1];
            for (int i = 0; i < q; ++i)
                res[0] = (res[0] * (long)2 + (long)text[i]) % Consts.PRIME;
            for (int i = q; i < len; ++i)
                res[i - q + 1] = ((res[i - q] - (long)text[i - q] * (long)(1 << (q-1))) * (long)2 + (long)text[i]) % Consts.PRIME;
            return res;
        }
        /// <summary>
        /// 计算两个文本的相似度
        /// 使用串的散列值匹配算法
        /// 更利于找出重排函数位置的雷同方法
        /// </summary>
        /// <param name="text1">第一个文本</param>
        /// <param name="text2">第二个文本</param>
        /// <param name="p">设置步长，默认为10，推荐5-10为宜</param>
        /// <returns>计算后的相似度，在[0..1]范围内</returns>
        public static double calstringsim2(string text1, string text2, int q = 10)
        {
            long[] x = gethash(text1, q);
            long[] y = gethash(text2, q);
            int lenx = x.Length;
            int leny = y.Length;
            int totsame = 0;
            for (int locx = 0; locx < lenx; ++locx)
            {
                int nowleny = 0;
                for (int locy = 0; locy < leny; ++locy)
                    if (x[locx] == y[locy])
                    {
                        ++totsame;
                        locx++;
                        if (locx == lenx) break;
                    }
                    else
                    {
                        y[nowleny++] = y[locy];
                    }
                leny = nowleny;
            }
            return (double)(totsame * 2) / (double)(lenx + leny);
        }
    }
    #endregion
}
