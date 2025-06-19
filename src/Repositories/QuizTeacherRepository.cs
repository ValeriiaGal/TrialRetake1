using Microsoft.Data.SqlClient;
using Models;
using Models.DTOs;
using Repositories.Interfaces;

namespace Repositories;

public class QuizTeacherRepository(string connectionString) : IQuizTeacherRepository
{
    public async Task<int> CreateQuizTeacherAsync(int teacherId, CreateDTO dto)
    {
        await using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            await sqlConnection.OpenAsync();


            await using (var transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    if (teacherId < 0)
                    {
                        var teacherSql = @"INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)";


                        await using (SqlCommand cmd = new SqlCommand(teacherSql, sqlConnection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", dto.PotatoTeacherName);

                            teacherId = (int)await cmd.ExecuteScalarAsync();
                        }
                    }

                    var quizSql =
                        @"INSERT INTO Quiz (Name, PotatoTeacherId, PathFile) OUTPUT INSERTED.Id VALUES (@Name, @PotatoTeacherId, @PathFile);";

                    await using (SqlCommand cmd = new SqlCommand(quizSql, sqlConnection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Name", dto.Name);
                        cmd.Parameters.AddWithValue("@PotatoTeacherId", teacherId);
                        cmd.Parameters.AddWithValue("@PathFile", dto.Path);

                        await transaction.CommitAsync();

                        return (int)await cmd.ExecuteScalarAsync();
                    }
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    return -1;
                }
            }
        }
    }
}