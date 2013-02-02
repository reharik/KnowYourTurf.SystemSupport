using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Castle.Components.Validator;

using StructureMap;

namespace SystemSupport.Web.Controllers
{
    using SystemSupport.Web.Config;

    using AutoMapper;

    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.CustomAttributes;
    using CC.Core.DomainTools;
    using CC.Core.Enumerations;
    using CC.Core.Html;
    using CC.Core.Localization;
    using CC.Core.Services;
    using CC.Security.Interfaces;
    using CC.Core;
    using FluentNHibernate.Conventions;

    using KnowYourTurf.Core.Config;
    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Enums;
    using KnowYourTurf.Core.Services;
    using KnowYourTurf.Web.Controllers;

    using NHibernate.Linq;

    using Status = CC.Core.Enumerations.Status;

    public class UserController : KYTController
    {
        private readonly IRepository _repository;
        private readonly ISaveEntityService _saveEntityService;
        private readonly ISecurityDataService _securityDataService;
        private readonly ISelectListItemService _selectListItemService;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly IUpdateCollectionService _updateCollectionService;

        public UserController(
            IRepository repository,
            ISaveEntityService saveEntityService,
            ISecurityDataService securityDataService,
            ISelectListItemService selectListItemService,
            IAuthorizationRepository authorizationRepository,
            IFileHandlerService fileHandlerService,
            IUpdateCollectionService updateCollectionService)
        {
            _repository = repository;
            _saveEntityService = saveEntityService;
            _securityDataService = securityDataService;
            _selectListItemService = selectListItemService;
            _authorizationRepository = authorizationRepository;
            _fileHandlerService = fileHandlerService;
            _updateCollectionService = updateCollectionService;
        }

        public ActionResult AddUpdate_Template(ViewModel input)
        {
            return View("AddUpdate", new UserViewModel());
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            var employee = _repository.Query<User>(x => x.EntityId == input.EntityId).Fetch(x => x.UserLoginInfo).FirstOrDefault();
            var availableUserRoles = _repository.FindAll<UserRole>().Select(x => new TokenInputDto { id = x.EntityId.ToString(), name = x.Name });
            var selectedUserRoles = employee.UserRoles != null
                                                    ? employee.UserRoles.Select(x => new TokenInputDto { id = x.EntityId.ToString(), name = x.Name })
                                                    : null;

            var model = Mapper.Map<User, UserViewModel>(employee);
            model.UserLoginInfoPassword = "";
            model.FileUrl = model.FileUrl.IsNotEmpty() ? model.FileUrl.AddImageSizeToName("thumb") : "";
            model._StateList = _selectListItemService.CreateList<State>();
            model._UserLoginInfoStatusList = _selectListItemService.CreateList<Status>();
            model._Title = WebLocalizationKeys.EMPLOYEE_INFORMATION.ToString();
            model._returnToList = input.EntityId > 0;
            model._saveUrl = UrlContext.GetUrlForAction<UserController>(x => x.Save(null));
            model.UserRoles = new TokenInputViewModel { _availableItems = availableUserRoles, selectedItems = selectedUserRoles };
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
                                       EntityId = user.EntityId
                                   };
            return new CustomJsonResult(notification);
        }

        public JsonResult Save(UserViewModel input)
        {
            User employee;
            if (input.EntityId > 0)
            {
                employee = _repository.Find<User>(input.EntityId);
            }
            else
            {
                employee = new User();
           // need to add a client ddl or something
                //     var clientId = _sessionContext.GetClientId();
          //      var client = _repository.Find<Client>(clientId);
          //      employee.Client = client;
            }
            employee = mapToDomain(input, employee);
            mapRolesToGroups(employee);
            handlePassword(input, employee);
            if (input.DeleteImage)
            {
                _fileHandlerService.DeleteFile(employee.FileUrl);
                employee.FileUrl = string.Empty;
            }
            if (_fileHandlerService.RequsetHasFile())
            {
                employee.FileUrl =
                    _fileHandlerService.SaveAndReturnUrlForFile(SiteConfig.Config.CustomerPhotosEmployeePath);
            }

            var crudManager = _saveEntityService.ProcessSave(employee);

            var notification = crudManager.Finish();
            return new CustomJsonResult(notification, "text/plain");
        }

        private void mapRolesToGroups(User employee)
        {
            foreach (var x in employee.UserRoles)
            {
                if (x.Name == UserType.Administrator.Key)
                {
                    _authorizationRepository.AssociateUserWith(employee, UserType.Administrator.Key);
                }
                else if (!employee.UserRoles.Any(r => r.Name == x.Name))
                {
                    _authorizationRepository.DetachUserFromGroup(employee, UserType.Administrator.Key);
                }
                if (x.Name == UserType.Employee.Key)
                {
                    _authorizationRepository.AssociateUserWith(employee, UserType.Employee.Key);
                }
                else if (!employee.UserRoles.Any(r => r.Name == x.Name))
                {
                    _authorizationRepository.DetachUserFromGroup(employee, UserType.Employee.Key);
                }
                if (x.Name == UserType.Facilities.Key)
                {
                    _authorizationRepository.AssociateUserWith(employee, UserType.Facilities.Key);
                }
                else if (!employee.UserRoles.Any(r => r.Name == x.Name))
                {
                    _authorizationRepository.DetachUserFromGroup(employee, UserType.Facilities.Key);
                }
                if (x.Name == UserType.KYTAdmin.Key)
                {
                    _authorizationRepository.AssociateUserWith(employee, UserType.KYTAdmin.Key);
                }
                else if (!employee.UserRoles.Any(r => r.Name == x.Name))
                {
                    _authorizationRepository.DetachUserFromGroup(employee, UserType.KYTAdmin.Key);
                }
                if (x.Name == UserType.MultiTenant.Key)
                {
                    _authorizationRepository.AssociateUserWith(employee, UserType.MultiTenant.Key);
                }
                else if (!employee.UserRoles.Any(r => r.Name == x.Name))
                {
                    _authorizationRepository.DetachUserFromGroup(employee, UserType.MultiTenant.Key);
                }
            }
        }

        private User mapToDomain(UserViewModel model, User employee)
        {
            employee.EmployeeId = model.EmployeeId;
            employee.Address1 = model.Address1;
            employee.Address2 = model.Address2;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.EmergencyContact = model.EmergencyContact;
            employee.EmergencyContactPhone = model.EmergencyContactPhone;
            employee.Email = model.Email;
            employee.PhoneMobile = model.PhoneMobile;
            employee.City = model.City;
            employee.State = model.State;
            employee.ZipCode = model.ZipCode;
            employee.Notes = model.Notes;
            if (employee.UserLoginInfo == null)
            {
                employee.UserLoginInfo = new UserLoginInfo();
            }
            employee.UserLoginInfo.LoginName = model.Email;
            _updateCollectionService.Update(employee.UserRoles, model.UserRoles, employee.AddUserRole, employee.RemoveUserRole);
            if (!employee.UserRoles.Any())
            {
                var emp = _repository.Query<UserRole>(x => x.Name == UserType.Employee.ToString()).FirstOrDefault();
                employee.AddUserRole(emp);
            }
            return employee;
        }

        private void handlePassword(UserViewModel input, User origional)
        {
            if (input.UserLoginInfoPassword.IsNotEmpty())
            {
                origional.UserLoginInfo.Salt = _securityDataService.CreateSalt();
                origional.UserLoginInfo.Password = _securityDataService.CreatePasswordHash(input.UserLoginInfoPassword,
                                                            origional.UserLoginInfo.Salt);
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
        public TokenInputViewModel UserRoles { get; set; }
        public string _pendingGridUrl { get; set; }
        public string _completedGridUrl { get; set; }
        public bool _returnToList { get; set; }

        public bool DeleteImage { get; set; }
        public string EmployeeId { get; set; }
        [ValidateNonEmpty]
        public string FirstName { get; set; }
        [ValidateNonEmpty]
        public string LastName { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string UserLoginInfoPassword { get; set; }
        public string PasswordConfirmation { get; set; }
        [ValidateNonEmpty]
        [ValueOf(typeof(Status))]
        public string UserLoginInfoStatus { get; set; }
        public IEnumerable<SelectListItem> _UserLoginInfoStatusList { get; set; }
        [ValidateNonEmpty]
        public string Email { get; set; }
        [ValidateNonEmpty]
        public string PhoneMobile { get; set; }
        public string FileUrl { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [ValueOf(typeof(State))]
        public string State { get; set; }
        public string ZipCode { get; set; }
        [TextArea]
        public string Notes { get; set; }
        public IEnumerable<SelectListItem> _StateList { get; set; }
        public string _saveUrl { get; set; }
    }
}