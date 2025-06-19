using Microsoft.Data.SqlClient;
using Models;

namespace Repositories.Interfaces;

public interface IPotatoTeacherRepository
{
    public Task<PotatoTeacher?> GetTeacherByNameAsync(string potatoTeacherName);
    public Task<int> CreateNewTeacherByNameAsync(string potatoTeacherName);
    
}