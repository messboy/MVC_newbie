//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Backend.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public System.Guid ID { get; set; }
        public System.Guid ArticleID { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Article Article { get; set; }
    }
}
