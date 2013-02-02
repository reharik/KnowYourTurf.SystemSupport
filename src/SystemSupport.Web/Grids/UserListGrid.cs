using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class UserListGrid : Grid<User>, IEntityListGrid<User>
    {
        public UserListGrid(IGridBuilder<User> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<User> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.FullName)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.UserLoginInfo.LoginName);
            GridBuilder.ImageButtonColumn()
                .ToPerformAction(ColumnAction.Login)
                .ImageName("eye_of_horus-31px.png");
            return this;
        }
    }


    public class UserListGrid2 : Grid<UserListGridDto>, IEntityListGrid<UserListGridDto>
    {
        public UserListGrid2(IGridBuilder<UserListGridDto> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<UserListGridDto> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.FullName)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.FirstName);
            GridBuilder.DisplayFor(x => x.LastName);
            GridBuilder.DisplayFor(x => x.LoginName);
            GridBuilder.ImageButtonColumn()
                .ToPerformAction(ColumnAction.Login)
                .ImageName("eye_of_horus-31px.png");
            GridBuilder.HideColumnFor(x => x.LoginInfoEntityId);
            return this;
        }
    }
}
