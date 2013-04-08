using System.Collections.Generic;

namespace SystemSupport.Web.Menus
{
    using CC.Core.Html.Menu;

    public interface IMenuConfig
    {
        IList<MenuItem> Build(bool withoutPermissions = false);
    }
}