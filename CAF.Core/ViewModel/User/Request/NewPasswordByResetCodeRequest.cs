namespace CAF.Core.ViewModel.User.Request
{
    public class NewPasswordByResetCodeRequest
    {
        /// <summary>
        /// Şifre sıfırlama kodu
        /// </summary>
        public string ResetCode { get; set; }
        /// <summary>
        /// Yeni şifre
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// Yeni şifreyi doğrula
        /// </summary>
        public string ConfirmNewPassword { get; set; }
    }
}
