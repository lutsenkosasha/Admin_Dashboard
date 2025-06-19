using Microsoft.EntityFrameworkCore;
namespace backend.Models;

public class Payment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}