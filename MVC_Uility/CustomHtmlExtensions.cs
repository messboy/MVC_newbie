using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

namespace MVC_Uility
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
                (string)helper.ViewContext.RouteData.Values["controller"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判斷是否為當前控制項頁面
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool IsCurrentController(this HtmlHelper helper, string controllerName, string actionName)
        {
            var currentControllerName =
                (string)helper.ViewContext.RouteData.Values["controller"];

            var currentActionName =
               (string)helper.ViewContext.RouteData.Values["action"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
                &&
                currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
