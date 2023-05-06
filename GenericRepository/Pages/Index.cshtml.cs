using GenericRepository.GenRepo;
using GenericRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IGenericRepository<Person> _genericRepository;

		public IndexModel(IGenericRepository<Person> genericRepository)
		{
			_genericRepository = genericRepository;
		}

        public IList<Person>? people { get; set; }

        [BindProperty]
        public Person? person { get; set; }
		public string NameSort { get; set; }

		public async Task OnGetAsync(string sortOrder)
        {
			NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

			IQueryable<Person> personIQ = from s in _genericRepository.GetQueryable()
											 select s;
			switch (sortOrder)
			{
				case "name_desc":
					personIQ = personIQ.OrderBy(s => s.Name);
					break;
				default:
					personIQ = personIQ.OrderBy(s => s.Id);
					break;
			}

			people = await personIQ.ToListAsync();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (person != null) _genericRepository.Insert(person);
			await _genericRepository.SaveAsync();

			return RedirectToPage("./Index");
		}

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
			var entity = await _genericRepository.GetEntityAsync(id);

            if (entity != null)
            {
                _genericRepository.Delete(entity);
                await _genericRepository.SaveAsync();
            }

            return RedirectToPage();
        }



	}
}