using System.Collections.Generic;
using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Services;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Web.Controllers;

    public class CompanyController : KYTController
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public CompanyController(IRepository repository,
            ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new CompanyViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            Company item = input.EntityId > 0 ? _repository.Find<Company>(input.EntityId) : new Company();
            var model = new CompanyViewModel{
                Company = item,
                _Title = WebLocalizationKeys.CLIENT.ToString(),
            };
            return new CustomJsonResult(model);
        }

        public JsonResult Save(CompanyViewModel input)
        {
            var item = input.EntityId > 0 ? _repository.Find<Company>(input.EntityId) : new Company();
            mapItem(ref item, input);
            var crudManger = _saveEntityService.ProcessSave(item);
            var notification = crudManger.Finish();
            return Json(notification);
        }

        private void mapItem(ref Company original, CompanyViewModel input)
        {
            original.Name = input.Company.Name;
        }
    }

    public class CompanyViewModel:ViewModel
    {
        public Company Company { get; set; }

        public IEnumerable<SelectListItem> TenantList { get; set; }
    }
}