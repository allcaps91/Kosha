using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBaseAcpListNew : Form
    {

        #region //폼에서 사용하는 변수

        //EmrPatient AcpEmr = null; //외부에서 전달받은 환자 정보
        EmrPatient pView = null; //뷰어용 환자정보
        EmrPatient pCopy = null; //복사 신청용 환자정보
        EmrForm fView = null;

        string mPTNO = "";

        bool mViewNpChart = false;

        string sKeyHead = "^";
        //private int nImage = 0;
        //private int nSelectedImage = 1;
        //private int nImageSaved = 2;
        //private int nSelectedImageSaved = 3;

        #endregion //폼에서 사용하는 변수

        #region //임시변수
        string GstrView01 = "";
        string gJinGubun = "";
        string gJinState = "";
        //bool gDateSET = false;
        private bool FindScanImageYn(string strInOutCls, string strMedFrDate, string strMedDeptCd, string strMedMedDrCd, string strMedEndDate)
        {
            return false;
        }
        #endregion //임시변수

        #region //이벤트 전달
        ////폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //기록지 관련 : 작성된 기록지 호출
        public delegate void ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode);
        public event ViewChart rViewChart;

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

            optEmrSearchGubun2.Checked = true;

        }

        public void GetJupHis(string pPTNO)
        {
            mPTNO = pPTNO;
            GetHisDept();
            GetHisChart();
        }

        #endregion


        public frmEmrBaseAcpListNew()
        {
            InitializeComponent();
        }

        public frmEmrBaseAcpListNew(string pPTNO)
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

            trvEmrView.ImageList = this.ImageList2;

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
                cboDept.Items.Add(dt.Rows[i]["DEPTKORNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
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
                panViewEmrAcpForm.Visible = true;
                panViewEmrAcpForm.BringToFront();
            }
        }

        private void optEmrSearchGubun3_CheckedChanged(object sender, EventArgs e)
        {
            if (optEmrSearchGubun3.Checked == true)
            {
                panViewEmrAcpCopy.Visible = true;
                panViewEmrAcpCopy.BringToFront();

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
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssViewEmrAcpDept_Sheet1.RowCount = 0;
            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            if (mPTNO.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);


            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE,";
            SQL = SQL + ComNum.VBLF + "  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME, XX.DRNAME, XX.GBSPC, XX.GBSTS,  ";
            SQL = SQL + ComNum.VBLF + "  (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME ";
            SQL = SQL + ComNum.VBLF + "FROM (";

            if (optEmrInOutDeptO.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                if (chkEmrSearchGubunA.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT 'O' AS INOUTCLS, A.Pano AS PTNO,A.SName AS PTNAME, A.Sex, A.Age, ";
                    SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    NVL(TO_CHAR(A.BDATE,'YYYYMMDD'),TO_CHAR(A.ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC, '0' GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + mPTNO + "' ";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.GBUSE = 'Y' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = B.DRCODE(+) ";
                }
                else
                {
                    //=================================================================
                    //2011-06-15 HD외래의 경우 한달 이내의 내역만 조회 요청(의뢰서)
                    //=================================================================
                    SQL = SQL + ComNum.VBLF + " SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ";
                    SQL = SQL + ComNum.VBLF + "    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE IN ('HD','RM') ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND GBUSE = 'Y'";
                    }
                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = B.DRCODE(+) ";

                    SQL = SQL + ComNum.VBLF + " UNION ALL         ";
                    SQL = SQL + ComNum.VBLF + " SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ";
                    SQL = SQL + ComNum.VBLF + "    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE NOT IN ('HD','RM') ";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    if (gJinGubun == "" || gJinGubun == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                    }
                    if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND GBUSE = 'Y' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND A.DRCODE =B.DRCODE(+)";
                }
            }

            if (optEmrInOutDeptA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "UNION ALL";
            }

            if (optEmrInOutDeptI.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age, ";
                SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A , " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "AND A.GBSTS <> '9'";
                SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = B.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                if (GstrView01 == "1")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , B.GBSPC, B.GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXMLMST A, ";
                    SQL = SQL + ComNum.VBLF + "    " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_PMPA + "BAS_DOCTOR C ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO =  '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "AND A.INOUTCLS = 'I' ";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "AND B.GBSTS = '9' ";
                    SQL = SQL + ComNum.VBLF + "AND A.PTNO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDDEPTCD = B.DeptCode ";

                }
                else if (GstrView01 == "" || GstrView01 == "0")
                {
                    SQL = SQL + ComNum.VBLF + " SELECT INOUTCLS, PTNO, PTNAME, SEX, AGE, MEDDEPTCD, MEDDRCD, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, DRNAME, GBSPC, GBSTS";
                    SQL = SQL + ComNum.VBLF + "  FROM (";
                    SQL = SQL + ComNum.VBLF + "SELECT 'I' AS INOUTCLS, PANO AS PTNO, SName AS PTNAME, SEX, AGE,";
                    SQL = SQL + ComNum.VBLF + "    DEPTCODE AS MEDDEPTCD, A.DRCODE AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(INDATE,'YYYYMMDD') AS MEDFRDATE, '1200' AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(OUTDATE,'YYYYMMDD') AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO =  '" + mPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "AND GBSTS = '9'";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "     AND INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND A.DRCODE = B.DRCODE(+) ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY 'I', PANO, SNAME, SEX, AGE, DEPTCODE, A.DRCODE, TO_CHAR(INDATE,'YYYYMMDD'), '1200', TO_CHAR(OUTDATE,'YYYYMMDD'), DRNAME, GBSPC, GBSTS )";
                }
            }
            SQL = SQL + ComNum.VBLF + "UNION ALL ";

            SQL = SQL + ComNum.VBLF + " SELECT A.CLASS AS INOUTCLS, A.PATID AS PTNO,  B.NAME AS PTNAME, B.Sex, 0 AS Age,  ";
            SQL = SQL + ComNum.VBLF + "    A.CLINCODE AS MEDDEPTCD, C.DRCODE AS MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "    A.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "    A.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, C.DRNAME , '' GBSPC, '0' GBSTS   ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT A, KOSMOS_EMR.EMR_PATIENTT B, " + ComNum.DB_MED + "OCS_DOCTOR C ";
            SQL = SQL + ComNum.VBLF + "WHERE A.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.DOCCODE = C.DOCCODE(+) ";
            SQL = SQL + ComNum.VBLF + "AND A.DELDATE IS NULL";
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "     AND A.INDATE >= '" + dtpDateDeptS.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.INDATE <= '" + dtpDateDeptE.Value.ToString("yyyyMMdd") + "' ";
            }
            SQL = SQL + ComNum.VBLF + "AND A.PATID = B.PATID ";
            if (chkEmrSearchGubunA.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "AND ((A.CLINCODE IN ('HD','RM') AND A.INDATE >= '" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "') OR (A.CLINCODE NOT IN ('HD','RM') AND A.INDATE >= '19000101'))";
            }
            if (optEmrInOutDeptO.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'O'";
            }
            else if (optEmrInOutDeptI.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'I'";
            }

            SQL = SQL + ComNum.VBLF + "AND (A.CLASS, A.INDATE, A.CLINCODE) NOT IN ( ";
            SQL = SQL + ComNum.VBLF + "            SELECT INOUTCLS, MEDFRDATE, MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM ";
            if (chkEmrSearchGubunA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "            (SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "            WHERE PANO = '" + mPTNO + "' ";
                if (chkGikan.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','8','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }

                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "                AND GBUSE = 'Y' ";
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "            (SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "            WHERE PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE IN ('HD','RM') ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "                AND GBUSE = 'Y' ";
                }
                SQL = SQL + ComNum.VBLF + "        UNION ALL    ";
                SQL = SQL + ComNum.VBLF + "            SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "            WHERE PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE NOT IN ('HD','RM') ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ";
                if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "                AND GBUSE = 'Y' ";
                }
            }

            SQL = SQL + ComNum.VBLF + "            UNION ALL ";
            SQL = SQL + ComNum.VBLF + "             SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',A2.DeptCode) AS MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2  ";
            SQL = SQL + ComNum.VBLF + "            WHERE A2.PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "            AND A2.GBSTS <> '9'";
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
            }
            SQL = SQL + ComNum.VBLF + "           UNION ALL";
            SQL = SQL + ComNum.VBLF + "            SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',B2.FRDEPT) AS MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "              FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2,   " + ComNum.DB_PMPA + "IPD_TRANSFOR B2";
            SQL = SQL + ComNum.VBLF + "              Where A2.PANO = B2.PANO ";
            SQL = SQL + ComNum.VBLF + "                AND A2.PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "                AND A2.IPDNO = B2.IPDNO";
            if (chkGikan.Checked == true && chkEmrSearchGubunA.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
            }
            SQL = SQL + ComNum.VBLF + "             AND A2.GBSTS <> '9')";
            SQL = SQL + ComNum.VBLF + "    )  ";
            SQL = SQL + ComNum.VBLF + ") XX";
            SQL = SQL + ComNum.VBLF + "  WHERE XX.INOUTCLS IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "    ORDER BY XX.INOUTCLS ASC,  XX.MEDFRDATE DESC, XX.MEDDEPTCD";

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
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "입원취소";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = strMedEndDate;
                    }
                }
                else
                {
                    if (clsEmrQueryPohangS.READ_DOCREPRINT(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) == true)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "서류재발급";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
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

                if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                }
                else if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                {
                    if (clsVbfunc.READ_SPECIAL_SERVICE(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE, dt.Rows[i]["INOUTCLS"].ToString().Trim()) == true)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                    }
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                }

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

            if (optEmrInOutDeptO.Checked == true)
            {
                ssViewEmrAcpDept_Sheet1.Columns[2].Visible = false;
            }
            if (optEmrInOutDeptI.Checked == true)
            {
                ssViewEmrAcpDept_Sheet1.Columns[2].Visible = true;
            }

            Cursor.Current = Cursors.Default;

        }

        private void ssViewEmrAcpDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpDept_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpDept, e.Column);
                return;
            }

            ssViewEmrAcpDeptCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpDeptCellDoubleClick(int Row, int Column)
        {
            ssViewEmrAcpDept_Sheet1.Cells[0, 0, ssViewEmrAcpDept_Sheet1.RowCount - 1, ssViewEmrAcpDept_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpDept_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpDept_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strInOutCls = "";
            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDeptCd = "";
            string strMedFrTime = "";
            string strMedEndTime = "";
            string strMedMedDrCd = "";

            strInOutCls = ssViewEmrAcpDept_Sheet1.Cells[Row, 0].Text.Trim();
            strMedFrDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            strMedDeptCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 3].Text.Trim();
            strMedEndDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");

            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            else if (strMedEndDate == "서류재발급")
            {
                strMedEndDate = "";
                clsEmrQueryPohangS.READ_DOCREPRINTHIS(clsDB.DbCon, this, mPTNO, strMedFrDate, strMedDeptCd);
            }

            strMedFrTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 5].Text.Trim();
            strMedEndTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 6].Text.Trim();
            strMedMedDrCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 7].Text.Trim();

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "NP")
                {
                    if (mViewNpChart == false)
                    {
                        ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                        return;
                    }
                }
            }

            //EMR 내원 내역을 담는다
            //With gptEmrPt
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

            //int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (clsEmrPublic.gUserGrade == "SIMSA")
            {
                if (strInOutCls == "O")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + strMedDeptCd + "' ";
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
                        //frmTextEmrMain.lblPtNm.Caption = Trim(AdoGetString(RS4, "SNAME", 0)) + "(" & READ_Bi_Name(Trim(AdoGetString(RS4, "BI", 0))) & ")"
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (strInOutCls == "I")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + strMedFrDate + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + strMedFrDate + " 23:59','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strMedDeptCd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9')";
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
                        //frmTextEmrMain.lblPtNm.Caption = Trim(AdoGetString(RS4, "SNAME", 0)) + "(" & READ_Bi_Name(Trim(AdoGetString(RS4, "BI", 0))) & ")"
                    }
                    dt.Dispose();
                    dt = null;
                }
            }

            if (strInOutCls == "I" && clsEmrQueryPohangS.READ_ER_IPWON(clsDB.DbCon, mPTNO, strMedFrDate) == true)
            {
                GetHisForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd, "#");
            }
            else
            {
                GetHisForm(strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, strMedMedDrCd);
            }

            if (strInOutCls == "O")
            {
                if (ssViewEmrAcpDeptChartList_Sheet1.RowCount > 0)
                {
                    ssViewEmrAcpDeptChartListCellDoubleClick(0, 0);
                }
            }
        }

        private void GetHisForm(string strInOutCls, string strMedFrDate, string strMedEndDate, string strMedDeptCd, string strMedMedDrCd, string strREP = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            ssViewEmrAcpDeptChartList_Sheet1.RowCount = 0;

            SQL = " SELECT INOUTCLS, GRPFORMNO, FORMNO, FORMNAME, SCANYN, SUM(PCNT) AS PCNT, SUM(CNT) AS CNT, FORMCODE, TREATNO, DISPSEQ, RANKING, COLOR , MAX(EMRNO) AS EMRNO";
            SQL = SQL + ComNum.VBLF + "FROM (";
            //=========================== 스캔내역 불러오기 ==================================
            SQL = SQL + ComNum.VBLF + " SELECT TO_NUMBER(T.TREATNO) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, 0 AS ACPNO,";
            SQL = SQL + ComNum.VBLF + "        T.PATID AS PTNO, T.CLASS AS INOUTCLS, T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME,";
            SQL = SQL + ComNum.VBLF + "        T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,";
            SQL = SQL + ComNum.VBLF + "        F.FORMNAME1 FORMNAME,  F.GRPFORMNO, G.GRPFORMNAME AS GRPFORMNAME, C.NAME AS DEPTKORNAME,";
            SQL = SQL + ComNum.VBLF + "        T.TREATNO, P.FORMCODE,  'S' AS SCANYN, G.DISPSEQ, 1 PCNT, 0 CNT, S.RANKING, S.COLOR";
            SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_EMR.EMR_TREATT T,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMRFORM F,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMRMAPPING M,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMRGRPFORM G,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMR_CLINICT C,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMR_CHARTPAGET P,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_EMR.EMRFORM_SET S";
            SQL = SQL + ComNum.VBLF + "     WHERE T.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "       AND T.INDATE = '" + strMedFrDate + "'";
            SQL = SQL + ComNum.VBLF + "       AND T.CLASS = '" + strInOutCls + "'";
            //if (gDateSET == true)
            //{
            //    SQL = SQL + ComNum.VBLF + "       AND T.INDATE >= '" + Replace(clsEmrQueryPohangS.ReadOptionDate(gUserGrade, gptEmrPt.PtInOutCls, gptEmrPt.PtMedDeptCd, gptEmrUs.UsUseId, strMedFrDate), "-", "") + "'";
            //}
            SQL = SQL + ComNum.VBLF + "       AND T.TREATNO = P.TREATNO";
            SQL = SQL + ComNum.VBLF + "       AND P.FORMCODE = M.FORMCODE";
            SQL = SQL + ComNum.VBLF + "       AND T.CLINCODE = C.CLINCODE";
            SQL = SQL + ComNum.VBLF + "       AND F.GRPFORMNO = G.GRPFORMNO";
            SQL = SQL + ComNum.VBLF + "       AND F.FORMNO = M.FORMNO";
            SQL = SQL + ComNum.VBLF + "       AND C.CONSYN = 'Y'";

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125"))
                {
                    SQL = SQL + ComNum.VBLF + "                AND T.CLINCODE = 'RA'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                AND T.CLINCODE = '" + strMedDeptCd + "'";
                }
            }
            if (mViewNpChart == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND T.CLINCODE <> 'NP'";
            }
            SQL = SQL + ComNum.VBLF + "           AND F.FORMNO = S.FORMNO(+)";
            SQL = SQL + ComNum.VBLF + "           AND S.SABUN(+) = " + clsType.User.IdNumber;  //FstrPassId

            SQL = SQL + ComNum.VBLF + "    UNION ALL ";

            //=========================== TEXT내역 불러오기 ==================================
            SQL = SQL + ComNum.VBLF + "SELECT  /*+index(A INDEX_EMRXMLMST7)*/ A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,";
            SQL = SQL + ComNum.VBLF + "        0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,";
            SQL = SQL + ComNum.VBLF + "        A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,";
            SQL = SQL + ComNum.VBLF + "        A.MEDDRCD,  B.FORMNAME1 FORMNAME,  B.GRPFORMNO, C.GRPFORMNAME,";
            SQL = SQL + ComNum.VBLF + "          D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, C.DISPSEQ, 0 PCNT, 1 CNT, S.RANKING, S.COLOR";
            SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_EMR.EMRXMLMST A, ";
            SQL = SQL + ComNum.VBLF + " KOSMOS_EMR.EMRFORM B, ";
            SQL = SQL + ComNum.VBLF + " KOSMOS_EMR.EMRGRPFORM C, ";
            SQL = SQL + ComNum.VBLF + " KOSMOS_EMR.EMR_CLINICT D, ";
            SQL = SQL + ComNum.VBLF + " KOSMOS_EMR.EMRFORM_SET S";
            SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + mPTNO + "'";
            SQL = SQL + ComNum.VBLF + "          AND A.MEDFRDATE = '" + strMedFrDate + "'";
            if (strMedDeptCd != "HD")
            {
                if (strREP == "#")
                {
                    SQL = SQL + ComNum.VBLF + "                                        AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                        AND A.INOUTCLS = '" + strInOutCls + "'";
                }
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                {
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = 'MD'";
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD IN ('1107','1125')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD NOT IN ('1107','1125')";
                }
            }
            if (mViewNpChart == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
            }
            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO NOT IN ('1796')";
            }
            //if (gDateSET == true)
            //{
            //    SQL = SQL + ComNum.VBLF + "            AND A.CHARTDATE >= '" + Replace(clsEmrQueryPohangS.ReadOptionDate(gUserGrade, gptEmrPt.PtInOutCls, gptEmrPt.PtMedDeptCd, gptEmrUs.UsUseId, strMedFrDate), "-", "") + "'";
            //}
            SQL = SQL + ComNum.VBLF + "            AND A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "            AND B.GRPFORMNO = C.GRPFORMNO(+)";
            SQL = SQL + ComNum.VBLF + "            AND A.MEDDEPTCD = D.CLINCODE";
            SQL = SQL + ComNum.VBLF + "           AND B.FORMNO = S.FORMNO(+)";
            SQL = SQL + ComNum.VBLF + "           AND S.SABUN(+) = " + clsType.User.IdNumber; // FstrPassId;

            SQL = SQL + ComNum.VBLF + "    UNION ALL ";

            //===========================투약기록지 내역 불러오기 ==================================
            SQL = SQL + ComNum.VBLF + " SELECT  A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,";
            SQL = SQL + ComNum.VBLF + "         0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE,";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRTIME,  A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD,";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,  '투약기록지' FORMNAME , 3 GRPFORMNO, '간호기록' GRPFORMNAME,";
            SQL = SQL + ComNum.VBLF + "           D.NAME AS DEPTKORNAME,   0 AS TREATNO, '000' AS FORMCODE,  'T' AS SCANYN, 0 DISPSEQ, 0 PCNT, 1 CNT, '' RANKING, '' COLOR";
            SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_EMR.EMRXML_TUYAK A,";
            SQL = SQL + ComNum.VBLF + "          KOSMOS_EMR.EMR_CLINICT d";
            SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + mPTNO + "'";
            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "         AND 1 = 2";
            }
            SQL = SQL + ComNum.VBLF + "          AND A.MEDFRDATE = '" + strMedFrDate + "'";
            if (strMedDeptCd != "HD")
            {
                if (strREP == "#")
                {
                    SQL = SQL + ComNum.VBLF + "                                        AND (A.INOUTCLS = 'I' OR (A.MEDDEPTCD = 'ER' AND A.INOUTCLS = 'O'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                        AND A.INOUTCLS = '" + strInOutCls + "'";
                }
            }

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                {
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = 'MD'";
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD IN ('1107','1125')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
                    SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD NOT IN ('1107','1125')";
                }
            }
            if (mViewNpChart == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
            }

            //if (gDateSET == true)
            //{
            //    SQL = SQL + ComNum.VBLF + "            AND A.CHARTDATE >= '" + Replace(clsEmrQueryPohangS.ReadOptionDate(gUserGrade, gptEmrPt.PtInOutCls, gptEmrPt.PtMedDeptCd, gptEmrUs.UsUseId, strMedFrDate), "-", "") + "'";
            //}
            SQL = SQL + ComNum.VBLF + "            AND A.MEDDEPTCD = D.CLINCODE(+)";

            SQL = SQL + ComNum.VBLF + "     ) ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY INOUTCLS, GRPFORMNO, FORMNO, FORMNAME, SCANYN, FORMCODE, TREATNO, DISPSEQ, RANKING, COLOR";
            SQL = SQL + ComNum.VBLF + "  ORDER BY RANKING ASC, INOUTCLS DESC, DISPSEQ, FORMNO";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            int nRow = 0;

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;

                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO

                    if (clsEmrPublic.gUserGrade == "SIMSA")
                    {
                        switch (dt.Rows[i]["COLOR"].ToString().Trim())
                        {
                            case "1":
                                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
                                break;
                            default:
                                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                                break;
                        }
                    }
                    else
                    {
                        switch (dt.Rows[i]["GRPFORMNO"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "2":
                            case "27":
                                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
                                break;
                            default:
                                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                                break;
                        }
                    }

                    if (clsEmrPublic.gUserGrade == "WRITE" || clsType.User.IdNumber == "24432")
                    {
                        if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1647")
                        {
                            if (clsEmrQueryPohangS.CHECK_COMPLETE(clsDB.DbCon, mPTNO, strMedFrDate))
                            {
                                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(250)))));
                            }
                        }
                    }

                    //코드화 작업
                    if (clsEmrQueryPohangS.READ_FORM_BOLD(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim()) == true)
                    {
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                    }

                    if (dt.Rows[i]["FORMNO"].ToString().Trim() == "1965" && clsEmrPublic.gUserGrade == "SIMSA")
                    {
                        nRow = nRow + 1;
                        ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "수혈기록지2";
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "9999";
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
                        ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EMRNO"].ToString().Trim();  //TREATNO
                    }
                }
            }
            dt.Dispose();
            dt = null;

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            SQL = "  SELECT 'I' INOUTCLS, A.MEDDEPTCD, 1761 AS FORMNO, ";
            SQL = SQL + ComNum.VBLF + "        'ICU기록지' AS FORMNAME, 'T' AS SCANYN , 1 AS CNT, ";
            SQL = SQL + ComNum.VBLF + "        '000' AS FORMCODE, 0 AS TREATNO, 0 AS PCNT";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXMLMST A";
            SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "          AND A.INOUTCLS = '" + strInOutCls + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE >= '" + strMedFrDate + "' ";
            if (strMedEndDate == "")
            {
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strCurDate + "' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strMedEndDate + "' ";
            }
            SQL = SQL + ComNum.VBLF + "          AND A.FORMNO IN (1790,1791,1795,1807)";

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
            }
            dt.Dispose();
            dt = null;

            if (clsEmrPublic.gUserGrade == "SIMSA" && strInOutCls == "I")
            {
                nRow = nRow + 1;
                ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
            }
            else
            {
                if (clsEmrQueryPohangS.IsDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
                {
                    nRow = nRow + 1;
                    ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                    ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
                }
            }

            if (clsEmrQueryPohangS.IsDrOrder(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedEndDate, strMedDeptCd, clsEmrPublic.gUserGrade) == true)
            {
                nRow = nRow + 1;
                ssViewEmrAcpDeptChartList_Sheet1.RowCount = nRow;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 0].Text = strInOutCls;
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 1].Text = "Dr Order";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 2].Text = "1";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 3].Text = "0";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 4].Text = "1680";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 5].Text = "O";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 6].Text = "000";
                ssViewEmrAcpDeptChartList_Sheet1.Cells[nRow - 1, 7].Text = "0";
            }

            ssViewEmrAcpDeptChartList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            Cursor.Current = Cursors.Default;
        }

        private void ssViewEmrAcpDeptChartList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpDeptChartList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpDeptChartList, e.Column);
                return;
            }

            ssViewEmrAcpDeptChartListCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpDeptChartListCellDoubleClick(int Row, int Column)
        {
            ssViewEmrAcpDeptChartList_Sheet1.Cells[0, 0, ssViewEmrAcpDeptChartList_Sheet1.RowCount - 1, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpDeptChartList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strFormNo = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 4].Text.Trim();
            string strSCANYN = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 5].Text.Trim();
            string strFormCode = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 6].Text.Trim();
            string strEmrNo = ssViewEmrAcpDeptChartList_Sheet1.Cells[Row, 7].Text.Trim();
            //string strUpdateNo = "0";
            string strTreatNo = "0";

            pView = clsEmrChart.ClearPatient();
            pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
            if (pView == null)
            {
                ComFunc.MsgBoxEx(this, "서식지 작성 내역을 찾을 수 없습니다.");
                return;
            }
            pView.formNo = (long)VB.Val(strFormNo);
            pView.updateNo = 0;

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode);

        }

        #endregion panViewEmrAcpDept

        #region panViewEmrAcpForm

        private void btnSearchEmrForm_Click(object sender, EventArgs e)
        {
            GetHisChart();
        }

        private void GetHisChart()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssViewEmrAcpForm_Sheet1.RowCount = 0;
            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            if (mPTNO.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            SQL = " SELECT ET.GRPFORMNO, MAX(ET.GRPFORMNAME) AS GRPFORMNAME , ";
            SQL = SQL + ComNum.VBLF + "    ET.FORMNO,  MAX(ET.FORMNAME) AS FORMNAME, ET.FORMCODE, MAX(ET.SCANYN) AS SCANYN , COUNT(ET.EMRNO) AS CNT, ";
            SQL = SQL + ComNum.VBLF + "    MAX(ET.PCNT) AS PCNT";
            SQL = SQL + ComNum.VBLF + "FROM  ";
            SQL = SQL + ComNum.VBLF + "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME,  ";
            SQL = SQL + ComNum.VBLF + "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME,  ";
            SQL = SQL + ComNum.VBLF + "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,   ";
            SQL = SQL + ComNum.VBLF + "            F.FORMNAME1 FORMNAME,  F.GRPFORMNO,  ";
            SQL = SQL + ComNum.VBLF + "            (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "            (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME,  ";
            SQL = SQL + ComNum.VBLF + "            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN  ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_TREATT T,  ";
            SQL = SQL + ComNum.VBLF + "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT,  ";
            SQL = SQL + ComNum.VBLF + "                TO_CHAR(P.TREATNO) AS EMRNO1  ";
            SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_EMR.EMR_CHARTPAGET P,  ";
            SQL = SQL + ComNum.VBLF + "                KOSMOS_EMR.EMR_TREATT X  ";
            SQL = SQL + ComNum.VBLF + "            WHERE X.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "                AND P.TREATNO = X.TREATNO  ";
            SQL = SQL + ComNum.VBLF + "            GROUP BY P.TREATNO, P.FORMCODE) SS,  ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRFORM F, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRMAPPING M ";
            SQL = SQL + ComNum.VBLF + "    WHERE T.PATID = '" + mPTNO + "' ";
            if (optEmrInOutFormO.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND T.CLASS = 'O'         ";
            }
            else if (optEmrInOutFormI.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND T.CLASS = 'I'         ";
            }
            SQL = SQL + ComNum.VBLF + "        AND T.TREATNO = SS.TREATNO  ";
            SQL = SQL + ComNum.VBLF + "        AND M.FORMCODE = SS.FORMCODE ";
            SQL = SQL + ComNum.VBLF + "        AND F.FORMNO = M.FORMNO ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL  ";
            SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,   ";
            SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,   ";
            SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD,  ";
            SQL = SQL + ComNum.VBLF + "      B.FORMNAME1 FORMNAME,  C.GRPFORMNO,  ";
            SQL = SQL + ComNum.VBLF + "      (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = C.GRPFORMNO) AS GRPFORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME,  ";
            SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN  ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXMLMST A INNER JOIN KOSMOS_EMR.EMRFORM B  ";
            SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = B.FORMNO  ";
            SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRFORM C  ";
            SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = C.FORMNO  ";
            SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
            if (optEmrInOutFormO.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'O'         ";
            }
            else if (optEmrInOutFormI.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = 'I'         ";
            }
            SQL = SQL + ComNum.VBLF + "    ) ET, ";
            SQL = SQL + ComNum.VBLF + "   KOSMOS_EMR.EMRGRPFORM GP";
            SQL = SQL + ComNum.VBLF + "   WHERE ET.GRPFORMNO = GP.GRPFORMNO";
            SQL = SQL + ComNum.VBLF + "GROUP BY GP.DISPSEQ, ET.GRPFORMNO,  ET.FORMNO, ET.FORMCODE";
            SQL = SQL + ComNum.VBLF + "ORDER BY GP.DISPSEQ, ET.FORMNO";

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

            ssViewEmrAcpForm_Sheet1.RowCount = dt.Rows.Count + 1;
            ssViewEmrAcpForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssViewEmrAcpForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CNT"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PCNT"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 4].Text = ""; // dt.Rows[i]["UPDATENO"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                ssViewEmrAcpForm_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            ssViewEmrAcpForm_Sheet1.Cells[i, 0].Text = "Dr Order";
            ssViewEmrAcpForm_Sheet1.Cells[i, 1].Text = "1";
            ssViewEmrAcpForm_Sheet1.Cells[i, 2].Text = "0";
            ssViewEmrAcpForm_Sheet1.Cells[i, 3].Text = "1680";
            ssViewEmrAcpForm_Sheet1.Cells[i, 4].Text = "O";
            ssViewEmrAcpForm_Sheet1.Cells[i, 5].Text = "O";
            ssViewEmrAcpForm_Sheet1.Cells[i, 6].Text = "000";

            Cursor.Current = Cursors.Default;
        }


        private void ssViewEmrAcpForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
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
            ssViewEmrAcpForm_Sheet1.Cells[0, 0, ssViewEmrAcpForm_Sheet1.RowCount - 1, ssViewEmrAcpForm_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpForm_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpForm_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            ssViewEmrAcpFormChartList_Sheet1.RowCount = 0;

            string strFormNo = "";
            string strFormCode = "";

            strFormNo = ssViewEmrAcpForm_Sheet1.Cells[Row, 3].Text.Trim();
            strFormCode = ssViewEmrAcpForm_Sheet1.Cells[Row, 6].Text.Trim();

            GetHisChartDept(strFormNo, strFormCode);
        }

        private void GetHisChartDept(string strFormNo, string strFormCode)
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
                SQL = SQL + ComNum.VBLF + "    ET.MEDENDDATE, ET.MEDENDTIME, ET.MEDDEPTCD, ET.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    ET.FORMNAME , ET.GRPFORMNO, ET.GRPFORMNAME, ET.DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    ET.TREATNO, ET.FORMCODE,  ET.PCNT, ET.SCANYN ";
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,  ";
                SQL = SQL + ComNum.VBLF + "            F.FORMNAME1 FORMNAME,  F.GRPFORMNO, ";
                SQL = SQL + ComNum.VBLF + "            (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + "            (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_TREATT T, ";
                SQL = SQL + ComNum.VBLF + "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT, ";
                SQL = SQL + ComNum.VBLF + "                TO_CHAR(P.TREATNO) AS EMRNO1 ";
                SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_EMR.EMR_CHARTPAGET P, ";
                SQL = SQL + ComNum.VBLF + "                KOSMOS_EMR.EMR_TREATT X ";
                SQL = SQL + ComNum.VBLF + "            WHERE X.PATID = '" + mPTNO + "' ";
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND X.CLINCODE <> 'NP'";
                }
                SQL = SQL + ComNum.VBLF + "                AND P.TREATNO = X.TREATNO ";
                SQL = SQL + ComNum.VBLF + "            GROUP BY P.TREATNO, P.FORMCODE) SS, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRFORM F, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRMAPPING M ";
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
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "      B.FORMNAME1 FORMNAME,  C.GRPFORMNO, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = C.GRPFORMNO) AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXMLMST A INNER JOIN KOSMOS_EMR.EMRFORM B ";
                SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRFORM C ";
                SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = C.FORMNO ";
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
                SQL = SQL + ComNum.VBLF + "          AND A.FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "    ) ET ";
                SQL = SQL + ComNum.VBLF + "    WHERE ET.FORMNO = '" + strFormNo + "'";
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
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssViewEmrAcpFormChartList_Sheet1.RowCount = dt.Rows.Count;
                ssViewEmrAcpFormChartList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                    if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "")
                    {
                        ssViewEmrAcpFormChartList_Sheet1.Cells[i, 2].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                    }
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SCANYN"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    ssViewEmrAcpFormChartList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["FORMCODE"].ToString().Trim();
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

        private void ssViewEmrAcpFormChartList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpFormChartList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpFormChartList, e.Column);
                return;
            }
            ssViewEmrAcpFormChartListCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpFormChartListCellDoubleClick(int Row, int Col)
        {
            ssViewEmrAcpFormChartList_Sheet1.Cells[0, 0, ssViewEmrAcpFormChartList_Sheet1.RowCount - 1, ssViewEmrAcpFormChartList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpFormChartList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;


            //TODO Init_Modify
            string strFormName = "";
            string strSCANYN = "N";
            string strFormNo = "0";
            string strEmrNo = "0";
            string strTreatNo = "0";
            string strFormCode = "";
            string strInOutCls = "";
            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDeptCd = "";
            //string strMedFrTime = "";
            //string strMedEndTime = "";
            //string strMedMedDrCd = "";

            strInOutCls = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 0].Text.Trim();
            strMedFrDate = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            strMedEndDate = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 3].Text.Trim().Replace("-", "");
            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            strMedDeptCd = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 4].Text.Trim();

            //strMedFrTime = "";
            //strMedEndTime = "";
            //strMedMedDrCd = "";

            strFormNo = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 5].Text.Trim();
            strFormName = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 6].Text.Trim();
            strSCANYN = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 7].Text.Trim();
            strEmrNo = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 8].Text.Trim();
            strFormCode = ssViewEmrAcpFormChartList_Sheet1.Cells[Row, 9].Text.Trim();

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

            pView = clsEmrChart.ClearPatient();
            pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
            if (pView == null)
            {
                ComFunc.MsgBoxEx(this, "서직지 작성 내역을 찾을 수 없습니다.");
                return;
            }
            pView.formNo = (long)VB.Val(strFormNo);
            pView.updateNo = 0;

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode);
        }

        #endregion panViewEmrAcpForm


        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                trvEmrView.Nodes.Clear();
                ssViewEmrAcpCopy.Visible = false;
            }
            else
            {
                ssViewEmrAcpCopy.Visible = true;
            }
        }

        private void btnSearchEmrCopy_Click(object sender, EventArgs e)
        {
            GetDataCopy();
        }

        private void GetDataCopy()
        {
            if (mPTNO.Trim() == "") return;
            pCopy = null;

            trvEmrView.Nodes.Clear();
            ssViewEmrAcpCopy_Sheet1.RowCount = 0;

            if (chkDate.Checked == true)
            {
                if (optEmrInOutCopyO.Checked == false)
                {
                    ComFunc.MsgBoxEx(this, "기간별은 외래만 조회가 가능합니다.");
                }
                optEmrInOutCopyO.Checked = true;
                optEmrInOutCopyA.Checked = false;
                optEmrInOutCopyI.Checked = false;

                string strMedFrDate = dtpDateCopyS.Value.ToString("yyyyMMdd");
                string strMedDeptCd = "";
                string strMedMedDrCd = "";
                string strMedEndDate = dtpDateCopyE.Value.ToString("yyyyMMdd");

                if (cboDept.SelectedIndex > 0)
                {
                    strMedDeptCd = VB.Right(cboDept.Text.Trim(), 3).Trim();
                }

                GetHisSheetDsp("O", strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate, "1");
            }
            else
            {
                GetHisSheet();
            }
        }

        private void GetHisSheetDsp(string strInOutCls, string strMedFrDate, string strMedDeptCd,
                                    string strMedMedDrCd, string strMedEndDate, string strOption)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string sKey1 = "";
            string sKey2 = "";
            string sKey3 = "";
            string sKey4 = "";
            string sKey5 = "";

            string sKeyOld1 = "";
            string sKeyOld2 = "";
            string sKeyOld3 = "";
            string sKeyOld4 = "";
            string sKeyOld5 = "";
            string sKeyName1 = "";
            string sKeyName2 = "";
            string sKeyName3 = "";
            string sKeyName4 = "";
            //string strSCANCNT = "";
            string sChartDT = "";

            TreeNode oNodex;

            trvEmrView.Nodes.Clear();

            SQL = " SELECT ET.EMRNO, ET.FORMNO, ET.CHARTDATE, ET.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "    ET.ACPNO, ET.PTNO, ET.INOUTCLS, ET.MEDFRDATE, ET.MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "    ET.MEDENDDATE, ET.MEDENDTIME, ET.MEDDEPTCD, ET.MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "    ET.FORMNAME , ET.GRPFORMNO, ET.GRPFORMNAME, ET.DEPTKORNAME, ";
            SQL = SQL + ComNum.VBLF + "    ET.TREATNO, ET.FORMCODE,  ET.PCNT, ET.SCANYN ";
            SQL = SQL + ComNum.VBLF + "FROM ";
            SQL = SQL + ComNum.VBLF + "    (SELECT  TO_NUMBER(SS.EMRNO1) AS EMRNO, F.FORMNO, T.INDATE AS CHARTDATE, '120000' AS CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "            0 AS ACPNO, T.PATID AS PTNO, T.CLASS AS INOUTCLS,  T.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "            T.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, T.CLINCODE AS MEDDEPTCD, T.DOCCODE AS MEDDRCD,  ";
            SQL = SQL + ComNum.VBLF + "            F.FORMNAME1 FORMNAME,  F.GRPFORMNO, ";
            SQL = SQL + ComNum.VBLF + "            (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = F.GRPFORMNO) AS GRPFORMNAME, ";
            SQL = SQL + ComNum.VBLF + "            (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = T.CLINCODE) AS DEPTKORNAME, ";
            SQL = SQL + ComNum.VBLF + "            T.TREATNO, SS.FORMCODE,  SS.PCNT, 'S' AS SCANYN ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_TREATT T, ";
            SQL = SQL + ComNum.VBLF + "        (SELECT P.TREATNO, P.FORMCODE, COUNT(P.PAGENO) AS PCNT, ";
            SQL = SQL + ComNum.VBLF + "                TO_CHAR(P.TREATNO) AS EMRNO1 ";
            SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_EMR.EMR_CHARTPAGET P, ";
            SQL = SQL + ComNum.VBLF + "                KOSMOS_EMR.EMR_TREATT X ";
            SQL = SQL + ComNum.VBLF + "            WHERE X.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "                AND P.TREATNO = X.TREATNO ";
            SQL = SQL + ComNum.VBLF + "            GROUP BY P.TREATNO, P.FORMCODE) SS, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRFORM F, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_EMR.EMRMAPPING M ";
            SQL = SQL + ComNum.VBLF + "    WHERE T.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "        AND T.TREATNO = SS.TREATNO  ";
            SQL = SQL + ComNum.VBLF + "        AND M.FORMCODE = SS.FORMCODE ";
            SQL = SQL + ComNum.VBLF + "        AND F.FORMNO = M.FORMNO ";
            if (strOption == "")
            {
                SQL = SQL + ComNum.VBLF + "        AND T.CLASS = '" + strInOutCls + "'";
                SQL = SQL + ComNum.VBLF + "        AND T.INDATE = '" + strMedFrDate + "'";
                if (strInOutCls == "O")
                {
                    if (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125"))
                    {
                        SQL = SQL + ComNum.VBLF + "                AND T.CLINCODE = 'RA'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                AND T.CLINCODE = '" + strMedDeptCd + "'";
                    }
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        AND T.INDATE >= '" + strMedFrDate + "'";
                SQL = SQL + ComNum.VBLF + "        AND T.INDATE <= '" + strMedEndDate + "'";
            }
            //2012-11-23 NP챠트 조회 조건 간단하게 변경
            if (mViewNpChart == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND T.CLINCODE <> 'NP'";
            }

            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,  ";
            SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ";
            SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "      B.FORMNAME1 FORMNAME,  C.GRPFORMNO, ";
            SQL = SQL + ComNum.VBLF + "      (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = C.GRPFORMNO) AS GRPFORMNAME, ";
            SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ";
            SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXMLMST A INNER JOIN KOSMOS_EMR.EMRFORM B ";
            SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRFORM C ";
            SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = C.FORMNO ";
            SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = '" + strInOutCls + "'";
            if (strOption == "")
            {
                SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE = '" + strMedFrDate + "'";
                if (strInOutCls == "O")
                {
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                    {
                        SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = 'MD'";
                        SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD IN ('1107','1125') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD NOT IN ('1107','1125')";
                    }
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE >= '" + strMedFrDate + "'";
                SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE <= '" + strMedEndDate + "'";
            }

            if (mViewNpChart == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
            }

            if (clsEmrQueryPohangS.START_TUYAK(clsDB.DbCon) == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO NOT IN ('1796')";
                SQL = SQL + ComNum.VBLF + "    UNION ALL";
                SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.FORMNO, A.CHARTDATE, A.CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + "      0 ACPNO, A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "      A.MEDENDDATE, A.MEDENDTIME, A.MEDDEPTCD, A.MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "      B.FORMNAME1 FORMNAME,  C.GRPFORMNO, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT GRPFORMNAME FROM KOSMOS_EMR.EMRGRPFORM WHERE GRPFORMNO = C.GRPFORMNO) AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = A.MEDDEPTCD) AS DEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'T' AS SCANYN ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML_TUYAK A INNER JOIN KOSMOS_EMR.EMRFORM B ";
                SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRFORM C ";
                SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "      WHERE A.PTNO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "        AND A.INOUTCLS = '" + strInOutCls + "'";
                if (strOption == "")
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE = '" + strMedFrDate + "'";
                    if (strInOutCls == "O")
                    {
                        if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                        {
                            SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = 'MD'";
                            SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD IN ('1107','1125') ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                AND A.MEDDEPTCD = '" + strMedDeptCd + "'";
                            SQL = SQL + ComNum.VBLF + "                AND A.MEDDRCD NOT IN ('1107','1125')";
                        }
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE >= '" + strMedFrDate + "'";
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDFRDATE <= '" + strMedEndDate + "'";
                }
                if (mViewNpChart == false)
                {
                    SQL = SQL + ComNum.VBLF + "        AND A.MEDDEPTCD <> 'NP'";
                }
            }

            if (strInOutCls == "O")
            {
                bool isScanYn = FindScanImageYn(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate);

                if (isScanYn == false)
                {
                    SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "    SELECT ";
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && strMedMedDrCd == "1107"))
                    {
                        SQL = SQL + ComNum.VBLF + "      'RA' AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      '19094' AS MEDDRCD,  ";
                    }
                    else if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && strMedMedDrCd == "1125"))
                    {
                        SQL = SQL + ComNum.VBLF + "      'RA' AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      '30322' AS MEDDRCD,  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    }
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER' AS FORMNAME ,  ";
                    SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      '의사지시기록' AS GRPFORMNAME,  ";
                    if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                    {
                        SQL = SQL + ComNum.VBLF + "      '류마티스내과' AS DEPTKORNAME,  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ";
                    }
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OORDER O, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_DOCTOR N, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_SUN     S ";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";

                    if (strOption == "")
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                        if (strInOutCls == "O")
                        {
                            if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                            {
                                SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                                SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + strMedDeptCd + "'";
                                SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125')";
                            }
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ";
                    }

                    if (mViewNpChart == false)
                    {
                        SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";
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
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 2090 AS FORMNO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'O' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                    SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER(ER)' AS FORMNAME ,  ";
                    SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      'ER 의사지시기록' AS GRPFORMNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = 'ER') AS DEPTKORNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ODOSAGE D, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_DOCTOR N, ";
                    SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_SUN     S ";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
                    if (strOption == "")
                    {
                        if (VB.Val(strMedEndDate) != 0)
                        {
                            SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                            SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ";
                        }
                        else
                        {
                            if (clsEmrQueryPohangS.NEXTDATE(clsDB.DbCon, strMedFrDate, mPTNO) == true)
                            {
                                SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                                ComFunc pComFunc = new ComFunc();
                                SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + pComFunc.DATE_ADD(clsDB.DbCon, ComFunc.FormatStrToDate(strMedFrDate, "D"), 1) + "','YYYY-MM-DD')  ";
                                pComFunc = null;
                            }
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ";
                    }

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
            }
            else
            {
                bool isScanYn = FindScanImageYn(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate);
                if (isScanYn == false)
                {
                    SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT ";
                    SQL = SQL + ComNum.VBLF + "     TO_NUMBER(TO_CHAR(O.BDATE,'YYYYMMDD')) AS EMRNO, 1680 AS FORMNO, TO_CHAR(O.BDATE,'YYYYMMDD') AS CHARTDATE, '120000' AS CHARTTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      0 AS ACPNO, O.PTNO, 'I' AS INOUTCLS, TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,   ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, ";
                    if (strMedDeptCd == "MD" && strMedMedDrCd == "1107")
                    {
                        SQL = SQL + ComNum.VBLF + "      'RA' AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      '19094' AS MEDDRCD,  ";
                    }
                    else if (strMedDeptCd == "MD" && strMedMedDrCd == "1125")
                    {

                        SQL = SQL + ComNum.VBLF + "      'RA' AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      '30322' AS MEDDRCD,  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      O.DEPTCODE AS MEDDEPTCD, ";
                        SQL = SQL + ComNum.VBLF + "      max(O.DRCODE) AS MEDDRCD,  ";
                    }
                    SQL = SQL + ComNum.VBLF + "      'Dr ORDER' AS FORMNAME ,  ";
                    SQL = SQL + ComNum.VBLF + "      16 AS GRPFORMNO,  ";
                    SQL = SQL + ComNum.VBLF + "      '의사지시기록' AS GRPFORMNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = O.DEPTCODE) AS DEPTKORNAME,  ";
                    SQL = SQL + ComNum.VBLF + "      0 AS TREATNO, '000' AS FORMCODE,  0 AS PCNT, 'O' AS SCANYN  ";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER O, ";
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

                    if (strOption == "")
                    {
                        if (strInOutCls == "O")
                        {
                            if (strMedDeptCd == "RA" || (strMedDeptCd == "MD" && (strMedMedDrCd == "1107" || strMedMedDrCd == "1125")))
                            {
                                SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                                SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + strMedDeptCd + "'";
                                SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125') ";
                            }
                        }
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                        if (VB.Val(strMedEndDate) != 0)
                        {
                            SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD')  ";
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD')  ";
                    }

                    SQL = SQL + ComNum.VBLF + "          AND O.GBSTATUS NOT IN ('D-','D','D+' )   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)    ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)       ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+)  ";
                    SQL = SQL + ComNum.VBLF + "          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')";
                    SQL = SQL + ComNum.VBLF + "          GROUP BY  O.PTNO, O.BDATE, O.DEPTCODE ";
                }
            }
            SQL = SQL + ComNum.VBLF + "    ) ET ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ET.INOUTCLS DESC, ET.MEDDEPTCD, ET.GRPFORMNO, ET.FORMNO, ET.CHARTDATE DESC, ET.EMRNO ";

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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

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

                sKey4 = dt.Rows[i]["FORMNO"].ToString().Trim();
                sKeyName4 = dt.Rows[i]["FORMNAME"].ToString().Trim();

                sKey5 = dt.Rows[i]["SCANYN"].ToString().Trim() + "|" + dt.Rows[i]["EMRNO"].ToString().Trim() + "|" + dt.Rows[i]["FORMCODE"].ToString().Trim();

                sChartDT = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "DS") + ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");

                if (sKey1 == "O")
                {
                    sKeyName1 = "외래";
                }
                else
                {
                    sKeyName1 = "입원";
                }

                if (sKeyOld1 != sKey1)
                {
                    oNodex = trvEmrView.Nodes.Add(sKeyHead + sKey1, sKeyName1, 2, 1);
                    sKeyOld1 = sKey1;

                    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                    sKeyOld2 = sKey2;

                    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                    sKeyOld3 = sKey3;

                    oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                    sKeyOld4 = sKey4;
                }
                else
                {
                    if (sKeyOld2 != sKey2)
                    {
                        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2, sKeyName2, 2, 1);
                        sKeyOld2 = sKey2;
                        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                        sKeyOld3 = sKey3;
                        oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                        sKeyOld4 = sKey4;
                    }
                    else
                    {
                        if (sKeyOld3 != sKey3)
                        {
                            oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, sKeyName3, 2, 1);
                            sKeyOld3 = sKey3;
                            oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                            sKeyOld4 = sKey4;
                        }
                        else
                        {
                            if (sKeyOld5 != sKey5)
                            {
                                oNodex = trvEmrView.Nodes.Find(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3, true)[0].Nodes.Add(sKeyHead + sKey1 + sKeyHead + sKey2 + sKeyHead + sKey3 + sKeyHead + sKey4 + sKeyHead + sKey5, sKeyName4 + " [" + sChartDT + "]", 2, 1);
                                sKeyOld5 = sKey5;
                            }
                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;

        }

        private void GetHisSheet()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            trvEmrView.Nodes.Clear();
            ssViewEmrAcpCopy_Sheet1.RowCount = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE,";
            SQL = SQL + ComNum.VBLF + "  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME,";
            SQL = SQL + ComNum.VBLF + "  (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME, DD.DRNAME";
            SQL = SQL + ComNum.VBLF + "FROM (";

            if (optEmrInOutCopyO.Checked == true || optEmrInOutCopyA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " SELECT 'O' AS INOUTCLS, A.Pano AS PTNO,A.SName AS PTNAME, A.Sex, A.Age, ";
                SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "     NVL(TO_CHAR(A.BDATE,'YYYYMMDD'),  TO_CHAR(A.ACTDATE,'YYYYMMDD')) AS MEDFRDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.OPD_MASTER A";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + mPTNO + "' ";
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.GBUSE = 'Y'";
                }
            }

            if (optEmrInOutCopyA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "UNION ALL";
            }

            if (optEmrInOutCopyI.Checked == true || optEmrInOutCopyA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age, ";
                SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER A ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "AND A.GBSTS <> '9'";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                if (GstrView01 == "1")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXMLMST A, ";
                    SQL = SQL + ComNum.VBLF + "    KOSMOS_PMPA.IPD_NEW_MASTER B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO =  '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "AND A.INOUTCLS = 'I' ";
                    SQL = SQL + ComNum.VBLF + "AND B.GBSTS = '9' ";
                    SQL = SQL + ComNum.VBLF + "AND A.PTNO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDDEPTCD = B.DeptCode ";
                }
                else if (GstrView01 == "0" || GstrView01 == "")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT 'I' AS INOUTCLS, PANO AS PTNO, SName AS PTNAME, SEX, AGE,";
                    SQL = SQL + ComNum.VBLF + "    DEPTCODE AS MEDDEPTCD, DRCODE AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(INDATE,'YYYYMMDD') AS MEDFRDATE, TO_CHAR(INDATE, 'HH24MI') AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(OUTDATE,'YYYYMMDD') AS MEDENDDATE, '' AS MEDENDTIME";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + "   WHERE PANO =  '" + mPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "     AND GBSTS = '9'";
                }
            }

            SQL = SQL + ComNum.VBLF + "UNION ALL ";

            SQL = SQL + ComNum.VBLF + " SELECT A.CLASS AS INOUTCLS, A.PATID AS PTNO,  B.NAME AS PTNAME, B.Sex, 0 AS Age,  ";
            SQL = SQL + ComNum.VBLF + "    A.CLINCODE AS MEDDEPTCD, A.DOCCODE AS MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "    A.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "    A.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT A, KOSMOS_EMR.EMR_PATIENTT B   ";
            SQL = SQL + ComNum.VBLF + "WHERE A.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "AND A.DELDATE IS NULL";
            SQL = SQL + ComNum.VBLF + "AND A.PATID = B.PATID    ";
            if (optEmrInOutCopyO.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'O'";
            }
            else if (optEmrInOutCopyI.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'I'";
            }
            SQL = SQL + ComNum.VBLF + "AND (A.CLASS, A.INDATE, A.CLINCODE) NOT IN ( ";
            SQL = SQL + ComNum.VBLF + "            SELECT INOUTCLS, MEDFRDATE, MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM ";
            SQL = SQL + ComNum.VBLF + "            (SELECT 'O' AS INOUTCLS, NVL(TO_CHAR(A1.BDATE,'YYYYMMDD'),  TO_CHAR(A1.ACTDATE,'YYYYMMDD') ) AS MEDFRDATE, ";
            SQL = SQL + ComNum.VBLF + "            DECODE(A1.DRCODE,'1107','RA','1125','RA',A1.DeptCode) AS MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_PMPA.OPD_MASTER A1";
            SQL = SQL + ComNum.VBLF + "            WHERE A1.PANO = '" + mPTNO + "' ";
            if (gJinGubun == "" || gJinGubun == "2")
            {
                SQL = SQL + ComNum.VBLF + "    AND A1.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND A1.Jin    IN ('0','1','2','3','4','5','6','7','8','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }

            if (clsEmrPublic.gUserGrade == "SIMSA" || clsEmrPublic.gUserGrade == "")
            {
                SQL = SQL + ComNum.VBLF + "                AND A1.GBUSE = 'Y' ";
            }

            SQL = SQL + ComNum.VBLF + "            UNION ALL ";
            SQL = SQL + ComNum.VBLF + "             SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, A2.DeptCode AS MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_PMPA.IPD_NEW_MASTER A2  ";
            SQL = SQL + ComNum.VBLF + "            WHERE A2.PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "            AND A2.GBSTS <> '9') ";
            SQL = SQL + ComNum.VBLF + "    )  ";

            SQL = SQL + ComNum.VBLF + ") XX, KOSMOS_PMPA.BAS_DOCTOR DD ";
            SQL = SQL + ComNum.VBLF + "  WHERE XX.MEDDRCD = DD.DRCODE(+)";
            SQL = SQL + ComNum.VBLF + "  AND XX.INOUTCLS IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "    ORDER BY XX.INOUTCLS ASC, XX.MEDFRDATE DESC,  XX.MEDDEPTCD";


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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDEPTCODE = "";
            //int FnCheck = 0;
            //string FstrDateCheck = "";

            ssViewEmrAcpCopy_Sheet1.RowCount = dt.Rows.Count;
            ssViewEmrAcpCopy_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                strMedFrDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                strMedEndDate = "";
                if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "")
                {
                    strMedEndDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                }
                strMedDEPTCODE = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[i, 1].Text = strMedFrDate;

                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "I")
                {
                    if (clsEmrQueryPohangS.GetIPDCancel(clsDB.DbCon, mPTNO, strMedFrDate, strMedEndDate, strMedDEPTCODE) == "입원취소" && dt.Rows[i]["MEDENDTIME"].ToString().Trim() == "")
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Text = "입원취소";
                    }
                    else
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Text = strMedEndDate;
                    }
                }
                else
                {
                    if (clsEmrQueryPohangS.READ_DOCREPRINT(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) == true)
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Text = "서류재발급";
                    }
                    else
                    {
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpCopy_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                    }
                }
                ssViewEmrAcpCopy_Sheet1.Cells[i, 3].Text = strMedDEPTCODE;

                if ((dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1107" || dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1125") && dt.Rows[i]["MEDDEPTCD"].ToString().Trim() == "MD")
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 4].Text = "류마티스내과";
                }
                else
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                }
                ssViewEmrAcpCopy_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[i, 6].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "O")
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 7].Text = "";
                }
                else
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 7].Text = dt.Rows[i]["MEDENDTIME"].ToString().Trim();
                }
                ssViewEmrAcpCopy_Sheet1.Cells[i, 8].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                ssViewEmrAcpCopy_Sheet1.Cells[i, 9].Text = strMedEndDate;
                ssViewEmrAcpCopy_Sheet1.Cells[i, 10].Text = strMedDEPTCODE;
                //ssViewEmrAcpCopy_Sheet1.Cells[i, 11].Text = ACPNO;
            }
            dt.Dispose();
            dt = null;

            if (optEmrInOutDeptO.Checked == true)
            {
                ssViewEmrAcpCopy_Sheet1.Columns[2].Visible = false;
            }
            if (optEmrInOutDeptI.Checked == true)
            {
                ssViewEmrAcpCopy_Sheet1.Columns[2].Visible = true;
            }

            string cBDATE = "";
            string cDEPTCD = "";
            string cIO = "";

            for (i = 0; i < ssViewEmrAcpCopy_Sheet1.RowCount; i++)
            {
                cIO = ssViewEmrAcpCopy_Sheet1.Cells[i, 0].Text.Trim();
                cBDATE = ssViewEmrAcpCopy_Sheet1.Cells[i, 1].Text.Trim();
                cDEPTCD = ssViewEmrAcpCopy_Sheet1.Cells[i, 10].Text.Trim();
                if (clsVbfunc.READ_SPECIAL_SERVICE(clsDB.DbCon, mPTNO, cBDATE, cDEPTCD, cIO) == true)
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 0].BackColor = Color.Green;
                }
                else
                {
                    ssViewEmrAcpCopy_Sheet1.Cells[i, 0].BackColor = Color.White;
                }
            }

            Cursor.Current = Cursors.Default;

        }

        private void ssViewEmrAcpCopy_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpCopy_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpCopy, e.Column);
                return;
            }

            trvEmrView.Nodes.Clear();
            pCopy = null;

            ssViewEmrAcpCopyCellDoubleClick(e.Row, e.Column);
        }

        private void ssViewEmrAcpCopyCellDoubleClick(int Row, int Column)
        {
            ssViewEmrAcpCopy_Sheet1.Cells[0, 0, ssViewEmrAcpCopy_Sheet1.RowCount - 1, ssViewEmrAcpCopy_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpCopy_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpCopy_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strInOutCls = "";
            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDeptCd = "";
            string strMedFrTime = "";
            string strMedEndTime = "";
            string strMedMedDrCd = "";

            strInOutCls = ssViewEmrAcpCopy_Sheet1.Cells[Row, 0].Text.Trim();
            strMedFrDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            strMedEndDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");
            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            strMedDeptCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 3].Text.Trim();

            strMedFrTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 6].Text.Trim();
            strMedEndTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 7].Text.Trim();
            strMedMedDrCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 8].Text.Trim();

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "NP")
                {
                    ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                    return;
                }
            }

            pCopy = null;
            pCopy = clsEmrChart.ClearPatient();
            pCopy = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedDeptCd);
            if (pCopy == null)
            {
                ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                return;
            }

            GetHisSheetDsp(strInOutCls, strMedFrDate, strMedDeptCd, strMedMedDrCd, strMedEndDate, "");

        }

        private void trvEmrView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode Node;
            Node = e.Node;
            string strIndex = Node.Name.ToString();

            if (Node.GetNodeCount(false) > 0)
            {
                if (Node.IsExpanded == false)
                {
                    Node.Expand();
                }
                return;
            }
            string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
            string[] arryEmrNo = VB.Split(VB.Trim(strParams[5]), "|");

            string strFormNo = strParams[4];
            //string strUpdateNo = "0";

            string strSCANYN = arryEmrNo[0];
            string strEmrNo = arryEmrNo[1];
            string strFormCode = arryEmrNo[2];

            string strTreatNo = "0";

            pView = clsEmrChart.ClearPatient();
            pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEmrNo);
            if (pView == null)
            {
                ComFunc.MsgBoxEx(this, "서직지 작성 내역을 찾을 수 없습니다.");
                return;
            }
            pView.formNo = (long)VB.Val(strFormNo);
            pView.updateNo = 0;

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            if (VB.Val(strEmrNo) == 0)
            {
                return;
            }

            if (strSCANYN == "S")
            {
                strTreatNo = strEmrNo;
                strEmrNo = "0";
            }
            rViewChart(pView, fView, strEmrNo, strTreatNo, strSCANYN, strFormCode);
        }

        private void trvEmrView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.GetNodeCount(false) == 0)
            {
                if (e.Node.Checked == true)
                {
                    string[] arryPara = VB.Split(e.Node.Name.ToString(), sKeyHead);
                    string sEmrNo = VB.Split(arryPara[5], "|")[0];

                    if (sEmrNo == "S")
                    {
                        e.Node.Checked = false;
                        ComFunc.MsgBoxEx(this, "스캔은 오른쪽 이미지 View화면에서 복사 신청 하십시오.");
                    }
                }
                return;
            }
            foreach (TreeNode childNode in e.Node.Nodes)
            {
                childNode.Checked = e.Node.Checked;
            }
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
        }

        private bool SaveDataPrintReq()
        {
            string strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수");

            if (VB.Val(strCopyCnt) == 0)
            {
                ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
                strCopyCnt = VB.InputBox("복사 신청할 부수를 입력하십시요.(숫자만 입력)", "인쇄 부수");
                if (VB.Val(strCopyCnt) == 0)
                {
                    ComFunc.MsgBoxEx(this, "인쇄 부수를 정확히 입력해 주십시요.");
                    return false;
                }
            }

            List<String> strTree = CheckedNames(trvEmrView.Nodes);

            if (strTree.Count == 0)
            {
                ComFunc.MsgBoxEx(this, "기록지를 정확히 선택해 주십시요." + ComNum.VBLF + "선택된 기록지가 없습니다.");
                return false;
            }

            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strREQDATE = VB.Left(strCurDateTime, 8);
                string strPRTREQNO = (ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRPRTREQNO")).ToString();

                for (i = 0; i < strTree.Count; i++)
                {
                    string[] arryPara = VB.Split(strTree[i], sKeyHead);
                    string[] arryEmrNo = VB.Split(arryPara[5], "|");
                    string strPRTOPTION = "";

                    if (arryPara[2] != "ER" && arryPara[3] == "2090")
                    {
                        arryPara[2] = "ER";
                    }

                    if (chkDate.Checked == true && (arryPara[3] == "1680" || arryPara[3] == "2090"))
                    {
                        strPRTOPTION = arryEmrNo[0] + "^" + arryEmrNo[1] + "^" + arryPara[2] + "^" + clsEmrQuery.ReadDeptDoctor(clsDB.DbCon, arryEmrNo[1], arryPara[2], mPTNO) + "^" + arryEmrNo[1];
                    }
                    else
                    {
                        if (pCopy == null)
                        {
                            ComFunc.MsgBoxEx(this, "접수 정보를 찾을 수 없습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                        strPRTOPTION = pCopy.inOutCls + "^" + pCopy.medFrDate + "^" + arryPara[2] + "^" + pCopy.medDrCd + "^" + arryEmrNo[1];
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
                            SQL = SQL + ComNum.VBLF + "        EMRNO, SCANYN, FORMCODE, USEID,INPDATE,INPTIME,PRTOPTION, REQCNT)";
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
                            SQL = SQL + ComNum.VBLF + "            '" + strPRTOPTION + "'," + VB.Val(strCopyCnt) + ")";

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
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
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
                            aResult.Add(aNode.Name.ToString());
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
    }
}
