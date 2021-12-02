using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ComDbB;
using ComBase;
using ComNurLibB;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmEMC.cs
    /// Description     : 응급의료자료전송
    /// Author          : 유진호
    /// Create Date     : 2018-05-07
    /// <history>       
    /// D:\참고소스\포항성모병원 VB Source(2018.04.01)\OPD\emc\frmEMC3(frmEMC.frm)
    /// </history>
    /// </summary>
    public partial class frmEMC3 : Form
    {
        //중앙응급의료센터 FTP서버
        private string EMC_SVR_IP = "211.112.137.51";
        private string EMC_SVR_PORT = "13391";
        private string EMC_SVR_ID = "realtime";
        private string EMC_SVR_PW = "emcto06@28^";
        private string strFilePath = @"c:\cmc\";    //LocalPath

        int FnTimer = 0;
        int FnSS = 0;

        public frmEMC3()
        {
            InitializeComponent();
        }

        private void frmEMC3_Load(object sender, EventArgs e)
        {            
            if (clsType.User.Sabun == "42388" || clsType.User.Sabun == "21403") //'김아린만 보이도록
            {
                mnuSetErCode.Enabled = false;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            FnTimer = 0;
            txtDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            txtWait.Text = FnTimer + "(분)";
            txtLog.Text = "";

            READ_LAST_DATA();
            
            timer1.Enabled = true;
        }

        private void READ_LAST_DATA()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT CODE, RCNT, BCNT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.ETC_EMC_V3";
                SQL = SQL + ComNum.VBLF + " WHERE SID = (";
                SQL = SQL + ComNum.VBLF + "               SELECT MAX(SID) ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.ETC_EMC_V3";
                SQL = SQL + ComNum.VBLF + "                WHERE SENDDATE >= TRUNC(SYSDATE-20))";
                SQL = SQL + ComNum.VBLF + "   AND CODE IN ('O026','O027','O028','O029','O030',";
                SQL = SQL + ComNum.VBLF + "                'O031','O032','O033','O034','O035','O036','O037')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["CODE"].ToString().Trim())
                        {
                            case "O026":
                                ss1_Sheet1.Cells[13, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[13, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O027":
                                ss1_Sheet1.Cells[14, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[14, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O028":
                                ss1_Sheet1.Cells[15, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[15, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O029":
                                ss1_Sheet1.Cells[16, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[16, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O030":
                                ss1_Sheet1.Cells[17, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[17, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O031":
                                ss1_Sheet1.Cells[18, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[18, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O032":
                                ss1_Sheet1.Cells[19, 4].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[19, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O033":
                                ss1_Sheet1.Cells[16, 11].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[16, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O034":
                                ss1_Sheet1.Cells[17, 11].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[17, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O035":
                                ss1_Sheet1.Cells[18, 11].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[18, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O036":
                                ss1_Sheet1.Cells[19, 11].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[19, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                            case "O037":
                                ss1_Sheet1.Cells[20, 11].Text = dt.Rows[i]["RCNT"].ToString().Trim();
                                ss1_Sheet1.Cells[20, 5].Text = dt.Rows[i]["BCNT"].ToString().Trim();
                                break;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
               
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            if (FnSS == 60)
            {
                FnTimer = FnTimer + 1;
                txtWait.Text = FnTimer + "(분)";
                FnSS = 0;
            }
            else
            {
                FnSS = FnSS + 1;
                txtWait.Text = FnTimer + "분" + FnSS + "초";
            }

            //'매 4분마다 현재의 병실상황을 읽음
            if (FnTimer == 4 || FnTimer == 8 || FnTimer == 12)
            {
                mnuSendClick();
                FnTimer = 0;
                FnSS = 0;
            }

            timer1.Enabled = true;
        }

        private void mnuView_Click(object sender, EventArgs e)
        {
            READ_WARDROOM();
            READ_CNT_TOTAL();
            READ_CNT_USE();
            READ_OP();
            READ_CNT_MEDICALMACHINE();
        }


        private void READ_WARDROOM()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDATA = "";
            int nCnt1 = 0;
            int nCnt2 = 0;
            int nRow = 0;
            int nLastRow = 0;

            int nTot1 = 0;
            int nTot2 = 0;
            int nTot3 = 0;

            int nIcuBed = 0;
            int nWardBed = 0;

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < ss2_Sheet1.RowCount; j++)
                {
                    ss2_Sheet1.Cells[j, i].Text = "";
                }
            }

            try
            {
                //'병동별 입원,퇴원,재원환자수
                SQL = "SELECT WardCode,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' ";     //'지병환자
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";     //'전산실연습
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE NOT IN ('564','565')  ";     //'신생아중환자실 2013-06-27
                SQL = SQL + ComNum.VBLF + "   AND WardCode NOT IN ('2W','3A','IQ','IU','ND','NR','32','33','35','65') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT '대기' WARDCODE, COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + "  From KOSMOS_PMPA.NUR_ER_PATIENT_IPWON_WAIT";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '일반병실'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  Group By '대기'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDATA = dt.Rows[i]["WardCode"].ToString().Trim();
                        nCnt1 = (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        //'해당 병동을 찾음
                        nRow = 0;
                        for (j = 0; j < ss2_Sheet1.RowCount; j++)
                        {
                            if (ss2_Sheet1.Cells[j, 0].Text == strDATA)
                            {
                                nRow = j;
                                break;
                            }
                        } //for j
                        if (nRow == 0)
                        {                            
                            nRow = nLastRow;
                            ss2_Sheet1.Cells[nRow, 0].Text = strDATA;
                            ss2_Sheet1.Cells[nRow, 1].Text = READ_ROOM_CNT(strDATA);
                            nLastRow = nLastRow + 1;
                        }

                        ss2_Sheet1.Cells[nRow, 2].Text = nCnt1.ToString();
                    } //for i            
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }

            //'합계를 구함
            nTot1 = 0;
            nTot2 = 0;
            nTot3 = 0;
            nIcuBed = 0;
            nWardBed = 0;
            for (i = 0; i < nLastRow; i++)
            {
                nCnt1 = (int)VB.Val(ss2_Sheet1.Cells[i, 1].Text);
                nCnt2 = (int)VB.Val(ss2_Sheet1.Cells[i, 2].Text);
                ss2_Sheet1.Cells[i, 3].Text = (nCnt1 - nCnt2).ToString();

                if (ss2_Sheet1.Cells[i, 0].Text == "IU" || ss2_Sheet1.Cells[i, 0].Text == "32" ||
                   ss2_Sheet1.Cells[i, 0].Text == "33" || ss2_Sheet1.Cells[i, 0].Text == "35")
                {
                    nIcuBed = nIcuBed + (nCnt1 - nCnt2);   //'ICU 여유병상
                }
                else if (ss2_Sheet1.Cells[i, 0].Text != "IQ" && ss2_Sheet1.Cells[i, 0].Text != "ND" &&
                        ss2_Sheet1.Cells[i, 0].Text != "HU" && ss2_Sheet1.Cells[i, 0].Text != "NR")
                {
                    nWardBed = nWardBed + (nCnt1 - nCnt2); //'일반병동 여유병상 (신생아실,인큐베이타,호스피스제외)
                }

                nTot1 = nTot1 + nCnt1;
                nTot2 = nTot2 + nCnt2;
                nTot3 = nTot1 - nTot2;
            }

            ss2_Sheet1.Cells[nLastRow, 0].Text = "합계";
            ss2_Sheet1.Cells[nLastRow, 1].Text = nTot1.ToString();
            ss2_Sheet1.Cells[nLastRow, 2].Text = nTot2.ToString();
            ss2_Sheet1.Cells[nLastRow, 3].Text = nTot3.ToString();

            ss1_Sheet1.Cells[20, 4].Text = nTot3.ToString();    //'가용병상수
            ss1_Sheet1.Cells[20, 5].Text = nTot1.ToString();    //'기준병상수            
        }

        private string READ_ROOM_CNT(string strWARD)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT SUM(TBed) TBed ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE TBed > 0 ";
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE NOT IN ('564','565')  ";     //'신생아중환자실 2013-06-27  , '6층 특실 제외
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + strWARD + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY WardCode ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["TBED"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void READ_CNT_TOTAL()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strCODE = "";
            string strCnt = "";
            string strOK = "";

            try
            {
                SQL = " SELECT SUM(A.TBED) CNT, B.ERCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.VIEW_ER_EMC_ROOM A, KOSMOS_PMPA.NUR_ER_EMC_ROOM B";
                SQL = SQL + ComNum.VBLF + " WHERE A.WARDCODE = B.WARDCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE = B.ROOMCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.CODE = B.CODE";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.ERCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.ERCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["ERCODE"].ToString().Trim();
                        strCnt = dt.Rows[i]["CNT"].ToString().Trim();

                        strOK = "";

                        if (strCODE != "")
                        {
                            for (j = 2; j < 21; j++)
                            {
                                if (strOK == "")
                                {
                                    if (ss1_Sheet1.Cells[j, 3].Text.Trim() == strCODE)
                                    {
                                        strOK = "OK";
                                        ss1_Sheet1.Cells[j, 5].Text = strCnt;
                                    }
                                }

                                if (strOK == "")
                                {
                                    if (ss1_Sheet1.Cells[j, 10].Text == strCODE)
                                    {
                                        strOK = "OK";
                                        ss1_Sheet1.Cells[j, 12].Text = strCnt;
                                    }
                                }
                            } //for j
                        }
                    } //for i            
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_CNT_USE()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strCODE = "";
            string strCntT = "";      //'기준값
            string strCnt = "";       //'가용 => 기준값 빼기 현재 사용 중인 개수
            //string strOK = "";

            try
            {
                SQL = " SELECT B.ERCODE, SUM(1) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.NUR_ER_EMC_ROOM B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = B.WARDCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE = TO_NUMBER(B.ROOMCODE)";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN = 'WARD'";
                SQL = SQL + ComNum.VBLF + "   AND B.ERCODE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.ERCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["ERCODE"].ToString().Trim();
                        strCnt = dt.Rows[i]["CNT"].ToString().Trim();

                        for (j = 2; j < 21; j++)
                        {
                            if (ss1_Sheet1.Cells[j, 3].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 5].Text;
                                ss1_Sheet1.Cells[j, 4].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }

                            if (ss1_Sheet1.Cells[j, 10].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 12].Text;
                                ss1_Sheet1.Cells[j, 11].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }


            try
            {
                SQL = " SELECT B.ERCODE, SUM(1) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.NUR_ER_EMC_ROOM B";
                SQL = SQL + ComNum.VBLF + " WHERE A.OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "  AND A.WARDCODE = B.WARDCODE";
                SQL = SQL + ComNum.VBLF + "  AND A.BEDNUM = B.CODE";
                SQL = SQL + ComNum.VBLF + "  AND B.GUBUN = 'ICU'";
                SQL = SQL + ComNum.VBLF + "  AND B.ERCODE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY B.ERCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["ERCODE"].ToString().Trim();
                        strCnt = dt.Rows[i]["CNT"].ToString().Trim();

                        for (j = 2; j < 21; j++)
                        {
                            if (ss1_Sheet1.Cells[j, 3].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 5].Text;
                                ss1_Sheet1.Cells[j, 4].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }

                            if (ss1_Sheet1.Cells[j, 10].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 12].Text;
                                ss1_Sheet1.Cells[j, 11].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }


            try
            {
                SQL = " SELECT 'O001' ERCODE, COUNT(PANO) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = 'ER' ";
                SQL = SQL + ComNum.VBLF + "   AND (OcsJin Is Null Or OcsJin <>'#')";   //'접수하면 바로 인원추가됨
                SQL = SQL + ComNum.VBLF + "   AND ER_NUM NOT IN (";
                SQL = SQL + ComNum.VBLF + "       '52','53','54',";         //'응급실 소아병상
                SQL = SQL + ComNum.VBLF + "       '58','59',";              //'응급실 읍압격리 병상
                SQL = SQL + ComNum.VBLF + "       '55','56','57','47','48','49') ";        //'응급실 일반격리 병상(4-1,4-2,-4-3은 일반격리병사에 포함시킴)
                SQL = SQL + ComNum.VBLF + "  UNION ALL    ";
                SQL = SQL + ComNum.VBLF + "  SELECT 'O002' ERCODE, COUNT(PANO) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = 'ER' ";
                SQL = SQL + ComNum.VBLF + "   AND (OcsJin Is Null Or OcsJin <>'#')";   //'접수하면 바로 인원추가됨
                SQL = SQL + ComNum.VBLF + "   AND ER_NUM IN (";
                SQL = SQL + ComNum.VBLF + "       '52','53','54')";         //'응급실 소아병상
                SQL = SQL + ComNum.VBLF + "  UNION ALL    ";
                SQL = SQL + ComNum.VBLF + "  SELECT 'O003' ERCODE, COUNT(PANO) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = 'ER' ";
                SQL = SQL + ComNum.VBLF + "   AND (OcsJin Is Null Or OcsJin <>'#')";   //'접수하면 바로 인원추가됨
                SQL = SQL + ComNum.VBLF + "   AND ER_NUM IN (";
                SQL = SQL + ComNum.VBLF + "       '58','59')";
                SQL = SQL + ComNum.VBLF + "  UNION ALL    ";
                SQL = SQL + ComNum.VBLF + "  SELECT 'O004' ERCODE, COUNT(PANO) CNT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = 'ER' ";
                SQL = SQL + ComNum.VBLF + "   AND (OcsJin Is Null Or OcsJin <>'#')";   //'접수하면 바로 인원추가됨
                //''응급실 일반격리 병상(4 - 1, 4 - 2, -4 - 3은 일반격리병사에 포함시킴)
                SQL = SQL + ComNum.VBLF + "   AND ER_NUM IN (";
                SQL = SQL + ComNum.VBLF + "       '55','56','57','47','48','49') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["ERCODE"].ToString().Trim();
                        strCnt = dt.Rows[i]["CNT"].ToString().Trim();

                        for (j = 2; j < 21; j++)
                        {
                            if (ss1_Sheet1.Cells[j, 3].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 5].Text;
                                ss1_Sheet1.Cells[j, 4].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }

                            if (ss1_Sheet1.Cells[j, 10].Text == strCODE)
                            {
                                strCntT = ss1_Sheet1.Cells[j, 12].Text;
                                ss1_Sheet1.Cells[j, 11].Text = ((int)VB.Val(strCntT) - (int)VB.Val(strCnt)).ToString();
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }

            int nTemp = 0;
            try
            {
                SQL = "SELECT SUM(CNT) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' ";     //'지병환자
                SQL = SQL + ComNum.VBLF + "  AND Pano <> '81000004' ";      //'전산실연습
                SQL = SQL + ComNum.VBLF + "  AND GBSTS NOT IN ('1','7','9')";
                SQL = SQL + ComNum.VBLF + "  AND ( OUTDATE IS NULL OR OUTDATE >=TRUNC(SYSDATE) ) ";   //'당일은 보임.2011-09-29
                SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '33') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nTemp = 20;
                    ss1_Sheet1.Cells[2, 12].Text = nTemp.ToString();
                    ss1_Sheet1.Cells[2, 11].Text = (nTemp - (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim())).ToString();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }


            try
            {
                SQL = "SELECT SUM(CNT) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano <  '90000000' "; //'지병환자
                SQL = SQL + ComNum.VBLF + "  AND Pano <> '81000004' ";  //'전산실연습
                SQL = SQL + ComNum.VBLF + "  AND GBSTS NOT IN ('1','7','9')";
                SQL = SQL + ComNum.VBLF + "  AND ( OUTDATE IS NULL OR OUTDATE >=TRUNC(SYSDATE) ) ";   //'당일은 보임.2011-09-29
                SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '35') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nTemp = 16;
                    ss1_Sheet1.Cells[14, 12].Text = nTemp.ToString();
                    ss1_Sheet1.Cells[14, 11].Text = (nTemp - (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim())).ToString();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_OP()
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nCnt1 = 0;

            try
            {
                SQL = "SELECT COUNT(GUBUN) Cnt FROM KOSMOS_PMPA.ORAN_GUIDE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN IN('2','3','4','5') ";    //'대기중, 회복중 이외 전체 '2014 - 03 - 25 김현욱 수정

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCnt1 = (int)VB.Val(dt.Rows[0]["Cnt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }

            ss1_Sheet1.Cells[11, 4].Text = (9 - nCnt1).ToString();
            ss1_Sheet1.Cells[11, 5].Text = "9";

        }

        private void READ_CNT_MEDICALMACHINE()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strCODE = "";
            string strCnt = "";
            string strOK = "";

            try
            {
                SQL = " SELECT CODE, NAME CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_ER_응급실_의료장비수'";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["CODE"].ToString().Trim();
                        strCnt = dt.Rows[i]["CNT"].ToString().Trim();

                        strOK = "";

                        if (strCODE != "")
                        {
                            for (j = 2; j < 21; j++)
                            {
                                if (strOK == "")
                                {
                                    if (ss1_Sheet1.Cells[j, 3].Text == strCODE)
                                    {
                                        strOK = "OK";
                                        ss1_Sheet1.Cells[j, 5].Text = strCnt;
                                    }
                                }

                                if (strOK == "")
                                {
                                    if (ss1_Sheet1.Cells[j, 10].Text == strCODE)
                                    {
                                        strOK = "OK";
                                        ss1_Sheet1.Cells[j, 12].Text = strCnt;

                                    }
                                }
                            } //for j
                        }
                    } //for i
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void mnuSend_Click(object sender, EventArgs e)
        {
            mnuSendClick();
        }

        private bool mnuSendClick()
        {
            bool rtnVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strFilename = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            READ_WARDROOM();
            READ_CNT_TOTAL();
            READ_CNT_USE();
            READ_OP();
            READ_CNT_MEDICALMACHINE();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (clsType.User.Sabun == "21403")
                { }
                else
                {
                    strFilename = CREATE_SEND_DATA();

                    if (FTP_EMC_SEND(strFilePath + strFilename, strFilename, EMC_SVR_IP, EMC_SVR_ID, EMC_SVR_PW, (int)VB.Val(EMC_SVR_PORT)) == false)
                    {
                        SQL = " INSERT INTO KOSMOS_PMPA.ETC_EMC_V3_SENDLOG( ";
                        SQL = SQL + ComNum.VBLF + "  SID, STAT, SENDDATE) VALUES (";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Replace(strFilename, ".rtb", "") + "','N',SYSDATE)";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);

                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        txtLog.Text = "★전송오류!! " + VB.Right(clsPublic.GstrSysDate, 5) + " " + clsPublic.GstrSysTime + ComNum.VBLF + txtLog.Text;
                        if (VB.Len(txtLog.Text) > 2000) txtLog.Text = VB.Left(txtLog.Text, 2000);
                    }
                    else
                    {
                        SQL = " INSERT INTO KOSMOS_PMPA.ETC_EMC_V3_SENDLOG( ";
                        SQL = SQL + ComNum.VBLF + "  SID, STAT, SENDDATE) VALUES (";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Replace(strFilename, ".rtb", "") + "','Y',SYSDATE)";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        txtLog.Text = "전송 OK ..." + VB.Right(clsPublic.GstrSysDate, 5) + " " + clsPublic.GstrSysTime + ComNum.VBLF + txtLog.Text;
                        if (VB.Len(txtLog.Text) > 2000) txtLog.Text = VB.Left(txtLog.Text, 2000);
                    }
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        
        private string CREATE_SEND_DATA()
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strHead = "";
            string strTSUB = "";
            string strSUB = "";
            string strSENDDATA = "";
            string strTIME = "";

            string strCODE = "";
            string strUCnt = "";
            string strTCnt = "";
            string strStat = "";


            SQL = " SELECT TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') SDATE FROM DUAL";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {

                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                strTIME = dt.Rows[i]["SDATE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            

            strTSUB = "";
            for (i = 2; i < 21; i++)
            {
                strCODE = ss1_Sheet1.Cells[i, 3].Text.Trim();
                strUCnt = ss1_Sheet1.Cells[i, 4].Text.Trim();
                strTCnt = ss1_Sheet1.Cells[i, 5].Text.Trim();
                strStat = ss1_Sheet1.Cells[i, 6].Text.Trim();

                if ((strUCnt != "" && strTCnt != "") || (strUCnt == "Y" || strUCnt == "N"))
                {
                    //'없음은 보내지 않음
                    //'If strUCnt = "없음" Then strUCnt = ""

                    strSUB = "{“BedCode”:“" + strCODE + "”,“RtmInpt”:“" + strUCnt + "”,“BaseInpt”:“";
                    strSUB = strSUB + strTCnt + "”,“SendBase”:“" + strStat + "”},";
                    strTSUB = strTSUB + strSUB + ComNum.VBLF;
                }

                SQL = " INSERT INTO KOSMOS_PMPA.ETC_EMC_V3( ";
                SQL = SQL + ComNum.VBLF + "  SID, CODE, RCNT, BCNT, ";
                SQL = SQL + ComNum.VBLF + "  STAT, SENDDATE) VALUES ( ";
                SQL = SQL + ComNum.VBLF + " 'A2700016" + strTIME + "','" + strCODE + "','" + strUCnt + "','" + strTCnt + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strStat + "', SYSDATE) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                strCODE = ss1_Sheet1.Cells[i, 10].Text.Trim();
                strUCnt = ss1_Sheet1.Cells[i, 11].Text.Trim();
                strTCnt = ss1_Sheet1.Cells[i, 12].Text.Trim();
                strStat = ss1_Sheet1.Cells[i, 13].Text.Trim();

                SQL = " INSERT INTO KOSMOS_PMPA.ETC_EMC_V3( ";
                SQL = SQL + ComNum.VBLF + "  SID, CODE, RCNT, BCNT, ";
                SQL = SQL + ComNum.VBLF + "  STAT, SENDDATE) VALUES ( ";
                SQL = SQL + ComNum.VBLF + " 'A2700016" + strTIME + "','" + strCODE + "','" + strUCnt + "','" + strTCnt + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strStat + "', SYSDATE) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if ((strUCnt != "" && strTCnt != "") || (strUCnt == "Y" || strUCnt == "N"))
                {
                    //'없음은 보내지 않음
                    //'If strUCnt = "없음" Then strUCnt = ""

                    strSUB = "{“BedCode”:“" + strCODE + "”,“RtmInpt”:“" + strUCnt + "”,“BaseInpt”:“";
                    strSUB = strSUB + strTCnt + "”,“SendBase”:“" + strStat + "”},";
                    strTSUB = strTSUB + strSUB + ComNum.VBLF;
                }
            }

            strTSUB = VB.Mid(strTSUB, 1, VB.Len(strTSUB) - 3);
            strTSUB = strTSUB + "]}";       //'마지막에 중괄호, 대괄호 닫아주기

            strHead = "{”EmogCode”:”A2700016”,”SendDate”:”" + strTIME + "”,”SendStatus”:”S”,";
            strHead = strHead + ComNum.VBLF + "“Data”:[";

            strSENDDATA = strHead + strTSUB;

            strSENDDATA = VB.Replace(strSENDDATA, "”", @"""");
            strSENDDATA = VB.Replace(strSENDDATA, "“", @"""");


            //파일생성            
            string strFileName = @"A2700016" + strTIME + ".rtb";
            
            FileInfo file = new FileInfo(strFilePath + strFileName);
            if (!file.Exists)
            {
                FileStream fc = file.Create();
                fc.Close();
            }
            
            //파일쓰기
            FileStream fs = file.OpenWrite();
            TextWriter tw = new StreamWriter(fs);
            tw.Write(strSENDDATA);
            tw.Close();
            fs.Close();
            
            return strFileName;
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss2_Sheet1.Cells[e.Row, 0].Text != "")
            {
                frmWardList frmWardListX = new frmWardList(ss2_Sheet1.Cells[e.Row, 0].Text);
                frmWardListX.StartPosition = FormStartPosition.CenterParent;
                frmWardListX.ShowDialog();
            }
        }

        private bool FTP_EMC_SEND(string strLocal, string strRemote, string strIPAddr, string strUser, string strPass, int strPort)
        {            
            bool bRet = false;
            Ftpedt FTP = new Ftpedt();
            
            if (strIPAddr != "" && strUser != "" && strPass != "")
            {                
                if (FTP.FtpConnetBatch(strIPAddr, strUser, strPass, strPort) == false)
                {
                    ComFunc.MsgBox("FTP Server Connect ERROR !!!");
                    bRet = false;
                    return bRet;
                }
            }
            else
            {                
                bRet = false;
                return bRet;
            }
            
            bRet = FTP.FtpUploadBatch(strLocal, strRemote, "/");
            FTP.FtpDisConnetBatch();
            FTP = null;

            return bRet;
        }

        private void mnuErOverDuty_Click(object sender, EventArgs e)
        {
            frmEmergencyRoomDuty frmEmergencyRoomDutyX = new frmEmergencyRoomDuty();
            frmEmergencyRoomDutyX.StartPosition = FormStartPosition.CenterParent;
            frmEmergencyRoomDutyX.ShowDialog();
        }

        private void mnuSetErCode_Click(object sender, EventArgs e)
        {
            frmCodeSet frmCodeSetX = new frmCodeSet();
            frmCodeSetX.StartPosition = FormStartPosition.CenterParent;
            frmCodeSetX.ShowDialog();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
