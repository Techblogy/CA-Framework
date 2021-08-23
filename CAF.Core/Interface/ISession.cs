using CAF.Core.Enums;

using System;

namespace CAF.Core.Interface
{
    public interface ISession
    {
        public long Id { get; }

        public string Type { get; }

        public string AppName { get; }

        public UserRole Role { get; }
        /// <summary>
        /// Bu token access token ile üretilme durumu
        /// </summary>
        public bool IsAccessToken { get; }
        /// <summary>
        /// Header'da oaln token değeri
        /// </summary>
        public string Token { get; }
        /// <summary>
        /// Oturum bozlı cache verilerin için oturum numarası
        /// </summary>
        public Guid LoginId { get; }
        public Wangkanai.Detection.Models.Device Device { get; }
    }
}
