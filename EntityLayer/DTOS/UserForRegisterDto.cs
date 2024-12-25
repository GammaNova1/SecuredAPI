

using EntityLayer.Abstract;

namespace EntityLayer.DTOS
{
    public class UserForRegisterDto : IDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
    }
}
