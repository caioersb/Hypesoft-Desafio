namespace Hypesoft.Application.DTOs;


public record CategoryReadDto(
string Id, string Name, string Description);


public record CategoryCreateDto(
string Name, string Description);


public record CategoryUpdateDto(
string Id, string Name, string Description);