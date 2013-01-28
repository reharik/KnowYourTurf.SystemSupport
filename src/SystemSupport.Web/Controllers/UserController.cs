using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Castle.Components.Validator;

using StructureMap;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Html;
    using CC.Core.Services;
    using CC.Security.Interfaces;

    using FluentNHibernate.Conventions;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class UserController : DCIController
    {
        private readonly IRepository _repository;

        private readonly ISaveEntityService _saveEntityService;

        private readonly ISecurityDataService _securityDataService;

        private readonly ISelectListItemService _selectListItemService;

        private readonly IAuthorizationRepository _authorizationRepository;

        public UserController(
            IRepository repository,
            ISaveEntityService saveEntityService,
            ISecurityDataService securityDataService,
            ISelectListItemService selectListItemService,
            IAuthorizationRepository authorizationRepository)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
            _securityDataService = securityDataService;
            _selectListItemService = selectListItemService;
            _authorizationRepository = authorizationRepository;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new UserViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            var companys = _selectListItemService.CreateList<Company>(x => x.Name, x => x.EntityId, true);

            User user;
            UserLoginInfo loginInfo = new UserLoginInfo();
            int subscriptionLevel = 0;
            if (input.EntityId > 0)
            {
                loginInfo = _repository.Find<UserLoginInfo>(input.EntityId);
                user = loginInfo.User;
            }
            else
            {
                user = new User();
            }

            var model = new UserViewModel
                            {
                                User = user,
                                UserLoginInfo = loginInfo,
                                CompanyList = companys,
                                _Title =
                                    input.EntityId > 0
                                        ? WebLocalizationKeys.USER.ToString()
                                        : WebLocalizationKeys.ADD_NEW + " " + WebLocalizationKeys.USER,
                            };
            if (loginInfo.CompanyId > 0)
            {
                // stupid problem with dropdownlists
                var company = _repository.Find<Company>(loginInfo.CompanyId);
                model.Company = loginInfo.CompanyId;
                model.UsersCompany = company;
            }
            return new CustomJsonResult(model);
        }

        public ActionResult Login(ViewModel input)
        {
            var repository = ObjectFactory.Container.GetInstance<IRepository>();
            var user = repository.Find<User>(input.EntityId);
            user.UserLoginInfo.ByPassToken = Guid.NewGuid();
            repository.Save(user);
            repository.Commit();
            var notification = new Notification
                                   {
                                       Success = true,
                                       Variable = user.UserLoginInfo.ByPassToken.ToString(),
                                       EntityId = user.UserLoginInfo.User.EntityId
                                   };
            return new CustomJsonResult(notification);
        }

        public JsonResult Save(UserViewModel input)
        {
            User origional;
            if (input.EntityId > 0)
            {
                origional = _repository.Find<User>(input.EntityId);
            }
            else
            {
                origional = new User();
                var userLoginInfo = new UserLoginInfo();
                origional.UserLoginInfo=userLoginInfo;
            }
            mapProperties(origional, input);
            handlePassword(origional, input);

            assignUserGroupAndPermissions(origional);

            var crudManager = _saveEntityService.ProcessSave(origional);
            var notification = crudManager.Finish();
            return new CustomJsonResult(notification);
        }

        private void mapProperties(User origional, UserViewModel input)
        {
            origional.BirthDate = input.User.BirthDate;
            origional.FirstName = input.User.FirstName;
            origional.LastName = input.User.LastName;
            origional.BirthDate = input.User.BirthDate;
                origional.Email=input.DefaultEmail;

            var loginInfo = origional.UserLoginInfo;
            loginInfo.LoginName = input.UserLoginInfo.LoginName;
            loginInfo.CompanyId = input.Company > 0 ? input.Company : loginInfo.CompanyId;
        }

        private void handlePassword(User origional, UserViewModel input)
        {
            if (input.Password.IsNotEmpty())
            {
                var loginInfo = origional.UserLoginInfo;
                loginInfo.Salt = _securityDataService.CreateSalt();
                loginInfo.Password = _securityDataService.CreatePasswordHash(input.Password, loginInfo.Salt);
            }
        }

        private void assignUserGroupAndPermissions(User user)
        {
//            _authorizationRepository.AssociateUserWith(user, "/Portfolio/" + level.Name);
//            AssignPermissionProfile.AssignBySubscriptionLevel(loginInfo, level);
        }

        public class DeleteUserProfileItemViewModel : ViewModel
        {
            public string ItemType { get; set; }
        }
    }

    public class UserViewModel : ViewModel
        {

            public User User { get; set; }
            public string Password { get; set; }
            [ValidateSameAs("Password")]
            public string PasswordConfirmation { get; set; }
            [ValidateNonEmpty]
            [ValidateEmail]
            public string DefaultEmail { get; set; }
            public IEnumerable<SelectListItem> CompanyList { get; set; }
            [ValidateNonEmpty]
            public int SubscriptionLevel { get; set; }
            [ValidateNonEmpty]
            public int Company { get; set; }
            public UserLoginInfo UserLoginInfo { get; set; }
            public bool NotApplicable { get; set; }
            public Company UsersCompany { get; set; }

            public string PermissionsUrl { get; set; }
        }
}