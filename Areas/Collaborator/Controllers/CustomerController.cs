using Microsoft.AspNetCore.Mvc;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;
using OnlineStore.Libraries.Filters;
using X.PagedList;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class CustomerController : Controller
    {
        private ICustomerRepository customerRepository;
        private const int NumberOfItemsPerPage = 10;

        public CustomerController(ICustomerRepository customerRepository) => 
            this.customerRepository = customerRepository;
        
        public IActionResult Index(int? page, string searchParameter)
        {
            IPagedList<OnlineStore.Models.Customer> customers = 
                customerRepository.GetAllCustomers(page, NumberOfItemsPerPage, searchParameter);
            return View(customers);
        }

        [ValidateHttpReferer]
        public IActionResult ActivateOrDeactivateTheCustomerAccount(int id)
        {
            OnlineStore.Models.Customer customer = customerRepository.GetCustomer(id);

            customer.IsTheAccountActive = !customer.IsTheAccountActive;
            customerRepository.Update(customer);

            TempData["MSG_OK"] = Message.MSG_OK_002;
            return RedirectToAction(nameof(Index));
        }
    }
}