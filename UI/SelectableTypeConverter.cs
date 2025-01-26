using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Audio
{
    public class SelectableTypeConverter<T> : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // Enable dropdown in the designer
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // Only allow values from the list
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Container != null)
            {
                // Return all components of type AudioPlayer
                var components = context.Container.Components
                    .OfType<T>()
                    .ToArray();
                return new StandardValuesCollection(components);
            }
            return base.GetStandardValues(context);
        }
    }
}
