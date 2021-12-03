using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmMasterSearch : Form, MainFormMessage
    {
        #region //MainFormMessage
        //string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        int nSeq = 0;
        string strInDate = "";
        string strOptSql = "";
        string[] strBis = new string[99];

        public frmMasterSearch()
        {
            InitializeComponent();
        }

        public frmMasterSearch(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        //public frmMasterSearch(MainFormMessage pform, string sPara1)
        //{
        //    InitializeComponent();
        //    mCallForm = pform;
        //    mPara1 = sPara1;
        //}

        private void frmMasterSearch_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Load_Bi_IDs();
        }

        void Load_Bi_IDs()
        {
            strBis[11] = "공단"; strBis[21] = "보호1"; strBis[31] = "산재";
            strBis[12] = "직장"; strBis[22] = "보호2"; strBis[32] = "공상";
            strBis[13] = "지역"; strBis[23] = "보호3"; strBis[33] = "산재공상";
            strBis[14] = ""; strBis[24] = "행려"; strBis[34] = "";
            strBis[15] = ""; strBis[25] = ""; strBis[35] = "";

            strBis[41] = "공단180"; strBis[51] = "일반";
            strBis[42] = "직장180"; strBis[52] = "자보";
            strBis[43] = "지역180"; strBis[53] = "계약";
            strBis[44] = "가족계획"; strBis[54] = "미확인";
            strBis[45] = "보험계약"; strBis[55] = "자보일반";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        void SS_Setting()
        {
            ssIpd_Sheet1.RowCount = 0;
            ssIpd_Sheet1.RowCount = 20;
            ssIpd_Sheet1.Cells[0, 0, 19, 10].Text = "";
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SS_Setting();

            txtSName.Text = "";
            txtPName.Text = "";
            txtInDate.Text = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    txtSName.Focus();
                    break;
                case 1:
                    rdoSeq0.Focus();
                    break;
                case 2:
                    txtPName.Focus();
                    break;
                case 3:
                    txtInDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                    txtInDate.Focus();
                    break;
            }
        }

        private void txtInDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtInDate.Text = txtInDate.Text.Trim();
                if (txtInDate.Text.Length == 10)
                {
                    #region Format_Check
                    strInDate = txtInDate.Text.Trim();
                    Patient_Sql();
                    #endregion
                }
                txtInDate.Focus();
                txtInDate.SelectionStart = 0;
                txtInDate.SelectionLength = txtInDate.Text.Length;
            }
        }

        private void txtInDate_Enter(object sender, EventArgs e)
        {
            txtInDate.SelectionStart = 0;
            txtInDate.SelectionLength = txtInDate.Text.Length;
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSName.Text.Trim() != "")
                {
                    Patient_Sql();
                }
                txtSName.SelectionStart = 0;
                txtSName.SelectionLength = txtSName.Text.Length;
            }
        }

        private void txtSName_Enter(object sender, EventArgs e)
        {
            txtSName.ImeMode = ImeMode.Hangul;

            txtSName.SelectionStart = 0;
            txtSName.SelectionLength = txtSName.Text.Length;
        }

        private void txtPName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPName.Text.Trim() != "")
                {
                    Patient_Sql();
                }
                txtPName.SelectionStart = 0;
                txtPName.SelectionLength = txtPName.Text.Length;
            }
        }

        private void txtPName_Enter(object sender, EventArgs e)
        {
            txtPName.ImeMode = ImeMode.Hangul;

            txtPName.SelectionStart = 0;
            txtPName.SelectionLength = txtPName.Text.Length;
        }

        void Patient_Sql()
        {
            SS_Setting();

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strBi = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT WardCode,RoomCode,Pano,Bi,Sname,Pname,Sex,Age,";
                SQL += ComNum.VBLF + " TO_CHAR(InDate, 'yy-mm-dd') InDate,DeptCode,DrName, Amset1, AmSet6 ";
                SQL += ComNum.VBLF + " FROM IPD_NEW_MASTER i,BAS_DOCTOR k ";

                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        SQL += ComNum.VBLF + " WHERE Sname LIKE '%" + txtSName.Text + "%'";
                        break;
                    case 1:
                        SQL += ComNum.VBLF + strOptSql;
                        break;
                    case 2:
                        SQL += ComNum.VBLF + " WHERE Pname LIKE '%" + txtPName.Text + "%'";
                        break;
                    case 3:
                        SQL += ComNum.VBLF + " WHERE InDate >= TO_DATE('" + txtInDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "   AND InDate <  TO_DATE('" + Convert.ToDateTime(txtInDate.Text).AddDays(+1).ToShortDateString() + "','YYYY-MM-DD') ";
                        break;
                }

                SQL += ComNum.VBLF + " AND I.GBSTS IN ('0','2')";
                SQL += ComNum.VBLF + " AND I.OUTDATE IS NULL ";
                SQL += ComNum.VBLF + " AND i.DrCode = k.DrCode";
                SQL += ComNum.VBLF + " ORDER BY Sname";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count < 20)
                {
                    ssIpd_Sheet1.RowCount = 21;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssIpd_Sheet1.RowCount = dt.Rows.Count;
                ssIpd_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();

                    ssIpd_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 3].Text = strBis[(int)VB.Val(strBi)];
                    ssIpd_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Pname"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 6].Text = dt.Rows[i]["sex"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 8].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssIpd_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DrName"].ToString().Trim();

                    switch (dt.Rows[i]["AmSet1"].ToString().Trim())
                    {
                        case "0":
                            ssIpd_Sheet1.Cells[i, 11].Text = "";
                            break;
                        case "1":
                            ssIpd_Sheet1.Cells[i, 11].Text = "퇴원완료";
                            break;
                        case "2":
                            ssIpd_Sheet1.Cells[i, 11].Text = "계산중";
                            break;
                        case "3":
                            ssIpd_Sheet1.Cells[i, 11].Text = "가퇴원";
                            break;
                    }

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void rdoSeq_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = sender as RadioButton;
            nSeq = (int)VB.Val(VB.Right(rdo.Name, 2).Replace("q", ""));

            strOptSql = " WHERE Sname >= '" + rdo.Text + "'";
            if (nSeq < 13)
            {
                strOptSql = strOptSql + " AND   Sname < '";
                strOptSql = strOptSql + tabControl1.TabPages[1].Controls["rdoSeq" + (nSeq + 1)].Text + "'";
            }

            rdo = null;
            Patient_Sql();
        }

        private void rdoSeq_Enter(object sender, EventArgs e)
        {
            RadioButton rdo = sender as RadioButton;
            nSeq = (int)VB.Val(VB.Right(rdo.Name, 2).Replace("q", ""));
            rdo = null;
        }

        private void rdoSeq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RadioButton rdo = sender as RadioButton;
                nSeq = (int)VB.Val(VB.Right(rdo.Name, 2).Replace("q", ""));

                strOptSql = " WHERE Sname >= '" + rdo.Text + "'";
                if (nSeq < 13)
                {
                    strOptSql = strOptSql + " AND   Sname < '";
                    strOptSql = strOptSql + tabControl1.TabPages[1].Controls[0].Controls["rdoSeq" + (nSeq + 1)].Text + "'";
                }

                rdo = null;
                Patient_Sql();
            }
        }

        private void frmMasterSearch_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmMasterSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
