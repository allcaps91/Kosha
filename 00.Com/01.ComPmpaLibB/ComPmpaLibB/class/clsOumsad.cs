using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Diagnostics;
using System.Threading;
using ComPmpaLibB;
using System.Runtime.InteropServices;

namespace ComPmpaLibB
{
    /// <summary>
    /// Description : 원무모듈(OUMSAD.bas)
    /// Author : 박병규
    /// Create Date : 2017.08.08
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="OUMSAD.BAS"/> 

    public class clsOumsad
    {
        FarPoint.Win.Spread.FpSpread GssSpreadILL = null;

        [DllImport("User32")]
        public static extern int ShowWindow(IntPtr handle, int ShowMode);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private const int WM_SHOWNOACTIVATE = 4;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        public string FJinSucode = string.Empty;

        public void Load_Account_Set(PsmhDb pDbCon)
        {
            DataTable DtSa = new DataTable();
            string SQL = "";
            string SqlErr = "";

            long nAmt = 0;
            long nAmt_Old = 0;
            long nIAmt = 0;  //일반금액
            long nIAmt_Old = 0;//일반금액
            string strSunext = "";
            //string strSuDate = "";

            clsPmpaPb.GnJinAmts[1] = 0;           //진찰료 초진
            clsPmpaPb.GnJinAmts[2] = 0;           //진찰료 초진심야/휴일
            clsPmpaPb.GnJinAmts[3] = 0;           //진찰료 재진
            clsPmpaPb.GnJinAmts[4] = 0;           //진찰료 재진심야/휴일
            clsPmpaPb.GnJinAmts[5] = 0;           //진찰료 초진특진료
            clsPmpaPb.GnJinAmts[6] = 0;           //진찰료 재진특진료
            clsPmpaPb.GnJinAmts[7] = 0;           //진찰료 물리치료
            clsPmpaPb.GnJinAmts[8] = 0;           //진찰료 환자가족
            clsPmpaPb.GnJinAmts[9] = 0;           //진찰료 금연처방(초진)
            clsPmpaPb.GnJinAmts[10] = 0;          //진찰료 금연처방(재진)
            clsPmpaPb.GnJinAmts[11] = 0;          //의료질평가지원금
            clsPmpaPb.GnJinAmts[12] = 0;          //교육수련분야지원금

            clsPmpaPb.GnJinAmts1[1] = 0;          //진찰료 초진
            clsPmpaPb.GnJinAmts1[2] = 0;          //진찰료 초진심야
            clsPmpaPb.GnJinAmts1[3] = 0;          //진찰료 재진
            clsPmpaPb.GnJinAmts1[4] = 0;          //진찰료 재진심야
            clsPmpaPb.GnJinAmts1[5] = 0;          //진찰료 초진특진료
            clsPmpaPb.GnJinAmts1[6] = 0;          //진찰료 재진특진료
            clsPmpaPb.GnJinAmts1[7] = 0;          //진찰료 물리치료
            clsPmpaPb.GnJinAmts1[8] = 0;          //진찰료 환자가족
            clsPmpaPb.GnJinAmts1[9] = 0;          //진찰료 초진휴일
            clsPmpaPb.GnJinAmts1[10] = 0;         //진찰료 재진휴일
            clsPmpaPb.GnJinAmts1[11] = 0;         //NP단일수가 접수비

            clsPmpaPb.GnJinAmts3[1] = 0;          //진찰료 초진(일반)
            clsPmpaPb.GnJinAmts3[2] = 0;          //진찰료 초진심야(일반)
            clsPmpaPb.GnJinAmts3[3] = 0;          //진찰료 재진(일반)
            clsPmpaPb.GnJinAmts3[4] = 0;          //진찰료 재진심야(일반)
            clsPmpaPb.GnJinAmts3[5] = 0;          //진찰료 초진특진료(일반)
            clsPmpaPb.GnJinAmts3[6] = 0;          //진찰료 재진특진료(일반)
            clsPmpaPb.GnJinAmts3[7] = 0;          //진찰료 물리치료(일반)
            clsPmpaPb.GnJinAmts3[8] = 0;          //진찰료 환자가족(일반)
            clsPmpaPb.GnJinAmts3[9] = 0;          //진찰료 초진휴일(일반)
            clsPmpaPb.GnJinAmts3[10] = 0;         //진찰료 재진휴일(일반)
            clsPmpaPb.GnJinAmts3[11] = 0;         //NP단일수가 접수비(일반)


            clsPmpaPb.GnJinAmts1_Old[1] = 0;      //진찰료 초진
            clsPmpaPb.GnJinAmts1_Old[2] = 0;      //진찰료 초진심야
            clsPmpaPb.GnJinAmts1_Old[3] = 0;      //진찰료 재진
            clsPmpaPb.GnJinAmts1_Old[4] = 0;      //진찰료 재진심야
            clsPmpaPb.GnJinAmts1_Old[5] = 0;      //진찰료 초진특진료
            clsPmpaPb.GnJinAmts1_Old[6] = 0;      //진찰료 재진특진료
            clsPmpaPb.GnJinAmts1_Old[7] = 0;      //진찰료 물리치료
            clsPmpaPb.GnJinAmts1_Old[8] = 0;      //진찰료 환자가족
            clsPmpaPb.GnJinAmts1_Old[9] = 0;      //진찰료 초진휴일
            clsPmpaPb.GnJinAmts1_Old[10] = 0;     //진찰료 재진휴일

            clsPmpaPb.GnJinAmts2[1] = 0;          //진찰료 초진(내과)
            clsPmpaPb.GnJinAmts2[2] = 0;          //진찰료 초진(외과)
            clsPmpaPb.GnJinAmts2[3] = 0;          //진찰료 초진(기타)
            clsPmpaPb.GnJinAmts2[4] = 0;          //진찰료 초진심야(내과)
            clsPmpaPb.GnJinAmts2[5] = 0;          //진찰료 초진심야(외과)
            clsPmpaPb.GnJinAmts2[6] = 0;          //진찰료 초진심야(기타)
            clsPmpaPb.GnJinAmts2[7] = 0;          //진찰료 재진(내과)
            clsPmpaPb.GnJinAmts2[8] = 0;          //진찰료 재진(외과)
            clsPmpaPb.GnJinAmts2[9] = 0;          //진찰료 재진(기타)
            clsPmpaPb.GnJinAmts2[10] = 0;         //진찰료 재진심야(내과)
            clsPmpaPb.GnJinAmts2[11] = 0;         //진찰료 재진심야(외과)
            clsPmpaPb.GnJinAmts2[12] = 0;         //진찰료 재진심야(기타)
            clsPmpaPb.GnJinAmts2[13] = 0;         //진찰료 재진화자가족(내과)
            clsPmpaPb.GnJinAmts2[14] = 0;         //진찰료 재진환자가족(외과)
            clsPmpaPb.GnJinAmts2[15] = 0;         //진찰료 재진환자가족(기타)

            clsPmpaPb.GnGAmt1 = 0;
            clsPmpaPb.GnGAmt2 = 0;
            clsPmpaPb.GnGAmt3 = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUNEXT,                                      --01 품명코드 ";
                SQL += ComNum.VBLF + "        BAMT,  Iamt,                                      --02 보험수가 ";
                SQL += ComNum.VBLF + "        TO_CHAR(SUDATE, 'YYYY-MM-DD') SUDATE,        --03 수가변경일자 ";
                SQL += ComNum.VBLF + "        OLDBAMT,OLDIAMT                                      --04 이전보험수가 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
                SQL += ComNum.VBLF + "    AND SUNEXT IN ( ";
                SQL += ComNum.VBLF + "        'AA100', 'AA1001','AA200','AA2001','AA223','AA223A', ";
                SQL += ComNum.VBLF + "        'AA146','AA156','AA166','AA246','AA256','AA266', ";
                SQL += ComNum.VBLF + "        'AA1461','AA1561','AA1661','AA2461','AA2561','AA2661','AA222', ";
                SQL += ComNum.VBLF + "        'AA346','AA356','AA366',";
                SQL += ComNum.VBLF + "        'AA176SC','AA276SC', ";
                SQL += ComNum.VBLF + "        'AA176','AA1761','AA1762', ";
                SQL += ComNum.VBLF + "        'AA276','AA2761','AA2762',";
                SQL += ComNum.VBLF + "        'AA376','AA333','A1011','AR100', ";
                SQL += ComNum.VBLF + "        'AU214','AU312')  ";
                SqlErr = clsDB.GetDataTableEx(ref DtSa, SQL, pDbCon);

                for (int i = 0; i < DtSa.Rows.Count; i++)
                {
                    nAmt = Convert.ToInt64(DtSa.Rows[i]["BAMT"].ToString());
                    nAmt_Old = Convert.ToInt64(DtSa.Rows[i]["OLDBAMT"].ToString());
                    nIAmt = Convert.ToInt64(DtSa.Rows[i]["IAMT"].ToString());
                    nIAmt_Old = Convert.ToInt64(DtSa.Rows[i]["OLDIAMT"].ToString());
                    strSunext = DtSa.Rows[i]["SUNEXT"].ToString().Trim();

                    if (DateTime.Compare(Convert.ToDateTime(clsPublic.GstrSysDate), Convert.ToDateTime(DtSa.Rows[i]["SUDATE"].ToString())) < 0)
                    {
                        nAmt = Convert.ToInt64(DtSa.Rows[i]["OLDBAMT"].ToString());
                        nIAmt = Convert.ToInt64(DtSa.Rows[i]["OLDIAMT"].ToString());
                    }
                  

                    switch (strSunext)
                    {
                        case "AA100": clsPmpaPb.GnJinAmts[1] = nAmt; break;     //진찰료 초진
                        case "AA1001": clsPmpaPb.GnJinAmts[2] = nAmt; break;    //진찰료 초진심야/휴일
                        case "AA200": clsPmpaPb.GnJinAmts[3] = nAmt; break;     //진찰료 재진
                        case "AA2001": clsPmpaPb.GnJinAmts[4] = nAmt; break;    //진찰료 재진심야/휴일
                        case "AA223": clsPmpaPb.GnJinAmts[7] = nAmt; break;     //진찰료 물리치료/ 주사멀티
                        case "AA222": clsPmpaPb.GnJinAmts[7] = nAmt; break;     //진찰료 물리치료/ 주사멀티
                        case "AA223A": clsPmpaPb.GnJinAmts[8] = nAmt; break;    //진찰료 환자가족
                        case "AA176SC": clsPmpaPb.GnJinAmts[9] = nAmt; break;   //진찰료 금연처방(초진)
                        case "AA276SC": clsPmpaPb.GnJinAmts[10] = nAmt; break;  //진찰료 금연처방(재진)
                        case "AU214": clsPmpaPb.GnJinAmts[11] = nAmt; break;    //의료질평가 지원금       2015-08-31
                        case "AU312": clsPmpaPb.GnJinAmts[12] = nAmt; break;    //교육수련분야 지원금     2015-08-31

                        //2002-01-01부터사용-----------------------------------------
                        case "AA176": clsPmpaPb.GnJinAmts1[1] = nAmt; break;    //진찰료 초진
                        case "AA1761": clsPmpaPb.GnJinAmts1[2] = nAmt; break;   //진찰료 초진심야/휴일
                        case "AA276": clsPmpaPb.GnJinAmts1[3] = nAmt; break;    //진찰료 재진
                        case "AA2761": clsPmpaPb.GnJinAmts1[4] = nAmt; break;   //진찰료 재진심야/휴일
                        case "AA333": clsPmpaPb.GnJinAmts1[7] = nAmt; break;    //진찰료 물리치료 /주사멀티
                        //case "AA376": clsPmpaPb.GnJinAmts1[8] = nAmt; break;    //진찰료 환자가족
                        case "AA256": clsPmpaPb.GnJinAmts1[8] = nAmt; break;    //진찰료 환자가족
                        case "AA1762": clsPmpaPb.GnJinAmts1[9] = nAmt; break;   //진찰료 초진 휴일
                        case "AA2762": clsPmpaPb.GnJinAmts1[10] = nAmt; break;  //진찰료 재진 휴일
                        case "A1011": clsPmpaPb.GnJinAmts1[11] = nAmt; break;   //NP단일수가 접수비
                        case "AR100": clsPmpaPb.GnJinAmts1[11] = nAmt; break;   //NP단일수가 접수비
                        //------------------------------------------------------------

                        case "AA146": clsPmpaPb.GnJinAmts2[1] = nAmt; break;    //진찰료 초진(내과)
                        case "AA156": clsPmpaPb.GnJinAmts2[2] = nAmt; break;    //진찰료 초진(외과)
                        case "AA166": clsPmpaPb.GnJinAmts2[3] = nAmt; break;    //진찰료 초진(기타)
                        case "AA1461": clsPmpaPb.GnJinAmts2[4] = nAmt; break;    //진찰료 초진심야(내과)
                        case "AA1561": clsPmpaPb.GnJinAmts2[5] = nAmt; break;    //진찰료 초진심야(외과)
                        case "AA1661": clsPmpaPb.GnJinAmts2[6] = nAmt; break;    //진찰료 초진심야(기타)
                        case "AA246": clsPmpaPb.GnJinAmts2[7] = nAmt; break;    //진찰료 재진(내과)
                       // case "AA256": clsPmpaPb.GnJinAmts2[8] = nAmt; break;    //진찰료 재진(외과)
                        case "AA266": clsPmpaPb.GnJinAmts2[9] = nAmt; break;    //진찰료 재진(기타)
                        case "AA2461": clsPmpaPb.GnJinAmts2[10] = nAmt; break;   //진찰료 재진심야(내과)
                        case "AA2561": clsPmpaPb.GnJinAmts2[11] = nAmt; break;   //진찰료 재진심야(외과)
                        case "AA2661": clsPmpaPb.GnJinAmts2[12] = nAmt; break;   //진찰료 재진심야(기타)
                        case "AA346": clsPmpaPb.GnJinAmts2[13] = nAmt; break;   //진찰료 재진환자가족(내과)
                        case "AA356": clsPmpaPb.GnJinAmts2[14] = nAmt; break;   //진찰료 재진환자가족(외과)
                        case "AA366": clsPmpaPb.GnJinAmts2[15] = nAmt; break;   //진찰료 재진환자가족(기타)
                    }

                    switch (strSunext)
                    {
                        case "AA176": clsPmpaPb.GnJinAmts1_Old[1] = nAmt_Old; break;    //진찰료 초진
                        case "AA1761": clsPmpaPb.GnJinAmts1_Old[2] = nAmt_Old; break;   //진찰료 초진심야/휴일
                        case "AA276": clsPmpaPb.GnJinAmts1_Old[3] = nAmt_Old; break;    //진찰료 재진
                        case "AA2761": clsPmpaPb.GnJinAmts1_Old[4] = nAmt_Old; break;   //진찰료 재진심야/휴일
                        case "AA333": clsPmpaPb.GnJinAmts1_Old[7] = nAmt_Old; break;    //진찰료 물리치료 /주사멀티
                        case "AA376": clsPmpaPb.GnJinAmts1_Old[8] = nAmt_Old; break;    //진찰료 환자가족
                        case "AA1762": clsPmpaPb.GnJinAmts1_Old[9] = nAmt_Old; break;   //진찰료 초진 휴일
                        case "AA2762": clsPmpaPb.GnJinAmts1_Old[10] = nAmt_Old; break;  //진찰료 재진 휴일
                    }


                    switch (strSunext)
                    {
                        //2020-01-01부터사용 일반 수가 -----------------------------------------
                        case "AA176": clsPmpaPb.GnJinAmts3[1] = nIAmt; break;    //진찰료 초진
                        case "AA1761": clsPmpaPb.GnJinAmts3[2] = nIAmt; break;   //진찰료 초진심야/휴일
                        case "AA276": clsPmpaPb.GnJinAmts3[3] = nIAmt; break;    //진찰료 재진
                        case "AA2761": clsPmpaPb.GnJinAmts3[4] = nIAmt; break;   //진찰료 재진심야/휴일
                        case "AA333": clsPmpaPb.GnJinAmts3[7] = nIAmt; break;    //진찰료 물리치료 /주사멀티
                        case "AA256": clsPmpaPb.GnJinAmts3[8] = nIAmt; break;    //진찰료 환자가족
                        case "AA1762": clsPmpaPb.GnJinAmts3[9] = nIAmt; break;   //진찰료 초진 휴일
                        case "AA2762": clsPmpaPb.GnJinAmts3[10] = nIAmt; break;  //진찰료 재진 휴일
                        case "A1011": clsPmpaPb.GnJinAmts3[11] = nIAmt; break;   //NP단일수가 접수비
                        case "AR100": clsPmpaPb.GnJinAmts3[11] = nIAmt; break;   //NP단일수가 접수비
                                                                                //------------------------------------------------------------
                    }
                }
                DtSa.Dispose();
                DtSa = null;

                clsPmpaPb.GnJinAmts[5] = 0;       //진찰료 초진특진료
                clsPmpaPb.GnJinAmts[6] = 0;       //진찰료 재진특진료

                //↓↓ 2018년 이후 특진료제도 적용안함

                ////진찰료 초진특진료
                //strSuDate = "";

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT ";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                //SQL += ComNum.VBLF + "    AND SUCODE    = 'AA176' ";
                //SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                //SQL += ComNum.VBLF + "  ORDER By SUDATE DESC ";
                //SqlErr = clsDB.GetDataTableEx(ref DtSa, SQL, pDbCon);

                //if (DtSa.Rows.Count > 0)
                //{
                //    strSuDate = DtSa.Rows[0]["SUDATE"].ToString();

                //    if (DateTime.Compare(Convert.ToDateTime(strSuDate), Convert.ToDateTime(clsPublic.GstrSysDate)) > 0)
                //        strSuDate = DtSa.Rows[1]["SUDATE"].ToString();
                //}

                //DtSa.Dispose();
                //DtSa = null;

                //if (strSuDate == "")
                //{
                //    MessageBox.Show("초진진찰료(특진) 적용일자를 확인 할 수 없습니다. 전산실로 문의해주십시오.", "사용불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    clsDB.DisDBConnect(pDbCon);
                //    Application.Exit();
                //}

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT SUCODE, SUDATE, IAMT, ";
                //SQL += ComNum.VBLF + "        BAMT, TAMT, SAMT, ";
                //SQL += ComNum.VBLF + "        SELAMT";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                //SQL += ComNum.VBLF + "    AND SUCODE    = 'AA176'";
                //SQL += ComNum.VBLF + "    AND SUDATE    = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                //SQL += ComNum.VBLF + "  ORDER BY SUDATE DESC  ";
                //SqlErr = clsDB.GetDataTableEx(ref DtSa, SQL, pDbCon);

                //if (DtSa.Rows.Count > 0)
                //    clsPmpaPb.GnJinAmts[5] = Convert.ToInt64(DtSa.Rows[0]["SELAMT"].ToString());

                //DtSa.Dispose();
                //DtSa = null;

                //if (clsPmpaPb.GnJinAmts[5] == 0)
                //    MessageBox.Show("초진진찰료(특진) 금액이 세팅되지 않았습니다. (0원)" + '\r' + "금액을 확인해주십시오.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ////진찰료 재진특진료
                //strSuDate = "";

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                //SQL += ComNum.VBLF + "    AND SUCODE    = 'AA276' ";
                //SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                //SQL += ComNum.VBLF + "  ORDER By SUDATE DESC ";
                //SqlErr = clsDB.GetDataTableEx(ref DtSa, SQL, pDbCon);

                //if (DtSa.Rows.Count > 0)
                //{
                //    strSuDate = DtSa.Rows[0]["SUDATE"].ToString();

                //    if (DateTime.Compare(Convert.ToDateTime(strSuDate), Convert.ToDateTime(clsPublic.GstrSysDate)) > 0)
                //        strSuDate = DtSa.Rows[1]["SUDATE"].ToString();
                //}

                //DtSa.Dispose();
                //DtSa = null;

                //if (strSuDate == "")
                //{
                //    MessageBox.Show("재진진찰료(특진) 적용일자를 확인 할 수 없습니다. 전산실로 문의해주십시오.", "사용불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    clsDB.DisDBConnect(pDbCon);
                //    Application.Exit();
                //}

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT SUCODE, SUDATE, IAMT, BAMT, TAMT, SAMT, SELAMT";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                //SQL += ComNum.VBLF + "    AND SUCODE    = 'AA276' ";
                //SQL += ComNum.VBLF + "    AND SUDATE    = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                //SQL += ComNum.VBLF + "  ORDER BY SUDATE DESC";
                //SqlErr = clsDB.GetDataTableEx(ref DtSa, SQL, pDbCon);

                //if (DtSa.Rows.Count > 0)
                //    clsPmpaPb.GnJinAmts[6] = Convert.ToInt64(DtSa.Rows[0]["SELAMT"].ToString());

                //DtSa.Dispose();
                //DtSa = null;

                //if (clsPmpaPb.GnJinAmts[6] == 0)
                //    MessageBox.Show("재진진찰료(특진) 금액이 세팅되지 않았습니다. (0원)" + '\r' + "금액을 확인해주십시오.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// Description : 외래 진찰료 계산 및 각 부담율 별 금액산정
        /// Author : 박병규
        /// Create Date : 2017.08.23
        /// <param name="pDbCon"></param>
        /// <param name="Ptno"></param>
        /// <param name="JinSel">진찰료수납여부(OPD_MASTER JIN 구분)</param>
        /// <param name="ChoSel">초재진구분(초진(1),재진(3),초진심야(2), 재진심야(4), 초진휴일(5), 재진휴일(6))</param>
        /// <param name="SpcSel">특진여부(1/0)</param>
        /// <param name="GamSel">감액구분(자격)</param>
        /// <param name="GamCSel">감액구분(CASE)</param>
        /// <param name="BiSel">환자구분</param>
        /// <param name="Jangae">장애구분</param>
        /// <param name="Dept">진료과</param>
        /// <param name="Doct">(예약)의사코드</param>
        /// <param name="BDate">예약진료일자</param>
        /// <param name="GyeJin">계약처후불(1)</param>
        /// <param name="MCode">희귀난치성코드</param>
        /// <param name="VCode">중증(암)</param>
        /// <param name="Gubun">진찰료없음</param>
        /// <param name="Argilban2">외국인 일반 2배</param>
        /// <param name="ArgJinDtl">접수상세구분(OPD_MASTER JINDTL구분)</param>
        /// <param name="ArgJiwon">의료질평가 지원금 산정 제외여부(OK/Null)</param>
        /// </summary>
        /// <seealso cref="OUMSAD.bas : Jin_Amt_Account"/>
        public void Jin_Amt_Account(PsmhDb pDbCon, string Ptno, string JinSel, int ChoSel, int SpcSel, string GamSel, string GamCSel, string BiSel, string Jangae, string Dept, string Doct, string BDate, int GyeJin, string MCode, string VCode, string Gubun, string Argilban2, string ArgJinDtl, string ArgJiwon = "")
        {
            clsIpdAcct cIAcct = new ComPmpaLibB.clsIpdAcct();
            clsOumsad cO = new clsOumsad();
            clsPmpaFunc cPF = new ComPmpaLibB.clsPmpaFunc();
            clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();
            DataTable DtSad = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strJuminNo = "";

            clsPmpaPb.gnJinAMT1 = 0;        //진찰료 발생금액
            clsPmpaPb.gnJinAMT2 = 0;        //진찰료 특진료
            clsPmpaPb.gnJinAMT3 = 0;        //진찰료 총액
            clsPmpaPb.gnJinAMT4 = 0;        //진찰료 조합부담
            clsPmpaPb.gnJinAMT5 = 0;        //진찰료 감액
            clsPmpaPb.gnJinAMT6 = 0;        //진찰료 미수금액
            clsPmpaPb.gnJinAMT7 = 0;        //진찰료 영수금액 

            clsPmpaPb.GnGAmt1 = 0;          //의료질평가지원금 발생금액
            clsPmpaPb.GnGAmt2 = 0;          //교육수련분야지원금 발생금액
            clsPmpaPb.GnGAmt3 = 0;          //기술분야지원금 발생금액

            clsPmpaPb.GnJinDanAmt = 0;      //진찰료 절사금액
            clsPmpaPb.GnJinAmtTel = 0;      //전화접수 진찰료
            clsPmpaPb.GnJinAmtTel2 = 0;     //전화접수 진찰료
            clsPmpaPb.GstrJinBonFlag = "";  //진찰료 본인부담율

            clsAlert cA = new ComPmpaLibB.clsAlert();

            //접수구분, 접수 상세구분, 진료과에 따라 접수비 없음 또는 후불처리
            if (Rtn_NoAmt_Jin_Gubun(JinSel)) { return; }
            if (Rtn_NoAmt_JinDtl_Gubun(ArgJinDtl)) { return; }
            //진단서재발급시 수납(0)으로 값변경
            if (JinSel == "7") { JinSel = "0"; return; }
            //인공신장
            if (Dept == "HD") { return; }
            if (Gubun == "1") { return; }

            //조건에 따른 부담율 차이로 인해 후불로 지정
            if (BiSel == "22" && JinSel != "I" && JinSel != "J" && Dept == "NP" && DateTime.Compare(Convert.ToDateTime(BDate), Convert.ToDateTime(clsPmpaPb.GstrNPRateDay)) < 0) { return; }

            #region //본인부담율 코드값 READ
            //건강보험 유형 통합 (11,12,13 >> 11)
            cBON.BI = BiSel;
            if (cBON.BI == "") { return; }
            cBON.SDATE = BDate;
            //기준일자 세팅
            strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
            //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상)
            
            cBON.MCODE = MCode;
            cBON.VCODE = VCode;
            cBON.DEPT = Dept;
            //입원시 면제코드 구분
            cBON.OGPDBUN = "";
            //수납구분
            cBON.JIN = JinSel;
            //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
            cBON.FCODE = "";
            if (ArgJinDtl == "22")
                cBON.FCODE = "03";
            else if (ArgJinDtl == "25")
                cBON.FCODE = "02";

            //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
            if (VB.Left(cBON.BI, 1) == "1" && cBON.DEPT == "DT")
            {
                if (ArgJinDtl != "02" || ArgJinDtl != "07")
                    cBON.DEPT = "**";
            }

            if (Ptno.Trim() != "")
            {
                cBON.IO = "I";
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaPb.GnAge, strJuminNo, BDate, cBON.IO);
                //***입원 본인부담율 세팅
                if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false )
                {
                    if (Dept == "ER") { cA.Alert_BonRate(cBON); }
                }
                  

                if (Jangae == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.IBR.Jin = 0;
                        clsPmpaType.IBR.Bohum = 0;
                        clsPmpaType.IBR.CTMRI = 0;
                    }
                }
            }

            //2018.05.31 박병규 : 입원본인부담율 구하면서 cBON 변수값을 치환시키므로 외래본인부담율 구할때 다시 조건을 설정해준다
            //건강보험 유형 통합 (11,12,13 >> 11)
            cBON.BI = BiSel;
            if (cBON.BI == "") { return; }
            cBON.SDATE = BDate;
            //기준일자 세팅
            strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
            //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상, 5 1세미만)
            
            cBON.MCODE = MCode;
            cBON.VCODE = VCode;
            cBON.DEPT = Dept;
            //입원시 면제코드 구분
            cBON.OGPDBUN = "";
            //수납구분
            cBON.JIN = JinSel;
            //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
            cBON.FCODE = "";
            if (ArgJinDtl == "22")
                cBON.FCODE = "03";
            else if (ArgJinDtl == "25")
                cBON.FCODE = "02";

            //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
            if ( cBON.DEPT == "DT")
            {
                if (ArgJinDtl != "02" && ArgJinDtl != "07")
                    cBON.DEPT = "**";
            }

            cBON.JINDTL = ArgJinDtl;

            if (Ptno.Trim() != "")
            {
                cBON.IO = "O";
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaPb.GnAge, strJuminNo, BDate, cBON.IO);
                //***외래 본인부담율 세팅
                if (cO.Read_OBon_Rate(pDbCon, cBON) == false)
                {
                    cA.Alert_BonRate(cBON);
                   // return;
                }   
                    
                if (Jangae == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.OBR.Jin = 0;
                        clsPmpaType.OBR.Bohum = 0;
                        clsPmpaType.OBR.CTMRI = 0;
                    }
                }

            }
            #endregion

            //진찰료 발생금액 계산
            AMT1_Gesan(pDbCon, ref BDate, ref ChoSel, ref JinSel, ref ArgJinDtl, ref BiSel, ref Dept, ref Jangae, ref Argilban2, ref ArgJiwon);
            
            //진찰료 특진료 계산 : 2018년 제도폐지
            //AMT2_Gesan(pDbCon, ref ChoSel, ref SpcSel, ref Dept, ref Doct);

            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2;//진찰료총액

            //진찰료 조합부담 계산
            AMT4_Gesan(ref BDate, ref BiSel, ref Jangae, ref JinSel, ref Dept, ref MCode, ref ArgJinDtl);
            //진찰료 감액 계산
            AMT5_Gesan(ref BiSel, ref JinSel, ref Dept, ref GamSel, ref GamCSel, ref MCode);


            //진찰료 계산
            if (JinSel == "1" || JinSel == "5" || Jangae == "1" || Jangae == "2" || GyeJin == 1 && MCode != "H000" && JinSel != "E")
            {
                clsPmpaPb.gnJinAMT6 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4 - clsPmpaPb.gnJinAMT5;
                clsPmpaPb.gnJinAMT6 = (double)Math.Truncate((clsPmpaPb.gnJinAMT6 / 10)) * 10;
            }
            else
            {
                clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4 - clsPmpaPb.gnJinAMT5;
                clsPmpaPb.gnJinAMT7 = (double)Math.Truncate((clsPmpaPb.gnJinAMT7 / 10)) * 10;
            }

            //영수금액이 -금액 발생하면 GnJinAmt7 = 0으로 함. 원무과장님이 요청
            if (clsPmpaPb.gnJinAMT7 < 0)
                clsPmpaPb.gnJinAMT7 = 0;

            //의료급여1종인 수납시 접수을 받음.
            if (Convert.ToInt32(VB.Val(BiSel)) == 21)
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
            }

            if (Convert.ToInt32(VB.Val(BiSel)) == 22 && JinSel == "Q")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0; 
            }

            if (Convert.ToInt32(VB.Val(BiSel)) == 22 && MCode == "B099")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0; 
            }
            
            //차상위2종은 접수비 없이 처리하고 수납시 처방코드에 따라 접수코드적용.
            if ((Convert.ToInt32(VB.Val(BiSel)) >= 11 && Convert.ToInt32(VB.Val(BiSel)) <= 13) && 
                (MCode == "E000" || MCode == "F000" || (MCode == "V000" && (clsPmpaPb.GstrCanCer == "V206" || clsPmpaPb.GstrCanCer == "V231" || clsPmpaPb.GstrCanCer == "V246"))))
            {
                if (ArgJinDtl != "12")
                {
                    clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                    clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                    clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0; 
                    clsPmpaPb.GnJinDanAmt = 0;
                }
            }

            //외래부담률 처방에 따른 후불계산
            if (Convert.ToInt32(VB.Val(BiSel)) < 30 && ArgJinDtl == "22")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }

            //외래부담률 처방에 따른 후불계산
            if (Convert.ToInt32(VB.Val(BiSel)) < 30 && ArgJinDtl == "23")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }

            //외래부담률 처방에 따른 후불계산
            if (Convert.ToInt32(VB.Val(BiSel)) < 30 && ArgJinDtl == "25")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }

            // 건강검진 결핵 유소견자 면제 후불계산
            if (Convert.ToInt32(VB.Val(BiSel)) < 30 && ArgJinDtl == "29")
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }
            if (JinSel == "5" )
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }

        }

        //진찰료 발생금액 계산
        public void AMT1_Gesan(PsmhDb pDbCon, ref string BDate, ref int ChoSel, ref string JinSel, ref string ArgJinDtl, ref string BiSel, ref string Dept, ref string Jangae, ref string Argilban2, ref string ArgJiwon)
        {
            clsBasAcct CPA = new clsBasAcct();

            DataTable DtSad = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            string strCode = "";
            string strGubun = "";
            int nAdd = 0;

            if (BDate.Trim() == "") BDate = DateTime.Today.ToString("yyyy-MM-dd");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NAME, GUBUN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN = 'BAS_초재진구분' ";
            SQL += ComNum.VBLF + "    AND CODE  = " + ChoSel + " ";
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtSad.Rows.Count > 0)
                strCode = DtSad.Rows[0]["GUBUN2"].ToString().Trim();

            DtSad.Dispose();
            DtSad = null;

            if (strCode == "")
            {
                if (ChoSel == 1 || ChoSel == 2 || ChoSel == 5)
                    strCode = "AA156";
                else
                    strCode = "AA256";
            }

            switch (ChoSel)
            {
                case 1://초진
                case 3://재진
                    strGubun = strCode;
                    break;
                case 2://초진심야
                case 4://재진심야
                    strGubun = strCode + "010";
                    break;
                case 5://초진공휴
                case 6://재진공휴
                    strGubun = strCode + "050";
                    break;
            }


            //NP단일수가 접수비
            if ((BiSel == "21" || BiSel == "22") && Dept == "NP" && DateTime.Compare(Convert.ToDateTime(BDate), Convert.ToDateTime(clsPmpaPb.GstrNPRateDay)) < 0)
                strGubun = "AR100";
            //만6세미만(물리치료, 주사멀티는 소아가산 무)
            else if (clsPmpaPb.GnAge < 6 && JinSel != "8" && JinSel != "G" && JinSel != "C" && JinSel != "U" && JinSel != "T")
            {
                if (strGubun.Length == 5)
                    strGubun = strGubun + "600";
                else
                    strGubun = strGubun.Substring(0, 5) + "6" + strGubun.Substring(strGubun.Length - 2, 2); //심야 만6세미만

                //2018.05.26 박병규 : 소아 1세미만 
                if (clsPmpaPb.GnAge == 0)
                    strGubun = strGubun.Substring(0, 5) + "1" + strGubun.Substring(strGubun.Length - 2, 2); //심야 만1세미만 // strCode + "100";
            }

            switch (JinSel)
            {
                //재진료50%(물리치료등)
                case "8"://물리치료
                case "C"://주사멀티
                case "G"://물리상병특례
                case "T"://소아물리상병특례
                case "U"://소아물리상병특례(만6세미만)
                    strGubun = "AA222";
                    break;
            }

            switch (ArgJinDtl)
            {
                //통합재진료50%(보호자내원)
                case "05"://물리치료
                    strGubun = "AA256090";
                    break;
            }

            FJinSucode = strGubun;
            
            clsPmpaPb.gnJinAMT1 = CPA.Read_EDI_SUGA_PCode(FJinSucode, BDate);//진찰료 발생금액

            //2018.06.12 박병규 : 아래루틴을 661줄 라인으로 이동함
            //clsPmpaPb.GnGAmt1 = CPA.Read_EDI_SUGA_PCode("AU233", BDate);//의료질평가지원금 발생금액
            //clsPmpaPb.GnGAmt2 = CPA.Read_EDI_SUGA_PCode("AU313", BDate);//교육수련지원금 발생금액

            if (JinSel == "8" || JinSel == "C" || JinSel == "G" || JinSel == "T" || JinSel == "U")
            {
                clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[7];

                if ((Argilban2 == "1" || Argilban2 == "Y") && BiSel == "51")
                {
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts3[7];
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 * 2;
                }
                    
            }
            

            else if (BiSel == "22" && JinSel != "I" && JinSel != "J" && Dept == "NP")
                clsPmpaPb.gnJinAMT1 = 0;//조건에 따른 부담률 차이로 인해 후불로 지정

            else
            {
                if ( BiSel == "51")
                {
                    if (ChoSel == 5) { ChoSel = 9; }
                    if (ChoSel == 6) { ChoSel = 10; }
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts3[ChoSel]; // '나머지 진찰료


                    if (ArgJinDtl =="05")
                    {
                        clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts3[8];  //보호자 내원
                    }
                    
                    if (Argilban2 == "1" || Argilban2 == "Y")
                    {
                        clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 * 2;
                    }

                }
                
             
                if (BiSel != "21" && BiSel != "22")
                {
                    clsPmpaPb.GnGAmt1 = CPA.Read_EDI_SUGA_PCode("AU233", BDate);//의료질평가지원금 발생금액(AU214)
                    clsPmpaPb.GnGAmt2 = CPA.Read_EDI_SUGA_PCode("AU313", BDate);//교육수련지원금 발생금액(AU312)
                    if (DateTime.Compare(Convert.ToDateTime(BDate), Convert.ToDateTime("2018-09-01")) >= 0) {clsPmpaPb.GnGAmt3 = CPA.Read_EDI_SUGA_PCode("AU413", BDate);}

                    if (ArgJiwon == "OK")
                    {
                        clsPmpaPb.GnGAmt1 = 0;
                        clsPmpaPb.GnGAmt2 = 0;
                        clsPmpaPb.GnGAmt3 = 0;
                    }
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 + clsPmpaPb.GnGAmt1 + clsPmpaPb.GnGAmt2 + clsPmpaPb.GnGAmt3 ;
                }
            }
            
            if (clsPmpaPb.GnAge  < 6 && (JinSel != "8" || JinSel != "C" || JinSel != "G" || JinSel != "T" || JinSel != "U" ) && ArgJinDtl != "05" && BiSel == "51" ) 
            {
                    switch (ChoSel)
                    {
                        case 1://초진
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg1;
                            else
                                nAdd = clsPmpaPb.PedAdd1;

                            break;

                        case 2://초진심야
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg2;
                            else
                                nAdd = clsPmpaPb.PedAdd2;

                            break;

                        case 3://재진
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg3;
                            else
                                nAdd = clsPmpaPb.PedAdd3;

                            break;

                        case 4://재진심야
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg4;
                            else
                                nAdd = clsPmpaPb.PedAdd4;

                            break;

                        case 9://초진휴일
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg5;
                            else
                                nAdd = clsPmpaPb.PedAdd5;

                            break;

                        case 10://재진휴일
                            if (clsPmpaPb.GnAge == 0)
                                nAdd = clsPmpaPb.PedAddYg6;
                            else
                                nAdd = clsPmpaPb.PedAdd6;

                            break;
                }

                clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 + nAdd;
            }
           

            //진찰료 없음(접수||, 신생아, 진단서발급, 예방접종 )
            if (JinSel == "2" || JinSel == "3" || JinSel == "4" || JinSel == "B")
            {
                clsPmpaPb.gnJinAMT1 = 0;
                clsPmpaPb.GnGAmt1 = 0;
                clsPmpaPb.GnGAmt2 = 0;
                clsPmpaPb.GnGAmt3 = 0;
            }

            //진찰료 없음(시설환자(나자렛집))
            if (clsPmpaPb.GstrSi22 == "OK") { clsPmpaPb.gnJinAMT1 = 0; }
            
            //진찰료 없음(의료급여, 인공신장)
            if (string.Compare(BiSel, "20") > 0 && string.Compare(BiSel, "30") < 0 && clsPmpaPb.GstrGwa == "HD")
            {
                clsPmpaPb.gnJinAMT1 = 0;
                clsPmpaPb.GnGAmt1 = 0;
                clsPmpaPb.GnGAmt2 = 0;
                clsPmpaPb.GnGAmt3 = 0;
            }

        }

        //진찰료 발생금액 계산
        public void AMT2_Gesan(PsmhDb pDbCon, ref int ChoSel, ref int SpcSel, ref string Dept, ref string Doct)
        {
            if (SpcSel > 0)
            {
                clsPmpaPb.gnJinAMT2 = 0;

                if (Dept == "GS")
                {
                    if (Doct != "2119")
                        clsPmpaPb.gnJinAMT2 = READ_JINSPC_AMT(pDbCon, ChoSel);
                }
                else
                    clsPmpaPb.gnJinAMT2 = READ_JINSPC_AMT(pDbCon, ChoSel);
            }

        }

        //진찰료 조합부담 계산
        public void AMT4_Gesan(ref string BDate, ref string BiSel, ref string Jangae, ref string JinSel, ref string Dept, ref string MCode, ref string ArgJinDtl)
        {
            //clsPmpaPb.gnJinAMT1 : 진찰료 발생금액
            //clsPmpaPb.gnJinAMT2 : 진찰료 특진료
            //clsPmpaPb.gnJinAMT3 : 진찰료 총액
            //clsPmpaPb.gnJinAMT4 : 진찰료 조합부담
            //clsPmpaPb.gnJinAMT5 : 진찰료 감액
            //clsPmpaPb.gnJinAMT6 : 진찰료 미수
            //clsPmpaPb.gnJinAMT7 : 진찰료 본인부담액

            clsBasAcct CPA = new clsBasAcct();

            clsPmpaPb.GstrJinBonFlag = "0"; //진찰료 본인부담율(0.기타 1.보험60 %, 2.보험45 %, 3.보험20 %, 4.보험10%, A.보호 1, 500원)

            if (BiSel.Equals("22") && Jangae.Equals("3"))//의료급여2종 장애인(조합100%청구)
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
            else if (BiSel.Equals("22") && clsPmpaPb.GstrOtherHD != "*" && (clsPmpaPb.GstrGwa.Equals("HD") || JinSel.Equals("6")))//의료급여2종+동일타과인공신장체크+인공신장 
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                clsPmpaPb.GstrJinBonFlag = "A";
            }
            else if (clsPmpaPb.GstrGwa == "HD" || JinSel == "6" || clsPmpaPb.GstrOtherHD == "*")//인공신장 일반과접수
            {
                if (MCode.Equals("C000"))//차상위계층환자
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                else
                {
                    if (string.Compare(BiSel, "13") <= 0)
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix(((int)(clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);  //HD,6접수 상병특례 희귀난치성,등록희귀난치질환 10% 2009-07-01시행 윤조연추가
                        clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (10 / 100.00)) - VB.Fix((int)(clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100) * 100;
                        clsPmpaPb.GstrJinBonFlag = "4";//본인부담10%
                    }
                    else
                    {
                        if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsPmpaType.IBR.Jin / 100.0)) % 2) == 0)
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (clsPmpaType.IBR.Jin / 100.00) / 10)) * 10);
                        else
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (clsPmpaType.IBR.Jin / 100.00)) / 10 - 0.5) * 10);

                        clsPmpaPb.GstrJinBonFlag = "3";//본인부담20%
                    }
                }

                if (Jangae.Equals("3"))//보호 장애인 진찰료 조합부담
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
            }
            else if (JinSel == "B")
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 / 100.00 / 100);
            else if (BiSel.Equals("22") && JinSel.Equals("I"))//의료급여2종+상병1500
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - 1500;
            else if (BiSel.Equals("22") && JinSel.Equals("J"))//의료급여2종+상병1000
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - 1000;
            else if (BiSel.Equals("22") && Dept.Equals("NP"))//의료급여2종+신경정신과
            {
                if (clsPmpaPb.GstrSPR == "OK") //NP조현병일 경우
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                else
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);
            }
            else if (string.Compare(BiSel, "20") > 0)
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00));
            }
            else
            {
                if (string.Compare(BDate, clsPmpaPb.OBON_DATE) >= 0)
                {
                    if (MCode.Equals("C000"))//차상위계층환자
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (0 / 100));
                    else if (JinSel == "F" || JinSel == "G")//상병특례
                    {
                        if ((MCode.Equals("H000") || MCode.Equals("V000")) && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))    //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);  //상병특례 희귀난치성,등록희귀난치질환 10% 2009-07-01시행 윤조연추가
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (10 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);
                        }
                        else
                        {
                            if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))   //100단위작업 2009-07-01
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (20 / 100.00)) / 100)) * 100);   //20%
                            else
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (20 / 100.00));              //20%
                        }
                    }
                    else if ((JinSel == "R" || JinSel == "U" ) && string.Compare(BDate, "2019-01-01") >= 0 && clsPmpaPb.GnAge == 0)     //소아환자(만 1세미만) 15%
                    {
                        if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (15 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (15 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (15 / 100.00)) / 100)) * 100);
                        }
                       
                    }
                    else if (JinSel == "R" || JinSel == "U")      //소아환자(만 6세미만) 35%
                    {
                        if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (35 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 100)) * 100);
                        }
                        else
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 10 - 0.5) * 10);
                    }
                    else if (JinSel == "S" || JinSel == "T")       //소아환자(만 6세이하) 상병특례 14%
                    {
                        if (MCode == "V000" && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))) //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (10 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);

                        }
                        else if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")) //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (14 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 100)) * 100);
                        }
                        else
                        {
                            if ((clsPmpaPb.gnJinAMT1 * (0.14) % 2) == 0)
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 10) * 10);
                            else
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 10 - 0.5) * 10);
                        }
                    }
                    else if (JinSel == "C" && clsPmpaPb.GnAge <= 5)
                    {
                        if (MCode == "V000" && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))   //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (10 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (10 / 100.00)) / 100)) * 100);

                        }
                        else if (MCode == "H000")
                        {
                            if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (14 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 100)) * 100);
                            }
                            else
                            {
                                if ((clsPmpaPb.gnJinAMT1 * (0.14) % 2) == 0)
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 10) * 10);
                                else
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (14 / 100.00)) / 10 - 0.5) * 10);
                            }
                        }
                        else
                        {
                            if ( string.Compare(BDate, "2019-01-01") >= 0 && clsPmpaPb.GnAge == 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (15 / 100.00)) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (15 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (15 / 100.00)) / 100)) * 100);
                            }
                            else if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (35 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 100)) * 100);
                            }
                            else
                            {
                                if ((clsPmpaPb.gnJinAMT1 * (0.35) % 2) == 0)
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 10) * 10);
                                else
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (35 / 100.00)) / 10 - 0.5) * 10);
                            }
                        }
                    }
                    else
                    {
                        if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * clsPmpaType.OBR.Jin / 100.00) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 100)) * 100);
                        }
                        else
                        {
                            if ((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00) % 2) == 0)
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 10) * 10);
                            else
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 10 - 0.5) * 10);
                        }

                        clsPmpaPb.GstrJinBonFlag = "1"; //본인부담 50%

                        //2014-06-27 급여제한자는 본인부담 100%
                        if (ArgJinDtl == "09")
                        {
                            clsPmpaPb.gnJinAMT4 = 0;
                            clsPmpaPb.GnJinDanAmt = 0;
                            clsPmpaPb.GstrJinBonFlag = "0";
                        }
                    }
                }
                else
                {
                    if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 100)) * 100);
                        clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 100)) * 100);
                    }
                    else
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00));
                    }

                    clsPmpaPb.GstrJinBonFlag = "1"; //본인부담 60%
                }
            }

            if (clsPmpaPb.GstrCanCer != "")
            {
                string strCanCer = clsPmpaPb.GstrCanCer;

                if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))
                {
                    if (strCanCer == "V191" || strCanCer == "V192" || strCanCer == "V193" || strCanCer == "V194")
                    {
                        if (BiSel.Equals("22") && Jangae == "3")  //자격이 22종 장애인 2006-07-24
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                        else
                        {
                            if (MCode != "C000")
                            {
                                if ((BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")) && (strCanCer == "V191" || strCanCer == "V192" || strCanCer == "V193" || strCanCer == "V194"))
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                                    clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (5 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                                }
                                else if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")) //100단위작업 2009-07-01
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) / 100)) * 100);
                                    clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00) / 100) * 100));
                                }
                                else
                                {
                                    if (((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) % 2) == 0)
                                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) / 10) * 10);
                                    else
                                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) / 10 - 0.5) * 10);
                                }
                            }
                        }
                    }
                    else if (strCanCer == "V247" || strCanCer == "V248" || strCanCer == "V249" || strCanCer == "V250") //2010-07-01 중증화상 5%
                    {
                        if (MCode != "C000")
                        {
                            if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (5 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                            }
                            else
                            {
                                if (((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) % 2) == 0)
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) / 10) * 10);
                                else
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (CPA.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.00)) / 10 - 0.5) * 10);
                            }
                        }
                    }
                    else if (MCode == "F003")
                    {
                        if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin) / 100.00) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 100)) * 100);
                        }
                        else
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Convert.ToInt32((clsPmpaPb.gnJinAMT1 * (clsPmpaType.OBR.Jin / 100.00)) / 10) * 10);
                    }
                    else if (MCode == "V000")
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                }
                else if (BiSel.Equals("22"))
                {
                    if (strCanCer == "V247" || strCanCer == "V248" || strCanCer == "V249" || strCanCer == "V250")   //2010-07-01 중증화상 5%
                    {
                        if (MCode != "C000")
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (5 / 100.00)) - (VB.Fix((int)((clsPmpaPb.gnJinAMT1 * (5 / 100.00)) / 100)) * 100);
                        }
                    }
                    else if (strCanCer == "V000")
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                    }
                }
            }

            if (clsPmpaPb.GstatEROVER == "*")//본인부담20%   //응급실 6시간이상,낮병 동
                clsPmpaPb.GstrJinBonFlag = "3";
        }

        public void AMT5_Gesan(ref string BiSel, ref string JinSel, ref string Dept, ref string GamSel, ref string GamCSel, ref string MCode)
        {
            if (Dept.Equals("DT"))
            {
                if (string.Compare(GamSel, "00") > 0 && Convert.ToInt32(BiSel) < 25)//감액대상자+건강보험/의료급여
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.DTJin_Rate / 100.00);
                else if (GamSel == "55" && Convert.ToInt32(BiSel) == 33)//계약처+산재공상
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.00);
                else if (GamSel == "55" && Convert.ToInt32(BiSel) == 51)//계약처+일반
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.00);
                else if (GamSel == "55" && Convert.ToInt32(BiSel) == 41)//계약처+공단100%
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.00);
            }
            else
            {
                if (MCode == "H000")
                {
                    if (JinSel != "F" && JinSel != "G" && JinSel != "S" && JinSel != "T")//F:상병특례 G:물리치료상병특례 S:소아상병특례 T:소아물리치료상병
                    {
                        if (JinSel == "K" || JinSel == "L" || JinSel == "M") //K.RM협진, L.UR협진, M.PET-CT 접수비
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (100.00 / 100);
                        else if ((GamSel == "55" || GamSel == "51") && Convert.ToInt32(BiSel) == 33)
                        {
                            clsPmpaPb.gnJinAMT5 = Convert.ToInt32(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.00));
                            clsPmpaPb.gnJinAMT5 = (int)(Convert.ToInt32(clsPmpaPb.gnJinAMT5 / 10) * 10);
                        }
                        else if ((GamSel == "55" || GamSel == "51") && Convert.ToInt32(BiSel) == 51)
                        {
                            clsPmpaPb.gnJinAMT5 = Convert.ToInt32(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.00));
                            clsPmpaPb.gnJinAMT5 = (int)(Convert.ToInt32(clsPmpaPb.gnJinAMT5 / 10) * 10);
                        }
                        else if (string.Compare(GamSel, "00") > 0 && Convert.ToInt32(BiSel) != 52)
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Jin_Rate / 100.00);
                        }
                    }
                }
                else
                {
                    if (JinSel == "K" || JinSel == "L" || JinSel == "M") //K.RM협진, L.UR협진, M.PET-CT 접수비
                        clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (100.00 / 100);
                    else if ((GamSel == "55" || GamSel == "51") && Convert.ToInt32(BiSel) == 33)
                    {
                        clsPmpaPb.gnJinAMT5 = Convert.ToInt32(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.00));
                        clsPmpaPb.gnJinAMT5 = (int)(Convert.ToInt32((clsPmpaPb.gnJinAMT5 / 10)) * 10);
                    }
                    else if ((GamSel == "55" || GamSel == "51") && Convert.ToInt32(BiSel) == 51)
                    {
                        clsPmpaPb.gnJinAMT5 = Convert.ToInt32(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.00));
                        clsPmpaPb.gnJinAMT5 = (int)(Convert.ToInt32((clsPmpaPb.gnJinAMT5 / 10)) * 10);
                    }
                    else if ((GamSel == "55" || GamSel == "51") && Convert.ToInt32(BiSel) == 41)
                    {
                        clsPmpaPb.gnJinAMT5 = Convert.ToInt32(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.00));
                        clsPmpaPb.gnJinAMT5 = (int)(Convert.ToInt32((clsPmpaPb.gnJinAMT5 / 10)) * 10);
                    }
                    else if (string.Compare(GamSel, "00") > 0 && Convert.ToInt32(BiSel) != 52)
                    {
                        clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT1 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Jin_Rate / 100.00);

                        if (string.Compare(GamCSel, "50") > 0)
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAMC.Jin_Rate / 100.00);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Description : 특진진찰료 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.24
        /// </summary>
        /// <returns></returns>
        public long READ_JINSPC_AMT(PsmhDb pDbCon, int ArgChoSel)
        {
            DataTable DtSad = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;
            string strSuDate = string.Empty;

            strSuDate = "";

            if (ArgChoSel == 1 || ArgChoSel == 2 || ArgChoSel == 5)
            {
                //진찰료 초진특진료
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                SQL += ComNum.VBLF + "    AND SUCODE    = 'AA176' ";
                SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "  ORDER By SUDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

                if (DtSad.Rows.Count > 0)
                {
                    strSuDate = DtSad.Rows[0]["SUDATE"].ToString();

                    if (DateTime.Compare(Convert.ToDateTime(strSuDate), Convert.ToDateTime(clsPublic.GstrSysDate)) > 0)
                        strSuDate = DtSad.Rows[1]["SUDATE"].ToString();
                }

                DtSad.Dispose();
                DtSad = null;

                if (strSuDate == "")
                {
                    ComFunc.MsgBox("초진진찰료(특진) 적용일자를 확인 할 수 없습니다. 전산실로 문의해주십시오.", "사용불가");
                    clsDB.DisDBConnect(pDbCon);
                    Application.Exit();
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUCODE, SUDATE, IAMT, ";
                SQL += ComNum.VBLF + "        BAMT, TAMT, SAMT, ";
                SQL += ComNum.VBLF + "        SELAMT";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                SQL += ComNum.VBLF + "    AND SUCODE    = 'AA176'";
                SQL += ComNum.VBLF + "    AND SUDATE    = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "  ORDER BY SUDATE DESC  ";
                SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

                if (DtSad.Rows.Count > 0)
                    rtnVal = Convert.ToInt64(DtSad.Rows[0]["SELAMT"].ToString());

                DtSad.Dispose();
                DtSad = null;

                if (rtnVal == 0)
                    ComFunc.MsgBox("초진진찰료(특진) 금액이 세팅되지 않았습니다. (0원)" + '\r' + "금액을 확인해주십시오.", "확인요망");

            }
            else
            {
                //진찰료 재진특진료
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                SQL += ComNum.VBLF + "  WHERE 1         = 1  ";
                SQL += ComNum.VBLF + "    AND SUCODE    = 'AA276' ";
                SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "  ORDER By SUDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

                if (DtSad.Rows.Count > 0)
                {
                    strSuDate = DtSad.Rows[0]["SUDATE"].ToString();

                    if (DateTime.Compare(Convert.ToDateTime(strSuDate), Convert.ToDateTime(clsPublic.GstrSysDate)) > 0)
                        strSuDate = DtSad.Rows[1]["SUDATE"].ToString();
                }

                DtSad.Dispose();
                DtSad = null;

                if (strSuDate == "")
                {
                    ComFunc.MsgBox("재진진찰료(특진) 적용일자를 확인 할 수 없습니다. 전산실로 문의해주십시오.", "사용불가");
                    clsDB.DisDBConnect(pDbCon);
                    Application.Exit();
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUCODE, SUDATE, IAMT, BAMT, TAMT, SAMT, SELAMT";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SUCODE    = 'AA276' ";
                SQL += ComNum.VBLF + "    AND SUDATE    = TO_DATE('" + strSuDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "  ORDER BY SUDATE DESC";
                SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

                if (DtSad.Rows.Count > 0)
                    rtnVal = Convert.ToInt64(DtSad.Rows[0]["SELAMT"].ToString());

                DtSad.Dispose();
                DtSad = null;

                if (clsPmpaPb.GnJinAmts[6] == 0)
                    ComFunc.MsgBox("재진진찰료(특진) 금액이 세팅되지 않았습니다. (0원)" + '\r' + "금액을 확인해주십시오.", "확인요망");
            }

            return rtnVal;

        }


        /// <summary>
        /// Description : 외래 진찰료 계산 및 각 부담율 별 금액산정
        /// Author : 김민철
        /// Create Date : 2017.07.14
        /// <param name="JinSel">OPD_MASTER JIN 구분</param>
        /// <param name="ChoSel">초진(1),재진(3),휴일초진(2), 휴일재진(4)...</param>
        /// <param name="SpcSel">특진여부 1 / 0</param>
        /// <param name="GamSel">감액코드 2자리</param>
        /// <param name="BiSel">환자종류 11,12,13...</param>
        /// <param name="Jangae">장애여부 1,2,3</param>
        /// <param name="Dept">진료과</param>
        /// <param name="Doct">의사코드</param>
        /// <param name="BDate">진료일자</param>
        /// <param name="GyeJin">계약처</param>
        /// <param name="MCode">급여코드 C000, E000, F000</param>
        /// <param name="Argilban2">외국인 일반 2배</param>
        /// <param name="ArgJinDtl">OPD_MASTER JINDTL 접수상세구분</param>
        /// <param name="ArgJiwon">의료질평가 지원금 산정 제외여부  OK / Null</param>
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// JinSel, ChoSel, SpcSel, BiSel, MCode, Argilban2
        /// 
        /// SELECT  JINSEL, CHOSEL, SPCSEL, BISEL, MCODE, ARGILBAN2 FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = '' AND DEPTCODE = 'DEPT' AND BDATE = BDATE  
        /// 
        /// <seealso cref="OUMSAD.bas : Jin_Amt_Account"/>
        public void Jin_Amt_Account_OLD(string JinSel, int ChoSel, int SpcSel, string GamSel, string BiSel, string Jangae, string Dept, string Doct, string BDate, int GyeJin, string MCode, string Argilban2, string ArgJinDtl, string ArgJiwon)
        {
            int nPedAdd = 0;
            int nBi = 0;
            //사용안함 입원 예약금 산정 로직 없어짐.
            clsBasAcct clsBAcc = new clsBasAcct();

            clsPmpaPb.gnJinAMT1 = 0;        //진찰료 발생금액
            clsPmpaPb.gnJinAMT2 = 0;        //진찰료 특진료
            clsPmpaPb.gnJinAMT3 = 0;        //진찰료 총액
            clsPmpaPb.gnJinAMT4 = 0;        //진찰료 조합부담
            clsPmpaPb.gnJinAMT5 = 0;        //진찰료 감액
            clsPmpaPb.gnJinAMT6 = 0;        //진찰료 미수금액
            clsPmpaPb.gnJinAMT7 = 0;        //진찰료 영수금액 *2017.07.17.김홍록 : 필요한것...

            clsPmpaPb.GnGAmt1 = 0;
            clsPmpaPb.GnGAmt2 = 0;          //의료질평가 지원금
            clsPmpaPb.GnGAmt3 = 0;          //의료질평가 지원금

            clsPmpaPb.GnJinDanAmt = 0;                   //진찰료 절사금액
            clsPmpaPb.GnJinAmtTel = 0;                   //전화접수 진찰료
            clsPmpaPb.GnJinAmtTel2 = 0;                  //전화접수 진찰료
            clsPmpaPb.GstrJinBonFlag = "";               //진찰료 본인부담율

            nBi = Convert.ToInt16(BiSel);

            #region 접수비 없음 또는 후불처리 조건
            //접수구분, 접수 상세구분에 따라 접수비 없음 또는 후불처리
            if (Rtn_NoAmt_Jin_Gubun(JinSel) || Rtn_NoAmt_JinDtl_Gubun(ArgJinDtl))
            {
                return;
            }

            if (Dept == "HD")
            {
                return;
            }

            if (JinSel.Equals("7"))
            {
                JinSel = "0";
                return;
            }
            #endregion

            #region 진찰료 발생금액 계산

            if (JinSel.Equals("8") || JinSel.Equals("C") || JinSel.Equals("G") || JinSel.Equals("T") || JinSel.Equals("U"))
            {
                clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[7];       //물리치료 진찰료
                if ((Argilban2.Equals("1") || Argilban2.Equals("Y")) && nBi == 51)
                {
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[7] * 2;  //외국new
                }
            }
            else if (ArgJinDtl.Equals("05")) //보호자 내원
            {
                clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[8];       //환자가족 진찰료 AA256
                if ((Argilban2.Equals("1") || Argilban2.Equals("Y")) && nBi == 51)
                {
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[8] * 2;  //외국new
                }

                if (string.Compare(BDate, clsPmpaPb.GstrMedSupDay) >= 0 && nBi != 21 && nBi != 22)
                {
                    clsPmpaPb.GnGAmt1 = clsPmpaPb.GnJinAmts[11];
                    clsPmpaPb.GnGAmt2 = clsPmpaPb.GnJinAmts[12];
                    if (string.Compare(BDate, "2018-09-01") >= 0) { clsPmpaPb.GnGAmt3 = clsPmpaPb.GnJinAmts[13]; }
                    if (ArgJiwon == "OK")
                    {
                        clsPmpaPb.GnGAmt1 = 0;
                        clsPmpaPb.GnGAmt2 = 0;
                        clsPmpaPb.GnGAmt3 = 0;
                    }
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 + clsPmpaPb.GnGAmt1 + clsPmpaPb.GnGAmt2 + clsPmpaPb.GnGAmt3;
                }
            }
            else if ((nBi == 21 || nBi == 22) && Dept.Equals("NP") && string.Compare(BDate, clsPmpaPb.GstrNPRateDay) < 0)
            {
                clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[11];
                if ((Argilban2.Equals("1") || Argilban2.Equals("Y")) && nBi == 51)
                {
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[11] * 2;  //외국new
                }
            }
            else if (nBi == 22 && JinSel != "I" && JinSel != "J" && Dept.Equals("NP") && string.Compare(BDate, clsPmpaPb.GstrNPRateDay) >= 0)
            {
                clsPmpaPb.gnJinAMT1 = 0;   //조건에 따른 부담률 차이로 인해 후불로 지정 'KYO
            }
            else
            {
                if (ChoSel == 5) { ChoSel = 9; }
                if (ChoSel == 6) { ChoSel = 10; }
                clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[ChoSel];  //나머지 진찰료

                if ((Argilban2.Equals("1") || Argilban2.Equals("Y")) && nBi == 51)
                {
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.GnJinAmts1[ChoSel] * 2;  //외국new
                }

                if (string.Compare(BDate, clsPmpaPb.GstrMedSupDay) >= 0 && nBi != 21 && nBi != 22)
                {
                    clsPmpaPb.GnGAmt1 = clsPmpaPb.GnJinAmts[11];
                    clsPmpaPb.GnGAmt2 = clsPmpaPb.GnJinAmts[12];
                    if (string.Compare(BDate, "2018-09-01") >= 0) { clsPmpaPb.GnGAmt3 = clsPmpaPb.GnJinAmts[13]; }  
                    if (ArgJiwon == "OK")
                    {
                        clsPmpaPb.GnGAmt1 = 0;
                        clsPmpaPb.GnGAmt2 = 0;
                        clsPmpaPb.GnGAmt3 = 0;
                    }
                    clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 + clsPmpaPb.GnGAmt1 + clsPmpaPb.GnGAmt2 + clsPmpaPb.GnGAmt3;
                }
            }

            //소아가산 치과 장애인은 총액에서 500원 가산
            if (Dept.Equals("DT") && (nBi == 21 || nBi == 22) && Jangae.Equals("3"))
            {
                clsPmpaPb.gnJinAMT1 += 500; //나이불문 500원 가산( 장애자)
            }

            else if (clsPmpaPb.GnAge < 6 && JinSel != "8" && JinSel != "G" && JinSel != "C" && JinSel != "U" && JinSel != "T")
            {
                switch (ChoSel)
                {
                    case 1:
                        nPedAdd = clsPmpaPb.PedAdd1;       //6세미만 초진가산
                        break;
                    case 2:
                        nPedAdd = clsPmpaPb.PedAdd2;       //6세미만 초진심야가산
                        break;
                    case 3:
                        nPedAdd = clsPmpaPb.PedAdd3;       //6세미만 재진가산
                        break;
                    case 4:
                        nPedAdd = clsPmpaPb.PedAdd4;       //6세미만 재진심야가산
                        break;
                    case 9:
                        nPedAdd = clsPmpaPb.PedAdd5;       //6세미만 초진휴일가산
                        break;
                    case 10:
                        nPedAdd = clsPmpaPb.PedAdd6;       //6세미만 재진휴일가산
                        break;
                }
                clsPmpaPb.gnJinAMT1 += nPedAdd;
            }

            if (JinSel.Equals("2") || JinSel.Equals("3") || JinSel.Equals("4") || JinSel.Equals("B"))
            {
                clsPmpaPb.gnJinAMT1 = 0;
                clsPmpaPb.GnGAmt1 = 0;
                clsPmpaPb.GnGAmt2 = 0;
                clsPmpaPb.GnGAmt3 = 0;
            }

            if (clsPmpaPb.GstrSi22 == "OK") { clsPmpaPb.gnJinAMT1 = 0; } //시설환자(나자렛집) 진찰료 없음

            if (nBi > 20 && nBi < 30 && Dept.Equals("HD"))
            {
                clsPmpaPb.gnJinAMT1 = 0;
                clsPmpaPb.GnGAmt1 = 0;
                clsPmpaPb.GnGAmt2 = 0;
                clsPmpaPb.GnGAmt3 = 0;
            }
            #endregion

            #region 진찰료 특진금액 계산
            if (SpcSel == 1)
            {
                if (ChoSel < 3)
                {
                    clsPmpaPb.gnJinAMT2 = clsPmpaPb.GnJinAmts[5];
                }
                else
                {
                    clsPmpaPb.gnJinAMT2 = clsPmpaPb.GnJinAmts[6];
                }

                //의료급여는 발생안함
                if (nBi == 21 || nBi == 22)
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }

                //외과 하동엽과장 발생안함
                if (Dept.Equals("GS") && Doct.Equals("2119"))
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }

                //특정과는 발생안함
                if (Dept.Equals("PC") || Dept.Equals("NP") || Dept.Equals("RD") || Dept.Equals("MG"))
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }

                //특정감액 계정은 발생안함1
                if (string.Compare(GamSel, "11") >= 0 && string.Compare(GamSel, "14") <= 0)
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }

                //특정감액 계정은 발생안함2
                if (string.Compare(GamSel, "21") >= 0 && string.Compare(GamSel, "27") <= 0)
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }

                //특정감액 계정은 발생안함3
                if (string.Compare(GamSel, "30") >= 0 && string.Compare(GamSel, "34") <= 0)
                {
                    clsPmpaPb.gnJinAMT2 = 0;
                }
            }
            #endregion

            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2;

            #region 진찰료 조합부담

            clsPmpaPb.GstrJinBonFlag = "0";

            if (BiSel.Equals("22") && Jangae.Equals("3"))  //보호 장애인 진찰료 조합부담 -> 2013-12-03 급여2종 장애인만 조합100% 청구됨
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
            }
            else if ((Dept.Equals("HD") || JinSel.Equals("6")) && BiSel.Equals("22") && clsPmpaPb.GstrOtherHD != "*")
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;   //HD환자는 진찰료가 없음.=> 정액진료비임.(2008-07-17)
                clsPmpaPb.GstrJinBonFlag = "A";
            }
            else if (Dept.Equals("HD") || JinSel.Equals("6") || clsPmpaPb.GstrOtherHD == "*") //HD환자 일반과 접수
            {
                if (MCode.Equals("C000"))
                {
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                }
                else
                {
                    if (string.Compare(BiSel, "13") <= 0) //100단위작업 2009-07-01
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);  //HD,6접수 상병특례 희귀난치성,등록희귀난치질환 10% 2009-07-01시행 윤조연추가
                        clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (0.1)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (10 / 100)) / 100) * 100);
                        clsPmpaPb.GstrJinBonFlag = "4";                                                        //본인부담10%
                    }
                    else
                    {
                        if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsPmpaPb.IBON[nBi] / 100.0)) % 2) == 0)
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.IBON[nBi] / 100.0) / 10)) * 10);
                        }
                        else
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.IBON[nBi] / 100.0)) / 10 - 0.5) * 10);
                        }
                        clsPmpaPb.GstrJinBonFlag = "3";                                                        //본인부담20%
                    }
                }

                if (Jangae.Equals("3"))
                {
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;                                 //보호 장애인 진찰료 조합부담
                }
            }
            //접수구분 B는 사용하지 않는것으로 판단하여 제외함 2017-07-19 KMC
            //else if (JinSel.Equals("B"))
            //{
            //    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 / 100.0 / 100.0);
            //}
            else if (BiSel.Equals("22") && JinSel.Equals("I"))
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - 1500;
            }
            else if (BiSel.Equals("22") && JinSel.Equals("J"))
            {
                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - 1000;
            }
            else if (BiSel.Equals("22") && Dept.Equals("NP") && string.Compare(BDate, clsPmpaPb.GstrNPRateDay) >= 0)
            {
                if (clsPmpaPb.GstrSPR.Equals("OK"))
                {   //KYO 2017-03-13  NP 22종 본인부담 5%
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05)) / 100) * 100);
                }
                else
                {   //KYO 2017-03-13  NP 22종 본인부담 10%
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);
                }
            }
            else if (string.Compare(BiSel, "20") > 0)
            {
                if (string.Compare(BDate, clsPmpaPb.OBON_DATE) >= 0)
                {
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0)); //50%
                }
                else
                {
                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OLD_OBON[nBi] / 100.0)); //60%
                }
            }
            else
            {
                if (string.Compare(BDate, clsPmpaPb.OBON_DATE) >= 0)
                {
                    if (MCode.Equals("C000"))
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;      //차상위계층환자
                    }
                    else if (JinSel.Equals("F") || JinSel.Equals("G"))  //상병특례
                    {
                        if (string.Compare(BDate, "2009-07-01") >= 0 && (MCode.Equals("H000") || MCode.Equals("V000")) && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))    //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);  //상병특례 희귀난치성,등록희귀난치질환 10% 2009-07-01시행 윤조연추가
                            clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.1)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);
                        }
                        else
                        {
                            if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))   //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.2) / 100) * 100);   //20%
                            }
                            else
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.2));              //20%
                            }
                        }
                    }
                    else if (JinSel.Equals("R") || JinSel.Equals("U"))      //소아환자(만 6세미만) 35%
                    {
                        if (string.Compare(BDate, "2007-12-31") >= 0)
                        {
                            if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 100) * 100);
                                clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.35)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 100) * 100);
                            }
                            else
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 10 - 0.5) * 10);
                            }
                        }
                        else
                        {
                            if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.35)) % 2) == 0)
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35) / 10)) * 10);
                            }
                            else
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 10 - 0.5) * 10);
                            }
                        }
                    }
                    else if (JinSel.Equals("S") || JinSel.Equals("T"))       //소아환자(만 6세이하) 상병특례 14%
                    {
                        if (string.Compare(BDate, "2009-07-01") >= 0 && MCode.Equals("V000") && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))) //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (0.1)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);
                        }
                        else if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))) //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (14 / 100)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14)) / 100) * 100);
                        }
                        else
                        {
                            if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.14)) % 2) == 0)
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14) / 10)) * 10);
                            }
                            else
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14)) / 10 - 0.5) * 10);
                            }
                        }
                    }
                    //TODO : 환자나이 체크
                    else if (JinSel.Equals("C") && clsPmpaPb.GnAge <= 5)   //2008-04-28추가 했음.
                    {
                        if (MCode.Equals("V000") && string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))   //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.1)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.1)) / 100) * 100);
                        }
                        else if (MCode.Equals("H000"))
                        {
                            if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (0.14)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14)) / 100) * 100);
                            }
                            else
                            {
                                if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.14)) % 2) == 0)
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14) / 10)) * 10);
                                }
                                else
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.14)) / 10 - 0.5) * 10);
                                }
                            }
                        }
                        else
                        {
                            if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.35)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 100) * 100);
                            }
                            else
                            {
                                if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.35)) % 2) == 0)
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35) / 10)) * 10);
                                }
                                else
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.35)) / 10 - 0.5) * 10);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 100)) * 100);
                        }
                        else
                        {
                            if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0)) % 2) == 0)
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 10)) * 10);
                            }
                            else
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0)) / 10 - 0.5) * 10);
                            }
                        }

                        clsPmpaPb.GstrJinBonFlag = "1"; //본인부담 50%

                        //2014-06-27 급여제한자는 본인부담 100%
                        if (ArgJinDtl.Equals("09"))
                        {
                            clsPmpaPb.gnJinAMT4 = 0;
                            clsPmpaPb.GnJinDanAmt = 0;
                            clsPmpaPb.GstrJinBonFlag = "0";
                        }

                    }
                }
                else
                {
                    if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OLD_OBON[nBi] / 100.0) / 100)) * 100);
                        clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OLD_OBON[nBi] / 100.0)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OLD_OBON[nBi] / 100.0) / 100)) * 100);
                    }
                    else
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OLD_OBON[nBi] / 100.0)); //60%
                    }
                    clsPmpaPb.GstrJinBonFlag = "1"; //본인부담 60%
                }
            }

            if (clsPmpaPb.GstrCanCer != "" && clsPmpaPb.GstrCanCer != null )
            {
                //변수에 담아서 사용
                string strCanCer = clsPmpaPb.GstrCanCer;

                if (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))
                {
                    if (strCanCer.Equals("V191") || strCanCer.Equals("V192") || strCanCer.Equals("V193") || strCanCer.Equals("V194"))
                    {
                        if (BiSel.Equals("22") && Jangae.Equals("3"))  //자격이 22종 장애인 2006-07-24
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                        }
                        else
                        {
                            if (MCode != "C000")
                            {
                                if (string.Compare(BDate, "2009-12-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")) && (strCanCer.Equals("V191") || strCanCer.Equals("V192") || strCanCer.Equals("V193") || strCanCer.Equals("V194")))
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                                    clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (0.05)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                                }
                                else if (string.Compare(BDate, "2009-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13"))) //100단위작업 2009-07-01
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 100)) * 100);
                                    clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 100)) * 100);
                                }
                                else
                                {
                                    if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) % 2) == 0)
                                    {
                                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10)) * 10);
                                    }
                                    else
                                    {
                                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10 - 0.5)) * 10);
                                    }
                                }
                            }
                        }
                    }
                    else if (strCanCer.Equals("V247") || strCanCer.Equals("V248") || strCanCer.Equals("V249") || strCanCer.Equals("V250")) //2010-07-01 중증화상 5%
                    {
                        if (MCode != "C000")
                        {
                            if (string.Compare(BDate, "2010-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (0.05)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                            }
                            else
                            {
                                if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) % 2) == 0)
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10)) * 10);
                                }
                                else
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10 - 0.5)) * 10);
                                }
                            }
                        }
                    }
                    else if (MCode.Equals("F003"))
                    {
                        if (string.Compare(BDate, "2010-07-01") >= 0 && (BiSel.Equals("11") || BiSel.Equals("12") || BiSel.Equals("13")))  //100단위작업 2009-07-01
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 100)) * 100);
                            clsPmpaPb.GnJinDanAmt = (clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 100)) * 100);
                        }
                        else
                        {
                            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsPmpaPb.OBON[nBi] / 100.0) / 10)) * 10);
                        }
                    }
                    else if (MCode.Equals("V000"))
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                    }
                }
                else if (BiSel.Equals("22"))
                {
                    if (strCanCer.Equals("V247") || strCanCer.Equals("V248") || strCanCer.Equals("V249") || strCanCer.Equals("V250"))   //2010-07-01 중증화상 5%
                    {
                        if (MCode != "C000")
                        {
                            if (string.Compare(BDate, "2010-07-01") >= 0)
                            {
                                clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                                clsPmpaPb.GnJinDanAmt = Math.Truncate(clsPmpaPb.gnJinAMT1 * (5 / 100)) - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (0.05) / 100)) * 100);
                            }
                            else
                            {
                                if ((Math.Truncate(clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0)) % 2) == 0)
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10)) * 10);
                                }
                                else
                                {
                                    clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1 - (Math.Truncate((clsPmpaPb.gnJinAMT1 * (clsBAcc.Read_Cancer_BonRate(BiSel, BDate, strCanCer) / 100.0) / 10 - 0.5)) * 10);
                                }
                            }
                        }
                    }
                    else if (strCanCer.Equals("V000"))
                    {
                        clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT1;
                    }
                }
            }

            if (clsPmpaPb.GstatEROVER.Equals("*"))
            {
                clsPmpaPb.GstrJinBonFlag = "3";     //본인부담20%   //응급실 6시간이상,낮병 동
            }

            #endregion

            #region 진찰료 감액설정
            if (Dept.Equals("DT"))
            {
                if (string.Compare(GamSel, "00") > 0 && nBi < 25)
                {
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.DTJin_Rate / 100.0);
                }
                else if (GamSel.Equals("55") && nBi == 33)
                {
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.0);
                }
                else if (GamSel.Equals("55") && nBi == 51)
                {
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.0);
                }
                else if (GamSel.Equals("55") && nBi == 41)
                {
                    clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Amt50_Rate / 100.0);
                }
            }
            else
            {
                if (MCode.Equals("H000"))
                {
                    if (JinSel != "F" && JinSel != "G" && JinSel != "S" && JinSel != "T")  //F:상병특례 G:물리치료상병특례 S:소아상병특례 T:소아물리치료상병
                    {
                        if (JinSel.Equals("K") || JinSel.Equals("L") || JinSel.Equals("M")) //K.RM협진, L.UR협진, M.PET-CT 접수비
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (100 / 100.0);
                        }
                        else if ((GamSel.Equals("55") || GamSel.Equals("51")) && nBi == 33)
                        {
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.0));
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 / 10);
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 * 10);
                        }
                        else if ((GamSel.Equals("55") || GamSel.Equals("51")) && nBi == 51)
                        {
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.0));
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 / 10);
                            clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 * 10);
                        }
                        else if (string.Compare(GamSel, "00") > 0 && nBi != 52)
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Jin_Rate / 100.0);
                        }
                    }
                }
                else
                {
                    if (JinSel.Equals("K") || JinSel.Equals("L") || JinSel.Equals("M")) //K.RM협진, L.UR협진, M.PET-CT 접수비
                    {
                        clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (100 / 100.0);
                    }
                    else if ((GamSel.Equals("55") || GamSel.Equals("51")) && nBi == 33)
                    {
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.0));
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 / 10);
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 * 10);
                    }
                    else if ((GamSel.Equals("55") || GamSel.Equals("51")) && nBi == 51)
                    {
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.0));
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 / 10);
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 * 10);
                    }
                    else if ((GamSel.Equals("55") || GamSel.Equals("51")) && nBi == 41)
                    {
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT3 * (clsPmpaType.GAM.Amt50_Rate / 100.0));
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 / 10);
                        clsPmpaPb.gnJinAMT5 = Math.Truncate(clsPmpaPb.gnJinAMT5 * 10);
                    }
                    else if (string.Compare(GamSel, "00") > 0 && nBi != 52)
                    {
                        if (clsPmpaPb.gnJinAMT2 > 0)
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT1 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Jin_Rate / 100.0);
                        }
                        else
                        {
                            clsPmpaPb.gnJinAMT5 = (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) * (clsPmpaType.GAM.Jin_Rate / 100.0);
                        }

                    }
                }
            }
            #endregion

            #region 진찰료 계산
            if ((JinSel.Equals("1") || JinSel.Equals("5") || Jangae.Equals("1") || Jangae.Equals("2") || GyeJin == 1) && MCode != "H000" && JinSel != "E")
            {
                clsPmpaPb.gnJinAMT6 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4 - clsPmpaPb.gnJinAMT5;
                clsPmpaPb.gnJinAMT6 = clsPmpaPb.gnJinAMT6 / 10;
                clsPmpaPb.gnJinAMT6 = clsPmpaPb.gnJinAMT6 * 10;
            }
            else
            {
                if (clsPmpaPb.GstrSelUse == "OK" && nBi == 52 && SpcSel == 1)
                {
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4 - clsPmpaPb.gnJinAMT5 - clsPmpaPb.gnJinAMT2;
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT7 / 10;
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT7 * 10;
                }
                else
                {
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4 - clsPmpaPb.gnJinAMT5;
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT7 / 10;
                    clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT7 * 10;
                }
            }

            if (clsPmpaPb.gnJinAMT7 < 0)
            {
                clsPmpaPb.gnJinAMT7 = 0;
            }

            if (nBi == 21 || (nBi == 22 && JinSel.Equals("Q")) || (nBi == 22 && MCode.Equals("B099")))
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
            }

            //2009-04-01 차상위2종은 접수비 없이 처리하고 수납시 처방코드에 따라 접수코드적용.
            if ((nBi >= 11 && nBi <= 13) && (MCode.Equals("E000") || MCode.Equals("F000") || (MCode.Equals("V000") && (clsPmpaPb.GstrCanCer.Equals("V206") || clsPmpaPb.GstrCanCer.Equals("V231") || clsPmpaPb.GstrCanCer.Equals("V246")))))
            {
                if (ArgJinDtl != "12")
                {
                    clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                    clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                    clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                    clsPmpaPb.GnJinDanAmt = 0;
                }
            }

            if (nBi < 30 && ArgJinDtl.Equals("22"))
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }

            if (nBi < 30 && ArgJinDtl.Equals("23") && string.Compare(BDate, "2017-05-26") >= 0)
            {
                clsPmpaPb.gnJinAMT1 = 0; clsPmpaPb.gnJinAMT2 = 0; clsPmpaPb.gnJinAMT3 = 0;
                clsPmpaPb.gnJinAMT4 = 0; clsPmpaPb.gnJinAMT5 = 0; clsPmpaPb.gnJinAMT6 = 0; clsPmpaPb.gnJinAMT7 = 0;
                clsPmpaPb.GnGAmt1 = 0; clsPmpaPb.GnGAmt2 = 0; clsPmpaPb.GnGAmt3 = 0;
                clsPmpaPb.GnJinDanAmt = 0;
            }
            #endregion
        }

        //OPD_MASTER JIN 컬럼 접수비 없음 후불구분 대상
        private bool Rtn_NoAmt_Jin_Gubun(string strJin)
        {
            bool rtnVal = false;

            switch (strJin)
            {
                case "2"://접수||
                    rtnVal = true;
                    break;
                case "8"://물리치료
                    rtnVal = true;
                    break;
                case "9"://가정간호
                    rtnVal = true;
                    break;
                case "A"://희귀가정간호
                    rtnVal = true;
                    break;
                case "D"://일반건진
                    rtnVal = true;
                    break;
                case "G"://물리상병특례
                    rtnVal = true;
                    break;
                case "L"://결핵쿠폰
                    rtnVal = true;
                    break;
                case "N"://포스코접수
                    rtnVal = true;
                    break;
                case "T"://소아물리상병특례
                    rtnVal = true;
                    break;
                case "U"://소아물리상병특례(만6세미만)
                    rtnVal = true;
                    break;
                case "V"://수탁검사
                    rtnVal = true;
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }

        //OPD_MASTER JINDTL 컬럼 접수비 없음 후불구분 대상
        private bool Rtn_NoAmt_JinDtl_Gubun(string strJinDtl)
        {
            bool rtnVal = false;

            switch (strJinDtl)
            {
                case "02"://완전틀니대상(만65세이상)
                    rtnVal = true;
                    break;
                case "07"://치과임플란트(만65세이상)
                    rtnVal = true;
                    break;
                case "10"://동일상병 타과진료
                    rtnVal = true;
                    break;
                case "11"://언어물리치료
                    rtnVal = true;
                    break;
                case "12"://금연치료
                    rtnVal = true;
                    break;
                case "14"://진료의사본과접수
                    rtnVal = true;
                    break;
                case "15"://해바라기센터(가정폭력)
                    rtnVal = true;
                    break;
                case "16"://해바라기센터(성폭력)
                    rtnVal = true;
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }


        /// <summary>
        /// Description : 등록번호로 환자기본정보 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.17
        /// <param name="ArgPtno">등록번호</param>
        /// </summary>
        /// <seealso cref="OUMSAD.BAS:READ_BAS_PATIENT"/>
        public string READ_BAS_PATIENT(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnval = string.Empty;

            rtnval = "NO";

            clsPmpaType cPT = new clsPmpaType();
            cPT.Clear_Type_Bas_Patient();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtPat, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnval;
                }

                if (DtPat.Rows.Count > 0)
                {
                    rtnval = "OK";

                    cPT.Set_Type_Bas_Patient(DtPat);
                }

                DtPat.Dispose();
                DtPat = null;

                return rtnval;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnval;
            }
        }



        /// <summary>
        /// Description : 외래접수마스터
        /// Author : 박병규
        /// Create Date : 2017.09.11
        /// <param name="ArgPtno"></param>
        /// <param name="ArgActDate"></param>
        /// <param name="ArgBdate"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:Read_OPD_MASTER"/>
        public string READ_OPD_MASTER(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgActDate, string ArgBdate)
        {
            DataTable DtSad = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            string strBi = string.Empty;
            string strBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strVCode = string.Empty;
            string strFCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strDept = string.Empty;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsPmpaType cPT = new clsPmpaType();

            cPT.Clear_Type_Opd_Master();

            rtnVal = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OpdNo, Pano, DeptCode, Bi,                        --등록번호,진료과목,환자구분";
            SQL += ComNum.VBLF + "        Sname, Sex, Age,                                  --수진자명,성별,나이";
            SQL += ComNum.VBLF + "        JiCode, DrCode, Reserved,                         --지역코드,의사코드,예약구분(0.당일접수 1.예약접수)";
            SQL += ComNum.VBLF + "        Chojae, GbGamek, GbGamekC,                        --초재구분,감액구분(자격),감액구분(CASE)";
            SQL += ComNum.VBLF + "        GbSpc, Jin, SinGu,                                --선택진료,진찰료수납여부,신환여부";
            SQL += ComNum.VBLF + "        Bohun, Change, Sheet,                             --보훈대상여부,보험정산대상자여부,진료사실통보서여부";
            SQL += ComNum.VBLF + "        Rep, Part,                                        --영수증발행여부(+.발행 -.환불 #.입원환불)";
            SQL += ComNum.VBLF + "        TO_CHAR(Jtime,'YYYY-MM-DD') Jtime,                --접수일자";
            SQL += ComNum.VBLF + "        TO_CHAR(Jtime,'YYYY-MM-DD HH24:MI') Jtime2,       --접수시간";
            SQL += ComNum.VBLF + "        TO_CHAR(OUTTIME,'YYYY-MM-DD HH24:MI') OUTTIME,    --응급실퇴실시간";
            SQL += ComNum.VBLF + "        TO_CHAR(Stime,'YYYY-MM-DD') Stime,                --수납일자";
            SQL += ComNum.VBLF + "        Fee1, Fee2, Fee3,                                 --근육주사수수료,정맥주사수수료,내복조제수수료";
            SQL += ComNum.VBLF + "        Fee31, Fee5, Fee51,                               --내복제제수수료,외용조재수수료,외용제제수수료";
            SQL += ComNum.VBLF + "        Fee7, Amt1, Amt2,                                 --수혈수기료,진찰료발생금액,진찰료특진료";
            SQL += ComNum.VBLF + "        Amt3, Amt4, Amt5,                                 --진찰료총액,진찰료조합부담,진찰료감액";
            SQL += ComNum.VBLF + "        Amt6, Amt7, GelCode,                              --진찰료미수,진찰료영수금액,계약처코드";
            SQL += ComNum.VBLF + "        TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,            --회계일자";
            SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate,                --진료일자";
            SQL += ComNum.VBLF + "        Bunup, OCSJIN, ROWID,                             --의약분업예외환자구분,외래OCS상태(*.진료 #.응급실퇴실 기타.대기중)";
            SQL += ComNum.VBLF + "        MksJin, GbNight, GbFlu_Vac,                       --접수구분2,소아야간,플루예방접종여부";
            SQL += ComNum.VBLF + "        OldMan, GbDementia, Gbilban2,                     --어르신먼저진료,치매검사여부,외국인일반수가2배";
            SQL += ComNum.VBLF + "        GbFlu_Ltd, VCODE, KTASLEVL,                       --사업장독감접종구분,VCODE,K-TAS";
            SQL += ComNum.VBLF + "        CARDSEQNO, CARDGB, MCODE,                         --신용카드승인번호,카드구분,MCODE";
            SQL += ComNum.VBLF + "        MSEQNO, MQCODE, Jiwon,                            --의료급여진료확인번호,MQCODE,의료질평가지원금여부";
            SQL += ComNum.VBLF + "        AMT8, JinDtl, INSULIN,                            --희귀난치성질환자지원금,접수구분,인슐린투여여부";
            SQL += ComNum.VBLF + "        DrSabun, J2_Sayu ,nvl(gbres,'0') gbres ,nvl(GBPOWDER,'N') GBPOWDER,YHOSP_KIHO,            --의사사번,접수2사유,후불예약 ,파우더가산";
            SQL += ComNum.VBLF + "        GBFLU ";      // -- 접수 시 사전 오더(NCOV-PA)
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";

            if (ArgBdate != "")
                SQL += ComNum.VBLF + "   AND BDate  = TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD') ";

            if (ArgActDate != "")
                SQL += ComNum.VBLF + "   AND ActDate <= TO_DATE('" + ArgActDate + "', 'YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtSad.Rows.Count > 0)
            {
                rtnVal = "OK";

                clsPmpaType.TOM.OpdNo = Convert.ToInt64(VB.Val(DtSad.Rows[0]["OpdNo"].ToString().Trim()));
                clsPmpaType.TOM.Pano = DtSad.Rows[0]["PANO"].ToString().Trim();
                clsPmpaType.TOM.DeptCode = DtSad.Rows[0]["DEPTCODE"].ToString().Trim();
                clsPmpaType.TOM.Bi = DtSad.Rows[0]["BI"].ToString().Trim();
                clsPmpaType.TOM.sName = DtSad.Rows[0]["SNAME"].ToString().Trim();
                clsPmpaType.TOM.Sex = DtSad.Rows[0]["SEX"].ToString().Trim();
                clsPmpaType.TOM.Age = Convert.ToInt32(VB.Val(DtSad.Rows[0]["AGE"].ToString().Trim()));
                clsPmpaType.TOM.Jiyuk = DtSad.Rows[0]["JICODE"].ToString().Trim();
                clsPmpaType.TOM.DrCode = DtSad.Rows[0]["DRCODE"].ToString().Trim();
                clsPmpaType.TOM.Reserved = DtSad.Rows[0]["RESERVED"].ToString().Trim();
                clsPmpaType.TOM.Chojae = DtSad.Rows[0]["CHOJAE"].ToString().Trim();
                clsPmpaType.TOM.GbGameK = DtSad.Rows[0]["GBGAMEK"].ToString().Trim();
                clsPmpaType.TOM.GbGameKC = DtSad.Rows[0]["GBGAMEKC"].ToString().Trim();
                clsPmpaType.TOM.GbSpc = DtSad.Rows[0]["GBSPC"].ToString().Trim();
                clsPmpaType.TOM.Jin = DtSad.Rows[0]["JIN"].ToString().Trim();
                clsPmpaType.TOM.Sinwhan = DtSad.Rows[0]["SINGU"].ToString().Trim();
                clsPmpaType.TOM.Bohun = DtSad.Rows[0]["BOHUN"].ToString().Trim();
                clsPmpaType.TOM.Change = DtSad.Rows[0]["CHANGE"].ToString().Trim();
                clsPmpaType.TOM.Sheet = DtSad.Rows[0]["SHEET"].ToString().Trim();
                clsPmpaType.TOM.Rep = DtSad.Rows[0]["REP"].ToString().Trim();
                clsPmpaType.TOM.Part = DtSad.Rows[0]["PART"].ToString().Trim();
                clsPmpaType.TOM.JTime = DtSad.Rows[0]["JTIME"].ToString().Trim();
                clsPmpaType.TOM.Stime = DtSad.Rows[0]["STIME"].ToString().Trim();
                clsPmpaType.TOM.Fee1 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE1"].ToString().Trim()));
                clsPmpaType.TOM.Fee2 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE2"].ToString().Trim()));
                clsPmpaType.TOM.Fee3 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE3"].ToString().Trim()));
                clsPmpaType.TOM.Fee31 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE31"].ToString().Trim()));
                clsPmpaType.TOM.Fee5 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE5"].ToString().Trim()));
                clsPmpaType.TOM.Fee51 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE51"].ToString().Trim()));
                clsPmpaType.TOM.Fee7 = Convert.ToInt32(VB.Val(DtSad.Rows[0]["FEE7"].ToString().Trim()));
                clsPmpaType.TOM.Amt1 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT1"].ToString().Trim()));
                clsPmpaType.TOM.Amt2 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT2"].ToString().Trim()));
                clsPmpaType.TOM.Amt3 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT3"].ToString().Trim()));
                clsPmpaType.TOM.Amt4 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT4"].ToString().Trim()));
                clsPmpaType.TOM.Amt5 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT5"].ToString().Trim()));
                clsPmpaType.TOM.Amt6 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT6"].ToString().Trim()));
                clsPmpaType.TOM.Amt7 = Convert.ToInt64(VB.Val(DtSad.Rows[0]["AMT7"].ToString().Trim()));
                clsPmpaType.TOM.GelCode = DtSad.Rows[0]["GELCODE"].ToString().Trim();
                clsPmpaType.TOM.BDate = DtSad.Rows[0]["BDATE"].ToString().Trim();
                clsPmpaType.TOM.ActDate = DtSad.Rows[0]["ACTDATE"].ToString().Trim();
                clsPmpaType.TOM.WardCode = "";
                clsPmpaType.TOM.RoomCode = 0;
                clsPmpaType.TOM.Bunup = DtSad.Rows[0]["BUNUP"].ToString().Trim();
                clsPmpaType.TOM.OCSJIN = DtSad.Rows[0]["OCSJIN"].ToString().Trim();
                clsPmpaType.TOM.ROWID = DtSad.Rows[0]["ROWID"].ToString().Trim();
                clsPmpaType.TOM.CardSeqNo = Convert.ToInt64(VB.Val(DtSad.Rows[0]["CARDSEQNO"].ToString().Trim()));
                clsPmpaType.TOM.VCode = DtSad.Rows[0]["VCODE"].ToString().Trim();
                clsPmpaType.TOM.MCode = DtSad.Rows[0]["MCODE"].ToString().Trim();
                clsPmpaType.TOM.MSeqNo = DtSad.Rows[0]["MSEQNO"].ToString().Trim();
                clsPmpaType.TOM.MQCODE = DtSad.Rows[0]["MQCODE"].ToString().Trim();
                clsPmpaType.TOM.MksJin = DtSad.Rows[0]["MKSJIN"].ToString().Trim();
                clsPmpaType.TOM.GbNight = DtSad.Rows[0]["GBNIGHT"].ToString().Trim();
                clsPmpaType.TOM.GbFlu_Vac = DtSad.Rows[0]["GBFLU_VAC"].ToString().Trim();
                clsPmpaType.TOM.OldMan = DtSad.Rows[0]["OLDMAN"].ToString().Trim();
                clsPmpaType.TOM.GbExam = "";
                clsPmpaType.TOM.GbDementia = DtSad.Rows[0]["GBDEMENTIA"].ToString().Trim();
                clsPmpaType.TOM.Jtime2 = DtSad.Rows[0]["JTIME2"].ToString().Trim();
                clsPmpaType.TOM.Gbilban2 = DtSad.Rows[0]["GBILBAN2"].ToString().Trim();
                clsPmpaType.TOM.GbFlu_Ltd = DtSad.Rows[0]["GBFLU_LTD"].ToString().Trim();
                clsPmpaType.TOM.JinDtl = DtSad.Rows[0]["JINDTL"].ToString().Trim();
                clsPmpaType.TOM.INSULIN = DtSad.Rows[0]["INSULIN"].ToString().Trim();
                clsPmpaType.TOM.OUTTIME = DtSad.Rows[0]["OUTTIME"].ToString().Trim();
                clsPmpaType.TOM.DrSabun = Convert.ToInt64(VB.Val(DtSad.Rows[0]["DRSABUN"].ToString().Trim()));
                clsPmpaType.TOM.J2_Sayu = DtSad.Rows[0]["J2_SAYU"].ToString().Trim();
                clsPmpaType.TOM.Jiwon = DtSad.Rows[0]["JIWON"].ToString().Trim();
                clsPmpaType.TOM.KTASLEVL = DtSad.Rows[0]["KTASLEVL"].ToString().Trim();
                clsPmpaType.TOM.GBRES = DtSad.Rows[0]["GBRES"].ToString().Trim();
                clsPmpaType.TOM.GBPOWDER = DtSad.Rows[0]["GBPOWDER"].ToString().Trim();
                clsPmpaType.TOM.YHOSP_KIHO = DtSad.Rows[0]["YHOSP_KIHO"].ToString().Trim();

                clsPmpaType.TOM.GbFlu = DtSad.Rows[0]["GBFLU"].ToString().Trim();   //2021-09-16

                //2018.05.30 박병규 : 주석요청에 의한 처리
                //if (clsPmpaPb.GstrFrom == "접수")
                //{
                //    if (clsPmpaType.TOM.CardSeqNo != 0)
                //    {
                //        if (DtSad.Rows[0]["CARDGB"].ToString().Trim() == "1")
                //        {
                //            clsPublic.GstrMsgTitle = "확인";
                //            clsPublic.GstrMsgList = "승인구분 : 카드승인    승인금액 : " + clsPmpaType.TOM.CardSeqNo + "원 승인" + '\r';
                //            clsPublic.GstrMsgList += "꼭 확인하시고 작업하시기 바랍니다." + '\r';
                //            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                //        }
                //        else if ((DtSad.Rows[0]["CARDGB"].ToString().Trim() == "2"))
                //        {
                //            clsPublic.GstrMsgTitle = "확인";
                //            clsPublic.GstrMsgList = "승인구분 : 현금영수증    승인금액 : " + clsPmpaType.TOM.CardSeqNo + "원 승인" + '\r';
                //            clsPublic.GstrMsgList += "꼭 확인하시고 작업하시기 바랍니다." + '\r';
                //            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                //        }
                //    }
                //}
            }

            if (clsPmpaType.TOM.Jin == "6" && clsPmpaType.TOM.VCode == "F003")
                ComFunc.MsgBox("인공신장 환자는 의약분업(F003)이 있으면 안됨. 전산팀으로 전화하시기 바랍니다.", "확인");


            DtSad.Dispose();
            DtSad = null;

            if (rtnVal == "OK")
            {
                clsAlert cA = new ComPmpaLibB.clsAlert();
                clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();

                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI.Trim(), 1) == "1") { cBON.BI = "11"; }

                //기준일자 세팅
                cBON.SDATE = clsPmpaType.TOM.BDate;
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
                //나이구분 체크
                

                cBON.MCODE = clsPmpaType.TOM.MCode;
                cBON.VCODE = clsPmpaType.TOM.VCode;

                cBON.OGPDBUN = "";

                cBON.FCODE = "";
                if (clsPmpaType.TOM.JinDtl == "22")
                    cBON.FCODE = "03";
                else if (clsPmpaType.TOM.JinDtl == "25")
                    cBON.FCODE = "02";

                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if (VB.Left(cBON.BI, 1) == "1" && cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" && clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }

                cBON.IO = "I";
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, strBDate, cBON.IO);
                //***입원 본인부담율 세팅
                if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
                {
                    if (clsPmpaType.TOM.DeptCode == "ER") { cA.Alert_BonRate(cBON); }
                }
                  

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.IBR.Jin = 0;
                        clsPmpaType.IBR.Bohum = 0;
                        clsPmpaType.IBR.CTMRI = 0;
                    }
                }

                //2018.05.31 박병규 : 입원본인부담율 구하면서 cBON 변수값을 치환시키므로 외래본인부담율 구할때 다시 조건을 설정해준다
                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI.Trim(), 1) == "1") { cBON.BI = "11"; }

                //기준일자 세팅
                cBON.SDATE = clsPmpaType.TOM.BDate;
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
                //나이구분 체크
                

                cBON.MCODE = clsPmpaType.TOM.MCode;
                cBON.VCODE = clsPmpaType.TOM.VCode;

                cBON.OGPDBUN = "";

                cBON.FCODE = "";
                if (clsPmpaType.TOM.JinDtl == "22")
                    cBON.FCODE = "03";
                else if (clsPmpaType.TOM.JinDtl == "25")
                    cBON.FCODE = "02";

                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if ( cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" && clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }

                cBON.IO = "O";
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, strBDate, cBON.IO);

                cBON.JINDTL = clsPmpaType.TOM.JinDtl;

                //***외래 본인부담율 세팅
                if (Read_OBon_Rate(pDbCon, cBON) == false)
                    cA.Alert_BonRate(cBON);

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.OBR.Jin = 0;
                        clsPmpaType.OBR.Bohum = 0;
                        clsPmpaType.OBR.CTMRI = 0;
                    }
                }

            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 당일 인공신장 접수 확인
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// <seealso cref="OUMSAD.bas : READ_OPD_SLIP_HD"/>
        /// </summary>
        public string READ_OPD_SLIP_HD(PsmhDb pDbCon, string ArgPtno, string ArgBdate)
        {
            DataTable DtSad = null;
            string SQL = "";
            string SqlErr = "";
            string strMcode = "";
            string strDeptList = "";
            string strGwa = "";

            clsPmpaPb.GstrOtherHD = ""; //동일 타과 인공신장체크
            clsPmpaPb.GstrOtherHD_MCode = ""; //동일과 HD자격

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptCode, MCode, BI ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            //SQL += ComNum.VBLF + "    AND BI IN ('11','12','13') "; //건강보험자격만
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY Jtime ";
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("당일 인공신장 접수확인 오류발생");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return strDeptList;
            }

            if (DtSad.Rows.Count > 0)
            {
                for (int i = 0; i < DtSad.Rows.Count; i++)
                {
                    strGwa = DtSad.Rows[i]["DeptCode"].ToString().Trim();
                    strMcode = DtSad.Rows[i]["MCode"].ToString().Trim();
                    strDeptList += "  " + strGwa + "  ";
                    
                    if (DtSad.Rows[i]["BI"].ToString().Trim() == "11" || DtSad.Rows[i]["BI"].ToString().Trim() == "12" || DtSad.Rows[i]["BI"].ToString().Trim() == "13")
                    {
                        if (strGwa == "HD")
                            clsPmpaPb.GstrOtherHD = "*";

                        if (clsPmpaPb.GstrOtherHD == "*" && (strMcode == "H000" || strMcode == "V000"))
                            clsPmpaPb.GstrOtherHD_MCode = strMcode;
                    }
                }
            }

            DtSad.Dispose();
            DtSad = null;

            return strDeptList;
        }

        public string Country_Check(PsmhDb pDbCon, string ArgCountry)
        {
            DataTable DtSad = null;
            string SQL = "";
            string SqlErr = "";
            string strCountry = "";
          
         

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.CODE,A.NAME ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE a ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 and gubun ='BAS_외국인환자국적' ";
            SQL += ComNum.VBLF + "    AND  NAME LIKE '%" + ArgCountry + "%' ";
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);

            if (SqlErr != "")
            {
             
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return strCountry;
            }

            if (DtSad.Rows.Count > 0)
            {
                for (int i = 0; i < DtSad.Rows.Count; i++)
                {
                    strCountry = DtSad.Rows[i]["CODE"].ToString().Trim() + "." +DtSad.Rows[i]["NAME"].ToString().Trim();
                  
                }
            }

            DtSad.Dispose();
            DtSad = null;

            return strCountry;
        }


        /// <summary>
        /// Description : 환자보험정보 입력
        /// author : 박병규
        /// Create Date : 2017-10-26
        /// <param name="str"></param>
        /// <seealso cref="OUMSAD.bas : Update_Bas_Mih"/>
        /// </summary>
        public bool UPDATE_BAS_MIH(PsmhDb pDbCon)
        {
            DataTable DtSad = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = true;
            string strMihKiho = "";
            string strMihGKiho = "";
            string strMihGwange = "";
            string strMihPname = "";
            string strMihRowid = "";

            if (clsPmpaPb.strDataFlag != "OK") { return rtnVal; }

            //등록번호,종류,일자 같은것
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(TransDate,'yyyy-mm-dd') TransDate,Rowid  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIH ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + clsPmpaType.TMI.Ptno + "' ";
            SQL += ComNum.VBLF + "    AND Bi        = '" + clsPmpaType.TMI.Bi + "' ";
            SQL += ComNum.VBLF + "    AND TransDate = TO_DATE('" + clsPmpaType.TMI.TransDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다" + ComNum.VBLF + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtSad.Dispose();
                DtSad = null;
                return rtnVal;
            }

            if (DtSad.Rows.Count == 1)         //같은것이 있으면 Update
            {
                UPDATE_BAS_MIH_SUB(pDbCon, DtSad.Rows[0]["ROWID"].ToString());

                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            DtSad.Dispose();
            DtSad = null;

            //등록번호,종류가 같은것 
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Kiho, GKiho, Gwange, ";
            SQL += ComNum.VBLF + "        TO_CHAR(TransDate,'YYYY-MM-DD') TransDate, Pname, RowId  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIH  ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Pano  = '" + clsPmpaType.TMI.Ptno + "'   ";
            SQL += ComNum.VBLF + "    AND Bi    = '" + clsPmpaType.TMI.Bi + "'   ";
            SQL += ComNum.VBLF + "  ORDER BY TransDate Desc   ";
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다" + ComNum.VBLF + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtSad.Dispose();
                DtSad = null;
                return rtnVal;
            }

            if (DtSad.Rows.Count == 0)         //같은것이 있으면 Update
            {
                INSERT_BAS_MIH_SUB(pDbCon);

                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            //보험100%,일반,계약처,미확인은 최종분의 내역만 관리
            if (clsPmpaType.TMI.Bi == "41" || clsPmpaType.TMI.Bi == "42" || clsPmpaType.TMI.Bi == "43" || clsPmpaType.TMI.Bi == "51" || clsPmpaType.TMI.Bi == "53" || clsPmpaType.TMI.Bi == "54" || clsPmpaType.TMI.Bi == "55")
            {
                UPDATE_BAS_MIH_SUB(pDbCon, DtSad.Rows[0]["ROWID"].ToString());
                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            //동일한 내역의 보험사항이 있는지 Check
            if (DtSad.Rows.Count > 0)
            {
                strMihKiho = DtSad.Rows[0]["KIHO"].ToString().Trim();
                strMihGKiho = DtSad.Rows[0]["GKiho"].ToString().Trim();
                strMihGwange = DtSad.Rows[0]["Gwange"].ToString().Trim();
                strMihPname = DtSad.Rows[0]["Pname"].ToString().Trim();
                strMihRowid = DtSad.Rows[0]["RowId"].ToString().Trim();
            }

            if (clsPmpaType.TMI.Kiho != strMihKiho)
            {
                INSERT_BAS_MIH_SUB(pDbCon);
                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            if (clsPmpaType.TMI.Bi == "31" || clsPmpaType.TMI.Bi == "52")
            {
                UPDATE_BAS_MIH_SUB(pDbCon, DtSad.Rows[0]["ROWID"].ToString());
                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            if (clsPmpaType.TMI.GKiho != strMihGKiho)
            {
                INSERT_BAS_MIH_SUB(pDbCon);
                DtSad.Dispose();
                DtSad = null;

                return rtnVal;
            }

            if (strMihRowid != "")
                UPDATE_BAS_MIH_SUB(pDbCon, strMihRowid);

            DtSad.Dispose();
            DtSad = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 
        /// author : 박병규
        /// Create Date : 2017-10-30
        /// <param name="str"></param>
        /// <seealso cref="OUMSAD.bas : Update_Bas_Mih"/>
        /// </summary>
        public bool UPDATE_BAS_MIH_SUB(PsmhDb pDbCon, string ArgRowid)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " UPDate " + ComNum.DB_PMPA + "BAS_MIH ";

            if (clsPmpaType.TMI.PName.Length > 5)
                SQL += ComNum.VBLF + "   SET Pname  = '" + VB.Left(clsPmpaType.TMI.PName.Trim(), 5) + "', ";
            else
                SQL += ComNum.VBLF + "   SET Pname  = '" + clsPmpaType.TMI.PName + "', ";

            SQL += ComNum.VBLF + "       Gwange = '" + clsPmpaType.TMI.Gwange + "', ";
            SQL += ComNum.VBLF + "       GKiho  = '" + clsPmpaType.TMI.GKiho + "', ";
            SQL += ComNum.VBLF + "       Kiho   = '" + clsPmpaType.TMI.Kiho + "' ";
            SQL += ComNum.VBLF + " WHERE Rowid  = '" + ArgRowid + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("UPDATE_BAS_MIH 오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// author : 박병규
        /// Create Date : 2017-10-30
        /// <param name="str"></param>
        /// <seealso cref="OUMSAD.bas : Update_Bas_Mih"/>
        /// </summary>
        public bool INSERT_BAS_MIH_SUB(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = true;

            if (clsPmpaType.TMI.Bi.Trim() == "")
            {
                ComFunc.MsgBox("보험유형이 공란임", "알림");
                rtnVal = false;
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_MIH ";
            SQL += ComNum.VBLF + "        (Pano, Bi, TransDate, ";
            SQL += ComNum.VBLF + "         Pname, Gwange, Kiho, ";
            SQL += ComNum.VBLF + "         Gkiho) ";
            SQL += ComNum.VBLF + " VALUES('" + clsPmpaType.TMI.Ptno + "', ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMI.Bi + "', ";
            SQL += ComNum.VBLF + "        TO_DATE('" + clsPmpaType.TMI.TransDate + "', 'YYYY-MM-DD'), ";

            if (clsPmpaType.TMI.PName.Length > 5)
                SQL += ComNum.VBLF + "        '" + VB.Left(clsPmpaType.TMI.PName.Trim(), 5) + "', ";
            else
                SQL += ComNum.VBLF + "        '" + clsPmpaType.TMI.PName + "', ";

            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMI.Gwange + "', ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMI.Kiho + "', ";
            SQL += ComNum.VBLF + "        '" + clsPmpaType.TMI.GKiho + "' )";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("INSERT_BAS_MIH 오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 재원자 MASTER READ
        /// Author : 박병규
        /// Create Date : 2017.12.12
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="Oumsad.BAS:READ_IPD_MASTER"/>
        public string READ_IPD_MASTER(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            string strBi = string.Empty;
            string strBDate = string.Empty;
            string strJuminNo = string.Empty;
            string strChild = string.Empty;
            string strMCode = string.Empty;
            string strVCode = string.Empty;
            string strFCode = string.Empty;
            string strOgPdBun = string.Empty;
            string strDept = string.Empty;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsPmpaType cPT = new clsPmpaType();

            cPT.Clear_Type_Opd_Master();

            rtnVal = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND (ACTDate IS NULL OR ACTDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = "OK";

                clsPmpaType.TOM.Pano = ArgPtno;
                clsPmpaType.TOM.BDate = ArgDate;
                clsPmpaType.TOM.DeptCode = DtFunc.Rows[0]["DEPTCODE"].ToString().Trim();
                clsPmpaPb.GstrllDept = clsPmpaType.TOM.DeptCode;
                clsPmpaType.TOM.sName = DtFunc.Rows[0]["SNAME"].ToString().Trim();
                clsPmpaType.TOM.Sex = DtFunc.Rows[0]["SEX"].ToString().Trim();
                clsPmpaType.TOM.Age = Convert.ToInt32(DtFunc.Rows[0]["AGE"].ToString().Trim());
                clsPmpaType.TOM.DrCode = DtFunc.Rows[0]["DRCODE"].ToString().Trim();
                clsPmpaType.TOM.Bi = DtFunc.Rows[0]["BI"].ToString().Trim();
                clsPmpaType.TOM.GbGameK = DtFunc.Rows[0]["GBGAMEK"].ToString().Trim();
                clsPmpaType.TOM.GbGameKC = DtFunc.Rows[0]["GBGAMEKC"].ToString().Trim();
                clsPmpaType.TOM.GelCode = DtFunc.Rows[0]["GelCode"].ToString().Trim();
                clsPmpaType.TOM.WardCode = DtFunc.Rows[0]["WardCode"].ToString().Trim();
                clsPmpaType.TOM.RoomCode = Convert.ToInt32(DtFunc.Rows[0]["RoomCode"].ToString().Trim());
                clsPmpaType.TOM.GbExam = DtFunc.Rows[0]["GbExam"].ToString().Trim();
                clsPmpaType.TOM.Jiyuk = "01";
                clsPmpaType.TOM.Reserved = "0";
                clsPmpaType.TOM.Chojae = "3";
                clsPmpaType.TOM.GbSpc = "0";
                clsPmpaType.TOM.Jin = "0";
                clsPmpaType.TOM.Sinwhan = "0";
                clsPmpaType.TOM.Bohun = "0";
                clsPmpaType.TOM.Rep = " ";
                clsPmpaType.TOM.Part = "#";
                clsPmpaType.TOM.Fee1 = 0; //근육주사 수기료
                clsPmpaType.TOM.Fee2 = 0; //정맥주사 수기료
                clsPmpaType.TOM.Fee3 = 0; //내복조제 수기료
                clsPmpaType.TOM.Fee31 = 0; //내복제제 수기료
                clsPmpaType.TOM.Fee5 = 0; //외용조제 수기료
                clsPmpaType.TOM.Fee51 = 0; //외용제제 수기료
                clsPmpaType.TOM.Fee7 = 0; //수혈 수기료(ABO검사횟수)
                clsPmpaType.TOM.Amt1 = 0; //진찰료 발생금액
                clsPmpaType.TOM.Amt2 = 0; //진찰료 특진료
                clsPmpaType.TOM.Amt3 = 0; //진찰료 총액
                clsPmpaType.TOM.Amt4 = 0; //진찰료 조합부담
                clsPmpaType.TOM.Amt5 = 0; //진찰료 감액
                clsPmpaType.TOM.Amt6 = 0; //진찰료 미수
                clsPmpaType.TOM.Amt7 = 0; //진찰료 영수금액

                switch (clsPmpaType.TOM.Bi)
                {
                    case "11":
                        clsPmpaType.TOM.Bi = "41"; //공단100%
                        break;
                    case "12":
                        clsPmpaType.TOM.Bi = "42"; //직장100%
                        break;
                    case "51": //일반
                    case "53": //계약
                    case "54": //미확인
                        clsPmpaType.TOM.Bi = "51";
                        break;
                    case "52": //자동차보험
                    case "55": //자동차일반
                        clsPmpaType.TOM.Bi = "55";
                        break;
                    default:
                        clsPmpaType.TOM.Bi = "43"; //지역100%
                        break;
                }
            }
            else
            {
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            DtFunc.Dispose();
            DtFunc = null;

            //2018.06.13 박병규 : 재원자 선수납시 본인부담율 구하기 추가
            if (rtnVal == "OK")
            {
                clsAlert cA = new ComPmpaLibB.clsAlert();
                clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();

                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI.Trim(), 1) == "1") { cBON.BI = "11"; }

                //기준일자 세팅
                cBON.SDATE = clsPmpaType.TOM.BDate;
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
                //나이구분 체크
                

                cBON.MCODE = clsPmpaType.TOM.MCode;
                cBON.VCODE = clsPmpaType.TOM.VCode;

                cBON.OGPDBUN = "";

                cBON.FCODE = "";
                if (clsPmpaType.TOM.JinDtl == "22")
                    cBON.FCODE = "03";
                else if (clsPmpaType.TOM.JinDtl == "25")
                    cBON.FCODE = "02";

                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if (VB.Left(cBON.BI, 1) == "1" && cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" || clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }

                cBON.IO = "I";
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, strBDate, cBON.IO);
                //***입원 본인부담율 세팅
                if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
                {
                    if (clsPmpaType.TOM.DeptCode == "ER") { cA.Alert_BonRate(cBON); }
                }
                   

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.IBR.Jin = 0;
                        clsPmpaType.IBR.Bohum = 0;
                        clsPmpaType.IBR.CTMRI = 0;
                    }
                }

                //2018.05.31 박병규 : 입원본인부담율 구하면서 cBON 변수값을 치환시키므로 외래본인부담율 구할때 다시 조건을 설정해준다
                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI.Trim(), 1) == "1") { cBON.BI = "11"; }

                //기준일자 세팅
                cBON.SDATE = clsPmpaType.TOM.BDate;
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
               

                cBON.MCODE = clsPmpaType.TOM.MCode;
                cBON.VCODE = clsPmpaType.TOM.VCode;

                cBON.OGPDBUN = "";

                cBON.FCODE = "";
                if (clsPmpaType.TOM.JinDtl == "22")
                    cBON.FCODE = "03";
                else if (clsPmpaType.TOM.JinDtl == "25")
                    cBON.FCODE = "02";

                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if ( cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" && clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }

                cBON.IO = "O";
                //나이구분 체크
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, strBDate, cBON.IO);

                cBON.JINDTL = clsPmpaType.TOM.JinDtl;

                //***외래 본인부담율 세팅
                if (Read_OBon_Rate(pDbCon, cBON) == false)
                    cA.Alert_BonRate(cBON);

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.OBR.Jin = 0;
                        clsPmpaType.OBR.Bohum = 0;
                        clsPmpaType.OBR.CTMRI = 0;
                    }
                }

            }

            return rtnVal;
        }

        public string SugbF_Check(string ArgBi, string ArgSugbF, string ArgSugbQ, string ArgSugbR)
        {
            string rtnVal = ArgSugbF;

            if (ArgBi == "52" && Convert.ToInt32(ArgSugbF) > 0 && ArgSugbR == "0") { rtnVal = "0"; }
            if ((ArgBi == "31" || ArgBi == "33") && Convert.ToInt32(ArgSugbQ) > 0) { rtnVal = "0"; }

            return rtnVal;
        }

        /// <summary>
        /// 의약분업 체크
        /// </summary>
        /// <param name="ArgCnt"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="oumsad.bas:OutDRUG_Check"/>
        public void Check_OutDrug(PsmhDb pDbCon, int ArgCnt, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";

            int nBun11CNT = 0;  //내복약건수
            int nBun12CNT = 0;  //외용약건수
            int nBun13CNT = 0;  //주사약건수
            int nBun21CNT = 0;  //내복약예외건수
            int nBun22CNT = 0;  //외용약예외건수
            int nBun23CNT = 0;  //수사약예외건수
            int nOnlyOutDrug = 0; //원외처방만 가능한 약품 건수
            int nSugaJ = 0; //수가 J항(원내조제만 가능한 약 건수)

            string strV001 = "";
            string strInside = "";
            string strBunup = ""; //의약분업 사유
            string strERJoje = ""; //ER수납시 원내조제 체크
            string strMF05 = ""; //다이아벡스XR
            string strHigh = "NO"; //마약이 있는지 검사

            clsPmpaPb.GnOutTuyakNo = 0; //원외처방전 투약번호
            clsPmpaPb.GnOutDrugNo = 0; //원외처방전 번호

            clsPmpaPb.GstrBunup11 = "N";
            clsPmpaPb.GstrBunup12 = "N";
            clsPmpaPb.GstrBunup20 = "N";

            strBunup = clsPmpaType.TOM.Bunup.Trim();

            if (clsPmpaType.TOM.Jin == "9" || clsPmpaType.TOM.Jin == "A" || (clsPmpaType.TOM.MksJin == "9" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J")))
                strBunup = "##22";

            for (int i = 0; i < ArgCnt; i++)
            {
                if (string.IsNullOrEmpty(clsPmpaType.SA[i].SuCode) == false)
                {
                    //2018.06.10 박병규 : 처방스프레드에 사용자가 공백행을 입력하여 처리하는경우가 발생하여 아래루틴을 주석처리함.
                    //if (clsPmpaType.SA[i].SuCode == null || clsPmpaType.SA[i].SuCode == "") { break; }

                    if (ArgDept == "ER" && clsPmpaType.SA[i].SuCode.Trim() == "##24") { strERJoje = "OK"; }
                    if (clsPmpaType.SA[i].SuCode.Trim() == "MFO5") { strMF05 = "Y"; }

                    //OCS오더에서 원내조제를 입력한 경우 원내조제 처리
                    if (strBunup == "")
                    {
                        if (string.Compare(clsPmpaType.SA[i].SuCode.Trim(), "##11") >= 0 && string.Compare(clsPmpaType.SA[i].SuCode.Trim(), "##49") <= 0) { strBunup = clsPmpaType.SA[i].SuCode.Trim(); }
                        if (clsPmpaType.SA[i].SuCode.Trim() == "##61") { strBunup = clsPmpaType.SA[i].SuCode.Trim(); }
                        //요양병원. 타병원 입원중 진료 추가
                        if (clsPmpaType.SA[i].SuCode.Trim() == "$$45") { strBunup = clsPmpaType.SA[i].SuCode.Trim(); }
                        if (clsPmpaType.SA[i].SuCode.Trim() == "$$53") { strBunup = clsPmpaType.SA[i].SuCode.Trim(); }
                    }

                    //응급실에서 사용한 약품(퇴원약이 아닌것)은 "8.시술시 필요약품"으로 SET(OCS오더만)
                    if (ArgDept == "ER" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0 && (clsPmpaType.SA[i].Bun == "11" || clsPmpaType.SA[i].Bun == "12"))
                    {
                        if (clsPmpaType.SA[i].OrderNo > 0 && clsPmpaType.SA[i].GbIPD != "T")
                            clsPmpaType.SA[i].SugbO = "8";
                    }

                    //주사 용법코드에서 주사실,약국이 아닌것은 "8.시술시 필요약품"으로 SET
                    if (clsPmpaType.SA[i].Bun == "20" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                    {
                        switch (VB.Mid(clsPmpaType.SA[i].Dev, 5, 2))
                        {
                            case "01"://주사실
                            case "05"://약국
                                break;

                            default:
                                clsPmpaType.SA[i].SugbO = "8";
                                break;
                        }
                    }

                    //주사약제중 주사실로 용법이 난것은 예외항목으로 처리
                    if (clsPmpaType.SA[i].Bun == "20" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0 && clsPmpaType.SA[i].SugbJ.Trim() == "3" && VB.Mid(clsPmpaType.SA[i].Dev, 5, 2) == "01")
                    {
                        if (clsPmpaType.SA[i].SugbO == "0")
                            clsPmpaType.SA[i].SugbO = "$";
                    }

                    //외용약 진료과 용법, 원내혼용중 의약분업 0
                    if (clsPmpaType.SA[i].Bun == "12" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0 && clsPmpaType.SA[i].SugbJ.Trim() == "3" && clsPmpaType.SA[i].Dev.Trim() == "890102")
                    {
                        if (clsPmpaType.SA[i].SugbO == "0")
                            clsPmpaType.SA[i].SugbO = "$";
                    }

                    //약건수 및 예외약품 Check
                    if (clsPmpaType.SA[i].SuCode.Trim() != "" && VB.Left(clsPmpaType.SA[i].SuCode.Trim(), 2) != "##")
                    {
                        if (string.Compare(clsPmpaType.SA[i].SugbA, "1") > 0) //묶음코드
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT a.Bun vBun, a.SugbJ vSugbJ, b.SugbO vSugbO ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUH a, ";
                            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND a.SuCode  = '" + clsPmpaType.SA[i].SuCode.Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND a.Bun IN ('11','12','20') "; //내복,외용,주사
                            SQL += ComNum.VBLF + "    And a.SuNext NOT IN ('NIG06') ";
                            SQL += ComNum.VBLF + "    AND a.SuNext  = b.SuNext ";
                            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("약건수 및 예외약품 Check 조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                DtFunc.Dispose();
                                DtFunc = null;
                                return;
                            }

                            if (DtFunc.Rows.Count > 0)
                            {
                                for (int j = 0; j < DtFunc.Rows.Count; j++)
                                {
                                    switch (DtFunc.Rows[j]["vBUN"].ToString().Trim())
                                    {
                                        case "11": //내복약
                                            if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "1") //원외처방만 가능한 약품
                                            {
                                                nBun11CNT = 1; //원외처방
                                                nOnlyOutDrug = 1; //원외처방만 가능한 약품
                                                clsPmpaType.SA[i].SugbO = "0";
                                            }
                                            else if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "4") //원내조제만 가능한 약품
                                            {
                                                clsPmpaType.SA[i].SugbO = "4";
                                                nSugaJ = 1;
                                            }
                                            else
                                            {
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() == "0") { nBun11CNT = 1; } //원외처방
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() != "0") { nBun21CNT = 1; } //예외약품
                                            }

                                            break;

                                        case "12": //외용약
                                            if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "1") //원외처방만 가능한 약품
                                            {
                                                nBun12CNT = 1;
                                                nOnlyOutDrug = 1;
                                                clsPmpaType.SA[i].SugbO = "0";
                                            }
                                            else if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "4") //원내조제만 가능한 약품
                                            {
                                                clsPmpaType.SA[i].SugbO = "4";
                                                nSugaJ = 1;
                                            }
                                            else
                                            {
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() == "0") { nBun12CNT = 1; } //원외처방
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() != "0") { nBun22CNT = 1; } //예외약품
                                            }

                                            break;

                                        case "20": //주사약
                                            if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "1") //원외처방만 가능한 약품
                                            {
                                                nBun13CNT = 1;
                                                nOnlyOutDrug = 1;
                                                clsPmpaType.SA[i].SugbO = "0";
                                            }
                                            else if (DtFunc.Rows[j]["vSugbJ"].ToString().Trim() == "4") //원내조제만 가능한 약품
                                            {
                                                clsPmpaType.SA[i].SugbO = "4";
                                                nSugaJ = 1;
                                            }
                                            else
                                            {
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() == "0") { nBun13CNT = 1; } //원외처방
                                                if (DtFunc.Rows[j]["vSugbO"].ToString().Trim() != "0") { nBun23CNT = 1; } //예외약품
                                            }

                                            break;
                                    }
                                }
                            }

                            DtFunc.Dispose();
                            DtFunc = null;
                        }
                        else //단순코드
                        {
                            switch (clsPmpaType.SA[i].Bun.Trim())
                            {
                                case "11": //내복약
                                    if (clsPmpaType.SA[i].SugbJ == "1") //원외처방만 가능한 약품
                                    {
                                        nBun11CNT = 1; //원외처방
                                        nOnlyOutDrug = 1; //원외처방만 가능한 약품
                                        clsPmpaType.SA[i].SugbO = "0";
                                    }
                                    else if (clsPmpaType.SA[i].SugbJ == "4") //원내조제만 가능한 약품
                                    {
                                        clsPmpaType.SA[i].SugbO = "4";
                                        nSugaJ = 1;
                                    }
                                    else
                                    {
                                        if (clsPmpaType.SA[i].SugbO == "0") { nBun11CNT = 1; } //원외처방
                                        if (clsPmpaType.SA[i].SugbO != "0") { nBun21CNT = 1; } //예외약품
                                    }

                                    break;

                                case "12": //외용약
                                    if (clsPmpaType.SA[i].SugbJ == "1") //원외처방만 가능한 약품
                                    {
                                        nBun12CNT = 1; //원외처방
                                        nOnlyOutDrug = 1; //원외처방만 가능한 약품
                                        clsPmpaType.SA[i].SugbO = "0";
                                    }
                                    else if (clsPmpaType.SA[i].SugbJ == "4") //원내조제만 가능한 약품
                                    {
                                        clsPmpaType.SA[i].SugbO = "4";
                                        nSugaJ = 1;
                                    }
                                    else
                                    {
                                        if (strHigh == "OK") { clsPmpaType.SA[i].SugbO = "5"; }
                                        if (clsPmpaType.SA[i].SugbO == "0") { nBun12CNT = 1; } //원외처방
                                        if (clsPmpaType.SA[i].SugbO != "0") { nBun22CNT = 1; } //예외약품
                                    }

                                    break;

                                case "20": //주사약
                                    if (clsPmpaType.SA[i].SugbJ == "1") //원외처방만 가능한 약품
                                    {
                                        nBun13CNT = 1; //원외처방
                                        nOnlyOutDrug = 1; //원외처방만 가능한 약품
                                        clsPmpaType.SA[i].SugbO = "0";
                                    }
                                    else if (clsPmpaType.SA[i].SugbJ == "4") //원내조제만 가능한 약품
                                    {
                                        clsPmpaType.SA[i].SugbO = "4";
                                        nSugaJ = 1;
                                    }
                                    else
                                    {
                                        if (clsPmpaType.SA[i].SugbO == "0") { nBun13CNT = 1; } //원외처방
                                        if (clsPmpaType.SA[i].SugbO != "0") { nBun23CNT = 1; } //예외약품
                                    }

                                    break;
                            }
                        }
                    }

                }
            }

            clsPmpaPb.GstrBunup11 = "N";
            clsPmpaPb.GstrBunup12 = "N";
            clsPmpaPb.GstrBunup20 = "N";

            if (strBunup != "" && nOnlyOutDrug == 0) //의사가 원내조제를 선택하고,원외약품이 1건도 없으면
            {
                clsPmpaPb.GstrBunup11 = "N";
                clsPmpaPb.GstrBunup12 = "N";
                clsPmpaPb.GstrBunup20 = "N";

                for (int i = 0; i < ArgCnt; i++)
                {
                    if (clsPmpaType.SA[i].Bun == "11" || clsPmpaType.SA[i].Bun == "12" || clsPmpaType.SA[i].Bun == "20")
                        clsPmpaType.SA[i].SugbO = "$"; //예외환자
                }
            }
            else if (ArgDept == "NP" && string.Compare(clsPmpaType.TOM.Bi, "21") >= 0 && string.Compare(clsPmpaType.TOM.Bi, "29") <= 0) //NP 21,22 보호환자는 무조건 원내조제
            {
                if (nBun21CNT > 0 || nBun22CNT > 0 || nBun23CNT > 0)
                {
                    for (int i = 0; i < ArgCnt; i++)
                    {
                        if (clsPmpaType.SA[i].Bun == "11" || clsPmpaType.SA[i].Bun == "12" || clsPmpaType.SA[i].Bun == "20")
                            clsPmpaType.SA[i].SugbO = "$"; //예외환자
                    }
                }
                else
                {
                    if (nBun11CNT == 1) //내복약
                    {
                        clsPmpaPb.GstrBunup11 = "Y";

                        for (int i = 0; i < ArgCnt; i++)
                        {
                            if (clsPmpaType.SA[i].Bun == "11" && clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                clsPmpaType.SA[i].SugbO = "0";
                        }
                    }

                    if (nBun12CNT == 1) //외용약
                    {
                        clsPmpaPb.GstrBunup12 = "Y";

                        for (int i = 0; i < ArgCnt; i++)
                        {
                            if (clsPmpaType.SA[i].Bun == "12" && clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                clsPmpaType.SA[i].SugbO = "0";
                        }
                    }

                    if (nBun13CNT == 1) //주사약
                    {
                        clsPmpaPb.GstrBunup20 = "Y";

                        for (int i = 0; i < ArgCnt; i++)
                        {
                            if (clsPmpaType.SA[i].Bun == "20" && clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                clsPmpaType.SA[i].SugbO = "0";
                        }
                    }
                }
            }
            else
            {
                if (nBun11CNT == 1) //내복약
                {
                    clsPmpaPb.GstrBunup11 = "Y";

                    for (int i = 0; i < ArgCnt; i++)
                    {
                        if (clsPmpaType.SA[i].Bun == "11")
                        {
                            if (nSugaJ == 1) //원내조제만 가능한 약이 있는 경우
                            {
                                if (clsPmpaType.SA[i].SugbO == "4")
                                    clsPmpaType.SA[i].SugbO = "$";
                                else
                                {
                                    if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                        clsPmpaType.SA[i].SugbO = "0";
                                }
                            }
                            else
                            {
                                if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                    clsPmpaType.SA[i].SugbO = "0";
                            }
                        }
                    }
                }

                if (nBun12CNT == 1) //외용약
                {
                    clsPmpaPb.GstrBunup12 = "Y";

                    for (int i = 0; i < ArgCnt; i++)
                    {
                        if (clsPmpaType.SA[i].Bun == "12")
                        {
                            if (nSugaJ == 1) //원내조제만 가능한 약이 있는 경우
                            {
                                if (clsPmpaType.SA[i].SugbO == "4")
                                    clsPmpaType.SA[i].SugbO = "$";
                                else
                                {
                                    if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                        clsPmpaType.SA[i].SugbO = "0";
                                }
                            }
                            else
                            {
                                if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                    clsPmpaType.SA[i].SugbO = "0";
                            }
                        }
                    }
                }

                if (nBun13CNT == 1) //주사약
                {
                    clsPmpaPb.GstrBunup20 = "Y";

                    for (int i = 0; i < ArgCnt; i++)
                    {
                        if (clsPmpaType.SA[i].Bun == "20")
                        {
                            if (nSugaJ == 1) //원내조제만 가능한 약이 있는 경우
                            {
                                if (clsPmpaType.SA[i].SugbO == "4")
                                    clsPmpaType.SA[i].SugbO = "$";
                                else
                                {
                                    if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                        clsPmpaType.SA[i].SugbO = "0";
                                }
                            }
                            else
                            {
                                if (clsPmpaType.SA[i].SugbO != "$" && string.Compare(clsPmpaType.SA[i].SugbO, "1") < 0)
                                    clsPmpaType.SA[i].SugbO = "0";
                            }
                        }
                    }
                }
            }

            if (strMF05 == "Y")
            {
                for (int i = 0; i < ArgCnt; i++)
                {
                    if (clsPmpaType.SA[i].SuCode.Trim() != "")
                    {
                        if (clsPmpaType.SA[i].SugbO.Trim() != "0" && clsPmpaType.SA[i].SuCode.Trim().Length > 2)
                            clsPmpaPb.GstrMFO5_InSideYN = "Y";

                        break;
                    }

                }
            }

            for (int i = 0; i < ArgCnt; i++)
            {
                if (clsPmpaType.SA[i].SuCode.Trim() != "")
                {
                    if (clsPmpaType.SA[i].SugbO.Trim() != "0" && clsPmpaType.SA[i].SuCode.Trim().Length > 2 && clsPmpaType.SA[i].SugbF.Trim() == "0")
                    {
                        strInside = "Y";
                        break;
                    }

                }
            }

            //HD접수환자 타과 접수시 원외처방전 발행막음(본인10% 부담이기 때문...)
            if (clsPmpaPb.GstrOtherHD == "*" && (nBun11CNT + nBun12CNT + nBun13CNT) > 0 && strInside != "Y")
            {
                for (int i = 0; i < ArgCnt; i++)
                {
                    if (clsPmpaType.SA[i].SuNext.Trim() == "@V001")
                    {
                        strV001 = "OK";
                        break;
                    }
                }

                if (strV001 == "")
                    clsPmpaPb.GstrHDOtherGwaGb = "OK";
            }

            //ER수납시 원내조제 ##24 코드가 없고 내복,외용,주사제가 있으면 확인메세지 발생
            if (ArgDept == "ER" && strERJoje == "")
            {
                for (int i = 0; i < ArgCnt; i++)
                {
                    if (clsPmpaType.SA[i].Bun == "11" || clsPmpaType.SA[i].Bun == "12" || clsPmpaType.SA[i].Bun == "20")
                        clsPmpaPb.GstrERInSideJoje = "OK";
                }
            }
        }



        /// <summary>
        /// Description : 자격 진료승인번호 요청
        /// Author : 박병규
        /// Create Date : 2018.2.5
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <seealso cref="oumsad.bas:M3_HIC_21"/>
        public void M3_HIC_BOHO(PsmhDb pDbCon, string ArgGubun, FarPoint.Win.Spread.FpSpread ssSpreadILL = null)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();
            DataTable DtS = new DataTable();
            clsPmpaSel CPS = new clsPmpaSel();
            string SQL = "";
            string SqlErr = "";
            string strBunUp = "";

            GssSpreadILL = ssSpreadILL;

            //GM3_HIC(1):진료형태,         GM3_HIC(2):입(내)원일수,          GM3_HIC(3):투약일수 
            //GM3_HIC(4):본인일부부담금,   GM3_HIC(5):건강생활유지비청구액,  GM3_HIC(6):기관부담금 
            //GM3_HIC(7):주상병분류기호,   GM3_HIC(8):진료일자,              GM3_HIC(9):처방전교부번호 
            //GM3_HIC(10):본인부담여부,    GM3_HIC(11):타기관의료여부(Y/N)

            clsPmpaPb.GstrBunup2 = ""; //원외처방전발행
            clsPmpaPb.GstrDrugBunup2 = ""; //원내조제
            clsPmpaPb.GstrJupsuCode2 = "OK";
            clsPmpaPb.GstrGemsa2 = "";

            for (int i = 0; i < 999; i++)
            {
                if (ArgGubun =="환불") { break; }
                if (string.IsNullOrEmpty(clsPmpaType.SW[i].SuNext) != true )
                {
                    if (clsPmpaType.SW[i].GbBunup == "0" && clsPmpaType.SW[i].GbSelf == "0" && (clsPmpaType.SW[i].Bun == "11" || clsPmpaType.SW[i].Bun == "12" || clsPmpaType.SW[i].Bun == "20")) //원외처방전발행
                        clsPmpaPb.GstrBunup2 = "OK";
                    else if (clsPmpaType.SW[i].GbBunup != "0" && clsPmpaType.SW[i].GbSelf == "0" && (clsPmpaType.SW[i].Bun == "11" || clsPmpaType.SW[i].Bun == "12" || clsPmpaType.SW[i].Bun == "20")) //원내조제
                        clsPmpaPb.GstrDrugBunup2 = "OK";

                    
                    else if (string.Compare(clsPmpaType.BAT.BDate, "2021-06-01") >= 0 &&  clsPmpaType.SW[i].GbBunup != "0" && clsPmpaType.SW[i].GbSelf == "2" && (clsPmpaType.SW[i].Bun == "11" || clsPmpaType.SW[i].Bun == "12" || clsPmpaType.SW[i].Bun == "20")) //원내조제
                    {
                        CPS.Suga_Read_Select_Gbn(clsDB.DbCon, clsPmpaType.SW[i].SuCode, clsPmpaType.SW[i].SuNext);

                        if (clsPmpaPb.GstrSuDaiCode != "325" && clsPmpaPb.GstrSugbP != "1" && clsPmpaType.SW[i].GbSugbS == "1")
                        {
                            //전액본인부담시(처방시 급여항에 “2”,수납화면에 F항에 “2”)
                            //수가등록화면에서 B항에 묶인 수기료(C-LORV 같은 경우 B항에 “1”) KK010은 급여,
                            //급여처리
                            clsPmpaPb.GstrDrugBunup2 = "OK";
                        }
                    }

                    else if (clsPmpaType.SW[i].SuNext.Trim() == "O9991" || clsPmpaType.SW[i].SuNext.Trim() == "O9992" ) //원내조제(HD환자 O9991이 수가코드 발생시 원내조제)
                        clsPmpaPb.GstrDrugBunup2 = "OK";
                    else if (clsPmpaType.SW[i].GbSelf == "0" && clsPmpaType.SW[i].GbSugbL == "3" && clsPmpaType.SW[i].Bun != "11" && clsPmpaType.SW[i].Bun != "12" && clsPmpaType.SW[i].Bun != "20" && clsPmpaType.SW[i].Bun != "72" && clsPmpaType.SW[i].Bun != "73")
                    {
                        if (clsPmpaType.SW[i].SuNext.Trim() != "O2L")
                            clsPmpaPb.GstrDrugBunup2 = "OK"; //원내조제(검사시 약제를 사용할 경우 원내조제에 들어감  단. CT,MRI는 제외임)
                    }
                    else if (clsPmpaType.SW[i].Bun == "01" || clsPmpaType.SW[i].Bun == "02" || clsPmpaType.SW[i].Bun == "75")//진찰료, 증명료
                        clsPmpaPb.GstrJupsuCode2 = "OK";
                    else if (clsPmpaType.SW[i].SuCode.Trim() == "AY100")//가정간호 기본방문료가 진찰료임
                        clsPmpaPb.GstrJupsuCode2 = "OK";
                    else if (string.Compare(clsPmpaType.SW[i].Bun, "80") < 0 && (VB.Left(clsPmpaType.SW[i].SuCode.Trim(), 2) != "$$" && VB.Left(clsPmpaType.SW[i].SuCode.Trim(), 2) != "##" && VB.Left(clsPmpaType.SW[i].SuCode.Trim(), 2) != "@V") && (clsPmpaType.SW[i].Bun != "11" && clsPmpaType.SW[i].Bun != "12" && clsPmpaType.SW[i].Bun != "20"))
                        clsPmpaPb.GstrGemsa2 = "OK";
                }
            }

            clsPmpaPb.Gm4_Hic_Bun = "";//의료급여 1.처방전미발행  2.처방전발행

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.BAT.Ptno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.BAT.DeptCode + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPmpaType.BAT.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BUN IN ('11','12') ";
            SQL += ComNum.VBLF + "    AND GBBUNUP   = '0'";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return;
            }

            if (DtS.Rows.Count > 0)
            {
                if (Convert.ToDouble(DtS.Rows[0]["CNT"].ToString()) > 0)
                    strBunUp = "Y";
                else
                    strBunUp = "N";
            }

            DtS.Dispose();
            DtS = null;

            if (clsPmpaPb.GstrWonLe == "원내조제정신")
                clsPmpaType.BAT.M3_Tuyak = clsPmpaPb.GnCntNal;

            Read_Opd_Illcode(pDbCon, GssSpreadILL); //외래상병코드

            clsPmpaPb.GnBonInAmt = 0;
            clsPmpaType.BAT.M3_JinType = "2"; //진료형태 1.입원 2.외래
            clsPmpaType.BAT.M3_Ilsu = 1;      //일수
            clsPmpaType.BAT.M3_OutCode = "9"; //1:입원중 2.퇴원 9:기타(외래 등)
            clsPmpaType.BAT.Amt_Manual = 0;

            if (clsPmpaType.TOM.DeptCode == "DT" && (clsPmpaType.TOM.JinDtl == "02" || clsPmpaType.TOM.JinDtl == "07")) { return; }

            if (clsPmpaType.TOM.DeptCode == "")
            {
            }
            else
            { 
                if (clsPmpaType.TOM.Bi == "21")
                {
                    if (clsPmpaType.TOM.MCode == "M000")
                    {
                        if (clsPmpaType.TOM.Jin == "9")
                        {
                            if (CPF.Read_Boho_HomeCare_Exception(pDbCon, clsPmpaType.TOM.Pano, clsPmpaType.TOM.BDate) == "OK")//의료급여_가정간호_강제대상
                            {
                                if (clsPmpaPb.GstrBunup2 == "OK")
                                {
                                    clsPmpaType.BAT.Amt = 1500;
                                    clsPmpaPb.GnBonInAmt = 1500;
                                    clsPmpaPb.Gm4_Hic_Bun = "2"; //처방전발행

                                    if (ArgGubun == "부분취소")
                                        Read_OutDrug_LastDate(pDbCon);//원외처방전최종
                                    else
                                    {
                                        Read_OutDrug_NewNo(pDbCon);//원외처방전신규번호  
                                        clsPmpaType.BAT.M3_ODrug = clsPmpaPb.GnOutDrugNo.ToString();
                                    }
                                }
                                else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "" && clsPmpaPb.GstrGemsa2 == "")
                                {
                                    //접수비(진찰료만 발생할 경우)
                                    clsPmpaType.BAT.Amt = 1500;
                                    clsPmpaPb.GnBonInAmt = 1500;
                                    clsPmpaPb.Gm4_Hic_Bun = "2"; //처방전발행
                                    clsPmpaType.BAT.M3_ODrug = "";
                                }
                                else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "OK")
                                {
                                    clsPmpaType.BAT.Amt = 2000;
                                    clsPmpaPb.GnBonInAmt = 2000;
                                    clsPmpaPb.Gm4_Hic_Bun = "1";
                                    clsPmpaType.BAT.M3_ODrug = "";
                                }
                                else
                                {
                                    clsPmpaType.BAT.Amt = 1500;
                                    clsPmpaPb.GnBonInAmt = 1500;
                                    clsPmpaPb.Gm4_Hic_Bun = "2"; //처방전발행
                                    clsPmpaType.BAT.M3_ODrug = "";
                                }
                            }
                            else
                            {
                                if (clsPmpaPb.GstrBunup11 != "Y" && clsPmpaPb.GstrBunup12 != "Y" && clsPmpaPb.GstrBunup20 != "Y")
                                {
                                    if (strBunUp == "Y")
                                    {
                                        clsPmpaPb.Gm4_Hic_Bun = "2";
                                        Read_OutDrug_LastDate(pDbCon);
                                    }
                                    else
                                        clsPmpaPb.Gm4_Hic_Bun = "";

                                    clsPmpaType.BAT.Amt = 0;
                                    clsPmpaPb.GnBonInAmt = 0;
                                }
                                else
                                {
                                    clsPmpaType.BAT.Amt = 0;
                                    clsPmpaPb.GnBonInAmt = 0;

                                    if (ArgGubun == "부분취소")
                                        Read_OutDrug_LastDate(pDbCon);
                                    else
                                    {
                                        Read_OutDrug_NewNo(pDbCon);
                                        clsPmpaType.BAT.M3_ODrug = clsPmpaPb.GnOutDrugNo.ToString();

                                        if (clsPmpaType.BAT.BDate != clsPublic.GstrSysDate)
                                            clsPmpaType.BAT.M3_ODrug = VB.Left(clsPmpaType.BAT.BDate.Trim(), 4) + VB.Mid(clsPmpaType.BAT.BDate.Trim(), 6, 2) + VB.Right(clsPmpaType.BAT.BDate.Trim(), 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaPb.GnOutDrugNo));
                                    }

                                    clsPmpaPb.Gm4_Hic_Bun = "";
                                }
                            }
                        }
                        else if (clsPmpaType.TOM.Jin == "2" && clsPmpaType.TOM.DeptCode != "ER" && ((clsPmpaType.AAT[8].Amt1 - clsPmpaPb.Gn100SAmt) < 2000) ) //무료접수 급여분총액 - 선별급여 지원비보다 적은 경우 0원처리 
                        {
                            clsPmpaType.BAT.Amt = 0;
                            clsPmpaPb.GnBonInAmt = 0;
                            clsPmpaPb.Gm4_Hic_Bun = "";
                            clsPmpaType.BAT.M3_ODrug = "";
                        }

                        else if (clsPmpaType.TOM.Jin == "3") //신생아
                        {
                            clsPmpaType.BAT.Amt = 0;
                            clsPmpaPb.GnBonInAmt = 0;
                            clsPmpaPb.Gm4_Hic_Bun = "";
                            clsPmpaType.BAT.M3_ODrug = "";
                        }

                        else if  (clsPmpaPb.GstatEROVER == "*")  //응급실 입원 
                            {
                            clsPmpaType.BAT.Amt = 0;
                            clsPmpaPb.GnBonInAmt = 0;
                            clsPmpaPb.Gm4_Hic_Bun = "";
                            clsPmpaType.BAT.M3_ODrug = "";
                        }

                        else
                        {
                            if (clsPmpaPb.GstrBunup2 == "OK")
                            {
                                clsPmpaType.BAT.Amt = 1500;
                                clsPmpaPb.GnBonInAmt = 1500;
                                clsPmpaPb.Gm4_Hic_Bun = "2";

                                if (ArgGubun == "부분취소")
                                    Read_OutDrug_LastDate(pDbCon);
                                else
                                {
                                    Read_OutDrug_NewNo(pDbCon);
                                    clsPmpaType.BAT.M3_ODrug = clsPmpaPb.GnOutDrugNo.ToString();

                                    if (clsPmpaType.BAT.BDate != clsPublic.GstrSysDate)
                                        clsPmpaType.BAT.M3_ODrug = VB.Left(clsPmpaType.BAT.BDate.Trim(), 4) + VB.Mid(clsPmpaType.BAT.BDate.Trim(), 6, 2) + VB.Right(clsPmpaType.BAT.BDate.Trim(), 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaPb.GnOutDrugNo));
                                }
                            }
                            else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "" && clsPmpaPb.GstrGemsa2 == "")
                            {
                                //접수비(진찰료만 발생할 경우)
                                clsPmpaType.BAT.Amt = 1500;
                                clsPmpaPb.GnBonInAmt = 1500;
                                clsPmpaPb.Gm4_Hic_Bun = "2";
                                clsPmpaType.BAT.M3_ODrug = "";
                            }
                            else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "OK")
                            {
                                clsPmpaType.BAT.Amt = 2000;
                                clsPmpaPb.GnBonInAmt = 2000;
                                clsPmpaPb.Gm4_Hic_Bun = "1";
                                clsPmpaType.BAT.M3_ODrug = "";
                            }
                            else
                            {
                                clsPmpaType.BAT.Amt = 1500;
                                clsPmpaPb.GnBonInAmt = 1500;
                                clsPmpaPb.Gm4_Hic_Bun = "2";
                                clsPmpaType.BAT.M3_ODrug = "";
                            }
                        }
                    }
                    else if (clsPmpaType.TOM.MCode == "")
                        ComFunc.MsgBox("의료급여 본인부담코드가 없습니다. 꼭 의료정보과로 전화 주세요 ★승인하지 마세요★");
                    else
                    {
                        if (clsPmpaPb.GstrBunup11 != "Y" && clsPmpaPb.GstrBunup12 != "Y" && clsPmpaPb.GstrBunup20 != "Y")
                        {
                            if (strBunUp == "Y")
                            {
                                clsPmpaPb.Gm4_Hic_Bun = "2";
                                Read_OutDrug_LastDate(pDbCon);
                            }
                            else
                                clsPmpaPb.Gm4_Hic_Bun = "";

                            clsPmpaType.BAT.Amt = 0;
                            clsPmpaPb.GnBonInAmt = 0;
                        }
                        else
                        {
                            clsPmpaType.BAT.Amt = 0;
                            clsPmpaPb.GnBonInAmt = 0;

                            if (ArgGubun == "부분취소")
                                Read_OutDrug_LastDate(pDbCon);
                            else
                            {
                                Read_OutDrug_NewNo(pDbCon);
                                clsPmpaType.BAT.M3_ODrug = clsPmpaPb.GnOutDrugNo.ToString();

                                if (clsPmpaType.BAT.BDate != clsPublic.GstrSysDate)
                                    clsPmpaType.BAT.M3_ODrug = VB.Left(clsPmpaType.BAT.BDate.Trim(), 4) + VB.Mid(clsPmpaType.BAT.BDate.Trim(), 6, 2) + VB.Right(clsPmpaType.BAT.BDate.Trim(), 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaPb.GnOutDrugNo));
                            }

                            clsPmpaPb.Gm4_Hic_Bun = "";
                        }
                    }
                }
                else if (clsPmpaType.TOM.Bi == "22")
                {
                    if (clsPmpaPb.GstrBunup11 != "Y" && clsPmpaPb.GstrBunup12 != "Y" && clsPmpaPb.GstrBunup20 != "Y")
                    {
                        clsPmpaType.BAT.Amt = 0;
                        clsPmpaPb.Gm4_Hic_Bun = "";
                        Read_OutDrug_LastDate(pDbCon);
                    }
                    else
                    {
                        clsPmpaType.BAT.Amt = 0;

                        if (ArgGubun == "부분취소")
                            Read_OutDrug_LastDate(pDbCon);
                        else
                        {
                            Read_OutDrug_NewNo(pDbCon);
                            clsPmpaType.BAT.M3_ODrug = clsPmpaPb.GnOutDrugNo.ToString();

                            if (clsPmpaType.BAT.BDate != clsPublic.GstrSysDate)
                                clsPmpaType.BAT.M3_ODrug = VB.Left(clsPmpaType.BAT.BDate.Trim(), 4) + VB.Mid(clsPmpaType.BAT.BDate.Trim(), 6, 2) + VB.Right(clsPmpaType.BAT.BDate.Trim(), 2) + string.Format("{0:D5}", Convert.ToInt64(clsPmpaPb.GnOutDrugNo));
                        }

                        clsPmpaPb.Gm4_Hic_Bun = "";
                    }
                }
            }

            int nNal = Read_InDrug_Ilsu(pDbCon);//원내처방_일수

            if (clsPmpaType.BAT.M3_Tuyak < nNal)
                clsPmpaType.BAT.M3_Tuyak = nNal;

            //AIDS 관련 강제처리
            if (clsPmpaPb.GstrChkHIV == "OK")
            {
                clsPmpaType.BAT.Amt = 0;
                clsPmpaPb.GnBonInAmt = 0;
            }

            //타기관 입원중인 의료급여 환자 승인금액 0원(시작일자가 필요하면 시작일자를 조건에 추가하기)
            if (clsPmpaType.TOM.Bi == "21" && clsPmpaType.TOM.JinDtl == "18")
            {
                clsPmpaType.BAT.Amt = 0;
                clsPmpaPb.GnBonInAmt = 0;
                clsPmpaPb.Gm4_Hic_Bun = "";
            }

            //금연처방 강제처리
            if (clsPmpaType.TOM.JinDtl == "12")
            {
                clsPmpaType.BAT.Amt = 0;
                clsPmpaPb.GnBonInAmt = 0;
            }
        }

        /// <summary>
        /// 원내처방_일수
        /// </summary>
        /// <seealso cref="oumsad.bas:원내처방_일수"/>
        private int Read_InDrug_Ilsu(PsmhDb pDbcon)
        {
            DataTable DtS = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(NAL), 0) NAL ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_DRUGATC ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";

            if (clsPmpaType.TOM.DeptCode.Trim() == "HD")
                SQL += ComNum.VBLF + "AND GBIO   = '1' ";
            else
                SQL += ComNum.VBLF + "AND GBIO   = 'O' ";

            SQL += ComNum.VBLF + "    AND TUYAKNO IN ( SELECT MAX(TUYAKNO) FROM " + ComNum.DB_MED + "OCS_DRUGATC ";
            SQL += ComNum.VBLF + "                      WHERE BDATE     = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "                        AND PANO      = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "                        AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";

            if (clsPmpaType.TOM.DeptCode == "HD")
                SQL += ComNum.VBLF + "   AND GBIO   = '1' ) ";
            else
                SQL += ComNum.VBLF + "   AND GBIO   = 'O' ) ";

            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbcon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
                rtnVal = Convert.ToInt32(VB.Val(DtS.Rows[0]["NAL"].ToString().Trim()));
            else
                rtnVal = 0;

            DtS.Dispose();
            DtS = null;

            return rtnVal;
        }

        /// <summary>
        /// 외래_상병코드
        /// </summary>
        /// <seealso cref="oumsad.bas:외래_상병코드"/>
        public void Read_Opd_Illcode(PsmhDb pDbcon, FarPoint.Win.Spread.FpSpread ssSpreadILL = null)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();
            DataTable DtS = new DataTable();
            string SQL = "";
            string SqlErr = "";

            GssSpreadILL = ssSpreadILL;

            SQL = "";

            if (clsPmpaType.TOM.DeptCode == "ER")
                SQL += ComNum.VBLF + " SELECT ILLCODE FROM " + ComNum.DB_MED + "OCS_EILLS ";
            else
                SQL += ComNum.VBLF + " SELECT ILLCODE FROM " + ComNum.DB_MED + "OCS_OILLS ";

            SQL += ComNum.VBLF + " WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "   AND PTNO       = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "   AND BDATE      = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND DEPTCODE   = '" + clsPmpaType.TOM.DeptCode + "' ";

            if (clsPmpaType.TOM.DeptCode != "ER")
                SQL += ComNum.VBLF + " ORDER BY SEQNO  ";

            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbcon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return;
            }

            if (DtS.Rows.Count > 0)
                clsPmpaType.BAT.M3_Msym = CPF.Get_KCD6(pDbcon, DtS.Rows[0]["ILLCODE"].ToString().Trim());
            else
                clsPmpaType.BAT.M3_Msym = "";

            DtS.Dispose();
            DtS = null;

            if (clsPmpaType.BAT.M3_Msym == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ILLCODE1 FROM " + ComNum.DB_PMPA + "ETC_PTMASTER ";
                SQL += ComNum.VBLF + "  WHERE PANO      = '" + clsPmpaType.TOM.Pano + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbcon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtS.Dispose();
                    DtS = null;
                    return;
                }

                if (DtS.Rows.Count > 0)
                    clsPmpaType.BAT.M3_Msym = CPF.Get_KCD6(pDbcon, DtS.Rows[0]["ILLCODE1"].ToString().Trim());
                else
                    clsPmpaType.BAT.M3_Msym = "";

                DtS.Dispose();
                DtS = null;
            }

            if (clsPmpaType.BAT.M3_Msym == "")
                clsPmpaType.BAT.M3_Msym = GssSpreadILL.ActiveSheet.Cells[0, 1].Text.Trim();
        }

        /// <summary>
        /// 추가수납시 원외처방전 있으면 최종건 조회
        /// </summary>
        /// <seealso cref="oumsad.bas:원외처방전최종"/>
        public void Read_OutDrug_LastDate(PsmhDb pDbcon)
        {
            DataTable DtS = new DataTable();
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, TO_CHAR(SLIPDATE,'YYYYMMDD') SLIPDATE, ";
            SQL += ComNum.VBLF + "        MAX(SLIPNO) SLIPNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "    AND FLAG      = 'P' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";
            SQL += ComNum.VBLF + "  GROUP BY PANO, TO_CHAR(SLIPDATE,'YYYYMMDD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbcon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return;
            }

            if (DtS.Rows.Count > 0)
                clsPmpaType.BAT.M3_ODrug = DtS.Rows[0]["SLIPDATE"].ToString().Trim() + string.Format("{0:D5}", Convert.ToInt64(DtS.Rows[0]["SLIPNO"]));

            DtS.Dispose();
            DtS = null;
        }


        /// <summary>
        /// 해당과의 당일 원외처방전중 인쇄하지 않은 처방전이 있으면 합산
        /// </summary>
        /// <seealso cref="oumsad.bas:원외처방전신규번호"/>
        public void Read_OutDrug_NewNo(PsmhDb pDbcon)
        {
            DataTable DtO = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SLIPNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SLIPDATE  = trunc(sysdate) ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPmpaType.TOM.BDate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";
            SQL += ComNum.VBLF + "    AND FLAG      = '0' ";
            SqlErr = clsDB.GetDataTableEx(ref DtO, SQL, pDbcon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtO.Dispose();
                DtO = null;
                return;
            }

            if (DtO.Rows.Count > 0)
                clsPmpaPb.GnOutDrugNo = Convert.ToInt32(VB.Val(DtO.Rows[0]["SlipNo"].ToString().Trim()));
            else
            {
                clsPmpaPb.GnOutDrugNo = 0;

                SQL = "";

                if (clsPmpaType.TOM.BDate != clsPublic.GstrSysDate)
                {
                    SQL += ComNum.VBLF + " SELECT MAX(SLIPNO) + 1 OutDrugNo  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                    SQL += ComNum.VBLF + "  WHERE BDATE = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "SELECT SEQ_OUTDRUG.NEXTVAL OutDrugNo FROM DUAL ";
                }
                SqlErr = clsDB.GetDataTableEx(ref DtSub, SQL, pDbcon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                    clsPmpaPb.GnOutDrugNo = Convert.ToInt32(DtSub.Rows[0]["OUTDRUGNO"].ToString());
                else
                {
                    clsPmpaPb.GnOutDrugNo = 0;
                    ComFunc.MsgBox("원외처방전 신규번호 부여시 오류가 발생");
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                DtSub.Dispose();
                DtSub = null;
            }

            DtO.Dispose();
            DtO = null;
        }

        /// <summary>
        /// Description : 차상위2종 만성질환(장애인) 및 만18세 대상자
        /// Author : 박병규
        /// Create Date : 2018.2.5
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <seealso cref="oumsad.bas:M3_HIC_13_차상위2"/>
        public void M3_HIC_NPG(PsmhDb pDbcon, string ArgGubun)
        {
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            DataTable DtS = new DataTable();
            string SQL = "";
            string SqlErr = "";

            int nBuCount = 0;  //특정기호 V001~
            int nBuCount2 = 0; //중증환자 V193
            string strBunUp = "";

            clsPmpaPb.GnBoninAmt_EF = 0;//차상위2종 1,500원 100원금액
            clsPmpaPb.GstrDrugBunup2 = "";
            clsPmpaPb.GstrBunup2 = "";
            clsPmpaPb.GstrJupsuCode2 = "OK"; //차상위E,F는 진찰료 수납시 입력함.. 추가수납시 진찰코드 안넣기 때문에 기본 OK로 세팅
            clsPmpaPb.GstrGemsa2 = "";

            if (VB.Left(clsPmpaType.TOM.Bi, 1) != "1") { return; }
            if (clsPmpaType.TOM.MCode != "E000" && clsPmpaType.TOM.MCode != "F000") { return; }

            for (int i = 0; i < 999; i++)
            {
                if (string.IsNullOrEmpty(clsPmpaType.SW[i].SuNext) == false)
                {
                    switch (clsPmpaType.SW[i].SuCode.Trim())
                    {
                        case "@V001":
                        case "@V003":
                        case "@V005":
                        case "@V009":
                        case "@V027":
                        case "@V117":
                        case "@V012":
                        case "@V013":
                        case "@V014":
                        case "@V015":
                        case "@V191":
                        case "@V192":
                        case "@V193":
                        case "@V194":
                            nBuCount += 1;

                            break;
                    }

                    if (clsPmpaType.TOM.VCode == "EV00" || clsPmpaType.TOM.VCode == "V206" || clsPmpaType.TOM.VCode == "V231" || clsPmpaType.TOM.VCode == "V246")
                    {
                        if (CPF.Read_Rare_Vcode(pDbcon, clsPmpaType.SW[i].SuCode.Trim()) == "OK")
                            nBuCount += 1;
                    }

                    if (clsPmpaType.SW[i].SuCode.Trim() == "@V193" || clsPmpaType.SW[i].SuCode.Trim() == "@V194")
                        nBuCount2 = 1;

                    if (clsPmpaType.SW[i].GbBunup.Trim() == "0" && (clsPmpaType.SW[i].Bun == "11" || clsPmpaType.SW[i].Bun == "12" || clsPmpaType.SW[i].Bun == "20"))
                        clsPmpaPb.GstrBunup2 = "OK"; //원외처방전 발행
                    else if (clsPmpaType.SW[i].GbBunup.Trim() != "0" && clsPmpaType.SW[i].GbSelf == "0" && (clsPmpaType.SW[i].Bun == "11" || clsPmpaType.SW[i].Bun == "12" || clsPmpaType.SW[i].Bun == "20"))
                        clsPmpaPb.GstrDrugBunup2 = "OK"; //원내조제
                    else if ((clsPmpaType.SW[i].SuCode.Trim() == "E7660" || clsPmpaType.SW[i].SuCode.Trim() == "E7660S" || clsPmpaType.SW[i].SuCode.Trim() == "E7630" || clsPmpaType.SW[i].SuCode.Trim() == "E7630S") && clsPmpaType.SW[i].GbSelf == "0")
                        clsPmpaPb.GstrDrugBunup2 = "OK"; //원내조제
                    else if (clsPmpaType.SW[i].SuCode.Trim() == "O9991" || clsPmpaType.SW[i].SuCode.Trim() == "O9992" )
                        clsPmpaPb.GstrDrugBunup2 = "OK"; //원내조제
                    else if (clsPmpaType.SW[i].GbSugbL == "3" && clsPmpaType.SW[i].Bun != "11" && clsPmpaType.SW[i].Bun != "12" && clsPmpaType.SW[i].Bun != "20" && clsPmpaType.SW[i].Bun != "72" && clsPmpaType.SW[i].Bun != "73")
                        clsPmpaPb.GstrDrugBunup2 = "OK"; //원내조제(검사시 약제를 사용할 경우 원내조제)
                    else if (clsPmpaType.SW[i].Bun == "01" && clsPmpaType.SW[i].Bun == "02" && clsPmpaType.SW[i].Bun == "75")
                        clsPmpaPb.GstrJupsuCode2 = "OK";
                    else if (clsPmpaType.SW[i].SuCode.Trim() == "AY100")
                        clsPmpaPb.GstrJupsuCode2 = "OK"; //가정간호 기본방문료가 진찰료
                    else if (string.Compare(clsPmpaType.SW[i].Bun, "80") < 0 && (VB.Left(clsPmpaType.SW[i].SuCode, 2) != "$$" && VB.Left(clsPmpaType.SW[i].SuCode, 2) != "##" && VB.Left(clsPmpaType.SW[i].SuCode, 2) != "@V") && (clsPmpaType.SW[i].Bun != "11" && clsPmpaType.SW[i].Bun != "12" && clsPmpaType.SW[i].Bun != "20"))
                        clsPmpaPb.GstrGemsa2 = "OK";
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.TOM.Pano + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BUN IN ('11','12') ";
            SQL += ComNum.VBLF + "    AND GBBUNUP   = '0'";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbcon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return;
            }

            if (DtS.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtS.Rows[0]["CNT"].ToString()) > 0)
                    strBunUp = "Y";
                else
                    strBunUp = "N";
            }

            DtS.Dispose();
            DtS = null;

            if (nBuCount == 0) { return; }

            if (clsPmpaPb.GstrBunup2 == "OK")
            {
                clsPmpaPb.GnBonInAmt = 1000;
                clsPmpaPb.GnBoninAmt_EF = 1000;
            }
            else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "" && clsPmpaPb.GstrGemsa2 == "")
            {
                clsPmpaPb.GnBonInAmt = 1000;
                clsPmpaPb.GnBoninAmt_EF = 1000;
            }
            else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrDrugBunup2 == "OK")
            {
                clsPmpaPb.GnBonInAmt = 1500;
                clsPmpaPb.GnBoninAmt_EF = 1500;
            }
            else if (clsPmpaPb.GstrJupsuCode2 == "OK" && clsPmpaPb.GstrGemsa2 == "OK")
            {
                clsPmpaPb.GnBonInAmt = 1000;
                clsPmpaPb.GnBoninAmt_EF = 1000;
            }
            else
            {
                clsPmpaPb.GnBonInAmt = 1000;
                clsPmpaPb.GnBoninAmt_EF = 1000;
            }
        }

        /// <summary>
        /// Description : 투약전광판 Data INSERT
        /// author : 박병규
        /// Create Date : 2018-02-12
        /// <param name="str"></param>
        /// <seealso cref="OUMSAD.bas : ETC_TUYAK_INSERT"/>
        /// </summary>
        public void Etc_Tuyak_Insert(PsmhDb pDbcon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strGbn = ""; //1.내복, 2.외용, 3.내복+외용
            int nNal = 0;

            if (clsPmpaPb.GnTuyakNo == 0) { return; }

            if ((clsPmpaPb.G7NAL11 > 0 || clsPmpaPb.G7NAL11A > 0) && (clsPmpaPb.G7NAL12 > 0 || clsPmpaPb.G7NAL12A > 0))
                strGbn = "3";
            else if (clsPmpaPb.G7NAL12 > 0 || clsPmpaPb.G7NAL12A > 0)
                strGbn = "2";
            else
                strGbn = "1";

            nNal = clsPmpaPb.G7NAL11;
            if (clsPmpaPb.G7NAL11A > nNal) { nNal = clsPmpaPb.G7NAL11A; }
            if (clsPmpaPb.G7NAL12A > nNal) { nNal = clsPmpaPb.G7NAL12A; }
            if (clsPmpaPb.G7NAL12 > nNal) { nNal = clsPmpaPb.G7NAL12; }



            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "ETC_TUYAK ";
                SQL += ComNum.VBLF + "       (TuDate, TuNo, Flag, ";
                SQL += ComNum.VBLF + "        Pano, Sname, SunapTime,";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TuIlsu, ";
                SQL += ComNum.VBLF + "        TuGubun, SlipGbn, PRTBUN, ";
                SQL += ComNum.VBLF + "        WorkGbn) ";
                SQL += ComNum.VBLF + " SELECT TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.GnTuyakNo + ", ";
                SQL += ComNum.VBLF + "        '0', ";
                SQL += ComNum.VBLF + "        a.Pano, ";
                SQL += ComNum.VBLF + "        b.Sname, ";
                SQL += ComNum.VBLF + "        SYSDATE,";
                SQL += ComNum.VBLF + "        a.DeptCode, ";
                SQL += ComNum.VBLF + "        a.DrCode, ";
                SQL += ComNum.VBLF + "         " + nNal + ", ";
                SQL += ComNum.VBLF + "        '" + strGbn + "', ";
                SQL += ComNum.VBLF + "        '0', ";
                SQL += ComNum.VBLF + "        '" + VB.Left(clsPmpaPb.GstrPrtBun.Trim(), 1) + "', ";
                SQL += ComNum.VBLF + "        '" + clsPmpaPb.GstrDrugJobGb + "'  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_DRUGATC a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_Patient b ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND a.BDate   = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND a.TuyakNo = " + clsPmpaPb.GnTuyakNo + " ";
                SQL += ComNum.VBLF + "    AND a.Pano    = b.Pano(+) ";
                SQL += ComNum.VBLF + "    AND ROWNUM    <= 1 ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbcon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbcon);
                    ComFunc.MsgBox("약국 ATC Data Insert Error !", "경고 (전산실연락요망)");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbcon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbcon);
                Cursor.Current = Cursors.Default;
                return;
            }

            return;
        }

        /// <summary>
        /// 원외처방 발행용 DATA INSERT
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgSeqNo"></param>
        /// <param name="ArgPrt"></param>
        /// <returns></returns>
        /// <seealso cref="oumsad.bas:OutDRUG_INSERT"/>
        public string OutDrug_Insert(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgBdate, int ArgSeqNo, string ArgPrt)
        {
            DataTable DtS = new DataTable();
            clsOumsadChk CPO = new clsOumsadChk();

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            int nSeqNo = 0;
            string strRowID = "";
            string strCheck252 = "N";
            string strCheck352 = "N";
            int nBunupCnt = 0;
            string strDiease1 = "";
            string strDiease2 = "";
            string strDiease1_RO = "";
            string strDiease2_RO = "";

            //투약대기번호
            switch (clsPmpaType.TOM.Bi)
            {
                case "21":
                case "22":
                    if (clsPmpaPb.GstrHoan == "OK") //의료급여 환불할때 투약번호 새로 생성
                    {
                        clsPmpaPb.GnOutTuyakNo = 0;
                        clsPmpaPb.GnOutDrugNo = 0;
                    }

                    break;

                default:
                    clsPmpaPb.GnOutTuyakNo = 0;
                    clsPmpaPb.GnOutDrugNo = 0;

                    break;
            }

            //의약분업이 아니면 처리안함
            if (clsPmpaPb.GstrBunup11 != "Y" && clsPmpaPb.GstrBunup12 != "Y" && clsPmpaPb.GstrBunup20 != "Y")
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //내복,외용,주사 원외처방전 자료가 없으면 처리안함
            SQL = "";
            //   SQL += ComNum.VBLF + " SELECT SuNext, NVL(SUM(Qty*NAL), 0) Qty ";
            SQL += ComNum.VBLF + " SELECT SuNext, NVL(SUM(NAL), 0) Qty ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Part      = '" + clsType.User.IdNumber + "' ";
            SQL += ComNum.VBLF + "    AND SeqNo     = " + ArgSeqNo + " ";
            SQL += ComNum.VBLF + "    AND Bun IN ('11','12','20') "; //내복,외용,주사
            SQL += ComNum.VBLF + "    AND GbBunup   = '0' "; //원내조제는 제외
            SQL += ComNum.VBLF + "  GROUP BY SuNext ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
            {
                for (int i = 0; i < DtS.Rows.Count; i++)
                {
                    if (DtS.Rows[i]["SuNext"].ToString() != "")
                    {
                        if (Convert.ToInt32(VB.Val(DtS.Rows[i]["QTY"].ToString())) != 0)
                            nBunupCnt += 1;
                    }
                }
            }

            DtS.Dispose();
            DtS = null;

            if (nBunupCnt == 0)
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //상병체크
            SQL = "";
            SQL += ComNum.VBLF + " SELECT IllCode, RO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OILLS ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "  ORDER BY SeqNo ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
            {
                strDiease1 = DtS.Rows[0]["IllCode"].ToString();
                strDiease1_RO = DtS.Rows[0]["RO"].ToString();
            }

            if (DtS.Rows.Count > 1)
            {
                strDiease2 = DtS.Rows[1]["IllCode"].ToString();
                strDiease2_RO = DtS.Rows[1]["RO"].ToString();
            }

            DtS.Dispose();
            DtS = null;

            if (strDiease1 != "")
            {
                //차상위의경우 V252
                if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaType.TOM.MCode.Trim() == "" || clsPmpaType.TOM.MCode.Trim() == "E000" || clsPmpaType.TOM.MCode.Trim() == "F000") && (clsPmpaType.TOM.VCode.Trim() == "" || clsPmpaType.TOM.VCode.Trim() == "F003") && clsPmpaType.TOM.INSULIN != "Y" && strDiease1 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT rowid ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                    SQL += ComNum.VBLF + "  WHERE ILLCODE   = '" + strDiease1 + "' ";
                    SQL += ComNum.VBLF + "    AND GbV252    = '*' ";
                    SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtS.Dispose();
                        DtS = null;
                        return rtnVal;
                    }

                    if (DtS.Rows.Count > 0)
                        strCheck252 = "Y";

                    DtS.Dispose();
                    DtS = null;
                }

                //차상위의경우 V352
                if ( string.Compare(clsPmpaType.TOM.BDate, "2018-11-01") >= 0 && VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaType.TOM.MCode.Trim() == "" || clsPmpaType.TOM.MCode.Trim() == "E000" || clsPmpaType.TOM.MCode.Trim() == "F000") && (clsPmpaType.TOM.VCode.Trim() == "" || clsPmpaType.TOM.VCode.Trim() == "F003") && clsPmpaType.TOM.INSULIN != "Y" && strDiease1 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT rowid ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                    SQL += ComNum.VBLF + "  WHERE ILLCODE   = '" + strDiease1 + "' ";
                    SQL += ComNum.VBLF + "    AND GbV352    = '*' ";
                    SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtS.Dispose();
                        DtS = null;
                        return rtnVal;
                    }

                    if (DtS.Rows.Count > 0)
                        strCheck352 = "Y";

                    DtS.Dispose();
                    DtS = null;
                }

                //의료급여 대상자도 V252
                if ((clsPmpaType.TOM.Bi == "21" || clsPmpaType.TOM.Bi == "22") && strDiease1 != "")
                {
                    strCheck252 = "N";

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT rowid ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                    SQL += ComNum.VBLF + "  WHERE ILLCODE   = '" + strDiease1 + "' ";
                    SQL += ComNum.VBLF + "    AND GbV252    = '*' ";
                    SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtS.Dispose();
                        DtS = null;
                        return rtnVal;
                    }

                    if (DtS.Rows.Count > 0)
                        strCheck252 = "Y";

                    DtS.Dispose();
                    DtS = null;
                }

                //의료급여 대상자도 V352
                if ( string.Compare(clsPmpaType.TOM.BDate, "2018-11-01") >= 0 &&  (clsPmpaType.TOM.Bi == "21" || clsPmpaType.TOM.Bi == "22") && strDiease1 != "")
                {
                    strCheck352 = "N";

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT rowid ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                    SQL += ComNum.VBLF + "  WHERE ILLCODE   = '" + strDiease1 + "' ";
                    SQL += ComNum.VBLF + "    AND GbV352    = '*' ";
                    SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtS.Dispose();
                        DtS = null;
                        return rtnVal;
                    }

                    if (DtS.Rows.Count > 0)
                        strCheck352 = "Y";

                    DtS.Dispose();
                    DtS = null;
                }

            }

            //당뇨주사제 처방받은 사람은 V252 경증대상자에서 제외
            if (strCheck252 == "Y")
            {
                if (CPO.Check_Except_V252(clsPmpaType.TOM.Pano, clsPmpaType.TOM.DeptCode, clsPmpaType.TOM.BDate) == true)
                {
                    if (string.Compare(clsPmpaType.TOM.BDate, "2018-11-01") >= 0)
                    {
                        strCheck252 = "C";
                    }
                    else
                    {
                        strCheck252 = "";
                    }
                }
            }
           

            //V352 대상중 해당상병 및 6세미만 경증 제외
            if (strCheck352 == "Y")
            {
                if (CPO.Check_Except_V352_AGE(clsPmpaType.TOM.Pano, strDiease1) == true  && clsPmpaType.TOM.Age < 6 )
                {
                    strCheck352 = "C";
                }
            }

            //V352 대상중 외부의뢰 제외
            if (strCheck352 == "Y")
            {
                if (CPO.Check_Except_V352_Return(clsPmpaType.TOM.Pano, clsPmpaType.TOM.DeptCode, clsPmpaType.TOM.BDate) == true)
                {
                    strCheck352 = "C";
                }

            }



            //당일 해당과의 원외처방을 체크해서 있으면 SEQNO 업데이트
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SlipNo, Change, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SlipDate  = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
            {
                nSeqNo = Convert.ToInt32(DtS.Rows[0]["Change"].ToString()) + 1;

                for (int i = 0; i < DtS.Rows.Count; i++)
                {
                    strRowID = DtS.Rows[0]["ROWID"].ToString();
                    OutDrug_Change_SeqNo(pDbCon, nSeqNo, strCheck252, strRowID, strDiease1, strDiease1_RO, strDiease2, strDiease2_RO, strCheck352);
                }
            }

            DtS.Dispose();
            DtS = null;

            strRowID = "";

            //해당과의 당일 원외처방전중 인쇄하지 않은 처방전이 있으면 합산
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SlipNo, Change, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SlipDate  = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND Flag      = '0' ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count == 1)
            {
                clsPmpaPb.GnOutDrugNo = Convert.ToInt32(VB.Val(DtS.Rows[0]["SlipNo"].ToString()));
                nSeqNo = Convert.ToInt32(VB.Val(DtS.Rows[0]["Change"].ToString())) + 1;
                strRowID = DtS.Rows[0]["ROWID"].ToString();
                OutDrug_Change_SeqNo(pDbCon, nSeqNo, strCheck252, strRowID, strDiease1, strDiease1_RO, strDiease2, strDiease2_RO, strCheck352);
                rtnVal = OutDrug_Detail_Insert(pDbCon, ArgPtno, ArgDept, ArgBdate, ArgSeqNo);
            }
            else
            {
                switch (clsPmpaType.TOM.Bi)
                {
                    case "21":
                    case "22":
                        if (clsPmpaPb.GnOutDrugNo == 0)
                            rtnVal = OutDrug_Select_SeqNo(pDbCon, ArgBdate);
                        break;

                    default:
                        clsPmpaPb.GnOutDrugNo = 0;
                        rtnVal = OutDrug_Select_SeqNo(pDbCon, ArgBdate);
                        break;
                }

                rtnVal = OutDrug_Master_Insert(pDbCon, ArgPtno, ArgDept, ArgBdate, ArgSeqNo, nSeqNo, strDiease1, strDiease2, strDiease1_RO, strDiease2_RO, strCheck252, strCheck352);
                OutDrug_Detail_Insert(pDbCon, ArgPtno, ArgDept, ArgBdate, ArgSeqNo);
            }

            rtnVal = OutDrug_Etc_Tuyak(pDbCon, ArgBdate); //약국전광판 Data에 INSERT

            rtnVal = "OK";

            DtS.Dispose();
            DtS = null;

            return rtnVal;
        }

        /// <summary>
        /// 약국전광판 Data에 INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgBdate"></param>
        /// <returns></returns>
        private string OutDrug_Etc_Tuyak(PsmhDb pDbCon, string ArgBdate)
        {
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string rtnVal = "";

            if (ArgBdate != clsPublic.GstrSysDate)
                clsPmpaPb.GnOutTuyakNo = Read_Drug_Max_Yakno(ArgBdate);
            else
                clsPmpaPb.GnOutTuyakNo = CPF.Read_Drug_YakNo();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_TUYAK ";
                SQL += ComNum.VBLF + "        (TuDate, TuNo, Flag, ";
                SQL += ComNum.VBLF + "         Pano, Sname, SunapTime,";
                SQL += ComNum.VBLF + "         DeptCode, DrCode, TuIlsu, ";
                SQL += ComNum.VBLF + "         TuGubun, SlipGbn, PRTBUN, ";
                SQL += ComNum.VBLF + "         WorkGbn) ";
                SQL += ComNum.VBLF + "  SELECT TO_DATE('" + ArgBdate + "','YYYY-MM-DD') , ";
                SQL += ComNum.VBLF + "          " + clsPmpaPb.GnOutTuyakNo + ",";
                SQL += ComNum.VBLF + "         '0', ";
                SQL += ComNum.VBLF + "         a.Pano, ";
                SQL += ComNum.VBLF + "         b.Sname, ";
                SQL += ComNum.VBLF + "         SYSDATE,";
                SQL += ComNum.VBLF + "         a.DeptCode, ";
                SQL += ComNum.VBLF + "         a.DrCode, ";
                SQL += ComNum.VBLF + "          " + clsPmpaPb.GnOutDrugNo + ", ";
                SQL += ComNum.VBLF + "         '1', ";
                SQL += ComNum.VBLF + "         '3', ";
                SQL += ComNum.VBLF + "         '" + VB.Left(clsPmpaPb.GstrPrtBun.Trim(), 1) + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrDrugJobGb + "' ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_Patient b ";
                SQL += ComNum.VBLF + "   WHERE 1            = 1 ";
                SQL += ComNum.VBLF + "     AND a.SlipDate   = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND a.SlipNo     = " + clsPmpaPb.GnOutDrugNo + " ";
                SQL += ComNum.VBLF + "     AND a.Pano       = b.Pano(+) ";
                SQL += ComNum.VBLF + "     AND ROWNUM       <= 1 ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("약국전광판(ETC_TUYAK) Insert Error !" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    rtnVal = "NO";
                    return rtnVal;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = "NO";
                return rtnVal;
            }

            return rtnVal;
        }

        /// <summary>
        /// 원외처방전 전광판표시용 약번호 -5000번 보다 작은 최종번호
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgBdate"></param>
        /// <returns></returns>
        /// <seealso cref="READ_DRUG_MAX_YAKNO"/>
        public int Read_Drug_Max_Yakno(string ArgBdate)
        {
            DataTable DtS = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MAX(TuNo) MaxTuNo ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_TUYAK ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND TuDate    = TO_DATE( '" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND TuNo      < 5000 ";
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
                rtnVal = Convert.ToInt32(DtS.Rows[0]["MaxTuNo"].ToString()) + 1;

            DtS.Dispose();
            DtS = null;

            return rtnVal;
        }

        /// <summary>
        /// 원외처방전 마스타 신규 INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgSeqNo"></param>
        /// <param name="ArgnSeqNo"></param>
        /// <param name="ArgDiease1"></param>
        /// <param name="ArgDiease2"></param>
        /// <param name="ArgDiease1_RO"></param>
        /// <param name="ArgDiease2_RO"></param>
        /// <param name="Arg252"></param>
        /// <returns></returns>
        private string OutDrug_Master_Insert(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgBdate, int ArgSeqNo, int ArgnSeqNo, string ArgDiease1, string ArgDiease2, string ArgDiease1_RO, string ArgDiease2_RO, string Arg252, string Arg352)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string rtnVal = "";

            string strPrtBun = VB.Left(clsPmpaPb.GstrPrtBun, 1);

            if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "1") { strPrtBun = "6"; }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                SQL += ComNum.VBLF + "        (SlipDate, SlipNo, Pano, ";
                SQL += ComNum.VBLF + "         BDate, DeptCode, DrCode, ";
                SQL += ComNum.VBLF + "         Bi, Flag, PrtDept, ";
                SQL += ComNum.VBLF + "         ActDate, Part, SeqNo, ";
                SQL += ComNum.VBLF + "         EntDate, WRTNO, DrBunho,";
                SQL += ComNum.VBLF + "         IpdOpd, PrtBun, Change, ";
                SQL += ComNum.VBLF + "         DIEASE1, DIEASE2, DIEASE1_RO, ";
                SQL += ComNum.VBLF + "         DIEASE2_RO, GBV252,GBV352, DOCCODE) ";
                SQL += ComNum.VBLF + "  SELECT TO_DATE('" + ArgBdate + "','YYYY-MM-DD') , ";
                SQL += ComNum.VBLF + "          " + clsPmpaPb.GnOutDrugNo + ", ";
                SQL += ComNum.VBLF + "         Pano, ";
                SQL += ComNum.VBLF + "         BDate, ";
                SQL += ComNum.VBLF + "         DeptCode,";
                SQL += ComNum.VBLF + "         DrCode, ";
                SQL += ComNum.VBLF + "         Bi, ";
                SQL += ComNum.VBLF + "         '0', ";
                SQL += ComNum.VBLF + "         ' ', ";
                SQL += ComNum.VBLF + "         ActDate, ";
                SQL += ComNum.VBLF + "         Part, ";
                SQL += ComNum.VBLF + "         SeqNo, ";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         'O', ";
                SQL += ComNum.VBLF + "         '" + strPrtBun + "', ";
                SQL += ComNum.VBLF + "          " + ArgnSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + ArgDiease1 + "', ";
                SQL += ComNum.VBLF + "         '" + ArgDiease2 + "', ";
                SQL += ComNum.VBLF + "         '" + ArgDiease1_RO + "', ";
                SQL += ComNum.VBLF + "         '" + ArgDiease2_RO + "', ";
                SQL += ComNum.VBLF + "         '" + Arg252 + "', ";
                SQL += ComNum.VBLF + "         '" + Arg352 + "', ";
                SQL += ComNum.VBLF + "         DrSabun  ";
                SQL += ComNum.VBLF + "    FROM OPD_SLIP ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND Pano     = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "     AND DeptCode = '" + ArgDept + "' ";
                SQL += ComNum.VBLF + "     AND ActDate  = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND Part     = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "     AND SeqNo    = " + ArgSeqNo + " ";
                SQL += ComNum.VBLF + "     AND ROWNUM   <= 1 ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("원외처방전 마스타 신규 INSERT 오류" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "NO";
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            return rtnVal;
        }

        /// <summary>
        /// 원외처방전 신규번호를 부여
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgBdate"></param>
        /// <returns></returns>
        private string OutDrug_Select_SeqNo(PsmhDb pDbCon, string ArgBdate)
        {
            DataTable DtS = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            SQL = "";

            if (ArgBdate != clsPublic.GstrSysDate)
            {
                SQL += ComNum.VBLF + " SELECT MAX(SLIPNO) + 1 OUTDRUGNO  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                SQL += ComNum.VBLF + "  WHERE BDATE = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            }
            else
            {
                SQL += ComNum.VBLF + " SELECT SEQ_OUTDRUG.NEXTVAL OutDrugNo FROM DUAL ";
            }
            SqlErr = clsDB.GetDataTableEx(ref DtS, SQL, pDbCon);

            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                DtS.Dispose();
                DtS = null;
                return rtnVal;
            }

            if (DtS.Rows.Count > 0)
                clsPmpaPb.GnOutDrugNo = Convert.ToInt32(DtS.Rows[0]["OutDrugNo"].ToString());
            else
            {
                clsPmpaPb.GnOutDrugNo = 0;
                ComFunc.MsgBox("원외처방전 신규번호 부여시 오류가 발생");
                rtnVal = "NO";
                clsDB.setRollbackTran(pDbCon);
                Application.Exit();
            }

            DtS.Dispose();
            DtS = null;

            return rtnVal;
        }

        /// <summary>
        /// 원외처방전 내역을 INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgSeqNo"></param>
        private string OutDrug_Detail_Insert(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgBdate, int ArgSeqNo)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OUTDRUG ";
                SQL += ComNum.VBLF + "        (SlipDate, SlipNo, Pano, ";
                SQL += ComNum.VBLF + "         DeptCode, Bun, SuCode, ";
                SQL += ComNum.VBLF + "         Qty, Nal, DosCode, ";
                SQL += ComNum.VBLF + "         OrderNo, gbself, MULTI, ";
                SQL += ComNum.VBLF + "         MULTIREMARK, DUR, SCODESAYU,GBSUGBS, ";
                SQL += ComNum.VBLF + "         SCODEREMARK) ";
                SQL += ComNum.VBLF + "  SELECT TO_DATE('" + ArgBdate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "          " + clsPmpaPb.GnOutDrugNo + ", ";
                SQL += ComNum.VBLF + "         Pano, ";
                SQL += ComNum.VBLF + "         DeptCode, ";
                SQL += ComNum.VBLF + "         Bun, ";
                SQL += ComNum.VBLF + "         SuNext, ";
                SQL += ComNum.VBLF + "         Qty, ";
                SQL += ComNum.VBLF + "         Nal, ";
                SQL += ComNum.VBLF + "         DosCode, ";
                SQL += ComNum.VBLF + "         OrderNo, ";
                SQL += ComNum.VBLF + "         gbself, ";
                SQL += ComNum.VBLF + "         MULTI, ";
                SQL += ComNum.VBLF + "         MULTIREMARK, ";
                SQL += ComNum.VBLF + "         DUR, ";
                SQL += ComNum.VBLF + "         SCODESAYU, ";
                SQL += ComNum.VBLF + "         GBSUGBS, ";
                SQL += ComNum.VBLF + "         SCODEREMARK ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND Pano     = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "     AND DeptCode = '" + ArgDept + "'  ";
                SQL += ComNum.VBLF + "     AND ActDate  = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND Part     = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "     AND SeqNo    = " + ArgSeqNo + " ";
                SQL += ComNum.VBLF + "     AND GbBunup  = '0' ";
                SQL += ComNum.VBLF + "     AND BUN IN ('11','12','20') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("원외처방 발행용 DATA INSERT Error !!" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "NO";
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            return rtnVal;
        }


        /// <summary>
        /// 원외처방전 마스터에 변경되면 시퀀스 증가
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSeqNo"></param>
        /// <param name="Arg252"></param>
        /// <param name="ArgRowID"></param>
        /// <param name="ArgDiease1"></param>
        /// <param name="ArgDiease1_RO"></param>
        /// <param name="ArgDiease2"></param>
        /// <param name="ArgDiease2_RO"></param>
        private void OutDrug_Change_SeqNo(PsmhDb pDbCon, int ArgSeqNo, string Arg252, string ArgRowID, string ArgDiease1, string ArgDiease1_RO, string ArgDiease2, string ArgDiease2_RO, string Arg352)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (ArgRowID != "")
            {
                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                    SQL += ComNum.VBLF + "    SET Change        = " + ArgSeqNo + ", ";

                    if (clsPmpaPb.GnOutDrugNo > 0)
                        SQL += ComNum.VBLF + "    Bi            = '" + clsPmpaType.TOM.Bi + "', ";

                    SQL += ComNum.VBLF + "        GBV252        = '" + Arg252 + "', ";
                    SQL += ComNum.VBLF + "        GBV352        = '" + Arg352 + "', ";
                    SQL += ComNum.VBLF + "        DIEASE1       = '" + ArgDiease1 + "', ";
                    SQL += ComNum.VBLF + "        DIEASE1_RO    = '" + ArgDiease1_RO + "', ";
                    SQL += ComNum.VBLF + "        DIEASE2       = '" + ArgDiease2 + "', ";
                    SQL += ComNum.VBLF + "        DIEASE2_RO    = '" + ArgDiease2_RO + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + ArgRowID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("원외처방 MST 변경시퀀스 업데이트 Error !!" + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
        }

        /// <summary>
        /// OCS오더가 아닌 검사처방 전달
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSeqNo"></param>
        /// <seealso cref="oumsad.bas : EXAM_ORDER_INSERT"/>
        public bool Exam_Order_Insert(PsmhDb pDbCon, int ArgSeqNo)
        {
            DataTable DtO = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;
            string strSysTime = "";
            double nQty = 0;

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaType.TOM.DeptCode != "PD")
                {
                    rtnVal = true;
                    return rtnVal;
                }
            }

            ComFunc.ReadSysDate(pDbCon);
            strSysTime = VB.Left(clsPublic.GstrSysTime, 2) + VB.Right(clsPublic.GstrSysTime, 2);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, a.Pano, a.Bi, ";
                SQL += ComNum.VBLF + "        b.SName, b.Age, b.Sex, ";
                SQL += ComNum.VBLF + "        a.DeptCode, a.DrCode, a.SuCode,";
                SQL += ComNum.VBLF + "        (DECODE(a.Qty,0.5,1,0.6,1,0.7,1,0.8,1,a.Qty) * a.Nal) Qty, ";
                SQL += ComNum.VBLF + "        OrderNo ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "OPD_MASTER b ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND a.ActDate     = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.Part        = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "    AND a.SeqNo       =  " + ArgSeqNo + " ";
                SQL += ComNum.VBLF + "    AND a.DeptCode    <> 'ER' ";
                SQL += ComNum.VBLF + "    AND a.OrderNo     = 0  "; // OCS오더가 아닌것
                SQL += ComNum.VBLF + "    AND ((a.Bun >= '52' AND a.Bun <= '64') OR a.Bun = '83' ";
                SQL += ComNum.VBLF + "     OR a.Bun = '37' OR (a.Bun = '41' AND a.SuCode LIKE 'D16%') ";
                SQL += ComNum.VBLF + "     OR (a.Bun = '41' AND a.SuCode LIKE 'C%')) ";
                SQL += ComNum.VBLF + "    AND a.SuCode NOT IN (SELECT YName ";
                SQL += ComNum.VBLF + "                           FROM " + ComNum.DB_MED + "EXAM_SPECODE ";
                SQL += ComNum.VBLF + "                          WHERE Gubun = '51') ";
                SQL += ComNum.VBLF + "    AND a.GbHost IN ('0','1') ";
                SQL += ComNum.VBLF + "    AND a.ActDate     = b.ActDate ";
                SQL += ComNum.VBLF + "    AND a.Pano        = b.Pano ";
                SQL += ComNum.VBLF + "    AND a.DeptCode    = b.DeptCode ";
                SqlErr = clsDB.GetDataTableEx(ref DtO, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtO.Dispose();
                    DtO = null;
                    return rtnVal;
                }

                if (DtO.Rows.Count > 0)
                {
                    for (int i = 0; i < DtO.Rows.Count; i++)
                    {
                        nQty = Convert.ToDouble(DtO.Rows[i]["QTY"].ToString());
                        int k = VB.Fix((int)nQty);

                        if (k < 0) { k = k * -1; }

                        for (int j = 1; j <= k; j++)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "EXAM_ORDER ";
                            SQL += ComNum.VBLF + "        (IpdOpd, BDate, ActDate, ";
                            SQL += ComNum.VBLF + "         Pano, Bi, Sname, ";
                            SQL += ComNum.VBLF + "         Age, Sex, DeptCode, ";
                            SQL += ComNum.VBLF + "         DrCode, MasterCode, Qty,";
                            SQL += ComNum.VBLF + "         STRT, Cancel, OrderNo) ";
                            SQL += ComNum.VBLF + " VALUES ('O', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + DtO.Rows[i]["BDATE"].ToString() + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["PANO"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["BI"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "          " + Convert.ToInt32(DtO.Rows[i]["AGE"].ToString()) + ", ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SEX"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DEPTCODE"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DRCODE"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SUCODE"].ToString().Trim() + "',";

                            if (nQty < 0) //취소처방이면
                                SQL += ComNum.VBLF + "        -1, 'R', ' ', " + strSysTime + ") ";
                            else
                                SQL += ComNum.VBLF + "        1, 'R', ' ', " + strSysTime + ") ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("EXAM_ORDER INSERT Error !!" + SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                DtO.Dispose();
                DtO = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception e)
            {
                ComFunc.MsgBox(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// 임상심리검사 오더 INSERT
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSeqNo"></param>
        /// <returns></returns>
        /// <seealso cref="oumsad.bas : SIMLI_ORDER_INSERT"/>
        public bool Simli_Order_Insert(PsmhDb pDbCon, int ArgSeqNo)
        {
            DataTable DtO = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;
            string strSysTime = "";

            ComFunc.ReadSysDate(pDbCon);
            strSysTime = VB.Left(clsPublic.GstrSysTime, 2) + VB.Right(clsPublic.GstrSysTime, 2);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Pano, b.SName, TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, ";
                SQL += ComNum.VBLF + "        b.Age, b.Sex, a.DeptCode, ";
                SQL += ComNum.VBLF + "        a.DrCode, a.SuNext, a.Qty * a.Nal Qty ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "OPD_MASTER b ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND a.ActDate     = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.Part        = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "    AND a.SeqNo       =  " + ArgSeqNo + " ";
                SQL += ComNum.VBLF + "    AND a.SuNext IN (SELECT SuNext FROM BAS_SUN WHERE GbSimli = 'Y') ";
                SQL += ComNum.VBLF + "    AND a.BDate       = b.BDate ";
                SQL += ComNum.VBLF + "    AND a.Pano        = b.Pano ";
                SQL += ComNum.VBLF + "    AND a.DeptCode    = b.DeptCode ";
                SqlErr = clsDB.GetDataTableEx(ref DtO, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtO.Dispose();
                    DtO = null;
                    return rtnVal;
                }

                if (DtO.Rows.Count == 0)
                {
                    DtO.Dispose();
                    DtO = null;
                    return rtnVal;
                }

                for (int i = 0; i < DtO.Rows.Count; i++)
                {
                    string strPtno = DtO.Rows[i]["PANO"].ToString().Trim();
                    string strBdate = DtO.Rows[i]["BDATE"].ToString().Trim();
                    string strSunext = DtO.Rows[i]["SUNEXT"].ToString().Trim();
                    double nQty = Convert.ToDouble(DtO.Rows[i]["QTY"].ToString());

                    if (nQty > 0)
                    {
                        //취소된 오더가 있는지 Check
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID FROM SIMRI_ORDER ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SuNext    = '" + strSunext + "' ";
                        SQL += ComNum.VBLF + "    AND Qty       = 0 ";
                        SqlErr = clsDB.GetDataTableEx(ref DtSub, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtSub.Dispose();
                            DtSub = null;
                            return rtnVal;
                        }

                        if (DtSub.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE SIMRI_ORDER ";
                            SQL += ComNum.VBLF + "    SET Qty   =  " + nQty + " ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + DtSub.Rows[0]["ROWID"].ToString() + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO SIMRI_ORDER ";
                            SQL += ComNum.VBLF + "        (Pano, ActDate, SName, ";
                            SQL += ComNum.VBLF + "         IpdOpd, BDate, Sex, ";
                            SQL += ComNum.VBLF + "         Age, DeptCode, DrCode, ";
                            SQL += ComNum.VBLF + "         SuNext, Qty, SuNapTime, ";
                            SQL += ComNum.VBLF + "         SunapSabun) ";
                            SQL += ComNum.VBLF + " VALUES ('" + strPtno + "', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         'O', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + strBdate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SEX"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "          " + DtO.Rows[i]["AGE"].ToString().Trim() + ", ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + strSunext + "', ";
                            SQL += ComNum.VBLF + "          " + nQty + ", ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ") ";
                        }
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("임상심리 오더에 INSERT시 오류 발생" + SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            DtSub.Dispose();
                            DtSub = null;
                            DtO.Dispose();
                            DtO = null;
                            return rtnVal;
                        }

                        DtSub.Dispose();
                        DtSub = null;
                    }
                    else if (nQty < 0)
                    {
                        //동일한 오더가 있는지 Check
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID FROM SIMRI_ORDER ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SuNext    = '" + strSunext + "' ";
                        SQL += ComNum.VBLF + "    AND Qty       =  " + (nQty * -1) + " ";
                        SQL += ComNum.VBLF + "    AND RESULTTIME IS NULL ";
                        SqlErr = clsDB.GetDataTableEx(ref DtSub, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtSub.Dispose();
                            DtSub = null;
                            return rtnVal;
                        }

                        if (DtSub.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " DELETE SIMRI_ORDER ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + DtSub.Rows[0]["ROWID"].ToString() + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO SIMRI_ORDER ";
                            SQL += ComNum.VBLF + "        (Pano, ActDate, SName, ";
                            SQL += ComNum.VBLF + "         IpdOpd, BDate, Sex, ";
                            SQL += ComNum.VBLF + "         Age, DeptCode, DrCode, ";
                            SQL += ComNum.VBLF + "         SuNext, Qty, SuNapTime, ";
                            SQL += ComNum.VBLF + "         SunapSabun) ";
                            SQL += ComNum.VBLF + " VALUES ('" + strPtno + "', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         'O', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + strBdate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SEX"].ToString() + "', ";
                            SQL += ComNum.VBLF + "          " + Convert.ToInt32(DtO.Rows[i]["AGE"].ToString()) + " ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DEPTCODE"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["DRCODE"].ToString() + "', ";
                            SQL += ComNum.VBLF + "         '" + strSunext + "', ";
                            SQL += ComNum.VBLF + "          " + nQty + " ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ") ";
                        }
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("임상심리 오더를 취소시 오류 발생" + SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            DtSub.Dispose();
                            DtSub = null;
                            DtO.Dispose();
                            DtO = null;
                            return rtnVal;
                        }

                        DtSub.Dispose();
                        DtSub = null;
                    }
                }

                DtO.Dispose();
                DtO = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// 수술실에서 입력한 선수납 물품을 관리과에 전달
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <seealso cref="oumsad.bas : OCS_GUMESEND_INSERT"/>
        public bool OCS_GumeSend_Insert(PsmhDb pDbCon, string ArgPtno, string ArgSname)
        {
            DataTable DtO = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;

            const string BUSE_SUSUL = "033102"; //수술실
            const string BUSE_MARCH = "033103"; //마취통증의학과
            const string BUSE_ANGIO = "100570"; //ANGIO

            //수술 / 마취 선수납 읽은 건수가 없으면 처리를 안함
            if (clsPmpaPb.GnOrAnSlipCnt == 0)
            {
                rtnVal = true;
                return rtnVal;
            }

            try
            {
                //처방전 교환물품을 OCS_GUMESEND에 INSERT
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.WRTNO, a.DeptCode, a.DrCode, ";
                SQL += ComNum.VBLF + "        a.BuCode, a.JepCode, a.SuCode,";
                SQL += ComNum.VBLF + "        b.JepName, b.Buse_Unit, b.Buse_Gesu, ";
                SQL += ComNum.VBLF + "        SUM(a.Qty) Qty ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ORAN_SLIP a, ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_ERP + "ORD_JEP b ";
                SQL += ComNum.VBLF + " WHERE 1              = 1 ";
                SQL += ComNum.VBLF + "   AND a.Pano         = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "   AND a.OpDate       >= TRUNC(SYSDATE-4) ";
                SQL += ComNum.VBLF + "   AND a.CodeGbn      = '2' "; //관리과물품
                SQL += ComNum.VBLF + "   AND a.GbSunap      = '1' "; //선수납
                SQL += ComNum.VBLF + "   AND SlipSend IS NULL "; //미전송
                SQL += ComNum.VBLF + "   AND a.JepCode      = b.JepCode(+) ";
                SQL += ComNum.VBLF + "   AND b.GbExchange   = '1' "; //처방전교환물품
                SQL += ComNum.VBLF + "   AND (b.GbReUse IS NULL OR b.GbReUse<>'Y') "; //소독후 재 사용물품은 제외
                SQL += ComNum.VBLF + " GROUP BY a.WRTNO, a.DeptCode, a.DrCode, ";
                SQL += ComNum.VBLF + "          a.BuCode, a.JepCode, a.SuCode, ";
                SQL += ComNum.VBLF + "          b.JepName, b.Buse_Unit, b.Buse_Gesu ";
                SQL += ComNum.VBLF + " ORDER BY a.WRTNO, a.DeptCode, a.DrCode, ";
                SQL += ComNum.VBLF + "          a.BuCode, a.JepCode, b.JepName ";
                SqlErr = clsDB.GetDataTableEx(ref DtO, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtO.Dispose();
                    DtO = null;
                    return rtnVal;
                }

                for (int i = 0; i < DtO.Rows.Count; i++)
                {
                    double nQty = Convert.ToDouble(DtO.Rows[i]["QTY"].ToString());

                    if (nQty != 0)
                    {
                        double nJepQty = nQty;
                        long nWrtno = Convert.ToInt64(DtO.Rows[i]["WRTNO"].ToString());
                        string strDeptcode = DtO.Rows[i]["DEPTCODE"].ToString().Trim();
                        string strDrcode = DtO.Rows[i]["DrCode"].ToString().Trim();
                        string strBucode = DtO.Rows[i]["BuCode"].ToString().Trim();
                        string strSucode = DtO.Rows[i]["SuCode"].ToString().Trim();
                        string strJepCode = DtO.Rows[i]["JepCode"].ToString().Trim();
                        string strJepName = DtO.Rows[i]["JepName"].ToString().Trim();
                        double nBuseGesu = Convert.ToDouble(DtO.Rows[i]["Buse_Gesu"].ToString());

                        if (nBuseGesu == 0) { nBuseGesu = 1; }

                        nJepQty = nQty / nBuseGesu;

                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_GUMESEND ";
                        SQL += ComNum.VBLF + "        (BDate, GbIO, GbBuse, ";
                        SQL += ComNum.VBLF + "         Pano, SName, WardCode, ";
                        SQL += ComNum.VBLF + "         OrderCode, OrderName, SuCode, ";
                        SQL += ComNum.VBLF + "         Qty, GbInfo, OrderNo, ";
                        SQL += ComNum.VBLF + "         CDate, EntDate, EntSabun, ";
                        SQL += ComNum.VBLF + "         WriteDate, DeptCode, DrCode, ";
                        SQL += ComNum.VBLF + "         JepCode) ";
                        SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                        SQL += ComNum.VBLF + "         'I', ";
                        SQL += ComNum.VBLF + "         '1', ";
                        SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                        SQL += ComNum.VBLF + "         '" + ArgSname + "', ";

                        if (strBucode == BUSE_SUSUL)
                            SQL += ComNum.VBLF + "     'OR', ";
                        else if (strBucode == BUSE_ANGIO)
                            SQL += ComNum.VBLF + "     'AG', ";
                        else if (strBucode == BUSE_MARCH)
                            SQL += ComNum.VBLF + "     'AN', ";

                        SQL += ComNum.VBLF + "         '" + strJepCode + "', ";
                        SQL += ComNum.VBLF + "         '" + strJepName + "', ";
                        SQL += ComNum.VBLF + "         '" + strSucode + "', ";
                        SQL += ComNum.VBLF + "          " + nJepQty + ", ";
                        SQL += ComNum.VBLF + "         '', ";
                        SQL += ComNum.VBLF + "          " + nWrtno + ", ";
                        SQL += ComNum.VBLF + "         '', ";
                        SQL += ComNum.VBLF + "         SYSDATE, ";
                        SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ", ";
                        SQL += ComNum.VBLF + "         SYSDATE, ";
                        SQL += ComNum.VBLF + "         '" + strDeptcode + "', ";
                        SQL += ComNum.VBLF + "         '" + strDrcode + "', ";
                        SQL += ComNum.VBLF + "         '" + strJepCode + "') ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("관리과 전달 중 오류 발생" + SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            DtO.Dispose();
                            DtO = null;
                            return rtnVal;
                        }
                    }
                }

                DtO.Dispose();
                DtO = null;

                //수술실 처방오더에 수납완료 SET
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ORAN_SLIP ";
                SQL += ComNum.VBLF + "    SET SlipSend  = SYSDATE   ";  //전송완료
                SQL += ComNum.VBLF + "  WHERE Pano      = '" + ArgPtno + "'";
                SQL += ComNum.VBLF + "    AND OpDate    >= TRUNC(SYSDATE-4) ";
                SQL += ComNum.VBLF + "    AND GbSunap   = '1' "; //선수납
                SQL += ComNum.VBLF + "    AND SlipSend IS NULL ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("ORAN_SLIP  UPDATE 오류 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// 수술실 외래 오더에 수납완료 처리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="oumsad.bas : ORAN_SLIP_SendTime_Update"/>
        public bool Oran_Slip_SendTime_Update(PsmhDb pDbCon, bool ArgRead, string ArgPtno, string ArgDept, string ArgDr)
        {
            DataTable DtO = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;

            if (ArgRead == false)
            {
                rtnVal = true;
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.JepCode, b.JepName, b.Buse_Unit, ";
                SQL += ComNum.VBLF + "        b.Buse_Gesu, c.GbCSR, a.BuCode, ";
                SQL += ComNum.VBLF + "        a.SuCode, a.WRTNO, SUM(a.Qty) Qty ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "ORD_JEP b, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "OPR_BUSEJEPUM c ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND a.Pano        = '" + ArgPtno + "'";
                SQL += ComNum.VBLF + "    AND a.OpDate      >= TO_DATE('" + clsPublic.GstrBdate + "','YYYY-MM-DD') ";

                if (clsPmpaPb.GstrErJobFlag != "OK")
                    SQL += ComNum.VBLF + "AND a.DeptCode    = '" + ArgDept + "'";

                SQL += ComNum.VBLF + "    AND a.IpdOpd      = 'O' "; //외래수술
                SQL += ComNum.VBLF + "    AND a.CodeGbn     = '2' "; // 관리과물품
                SQL += ComNum.VBLF + "    AND a.SuCode IS NOT NULL ";
                SQL += ComNum.VBLF + "    AND a.SlipSend IS NULL "; //미전송
                SQL += ComNum.VBLF + "    AND (b.GbReUse IS NULL OR b.GbReUse <> 'Y') "; // 소독후 재 사용물품은 제외
                SQL += ComNum.VBLF + "    AND (b.GbExchange = '1' OR c.GbCSR = 'Y') "; // 처방전교환물품
                SQL += ComNum.VBLF + "    AND a.JepCode     = b.JepCode(+) ";
                SQL += ComNum.VBLF + "    AND a.JepCode     = c.JepCode(+) ";
                SQL += ComNum.VBLF + "    AND a.BuCode      = c.BuCode(+) ";
                SQL += ComNum.VBLF + "  GROUP BY a.JepCode, b.JepName, b.Buse_Unit, ";
                SQL += ComNum.VBLF + "           b.Buse_Gesu, c.GbCSR, a.BuCode,";
                SQL += ComNum.VBLF + "           a.SuCode, a.WRTNO ";
                SQL += ComNum.VBLF + "  ORDER BY a.JepCode, b.JepName ";
                SqlErr = clsDB.GetDataTableEx(ref DtO, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtO.Dispose();
                    DtO = null;
                    return rtnVal;
                }

                if (DtO.Rows.Count > 0)
                {
                    for (int i = 0; i < DtO.Rows.Count; i++)
                    {
                        double nQty = Convert.ToDouble(DtO.Rows[i]["QTY"].ToString());

                        if (nQty != 0)
                        {
                            double nJepQty = nQty;
                            double nBuseGesu = Convert.ToDouble(DtO.Rows[i]["Buse_Gesu"].ToString());
                            if (nBuseGesu == 0) { nBuseGesu = 1; }

                            string strGbBuse = "1";
                            if (DtO.Rows[i]["GBCSR"].ToString() == "Y") { strGbBuse = "2"; } //공급실

                            if (strGbBuse == "1")
                                nJepQty = nQty / nBuseGesu;

                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_GUMESEND ";
                            SQL += ComNum.VBLF + "        (BDate, GbIO, GbBuse, ";
                            SQL += ComNum.VBLF + "         Pano, SName, WardCode, ";
                            SQL += ComNum.VBLF + "         OrderCode, OrderName, SuCode, ";
                            SQL += ComNum.VBLF + "         Qty, GbInfo, OrderNo, ";
                            SQL += ComNum.VBLF + "         CDate, EntDate, EntSabun, ";
                            SQL += ComNum.VBLF + "         WriteDate, DeptCode, DrCode, ";
                            SQL += ComNum.VBLF + "         JepCode) ";
                            SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                            SQL += ComNum.VBLF + "         'I', ";
                            SQL += ComNum.VBLF + "         '" + strGbBuse + "', ";
                            SQL += ComNum.VBLF + "         '" + ArgPtno.Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + clsPmpaType.TBP.Sname.Trim() + "', ";

                            if (DtO.Rows[i]["BUCODE"].ToString().Trim() == "033102")
                                SQL += ComNum.VBLF + "     'OR', ";
                            else if (DtO.Rows[i]["BUCODE"].ToString().Trim() == "100570")
                                SQL += ComNum.VBLF + "     'AG', ";
                            else
                                SQL += ComNum.VBLF + "     'AN', ";

                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["JEPCODE"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["JEPNAME"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["SUCODE"].ToString().Trim() + "', ";
                            SQL += ComNum.VBLF + "          " + nJepQty + ", ";
                            SQL += ComNum.VBLF + "         '', ";
                            SQL += ComNum.VBLF + "          " + Convert.ToInt64(DtO.Rows[i]["WRTNO"].ToString()) + ", ";
                            SQL += ComNum.VBLF + "         '', ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "          " + clsType.User.IdNumber + ", ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "         '" + ArgDept + "', ";
                            SQL += ComNum.VBLF + "         '" + ArgDr + "', ";
                            SQL += ComNum.VBLF + "         '" + DtO.Rows[i]["JEPCODE"].ToString().Trim() + "') ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("관리과 전달 중 오류 발생" + SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                Cursor.Current = Cursors.Default;
                                DtO.Dispose();
                                DtO = null;
                                return rtnVal;
                            }
                        }
                    }
                }

                DtO.Dispose();
                DtO = null;

                //수술실 처방오더에 수납완료 SET
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ORAN_SLIP ";
                SQL += ComNum.VBLF + "    SET SlipSend  = SYSDATE   ";  //전송완료
                SQL += ComNum.VBLF + "  WHERE Pano      = '" + ArgPtno + "'";

                if (clsPmpaPb.GstrErJobFlag != "OK")
                    SQL += ComNum.VBLF + "AND DEPTCODE  = '" + ArgDept + "' ";

                SQL += ComNum.VBLF + "    AND OpDate    >= TO_DATE('" + clsPublic.GstrBdate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND IPDOPD    = 'O' "; //외래수술
                SQL += ComNum.VBLF + "    AND SUCODE IS NOT NULL ";
                SQL += ComNum.VBLF + "    AND SlipSend IS NULL ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("ORAN_SLIP  UPDATE 오류 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }


        }

        /// <summary>
        /// 예약검사 마스터 테이블
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="nResvExamNo"></param>
        /// <seealso cref="oumsad.bas : OPD_EXAM_RESV_INSERT"/>
        public bool Opd_Exam_Resv_Insert(PsmhDb pDbCon, long nResvExamNo)
        {
            clsOumsadChk COC = new clsOumsadChk();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            bool rtnVal = false;

            string strRemark = "번호:" + nResvExamNo.ToString();
            int nSeqNo = 0;
            string strGubun = COC.Check_Yeyak_Exam_Bun();

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "        (ACTDATE, OpdNo, PANO, AMT, ";
            SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
            SQL += ComNum.VBLF + "         BIGO, REMARK, SEQNO2, ";
            SQL += ComNum.VBLF + "         DEPTCODE, BI, GBSPC) ";
            SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "          " + clsPmpaType.TOM.OpdNo + ", ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Pano + "', ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[5] + ", ";
            SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
            SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
            SQL += ComNum.VBLF + "         '" + strRemark + "', ";
            SQL += ComNum.VBLF + "         '예약검사' , ";
            SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DeptCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bi + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbSpc + "' ) ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox("예약검사 마스터 테이블  INSERT 오류 발생" + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
            SQL += ComNum.VBLF + "        (PANO, DEPTCODE, BI,              --등록번호,진료과,보험종류";
            SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE,          --수진자명,의사코드,회계일자";
            SQL += ComNum.VBLF + "         BDATE,GBGAMEK, GBSPC,            --처방일자,감액코드,특진여부";
            SQL += ComNum.VBLF + "         JIN, AMT1, AMT2,                 --접수구분,진료비총액,선택진료비";
            SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5,                --급여총액,비급여총액,감액";
            SQL += ComNum.VBLF + "         AMT6, PART, BOHUN,               --영수금액,작업조,보훈여부";
            SQL += ComNum.VBLF + "         GELCODE, MCODE, VCODE,           --계약처코드,특정코드,중증코드";
            SQL += ComNum.VBLF + "         REMARK, WRTNO, MANUAL,           --참고사항,고유번호,수동처리";
            SQL += ComNum.VBLF + "         ENTDATE, Gubun, GBEND,           --작업시간,구분,종료여부";
            SQL += ComNum.VBLF + "         Date1)";
            SQL += ComNum.VBLF + " Values ('" + clsPmpaType.TOM.Pano + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DeptCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bi + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.sName + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DrCode + "', ";
            SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrActDate + "', 'YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.TOM.BDate + "', 'YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbGameK + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbSpc + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Jin + "' , ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[0] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[1] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[2] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[3] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[4] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[5] + ", ";
            SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bohun + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GelCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.MCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.VCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrResExamRemark + "', ";
            SQL += ComNum.VBLF + "          " + nResvExamNo + ", ";
            SQL += ComNum.VBLF + "         'N', ";
            SQL += ComNum.VBLF + "         SYSDATE , ";
            SQL += ComNum.VBLF + "         '" + strGubun + "', ";

            if ((clsPmpaPb.GnResExamAmt[5] < 0 || clsPmpaPb.GnResExamEnd < 0)  && clsPmpaPb.GnResExamWRTNO > 0)
            {
                SQL += ComNum.VBLF + "     'Y', ";
                SQL += ComNum.VBLF + "     SYSDATE ) ";
            }
            else
            {
                SQL += ComNum.VBLF + "     'N', ";
                SQL += ComNum.VBLF + "     '' ) ";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox("예약검사 마스터 테이블  INSERT 오류 발생" + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            //백업저장
            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM_BACKUP ";
            SQL += ComNum.VBLF + "        (PANO, DEPTCODE, BI, ";
            SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
            SQL += ComNum.VBLF + "         BDATE,GBGAMEK, GBSPC,";
            SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
            SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
            SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
            SQL += ComNum.VBLF + "         GELCODE, MCODE, VCODE, ";
            SQL += ComNum.VBLF + "         REMARK, WRTNO, MANUAL, ";
            SQL += ComNum.VBLF + "         ENTDATE, Gubun, GBEND, ";
            SQL += ComNum.VBLF + "         Date1)";
            SQL += ComNum.VBLF + " Values ('" + clsPmpaType.TOM.Pano + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DeptCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bi + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.sName + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DrCode + "', ";
            SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrActDate + "', 'YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.TOM.BDate + "', 'YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbGameK + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbSpc + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Jin + "' , ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[0] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[1] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[2] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[3] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[4] + ", ";
            SQL += ComNum.VBLF + "          " + clsPmpaPb.GnResExamAmt[5] + ", ";
            SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bohun + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GelCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.MCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.VCode + "', ";
            SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrResExamRemark + " back" + "', ";
            SQL += ComNum.VBLF + "          " + nResvExamNo + ", ";
            SQL += ComNum.VBLF + "         'N', ";
            SQL += ComNum.VBLF + "         SYSDATE , ";
            SQL += ComNum.VBLF + "         '" + strGubun + "', ";

            if ((clsPmpaPb.GnResExamAmt[5] < 0 || clsPmpaPb.GnResExamEnd < 0) && clsPmpaPb.GnResExamWRTNO > 0)
            {
                SQL += ComNum.VBLF + "     'Y', ";
                SQL += ComNum.VBLF + "     SYSDATE ) ";
            }
            else
            {
                SQL += ComNum.VBLF + "     'N', ";
                SQL += ComNum.VBLF + "     '' ) ";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox("예약검사 마스터 테이블 back INSERT 오류 발생" + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            //환불시 마스터 종료갱신
            if (clsPmpaPb.GnResExamAmt[5] < 0 || clsPmpaPb.GnResExamEnd < 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "    SET GbEnd = 'Y', ";
                SQL += ComNum.VBLF + "        Date1 = SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND Pano  = '" + clsPmpaType.TOM.Pano + "' ";
                SQL += ComNum.VBLF + "    AND WRTNO =  " + clsPmpaPb.GnResExamWRTNO + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("환불시 마스터 종료갱신 UPDATE 오류 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_API ";
            SQL += ComNum.VBLF + "        (ActDate, Pano, Part, ";
            SQL += ComNum.VBLF + "         SeqNo, Gubun, GbSend, ";
            SQL += ComNum.VBLF + "         EntDate, WRTNO, RES ) ";
            SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Pano + "', ";
            SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
            SQL += ComNum.VBLF + "         0, ";
            SQL += ComNum.VBLF + "         '+', ";
            SQL += ComNum.VBLF + "         ' ', ";
            SQL += ComNum.VBLF + "         SysDate, ";
            SQL += ComNum.VBLF + "         " + nResvExamNo + ", ";
            SQL += ComNum.VBLF + "         '1' )  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox("OCS_API INSERT 오류 발생" + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            for (int i = 0; i < 999; i++)
            {
                if (string.IsNullOrEmpty(clsPmpaType.SW[i].SuCode) == true) { break; }

                if (clsPmpaType.SW[i].SuCode.Trim() != "" && clsPmpaType.SW[i].SuNext.Trim() != "")
                {
                    if (clsPmpaType.SW[i].OrderNo != 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OORDER ";
                        SQL += ComNum.VBLF + "    SET GbSunap   = '1', ";
                        SQL += ComNum.VBLF + "        WRTNO     = " + nResvExamNo + " ";
                        SQL += ComNum.VBLF + "  WHERE BDate     = TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND Ptno      = '" + clsPmpaType.TOM.Pano + "' ";
                        SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaType.TOM.DeptCode + "' ";
                        SQL += ComNum.VBLF + "    AND SuCode    = '" + clsPmpaType.SW[i].SuCode.Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND OrderNo   = " + clsPmpaType.SW[i].OrderNo + " ";
                        SQL += ComNum.VBLF + "    AND Nal       = " + clsPmpaType.SW[i].Nal + " ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("OCS_OORDER UPDATE 오류 발생" + SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SLIP_RES ";
                    SQL += ComNum.VBLF + "        (ACTDATE, PANO, BI, ";
                    SQL += ComNum.VBLF + "         BDATE, SUNEXT, BUN, ";
                    SQL += ComNum.VBLF + "         NU, QTY, NAL, ";
                    SQL += ComNum.VBLF + "         BASEAMT, GBNGT, GBGISUL, ";
                    SQL += ComNum.VBLF + "         GBSELF, GBCHILD, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         DRCODE, SUCODE, GBHOST, ";
                    SQL += ComNum.VBLF + "         PART, AMT1, AMT2, ";
                    SQL += ComNum.VBLF + "         SEQNO, ORDERNO, GbImiv, ";
                    SQL += ComNum.VBLF + "         GbBunup, DosCode, GbIpd, ";
                    SQL += ComNum.VBLF + "         Div, KsJin) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Pano + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bi + "', ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.TOM.BDate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].SuNext + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].Bun + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].Nu + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].Qty + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].Nal + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].BaseAmt + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbNgt + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbGisul + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbSelf + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbChild + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DeptCode + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DrCode + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].SuCode + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbHost + "', ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.SW[i].Amt1 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.SW[i].Amt2 + ", ";
                    SQL += ComNum.VBLF + "          " + nResvExamNo + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.SW[i].OrderNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbImiv + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbBunup + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].DosCode + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].GbIPD + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].Div + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.SW[i].KsJin + "') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SLIP_RES INSERT 오류 발생" + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
            }

            rtnVal = true;
            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgName"></param>
        /// <param name="ArgAmt"></param>
        /// <seealso cref="oumsad.bas : Customer_Display"/>
        public void Customer_Display_Opd(string ArgName, string ArgAmt)
        {
            if (clsPmpaPb.GstrCreditIF != "T") { return; }

            IntPtr hWnd = FindWindow(null, "금액표시");

            //이미 폼이 떠있는지 확인한다.
            if (!hWnd.Equals(IntPtr.Zero))
            {
                // 떠있는 화면 종료
                SendMessage(hWnd, 0x0010, 0, 0);
            }

            frmTablet_b frm = new frmTablet_b(ArgName, ArgAmt);

            ShowWindow(frm.Handle, WM_SHOWNOACTIVATE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgName"></param>
        /// <param name="ArgAmt"></param>
        /// <seealso cref="oumsad.bas : Customer_Display"/>
        public void Customer_Display(string ArgName, string ArgAmt)
        {
            if (clsPmpaPb.GstrCreditIF == "P") { return; }

            //이미 폼이 떠있는지 확인한다.
            foreach (Form frmFindform in Application.OpenForms)
            {
                if (frmFindform.GetType() == typeof(frmTablet_b))
                {
                    frmFindform.Close();
                    frmFindform.Dispose();
                    break;
                }
            }

            frmTablet_b frm = new frmTablet_b(ArgName, ArgAmt);
            frm.Show();
            
        }

        public void Show_Tablet_A()
        {
            IntPtr hWnd = FindWindow(null, "Tablet_a");

            //이미 폼이 떠있는지 확인한다.
            if (!hWnd.Equals(IntPtr.Zero))
            {
                // 떠있는 화면 종료
                SendMessage(hWnd, 0x0010, 0, 0);

                //// 윈도우가 최소화 되어 있다면 활성화 시킨다
                //ShowWindowAsync(hWnd, SW_SHOWNORMAL);
            }

            frmTablet_a frm = new frmTablet_a();

            ShowWindow(frm.Handle, WM_SHOWNOACTIVATE);

        }

        /// <summary>
        /// Description : 외래 진료비 본인부담율 저장
        /// Author : 김민철
        /// Create Date : 2018.03.12
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strBi"></param>
        /// <param name="strIO"></param>
        /// <param name="strChild"></param>
        /// <param name="strMCode"></param>
        /// <param name="strVCode"></param>
        /// <param name="strOgPdBun"></param>
        /// <param name="strFCode"></param>
        /// <param name="strSDate"></param>
        /// <returns></returns>
        public bool Read_OBon_Rate(PsmhDb pDbCon, clsPmpaType.BonRate cBON)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            string strDeptCode = string.Empty;

            bool rtnVal = false;
            clsBasAcct cBAcct = new clsBasAcct();

            //본인부담율 초기화
            OBon_Rate_Variable_Clear();

            #region 이전 세팅 기준 주석처리 2018-05-03 KMC
            //if (cBON.DEPT != "NP" && cBON.DEPT != "DT")
            //{
            //    strDeptCode = "**";
            //}
            //else
            //{
            //    strDeptCode = cBON.DEPT;
            //}

            ////***나이치환
            ////1.외래는 3.6세이상 15세미만 본인부담율 없음 >> 0.성인으로 치환
            //if (cBON.CHILD == "3")
            //{
            //    cBON.CHILD = "0";
            //}

            ////2.65세이상 본인부담율은 DT의 임플란트 틀니 부담율 때문에 구분하였으므로 DT인 경우 65세이상은 0.성인으로 치환
            //if (cBON.DEPT != "DT")
            //{
            //    if (cBON.CHILD == "4")
            //        cBON.CHILD = "0";
            //}

            ////***보험종별 치환
            ////1.건강보험은 11종으로 치환
            //if (VB.Left(cBON.BI, 1) == "1")
            //    cBON.BI = "11";
            ////2.건강보험100%은 41종으로 치환
            //else if (VB.Left(cBON.BI, 1) == "4")
            //    cBON.BI = "41";

            #endregion

            //부담율 Argument 치환(외래/입원 공통)
            cBAcct.Convert_Rate_Argument(cBON);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT BI,MCODE,GBCHILD,VCODE_NAME,VCODE,JIN,BOHUM,CTMRI,FOOD,GBIO,ENTSABUN,";
                SQL += ComNum.VBLF + "        TO_CHAR(SDATE, 'YYYY-MM-DD') SDate, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') EntDate,";
                SQL += ComNum.VBLF + "        DT1,DT2,TO_CHAR(DELDATE, 'YYYY-MM-DD') DelDate,WRTNO,FAMT1,FAMT2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SQL += ComNum.VBLF + "  WHERE BI = '" + cBON.BI + "' ";
                SQL += ComNum.VBLF + "    AND GBIO = '" + cBON.IO + "' ";
                SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
                SQL += ComNum.VBLF + "    AND SDATE <= TO_DATE('" + cBON.SDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GBCHILD = '" + cBON.CHILD + "' ";
                SQL += ComNum.VBLF + "    AND DEPT = '" + cBON.DEPT + "' ";
                if (cBON.MCODE != "")
                {
                    SQL += ComNum.VBLF + "    AND MCODE = '" + cBON.MCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (MCODE IS NULL OR MCODE = '' OR MCODE = ' ') ";
                }

                if (cBON.VCODE != "")
                {
                    SQL += ComNum.VBLF + "    AND VCODE = '" + cBON.VCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (VCODE IS NULL OR VCODE = '' OR VCODE = ' ') ";
                }

                if (cBON.OGPDBUN != "")
                {
                    SQL += ComNum.VBLF + "    AND OGPDBUN = '" + cBON.OGPDBUN + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (OGPDBUN IS NULL OR OGPDBUN = '' OR OGPDBUN = ' ') ";
                }

                if (cBON.FCODE != "")
                {
                    SQL += ComNum.VBLF + "    AND FCODE = '" + cBON.FCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (FCODE IS NULL OR FCODE = '' OR FCODE = ' ') ";
                }

                SQL += ComNum.VBLF + "  ORDER By SDATE DESC ";


                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.", "전산실 연락요망");
                    return rtnVal;
                }
                else
                {
                    clsPmpaType.OBR.Jin = Convert.ToInt32(Dt.Rows[0]["JIN"].ToString());        //진찰료
                    clsPmpaType.OBR.Bohum = Convert.ToInt32(Dt.Rows[0]["BOHUM"].ToString());    //진료비
                    clsPmpaType.OBR.CTMRI = Convert.ToInt32(Dt.Rows[0]["CTMRI"].ToString());    //CT/MRI
                    clsPmpaType.OBR.Food = Convert.ToInt32(Dt.Rows[0]["FOOD"].ToString());      //식대료
                    clsPmpaType.OBR.Dt1 = Convert.ToInt32(Dt.Rows[0]["DT1"].ToString());        //틀니
                    clsPmpaType.OBR.Dt2 = Convert.ToInt32(Dt.Rows[0]["DT2"].ToString());        //임플란트
                    clsPmpaType.OBR.FAmt1 = Convert.ToInt64(Dt.Rows[0]["FAMT1"].ToString());    //진료비 정액
                    clsPmpaType.OBR.FAmt2 = Convert.ToInt64(Dt.Rows[0]["FAMT2"].ToString());    //진료비 정액(직접조제시)
                    clsPmpaType.OBR.SDate = Dt.Rows[0]["SDATE"].ToString().Trim();              //적용일자
                }

                Dt.Dispose();
                Dt = null;

                cBAcct = null;

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        public void OBon_Rate_Variable_Clear()
        {
            //기본적으로 본인 100% 부담율로 세팅
            clsPmpaType.OBR.Jin = 100;
            clsPmpaType.OBR.Bohum = 100;
            clsPmpaType.OBR.CTMRI = 100;
            clsPmpaType.OBR.Food = 100;
            clsPmpaType.OBR.Dt1 = 100;
            clsPmpaType.OBR.Dt2 = 100;
            clsPmpaType.OBR.FAmt1 = 0;
            clsPmpaType.OBR.FAmt2 = 0;
            clsPmpaType.OBR.SDate = "";
        }
    }
}
