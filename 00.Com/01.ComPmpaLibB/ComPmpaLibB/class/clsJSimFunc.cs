using ComBase;
using ComDbB;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsJSimFunc
    {
        //clsPmpaFunc cPF = new clsPmpaFunc();
        
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수
        
        public bool MIR_ILLS_PROCESS(PsmhDb pDbCon, FpSpread ssiLL, string strPano, string ArgRemark, string ArgRowid)
        {
            bool rtnVal = true;

            int intRowAffected = 0;
            int i = 0;
            int nRank = 0;
            string strIllCode = string.Empty;
            string strGBILL = string.Empty;
            string strIllName = string.Empty;
            string strROWID = string.Empty;
            string strSel = string.Empty;

            string[] strRowIdTAB = new string[6];
            string[] strCodeTAB = new string[6];
            int[] nRankTAB = new int[6];
            string[] strNameTAB = new string[6];

            try
            {

                if (ArgRowid == "")
                {
                    #region INSERT INTO MIR_ILLS
                    SQL = "";
                    SQL += " INSERT INTO " + ComNum.DB_PMPA + "MIR_ILLS  (                                                  \r\n";
                    SQL += "        Pano,Bi,IpdOpd,InDate,Rank,IllCode,IllName, DEPTCODE, IPDNO, TRSNO, GBIPD, GBILL )      \r\n";
                    SQL += " VALUES (                                                                                       \r\n";
                    SQL += "        '" + strPano + "'                                               --PANO                  \r\n";
                    SQL += "       ,'" + clsPmpaType.TIT.Bi + "'                                    --BI                    \r\n";
                    SQL += "       ,'I'                                                             --IPDOPD                \r\n";
                    SQL += "       , TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')         --INDATE                \r\n";
                    SQL += "       ,'0'                                                             --Rank                  \r\n";
                    SQL += "       ,''                                                              --ILLCODE               \r\n";
                    SQL += "       ,'" + ArgRemark + "'                                             --ILLNAME               \r\n";
                    SQL += "       ,'" + clsPmpaType.TIT.DeptCode + "'                              --DEPTCODE              \r\n";
                    SQL += "       , " + clsPmpaType.TIT.Ipdno + "                                  --IPDNO                 \r\n";
                    SQL += "       , " + clsPmpaType.TIT.Trsno + "                                  --TRSNO                 \r\n";
                    SQL += "       ,'" + clsPmpaType.TIT.GbIpd + "'                                 --GBIPD                 \r\n";
                    SQL += "       ,''                                                              --GBILL                 \r\n";
                    SQL += "       )                                                                                            " ;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region UPDATE INTO MIR_ILLS
                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "MIR_ILLS                          \r\n";
                    SQL += "    SET ILLNAME = '" + ArgRemark + "'                           \r\n";
                    SQL += "  WHERE ROWID = '" + ArgRowid + "'                                  ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion
                }

                #region Data Insert, Delete, Update
                for (i = 0; i < ssiLL.ActiveSheet.RowCount; i++)
                {
                    nRank = (i + 1) * 10;
                    strIllCode = ssiLL.ActiveSheet.Cells[i, (int)clsPmpaMirSpd.enmJSimMirILL.ILLCODE].Text.Trim();
                    strGBILL = ssiLL.ActiveSheet.Cells[i, (int)clsPmpaMirSpd.enmJSimMirILL.GBILL].Text.Trim();
                    strIllName = ssiLL.ActiveSheet.Cells[i, (int)clsPmpaMirSpd.enmJSimMirILL.ILLNAME].Text.Trim();
                    strROWID = ssiLL.ActiveSheet.Cells[i, (int)clsPmpaMirSpd.enmJSimMirILL.ROWID].Text.Trim();
                    strSel = ssiLL.ActiveSheet.Cells[i, (int)clsPmpaMirSpd.enmJSimMirILL.chk01].Text;

                    if (strSel == "True")
                    {
                        if (strROWID != "")
                        {
                            if (Mir_Ill_Delete(pDbCon, strROWID) == false)
                            {
                                ComFunc.MsgBox("청구상병 수정전 자료 삭제오류", "오류");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (strROWID == "" && strIllName != "")
                        {
                            if (Mir_Ill_Insert(pDbCon, strPano, nRank, strIllCode, strIllName, strGBILL) == false)
                            {
                                ComFunc.MsgBox("청구상병 Data입력 오류", "오류");
                                return false;
                            }
                        }
                        else
	                    {
                            if (strROWID != "" && strIllCode == "" && strIllName == "")
                            {
                                if (Mir_Ill_Delete(pDbCon, strROWID) == false)
                                {
                                    ComFunc.MsgBox("청구상병 수정전 자료 삭제오류", "오류");
                                    return false;
                                }
                            }
                            else
                            {
                                if (Mir_Ill_Update(pDbCon, nRank, strIllCode, strIllName, strGBILL, strROWID) == false)
                                {
                                    ComFunc.MsgBox("청구상병 수정전 자료  UpDate오류", "오류");
                                    return false;
                                }
                                
                            }
                        }
                    }
                }
                #endregion

                #region //'*-----------( 입원마스타,퇴원마스타에 Update )------------*

                if (Set_Mir_iLLS(pDbCon, strPano, ref strCodeTAB) == false)
                {
                    ComFunc.MsgBox("청구상병 Data 조회 오류", "오류");
                    return false;
                }

                if (Mir_Ill_Ipwon_Update(pDbCon, strPano, strCodeTAB) == false)
                {
                    ComFunc.MsgBox("청구상병 IPD_TRANS UpDate오류", "오류");
                    return false;
                }

                #endregion
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        bool Set_Mir_iLLS(PsmhDb pDbCon, string ArgPano, ref string[] ArgCodeTab)
        {
            bool rtnVal = true;

            DataTable dt = null;
            int i = 0;
            int nREAD = 0;

            //'* 입원상병 6개만 관리함
            for (i = 0; i < 6; i++)
            {
                ArgCodeTab[i] = "";
            }

            try
            {
                SQL = "";
                SQL += " SELECT IllCode,IllName ";
                SQL += "   FROM " + ComNum.DB_PMPA + "MIR_ILLS                                      ";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                            ";
                SQL += "    AND DEPTCODE = '" + clsPmpaType.TIT.DeptCode + "'                       ";
                SQL += "    AND IpdOpd = 'I'                                                        ";
                SQL += "    AND InDate = TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += "    AND ( IPDNO = " + clsPmpaType.TIT.Ipdno + " OR IPDNO IS NULL)           ";
                SQL += "    AND ( TRSNO = " + clsPmpaType.TIT.Trsno + " OR TRSNO IS NULL)           ";
                if (clsPmpaType.TIT.GbIpd == "9")
                {
                    SQL += "  AND GBIPD = '9'                                                       ";
                }
                else
                {
                    SQL += "  AND (GBIPD IS NULL OR GBIPD NOT IN ('9'))                             ";
                }
                SQL += "  ORDER BY Rank,IllCode                                                     ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 5)
                {
                    nREAD = 5;
                }

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        ArgCodeTab[i] = dt.Rows[i]["IllCode"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }   
        }

        bool Mir_Ill_Ipwon_Update(PsmhDb pDbCon, string ArgPano, string[] ArgCodeTab)
        {
            bool rtnVal = true;
            int intRowAffected = 0;

            try
            {
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS                                     ";
                SQL += "    SET IllCode1 = '" + ArgCodeTab[0] + "',                                 ";
                SQL += "        IllCode2 = '" + ArgCodeTab[1] + "',                                 ";
                SQL += "        IllCode3 = '" + ArgCodeTab[2] + "',                                 ";
                SQL += "        IllCode4 = '" + ArgCodeTab[3] + "',                                 ";
                SQL += "        IllCode5 = '" + ArgCodeTab[4] + "',                                 ";
                SQL += "        IllCode6 = '" + ArgCodeTab[5] + "'                                  ";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                            ";
                SQL += "    AND InDate = TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')     ";
                SQL += "    AND ( IPDNO = " + clsPmpaType.TIT.Ipdno + " OR IPDNO IS NULL)           ";
                SQL += "    AND ( TRSNO = " + clsPmpaType.TIT.Trsno + " OR TRSNO IS NULL)           ";
                if (clsPmpaType.TIT.GbIpd == "9")
                {
                    SQL += "  AND GBIPD = '9'                                                       ";
                }
                else
                {
                    SQL += "  AND (GBIPD IS NULL OR GBIPD NOT IN ('9'))                             ";
                }
             
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        bool Mir_Ill_Delete(PsmhDb pDbCon, string ArgRowid)
        {
            bool rtnVal = true;
            int intRowAffected = 0;

            try
            {
                SQL = "";
                SQL += " DELETE " + ComNum.DB_PMPA + "MIR_ILLS      ";
                SQL += "  WHERE RowId = '" + ArgRowid + "'          ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        bool Mir_Ill_Insert(PsmhDb pDbCon, string ArgPano, int nRank, string ArgIllCode, string ArgiLLName, string ArgGbiLL)
        {
            bool rtnVal = true;
            int intRowAffected = 0;

            try
            {                
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "MIR_ILLS  (                                                  \r\n";
                SQL += "        Pano,Bi,IpdOpd,InDate,Rank,IllCode,IllName, DEPTCODE, IPDNO, TRSNO, GBIPD, GBILL )      \r\n";
                SQL += " VALUES (                                                                                       \r\n";
                SQL += "        '" + ArgPano + "'                                               --PANO                  \r\n";
                SQL += "       ,'" + clsPmpaType.TIT.Bi + "'                                    --BI                    \r\n";
                SQL += "       ,'I'                                                             --IPDOPD                \r\n";
                SQL += "       , TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')         --INDATE                \r\n";
                SQL += "       ,'" + nRank.ToString() + "'                                      --Rank                  \r\n";
                SQL += "       ,'" + ArgIllCode + "'                                            --ILLCODE               \r\n";
                SQL += "       ,'" + ArgiLLName + "'                                            --ILLNAME               \r\n";
                SQL += "       ,'" + clsPmpaType.TIT.DeptCode + "'                              --DEPTCODE              \r\n";
                SQL += "       , " + clsPmpaType.TIT.Ipdno + "                                  --IPDNO                 \r\n";
                SQL += "       , " + clsPmpaType.TIT.Trsno + "                                  --TRSNO                 \r\n";
                SQL += "       ,'" + clsPmpaType.TIT.GbIpd + "'                                 --GBIPD                 \r\n";
                SQL += "       ,'" + ArgGbiLL + "'                                              --GBILL                 \r\n";
                SQL += "       )                                                                                            ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        bool Mir_Ill_Update(PsmhDb pDbCon, int nRank, string ArgiLLCode, string ArgiLLName, string ArgGBILL, string ArgRowid)
        {

            bool rtnVal = true;
            int intRowAffected = 0;

            try
            {
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "MIR_ILLS              ";
                SQL += "    SET Rank = '" + nRank + "',                     ";
                SQL += "        IllCode = '" + ArgiLLCode + "',             ";
                SQL += "        IllName = '" + ArgiLLName + "' ,            ";
                SQL += "        GBILL = '" + ArgGBILL + "'                  ";
                SQL += "  WHERE ROWID = '" + ArgRowid + "'                  ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 심사파트에서 수가 자동발생용 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        public void eSave_SuCode(PsmhDb pDbCon, string ArgSuCode)
        {
            if (clsPmpaType.TIT.Pano == "")
            {
                ComFunc.MsgBox("환자 선택 후 작업요망.", "작업불가");
                return;
            }

            try
            {
                clsPmpaFunc cPF = new clsPmpaFunc();

                ComFunc.ReadSysDate(pDbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                if (cPF.Ins_IpdSlip_SuCode(pDbCon, ArgSuCode, clsPublic.GstrSysDate) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ArgSuCode + " 수가입력 오류!", "작업불가");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(pDbCon);

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 입원환자 수술일자 읽기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public string Read_OP_Date(PsmhDb pDbCon, string ArgPano)
        {
            string rtnVal = string.Empty;
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_MASTER     ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + ArgPano + "'              ";
                SQL += ComNum.VBLF + "    AND IPDOPD = 'I'                          ";
                SQL += ComNum.VBLF + "  ORDER By OPDATE DESC                        ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return rtnVal;
                }
                else
                {
                    rtnVal = Dt.Rows[0]["OPDATE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }

        }
    }
}
