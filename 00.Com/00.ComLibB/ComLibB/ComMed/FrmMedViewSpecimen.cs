using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 검체선택
    /// Author : 이상훈
    /// Create Date : 2017.11.03
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewSpeciman.frm"/>
    public partial class 
        FrmMedViewSpecimen : Form
    {        
        string strSlipNo;
        string strSpecCode;
        string strOrderCode;

        int nStartRow;

        clsSpread SP = new clsSpread();

        //public delegate void Specimen_DoubleClick(string strSpecName, string strSpecCode, int nRow);
        //public static event Specimen_DoubleClick SpecimenDoubleClick;

        public FrmMedViewSpecimen()
        {
            InitializeComponent();
        }

        public FrmMedViewSpecimen(string sOrderCode, string sSlipNo, string sSpecCode, int nRow)
        {
            InitializeComponent();

            strOrderCode = sOrderCode;
            strSlipNo = sSlipNo;
            strSpecCode = sSpecCode;
            nStartRow = nRow;
        }

        private void FrmMedViewSpecimen_Load(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";     //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            int j = 0;

            this.Refresh();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            // 전산업무의뢰서 2019-1363
            lblTitleSub0.Text = "OrderCode : " + READ_ORDERNAME(strOrderCode);

            SP.Spread_All_Clear(ssSpecimen);

            try
            {
                SQL = "";
                SQL += " SELECT SpecCode, SpecName              \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OSPECIMAN        \r";
                SQL += "  WHERE Slipno = '" + strSlipNo + "'    \r";
                SQL += "  ORDER BY SPECCODE                     \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssSpecimen.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSpecimen.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SPECNAME"].ToString();
                        ssSpecimen.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SPECCODE"].ToString();
                        if (strSpecCode.Trim() == dt.Rows[i]["SPECCODE"].ToString())
                        {
                            j = i;
                        }
                    }
                }

                if (ssSpecimen.ActiveSheet.RowCount > 0)
                {
                    ssSpecimen.ActiveSheet.ActiveRowIndex = j;
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

        private void ssSpecimen_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSpecimen_Sheet1.RowCount == 0) return;

            if (e.RowHeader == true)
            {
                SP.setSpdSort(ssSpecimen, e.Column, true);
                return;
            }

            //SpecimenDoubleClick(ssSpecimen.ActiveSheet.Cells[e.Row, 0].Text, ssSpecimen.ActiveSheet.Cells[e.Row, 1].Text, nStartRow);
            clsOrdFunction.GstrSpecNm = ssSpecimen.ActiveSheet.Cells[e.Row, 0].Text;
            clsOrdFunction.GstrSpecCd = ssSpecimen.ActiveSheet.Cells[e.Row, 1].Text;

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrSpecNm = ssSpecimen.ActiveSheet.Cells[ssSpecimen.ActiveSheet.ActiveRowIndex, 0].Text;
            clsOrdFunction.GstrSpecCd = ssSpecimen.ActiveSheet.Cells[ssSpecimen.ActiveSheet.ActiveRowIndex, 1].Text;

            this.Close();
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
