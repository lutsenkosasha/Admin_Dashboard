using Microsoft.EntityFrameworkCore;
namespace backend.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal BalanceT { get; set; }
}