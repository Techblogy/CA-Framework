using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Hangfire
{
    public class Tokens
    {
        private static Dictionary<string, string> TokenData = new Dictionary<string, string>();

        public static void Set(string threadId, string token)
        {
            if (TokenData.ContainsKey(threadId)) TokenData[threadId] = token;
            else TokenData.Add(threadId, token);
        }
        public static string Get(string threadId) => TokenData[threadId];
    }
}
