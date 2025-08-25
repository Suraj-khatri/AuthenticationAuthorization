using AuthenticationAuthorization.Domain.Options;
using MailKit.Security;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace AuthenticationAuthorization.API.Loggings
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(ConfigureHostBuilder hostBuilder, IConfiguration configuration)
        {
            hostBuilder.UseSerilog((context, services, loggerConfig) =>
            {
                var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

                var emailOptions = new EmailSinkOptions
                {
                    From = emailSettings.From,
                    To = emailSettings.To,
                    Host = emailSettings.Host,
                    Port = emailSettings.Port,
                    Credentials = new System.Net.NetworkCredential(emailSettings.Username, emailSettings.Password),
                    Subject = new MessageTemplateTextFormatter(emailSettings.Subject),
                    Body = new MessageTemplateTextFormatter(emailSettings.Body),
                    IsBodyHtml = emailSettings.IsBodyHtml,
                    ConnectionSecurity = emailSettings.ConnectionSecurity == "StartTls"
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto
                };

                loggerConfig
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("Loggings/Logs/log-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.MSSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                        columnOptions: GetSqlColumnOptions(),
                        restrictedToMinimumLevel: LogEventLevel.Information
                    )
                    .WriteTo.Email(emailOptions, restrictedToMinimumLevel: LogEventLevel.Information);
            });
        }
        private static ColumnOptions GetSqlColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            // Add additional columns
            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn("UserId", SqlDbType.NVarChar, dataLength: 128),
                new SqlColumn("ControllerName", SqlDbType.NVarChar, dataLength: 128),
                new SqlColumn("ActionName", SqlDbType.NVarChar, dataLength: 128),
                new SqlColumn("RequestBody", SqlDbType.NVarChar, dataLength: -1),   // -1 = MAX
                new SqlColumn("ResponseBody", SqlDbType.NVarChar, dataLength: -1),
                new SqlColumn("CorrelationId", SqlDbType.NVarChar, dataLength: 128),
                new SqlColumn("IpAddress", SqlDbType.NVarChar, dataLength: 64)
            };

            return columnOptions;
        }

    }
}
