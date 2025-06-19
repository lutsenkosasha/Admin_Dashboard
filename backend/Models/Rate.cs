using Microsoft.EntityFrameworkCore;
namespace backend.Models;

public class Rate
{
    public int Id { get; set; }
    public decimal TokenRate { get; set; }
}