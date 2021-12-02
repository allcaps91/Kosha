using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirDementiaCheckDTL.cs
    /// Description     : 치매검사 특정내역 설정
    /// Author          : 박성완
    /// Create Date     : 2017-12-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\Frm치매검사특정내역.FRM
    /// </vbp>
    public partial class frmComMirDementiaCheckDTL : Form
    {
        string strPano = "";
        string strRowID = "";

        public frmComMirDementiaCheckDTL()
        {
            MessageBox.Show("입력된 환자번호가 없습니다.");
            this.Close();
        }

        /// <summary>
        /// 환자번호 받아서 처리
        /// </summary>
        /// <param name="Pano">환자번호</param>
        public frmComMirDementiaCheckDTL(string Pano)
        {
            InitializeComponent();

            strPano = Pano;
        }

        private void frmComMirDementiaCheckDTL_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            txtPano.Text = strPano;
            txtBDate.Text = "";
            txtRemark.Text = "";

            ReadData();
        }

        private void ReadData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = " SELECT PANO ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, REMARK,rowid " + ComNum.VBLF;
            SQL += " FROM KOSMOS_PMPA.MIR_INS_REMARK " + ComNum.VBLF;
            SQL += "  WHERE PANO ='" + strPano + "' " + ComNum.VBLF;
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    strRowID = dt.Rows[0]["ROWID"].ToString();
                    txtBDate.Text = dt.Rows[0]["BDate"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            if (strRowID == "")
            {
                SQL = " INSERT INTO KOSMOS_PMPA.MIR_INS_REMARK ( PANO,BDATE,REMARK ) VALUES ( " + ComNum.VBLF;
                SQL += " '" + txtPano.Text.Trim() + "',TO_DATE('" + txtBDate.Text.Trim() + "','YYYY-MM-DD') ,'" + txtRemark.Text.Trim() + "' ) " + ComNum.VBLF;
            }
            else
            {
                SQL = " UPDATE KOSMOS_PMPA.MIR_INS_REMARK SET " + ComNum.VBLF;
                SQL += "  BDATE = TO_DATE('" + txtBDate.Text.Trim() + "','YYYY-MM-DD') , " + ComNum.VBLF;
                SQL += "  REMARK ='" + txtRemark.Text.Trim() + "'  " + ComNum.VBLF;
                SQL += "   WHERE ROWID ='" + strRowID + "' " + ComNum.VBLF;
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
