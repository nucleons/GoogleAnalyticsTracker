﻿using System;

namespace GoogleAnalyticsTracker.Core.v1
{
    public static class EnumExtensions
    {
        public static bool IsNullableEnum(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}
