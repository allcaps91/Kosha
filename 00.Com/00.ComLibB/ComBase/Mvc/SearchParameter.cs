using ComBase.Mvc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class SearchParameter
    {
        private Dictionary<string, object> Parameter = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            if(Parameter.ContainsKey(key))
            {
                Parameter[key] = value;
            }
            else
            {
                Parameter.Add(key, value);
            }
        }

        public void Delete(string key)
        {
            Parameter.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return Parameter.ContainsKey(key);
        }

        public object GetData(string key)
        {
            if(!Parameter.ContainsKey(key))
            {
                throw new MTSException($"{key} 항목이 없습니다.");
            }

            return Parameter[key];
        }
    }
}
