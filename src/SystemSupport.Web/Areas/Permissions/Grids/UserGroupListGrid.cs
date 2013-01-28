using SystemSupport.Web.Areas.Permissions.Controllers;

namespace SystemSupport.Web.Areas.Permissions.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Services;

    public class UserGroupListGrid : Grid<UserGroupDto>, IEntityListGrid<UserGroupDto>
    {
        public UserGroupListGrid(IGridBuilder<UserGroupDto> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<UserGroupDto> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM).DisplayHeader(WebLocalizationKeys.USER_GROUP_NAME);
            GridBuilder.GroupingColumnFor(x => x.FirstToken).GroupingColumnName("FirstToken");
            return this;
        }
    }
}