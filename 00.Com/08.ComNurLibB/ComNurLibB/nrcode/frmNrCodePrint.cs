using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComLibB;
using ComNurLibB;

namespace ComNurLibB
{
    public partial class frmNrCodePrint : Form
    {
        clsSpdNr CS = new clsSpdNr();
        ComFunc CF = new ComFunc();

        public frmNrCodePrint()
        {
            InitializeComponent();
        }

        void frmNrCodePrint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                        
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
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssNrCodePrint_Sheet1.Rows.Count = 0;
            
            btnSearch.Enabled = false;
            btnExit.Enabled = true;
            btnPrint.Enabled = true;

            try
            {
                for (i = 1; i < 5; i++)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "    Code, Name ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                    SQL = SQL + ComNum.VBLF + " WHERE Gubun= '" + i + "'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking,Code";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssNrCodePrint_Sheet1.RowCount = dt.Rows.Count + 5;

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
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        void btnPrint_Click(object sender, EventArgs e)
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            ComFunc.ReadSysDate(clsDB.DbCon);

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "간호부 기초코드집 인쇄" + "/f1/n";
            strhead[1] = "/n/l/f2" + " " + " /l/f2" + "  출력시간 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " /n";

            CS.SPREAD_PRINT(ssNrCodePrint_Sheet1, ssNrCodePrint, strhead, strfont, 10, 10, 2, true);
        }
 
    }
}
