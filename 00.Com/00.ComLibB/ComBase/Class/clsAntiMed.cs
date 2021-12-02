using System;
using System.Data;


namespace ComBase
{
    public class clsAntiMed : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //2019-04-06 작업
        //string SQL;
        //long rowcounter;
        //string strRtn;
        //string strValue;
        //DataTable dt = null;
        //DataTable dt1 = null;
        //string SqlErr = ""; //에러문 받는 변수

        /// <summary>
        /// Read_AntiMed_Use_Authority(READ_제한항생제_사용권한)
        /// </summary>
        /// <param name="ArgCode1"></param>
        /// <param name="ArgCode2"></param>
        /// <returns></returns>
        public string Read_AntiMed_Use_Authority(string ArgCode1, long ArgCode2)
        {
            string SQL = "";
            //long rowcounter;
            string strRtn = "";
            string strValue = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //감염내과 김미정 허용 - 어디에서도
                if (ArgCode2 == 35104 || ArgCode2 == 38732 || ArgCode2 == 44346)
                {
                    strRtn = "OK";
                    return strRtn;
                }
                else if (ArgCode1.Trim() == "192.168.2.77")
                {
                    strRtn = "OK"; //'전산실
                    return strRtn;
                }
                else if (clsType.User.Sabun.Trim() == "29519" && DateTime.Parse(clsPublic.GstrSysDate) >= DateTime.Parse("2018-08-27") && DateTime.Parse(clsPublic.GstrSysDate) <= DateTime.Parse("2018-09-02"))
                {   
                    strRtn = "OK"; //CS : 고무성 과장
                    return strRtn;
                }
                else if (clsType.User.Sabun.Trim() == "48087")
                {
                    strRtn = "OK"; //CS : 고무성 과장
                    return strRtn;
                }
                else
                {

                    SQL = "";
                    SQL += " SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE   \r";
                    SQL += " WHERE GUBUN ='OCS_제한항생제승인관리'              \r";
                    SQL += "  AND TRIM(CODE) ='" + ArgCode1.Trim() + "'         \r";
                    SQL += "  AND (DELDATE IS NULL OR DELDATE ='')              \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        strValue = "";
                        return strValue;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strValue = "OK";
                        strRtn = "OK";

                        SQL = "";
                        SQL += " SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE   \r";
                        SQL += "  WHERE GUBUN ='OCS_제한항생제승인관리'             \r";
                        SQL += "    AND TRIM(CODE) = '" + ArgCode2 + "'             \r";
                        SQL += "    AND (DELDATE IS NULL OR DELDATE ='')            \r";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return "";
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strRtn = "OK";
                        }
                        dt1.Dispose();
                        dt1 = null;
                        return strRtn;
                    }

                    dt.Dispose();
                    dt = null;
                    return strValue;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }
    }
}
