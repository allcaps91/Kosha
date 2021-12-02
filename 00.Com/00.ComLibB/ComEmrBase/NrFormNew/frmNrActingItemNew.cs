using System;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNrActingItemNew : Form
    {
        //폼이 Close될 경우
        public delegate void EventClosed(double dblEMRNO);
        public event EventClosed rEventClosed;

        private usTimeSet usTimeSetEvent;
        private ComboBox mMaskBox = null;

        private string mstrITEMTIME = "";
        private string mITEMCD = "";
        private string mChartDate = "";
        private EmrPatient AcpEmr = null;

        private string mstrFormNo = "1575";
        private string mstrUpdateNo = "2";

        private double mNRACTSEQ = 0;

        private string mstrGB = "T"; //작업구분


        public frmNrActingItemNew()
        {
            InitializeComponent();
        }

        public frmNrActingItemNew(string strGB, EmrPatient pAcp, string strActDate, string strITEMCD, string strITEMTIME, string strFormNo, string strUpdateNo)
        {
            InitializeComponent();

            mstrGB = strGB;
            AcpEmr = pAcp;
            mChartDate = strActDate;
            mstrITEMTIME = strITEMTIME;
            mITEMCD = strITEMCD;

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
        }

        private void frmNrActingItemNew_Load(object sender, EventArgs e)
        {
            panTime.Top = panItem.Top;
            panTime.Left = panItem.Left;

            GetDataTime();
            GetDataItem();

            if (mstrGB == "I")
            {
                optItem.Checked = true;
                for (int i = 0; i < cboActItem.Items.Count; i++)
                {
                    if (VB.Right(cboActItem.Items[i].ToString().Trim(), 20).Trim() == mITEMCD.Trim())
                    {
                        cboActItem.SelectedIndex = i;
                        return;
                    }
                }
            }
            else
            {
                optTime.Checked = true;

                if (mstrITEMTIME.Trim() != "")
                {
                    for (int i = 0; i < cboActTime.Items.Count; i++)
                    {
                        if (VB.Right(cboActTime.Items[i].ToString().Trim(), 20).Trim() == mstrITEMTIME.Trim())
                        {
                            cboActTime.SelectedIndex = i;
                            return;
                        }
                    }
                }
                else
                {
                    cboActTime.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 해당일자의 액팅 시간을 세팅한다
        /// </summary>
        private void GetDataTime()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboActTime.Items.Clear();

            string[] arryTime = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    T.REMARK1 AS ACTTIME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD T";
            SQL = SQL + ComNum.VBLF + "    ON A.ACTTIMECD = T.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND T.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND T.UNITCLS = '수행시간'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + mChartDate + "')";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.BASVAL";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    arryTime = VB.Split(dt.Rows[i]["ACTTIME"].ToString().Trim(), ",");
                }
                else
                {
                    string[] arryTimeTmp = VB.Split(dt.Rows[i]["ACTTIME"].ToString().Trim(), ",");

                    for (int j = 0; j < arryTimeTmp.Length; j++)
                    {
                        bool isExists = false;
                        for (int k = 0; k < arryTime.Length; k++)
                        {
                            if (arryTimeTmp[j].Trim() == arryTime[k].Trim())
                            {
                                isExists = true;
                            }
                        }

                        if (isExists == false)
                        {
                            Array.Resize<string>(ref arryTime, arryTime.Length + 1);
                            arryTime[arryTime.Length - 1] = arryTimeTmp[j].Trim();
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //cboActTime.SelectedIndex = 0;

            if (arryTime != null)
            {
                Array.Sort(arryTime);

                for (i = 0; i < arryTime.Length; i++)
                {
                    cboActTime.Items.Add(arryTime[i].Trim());
                }
            }
        }

        /// <summary>
        /// 해당일자의 아이템을 설정한다
        /// </summary>
        private void GetDataItem()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboActItem.Items.Clear();

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ITEMCD, B.BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD T";
            SQL = SQL + ComNum.VBLF + "    ON A.ACTTIMECD = T.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND T.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND T.UNITCLS = '수행시간'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + mChartDate + "')";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.BASVAL";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboActItem.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(200) + dt.Rows[i]["ITEMCD"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            //cboActItem.SelectedIndex = 0;
            
        }

        private void optTime_CheckedChanged(object sender, EventArgs e)
        {
            if (optTime.Checked == true)
            {
                panItem.Visible = false;
                panTime.Visible = true;
                SetActTime();
            }
        }

        private void optItem_CheckedChanged(object sender, EventArgs e)
        {
            if (optItem.Checked == true)
            {
                panItem.Visible = true;
                panTime.Visible = false;
                SetActItem();
            }
        }

        /// <summary>
        /// 시간별 모든 아이템을 세팅한다
        /// </summary>
        private void SetActTime()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strTime = cboActTime.Text.Trim();

            ssActing_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            //SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE, A.ITEMCNT, B.BASNAME, T.REMARK1 AS ACTTIME ";
            SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ITEMCNT, ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME, ";
            SQL = SQL + ComNum.VBLF + "    T.REMARK1 AS ACTTIME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '간호활동그룹'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD T";
            SQL = SQL + ComNum.VBLF + "    ON A.ACTTIMECD = T.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND T.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND T.UNITCLS = '수행시간'";
            SQL = SQL + ComNum.VBLF + "    AND T.REMARK1 LIKE '%" + strTime + "%'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + mChartDate + "')";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            ssActing_Sheet1.RowCount = dt.Rows.Count;
            ssActing_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            string strGRPNAME = "";
            int intS = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssActing_Sheet1.Cells[i, ssActing_Sheet1.ColumnCount - 3].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();


                //ssActing_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                if (strGRPNAME != dt.Rows[i]["GRPNAME"].ToString().Trim())
                {
                    ssActing_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssActing_Sheet1.AddSpanCell(intS, 0, i - intS, 1);
                    }
                    intS = i;
                }
                strGRPNAME = dt.Rows[i]["GRPNAME"].ToString().Trim();

                ssActing_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                ssActing_Sheet1.Cells[i, 2].Text = strTime;


                SetActInfo(i, dt.Rows[i]["ITEMCD"].ToString().Trim(), strTime);
            }

            ssActing_Sheet1.AddSpanCell(intS, 0, i - intS, 1);

            dt.Dispose();
            dt = null;
            
        }
        
        /// <summary>
        /// 아이템별 해당일자의 모든 시간을 세팅한다
        /// </summary>
        private void SetActItem()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strItem = VB.Right(cboActItem.Text.Trim(), 20).Trim();

            ssActing_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            //SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE, A.ITEMCNT, B.BASNAME, T.REMARK1 AS ACTTIME";
            SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ITEMCNT, ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME, ";
            SQL = SQL + ComNum.VBLF + "    T.REMARK1 AS ACTTIME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '간호활동그룹'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD T";
            SQL = SQL + ComNum.VBLF + "    ON A.ACTTIMECD = T.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND T.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND T.UNITCLS = '수행시간'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + mChartDate + "')";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strItem + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            #region //최대 ROW 수를 결정한다
            
            #endregion //최대 ROW 수를 결정한다

            string[] arryTime = VB.Split(dt.Rows[0]["ACTTIME"].ToString().Trim(), ",");
            int intMaxCol = arryTime.Length;

            ssActing_Sheet1.RowCount = intMaxCol;
            ssActing_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            string strGRPNAME = "";
            int intS = 0;

            for (i = 0; i < arryTime.Length; i++)
            {
                ssActing_Sheet1.Cells[i, ssActing_Sheet1.ColumnCount - 3].Text = dt.Rows[0]["ITEMCD"].ToString().Trim();


                //ssActing_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                if (strGRPNAME != dt.Rows[0]["GRPNAME"].ToString().Trim())
                {
                    ssActing_Sheet1.Cells[i, 0].Text = dt.Rows[0]["GRPNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssActing_Sheet1.AddSpanCell(intS, 0, i - intS, 1);
                    }
                    intS = i;
                }
                strGRPNAME = dt.Rows[0]["GRPNAME"].ToString().Trim();

                ssActing_Sheet1.Cells[i, 1].Text = dt.Rows[0]["ITEMNAME"].ToString().Trim();
                ssActing_Sheet1.Cells[i, 2].Text = arryTime[i].Trim();

                SetActInfo(i, dt.Rows[0]["ITEMCD"].ToString().Trim(), arryTime[i].Trim());
            }

            ssActing_Sheet1.AddSpanCell(intS, 0, i - intS, 1);

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 간호활동 내역을 불러와 세팅한다
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strTime"></param>
        private void SetActInfo(int Row, string strITEMCD, string strTime)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    AC.EMRNO, AC.NRACTSEQ, AC.ACTGB, AC.ACTGB1, AC.ACTRMK, AC.ACTUSEID, U.USENAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRNRACT AC";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON AC.ACTUSEID = U.USEID";
            SQL = SQL + ComNum.VBLF + "WHERE AC.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND AC.ACTDATE = '" + mChartDate + "'";
            SQL = SQL + ComNum.VBLF + "    AND AC.ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "    AND AC.ACTTIME = '" + strTime.Replace(":", "").Trim() + "00" + "'";
            SQL = SQL + ComNum.VBLF + "    AND AC.DCCLS = '0'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            ssActing_Sheet1.Cells[Row, ssActing_Sheet1.ColumnCount - 1].Text = dt.Rows[0]["NRACTSEQ"].ToString().Trim();
            ssActing_Sheet1.Cells[Row, ssActing_Sheet1.ColumnCount - 2].Text = dt.Rows[0]["EMRNO"].ToString().Trim();

            ssActing_Sheet1.Cells[Row, 3].Text = dt.Rows[0]["USENAME"].ToString().Trim();
            ssActing_Sheet1.Cells[Row, 4].Text = dt.Rows[0]["ACTGB"].ToString().Trim();
            ssActing_Sheet1.Cells[Row, 5].Text = dt.Rows[0]["ACTGB1"].ToString().Trim();
            ssActing_Sheet1.Cells[Row, 6].Text = dt.Rows[0]["ACTRMK"].ToString().Trim();

            dt.Dispose();
            dt = null;
        }

        private void cboActItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetActItem();
        }

        private void cboActTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetActTime();
        }

        private void ssActing_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssActing_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == 0)
            {
                return;
            }

            for (int i = 0; i < ssActing_Sheet1.RowCount; i++)
            {
                ssActing_Sheet1.Cells[i, 1, i, ssActing_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            }
            ssActing_Sheet1.Cells[e.Row, 1, e.Row, ssActing_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strITEMCD = ssActing_Sheet1.Cells[e.Row, ssActing_Sheet1.ColumnCount - 3].Text.Trim();
            string strEMRNO = ssActing_Sheet1.Cells[e.Row, ssActing_Sheet1.ColumnCount - 2].Text.Trim();
            string strNRACTSEQ = ssActing_Sheet1.Cells[e.Row, ssActing_Sheet1.ColumnCount - 1].Text.Trim();
            txtTime.Text = "";

            SetValue(strITEMCD, strEMRNO, strNRACTSEQ);

        }

        /// <summary>
        /// 아이템별 등록된 기본값 및 저장된 값을 세팅한다
        /// </summary>
        /// <param name="strITEMCD"></param>
        /// <param name="strEMRNO"></param>
        /// <param name="strNRACTSEQ"></param>
        private void SetValue(string strITEMCD, string strEMRNO, string strNRACTSEQ)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strTime = cboActTime.Text.Trim();

            cboValue1.Items.Clear();
            cboValue2.Items.Clear();
            txtValue.Text = "";

            cboValue1.Items.Add("");
            cboValue2.Items.Add("");

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, MAX(VALUENO) AS VALUENO, ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVALUE";
            SQL = SQL + ComNum.VBLF + "WHERE ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "ORDER BY MAX(DSPSEQ)";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboValue1.Items.Add(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            if (VB.Val(strNRACTSEQ) > 0)
            {
                string strACTGB = "";
                string strACTGB1 = "";
                string strACTRMK = "";

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     ACTGB, ACTGB1, ACTRMK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRNRACT";
                SQL = SQL + ComNum.VBLF + "WHERE NRACTSEQ = " + VB.Val(strNRACTSEQ);

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strACTGB = dt.Rows[0]["ACTGB"].ToString().Trim();
                    strACTGB1 = dt.Rows[0]["ACTGB1"].ToString().Trim();
                    strACTRMK = dt.Rows[0]["ACTRMK"].ToString().Trim();

                    for (i = 0; i < cboValue1.Items.Count; i++)
                    {
                        if (cboValue1.Items[i].ToString().Trim() == strACTGB)
                        {
                            cboValue1.SelectedIndex = i;
                            break;
                        }
                    }

                    if (cboValue1.SelectedIndex > 0)
                    {
                        for (i = 0; i < cboValue2.Items.Count; i++)
                        {
                            if (cboValue2.Items[i].ToString().Trim() == strACTGB1)
                            {
                                cboValue2.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    txtValue.Text = strACTRMK;
                }
                dt.Dispose();
                dt = null;
            }
            else
            {
                //cboValue1.SelectedIndex = 0;
            }
        }
        
        private void cboValue1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strITEMCD = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 3].Text.Trim();
            string ITEMVALUE = cboValue1.Text.Trim();

            cboValue2.Items.Clear();
            txtValue.Text = "";

            cboValue2.Items.Add("");

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, VALUENO, ITEMVALUE1 ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVALUE";
            SQL = SQL + ComNum.VBLF + "WHERE ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + ITEMVALUE + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ1";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboValue2.Items.Add(dt.Rows[i]["ITEMVALUE1"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            cboValue2.SelectedIndex = 0;
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            if (ssActing_Sheet1.ActiveColumnIndex <= 0) return;

            if (DeleteData() == true)
            {
                if (optTime.Checked == true)
                {
                    SetActTime();
                }
                else
                {
                    SetActItem();
                }
            }
        }

        /// <summary>
        /// 액팅 정보를 삭제 한다
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strITEMCD = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 3].Text.Trim();
                string strEMRNO = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 2].Text.Trim();
                string strNRACTSEQ = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 1].Text.Trim();
                string strACTTIME = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, 1].Text.Replace(":", "").Trim();

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                
                if (VB.Val(strNRACTSEQ) > 0)
                {
                    SQL = "";
                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     ACTUSEID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRNRACT";
                    SQL = SQL + ComNum.VBLF + "WHERE NRACTSEQ = " + strNRACTSEQ;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (clsType.User.IdNumber != dt.Rows[0]["ACTGB"].ToString().Trim())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "작성자가 다릅니다. 삭제할 수 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    #region //기존 내용을 DC처리한다
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRNRACT SET";
                    SQL = SQL + ComNum.VBLF + "     DCCLS = '1', ";
                    SQL = SQL + ComNum.VBLF + "     DCUSEID = '" + clsType.User.IdNumber + "', ";
                    SQL = SQL + ComNum.VBLF + "     DCDATE = '" + VB.Left(strCurDateTime, 8) + "', ";
                    SQL = SQL + ComNum.VBLF + "     DCTIME = '" + VB.Right(strCurDateTime, 6) + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE NRACTSEQ = " + strNRACTSEQ;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion //기존 내용을 DC처리한다
                }

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     NRACTSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRNRACT";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "     AND ACTDATE = '" + mChartDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND DCCLS = '0'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    //AEMRCHARTMST 를 백업을 한다
                }
                
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            if (ssActing_Sheet1.ActiveColumnIndex <= 0) return;

            if (SaveData() == true)
            {
                if (optTime.Checked == true)
                {
                    SetActTime();
                }
                else
                {
                    SetActItem();
                }
            }
        }

        /// <summary>
        /// 액팅 정보를 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            //=================
            // EMRNO는 하루에 하나만 발생하도록 했음
            // 건별로 하면 너무 많이 발생해서
            //=================
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strITEMCD = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 3].Text.Trim();
                string strEMRNO = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 2].Text.Trim();
                string strNRACTSEQ = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, ssActing_Sheet1.ColumnCount - 1].Text.Trim();
                string strACTTIME = ssActing_Sheet1.Cells[ssActing_Sheet1.ActiveRowIndex, 2].Text.Replace(":", "").Trim();

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                double newDCNRACTSEQ = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT " + ComNum.DB_EMR + " AEMRNRACT_NRACTSEQ_SEQ.NEXTVAL AS SEQ";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    newDCNRACTSEQ = VB.Val(dt.Rows[0]["SEQ"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                if (VB.Val(strNRACTSEQ) > 0)
                {
                    SQL = "";
                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     ACTUSEID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRNRACT";
                    SQL = SQL + ComNum.VBLF + "WHERE NRACTSEQ = " + strNRACTSEQ;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (clsType.User.IdNumber != dt.Rows[0]["ACTUSEID"].ToString().Trim())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "작성자가 다릅니다. 수정할 수 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    #region //기존 내용을 DC처리한다
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRNRACT SET";
                    SQL = SQL + ComNum.VBLF + "     DCCLS = '1', ";
                    SQL = SQL + ComNum.VBLF + "     DCNRACTSEQ = " + newDCNRACTSEQ + ", ";
                    SQL = SQL + ComNum.VBLF + "     DCUSEID = '" + clsType.User.IdNumber + "', ";
                    SQL = SQL + ComNum.VBLF + "     DCDATE = '" + VB.Left(strCurDateTime, 8) + "', ";
                    SQL = SQL + ComNum.VBLF + "     DCTIME = '" + VB.Right(strCurDateTime, 6) + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE NRACTSEQ = " + strNRACTSEQ;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion //기존 내용을 DC처리한다
                }

                if (VB.Val(strEMRNO) == 0)
                {
                    SQL = "";
                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     EMRNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + AcpEmr.acpNo;
                    SQL = SQL + ComNum.VBLF + "     AND CHARTDATE = '" + mChartDate + "'";
                    SQL = SQL + ComNum.VBLF + "     AND FORMNO = " + mstrFormNo;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        strEMRNO = "0";
                        string strSAVEGB = "1";
                        string strSAVECERT = "1";
                        string strSaveFlag = "";

                        //double dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, AcpEmr, this, false, this,
                        //                                            mstrFormNo, mstrUpdateNo, strEMRNO, mChartDate, strACTTIME,
                        //                                            clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", strSaveFlag);
                        double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                        double dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));

                        strEMRNO = dblEmrNoNew.ToString();
                        if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, mstrFormNo, mstrUpdateNo,
                                        mChartDate, strACTTIME,
                                        clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0",
                                        VB.Left(strCurDateTime, 8), VB.Right(strCurDateTime, 6), strSaveFlag) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    else
                    {
                        strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRNRACT";
                SQL = SQL + ComNum.VBLF + "	    (NRACTSEQ, ACPNO, EMRNO, ";
                SQL = SQL + ComNum.VBLF + "	    ACTDATE, ACTTIME, ITEMCD,  ";
                SQL = SQL + ComNum.VBLF + "	    ACTGB, ACTGB1, ACTRMK,  ";
                SQL = SQL + ComNum.VBLF + "	    ACTUSEID, ACTWDATE, ACTWTIME,  ";
                SQL = SQL + ComNum.VBLF + "	    DCCLS )";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "		" + newDCNRACTSEQ + ","; //NRACTSEQ, 
                SQL = SQL + ComNum.VBLF + "		" + AcpEmr.acpNo + ","; //ACPNO, 
                SQL = SQL + ComNum.VBLF + "		'" + VB.Val(strEMRNO) + "',"; //EMRNO, 
                SQL = SQL + ComNum.VBLF + "		'" + mChartDate + "',"; //ACTDATE, 
                SQL = SQL + ComNum.VBLF + "		'" + strACTTIME + "00" + "',"; //ACTTIME, 
                SQL = SQL + ComNum.VBLF + "		'" + strITEMCD + "',"; //ITEMCD, 
                SQL = SQL + ComNum.VBLF + "		'" + cboValue1.Text.Trim() + "',"; //ACTGB, 
                SQL = SQL + ComNum.VBLF + "		'" + cboValue2.Text.Trim() + "',"; //ACTGB1, 
                SQL = SQL + ComNum.VBLF + "		'" + txtValue.Text.Trim() + "',"; //ACTRMK, 
                SQL = SQL + ComNum.VBLF + "		'" + clsType.User.IdNumber + "',"; //ACTUSEID, 
                SQL = SQL + ComNum.VBLF + "		'" + VB.Left(strCurDateTime, 8) + "',"; //ACTWDATE, 
                SQL = SQL + ComNum.VBLF + "		'" + VB.Right(strCurDateTime, 6) + "',"; //ACTWTIME, 
                SQL = SQL + ComNum.VBLF + "		'" + "0" + "'"; //DCCLS
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
