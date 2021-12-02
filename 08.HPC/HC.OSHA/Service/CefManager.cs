using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    public class CefManager
    {
        private readonly static object Lock_Object = new object();
        private static CefManager instance;
        private CkEditor ckEditor;
/*
        public static CefManager Instance
        {
            get
            {
                lock (Lock_Object)
                {
                    if(instance == null)
                    {
                        instance = new CefManager();
                    }
                    return instance;
                }
            }
        }

        public CefManager()
        {
            ckEditor = new CkEditor("index.html");
        }

        public CkEditor GetCkeditor()
        {
            return this.ckEditor;
        }
        */
    }
}
