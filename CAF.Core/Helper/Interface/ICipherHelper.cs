namespace CAF.Core.Helper
{
    public interface ICipherHelper
    {
        public string Encrypt(string input);

        public string Decrypt(string cipherText);
    }
}
