using Refactoring.LegacyService.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refactoring.JuniorApi.Models
{
    public class CandidateModel
    {
        [Required]
        [AgeValidation(ErrorMessage = "Age validation failed. Candidate must be greater than 18 years.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(1)]
        public string Firstname { get; set; }

        public string Surname { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PositionId { get; set; }

        public override string ToString()
        {
            return $"{nameof(DateOfBirth)} : {DateOfBirth}, " +
                $"{nameof(EmailAddress)} : {EmailAddress}, " +
                $"{nameof(Firstname)} : {Firstname}, " +
                $"{nameof(Surname)} : {Surname}, " +
                $"{nameof(PositionId)} : {PositionId}";
        }
    }
}
