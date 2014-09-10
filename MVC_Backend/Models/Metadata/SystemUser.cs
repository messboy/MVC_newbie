using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Backend.Models
{
    [MetadataType(typeof(SystemUserMetadata))]
    public partial class SystemUser
    {
        public class SystemUserMetadata
        {
             [DataType(DataType.Password)]
            public object Password { get; set; }

            [DataType(DataType.EmailAddress)]
            public object Email { get; set; }

            [UIHint("SystemUser")]
            public object CreateUser { get; set; }

            [UIHint("SystemUser")]
            public object UpdateUser { get; set; }
        }
    }
}