using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;

using Microsoft.EntityFrameworkCore;
namespace inventory.Services;
public interface IUpsertItemasync
{
    Task<Result> ExecuteUpsertItemasync(int id, UpsertItemDetails itemDetails, CancellationToken cancellation);
}
public class UpsertItemasync(IHasher _security, DbManager _db) : IUpsertItemasync
{
    public async Task<Result> ExecuteUpsertItemasync(int id, UpsertItemDetails itemDetails, CancellationToken cancellation)
    {
        Result<int> inventoryId = await Task.Run(() => _security.DecodeHashid(itemDetails.InventoryId));
            if(!inventoryId.IsSuccess) return Result.Failure(inventoryId.Error ?? "INvalid Credentials", inventoryId.StatusCode);
        Result<int> storageId = await Task.Run(() => _security.DecodeHashid(itemDetails.StorageId));
            if(!storageId.IsSuccess) return Result.Failure(storageId.Error ?? "INvalid Credentials", storageId.StatusCode);
        var userInventory = await _db.UserInventories
                                .Where(i=> i.InventoryId == inventoryId.Value && i.UserId == id)
                                .Select(i => new
                                {
                                   InventoryItem  = i,
                                   ExisitingItem = i.Items.FirstOrDefault(item => item.StorageId == storageId.Value),
                                   Itemtemplate = _db.Storages.Any(i => i.StorageId == storageId.Value)
                                })
                                .FirstOrDefaultAsync(cancellation);
        if(userInventory == null)
            return Result.Failure("Wrong credentials", 403);
        if(!itemDetails.Category.HasValue) return Result.Failure("No Category Selected", 400);
        if(userInventory.ExisitingItem != null)
        {
            var inventory = userInventory.ExisitingItem;
            inventory.category = itemDetails.Category.Value;
            inventory.price = itemDetails.Price;
            inventory.quantity = itemDetails.Quantity;
            await _db.SaveChangesAsync(cancellation);
            return Result.Success();
        }
        else
        {
            if(userInventory.Itemtemplate == false) 
                return Result.Failure("Wrong item Id.", 400);
            var NewItem = new InventoryItem
            {
                StorageId = storageId.Value,
                UserInventoryId = inventoryId.Value,
                category = itemDetails.Category.Value,
                price = itemDetails.Price,
                quantity = itemDetails.Quantity
            };
            await _db.inventoryItems.AddAsync(NewItem,cancellation);
            await _db.SaveChangesAsync();
            return Result.Success();
        }
    }
}