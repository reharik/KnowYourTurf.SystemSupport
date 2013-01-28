using System;
using System.Web.Mvc;
using SystemSupport.Security.Interfaces;
using SystemSupport.Web.Controllers;
using SystemSupport.Web.Models;
using DecisionCritical.Core;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Domain.Tools;
using DecisionCritical.Core.Services;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;

namespace SystemSupport.Test.Web.Controllers
{
    public class UserControllerTester
    {
    }

    [TestFixture, Ignore("getting repo from objectfactory needs to inject container to test")]
    public class when_calling_Login_on_userController : ControllerTester<UserController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private User _user;
        private Notification _notification;
        private ICrudManager _crudManager;
        private ISaveEntityService _saveEntityService;
        private SpecificationExtensions.CapturingConstraint _sesCatcher;
        private Notification _result;

        public when_calling_Login_on_userController()
            : base((c, i) => c.Login(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1
                        };
            _repository = MockFor<IRepository>();
            _user = ObjectMother.ValidUser("raif").WithEntityId(3);
            _repository.Expect(x => x.Find<User>(1)).Return(_user);
            _notification = new Notification { Success = true };
            _crudManager = MockFor<ICrudManager>();
            _saveEntityService = MockFor<ISaveEntityService>();
            _crudManager.Expect(x => x.Finish()).Return(_notification);
            _sesCatcher = _saveEntityService.CaptureArgumentsFor(x => x.ProcessSave(_user), x => x.Return(_crudManager));
            _result = (Notification)((JsonResult)Output).Data;

        }


        [Test]
        public void should_lookup_user_by_entityid()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_save_guid_in_user_ByPassToken()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<User>().UserLoginInfos.FirstOrDefault().ByPassToken.ShouldNotBeNull();
        }

        [Test]
        public void should_put_bypasstoken_on_notification()
        {
            var byPassToken = _sesCatcher.First<User>().UserLoginInfos.FirstOrDefault().ByPassToken;
            _result.Variable.ShouldEqual(byPassToken);
        }

        [Test]
        public void should_put_userid_on_result_entityId()
        {
            _result.EntityId.ShouldEqual(_user.EntityId);
            
        }

    }

    [TestFixture]
    public class When_callingAddUpdate_for_existing_item : ControllerTester<UserController, UserViewModel, ActionResult>
    {
        private UserViewModel _viewModel;
        private IRepository _repository;
        private UserLoginInfo _item;
        private ISelectListItemService _selectListItemService;
        private SelectListItem[] promoSelectListItems;
        private Client _client;

        public When_callingAddUpdate_for_existing_item()
            : base((c, i) => c.AddUpdate(i))
        { }

        protected override void beforeEach()
        {
            Given = new UserViewModel
            {
                EntityId = 1
            };
            _client = ObjectMother.ValidClient("Raif").WithEntityId(2);
            _item = ObjectMother.ValidUserLoginInfo("Raif").WithEntityId(1);
           // _item.AddUserSubscription( new UserSubscription { EntityId = 666, Promotion = new Promotion { EntityId = 1 },Current = true});
            _item.ClientId = _client.EntityId;
            _repository = MockFor<IRepository>();
            _repository.Expect(z => z.Find<UserLoginInfo>(_item.EntityId))
                                        .IgnoreArguments()
                                        .Return(_item);
            _selectListItemService = MockFor<ISelectListItemService>();
            _selectListItemService.Expect(y=>y.CreateList<AddressType>(x => x.Name, x => x.EntityId, true)).IgnoreArguments().Return(new[] {new SelectListItem()});
            _selectListItemService.Expect(y => y.CreateList<PhoneType>(x => x.Name, x => x.EntityId, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            _selectListItemService.Expect(y => y.CreateList<Client>(x => x.Name, x => x.EntityId, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            promoSelectListItems = new[] { new SelectListItem { Text = "test", Value = "1" } };
            _selectListItemService.Expect(y => y.CreateList<Promotion>(x => x.Name, x => x.EntityId, true)).IgnoreArguments().Return(
                new[] {new SelectListItem{Text="test", Value = "1",Selected = true}});
            _selectListItemService.Expect(y => y.SetSelectedItemByValue(promoSelectListItems,"1")).IgnoreArguments().Return(new[] { new SelectListItem { Text = "test", Value = "1" } });

            _viewModel = (UserViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_return_viewModel()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _viewModel.ShouldNotBeNull();
        }


        [Test]
        public void should_call_Find_on_Irepository()
        {
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_put_correct_entity_on_viewModel()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _viewModel.User.EntityId.ShouldEqual(_item.User.EntityId);
        }

        [Test]
        public void should_call_selectListitemservice_for_dropdowns()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<ISelectListItemService>();
        }

        [Test]
        public void should_set_promo_select_items_to_have_users_promo_selected()
        {
            _viewModel.PromotionList.FirstOrDefault(x => x.Selected).Value.ShouldEqual(1);
        }

        [Test]
        public void should_set_clientid_on_model_to_users_clientid()
        {
            _viewModel.Client.ShouldEqual(_item.ClientId);
        }

    }

    [TestFixture]
    public class When_callingAddUpdate_for_new_item :
        ControllerTester<UserController, UserViewModel, ActionResult>
    {
        private UserViewModel _viewModel;
        private Grant _item;
        private IRepository _repository;
        private UserLoginInfo _uli;
        private ISelectListItemService _selectListItemService;
        private Client _client;

        public When_callingAddUpdate_for_new_item()
            : base((c, i) => c.AddUpdate(i))
        {
        }

        protected override void beforeEach()
        {
            Given = new UserViewModel()
            {
                EntityId=1,
                ParentId = 2
            };
            _repository = MockFor<IRepository>();
            _uli = ObjectMother.ValidUserLoginInfo("Riaf");
            _uli.EntityId = 2;
            _uli.ClientId=2;
            _repository.Expect(x => x.Find<UserLoginInfo>(Given.EntityId)).Return(_uli);
            _selectListItemService = MockFor<ISelectListItemService>();
            _selectListItemService.Expect(x => x.CreateList<AddressType>(null, null, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            _selectListItemService.Expect(x => x.CreateList<PhoneType>(null, null, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            _selectListItemService.Expect(x => x.CreateList<Client>(null, null, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            _selectListItemService.Expect(x => x.CreateList<Promotion>(null, null, true)).IgnoreArguments().Return(new[] { new SelectListItem() });
            _viewModel = (UserViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_selectListItemservice()
        {
            VerifyCallsTo<ISelectListItemService>();
        }

        [Test]
        public void should_call_repo_for_user()
        {
            VerifyCallsTo<IRepository>();
        }


        [Test]
        public void should_return_viewModel()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _viewModel.ShouldNotBeNull();
        }

        [Test]
        public void should_put_new_entity_on_viewModel_when_enityId_is_zero()
        {
            _viewModel.User.ShouldNotBeNull();
            _viewModel.User.EntityId.ShouldEqual(0);
        }
    }

    [TestFixture]
    public class when_calling_deletemultiple : ControllerTester<UserListController, BulkActionViewModel, ActionResult>
    {
        private IRepository _repository;
        private ClinicalExperience _clinicalExperience;
        private ISessionContext _sessionContext;
        private UserLoginInfo _user1;
        private UserLoginInfo _user2;
        private UserLoginInfo _user3;
        private UserLoginInfo _user4;
        private ISaveEntityService _saveEntityService;
        private ICrudManager _crudManager;
        private SpecificationExtensions.CapturingConstraint _sesCatcher;
        private IAuthorizationRepository _authorizationRepository;

        public when_calling_deletemultiple()
            : base((c, i) => c.DeleteMultiple(i))
        {

        }

        protected override void beforeEach()
        {
            _repository = MockFor<IRepository>();
            _user1 = ObjectMother.ValidUserLoginInfo("Raif");
            _repository.Expect(x => x.Find<UserLoginInfo>(1)).IgnoreArguments().Repeat.Times(3).Return(_user1);
            _repository.Expect(x => x.Delete(_user1)).IgnoreArguments().Repeat.Times(3);
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            _authorizationRepository.Expect(x => x.DetachUserFromAllGroups(_user1)).IgnoreArguments().Repeat.Times(3);
            Given = new BulkActionViewModel
                        {
                            EntityIds = new[] {1, 2, 3}
                        };

        }

        [Test]
        public void should_detatch_users_from_usergroups()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IAuthorizationRepository>();
        }

        [Test]
        public void should_get_and_delete_users()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }
    }
}