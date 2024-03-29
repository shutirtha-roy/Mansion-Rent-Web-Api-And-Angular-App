﻿using MansionRentBackend.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MansionRentBackend.Application.Entities;

public sealed class Mansion : IEntity<Guid>, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Details { get; set; }
    public double? Rate { get; set; }
    public int? Sqft { get; set; }
    public int? Occupancy { get; set; }
    public string? Base64Image { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}