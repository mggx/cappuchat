using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Chat.Shared.Encryption;
using Chat.Shared.Models;

namespace Chat.Client.SignalHelpers.Helper
{
    public static class CipherHelper
    {
        private const string Phrase = "69b03b65-dbd0-4f51-a12c-c319ff133c94";

        public static SimpleMessage EncryptMessage(SimpleMessage simpleMessage)
        {
            var copy = DeepCopy(simpleMessage);
            var encryptedMessage = StringCipher.Encrypt(simpleMessage.Message, Phrase);
            copy.Message = encryptedMessage;
            return copy;
        }

        public static SimpleMessage DecryptMessage(SimpleMessage simpleMessage)
        {
            var decryptedMessage = StringCipher.Decrypt(simpleMessage.Message, Phrase);
            simpleMessage.Message = decryptedMessage;
            return simpleMessage;
        }

        private static T DeepCopy<T>(T other)
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, other);
                memoryStream.Position = 0;
                return (T) formatter.Deserialize(memoryStream);
            }
        }
    }
}
