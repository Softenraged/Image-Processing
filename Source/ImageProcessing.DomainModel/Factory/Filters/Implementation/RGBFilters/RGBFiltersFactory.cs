﻿using System;
using System.Linq;
using System.Reflection;

using ImageProcessing.Common.Attributes;
using ImageProcessing.Common.Enums;
using ImageProcessing.Common.Extensions.EnumExtensions;
using ImageProcessing.Common.Extensions.TypeExtensions;
using ImageProcessing.Common.Helpers;
using ImageProcessing.Core.Factory.RGBFilters;
using ImageProcessing.Core.Model.RGBFilters;
using ImageProcessing.RGBFilters.Binary;
using ImageProcessing.RGBFilters.Color;
using ImageProcessing.RGBFilters.Grayscale;
using ImageProcessing.RGBFilters.Inversion;

namespace ImageProcessing.Factory.Filters.RGBFilters
{
    public class RGBFiltersFactory : IRGBFiltersFactory
    {
        public IRGBFilter GetFilter(string filter)
        {
            Requires.IsNotNull(filter, nameof(filter));

            switch (filter.GetEnumValueByName<RGBFilter>())
            {
                case RGBFilter.Binary:
                    return new BinaryFilter();
                case RGBFilter.Grayscale:
                    return new GrayscaleFilter();
                case RGBFilter.Inversion:
                    return new InversionFilter();

                default: throw new NotSupportedException(filter);
            }
        }

        public IRGBFilter GetColorFilter(RGBColors color)
        {
            var filter = Assembly
                 .GetExecutingAssembly()
                 .GetTypes()
                 .Where(type => type.GetInterface(nameof(IColor)) != null && type.IsClass)
                 .Where(type => type.HasAttribute<ColorAttribute>())
                 .Where(type => type.GetAttributeValue((ColorAttribute attr) => attr.Color == color))
                 .Select(type => (IColor)Activator.CreateInstance(type))
                 .Single();

            return new ColorFilter(filter);
        }
    }
}
