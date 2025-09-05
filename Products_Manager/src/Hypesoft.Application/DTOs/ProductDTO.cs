namespace Hypesoft.Application.DTOs;


public record ProductReadDto(
string Id, string Name, string Description, decimal Price, string CategoryId, int StockQuantity);


public record ProductCreateDto(
string Name, string Description, decimal Price, string CategoryId, int StockQuantity);


public record ProductUpdateDto(
string Id, string Name, string Description, decimal Price, string CategoryId, int StockQuantity);