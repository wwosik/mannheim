using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Logging;
using Google.Apis.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GoogleApis = global::Google.Apis;

namespace Mannheim.Google
{
    public class GoogleLogForwarder : global::Google.Apis.Logging.BaseLogger
    {
        private readonly ILogger<GoogleLogForwarder> logger;
        private static readonly Dictionary<GoogleApis.Logging.LogLevel, Microsoft.Extensions.Logging.LogLevel> levelsMap = new Dictionary<GoogleApis.Logging.LogLevel, Microsoft.Extensions.Logging.LogLevel>
        {
            { GoogleApis.Logging.LogLevel.Warning, Microsoft.Extensions.Logging.LogLevel.Warning },
            { GoogleApis.Logging.LogLevel.Info, Microsoft.Extensions.Logging.LogLevel.Information },
            { GoogleApis.Logging.LogLevel.Error, Microsoft.Extensions.Logging.LogLevel.Error },
            { GoogleApis.Logging.LogLevel.Debug, Microsoft.Extensions.Logging.LogLevel.Debug },
        };

        public GoogleLogForwarder(ILogger<GoogleLogForwarder> logger)
            : base(GoogleApis.Logging.LogLevel.All, GoogleApis.Util.SystemClock.Default, forType: null)
        {
            this.logger = logger;
        }

        protected override GoogleApis.Logging.ILogger BuildNewLogger(Type type)
        {
            return this;
        }

        protected override void Log(GoogleApis.Logging.LogLevel logLevel, string formattedMessage)
        {
            this.logger.Log(levelsMap[logLevel], formattedMessage);
        }

        public void Install()
        {
            global::Google.ApplicationContext.RegisterLogger(this);
        }

        public static void Install(IServiceProvider serviceProvider) {
            var forwarder = ActivatorUtilities.CreateInstance<GoogleLogForwarder>(serviceProvider);
            forwarder.Install();
        }
    }
}
