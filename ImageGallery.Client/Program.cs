using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAccessTokenManagement();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler();

// configure authenthication middleware.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:5001/";
    options.ClientId = "imagegalleryclient";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");  We do not to speciy as by default both these scope are requested by middleware.
    //options.CallbackPath = new PathString("signin-oidc"); default value
    //options.SignedOutCallbackPath = ""  
    // Must match with the post logout redirect URI at IDP client config
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("roles");
    options.Scope.Add("imagegalleryapi.read");
    options.Scope.Add("imagegalleryapi.write");
    options.Scope.Add("country");
    options.Scope.Add("offline_access");
    options.ClaimActions.MapJsonKey("role", "role");
    options.ClaimActions.MapJsonKey("country", "country");
    options.TokenValidationParameters = new()
    {
        NameClaimType = "given_name",
        RoleClaimType = "role",
    };
});

builder.Services.AddAuthorization(AuthorizationOptions =>
{
    AuthorizationOptions.AddPolicy("UserCanAddImage",
        AuthorizationPolicies.CanAddImage());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
