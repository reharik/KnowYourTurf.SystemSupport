namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class PendingChargesListGrid : Grid<UserLoginInfo>, IEntityListGrid<UserLoginInfo>
    {
        public PendingChargesListGrid(IGridBuilder<UserLoginInfo> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<UserLoginInfo> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.User.FullNameFNF)
                .ToPerformAction(ColumnAction.DisplayItem)
                .ToolTip(WebLocalizationKeys.DISPLAY_ITEM);
            GridBuilder.DisplayFor(x => x.User.FirstOrDefaultAddress.Address1).IsSortable(false);
            GridBuilder.DisplayFor(x => x.User.FirstOrDefaultAddress.City).IsSortable(false);
            GridBuilder.DisplayFor(x => x.User.FirstOrDefaultAddress.State).IsSortable(false);
            GridBuilder.DisplayFor(x => x.User.FirstOrDefaultPhone.PhoneNumber).IsSortable(false);
            GridBuilder.DisplayFor(x => x.User.FirstOrDefaultEmail.EmailAddress).IsSortable(false);
            GridBuilder.DisplayFor(x => x.CompanyId);
            GridBuilder.ImageButtonColumn()
                .ToPerformAction(ColumnAction.Login)
                .ImageName("lion.gif").ToolTip(WebLocalizationKeys.LOG_IN); 
            return this;
        }
    }
}