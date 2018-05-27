using System.Numerics;

namespace Server.Services
{
    public interface IRSAService
    {
        (BigInteger e, BigInteger n) GetPublicKeys(int id);
        string GetDecryptedText(string encryptedText, int id);
    }
}