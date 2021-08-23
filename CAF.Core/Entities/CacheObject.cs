using CAF.Core.Entities.Base;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class CacheObject : StringEntitiy
    {
        public override string Id { get => base.Id; set => base.Id = value; }
        public string ClearId { get; set; }
        public string ObjectTypeFullName { get; set; }
        public object OutputResponse { get; set; }
        public DateTime CacheRegisterDate { get; set; }
    }
}
