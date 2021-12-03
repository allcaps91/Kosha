using System;
using System.ComponentModel;

namespace ComBase.Controls
{
    public static class clsBindingList
    {
        public static void AddNew<T>(this BindingList<T> list, int count)
        {
            for(int i=0; i<count; i++)
            {
                list.AddNew();
            }
        }
    }
}
