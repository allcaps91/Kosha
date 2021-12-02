using ComBase;
using ComBase.Controls;
using ComDbB;
using System;
using System.Data;

namespace HC_Main
{
    public class clsHcMainQuery
    {
        public DataTable sel_HcGroupCode(PsmhDb pDbCon, string strJong, string strKey, string strName, System.Collections.Generic.List<string> lstJong = null)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strJongList = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code, Name, GbSelf, '' AS GRPCD, HANG         ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_GROUPCODE           ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                         ";
                SQL = SQL + ComNum.VBLF + "   AND GBSELECT = 'Y'                                ";
                SQL = SQL + ComNum.VBLF + "   AND HANG != 'M'                                   ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE >TRUNC(SYSDATE))  ";
                if (lstJong != null)
                {
                    if (lstJong.Count == 1)
                    {
                        SQL = SQL + ComNum.VBLF + " AND JONG ='" + lstJong + "'                 ";
                    }
                    else
                    {
                        strJongList = string.Join("','", lstJong.ToArray());
                        SQL = SQL + ComNum.VBLF + " AND JONG IN ('" + strJongList + "')         ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND (JONG = '" + strJong + "' ";
                    if (strKey != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    OR HANG ='" + strKey + "'                 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND HANG NOT IN ('I', 'H')                ";
                    }

                    SQL = SQL + ComNum.VBLF + " )                ";
                }
                
                if (strName != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + strName + "%'                 ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY Name                                       ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return null;
            }
        }

        public DataTable sel_HcGroupCode_MCode(PsmhDb pDbCon, string argName)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CODE, NAME, '' AS GBSELF                              ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_PMPA.FC_HIC_MCODE_GROUPCODE(CODE) AS GRPCD    ";
                //SQL = SQL + ComNum.VBLF + "      , KOSMOS_PMPA.FC_HIC_MCODE_GROUPCODE(HANG) AS HANG     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_MCODE                       ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                                 ";
                if (!argName.IsNullOrEmpty())
                {
                    SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + argName + "%'                         ";
                }
                //SQL = SQL + ComNum.VBLF + "   AND CODE NOT IN ('V11','V12','V13')                     ";
                SQL = SQL + ComNum.VBLF + " ORDER BY NAME                                               ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return null;
            }
        }

        public DataTable sel_HeaGroupCode(PsmhDb pDbCon, string strJong, string strKey, string strName)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code, Name, BuRate AS GBSELF                        ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_GROUPCODE           ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                         ";
                SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate >TRUNC(SYSDATE))  ";
                
                if (strName != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + strName + "%'                 ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY Name                                       ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return null;
            }
        }
    }
}
