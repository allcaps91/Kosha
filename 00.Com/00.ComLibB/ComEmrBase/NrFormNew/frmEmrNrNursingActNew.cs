using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrNrNursingActNew : Form, EmrChartForm
    {

        #region // 폼에 사용하는 변수를 코딩하는 부분
        private usTimeSet usTimeSetEvent;
        private ComboBox mMaskBox = null;


        Color mSelColor = Color.LightCyan;
        Color mDeSelColor = Color.White;
        Button[] ActButton = null;

        bool isFormLoad = false; //쿼리 두번 도는거 방지
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";

        Font Notefont = null;
        #endregion

        #region //EmrChartForm

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
        public double SaveDataMsg(string strFlag)
        {
            //double dblEmrNo = pSaveData(strFlag);
            return 0;
        }

        public bool DelDataMsg()
        {
            //return pDelData();
            return false;
        }

        public void ClearFormMsg()
        {
            //mstrEmrNo = "0";
            //pClearForm();
        }
        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            return 0;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            return 0;
        }
        #endregion

        public frmEmrNrNursingActNew()
        {
            InitializeComponent();
        }

        public frmEmrNrNursingActNew(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmEmrNrNursingActNew(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        private void frmEmrNrNursingActNew_Load(object sender, EventArgs e)
        {
            #region 버튼 권한 설정
            btnAct.Visible = mstrMode.Equals("W");
            mbtnUpdate.Visible = btnAct.Visible;

            btnSaveWard.Enabled = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
            btnSaveDefault.Enabled = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
            if (clsType.User.IdNumber != "8822")
            {
                btnSaveDefault.Enabled = false;
            }
            
            #endregion


            isFormLoad = true;

            Notefont = new Font("굴림", 10);

            SetTimeCombo();
            if (VB.Val(mstrEmrNo) != 0)
            {
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                dtpChartDate.Value = DateTime.ParseExact(string.IsNullOrWhiteSpace(p.medEndDate) || p.medEndDate.Equals(strCurDate) ? strCurDate : p.medFrDate, "yyyyMMdd", null);
                //clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpChartDate, null);
            }
            else
            {
                dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }

            txtChartTime.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

            isFormLoad = false;

            lblPT.Visible = FormPatInfoFunc.Set_FormPatInfo_PTOrder(clsDB.DbCon, p, dtpChartDate.Value.ToString("yyyy-MM-dd"));

            pClearFormExcept();

            //using (frmEmrNrActWorkList frmEmrNrActWorkListX = new frmEmrNrActWorkList(p, dtpChartDate.Value.ToString("yyyyMMdd")))
            //{
            //    frmEmrNrActWorkListX.StartPosition = FormStartPosition.CenterScreen;
            //    frmEmrNrActWorkListX.ShowDialog(this);
            //}
        }

        private void mbtnUpdate_Click(object sender, EventArgs e)
        {
            using (frmNrActingSetNew frmNrActingSetX = new frmNrActingSetNew(p, "간호활동항목"))
            {
                frmNrActingSetX.TopMost = true;
                frmNrActingSetX.ShowDialog();
            }

            pClearFormExcept();
        }

        private void dtpChartDate_ValueChanged(object sender, EventArgs e)
        {
            if (isFormLoad == true)
            {
                isFormLoad = false;
                return;
            }
            pClearFormExcept();
            lblPT.Visible = FormPatInfoFunc.Set_FormPatInfo_PTOrder(clsDB.DbCon, p, dtpChartDate.Value.ToString("yyyy-MM-dd"));
        }

        private void mbtnBefore_Click(object sender, EventArgs e)
        {
            dtpChartDate.Value = dtpChartDate.Value.AddDays(-1);
        }

        private void mbtnNext_Click(object sender, EventArgs e)
        {
            dtpChartDate.Value = dtpChartDate.Value.AddDays(+1);
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            pClearFormExcept();
        }

        private void mbtnNrActingList_Click(object sender, EventArgs e)
        {
            using (frmNrActingListNew frm = new frmNrActingListNew(p))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }


        private void ssChart_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 3)
            {
                return;
            }

            if (ssChart_Sheet1.Cells[e.Row, 3].BackColor == mSelColor)
            {
                ssChart_Sheet1.Cells[e.Row, 3].BackColor = mDeSelColor;
            }
            else
            {
                ssChart_Sheet1.Cells[e.Row, 3].BackColor = mSelColor;
            }
        }

        private void ssChart_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 2)
            {
                return;
            }

            string strGrpCd = ssChart_Sheet1.Cells[e.Row, 0].Text.Trim();
            Color SelectedColor = ssChart_Sheet1.Cells[e.Row, 3].BackColor;
            Color SetColor;
            if (SelectedColor == mSelColor)
            {
                SetColor = mDeSelColor;
            }
            else
            {
                SetColor = mSelColor;
            }

            for (int i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                if (strGrpCd == ssChart_Sheet1.Cells[i, 0].Text.Trim())
                {
                    ssChart_Sheet1.Cells[i, 3].BackColor = SetColor;
                }
            }
        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            ssChart_Sheet1.RowCount = 0;
            ssChart0_Sheet1.RowCount = 0;
            ssChart1_Sheet1.RowCount = 0;

            ssChart_Sheet1.ColumnCount = 5;
            ssChart0_Sheet1.ColumnCount = 5;
            ssChart1_Sheet1.ColumnCount = 5;

            if (ActButton != null)
            {
                for (int i = 0; i < ActButton.Length; i++)
                {
                    ActButton[i].Dispose();
                    ActButton[i] = null;
                }
                ActButton = null;
            }

            CheckItemExists(dtpChartDate.Value.ToString("yyyyMMdd"), ssChart_Sheet1);

            //GetData(dtpChartDate.Value.AddDays(-1).ToString("yyyyMMdd"), ssChart0_Sheet1);
            GetData(dtpChartDate.Value.ToString("yyyyMMdd"), ssChart_Sheet1);
            //GetData(dtpChartDate.Value.AddDays(+1).ToString("yyyyMMdd"), ssChart1_Sheet1);
        }

        /// <summary>
        /// 차트일자에 아이템이 존재하는지 체크해서 넣는다
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="Spd"></param>
        private void CheckItemExists(string strChartDate, FarPoint.Win.Spread.SheetView Spd)
        {
            if (p == null) return;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;
            int intRowAffected = 0;

            string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strWDate = VB.Left(strCurDataTime, 8);
            string strWTime = VB.Right(strCurDataTime, 6);
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT A.ACPNO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, "초기값 세팅중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;

                #region 일단위이고 1일 이후 항목들 간격 날짜만큼 지났을경우 

                SQL = "WITH IPD_TRANS_CHK AS ";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + "  SELECT MAX(TRSDATE) TRSDATE, TOWARD";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.IPD_TRANSFOR I";
                SQL = SQL + ComNum.VBLF + "   WHERE IPDNO = " + p.acpNoIn;
                SQL = SQL + ComNum.VBLF + "   GROUP BY TOWARD ";
                SQL = SQL + ComNum.VBLF + "   UNION ALL";
                SQL = SQL + ComNum.VBLF + "  SELECT TO_DATE('" + strChartDate + "'), '" +  p.ward + "' TOWARD";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL";
                SQL = SQL + ComNum.VBLF + "   WHERE NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "    SELECT 1";
                SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_TRANSFOR I";
                SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = " + p.acpNoIn;
                SQL = SQL + ComNum.VBLF + "   )";
                SQL = SQL + ComNum.VBLF + ")";

                SQL = SQL + ComNum.VBLF + " SELECT 1 ";
                SQL = SQL + ComNum.VBLF + " FROM DUAL";
                SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + " (";
                SQL = SQL + ComNum.VBLF + "     SELECT 1 ";
                SQL = SQL + ComNum.VBLF + "     FROM";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "	                  A.ACPNO";
                SQL = SQL + ComNum.VBLF + "	                , A.PTNO";
                SQL = SQL + ComNum.VBLF + "	       	        , A.ITEMCD";
                SQL = SQL + ComNum.VBLF + "	                , A.ACTINTERVAL";
                SQL = SQL + ComNum.VBLF + "	                , A.ACTINTERVALCD";
                SQL = SQL + ComNum.VBLF + "	                , A.ACTCNT";
                SQL = SQL + ComNum.VBLF + "	                , TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD') AS CHARTDATE";
                SQL = SQL + ComNum.VBLF + "	          FROM KOSMOS_EMR.AEMRBNRACTSET A";
                SQL = SQL + ComNum.VBLF + "             INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "                ON A.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "               AND B.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "               AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "               AND EXISTS";
                SQL = SQL + ComNum.VBLF + "               (";
                SQL = SQL + ComNum.VBLF + "                 SELECT 1";
                SQL = SQL + ComNum.VBLF + "                   FROM " + ComNum.DB_EMR + "AEMRCHARTMST M";
                SQL = SQL + ComNum.VBLF + "                     INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "                        ON M.EMRNO = R.EMRNO";
                SQL = SQL + ComNum.VBLF + "                       AND M.EMRNOHIS = R.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "                  WHERE M.MEDFRDATE = '" + p.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "                    AND M.PTNO = '" + p.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "                    AND M.CHARTDATE = A.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "                    AND R.ITEMCD = A.ITEMCD";
                SQL = SQL + ComNum.VBLF + "               )";
                SQL = SQL + ComNum.VBLF + "          WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "            AND A.ACTINTERVAL > 1";
                SQL = SQL + ComNum.VBLF + "            AND A.ACTINTERVALCD = '일'";
                SQL = SQL + ComNum.VBLF + "            AND A.CHARTDATE >= '" + p.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "          GROUP BY A.ACPNO, A.PTNO, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT";
                SQL = SQL + ComNum.VBLF + "     ) A";
                SQL = SQL + ComNum.VBLF + "     WHERE (TO_DATE('" + strChartDate + "', 'YYYYMMDD')  - A.CHARTDATE) = A.ACTINTERVAL ";
                SQL = SQL + ComNum.VBLF + "       AND EXISTS";
                SQL = SQL + ComNum.VBLF + "       (";
                SQL = SQL + ComNum.VBLF + "         SELECT 1";
                SQL = SQL + ComNum.VBLF + "           FROM IPD_TRANS_CHK";
                //SQL = SQL + ComNum.VBLF + "          WHERE ((A.CHARTDATE  - TRUNC(TRSDATE)) >= A.ACTINTERVAL OR A.CHARTDATE = TRUNC(TRSDATE) OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') = TRUNC(TRSDATE))";
                SQL = SQL + ComNum.VBLF + "          WHERE (A.CHARTDATE  - TRUNC(TRSDATE) >= A.ACTINTERVAL OR A.CHARTDATE = TRUNC(TRSDATE) ";
                SQL = SQL + ComNum.VBLF + "              OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') = TRUNC(TRSDATE)";
                SQL = SQL + ComNum.VBLF + "              OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') - A.CHARTDATE = A.ACTINTERVAL) ";
                SQL = SQL + ComNum.VBLF + "            AND TOWARD = '" + p.ward + "'";
                SQL = SQL + ComNum.VBLF + "       )";
                SQL = SQL + ComNum.VBLF + "       AND EXISTS";
                SQL = SQL + ComNum.VBLF + "       (";
                SQL = SQL + ComNum.VBLF + "             SELECT 1             ";
                SQL = SQL + ComNum.VBLF + "               FROM KOSMOS_EMR.AEMRBASCD BB                                  ";
                SQL = SQL + ComNum.VBLF + "                 INNER JOIN KOSMOS_EMR.AEMRBASCD B                             ";
                SQL = SQL + ComNum.VBLF + "                      ON BB.BASCD = B.BASCD                                    ";
                SQL = SQL + ComNum.VBLF + "                      AND B.BSNSCLS = '기록지관리'                                ";
                SQL = SQL + ComNum.VBLF + "                      AND B.UNITCLS = '간호활동항목'                               ";
                SQL = SQL + ComNum.VBLF + "                      AND B.USECLS = '0'                                       ";
                SQL = SQL + ComNum.VBLF + "                 INNER JOIN KOSMOS_EMR.AEMRBASCD BG                            ";
                SQL = SQL + ComNum.VBLF + "                      ON B.VFLAG1 = BG.BASCD                                   ";
                SQL = SQL + ComNum.VBLF + "                      AND BG.BSNSCLS = '기록지관리'                               ";
                SQL = SQL + ComNum.VBLF + "                      AND BG.UNITCLS = '간호활동그룹'                              ";
                SQL = SQL + ComNum.VBLF + "              WHERE BB.BSNSCLS = '기록지관리'                                  ";
                SQL = SQL + ComNum.VBLF + "                AND BB.UNITCLS = '간호활동병동_" + p.ward + "'                           ";
                SQL = SQL + ComNum.VBLF + "                AND BB.USECLS = '0'                                      ";
                SQL = SQL + ComNum.VBLF + "                AND B.BASCD = A.ITEMCD                                      ";
                SQL = SQL + ComNum.VBLF + "       )";
                SQL = SQL + ComNum.VBLF + "       AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "       (";
                SQL = SQL + ComNum.VBLF + "         SELECT 1";
                SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRBNRACTSET";
                SQL = SQL + ComNum.VBLF + "          WHERE ITEMCD = A.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "            AND CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "            AND ACPNO  = A.ACPNO ";
                SQL = SQL + ComNum.VBLF + "       )";
                SQL = SQL + ComNum.VBLF + " )";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "초기값 세팅중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {                    
                    clsDB.setBeginTran(clsDB.DbCon);
                    try
                    {
                        SQL = "INSERT INTO  " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "   DISTINCT A.ACPNO, A.PTNO, ";
                        SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "' AS CHARTDATE,  ";
                        SQL = SQL + ComNum.VBLF + "    A.ITEMCD, A.ACTINTERVAL, A.ACTINTERVALCD,  ";
                        SQL = SQL + ComNum.VBLF + "    A.ACTCNT,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + strWDate + "' AS WRITEDATE,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + strWTime + "' AS WRITETIME,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' AS WRITEUSEID ";
                        SQL = SQL + ComNum.VBLF + " FROM";
                        SQL = SQL + ComNum.VBLF + " (";
                        SQL = SQL + ComNum.VBLF + "     SELECT";
                        SQL = SQL + ComNum.VBLF + "	              A.ACPNO";
                        SQL = SQL + ComNum.VBLF + "	            , A.PTNO";
                        SQL = SQL + ComNum.VBLF + "	   	        , A.ITEMCD";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVAL";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVALCD";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTCNT";
                        SQL = SQL + ComNum.VBLF + "	            , TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD') AS CHARTDATE";
                        SQL = SQL + ComNum.VBLF + "	      FROM KOSMOS_EMR.AEMRBNRACTSET A";
                        SQL = SQL + ComNum.VBLF + "         INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                        SQL = SQL + ComNum.VBLF + "            ON A.ITEMCD = B.BASCD";
                        SQL = SQL + ComNum.VBLF + "           AND B.BSNSCLS = '기록지관리'";
                        SQL = SQL + ComNum.VBLF + "           AND B.UNITCLS = '간호활동항목'";
                        SQL = SQL + ComNum.VBLF + "           AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "           (";
                        SQL = SQL + ComNum.VBLF + "             SELECT 1";
                        SQL = SQL + ComNum.VBLF + "               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M";
                        SQL = SQL + ComNum.VBLF + "                 INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                        SQL = SQL + ComNum.VBLF + "                    ON M.EMRNO = R.EMRNO";
                        SQL = SQL + ComNum.VBLF + "                   AND M.EMRNOHIS = R.EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "              WHERE M.MEDFRDATE = '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "                AND M.PTNO = '" + p.ptNo + "'";
                        SQL = SQL + ComNum.VBLF + "                AND M.CHARTDATE = A.CHARTDATE";
                        SQL = SQL + ComNum.VBLF + "                AND R.ITEMCD = A.ITEMCD";
                        SQL = SQL + ComNum.VBLF + "           )";
                        SQL = SQL + ComNum.VBLF + "      WHERE A.ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVAL > 1";
                        SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVALCD = '일'";
                        SQL = SQL + ComNum.VBLF + "        AND A.CHARTDATE >= '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "      GROUP BY A.ACPNO, A.PTNO, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT";
                        SQL = SQL + ComNum.VBLF + " ) A";
                        SQL = SQL + ComNum.VBLF + " WHERE (TO_DATE('" + strChartDate + "', 'YYYYMMDD')  - A.CHARTDATE) = A.ACTINTERVAL ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                        SQL = SQL + ComNum.VBLF + "   (";
                        SQL = SQL + ComNum.VBLF + "     SELECT 1";
                        SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBNRACTSET";
                        SQL = SQL + ComNum.VBLF + "      WHERE ITEMCD = A.ITEMCD ";
                        SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = '" + strChartDate + "'";
                        SQL = SQL + ComNum.VBLF + "        AND ACPNO  = A.ACPNO ";
                        SQL = SQL + ComNum.VBLF + "   )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "초기값 세팅중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBoxEx(this, ex.Message);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

                Cursor.Current = Cursors.Default;
                return;
            }
            dt.Dispose();
            dt = null;

            #region //기본값 설정
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {


                SQL = "";
                SQL = "SELECT B.BASVAL, B.BASCD, BASNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBNRACTSET AA ";
                SQL = SQL + ComNum.VBLF + "                                    WHERE AA.ACPNO = " + p.acpNo ;
                SQL = SQL + ComNum.VBLF + "                                        AND AA.CHARTDATE < '" + strChartDate + "')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "초기값 세팅중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    #region 1일 항목이거나 일단위가 아닐경우.
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO  " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "   DISTINCT A.ACPNO, A.PTNO, ";
                    SQL = SQL + ComNum.VBLF + "   '" + strChartDate + "' AS CHARTDATE,  ";
                    SQL = SQL + ComNum.VBLF + "   A.ITEMCD, A.ACTINTERVAL, A.ACTINTERVALCD,  ";
                    SQL = SQL + ComNum.VBLF + "   A.ACTCNT,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + strWDate + "' AS WRITEDATE,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + strWTime + "' AS WRITETIME,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + clsType.User.IdNumber + "' AS WRITEUSEID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                    SQL = SQL + ComNum.VBLF + "   ON A.ITEMCD = B.BASCD";
                    SQL = SQL + ComNum.VBLF + "  AND B.BSNSCLS = '기록지관리'";
                    SQL = SQL + ComNum.VBLF + "  AND B.UNITCLS = '간호활동항목'";
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "  AND ((A.ACTINTERVAL <= 1 AND A.ACTINTERVALCD = '일') OR A.ACTINTERVALCD  <> '일')";
                    SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBNRACTSET AA ";
                    SQL = SQL + ComNum.VBLF + "                                    WHERE AA.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                        AND AA.CHARTDATE < '" + strChartDate + "')";
                    SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "   (";
                    SQL = SQL + ComNum.VBLF + "     SELECT 1";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBNRACTSET";
                    SQL = SQL + ComNum.VBLF + "      WHERE ITEMCD = A.ITEMCD ";
                    SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = '" + strChartDate + "'";
                    SQL = SQL + ComNum.VBLF + "        AND ACPNO  = A.ACPNO ";
                    SQL = SQL + ComNum.VBLF + "   )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "초기값 세팅중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    #region 일단위이고 1일 이후 항목들 간격 날짜만큼 지났을경우 

                    SQL = "WITH IPD_TRANS_CHK AS ";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "  SELECT MAX(TRSDATE) TRSDATE, TOWARD";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.IPD_TRANSFOR I";
                    SQL = SQL + ComNum.VBLF + "   WHERE IPDNO = " + p.acpNoIn;
                    SQL = SQL + ComNum.VBLF + "   GROUP BY TOWARD ";
                    SQL = SQL + ComNum.VBLF + "   UNION ALL";
                    SQL = SQL + ComNum.VBLF + "  SELECT TO_DATE('" + strChartDate + "'), '" + p.ward + "' TOWARD";
                    SQL = SQL + ComNum.VBLF + "    FROM DUAL";
                    SQL = SQL + ComNum.VBLF + "   WHERE NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "   (";
                    SQL = SQL + ComNum.VBLF + "    SELECT 1";
                    SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_TRANSFOR I";
                    SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = " + p.acpNoIn;
                    SQL = SQL + ComNum.VBLF + "   )";
                    SQL = SQL + ComNum.VBLF + ")";

                    SQL = SQL + ComNum.VBLF + " SELECT 1 ";
                    SQL = SQL + ComNum.VBLF + " FROM DUAL";
                    SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
                    SQL = SQL + ComNum.VBLF + " (";
                    SQL = SQL + ComNum.VBLF + "     SELECT 1 ";
                    SQL = SQL + ComNum.VBLF + "     FROM";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SELECT";
                    SQL = SQL + ComNum.VBLF + "	                  A.ACPNO";
                    SQL = SQL + ComNum.VBLF + "	                , A.PTNO";
                    SQL = SQL + ComNum.VBLF + "	       	        , A.ITEMCD";
                    SQL = SQL + ComNum.VBLF + "	                , A.ACTINTERVAL";
                    SQL = SQL + ComNum.VBLF + "	                , A.ACTINTERVALCD";
                    SQL = SQL + ComNum.VBLF + "	                , A.ACTCNT";
                    SQL = SQL + ComNum.VBLF + "	                , TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD') AS CHARTDATE";
                    SQL = SQL + ComNum.VBLF + "	          FROM KOSMOS_EMR.AEMRBNRACTSET A";
                    SQL = SQL + ComNum.VBLF + "             INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                    SQL = SQL + ComNum.VBLF + "                ON A.ITEMCD = B.BASCD";
                    SQL = SQL + ComNum.VBLF + "               AND B.BSNSCLS = '기록지관리'";
                    SQL = SQL + ComNum.VBLF + "               AND B.UNITCLS = '간호활동항목'";
                    SQL = SQL + ComNum.VBLF + "               AND EXISTS";
                    SQL = SQL + ComNum.VBLF + "               (";
                    SQL = SQL + ComNum.VBLF + "                 SELECT 1";
                    SQL = SQL + ComNum.VBLF + "                   FROM " + ComNum.DB_EMR + "AEMRCHARTMST M";
                    SQL = SQL + ComNum.VBLF + "                     INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                    SQL = SQL + ComNum.VBLF + "                        ON M.EMRNO = R.EMRNO";
                    SQL = SQL + ComNum.VBLF + "                       AND M.EMRNOHIS = R.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                  WHERE M.MEDFRDATE = '" + p.medFrDate + "'";
                    SQL = SQL + ComNum.VBLF + "                    AND M.PTNO = '" + p.ptNo + "'";
                    SQL = SQL + ComNum.VBLF + "                    AND M.CHARTDATE = A.CHARTDATE";
                    SQL = SQL + ComNum.VBLF + "                    AND R.ITEMCD = A.ITEMCD";
                    SQL = SQL + ComNum.VBLF + "               )";
                    SQL = SQL + ComNum.VBLF + "          WHERE A.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "            AND A.ACTINTERVAL > 1";
                    SQL = SQL + ComNum.VBLF + "            AND A.ACTINTERVALCD = '일'";
                    SQL = SQL + ComNum.VBLF + "            AND A.CHARTDATE >= '" + p.medFrDate + "'";
                    SQL = SQL + ComNum.VBLF + "          GROUP BY A.ACPNO, A.PTNO, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT";
                    SQL = SQL + ComNum.VBLF + "     ) A";
                    SQL = SQL + ComNum.VBLF + "     WHERE (TO_DATE('" + strChartDate + "', 'YYYYMMDD')  - A.CHARTDATE) = A.ACTINTERVAL ";
                    SQL = SQL + ComNum.VBLF + "       AND EXISTS";
                    SQL = SQL + ComNum.VBLF + "       (";
                    SQL = SQL + ComNum.VBLF + "         SELECT 1";
                    SQL = SQL + ComNum.VBLF + "           FROM IPD_TRANS_CHK";
                    //SQL = SQL + ComNum.VBLF + "          WHERE ((A.CHARTDATE  - TRUNC(TRSDATE)) >= A.ACTINTERVAL OR A.CHARTDATE = TRUNC(TRSDATE) OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') = TRUNC(TRSDATE))";
                    SQL = SQL + ComNum.VBLF + "          WHERE (A.CHARTDATE  - TRUNC(TRSDATE) >= A.ACTINTERVAL OR A.CHARTDATE = TRUNC(TRSDATE) ";
                    SQL = SQL + ComNum.VBLF + "              OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') = TRUNC(TRSDATE)";
                    SQL = SQL + ComNum.VBLF + "              OR TO_DATE('" + strChartDate + "', 'YYYYMMDD') - A.CHARTDATE = A.ACTINTERVAL) ";
                    SQL = SQL + ComNum.VBLF + "            AND TOWARD = '" + p.ward + "'";
                    SQL = SQL + ComNum.VBLF + "       )";
                    SQL = SQL + ComNum.VBLF + "       AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "       (";
                    SQL = SQL + ComNum.VBLF + "         SELECT 1";
                    SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRBNRACTSET";
                    SQL = SQL + ComNum.VBLF + "          WHERE ITEMCD = A.ITEMCD ";
                    SQL = SQL + ComNum.VBLF + "            AND CHARTDATE = '" + strChartDate + "'";
                    SQL = SQL + ComNum.VBLF + "            AND ACPNO  = A.ACPNO ";
                    SQL = SQL + ComNum.VBLF + "       )";
                    SQL = SQL + ComNum.VBLF + " )";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "초기값 세팅중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {

                        #region INSERT 
                        SQL = "INSERT INTO  " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "   DISTINCT A.ACPNO, A.PTNO, ";
                        SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "' AS CHARTDATE,  ";
                        SQL = SQL + ComNum.VBLF + "    A.ITEMCD, A.ACTINTERVAL, A.ACTINTERVALCD,  ";
                        SQL = SQL + ComNum.VBLF + "    A.ACTCNT,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + strWDate + "' AS WRITEDATE,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + strWTime + "' AS WRITETIME,  ";
                        SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' AS WRITEUSEID ";
                        SQL = SQL + ComNum.VBLF + " FROM";
                        SQL = SQL + ComNum.VBLF + " (";
                        SQL = SQL + ComNum.VBLF + "     SELECT";
                        SQL = SQL + ComNum.VBLF + "	              A.ACPNO";
                        SQL = SQL + ComNum.VBLF + "	            , A.PTNO";
                        SQL = SQL + ComNum.VBLF + "	   	        , A.ITEMCD";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVAL";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVALCD";
                        SQL = SQL + ComNum.VBLF + "	            , A.ACTCNT";
                        SQL = SQL + ComNum.VBLF + "	            , TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD') AS CHARTDATE";
                        SQL = SQL + ComNum.VBLF + "	      FROM KOSMOS_EMR.AEMRBNRACTSET A";
                        SQL = SQL + ComNum.VBLF + "         INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                        SQL = SQL + ComNum.VBLF + "            ON A.ITEMCD = B.BASCD";
                        SQL = SQL + ComNum.VBLF + "           AND B.BSNSCLS = '기록지관리'";
                        SQL = SQL + ComNum.VBLF + "           AND B.UNITCLS = '간호활동항목'";
                        SQL = SQL + ComNum.VBLF + "           AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "           (";
                        SQL = SQL + ComNum.VBLF + "             SELECT 1";
                        SQL = SQL + ComNum.VBLF + "               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M";
                        SQL = SQL + ComNum.VBLF + "                 INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                        SQL = SQL + ComNum.VBLF + "                    ON M.EMRNO = R.EMRNO";
                        SQL = SQL + ComNum.VBLF + "                   AND M.EMRNOHIS = R.EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "              WHERE M.MEDFRDATE = '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "                AND M.PTNO = '" + p.ptNo + "'";
                        SQL = SQL + ComNum.VBLF + "                AND M.CHARTDATE = A.CHARTDATE";
                        SQL = SQL + ComNum.VBLF + "                AND R.ITEMCD = A.ITEMCD";
                        SQL = SQL + ComNum.VBLF + "           )";
                        SQL = SQL + ComNum.VBLF + "      WHERE A.ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVAL > 1";
                        SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVALCD = '일'";
                        SQL = SQL + ComNum.VBLF + "        AND A.CHARTDATE >= '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "      GROUP BY A.ACPNO, A.PTNO, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT";
                        SQL = SQL + ComNum.VBLF + " ) A";
                        SQL = SQL + ComNum.VBLF + " WHERE (TO_DATE('" + strChartDate + "', 'YYYYMMDD')  - A.CHARTDATE) = A.ACTINTERVAL ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                        SQL = SQL + ComNum.VBLF + "   (";
                        SQL = SQL + ComNum.VBLF + "     SELECT 1";
                        SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBNRACTSET";
                        SQL = SQL + ComNum.VBLF + "      WHERE ITEMCD = A.ITEMCD ";
                        SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = '" + strChartDate + "'";
                        SQL = SQL + ComNum.VBLF + "        AND ACPNO  = A.ACPNO ";
                        SQL = SQL + ComNum.VBLF + "   )";

                        #endregion

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "초기값 세팅중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                    #endregion

                    #region 일단위이고 1일 이후 항목들 간격 날짜만큼 지났을경우 
                    //SQL = "";
                    //SQL = SQL + ComNum.VBLF + "INSERT INTO  " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                    //SQL = SQL + ComNum.VBLF + " SELECT ";
                    //SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.PTNO, ";
                    //SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "' AS CHARTDATE,  ";
                    //SQL = SQL + ComNum.VBLF + "    A.ITEMCD, A.ACTINTERVAL, A.ACTINTERVALCD,  ";
                    //SQL = SQL + ComNum.VBLF + "    A.ACTCNT,  ";
                    //SQL = SQL + ComNum.VBLF + "    '" + strWDate + "' AS WRITEDATE,  ";
                    //SQL = SQL + ComNum.VBLF + "    '" + strWTime + "' AS WRITETIME,  ";
                    //SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' AS WRITEUSEID ";
                    //SQL = SQL + ComNum.VBLF + " FROM";
                    //SQL = SQL + ComNum.VBLF + " (";
                    //SQL = SQL + ComNum.VBLF + "     SELECT";
                    //SQL = SQL + ComNum.VBLF + "	              A.ACPNO";
                    //SQL = SQL + ComNum.VBLF + "	            , A.PTNO";
                    //SQL = SQL + ComNum.VBLF + "	   	        , A.ITEMCD";
                    //SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVAL";
                    //SQL = SQL + ComNum.VBLF + "	            , A.ACTINTERVALCD";
                    //SQL = SQL + ComNum.VBLF + "	            , A.ACTCNT";
                    //SQL = SQL + ComNum.VBLF + "	            , TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD') AS CHARTDATE";
                    //SQL = SQL + ComNum.VBLF + "	      FROM KOSMOS_EMR.AEMRBNRACTSET A";
                    //SQL = SQL + ComNum.VBLF + "         INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                    //SQL = SQL + ComNum.VBLF + "            ON A.ITEMCD = B.BASCD";
                    //SQL = SQL + ComNum.VBLF + "           AND B.BSNSCLS = '기록지관리'";
                    //SQL = SQL + ComNum.VBLF + "           AND B.UNITCLS = '간호활동항목'";
                    //SQL = SQL + ComNum.VBLF + "           AND EXISTS";
                    //SQL = SQL + ComNum.VBLF + "           (";
                    //SQL = SQL + ComNum.VBLF + "             SELECT 1";
                    //SQL = SQL + ComNum.VBLF + "               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M";
                    //SQL = SQL + ComNum.VBLF + "                 INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                    //SQL = SQL + ComNum.VBLF + "                    ON M.EMRNO = R.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                   AND M.EMRNOHIS = R.EMRNOHIS";
                    //SQL = SQL + ComNum.VBLF + "              WHERE M.MEDFRDATE = '" + p.medFrDate + "'";
                    //SQL = SQL + ComNum.VBLF + "                AND M.PTNO = '" + p.ptNo + "'";
                    //SQL = SQL + ComNum.VBLF + "                AND M.CHARTDATE = A.CHARTDATE";
                    //SQL = SQL + ComNum.VBLF + "                AND R.ITEMCD = A.ITEMCD";
                    //SQL = SQL + ComNum.VBLF + "           )";
                    //SQL = SQL + ComNum.VBLF + "      WHERE A.ACPNO = " + p.acpNo;
                    //SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVAL > 1";
                    //SQL = SQL + ComNum.VBLF + "        AND A.ACTINTERVALCD = '일'";
                    //SQL = SQL + ComNum.VBLF + "        AND A.CHARTDATE >= '" + p.medFrDate + "'";
                    //SQL = SQL + ComNum.VBLF + "      GROUP BY A.ACPNO, A.PTNO, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT";
                    //SQL = SQL + ComNum.VBLF + " ) A";
                    //SQL = SQL + ComNum.VBLF + " WHERE (TO_DATE('" + strChartDate + "', 'YYYYMMDD')  - A.CHARTDATE) >= A.ACTINTERVAL ";
                    //SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                    //SQL = SQL + ComNum.VBLF + "   (";
                    //SQL = SQL + ComNum.VBLF + "     SELECT 1";
                    //SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBNRACTSET";
                    //SQL = SQL + ComNum.VBLF + "      WHERE ITEMCD = A.ITEMCD ";
                    //SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = '" + strChartDate + "'";
                    //SQL = SQL + ComNum.VBLF + "        AND ACPNO  = A.ACPNO ";
                    //SQL = SQL + ComNum.VBLF + "   )";
                    #endregion

                    #endregion

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME, ";
                SQL = SQL + ComNum.VBLF + "     BT.VFLAG1, BT.NFLAG1, BT.NFLAG2, BT.NFLAG3";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "     ON BB.BASCD = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG";
                SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BG.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND BG.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND BG.UNITCLS = '간호활동그룹'";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD BT";
                SQL = SQL + ComNum.VBLF + "     ON B.VFLAG3 = BT.BASCD";
                SQL = SQL + ComNum.VBLF + "     AND BT.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "     AND BT.UNITCLS = '간호활동간격'";
                SQL = SQL + ComNum.VBLF + "WHERE BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동병동_" + p.ward + "'";
                SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BG.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "초기값 세팅중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        string strVFLAG1 = "";
                        string strACTINTERVALCD = "0001";
                        string strACTCNT = "3";
                        string strACTINTERVAL = "3";

                        strVFLAG1 = dt.Rows[i]["VFLAG1"].ToString().Trim();

                        if (dt.Rows[i]["NFLAG3"].ToString().Trim() == "0")
                        {
                            strACTINTERVAL = dt.Rows[i]["NFLAG1"].ToString().Trim();
                            strACTINTERVALCD = dt.Rows[i]["VFLAG1"].ToString().Trim();
                            strACTCNT = dt.Rows[i]["NFLAG2"].ToString().Trim();
                        }
                        else
                        {
                            strACTINTERVAL = "0";
                            strACTINTERVALCD = dt.Rows[i]["VFLAG1"].ToString().Trim();
                            strACTCNT = "0";
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
                        SQL = SQL + ComNum.VBLF + "     (ACPNO, PTNO, CHARTDATE, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES (";
                        SQL = SQL + ComNum.VBLF + "      " + p.acpNo + ","; //ACPNO
                        SQL = SQL + ComNum.VBLF + "     '" + p.ptNo + "',"; //PTNO
                        SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',"; //CHARTDATE
                        SQL = SQL + ComNum.VBLF + "     '" + dt.Rows[i]["BASCD"].ToString().Trim() + "',"; //ITEMCD
                        SQL = SQL + ComNum.VBLF + "     " + strACTINTERVAL + ","; //ACTINTERVAL
                        SQL = SQL + ComNum.VBLF + "     '" + strACTINTERVALCD + "',"; //ACTINTERVALCD
                        SQL = SQL + ComNum.VBLF + "     " + strACTCNT + ","; //ACTCNT
                        SQL = SQL + ComNum.VBLF + "     '" + strWDate + "',"; //WRITEDATE
                        SQL = SQL + ComNum.VBLF + "     '" + strWTime + "' ,"; //WRITETIME
                        SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                        SQL = SQL + ComNum.VBLF + ")";
                        
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "초기값 세팅중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
            }
            #endregion //기본값 설정
        }

        /// <summary>
        /// 데이타를 조회한다
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="Spd"></param>
        private void GetData(string strChartDate, FarPoint.Win.Spread.SheetView Spd)
        {
            if (p == null) return;

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Spd.RowCount = 0;
            if (strChartDate == dtpChartDate.Value.ToString("yyyyMMdd"))
            {
                if (ActButton != null)
                {
                    for (i = 0; i < ActButton.Length; i++)
                    {
                        ActButton[i].Dispose();
                        ActButton[i] = null;
                    }
                    ActButton = null;
                }
            }

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ACTINTERVAL, A.ACTINTERVALCD, A.ACTCNT, ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "   AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "   AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "   AND BB.UNITCLS = '간호활동그룹'";
            SQL = SQL + ComNum.VBLF + " WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + strChartDate + "')";
            SQL = SQL + ComNum.VBLF + " ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, 38);

                #region //최대 칼럼수를 결정한다

                #region //Old 
                //int intMaxCol = 0;
                //for (i = 0; i < dt.Rows.Count; i++)
                //{
                //    int intCnt = (int)VB.Val(dt.Rows[i]["ACTCNT"].ToString().Trim());
                //    if (intMaxCol < intCnt)
                //    {
                //        intMaxCol = intCnt;
                //    }
                //}
                #endregion //Old 

                int intMaxCol = 3;
                int intChartCnt = GetChartCnt(strChartDate); //수행시간 카운트

                if (intMaxCol < intChartCnt)
                {
                    intMaxCol = intChartCnt;
                }

                Spd.ColumnCount = intMaxCol + 5;
                if (intMaxCol > 0)
                {
                    Spd.Cells[0, 5, Spd.RowCount - 1, Spd.ColumnCount - 1].BackColor = mDeSelColor;

                    for (int iCol = 5; iCol < Spd.ColumnCount; iCol++)
                    {
                        Spd.SetColumnWidth(iCol, 120);  //126
                    }
                }
                #endregion //최대 칼럼수를 결정한다

                string strGRPNAME = "";
                int intS = 0;
   
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.Cells[i, 0].Text = dt.Rows[i]["VFLAG1"].ToString().Trim();
                    Spd.Cells[i, 1].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                    //Spd.Cells[i, 2].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                    Spd.Cells[i, 3].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    if (dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() == "연동" || dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() == "설정무")
                    {
                        Spd.Cells[i, 4].Text = dt.Rows[i]["ACTINTERVALCD"].ToString().Trim();
                    }
                    else
                    {
                        Spd.Cells[i, 4].Text = dt.Rows[i]["ACTINTERVAL"].ToString().Trim() + dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() + " " + dt.Rows[i]["ACTCNT"].ToString().Trim() + "회";
                    }

                    if (strGRPNAME != dt.Rows[i]["GRPNAME"].ToString().Trim())
                    {
                        Spd.Cells[i, 2].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            Spd.AddSpanCell(intS, 2, i - intS, 1);
                        }
                        intS = i;
                    }
                    strGRPNAME = dt.Rows[i]["GRPNAME"].ToString().Trim();

                    //SetActTime(dt.Rows[i]["ACTTIME"].ToString().Trim(), Spd, i);

                    GetActInfo(strChartDate, dt.Rows[i]["ITEMCD"].ToString().Trim(), Spd, i);
                }

                Spd.AddSpanCell(intS, 2, i - intS, 1);
            }
            dt.Dispose();
            dt = null;

            #region //Remove Colume
            for (i = Spd.ColumnCount - 1; i > 4; i--)
            {
                bool isExistsData = false;
                for (int j = 0; j < Spd.Rows.Count; j++)
                {
                    if (Spd.Cells[j, i].Text.Trim() != "")
                    {
                        isExistsData = true;
                        break;
                    }
                }

                if (isExistsData == false)
                {
                    Spd.Columns[i].Remove();
                }
            }
            #endregion //Remove Colume

            if (strChartDate == dtpChartDate.Value.ToString("yyyyMMdd"))
            {
                GetSetActInfoButton();

                txtChartTimeTextChanged();
            }
        }

        /// <summary>
        /// 수행시간 카운트
        /// </summary>
        /// <returns></returns>
        private int GetChartCnt(string strChartDate)
        {
            int rtnVal = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "GROUP BY CHARTTIME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return 0;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return 0;
                }

                rtnVal = dt.Rows.Count; 

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 액팅 했는 시간 버튼을 만든다.
        /// </summary>
        private void GetSetActInfoButton()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ActButton != null)
            {
                for (i = 0; i < ActButton.Length; i++)
                {
                    ActButton[i].Dispose();
                    ActButton[i] = null;
                }
                ActButton = null;
            }

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST C  ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC ";
            

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

            ActButton = new Button[dt.Rows.Count];
            int intWidth = 50;
            int intLocationX = 4;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ActButton[i] = new Button();

                ActButton[i].Name = "ActButton_" + VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4);
                ActButton[i].Size = new System.Drawing.Size(intWidth, 24);
                ActButton[i].TabIndex = 100 + i;
                ActButton[i].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                ActButton[i].UseVisualStyleBackColor = true;
                ActButton[i].Click += new System.EventHandler(btnActInfo_Click);
                
                ActButton[i].Location = new System.Drawing.Point(intLocationX + (i * intWidth), 4);
                ActButton[i].Dock = System.Windows.Forms.DockStyle.Left;

                this.Controls.Add(ActButton[i]);
                ActButton[i].Parent = this.panTodayHead;
                ActButton[i].Visible = true;
                ActButton[i].BringToFront();
            }
            dt.Dispose();
            dt = null;
        }

        private void btnActInfo_Click(object sender, EventArgs e)
        {
            string strTime = ((Button)sender).Text.Trim();
            txtChartTime.Text = strTime;
            txtChartTimeTextChanged();
        }

        /// <summary>
        /// 액팅시간을 세팅한다 : 사용안함
        /// </summary>
        /// <param name="strActTime"></param>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        private void SetActTime(string strActTime, FarPoint.Win.Spread.SheetView Spd, int Row)
        {
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            MatchCollection matches = Regex.Matches(strActTime, ",");
            int intMaxCol = matches.Count + 1;

            for (int iCol = 0; iCol < intMaxCol; iCol++)
            {
                //Spd.SetColumnWidth(iCol + 3, 126);
                Spd.Cells[Row, iCol + 5].CellType = TypeText;
                Spd.Cells[Row, iCol + 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                Spd.Cells[Row, iCol + 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                Spd.Cells[Row, iCol + 5].Locked = true;

                Spd.Cells[Row, iCol + 5].Text = ComFunc.SptChar(strActTime, iCol, ",");
                Spd.Cells[Row, iCol + 5].Tag = ComFunc.SptChar(strActTime, iCol, ",");

                Spd.Cells[Row, iCol + 5].BackColor = mDeSelColor;
            }
        }
        
        /// <summary>
        /// 액팅 정보를 불러온다
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        private void GetActInfo(string strChartDate, string strITEMCD, FarPoint.Win.Spread.SheetView Spd, int Row)
        {
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            #region 도뇨관 리스트
            //SQL = SQL + ComNum.VBLF + "WITH ITEM_LIST AS";
            //SQL = SQL + ComNum.VBLF + "(";
            //SQL = SQL + ComNum.VBLF + "SELECT    A.CHARTDATE";
            //SQL = SQL + ComNum.VBLF + "        , R.ITEMNO";
            //SQL = SQL + ComNum.VBLF + "        , R.ITEMVALUE";
            //SQL = SQL + ComNum.VBLF + "	        FROM KOSMOS_EMR.AEMRCHARTMST A ";
            //SQL = SQL + ComNum.VBLF + "	   	   INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            //SQL = SQL + ComNum.VBLF + "	   	      ON A.EMRNO = R.EMRNO ";
            //SQL = SQL + ComNum.VBLF + "	   		 AND A.EMRNOHIS = R.EMRNOHIS ";
            //SQL = SQL + ComNum.VBLF + "	   		 AND R.ITEMNO = '" + (strITEMCD.IndexOf("_") != -1 ? strITEMCD.Split('_')[0] : strITEMCD) + "'";
            //SQL = SQL + ComNum.VBLF + "	       WHERE A.MEDFRDATE = '" + p.medFrDate + "'";
            //SQL = SQL + ComNum.VBLF + "	   	     AND A.PTNO = '" + p.ptNo + "'";
            //SQL = SQL + ComNum.VBLF + "	   	     AND A.FORMNO IN (1575, 3150)";
            //SQL = SQL + ComNum.VBLF + ")";
            #endregion

            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "      C.EMRNO";
            SQL = SQL + ComNum.VBLF + "    , C.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "    , C.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "    , C.CHARTTIME ";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMNO ";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMINDEX";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMTYPE";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "    , R.ITEMVALUE1 ";
            SQL = SQL + ComNum.VBLF + "    , U.NAME AS USENAME ";
            //SQL = SQL + ComNum.VBLF + "    , CASE WHEN EXISTS(";
            //SQL = SQL + ComNum.VBLF + "         SELECT 1  ";
            //SQL = SQL + ComNum.VBLF + "		      FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
            //SQL = SQL + ComNum.VBLF + "		     WHERE EMRNO = C.EMRNO";
            //SQL = SQL + ComNum.VBLF + "			   AND ITEMCD    = '" + strITEMCD + "'";
            //SQL = SQL + ComNum.VBLF + "			   AND ITEMVALUE = R.ITEMVALUE";
            //SQL = SQL + ComNum.VBLF + "			   AND GBSTATUS = ' '";
            //SQL = SQL + ComNum.VBLF + "		 ) THEN '1' END ORDER_CHECK";

            SQL = SQL + ComNum.VBLF + ", (                                                                                                            ";
            SQL = SQL + ComNum.VBLF + " SELECT LISTAGG(S.SUNAMEK || ' ' || QTY || ' (' || H.EMP_NM  || ')', '\r\n') WITHIN GROUP(ORDER BY O.ORDERNO)  ";
            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER O                                                                     ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_OCS.OCS_IORDER O2                                                                       ";
            SQL = SQL + ComNum.VBLF + "        ON O.ORDERNO  = O2.ORDERNO                                                                             ";
            SQL = SQL + ComNum.VBLF + "       AND O2.PTNO  = '" + p.ptNo + "'                                                                         ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_PMPA.BAS_SUN S                                                                          ";
            SQL = SQL + ComNum.VBLF + "        ON S.SUNEXT = O2.SUCODE                                                                                ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_ERP.HR_EMP_BASIS H                                                                      ";
            SQL = SQL + ComNum.VBLF + "        ON H.EMP_ID   = O.WRITEUSEID                                                                             ";
            SQL = SQL + ComNum.VBLF + "   WHERE O.EMRNO      = C.EMRNO";
            SQL = SQL + ComNum.VBLF + "     AND O.ITEMCD     = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "     AND O.ITEMVALUE  = R.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "     AND O.GBSTATUS = ' '                                                                                      ";
            SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                                                     ";



            #region 유치도뇨관 제거, 유지, 등등
            //SQL = SQL + ComNum.VBLF + "    , CASE WHEN EXISTS(";
            //SQL = SQL + ComNum.VBLF + "         SELECT 1  ";
            //SQL = SQL + ComNum.VBLF + "		      FROM KOSMOS_EMR.AEMRBASCD";
            //SQL = SQL + ComNum.VBLF + "		     WHERE BSNSCLS  ='기록지관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND UNITCLS  ='도뇨관관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND BASCD = R.ITEMNO) THEN";
            //SQL = SQL + ComNum.VBLF + "		 (  SELECT ";
            //SQL = SQL + ComNum.VBLF + "			   	   CASE WHEN MAX(CHARTDATE) IS NOT NULL THEN '삽입일 - ' || MAX(CHARTDATE) END A";
            //SQL = SQL + ComNum.VBLF + "			        FROM ITEM_LIST R";
            //SQL = SQL + ComNum.VBLF + "			   	   WHERE R.ITEMNO = '" + (strITEMCD.IndexOf("_") != -1 ? strITEMCD.Split('_')[0] : strITEMCD) + "'";
            //SQL = SQL + ComNum.VBLF + "			   		 AND (UPPER(R.ITEMVALUE) LIKE '%START%' OR R.ITEMVALUE LIKE '%삽입%' OR R.ITEMVALUE LIKE '%교체%' OR R.ITEMVALUE LIKE '%교환%' OR R.ITEMVALUE LIKE '%at%')";
            //SQL = SQL + ComNum.VBLF + "		 ) END INSERTVAL";

            //SQL = SQL + ComNum.VBLF + "    , CASE WHEN EXISTS(";
            //SQL = SQL + ComNum.VBLF + "         SELECT 1  ";
            //SQL = SQL + ComNum.VBLF + "		      FROM KOSMOS_EMR.AEMRBASCD";
            //SQL = SQL + ComNum.VBLF + "		     WHERE BSNSCLS  ='기록지관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND UNITCLS  ='도뇨관관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND BASCD = R.ITEMNO) THEN";
            //SQL = SQL + ComNum.VBLF + "		 (  SELECT ";
            //SQL = SQL + ComNum.VBLF + "			   	   CASE WHEN MAX(CHARTDATE) IS NOT NULL THEN '유지일 - ' || (TRUNC(SYSDATE + 1) - TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD')) END A";
            //SQL = SQL + ComNum.VBLF + "			        FROM ITEM_LIST R";
            //SQL = SQL + ComNum.VBLF + "			   	   WHERE R.ITEMNO = '" + (strITEMCD.IndexOf("_") != -1 ? strITEMCD.Split('_')[0] : strITEMCD) + "'";
            //SQL = SQL + ComNum.VBLF + "			   		 AND (UPPER(R.ITEMVALUE) LIKE '%START%' OR R.ITEMVALUE LIKE '%삽입%' OR R.ITEMVALUE LIKE '%교체%' OR R.ITEMVALUE LIKE '%교환%' OR R.ITEMVALUE LIKE '%at%')";
            //SQL = SQL + ComNum.VBLF + "		 ) END USEVAL";

            //SQL = SQL + ComNum.VBLF + "    , CASE WHEN EXISTS(";
            //SQL = SQL + ComNum.VBLF + "         SELECT 1  ";
            //SQL = SQL + ComNum.VBLF + "		      FROM KOSMOS_EMR.AEMRBASCD";
            //SQL = SQL + ComNum.VBLF + "		     WHERE BSNSCLS  ='기록지관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND UNITCLS  ='도뇨관관리'";
            //SQL = SQL + ComNum.VBLF + "			   AND BASCD = R.ITEMNO) THEN";
            //SQL = SQL + ComNum.VBLF + "		 (  SELECT ";
            //SQL = SQL + ComNum.VBLF + "			   	   CASE WHEN MAX(CHARTDATE) IS NOT NULL THEN '최근 제거일 - ' || MAX(CHARTDATE) END AA";
            //SQL = SQL + ComNum.VBLF + "			        FROM ITEM_LIST R";
            //SQL = SQL + ComNum.VBLF + "			   	   WHERE R.ITEMNO = '" + (strITEMCD.IndexOf("_") != -1 ? strITEMCD.Split('_')[0] : strITEMCD) + "'";
            //SQL = SQL + ComNum.VBLF + "			   		 AND (R.ITEMVALUE LIKE '%제거%' OR UPPER(R.ITEMVALUE) LIKE '%REMOVE%')";
            //SQL = SQL + ComNum.VBLF + "		 ) END DELVAL";
            #endregion

            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "       ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "      AND C.EMRNOHIS = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "      AND R.ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_EMR + "EMR_USERT U ";
            SQL = SQL + ComNum.VBLF + "       ON U.USERID = C.CHARTUSEID ";
            SQL = SQL + ComNum.VBLF + " WHERE C.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "   AND C.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "   AND C.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY C.CHARTTIME, R.DSPSEQ ";

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

            int iCol = 5;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (iCol + i >= Spd.ColumnCount)
                {
                    Spd.ColumnCount += 1;
                }

                Spd.Cells[Row, iCol + i].CellType = TypeText;
                Spd.Cells[Row, iCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                Spd.Cells[Row, iCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                Spd.Cells[Row, iCol + i].Locked = true;

                Spd.Cells[Row, iCol + i].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                Spd.Cells[Row, iCol + i].Tag = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                if ( (dt.Rows[i]["ITEMVALUE"].ToString().Trim() + dt.Rows[i]["ITEMVALUE1"].ToString().Trim()).Length > 0)
                {
                    Spd.Cells[Row, iCol + i].Text = Spd.Cells[Row, iCol + i].Text + " (" + dt.Rows[i]["USENAME"].ToString().Trim() + ")" 
                        + ComNum.VBLF + VB.Left(dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim(), 12);
                    Spd.Cells[Row, iCol + i].Tag = Spd.Cells[Row, iCol + i].Text + " (" + dt.Rows[i]["USENAME"].ToString().Trim() + ")"
                        + ComNum.VBLF + dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    Spd.Rows[Row].Height = Spd.Rows[Row].GetPreferredHeight() + 4;
                }
                else
                {
                    Spd.Cells[Row, iCol + i].Text = Spd.Cells[Row, iCol + i].Text + " (" + dt.Rows[i]["USENAME"].ToString().Trim() + ")";
                    Spd.Rows[Row].Height = Spd.Rows[Row].GetPreferredHeight() + 4;
                }
                Spd.Cells[Row, iCol + i].BackColor = mSelColor;

                #region 식이!
                if (dt.Rows[i]["ITEMCD"].ToString().Trim().Equals("I0000001465") ||
                    dt.Rows[i]["ITEMCD"].ToString().Trim().Equals("I0000037841"))
                {
                    Spd.Cells[Row, iCol + i].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                    Spd.Cells[Row, iCol + i].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                    Spd.Cells[Row, iCol + i].NoteIndicatorColor = Color.Pink;
                    Spd.Cells[Row, iCol + i].NoteIndicatorSize = new Size(20, 20);
                    Spd.Cells[Row, iCol + i].Note = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();

                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                    nsinfo = Spd.GetStickyNoteStyleInfo(Row, iCol + i);
                    //nsinfo.BackColor = Color.Red;
                    nsinfo.Font = Notefont;
                    nsinfo.ForeColor = Color.Black;
                    nsinfo.Width = 300; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                    nsinfo.Height = 100; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                    nsinfo.ShapeOutlineColor = Color.Red;
                    nsinfo.ShapeOutlineThickness = 1;
                    nsinfo.ShadowOffsetX = 3;
                    nsinfo.ShadowOffsetY = 3;
                    Spd.SetStickyNoteStyleInfo(Row, iCol + i, nsinfo);
                }
                #endregion

                #region 처방 체크
                if (dt.Rows[i]["NOTE"].ToString().NotEmpty())
                {
                    Spd.Cells[Row, iCol + i].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                    Spd.Cells[Row, iCol + i].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                    Spd.Cells[Row, iCol + i].NoteIndicatorColor = Color.Pink;
                    Spd.Cells[Row, iCol + i].NoteIndicatorSize = new Size(20, 20);
                    Spd.Cells[Row, iCol + i].Note = dt.Rows[i]["NOTE"].ToString().Trim();

                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo2 = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                    //nsinfo.BackColor = Color.Red;
                    nsinfo2.Font = Notefont;
                    nsinfo2.ForeColor = Color.Black;
                    nsinfo2.Width = 400; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                    nsinfo2.Height = 100; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                    nsinfo2.ShapeOutlineColor = Color.Red;
                    nsinfo2.ShapeOutlineThickness = 1;
                    nsinfo2.ShadowOffsetX = 3;
                    nsinfo2.ShadowOffsetY = 3;
                    Spd.SetStickyNoteStyleInfo(Row, iCol + i, nsinfo2);
                }
                #endregion
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 콤보에 시간을 세팅한다
        /// </summary>
        private void SetTimeCombo()
        {
            if (p == null) return;

            int i = 0;

            txtChartTime.Items.Clear();

            if (p.ward.Trim() == "33" || p.ward.Trim() == "35")
            {
                for (i = 0; i < 24; i++)
                {
                    txtChartTime.Items.Add(VB.Format(i, "00") + ":00");
                }
            }
            else
            {
                txtChartTime.Items.Add("08:00");
                txtChartTime.Items.Add("13:00");
                txtChartTime.Items.Add("19:00");
            }
        }

        /// <summary>
        /// 액팅정보 조회 : 사용안함
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        private void GetActInfoOld(string strChartDate, string strITEMCD, FarPoint.Win.Spread.SheetView Spd, int Row)
        {
            //CNT에 따라서 시간을 세팅을 한다
            //액팅정보를 가지고 온다.

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT C.EMRNO,";
            SQL = SQL + ComNum.VBLF + "    A.NRACTSEQ, A.ACTGB, ";
            SQL = SQL + ComNum.VBLF + "    A.ACTTIME, A.ACTUSEID, ";
            SQL = SQL + ComNum.VBLF + "    U.USENAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRNRACT A ";
            SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.DCCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
            SQL = SQL + ComNum.VBLF + "    ON U.USEID = A.ACTUSEID ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY A.ACTTIME ASC";

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (int iCol = 5; iCol < Spd.ColumnCount; iCol++)
                {
                    if (Spd.Cells[Row, iCol].Text.Trim() == ComFunc.FormatStrToDate(dt.Rows[i]["ACTTIME"].ToString().Trim(), "M"))
                    {
                        //Spd.Cells[Row, iCol].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ACTTIME"].ToString().Trim(), "M") + " (" + dt.Rows[i]["USENAME"].ToString().Trim() + ")";
                        //Spd.Cells[Row, iCol].Text = Spd.Cells[Row, iCol].Text + ComNum.VBLF;
                        //Spd.Cells[Row, iCol].Text = Spd.Cells[Row, iCol].Text + dt.Rows[i]["ACTGB"].ToString().Trim();
                        Spd.Cells[Row, iCol].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ACTTIME"].ToString().Trim(), "M");
                        Spd.Cells[Row, iCol].Text = Spd.Cells[Row, iCol].Text + ComNum.VBLF;
                        Spd.Cells[Row, iCol].Text = Spd.Cells[Row, iCol].Text + " (" + dt.Rows[i]["USENAME"].ToString().Trim() + ")";
                        Spd.Cells[Row, iCol].BackColor = mSelColor;
                        break;
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void btnSaveDefault_Click(object sender, EventArgs e)
        {
            using (frmEmrBaseItemSetBaseValue frm = new frmEmrBaseItemSetBaseValue("ACT"))
            {
                frm.TopMost = true;
                frm.ShowDialog(this);
            }

            ssChart_Sheet1.RowCount = 0;
            ssChart_Sheet1.ColumnCount = 5;
            GetData(dtpChartDate.Value.ToString("yyyyMMdd"), ssChart_Sheet1);
        }

        private void btnSizeLeftS_Click(object sender, EventArgs e)
        {
            panYesterday.Width = 76;
        }

        private void btnSizeLeftB_Click(object sender, EventArgs e)
        {
            panTommrrow.Width = 76;
            panYesterday.Width = 76 + panToday.Width;
        }

        private void btnSizeRightB_Click(object sender, EventArgs e)
        {
            panYesterday.Width = 76;
            panTommrrow.Width = 76 + panToday.Width;
        }

        private void btnSizeRightS_Click(object sender, EventArgs e)
        {
            panTommrrow.Width = 76;
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            DateTime dtpNow = DateTime.ParseExact(dtpChartDate.Value.ToString("yyyyMMdd") + VB.Left(txtChartTime.Text.Trim().Replace(":", ""), 4), "yyyyMMddHHmm", null);

            if (clsType.User.IdNumber.Equals("8822") == false && (dtpSys - dtpNow).TotalMinutes < -60)
            {
                ComFunc.MsgBoxEx(this, "현재시간에서 1시간 이후 까지만 수행 할 수 있습니다.");
                return;
            }

            string strITEMTIME = txtChartTime.Text.Trim();
            string strITEMCD = "";
            for (int i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                if (ssChart_Sheet1.Cells[i, 3].BackColor == mSelColor)
                {
                    strITEMCD = strITEMCD + ssChart_Sheet1.Cells[i, 1].Text.Trim() + ",";
                }
            }

            if (strITEMCD.Trim() == "" )
            {
                ComFunc.MsgBoxEx(this, "수행할 항목을 선택해 주십시요.");
            }

            if (CheckIsActivedItem() == false)
            {
                ComFunc.MsgBoxEx(this, "선택된 항목이 이전 수행한 항목 보다 적습니다." + ComNum.VBLF + "확인 바랍니다.");
                txtChartTimeTextChanged();
                return;
            }
            
            if (!string.IsNullOrWhiteSpace(strITEMCD))
            {
                strITEMCD = VB.Left(strITEMCD, strITEMCD.Length - 1);
            }

            using (frmNrActingItemNew1 frm = new frmNrActingItemNew1(p, dtpChartDate.Value.ToString("yyyyMMdd"), strITEMTIME, mstrFormNo, mstrUpdateNo, strITEMCD))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.rEventSendave += frmNrActingItemNew1_rEventSendave;
                frm.ShowDialog(this);
            }

            ssChart_Sheet1.RowCount = 0;
            ssChart_Sheet1.ColumnCount = 5;
            GetData(dtpChartDate.Value.ToString("yyyyMMdd"), ssChart_Sheet1);
        }

        /// <summary>
        /// 저장/삭제 새로고침
        /// </summary>
        /// <param name="Save"></param>
        private void frmNrActingItemNew1_rEventSendave(bool Save)
        {
            if (Save == false)
                return;

            ssChart_Sheet1.RowCount = 0;
            ssChart_Sheet1.ColumnCount = 5;
            GetData(dtpChartDate.Value.ToString("yyyyMMdd"), ssChart_Sheet1);
        }

        private bool CheckIsActivedItem()
        {
            int i = 0;
            string strITEMTIME = txtChartTime.Text.Replace(":", "").Trim() + "00";
            string strITEMCD = "";
            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                if (ssChart_Sheet1.Cells[i, 3].BackColor == mSelColor)
                {
                    strITEMCD = strITEMCD + "'" + ssChart_Sheet1.Cells[i, 1].Text.Trim() + "',";
                }
            }

            if (strITEMCD.Length > 0)
            {
                strITEMCD = VB.Left(strITEMCD, strITEMCD.Length - 1);
            }

            if (string.IsNullOrWhiteSpace(strITEMCD))
            {
                return false;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "SELECT  ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_EMR.AEMRCHARTMST C  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRCHARTROW R  ";
            SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO  ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD NOT IN (" + strITEMCD + ") ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + dtpChartDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTTIME = '" + strITEMTIME + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return false;
            }
            dt.Dispose();
            dt = null;
            return true;
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            mMaskBox = txtChartTime;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = mMaskBox.Top - 5;
            usTimeSetEvent.Left = mMaskBox.Left;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            txtChartTime.Text = strText;
            txtChartTimeTextChanged();
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            int i = 0;
            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                ssChart_Sheet1.Cells[i, 3].BackColor = mSelColor;
            }
        }

        private void btnDeSelect_Click(object sender, EventArgs e)
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            int i = 0;
            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                ssChart_Sheet1.Cells[i, 3].BackColor = mDeSelColor;
            }
        }

        private void txtChartTime_TextChanged(object sender, EventArgs e)
        {
            //if (txtChartTime.Text.Trim().IndexOf(":") < 0)
            //{
            //    txtChartTime.Text = ComFunc.FormatStrToDate(txtChartTime.Text.Trim(), "M");
            //}
            //if (ComFunc.CheckBirthDay("2020-01-01 " + txtChartTime.Text.Trim()) == false)
            //{
            //    ComFunc.MsgBoxEx(this, "시간형식이 맞지 않습니다.");
            //    txtChartTime.Focus();
            //    return;
            //}

            //txtChartTimeTextChanged();
        }

        private void txtChartTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                txtChartTimeLeave();
            }
        }

        private void txtChartTime_Leave(object sender, EventArgs e)
        {
            txtChartTimeLeave();
        }

        private void txtChartTimeLeave()
        {
            if (txtChartTime.Text.Trim().IndexOf(":") < 0)
            {
                txtChartTime.Text = ComFunc.FormatStrToDate(txtChartTime.Text.Trim(), "M");
            }
            if (ComFunc.CheckBirthDay("2020-01-01 " + txtChartTime.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "시간형식이 맞지 않습니다.");
                txtChartTime.Focus();
                return;
            }
            txtChartTimeTextChanged();
        }

        private void txtChartTime_Enter(object sender, EventArgs e)
        {
            txtChartTime.SelectAll();
        }

        private void txtChartTimeTextChanged()
        {
            if (ssChart_Sheet1.RowCount == 0)
            {
                return;
            }

            int i = 0;
            int j = 0;
            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                ssChart_Sheet1.Cells[i, 3].BackColor = mDeSelColor;
            }

            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                for (j = 5; j < ssChart_Sheet1.ColumnCount; j++)
                {
                    ssChart_Sheet1.Cells[i, j].Font = new Font("굴림", 9, FontStyle.Regular);
                }
            }

            for (i = 0; i < ssChart_Sheet1.RowCount; i++)
            {
                for (j = 5; j < ssChart_Sheet1.ColumnCount; j++)
                {
                    if (ssChart_Sheet1.Cells[i, j].Tag != null)
                    {
                        string strTag = ssChart_Sheet1.Cells[i, j].Tag.ToString();
                        string ChartTime = strTag.Substring(0, 5);
                        if (ChartTime == txtChartTime.Text.Trim())
                        {
                            ssChart_Sheet1.Cells[i, j].Font = new Font("굴림", 9, FontStyle.Bold);
                            ssChart_Sheet1.Cells[i, 3].BackColor = mSelColor;
                            break;
                        }
                    }
                }
            }
        }

        private void btnReSelect_Click(object sender, EventArgs e)
        {
            txtChartTimeTextChanged();
        }

        private void btnSaveWard_Click(object sender, EventArgs e)
        {
            using (frmNrActingSetWard frmNrActingSetWardX = new frmNrActingSetWard())
            {
                frmNrActingSetWardX.ShowDialog(this);
            }
        }


        private void btnDay_Click(object sender, EventArgs e)
        {
            txtChartTime.Text = "13:00";
            txtChartTimeTextChanged();
        }

        private void btnEvening_Click(object sender, EventArgs e)
        {
            txtChartTime.Text = "21:00";
            txtChartTimeTextChanged();
        }

        private void btnNight_Click(object sender, EventArgs e)
        {
            txtChartTime.Text = "05:00";
            txtChartTimeTextChanged();
        }
    }
}
