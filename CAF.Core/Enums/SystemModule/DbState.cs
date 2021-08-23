using System.ComponentModel.DataAnnotations;

namespace CAF.Core.Enums
{
    public enum DbState : int
    {
        [Display(Name = "Aktif")]
        Active = 0,
        [Display(Name = "Pasif")]
        Passive = 1,
        [Display(Name = "Silinmiş")]
        Deleted = 2,
        /// <summary>
        /// Durumu bu şekilde update edilen kayıtlar silinir
        /// </summary>
        [Display(Name = "Kalıcı Sil")]
        HardDelete = 3,
    }
}
