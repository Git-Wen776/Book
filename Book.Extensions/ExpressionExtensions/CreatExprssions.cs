using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Book.Models.Attributes;
using Book.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Extensions.ExpressionExtensions
{
    public class CreatExprssions
    {
        private static readonly object obj = 1;
        public static CreatExprssions expression = null;
        public static CreatExprssions GetCreate()
        {
            if (expression == null)
            {
                lock (obj)
                {
                    if (expression == null)
                        expression = new CreatExprssions();
                }
            }
            return expression;
        }
        /// <summary>
        /// 两表查询的join,ob1,ob2为模型类字段
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="ob1"></param>
        /// <param name="ob2"></param>
        /// <returns></returns>
        public Expression<Func<T1,T2,object[]>> JoinExpression<T1, T2>(string ob1, string ob2)
        {
            var para1 = Expression.Parameter(typeof(T1), "o");
            var para2 = Expression.Parameter(typeof(T2), "c");
            var prop1 = Expression.Property(para1, ob1);
            var prop2=Expression.Property(para2, ob2);
            var eq = this.Equal(prop1, prop2);
            return Expression.Lambda<Func<T1,T2,object[]>>(eq, para1, para2);
        }
        public Expression Equal(Expression left,Expression right)
        {
            return Expression.Equal(left, right);   
        }
        public Expression And(Expression left,Expression right)
        {
            return Expression.And(left, right);
        }
        public Expression Or(Expression left,Expression right) 
        { 
            return Expression.Or(left, right);
        }
        /// <summary>
        /// 三表查询的join，ob1,ob2,ob3为模型字段
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="ob1"></param>
        /// <param name="ob2"></param>
        /// <param name="ob3"></param>
        /// <returns></returns>
        public Expression<Func<T1,T2,T3,object[]>> JoinExpression<T1,T2,T3>(string ob1,string ob2,string ob3)
        {
            var para1 = Expression.Parameter(typeof(T1), "p");
            var para2 = Expression.Parameter(typeof(T2), "o");
            var para3=Expression.Parameter(typeof(T3), "c");
            var eq1=this.Equal(para1, para2);
            var eq2=this.Equal(para2, para3);
            var and=this.And(eq1, eq2);
            return Expression.Lambda<Func<T1,T2,T3,object[]>>(and, para1, para2);
        }
        /// <summary>
        /// 根据实体查询--实体特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception> 
        public Expression<Func<T,bool>> CatchAll<T>(T t)
        {
            var para=Expression.Parameter(typeof(T), "p");
            if(t==null)
                throw new ArgumentNullException("参数为空");
            BinaryExpression whereExpression = Expression.And(Expression.Constant(true), Expression.Constant(true));
            var props=typeof(T).GetProperties();
            Expression expre = null;
            foreach(var prop in props)
            {
                if (prop.GetValue(t) == null||prop.GetType()==typeof(DateTime))//过滤空值属性和datetime类型的字段
                    continue;
                var p = Expression.Property(para, prop.Name);
                var attrs = prop.GetCustomAttributes().ToArray();
                var method = typeof(String).GetMethod("Contains", new[] { typeof(string) });//Like
                foreach (var attr in attrs)
                {
                    if (!(attr.GetType() == typeof(ExpressionAttribute)))
                        continue;
                    var attribute=attr as ExpressionAttribute;
                    var value = prop.GetValue(t);
                    switch (attribute.SwitchType())
                    {
                        case 1:
                            expre = Expression.Equal(p, Expression.Constant(value));
                            whereExpression = Expression.AndAlso(whereExpression, expre);
                            break;
                        case 2:
                            expre = Expression.Equal(p, Expression.Constant(value));
                            whereExpression = Expression.Or(whereExpression, expre);
                            break;
                        case 3:
                            expre = Expression.Call(p, method, Expression.Constant(value));
                            whereExpression = Expression.AndAlso(whereExpression, expre);
                            break;
                        case 4:
                            expre = Expression.Call(p, method, Expression.Constant(value));
                            whereExpression = Expression.Or(whereExpression, expre);
                            break;
                    }
                }
            }
            return Expression.Lambda<Func<T, bool>>(whereExpression, para);
        }
        public Expression Equal<T>(string filed,T t)
        {
            var value = this.GetValue(filed, t);
            var para = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(para, filed);
            return Expression.Equal(prop, Expression.Constant(value,value.GetType()));
        }
        /// <summary>
        /// 获取字段值 filed字段名 模型实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object GetValue<T>(string filed,T t)
        {
            if (filed == null||t==null)
                throw new ArgumentNullException("参数字段为空");
            return typeof(T).GetProperties().FirstOrDefault(p => p.Name == filed).GetValue(t);
        }
        /// <summary>
        /// 根据实体查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Expression<Func<T,bool>> CatchModel<T>(T t)
        {
            if(t==null)
                throw new ArgumentNullException("参数实体为空");
            var para=Expression.Parameter(typeof(T), "p");
            var props = typeof(T).GetProperties();
            BinaryExpression whereExpression = Expression.And(Expression.Constant(true), Expression.Constant(true));
            foreach (var prop in props)
            {
                var value=prop.GetValue(t);
                if (value == null||prop.PropertyType==typeof(DateTime))
                    continue;
                var p=Expression.Property(para, prop);
                var eq = Expression.Equal(p, Expression.Constant(value,value.GetType()));
                whereExpression=Expression.AndAlso(eq, Expression.Constant(true));
            }
            return Expression.Lambda<Func<T, bool>>(whereExpression, para);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Expression MorethanEqual<T>(string filed,T t)
        {
            var para = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(para, filed);
            var value = this.GetValue<T>(filed, t);
            return Expression.GreaterThan(prop, Expression.Constant(value, value.GetType()));
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Expression LessthanEqual<T>(string filed,T t)
        {
            var para = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(para, filed);
            var value=this.GetValue<T>(filed,t);
            return Expression.LessThanOrEqual(prop, Expression.Constant(value, value.GetType()));
        }
        /// <summary>
        /// In查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed"></param>
        /// <param name="t"></param>
        /// <param name="stringvalues"></param>
        /// <returns></returns>
        public Expression In<T>(string filed,T t,string[] stringvalues)
        {
            var para=Expression.Parameter(typeof(T), "p");
            var prop=Expression.Property(para, filed);
            var contains = typeof(Enumerable).GetMethods().FirstOrDefault(p => p.GetParameters().Length == 2 && p.Name == "Contains").MakeGenericMethod(typeof(string));
            return Expression.Call(null, contains, Expression.Constant(stringvalues), prop); 
        }
        public Expression In<T>(string filed,T t,int[] intvalues)
        {
            var para = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(para, filed);
            var contains = typeof(Enumerable).GetMethods().FirstOrDefault(p => p.GetParameters().Length == 2 && p.Name == "Contains").MakeGenericMethod(typeof(int));
            return Expression.Call(null, contains, Expression.Constant(intvalues), prop);
        }
    }
    public static class AddExpression
    {
        public static void AddExpressionSetup(this IServiceCollection service)
        {
            service.AddSingleton<CreatExprssions>(p => CreatExprssions.GetCreate());
        }
    }
}
