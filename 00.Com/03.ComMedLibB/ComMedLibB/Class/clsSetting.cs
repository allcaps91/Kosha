using ComBase; //기본 클래스
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ComMedLibB
{
    /// <summary>
    /// 작성자 : 이상훈 
    /// 처방 로드시 사용자 환경설정 Read 및 권한 체크
    /// </summary>
    public class clsSetting
    {
        DataTable dt = null;
        string SQL = "";    //Query문
        string SqlErr = ""; //에러문 받는 변수

        public void fn_EnvSetting(string sUserId)
        {
            //fn_EnvSet_Clear();

            try
            {
                SQL = "";
                SQL += " SELECT *                               \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ENVSETTING       \r";
                SQL += "  WHERE USERID = '" + sUserId + "'      \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    clsOrdFunction.GEnvSet_Item01 = dt.Rows[0]["ITEM01"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item02 = dt.Rows[0]["ITEM02"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item03 = dt.Rows[0]["ITEM03"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item04 = dt.Rows[0]["ITEM04"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item05 = dt.Rows[0]["ITEM05"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item06 = dt.Rows[0]["ITEM06"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item07 = dt.Rows[0]["ITEM07"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item08 = dt.Rows[0]["ITEM08"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item09 = dt.Rows[0]["ITEM09"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item10 = dt.Rows[0]["ITEM10"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item11 = dt.Rows[0]["ITEM11"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item12 = dt.Rows[0]["ITEM12"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item13 = dt.Rows[0]["ITEM13"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item14 = dt.Rows[0]["ITEM14"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item15 = dt.Rows[0]["ITEM15"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item16 = dt.Rows[0]["ITEM16"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item17 = dt.Rows[0]["ITEM17"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item18 = dt.Rows[0]["ITEM18"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item19 = dt.Rows[0]["ITEM19"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item20 = dt.Rows[0]["ITEM20"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item21 = dt.Rows[0]["ITEM21"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item22 = dt.Rows[0]["ITEM22"].ToString().Trim();

                    clsOrdFunction.GEnvSet_Item51 = dt.Rows[0]["ITEM51"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item52 = dt.Rows[0]["ITEM52"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item53 = dt.Rows[0]["ITEM53"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item54 = dt.Rows[0]["ITEM54"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item55 = dt.Rows[0]["ITEM55"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item56 = dt.Rows[0]["ITEM56"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item57 = dt.Rows[0]["ITEM57"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item58 = dt.Rows[0]["ITEM58"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item59 = dt.Rows[0]["ITEM59"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item60 = dt.Rows[0]["ITEM60"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item61 = dt.Rows[0]["ITEM61"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item62 = dt.Rows[0]["ITEM62"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item63 = dt.Rows[0]["ITEM63"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item64 = dt.Rows[0]["ITEM64"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item65 = dt.Rows[0]["ITEM65"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item66 = dt.Rows[0]["ITEM66"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item67 = dt.Rows[0]["ITEM67"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item68 = dt.Rows[0]["ITEM68"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item69 = dt.Rows[0]["ITEM69"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item70 = dt.Rows[0]["ITEM70"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
        
        public void fn_EnvSet_Clear()
        {
            //clsOrdFunction.GEnvSet_Item01 = "";
            //clsOrdFunction.GEnvSet_Item02 = "";
            //clsOrdFunction.GEnvSet_Item03 = "";
            //clsOrdFunction.GEnvSet_Item04 = "";
            //clsOrdFunction.GEnvSet_Item05 = "";
            //clsOrdFunction.GEnvSet_Item06 = "";
            //clsOrdFunction.GEnvSet_Item07 = "";
            //clsOrdFunction.GEnvSet_Item08 = "";
            //clsOrdFunction.GEnvSet_Item09 = "";
            //clsOrdFunction.GEnvSet_Item10 = "";
            //clsOrdFunction.GEnvSet_Item11 = "";
            //clsOrdFunction.GEnvSet_Item12 = "";
            //clsOrdFunction.GEnvSet_Item13 = "";
            //clsOrdFunction.GEnvSet_Item14 = "";
            //clsOrdFunction.GEnvSet_Item15 = "";
            //clsOrdFunction.GEnvSet_Item16 = "";
            //clsOrdFunction.GEnvSet_Item17 = "";
            //clsOrdFunction.GEnvSet_Item18 = "";
            //clsOrdFunction.GEnvSet_Item19 = "";
            //clsOrdFunction.GEnvSet_Item20 = "";

            //clsOrdFunction.GEnvSet_Item51 = "";
            //clsOrdFunction.GEnvSet_Item52 = "";
            //clsOrdFunction.GEnvSet_Item53 = "";
            //clsOrdFunction.GEnvSet_Item54 = "";
            //clsOrdFunction.GEnvSet_Item55 = "";
            //clsOrdFunction.GEnvSet_Item56 = "";
            //clsOrdFunction.GEnvSet_Item57 = "";
            //clsOrdFunction.GEnvSet_Item58 = "";
            //clsOrdFunction.GEnvSet_Item59 = "";
            //clsOrdFunction.GEnvSet_Item60 = "";
            //clsOrdFunction.GEnvSet_Item61 = "";
            //clsOrdFunction.GEnvSet_Item62 = "";
            //clsOrdFunction.GEnvSet_Item63 = "";
            //clsOrdFunction.GEnvSet_Item64 = "";
            //clsOrdFunction.GEnvSet_Item65 = "";
            //clsOrdFunction.GEnvSet_Item66 = "";
            //clsOrdFunction.GEnvSet_Item67 = "";
            //clsOrdFunction.GEnvSet_Item68 = "";
            //clsOrdFunction.GEnvSet_Item69 = "";
            //clsOrdFunction.GEnvSet_Item70 = "";
        }
    }
}
