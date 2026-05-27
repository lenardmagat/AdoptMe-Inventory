using BCryptTool = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HashidsNet;
using inventory.ErrorHandling;

namespace inventory.CredentialSecurity;
public interface IHasher
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashPassword);
    public string CreateToken(int UserId, string role);
    public Result<List<int>> DecodeMultipleHashids(List<string> hashdis);
    public string encodeHashidsId(int id);
    public Result<int> DecodeHashid(string id);
    // public string CreateHashids(int GroupId);
    // public Result<int> DecodeHashids(string hashh);
}
public class Security : IHasher
{
    private readonly IHashids _hashids;
    private readonly string __JWTKeyString;
    private readonly string __IssuerKeyString;
    private readonly string __AudienceKeyString;
    
    public Security(IHashids hashids, string keyString, string issuer, string audience)
    {
        _hashids = hashids;
        __JWTKeyString = keyString;
        __IssuerKeyString = issuer;
        __AudienceKeyString = audience;
    }
    public string HashPassword(string password)
        => BCryptTool.HashPassword(password);
    
    public bool VerifyPassword(string password, string hashPassword)
        => BCryptTool.Verify(password, hashPassword);
    public string CreateToken(int Userid, string role)
        {
            DotNetEnv.Env.Load();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(__JWTKeyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Userid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds,
                Issuer = __IssuerKeyString,
                Audience = __AudienceKeyString
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    public Result<List<int>> DecodeMultipleHashids(List<string> hashids)
    {
        var decodedhashids = hashids.Select(id => _hashids.Decode(id)).ToList();
        if(decodedhashids.Any(id => id == null || id == null))
            return Result<List<int>>.Failure("Invalid credentials or Corrupted Id format.", 404);
        return Result<List<int>>.Success(decodedhashids.Select(id => id[0]).ToList());
    }
    public string encodeHashidsId(int id)
        => _hashids.Encode(id);
    
    public Result<int> DecodeHashid(string id)
    {
        if(!_hashids.TryDecodeSingle(id, out int decoded) || decoded == 0)
            return Result<int>.Failure("Invalid credentials or Corrupted Id format.", 404);
        return Result<int>.Success(decoded);
    }
}