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
    public class PromotionControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_promotioncontroller : ControllerTester<PromotionController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private PromotionViewModel _result;
        private Promotion _promotion;
        private ISelectListItemService _selectListItemService;

        public when_calling_addedit_on_promotioncontroller()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1
                        };
            _promotion = ObjectMother.ValidPromotion("raif").WithEntityId(1);
            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<Promotion>(1)).Return(_promotion);
            _result = (PromotionViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_repo_for_client()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();

        }
    }

    [TestFixture]
    public class when_calling_save_on_promocontroller : ControllerTester<PromotionController, PromotionViewModel, ActionResult>
    {
        private IRepository _repository;
        private Promotion _promo;
        private PromotionViewModel _result;
        private CrudManager _crudManager;
        private ISaveEntityService _saveEntityService;
        private SpecificationExtensions.CapturingConstraint _sesCatcher;

        public when_calling_save_on_promocontroller()
            : base((c, i) => c.Save(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PromotionViewModel
                        {
                            EntityId = 1,
                            Promotion = new Promotion
                                            {
                                                EntityId = 1,
                                                Name = "fred",
                                                Description = "is dead",
                                                PercentageDiscount = 50
                                            }
                        };
            _repository = MockFor<IRepository>();
            _promo = ObjectMother.ValidPromotion("raif");
            _repository.Expect(x => x.Find<Promotion>(1)).Return(_promo);
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
        public void should_put_new_properties_on_promo()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<Promotion>().Name.ShouldEqual("fred");
        }

        [Test]
        public void should_handle_percentage_for_both_decimal_and_integer()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<Promotion>().PercentageDiscount.ShouldEqual(.5);
        }
    }
}