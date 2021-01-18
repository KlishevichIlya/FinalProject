﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.CustomValidationAttributes
{
    public class PermittedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] permittedExtensions;
        public PermittedExtensionsAttribute(string[] permittedExtensions)
        {
            this.permittedExtensions = permittedExtensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            if (!(file == null))
            {
                var extension = Path.GetExtension(file.FileName);
                if (!permittedExtensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Файлы такого типа запрещено добавлять. (Используйте форматы .jpg, .png, .gif, .jpeg)";
        }
    }
}