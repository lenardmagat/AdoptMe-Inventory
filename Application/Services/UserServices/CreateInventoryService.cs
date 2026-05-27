using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
using inventory.CredentialSecurity;
namespace inventory.Services;

public interface ICreateInventoryService
{
    Task<Result<string>> ExecuteCreateInVentory(int UserId, CreateInventoryDetails inventoryDetails, CancellationToken cancellation);
}
public class CreateInventoryService(IHasher _security, DbManager _db) : ICreateInventoryService
{
    public async Task<Result<string>> ExecuteCreateInVentory (int userId, CreateInventoryDetails inventoryDetails, CancellationToken cancellation)
    {
        UserInventory? inventory = await _db.UserInventories
                                        .AsNoTracking()
                                        .Where(i => i.UserId == userId)
                                        .Where(d => d.InventoryName == inventoryDetails.InventoryName)
                                        .FirstOrDefaultAsync();
        if(inventory != null) return Result<string>.Failure("Inventory name is already exist.", 409);
        UserInventory userInventoryToAdd = new UserInventory{
                                UserId = userId, 
                                InventoryName = 
                                inventoryDetails.InventoryName,
                                InvetoryStatus = true,
                                DateCreated = DateTime.UtcNow.ToLongTimeString()};
        await _db.UserInventories.AddAsync(userInventoryToAdd);
        await _db.SaveChangesAsync();
        return Result<string>.Success(await Task.Run(() => _security.encodeHashidsId(userInventoryToAdd.InventoryId)));
    }
}