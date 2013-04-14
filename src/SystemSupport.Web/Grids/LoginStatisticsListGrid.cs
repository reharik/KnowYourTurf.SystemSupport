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
            GridBuilder.DisplayFor(x => x.User.FullNameLNF).Width(50);
            GridBuilder.DisplayFor(x => x.CreatedDate).DateFormat("MM/dd/yy H:mm tt").DisplayHeader(WebLocalizationKeys.LOGIN_TIME).Width(25);
            GridBuilder.DisplayFor(x => x.BrowserType).Width(25);
            GridBuilder.DisplayFor(x => x.BrowserVersion).Width(25);
            GridBuilder.DisplayFor(x => x.UserAgent);
            GridBuilder.DisplayFor(x => x.UserHostName).Width(25);
            GridBuilder.DisplayFor(x => x.UserHostAddress).Width(25);
            return this;
        }
    }
}
