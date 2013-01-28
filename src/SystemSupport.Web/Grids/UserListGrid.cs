using SystemSupport.Web.Controllers;

namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class UserListGrid : Grid<UserLoginInfo>, IEntityListGrid<UserLoginInfo>
    {
        public UserListGrid(IGridBuilder<UserLoginInfo> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<UserLoginInfo> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.User.FullName)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.User.FirstName);
            GridBuilder.DisplayFor(x => x.User.LastName);
            GridBuilder.DisplayFor(x => x.LoginName);
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
