using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
using inventory.CredentialSecurity;
namespace inventory.Services;
public interface IRemoveItems
{
    Task<Result> RemoveExecute(ItemHashids ItemsHashids, CancellationToken cancellation);
}
public class RemoveItems(IHasher _security, DbManager _db) : IRemoveItems
{
    public async Task<Result> RemoveExecute(ItemHashids ItemHashids, CancellationToken cancellation)
    {
        Result<List<int>> IDs = await Task.Run(() => _security.DecodeMultipleHashids(ItemHashids.Items));
            if(!IDs.IsSuccess) return Result.Failure(IDs.Error ?? "Invalid Credentials", IDs.StatusCode);
        if(IDs.Value == null)
            return Result.Failure("Invalid IDs", 403);
        await _db.Storages
                .Where(
                    DbItem => IDs.Value.Contains(
                        DbItem.StorageId
                        )
                    )
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        item => item.Status, false
                        ),
                    cancellation
                    );
        return Result.Success();
    }
}