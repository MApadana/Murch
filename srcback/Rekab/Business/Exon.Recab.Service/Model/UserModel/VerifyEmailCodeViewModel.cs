

namespace Exon.Recab.Service.Model.UserModel
{
    public class VerifyEmailCodeViewModel
    {
        public bool userExist { get; set; }

        public bool emailVerified { get; set; }

        public string id { get; set; }
        public WebLoginModel user { get;  set; }
    }
}
