using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

public class KisaKenarUzunKenarValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (InputAndResultViewModel)validationContext.ObjectInstance;

        if (model.KisaKenar.HasValue && model.UzunKenar.HasValue)
        {
            if (model.KisaKenar.Value > model.UzunKenar.Value)
            {
                return new ValidationResult("Kısa kenar, uzun kenardan büyük olamaz.");
            }
        }

        return ValidationResult.Success;
    }
}
