using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using CC.Core;

namespace SystemSupport.Web.Config
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {

        protected override object DoGetInstance(Type serviceType, string key)
        {
            try
            {
                return key.IsEmpty()
                           ? ObjectFactory.GetInstance(serviceType)
                           : ObjectFactory.GetNamedInstance(serviceType, key);
            }
            catch(Exception e)
            {
            }
            return null;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ObjectFactory.GetAllInstances(serviceType).Cast<object>().AsEnumerable();
        }
    }
}