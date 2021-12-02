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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmMailReg.cs
    /// Description     : 우편번호 등록하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\BuCode19.frm(frmMail) => frmMailReg.cs 으로 변경함
    /// cvtToEng, cvtToHan 구현 필요
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\BuCode19.frm(frmMail)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmMailReg : Form, MainFormMessage
    {
        #region MainFormMessage InterFace

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

        #endregion

        public frmMailReg(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;            
        }

        public frmMailReg()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmMailReg_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optView0.Checked = true;

            SetCombo();

            Screen_clear();

            txtMail.Select();

        }

        void SetCombo()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  JiCode, JiName, ZipCode ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_Area";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboJiyek.Items.Add(ComFunc.SetAutoZero(dt.Rows[i]["Jicode"].ToString().Trim(), 2) + "." + dt.Rows[i]["JiName"].ToString().Trim());
            }

            cboJiyek.SelectedIndex = 0;

            dt.Dispose();
            dt = null;

        }

        void Screen_clear()
        {
            txtMail.Text = "";
            txtDong.Text = "";
            txtJuso.Text = "";
            txtViewCode.Text = "";
            
            btnReg.Enabled = false;
            btnCancel.Enabled = false;
            btnDel.Enabled = false;

            ssJuso_Sheet1.RowCount = 0;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_clear();
            txtMail.Focus();
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();           
        }

        void DelData()
        {
            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAIL";
                SQL += ComNum.VBLF + "WHERE MailCode = '" + txtMail.Text.Trim() + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;

                Screen_clear();
                txtMail.Focus();
            }
        }

        void btnReg_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            RegData();
        }

        void RegData()
        {
            string strROWID = "";

            DataTable dt = null;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAIL";
                SQL += ComNum.VBLF + "WHERE MailCode = '" + txtMail.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strROWID = "";

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_MAIL ";
                    SQL += ComNum.VBLF + "(MailCode, MailDong, MailJuso, MailJiyek)";
                    SQL += ComNum.VBLF + "VALUES( ";
                    SQL += ComNum.VBLF + "'" + txtMail.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "'" + txtDong.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "'" + txtJuso.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "" + Convert.ToInt16(VB.Left(cboJiyek.SelectedItem.ToString(), 2)) + ")";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_MAIL SET ";
                    SQL += ComNum.VBLF + "MailCode = '" + txtMail.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "MailDong = '" + txtDong.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "MailJuso = '" + txtJuso.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "MailJiyek = " + Convert.ToInt16(VB.Left(cboJiyek.SelectedItem.ToString(), 2)) + " ";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        void btnView_Click(object sender, EventArgs e)
        {            
            GetData();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strJiyek = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "*";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Mail";
                SQL += ComNum.VBLF + "WHERE MAILCODE = '" + txtMail.Text.Trim() + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    txtDong.Text = dt.Rows[0]["MailDong"].ToString().Trim();
                    txtJuso.Text = dt.Rows[0]["MailJuso"].ToString().Trim();
                    strJiyek = ComFunc.SetAutoZero(dt.Rows[0]["MailJiyek"].ToString().Trim(), 2);

                    foreach (string item in cboJiyek.Items)
                    {
                        if (strJiyek == VB.Left(item.ToString(), 2))
                        {
                            cboJiyek.Text = item.ToString();
                            break;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    btnReg.Enabled = true;
                    btnCancel.Enabled = true;
                    btnDel.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {            
            DataSearch();
        }

        void DataSearch()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Mailcode, MailDong, MailJuso, MailJiyek";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAIL";
                if (optView0.Checked == true)
                {
                    SQL += ComNum.VBLF + "WHERE MailDong LIKE '%" + txtViewCode.Text.Trim() + "%'";
                }
                else
                {
                    SQL += ComNum.VBLF + "WHERE MailCode LIKE '%" + txtViewCode.Text.Trim() + "%'";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssJuso_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < ssJuso_Sheet1.RowCount; i++)
                {
                    ssJuso_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Mailcode"].ToString().Trim();
                    ssJuso_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MailJuso"].ToString().Trim();
                    ssJuso_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MailDong"].ToString().Trim();
                    ssJuso_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MailJiyek"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void cboJiyek_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void ssJuso_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtMail.Text = ssJuso_Sheet1.Cells[e.Row, 0].Text;
            GetData();
            txtDong.Focus();
        }

        void txtJuso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtDong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
                txtDong.Focus();
                GetData();
            }
        }

        void txtViewCode_Click(object sender, EventArgs e)
        {
            txtViewCode.ImeMode = ImeMode.Hangul;
        }

        void txtViewCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //txtViewCode.ImeMode = ImeMode.Hangul;
                //SendKeys.Send("{TAB}");
                //btnSearch.Focus();
                txtDong.Focus();
                DataSearch();
            }
        }

        void txtViewCode_Leave(object sender, EventArgs e)
        {
            txtViewCode.ImeMode = ImeMode.Alpha;
        }
    }
}
