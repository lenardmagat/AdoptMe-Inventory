using inventory.CredentialSecurity;
using inventory.DataBase;
using inventory.DTOs;
using inventory.ErrorHandling;
using inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory.Services;
public interface ILoginAccount
{
    Task<Result<string>> LoginExecute(AccountCredentials credentials, CancellationToken cancellationToken);
}

public class LoginService(IHasher _security, DbManager _db) : ILoginAccount
{
    public async Task<Result<string>> LoginExecute(AccountCredentials credentials, CancellationToken cancellationToken)
    {
        User? user = await _db.Users.AsNoTracking().Where(u => u.Username == credentials.Name).FirstOrDefaultAsync();
        if(user == null) 
            return Result<string>.Failure("Username is not existing.", 400);
        if(!await Task.Run(() =>_security.VerifyPassword(credentials.Password, user.HashedPassword)))
            return Result<string>.Failure("Wrong password", 400);
        var token = await Task.Run(()=> _security.CreateToken(user.UserId, user.Role.ToString()));
        return Result<string>.Success(token);
    }
}