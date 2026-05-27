using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
namespace inventory.Services;
public interface IUpsertService
{
    Task<Result> UpsertExecute(StorageItems items, CancellationToken cancellation);
}
public class UpsertUService(DbManager _db) : IUpsertService

{
    public async Task<Result> UpsertExecute(StorageItems items, CancellationToken cancellation)
    {
        var incomingNames = items.Items.Select(i => i.ItemName).ToList();
        var existingNames = await  _db.Storages.Where(
                                dbItem => incomingNames.Contains(
                                    dbItem.ItemName)
                                        ).ToListAsync();
        foreach(var existingName in existingNames)
        {
            if(!existingName.Status) 
                 existingName.Status = true;
        }
        var existingnames = existingNames.Select(e => e.ItemName).ToHashSet();
        var newItems = items.Items.Where(
                                incoming => !existingnames.Contains(
                                    incoming.ItemName))
                                        .ToList();
        if (newItems.Any())
        {
            var ItemsToAdd = newItems.Select(newItem => new Storage
            {
                ItemName = newItem.ItemName,
                Status = true
            }
            );
            await _db.AddRangeAsync(ItemsToAdd, cancellation);
            await _db.SaveChangesAsync(cancellation);
        }
        return Result.Success();
    }
}
