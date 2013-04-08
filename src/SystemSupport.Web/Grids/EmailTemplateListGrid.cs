namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class EmailTemplateListGrid : Grid<EmailTemplate>, IEntityListGrid<EmailTemplate>
    {
        public EmailTemplateListGrid(IGridBuilder<EmailTemplate> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<EmailTemplate> BuildGrid()
        {
            GridBuilder.LinkColumnFor(x => x.Name)
                .ToPerformAction(ColumnAction.AddUpdateItem)
                .ToolTip(WebLocalizationKeys.EDIT_ITEM);
            GridBuilder.DisplayFor(x => x.CreatedDate);
            return this;
        }
    }
}
