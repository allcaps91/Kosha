using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmPersonJin.cs
    /// Description     : 개인별 진료 내역 조회
    /// Author          : 김효성
    /// Create Date     : 2017-06-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\basic\busanid\OVIEWA04.frm => frmPersonJin.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\OPD\oviewa\OVIEWA04.FRM(FrmPersonJin)
    /// </seealso>
    /// <vbp>
    /// default : VB\OPD\oviewa\oviewa.vbp
    /// </vbp>

    public partial class frmPersonJin : Form
    {
        string GJobPart = "";
        string GPANO = "";
        string dtp;

        public frmPersonJin ()
        {
            InitializeComponent ();
        }

        public frmPersonJin (string strJobPart , string strPANO)
        {
            InitializeComponent ();

            GJobPart = strJobPart;
            GPANO = strPANO;
        }

        string [] strDate = new string [10];
        string [] strDate1 = new string [10];
        string StrPanoNumber = "";
        string [] strDEPTCODE = new string [10];
        string strName = "";
        string strActDate = "";
        string strBDate = "";
        string StrDate2 = "";
        int nRowindi = 0;
        int nChoHap = 0;
        int nGamek = 0;
        int nGein = 0;
        int nTotAmt = 0;
        int nYoungSu = 0;


        private void frmPersonJin_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            dtp = ComFunc.FormatStrToDateEx (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D" , "-");

            pan5.Dock = DockStyle.None;
            pan5.Visible = false;
            txtNamePano.Enabled = false;
            txtPart.Enabled = false;
            dtpyyyy.Enabled = false;
            btnSearch.Enabled = false;
            strDate1 = null;
        }

        private void SSPersonJinClear ()
        {
            ssMainView_Sheet1.Cells [0 , 0 , ssMainView_Sheet1.RowCount - 1 , ssMainView_Sheet1.ColumnCount - 1].Text = "";
            ssMainView_Sheet1.SetActiveCell (0 , 0);
        }

        private void ListPersonJinBuild ()
        {
            int i = 0;
            int j = 0;
            int nReserved = 0;
            int chkFlag = 0;
            int nCheck = 0;
            //string strPano = "";
            //string strSname = "";
            //string StrDate2 = "";
            //string strBDate = "";
            //string strDept = "";
            //string strBi = "";
            //string StrMessage1 = "";
            string strDateCheck = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            nReserved = 0;
            chkFlag = 1;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano, SName, TO_CHAR(ActDate,'YYYY-MM-DD') ADate, DeptCode, Bi, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_MASTER ";

                if (chkSelect1.Checked == true) SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtNamePano.Text + "'  ";

                else if (chkSelect2.Checked == true) SQL = SQL + ComNum.VBLF + " WHERE Sname = '" + txtNamePano.Text + "'  ";

                else if (chkSelect3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + dtpyyyy.Value.ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    if ((txtPart.Text.Trim () != "" && (txtPart.Text.Trim () == null))) SQL = SQL + ComNum.VBLF + " AND Part = '" + txtPart.Text + "' ";
                }

                if (chkSelect3.Checked == true)
                    SQL = SQL + ComNum.VBLF + " ORDER BY Deptcode, ActDate Desc, Pano ";
                else
                    SQL = SQL + ComNum.VBLF + " ORDER BY ActDate Desc, Pano ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                nRowindi = dt.Rows.Count;

                if (dt.Rows.Count == 0)
                {
                    nReserved = 1;
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano, SName, TO_CHAR(Date1, 'YYYY-MM-DD') ADate, DeptCode, Bi ";
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED ";

                    if (chkSelect1.Checked == true) SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtNamePano.Text + "'  ";
                    else if (chkSelect2.Checked == true) SQL = SQL + ComNum.VBLF + " WHERE Sname = '" + txtNamePano.Text + "'  ";
                    else if (chkSelect3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE Date1 = TO_DATE('" + dtpyyyy.Value.ToString ("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        if (txtPart.Text != "" && txtPart.Text.Trim () == null)
                        {
                            SQL = SQL + ComNum.VBLF + " AND Part = '" + txtPart.Text + "' ";
                        }
                    }
                    if (chkSelect3.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY Deptcode, Date1 Desc, Pano ";
                    else SQL = SQL + ComNum.VBLF + " ORDER BY Date1 Desc, Pano ";

                    SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                    nRowindi = dt.Rows.Count;

                    if (dt.Rows.Count == 0)
                    {
                        if (chkSelect1.Checked == true || chkSelect2.Checked == true)
                        {
                            txtNamePano.Focus ();
                        }
                        else if (chkSelect3.Checked == true)
                        {
                            dtpyyyy.Focus ();
                        }
                        return;
                    }
                }

                strDateCheck = "";
                nCheck = 0;
                ssMainView_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssMainView_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssMainView_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["Pano"].ToString ().Trim ();
                        ssMainView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["Sname"].ToString ().Trim ();
                        ssMainView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["ADate"].ToString ().Trim ();

                        if (strDateCheck != VB.Left (ssMainView_Sheet1.Cells [i , 2].ToString ().Trim () , 4))
                        {
                            strDateCheck = VB.Left (ssMainView_Sheet1.Cells [i , 2].ToString ().Trim () , 4);
                            nCheck = nCheck + 1;
                        }
                        if (nCheck % 2 == 0)
                        {
                            ssMainView_Sheet1.Cells [i , 0].BackColor = Color.FromArgb (238 , 232 , 170);
                            ssMainView_Sheet1.Cells [i , 1].BackColor = Color.FromArgb (238 , 232 , 170);
                            ssMainView_Sheet1.Cells [i , 2].BackColor = Color.FromArgb (238 , 232 , 170);
                            ssMainView_Sheet1.Cells [i , 3].BackColor = Color.FromArgb (238 , 232 , 170);
                            ssMainView_Sheet1.Cells [i , 4].BackColor = Color.FromArgb (238 , 232 , 170);
                            ssMainView_Sheet1.Cells [i , 5].BackColor = Color.FromArgb (238 , 232 , 170);
                        }
                        ssMainView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["BDate"].ToString ().Trim ();
                        ssMainView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["DeptCode"].ToString ().Trim ();
                        ssMainView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["Bi"].ToString ().Trim ();
                    }
                }//end if

                #region //FetchForMove
                //for (i = 0; i < dt.Rows.Count; i++)
                //{
                //    StrMessage1 = "";
                //    strPano[i] = dt.Rows [i] ["Pano"].ToString ().Trim ();
                //    strSname[i] = dt.Rows [i] ["Sname"].ToString ().Trim ();
                //    StrDate2[i] = dt.Rows [i] ["ADate"].ToString ().Trim ();
                //    strBDate[i] = dt.Rows [i] ["BDate"].ToString ().Trim ();
                //    strDept[i] = dt.Rows [i] ["DeptCode"].ToString ().Trim ();
                //    strBi[i] = dt.Rows [i] ["Bi"].ToString ().Trim ();
                //    StrMessage1 = "  " + strPano + strSname + StrDate2 + strBDate + strDept + strBi;

                //    Application.DoEvents ();
                //}
                #endregion

                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SSPersonJinBuild ()
        {
            int i = 0;
            int j = 0;
            int nJubsuPay = 0;
            int nReserved = 0;
            int ifSu = 0;
            string strSpc = "";
            string strNgt = "";
            string strGisul = "";
            string strSelf = "";
            string strChild = "";
            string strTot = "";
            string StrDateAct = "";
            string StrMessage = "";
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            DataTable dt = null;

            StrDateAct = strActDate;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(BDate,'YYYY-MM-DD') BalDate, TO_CHAR(EntDate,'YYYY-MM-DD') EDate, ";
            SQL = SQL + ComNum.VBLF + "       Bi, SuCode, A.SuNext, B.SuNameK, BaseAmt, Qty, Nal, Bun, GbSpc, GbNgt, GbGisul, GbSelf, GbChild, ";
            SQL = SQL + ComNum.VBLF + "       Amt1, Amt2, DeptCode, DrCode, SeqNo, Part ";
            SQL = SQL + ComNum.VBLF + "  FROM BAS_SUN B, OPD_SLIP A ";
            SQL = SQL + ComNum.VBLF + " WHERE A.SuNext = B.SuNext ";
            SQL = SQL + ComNum.VBLF + "   AND Pano = '" + StrPanoNumber + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BDate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

            if (chkSelect3.Checked == true)
            {
                if (txtPart.Text != "" && txtPart.Text == "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND Part = '" + txtPart.Text + "' ";
                }
            }

            SQL = SQL + ComNum.VBLF + " ORDER BY a.Entdate,SeqNo, Nu, Bun, SuCode, SuNext ";

            SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose ();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + StrDateAct + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano = '" + StrPanoNumber + "' ";
                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0) nJubsuPay = 0;
                else nJubsuPay = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());

                dt.Dispose ();
                dt = null;

                if (VB.Val (StrDateAct.Replace ("-" , "")) < VB.Val (StrDate2.Replace ("-" , "")))
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_RESERVEDBACKUP ";
                    SQL = SQL + ComNum.VBLF + " WHERE  DATE1 = TO_DATE('" + StrDateAct + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND  Pano = '" + StrPanoNumber + "' ";

                    SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                    if (dt.Rows.Count == 0) nReserved = 0;
                    else nReserved = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());
                }

                else if (VB.Val (StrDateAct.Replace ("-" , "")) == VB.Val (StrDate2.Replace ("-" , "")))
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_RESERVED ";
                    SQL = SQL + ComNum.VBLF + " WHERE DATE1 = '" + StrDateAct + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Pano = '" + StrPanoNumber + "' ";
                    SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                    if (dt.Rows.Count == 0) nReserved = 0;
                    else nReserved = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());

                    dt.Dispose ();
                    dt = null;
                }

                StrMessage = "";
                StrMessage = "1.접수영수금액: " + nJubsuPay.ToString ("###,###,##0") + ComNum.VBLF + ComNum.VBLF;
                StrMessage = StrMessage + "2.예약접수영수금액: " + nReserved.ToString ("###,###,##0") + ComNum.VBLF + ComNum.VBLF;
                StrMessage = StrMessage + "3.수납금액   : SLIP DATA 없음 ";

                ComFunc.MsgBox (StrMessage , "접수 금액 Date");

                return;
            }


            nTotAmt = 0;
            nChoHap = 0;
            nGamek = 0;
            nGein = 0;
            nYoungSu = 0;
            lblTot.Text = "";
            lblCho.Text = "";
            lblGam.Text = "";
            lblGein.Text = "";
            lblYoung.Text = "";

            pan5.Visible = true;
            btnExit.Focus ();
            lblSSViewTitle.Text = StrPanoNumber + "   " + strName + "   ( " + strActDate + " )";

            //ssSubView_Sheet1.re
            ssSubView_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssSubView_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["BalDate"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["EDate"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["Bi"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["SuCode"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["SuNext"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["SuNameK"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["BaseAmt"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["Qty"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["Nal"].ToString ().Trim ();

                strSpc = dt.Rows [i] ["GbSpc"].ToString ().Trim ();
                strNgt = dt.Rows [i] ["GbNgt"].ToString ().Trim ();
                strGisul = dt.Rows [i] ["GbGisul"].ToString ().Trim ();
                strSelf = dt.Rows [i] ["GbSelf"].ToString ().Trim ();
                strChild = dt.Rows [i] ["GbChild"].ToString ().Trim ();

                strTot = strSpc + strNgt + strGisul + strSelf + strChild;

                ssSubView_Sheet1.Cells [i , 9].Text = VB.Format (Convert.ToInt32 (strTot) , "#,###,##0");
                ssSubView_Sheet1.Cells [i , 10].Text = VB.Format (Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim ()) , "#,###,##0");
                ssSubView_Sheet1.Cells [i , 11].Text = VB.Format (Convert.ToInt32 (dt.Rows [i] ["Amt2"].ToString ().Trim ()) , "#,###,##0");
                ssSubView_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["DeptCode"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 13].Text = dt.Rows [i] ["Drcode"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 14].Text = dt.Rows [i] ["SeqNo"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 15].Text = dt.Rows [i] ["Part"].ToString ().Trim ();
                ssSubView_Sheet1.Cells [i , 16].Text = dt.Rows [i] ["Bun"].ToString ().Trim ();

                if (Convert.ToInt32 (dt.Rows [i] ["Bun"].ToString ().Trim ()) < 85)
                    nTotAmt = nTotAmt + Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim () + dt.Rows [i] ["Amt2"].ToString ().Trim ());
                if (Convert.ToInt32 (dt.Rows [i] ["Bun"].ToString ().Trim ()) == 98)
                    nChoHap = nChoHap + Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim () + dt.Rows [i] ["Amt2"].ToString ().Trim ());
                if (Convert.ToInt32 (dt.Rows [i] ["Bun"].ToString ().Trim ()) == 92)
                    nGamek = nGamek + Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim () + dt.Rows [i] ["Amt2"].ToString ().Trim ());
                if (Convert.ToInt32 (dt.Rows [i] ["Bun"].ToString ().Trim ()) == 96)
                    nGein = nGein + Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim () + dt.Rows [i] ["Amt2"].ToString ().Trim ());
                if (Convert.ToInt32 (dt.Rows [i] ["Bun"].ToString ().Trim ()) == 99)
                    nYoungSu = nYoungSu + Convert.ToInt32 (dt.Rows [i] ["Amt1"].ToString ().Trim () + dt.Rows [i] ["Amt2"].ToString ().Trim ());
            }

            dt.Dispose ();
            dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + StrDateAct + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND Pano = '" + StrPanoNumber + "' ";
            SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0) nJubsuPay = 0;
            else nJubsuPay = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());

            dt.Dispose ();
            dt = null;

            if (VB.Val (StrDateAct.Replace ("-" , "")) < VB.Val (StrDate2.Replace ("-" , "")))
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_RESERVEDBACKUP ";
                SQL = SQL + ComNum.VBLF + " WHERE  DATE1 = TO_DATE('" + StrDateAct + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  Pano = '" + StrPanoNumber + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0) nReserved = 0;
                else nReserved = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());

                dt.Dispose ();
                dt = null;
            }
            else if (VB.Val (StrDateAct.Replace ("-" , "")) == VB.Val (StrDate2.Replace ("-" , "")))
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano, Sname, Amt7, Part from OPD_RESERVED ";
                SQL = SQL + ComNum.VBLF + " WHERE  DATE1 = '" + StrDateAct + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  Pano = '" + StrPanoNumber + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0) nReserved = 0;
                else nReserved = Convert.ToInt32 (dt.Rows [0] ["Amt7"].ToString ().Trim ());

                dt.Dispose ();
                dt = null;
            }
            lblTot.Text = VB.Format (Convert.ToInt32 (nTotAmt.ToString ()) , "#,###,##0");
            lblCho.Text = VB.Format (Convert.ToInt32 (nChoHap.ToString ()) , "#,###,##0");
            lblGam.Text = VB.Format (Convert.ToInt32 (nGamek.ToString ()) , "#,###,##0");
            lblGein.Text = VB.Format (Convert.ToInt32 (nGein.ToString ()) , "#,###,##0");
            lblYoung.Text = VB.Format (Convert.ToInt32 (nYoungSu.ToString ()) , "#,###,##0");
            lbllJub.Text = VB.Format (Convert.ToInt32 (nJubsuPay.ToString ()) , "#,###,##0");
            lblReser.Text = VB.Format (Convert.ToInt32 (nReserved.ToString ()) , "#,###,##0");

            txtNamePano.Enabled = false;
            txtPart.Enabled = false;
            dtpyyyy.Enabled = false;
            chkSelect1.Enabled = false;
            chkSelect2.Enabled = false;
            chkSelect3.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void chkSelect1_Click (object sender , EventArgs e)
        {
            if (chkSelect1.Checked == true)
            {
                chkSelect2.Checked = false;
                chkSelect3.Checked = false;
                lblNamePano.Text = "병록번호";
                btnSearch.Enabled = true;
                txtNamePano.Text = GJobPart;
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = true;
                dtpyyyy.Enabled = false;
                txtPart.Enabled = false;
                txtNamePano.Focus ();
            }

        }

        private void chkSelect1_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chkSelect1.Checked = true;
                chkSelect2.Checked = false;
                chkSelect3.Checked = false;
                lblNamePano.Text = "병록번호";
                btnSearch.Enabled = true;
                txtNamePano.Text = "";
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = true;
                dtpyyyy.Enabled = false;
                txtPart.Enabled = false;
                txtNamePano.Focus ();
            }
        }

        private void chkSelect2_Click (object sender , EventArgs e)
        {
            if (chkSelect2.Checked == true)
            {
                chkSelect1.Checked = false;
                chkSelect3.Checked = false;
                lblNamePano.Text = "수진자명";
                btnSearch.Enabled = true;
                txtNamePano.Text = "";
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = true;
                dtpyyyy.Enabled = false;
                txtPart.Enabled = false;
                txtNamePano.Focus ();
            }
        }

        private void chkSelect2_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chkSelect1.Checked = false;
                chkSelect2.Checked = true;
                chkSelect3.Checked = false;
                lblNamePano.Text = "수진자명";
                btnSearch.Enabled = true;
                txtNamePano.Text = "";
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = true;
                dtpyyyy.Enabled = false;
                txtPart.Enabled = false;
                txtNamePano.Focus ();
            }
        }

        private void chkSelect3_Click (object sender , EventArgs e)
        {
            if (chkSelect3.Checked == true)
            {
                chkSelect1.Checked = false;
                chkSelect2.Checked = false;
                lblNamePano.Text = "";
                btnSearch.Enabled = true;
                strDate1 = null;
                txtNamePano.Text = "";
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = false;
                dtpyyyy.Enabled = true;
                dtpyyyy.Value = Convert.ToDateTime (dtp);
                txtPart.Enabled = true;
                txtPart.Text = GJobPart;
                txtPart.Focus ();
            }
        }

        private void chkSelect3_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chkSelect1.Checked = false;
                chkSelect2.Checked = false;
                chkSelect3.Checked = true;
                lblNamePano.Text = "";
                btnSearch.Enabled = true;
                txtNamePano.Text = "";
                dtpyyyy.Text = "";
                txtPart.Text = "";
                txtNamePano.Enabled = false;
                dtpyyyy.Enabled = true;
                //dtp.Text = Date;
                txtPart.Enabled = true;
                txtPart.Text = GJobPart;
                txtPart.Focus ();
            }
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnExit_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNamePano.Enabled = true;
                txtPart.Enabled = true;
                dtpyyyy.Enabled = true;
                chkSelect1.Enabled = true;
                chkSelect2.Enabled = true;
                chkSelect3.Enabled = true;
                SSPersonJinClear ();
                pan5.Visible = false;
            }
        }

        private void btnSearch_Click (object sender , EventArgs e)
        {
            if (txtNamePano.Text == "" || txtNamePano.Text == null && chkSelect3.Checked == false)
            {
                if (chkSelect1.Checked == true)
                    ComFunc.MsgBox ("병록번호가 비어있습니다." , "주의");
                else if (chkSelect2.Checked == true)
                    ComFunc.MsgBox ("수진자명이 비어있습니다." , "주의");
                txtNamePano.Focus ();
            }

            if (chkSelect3.Checked == true && txtPart.Text.Trim () == "")
            {
                ComFunc.MsgBox ("조가 비어있습니다." , "NO Data");
                txtPart.Focus ();
            }

            GPANO = txtNamePano.Text;

            ListPersonJinBuild ();

            if (nRowindi < 0) { }
            else if (chkSelect3.Checked == true) dtpyyyy.Focus ();
            else if (chkSelect1.Checked == true || chkSelect2.Checked == true) txtNamePano.Focus ();

        }

        private void btnSearch_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtNamePano.Text == "" || txtNamePano.Text == null && chkSelect3.Checked == false)
                {
                    if (chkSelect1.Checked == true) ComFunc.MsgBox ("병록번호가 비어있습니다." , "주의");
                    else if (chkSelect2.Checked == true) ComFunc.MsgBox ("수진자명이 비어있습니다." , "주의");
                    txtNamePano.Focus ();
                }

                if (chkSelect3.Checked == true && txtPart.Text.Trim () == "")
                {
                    ComFunc.MsgBox ("조가 비어있습니다." , "NO Data");
                    txtPart.Focus ();
                }
                ListPersonJinBuild ();
            }

            if (nRowindi > 0)
            {
                btnSearch.Enabled = false;
            }
            else if (chkSelect3.Checked == true) dtpyyyy.Focus ();
            else if (chkSelect1.Checked == true || chkSelect2.Checked == true) txtNamePano.Focus ();
        }

        private void dtp_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnSearch.Focus ();
        }

        private void ssMainView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {

            if (ssMainView_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow (ssMainView , e.Column);//정렬
                return;
            }

            //ssMainView_Sheet1.Cells [0 , 0 , ssMainView_Sheet1.RowCount - 1 , ssMainView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;//클릭 시 배경
            //ssMainView_Sheet1.Cells [e.Row , 0 , e.Row , ssMainView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;// 클릭 해제 시 해제

            if (e.Row < 0)
            {
                return;
            }

            StrPanoNumber = ssMainView_Sheet1.Cells [e.Row , 0].Text;
            strName = ssMainView_Sheet1.Cells [e.Row , 1].Text;
            strActDate = ssMainView_Sheet1.Cells [e.Row , 2].Text;
            strBDate = ssMainView_Sheet1.Cells [e.Row , 3].Text;

            SSPersonJinBuild ();
        }

        private void txtNamePano_TextChanged (object sender , EventArgs e)
        {
            ssMainView_Sheet1.RowCount = 0;
        }

        private void txtNamePano_Enter (object sender , EventArgs e)
        {
            if (chkSelect2.Checked == true) txtNamePano.ImeMode = ImeMode.Hangul;

            txtNamePano.SelectionStart = 0;
            txtNamePano.SelectionLength = VB.Len (txtNamePano.Text);
            btnSearch.Enabled = true;
        }

        private void txtNamePano_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send ("{Tab}");
            }
        }

        private void txtNamePano_Leave (object sender , EventArgs e)
        {
            if (chkSelect1.Checked == true) txtNamePano.Text = ComFunc.LPAD (txtNamePano.Text.Trim () , 8 , "0");
            else txtNamePano.ImeMode = ImeMode.Hangul;
        }

        private void txtPart_Enter (object sender , EventArgs e)
        {
            txtPart.SelectionStart = 0;
            txtPart.SelectionLength = VB.Len (txtPart.Text);
            dtpyyyy.Value = Convert.ToDateTime (dtp);
        }

        private void txtPart_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPart.Text = VB.UCase (txtPart.Text);
                dtpyyyy.Focus ();
            }
        }

        private void txtPart_Leave (object sender , EventArgs e)
        {
            txtPart.Text = VB.UCase (txtPart.Text);
        }

        private void button1_Click (object sender , EventArgs e)
        {
            if (chkSelect3.Checked == true)
            {
                txtPart.Enabled = true;
                dtpyyyy.Enabled = true;
            }
            else
            {
                txtNamePano.Enabled = true;
            }

            //SSPersonJinClear ();

            chkSelect1.Enabled = true;
            chkSelect2.Enabled = true;
            chkSelect3.Enabled = true;
            btnSearch.Enabled = true;
            pan5.Visible = false;

        }
    }
}
