using Application.Service;
using Domain.Model;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System.IO;
using System.Linq;
using System.Text;

namespace MS_Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllers().AddFluentValidation();
            services.AddControllers();
            services.AddCors();
            services.AddLogging();

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("MSConsumerSettings").GetSection("PrivateSecretKey").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
       
            // Add framework services.

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "V1";
                    document.Info.Title = "PAM - Microservice Consumer";
                    document.Info.Description = "API's Documentation of Microservice Consumer of PAM Plataform";
                };

                config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                });

                config.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            string logFilePath = Configuration.GetSection("LogSettings").GetSection("LogFilePath").Value;
            string logFileName = Configuration.GetSection("LogSettings").GetSection("LogFileName").Value;

            string connectionString = Configuration.GetSection("MSConsumerSettings").GetSection("ConnectionString").Value;
            string privateSecretKey = Configuration.GetSection("MSConsumerSettings").GetSection("PrivateSecretKey").Value;
            string tokenValidationMinutes = Configuration.GetSection("MSConsumerSettings").GetSection("TokenValidationMinutes").Value;
            
            BaseURLWebApplication baseURLWebApplication = new BaseURLWebApplication() {
                Administrator = Configuration.GetSection("MSConsumerSettings").GetSection("baseURLWebApplication:Administrator").Value,
                Partner = Configuration.GetSection("MSConsumerSettings").GetSection("baseURLWebApplication:Partner").Value
            };

            TwilioSettings twilioSettings = new TwilioSettings() {
                AccountSID = Configuration.GetSection("TwilioAccount").GetSection("AccountSID").Value,
                AuthToken = Configuration.GetSection("TwilioAccount").GetSection("AuthToken").Value,
                PhoneNumber = Configuration.GetSection("TwilioAccount").GetSection("PhoneNumber").Value,
            };

            HttpEndPoints httpEndPoints = new HttpEndPoints() {
                MSCommunicationBaseUrl = Configuration.GetSection("HttpEndPoints").GetSection("MSCommunicationBaseUrl").Value,
                MSStorageBaseUrl = Configuration.GetSection("HttpEndPoints").GetSection("MSStorageBaseUrl").Value
            };

            EmailSettings emailSettings = new EmailSettings()
            {
                PrimaryDomain = Configuration.GetSection("EmailSettings:PrimaryDomain").Value,
                PrimaryPort = Configuration.GetSection("EmailSettings:PrimaryPort").Value,
                UsernameEmail = Configuration.GetSection("EmailSettings:UsernameEmail").Value,
                UsernamePassword = Configuration.GetSection("EmailSettings:UsernamePassword").Value,
                FromEmail = Configuration.GetSection("EmailSettings:FromEmail").Value,
                ToEmail = Configuration.GetSection("EmailSettings:ToEmail").Value,
                CcEmail = Configuration.GetSection("EmailSettings:CcEmail").Value,
                EnableSsl = Configuration.GetSection("EmailSettings:EnableSsl").Value,
                UseDefaultCredentials = Configuration.GetSection("EmailSettings:UseDefaultCredentials").Value
            };

            services.AddSingleton((ILogger)new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File(Path.Combine(logFilePath, logFileName), rollingInterval: RollingInterval.Day)
              .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
              .CreateLogger());

            services.AddScoped<IConsumerRepository, ConsumerRepository>(
                provider => new ConsumerRepository(connectionString, provider.GetService<ILogger>()));

            services.AddScoped<IAddressRepository, AddressRepository>(
                provider => new AddressRepository(connectionString, provider.GetService<ILogger>()));
            services.AddScoped<ICardRepository, CardRepository>(
                provider => new CardRepository(connectionString, provider.GetService<ILogger>()));

            services.AddScoped<IAddressService, AddressService>(
                provider => new AddressService(provider.GetService<IAddressRepository>(), provider.GetService<IConsumerService>(), provider.GetService<ILogger>()));

            services.AddScoped<IConsumerService, ConsumerService>(
                provider => new ConsumerService(provider.GetService<IConsumerRepository>(), provider.GetService<ILogger>()));

            services.AddScoped<ICardService, CardService>(
                provider => new CardService(provider.GetService<ICardRepository>(), provider.GetService<ILogger>(), privateSecretKey, tokenValidationMinutes));


            services.AddTransient<IValidator<CreateConsumerRequest>, CreateConsumerRequestValidator>();
            services.AddTransient<IValidator<UpdateConsumerRequest>, UpdateConsumerRequestValidator>();

            services.AddTransient<IValidator<CreateAddressRequest>, CreateAddressRequestValidator>();
            services.AddTransient<IValidator<UpdateAddressRequest>, UpdateAddressRequestValidator>();

            services.AddTransient<IValidator<CreateCardRequest>, CreateCardRequestValidator>();
            services.AddTransient<IValidator<UpdateCardRequest>, UpdateCardRequestValidator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            // add the Swagger generator and the Swagger UI middlewares   
            app.UseSwaggerUi3();

            app.UseCors(builder =>
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMvc();


        }
    }
}