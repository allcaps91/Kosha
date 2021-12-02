using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
	public partial class frmVolunteerApplicationTong : Form
    {
		public delegate void GetInfo(string strSeqno);
		public event GetInfo rGetPatientInfo;

		//폼이 Close될 경우
		public delegate void EventClosed();
		public event EventClosed rEventClosed;

		public frmVolunteerApplicationTong()
        {
            InitializeComponent();
        }

        private void frmVolunteerApplicationTong_Load(object sender, EventArgs e)
        {
			string strSysDate = "";

			strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

			dtpSDate.Value = Convert.ToDateTime(strSysDate);
			dtpEDate.Value = Convert.ToDateTime(strSysDate);

			GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
			int i = 0;
			DataTable dt = null;
			string SQL = "";    //Query문
			string SqlErr = ""; //에러문 받는 변수

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				SQL = "";
				SQL = "SELECT * FROM KOSMOS_ADM.SOCIAL_VOLUNTEER";

				if (rdoYes.Checked == true)
				{
					SQL = SQL + ComNum.VBLF + " WHERE DELYN = 'Y'";
				}
				else if (rdoNo.Checked == true)
				{
					SQL = SQL + ComNum.VBLF + " WHERE DELYN = 'N'";
				}

				SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO";

				SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

				if (SqlErr != "")
				{
					Cursor.Current = Cursors.Default;
					ComFunc.MsgBox("조회중 문제가 발생했습니다");
					clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
					return;
				}

				if (dt.Rows.Count == 0)
				{
					dt.Dispose();
					dt = null;
					Cursor.Current = Cursors.Default;
					ComFunc.MsgBox("해당 DATA가 없습니다.");
					return;
				}

				ssView_Sheet1.RowCount = dt.Rows.Count;
				ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

				for (i = 0; i < dt.Rows.Count; i++)
				{
					ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
					ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
					ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[i]["JUMIN2"].ToString().Trim();
					ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PHONE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["RELIGION"].ToString().Trim();
					ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SERENAME"].ToString().Trim();
					ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GRADE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["BONDANG"].ToString().Trim();
					ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CHUKDAY"].ToString().Trim();
					ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["BIRTHDAY"].ToString().Trim();
					ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["JUSO"].ToString().Trim();
					ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ACTIVITYDAY"].ToString().Trim();
					ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ACTIVITYTIME"].ToString().Trim();
					ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["VOLUNCAREER"].ToString().Trim();
					ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["VOLUNTEXT"].ToString().Trim();
					ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["VOLUNDONGI"].ToString().Trim();
					ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["INVITEROUTE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 18].Text = dt.Rows[i]["FAMILYNAME"].ToString().Trim();
					ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["FAMILYSERE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["FAMILYREL"].ToString().Trim();
					ssView_Sheet1.Cells[i, 21].Text = dt.Rows[i]["FAMILYAGE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 22].Text = dt.Rows[i]["FAMILYHAK"].ToString().Trim();
					ssView_Sheet1.Cells[i, 23].Text = dt.Rows[i]["FAMILYJOB"].ToString().Trim();
					ssView_Sheet1.Cells[i, 24].Text = dt.Rows[i]["ETCDATA"].ToString().Trim();
					ssView_Sheet1.Cells[i, 25].Text = dt.Rows[i]["APPDATE"].ToString().Trim();
					ssView_Sheet1.Cells[i, 26].Text = dt.Rows[i]["DELYN"].ToString().Trim();
				}

				dt.Dispose();
				dt = null;
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				if (dt != null)
				{
					dt.Dispose();
					dt = null;
				}

				Cursor.Current = Cursors.Default;
				ComFunc.MsgBox(ex.Message);
				clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
			}
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			//if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
			//{
			//	return; //권한 확인
			//}

			string strTitle = "";
			string strHeader = "";
			string strFooter = "";
			bool PrePrint = true;

			clsSpread.SpdPrint_Margin setMargin;
			clsSpread.SpdPrint_Option setOption;
			clsSpread CS = new clsSpread();

			strTitle = "";

			strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
			//strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
			//strHeader += CS.setSpdPrint_String("증빙서류 : " + cboBun.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

			strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
			strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

			setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
			setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

			CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
			CS = null;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
		{
			string strSeqno = "";

			if (ssView_Sheet1.RowCount == 0)
			{
				return;
			}

			if (e.ColumnHeader == true)
			{
				return;
			}

			strSeqno = VB.Format(Convert.ToInt32(ssView_Sheet1.Cells[e.Row, 0].Text.Trim()), "00000000000");

			rGetPatientInfo(strSeqno);
			rEventClosed();
		}
	}
}
