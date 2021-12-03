using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : Order Slip 조회
    /// Author : 이상훈
    /// Create Date : 2017.11.02
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewSlipSub.frm"/>
    public partial class FrmMedViewSlipSub : Form
    {
        string strSlipNo;
        string strSubRate;
        string strWRTFLAG;

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        int nSort = 0;

        clsSpread SP = new clsSpread();

        public delegate void OrderSub_DoubleClick(string OrderCode, string SlipNo);
        public event OrderSub_DoubleClick OrdSubCodeDoubleClick;

        public FrmMedViewSlipSub()
        {
            InitializeComponent();
        }

        public FrmMedViewSlipSub(string sSlipNo, string sSubRate)
        {
            InitializeComponent();

            strSlipNo = sSlipNo;
            strSubRate = sSubRate;
        }

        private void FrmMedViewSlipSub_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            fn_Read_Slip(strSlipNo, strSubRate);
        }

        void fn_Read_Slip(string sSlipNo, string sSubRate)
        {
            string strSlipNo;
            string strDeptDr;
            int nDispSpace;
            string strUnit;
            string cDeptDr;
            string strPrm;

            string strNameE;
            string strNameG;

            int j;

            strWRTFLAG = "";

            strDeptDr = clsOrdFunction.GstrDrCode;
            strSlipNo = sSlipNo;
            cDeptDr = strDeptDr;

            strPrm = fn_Prm_Check();
            SP.Spread_All_Clear(ssOrdCode);
            nSort = 0;

            this.Text = "";

            try
            {
                if (strPrm != "")
                {
                    SQL = "";
                    SQL += " SELECT OrderCode, OrderName, OrderNameS, GbInput,    DispSpace \r";
                    SQL += "      , GbInfo,    GbBoth,    Bun,        NextCode,   SuCode    \r";
                    SQL += "      , GbDosage,  SpecCode,  Slipno,     GbImiv                \r";
                    SQL += "   FROM ADMIN.OCS_ORDERCODE                                \r";
                    SQL += "  WHERE Slipno    = '" + strSlipNo + "'                         \r";
                    SQL += "    AND Seqno    <> 0                                           \r";
                    SQL += "    AND SendDept <> 'N'                                         \r"; //사용하지않는 코드
                    SQL += "    AND SubRate <> '" + strSubRate + "'                         \r";
                    SQL += "    AND SubRate LIKE '" + ComFunc.LeftH(strSubRate, 2) + "%'    \r";
                    SQL += "  ORDER BY Seqno                                                \r";
                }
                else
                {
                    SQL = "";
                    SQL += " SELECT OrderCode, OrderName, OrderNameS, GbInput,    DispSpace     \r";
                    SQL += "      , GbInfo,    GbBoth,    Bun,        NextCode,   SuCode        \r";
                    SQL += "      , GbDosage,  SpecCode,  Slipno,     GbImiv                    \r";
                    SQL += "   FROM ADMIN.OCS_ORDERCODE                                    \r";
                    SQL += "  WHERE Slipno    = '" + strSlipNo + "'                             \r";
                    SQL += "    AND Seqno    <> 0                                               \r";
                    SQL += "    AND SendDept <> 'N'                                             \r"; //사용하지않는 코드
                    SQL += "    AND SubRate <>   '" + strSubRate.Trim() + "'                    \r";
                    SQL += "    AND SubRate LIKE '" + ComFunc.LeftH(strSubRate, 2) + "%'        \r";
                    SQL += "  ORDER BY Seqno                                                    \r";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssOrdCode_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count == 0)
                {
                    this.Close();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strNameE = "";
                        strNameG = "";
                        strUnit = dt.Rows[i]["ORDERNAMES"].ToString();
                        nDispSpace = Int32.Parse(dt.Rows[i]["DISPSPACE"].ToString());

                        j = 1 + (2 * nDispSpace);
                        if (strPrm == "")
                        {
                            if (dt.Rows[i]["GBINFO"].ToString().Trim() == "1")
                            {
                                ssOrdCode.ActiveSheet.Cells[i, 1].Text = "▲" + VB.Space(j) + dt.Rows[i]["ORDERNAME"].ToString();
                            }
                            else
                            {
                                ssOrdCode.ActiveSheet.Cells[i, 1].Text = " " + VB.Space(j) + dt.Rows[i]["ORDERNAME"].ToString();
                            }
                        }
                        else
                        {
                            strNameE = VB.Space(j) + strUnit + dt.Rows[i]["DISPHEADER"].ToString().Trim();
                            strNameG = VB.Space(j) + strUnit + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            ssOrdCode.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim();
                            ssOrdCode.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            ssOrdCode.ActiveSheet.Cells[i, 18].Text = VB.Space(1) + strUnit;
                            ssOrdCode.ActiveSheet.Cells[i, 1].Text = strNameG;
                        }

                        ssOrdCode.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim(); 
                        ssOrdCode.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["GBINPUT"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GBBOTH"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["NEXTCODE"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["GBDOSAGE"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssOrdCode.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["GBIMIV"].ToString().Trim();

                        if (ssOrdCode.ActiveSheet.Cells[i, 3].Text == "0")
                        {
                            ssOrdCode.ActiveSheet.Cells[i, 0, i, ssOrdCode.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 128);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        string fn_Prm_Check()
        {
            int rowcounter = 0;
            string strRtn = "";

            try
            {
                SQL = "";
                SQL += " SELECT * FROM ADMIN.OCS_ORDERCODE     \r";
                SQL += "  WHERE Slipno = '" + strSlipNo + "'        \r";
                SQL += "    AND ( Seqno = 0 OR Bun > '  ' )         \r";
                SQL += "  ORDER BY Seqno                            \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }

                rowcounter = dt.Rows.Count;

                if (dt.Rows.Count > 2) rowcounter = 2;

                if (dt.Rows.Count == 2)
                {
                    if (strSlipNo == "0003" || strSlipNo == "0004" || strSlipNo == "0005")
                    {
                        switch (dt.Rows[0]["BUN"].ToString())
                        {
                            case "11":
                                strRtn = "14";
                                break;
                            case "12":
                                strRtn = "15";
                                break;
                            default:
                                break;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //string strOK;
            //string strORDERCODE;
            string strOrderName;
            //int nRow;

            for (int i = 0; i < ssOrdCode.ActiveSheet.RowCount; i++)
            {
                if (ssOrdCode_Sheet1.Cells[i, 0].Text == "True")
                {
                    strOrderName = ssOrdCode_Sheet1.Cells[i, 1].Text;
                    clsOrdFunction.GstrselOrderCode = ssOrdCode_Sheet1.Cells[i, 2].Text;
                    clsOrdFunction.GstrOrderCode = ssOrdCode_Sheet1.Cells[i, 2].Text;
                    clsOrdFunction.GstrSelSlipno = ssOrdCode_Sheet1.Cells[i, 12].Text;

                    //입력불가 GBINPUT(1 : 입력가능)
                    if (ssOrdCode.ActiveSheet.Cells[i, 3].Text.Trim() != "1")
                    {
                        MessageBox.Show(ssOrdCode.ActiveSheet.Cells[i, 1].Text + "[" + clsOrdFunction.GstrOrderCode + "] " + "처방은 처방입력이 불가합니다!!!", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }

                    OrdSubCodeDoubleClick(clsOrdFunction.GstrOrderCode.ToString(), clsOrdFunction.GstrSelSlipno.ToString());
                }
            }
        }

        private void ssOrdCode_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssOrdCode_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                return;
            }

            string strOrderName;

            strOrderName = ssOrdCode_Sheet1.Cells[e.Row, 1].Text;
            clsOrdFunction.GstrselOrderCode = ssOrdCode_Sheet1.Cells[e.Row, 2].Text;
            clsOrdFunction.GstrOrderCode = ssOrdCode_Sheet1.Cells[e.Row, 2].Text;
            clsOrdFunction.GstrSelSlipno = ssOrdCode_Sheet1.Cells[e.Row, 12].Text;

            //입력불가 GBINPUT(1 : 입력가능)
            if (ssOrdCode.ActiveSheet.Cells[e.Row, 3].Text.Trim() != "1")
            {
                MessageBox.Show(ssOrdCode.ActiveSheet.Cells[e.Row, 1].Text + "[" + clsOrdFunction.GstrOrderCode + "] " + "처방은 처방입력이 불가합니다!!!", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OrdSubCodeDoubleClick(clsOrdFunction.GstrOrderCode.ToString(), clsOrdFunction.GstrSelSlipno.ToString());
        }

        private void ssOrdCode_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssOrdCode_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
            }

            ssOrdCode.ActiveSheet.Cells[0, 0, ssOrdCode_Sheet1.RowCount - 1, ssOrdCode_Sheet1.ColumnCount - 1].BackColor = Color.White;
            ssOrdCode.ActiveSheet.Cells[e.Row, 0, e.Row, ssOrdCode.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;
        }        
    }
}
