using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Constant
{
    public class StorageFolderConstant
    {
        private const string BaseFolder = CommonConstant.ApplicationKey;

        public const string UserProfileImage = BaseFolder + "/UsersPhotos";
        /// <summary>
        /// 0=İlişki numarası, 1=Klasör tipi
        /// </summary>
        public const string AllFile = BaseFolder + "/AllFile/{0}/{1}";
    }
}
