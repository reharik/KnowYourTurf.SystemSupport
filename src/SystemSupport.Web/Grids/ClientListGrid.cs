namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class ClientListGrid : Grid<Company>, IEntityListGrid<Company>
    {
        public ClientListGrid(IGridBuilder<Company> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<Company> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.CreatedDate);
            return this;
        }
    }
}
