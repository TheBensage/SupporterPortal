using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SupporterPortal.Application.Models.Crm;
using SupporterPortal.Application.Services;

namespace SupporterPortal.Infrastructure.Services;

public class CrmService : ICrmService
{
    private static readonly ConcurrentDictionary<string, CrmRecord> _records = new();
    private readonly ILogger<CrmService> _logger;

    public CrmService(ILogger<CrmService> logger)
    {
        _logger = logger;
    }

    public Task<CrmRecord?> GetRecordByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("GetRecordByEmailAsync called with empty email.");
            return Task.FromResult<CrmRecord?>(null);
        }

        _records.TryGetValue(email.ToLower(), out var existing);
        if (existing != null)
        {
            _logger.LogInformation("Found existing record for {Email}", email);
        }
        else
        {
            _logger.LogInformation("No record found for {Email}", email);
        }

        return Task.FromResult(existing);
    }
    public Task SendRecordAsync(CrmRecord record)
    {
        if (record == null || string.IsNullOrWhiteSpace(record.Email))
        {
            _logger.LogWarning("SendRecordAsync called with invalid record.");
            return Task.CompletedTask;
        }

        string key = record.Email.ToLower();

        if (!_records.ContainsKey(key))
        {
            record.Id = Guid.NewGuid();
            _records[key] = record;
            _logger.LogInformation("Added new record: {Email}", record.Email);
        }
        else
        {
            _logger.LogInformation("Record already exists, not adding: {Email}", record.Email);
        }

        return Task.CompletedTask;
    }
}
