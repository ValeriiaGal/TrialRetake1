﻿namespace Models;

public class Quiz
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PathFile { get; set; }
    
    public int PotatoTeacherId { get; set; }
    public PotatoTeacher PotatoTeacher { get; set; }
}