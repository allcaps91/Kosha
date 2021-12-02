using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWJicwonmisu.cs
    /// Description     : 조합별 년간통계 조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\misu\misumir.vbp\FrmGelYear(MISUM303.FRM)  >> frmPmPaVIEWJicwonmisu.cs 폼이름 재정의" />	
    /// 

    public partial class frmPmPaGelYearSTS : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu pm = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();
        frmPmpaViewGelSearch frm = null;

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrMiaCode = "";
        string GstrMiaName = "";
        double[,] nTotAmt = new double[14, 7];


        public frmPmPaGelYearSTS(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;

            InitializeComponent();
        }

        public frmPmPaGelYearSTS()
        {
            InitializeComponent();
        }

        private void frmPmPaGelYearSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));

            for (i = 1; i <= 60; i++)
            {
                cboYYMM.Items.Add((nYY).ToString("0000") + "년도");
                nYY = nYY - 1;
            }
            cboYYMM.SelectedIndex = 1;
            txtGelcode.Text = "";
            lblGelName.Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            string strYEAR = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strYEAR = VB.Left(cboYYMM.Text, 4);

            for (i = 1; i <= 13; i++)
            {
                for (j = 1; j <= 6; j++)
                {
                    nTotAmt[i, j] = 0;
                }
            }

            btnView.Enabled = false;
            btnPrint.Enabled = false;

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            Cursor.Current = Cursors.WaitCursor;

            for (i = 1; i <= 12; i++)
            {
                ssView_Sheet1.Cells[i - 1, 0].Text = strYEAR + "." + i.ToString("00") + "월";
            }
            ssView_Sheet1.Cells[12, 0].Text = "합 계";
            try
            {

                //월별로 금액을 누적

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMM,SUM(IwolAmt) cIwolAmt,SUM(MisuAmt) cMisuAmt,  ";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt) cIpgumAmt,SUM(SakAmt) cSakAmt,       ";
                SQL = SQL + ComNum.VBLF + "        SUM(BanAmt+EtcAmt) cEtcAmt,SUM(JanAmt) cJanAmt     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strYEAR + "01'                        ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strYEAR + "12'                        ";
                SQL = SQL + ComNum.VBLF + "    AND GelCode = '" + txtGelcode.Text + "'     ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM                                            ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnView.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnView.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    j = (int)VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2));

                    nTotAmt[j, 1] = nTotAmt[j, 1] + VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[j, 2] = nTotAmt[j, 2] + VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[j, 3] = nTotAmt[j, 3] + VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[j, 4] = nTotAmt[j, 4] + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[j, 5] = nTotAmt[j, 5] + VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                    nTotAmt[j, 6] = nTotAmt[j, 6] + VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());
                    nTotAmt[13, 1] = nTotAmt[13, 1] + VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[13, 2] = nTotAmt[13, 2] + VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[13, 3] = nTotAmt[13, 3] + VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[13, 4] = nTotAmt[13, 4] + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[13, 5] = nTotAmt[13, 5] + VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                    nTotAmt[13, 6] = nTotAmt[13, 6] + VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                for (i = 1; i <= 13; i++)
                {
                    for (j = 1; j <= 6; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, j].Text = nTotAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }
                Cursor.Current = Cursors.Default;

                btnView.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnView.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "조합별 년간통계 조회";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 27, 0);
                ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtGelcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtGelcode.Text = "";
                return;
            }
            else
            {
                lblGelName.Text = CF.Read_MiaName(clsDB.DbCon, txtGelcode.Text, false);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            frm = new frmPmpaViewGelSearch();

            frm.rGetData += frm_rGetData;
            frm.rEventClose += frm_rEventClose;
            frm.Show();

            
        }

        private void frm_rEventClose()
        {
            if (frm != null)
            {
                frm.Dispose();
                frm = null;
            }
        }

        private void frm_rGetData(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;

            if (GstrMiaCode != "")
            {
                txtGelcode.Text = GstrMiaCode.Trim();
                lblGelName.Text = GstrMiaName.Trim();
                GstrMiaCode = "";
                btnView.Focus();
            }

            frm.Dispose();
            frm = null;
        }
    }
}
