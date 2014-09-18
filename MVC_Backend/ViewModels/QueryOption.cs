using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;

namespace MVC_Backend.ViewModels
{
    public class QueryOption<T>
    {
        public string Keyword { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Column { get; set; }

        public Order Order { get; set; }

        public IPagedList<T> Result { get; set; }

        //建構子
        public QueryOption()
        {
            Page = 1;
            PageSize = 20;
        }

        public void SetSource(IEnumerable<T> source)
        {
            SetSource(source.AsQueryable());
        }

        public void SetSource(IQueryable<T> source)
        {
            //處理排序
            if (string.IsNullOrEmpty(Column))
            {
                Column = typeof(T).GetProperties().First().Name;
            }

            var param = Expression.Parameter(typeof(T));

            Expression parent = param;

            foreach (var column in Column.Split(new[] { '.' }))
            {
                parent = Expression.Property(parent, column);
            }

            dynamic keySelector = Expression.Lambda(parent, param);

            IOrderedQueryable<T> query = null;

            if (Order == Order.Ascending)
            {
                query = Queryable.OrderBy(source, keySelector);
            }
            else
            {
                query = Queryable.OrderByDescending(source, keySelector);
            }

            //設定分頁
            Result = query.ToPagedList(Page, PageSize);
        }
    }

}