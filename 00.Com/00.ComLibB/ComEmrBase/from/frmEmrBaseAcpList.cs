using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;

namespace ComEmrBase
{
    public partial class frmEmrBaseAcpList : Form
    {

        #region //폼에서 사용하는 변수

        //EmrPatient AcpEmr = null; //외부에서 전달받은 환자 정보
        /// <summary>
        /// 뷰어용 환자정보
        /// </summary>
        EmrPatient pView = null; //뷰어용 환자정보
        /// <summary>
        /// 복사 신청용 환자정보
        /// </summary>
        EmrPatient pCopy = null; //복사 신청용 환자정보
        /// <summary>
        /// 기록지 폼 정보
        /// </summary>
        EmrForm fView = null;

        /// <summary>
        /// 저장 후 폼 자동 로드
        /// </summary>
        string mstrFormName = string.Empty;

        /// <summary>
        /// 등록번호
        /// </summary>
        string mPTNO = string.Empty;
        /// <summary>
        /// 정신과차트조회권한
        /// </summary>
        bool mViewNpChart = false;
        /// <summary>
        /// 가정의학과 지정된 환자 조회권한
        /// </summary>
        bool GbViewFMChart = false;

        string sKeyHead = "^";
        //private int nImage = 0;
        //private int nSelectedImage = 1;
        //private int nImageSaved = 2;
        //private int nSelectedImageSaved = 3;
        Font boldFont = new Font("맑은 고딕", 10, FontStyle.Bold);
        Font RegularFont = new Font("맑은 고딕", 10, FontStyle.Regular);

        #endregion //폼에서 사용하는 변수

        #region //임시변수
        string gJinGubun = "";
        string gJinState = "";
        private bool FindScanImageYn(string strInOutCls, string strMedFrDate, string strMedDeptCd, string strMedMedDrCd, string strMedEndDate)
        {
            return false;
        }
        #endregion //임시변수

        #region 마지막 클릭 내원내역 정보
        string mInOutCls = string.Empty;
        string mFrDate = string.Empty;
        string mEndDate = string.Empty;
        string mDeptCd = string.Empty;
        #endregion

        #region //이벤트 전달
        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //기록지 관련 : 작성된 기록지 호출
        public delegate void ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode, string strFormCnt, string strInOutCls);
        public event ViewChart rViewChart;

        public delegate void ViewPanOCSFirstOpen(string strFirstOpen);
        public event ViewPanOCSFirstOpen rViewPanOCSFirstOpen;

        public void ClearForm()
        {
            //AcpEmr = null;
            pView = null;
            fView = null;
            mPTNO = "";

            ssViewEmrAcpCopy_Sheet1.RowCount = 0;
            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            ssViewEmrAcpForm_Sheet1.RowCount = 0;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            ssViewEmrAcpCopy_Sheet1.RowCount = 0;
            //treeView1.

            optEmrSearchGubun1.Checked = true;

        }

        /// <summary>
        /// 진료 미비찾는용도
        /// </summary>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        public void SetMibiCell(string strInDate, string strOutDate)
        {
            for (int i = 0; i < ssViewEmrAcpDept_Sheet1.RowCount; i++)
            {
                if (ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text.Trim().Replace("-", "") == strInDate &&
                   ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text.Trim().Replace("-", "") == strOutDate)
                {
                    ssViewEmrAcpDept.ShowRow(0, i, FarPoint.Win.Spread.VerticalPosition.Nearest);
                    ssViewEmrAcpDept_Sheet1.SetActiveCell(i, 0);
                    ssViewEmrAcpDeptCellDoubleClick(i, 0);
                    return;
                }
            }
        }


        /// <summary>
        /// 연속보기에서 수정/ 삭제 후 새로고침 용도.
        /// </summary>
        /// <param name="strInOutCls"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strFormName"></param>
        public void SetAutoRefresh(string strInOutCls, string strFrDate, string strDeptCode, string strFormName = "")
        {
            mstrFormName = strFormName;

            int RecordIndex = ssViewEmrAcpDeptChartList.ActiveSheet.ActiveRowIndex;
            for (int i = 0; i < ssViewEmrAcpDept_Sheet1.RowCount; i++)
            {
                if (ssViewEmrAcpDept_Sheet1.Cells[i, 0].Text.Trim().Equals(strInOutCls) &&
                    ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text.Trim().Replace("-", "").Equals(strFrDate) &&
                    ssViewEmrAcpDept_Sheet1.Cells[i, 3].Text.Trim().Equals(strDeptCode))
                {
                    ssViewEmrAcpDeptCellDoubleClick(i, 0);
                    break;
                }
            }

            ssViewEmrAcpDeptChartList.ActiveSheet.SetActiveCell(RecordIndex, 0);
            ssViewEmrAcpDeptChartList.ShowRow(0, RecordIndex, FarPoint.Win.Spread.VerticalPosition.Center);
        }

        /// <summary>
        /// 의료정보팀 사용
        /// </summary>
        /// <param name="strOutDate"></param>
        public void SetOutDateSearch(string strOutDate)
        {
            for (int i = 0; i < ssViewEmrAcpDept_Sheet1.RowCount; i++)
            {
                if (ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text.Trim().Equals(strOutDate))
                {
                    ssViewEmrAcpDeptCellDoubleClick(i, 0);
                    return;
                }
            }
        }

        /// <summary>
        /// ADMIN.ETC_JUPMST  <=> ADMIN.EMR_TREATT 매핑
        /// </summary>
        /// <param name="ROWID"></param>
        public void SetEkg(string ROWID)
        {
            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            #endregion

            #region 쿼리
            SQL = string.Empty;
            
            SQL += ComNum.VBLF + "WITH ORDER_DATA AS                                                ";
            SQL += ComNum.VBLF + "(                                                                 ";
            SQL += ComNum.VBLF + "SELECT T.TREATNO                                                   ";
            SQL += ComNum.VBLF + "	,	T.INDATE                                                    ";
            SQL += ComNum.VBLF + "	,	T.CLINCODE                                                  ";
            SQL += ComNum.VBLF + "	,	T.CLASS                                                     ";
            SQL += ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A                                    ";
            SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EMR_TREATT T                            ";
            SQL += ComNum.VBLF + "       ON T.PATID = A.PTNO                                        ";
            SQL += ComNum.VBLF + "      AND T.CLASS = A.GBIO                                        ";
            SQL += ComNum.VBLF + "      AND T.CLINCODE = A.DEPTCODE                                 ";
            SQL += ComNum.VBLF + "      AND T.INDATE <= TO_CHAR(A.BDATE, 'YYYYMMDD')                ";
            SQL += ComNum.VBLF + " WHERE A.ROWID = '" + ROWID + "'                                  ";
            SQL += ComNum.VBLF + ")                                                                 ";
            SQL += ComNum.VBLF + " SELECT   INDATE                                                  ";
            SQL += ComNum.VBLF + "      ,   CLINCODE                                                ";
            SQL += ComNum.VBLF + "      ,   CLASS                                                   ";
            SQL += ComNum.VBLF + "   FROM ORDER_DATA A                                              ";
            SQL += ComNum.VBLF + "  WHERE INDATE = (SELECT MAX(INDATE) FROM ORDER_DATA)             ";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr.NotEmpty())
            {
                clsDB.SaveSqlErrLog(SQL, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                SetAutoRefresh(dt.Rows[0]["CLASS"].ToString(),
                               dt.Rows[0]["INDATE"].ToString(),
                               dt.Rows[0]["CLINCODE"].ToString(),
                               dt.Rows[0]["CLASS"].ToString().Equals("O") ? "EKG" : "EKG 및 각종 검사결과지");
            }

            dt.Dispose();
        }


        public void GetJupHis(string pPTNO)
        {
            mPTNO = pPTNO;
            trvEmrView.Nodes.Clear();
            cboDept.SelectedIndex = cboDept.Items.Count > 0 ? 0 : -1;
            GetHisDept();
            ssViewEmrAcpForm_Sheet1.RowCount = 0;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;
            //GetHisChart();
        }

        #endregion


        public frmEmrBaseAcpList()
        {
            InitializeComponent();
        }

        public frmEmrBaseAcpList(string pPTNO)
        {
            InitializeComponent();
            mPTNO = pPTNO;
        }

        private void frmEmrBaseAcpList_Load(object sender, EventArgs e)
        {
            FormInit();
        }

        private void FormInit()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            dtpDateCopyE.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCurDate, "D"));
            dtpDateCopyS.Value = VB.DateAdd("m", -3, ComFunc.FormatStrToDate(strCurDate, "D"));

            dtpDateDeptS.Value = dtpDateCopyE.Value.AddDays(-30);
            dtpDateDeptE.Value = dtpDateCopyE.Value;

            chkGikan.Checked = clsType.User.BuseCode.Equals("055307");

            trvEmrView.ImageList = this.ImageList2;

            chkAllCopy.Visible = clsType.User.BuseCode.Equals("044201") || dtpDateCopyE.Value >= Convert.ToDateTime("2019-12-02");
            chkAllCopy.Checked = clsType.User.BuseCode.Equals("044201") || clsType.User.JobGroup.Equals("JOB002002") || clsType.User.JobGroup.Equals("JOB002003");

            mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            GbViewFMChart = clsEmrQueryOld.ViewFMChart(clsType.User.Sabun);

            //Tab 세팅
            //if(clsType.User.DrCode.Length > 0)
            //{
            //    tabRecord.Dock = DockStyle.Fill;
            //    tabRecord.TabPages[0].Controls.Add(ssViewEmrAcpDeptChartList);
            //    tabRecord.Visible = true;
            //    ssViewEmrAcpDeptChartList.Dock = DockStyle.Fill;

            //}
            //else
            //{
            //    ssViewEmrAcpDeptChartList.Dock = DockStyle.Fill;
            //}

            ssViewEmrAcpDeptChartList.Dock = DockStyle.Fill;


            //진료과 세팅
            cboDept.Items.Clear();
            cboDept.Items.Add("전   체" + VB.Space(50) + "0");

            SQL = ComNum.VBLF + "ORDER BY PRTGRD";
            dt = clsEmrQuery.GetMedDeptInfo(clsDB.DbCon, SQL);

            if (dt == null)
            {
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "진료과 정보 조회중 에러가 발생했습니다.");
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (mViewNpChart  == false && dt.Rows[i]["MEDDEPTCD"].ToString().Trim().Equals("NP") ||
                    GbViewFMChart == false && dt.Rows[i]["MEDDEPTCD"].ToString().Trim().Equals("FM"))
                {
                    continue;
                }

                cboDept.Items.Add(dt.Rows[i]["DEPTKORNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
            }
            dt.Dispose();

            cboDept.SelectedIndex = 0;

            panViewEmrAcpCopy.Dock = DockStyle.Fill;
            panViewEmrAcpCopy.Visible = false;

            panViewEmrAcpForm.Dock = DockStyle.Fill;
            panViewEmrAcpForm.Visible = false;

            panViewEmrAcpDept.Dock = DockStyle.Fill;
            panViewEmrAcpDept.Visible = true;
            panViewEmrAcpDept.BringToFront();

            ssViewEmrAcpCopy_Sheet1.RowCount = 0;
            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            ssViewEmrAcpForm_Sheet1.RowCount = 0;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            ssViewEmrAcpCopy_Sheet1.RowCount = 0;
            //treeView1.


            string basePath = @"C:\HealthSoft\EmrImageTmp\Update";
            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }

            basePath = @"C:\HealthSoft\EmrImageTmp\New";
            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }

            if (clsEmrPublic.gUserGrade.Equals("SIMSA"))
            {
                ssViewEmrAcpDeptChartList_Sheet1.Columns.Get(1).AllowAutoFilter = true;
                ssViewEmrAcpDeptChartList_Sheet1.AutoSortEnhancedContextMenu = true;
                ssViewEmrAcpDeptChartList_Sheet1.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;
            }
        }

        private void optEmrSearchGubun1_CheckedChanged(object sender, EventArgs e)
        {
            if (optEmrSearchGubun1.Checked == true)
            {
                panViewEmrAcpDept.Visible = true;
                panViewEmrAcpDept.BringToFront();
            }
        }

        private void optEmrSearchGubun2_CheckedChanged(object sender, EventArgs e)
        {
            if (optEmrSearchGubun2.Checked == true)
            {
                //의료정보팀일경우 차트복사 패널 보이게
                if (clsType.User.BuseCode.Equals("044201"))
                {
                    ssViewEmrAcpFormChartList_Sheet1.Columns[0].Visible = true;
                    panMCopy.Visible = true;
                }

                panViewEmrAcpForm.Visible = true;
                panViewEmrAcpForm.BringToFront();

                GetHisChart();
            }
            else
            {
                ssViewEmrAcpFormChartList_Sheet1.Columns[0].Visible = false;
            }
        }

        private void optEmrSearchGubun3_CheckedChanged(object sender, EventArgs e)
        {
            if (optEmrSearchGubun3.Checked == true)
            {
                panViewEmrAcpCopy.Visible = true;
                panViewEmrAcpCopy.BringToFront();

                GetHisSheet(""); 

                //ErBackColor(ssViewEmrAcpCopy);

                if (ssViewEmrAcpCopy_Sheet1.RowCount == 0 && trvEmrView.Nodes.Count == 0)
                {
                    GetDataCopy();
                }
            }
        }

        #region panViewEmrAcpDept

        private void btnSearchEmrDept_Click(object sender, EventArgs e)
        {
           GetHisDept();             
        }

        private void GetHisDept()
        {
            int i = 0;
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssViewEmrAcpDept_Sheet1.RowCount = 0;
            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            if (mPTNO.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);


            SQL.AppendLine("SELECT ");
            SQL.AppendLine("  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE,");
            SQL.AppendLine("  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME, XX.DRNAME, XX.GBSPC, XX.GBSTS,  ");
            SQL.AppendLine("  (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME ");
            SQL.AppendLine("  , CASE WHEN XX.JINDTL = '28' THEN '사본발급' END COPYSTR");

            SQL.AppendLine("  , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.OCS_MCCERTIFI_WONMU_REPRINT WHERE PANO = XX.PTNO AND BDATE = TO_DATE(XX.MEDFRDATE, 'YYYYMMDD') AND DEPTCODE = XX.MEDDEPTCD) THEN 1 END READ_DOCREPRINT");
            SQL.AppendLine("  , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.OPD_TELRESV WHERE PANO = XX.PTNO AND RDATE = TO_DATE(XX.MEDFRDATE, 'YYYYMMDD') AND DEPTCODE = XX.MEDDEPTCD AND GBCOPY = 'Y' ) THEN 1 END READ_CHARTCOPY");

            SQL.AppendLine("  , CASE WHEN XX.INOUTCLS = 'I' AND EXISTS ");
            SQL.AppendLine("  ( ");
            SQL.AppendLine("    SELECT 1                                                       	    ");
            SQL.AppendLine("    FROM ADMIN.IPD_NEW_MASTER                                     ");
            SQL.AppendLine("    WHERE PANO = XX.PTNO                                                ");
            SQL.AppendLine("      AND INDATE >= TO_DATE(XX.MEDFRDATE || ' 000000','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND INDATE <= TO_DATE(XX.MEDFRDATE || ' 235959','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND GBSTS NOT IN ('9')                                            ");
            SQL.AppendLine("      AND AMSET7 IN ('3','4','5')                                       ");
            SQL.AppendLine("   ) THEN '1' END ER_IPWON                                              ");

            SQL.AppendLine("FROM (");

            if (optEmrInOutDeptO.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                if (chkEmrSearchGubunA.Checked == true)
                {
                    SQL.AppendLine(" SELECT 'O' AS INOUTCLS, A.Pano AS PTNO,A.SName AS PTNAME, A.Sex, A.Age, ");
                    SQL.AppendLine("    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                    SQL.AppendLine("    NVL(TO_CHAR(A.BDATE,'YYYYMMDD'),TO_CHAR(A.ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDFRTIME,");
                    SQL.AppendLine("    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC, '0' GBSTS   ");
                    SQL.AppendLine("    , A.JINDTL");
                    SQL.AppendLine("FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B");
                    SQL.AppendLine("WHERE A.PANO = '" + mPTNO + "' ");
                    if (chkGikan.Checked == true)
                    {
                        SQL.AppendLine("     AND A.BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                        SQL.AppendLine("     AND A.BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    }
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL.AppendLine("    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                    }
                    else
                    {
                        SQL.AppendLine("    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")");
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL.AppendLine("    AND A.GBUSE = 'Y' ");
                    }
                    SQL.AppendLine("  AND A.DRCODE = B.DRCODE(+) ");
                }
                else
                {
                    //=================================================================
                    //2011-06-15 HD외래의 경우 한달 이내의 내역만 조회 요청(의뢰서)
                    //=================================================================
                    SQL.AppendLine(" SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ");
                    SQL.AppendLine("    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                    SQL.AppendLine("    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,");
                    SQL.AppendLine("    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ");
                    SQL.AppendLine("    , A.JINDTL");
                    SQL.AppendLine("FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ");
                    SQL.AppendLine("WHERE PANO = '" + mPTNO + "' ");
                    SQL.AppendLine("  AND DEPTCODE IN ('HD','RM') ");
                    SQL.AppendLine("  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL.AppendLine("    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                    }
                    else
                    {
                        SQL.AppendLine("    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")");
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL.AppendLine("    AND GBUSE = 'Y'");
                    }
                    SQL.AppendLine("     AND A.DRCODE = B.DRCODE(+) ");

                    SQL.AppendLine(" UNION ALL         ");
                    SQL.AppendLine(" SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ");
                    SQL.AppendLine("    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                    SQL.AppendLine("    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,");
                    SQL.AppendLine("    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ");
                    SQL.AppendLine("    , A.JINDTL");
                    SQL.AppendLine("FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ");
                    SQL.AppendLine("WHERE PANO = '" + mPTNO + "' ");
                    SQL.AppendLine("  AND DEPTCODE NOT IN ('HD','RM') ");
                    if (chkGikan.Checked == true)
                    {
                        SQL.AppendLine("     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                        SQL.AppendLine("     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    }
                    SQL.AppendLine("  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ");
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL.AppendLine("    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                    }
                    else
                    {
                        SQL.AppendLine("    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")");
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL.AppendLine("    AND GBUSE = 'Y' ");
                    }
                    SQL.AppendLine("  AND A.DRCODE =B.DRCODE(+)");
                }
            }

            if (optEmrInOutDeptA.Checked == true)
            {
                SQL.AppendLine("UNION ALL");
            }

            if (optEmrInOutDeptI.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                SQL.AppendLine(" SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age, ");
                SQL.AppendLine("    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                SQL.AppendLine("    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,");
                SQL.AppendLine("    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ");
                SQL.AppendLine("    , '' AS JINDTL");
                SQL.AppendLine("FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A , " + ComNum.DB_PMPA + "BAS_DOCTOR B ");
                SQL.AppendLine("WHERE A.PANO = '" + mPTNO + "' ");
                SQL.AppendLine("AND A.GBSTS <> '9'");
                SQL.AppendLine("    AND A.DRCODE = B.DRCODE(+) ");
                SQL.AppendLine("UNION ALL");
                if (clsEmrPublic.GstrView01 == "1")
                {
                    SQL.AppendLine("SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ");
                    SQL.AppendLine("    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ");
                    SQL.AppendLine("    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ");
                    SQL.AppendLine("    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME, '' AS DRNAME , '' AS GBSPC, '' AS GBSTS ");
                    SQL.AppendLine("    , '' AS JINDTL");
                    SQL.AppendLine("FROM ADMIN.EMRXMLMST A, ");
                    SQL.AppendLine("    " + ComNum.DB_PMPA + "IPD_NEW_MASTER B");//, " + ComNum.DB_PMPA + "BAS_DOCTOR C ");
                    SQL.AppendLine("WHERE A.PTNO =  '" + mPTNO + "' ");
                    SQL.AppendLine("AND A.INOUTCLS = 'I' ");
                    if (chkGikan.Checked == true)
                    {
                        SQL.AppendLine("     AND B.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD') ");
                        SQL.AppendLine("     AND B.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD') ");
                    }
                    SQL.AppendLine("AND B.GBSTS = '9' ");
                    SQL.AppendLine("AND A.PTNO = B.PANO ");
                    SQL.AppendLine("AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ");
                    SQL.AppendLine("AND A.MEDDEPTCD = B.DeptCode ");

                    SQL.AppendLine("UNION ALL ");
                    SQL.AppendLine("SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ");
                    SQL.AppendLine("    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ");
                    SQL.AppendLine("    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ");
                    SQL.AppendLine("    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME, '' AS DRNAME , '' AS GBSPC, '' AS GBSTS   ");
                    SQL.AppendLine("    , '' AS JINDTL");
                    SQL.AppendLine("FROM ADMIN.AEMRCHARTMST A, ");
                    SQL.AppendLine("    " + ComNum.DB_PMPA + "IPD_NEW_MASTER B");//, " + ComNum.DB_PMPA + "BAS_DOCTOR C ");
                    SQL.AppendLine("WHERE A.PTNO =  '" + mPTNO + "' ");
                    SQL.AppendLine("AND A.INOUTCLS = 'I' ");
                    if (chkGikan.Checked == true)
                    {
                        SQL.AppendLine("     AND B.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD') ");
                        SQL.AppendLine("     AND B.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD') ");
                    }
                    SQL.AppendLine("AND B.GBSTS = '9' ");
                    SQL.AppendLine("AND A.PTNO = B.PANO ");
                    SQL.AppendLine("AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ");
                    SQL.AppendLine("AND A.MEDDEPTCD = B.DeptCode ");

                }
                else if (clsEmrPublic.GstrView01 == "" || clsEmrPublic.GstrView01 == "0")
                {
                    SQL.AppendLine(" SELECT INOUTCLS, PTNO, PTNAME, SEX, AGE, MEDDEPTCD, MEDDRCD, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, DRNAME, GBSPC, GBSTS");
                    SQL.AppendLine("    , '' AS JINDTL");
                    SQL.AppendLine("  FROM (");
                    SQL.AppendLine("SELECT 'I' AS INOUTCLS, PANO AS PTNO, SName AS PTNAME, SEX, AGE,");
                    SQL.AppendLine("    DEPTCODE AS MEDDEPTCD, A.DRCODE AS MEDDRCD,");
                    SQL.AppendLine("    TO_CHAR(INDATE,'YYYYMMDD') AS MEDFRDATE, '1200' AS MEDFRTIME,");
                    SQL.AppendLine("    TO_CHAR(OUTDATE,'YYYYMMDD') AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ");
                    SQL.AppendLine("    From " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ");
                    SQL.AppendLine("WHERE PANO =  '" + mPTNO + "'");
                    SQL.AppendLine("AND GBSTS = '9'");
                    if (chkGikan.Checked == true)
                    {
                        SQL.AppendLine("     AND INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ");
                        SQL.AppendLine("     AND INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ");
                    }
                    SQL.AppendLine(" AND A.DRCODE = B.DRCODE(+) ");
                    SQL.AppendLine(" GROUP BY 'I', PANO, SNAME, SEX, AGE, DEPTCODE, A.DRCODE, TO_CHAR(INDATE,'YYYYMMDD'), '1200', TO_CHAR(OUTDATE,'YYYYMMDD'), DRNAME, GBSPC, GBSTS )");
                }
            }
            SQL.AppendLine("UNION ALL ");

            SQL.AppendLine(" SELECT A.CLASS AS INOUTCLS, A.PATID AS PTNO,  B.NAME AS PTNAME, B.Sex, 0 AS Age,  ");
            SQL.AppendLine("    A.CLINCODE AS MEDDEPTCD, C.DRCODE AS MEDDRCD, ");
            SQL.AppendLine("    A.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ");
            SQL.AppendLine("    A.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, C.DRNAME , '' GBSPC, '0' GBSTS   ");
            SQL.AppendLine("    , '' AS JINDTL");
            SQL.AppendLine("FROM ADMIN.EMR_TREATT A, ADMIN.EMR_PATIENTT B, " + ComNum.DB_MED + "OCS_DOCTOR C ");
            SQL.AppendLine("WHERE A.PATID = '" + mPTNO + "' ");
            SQL.AppendLine("  AND A.DOCCODE = C.DOCCODE(+) ");
            SQL.AppendLine("AND A.DELDATE IS NULL");
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL.AppendLine("     AND A.INDATE >= '" + dtpDateDeptS.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("     AND A.INDATE <= '" + dtpDateDeptE.Value.ToString("yyyyMMdd") + "' ");
            }
            SQL.AppendLine("AND A.PATID = B.PATID ");
            if (chkEmrSearchGubunA.Checked == false)
            {
                SQL.AppendLine("AND ((A.CLINCODE IN ('HD','RM') AND A.INDATE >= '" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "') OR (A.CLINCODE NOT IN ('HD','RM') AND A.INDATE >= '19000101'))");
            }
            if (optEmrInOutDeptO.Checked == true)
            {
                SQL.AppendLine("AND A.CLASS = 'O'");
            }
            else if (optEmrInOutDeptI.Checked == true)
            {
                SQL.AppendLine("AND A.CLASS = 'I'");
            }

            SQL.AppendLine("AND (A.CLASS, A.INDATE, A.CLINCODE) NOT IN ( ");
            SQL.AppendLine("            SELECT INOUTCLS, MEDFRDATE, MEDDEPTCD ");
            SQL.AppendLine("            FROM ");
            if (chkEmrSearchGubunA.Checked == true)
            {
                SQL.AppendLine("            (SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ");
                SQL.AppendLine("            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD");
                SQL.AppendLine("            FROM " + ComNum.DB_PMPA + "OPD_MASTER ");
                SQL.AppendLine("            WHERE PANO = '" + mPTNO + "' ");
                if (chkGikan.Checked == true)
                {
                    SQL.AppendLine("     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    SQL.AppendLine("     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                }

                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                else
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','8','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }

                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL.AppendLine("                AND GBUSE = 'Y' ");
                }
            }
            else
            {
                SQL.AppendLine("            (SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ");
                SQL.AppendLine("            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD");
                SQL.AppendLine("            FROM " + ComNum.DB_PMPA + "OPD_MASTER ");
                SQL.AppendLine("            WHERE PANO = '" + mPTNO + "' ");
                SQL.AppendLine("  AND DEPTCODE IN ('HD','RM') ");
                SQL.AppendLine("  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");

                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                else
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL.AppendLine("                AND GBUSE = 'Y' ");
                }
                SQL.AppendLine("        UNION ALL    ");
                SQL.AppendLine("            SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ");
                SQL.AppendLine("            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD");
                SQL.AppendLine("            FROM " + ComNum.DB_PMPA + "OPD_MASTER ");
                SQL.AppendLine("            WHERE PANO = '" + mPTNO + "' ");
                SQL.AppendLine("  AND DEPTCODE NOT IN ('HD','RM') ");
                SQL.AppendLine("  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ");
                if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
                {
                    SQL.AppendLine("     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                    SQL.AppendLine("     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ");
                }
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                else
                {
                    SQL.AppendLine("    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL.AppendLine("                AND GBUSE = 'Y' ");
                }
            }

            SQL.AppendLine("            UNION ALL ");
            SQL.AppendLine("             SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',A2.DeptCode) AS MEDDEPTCD ");
            SQL.AppendLine("            FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2  ");
            SQL.AppendLine("            WHERE A2.PANO = '" + mPTNO + "' ");
            SQL.AppendLine("            AND A2.GBSTS <> '9'");
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL.AppendLine("        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ");
                SQL.AppendLine("        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ");
            }
            SQL.AppendLine("           UNION ALL");
            SQL.AppendLine("            SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',B2.FRDEPT) AS MEDDEPTCD");
            SQL.AppendLine("              FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2,   " + ComNum.DB_PMPA + "IPD_TRANSFOR B2");
            SQL.AppendLine("              Where A2.PANO = B2.PANO ");
            SQL.AppendLine("                AND A2.PANO = '" + mPTNO + "' ");
            SQL.AppendLine("                AND A2.IPDNO = B2.IPDNO");
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL.AppendLine("        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ");
                SQL.AppendLine("        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ");
            }
            SQL.AppendLine("             AND A2.GBSTS <> '9')");
            SQL.AppendLine("    )  ");
            SQL.AppendLine(") XX");
            SQL.AppendLine("  WHERE XX.INOUTCLS IS NOT NULL");
            SQL.AppendLine("    ORDER BY XX.INOUTCLS ASC,  XX.MEDFRDATE DESC, XX.MEDDEPTCD, XX.GBSTS");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDEPTCODE = "";
            int FnCheck = 0;
            string FstrDateCheck = "";

            ssViewEmrAcpDept_Sheet1.RowCount = dt.Rows.Count;
            ssViewEmrAcpDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                strMedFrDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                strMedEndDate = "";
                if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "")
                {
                    strMedEndDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                }
                strMedDEPTCODE = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text = strMedFrDate;

                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "I")
                {
                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "9" && dt.Rows[i]["MEDENDTIME"].ToString().Trim() == "")
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = boldFont;
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "입원취소";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = RegularFont;
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = strMedEndDate;
                    }

                    if ((clsType.User.BuseCode.Equals("044201") || clsType.User.JobGroup.Equals("JOB002002") || clsType.User.JobGroup.Equals("JOB002003")) && dt.Rows[i]["ER_IPWON"].ToString().Equals("1"))
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(254, 224, 224);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (dt.Rows[i]["READ_DOCREPRINT"].ToString().Equals("1"))
                    //if (clsEmrQueryPohangS.READ_DOCREPRINT(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) == true)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = boldFont;
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "서류재발급";
                    }
                    else if (dt.Rows[i]["READ_CHARTCOPY"].ToString().Equals("1") || dt.Rows[i]["COPYSTR"].ToString().Trim().Equals("사본발급"))
                    //else if(clsEmrQueryPohangS.READ_CHARTCOPY(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) || dt.Rows[i]["COPYSTR"].ToString().Trim().Equals("사본발급"))
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = Color.FromArgb(102, 153, 51);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = boldFont;
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "사본발급";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = RegularFont;
                    }
                }
                ssViewEmrAcpDept_Sheet1.Cells[i, 3].Text = strMedDEPTCODE;

                if ((dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1107" || dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1125") && dt.Rows[i]["MEDDEPTCD"].ToString().Trim() == "MD")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 4].Text = "류마티스내과";
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                }

                ssViewEmrAcpDept_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "O")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 6].Text = "";
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 6].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                }
                ssViewEmrAcpDept_Sheet1.Cells[i, 7].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 9].Text = strMedEndDate;
                ssViewEmrAcpDept_Sheet1.Cells[i, 10].Text = strMedDEPTCODE;

                #region 21-06-30 의료정보팀 이동춘팀장님 해당 날짜에 스캔 기록이 많다고 음영 부탁하심.
                if (strMedFrDate.Equals("2004-12-31"))
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 0, i, 1].BackColor = Color.FromArgb(254, 224, 224);
                }
                #endregion

                //if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                //{
                //    //ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                //}
                //else if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                //{
                //    //if (clsVbfunc.READ_SPECIAL_SERVICE(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE, dt.Rows[i]["INOUTCLS"].ToString().Trim()) == true)
                //    //{
                //    //    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                //    //}
                //    //else
                //    //{
                //    //    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                //    //}
                //}
                //else
                //{
                //    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                //}

                if (clsEmrPublic.gUserGrade == "SIMSA")
                {
                    if (FstrDateCheck != VB.Left(strMedFrDate, 4))
                    {
                        FstrDateCheck = VB.Left(strMedFrDate, 4);
                        FnCheck = FnCheck + 1;
                    }
                    if (FnCheck % 2 == 0)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 1].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(232)))), ((int)(((byte)(170)))));
                    }
                }
            }
            dt.Dispose();
            dt = null;
            

            //ErBackColor(ssViewEmrAcpDept);

            Cursor.Current = Cursors.Default;


            if (clsType.User.Sabun == "46037")
            {
                //김병욱 의무원장 요청사항
                //환자 조회 시 자동으로 최상단 내역 더블클릭
                //우측 연속보기 시 전체 서식 보이기
                if (ssViewEmrAcpDept_Sheet1.Rows.Count > 0)
                {
                    ssViewEmrAcpDeptCellDoubleClick(0, 2);
                    if (rViewPanOCSFirstOpen != null)
                    {
                        rViewPanOCSFirstOpen("46037");
                    }
                    
                }
            }

        }

        private void ssViewEmrAcpDept_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpDept_Sheet1.RowCount == 0) return;

            //퇴원일자가 아닐경우만 정렬
            //19-08-02 추가
            if (e.ColumnHeader == true)
            {
                if (clsType.User.BuseCode.Equals("044201"))
                {
                    return;
                }
                if (e.Column != 2)
                {
                    clsSpread.gSpdSortRow(ssViewEmrAcpDept, e.Column);
                }
                return;
            }
        }

        private void ssViewEmrAcpDeptCellDoubleClick(int Row, int Column)
        {
            string strInOutCls = ssViewEmrAcpDept_Sheet1.Cells[Row, 0].Text.Trim();
            string strMedFrDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            string strMedDeptCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 3].Text.Trim();
            string strMedEndDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");
            string strMedFrDateType = string.Empty;

            if (string.IsNullOrWhiteSpace(strMedFrDate) == false)
            {
                strMedFrDateType = DateTime.ParseExact(strMedFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd");
            }

            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            else if (strMedEndDate == "서류재발급")
            {
                strMedEndDate = "";
                clsEmrQueryPohangS.READ_DOCREPRINTHIS(clsDB.DbCon, this, mPTNO, strMedFrDate, strMedDeptCd);
            }

            mInOutCls = strInOutCls;
            mFrDate = strMedFrDate;
            mEndDate = strMedEndDate;
            mDeptCd = strMedDeptCd;

            //string strMedFrTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 5].Text.Trim();
            //string strMedEndTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 6].Text.Trim();
            string strMedMedDrCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 7].Text.Trim();

            if (strInOutCls.Equals("O"))
            {
                if (strMedDeptCd.Equals("FM") && strMedMedDrCd.Equals("1404") == false)
                {
                    GbViewFMChart = true;
                }
                else if (strMedDeptCd.Equals("FM") && strMedMedDrCd.Equals("1404"))
                {
                    GbViewFMChart = clsEmrQueryOld.ViewFMChart(clsType.User.Sabun);
                }

                if (strMedDeptCd.Equals("NP") && mViewNpChart == false || strMedDeptCd.Equals("FM") && GbViewFMChart == false)
                {
                    ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;
                    ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                    return;
                }
            }

            #region 심사팀용 보험 표시
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            if (clsEmrPublic.gUserGrade.Equals("SIMSA"))
            {
                if (strInOutCls == "O")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO     = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE    = TO_DATE('" + strMedFrDateType + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strMedDeptCd + "' ";
                }
                else if (strInOutCls == "I")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO     = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE   >= TO_DATE('" + strMedFrDateType + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE   <= TO_DATE('" + strMedFrDateType + " 23:59','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strMedDeptCd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9')";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (ParentForm != null && ParentForm is frmEmrBaseChartView && ParentForm.ParentForm != null && ParentForm.ParentForm.Name.Equals("frmEmrViewer"))
                    {
                        Control lblName = ParentForm.ParentForm.Controls.Find("lblName", true)[0];
                        lblName.Text = dt.Rows[0]["SNAME"].ToString().Trim() + "(" + clsVbfunc.GetBiName(dt.Rows[0]["BI"].ToString().Trim()) + ")";
                    }
                }

                dt.Dispose();
            }

            #endregion


            if (strInOutCls == "I" && clsEmrQueryPohangS.READ_ER_IPWON(clsDB.DbCon, mPTNO, strMedFrDateType) == true)
            {
                GetHisForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd, "#");
                //if (clsType.User.DrCode.Length > 0 && clsEmrFunc.EMR_LIKERecord(clsDB.DbCon, clsType.User.Sabun))
                //{
                //    GetHisLikeForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd, "#");
                //}
            }
            else
            {
                GetHisForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd);
                //if (clsType.User.DrCode.Length > 0 && clsEmrFunc.EMR_LIKERecord(clsDB.DbCon, clsType.User.Sabun))
                //{
                //    GetHisLikeForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd);
                //}
            }

            if (ssViewEmrLikeList_Sheet1.RowCount > 0)
            {
                tabRecord.SelectedIndex = 1;
            }

            if (clsType.User.BuseCode.Equals("044201"))
            {
                if (ssViewEmrAcpDeptChartList_Sheet1.RowCount > 0 && ssViewEmrAcpDeptChartList_Sheet1.Cells[0, 1].Text.Trim().Equals("입퇴원 요약지") && ssViewEmrAcpDeptChartList_Sheet1.Cells[0, 1].ForeColor == Color.FromArgb(0, 0, 250))
                {
                    ssViewEmrAcpDeptChartList_Sheet1.ActiveRowIndex = -1;
                }
            }


            if (strInOutCls == "O")
            {
                if (ssViewEmrAcpDeptChartList_Sheet1.RowCount > 0)
                {
                    int intCol = -1;
                    int intRow = -1;

                    if (mstrFormName.Equals("EKG") || mstrFormName.Equals("EKG 및 각종 검사결과지"))
                    {
                        if (mstrFormName.Length > 0)
                        {
                            ssViewEmrAcpDeptChartList.Search(0, mstrFormName, true, true, true, true, 0, 0, ref intRow, ref intCol);
                            mstrFormName = string.Empty;
                        }
                    }
                    else
                    {
                        #region 19-07-27 호흡기전담 황보신영 간호사 요청으로 ER일경우 EM note 먼저 보이게 수정
                        ssViewEmrAcpDeptChartList.Search(0, "EM note", true, true, true, true, 0, 0, ref intRow, ref intCol);
                        #endregion
                    }

                    ssViewEmrAcpDeptChartListCellDoubleClick(strMedDeptCd == "ER" && intRow != -1 ? intRow : 0, 0, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd);
                }
                else
                {
                    rViewChart(null, null, "0", "0", "T", "", "1", strInOutCls);
                    return;
                }
            }
            else
            {
                if (ssViewEmrAcpDeptChartList_Sheet1.RowCount > 0)
                {
                    int intCol = -1;
                    int intRow = 0;
                    if (mstrFormName.Length > 0)
                    {
                        int RecordIndex = ssViewEmrAcpDeptChartList.ActiveSheet.ActiveRowIndex;
                        for (int i = 0; i < ssViewEmrAcpDept_Sheet1.RowCount; i++)
                        {
                            if (ssViewEmrAcpDept_Sheet1.Cells[i, 0].Text.Trim().Equals("I") &&
                                ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text.Trim().Equals(mstrFormName))
                            {
                                intRow = i;
                                intCol = 3;
                                break;
                            }
                        }

                        mstrFormName = string.Empty;
                    }
                    ssViewEmrAcpDeptChartListCellDoubleClick(intRow, intCol, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd);
                }
            }

        }

        private void GetHisForm(string strInOutCls, string strMedFrDate, string strMedEndDate, string strMedDeptCd, string strMedMedDrCd, string strREP = "")
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            bool ErCoordinator = clsEmrFunc.IsErCoordinator(clsDB.DbCon);
            //bool IsNurseNA = clsEmrFunc.IsNurseNA(clsDB.DbCon);

            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            SQL.AppendLine(" SELECT INOUTCLS, A.GRPFORMNO, FORMNO, FORMNAME, SCANYN, SUM(PCNT) AS PCNT, SUM(CNT) AS CNT, FORMCODE, TREATNO, RANKING, COLOR , MAX(EMRNO) AS EMRNO, UPDATENO, FORMBOLD, SUM(NOTCERT) NOTCERT");
            SQL.AppendLine(", CASE WHEN EXISTS (SELECT 1 FROM ADMIN.BAS_BCODE WHERE GUBUN = 'EMR_ER_대면기록지' AND CODE = A.FORMNO || '' AND DELDATE IS NULL) THEN 1 END READ_FORM_BOLD_RED ");

            SQL.AppendLine("FROM (");
            //=========================== 스캔내역 불러오기 ==================================
            SQL.AppendLine(" SELECT TO_NUMBER(T.TREATNO) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, 0 AS ACPNO,");
            SQL.AppendLine("        T.PATID AS PTNO, T.CLASS AS INOUTCLS, T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME,");
            SQL.AppendLine("        T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,");
            SQL.AppendLine("        NVL(F2.NAME, F.FORMNAME) AS FORMNAME,  F.GRPFORMNO, G.GRPFORMNAME AS GRPFORMNAME, C.NAME AS DEPTKORNAME,");
            SQL.AppendLine("        T.TREATNO, P.FORMCODE,  'S' AS SCANYN,");
            //SQL.AppendLine("        CASE WHEN G.GROUPPARENT > 0 THEN  (SELECT DISPSEQ FROM ADMIN.AEMRGRPFORM WHERE grpformno = G.GROUPPARENT ) * 10 + G.DISPSEQ ELSE G.DISPSEQ END DISPSEQ,  1 PCNT, 0 CNT, S.RANKING,");
            SQL.AppendLine("        1 PCNT, 0 CNT, S.RANKING,");
            SQL.AppendLine("        Case WHEN S.COLOR IS NULL THEN F.FORMCOLOR ELSE S.COLOR END COLOR, 1 AS UPDATENO, F.FORMBOLD");
            SQL.AppendLine("        , 0 AS NOTCERT");
            SQL.AppendLine("     FROM ADMIN.EMR_TREATT T,");
            SQL.AppendLine("          ADMIN.AEMRFORM F,");
            SQL.AppendLine("          ADMIN.EMRMAPPING M,");
            SQL.AppendLine("          ADMIN.AEMRGRPFORM G,");
            SQL.AppendLine("          ADMIN.EMR_CLINICT C,");
            SQL.AppendLine("          ADMIN.EMR_CHARTPAGET P,");
            SQL.AppendLine("          ADMIN.EMRFORM_SET S,");
            SQL.AppendLine("          ADMIN.EMR_FORMT F2");
            SQL.AppendLine("     WHERE T.PATID = '" + mPTNO + "' ");
       
            if (strREP == "#")
            {
                SQL.AppendLine("       AND T.INDATE = '" + strMedFrDate + "'");
                SQL.AppendLine("       AND T.CLASS IN ('O', 'I')");
            }
            else
            {
                SQL.AppendLine("       AND T.INDATE = '" + strMedFrDate + "'");
                SQL.AppendLine("       AND T.CLASS = '" + strInOutCls + "'");
            }



            if (clsEmrPublic.gUserGrade.Equals("SIMSA") && clsEmrPublic.gDateSET == true )
            {
                SQL.AppendLine("       AND T.INDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
            }

            SQL.AppendLine("       AND T.TREATNO = P.TREATNO");
            SQL.AppendLine("       AND P.FORMCODE = M.FORMCODE");
            SQL.AppendLine("       AND F2.FORMCODE = M.FORMCODE");
            SQL.AppendLine("       AND T.CLINCODE = C.CLINCODE");
            SQL.AppendLine("       AND F.GRPFORMNO = G.GRPFORMNO");
            SQL.AppendLine("       AND F.FORMNO = M.FORMNO");
            SQL.AppendLine("       AND C.CONSYN = 'Y'");

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125"))
                {
                    SQL.AppendLine("                AND T.CLINCODE = 'RA'");
                }
                else
                {
                    SQL.AppendLine("                AND T.CLINCODE = '" + strMedDeptCd + "'");
                }
            }
            else
            {

                if (strREP == "#")
                {
                    SQL.AppendLine("       AND CASE WHEN T.CLASS = 'O' AND T.CLINCODE = 'ER' THEN 1 ");
                    SQL.AppendLine("                WHEN T.CLASS = 'I' THEN 1                       ");
                    SQL.AppendLine("            END = 1                                             ");
                }
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine("        AND T.CLINCODE <> 'NP'");
            }

            if (GbViewFMChart == false)
            {
                SQL.AppendLine("        AND T.CLINCODE <> 'FM'");
            }

            SQL.AppendLine("           AND S.SABUN(+) = " + clsType.User.IdNumber);  //FstrPassId
            SQL.AppendLine("           AND F.FORMNO = S.FORMNO(+)");
            SQL.AppendLine("           AND F.UPDATENO = 1");
            SQL.AppendLine("    UNION ALL ");

            //=========================== TEXT내역 불러오기 ==================================
            SQL.AppendLine("SELECT  /*+index(A INDEX_EMRXMLMST7)*/ A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,");
            SQL.AppendLine("        0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,");
            SQL.AppendLine("        A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,");
            SQL.AppendLine("        A.MEDDRCD,  ");
            SQL.AppendLine("        CASE WHEN  A.FORMNO = 965 OR A.FORMNO = 2137 OR A.FORMNO = 2049 OR A.FORMNO = 963 THEN B.FORMNAME || '(신규)' ELSE B.FORMNAME END FORMNAME,");
            SQL.AppendLine("        B.GRPFORMNO, C.GRPFORMNAME,");
            SQL.AppendLine("        D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN,");
            //SQL.AppendLine("      CASE WHEN C.GROUPPARENT > 0 THEN  (SELECT DISPSEQ FROM ADMIN.AEMRGRPFORM WHERE grpformno = C.GROUPPARENT ) * 10 + C.DISPSEQ ELSE C.DISPSEQ END DISPSEQ,  0 PCNT, 1 CNT, S.RANKING,");
            SQL.AppendLine("        0 PCNT, 1 CNT, S.RANKING,");
            SQL.AppendLine("        CASE WHEN  S.COLOR IS NULL THEN B.FORMCOLOR ELSE S.COLOR END COLOR,");
            SQL.AppendLine("        CASE WHEN  A.FORMNO = 965 OR A.FORMNO = 2137 OR A.FORMNO = 2049 THEN 3");
            SQL.AppendLine("             WHEN  A.FORMNO = 963 THEN 2");
            SQL.AppendLine("             ELSE 1 ");
            SQL.AppendLine("        END UPDATENO,");
            SQL.AppendLine("        B.FORMBOLD");
            SQL.AppendLine("        , 0 AS NOTCERT");
            SQL.AppendLine("     FROM ADMIN.EMRXMLMST A, ");
            SQL.AppendLine("          ADMIN.AEMRFORM B, ");
            SQL.AppendLine("          ADMIN.AEMRGRPFORM C, ");
            SQL.AppendLine("          ADMIN.EMR_CLINICT D, ");
            SQL.AppendLine("          ADMIN.EMRFORM_SET S");
            SQL.AppendLine("     WHERE A.PTNO = '" + mPTNO + "'");
            SQL.AppendLine("       AND A.MEDFRDATE = '" + strMedFrDate + "'");
            SQL.AppendLine("       AND A.USEID IS NOT NULL");
            if (strMedDeptCd != "HD")
            {
                if (strREP == "#")
                {
                    SQL.AppendLine("       AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))");
                }
                else
                {
                    SQL.AppendLine("       AND A.INOUTCLS = '" + strInOutCls + "'");
                }
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                {
                    SQL.AppendLine("       AND A.MEDDEPTCD = 'MD'");
                    SQL.AppendLine("       AND A.MEDDRCD IN ('1107','1125')");
                }
                else
                {
                    SQL.AppendLine("       AND A.MEDDEPTCD = '" + strMedDeptCd + "'");
                    SQL.AppendLine("       AND A.MEDDRCD NOT IN ('1107','1125')");
                }
            }
            if (mViewNpChart == false)
            {
                SQL.AppendLine("       AND A.MEDDEPTCD <> 'NP'");
            }

            if (GbViewFMChart == false)
            {
                SQL.AppendLine("       AND A.MEDDEPTCD <> 'FM'");
            }

            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                SQL.AppendLine("       AND A.FORMNO NOT IN (1796)");
            }
            if (clsEmrPublic.gUserGrade.Equals("SIMSA") && clsEmrPublic.gDateSET == true)
            {
                SQL.AppendLine("       AND A.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
            }

            SQL.AppendLine("       AND A.FORMNO = B.FORMNO");
            SQL.AppendLine("       AND B.UPDATENO = 1");
            SQL.AppendLine("       AND B.GRPFORMNO = C.GRPFORMNO(+)");
            SQL.AppendLine("       AND A.MEDDEPTCD = D.CLINCODE");
            SQL.AppendLine("       AND S.SABUN(+) = " + clsType.User.IdNumber);
            SQL.AppendLine("       AND B.FORMNO = S.FORMNO(+)");

            #region 19-05-23 신규 EMR 추가 
            SQL.AppendLine("    UNION ALL ");
            SQL.AppendLine("SELECT  A2.EMRNO, A2.FORMNO, A2.CHARTDATE, A2.CHARTTIME,");
            SQL.AppendLine("        0 ACPNO, A2.PTNO, A2.INOUTCLS, A2.MEDFRDATE,");
            SQL.AppendLine("        A2.MEDFRTIME,  A2.MEDENDDATE, A2.MEDENDTIME, A2.MEDDEPTCD,");
            SQL.AppendLine("        A2.MEDDRCD,  B2.FORMNAME,  B2.GRPFORMNO, C2.GRPFORMNAME,");
            SQL.AppendLine("        D2.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, ");
            SQL.AppendLine("        0 PCNT, 1 CNT, S2.RANKING,");
            SQL.AppendLine("        CASE WHEN  S2.COLOR IS NULL THEN B2.FORMCOLOR ELSE S2.COLOR END COLOR, ");
            SQL.AppendLine("        B2.UPDATENO, B2.FORMBOLD");
            SQL.AppendLine("        , CASE WHEN A2.SAVECERT = '0'  AND EXISTS (SELECT 1 FROM ADMIN.HR_EMP_BASIS WHERE EMP_ID = A2.CHARTUSEID AND JOBKIND_CD = '41') THEN 1 ELSE 0 END NOTCERT");
            SQL.AppendLine("     FROM ADMIN.AEMRCHARTMST A2, ");
            SQL.AppendLine("          ADMIN.AEMRFORM B2, ");
            SQL.AppendLine("          ADMIN.AEMRGRPFORM C2, ");
            SQL.AppendLine("          ADMIN.EMR_CLINICT D2, ");
            SQL.AppendLine("          ADMIN.EMRFORM_SET S2");
            SQL.AppendLine("     WHERE A2.PTNO = '" + mPTNO + "'");
            SQL.AppendLine("       AND A2.MEDFRDATE = '" + strMedFrDate + "'");
            if (strMedDeptCd != "HD")
            {
                if (strREP == "#")
                {
                    SQL.AppendLine("       AND (A2.INOUTCLS = 'I' OR (A2.MEDDEPTCD = 'ER' AND A2.INOUTCLS = 'O'))");
                }
                else
                {
                    SQL.AppendLine("       AND A2.INOUTCLS = '" + strInOutCls + "'");
                }
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                {
                    SQL.AppendLine("       AND A2.MEDDEPTCD = 'MD'");
                    SQL.AppendLine("       AND A2.MEDDRCD IN ('1107','1125')");
                }
                else
                {
                    SQL.AppendLine("       AND A2.MEDDEPTCD = '" + strMedDeptCd + "'");
                    SQL.AppendLine("       AND A2.MEDDRCD NOT IN ('1107','1125')");
                }
            }
            if (mViewNpChart == false)
            {
                SQL.AppendLine("       AND A2.MEDDEPTCD <> 'NP'");
            }
            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                //SQL.AppendLine("       AND A2.FORMNO NOT IN (1568)");
            }
            if (clsEmrPublic.gUserGrade.Equals("SIMSA") && clsEmrPublic.gDateSET == true)
            {
                SQL.AppendLine("       AND A2.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
            }
            SQL.AppendLine("       AND A2.FORMNO = B2.FORMNO");
            SQL.AppendLine("       AND A2.UPDATENO = B2.UPDATENO");
            SQL.AppendLine("       AND B2.GRPFORMNO = C2.GRPFORMNO(+)");
            SQL.AppendLine("       AND A2.MEDDEPTCD = D2.CLINCODE");
            SQL.AppendLine("       AND S2.SABUN(+) = " + clsType.User.IdNumber);
            SQL.AppendLine("       AND B2.FORMNO = S2.FORMNO(+)");
            SQL.AppendLine("       AND A2.CHARTUSEID <> '합계'");

            SQL.AppendLine("    UNION ALL ");
            #endregion

            //===========================투약기록지 내역 불러오기 ==================================
            SQL.AppendLine(" SELECT  A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,");
            SQL.AppendLine("         0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,");
            SQL.AppendLine("         A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,");
            SQL.AppendLine("         A.MEDDRCD,  '투약기록지' FORMNAME , B.GRPFORMNO, '간호기록' GRPFORMNAME,");
            SQL.AppendLine("           D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN,");
            //SQL.AppendLine("           CASE WHEN C.GROUPPARENT > 0 THEN  (SELECT DISPSEQ FROM ADMIN.AEMRGRPFORM WHERE grpformno = C.GROUPPARENT ) * 10  + C.DISPSEQ ELSE C.DISPSEQ END DISPSEQ,  0 PCNT, 1 CNT, '' RANKING, B.FORMCOLOR COLOR, 1 UPDATENO, B.FORMBOLD");
            SQL.AppendLine("           0 PCNT, 1 CNT, '' RANKING, B.FORMCOLOR COLOR, 1 UPDATENO, B.FORMBOLD");
            SQL.AppendLine("         , 0 AS NOTCERT");
            SQL.AppendLine("      FROM ADMIN.EMRXML_TUYAK A,");
            SQL.AppendLine("           ADMIN.EMR_CLINICT d,");
            SQL.AppendLine("           ADMIN.AEMRFORM B,");
            SQL.AppendLine("           ADMIN.AEMRGRPFORM C");
            SQL.AppendLine("     WHERE A.PTNO = '" + mPTNO + "'");
            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
            }
            else
            {
                SQL.AppendLine("       AND 1 = 2");
            }
            SQL.AppendLine("       AND A.MEDFRDATE = '" + strMedFrDate + "'");
            if (strMedDeptCd != "HD")
            {
                if (strREP == "#")
                {
                    SQL.AppendLine("       AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))");
                }
                else
                {
                    SQL.AppendLine("       AND A.INOUTCLS = '" + strInOutCls + "'");
                }
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                {
                    SQL.AppendLine("       AND A.MEDDEPTCD = 'MD'");
                    SQL.AppendLine("       AND A.MEDDRCD IN ('1107','1125')");
                }
                else
                {
                    SQL.AppendLine("       AND A.MEDDEPTCD = '" + strMedDeptCd + "'");
                    SQL.AppendLine("       AND A.MEDDRCD NOT IN ('1107','1125')");
                }
            }

            if (mViewNpChart == true)
            {
                SQL.AppendLine("       AND A.MEDDEPTCD <> 'NP'");
            }

            if (GbViewFMChart == true)
            {
                SQL.AppendLine("       AND A.MEDDEPTCD <> 'FM'");
            }

            if (clsEmrPublic.gUserGrade.Equals("SIMSA") && clsEmrPublic.gDateSET == true)
            {
                SQL.AppendLine("          AND A.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
            }
            SQL.AppendLine("       AND A.MEDDEPTCD = D.CLINCODE(+)");
            SQL.AppendLine("       AND B.FORMNO  = 1796");
            SQL.AppendLine("       AND B.UPDATENO = 1");
            SQL.AppendLine("       AND B.GRPFORMNO = C.GRPFORMNO(+)");

            //전자동의서
            SQL.AppendLine(" UNION ALL ");
            SQL.AppendLine("   SELECT C.ID AS EMRNO, A.FORMNO,  TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME, ");
            SQL.AppendLine("     0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,      ");
            SQL.AppendLine("     A.FORMNAME, A.GRPFORMNO,       ");
            SQL.AppendLine("     D.GRPFORMNAME,      ");
            SQL.AppendLine("     (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME, ");
            SQL.AppendLine("     0 AS TREATNO, '000' AS FORMCODE,  'E' AS SCANYN, 0 PCNT, 1 CNT, S.RANKING, ");
            SQL.AppendLine("          Case WHEN S.COLOR IS NULL THEN A.FORMCOLOR ELSE S.COLOR END COLOR, A.UPDATENO, A.FORMBOLD");
            SQL.AppendLine("     , 0 AS NOTCERT");
            SQL.AppendLine("     FROM ADMIN.AEMRFORM A ");
            SQL.AppendLine("     INNER JOIN ADMIN.AEASFORMCONTENT B ");
            SQL.AppendLine("     ON A.FORMNO = B.FORMNO ");
            SQL.AppendLine("     AND A.UPDATENO = B.UPDATENO ");
            SQL.AppendLine("     INNER JOIN ADMIN.AEASFORMDATA C ");
            SQL.AppendLine("     ON B.ID = C.EASFORMCONTENT ");
            SQL.AppendLine("     INNER JOIN ADMIN.AEMRGRPFORM D ");
            SQL.AppendLine("     ON A.GRPFORMNO = D.GRPFORMNO ");
            SQL.AppendLine("     LEFT OUTER JOIN ADMIN.EMRFORM_SET S ");
            SQL.AppendLine("     ON S.SABUN = " + clsType.User.IdNumber);
            SQL.AppendLine("     AND A.FORMNO = S.FORMNO ");
            SQL.AppendLine("     WHERE C.PTNO = '" + mPTNO + "' ");
            SQL.AppendLine("     AND C.INOUTCLS = '" + strInOutCls + "' ");
            SQL.AppendLine("     AND C.MEDFRDATE = '" + strMedFrDate + "' ");
            SQL.AppendLine("     AND C.ISDELETED = 'N' ");
            SQL.AppendLine("     ) A");
            SQL.AppendLine("     INNER JOIN ADMIN.AEMRGRPFORM B");
            SQL.AppendLine("        ON B.GRPFORMNO  = (SELECT GROUPPARENT FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO  = A.GRPFORMNO)");
            SQL.AppendLine("     INNER JOIN ADMIN.AEMRGRPFORM B2");
            SQL.AppendLine("        ON B2.GRPFORMNO  = A.GRPFORMNO");
            if (clsType.User.AuAIMAGE.Equals("1")) //|| clsEmrPublic.gUserGrade.Equals("XRAY"))
            {
                SQL.AppendLine("     WHERE A.GRPFORMNO IN(1049, 1050, 1051, 1052, 1053, 1054, 1055, 1066, 1068, 1078, 1080)");
            }
            SQL.AppendLine("  GROUP BY INOUTCLS, A.GRPFORMNO, FORMNO, UPDATENO, FORMNAME, SCANYN, FORMCODE, TREATNO, B.DEPTH, B.DISPSEQ, B2.DISPSEQ, RANKING, COLOR, FORMBOLD");
            SQL.AppendLine("  ORDER BY RANKING ASC, INOUTCLS DESC, B.DEPTH, B.DISPSEQ, B2.DISPSEQ, FORMNO");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            int nRow = 0;

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    #region 사회사업실 인데 부서코드가 아닐경우 추가 안함.
                    if (dt.Rows[i]["GRPFORMNO"].ToString().Equals("1083") && (!clsType.User.BuseCode.Equals("033121") && !clsType.User.BuseCode.Equals("077900") && !clsType.User.BuseCode.Equals("077901")))
                    {
                        continue;
                    }
                    #endregion

                    nRow += 1;
                    ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;

                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Tag = strREP;
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();  //UPDATENO

                    if (clsEmrPublic.gUserGrade == "SIMSA")
                    {
                        switch (dt.Rows[i]["COLOR"].ToString().Trim())
                        {
                            case "1":
                                ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(240, 240, 250);
                                ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].Font = boldFont;
                                break;
                            default:
                                ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(255, 255, 255);
                                ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].Font = RegularFont;
                                break;
                        }
                    }
                    else
                    {
                        ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = ColorTranslator.FromHtml(dt.Rows[i]["COLOR"].ToString().Trim());

                        #region 간호사 미인증 차트 색상 변경
                        if (clsType.User.IsNurse.Equals("OK") && clsType.User.AuAWRITE.Equals("1") && dt.Rows[i]["NOTCERT"].To<int>() > 0)
                        {
                            ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.LightPink;
                        }
                        #endregion

                        //코드화 작업
                        if (dt.Rows[i]["FORMBOLD"].ToString().Trim().Equals("1"))
                        {
                            ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].Font = boldFont;
                        }
                        //if (clsEmrQueryPohangS.READ_FORM_BOLD(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim()) == true)
                        //{
                        //    ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].Font = boldFont;
                        //}

                        //코드화 작업
                        if (dt.Rows[i]["READ_FORM_BOLD_RED"].ToString().Trim().Equals("1"))
                            //if (clsEmrQueryPohangS.READ_FORM_BOLD_RED(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim()) == true)
                        {
                            ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].Font = boldFont;
                            ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].ForeColor = Color.Red;
                        }

                        //switch (dt.Rows[i]["GRPFORMNO"].ToString().Trim())
                        //{
                        //    case "11":
                        //    case "12":
                        //    case "13":
                        //    case "2":
                        //    case "27":
                        //        ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
                        //        break;
                        //    default:
                        //        ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                        //        break;
                        //}
                    }

                    if (clsEmrPublic.gUserGrade == "WRITE" || ErCoordinator == true)
                    {
                        if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1647")
                        {
                            if (clsEmrQueryPohangS.CHECK_COMPLETE(clsDB.DbCon, mPTNO, strMedFrDate, dt.Rows[i]["UPDATENO"].ToString().Trim()))
                            {
                                ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(250)))));
                            }
                        }
                    }

                    //if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1965" && clsEmrPublic.gUserGrade == "SIMSA")
                    //{
                    //    nRow += 1;
                    //    ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "수혈기록지2";
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1965";
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO
                    //    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
                    //}
                }
            }
            dt.Dispose();
            dt = null;

            #region 스캔/대리처방 권한자일경우 DR오더지도 안나오게
            if (clsType.User.AuAIMAGE.Equals("1"))// || clsEmrPublic.gUserGrade.Equals("XRAY"))
                return;
            #endregion

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            SQL.Clear();
            SQL.AppendLine("  SELECT COUNT(*) CNT");
            SQL.AppendLine("    FROM ADMIN.EMRXMLMST A");
            SQL.AppendLine("      WHERE A.PTNO = '" + mPTNO + "' ");
            SQL.AppendLine("        AND A.INOUTCLS = '" + strInOutCls + "'");
            SQL.AppendLine("        AND A.CHARTDATE >= '" + strMedFrDate + "' ");
            if (strMedEndDate == "")
            {
                SQL.AppendLine("     AND A.CHARTDATE <= '" + strCurDate + "' ");
            }
            else
            {
                SQL.AppendLine("     AND A.CHARTDATE <= '" + strMedEndDate + "' ");
            }
            SQL.AppendLine("          AND A.FORMNO IN (1790,1791,1795,1807)");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0 && dt.Rows[0]["CNT"].ToString().Trim().Equals("0") == false)
            {
                nRow = nRow + 1;
                ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = "I";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "ICU기록지";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1761";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "T";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
            }
            dt.Dispose();
            dt = null;


            //심사과는 입원환자의 경우 오더 내역체크 안하고 무조건 표시해줌!!!
            if (clsEmrPublic.gUserGrade == "SIMSA" && strInOutCls == "I")
            {
                nRow += 1;
                ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
            }
            else
            {
                //GbViewFMChart
                if (clsEmrQueryPohangS.IsDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
                {
                    nRow += 1;
                    ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
                }
            }

            if ((strREP == "#" || strMedDeptCd == "ER") && clsEmrQueryPohangS.IsERDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
            {
                nRow += 1;
                ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order(ER)";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "2090";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
            }

            ssViewEmrAcpDeptChartList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            Cursor.Current = Cursors.Default;
        }

        #region 사용안함 주석
        ///// <summary>
        ///// 즐겨보는 기록지
        ///// </summary>
        //void GetHisLikeForm(string strInOutCls, string strMedFrDate, string strMedEndDate, string strMedDeptCd, string strMedMedDrCd, string strREP = "")
        //{
        //    string SQL = "";
        //    string SqlErr = ""; //에러문 받는 변수
        //    int i = 0;
        //    DataTable dt = null;

        //    ssViewEmrLikeList_Sheet1.RowCount = 0;

        //    SQL = " SELECT INOUTCLS, GRPFORMNO, FORMNO, FORMNAME, SCANYN, SUM(PCNT) AS PCNT, SUM(CNT) AS CNT, FORMCODE, TREATNO, DISPSEQ, RANKING, COLOR , MAX(EMRNO) AS EMRNO, UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "FROM (";
        //    //=========================== 스캔내역 불러오기 ==================================
        //    SQL = SQL + ComNum.VBLF + " SELECT TO_NUMBER(T.TREATNO) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, 0 AS ACPNO,";
        //    SQL = SQL + ComNum.VBLF + "        T.PATID AS PTNO, T.CLASS AS INOUTCLS, T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME,";
        //    SQL = SQL + ComNum.VBLF + "        T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,";
        //    SQL = SQL + ComNum.VBLF + "        F.FORMNAME,  F.GRPFORMNO, G.GRPFORMNAME AS GRPFORMNAME, C.NAME AS DEPTKORNAME,";
        //    SQL = SQL + ComNum.VBLF + "        T.TREATNO, P.FORMCODE,  'S' AS SCANYN, G.DISPSEQ, 1 PCNT, 0 CNT, S.RANKING,";
        //    SQL = SQL + ComNum.VBLF + "        Case WHEN  S.COLOR IS NULL THEN F.FORMCOLOR ELSE S.COLOR END COLOR, 1 AS UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMR_TREATT T,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.AEMRFORM F,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMRMAPPING M,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMRGRPFORM G,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMR_CLINICT C,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMR_CHARTPAGET P,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMRFORM_SET S,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMR_LIKERECORD L";
        //    SQL = SQL + ComNum.VBLF + "     WHERE T.PATID = '" + mPTNO + "' ";
        //    SQL = SQL + ComNum.VBLF + "       AND T.INDATE = '" + strMedFrDate + "'";
        //    SQL = SQL + ComNum.VBLF + "       AND T.CLASS = '" + strInOutCls + "'";
        //    if (clsEmrPublic.gDateSET == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + ("       AND T.INDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND T.TREATNO = P.TREATNO";
        //    SQL = SQL + ComNum.VBLF + "       AND P.FORMCODE = M.FORMCODE";
        //    SQL = SQL + ComNum.VBLF + "       AND T.CLINCODE = C.CLINCODE";
        //    SQL = SQL + ComNum.VBLF + "       AND F.GRPFORMNO = G.GRPFORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND F.FORMNO = M.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND C.CONSYN = 'Y'";

        //    if (strInOutCls == "O")
        //    {
        //        if (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125"))
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND T.CLINCODE = 'RA'";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "        AND T.CLINCODE = '" + strMedDeptCd + "'";
        //        }
        //    }
        //    if (mViewNpChart == false)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND T.CLINCODE <> 'NP'";
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND F.FORMNO = S.FORMNO(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND F.UPDATENO = 1";
        //    SQL = SQL + ComNum.VBLF + "       AND S.SABUN(+) = " + clsType.User.IdNumber;  //FstrPassId
        //    SQL = SQL + ComNum.VBLF + "       AND L.FORMNO = F.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND L.UPDATENO = 1";

        //    SQL = SQL + ComNum.VBLF + "    UNION ALL ";

        //    //=========================== TEXT내역 불러오기 ==================================
        //    SQL = SQL + ComNum.VBLF + "SELECT  /*+index(A INDEX_EMRXMLMST7)*/ A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,";
        //    SQL = SQL + ComNum.VBLF + "        0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,";
        //    SQL = SQL + ComNum.VBLF + "        A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,";
        //    SQL = SQL + ComNum.VBLF + "        A.MEDDRCD,  B.FORMNAME,  B.GRPFORMNO, C.GRPFORMNAME,";
        //    SQL = SQL + ComNum.VBLF + "          D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, C.DISPSEQ, 0 PCNT, 1 CNT, S.RANKING,";
        //    SQL = SQL + ComNum.VBLF + "        Case WHEN  S.COLOR IS NULL THEN B.FORMCOLOR ELSE S.COLOR END COLOR, 1 AS UPDATENO, B.FORMBOLD";
        //    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXMLMST A, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.AEMRFORM B, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMRGRPFORM C, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMR_CLINICT D, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMRFORM_SET S,";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMR_LIKERECORD L";
        //    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + mPTNO + "'";
        //    SQL = SQL + ComNum.VBLF + "       AND A.MEDFRDATE = '" + strMedFrDate + "'";
        //    if (strMedDeptCd != "HD")
        //    {
        //        if (strREP == "#")
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.INOUTCLS = '" + strInOutCls + "'";
        //        }
        //    }

        //    if (strInOutCls == "O")
        //    {
        //        if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = 'MD'";
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDRCD IN ('1107','1125')";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDRCD NOT IN ('1107','1125')";
        //        }
        //    }
        //    if (mViewNpChart == false)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD <> 'NP'";
        //    }
        //    if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND A.FORMNO NOT IN (1796)";
        //    }
        //    if (clsEmrPublic.gDateSET == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + ("       AND A.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND A.FORMNO = B.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND B.UPDATENO = 1";
        //    SQL = SQL + ComNum.VBLF + "       AND B.GRPFORMNO = C.GRPFORMNO(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = D.CLINCODE";
        //    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = S.FORMNO(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND S.SABUN(+) = " + clsType.User.IdNumber;
        //    SQL = SQL + ComNum.VBLF + "       AND L.FORMNO = B.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND L.UPDATENO = 1";

        //    SQL = SQL + ComNum.VBLF + "       UNION ALL ";

        //    #region 19-05-23 신규 EMR 추가
        //    SQL = SQL + ComNum.VBLF + "SELECT  A2.EMRNO, A2.FORMNO, A2.CHARTDATE, A2.CHARTTIME,";
        //    SQL = SQL + ComNum.VBLF + "        0 ACPNO, A2.PTNO, A2.INOUTCLS, A2.MEDFRDATE,";
        //    SQL = SQL + ComNum.VBLF + "        A2.MEDFRTIME,  A2.MEDENDDATE, A2.MEDENDTIME, A2.MEDDEPTCD,";
        //    SQL = SQL + ComNum.VBLF + "        A2.MEDDRCD,  B2.FORMNAME,  B2.GRPFORMNO, C2.GRPFORMNAME,";
        //    SQL = SQL + ComNum.VBLF + "          D2.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, C2.DISPSEQ, 0 PCNT, 1 CNT, S2.RANKING,";
        //    SQL = SQL + ComNum.VBLF + "          Case WHEN  S2.COLOR IS NULL THEN B2.FORMCOLOR ELSE S2.COLOR END COLOR, B2.FORMBOLD";
        //    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.AEMRCHARTMST A2, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.AEMRFORM B2, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.AEMRGRPFORM C2, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMR_CLINICT D2, ";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMRFORM_SET S2,";
        //    SQL = SQL + ComNum.VBLF + " ADMIN.EMR_LIKERECORD L";
        //    SQL = SQL + ComNum.VBLF + "     WHERE A2.PTNO = '" + mPTNO + "'";
        //    SQL = SQL + ComNum.VBLF + "       AND A2.MEDFRDATE = '" + strMedFrDate + "'";
        //    if (strMedDeptCd != "HD")
        //    {
        //        if (strREP == "#")
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND (A2.INOUTCLS = 'I' OR (A2.MEDDEPTCD = 'ER' AND A2.INOUTCLS = 'O'))";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A2.INOUTCLS = '" + strInOutCls + "'";
        //        }
        //    }

        //    if (strInOutCls == "O")
        //    {
        //        if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A2.MEDDEPTCD = 'MD'";
        //            SQL = SQL + ComNum.VBLF + "       AND A2.MEDDRCD IN ('1107','1125')";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A2.MEDDEPTCD = '" + strMedDeptCd + "'";
        //            SQL = SQL + ComNum.VBLF + "       AND A2.MEDDRCD NOT IN ('1107','1125')";
        //        }
        //    }
        //    if (mViewNpChart == false)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND A2.MEDDEPTCD <> 'NP'";
        //    }
        //    if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND A2.FORMNO NOT IN (1796)";
        //    }
        //    if (clsEmrPublic.gDateSET == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + ("       AND A.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND A2.FORMNO = B2.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND A2.UPDATENO = B2.UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "       AND B2.GRPFORMNO = C2.GRPFORMNO(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND A2.MEDDEPTCD = D2.CLINCODE";
        //    SQL = SQL + ComNum.VBLF + "       AND B2.FORMNO = S2.FORMNO(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND S2.SABUN(+) = " + clsType.User.IdNumber;
        //    SQL = SQL + ComNum.VBLF + "       AND L.FORMNO = B2.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "       AND L.UPDATENO = B2.UPDATENO";

        //    SQL = SQL + ComNum.VBLF + "       UNION ALL ";
        //    #endregion

        //    //===========================투약기록지 내역 불러오기 ==================================
        //    SQL = SQL + ComNum.VBLF + " SELECT  A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,";
        //    SQL = SQL + ComNum.VBLF + "         0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,";
        //    SQL = SQL + ComNum.VBLF + "         A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,";
        //    SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,  '투약기록지' FORMNAME , 3 GRPFORMNO, '간호기록' GRPFORMNAME,";
        //    SQL = SQL + ComNum.VBLF + "           D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, 0 DISPSEQ, 0 PCNT, 1 CNT, '' RANKING, '' COLOR, 1 UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "      FROM ADMIN.EMRXML_TUYAK A,";
        //    SQL = SQL + ComNum.VBLF + "           ADMIN.EMR_CLINICT d,";
        //    SQL = SQL + ComNum.VBLF + "           ADMIN.EMR_LIKERECORD L";
        //    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + mPTNO + "'";
        //    if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
        //    {
        //    }
        //    else
        //    {
        //        SQL = SQL + ComNum.VBLF + "        AND 1 = 2";
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND A.MEDFRDATE = '" + strMedFrDate + "'";
        //    if (strMedDeptCd != "HD")
        //    {
        //        if (strREP == "#")
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.INOUTCLS = '" + strInOutCls + "'";
        //        }
        //    }

        //    if (strInOutCls == "O")
        //    {
        //        if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = 'MD'";
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDRCD IN ('1107','1125')";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
        //            SQL = SQL + ComNum.VBLF + "       AND A.MEDDRCD NOT IN ('1107','1125')";
        //        }
        //    }
        //    if (mViewNpChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD <> 'NP'";
        //    }

        //    if (clsEmrPublic.gDateSET == true)
        //    {
        //        SQL = SQL + ComNum.VBLF +  ("       AND A.CHARTDATE >= '" + clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, strInOutCls, strMedDeptCd, clsType.User.IdNumber, strMedFrDate).Replace("-", "") + "'");
        //    }
        //    SQL = SQL + ComNum.VBLF + "       AND A.MEDDEPTCD = D.CLINCODE(+)";
        //    SQL = SQL + ComNum.VBLF + "       AND L.FORMNO = 1796";
        //    SQL = SQL + ComNum.VBLF + "       AND L.UPDATENO = 1";

        //    //전자동의서
        //    SQL = SQL + ComNum.VBLF + " UNION ALL ";
        //    SQL = SQL + ComNum.VBLF + "   SELECT B.ID AS EMRNO, A.FORMNO,  TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME, ";
        //    SQL = SQL + ComNum.VBLF + "     0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,      ";
        //    SQL = SQL + ComNum.VBLF + "     A.FORMNAME, A.GRPFORMNO,       ";
        //    SQL = SQL + ComNum.VBLF + "     D.GRPFORMNAME,      ";
        //    SQL = SQL + ComNum.VBLF + "     (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME, ";
        //    SQL = SQL + ComNum.VBLF + "     0 AS TREATNO, '000' AS FORMCODE,  'E' AS SCANYN, D.DISPSEQ, 0 PCNT, 1 CNT, S.RANKING, ";
        //    SQL = SQL + ComNum.VBLF + "          Case WHEN S.COLOR IS NULL THEN A.FORMCOLOR ELSE S.COLOR END COLOR, A.UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.AEMRFORM A ";
        //    SQL = SQL + ComNum.VBLF + "     INNER JOIN ADMIN.AEASFORMCONTENT B ";
        //    SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = B.FORMNO ";
        //    SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO ";
        //    SQL = SQL + ComNum.VBLF + "     INNER JOIN ADMIN.AEASFORMDATA C ";
        //    SQL = SQL + ComNum.VBLF + "     ON B.ID = C.EASFORMCONTENT ";
        //    SQL = SQL + ComNum.VBLF + "     INNER JOIN ADMIN.AEMRGRPFORM D ";
        //    SQL = SQL + ComNum.VBLF + "     ON A.GRPFORMNO = D.GRPFORMNO ";
        //    SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.EMRFORM_SET S ";
        //    SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = S.FORMNO ";
        //    SQL = SQL + ComNum.VBLF + "     AND S.SABUN = " + clsType.User.IdNumber;
        //    SQL = SQL + ComNum.VBLF + "     WHERE C.PTNO = '" + mPTNO + "' ";
        //    SQL = SQL + ComNum.VBLF + "     AND C.INOUTCLS = '" + strInOutCls + "' ";
        //    SQL = SQL + ComNum.VBLF + "     AND C.MEDFRDATE = '" + strMedFrDate + "' ";

        //    SQL = SQL + ComNum.VBLF + "     ) ";
        //    SQL = SQL + ComNum.VBLF + "  GROUP BY INOUTCLS, GRPFORMNO, FORMNO, FORMNAME, SCANYN, FORMCODE, TREATNO, DISPSEQ, RANKING, COLOR, UPDATENO";
        //    SQL = SQL + ComNum.VBLF + "  ORDER BY RANKING ASC, INOUTCLS DESC, DISPSEQ, FORMNO";

        //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }

        //    int nRow = 0;

        //    if (dt.Rows.Count > 0)
        //    {
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            nRow += 1;
        //            ssViewEmrLikeList_Sheet1.RowCount = nRow;

        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO
        //            ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();  //UPDATENO

        //            if (clsEmrPublic.gUserGrade == "SIMSA")
        //            {
        //                switch (dt.Rows[i]["COLOR"].ToString().Trim())
        //                {
        //                    case "1":
        //                        ssViewEmrLikeList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(240, 240, 250);
        //                        break;
        //                    default:
        //                        ssViewEmrLikeList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(255, 255, 255);
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                ssViewEmrLikeList_Sheet1.Rows[nRow - 1].BackColor = ColorTranslator.FromHtml(dt.Rows[i]["COLOR"].ToString().Trim());

        //                //switch (dt.Rows[i]["GRPFORMNO"].ToString().Trim())
        //                //{
        //                //    case "11":
        //                //    case "12":
        //                //    case "13":
        //                //    case "2":
        //                //    case "27":
        //                //        ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
        //                //        break;
        //                //    default:
        //                //        ssViewEmrAcpDeptChartList_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        //                //        break;
        //                //}
        //            }

        //            if (clsEmrPublic.gUserGrade == "WRITE" || clsType.User.IdNumber == "24432")
        //            {
        //                if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1647")
        //                {
        //                    if (clsEmrQueryPohangS.CHECK_COMPLETE(clsDB.DbCon, mPTNO, strMedFrDate, dt.Rows[i]["UPDATENO"].ToString().Trim()))
        //                    {
        //                        ssViewEmrLikeList_Sheet1.Rows[nRow - 1].ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(250)))));
        //                    }
        //                }
        //            }

        //            //코드화 작업
        //            //if (clsEmrQueryPohangS.READ_FORM_BOLD(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim()) == true)
        //            //{
        //            //    ssViewEmrLikeList_Sheet1.Rows[nRow - 1].Font = boldFont;
        //            //}

        //            //코드화 작업
        //            if (clsEmrQueryPohangS.READ_FORM_BOLD_RED(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim()) == true)
        //            {
        //                ssViewEmrLikeList_Sheet1.Rows[nRow - 1].Font = boldFont;
        //                ssViewEmrLikeList_Sheet1.Rows[nRow - 1].ForeColor = Color.Red;
        //            }

        //            if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1965" && clsEmrPublic.gUserGrade == "SIMSA")
        //            {
        //                nRow += 1;
        //                ssViewEmrLikeList_Sheet1.RowCount = nRow;
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 1].Text = "수혈기록지2";
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 4].Text = "9999";
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO
        //                ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
        //            }
        //        }
        //    }
        //    dt.Dispose();
        //    dt = null;

        //    string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

        //    SQL = "  SELECT COUNT(*) CNT ";
        //    SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXMLMST A,";
        //    SQL = SQL + ComNum.VBLF + "         ADMIN.EMR_LIKERECORD L";

        //    SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
        //    SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = '" + strInOutCls + "'";
        //    SQL = SQL + ComNum.VBLF + "        AND A.CHARTDATE >= '" + strMedFrDate + "' ";
        //    if (strMedEndDate == "")
        //    {
        //        SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE <= '" + strCurDate + "' ";
        //    }
        //    else
        //    {
        //        SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE <= '" + strMedEndDate + "' ";
        //    }
        //    SQL = SQL + ComNum.VBLF + "        AND A.FORMNO IN (1790,1791,1795,1807)";
        //    SQL = SQL + ComNum.VBLF + "        AND L.FORMNO = A.FORMNO";
        //    SQL = SQL + ComNum.VBLF + "        AND L.UPDATENO = 1";

        //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }
        //    if (dt.Rows.Count > 0 && dt.Rows[0]["CNT"].ToString().Trim().Equals("0") == false)
        //    {
        //        nRow = nRow + 1;
        //        ssViewEmrLikeList_Sheet1.RowCount = nRow;
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 0].Text = "I";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 1].Text = "ICU기록지";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 2].Text = "1";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 3].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 4].Text = "1761";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 5].Text = "T";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 6].Text = "000";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 7].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
        //    }
        //    dt.Dispose();
        //    dt = null;


        //    if (clsEmrQueryPohangS.IsDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
        //    {
        //        nRow += 1;
        //        ssViewEmrLikeList_Sheet1.RowCount = nRow;
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 2].Text = "1";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 3].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 5].Text = "O";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 6].Text = "000";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 7].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
        //    }

        //    if ((strREP == "#" || strMedDeptCd == "ER") && clsEmrQueryPohangS.IsERDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
        //    {
        //        nRow += 1;
        //        ssViewEmrLikeList_Sheet1.RowCount = nRow;
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order(ER)";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 2].Text = "1";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 3].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 4].Text = "2090";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 5].Text = "O";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 6].Text = "000";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 7].Text = "0";
        //        ssViewEmrLikeList_Sheet1.Cells[nRow - 1, 8].Text = "1"; //UPDATENO
        //    }

        //    ssViewEmrAcpDeptChartList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        //    Cursor.Current = Cursors.Default;
        //}
        #endregion

        private void ssViewEmrAcpDeptChartList_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssViewEmrAcpDeptChartList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                if(clsType.User.BuseCode.Equals("044201"))
                {
                    return;
                }
                clsSpread.gSpdSortRow(ssViewEmrAcpDeptChartList, e.Column);
                return;
            }
        }

        private void ssViewEmrAcpDeptChartListCellDoubleClick(int Row, int Column, string strInOutCls = "", string strMedFrDate = "", string strMedEndDate = "", string strMedDeptCd = "")
        {
            string strPtno = mPTNO;

            if (strInOutCls == "")
            {
                strInOutCls = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 0].Text.Trim();
                ////strMedFrDate = ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 1].Text.Trim().Replace("-", "");
                ////strMedDeptCd = ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 3].Text.Trim();
                ////strMedEndDate = ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 2].Text.Trim().Replace("-", "");
                //if (ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 0].Text.Trim() == "I" &&
                //    strInOutCls == "O")
                //{
                //    strMedDeptCd = "";
                //}
                //else
                //{
                //    strInOutCls = ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 0].Text.Trim();
                //}

                strMedFrDate = mFrDate;
                strMedEndDate = mEndDate;
                strMedDeptCd = mDeptCd;

                if (mInOutCls.Equals("I") && strInOutCls == "O")
                {
                    strMedDeptCd = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 0].Tag != null && ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 0].Tag.ToString().Equals("#") ? "ER" :  "";
                }
                else
                {
                    strInOutCls = mInOutCls; //ssViewEmrAcpDept_Sheet1.Cells[ssViewEmrAcpDept_Sheet1.ActiveRowIndex, 0].Text.Trim();
                }
            }

            string strEmrCnt = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 2].Text.Trim();
            string strFormNo = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 4].Text.Trim();
            string strSCANYN = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 5].Text.Trim();
            string strFormCode = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 6].Text.Trim();
            string strEmrNo = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 7].Text.Trim();
            string strUpdateNo = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 8].Text.Trim();
            string strTreatNo = "0";

            #region (ER 경유시 기록지 처리)
            if ((strFormNo == "1796" || strFormNo == "2049" || strFormNo == "2605") && strInOutCls == "O")
            {
                strMedDeptCd = "ER";
            }
            #endregion

            pView = clsEmrChart.ClearPatient();
            if (strSCANYN == "E")
            {
                pView = clsEmrChart.SetEmrPatInfoEas(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = (long)VB.Val(strFormNo);
                pView.updateNo = (int)VB.Val(strUpdateNo);
            }
            else if (strSCANYN == "S")
            {
                pView = clsEmrChart.SetEmrPatInfoScan(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = 0;
                pView.updateNo = 1;
            }
            else
            {
                pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, strInOutCls, strMedFrDate, strMedDeptCd);
                if (pView == null)
                {
                    pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        return;
                    }
                }
                
                pView.formNo = (long)VB.Val(strFormNo);
                pView.updateNo = (int)VB.Val(strUpdateNo);
            }

            if (Column == -1)
            {
                rViewChart(pView, null, strEmrNo, strTreatNo, strSCANYN, strFormCode, strEmrCnt, strInOutCls);
                return;
            }

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (strSCANYN == "T" && VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            else
            {
                if (fView == null)
                {
                    return;
                }
            }
            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode, strEmrCnt, strInOutCls);
        }

        #endregion panViewEmrAcpDept

        #region panViewEmrAcpForm

        private void btnSearchEmrForm_Click(object sender, EventArgs e)
        {
            GetHisChart();
        }

        private void GetHisChart()
        {
            bool IsNurseNA = clsEmrFunc.IsNurseNA(clsDB.DbCon);
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            ssViewEmrAcpForm_Sheet1.RowCount = 0;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            if (mPTNO.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            SQL.AppendLine(" SELECT ET.GRPFORMNO, MAX(ET.GRPFORMNAME) AS GRPFORMNAME , ");
            SQL.AppendLine( "    ET.FORMNO,  MAX(ET.FORMNAME) AS FORMNAME, ET.FORMCODE, MAX(ET.SCANYN) AS SCANYN , COUNT(ET.EMRNO) AS CNT, ");
            SQL.AppendLine( "    MAX(ET.PCNT) AS PCNT, UPDATENO"); ;
            SQL.AppendLine( "FROM  "); 
            SQL.AppendLine( "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME,  ");
            SQL.AppendLine( "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME,  ");
            SQL.AppendLine( "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,   ");
            SQL.AppendLine("             NVL(F2.NAME, F.FORMNAME) AS FORMNAME,  F.GRPFORMNO,  ");
            SQL.AppendLine( "            (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME,  ");
            SQL.AppendLine( "            (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME,  ");
            SQL.AppendLine( "            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN, 1 AS UPDATENO  ");
            SQL.AppendLine( "    FROM ADMIN.EMR_TREATT T,  ");
            SQL.AppendLine( "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT,  ");
            SQL.AppendLine( "                TO_CHAR(P.TREATNO) AS EMRNO1  ");
            SQL.AppendLine( "            FROM ADMIN.EMR_CHARTPAGET P,  ");
            SQL.AppendLine( "                ADMIN.EMR_TREATT X  ");
            SQL.AppendLine( "            WHERE X.PATID = '" + mPTNO + "' ");
            SQL.AppendLine( "                AND P.TREATNO = X.TREATNO  ");
            SQL.AppendLine( "            GROUP BY P.TREATNO, P.FORMCODE) SS,  ");
            SQL.AppendLine( "        ADMIN.AEMRFORM F, ");
            SQL.AppendLine( "        ADMIN.EMRMAPPING M, ");
            SQL.AppendLine("         ADMIN.EMR_FORMT F2");


            SQL.AppendLine("    WHERE T.PATID = '" + mPTNO + "' ");
            if (optEmrInOutFormO.Checked == true)
            {
                SQL.AppendLine("        AND T.CLASS = 'O'         ");
            }
            else if (optEmrInOutFormI.Checked == true)
            {
                SQL.AppendLine("        AND T.CLASS = 'I'         ");
            }
            SQL.AppendLine( "        AND T.TREATNO = SS.TREATNO  ");
            SQL.AppendLine( "        AND M.FORMCODE = SS.FORMCODE ");
            SQL.AppendLine("         AND F2.FORMCODE = M.FORMCODE");
            SQL.AppendLine( "        AND F.FORMNO = M.FORMNO ");
            SQL.AppendLine( "        AND F.UPDATENO = 1");

            //if (IsNurseNA || clsEmrPublic.gUserGrade.Equals("XRAY"))
            if (IsNurseNA || clsType.User.AuAVIEW.Equals("0") && clsType.User.AuAIMAGE.Equals("0"))// || clsEmrPublic.gUserGrade.Equals("XRAY"))
            {
                    SQL.AppendLine(("        AND F.GRPFORMNO IN(1050, 1051, 1052, 1053, 1054, 1055, 1066, 1068, 1078, 1080)"));
            }

            // 간호조무사 아닐경우 그리고 XRAY 아닐경우 
            //if (IsNurseNA == false && clsEmrPublic.gUserGrade.Equals("XRAY") == false)
            if (clsType.User.AuAVIEW.Equals("1"))// && clsEmrPublic.gUserGrade.Equals("XRAY") == false)
            {
                SQL.AppendLine( "    UNION ALL  ");
                SQL.AppendLine( "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,   ");
                SQL.AppendLine( "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,   ");
                SQL.AppendLine( "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD,  ");
                SQL.AppendLine( "      B.FORMNAME,  B.GRPFORMNO,  ");
                SQL.AppendLine( "      (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = B.GRPFORMNO) AS GRPFORMNAME,  ");
                SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME,  ");
                SQL.AppendLine( "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN,  1 AS UPDATENO  ");
                SQL.AppendLine( "    FROM ADMIN.EMRXMLMST A");
                SQL.AppendLine( "      INNER JOIN ADMIN.AEMRFORM B  ");
                SQL.AppendLine( "          ON A.FORMNO = B.FORMNO  ");
                SQL.AppendLine( "          AND B.UPDATENO = 1");
                SQL.AppendLine("      WHERE A.PTNO = '" + mPTNO + "' ");
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL.AppendLine("        AND A.INOUTCLS = 'O'         ");
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL.AppendLine("        AND A.INOUTCLS = 'I'         ");
                }
                #region 19-05-23 신규 EMR
                SQL.AppendLine( "    UNION ALL  ");
                SQL.AppendLine( "    SELECT A2.EMRNO, A2.FORMNO, A2.CHARTDATE, A2.CHARTTIME,   ");
                SQL.AppendLine( "      0 ACPNO, A2.PTNO, A2.INOUTCLS, A2.MEDFRDATE, A2.MEDFRTIME,   ");
                SQL.AppendLine( "      A2.MEDENDDATE, A2.MEDENDTIME, A2.MEDDEPTCD, A2.MEDDRCD,  ");
                SQL.AppendLine( "      B2.FORMNAME,  B2.GRPFORMNO,  ");
                SQL.AppendLine( "      (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = B2.GRPFORMNO) AS GRPFORMNAME,  ");
                SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A2.MEDDEPTCD) AS DEPTKORNAME,  ");
                SQL.AppendLine( "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, B2.UPDATENO  ");
                SQL.AppendLine( "    FROM ADMIN.AEMRCHARTMST A2");
                SQL.AppendLine( "      INNER JOIN ADMIN.AEMRFORM B2  ");
                SQL.AppendLine( "         ON  A2.FORMNO = B2.FORMNO  ");
                SQL.AppendLine( "        AND A2.UPDATENO = B2.UPDATENO  ");
                SQL.AppendLine("      WHERE A2.PTNO = '" + mPTNO + "' ");
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL.AppendLine("        AND A2.INOUTCLS = 'O'         ");
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL.AppendLine("        AND A2.INOUTCLS = 'I'         ");
                }
                #endregion
                #region 20-04-01 전자동의서
                SQL.AppendLine("           UNION ALL                                                                                                                            ");
                SQL.AppendLine("           SELECT C.ID AS EMRNO, A.FORMNO,  TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME,             ");
                SQL.AppendLine("         0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,                  ");
                SQL.AppendLine("     A.FORMNAME, A.GRPFORMNO, D.GRPFORMNAME,                                                                                                    ");
                SQL.AppendLine("     (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME,                                            ");
                SQL.AppendLine("     0 AS TREATNO, '000' AS FORMCODE,  0 as PCNT, 'E' AS SCANYN, A.updateNo                                                                     ");
                SQL.AppendLine("     FROM ADMIN.AEMRFORM A                  ");
                SQL.AppendLine("     INNER JOIN ADMIN.AEASFORMCONTENT B     ");
                SQL.AppendLine("     ON A.FORMNO = B.FORMNO                      ");
                SQL.AppendLine("     AND A.UPDATENO = B.UPDATENO                 ");
                SQL.AppendLine("     INNER JOIN ADMIN.AEASFORMDATA C        ");
                SQL.AppendLine("     ON B.ID = C.EASFORMCONTENT                  ");
                SQL.AppendLine("     INNER JOIN ADMIN.AEMRGRPFORM D         ");
                SQL.AppendLine("     ON A.GRPFORMNO = D.GRPFORMNO                ");
                SQL.AppendLine("      WHERE  C.ISDELETED ='N' AND  C.PTNO = '" + mPTNO + "' ");
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL.AppendLine("        AND C.INOUTCLS = 'O'         ");
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL.AppendLine("         AND C.INOUTCLS = 'I'         ");
                }

                #endregion

                #region 19-08-16 투약기록지 추가
                if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
                {
                    SQL.AppendLine( "    UNION ALL");
                    SQL.AppendLine( "    SELECT A.EMRNO, A.FORMNO,  A.CHARTDATE, A.CHARTTIME,  ");
                    SQL.AppendLine( "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ");
                    SQL.AppendLine( "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ");
                    SQL.AppendLine( "      C.FORMNAME,  C.GRPFORMNO, ");
                    SQL.AppendLine( "      (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = C.GRPFORMNO) AS GRPFORMNAME, ");
                    SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ");
                    SQL.AppendLine( "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, 1 AS UPDATENO ");
                    SQL.AppendLine( "    FROM ADMIN.EMRXML_TUYAK A ");
                    SQL.AppendLine( "      INNER JOIN ADMIN.AEMRFORM C ");
                    SQL.AppendLine( "         ON A.FORMNO = C.FORMNO ");
                    SQL.AppendLine( "        AND C.UPDATENO = 1");
                    SQL.AppendLine("      WHERE A.PTNO = '" + mPTNO + "' ");

                    if (optEmrInOutFormO.Checked == true)
                    {
                        SQL.AppendLine("        AND A.INOUTCLS = 'O'         ");
                    }
                    else if (optEmrInOutFormI.Checked == true)
                    {
                        SQL.AppendLine("        AND A.INOUTCLS = 'I'         ");
                    }

                    if (mViewNpChart == false)
                    {
                        SQL.AppendLine("        AND A.MEDDEPTCD <> 'NP'");
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL.AppendLine("        AND A.MEDDEPTCD <> 'FM'");
                    }
                }
                #endregion

                #region 19-08-16 DR오더지 추가
                if (optEmrInOutFormO.Checked || optEmrInOutFormA.Checked)
                {
                    SQL.AppendLine( "    UNION ALL ");
                    SQL.AppendLine( "    SELECT ");
                    SQL.AppendLine( "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO,  TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                    SQL.AppendLine( "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                    SQL.AppendLine( "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                    SQL.AppendLine( "      O.DEPTCODE AS MEDDEPTCD, ");
                    SQL.AppendLine( "      max(O.DRCODE) AS MEDDRCD,  ");
                    SQL.AppendLine("      'Dr ORDER' AS FORMNAME ,  ");
                    //SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL.AppendLine( "      1012 AS GRPFORMNO,  ");
                    SQL.AppendLine( "      '의사지시기록' AS GRPFORMNAME,  ");
                    SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ");
                    SQL.AppendLine( "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ");
                    SQL.AppendLine( "    FROM " + ComNum.DB_MED + "OCS_OORDER O, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                    SQL.AppendLine( "          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                    SQL.AppendLine( "      WHERE O.PTNO = '" + mPTNO + "' ");
                    SQL.AppendLine( "        AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");

                    if (mViewNpChart == false)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'FM'");
                    }

                    SQL.AppendLine( "        AND    O.GBSUNAP ='1' AND O.Seqno    > '0'   AND O.NAL      > '0' ");
                    SQL.AppendLine( "        AND    O.SlipNo     =  C.SlipNo(+)       ");
                    SQL.AppendLine( "        AND    O.OrderCode  =  C.OrderCode(+)    ");
                    SQL.AppendLine( "        AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                    SQL.AppendLine( "        AND    O.DosCode    =  D.DosCode(+)      ");
                    SQL.AppendLine( "        AND    O.DRCODE      =  N.SABUN(+)       ");
                    SQL.AppendLine( "        AND    O.SUCODE = S.SUNEXT(+)  ");
                    SQL.AppendLine( "      GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                    SQL.AppendLine( "    UNION ALL ");
                    SQL.AppendLine( " SELECT ");
                    SQL.AppendLine( "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 2090 AS FORMNO,  TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                    SQL.AppendLine( "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                    SQL.AppendLine( "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                    SQL.AppendLine( "      O.DEPTCODE AS MEDDEPTCD, ");
                    SQL.AppendLine( "      max(O.DRCODE) AS MEDDRCD,  ");
                    SQL.AppendLine("      'Dr ORDER(ER)' AS FORMNAME ,  ");
                    //SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL.AppendLine( "      1012 AS GRPFORMNO,  ");
                    SQL.AppendLine( "      'ER 의사지시기록' AS GRPFORMNAME,  ");
                    SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = 'ER') AS DEPTKORNAME,  ");
                    SQL.AppendLine( "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ");
                    SQL.AppendLine( "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                    SQL.AppendLine( "          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                    SQL.AppendLine( "      WHERE O.PTNO = '" + mPTNO + "' ");
                    SQL.AppendLine( "        AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");
                    SQL.AppendLine( "        AND O.GBSTATUS NOT IN ('D-','D','D+' )   ");
                    SQL.AppendLine( "        AND    O.SlipNo     =  C.SlipNo(+)       ");
                    SQL.AppendLine( "        AND    O.OrderCode  =  C.OrderCode(+)    ");
                    SQL.AppendLine( "        AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                    SQL.AppendLine( "        AND    O.DosCode    =  D.DosCode(+)      ");
                    SQL.AppendLine( "        AND    O.DRCODE      =  N.SABUN(+)       ");
                    SQL.AppendLine( "        AND    O.SUCODE = S.SUNEXT(+)  ");
                    SQL.AppendLine( "        AND    O.GBIOE IN ('E','EI')");
                    SQL.AppendLine("       GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                }

                //if (optEmrInOutFormI.Checked || optEmrInOutFormA.Checked)
                //{
                //    SQL.AppendLine("    UNION ALL ");
                //    SQL.AppendLine(" SELECT ");
                //    SQL.AppendLine("     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO,  TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                //    SQL.AppendLine("      0 AS ACPNO, O.PTNO, 'I' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                //    SQL.AppendLine("      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                //    SQL.AppendLine("      O.DEPTCODE AS MEDDEPTCD, ");
                //    SQL.AppendLine("      max(O.DRCODE) AS MEDDRCD,  ");
                //    SQL.AppendLine("      'Dr ORDER' AS FORMNAME ,  ");
                //    SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                //    SQL.AppendLine("      1012 AS GRPFORMNO,  ");
                //    SQL.AppendLine("      '의사지시기록' AS GRPFORMNAME,  ");
                //    SQL.AppendLine("      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ");
                //    SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ");
                //    SQL.AppendLine("    FROM " + ComNum.DB_MED + "OCS_IORDER O, ");
                //    SQL.AppendLine("          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                //    SQL.AppendLine("          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                //    SQL.AppendLine("          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                //    SQL.AppendLine("          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                //    SQL.AppendLine("      WHERE O.PTNO = '" + mPTNO + "' ");
                //    SQL.AppendLine("          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");

                //    if (mViewNpChart == false)
                //    {
                //        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                //    }

                //    if (GbViewFMChart == false)
                //    {
                //        SQL.AppendLine("        AND O.DEPTCODE <> 'FM'");
                //    }

                //    SQL.AppendLine("          AND O.GBSTATUS NOT IN ('D-','D','D+' )   ");
                //    SQL.AppendLine("          AND    O.SlipNo     =  C.SlipNo(+)       ");
                //    SQL.AppendLine("          AND    O.OrderCode  =  C.OrderCode(+)    ");
                //    SQL.AppendLine("          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                //    SQL.AppendLine("          AND    O.DosCode    =  D.DosCode(+)      ");
                //    SQL.AppendLine("          AND    O.DRCODE      =  N.SABUN(+)       ");
                //    SQL.AppendLine("          AND    O.SUCODE = S.SUNEXT(+)  ");
                //    SQL.AppendLine("          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')");
                //    SQL.AppendLine("          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                //}
                #endregion
            }

            #region 기존 19-05-23
            //SQL = SQL + ComNum.VBLF + "    ) ET, ";
            //SQL = SQL + ComNum.VBLF + "   ADMIN.EMRGRPFORM GP";
            //SQL = SQL + ComNum.VBLF + "   WHERE ET.GRPFORMNO = GP.GRPFORMNO";
            //SQL = SQL + ComNum.VBLF + "GROUP BY GP.DISPSEQ, ET.GRPFORMNO,  ET.FORMNO, ET.FORMCODE";
            //SQL = SQL + ComNum.VBLF + "ORDER BY GP.DISPSEQ, ET.FORMNO";
            #endregion

            #region 19-05-23 신규
            SQL.AppendLine( "    ) ET ");
            SQL.AppendLine( "   INNER JOIN ADMIN.AEMRGRPFORM GR");
            SQL.AppendLine( "      ON GR.GRPFORMNO = (SELECT GROUPPARENT FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = ET.GRPFORMNO)");
            SQL.AppendLine( "   INNER JOIN ADMIN.AEMRGRPFORM GR2");
            SQL.AppendLine( "      ON GR2.GRPFORMNO = ET.GRPFORMNO");
            SQL.AppendLine( "GROUP BY GR.DEPTH, GR.DISPSEQ, GR2.DISPSEQ, ET.GRPFORMNO, ET.FORMNO, ET.FORMNAME, ET.FORMCODE, ET.UPDATENO");
            SQL.AppendLine("ORDER BY GR.DEPTH, GR.DISPSEQ, GR2.DISPSEQ, ET.FORMNAME");
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssViewEmrAcpForm_Sheet1.RowCount = dt.Rows.Count;
            ssViewEmrAcpForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssViewEmrAcpForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CNT"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 4].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            //ssViewEmrAcpForm_Sheet1.Cells[i, 0].Text = "Dr Order";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 1].Text = "1";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 2].Text = "0";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 3].Text = "1680";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 4].Text = "1";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 5].Text = "O";
            //ssViewEmrAcpForm_Sheet1.Cells[i, 6].Text = "000";

            Cursor.Current = Cursors.Default;
        }

        private void ssViewEmrAcpForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpForm_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpForm, e.Column);
                return;
            }

            ssViewEmrAcpFormCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpFormCellDoubleClick(int Row, int Col)
        {
            //TODO
            //Init_Modify

            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            string strFormNo = ssViewEmrAcpForm_Sheet1.Cells[Row, 3].Text.Trim();
            string strUpdateNo = ssViewEmrAcpForm_Sheet1.Cells[Row, 4].Text.Trim();
            string strFormCode = ssViewEmrAcpForm_Sheet1.Cells[Row, 6].Text.Trim();

            GetHisChartDept(strFormNo, strUpdateNo, strFormCode);
        }

        private void GetHisChartDept(string strFormNo, string strUpdateNo, string strFormCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.Default;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT ET.EMRNO, ET.FORMNO, ET.CHARTDATE, ET.CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "    ET.ACPNO, ET.PTNO, ET.INOUTCLS, ET.MEDFRDATE, ET.MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    CASE WHEN INOUTCLS = 'I' THEN  ";
                SQL = SQL + ComNum.VBLF + "        	(SELECT TO_CHAR(OUTDATE, 'YYYYMMDD')  ";
                SQL = SQL + ComNum.VBLF + "            FROM ADMIN.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + "           WHERE INDATE >= TO_DATE(ET.MEDFRDATE || '00:00', 'YYYYMMDDHH24:MI')";
                SQL = SQL + ComNum.VBLF + "             AND INDATE <= TO_DATE(ET.MEDFRDATE || '23:59', 'YYYYMMDDHH24:MI')";
                SQL = SQL + ComNum.VBLF + "             AND PANO  = ET.PTNO";
                SQL = SQL + ComNum.VBLF + "             AND GBSTS <> '9'";
                SQL = SQL + ComNum.VBLF + "          ) ";
                SQL = SQL + ComNum.VBLF + "    END MEDENDDATE  ";
                SQL = SQL + ComNum.VBLF + "    , ET.MEDENDTIME, ET.MEDDEPTCD, ET.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    ET.FORMNAME , ET.GRPFORMNO, ET.GRPFORMNAME, ET.DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    ET.TREATNO, ET.FORMCODE,  ET.PCNT, ET.SCANYN, ET.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,  ";
                SQL = SQL + ComNum.VBLF + "            F.FORMNAME,  F.GRPFORMNO, F.FORMNAME AS GRPFORMNAME,";
                SQL = SQL + ComNum.VBLF + "            (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN, 1 UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMR_TREATT T, ";
                SQL = SQL + ComNum.VBLF + "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT, ";
                SQL = SQL + ComNum.VBLF + "                TO_CHAR(P.TREATNO) AS EMRNO1 ";
                SQL = SQL + ComNum.VBLF + "            FROM ADMIN.EMR_CHARTPAGET P, ";
                SQL = SQL + ComNum.VBLF + "                ADMIN.EMR_TREATT X ";
                SQL = SQL + ComNum.VBLF + "            WHERE X.PATID = '" + mPTNO + "' ";
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND X.CLINCODE <> 'NP'";
                }
                if (GbViewFMChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND X.CLINCODE <> 'FM'";
                }

                SQL = SQL + ComNum.VBLF + "                AND P.TREATNO = X.TREATNO ";
                SQL = SQL + ComNum.VBLF + "            GROUP BY P.TREATNO, P.FORMCODE) SS, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.AEMRFORM F, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.EMRMAPPING M ";
                SQL = SQL + ComNum.VBLF + "    WHERE T.PATID = '" + mPTNO + "' ";
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND T.CLASS = 'O'         ";
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND T.CLASS = 'I'         ";
                }
                SQL = SQL + ComNum.VBLF + "        AND SS.FORMCODE = '" + strFormCode + "'        ";
                SQL = SQL + ComNum.VBLF + "        AND T.TREATNO = SS.TREATNO ";
                SQL = SQL + ComNum.VBLF + "        AND M.FORMCODE = SS.FORMCODE ";
                SQL = SQL + ComNum.VBLF + "        AND F.FORMNO = M.FORMNO ";
                SQL = SQL + ComNum.VBLF + "        AND F.UPDATENO = 1 ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "      B.FORMNAME,  B.GRPFORMNO, B.FORMNAME  AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, 1 UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXMLMST A ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.AEMRFORM B ";
                SQL = SQL + ComNum.VBLF + "         ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "         AND B.UPDATENO = 1";
                SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'O'         ";
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'I'         ";
                }
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
                }
                if (GbViewFMChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'FM'";
                }

                SQL = SQL + ComNum.VBLF + "          AND A.FORMNO = " + strFormNo;
                #region 신규  EMR 추가
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT A2.EMRNO, A2.FORMNO, A2.CHARTDATE, A2.CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + "      0 ACPNO, A2.PTNO, A2.INOUTCLS, A2.MEDFRDATE, A2.MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "      A2.MEDENDDATE, A2.MEDENDTIME, A2.MEDDEPTCD, A2.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "      B2.FORMNAME,  B2.GRPFORMNO,  B2.FORMNAME  AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A2.MEDDEPTCD) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, B2.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.AEMRCHARTMST A2 ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.AEMRFORM B2 ";
                SQL = SQL + ComNum.VBLF + "         ON A2.FORMNO = B2.FORMNO ";
                SQL = SQL + ComNum.VBLF + "        AND A2.UPDATENO = B2.UPDATENO";
                SQL = SQL + ComNum.VBLF + "      WHERE A2.PTNO = '" + mPTNO + "' ";
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A2.INOUTCLS = 'O'         ";
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A2.INOUTCLS = 'I'         ";
                }
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A2.MEDDEPTCD <> 'NP'";
                }
                if (GbViewFMChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A2.MEDDEPTCD <> 'FM'";
                }
                SQL = SQL + ComNum.VBLF + "          AND A2.FORMNO = " + strFormNo;
                #endregion

                #region 2020-04-01 전자동의서
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "       SELECT C.ID AS EMRNO, A.FORMNO,  TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + " 0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,        ";
                SQL = SQL + ComNum.VBLF + " A.FORMNAME, A.GRPFORMNO, D.GRPFORMNAME,        ";
                SQL = SQL + ComNum.VBLF + " (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME,  ";
                SQL = SQL + ComNum.VBLF + " 0 AS TREATNO, '000' AS FORMCODE,  0 as PCNT, 'E' AS SCANYN, A.updateNo  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.AEMRFORM A  ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.AEASFORMCONTENT B  ";
                SQL = SQL + ComNum.VBLF + " ON A.FORMNO = B.FORMNO  ";
                SQL = SQL + ComNum.VBLF + " AND A.UPDATENO = B.UPDATENO  ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.AEASFORMDATA C  ";
                SQL = SQL + ComNum.VBLF + " ON B.ID = C.EASFORMCONTENT  ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.AEMRGRPFORM D  ";
                SQL = SQL + ComNum.VBLF + " ON A.GRPFORMNO = D.GRPFORMNO  ";
      
                SQL = SQL + ComNum.VBLF + "      WHERE C.ISDELETED ='N' AND C.PTNO = '" + mPTNO + "' ";
                if (optEmrInOutFormO.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND C.INOUTCLS = 'O'         ";
                }
                else if (optEmrInOutFormI.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        AND C.INOUTCLS = 'I'         ";
                }
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND C.MEDDEPTCD <> 'NP'";
                }
                if (GbViewFMChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND C.MEDDEPTCD <> 'FM'";
                }
                SQL = SQL + ComNum.VBLF + "          AND A.FORMNO = " + strFormNo;

                #endregion

                #region 19-08-16 투약기록지 추가
                if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
                {
                    SQL = SQL + ComNum.VBLF + "    UNION ALL";
                    SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO,  A.CHARTDATE, A.CHARTTIME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ";
                    SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ";
                    SQL = SQL + ComNum.VBLF + "      B.FORMNAME,  B.GRPFORMNO, B.FORMNAME AS GRPFORMNAME, ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, 1 AS UPDATENO ";
                    SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML_TUYAK A INNER JOIN ADMIN.AEMRFORM B ";
                    SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = B.FORMNO ";
                    SQL = SQL + ComNum.VBLF + "          AND B.UPDATENO = 1";
                    SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";

                    if (optEmrInOutFormO.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'O'         ";
                    }
                    else if (optEmrInOutFormI.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'I'         ";
                    }

                    if (mViewNpChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'FM'";
                    }
                }
                #endregion

                #region 19-08-16 DR오더지 추가
                if (strFormCode.Equals("000") && (optEmrInOutFormO.Checked || optEmrInOutFormA.Checked))
                {
                    SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "    SELECT ";
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO,  TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                    SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER' AS FORMNAME ,  ";
                    //SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      1012 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      '의사지시기록' AS GRPFORMNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OORDER O, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_DOCTOR N, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_SUN     S ";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";

                    if (mViewNpChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'FM'";
                    }

                    SQL = SQL + ComNum.VBLF + "          AND    O.GBSUNAP ='1' AND O.Seqno    > '0'   AND O.NAL      > '0' ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+)  ";
                    SQL = SQL + ComNum.VBLF + "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT ";
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 2090 AS FORMNO,  TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                    SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER(ER)' AS FORMNAME ,  ";
                    //SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      1012 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      'ER 의사지시기록' AS GRPFORMNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = 'ER') AS DEPTKORNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_DOCTOR N, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_SUN     S ";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "          AND O.GBSTATUS NOT IN ('D-','D','D+' )   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+)  ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.GBIOE IN ('E','EI')";
                    SQL = SQL + ComNum.VBLF + "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ";
                }

                if (strFormCode.Equals("000") && (optEmrInOutFormI.Checked || optEmrInOutFormA.Checked))
                {
                    SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT ";
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO, TO_CHAR(O.BDATE, 'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'I' AS INOUTCLS, TO_CHAR(I.INDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(I.OUTDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                    SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER' AS FORMNAME ,  ";
                    //SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      1012 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      '의사지시기록' AS GRPFORMNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 1 AS UPDATENO  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_DOCTOR N, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_SUN   S, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "IPD_NEW_MASTER I ";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "        AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "        AND O.BDATE >= TRUNC(I.INDATE)";
                    SQL = SQL + ComNum.VBLF + "        AND O.BDATE <= I.OUTDATE";
                    SQL = SQL + ComNum.VBLF + "        AND O.PTNO  = I.PANO";

                    if (mViewNpChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'FM'";
                    }

                    SQL = SQL + ComNum.VBLF + "        AND O.GBSTATUS NOT IN ('D-','D','D+' )   ";
                    SQL = SQL + ComNum.VBLF + "        AND    O.SlipNo     =  C.SlipNo(+)       ";
                    SQL = SQL + ComNum.VBLF + "        AND    O.OrderCode  =  C.OrderCode(+)    ";
                    SQL = SQL + ComNum.VBLF + "        AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ";
                    SQL = SQL + ComNum.VBLF + "        AND    O.DosCode    =  D.DosCode(+)      ";
                    SQL = SQL + ComNum.VBLF + "        AND    O.DRCODE      =  N.SABUN(+)       ";
                    SQL = SQL + ComNum.VBLF + "        AND    O.SUCODE = S.SUNEXT(+)  ";
                    SQL = SQL + ComNum.VBLF + "        AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')";
                    SQL = SQL + ComNum.VBLF + "      GROUP BY O.BDATE, O.PTNO, TO_CHAR(I.INDATE,'YYYYMMDD'), TO_CHAR(I.OUTDATE,'YYYYMMDD'), O.DEPTCODE ";
                }
                #endregion

                SQL = SQL + ComNum.VBLF + "    ) ET ";
                SQL = SQL + ComNum.VBLF + "    WHERE ET.FORMNO   = '" + strFormNo + "'";
                SQL = SQL + ComNum.VBLF + "      AND ET.UPDATENO = " + strUpdateNo;
                SQL = SQL + ComNum.VBLF + "ORDER BY ET.INOUTCLS DESC, ET.CHARTDATE DESC, ET.MEDDEPTCD";

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
                    //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssViewEmrAcpFormChartList_Sheet1.RowCount = dt.Rows.Count;
                ssViewEmrAcpFormChartList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMNO"].ToString().Trim().Equals("1680") || dt.Rows[i]["FORMNO"].ToString().Trim().Equals("2090") ?
                        ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D") :
                        ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");

                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 2].Tag = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                    if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "" && dt.Rows[i]["INOUTCLS"].ToString().Trim().Equals("I"))
                    {
                        ssViewEmrAcpFormChartList_Sheet1.Cells[i, 3].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                    }
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssViewEmrAcpFormChartList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpFormChartList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpFormChartList, e.Column);
                return;
            }
        }

        private void ssViewEmrAcpFormChartListCellDoubleClick(int Row, int Col)
        {
            //TODO Init_Modify
            string strPtno = mPTNO;
            string strFormName = "";
            string strSCANYN = "N";
            string strFormNo = "0";
            string strEmrNo = "0";
            string strUpdateNo = "0";
            string strTreatNo = "0";
            string strFormCode = "";
            string strInOutCls = "";
            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDeptCd = "";
            //string strMedFrTime = "";
            //string strMedEndTime = "";
            //string strMedMedDrCd = "";

            strFormNo = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 6].Text.Trim();

            strInOutCls = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 1].Text.Trim();
            strMedFrDate = strFormNo.Equals("1680") || strFormNo.Equals("2090") ? ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 2].Tag.ToString() : ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");
            strMedEndDate = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 3].Text.Trim().Replace("-", "");
            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            strMedDeptCd = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 4].Text.Trim();

            strUpdateNo = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 7].Text.Trim();
            strFormName = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 8].Text.Trim();
            strSCANYN = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 9].Text.Trim();
            strEmrNo = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 10].Text.Trim();
            strFormCode = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 11].Text.Trim();

            //lblFORMNAME.Caption = strFormName
            //lblFORMNO.Caption = strFormNo
            //lblEMRNO.Caption = strEmrNo

            //With mEmrPt
            //    .PtPtNo = mPTNO
            //    .PtAcpNo = "0"
            //    .PtInOutCls = strInOutCls
            //    .PtMedFrDate = strMedFrDate
            //    .PtMedFrTime = strMedFrTime
            //    .PtMedEndDate = strMedEndDate
            //    .PtMedEndTime = strMedEndTime
            //    .PtMedDeptCd = strMedDeptCd
            //    .PtMedDrCd = strMedMedDrCd
            //End With

            //pView = clsEmrChart.ClearPatient();
            //if (strSCANYN == "E")
            //{
            //    pView = clsEmrChart.SetEmrPatInfoEas(clsDB.DbCon, strEmrNo);
            //    if (pView == null)
            //    {
            //        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
            //        return;
            //    }
            //    pView.formNo = (long)VB.Val(strFormNo);
            //    pView.updateNo = (int)VB.Val(strUpdateNo);
            //}
            //else
            //{
            //    if (strFormNo == "1796" || strFormNo == "1680" || strFormNo == "2090")
            //    {
            //        pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, strInOutCls, strMedFrDate, strMedDeptCd);
            //        if (pView == null)
            //        {
            //            ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
            //            return;
            //        }
            //        pView.formNo = (long)VB.Val(strFormNo);
            //        pView.updateNo = 1;
            //    }
            //    else if (strSCANYN == "S")
            //    {

            //    }
            //    else
            //    {
            //        pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
            //        if (pView == null)
            //        {
            //            ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
            //            return;
            //        }
            //        pView.formNo = (long)VB.Val(strFormNo);
            //        pView.updateNo = (int)VB.Val(strUpdateNo);
            //    }
            //}

            //fView = clsEmrChart.ClearEmrForm();
            //fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            //if (strSCANYN == "S")
            //{
            //    strTreatNo = strEmrNo;
            //    strEmrNo = "0";
            //}
            //else if (strFormNo == "1796" || strFormNo == "1680" || strFormNo == "2090")
            //{

            //}
            //else
            //{
            //    if (VB.Val(strEmrNo) == 0)
            //    {
            //        return;
            //    }
            //}

            //if (fView == null && strSCANYN != "S")
            //{
            //    return;
            //}

            //rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode, "", strInOutCls);


            #region 투약기록지처리 (ER)
            if ((strFormNo == "1796" || strFormNo == "2049") && strInOutCls == "O")
            {
                strMedDeptCd = "ER";
            }
            #endregion

            pView = clsEmrChart.ClearPatient();
            if (strSCANYN == "E")
            {
                pView = clsEmrChart.SetEmrPatInfoEas(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = (long)VB.Val(strFormNo);
                pView.updateNo = (int)VB.Val(strUpdateNo);
            }
            else if (strSCANYN == "S")
            {
                pView = clsEmrChart.SetEmrPatInfoScan(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = 0;
                pView.updateNo = 1;
            }
            else
            {
                if (strFormNo.Equals("1680") || strFormNo.Equals("2090"))
                {
                    pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strFormNo.Equals("2090") ? "ER" : "");
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        return;
                    }
                }
                else
                {
                    pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, strInOutCls, strMedFrDate, strMedDeptCd);
                    if (pView == null)
                    {
                        pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
                        if (pView == null)
                        {
                            ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                            return;
                        }
                    }
                }
             
                pView.formNo = (long)VB.Val(strFormNo);
                pView.updateNo = (int)VB.Val(strUpdateNo);
            }

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (strSCANYN == "T" && VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            else
            {
                if (fView == null)
                {
                    return;
                }
            }
            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode, "1", strInOutCls);
        }

        #endregion panViewEmrAcpForm


        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            trvEmrView.Nodes.Clear();

            if (chkDate.Checked == true)
            {
                dtpDateCopyS.Enabled = true;
                dtpDateCopyE.Enabled = true;
                optEmrInOutCopyO.Checked = true;
                chkAllCopy.Checked = false;
                //ssViewEmrAcpCopy.Visible = false;
            }
            else
            {
                if (clsType.User.BuseCode.Equals("044201") || clsType.User.JobGroup.Equals("JOB002002") || clsType.User.JobGroup.Equals("JOB002003"))
                {
                    chkAllCopy.Checked = true;
                }
            }
            //else
            //{
            //    ssViewEmrAcpCopy.Visible = true;
            //}
        }

        private void btnSearchEmrCopy_Click(object sender, EventArgs e)
        {
            GetDataCopy();
        }

        private void GetDataCopy()
        {
            bool IsNurseNA = clsEmrFunc.IsNurseNA(clsDB.DbCon);

            //if (IsNurseNA || clsEmrPublic.gUserGrade.Equals("XRAY"))
            if (IsNurseNA || clsType.User.AuAVIEW.Equals("0") && clsType.User.AuAIMAGE.Equals("0"))// || clsEmrPublic.gUserGrade.Equals("XRAY"))
                return;

            if (mPTNO.Trim() == "") return;
            pCopy = null;

            trvEmrView.Nodes.Clear();
            //trvEmrView.Nodes.Clear();
            //ssViewEmrAcpCopy_Sheet1.RowCount = 0;
            string strMedDeptCd = cboDept.SelectedIndex > 0 ? VB.Right(cboDept.Text.Trim(), 3).Trim() : string.Empty;

            if (chkDate.Checked == true)
            {
                if (optEmrInOutCopyO.Checked == false)
                {
                    ComFunc.MsgBoxEx(this, "기간별은 외래만 조회가 가능합니다.");
                }
                optEmrInOutCopyO.Checked = false;
                optEmrInOutCopyA.Checked = false;
                optEmrInOutCopyI.Checked = false;
                optEmrInOutCopyO.Checked = true;

                string strMedFrDate = dtpDateCopyS.Value.ToString("yyyyMMdd");
                string strMedMedDrCd = string.Empty;
                string strMedEndDate = dtpDateCopyE.Value.ToString("yyyyMMdd");

                GetHisSheetDsp("O", strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate, "1");
            }
            else
            {
                GetHisSheet(strMedDeptCd);
            }

            //ErBackColor(ssViewEmrAcpCopy);
        }

        /// <summary>
        /// ER경유 입원 색표시
        /// </summary>
        void ErBackColor(FarPoint.Win.Spread.FpSpread spd)
        {
            if (spd.ActiveSheet.RowCount == 0 || (clsType.User.BuseCode.Equals("044201") == false && clsType.User.JobGroup.Equals("JOB002002") == false))
                return;

            for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
            {
                string strInOutCls = spd.ActiveSheet.Cells[i, 0].Text.Trim();
                string strMedFrDateType = spd.ActiveSheet.Cells[i, 1].Text.Trim();
                if (strInOutCls.Equals("I") && clsEmrQueryPohangS.READ_ER_IPWON(clsDB.DbCon, mPTNO, strMedFrDateType) == true)
                {
                    spd.ActiveSheet.Cells[i, 0].BackColor = Color.FromArgb(254, 224, 224);
                    spd.ActiveSheet.Cells[i, 0].ForeColor = Color.Red;
                }
            }
        }

        private void GetHisSheetDsp(string strInOutCls, string strMedFrDate, string strMedDeptCd,
                                    string strMedMedDrCd, string strMedEndDate, string strOption)
        {
            int i = 0;
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string sKey1 = "";
            string sKey2 = "";
            string sKey3 = "";
            string sKey4 = "";
            string sKey5 = "";

            //string sKeyOld1 = "";
            //string sKeyOld2 = "";
            //string sKeyOld3 = "";
            //string sKeyOld4 = "";
            //string sKeyOld5 = "";
            string sKeyName1 = "";
            string sKeyName2 = "";
            string sKeyName3 = "";
            string sKeyName4 = "";
            //string strSCANCNT = "";
            string sChartDT = "";

            //TreeNode oNodex;

            trvEmrView.Nodes.Clear();

            strMedDeptCd = strMedDeptCd.IndexOf("'") == -1 && strMedDeptCd.Length > 0 ? "'" + strMedDeptCd + "'" : strMedDeptCd;

            #region sql
            SQL.AppendLine( " SELECT ET.EMRNO, ET.FORMNO, ET.UPDATENO, ET.CHARTDATE, ET.CHARTTIME, ");
            SQL.AppendLine( "     ET.ACPNO, ET.PTNO, ET.INOUTCLS, ET.MEDFRDATE, ET.MEDFRTIME, ");
            SQL.AppendLine( "     ET.MEDENDDATE, ET.MEDENDTIME, ET.MEDDEPTCD, ET.MEDDRCD, ");
            SQL.AppendLine( "     ET.FORMNAME , ET.GRPFORMNO, ET.GRPFORMNAME, ET.DEPTKORNAME, ");
            SQL.AppendLine( "     ET.TREATNO, ET.FORMCODE,  ET.PCNT, ET.SCANYN, ET.EMRNOHIS ");

            SQL.AppendLine("  , CASE WHEN ET.INOUTCLS = 'I' AND EXISTS ");
            SQL.AppendLine("  ( ");
            SQL.AppendLine("    SELECT 1                                                       	    ");
            SQL.AppendLine("    FROM ADMIN.IPD_NEW_MASTER                                     ");
            SQL.AppendLine("    WHERE PANO = ET.PTNO                                                ");
            SQL.AppendLine("      AND INDATE >= TO_DATE(ET.MEDFRDATE || ' 000000','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND INDATE <= TO_DATE(ET.MEDFRDATE || ' 235959','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND GBSTS NOT IN ('9')                                            ");
            SQL.AppendLine("      AND AMSET7 IN ('3','4','5')                                       ");
            SQL.AppendLine("   ) THEN '1' END ER_IPWON                                              ");

            SQL.AppendLine( "FROM ");
            SQL.AppendLine( "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO,  1 AS UPDATENO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, ");
            SQL.AppendLine( "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ");
            SQL.AppendLine( "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,  ");
            SQL.AppendLine("             NVL(F2.NAME, F.FORMNAME) AS FORMNAME,  F.GRPFORMNO, ");
            SQL.AppendLine( "            (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME, ");
            SQL.AppendLine( "            (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME, ");
            SQL.AppendLine("            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN, 0 AS EMRNOHIS ");
            SQL.AppendLine( "    FROM ADMIN.EMR_TREATT T, ");
            SQL.AppendLine( "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT, ");
            SQL.AppendLine( "                TO_CHAR(P.TREATNO) AS EMRNO1 ");
            SQL.AppendLine( "            FROM ADMIN.EMR_CHARTPAGET P, ");
            SQL.AppendLine( "                ADMIN.EMR_TREATT X ");
            SQL.AppendLine( "            WHERE X.PATID = '" + mPTNO + "' ");
            SQL.AppendLine( "                AND P.TREATNO = X.TREATNO ");
            SQL.AppendLine( "            GROUP BY P.TREATNO, P.FORMCODE) SS, ");
            SQL.AppendLine( "        ADMIN.AEMRFORM F, ");
            SQL.AppendLine( "        ADMIN.EMRMAPPING M, ");
            SQL.AppendLine("         ADMIN.EMR_FORMT F2");

            SQL.AppendLine( "    WHERE T.PATID = '" + mPTNO + "' ");
            SQL.AppendLine( "        AND T.TREATNO = SS.TREATNO  ");
            SQL.AppendLine( "        AND M.FORMCODE = SS.FORMCODE ");
            SQL.AppendLine( "        AND F.FORMNO = M.FORMNO ");
            SQL.AppendLine( "        AND F2.FORMCODE = M.FORMCODE");

            SQL.AppendLine( "        AND F.UPDATENO = 1");
            if (strOption == "")
            {
                SQL.AppendLine( "        AND T.CLASS = '" + strInOutCls + "'");
                SQL.AppendLine( "        AND T.INDATE = '" + strMedFrDate + "'");
                if (strInOutCls == "O")
                {
                    if (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125"))
                    {
                        SQL.AppendLine( "                AND T.CLINCODE = 'RA'");
                    }
                    else
                    {
                        SQL.AppendLine( "                AND T.CLINCODE IN(" + strMedDeptCd + ")");
                    }
                }
            }
            else
            {
                SQL.AppendLine( "        AND T.INDATE >= '" + strMedFrDate + "'");
                SQL.AppendLine( "        AND T.INDATE <= '" + strMedEndDate + "'");
            }
            //2012-11-23 NP챠트 조회 조건 간단하게 변경
            if (mViewNpChart == false)
            {
                SQL.AppendLine( "        AND T.CLINCODE <> 'NP'");
            }

            if (GbViewFMChart == false)
            {
                SQL.AppendLine("        AND T.CLINCODE <> 'FM'");
            }

            #region //Text EMR
            SQL.AppendLine( "    UNION ALL ");
            SQL.AppendLine( "    SELECT A.EMRNO, A.FORMNO,  1 AS UPDATENO, A.CHARTDATE, A.CHARTTIME,  ");
            SQL.AppendLine( "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ");
            SQL.AppendLine( "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ");
            SQL.AppendLine( "      B.FORMNAME,  B.GRPFORMNO, ");
            SQL.AppendLine( "      (SELECT GRPFORMNAME FROM ADMIN.EMRGRPFORM WHERE GRPFORMNO = B.GRPFORMNO) AS GRPFORMNAME, ");
            SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ");
            SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, 0 AS EMRNOHIS ");
            SQL.AppendLine("     FROM ADMIN.EMRXMLMST A INNER JOIN ADMIN.AEMRFORM B ");
            SQL.AppendLine( "          ON A.FORMNO = B.FORMNO ");
            SQL.AppendLine( "          AND B.UPDATENO = 1");
            SQL.AppendLine("      WHERE A.PTNO = '" + mPTNO + "' ");
            SQL.AppendLine( "        AND A.INOUTCLS = '" + strInOutCls + "'");
            if (strOption == "")
            {
                SQL.AppendLine( "        AND A.MEDFRDATE = '" + strMedFrDate + "'");
                if (strInOutCls == "O")
                {
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                    {
                        SQL.AppendLine( "                AND A.MEDDEPTCD = 'MD'");
                        SQL.AppendLine( "                AND A.MEDDRCD IN ('1107','1125') ");
                    }
                    else
                    {
                        SQL.AppendLine( "                AND A.MEDDEPTCD IN(" + strMedDeptCd + ")");
                        SQL.AppendLine( "                AND A.MEDDRCD NOT IN ('1107','1125')");
                    }
                }
            }
            else
            {
                SQL.AppendLine( "        AND A.MEDFRDATE >= '" + strMedFrDate + "'");
                SQL.AppendLine( "        AND A.MEDFRDATE <= '" + strMedEndDate + "'");
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine("        AND A.MEDDEPTCD <> 'NP'");
            }


            if (GbViewFMChart == false)
            {
                SQL.AppendLine("        AND A.MEDDEPTCD <> 'FM'");
            }

            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                SQL.AppendLine( "    AND A.FORMNO NOT IN (1796)");
            }

            SQL.AppendLine( "    UNION ALL ");
            SQL.AppendLine( "    	SELECT C.EMRNO, C.FORMNO, C.UPDATENO, C.CHARTDATE, C.CHARTTIME,   ");
            SQL.AppendLine( "          C.ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME,  ");
            SQL.AppendLine( "          C.MEDENDDATE, C.MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,  ");
            SQL.AppendLine( "          F.FORMNAME AS FORMNAME,  F.GRPFORMNO,  ");
            SQL.AppendLine( "          (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME,  ");
            SQL.AppendLine( "          (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME,  ");
            SQL.AppendLine("          0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, C.EMRNOHIS  ");
            SQL.AppendLine( "        FROM ADMIN.AEMRCHARTMST C  ");
            SQL.AppendLine( "          INNER JOIN ADMIN.AEMRFORM F  ");
            SQL.AppendLine( "             ON C.FORMNO = F.FORMNO ");
            SQL.AppendLine( "             AND C.UPDATENO = F.UPDATENO ");
            SQL.AppendLine( "      WHERE C.PTNO = '" + mPTNO + "' ");
            SQL.AppendLine( "        AND C.INOUTCLS = '" + strInOutCls + "'");
            if (strOption == "")
            {
                SQL.AppendLine( "        AND C.MEDFRDATE = '" + strMedFrDate + "'");
                if (strInOutCls == "O")
                {
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                    {
                        SQL.AppendLine( "                AND C.MEDDEPTCD = 'MD'");
                        SQL.AppendLine( "                AND C.MEDDRCD IN ('1107','1125') ");
                    }
                    else
                    {
                        SQL.AppendLine( "                AND C.MEDDEPTCD IN(" + strMedDeptCd + ")");
                        SQL.AppendLine( "                AND C.MEDDRCD NOT IN ('1107','1125')");
                    }
                }
            }
            else
            {
                SQL.AppendLine( "        AND C.MEDFRDATE >= '" + strMedFrDate + "'");
                SQL.AppendLine( "        AND C.MEDFRDATE <= '" + strMedEndDate + "'");
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine( "        AND C.MEDDEPTCD <> 'NP'");
            }

            if (GbViewFMChart == false)
            {
                SQL.AppendLine("        AND C.MEDDEPTCD <> 'FM'");
            }

            #endregion //Text EMR

            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                SQL.AppendLine( "    UNION ALL");
                SQL.AppendLine( "    SELECT A.EMRNO, A.FORMNO,  1 AS UPDATENO, A.CHARTDATE, A.CHARTTIME,  ");
                SQL.AppendLine( "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ");
                SQL.AppendLine( "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ");
                SQL.AppendLine( "      B.FORMNAME,  B.GRPFORMNO,");
                SQL.AppendLine("       (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = B.GRPFORMNO) AS GRPFORMNAME, ");
                SQL.AppendLine("      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ");
                SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN, 0 AS EMRNOHIS  ");
                SQL.AppendLine( "    FROM ADMIN.EMRXML_TUYAK A INNER JOIN ADMIN.AEMRFORM B ");
                SQL.AppendLine( "          ON A.FORMNO = B.FORMNO ");
                SQL.AppendLine( "          AND B.UPDATENO = 1 ");
                SQL.AppendLine( "      WHERE A.PTNO = '" + mPTNO + "' ");
                SQL.AppendLine( "        AND A.INOUTCLS = '" + strInOutCls + "'");
                if (strOption == "")
                {
                    SQL.AppendLine( "        AND A.MEDFRDATE = '" + strMedFrDate + "'");
                    if (strInOutCls == "O")
                    {
                        if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                        {
                            SQL.AppendLine( "                AND A.MEDDEPTCD = 'MD'");
                            SQL.AppendLine( "                AND A.MEDDRCD IN ('1107','1125') ");
                        }
                        else
                        {
                            SQL.AppendLine( "                AND A.MEDDEPTCD IN (" + strMedDeptCd + ")");
                            SQL.AppendLine( "                AND A.MEDDRCD NOT IN ('1107','1125')");
                        }
                    }
                }
                else
                {
                    SQL.AppendLine( "        AND A.MEDFRDATE >= '" + strMedFrDate + "'");
                    SQL.AppendLine( "        AND A.MEDFRDATE <= '" + strMedEndDate + "'");
                }
                if (mViewNpChart == false)
                {
                    SQL.AppendLine( "        AND A.MEDDEPTCD <> 'NP'");
                }

                if (GbViewFMChart == false)
                {
                    SQL.AppendLine("        AND A.MEDDEPTCD <> 'FM'");
                }
            }

            if (strInOutCls == "O")
            {
                bool isScanYn = FindScanImageYn(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate);

                if (isScanYn == false)
                {
                    SQL.AppendLine( "    UNION ALL ");
                    SQL.AppendLine( "    SELECT ");
                    SQL.AppendLine( "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO,  1 AS UPDATENO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                    SQL.AppendLine( "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                    SQL.AppendLine( "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && strMedMedDrCd == "1107"))
                    {
                        SQL.AppendLine( "      'RA' AS MEDDEPTCD, ");
                        SQL.AppendLine( "      '19094' AS MEDDRCD,  ");
                    }
                    else if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && strMedMedDrCd == "1125"))
                    {
                        SQL.AppendLine( "      'RA' AS MEDDEPTCD, ");
                        SQL.AppendLine( "      '30322' AS MEDDRCD,  ");
                    }
                    else
                    {
                        SQL.AppendLine( "      O.DEPTCODE AS MEDDEPTCD, ");
                        SQL.AppendLine( "      max(O.DRCODE) AS MEDDRCD,  ");
                    }
                    SQL.AppendLine( "      'Dr ORDER' AS FORMNAME ,  ");
                    SQL.AppendLine("      1012 AS GRPFORMNO,  ");
                    SQL.AppendLine( "      '의사지시기록' AS GRPFORMNAME,  ");
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                    {
                        SQL.AppendLine( "      '류마티스내과' AS DEPTKORNAME,  ");
                    }
                    else
                    {
                        SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ");
                    }
                    SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 0 AS EMRNOHIS   ");
                    SQL.AppendLine( "    FROM " + ComNum.DB_MED + "OCS_OORDER O, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                    SQL.AppendLine( "          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                    SQL.AppendLine( "      WHERE O.PTNO = '" + mPTNO + "' ");
                    SQL.AppendLine( "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");

                    if (strOption == "")
                    {
                        SQL.AppendLine( "          AND O.BDATE = TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                        if (strInOutCls == "O")
                        {
                            if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                            {
                                SQL.AppendLine( "                AND O.DEPTCODE = 'MD'");
                                SQL.AppendLine( "                AND O.DRCODE IN ('1107','1125') ");
                            }
                            else
                            {
                                SQL.AppendLine( "                AND O.DEPTCODE IN(" + strMedDeptCd + ")");
                                SQL.AppendLine( "                AND O.DRCODE NOT IN ('1107','1125')");
                            }
                        }
                    }
                    else
                    {
                        SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                        SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ");
                    }

                    if (mViewNpChart == false)
                    {
                        SQL.AppendLine( "        AND O.DEPTCODE <> 'NP'");
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'FM'");
                    }

                    SQL.AppendLine( "          AND    O.GBSUNAP ='1' AND O.Seqno    > '0'   AND O.NAL      > '0' ");
                    SQL.AppendLine( "          AND    O.SlipNo     =  C.SlipNo(+)       ");
                    SQL.AppendLine( "          AND    O.OrderCode  =  C.OrderCode(+)    ");
                    SQL.AppendLine( "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                    SQL.AppendLine( "          AND    O.DosCode    =  D.DosCode(+)      ");
                    SQL.AppendLine( "          AND    O.DRCODE      =  N.SABUN(+)       ");
                    SQL.AppendLine( "          AND    O.SUCODE = S.SUNEXT(+)  ");
                    SQL.AppendLine( "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                    SQL.AppendLine( "    UNION ALL ");
                    SQL.AppendLine( " SELECT ");
                    SQL.AppendLine( "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 2090 AS FORMNO,  1 AS UPDATENO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                    SQL.AppendLine( "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                    SQL.AppendLine( "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                    SQL.AppendLine( "      O.DEPTCODE AS MEDDEPTCD, ");
                    SQL.AppendLine( "      max(O.DRCODE) AS MEDDRCD,  ");
                    SQL.AppendLine( "      'Dr ORDER(ER)' AS FORMNAME ,  ");
                    SQL.AppendLine( "      1012 AS GRPFORMNO,  ");
                    SQL.AppendLine( "      'ER 의사지시기록' AS GRPFORMNAME,  ");
                    SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = 'ER') AS DEPTKORNAME,  ");
                    SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 0 AS EMRNOHIS  ");
                    SQL.AppendLine( "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                    SQL.AppendLine( "          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                    SQL.AppendLine( "      WHERE O.PTNO = '" + mPTNO + "' ");
                    SQL.AppendLine( "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");
                    if (strOption == "")
                    {
                        if (VB.Val(strMedEndDate) != 0)
                        {
                            SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                            SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ");
                        }
                        else
                        {
                            if (clsEmrQueryPohangS.NEXTDATE(clsDB.DbCon, strMedFrDate, mPTNO) == true)
                            {
                                SQL.AppendLine( "          AND O.BDATE = TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                            }
                            else
                            {
                                SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                                SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + DateTime.ParseExact(strMedFrDate, "yyyyMMdd", null).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')  ");
                            }
                        }
                    }
                    else
                    {
                        SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                        SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ");
                    }

                    SQL.AppendLine( "          AND O.GBSTATUS NOT IN ('D-','D','D+' )   ");
                    SQL.AppendLine( "          AND    O.SlipNo     =  C.SlipNo(+)       ");
                    SQL.AppendLine( "          AND    O.OrderCode  =  C.OrderCode(+)    ");
                    SQL.AppendLine( "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                    SQL.AppendLine( "          AND    O.DosCode    =  D.DosCode(+)      ");
                    SQL.AppendLine( "          AND    O.DRCODE      =  N.SABUN(+)       ");
                    SQL.AppendLine( "          AND    O.SUCODE = S.SUNEXT(+)  ");
                    SQL.AppendLine( "          AND    O.GBIOE IN ('E','EI')");
                    SQL.AppendLine( "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                }
            }
            else
            {
                bool isScanYn = FindScanImageYn(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate);
                if (isScanYn == false)
                {
                    SQL.AppendLine( "    UNION ALL ");
                    SQL.AppendLine( " SELECT ");
                    SQL.AppendLine( "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO,  1 AS UPDATENO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ");
                    SQL.AppendLine( "      0 AS ACPNO, O.PTNO, 'I' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ");
                    SQL.AppendLine( "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ");
                    if (strMedDeptCd == "MD" && strMedMedDrCd == "1107")
                    {
                        SQL.AppendLine( "      'RA' AS MEDDEPTCD, ");
                        SQL.AppendLine( "      '19094' AS MEDDRCD,  ");
                    }
                    else if (strMedDeptCd == "MD" && strMedMedDrCd == "1125")
                    {

                        SQL.AppendLine( "      'RA' AS MEDDEPTCD, ");
                        SQL.AppendLine( "      '30322' AS MEDDRCD,  ");
                    }
                    else
                    {
                        SQL.AppendLine( "      O.DEPTCODE AS MEDDEPTCD, ");
                        SQL.AppendLine( "      max(O.DRCODE) AS MEDDRCD,  ");
                    }
                    SQL.AppendLine( "      'Dr ORDER' AS FORMNAME ,  ");
                    SQL.AppendLine( "      1012 AS GRPFORMNO,  ");
                    SQL.AppendLine( "      '의사지시기록' AS GRPFORMNAME,  ");
                    SQL.AppendLine( "      (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ");
                    SQL.AppendLine("      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN, 0 AS EMRNOHIS  ");
                    SQL.AppendLine( "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ");
                    SQL.AppendLine( "          " + ComNum.DB_MED + "OCS_DOCTOR N, ");
                    SQL.AppendLine( "          " + ComNum.DB_PMPA + "BAS_SUN     S ");
                    SQL.AppendLine( "      WHERE O.PTNO = '" + mPTNO + "' ");
                    SQL.AppendLine( "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ");
                    if (mViewNpChart == false)
                    {
                        SQL.AppendLine( "        AND O.DEPTCODE <> 'NP'");
                    }

                    if (GbViewFMChart == false)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'FM'");
                    }

                    if (strOption == "")
                    {
                        if (strInOutCls == "O")
                        {
                            if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                            {
                                SQL.AppendLine( "                AND O.DEPTCODE = 'MD'");
                                SQL.AppendLine( "                AND O.DRCODE IN ('1107','1125') ");
                            }
                            else
                            {
                                SQL.AppendLine( "                AND O.DEPTCODE IN(" + strMedDeptCd + ")");
                                SQL.AppendLine( "                AND O.DRCODE NOT IN ('1107','1125') ");
                            }
                        }
                        SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                        if (VB.Val(strMedEndDate) != 0)
                        {
                            SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ");
                        }
                    }
                    else
                    {
                        SQL.AppendLine( "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ");
                        SQL.AppendLine( "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ");
                    }

                    SQL.AppendLine( "          AND O.GBSTATUS NOT IN ('D-','D','D+' )   ");
                    SQL.AppendLine( "          AND    O.SlipNo     =  C.SlipNo(+)       ");
                    SQL.AppendLine( "          AND    O.OrderCode  =  C.OrderCode(+)    ");
                    SQL.AppendLine( "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ");
                    SQL.AppendLine( "          AND    O.DosCode    =  D.DosCode(+)      ");
                    SQL.AppendLine( "          AND    O.DRCODE      =  N.SABUN(+)       ");
                    SQL.AppendLine( "          AND    O.SUCODE = S.SUNEXT(+)  ");
                    SQL.AppendLine( "          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')");
                    SQL.AppendLine( "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ");
                }
            }
            //전자동의서
            SQL.AppendLine( "    UNION ALL");
            SQL.AppendLine( "          SELECT C.ID AS EMRNO, A.FORMNO, A.UPDATENO, TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME,     ");
            SQL.AppendLine( "          0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,     ");
            SQL.AppendLine( "          A.FORMNAME,  A.GRPFORMNO,     ");
            SQL.AppendLine( "          (SELECT GRPFORMNAME FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = A.GRPFORMNO) AS GRPFORMNAME,     ");
            SQL.AppendLine( "          (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME,     ");
            SQL.AppendLine("          0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'E' AS SCANYN, 0 AS EMRNOHIS     ");
            SQL.AppendLine( "          FROM ADMIN.AEMRFORM A     ");
            SQL.AppendLine( "          INNER JOIN ADMIN.AEASFORMCONTENT B     ");
            SQL.AppendLine( "          ON A.FORMNO = B.FORMNO     ");
            SQL.AppendLine( "          AND A.UPDATENO = B.UPDATENO     ");
            SQL.AppendLine( "          INNER JOIN ADMIN.AEASFORMDATA C     ");
            SQL.AppendLine( "          ON B.ID = C.EASFORMCONTENT     ");
            SQL.AppendLine( "          WHERE C.ISDELETED = 'N' AND C.PTNO = '" + mPTNO + "'     ");
            SQL.AppendLine( "          AND C.INOUTCLS = '" + strInOutCls + "'     ");
            SQL.AppendLine( "          AND C.MEDFRDATE = '" + strMedFrDate + "'     ");

            SQL.AppendLine("    ) ET ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRGRPFORM GR");
            SQL.AppendLine("       ON GR.GRPFORMNO = (SELECT GROUPPARENT FROM ADMIN.AEMRGRPFORM WHERE GRPFORMNO = ET.GRPFORMNO)");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRGRPFORM GR2");
            SQL.AppendLine("       ON GR2.GRPFORMNO = ET.GRPFORMNO");
            SQL.AppendLine("     LEFT OUTER JOIN ADMIN.BAS_CLINICDEPT CD");
            SQL.AppendLine("       ON CD.DEPTCODE = ET.MEDDEPTCD");

            if (cboDept.SelectedIndex != 0)
            {
                SQL.AppendLine("    WHERE ET.MEDDEPTCD IN(" + strMedDeptCd + ")");
            }

            if (chkAllCopy.Checked)
            {
                SQL.AppendLine("ORDER BY ET.INOUTCLS DESC, GR.DEPTH, GR.DISPSEQ, GR2.DISPSEQ, (ET.CHARTDATE || ET.CHARTTIME) DESC");
            }
            else
            {
                SQL.AppendLine("ORDER BY ET.INOUTCLS DESC, CD.PRINTRANKING, GR.DEPTH, GR.DISPSEQ, GR2.DISPSEQ, (ET.CHARTDATE || ET.CHARTTIME) DESC");
            }

            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                Cursor.Current = Cursors.Default;
                return;
            }

            #region 1. 간호사 2. 심사팀 아님 3. 응급실 아닐경우만 오늘 검사변환 내역 없으면 변환창
            DateTime dtpSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
            if (clsEmrPublic.gUserGrade.Equals("SIMSA") == false && clsType.User.IsNurse.Equals("OK") &&
                clsType.User.BuseCode.Equals("033109") == false && strInOutCls.Equals("I") && (clsEmrFunc.IsNowHolyDay(clsDB.DbCon) || dtpSysDate.Hour >= 17 || dtpSysDate.DayOfWeek == DayOfWeek.Saturday && dtpSysDate.Hour >= 12))
            {
                string TreatNo = dt.AsEnumerable().Max(d => d["ACPNO"]).ToString();
                if (TreatNo.Equals("0") == false && clsEmrFunc.IsImageCvt(clsDB.DbCon, TreatNo))
                {
                    using (Form frm = new frmNrImgCvt(TreatNo))
                    {
                        frm.ControlBox = false;
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog(this);
                    }
                }
            }
            #endregion



            Dictionary<string, TreeNode> _treeNodes = new Dictionary<string, TreeNode>();
            TreeNode treeNode = null;
            string strMedDeptCd1 = "";
            string strMedMedDrCd1 = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                sKey1 = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                strMedDeptCd1 = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                strMedMedDrCd1 = dt.Rows[i]["MEDDRCD"].ToString().Trim();

                sKey2 = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                sKeyName2 = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();

                sKey3 = dt.Rows[i]["FORMNO"].ToString().Trim();
                sKeyName3 = dt.Rows[i]["FORMNAME"].ToString().Trim();

                sKey4 = dt.Rows[i]["UPDATENO"].ToString().Trim();
                sKeyName4 = dt.Rows[i]["FORMNAME"].ToString().Trim() + (dt.Rows[i]["SCANYN"].ToString().Trim().Equals("S") ? "(스캔)" : "");

                sKey5 = dt.Rows[i]["SCANYN"].ToString().Trim() + "|" + dt.Rows[i]["EMRNO"].ToString().Trim() + "|" + dt.Rows[i]["FORMCODE"].ToString().Trim() + "|" + dt.Rows[i]["EMRNOHIS"].ToString().Trim();

                sChartDT = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "DS") + " " +  ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");

                if (sKey1 == "O")
                {
                    sKeyName1 = "외래";
                }
                else
                {
                    sKeyName1 = "입원";
                }

                if (chkAllCopy.Checked == false)
                {
                    #region 기존 방식
                    //if (sKeyOld1 != sKey1)
                    //{
                    //    oNodex = trvEmrView.Nodes.Add(sKeyHead + sKey1, sKeyName1, 2, 1);
                    //    sKeyOld1 = sKey1;

                    //    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                    //    sKeyOld2 = sKey2;

                    //    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                    //    sKeyOld3 = sKey3;

                    //    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                    //    sKeyOld4 = sKey4;
                    //}
                    //else
                    //{
                    //    if (sKeyOld2 != sKey2)
                    //    {
                    //        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                    //        sKeyOld2 = sKey2;
                    //        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                    //        sKeyOld3 = sKey3;
                    //        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                    //        sKeyOld4 = sKey4;
                    //    }
                    //    else
                    //    {
                    //        if (sKeyOld3 != sKey3)
                    //        {
                    //            oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                    //            sKeyOld3 = sKey3;
                    //            oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                    //            sKeyOld4 = sKey4;
                    //        }
                    //        else
                    //        {
                    //            if (sKeyOld5 != sKey5)
                    //            {
                    //                oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                    //                sKeyOld5 = sKey5;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    #region 수정
                    if(_treeNodes.ContainsKey(sKeyHead + sKey1) ==  false)
                    {
                        treeNode = trvEmrView.Nodes.Add(sKeyHead + sKey1, sKeyName1, 2, 1);
                        _treeNodes.Add(sKeyHead + sKey1, treeNode);

                        treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                        _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, treeNode);

                        treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                        _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, treeNode);

                        treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                        //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                    }
                    else
                    {
                        //과 없을 경우
                        if (_treeNodes.TryGetValue(sKeyHead + sKey1 + sKeyHead + sKey2, out treeNode) == false)
                        {
                            _treeNodes.TryGetValue(sKeyHead + sKey1, out treeNode);
                            //과를 추가한다
                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                            _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, treeNode);

                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                            _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, treeNode);

                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                            //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                        }
                        else
                        {

                            if (_treeNodes.TryGetValue(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, out treeNode) == false)
                            {
                                _treeNodes.TryGetValue(sKeyHead + sKey1 + sKeyHead + sKey2, out treeNode);

                                treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                                _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, treeNode);

                                treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                                //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                            }
                            else
                            {
                                treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                                //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                            }
                        }
                    }
                  
                    #endregion
                }
                else
                {
                    #region 과 구분 없이 기록지 전부
                    if (_treeNodes.ContainsKey(sKeyHead + sKey1) == false)
                    {
                        //입,외 구분
                        treeNode = trvEmrView.Nodes.Add(sKeyHead + sKey1, sKeyName1, 2, 1);
                        _treeNodes.Add(sKeyHead + sKey1, treeNode);

                        treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3, sKeyName3, 2, 1);
                        _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3, treeNode);

                        treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                        treeNode.Tag = sKey2;
                        //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                    }
                    else
                    {
                        //기록지 없을경우
                        if(_treeNodes.TryGetValue(sKeyHead + sKey1 + sKeyHead + sKey3, out treeNode) == false)
                        {
                            //기록지 추가한다
                            _treeNodes.TryGetValue(sKeyHead + sKey1, out treeNode);

                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3, sKeyName3, 2, 1);
                            _treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3, treeNode);

                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                            treeNode.Tag = sKey2;
                            //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                        }
                        else
                        {
                            //기록지 항목에 기록지리스트 넣기
                            treeNode = treeNode.Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                            treeNode.Tag = sKey2;
                            //_treeNodes.Add(sKeyHead + sKey1 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, treeNode);
                        }
                    }
                    #endregion
                }
            }

            if (clsType.User.BuseCode.Equals("044201"))
            {
                #region 차트복사 진료기록지 일경우 굵게, 차트갯수 표시
                if (chkAllCopy.Checked)
                {
                    var query =
                    from product in dt.AsEnumerable().Where(dr =>
                    dr.Field<decimal>("GRPFORMNO") == (1000) ||
                    dr.Field<decimal>("GRPFORMNO") == (1009) ||
                    dr.Field<decimal>("GRPFORMNO") == (1002) ||
                    dr.Field<decimal>("GRPFORMNO") == (1001) ||
                    dr.Field<decimal>("GRPFORMNO") == (1003) ||
                    dr.Field<decimal>("GRPFORMNO") == (1011) ||
                    dr.Field<decimal>("GRPFORMNO") == (1010) ||
                    dr.Field<decimal>("GRPFORMNO") == (1006) ||
                    dr.Field<decimal>("GRPFORMNO") == (1007) ||
                    dr.Field<decimal>("GRPFORMNO") == (1004) ||
                    dr.Field<decimal>("GRPFORMNO") == (1005) ||
                    dr.Field<decimal>("GRPFORMNO") == (1008) ||
                    dr.Field<decimal>("GRPFORMNO") == (1012) ||
                    //의사지시(1012) Dr Order
                    dr.Field<decimal>("GRPFORMNO") == (1075) ||
                    dr.Field<decimal>("GRPFORMNO") == (1074))
                    group product by product.Field<decimal>("FORMNO") into g
                    select new { FORMNO = g.Key, CHARTCNT = g.Count() };

                    foreach (var product in query)
                    {
                        string SearchText = "^" + product.FORMNO;
                        List<string> lstForm = _treeNodes.Keys.Where(d => d.IndexOf(SearchText) != -1).ToList();

                        for (int index = 0; index < lstForm.Count; index++)
                        {
                            if (_treeNodes.TryGetValue(lstForm[index], out treeNode))
                            {
                                //Console.WriteLine("FORMNAME = {0} \t ProductCount = {1}",
                                //    product.FORMNO,
                                //    product.CHARTCNT);
                                treeNode.NodeFont = boldFont;
                                treeNode.Text += (product.FORMNO != 1680 && product.FORMNO != 2090 ? " (" + product.CHARTCNT + ")" : "");
                            }
                        }
                    }
                }
                else
                {
                    var query = dt.AsEnumerable().Where(dr =>
                    dr.Field<decimal>("GRPFORMNO") == (1000) ||
                    dr.Field<decimal>("GRPFORMNO") == (1009) ||
                    dr.Field<decimal>("GRPFORMNO") == (1002) ||
                    dr.Field<decimal>("GRPFORMNO") == (1001) ||
                    dr.Field<decimal>("GRPFORMNO") == (1003) ||
                    dr.Field<decimal>("GRPFORMNO") == (1011) ||
                    dr.Field<decimal>("GRPFORMNO") == (1010) ||
                    dr.Field<decimal>("GRPFORMNO") == (1006) ||
                    dr.Field<decimal>("GRPFORMNO") == (1007) ||
                    dr.Field<decimal>("GRPFORMNO") == (1004) ||
                    dr.Field<decimal>("GRPFORMNO") == (1005) ||
                    dr.Field<decimal>("GRPFORMNO") == (1008) ||
                    dr.Field<decimal>("GRPFORMNO") == (1012) ||
                    //의사지시(1012) Dr Order
                    dr.Field<decimal>("GRPFORMNO") == (1075) ||
                    dr.Field<decimal>("GRPFORMNO") == (1074)).GroupBy
                    (
                        d => new
                        {
                            FORMNO = d.Field<decimal>("FORMNO"),
                            DEPTCD = d.Field<string>("MEDDEPTCD"),
                        }
                    ).Select(g => new { KEY = g.Key, CHARTCNT = g.Count() });

                    foreach (var product in query)
                    {
                        string SearchText = "^" + product.KEY.DEPTCD + "^" + product.KEY.FORMNO;
                        List<string> lstForm = _treeNodes.Keys.Where(d => d.IndexOf(SearchText) != -1).ToList();

                        for (int index = 0; index < lstForm.Count; index++)
                        {
                            if (_treeNodes.TryGetValue(lstForm[index], out treeNode))
                            {
                                treeNode.NodeFont = boldFont;
                                treeNode.Text += (product.KEY.FORMNO != 1680 && product.KEY.FORMNO != 2090 ? " (" + product.CHARTCNT + ")" : "");
                            }
                        }
                    }
                }
                #endregion
            }
            dt.Dispose();
            dt = null;
        }

        private void GetHisSheet(string strDept)
        {
            bool IsNurseNA = clsEmrFunc.IsNurseNA(clsDB.DbCon);

            //if (IsNurseNA)// || clsEmrPublic.gUserGrade.Equals("XRAY"))
            if (IsNurseNA || clsType.User.AuAVIEW.Equals("0") && clsType.User.AuAIMAGE.Equals("0"))
                return;

            DataTable dt = null;
            StringBuilder SQL   = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            trvEmrView.Nodes.Clear();
            ssViewEmrAcpCopy_Sheet1.RowCount = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            SQL.AppendLine("SELECT ");
            SQL.AppendLine( "  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE,");
            SQL.AppendLine( "  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME,");
            SQL.AppendLine( "  (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME, DD.DRNAME");
            SQL.AppendLine("  , CASE WHEN XX.INOUTCLS = 'I' AND EXISTS ");
            SQL.AppendLine( "  ( ");
            SQL.AppendLine("    SELECT 1                                                       	    ");
            SQL.AppendLine("    FROM ADMIN.IPD_NEW_MASTER                                     ");
            SQL.AppendLine("    WHERE PANO = XX.PTNO                                                ");
            SQL.AppendLine("      AND INDATE >= TO_DATE(XX.MEDFRDATE,'YYYYMMDD')     				");
            SQL.AppendLine("      AND INDATE <= TO_DATE(XX.MEDFRDATE,'YYYYMMDD')     				");
            SQL.AppendLine("      AND DEPTCODE = XX.MEDDEPTCD                                       ");
            SQL.AppendLine("      AND OUTDATE = TO_DATE(XX.MEDENDDATE,'YYYYMMDD' )                  ");
            SQL.AppendLine("      AND GBSTS = '9'                                                   ");
            SQL.AppendLine("   ) THEN '입원취소' END GETIPDCANCEL                                    ");

            SQL.AppendLine("  , CASE WHEN EXISTS ");
            SQL.AppendLine("  ( ");
            SQL.AppendLine("    SELECT 1                                                       	    ");
            SQL.AppendLine("    FROM ADMIN.OCS_MCCERTIFI_WONMU_REPRINT                         ");
            SQL.AppendLine("    WHERE PANO = XX.PTNO                                                ");
            SQL.AppendLine("      AND BDATE = TO_DATE(XX.MEDFRDATE,'YYYYMMDD')     				    ");
            SQL.AppendLine("      AND DEPTCODE = XX.MEDDEPTCD                                       ");
            SQL.AppendLine("   ) THEN '1' END READ_DOCREPRINT                                       ");

            SQL.AppendLine("  , CASE WHEN XX.INOUTCLS = 'I' AND EXISTS ");
            SQL.AppendLine("  ( ");
            SQL.AppendLine("    SELECT 1                                                       	    ");
            SQL.AppendLine("    FROM ADMIN.IPD_NEW_MASTER                                     ");
            SQL.AppendLine("    WHERE PANO = XX.PTNO                                                ");
            SQL.AppendLine("      AND INDATE >= TO_DATE(XX.MEDFRDATE || ' 000000','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND INDATE <= TO_DATE(XX.MEDFRDATE || ' 235959','YYYYMMDD HH24MISS')     				");
            SQL.AppendLine("      AND GBSTS NOT IN ('9')                                            ");
            SQL.AppendLine("      AND AMSET7 IN ('3','4','5')                                       ");
            SQL.AppendLine("   ) THEN '1' END ER_IPWON                                              ");

            SQL.AppendLine("FROM (");


            if (optEmrInOutCopyO.Checked == true || optEmrInOutCopyA.Checked == true)
            {
                SQL.AppendLine( " SELECT 'O' AS INOUTCLS, A.Pano AS PTNO,A.SName AS PTNAME, A.Sex, A.Age, ");
                SQL.AppendLine( "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                SQL.AppendLine( "     NVL(TO_CHAR(A.BDATE,'YYYYMMDD'),  TO_CHAR(A.ACTDATE,'YYYYMMDD')) AS MEDFRDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDFRTIME,");
                SQL.AppendLine( "    '' AS MEDENDDATE, '' AS MEDENDTIME");
                SQL.AppendLine( "FROM ADMIN.OPD_MASTER A");
                SQL.AppendLine( "WHERE A.PANO = '" + mPTNO + "' ");
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL.AppendLine("  AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
                }
                else
                {
                    SQL.AppendLine("  AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")");
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL.AppendLine("  AND A.GBUSE = 'Y'");
                }
            }

            if (optEmrInOutCopyA.Checked == true)
            {
                SQL.AppendLine( "UNION ALL");
            }

            if (optEmrInOutCopyI.Checked == true || optEmrInOutCopyA.Checked == true)
            {
                SQL.AppendLine( " SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age, ");
                SQL.AppendLine( "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,");
                SQL.AppendLine( "    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,");
                SQL.AppendLine( "    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME");
                SQL.AppendLine( "FROM ADMIN.IPD_NEW_MASTER A ");
                SQL.AppendLine( "WHERE A.PANO = '" + mPTNO + "' ");
                SQL.AppendLine( "  AND A.GBSTS <> '9'");
                SQL.AppendLine( "UNION ALL");
                if (clsEmrPublic.GstrView01 == "1")
                {
                    SQL.AppendLine( "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ");
                    SQL.AppendLine( "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ");
                    SQL.AppendLine( "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ");
                    SQL.AppendLine( "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME ");
                    SQL.AppendLine( "FROM ADMIN.EMRXMLMST A, ");
                    SQL.AppendLine( "    ADMIN.IPD_NEW_MASTER B ");
                    SQL.AppendLine( "WHERE A.PTNO =  '" + mPTNO + "' ");
                    SQL.AppendLine( "  AND A.INOUTCLS = 'I' ");
                    SQL.AppendLine( "  AND B.GBSTS = '9' ");
                    SQL.AppendLine( "  AND A.PTNO = B.PANO ");
                    SQL.AppendLine( "  AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ");
                    SQL.AppendLine( "  AND A.MEDDEPTCD = B.DeptCode ");
                    SQL.AppendLine( "UNION ALL ");
                    SQL.AppendLine( "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ");
                    SQL.AppendLine( "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ");
                    SQL.AppendLine( "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ");
                    SQL.AppendLine( "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME ");
                    SQL.AppendLine( "FROM ADMIN.AEMRCHARTMST A, ");
                    SQL.AppendLine( "    ADMIN.IPD_NEW_MASTER B ");
                    SQL.AppendLine( "WHERE A.PTNO =  '" + mPTNO + "' ");
                    SQL.AppendLine( "  AND A.INOUTCLS = 'I' ");
                    SQL.AppendLine( "  AND B.GBSTS = '9' ");
                    SQL.AppendLine( "  AND A.PTNO = B.PANO ");
                    SQL.AppendLine( "  AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ");
                    SQL.AppendLine( "  AND A.MEDDEPTCD = B.DeptCode ");
                }
                else if (clsEmrPublic.GstrView01 == "0" || clsEmrPublic.GstrView01 == "")
                {
                    SQL.AppendLine( "SELECT 'I' AS INOUTCLS, PANO AS PTNO, SName AS PTNAME, SEX, AGE,");
                    SQL.AppendLine( "    DEPTCODE AS MEDDEPTCD, DRCODE AS MEDDRCD,");
                    SQL.AppendLine( "    TO_CHAR(INDATE,'YYYYMMDD') AS MEDFRDATE, TO_CHAR(INDATE, 'HH24MI') AS MEDFRTIME,");
                    SQL.AppendLine( "    TO_CHAR(OUTDATE,'YYYYMMDD') AS MEDENDDATE, '' AS MEDENDTIME");
                    SQL.AppendLine( "    From ADMIN.IPD_NEW_MASTER");
                    SQL.AppendLine( "   WHERE PANO =  '" + mPTNO + "'");
                    SQL.AppendLine( "     AND GBSTS = '9'");
                }
            }

            SQL.AppendLine( "UNION ALL ");

            SQL.AppendLine( " SELECT A.CLASS AS INOUTCLS, A.PATID AS PTNO,  B.NAME AS PTNAME, B.Sex, 0 AS Age,  ");
            SQL.AppendLine( "    A.CLINCODE AS MEDDEPTCD, A.DOCCODE AS MEDDRCD, ");
            SQL.AppendLine( "    A.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ");
            SQL.AppendLine( "    A.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME ");
            SQL.AppendLine( "FROM ADMIN.EMR_TREATT A, ADMIN.EMR_PATIENTT B   ");
            SQL.AppendLine( "WHERE A.PATID = '" + mPTNO + "' ");
            SQL.AppendLine( "  AND A.DELDATE IS NULL");
            SQL.AppendLine( "  AND A.PATID = B.PATID    ");
            if (optEmrInOutCopyO.Checked == true)
            {
                SQL.AppendLine("  AND A.CLASS = 'O'");
            }
            else if (optEmrInOutCopyI.Checked == true)
            {
                SQL.AppendLine("  AND A.CLASS = 'I'");
            }
            SQL.AppendLine("  AND (A.CLASS, A.INDATE, A.CLINCODE) NOT IN ( ");
            SQL.AppendLine( "            SELECT INOUTCLS, MEDFRDATE, MEDDEPTCD ");
            SQL.AppendLine( "            FROM ");
            SQL.AppendLine( "            (SELECT 'O' AS INOUTCLS, NVL(TO_CHAR(A1.BDATE,'YYYYMMDD'),  TO_CHAR(A1.ACTDATE,'YYYYMMDD') ) AS MEDFRDATE, ");
            SQL.AppendLine( "            DECODE(A1.DRCODE,'1107','RA','1125','RA',A1.DeptCode) AS MEDDEPTCD");
            SQL.AppendLine( "            FROM ADMIN.OPD_MASTER A1");
            SQL.AppendLine( "            WHERE A1.PANO = '" + mPTNO + "' ");
            
            if (gJinGubun == "" || gJinGubun == "2")
            {
                SQL.AppendLine("              AND A1.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
            }
            else
            {
                SQL.AppendLine("              AND A1.Jin    IN ('0','1','2','3','4','5','6','7','8','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ");
            }

            if (clsEmrPublic.gUserGrade == "SIMSA" || clsEmrPublic.gUserGrade == "")
            {
                SQL.AppendLine("              AND A1.GBUSE = 'Y' ");
            }

            SQL.AppendLine( "            UNION ALL ");
            SQL.AppendLine( "             SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, A2.DeptCode AS MEDDEPTCD ");
            SQL.AppendLine( "            FROM ADMIN.IPD_NEW_MASTER A2  ");
            SQL.AppendLine( "            WHERE A2.PANO = '" + mPTNO + "' ");
            SQL.AppendLine("               AND A2.GBSTS <> '9') ");
            SQL.AppendLine( "    )  ");

            SQL.AppendLine( ") XX, ADMIN.BAS_DOCTOR DD ");
            SQL.AppendLine( "  WHERE XX.MEDDRCD = DD.DRCODE(+)");
            SQL.AppendLine( "  AND XX.INOUTCLS IS NOT NULL");
            if (strDept.Length > 0)
            {
                SQL.AppendLine( "  AND XX.MEDDEPTCD = '" + strDept + "'");
            }

            if (optEmrInOutCopyO.Checked && chkDate.Checked)
            {
                SQL.AppendLine("  AND XX.MEDFRDATE >= '" + dtpDateCopyS.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("  AND XX.MEDFRDATE <= '" + dtpDateCopyE.Value.ToString("yyyyMMdd") + "'");

            }

            SQL.AppendLine("ORDER BY XX.INOUTCLS ASC, XX.MEDFRDATE DESC, DECODE(DRNAME, NULL, 1, 0),  XX.MEDDEPTCD");


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssViewEmrAcpCopy_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strMedFrDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                string strMedEndDate = string.Empty;
                if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "")
                {
                    strMedEndDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                }
                string strMedDEPTCODE = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();

                #region 전과 일경우 추가 안함
                bool Next = false;
                if (ssViewEmrAcpCopy_Sheet1.RowCount > 0 && dt.Rows[i]["INOUTCLS"].ToString().Trim() == "I" && dt.Rows[i]["DRNAME"].ToString().Trim().Length == 0)
                {
                    //입원일자 똑같음.
                    for (int j = 0; j < ssViewEmrAcpCopy_Sheet1.RowCount; j++)
                    {
                        if (ssViewEmrAcpCopy_Sheet1.Cells[j, 1].Text.Trim().Equals(strMedFrDate))
                        {
                            ssViewEmrAcpCopy_Sheet1.Cells[j, 12].Text += strMedDEPTCODE + ",";
                            Next = true;
                            break;
                        }
                    }

                    if (Next)
                        continue;
                }
                #endregion

                ssViewEmrAcpCopy_Sheet1.RowCount += 1;
                int nRow = ssViewEmrAcpCopy_Sheet1.RowCount - 1;
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 1].Text = strMedFrDate;

                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "I")
                {
                    //if (clsEmrQueryPohangS.GetIPDCancel(clsDB.DbCon, mPTNO, strMedFrDate, strMedEndDate, strMedDEPTCODE) == "입원취소" && dt.Rows[i]["MEDENDTIME"].ToString().Trim() == "")
                    if (dt.Rows[i]["GETIPDCANCEL"].ToString().Equals("입원취소") && dt.Rows[i]["MEDENDTIME"].ToString().Trim() == "")
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Text = "입원취소";
                    }
                    else
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Text = strMedEndDate;
                    }

                    if ((clsType.User.BuseCode.Equals("044201") || clsType.User.JobGroup.Equals("JOB002002") || clsType.User.JobGroup.Equals("JOB002003")) &&  dt.Rows[i]["ER_IPWON"].ToString().Equals("1"))
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(254, 224, 224);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (dt.Rows[i]["READ_DOCREPRINT"].ToString().Equals("1"))
                    //if (clsEmrQueryPohangS.READ_DOCREPRINT(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) == true)
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Text = "서류재발급";
                    }
                    else
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[nRow, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                    }
                }
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 3].Text = strMedDEPTCODE;

                if ((dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1107" || dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1125") && dt.Rows[i]["MEDDEPTCD"].ToString().Trim() == "MD")
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[nRow, 4].Text = "류마티스내과";
                }
                else
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[nRow, 4].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                }
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 6].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "O")
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[nRow, 7].Text = "";
                }
                else
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[nRow, 7].Text = dt.Rows[i]["MEDENDTIME"].ToString().Trim();
                }
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 8].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 9].Text = strMedEndDate;
                ssViewEmrAcpCopy_Sheet1.Cells[nRow, 10].Text = strMedDEPTCODE;
                //ssViewEmrAcpCopy_Sheet1.Cells[i, 11].Text = ACPNO;
            }
            dt.Dispose();

            ssViewEmrAcpCopy_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (optEmrInOutDeptO.Checked == true)
            {
                ssViewEmrAcpCopy_Sheet1.Columns[2].Visible = false;
            }
            if (optEmrInOutDeptI.Checked == true)
            {
                ssViewEmrAcpCopy_Sheet1.Columns[2].Visible = true;
            }

            //for (int i = 0; i < ssViewEmrAcpCopy_Sheet1.RowCount; i++)
            //{
            //    string cIO = ssViewEmrAcpCopy_Sheet1.Cells[i, 0].Text.Trim();
            //    string cBDATE = ssViewEmrAcpCopy_Sheet1.Cells[i, 1].Text.Trim();
            //    string cDEPTCD = ssViewEmrAcpCopy_Sheet1.Cells[i, 10].Text.Trim();
            //    if (clsVbfunc.READ_SPECIAL_SERVICE(clsDB.DbCon, mPTNO, cBDATE, cDEPTCD, cIO) == true)
            //    {
            //        ssViewEmrAcpCopy_Sheet1.Cells[i, 0].BackColor = Color.Green;
            //    }
            //    else
            //    {
            //        ssViewEmrAcpCopy_Sheet1.Cells[i, 0].BackColor = Color.White;
            //    }
            //}

            Cursor.Current = Cursors.Default;

        }

        private void ssViewEmrAcpCopy_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpCopy_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true && clsType.User.BuseCode.Equals("044201") == false)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpCopy, e.Column);
                return;
            }

            trvEmrView.Nodes.Clear();
            pCopy = null;

        }

        private void ssViewEmrAcpCopyCellDoubleClick(int Row, int Column)
        {
            string strInOutCls = ssViewEmrAcpCopy_Sheet1.Cells[Row, 0].Text.Trim();
            string strMedFrDate = ssViewEmrAcpCopy_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            string strMedEndDate = ssViewEmrAcpCopy_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");
            string strMedDeptCd = ssViewEmrAcpCopy_Sheet1.Cells[Row, 3].Text.Trim();
            string strMedFrTime = ssViewEmrAcpCopy_Sheet1.Cells[Row, 6].Text.Trim();
            string strMedEndTime = ssViewEmrAcpCopy_Sheet1.Cells[Row, 7].Text.Trim();
            string strMedMedDrCd = ssViewEmrAcpCopy_Sheet1.Cells[Row, 8].Text.Trim();

            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "NP" && mViewNpChart == false || strMedDeptCd == "FM" && GbViewFMChart == false) 
                {
                    ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                    return;
                }
            }

            pCopy = clsEmrChart.ClearPatient();
            pCopy = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedDeptCd);
            //if (pCopy == null)
            //{
            //    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
            //    return;
            //}

            if (string.IsNullOrWhiteSpace(ssViewEmrAcpCopy_Sheet1.Cells[Row, 12].Text.Trim()))
            {
                strMedDeptCd = "'" + strMedDeptCd + "'";
            }
            else
            {
                strMedDeptCd = "'" + strMedDeptCd + "'";

                foreach (string str in ssViewEmrAcpCopy_Sheet1.Cells[Row, 12].Text.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        strMedDeptCd += ", '" + str + "'";
                    }
                }

                strMedDeptCd = VB.Left(strMedDeptCd, strMedDeptCd.Length - 1);
            }

            GetHisSheetDsp(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate, "");

        }

        private void trvEmrView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode childNode in e.Node.Nodes)
            {
                childNode.Checked = e.Node.Checked;
            }
        }

        private void trvEmrView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int ChkX = 0;
            switch(e.Node.Level)
            {
                case 2:
                    ChkX = 85;
                    break;
                case 3:
                    ChkX = 92;
                    break;
            }

            if (e.X < ChkX)
                return;

            TreeNode Node;
            Node = e.Node;
            string strIndex = Node.Name.ToString();

            if (Node.GetNodeCount(false) > 0)
            {
                //if (Node.IsExpanded == false)
                //{
                //    Node.Expand();
                //}
                return;
            }

            string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);

            string[] arryEmrNo;

            string strFormNo = string.Empty;
            string strUpdateNo = string.Empty;

            string strSCANYN = string.Empty; //E 이면 전자동의서
            string strEmrNo = string.Empty;
            string strFormCode = string.Empty;
            string strTreatNo = "0";


            //전체
            if (Node.Level == 2)
            {
                arryEmrNo = VB.Split(VB.Trim(strParams[4]), "|");

                strFormNo = strParams[2];
                strUpdateNo = strParams[3];

                strSCANYN = arryEmrNo[0]; //E 이면 전자동의서
                strEmrNo = arryEmrNo[1];
                strFormCode = arryEmrNo[2];


                strTreatNo = "0";
            }
            else//과 나뉘어질때
            {
                arryEmrNo = VB.Split(VB.Trim(strParams[5]), "|");

                strFormNo = strParams[3];
                strUpdateNo = strParams[4];

                strSCANYN = arryEmrNo[0]; //E 이면 전자동의서
                strEmrNo = arryEmrNo[1];
                strFormCode = arryEmrNo[2];

                strTreatNo = "0";
            }


            pView = clsEmrChart.ClearPatient();
            if (strSCANYN == "E")
            {
                pView = clsEmrChart.SetEmrPatInfoEas(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = (long)VB.Val(strFormNo);
                pView.updateNo = (int)VB.Val(strUpdateNo);
            }
            else if (strSCANYN == "S")
            {
                pView = clsEmrChart.SetEmrPatInfoScan(clsDB.DbCon, strEmrNo);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = 0;
                pView.updateNo = 1;
            }
            else if (strSCANYN == "O")
            {
                if (pCopy == null)
                {
                    pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strParams[1], arryEmrNo[1], strParams[2]);
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        return;
                    }
                }
                else
                {
                    pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strParams[1], pCopy.medFrDate, strFormNo.Equals("2090") ? "ER" : pCopy.medDeptCd);
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        return;
                    }
                }

                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                pView.medDeptCd = Node.Level == 2 ? (strFormNo.Equals("2090") ? "ER" : Node.Tag.ToString()) : strFormNo.Equals("2090") ? "ER" : strParams[2];
            }
            else
            {
                if (pCopy == null)
                {
                    pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        return;
                    }
                }
                else
                {
                    pView = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strParams[1], pCopy.medFrDate);
                    if (pView == null)
                    {
                        pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
                        if (pView == null)
                        {
                            ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                            return;
                        }
                    }
                }

                pView.medDeptCd = Node.Level == 2 ? Node.Tag.ToString() : strParams[2];
            }

            pView.formNo = (long)VB.Val(strFormNo);
            pView.updateNo = (int)VB.Val(strUpdateNo);

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (strSCANYN == "T" && VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            else
            {
                if (fView == null)
                {
                    return;
                }
            }

            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode, "", "");
        }

        private void btnExpend_Click(object sender, EventArgs e)
        {
            trvEmrView.ExpandAll();
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            trvEmrView.CollapseAll();
        }

        private void btnChartCopy_Click(object sender, EventArgs e)
        {
            if (SaveDataPrintReq() == true)
            {
                //ClearCopyReq();
                ComFunc.MsgBoxEx(this, "복사신청을 완료했습니다.");
            }

            foreach (TreeNode childNode in trvEmrView.Nodes)
            {
                childNode.Checked = false;
            }
        }

        private bool SaveDataPrintReq()
        {
            //string strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수", "1");

            //if (VB.Val(strCopyCnt) == 0)
            //{
            //    ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
            //    strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수");
            //    if (VB.Val(strCopyCnt) == 0)
            //    {
            //        ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
            //        return false;
            //    }
            //}

            string strCopyCnt = "1";

            List<String> strTree = CheckedNames(trvEmrView.Nodes);

            if (strTree.Count == 0)
            {
                ComFunc.MsgBoxEx(this, "기록지를 정확히 선택해 주십시요." + ComNum.VBLF + "선택된 기록지가 없습니다.");
                return false;
            }

            int i = 0;
            //DataTable /*dt*/ = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strREQDATE = VB.Left(strCurDateTime, 8);
                string strPRTREQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRPRTREQNO").ToString();

                for (i = 0; i < strTree.Count; i++)
                {
                    string[] arryPara = strTree[i].Split(sKeyHead.ToCharArray());
                    string[] arryEmrNo = arryPara[5].Split('|');
                    string strPRTOPTION = "";

                    if (arryEmrNo[0].Equals("S"))
                    {
                        if(arryPara[3].Equals("963") && pCopy != null && (VB.Val(pCopy.medEndDate) == 0 || VB.Val(pCopy.medEndDate) > 0 && VB.Val(pCopy.medEndDate) > 20091231) || arryPara[3].Equals("963") &&  pCopy == null)
                        {
                            continue;
                        }
                        CopyInsert(arryEmrNo[1], arryEmrNo[2], strCopyCnt);
                        continue;
                    }

                    if (arryPara[2] != "ER" && arryPara[3].Equals("2090"))
                    {
                        arryPara[2] = "ER";
                    }

                    if (chkDate.Checked == true && (arryPara[3].Equals("1680") || arryPara[3].Equals("2090")))
                    {
                        strPRTOPTION = arryEmrNo[0] + "^" + arryEmrNo[1] + "^" + arryPara[2] + "^" + clsEmrQuery.ReadDeptDoctor(clsDB.DbCon, arryEmrNo[1], arryPara[2], mPTNO) + "^" + arryEmrNo[1];
                    }
                    else
                    {
                        if (pCopy == null)
                        {
                            if(arryPara[3].Equals("1680") || arryPara[3].Equals("2090"))
                            {
                                ComFunc.MsgBoxEx(this, "DR오더지는 내원내역을 선택하시고 전송해주세요.");
                                break;
                            }
                            strPRTOPTION = "^^" + arryPara[2] + "^^" + arryEmrNo[1];
                        }
                        else
                        {
                            strPRTOPTION = pCopy.inOutCls + "^" + pCopy.medFrDate + "^" + arryPara[2] + "^" + pCopy.medDrCd + "^" + arryEmrNo[1];
                        }
                    }

                    if (arryPara[3] == "1680" && VB.Val(arryEmrNo[1]) < 20090901)
                    {
                        ComFunc.MsgBoxEx(this, "2009년 9월 이전 Dr Order는 영상챠트를 복사신청하셔야 합니다.");
                    }
                    else
                    {
                        if (clsEmrQuery.READ_DUPLICATE(clsDB.DbCon, mPTNO, arryEmrNo[1], strREQDATE, arryEmrNo[0], strPRTOPTION) == false || ChkDuplicate.Checked == false)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "   INSERT INTO " + ComNum.DB_EMR + "EMRPRTREQ (";
                            SQL = SQL + ComNum.VBLF + "        PRTREQNO,REQDATE,PTNO,";
                            SQL = SQL + ComNum.VBLF + "        EMRNO, SCANYN, FORMCODE, USEID,INPDATE,INPTIME,PRTOPTION, REQCNT, EMRNOHIS)";
                            SQL = SQL + ComNum.VBLF + "        VALUES (";
                            SQL = SQL + ComNum.VBLF + "            " + strPRTREQNO + ",";
                            SQL = SQL + ComNum.VBLF + "            '" + strREQDATE + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + mPTNO + "',";
                            SQL = SQL + ComNum.VBLF + "            " + arryEmrNo[1] + ",";
                            SQL = SQL + ComNum.VBLF + "            '" + arryEmrNo[0] + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + arryEmrNo[2] + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + VB.Val(clsType.User.IdNumber) + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + VB.Left(strCurDateTime, 8) + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + VB.Right(strCurDateTime, 6) + "',";
                            SQL = SQL + ComNum.VBLF + "            '" + strPRTOPTION + "'," + VB.Val(strCopyCnt) + ", " + arryEmrNo[3] + ")";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        List<String> CheckedNames(TreeNodeCollection theNodes)
        {
            List<String> aResult = new List<String>();

            if (theNodes != null)
            {
                foreach (TreeNode aNode in theNodes)
                {
                    if (aNode.Checked)
                    {
                        if (aNode.GetNodeCount(false) == 0)
                        {
                            ///전체 항목 
                            if (aNode.Level == 2)
                            {
                                string[] arryPara = aNode.Name.Split(sKeyHead.ToCharArray());
                                aResult.Add(string.Concat(
                                    sKeyHead, arryPara[1], sKeyHead,
                                    aNode.Tag.ToString(), sKeyHead, 
                                    arryPara[2], sKeyHead,
                                    arryPara[3], sKeyHead, arryPara[4]));
                            }
                            else
                            {
                                aResult.Add(aNode.Name);
                            }
                            //aNode.Checked = false;
                        }
                    }

                    aResult.AddRange(CheckedNames(aNode.Nodes));
                }
            }

            return aResult;
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            ComFunc.MsgBoxEx(this, "당일 복사 신청한 내용 중 중복되는 내용인 복사신청을 하지 않습니다."
                            + ComNum.VBLF + "대량으로 복사 신청할 경우 신청시간이 오래 걸릴 수도 있습니다.");
        }


        /// <summary>
        /// 복사신청 함수
        /// 당일날 이미 신청했으면 제외 하고 신청
        /// </summary>
        /// <returns></returns>
        bool CopyInsert(string strTreatNo, string FormCode, string PrtCnt)
        {
            bool rtnVal = false;
            StringBuilder SQL = new StringBuilder();
            int RowAffected = 0;

            try
            {
                SQL.Clear();
                SQL.AppendLine("INSERT INTO ADMIN.EMR_PRINTNEEDT(");
                SQL.AppendLine("TREATNO, PAGENO, CUSERID, PRINTCODE,");
                SQL.AppendLine("CDATE, NEEDGUBUN, NEEDCNT");
                SQL.AppendLine(")");
                SQL.AppendLine("SELECT");
                SQL.AppendLine("   C.TREATNO, C.PAGENO, ");
                SQL.AppendLine("'" + clsType.User.IdNumber + "',"); //로그인 사번
                SQL.AppendLine("'002',"); //전송용
                SQL.AppendLine("TO_CHAR(SYSDATE, 'YYYYMMDD'), ");//서버날짜
                SQL.AppendLine("'1', ");//NEEDGUBUN
                SQL.AppendLine(VB.Val(PrtCnt).ToString());//요청 갯수
                SQL.AppendLine("FROM ADMIN.EMR_PAGET P  ");
                SQL.AppendLine("  INNER JOIN ADMIN.EMR_CHARTPAGET C ");
                SQL.AppendLine("     ON P.PAGENO = C.PAGENO");
                SQL.AppendLine("     AND C.TREATNO = " + strTreatNo);
                SQL.AppendLine("     AND C.PAGE > 0");
                SQL.AppendLine("  INNER JOIN ADMIN.EMR_FORMT F ");
                SQL.AppendLine("     ON C.FORMCODE = F.FORMCODE ");
                SQL.AppendLine("     AND F.FORMCODE = '" + FormCode + "'");
                SQL.AppendLine("WHERE NOT EXISTS");
                SQL.AppendLine("(");
                SQL.AppendLine("SELECT 1");
                SQL.AppendLine("FROM ADMIN.EMR_PRINTNEEDT");
                SQL.AppendLine("WHERE CDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')");
                SQL.AppendLine("  AND PAGENO  = C.PAGENO");
                SQL.AppendLine("  AND TREATNO = C.TREATNO");
                SQL.AppendLine("  AND CUSERID = '" + clsType.User.IdNumber + "'");
                SQL.AppendLine(")");

                string sqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnChartCopy2_Click(object sender, EventArgs e)
        {
            if (SaveDataPrintReq2() == true)
            {
                //ClearCopyReq();
                ComFunc.MsgBoxEx(this, "복사신청을 완료했습니다.");
            }
        }

        private bool SaveDataPrintReq2()
        {
            //string strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수", "1");

            //if (VB.Val(strCopyCnt) == 0)
            //{
            //    ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
            //    strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수", "1");
            //    if (VB.Val(strCopyCnt) == 0)
            //    {
            //        ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
            //        return false;
            //    }
            //}

            string strCopyCnt = "1";

            int ChkCnt = 0;
            int i;
            for (i = 0; i < ssViewEmrAcpFormChartList_Sheet1.RowCount; i++)
            {
                if (ssViewEmrAcpFormChartList_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    ChkCnt += 1;
                }
            }

            if (ChkCnt == 0)
            {
                ComFunc.MsgBoxEx(this, "차트를 선택해주세요.");
                return false;
            }

            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strREQDATE = VB.Left(strCurDateTime, 8);
                string strPRTREQNO = (ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRPRTREQNO")).ToString();

                for (i = 0; i < ssViewEmrAcpFormChartList_Sheet1.RowCount; i++)
                {
                    if (ssViewEmrAcpFormChartList_Sheet1.Cells[i, 0].Text.Trim() != "True")
                    {
                        continue;
                    }

                    string strInOut = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 1].Text.Trim();
                    string strFrDate = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 2].Text.Trim();
                    string strEndDate = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 3].Text.Trim();
                    string strDeptCd = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 4].Text.Trim();
                    string strFormNo = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 6].Text.Trim();
                    string strScanYn = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 9].Text.Trim();
                    string strEmrNo = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 10].Text.Trim();
                    string strFormCode = ssViewEmrAcpFormChartList_Sheet1.Cells[i, 11].Text.Trim();

                    string strPRTOPTION = string.Empty;

                    if (strScanYn.Equals("S"))
                    {
                        CopyInsert(strEmrNo, strFormCode, strCopyCnt);
                        continue;
                    }

                    if (strDeptCd != "ER" && strFormNo == "2090")
                    {
                        strDeptCd = "ER";
                    }

                    if (strInOut == "O" && (strFormNo == "1680" || strFormNo == "2090"))
                    {
                        strPRTOPTION = strInOut + "^" + strFrDate + "^" + strDeptCd + "^" + clsEmrQuery.ReadDeptDoctor(clsDB.DbCon, strFrDate, strDeptCd, mPTNO) + "^" + strEmrNo;
                    }
                    else
                    {
                        if (pView == null)
                        {
                            ComFunc.MsgBoxEx(this, "접수 정보를 찾을 수 없습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                        strPRTOPTION = pView.inOutCls + "^" + pView.medFrDate + "^" + strDeptCd + "^" + pView.medDrCd + "^" + strEmrNo;
                    }

                    if (strFormNo == "1680" && VB.Val(strEmrNo) < 20090901)
                    {
                        ComFunc.MsgBoxEx(this, "2009년 9월 이전 Dr Order는 영상챠트를 복사신청하셔야 합니다.");
                    }
                    else
                    {
                        if (clsEmrQuery.READ_DUPLICATE(clsDB.DbCon, mPTNO, strEmrNo, strREQDATE, strScanYn, strPRTOPTION) == false || ChkDuplicate.Checked == false)
                        {
                            SQL.Clear();
                            SQL.AppendLine( "   INSERT INTO " + ComNum.DB_EMR + "EMRPRTREQ (");
                            SQL.AppendLine( "        PRTREQNO,REQDATE,PTNO,");
                            SQL.AppendLine( "        EMRNO, SCANYN, FORMCODE, USEID,INPDATE,INPTIME,PRTOPTION, REQCNT)");
                            SQL.AppendLine( "        VALUES (");
                            SQL.AppendLine( "            " + strPRTREQNO + ",");
                            SQL.AppendLine( "            '" + strREQDATE + "',");
                            SQL.AppendLine( "            '" + mPTNO + "',");
                            SQL.AppendLine( "            " + strEmrNo + ",");
                            SQL.AppendLine( "            '" + strScanYn + "',");
                            SQL.AppendLine( "            '" + strFormCode + "',");
                            SQL.AppendLine( "            '" + VB.Val(clsType.User.IdNumber) + "',");
                            SQL.AppendLine( "            '" + VB.Left(strCurDateTime, 8) + "',");
                            SQL.AppendLine( "            '" + VB.Right(strCurDateTime, 6) + "',");
                            SQL.AppendLine( "            '" + strPRTOPTION + "'," + VB.Val(strCopyCnt) + ")");

                            SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }

        private void OptEmrInOutCopyA_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (sender.Equals(optEmrInOutCopyO) == false)
                {
                    chkDate.Checked = false;
                }

                if (chkDate.Checked == false)
                {
                    dtpDateCopyS.Enabled = false;
                    dtpDateCopyE.Enabled = false;
                }
                
                string strMedDeptCd = cboDept.SelectedIndex > 0 ? VB.Right(cboDept.Text.Trim(), 3).Trim() : string.Empty;

                GetHisSheet(strMedDeptCd);
                //C(ssViewEmrAcpCopy);
            }
        }

        private void OptEmrInOutDeptA_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (optEmrInOutDeptO.Checked == true)
                {
                    ssViewEmrAcpDept_Sheet1.Columns[2].Visible = false;
                }

                if (optEmrInOutDeptA.Checked == true || optEmrInOutDeptI.Checked == true)
                {
                    ssViewEmrAcpDept_Sheet1.Columns[2].Visible = true;
                }

                GetHisDept();
            }
        }

        private void FrmEmrBaseAcpList_FormClosing(object sender, FormClosingEventArgs e)
        {
            boldFont.Dispose();
            RegularFont.Dispose();
        }

        private void ssViewEmrAcpFormChartList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpForm_Sheet1.RowCount == 0) return;

            ssViewEmrAcpFormChartListCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpDeptChartList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpDeptChartList_Sheet1.RowCount == 0) return;

            ssViewEmrAcpDeptChartListCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssViewEmrAcpDeptCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpCopy_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssViewEmrAcpCopyCellDoubleClick(e.Row, e.Column);
        }
    }
}
