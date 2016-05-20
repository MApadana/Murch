using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.User
{
    public class ChangePasswordModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string oldPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(length: 6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        [Compare(otherProperty: "rePassword", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "ComparePassword")]
        public string newPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        [MinLength(6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "MinLength")]
        public string rePassword { get; set; }

        
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

       
    }
}