using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    public partial class frmAgentSend_NEDIS : Form
    {
        public frmAgentSend_NEDIS()
        {
            InitializeComponent();
        }

        private void frmAgentSend_NEDIS_Load(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                                                
                //'응급실 환자 목록에 존재하는 환자 만 전송
                SQL = " SELECT A.JDATE , A.PANO , A.AGE, A.DEPTCODE, B.SNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM NUR_ER_PATIENT A, BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + " WHERE A.JDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE= 'ER'";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+)  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (VB.IsDate(dt.Rows[i]["JDATE"].ToString().Trim()) == true)
                        {
                            ss1_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToShortDateString();
                        }
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
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

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strJDate = "";
            string strPano = "";
            int nAge = 0;
            string strCode = "";

            try
            {
                
                strJDate = ss1_Sheet1.Cells[e.Row, 0].Text;
                strPano = ss1_Sheet1.Cells[e.Row, 1].Text;
                nAge = (int)VB.Val(ss1_Sheet1.Cells[e.Row, 3].Text);

                //'외래 처방 을 읽어서 처리하도록 검토함
                SQL = " SELECT A.BDATE, A.PANO, A.SUCODE, A.SUNEXT , A.BUN, A.GBNGT, A.GBCHILD, A.GBGISUL, A.GBSELF,  A.QTY,  SUM(A.QTY* A.NAL) qty , B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.BDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE= 'ER'";
                SQL = SQL + ComNum.VBLF + "   AND A.BUN >='28' AND A.BUN <='73'";
                SQL = SQL + ComNum.VBLF + "   AND A.GBGISUL ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.BDATE, A.PANO, A.SUCODE, A.SUNEXT, A.BUN, A.GBNGT, A.GBCHILD, A.GBGISUL, A.GBSELF, A.QTY,  B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(A.QTY * A.NAL) <> 0 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["bdate"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["pano"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["sucode"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BUN"].ToString().Trim() + " " + dt.Rows[i]["sunamek"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBCHILD"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["gbgisul"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["gbSELF"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["QTY"].ToString().Trim();

                        strCode = clsErAgent.READ_NEDISCODE(strPano, strJDate, dt.Rows[i]["BUN"].ToString().Trim(), dt.Rows[i]["SUNEXT"].ToString().Trim(), dt.Rows[i]["GBNGT"].ToString().Trim(), dt.Rows[i]["GBCHILD"].ToString().Trim(), nAge, dt.Rows[i]["gbgisul"].ToString().Trim(), (int)VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                        ss2_Sheet1.Cells[i, 10].Text = strCode;


                        //'MEDIS 코드 여부조회
                        SQL = " SELECT CODE, CODEK  FROM NUR_ER_NEDISCODE ";
                        SQL = SQL + ComNum.VBLF + " WHERE CODE = '" + strCode + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ss2_Sheet1.Cells[i, 11].Text = dt1.Rows[0]["Codek"].ToString().Trim();
                            ss2_Sheet1.Rows[i].BackColor = Color.FromArgb(200, 200, 255);                 
                        }
                        else
                        {
                            ss2_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 100);
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
    }
}
