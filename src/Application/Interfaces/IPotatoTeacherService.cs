using Models.DTOs;

namespace Services.Interfaces;

public interface IPotatoTeacherService
{
    public Task<List<TestDTO>> GetAllTestsAsync();
    public Task<GetSpecificDTO> GetSpecificTestAsync(int id);
    public Task<int> CreateTestAsync(CreateDTO test);
}