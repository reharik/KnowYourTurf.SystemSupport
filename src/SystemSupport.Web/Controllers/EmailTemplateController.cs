using System.Web.Mvc;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Services;

    using KnowYourTurf.Core.Domain;

    public class EmailTemplateController:Controller
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;

        public EmailTemplateController(IRepository repository, ISaveEntityService saveEntityService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            EmailTemplate item = input.EntityId > 0 ? _repository.Find<EmailTemplate>(input.EntityId) : new EmailTemplate();
            var client = _repository.Find<Company>(input.ParentId);
            var model = new EmailTemplateViewModel
            {
                EmailTemplate = item,
                _Title = WebLocalizationKeys.EMAIL_TEMPLATE.ToString(),
                Company = client
            };
            return View(model);
        }

        [ValidateInput(false)]
        public JsonResult Save(EmailTemplateViewModel input)
        {
            var item = input.EntityId > 0 ? _repository.Find<EmailTemplate>(input.EntityId) : new EmailTemplate();
            mapItem(ref item, input);
            var crudManger = _saveEntityService.ProcessSave(item);
            var notification = crudManger.Finish();
            return Json(notification);
        }

        private void mapItem(ref EmailTemplate item, EmailTemplateViewModel input)
        {
            item.Description = input.EmailTemplate.Description;
            item.Name = input.EmailTemplate.Name;
            item.Body = input.EmailTemplate.Body;
        }
    }

    public class EmailTemplateViewModel : ViewModel
    {
        public EmailTemplate EmailTemplate { get; set; }
        public Company Company { get; set; }
    }
}