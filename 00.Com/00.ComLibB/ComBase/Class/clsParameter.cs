using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace ComBase
{
    public class ClsParameter : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Parameters.Clear();
                Parameters = null;
            }

            base.Dispose(disposing);
        }

        public List<OracleParameter> Parameters { get; set; }

        public ClsParameter()
        {
            Parameters = new List<OracleParameter>();
        }

        public ClsParameter Add(string parameterName, object parameterValue)
        {
            Parameters.Add((new OracleParameter(parameterName, parameterValue)));
            return this;
        }
        public void Clear()
        {
            this.Parameters.Clear();
        }


    }

}
