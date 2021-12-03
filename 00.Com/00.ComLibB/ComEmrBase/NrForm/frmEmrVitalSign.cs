using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrVitalSign : Form, EmrChartForm, FormEmrMessage
    {
        class GV
        {
            public string Code = "";
            public string Y = "";
            public double X = 0;
        }

        #region FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {

        }

        public void MsgDelete()
        {
            return;

        }

        public void MsgClear()
        {
            return;

        }

        public void MsgPrint()
        {
            return;

        }
        #endregion

        #region 폼에 사용하는 변수를 코딩하는 부분
        /// <summary>
        ///    //기록지 작성방향(H: 옆으로, V:아래로)
        /// </summary>
        private const string mDirection = "H";
        /// <summary>
        /// //해드 칼럼 수
        /// </summary>
        private const int mintTCol = 5;
        ////해드 로우 수
        private const int mintTRow = 4;
        ///// <summary>
        ///// //밑줄
        ///// </summary>
        //private const int mintBRow = 3;  
        ///// <summary>
        ///// 셀 높이
        ///// </summary>
        //private const int mintColW_I = 120;
        /// <summary>
        /// 셀 높이
        /// </summary>
        private const int mintColW_V = 70;

        /// <summary>
        /// //JOBGB Colume
        /// </summary>
        private const int COL_JOB = 0;
        /// <summary>
        /// //ITEM Colume
        /// </summary>
        private const int COL_ITEM = 1;
        /// <summary>
        /// //확장 Colume
        /// </summary>
        private const int COL_EXP = 2;
        /// <summary>
        /// //그룹 이름 Colume
        /// </summary>
        private const int COL_GNAME = 3;
        /// <summary>
        /// //아이템 이름 Colume
        /// </summary>
        private const int COL_INAME = 4;
        /// <summary>
        /// //DATA 시작 Colume
        /// </summary>
        private const int COL_DATAS = 5;

        public string mstrFormNameGb = "기록지관리";
        public string mstrFormNameWard = "임상관찰";
        /// <summary>
        /// IVT : 혈액학적 ssVital_Sheet1, IIO : 섭취배셜 ssVital_Sheet1
        /// </summary>
        private string mJOBGB = "IVT";

        private string mstrNightTimeAm = "00:00/07:00";
        private string mstrDayTime = "07:01/15:00";
        private string mstrEveTime = "15:01/23:00";
        private string mstrNightTime = "23:01/24:00";

        /// <summary>
        /// 아이템코드, 최대값 매핑
        /// </summary>
        private Dictionary<string, int> keysItemMax = null;

        private Font BoldFont = null;

        FarPoint.Win.Spread.FpSpread mSpd;
        FarPoint.Win.Spread.SheetView mSpdView;

        private frmNrIcuTimeSet frmNrIcuTimeSetX = null;

        ContextMenu PopupMenu = null;
        int mPopRow = 0;
        int mPopCol = 0;

        Font Notefont = null;

        //private frmEmrChartNew frmEmrChartX = null;
        //private frmOcrPrint frmOcrPrintX = null;

        /// <summary>
        /// 인터페이스 폼
        /// </summary>
        Form fEmrInterface = null;

        /// <summary>
        /// 기본값 함수로 되어 있는것
        /// </summary>
        frmEmrBaseDefaultValueSet frmEmrBaseDefaultValueSetX = null;

        /// <summary>
        /// 쿼리 두번 도는거 방지
        /// </summary>
        bool isFormLoad = false;

        /// <summary>
        /// ICU 메모
        /// </summary>
        Form frmIcuMemo = null;

        /// <summary>
        /// 수액기록지
        /// </summary>
        Form frmEmrBaseRingerIOX = null;

        /// <summary>
        /// 산소 계산 화면
        /// </summary>
        Form frmO2 = null;
        #endregion

        #region 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "1761";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        //private mtsPanel15.TransparentPanel panEditLock = null;

        private Dictionary<int, ExpendRowVal> ExpendRows = new Dictionary<int, ExpendRowVal>();

        private struct ExpendRowVal
        {
            public string strJOBGB;
            public string strJobExpend;
        }

        #endregion

        #region EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public double SaveDataMsg(string strFlag)
        {
            //TODO
            //double dblEmrNo = pSaveData(strFlag);
            //return dblEmrNo;
            return 0;
        }

        public bool DelDataMsg()
        {
            //TODO
            //return pDelData();
            return false;
        }

        public void ClearFormMsg()
        {
            //TODO
            mstrEmrNo = "0";
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
            //int rtnVal = 0;
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            //return rtnVal;
            return 1;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            //int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            //return rtnVal;
            return 1;
        }
        #endregion

        #region 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            pClearForm();
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        public void pSetEmrInfo()
        {

        }

        #endregion

        #region 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            ////모든 컨트롤을 초기화 한다.
            if (frmIcuMemo != null)
            {
                frmIcuMemo.Dispose();
                frmIcuMemo = null;
            }
            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            LoadData(ssVital_Sheet1, "IVT");
            SetWorkDuty();
        }

        /// <summary>
        /// 데이타를 불러와 세팅한다
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadData(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            LoadDataIVT_ALL(SpdView, strJOBGB);
        }

        /// <summary>
        /// Vital 저장된 값을 조회한다
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadDataIVT_ALL(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            if (p == null) return;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            DateTime dtpNow = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            #region 기존 쿼리
            if (dtpNow < ("2020-09-22").To<DateTime>() && p.ptNo.Substring(0, 3).Equals("810") == false)

            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
                SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
                SQL = SQL + ComNum.VBLF + "                B. INPUSEID, B.INPDATE, B.INPTIME,  ";
                SQL = SQL + ComNum.VBLF + "                (SELECT MAX(U.USENAME) FROM  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
                SQL = SQL + ComNum.VBLF + "                     WHERE  U.USEID = B.INPUSEID) AS INPUSENAME, ";
                SQL = SQL + ComNum.VBLF + "                (SELECT MAX(U.USENAME) FROM  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
                SQL = SQL + ComNum.VBLF + "                     WHERE  U.USEID = A.CHARTUSEID) AS USENAME, "; //U.USENAME,
                SQL = SQL + ComNum.VBLF + "                 BC.BASVAL";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "     ON  B.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "    AND  B.EMRNOHIS = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
                SQL = SQL + ComNum.VBLF + "     ON BC.BASCD = B.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "    AND BC.BSNSCLS = '" + mstrFormNameGb + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호' )";
                //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
                //SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
                if (p.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
                }
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                //SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.UNITCLS, BC.BASVAL";
            }
            #endregion
            else
            {
                SQL =  "WITH M AS                                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "(                                                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "    SELECT A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN,                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "                    B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                    B. INPUSEID, B.INPDATE, B.INPTIME,                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                    (SELECT MAX(U.USENAME) FROM  KOSMOS_EMR.AVIEWEMRUSER U                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "                         WHERE  U.USEID = B.INPUSEID) AS INPUSENAME,                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "                    (SELECT MAX(U.USENAME) FROM  KOSMOS_EMR.AVIEWEMRUSER U                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "                         WHERE  U.USEID = A.CHARTUSEID) AS USENAME,                                                                                                                                             ";
                SQL = SQL + ComNum.VBLF + "                     BC.BASVAL, BC.UNITCLS                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN  KOSMOS_EMR.AEMRCHARTROW B                                                                                                                                                                     ";
                SQL = SQL + ComNum.VBLF + "         ON B.EMRNO = A.EMRNO                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "        AND B.EMRNOHIS = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.AEMRBASCD BC                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "         ON BC.BASCD = B.ITEMCD                                                                                                                                                                                 ";
                SQL = SQL + ComNum.VBLF + "        AND BC.BSNSCLS = '기록지관리'                                                                                                                                                                             ";
                SQL = SQL + ComNum.VBLF + "        AND BC.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호' )                                                                                                                                          ";
                if (p.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "    WHERE A.ACPNO = " + p.acpNo;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    WHERE A.PTNO = '" + p.ptNo + "'";
                }
                SQL = SQL + ComNum.VBLF + "      AND A.FORMNO   = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "--      AND A.UPDATENO = 1                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "      AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "      AND B.ITEMCD NOT IN ('I0000022324') -- 기존 수혈 삭제                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL  --수혈 기록지에서 값 가저오기                                                                                                                                                                            ";
                //SQL = SQL + ComNum.VBLF + "    SELECT EMRNO, EMRNOHIS, CHARTDATE, RPAD(CHARTTIME, 6, '00') CHARTTIME, CHARTUSEID, PRNTYN, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, INPUSEID, INPDATE, INPTIME, INPUSENAME, USENAME, BASVAL, '섭취배설' AS UNITCLS             ";
                SQL = SQL + ComNum.VBLF + "   SELECT EMRNO, EMRNOHIS, CHARTDATE, RPAD(CHARTTIME, 6, '00') CHARTTIME, MAX(CHARTUSEID) CHARTUSEID, PRNTYN, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, (SUM(ITEMVALUE) || '') ITEMVALUE, MAX(INPUSEID) INPUSEID, MAX(INPDATE) INPDATE, MAX(INPTIME) INPTIME, MAX(INPUSENAME) INPUSENAME , MAX(USENAME) USENAME, BASVAL, '섭취배설' AS UNITCLS";
                SQL = SQL + ComNum.VBLF + "    FROM                                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "        SELECT 0 AS EMRNO                                                                                                                                                                                     ";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "             , (REPLACE(REPLACE((SELECT ITEMVALUE                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "                                  FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                                 WHERE EMRNO = A.EMRNO                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                                   AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                                   AND ITEMCD = 'I0000037490'),'-',''),'/','')) AS CHARTDATE--수혈종료일자                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , REPLACE((SELECT ITEMVALUE                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "                          FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                         WHERE EMRNO = A.EMRNO                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                           AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                           AND ITEMCD = 'I0000037491'),':','')AS CHARTTIME  --수혈종료시간                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , A.CHARTUSEID                                                                                                                                                                                     ";
                SQL = SQL + ComNum.VBLF + "             , A.PRNTYN                                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMCD                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMNO                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , B.ITEMINDEX                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "             , B.ITEMTYPE                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "             , B.ITEMVALUE                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "             , B. INPUSEID                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "             , B.INPDATE                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , B.INPTIME                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , (SELECT MAX(U.USENAME)                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_EMR.AVIEWEMRUSER U                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.USEID = B.INPUSEID) AS INPUSENAME                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "             , (SELECT MAX(U.USENAME)                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_EMR.AVIEWEMRUSER U                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.USEID = A.CHARTUSEID) AS USENAME                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "             , 0 AS BASVAL                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "         INNER JOIN  KOSMOS_EMR.AEMRCHARTROW B                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "            ON A.EMRNO = B.EMRNO                                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "           AND A.EMRNOHIS = B.EMRNOHIS";

                if (p.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "         WHERE A.ACPNO = " + p.acpNo;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         WHERE A.PTNO = '" + p.ptNo + "'";
                }
                SQL = SQL + ComNum.VBLF + "           AND A.FORMNO IN (1965, 3535)                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "           AND B.ITEMCD = 'I0000013528'                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "           AND MEDFRDATE = '" + p.medFrDate + "'";

                SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "    WHERE CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "    GROUP BY CHARTDATE, RPAD(CHARTTIME, 6, '00'), PRNTYN, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, BASVAL";
                SQL = SQL + ComNum.VBLF + ")                                                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO, EMRNOHIS, CHARTDATE, CHARTTIME, CHARTUSEID, PRNTYN, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, INPUSEID, INPDATE, INPTIME, INPUSENAME, USENAME, BASVAL, UNITCLS                              ";
                SQL = SQL + ComNum.VBLF + "  FROM M                                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + " WHERE ITEMNO NOT IN ('I0000030622','I0000030623') --기존 섭취배설 기록 삭제                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "UNION ALL --섭취계산                                                                                                                                                                                                 ";
                SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO , A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "     , '' AS CHARTUSEID                                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "     , 'N' AS PRNTYN                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , 'I0000030622' AS ITEMCD                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , 'I0000030622' AS ITEMNO                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , -1 AS ITEMINDEX                                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "     , 'TEXT' AS ITEMTYPE                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(SUM(A.ITEMVALUE)) AS ITEMVALUE                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPUSEID                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPDATE                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPTIME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPUSENAME                                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "     , '' AS USENAME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , 0 AS BASVAL                                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "     , '섭취배설' AS UNITCLS                                                                                                                                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '01.섭취'                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + @"   AND REGEXP_LIKE(A.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능

                SQL = SQL + ComNum.VBLF + "GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + " UNION ALL -- 베설 계산                                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO , A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "     , '' AS CHARTUSEID                                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "     , 'N' AS PRNTYN                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , 'I0000030623' AS ITEMCD                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , 'I0000030623' AS ITEMNO                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , -1 AS ITEMINDEX                                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "     , 'TEXT' AS ITEMTYPE                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(SUM(A.ITEMVALUE)) AS ITEMVALUE                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPUSEID                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPDATE                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPTIME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , '' AS INPUSENAME                                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "     , '' AS USENAME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "     , 0 AS BASVAL                                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "     , '섭취배설' AS UNITCLS                                                                                                                                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '11.배설'                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + @"   AND REGEXP_LIKE(A.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능

                SQL = SQL + ComNum.VBLF + "GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "ORDER BY CHARTDATE ASC , CHARTTIME ASC , EMRNO, UNITCLS, BASVAL                                                                                                                                                 ";
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
                if (p.medDeptCd.Equals("ER"))
                {
                    //LoadKTAS(SpdView);
                }
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = COL_DATAS; j < SpdView.Columns.Count; j++)
                {
                    //if (SpdView.Cells[3, j].Text.Trim() == "합계")
                    //{
                    //    continue;
                    //}

                    //SaveTime(dt.Rows[i]["CHARTTIME"].ToString().Substring(0, 4));

                    if (SpdView.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            if (SpdView.Cells[3, j].Text.Trim() == "합계")
                            {
                                SpdView.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();

                                SpdView.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                                SpdView.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                                SpdView.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                                SpdView.Cells[SpdView.Rows.Count - 5, j].Text = dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                                if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                                {
                                    SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = false;
                                }
                                else
                                {
                                    SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = false;
                                }

                                for (k = 4; k < SpdView.RowCount - 5; k++)
                                {
                                    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, COL_ITEM].Text.Trim())
                                    {
                                        SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                        if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                                        {
                                            SpdView.Cells[k, j].BackColor = Color.LightCyan;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SpdView.Cells[3, j].Text.Trim() != "합계")
                            {                              
                                SpdView.Cells[3, j].Text = ""; // dt.Rows[i]["USENAME"].ToString().Trim(); //작성자 숨기기 ㅠ.ㅠ

                                SpdView.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                                SpdView.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                                SpdView.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                                SpdView.Cells[SpdView.Rows.Count - 5, j].Text = dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                                SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();

                                //if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                                //{
                                //    SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = false;
                                //}
                                //else
                                //{
                                //    SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = false;
                                //}

                                for (k = 4; k < SpdView.RowCount - 5; k++)
                                {
                                    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, COL_ITEM].Text.Trim())
                                    {
                                       

                                        SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();

                                        #region 정상수치 벗어나면 빨간색 표시
                                        double redVal = VB.Val(SpdView.Cells[k, j].Text);
                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000014815" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text)) //맥박
                                        {
                                            if (redVal < 51 || redVal > 100)
                                            {
                                                SpdView.Cells[k, j].ForeColor = Color.Red;
                                            }
                                        }

                                        //SBP
                                        redVal = VB.Val(SpdView.Cells[k, j].Text);
                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002018" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text))
                                        {
                                            if (redVal < 101 || redVal > 199)
                                            {
                                                SpdView.Cells[k, j].ForeColor = Color.Red;
                                            }
                                        }

                                        //호흡수 RR
                                        redVal = VB.Val(SpdView.Cells[k, j].Text);
                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002009" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text))
                                        {
                                            if (redVal < 9 || redVal > 14)
                                            {
                                                SpdView.Cells[k, j].ForeColor = Color.Red;
                                            }
                                        }

                                        //체온 BT
                                        redVal = VB.Val(SpdView.Cells[k, j].Text);
                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000001811" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text))
                                        {
                                            if (redVal < 36.1 || redVal > 37.4)
                                            {
                                                SpdView.Cells[k, j].ForeColor = Color.Red;
                                            }
                                        }

                                        //의식수준, AVPU SCORE
                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000031260" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text) ||
                                            dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000037778" && !string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text))
                                        {
                                            if (string.IsNullOrWhiteSpace(SpdView.Cells[k, j].Text) == false && SpdView.Cells[k, j].Text.Equals("Alert") == false)
                                            {
                                                SpdView.Cells[k, j].ForeColor = Color.Red;
                                            }
                                        }
                                        #endregion

                                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030580" || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622" || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"
                                            || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000037796" || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000037798" || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000009122"
                                            || dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000022324")
                                        {
                                            SpdView.Cells[k, j].Locked = true;
                                        }
                                        else
                                        {
                                            if (SpdView.Cells[k, j].Text.Trim() != "")
                                            {
                                                SpdView.Cells[k, j].Tag = dt.Rows[i]["INPUSENAME"].ToString().Trim() + " / " + dt.Rows[i]["INPUSEID"].ToString().Trim() + " / " + dt.Rows[i]["INPDATE"].ToString().Trim() + " / " + dt.Rows[i]["INPTIME"].ToString().Trim();

                                                if (clsType.User.IdNumber != dt.Rows[i]["INPUSEID"].ToString().Trim())
                                                {
                                                    SpdView.Cells[k, j].Locked = true;
                                                    //SpdView.Cells[k, j].BackColor = Color.LightPink;
                                                }
                                                else
                                                {
                                                    SpdView.Cells[k, j].Locked = false;
                                                }
                                            }
                                            else
                                            {
                                                SpdView.Cells[k, j].Locked = false;
                                            }
                                        }

                                        if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                                        {
                                            SpdView.Cells[k, j].BackColor = Color.LightCyan;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }

                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            //if (strJOBGB == "IIO")
            //{
            //    LoadEmrChartInfoTot(SpdView, strJOBGB);
            //}
        }

        /// <summary>
        /// Vital 저장된 값을 조회한다(이전내역
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadBeforeDataIVT_ALL(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strChartTime = txtTime.Text.Trim().Replace(":", "");
            if (string.IsNullOrWhiteSpace(strChartTime))
            {
                strChartTime = ssVital_Sheet1.Cells[2, ssVital_Sheet1.ActiveColumnIndex].Text.Trim().Replace(":", "");
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "   AND  B.EMRNOHIS = A.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "    AND BC.BSNSCLS = '" + mstrFormNameGb + "' ";
            SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호' )";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND (A.CHARTDATE || CHARTTIME) = ";
            SQL = SQL + ComNum.VBLF + "       (SELECT MAX(CHARTDATE ||CHARTTIME)";
            SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST ";
            SQL = SQL + ComNum.VBLF + "         WHERE PTNO = A.PTNO ";
            SQL = SQL + ComNum.VBLF + "           AND FORMNO = A.FORMNO";
            SQL = SQL + ComNum.VBLF + "           AND MEDFRDATE = A.MEDFRDATE";
            SQL = SQL + ComNum.VBLF + "           AND (A.CHARTDATE || CHARTTIME) < '" + dtpFrDate.Value.ToString("yyyyMMdd") + strChartTime + "'";
            SQL = SQL + ComNum.VBLF + "       )";
            //SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계'";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.UNITCLS, BC.BASVAL";

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
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = ssVital_Sheet1.ActiveColumnIndex;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                {
                    if (SpdView.Cells[3, j].Text.Trim() == "합계")
                    {
                        SpdView.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();

                        for (k = 4; k < SpdView.RowCount - 5; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, COL_ITEM].Text.Trim())
                            {
                                SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                                {
                                    SpdView.Cells[k, j].BackColor = Color.LightCyan;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (SpdView.Cells[3, j].Text.Trim() != "합계")
                    {
                        SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        for (k = 4; k < SpdView.RowCount - 5; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, COL_ITEM].Text.Trim())
                            {
                                SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                                {
                                    SpdView.Cells[k, j].BackColor = Color.LightCyan;
                                }
                            }
                        }
                    }
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            //if (strJOBGB == "IIO")
            //{
            //    LoadEmrChartInfoTot(SpdView, strJOBGB);
            //}
        }

        /// <summary>
        /// 저장된 데이타를 조회한다
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadEmrChartInfoTot(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "   AND  B.EMRNOHIS = A.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = '" + mstrFormNameGb + "' ";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '임상관찰'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '섭취배설'";
                    break;
                case "IST":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '특수치료'";
                    break;
                case "IBN":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '기본간호'";
                    break;
            }
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID = '합계'";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.BASVAL";
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

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = COL_DATAS; j < SpdView.Columns.Count; j++)
                {
                    if (SpdView.Cells[3, j].Text.Trim() != "합계")
                    {
                        continue;
                    }

                    if (SpdView.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        SpdView.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                        SpdView.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                        SpdView.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        }
                        else
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                        }


                        SpdView.Cells[SpdView.Rows.Count - 5, j].Text = dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = true;
                        }
                        else
                        {
                            if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                            {
                                SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = false;
                            }
                            else
                            {
                                SpdView.Cells[4, j, SpdView.Rows.Count - 6, j].Locked = true;
                            }
                        }

                        for (k = 4; k < SpdView.RowCount - 5; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, COL_ITEM].Text.Trim())
                            {
                                SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                //i = i + 1;
                                //if (i >= dt.Rows.Count) break;
                            }
                        }
                    }
                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData(string strFlag)
        {
            double dblEmrNo = 0;

            SetSpread();

            Cursor.Current = Cursors.WaitCursor;

            if (SaveTimeSet(mSpdView, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                Cursor.Current = Cursors.Default;
                return dblEmrNo;
            }

            int i = 0;
            int j = 0;

            #region //Save CHARTMST && AEMRCHARTROW
            string strError = "";
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            for (i = COL_DATAS; i < mSpdView.Columns.Count; i++)
            {
                string[] arryEMRNO = null;
                string[] arryITEMCD = null;
                string[] arryITEMNO = null;
                string[] arryITEMINDEX = null;
                string[] arryITEMTYPE = null;
                string[] arryITEMVALUE = null;
                string[] arryITEMVALUE1 = null;
                string[] arryDSPSEQ = null;
                string[] arryINPUSEID = null;
                string[] arryINPDATE = null;
                string[] arryINPTIME = null;

                string strVital = "";

                if (mSpdView.Cells[mSpdView.RowCount - 1, i].Text == "Y")
                {
                    for (j = 4; j < mSpdView.RowCount - 5; j++)
                    {
                        if (mSpdView.Cells[j, i].Text.Trim() != "")
                        {
                            strVital = strVital + mSpdView.Cells[j, i].Text.Trim();
                        }
                    }

                    if (strVital == "")
                    {
                        continue;
                    }
                }

                if (mSpdView.Cells[mSpdView.RowCount - 1, i].Text == "Y")
                {
                    string strEmrNoHis = Convert.ToString(VB.Val(mSpdView.Cells[mSpdView.Rows.Count - 5, i].Text.Trim()));
                    string strEmrNo = Convert.ToString(VB.Val(mSpdView.Cells[mSpdView.Rows.Count - 4, i].Text.Trim()));
                    string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// mSpdView.Cells[0, i].Text.Trim().Replace("-", "");
                    string strChartTime = mSpdView.Cells[2, i].Text.Trim().Replace(":", "") + "00";
                    string strCHARTUSEID = "";
                    string strCOMPUSEID = "";
                    string strSAVEGB = "0";
                    string strFORMGB = "0";

                    #region //해당 시간에 저장된 데이타가 있으면 저장 못하도록 한다.
                    DataTable dt = null;
                    string SQL = "";    //Query문
                    string SqlErr = ""; //에러문 받는 변수

                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME, ";
                    SQL = SQL + ComNum.VBLF + "    U.USENAME ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST C";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_EMR.AVIEWEMRUSER U";
                    SQL = SQL + ComNum.VBLF + "    ON C.CHARTUSEID = U.USEID";
                    SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "'";
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTTIME = '" + strChartTime + "'";
                    SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTUSEID <> '합계'";
                    if (VB.Val(strEmrNoHis) > 0)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS <> " + strEmrNoHis;
                    }
                    //else
                    //{
                    //    SQL = SQL + ComNum.VBLF + "    AND C.EMRNO IS NOT NULL ";
                    //}

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return 0;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //시간 : 99:99,  선 작성자 : 홍길동 (9999-99-99 99:99)
                        strError = strError + ComNum.VBLF
                                + "시간 : " + ComFunc.FormatStrToDate(strChartTime, "M")
                                + "    선 작성자 : " + dt.Rows[0]["USENAME"].ToString().Trim()
                                + " (" + ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D")
                                + " " + ComFunc.FormatStrToDate(dt.Rows[0]["CHARTTIME"].ToString().Trim(), "M") + ")";
                        dt.Dispose();
                        dt = null;

                        using (frmEmrVitalSignSaveError frm = new frmEmrVitalSignSaveError(strError))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog(this);
                        }

                        return 0;
                        //break;
                        //continue;
                    }
                    dt.Dispose();
                    dt = null;
                    #endregion 

                    if (strChartTime.Length < 6)
                    {
                        strChartTime = strChartTime.PadRight(6, '0');
                    }

                    if (VB.Val(strEmrNo) > 0)
                    {
                        strCHARTUSEID = mSpdView.Cells[mSpdView.Rows.Count - 3, i].Text.Trim();
                        strCOMPUSEID = mSpdView.Cells[mSpdView.Rows.Count - 3, i].Text.Trim();
                    }
                    else
                    {
                        strCHARTUSEID = clsType.User.IdNumber;
                        strCOMPUSEID = clsType.User.IdNumber;
                    }

                    #region //데이타를 담는다
                    for (j = 4; j < mSpdView.RowCount - 5; j++)
                    {
                        string strITEMCD = "";
                        string[] strItem = null;
                        string strITEMNO = "";
                        string strITEMINDEX = "-1";
                        string strDSPSEQ = "0";
                        string strITEMTYPE = "TEXT";
                        string strITEMVALUE = "";
                        string strITEMVALUE1 = "";
                        string strINPUSEID = "";
                        string strINPDATE = "";
                        string strINPTIME = "";
                        string strTag = "";

                        strITEMCD = mSpdView.Cells[j, COL_ITEM].Text.Trim();

                        if (mSpdView.Cells[j, COL_EXP].Text == "+" || mSpdView.Cells[j, COL_EXP].Text == "-")
                        {
                            continue;
                        }

                        //총섭취량, 총배설량, 혈액
                        if (strITEMCD == "")
                        {
                            continue;
                        }

                        if (strCurDate.To<DateTime>() >= "2020-09-22".To<DateTime>() && (strITEMCD == "I0000030622" || strITEMCD == "I0000030623" || strITEMCD == "I0000022324"))
                        {
                            continue;
                        }

                        strItem = mSpdView.Cells[j, COL_ITEM].Text.Trim().Split('_');
                        strITEMNO = strITEMCD;
                        strITEMINDEX = "-1";

                        //if (strItem.Length > 0)
                        //{
                        //    strITEMNO = strItem[0].Trim();
                        //}
                        //if (strItem.Length > 1)
                        //{
                        //    strITEMINDEX = strItem[1].Trim();
                        //}

                        if (strITEMCD.IndexOf("_") > -1)
                        {
                            strITEMNO = strITEMCD.Split('_')[0];
                            strITEMINDEX = strITEMCD.Split('_')[1];
                        }
                        else
                        {
                            strITEMNO = strITEMCD;
                            strITEMINDEX = "-1";
                        }

                        strDSPSEQ = "0";
                        strITEMTYPE = "TEXT";
                        strITEMVALUE = mSpdView.Cells[j, i].Text.Trim();
                        strITEMVALUE1 = "";

                        if (mSpdView.Cells[j, i].Tag == null)
                        {
                            if (strITEMVALUE != "")
                            {
                                strINPUSEID = clsType.User.IdNumber;
                                strINPDATE = strCurDate;
                                strINPTIME = strCurTime;
                            }
                        }
                        else
                        {
                            if (mSpdView.Cells[j, i].Tag.ToString().Trim() != "")
                            {
                                if (strITEMVALUE != "")
                                {
                                    strTag = mSpdView.Cells[j, i].Tag.ToString().Trim();
                                    strINPUSEID = ComFunc.SptChar(strTag, 1, "/").Trim();
                                    strINPDATE = ComFunc.SptChar(strTag, 2, "/").Trim();
                                    strINPTIME = ComFunc.SptChar(strTag, 3, "/").Trim();
                                }
                            }
                            else
                            {
                                if (strITEMVALUE != "")
                                {
                                    strINPUSEID = clsType.User.IdNumber;
                                    strINPDATE = strCurDate;
                                    strINPTIME = strCurTime;
                                }
                            }
                        }

                        //정맥주입, 총섭취량, 총배설량, 목막투석(연동), 혈당(연동) 은 사용자 정보를 없앤다
                        if (strITEMCD == "I0000030580" || strITEMCD == "I0000030622" || strITEMCD == "I0000030623"
                            || strITEMCD == "I0000037796" || strITEMCD == "I0000037798" || strITEMCD == "I0000009122")
                        {
                            strINPUSEID = "";
                            strINPDATE = strCurDate;
                            strINPTIME = strCurTime;
                        }

                        if (arryEMRNO == null)
                        {
                            arryEMRNO = new string[0];
                            arryITEMCD = new string[0];
                            arryITEMNO = new string[0];
                            arryITEMINDEX = new string[0];
                            arryITEMTYPE = new string[0];
                            arryITEMVALUE = new string[0];
                            arryITEMVALUE1 = new string[0];
                            arryDSPSEQ = new string[0];
                            arryINPUSEID = new string[0];
                            arryINPDATE = new string[0];
                            arryINPTIME = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                        Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                        Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                        Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                        Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                        Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);
                        Array.Resize<string>(ref arryINPUSEID, arryINPUSEID.Length + 1);
                        Array.Resize<string>(ref arryINPDATE, arryINPDATE.Length + 1);
                        Array.Resize<string>(ref arryINPTIME, arryINPTIME.Length + 1);

                        arryEMRNO[arryEMRNO.Length - 1] = "0";
                        arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                        arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                        arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                        arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                        arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE.Replace("'", "`");
                        arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1.Replace("'", "`");
                        arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                        arryINPUSEID[arryEMRNO.Length - 1] = strINPUSEID;
                        arryINPDATE[arryEMRNO.Length - 1] = strINPDATE;
                        arryINPTIME[arryEMRNO.Length - 1] = strINPTIME;
                    }
                    #endregion //데이타를 담는다

                    string strSAVECERT = "1";
                    double dblEmrNoNew = clsEmrQuery.SaveFlowChartEx(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime,
                        strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                        arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1,
                        arryINPUSEID, arryINPDATE, arryINPTIME
                        );
                    if (dblEmrNoNew == 0)
                    {
                        //에러임
                    }
                    else
                    {
                        #region //전자인증 하기
                        if (strSAVECERT == "1")
                        {
                            if (System.Diagnostics.Debugger.IsAttached == false)
                            {
                                if (dblEmrNoNew > 0)
                                {
                                    if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                                    {
                                        clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew);
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }
            }
            #endregion //CHARTMST && AEMRCHARTROW

            Cursor.Current = Cursors.Default;

            if (strError != "")
            {
                using (frmEmrVitalSignSaveError frm = new frmEmrVitalSignSaveError(strError))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }

            return dblEmrNo;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData(string strFlag)
        {
            double dblEmrNo = 0;

            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData(string strUseId = "")
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (strUseId != "합계")
            {
                if (VB.Val(mstrEmrNo) != 0)
                {
                    //if (clsEmrQuery.IsChangeAuthCopy(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return false;
                }
                strUseId = clsType.User.IdNumber;
            }

            if (clsXML.gDeleteEmrXmlNotAuth(clsDB.DbCon, mstrEmrNo, strUseId) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }

        #endregion

        #region 생성자
        public frmEmrVitalSign()
        {
            InitializeComponent();
        }

        public frmEmrVitalSign(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmEmrVitalSign(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        public frmEmrVitalSign(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm, Panel panParent)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        #endregion //생성자

        #region 폼 이벤트

        private void frmEmrVitalSign_Load(object sender, EventArgs e)
        {
            #region 버튼 권한 설정
            btnSaveAll.Visible = mstrMode.Equals("W");
            btnBST.Visible = btnSaveAll.Visible;
            btnInsertTime.Visible = btnSaveAll.Visible;
            mbtnNrHistory.Visible = btnSaveAll.Visible;
            mbtnDeleteAll.Visible = btnSaveAll.Visible;
            btnInsert.Visible = btnSaveAll.Visible;
            btnRinger.Visible = btnSaveAll.Visible;
            btnSaveItemIVT.Visible = btnSaveAll.Visible;
            btnSaveItemIIO.Visible = btnSaveAll.Visible;
            mbtnSaveItemIST.Visible = btnSaveAll.Visible;
            mbtnSaveItemIBN.Visible = btnSaveAll.Visible;

            btnSaveAll.Enabled = clsType.User.AuAWRITE.Equals("1");
            mbtnDeleteAll.Enabled = clsType.User.AuAWRITE.Equals("1");

            btnSaveWard.Enabled = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
            //btnRegBaseValue.Enabled = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
         

            btnInterface.Visible = mstrMode.Equals("W") && (mstrFormNo.Equals("2201") || mstrFormNo.Equals("1969") || p.ptNo.Equals("81000005"));
            btnO2.Visible = mstrFormNo.Equals("2201") == false && mstrFormNo.Equals("1969") == false && mstrMode.Equals("W");
            btnHDView.Visible = btnInterface.Visible;

            //btnInterface.Visible = mstrFormNo.Equals("2201") || p.ward.Equals("ER") || p.ward.Equals("33") || p.ward.Equals("35"); //인공신장실만 사용.
            #endregion

            btnMemo.Visible = mstrMode.Equals("W") && (p.ward.Equals("33") || p.ward.Equals("35"));       
            isFormLoad = true;

            BoldFont = new Font("굴림", 10, FontStyle.Bold);
            Notefont = new Font("굴림", 10);
            keysItemMax = new Dictionary<string, int>();

            panGraph.Dock = DockStyle.Top;
            panGraph.Height = 600; //300
            panSplitH.Dock = DockStyle.Top;
            panVital.Dock = DockStyle.Fill;
            panGraph.Visible = false;
            panTot.Visible = false;
            panTotHead.Visible = false;

            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.ColumnCount = 0;
            ssVital_Sheet1.FrozenColumnCount = COL_INAME + 1;

            ssIoTot_Sheet1.RowCount = 0;
            ssIoTot_Sheet1.ColumnCount = 0;
            ssIoTot_Sheet1.FrozenColumnCount = COL_INAME + 1;

            panOrder.Height = 33;

            SetDutyTime();

            mSpd = ssVital;
            mSpdView = ssVital_Sheet1;

            SetPatInfo();

            if (p.acpNo.Equals("0") && p.ptNo.Substring(0, 4).Equals("8100") == false && (p.medDeptCd.Equals("TO") || p.medDeptCd.Equals("HR")))
            {
                clsImgcvt.NEW_PohangTreatInterface(clsDB.DbCon, this, p.ptNo);
                
                p = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, p.ptNo, p.inOutCls, p.medFrDate, p.medDeptCd);
                if (p.acpNo.Equals("0"))
                {
                    ComFunc.MsgBoxEx(this, "환자 접수정보가 잘못되었습니다\r\n환자리스트에서 다시 클릭 해주세요.");
                }
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (p.medEndDate == "" || p.medEndDate == "99991231")
                {
                    dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                }
                else
                {
                    string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                    dtpFrDate.Value = DateTime.ParseExact(string.IsNullOrWhiteSpace(p.medEndDate) || p.medEndDate.Equals(strCurDate) ? strCurDate : p.medFrDate, "yyyyMMdd", null);
                }
            }
            else
            {
                dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }

            #region 주사처방 조회 및 아이템 가져오게
            CheckItemExists(dtpFrDate.Value.ToString("yyyyMMdd"));
            pGetInjectOrderInfo();         
            #endregion
            GetSetVITALSET(dtpFrDate.Value.ToString("yyyyMMdd"));

            #region 수혈 관련 아이템 저장
            if (mstrMode.Equals("W") && FormPatInfoFunc.Set_FormPatInfo_IsBlood(clsDB.DbCon, p.ptNo, dtpFrDate.Value.ToString("yyyy-MM-dd")))
            {
                btnBlood.Visible = true;
                clsEmrQuery.CheckItemExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"), "I0000022324");
                clsEmrQuery.CheckItemExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"), "I0000030622");
                clsEmrQuery.CheckBloodTimeExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"));
            }
            else
            {
                btnBlood.Visible = false;
            }
            #endregion

            mJOBGB = "IVT";

            LoadJobData();
        }

        private void frmEmrVitalSign_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (ssVital_Sheet1.ActiveColumnIndex >= COL_DATAS)
                {
                    ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, ssVital_Sheet1.ActiveColumnIndex].Text = "Y";
                }
            }
        }

        private void frmEmrVitalSign_Resize(object sender, EventArgs e)
        {
            pSubFormResize();
        }

        private void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            //frm.Dock = DockStyle.Fill;
            frm.Show();

        }

        private void pSubFormResize()
        {
            try
            {

            }
            catch
            {

            }
        }

        #endregion

        #region 공통모듈

        /// <summary>
        /// 폼별 초기화
        /// </summary>
        private void pClearFormExcept()
        {
            SetDefault();
        }

        /// <summary>
        /// 최초 설정된 값을 불러서 세팅한다
        /// </summary>
        private void SetDefault()
        {
            panTot.Visible = false;
            panTotHead.Visible = false;

            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.ColumnCount = 0;

            ssIoTot_Sheet1.RowCount = 0;
            ssIoTot_Sheet1.ColumnCount = 0;

            btnViewInpUser.Text = "작성자 보기";

            LoadJobData();
        }


        /// <summary>
        /// 병동별 Duty 설정
        /// </summary>
        private void SetDutyTime()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = 'DUTYTIME'";
            
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
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["BASCD"].ToString().Trim() == "DAY")
                    {
                        mstrDayTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["BASCD"].ToString().Trim() == "EVE")
                    {
                        mstrEveTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["BASCD"].ToString().Trim() == "NIGHT")
                    {
                        mstrNightTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["BASCD"].ToString().Trim() == "NIGHTAM")
                    {
                        mstrNightTimeAm = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                }
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 차트에는 시간이 있는데 화면에 시간 생성이 안되어있을때 생성
        /// </summary>ㅌ
        private void SetChartTimeCreate()
        {
            if (p == null) return;

            DataTable dt = null;
            string SQL    = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            string strCurTime = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");

            SQL = "SELECT SUBSTR(CHARTTIME, 0, 4) CHARTTIME ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBVITALTIME B";
            SQL = SQL + ComNum.VBLF + "     ON A.ACPNO = B.ACPNO";
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = B.CHARTDATE";
            SQL = SQL + ComNum.VBLF + "    AND B.TIMEVALUE = SUBSTR(A.CHARTTIME, 0, 4)";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.TIMEVALUE IS NULL";
            SQL = SQL + ComNum.VBLF + "ORDER BY CHARTTIME";

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
                clsDB.setBeginTran(clsDB.DbCon);
                try
                {
                    int RowAffected = 0;

                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, CHARTDATE, JOBGB, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT DISTINCT A.FORMNO";
                    SQL = SQL + ComNum.VBLF + "     , A.ACPNO";
                    SQL = SQL + ComNum.VBLF + "     , A.CHARTDATE";
                    SQL = SQL + ComNum.VBLF + "     , 'IVT'";
                    SQL = SQL + ComNum.VBLF + "     , SUBSTR(A.CHARTTIME, 0, 4)";
                    SQL = SQL + ComNum.VBLF + "     , '0'";
                    SQL = SQL + ComNum.VBLF + "     , TO_CHAR(SYSDATE, 'YYYYMMDD')";
                    SQL = SQL + ComNum.VBLF + "     , TO_CHAR(SYSDATE, 'HH24MISS')";
                    SQL = SQL + ComNum.VBLF + "     , '" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + " WHERE FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + p.ptNo + "'";
                    SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "   (";
                    SQL = SQL + ComNum.VBLF + "     SELECT 1";
                    SQL = SQL + ComNum.VBLF + "       FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
                    SQL = SQL + ComNum.VBLF + "      WHERE FORMNO = A.FORMNO";
                    SQL = SQL + ComNum.VBLF + "        AND ACPNO = A.ACPNO";
                    SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = A.CHARTDATE";
                    SQL = SQL + ComNum.VBLF + "        AND TIMEVALUE = SUBSTR(A.CHARTTIME, 0, 4)";
                    SQL = SQL + ComNum.VBLF + "   )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
                catch(Exception ex)
                {
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
            }

            dt.Dispose();
        }

        /// <summary>
        /// 환자 정보 세팅
        /// </summary>
        private void SetPatInfo()
        {
            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            if (p == null) return;

            if (mstrMode.Equals("W") && mstrFormNo.Equals("3150") &&  (p.ward.Equals("33") || p.ward.Equals("35")))
            {
                if (frmIcuMemo != null)
                {
                    frmIcuMemo.Dispose();
                    frmIcuMemo = null;
                }

                Screen screen = Screen.FromControl(this);
                frmIcuMemo = new frmEmrBaseNurseMemo(p);
                frmIcuMemo.Location = new Point(screen.WorkingArea.Right - frmIcuMemo.Width, 300);
                frmIcuMemo.StartPosition = FormStartPosition.Manual;
                frmIcuMemo.FormClosed += FrmIcuMemo_FormClosed;
                frmIcuMemo.Show(this);
            }
        }

        /// <summary>
        /// 당일 AEMRBVITALSET에 데이타가 없을 경우 전날 데이타를 읽어서 저장한다
        /// </summary>
        /// <param name="strChartData"></param>
        private void GetSetVITALSET(string strChartData)
        {
            if (mstrFormNo == "2135" || mstrFormNo == "1935" || mstrFormNo == "2431" || mstrFormNo == "1969" || mstrFormNo == "2201") //회복실, Angio, 진정 환자 평가, 응급실 SPECIAL WATCH RECORD, 인공신장실 V/S
            {
                return;
            }

            if (p == null) return;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT B.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET B";
            SQL = SQL + ComNum.VBLF + "WHERE B.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND B.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND B.CHARTDATE = '" + strChartData + "'";
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
                dt.Dispose();
                dt = null;
                return;
            }
            dt.Dispose();
            dt = null;


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     A.FORMNO, A.ACPNO, A.PTNO, ";
                SQL = SQL + ComNum.VBLF + "     '" + strChartData + "' AS CHARTDATE,  ";
                SQL = SQL + ComNum.VBLF + "     A.JOBGB, A.ITEMCD, A.WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "A.WRITETIME, A.WRITEUSEID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "     AND A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "     AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN') ";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE = (SELECT MAX(B.CHARTDATE) AS CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET B";
                SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                                             AND B.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "                                             AND B.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN') ";
                SQL = SQL + ComNum.VBLF + "                                             AND B.CHARTDATE <= '" + strChartData + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 선택된 스프래드 설정
        /// </summary>
        private void SetSpread()
        {
            mSpd = ssVital;
            mSpdView = ssVital_Sheet1;
        }

        /// <summary>
        /// 그룹별 아이템 세팅
        /// </summary>
        /// <param name="strFormNameGb"></param>
        private void SetItem(string strFormNameGb)
        {
            //pSaveData("1");

            using (frmVitalSet frmVitalSetX = new frmVitalSet(p, strFormNameGb, mstrFormNo, mstrUpdateNo))
            {
                frmVitalSetX.ShowDialog(this);
            }

            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.ColumnCount = 0;

            LoadJobData();

            ColVisible();
        }

        /// <summary>
        /// 임상관찰 기록지를 조회해서 화면에 뿌린다
        /// 당일 아이템 체크 추가
        /// </summary>
        private void LoadJobData()
        {
            GetSetTodayItem();

            LoadJobDataSub();
        }

        /// <summary>
        /// 임상관찰 기록지를 조회해서 화면에 뿌린다
        /// </summary>
        private void LoadJobDataSub()
        {
            if (clsEmrQueryEtc.GetSetTotInOut(clsDB.DbCon, p.acpNo.ToString(), p.ptNo.ToString(), mstrFormNo, "IIO", dtpFrDate.Value.ToString("yyyyMMdd")) == false)
            {

            }
            SetDefaultDataIVT_ALL(ssVital_Sheet1, "IVT");
            LoadData(ssVital_Sheet1, "IVT");
            SetWorkDuty();
        }

        /// <summary>
        /// 해당일자의 아이템을 만든다
        /// </summary>
        private void GetSetTodayItem()
        {
            if (p == null) return;

            //1.일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
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
                dt.Dispose();
                dt = null;
                return;
            }

            dt.Dispose();
            dt = null;

            //2.없으면 이전 날짜 가지고 와서 세팅

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(AA.CHARTDATE) AS CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET AA";
            SQL = SQL + ComNum.VBLF + "                                        WHERE AA.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "                                            AND AA.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "                                            AND AA.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "                                            AND AA.CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";
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
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     A.FORMNO, A.ACPNO, A.PTNO, ";
                    SQL = SQL + ComNum.VBLF + "     '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' AS CHARTDATE, ";
                    SQL = SQL + ComNum.VBLF + "     A.JOBGB, A.ITEMCD, A.WRITEDATE, A.WRITETIME, A.WRITEUSEID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                    SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
                    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설''특수치료', '기본간호')";
                    SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(AA.CHARTDATE) AS CHARTDATE ";
                    SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET AA";
                    SQL = SQL + ComNum.VBLF + "                                        WHERE AA.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND AA.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND AA.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "                                            AND AA.CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
            dt.Dispose();
            dt = null;

            //3.없으면 기본값을 세팅

        }

        /// <summary>
        /// Vital 값 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void SetDefaultDataIVT_ALL(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            Cursor.Current = Cursors.WaitCursor;

            SetTopRow(ssVital_Sheet1);

            //정맥주입 로우 찾는용도
            int VeinRow = -1;
            //혈액 로우 
            int BloodRow = -1;

            SetDefaultDataIVT_Sub(ssVital_Sheet1, "IVT", ref VeinRow, ref BloodRow); //임상관찰
            SetDefaultDataIVT_Sub(ssVital_Sheet1, "IIO", ref VeinRow, ref BloodRow); //섭취배설
            SetDefaultDataIVT_Sub(ssVital_Sheet1, "IST", ref VeinRow, ref BloodRow); //특수치료
            SetDefaultDataIVT_Sub(ssVital_Sheet1, "IBN", ref VeinRow, ref BloodRow); //기본간호

            SetButtonRow(ssVital_Sheet1);

            SetChartTimeCreate();
            SetTimeSet(ssVital_Sheet1, strJOBGB);

            SetJobGroupSet(ssVital_Sheet1);

            if (VeinRow != -1)
            {
                SpdView.Cells[VeinRow, COL_ITEM, VeinRow, SpdView.ColumnCount - 1].Locked = true;
            }

            if (BloodRow != -1)
            {
                SpdView.Cells[BloodRow, COL_ITEM, BloodRow, SpdView.ColumnCount - 1].Locked = true;
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 작업 그룹 코드 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void SetJobGroupSet(FarPoint.Win.Spread.SheetView SpdView)
        {
            for (int i = 0; i < SpdView.RowCount; i++)
            {
                if (SpdView.Cells[i, COL_EXP].Text.Trim() == "+" || SpdView.Cells[i, COL_EXP].Text == "-")
                {
                    SpdView.Cells[i, COL_GNAME].BackColor = Color.LightBlue;
                    SpdView.AddSpanCell(i, 3, 1, SpdView.ColumnCount - 3);
                }
            }

            int intRowIVT = 0;
            int intRowIIO = 0;
            int intRowIST = 0;
            int intRowIBN = 0;

            int intRowIVT_S = 0;
            int intRowIVT_E = 0;

            int intRowIIO_S = 0;
            int intRowIIO_E = 0;

            int intRowIST_S = 0;
            int intRowIST_E = 0;

            int intRowIBN_S = 0;
            int intRowIBN_E = 0;

            for (int i = 4; i < ssVital_Sheet1.Rows.Count - 5; i++)
            {
                if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == "IVT")
                {
                    if (intRowIVT == 0)
                    {
                        intRowIVT_S = i;
                    }
                    else
                    {
                        intRowIVT_E = i;
                    }
                    intRowIVT = intRowIVT + 1;
                }
                else if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == "IIO")
                {
                    if (intRowIIO == 0)
                    {
                        intRowIIO_S = i;
                    }
                    else
                    {
                        intRowIIO_E = i;
                    }
                    intRowIIO = intRowIIO + 1;
                }
                else if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == "IST")
                {
                    if (intRowIST == 0)
                    {
                        intRowIST_S = i;
                    }
                    else
                    {
                        intRowIST_E = i;
                    }
                    intRowIST = intRowIST + 1;
                }
                else if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == "IBN")
                {
                    if (intRowIBN == 0)
                    {
                        intRowIBN_S = i;
                    }
                    else
                    {
                        intRowIBN_E = i;
                    }
                    intRowIBN = intRowIBN + 1;
                }
            }

            if (intRowIVT_S >= 4) SpdView.AddSpanCell(intRowIVT_S + 1, 2, intRowIVT_E - intRowIVT_S, 1);
            if (intRowIIO_S >= 4) SpdView.AddSpanCell(intRowIIO_S + 1, 2, intRowIIO_E - intRowIIO_S, 1);
            if (intRowIST_S >= 4) SpdView.AddSpanCell(intRowIST_S + 1, 2, intRowIST_E - intRowIST_S, 1);
            if (intRowIBN_S >= 4) SpdView.AddSpanCell(intRowIBN_S + 1, 2, intRowIBN_E - intRowIBN_S, 1);
        }

        /// <summary>
        /// Vital 서브 그룹 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void SetDefaultDataIVT_Sub(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB, ref int VeinRow, ref int BloodRow)
        {
            if (p == null) return;
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBASEXNAME = "";
            int intS = 0;

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE, B.BASVAL ";
            SQL = SQL + ComNum.VBLF + ", (                                                                                                            ";
            SQL = SQL + ComNum.VBLF + " SELECT LISTAGG(S.SUNAMEK || ' ' || QTY || ' (' || H.EMP_NM  || ')', '\r\n') WITHIN GROUP(ORDER BY O.ORDERNO)  ";
            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER O                                                                     ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_OCS.OCS_IORDER O2                                                                       ";
            SQL = SQL + ComNum.VBLF + "        ON O.ORDERNO  = O2.ORDERNO                                                                             ";
            SQL = SQL + ComNum.VBLF + "       AND O2.PTNO  = '" + p.ptNo + "'                                                                         ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_PMPA.BAS_SUN S                                                                          ";
            SQL = SQL + ComNum.VBLF + "        ON S.SUNEXT = O2.SUCODE                                                                                ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_ERP.HR_EMP_BASIS H                                                                      ";
            SQL = SQL + ComNum.VBLF + "        ON H.EMP_ID = O.WRITEUSEID                                                                             ";
            SQL = SQL + ComNum.VBLF + "   WHERE O.EMRNO    = " + dtpFrDate.Value.ToString("yyyyMMdd");
            SQL = SQL + ComNum.VBLF + "     AND O.ITEMCD   = A.ITEMCD                                                                                 ";
            SQL = SQL + ComNum.VBLF + "     AND O.GBSTATUS = ' '                                                                                      ";
            SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                                                     ";
            //SQL = SQL + ComNum.VBLF + "     , CASE WHEN EXISTS";
            //SQL = SQL + ComNum.VBLF + "     (";
            //SQL = SQL + ComNum.VBLF + "     SELECT 1";
            //SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER O";
            //SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_OCS.OCS_IORDER O2";
            //SQL = SQL + ComNum.VBLF + "            ON O.ORDERNO  = O2.ORDERNO ";
            //SQL = SQL + ComNum.VBLF + "           AND O2.PTNO  = '" + p.ptNo + "'";
            //SQL = SQL + ComNum.VBLF + "      WHERE O.EMRNO    = " + dtpFrDate.Value.ToString("yyyyMMdd");
            //SQL = SQL + ComNum.VBLF + "        AND O.ITEMCD   = A.ITEMCD";
            //SQL = SQL + ComNum.VBLF + "        AND O.GBSTATUS = ' '";
            //SQL = SQL + ComNum.VBLF + "     ) THEN 1 END ORDER_CHECK";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '임상관찰'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설'";
                    break;
                case "IST":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '특수치료'";
                    break;
                case "IBN":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '기본간호'";
                    break;
            }
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '임상관찰그룹'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '섭취배설그룹'";
                    break;
                case "IST":
                    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '특수치료그룹'";
                    break;
                case "IBN":
                    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '기본간호그룹'";
                    break;
            }
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND A.JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";

            //SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) AS CHARTDATE ";
            //SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
            //SQL = SQL + ComNum.VBLF + "                                        WHERE ACPNO = " + p.acpNo;
            //SQL = SQL + ComNum.VBLF + "                                            AND JOBGB = '" + strJOBGB + "'";
            //SQL = SQL + ComNum.VBLF + "                                            AND CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";
            //SQL = SQL + ComNum.VBLF + "ORDER BY B.BASEXNAME, B.DISSEQNO";
            //SQL = SQL + ComNum.VBLF + "ORDER BY B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

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
                #region //title
                SpdView.RowCount = SpdView.RowCount + 1;

                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                SpdView.Cells[SpdView.RowCount - 1, COL_JOB].Text = strJOBGB;

                SpdView.Cells[SpdView.RowCount - 1, COL_ITEM].Text = "";
                SpdView.Cells[SpdView.RowCount - 1, COL_EXP].Text = "-";
                switch (strJOBGB)
                {
                    case "IVT":
                        SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].Text = "임상관찰";
                        break;
                    case "IIO":
                        SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].Text = "섭취배설";
                        break;
                    case "IST":
                        SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].Text = "특수치료";
                        break;
                    case "IBN":
                        SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].Text = "기본간호";
                        break;
                }

                SpdView.SetRowHeight(SpdView.RowCount - 1, ComNum.SPDROWHT);
                SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].BackColor = Color.LightBlue;
                SpdView.Cells[SpdView.RowCount - 1, COL_INAME].BackColor = Color.LightBlue;
                SpdView.AddSpanCell(SpdView.RowCount - 1, 3, 1, 2);
                #endregion //title

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SpdView.RowCount = SpdView.RowCount + 1;

                    if (keysItemMax.ContainsKey(dt.Rows[i]["BASCD"].ToString().Trim()) == false)
                    {
                        keysItemMax.Add(dt.Rows[i]["BASCD"].ToString().Trim(), (int)VB.Val(dt.Rows[i]["BASVAL"].ToString().Trim()));
                    }

                    #region 처방 체크
                    if (dt.Rows[i]["NOTE"].ToString().NotEmpty())
                    {
                        SpdView.Cells[SpdView.RowCount - 1, COL_INAME].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                        SpdView.Cells[SpdView.RowCount - 1, COL_INAME].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                        SpdView.Cells[SpdView.RowCount - 1, COL_INAME].NoteIndicatorColor = Color.FromArgb(214, 230, 245);
                        SpdView.Cells[SpdView.RowCount - 1, COL_INAME].NoteIndicatorSize = new Size(20, 20);
                        SpdView.Cells[SpdView.RowCount - 1, COL_INAME].Note = dt.Rows[i]["NOTE"].ToString().Trim();

                        FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo2 = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                        //nsinfo.BackColor = Color.Red;
                        nsinfo2.Font = Notefont;
                        nsinfo2.ForeColor = Color.Black;
                        nsinfo2.Width = 500; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                        nsinfo2.Height = 200; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                        nsinfo2.ShapeOutlineColor = Color.Red;
                        nsinfo2.ShapeOutlineThickness = 1;
                        nsinfo2.ShadowOffsetX = 3;
                        nsinfo2.ShadowOffsetY = 3;
                        SpdView.SetStickyNoteStyleInfo(SpdView.RowCount - 1, COL_INAME, nsinfo2);
                    }
                    #endregion

                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    //정맥주입
                    if (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030580")
                    {
                        VeinRow = SpdView.RowCount - 1;
                    }

                    //혈액
                    if (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000022324")
                    {
                        BloodRow = SpdView.RowCount - 1;
                    }

                    SpdView.Cells[SpdView.RowCount - 1, COL_JOB].Text = strJOBGB;

                    SpdView.Cells[SpdView.RowCount - 1, COL_ITEM].Text = dt.Rows[i]["BASCD"].ToString().Trim();

                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            SpdView.AddSpanCell(intS, 3, SpdView.RowCount - 1 - intS, 1);
                        }
                        intS = SpdView.RowCount - 1;
                    }


                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    SpdView.Cells[SpdView.RowCount - 1, COL_INAME].Text = dt.Rows[i]["BASNAME"].ToString().Trim();


                    if (strJOBGB == "IIO")
                    {
                        if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                        {
                            SpdView.Cells[SpdView.RowCount - 1, COL_GNAME].BackColor = Color.LightCyan;
                            SpdView.Cells[SpdView.RowCount - 1, COL_INAME].BackColor = Color.LightCyan;
                        }
                    }

                    //SpdView.SetColumnWidth(2, Convert.ToInt32(SpdView.GetPreferredColumnWidth(i)) + 4);
                    SpdView.SetRowHeight(SpdView.RowCount - 1, ComNum.SPDROWHT);
                }

                SpdView.AddSpanCell(intS, 3, SpdView.RowCount - intS, 1);
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 해드 값 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void SetTopRow(FarPoint.Win.Spread.SheetView SpdView)
        {
            InitSpdSet(SpdView);

            clsSpread.SetTypeAndValue(SpdView, 0, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 2, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 3, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            clsSpread.SetTypeAndValue(SpdView, 0, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성일자", false);
            SpdView.Cells[0, 0, 0, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(0, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "Duty", false);
            SpdView.Cells[1, 0, 1, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(1, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 2, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성시간", false);
            SpdView.Cells[2, 0, 2, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(2, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 3, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "구   분", false);
            SpdView.Cells[3, 0, 3, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(3, 3, 1, 2);
        }

        /// <summary>
        /// 스프래드 기초 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void InitSpdSet(FarPoint.Win.Spread.SheetView SpdView)
        {
            SpdView.RowCount = 0;
            SpdView.ColumnCount = 0;

            SpdView.RowCount = mintTRow;
            SpdView.ColumnCount = mintTCol;
            SpdView.SetRowHeight(-1, ComNum.SPDROWHT);
            //SpdView.SetColumnWidth(-1, mintColW_I);
            SpdView.SetColumnWidth(COL_JOB, 60);
            SpdView.SetColumnWidth(COL_ITEM, 60);
            SpdView.SetColumnWidth(COL_EXP, 18);
            SpdView.SetColumnWidth(COL_GNAME, 80);
            SpdView.SetColumnWidth(COL_INAME, 160);

            SpdView.Columns[COL_JOB].Visible = false;
            SpdView.Columns[COL_ITEM].Visible = false;

            SpdView.Rows[0].Visible = false; //작성일자
            SpdView.Rows[1].Visible = false; //Duty
            SpdView.Rows[3].Visible = true; //작성자
        }

        /// <summary>
        /// 아래 값 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void SetButtonRow(FarPoint.Win.Spread.SheetView SpdView)
        {
            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "EMRNOHIS", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "EMRNO", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "USEID", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "PRNTYN", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "CHANG", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.Rows[SpdView.RowCount - 5].Visible = false;
            SpdView.Rows[SpdView.RowCount - 4].Visible = false;
            SpdView.Rows[SpdView.RowCount - 3].Visible = false;
            SpdView.Rows[SpdView.RowCount - 2].Visible = false;
            SpdView.Rows[SpdView.RowCount - 1].Visible = false;
        }

        /// <summary>
        /// 시간 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void SetTimeSet(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            if (p == null) return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;

            string strCurTime = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");

            SQL = "";
            SQL = "SELECT TIMEVALUE, SUBGB ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY TO_NUMBER(TIMEVALUE)";
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
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SpdView.Columns.Count = SpdView.Columns.Count + 1;
                    SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                    clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate(dt.Rows[i]["TIMEVALUE"].ToString().Trim(), "M"), false);
                    SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    if (dt.Rows[i]["SUBGB"].ToString().Trim() == "1")
                    {
                        SpdView.Cells[3, SpdView.Columns.Count - 1].Text = "합계";
                    }

                    for (j = 4; j < SpdView.RowCount - 5; j++)
                    {
                        clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    }
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 5, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                }
                dt.Dispose();
                dt = null;

                SetCellMax();
                return;
            }
            dt.Dispose();
            dt = null;

            if (mstrFormNo == "2135" || mstrFormNo == "1935" || mstrFormNo == "2431" || mstrFormNo == "1969" || mstrFormNo == "2201") //회복실, Angio, 진정 환자 평가, 응급실 SPECIAL WATCH RECORD
            {
                return;
            }

            #region //기본 시간 설정
            if (p.medDeptCd == "ER")
            {
                #region 응급실 시간 => 현재시간
                SpdView.Columns.Count = SpdView.Columns.Count + 1;
                SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_C, strCurTime, false);
                SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;

                for (j = 4; j < SpdView.RowCount - 5; j++)
                {
                    clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                }
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 5, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                #endregion
            }
            else if (p.ward == "33" || p.ward == "35" || p.ward == "40" || p.ward == "73" || p.ward == "75")
            {
                for (i = 0; i < 25; i++)
                {
                    SpdView.Columns.Count = SpdView.Columns.Count + 1;
                    SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                    clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    if (i == 24)
                    {
                        clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "23:59", false);
                        SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    }
                    else
                    {
                        clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.SetAutoZero(i.ToString().Trim(), 2) + ":00", false);
                        SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    }

                    clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;

                    for (j = 4; j < SpdView.RowCount - 5; j++)
                    {
                        clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    }
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 5, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                }
            }
            else
            {
                for (i = 1; i <= 23; i += 2)
                {
                    SpdView.Columns.Count = SpdView.Columns.Count + 1;
                    SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                    clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.SetAutoZero(i.ToString().Trim(), 2) + ":00", false);
                    SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;

                    for (j = 4; j < SpdView.RowCount - 5; j++)
                    {
                        clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    }
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 5, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                }

            }

            #endregion //기본 시간 설정

            #region 기초코드 최대값 지정
            SetCellMax();
            #endregion

            if (SaveTimeSet(mSpdView, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }
        }

        /// <summary>
        /// 기초코드 최대값 지정
        /// </summary>
        private void SetCellMax()
        {
            #region 기초코드 최대값 지정
            for (int i = 4; i < ssVital_Sheet1.RowCount; i++)
            {
                int Val = 0;
                //아이템 값이 있으면 out 변수로 최대값을 받아오고 그 값이 0 보다 크면
                if (keysItemMax.TryGetValue(ssVital_Sheet1.Cells[i, COL_ITEM].Text.Trim(), out Val) && Val > 0)
                {
                    //텍스트 셀 생성 후 전체 입력 셀에 자릿수 지정.
                    TextCellType textCellType = new TextCellType();
                    textCellType.MaxLength = Val;
                    ssVital_Sheet1.Cells[i, COL_DATAS, i, ssVital_Sheet1.ColumnCount - 1].CellType = textCellType;
                }
            }
            #endregion
        }

        /// <summary>
        /// 시간 삽입
        /// </summary>
        /// <param name="strTime"></param>
        private void SaveTime(string strTime)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(strTime.Replace(":", "")));

            SetSpread();

            if (intTime <= 0 && strTime.Equals("00:00") == false) return;
            if (intTime > 2400) return;

            for (i = intSelCol; i < mSpdView.ColumnCount; i++)
            {
                if (mSpdView.Cells[2, i].Text.Trim() == strTime)
                {
                    //이미 추가되어있는 시간.
                    return;
                }
            }

            if (mSpdView.ColumnCount > 5 && intTime < Convert.ToInt32(VB.Val(mSpdView.Cells[2, COL_DATAS].Text.Replace(":", ""))))
            {
                intSelCol = COL_DATAS;
                mSpdView.AddColumns(intSelCol, 1);
            }
            else
            {
                for (i = COL_DATAS; i < mSpdView.Columns.Count; i++)
                {
                    if (intTime > Convert.ToInt32(VB.Val(mSpdView.Cells[2, i].Text.Replace(":", ""))))
                    {
                        if (i + 1 == mSpdView.Columns.Count)
                        {
                            break;
                        }
                        else
                        {
                            if (intTime < Convert.ToInt32(VB.Val(mSpdView.Cells[2, i + 1].Text.Replace(":", ""))))
                            {
                                intSelCol = i;
                                break;
                            }
                        }
                    }
                }
                if (intSelCol == 0)
                {
                    mSpdView.Columns.Count = mSpdView.Columns.Count + 1;
                    intSelCol = mSpdView.Columns.Count - 1;
                }
                else
                {
                    intSelCol = intSelCol + 1;
                    mSpdView.AddColumns(intSelCol, 1);
                }
            }

            mSpdView.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(mSpdView, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(mSpdView, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(mSpdView, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[2, intSelCol].BackColor = Color.LightBlue;
            mSpdView.Cells[2, intSelCol].Text = strTime.Trim();
            clsSpread.SetTypeAndValue(mSpdView, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[3, intSelCol].BackColor = Color.LightBlue;

            for (j = 4; j < mSpdView.RowCount - 5; j++)
            {
                clsSpread.SetTypeAndValue(mSpdView, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
            }
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 5, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            if (clsEmrQueryEtc.CheckHCBuse(clsType.User.BuseCode) == true)
            {
                for (k = 0; k < mSpdView.Rows.Count; k++)
                {
                    if (mSpdView.Cells[k, 1].Text == "I0000037575")
                    {
                        mSpdView.Cells[k, intSelCol].Text = "Rt Arm";
                        break;
                    }
                }
            }

            if (SaveTimeSet(mSpdView, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }
        }

        /// <summary>
        /// 칼럼 보이기 설정
        /// </summary>
        private void ColVisible()
        {
            if (optAll.Checked == true)
            {
                VisibleSetTrue();
            }
            else if (optDay.Checked == true)
            {
                VisibleSet(mstrDayTime);
            }
            else if (optEve.Checked == true)
            {
                VisibleSet(mstrEveTime);
            }
            else if (optNight.Checked == true)
            {
                VisibleSet(mstrNightTime);
            }
            else if (optNightAm.Checked == true)
            {
                VisibleSet(mstrNightTimeAm);
            }
        }

        /// <summary>
        /// 설정해 놓은 작업 시간을 Duty를 표시한다
        /// </summary>
        private void SetWorkDuty()
        {
            if (clsEmrPublic.VitalWorkDutySet == "ALL")
            {
                if (optAll.Checked == true)
                {
                    ColVisible();
                }
                else
                {
                    optAll.Checked = true;
                }
            }
            else if (clsEmrPublic.VitalWorkDutySet == "NAM")
            {
                if (optNightAm.Checked == true)
                {
                    ColVisible();
                }
                else
                {
                    optNightAm.Checked = true;
                }
            }
            else if (clsEmrPublic.VitalWorkDutySet == "DAY")
            {
                if (optDay.Checked == true)
                {
                    ColVisible();
                }
                else
                {
                    optDay.Checked = true;
                }
            }
            else if (clsEmrPublic.VitalWorkDutySet == "EVE")
            {
                if (optEve.Checked == true)
                {
                    ColVisible();
                }
                else
                {
                    optEve.Checked = true;
                }
            }
            else if (clsEmrPublic.VitalWorkDutySet == "NPM")
            {
                if (optNight.Checked == true)
                {
                    ColVisible();
                }
                else
                {
                    optNight.Checked = true;
                }
            }
        }

        /// <summary>
        /// 칼럼 보이기 설정 
        /// </summary>
        private void VisibleSetTrue()
        {
            int i = 0;

            for (i = COL_EXP; i < ssVital_Sheet1.Columns.Count; i++)
            {
                ssVital_Sheet1.Columns[i].Visible = true;
            }

        }

        /// <summary>
        /// 칼럼 보이기 설정
        /// </summary>
        /// <param name="strTime"></param>
        private void VisibleSet(string strTime)
        {
            VisibleSetDt(ssVital_Sheet1, strTime.Split('/')[0], strTime.Split('/')[1]);
        }

        /// <summary>
        /// 칼럼 보이기 설정
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="STime"></param>
        /// <param name="ETime"></param>
        private void VisibleSetDt(FarPoint.Win.Spread.SheetView SpdView, string STime, string ETime)
        {
            int i = 0;

            for (i = COL_DATAS; i < SpdView.Columns.Count; i++)
            {
                SpdView.Columns[i].Visible = true;
            }

            for (i = COL_DATAS; i < SpdView.Columns.Count; i++)
            {
                if (VB.Val(STime) <= 2300)
                {
                    if (VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) < VB.Val(STime.Replace(":", "")))
                    {
                        SpdView.Columns[i].Visible = false;
                    }
                    else if (VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) > VB.Val(ETime.Replace(":", "")))
                    {
                        SpdView.Columns[i].Visible = false;
                    }
                }
                else
                {
                    if (VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) > 700 && VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) < 2301)
                    {
                        SpdView.Columns[i].Visible = false;
                    }
                }

            }
        }

        /// <summary>
        /// 그룹 확장
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="Row"></param>
        /// <param name="strJOBGB"></param>
        /// <param name="strJobExpend"></param>
        private void ExpendRow(FarPoint.Win.Spread.SheetView SpdView, int Row, string strJOBGB, string strJobExpend)
        {
            bool blnExpend = false;

            if (strJobExpend == "+" || strJobExpend == "-")
            {
                if (strJobExpend == "+")
                {
                    blnExpend = true;
                    ssVital_Sheet1.Cells[Row, COL_EXP].Text = "-";
                }
                else
                {
                    blnExpend = false;
                    ssVital_Sheet1.Cells[Row, COL_EXP].Text = "+";
                }

                if (SpdView != null)
                {
                    SetRowSave(Row, strJOBGB, strJobExpend);
                }

                for (int i = Row; i < ssVital_Sheet1.RowCount; i++)
                {
                    if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == strJOBGB)
                    {
                        if (ssVital_Sheet1.Cells[i, COL_EXP].Text.Trim() == "")
                        {
                            ssVital_Sheet1.Rows[i].Visible = blnExpend;
                        }
                    }
                }
            }
        }

        private void SetRowSave(int Row, string strJOBGB, string strJobExpend)
        {
            ExpendRowVal expendRow = new ExpendRowVal();

            if (ExpendRows.TryGetValue(Row, out expendRow))
            {
                ExpendRows.Remove(Row);

                expendRow.strJOBGB = strJOBGB;
                expendRow.strJobExpend = strJobExpend;
                ExpendRows.Add(Row, expendRow);
            }
            else
            {
                expendRow.strJOBGB = strJOBGB;
                expendRow.strJobExpend = strJobExpend;
                ExpendRows.Add(Row, expendRow);
            }
        }

        /// <summary>
        /// 그룹이 시작하는 위치 찾기
        /// </summary>
        /// <param name="strJOBGB"></param>
        /// <returns></returns>
        private int FindJobGbRow(string strJOBGB)
        {
            int rtnVal = 0;

            for (int i = 0; i < ssVital_Sheet1.RowCount; i++)
            {
                if (ssVital_Sheet1.Cells[i, COL_JOB].Text.Trim() == strJOBGB)
                {
                    rtnVal = i;
                    break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 팦업 메뉴(공통)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemValue_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strPopMenuName = ((MenuItem)sender).Text.Trim();

            mSpd.ContextMenu = null;

            if (mPopRow == -1) return;
            if (mSpdView == null) return;

            if (strPopMenuName.IndexOf("]") != -1)
            {
                if (mSpdView.Cells[mPopRow, mPopCol].Text.Trim() != "")
                {
                    mSpdView.Cells[mPopRow, mPopCol].Text = mSpdView.Cells[mPopRow, mPopCol].Text.Trim() + "," + (strPopMenuName.Split(']')[0]).Trim();
                }
                else
                {
                    mSpdView.Cells[mPopRow, mPopCol].Text = (strPopMenuName.Split(']')[0]).Trim();
                }
            }
            else
            {
                string ItemCd = mSpdView.Cells[mPopRow, COL_ITEM].Text.Trim();
                string ItemNm = mSpdView.Cells[mPopRow, COL_INAME].Text.Trim();

                if (mPopCol == COL_INAME && FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, mstrFormNo, p.ward, ItemCd, strPopMenuName))
                {
                    using (frmSugaOrderSave frmSugaOrderSaveX = new frmSugaOrderSave(dtpFrDate.Value.ToString("yyyy-MM-dd"), "3150", ItemCd, strPopMenuName, ItemNm , p, dtpFrDate.Value.ToString("yyyyMMdd"), null, -1))
                    {
                        frmSugaOrderSaveX.GetOrderData(dtpFrDate.Value.ToString("yyyyMMdd"));
                        frmSugaOrderSaveX.ShowDialog(this);
                    }
                    btnSearch.PerformClick();
                    return;
                }

                if ((strPopMenuName.Equals("Keep 이동") || strPopMenuName.Equals("Keep이동")) && ComFunc.MsgBoxQEx(this, "수술방으로 이동하는 환자인가요?\r\n아니라면 Keep으로 작성 부탁드립니다.") == DialogResult.No)
                {
                    return;
                }

                mSpdView.Cells[mPopRow, mPopCol].Text = strPopMenuName;

                if (ssVital_Sheet1.Cells[mPopRow, COL_JOB].Text.Trim() == "IVT")
                {
                    SumGCSOutPutRow(mPopRow, mPopCol);
                }
            }

            if (strPopMenuName != "")
            {
                mSpdView.Cells[mSpdView.RowCount - 1, mPopCol].Text = "Y";
            }
        }

        /// <summary>
        /// 시간 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        /// <returns></returns>
        private bool SaveTimeSet(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            bool rtnVal = false;

            int i = 0;
            OracleDataReader reader = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                SQL = "";
                SQL = "DELETE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                for (i = COL_DATAS; i < SpdView.Columns.Count; i++)
                {
                    #region 무결성 오류 나서 점검
                    SQL = "SELECT 1 AS CNT";
                    SQL = SQL + ComNum.VBLF + "FROM DUAL";
                    SQL = SQL + ComNum.VBLF + "WHERE EXISTS";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "SELECT 1";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBVITALTIME";
                    SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND JOBGB = '" + strJOBGB + "'";
                    SQL = SQL + ComNum.VBLF + "   AND TIMEVALUE = '" + SpdView.Cells[2, i].Text.Trim().Replace(":", "") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND SUBGB = '" + (SpdView.Cells[3, i].Text.Trim().Equals("합계") ? "1" : "0") + "'";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion

                    if (reader.HasRows == false)
                    {
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                        SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES (";
                        SQL = SQL + ComNum.VBLF + "" + mstrFormNo + ",";
                        SQL = SQL + ComNum.VBLF + "" + p.acpNo + ",";
                        SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";
                        SQL = SQL + ComNum.VBLF + "'" + dtpFrDate.Value.ToString("yyyyMMdd") + "',";
                        SQL = SQL + ComNum.VBLF + "'" + SpdView.Cells[2, i].Text.Trim().Replace(":", "") + "',";
                        if (SpdView.Cells[3, i].Text.Trim() == "합계")
                        {
                            SQL = SQL + ComNum.VBLF + "'1',";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "'0',";
                        }
                        SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "',";
                        SQL = SQL + ComNum.VBLF + "'" + VB.Right(strCurDateTime, 6) + "',";
                        SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
                        SQL = SQL + ComNum.VBLF + ")";
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

                    reader.Dispose();
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
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

        #endregion 공통모듈

        #region 폼 이벤트 공통

        private void mbtnSaveItemIVT_Click(object sender, EventArgs e)
        {
            SetItem("임상관찰");
        }

        private void mbtnSaveItemIIO_Click(object sender, EventArgs e)
        {
            SetItem("섭취배설");
        }

        private void mbtnSaveItemIST_Click(object sender, EventArgs e)
        {
            SetItem("특수치료");
        }

        private void mbtnSaveItemIBN_Click(object sender, EventArgs e)
        {
            SetItem("기본간호");
        }

        private void ssVital_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string strJOBGB = ssVital_Sheet1.Cells[e.Row, COL_JOB].Text.Trim();
                string strJobExpend = ssVital_Sheet1.Cells[e.Row, COL_EXP].Text.Trim();

                if (e.Column == COL_EXP)
                {
                    ExpendRow(ssVital_Sheet1, e.Row, strJOBGB, strJobExpend);
                    return;
                }

                if (e.Column == COL_INAME)
                {
                    return;
                }
            }


            int i = 0;

            if (ssVital_Sheet1.Columns.Count <= mintTCol) return;

            if (e.Button == MouseButtons.Right && e.Column == COL_INAME)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }

            if (e.Column < COL_DATAS) return;

            for (i = COL_DATAS; i < ssVital_Sheet1.Columns.Count; i++)
            {
                ssVital_Sheet1.Cells[0, i, ssVital_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssVital_Sheet1.Cells[0, e.Column, ssVital_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);

            for (i = 0; i < ssVital_Sheet1.RowCount; i++)
            {
                ssVital_Sheet1.Cells[i, 0, i, 4].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssVital_Sheet1.Cells[e.Row, 4, e.Row, 4].Font = new Font("굴림", 9, FontStyle.Bold);

            if (e.Button == MouseButtons.Right)
            {                
                ViewItemValue(e.Row, e.Column);               
                return;
            }

        }

        private void ssVital_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < COL_DATAS) return;
            if (e.Row < mintTRow) return;
            if (e.Row > ssVital_Sheet1.RowCount - 5) return;

            ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, e.Column].Text = "Y";

            //합계를 구한다.
            if (ssVital_Sheet1.Cells[e.Row, COL_JOB].Text.Trim() == "IIO")
            {
                SumIntakeOutPutRow(e.Column);
            }
            else if (ssVital_Sheet1.Cells[e.Row, COL_JOB].Text.Trim() == "IVT")
            {
                SumGCSOutPutRow(e.Row, e.Column);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string strJOBGB = "";
            string strJobExpend = "-";
            int Row = 0;

            strJOBGB = "IVT";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IIO";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IST";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IBN";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            string strJOBGB = "";
            string strJobExpend = "+";
            int Row = 0;

            strJOBGB = "IVT";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IIO";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IST";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);

            strJOBGB = "IBN";
            Row = FindJobGbRow(strJOBGB);
            if (Row > 0) ExpendRow(ssVital_Sheet1, Row, strJOBGB, strJobExpend);
        }

        private void txtTime_Enter(object sender, EventArgs e)
        {
            txtTime.SelectAll();
        }

        private void txtTime_Click(object sender, EventArgs e)
        {
            txtTime.SelectAll();
        }

        private void mbtnInsert_Click(object sender, EventArgs e)
        {
            SaveTime(txtTime.Text.Trim());
            ColVisible();
        }

        private void mbtnInsertTime_Click(object sender, EventArgs e)
        {
            if (frmNrIcuTimeSetX != null)
            {
                frmNrIcuTimeSetX.Dispose();
                frmNrIcuTimeSetX = null;
            }

            frmNrIcuTimeSetX = new frmNrIcuTimeSet();
            frmNrIcuTimeSetX.rSetTime += new frmNrIcuTimeSet.SetTime(frmNrIcuTimeSet_SetTime);
            frmNrIcuTimeSetX.rEventClosed += new frmNrIcuTimeSet.EventClosed(frmNrIcuTimeSet_EventClosed);
            frmNrIcuTimeSetX.TopMost = true;
            frmNrIcuTimeSetX.ShowDialog(this);
        }

        private void frmNrIcuTimeSet_EventClosed()
        {
            frmNrIcuTimeSetX.Dispose();
            frmNrIcuTimeSetX = null;
        }

        private void frmNrIcuTimeSet_SetTime(string strTime)
        {
            frmNrIcuTimeSetX.Dispose();
            frmNrIcuTimeSetX = null;

            string[] arryTime = strTime.Split('/');

            int i = 0;
            int j = 0;

            SetSpread();

            for (i = 0; i < arryTime.Length; i++)
            {
                string strTimeCheck = arryTime[i];
                bool blnFind = false;

                for (j = COL_DATAS; j < mSpdView.Columns.Count; j++)
                {
                    if (mSpdView.Cells[2, j].Text.Trim() == strTimeCheck.Trim())
                    {
                        blnFind = true;
                        break;
                    }
                }
                if (blnFind == false)
                {
                    SaveTime(arryTime[i].Trim());
                }
            }

            ColVisible();
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                clsEmrPublic.VitalWorkDutySet = "ALL";
                ColVisible();
            }
        }

        private void optDay_CheckedChanged(object sender, EventArgs e)
        {
            if (optDay.Checked == true)
            {
                clsEmrPublic.VitalWorkDutySet = "DAY";
                ColVisible();
            }
        }

        private void optEve_CheckedChanged(object sender, EventArgs e)
        {
            if (optEve.Checked == true)
            {
                clsEmrPublic.VitalWorkDutySet = "EVE";
                ColVisible();
            }
        }

        private void optNight_CheckedChanged(object sender, EventArgs e)
        {
            if (optNight.Checked == true)
            {
                clsEmrPublic.VitalWorkDutySet = "NPM";
                ColVisible();
            }
        }

        private void optNightAm_CheckedChanged(object sender, EventArgs e)
        {
            if (optNightAm.Checked == true)
            {
                clsEmrPublic.VitalWorkDutySet = "NAM";
                ColVisible();
            }
        }

        private void dtpFrDate_ValueChanged(object sender, EventArgs e)
        {
            if (isFormLoad == true)
            {
                isFormLoad = false;
                return;
            }
            CheckItemExists(dtpFrDate.Value.ToString("yyyyMMdd"));
            pClearFormExcept();
            pLoadEmrChartInfo();
            pGetInjectOrderInfo();
            SetWorkDuty();
            if (mstrMode.Equals("W") && FormPatInfoFunc.Set_FormPatInfo_IsBlood(clsDB.DbCon, p.ptNo, dtpFrDate.Value.ToString("yyyy-MM-dd")))
            {
                btnBlood.Visible = true;
                clsEmrQuery.CheckItemExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"), "I0000022324");
                clsEmrQuery.CheckItemExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"), "I0000030622");
                clsEmrQuery.CheckBloodTimeExists(p, mstrFormNo, dtpFrDate.Value.ToString("yyyyMMdd"));
            }
            else
            {
                btnBlood.Visible = false;
            }
        }


        /// <summary>
        /// 차트일자에 아이템이 존재하는지 체크해서 넣는다
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="Spd"></param>
        private void CheckItemExists(string strChartDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT A.ACPNO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";

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
                Cursor.Current = Cursors.Default;
                return;
            }
            dt.Dispose();
            dt = null;

            #region //기본값 설정
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                SQL = "";
                SQL = "SELECT A.ACPNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBVITALSET AA ";
                SQL = SQL + ComNum.VBLF + "                                    WHERE AA.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                                        AND AA.ACPNO = " + p.acpNo;
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

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO  " + ComNum.DB_EMR + "AEMRBVITALSET A";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "   A.FORMNO, A.ACPNO, A.PTNO, ";
                    SQL = SQL + ComNum.VBLF + "   '" + strChartDate + "' AS CHARTDATE,  ";
                    SQL = SQL + ComNum.VBLF + "   A.JOBGB, A.ITEMCD,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + strWDate + "' AS WRITEDATE,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + strWTime + "' AS WRITETIME,  ";
                    SQL = SQL + ComNum.VBLF + "   '" + clsType.User.IdNumber + "' AS WRITEUSEID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                    SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBVITALSET AA ";
                    SQL = SQL + ComNum.VBLF + "                                    WHERE AA.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "                                        AND AA.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                        AND AA.CHARTDATE < '" + strChartDate + "')";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     B.UNITCLS, B.BASCD, B.BASNAME, B.BASEXNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "     ON BB.BASCD = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호' ) ";
                SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "WHERE BB.BSNSCLS = '기록지관리' ";
                if (mstrFormNo == "2135") //회복실
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + "OP" + "'";
                }
                else if (mstrFormNo == "1935") //Angio
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + "AG" + "'";
                }
                else if (mstrFormNo == "2431") //진정 환자 평가
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + "ENDO" + "'";
                }
                else if (mstrFormNo == "1969") //응급실 SPECIAL WATCH RECORD
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + "ER" + "'";
                }
                else if (mstrFormNo == "2201") //인공신장실 V/S Sheet
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + "HD" + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + (string.IsNullOrWhiteSpace(p.ward) && p.medDeptCd.Equals("ER") ? "ER" : p.ward) + "'";
                }
                SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";

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
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                        SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES ( ";
                        SQL = SQL + ComNum.VBLF + "     " + mstrFormNo + ", ";
                        SQL = SQL + ComNum.VBLF + "     " + p.acpNo + ", ";
                        SQL = SQL + ComNum.VBLF + "     '" + p.ptNo + "', ";
                        SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',";
                        if (dt.Rows[i]["UNITCLS"].ToString().Trim() == "임상관찰")
                        {
                            SQL = SQL + ComNum.VBLF + "     'IVT', ";
                        }
                        else if (dt.Rows[i]["UNITCLS"].ToString().Trim() == "섭취배설")
                        {
                            SQL = SQL + ComNum.VBLF + "     'IIO', ";
                        }
                        else if (dt.Rows[i]["UNITCLS"].ToString().Trim() == "특수치료")
                        {
                            SQL = SQL + ComNum.VBLF + "     'IST', ";
                        }
                        else if (dt.Rows[i]["UNITCLS"].ToString().Trim() == "기본간호")
                        {
                            SQL = SQL + ComNum.VBLF + "     'IBN', ";
                        }
                        SQL = SQL + ComNum.VBLF + "     '" + dt.Rows[i]["BASCD"].ToString().Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "     '" + strWDate + "', ";
                        SQL = SQL + ComNum.VBLF + "     '" + strWTime + "', ";
                        SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";
                        SQL = SQL + ComNum.VBLF + "     )";
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

        private void mbtnBefore_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(-1);

            if (panGraph.Visible)
            {
                dtpGraphFDate.Value = dtpFrDate.Value;
                dtpGraphTDate.Value = dtpFrDate.Value;

                GetVitalGraph();
            }
        }

        private void mbtnNext_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(+1);

            if (panGraph.Visible)
            {
                dtpGraphFDate.Value = dtpFrDate.Value;
                dtpGraphTDate.Value = dtpFrDate.Value;

                GetVitalGraph();
            }

        }

        #endregion 폼 이벤트 공통

        #region IO관련

        #region IO 함수

        /// <summary>
        /// 주사처방을 조회 한다
        /// </summary>
        private void pGetInjectOrderInfo()
        {
            READ_OCS_IORDER(p.ptNo, dtpFrDate.Value.ToShortDateString(), "20", ssOrder);
            READ_OCS_IORDER(p.ptNo, dtpFrDate.Value.AddDays(-1).ToShortDateString(), "20", ssOrder1);
        }

        /// <summary>
        /// 오더 조회
        /// </summary>
        /// <param name="strPtno">등록번호</param>
        /// <param name="strDate">날짜</param>
        /// <param name="strBun">처방 약분류 코드</param>
        /// <param name="spd">스프레드</param>
        void READ_OCS_IORDER(string strPtno, string strDate, string strBun, FarPoint.Win.Spread.FpSpread spd)
        {
            string SQL = string.Empty;
            DataTable dt = null;

            spd.ActiveSheet.RowCount = 0;

            try
            {
                #region //입원
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "     'IPD' AS SITEGB, ";
                SQL = SQL + ComNum.VBLF + "        O.BUN , O.OrderCode, O.Contents, O.BCONTENTS, O.GbGroup, O.SlipNo, O.SeqNo,  ";
                SQL = SQL + ComNum.VBLF + "        D.DOSCODE, D.DosName,  ";
                SQL = SQL + ComNum.VBLF + "        S.SUNAMEK,  S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4  ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER  M,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S                            ";
                SQL = SQL + ComNum.VBLF + " WHERE  O.BDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  O.Ptno = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  O.Bun IN ( '" + strBun + "' ) ";
                SQL = SQL + ComNum.VBLF + "   AND  (O.GbPRN IN  NULL OR O.GbPRN <> 'P') ";
                SQL = SQL + ComNum.VBLF + "   AND    O.GbPRN <>'S' ";//  'jjy 추가(2000/05/22 'S는 선수납(선불)
                SQL = SQL + ComNum.VBLF + "   AND   (O.GbStatus    = ' ' OR O.GbStatus IS NULL)    ";
                SQL = SQL + ComNum.VBLF + "   AND   (O.GbStatus  <> 'D' AND O.GbStatus <> 'D-')    ";
                SQL = SQL + ComNum.VBLF + "   AND    O.Ptno       =  M.Pano           ";
                SQL = SQL + ComNum.VBLF + "   AND   O.GBDIV >= O.ACTDIV ";
                SQL = SQL + ComNum.VBLF + "   AND    M.GBSTS IN ('0','2')              ";
                SQL = SQL + ComNum.VBLF + "   AND    M.OUTDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "   AND    O.Ptno       =  P.Pano(+)        ";
                SQL = SQL + ComNum.VBLF + "   AND    O.SlipNo     =  C.SlipNo(+)      ";
                SQL = SQL + ComNum.VBLF + "   AND    O.OrderCode  =  C.OrderCode(+)   ";
                SQL = SQL + ComNum.VBLF + "   AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)   ";
                SQL = SQL + ComNum.VBLF + "   AND    O.DosCode    =  D.DosCode(+)     ";
                SQL = SQL + ComNum.VBLF + "   AND    O.DRCODE      =  N.SABUN(+)      ";
                SQL = SQL + ComNum.VBLF + "   AND    O.SUCODE = S.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "   AND    (O.ORDERCODE IN";
                SQL = SQL + ComNum.VBLF + "           (SELECT BASCD ";
                SQL = SQL + ComNum.VBLF + "              FROM KOSMOS_EMR.AEMRBASCD";
                SQL = SQL + ComNum.VBLF + "             WHERE BSNSCLS  = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "               AND UNITCLS  = '꼬마병'";
                SQL = SQL + ComNum.VBLF + "           ) OR (O.GBGROUP <> ' ')";
                SQL = SQL + ComNum.VBLF + "          ) ";
                //SQL = SQL + ComNum.VBLF + "ORDER  BY o.GbGroup, o.bun, M.RoomCode, M.Pano,  O.DOSCODE,O.SlipNo, S.SUNAMEK,  O.SeqNo     ";
                #endregion //입원
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                #region //응급실
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "     'ERD' AS SITEGB, ";
                SQL = SQL + ComNum.VBLF + "        O.BUN , O.OrderCode, O.Contents, O.BCONTENTS, O.GbGroup, O.SlipNo, O.SeqNo,  ";
                SQL = SQL + ComNum.VBLF + "        D.DOSCODE, D.DosName,  ";
                SQL = SQL + ComNum.VBLF + "        S.SUNAMEK,  S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4  ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.OPD_MASTER  M,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S ";
                SQL = SQL + ComNum.VBLF + " WHERE  O.PTNO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "    AND  O.BUN IN ( " + strBun + " ) ";
                SQL = SQL + ComNum.VBLF + "    AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
                SQL = SQL + ComNum.VBLF + "    AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ";
                SQL = SQL + ComNum.VBLF + "    AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
                SQL = SQL + ComNum.VBLF + "    AND   O.BDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND    O.GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
                SQL = SQL + ComNum.VBLF + "    AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
                SQL = SQL + ComNum.VBLF + "    AND    O.PTNO       =  M.PANO           ";
                SQL = SQL + ComNum.VBLF + "    AND   O.QTY  <>  '0'    ";
                SQL = SQL + ComNum.VBLF + "    AND  M.ACTDATE = TO_DATE('" + p.medFrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND  M.DEPTCODE = 'ER'";
                SQL = SQL + ComNum.VBLF + "    AND   O.GBTFLAG <> 'T'";        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
                SQL = SQL + ComNum.VBLF + "    AND    O.PTNO       =  P.PANO(+)        ";
                SQL = SQL + ComNum.VBLF + "    AND    O.SLIPNO     =  C.SLIPNO(+)      ";
                SQL = SQL + ComNum.VBLF + "    AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
                SQL = SQL + ComNum.VBLF + "    AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
                SQL = SQL + ComNum.VBLF + "    AND    O.DOSCODE    =  D.DOSCODE(+)     ";
                SQL = SQL + ComNum.VBLF + "    AND    O.DRCODE      =  N.SABUN(+)      ";
                SQL = SQL + ComNum.VBLF + "    AND    O.SUCODE = S.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "   AND    (O.ORDERCODE IN";
                SQL = SQL + ComNum.VBLF + "           (SELECT BASCD ";
                SQL = SQL + ComNum.VBLF + "              FROM KOSMOS_EMR.AEMRBASCD";
                SQL = SQL + ComNum.VBLF + "             WHERE BSNSCLS  = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "               AND UNITCLS  = '꼬마병'";
                SQL = SQL + ComNum.VBLF + "           ) OR (O.GBGROUP <> ' ')";
                SQL = SQL + ComNum.VBLF + "          ) ";
                #endregion //응급실
                SQL = SQL + ComNum.VBLF + "ORDER  BY SITEGB, GbGroup, BUN, DOSCODE, SlipNo, SUNAMEK, SeqNo  ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                    return;

                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double nUnitNew1 = VB.Val(dt.Rows[i]["UnitNew1"].ToString().Trim());
                    double nBContents = VB.Val(dt.Rows[i]["BCONTENTS"].ToString().Trim());
                    string strUnitNew2 = dt.Rows[i]["UnitNew2"].ToString().Trim();
                    string strUnitNew3 = dt.Rows[i]["UnitNew3"].ToString().Trim();
                    double nUnitNew4 = VB.Val(dt.Rows[i]["UnitNew4"].ToString().Trim());

                    nUnitNew4 = nUnitNew4 == 0 ? nUnitNew1 : nUnitNew4;

                    double nContents = VB.Val(dt.Rows[i]["Contents"].ToString().Trim());

                    spd.ActiveSheet.Cells[i, 0].Text = " " + dt.Rows[i]["OrderCode"].ToString().Trim();
                    if (dt.Rows[i]["SITEGB"].ToString().Trim() == "ERD")
                    {
                        spd.ActiveSheet.Cells[i, 1].Text = "(ER)" + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[i, 1].Text = "  " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                    spd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DosName"].ToString().Trim();
                }

                dt.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// GCS 합
        /// </summary>
        /// <param name="Col"></param>
        private void SumIntakeOutPutRow2(int Col)
        {
            int j = 0;
            int intIntakeRow = -1;
            int intOutPutRow = -1;
            int intIntakeSum = 0;
            int intOutPutSum = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            //Intake
            //OutPut
            for (j = 4; j < ssVital_Sheet1.RowCount - 5; j++)
            {
                string strITEMCD = ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim();

                if ((strITEMCD == "I0000030622") || (strITEMCD == "I0000030623"))
                {
                    if (strITEMCD == "I0000030622")
                    {
                        intIntakeRow = j;
                    }
                    else if (strITEMCD == "I0000030623")
                    {
                        intOutPutRow = j;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = SQL + "SELECT VFLAG3";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                    SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '" + mstrFormNameGb + "'";
                    SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설'";
                    SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";

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
                        if (dt.Rows[0]["VFLAG3"].ToString().Trim() == "01.섭취")
                        {
                            intIntakeSum = intIntakeSum + Convert.ToInt32(VB.Val(ssVital_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                        else if (dt.Rows[0]["VFLAG3"].ToString().Trim() == "11.배설")
                        {
                            intOutPutSum = intOutPutSum + Convert.ToInt32(VB.Val(ssVital_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }

            if (intIntakeRow >= 0)
            {
                ssVital_Sheet1.Cells[intIntakeRow, Col].Text = intIntakeSum.ToString();
            }

            if (intOutPutRow >= 0)
            {
                ssVital_Sheet1.Cells[intOutPutRow, Col].Text = intOutPutSum.ToString();
            }
        }

        /// <summary>
        /// GCS TOTAL
        /// </summary>
        /// <param name="Col"></param>
        private void SumGCSOutPutRow(int Row, int Col)
        {
            int intOutPutSum = 0;
            int intOutPutSumRow = -1;
            int intStartSumRow = -1;
            int j;

            for (j = 4; j < ssVital_Sheet1.RowCount - 5; j++)
            {
                string strITEMCD = ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim();
                if (ssVital_Sheet1.Cells[j, 3].Text.Trim().Equals("GCS"))
                {
                    intStartSumRow = j;
                }

                //GCS TOTAL
                if (strITEMCD == "I0000034033")
                {
                    intOutPutSumRow = j;
                    break;
                }
            }

            if (intOutPutSumRow == -1)
                return;

            for (j = intStartSumRow; j < intOutPutSumRow; j++)
            {
                intOutPutSum += (int)VB.Val(returnNumber(ssVital_Sheet1.Cells[j, Col].Text.Trim()));
            }

            ssVital_Sheet1.Cells[intOutPutSumRow, Col].Text = intOutPutSum.ToString();
        }

        /// <summary>
        /// 문자열에서 숫자만 추출
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        private string returnNumber(string Val)
        {
            return Regex.Replace(Val, @"[^0-9]", "");
        }

        /// <summary>
        /// IO 함계를 구한다
        /// </summary>
        /// <param name="Col"></param>
        private void SumIntakeOutPutRow(int Col)
        {
            int j = 0;
            int intIntakeRow = -1;
            int intOutPutRow = -1;
            double lngIntakeSum = 0;
            double lngOutPutSum = 0;

            //Intake
            //OutPut

            #region 섭취배설 데이터 받아놓기
            //섭취배설 아이템 목록
            Dictionary<string, string> lstItem = new Dictionary<string, string>();
            string SQL = string.Empty;
            string SqlErr = string.Empty;   //에러문 받는 변수
            OracleDataReader reader = null;

            SQL += "SELECT BASCD, VFLAG3";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL += ComNum.VBLF + "WHERE BSNSCLS = '" + mstrFormNameGb + "'";
            SQL += ComNum.VBLF + "    AND UNITCLS = '섭취배설'";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lstItem.Add(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim());
                }

            }
            reader.Dispose();
            #endregion

            for (j = 4; j < ssVital_Sheet1.RowCount - 5; j++)
            {
                string strITEMCD = ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim();

                if ((strITEMCD == "I0000030622") || (strITEMCD == "I0000030623"))
                {
                    if (strITEMCD == "I0000030622")
                    {
                        intIntakeRow = j;
                    }
                    else if (strITEMCD == "I0000030623")
                    {
                        intOutPutRow = j;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(strITEMCD))
                    {
                        string strValue = string.Empty;
                        if (lstItem.TryGetValue(strITEMCD, out strValue))
                        {
                            if (strValue.Equals("01.섭취"))
                            {
                                lngIntakeSum += VB.Val(ssVital_Sheet1.Cells[j, Col].Text.Trim());
                            }
                            else if (strValue.Equals("11.배설"))
                            {
                                lngOutPutSum += VB.Val(ssVital_Sheet1.Cells[j, Col].Text.Trim());
                            }
                        }
                    }
                }
            }

            if (intIntakeRow >= 0)
            {
                ssVital_Sheet1.Cells[intIntakeRow, Col].Text = lngIntakeSum.ToString();
            }

            if (intOutPutRow >= 0)
            {
                ssVital_Sheet1.Cells[intOutPutRow, Col].Text = lngOutPutSum.ToString();
            }
        }

        /// <summary>
        /// 팦업 메뉴 보이기
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        private void ViewItemValue(int Row, int Col)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            PopupMenu = null;

            PopupMenu = new ContextMenu();
            mSpd.ContextMenu = null;
            mPopRow = -1;
            mPopCol = -1;

            mPopRow = Row;
            mPopCol = Col;

            if (Row < 4)
            {
                if (mSpd.Name == "")
                {

                }
                string strCHARTUSEID = ssVital_Sheet1.Cells[3, mPopCol].Text.Trim();
                if (strCHARTUSEID == "합계")
                {
                    PopupMenu = new ContextMenu();
                    PopupMenu.MenuItems.Add("합계 삭제", new System.EventHandler(SubMenuIo_Click));
                    mSpd.ContextMenu = PopupMenu; // 입력
                    return;
                }
                else
                {
                    PopupMenu = new ContextMenu();
                    PopupMenu.MenuItems.Add("합계 추가", new System.EventHandler(SubMenuIo_Click));
                    if (p.ward.Equals("33") || p.ward.Equals("35") || mstrFormNo.Equals("1969") || p.ptNo.Equals("81000005"))
                    {
                        PopupMenu.MenuItems.Add("연동", new System.EventHandler(btnInterface_Click)).Tag = mSpdView.Cells[2, mPopCol].Text.Trim();
                    }
                    mSpd.ContextMenu = PopupMenu; // 입력
                    return;
                }
            }

            mSpdView.SetActiveCell(Row, Col);

            string strITEMCD = mSpdView.Cells[Row, COL_ITEM].Text.Trim();
            string strITEMNM = mSpdView.Cells[Row, COL_INAME].Text.Trim();


            if (strITEMCD.Equals("I0000037364"))
            {
                frmEmrBaseDefaultValueSetX = new frmEmrBaseDefaultValueSet("IVT", strITEMCD);
                frmEmrBaseDefaultValueSetX.rSetValue += new frmEmrBaseDefaultValueSet.SetValue(frmEmrBaseDefaultValueSet_SetValue);
                frmEmrBaseDefaultValueSetX.rEventClosed += new frmEmrBaseDefaultValueSet.EventClosed(frmEmrBaseDefaultValueSet_EventClosed);
                frmEmrBaseDefaultValueSetX.ShowDialog(this);

                return;
            }
            //SQL = "";
            //SQL = "SELECT ";
            //SQL = SQL + ComNum.VBLF + "     U.ITEMCD, F.FUNCCD, F.FUNCNAME ";
            //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEFUNC U";
            //SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRFUNCTION  F";
            //SQL = SQL + ComNum.VBLF + "     ON U.FUNCCD = F.FUNCCD ";
            //SQL = SQL + ComNum.VBLF + "WHERE U.JOBGB = 'IVT'";
            //SQL = SQL + ComNum.VBLF + "     AND U.ITEMCD = '" + strITEMCD + "'";
            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return;
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    dt.Dispose();
            //    dt = null;

            //    frmEmrBaseDefaultValueSetX = new frmEmrBaseDefaultValueSet();
            //    frmEmrBaseDefaultValueSetX.rSetValue += new frmEmrBaseDefaultValueSet.SetValue(frmEmrBaseDefaultValueSet_SetValue);
            //    frmEmrBaseDefaultValueSetX.rEventClosed += new frmEmrBaseDefaultValueSet.EventClosed(frmEmrBaseDefaultValueSet_EventClosed);
            //    frmEmrBaseDefaultValueSetX.ShowDialog(this);

            //    return;
            //}
            //dt.Dispose();
            //dt = null;

            Cursor.Current = Cursors.WaitCursor;
            List<string> lstOut = new List<string>();
            #region 혈압측정부위 제외
            if (strITEMCD.Equals("I0000035464") || strITEMCD.Equals("I0000037575")) //체온, 혈압 측정위치
            {
                SQL = "SELECT RT_A, LT_A, RT_L, LT_L";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "NUR_VITAL_REGION";
                SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + p.ptNo + "'";

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
                    if (dt.Rows[0]["RT_A"].ToString().Equals("1"))
                    {
                        lstOut.Add("'Rt Arm'");
                    }

                    if (dt.Rows[0]["LT_A"].ToString().Equals("1"))
                    {
                        lstOut.Add("'Lt Arm'");
                    }

                    if (dt.Rows[0]["RT_L"].ToString().Equals("1"))
                    {
                        lstOut.Add("'Rt Leg'");
                    }

                    if (dt.Rows[0]["LT_L"].ToString().Equals("1"))
                    {
                        lstOut.Add("'Lt Leg'");
                    }
                }

                dt.Dispose();
            }
            #endregion

            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + ",    CASE WHEN EXISTS";
            SQL = SQL + ComNum.VBLF + "     (";
            SQL = SQL + ComNum.VBLF + "         SELECT 1";
            SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING";
            SQL = SQL + ComNum.VBLF + "          WHERE FORMNO    = 3150";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD    = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE = A.ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "            AND WARD = '" + p.ward + "'";
            SQL = SQL + ComNum.VBLF + "     )";
            SQL = SQL + ComNum.VBLF + "     THEN 1 END MAPPING";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL A";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + "IVT" + "'";
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "' ";

            if (strITEMCD.Equals("I0000035464") || strITEMCD.Equals("I0000037575")) //체온, 혈압 측정위치
            {
                if (lstOut.Count > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND ITEMVALUE NOT IN (" + string.Join(",", lstOut) + ")";
                }
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ ";

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

            PopupMenu.Name = "임상관찰";
            if ((dt.Rows.AsParallel().OfType<DataRow>().Where(d => d["MAPPING"].ToString().NotEmpty()).Any()) && Col == COL_INAME)
            {
                PopupMenu.MenuItems.Add("전체", new System.EventHandler(mnuItemValue_Click));
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (Col == COL_INAME)
                {
                    if (dt.Rows[i]["MAPPING"].ToString().Equals("1"))
                    {
                        PopupMenu.MenuItems.Add(dt.Rows[i]["ITEMVALUE"].ToString().Trim(), new System.EventHandler(mnuItemValue_Click));
                    }
                }
                else
                {
                    PopupMenu.MenuItems.Add(dt.Rows[i]["ITEMVALUE"].ToString().Trim(), new System.EventHandler(mnuItemValue_Click));
                }
            }
            dt.Dispose();
            dt = null;


            Cursor.Current = Cursors.Default;
            mSpd.ContextMenu = PopupMenu;

        }

        private void frmEmrBaseDefaultValueSet_SetValue(string strValue)
        {
            frmEmrBaseDefaultValueSetX.Close();
            frmEmrBaseDefaultValueSetX = null; 

            mSpdView.Cells[mSpdView.ActiveRowIndex, mSpdView.ActiveColumnIndex].Text = strValue;
            //this.Focus();
        }

        private void frmEmrBaseDefaultValueSet_EventClosed()
        {
            frmEmrBaseDefaultValueSetX.Close();
            frmEmrBaseDefaultValueSetX = null;
            //this.Focus();
        }

        /// <summary>
        /// 팦업 메뉴 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubMenuIo_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strJob = ((MenuItem)sender).Text.Trim();
            string strTime = mSpdView.Cells[2, mPopCol].Text.Trim();
            mSpd.ContextMenu = null;
            if (strJob == "합계 추가")
            {
                TotAdd(strTime);
            }
            else if (strJob == "합계 삭제")
            {
                TotDel(strTime);
            }
        }

        /// <summary>
        /// 합계 삭제
        /// </summary>
        /// <param name="strTime"></param>
        private void TotDel(string strTime)
        {
            int intSelCol = mPopCol;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(ssVital_Sheet1.Cells[ssVital_Sheet1.Rows.Count - 4, intSelCol].Text.Trim()));

            if (ssVital_Sheet1.Cells[3, intSelCol].Text.Trim() == "합계")
            {
                if (DeleteAll(mSpdView, "합계") == true)
                {
                    LoadJobDataSub();
                    mstrEmrNo = "0";
                }
            }
        }

        /// <summary>
        /// 삭제할 컬럼에 다른 사람이 작성한 차트가 있는지 확인한다
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        private bool CheckChangedData(int Column, string strTime)
        {
            bool rtnVal = false;

            for (int Row = 4; Row < ssVital_Sheet1.RowCount - 5; Row++)
            {
                string strItemCd = ssVital_Sheet1.Cells[Row, COL_ITEM].Text.Trim();
                string strValue = ssVital_Sheet1.Cells[Row, Column].Text.Trim();

                if (ssVital_Sheet1.Cells[Row, Column].Tag != null)
                {
                    string strUseId = ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();

                    if (strValue != "" || VB.Val(strValue) != 0)
                    {
                        //정맥주입, 총섭취량, 총배설량, 제수량(복막투석)
                        if (strItemCd == "I0000030580") // || strItemCd == "I0000030622" || strItemCd == "I0000030623" || strItemCd == "I0000013209")
                        {
                            rtnVal = true;
                            break;
                        }
                        else
                        {
                            if (strUseId != clsType.User.IdNumber)
                            {
                                rtnVal = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (strValue != "" && VB.Val(strValue) != 0)
                    {
                        //정맥주입, 총섭취량, 총배설량, 제수량(복막투석)
                        if (strItemCd == "I0000030580") // || strItemCd == "I0000030622" || strItemCd == "I0000030623" || strItemCd == "I0000013209")
                        {
                            rtnVal = true;
                            break;
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// Data 삭제 하나만
        /// </summary>
        /// <param name="SpdView"></param>
        private void DeleteOne(FarPoint.Win.Spread.SheetView SpdView)
        {
            int intSelCol = 0;

            if (SpdView.Columns.Count <= mintTCol) return;

            //if (SpdView.ActiveColumnIndex < 6) return;
            intSelCol = SpdView.ActiveColumnIndex;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(SpdView.Cells[SpdView.Rows.Count - 4, intSelCol].Text.Trim()));

            if (VB.Val(strEmrNo) == 0)
            {
                for (int k = 4; k < SpdView.RowCount - 5; k++)
                {
                    SpdView.Cells[k, intSelCol].Text = "";
                }

                SpdView.Columns.Remove(intSelCol, 1);

                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    //return;
                }

                return;
            }

            mstrEmrNo = strEmrNo;
            if (pDelData() == true)
            {
                //if (ComFunc.MsgBoxQEx(this, "시간도 삭제하시겠습니까?") == DialogResult.Yes)
                //{
                SpdView.Columns[intSelCol].Remove();
                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    //return;
                }
                //}

                mstrEmrNo = "0";

                SetSpread();

                LoadJobDataSub();

                if (mEmrCallForm != null)
                {
                    mEmrCallForm.MsgDelete();
                }
            }
        }

        /// <summary>
        /// Data 삭제 함수
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strUseId"></param>
        private bool DeleteAll(FarPoint.Win.Spread.SheetView SpdView, string strUseId = "")
        {
            int intSelCol = 0;

            if (SpdView.Columns.Count <= mintTCol) return false;

            //if (SpdView.ActiveColumnIndex <= mintTCol) return false;

            intSelCol = SpdView.ActiveColumnIndex;
            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(SpdView.Cells[SpdView.Rows.Count - 4, intSelCol].Text.Trim()));

            if (VB.Val(strEmrNo) == 0)
            {
                SpdView.Columns[intSelCol].Remove();
                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    return false;
                }
                return true;
            }

            mstrEmrNo = strEmrNo;

            if (pDelData(strUseId) == true)
            {
                SpdView.Columns[intSelCol].Remove();
                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 합계 추가
        /// </summary>
        /// <param name="strTime"></param>
        private void TotAdd(string strTime)
        {
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(strTime.Replace(":", "")));

            if (intTime <= 0) return;

            if (mPopCol == ssVital_Sheet1.ColumnCount - 1)
            {
                ssVital_Sheet1.ColumnCount = ssVital_Sheet1.ColumnCount + 1;
            }
            else
            {
                ssVital_Sheet1.AddColumns(mPopCol + 1, 1);
            }
            intSelCol = mPopCol + 1;

            ssVital_Sheet1.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(ssVital_Sheet1, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssVital_Sheet1.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssVital_Sheet1, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssVital_Sheet1.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssVital_Sheet1, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssVital_Sheet1.Cells[2, intSelCol].BackColor = Color.LightBlue;
            ssVital_Sheet1.Cells[2, intSelCol].Text = strTime;
            clsSpread.SetTypeAndValue(ssVital_Sheet1, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssVital_Sheet1.Cells[3, intSelCol].BackColor = Color.LightBlue;

            for (j = 4; j < ssVital_Sheet1.RowCount - 5; j++)
            {
                clsSpread.SetTypeAndValue(ssVital_Sheet1, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                if ((ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim() == "I0000030622") || (ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim() == "I0000030623"))
                {
                    ssVital_Sheet1.Cells[j, 0, j, ssVital_Sheet1.ColumnCount - 1].BackColor = Color.LightCyan;
                }
            }
            clsSpread.SetTypeAndValue(ssVital_Sheet1, ssVital_Sheet1.RowCount - 5, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssVital_Sheet1, ssVital_Sheet1.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssVital_Sheet1, ssVital_Sheet1.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssVital_Sheet1, ssVital_Sheet1.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssVital_Sheet1, ssVital_Sheet1.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            ssVital_Sheet1.Cells[3, intSelCol].Text = "합계";
            ssVital_Sheet1.Cells[ssVital_Sheet1.Rows.Count - 3, intSelCol].Text = "합계";

            if (SaveTimeSet(ssVital_Sheet1, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }

            pSaveDataTot(intSelCol);

            LoadJobDataSub();
        }

        /// <summary>
        /// 합계를 저장한다
        /// </summary>
        /// <param name="intSelCol"></param>
        /// <returns></returns>
        public double pSaveDataTot(int intSelCol)
        {
            double dblEmrNo = 0;

            int i = intSelCol;
            int j = 0;

            string[] arryEMRNO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            string strEmrNo = Convert.ToString(VB.Val(ssVital_Sheet1.Cells[ssVital_Sheet1.Rows.Count - 4, i].Text.Trim()));
            string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// ssVital_Sheet1.Cells[0, i].Text.Trim().Replace("-", "");
            string strChartTime = ssVital_Sheet1.Cells[2, i].Text.Trim().Replace(":", "") + "00";
            string strCHARTUSEID = "합계";
            string strCOMPUSEID = "합계";
            string strSAVEGB = "0";
            string strFORMGB = "0";

            SumIntakeOutPutRowTot(intSelCol);

            for (j = 4; j < ssVital_Sheet1.RowCount - 5; j++)
            {
                string strITEMCD = ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim();

                if (ssVital_Sheet1.Cells[j, COL_EXP].Text == "+" || ssVital_Sheet1.Cells[j, COL_EXP].Text == "-")
                {
                    continue;
                }

                if (strITEMCD == "")
                {
                    continue;
                }

                string[] strItem = ssVital_Sheet1.Cells[j, COL_ITEM].Text.Trim().Split('_');
                string strITEMNO = "";
                string strITEMINDEX = "-1";
                if (strItem.Length > 0)
                {
                    strITEMNO = strItem[0].Trim();
                }
                if (strItem.Length > 1)
                {
                    strITEMINDEX = strItem[1].Trim();
                }
                string strDSPSEQ = "0";
                string strITEMTYPE = "TEXT";
                string strITEMVALUE = ssVital_Sheet1.Cells[j, i].Text.Trim();
                string strITEMVALUE1 = "";

                if (arryEMRNO == null)
                {
                    arryEMRNO = new string[0];
                    arryITEMCD = new string[0];
                    arryITEMNO = new string[0];
                    arryITEMINDEX = new string[0];
                    arryITEMTYPE = new string[0];
                    arryITEMVALUE = new string[0];
                    arryITEMVALUE1 = new string[0];
                    arryDSPSEQ = new string[0];
                }

                Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                arryEMRNO[arryEMRNO.Length - 1] = "0";
                arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
            }

            string strSAVECERT = "1";

            if (clsEmrQuery.SaveFlowChart(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime,
                strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1
                ) == 0)
            {
                //에러임
            }

            return dblEmrNo;
        }

        /// <summary>
        /// 합계를 구한다
        /// </summary>
        /// <param name="TotCol"></param>
        private void SumIntakeOutPutRowTot(int TotCol)
        {
            int Col = 0;
            int Row = 0;
            int intSum = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (Row = 4; Row < ssVital_Sheet1.RowCount - 5; Row++)
            {
                string strITEMCD = ssVital_Sheet1.Cells[Row, COL_ITEM].Text.Trim();
                if (strITEMCD == "")
                {
                    continue;
                }

                intSum = 0;

                SQL = "";
                SQL = SQL + "SELECT VFLAG3";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '" + mstrFormNameGb + "'";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설'";
                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";

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
                    for (Col = TotCol - 1; Col >= COL_DATAS; Col--)
                    {
                        if (ssVital_Sheet1.Cells[3, Col].Text.Trim() == "합계")
                        {
                            break;
                        }
                        else
                        {
                            intSum = intSum + Convert.ToInt32(VB.Val(ssVital_Sheet1.Cells[Row, Col].Text.Trim()));
                        }
                    }
                    ssVital_Sheet1.Cells[Row, TotCol].Text = intSum.ToString();
                }
                dt.Dispose();
                dt = null;
            }

        }

        /// <summary>
        /// IO 합계를 구한다
        /// </summary>
        /// <param name="strDuty"></param>
        /// <param name="strSTime"></param>
        /// <param name="strETime"></param>
        private void LoadIoTot(string strDuty, string strSTime, string strETime)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int j = 0;
            int k = 0;

            DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            ssIoTot_Sheet1.Columns.Count = ssIoTot_Sheet1.Columns.Count + 1;
            j = ssIoTot_Sheet1.Columns.Count - 1;
            ssIoTot_Sheet1.Columns[j].Width = 40;

            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 0, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 1, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 2, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 3, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIoTot_Sheet1.Cells[0, j, 3, j].Font = BoldFont;
            ssIoTot_Sheet1.Cells[0, j, 3, j].BackColor = Color.LightBlue;
            ssIoTot_Sheet1.Cells[2, j].Text = strDuty;
            ssIoTot_Sheet1.AddSpanCell(2, j, 2, 1);

            Cursor.Current = Cursors.WaitCursor;

            #region 이전 로직
            if (dtpSys.Date < ("2020-09-22").To<DateTime>() && p.ptNo.Substring(0, 3).Equals("810") == false)
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    B.ITEMCD,  ";
                SQL = SQL + ComNum.VBLF + "    SUM(DECODE(B.ITEMVALUE, NULL, 0, TO_NUMBER(B.ITEMVALUE))) AS ITEMVALUE , MAX(BC.BASNAME) AS ITEMNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBVITALTIME TM ";
                SQL = SQL + ComNum.VBLF + "    ON  A.ACPNO = TM.ACPNO ";
                SQL = SQL + ComNum.VBLF + "    AND TM.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND TM.SUBGB = '0' ";
                SQL = SQL + ComNum.VBLF + "    AND (A.CHARTDATE || A.CHARTTIME )= (TM.CHARTDATE || TM.TIMEVALUE || '00') ";
                if ((strDuty == "Tot") || (strDuty == "Night"))
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
                }
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
                SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO ";
                SQL = SQL + ComNum.VBLF + "   AND  B.EMRNOHIS = A.EMRNOHIS";
                //SQL = SQL + ComNum.VBLF + "    AND REGEXP_INSTR(REPLACE(B.ITEMVALUE, '.', ''),'[^0-9]') = 0 "; //숫자인 것만
                //SQL = SQL + ComNum.VBLF + @"    AND REGEXP_INSTR(B.ITEMVALUE,'^?\d*(\.?\d*)$') = 1"; //소수점까지 체크가능
                SQL = SQL + ComNum.VBLF + @"   AND REGEXP_LIKE(B.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능

                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
                SQL = SQL + ComNum.VBLF + "   ON BC.BASCD = B.ITEMCD  ";
                SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = '" + mstrFormNameGb + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND BC.UNITCLS = '섭취배설' ";
                SQL = SQL + ComNum.VBLF + "   AND BC.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계' ";

                if ((strDuty == "Tot") || (strDuty == "Night"))
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY B.ITEMCD ";
            }
            #endregion

            #region 신규
            else
            {
                SQL = " WITH M AS  ";
                SQL = SQL + ComNum.VBLF + " (  ";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    B.ITEMCD,  ";
                SQL = SQL + ComNum.VBLF + "    SUM(DECODE(B.ITEMVALUE, NULL, 0, TO_NUMBER(B.ITEMVALUE))) AS ITEMVALUE , MAX(BC.BASNAME) AS ITEMNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBVITALTIME TM ";
                SQL = SQL + ComNum.VBLF + "    ON  A.ACPNO = TM.ACPNO ";
                SQL = SQL + ComNum.VBLF + "   AND TM.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "   AND TM.SUBGB = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND (A.CHARTDATE || A.CHARTTIME )= (TM.CHARTDATE || TM.TIMEVALUE || '00') ";
                if ((strDuty == "Tot") || (strDuty == "Night"))
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
                }
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
                SQL = SQL + ComNum.VBLF + "   ON  B.EMRNO = A.EMRNO ";
                SQL = SQL + ComNum.VBLF + "  AND  B.EMRNOHIS = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  AND  B.ITEMCD NOT IN ('I0000022324', 'I0000030622', 'I0000030623') -- 기존 수혈, 총섭취,배설량 삭제                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + @"  AND REGEXP_LIKE(B.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
                SQL = SQL + ComNum.VBLF + "   ON BC.BASCD = B.ITEMCD  ";
                SQL = SQL + ComNum.VBLF + "  AND BC.BSNSCLS = '" + mstrFormNameGb + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND BC.UNITCLS = '섭취배설' ";
                SQL = SQL + ComNum.VBLF + "  AND BC.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계' ";

                if ((strDuty == "Tot") || (strDuty == "Night"))
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY B.ITEMCD ";

                #region 혈액(수혈) 기록지 데이터 가져오기
                SQL = SQL + ComNum.VBLF + "    UNION ALL  --수혈 기록지에서 값 가저오기                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "    SELECT ITEMCD, SUM(ITEMVALUE) ITEMVALUE,  '혈액'";
                SQL = SQL + ComNum.VBLF + "    FROM                                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "        SELECT A.EMRNO                                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , A.EMRNOHIS                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "             , (REPLACE(REPLACE((SELECT ITEMVALUE                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "                                  FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                                 WHERE EMRNO = A.EMRNO                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                                   AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                                   AND ITEMCD = 'I0000037490'),'-',''),'/','')) AS CHARTDATE--수혈종료일자                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , REPLACE((SELECT ITEMVALUE                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "                          FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                         WHERE EMRNO = A.EMRNO                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                           AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                           AND ITEMCD = 'I0000037491'),':','')AS CHARTTIME  --수혈종료시간                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMCD                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMNO                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , TO_NUMBER(B.ITEMVALUE) AS  ITEMVALUE                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "         INNER JOIN  KOSMOS_EMR.AEMRCHARTROW B                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "            ON A.EMRNO = B.EMRNO                                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "           AND A.EMRNOHIS = B.EMRNOHIS";

                if (p.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "         WHERE A.ACPNO = " + p.acpNo;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         WHERE A.PTNO = '" + p.ptNo + "'";
                }
                SQL = SQL + ComNum.VBLF + "           AND A.FORMNO IN (1965, 3535)                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "           AND B.ITEMCD = 'I0000013528'                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "           AND MEDFRDATE = '" + p.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                                                           ";
                if ((strDuty == "Tot") || (strDuty == "Night"))
                {
                    SQL = SQL + ComNum.VBLF + " WHERE SUBSTR((CHARTDATE || CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((CHARTDATE || CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE SUBSTR((CHARTDATE || CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR((CHARTDATE || CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY ITEMCD ";
                SQL = SQL + ComNum.VBLF + " )  ";
                #endregion

                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + "      ITEMCD  ";
                SQL = SQL + ComNum.VBLF + "    , ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "    , ITEMNAME";
                SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";

                SQL = SQL + ComNum.VBLF + "UNION ALL--섭취계산                                                                                                                                                                                                 ";
                SQL = SQL + ComNum.VBLF + " SELECT                                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "       'I0000030622' AS ITEMCD                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , SUM(A.ITEMVALUE) AS ITEMVALUE                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "     , '총섭취량' AS USENAME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "   --AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "   --AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '01.섭취'                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.VFLAG3                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "--GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + " UNION  ALL -- 베설 계산                                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + " SELECT  ";
                SQL = SQL + ComNum.VBLF + "       'I0000030623' AS ITEMCD                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "     , SUM(A.ITEMVALUE) AS ITEMVALUE                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "     , '총배설량' AS USENAME                                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                                                                                                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "  -- AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "  -- AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '11.배설'                                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.VFLAG3                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "--GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "--ORDER BY CHARTDATE ASC , CHARTTIME ASC , EMRNO, UNITCLS, BASVAL                                                                                                                                                 ";
            }
            #endregion

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (ssIoTot_Sheet1.Cells[3, j].Text.Trim() == "합계")
                {
                    continue;
                }

                for (k = 4; k < ssIoTot_Sheet1.RowCount - 5; k++)
                {
                    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssIoTot_Sheet1.Cells[k, COL_ITEM].Text.Trim())
                    {
                        if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                        {
                            ssIoTot_Sheet1.Cells[k, j].BackColor = Color.LightCyan;
                            if (strDuty == "Tot")
                            {
                                ssIoTot_Sheet1.Cells[k, j].Font = BoldFont;
                            }
                        }
                        //if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) == 0)
                        //{
                        //    ssIoTot_Sheet1.Cells[k, j].Text = "";
                        //}
                        //else
                        //{
                            ssIoTot_Sheet1.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        //}
                }
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// IO 함계 스프래드 세팅
        /// </summary>
        private void SetIoTotDefault()
        {
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SetTopRow(ssIoTot_Sheet1);

            string strBASEXNAME = "";
            int intS = 0;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "   ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "  AND B.BSNSCLS   = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.UNITCLS   = '섭취배설'";
            SQL = SQL + ComNum.VBLF + "  AND B.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "   ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "  AND BB.BSNSCLS  = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "  AND BB.UNITCLS  = '섭취배설그룹'";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO    = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.ACPNO     = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.JOBGB = 'IIO'";
            //SQL = SQL + ComNum.VBLF + "ORDER BY B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
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
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssIoTot_Sheet1.RowCount += 1;

                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_JOB].Text = "IIO";

                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_ITEM].Text = dt.Rows[i]["BASCD"].ToString().Trim();

                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssIoTot_Sheet1.AddSpanCell(intS, 3, ssIoTot_Sheet1.RowCount - 1 - intS, 1);
                        }
                        intS = ssIoTot_Sheet1.RowCount - 1;
                    }


                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                    if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].BackColor = Color.LightCyan;
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].BackColor = Color.LightCyan;
                    }

                    ssIoTot_Sheet1.SetRowHeight(ssIoTot_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                }
                dt.Dispose();
                dt = null;

                ssIoTot_Sheet1.AddSpanCell(intS, 3, ssIoTot_Sheet1.RowCount - intS, 1);

                SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, strJOBGB);
            }
            else
            {
                dt.Dispose();
                dt = null;
                SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, strJOBGB);
            }

            ssIoTot_Sheet1.Columns[2].Visible = false;
            //ssIoTot_Sheet1.Columns[2].Visible = false;
        }

        #endregion IO 함수

        private void mbtnSaveAll_Click(object sender, EventArgs e)
        {
            mbtnSearchAll.Enabled = false;
            SetSave();
            mbtnSearchAll.Enabled = true;
        }

        /// <summary>
        /// 저장 버튼 함수
        /// </summary>
        private void SetSave()
        {
            pSaveData("1");

            if (mEmrCallForm != null)
            {
                mEmrCallForm.MsgSave("0");
            }

            #region 오늘 작성된 내역 전자인증 다시
            clsEmrFunc.NowEmrCert(clsDB.DbCon, p.medFrDate, p.ptNo);
            #endregion

            btnViewInpUser.Text = "작성자 보기";

            SetSpread();

            LoadJobDataSub();

            foreach (KeyValuePair<int, ExpendRowVal> kv in ExpendRows)
            {
                ExpendRow(null, kv.Key, kv.Value.strJOBGB, kv.Value.strJobExpend);
            }
        }

        private void mbtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "정말 삭제하시겠습니까?", "삭제") == DialogResult.No)
                return;

            SetSpread();

            if (mSpdView.Columns.Count <= 3) return;

            if (mSpdView.ActiveColumnIndex < 3) return;

            string strTime = mSpdView.Cells[2, mSpdView.ActiveColumnIndex].Text.Trim();

            if (CheckChangedData(mSpdView.ActiveColumnIndex, strTime) == true)
            {
                ComFunc.MsgBoxEx(this, strTime + "시간에 다른 사람이 작성한 내역이 존재 합니다. " + ComNum.VBLF + "삭제 할 수 없습니다.");
                return;
            }

            if (VB.Right(strTime, 2) == "00")
            {
                DeleteOne(mSpdView);
            }
            else
            {
                if (DeleteAll(mSpdView) == true)
                {
                    btnViewInpUser.Text = "작성자 보기";

                    LoadJobDataSub();
                }
            }

            ColVisible();
        }

        private void mbtnTotal_Click(object sender, EventArgs e)
        {
            if (panTot.Visible == true)
            {
                btnO2.Visible = mstrFormNo.Equals("2201") == false && mstrFormNo.Equals("1969") == false && mstrMode.Equals("W");
                panTot.Visible = false;
                panTotHead.Visible = false;
            }
            else
            {
                btnO2.Visible = false;
                panTot.Visible = true;
                panTotHead.Visible = true;

                if (dtpFrDateTot.Value == dtpFrDate.Value)
                {
                    GetIoTot();
                }
                else
                {
                    dtpFrDateTot.Value = dtpFrDate.Value;
                }
            }
        }

        private void GetIoTot()
        {
            SetIoTotDefault();

            LoadIoTot("Day", "050100", "130000");
            LoadIoTot("Eve", "130100", "210000");
            LoadIoTot("Night", "210100", "050000");
            LoadIoTot("Tot", "050100", "050000");
            //LoadIoTot("Tot", "070000", "065900");
        }

        private void btnRinger_Click(object sender, EventArgs e)
        {
            Ringer();
        }

        /// <summary>
        /// 수액기록지 버튼
        /// </summary>
        private void Ringer()
        {
            //pSaveData("1");

            //수액 아이템이 없으면 폼을 띄우지 않는다
            if (ExistsRinger() == false)
            {
                ComFunc.MsgBoxEx(this, "정맥주입 혹은 총섭취량 아이템이 없습니다." + ComNum.VBLF + "추가후 수액기록을 선택해 주십시요");
                return;
            }

            if (frmEmrBaseRingerIOX != null)
            {
                frmEmrBaseRingerIOX.Dispose();
                frmEmrBaseRingerIOX = null;
            }

            frmEmrBaseRingerIOX = new frmEmrBaseRingerIO(p, dtpFrDate.Value.ToString("yyyyMMdd"), mstrFormNo, mstrUpdateNo);
            frmEmrBaseRingerIOX.StartPosition = FormStartPosition.CenterParent;
            frmEmrBaseRingerIOX.FormClosed += FrmEmrBaseRingerIOX_FormClosed;
            frmEmrBaseRingerIOX.Show(this);
        }

        private void FrmEmrBaseRingerIOX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrBaseRingerIOX != null)
            {
                frmEmrBaseRingerIOX.Dispose();
                frmEmrBaseRingerIOX = null;
            }

            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.ColumnCount = 0;

            LoadJobData();

            ColVisible();
        }

        /// <summary>
        /// 해당일자에 수액 아이템이 있는지 확인한다
        /// </summary>
        /// <returns></returns>
        private bool ExistsRinger()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    ITEMCD ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = 'IIO'";
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = 'I0000030580'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
                dt.Dispose();
                dt = null;

                if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    ITEMCD ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "    AND JOBGB = 'IIO'";
                    SQL = SQL + ComNum.VBLF + "    AND ITEMCD = 'I0000030622'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return false;
                    }
                    dt.Dispose();
                    dt = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        #endregion IO관련

        #region Grape관련

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            GetVitalGraph();
        }

        private double WheelValue = 0;

        private void GetVitalGraph()
        {

            //데이터 조회
            if (p == null) return;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            List<GV> GVLsit = new List<GV>();
            List<string> XList = new List<string>();

            bool blnData = false;
            int i = 0;

            chartVital.Series.Clear();
            chartVital.Titles.Clear();
            chartVital.ChartAreas.Clear();

            chartVital.ChartAreas.Add("Default");
            chartVital.Titles.Add("Vital Sign");
            chartVital.Titles[0].Font = new Font("굴림", 16F, FontStyle.Bold);

            if ((chkSBP.Checked == false) && (chkPR.Checked == false) && (chkRR.Checked == false) && (chkBT.Checked == false))
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT                  ";
                SQL = SQL + ComNum.VBLF + "        A.CHARTDATE      ";
                SQL = SQL + ComNum.VBLF + "      , A.CHARTTIME      ";
                SQL = SQL + ComNum.VBLF + "      , B.ITEMCD         ";
                SQL = SQL + ComNum.VBLF + "      , B.ITEMVALUE      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "     ON B.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "    AND B.EMRNOHIS = A.EMRNOHIS";

                if (p.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + dtpGraphFDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + dtpGraphTDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.ITEMCD IN('I0000002018','I0000001765','I0000014815','I0000002009','I0000001811' ) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strDateTime = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4).Insert(2, "-") + "\r\n" + VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4).Insert(2, ":");

                    if (XList.IndexOf(strDateTime) == -1)
                    {
                        XList.Add(strDateTime);
                    }

                    if (chkSBP.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002018")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 100)
                                {
                                    GVLsit.Add(new GV()
                                    {
                                        Code = "SBP"
                                        ,
                                        X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                        ,
                                        Y = strDateTime
                                    }
                                    );
                                }
                                else 
                                {
                                    GVLsit.Add(new GV()
                                    {
                                        Code = "SBP2"
                                        ,
                                        X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                        ,
                                        Y = strDateTime
                                    }
                                    );
                                }
                            }
                        }

                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000001765")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "DBP"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkPR.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000014815")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "맥박"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkRR.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002009")
                        {

                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "호흡"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkBT.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000001811")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "체온"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (blnData == false) return;

                chartVital.ChartAreas["Default"].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 5, 85, 90);
                chartVital.ChartAreas["Default"].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(13, 5, 90, 90);

                if (chkSBP.Checked == true)
                {
                    chartVital.Series.Add("SBP");
                    chartVital.Series["SBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP.png";
                    chartVital.Series["SBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("DBP");
                    chartVital.Series["DBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["DBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\DBP.png";
                    chartVital.Series["DBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("SBP2");
                    chartVital.Series["SBP2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP2"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP2.png";
                    chartVital.Series["SBP2"].IsValueShownAsLabel = false;
                    chartVital.Series["SBP2"].IsVisibleInLegend = false;
                }

                if (chkPR.Checked == true)
                {
                    chartVital.Series.Add("맥박");
                    chartVital.Series["맥박"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["맥박"].BorderWidth = 2;
                    chartVital.Series["맥박"].Color = Color.Blue;
                    chartVital.Series["맥박"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["맥박"].MarkerColor = Color.Blue;
                    chartVital.Series["맥박"].MarkerSize = 6;
                }

                if (chkRR.Checked == true)
                {
                    chartVital.Series.Add("호흡");
                    chartVital.Series["호흡"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["호흡"].BorderWidth = 2;
                    chartVital.Series["호흡"].Color = Color.Green;
                    chartVital.Series["호흡"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["호흡"].MarkerColor = Color.Green;
                    chartVital.Series["호흡"].MarkerSize = 6;
                }

                if (chkBT.Checked == true)
                {
                    chartVital.Series.Add("체온");
                    chartVital.Series["체온"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["체온"].BorderWidth = 2;
                    chartVital.Series["체온"].Color = Color.Red;
                    chartVital.Series["체온"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["체온"].MarkerColor = Color.Red;
                    chartVital.Series["체온"].MarkerSize = 6;
                }

                chartVital.Series.Add("주의선");
                chartVital.Series["주의선"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartVital.Series["주의선"].BorderWidth = 2;
                chartVital.Series["주의선"].Color = Color.Orange;
                chartVital.Series["주의선"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;

                // X축 그리기

                XList.Sort();

                foreach (string DateTiem in XList)
                {
                    if (GVLsit.Where(d => d.Y == DateTiem).Any())
                    {
                        List<GV> list = GVLsit.Where(d => d.Y == DateTiem).ToList();

                        foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chartVital.Series)
                        {
                            if (list.Where(d => d.Code == series.Name).Any())
                            {
                                series.Points.AddY(list.Where(d => d.Code == series.Name).First().X);
                            }
                            else
                            {
                                if (series.Name == "주의선")
                                {
                                    series.Points.AddY(100);
                                    continue;
                                }

                                series.Points.AddY(double.NaN);
                                series.Points[series.Points.Count - 1].IsEmpty = true;
                            }
                        }
                        chartVital.Series[0].Points[chartVital.Series[0].Points.Count - 1].AxisLabel = DateTiem;
                    }
                }

                chartVital.ChartAreas["Default"].AxisX.Interval = 1;
                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                chartVital.ChartAreas["Default"].AxisY.Minimum = 40; //30
                chartVital.ChartAreas["Default"].AxisY.Maximum = 250; //250
                chartVital.ChartAreas["Default"].Position.X = 12;
                chartVital.ChartAreas["Default"].InnerPlotPosition.X = 2;
                chartVital.ChartAreas["Default"].AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.ScrollBar.Enabled = true;
                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;

                chartVital.Series["주의선"].ChartArea = "Default";

                CheckBox[] checkAee = new CheckBox[4];

                checkAee[0] = chkSBP;
                checkAee[1] = chkPR;
                checkAee[2] = chkRR;
                checkAee[3] = chkBT;

                int f = 1;

                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {

                        if (box == chkPR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 30;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 150;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Blue;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Blue;

                            }

                            f += 1;

                        }

                        if (box == chkRR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 1;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 1;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 10;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 40;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Green;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Green;
                            }

                            f += 1;

                        }

                        if (box == chkBT)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 34.0;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 45.0;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Red;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Red;
                            }

                            f += 1;

                        }

                    }
                }

                if (chkSBP.Checked == false)
                {
                    f = f - 1;
                }

                int PositionX = 0; // Y축범위 가로 간격

                if (chartVital.Width <= 970)
                {
                    PositionX = 4;
                }
                else
                {
                    PositionX = 3;
                }

                chartVital.ChartAreas["Default"].Position.X = PositionX * f;


                f = 1;

                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {
                        if (box == chkRR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Green)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["호흡"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkPR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Blue)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["맥박"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkBT)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Red)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], PositionX * f, 2);
                            f += 1;
                        }
                    }
                }

                chartVital.ChartAreas["Default"].AxisY.LineWidth = 2;

                if (chartVital.ChartAreas.Count > 0)
                {
                    for (int k = 1; k < chartVital.ChartAreas.Count; k++)
                    {
                        if (chartVital.ChartAreas[k].Name.Split('_')[1] == "맥박")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 10;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 10;
                            chartVital.ChartAreas[k].AxisY.Minimum = 30;
                            chartVital.ChartAreas[k].AxisY.Maximum = 150;
                        }
                        else if (chartVital.ChartAreas[k].Name.Split('_')[1] == "체온")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 0.5;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 34.0;
                            chartVital.ChartAreas[k].AxisY.Maximum = 45.0;
                        }
                        else if (chartVital.ChartAreas[k].Name.Split('_')[1] == "호흡")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 1;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 10;
                            chartVital.ChartAreas[k].AxisY.Maximum = 40;
                        }
                    }
                }

                WheelValue = 21;

                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

                if (chartVital.Series[0].Points.Count > WheelValue)
                {
                    chartVital.ChartAreas["Default"].AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                }

                foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
                {
                    if (item == chartVital.ChartAreas["Default"])
                        continue;

                    item.AxisX.ScrollBar.Enabled = true;
                    item.AxisX.ScaleView.Zoomable = true;
                    item.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
                    item.AxisX.ScrollBar.ChartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    item.AxisX.ScrollBar.LineColor = Color.Transparent;
                    item.AxisX.ScaleView.Zoom(0, WheelValue);
                    
                    if (chartVital.Series[0].Points.Count > WheelValue)
                    {
                        item.AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                    }

                }
                //=========
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);

            }
            Cursor.Current = Cursors.Default;
        }

        private void chartVital_MouseWheel(object sender, MouseEventArgs e)
        {



            if (e.Delta < 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum < WheelValue + 1)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum;
                }
                else
                {
                    WheelValue = WheelValue + 1;
                }


            }

            if (e.Delta > 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum > WheelValue)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum;
                }
                else
                {
                    WheelValue = WheelValue - 1;
                }


            }

            chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

            foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            {
                if (item == chartVital.ChartAreas["Default"])
                    continue;

                item.AxisX.ScaleView.Zoom(0, WheelValue);
            }

        }

        private void chartVital_DoubleClick(object sender, EventArgs e)
        {
            //chartVital.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();

            //foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            //{
            //    if (item == chartVital.ChartAreas["Default"])
            //        continue;

            //    item.AxisX.ScaleView.ZoomReset();
            //}
        }

        public void CreateYAxis(System.Windows.Forms.DataVisualization.Charting.Chart chart, System.Windows.Forms.DataVisualization.Charting.ChartArea area,
            System.Windows.Forms.DataVisualization.Charting.Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            System.Windows.Forms.DataVisualization.Charting.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;


            series.ChartArea = areaSeries.Name;

            if (series.Name == "체온"
                || series.Name == "맥박"
                || series.Name == "호흡")
            {

                // Create new chart area for axis
                System.Windows.Forms.DataVisualization.Charting.ChartArea areaAxis = chart.ChartAreas.Add("AxisY-" + series.ChartArea);
                areaAxis.BackColor = Color.Transparent;
                areaAxis.BorderColor = Color.Transparent;
                areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
                areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

                // Create a copy of specified series
                System.Windows.Forms.DataVisualization.Charting.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
                seriesCopy.ChartType = series.ChartType;
                foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in series.Points)
                {
                    seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                }

                // Hide copied series
                seriesCopy.IsVisibleInLegend = false;
                seriesCopy.Color = Color.Transparent;
                seriesCopy.BorderColor = Color.Transparent;
                seriesCopy.ChartArea = areaAxis.Name;

                // Disable drid lines & tickmarks
                areaAxis.AxisX.LineWidth = 0;
                areaAxis.AxisX.MajorGrid.Enabled = false;
                areaAxis.AxisX.MajorTickMark.Enabled = false;
                areaAxis.AxisX.LabelStyle.Enabled = false;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
                areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

                if (series.Name == "체온")
                {
                    areaAxis.AxisY.LineColor = Color.Red;
                    areaAxis.AxisY.InterlacedColor = Color.Red;
                }
                else if (series.Name == "맥박")
                {
                    areaAxis.AxisY.LineColor = Color.Blue;
                    areaAxis.AxisY.InterlacedColor = Color.Blue;
                }
                else if (series.Name == "호흡")
                {
                    areaAxis.AxisY.LineColor = Color.Green;
                    areaAxis.AxisY.InterlacedColor = Color.Green;
                }

                areaAxis.AxisY.LineWidth = 2;

                // Adjust area position
                areaAxis.Position.X = axisOffset;
                areaAxis.InnerPlotPosition.X = labelsSize;
            }

        }


        #endregion Grape관련

        #region //기타

        private void btnRegBaseValue_Click(object sender, EventArgs e)
        {
            if (clsType.User.IdNumber.Equals("8822")  == false &&
                clsType.User.IdNumber.Equals("14472") == false)
            {
                ComFunc.MsgBoxEx(this, "해당 값 설정은 간호부 팀장님들께 문의 부탁드립니다.");
                return;
            }

            using (frmEmrBaseItemSetBaseValue frm = new frmEmrBaseItemSetBaseValue("IVT"))
            {
                frm.TopMost = true;
                frm.ShowDialog(this);
            }
        }

        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            if (panGraph.Visible == false)
            {

                dtpGraphFDate.Value = dtpFrDate.Value;
                dtpGraphTDate.Value = dtpFrDate.Value;

                GetVitalGraph();
                panGraph.Height = 480;
                panGraph.Visible = true;
                chartVital.Size = panGraphSub.Size;

            }
            else
            {
                panGraph.Visible = false;
            }
        }

        private void btnHideGraph_Click(object sender, EventArgs e)
        {
            panGraph.Visible = false;
        }

        #endregion //기타

        private void ssVital_KeyDown(object sender, KeyEventArgs e)
        {
            if (ssVital_Sheet1.Rows.Count == 0) return;

            if (e.KeyCode != Keys.Return) return; //   COL_DATAS)

            if (ssVital_Sheet1.ActiveColumnIndex < COL_DATAS) return;

            if (ssVital_Sheet1.ActiveRowIndex + 1 == ssVital_Sheet1.Rows.Count - 1) return;

            if (ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex + 1, COL_EXP].Text.Trim() == "+" || ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex + 1, COL_EXP].Text.Trim() == "-")
            {
                if (ssVital_Sheet1.ActiveRowIndex + 1 == ssVital_Sheet1.Rows.Count - 1) return;
                ssVital_Sheet1.SetActiveCell(ssVital_Sheet1.ActiveRowIndex + 1, ssVital_Sheet1.ActiveColumnIndex);
                ssVital.ShowRow(0, ssVital_Sheet1.ActiveRowIndex + 1, FarPoint.Win.Spread.VerticalPosition.Nearest);
                return;
            }
            else
            {
                //ssVital_Sheet1.SetActiveCell(ssVital_Sheet1.ActiveRowIndex + 1, ssVital_Sheet1.ActiveColumnIndex);
                //ssVital.ShowRow(0, ssVital_Sheet1.ActiveRowIndex + 1, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        private void mbtnUp_Click(object sender, EventArgs e)
        {
            panOrder.Height = 250;
        }

        private void mbtnDown_Click(object sender, EventArgs e)
        {
            panOrder.Height = 33;
        }

        private void ssVital_EditModeOff(object sender, EventArgs e)
        {
            if (ssVital_Sheet1.Rows.Count == 0) return;

            ssVital_Sheet1.Cells[ssVital_Sheet1.Rows.Count - 3, ssVital_Sheet1.ActiveColumnIndex].Text = clsType.User.IdNumber;

            if (ssVital_Sheet1.ActiveRowIndex == 2)
            {
                if (SaveTimeSet(mSpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                }
            }

            if (ssVital_Sheet1.ActiveColumnIndex < COL_DATAS) return;

            if (ssVital_Sheet1.ActiveRowIndex + 1 == ssVital_Sheet1.Rows.Count - 1) return;

            if (ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex + 1, COL_EXP].Text.Trim() == "+" || ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex + 1, COL_EXP].Text.Trim() == "-")
            {
                if (ssVital_Sheet1.ActiveRowIndex + 2 == ssVital_Sheet1.Rows.Count - 1) return;
                ssVital_Sheet1.SetActiveCell(ssVital_Sheet1.ActiveRowIndex + 2, ssVital_Sheet1.ActiveColumnIndex);
                ssVital.ShowRow(0, ssVital_Sheet1.ActiveRowIndex + 2, FarPoint.Win.Spread.VerticalPosition.Nearest);
                return;
            }
            else
            {
                ssVital_Sheet1.SetActiveCell(ssVital_Sheet1.ActiveRowIndex + 1, ssVital_Sheet1.ActiveColumnIndex);
                ssVital.ShowRow(0, ssVital_Sheet1.ActiveRowIndex + 1, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }
        }

        private void btnSaveWard_Click(object sender, EventArgs e)
        {
            using (frmEmrBasVitalSetWard frmEmrBasVitalSetWardX = new frmEmrBasVitalSetWard())
            {
                frmEmrBasVitalSetWardX.ShowDialog(this);
            }
        }

        private void mbtnNrHistory_Click(object sender, EventArgs e)
        {
            LoadBeforeDataIVT_ALL(ssVital_Sheet1, "IVT");
        }

        private void ssVital_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //정맥주입
            if (ssVital_Sheet1.Cells[e.Row, COL_ITEM].Text.Trim() == "I0000030580")
            {
                Ringer();
            }
        }

        private void btnSearchOrder_Click(object sender, EventArgs e)
        {
            pGetInjectOrderInfo();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CheckItemExists(dtpFrDate.Value.ToString("yyyyMMdd"));

            if (ssVital_Sheet1.ColumnCount > COL_DATAS)
            {
                ssVital_Sheet1.Cells[4, COL_DATAS, ssVital_Sheet1.RowCount - 4, ssVital_Sheet1.ColumnCount - 1].ForeColor = Color.Black;
            }

            pClearFormExcept();

            pLoadEmrChartInfo();

            pGetInjectOrderInfo();
        }

        private void btnViewInpUser_Click(object sender, EventArgs e)
        {
            string strFlag = "보기";

            if (btnViewInpUser.Text.Trim() == "작성자 보기")
            {
                strFlag = "보기";
                btnViewInpUser.Text = "작성자 숨김";
            }
            else
            {
                strFlag = "숨김";
                btnViewInpUser.Text = "작성자 보기";
            }

            int Row = 0;
            int Column = 0;
            Font font = new Font("굴림", 10);

            for (Row = 4; Row <= ssVital_Sheet1.RowCount - 5; Row++)
            {
                for (Column = 4; Column < ssVital_Sheet1.ColumnCount; Column++)
                {
                    if (ssVital_Sheet1.Cells[Row, Column].Tag != null)
                    {
                        if (ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim() != "")
                        {
                            if (ssVital_Sheet1.Cells[Row, Column].Text.Trim() != "")
                            {
                                if (strFlag == "보기")
                                {
                                    //ssVital_Sheet1.Cells[Row, Column].Text = ssVital_Sheet1.Cells[Row, Column].Text.Trim() + ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();
                                    //string strUseName = ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();

                                    string strUseName = ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 0, "/").Trim();
                                    strUseName = strUseName + ComNum.VBLF;
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 2, "/").Trim(), "D") + " ";
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 3, "/").Trim(), "M");

                                    //Size TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    //List<int> lstWidth = new List<int>();
                                    //lstWidth.Add(TxtSize.Width);

                                    StringBuilder strNote = new StringBuilder();
                                    strNote.AppendLine(strUseName);
                                    ////텍스트길이 계산
                                    //TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    ////List에 넣기
                                    //lstWidth.Add(TxtSize.Width);

                                    ssVital_Sheet1.Cells[Row, Column].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                                    ssVital_Sheet1.Cells[Row, Column].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                                    ssVital_Sheet1.Cells[Row, Column].NoteIndicatorSize = new Size(9, 9);
                                    ssVital_Sheet1.Cells[Row, Column].Note = strNote.ToString().Trim();
                                    string strINPUSEID = ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();
                                    if (strINPUSEID != clsType.User.IdNumber)
                                    {
                                        ssVital_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.Pink;
                                    }
                                    else
                                    {
                                        ssVital_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.SkyBlue;
                                    }
                                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                                    nsinfo = ssVital_Sheet1.GetStickyNoteStyleInfo(Row, Column);

                                    nsinfo.Font = font;
                                    nsinfo.ForeColor = Color.Black;
                                    nsinfo.Width = 100; // lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
                                    nsinfo.ShapeOutlineColor = Color.Red;
                                    nsinfo.ShapeOutlineThickness = 1;
                                    nsinfo.ShadowOffsetX = 3;
                                    nsinfo.ShadowOffsetY = 3;
                                    ssVital_Sheet1.SetStickyNoteStyleInfo(Row, 4, nsinfo);
                                }
                                else
                                {
                                    ssVital_Sheet1.Cells[Row, Column].Note = string.Empty;
                                    ssVital_Sheet1.Cells[Row, Column].Note = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region 인공신장실 인터페이스 항목 연동
        /// <summary>
        /// 인공 신장실 인터페이스 연동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInterface_Click(object sender, EventArgs e)
        {
            if (fEmrInterface != null)
            {
                fEmrInterface.Dispose();
                fEmrInterface = null;
            }

            if (mstrFormNo.Equals("2201"))
            {
                fEmrInterface = new frmHemodialysisInterface("VITAL", p.ptNo);
                fEmrInterface.FormClosed += fEmrHemodialysisInterface_FormClosed;
                (fEmrInterface as frmHemodialysisInterface).rSendInterface2 += fEmrHemodialysisInterface_rSendInterface;
                fEmrInterface.Show(this);
            }
            else
            {
                string strTime = string.Empty;
                if (sender is MenuItem)
                {
                    strTime = (sender as MenuItem).Tag == null ? string.Empty : (sender as MenuItem).Tag.ToString();
                }

                fEmrInterface = new frmPatientMonitorInterface(p, dtpFrDate.Value.ToString("yyyyMMdd"), strTime);
                fEmrInterface.FormClosed += fEmrHemodialysisInterface_FormClosed;
                (fEmrInterface as frmPatientMonitorInterface).rSendInterface += FrmEmrVitalSign_rSendInterface;
                fEmrInterface.Show(this);
            }

            
        }

        private void FrmEmrVitalSign_rSendInterface(Dictionary<string, string> strData)
        {
            if (strData.Count == 0)
                return;

            for (int i = 4; i < ssVital_Sheet1.RowCount; i++)
            {
                string Val = string.Empty;
                int nCol = -1;
                if (strData.TryGetValue("측정시간", out Val))
                {
                    for (int j = 0; j < mSpdView.ColumnCount; j++)
                    {
                        //이미 추가되어있는 시간.
                        if (ssVital_Sheet1.Cells[2, j].Text.Trim() == Val)
                        {
                            nCol = j;
                            break;
                        }
                    }

                    if (nCol == -1)
                    {
                        SaveTime(Val);//시간 추가(있으면 추가안됨)
                    }

                    if (strData.TryGetValue(ssVital_Sheet1.Cells[i, COL_ITEM].Text, out Val))
                    {
                        if (string.IsNullOrWhiteSpace(Val))
                            continue;

                        ssVital_Sheet1.Cells[i, nCol].Text = Val;
                        ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, COL_DATAS].Text = "Y";
                        ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, nCol].Text = "Y";
                    }
                }
            }
        }

        private void fEmrHemodialysisInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrInterface != null)
            {
                fEmrInterface.Dispose();
                fEmrInterface = null;
            }
        }

        private void fEmrHemodialysisInterface_rSendInterface(Dictionary<string, string> strData)
        {
            if (fEmrInterface != null)
            {
                fEmrInterface.Dispose();
                fEmrInterface = null;
            }

            if (strData.Count == 0)
                return;

            for (int i = 4; i < ssVital_Sheet1.RowCount; i++)
            {
                string Val = string.Empty;
                int nCol = -1;
                if (strData.TryGetValue("측정시간", out Val))
                {
                    for (int j = 0; j < mSpdView.ColumnCount; j++)
                    {
                        //이미 추가되어있는 시간.
                        if (ssVital_Sheet1.Cells[2, j].Text.Trim() == Val)
                        {
                            nCol = j;
                            break;
                        }
                    }



                    string Val2 = string.Empty;
                    if (strData.Count == 2 && strData.TryGetValue("I0000035472", out Val2) && string.IsNullOrWhiteSpace(Val2) == false)
                    {
                        if (ssVital_Sheet1.Cells[i, COL_ITEM].Text.Equals("I0000035472"))
                        {
                            ssVital_Sheet1.Cells[i, COL_DATAS].Text = Val2;
                            ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, COL_DATAS].Text = "Y";
                            return;
                        }
                    }
                    else
                    {
                        if (nCol == -1)
                        {
                            SaveTime(Val);//시간 추가(있으면 추가안됨)
                        }
                    }
                }

                if (strData.TryGetValue(ssVital_Sheet1.Cells[i, COL_ITEM].Text, out Val))
                {
                    if (string.IsNullOrWhiteSpace(Val))
                        continue;

                    //UFR
                    if (ssVital_Sheet1.Cells[i, COL_ITEM].Text.Equals("I0000035472"))
                    {
                        ssVital_Sheet1.Cells[i, COL_DATAS].Text = Val;
                        ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, COL_DATAS].Text = "Y";
                        continue;
                    }

                    ssVital_Sheet1.Cells[i, nCol].Text = Val;
                    ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, nCol].Text = "Y";
                }
            }

        }
        #endregion

        private void mbtnBeforeTot_Click(object sender, EventArgs e)
        {
            dtpFrDateTot.Value = dtpFrDateTot.Value.AddDays(-1);
        }

        private void mbtnNextTot_Click(object sender, EventArgs e)
        {
            dtpFrDateTot.Value = dtpFrDateTot.Value.AddDays(+1);
        }

        private void dtpFrDateTot_ValueChanged(object sender, EventArgs e)
        {
            GetIoTot();
        }

        private void btnSearchTot_Click(object sender, EventArgs e)
        {
            GetIoTot();
        }

        private void btnBST_Click(object sender, EventArgs e)
        {
            string strEmrNo = "0";
            string strFormNo = "1572";
            double dUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo));
            //ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
            using (frmEmrChartFlowOld fEmrChartFlowOld = new frmEmrChartFlowOld(strFormNo, dUpdateNo.ToString(), p, strEmrNo, "W", this))
            {
                fEmrChartFlowOld.StartPosition = FormStartPosition.CenterScreen;
                fEmrChartFlowOld.ShowDialog(this);
            }
        }

        /// <summary>
        /// 숫자만 입력해야 하는데 문자를 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssVital_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            if (ssVital_Sheet1.ActiveRowIndex <= 4) return;
            if (ssVital_Sheet1.ActiveColumnIndex < mintTCol) return;

            string strITEMCD = ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex, COL_ITEM].Text.Trim();
            string strValue = ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex, ssVital_Sheet1.ActiveColumnIndex].Text.Trim();

            if (strValue != "")
            {
                if (clsEmrQueryEtc.InputVlaueType(clsDB.DbCon, "", "", strITEMCD) == "N")
                {
                    if (VB.IsNumeric(strValue) == true)
                    {
                        return;
                    }
                    //strValue = Regex.Replace(strValue, @"[^0-9]", "");
                    //ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex, ssVital_Sheet1.ActiveColumnIndex].Text = strValue;
                    ComFunc.MsgBoxEx(this, "숫자만 입력이 가능합니다");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (frmEmrBaseRingerIOViewer frm = new frmEmrBaseRingerIOViewer(p, dtpFrDate.Value.ToString("yyyyMMdd"), mstrFormNo, mstrUpdateNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnBlood_Click(object sender, EventArgs e)
        {
            string strEmrNo = "0";
            string strFormNo = "1965";
            double dUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo));
            //ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
            using (frmEmrBloodInfo fEmrChartFlowOld = new frmEmrBloodInfo(strFormNo, dUpdateNo.ToString(), p, strEmrNo, "W", this))
            {
                fEmrChartFlowOld.StartPosition = FormStartPosition.CenterScreen;
                fEmrChartFlowOld.ShowDialog(this);
            }
        }

        private void btnMemo_Click(object sender, EventArgs e)
        {
            if (frmIcuMemo != null)
            {
                frmIcuMemo.Dispose();
                frmIcuMemo = null;
            }

            Screen screen = Screen.FromControl(this);
            frmIcuMemo = new frmEmrBaseNurseMemo(p);
            frmIcuMemo.Location = new Point(screen.WorkingArea.Right - frmIcuMemo.Width, 300);
            frmIcuMemo.StartPosition = FormStartPosition.Manual;
            frmIcuMemo.FormClosed += FrmIcuMemo_FormClosed;
            frmIcuMemo.Show(this);
        }

        private void FrmIcuMemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmIcuMemo != null)
            {
                frmIcuMemo.Dispose();
                frmIcuMemo = null;
            }
        }

        private void ssVital_ClipboardPasted(object sender, FarPoint.Win.Spread.ClipboardPastedEventArgs e)
        {
            if (e.CellRange.Column >= COL_DATAS)
            {                
                if (ssVital_Sheet1.Cells[e.CellRange.Row, COL_JOB].Text.Trim().Equals("IIO"))
                {
                    SumIntakeOutPutRow(e.CellRange.Column);
                }
            }
        }

        private void frmEmrVitalSign_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrBaseRingerIOX  != null)
            {
                frmEmrBaseRingerIOX.Dispose();
                frmEmrBaseRingerIOX = null;
            }
            if (frmIcuMemo != null)
            {
                frmIcuMemo.Dispose();
                frmIcuMemo = null;
            }

            if (frmO2 != null)
            {
                frmO2.Dispose();
                frmO2 = null;
            }
        }

        private void btnHDView_Click(object sender, EventArgs e)
        {
            using(frmHDRecordView frm = new frmHDRecordView(p))
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssVital_ClipboardPasting(object sender, FarPoint.Win.Spread.ClipboardPastingEventArgs e)
        {
            if (ssVital_Sheet1.ActiveColumnIndex >= COL_DATAS &&
                (ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex, COL_ITEM].Text.Equals("I0000030580") ||
                 ssVital_Sheet1.Cells[ssVital_Sheet1.ActiveRowIndex, COL_ITEM].Text.Equals("I0000022324")))
            {
                e.Handled = true;
            }
        }

        private void btnO2_Click(object sender, EventArgs e)
        {
            if (frmO2 != null)
            {
                frmO2.Dispose();
                frmO2 = null;
            }

            frmO2 = new frmDaySugaInterface();
            frmO2.StartPosition = FormStartPosition.CenterScreen;
            frmO2.FormClosed += FrmO2_FormClosed;
            frmO2.Show();
        }

        private void FrmO2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmO2 != null)
            {
                frmO2.Dispose();
                frmO2 = null;
            }
        }
    }
}

