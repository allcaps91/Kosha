using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmChartComplete
    /// Description     : 챠트 검수 완료 내역 조회
    /// Author          : 이현종
    /// Create Date     : 2019-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmChartComplete.frm) >> frmChartComplete.cs 폼이름 재정의" />
    /// 
    public partial class frmChartComplete : Form
    {
        //환자정보 전달
        public delegate void SendOutPatNo(string strPtNo, string strOutDate);
        public event SendOutPatNo rSendPatInfo;

        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        /// <summary>
        /// 미비통계
        /// </summary>
        frmEmrJobMiBiStatisticsTewon fEmrJobMiBiStatisticsTewon = null;

        public frmChartComplete()
        {
            InitializeComponent();
        }

        private void FrmChartComplete_Load(object sender, EventArgs e)
        {
            dtpSDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-1);
            dtpEDATE.Value = dtpSDATE.Value;

            txtPano.Clear();
            lblPatient.Text = "";

            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("1.검수완료");
            cboGubun2.Items.Add("2.미검수");
            cboGubun2.Items.Add("3.전체");
            cboGubun2.SelectedIndex = 0;


            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.퇴원일");
            cboGubun.Items.Add("2.검수일");
            cboGubun.SelectedIndex = 0;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }

        void GetSearhData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;

            if(VB.Left(cboGubun.Text, 1).Equals("2") && VB.Left(cboGubun2.Text, 1).Equals("1") == false)
            {
                ComFunc.MsgBoxEx(this, "검수일 기준 조회 시 검수완료 항목만 조회가 가능합니다.");
                return;
            }


            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT MST.PANO, B.SNAME,  ";
                SQL += ComNum.VBLF + "(SELECT KORNAME FROM ADMIN.INSA_MST WHERE TRIM(SABUN) = TRIM(MST.CSABUN)) AS KORNAME,";
                SQL += ComNum.VBLF + "MEDFRDATE, CDATE, CSABUN, TDEPT, TO_CHAR(D.OUTDATE, 'YYYY-MM-DD') OUTDATE, D.DEPTCODE, BB.DRNAME";
                #region 미비
                SQL += ComNum.VBLF + (", CASE WHEN");
                // SQL += ComNum.VBLF + ("  EXISTS(SELECT 1 FROM ADMIN.EMRMIBI WHERE PTNO = MST.PANO AND MEDDEPTCD = MST.TDEPT AND MEDFRDATE = MST.MEDFRDATE  AND MIBICLS = 1) THEN '●' ");
                SQL += ComNum.VBLF + ("  EXISTS(SELECT 1 FROM ADMIN.EMRMIBI WHERE PTNO = MST.PANO AND MEDFRDATE = MST.MEDFRDATE  AND MIBICLS = 1) THEN '●' ");
                SQL += ComNum.VBLF + (" ELSE NULL ");
                SQL += ComNum.VBLF + (" END MIBI");
                #endregion
                SQL += ComNum.VBLF + " FROM ( ";

                if (VB.Left(cboGubun.Text, 1).Equals("2"))
                {
                    SQL += ComNum.VBLF + " SELECT A.PANO, B.MEDFRDATE, TO_CHAR(B.CDATE, 'YYYY-MM-DD HH24:MI') CDATE, B.CSABUN, A.TDEPT ";
                    SQL += ComNum.VBLF + "FROM ADMIN.MID_SUMMARY A, ADMIN.EMRXML_COMPLETE B";
                    SQL += ComNum.VBLF + "WHERE A.PANO = B.PTNO";
                    SQL += ComNum.VBLF + "  AND A.INDATE = B.INDATE";
                    SQL += ComNum.VBLF + "  AND A.OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                    if(txtPano.Text.Trim().Length > 0)
                    {
                        SQL += ComNum.VBLF + "  AND PANO = '" + txtPano.Text.Trim() + "' ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND B.CDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString()  + " 00:00','YYYY-MM-DD HH24:MI')";
                        SQL += ComNum.VBLF + "  AND B.CDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString()  + " 23:59','YYYY-MM-DD HH24:MI')";
                    }
                }
                else
                {
                    switch(VB.Left(cboGubun2.Text, 1))
                    {
                        case "1":
                            SQL += ComNum.VBLF + " SELECT A.PANO, B.MEDFRDATE, TO_CHAR(B.CDATE, 'YYYY-MM-DD HH24:MI') CDATE, B.CSABUN, A.TDEPT ";
                            SQL += ComNum.VBLF + "FROM ADMIN.MID_SUMMARY A, ADMIN.EMRXML_COMPLETE B";
                            SQL += ComNum.VBLF + "WHERE A.PANO = B.PTNO";
                            SQL += ComNum.VBLF + "  AND A.INDATE = B.INDATE";
                            SQL += ComNum.VBLF + "  AND A.OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "  AND PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "  AND A.OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "  AND A.OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }
                            break;
                        case "2":
                            SQL += ComNum.VBLF + " SELECT PANO, TO_CHAR(INDATE,'YYYYMMDD') MEDFRDATE, '' CDATE, '' CSABUN, A.TDEPT";     
                            SQL += ComNum.VBLF + "   FROM ADMIN.MID_SUMMARY A ";
                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "  WHERE OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "    AND OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }
                            SQL += ComNum.VBLF + "  AND OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + " MINUS";
                            SQL += ComNum.VBLF + " SELECT A.PANO, TO_CHAR(A.INDATE,'YYYYMMDD') MEDFRDATE, '' CDATE, '' CSABUN, A.TDEPT";
                            SQL += ComNum.VBLF + "   FROM ADMIN.MID_SUMMARY A, ADMIN.EMRXML_COMPLETE B";
                            SQL += ComNum.VBLF + " WHERE A.PANO = B.PTNO";

                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "  AND A.PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "    AND A.OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "    AND A.OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }

                            SQL += ComNum.VBLF + "  AND A.OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "   AND A.INDATE = B.INDATE";

                            break;
                        case "3":
                            SQL += ComNum.VBLF + "  SELECT A.PANO, B.MEDFRDATE, TO_CHAR(B.CDATE, 'YYYY-MM-DD HH24:MI') CDATE, TO_CHAR(B.CSABUN) CSABUN, A.TDEPT ";
                            SQL += ComNum.VBLF + "FROM ADMIN.MID_SUMMARY A, ADMIN.EMRXML_COMPLETE B";
                            SQL += ComNum.VBLF + "WHERE A.PANO = B.PTNO";
                            SQL += ComNum.VBLF + "  AND A.INDATE = B.INDATE";
                            SQL += ComNum.VBLF + "  AND A.OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "  AND PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "    AND A.OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "    AND A.OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }

                            SQL += ComNum.VBLF + " UNION ALL ";
                            SQL += ComNum.VBLF + " SELECT PANO, TO_CHAR(INDATE,'YYYYMMDD') MEDFRDATE, '' CDATE, '' CSABUN, TDEPT";
                            SQL += ComNum.VBLF + "   FROM ADMIN.MID_SUMMARY ";

                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "  WHERE OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "    AND OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }

                            SQL += ComNum.VBLF + "   AND OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + " MINUS";
                            SQL += ComNum.VBLF + " SELECT A.PANO, TO_CHAR(A.INDATE,'YYYYMMDD') MEDFRDATE, '' CDATE, '' CSABUN, A.TDEPT";
                            SQL += ComNum.VBLF + "   FROM ADMIN.MID_SUMMARY A, ADMIN.EMRXML_COMPLETE B";
                            SQL += ComNum.VBLF + " WHERE A.PANO = B.PTNO";
                            SQL += ComNum.VBLF + "   AND A.OUTDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";

                            if (txtPano.Text.Trim().Length > 0)
                            {
                                SQL += ComNum.VBLF + "   AND A.PANO = '" + txtPano.Text.Trim() + "' ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "   AND A.OUTDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD')";
                            }
                            SQL += ComNum.VBLF + "   AND A.INDATE = B.INDATE";
                            break;
                    }
                }

                SQL += ComNum.VBLF + " ) MST ";
                SQL += ComNum.VBLF + " INNER JOIN ADMIN.IPD_NEW_MASTER D";
                SQL += ComNum.VBLF + "    ON D.PANO = MST.PANO";
                SQL += ComNum.VBLF + "   AND D.INDATE >= TO_DATE(MST.MEDFRDATE ||' 00:00:00', 'YYYY-MM-DD HH24:MI:SS')";
                SQL += ComNum.VBLF + "   AND D.INDATE <= TO_DATE(MST.MEDFRDATE ||' 23:59:59', 'YYYY-MM-DD HH24:MI:SS')";
                SQL += ComNum.VBLF + "   AND D.GBSTS <> '9'";
                SQL += ComNum.VBLF + " INNER JOIN ADMIN.BAS_DOCTOR BB";
                SQL += ComNum.VBLF + "    ON BB.DRCODE = D.DRCODE";
                SQL += ComNum.VBLF + " INNER JOIN ADMIN.BAS_PATIENT B";
                SQL += ComNum.VBLF + "    ON B.PANO = MST.PANO";

                if (chkAll.Checked == false)
                {
                    SQL += ComNum.VBLF + " WHERE EXISTS ";
                    SQL += ComNum.VBLF + "  ( SELECT * ";
                    SQL += ComNum.VBLF + "      FROM ADMIN.MID_DEPTPART SUB";
                    SQL += ComNum.VBLF + "    WHERE SUB.DEPTCODE = MST.TDEPT ";
                    SQL += ComNum.VBLF + "         AND SUB.GUBUN = '2'";
                    SQL += ComNum.VBLF + "         AND SUB.SABUN = " + clsType.User.IdNumber + ")";
                }

                SQL += ComNum.VBLF + " ORDER BY PANO ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text =
                            string.Format("{0}-{1}-{2}",
                            VB.Left(SS1_Sheet1.Cells[i, 2].Text, 4),
                            VB.Mid(SS1_Sheet1.Cells[i, 2].Text, 5, 2),
                            VB.Right(SS1_Sheet1.Cells[i, 2].Text, 2));
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        //OutPatSp(i, SS1_Sheet1.Cells[i, 0].Text, dt.Rows[i]["MEDFRDATE"].ToString().Trim());

                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                        if (VB.Left(cboGubun2.Text, 1).Equals("2"))
                        { 
                            SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MIBI"].ToString().Trim();
                        }

                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 환자 이름 가져오기
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        private string GetPatientName(string strPano)
        {
            string rtnVal = string.Empty;
            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT SNAME";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            strSql = strSql + ComNum.VBLF + "  WHERE PANO = '" + strPano  + "'";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string strHeader = string.Empty;


            using (clsSpread CS = new clsSpread())
            {
                Font Titlefont = new Font("굴림체", 16);
                Font SubTitlefont = new Font("굴림체", 11);
                strHeader = CS.setSpdPrint_String("챠트 검수 완료 내역", Titlefont, clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("    (" + dtpSDATE.Value.ToShortDateString() + " ~ " + dtpEDATE.Value.ToShortDateString() + ")", SubTitlefont, clsSpread.enmSpdHAlign.Left, false, true);

                Titlefont.Dispose();
                SubTitlefont.Dispose();
            }
            SS1_Sheet1.PrintInfo.Header = strHeader;

            SS1_Sheet1.PrintInfo.Margin.Left = 35;
            SS1_Sheet1.PrintInfo.Margin.Top = 50;
            SS1_Sheet1.PrintInfo.Margin.Bottom = 50;

            SS1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowBorder = true;
            SS1_Sheet1.PrintInfo.ShowColor = false;
            SS1_Sheet1.PrintInfo.ShowGrid = true;
            SS1_Sheet1.PrintInfo.ShowShadows = false;
            SS1_Sheet1.PrintInfo.UseMax = false;
            //SS1_Sheet1.PrintInfo.Preview = true;
            SS1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            SS1_Sheet1.PrintInfo.ZoomFactor = 0.90f;
            SS1.PrintSheet(0);
        }

        /// <summary>
        /// 진료과 선택 목록 불러오기
        /// </summary>
        void ReadDEPT()
        {
            string SQL    = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            panFrame.Visible = true;

            ssDept_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT A.DEPTCODE DEPT1, B.DEPTCODE DEPT2, DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_CLINICDEPT A, ADMIN.MID_DEPTPART B";
                SQL = SQL + ComNum.VBLF + " WHERE A.DEPTCODE = B.DEPTCODE(+)";
                SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE NOT IN('OC', 'II','R6','HR','TO','PT','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + "    AND B.SABUN(+) = " + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + "    AND B.GUBUN(+) = '2'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.PRINTRANKING";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                ssDept_Sheet1.RowCount = dt.Rows.Count;
                ssDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssDept_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPT2"].ToString().Trim().Length > 0 ? "True" :"False";
                    ssDept_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    ssDept_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPT1"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }

        private void BtnSearchDept_Click(object sender, EventArgs e)
        {
            ReadDEPT();
            panFrame.Visible = true;
        }

        private void BtnSave1_Click(object sender, EventArgs e)
        {
            Save_Data();

            panFrame.Visible = false;
        }

        /// <summary>
        /// 진료과 선택 삭제 및 저장
        /// </summary>
        /// <param name="strGubun"></param>
        /// <returns></returns>
        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE ADMIN.MID_DEPTPART ";
                SQL += ComNum.VBLF + " WHERE SABUN = " + clsType.User.Sabun;
                SQL += ComNum.VBLF + "   AND GUBUN = '2'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < ssDept_Sheet1.NonEmptyRowCount; i++)
                {
                    if (ssDept_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = " INSERT INTO ADMIN.MID_DEPTPART(SABUN, DEPTCODE, GUBUN) VALUES (";
                        SQL += ComNum.VBLF + clsType.User.Sabun + ", '" + ssDept_Sheet1.Cells[i, 2].Text.Trim() + "','2')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void BtnExitPan_Click(object sender, EventArgs e)
        {
            panFrame.Visible = false;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            string strPano = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (strPano.Length == 0)
                return;

            if (e.Column == 9)
            {
                if (fEmrJobMiBiStatisticsTewon != null)
                {
                    fEmrJobMiBiStatisticsTewon.SetNewMiBi(strPano);
                    fEmrJobMiBiStatisticsTewon.BringToFront();
                    fEmrJobMiBiStatisticsTewon.Show();
                    return;
                }

                fEmrJobMiBiStatisticsTewon = new frmEmrJobMiBiStatisticsTewon(strPano);
                fEmrJobMiBiStatisticsTewon.StartPosition = FormStartPosition.CenterScreen;
                fEmrJobMiBiStatisticsTewon.FormClosed += FEmrJobMiBiStatisticsTewon_FormClosed;
                fEmrJobMiBiStatisticsTewon.Show(this);
                return;
            }


            rSendPatInfo(SS1_Sheet1.Cells[e.Row, 0].Text.Trim(), SS1_Sheet1.Cells[e.Row, 3].Text.Trim());
        }

        private void FEmrJobMiBiStatisticsTewon_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrJobMiBiStatisticsTewon != null)
            {
                fEmrJobMiBiStatisticsTewon.Dispose();
                fEmrJobMiBiStatisticsTewon = null;
            }
        }

        private void TxtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim().Length == 0)
                    return;

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                lblPatient.Text = GetPatientName(txtPano.Text.Trim());
            }
        }

        private void FrmChartComplete_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrJobMiBiStatisticsTewon != null)
            {
                fEmrJobMiBiStatisticsTewon.Dispose();
                fEmrJobMiBiStatisticsTewon = null;
            }

            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(SS1, e.Column);
            }
        }

        private void cboGubun2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGubun2.SelectedIndex == 1)
            {
                SS1_Sheet1.Columns[9].Visible = true;
                Width = 876;
            }
            else
            {
                SS1_Sheet1.Columns[9].Visible = false;
                Width = 816;
            }
        }
    }
}
