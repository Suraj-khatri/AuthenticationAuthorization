using AuthenticationAuthorization.API;
using AuthenticationAuthorization.API.Loggings;
using AuthenticationAuthorization.Domain.Options;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Configure Logging
//builder.Host.UseSerilog((context, services, configuration) =>
//{
//    var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
//    var emailOptions = new EmailSinkOptions
//    {
//        From = emailSettings.From,
//        To = emailSettings.To,
//        Host = emailSettings.Host,
//        Port = emailSettings.Port,
//        Credentials = new System.Net.NetworkCredential(emailSettings.Username, emailSettings.Password),
//        Subject = new MessageTemplateTextFormatter(emailSettings.Subject),
//        Body = new MessageTemplateTextFormatter(emailSettings.Body),
//        IsBodyHtml = emailSettings.IsBodyHtml,
//        ConnectionSecurity = emailSettings.ConnectionSecurity == "StartTls"
//        ? SecureSocketOptions.StartTls
//        : SecureSocketOptions.Auto // You can expand this if needed for SslOnConnect or None
//    };

//    configuration
//        .MinimumLevel.Information() // Default minimum log level
//        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Override log level for Microsoft namespace
//        .Enrich.FromLogContext() // Enrich log context with additional info like UserId
//        .WriteTo.Console() // Write logs to console for development/debugging
//        .WriteTo.File("Loggings/Logs/log-.txt", rollingInterval: RollingInterval.Day) // Write logs to a file, rolling daily
//        .WriteTo.MSSqlServer(
//            context.Configuration.GetConnectionString("DefaultConnection"),
//            sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
//            restrictedToMinimumLevel: LogEventLevel.Information // Logs to SQL Server at Information level and above
//        )
//        .WriteTo.Email(
//            options: emailOptions, // Send logs via email with configured options
//            restrictedToMinimumLevel: LogEventLevel.Information // Only send errors or higher level logs via email
//        );
//});

// Call custom Serilog config
SerilogConfiguration.ConfigureSerilog(builder.Host, builder.Configuration);

// Add services to the container.

//adding jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//adding swagger for jwt authentication
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Authentication And Authorization",
        Description = "My API for secure Authentication And Authorization",
        Contact = new OpenApiContact
        {
            Name = "Suraj Khatri",
            Email = "surajpoudelkhatri@gmail.com",
        },
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter the Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddApiDI(builder.Configuration);

// Configure Forwarded Headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(System.Net.IPAddress.Parse("192.168.1.21")); // Replace with your actual server IP
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
