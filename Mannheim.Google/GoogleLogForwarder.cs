using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using GoogleLogging = global::Google.Apis.Logging;
using MicrosoftLogging = Microsoft.Extensions.Logging;

namespace Mannheim.Google
{
    public class GoogleLogForwarder : GoogleLogging.BaseLogger
    {
        private readonly MicrosoftLogging.ILogger logger;

        public GoogleLogForwarder(MicrosoftLogging.ILogger logger, Type t = null) : base(GoogleLogging.LogLevel.Debug, global::Google.Apis.Util.SystemClock.Default, t)
        {
            this.logger = logger;
        }

        protected override GoogleLogging.ILogger BuildNewLogger(Type type)
        {
            return new GoogleLogForwarder(this.logger, type);
        }

        protected override void Log(GoogleLogging.LogLevel logLevel, string formattedMessage)
        {
            switch (logLevel)
            {
                case GoogleLogging.LogLevel.Debug:
                    this.logger.LogDebug(formattedMessage);
                    break;
                case GoogleLogging.LogLevel.Error:
                    this.logger.LogError(formattedMessage);
                    break;
                case GoogleLogging.LogLevel.Warning:
                    this.logger.LogWarning(formattedMessage);
                    break;
                case GoogleLogging.LogLevel.Info:
                default:
                    this.logger.LogInformation(formattedMessage);
                    break;
            }
        }
    }
}
