using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComNurLibB;

namespace ComNurLibB
{
    public partial class frmNrCodePrint : Form
    {
        clsSpdNr cspd = new clsSpdNr();
        ComFunc cfun = new ComFunc();

        public frmNrCodePrint()
        {
            InitializeComponent();
        }

        void frmNrCodePrint_Load(object sender, EventArgs e)
        {
            // FormInfo_History();
            btnSearch.Enabled = true;
            btnExit.Enabled = true;
            btnPrint.Enabled = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            CellClear();
            btnSearch.Enabled = true;
        }

        void CellClear()
        {
            for (int i = 0; i < ssNrCodePrint_Sheet1.Rows.Count; i++)
            {
                for (int j = 0; j < ssNrCodePrint_Sheet1.Columns.Count; j++)
                {
                    ssNrCodePrint_Sheet1.Cells[i, j].Text = "";
                }
            }
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();           
        }

        void GetData()
        {
            int i = 0, j = 0;
            
            ssNrCodePrint_Sheet1.Rows.Count = 0;

            string strSql = string.Empty;
            DataTable dt = null;

            btnSearch.Enabled = false;
            btnExit.Enabled = true;
            btnPrint.Enabled = true;

            try
            {
                for (i = 1; i < 5; i++)
                {
                    strSql = "";
                    strSql = strSql + ComNum.VBLF + "SELECT";
                    strSql = strSql + ComNum.VBLF + "    Code, Name ";
                    strSql = strSql + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                    strSql = strSql + ComNum.VBLF + " WHERE Gubun= '" + i + "'";
                    strSql = strSql + ComNum.VBLF + " ORDER BY PrintRanking,Code";
                    dt = clsDB.GetDataTable(strSql);
                    ssNrCodePrint_Sheet1.RowCount = dt.Rows.Count + 5;

                    if (dt == null)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (i == 1)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            ssNrCodePrint_Sheet1.Cells[j, 0].Text = dt.Rows[j]["Code"].ToString().Trim();
                            ssNrCodePrint_Sheet1.Cells[j, 1].Text = dt.Rows[j]["Name"].ToString().Trim();
                        }
                    }

                    else if (i == 2)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            ssNrCodePrint_Sheet1.Cells[j, 3].Text = dt.Rows[j]["Code"].ToString().Trim();
                            ssNrCodePrint_Sheet1.Cells[j, 4].Text = dt.Rows[j]["Name"].ToString().Trim();
                        }
                    }

                    else if (i == 3)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            ssNrCodePrint_Sheet1.Cells[j, 6].Text = dt.Rows[j]["Code"].ToString().Trim();
                            ssNrCodePrint_Sheet1.Cells[j, 7].Text = dt.Rows[j]["Name"].ToString().Trim();
                        }
                    }

                    else if (i == 4)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            ssNrCodePrint_Sheet1.Cells[j, 9].Text = dt.Rows[j]["Code"].ToString().Trim();
                            ssNrCodePrint_Sheet1.Cells[j, 10].Text = dt.Rows[j]["Name"].ToString().Trim();
                        }
                    }

                }                  
                
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다.");
            }

            dt.Dispose();
            dt = null;
        }
        
        void btnPrint_Click(object sender, EventArgs e)
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            cfun.Read_SysDate();

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "간호부 기초코드집 인쇄" + "/f1/n";
            strhead[1] = "/n/l/f2" + " " + " /l/f2" + "  출력시간 : " + cfun.strSysDate + " " + cfun.strSysTime + " /n";

            cspd.SPREAD_PRINT(ssNrCodePrint_Sheet1, ssNrCodePrint, strhead, strfont, 10, 10, 2, true);
        }
 
    }
}
