using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirSetSucodeIndividual.cs
    /// Description     : 개인별 수가코드 세팅
    /// Author          : 박성완
    /// Create Date     : 2017-12-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\Frm개인별수가세팅.frm
    public partial class frmComMirSetSucodeIndividual : Form
    {
        private string FstrRowID = "";
        //private string FstrRowID2 = "";

        public frmComMirSetSucodeIndividual()
        {
            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirSetSucodeIndividual_Load;

            this.btnNew.Click += BtnNew_Click;
            this.btnSetSave.Click += BtnSetSave_Click;
            this.btnSetUpdate.Click += BtnSetUpdate_Click;

            this.btnSearch.Click += BtnSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnDelete.Click += BtnDelete_Click;

            this.btnExit.Click += BtnExit_Click;

            this.ssTitle.CellDoubleClick += SsTitle_CellDoubleClick;

        }

        private void SsTitle_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            FstrRowID = "";

            FstrRowID = ssTitle.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            lbl_Title.Text = ssTitle.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            txtTitle.Text = ssTitle.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            ReadSetSuga(ssSuga, "001", clsType.User.Sabun, lbl_Title.Text.Trim());

            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            btnSetSave.Enabled = false;
            btnSetUpdate.Enabled = true;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 제목 및 수가 내역을 모두 삭제하시겠습니까??", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " DELETE FROM KOSMOS_PMPA.MIR_SET_SUGA " + ComNum.VBLF;
            SQL += " WHERE Remark ='" + lbl_Title.Text.Trim() + "' " + ComNum.VBLF;
            SQL += "  AND SABUN ='" + clsType.User.Sabun + "'     " + ComNum.VBLF;
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            clsDB.setCommitTran(clsDB.DbCon);

            ssTitle.ActiveSheet.Rows.Count = 0;

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

            ssSuga.ActiveSheet.Rows.Count = 0;

            ScreenClear();


        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strSuCode = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < ssSuga.ActiveSheet.Rows.Count; i++)
            {
                strSuCode = ssSuga.ActiveSheet.Cells[i, 1].Text.Trim();
                strROWID = ssSuga.ActiveSheet.Cells[i, 2].Text.Trim();

                if (ssSuga.ActiveSheet.Cells[i, 0].Text != "1" && strSuCode != "" && strROWID == "")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.MIR_SET_SUGA (SABUN,GUBUN,REMARK,SuCode ) VALUES (  ";
                    SQL = SQL + " '" + clsType.User.Sabun + "','001','" + lbl_Title.Text.Trim() + "','" + strSuCode.ToUpper() + "'  ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);                
                }
                else
                {
                    if (ssSuga.ActiveSheet.Cells[i,0].Text == "True" && strROWID != "")
                    {
                        SQL = " DELETE FROM KOSMOS_PMPA.MIR_SET_SUGA " + ComNum.VBLF;
                        SQL += " WHERE ROWID ='" + strROWID + "'     " + ComNum.VBLF;
                        SQL += "  AND SABUN ='" + clsType.User.Sabun + "'     " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                    else if (strROWID != "")
                    {
                        SQL = " UPDATE KOSMOS_PMPA.MIR_SET_SUGA SET " + ComNum.VBLF;
                        SQL += " SuCode ='" + strSuCode.ToUpper() + "' " + ComNum.VBLF;
                        SQL += " WHERE ROWID ='" + strROWID + "'     " + ComNum.VBLF;
                        SQL += "  AND SABUN ='" + clsType.User.Sabun + "'     " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            ssTitle.ActiveSheet.Rows.Count = 0;

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

            ssSuga.ActiveSheet.Rows.Count = 0;

            ScreenClear();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {            
            ssTitle.ActiveSheet.Rows.Count = 0;

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

            ssSuga.ActiveSheet.Rows.Count = 0;

            ScreenClear();
        }

        private void BtnSetUpdate_Click(object sender, EventArgs e)
        {
            if (lbl_Title.Text == txtTitle.Text.Trim())
            {
                MessageBox.Show("같은제목으로 변경할 수 없습니다.");
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " UPDATE  KOSMOS_PMPA.MIR_SET_SUGA SET " + ComNum.VBLF;
            SQL += " REMARK ='" + txtTitle.Text.Trim() + "' " + ComNum.VBLF;
            SQL += " WHERE REMARK ='" + lbl_Title.Text + "' " + ComNum.VBLF;
            SQL += "  AND SABUN ='" + clsType.User.Sabun + "'     " + ComNum.VBLF;
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            clsDB.setCommitTran(clsDB.DbCon);

            ssTitle.ActiveSheet.Rows.Count = 0;

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

            ssSuga.ActiveSheet.Rows.Count = 0;

            ScreenClear();

            txtTitle.Text = "";
        }

        private void BtnSetSave_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                MessageBox.Show("내용을 입력후 생성하십시오!!");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = " INSERT INTO KOSMOS_PMPA.MIR_SET_SUGA (SABUN,GUBUN,REMARK ) VALUES (  ";
            SQL = SQL + " '" + clsType.User.Sabun + "','000','" + txtTitle.Text.Trim() + "' ) ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            clsDB.setCommitTran(clsDB.DbCon);

            ssTitle.ActiveSheet.Rows.Count = 0;

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

            ssSuga.ActiveSheet.Rows.Count = 0;

            ScreenClear();

            txtTitle.Text = "";
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ScreenClear();

            btnSetSave.Enabled = true;
            btnSetUpdate.Enabled = false;

            txtTitle.Text = "";
            txtTitle.Focus();
        }

        private void FrmComMirSetSucodeIndividual_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            FstrRowID = "";
            //FstrRowID2 = "";

            lblName.Text = clsType.User.JobName;

            ssTitle.ActiveSheet.Columns[1].Visible = false;
            ssSuga.ActiveSheet.Columns[2].Visible = false;

            ScreenClear();

            ReadSetSuga(ssTitle, "000", clsType.User.Sabun, "");

        }

        private void ReadSetSuga(FpSpread ssTitle, string Gubun, string Sabun, string Title)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssTitle.ActiveSheet.Rows.Count = 0;

            SQL = " SELECT Sabun,Gubun,SuCode,Remark,ROWID " + ComNum.VBLF;
            SQL += " FROM KOSMOS_PMPA.MIR_SET_SUGA " + ComNum.VBLF;
            SQL += "  WHERE SABUN ='" + Sabun + "' " + ComNum.VBLF;
            SQL += "   AND GUBUN ='" + Gubun + "' " + ComNum.VBLF;
            if (Title != "") { SQL += "   AND Remark ='" + Title + "' " + ComNum.VBLF; }
            SQL += "  ORDER BY GUBUN,REMARK " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (Title != "")
            {
                ssTitle.ActiveSheet.Rows.Count = dt.Rows.Count + 10;
            }
            else
            {
                ssTitle.ActiveSheet.Rows.Count = dt.Rows.Count;
            }

            if (dt.Rows.Count > 0)
            {                
                for (int i=0; i < dt.Rows.Count; i++)
                {
                    ssTitle.ActiveSheet.Rows[i].Height = 23;

                    if (Gubun == "000")
                    {
                        ssTitle.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssTitle.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        ssTitle.ActiveSheet.Cells[i, 0].Text = "";
                        ssTitle.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssTitle.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
            }

            ssSuga.ActiveSheet.SetRowHeight(-1, 23);
            dt.Dispose();
            dt = null;
        }

        private void ScreenClear()
        {
            txtTitle.Text = "";
            lbl_Title.Text = "";

            ssSuga.ActiveSheet.Rows.Count = 0;

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            btnSetSave.Enabled = false;
            btnSetUpdate.Enabled = false;
        }

       
    }
}
