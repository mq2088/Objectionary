namespace Objectionary.Models;

public record GenerateResponse(string Text);

public record ErrorResponse(string Error, string? Limit, string Message);
