using SimpleInjector.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Sif.Framework.Utils
{

    public class PropertySelectionBehaviour<TAttribute> : IPropertySelectionBehavior where TAttribute : Attribute
    {

        public bool SelectProperty(Type serviceType, PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(TAttribute)).Any();
        }

    }

}
