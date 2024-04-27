//bu komutu Entityframework.Core,Sqlserver ve tools paketlerini kurduğun dizinde çalıştır yani repo projende --startup-project kısmına da design paketini kurduğun proje dizinini ver yani apiyi
// dotnet ef --startup-project ..\Store.API\  migrations add init    
// dotnet ef --startup-project ..\Store.API\  database update  

//bu komut migrations olu�turmadan veritaban�na i�ler
//dotnet ef database update --verbose --project .\Store.Repo\   --startup-project .\Store.API\
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Store.API;
using Store.API.Extensions;
using Store.Service.Contracts;
using Store.Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

//NLog dosyasının bulunduğu dizini alıp kaydettik
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

//apimizin csv tipinde de dönüş yapabilmesi için AddCustomCsvFormatter extensionunu yazdık ve buraya ekledik
builder.Services.AddControllers(
    config => config.CacheProfiles.Add("5MinutesDuration", new CacheProfile() { Duration = 300 })//5 dakika cache oluşturduk   
)
.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
.AddCustomCsvFormatter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServiceValidation();//fluentvalidation için eklenti yazıp çağırdık
builder.Services.AddServiceSwagger();

builder.Services.AddScoped<ValidationFilterAttribute>();//actionfilter yazdık ve kaydettik

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
//eklentilerimiz 
builder.Services.AddServicesSql(builder.Configuration);
builder.Services.AddServiceRepo();
builder.Services.AddServiceServices();
builder.Services.AddServiceLogger();
builder.Services.AddAutoMapper(typeof(MappingProfile));//automapper kütüphanesinin çalışacağı assemblyi verdik yani api projesi
builder.Services.AddActionFilters();
builder.Services.AddServiceCors();
builder.Services.AddServiceDataShaper();
builder.Services.AddServiceCaching();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.AddMemoryCache();//hız sınırlama(apiye atılan isteği sınırlamak için)
builder.Services.AddServiceRateLimiting();//hız sınırlama
builder.Services.AddHttpContextAccessor();//hı sınırlama

builder.Services.AddIdentity();
builder.Services.AddAuthentication();//bu metodu eklenti olarak AddServiceJWT metodunun içinde zaten kullandığımız için yoruma aldık
builder.Services.AddServiceJWT(builder.Configuration);//oturum açma işlemleri için

var app = builder.Build();

//global hata yönetimi çin oluşturduğumuz extension u kaydettik. bunu uygulamayı elde ettikten sonra yaptık.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())//global hata yönetimi konusunda ekledik
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseIpRateLimiting();//hız sınırlama
app.UseCors("CorsPolicy");
app.UseResponseCaching();//keşleme işlemi için önce eklenti olarak ekledik sonra app üzerinden

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
