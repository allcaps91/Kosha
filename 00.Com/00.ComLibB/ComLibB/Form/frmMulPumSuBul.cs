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
    /// <summary> 물품별 수불 조회 </summary>
    public partial class frmMulPumSuBul : Form
    {
        int GnQty = 0;
        string GstrJepCode1 = "";

        /// <summary> 물품별 수불 조회 </summary>
        public frmMulPumSuBul()
        {
            InitializeComponent();
        }

        void frmMulPumSuBul_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ScreenClear ();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            try
            {
                int i = 0;
                int j = 0;
                int intBuNo = 0;
                int intIpGo = 0;
                int intOpGo = 0;
                int intIqty = 0;

                int intRow = 0;
                int intIwolQty = 0;

                string strDay = "";
                string strBuse = "";
                string strBuse1 = "";
                string strGubun = "";
                string strJepCode = "";
                string stroldDay = "";

                string SQL = string.Empty;
                string SqlErr = string.Empty;
                DataTable dt = null;
                GnQty = 0;

                Cursor.Current = Cursors.WaitCursor;

                if (txtJepCode.Text == "")
                {
                    ComFunc.MsgBox("물품코드가 입력되지 않았습니다.", "물품코드 입력");
                    Cursor.Current = Cursors.Default;
                    ScreenClear();
                    txtJepCode.Focus();
                    return;
                }

                //strJepCode = UCase(txtJepCode.Text);
                txtJepCode.Text = strJepCode;
                ssView.Enabled = true;
                intIpGo = 0;
                intOpGo = 0;

                btnLastDisplay(strJepCode, strDay, strBuse, intIpGo, intOpGo, intIwolQty, intRow);
                btnIpGoDisplay(strJepCode, intIpGo, intIqty, strDay, intRow, intOpGo, stroldDay, strBuse);

                //CSR_ILBO에서 물품출고량을 조회
                SQL = "";
                SQL = SQL + "SELECT YYMM,BUCODE,JEPCODE,GUBUN, ";
                SQL = SQL + ComNum.VBLF + "  Day01,Day02,Day03,Day04,Day05,Day06,Day07,Day08,Day09,Day10,";
                SQL = SQL + ComNum.VBLF + "  Day11,Day12,Day13,Day14,Day15,Day16,Day17,Day18,Day19,Day20,";
                SQL = SQL + ComNum.VBLF + "  Day21,Day22,Day23,Day24,Day25,Day26,Day27,Day28,Day29,Day30,";
                SQL = SQL + ComNum.VBLF + "  Day31 ";
                SQL = SQL + ComNum.VBLF + " FROM CSR_ILBO ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + dtpDate.Value.ToString("yyyyMM") + "'  ";
                SQL = SQL + ComNum.VBLF + " AND JepCode = '" + strJepCode + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY YYMM,BuCode,JepCode ";
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


                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnView.Enabled = false;
                    btnCancel.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    ssView.Enabled = true;
                    GnQty = 0;
                    return;
                }

                //'일자별로 출고량을 DISPLAY
                for (j = 1; j <= Convert.ToInt32(dtpDate.Value.ToString("dd")); j++)
                {
                    for (i = 0; i < Convert.ToInt32(dtpDate.Value.ToString("dd")); i++)
                    {
                        strGubun = dt.Rows[i]["Gubun"].ToString().Trim();

                        if (strGubun == "1")
                        {
                            strDay = VB.Mid(dt.Rows[i]["YYMM"].ToString().Trim(), 5, 2) + ". " + j;
                            strBuse = dt.Rows[i]["BuCode"].ToString().Trim();
                            intIpGo = Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim()).ToString("00"));
                        }
                        else if (strGubun == "2")
                        {
                            strDay = VB.Mid(dt.Rows[i]["YYMM"].ToString().Trim(), 5, 2) + ". " + j;
                            strBuse = dt.Rows[i]["BuCode"].ToString().Trim();
                            intOpGo = Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim()).ToString("00"));
                        }
                        else if (strGubun == "3")
                        {
                            strDay = VB.Mid(dt.Rows[i]["YYMM"].ToString().Trim(), 5, 2) + ". " + j;
                            strBuse = dt.Rows[i]["BuCode"].ToString().Trim();
                            intOpGo = Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim()).ToString("00"));
                        }

                        if (intIpGo != 0 || intOpGo != 0)
                        {
                            strBuse1 = READBuseName(strBuse);
                            btnDisplay(strGubun, intBuNo, strDay, strBuse1, intIpGo, intIqty, intOpGo, intRow, stroldDay);
                        }
                    }
                }


                btnDisplay(strGubun, intBuNo, strDay, strBuse1, intIpGo, intIqty, intOpGo, intRow, stroldDay);
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                btnView.Enabled = false;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                ssView.Enabled = true;
                GnQty = 0;
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
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/l/fn\"굴림체\" /fz\"20\" ";
            strHead1 = "/n" + "물품별 월간수불부" + "/n";
            strFont2 = "/n/fn\"굴림체\" /fb0/fu0/fz\"11\" ";
            strHead2 = "/c/f1" + "인쇄일자: " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":")) + " /f1/n";
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

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //부서명 READ
        string READBuseName(string strCode)
        {
            try
            {
                string strVal = "";
                string SQL = string.Empty;
                string SqlErr = string.Empty;
                DataTable dt = null;

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return strVal; //권한 확인
                }

                SQL = "";
                SQL = SQL + "SELECT Sname FROM KOSMOS_PMPA.BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE BuCode = '" + VB.Format(strCode, "000000") + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVal;
                }

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }
                else
                {
                    strVal = "";
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                string strVal = "";
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        //출고량을 DISPLAY
        void btnDisplay(string strGubun, int intBuNo, string strDay, string strBuse1, int intIpGo, int intIqty, int intOpGo, int intRow, string stroldDay)
        {
            int j = 0;

            if (VB.Val(strGubun.ToString().Trim()) != 1)
            {
                GnQty = GnQty - intOpGo;
                //intOqty = intOqty + intOpGo;
            }
            else
            {
                GnQty = GnQty + intIpGo;
            }

            if (j == intBuNo + 1)
            {
                strDay = "";
                strBuse1 = "소계";
                intIpGo = intIqty;
                //intOpGo = intOqty;
            }

            if (strDay == stroldDay)
            {
                strDay = "";
            }
            else
            {
                stroldDay = strDay;
            }

            intRow = intRow + 1;

            if (intRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = intRow;
            }

            ssView_Sheet1.Cells[intRow, 0].Text = strDay;
            ssView_Sheet1.Cells[intRow, 1].Text = strBuse1;
            ssView_Sheet1.Cells[intRow, 2].Text = intIpGo.ToString();
            ssView_Sheet1.Cells[intRow, 3].Text = intOpGo.ToString();
            ssView_Sheet1.Cells[intRow, 4].Text = GnQty.ToString();

            if (strDay != "" || strBuse1 == "소계")
            {
                if (strBuse1 == "소계")
                {
                    ssView_Sheet1.Rows.Get(intRow).Border = new FarPoint.Win.LineBorder(Color.Red, 2, false, true, false, true);
                }
                else
                {
                    ssView_Sheet1.Rows.Get(intRow).Border = new FarPoint.Win.LineBorder(Color.Red, 2, false, true, false, false);
                }
            }

            intIpGo = 0;
            intOpGo = 0;

            return;
        }

        void btnIpGoDisplay(string strJepCode, int intIpGo, int intIqty, string strDay, int intRow, int intOpGo, string stroldDay, string strBuse)
        {
            try
            {
                int i = 0;
                int intIpGoQty = 0;
                int intCsrRate = 0;

                string SQL = string.Empty;
                string SqlErr = string.Empty;
                DataTable dt = null;


                //'입고량을 DISPLAY
                SQL = "";
                SQL = SQL + "SELECT A.INDATE,A.GELCODE,A.IPCHGBN,A.SEQNO,A.JEPCODE,A.GUBUN,A.BOXQTY,A.QTY,B.CSRNAME,B.CSRRATE ";
                SQL = SQL + ComNum.VBLF + " FROM ORD_IPCH A,ORD_JEP B  ";
                SQL = SQL + ComNum.VBLF + " WHERE TO_CHAR(A.INDATE,'YYYY-MM') = '" + dtpDate.Value.ToString("yyyy-MM") + "'  ";
                SQL = SQL + ComNum.VBLF + " AND A.JepCode = '" + strJepCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND A.IPCHGBN = '2' AND A.GELCODE = '033107' AND A.JEPCODE = B.JEPCODE(+)  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.INDATE,A.JepCode  ";
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

                for (i = 1; i < dt.Rows.Count; i++)
                {
                    intIpGoQty = Convert.ToInt32(dt.Rows[i]["QTY"].ToString().Trim());
                    intCsrRate = Convert.ToInt32(dt.Rows[i]["CSRRATE"].ToString().Trim());
                    intIpGo = intIpGoQty * intCsrRate;
                    intIqty = intIqty + intIpGo;
                    strDay = VB.Mid(dt.Rows[i]["indate"].ToString().Trim(), 4, 5);
                    GnQty = GnQty + intIpGo;

                    intRow = intRow + 1;

                    if (intRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = intRow;
                    }

                    ssView_Sheet1.Cells[intRow, 0].Text = strDay;
                    ssView_Sheet1.Cells[intRow, 1].Text = "관리과"; //dt.Rows[i]["GELCODE"].ToString().Trim()
                    ssView_Sheet1.Cells[intRow, 2].Text = intIpGo.ToString();
                    ssView_Sheet1.Cells[intRow, 3].Text = intOpGo.ToString();
                    ssView_Sheet1.Cells[intRow, 4].Text = GnQty.ToString();

                    if (strDay != stroldDay)
                    {
                        ssView_Sheet1.Rows.Get(intRow).Border = new FarPoint.Win.LineBorder(Color.Red, 2, false, true, false, false);
                    }

                    stroldDay = strDay;
                    strDay = "";
                    strBuse = "";
                    intIpGo = 0;
                    intOpGo = 0;
                }

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        //전월이월량을 DISPLAY
        void btnLastDisplay(string strJepCode, string strDay, string strBuse, int intIpGo, int intOpGo, int intIwolQty, int intRow)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            try
            {
                int i = 0;
                string SQL = string.Empty;
                string SqlErr = string.Empty;
                DataTable dt = null;

                SQL = "";
                SQL = SQL + "SELECT YEAR,JEPCODE,IWOLQTY,IWOLAMT,IQTY01,CQTY01,IQTY02,CQTY02, ";
                SQL = SQL + ComNum.VBLF + " IQTY03,CQTY03,IQTY04,CQTY04,IQTY05,CQTY05,IQTY06,CQTY06, ";
                SQL = SQL + ComNum.VBLF + " IQTY07,CQTY07,IQTY08,CQTY08,IQTY09,CQTY09,IQTY10,CQTY10, ";
                SQL = SQL + ComNum.VBLF + " IQTY11,CQTY11,IQTY12,CQTY12  ";
                SQL = SQL + ComNum.VBLF + " FROM CSR_SUBUL ";
                SQL = SQL + ComNum.VBLF + " WHERE SUBSTR(YEAR,1,4) = '" + dtpDate.Value.ToString("yyyy") + "'  ";
                SQL = SQL + ComNum.VBLF + " AND JepCode = '" + strJepCode + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY YEAR,JepCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    strDay = "";
                    strBuse = "전월이월";
                    intIpGo = 0;
                    intOpGo = 0;
                    GnQty = 0;
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strDay = "";
                strBuse = "전월이월";
                intIpGo = 0;
                intOpGo = 0;

                intIwolQty = Convert.ToInt32(dt.Rows[0]["IWOLQTY"].ToString().Trim());
                GnQty = GnQty + intIwolQty;

                for (i = 1; i < 13; i++)
                {
                    if (dtpDate.Value.ToString("MM") == VB.Format(i, "00"))
                    {
                        //Exit For
                    }
                    GnQty = GnQty + Convert.ToInt32(VB.Val(dt.Rows[0]["IQTY"].ToString().Trim()).ToString("00"));
                    GnQty = GnQty - Convert.ToInt32(VB.Val(dt.Rows[0]["CQTY"].ToString().Trim()).ToString("00"));
                }

                i = 0;

                intRow = intRow + 1;

                if (intRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = intRow;
                }

                ssView_Sheet1.Cells[intRow, 0].Text = strDay;
                ssView_Sheet1.Cells[intRow, 1].Text = strBuse;
                ssView_Sheet1.Cells[intRow, 2].Text = intIpGo.ToString();
                ssView_Sheet1.Cells[intRow, 3].Text = intOpGo.ToString();
                ssView_Sheet1.Cells[intRow, 4].Text = GnQty.ToString();

                strDay = "";
                strBuse = "";
                intIpGo = 0;
                intOpGo = 0;

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void ScreenClear()
        {
            // FrameDate.Enabled = True
            ssView.Enabled = false;
            btnView.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = true;
            txtJepCode.Text = "";
        }

        void txtJepCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        //TODO: frmCodeView 생성이 안되어 있음
        void txtJepCode_DoubleClick(object sender, EventArgs e)
        {
            //frmCodeView frm = new frmCodeView;
            //frm.Show();
            txtJepCode.Text = GstrJepCode1;

            if (Keys.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}

