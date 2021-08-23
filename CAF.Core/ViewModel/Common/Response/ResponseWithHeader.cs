using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Response
{
    public class ResponseWithHeader<T>
    {
        public T Data { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public ResponseWithHeader()
        {
            this.Headers = new Dictionary<string, string>();
        }
    }
}
