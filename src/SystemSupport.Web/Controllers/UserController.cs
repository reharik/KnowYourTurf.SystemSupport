using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Castle.Components.Validator;

using StructureMap;

namespace SystemSupport.Web.Controllers
{
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
                model.Client = loginInfo.CompanyId;
                model.UsersCompany = company;
            }
            return View(model);
        }

        public ActionResult Login(ViewModel input)
        {
            var repository = ObjectFactory.Container.GetInstance<IRepository>();
            var uli = repository.Find<UserLoginInfo>(input.EntityId);
            uli.ByPassToken = Guid.NewGuid();
            repository.Save(uli);
            repository.Commit();
            var notification = new Notification
                                   {
                                       Success = true,
                                       Variable = uli.ByPassToken.ToString(),
                                       EntityId = uli.User.EntityId
                                   };
            return Json(notification, JsonRequestBehavior.AllowGet);
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
                origional.AddUserLoginInfo(userLoginInfo);
            }
            mapProperties(origional, input);
            handlePassword(origional, input);

            var loginInfo = input.UserLoginInfo.EntityId == 0
                                ? origional.UserLoginInfos.FirstOrDefault()
                                : origional.UserLoginInfos.FirstOrDefault(
                                    x => x.EntityId == input.UserLoginInfo.EntityId);
            assignUserGroupAndPermissions(loginInfo);

            var crudManager = _saveEntityService.ProcessSave(origional);
            var notification = crudManager.Finish();
            return Json(notification, JsonRequestBehavior.AllowGet);
        }

        private void mapProperties(User origional, UserViewModel input)
        {
            origional.BirthDate = input.User.BirthDate;
            origional.FirstName = input.User.FirstName;
            origional.LastName = input.User.LastName;
            origional.BirthDate = input.User.BirthDate;
            if (input.DefaultEmail.IsNotEmpty())
            {
                origional.AddEmail(new Email { EmailAddress = input.DefaultEmail, IsDefault = true });
            }

            var loginInfo = origional.UserLoginInfos.FirstOrDefault(x => x.EntityId == input.UserLoginInfo.EntityId);
            loginInfo.LoginName = input.UserLoginInfo.LoginName;
            loginInfo.ClientId = input.Client > 0 ? input.Client : loginInfo.ClientId;
            loginInfo.IsActive = true;
            
        }

        private void handlePassword(User origional, UserViewModel input)
        {
            if (input.Password.IsNotEmpty())
            {
                var loginInfo = origional.UserLoginInfos.FirstOrDefault(x => x.EntityId == input.UserLoginInfo.EntityId);
                loginInfo.Salt = _securityDataService.CreateSalt();
                loginInfo.Password = _securityDataService.CreatePasswordHash(input.Password, loginInfo.Salt);
            }
        }

        private void assignUserGroupAndPermissions(UserLoginInfo loginInfo)
        {
            _authorizationRepository.AssociateUserWith(loginInfo, "/Portfolio/" + level.Name);
            AssignPermissionProfile.AssignBySubscriptionLevel(loginInfo, level);
        }

        public class DeleteUserProfileItemViewModel : ViewModel
        {
            public string ItemType { get; set; }
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
}