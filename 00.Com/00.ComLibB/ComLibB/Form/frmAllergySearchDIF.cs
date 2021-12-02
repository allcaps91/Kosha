using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmAllergySearchDIF : Form
    {
        string FstrDamGb;
        string FstrSearch;

        private IAllergyInterface frm = null;

        public frmAllergySearchDIF(IAllergyInterface frm, string argDamGb, string argSearch)
        {
            InitializeComponent();

            this.frm = frm;
            FstrDamGb = argDamGb;
            FstrSearch = argSearch;
        }

        private void frmAllergySearchDIF_Load(object sender, EventArgs e)
        {
            //if (Debugger.IsAttached == false)
            //{
            //    //ssView.ActiveSheet.Columns[1, 6].Visible = false;
            ssView.ActiveSheet.Columns[3, 6].Visible = false;
            ssView.ActiveSheet.Columns[0].Visible = false;
            //}

            if (FstrDamGb == "1")
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "약품명(한)";
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "약품명(영)";
                ssView_Sheet1.ColumnHeader.Cells[0, 6].Text = "성분명(셀 더블클릭하면 해당 단일성분명 검색합니다. 다중 성분은 검색이 안되며 확인이 필요합니다.)";
                ssView_Sheet1.Columns[6].Visible = true;
                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.Columns[6].BackColor = Color.AliceBlue;
            }
            else if (FstrDamGb == "2")
            {
                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "성분명(영)";
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "성분명(한)";                
            }
            else if (FstrDamGb == "3")
            {
                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "성분군(영)";
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "성분군(한)";                
            }
            else
            {
                ssView_Sheet1.Columns[7].Visible = true;
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "분류";
                ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "다빈도성분군(한)";
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "다빈도성분군(영)";
                ssView_Sheet1.Columns[0].Visible = true;
                ssView_Sheet1.Columns[0].MergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always;
                ssView_Sheet1.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssView_Sheet1.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                
            }

            OracleCommand cmd = new OracleCommand();
            ComDbB.PsmhDb pDbCon = clsDB.DbCon;
            DataSet ds = new DataSet();

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_AllergyInfoSearch";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("PALLERGYTYPECD", OracleDbType.Varchar2, 1, FstrDamGb, ParameterDirection.Input);
            cmd.Parameters.Add("PKEYWORD", OracleDbType.Varchar2, 150, FstrSearch, ParameterDirection.Input);
            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataAdapter ODA = new OracleDataAdapter(cmd);
            ODA.Fill(ds);

            cmd.Dispose();
            cmd = null;

            ssView.ActiveSheet.Rows.Count = 0;
            ssView.ActiveSheet.Rows.Count = ds.Tables[0].Rows.Count;
            ssView.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("검색 결과가 없습니다.");
                this.Close();
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ssView.ActiveSheet.Cells[i, 0].Text = ds.Tables[0].Rows[i]["AllergeGroup"].ToString();  //다빈도만 사용
                if (FstrDamGb == "1" || FstrDamGb == "4")
                {
                    ssView.ActiveSheet.Cells[i, 1].Text = ds.Tables[0].Rows[i]["AllergeDesc_Kr"].ToString();
                    ssView.ActiveSheet.Cells[i, 2].Text = ds.Tables[0].Rows[i]["AllergeDesc"].ToString();
                }
                else
                {
                    ssView.ActiveSheet.Cells[i, 1].Text = ds.Tables[0].Rows[i]["AllergeDesc"].ToString();
                    ssView.ActiveSheet.Cells[i, 2].Text = ds.Tables[0].Rows[i]["AllergeDesc_Kr"].ToString();
                }                
                ssView.ActiveSheet.Cells[i, 3].Text = ds.Tables[0].Rows[i]["AllergeCode"].ToString();
                ssView.ActiveSheet.Cells[i, 4].Text = ds.Tables[0].Rows[i]["AllergeTypeCd"].ToString();
                ssView.ActiveSheet.Cells[i, 5].Text = ds.Tables[0].Rows[i]["AllergeTypeNm"].ToString();
                ssView.ActiveSheet.Cells[i, 6].Text = ds.Tables[0].Rows[i]["AllergeIgr"].ToString();   //약품명에서 사용 (성분명)

                //if (ds.Tables[0].Rows[i]["AllergeDesc_Kr"].ToString() == "")
                //{
                //    ssView.ActiveSheet.Cells[i, 0].Text = ds.Tables[0].Rows[i]["AllergeDesc"].ToString();
                //}
                //else
                //{
                //    ssView.ActiveSheet.Cells[i, 0].Text = ds.Tables[0].Rows[i]["AllergeDesc_Kr"].ToString() + "(" + ds.Tables[0].Rows[i]["AllergeDesc"].ToString() + ")";
                //}
            }
            
            ds.Dispose();
            ds = null;

            //해당의약품 예 칼럼 셋팅
            if (FstrDamGb == "4")
            {
                //해당의약품 예 값 셋팅
                ssView_Sheet1.Columns[7].Label = "해당 의약품 예";
                ssView_Sheet1.Cells[0, 7].Text = "마약, tramadol";
                ssView_Sheet1.Cells[1, 7].Text = "ergotamine포함 편두통 치료제";
                ssView_Sheet1.Cells[2, 7].Text = "aspirin";
                ssView_Sheet1.Cells[3, 7].Text = "sumatriptan, zolmitriptan";
                ssView_Sheet1.Cells[4, 7].Text = "sulpyrine";

                ssView_Sheet1.Cells[28, 7].Text = "겔포스";
                ssView_Sheet1.Cells[29, 7].Text = "almagate제제(Mg+AI)";
                ssView_Sheet1.Cells[31, 7].Text = "omeprazole, pantoprazole, lansoprazole";
                ssView_Sheet1.Cells[32, 7].Text = "cimetidine, ranitidine, famotidine";

                ssView_Sheet1.Cells[34, 7].Text = "cimetropium(알기론, 브로퓸)";
                ssView_Sheet1.Cells[35, 7].Text = "trimebutine, mebeverine, dicyclomine";
            }

            for (int i = 0; i < ssView.ActiveSheet.Columns.Count; i++)
            {
                ssView.ActiveSheet.Columns[i].Width = ssView.ActiveSheet.Columns[i].GetPreferredWidth() + 8;
            }           
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            List<string> lstDamKor = new List<string>();
            List<string> lstDamEng = new List<string>();
            List<string> lstDamCd = new List<string>();
            List<string> lstDamTypeNm = new List<string>();
            List<string> lstDamTypeCd = new List<string>();

            int nSelectedRowCount = 0;

            for (int i = 0; i < ssView.ActiveSheet.Rows.Count; i++)
            {
                if (ssView.ActiveSheet.Rows[i].BackColor != Color.LightGoldenrodYellow)
                {
                    continue;
                }

                nSelectedRowCount++;

                if (FstrDamGb == "1" || FstrDamGb == "4")
                {
                    lstDamEng.Add(ssView.ActiveSheet.Cells[i, 2].Text);
                    lstDamKor.Add(ssView.ActiveSheet.Cells[i, 1].Text);
                }
                else
                {
                    lstDamEng.Add(ssView.ActiveSheet.Cells[i, 1].Text);
                    lstDamKor.Add(ssView.ActiveSheet.Cells[i, 2].Text);
                }                
                lstDamCd.Add(ssView.ActiveSheet.Cells[i, 3].Text);
                lstDamTypeCd.Add(ssView.ActiveSheet.Cells[i, 4].Text);
                lstDamTypeNm.Add(ssView.ActiveSheet.Cells[i, 5].Text);

            }

            if (nSelectedRowCount == 0)
            {
                MessageBox.Show("약품을 선택하여 주십시오.", "확인");
                return;
            }

            string[] strDamKor = lstDamKor.ToArray();
            string[] strDamEng = lstDamEng.ToArray();
            string[] strDamCd = lstDamCd.ToArray();
            string[] strDamTypeNm = lstDamTypeNm.ToArray();
            string[] strDamTypeCd = lstDamTypeCd.ToArray();

            frm.setAllergyData(txtRmk.Text, strDamKor, strDamEng, strDamCd, strDamTypeNm, strDamTypeCd);

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView.ActiveSheet.Rows[e.Row].BackColor == Color.LightGoldenrodYellow)
            {
                ssView.ActiveSheet.Rows[e.Row].BackColor = Color.White;
            }
            else
            {
                ssView.ActiveSheet.Rows[e.Row].BackColor = Color.LightGoldenrodYellow;
            }           
            
            if (FstrDamGb == "4")
            {
                if (e.Row == 29)
                {
                    ssView.ActiveSheet.Rows[27].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[28].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[29].BackColor = Color.White;
                }
                else if (e.Row == 32)
                {
                    ssView.ActiveSheet.Rows[30].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[31].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[32].BackColor = Color.White;
                }
                else if (e.Row == 36)
                {
                    ssView.ActiveSheet.Rows[33].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[34].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[35].BackColor = Color.LightGoldenrodYellow;
                    ssView.ActiveSheet.Rows[36].BackColor = Color.White;
                }
            }
        }

        private void txtRmk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSend.PerformClick();
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {            
            if (e.Column == 6 && e.ColumnHeader == false)
            {
                FstrDamGb = "2";
                FstrSearch = ssView_Sheet1.Cells[e.Row, e.Column].Text;

                ssView.ActiveSheet.Columns[3, 6].Visible = false;
                ssView.ActiveSheet.Columns[0].Visible = false;

                ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "성분명(영)";
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "성분명(한)";

                OracleCommand cmd = new OracleCommand();
                ComDbB.PsmhDb pDbCon = clsDB.DbCon;
                DataSet ds = new DataSet();

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_AllergyInfoSearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PALLERGYTYPECD", OracleDbType.Varchar2, 1, FstrDamGb, ParameterDirection.Input);
                cmd.Parameters.Add("PKEYWORD", OracleDbType.Varchar2, 150, FstrSearch, ParameterDirection.Input);
                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                OracleDataAdapter ODA = new OracleDataAdapter(cmd);
                ODA.Fill(ds);

                cmd.Dispose();
                cmd = null;

                ssView.ActiveSheet.Rows.Count = 0;
                ssView.ActiveSheet.Rows.Count = ds.Tables[0].Rows.Count;
                ssView.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("검색 결과가 없습니다.");
                    this.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ssView.ActiveSheet.Cells[i, 0].Text = ds.Tables[0].Rows[i]["AllergeGroup"].ToString();  //다빈도만 사용
                    ssView.ActiveSheet.Cells[i, 1].Text = ds.Tables[0].Rows[i]["AllergeDesc"].ToString();
                    ssView.ActiveSheet.Cells[i, 2].Text = ds.Tables[0].Rows[i]["AllergeDesc_Kr"].ToString();
                    ssView.ActiveSheet.Cells[i, 3].Text = ds.Tables[0].Rows[i]["AllergeCode"].ToString();
                    ssView.ActiveSheet.Cells[i, 4].Text = ds.Tables[0].Rows[i]["AllergeTypeCd"].ToString();
                    ssView.ActiveSheet.Cells[i, 5].Text = ds.Tables[0].Rows[i]["AllergeTypeNm"].ToString();
                    ssView.ActiveSheet.Cells[i, 6].Text = ds.Tables[0].Rows[i]["AllergeIgr"].ToString();   //약품명에서 사용 (성분명)
                }

                ds.Dispose();
                ds = null;

                for (int i = 0; i < ssView.ActiveSheet.Columns.Count; i++)
                {
                    ssView.ActiveSheet.Columns[i].Width = ssView.ActiveSheet.Columns[i].GetPreferredWidth() + 8;
                }
            }
        }
    }
}
