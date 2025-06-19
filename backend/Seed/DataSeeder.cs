using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Seed;

public static class DataSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Clients.Any())
        {
            var clients = new[]
            {
                new Client { Name = "Alice", Email = "alice@mail.com", BalanceT = 100 },
                new Client { Name = "Bob", Email = "bob@mail.com", BalanceT = 50 },
                new Client { Name = "Charlie", Email = "charlie@mail.com", BalanceT = 75 }
            };
            db.Clients.AddRange(clients);
            db.SaveChanges();
        }

        if (!db.Payments.Any())
        {
            var clients = db.Clients.ToList();

            var payments = new[]
            {
                new Payment { ClientId = clients[0].Id, Amount = 10, Timestamp = DateTime.UtcNow },
                new Payment { ClientId = clients[1].Id, Amount = 20, Timestamp = DateTime.UtcNow },
                new Payment { ClientId = clients[2].Id, Amount = 30, Timestamp = DateTime.UtcNow },
                new Payment { ClientId = clients[0].Id, Amount = 5, Timestamp = DateTime.UtcNow },
                new Payment { ClientId = clients[1].Id, Amount = 15, Timestamp = DateTime.UtcNow }
            };
            db.Payments.AddRange(payments);
        }

        if (!db.Rates.Any())
        {
            db.Rates.Add(new Rate { TokenRate = 10 });
        }

        db.SaveChanges();
    }
}
