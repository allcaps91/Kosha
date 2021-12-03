using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmDualSign
    /// Description     : 전공의 작성 챠트 주치의 확인
    /// Author          : 이현종
    /// Create Date     : 2019-07-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(FrmDualSign.frm) >> FrmDualSign.cs 폼이름 재정의" />
    /// 
    public partial class frmDualSign : Form
    {
        EmrPatient p = clsEmrChart.ClearPatient();

        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        public frmDualSign()
        {
            InitializeComponent();
        }

        private void FrmDualSign_Load(object sender, EventArgs e)
        {
            dtpSDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-7); ;
            dtpEDATE.Value = dtpSDATE.Value.AddDays(+7);

            READ_DATA("1");
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if(sender == btnSearch)
            {
                READ_DATA("1");
            }
            else
            {
                string strGubun = VB.Right(((Control)sender).Name, 1);
                READ_DATA(strGubun);
            }
        }

        private void RdoGubun1_CheckedChanged(object sender, EventArgs e)
        {
            READ_DATA("1");
        }

        void READ_DATA(string ArgGubun)
        {
            SS1_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 0;

            string strSql = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                strSql = "SELECT A.PTNO, B.SNAME, DRNAME, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE, TO_CHAR(B.OUTDATE, 'YYYY-MM-DD') OUTDATE, B.DEPTCODE, B.DRCODE";
                strSql = strSql + ComNum.VBLF + "  FROM ";
                strSql = strSql + ComNum.VBLF + "  (";
                strSql = strSql + ComNum.VBLF + "    SELECT PTNO, CHARTDATE, EMRNO, MEDFRDATE, FORMNO, RPAD(USEID, 6, ' ') AS USEID";
                strSql = strSql + ComNum.VBLF + "    FROM ADMIN.EMRXMLMST";
                strSql = strSql + ComNum.VBLF + "   WHERE CHARTDATE >='20160410'";
                strSql = strSql + ComNum.VBLF + "     AND CHARTDATE <='" + strSysDate + "'";
                strSql = strSql + ComNum.VBLF + "  UNION ALL";
                strSql = strSql + ComNum.VBLF + "    SELECT PTNO, CHARTDATE, EMRNO, MEDFRDATE, FORMNO, RPAD(CHARTUSEID, 6, ' ') AS USEID";
                strSql = strSql + ComNum.VBLF + "    FROM ADMIN.AEMRCHARTMST";
                strSql = strSql + ComNum.VBLF + "   WHERE CHARTDATE >='20160410'";
                strSql = strSql + ComNum.VBLF + "     AND CHARTDATE <='" + strSysDate + "'";
                strSql = strSql + ComNum.VBLF + "  )A";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.IPD_NEW_MASTER B";
                strSql = strSql + ComNum.VBLF + "       ON A.PTNO = B.PANO";
                strSql = strSql + ComNum.VBLF + "      AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD')";
                strSql = strSql + ComNum.VBLF + "      AND B.INDATE >= TO_DATE('2017-06-01 00:00','YYYY-MM-DD HH24:MI')";
                strSql = strSql + ComNum.VBLF + "      AND B.OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + "      AND B.OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_DOCTOR D";
                strSql = strSql + ComNum.VBLF + "       ON D.DRCODE = B.DRCODE";
                strSql = strSql + ComNum.VBLF + "     WHERE EXISTS (";
                strSql = strSql + ComNum.VBLF + "                   SELECT 1";
                strSql = strSql + ComNum.VBLF + "                     FROM ADMIN.INSA_MST";
                //'('022101','022105','022150','022160')    --내과, 정형외과, 인턴, 일반의
                if(clsType.User.BuseCode == "044201")
                {
                    strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101','022105', '022150')";
                    strSql = strSql + ComNum.VBLF + "                      AND A.USEID = SABUN";
                    strSql = strSql + ComNum.VBLF + "            )";
                }
                else
                {
                    switch (VB.Left(clsType.User.DrCode, 2))

                    {
                        case "22"://      '정형외과면
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022105')";
                            break;
                        case "01":
                        case "02":
                        case "03":
                        case "04":
                        case "05":
                        case "07":
                        case "09":
                        case "11": //'내과계면
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101')";
                            break;
                        default:
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022150','022160')";
                            break;
                    }
                    strSql = strSql + ComNum.VBLF + "                      AND A.USEID = TO_CHAR(SABUN3)";
                    strSql = strSql + ComNum.VBLF + "            )";
                    strSql = strSql + ComNum.VBLF + "     AND B.DRCODE = '" + clsType.User.DrCode + "'";
                }



                strSql = strSql + ComNum.VBLF + "     AND A.FORMNO NOT IN (963, 1232)";

                if(rdoGubun1.Checked)
                {
                    strSql = strSql + ComNum.VBLF + "     AND NOT EXISTS";
                }
                else if(rdoGubun2.Checked)
                {
                    strSql = strSql + ComNum.VBLF + "     AND EXISTS";
                }
                
                strSql = strSql + ComNum.VBLF + "   ( SELECT * FROM ADMIN.EMRXML_DUALSIGN SUB";
                strSql = strSql + ComNum.VBLF + "       WHERE SUB.EMRNO = A.EMRNO)";

                strSql = strSql + ComNum.VBLF + " GROUP BY A.PTNO, TO_CHAR(B.INDATE,'YYYY-MM-DD'), B.OUTDATE, B.DEPTCODE, B.DRCODE, D.DRNAME, B.SNAME";

                switch(ArgGubun)
                {
                    case "1":
                        strSql = strSql + ComNum.VBLF + "     ORDER BY A.PTNO";
                        break;
                    case "2":
                        strSql = strSql + ComNum.VBLF + "     ORDER BY B.SNAME";
                        break;
                    case "3":
                        strSql = strSql + ComNum.VBLF + "     ORDER BY TO_CHAR(B.INDATE,'YYYY-MM-DD'), B.SNAME";
                        break;
                    case "4":
                        strSql = strSql + ComNum.VBLF + "     ORDER BY B.OUTDATE, B.SNAME";
                        break;
                    default:
                        //strSql = strSql + ComNum.VBLF + "     ORDER BY A.INDATE ";
                        break;
                }


                SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SS1_Sheet1.RowCount = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnSaveCerti_Click(object sender, EventArgs e)
        {
            string strOK = string.Empty;
            for(int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                if(SS1_Sheet1.Cells[i, 0].Text.Trim() == "True")
                {
                    strOK = "OK";
                    break;
                }
            }

            if(strOK == "OK")
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Trim() == "True")
                    {
                        SS1_CellDoubleClick(i);
                        if(SS2_Sheet1.RowCount > 0)
                        {
                            for(int j = 0; j < SS2_Sheet1.RowCount; j++)
                            {
                                SS2_Sheet1.Cells[j, 0].Text = "True";
                            }
                            Certify_Form();
                        }
                    }
                }

                btnSearch.PerformClick();
                return;
            }

            Certify_Form();
            btnSearch.PerformClick();
        }

        void Certify_Form()
        {
            string SQL = string.Empty;
            int RowAffected = 0;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < SS2_Sheet1.RowCount; i++)
                {
                    if(SS2_Sheet1.Cells[i, 0].Text.Trim() == "True")
                    {
                        string strEmrNo    = SS2_Sheet1.Cells[i, 12].Text.Trim();

                        SQL = " INSERT INTO ADMIN.EMRXML_DUALSIGN("                                              ;
                        SQL = SQL + ComNum.VBLF + "  EMRNO, FORMNO, USEID, CHARTDATE,"                                       ;
                        SQL = SQL + ComNum.VBLF + "  CHARTTIME, PTNO, INOUTCLS, MEDFRDATE,"                                  ;
                        SQL = SQL + ComNum.VBLF + "  MEDFRTIME, MEDENDDATE, MEDENDTIME, MEDDEPTCD,"                          ;
                        SQL = SQL + ComNum.VBLF + "  MEDDRCD, WRITEDATE, WRITETIME, CERTDATE,"                               ;
                        SQL = SQL + ComNum.VBLF + "  CERTTIME, CERTUSEID, CERTNO, DSIGNDATE,"                                ;
                        SQL = SQL + ComNum.VBLF + "  DSIGNTIME, DSIGNSABUN)"                                                 ;

                        SQL = SQL + ComNum.VBLF + "  SELECT EMRNO, FORMNO, USEID, CHARTDATE,"                                ;
                        SQL = SQL + ComNum.VBLF + "  CHARTTIME, PTNO, INOUTCLS, MEDFRDATE,"                                  ;
                        SQL = SQL + ComNum.VBLF + "  MEDFRTIME, MEDENDDATE, MEDENDTIME, MEDDEPTCD,"                          ;
                        SQL = SQL + ComNum.VBLF + "  MEDDRCD, WRITEDATE, WRITETIME, CERTDATE,"                               ;
                        SQL = SQL + ComNum.VBLF + "  CERTTIME , CERTUSEID, CERTNO, TO_CHAR(SYSDATE, 'YYYYMMDD'), TO_CHAR(SYSDATE, 'HH24MISS')" ;
                        SQL = SQL + ComNum.VBLF + "  ,'" + clsType.User.IdNumber + "'"  ;
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMRXML"                                                 ;
                        SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                        
                        SQL = SQL + ComNum.VBLF + "  UNION ALL";
                        SQL = SQL + ComNum.VBLF + "  SELECT EMRNO, FORMNO, CHARTUSEID, CHARTDATE,";
                        SQL = SQL + ComNum.VBLF + "  CHARTTIME, PTNO, INOUTCLS, MEDFRDATE,";
                        SQL = SQL + ComNum.VBLF + "  MEDFRTIME, MEDENDDATE, MEDENDTIME, MEDDEPTCD,";
                        SQL = SQL + ComNum.VBLF + "  MEDDRCD, WRITEDATE, WRITETIME, CERTDATE,";
                        SQL = SQL + ComNum.VBLF + "  '' , CHARTUSEID, CERTNO, TO_CHAR(SYSDATE, 'YYYYMMDD'), TO_CHAR(SYSDATE, 'HH24MISS')";
                        SQL = SQL + ComNum.VBLF + "  ,'" + clsType.User.IdNumber + "'";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST";
                        SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;

                        string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if(sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return;
                        }

                        if (RowAffected > 0)
                        {
                            SQL = "  UPDATE ADMIN.AEMRCHARTMST SET";
                            SQL = SQL + ComNum.VBLF + "    COMPUSEID = '" + clsType.User.IdNumber + "'";
                            SQL = SQL + ComNum.VBLF + "  , COMPDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')";
                            SQL = SQL + ComNum.VBLF + "  , COMPTIME = TO_CHAR(SYSDATE, 'HH24MISS')";
                            SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;

                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (sqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, sqlErr);
                                return;
                            }
                        }
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex) 
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        void Certify_PTNO()
        {
            string SQL = string.Empty;
            int RowAffected = 0;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Trim() == "True")
                    {
                        string strPtNo = SS1_Sheet1.Cells[i, 1].Text.Trim();
                        string strInDate = SS1_Sheet1.Cells[i, 3].Text.Trim();
                        string strOutDate = SS1_Sheet1.Cells[i, 4].Text.Trim();
                        string strDeptCode = SS1_Sheet1.Cells[i, 5].Text.Trim();
                        string strDrCode = SS1_Sheet1.Cells[i, 7].Text.Trim();

                        SQL = " INSERT INTO ADMIN.EMRXML_DUALSIGN_PTNO(";
                        SQL = SQL + ComNum.VBLF + "  PTNO, INDATE, OUTDATE, DEPTCODE, ";
                        SQL = SQL + ComNum.VBLF + "  DRCODE, CONFIRMDATE, CONFIRMSABUN) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + "'" + strPtNo + "', TO_DATE('" + strInDate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + " TO_DATE('" + strOutDate + "','YYYY-MM-DD'),'" + strDeptCode + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', SYSDATE, " + clsType.User.IdNumber + ")";

                        string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return;
                        }
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, 0].Text = "True";
        }

        private void BtnAll2_Click(object sender, EventArgs e)
        {
            SS2_Sheet1.Cells[0, 0, SS2_Sheet1.RowCount - 1, 0].Text = "True";
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;


            SS1_CellDoubleClick(e.Row);
        }

        private void SS1_CellDoubleClick(int Row)
        {
            string strChk = SS1_Sheet1.Cells[Row, 0].Text.Trim() == "True" ? "1" : "0";
            string strPano = SS1_Sheet1.Cells[Row, 1].Text.Trim();
            string strInDate = SS1_Sheet1.Cells[Row, 3].Text.Trim().Replace("-", "");
            string strDeptCode = SS1_Sheet1.Cells[Row, 5].Text.Trim();
            string strDrCode = SS1_Sheet1.Cells[Row, 7].Text.Trim();

            p = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "I", strInDate, strDeptCode);

            READ_DATA_DETAIL(strPano, strInDate, strDeptCode, strDrCode, strChk);
        }

        void READ_DATA_DETAIL(string argPTNO, string ArgInDate, string ArgDeptCode, string ArgDrCode, string ArgChk = "")
        {
            SS2_Sheet1.RowCount = 0;

            string strSql = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                strSql = "SELECT A.EMRNO, A.PTNO, B.SNAME, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE, B.OUTDATE,";
                strSql = strSql + ComNum.VBLF + "  B.DEPTCODE, B.DRCODE, D.DRNAME, A.USEID, M.KORNAME, A.FORMNO, F.FORMNAME, A.CHARTDATE, A.CHARTTIME,";
                strSql = strSql + ComNum.VBLF + "  E.DSIGNDATE, E.DSIGNTIME, F.OLDGB";
                strSql = strSql + ComNum.VBLF + "  FROM ";
                strSql = strSql + ComNum.VBLF + "  (";
                strSql = strSql + ComNum.VBLF + "    SELECT PTNO, CHARTDATE, CHARTTIME, EMRNO, MEDFRDATE, RPAD(USEID, 6, ' ') AS USEID, FORMNO, 1 AS UPDATENO";
                strSql = strSql + ComNum.VBLF + "    FROM ADMIN.EMRXMLMST";
                strSql = strSql + ComNum.VBLF + "   WHERE CHARTDATE >='20160410'";
                strSql = strSql + ComNum.VBLF + "     AND CHARTDATE <='" + strSysDate + "'";
                strSql = strSql + ComNum.VBLF + "  UNION ALL";
                strSql = strSql + ComNum.VBLF + "    SELECT PTNO, CHARTDATE, CHARTTIME, EMRNO, MEDFRDATE, RPAD(CHARTUSEID, 6, ' ') AS USEID, FORMNO, UPDATENO";
                strSql = strSql + ComNum.VBLF + "    FROM ADMIN.AEMRCHARTMST";
                strSql = strSql + ComNum.VBLF + "   WHERE CHARTDATE >='20160410'";
                strSql = strSql + ComNum.VBLF + "     AND CHARTDATE <='" + strSysDate + "'";
                strSql = strSql + ComNum.VBLF + "  )A";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.IPD_NEW_MASTER B";
                strSql = strSql + ComNum.VBLF + "       ON A.PTNO = B.PANO";
                strSql = strSql + ComNum.VBLF + "      AND B.INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI')";
                strSql = strSql + ComNum.VBLF + "      AND B.INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI')";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_DOCTOR D";
                strSql = strSql + ComNum.VBLF + "       ON D.DRCODE = B.DRCODE";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.INSA_MST M";
                strSql = strSql + ComNum.VBLF + "       ON M.SABUN = A.USEID";
                strSql = strSql + ComNum.VBLF + "    INNER JOIN ADMIN.AEMRFORM F";
                strSql = strSql + ComNum.VBLF + "       ON F.FORMNO = A.FORMNO";
                strSql = strSql + ComNum.VBLF + "      AND F.UPDATENO = A.UPDATENO";
                strSql = strSql + ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.EMRXML_DUALSIGN E";
                strSql = strSql + ComNum.VBLF + "       ON E.EMRNO = A.EMRNO";
                strSql = strSql + ComNum.VBLF + "  WHERE A.MEDFRDATE = '" + ArgInDate + "'";
                strSql = strSql + ComNum.VBLF + "    AND A.CHARTDATE >='20160410'";
                strSql = strSql + ComNum.VBLF + "    AND A.CHARTDATE <='" + strSysDate + "'";
                strSql = strSql + ComNum.VBLF + "    AND A.PTNO = '" + argPTNO + "'";
                strSql = strSql + ComNum.VBLF + "    AND EXISTS (";
                strSql = strSql + ComNum.VBLF + "                   SELECT TO_CHAR(SABUN3)";
                strSql = strSql + ComNum.VBLF + "                     FROM ADMIN.INSA_MST";
                //'('022101','022105','022150','022160')    --내과, 정형외과, 인턴, 일반의
                if (clsType.User.BuseCode == "044201")
                {
                    strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101','022105', '022150')";
                    //strSql = strSql + ComNum.VBLF + "                      AND CODE IN('27', '28') -- 레지던트, 인턴";
                    strSql = strSql + ComNum.VBLF + "                      AND A.USEID = SABUN";
                    strSql = strSql + ComNum.VBLF + "            )";
                }
                else
                {
                    switch (VB.Left(clsType.User.DrCode, 2))

                    {
                        case "22"://      '정형외과면
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022105')";
                            break;
                        case "01":
                        case "02":
                        case "03":
                        case "04":
                        case "05":
                        case "07":
                        case "09":
                        case "11": //'내과계면
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101')";
                            break;
                        default:
                            strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022150','022160')";
                            break;
                    }
                    strSql = strSql + ComNum.VBLF + "                      AND A.USEID = SABUN3";
                    //strSql = strSql + ComNum.VBLF + "                      AND CODE IN('27', '28') -- 레지던트, 인턴";
                    strSql = strSql + ComNum.VBLF + "            )";
                }

                strSql = strSql + ComNum.VBLF + "     AND A.FORMNO NOT IN (963, 1232)";
                strSql = strSql + ComNum.VBLF + "     AND B.DRCODE = '" + ArgDrCode + "'";
                strSql = strSql + ComNum.VBLF + "     AND B.DEPTCODE = '" + ArgDeptCode + "'";
                strSql = strSql + ComNum.VBLF + "     ORDER BY A.FORMNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SS2_Sheet1.RowCount = dt.Rows.Count;
                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00")
                        + " " + VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                    SS2_Sheet1.Cells[i, 9].Text =  dt.Rows[i]["KORNAME"].ToString().Trim();

                    if(dt.Rows[i]["DSIGNDATE"].ToString().Trim().Length > 0)
                    {
                        SS2_Sheet1.Cells[i, 11].Text = VB.Val(dt.Rows[i]["DSIGNDATE"].ToString().Trim()).ToString("0000-00-00");
                        SS2_Sheet1.Cells[i, 11].Text += " " + VB.Val(VB.Left(dt.Rows[i]["DSIGNTIME"].ToString().Trim(), 4)).ToString("00:00");
                    }

                    //READ_CERT(dt.Rows[i]["EMRNO"].ToString().Trim());

                    if (SS2_Sheet1.Cells[i, 11].Text.Length > 0)
                    {
                        SS2_Sheet1.Cells[i, 0].Text = "False";
                        SS2_Sheet1.Cells[i, 10].Text = "(*확인함)";
                        SS2_Sheet1.Cells[i, 10].ForeColor = Color.FromArgb(0, 0, 255);
                    }

                    SS2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS2_Sheet1.RowCount == 0)
                return;

            string strEmrNo = SS2_Sheet1.Cells[e.Row, 12].Text.Trim();
            string strFormNo = SS2_Sheet1.Cells[e.Row, 13].Text.Trim();
            double dUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo));


            //string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode
            using (frmEmrChartNew frmEmrChartNew = new frmEmrChartNew(strFormNo, dUpdateNo.ToString(), p, strEmrNo, "W"))
            {
                frmEmrChartNew.StartPosition = FormStartPosition.CenterScreen;
                frmEmrChartNew.ShowDialog(this);
            }
        }

    }
}
