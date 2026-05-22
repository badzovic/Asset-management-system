using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace AMS_DeviceAgent
{
    public class DeviceLicenseClient
    {
        private readonly IConfiguration _configuration;

        public DeviceLicenseClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(bool Licensed, string Status, string MachineKey)> CheckLicenseAsync()
        {
            var licenseServerUrl = _configuration["LicenseServer:BaseUrl"];
            var apiKey = _configuration["LicenseServer:ApiKey"];

            var machineKey = GetMachineKey();

            if (string.IsNullOrWhiteSpace(licenseServerUrl))
                return (false, "LicenseServerUrlMissing", machineKey);

            if (string.IsNullOrWhiteSpace(apiKey))
                return (false, "LicenseApiKeyMissing", machineKey);

            try
            {
                using var client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                var request = new
                {
                    machineKey,
                    machineName = Environment.MachineName
                };

                var response = await client.PostAsJsonAsync(
                    $"{licenseServerUrl.TrimEnd('/')}/api/license/validate",
                    request);

                if (!response.IsSuccessStatusCode)
                    return (false, $"LicenseServerHttp{(int)response.StatusCode}", machineKey);

                var result = await response.Content.ReadFromJsonAsync<LicenseValidateResponse>();

                return (
                    result?.Licensed == true,
                    result?.Status ?? "Unknown",
                    machineKey
                );
            }
            catch (HttpRequestException)
            {
                return (false, "LicenseServerUnavailable", machineKey);
            }
            catch (TaskCanceledException)
            {
                return (false, "LicenseServerTimeout", machineKey);
            }
            catch (Exception)
            {
                return (false, "LicenseCheckError", machineKey);
            }
        }

        private static string GetMachineKey()
        {
            var raw = $"{Environment.MachineName}_{Environment.UserName}";

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));

            return Convert.ToHexString(bytes);
        }

        public async Task<(bool Success, string Status, string MachineKey)> RequestActivationAsync(string? requestedBy = null, string? note = null)
        {
            var licenseServerUrl = _configuration["LicenseServer:BaseUrl"];
            var apiKey = _configuration["LicenseServer:ApiKey"];

            var machineKey = GetMachineKey();

            if (string.IsNullOrWhiteSpace(licenseServerUrl))
                return (false, "LicenseServerUrlMissing", machineKey);

            if (string.IsNullOrWhiteSpace(apiKey))
                return (false, "LicenseApiKeyMissing", machineKey);

            try
            {
                using var client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                var request = new
                {
                    machineKey,
                    machineName = Environment.MachineName,
                    requestedBy,
                    note
                };

                var response = await client.PostAsJsonAsync(
                    $"{licenseServerUrl.TrimEnd('/')}/api/license/request-activation",
                    request);

                if (!response.IsSuccessStatusCode)
                    return (false, $"Http{(int)response.StatusCode}", machineKey);

                return (true, "ActivationRequestSent", machineKey);
            }
            catch (HttpRequestException)
            {
                return (false, "LicenseServerUnavailable", machineKey);
            }
            catch (TaskCanceledException)
            {
                return (false, "LicenseServerTimeout", machineKey);
            }
            catch
            {
                return (false, "ActivationRequestError", machineKey);
            }
        }
        public class LicenseValidateResponse
        {
            public bool Licensed { get; set; }
            public string Status { get; set; } = "";
            public DateTime? ValidUntil { get; set; }
            public string? Message { get; set; }
        }
    }
}