using Business.Constants;
using Entities.Dtos.Register;
using FluentValidation;

namespace Business.ValidationRules;

public class SchoolValidator : AbstractValidator<SchoolRegisterDto>
{
    public SchoolValidator()
    {
        // UserRegisterDto alanları için validasyonlar
        RuleFor(u => u.FullName)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Ad Soyad"))
            .MaximumLength(100).WithMessage(Messages.MaximumLengthExceeded.Replace("{0}", "Ad Soyad").Replace("{1}", "100"));

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "E-posta"))
            .EmailAddress().WithMessage(Messages.InvalidEmailFormat);

        RuleFor(u => u.MobilePhone)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Telefon Numarası"))
            .Matches(@"^\+?\d+$").WithMessage(Messages.InvalidPhoneNumber);

        RuleFor(u => u.BirthDate)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Doğum Tarihi"))
            .LessThanOrEqualTo(DateTime.Now).WithMessage(Messages.RecordDateCannotBeFuture);

        RuleFor(u => u.Gender)
            .InclusiveBetween(0, 1).WithMessage("Cinsiyet yalnızca 0 (Erkek) veya 1 (Kadın) olabilir.");

        RuleFor(u => u.Address)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Adres"))
            .MaximumLength(250).WithMessage(Messages.MaximumLengthExceeded.Replace("{0}", "Adres").Replace("{1}", "250"));

        RuleFor(u => u.Notes)
            .MaximumLength(500).WithMessage(Messages.MaximumLengthExceeded.Replace("{0}", "Notlar").Replace("{1}", "500"));

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Parola"))
            .MinimumLength(8).WithMessage("Parola en az 8 karakter uzunluğunda olmalıdır.")
            .Matches(@"[A-Z]").WithMessage(Messages.PasswordRequirements)
            .Matches(@"[a-z]").WithMessage(Messages.PasswordRequirements)
            .Matches(@"[0-9]").WithMessage(Messages.PasswordRequirements)
            .Matches(@"[\W]").WithMessage(Messages.PasswordRequirements);

        // SchoolRegisterDto alanları için validasyonlar
        RuleFor(s => s.SchoolName)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Okul Adı"))
            .MaximumLength(150).WithMessage(Messages.MaximumLengthExceeded.Replace("{0}", "Okul Adı").Replace("{1}", "150"));

        RuleFor(s => s.SchoolAddress)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Okul Adresi"))
            .MaximumLength(250).WithMessage(Messages.MaximumLengthExceeded.Replace("{0}", "Okul Adresi").Replace("{1}", "250"));

        RuleFor(s => s.SchoolPhone)
            .NotEmpty().WithMessage(Messages.FieldCannotBeEmpty.Replace("{0}", "Okul Telefonu"))
            .Matches(@"^\+?\d+$").WithMessage(Messages.InvalidPhoneNumber);
    }
}