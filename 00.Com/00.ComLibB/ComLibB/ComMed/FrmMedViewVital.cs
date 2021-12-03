using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedViewVital.cs
    /// Description     : Vital Sign & Bed Rest
    /// Author          : 이정현
    /// Create Date     : 2018-04-19
    /// <history> 
    /// Vital Sign & Bed Rest
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmViewVital.frm
    /// PSMH\Ocs\ipdocs\eorder\eorder\FrmViewVital.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// default 		: PSMH\Ocs\ipdocs\eorder\eorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmMedViewVital : Form
    {
        clsOrdFunction OF = new clsOrdFunction();

        private FarPoint.Win.Spread.FpSpread GssOrder = null;
        private bool GbolSchedule = false;
        private string GstrSELECTSlipnos = "";
        private int nActiveRow = 0;
        private string GstrGubun = "";

        public FrmMedViewVital(FarPoint.Win.Spread.FpSpread ssOrder, bool bolSchedule, string strSELECTSlipnos)
        {
            InitializeComponent();

            GssOrder = ssOrder;
            GbolSchedule = bolSchedule;
            GstrSELECTSlipnos = strSELECTSlipnos;
        }

        private void FrmMedViewVital_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (GssOrder.Name == "ssIpdOrder") { GstrGubun = "IPD"; }
            else if (GssOrder.Name == "ssErOrder") { GstrGubun = "ER"; }

            GetList();
        }

        private void GetList()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO = 'A1' ";
                SQL = SQL + ComNum.VBLF + "         AND SENDDEPT != 'N' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO > 0 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string strOpt = "";
            //int intAddCol = 0;
            //if (GstrGubun == "IPD") { intAddCol = 1; }

            if (GbolSchedule == true) { CmdOk_Click_Schedual(); return; }

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is CheckBox)
                {
                    if (VB.Left(control.Name, 6) == "chkOpt")
                    {
                        if (((CheckBox)control).Checked == true)
                        {
                            strOpt += control.Text + ", ";
                        }
                    }
                }
            }

            if (strOpt.Trim() != "") { strOpt = VB.Left(strOpt, strOpt.Length - 2); }

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    if (VB.Left(control.Name, 6) == "optVs0" || VB.Left(control.Name, 6) == "optVs1")
                    {
                        if (((RadioButton)control).Checked == true)
                        {
                            if (clsOrdFunction.GnActiveRow != 0)
                            {
                                if (nActiveRow == clsOrdFunction.GnActiveRow)
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    GssOrder.ActiveSheet.ActiveColumnIndex = 1;
                                    nActiveRow = 0;
                                }
                                else
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow + 1;
                                    GssOrder.ActiveSheet.AddRows(GssOrder.ActiveSheet.ActiveRowIndex, 1);
                                    //clsOrdFunction.GnActiveRow++;
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    GssOrder.ActiveSheet.ActiveColumnIndex = 1;
                                }
                            }
                            else
                            {
                                GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;
                            }

                            if (GstrGubun == "IPD")
                            {
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V/S";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(" + strOpt + ") " + control.Text.Trim() + " (" + txtCnt.Text + " 회)";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V/S";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "(" + strOpt + ") " + control.Text.Trim() + " (" + txtCnt.Text + " 회)";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SORT].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }

                            break;
                        }
                    }
                }
            }

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    if (VB.Left(control.Name, 6) == "optVs2")
                    {
                        if (((RadioButton)control).Checked == true)
                        {
                            if (clsOrdFunction.GnActiveRow != 0)
                            {
                                if (nActiveRow == clsOrdFunction.GnActiveRow)
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    GssOrder.ActiveSheet.ActiveColumnIndex = 1;
                                    nActiveRow = 0;
                                }
                                else
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow + 1;
                                    GssOrder.ActiveSheet.AddRows(GssOrder.ActiveSheet.ActiveRowIndex, 1);
                                    //clsOrdFunction.GnActiveRow++;
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    GssOrder.ActiveSheet.ActiveColumnIndex = 1;
                                }
                            }
                            else
                            {
                                GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;
                            }

                            if (GstrGubun == "IPD")
                            {
                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "V005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "V005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SORT].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            

                            break;
                        }
                    }
                }
            }
        }

        private void CmdOk_Click_Schedual()
        {
            string strOpt = "";

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is CheckBox)
                {
                    if (VB.Left(control.Name, 6) == "chkOpt") 
                    {
                        if (((CheckBox)control).Checked == true)
                        {
                            strOpt += control.Text + ", ";
                        }
                    }
                }
            }

            if (strOpt.Trim() != "") { strOpt = VB.Left(strOpt, strOpt.Length - 2); }

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    if (VB.Left(control.Name, 6) == "optVs0" || VB.Left(control.Name, 6) == "optVs1")
                    {
                        if (((RadioButton)control).Checked == true)
                        {
                            GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;

                            if (GstrGubun == "ER")
                            {
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V/S";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 5].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 6].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 17].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = "(" + strOpt + ") " + control.Text.Trim() + " (" + txtCnt.Text + " 회)";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 16].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 35].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 22].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 32].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;

                            }
                            else if(GstrGubun == "IPD")
                            {
                                //2021-01-13 안정수, iorder 칼럼추가로 인해 위치변경
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V/S";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 5].Text = "1";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 6].Text = "1";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 17].Text = "1";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = "(" + strOpt + ") " + control.Text.Trim() + " (" + txtCnt.Text + " 회)";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21].Text = "";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 16].Text = GstrSELECTSlipnos;
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 35].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 22].Text = "80";
                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 32].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                //GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;

                                //2021-01-13 안정수, iorder 칼럼추가로 인해 위치변경
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V/S";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 5].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 6].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 20].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = "(" + strOpt + ") " + control.Text.Trim() + " (" + txtCnt.Text + " 회)";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 24].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 19].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 38].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 25].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 35].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            

                            break;
                        }
                    }
                }
            }

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    if (VB.Left(control.Name, 6) == "optVs2")
                    {
                        if (((RadioButton)control).Checked == true)
                        {
                            GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;

                            if (GstrGubun == "IPD")
                            {
                                //2021-01-13 ipd일 경우 칼럼위치 수정

                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 37].Text = "V005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 5].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 6].Text = "1";

                                //TODO
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 20].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 24].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 19].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 38].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 25].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 35].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34].Text = "V005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 5].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 6].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 17].Text = "1";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 16].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 35].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 22].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 32].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Next_Process();
        }

        private void Next_Process()
        {
            int i = 0;
            string strOK = "";

            for (i = clsOrdFunction.GnSELECTSlipsCurrent + 1; i <= 53; i++)
            {
                if (OF.GnSELECTSlips[i] == 1)
                {
                    strOK = "OK";
                    clsOrdFunction.GnSELECTSlipsCurrent = i;
                    break;
                }
            }

            if (strOK == "OK")
            {
                OF.fn_ClearMemory(this);
                this.Close();

                switch (GstrSELECTSlipnos)
                {
                    //TODO : 미완성폼
                    //case "A1":   FrmViewVital.Show 1
                    //case "A2":   FrmViewDiet2.Show 1
                    //case "A4":   FrmViewSlipSpc.Show 1
                    //case "0001": FrmViewPRM.Show 1
                    //case "0028": '혈액은행
                    //             ' FrmBloodCount.Show 1
                    //             FrmViewSlip.Show 1
                    //case Else:   FrmViewSlip.Show 1
                }
            }
            else
            {
                OF.fn_ClearMemory(this);
                this.Close();
            }
        }

        private void btnO2_Click(object sender, EventArgs e)
        {
            //int intAddCol = 0;
            //if (GstrGubun == "IPD") { intAddCol = 1; }

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    if (VB.Left(control.Name, 5) == "optO2")
                    {
                        if (((RadioButton)control).Checked == true)
                        {
                            if (clsOrdFunction.GnActiveRow != 0)
                            {
                                if (nActiveRow == clsOrdFunction.GnActiveRow)
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    clsOrdFunction.GnActiveRow++;
                                }
                                else
                                {
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                    GssOrder.ActiveSheet.AddRows(GssOrder.ActiveSheet.ActiveRowIndex, 1);
                                    clsOrdFunction.GnActiveRow++;
                                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                                }
                            }
                            else
                            {
                                GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;
                            }

                            if (GstrGubun == "IPD")
                            {
                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "OX001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "OX002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "OX003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "OX004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "OX005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3);

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                switch (VB.Right(control.Name, 1))
                                {
                                    case "0":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "OX001";
                                        break;
                                    case "1":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "OX002";
                                        break;
                                    case "2":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "OX003";
                                        break;
                                    case "3":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "OX004";
                                        break;
                                    case "4":
                                        GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "OX005";
                                        break;
                                }

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = control.Text;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = GstrSELECTSlipnos;
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SORT].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3);

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();

                                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int intAddCol = 0;

            //2021-01-13 안정수, ipd 칼럼 3개 추가되서 intAddCol 1 -> 4로 변경 
            if (GstrGubun == "IPD") { intAddCol = 4; }

            if (GbolSchedule == true) { List1_DblClick_Schedual(e.Row); return; }

            if (clsOrdFunction.GnActiveRow != 0)
            {
                if (nActiveRow == clsOrdFunction.GnActiveRow)
                {
                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                    nActiveRow = 0;
                }
                else
                {
                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow + 1;
                    GssOrder.ActiveSheet.AddRows(GssOrder.ActiveSheet.ActiveRowIndex, 1);
                    //clsOrdFunction.GnActiveRow++;
                    GssOrder.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
                }
            }
            else
            {
                GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;
            }

           
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 33 + intAddCol].Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 14 + intAddCol].Text = "";
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 15 + intAddCol].Text = GstrSELECTSlipnos;
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 34 + intAddCol].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21 + intAddCol].Text = "80";
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21 + intAddCol].Text = GstrSELECTSlipnos;

            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 31 + intAddCol].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
           
        }

        private void List1_DblClick_Schedual(int intRow)
        {
            GssOrder.ActiveSheet.ActiveRowIndex = GssOrder.ActiveSheet.NonEmptyRowCount;

            if (GstrGubun == "IPD")
            {
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 43].Text = ssList_Sheet1.Cells[intRow, 0].Text.Trim() + " " + ssList_Sheet1.Cells[intRow, 1].Text.Trim();
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = ssList_Sheet1.Cells[intRow, 0].Text.Trim();
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 24].Text = "";
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 25].Text = GstrSELECTSlipnos;
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 44].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 31].Text = "80";
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 31].Text = GstrSELECTSlipnos;

                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 41].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();
            }
            else
            {
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 40].Text = ssList_Sheet1.Cells[intRow, 0].Text.Trim() + " " + ssList_Sheet1.Cells[intRow, 1].Text.Trim();
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 2].Text = ssList_Sheet1.Cells[intRow, 0].Text.Trim();
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 21].Text = "";
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 22].Text = GstrSELECTSlipnos;
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 7);
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 41].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(GstrSELECTSlipnos, "", ""), 3).Trim();

                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 28].Text = "80";
                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 28].Text = GstrSELECTSlipnos;

                GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 38].Text = GssOrder.ActiveSheet.ActiveRowIndex.ToString();
            }
            GssOrder.ActiveSheet.Cells[GssOrder.ActiveSheet.ActiveRowIndex, 1, GssOrder.ActiveSheet.ActiveRowIndex, GssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
        }
    }
}
