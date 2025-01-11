namespace Business.Constants
{
    public static partial class Messages
    {
        public static string InvalidCode => "Geçersiz kod.";
        public static string StringLengthMustBeGreaterThanThree => "Metin uzunluğu en az üç karakter olmalıdır.";
        public static string FieldCannotBeEmpty => "{0} alanı boş bırakılamaz.";
        public static string InvalidEmailFormat => "Geçerli bir e-posta adresi giriniz.";
        public static string InvalidPhoneNumber => "Telefon numarası yalnızca rakamlardan oluşmalıdır.";
        public static string PasswordRequirements =>
            "Parola en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.";
        public static string RecordDateCannotBeFuture => "Kayıt tarihi gelecekte bir tarih olamaz.";
        public static string MaximumLengthExceeded => "{0} alanı en fazla {1} karakter uzunluğunda olabilir.";
    }
}