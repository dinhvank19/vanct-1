using System.Runtime.Serialization;

namespace Vanct.WebApp.Webservice
{
    [DataContract(Name = "File", Namespace = "http://Vanct.vn/SOAP/Files")]
    public class MyFile
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "Position")]
        public int Position { get; set; }

        [DataMember(Name = "FilePath")]
        public string FilePath { get; set; }

        [DataMember(Name = "HtmlContent")]
        public string HtmlContent { get; set; }

        [DataMember(Name = "Overview")]
        public string Overview { get; set; }
    }
}