using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class Setting : GuidEntitiy
    {
        public string Key { get; set; }
        public string GroupKey { get; set; }
        public string Value { get; set; }
    }
}
