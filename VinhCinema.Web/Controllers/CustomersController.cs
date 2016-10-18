using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VinhCinema.Data.Infrastructure;
using VinhCinema.Entities;
using VinhCinema.Web.Infrastructure.Core;
using VinhCinema.Web.Models;

namespace VinhCinema.Web.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Customer> _customersRepository;
        public CustomersController(IEntityBaseRepository<Customer> customersRepository
            , IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork unitOfWork) : base(_errorsRepository, unitOfWork)
        {
            _customersRepository = customersRepository;
        }

        [HttpGet]
        [Route("search/{page:int=0}/{pageSize=4}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int CurrentPage = page.Value;
            int CurrentPageSize = pageSize.Value;
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Customer> customers = null;
                int totalCustomers;
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    customers = _customersRepository.FindBy(c => c.LastName.ToLower().Contains(filter)
                                    || c.IdentityCard.ToLower().Contains(filter)
                                    || c.FirstName.ToLower().Contains(filter))
                                .OrderBy(c => c.ID)
                                .Skip(CurrentPage * CurrentPageSize)
                                .Take(CurrentPageSize)
                                .ToList();
                    totalCustomers = _customersRepository.GetAll().Where(c => c.LastName.ToLower().Contains(filter)
                                            || c.IdentityCard.ToLower().Contains(filter)
                                            || c.FirstName.ToLower().Contains(filter))
                                    .Count();
                }
                else
                {
                    customers = _customersRepository.GetAll().OrderBy(c => c.ID).Skip(CurrentPage * CurrentPageSize).Take(CurrentPageSize).ToList();
                    totalCustomers = _customersRepository.GetAll().Count();
                }
                var customerVM = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers);
                PaginationSet<CustomerViewModel> pageSet = new PaginationSet<CustomerViewModel>()
                {
                    Page = CurrentPage,
                    TotalCount = totalCustomers,
                    TotalPages = (int)Math.Ceiling((decimal)totalCustomers / CurrentPageSize),
                    Items = customerVM
                };
                response = request.CreateResponse(HttpStatusCode.OK, pageSet);
                return response;
            });
        }
    }
}
