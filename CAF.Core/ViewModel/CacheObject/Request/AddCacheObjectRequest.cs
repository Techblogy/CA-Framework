using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.CacheObject.Request
{
    public class AddCacheObjectRequest
    {

        public AddCacheObjectRequest(string id, string clearId, object value, Type type)
        {
            Id = id;
            ClearId = clearId;
            Value = value;
            Type = type;
        }
        public string Id { get; set; }
        public string ClearId { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }
        public int ExpiryHours { get; set; } = 3;
    }
}
