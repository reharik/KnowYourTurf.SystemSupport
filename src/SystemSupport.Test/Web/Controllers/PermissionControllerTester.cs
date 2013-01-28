using System;
using System.Web.Mvc;
using SystemSupport.Security.Interfaces;
using SystemSupport.Security.Model;
using SystemSupport.Web.Areas.Permissions.Controllers;
using DecisionCritical.Core;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Services;
using DecisionCritical.UnitTests.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace SystemSupport.Test.Web.Controllers
{
    public class PermissionControllerTester
    {
    }

    [TestFixture]
    public class when_calling_addedit_on_Permissioncontroller_for_newItem : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private IRepository _repository;
        private PermissionViewModel _result;
        private Permission _client;
        private Permission _permission;
        private IPermissionsService _permissionsService;
        private ISelectListItemService _selectListItemService;
        private IAuthorizationRepository _authorizationRepository;
        private Operation[] operations;

        public when_calling_addedit_on_Permissioncontroller_for_newItem()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
                        {
                        };
            
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            operations = new[] {new Operation{Name="hello",EntityId= 1}};
            _authorizationRepository.Expect(x => x.GetAllOperations()).Return(operations);
            _selectListItemService = MockFor<ISelectListItemService>();
            _selectListItemService.Expect(x => x.CreateList(operations, y => y.Name, y => y.EntityId, true)).IgnoreArguments().Return(
                new[] {new SelectListItem()});
            _result = (PermissionViewModel)((ViewResult)Output).Model;
        }

        [Test]
        public void should_put_perm_on_the_model()
        {
            _result.Item.ShouldNotBeNull();
        }

        [Test]
        public void should_put_operations_on_model_if_new()
        {
            _result.OperationList.ShouldNotBeNull();
        }

        [Test]
        public void should_call_authRep_for_operatins_if_new()
        {
            VerifyCallsTo<IAuthorizationRepository>();
        }

        [Test]
        public void should_cal_iselectlistservice_if_new()
        {
            VerifyCallsTo<ISelectListItemService>();
        }
    }

    [TestFixture]
    public class when_calling_addedit_on_Permissioncontroller_for_existing : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private PermissionViewModel _result;
        private Permission _permission;
        private IPermissionsService _permissionsService;

        public when_calling_addedit_on_Permissioncontroller_for_existing()
            : base((c, i) => c.AddUpdate(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
                        {
                            EntityId = 1,
                        };
            _permission = new Permission
                              {
                                  EntityId = 1,
                                  Operation = new Operation {Name = "/operation"},
                                  User = ObjectMother.ValidUserLoginInfo("raif"),
                                  Level = 1
                              };
            _permissionsService = MockFor<IPermissionsService>();
            _permissionsService.Expect(x => x.GetPermission(Given.EntityId)).Return(_permission);
            _result = (PermissionViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_PermsissionService_for_permission()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IPermissionsService>();
        }

        [Test]
        public void should_put_perm_on_the_model()
        {
            _result.Item.ShouldEqual(_permission);
        }

    }

    [TestFixture]
    public class when_calling_display_on_Permissioncontroller : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private IRepository _repository;
        private PermissionViewModel _result;
        private Permission _client;
        private Permission _permission;
        private IPermissionsService _permissionsService;

        public when_calling_display_on_Permissioncontroller()
            : base((c, i) => c.Display(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
            {
                EntityId = 1
            };
            _permission = new Permission
            {
                EntityId = 1,
                Operation = new Operation { Name = "/operation" },
                User = ObjectMother.ValidUserLoginInfo("raif"),
                Level = 1
            };
            _permissionsService = MockFor<IPermissionsService>();
            _permissionsService.Expect(x => x.GetPermission(Given.EntityId)).Return(_permission);
            _result = (PermissionViewModel)((ViewResult)Output).Model;
        }


        [Test]
        public void should_call_PermsissionService_for_permission()
        {
            TriggerOutputIfNotAlreadyTriggered();
            VerifyCallsTo<IPermissionsService>();
        }

        [Test]
        public void should_put_perm_on_the_model()
        {
            _result.Item.ShouldEqual(_permission);
        }
    }

    [TestFixture]
    public class when_calling_save_on_PermissonController_for_usergroup_for_deny_for_new : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private Permission _permissionOld;
        private IPermissionsService _permissionsService;
        private IAuthorizationRepository _authorizationRepository;
        private IPermissionsBuilderService _permissionsBuilderService;
        private IForPermissionBuilder _forPermissionBuilder;
        private IOnPermissionBuilder _onPermissionBuilder;
        private UsersGroup _userGroup;
        private ILevelPermissionBuilder _levelPermissionBuilder;
        private IPermissionBuilder _permissionBuilder;
        private Permission _permissionNew;

        public when_calling_save_on_PermissonController_for_usergroup_for_deny_for_new()
            : base((c, i) => c.Save(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
                        {
                            Item = new Permission
                                       {
                                           Allow = false,
                                           Level = 10,
                                           Operation = new Operation { EntityId =1 },
                                           Description = "fu"
                                       },
                            UserGroupId = 5,
                        };
            
            _permissionNew = new Permission
                                 {
                                     Operation = new Operation { EntityId = 1 },
                                     Allow = false,
                                     Level = 10,
                                     UsersGroup = _userGroup
                                 };
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            _userGroup = new UsersGroup();
            _authorizationRepository.Stub(x => x.GetUsersGroupById(Given.UserGroupId)).Return(_userGroup);
            _permissionsBuilderService = MockFor<IPermissionsBuilderService>();
            _forPermissionBuilder = MockFor<IForPermissionBuilder>();
            _onPermissionBuilder = MockFor<IOnPermissionBuilder>();
            _levelPermissionBuilder = MockFor<ILevelPermissionBuilder>();
            _permissionBuilder = MockFor<IPermissionBuilder>();
            _permissionsBuilderService.Expect(x => x.Deny(Given.Item.Operation.EntityId)).Return(_forPermissionBuilder);
            _forPermissionBuilder.Expect(x => x.For(_userGroup)).Return(_onPermissionBuilder);
            _onPermissionBuilder.Expect(x => x.OnEverything()).Return(_levelPermissionBuilder);
            _levelPermissionBuilder.Expect(x => x.Level(10)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Description(Given.Item.Description)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Save()).Return(_permissionNew);
        }


        [Test]
        public void should_put_op_and_permissionDeny_on_perm()
        {
            VerifyCallsTo<IPermissionsBuilderService>();
        }

        [Test]
        public void should_put_usergroup_on_perm()
        {
            VerifyCallsTo<IForPermissionBuilder>();
        }

        [Test]
        public void should_put_on_everything_on_perm()
        {
            VerifyCallsTo<IOnPermissionBuilder>();
        }

        [Test]
        public void should_put_level_on_perm()
        {
            VerifyCallsTo<ILevelPermissionBuilder>();
        }

        [Test]
        public void should_call_save_on_perm()
        {
            VerifyCallsTo<IPermissionBuilder>();
        }
    }
 
    [TestFixture]
    public class when_calling_save_on_PermissonController_for_usergroup_for_allow_for_new : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private Permission _permissionOld;
        private IPermissionsService _permissionsService;
        private IAuthorizationRepository _authorizationRepository;
        private IPermissionsBuilderService _permissionsBuilderService;
        private IForPermissionBuilder _forPermissionBuilder;
        private IOnPermissionBuilder _onPermissionBuilder;
        private UsersGroup _userGroup;
        private ILevelPermissionBuilder _levelPermissionBuilder;
        private IPermissionBuilder _permissionBuilder;
        private Permission _permissionNew;

        public when_calling_save_on_PermissonController_for_usergroup_for_allow_for_new()
            : base((c, i) => c.Save(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
            {
                Item = new Permission
                {
                    Allow = true,
                    Level = 10,
                    Operation = new Operation { EntityId = 1 },
                    Description = "fu"
                },
                UserGroupId = 5
            };
          
            _permissionNew = new Permission
            {
                Operation = new Operation { EntityId = 1 },
                    Description = "fu",
                Allow = true,
                Level = 10,
                UsersGroup = _userGroup
            };
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            _userGroup = new UsersGroup();
            _authorizationRepository.Stub(x => x.GetUsersGroupById(Given.UserGroupId)).Return(_userGroup);
            _permissionsBuilderService = MockFor<IPermissionsBuilderService>();
            _forPermissionBuilder = MockFor<IForPermissionBuilder>();
            _onPermissionBuilder = MockFor<IOnPermissionBuilder>();
            _levelPermissionBuilder = MockFor<ILevelPermissionBuilder>();
            _permissionBuilder = MockFor<IPermissionBuilder>();
            _permissionsBuilderService.Expect(x => x.Allow(Given.Item.Operation.EntityId)).Return(_forPermissionBuilder);
            _forPermissionBuilder.Expect(x => x.For(_userGroup)).Return(_onPermissionBuilder);
            _onPermissionBuilder.Expect(x => x.OnEverything()).Return(_levelPermissionBuilder);
            _levelPermissionBuilder.Expect(x => x.Level(10)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Description(Given.Item.Description)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Save()).Return(_permissionNew);
        }

        [Test]
        public void should_put_op_and_permissionDeny_on_perm()
        {
            VerifyCallsTo<IPermissionsBuilderService>();
        }

        [Test]
        public void should_put_usergroup_on_perm()
        {
            VerifyCallsTo<IForPermissionBuilder>();
        }

        [Test]
        public void should_put_on_everything_on_perm()
        {
            VerifyCallsTo<IOnPermissionBuilder>();
        }

        [Test]
        public void should_put_level_on_perm()
        {
            VerifyCallsTo<ILevelPermissionBuilder>();
        }

        [Test]
        public void should_call_save_on_perm()
        {
            VerifyCallsTo<IPermissionBuilder>();
        }
    }
 
     [TestFixture]
    public class when_calling_save_on_PermissonController_for_user_for_allow_for_new : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
    {
        private Permission _permissionOld;
        private IPermissionsService _permissionsService;
        private IAuthorizationRepository _authorizationRepository;
        private IPermissionsBuilderService _permissionsBuilderService;
        private IForPermissionBuilder _forPermissionBuilder;
        private IOnPermissionBuilder _onPermissionBuilder;
        private UsersGroup _userGroup;
        private ILevelPermissionBuilder _levelPermissionBuilder;
        private IPermissionBuilder _permissionBuilder;
        private Permission _permissionNew;
         private UserLoginInfo _userLoginInfo;
         private IRepository _repository;

         public when_calling_save_on_PermissonController_for_user_for_allow_for_new()
            : base((c, i) => c.Save(i))
        {

        }

        protected override void beforeEach()
        {
            Given = new PermissionViewModel
                        {
                            Item = new Permission
                                       {
                                           Allow = true,
                                           Level = 10,
                                           Operation = new Operation { EntityId = 1 },
                                           Description = "fu"
                                       },
                            UliId = 1,
                        };
          
            _userLoginInfo = ObjectMother.ValidUserLoginInfo("raif").WithEntityId(1);
            _permissionNew = new Permission
                                 {
                                     Operation = new Operation { EntityId = 1 },
                                           Description = "fu",
                                     Allow = true,
                                     Level = 10,
                                     User = _userLoginInfo
                                 };

            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<UserLoginInfo>(1)).Return(_userLoginInfo);
            _authorizationRepository = MockFor<IAuthorizationRepository>();
            _permissionsBuilderService = MockFor<IPermissionsBuilderService>();
            _forPermissionBuilder = MockFor<IForPermissionBuilder>();
            _onPermissionBuilder = MockFor<IOnPermissionBuilder>();
            _levelPermissionBuilder = MockFor<ILevelPermissionBuilder>();
            _permissionBuilder = MockFor<IPermissionBuilder>();
            _permissionsBuilderService.Expect(x => x.Allow(Given.Item.Operation.EntityId)).Return(_forPermissionBuilder);
            _forPermissionBuilder.Expect(x => x.For(_userLoginInfo)).Return(_onPermissionBuilder);
            _onPermissionBuilder.Expect(x => x.OnEverything()).Return(_levelPermissionBuilder);
            _levelPermissionBuilder.Expect(x => x.Level(10)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Description(Given.Item.Description)).Return(_permissionBuilder);
            _permissionBuilder.Expect(x => x.Save()).Return(_permissionNew);
        }

        [Test]
        public void should_remove_old_perm()
        {
            VerifyCallsTo<IAuthorizationRepository>();
        }

        [Test]
        public void should_put_op_and_permissionDeny_on_perm()
        {
            VerifyCallsTo<IPermissionsBuilderService>();
        }

        [Test]
        public void should_put_usergroup_on_perm()
        {
            VerifyCallsTo<IForPermissionBuilder>();
        }

        [Test]
        public void should_put_on_everything_on_perm()
        {
            VerifyCallsTo<IOnPermissionBuilder>();
        }

        [Test]
        public void should_put_level_on_perm()
        {
            VerifyCallsTo<ILevelPermissionBuilder>();
        }

        [Test]
        public void should_call_save_on_perm()
        {
            VerifyCallsTo<IPermissionBuilder>();
        }
    }
     [TestFixture]
     public class when_calling_save_on_PermissonController_for_user_for_deny : ControllerTester<PermissionController, PermissionViewModel, ActionResult>
     {
         private Permission _permissionOld;
         private IPermissionsService _permissionsService;
         private IAuthorizationRepository _authorizationRepository;
         private IPermissionsBuilderService _permissionsBuilderService;
         private IForPermissionBuilder _forPermissionBuilder;
         private IOnPermissionBuilder _onPermissionBuilder;
         private UsersGroup _userGroup;
         private ILevelPermissionBuilder _levelPermissionBuilder;
         private IPermissionBuilder _permissionBuilder;
         private Permission _permissionNew;
         private IRepository _repository;
         private UserLoginInfo _userLoginInfo;

         public when_calling_save_on_PermissonController_for_user_for_deny()
             : base((c, i) => c.Save(i))
         {

         }

         protected override void beforeEach()
         {
             Given = new PermissionViewModel
             {
                 Item = new Permission
                 {
                     Allow = false,
                     Level = 10,
                     Operation = new Operation { EntityId = 1 },
                     Description = "fu"
                 },
                 UliId = 1,
             };
             
            _userLoginInfo = ObjectMother.ValidUserLoginInfo("raif").WithEntityId(1);
             _permissionNew = new Permission
             {
                 Operation = new Operation { Name = "/operation" },
                 Allow = false,
                 Level = 10,
                                     User = _userLoginInfo
                                 };

            _repository = MockFor<IRepository>();
            _repository.Expect(x => x.Find<UserLoginInfo>(1)).Return(_userLoginInfo);
             _authorizationRepository = MockFor<IAuthorizationRepository>();
             _permissionsBuilderService = MockFor<IPermissionsBuilderService>();
             _forPermissionBuilder = MockFor<IForPermissionBuilder>();
             _onPermissionBuilder = MockFor<IOnPermissionBuilder>();
             _levelPermissionBuilder = MockFor<ILevelPermissionBuilder>();
             _permissionBuilder = MockFor<IPermissionBuilder>();
             _permissionsBuilderService.Expect(x => x.Deny(Given.Item.Operation.EntityId)).Return(_forPermissionBuilder);
             _forPermissionBuilder.Expect(x => x.For(_userLoginInfo)).Return(_onPermissionBuilder);
             _onPermissionBuilder.Expect(x => x.OnEverything()).Return(_levelPermissionBuilder);
             _levelPermissionBuilder.Expect(x => x.Level(10)).Return(_permissionBuilder);
             _permissionBuilder.Expect(x => x.Description(Given.Item.Description)).Return(_permissionBuilder);
             _permissionBuilder.Expect(x => x.Save()).Return(_permissionNew);
         }

         [Test]
         public void should_remove_old_perm()
         {
             VerifyCallsTo<IAuthorizationRepository>();
         }

         [Test]
         public void should_put_op_and_permissionDeny_on_perm()
         {
             VerifyCallsTo<IPermissionsBuilderService>();
         }

         [Test]
         public void should_put_usergroup_on_perm()
         {
             VerifyCallsTo<IForPermissionBuilder>();
         }

         [Test]
         public void should_put_on_everything_on_perm()
         {
             VerifyCallsTo<IOnPermissionBuilder>();
         }

         [Test]
         public void should_put_level_on_perm()
         {
             VerifyCallsTo<ILevelPermissionBuilder>();
         }

         [Test]
         public void should_call_save_on_perm()
         {
             VerifyCallsTo<IPermissionBuilder>();
         }
     }

    }