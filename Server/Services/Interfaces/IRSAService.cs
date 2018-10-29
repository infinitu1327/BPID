using System.Numerics;

namespace Server.Services.Interfaces
{
    public interface IRSAService
    {
        (BigInteger e, BigInteger n) GetPublicKeys(string id);
        string GetDecryptedText(string encryptedText, string id);
    }
}