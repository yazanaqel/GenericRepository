using GenericRepository.GenRepo;
using GenericRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.Pages
{
    public class EditModel : PageModel
    {
        private readonly IGenericRepository<Person> _genericRepository;

        public EditModel(IGenericRepository<Person> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [BindProperty]
        public Person? person { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            person = await _genericRepository.GetEntityAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (person != null)
            {
                _genericRepository.Update(person);

                try
                {
                    await _genericRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToPage("./Index");
        }
        private bool ManExists(Guid id)
        {
            return _genericRepository.GetQueryable().Any(x => x.Id == id);
        }
    }
}
