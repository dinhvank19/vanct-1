using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Namviet.Data;

namespace Namviet.Helpers
{
    public class SessionHelper : ApplicationHelper
    {
        public static BaseUser LoginUser
        {
            get { return Session["LoginUser"] as BaseUser; }
            set { Session["LoginUser"] = value; }
        }
    }
}