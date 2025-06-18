using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class CreateDTO
{
    [Required]
    public string Name { get; set; } // quiz name
    [Required]
    public string PotatoTeacherName { get; set; }
    [Required]
    public string Path { get; set; }
}