using EscueladeDanzasCore.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace WebAPIAutores1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); //limpio el mapeo de los tipos de los claims.. asi no me hace esos tipos raros
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones=>
            {
                opciones.Filters.Add(typeof(MiFiltrodeAcccion));
            })
                .AddJsonOptions(x=> x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddNewtonsoftJson();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));            
            services.AddEndpointsApiExplorer();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters {
                                            ValidateIssuer = false,
                                            ValidateAudience = false,
                                            ValidateLifetime = true,
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(Configuration["Llavejwt"]) ),
                                            ClockSkew = TimeSpan.Zero
                    });

            services.AddSwaggerGen(c=>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header 
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EsAdmin", politica => politica.RequireClaim("EsAdmin"));
            });

        }

        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
