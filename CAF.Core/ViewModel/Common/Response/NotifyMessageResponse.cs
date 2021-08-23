using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Response
{
    public class NotifyMessageResponse<Key>
    {
        public Key Id { get; set; }
        public string Message { get; set; }

        public NotifyMessageResponse(Key id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
