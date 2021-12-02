using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Drawing;
using FarPoint.Win.Spread.CellType;
using System.Text;
using System.IO;

namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-30
    /// Update History  : 19-07-19 화면수정
    /// </summary>
    /// <history>  
    /// 정정내역 신청내역에 나오게 수정.
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\FrmEMRCopyList" >> frmEMRCopyList.cs 폼이름 재정의" />

    public partial class frmEMRCopyList : Form
    {
        string GstrPano = string.Empty;
        bool bolSort = false;
        public frmEMRCopyList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GstrPanoX">등록번호 </param>
        public frmEMRCopyList(string GstrPanoX)
        {
            GstrPano = GstrPanoX;

            InitializeComponent();
        }

        private void frmEMRCopyList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //ss1_Sheet1.Columns[3].Visible = clsType.User.BuseCode.Equals("044201");

            ss1_Sheet1.RowCount = 0;
            TxtPANO.Clear();
            lbllName.Text = "";

            dtpDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpEDATE.Value = dtpDATE.Value;

            if (GstrPano.Length > 0)
            {
                TxtPANO.Text = ComFunc.LPAD(GstrPano, 8, "0");
                KeyEnterPation();
                Search();
            }
            
        }



        private void TxtPANO_KeyDown(object sender, KeyEventArgs e)
        {
 
            lbllName.Text = "";

            if (e.KeyCode == Keys.Enter)
            {
                KeyEnterPation();
            }
              
        }

        private void KeyEnterPation()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            TxtPANO.Text = TxtPANO.Text.PadLeft(8, '0');    //0 숫자 포맷형식 8자리 채우기
            lbllName.Text = READ_PatientName(TxtPANO.Text);

            try
            {
                SQL = " SELECT TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, OUTDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + (TxtPANO.Text).Trim() + "'";
                SQL = SQL + ComNum.VBLF + "     AND GBSTS NOT IN ('7','9') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC";

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    dtpDATE.Value = Convert.ToDateTime(dt.Rows[0]["INDATE"].ToString().Trim());
                    dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_PatientName(string ArgPano)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string strVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + ArgPano + "' ";

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search();
        }

        private void Search()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            TextCellType textCell = new TextCellType();

            Cursor.Current = Cursors.WaitCursor;

            ss1_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT A.REQDATE, '텍스트' GUBUN, C.FORMNAME1 FORMNAME,  B.INOUTCLS, B.MEDFRDATE, B.CHARTDATE, B.CHARTTIME, B.USEID, A.USEID USEID2, A.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A, " + ComNum.DB_EMR + "EMRXMLMST B, " + ComNum.DB_EMR + "EMRFORM C";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd")  + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.SCANYN = 'T'";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = C.FORMNO";


                #region 신규 기록지
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT A.REQDATE, '텍스트' GUBUN, C.FORMNAME,  B.INOUTCLS, B.MEDFRDATE, B.CHARTDATE, B.CHARTTIME, B.CHARTUSEID AS USEID, A.USEID USEID2, A.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A, " + ComNum.DB_EMR + "AEMRCHARTMST B, " + ComNum.DB_EMR + "AEMRFORM C";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.SCANYN = 'T'";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = C.FORMNO";
                SQL = SQL + ComNum.VBLF + "   AND B.UPDATENO = C.UPDATENO";
                #endregion


                #region 투약기록지
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT A2.REQDATE, '텍스트' GUBUN, C2.FORMNAME1 FORMNAME,  B2.INOUTCLS, B2.MEDFRDATE, B2.CHARTDATE, B2.CHARTTIME, B2.USEID, A2.USEID USEID2, A2.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A2, " + ComNum.DB_EMR + "EMRXML_TUYAK B2, " + ComNum.DB_EMR + "EMRFORM C2";
                SQL = SQL + ComNum.VBLF + "WHERE A2.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A2.SCANYN = 'T'";
                SQL = SQL + ComNum.VBLF + "   AND A2.EMRNO = B2.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND B2.FORMNO = C2.FORMNO";
                #endregion

                #region 발급후 수정한 차트 내역 추가 (투약 제외)
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT A2.REQDATE, '정정' GUBUN, C2.FORMNAME1 FORMNAME,  B2.INOUTCLS, B2.MEDFRDATE, B2.CHARTDATE, B2.CHARTTIME, B2.USEID, A2.USEID USEID2, A2.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A2, " + ComNum.DB_EMR + "EMRXMLMST_HISTORY B2, " + ComNum.DB_EMR + "EMRFORM C2";
                SQL = SQL + ComNum.VBLF + "WHERE A2.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A2.SCANYN = 'T'";
                SQL = SQL + ComNum.VBLF + "   AND A2.EMRNO = B2.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND B2.FORMNO = C2.FORMNO";
                #endregion

                #region 발급후 수정한 차트 내역 추가 (투약)
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT A2.REQDATE, '정정' GUBUN, C2.FORMNAME1 FORMNAME,  B2.INOUTCLS, B2.MEDFRDATE, B2.CHARTDATE, B2.CHARTTIME, B2.USEID, A2.USEID USEID2, A2.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A2, " + ComNum.DB_EMR + "EMRXMLHISTORY_TUYAK B2, " + ComNum.DB_EMR + "EMRFORM C2";
                SQL = SQL + ComNum.VBLF + "WHERE A2.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A2.SCANYN = 'T'";
                SQL = SQL + ComNum.VBLF + "   AND A2.EMRNO = B2.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND B2.FORMNO = C2.FORMNO";
                #endregion

                #region 발급후 수정한 차트 내역 추가 (신규 기록지)
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT A2.REQDATE, '정정' GUBUN, C2.FORMNAME,  B2.INOUTCLS, B2.MEDFRDATE, B2.CHARTDATE, B2.CHARTTIME, B2.CHARTUSEID, A2.USEID USEID2, A2.PRINTDATE YN, 0 AS PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRPRTREQ A2";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTMSTHIS B2";
                SQL = SQL + ComNum.VBLF + "       ON A2.EMRNO    = B2.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND A2.EMRNOHIS = B2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRFORM C2";
                SQL = SQL + ComNum.VBLF + "       ON C2.FORMNO = B2.FORMNO";
                SQL = SQL + ComNum.VBLF + "      AND C2.UPDATENO = B2.UPDATENO";
                SQL = SQL + ComNum.VBLF + "WHERE A2.PTNO = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A2.REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A2.SCANYN = 'T'";
                #endregion

                #region 스캔
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                SQL = SQL + ComNum.VBLF + " SELECT N.CDATE REQDATE, '스캔' GUBUN,  CODE.NAME FORMNAME, T.CLASS INOUTCLS, T.INDATE MEDFRDATE, C.CDATE CHARTDATE, '' CHARTTIME, '' USEID, N.CUSERID USEID2, N.PRINTED YN, N.PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMR_PRINTNEEDT N,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_CHARTPAGET C,         " + ComNum.DB_EMR + "EMR_TREATT T,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_PATIENTT P,         " + ComNum.DB_EMR + "EMR_USERT U,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_PRINTCODET PC,         " + ComNum.DB_EMR + "EMR_FORMT CODE";
                SQL = SQL + ComNum.VBLF + " WHERE P.PATID = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND N.CDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd")  + "'";
                    SQL = SQL + ComNum.VBLF + "   AND N.CDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND N.PAGENO = C.PAGENO";
                SQL = SQL + ComNum.VBLF + "   AND C.TREATNO = T.TREATNO";
                SQL = SQL + ComNum.VBLF + "   AND T.PATID = P.PATID";
                SQL = SQL + ComNum.VBLF + "   AND N.CUSERID = U.USERID";
                SQL = SQL + ComNum.VBLF + "   AND N.PRINTCODE = PC.CODE";
                SQL = SQL + ComNum.VBLF + "   AND C.FORMCODE = CODE.FORMCODE";
                #endregion

                #region 스캔2
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                SQL = SQL + ComNum.VBLF + " SELECT N.CDATE REQDATE, '스캔(변환)' GUBUN,  CODE.NAME FORMNAME, T.CLASS INOUTCLS, T.INDATE MEDFRDATE, C.CDATE CHARTDATE, '' CHARTTIME, '' USEID, N.CUSERID USEID2, N.PRINTED YN, N.PAGENO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMR_PRINTNEEDT_BACKUP N,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_DELETEPAGET C,         " + ComNum.DB_EMR + "EMR_TREATT T,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_PATIENTT P,         " + ComNum.DB_EMR + "EMR_USERT U,";
                SQL = SQL + ComNum.VBLF + "   " + ComNum.DB_EMR + "EMR_PRINTCODET PC,         " + ComNum.DB_EMR + "EMR_FORMT CODE";
                SQL = SQL + ComNum.VBLF + " WHERE P.PATID = '" + (TxtPANO.Text).Trim() + "' ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND N.CDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND N.CDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND N.PAGENO = C.PAGENO";
                SQL = SQL + ComNum.VBLF + "   AND C.TREATNO = T.TREATNO";
                SQL = SQL + ComNum.VBLF + "   AND T.PATID = P.PATID";
                SQL = SQL + ComNum.VBLF + "   AND N.CUSERID = U.USERID";
                SQL = SQL + ComNum.VBLF + "   AND N.PRINTCODE = PC.CODE";
                SQL = SQL + ComNum.VBLF + "   AND C.FORMCODE = CODE.FORMCODE";
                #endregion

                SQL = SQL + ComNum.VBLF + "ORDER BY REQDATE, FORMNAME, INOUTCLS, MEDFRDATE ";

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    textCell.Static = true;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GUBUN"].ToString().Trim().IndexOf("스캔") == -1)
                        {
                            ss1_Sheet1.Cells[i, 3].CellType = textCell;
                        }

                        ss1_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["REQDATE"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["REQDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["REQDATE"].ToString().Trim(), 2);
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Tag = dt.Rows[i]["pageno"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                        ss1_Sheet1.Cells[i, 6].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D")
                        + " " +
                            ComFunc.FormatStrToDate(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4), "M");

                        if(ss1_Sheet1.Cells[i, 1].Text == "정정")
                        {
                            ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 192, 255);
                        }

                        ss1_Sheet1.Cells[i, 7].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["USEID"].ToString().Trim());
                        ss1_Sheet1.Cells[i, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["USEID2"].ToString().Trim());
                        ss1_Sheet1.Cells[i, 9].Text = (dt.Rows[i]["YN"].ToString().Trim() != "" ? "인쇄" : "");
                    }
                }

                dt.Dispose();
                dt = null;

                #region ORDER지
                SQL = "SELECT REQDATE, PRTOPTION, USEID, PRINTYN";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + (TxtPANO.Text).Trim() + "' ";
                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "   AND REQDATE >= '" + dtpDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND REQDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND REQDATE >= '20010101'";
                }
                SQL = SQL + ComNum.VBLF + "   AND SCANYN = 'O'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        ss1_Sheet1.RowCount += 1;

                        string[] sPara = dt.Rows[i]["PRTOPTION"].ToString().Trim().Split('^');
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].CellType = textCell;

                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = VB.Left(dt.Rows[i]["REQDATE"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["REQDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["REQDATE"].ToString().Trim(), 2);
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = (sPara[2] == "ER" ? "Dr Order(ER)" : "Dr Order");
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 4].Text = sPara[0];//        'PtInOutCls
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 5].Text = ComFunc.FormatStrToDate(sPara[1], "D");    //'PtMedFrDate
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 6].Text = ComFunc.FormatStrToDate(sPara[4], "DM");    //'sEmrNo(1)
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 7].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, sPara[3]);
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["USEID"].ToString().Trim());
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 9].Text = (dt.Rows[i]["PRINTYN"].ToString().Trim() != "" ? "인쇄" : "");
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column, ref bolSort, true);
                return;
            }
            else
            {
                if (e.Column == 3 && e.Button == MouseButtons.Right)
                {
                    LoadScanChartPSMH(ss1_Sheet1.Cells[e.Row, 3].Tag.ToString().Trim());
                }
            }
        }

        #region LoadScanChartPSMH
        /// <summary>
        /// TREATNO로 환자 스캔 정보 가져오는 함수
        /// </summary>
        private void LoadScanChartPSMH(string strPageNo)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (VB.Val(strPageNo) == 0)
            {
                return;
            }

            int i = 0;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                #region 이전
                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                SQL.AppendLine("    CASE  ");
                SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                SQL.AppendLine("        ELSE P.EXTENSION  ");
                SQL.AppendLine("    END AS EXTENSION,  ");
                SQL.AppendLine("    C.SECURITY, P.FILESIZE, P.CDATE, ");
                SQL.AppendLine("    C.FORMCODE, C.UNREADY, C.CDNO, T.CLASS , ");
                SQL.AppendLine("    (SELECT C1.NAME  ");
                SQL.AppendLine("        FROM KOSMOS_EMR.EMR_CLINICT C1  ");
                SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                SQL.AppendLine("FROM KOSMOS_EMR.EMR_PAGET P  ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C ");
                SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                SQL.AppendLine("  AND C.PAGENO = " + VB.Val(strPageNo));
                SQL.AppendLine("  AND C.PAGE > 0 ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_TREATT T ");
                SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_PATHT S ");
                SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                #endregion

                #region 신규
                SQL.AppendLine(" UNION ALL  ");
                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                SQL.AppendLine("    CASE  ");
                SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                SQL.AppendLine("        ELSE P.EXTENSION  ");
                SQL.AppendLine("    END AS EXTENSION,  ");
                SQL.AppendLine("    '' AS SECURITY, P.FILESIZE, P.CDATE, ");
                SQL.AppendLine("    C.FORMCODE, '' AS UNREADY, '' AS CDNO, T.CLASS , ");
                SQL.AppendLine("    (SELECT C1.NAME  ");
                SQL.AppendLine("        FROM KOSMOS_EMR.EMR_CLINICT C1  ");
                SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                SQL.AppendLine("FROM KOSMOS_EMR.EMR_PAGET P  ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_DELETEPAGET C ");
                SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                SQL.AppendLine("  AND C.PAGENO = " + VB.Val(strPageNo));
                SQL.AppendLine("  AND C.PAGE > 0 ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_TREATT T ");
                SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_PATHT S ");
                SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                string strSVRFILEPATH = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                string strEXTENSION = dt.Rows[i]["EXTENSION"].ToString().Trim();
                string strFTPUSER = dt.Rows[i]["FTPUSER"].ToString().Trim();
                string strFTPPASSWD = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                string strLOCALPATH = dt.Rows[i]["LOCALPATH"].ToString().Trim();

                string strSVRIP = dt.Rows[i]["IPADDRESS"].ToString().Trim();
                string strSVRID = dt.Rows[i]["FTPUSER"].ToString().Trim();
                string strSVRPW = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                string strFileNm = dt.Rows[i]["PAGENO"].ToString().Trim() + "." + dt.Rows[i]["EXTENSION"].ToString().Trim();
                string strSvrPath = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                strSvrPath = strSvrPath.Replace("\\", "/");

                Cursor.Current = Cursors.Default;
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                string mstrViewPathInit = @"C:\PSMHEXE\ScanTmp\Formname";
                string mstrViewPath = mstrViewPathInit + "\\" + strCurDate;

                if (Directory.Exists(mstrViewPath) == false)
                {
                    Directory.CreateDirectory(mstrViewPath);
                }

                #region FILE DOWNLOAD => VIEW
                using (Ftpedt FtpedtX = new Ftpedt())
                {
                    FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx(strSVRIP, strSVRID, strSVRPW);
                    if (FtpedtX.FtpConBatchEx == null)
                    {
                        FtpedtX.Dispose();
                        return;
                    }

                    bool blnDown = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx, mstrViewPath + "\\" + strFileNm, strFileNm, strSvrPath); //파일다운로드
                    if (blnDown)
                    {
                        //ltkPageView.Visible = true;
                        //ltkPageView.BestFit();
                        //if (ltkPageView.Load(mstrViewPath + "\\" + strFileNm, 1))
                        //{
                        //    MoveImgView();
                        //}

                        //ltkPageView.Visible = true;
                        MoveImgView(mstrViewPath + "\\" + strFileNm);
                    }
                }


                #endregion

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 이미지 중앙 이동
        /// </summary>
        void MoveImgView(string strImagePath)
        {
            //ltkPageView.BestFit();

            //double horzRatio = (double)Width / ltkPageView.ImageWidth;
            //double vertRatio = (double) Height / ltkPageView.ImageHeight;

            //double ratio = horzRatio < vertRatio ? horzRatio : vertRatio;

            //ltkPageView.Width = (int)(ltkPageView.ImageWidth * ratio);
            //ltkPageView.Height = (int)(ltkPageView.ImageHeight * ratio) - 100;

            //ltkPageView.Left =  (Width / 2)  - (ltkPageView.Width / 2);
            //ltkPageView.Top  =  (Height / 2) - (ltkPageView.Height / 2);

            Image BackImage = clsCyper.DecryptImage(strImagePath);


            //using (FileStream stream = new FileStream(strImagePath, FileMode.Open, FileAccess.Read))
            //{
            //    using (BinaryReader reader = new BinaryReader(stream))
            //    {
            //        using (var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length)))
            //        {
            //            BackImage = new Bitmap(memoryStream);
            //        }
            //    }
            //}

            double horzRatio = (double)((double)Width / (double)BackImage.Width);
            double vertRatio = (double)((double)(Height - 60) / (double)BackImage.Height);
            double ratio = (double)(horzRatio < vertRatio ? horzRatio : vertRatio);

            picBig1.Width = (int)(BackImage.Width * ratio);
            picBig1.Height = (int)(BackImage.Height * ratio) - 100;

            picBig1.Left = (Width / 2) - (picBig1.Width / 2);
            picBig1.Top = (Height / 2) - (picBig1.Height / 2);

            picBig1.Image = (Image)BackImage;
            picBig1.SizeMode = PictureBoxSizeMode.StretchImage;
            picBig1.Visible = true;
            picBig1.BringToFront();
        }
        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "복사신청 내역조회";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            strHeader += CS.setSpdPrint_String("   환자번호 : " + TxtPANO.Text.Trim() + "(" + lbllName.Text.Trim() + ")", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.90f);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void ss1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0)
                return;

            if (ss1_Sheet1.Cells[e.Row, 1].Text.Trim().IndexOf("스캔") == -1)
            {
                return;
            }


            if (e.Column == 3)
            {
                string strPageNo = ss1_Sheet1.Cells[e.Row, 3].Tag.ToString().Trim();

                using (frmCopyScanView frm = new frmCopyScanView(strPageNo))
                {
                    Screen screen = Screen.FromControl(this);
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(screen.WorkingArea.Left, screen.WorkingArea.Top);
                    frm.WindowState = FormWindowState.Maximized;
                    frm.ShowDialog(this);
                }
            }
        }

        private void ss1_MouseUp(object sender, MouseEventArgs e)
        {
            picBig1.Visible = false;
            //ltkPageView.Visible = false;
        }

        private void picBig1_Click(object sender, EventArgs e)
        {
            picBig1.Visible = false;
        }
    }
}
