using System.ComponentModel.DataAnnotations;

namespace CAF.Core.Enums
{
    public enum UserRole : int
    {
        [Display(Name = "Public")]
        Public = 0,
        [Display(Name = "Stanadart")]
        Standard = 1,
        /// <summary>
        /// Tam yetkili kullanıcı
        /// </summary>
        [Display(Name = "Sistem Yöneticisi")]
        SystemAdmin = 100
    }
}
