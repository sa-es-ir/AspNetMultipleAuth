using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MultipleAuthSample.AuthHandlers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config = builder.Configuration;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  //IdentityServerA, the Scheme name is JwtBearerDefaults.AuthenticationScheme (Bearer)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidAudience = config.GetValue<string>("IdentitySeverA:Audience"),
          ValidIssuer = config.GetValue<string>("IdentitySeverA:Issuer"),
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A-Secret-Key:)"))
      };
  })
  //IdentityServerB, the Scheme name is Scheme_ServerB
  .AddJwtBearer("Scheme_ServerB", options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidAudience = config.GetValue<string>("IdentitySeverB:Audience"),
          ValidIssuer = config.GetValue<string>("IdentitySeverB:Issuer"),
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B-Secret-Key:)"))
      };
  })
  //The scheme name is CustomToken
  .AddScheme<CustomAuthSchemeOptions, CustomAuthenticationHandler>("CustomToken", options =>
  {
      // no need to set any options because you will handle them in the handler implementation
  });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
