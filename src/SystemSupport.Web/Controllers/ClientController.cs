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

    public class ClientController : KYTController
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public ClientController(IRepository repository,
            ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new ClientViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            Client item = input.EntityId > 0 ? _repository.Find<Client>(input.EntityId) : new Client();
            var model = new ClientViewModel{
                Client = item,
                _Title = WebLocalizationKeys.CLIENT.ToString(),
            };
            return new CustomJsonResult(model);
        }

        public JsonResult Save(ClientViewModel input)
        {
            var item = input.EntityId > 0 ? _repository.Find<Client>(input.EntityId) : new Client();
            mapItem(ref item, input);
            var crudManger = _saveEntityService.ProcessSave(item);
            var notification = crudManger.Finish();
            return Json(notification);
        }

        private void mapItem(ref Client original, ClientViewModel input)
        {
            original.Name = input.Client.Name;
        }
    }

    public class ClientViewModel:ViewModel
    {
        public Client Client { get; set; }

        public IEnumerable<SelectListItem> TenantList { get; set; }
    }
}