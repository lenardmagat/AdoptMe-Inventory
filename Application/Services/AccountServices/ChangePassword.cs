using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
namespace inventory.Services;
public interface IChangePasswordServices{
    Task<Result> ChangePasswordExecute(ChangePasswordCredentials credentials, CancellationToken cancellationToken, int userId);
}
public class ChangePasswordServices(IHasher _security, DbManager _db) : IChangePasswordServices
{
    public async Task<Result> ChangePasswordExecute(ChangePasswordCredentials credentials, CancellationToken cancellationToken, int userId)
    {
        User? user = await _db.Users.FindAsync(userId, cancellationToken);
        if(user == null) return Result.Failure("Can't find user.", 404);
        if(!await Task.Run(() => _security.VerifyPassword(credentials.Password, user.HashedPassword)))
            return Result.Failure("Wrong Password.", 403);
        string NewPasswordHashed = await Task.Run(() => _security.HashPassword(credentials.NewPassword));
        user.HashedPassword = NewPasswordHashed;
        await _db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}