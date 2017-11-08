using System.Collections.Generic;

namespace MyCompanyName.AbpZeroTemplate.Com.Helper
{
    public class ArrayParamReader
    {

        /// <summary>
        /// 读取参数 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IDictionary<string, string> Read(string[] args)
        {
            Dictionary<string, string> _params = new Dictionary<string, string>();
            for (var i = 0; i < args.Length; i++)
            {
                var p = args[i];
                if (p.StartsWith("--"))
                {
                    //参数
                    _params.Add(p.Replace("--", ""),
                        ((i < args.Length - 1) && !args[i + 1].StartsWith("--")) ? args[i + 1] : "TRUE");
                }
            }
            return _params;
        }
    }
}
