using Microsoft.Data.SqlClient;
using Models;
using Repositories.Exceptions;
using Repositories.Interfaces;

namespace Repositories;

public class PotatoTeacherRepository : IPotatoTeacherRepository
{
    public async Task<PotatoTeacher?> GetTeacherByNameAsync(string potatoTeacherName)
    {
        var sql = @"SELECT * FROM PotatoTeacher q  WHERE Name = @Name";

        var result = new PotatoTeacher();

            await using (SqlCommand cmd = new SqlCommand(sql))
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

    public async Task<int> CreateNewTeacherByNameAsync(string potatoTeacherName)
    {
        var sql = @"INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)";

      
            await using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Name", potatoTeacherName);

                return (int)await cmd.ExecuteScalarAsync();
            }
        }
    
    }
