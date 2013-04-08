using SystemSupport.Web.Areas.Permissions.Controllers;

namespace SystemSupport.Web.Areas.Permissions.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Services;

    public class UserPermissionListGrid : Grid<PermissionDto>, IEntityListGrid<PermissionDto>
    {
        public UserPermissionListGrid(IGridBuilder<PermissionDto> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<PermissionDto> BuildGrid()
        {
            // need the ugId and the UliId to associate the perm with
            GridBuilder.LinkColumnFor(x => x.Operation)
                .ReturnValueWithTrigger(x => x.UserGroupEntityId)
                .ReturnValueWithTrigger(x => x.UliId)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.State);
            GridBuilder.DisplayFor(x => x.Level);
            GridBuilder.DisplayFor(x => x.OnUser);
            GridBuilder.LinkColumnFor(x => x.UserGroup)
                .ReturnValueWithTrigger(x => x.UserGroupEntityId)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.GroupingColumnFor(x => x.FirstToken).GroupingColumnName("FirstToken");
            return this;
        }
    }

    public class GroupPermissionListGrid : Grid<PermissionDto>, IEntityListGrid<PermissionDto>
    {
        public GroupPermissionListGrid(IGridBuilder<PermissionDto> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<PermissionDto> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Operation)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ReturnValueWithTrigger(x => x.UserGroupEntityId)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.State);
            GridBuilder.DisplayFor(x => x.Level);
            GridBuilder.HideColumnFor(x => x.EntityId);
            GridBuilder.GroupingColumnFor(x => x.FirstToken).GroupingColumnName("FirstToken");

            return this;
        }
    }
}