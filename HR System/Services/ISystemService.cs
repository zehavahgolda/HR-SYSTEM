using HR_System.DTOs.Systems;

namespace HR_System.Services
{
    public interface ISystemService
    {
   
        Task<List<SystemListItemDto>> GetSystemsAsync(
            int? year = null,
            string? status = null,
            string? ownerManagerName = null,
            string? search = null);

      
        Task<SystemDetailsDto?> GetSystemByIdAsync(string id);
    }
}
