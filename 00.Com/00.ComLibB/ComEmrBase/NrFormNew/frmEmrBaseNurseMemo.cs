using System;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using ComBase;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrBaseNurseMemo : Form
    {
        /// <summary>
        /// 환자정보
        /// </summary>
        EmrPatient AcpEmr = null;

        RichTextBox ActiveRich = null;


        /// <summary>
        /// 이것만 사용 가능.
        /// </summary>
        /// <param name="pAcp"></param>
        public frmEmrBaseNurseMemo(EmrPatient pAcp)
        {
            AcpEmr = pAcp;
            InitializeComponent();
        }

        private void frmEmrBaseNurseMemo_Load(object sender, EventArgs e)
        {
            Text = string.Format("등록번호 :{0} 이름 :{1}", AcpEmr.ptNo, AcpEmr.ptName);

            cboSize.Items.Clear();
            for (int i = 9; i < 31; i++)
            {
                cboSize.Items.Add(i);
            }

            cboSize.Text = "10";

            foreach (FontFamily fontFamily in  new InstalledFontCollection().Families)
            {
                cboFont.Items.Add(fontFamily.Name);
            }

            cboFont.Text = SystemFonts.DefaultFont.Name;

            dtpBdate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            GetMemo();
        }

        private void mbtnBeforeTot_Click(object sender, EventArgs e)
        {
            dtpBdate.Value = dtpBdate.Value.AddDays(-1);
        }

        private void mbtnNextTot_Click(object sender, EventArgs e)
        {
            dtpBdate.Value = dtpBdate.Value.AddDays(1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetMemo(sender.Equals(btnSearch2));
        }

        private bool SaveData()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            if (Encoding.Default.GetBytes(txtRich.Rtf).Length > 4000)
            {
                ComFunc.MsgBoxEx(this, "Night에 글자수가 너무 많습니다.");
                return rtnVal;
            }

            if (Encoding.Default.GetBytes(txtRich2.Rtf).Length > 4000)
            {
                ComFunc.MsgBoxEx(this, "Day에 글자수가 너무 많습니다.");
                return rtnVal;
            }

            if (Encoding.Default.GetBytes(txtRich3.Rtf).Length > 4000)
            {
                ComFunc.MsgBoxEx(this, "Evening에 글자수가 너무 많습니다.");
                return rtnVal;
            }

            OracleCommand Cmd = null;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                Cmd = clsDB.DbCon.Con.CreateCommand();

                #region 쿼리
                SQL = " DELETE ADMIN.AEMRMEMO ";
                SQL += ComNum.VBLF + " WHERE BDATE = '" + dtpBdate.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "   AND PTNO  = '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + "   AND IPDNO = " + AcpEmr.acpNoIn;
                #endregion

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 30;

                int RowAffected = Cmd.ExecuteNonQuery();

                #region 쿼리
                SQL = " INSERT INTO ADMIN.AEMRMEMO ";
                SQL += ComNum.VBLF + " (";
                SQL += ComNum.VBLF + " BDATE, ACPNO, PTNO, ";
                SQL += ComNum.VBLF + " IPDNO, MEMO, MEMO2, MEMO3,";
                SQL += ComNum.VBLF + " WRITESABUN, WRITEDATE, WRITETIME";
                SQL += ComNum.VBLF + " ) ";
                SQL += ComNum.VBLF + " VALUES ( ";
                SQL += ComNum.VBLF + " '" + dtpBdate.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + " , " + AcpEmr.acpNo;
                SQL += ComNum.VBLF + " , '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + " , " + AcpEmr.acpNoIn;
                SQL += ComNum.VBLF + " , :MEMO";
                SQL += ComNum.VBLF + " , :MEMO2";
                SQL += ComNum.VBLF + " , :MEMO3";
                SQL += ComNum.VBLF + " , '" + clsType.User.IdNumber + "'";
                SQL += ComNum.VBLF + " , TO_CHAR(SYSDATE, 'YYYYMMDD')";
                SQL += ComNum.VBLF + " , TO_CHAR(SYSDATE, 'HH24MISS')";
                SQL += ComNum.VBLF + " )";
                #endregion

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 30;
                Cmd.Parameters.Add("MEMO", OracleDbType.Varchar2).Value = txtRich.Rtf;
                Cmd.Parameters.Add("MEMO2", OracleDbType.Varchar2).Value = txtRich2.Rtf;
                Cmd.Parameters.Add("MEMO3", OracleDbType.Varchar2).Value = txtRich3.Rtf;

                RowAffected = Cmd.ExecuteNonQuery();

                Cmd.Dispose();

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;

            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                }
            }

        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <param name="BeforeText">false: 날짜 조회, true: 가장 최근에 쓴 항목 가져옴</param>
        private void GetMemo(bool BeforeText = false)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            OracleDataReader reader = null;

            txtRich.Clear();
            txtRich.Rtf = "";

            txtRich2.Clear();
            txtRich2.Rtf = "";

            txtRich3.Clear();
            txtRich3.Rtf = "";

            try
            {
                #region 쿼리
                SQL = "SELECT ";
                SQL += ComNum.VBLF + " MEMO, MEMO2, MEMO3";
                SQL += ComNum.VBLF + " FROM ADMIN.AEMRMEMO ";
                if (BeforeText == false)
                {
                    SQL += ComNum.VBLF + " WHERE BDATE = '" + dtpBdate.Value.ToString("yyyyMMdd") + "'";
                    if (VB.Val(AcpEmr.acpNo) > 0)
                    {
                        SQL += ComNum.VBLF + "   AND ACPNO = " + AcpEmr.acpNo;
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND PTNO = '" + AcpEmr.ptNo + "'";
                    }
                }
                else
                {
                    SQL += ComNum.VBLF + " WHERE BDATE = ";
                    SQL += ComNum.VBLF + "               (";
                    SQL += ComNum.VBLF + "               SELECT MAX(BDATE)";
                    SQL += ComNum.VBLF + "                 FROM ADMIN.AEMRMEMO ";
                    SQL += ComNum.VBLF + "                WHERE IPDNO = " + AcpEmr.acpNoIn;
                    SQL += ComNum.VBLF + "               )";
                    SQL += ComNum.VBLF + "   AND IPDNO = " + AcpEmr.acpNoIn;
                }
                
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txtRich.Rtf = reader.GetValue(0).ToString().Trim();
                    txtRich2.Rtf = reader.GetValue(1).ToString().Trim();
                    txtRich3.Rtf = reader.GetValue(2).ToString().Trim();
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;

            }
   
        }
        

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dtpFrDateTot_ValueChanged(object sender, EventArgs e)
        {
            GetMemo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                GetMemo();
            }
        }

        private void btnTool0_Click(object sender, EventArgs e)
        {
            if (ActiveRich == null)
                return;

            switch (VB.Right(((Button)sender).Name, 1))
            {
                case "0":
                    ActiveRich.SelectionFont = new Font(ActiveRich.Font, ActiveRich.SelectionFont == null || ActiveRich.SelectionFont.Italic ? FontStyle.Regular : FontStyle.Italic);
                    break;
                case "1":
                    ActiveRich.SelectionFont = new Font(ActiveRich.Font, ActiveRich.SelectionFont == null || ActiveRich.SelectionFont.Bold ? FontStyle.Regular : FontStyle.Bold);
                    break;
                case "2":
                    ActiveRich.SelectionFont = new Font(ActiveRich.Font, ActiveRich.SelectionFont == null || ActiveRich.SelectionFont.Strikeout ? FontStyle.Regular : FontStyle.Strikeout);
                    break;
                case "3":
                    ActiveRich.SelectionFont = new Font(ActiveRich.Font, ActiveRich.SelectionFont == null || ActiveRich.SelectionFont.Underline ? FontStyle.Regular : FontStyle.Underline);
                    break;
                case "4":
                    using(ColorDialog colorDialog = new ColorDialog())
                    {
                        colorDialog.ShowDialog(this);
                        ActiveRich.SelectionColor = colorDialog.Color;
                    }
                    break;
            }

            ActiveRich.Focus();
        }

        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveRich == null)
                return;

            if (ActiveRich.SelectionFont == null)
                return;

            ActiveRich.SelectionFont = new Font(ActiveRich.SelectionFont.FontFamily, Convert.ToSingle(cboSize.Text), ActiveRich.SelectionFont.Style);
            ActiveRich.Focus();
        }

        private void lblColor0_Click(object sender, EventArgs e)
        {
            if (ActiveRich == null)
                return;

            if (ActiveRich.SelectionColor == null)
                return;

            ActiveRich.SelectionColor = ((Label)sender).BackColor;
            ActiveRich.Focus();
        }

        private void cboFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveRich == null)
                return;

            if (ActiveRich.SelectionFont == null || string.IsNullOrWhiteSpace(cboSize.Text))
                return;

            ActiveRich.SelectionFont = new Font(cboFont.Text.Trim(), Convert.ToSingle(cboSize.Text), ActiveRich.SelectionFont.Style);
            ActiveRich.Focus();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtRich_Click(object sender, EventArgs e)
        {
            ActiveRich = sender as RichTextBox;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtRich_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (ActiveRich == null)
                return;

            if (btnCopy.Text.Equals("복사"))
            {
                btnCopy.Tag = ActiveRich.SelectedRtf;
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (btnCopy.Tag == null || ActiveRich == null)
                return;

            ActiveRich.SelectedRtf = btnCopy.Tag as string;
        }

        private void txtRich_Enter(object sender, EventArgs e)
        {
            ActiveRich = sender as RichTextBox;
        }

        private void txtRich_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (ActiveRich == null)
                return;


            if (e.Control && e.KeyCode == Keys.C)
            {
                btnCopy.Tag = ActiveRich.SelectedRtf;
            }
            else if (e.Control && e.KeyCode == Keys.V && btnCopy.Tag != null)
            {
                ActiveRich.SelectedRtf = btnCopy.Tag as string;
            }

        }
    }
}
