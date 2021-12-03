using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase.Controls;
using System.ComponentModel;

namespace ComBase.Mvc
{
    public class MTSCollection<T>
    {
        public MTSCollection()
        {
            Data = new BindingList<T>();
        }

        public void Add(T t)
        {
            Data.Add(t);
            //fpSpread.AddRows();
        }
        public void Clear()
        {
            Data.Clear();
        }

        public BindingList<T> Data { get; }

    }
}
