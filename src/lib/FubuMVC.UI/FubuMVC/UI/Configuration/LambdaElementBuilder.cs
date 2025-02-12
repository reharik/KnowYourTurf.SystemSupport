﻿// Type: FubuMVC.UI.Configuration.LambdaElementBuilder
// Assembly: FubuMVC.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

using System;

namespace FubuMVC.UI.Configuration
{
    public class LambdaElementBuilder : IElementBuilder
    {
        private readonly Func<AccessorDef, TagBuilder> _builderCreator;
        private readonly Func<AccessorDef, bool> _matches;

        public LambdaElementBuilder(Func<AccessorDef, bool> matches, Func<AccessorDef, TagBuilder> builderCreator)
        {
            _matches = matches;
            _builderCreator = builderCreator;
        }

        #region IElementBuilder Members

        public TagBuilder CreateInitial(AccessorDef accessorDef)
        {
            return _matches(accessorDef) ? _builderCreator(accessorDef) : null;
        }

        #endregion
    }
}