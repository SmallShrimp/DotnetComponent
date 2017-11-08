using System;
using System.Linq;
using Xugege.DotnetExtension.Com.Extension;

namespace Xugege.DotnetExtension.Com.Helper
{

    /// <summary>
    /// 生成顺序码助手类 
    /// </summary>
    public class CodeHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="rule">规则：3-3-2</param>
        /// <param name="prefix">前缀:如果传递了前缀，origin应该要包含前缀</param>
        /// <returns></returns>
        public static string Get(string origin, string rule = "3-3-2", string prefix = "")
        {

            if (!prefix.IsNullOrEmpty()) origin = origin.Replace(prefix, "").Trim();

            var rules = rule.Split('-').ToList().Select(int.Parse).ToList();

            if (origin.IsNullOrEmpty())
            {
                origin = "0".PadLeft(rules[0], '0');
            }

            long originLong = 0;
            if (!long.TryParse(origin, out originLong))
            {
                return string.Empty;
            }

            int length = origin.Length;
            if (length > rules.Sum())
            {
                throw new Exception("传入字符串不符合规则");
            }

            int level = 1;
            int levelLength = 0;
            for (int i = 0; i < rules.Count; i++)
            {
                levelLength = levelLength + rules[i];
                if (length == levelLength)
                {
                    level = i + 1;
                    break;
                }
            }

            //int level = GetCodeLevel(origin, rule);


            string sub = origin.Substring(levelLength - rules[level - 1]);
            string result = prefix + origin.Substring(0, levelLength - rules[level - 1]) +
                GenerateOrder(long.Parse(sub), rules[level - 1]);
            return result;
        }

        public static int GetCodeLevel(string code, string rule = "3-3-3")
        {
            int length = code.Length;
            var rules = rule.Split('-').ToList().Select(int.Parse).ToList();
            if (length > rules.Sum())
            {
                throw new Exception("传入字符串不符合规则");
            }
            int level = 1;
            int levelLength = 0;
            for (int i = 0; i < rules.Count; i++)
            {
                levelLength = levelLength + rules[i];
                if (length == levelLength)
                {
                    level = i + 1;
                    break;
                }
            }
            return level;
        }

        /// <summary>
        /// 生成顺序号
        /// </summary>
        /// <param name="curOrder">当前顺序</param>
        /// <param name="pos">位数</param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        private static string GenerateOrder(long curOrder, int pos = 4, string prefix = "")
        {

            long order = curOrder + 1;

            long max = long.Parse("9".PadLeft(pos, '9'));
            if (order > max)
            {
                throw new Exception("操作规则范围最大值");
            }

            var orderString = order.ToString();
            return prefix + orderString.PadLeft(pos, '0');
        }
    }
}
