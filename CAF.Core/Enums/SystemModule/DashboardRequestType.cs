using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Enums
{
    public enum DashboardRequestType : int
    {
        /// <summary>
        /// Bugün
        /// </summary>
        Today = 1,
        /// <summary>
        /// Bu Hafta
        /// </summary>
        ThisWeek = 2,
        /// <summary>
        /// Bu Ay
        /// </summary>
        ThisMonth = 3,
        /// <summary>
        /// Son 3 ay
        /// </summary>
        LastThreeMonths = 4,
        /// <summary>
        /// Son 6 ay
        /// </summary>
        LastSixMonths = 5,
        /// <summary>
        /// Bu Yıl
        /// </summary>
        ThisYear = 6,
        /// <summary>
        /// Özel
        /// </summary>
        Custom = 7
    }
}
