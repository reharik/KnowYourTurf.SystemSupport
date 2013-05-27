namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class FieldListGrid : Grid<Field>, IEntityListGrid<Field>
    {
        public FieldListGrid(IGridBuilder<Field> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<Field> BuildGrid()
        {
            GridBuilder.DisplayFor(x => x.Site.Name);
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.Abbreviation);
            GridBuilder.DisplayFor(x => x.Size);
            return this;
        }
    }
}
