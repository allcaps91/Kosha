using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmMail : Form
    {
        public delegate void SendDataHandler(string SendRetValue);
        public event SendDataHandler SendEvent;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string GstrRetValue = "";
        string GstrJiyek = "";
        int FnRow = 0;

        public frmMail()
        {
            InitializeComponent();
        }
        public frmMail(string SendedRetValue)
        {
            InitializeComponent();
            GstrRetValue = SendedRetValue;
        }

        private void frmMail_Load(object sender, EventArgs e)

        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            //Call FormInfo_History(Me.Name, Me.Caption)
            txtData.Text = "";
            GstrRetValue = "";

            ssMail_Sheet1.RowCount = 0;

            optDong.Checked = true;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            if(ssMail_Sheet1.RowCount > 0)
            {
                ssMail_Sheet1.RowCount = 20;
                SS_Clear(ssMail_Sheet1);
            }

            txtData.Text = txtData.Text.Trim();

            if(txtData.Text == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM";
                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_MAILNEW";
                if(optDong.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE MailDong LIKE '%" + txtData.Text.Trim() + "%'";
                }

                else if(optNum.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE MailCode LIKE '" + txtData.Text.Trim() + "%'";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE MailJuso LIKE '%" + txtData.Text.Trim() + "%'";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count == 1)
                {
                    GstrRetValue = ComFunc.SetAutoZero(dt.Rows[0]["MailCode"].ToString().Trim(), 6);
                    GstrRetValue = GstrRetValue + ComFunc.SetAutoZero(dt.Rows[0]["MailJiyek"].ToString().Trim(), 2);
                    GstrRetValue = GstrRetValue + dt.Rows[0]["MailJuso"].ToString().Trim();
                    SendEvent(GstrRetValue);
                    dt.Dispose();
                    dt = null;

                    rEventClosed();
                    return;
                }

                else if(dt.Rows.Count > 0)
                {
                    ssMail_Sheet1.RowCount = dt.Rows.Count;
                    for(i = 0; i < ssMail_Sheet1.RowCount; i++)
                    {
                        ssMail_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["MailCode"].ToString().Trim(), 3) + "-" + VB.Right(dt.Rows[i]["MailCode"].ToString().Trim(), 3);
                        ssMail_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MailJuso"].ToString().Trim();
                        ssMail_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MailDong"].ToString().Trim();
                        ssMail_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MailJiyek"].ToString().Trim();
                    }
                }

                ssMail.Focus();
                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for(int i = 0; i < Spd.RowCount; i++)
            {
                for(int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        void ssMail_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strCode = "";
            string strJiyek = "";
            string strJuso = "";

            strJuso = ssMail_Sheet1.Cells[e.Row, 1].Text;
            if(strJuso == "")
            {
                ComFunc.MsgBox("주소가 공란입니다.");
                return;
            }

            strCode = VB.Left(ssMail_Sheet1.Cells[e.Row, 0].Text.Trim(), 3) + VB.Right(ssMail_Sheet1.Cells[e.Row, 0].Text.Trim(), 3);
            strJiyek = ComFunc.SetAutoZero(ssMail_Sheet1.Cells[e.Row, 3].Text.Trim(), 2);

            //우편번호 Help시 Return값
            //우편번호1(3) + 우편번호2(3) + 지역코드(2) + 주소
            GstrRetValue = strCode + strJiyek + strJuso;
            SendEvent(GstrRetValue);
            GstrJiyek = strJiyek;
            rEventClosed();
        }

        void ssMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCode = "";
            string strJiyek = "";
            string strJuso = "";

            if(e.KeyChar != 13)
            {
                return;
            }

            strJuso = ssMail_Sheet1.Cells[FnRow, 1].Text;
            if(strJuso == "")
            {
                ComFunc.MsgBox("주소가 공란입니다.");
                return;
            }

            strCode = VB.Left(ssMail_Sheet1.Cells[FnRow, 0].Text, 3) + VB.Right(ssMail_Sheet1.Cells[FnRow, 0].Text, 3);
            strJiyek = ssMail_Sheet1.Cells[FnRow, 3].Text;

            //우편번호 Help시 Return값
            //우편번호1(3) + 우편번호2(3) + 지역코드(2) + 주소
            GstrRetValue = strCode + strJiyek + strJuso;
            SendEvent(GstrRetValue);
            rEventClosed();
        }

        void ssMail_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            FnRow = e.Row;
        }

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(txtData.Text != "")
                {
                    GetData();
                }
            }
        }
    }
}
