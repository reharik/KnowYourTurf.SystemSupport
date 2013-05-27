using System.Collections.Generic;
using System.Web.Mvc;
using SystemSupport.Web.Config;
using AutoMapper;
using CC.Core.CoreViewModelAndDTOs;
using CC.Core.CustomAttributes;
using CC.Core.DomainTools;
using CC.Core.Services;
using Castle.Components.Validator;
using KnowYourTurf.Core.Domain;

namespace SystemSupport.Web.Controllers
{
    public class SiteController:Controller
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public SiteController(IRepository repository, ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new SiteViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            Site item = input.EntityId > 0 ? _repository.Find<Site>(input.EntityId) : new Site();
            var model = Mapper.Map<Site, SiteViewModel>(item); 
            model._Title = WebLocalizationKeys.SITE.ToString();
            return new CustomJsonResult(model);
        }

        public JsonResult Save(SiteViewModel input)
        {
            var item = input.EntityId > 0 ? _repository.Find<Site>(input.EntityId) : new Site();
            mapItem(ref item, input);
            var crudManger = _saveEntityService.ProcessSave(item);
            var notification = crudManger.Finish();
            return new CustomJsonResult(notification);
        }

        private void mapItem(ref Site item, SiteViewModel input)
        {
            item.Name = input.Name;
            item.SiteOperation = input.SiteOperation;
            item.Description = input.Description;
        }
    }

    public class SiteViewModel : ViewModel
    {
        [ValidateNonEmpty]
        public string Name { get; set; }
        public string SiteOperation { get; set; }
        [ValidateNonEmpty]
        [TextArea]
        public string Description { get; set; }
        public string _saveUrl { get; set; }

    }
}