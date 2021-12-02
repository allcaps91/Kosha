using ComBase;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSnameView.cs
    /// Description     : 수신자 List
    /// Author          : 이현종
    /// Create Date     : 2018-12-14
    /// <history> 
    /// 
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\FrmSnameView.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaViewSnameView : Form
    {
        public frmPmpaViewSnameView()
        {
            InitializeComponent();
        }

        private void frmPmpaViewSnameView_Load(object sender, EventArgs e)
        {
            ss1_Sheet1.Columns[7, 10].Visible = false;

            JengSanView00();
        }

        void JengSanView00()
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine(" SELECT Pano,Sname,Sex,Jumin1,Jumin3,");
                SQL.AppendLine("TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,");
                SQL.AppendLine("TO_CHAR(LastDate,'YYYY-MM-DD') LastDate,JiName,P.ZipCode1,");
                SQL.AppendLine("P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel");
                SQL.AppendLine("FROM BAS_PATIENT P,BAS_AREA A,BAS_ZIPSNEW Z");
                SQL.AppendLine("WHERE P.JiCode = A.JiCode(+)");
                SQL.AppendLine("  AND P.ZipCode1 = Z.ZipCode1(+)");
                SQL.AppendLine("  AND P.ZipCode2 = Z.ZipCode2(+)");
                SQL.AppendLine(clsPmpaPb.GstrFal == "1" ? "AND Sname LIKE '" + clsPmpaPb.GstrName + "%' " : " AND Pano = '" + clsPmpaPb.GstrPANO + "'");
                SQL.AppendLine("GROUP BY Pano, Sname, Sex, Jumin1, Jumin3, startdate, Lastdate,");
                SQL.AppendLine("JiName, P.ZipCode1, P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel");
                SQL.AppendLine("ORDER BY  P.JUMIN1, Sname, Pano");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());

                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["LastDate"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JiName"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Juso"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["StartDate"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[i]["ZipCode2"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ZipName1"].ToString().Trim() + " " + dt.Rows[i]["ZipName2"].ToString().Trim() + " " + dt.Rows[i]["ZipName3"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0)
                return;

            if (e.ColumnHeader == true)
                return;

            clsPmpaPb.GstrPANO = ss1_Sheet1.Cells[e.Row, 0].Text;
            clsPmpaPb.GstrSname = ss1_Sheet1.Cells[e.Row, 1].Text;
            clsPmpaPb.GstrSex = ss1_Sheet1.Cells[e.Row, 2].Text;
            clsPmpaPb.GstrJumin1 = VB.Left(ss1_Sheet1.Cells[e.Row, 3].Text, 6);
            clsPmpaPb.GstrJumin2 = VB.Right(ss1_Sheet1.Cells[e.Row, 3].Text, 7);

            clsPmpaPb.GstrLastDate = ss1_Sheet1.Cells[e.Row, 4].Text;
            clsPmpaPb.GstrJiname = ss1_Sheet1.Cells[e.Row, 5].Text;
            clsPmpaPb.GstrJuso = ss1_Sheet1.Cells[e.Row, 7].Text;
            clsPmpaPb.GstrStartDate = ss1_Sheet1.Cells[e.Row, 8].Text;
            clsPmpaPb.GstrZipCode = ss1_Sheet1.Cells[e.Row, 9].Text;
            clsPmpaPb.GstrJuso = ss1_Sheet1.Cells[e.Row, 10].Text;

            Close();
        }
    }
}
