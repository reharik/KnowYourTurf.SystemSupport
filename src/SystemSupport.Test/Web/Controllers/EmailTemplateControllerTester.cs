using System.Web.Mvc;
using SystemSupport.Web.Controllers;
using DecisionCritical.Core;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Services;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace SystemSupport.Test.Web.Controllers
{
    public class EmailTemplateControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_emailTemplatecontroller : ControllerTester<EmailTemplateController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private EmailTemplateViewModel _result;
        private EmailTemplate _emailTemplate;
        private ISelectListItemService _selectListItemService;
        private Client _cllient;

        public when_calling_addedit_on_emailTemplatecontroller()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1,
                            ParentId = 2
                        };
            _emailTemplate = ObjectMother.ValidEmailTemplate("raif").WithEntityId(1);
            _emailTemplate.ClientId = 2;
            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<EmailTemplate>(1)).Return(_emailTemplate);
            _repository.Expect(x => x.Find<Client>(Given.ParentId)).Return(_cllient);
            _result = (EmailTemplateViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_repo_for_template_and_client()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }
    }

    [TestFixture]
    public class when_calling_save_on_emailtemplatecontroller : ControllerTester<EmailTemplateController, EmailTemplateViewModel, ActionResult>
    {
        private IRepository _repository;
        private EmailTemplate _promo;
        private EmailTemplateViewModel _result;
        private CrudManager _crudManager;
        private ISaveEntityService _saveEntityService;
        private SpecificationExtensions.CapturingConstraint _sesCatcher;

        public when_calling_save_on_emailtemplatecontroller()
            : base((c, i) => c.Save(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new EmailTemplateViewModel
                        {
                            EntityId = 1,
                            EmailTemplate = new EmailTemplate
                                            {
                                                EntityId = 1,
                                                Name = "fred",
                                                Description = "is dead",
                                                Body = "<p>I rock</p>" 
                                            }
                        };
            _repository = MockFor<IRepository>();
            _promo = ObjectMother.ValidEmailTemplate("raif");
            _repository.Expect(x => x.Find<EmailTemplate>(1)).Return(_promo);
            _crudManager = new CrudManager(_repository);
            _saveEntityService = MockFor<ISaveEntityService>();
            _sesCatcher = _saveEntityService.CaptureArgumentsFor(x => x.ProcessSave(_promo), y => y.Return(_crudManager));

        }


        [Test]
        public void should_call_repo_for_existing_item()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_put_new_properties_on_template()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<EmailTemplate>().Name.ShouldEqual("fred");
        }

        [Test]
        public void should_put_the_email_body_on_template()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<EmailTemplate>().Body.ShouldEqual( "<p>I rock</p>" );
        }
    }

    
}