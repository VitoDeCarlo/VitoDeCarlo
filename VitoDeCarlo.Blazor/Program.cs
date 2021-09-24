using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VitoDeCarlo.Areas.Identity;
using VitoDeCarlo.Blazor.Helpers;
using VitoDeCarlo.Core.Services;
using VitoDeCarlo.Data;
using VitoDeCarlo.Data.Identity;
using VitoDeCarlo.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration.GetConnectionString("AppConfig");
builder.Configuration.AddAzureAppConfiguration(appConfig);

// Add Database Connection, Context, and ExceptionFilter
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<VitoDbContext>(options => options.UseSqlServer(connectionString));

if (builder.Environment.IsDevelopment())
{
    //builder.Configuration.AddUserSecrets<Program>();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// Add custom Identity
builder.Services.AddIdentity<User, Role>(options => {
    options.Password.RequiredLength = 8;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddDefaultTokenProviders();


// Add Authentication
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
var twitterConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"];
var twitterConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale", "string"); // returns "en"
        options.ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender", "string");
        options.SaveTokens = true;
        options.Events.OnCreatingTicket = ctx =>
        {
            List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();
            ctx.Properties.StoreTokens(tokens);
            return Task.CompletedTask;
        };
    })
    .AddFacebook(options =>
    {
        options.AppId = facebookAppId;
        options.AppSecret = facebookAppSecret;
        options.SaveTokens = true;
        options.Events.OnCreatingTicket = ctx =>
        {
            List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();
            ctx.Properties.StoreTokens(tokens);
            return Task.CompletedTask;
        };
        //options.AccessDeniedPath = "/facebook-denied";
    })
    .AddTwitter(options =>
    {
        options.ConsumerKey = twitterConsumerKey;
        options.ConsumerSecret = twitterConsumerSecret;
        options.SaveTokens = true;
        options.RetrieveUserDetails = true;
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name", "string");
        options.Events.OnCreatingTicket = ctx =>
        {
            List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();
            ctx.Properties.StoreTokens(tokens);
            return Task.CompletedTask;
        };
    });
////.AddMicrosoftAccount(options =>
////{
////    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
////    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
////    options.CallbackPath = "/Identity/Account/signin-microsoft";
////})

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddResponseCompression(o =>
{
    o.EnableForHttps = true;
});

// Add Services
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IRoleStore<Role>, RoleStore>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddHttpClient<TwilioVerifyService>();
builder.Services.AddHttpClient<IYouTubeService, YouTubeService>(options =>
{
    options.BaseAddress = new Uri("https://youtube.googleapis.com/youtube/v3/");
});
builder.Services.AddScoped<BrowserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
