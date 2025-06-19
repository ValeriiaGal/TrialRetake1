using Microsoft.Data.SqlClient;
using Models;
using Repositories.Exceptions;
using Repositories.Interfaces;

namespace Repositories;

public class PotatoTeacherRepository(string connectionString) : IPotatoTeacherRepository
{
    public async Task<PotatoTeacher?> GetTeacherByNameAsync(string potatoTeacherName)
    {
        await using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            var sql = @"SELECT * FROM PotatoTeacher q  WHERE Name = @Name";

            var result = new PotatoTeacher();

            await using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", potatoTeacherName);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync()) return null;

                    result = new PotatoTeacher()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                }
            }


            return result;
        }
    }
}