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

    public class FieldController:Controller
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public FieldController(IRepository repository, ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new FieldViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            Field item = input.EntityId > 0 ? _repository.Find<Field>(input.EntityId) : new Field();
            var model = Mapper.Map<Field, FieldViewModel>(item); 
            model._Title = WebLocalizationKeys.SITE.ToString();
            return new CustomJsonResult(model);
        }

        public JsonResult Save(FieldViewModel input)
        {
            var item = input.EntityId > 0 ? _repository.Find<Field>(input.EntityId) : new Field();
            mapItem(ref item, input);
            var crudManger = _saveEntityService.ProcessSave(item);
            var notification = crudManger.Finish();
            return new CustomJsonResult(notification);
        }

        private void mapItem(ref Field item, FieldViewModel input)
        {
            item.Name = input.Name;
            item.Abbreviation = input.Abbreviation;
            item.Description = input.Description;
            item.Size = input.Size;
            item.FieldColor = input.FieldColor;
        }
    }

    public class FieldViewModel : ViewModel
    {
        [ValidateNonEmpty]
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        [ValidateNonEmpty]
        [TextArea]
        public string Description { get; set; }
        [ValidateNonEmpty, ValidateInteger]
        public int Size { get; set; }
        public string FieldColor { get; set; }

        public string _saveUrl { get; set; }

    }
}