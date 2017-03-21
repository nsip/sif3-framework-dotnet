using System;

namespace Sif.Framework.Utils
{

    [AttributeUsage((AttributeTargets.Field | AttributeTargets.Property), AllowMultiple = false, Inherited = true)]
    public class PropertyInjectAttribute : Attribute
    {
    }

}
