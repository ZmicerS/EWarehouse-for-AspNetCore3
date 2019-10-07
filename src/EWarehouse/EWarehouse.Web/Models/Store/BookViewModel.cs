using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EWarehouse.Web.Models.Store
{
    public class BookViewModel : BookCoreVieweModel, IValidatableObject
    {
        public BookCoverViewModel imageOfCover { set; get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add(new ValidationResult($"{nameof(Name)} is missing."));
            }
            if (string.IsNullOrWhiteSpace(Author))
            {
                errors.Add(new ValidationResult($"{nameof(Author)} is missing."));
            }
            if (string.IsNullOrWhiteSpace(Isbn))
            {
                errors.Add(new ValidationResult($"{nameof(Isbn)} is missing."));
            }
            if (Price < 0.001m)
            {
                errors.Add(new ValidationResult($"{nameof(Price)} is missing."));
            }
            if (Quantity == 0)
            {
                errors.Add(new ValidationResult($"{nameof(Quantity)} is missing."));
            }
            if (LanguageId == 0)
            {
                errors.Add(new ValidationResult($"{nameof(LanguageId)} is missing."));
            }
            if (string.IsNullOrWhiteSpace(Content))
            {
                errors.Add(new ValidationResult($"{nameof(Content)} is missing."));
            }

            return errors;
        }

    }
}
