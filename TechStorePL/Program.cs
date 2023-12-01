using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using TechStoreBL.EmailSenderProcess;
using TechStoreBL.ImplementationOfManagers;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.AddContext;
using TechStoreDL.ImplementationOfRepos;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.IdentityModels;
using TechStoreEL.Mappings;
using TechStorePL.CreateDefaultData;

var builder = WebApplication.CreateBuilder(args);

//culture info
var cultureInfo = new CultureInfo("tr-TR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


// serilog logger ayarlari

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


//contexti ayarliyoruz.
builder.Services.AddDbContext<TechStoreContext>(options =>
{
    //klasik mvcde connection string web configte yer alir.
    //core mvcde connection string appsetting.json dosyasindan alinir.
    options.UseSqlServer(builder.Configuration.GetConnectionString("TechStoreCon"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

//appuser ve approle identity ayari
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

}).AddDefaultTokenProviders().AddEntityFrameworkStores<TechStoreContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.SlidingExpiration = true;
});
builder.Services.AddSession();
//automapper ayari 
builder.Services.AddAutoMapper(a =>
{
    a.AddExpressionMapping();
    a.AddProfile(typeof(Maps));
    //a.CreateMap<AppUser, ProfileViewModel>();

});

//interfacelerin DI yasam dongusu
builder.Services.AddScoped<IEmailManager, EmailManager>();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductManager, ProductManager>();

builder.Services.AddScoped<IProductPropertyRepo, ProductPropertyRepo>();
builder.Services.AddScoped<IProductPropertyManager, ProductPropertyManager>();

builder.Services.AddScoped<IProductPictureRepo, ProductPictureRepo>();
builder.Services.AddScoped<IProductPictureManager, ProductPictureManager>();

builder.Services.AddScoped<IProductDiscountRepo, ProductDiscountRepo>();
builder.Services.AddScoped<IProductDiscountManager, ProductDiscountManager>();

builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderManager, OrderManager>();

builder.Services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
builder.Services.AddScoped<IOrderDetailManager, OrderDetailManager>();

builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryManager, CategoryManager>();

builder.Services.AddScoped<ICategoryProductPropertyRepo, CategoryProductPropertyRepo>();
builder.Services.AddScoped<ICategoryProductPropertyManager, CategoryProductPropertyManager>();
builder.Services.AddScoped<IContactMessageRepo, ContactMessageRepo>();
builder.Services.AddScoped<IContactMessageManager, ContactMessageManager>();

//digerleri buraya yazilacaktir




// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //login logout
app.UseAuthorization(); //yetki


app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Admin}/{action=Index}/{id?}");


//Sistem ilk ayaga kalktiginnda rolleri ekleyelim
//ADMIN, MEMBER, WAITINGFORACTIVATION, PASSIVE

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    //    var roleManager = serviceProvider.
    //GetRequiredService<RoleManager<AppRole>>();

    CreateData c = new CreateData();
    c.CreateRoles(serviceProvider);


}

app.Run();
