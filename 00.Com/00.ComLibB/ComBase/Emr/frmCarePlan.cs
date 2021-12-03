using System;
using System.Data;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmCarePlan.cs
    /// Description     : Care Plan
    /// Author          : 박창욱
    /// Create Date     : 2018-05-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\mtsEmr\CarePlan\FrmCarePlan.frm(FrmCarePlan.frm) >> frmCarePlan.cs 폼이름 재정의" />	
    public partial class frmCarePlan : Form
    {
        string FstrPTNO = "";
        string fstrIPDNO = "";
        string FstrInDate = "";
        string FstrWard = "";
        string FstrROOM = "";
        string FstrAge = "";
        string FstrSex = "";
        string FstrDeptCode = "";
        string FstrSname = "";
        string FstrUSEID = "";

        frmBasDietEvaluationList frmBasDietEvaluationListX = null;

        public frmCarePlan()
        {
            InitializeComponent();
        }

        public frmCarePlan(string argPano, string argInDate, string argIPDNO, string argUserId)
        {
            InitializeComponent();

            FstrPTNO = argPano;
            FstrInDate = argInDate;
            fstrIPDNO = argIPDNO;
            FstrUSEID = argUserId;
        }

        void Nur_Info(string argPano, string argInDate, string argOutDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strEmrNo = "";
            string strFormNo = "";

            string strA = "";
            string strB = "";
            string strC = "";

            string strName = "";
            string strChartDate = "";

            argInDate = argInDate.Replace("-", "");
            argOutDate = argOutDate.Replace("-", "");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (2356,2311,2305,2294)";

                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (2356, 2311, 2305, 2294)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                    strName = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["USEID"].ToString().Trim());
                    strChartDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strChartDate = strChartDate + " " + ComFunc.FormatStrToDate(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                }

                dt.Dispose();
                dt = null;

                if (strEmrNo == "")
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                switch (strFormNo)
                {
                    // FORMNO : 2356 간호정보조사지(산모)
                    case "2356":
                        SQL = "";
                        SQL = " SELECT extractValue(chartxml, '//it35') A, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta1') B, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik18') C1, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik19') C2, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik20') C3, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik21') C4, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik22') C5, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik23') C6, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik24') C7, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it37') C8,";
                        SQL = SQL + ComNum.VBLF + " '' AS C9, --과거병력 뇌졸중";
                        SQL = SQL + ComNum.VBLF + " '' AS C10, --과거병력 심장질환";
                        SQL = SQL + ComNum.VBLF + " '' AS C11,  --과거병력 암";
                        SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SQL = SQL + ComNum.VBLF + " UNION ALL";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002168') AS A,  --주증상";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000028908') AS B,  --입원동기";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034374') AS C1, --과거병력 없음";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034376') AS C2, --과거병력 모름";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034378') AS C3, --과거병력 고혈압";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034379') AS C4, --과거병력 당뇨";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034770') AS C5, --과거병력 결핵";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034771') AS C6, --과거병력 간염";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_1') AS C7, --과거병력 기타";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_2') AS C8, --과거병력 기타 내용";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034772') AS C9, --과거병력 뇌졸중";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034773') AS C10, --과거병력 심장질환";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true) FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034774') AS C11,  --과거병력 암";
                        SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strA = dt.Rows[0]["A"].ToString().Trim();
                            strB = dt.Rows[0]["B"].ToString().Trim();
                            strC = "";
                            if (dt.Rows[0]["C1"].ToString().Trim() == "true")
                            {
                                strC = strC + " 없음 ";
                            }
                            if (dt.Rows[0]["C2"].ToString().Trim() == "true")
                            {
                                strC = strC + " 모름 ";
                            }
                            if (dt.Rows[0]["C3"].ToString().Trim() == "true")
                            {
                                strC = strC + " 고혈압 ";
                            }
                            if (dt.Rows[0]["C4"].ToString().Trim() == "true")
                            {
                                strC = strC + " 당뇨 ";
                            }
                            if (dt.Rows[0]["C5"].ToString().Trim() == "true")
                            {
                                strC = strC + " 결핵 ";
                            }
                            if (dt.Rows[0]["C6"].ToString().Trim() == "true")
                            {
                                strC = strC + " 간염 ";
                            }
                            if (dt.Rows[0]["C9"].ToString().Trim() == "true")
                            {
                                strC = strC + " 뇌졸중 ";
                            }
                            if (dt.Rows[0]["C10"].ToString().Trim() == "true")
                            {
                                strC = strC + " 심장질환 ";
                            }
                            if (dt.Rows[0]["C11"].ToString().Trim() == "true")
                            {
                                strC = strC + " 암 ";
                            }
                            if (dt.Rows[0]["C7"].ToString().Trim() == "true")
                            {
                                strC = strC + " 기타 ";
                            }
                            if (dt.Rows[0]["C8"].ToString().Trim() != "")
                            {
                                strC = strC + "(" + dt.Rows[0]["C8"].ToString().Trim() + ")";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                        break;
                    // FORMNO : 2311 간호정보조사지
                    case "2311":
                        SQL = "";
                        SQL = " SELECT extractValue(chartxml, '//it32') A, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta1') B, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik25') C1, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik26') C2, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik27') C3, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik28') C4, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik29') C5, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik30') C6, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik31') C7, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it34') C8,";
                        SQL = SQL + ComNum.VBLF + " '' AS C9, --과거병력 뇌졸중";
                        SQL = SQL + ComNum.VBLF + " '' AS C10, --과거병력 심장질환";
                        SQL = SQL + ComNum.VBLF + " '' AS C11,  --과거병력 암";
                        SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SQL = SQL + ComNum.VBLF + " UNION ALL";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002168') AS A,  --주증상";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000028908') AS B,  --입원동기";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034374') AS C1, --과거병력 없음";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034376') AS C2, --과거병력 모름";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034378') AS C3, --과거병력 고혈압";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034379') AS C4, --과거병력 당뇨";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034770') AS C5, --과거병력 결핵";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034771') AS C6, --과거병력 간염";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_1') AS C7, --과거병력 기타";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_2') AS C8, --과거병력 기타 내용";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034772') AS C9, --과거병력 뇌졸중";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034773') AS C10, --과거병력 심장질환";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034774') AS C11,  --과거병력 암";
                        SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strA = dt.Rows[0]["A"].ToString().Trim();
                            strB = dt.Rows[0]["B"].ToString().Trim();
                            strC = "";
                            if (dt.Rows[0]["C1"].ToString().Trim() == "true")
                            {
                                strC = strC + " 없음 ";
                            }
                            if (dt.Rows[0]["C2"].ToString().Trim() == "true")
                            {
                                strC = strC + " 모름 ";
                            }
                            if (dt.Rows[0]["C3"].ToString().Trim() == "true")
                            {
                                strC = strC + " 고혈압 ";
                            }
                            if (dt.Rows[0]["C4"].ToString().Trim() == "true")
                            {
                                strC = strC + " 당뇨 ";
                            }
                            if (dt.Rows[0]["C5"].ToString().Trim() == "true")
                            {
                                strC = strC + " 결핵 ";
                            }
                            if (dt.Rows[0]["C6"].ToString().Trim() == "true")
                            {
                                strC = strC + " 간염 ";
                            }
                            if (dt.Rows[0]["C9"].ToString().Trim() == "true")
                            {
                                strC = strC + " 뇌졸중 ";
                            }
                            if (dt.Rows[0]["C10"].ToString().Trim() == "true")
                            {
                                strC = strC + " 심장질환 ";
                            }
                            if (dt.Rows[0]["C11"].ToString().Trim() == "true")
                            {
                                strC = strC + " 암 ";
                            }
                            if (dt.Rows[0]["C7"].ToString().Trim() == "true")
                            {
                                strC = strC + " 기타 ";
                            }
                            if (dt.Rows[0]["C8"].ToString().Trim() == "")
                            {
                                strC = strC + "(" + dt.Rows[0]["C8"].ToString().Trim() + ")";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                        break;
                    // FORMNO : 2305 간호정보조사지(신생아용)
                    case "2305":
                        SQL = "";
                        SQL = " SELECT extractValue(chartxml, '//it35') A, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta1') B, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik11') C1, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik12') C2, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik13') C3, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik14') C4, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik15') C5, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik16') C6, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik17') C7, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik18') C8, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik19') C9, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it38') C10,";
                        SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SQL = SQL + ComNum.VBLF + " UNION ALL";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002168') AS A,  --주증상";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000028908') AS B,  --입원동기";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034374') AS C1, --과거병력 없음";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034376') AS C2, --과거병력 모름";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034377') AS C3, --과거병력 폐렴";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034378') AS C4, --과거병력 장염";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034379') AS C5, --과거병력 경련";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034380') AS C6, --과거병력 미숙아";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034381') AS C7, --과거병력 황달";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034382') AS C8, --과거병력 패혈증";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_1') AS C9, --과거병력 기타";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_2') AS C10, --과거병력 기타 내용";
                        SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strA = dt.Rows[0]["A"].ToString().Trim();
                            strB = dt.Rows[0]["B"].ToString().Trim();
                            strC = "";
                            if (dt.Rows[0]["C1"].ToString().Trim() == "true")
                            {
                                strC = strC + " 없음 ";
                            }
                            if (dt.Rows[0]["C2"].ToString().Trim() == "true")
                            {
                                strC = strC + " 모름 ";
                            }
                            if (dt.Rows[0]["C3"].ToString().Trim() == "true")
                            {
                                strC = strC + " 폐렴 ";
                            }
                            if (dt.Rows[0]["C4"].ToString().Trim() == "true")
                            {
                                strC = strC + " 장염 ";
                            }
                            if (dt.Rows[0]["C5"].ToString().Trim() == "true")
                            {
                                strC = strC + " 경련 ";
                            }
                            if (dt.Rows[0]["C6"].ToString().Trim() == "true")
                            {
                                strC = strC + " 미숙아 ";
                            }
                            if (dt.Rows[0]["C7"].ToString().Trim() == "true")
                            {
                                strC = strC + " 황달 ";
                            }
                            if (dt.Rows[0]["C8"].ToString().Trim() == "true")
                            {
                                strC = strC + " 폐혈증 ";
                            }
                            if (dt.Rows[0]["C9"].ToString().Trim() == "true")
                            {
                                strC = strC + " 기타 ";
                            }
                            if (dt.Rows[0]["C10"].ToString().Trim() != "")
                            {
                                strC = strC + "(" + dt.Rows[0]["C10"].ToString().Trim() + ")";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                        break;
                    // FORMNO : 2294 간호정보조사지(소아용)
                    case "2294":
                        SQL = "";
                        SQL = " SELECT extractValue(chartxml, '//it34') A, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta1') B, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik13') C1, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik14') C2, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik15') C3, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik16') C4, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik17') C5, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik18') C6, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik19') C7, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik20') C8, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik21') C9, ";
                        SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it37') C10,";
                        SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;


                        SQL = SQL + ComNum.VBLF + " UNION ALL";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002168') AS A,  --주증상";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000028908') AS B,  --입원동기";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034374') AS C1, --과거병력 없음";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034376') AS C2, --과거병력 모름";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035316') AS C3, --과거병력 폐렴";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035317') AS C4, --과거병력 장염";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035070') AS C5, --과거병력 요로감염";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035318') AS C6, --과거병력 경련";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034773') AS C7, --과거병력 심장질환";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000034774') AS C8, --과거병력 암";
                        SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_1') AS C9, --과거병력 기타";
                        SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034383_2') AS C10, --과거병력 기타 내용";
                        SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strA = dt.Rows[0]["A"].ToString().Trim();
                            strB = dt.Rows[0]["B"].ToString().Trim();
                            strC = "";
                            if (dt.Rows[0]["C1"].ToString().Trim() == "true")
                            {
                                strC = strC + " 없음 ";
                            }
                            if (dt.Rows[0]["C2"].ToString().Trim() == "true")
                            {
                                strC = strC + " 모름 ";
                            }
                            if (dt.Rows[0]["C3"].ToString().Trim() == "true")
                            {
                                strC = strC + " 폐렴 ";
                            }
                            if (dt.Rows[0]["C4"].ToString().Trim() == "true")
                            {
                                strC = strC + " 장염 ";
                            }
                            if (dt.Rows[0]["C5"].ToString().Trim() == "true")
                            {
                                strC = strC + " 요로감염 ";
                            }
                            if (dt.Rows[0]["C6"].ToString().Trim() == "true")
                            {
                                strC = strC + " 경련 ";
                            }
                            if (dt.Rows[0]["C7"].ToString().Trim() == "true")
                            {
                                strC = strC + " 심장질환 ";
                            }
                            if (dt.Rows[0]["C8"].ToString().Trim() == "true")
                            {
                                strC = strC + " 암 ";
                            }
                            if (dt.Rows[0]["C9"].ToString().Trim() == "true")
                            {
                                strC = strC + " 기타 ";
                            }
                            if (dt.Rows[0]["C10"].ToString().Trim() != "")
                            {
                                strC = strC + "(" + dt.Rows[0]["C10"].ToString().Trim() + ")";
                            }
                        }

                        dt.Dispose();
                        dt = null;
                        break;
                }

                if (strA != "" || strB != "" || strC != "")
                {
                    ssNurInfo_Sheet1.Cells[1, 1].Text = strA;
                    ssNurInfo_Sheet1.Cells[2, 1].Text = strB;
                    ssNurInfo_Sheet1.Cells[3, 1].Text = strC;

                    ssNurInfo_Sheet1.Cells[0, 2].Text = strName + "/" + strChartDate;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Nur_Info_Er(string argPano, string argInDate, string argOutDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strEmrNo = "";
            string strFormNo = "";

            string strTemp = "";
            string strA = "";
            string strB = "";
            string strC = "";

            string strName = "";
            string strChartDate = "";

            argInDate = argInDate.Replace("-", "");
            argOutDate = argOutDate.Replace("-", "");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // FORMNO : 1836 응급의료센터 간호정보조사지
                // FORMNO : 2442 간호정보조사지(응급의료센터)
                // FORMNO : 2486 간호정보조사지(응급의료센터-1)
                SQL = "";
                SQL = " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO IN (2678)";

                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO  = 2678";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();

                    strName = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["USEID"].ToString().Trim());
                    strChartDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strChartDate = strChartDate + " " + ComFunc.FormatStrToDate(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                }

                dt.Dispose();
                dt = null;

                if (strEmrNo == "")
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT extractValue(chartxml, '//it16') A1, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it17') A2, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it16') A3, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it18') A4, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it19') A5, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it20') A6, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik11') B1, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik12') B2, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik13') B3, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ik14') B4, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta1') C,";
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002018') AS A1,  --혈압 SBP";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001765') AS A2,  --혈압 DBP";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014815') AS A3,  --맥박";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002009') AS A4,  --호흡";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001811') AS A5,  --체온";
                SQL = SQL + ComNum.VBLF + " '' AS A6,  --?";

                SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000011727') AS B1,   --의식상태 ";
                SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000010573') AS B2,   --의식상태 ";
                SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014736') AS B3,   --의식상태 ";
                SQL = SQL + ComNum.VBLF + " (SELECT DECODE(ITEMVALUE, '0', 'False', 'true') FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034157') AS B4,   --의식상태 ";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000034158') AS C,    --내원동기";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strTemp = "";
                    strTemp = strTemp + "  ▶혈압:" + dt.Rows[0]["A1"].ToString().Trim() + "/" + dt.Rows[0]["A2"].ToString().Trim() + "mmHg" + ComNum.VBLF;
                    strTemp = strTemp + VB.Left("  ▶맥박:" + dt.Rows[0]["A3"].ToString().Trim() + "회/min" + VB.Space(20), 20) + "▶호흡:" + dt.Rows[0]["A4"].ToString().Trim() + "회/min" + ComNum.VBLF;
                    strTemp = strTemp + VB.Left("  ▶체온:" + dt.Rows[0]["A5"].ToString().Trim() + "℃" + VB.Space(24), 24);
                    strA = strTemp;
                    strB = "";
                    if (dt.Rows[0]["B1"].ToString().Trim() == "true")
                    {
                        strB = strB + " Alert ";
                    }
                    if (dt.Rows[0]["B2"].ToString().Trim() == "true")
                    {
                        strB = strB + " Verbal R ";
                    }
                    if (dt.Rows[0]["B3"].ToString().Trim() == "true")
                    {
                        strB = strB + " Pain R ";
                    }
                    if (dt.Rows[0]["B4"].ToString().Trim() == "true")
                    {
                        strB = strB + " Unconsciousness ";
                    }
                    strC = dt.Rows[0]["C"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strA != "" || strB != "" || strC != "")
                {
                    ssNurInfoEr_Sheet1.Cells[1, 1].Text = strA;
                    ssNurInfoEr_Sheet1.Cells[2, 1].Text = strB;
                    ssNurInfoEr_Sheet1.Cells[3, 1].Text = strC;

                    ssNurInfoEr_Sheet1.Cells[0, 2].Text = strName + "/" + strChartDate;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Ipwon_Info_Er(string argPano, string argInDate, string argOutDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strEmrNo = "";
            string strFormNo = "";

            string strA = "";
            string strB = "";

            string strName = "";
            string strChartDate = "";

            argInDate = argInDate.Replace("-", "");
            argOutDate = argOutDate.Replace("-", "");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // FORMNO : 2356 간호정보조사지 (산모)
                // FORMNO : 2311 간호정보조사지 
                // FORMNO : 2305 간호정보조사지 (신생아용)
                // FORMNO : 2294 간호정보조사지 (소아용)
                SQL = "";
                SQL = " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO  = 2605";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, FORMNO,";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO  = 2605";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();

                    strName = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["USEID"].ToString().Trim());
                    strChartDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strChartDate = strChartDate + " " + ComFunc.FormatStrToDate(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                }

                dt.Dispose();
                dt = null;

                if (strEmrNo == "")
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT extractValue(chartxml, '//ta3') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta5') B, ";
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000014279') AS A1,  --최종진단";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000014623_2') AS A2,  --O_2";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strA = dt.Rows[0]["A"].ToString().Trim();
                    strB = dt.Rows[0]["B"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strA != "" || strB != "")
                {
                    ssErCho_Sheet1.Cells[1, 1].Text = strA;
                    ssErCho_Sheet1.Cells[2, 1].Text = strB;

                    ssErCho_Sheet1.Cells[0, 2].Text = strName + "/" + strChartDate;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Ipwon_Info(string argPano, string argInDate, string argOutDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strEmrNo = "";
            string strFormNo = "";

            string strA = "";
            string strB = "";

            string strPART01 = "";
            string strPART01_2 = "";
            string strPART02 = "";
            string strPART02_2 = "";

            string strName = "";
            string strChartDate = "";

            argInDate = argInDate.Replace("-", "");
            argOutDate = argOutDate.Replace("-", "");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region 신규          
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = B.PART01) A,";               
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = B.PART02) B, ";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = B.PART02_2) B2, ";
                SQL = SQL + ComNum.VBLF + " CHARTUSEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMR_CAREPLAN_CHART B";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO   = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + argOutDate + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strA = dt.Rows[0]["A"].ToString().Trim();
                    strB = dt.Rows[0]["B"].ToString().Trim();
                    if (dt.Rows[0]["B2"].ToString().Trim() != "")
                    {
                        strB = strB + ComNum.VBLF + dt.Rows[0]["B2"].ToString().Trim();
                    }

                    strName = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["CHARTUSEID"].ToString().Trim());
                    strChartDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strChartDate = strChartDate + " " + ComFunc.FormatStrToDate(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                }

                dt.Dispose();
                dt = null;

                if (strA != "" || strB != "")
                {
                    ssIpCho_Sheet1.Cells[1, 1].Text = strA;
                    ssIpCho_Sheet1.Cells[2, 1].Text = strB;

                    ssIpCho_Sheet1.Cells[0, 2].Text = strName + "/" + strChartDate;
                    return;
                }
                #endregion

                SQL = "";
                SQL = " SELECT A.EMRNO, A.FORMNO, B.PART01, B.PART01_2, B.PART02, B.PART02_2,";
                SQL = SQL + ComNum.VBLF + " A.USEID, A.CHARTDATE, A.CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST A, " + ComNum.DB_EMR + "EMR_CAREPLAN_CHART B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + argInDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + argOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN = '1' ";
                SQL = SQL + ComNum.VBLF + "   AND B.UPDATENO = 1 ";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO = B.FORMNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();

                    strPART01 = dt.Rows[0]["PART01"].ToString().Trim();
                    strPART01_2 = dt.Rows[0]["PART01_2"].ToString().Trim();
                    strPART02 = dt.Rows[0]["PART02"].ToString().Trim();
                    strPART02_2 = dt.Rows[0]["PART02_2"].ToString().Trim();

                    strName = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["USEID"].ToString().Trim());
                    strChartDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strChartDate = strChartDate + " " + ComFunc.FormatStrToDate(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                }

                dt.Dispose();
                dt = null;

                if (strEmrNo == "")
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT extractValue(chartxml, '//" + strPART01.ToLower() + "') A, ";
                if (strPART01_2 != "")
                {
                    SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//" + strPART01_2.ToLower() + "') A2, ";
                }
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//" + strPART02.ToLower() + "') B, ";
                if (strPART02_2 != "")
                {
                    SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//" + strPART02_2.ToLower() + "') B2, ";
                }
                SQL = SQL + ComNum.VBLF + " USEID, CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strA = dt.Rows[0]["A"].ToString().Trim();

                    if (strPART01_2 != "")
                    {
                        strA = strA + ComNum.VBLF + dt.Rows[0]["A2"].ToString().Trim();
                    }

                    strB = dt.Rows[0]["B"].ToString().Trim();

                    if (strPART02_2 != "")
                    {
                        strB = strB + ComNum.VBLF + dt.Rows[0]["B2"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (strA != "" || strB != "")
                {
                    ssIpCho_Sheet1.Cells[1, 1].Text = strA;
                    ssIpCho_Sheet1.Cells[2, 1].Text = strB;

                    ssIpCho_Sheet1.Cells[0, 2].Text = strName + "/" + strChartDate;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void chkNurDel_CheckedChanged(object sender, EventArgs e)
        {
            Read_NurProblem(FstrPTNO, FstrInDate);
        }

        private void frmCarePlan_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Screen_Clear();

            if (fstrIPDNO == "")
            {
                this.Close();
                return;
            }

            View_Data();
        }

        void View_Data()
        {
            ComFunc cf = new ComFunc();

            Screen_Clear();

            Read_PatInfo();

            clsType.User.Sabun = FstrUSEID;

            FstrInDate = FstrInDate.Replace("-", "");
            FstrInDate = VB.Left(FstrInDate, 4) + "-" + VB.Mid(FstrInDate, 5, 2) + "-" + VB.Right(FstrInDate, 2);

            lblTitleSub0.Text = "병동 : " + FstrWard + "병동     병실 : " + FstrROOM + "호     환자명/등록번호 : " + FstrSname + "/" + FstrPTNO + "     입원일자 : " + FstrInDate;

            Nur_Info(FstrPTNO, FstrInDate, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            FstrInDate = FstrInDate.Replace("-", "");
            FstrInDate = VB.Left(FstrInDate, 4) + "-" + VB.Mid(FstrInDate, 5, 2) + "-" + VB.Right(FstrInDate, 2);

            Nur_Info_Er(FstrPTNO, FstrInDate, cf.DATE_ADD(clsDB.DbCon, FstrInDate, 1));

            Ipwon_Info(FstrPTNO, FstrInDate, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            FstrInDate = FstrInDate.Replace("-", "");
            FstrInDate = VB.Left(FstrInDate, 4) + "-" + VB.Mid(FstrInDate, 5, 2) + "-" + VB.Right(FstrInDate, 2);

            Ipwon_Info_Er(FstrPTNO, FstrInDate, cf.DATE_ADD(clsDB.DbCon, FstrInDate, 1));
            Read_DietIndicator();
            Read_Diet();
            Read_Detail_Fall(FstrPTNO, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), fstrIPDNO, FstrAge);
            Read_Detail_Braden(FstrPTNO, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), fstrIPDNO, FstrAge, FstrWard);
            Read_Detail_Pain(fstrIPDNO, FstrPTNO);
            Read_NurProblem(FstrPTNO, FstrInDate);
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            using (frmCarePlanForm frm = new frmCarePlanForm())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        void Screen_Clear()
        {
            if (clsType.User.Sabun == "16109")
            {
                btnCode.Visible = true;
            }

            ssIpCho_Sheet1.Cells[1, 1, 2, 1].Text = "";
            ssIpCho_Sheet1.Cells[0, 2].Text = "";

            ssErCho_Sheet1.Cells[1, 1, 2, 1].Text = "";
            ssErCho_Sheet1.Cells[0, 2].Text = "";

            ssNurInfo_Sheet1.Cells[1, 1, 3, 1].Text = "";
            ssNurInfo_Sheet1.Cells[0, 2].Text = "";

            ssNurInfoEr_Sheet1.Cells[1, 1, 3, 1].Text = "";
            ssNurInfoEr_Sheet1.Cells[0, 2].Text = "";

            ssDiet_Sheet1.Cells[4, 2].Text = "";
            ssDiet_Sheet1.Cells[6, 2].Text = "";

            ssIndicator_Sheet1.Cells[1, 1, 3, 1].Text = "";
            ssIndicator_Sheet1.Cells[1, 3, 3, 3].Text = "";
            ssIndicator_Sheet1.Cells[1, 5, 3, 5].Text = "";

            lblTitleSub0.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Read_PatInfo()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT AGE, WARDCODE, ROOMCODE, SNAME, DEPTCODE, SEX ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + fstrIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + FstrPTNO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                FstrSname = dt.Rows[0]["SNAME"].ToString().Trim();
                FstrAge = dt.Rows[0]["AGE"].ToString().Trim();
                FstrWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                FstrROOM = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                FstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                FstrSex = dt.Rows[0]["SEX"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_Diet()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT RATING ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + fstrIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND CERTISABUN IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BUILDDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                switch (dt.Rows[0]["RATING"].ToString().Trim())
                {
                    case "1":
                        ssDiet_Sheet1.Cells[6, 2].Text = "양호군";
                        break;
                    case "2":
                        ssDiet_Sheet1.Cells[6, 2].Text = "중위험군";
                        break;
                    case "3":
                        ssDiet_Sheet1.Cells[6, 2].Text = "고위험군";
                        break;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            View_Data();
        }

        void Read_DietIndicator()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strP1 = "";
            string strP2 = "";
            string strP3 = "";
            string strP4 = "";
            string strP5 = "";
            string strP6 = "";
            string strP7 = "";
            string strP8 = "";

            string strDIET1 = "";
            string strDIET2 = "";
            string strDIET3 = "";
            string strDIET4 = "";

            string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PIBW, P1, ALBUMIN, P2, HB, P3, ";
                SQL = SQL + ComNum.VBLF + " TLC, P4, TCHOL, P5, AGE, P6,";
                SQL = SQL + ComNum.VBLF + " CAREFOOD, LTUBE, TPN, P7,";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM1 , DIETPROBLEM2, DIETPROBLEM3, DIETPROBLEM4, P8";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + fstrIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY BUILDDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strTemp = "";

                if (VB.Val(dt.Rows[0]["P1"].ToString().Trim()) > 0)
                {
                    strP1 = "※IBW:" + dt.Rows[0]["PIBW"].ToString().Trim() + "% ";
                }

                if (VB.Val(dt.Rows[0]["P2"].ToString().Trim()) > 0)
                {
                    strP2 = "※Albumin:" + dt.Rows[0]["ALBUMIN"].ToString().Trim() + " ";
                }

                if (VB.Val(dt.Rows[0]["P3"].ToString().Trim()) > 0)
                {
                    strP3 = "※Hb:" + dt.Rows[0]["HB"].ToString().Trim() + " ";
                }

                if (VB.Val(dt.Rows[0]["P4"].ToString().Trim()) > 0)
                {
                    strP4 = "※TLC:" + dt.Rows[0]["TLC"].ToString().Trim() + " ";
                }

                if (VB.Val(dt.Rows[0]["P5"].ToString().Trim()) > 0)
                {
                    strP5 = "※T.Chol:" + dt.Rows[0]["TCHOL"].ToString().Trim() + " ";
                }

                if (VB.Val(dt.Rows[0]["P6"].ToString().Trim()) > 0)
                {
                    strP6 = "※나이:" + dt.Rows[0]["AGE"].ToString().Trim() + " ";
                }

                strDIET1 = "";
                strDIET2 = "";
                strDIET3 = "";
                strDIET4 = "";

                if (VB.Val(dt.Rows[0]["P7"].ToString().Trim()) > 0)
                {
                    if (dt.Rows[0]["CAREFOOD"].ToString().Trim() == "1")
                    {
                        strDIET1 = "치료식섭취 ";
                    }

                    if (dt.Rows[0]["LTUBE"].ToString().Trim() == "1")
                    {
                        strDIET2 = "Tube Feeding";
                    }

                    if (dt.Rows[0]["TPN"].ToString().Trim() == "1")
                    {
                        strDIET3 = "TPN";
                    }

                    strP7 = "※식사처방:" + strDIET1 + strDIET2 + strDIET3 + " ";
                }

                strDIET1 = "";
                strDIET2 = "";
                strDIET3 = "";
                strDIET4 = "";

                if (VB.Val(dt.Rows[0]["P8"].ToString().Trim()) > 0)
                {
                    if (dt.Rows[0]["DIETPROBLEM1"].ToString().Trim() == "1")
                    {
                        strDIET1 = "연하곤란 ";
                    }

                    if (dt.Rows[0]["DIETPROBLEM2"].ToString().Trim() == "1")
                    {
                        strDIET2 = "구토 ";
                    }

                    if (dt.Rows[0]["DIETPROBLEM3"].ToString().Trim() == "1")
                    {
                        strDIET3 = "설사 ";
                    }

                    if (dt.Rows[0]["DIETPROBLEM4"].ToString().Trim() == "1")
                    {
                        strDIET3 = "변비 ";
                    }

                    strP8 = "※식사 시 문제점:" + strDIET1 + strDIET2 + strDIET3 + strDIET4 + " ";
                }

                strTemp = strP1 + strP2 + strP3 + strP4 + strP5 + strP6 + strP7 + strP8;

                dt.Dispose();
                dt = null;

                ssDiet_Sheet1.Cells[4, 2].Text = strTemp;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_Detail_Fall(string argPano, string argDate, string argIPDNO, string argAge)
        {
            //여기꺼 바꾸면 CARE PLAN 것도 바꿔야 함.

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strFall = "";

            string strTOTAL = "";
            string strCAUSE = "";
            string strDrug = "";
            string strTemp = "";

            string strTOOL = "";

            string strWARD_C = "";
            string strAGE_C = "";

            strFall = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            strWARD_C = "중환자실 재원 환자";
                            break;
                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            strWARD_C = "신생아실 재원 환자";
                            break;
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 70)
                    {
                        strFall = "OK";
                        strAGE_C = "70세 이상 환자";
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 7)
                    {
                        strFall = "OK";
                        strAGE_C = "7세 미만 환자";
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "  SELECT PANO, TOTAL ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + " AND IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                strTOOL = "The Morse Fall Scale";

                if (VB.Val(argAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "'";
                    SQL = SQL + ComNum.VBLF + " AND IPDNO = " + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                    strTOOL = "The Humpty Dumpty Scale";
                }

                //신생아의 경우 도구표 사용하지 않음
                if (strWARD_C == "신생아실 재원 환자")
                {
                    strTOOL = "";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                    if ((VB.Val(argAge) < 18 && VB.Val(strTOTAL) >= 12) || (VB.Val(argAge) >= 18 && VB.Val(strTOTAL) >= 51))
                    {
                        strFall = "OK";
                    }
                }

                dt.Dispose();
                dt = null;

                
                strCAUSE = "";
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND (WARNING1 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING6 = '1')";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";
                    strCAUSE = "";
                    if (strAGE_C == "")
                    {
                        if (dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                        { strCAUSE = strCAUSE + "70세이상 "; }

                        if (dt.Rows[0]["WARNING6"].ToString().Trim() == "1")
                        { strCAUSE = strCAUSE + "7세미만 "; }

                    }
                }

                dt.Dispose();
                dt = null;

                strDrug = "";
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_FALL_EVAL";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND (DRUG_01 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_02 = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_03 = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_04 = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_06 = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR PAT_CHANGE2 = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR PAT_CHANGE = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR TRANFOR = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR OP = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR RELEX = '1' ";
                SQL = SQL + ComNum.VBLF + "                  OR FALL = '1' ";
                SQL = SQL + ComNum.VBLF + " )";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";
                    strDrug = "";
                    if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    { strDrug = strDrug + "진정제 "; }

                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    { strDrug = strDrug + "수면제 "; }

                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    { strDrug = strDrug + "향정신성약품 "; }

                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    { strDrug = strDrug + "항우울제 "; }

                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    { strDrug = strDrug + "이뇨제 "; }

                    if (dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "주요상태변화 "; }

                    if (dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "의식상태변화 "; }

                    if (dt.Rows[0]["TRANFOR"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "전동 시 "; }

                    if (dt.Rows[0]["OP"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "수술/시술 후 "; }

                    if (dt.Rows[0]["RELEX"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "진검치료(검사) "; }

                    if (dt.Rows[0]["FALL"].ToString().Trim() == "1")
                    { strCAUSE = strCAUSE + "낙상 발생 시 "; }
                }

                dt.Dispose();
                dt = null;


                ssIndicator_Sheet1.Cells[1, 1].Text = strTOTAL;

                if (strFall == "OK")
                {
                    ssIndicator_Sheet1.Cells[2, 1].Text = "고위험 ";
                }

                strTemp = "";

                if (strWARD_C != "")
                {
                    strTemp = strTemp + strWARD_C;
                    strTemp = strTemp + ComNum.VBLF;
                }

                if (strAGE_C != "")
                {
                    strTemp = strTemp + strAGE_C;
                    strTemp = strTemp + ComNum.VBLF;
                }

                if (strCAUSE != "")
                {
                    strTemp = strTemp + strCAUSE;
                    strTemp = strTemp + ComNum.VBLF;
                }

                if (strDrug != "")
                {
                    strTemp = strTemp + "-위험약물:" + strDrug;
                }

                ssIndicator_Sheet1.Cells[3, 1].Text = strTemp;
                ssIndicator_Sheet1.Cells[4, 1].Text = strTOOL;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_Detail_Braden(string argPano, string argDate, string argIPDNO, string argAge, string argWard, string argDate2 = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBraden = "";
            string strGubun = "";
            string strTOTAL = "";
            string strBUN = "";
            string strTOOL = "";


            strBraden = "";
            strBUN = "";

            if (argWard == "NR" || argWard == "ND" || argWard == "IQ")
            {
                strGubun = "신생아";
                strTOOL = "신생아욕창사정 도구표";
            }
            else if (VB.Val(argAge) < 5)
            {
                strGubun = "소아";
                strTOOL = "소아욕창사정 도구표";
            }
            else
            {
                strGubun = "";
                strTOOL = "욕창사정 도구표";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strGubun == "")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPano + "' ";
                    if (argDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + argDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                        if ((VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18) || (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16))
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if (strGubun == "소아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_CHILD ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + argPano + "' ";
                    if (argDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + argDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if (strGubun == "신생아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_BRADEN_SCALE_BABY ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + argIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "' ";
                    if (argDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + argDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                        if (VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 20)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = " SELECT *";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( ";
                SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                SQL = SQL + ComNum.VBLF + "      )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strBraden = "OK";
                    strBUN = "";
                    if (dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "중환자실 ";
                    }
                    if (dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "중증도 분류 3군 이상 ";
                    }
                    if (dt.Rows[0]["PARAL"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "뇌, 척추 관련 마비 ";
                    }
                    if (dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "부종 ";
                    }
                    if (dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "영양불량 ";
                    }
                    if (dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "단백질 불량 ";
                    }
                    if (dt.Rows[0]["EDEMA"].ToString().Trim() == "1")
                    {
                        strBUN = strBUN + "부동 ";
                    }
                    //if (dt.Rows[0]["BRADEN"].ToString().Trim() == "1")
                    //{
                    //    strYOK = "현재 욕창이 있는 환자 ";
                    //}
                }

                dt.Dispose();
                dt = null;

                ssIndicator_Sheet1.Cells[1, 3].Text = strTOTAL;

                if (strBraden == "OK")
                {
                    ssIndicator_Sheet1.Cells[2, 3].Text = "고위험 ";
                }

                ssIndicator_Sheet1.Cells[3, 3].Text = strBUN;
                ssIndicator_Sheet1.Cells[4, 3].Text = strTOOL;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_Detail_Pain(string argIPDNO, string argPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL = SQL + ComNum.VBLF + "  MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL = SQL + ComNum.VBLF + "  DRUG, NODRUG, TIMES";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_PAIN_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE)    ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssIndicator_Sheet1.Cells[1, 5].Text = dt.Rows[0]["SCORE"].ToString().Trim();
                ssIndicator_Sheet1.Cells[2, 5].Text = dt.Rows[0]["REGION"].ToString().Trim();
                ssIndicator_Sheet1.Cells[4, 5].Text = dt.Rows[0]["TOOLS"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnDiet_Click(object sender, EventArgs e)
        {
            using (frmDietIndicator frmD = new frmDietIndicator())
            {
                frmD.StartPosition = FormStartPosition.CenterParent;
                frmD.ShowDialog(this);
            }
        }

        private void ssDiet_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strROWID = "";
            string strRE = "";
            string strGUBUN1 = "";
            string strGUBUN2 = "";

            string GstrHelpCode = "";
            string GstrHelpName = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID, CERTISABUN, NEED_SABUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_RATING_PATIENT_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + fstrIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY BUILDDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 1)
                {
                    GstrHelpCode = fstrIPDNO;

                    if (frmBasDietEvaluationListX != null)
                    {
                        frmBasDietEvaluationListX.Dispose();
                        frmBasDietEvaluationListX = null;
                    }

                    frmBasDietEvaluationListX = new frmBasDietEvaluationList(GstrHelpCode);
                    frmBasDietEvaluationListX.StartPosition = FormStartPosition.CenterParent;
                    frmBasDietEvaluationListX.rEventClose += FrmBasDietEvaluationListX_rEventClose;
                    frmBasDietEvaluationListX.ShowDialog();
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                strGUBUN1 = dt.Rows[0]["CERTISABUN"].ToString().Trim();
                strGUBUN2 = dt.Rows[0]["NEED_SABUN"].ToString().Trim();

                if (strROWID != "")
                {
                    GstrHelpName = strRE;
                    GstrHelpCode = strROWID;

                    if (strGUBUN1 != "")
                    {
                        frmDietEvaluationNew frmN = new frmDietEvaluationNew("CAREPLAN", GstrHelpCode, GstrHelpName);
                        frmN.ShowDialog();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void FrmBasDietEvaluationListX_rEventClose()
        {
            if (frmBasDietEvaluationListX != null)
            {
                frmBasDietEvaluationListX.Dispose();
                frmBasDietEvaluationListX = null;
            }
        }

        private void ssIndicator_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSCORE = "";

            string GstrHelpCode = "";
            string gsWard = "";

            gsWard = FstrWard;

            switch (e.Column)
            {
                case 1:
                    strSCORE = ssIndicator_Sheet1.Cells[1, 1].Text.Trim();
                    GstrHelpCode = fstrIPDNO;

                    switch (ssIndicator_Sheet1.Cells[4, 1].Text.Trim())
                    {
                        case "The Humpty Dumpty Scale":
                            FrmPatBun4Child frmC = new FrmPatBun4Child("CAREPLAN", GstrHelpCode, gsWard);
                            frmC.ShowDialog();
                            frmC.Dispose();
                            frmC = null;
                            break;
                        default:
                            frmPatBun4New3 frmN = new frmPatBun4New3("CAREPLAN", GstrHelpCode, gsWard);
                            frmN.ShowDialog();
                            frmN.Dispose();
                            frmN = null;
                            break;
                    }

                    GstrHelpCode = "";
                    break;
                case 3:
                    strSCORE = ssIndicator_Sheet1.Cells[1, 3].Text.Trim();
                    GstrHelpCode = FstrSname + "^^" + FstrPTNO + "^^" + FstrSex + "/" + FstrAge + "^^" + FstrROOM + "^^" + FstrDeptCode + "^^" + fstrIPDNO;

                    switch (ssIndicator_Sheet1.Cells[4, 3].Text.Trim())
                    {
                        case "신생아욕창사정 도구표":
                            frnPatBun3 frm3 = new frnPatBun3(FstrSname, FstrPTNO, FstrSex, FstrAge, FstrROOM, FstrDeptCode, fstrIPDNO, gsWard, "3");
                            frm3.ShowDialog();
                            frm3.Dispose();
                            frm3 = null;
                            break;
                        case "소아욕창사정 도구표":
                            frnPatBun3 frm2 = new frnPatBun3(FstrSname, FstrPTNO, FstrSex, FstrAge, FstrROOM, FstrDeptCode, fstrIPDNO, gsWard, "2");
                            frm2.ShowDialog();
                            frm2.Dispose();
                            frm2 = null;
                            break;
                        default:
                            frnPatBun3 frm1 = new frnPatBun3(FstrSname, FstrPTNO, FstrSex, FstrAge, FstrROOM, FstrDeptCode, fstrIPDNO, gsWard, "1");
                            frm1.ShowDialog();
                            frm1.Dispose();
                            frm1 = null;
                            break;
                    }

                    GstrHelpCode = "";
                    break;
                case 5:
                    strSCORE = ssIndicator_Sheet1.Cells[1, 5].Text.Trim();
                    GstrHelpCode = fstrIPDNO;
                    frmAcheDetail frmAche = new frmAcheDetail(GstrHelpCode, "");
                    frmAche.ShowDialog();
                    GstrHelpCode = "";
                    break;
            }
        }

        void Read_NurProblem(string argPano, string argMEDFRDATE)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssNurProblem_Sheet1.RowCount = 1;

            argMEDFRDATE = argMEDFRDATE.Replace("-", "");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.RANKING, A.SEQNO, B.NURPROBLEM, A.GOAL, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.EDATE,'YYYY-MM-DD') EDATE, A.BIGO, A.ROWID, '1' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMR_CADEX_NURPROBLEM A, " + ComNum.DB_EMR + "EMR_CADEX_NURPROBLEM_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.MEDFRDATE = '" + argMEDFRDATE + "' ";
                if (chkNurDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.EDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT RANKING, 0, PROBLEM, GOAL, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, '' BIGO, A.ROWID, '2' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_CARE_GOAL A";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.INDATE = TO_DATE('" + argMEDFRDATE + "','YYYY-MM-DD')  ";
                if (chkNurDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssNurProblem_Sheet1.RowCount = dt.Rows.Count + 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssNurProblem_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["NURPROBLEM"].ToString().Trim();
                    ssNurProblem_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["GOAL"].ToString().Trim();
                    ssNurProblem_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssNurProblem_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
    }
}
