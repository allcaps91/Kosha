using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmUnPaidPopup : Form
    {
        //string GstrBi = "";
        string GstrSuCode = "";

        public frmUnPaidPopup(string argSuCode)
        {
            InitializeComponent();

            //GstrBi = argBi;
            GstrSuCode = argSuCode;                        
        }

        private void frmUnPaidPopup_Load(object sender, EventArgs e)
        { 
            //51종인 경우 일반수가를 보여주기 위함 
            if(clsOrdFunction.Pat.Bi == "51")
            {
                ssView.ActiveSheet.Columns[3].Visible = false;
            }
            else
            {
                ssView.ActiveSheet.Columns[4].Visible = false;
            }

            GetData();
        }

   
        void GetData()
        {
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (GstrSuCode == "") return;

            try
            {
                ssView_Sheet1.RowCount = 0;
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = " SELECT A.SUGBF, B.SUNEXT, B.SUNAMEK, B.ROWID, A.BAMT, (a.BAMT*1.25) BBAMT,A.IAMT, B.BCODE , A.SUGBE, A.BUN ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT";
                //SQL = SQL + ComNum.VBLF + "    AND B.GBSELFHANG = 'Y'";                
                SQL = SQL + ComNum.VBLF + "    AND ((A.BUN IN ('71','73') AND A.SUGBF = '1') OR B.GBSELFHANG = 'Y')";
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT IN (" + GstrSuCode + ")";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.BUN, B.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "") 
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT + 7); 

                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = READ_BUN_NAME(dt.Rows[i]["BUN"].ToString().Trim());                        
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();  
                        
                        if (dt.Rows[i]["SUGBE"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BBAMT"].ToString().Trim(); //원금액 보험수가 * 25%
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BAMT"].ToString().Trim();  //원금액 보험수가 
                        }

                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IAMT"].ToString().Trim();

                        ssView_Sheet1.Columns[3].BackColor = Color.FromArgb(250, 244, 192);
                        ssView_Sheet1.Columns[4].BackColor = Color.FromArgb(250, 244, 192);
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }

            Cursor.Current = Cursors.Default;
        }

        public string READ_BUN_NAME(string argBun)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + " CODE || '.' || NAME AS NAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND JONG = '1'";
            SQL += ComNum.VBLF + "  AND CODE = '" + argBun + "'";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt2.Rows.Count > 0)
            {
                rtnVal = dt2.Rows[0]["NAME"].ToString().Trim();
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strPrintName1 = "";

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
            }
            catch 
            {
                clsPrint.gSetDefaultPrinter(strPrintName1);
            }

            if (ssView.ActiveSheet.Rows.Count == 0) return;

            int i = 0;

            ss1.ActiveSheet.Cells[0, 9].Value = 0;
            ss1.ActiveSheet.Cells[3, 9].Value = 0;

            ss1.ActiveSheet.Cells[0, 7].Text = clsOrdFunction.Pat.PtNo;
            ss1.ActiveSheet.Cells[2, 7].Text = clsOrdFunction.Pat.sName;
            ss1.ActiveSheet.Cells[4, 7].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, clsOrdFunction.Pat.DeptCode);

            if(clsOrdFunction.Pat.WardCode != "")
            {
                ss1.ActiveSheet.Cells[3, 9].Value = 1;
            }
            else
            {
                ss1.ActiveSheet.Cells[0, 9].Value = 1;
            }

            ss1.ActiveSheet.Cells[20, 6].Text = VB.Left(clsPublic.GstrSysDate, 4) + "년　" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월　" + VB.Right(clsPublic.GstrSysDate, 2) + "일";

            if (ssView.ActiveSheet.Rows.Count > 5)
            {
                ComFunc.MsgBoxEx(this, "비급여항목 5개 초과로 전산실 문의바랍니다. ☎8333");
                this.Close();
                return;
            }

            for (i = 0; i < ssView.ActiveSheet.Rows.Count; i++)
            {
                ss1.ActiveSheet.Cells[11 + i, 0].Text = (i + 1) + "." + "(" + ssView.ActiveSheet.Cells[i, 1].Text + ")" + " " + ssView.ActiveSheet.Cells[i, 2].Text; 

                if(clsOrdFunction.Pat.Bi == "51")
                {
                    ss1.ActiveSheet.Cells[11 + i, 7].Text = ssView.ActiveSheet.Cells[i, 4].Text;
                }
                else
                {
                    ss1.ActiveSheet.Cells[11 + i, 7].Text = ssView.ActiveSheet.Cells[i, 3].Text;
                }                
            }
            

            //ss1_Sheet1.PrintInfo.Printer = "PrimoPDF";
            ss1_Sheet1.PrintInfo.Printer = strPrintName1;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowBorder = false;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = false;
            ss1_Sheet1.PrintInfo.ShowShadows = true;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.Centering = Centering.Both;
            ss1_Sheet1.PrintInfo.PrintType = PrintType.All;

            ss1.PrintSheet(0);

            //this.Close();
            //return;

            ComFunc.Delay(1000);

            this.Close();
            return;
        }
    }
}
