using System;
using System.Collections.Generic;
using System.ComponentModel;
using ComBase;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmAnFormMapping : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetPatientInfo(OpSchedule opInfo);
        public event GetPatientInfo rGetPatientInfo;

        //폼이 Close 될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;


        OpSchedule opInfo = null;
        string mstrPano = string.Empty;
        string mstrInDate = string.Empty;

        public frmAnFormMapping()
        {
            InitializeComponent();
        }

        public frmAnFormMapping(string strPano, string strInDate)
        {
            InitializeComponent();
            mstrPano = strPano;
            mstrInDate = strInDate;
        }

        private void frmAnFormMapping_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
                SQL = SQL + ComNum.VBLF + "    PANO, TO_CHAR(OPDATE,'YYYY-MM-DD') AS OPDATE, DEPTCODE, FLAG, OPROOM, DIAGNOSIS, OPTITLE,    ";
                SQL = SQL + ComNum.VBLF + "    OPDOCT1, OPNURSE, OPBUN, OPCODE, OPPOSITION, WRTNO           ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ORAN_MASTER                                    ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mstrPano + "'                                  ";
                SQL = SQL + ComNum.VBLF + "  AND OPDATE >= " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(mstrInDate, "D", "-"), "D");
                //TODO
                //SQL = SQL + ComNum.VBLF + "WHERE PANO = '09152596'                                  ";
                //SQL = SQL + ComNum.VBLF + "  AND OPDATE >= " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx("20190621", "D", "-"), "D");

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                ssView.ActiveSheet.Rows.Count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OPDATE"].ToString();
                    ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DIAGNOSIS"].ToString();
                    ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OPTITLE"].ToString();
                    ssView.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["OPDOCT1"].ToString();
                    ssView.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["OPNURSE"].ToString();
                    ssView.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString();
                    ssView.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["FLAG"].ToString();
                    ssView.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["WRTNO"].ToString();
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            opInfo = new OpSchedule();

            // 수술스케줄 데이터
            ReadOpSchedule(ssView.ActiveSheet.Cells[e.Row, 7].Text);
            // VITAL SIGN
            ReadVitalSign(mstrPano, mstrInDate);
            // PREOP-CHECK LIST 데이터
            ReadPreOpCheckList(mstrPano, ssView.ActiveSheet.Cells[e.Row, 0].Text);
            // 마취전 평가서 과거력 데이터
            // 2020-04-28 간호정보조사지 과거력 데이터 가져오기
            ReadPastHxList(mstrPano, mstrInDate);
            // LAB 결과 데이터
            ReadExamResult(mstrPano, ssView.ActiveSheet.Cells[e.Row, 0].Text);
            // 혈액형
            READ_ABO(mstrPano);

            rGetPatientInfo(opInfo);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void ReadOpSchedule(string strWrtNo)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                       ";
                SQL = SQL + ComNum.VBLF + "    s.OPDATE, m.OPROOM, m.ASAADD, m.GBER,                    ";
                SQL = SQL + ComNum.VBLF + "    s.PREDIAGNOSIS AS PreOpDx, m.DIAGNOSIS AS PostOpDx,      ";
                SQL = SQL + ComNum.VBLF + "    s.OPILL AS PreOpTitle, m.OPTITLE AS PostOpTitle,         ";
                SQL = SQL + ComNum.VBLF + "    m.ANDOCT1,                                               ";
                SQL = SQL + ComNum.VBLF + "    m.OPDOCT1 AS Surgeon,                                   ";
                SQL = SQL + ComNum.VBLF + "    m.OPDOCT2 AS Assist,                                   ";  
                //m.OPDOCT1 || '/' || m.OPDOCT2 AS Surgeon,     ";
                SQL = SQL + ComNum.VBLF + "    m.ANNURSE AS AnesNr,                                     ";
                SQL = SQL + ComNum.VBLF + "    m.OPNURSE AS ScrubNr,                                    ";
                SQL = SQL + ComNum.VBLF + "    m.CNurse AS CirNr,	                                    ";
                SQL = SQL + ComNum.VBLF + "    s.WRTNO                                                  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_OPSCHE s                                 ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.ORAN_MASTER m                    ";
                SQL = SQL + ComNum.VBLF + "    ON s.WRTNO = m.WRTNO                                     ";
                SQL = SQL + ComNum.VBLF + "WHERE s.WRTNO = '" + strWrtNo + "'                           ";
                SQL = SQL + ComNum.VBLF + "  AND s.GBDEL <> '*'                                         ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    // 수술일자
                    opInfo.OpDate = dt.Rows[0]["OPDATE"].ToString();
                    // 수술실번호
                    opInfo.OpRoom = dt.Rows[0]["OPROOM"].ToString();
                    // ASA 점수
                    opInfo.AsaAdd = dt.Rows[0]["ASAADD"].ToString();
                    // 응급여부
                    opInfo.Emergency = dt.Rows[0]["GBER"].ToString();
                    // 수술전 진단명
                    opInfo.PreOpDx = dt.Rows[0]["PreOpDx"].ToString();
                    // 수술전 수술명
                    opInfo.PreOpTitle = dt.Rows[0]["PreOpTitle"].ToString();
                    // 수술후 진단명
                    opInfo.PostOpDx = dt.Rows[0]["PostOpDx"].ToString();
                    // 수술후 수술명
                    opInfo.PostOpTitle = dt.Rows[0]["PostOpTitle"].ToString();

                    // 마취의
                    opInfo.AnesDr = ReadSabun("0", dt.Rows[0]["ANDOCT1"].ToString()) + "/" + dt.Rows[0]["ANDOCT1"].ToString();
                    // 집도의
                    opInfo.Surgeon = ReadSabun("3", dt.Rows[0]["Surgeon"].ToString()) + "/" + dt.Rows[0]["Surgeon"].ToString();
                    // 어시스트
                    opInfo.Assist = ReadSabun("3", dt.Rows[0]["Assist"].ToString()) + "/" + dt.Rows[0]["Assist"].ToString();
                    // 마취간호사
                    opInfo.AnesNr = ReadSabun("1", dt.Rows[0]["AnesNr"].ToString()) + "/" + dt.Rows[0]["AnesNr"].ToString();
                    // 수술간호사
                    opInfo.ScrubNr = ReadSabun("1", dt.Rows[0]["ScrubNr"].ToString()) + "/" + dt.Rows[0]["ScrubNr"].ToString();
                    // 순환간호사
                    opInfo.CirNr = ReadSabun("2", dt.Rows[0]["CirNr"].ToString()) + "/" + dt.Rows[0]["CirNr"].ToString();

                    opInfo.WrtNo = dt.Rows[0]["WRTNO"].ToString();
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ReadPreOpCheckList(string strPano, string strChartDate)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT                                                       ";
                //SQL = SQL + ComNum.VBLF + "    EMRNO,                                                   ";
                //SQL = SQL + ComNum.VBLF + "    PTNO,                                                    ";
                //SQL = SQL + ComNum.VBLF + "    CHARTDATE,                                               ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it2') || ':' || extractValue(chartxml, '//it3') LEAVE,     ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it4') || ':' || extractValue(chartxml, '//it5') ARRIVE,    ";                
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it19') BP,	    -- 혈압         ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it18') PR,	    -- 맥박         ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it10') BW,	    -- 체중         ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//it17') BT,     -- 체온         ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//ir64') NPO_Y,                  ";
                //SQL = SQL + ComNum.VBLF + "    extractValue(chartxml, '//ir65') NPO_N                   ";
                //SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML                                      ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "      EMRNO ";
                SQL = SQL + ComNum.VBLF + "    , PTNO ";
                SQL = SQL + ComNum.VBLF + "    , CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "    , ((SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW                 ";
                SQL = SQL + ComNum.VBLF + "         WHERE ITEMCD = 'I0000034601' AND EMRNO = M.EMRNO) || ':' || ";
                SQL = SQL + ComNum.VBLF + "       (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW ";
                SQL = SQL + ComNum.VBLF + "         WHERE ITEMCD = 'I0000034602' AND EMRNO = M.EMRNO) ";
                SQL = SQL + ComNum.VBLF + "      ) LEAVE     -- 병실 출발시간 ";
                SQL = SQL + ComNum.VBLF + "    , ((SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW                 ";
                SQL = SQL + ComNum.VBLF + "         WHERE ITEMCD = 'I0000034603' AND EMRNO = M.EMRNO) || ':' || ";
                SQL = SQL + ComNum.VBLF + "       (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW ";
                SQL = SQL + ComNum.VBLF + "         WHERE ITEMCD = 'I0000034604' AND EMRNO = M.EMRNO) ";
                SQL = SQL + ComNum.VBLF + "      ) ARRIVE    -- 수술장 도착시간 ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW                              ";
                SQL = SQL + ComNum.VBLF + "        WHERE ITEMCD = 'I0000000418' AND EMRNO = M.EMRNO) BW      -- 체중         ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW  ";
                SQL = SQL + ComNum.VBLF + "        WHERE ITEMCD = 'I0000034631' AND EMRNO = M.EMRNO) NPO_Y   -- 금식여부Y    ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW  ";
                SQL = SQL + ComNum.VBLF + "        WHERE ITEMCD = 'I0000034632' AND EMRNO = M.EMRNO) NPO_N   -- 금식여부N    ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.AEMRCHARTMST M ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = '1808'                                        ";
                SQL = SQL + ComNum.VBLF + "  AND PTNO = '" + strPano + "'                               ";
                SQL = SQL + ComNum.VBLF + "  AND CHARTDATE = '" + strChartDate.Replace("-", "") + "'    ";
                //TODO                
                //SQL = SQL + ComNum.VBLF + "  AND PTNO = '09152596'                               ";
                //SQL = SQL + ComNum.VBLF + "  AND CHARTDATE = '20190621'                     ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["BP"].ToString().Trim().IndexOf("/") != -1)
                    //{
                    //    opInfo.Sbp = dt.Rows[0]["BP"].ToString().Trim().Split('/')[0];
                    //    opInfo.Dbp = dt.Rows[0]["BP"].ToString().Trim().Split('/')[1];
                    //}
                    //opInfo.Pr = dt.Rows[0]["PR"].ToString().Trim();
                    opInfo.Bw = dt.Rows[0]["BW"].ToString().Trim();
                    //opInfo.Bt = dt.Rows[0]["BT"].ToString().Trim();

                    opInfo.Leave = dt.Rows[0]["LEAVE"].ToString().Trim();
                    opInfo.Arrive = dt.Rows[0]["ARRIVE"].ToString().Trim();

                    // NPO
                    opInfo.NpoY = dt.Rows[0]["NPO_Y"].ToString().Trim();
                    opInfo.NpoN = dt.Rows[0]["NPO_N"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ReadVitalSign(string strPano, string strMEDFRDATE)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "WITH TEMPCHART AS (                                                               ";
                SQL = SQL + ComNum.VBLF + "SELECT ITEMNO, MAX(ITEMVALUE) AS ITEMVALUE, MAX(CHARTDATE) AS CHARTDATE           ";
                SQL = SQL + ComNum.VBLF + "FROM                                                                              ";
                SQL = SQL + ComNum.VBLF + "(                                                                                 ";
                SQL = SQL + ComNum.VBLF + "SELECT A.CHARTDATE || A.CHARTTIME AS CHARTDATE, B.ITEMNO, B.ITEMVALUE             ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A                                                  ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN  ADMIN.AEMRCHARTROW B                                            ";
                SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B.EMRNO                                                          ";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNOHIS = B.EMRNOHIS                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND B.ITEMNO IN('I0000002018', 'I0000001765', 'I0000001811', 'I0000014815')    ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPano + "'                                                 ";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO IN(3150, 2135, 1935, 2431, 1969)                                  ";
                SQL = SQL + ComNum.VBLF + "   AND A.MEDFRDATE = '" + strMEDFRDATE + "'                                       ";
                SQL = SQL + ComNum.VBLF + "   AND(A.CHARTDATE || CHARTTIME) =                                                ";
                SQL = SQL + ComNum.VBLF + "       (SELECT MAX(CHARTDATE || CHARTTIME)                                        ";
                SQL = SQL + ComNum.VBLF + "          FROM ADMIN.AEMRCHARTMST                                            ";
                SQL = SQL + ComNum.VBLF + "         WHERE PTNO = A.PTNO                                                      ";
                SQL = SQL + ComNum.VBLF + "           AND FORMNO = A.FORMNO                                                  ";
                SQL = SQL + ComNum.VBLF + "           AND MEDFRDATE = A.MEDFRDATE                                            ";
                SQL = SQL + ComNum.VBLF + "       )                                                                          ";
                SQL = SQL + ComNum.VBLF + ")                                                                                 ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ITEMNO                                                                   ";
                SQL = SQL + ComNum.VBLF + ")                                                                                 ";
                SQL = SQL + ComNum.VBLF + "SELECT                                                                            ";
                SQL = SQL + ComNum.VBLF + "      (SELECT ITEMVALUE FROM TEMPCHART WHERE ITEMNO = 'I0000002018') || '/' ||    ";
                SQL = SQL + ComNum.VBLF + "      (SELECT ITEMVALUE FROM TEMPCHART WHERE ITEMNO = 'I0000001765') BP  --혈압   ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT ITEMVALUE FROM TEMPCHART WHERE ITEMNO = 'I0000014815') PR  --맥박   ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT ITEMVALUE FROM TEMPCHART WHERE ITEMNO = 'I0000001811') BT  --체온   ";
                SQL = SQL + ComNum.VBLF + "FROM DUAL                                                                         ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["BP"].ToString().Trim().IndexOf("/") != -1)
                    {
                        opInfo.Sbp = dt.Rows[0]["BP"].ToString().Trim().Split('/')[0];
                        opInfo.Dbp = dt.Rows[0]["BP"].ToString().Trim().Split('/')[1];
                    }
                    opInfo.Pr = dt.Rows[0]["PR"].ToString().Trim();
                    opInfo.Bt = dt.Rows[0]["BT"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ReadPastHxList(string strPano, string strMEDFRDATE)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";                
                SQL = SQL + ComNum.VBLF + "SELECT                                                                               ";
                SQL = SQL + ComNum.VBLF + "       EMRNO                                                                         ";
                SQL = SQL + ComNum.VBLF + "     , PTNO                                                                          ";
                SQL = SQL + ComNum.VBLF + "     , CHARTDATE                                                                     ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "        WHERE ITEMCD = 'I0000034768' AND EMRNO = M.EMRNO) HTN        -- 고혈압       ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034769' AND EMRNO = M.EMRNO) DM     -- 당뇨         ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034770' AND EMRNO = M.EMRNO) TC     -- 결핵         ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034774' AND EMRNO = M.EMRNO) CC     -- 암           ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034771' AND EMRNO = M.EMRNO) LC     -- 간질환       ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034773' AND EMRNO = M.EMRNO) CD     -- 심장질환     ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT DECODE(ITEMVALUE, 1, 'true', 'false') FROM ADMIN.AEMRCHARTROW    ";
                SQL = SQL + ComNum.VBLF + "            WHERE ITEMCD = 'I0000034383_1' AND EMRNO = M.EMRNO) OTHER-- 기타         ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW                                ";
                SQL = SQL + ComNum.VBLF + "         WHERE ITEMCD = 'I0000034383_2' AND EMRNO = M.EMRNO) OTHERTXT-- 기타텍스     ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST M                                                     ";
                SQL = SQL + ComNum.VBLF + " WHERE M.PTNO = '" + strPano + "'                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND M.FORMNO IN(SELECT FORMNO FROM ADMIN.AEMRFORM                            ";
                SQL = SQL + ComNum.VBLF + "                     WHERE GRPFORMNO = '1013'                                        ";
                SQL = SQL + ComNum.VBLF + "                       AND USECHECK = '1')                                           ";
                SQL = SQL + ComNum.VBLF + "   AND M.MEDFRDATE = '" + strMEDFRDATE + "'                                          ";
                SQL = SQL + ComNum.VBLF + "   AND (M.CHARTDATE || CHARTTIME) = (SELECT MAX(CHARTDATE || CHARTTIME)              ";  
                SQL = SQL + ComNum.VBLF + "                                       FROM ADMIN.AEMRCHARTMST                  ";
                SQL = SQL + ComNum.VBLF + "                                      WHERE PTNO = M.PTNO                            ";
                SQL = SQL + ComNum.VBLF + "                                        AND FORMNO = M.FORMNO                        ";
                SQL = SQL + ComNum.VBLF + "                                        AND MEDFRDATE = M.MEDFRDATE)                 ";
                

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {                    
                    opInfo.PastHxHTM = dt.Rows[0]["HTN"].ToString().Trim();
                    opInfo.PastHxDM = dt.Rows[0]["DM"].ToString().Trim();                    
                    opInfo.PastHxTC = dt.Rows[0]["TC"].ToString().Trim();
                    opInfo.PastHxCC = dt.Rows[0]["CC"].ToString().Trim();
                    opInfo.PastHxLC = dt.Rows[0]["LC"].ToString().Trim();
                    opInfo.PastHxCD = dt.Rows[0]["CD"].ToString().Trim();
                    opInfo.PastHxOTHER = dt.Rows[0]["OTHER"].ToString().Trim();
                    opInfo.PastHxOTHERTXT = dt.Rows[0]["OTHERTXT"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ReadExamResult(string strPano, string strChartDate)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "WITH EXAMRESULT AS(                                                                  ";
                SQL = SQL + ComNum.VBLF + "SELECT A.SPECNO, A.RESULTDATE, C.EXAMYNAME, C.EXAMNAME, B.RESULT                                  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_SPECMST A                                                       ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EXAM_RESULTC B                                                 ";                
                SQL = SQL + ComNum.VBLF + "    ON A.SPECNO = B.SPECNO                                                           ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EXAM_MASTER C                                                  ";                
                SQL = SQL + ComNum.VBLF + "    ON B.SUBCODE = C.MASTERCODE                                                      ";
                //SQL = SQL + ComNum.VBLF + "    AND C.EXAMNAME IN('  Hb', '  Hct', ' Na', ' K', ' GOT', ' GPT', 'PT', 'aPTT')    ";
                SQL = SQL + ComNum.VBLF + "  AND C.EXAMYNAME IN ('Hgb', 'Hct', 'Sodium', 'Potassium', 'AST', 'ALT', 'PT', 'aPTT', 'PLT', 'WBC') ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + strPano + "'                                                     ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(A.RESULTDATE) <= TO_DATE('" + strChartDate + "', 'YYYY-MM-DD')           ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RESULTDATE DESC                                                             ";
                SQL = SQL + ComNum.VBLF + ")                                                                                    ";
                SQL = SQL + ComNum.VBLF + "SELECT 'Hb' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'Hgb'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'Hct' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'Hct'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'Na' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'Sodium'                                                          ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'K' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'Potassium'                                                       ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'GOT' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'AST'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'GPT' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'ALT'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'PT' EXAMNAME, RESULT                                                              ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'PT'                                                              ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'aPTT' EXAMNAME, RESULT                                                       ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'aPTT'                                                            ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'PLT' EXAMNAME, RESULT                                                        ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'PLT'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                                            ";
                SQL = SQL + ComNum.VBLF + "SELECT 'WBC' EXAMNAME, RESULT                                                        ";
                SQL = SQL + ComNum.VBLF + "  FROM EXAMRESULT                                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE EXAMYNAME = 'WBC'                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1                                                                    ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["EXAMNAME"].ToString().Trim())
                        {
                            case "Hb":
                                opInfo.ExamHb = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "Hct":
                                opInfo.ExamHct = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "Na":
                                opInfo.ExamNa = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "K":
                                opInfo.ExamK = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "GOT":
                                opInfo.ExamGOT = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "GPT":
                                opInfo.ExamGPT = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "PT":
                                opInfo.ExamPT = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "aPTT":
                                opInfo.ExamPTT = dt.Rows[i]["RESULT"].ToString().Trim();
                                break;
                            case "PLT":
                                if (dt.Rows[i]["RESULT"].ToString().Trim() != "")
                                {
                                    opInfo.ExamPLT = string.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["RESULT"].ToString().Trim()) * 1000);
                                    //Convert.ToString(VB.Val(dt.Rows[i]["RESULT"].ToString().Trim()) * 1000);
                                }
                                else
                                {
                                    opInfo.ExamPLT = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "WBC":
                                if (dt.Rows[i]["RESULT"].ToString().Trim() != "")
                                {
                                    opInfo.ExamWBC = string.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["RESULT"].ToString().Trim()) * 1000);
                                    //opInfo.ExamWBC = Convert.ToString(VB.Val(dt.Rows[i]["RESULT"].ToString().Trim()) * 1000);
                                }
                                else
                                {
                                    opInfo.ExamWBC = dt.Rows[i]["RESULT"].ToString().Trim();
                                }                                
                                break;
                        }
                    }                    
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string ReadSabun(string Gbn, string GbValue)
        {
            string rtnVal = "99999";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (Gbn == "0")     // Anes. Dr
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, DRNAME AS NAME FROM ADMIN.OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + " WHERE  DEPTCODE IN( 'RT','PC')                         ";
                    SQL = SQL + ComNum.VBLF + "   AND GBOUT = 'N'                                      ";
                    SQL = SQL + ComNum.VBLF + "  AND DRNAME = '" + GbValue + "'                        ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY DRCODE                                         ";
                }
                else if (Gbn == "1")    // Anes. Nr
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, KORNAME AS NAME                           ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.INSA_MST A                              ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BUSE B                       ";
                    SQL = SQL + ComNum.VBLF + "    ON A.BUSE = B.BUCODE                                ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.BUSE = '033103'                                 ";
                    SQL = SQL + ComNum.VBLF + "  AND TOIDAY IS NULL                                    ";
                    SQL = SQL + ComNum.VBLF + "  AND KORNAME = '" + GbValue + "'                       ";
                }
                else if (Gbn == "2")    // Cir.Nr
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, KORNAME AS NAME                           ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.INSA_MST A                              ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BUSE B                       ";
                    SQL = SQL + ComNum.VBLF + "    ON A.BUSE = B.BUCODE                                ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.BUSE = '033102'                                 ";
                    SQL = SQL + ComNum.VBLF + "  AND TOIDAY IS NULL                                    ";
                    SQL = SQL + ComNum.VBLF + "  AND KORNAME = '" + GbValue + "'                       ";
                }
                else if (Gbn == "3")    // Surgeon
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT DRCODE AS SABUN, DRNAME AS NAME                  ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_DOCTOR                           ";
                    SQL = SQL + ComNum.VBLF + " WHERE TOUR <> 'Y'                                      ";
                    SQL = SQL + ComNum.VBLF + "   AND DRNAME = '" + GbValue + "'                      ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SABUN"].ToString().Trim();                    
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return rtnVal;
        }

        private void READ_ABO(string strPano)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                       ";
                SQL = SQL + ComNum.VBLF + "    ADMIN.FC_EXAM_BLOOD_MASTER_ABO('" + strPano + "') AS ABO   ";
                SQL = SQL + ComNum.VBLF + " FROM DUAL                                                   ";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    opInfo.ABO = dt.Rows[0]["ABO"].ToString().Trim();                    
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
