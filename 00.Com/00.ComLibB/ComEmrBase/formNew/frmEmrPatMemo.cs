using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 환자 메모 폼
    /// </summary>
    public partial class frmEmrPatMemo : Form
    {
        private EmrPatient AcpEmr = null;
        private Form mCallForm = null;

        private string mPTNO = string.Empty;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        #region //외부에서 이벤트를 전달받을 경우
        public void RvPatInfo(EmrPatient pAcp)
        {
            //환자 정보를 옮기고
            AcpEmr = pAcp;
            ////폼을 초기화 한다
            RvClearForm();
            //차트 기록정보
            SetPatInfo();
        }

        public void RvClearForm()
        {
            txtProbNo.Clear();
            txtReg.Clear();
            txtUseId.Clear();
            ssProblemList_Sheet1.RowCount = 0;
            dtpIndate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")) ;
        }

        private void SetPatInfo()
        {
            RvGetData();
        }

        public void RvSetInit()
        {
            //panView.Dock = DockStyle.Fill;
            //panReg.Dock = DockStyle.Fill;
            //panReg.Visible = false;
            //panView.BringToFront();
            ssProblemList_Sheet1.RowCount = 0;
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        public void RvGetData()
        {
            DataTable dt = null;

            ssProblemList_Sheet1.RowCount = 0;

            string strSql = " ";
            strSql = strSql + ComNum.VBLF + " SELECT A.INPDATE, A.CONTENT, A.USEID, A.PROBLEMNO, B.USENAME  ";
            strSql = strSql + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRPROBLEMLIST A ";
            strSql = strSql + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER B ";
            strSql = strSql + ComNum.VBLF + "       ON A.USEID = B.USEID ";
            strSql = strSql + ComNum.VBLF + " WHERE A.PTNO = '" + mPTNO + "' ";

            if (chkPlistAll.Checked == false)
            {
                strSql = strSql + ComNum.VBLF + "   AND A.USEID = '" + clsType.User.IdNumber + "' ";
            }
            if (chkDel.Checked == false)
            {
                strSql = strSql + ComNum.VBLF + "   AND A.DELYN = '0' ";
            }
            strSql = strSql + ComNum.VBLF + "    ORDER BY A.INPDATE DESC";

            string sqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
            if(sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, strSql, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssProblemList_Sheet1.RowCount = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssProblemList_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate(dt.Rows[i]["INPDATE"].ToString().Trim(), "D");
                ssProblemList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                ssProblemList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                ssProblemList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["USEID"].ToString().Trim();
                ssProblemList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PROBLEMNO"].ToString().Trim();
                ssProblemList_Sheet1.Cells[i, 5].Text = "";
                ssProblemList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssProblemList_Sheet1.GetPreferredRowHeight(i)) + 10);
            }
            dt.Dispose();
        }
        #endregion

        public frmEmrPatMemo()
        {
            InitializeComponent();
        }

        public frmEmrPatMemo(EmrPatient pAcp, Form pCallForm)
        {
            InitializeComponent();
            //환자 정보를 옮기고
            AcpEmr = pAcp;
            mCallForm = pCallForm;
        }

        public frmEmrPatMemo(string strPtno, Form pCallForm)
        {
            InitializeComponent();
            mPTNO = strPtno;
            mCallForm = pCallForm;
        }

        private void frmEmrPatMemo_Load(object sender, EventArgs e)
        {
            //panView.Dock = DockStyle.Fill;
            //panReg.Dock = DockStyle.Fill;
            //panReg.Visible = false;
            //panView.BringToFront();
            //ssProblemList_Sheet1.RowCount = 0;

            if(AcpEmr != null)
            {
                mPTNO = AcpEmr.ptNo;
            }

            ////폼을 초기화 한다
            RvClearForm();
            //차트 기록정보
            SetPatInfo();

            if (mCallForm != null)
            {
                this.Top = mCallForm.Top;
                this.Left = mCallForm.Left;
            }
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnSearch_Click(object sender, EventArgs e)
        {
            RvGetData();
        }
    
           

        /// <summary>
        /// 저장 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                txtProbNo.Text = "";
                txtReg.Text = "";
                txtUseId.Text = "";
                RvGetData();
            }
        }

        /// <summary>
        /// 저장함수
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            bool rtnVal = false;

            double dblProblemNo = VB.Val(txtProbNo.Text.Trim());
            if (txtUseId.Text.Trim().Length > 0 && txtUseId.Text.Trim() != clsType.User.IdNumber)
            {
                ComFunc.MsgBoxEx(this, "작성자만 수정할 수 있습니다.");
                return rtnVal;
            }

            if (txtReg.Text.Trim().Length == 0)
            {
                ComFunc.MsgBoxEx(this, "등록할 내용이 없습니다.");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDate = dtpIndate.Value.ToString("yyyyMMdd");
                string strSql;
                if (dblProblemNo == 0)
                {
                    dblProblemNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRPROBLEM_PROBLEMNO");
                    strSql = " INSERT INTO " + ComNum.DB_EMR + "AEMRPROBLEMLIST (PROBLEMNO,PTNO,CONTENT,USEID,INPDATE,DELYN) ";
                    strSql = strSql + ComNum.VBLF + " VALUES (" + dblProblemNo + ", ";
                    strSql = strSql + ComNum.VBLF + "         '" + mPTNO + "', ";
                    strSql = strSql + ComNum.VBLF + "         '" + txtReg.Text.Trim().Replace("'", "`") + "', ";
                    strSql = strSql + ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    strSql = strSql + ComNum.VBLF + "         '" + strCurDate + "', ";
                    strSql = strSql + ComNum.VBLF + "         '0') ";
                }
                else
                {
                    strSql = " UPDATE " + ComNum.DB_EMR + "AEMRPROBLEMLIST SET CONTENT = '" + txtReg.Text.Trim().Replace("'", "`") + "' , ";
                    strSql = strSql + ComNum.VBLF + "                          INPDATE = '" + strCurDate + "' ";
                    strSql = strSql + ComNum.VBLF + " WHERE  PROBLEMNO = " + dblProblemNo;
                }

                int RowAffected = 0;
                string sqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, strSql, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                
                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {
                txtProbNo.Clear();
                txtReg.Clear();
                txtUseId.Clear();
                RvGetData();
                //panReg.Visible = false;
                //panView.BringToFront();
            }
        }

        /// <summary>
        /// 삭제 함수
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            bool rtnVal = false;

            double dblProblemNo = VB.Val(txtProbNo.Text.Trim());

            if (dblProblemNo == 0) return rtnVal;

            if (txtUseId.Text.Trim().Length > 0 && txtUseId.Text.Trim() != clsType.User.IdNumber)
            {
                ComFunc.MsgBoxEx(this, "작성자만 수정할 수 있습니다.");
                return rtnVal;
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strSql = " UPDATE " + ComNum.DB_EMR + "AEMRPROBLEMLIST SET DELYN = '1' ";
                strSql = strSql + ComNum.VBLF + " WHERE  PROBLEMNO = " + dblProblemNo;

                int RowAffected = 0;
                string sqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, strSql, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
     

        private void mbtnClear_Click(object sender, EventArgs e)
        {

            txtProbNo.Clear();
            txtReg.Clear();
            txtUseId.Clear();
        }

        private void mbtnExitReg_Click(object sender, EventArgs e)
        {
            RvGetData();
        }

        private void mbtnExitView_Click(object sender, EventArgs e)
        {
            if (rEventClosed == null)
                return;

            rEventClosed();
        }

        private void ssProblemList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssProblemList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssProblemList, e.Column);
                return;
            }

            ComFunc.SelectRowColor(ssProblemList_Sheet1, e.Row);

            txtProbNo.Clear();
            txtReg.Clear();
            txtUseId.Clear();

            txtProbNo.Text = ssProblemList_Sheet1.Cells[e.Row, 4].Text.Trim();
            txtReg.Text = ssProblemList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtUseId.Text = ssProblemList_Sheet1.Cells[e.Row, 3].Text.Trim();
            dtpIndate.Value = Convert.ToDateTime(ssProblemList_Sheet1.Cells[e.Row, 0].Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strText = txtReg.SelectedText;
            string[] imsi = strText.Split(' ');
            string str = string.Empty;

            for (int i = 0; i <= imsi.Length; i++)
            {
                str = str + clsEngToKor.UDF_Eng2Han(imsi[i]) + " ";
            }
            txtReg.SelectedText = str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strText = txtReg.SelectedText;
            string[] imsi = strText.Split(' ');
            string str = string.Empty;

            for (int i = 0; i <= imsi.Length; i++)
            {
                str = str + clsEngToKor.UDF_Eng2Han_OneChar(imsi[i]) + " ";
            }
        }
    }
}
