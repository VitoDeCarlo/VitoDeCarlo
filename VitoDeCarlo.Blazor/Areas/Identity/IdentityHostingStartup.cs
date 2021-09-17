[assembly: HostingStartup(typeof(VitoDeCarlo.Blazor.Areas.Identity.IdentityHostingStartup))]
namespace VitoDeCarlo.Blazor.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {});
    }
}
