using System;
using System.Linq;
using System.Web.Mvc;
using SystemSupport.Web.Controllers;
using DecisionCritical.Core;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Domain.Tools;
using DecisionCritical.Core.Services;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace SystemSupport.Test.Web.Controllers
{
    public class PendingChargesControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_PendingChargescontroller : ControllerTester<PendingChargesController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private UserViewModel _result;
        private User _user;

        public when_calling_addedit_on_PendingChargescontroller()
            : base((c, i) => c.Display(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1
                        };
            _user = ObjectMother.ValidUser("raif").WithEntityId(1);
            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<User>(Given.EntityId)).Return(_user);
            _result = (UserViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_repo_for_user()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_put_client_on_the_model()
        {
            _result.User.ShouldEqual(_user);
        }

    }

    [TestFixture]
    public class when_calling_void_on_contrller : ControllerTester<PendingChargesController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private User _user;
        private IAuthNetWrapper _authNetWrapper;
        private UserLoginInfo _userLoginInfo;
        private UserLoginInfo _uli;
        private UserSubscription _userSubscription;

        public when_calling_void_on_contrller()
            : base((c, i) => c.Void(i))
        {
        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1,
                            ParentId = 2
                        };
            _repository = MockFor<IRepository>();
            _uli = ObjectMother.ValidUserLoginInfo("raif").WithEntityId(1);
            _userSubscription = new UserSubscription
            {
                CreateDate = DateTime.Now,
                InProcess = true,
                TransactionId = "123",
                AuthorizationCode = "321",
                AmountOfSale = 123d
            };
            _uli.AddInProcessUserSubscription(_userSubscription);
            _repository.Expect(x => x.Find<UserLoginInfo>(1)).Return(_uli);
            _authNetWrapper = MockFor<IAuthNetWrapper>();
            _authNetWrapper.Expect(
                x => x.VoidCharge(_userSubscription.TransactionId, _uli.User.EntityId)).Return(
                    new Notification {Success = true});
        }


        [Test]
        public void should_call_repo_for_user()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_call_gatewaywrapper()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IAuthNetWrapper>();
        }
    }

    [TestFixture]
    public class when_calling_charge_on_contrller : ControllerTester<PendingChargesController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private User _user;
        private IAuthNetWrapper _authNetWrapper;
        private UserLoginInfo _uli;
        private UserSubscription _userSubscription;

        public when_calling_charge_on_contrller()
            : base((c, i) => c.Charge(i))
        {
        }

        protected override void beforeEach()
        {
            Given = new ViewModel
            {
                EntityId = 1
            };
            _repository = MockFor<IRepository>();
            _uli = ObjectMother.ValidUserLoginInfo("raif").WithEntityId(1);
            _userSubscription = new UserSubscription
                                    { 
                                        CreateDate = DateTime.Now,
                                        InProcess = true,
                                        TransactionId = "123",
                                        AuthorizationCode = "321",
                                        AmountOfSale = 123d};
            _uli.AddInProcessUserSubscription(_userSubscription);
            _repository.Expect(x => x.Find<UserLoginInfo>(1)).Return(_uli);
            _authNetWrapper = MockFor<IAuthNetWrapper>();
            _authNetWrapper.Expect(
                x =>
                x.CommitCharge(decimal.Parse(_userSubscription.AmountOfSale.ToString()), _userSubscription.TransactionId,
                               _userSubscription.AuthorizationCode, _uli.User.EntityId)).Return(new Notification { Success = true });
        }


        [Test]
        public void should_call_repo_for_user()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_call_gatewaywrapper()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IAuthNetWrapper>();
        }
    }
}