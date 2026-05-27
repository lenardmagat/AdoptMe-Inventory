using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
namespace inventory.Services;
public interface IGetItemLibraryService
{
    Task<Result<List<ItemLibraryData>>> ExecuteGetItemLibrary(CancellationToken cancellation);
}
public class GetPetLibraryService(IHasher _security, DbManager _db) : IGetItemLibraryService
{
    public async Task<Result<List<ItemLibraryData>>> ExecuteGetItemLibrary(CancellationToken cancellation)
    {

        List<Storage> petDatas = await _db.Storages
                                    .AsNoTracking()
                                    .Where(d => d.Status == true)
                                    .ToListAsync(cancellation);
        List<ItemLibraryData> DataToReturn 
                                = petDatas
                                    .Select( p => 
                                        new ItemLibraryData(
                                            p.ItemName,
                                            _security.encodeHashidsId(p.StorageId)) 
                                        )
                                    .ToList();
        return Result<List<ItemLibraryData>>.Success(DataToReturn);
    }
}