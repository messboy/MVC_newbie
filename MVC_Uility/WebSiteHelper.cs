﻿using MVC_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace MVC_Uility
{
    public class WebSiteHelper
    {
        public static Guid CurrentUserID
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return Guid.Empty;
                }

                return Guid.Parse(identity.Ticket.UserData); ;
            }
        }

        public static string CurrentUserName
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                return identity == null ? "未登入" : identity.Name;
            }
        }

        public static string SystemUserName(Object id)
        {
            Guid systemUserID;
            if (Guid.TryParse(id.ToString(), out systemUserID))
            {
                if (systemUserID.Equals(Guid.Empty))
                {
                    return "系統預設";
                }
                else
                {
                    using (WorkShopEntities db = new WorkShopEntities())
                    {
                        var user = db.SystemUsers.FirstOrDefault(x => x.ID == systemUserID);
                        return (user != null) ? user.Name : "";
                    }
                }
            }
            return "";
        }
    }
}
