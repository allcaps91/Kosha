using ComLibB;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 신환환자 추천 조회
/// Author : 박병규
/// Create Date : 2017.06.16
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaViewRecommend : Form
    {
        clsUser CU = new clsUser();
        ComQuery CQ = new ComQuery();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        public frmPmpaViewRecommend()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            this.dtpFdate.ValueChanged += new EventHandler(eForm_Clear);
            this.dtpTdate.ValueChanged += new EventHandler(eForm_Clear);

            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.PANO, A.NAME, A.SABUN, A.SNAME, ";
            SQL += ComNum.VBLF + "        A.DEPTCODE, B.NAME AS BUNAME, A.GWANGE, A.REMARK, A.SIGN1SABUN  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SINHOAN A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_BUSE B, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MST C ";
            SQL += ComNum.VBLF + "  WHERE A.BDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.BDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.SABUN = C.SABUN ";
            SQL += ComNum.VBLF + "    AND C.BUSE = B.BUCODE ";
            SQL += ComNum.VBLF + "    AND A.CANDATE IS NULL ";
            SQL += ComNum.VBLF + "  ORDER BY BDATE, PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            String strPtno = string.Empty;

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["BDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["SABUN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["BUNAME"].ToString().Trim();

                switch (Dt.Rows[i]["GWANGE"].ToString().Trim())
                {
                    case "1":
                        ssList_Sheet1.Cells[i, 7].Text = "가족";
                        break;
                    case "2":
                        ssList_Sheet1.Cells[i, 7].Text = "친지";
                        break;
                    case "3":
                        ssList_Sheet1.Cells[i, 7].Text = "지인";
                        break;
                    case "4":
                        ssList_Sheet1.Cells[i, 7].Text = "사업장";
                        break;
                    case "5":
                        ssList_Sheet1.Cells[i, 7].Text = "기타";
                        break;
                }

                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["REMARK"].ToString().Trim();

                if (Dt.Rows[i]["SIGN1SABUN"].ToString().Trim() == "")
                {
                    CS.setSpdForeColor(ssList, i, 0, i, ssList.Sheets[0].RowCount, System.Drawing.Color.Red);
                }

            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "신환환자 추천리스트";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("발생일자 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
    }
}
