using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 월별 재고수량 조회 </summary>
    public partial class frmMulPumSuBul1 : Form
    {
        /// <summary> 월별 재고수량 조회 </summary>
        public frmMulPumSuBul1()
        {
            InitializeComponent();
        }

        void frmMulPumSuBul1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ScreenClear ();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //TODO: FrameYear 숨겨져있음/ intRow, strJepCode, strJepName 정의를 못 찾음
        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            try
            {
                string strDay = "";
                string strYear = "";
                string strUnit = "";
                int i = 0;
                int j = 0;
                int intMM = 0;
                int intQtyTot = 0;
                int intAmtTot = 0;
                int intUnitAmt = 0;

                string SQL = string.Empty;
                string SqlErr = string.Empty;
                DataTable dt = null;

                //FrameYear.Enabled = False
                btnView.Enabled = false;
                ssView.Enabled = true;

                ssView_Sheet1.RowCount = 0;

                //CSR_SUBUL에서 수불통계를 조회
                SQL = "";
                SQL = SQL + "SELECT A.YEAR,A.JEPCODE,A.IWOLQTY,A.IWOLAMT,A.IQTY01,A.IAMT01,A.CQTY01,A.CAMT01, ";
                SQL = SQL + ComNum.VBLF + " A.IQTY02,A.IAMT02,A.CQTY02,A.CAMT02,A.IQTY03,A.IAMT03,A.CQTY03,A.CAMT03,A.IQTY04,A.IAMT04,A.CQTY04,A.CAMT04, ";
                SQL = SQL + ComNum.VBLF + " A.IQTY05,A.IAMT05,A.CQTY05,A.CAMT05,A.IQTY06,A.IAMT06,A.CQTY06,A.CAMT06,A.IQTY07,A.IAMT07,A.CQTY07,A.CAMT07, ";
                SQL = SQL + ComNum.VBLF + " A.IQTY08,A.IAMT08,A.CQTY08,A.CAMT08,A.IQTY09,A.IAMT09,A.CQTY09,A.CAMT09,A.IQTY10,A.IAMT10,A.CQTY10,A.CAMT10, ";
                SQL = SQL + ComNum.VBLF + " A.IQTY11,A.IAMT11,A.CQTY11,A.CAMT11,A.IQTY12,A.IAMT12,A.CQTY12,A.CAMT12,B.JEPNAME,B.COVUNIT  ";
                SQL = SQL + ComNum.VBLF + " FROM CSR_SUBUL A, ORD_JEP B  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.JEPCODE = B.JEPCODE(+)  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.Year,A.JepCode  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dtpDate.Value.ToString("yyyy") == VB.Left(dt.Rows[i]["YEAR"].ToString().Trim(), 4))
                    {
                        //strJepCode = ;
                        intQtyTot = Convert.ToInt32(dt.Rows[i]["IWOLQTY"].ToString().Trim());
                        intAmtTot = Convert.ToInt32(dt.Rows[i]["IWOLAMT"].ToString().Trim());

                        for (j = 1; j <= Convert.ToInt32(dtpDate.Value.ToString("MM")); j++)
                        {
                            intQtyTot = intQtyTot + Convert.ToInt32(VB.Val(dt.Rows[i]["IQTY"].ToString().Trim()).ToString("00"));
                            intQtyTot = intQtyTot - Convert.ToInt32(VB.Val(dt.Rows[i]["CQTY"].ToString().Trim()).ToString("00"));
                            intAmtTot = intAmtTot + Convert.ToInt32(VB.Val(dt.Rows[i]["IAMT"].ToString().Trim()).ToString("00"));
                            intAmtTot = intAmtTot - Convert.ToInt32(VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()).ToString("00"));
                        }

                        strUnit = dt.Rows[i]["COVUNIT"].ToString().Trim();

                        if (intAmtTot != 0)
                        {
                            intUnitAmt = intAmtTot / intQtyTot;
                        }
                        else
                        {
                            intUnitAmt = 0;
                        }

                        //strJepName = ;
                        //intRow = intRow + 1;

                        //if (intRow > ssView_Sheet1.RowCount)
                        //{
                        //    ssView_Sheet1.RowCount = intRow;
                        //}

                        //ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                        //ssView_Sheet1.Cells[intRow, 1].Text = intQtyTot.ToString();
                        //ssView_Sheet1.Cells[intRow, 2].Text = strUnit;
                        //ssView_Sheet1.Cells[intRow, 3].Text = intUnitAmt.ToString();
                        //ssView_Sheet1.Cells[intRow, 4].Text = intAmtTot.ToString();
                        //ssView_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["JEPNAME"].ToString().Trim();

                        //strJepCode = "";
                        intQtyTot = 0;
                        strUnit = "";
                        intUnitAmt = 0;
                        intAmtTot = 0;
                        //strJepName = "";
                    }
                }

                btnView.Enabled = false;
                btnCancel.Enabled = true;
                btnExit.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            dtpDate.Focus();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/l/fn\"굴림체\" /fz\"20\" ";
            strHead1 = "/n" + "공급실 월별 수불통계 조회" + "/n";
            strFont2 = "/n/fn\"굴림체\" /fb0/fu0/fz\"11\" ";
            strHead2 = "출고일자: " + dtpDate.Value.ToString("yyyy-MM") + "월분";
            strHead2 = strHead2 + "인쇄일자: " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            strHead2 = strHead2 + "Page : /p  ";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 300;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        //TODO: FrameYear 숨겨져있음
        void ScreenClear()
        {
            ssView_Sheet1.RowCount = 0;
            //FrameYear.Enabled = True
            ssView.Enabled = false;
            btnView.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = true;
        }

        void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
