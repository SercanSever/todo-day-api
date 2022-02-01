using Microsoft.OpenApi.Models;
using Couchbase.Extensions.DependencyInjection;
using TO_DO.SERVİCE.Contracts;
using Autofac;
using TO_DO.SERVİCE.DependencyResolvers.Autofac;
using Autofac.Extensions.DependencyInjection;
using Couchbase.Linq;
using Microsoft.AspNetCore.DataProtection;
using AutoMapper;
using TO_DO.SERVİCE.Infrastructure.AutoMapperProfiler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TO_DO.SERVİCE.Concrete;
using TO_DO.API.JWTService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//Protector
builder.Services.AddDataProtection();
var serviceProvider = builder.Services.BuildServiceProvider();
var provider = serviceProvider.GetService<IDataProtectionProvider>();
var protector = provider.CreateProtector(builder.Configuration["Protector_Key"]);

//AutoMapper
var mappingConfig = new MapperConfiguration(mapper =>
     {
        mapper.AddProfile(new BusinessProfile(protector));
     });
IMapper autoMapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(autoMapper);

//Coachbase
builder.Services.AddCouchbase(options =>
        {
           builder.Configuration.GetSection("Couchbase").Bind(options);
           options.AddLinq();
        })
        .AddCouchbaseBucket<ITodoBucketProvider>("Todos")
        .AddCouchbaseBucket<IUserBucketProvider>("Users");

//Autofac Ioc.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new TodoModule()));

//JWT
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters()
   {
      ValidateActor = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["Issuer"],
      ValidAudience = builder.Configuration["Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"]))
   };
});
builder.Services.AddSingleton<ITokenService>(new TokenService(builder, autoMapper));

//Swagger
builder.Services.AddSwaggerGen(options =>
{
   options.SwaggerDoc("v1", new OpenApiInfo
   {
      Version = "v1",
      Title = "TO-DO API",
      Description = "An api for personal to-do list",
      Contact = new OpenApiContact
      {
         Name = "My Github Profile",
         Url = new Uri("https://github.com/SercanSever")
      },
   });

   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      Description = "JWT Authorization header using the Bearer scheme",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer"
   });

   options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme{
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});







var app = builder.Build();
if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
   options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
   options.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
