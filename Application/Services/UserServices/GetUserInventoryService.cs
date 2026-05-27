using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
namespace inventory.Services;
public interface IGetUserInventoriesService
{
    Task<Result<List<UserInventoryData>>> ExecuteGetUserInventories(int UserId, CancellationToken cancellation);
}
public class GetUserInventoryService(IHasher _security, DbManager _db) : IGetUserInventoriesService
{
    public async Task<Result<List<UserInventoryData>>> ExecuteGetUserInventories(int UserId, CancellationToken cancellation)
    {
        var UserInvetories = await _db.UserInventories
                                .AsNoTracking()
                                .Where(d => d.UserId == UserId && d.InvetoryStatus == true)
                                .Select(i => new
                                {
                                    i.InventoryId,
                                    i.InventoryName,
                                    i.DateCreated    
                                }
                                    )
                                .ToListAsync(cancellation);
        if(UserInvetories == null) return Result<List<UserInventoryData>>.Failure("No data to transfer", 204);
        List<UserInventoryData> userInventoryDatas = await Task.Run(() => UserInvetories
                                    .Select(i => new UserInventoryData
                                    {
                                        InventoryId = _security.encodeHashidsId(i.InventoryId),
                                        InventoryName = i.InventoryName,
                                        DateCreate = i.DateCreated
                                    } 
                                ).ToList()
                                );
        return Result<List<UserInventoryData>>.Success(userInventoryDatas);
    }
}