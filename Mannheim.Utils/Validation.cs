using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mannheim.Utils
{
    public static class Validation
    {
        public static (bool, ICollection<ValidationResult>) Validate(object obj)
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var success = Validator.TryValidateObject(obj, context, results, true);
            return (success, results);
        }
    }
}
