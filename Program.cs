using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? throw new ArgumentNullException("BOT_TOKEN environment variable is not set.");
    private static readonly string chatId = Environment.GetEnvironmentVariable("CHAT_ID") ?? throw new ArgumentNullException("CHAT_ID environment variable is not set.");
    private static readonly string healthchecksUrl = Environment.GetEnvironmentVariable("HEALTHCHECKS_URL") ?? throw new ArgumentNullException("HEALTHCHECKS_URL environment variable is not set.");
    private static readonly string[] services = { "your-app", "error-log-monitor" };

    static async Task Main(string[] args)
    {
        var downServices = CheckServices();

        if (downServices.Length > 0)
        {
            string message = $"🚨 Alert: The following services are down on {Environment.MachineName}:\n";
            foreach (var service in downServices)
            {
                message += $"- {service}\n";
            }
            await SendTelegramMessage(message);
        }

        await PingHealthchecks();
    }

    private static string[] CheckServices()
    {
        var downServices = new System.Collections.Generic.List<string>();

        foreach (var service in services)
        {
            if (!IsServiceActive(service))
            {
                downServices.Add(service);
            }
        }

        return downServices.ToArray();
    }

    private static bool IsServiceActive(string serviceName)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "systemctl",
                Arguments = $"is-active --quiet {serviceName}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        return process.ExitCode == 0;
    }

    private static async Task SendTelegramMessage(string message)
    {
        using (var client = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("chat_id", chatId),
                new KeyValuePair<string, string>("text", message),
                new KeyValuePair<string, string>("parse_mode", "HTML")
            });

            await client.PostAsync(url, content);
        }
    }

    private static async Task PingHealthchecks()
    {
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            await client.GetAsync(healthchecksUrl);
        }
    }
}