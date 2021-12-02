using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmAutoTextPT
    /// Description     : 
    /// Author          : 이현종
    /// Create Date     : 2020-05-16(이현종)
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// 신규 기록지 추가
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr\(FrmAutoTextPR) >> FrmAutoTextPT.cs 폼이름 재정의" />
    public partial class frmAutoTextPT : Form
    {
        //상용구 전달
        public delegate void SendData(string strData, Form ClosedForm);
        public event SendData rSendData;

        string gSEQNO = string.Empty;

        string fstrTable = string.Empty;
        string fstrROWID = string.Empty;
        string fstrContents = string.Empty;

        /// <summary>
        /// 팝업 메뉴
        /// </summary>
        ContextMenu contextMenuX = null;

        /// <summary>
        /// EMR 폼 정보
        /// </summary>
        private EmrForm pForm = null;

        public frmAutoTextPT(EmrForm mEmrForm)
        {
            pForm = mEmrForm;
            InitializeComponent();
        }

        #region 폼, 버튼 이벤트
        private void frmAutoTextPT_Load(object sender, EventArgs e)
        {
            contextMenuX =  new ContextMenu();
            contextMenuX.MenuItems.Add("수정하기").Click += MenuModify_Click;
            contextMenuX.MenuItems.Add("삭제하기").Click += MenuDelete_Click;

            SetGubun();
            readContent(VB.Left(cboGubun.Text, 4));
        }

        private void MenuModify_Click(object sender, EventArgs e)
        {
            if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACRO") == false)
            {
                panContent.Visible = true;
                panContent.BringToFront();
                txtContent.Text = fstrContents;
                return;
            }

            string strCONTENTS = VB.InputBox("수정 구분 명칭을 입력하세요.", "구분 수정", fstrContents);

            if (string.IsNullOrWhiteSpace(strCONTENTS))
                return;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "UPDATE KOSMOS_EMR.EMRETCMACRO SET ";
                SQL += ComNum.VBLF + " CONTENTS = '" + strCONTENTS + "'";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + fstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                readContent(VB.Left(cboGubun.Text, 4));

                fstrTable = "";
                fstrROWID = "";
                fstrContents = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "해당 항목을 삭제하시겠습니까?", "확인") == DialogResult.No)
                return;

            if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACRO") == false && fstrTable.Equals("KOSMOS_EMR.EMRETCMACROSUB") == false)
            {
                fstrTable = "";
                fstrROWID = "";
                fstrContents = "";
                return;
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACRO"))
                {
                    if (ComFunc.MsgBoxQEx(this, "구분을 삭제하면 하위 내용도 모두 삭제 됩니다. 계속 하시겠습니까?", "확인") == DialogResult.No)
                        return;

                    SQL = " UPDATE KOSMOS_EMR.EMRETCMACRO SET";
                    SQL += ComNum.VBLF + " DELDATE = SYSDATE ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + fstrROWID + "' ";

                }
                else if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACROSUB"))
                {
                    SQL = " UPDATE KOSMOS_EMR.EMRETCMACROSUB SET";
                    SQL += ComNum.VBLF + " DELDATE = SYSDATE ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + fstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACRO"))
                {
                    readContent(VB.Left(cboGubun.Text, 4));
                }
                else if (fstrTable.Equals("KOSMOS_EMR.EMRETCMACROSUB"))
                {
                    readContentSub(gSEQNO);
                }

                fstrTable = "";
                fstrROWID = "";
                fstrContents = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            string strMacroFormNO = VB.Left(cboGubun.Text, 4); ;
            string strCONTENTS = VB.InputBox("추가할 구분 명칭을 입력하세요.", "구분 명칭");

            if (string.IsNullOrWhiteSpace(strCONTENTS))
                return;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " INSERT INTO KOSMOS_EMR.EMRETCMACRO( ";
                SQL += ComNum.VBLF + " BUSE, SEQNO, FORMNO, WRITEDATE, ";
                SQL += ComNum.VBLF + " WRITESABUN, CONTENTS) ";
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + "'" + clsType.User.BuseCode + "', KOSMOS_EMR.SEQNO_EMRETCMACRO.NEXTVAL,'" + strMacroFormNO + "', SYSDATE, ";
                SQL += ComNum.VBLF + clsType.User.IdNumber + ", '" + strCONTENTS + "'";
                SQL += ComNum.VBLF + " FROM DUAL ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(gSEQNO))
            {
                ComFunc.MsgBoxEx(this, "구분을 선택하십시요");
                return;
            }

            txtContent.Clear();
            panContent.BringToFront();
            panContent.Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            readContent(VB.Left(cboGubun.Text, 4));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            txtText.Clear();
        }

        private void btnText_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtText.Text))
            {
                ComFunc.MsgBoxEx(this, "내용이 없습니다.");
                return;
            }

            rSendData(txtText.Text.Trim(), this);
            Close();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            string strCONTENTS = txtContent.Text.Trim();

            if (string.IsNullOrWhiteSpace(strCONTENTS) || string.IsNullOrWhiteSpace(gSEQNO))
                return;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (string.IsNullOrWhiteSpace(fstrROWID) && string.IsNullOrWhiteSpace(fstrTable))
                {
                    SQL = " INSERT INTO KOSMOS_EMR.EMRETCMACROSUB( ";
                    SQL += ComNum.VBLF + " SEQNO, WRITEDATE, ";
                    SQL += ComNum.VBLF + " WRITESABUN, CONTENTS) VALUES (";
                    SQL += ComNum.VBLF + gSEQNO + ", SYSDATE, ";
                    SQL += ComNum.VBLF + clsType.User.IdNumber + ", '" + strCONTENTS + "') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }
                }
                else if (string.IsNullOrWhiteSpace(fstrROWID) == false && fstrTable.Equals("KOSMOS_EMR.EMRETCMACROSUB"))
                {
                    SQL = " UPDATE KOSMOS_EMR.EMRETCMACROSUB SET";
                    SQL += ComNum.VBLF + " CONTENTS = '" + strCONTENTS + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + fstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                readContentSub(gSEQNO);

                btnVisible.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnVisible_Click(object sender, EventArgs e)
        {
            SetVisible();
        }

        #endregion

        #region 함수

        private void SetVisible()
        {
            panContent.Visible = false;

            fstrTable = "";
            fstrROWID = "";
            fstrContents = "";

            readContentSub(gSEQNO);
        }

        /// <summary>
        /// 콤보박스 설정
        /// </summary>
        private void SetGubun()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            cboGubun.Items.Clear();
            #endregion

            #region 쿼리
            SQL = " SELECT A.FORMNO, A.FORMNAME";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRFORM A";
            SQL += ComNum.VBLF + " WHERE A.GRPFORMNO = 1031";
            SQL += ComNum.VBLF + "   AND A.USECHECK = 1";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    int index = -1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboGubun.Items.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + "." + dt.Rows[i]["FORMNAME"].ToString().Trim());
                        if (dt.Rows[i]["FORMNO"].ToString().Equals(pForm.FmFORMNO.ToString()))
                        {
                            index = i;
                        }
                    }

                    if (index > - 1)
                    {
                        cboGubun.SelectedIndex = index;
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 내용 읽기
        /// </summary>
        private void readContent(string strFormNo)
        {
            SS1_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (string.IsNullOrEmpty(strFormNo))
                return;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            #endregion

            #region 쿼리
            SQL = " SELECT CONTENTS, ROWID, WRITESABUN, SEQNO ";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRETCMACRO ";
            SQL += ComNum.VBLF + " WHERE BUSE = '" + clsType.User.BuseCode + "' ";
            if (strFormNo.Equals("****") == false)
            {
                SQL += ComNum.VBLF + "     AND FORMNO = '" + VB.Left(cboGubun.Text, 4) + "' ";
            }

            if (chkMy.Checked)
            {
                SQL += ComNum.VBLF + "   AND WRITESABUN = " + clsType.User.IdNumber;
            }
            SQL += ComNum.VBLF + "    AND DELDATE IS NULL";
            SQL += ComNum.VBLF + "  ORDER BY FORMNO, SEQNO     ";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WRITESABUN"].ToString().Trim();
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 내용 읽기
        /// </summary>
        private void readContentSub(string ArgSeqno)
        {
            SS2_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (string.IsNullOrEmpty(ArgSeqno))
                return;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            #endregion

            #region 쿼리
            SQL = "SELECT CONTENTS, ROWID, WRITESABUN";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRETCMACROSUB ";
            SQL += ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;
            SQL += ComNum.VBLF + "   AND DELDATE IS NULL";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = dt.Rows.Count;
                    SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WRITESABUN"].ToString().Trim();
                        SS2_Sheet1.Rows[i].Height = SS2_Sheet1.Rows[i].GetPreferredHeight() + 5;
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }



        #endregion

        private void cboGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            readContent(VB.Left(cboGubun.Text, 4));
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            gSEQNO = string.Empty;

            if (SS1_Sheet1.RowCount == 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                fstrTable = "";
                fstrROWID = "";
                fstrContents = "";

                if (clsType.User.IdNumber.Equals(SS1_Sheet1.Cells[e.Row, 3].Text.Trim())  == false)
                {
                    ComFunc.MsgBoxEx(this, "작성자만 수정과 삭제가 가능합니다." +  ComNum.VBLF + "작성자 : " + 
                        clsVbfunc.GetInSaName(clsDB.DbCon, SS1_Sheet1.Cells[e.Row, 3].Text.Trim()));
                    return;
                }

                fstrTable = "KOSMOS_EMR.EMRETCMACRO";
                fstrROWID = SS1_Sheet1.Cells[e.Row, 2].Text.Trim();
                fstrContents = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();

                SS1.ContextMenu = contextMenuX;
                contextMenuX.Show(SS1, new Point(e.X, e.Y));
            }
            else
            {
                gSEQNO = SS1_Sheet1.Cells[e.Row, 1].Text.Trim();
                readContentSub(gSEQNO);
            }
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS2_Sheet1.RowCount == 0)
                return;

            txtText.Text += ComNum.VBLF + SS2_Sheet1.Cells[e.Row, 0].Text.Trim();
        }


    }
}
