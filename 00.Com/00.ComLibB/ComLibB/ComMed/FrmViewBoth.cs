using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description : 영상의학 부위선택
/// Author : 이상훈
/// Create Date : 2017.07.24
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmViewBoth.frm, "/>
namespace ComLibB
{
    public partial class FrmViewBoth : Form
    {
        string strOrdCode;
        string strCallFormName;
        int nStartRow;

        public FrmViewBoth()
        {
            InitializeComponent();
        }

        public FrmViewBoth(string sCallFormName, string sOrdCode, int nRow)
        {
            InitializeComponent();

            strOrdCode = sOrdCode;
            strCallFormName = sCallFormName;
            nStartRow = nRow;
        }

        string SQL = "";
        DataTable dt = null;
        string SqlErr = "";

        clsSpread SP = new clsSpread();

        //public delegate void XrayBoth_DoubleClick(string strSuCode, string strSubName, int nStartRow);
        //public static event XrayBoth_DoubleClick ssXrayBothDoubleClick;

        private void FrmViewBoth_Load(object sender, EventArgs e)
        {
            // 전산업무의뢰서 2019-1363
            lblTitleSub0.Text = "Order : " + READ_ORDERNAME(strOrdCode);

            txtSearch.Text = "";
            txtSearch.Visible = false;

            if (strCallFormName == "Order")
            {
                this.ControlBox = false;
            }

            if (strOrdCode == "$$39")
            {
                txtSearch.Visible = true;
            }

            fn_View_Data("");
        }

        private void ssXrayPart_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true) return;

            btnOK_Click(btnOK, e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ssXrayPart_Sheet1.NonEmptyRowCount == 0)
            {
                clsOrdFunction.GstrSELECTGbInfo = "";
            }
            else
            {   
                clsOrdFunction.GstrSELECTGbInfo = ssXrayPart.ActiveSheet.Cells[ssXrayPart.ActiveSheet.ActiveRowIndex, 0].Text.Trim();
                clsOrdFunction.GstrSELECTSuCode = ssXrayPart.ActiveSheet.Cells[ssXrayPart.ActiveSheet.ActiveRowIndex, 1].Text.Trim();
                
                try
                {
                    SQL = "";
                    SQL += " SELECT b.SuGbF                                                     \r";
                    SQL += "   FROM KOSMOS_PMPA.BAS_SUT b                                       \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUN C                                       \r";
                    SQL += "  WHERE B.SUNEXT = '" + clsOrdFunction.GstrSELECTSuCode.Trim() + "' \r";
                    SQL += "    AND B.SuCode  = C.SuNEXT(+)                                     \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {   
                        clsOrdFunction.GstrSELECTGbSelf = dt.Rows[0]["SUGBF"].ToString();
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                //ssXrayBothDoubleClick(clsOrdFunction.GstrSELECTSuCode, clsOrdFunction.GstrSELECTGbInfo, nStartRow);
            }

            this.Close();
        }

        private void ssXrayPart_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2")
            //    {
            //        btnOK_Click(btnOK, e);
            //    }
            //}
        }

        private void ssXrayPart_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssXrayPart.ActiveSheet.Cells[0, 0, ssXrayPart_Sheet1.RowCount - 1, ssXrayPart_Sheet1.ColumnCount - 1].BackColor = Color.White;
            ssXrayPart.ActiveSheet.Cells[e.Row, 0, e.Row, ssXrayPart.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                fn_View_Data(txtSearch.Text.Trim());
            }
        }

        void fn_View_Data(string strSchWord)
        {
            int j = -1;

            SP.Spread_All_Clear(ssXrayPart);

            try
            {
                SQL = "";
                SQL += " SELECT  SubName, SuCode FROM KOSMOS_OCS.OCS_SUBCODE                    \r";
                SQL += "  WHERE OrderCode = '" + strOrdCode.Trim() + "'                         \r";
                SQL += "    AND (DelDate IS NULL OR DelDate ='')                                \r";
                if (strSchWord != "")
                {
                    SQL += "    AND SUBNAME LIKE '%" + strSchWord + "%'                         \r";
                }
                if (strOrdCode.Trim() == "$$39")
                {
                    SQL += "  ORDER BY substr(subname,1,2) , substr(subname,9,2)                \r";
                }
                else
                {
                    //SQL += "  ORDER BY SEQNO                                                    \r";
                    SQL += "  ORDER BY SUBNAME                                                    \r"; 
                    //2021-1071 전산업무 의뢰서로 인한 작업.
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssXrayPart.ActiveSheet.RowCount = dt.Rows.Count;
                    if (strCallFormName != "Order")
                    {
                        clsDB.DataTableToSpdRow(dt, ssXrayPart, 0, true);
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (dt.Rows[i - 1]["SUBNAME"].ToString().Trim() != dt.Rows[i]["SUBNAME"].ToString().Trim())
                                {
                                    ssXrayPart.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SUBNAME"].ToString().Trim();
                                    ssXrayPart.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                                    if (dt.Rows[i]["SUBNAME"].ToString().Trim() == clsOrdFunction.GstrSELECTGbInfo)
                                    {
                                        j = i;
                                    }
                                }
                            }
                            else
                            {
                                ssXrayPart.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SUBNAME"].ToString().Trim();
                                ssXrayPart.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                                if (dt.Rows[i]["SUBNAME"].ToString().Trim() == clsOrdFunction.GstrSELECTGbInfo)
                                {
                                    j = i;
                                }
                            }
                        }
                    }
                    if (strCallFormName != "Order")
                    {
                        if (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2")
                        {
                            if (ssXrayPart_Sheet1.RowCount > 0 && j >= 0)
                            {
                                ssXrayPart.ActiveSheet.ActiveRowIndex = j;

                                CellClickEventArgs k = new CellClickEventArgs(new SpreadView(), j, 0, j, 0, new MouseButtons(), false, false);
                                ssXrayPart_CellClick(this.ssXrayPart, k);
                            }
                        }
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

        private string READ_ORDERNAME(string strOrderCode)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";     //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += " SELECT ORDERNAME FROM KOSMOS_OCS.OCS_ORDERCODE \r";
                SQL += " WHERE ORDERCODE = '" + strOrderCode + "'       \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ORDERNAME"].ToString();

                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
