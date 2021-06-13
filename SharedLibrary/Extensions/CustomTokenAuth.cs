using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configuration;
using SharedLibrary.Services;
using System;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOption)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = tokenOption.Issuer,
                    ValidAudience = tokenOption.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

            ///
            /// Aşağıdaki kod bloğu ek olarak farklı bir Jwt çözümleme şeması kullanılmak istendiğinde kullanılır.
            /// Ve şemanın kullanılması istenilen controller sınıfına bu kod eklenir =>  [Authorize(AuthenticationSchemes = "Test")]
            ///

            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = "Test";
            //    opt.DefaultChallengeScheme = "Test";
            //}).AddJwtBearer("Test", opt =>
            //{
            //    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidIssuer = tokenOption.Issuer,
            //        ValidAudience = tokenOption.Audience[0],
            //        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),

            //        ValidateIssuerSigningKey = true,
            //        ValidateAudience = true,
            //        ValidateIssuer = true,
            //        ValidateLifetime = true,

            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            //services.AddAuthorization(options =>
            //{
            //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //        JwtBearerDefaults.AuthenticationScheme,
            //        "Test");
            //    defaultAuthorizationPolicyBuilder =
            //        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            //});
        }
    }
}
