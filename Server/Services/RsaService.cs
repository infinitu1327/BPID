using System.Collections.Generic;
using System.Numerics;

namespace Server.Services
{
    internal class RSAService : IRSAService
    {
        private readonly Dictionary<int, RSA.RSA> _users = new Dictionary<int, RSA.RSA>();

        public (BigInteger e, BigInteger n) GetPublicKeys(int id)
        {
            if (_users.ContainsKey(id)) return _users[id].PublicKeys;

            _users.Add(id, new RSA.RSA());
            return _users[id].PublicKeys;
        }

        public string GetDecryptedText(string encryptedText, int id)
        {
            var decryptedText = _users[id].Decrypt(encryptedText);
            return decryptedText;
        }
    }
}