using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System;

namespace net_registri_log.Shared.ExtensionMethods
{
    public static class LogsExtension
    {
        public static void LogTracePushProperty(this ILogger logger, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogTrace(message, args);
            ClearPushProperty();
        }

        public static void LogDebugPushProperty(this ILogger logger, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogDebug(message, args);
            ClearPushProperty();
        }

        public static void LogInformationPushProperty(this ILogger logger, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogInformation(message, args);
            ClearPushProperty();
        }

        public static void LogWarningPushProperty(this ILogger logger, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogWarning(message, args);
            ClearPushProperty();
        }

        public static void LogCriticalPushProperty(this ILogger logger, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogCritical(message, args);
            ClearPushProperty();
        }

        public static void LogErrorPushProperty(this ILogger logger, Exception exception, string message, object[] args = null, object jsonObject = null, object operation = null)
        {
            SetPushProperty(jsonObject, operation);
            logger.LogError(exception, message, args);
            ClearPushProperty();
        }

        private static void SetPushProperty(object jsonObject, object operation)
        {
            LogContext.PushProperty("JsonObject", JsonConvert.SerializeObject(jsonObject));
            LogContext.PushProperty("Operation", operation?.ToString());
        }

        private static void ClearPushProperty()
        {
            LogContext.PushProperty("JsonObject", null);
            LogContext.PushProperty("Operation", null);
        }
    }
}
