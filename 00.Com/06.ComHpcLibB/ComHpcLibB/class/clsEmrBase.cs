using System.Data;
using Microsoft.Win32;
using ComBase;
using ComLibB;


namespace ComHpcLibB
{
    public class clsMedOrderPublic
    {
        public static frmEmrViewMain MedOrderEmr = null;  //진료 EMR Main

        /// <summary>
        /// 신/구 EMR 옵션설정
        /// </summary>
        /// <returns></returns>
        public static string Read_EmrViewGb()
        {
            string rtnVal = "1";
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수


            string strCurdateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            if (VB.Val(strCurdateTime) < 20190905080000)
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
                string strEmrViewGb = "0";
                strEmrViewGb = reg.GetValue("EmrViewer", "0").ToString();
                reg.Close();
                reg.Dispose();

                rtnVal = strEmrViewGb;
                return rtnVal;
            }

            SQL = " ";
            SQL += " SELECT \r";
            SQL += "    * \r";
            SQL += " FROM KOSMOS_PMPA.BAS_BASCD \r";
            SQL += " WHERE GRPCDB = '프로그램PC세팅' \r";
            SQL += "    AND GRPCD = '진료EMRVIEW' \r";
            SQL += "    AND BASCD = '" + clsCompuInfo.gstrCOMIP + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon); ;

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 오류가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            rtnVal = "0";
            dt.Dispose();
            dt = null;
            return rtnVal;
        }
    }
}
