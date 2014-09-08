using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVC_Backend.ViewModels;


namespace MVC_Backend.Helpers
{
    public static class QueryOptionHtmlExtensions
    {
        public static MvcHtmlString SortableFor<TEntity, TProperty>(
            this HtmlHelper<QueryOption<TEntity>> htmlHelper,
            Expression<Func<TEntity, TProperty>> expression,
            string tagName,
            string text = null,
            object htmlAttributes = null)
        {
            var column = GetFullPropertyName(expression);

            var tag = new TagBuilder(tagName);

            tag.AddCssClass("sortable");
            tag.Attributes["data-column"] = column;

            if (text == null)
            {
                var memberAccess = expression.Body as MemberExpression;
                text = memberAccess.Member.Name;

                var displayAttr = (DisplayNameAttribute)memberAccess.Member.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                if (displayAttr != null)
                {
                    text = displayAttr.DisplayName;
                }
            }

            tag.SetInnerText(text);

            var queryOption = htmlHelper.ViewData.Model;

            if (queryOption.Column == column)
            {
                var order = queryOption.Order;

                tag.Attributes["data-direction"] = order.ToString();

                var icon = new TagBuilder("i");
                icon.AddCssClass("glyphicon");

                switch (order)
                {
                    case Order.Ascending:
                        icon.AddCssClass("glyphicon-chevron-up");
                        break;

                    case Order.Descending:
                        icon.AddCssClass("glyphicon-chevron-down");
                        break;
                }

                tag.InnerHtml += " " + icon.ToString();
            }

            if (htmlAttributes != null)
            {
                var attributes = new RouteValueDictionary(htmlAttributes);
                tag.MergeAttributes(attributes);
            }

            return new MvcHtmlString(tag.ToString());
        }

        // code adjusted to prevent horizontal overflow
        static string GetFullPropertyName<TEntity, TProperty>(
            Expression<Func<TEntity, TProperty>> expression)
        {
            MemberExpression memberExpression;

            if (!TryFindMemberExpression(expression.Body, out memberExpression))
            {
                return string.Empty;
            }

            var memberNames = new Stack<string>();

            do
            {
                memberNames.Push(memberExpression.Member.Name);
            }
            while (TryFindMemberExpression(memberExpression.Expression, out memberExpression));

            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow
        private static bool TryFindMemberExpression(
            Expression expression,
            out MemberExpression memberExpression)
        {
            memberExpression = expression as MemberExpression;

            if (memberExpression != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(expression) && expression is UnaryExpression)
            {
                memberExpression = ((UnaryExpression)expression).Operand as MemberExpression;

                if (memberExpression != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
            );
        }

    }
}