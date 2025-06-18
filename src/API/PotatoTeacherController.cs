using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using Repositories.Exceptions;
using Services.Interfaces;

namespace TrialRetake1;

[ApiController]
[Route("api/quizzes")]
public class PotatoTeacherController(IPotatoTeacherService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TestDTO>>> GetAllTests()
    {
        try
        {
            var test = await service.GetAllTestsAsync();
            return Ok(test);
        }
        catch (ServerConnectionException e)
        {
            return Problem(e.Message); //code: 500
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetSpecificDTO>> GetSpecificTest(int id)
    {
        try
        {
            var test = await service.GetSpecificTestAsync(id);
            return Ok(test);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message); //code: 404
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTest([FromBody] CreateDTO dto)
    {
        try
        {
            var id = await service.CreateTestAsync(dto);
            return Created(id.ToString(), id);
        }
        catch (SqlException e)
        {
            return Problem(e.Message);
        }
    }
}