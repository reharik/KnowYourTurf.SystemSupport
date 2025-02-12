﻿// Type: FubuMVC.UI.Configuration.AccessorDef
// Assembly: FubuMVC.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

using System;
using System.Linq.Expressions;
using FubuMVC.Core.Util;

namespace FubuMVC.UI.Configuration
{
    public class AccessorDef
    {
        public Accessor Accessor { get; set; }

        public Type ModelType { get; set; }

        public static AccessorDef For<T>(Expression<Func<T, object>> expression)
        {
            return new AccessorDef
                       {
                           Accessor = expression.ToAccessor(),
                           ModelType = typeof (T)
                       };
        }

        public bool Equals(AccessorDef other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            else
                return Equals(other.Accessor, Accessor) && Equals(other.ModelType, ModelType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (AccessorDef))
                return false;
            else
                return Equals((AccessorDef) obj);
        }

        public override int GetHashCode()
        {
            return (Accessor != null ? Accessor.GetHashCode() : 0)*397 ^
                   (ModelType != null ? ModelType.GetHashCode() : 0);
        }

        public bool Is<T>()
        {
            return Accessor.PropertyType.Equals(typeof (T));
        }
    }
}