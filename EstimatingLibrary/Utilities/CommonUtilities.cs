﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public static class CommonUtilities
    {
        public static void ObservablyClear<T>(this ObservableCollection<T> collection)
        {
            List<T> toRemove = new List<T>();
            toRemove.AddRange(collection);
            foreach(T item in toRemove)
            {
                collection.Remove(item);
            }
        }

        public static bool Matches<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return (first.Except(second).Count() == 0)
                && (second.Except(first).Count() == 0);
        }
    }
}
