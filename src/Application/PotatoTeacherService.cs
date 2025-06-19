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
    IQuizTeacherRepository quizTeacherRepository
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
        var teacher = await pTRepository.GetTeacherByNameAsync(test.PotatoTeacherName);

        int teacherId;
        if (teacher == null)
        {
            teacherId = await quizTeacherRepository.CreateQuizTeacherAsync(-1, test);
        }
        else
        {
            teacherId = await quizTeacherRepository.CreateQuizTeacherAsync(teacher.Id, test);
        }

        return teacherId;
    }
}