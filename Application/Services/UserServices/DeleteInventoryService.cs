using inventory.CredentialSecurity;
using inventory.DataBase;
using inventory.DTOs;
using inventory.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace inventory.Services;
public interface IDeleteInvetoryService
{
    Task<Result> ExecuteDeleteInventory(DeleteInventory inventoryDetail, int UserId, CancellationToken cancellation);
}
public class DeleteInventoryService(IHasher _security, DbManager _db) : IDeleteInvetoryService
{
    public async Task<Result> ExecuteDeleteInventory(DeleteInventory inventoryDetail, int UserId, CancellationToken cancellation)
    {
        Result<int> InventoryId = await Task.Run(() => _security.DecodeHashid(inventoryDetail.InventoryId));
        if(!InventoryId.IsSuccess) return InventoryId;
        var inventory = await _db.UserInventories
                                .Where(i => i.InventoryId == InventoryId.Value)
                                .FirstOrDefaultAsync();
        if(inventory?.InvetoryStatus == false || inventory == null)
            return Result.Failure("Inventory not Found.", 400);
        inventory.InvetoryStatus = false;
        await _db.SaveChangesAsync();
        return Result.Success();
    }
}