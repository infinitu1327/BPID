using System.Collections.Generic;
using System.Numerics;
using Server.Services.Interfaces;

namespace Server.Services
{
    internal class RSAService : IRSAService
    {
        private readonly Dictionary<string, RSA.RSA> _users = new Dictionary<string, RSA.RSA>();

        public (BigInteger e, BigInteger n) GetPublicKeys(string id)
        {
            if (_users.ContainsKey(id)) return _users[id].PublicKeys;

            _users.Add(id, new RSA.RSA());
            return _users[id].PublicKeys;
        }

        public string GetDecryptedText(string encryptedText, string id)
        {
            var decryptedText = _users[id].Decrypt(encryptedText);
            return decryptedText;
        }
    }
}