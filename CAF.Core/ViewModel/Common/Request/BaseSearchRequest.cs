using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Request
{
    public class BaseSearchRequest
    {
        /// <summary>
        /// Arame metni. En az 3 karakter gönderilmelidir
        /// </summary>
        public string SearchText { get; set; }
        /// <summary>
        /// Listenecek max kayıt sayısı. Min 3 max 20
        /// </summary>
        public int MaxRecord { get; set; }
    }
}
