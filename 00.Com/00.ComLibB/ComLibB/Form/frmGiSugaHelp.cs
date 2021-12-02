using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmGiSugaHelp : Form
    {
        string GstrhelpCode = "";
        public frmGiSugaHelp()
        {
            InitializeComponent();
        }

        void frmGiSugaHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optName.Checked = true;

            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            int i, j;

            for(i = 0; i < ssSuga_Sheet1.RowCount; i++)
            {
                for(j = 0; j < ssSuga_Sheet1.ColumnCount; j++)
                {
                    ssSuga_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            GstrhelpCode = "";
            this.Close();
        }

        void btnReView_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtData.Focus();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;

            txtData.Text = txtData.Text.Trim();
            if(txtData.Text == "")
            {
                ComFunc.MsgBox("찾으실 자료가 공란입니다.");
                txtData.Focus();
                return;
            }

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, PNAME, SPEC, JAJIL, UNIT, JEJO, COMPNY,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDATE1,'YYYY-MM-DD') JDATE1, PRICE1,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDATE2,'YYYY-MM-DD') JDATE2, PRICE2,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDATE3,'YYYY-MM-DD') JDATE3, PRICE3,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDATE4,'YYYY-MM-DD') JDATE4, PRICE4,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDATE5,'YYYY-MM-DD') JDATE5, PRICE5";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_GISUGA";
                if(optName.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE PNAME LIKE '%" + txtData.Text.Trim() + "%'  ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PNAME";
                }

                else if(optCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE CODE LIKE '%" + txtData.Text.Trim().ToUpper() + "%'  ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY CODE";
                }

                else if(optCom.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE JEJO LIKE '%" + txtData.Text.Trim().ToUpper() + "%'  ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY JEJO, CODE";
                }

                else if(optOEM.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE COMPNY LIKE '%" + txtData.Text.Trim().ToUpper() + "%'  ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY COMPNY, CODE";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssSuga_Sheet1.Rows.Count = dt.Rows.Count + 20;
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PNAME"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SPEC"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JAJIL"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 4].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JEJO"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 6].Text = dt.Rows[i]["COMPNY"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 7].Text = dt.Rows[i]["JDATE1"].ToString().Trim();

                    ssSuga_Sheet1.Cells[i, 8].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE1"]);
                    ssSuga_Sheet1.Cells[i, 9].Text = dt.Rows[i]["JDATE2"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 10].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE2"]);
                    ssSuga_Sheet1.Cells[i, 11].Text = dt.Rows[i]["JDATE3"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 12].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE3"]);
                    ssSuga_Sheet1.Cells[i, 13].Text = dt.Rows[i]["JDATE4"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 14].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE4"]);
                    ssSuga_Sheet1.Cells[i, 15].Text = dt.Rows[i]["JDATE5"].ToString().Trim();
                    ssSuga_Sheet1.Cells[i, 16].Text = String.Format("{0:#,##0}", dt.Rows[i]["PRICE5"]);
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }
    }
}
