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
    /// File Name       : frmPmPaVIEWMisuDaily
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misuper.vbp \MISUP205.FRM (FrmMisuDaily)" >> frmPmPaVIEWBansongPrint.cs 폼이름 재정의" />


    public partial class frmPmPaVIEWMisuDaily : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrPANO = "";

        public frmPmPaVIEWMisuDaily()
        {
            InitializeComponent();
        }

        public frmPmPaVIEWMisuDaily(string strPANO)
        {
            GstrPANO = strPANO;

            InitializeComponent();
        }

        private void frmPmPaVIEWMisuDaily_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(CPM.DATE_ADD(clsDB.DbCon, strDTP, -1));
            dtpTDate.Value = Convert.ToDateTime(strDTP);

        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            clsOrdFunction OF = null;
            if (e.Row == 0 || e.Column == 0)
            {
                return;
            }
            GstrPANO = ssView_Sheet1.Cells[e.Row, 1].Text;

            frmMisuAdd frm = new frmMisuAdd(GstrPANO);

            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(10, 10);
            frm.ShowDialog();

        

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            double nIDno = 0;
            double nTotMisu = 0;
            double nTotIpgum = 0;
            string strMisuDtl = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            ssView_Sheet1.RowCount = 0;


            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT To_Char(Bdate,'YYYY-MM-DD') Bdate, Pano, Gubun1, Gubun2,  Amt, ";
                SQL = SQL + ComNum.VBLF + "        Remark,Idno,MisuDtl,FLAG,                                      ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime                  ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_GAINSLIP                                                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')        ";
                SQL = SQL + ComNum.VBLF + "    AND BDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')        ";

                if (rdosort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Pano,Bdate,Gubun1                                       ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate,Pano,Gubun1                                       ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                nTotMisu = 0;
                nTotIpgum = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count + 1 ;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = CPF.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim()).Rows[0]["Sname"].ToString();

                    if (dt.Rows[i]["Gubun1"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                        nTotMisu = nTotMisu + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                        nTotIpgum = nTotIpgum + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    ssView_Sheet1.Cells[i, 5].Text = CPM.READ_BuseName(dt.Rows[i]["Gubun2"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Remark"].ToString().Trim();

                    if (dt.Rows[i]["Gubun1"].ToString().Trim() == "1")
                    {
                        strMisuDtl = VB.Left(dt.Rows[i]["MisuDtl"].ToString().Trim() + VB.Space(30), 30);
                        ssView_Sheet1.Cells[i, 7].Text = VB.Left(strMisuDtl, 1);
                        ssView_Sheet1.Cells[i, 8].Text = VB.Mid(strMisuDtl, 2, 2);
                        ssView_Sheet1.Cells[i, 9].Text = CPM.READ_PerMisuGye(VB.Mid(strMisuDtl, 4, 2));
                        ssView_Sheet1.Cells[i, 10].Text = VB.Val(VB.Mid(strMisuDtl, 6, 9)).ToString("###,###,###,###");
                        ssView_Sheet1.Cells[i, 11].Text = VB.Mid(strMisuDtl, 15, 8);
                        ssView_Sheet1.Cells[i, 12].Text = VB.Right(strMisuDtl, 8);
                    }
                    if (dt.Rows[i]["FLAG"].ToString().Trim() == "*")
                    {
                        ssView_Sheet1.Cells[i, 13].Text = "수납";
                    }
                   // nIDno = VB.Val(dt.Rows[i]["Idno"].ToString().Trim());


                    ssView_Sheet1.Cells[i, 14].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["Idno"].ToString().Trim());

                }
                dt.Dispose();
                dt = null;

                //ssView_Sheet1.RowCount = ssView_Sheet1.RowCount - 1;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "**합계**";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nTotMisu.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotIpgum.ToString("###,###,###,##0");

                if (nTotMisu > nTotIpgum)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "** 미수가 " + (nTotMisu - nTotIpgum).ToString("###,###,###,##0") + "원이 증가됨 **";
                }
                else if (nTotIpgum > nTotMisu)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "** 미수가 " + (nTotIpgum - nTotMisu).ToString("###,###,###,##0") + "원이 감소됨 **";
                }
                else
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "** 미수가 변동이 없음 **";
                }

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;

            }
            catch (Exception ex)
            {
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

            strTitle = "일자별 미수 발생 명 단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            btnSearch.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
