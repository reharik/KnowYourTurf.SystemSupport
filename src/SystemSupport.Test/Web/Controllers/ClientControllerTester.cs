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
    public class ClientControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_clientcontroller : ControllerTester<ClientController, ViewModel, ActionResult>
    {
        private IRepository _repository;
        private CompanyViewModel _result;
        private Client _client;

        public when_calling_addedit_on_clientcontroller()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new ViewModel
                        {
                            EntityId = 1
                        };
            _client = ObjectMother.ValidClient("raif").WithEntityId(1);
            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<Client>(1)).Return(_client);
            _result = (CompanyViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_repo_for_client()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_put_client_on_the_model()
        {
            _result.Company.ShouldEqual(_client);
        }

    }
}