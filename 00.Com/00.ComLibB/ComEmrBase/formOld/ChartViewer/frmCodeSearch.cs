using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmCodeSearch
    /// Description     : Code Search
    /// Author          : 이현종
    /// Create Date     : 2019-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmCodeSearch.frm) >> frmCodeSearch.cs 폼이름 재정의" />
    /// 
    public partial class frmCodeSearch : Form
    {

        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        public frmCodeSearch()
        {
            InitializeComponent();
        }

        private void FrmChartLog_Load(object sender, EventArgs e)
        {
            txtCode.Clear();
            txtNameE.Text = "";
            txtNameK.Text = "";
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }


        void GetSearhData()
        {
            string SQL = string.Empty;
            OracleDataReader reader = null;

            txtNameE.Text = "";
            txtNameK.Text = "";

            if (txtCode.Text.Trim().Length == 0)
                return;


            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT ILLNAMEK, ILLNAMEE ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ILLS";
                SQL += ComNum.VBLF + " WHERE ILLCODE = '" + txtCode.Text.Trim() + "'"                                                          ;

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(reader.HasRows && reader.Read())
                {
                    //ILLNAMEK
                    txtNameK.Text = reader.GetValue(0).ToString().Trim();
                    //ILLNAMEE
                    txtNameE.Text = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();

                Cursor.Current = Cursors.Default;
                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if(rClosed == null)
            {
                Close();
                return;
            }
            rClosed();
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCode.Text.Trim().Length == 0)
                return;

            if(e.KeyCode == Keys.Enter)
            {
                txtCode.Text = txtCode.Text.ToUpper();
                GetSearhData();
            }
        }
    }
}
