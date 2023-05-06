using System.ComponentModel.DataAnnotations;

namespace GenericRepository.Models
{
	public class Person
	{
        public Guid Id { get; set; }

        [Required(ErrorMessage ="please enter name!"), StringLength(10)]
        public string? Name { get; set; }
	}
}
