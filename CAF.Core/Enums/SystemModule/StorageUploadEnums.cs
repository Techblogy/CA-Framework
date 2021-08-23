namespace CAF.Core.Enums
{
    public enum ExistFileAction : int
    {
        /// <summary>
        /// Birşey yapma
        /// </summary>
        None = 0,
        /// <summary>
        /// Var olan dosyanın üstüne yaz
        /// </summary>
        Overwrite = 1,
        /// <summary>
        /// Aynı dosyadan varsa sonuna (1) gibi ek ekleyerek adını değiştirerek yükle
        /// </summary>
        Rename = 2
    }
}
