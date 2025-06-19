using Microsoft.Data.SqlClient;
using Models;
using Models.DTOs;

namespace Repositories.Interfaces;

public interface IQuizRepository
{
    public Task<IEnumerable<Quiz>> GetAllQuizesAsync();
    public Task<Quiz> GetQuizByIdAsync(int id);
}