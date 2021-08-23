namespace CAF.Core.Utilities
{
    public class AppSettings : IAppSettings
    {
        public string ApplicationName { get; set; }
        public string Secret { get; set; }
        public string CurrentCulture { get; set; }
        public string MailSmtp { get; set; }
        public int MailPort { get; set; }
        public string MailUser { get; set; }
        public string MailPassword { get; set; }
        public bool MailEnableSSL { get; set; }
        public string MailDisplayName { get; set; }
        public bool IsBodyHtml { get; set; }
        public string AzureBlobStorageContainerName { get; set; }
        public string AzureBlobStorageConneconString { get; set; }
        public string MongoDatabaseName { get; set; }
        public string MongoConnectionString { get; set; }
        public bool UseRequestLog { get; set; }
        /// <summary>
        /// istek loglarında belirtilen gün sayısını aşanları silinir. Mongo üzerinde 512MB kotasını aştığı için kullanılır.
        /// </summary>
        public int RequestLogExpireDay { get; set; }
        /// <summary>
        /// hata loglarında belirtilen gün sayısını aşanları silinir. Mongo üzerinde 512MB kotasını aştığı için kullanılır.
        /// </summary>
        public int ErrorLogExpireDay { get; set; }
        public string DateTimeFormat { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public int MoveToArchiveDay { get; set; }
        public long SystemUserId { get; set; }
        public string UIWebSiteUrl { get; set; }
    }
    public interface IAppSettings
    {
        public string ApplicationName { get; set; }
        public string Secret { get; set; }
        public string CurrentCulture { get; set; }
        public string MailSmtp { get; set; }
        public int MailPort { get; set; }
        public string MailUser { get; set; }
        public string MailPassword { get; set; }
        public bool MailEnableSSL { get; set; }
        public string MailDisplayName { get; set; }
        public bool IsBodyHtml { get; set; }
        public string AzureBlobStorageContainerName { get; set; }
        public string AzureBlobStorageConneconString { get; set; }
        public string MongoDatabaseName { get; set; }
        public string MongoConnectionString { get; set; }
        public bool UseRequestLog { get; set; }
        /// <summary>
        /// istek loglarında belirtilen gün sayısını aşanları silinir. Mongo üzerinde 512MB kotasını aştığı için kullanılır.
        /// </summary>
        public int RequestLogExpireDay { get; set; }
        /// <summary>
        /// hata loglarında belirtilen gün sayısını aşanları silinir. Mongo üzerinde 512MB kotasını aştığı için kullanılır.
        /// </summary>
        public int ErrorLogExpireDay { get; set; }
        public string DateTimeFormat { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public int MoveToArchiveDay { get; set; }
        public long SystemUserId { get; set; }
        public string UIWebSiteUrl { get; set; }
    }
}