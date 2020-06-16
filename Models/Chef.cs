using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ChefsNDishes.Models
{
    public class Chef
    {
        [Key]
        public int ChefId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DOB(MinAge = 0, MaxAge = 150)]
        public DateTime DOB { get; set; }

        [Range(1,5)]
        public int ChefRating { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public virtual string DataImageUrlField { get; set; }

        public List<Dish> CreatedDishes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int GetAge ()
        {
            int age = DateTime.Now.Year - DOB.Year;
            if (DateTime.Now.Month > DOB.Month)
            {
                if (DateTime.Now.Date > DOB.Date)
                {
                    age++;
                }
            }
            return age;
        }

        public class DOBAttribute : ValidationAttribute
        {
            public int MinAge { get; set; }
            public int MaxAge { get; set; }

            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;

                var val = (DateTime)value;

                if (val.AddYears(MinAge) > DateTime.Now)
                    return false;

                return (val.AddYears(MaxAge) > DateTime.Now);
            }
        }
    }
}