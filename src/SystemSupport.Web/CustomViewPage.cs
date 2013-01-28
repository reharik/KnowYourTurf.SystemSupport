using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace SystemSupport.Web
{
    using CC.Core.Html.Menu;
    using CC.Core.Localization;

    using KnowYourTurf.Core.Enums;
    using KnowYourTurf.Core.Html.Expressions;

    public abstract class CustomWebViewPage<T> : WebViewPage<T>
    {
        public static MetaExpression MetaTag()
        {
            return new MetaExpression();
        }

        public static LinkExpression LinkTag()
        {
            return new LinkExpression();
        }

        public static LinkExpression CSS(string url)
        {
            return new LinkExpression().Href(url).AsStyleSheet();
        }

        public static ScriptReferenceExpression Script(string url)
        {
            return new ScriptReferenceExpression(url);
        }

        public static ImageExpression ImageExpression(string url)
        {
            return new ImageExpression(url);
        }

        public static StandardButtonExpression StandardButtonFor(string name, string value)
        {
            return new StandardButtonExpression(name).NonLocalizedText(value);
        }

        public static StandardButtonExpression StandardButtonFor(string name, StringToken text)
        {
            return new StandardButtonExpression(name).LocalizedText(text);
        }

        public static MenuExpression MenuItems(IList<MenuItem> items)
        {
            return new MenuExpression(items);
        }

    }
}