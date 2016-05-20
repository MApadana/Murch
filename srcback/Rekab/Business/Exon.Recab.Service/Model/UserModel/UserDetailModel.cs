namespace Exon.Recab.Service.Model.UserModel
{
    public class UserDetailModel
    {

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string mobile { get; set; }

        public int genderType { get; set; }

        public bool emailVerified { get;  set; }

        public bool mobileVerified { get;  set; }
    }
}
