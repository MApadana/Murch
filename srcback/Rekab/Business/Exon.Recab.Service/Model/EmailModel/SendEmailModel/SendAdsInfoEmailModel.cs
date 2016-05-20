using System;
using System.Collections.Generic;

namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendAdsInfoEmailModel
    {
        public string Name { get; set; }

        public string AdSLink { get; set; }

        public string ADSTitle { get;  set; }
        public List<SendAdsInfoCategoryFeatureEmailModel> CFItems { get; set; }

        public SendAdsInfoEmailModel()
        {
            CFItems = new List<SendAdsInfoCategoryFeatureEmailModel>();
        }

    }
}
