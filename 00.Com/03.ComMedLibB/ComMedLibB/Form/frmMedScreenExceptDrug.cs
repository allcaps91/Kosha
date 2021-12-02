using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedScreenExceptDrug : Form
    {
        private string pInstCD;
        private string pUserId;
        private string pModuleID;
        private string pModuleNM;
        private string pModuleGubun;

        //public frmMedScreenExceptDrug()
        //{
        //    InitializeComponent();
        //}

        public frmMedScreenExceptDrug(string argInstCD, string argUserID, string argModuleID, string argModuleNM, string argModuleGubun)
        {
            InitializeComponent();

            pInstCD = argInstCD;
            pUserId = argUserID;
            pModuleID = argModuleID;
            pModuleNM = argModuleNM;
            pModuleGubun = argModuleGubun;
        }

        private void frmMedScreenExceptDrug_Load(object sender, EventArgs e)
        {
            ssExceptDrug.ActiveSheet.Columns[0].Visible = false;

            if (string.IsNullOrEmpty(pModuleID))
            {
                return;
            }

            if (pUserId != "SUPERUSER" && pModuleGubun == "H")
            {
                btnSave.Enabled = false;
            }

            lblModuleNM.Text = pModuleNM;

            SearchExceptDrugData();          
        }

        private void SearchExceptDrugData()
        {
            OracleCommand cmd = new OracleCommand();
            ComDbB.PsmhDb pDbCon = clsDB.DbCon;
            DataSet ds = new DataSet();

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_ScreenEnvExceptDrug";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("pInstCD", OracleDbType.Varchar2, 20, pInstCD, ParameterDirection.Input);
            cmd.Parameters.Add("pUserId", OracleDbType.Varchar2, 20, pUserId, ParameterDirection.Input);
            cmd.Parameters.Add("pModuleID", OracleDbType.Varchar2, 20, pModuleID, ParameterDirection.Input);
            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataAdapter ODA = new OracleDataAdapter(cmd);
            ODA.Fill(ds);

            cmd.Dispose();
            cmd = null;

            if (ds.Tables.Count > 0)
            {
                ssExceptDrug.DataSource = ds.Tables[0];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ssExceptDrug.ActiveSheet.Rows[i].Height = ComNum.SPDROWHT;
                }

                for (int i = 0; i < ssExceptDrug.ActiveSheet.Columns.Count; i++)
                {
                    ssExceptDrug.ActiveSheet.Columns[i].Width = ssExceptDrug.ActiveSheet.GetPreferredColumnWidth(i) + 8;
                }
            }            

            ds.Dispose();
            ds = null;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) && chkAll.Checked == false)
            {
                MessageBox.Show("키워드를 입력하세요");
                txtSearch.Focus();
                return;
            }

            string pSearchType = "";
            string pKeyWord = "";
            string pScode = "";

            if (rdoEngName.Checked == true)
            {
                pSearchType = "01";
            }
            else if (rdoEnglengName.Checked == true)
            {
                pSearchType = "02";
            }
            else if (rdoKorName.Checked == true)
            {
                pSearchType = "03";
            }
            else if (rdoJeyaksa.Checked == true)
            {
                pSearchType = "04";
            }
            else if (rdoHcode.Checked == true)
            {
                pSearchType = "05";
            }
            else if (rdoEdiCode.Checked == true)
            {
                pSearchType = "06";
            }

            pKeyWord = txtSearch.Text;
            pScode = (chkAll.Checked == true) ? "02" : "01";

            OracleCommand cmd = new OracleCommand();
            ComDbB.PsmhDb pDbCon = clsDB.DbCon;
            DataSet ds = new DataSet();

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_DrugSearch";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("pSearchType", OracleDbType.Varchar2, 4, pSearchType, ParameterDirection.Input);
            cmd.Parameters.Add("pKeyword", OracleDbType.Varchar2, 80, pKeyWord, ParameterDirection.Input);
            cmd.Parameters.Add("pScope", OracleDbType.Varchar2, 4, pScode, ParameterDirection.Input);
            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataAdapter ODA = new OracleDataAdapter(cmd);
            ODA.Fill(ds);

            cmd.Dispose();
            cmd = null;

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                ssDrugSearch.ActiveSheet.Rows.Count = 0;
                ssDrugSearch.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssDrugSearch.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Paytype"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Edinm"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["hdrugCd"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Engnm"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Iengnm"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Jeyaksanm"].ToString().Trim();
                    ssDrugSearch.ActiveSheet.SetRowHeight(i, ComNum.SPDROWHT);
                }

                //ssDrugSearch.ActiveSheet.Columns[1].Visible = false;        //원내/원외
                //ssDrugSearch.ActiveSheet.Columns[3].Visible = false;        //표준코드                
                //ssDrugSearch.ActiveSheet.Columns[7, 9].Visible = false;     //함량,보험기준가,청구단위
                //ssDrugSearch.ActiveSheet.Columns[11, 13].Visible = false;   //약품구분,퍼스트디스약품코드,투여경로

                //ssDrugSearch.ActiveSheet.Columns[0].Label = "급여구분";
                //ssDrugSearch.ActiveSheet.Columns[2].Label = "약품코드";
                //ssDrugSearch.ActiveSheet.Columns[4].Label = "약품명";
                //ssDrugSearch.ActiveSheet.Columns[5].Label = "영문상품명";
                //ssDrugSearch.ActiveSheet.Columns[6].Label = "영문성분명";
                //ssDrugSearch.ActiveSheet.Columns[10].Label = "제조회사";
                ssDrugSearch.ActiveSheet.Columns[0].MergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always;                

                dt.Dispose();
                dt = null;
            }

            
            ds.Dispose();
            ds = null;
        }

        private void btnRowDelete_Click(object sender, EventArgs e)
        {
            if (ssExceptDrug.ActiveSheet.Rows.Count == 0)
            {
                MessageBox.Show("삭제할 데이터가 없습니다.");
                return;
            }

            if (ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.ActiveRow.Index, 6].Text == "신규 추가")
            {
                //신규 추가 취소
                ssExceptDrug.ActiveSheet.ActiveRow.Remove();
            }
            else
            {
                //DB Data FLAG N으로 변경
                string SQL = "";
                string SqlErr = "";
                int nRowAffected = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                string strDrugCode = ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.ActiveRow.Index, 3].Text;

                SQL = " UPDATE KOSMOS_DRUG.DIFMENVEXPT SET APPLYFLAG = 'N', INPUTDT = SYSDATE ";
                SQL += ComNum.VBLF + " WHERE INSTCD = '"+ pInstCD +"' ";
                SQL += ComNum.VBLF + " AND USERID = '" + pUserId + "'";
                SQL += ComNum.VBLF + " AND MODULEID = '" + pModuleID + "'";
                SQL += ComNum.VBLF + " AND DRUGCODE = '" + strDrugCode + "'";                
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref nRowAffected, clsDB.DbCon);

                clsDB.setCommitTran(clsDB.DbCon);

                //제외약품 내용 갱신
                SearchExceptDrugData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ssExceptDrug.ActiveSheet.Rows.Count == 0)
            {
                MessageBox.Show("제외 약품 검색후 등록(더블클릭) 해주십시오.");
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int nRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < ssExceptDrug.ActiveSheet.Rows.Count; i++)
            {
                //기존의 데이터는 넘어가도록
                if (ssExceptDrug.ActiveSheet.Cells[i, 6].Text != "신규 추가")
                {
                    continue;
                }

                string strDrugCode = ssExceptDrug.ActiveSheet.Cells[i, 3].Text;

                SQL = " MERGE INTO KOSMOS_DRUG.DIFMENVEXPT ";
                SQL += ComNum.VBLF + " USING DUAL ";
                SQL += ComNum.VBLF + "   ON (INSTCD = '" + pInstCD + "' AND ";
                SQL += ComNum.VBLF + "   USERID = '" + pUserId + "'     AND ";
                SQL += ComNum.VBLF + "   MODULEID = '" + pModuleID + "' AND ";
                SQL += ComNum.VBLF + "   DRUGCODE = '" + strDrugCode + "') ";
                SQL += ComNum.VBLF + " WHEN MATCHED THEN ";
                SQL += ComNum.VBLF + "   UPDATE SET APPLYFLAG = 'Y', INPUTDT = SYSDATE ";
                SQL += ComNum.VBLF + " WHEN NOT MATCHED THEN";
                SQL += ComNum.VBLF + " INSERT ";
                SQL += ComNum.VBLF + "   (INSTCD, USERID, MODULEID, DRUGCODE, APPLYFLAG, INPUTDT) ";
                SQL += ComNum.VBLF + " VALUES('" + pInstCD + "',";
                SQL += ComNum.VBLF + " '" + pUserId + "',";
                SQL += ComNum.VBLF + " '" + pModuleID + "',";
                SQL += ComNum.VBLF + " '" + strDrugCode + "',";
                SQL += ComNum.VBLF + " 'Y', SYSDATE) ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref nRowAffected, clsDB.DbCon);                
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //제외약품 내용 갱신
            SearchExceptDrugData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void ssDrugSearch_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (string.IsNullOrEmpty(ssDrugSearch.ActiveSheet.Cells[e.Row, 2].Text))
            {
                MessageBox.Show("약품코드가 없는 약품입니다.");
                return;
            }

            if (ssExceptDrug.ActiveSheet.Rows.Count > 0)
            {
                for (int i = 0; i < ssExceptDrug.ActiveSheet.Rows.Count; i++)
                {
                    if (ssExceptDrug.ActiveSheet.Cells[i, 3].Text == ssDrugSearch.ActiveSheet.Cells[e.Row, 2].Text)
                    {
                        MessageBox.Show("중복된 약품코드 존재합니다.");
                        return;
                    }
                }
            }
                                   
            ssExceptDrug.ActiveSheet.Rows.Add(ssExceptDrug.ActiveSheet.Rows.Count, 1);

            ssExceptDrug.ActiveSheet.SetRowHeight(ssExceptDrug.ActiveSheet.Rows.Count - 1, ComNum.SPDROWHT);
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 0].Text = pModuleID;
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 2].Text = ssDrugSearch.ActiveSheet.Cells[e.Row, 1].Text;
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 3].Text = ssDrugSearch.ActiveSheet.Cells[e.Row, 2].Text;
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 4].Text = ssDrugSearch.ActiveSheet.Cells[e.Row, 4].Text;
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 5].Text = ssDrugSearch.ActiveSheet.Cells[e.Row, 4].Text;
            ssExceptDrug.ActiveSheet.Cells[ssExceptDrug.ActiveSheet.Rows.Count - 1, 6].Text = "신규 추가";

            for (int i = 0; i < ssExceptDrug.ActiveSheet.Columns.Count; i++)
            {
                ssExceptDrug.ActiveSheet.Columns[i].Width = ssExceptDrug.ActiveSheet.Columns[i].GetPreferredWidth() + 20;
            }
        }
    }
}
