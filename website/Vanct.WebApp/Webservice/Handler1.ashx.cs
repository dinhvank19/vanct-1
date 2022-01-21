using System.Collections.Generic;
using System.Web;
using System.Linq;
using Newtonsoft.Json;
using Telerik.Web.Data.Extensions;
using Vanct.Dal.Entities;

namespace Vanct.WebApp.Webservice
{
    class MyClass
    {
        public IList<Small> Data { set; get; }
    }

    class Small
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string ImageUrl { set; get; }
        public int Position { set; get; }
    }

    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            string method = context.Request.QueryString["method"];
            switch (method)
            {
                case "GetListFiles":
                    GetListFiles(context);
                    break;
                case "GetTotalPage":
                    GetTotalPage(context);
                    break;
                case "GetFileDetails":
                    GetFileDetails(context);
                    break;
            }
        }

        protected void GetFileDetails(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int fileId = context.Request.QueryString["fileid"] != null
                ? int.Parse(context.Request.QueryString["fileid"])
                : 0;

            using (var db = new VanctEntities())
            {
                BaseFile file = db.BaseFiles.SingleOrDefault(i => i.Id == fileId);
                if (file == null) return;
                context.Response.Write(file.HtmlContent);
            }
        }

        protected void GetListFiles(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int startIndex = context.Request.QueryString["startindex"] != null
                ? int.Parse(context.Request.QueryString["startindex"])
                : 0;
            int pagesize = context.Request.QueryString["pagesize"] != null
                ? int.Parse(context.Request.QueryString["pagesize"])
                : 10;

            using (var db = new VanctEntities())
            {
                var list = (from i in db.BaseFiles
                    orderby i.Position descending
                            select new Small
                    {
                        Id = i.Id,
                        Name = i.Name,
                        ImageUrl = "http://vanct.com/UploadManage/BaseFileFolder/" + i.ImageUrl,
                        Position = i.Position
                    })
                    .Skip(startIndex)
                    .Take(pagesize)
                    .ToList();

                context.Response.Write(JsonConvert.SerializeObject(new MyClass {Data = list}));
            }
        }

        protected void GetTotalPage(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pagesize = context.Request.QueryString["pagesize"] != null
                ? int.Parse(context.Request.QueryString["pagesize"])
                : 10;
            using (var db = new VanctEntities())
            {
                var totalRecord = QueryableExtensions.Count(db.BaseUsers);

                int totalPage = totalRecord / pagesize;

                //add the last page, ugly
                if (totalRecord % pagesize != 0) totalPage++;
                context.Response.Write(totalPage);
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}