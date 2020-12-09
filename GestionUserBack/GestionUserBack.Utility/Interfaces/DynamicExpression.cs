using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Utility.Interfaces
{
    public class DynamicExpression<T> : Helper
    {
        public Func<T, T> CreateNewFilteredStatement(List<string> fields, string dateFormat)
        {
            var xParameter = Expression.Parameter(typeof(T), "o");
            var xNew = Expression.New(typeof(T));
            var bindings = fields.Select(o => o.Trim())
                .Select(o =>
                {
                    string[] elts = o.Split('.');

                    var mi = typeof(T).GetProperty(elts[0]);
                    var xOriginal = Expression.Property(xParameter, mi);
                    return Expression.Bind(mi, xOriginal);
                }
            );
            var xInit = Expression.MemberInit(xNew, bindings);
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);
            return lambda.Compile();
        }

        public Func<T, bool> CreateFilterStatement(List<KeyValuePair<string, string>> conditions, string dateFormat)
        {


            var xParameter = Expression.Parameter(typeof(T), "o");
            Expression expression = null;
            foreach (KeyValuePair<string, string> pair in conditions)
            {
                bool isForeignProperty = false;
                string key = pair.Key;
                string[] fp = pair.Key.Split('.');
                if (fp.Length > 1)
                {
                    isForeignProperty = true;
                }
                if (isForeignProperty)
                {
                    key = fp[0];
                }
                MemberExpression prop = Expression.Property(xParameter, key);
                for (int i = 1; i < fp.Length; i++)
                {
                    prop = Expression.Property(prop, fp[i]);
                }

                PropertyInfo propertyInfo = (PropertyInfo)prop.Member;
                ConstantExpression equalTo = Expression.Constant(pair.Value);
                ConstantExpression nullValue = Expression.Constant(null);
                Expression equalExpression = null;

                if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
                {
                    if (!string.IsNullOrEmpty(pair.Value))
                    {
                        equalTo = Expression.Constant(DateTime.ParseExact(pair.Value, dateFormat, CultureInfo.InvariantCulture));
                    }
                }
                else if (propertyInfo.PropertyType == typeof(bool?) || propertyInfo.PropertyType == typeof(bool))
                {
                    if (!string.IsNullOrEmpty(pair.Value))
                    {
                        equalTo = Expression.Constant(Boolean.Parse(pair.Value));
                    }
                    equalExpression = Expression.Equal(prop, equalTo);
                }
                else if (propertyInfo.PropertyType == typeof(List<T>))
                {
                    var toStringMethod = typeof(List<T>).GetMethod("ToString");
                    var stringValue = Expression.Call(prop, toStringMethod);
                    equalExpression = Expression.Equal(stringValue, Expression.Constant(pair.Value.ToString()));
                }
                else if (propertyInfo.PropertyType == typeof(Guid?) || propertyInfo.PropertyType == typeof(Guid))
                {
                    var toStringMethod = typeof(Object).GetMethod("ToString");
                    var stringValue = Expression.Call(prop, toStringMethod);
                    equalExpression = Expression.Equal(stringValue, Expression.Constant(pair.Value.ToString()));
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    var toStringMethod = typeof(Object).GetMethod("ToString");
                    var stringValue = Expression.Call(prop, toStringMethod);
                    equalExpression = Expression.Equal(stringValue, equalTo);
                }
                else if (propertyInfo.PropertyType != typeof(string))
                {
                    var toStringMethod = typeof(Object).GetMethod("ToString");
                    var stringValue = Expression.Call(prop, toStringMethod);
                    equalExpression = Expression.Equal(stringValue, equalTo);
                    var notNullExpression = Expression.NotEqual(prop, nullValue);
                    equalExpression = Expression.AndAlso(notNullExpression, equalExpression);
                }

                else
                {
                    equalExpression = Expression.Equal(prop, equalTo);
                }
                if (expression == null)
                {
                    expression = equalExpression;
                }
                else
                {
                    expression = Expression.And(expression, equalExpression);
                }
            }
            if (expression == null)
            {
                expression = Expression.Constant(true);
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, xParameter);
            return lambda.Compile();
        }

        public Func<T, bool> CreateSearchStatement(string value, List<string> inFields, string dateFormat)
        {
            var xParameter = Expression.Parameter(typeof(T), "o");
            Expression expression = null;
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (inFields != null)
            {
                foreach (string field in inFields)
                {
                    var containsValue = Expression.Constant(value.ToLower());
                    var nullValue = Expression.Constant(null);

                    MemberExpression prop = null;
                    // var prop = Expression.Property(xParameter, field);

                    string[] elts = field.Split('.');
                    foreach (string el in elts)
                    {
                        if (prop == null)
                        {
                            prop = Expression.Property(xParameter, el);
                        }
                        else
                        {
                            prop = Expression.Property(prop, el);
                        }
                    }

                    PropertyInfo propertyInfo = (PropertyInfo)prop.Member;
                    Expression containsMethodExp = null;
                    BinaryExpression conditionalContaintsMethod = null;
                    if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
                    {
                        MethodInfo dateMethod = typeof(DateTime).GetMethod("ToString", new[] { typeof(string) });
                        var dateFormatConstant = Expression.Constant(dateFormat);
                        var dateToString = Expression.Call(prop, dateMethod, dateFormatConstant);
                        containsMethodExp = Expression.Call(dateToString, containsMethod, containsValue);

                        //var notNullExpression = Expression.NotEqual(prop, nullValue);
                        conditionalContaintsMethod = Expression.AndAlso(Expression.Constant(true), containsMethodExp);
                        //conditionalContaintsMethod = Expression.OrElse(Expression.Equal(prop, nullValue), conditionalContaintsMethod);
                    }
                    else if (propertyInfo.PropertyType == typeof(int) ||
                        propertyInfo.PropertyType == typeof(double) ||
                        propertyInfo.PropertyType == typeof(decimal) ||
                        propertyInfo.PropertyType == typeof(decimal)
                        )
                    {
                        var toStringMethod = typeof(Object).GetMethod("ToString");
                        var stringValue = Expression.Call(prop, toStringMethod);
                        containsMethodExp = Expression.Call(stringValue, containsMethod, containsValue);
                        conditionalContaintsMethod = Expression.OrElse(Expression.Constant(false), containsMethodExp);
                    }
                    else if (propertyInfo.PropertyType != typeof(string))
                    {
                        var toStringMethod = typeof(Object).GetMethod("ToString");
                        var stringValue = Expression.Call(prop, toStringMethod);
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var lowerValue = Expression.Call(stringValue, toLowerMethod);
                        containsMethodExp = Expression.Call(lowerValue, containsMethod, containsValue);

                        var notNullExpression = Expression.NotEqual(prop, nullValue);
                        conditionalContaintsMethod = Expression.AndAlso(notNullExpression, containsMethodExp);
                        //conditionalContaintsMethod = Expression.OrElse(Expression.Equal(prop, nullValue), conditionalContaintsMethod);
                    }
                    else
                    {
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var lowerValue = Expression.Call(prop, toLowerMethod);
                        containsMethodExp = Expression.Call(lowerValue, containsMethod, containsValue);

                        var notNullExpression = Expression.NotEqual(prop, nullValue);
                        conditionalContaintsMethod = Expression.AndAlso(notNullExpression, containsMethodExp);
                        //conditionalContaintsMethod = Expression.OrElse(Expression.Equal(prop, nullValue), conditionalContaintsMethod);
                    }
                    if (expression == null)
                    {
                        expression = conditionalContaintsMethod;
                    }
                    else
                    {
                        expression = Expression.OrElse(expression, conditionalContaintsMethod);
                    }
                }
            }
            if (expression == null)
            {
                expression = Expression.Constant(true);
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expression, xParameter);
            return lambda.Compile();
        }
    }
}
