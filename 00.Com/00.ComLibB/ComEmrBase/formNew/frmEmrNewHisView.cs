using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 차트 수정내역 조회 용도
    /// </summary>
    public partial class frmEmrNewHisView : Form
    {
        #region 생성자
        public frmEmrNewHisView(EmrPatient AcpEmr, EmrForm pForm)
        {
            this.AcpEmr = AcpEmr;
            this.pForm = pForm;
            InitializeComponent();
        }

        public frmEmrNewHisView(EmrPatient AcpEmr, EmrForm pForm, string strEmrNo)
        {
            this.AcpEmr = AcpEmr;
            this.pForm = pForm;
            this.strEmrNo = strEmrNo;
            InitializeComponent();
        }
        #endregion

        #region 기록지 변수
        /// <summary>
        /// 기록지 폼
        /// </summary>
        Form frmEmrChart = null;
        /// <summary>
        /// 전자동의서
        /// </summary>
        frmEasViewer frmEasViewer = null;

        /// <summary>
        /// 환자 내원내역 정보
        /// </summary>
        EmrPatient AcpEmr = null;
        /// <summary>
        /// 기록지 정보
        /// </summary>
        EmrForm pForm = null;

        /// <summary>
        /// 기록지 번호
        /// </summary>
        string strEmrNo = string.Empty;
        #endregion

        #region 스프레드 enum
        /// <summary>
        /// 스프레드 칼럼
        /// </summary>
        private enum SpdView
        {
            /// <summary>
            /// 작성일자
            /// </summary>
            CHARTDATE,
            /// <summary>
            /// 작성시간
            /// </summary>
            CHARTTIME,
            /// <summary>
            /// 작성자 이름
            /// </summary>
            WRITENAME,
            /// <summary>
            /// 작성자 사번
            /// </summary>
            USERID,
            /// <summary>
            /// 차트 등록할 시점에 과
            /// </summary>
            MEDDEPTCD,
            /// <summary>
            /// 수정한 시간
            /// </summary>
            HISTORYWRITE,
            /// <summary>
            /// 서식종류 (0: 정형화서식, 2: 전자동의서, 3: 플로우시트, 4: 코딩서식지)
            /// </summary>
            FORMTYPE,
            /// <summary>
            /// KOSMOS_EMR.EMR_TREATT(TREATNO) 동일
            /// </summary>
            ACPNO,
            /// <summary>
            /// EMRNO
            /// </summary>
            EMRNO,
            /// <summary>
            /// HISTORYNO
            /// </summary>
            EMRNOHISNO,
            /// <summary>
            /// 사본발급 여부 발급시: 사 본 ELSE NULL
            /// </summary>
            PRNYN
        }
        #endregion

        #region 폼 이벤트
        private void frmEmrNewHisView_Load(object sender, EventArgs e)
        {
            lblTitle.Text = pForm.FmFORMNAME;
            if (pForm.FmOLDGB == 1)
            {
                ComFunc.MsgBoxEx(this, "신규 기록지로 작성후 수정한 내역만 표시됩니다.");
                return;
            }
            GetSearchHis();
        }

        private void frmEmrNewHisView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrChart != null)
            {
                frmEmrChart.Dispose();
                frmEmrChart = null;
            }
        }
        #endregion

        #region 함수
        /// <summary>
        /// 조회 함수
        /// </summary>
        private void GetSearchHis()
        {
            ssView_Sheet1.RowCount = 0;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            try
            {
                #region 쿼리

                SQL = ComNum.VBLF + "SELECT  ";
                SQL += ComNum.VBLF + "      CHARTDATE, CHARTTIME, NAME, CHARTUSEID, MEDDEPTCD, ";
                SQL += ComNum.VBLF + "      DC, ACPNO, EMRNO, EMRNOHIS,";
                SQL += ComNum.VBLF + "      PRNTYN, ";
                SQL += ComNum.VBLF + "      CASE WHEN FORMTYPE = '0' THEN '정형화'";
                SQL += ComNum.VBLF + "           WHEN FORMTYPE = '2' THEN '전자동'";
                SQL += ComNum.VBLF + "           WHEN FORMTYPE = '3' THEN '플로우'";
                SQL += ComNum.VBLF + "           WHEN FORMTYPE = '4' THEN '코딩형'";
                SQL += ComNum.VBLF + "      END FORMTYPE, ";
                SQL += ComNum.VBLF + "      WRTIE";
                SQL += ComNum.VBLF + "  FROM( ";
                SQL += ComNum.VBLF + "        SELECT CHARTDATE, CHARTTIME, NAME, CHARTUSEID, MEDDEPTCD, (DCDATE||' '||DCTIME) AS DC, ACPNO, EMRNO, EMRNOHIS";
                SQL += ComNum.VBLF + "        , DECODE(A.PRNTYN, 'Y', '사 본', NULL) AS PRNTYN, FORMNO, UPDATENO";
                SQL += ComNum.VBLF + "        , (WRITEDATE||' '|| RPAD(WRITETIME, 6, '0')) AS WRTIE";
                SQL += ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS A";
                SQL += ComNum.VBLF + "          INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                SQL += ComNum.VBLF + "             ON A.CHARTUSEID = B.USERID";
                if (pForm.FmFORMTYPE.Equals("2") == false)
                {
                    SQL += ComNum.VBLF + "        WHERE A.EMRNO     = " + strEmrNo;
                    SQL += ComNum.VBLF + "          AND A.EMRNOHIS  > 0 ";
                }
                else
                {
                    SQL += ComNum.VBLF + "        WHERE A.PTNO       = '" + AcpEmr.ptNo + "'";
                    SQL += ComNum.VBLF + "          AND A.FORMNO     = " + pForm.FmFORMNO;
                    SQL += ComNum.VBLF + "          AND A.MEDFRDATE  = '" + AcpEmr.medFrDate + "'";
                }
                SQL += ComNum.VBLF + "        UNION ALL ";
                SQL += ComNum.VBLF + "        SELECT TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HH24MISS') AS CHARTTIME, B.NAME,A.CREATEDUSER";
                SQL += ComNum.VBLF + "        , (SELECT MEDDEPTCD FROM KOSMOS_EMR.AEASFORMDATA WHERE ID = A.EASFORMDATA) AS MEDDEPTCD, TO_CHAR(A.CREATED, 'YYYYMMDD HH24MISS') AS DC, 0 AS ACPNO, A.EASFORMDATA as EMRNO, A.ID AS EMRNOHIS";
                SQL += ComNum.VBLF + "        , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_EMR.EMRPRTREQ WHERE EMRNO = C.ID AND SCANYN = 'E') THEN '사 본' END PRNTYN, " + pForm.FmFORMNO + ", " + pForm.FmUPDATENO;
                SQL += ComNum.VBLF + "        , TO_CHAR(A.CREATED, 'YYYYMMDD HH24MISS') WRTIE";
                SQL += ComNum.VBLF + "         FROM KOSMOS_EMR.AEASFORMDATAHISTORY A";
                SQL += ComNum.VBLF + "           INNER JOIN KOSMOS_EMR.EMR_USERT B   ";
                SQL += ComNum.VBLF + "              ON A.CREATEDUSER = B.USERID   ";
                SQL += ComNum.VBLF + "           INNER JOIN KOSMOS_EMR.AEASFORMDATA C   ";
                SQL += ComNum.VBLF + "              ON C.ID = A.EASFORMDATA   ";
                //SQL += ComNum.VBLF + "           INNER JOIN KOSMOS_EMR.AEASFORMCONTENT D   ";
                //SQL += ComNum.VBLF + "              ON D.ID = C.EASFORMCONTENT   ";
                SQL += ComNum.VBLF + "           INNER JOIN KOSMOS_EMR.AEMRFORM E   ";
                SQL += ComNum.VBLF + "              ON E.FORMNO   = " + pForm.FmFORMNO;
                SQL += ComNum.VBLF + "             AND E.UPDATENO = " + pForm.FmUPDATENO;
                SQL += ComNum.VBLF + "           WHERE A.EASFORMDATA = " + strEmrNo;
                SQL += ComNum.VBLF + "             AND C.PTNO = '" + AcpEmr.ptNo +"'";
                SQL += ComNum.VBLF + "             AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "             AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd +"'";
                SQL += ComNum.VBLF + ") A ";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRFORM B";
                SQL += ComNum.VBLF + "    ON A.FORMNO = B.FORMNO";
                SQL += ComNum.VBLF + "   AND A.UPDATENO = B.UPDATENO";
                SQL += ComNum.VBLF + "ORDER BY DC DESC-- 최신 수정내역 부터 ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        ssView_Sheet1.RowCount += 1;

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.CHARTDATE].Text    = int.Parse(reader.GetValue(0).ToString().Trim()).ToString("0000-00-00"); 
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.CHARTTIME].Text    = int.Parse(reader.GetValue(1).ToString().Trim().Substring(0, 4)).ToString("00:00");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.WRITENAME].Text    = reader.GetValue(2).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.USERID].Text       = reader.GetValue(3).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.MEDDEPTCD].Text    = reader.GetValue(4).ToString().Trim();
                        if(reader.GetValue(5) != null)
                        {
                   
                        }
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.HISTORYWRITE].Text = DateTime.ParseExact(reader.GetValue(11).ToString().Trim(), "yyyyMMdd HHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.FORMTYPE].Text     = reader.GetValue(10).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.ACPNO].Text        = reader.GetValue(6).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.EMRNO].Text        = reader.GetValue(7).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.EMRNOHISNO].Text   = reader.GetValue(8).ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, (int) SpdView.PRNYN].Text        = reader.GetValue(9).ToString().Trim();
                    }
                }

                reader.Dispose();

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
       //         ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }
        #endregion

        #region 스프레드

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
                return;

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (frmEmrChart != null)
            {
                frmEmrChart.Dispose();
                frmEmrChart = null;
            }

            if (frmEasViewer != null)
            {
                frmEasViewer.Dispose();
                frmEasViewer = null;
            }

            string strFormType = ssView_Sheet1.Cells[e.Row, (int)SpdView.FORMTYPE].Text.Trim();

            //간호기록,
            //간호활동, 임상관찰
            if (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 ||
                pForm.FmFORMNO == 2201 || pForm.FmPROGFORMNAME.Equals("frmEmrVitalSign"))
            {
                strFormType = "플로우";
            }
            else if(pForm.FmFORMNO == 1965 || pForm.FmFORMNO == 3535 || pForm.FmFORMNO == 1577 || pForm.FmFORMNO == 963) //수혈, 혈액투석
            {
                strFormType = "정형화";
            }

            if (strFormType.Equals("정형화"))//정형화
            {
                string strEmrNoHis = ssView_Sheet1.Cells[e.Row, (int)SpdView.EMRNOHISNO].Text.Trim();

                frmEmrChart = new frmEmrChartNew(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), AcpEmr, strEmrNoHis, "H");
                frmEmrChart.TopLevel = false;
                frmEmrChart.FormBorderStyle = FormBorderStyle.None;
                frmEmrChart.Parent = panForm;
                frmEmrChart.Dock = DockStyle.Fill;
                frmEmrChart.Show();
            }
            else if (strFormType.Equals("전자동"))//전자동의서
            {
                string easFormDataId = ssView_Sheet1.Cells[e.Row, (int)SpdView.EMRNO].Text.Trim();
                string easFormDataHistoryId = ssView_Sheet1.Cells[e.Row, (int)SpdView.EMRNOHISNO].Text.Trim();

                EasManager easManager = EasManager.Instance;
                frmEasViewer = easManager.GetEasFormViewer();
                //easManager.HistoryView(pForm, AcpEmr, pForm.FmFORMNO, easFormDataHistoryId);
                easManager.HistoryView(pForm, AcpEmr, pForm.FmFORMNO, easFormDataId, easFormDataHistoryId);

                frmEasViewer.TopLevel = false;
                frmEasViewer.FormBorderStyle = FormBorderStyle.None;
                frmEasViewer.Parent = panForm;
                frmEasViewer.Dock = DockStyle.Fill;
                frmEasViewer.Show();

            }
            else if (strFormType.Equals("플로우"))//플로우
            {
                string strEmrNo = ssView_Sheet1.Cells[e.Row, (int)SpdView.EMRNO].Text.Trim();

                frmEmrChart = new frmEmrChartFlowOld(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "H");
                frmEmrChart.TopLevel = false;
                frmEmrChart.FormBorderStyle = FormBorderStyle.None;
                frmEmrChart.Parent = panForm;
                frmEmrChart.Dock = DockStyle.Fill;
                frmEmrChart.Show();
            }
        }
        #endregion


        #region 버튼
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion


    }
}
