﻿using System.Collections.Generic;

namespace Code.Extensions
{
    public static class ListExtension
    {
        public static T GetAndRemoveElement<T>(this List<T> list)
        {
            var element = list[0];
            list.Remove(element);
            return element;
        }
    }
}