using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xugege.DotnetExtension.Com.Extension.TypeExtend;

namespace Xugege.DotnetExtension.Com.Extension
{
    public static class QueryableExtend
    {
        public static IQueryable<TSource> WhereDynamic<TSource>(this IQueryable<TSource> source,
            IList<DynamicCondition> conditions) where TSource : class
        {
            if (conditions.Count > 0)
            {
                //构建 p=>Body中的c
                ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
                //构建p=>Body中的Body
                var body = GetExpressoinBody(param, conditions);
                if (body != null)
                {
                    //将二者拼为p=>Body
                    var expression = Expression.Lambda<Func<TSource, bool>>(body, param);
                    //传到Where中当做参数，类型为Expression<Func<T,bool>>
                    return source.Where(expression);
                }
            }
            return source;
        }

        //public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, DataPage dp) where TSource : class
        //{
        //    dp.RowCount = source.Count();
        //    Type type = typeof(TSource);
        //    ParameterExpression param = Expression.Parameter(type, "c");
        //    string callMethod = "OrderByDescending";
        //    PropertyInfo property;
        //    if (dp.OrderField == null)
        //        property = type.GetRuntimeProperties().First();
        //    else
        //    {
        //        //处理正反排序
        //        string[] orderFileds = dp.OrderField.Split(' ');
        //        if (orderFileds.Length == 2)
        //        {
        //            dp.OrderField = orderFileds[0].Trim();
        //            if (String.Compare(orderFileds[1].Trim(), "asc", StringComparison.OrdinalIgnoreCase) == 0) callMethod = "OrderBy";
        //        }
        //        property = type.GetProperty(dp.OrderField) ?? type.GetRuntimeProperties().First();
        //    }
        //    LambdaExpression le = Expression.Lambda(Expression.MakeMemberAccess(param, property), param);
        //    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), callMethod, new[] { type, property.PropertyType }
        //    , source.Expression, Expression.Quote(le));
        //    return source.Provider.CreateQuery<TSource>(resultExp).Skip((dp.PageIndex - 1) * dp.PageSize).Take(dp.PageSize);
        //}

        /// <summary>构建body</summary>
        private static Expression GetExpressoinBody(ParameterExpression param, IList<DynamicCondition> conditions)
        {
            var list = new List<Expression>();
            if (conditions.Count > 0)
            {
                var plist = param.Type.GetRuntimeProperties().ToDictionary(z => z.Name);//可以加缓存改善性能
                foreach (DynamicCondition item in conditions)
                {
                    try
                    {
                        string key = item.Name;
                        if (item.Exp.Equals(DynamicConditionExpression.Gt)) //可能大小查询
                        {

                            if (!plist.ContainsKey(key) || item.Value.Length <= 0) continue;
                            var rType = plist[key].GetMethod.ReturnType;
                            if (rType == typeof(string)) continue;
                            var e1 = Expression.Property(param, key);
                            object dValue = TypeConverterExtension.ConvertObject(item.Value, rType);
                            list.Add(Expression.GreaterThan(e1, Expression.Constant(dValue)));
                        }
                        else if (item.Exp.Equals(DynamicConditionExpression.Lt)) //可能大小查询
                        {

                            if (!plist.ContainsKey(key) || item.Value.Length <= 0) continue;
                            var rType = plist[key].GetMethod.ReturnType;
                            if (rType == typeof(string)) continue;
                            var e1 = Expression.Property(param, key);
                            object dValue = TypeConverterExtension.ConvertObject(item.Value, rType);
                            if (rType == typeof(DateTime)) dValue = ((DateTime)dValue).AddDays(1);
                            list.Add(Expression.LessThan(e1, Expression.Constant(dValue)));

                        }
                        else if (plist.ContainsKey(key) && item.Value.Length > 0)
                        {
                            var e1 = Expression.Property(param, key);
                            var rType = plist[key].GetMethod.ReturnType;
                            if (rType == typeof(string) && item.Exp.Equals(DynamicConditionExpression.Like)) //可能是like查询
                            {
                                var value = item.Value;
                                var e2 = Expression.Constant(value);
                                list.Add(Expression.Call(e1, "Contains", null, new Expression[] { e2 }));
                            }
                            else if (rType == typeof(string) && item.Exp.Equals(DynamicConditionExpression.Eq))
                            {
                                var value = item.Value;
                                var e2 = Expression.Constant(value);
                                list.Add(Expression.Equal(e1, e2));
                            }
                            else if (item.Exp.Equals(DynamicConditionExpression.Eq))
                            {
                                var value = item.Value;
                                object dValue = TypeConverterExtension.ConvertObject(value, rType);
                                Type underlyingType = Nullable.GetUnderlyingType(rType);
                                if (underlyingType != null)
                                {
                                    list.Add(Expression.NotEqual(e1, Expression.Constant(null)));
                                    var e2 = Expression.Property(e1, "Value");
                                    list.Add(Expression.Equal(e2, Expression.Constant(dValue)));
                                }
                                else
                                    list.Add(Expression.Equal(e1, Expression.Constant(dValue)));
                            }
                            else if (item.Exp.Equals(DynamicConditionExpression.In)) //可能是in查询
                            {
                                if (rType == typeof(short))
                                {
                                    CreateInExpression<short>(item, list, e1);
                                }
                                else if (rType == typeof(int))
                                {
                                    CreateInExpression<int>(item, list, e1);
                                }
                                else if (rType == typeof(long))
                                {
                                    CreateInExpression<long>(item, list, e1);
                                }
                            }
                            else
                            {
                                object dValue;
                                if (TypeConverterExtension.TryParser(item.Value, rType, out dValue))
                                    list.Add(Expression.Equal(e1, Expression.Constant(dValue)));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                        throw e;
                    }

                }
            }
            return list.Count > 0 ? list.Aggregate(Expression.AndAlso) : null;
        }

        private static void CreateInExpression<T>(DynamicCondition item, List<Expression> list, MemberExpression e1)
        {
            IList<T> searchList = new List<T>();
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertFrom(item.Value.GetType()))
                {
                    var values = item.Value.Split(',');
                    foreach (string v in values)
                    {
                        T v_ = (T)converter.ConvertFrom(v);
                        searchList.Add(v_);
                    }
                }
            }
            catch (Exception e)
            {
                //continue;
            }
            if (searchList.Count > 0)
            {
                list.Add(Expression.Call(Expression.Constant(searchList), "Contains", null,
                    new Expression[] { e1 }));
            }
        }
    }
}
