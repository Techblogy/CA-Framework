using CAF.Core.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class RequestLog : GuidEntitiy, ICreateEntity
    {
        public override Guid Id { get => base.Id; set => base.Id = value; }
        public string RequestUrl { get; set; }
        public string Ip { get; set; }
        public string RequestBody { get; set; }
        public object ResponseBody { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; }
    }
}
