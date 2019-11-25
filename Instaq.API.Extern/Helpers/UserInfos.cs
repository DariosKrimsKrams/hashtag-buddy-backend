namespace Instaq.API.Extern.Helpers
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
