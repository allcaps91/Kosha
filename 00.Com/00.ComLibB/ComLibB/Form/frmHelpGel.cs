using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Text;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmHelpGel
    /// File Name : frmHelpGel.cs
    /// Title or Description : 거래처코드조회 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-03
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmHelpGel : Form
    {
        //이벤트를 전달할 경우
        public delegate void EventSendRetGel(string RetGel);
        public event EventSendRetGel rEventSendRetGel;
        public delegate void EventSendGelName(string GelName);
        public event EventSendGelName rEventSendGelName;
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmHelpGel()
        {
            InitializeComponent();
        }

        private void frmHelpGel_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            txtInput.Text = "";
            libGel.Items.Clear();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string cInput = "";
            string strLtd = "";
            string strName = "";
            string strTaxNo = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //GoSub SQL_PREPARE
                switch (optCho0.Checked)
                {
                    case true: cInput = txtInput.Text + "%"; break;
                    case false: cInput = txtInput.Text + "%"; break;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT LtdCode, Name, TaxNo FROM ADMIN.AIS_LTD  ";
                switch (optCho0.Checked)
                {
                    case true:
                        SQL += ComNum.VBLF + " WHERE Name    Like '" + cInput + "' ";
                        SQL += ComNum.VBLF + "   AND (FLAG1 <> 'N' OR FLAG1 is null)";
                        SQL += ComNum.VBLF + " ORDER BY Name  ";
                        break;
                    case false:
                        SQL += ComNum.VBLF + " WHERE LtdCode Like '" + cInput + "' ";
                        SQL += ComNum.VBLF + "   AND (FLAG1 <> 'N' OR FLAG1 is null) ";
                        SQL += ComNum.VBLF + " ORDER BY LtdCode ";
                        break;
                }

                libGel.Items.Clear();

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strLtd = dt.Rows[i]["LtdCode"].ToString().Trim();
                    strName = dt.Rows[i]["Name"].ToString().Trim();
                    strTaxNo = dt.Rows[i]["TaxNo"].ToString().Trim();

                    int penLtd = 15 - Encoding.Default.GetBytes(strLtd).Length;
                    int penName = 40 - Encoding.Default.GetBytes(strName).Length;
                    int penTaxNo = 30 - Encoding.Default.GetBytes(strTaxNo).Length;

                    string strResult = string.Format("{0}{1}{2}", strLtd + "".PadRight(penLtd), strName + "".PadRight(penName), strTaxNo + "".PadRight(penTaxNo));

                    libGel.Items.Add(strResult);
                }
                
                if (libGel.Items.Count != 0)
                {
                    libGel.TopIndex = libGel.Items.Count - dt.Rows.Count;
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void libGel_DoubleClick(object sender, EventArgs e)
        {
            if (libGel.SelectedIndex == -1) { return; }

            rEventSendRetGel(libGel.SelectedItem.ToString().Substring(0, 6));
            rEventSendGelName(libGel.SelectedItem.ToString().Substring(7, 29).Trim());
            rEventClosed();
        }

        private void libGel_Enter(object sender, EventArgs e)
        {
            if (libGel.TopIndex == -1) return;
            if (libGel.Items.Count == 0) return;

            libGel.SelectedIndex = libGel.TopIndex;
        }

        private void libGel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (libGel.SelectedIndex == -1) return;

                rEventSendRetGel(libGel.SelectedItem.ToString().Substring(0, 6));

                rEventClosed();
            }
        }

        private void txtInput_Enter(object sender, EventArgs e)
        {
            txtInput.SelectionStart = 0;
            txtInput.SelectionLength = txtInput.Text.Length;
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            try
            {
                if (optCho1.Checked == true)
                    txtInput.Text = String.Format("{0:000000}", Int32.Parse(txtInput.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }
    }
}
