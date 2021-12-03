using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDrugIdentificationSign.cs
    /// Description     : 지참약 회신서 확인 
    /// Author          : 이정현
    /// Create Date     : 2017-12-06
    /// <history> 
    /// 지참약 회신서 확인 
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm약품식별회신확인.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupDrugIdentificationSign : Form
    {
        private string GstrIO = "";
        private string GstrBUSE = "";

        public frmComSupDrugIdentificationSign()
        {
            InitializeComponent();
        }

        public frmComSupDrugIdentificationSign(string strIO, string strBUSE)
        {
            InitializeComponent();

            GstrIO = strIO;
            GstrBUSE = strBUSE;
        }

        private void frmComSupDrugIdentificationSign_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;

            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            try
            {
                //의뢰일시 , 접수일시, 회신일시, 입원 / 외래, 진료과, 병동, 병실, 담당과장, 등록번호, 성명, 의뢰자, 답변자, WRTNO
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BUN, TO_CHAR(BDATE, 'YYYY-MM-DD HH24:MI') AS BDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDATE, 'YYYY-MM-DD HH24:MI') AS JDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(HDATE, 'YYYY-MM-DD HH24:MI') AS HDATE,";
                SQL = SQL + ComNum.VBLF + "     IPDOPD, DEPTCODE, WARDCODE, ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + "     DRCODE, PANO, DRSABUN, DRUGGIST, WRTNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST MST";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TRUNC(SYSDATE - 10) ";
                SQL = SQL + ComNum.VBLF + "         AND BUN = '2'";

                if (GstrIO != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = '" + GstrIO + "' ";

                    switch (GstrIO)
                    {
                        case "I":
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrBUSE + "' ";
                            break;
                        case "O":
                            SQL = SQL + ComNum.VBLF + "         AND DEPTCODE IN (" + GstrBUSE + ") ";
                            break;
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_ERP + "DRUG_HOIMST_CONFIRM SUB";
                SQL = SQL + ComNum.VBLF + "                     WHERE MST.WRTNO = SUB.WRTNO)";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Value = false;
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                        
                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "1":
                                ssView_Sheet1.Cells[i, 3].Text = "진행";
                                break;
                            case "2":
                                ssView_Sheet1.Cells[i, 3].Text = "완료";
                                break;
                        }

                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DRSABUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["HDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DRUGGIST"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ssView_Sheet1.RowCount == 0) { return; }

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strWRTNO = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strWRTNO = ssView_Sheet1.Cells[i, 14].Text.Trim();

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIMST_CONFIRM";
                        SQL = SQL + ComNum.VBLF + "     (WRTNO, CDATE, CSABUN, CIP)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + clsCompuInfo.gstrCOMIP + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

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
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 1, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void btnReferral_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            string strPano = "";

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (ssView_Sheet1.Cells[i, 1].BackColor == ComNum.SPSELCOLOR)
                {
                    strPano = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    break;
                }
            }

            if (strPano == "")
            {
                ComFunc.MsgBox("선택된 환자가 없습니다.");
                return;
            }

            frmSupDrstDrugIdentificationReferral frm = new frmSupDrstDrugIdentificationReferral(strPano);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }
    }
}
