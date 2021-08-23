using CAF.Core.Repository;

using Microsoft.Extensions.Configuration;

using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CAF.Core.Helper
{
    public class NLogLogger : ILogger
    {
        private Logger logger;
        readonly IErrorLogRepository _errorLogRepository;
        public NLogLogger(IErrorLogRepository errorLogRepository)
        {
            string connectionString = Environment.GetEnvironmentVariable("EF_CONNECTION_STRING");
            LogManager.Configuration.Variables["deryaConnectionString"] = connectionString;
            logger = LogManager.GetCurrentClassLogger();
            _errorLogRepository = errorLogRepository;
        }

        public void Error(System.Exception ex)
        {
            _errorLogRepository.Add(new Entities.ErrorLog()
            {
                AppName = Core.Constant.CommonConstant.ApplicationName,
                message = ex.Message,
                timestamp = CAF.Core.Helper.UtilitiesHelper.DateTimeNow(),
                stacktrace = ex.ToString()
            });

            var logEventInfo = new LogEventInfo(LogLevel.Fatal, "", "Exception");
            logEventInfo.Properties["CallIdentifier"] = Guid.NewGuid().ToString();
            logEventInfo.Properties["Date"] = CAF.Core.Helper.UtilitiesHelper.DateTimeNow();
            logEventInfo.Properties["Message"] = ex.Message;
            logEventInfo.Properties["StackTrace"] = ex.ToString();
            logEventInfo.Exception = ex;
            logger.Error(logEventInfo);
        }

        public void Error(string appName, string message, string log)
        {
            _errorLogRepository.Add(new Entities.ErrorLog()
            {
                AppName = appName,
                message = message,
                timestamp = CAF.Core.Helper.UtilitiesHelper.DateTimeNow(),
                stacktrace = log
            });

            var logEventInfo = new LogEventInfo(LogLevel.Fatal, "", "Exception");
            logEventInfo.Properties["CallIdentifier"] = Guid.NewGuid().ToString();
            logEventInfo.Properties["Date"] = CAF.Core.Helper.UtilitiesHelper.DateTimeNow();
            logEventInfo.Properties["Message"] = message;
            logEventInfo.Properties["StackTrace"] = log;
            logger.Error(logEventInfo);
        }
    }

    [LayoutRenderer("connectionstrings")]
    public class ConnectionStringsLayoutRenderer : LayoutRenderer
    {
        private static IConfigurationRoot _configurationRoot;

        internal IConfigurationRoot DefaultAppSettings
        {
            get => _configurationRoot;
            set => _configurationRoot = value;
        }

        /// <summary>
        /// Global configuration. Used if it has set
        /// </summary>
        public static IConfiguration AppSettings { private get; set; }

        ///<summary>
		/// The AppSetting name.
		///</summary>
		[RequiredParameter]
        [DefaultParameter]
        public string Name { get; set; }

        ///<summary>
        /// The default value to render if the AppSetting value is null.
        ///</summary>
        public string Default { get; set; }

        public ConnectionStringsLayoutRenderer()
        {

            if (AppSettings == null && DefaultAppSettings == null)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                DefaultAppSettings = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            }
        }

        /// <summary>
        /// Renders the specified application setting or default value and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (Name == null) return;

            string value = AppSettingValue;
            if (value == null && Default != null)
                value = Default;

            if (string.IsNullOrEmpty(value) == false)
                builder.Append(value);
        }

        private bool _cachedAppSettingValue = false;
        private string _appSettingValue = null;
        private string AppSettingValue
        {
            get
            {
                Name = Name.Replace('.', ':');
                if (_cachedAppSettingValue == false)
                {
                    _appSettingValue = AppSettings == null ? DefaultAppSettings.GetConnectionString(Name) : AppSettings[Name];
                    _cachedAppSettingValue = true;
                }
                return _appSettingValue;
            }
        }
    }
}
