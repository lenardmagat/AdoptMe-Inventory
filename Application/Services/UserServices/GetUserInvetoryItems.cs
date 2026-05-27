using inventory.ErrorHandling;
using inventory.DTOs;
using inventory.CredentialSecurity;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
namespace inventory.Services;
public interface IGetUserInvetoryItemsService
{
    Task<Result<List<UserInventoryItemData>>> ExecuteGetUserInventoryItems(string InventoryHashedId, CancellationToken cancellation);
}
public class GetUserInventoryItemsService(IHasher _security, DbManager _db) : IGetUserInvetoryItemsService
{
    public async Task<Result<List<UserInventoryItemData>>> ExecuteGetUserInventoryItems(string InventoryHashedId, CancellationToken cancellation)
    {
        Result<int> InventoryId = await Task.Run(() => _security.DecodeHashid(InventoryHashedId));
            if(!InventoryId.IsSuccess) return Result<List<UserInventoryItemData>>.Failure(InventoryId.Error ?? "Invalid Credential", InventoryId.StatusCode);
        var rawDatas = await _db.inventoryItems
                                .AsNoTracking()
                                .Where(i=> i.UserInventoryId == InventoryId.Value && i.Owner.InvetoryStatus == true)
                                .Select(i => new
                                {
                                    i.category,
                                    i.ItemId,
                                    i.price,
                                    i.quantity,
                                    i.ItemAttributes.ItemName,
                                    i.Owner.Owner.Username,
                                    i.StorageId
                                }
                                )
                                .ToListAsync(cancellation);
        if(rawDatas == null) return Result<List<UserInventoryItemData>>.Failure("Invalid Credentials", 400);
        List<UserInventoryItemData> InventoryItems =
                                            await Task.Run(() => 
                                                rawDatas.Select( i => new UserInventoryItemData
                                                {
                                                    ItemId = _security.encodeHashidsId(i.ItemId),
                                                    StorageId = _security.encodeHashidsId(i.StorageId),
                                                    Username = i.Username,
                                                    ItemName = i.ItemName,
                                                    Quantity = i.quantity,
                                                    Price = i.price,
                                                    Category = i.category.ToString()
                                                }
                                                ).ToList()
                                            );
        return Result<List<UserInventoryItemData>>.Success(InventoryItems);
    }
}