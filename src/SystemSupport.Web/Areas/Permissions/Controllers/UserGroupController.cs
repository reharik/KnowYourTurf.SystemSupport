using System.Web.Mvc;
using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Areas.Permissions.Grids
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Security.Interfaces;
    using CC.Security.Model;

    using KnowYourTurf.Web.Controllers;

    public class UserGroupController:KYTController
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IRepository _repository;

        public UserGroupController(IAuthorizationRepository authorizationRepository, IRepository repository)
        {
            _authorizationRepository = authorizationRepository;
            _repository = repository;
        }

        public ActionResult AddUpdate(UserGroupViewModel input)
        {
            var userGroup = input.EntityId>0?_authorizationRepository.GetUsersGroupById(input.EntityId) : new UsersGroup();
            var model = new UserGroupViewModel
                            {
                                Item = userGroup,
                                _Title = WebLocalizationKeys.USER_GROUP.ToString()
                            };
            return View(model);
        }

        public JsonResult Save(UserGroupViewModel input)
        {
            var userGroup = _authorizationRepository.GetUsersGroupById(input.EntityId);
            userGroup.Name = input.Item.Name;
            userGroup.Description = input.Item.Description;
            _authorizationRepository.UpdateUsersGroup(userGroup);
            _repository.Commit();
            return Json(new Notification{Success = true});
        }
    }

    public class UserGroupViewModel : ViewModel
    {
        public string Name { get; set; }

        public UsersGroup Item { get; set; }
    }
}