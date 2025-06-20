﻿using Microsoft.Data.SqlClient;
using Models;
using Models.DTOs;
using Repositories.Exceptions;
using Repositories.Interfaces;

namespace Repositories;

public class QuizRepository(string connectionString) : IQuizRepository
{
    public async Task<IEnumerable<Quiz>> GetAllQuizesAsync()
    {
        var sql = @"SELECT id, name FROM Quiz";

        var result = new List<Quiz>();

        try
        {
            await using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                await using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Quiz
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                            });
                        }
                    }
                }
            }

            return result;
        }
        catch (SqlException ex)
        {
            throw new ServerConnectionException("Error retrieving quizes");
        }
    }

    public async Task<Quiz> GetQuizByIdAsync(int id)
    {
        var sql = @"SELECT * FROM Quiz q JOIN PotatoTeacher pt ON q.PotatoTeacherId = pt.id WHERE q.id = @id";

        var result = new Quiz();

        await using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();

            await using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync()) throw new NotFoundException("Quiz not found");

                    result = new Quiz
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PotatoTeacherId = reader.GetInt32(2),
                        PathFile = reader.GetString(3),
                        PotatoTeacher = new PotatoTeacher
                        {
                            Id = reader.GetInt32(4),
                            Name = reader.GetString(5)
                        }
                    };
                }
            }
        }

        return result;
    }
}