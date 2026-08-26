using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace AMS_DeviceAgent
{
    public class LocalAgentServer
    {
        private readonly IConfiguration _configuration;
        private HttpListener? _listener;
        private bool _running;

        public LocalAgentServer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:5055/");

            _listener.Start();

            Task.Run(ListenLoop);
        }

        public void Stop()
        {
            _running = false;

            try
            {
                _listener?.Stop();
                _listener?.Close();
            }
            catch
            {
            }
        }

        private async Task ListenLoop()
        {
            while (_running && _listener != null)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
                catch
                {
                    if (!_running)
                        break;
                }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                AddCorsHeaders(context.Response);

                // CORS
                if (context.Request.HttpMethod == "OPTIONS")
                {
                    context.Response.StatusCode = 204;
                    context.Response.Close();
                    return;
                }

                // LICENSE
                if (context.Request.Url?.AbsolutePath == "/device/license")
                {
                    var client = new DeviceLicenseClient(_configuration);
                    var result = await client.CheckLicenseAsync();

                    var response = new
                    {
                        machineKey = result.MachineKey,
                        machineName = Environment.MachineName,
                        licensed = result.Licensed,
                        status = result.Status
                    };

                    await WriteJson(context.Response, response);
                    return;
                }

                // MARK
                if (context.Request.Url?.AbsolutePath == "/device/mark"
                    && context.Request.HttpMethod == "POST")
                {
                    using var reader = new StreamReader(
                        context.Request.InputStream,
                        context.Request.ContentEncoding);

                    var body = await reader.ReadToEndAsync();

                    var request = JsonSerializer.Deserialize<MarkRequest>(
                        body,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (request == null || request.JobId <= 0)
                    {
                        context.Response.StatusCode = 400;

                        await WriteJson(context.Response, new
                        {
                            success = false,
                            error = "JobId is required."
                        });

                        return;
                    }

                    var markMaster =
                        new MarkMasterAutomation(_configuration);

                    var markMasterResult =
                        markMaster.OpenLayout();

                    await WriteJson(context.Response, new
                    {
                        success = markMasterResult.Success,
                        jobId = request.JobId,
                        markMaster = markMasterResult.Message
                    });

                    return;
                }

                // NOT FOUND
                context.Response.StatusCode = 404;

                await WriteJson(context.Response, new
                {
                    error = "Not found"
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                await WriteJson(context.Response, new
                {
                    error = ex.Message
                });
            }
        }

        private class MarkRequest
        {
            public int JobId { get; set; }
        }

        private static void AddCorsHeaders(
            HttpListenerResponse response)
        {
            response.Headers.Add(
                "Access-Control-Allow-Origin",
                "*");

            response.Headers.Add(
                "Access-Control-Allow-Methods",
                "GET, POST, OPTIONS");

            response.Headers.Add(
                "Access-Control-Allow-Headers",
                "Content-Type");
        }

        private static async Task WriteJson(
            HttpListenerResponse response,
            object data)
        {
            response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase
                });

            var buffer = Encoding.UTF8.GetBytes(json);

            response.ContentLength64 = buffer.Length;

            await response.OutputStream.WriteAsync(
                buffer,
                0,
                buffer.Length);

            response.OutputStream.Close();
        }
    }
}