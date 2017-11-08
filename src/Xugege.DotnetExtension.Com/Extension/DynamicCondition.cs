namespace Xugege.DotnetExtension.Com.Extension
{
    public class DynamicCondition
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Exp { get; set; }
    }


    public class DynamicConditionExpression
    {
        /// <summary>
        /// 等于
        /// </summary>
        public const string Eq = "=";
        public const string Bw = "d";
        /// <summary>
        /// 大于
        /// </summary>
        public const string Gt = ">";
        /// <summary>
        /// 小于
        /// </summary>
        public const string Lt = "<";
        /// <summary>
        /// 大于等于
        /// </summary>
        public const string GtEq = ">=";
        /// <summary>
        /// 小于等于
        /// </summary>
        public const string LtEq = "<=";
        /// <summary>
        /// like
        /// </summary>
        public const string Like = "%";

        public const string In = "in";
    }
}
