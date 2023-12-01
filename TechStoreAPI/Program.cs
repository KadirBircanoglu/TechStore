using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Text;
using TechStoreBL.EmailSenderProcess;
using TechStoreBL.ImplementationOfManagers;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.AddContext;
using TechStoreDL.ImplementationOfRepos;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.IdentityModels;
using TechStoreEL.Mappings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
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


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); //login logout
app.UseAuthorization(); //yetki

app.MapControllers();

app.Run();
