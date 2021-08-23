using CAF.Core.Helper;

using Microsoft.AspNetCore.DataProtection;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Helper
{
    public class CipherHelper : ICipherHelper
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "L6a7hS87rc5m3yBKq8UBCgRzN5N3tNef";

        public CipherHelper(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}
