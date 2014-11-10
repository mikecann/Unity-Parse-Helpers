using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityParseHelpers;

public static class ParseExtensions
{
    public static string GetKey<TSource, TValue>(this TSource obj, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return GetKey<TSource, TValue>(expression);
    }

    public static void AddToList<TSource, TValue>(this TSource obj, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        obj.AddToList(GetKey(expression), value);
    }

    public static void RemoveFromList<TSource, TValue, TValues>(this TSource obj, Expression<Func<TSource, TValue>> expression, IEnumerable<TValues> values) where TSource : ParseObject
    {
        obj.RemoveAllFromList(GetKey(expression), values);
    }

    public static ParseQuery<TSource> WhereEqualTo<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereEqualTo(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> WhereNotEqualTo<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereNotEqualTo(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> WhereGreaterThan<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereGreaterThan(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> WhereLessThan<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereLessThan(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> WhereLessThanOrEqualTo<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereLessThanOrEqualTo(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> WhereGreaterThanOrEqualTo<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, object value) where TSource : ParseObject
    {
        return query.WhereGreaterThanOrEqualTo(GetKey<TSource, TValue>(expression), value);
    }

    public static ParseQuery<TSource> Include<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.Include(GetKey<TSource, TValue>(expression));
    }

    public static ParseQuery<TSource> OrderBy<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.OrderBy(GetKey<TSource, TValue>(expression));
    }

    public static ParseQuery<TSource> OrderByDescending<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.OrderByDescending(GetKey<TSource, TValue>(expression));
    }

    public static ParseQuery<TSource> ThenBy<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.ThenBy(GetKey<TSource, TValue>(expression));
    }

    public static ParseQuery<TSource> ThenByDescending<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.ThenByDescending(GetKey<TSource, TValue>(expression));
    }

    public static ParseQuery<TSource> WhereContainedIn<TSource, TValue, TIn>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, IEnumerable<TIn> values) where TSource : ParseObject
    {
        return query.WhereContainedIn(GetKey<TSource, TValue>(expression), values);
    }

    public static ParseQuery<TSource> WhereContains<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, string substring) where TSource : ParseObject
    {
        return query.WhereContains(GetKey<TSource, TValue>(expression), substring);
    }

    public static ParseQuery<TSource> WhereNotContainedIn<TSource, TValue, TIn>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, IEnumerable<TIn> values) where TSource : ParseObject
    {
        return query.WhereNotContainedIn(GetKey<TSource, TValue>(expression), values);
    }

    public static ParseQuery<TSource> WhereContainsAll<TSource, TValue, TIn>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, IEnumerable<TIn> values) where TSource : ParseObject
    {
        return query.WhereContainsAll(GetKey<TSource, TValue>(expression), values);
    }

    public static ParseQuery<TSource> WhereDoesNotExist<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.WhereDoesNotExist(GetKey<TSource, TValue>(expression));
    }

    //public static ParseQuery<TSource> WhereMatchesQuery<TSource, TValue, TOther>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, ParseQuery<TOther> otherQuery) where TSource : ParseObject
    //{
    //    return query.WhereMatchesQuery(GetKey<TSource, TValue>(expression), otherQuery);
    //}

    //public static ParseQuery<TSource> WhereDoesNotMatchQuery<TSource, TValue, TOther>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, ParseQuery<TOther> otherQuery) where TSource : ParseObject
    //{
    //    return query.WhereDoesNotMatchQuery(GetKey<TSource, TValue>(expression), otherQuery);
    //}

    //public static ParseQuery<TSource> WhereMatchesKeyInQuery<TSource, TValue, TOther>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, string keyInQuery, ParseQuery<TOther> otherQuery) where TSource : ParseObject
    //{
    //    return query.WhereMatchesKeyInQuery(GetKey<TSource, TValue>(expression), keyInQuery, otherQuery);
    //}

    public static ParseQuery<TSource> WhereEndsWith<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, string suffix) where TSource : ParseObject
    {
        return query.WhereEndsWith(GetKey<TSource, TValue>(expression), suffix);
    }

    public static ParseQuery<TSource> WhereExists<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression) where TSource : ParseObject
    {
        return query.WhereExists(GetKey<TSource, TValue>(expression));
    }

    //public static ParseQuery<TSource> WhereMatches<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, string pattern) where TSource : ParseObject
    //{
    //    return query.WhereMatches(GetKey<TSource, TValue>(expression), pattern);
    //}

    public static ParseQuery<TSource> WhereNear<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, ParseGeoPoint point) where TSource : ParseObject
    {
        return query.WhereNear(GetKey<TSource, TValue>(expression), point);
    }

    public static ParseQuery<TSource> WhereStartsWith<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, string substring) where TSource : ParseObject
    {
        return query.WhereStartsWith(GetKey<TSource, TValue>(expression), substring);
    }

    public static ParseQuery<TSource> WhereWithinDistance<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, ParseGeoPoint point, ParseGeoDistance maxDistance) where TSource : ParseObject
    {
        return query.WhereWithinDistance(GetKey<TSource, TValue>(expression), point, maxDistance);
    }

    public static ParseQuery<TSource> WhereWithinGeoBox<TSource, TValue>(this ParseQuery<TSource> query, Expression<Func<TSource, TValue>> expression, ParseGeoPoint southWest, ParseGeoPoint northEast) where TSource : ParseObject
    {
        return query.WhereWithinGeoBox(GetKey<TSource, TValue>(expression), southWest, northEast);
    }

    public static string GetKey<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
    {
        var chain = new List<string>();
        GetPropertyChain(expression.Body as MemberExpression, chain);            
        return String.Join(".", chain.ToArray());
    }

    public static Type GetPropertyChain(Expression expression, List<string> chain)
    {
        Type lastType = null;

        // Depending on what type of node this is in the chain 
        if (expression.NodeType == ExpressionType.MemberAccess)
        {
            // Get the sub expression and immediately reccurse so this is a bottom up recursion
            var memberExpression = expression as MemberExpression;
            lastType = GetPropertyChain(memberExpression.Expression, chain);

            // We try to look up the property in the last type
            var prop = lastType.GetProperty(memberExpression.Member.Name);
            if (prop == null) throw new ArgumentException(string.Format("Cannot find property '" + memberExpression.Member.Name + "' in type '" + lastType + "'"));

            // If there is a field name then we have found one link in the chain, huzzah!
            if (Attribute.IsDefined(prop, typeof(ParseFieldNameAttribute)))
            {
                var fieldName = ((ParseFieldNameAttribute)Attribute.GetCustomAttribute(prop, typeof(ParseFieldNameAttribute))).FieldName;
                chain.Add(fieldName);
            }
            else // there must be an error here somwhere
            {
                throw new ArgumentException(string.Format("Cannot find field name on proprty with expression '" + expression + "' perhaps you are using interfaces, consider using ParseFieldTypeAttribute to explicity declare the type of the property."));
            }

            // Now check to see if there is a type redirect, if there is then we will use that for the next node
            var fieldTypeAttr = Attribute.GetCustomAttribute(prop, typeof(ParseFieldTypeAttribute)) as ParseFieldTypeAttribute;
            if (fieldTypeAttr != null) return fieldTypeAttr.FieldType;
            else return expression.Type;
        }
        else if (expression.NodeType == ExpressionType.Call)
        {
            // this could be a method call or an array access which is turned into a method call expression at runtime
            var methodCall = expression as MethodCallExpression;
            lastType = GetPropertyChain(methodCall.Object, chain);

            // by returning the last type we are in effect skipping this node
            return lastType;
        }
        else if (expression.NodeType == ExpressionType.Parameter)
        {
            // We are at the start of the expression so just return
            return expression.Type;
        }

        return expression.Type;
    }
}