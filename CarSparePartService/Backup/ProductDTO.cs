﻿namespace CarSparePartService.Backup;

public record ProductDTO
{
    public long ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
}