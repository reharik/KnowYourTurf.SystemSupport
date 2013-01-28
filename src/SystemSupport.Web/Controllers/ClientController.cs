using System.Collections.Generic;
using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Services;

    using KnowYourTurf.Core.Domain;

    public class CompanyController:DCIController
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public CompanyController(IRepository repository,
            ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            Company item = input.EntityId > 0 ? _repository.Find<Company>(input.EntityId) : new Company();
            var model = new CompanyViewModel{
                Company = item,
                _Title = WebLocalizationKeys.CLIENT.ToString(),
            };
            return View(model);
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