using Microsoft.EntityFrameworkCore;
using inventory.Models;
namespace inventory.DataBase;

public class DbManager : DbContext
{
    public DbSet<User> Users {get; set;} = null!;
    public DbSet<UserInventory> UserInventories {get; set;} = null!;
    public DbSet<Storage> Storages {get; set;} = null!;
    public DbSet<InventoryItem> inventoryItems {get; set;} = null!;

    public DbManager(DbContextOptions<DbManager> options) : base(options){
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.UserId);
        modelBuilder.Entity<UserInventory>().HasIndex(ui => ui.UserId);
        modelBuilder.Entity<InventoryItem>().HasIndex(i => i.ItemId);
        modelBuilder.Entity<Storage>().HasIndex(s => s.StorageId);
    }
}
