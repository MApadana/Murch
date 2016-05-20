using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class EditFeatureValueModel
    {

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long id { get; set; }

        public string pub_IconUrl { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string hideContainer { get;  set; }

        public string showContainer { get; set; }
    }
}