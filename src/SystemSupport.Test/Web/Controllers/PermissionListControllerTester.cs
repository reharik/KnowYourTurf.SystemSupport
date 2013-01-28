using System.Collections.Generic;
using System.Web.Mvc;
using SystemSupport.Security.Interfaces;
using SystemSupport.Security.Model;
using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Grids;
using DecisionCritical.Core.CoreViewModelAndDTOs;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Services;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;

namespace SystemSupport.Test.Web.Controllers
{
    public class UserPermissionListControllerTester
    {
    }

    [TestFixture]
    public class when_calling_items_on_controller : ControllerTester<UserPermissionListController, GridItemsRequestModel, ActionResult>
    {
        private IRepository _repository;
        private UserLoginInfo _userLoginInfo;
        private IPermissionsService _permissionsService;
        private Permission _permission1;
        private IDynamicExpressionQuery _dynamicExpressionQuery;
        private Permission[] _permissions;
        private IEntityListGrid<PermissionDto> _grid;
        private GridItemsViewModel _result;
        private SpecificationExtensions.CapturingConstraint _sesCatcher;
        private Permission _permission2;

        public when_calling_items_on_controller()
            : base((c, i) => c.Items(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new GridItemsRequestModel {ParentId = 1, filters = string.Empty};
            _repository = MockFor<IRepository>();
            _userLoginInfo = ObjectMother.ValidUserLoginInfo("Raif");
            _repository.Expect(x => x.Find<UserLoginInfo>(Given.ParentId)).Return(_userLoginInfo);
            _permissionsService = MockFor<IPermissionsService>();
            _permission1 = new Permission
                               {
                                   Operation = new Operation {Name = "/Portfolio/MenuItem/fucknutz"}
                               };
            _permission2 = new Permission
                               {
                                Operation = new Operation {Name = "/Portfolio/fucknutz"}
                               };
            _permissions = new[] {_permission1,_permission2};
            _permissionsService.Expect(x => x.GetPermissionsFor(_userLoginInfo)).Return(_permissions);
            _dynamicExpressionQuery = MockFor<IDynamicExpressionQuery>();
            _grid = MockFor<IEntityListGrid<PermissionDto>>();
            var permDtos = new[] {new PermissionDto()}.AsQueryable();
            _sesCatcher = _dynamicExpressionQuery.CaptureArgumentsFor(x => x.PerformQueryWithItems(permDtos, null), y => y.Return(permDtos));
        }

        [Test]
        public void should_get_uli()
        {
            VerifyCallsTo<IRepository>();
        }

        [Test]
        public void should_return_proper_op_name()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<IEnumerable<PermissionDto>>().FirstOrDefault().Operation.ShouldEqual("fucknutz");
        }

        [Test]
        public void should_return_proper_firstToken()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<IEnumerable<PermissionDto>>().FirstOrDefault().FirstToken.ShouldEqual("Portfolio MenuItem ");
        }

        [Test]
        public void should_return_proper_op_name_for_short_op()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<IEnumerable<PermissionDto>>().LastOrDefault().Operation.ShouldEqual("fucknutz");
        }

        [Test]
        public void should_return_proper_firstToken_for_short_op()
        {
            TriggerOutputIfNotAlreadyTriggered();
            _sesCatcher.First<IEnumerable<PermissionDto>>().LastOrDefault().FirstToken.ShouldEqual("Portfolio");
        }
    }
}