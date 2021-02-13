using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Validations
{
    public class FileWeightValidation: ValidationAttribute
    {
        private readonly int _maxWeightInMegaBytes;

        public FileWeightValidation(int maxWeightInMegaBytes)
        {
            _maxWeightInMegaBytes = maxWeightInMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > _maxWeightInMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"The file's weight cann't be more than {_maxWeightInMegaBytes}MB");
            }

            return ValidationResult.Success;
        }
    }
}
