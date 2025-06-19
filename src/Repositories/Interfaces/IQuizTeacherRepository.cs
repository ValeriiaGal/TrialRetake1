using Models;

namespace Repositories.Interfaces;

public interface IQuizTeacherRepository
{
    public Task<int> CreateQuizTeacherAsync(int teacherId);
}