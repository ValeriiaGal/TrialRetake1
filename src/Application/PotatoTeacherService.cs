using Models;
using Models.DTOs;
using Services.Interfaces;
using Microsoft.Data.SqlClient;
using Repositories;
using Repositories.Exceptions;
using Repositories.Interfaces;

namespace Services;

public class PotatoTeacherService(
    IPotatoTeacherRepository pTRepository,
    IQuizRepository quizRepository,
    string _connectionStering
) : IPotatoTeacherService
{
    public async Task<List<TestDTO>> GetAllTestsAsync()
    {
        var query = await quizRepository.GetAllQuizesAsync();

        return query.Select(q => new TestDTO
        {
            Id = q.Id,
            Name = q.Name
        }).ToList();
    }

    public async Task<GetSpecificDTO> GetSpecificTestAsync(int id)
    {
        var query = await quizRepository.GetQuizByIdAsync(id);

        return new GetSpecificDTO
        {
            Id = query.Id,
            Name = query.Name,
            PotatoTeacherName = query.PotatoTeacher.Name,
            Path = query.PathFile
        };
    }

    public async Task<int> CreateTestAsync(CreateDTO test)
    {
        await using (SqlConnection conn = new SqlConnection(_connectionStering))
        {
            await conn.OpenAsync();

            await using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var teacher = await pTRepository.GetTeacherByNameAsync(test.PotatoTeacherName, conn, transaction);
                    
                    int teacherId;
                    if (teacher == null)
                    {
                        teacherId = await pTRepository.CreateNewTeacherByNameAsync(test.PotatoTeacherName, conn, transaction);
                    }
                    else
                    {
                        teacherId = teacher.Id;
                    }


                    var quiz = new Quiz
                    {
                        Name = test.Name,
                        PathFile = test.Path,
                        PotatoTeacherId = teacherId
                    };
                    var query = await quizRepository.CreateQuizAsync(quiz, conn, transaction);
                    
                    transaction.Commit();
                    
                    return query;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new ServerConnectionException("Baza upala :)");
                }
            }
        }
    }
}