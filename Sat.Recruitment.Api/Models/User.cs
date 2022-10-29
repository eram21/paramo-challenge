namespace Sat.Recruitment.Api.Models
{
    public enum UserType
    {
        Normal,
        SuperUser,
        Premium
    }
    public partial class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Money { get; set; }
        public UserType Type { get; set; }
    }
}