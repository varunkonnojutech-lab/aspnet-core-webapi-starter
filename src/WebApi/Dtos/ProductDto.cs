namespace WebApi.Dtos;

public record ProductDto(int Id, string Name, string? Description, decimal Price, int Stock);
