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

namespace ComLibB
{
    public partial class frmSelectJupsuList : Form
    {
        string FstrPANO = "";

        public frmSelectJupsuList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            READ_OPD_MASTER(FstrPANO);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSelectJupsuList_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            FstrPANO = clsPublic.GstrRetValue;

            READ_OPD_MASTER(FstrPANO);

            clsPublic.GstrRetValue = "";
        }

        private void READ_OPD_MASTER(string strPano)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ss99_Sheet1.RowCount = 0;

                SQL = "       SELECT '외래' GBN, PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + " FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + "  WHERE PANO ='" + strPano + "' ";
                SQL = SQL + "   AND BDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + "  GROUP BY PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(BDATE,'YYYY-MM-DD')";                
                SQL = SQL + " UNION ALL ";                
                //'2016-05-26
                SQL = SQL + "  SELECT '입원' GBN, PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(INDATE,'YYYY-MM-DD') BDATE";
                SQL = SQL + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                SQL = SQL + "    WHERE PANO ='" + strPano + "' ";
                SQL = SQL + "     AND (JDate =TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE =TRUNC(SYSDATE) )";
                SQL = SQL + "  GROUP BY PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(INDATE,'YYYY-MM-DD')";
                SQL = SQL + "  ORDER BY PANO,SName";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss99_Sheet1.RowCount = dt.Rows.Count;
                    ss99_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss99_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ss99_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void ss99_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GstrRetValue = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;

            clsPublic.GstrRetValue = ss99_Sheet1.Cells[e.Row, 3].Text + "^^";
            clsPublic.GstrRetValue = clsPublic.GstrRetValue + ss99_Sheet1.Cells[e.Row, 5].Text + "^^";

            this.Close();
        }
    }
}
