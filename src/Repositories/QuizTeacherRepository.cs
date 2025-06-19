using Microsoft.Data.SqlClient;
using Models;
using Repositories.Interfaces;

namespace Repositories;

public class QuizTeacherRepository(string connectionString) : IQuizTeacherRepository
{
    public async Task<int> CreateQuizTeacherAsync(int teacherId)
    {
        await using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
          await sqlConnection.OpenAsync();
            var sql = @"INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)";

      
            await using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Name", potatoTeacherName);

                return (int)await cmd.ExecuteScalarAsync();
            }
        }
    }
}