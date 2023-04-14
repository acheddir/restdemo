namespace BookStore.Dto;

public record UpdateBookCommand(int Id, string ISBN, string Title, string Year);
