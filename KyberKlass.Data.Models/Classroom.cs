﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KyberKlass.Common.EntityValidations.Classroom;

namespace KyberKlass.Data.Models;
public class Classroom
{
    public Classroom()
    {
        Id = Guid.NewGuid();

        Students = new HashSet<Student>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string Name { get; set; } = null!;

    [Required]
    public Guid TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; } = true;

    public ICollection<Student> Students { get; set; }

    [Required]
    public Guid SchoolId { get; set; }

    [ForeignKey(nameof(SchoolId))]
    public School School { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}