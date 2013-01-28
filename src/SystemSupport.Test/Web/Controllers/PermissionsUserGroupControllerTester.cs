using System.Web.Mvc;
using SystemSupport.Security.Interfaces;
using SystemSupport.Security.Model;
using SystemSupport.Web.Areas.Permissions.Grids;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace SystemSupport.Test.Web.Controllers
{
    public class PermissionsUserGroupControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_PermissionsUserGroupController : ControllerTester<UserGroupController, UserGroupViewModel, ActionResult>
    {
        private UserGroupViewModel _result;
        private IAuthorizationRepository _authorizationRepository;
        private UsersGroup _usersGroup;

        public when_calling_addedit_on_PermissionsUserGroupController()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new UserGroupViewModel
                        {
                            EntityId = 1,
                            Name = "/Portfolio/Silver"
                        };
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            _usersGroup = new UsersGroup();
            _authorizationRepository.Expect(x => x.GetUsersGroupById(Given.EntityId)).Return(_usersGroup);
            _result = (UserGroupViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_authrepo_for_userGroup()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IAuthorizationRepository>();
        }

        [Test]
        public void should_put_UserGroup_on_the_model()
        {
            _result.Item.ShouldEqual(_usersGroup);
        }

    }
}