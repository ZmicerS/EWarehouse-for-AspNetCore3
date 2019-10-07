using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace EWarehouse.Web.Models.Store
{
    public class UpdateBookViewModel : BookViewModel, IValidatableObject
    {
        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           var errors = base.Validate(validationContext).ToList();
            if (Id == 0)
            {
                errors.Add(new ValidationResult($"{nameof(Id)} is missing."));
            }
            return errors;
        }
    }
}
