/// <summary>
/// Description     : 건진센터 공용모듈 / return 값이 있는 Query 위주 모음
/// Author          : 김민철
/// Create Date     : 2019-07-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>

namespace ComHpcLibB
{
    using ComBase;
    using ComDbB;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class clsHcQuery
    {
        public DataTable sel_HicExCode(PsmhDb pDbCon, string ArgRowid)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {


                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                return null;
            }
        }
    }
}
