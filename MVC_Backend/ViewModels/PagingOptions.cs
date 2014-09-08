using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace MVC_Backend.ViewModels
{
    public class PagingOptions
    {
        public static PagedListRenderOptions Standard
        {
            get
            {
                var options = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    LinkToPreviousPageFormat = "上一頁",
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    LinkToNextPageFormat = "下一頁",
                    DisplayLinkToLastPage = PagedListDisplayMode.Always
                };

                return options;
            }
        }

    }
}