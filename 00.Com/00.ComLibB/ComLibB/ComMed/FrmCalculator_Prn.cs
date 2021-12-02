using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : Calcurator(Prn)
    /// Author : 이상훈
    /// Create Date : 2018.01.15
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmCalculater_PRN.frm"/>
    public partial class FrmCalculator_Prn : Form
    {
        string strPRN_Suga;

        public FrmCalculator_Prn()
        {
            InitializeComponent();
        }

        public FrmCalculator_Prn(string sPRN_Suga)
        {
            InitializeComponent();

            strPRN_Suga = sPRN_Suga;
        }

        private void FrmCalculator_Prn_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            clsOrdFunction.GstrCalReturn = "";
            clsOrdFunction.GnCalReturn = 0;
            clsOrdFunction.GnCalLabel = 1;

            lblValue.Text = "";
            lblValue.Tag = 0;

            ssInsulin1.Visible = false;
            

            if (VB.Pstr(clsPublic.GstrPRN_Suga, "^^", 1) == "1")
            {
                ssInsulin1.Visible = false;
                ssInsulin2.Visible = true;
                ss512.Visible = false;
            }
            else if (VB.Pstr(clsPublic.GstrPRN_Suga, "^^", 1) == "2")
            {
                ssInsulin1.Visible = true;
                ssInsulin2.Visible = false;
                ss512.Visible = false;

            }
            else if (VB.Pstr(clsPublic.GstrPRN_Suga, "^^", 1) == "3")
            {
                ssInsulin2.Visible = false;
                ssInsulin1.Visible = false;
                ss512.Visible = true;
                Set_ss512(VB.Pstr(clsPublic.GstrPRN_Suga, "^^", 3));
            }

            lblInfo.Text = VB.Pstr(clsPublic.GstrPRN_Suga, "^^", 2);

            if (clsOrdFunction.GstrCalStatus == "NAL")
            {
                btnPoint.Enabled = false;
                btn12.Enabled = false;
                btn13.Enabled = false;
                btn23.Enabled = false;
                btn14.Enabled = false;
                btn15.Enabled = false;
                btn25.Enabled = false;
                btn35.Enabled = false;
                btn45.Enabled = false;
                btn34.Enabled = false;
            }

            if (clsOrdFunction.GstrCalStatus == "Contents")
            {
                btnPoint.Enabled = false;
                btn12.Enabled = false;
                btn13.Enabled = false;
                btn23.Enabled = false;
                btn14.Enabled = false;
                btn15.Enabled = false;
                btn25.Enabled = false;
                btn35.Enabled = false;
                btn45.Enabled = false;
                btn34.Enabled = false;
            }

            if (clsOrdFunction.GstrMachChk == "OK")
            {
                btnPoint.Enabled = false;
                btn12.Enabled = false;
                btn13.Enabled = false;
                btn23.Enabled = false;
                btn14.Enabled = false;
                btn15.Enabled = false;
                btn25.Enabled = false;
                btn35.Enabled = false;
                btn45.Enabled = false;
                btn34.Enabled = false;
            }
        }

        private void ssInsulin1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row >= 4 && e.ColumnHeader == false)
            {
                Spd_DblClick(ssInsulin1, e.Row, e.Column);
            }
        }

        private void ssInsulin2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row >= 4 && e.ColumnHeader == false)
            {
                Spd_DblClick(ssInsulin2, e.Row, e.Column);
            }
        }

        void Spd_DblClick(FarPoint.Win.Spread.FpSpread SpdNm, int nRow, int nCol)
        {
            lblValue.Text = SpdNm.ActiveSheet.Cells[nRow, 4].Text.Trim();
            lblValue.Tag = VB.Val(lblValue.Text);
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComFunc.RightH(lblValue.Text, 1) == ".")
            {
                lblValue.Text = ComFunc.MidH(lblValue.Text, 1, lblValue.Text.Length - 1).Trim();
            }

            if (VB.IsNumeric(lblValue.Tag) == false)
            {
                clsOrdFunction.GnCalReturn = 0;
                clsOrdFunction.GstrCalReturn = "0";
            }

            clsOrdFunction.GnCalReturn = float.Parse(lblValue.Tag.ToString());
            clsOrdFunction.GstrCalReturn = lblValue.Text;
            clsOrdFunction.GnCalLabel = (chkAll.Checked == true ? 1 : 0);

            this.Close();
        }

        private void ss512_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row >= 4 && e.ColumnHeader == false)
            {
                Spd_DblClick(ss512, e.Row, e.Column);
            }
        }

        void Set_ss512(string strOrdSeq)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM KOSMOS_OCS.PRNINSULIN_3SCALE ";
                SQL = SQL + ComNum.VBLF + " WHERE ORDSEQ = '" + strOrdSeq + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["NO1_UNIT"].ToString().Trim() == "0")
                    {
                        ss512_Sheet1.Cells[4, 3].Text = dt.Rows[0]["NO1_VALUE"].ToString().Trim() + " units";
                        ss512_Sheet1.Cells[4, 4].Text = dt.Rows[0]["NO1_VALUE"].ToString().Trim();
                    }
                    else
                    {
                        ss512_Sheet1.Cells[4, 3].Text = "주치의 Notify";
                        ss512_Sheet1.Cells[4, 4].Text = "";
                    }

                    if (dt.Rows[0]["NO2_UNIT"].ToString().Trim() == "0")
                    {
                        ss512_Sheet1.Cells[5, 3].Text = dt.Rows[0]["NO2_VALUE"].ToString().Trim() + " units";
                        ss512_Sheet1.Cells[5, 4].Text = dt.Rows[0]["NO2_VALUE"].ToString().Trim();
                    }
                    else
                    {
                        ss512_Sheet1.Cells[5, 3].Text = "주치의 Notify";
                        ss512_Sheet1.Cells[5, 4].Text = "";
                    }

                    if (dt.Rows[0]["NO3_UNIT"].ToString().Trim() == "0")
                    {
                        ss512_Sheet1.Cells[6, 3].Text = dt.Rows[0]["NO3_VALUE"].ToString().Trim() + " units";
                        ss512_Sheet1.Cells[6, 4].Text = dt.Rows[0]["NO3_VALUE"].ToString().Trim();
                    }
                    else
                    {
                        ss512_Sheet1.Cells[6, 3].Text = "주치의 Notify";
                        ss512_Sheet1.Cells[6, 4].Text = "";
                    }

                    if (dt.Rows[0]["NO4_UNIT"].ToString().Trim() == "0")
                    {
                        ss512_Sheet1.Cells[7, 3].Text = dt.Rows[0]["NO4_VALUE"].ToString().Trim() + " units";
                        ss512_Sheet1.Cells[7, 4].Text = dt.Rows[0]["NO4_VALUE"].ToString().Trim();
                    }
                    else
                    {
                        ss512_Sheet1.Cells[7, 3].Text = "주치의 Notify";
                        ss512_Sheet1.Cells[7, 4].Text = "";
                    }
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }
    }
}
