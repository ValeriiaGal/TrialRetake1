using Models;
using Models.DTOs;

namespace Repositories.Interfaces;

public interface IQuizTeacherRepository
{
    public Task<int> CreateQuizTeacherAsync(int teacherId, CreateDTO dto);
}