using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xugege.DotnetExtension.Com.Dto;

namespace MyCompanyName.AbpZeroTemplate.Com.Helper
{
    public class EnumHelper
    {
        /// <summary>
        /// 获取枚举表示的name value列表
        /// </summary>
        /// <param name="eType">
        /// 枚举类型（必须是枚举）
        /// </param>
        /// <returns></returns>
        public static IList<NameValue<T>> GetNameValueList<T>(Type eType)
        {
            IList<NameValue<T>> result = new List<NameValue<T>>();
            foreach (var value in Enum.GetValues(eType))
            {
                string name = Enum.GetName(eType, value);
                result.Add(new NameValue<T>(name, (T)value));
            }
            return result;
        }
    }
}
