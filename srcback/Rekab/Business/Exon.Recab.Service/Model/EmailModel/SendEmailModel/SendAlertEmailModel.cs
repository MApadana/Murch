using System;
using System.Collections.Generic;


namespace Exon.Recab.Service.Model.EmailModel.SendEmailModel
{
    public class SendAlertEmailModel
    {
        public string Name { get; set; }

        public List<SendAdsInfoCategoryFeatureEmailModel> CFItems { get; set; }

        public List<AlertItemEmailModel> AlertItems { get; set; }

        public SendAlertEmailModel()
        {
            CFItems = new List<SendAdsInfoCategoryFeatureEmailModel>();
            AlertItems = new List<AlertItemEmailModel>();

        }

    }
}
