namespace CAF.Core.Enums
{
    public enum CreateUserType
    {
        /// <summary>
        ///Müşteri için Kullanıcı oluşturuldu
        /// </summary>
        Created = 1,
        /// <summary>
        /// Zaten kullanıcı mevcut
        /// </summary>
        ExistUser = 2,
        /// <summary>
        /// Kullanıcı oluşturma başarısız
        /// </summary>
        Fail = 3
    }
}
