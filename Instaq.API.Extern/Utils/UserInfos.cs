namespace Instaq.API.Extern.Utils
{
    using Microsoft.AspNetCore.Http;

    public class UserInfos
    {
        public static string GetIpAddress(HttpRequest request)
        {
            return request.HttpContext.Connection?.RemoteIpAddress?.ToString() ?? "";
        }
    }
}
