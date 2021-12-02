using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNrActingListNew : Form
    {
        #region // 폼에 사용하는 변수를 코딩하는 부분 ==> 꼭 변경을 요합니다.
        private const string mDirection = "V";   //기록지 작성방향(H: 옆으로, V:아래로)
        private const bool mHeadVisible = true;   //해드를 보이게 할지 여부
        private const int mintHeadCol = 4;  //해드 칼럼 수(작성, 조회 공통)
        private const int mintHeadRow = 2;  //해드 줄 수 (조회시에)
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "1575";
        public string mstrUpdateNo = "2";
        public string mstrFormText = "";
        public string mstrEmrNo = "0";
        public string mstrMode = "W";

        private EmrForm fView = null;
        #endregion

        #region // 외부 프린터
        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            rtnVal = pPrintExcept();
            return rtnVal;
        }
        #endregion

        //string mstrEmrNos = "";
        EmrPatient AcpEmr = null;

        public frmNrActingListNew()
        {
            InitializeComponent();
        }

        public frmNrActingListNew(EmrPatient po)
        {
            InitializeComponent();

            AcpEmr = po;
        }

        private void frmNrActingListNew_Load(object sender, EventArgs e)
        {
            dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medFrDate, "D"));
            mbtnPrint.Enabled = clsType.User.AuAPRINTOUT.Equals("1");

            if (AcpEmr.medEndDate != "")
            {
                dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medEndDate, "D"));
            }
            else
            {
                dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }
            GetData("");
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            GetData("");
        }


        /// <summary>
        /// 테이타를 불러 온다
        /// </summary>
        /// <param name="strSearch"></param>
        private void GetData(string strSearch)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.EMRNO, C.EMRNOHIS, C.CHARTUSEID, C.CHARTDATE, C.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMNO, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1,  ";
            SQL = SQL + ComNum.VBLF + "    B.BASEXNAME, B.BASNAME, U.NAME  ";
            SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST C  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R  ";
            SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO  ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    ON R.ITEMCD = B.BASCD ";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목' ";
            if (txtSearch.Text !="" )
            {
                SQL = SQL + ComNum.VBLF + "    AND B.BASEXNAME  like '%"+ txtSearch.Text +"'";
            }
            
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "EMR_USERT U  ";
            SQL = SQL + ComNum.VBLF + "    ON U.USERID = C.CHARTUSEID  ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo ;
            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC, B.BASEXNAME, B.NFLAG1, B.NFLAG2, R.DSPSEQ  ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC, B.BASEXNAME, B.NFLAG1, B.NFLAG2, R.DSPSEQ  ";
            }

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
                return;
            }

            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                ssView_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim() ;
                ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["NAME"].ToString().Trim();

                string strAct = "" ;
                if (dt.Rows[i]["ITEMVALUE1"].ToString().Trim() != "")
                {
                    strAct = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " (" + dt.Rows[i]["ITEMVALUE1"].ToString().Trim() +  ")";
                }
                else
                {
                    strAct = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
                ssView_Sheet1.Cells[i, 5].Text = strAct;

                ssView_Sheet1.Rows[i].Height = ssView_Sheet1.Rows[i].GetPreferredHeight() + 4;
            }

            dt.Dispose();
            dt = null;
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            pPrintExcept();
        }

        /// <summary>
        /// 출력방향, 마진 등등 변경
        /// </summary>
        /// <returns></returns>
        private int pPrintExcept()
        {
            mbtnPrint.Enabled = false;

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo.ToString(), mstrUpdateNo.ToString());

            int rtnVal = 0;

            rtnVal = PrintEmrSpreadForm(mstrFormNo, mstrUpdateNo, AcpEmr, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "COL", -1, ssView_Sheet1.ColumnCount - 3, mintHeadRow);

            //pInitFormSpc();

            mbtnPrint.Enabled = true;
            return rtnVal;
        }

        /// <summary>
        /// 출력을 한다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="blnAcp"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="spd"></param>
        /// <param name="PrintType"></param>
        /// <param name="MarginTop"></param>
        /// <param name="MarginBottom"></param>
        /// <param name="MarginLeft"></param>
        /// <param name="MarginRight"></param>
        /// <param name="UseSmartPrint"></param>
        /// <param name="Orientation"></param>
        /// <param name="strRowCol"></param>
        /// <param name="intChk"></param>
        /// <param name="intEmrNo"></param>
        /// <param name="intStartNo"></param>
        /// <returns></returns>
        public int PrintEmrSpreadForm(string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation,
                                        string strRowCol, int intChk, int intEmrNo, int intStartNo)
        {
            int intChkCnt = 0;
            int i = 0;
            int rtnVal = 0;

            if (strRowCol == "COL")
            {
                if (spd.Sheets[0].ColumnCount <= 0)
                {
                    return rtnVal;
                }

                if (intChk != -1)
                {
                    for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
                    {
                        if (Convert.ToBoolean(spd.Sheets[0].Cells[intChk, i].Value) == true)
                        {
                            intChkCnt = intChkCnt + 1;
                        }
                        else
                        {
                            spd.Sheets[0].Columns[i].Visible = false;
                        }
                    }

                    if (intChkCnt <= 0)
                    {
                        for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
                        {
                            spd.Sheets[0].Columns[i].Visible = true;
                        }
                        MessageBox.Show(new Form() { TopMost = true }, "출력이 체크되지 않았습니다.");
                        return rtnVal;
                    }
                }
            }
            else
            {
                if (spd.Sheets[0].RowCount <= 0)
                {
                    return rtnVal;
                }

                if (intChk != -1)
                {
                    for (i = 0; i < spd.Sheets[0].RowCount; i++)
                    {
                        if (Convert.ToBoolean(spd.Sheets[0].Cells[i, intChk].Value) == true)
                        {
                            intChkCnt = intChkCnt + 1;
                        }
                        else
                        {
                            spd.Sheets[0].Rows[i].Visible = false;
                        }
                    }

                    if (intChkCnt <= 0)
                    {
                        for (i = 0; i < spd.Sheets[0].RowCount; i++)
                        {
                            spd.Sheets[0].Rows[i].Visible = true;
                        }
                        MessageBox.Show(new Form() { TopMost = true }, "출력이 체크되지 않았습니다.");
                        return rtnVal;
                    }
                }
            }

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            clsFormPrint.mstrPRINTFLAG = "0";

            if (intChk != -1)
            {
                if (strRowCol == "COL")
                {
                    spd.Sheets[0].Rows[intChk].Visible = false;
                }
                else
                {
                    spd.Sheets[0].Columns[intChk].Visible = false;
                }
            }

            rtnVal = clsSpreadPrint.PrintSpdFormCnt(clsDB.DbCon, strFormNo, strUpdateNo, po, blnAcp, strFrDate, strEndDate,
                                         spd, PrintType, MarginTop, MarginBottom, MarginLeft, MarginRight, UseSmartPrint, Orientation, strCurDateTime);
            Application.DoEvents();

            int[] intPage = null;
            if (strRowCol == "COL")
            {
                intPage = ssView.GetColumnPageBreaks(0);
            }
            else
            {
                intPage = ssView.GetRowPageBreaks(0);
            }
            rtnVal = intPage.Length + 1;

            if (intChk != -1)
            {
                if (strRowCol == "COL")
                {
                    for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
                    {
                        spd.Sheets[0].Columns[i].Visible = true;
                    }
                    spd.Sheets[0].Rows[intChk].Visible = true;
                }
                else
                {
                    for (i = 0; i < spd.Sheets[0].RowCount; i++)
                    {
                        spd.Sheets[0].Rows[i].Visible = true;
                    }
                    spd.Sheets[0].Columns[intChk].Visible = true;
                }
            }

            return rtnVal;
        }


    }
}
