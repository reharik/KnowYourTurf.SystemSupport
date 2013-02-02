using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CC.Core.CoreViewModelAndDTOs;
using CC.Core.Html;
using KnowYourTurf.Core.Domain;

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
            var model = Mapper.Map<Client, ClientViewModel>(item);
            model._Title = WebLocalizationKeys.CLIENT.ToString();
            model._saveUrl = UrlContext.GetUrlForAction<ClientController>(x => x.Save(null));
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
            original.Name = input.Name;
            original.ZipCode = input.ZipCode;
            original.TaxRate = input.TaxRate;
        }
    }

    public class ClientViewModel:ViewModel
    {
        public virtual string Name { get; set; }
        public virtual double TaxRate { get; set; }
        public virtual string ZipCode { get; set; }
    }
}