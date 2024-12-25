using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions //Bir kişinin claimlerini ararken kullanılan class'ı genişlettik. Yazım kolaylığı sağlandı!
    {
        //ClaimsPrincipal .NET içerisinde bulunan ve jwt ile gelen kullanıcının claimlerini bulurken kullanılır
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }

}
