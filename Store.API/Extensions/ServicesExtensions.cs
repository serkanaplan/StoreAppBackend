using System.Reflection;
using System.Text;
using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store.Entities.DTOS;
using Store.Entities.Models;
using Store.Repo.Contracts;
using Store.Repo.EFCore;
using Store.Service;
using Store.Service.Concretes;
using Store.Service.Contracts;
using Store.Service.Validations;

namespace Store.API;

//program.cs dosyası çok şişmesin diye bazı kısımları eklenti olarak oluşturup program.cs dosyasına ekliyoruz.
public static class ServicesExtensions
{
    public static void AddServicesSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepoContext>(
            options => options.UseSqlServer(
                configuration.GetConnectionString("sqlConnection"),
                option => option.MigrationsAssembly(Assembly.GetAssembly(typeof(RepoContext))?.GetName().Name)
            )
        );
    }

    public static void AddServiceRepo(this IServiceCollection services)
    {
        services.AddScoped<IRepoManager, RepoManager>();
    }

    public static void AddServiceServices(this IServiceCollection services)
        => services.AddScoped<IServiceManager, ServiceManager>();

    public static void AddServiceLogger(this IServiceCollection services)
    => services.AddSingleton<ILoggerService, LoggerManager>();//herkes aynı örneğe erişecek oyuzden singleton yaptık

    public static void AddServiceValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<BookDTOForInsertionValidator>();
    }

    public static void AddActionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
        services.AddSingleton<LogFilterAttribute>();
    }


    //CORS, yani Kaynak Arası Paylaşım(Cross-Origin Resource Sharing), web güvenliği ile ilgili bir mekanizmadır.
    //Tarayıcıların, farklı bir alan adından gelen (aynı protokol ve portta olsa bile) kaynaklara(resimler, script dosyaları, vs.) erişim girişimlerini kısıtlamak için kullanılır.
    //Bu sayede kötü niyetli scriptlerin bir web sitesinden başka bir web sitesine erişerek hassas verilere ulaşması engellenir.
    //Örneğin, www.example.com adresindeki bir web sitesi, www.farklialan.com adresindeki bir API'den veri çekmek isteyebilir. CORS politikaları olmadığı durumda, tarayıcı güvenlik nedeniyle bu isteğe izin vermez.
    //Ancak, günümüz web uygulamalarında bazen farklı alan adlarından kaynaklara erişim gereklidir.CORS, bu gibi durumlarda güvenli bir şekilde kaynak paylaşımına izin verir.
    //Sunucu, CORS başlıklarını kullanarak hangi alan adlarının veya uygulamaların kendi kaynaklarına erişebileceğini belirtebilir.
    //CORS'un bileşenleri:
    //Köken (Origin): Protocol (http/https), domain adı ve port numarasından oluşan bir üçlü.Aynı kökendeki kaynaklar birbirlerine erişebilir.
    //CORS başlıkları: Sunucu tarafından gönderilen ve tarayıcıya hangi kaynakların erişime sahip olduğunu belirten HTTP başlıklarıdır.
    //    Origin                                        Sonuç                   Nedeni
    //https://www.gencayyildiz.com/page/1	            Başarılı	            protocol, host ve port aynı
    //https://www.gencayyildiz.com/images/rsm.png	    Başarılı	            protocol, host ve port aynı
    //https://www.gencayyildiz.com:88	                Başarısız	            protocol ve host aynı, port farklı
    //http://www.gencayyildiz.com	                    Başarısız	            protocol farklı, host ve port aynı
    //https://gencayyildiz.com	                        Başarısız	            protocol ve port aynı, host farklı
    public static void AddServiceCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>//Bu kısımda, "CorsPolicy" adında bir CORS politikası oluşturulur.
            builder.AllowAnyOrigin()//AllowAnyOrigin() metoduyla herhangi bir kaynaktan gelen isteklere izin verilir
            .AllowAnyMethod()// AllowAnyMethod() metoduyla herhangi bir HTTP metoduna izin verilir(GET, POST, PUT vb.)
            .AllowAnyHeader()// AllowAnyHeader() metoduyla herhangi bir HTTP başlığına izin verilir.
            .WithExposedHeaders("X-Pagination")//WithExposedHeaders("X-Pagination") metoduyla da "X-Pagination" başlığının dışarıya açılmasına izin verilir.
            );
        }
        );
    }

    public static void AddServiceDataShaper(this IServiceCollection services)
    {
        services.AddScoped<IDataShaper<BookDTO>, DataShaper<BookDTO>>();
    }

    public static void AddServiceCaching(this IServiceCollection services)
    => services.AddResponseCaching();

    public static void AddServiceRateLimiting(this IServiceCollection services)
    {
        var rateLimitRule = new List<RateLimitRule>()
        {
           new RateLimitRule()
           {
             Endpoint ="*",//tüm endpointler için(get,post,put vb.)
             Limit = 3, //dakikada 3 istek
             Period = "1m" //1 dakika içerisinde
           }
        };

        services.Configure<IpRateLimitOptions>(opt => opt.GeneralRules = rateLimitRule);

        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequiredLength = 6;

            opts.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<RepoContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddServiceJWT(this IServiceCollection services,IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWTSettings");
        var secretKey = jwtSettings["secretKey"];

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            }
        );
    }

      public static void AddServiceSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", 
                    new OpenApiInfo 
                    { 
                        Title = "BTK Akademi", 
                        Version = "v1",
                        Description = "BTK Akademi ASP.NET Core Web API",
                        TermsOfService = new Uri("https://www.btkakademi.gov.tr/"),
                        Contact = new OpenApiContact
                        {
                            Name = "Serkan KAPLAN",
                            Email = "serkanaplan@gmail.com",
                            Url = new Uri("https://www.zafercomert.com")
                        }
                    });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme="Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
}
