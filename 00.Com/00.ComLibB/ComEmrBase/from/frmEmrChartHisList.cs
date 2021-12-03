using ComBase;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrChartHisList : Form
    {
        //이벤트를 전달할 경우
        //public delegate void SetOldChartInfo(string strEmrNo, string strOldGb);
        //public event SetOldChartInfo rSetOldChartInfo;

        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        string mPTNO = string.Empty;
        string mACPNO = string.Empty;
        string mFORMNO = string.Empty;
        //string mUPDATENO = "";
        string mFORMNAME  = string.Empty;
        string mCHARTDATE = string.Empty;
        Control mForm = null;

        public frmEmrChartHisList()
        {
            InitializeComponent();
        }


        public frmEmrChartHisList(Control pForm, string strPTNO, string strACPNO, string strFORMNO, string strFORMNAME, string strCHARTDATE)
        {
            InitializeComponent();

            mPTNO = strPTNO;
            mACPNO = strACPNO;
            mFORMNO = strFORMNO;
            //mUPDATENO = strUPDATENO;
            mFORMNAME = strFORMNAME;
            mCHARTDATE = strCHARTDATE;
            mForm = pForm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPTNO">환자 번호</param>
        /// <param name="strACPNO">재원 정보 번호(트리트티 번호)</param>
        /// <param name="strFORMNO">조회 할 서직지 번호</param>
        /// <param name="strFORMNAME">조회 할 서식지 이름(조회 폼의 타이틀로 이용)</param>
        /// <param name="strCHARTDATE">현재 일자 // "" 이 들어오면 ACPNO로 조회 </param>
        public frmEmrChartHisList(string strPTNO, string strACPNO, string strFORMNO, string strFORMNAME, string strCHARTDATE)
        {
            InitializeComponent();
            
            mPTNO = strPTNO;
            mACPNO = strACPNO;
            mFORMNO = strFORMNO;
            //mUPDATENO = strUPDATENO;
            mFORMNAME = strFORMNAME;
            mCHARTDATE = strCHARTDATE;
        }

        private void frmEmrChartHisList_Load(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;

            lblTitle.Text = "작성내역조회 : " + mFORMNAME;

            GetData();
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;

            SQL = " SELECT E.ACPNO, E.EMRNO, E.PTNO, E.MEDFRDATE, E.MEDDEPTCD, E.CHARTDATE, E.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "     E.MEDDRCD, U.USENAME AS DRNAME, U.USENAME AS CHARTNAME, F.FORMNAME, '1' AS OLDGB";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML E";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWMEDDEPTCODE D ";
            SQL = SQL + ComNum.VBLF + "  ON E.MEDDEPTCD = D.CODE ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "VIEWBUSER U ";
            SQL = SQL + ComNum.VBLF + "  ON E.USEID = U.USEID ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWACP A ";
            SQL = SQL + ComNum.VBLF + "  ON E.ACPNO = A.ACPNO";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "EMRFORM F ";
            SQL = SQL + ComNum.VBLF + "   ON E.FORMNO = F.FORMNO";
            SQL = SQL + ComNum.VBLF + "WHERE E.PTNO = '" + mPTNO + "' ";
            //SQL = SQL + ComNum.VBLF + "      AND E.MEDFRDATE <= '" + mMEDFRDATE + "' ";
            if (mCHARTDATE != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + mCHARTDATE + "' ";
            }
            SQL = SQL + ComNum.VBLF + "     AND E.FORMNO IN (" + mFORMNO + ")";
            SQL = SQL + ComNum.VBLF + "UNION ALL";
            SQL = SQL + ComNum.VBLF + " SELECT C.ACPNO, C.EMRNO, C.PTNO, C.MEDFRDATE , T.CLINCODE AS MEDDEPTCD, C.CHARTDATE, C.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "    T.DOCCODE AS MEDDRCD, U.USENAME AS DRNAME, U1.USENAME AS CHARTNAME, F.FORMNAME, F.OLDGB";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "EMR_TREATT T ";
            SQL = SQL + ComNum.VBLF + "   ON T.TREATNO = C.ACPNO";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "   ON C.FORMNO = F.FORMNO";
            SQL = SQL + ComNum.VBLF + "  AND C.UPDATENO = F.UPDATENO";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U  ";
            SQL = SQL + ComNum.VBLF + "   ON U.USEID = T.DOCCODE ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U1  ";
            SQL = SQL + ComNum.VBLF + "   ON U1.USEID = C.CHARTUSEID ";

            //if (mCHARTDATE != "")
            //{
            //    SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + mPTNO + "' ";
            //    SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + mCHARTDATE + "' ";
            //}
            //else
            //{
            //    SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = '" + mACPNO + "' ";
            //}

            SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + mPTNO + "' ";
            if (mCHARTDATE != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + mCHARTDATE + "' ";
            }
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO IN (" + mFORMNO + ")";

            SQL = SQL + ComNum.VBLF + "ORDER BY MEDFRDATE DESC, CHARTDATE DESC";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate((dt.Rows[i]["MEDFRDATE"].ToString().Trim()), "D");
                ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CHARTNAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 5].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString().Trim()), "D") + " " + ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString().Trim()), "M");
                ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ACPNO"].ToString().Trim();
                ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OLDGB"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            //rEventClosed();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strEmrNo = "0";
            string strOldGb = "0";

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            strEmrNo = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();
            strOldGb = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();

            //#region 전과/전입 기록지 때문에 만듬.
            ////controls = mForm.Controls.Find("lblEmrNoTR", true);

            ////if (controls != null)
            ////{
            ////    if (controls.Length > 0)
            ////    {
            ////        if (controls[0] is Label)
            ////        {
            ////            ((Label)controls[0]).Text = strEmrNo;
            ////        }
            ////    }
            ////}
            //#endregion

            //rSetOldChartInfo(strEmrNo, strOldGb);
            
            if (strOldGb == "1")
            {
                clsOldChart.LoadDataXMLOldChart(mForm, strEmrNo, false, false, null, null);
                if (mForm is frmEmrChartNew)
                {
                    (mForm as frmEmrChartNew).SetImgDefault();
                    (mForm as frmEmrChartNew).SetInitChatValue();
                }
            }
            else
            {
                clsXML.LoadDataChartRow(clsDB.DbCon, mForm, strEmrNo, false, false, null, null);
                if (mForm is frmEmrChartNew)
                {
                    #region 혈액투석 기록지 이전 EMRNO 
                    if (mFORMNO.Equals("1577"))
                    {
                        (mForm as frmEmrChartNew).mstrEmrNo = strEmrNo;
                    }
                    #endregion
                    (mForm as frmEmrChartNew).SetImgDefault();
                    (mForm as frmEmrChartNew).SetInitChatValue();
                }
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //rEventClosed();
            this.Close();
        }
    }
}
