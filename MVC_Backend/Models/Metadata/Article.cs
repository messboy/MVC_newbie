using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Backend.Models
{
    [MetadataType(typeof(ArticleMetadata))]
    public partial class Article
    {
        public class ArticleMetadata
        {
            [DataType(DataType.Html)]
            public object Content { get; set; }

            [UIHint("SystemUser")]
            public object CreateUser { get; set; }

            [UIHint("SystemUser")]
            public object UpdateUser { get; set; }     
        }
    }
}