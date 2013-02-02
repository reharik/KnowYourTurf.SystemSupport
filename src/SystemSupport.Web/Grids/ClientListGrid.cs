namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class ClientListGrid : Grid<Client>, IEntityListGrid<Client>
    {
        public ClientListGrid(IGridBuilder<Client> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<Client> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.CreatedDate);
            return this;
        }
    }
}
