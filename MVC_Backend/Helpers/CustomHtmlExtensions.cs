using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Backend.Helpers
{
    public static class CustomHtmlExtensions
    {
        /// <summary>
        /// 判斷是否為當前控制項頁面
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool IsCurrentController(this HtmlHelper helper, string controllerName)
        {
            var currentControllerName =
                (string) helper.ViewContext.RouteData.Values["controller"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}