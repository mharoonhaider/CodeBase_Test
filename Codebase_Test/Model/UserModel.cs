using System.ComponentModel.DataAnnotations;

namespace Codebase_Test.Model
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CINumber { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string AccountPin { get; set; }
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
    }
}
