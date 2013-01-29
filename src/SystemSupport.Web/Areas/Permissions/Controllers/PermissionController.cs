using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using SystemSupport.Web.Controllers;
using SystemSupport.Web.Models;

namespace SystemSupport.Web.Areas.Permissions.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Services;
    using CC.Security.Interfaces;
    using CC.Security.Model;

    using KnowYourTurf.Core;
    using KnowYourTurf.Core.Domain;

    using CC.Core;

    using KnowYourTurf.Web.Controllers;

    public class PermissionController : KYTController
    {
        private readonly IRepository _repository;
        private readonly IPermissionsService _permissionsService;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IPermissionsBuilderService _permissionsBuilderService;
        private readonly ISelectListItemService _selectListItemService;

        public PermissionController(IRepository repository,
            IPermissionsService permissionsService,
            IAuthorizationRepository authorizationRepository,
            IPermissionsBuilderService permissionsBuilderService,
            ISelectListItemService selectListItemService)
        {
            _repository = repository;
            _permissionsService = permissionsService;
            _authorizationRepository = authorizationRepository;
            _permissionsBuilderService = permissionsBuilderService;
            _selectListItemService = selectListItemService;
        }

        public ActionResult AddUpdate(PermissionViewModel input)
        {
            var permission = input.EntityId > 0 ? _permissionsService.GetPermission(input.EntityId) : new Permission();
            var model = new PermissionViewModel()
                                          {
                                              Item = permission,
                                              UserGroupId =input.ParentId,
                                              UId = input.RootId
                                          };
            if (input.EntityId <= 0)
            {
                var allOperations = _authorizationRepository.GetAllOperations();
                var operations = _selectListItemService.CreateList(allOperations, x => x.Name, x => x.EntityId, true);
                model.OperationList = operations;
            }
            return View(model);
        }

        public ActionResult Display(PermissionViewModel input)
        {
            var permission = _permissionsService.GetPermission(input.EntityId);
            var model = new PermissionViewModel()
            {
                Item = permission
            };
            return View(model);
        }

        public ActionResult DeleteMultiple(BulkActionViewModel input)
        {
            if (input.EntityIds != null && input.EntityIds.Any())
            {
                input.EntityIds.ForEachItem(x =>
                                         {
                                             var permission = _permissionsService.GetPermission(x);
                                             _authorizationRepository.RemovePermission(permission);
                                         });

                _repository.Commit();
            }
            var notification = new Notification { Success = true, Message = CoreLocalizationKeys.SUCCESSFUL_SAVE.ToString() };
            return Json(notification,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(PermissionViewModel input)
        {
            if (input.EntityId >0)
            {
                var permission = _permissionsService.GetPermission(input.EntityId);
                Operation operation = _authorizationRepository.GetOperationById(input.Item.Operation.EntityId);
                permission.Level = input.Item.Level;
                permission.Allow = input.Item.Allow;
                permission.Operation = operation;
                permission.Description = input.Item.Description;
                if(input.UserGroupId>0)
                {
                    var usersGroup = _authorizationRepository.GetUsersGroupById(input.UserGroupId);
                    permission.UsersGroup = usersGroup;
                }
                else if (input.UId > 0)
                {
                    var user = _repository.Find<User>(input.UId);
                    permission.User = user;
                }
                _permissionsBuilderService.Save(permission);
            }
            else
            {
                if (input.Item.Allow)
                {
                    if (input.UserGroupId > 0)
                    {
                        var usersGroup = _authorizationRepository.GetUsersGroupById(input.UserGroupId);
                        _permissionsBuilderService.Allow(input.Item.Operation.EntityId).For(usersGroup).OnEverything().Level
                            (input.Item.Level).Description(input.Item.Description).Save();
                    }
                    else if (input.UId > 0)
                    {
                        var user = _repository.Find<User>(input.UId);
                        _permissionsBuilderService.Allow(input.Item.Operation.EntityId).For(user).OnEverything().
                            Level(input.Item.Level).Description(input.Item.Description).Save();
                    }
                }
                else
                {
                    if (input.UserGroupId > 0)
                    {
                        var usersGroup = _authorizationRepository.GetUsersGroupById(input.UserGroupId);
                        _permissionsBuilderService.Deny(input.Item.Operation.EntityId).For(usersGroup).OnEverything().Level(
                            input.Item.Level).Description(input.Item.Description).Save();
                    }
                    else if (input.UId > 0)
                    {
                        var userLoginInfo = _repository.Find<User>(input.UId);
                        _permissionsBuilderService.Deny(input.Item.Operation.EntityId).For(userLoginInfo).OnEverything().
                            Level(input.Item.Level).Description(input.Item.Description).Save();
                    }
                }
            }
            _repository.Commit();
            var notification = new Notification { Success = true, Message = CoreLocalizationKeys.SUCCESSFUL_SAVE.ToString() };
            return Json(notification,JsonRequestBehavior.AllowGet);
        }
    }

    public class PermissionViewModel : SecurityViewModel
    {
        public Permission Item { get; set; }
        public bool Allow { get; set; }
        public int Level { get; set; }
        public IEnumerable<SelectListItem> OperationList { get; set; }
        public int UserGroupId { get; set; }
        public int UId { get; set; }
    }

    public class SecurityViewModel : ViewModel
    {
        public string Name { get; set; }
        public string ParentName { get; set; }
        public int Id { get; set; }
    }


}