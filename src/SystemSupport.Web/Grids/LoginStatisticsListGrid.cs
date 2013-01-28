namespace SystemSupport.Web.Grids
{
    using CC.Core.Html.Grid;

    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Services;

    public class LoginStatisticsListGrid : Grid<LoginStatistics>, IEntityListGrid<LoginStatistics>
    {
        public LoginStatisticsListGrid(IGridBuilder<LoginStatistics> gridBuilder)
            : base(gridBuilder)
        {
        }

        protected override Grid<LoginStatistics> BuildGrid()
        {
            GridBuilder.DisplayFor(x => x.User.FullNameLNF);
            GridBuilder.DisplayFor(x => x.CreatedDate);
            GridBuilder.DisplayFor(x => x.BrowserType);
            GridBuilder.DisplayFor(x => x.BrowserVersion);
            GridBuilder.DisplayFor(x => x.UserAgent);
            GridBuilder.DisplayFor(x => x.UserHostName);
            GridBuilder.DisplayFor(x => x.UserHostAddress);
            return this;
        }
    }
}
