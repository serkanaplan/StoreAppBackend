using NLog;
using Store.Service.Contracts;

namespace Store.Service;

public class LoggerManager : ILoggerService
{
    //ctor ile injection yapmadık çünkü herkes aynı örneği kullanacak
    private static ILogger _logger = LogManager.GetCurrentClassLogger();
    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogInfo(string message) => _logger.Info(message);

    public void LogWarning(string message) => _logger.Warn(message);
}
