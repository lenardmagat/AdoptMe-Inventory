using inventory.CredentialSecurity;
using inventory.DTOs;
using inventory.Models;
using inventory.ErrorHandling;
using inventory.DataBase;
using Microsoft.EntityFrameworkCore;

namespace inventory.Services;
public interface IRegisterService
{
    Task<Result> ExecuteRegister(AccountCredentials accountCredentials, CancellationToken cancellationToken);
}
public class RegisterService(IHasher hasher, DbManager _db) : IRegisterService
{
    public  async Task<Result> ExecuteRegister(AccountCredentials accountCredentials, CancellationToken cancellationToken)
    {
        User? userIsExisting = await _db.Users.AsNoTracking()
                        .Where(u => u.Username == accountCredentials.Name)
                        .FirstOrDefaultAsync(cancellationToken);
        if(userIsExisting != null) return Result.Failure("Username already exist.", 400);
        string hashedPassword = await Task.Run(() =>hasher.HashPassword(accountCredentials.Password));
        User user = new User {
            Username = accountCredentials.Name, 
            HashedPassword = hashedPassword,
            Role = Roles.Admin,
            Status = true
            };
        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return Result.Success(201);
    }
}