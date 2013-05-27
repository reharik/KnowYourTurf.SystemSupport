namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class SiteListGrid : Grid<Site>, IEntityListGrid<Site>
    {
        public SiteListGrid(IGridBuilder<Site> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<Site> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.SiteOperation);
            return this;
        }
    }
}
