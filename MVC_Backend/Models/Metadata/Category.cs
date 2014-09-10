using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Backend.Models
{
	[MetadataType(typeof(CategoryMetadata))]
	public partial class Category
	{
		public class CategoryMetadata
		{
			[UIHint("SystemUser")]
			public object CreateUser { get; set; }

			[UIHint("SystemUser")]
			public object UpdateUser { get; set; }            
		}
	}
}