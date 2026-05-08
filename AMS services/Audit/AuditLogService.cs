using AMS_data;
using AMS_data.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace AMS_services.Audit
{
    public class AuditLogService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(
            string action,
            string entityName,
            string? entityId = null,
            object? oldValues = null,
            object? newValues = null)
        {
            var http = _httpContextAccessor.HttpContext;
            var user = http?.User;

            var audit = new AuditLog
            {
                UserId = user?.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = user?.Identity?.Name,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                OldValues = oldValues == null ? null : JsonSerializer.Serialize(oldValues),
                NewValues = newValues == null ? null : JsonSerializer.Serialize(newValues),
                IpAddress = http?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = http?.Request.Headers["User-Agent"].ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _db.AuditLogs.Add(audit);
            await _db.SaveChangesAsync();
        }
    }
}