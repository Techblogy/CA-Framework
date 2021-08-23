using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Response
{
    public class KeyValueVM<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public KeyValueVM()
        {

        }

        public KeyValueVM(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
