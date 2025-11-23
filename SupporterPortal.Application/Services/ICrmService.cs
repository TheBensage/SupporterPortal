using SupporterPortal.Application.Models.Crm;

namespace SupporterPortal.Application.Services;
public interface ICrmService
{
    Task SendRecordAsync(CrmRecord record);

    Task<CrmRecord?> GetRecordByEmailAsync(string email);
}
