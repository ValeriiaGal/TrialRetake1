using Microsoft.Data.SqlClient;
using Models;
using Repositories.Exceptions;
using Repositories.Interfaces;

namespace Repositories;

public class PotatoTeacherRepository : IPotatoTeacherRepository
{
    public async Task<PotatoTeacher?> GetTeacherByNameAsync(string potatoTeacherName, SqlConnection conn,
        SqlTransaction transaction)
    {
        var sql = @"SELECT * FROM PotatoTeacher q  WHERE Name = @Name";

        var result = new PotatoTeacher();
        try
        {
            await using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
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
        catch (NullReferenceException ex)
        {
            return null;
        }
    }

    public async Task<int> CreateNewTeacherByNameAsync(string potatoTeacherName, SqlConnection conn,
        SqlTransaction transaction)
    {
        var sql = @"INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)";

        try
        {
            await using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Name", potatoTeacherName);

                return (int)await cmd.ExecuteScalarAsync();
            }
        }
        catch (SqlException ex)
        {
            throw new ServerConnectionException("Object creationg exception");
        }
    }
}