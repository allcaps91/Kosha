using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// BST Interface
    /// </summary>
    public partial class frmBSTInterface : Form
    {
        EmrPatient AcpEmr = null;

        public frmBSTInterface()
        {
            InitializeComponent();
        }

        private void frmBSTInterface_Load(object sender, EventArgs e)
        {
            ssUserChart_Sheet1.RowCount = 0;
            ssChart_Sheet1.RowCount = 0;
            ChkMu.Enabled = false;
            WardList();
            chkTran.Visible = true;
            if (cboWard.Text.Trim() == "ER")
            {
                ChkMu.Visible = true;
                ChkMu.Enabled = true;
                chkTran.Visible = false;
            }

        }

        #region 함수

        /// <summary>
        /// 환자리스트를 가지고 온다
        /// </summary>
        void GetPatList()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            string strToDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(0).ToShortDateString();
            string strNextDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(1).ToShortDateString();

            try
            {
                ssUserChart_Sheet1.RowCount = 0;

                #region 주석
                //SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                //SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                //SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                //SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7 ";
                //SQL = SQL + ComNum.VBLF + " FROM   ADMIN.IPD_NEW_MASTER  M, ";
                //SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_PATIENT P, ";
                //SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_DOCTOR  D ";

                //switch (cboWard.Text.Trim())
                //{
                //    case "전체": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' "; break;
                //    case "MICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' "; break;
                //    case "SICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' "; break;
                //    case "ND":
                //    case "NR":
                //        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') "; break;
                //    //'Case "3B":   SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('3B','DR') "; //'COMBOBOX 처리
                //    default: SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text.Trim() + "' "; break;
                //}

                //SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                //SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                //SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";

                //SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                //SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
                #endregion
                if(cboWard.Text.Trim() == "ER")
                {
                    if (ChkMu.Checked == true)
                    {
                        SQL = " SELECT  'ER' ROOMCODE, B.PATIENT_ID PANO, '무명남' SNAME, '측정 값: ' AGE, VALUE SEX, '(mg/dL)' DEPTCODE, TRUNC(SYSDATE) INDATE, B.EMR ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST  B ";
                        SQL = SQL + ComNum.VBLF + " WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                        SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                        SQL = SQL + ComNum.VBLF + "   AND B.WARD LIKE 'ER%'";
                        SQL = SQL + ComNum.VBLF + "   AND B.PATIENT_ID IN ('0000000','00000000','000000000')";
                    }
                    else
                    {
                        SQL = " SELECT  'ER' ROOMCODE, I.PANO, I.SNAME, I.AGE, I.SEX, I.DEPTCODE, TO_CHAR(I.BDATE, 'YYYYMMDD') INDATE, SUM(DECODE(B.EMR, NULL, 1, 0)) EMR ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST  B ";
                        SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.OPD_MASTER I ";
                        SQL = SQL + ComNum.VBLF + "      ON SUBSTR(B.PATIENT_ID,1,8) = I.PANO";
                        SQL = SQL + ComNum.VBLF + "     AND I.ACTDATE >= TO_CHAR(SYSDATE-2, 'YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND I.DEPTCODE = 'ER'";
                        SQL = SQL + ComNum.VBLF + " WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                        SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                        SQL = SQL + ComNum.VBLF + "   AND B.WARD LIKE 'ER%'";
                        SQL = SQL + ComNum.VBLF + " GROUP BY SUBSTR(MEASURE_DT, 0, 8), I.PANO, I.SNAME, I.AGE, I.SEX, I.DEPTCODE, TO_CHAR(I.BDATE, 'YYYYMMDD')";
                        SQL = SQL + ComNum.VBLF + " ORDER BY I.SNAME, INDATE DESC";
                    }

                }
                else
                {
                    SQL = "SELECT I.ROOMCODE, I.PANO, I.SNAME, I.AGE, I.SEX, I.DEPTCODE, TO_CHAR(I.INDATE, 'YYYYMMDD') INDATE, SUM(DECODE(B.EMR, NULL, 1, 0)) EMR ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST  B ";
                    //SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_SPECMST S ";
                    //SQL = SQL + ComNum.VBLF + "      ON S.SPECNO = B.PATIENT_ID";
                    SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.IPD_NEW_MASTER I ";
                    //SQL = SQL + ComNum.VBLF + "      ON S.PANO = I.PANO";
                    SQL = SQL + ComNum.VBLF + "      ON SUBSTR(B.PATIENT_ID,1,8) = I.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND ((I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') AND I.GBSTS <> '7') OR (I.JDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND I.GBSTS = '7')";
                    SQL = SQL + ComNum.VBLF + "       OR I.OUTDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
                    //SQL = SQL + ComNum.VBLF + "     AND (I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') OR I.JDATE = TRUNC(SYSDATE))";
                    //SQL = SQL + ComNum.VBLF + "     AND I.GBSTS <> '7'";
                    SQL = SQL + ComNum.VBLF + " WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    if (chkTran.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND EXISTS ( ";
                        SQL = SQL + ComNum.VBLF + "              SELECT * FROM ADMIN.IPD_TRANSFOR SUB ";
                        SQL = SQL + ComNum.VBLF + "              WHERE SUB.TRSDATE >= I.INDATE ";
                        SQL = SQL + ComNum.VBLF + "                AND SUB.TRSDATE <= TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + "                AND SUB.PANO = I.PANO ";
                        SQL = SQL + ComNum.VBLF + "                AND(TOWARD = '" + cboWard.Text.Trim() + "' OR FRWARD = '" + cboWard.Text.Trim() + "'))  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    }
                    SQL = SQL + ComNum.VBLF + " GROUP BY SUBSTR(MEASURE_DT, 0, 8), I.ROOMCODE, I.PANO, I.SNAME, I.AGE, I.SEX, I.DEPTCODE, TO_CHAR(I.INDATE, 'YYYYMMDD')";
                    SQL = SQL + ComNum.VBLF + " ORDER BY I.ROOMCODE, SNAME, INDATE DESC";
                }

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssUserChart_Sheet1.RowCount = dt.Rows.Count;
                    ssUserChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                    {
                        if (ssUserChart_Sheet1.Columns[i].Tag != null)
                        {
                            ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssUserChart_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 6].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        if (dt.Rows[i]["EMR"].ToString().Trim() != "0")
                        {
                            ssUserChart_Sheet1.Cells[i, 7].ForeColor = System.Drawing.Color.Red;
                            ssUserChart_Sheet1.Cells[i, 7].Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
                            ssUserChart_Sheet1.Cells[i, 7].Text = dt.Rows[i]["EMR"].ToString().Trim();
                        }
                        else 
                        {
                            ssUserChart_Sheet1.Cells[i, 7].Text = "-";
                        }
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetPatList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// BST 검사 결과를 가지고 온다.
        /// </summary>
        void GetBSTList(string strPano)
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssChart_Sheet1.RowCount = 0;

                if (cboWard.Text.Trim() == "ER")
                {
                        SQL = "SELECT MEASURE_DT, B.VALUE, SUBSTR(B.PATIENT_ID,1,8) PATIENT_ID ,TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_MASTER I  ";
                        SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_INTERFACE_BST B";
                        SQL = SQL + ComNum.VBLF + "      ON MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                        SQL = SQL + ComNum.VBLF + "      AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                        SQL = SQL + ComNum.VBLF + "      AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                        SQL = SQL + ComNum.VBLF + "      AND SUBSTR(B.PATIENT_ID,1,8) = '" + strPano + "'";
                        //SQL = SQL + ComNum.VBLF + "     WHERE I.ACTDATE = '" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "'";
                        SQL = SQL + ComNum.VBLF + "     AND I.ACTDATE >= TO_CHAR(SYSDATE-2, 'YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND I.DEPTCODE = 'ER'";
                        SQL = SQL + ComNum.VBLF + "     AND I.PANO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "     ORDER BY MEASURE_DT DESC";
                }
                else
                {
                    //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                    SQL = "SELECT MEASURE_DT, B.VALUE, SUBSTR(B.PATIENT_ID,1,8) PATIENT_ID ,TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER I  ";
                    SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_INTERFACE_BST B";
                    SQL = SQL + ComNum.VBLF + "      ON MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "     AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    if (chkTran.Checked == true)
                    {

                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    }
                    SQL = SQL + ComNum.VBLF + "      AND SUBSTR(B.PATIENT_ID,1,8) = '" + strPano + "'";
                    //SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_SPECMST S ";
                    //SQL = SQL + ComNum.VBLF + "      ON S.SPECNO = B.PATIENT_ID";
                    //SQL = SQL + ComNum.VBLF + "     AND S.PANO   = I.PANO";
                    //SQL = SQL + ComNum.VBLF + "     AND S.BDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                    //SQL = SQL + ComNum.VBLF + " WHERE (I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') OR I.JDATE = TRUNC(SYSDATE))";
                    //SQL = SQL + ComNum.VBLF + "   AND I.GBSTS <> '7'";
                    SQL = SQL + ComNum.VBLF + " WHERE ((I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') AND I.GBSTS <> '7') OR (I.JDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND I.GBSTS = '7')";
                    SQL = SQL + ComNum.VBLF + "       OR OUTDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + "   AND I.PANO = '" + strPano + "'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY MEASURE_DT DESC";
                }
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssChart_Sheet1.RowCount = dt.Rows.Count;
                    ssChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime dtpDT = DateTime.ParseExact(dt.Rows[i]["MEASURE_DT"].ToString(), "yyyyMMddHHmmss", null);


                        if(string.IsNullOrEmpty(dt.Rows[i]["EMR"].ToString().Trim()) == false)
                        {
                            ssChart_Sheet1.Cells[i, 0].Locked = true;
                            //ssChart_Sheet1.Cells[i, 0].Text = "False";
                        }
                        
                        ssChart_Sheet1.Cells[i, 1].Text = dtpDT.ToString("yyyy-MM-dd");
                        ssChart_Sheet1.Cells[i, 2].Text = dtpDT.ToString("HH:mm");
                        ssChart_Sheet1.Cells[i, 2].Tag = dtpDT.ToString("yyyyMMddHHmmss");
                        ssChart_Sheet1.Cells[i, 3].Text = dt.Rows[i]["VALUE"].ToString().Trim();
                        //ssChart_Sheet1.Cells[i, 3].Tag  = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 3].Tag = dt.Rows[i]["PATIENT_ID"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EMR"].ToString().Trim();
                        
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetBSTList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 해당일자에 BST 검사 결과 갯수를 가져온다.
        /// </summary>
        private void GetBstCount()
        {
            DataTable dt = null;
            string SQL = string.Empty;
      
            try
            {
                ssChart_Sheet1.RowCount = 0;

                if (cboWard.Text.Trim() == "ER")
                {
                    SQL = "SELECT 1";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B";
                    SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    SQL = SQL + ComNum.VBLF + "  AND B.KIND IS NULL";
                }
                else
                {
                    //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                    SQL = "SELECT 1";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B";
                    SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    SQL = SQL + ComNum.VBLF + "  AND B.KIND IS NULL";
                }

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    Text = dtpExamDate.Value.ToString("yyyy-MM-dd") + " 일자의 총 검사 갯수 : " + dt.Rows.Count;
                }
                dt.Dispose();

                if (cboWard.Text.Trim() == "ER")
                {
                    SQL = "select timeline,count(*) cnt from ( SELECT case  when   substr(MEASURE_DT,9,2) >= '00' and substr(MEASURE_DT,9,2) <= '09'  then '10'";
                    SQL = SQL + ComNum.VBLF + " when   substr(MEASURE_DT,9,2) >= '10' and substr(MEASURE_DT,9,2) <= '13'  then '14'";
                    SQL = SQL + ComNum.VBLF + " when   substr(MEASURE_DT,9,2) >= '14' and substr(MEASURE_DT,9,2) <= '21'  then '22'";
                    SQL = SQL + ComNum.VBLF + " else   '23' end timeline ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B";
                    SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    SQL = SQL + ComNum.VBLF + "  AND B.KIND IS NULL ) group by timeline order by timeline";
                }
                else
                {
                    //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                    SQL = "select timeline,count(*) cnt from ( SELECT case  when   substr(MEASURE_DT,9,2) >= '00' and substr(MEASURE_DT,9,2) <= '09'  then '10'";
                    SQL = SQL + ComNum.VBLF + " when   substr(MEASURE_DT,9,2) >= '10' and substr(MEASURE_DT,9,2) <= '13'  then '14'";
                    SQL = SQL + ComNum.VBLF + " when   substr(MEASURE_DT,9,2) >= '14' and substr(MEASURE_DT,9,2) <= '21'  then '22'";
                    SQL = SQL + ComNum.VBLF + " else   '23' end timeline ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B";
                    SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                    SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                    SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                    SQL = SQL + ComNum.VBLF + "  AND B.KIND IS NULL ) group by timeline  order by timeline";
                }

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    labBSTCOUNT.Text = "";

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["timeline"].ToString().Trim() == "10")
                        {
                            labBSTCOUNT.Text = "10시 기준: " + dt.Rows[k]["cnt"].ToString().Trim() + "개";
                        }
                        else if (dt.Rows[k]["timeline"].ToString().Trim() == "14")
                        {
                            labBSTCOUNT.Text += " /14시 기준: " + dt.Rows[k]["cnt"].ToString().Trim() + "개";
                        }
                        else if (dt.Rows[k]["timeline"].ToString().Trim() == "22")
                        {
                            labBSTCOUNT.Text += " /22시 기준: " + dt.Rows[k]["cnt"].ToString().Trim() + "개";
                        }
                        else if (dt.Rows[k]["timeline"].ToString().Trim() == "23")
                        {
                            labBSTCOUNT.Text += " /22시 이후: " + dt.Rows[k]["cnt"].ToString().Trim() + "개";
                        }
                    }
                       
                }
                dt.Dispose();


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetBstCount()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        private void CheckMu()
        {
            DataTable dt = null;
            string SQL = string.Empty;
            //bool rtnVal = false;

            try
            {
                SQL = " SELECT  'ER' ROOMCODE, B.PATIENT_ID PANO, '무명남' SNAME, '측정 값: ' AGE, VALUE SEX, '(mg/dL)' DEPTCODE, TRUNC(SYSDATE) INDATE, B.EMR EMR ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST  B ";
                SQL = SQL + ComNum.VBLF + " WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "   AND B.WARD LIKE 'ER%'";
                SQL = SQL + ComNum.VBLF + "   AND B.PATIENT_ID IN ('0000000','00000000','000000000')";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                }
                
                if (dt.Rows.Count > 0)
                {
                        MessageBox.Show("★무명환자가 있습니다★ " + ComNum.VBLF + "환자 접수 후 '무명남 조회' 체크 및 환자조회 하십시오.");
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST 무명환자 조회중 에러발생", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void WardList()
        {
            #region ComboWard_SET()
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            int sIndex = -1;
            int sCount = 0;

            try
            {
                SQL = " SELECT NAME WARDCODE, MATCH_CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        cboWard.Items.Add(reader.GetValue(0).ToString().Trim());
                        if (reader.GetValue(1).ToString().Trim().Equals(clsType.User.BuseCode))
                        {
                            sIndex = sCount;
                        }
                            sCount += 1;
                        
                    }
                }

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;
                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface WardList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }


        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            bool rtnVal = true;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            //OracleDataReader reader = null;

            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for(int i = 0; i < ssChart_Sheet1.RowCount; i ++)
                {
                    //if (ssChart_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    if (string.IsNullOrEmpty(ssChart_Sheet1.Cells[i, 4].Text.Trim()) == true && ssChart_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {

                        double dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                        double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                        string strChartDate = ssChart_Sheet1.Cells[i, 2].Tag.ToString().Substring(0, 8);
                        string strChartTime = ssChart_Sheet1.Cells[i, 2].Tag.ToString().Substring(8, 6);

                        #region 해당 날짜/시간에 작성된 기록지 있는지 확인
                        //SQL = "SELECT 1 ";
                        //SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
                        //SQL += ComNum.VBLF + "WHERE MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                        //SQL += ComNum.VBLF + "  AND FORMNO = 1572";
                        //SQL += ComNum.VBLF + "  AND CHARTDATE = '" + strChartDate + "'";
                        //SQL += ComNum.VBLF + "  AND CHARTTIME = '" + strChartTime + "'";

                        //SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //{
                        //    ComFunc.MsgBoxEx(this, "기록지 작성여부 확인 도중 에러가 발생했습니다.");
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    Cursor.Current = Cursors.Default;
                        //    return false;
                        //}

                        //if (reader.HasRows)
                        //{
                        //    reader.Dispose();
                        //    continue;
                        //}

                        //reader.Dispose();
                        #endregion

                        #region //저장 CHRATMAST
                        string strSaveFlag = string.Empty;
                        DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                        if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString(),
                                            strChartDate, strChartTime,
                                            clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                            dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                        {
                            ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                        else
                        {

                            #region  //저장 CHARTROW
                            string strItemCd = "I0000009122"; //Glucose;
                            string ItemValue = ssChart_Sheet1.Cells[i, 3].Text.Trim();

                            SQL = string.Empty;
                            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                            SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                            SQL += ComNum.VBLF + "VALUES (";
                            SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                            SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                            SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                            SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                            SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                            SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                            SQL += ComNum.VBLF + "'" + ItemValue + "',";   //ITEMVALUE
                            SQL += ComNum.VBLF + "0,";   //DSPSEQ
                            SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                            SQL += ComNum.VBLF + ")";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                                return false;
                            }
                            #endregion //CHARTROW

                            #region 인터페이스 테이블 EMR 연동 시간 업데이트.
                            if (RowAffected > 0)
                            {
                                SQL = string.Empty;
                                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "EXAM_INTERFACE_BST ";
                                SQL += ComNum.VBLF + "SET ";
                                SQL += ComNum.VBLF + "EMR = SYSDATE";
                                SQL += ComNum.VBLF + "WHERE SUBSTR(PATIENT_ID, 1, 8) = '" + ssChart_Sheet1.Cells[i, 3].Tag.ToString().Trim() + "'";   //PATIENT_ID
                                SQL += ComNum.VBLF + "  AND MEASURE_DT = '" + ssChart_Sheet1.Cells[i, 2].Tag.ToString() + "'";   //MEASURE_DT
                                                                                                                                 //SQL += ComNum.VBLF + "  AND VALUE      = '" + ItemValue + "'";   //MEASURE_DT

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, "인터페이스 테이블에 차트 시간을 기록 하는 도중 에러가 발생했습니다.");
                                    return false;
                                }

                                #region 전자인증
                                if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                                {
                                    clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                                }
                                #endregion

                                #region 2020-08-28 안정수, 오더데이터 발생 추가 
                                if (clsEmrQuery.Ins_OCS_IORDER_BST(AcpEmr, dtpExamDate.Value.ToString("yyyy-MM-dd"), ssChart_Sheet1.Cells[i, 2].Tag.ToString(), cboWard.Text.Trim()) == false)
                                {
                                    ComFunc.MsgBoxEx(this, "OCS_IORDER(BST) 저장 중 에러가 발생했습니다.");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion //저장 CHRATMAST
                    } 
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, "당뇨기록지를 저장 하는 도중 에러가 발생했습니다.");
                clsDB.SaveSqlErrLog(ex.Message, "BSTInterface - SaveData", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            return rtnVal;
        }


        /// <summary>
        /// 일괄 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveDataAll()
        {
            if (cboWard.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "병동을 선택하세요.");
                return false;
            }

            bool rtnVal = true;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            int AllRowAffected = 0;
            DataTable dt = null;

            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");
            EmrPatient AcpEmr2 = null;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, S.PANO, TO_CHAR(I.INDATE, 'YYYYMMDD') MEDFRDATE, I.DEPTCODE, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = "SELECT MEASURE_DT, B.VALUE, SUBSTR(B.PATIENT_ID,1,8) PATIENT_ID, TO_CHAR(I.INDATE, 'YYYYMMDD') MEDFRDATE, I.DEPTCODE, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B  ";
                //SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_SPECMST S ";
                //SQL = SQL + ComNum.VBLF + "      ON S.SPECNO = B.PATIENT_ID";
                //SQL = SQL + ComNum.VBLF + "     AND S.BDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.IPD_NEW_MASTER I ";
                SQL = SQL + ComNum.VBLF + "      ON ((I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') AND I.GBSTS <> '7') OR (I.JDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND I.GBSTS = '7')";
                SQL = SQL + ComNum.VBLF + "       OR OUTDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
                //SQL = SQL + ComNum.VBLF + "      ON (I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') OR I.JDATE = TRUNC(SYSDATE))";
                //SQL = SQL + ComNum.VBLF + "     AND I.GBSTS <> '7'";
                SQL = SQL + ComNum.VBLF + "     AND I.PANO = SUBSTR(B.PATIENT_ID,1,8)";
                SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '" + cboWard.Text.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "  AND B.EMR IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY MEASURE_DT DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return false;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(cboWard.Text.Trim() == "ER")
                    {
                        AcpEmr2 = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, dt.Rows[i]["PATIENT_ID"].ToString().Trim(), "O", dt.Rows[i]["MEDFRDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                    else
                    {
                        AcpEmr2 = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, dt.Rows[i]["PATIENT_ID"].ToString().Trim(), "I", dt.Rows[i]["MEDFRDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                    
                    if (AcpEmr2 == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                    if (AcpEmr2.ptNo != dt.Rows[i]["PATIENT_ID"].ToString().Trim())
                    {
                        continue;
                    }


                    if (AcpEmr2.age.IndexOf("개월") != -1)
                    {
                        AcpEmr2.age = "0";
                    }


                    //20200810093944

                    string strChartDate = dt.Rows[i]["MEASURE_DT"].ToString().Substring(0, 8);
                    string strChartTime = dt.Rows[i]["MEASURE_DT"].ToString().Substring(8, 6);

                    #region 해당 날짜/시간에 작성된 기록지 있는지 확인
                    //SQL = "SELECT 1 ";
                    //SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
                    //SQL += ComNum.VBLF + "WHERE MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                    //SQL += ComNum.VBLF + "  AND FORMNO = 1572";
                    //SQL += ComNum.VBLF + "  AND CHARTDATE = '" + strChartDate + "'";
                    //SQL += ComNum.VBLF + "  AND CHARTTIME = '" + strChartTime + "'";

                    //SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    //{
                    //    ComFunc.MsgBoxEx(this, "기록지 작성여부 확인 도중 에러가 발생했습니다.");
                    //    clsDB.setRollbackTran(clsDB.DbCon);
                    //    Cursor.Current = Cursors.Default;
                    //    return false;
                    //}

                    //if (reader.HasRows)
                    //{
                    //    reader.Dispose();
                    //    continue;
                    //}

                    //reader.Dispose();
                    #endregion

                    double dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                    double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));


                    #region //저장 CHRATMAST
                    string strSaveFlag = string.Empty;
                    DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                    if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr2, ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString(),
                                        strChartDate, strChartTime,
                                        clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                        dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                    {
                        ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    else
                    {
                        #region  //저장 CHARTROW
                        string strItemCd = "I0000009122"; //Glucose;
                        string ItemValue = dt.Rows[i]["VALUE"].ToString();

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemValue + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + "0,";   //DSPSEQ
                        SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return false;
                        }

                        AllRowAffected += RowAffected;
                        #endregion //CHARTROW

                        #region 인터페이스 테이블 EMR 연동 시간 업데이트.
                        if (RowAffected > 0)
                        {
                            SQL = string.Empty;
                            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "EXAM_INTERFACE_BST ";
                            SQL += ComNum.VBLF + "SET ";
                            SQL += ComNum.VBLF + "EMR = SYSDATE";
                            SQL += ComNum.VBLF + "WHERE SUBSTR(PATIENT_ID, 1, 8) = '" + dt.Rows[i]["PATIENT_ID"].ToString() + "'";   //PATIENT_ID
                            SQL += ComNum.VBLF + "  AND MEASURE_DT = '" + dt.Rows[i]["MEASURE_DT"].ToString() + "'";   //MEASURE_DT

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, "인터페이스 테이블에 차트 시간을 기록 하는 도중 에러가 발생했습니다.");
                                return false;
                            }

                            #region 전자인증
                            if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                            {
                                clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                            }
                            #endregion

                            #region 2020-08-28 안정수, 오더데이터 발생 추가 
                            if (clsEmrQuery.Ins_OCS_IORDER_BST(AcpEmr2, dtpExamDate.Value.ToString("yyyy-MM-dd"), dt.Rows[i]["MEASURE_DT"].ToString(), cboWard.Text.Trim()) == false)
                            {
                                ComFunc.MsgBoxEx(this, "OCS_IORDER(BST) 저장 중 에러가 발생했습니다.");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion //저장 CHRATMAST


                }

                dt.Dispose();

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "총 " + AllRowAffected + "개의 기록지가 저장되었습니다.");

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, "당뇨기록지를 저장 하는 도중 에러가 발생했습니다.");
                clsDB.SaveSqlErrLog(ex.Message, "BSTInterface - SaveData", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        #endregion



        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if(cboWard.Text.Trim() == "ER")
            {
                CheckMu();
            }
            
            GetBstCount();
            GetPatList();
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //GetBSTList(ssUserChart_Sheet1.Cells[ssUserChart_Sheet1.ActiveRowIndex, 1].Text.Trim());

            if (SaveData())
            {
                MessageBox.Show("저장이 완료 되었습니다.");
                GetBSTList(ssUserChart_Sheet1.Cells[ssUserChart_Sheet1.ActiveRowIndex, 1].Text.Trim());

            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            //DataTable dt = null;
            string SQL = string.Empty;
            int RowAffected = 0;

            try
            {
                //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = "UPDATE ADMIN.EXAM_INTERFACE_BST SET ";
                SQL = SQL + ComNum.VBLF + " PATIENT_ID = '" + TxtPano.Text.Trim() + "'  ";
                SQL = SQL + ComNum.VBLF + "      WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "     AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "     AND WARD LIKE '%ER%'";
                SQL = SQL + ComNum.VBLF + "      AND PATIENT_ID IN ('0000000','00000000','000000000')";

                string sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);

                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                    return;
                }
                else
                {
                    MessageBox.Show("저장완료 하였습니다.");
                    GetPatList();
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "환자번호 업데이트 중 오류발생", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            
            PanPanoChane.Visible = false;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExit3_Click(object sender, EventArgs e)
        {
            PanPanoChane.Visible = false;
        }

        private void ssUserChart_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssUserChart_Sheet1.RowCount == 0)
                return;

            if (ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim() == "0000000" || ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim() == "00000000" || ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim() == "000000000")
            {
                PanPanoChane.Visible = true;
                MessageBox.Show("OPD 미생성 응급검사 했던 환자 입니다. OPD 생성후 검사진행한 환자의 환자번호를 입력후 저장을 눌러주세요.");
                return;
            }
            else
            {
                if(cboWard.Text.Trim() == "ER")
                {
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim(), "O", ssUserChart_Sheet1.Cells[e.Row, 6].Text.Trim(), ssUserChart_Sheet1.Cells[e.Row, 5].Text.Trim());
                }
                else
                {
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim(), "I", ssUserChart_Sheet1.Cells[e.Row, 6].Text.Trim(), ssUserChart_Sheet1.Cells[e.Row, 5].Text.Trim());
                }
                

                if (AcpEmr == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                if (AcpEmr.age.IndexOf("개월") != -1)
                {
                    AcpEmr.age = "0";
                }

                if (ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim() != AcpEmr.ptNo)
                {
                    ComFunc.MsgBoxEx(this, "등록번호가 다릅니다 다시 클릭해주세요");
                    ssChart_Sheet1.RowCount = 0;
                    return;
                }

                GetBSTList(ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim());
            }
        
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
                return;

            ssChart_Sheet1.Cells[0, 0, ssChart_Sheet1.RowCount - 1, 0].Text = chkAll.Checked.ToString();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (SaveDataAll())
            {
            //    MessageBox.Show("저장이 완료 되었습니다.");
            //    GetBSTList(ssUserChart_Sheet1.Cells[ssUserChart_Sheet1.ActiveRowIndex, 1].Text.Trim());
                
            }
            Cursor.Current = Cursors.Default;
        }


    }
}
