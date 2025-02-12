﻿// Type: FubuMVC.UI.Forms.FormLineExpression`1
// Assembly: FubuMVC.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FubuMVC.UI.Configuration;
using FubuMVC.UI.Tags;
using HtmlTags;

namespace FubuMVC.UI.Forms
{
    public class FormLineExpression<T> : ITagSource where T : class
    {
        private readonly Expression<Func<T, object>> _expression;
        private readonly ILabelAndFieldLayout _layout;
        private readonly ITagGenerator<T> _tags;
        private bool _isVisible = true;

        public FormLineExpression(ITagGenerator<T> tags, ILabelAndFieldLayout layout,
                                  Expression<Func<T, object>> expression)
        {
            _tags = tags;
            _layout = layout;
            _expression = expression;
            _layout.LabelTag = tags.LabelFor(expression);
        }

        #region ITagSource Members

        IEnumerable<HtmlTag> ITagSource.AllTags()
        {
            return !_isVisible ? new HtmlTag[0] : _layout.AllTags();
        }

        #endregion

        public FormLineExpression<T> AlterLabel(Action<HtmlTag> alter)
        {
            alter(_layout.LabelTag);
            return this;
        }

        public FormLineExpression<T> AlterBody(Action<HtmlTag> alter)
        {
            ensureBodyTag();
            alter(_layout.BodyTag);
            return this;
        }

        public FormLineExpression<T> AlterLayout(Action<ILabelAndFieldLayout> alter)
        {
            ensureBodyTag();
            alter(_layout);
            return this;
        }

        public FormLineExpression<T> AlterLayout(Action<ILabelAndFieldLayout, ElementRequest> alter)
        {
            ensureBodyTag();
            ElementRequest request = _tags.GetRequest(_expression);
            alter(_layout, request);
            return this;
        }

        public FormLineExpression<T> Label(HtmlTag tag)
        {
            _layout.LabelTag = tag;
            return this;
        }

        public FormLineExpression<T> Editable(bool condition)
        {
            _layout.BodyTag = !condition ? _tags.DisplayFor(_expression) : _tags.InputFor(_expression);
            return this;
        }

        public FormLineExpression<T> Visible(bool condition)
        {
            _isVisible = condition;
            return this;
        }

        public FormLineExpression<T> LabelId(string id)
        {
            _layout.LabelTag.Id(id);
            return this;
        }

        public FormLineExpression<T> BodyId(string id)
        {
            ensureBodyTag();
            _layout.BodyTag.Id(id);
            return this;
        }

        public override string ToString()
        {
            if (!_isVisible)
                return string.Empty;
            ensureBodyTag();
            return _layout.ToString();
        }

        private void ensureBodyTag()
        {
            if (_layout.BodyTag != null)
                return;
            _layout.BodyTag = _tags.DisplayFor(_expression);
        }
    }
}