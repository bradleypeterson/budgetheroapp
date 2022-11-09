using System.Security.Cryptography;
using DesktopApplication.Contracts.Services;

namespace DesktopApplication.Services;
public class PasswordService : IPasswordService
{
    private const int _saltSize = 16; // 128 bits
    private const int _keySize = 32; // 256 bits
    private const int _iterations = 100000;
    private const char segmentDelimiter = ':';
    private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            _iterations,
            _algorithm,
            _keySize
        );
        return string.Join(
            segmentDelimiter,
            Convert.ToHexString(key),
            Convert.ToHexString(salt),
            _iterations,
            _algorithm
        );
    }
    public bool VerifyPassword(string password, string hash)
    {
        var segments = hash.Split(segmentDelimiter);
        var key = Convert.FromHexString(segments[0]);
        var salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);
        var inputSecretKey = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            algorithm,
            key.Length
        );
        return key.SequenceEqual(inputSecretKey);
    }
}
