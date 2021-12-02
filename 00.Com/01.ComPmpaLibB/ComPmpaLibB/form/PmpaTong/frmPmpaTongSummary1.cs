using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongSummary1.cs
    /// Description     : 보험종류별 조합청구 예상금액
    /// Author          : 박창욱
    /// Create Date     : 2017-11-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 사용하지 않음
    /// </history>
    /// <seealso cref= "\misu\MISUM402.FRM(FrmHistoryView.frm) >> frmPmpaTongSummary1.cs 폼이름 재정의" />	
    public partial class frmPmpaTongSummary1 : Form
    {
        double[,] nTotAmt = new double[9, 7];

        public frmPmpaTongSummary1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRead = 0;
            int nBi = 0;
            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            if (string.Compare(strYYMM, "200201") >= 0)
            {
                ComFunc.MsgBox("이 프로그램은 2001-12-31일까지만 사용이 가능함");
                cboYYMM.Focus();
                return;
            }

            if (string.Compare(strYYMM, "199904") < 0)
            {
                ComFunc.MsgBox("1999년 4월분부터 가능합니다.");
                cboYYMM.Focus();
                return;
            }

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            for (i = 0; i < 9; i++)
            {
                for (k = 0; k < 7; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            try
            {
                //DATA를 DB에서 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Bi, IpdOpd, SUM(JohapAmt) JohapAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(JungAmt1) JungAmt1, SUM(JungAmt2) JungAmt2, SUM(MirAmt) MirAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Summary";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY Bi,IpdOpd";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                
                for (i = 0; i < nRead; i++)
                {
                    if (string.Compare(strYYMM, "200007") >= 0)
                    {
                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "11":
                            case "13":
                            case "32":
                            case "44":
                                nBi = 1;
                                break;
                            case "12":
                                nBi = 1;
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                                nBi = 3;
                                break;
                            case "31":
                                nBi = 5;
                                break;
                            case "52":
                                nBi = 6;
                                break;
                            default:
                                nBi = 0;
                                break;
                        }
                    }
                    else
                    {
                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "11":
                            case "13":
                            case "32":
                            case "44":
                                nBi = 1;
                                break;
                            case "12":
                                nBi = 2;
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                                nBi = 3;
                                break;
                            case "31":
                                nBi = 5;
                                break;
                            case "52":
                                nBi = 6;
                                break;
                            default:
                                nBi = 0;
                                break;
                        }
                    }

                    if (nBi > 0)
                    {
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "0") //외래
                        {
                            nTotAmt[nBi, 1] += VB.Val(dt.Rows[i]["MirAmt"].ToString().Trim());
                            nTotAmt[nBi, 6] += VB.Val(dt.Rows[i]["MirAmt"].ToString().Trim());
                        }
                        else
                        {
                            nTotAmt[nBi, 2] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                            nTotAmt[nBi, 3] += VB.Val(dt.Rows[i]["JungAmt1"].ToString().Trim());
                            nTotAmt[nBi, 4] += VB.Val(dt.Rows[i]["JungAmt2"].ToString().Trim());
                            nTotAmt[nBi, 5] += VB.Val(dt.Rows[i]["MirAmt"].ToString().Trim());
                            nTotAmt[nBi, 6] += VB.Val(dt.Rows[i]["MirAmt"].ToString().Trim());
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                //소계, 누계를 합산, Sheet에 Display
                for (i = 1; i < 7; i++)
                {
                    nTotAmt[4, i] = nTotAmt[1, i] + nTotAmt[2, i] + nTotAmt[3, i];
                    nTotAmt[7, i] = nTotAmt[5, i] + nTotAmt[6, i];
                    nTotAmt[8, i] = nTotAmt[4, i] + nTotAmt[7, i];

                    switch (i)
                    {
                        case 1:
                            ssView_Sheet1.Cells[0, 1].Text = nTotAmt[1, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[1, 1].Text = nTotAmt[2, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[2, 1].Text = nTotAmt[3, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[3, 1].Text = nTotAmt[4, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[5, 1].Text = nTotAmt[5, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[6, 1].Text = nTotAmt[6, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[7, 1].Text = nTotAmt[7, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[9, 1].Text = nTotAmt[8, i].ToString("###,###,###,##0 ");
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            ssView_Sheet1.Cells[0, i+1].Text = nTotAmt[1, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[1, i+1].Text = nTotAmt[2, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[2, i+1].Text = nTotAmt[3, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[3, i+1].Text = nTotAmt[4, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[5, i+1].Text = nTotAmt[5, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[6, i+1].Text = nTotAmt[6, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[7, i+1].Text = nTotAmt[7, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[9, i+1].Text = nTotAmt[8, i].ToString("###,###,###,##0 ");
                            break;
                        case 6:
                            ssView_Sheet1.Cells[0, 8].Text = nTotAmt[1, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[1, 8].Text = nTotAmt[2, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[2, 8].Text = nTotAmt[3, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[3, 8].Text = nTotAmt[4, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[5, 8].Text = nTotAmt[5, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[6, 8].Text = nTotAmt[6, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[7, 8].Text = nTotAmt[7, i].ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[9, 8].Text = nTotAmt[8, i].ToString("###,###,###,##0 ");
                            break;
                        default:
                            break;
                    }
                }

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                ssView.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboYYMM.Text + " 보험종류별 조합청구 예상금액";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작성자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void frmPmpaTongSummary1_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 15, "", "1");
            cboYYMM.SelectedIndex = 1;
        }
    }
}
