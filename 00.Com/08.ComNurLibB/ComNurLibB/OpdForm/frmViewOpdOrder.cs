using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmViewOpdOrder : Form
    {
        string gPano = "";
        string gDate = "";
        //string gSelDate = "";
        //string gSelGubun = "";
        bool gViewTitle = false;
        bool gViewHead = false;
        bool gExit = false;

        public frmViewOpdOrder()
        {
            InitializeComponent();
        }

        public frmViewOpdOrder(string argPano, string argDate, bool bViewTitle = true, bool bViewHead = true, bool bExit = true)
        {
            InitializeComponent();

            gPano = argPano;
            gDate = argDate;
            gViewTitle = bViewTitle;
            gViewHead = bViewHead;
            gExit = bExit;            
        }

        private void frmViewOpdOrder_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            fn_Spread_filter();
            fn_Order_Read_List_SS();
        }


        private void fn_Spread_filter()
        {
            clsSpread CS = new clsSpread();
                        
            CS.setSpdFilter(ssOrder, 1, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssOrder, 2, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssOrder, 16, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssOrder, 17, AutoFilterMode.EnhancedContextMenu, true);

            CS = null;
        }

        public void display_pano_view(string argPano, string argDate)
        {
            ssOrder_Sheet1.RowCount = 0;

            if (argPano != "")
            {
                gPano = argPano;
                gDate = argDate;
                fn_Order_Read_List_SS();
            }
        }

        private void fn_Order_Read_List_SS()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strUnit = "";
            //string strDept = "";
            
            ssOrder_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL += " SELECT OrderName,   DispHeader, C.DosName,  A.RealQty, a.deptcode, a.drcode            \r";
                SQL += "      , A.Nal,       A.GbDiv,    A.Remark,   B.DispRGB                                  \r";
                SQL += "      , A.OrderCode, A.Seqno,    OrderNameS, A.GbInfo                                   \r";
                SQL += "      , B.GbBoth,    A.Slipno,   A.DosCode,  a.GbTax                                    \r";
                SQL += "      , A.Gbboth     JUSA,       A.ROWID , a.GBSUNAP, D.NAL JNAL ,  D.ACTNAL  JACTNAL   \r";
                SQL += "      , a.SuCode,    a.BUN,      a.GBSELF                                               \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER    A                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ODOSAGE   C                                                      \r";
                SQL += "      , KOSMOS_OCS.ETC_JUSAMST   D                                                      \r";
                SQL += "  WHERE A.Ptno        = '" + gPano + "'                                          \r";
                SQL += "    AND A.BDate       = TO_DATE('" + gDate + "','YYYY-MM-DD')                          \r";
                //SQL += "    AND A.DeptCode    = '" + strDept.Trim() + "'                                        \r";
                SQL += "    AND A.GBSUNAP <> '2'                                                                \r";
                SQL += "    AND A.Seqno     > 0                                                                 \r";
                SQL += "    AND A.OrderCode = B.OrderCode(+)                                                    \r";
                SQL += "    AND A.Slipno    = B.Slipno(+)                                                       \r";
                SQL += "    AND A.DosCode   = C.DosCode(+)                                                      \r";
                SQL += "    AND A.ORDERNO   = D.ORDERNO(+)                                                      \r";
                SQL += "  ORDER BY A.GBSUNAP DESC, a.drcode, 10                                                 \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssOrder.ActiveSheet.RowCount = dt.Rows.Count;
                    ssOrder.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (dt.Rows[i]["SLIPNO"].ToString() >= "A1" && dt.Rows[i]["SLIPNO"].ToString() <= "A4")
                        if (string.Compare(dt.Rows[i]["SLIPNO"].ToString(), "A1") >= 0 &&
                            string.Compare(dt.Rows[i]["SLIPNO"].ToString(), "A4") <= 0)
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                            if (ssOrder.ActiveSheet.Cells[i, 7].Text == "#")
                            {
                                ssOrder.ActiveSheet.Cells[i, 7].Text = "";
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 1].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBBOTH"].ToString().Trim() == "1" && (dt.Rows[i]["GBINFO"].ToString().Trim() != ""))
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = VB.Left(ssOrder.ActiveSheet.Cells[i, 1].Text, 30) + dt.Rows[i]["GBINFO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["DOSNAME"].ToString().Trim() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                            }
                            else
                            {
                                if (dt.Rows[i]["DOSCODE"].ToString().Trim() != "")
                                {
                                    SQL = "";
                                    SQL += " SELECT SpecName                                                    \r";
                                    SQL += "   FROM KOSMOS_OCS.OCS_OSPECIMAN                                    \r";
                                    SQL += "  WHERE Slipno   = '" + dt.Rows[i]["SLIPNO"].ToString() + "'        \r";
                                    SQL += "    AND SpecCode = '" + dt.Rows[i]["DOSCODE"].ToString() + "'       \r";
                                    SQL += "    AND ROWNUM   = 1                                                \r";
                                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    if (dt1.Rows.Count == 1)
                                    {
                                        ssOrder.ActiveSheet.Cells[0, 2].Text = dt1.Rows[0]["SPECNAME"].ToString();
                                    }
                                    else
                                    {
                                        ssOrder.ActiveSheet.Cells[0, 2].Text = dt.Rows[i]["GBINFO"].ToString();
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }

                            ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["NAL"].ToString().Trim();
                            if (dt.Rows[i]["GBDIV"].ToString() != "0")
                            {
                                ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                            }
                            if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "01")
                            {
                                if (dt.Rows[i]["JUSA"].ToString() == "3")
                                {
                                    ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                }
                            }
                            if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "02")
                            {
                                if (dt.Rows[i]["JUSA"].ToString() == "1")
                                {
                                    ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                }
                            }
                            ssOrder.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString();
                            ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString();

                            if (dt.Rows[i]["DISPRGB"].ToString() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                            }

                            //ssOrder.ActiveSheet.Cells[i, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());

                            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = "★항혈전 " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                                ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 255);
                            }

                            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = "< !> " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            }

                            //if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString(), FstrDeptCode) == "OK")
                            //{
                            //    ssOrder.ActiveSheet.Cells[i, 1].Text = "[재고X] " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            //}

                            ssOrder.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["SUCODE"].ToString();
                            ssOrder.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["BUN"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["GBINFO"].ToString().Trim(); //촬영부위
                        }

                        ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim().PadRight(10) + ssOrder.ActiveSheet.Cells[i, 1].Text;

                        ssOrder.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssOrder.ActiveSheet.Cells[i, 17].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());
                    }

                    ssOrder.ActiveSheet.Cells[0, 1, ssOrder.ActiveSheet.NonEmptyRowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);

                    ////외래간호 요청으로 수납 Line 추가(2018.03.13)
                    //nSunapOrdCnt = fn_SunapOrder_Count(sBDate, sDeptCode, "OPD");

                    //if (nSunapOrdCnt > 0)
                    //{
                    //    ssOrder.ActiveSheet.Cells[nSunapOrdCnt - 1, 1, nSunapOrdCnt - 1, ssOrder.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Red, 1, false, false, false, true);
                    //}

                    //try
                    //{
                    //    ssDiagno.ActiveSheet.RowCount = 0;

                    //    SQL = "";
                    //    SQL += " SELECT A.IllCode, B.ILLNAMEK,a.RO,b.GbVCode,b.GbV252                   \r";
                    //    SQL += "   FROM KOSMOS_OCS.OCS_OILLS A                                          \r";
                    //    SQL += "      , KOSMOS_PMPA.BAS_ILLS B                                          \r";
                    //    SQL += "  WHERE A.Ptno      = '" + txtPano.Text + "'                            \r";
                    //    SQL += "    AND A.BDate     = TO_DATE('" + sBDate + "','YYYY-MM-DD')            \r";
                    //    SQL += "    AND A.DeptCode  = '" + strDept.Trim() + "'                          \r";
                    //    SQL += "    AND A.ILLCODE   = B.ILLCODE(+)                                      \r";
                    //    SQL += "    AND B.ILLCLASS  = '1'                                               \r";
                    //    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                    //    if (SqlErr != "")
                    //    {
                    //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //        return;
                    //    }

                    //    if (dt1.Rows.Count > 0)
                    //    {
                    //        ssDiagno.ActiveSheet.RowCount = dt1.Rows.Count;
                    //        ssDiagno.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    //        for (int i = 0; i < dt1.Rows.Count; i++)
                    //        {
                    //            ssDiagno.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["ILLCODE"].ToString();
                    //            ssDiagno.ActiveSheet.Cells[i, 2].Text = dt1.Rows[i]["ILLNAMEK"].ToString();
                    //            if (dt1.Rows[i]["GBV252"].ToString() == "*")
                    //            {
                    //                ssDiagno.ActiveSheet.Cells[i, 2].Text = "★" + ssDiagno.ActiveSheet.Cells[i, 2].Text;
                    //            }
                    //            ssDiagno.ActiveSheet.Cells[i, 3].Text = dt1.Rows[i]["RO"].ToString();
                    //            ssDiagno.ActiveSheet.Cells[i, 4].Text = dt1.Rows[i]["GBVCODE"].ToString();
                    //        }
                    //    }

                    //    dt1.Dispose();
                    //    dt1 = null;
                    //}
                    //catch (OracleException ex)
                    //{
                    //    ComFunc.MsgBox(ex.Message);
                    //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    //}
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
