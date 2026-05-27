using inventory.CredentialSecurity;
using inventory.DataBase;
using inventory.DTOs;
using inventory.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace inventory.Services;
public interface IDeleteItemService
{
    Task<Result> ExecuteDeleteItem(int UserId, DeleteItem ItemDetails, CancellationToken cancellation);
}
public class DeleteItemService(IHasher _security, DbManager _db) : IDeleteItemService
{
    public async Task<Result> ExecuteDeleteItem(int UserId, DeleteItem ItemDetails, CancellationToken cancellation)
    {
        Result<int> ItemId = await Task.Run(() => _security.DecodeHashid(ItemDetails.ItemId));
        Result<int> IventoryId = await Task.Run(() => _security.DecodeHashid(ItemDetails.InventoryId));
        if(!ItemId.IsSuccess || !IventoryId.IsSuccess)
            return Result.Failure("Invalid or Tampered Id.", 400);
        await _db.inventoryItems
                    .Where(i => i.ItemId == ItemId.Value)
                    .Where(i => i.UserInventoryId == IventoryId.Value)
                    .ExecuteUpdateAsync(i => i
                        .SetProperty(item => item.Status, item => false)
                        .SetProperty(item => item.quantity, item => 0)
                        .SetProperty(item => item.price, item => 0)
                        .SetProperty(item => item.category, item => Categories.None)
                        , cancellation);
        return Result.Success();
    }
}