using Microsoft.AspNetCore.Mvc;

namespace VitoDeCarlo.Blazor.Areas.Identity
{
    public static class UrlHelperExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                var url = urlHelper.Page("/Index");
                if (url == null)
                    throw new NullReferenceException("Could not determine the Local Url");
                return url;
            }
            return localUrl;
        }

        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            var link = urlHelper.Page(
                "/Identity/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
            if (link == null)
                throw new NullReferenceException("Could not determine the Email Confirmation Link Uri");
            return link;
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            var link = urlHelper.Page(
                "/Identity/Account/ResetPassword",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
            if (link == null)
                throw new NullReferenceException("Could not determine the Password Callback Link Uri");
            return link;
        }
    }
}