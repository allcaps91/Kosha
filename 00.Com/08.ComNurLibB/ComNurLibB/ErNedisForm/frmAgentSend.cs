using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmAgentSend.cs
    /// Description     : NEDIS응급자료전송
    /// Author          : 유진호
    /// Create Date     : 2018-05-03
    /// <history>       
    /// D:\참고소스\포항성모병원 VB Source(2018.04.01)\nurse\nrer\agentSend
    /// </history>
    /// </summary>
    public partial class frmAgentSend : Form
    {
        int FnTimer = 0;
        string NOTPANO = "";
        //string strInDate = "";
        //string strOutDate = "";
        string strPano = "";
        string strPTMIINDT = "";
        string strPTMIINTM = "";
        string[] strSanCode = new string[10];
        //string strOPPTOPCD = "";
        
        //PsmhDb pEdisAgentDb = null;

        public frmAgentSend()
        {
            InitializeComponent();
        }

        private void frmAgentSend_Load(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddYears(-1);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            timer1.Enabled = true;

            //'응급개별 전송
            //pEdisAgentDb = clsDB.DBConnect(clsEdisAgentDB.strDBIP, clsEdisAgentDB.strDBPORT, clsEdisAgentDB.strSOURCE, clsEdisAgentDB.strUSER, clsEdisAgentDB.strPASSWD);
            //if (pEdisAgentDb == null)
            //{
            //    ComFunc.MsgBox("EDISAGENT DB 접속실패");
                
            //}
            //else
            //{                
            //    timer1.Enabled = true;
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            NOTPANO = "'111'";
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-400);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            if (string.Compare(clsPublic.GstrSysTime, "03:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "06:00") <= 0)
            {
                INSERT_EMERGENCY_AID(); //응급실처치내역
            }            
            if (string.Compare(clsPublic.GstrSysTime, "08:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "08:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "10:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "10:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "12:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "14:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "14:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "16:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "16:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "18:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "18:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "20:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "20:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }
            else if (string.Compare(clsPublic.GstrSysTime, "22:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "22:30") <= 0)
            {
                INSERT_EM_PATIENT_LEAVEDIAG();  //응급환자퇴실진단
            }

            FnTimer = FnTimer + 1;
            lblTimer.Text = FnTimer.ToString();
            if (FnTimer == 300)            
            {                
                Data_Display();
                SELECT_EMRJOB_DSCH(); //기록실_퇴원자
                FnTimer = 0;
            }

            timer1.Enabled = true;
        }
        
        private bool Data_Display()
        {
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strPTMIEMCD = "";        //'     응급의료기관코드
            string strPTMIIDNO = "";        //'     환자등록번호
            string strPTMIINDT = "";        //'     내원일자
            string strPTMIINTM = "";        //'     내원시간
            string strPTMISTAT = "";        //'     자료처리상태 =>참고사항
            string strPTMINAME = "";        //'     성명
            string strPTMIBRTD = "";        //'     생년월일
            string strPTMISEXX = "";        //'     성별
            string strPTMIIUKD = "";        //'     보험유형
            string strPTMIHSCD = "";        //'     요양기관번호
            string strPTMIDRLC = "";        //'     진료의사 면허번호
            string strPTMIAKDT = "";        //'     발명일자(YYYYMMDD)
            string strPTMIAKTM = "";        //'     발명시간(HHMM)
            string strPTMIDGKD = "";        //'     내원사유(질병여부) 1.질병, 2.질병외 9.미상
            string strPTMIARCF = "";        //'     내원사유(의도성여부) 질병여부가 "질병외"인 경우만 입력
            string strPTMIARCS = "";        //'     내원사유(손상기전) 질병여부가 "질병외"인 경우만 입력
            string strPTMIINRT = "";        //'     내원경로
            string strPTMIINMN = "";        //'     내원수단
            string strPTMIMNSY = "";        // '     주증상
            int nPTMIMSSR = 0;              //'     주증상 일련번호
            string strPTMISYM2 = "";        //'     증상-2
            int nPTMISYS2 = 0;              //'     증상-2 일련번호
            string strPTMISYM3 = "";        //'     증상-3
            int nPTMISYS3 = 0;              //'     증상-3 일력번호
            string strPTMIETSY = "";        //'     기타증상
            string strPTMIEMSY = "";        //'     응급증상 해당 유무 Y:응급, N:비응급
            string strPTMIRESP = "";        //'     환자 내원시 반응 A/V/P/U
            int nPTMIHIBP = 0;              //'     내원시 수축기 혈압
            int nPTMILOBP = 0;              //'     내원시 이완기 혈압
            int nPTMIPULS = 0;              //'     내원시 분당 맥박수
            int nPTMIBRTH = 0;              //'     내원시 분당 호흡수
            double nPTMIBDHT = 0;           //'     내원시 체온
            string strPTMIEMRT = "";        //'     응급진료 결과 (귀가/전원/입원/사망/기타/미상)
            string strPTMIDEPT = "";        //'     주진료과
            string strPTMIOTDT = "";        //'     응급실 퇴실일자(YYYYMMDD)
            string strPTMIOTTM = "";        //'     응급실 퇴실시간(HHMM)
            string strPTMIDCRT = "";        //'     입원후 결과 (정상퇴원/자의퇴원/전원/사망/탈월/기타)
            string strPTMIDCDT = "";        //'     퇴원일자(YYYYMMDD)
            string strPTMIDCTM = "";        //'     퇴원시간(HHMM)
            string strPTMITAIP = "";        //'     교통사고당사자
            string strPTMITSBT = "";        //'     교통사고보장구-안전밸드
            string strPTMITSCS = "";        //'     교통사고보장구-이동용좌석
            string strPTMITSFA = "";        //'     교통사고보장구-전면에어백
            string strPTMITSSA = "";        //'     교통사고보장구-측면에어백
            string strPTMITSHM = "";        //'     교통사고보장구-헬맷
            string strPTMITSPT = "";        //'     교통사고보장구-무릎및 관절보호대
            string strPTMITSNO = "";        //'     교통사고보장구-전혀 착용 않함
            string strPTMITSUR = "";        //'     교통사고보장구-비해당
            string strPTMITSUK = "";        //'     교통사고보장구-미상
            string strPTMIETTX = "";        // '     퇴원구분 18,28,38,48,88 사유입력

            string strPTMIINTP = "";        //'전원보낸기관 구분
            string strPTMIDCTP = "";        //'전원보낼기관 구분
            string strPTMIDSID = "";        //'응급번호
            string strPTMIREID = "";        //'재난번호
            string strPTMIZIPC = "";
            string strPTMIADDR = "";
            string strPTMIINCD = "";
            string strPTMIDCCD = "";
            string strPTMIHSDT = "";
            string strPTMIHSTM = "";
            string strPTMIKTID = "";
            string strPTMIKPR1 = "";
            string strPTMIKTS1 = "";
            string strPTMIKTDT = "";
            string strPTMIKTTM = "";
            string strPTMIKJOB = "";
            string strPTMIKIDN = "";
            string strPTMIKPR2 = "";
            string strPTMIKTS2 = "";
            string strPTMIVOXS = "";
            string strPTMIAREA = "";
            string strPTMIHSRT = "";
            string strPTMIPSID = "";

            string strPTMISDCD = "";
            string strSDMDEMCD = "";
            string strSDMDIDNO = "";
            string strSDMDINDT = "";
            string strSDMDINTM = "";
            string strSDMDIDNO_OLD = "";
            string strSDMDINDT_OLD = "";
            string strSDMDINTM_OLD = "";
            string strSDMDDEPT = "";
            string strSDMDDIVI = "";
            string strSDMDDRLC = "";
            string strSDMDSDDT = "";
            string strSDMDSDTM = "";
            string strInDate = "";
            string strInTime = "";
            string strPano = "";

            int nSeqNo = 0;
            string strROWID = "";

            string strJobdateS = "";
            string strJobdateE = "";

            strJobdateS = dtpSDate.Value.ToString("yyyyMMdd");
            strJobdateE = dtpEDate.Value.ToString("yyyyMMdd");

            if (chk1.Checked == true) return rtnVal;

            //'이전까지 보내지 않았던 환자 보내기.
            //'일단 신규 자료 먼저 전송하고!
            //'신규 자료가 전송되면 그다음부터 동일환자에 대한 STAT는 U로 보낸다.
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                #region // 표준응급진료정보 EMIHPTMI 테이블 갱신(신규)
                
                #region // SELECT QUERY (NUR_ER_EMIHPTMI)

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + " /*+index(a INDEX_NUREREMIHPTMI1)*/ PTMIEMCD,PTMIIDNO,PTMIINDT,PTMIINTM,PTMISTAT,PTMINAME,PTMIBRTD,PTMISEXX,";
                SQL = SQL + ComNum.VBLF + " PTMIIUKD,PTMIHSCD,PTMIDRLC,PTMIAKDT,PTMIAKTM,PTMIDGKD,PTMIARCF,PTMIARCS,PTMIINRT,";
                SQL = SQL + ComNum.VBLF + " PTMIINMN,PTMIMNSY,PTMIMSSR,PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,PTMIEMSY,";
                SQL = SQL + ComNum.VBLF + " PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTDT,";
                SQL = SQL + ComNum.VBLF + " PTMIOTTM,PTMIDCRT,PTMIDCDT,PTMIDCTM,PTMITAIP,PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,";
                SQL = SQL + ComNum.VBLF + " PTMITSHM,PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,GBSEND,SEQNO,ROWID,PTMIETTX, ";
                SQL = SQL + ComNum.VBLF + " PTMIINTP,PTMIDCTP,PTMIDSID,PTMIREID,PTMIZIPC, PTMIADDR, PTMIINCD, PTMIDCCD, ";
                SQL = SQL + ComNum.VBLF + " PTMIKTID,PTMIKPR1,PTMIKTS1,PTMIKTDT,PTMIKTTM,PTMIKJOB,PTMIKIDN,PTMIKPR2,PTMIKTS2, ";
                SQL = SQL + ComNum.VBLF + " PTMIVOXS,PTMIAREA,PTMIHSRT,PTMIPSID,PTMISDCD    ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI A";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT >= '" + strJobdateS + "' ";     //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT <= '" + strJobdateE + "' ";     //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND  = ' ') ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMISTAT = 'C' ";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT /*+no_unnest*/ * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";

                #endregion

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        #region // DataTable Read

                        strPTMIEMCD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIEMCD"].ToString().Trim());
                        strPTMIIDNO = clsNurse.NullToEmpty(dt.Rows[i]["PTMIIDNO"].ToString().Trim());
                        strPTMIINDT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINDT"].ToString().Trim());
                        strPTMIINTM = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINTM"].ToString().Trim());
                        strPTMISTAT = clsNurse.NullToEmpty(dt.Rows[i]["PTMISTAT"].ToString().Trim());

                        strPTMIKTID = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKTID"].ToString().Trim());
                        strPTMIKPR1 = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKPR1"].ToString().Trim());
                        strPTMIKTS1 = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKTS1"].ToString().Trim());
                        strPTMIKTDT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKTDT"].ToString().Trim());
                        strPTMIKTTM = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKTTM"].ToString().Trim());
                        strPTMIKJOB = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKJOB"].ToString().Trim());
                        strPTMIKIDN = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKIDN"].ToString().Trim());
                        strPTMIKPR2 = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKPR2"].ToString().Trim());
                        strPTMIKTS2 = clsNurse.NullToEmpty(dt.Rows[i]["PTMIKTS2"].ToString().Trim());

                        strPTMIVOXS = clsNurse.NullToEmpty(dt.Rows[i]["PTMIVOXS"].ToString().Trim());
                        strPTMIAREA = clsNurse.NullToEmpty(dt.Rows[i]["PTMIAREA"].ToString().Trim());
                        strPTMIHSRT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIHSRT"].ToString().Trim());
                        strPTMIPSID = clsNurse.NullToEmpty(dt.Rows[i]["PTMIPSID"].ToString().Trim());

                        strPTMINAME = clsNurse.NullToEmpty(dt.Rows[i]["PTMINAME"].ToString().Trim());
                        strPTMIBRTD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIBRTD"].ToString().Trim());
                        strPTMISEXX = clsNurse.NullToEmpty(dt.Rows[i]["PTMISEXX"].ToString().Trim());
                        strPTMIIUKD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIIUKD"].ToString().Trim());
                        strPTMIHSCD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIHSCD"].ToString().Trim());
                        strPTMIDRLC = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDRLC"].ToString().Trim());
                        strPTMIAKDT = (dt.Rows[i]["PTMIAKDT"].ToString().Trim() == "0000" ? "1111" : dt.Rows[i]["PTMIAKDT"].ToString().Trim());
                        strPTMIAKTM = clsNurse.NullToEmpty(dt.Rows[i]["PTMIAKTM"].ToString().Trim());
                        strPTMIDGKD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDGKD"].ToString().Trim());
                        strPTMIARCF = clsNurse.NullToEmpty(dt.Rows[i]["PTMIARCF"].ToString().Trim());

                        if (strPTMIAKDT == "11111111" || strPTMIAKTM == "1111")
                        {
                            strPTMIAKDT = "11111111";
                            strPTMIAKTM = "1111";
                        }

                        strPTMIARCS = clsNurse.NullToEmpty(dt.Rows[i]["PTMIARCS"].ToString().Trim());
                        strPTMIINRT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINRT"].ToString().Trim());
                        strPTMIINMN = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINMN"].ToString().Trim());
                        strPTMIMNSY = clsNurse.NullToEmpty(dt.Rows[i]["PTMIMNSY"].ToString().Trim());
                        nPTMIMSSR = (int)VB.Val(dt.Rows[i]["PTMIMSSR"].ToString().Trim());
                        strPTMISYM2 = clsNurse.NullToEmpty(dt.Rows[i]["PTMISYM2"].ToString().Trim());
                        nPTMISYS2 = (int)VB.Val(dt.Rows[i]["PTMISYS2"].ToString().Trim());
                        strPTMISYM3 = clsNurse.NullToEmpty(dt.Rows[i]["PTMISYM3"].ToString().Trim());
                        nPTMISYS3 = (int)VB.Val(dt.Rows[i]["PTMISYS3"].ToString().Trim());
                        strPTMIETSY = clsNurse.NullToEmpty(dt.Rows[i]["PTMIETSY"].ToString().Trim());
                        strPTMIEMSY = clsNurse.NullToEmpty(dt.Rows[i]["PTMIEMSY"].ToString().Trim());
                        strPTMIRESP = clsNurse.NullToEmpty(dt.Rows[i]["PTMIRESP"].ToString().Trim());
                        nPTMIHIBP = (int)VB.Val(dt.Rows[i]["PTMIHIBP"].ToString().Trim() == "999" ? "-1" : dt.Rows[i]["PTMIHIBP"].ToString().Trim());
                        nPTMILOBP = (int)VB.Val(dt.Rows[i]["PTMILOBP"].ToString().Trim() == "999" ? "-1" : dt.Rows[i]["PTMILOBP"].ToString().Trim());
                        nPTMIPULS = (int)VB.Val(dt.Rows[i]["PTMIPULS"].ToString().Trim() == "999" ? "-1" : dt.Rows[i]["PTMIPULS"].ToString().Trim());

                        nPTMIBRTH = (int)VB.Val(dt.Rows[i]["PTMIBRTH"].ToString().Trim() == "999" ? "-1" : dt.Rows[i]["PTMIBRTH"].ToString().Trim());
                        nPTMIBDHT = (int)VB.Val(dt.Rows[i]["PTMIBDHT"].ToString().Trim() == "99.9" ? "-1" : dt.Rows[i]["PTMIBDHT"].ToString().Trim());
                        strPTMIEMRT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIEMRT"].ToString().Trim());
                        strPTMIDEPT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDEPT"].ToString().Trim());
                        strPTMIOTDT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIOTDT"].ToString().Trim());

                        strPTMIOTTM = clsNurse.NullToEmpty(dt.Rows[i]["PTMIOTTM"].ToString().Trim());
                        strPTMIDCRT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDCRT"].ToString().Trim());
                        strPTMIDCDT = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDCDT"].ToString().Trim());
                        strPTMIDCTM = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDCTM"].ToString().Trim());
                        strPTMITAIP = clsNurse.NullToEmpty(dt.Rows[i]["PTMITAIP"].ToString().Trim());

                        strPTMITSBT = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSBT"].ToString().Trim());
                        strPTMITSCS = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSCS"].ToString().Trim());
                        strPTMITSFA = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSFA"].ToString().Trim());
                        strPTMITSSA = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSSA"].ToString().Trim());
                        strPTMITSHM = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSHM"].ToString().Trim());

                        strPTMITSPT = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSPT"].ToString().Trim());
                        strPTMITSNO = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSNO"].ToString().Trim());
                        strPTMITSUR = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSUR"].ToString().Trim());
                        strPTMITSUK = clsNurse.NullToEmpty(dt.Rows[i]["PTMITSUK"].ToString().Trim());
                        strPTMIETTX = clsNurse.NullToEmpty(dt.Rows[i]["PTMIETTX"].ToString().Trim());
                        strROWID = clsNurse.NullToEmpty(dt.Rows[i]["ROWID"].ToString().Trim());

                        strPTMIINTP = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINTP"].ToString().Trim());
                        strPTMIDCTP = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDCTP"].ToString().Trim());
                        strPTMIDSID = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDSID"].ToString().Trim());
                        strPTMIREID = clsNurse.NullToEmpty(dt.Rows[i]["PTMIREID"].ToString().Trim());

                        strPTMIZIPC = clsNurse.NullToEmpty(dt.Rows[i]["PTMIZIPC"].ToString().Trim());
                        strPTMIADDR = clsNurse.NullToEmpty(dt.Rows[i]["PTMIADDR"].ToString().Trim());
                        strPTMIINCD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIINCD"].ToString().Trim());
                        strPTMIDCCD = clsNurse.NullToEmpty(dt.Rows[i]["PTMIDCCD"].ToString().Trim());
                        strPTMISDCD = clsNurse.NullToEmpty(dt.Rows[i]["PTMISDCD"].ToString().Trim());

                        #endregion

                        #region // INSERT QUERY (EDISAGENT.EMIHPTMI)

                        SQL = " INSERT INTO EMIHPTMI@EDISAGENT (";
                        SQL = SQL + ComNum.VBLF + " PTMIEMCD,PTMIIDNO,PTMIINDT,PTMIINTM,PTMISTAT,";
                        SQL = SQL + ComNum.VBLF + " PTMINAME,PTMIBRTD,PTMISEXX,PTMIIUKD,PTMIHSCD,";
                        SQL = SQL + ComNum.VBLF + " PTMIDRLC,PTMIAKDT,PTMIAKTM,PTMIDGKD,PTMIARCF,";
                        SQL = SQL + ComNum.VBLF + " PTMIARCS,PTMIINRT,PTMIINMN,PTMIMNSY,PTMIMSSR,";
                        SQL = SQL + ComNum.VBLF + " PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,";
                        SQL = SQL + ComNum.VBLF + " PTMIEMSY,PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,";
                        SQL = SQL + ComNum.VBLF + " PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTDT,";
                        SQL = SQL + ComNum.VBLF + " PTMIOTTM,PTMIDCRT,PTMIDCDT,PTMIDCTM,PTMITAIP,";
                        SQL = SQL + ComNum.VBLF + " PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,PTMITSHM,";
                        SQL = SQL + ComNum.VBLF + " PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,PTMIETTX,";
                        SQL = SQL + ComNum.VBLF + " PSHROWID,PTMIINTP,PTMIDCTP,PTMIDSID,PTMIREID,";
                        SQL = SQL + ComNum.VBLF + " PTMIZIPC,PTMIADDR,PTMIINCD,PTMIDCCD,PTMIKTID,";
                        SQL = SQL + ComNum.VBLF + " PTMIKPR1,PTMIKTS1,PTMIKTDT,PTMIKTTM,PTMIKJOB,";
                        SQL = SQL + ComNum.VBLF + " PTMIKIDN,PTMIKPR2,PTMIKTS2,PTMIVOXS,PTMIAREA,";
                        SQL = SQL + ComNum.VBLF + " PTMIHSRT,PTMIPSID,PTMIMDCD,PTMIBENO,PTMISDCD ";
                        SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIEMCD + "', '" + strPTMIIDNO + "', '" + strPTMIINDT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "', '" + strPTMISTAT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMINAME + "', '" + strPTMIBRTD + "', '" + strPTMISEXX + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIIUKD + "', '37100068',            ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDRLC + "', '" + strPTMIAKDT + "', '" + strPTMIAKTM + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDGKD + "', '" + strPTMIARCF + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIARCS + "', '" + strPTMIINRT + "', '" + strPTMIINMN + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIMNSY + "',  " + nPTMIMSSR + ",    ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMISYM2 + "',  " + nPTMISYS2 + ",    '" + strPTMISYM3 + "', ";
                        SQL = SQL + ComNum.VBLF + "  " + nPTMISYS3 + ",    '" + strPTMIETSY + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIEMSY + "', '" + strPTMIRESP + "',  " + nPTMIHIBP + ",   ";
                        SQL = SQL + ComNum.VBLF + "  " + nPTMILOBP + ",     " + nPTMIPULS + ",    ";
                        SQL = SQL + ComNum.VBLF + "  " + nPTMIBRTH + ",     " + nPTMIBDHT + ",    '" + strPTMIEMRT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDEPT + "', '" + strPTMIOTDT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIOTTM + "', '" + strPTMIDCRT + "', '" + strPTMIDCDT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDCTM + "', '" + strPTMITAIP + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMITSBT + "', '" + strPTMITSCS + "', '" + strPTMITSFA + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMITSSA + "', '" + strPTMITSHM + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMITSPT + "', '" + strPTMITSNO + "', '" + strPTMITSUR + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMITSUK + "', '" + strPTMIETTX + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strROWID + "','" + strPTMIINTP + "','" + strPTMIDCTP + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDSID + "','" + strPTMIREID + "',";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIZIPC + "','" + strPTMIADDR + "','" + strPTMIINCD + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIDCCD + "','" + strPTMIKTID + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIKPR1 + "','" + strPTMIKTS1 + "','" + strPTMIKTDT + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIKTTM + "','" + strPTMIKJOB + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIKIDN + "','" + strPTMIKPR2 + "','" + strPTMIKTS2 + "', ";
                        SQL = SQL + ComNum.VBLF + "  " + (strPTMIVOXS == "" ? "NULL" : strPTMIVOXS) + ",'" + strPTMIAREA + "',";                        
                        SQL = SQL + ComNum.VBLF + " '" + strPTMIHSRT + "',";
                        //SQL = SQL + ComNum.VBLF + "  " + (strPTMIPSID == "" ? "NULL" : strPTMIPSID) + ",'-','-','" + strPTMISDCD + "')";
                        SQL = SQL + ComNum.VBLF + "  '" + strPTMIPSID + "','-','-','" + strPTMISDCD + "')";

                        #endregion

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        #region // UPDATE QUERY (NUR_ER_EMIHPTMI)

                        SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND  = 'Y' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        #endregion

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("NUR_ER_EMIHPTMI 수정시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    } //for
                }
                dt.Dispose();
                dt = null;

                #endregion
                
                #region // 응급환자 전문의 진료내역 EMIHSDMD 테이블 갱신

                strSDMDIDNO = "";
                strSDMDINDT = "";
                strSDMDINTM = "";
                strSDMDIDNO_OLD = "";
                strSDMDINDT_OLD = "";
                strSDMDINTM_OLD = "";

                SQL = " SELECT SDMDEMCD, SDMDIDNO, SDMDINDT, SDMDINTM, SDMDDEPT,";
                SQL = SQL + ComNum.VBLF + " SDMDDIVI, SDMDDRLC, SDMDSDDT, SDMDSDTM, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_EMIHSDMD A";
                SQL = SQL + ComNum.VBLF + " WHERE SDMDINDT >= '" + strJobdateS + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDMDINDT <= '" + strJobdateE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND  = ' ') ";
                SQL = SQL + ComNum.VBLF + "   AND SDMDIDNO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.SDMDIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.SDMDINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.SDMDINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SDMDIDNO ASC, SDMDINDT ASC, SDMDINTM ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSDMDEMCD = clsNurse.NullToEmpty(dt.Rows[i]["SDMDEMCD"].ToString().Trim());
                        strSDMDIDNO = clsNurse.NullToEmpty(dt.Rows[i]["SDMDIDNO"].ToString().Trim());
                        strSDMDINDT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDINDT"].ToString().Trim());
                        strSDMDINTM = clsNurse.NullToEmpty(dt.Rows[i]["SDMDINTM"].ToString().Trim());

                        if (strSDMDIDNO != strSDMDIDNO_OLD || strSDMDINDT != strSDMDINDT_OLD || strSDMDINTM != strSDMDINTM_OLD)
                        {
                            SQL = " DELETE EMIHSDMD@EDISAGENT";
                            SQL = SQL + ComNum.VBLF + " WHERE SDMDIDNO = '" + strSDMDIDNO + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SDMDINDT = '" + strSDMDINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SDMDINTM = '" + strSDMDINTM + "' AND SDMDEMCD='C24C0083'  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                            strSDMDIDNO_OLD = strSDMDIDNO;
                            strSDMDINDT_OLD = strSDMDINDT;
                            strSDMDINTM_OLD = strSDMDINTM;
                        }

                        strSDMDDEPT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDEPT"].ToString().Trim());
                        strSDMDDIVI = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDIVI"].ToString().Trim());
                        strSDMDDRLC = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDRLC"].ToString().Trim());
                        strSDMDSDDT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDSDDT"].ToString().Trim());
                        strSDMDSDTM = clsNurse.NullToEmpty(dt.Rows[i]["SDMDSDTM"].ToString().Trim());
                        strROWID = clsNurse.NullToEmpty(dt.Rows[i]["ROWID"].ToString().Trim());


                        SQL = " INSERT INTO EMIHSDMD@EDISAGENT (";
                        SQL = SQL + ComNum.VBLF + " SDMDEMCD, SDMDIDNO, SDMDINDT, SDMDINTM, ";
                        SQL = SQL + ComNum.VBLF + " SDMDDEPT, SDMDDIVI, SDMDDRLC, SDMDSDDT, ";
                        SQL = SQL + ComNum.VBLF + " SDMDSDTM) VALUES (";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDEMCD + "','" + strSDMDIDNO + "','" + strSDMDINDT + "','" + strSDMDINTM + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDDEPT + "','" + strSDMDDIVI + "','" + strSDMDDRLC + "','" + strSDMDSDDT + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDSDTM + "')";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        SQL = " UPDATE NUR_ER_EMIHSDMD SET GBSEND  = 'Y' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion
                                
                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);
                
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }


            //'Agent PC
            clsDB.setBeginTran(clsDB.DbCon);
            //clsDB.setBeginTran(pEdisAgentDb);

            try
            {
                #region // 표준응급진료정보 EMIHPTMI 테이블 갱신(전체)

                SQL = " SELECT /*+index(A INDEX_NUREREMIHPTMI1) */ MAX(SEQNO) SEQNO, PTMIIDNO,PTMIINDT,PTMIINTM ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI A ";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT >= '" + strJobdateS + "' ";    //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT <= '" + strJobdateE + "' ";    //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND = ' ') ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO <> '81000004'";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT /*+no_unnest*/ *  FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + " GROUP BY PTMIIDNO,PTMIINDT,PTMIINTM";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strInDate = dt.Rows[i]["PTMIINDT"].ToString().Trim();
                        strInTime = dt.Rows[i]["PTMIINTM"].ToString().Trim();
                        strPano = dt.Rows[i]["PTMIIDNO"].ToString().Trim();
                        nSeqNo = (int)VB.Val(dt.Rows[i]["SEQNO"].ToString().Trim());


                        //'2016-02-29 막음!
                        //'너무 잦은 전송으로 인하여 마지막 입력 정보를 제대로 가져오지 못하는 경우가 발생함.
                        SQL = " SELECT SEQNO, GBSEND FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
                        SQL = SQL + ComNum.VBLF + " WHERE SEQNO = (";
                        SQL = SQL + ComNum.VBLF + " SELECT MAX(SEQNO) FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "      AND PTMIINDT = '" + strInDate + "'";
                        SQL = SQL + ComNum.VBLF + "      AND PTMIINTM = '" + strInTime + "')";
                        SQL = SQL + ComNum.VBLF + "      AND PTMIIDNO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "      AND PTMIINDT = '" + strInDate + "'";
                        SQL = SQL + ComNum.VBLF + "      AND PTMIINTM = '" + strInTime + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["GBSEND"].ToString().Trim() == "Y" && (int)VB.Val(dt1.Rows[0]["SEQNO"].ToString().Trim()) >= nSeqNo)
                            {
                                SQL = " UPDATE KOSMOS_PMPA.NUR_ER_EMIHPTMI ";
                                SQL = SQL + ComNum.VBLF + " SET GBSEND = 'A'";
                                SQL = SQL + ComNum.VBLF + " WHERE (GBSEND IS NULL OR GBSEND = ' ') ";
                                SQL = SQL + ComNum.VBLF + "      AND PTMIIDNO = '" + strPano + "'";
                                SQL = SQL + ComNum.VBLF + "      AND PTMIINDT = '" + strInDate + "'";
                                SQL = SQL + ComNum.VBLF + "      AND PTMIINTM = '" + strInTime + "'";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(pEdisAgentDb);
                                    clsDB.setRollbackTran(clsDB.DbCon);

                                    ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }


                        SQL = " SELECT ";
                        SQL = SQL + ComNum.VBLF + " PTMIEMCD,PTMIIDNO,PTMIINDT,PTMIINTM,PTMISTAT,PTMINAME,PTMIBRTD,PTMISEXX,";
                        SQL = SQL + ComNum.VBLF + " PTMIIUKD,PTMIHSCD,PTMIDRLC,PTMIAKDT,PTMIAKTM,PTMIDGKD,PTMIARCF,PTMIARCS,PTMIINRT,";
                        SQL = SQL + ComNum.VBLF + " PTMIINMN,PTMIMNSY,PTMIMSSR,PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,PTMIEMSY,";
                        SQL = SQL + ComNum.VBLF + " PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTDT,";
                        SQL = SQL + ComNum.VBLF + " PTMIOTTM,PTMIDCRT,PTMIDCDT,PTMIDCTM,PTMITAIP,PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,";
                        SQL = SQL + ComNum.VBLF + " PTMITSHM,PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,GBSEND,SEQNO,ROWID, ";
                        SQL = SQL + ComNum.VBLF + " PTMIINTP,PTMIDCTP,PTMIDSID,PTMIREID,PTMIETTX, PTMIZIPC, PTMIADDR, PTMIINCD, PTMIDCCD, ";
                        SQL = SQL + ComNum.VBLF + " PTMIKTID,PTMIKPR1,PTMIKTS1,PTMIKTDT,PTMIKTTM,PTMIKJOB,PTMIKIDN,PTMIKPR2,PTMIKTS2, ";
                        SQL = SQL + ComNum.VBLF + " PTMIVOXS,PTMIAREA,PTMIHSRT,PTMIPSID,PTMISDCD,PTMIHSDT,PTMIHSTM    ";
                        SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strInTime + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND = ' ') ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO <> '81000004' ";
                        SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI ";
                        SQL = SQL + ComNum.VBLF + "                   WHERE PTMIIDNO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "                   AND PTMIINDT = '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "                   AND PTMIINTM = '" + strInTime + "' )";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                #region // DataTable Read

                                strPTMIEMCD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIEMCD"].ToString().Trim());
                                strPTMIIDNO = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIIDNO"].ToString().Trim());

                                strPTMIINDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINDT"].ToString().Trim());
                                strPTMIINTM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINTM"].ToString().Trim());
                                strPTMISTAT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMISTAT"].ToString().Trim());
                                strPTMIKTID = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKTID"].ToString().Trim());
                                strPTMIKPR1 = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKPR1"].ToString().Trim());
                                strPTMIKTS1 = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKTS1"].ToString().Trim());
                                strPTMIKTDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKTDT"].ToString().Trim());
                                strPTMIKTTM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKTTM"].ToString().Trim());
                                strPTMIKJOB = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKJOB"].ToString().Trim());
                                strPTMIKIDN = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKIDN"].ToString().Trim());
                                strPTMIKPR2 = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKPR2"].ToString().Trim());
                                strPTMIKTS2 = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIKTS2"].ToString().Trim());
                                strPTMIVOXS = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIVOXS"].ToString().Trim());
                                strPTMIAREA = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIAREA"].ToString().Trim());
                                strPTMIHSRT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIHSRT"].ToString().Trim());
                                strPTMIPSID = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIPSID"].ToString().Trim());
                                strPTMINAME = clsNurse.NullToEmpty(dt1.Rows[j]["PTMINAME"].ToString().Trim());
                                strPTMIBRTD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIBRTD"].ToString().Trim());
                                strPTMISEXX = clsNurse.NullToEmpty(dt1.Rows[j]["PTMISEXX"].ToString().Trim());
                                strPTMIIUKD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIIUKD"].ToString().Trim());
                                strPTMIHSCD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIHSCD"].ToString().Trim());
                                strPTMIHSCD = "37100068";

                                strPTMIDRLC = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDRLC"].ToString().Trim());
                                strPTMIAKDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIAKDT"].ToString().Trim());
                                strPTMIAKTM = (dt1.Rows[j]["PTMIAKTM"].ToString().Trim() == "0000" ? "1111" : dt1.Rows[j]["PTMIAKTM"].ToString().Trim());
                                strPTMIDGKD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDGKD"].ToString().Trim());
                                strPTMIARCF = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIARCF"].ToString().Trim());
                                if (strPTMIAKDT == "11111111" || strPTMIAKTM == "1111")
                                {
                                    strPTMIAKDT = "11111111";
                                    strPTMIAKTM = "1111";
                                }


                                strPTMIARCS = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIARCS"].ToString().Trim());
                                strPTMIINRT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINRT"].ToString().Trim());
                                strPTMIINMN = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINMN"].ToString().Trim());
                                strPTMIMNSY = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIMNSY"].ToString().Trim());
                                nPTMIMSSR = (int)VB.Val(dt1.Rows[j]["PTMIMSSR"].ToString().Trim());


                                strPTMISYM2 = dt1.Rows[j]["PTMISYM2"].ToString().Trim();
                                nPTMISYS2 = (int)VB.Val(dt1.Rows[j]["PTMISYS2"].ToString().Trim());
                                strPTMISYM3 = dt1.Rows[j]["PTMISYM3"].ToString().Trim();
                                nPTMISYS3 = (int)VB.Val(dt1.Rows[j]["PTMISYS3"].ToString().Trim());
                                strPTMIETSY = dt1.Rows[j]["PTMIETSY"].ToString().Trim();


                                strPTMIEMSY = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIEMSY"].ToString().Trim());
                                strPTMIRESP = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIRESP"].ToString().Trim());
                                nPTMIHIBP = (int)VB.Val(dt1.Rows[j]["PTMIHIBP"].ToString().Trim() == "999" ? "-1" : dt1.Rows[j]["PTMIHIBP"].ToString().Trim());
                                nPTMILOBP = (int)VB.Val(dt1.Rows[j]["PTMILOBP"].ToString().Trim() == "999" ? "-1" : dt1.Rows[j]["PTMILOBP"].ToString().Trim());
                                nPTMIPULS = (int)VB.Val(dt1.Rows[j]["PTMIPULS"].ToString().Trim() == "999" ? "-1" : dt1.Rows[j]["PTMIPULS"].ToString().Trim());


                                nPTMIBRTH = (int)VB.Val(dt1.Rows[j]["PTMIBRTH"].ToString().Trim() == "999" ? "-1" : dt1.Rows[j]["PTMIBRTH"].ToString().Trim());
                                nPTMIBDHT = (int)VB.Val(dt1.Rows[j]["PTMIBDHT"].ToString().Trim() == "99.9" ? "-1" : dt1.Rows[j]["PTMIBDHT"].ToString().Trim());
                                strPTMIEMRT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIEMRT"].ToString().Trim());
                                strPTMIDEPT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDEPT"].ToString().Trim());
                                strPTMIOTDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIOTDT"].ToString().Trim());

                                strPTMIOTTM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIOTTM"].ToString().Trim());
                                strPTMIDCRT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDCRT"].ToString().Trim());
                                strPTMIDCDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDCDT"].ToString().Trim());
                                strPTMIDCTM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDCTM"].ToString().Trim());
                                strPTMITAIP = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITAIP"].ToString().Trim());

                                strPTMITSBT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSBT"].ToString().Trim());
                                strPTMITSCS = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSCS"].ToString().Trim());
                                strPTMITSFA = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSFA"].ToString().Trim());
                                strPTMITSSA = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSSA"].ToString().Trim());
                                strPTMITSHM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSHM"].ToString().Trim());

                                strPTMITSPT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSPT"].ToString().Trim());
                                strPTMITSNO = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSNO"].ToString().Trim());
                                strPTMITSUR = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSUR"].ToString().Trim());
                                strPTMITSUK = clsNurse.NullToEmpty(dt1.Rows[j]["PTMITSUK"].ToString().Trim());
                                strROWID = clsNurse.NullToEmpty(dt1.Rows[j]["ROWID"].ToString().Trim());

                                strPTMIINTP = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINTP"].ToString().Trim());
                                strPTMIDCTP = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDCTP"].ToString().Trim());
                                strPTMIDSID = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDSID"].ToString().Trim());
                                strPTMIREID = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIREID"].ToString().Trim());
                                strPTMIETTX = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIETTX"].ToString().Trim());

                                strPTMIZIPC = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIZIPC"].ToString().Trim());
                                strPTMIADDR = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIADDR"].ToString().Trim());
                                strPTMIINCD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIINCD"].ToString().Trim());
                                strPTMIDCCD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIDCCD"].ToString().Trim());
                                strPTMISDCD = clsNurse.NullToEmpty(dt1.Rows[j]["PTMISDCD"].ToString().Trim());

                                strPTMIHSDT = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIHSDT"].ToString().Trim());
                                if (strPTMIHSDT == "")
                                {
                                    strPTMIHSDT = "-";
                                }
                                strPTMIHSTM = clsNurse.NullToEmpty(dt1.Rows[j]["PTMIHSTM"].ToString().Trim());
                                if (strPTMIHSTM == "")
                                {
                                    strPTMIHSTM = "-";
                                }

                                #endregion

                                SQL = " SELECT PTMISTAT FROM EMIHPTMI@EDISAGENT ";
                                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPTMIIDNO + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' AND PTMIEMCD='C24C0083' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(pEdisAgentDb);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return rtnVal;
                                }

                                if (dt2.Rows.Count == 0)
                                {
                                    #region // INSERT QUERY (EDISAGENT.EMIHPTMI)

                                    SQL = " INSERT INTO EMIHPTMI@EDISAGENT (";
                                    SQL = SQL + ComNum.VBLF + " PTMIEMCD,PTMIIDNO,PTMIINDT,PTMIINTM,PTMISTAT,";
                                    SQL = SQL + ComNum.VBLF + " PTMINAME,PTMIBRTD,PTMISEXX,PTMIIUKD,PTMIHSCD,";
                                    SQL = SQL + ComNum.VBLF + " PTMIDRLC,PTMIAKDT,PTMIAKTM,PTMIDGKD,PTMIARCF,";
                                    SQL = SQL + ComNum.VBLF + " PTMIARCS,PTMIINRT,PTMIINMN,PTMIMNSY,PTMIMSSR,";
                                    SQL = SQL + ComNum.VBLF + " PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,";
                                    SQL = SQL + ComNum.VBLF + " PTMIEMSY,PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,";
                                    SQL = SQL + ComNum.VBLF + " PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTDT,";
                                    SQL = SQL + ComNum.VBLF + " PTMIOTTM,PTMIDCRT,PTMIDCDT,PTMIDCTM,PTMITAIP,";
                                    SQL = SQL + ComNum.VBLF + " PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,PTMITSHM,";
                                    SQL = SQL + ComNum.VBLF + " PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,PTMIETTX,";
                                    SQL = SQL + ComNum.VBLF + " PSHROWID,PTMIINTP,PTMIDCTP,PTMIDSID,PTMIREID,";
                                    SQL = SQL + ComNum.VBLF + " PTMIZIPC,PTMIADDR,PTMIINCD,PTMIDCCD,PTMIKTID,";
                                    SQL = SQL + ComNum.VBLF + " PTMIKPR1,PTMIKTS1,PTMIKTDT,PTMIKTTM,PTMIKJOB,";
                                    SQL = SQL + ComNum.VBLF + " PTMIKIDN,PTMIKPR2,PTMIKTS2,PTMIVOXS,PTMIAREA,";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSRT,PTMIPSID,PTMIMDCD,PTMIBENO,PTMISDCD,";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSDT,PTMIHSTM) VALUES (";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIEMCD + "', '" + strPTMIIDNO + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINDT + "', '" + strPTMIINTM + "', 'C', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMINAME + "', '" + strPTMIBRTD + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMISEXX + "', '" + strPTMIIUKD + "', '37100068',            ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIDRLC + "', '" + strPTMIAKDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIAKTM + "', '" + strPTMIDGKD + "', '" + strPTMIARCF + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIARCS + "', '" + strPTMIINRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINMN + "', '" + strPTMIMNSY + "',  " + nPTMIMSSR + ",    ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMISYM2 + "',  " + nPTMISYS2 + ",    ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMISYM3 + "',  " + nPTMISYS3 + ",    '" + strPTMIETSY + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIEMSY + "', '" + strPTMIRESP + "',  ";
                                    SQL = SQL + ComNum.VBLF + "  " + nPTMIHIBP + ",     " + nPTMILOBP + ",     " + nPTMIPULS + ",    ";
                                    SQL = SQL + ComNum.VBLF + "  " + nPTMIBRTH + ",     " + nPTMIBDHT + ",    ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIEMRT + "', '" + strPTMIDEPT + "', '" + strPTMIOTDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIOTTM + "', '" + strPTMIDCRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIDCDT + "', '" + strPTMIDCTM + "', '" + strPTMITAIP + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMITSBT + "', '" + strPTMITSCS + "', '" + strPTMITSFA + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMITSSA + "', '" + strPTMITSHM + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMITSPT + "', '" + strPTMITSNO + "', '" + strPTMITSUR + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMITSUK + "', '" + strPTMIETTX + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strROWID + "','" + strPTMIINTP + "','" + strPTMIDCTP + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIDSID + "','" + strPTMIREID + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIZIPC + "','" + strPTMIADDR + "','" + strPTMIINCD + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIDCCD + "','" + strPTMIKTID + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIKPR1 + "','" + strPTMIKTS1 + "','" + strPTMIKTDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIKTTM + "','" + strPTMIKJOB + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIKIDN + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + (VB.Len(strPTMIKPR2) > 5 ? VB.Left(strPTMIKPR2, 5) : strPTMIKPR2) + "',  ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIKTS2 + "',";
                                    SQL = SQL + ComNum.VBLF + "  " + (strPTMIVOXS == "" ? "NULL" : strPTMIVOXS) + ", ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIAREA + "',";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIHSRT + "',";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIPSID + "', '-', '-', ";
                                    SQL = SQL + ComNum.VBLF + " '" + (strPTMISDCD.Trim() == "" ? "-" : strPTMISDCD) + "',";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIHSDT + "','" + strPTMIHSTM + "')";

                                    #endregion

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    #region // UPDATE QUERY (NUR_ER_EMIHPTMI)

                                    SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND  = 'Y' ";
                                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strInDate + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strInTime + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SEQNO = " + nSeqNo + " ";

                                    #endregion

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("NUR_ER_EMIHPTMI 수정시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                else
                                {
                                    #region // UPDATE QUERY (EDISAGENT.EMIHPTMI)

                                    SQL = " UPDATE EMIHPTMI@EDISAGENT SET ";
                                    SQL = SQL + ComNum.VBLF + " PTMISTAT = '" + strPTMISTAT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMINAME = '" + VB.Trim(strPTMINAME) + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIBRTD = '" + strPTMIBRTD + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMISEXX = '" + strPTMISEXX + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIIUKD = '" + strPTMIIUKD + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSCD = '37100068', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDRLC = '" + strPTMIDRLC + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIAKDT = '" + strPTMIAKDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIAKTM = '" + strPTMIAKTM + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDGKD = '" + strPTMIDGKD + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIARCF = '" + strPTMIARCF + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIARCS = '" + strPTMIARCS + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIINRT = '" + strPTMIINRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIINMN = '" + strPTMIINMN + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIMNSY = '" + strPTMIMNSY + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIMSSR = " + nPTMIMSSR + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMISYM2 = '" + strPTMISYM2 + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMISYS2 = " + nPTMISYS2 + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMISYM3 = '" + strPTMISYM3 + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMISYS3 = " + nPTMISYS3 + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIETSY = '" + strPTMIETSY + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIEMSY = '" + strPTMIEMSY + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIRESP = '" + strPTMIRESP + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIHIBP = " + nPTMIHIBP + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMILOBP = " + nPTMILOBP + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIPULS = " + nPTMIPULS + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIBRTH = " + nPTMIBRTH + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIBDHT = " + nPTMIBDHT + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIEMRT = '" + strPTMIEMRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDEPT = '" + strPTMIDEPT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIOTDT = '" + strPTMIOTDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIOTTM = '" + strPTMIOTTM + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDCRT = '" + strPTMIDCRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDCDT = '" + strPTMIDCDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDCTM = '" + strPTMIDCTM + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITAIP = '" + strPTMITAIP + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSBT = '" + strPTMITSBT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSCS = '" + strPTMITSCS + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSFA = '" + strPTMITSFA + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSSA = '" + strPTMITSSA + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSHM = '" + strPTMITSHM + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSPT = '" + strPTMITSPT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSNO = '" + strPTMITSNO + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSUR = '" + strPTMITSUR + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMITSUK = '" + strPTMITSUK + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIINTP = '" + strPTMIINTP + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDCTP = '" + strPTMIDCTP + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDSID = '" + strPTMIDSID + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIREID = '" + strPTMIREID + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIZIPC = '" + strPTMIZIPC + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIADDR = '" + strPTMIADDR + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIINCD = '" + strPTMIINCD + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIDCCD = '" + strPTMIDCCD + "', ";
                                    //' SQL = SQL + ComNum.VBLF + " PTMIKTID = '" + strPTMIKTID + "', " ;       //**  최초 전송후 UPDATE 오류 처리됨
                                    //' SQL = SQL + ComNum.VBLF + " PTMIKPR1 = '" + strPTMIKPR1 + "', " ;
                                    //' SQL = SQL + ComNum.VBLF + " PTMIKTS1 = '" + strPTMIKTS1 + "', " ;
                                    //' SQL = SQL + ComNum.VBLF + " PTMIKTDT = '" + strPTMIKTDT + "', " ;
                                    //' SQL = SQL + ComNum.VBLF + " PTMIKTTM = '" + strPTMIKTTM + "', " ;
                                    SQL = SQL + ComNum.VBLF + " PTMIKJOB = '" + strPTMIKJOB + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIKIDN = '" + strPTMIKIDN + "', ";


                                    if (strPTMIOTDT == "")                                                    //'**  퇴실이후 전송 오류 처리됨
                                    {
                                        SQL = SQL + ComNum.VBLF + " PTMIKPR2 = '" + strPTMIKPR2 + "', ";
                                        SQL = SQL + ComNum.VBLF + " PTMIKTS2 = '" + strPTMIKTS2 + "', ";
                                    }

                                    SQL = SQL + ComNum.VBLF + " PTMIVOXS = " + (strPTMIVOXS == "" ? "NULL" : strPTMIVOXS) + ", ";
                                    SQL = SQL + ComNum.VBLF + " PTMIAREA = '" + strPTMIAREA + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSRT = '" + strPTMIHSRT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIPSID = '" + (strPTMIPSID == "" ? "NULL" : strPTMIPSID) + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMISDCD = '" + (strPTMISDCD == "" ? "-" : strPTMISDCD) + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSDT = '" + strPTMIHSDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIHSTM = '" + strPTMIHSTM + "', ";
                                    SQL = SQL + ComNum.VBLF + " PTMIETTX = '" + strPTMIETTX + "', ";

                                    if (strPTMISTAT == "D")
                                    {
                                        SQL = SQL + ComNum.VBLF + " PSHROWID = 'DEL' ";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + " PSHROWID = '" + strROWID + "' ";
                                    }
                                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strInDate + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strInTime + "' AND PTMIEMCD='C24C0083' ";

                                    #endregion

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    #region // UPDATE QUERY (NUR_ER_EMIHPTMI)

                                    SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND  = 'Y' ";
                                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPano + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strInDate + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strInTime + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SEQNO = " + nSeqNo + " ";

                                    #endregion

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("NUR_ER_EMIHPTMI 수정시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                                dt2.Dispose();
                                dt2 = null;
                            } //for
                        }
                        dt1.Dispose();
                        dt1 = null;

                    } //for

                }
                dt.Dispose();
                dt = null;

                #endregion

                #region // 응급환자 전문의 진료내역 EMIHSDMD 테이블 갱신

                strSDMDIDNO = "";
                strSDMDINDT = "";
                strSDMDINTM = "";
                strSDMDIDNO_OLD = "";
                strSDMDINDT_OLD = "";
                strSDMDINTM_OLD = "";
                SQL = " SELECT SDMDEMCD, SDMDIDNO, SDMDINDT, SDMDINTM, SDMDDEPT,";
                SQL = SQL + ComNum.VBLF + " SDMDDIVI, SDMDDRLC, SDMDSDDT, SDMDSDTM, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_EMIHSDMD A";
                SQL = SQL + ComNum.VBLF + " WHERE SDMDINDT >= '" + strJobdateS + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDMDINDT <= '" + strJobdateE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND  = ' ') ";
                SQL = SQL + ComNum.VBLF + "   AND SDMDIDNO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.SDMDIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.SDMDINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.SDMDINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SDMDIDNO ASC, SDMDINDT ASC, SDMDINTM ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSDMDEMCD = clsNurse.NullToEmpty(dt.Rows[i]["SDMDEMCD"].ToString().Trim());
                        strSDMDIDNO = clsNurse.NullToEmpty(dt.Rows[i]["SDMDIDNO"].ToString().Trim());
                        strSDMDINDT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDINDT"].ToString().Trim());
                        strSDMDINTM = clsNurse.NullToEmpty(dt.Rows[i]["SDMDINTM"].ToString().Trim());

                        if (strSDMDIDNO != strSDMDIDNO_OLD || strSDMDINDT != strSDMDINDT_OLD || strSDMDINTM != strSDMDINTM_OLD)
                        {
                            SQL = " DELETE EMIHSDMD@EDISAGENT ";
                            SQL = SQL + ComNum.VBLF + " WHERE SDMDIDNO = '" + strSDMDIDNO + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SDMDINDT = '" + strSDMDINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SDMDINTM = '" + strSDMDINTM + "' AND SDMDEMCD='C24C0083'  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                            strSDMDIDNO_OLD = strSDMDIDNO;
                            strSDMDINDT_OLD = strSDMDINDT;
                            strSDMDINTM_OLD = strSDMDINTM;
                        }

                        strSDMDDEPT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDEPT"].ToString().Trim());
                        strSDMDDIVI = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDIVI"].ToString().Trim());
                        strSDMDDRLC = clsNurse.NullToEmpty(dt.Rows[i]["SDMDDRLC"].ToString().Trim());
                        strSDMDSDDT = clsNurse.NullToEmpty(dt.Rows[i]["SDMDSDDT"].ToString().Trim());
                        strSDMDSDTM = clsNurse.NullToEmpty(dt.Rows[i]["SDMDSDTM"].ToString().Trim());
                        strROWID = clsNurse.NullToEmpty(dt.Rows[i]["ROWID"].ToString().Trim());


                        SQL = " INSERT INTO EMIHSDMD@EDISAGENT (";
                        SQL = SQL + ComNum.VBLF + " SDMDEMCD, SDMDIDNO, SDMDINDT, SDMDINTM, ";
                        SQL = SQL + ComNum.VBLF + " SDMDDEPT, SDMDDIVI, SDMDDRLC, SDMDSDDT, ";
                        SQL = SQL + ComNum.VBLF + " SDMDSDTM) VALUES (";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDEMCD + "','" + strSDMDIDNO + "','" + strSDMDINDT + "','" + strSDMDINTM + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDDEPT + "','" + strSDMDDIVI + "','" + strSDMDDRLC + "','" + strSDMDSDDT + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strSDMDSDTM + "')";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        SQL = " UPDATE NUR_ER_EMIHSDMD SET GBSEND  = 'Y' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);
                
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            
            
            
            string strDGOTEMCD = "";
            string strDGOTIDNO = "";
            string strDGOTINDT = "";
            string strDGOTINTM = "";
            string strDGOTDIAG = "";
            string strDGOTDGGB = "";
            string strPano_old = "";

            clsDB.setBeginTran(clsDB.DbCon);
            //clsDB.setBeginTran(pEdisAgentDb);

            try
            {
                #region // 응급환자 퇴실 시 진단 내역 EMIHDGOT 테이블 갱신

                SQL = " SELECT DGOTIDNO, DGOTINDT, DGOTINTM";
                SQL = SQL + ComNum.VBLF + "   FROM  KOSMOS_PMPA.NUR_ER_EMIHDGOT A";
                SQL = SQL + ComNum.VBLF + " WHERE DGOTINDT >= '20130101'";
                SQL = SQL + ComNum.VBLF + "   AND GBSEND IS NULL";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.DGOTIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.DGOTINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.DGOTINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + "   GROUP BY DGOTIDNO, DGOTINDT, DGOTINTM";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDGOTIDNO = clsNurse.NullToEmpty(dt.Rows[i]["DGOTIDNO"].ToString().Trim());
                        strDGOTINDT = clsNurse.NullToEmpty(dt.Rows[i]["DGOTINDT"].ToString().Trim());
                        strDGOTINTM = clsNurse.NullToEmpty(dt.Rows[i]["DGOTINTM"].ToString().Trim());

                        SQL = " DELETE EMIHDGOT@EDISAGENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE DGOTIDNO = '" + strDGOTIDNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINTM = '" + strDGOTINTM + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINDT = '" + strDGOTINDT + "' AND DGOTEMCD='C24C0083'  ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
                dt.Dispose();
                dt = null;


                SQL = "SELECT DGOTEMCD,DGOTIDNO,DGOTINDT,DGOTINTM,DGOTDIAG,GBSEND, DGOTDGGB ";
                SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.NUR_ER_EMIHDGOT A";
                SQL = SQL + ComNum.VBLF + " WHERE GBSEND IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.DGOTIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.DGOTINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.DGOTINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + "   AND DGOTINDT >= '20130101' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDGOTEMCD = clsNurse.NullToEmpty(dt.Rows[i]["DGOTEMCD"].ToString().Trim());
                        strDGOTIDNO = clsNurse.NullToEmpty(dt.Rows[i]["DGOTIDNO"].ToString().Trim());
                        strDGOTINDT = clsNurse.NullToEmpty(dt.Rows[i]["DGOTINDT"].ToString().Trim());
                        strDGOTINTM = clsNurse.NullToEmpty(dt.Rows[i]["DGOTINTM"].ToString().Trim());
                        strDGOTDIAG = clsNurse.NullToEmpty(dt.Rows[i]["DGOTDIAG"].ToString().Trim());
                        strDGOTDGGB = clsNurse.NullToEmpty(dt.Rows[i]["DGOTDGGB"].ToString().Trim());

                        if (VB.Len(strDGOTDIAG) > 6) strDGOTDIAG = VB.Left(strDGOTDIAG, 4);

                        SQL = " SELECT DGOTIDNO FROM EMIHDGOT@EDISAGENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE DGOTIDNO = '" + strDGOTIDNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINTM = '" + strDGOTINTM + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINDT = '" + strDGOTINDT + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTDIAG = '" + strDGOTDIAG + "' AND DGOTEMCD='C24C0083' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            SQL = " INSERT INTO EMIHDGOT@EDISAGENT (";
                            SQL = SQL + ComNum.VBLF + " DGOTEMCD,DGOTIDNO,DGOTINDT,DGOTINTM,";
                            SQL = SQL + ComNum.VBLF + " DGOTDIAG,DGOTSERL,DGOTDGGB) VALUES (";
                            SQL = SQL + ComNum.VBLF + " '" + strDGOTEMCD + "', '" + strDGOTIDNO + "', '" + strDGOTINDT + "', '" + strDGOTINTM + "',";
                            SQL = SQL + ComNum.VBLF + " '" + strDGOTDIAG + "', 0, '" + strDGOTDGGB + "') ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("Agent Pc에 자료 추가시 에러 발생함");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }


                            //'===============================================================================
                            //'김현욱 일단 막음(무한루프 때문에) 2009-09-07
                            if (strPano_old != strDGOTIDNO)
                            {
                                SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND = '', PTMISTAT = 'U'  ";
                                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strDGOTIDNO + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strDGOTINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strDGOTIDNO + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) SEQNO FROM NUR_ER_EMIHPTMI ";
                                SQL = SQL + ComNum.VBLF + "                                        WHERE PTMIINDT = '" + strDGOTINDT + "'  ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIINTM = '" + strDGOTINTM + "'  ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIIDNO = '" + strDGOTIDNO + "' )";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(pEdisAgentDb);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                strPano_old = strDGOTIDNO;
                            }
                            //'===============================================================================                            
                        }

                        SQL = " UPDATE NUR_ER_EMIHDGOT SET GBSEND  = 'Y' ";
                        SQL = SQL + ComNum.VBLF + " WHERE DGOTIDNO = '" + strDGOTIDNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINDT = '" + strDGOTINDT + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DGOTINTM = '" + strDGOTINTM + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND = ' ') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                    } // for
                }
                dt.Dispose();
                dt = null;

                #endregion

                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);       
                         
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAgentSend_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (pEdisAgentDb != null)
            //{
            //    clsDB.DisDBConnect(pEdisAgentDb);
            //}
        }

        private void mnuSendNow_01_Click(object sender, EventArgs e)
        {
            //INSERT_응급실처치내역
            INSERT_EMERGENCY_AID();
        }

        private void mnuSendNow_02_Click(object sender, EventArgs e)
        {

        }

        private void mnuSendNow_03_Click(object sender, EventArgs e)
        {
            Data_Display();
            //기록실_퇴원자_Select_now
            SELECT_EMRJOB_DSCH_NOW();
            FnTimer = 0;
        }

        private bool mnuSendNow_02Click()
        {
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nSeqNo = 0;
            string strDate = "";
            string strDate2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strDate2 = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-2).ToShortDateString();
            strDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-2).ToString("yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " SELECT PTNO FROM KOSMOS_OCS.OCS_EILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO IN (SELECT PTMIIDNO FROM NUR_ER_EMIHPTMI ";
                SQL = SQL + ComNum.VBLF + "                 WHERE PTMIINDT = '" + strDate + "' ";
                SQL = SQL + ComNum.VBLF + "                 GROUP BY PTMIIDNO) ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY PTNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PTNO"].ToString().Trim();

                        SQL = " SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM FROM NUR_ER_EMIHPTMI A";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIINDT  = '" + strDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                        SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                        SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strPTMIINDT = dt1.Rows[j]["PTMIINDT"].ToString().Trim();
                                strPTMIINTM = dt1.Rows[j]["PTMIINTM"].ToString().Trim();
                                nSeqNo = (int)VB.Val(dt1.Rows[j]["SEQNO"].ToString().Trim());


                                SQL = " INSERT INTO NUR_ER_EMIHDGOT (DGOTEMCD,DGOTIDNO,DGOTINDT,DGOTINTM,                       ";
                                SQL = SQL + ComNum.VBLF + " DGOTDIAG,DGOTSERL,GBSEND, DGOTDGGB)                                 ";
                                SQL = SQL + ComNum.VBLF + " SELECT 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "',        ";
                                SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',B.ILLCODED ILLCODE, 0, '',                    ";
                                SQL = SQL + ComNum.VBLF + "    DECODE(DGOTDGGB, NULL, '1', DGOTDGGB) DGOTDGGB                   ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.BAS_ILLS B                 ";
                                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPano + "'                                    ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')             ";
                                SQL = SQL + ComNum.VBLF + " AND A.ILLCODE = B.ILLCODE                                           ";
                                SQL = SQL + ComNum.VBLF + "   AND TRIM(B.ILLCODED) NOT IN(                                      ";
                                SQL = SQL + ComNum.VBLF + "                             SELECT DGOTDIAG FROM NUR_ER_EMIHDGOT    ";
                                SQL = SQL + ComNum.VBLF + "                              WHERE DGOTINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "                                AND DGOTINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "                                AND DGOTIDNO = '" + strPano + "')    ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND = '', PTMISTAT = 'U'  ";
                                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) SEQNO FROM NUR_ER_EMIHPTMI ";
                                SQL = SQL + ComNum.VBLF + "                                        WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIIDNO = '" + strPano + "' )   ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
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

        //응급실처치내역_INSERT
        private bool INSERT_EMERGENCY_AID()
        {
            bool rtVal = false;
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strIDNO = "";
            string strINDT = "";
            string strINTM = "";
            string strTRDT = "";
            string strTRTM = "";

            string strOTDT = "";
            string strOTTM = "";

            string strCode = "";
            string strORDERNO = "";
            string strTable_Name = "";
            string strBDATE = "";
            string strOK = "OK";

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region // 응급환자 검사/처치/수술 내역 EMIHTRPT 테이블 갱신

                SQL = " SELECT PTMIIDNO, PTMIINDT, PTMIINTM, PTMIOTDT, PTMIOTTM";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI A";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT >= '" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-30).ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT >= '20160222'";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT <= '" + Convert.ToDateTime(clsPublic.GstrSysDate).ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "   AND TRPT_SEND = '-' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(PTMIOTDT) IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + " GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM, PTMIOTDT, PTMIOTTM";
                SQL = SQL + ComNum.VBLF + " ORDER BY PTMIIDNO ASC, PTMIINDT ASC, PTMIINTM ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strIDNO = dt.Rows[i]["PTMIIDNO"].ToString().Trim();
                        strINDT = dt.Rows[i]["PTMIINDT"].ToString().Trim();
                        strOTDT = dt.Rows[i]["PTMIOTDT"].ToString().Trim();
                        if (strOTDT == "")
                        {
                            strOTDT = strINDT;
                        }
                        strINTM = dt.Rows[i]["PTMIINTM"].ToString().Trim();
                        strOTTM = dt.Rows[i]["PTMIOTTM"].ToString().Trim();


                        SQL = " SELECT A.SUCODE, B.BCODE, A.BUN, A.ORDERNO, A.BDATE, SUM(A.NAL) ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.BAS_SUN B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PTNO  = '" + strIDNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + strINDT + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOTDT + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT ";
                        SQL = SQL + ComNum.VBLF + "   AND B.BCODE IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN >= '28' AND A.BUN <= '73' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBSEND <> '*' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBIOE IN ('E','EI') ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.BCODE, A.BUN, A.ORDERNO, A.BDATE ";
                        SQL = SQL + ComNum.VBLF + " HAVING SUM(A.NAL) > 0 ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {

                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strCode = dt1.Rows[j]["BCODE"].ToString().Trim();
                                if (dt1.Rows[j]["BUN"].ToString().Trim() == "71") //'초음파
                                {
                                    if (strCode == "US22")
                                    {
                                        strCode = "US001";
                                    }
                                    else
                                    {
                                        strCode = "US002";
                                    }
                                }
                                strORDERNO = dt1.Rows[j]["ORDERNO"].ToString().Trim();
                                strBDATE = dt1.Rows[j]["BDATE"].ToString().Trim();

                                strTRDT = "-";
                                strTRTM = "-";
                                strTable_Name = "";


                                SQL = "SELECT TO_CHAR(ACTTIME,'YYYYMMDD') ACTDATE, TO_CHAR(ACTTIME,'HH24MI') ACTTIME";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER_ACT_ER A, KOSMOS_PMPA.BAS_SUN B";
                                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strIDNO + "'";
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + strINDT + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT";
                                SQL = SQL + ComNum.VBLF + "   AND B.BCODE = '" + strCode + "'";
                                SQL = SQL + ComNum.VBLF + "   AND (A.ORDERNO = " + strORDERNO + " OR A.ORDERNO IS NOT NULL)";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {

                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return rtVal;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    strTRDT = dt1.Rows[0]["ACTDATE"].ToString().Trim();
                                    strTRTM = dt1.Rows[0]["ACTTIME"].ToString().Trim();
                                }


                                if ((strTRDT == "-" || strTRDT == "") && strORDERNO != "")
                                {
                                    strTRDT = READ_STARTTIME(strIDNO, strBDATE, strORDERNO, strINDT + " " + strINTM, strOTDT + " " + strOTTM);
                                    if (strTRDT != "")
                                    {
                                        strTRTM = VB.Right(strTRDT, 4);
                                        strTRDT = VB.Left(strTRDT, 8);
                                    }
                                }

                                if (strTRDT == "") strTRDT = "-";
                                if (strTRTM == "") strTRTM = "-";


                                SQL = " SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHTRPT ";
                                SQL = SQL + ComNum.VBLF + " WHERE TRPTIDNO = '" + strIDNO + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRPTINDT = '" + strINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRPTINTM = '" + strINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRPTTRCD = '" + strCode + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRPTTRDT = '" + strTRDT + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRPTTRTM = '" + strTRTM + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {

                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return rtVal;
                                }

                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHTRPT (";
                                    SQL = SQL + ComNum.VBLF + " TRPTEMCD, TRPTIDNO, TRPTINDT, TRPTINTM,";
                                    SQL = SQL + ComNum.VBLF + " TRPTTRCD, TRPTTRDT, TRPTTRTM, TABLE_NAME, ";
                                    SQL = SQL + ComNum.VBLF + " ORDERNO, BDATE) VALUES ( ";
                                    SQL = SQL + ComNum.VBLF + " 'C24C0083','" + strIDNO + "', '" + strINDT + "','" + strINTM + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strCode + "','" + strTRDT + "','" + strTRTM + "','" + strTable_Name + "',";
                                    SQL = SQL + ComNum.VBLF + strORDERNO + ",TO_DATE('" + strBDATE + "','YYYY-MM-DD')) ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        strOK = "NO";
                                    }

                                    SQL = " UPDATE NUR_ER_EMIHPTMI SET TRPT_SEND = 'Y'  ";
                                    SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strINDT + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strINTM + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strIDNO + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        strOK = "NO";
                                    }
                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            } //for j
                        }
                    } //for i
                }
                dt.Dispose();
                dt = null;

                #endregion

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }


            clsDB.setBeginTran(clsDB.DbCon);
            //clsDB.setBeginTran(pEdisAgentDb);

            try
            {
                //'응급실에서 퇴실
                string strTRPTEMCD = "";
                string strTRPTIDNO = "";
                string strTRPTINDT = "";
                string strTRPTINTM = "";
                string strTRPTTRCD = "";
                string strTRPTTRDT = "";
                string strTRPTTRTM = "";
                string strROWID = "";

                string strPano_old = "";

                //strTRPTIDNO_OLD = "";
                //strTRPTIDDT_OLD = "";
                //strTRPTIDTM_OLD = "";

                SQL = " SELECT TRPTEMCD,TRPTIDNO,TRPTINDT,TRPTINTM,";
                SQL = SQL + ComNum.VBLF + " TRPTTRCD,TRPTTRDT,TRPTTRTM,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_EMIHTRPT A ";
                SQL = SQL + ComNum.VBLF + " WHERE TRPTINDT >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND TRPTINDT <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";
                //'2007-09-14 全 FULL SCAN 않하도록 처리
                SQL = SQL + ComNum.VBLF + "   AND TRPTINDT >= '20160222'";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.TRPTIDNO = B.PTMIIDNO";
                SQL = SQL + ComNum.VBLF + "     AND A.TRPTINDT = B.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "     AND A.TRPTINTM = B.PTMIINTM)";
                SQL = SQL + ComNum.VBLF + "   AND GBSEND IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND TRPTTRCD <> '999999' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY TRPTIDNO ASC, TRPTINDT ASC, TRPTINTM ASC     ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                strPano_old = "";
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strTRPTEMCD = dt.Rows[i]["TRPTEMCD"].ToString().Trim();
                        strTRPTIDNO = dt.Rows[i]["TRPTIDNO"].ToString().Trim();
                        strTRPTINDT = dt.Rows[i]["TRPTINDT"].ToString().Trim();
                        strTRPTINTM = dt.Rows[i]["TRPTINTM"].ToString().Trim();
                        strTRPTTRCD = dt.Rows[i]["TRPTTRCD"].ToString().Trim();
                        strTRPTTRDT = dt.Rows[i]["TRPTTRDT"].ToString().Trim();
                        strTRPTTRTM = dt.Rows[i]["TRPTTRTM"].ToString().Trim();
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();


                        if (strPano_old != strTRPTIDNO)
                        {
                            SQL = " DELETE EMIHTRPT@EDISAGENT ";
                            SQL = SQL + ComNum.VBLF + " WHERE TRPTIDNO = '" + strTRPTIDNO + "'";
                            SQL = SQL + ComNum.VBLF + "   AND TRPTINDT = '" + strTRPTINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND TRPTINTM = '" + strTRPTINTM + "' AND TRPTEMCD='C24C0083' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("에러발생함(EDISAGENT.EMIHTRPT)");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }

                        SQL = " INSERT INTO EMIHTRPT@EDISAGENT (";
                        SQL = SQL + ComNum.VBLF + " TRPTEMCD,TRPTIDNO,TRPTINDT,TRPTINTM,";
                        SQL = SQL + ComNum.VBLF + " TRPTTRCD,TRPTTRDT,TRPTTRTM) VALUES (";
                        SQL = SQL + ComNum.VBLF + " '" + strTRPTEMCD + "','" + strTRPTIDNO + "','" + strTRPTINDT + "','" + strTRPTINTM + "',";
                        SQL = SQL + ComNum.VBLF + " '" + strTRPTTRCD + "','" + strTRPTTRDT + "','" + strTRPTTRTM + "') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("에러발생함(EDISAGENT.EMIHTRPT)");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }

                        if (strPano_old != strTRPTIDNO)
                        {
                            SQL = " UPDATE EMIHPTMI@EDISAGENT SET ";
                            SQL = SQL + ComNum.VBLF + " PTMISTAT = 'U'";
                            SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strTRPTINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strTRPTINTM + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strTRPTIDNO + "' AND PTMIEMCD='C24C0083' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("에러발생함(EDISAGENT.EMIHTRPT)");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                            strPano_old = strTRPTIDNO;
                        }


                        SQL = " UPDATE KOSMOS_PMPA.NUR_ER_EMIHTRPT SET GBSEND  = 'Y' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("에러발생함(EDISAGENT.EMIHTRPT)");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }


                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        //응급환자퇴실진단_INSERT
        private bool INSERT_EM_PATIENT_LEAVEDIAG()
        {
            bool rtVal = false;
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nSeqNo = 0;
            string strDate = "";
            string strDate2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strDate2 = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-2).ToShortDateString();
            strDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-2).ToString("yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " SELECT PTNO FROM KOSMOS_OCS.OCS_EILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO IN (SELECT PTMIIDNO FROM NUR_ER_EMIHPTMI ";
                SQL = SQL + ComNum.VBLF + "                 WHERE PTMIINDT = '" + strDate + "' ";
                SQL = SQL + ComNum.VBLF + "                 GROUP BY PTMIIDNO) ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY PTNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PTNO"].ToString().Trim();

                        SQL = " SELECT MAX(SEQNO) SEQNO, PTMIIDNO, PTMIINDT, PTMIINTM FROM NUR_ER_EMIHPTMI A";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIINDT  = '" + strDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                        SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                        SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);

                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strPTMIINDT = dt1.Rows[j]["PTMIINDT"].ToString().Trim();
                                strPTMIINTM = dt1.Rows[j]["PTMIINTM"].ToString().Trim();
                                nSeqNo = (int)VB.Val(dt1.Rows[j]["SEQNO"].ToString().Trim());


                                SQL = " INSERT INTO NUR_ER_EMIHDGOT (DGOTEMCD,DGOTIDNO,DGOTINDT,DGOTINTM,                       ";
                                SQL = SQL + ComNum.VBLF + " DGOTDIAG,DGOTSERL,GBSEND, DGOTDGGB)                                 ";
                                SQL = SQL + ComNum.VBLF + " SELECT 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "',        ";
                                SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',B.ILLCODED ILLCODE, 0, '',                    ";
                                SQL = SQL + ComNum.VBLF + "    DECODE(DGOTDGGB, NULL, '1', DGOTDGGB) DGOTDGGB                   ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.BAS_ILLS B                 ";
                                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPano + "'                                    ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')             ";
                                SQL = SQL + ComNum.VBLF + " AND A.ILLCODE = B.ILLCODE                                           ";
                                SQL = SQL + ComNum.VBLF + "   AND TRIM(B.ILLCODED) NOT IN(                                      ";
                                SQL = SQL + ComNum.VBLF + "                             SELECT DGOTDIAG FROM NUR_ER_EMIHDGOT    ";
                                SQL = SQL + ComNum.VBLF + "                              WHERE DGOTINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "                                AND DGOTINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "                                AND DGOTIDNO = '" + strPano + "')    ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);

                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtVal;
                                }

                                SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND = '', PTMISTAT = 'U'  ";
                                SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) SEQNO FROM NUR_ER_EMIHPTMI ";
                                SQL = SQL + ComNum.VBLF + "                                        WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIINTM = '" + strPTMIINTM + "' ";
                                SQL = SQL + ComNum.VBLF + "                                          AND PTMIIDNO = '" + strPano + "' )   ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);

                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return rtVal;
                                }
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        //기록실_퇴원자_SELECT
        private void SELECT_EMRJOB_DSCH()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (string.Compare(clsPublic.GstrSysTime, "01:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "01:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "03:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "03:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "06:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "06:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "09:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "09:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "12:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "15:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "15:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "17:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "17:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "21:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "21:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
            else if (string.Compare(clsPublic.GstrSysTime, "23:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "23:30") <= 0)
            {
                INSERT_DX_OPERATION();
            }
        }

        //진단_수술_INSERT
        private bool INSERT_DX_OPERATION()
        {
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            int k = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strInDate = "";
            string strOutDate = "";
            string strPano = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";
            string[] strSanCode = new string[10];
            string striLLCode = "";
            string strDGGB = "";

            string strDCTP = "";
            string strTHCD = "";
            //string strDCDT = "";
            //string strDCTM = "";
            string strTm = "";
            string strHSDT = "";
            string strHSTM = "";
            string strTTime = "";
            //string strNURTime = "";
            string strORDERNO = "";
            string strBDATE = "";
            string strOPDT = "";
            string strOPTM = "";
            string strCode = "";
            string strSTAT = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            //clsDB.setBeginTran(pEdisAgentDb);

            try
            {

                //'/-----------------------------퇴원자 중 전송대상자 중 미전송자 조회
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,                                                   ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, PANO, TMODEL, PTMIDCTP, PTMIDCDT, PTMIDCTM, THCD  ";
                SQL = SQL + ComNum.VBLF + "  FROM MID_SUMMARY                                                                          ";
                SQL = SQL + ComNum.VBLF + " WHERE OUTDATE >= TO_DATE('2012-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('2013-07-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (GBPTMI IS NULL OR GBPTMI = ' ' or GBPTMI = '' )                                        ";
                SQL = SQL + ComNum.VBLF + "   AND IPKYNG = '1'                                                            "; //'응급실

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strInDate = dt.Rows[i]["INDATE"].ToString().Trim();
                        if (VB.IsDate(strInDate) == true)
                        {
                            strInDate = Convert.ToDateTime(strInDate).ToString("yyyyMMdd");
                        }
                        strOutDate = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDCTP = dt.Rows[i]["PTMIDCTP"].ToString().Trim();
                        strTHCD = dt.Rows[i]["THCD"].ToString().Trim();
                        strTTime = dt.Rows[i]["PTMIDCTM"].ToString().Trim();

                        strSTAT = "";


                        SQL = " SELECT PTMISTAT, PTMIINDT, PTMIINTM  FROM NUR_ER_EMIHPTMI A";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strInDate + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "'                       ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIEMRT IN ('31','32','33','34')                  ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                        SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                        SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                        SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO DESC"; ;

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);

                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strPTMIINDT = dt1.Rows[0]["PTMIINDT"].ToString().Trim();
                            strPTMIINTM = dt1.Rows[0]["PTMIINTM"].ToString().Trim();
                            strSTAT = dt1.Rows[0]["PTMISTAT"].ToString().Trim();

                            SQL = " DELETE FROM EMIHDGDC@EDISAGENT WHERE DGDCEMCD = 'C24C0083' ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCIDNO  = '" + strPano + "'  ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCINDT  = '" + strPTMIINDT + "'  ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCINTM  = '" + strPTMIINTM + "'  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }


                            SQL = " DELETE FROM NUR_ER_EMIHDGDC WHERE DGDCEMCD = 'C24C0083' ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCIDNO  = '" + strPano + "'  ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCINDT  = '" + strPTMIINDT + "'  ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCINTM  = '" + strPTMIINTM + "'  ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }



                            SQL = " SELECT DISTINCT B.ILLCODED DIAGNOSIS1, DECODE(SEQNO, 1, 1, 2) SEQNO ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.MID_DIAGNOSIS A, KOSMOS_PMPA.BAS_ILLS B ";
                            SQL = SQL + ComNum.VBLF + " WHERE A.PANO  = '" + strPano + "'                               ";
                            SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD')    ";
                            SQL = SQL + ComNum.VBLF + "   AND A.DIAGNOSIS1 = B.ILLCODE ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO    ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    striLLCode = dt2.Rows[j]["DIAGNOSIS1"].ToString().Trim();
                                    for (k = 0; k < striLLCode.Length; k++)
                                    {
                                        if (VB.Mid(striLLCode, k, 1) == "+")
                                        {
                                            striLLCode = VB.Pstr(striLLCode, "+", 1);
                                            break;
                                        }
                                        else if (VB.Mid(striLLCode, k, 1) == "*")
                                        {
                                            striLLCode = VB.Pstr(striLLCode, "*", 1);
                                            break;
                                        }
                                    } //for k

                                    strDGGB = dt2.Rows[j]["SEQNO"].ToString().Trim();


                                    SQL = " INSERT INTO EMIHDGDC@EDISAGENT (DGDCEMCD,DGDCIDNO,DGDCINDT,                ";
                                    SQL = SQL + ComNum.VBLF + "       DGDCINTM,DGDCDIAG,DGDCSERL, DGDCDGGB) VALUES (  ";
                                    SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',                                ";
                                    SQL = SQL + ComNum.VBLF + " '" + striLLCode + "',0,'" + strDGGB + "' )            ";

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("진단내역(EMIHDGDC) 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHDGDC(DGDCEMCD,DGDCIDNO,DGDCINDT,       ";
                                    SQL = SQL + ComNum.VBLF + "       DGDCINTM,DGDCDIAG,DGDCSERL, DGDCDGGB) VALUES (  ";
                                    SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',                                ";
                                    SQL = SQL + ComNum.VBLF + " '" + striLLCode + "',0, '" + strDGGB + "' )           ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("진단내역(EMIHDGDC) 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }


                                    SQL = " UPDATE NUR_ER_EMIHDGDC SET GBSEND  = 'Y'                                ";
                                    SQL = SQL + ComNum.VBLF + " WHERE DGDCIDNO = '" + strPano + "'                  ";
                                    SQL = SQL + ComNum.VBLF + "   AND DGDCINDT = '" + strPTMIINDT + "'              ";
                                    SQL = SQL + ComNum.VBLF + "   AND DGDCINTM = '" + strPTMIINTM + "'              ";
                                    SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND = ' ')              ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("에러발생함(EMIHDGDC)");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            //'/----------------------수술내역 보내기
                            SQL = " SELECT OPERATION FROM MID_OP      ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "'                               ";
                            SQL = SQL + ComNum.VBLF + "   AND OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD')    ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO                                                ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT  ";
                                    SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'       ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'   ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'   ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + dt2.Rows[j]["OPERATION"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD = 'C24C0083' ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count == 0)
                                    {
                                        SQL = " INSERT INTO EMIHOPPT@EDISAGENT ( ";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD,OPPTIDNO,OPPTINDT,OPPTINTM,OPPTOPCD ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', ";
                                        SQL = SQL + ComNum.VBLF + " '" + strPTMIINDT + "', '" + strPTMIINTM + "',      ";
                                        SQL = SQL + ComNum.VBLF + " '" + dt2.Rows[j]["OPERATION"].ToString().Trim() + "' )   ";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);

                                            ComFunc.MsgBox("수술내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }

                                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT ( ";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD,OPPTIDNO,OPPTINDT,OPPTINTM,OPPTOPCD ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', ";
                                        SQL = SQL + ComNum.VBLF + " '" + strPTMIINDT + "', '" + strPTMIINTM + "',      ";
                                        SQL = SQL + ComNum.VBLF + " '" + dt2.Rows[j]["OPERATION"].ToString().Trim() + "' )   ";

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);

                                            ComFunc.MsgBox("수술내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                    }
                                    dt3.Dispose();
                                    dt3 = null;
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            //'==================================================
                            //'2014년도 부터 모든 처치검사전송
                            //'/--------------------입원시 처치내역 읽어오기

                            string strConvPTMIINDT = ComFunc.FormatStrToDateEx(strPTMIINDT.Replace("-",""), "D", "-");

                            if (Convert.ToDateTime(strOutDate) > Convert.ToDateTime(strConvPTMIINDT).AddDays(1))
                            {
                                SQL = " SELECT A.SUCODE, B.BCODE, SUM(A.NAL) NAL, A.GBIOE ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.BAS_SUN B";
                                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BUN >= '23' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BUN <= '73' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT).AddDays(2), "D");
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE IS NULL OR A.GBIOE = 'I' OR GBIOE = '')";
                                SQL = SQL + ComNum.VBLF + "   AND B.BCODE IS NOT NULL ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.BCODE, A.GBIOE ";
                                SQL = SQL + ComNum.VBLF + " HAVING SUM(A.NAL) > 0 ";

                                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(pEdisAgentDb);
                                    clsDB.setRollbackTran(clsDB.DbCon);

                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return rtnVal;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strCode = dt2.Rows[j]["BCODE"].ToString().Trim();

                                        SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT ";
                                        SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + strCode + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD='C24C0083' ";

                                        SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);

                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            return rtnVal;
                                        }

                                        if (dt3.Rows.Count == 0)
                                        {
                                            SQL = " INSERT INTO EMIHOPPT@EDISAGENT (";
                                            SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, OPPTOPCD ";
                                            SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                            SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                            SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "', '" + strCode + "')";

                                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                            if (SqlErr != "")
                                            {
                                                //clsDB.setRollbackTran(pEdisAgentDb);
                                                clsDB.setRollbackTran(clsDB.DbCon);

                                                ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                                Cursor.Current = Cursors.Default;
                                                return rtnVal;
                                            }

                                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT (";
                                            SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, OPPTOPCD ";
                                            SQL = SQL + ComNum.VBLF + " ) VALUES (";
                                            SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                            SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "', '" + strCode + "')";

                                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                            if (SqlErr != "")
                                            {
                                                //clsDB.setRollbackTran(pEdisAgentDb);
                                                clsDB.setRollbackTran(clsDB.DbCon);

                                                ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                                Cursor.Current = Cursors.Default;
                                                return rtnVal;
                                            }
                                        }
                                        dt3.Dispose();
                                        dt3 = null;
                                    } //for j
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }


                            SQL = " SELECT A.SUCODE, B.BCODE, SUM(A.NAL) NAL, A.GBIOE, A.ORDERNO, A.BDATE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.BAS_SUN B";
                            SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BUN >= '23' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BUN <= '73' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT), "D");
                            if (Convert.ToDateTime(strOutDate) <= Convert.ToDateTime(strConvPTMIINDT).AddDays(1))
                            {
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT).AddDays(1), "D");
                            }
                            SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE IS NULL OR A.GBIOE = 'I' OR GBIOE = '')";
                            SQL = SQL + ComNum.VBLF + "   AND B.BCODE IS NOT NULL ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.BCODE, A.GBIOE, A.ORDERNO, A.BDATE ";
                            SQL = SQL + ComNum.VBLF + " HAVING SUM(A.NAL) > 0 ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    strORDERNO = dt2.Rows[j]["ORDERNO"].ToString().Trim();
                                    strBDATE = dt2.Rows[j]["BDATE"].ToString().Trim();
                                    strCode = dt2.Rows[j]["BCODE"].ToString().Trim();

                                    strOPDT = "-";
                                    strOPTM = "-";

                                    SQL = "SELECT TO_CHAR(ACTTIME,'YYYYMMDD') ACTDATE, TO_CHAR(ACTTIME,'HH24MI') ACTTIME";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER_ACT_ER A, KOSMOS_PMPA.BAS_SUN B";
                                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPano + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + strPTMIINDT + "','YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT";
                                    SQL = SQL + ComNum.VBLF + "   AND B.BCODE = '" + strCode + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND (A.ORDERNO = " + strORDERNO + " OR A.ORDERNO IS NOT NULL)";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count > 0)
                                    {
                                        strOPDT = dt3.Rows[0]["ACTDATE"].ToString().Trim();
                                        strOPTM = dt3.Rows[0]["ACTTIME"].ToString().Trim();
                                    }

                                    dt3.Dispose();
                                    dt3 = null;


                                    if ((strOPDT == "-" || strOPDT == "") && strORDERNO != "")
                                    {
                                        strOPDT = READ_STARTTIME(strPano, strBDATE, strORDERNO, strPTMIINDT + " " + strPTMIINTM);
                                        if (strOPDT != "")
                                        {
                                            strOPTM = VB.Right(strOPDT, 4);
                                            strOPDT = VB.Left(strOPDT, 8);
                                        }
                                    }

                                    if (strOPDT == "") strOPDT = "-";

                                    if (strOPTM == "") strOPTM = "-";


                                    SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT ";
                                    SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + strCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD = 'C24C0083' ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);

                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count == 0)
                                    {
                                        SQL = " INSERT INTO EMIHOPPT@EDISAGENT (";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, ";
                                        SQL = SQL + ComNum.VBLF + " OPPTOPCD, OPPTOPDT, OPPTOPTM ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', '" + strPTMIINTM + "',";
                                        SQL = SQL + ComNum.VBLF + " '" + strCode + "','" + strOPDT + "','" + strOPTM + "')";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);

                                            ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }


                                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT (";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM,";
                                        SQL = SQL + ComNum.VBLF + " OPPTOPCD, OPPTOPDT, OPPTOPTM ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', '" + strPTMIINTM + "',";
                                        SQL = SQL + ComNum.VBLF + " '" + strCode + "','" + strOPDT + "','" + strOPTM + "' )";

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);

                                            ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                    }
                                    dt3.Dispose();
                                    dt3 = null;
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            switch (dt.Rows[i]["TMODEL"].ToString().Trim())
                            {
                                case "1":
                                    strTm = "1";
                                    break;
                                case "2":
                                    strTm = "2";
                                    break;
                                case "3":
                                    strTm = "3";
                                    break;
                                case "4":
                                    strTm = "5";
                                    break;
                                case "5":
                                    strTm = "4";
                                    break;
                                default:
                                    strTm = "8";
                                    break;
                            }


                            strHSDT = "";
                            strHSTM = "";
                            strHSDT = READ_IPWONTIME(strPano, strPTMIINDT);
                            if (strHSDT != "")
                            {
                                if (strHSDT.IndexOf("월") != -1)
                                {
                                    strHSDT = VB.Replace(strHSDT, "년", "-");
                                    strHSDT = VB.Replace(strHSDT, "월", "-");
                                    strHSDT = VB.Replace(strHSDT, "일", "");
                                    strHSDT = VB.Replace(strHSDT, " ", "");
                                }
                                strHSTM = VB.Right(strHSDT, 5);
                                strHSDT = VB.Left(strHSDT, 10);

                                if (VB.IsDate(strHSTM) && VB.IsDate(strHSDT))
                                {
                                    strHSTM = VB.Replace(strHSTM, ":", "");
                                    strHSTM = VB.Replace(strHSTM, "-", "");
                                    if (VB.Len(strHSTM) < 4)
                                    {
                                        strHSTM = ComFunc.SetAutoZero(strHSTM, 4);
                                    }
                                    strHSDT = VB.Replace(strHSDT, "-", "");
                                    strHSDT = VB.Replace(strHSDT, "/", "");
                                }
                                else
                                {
                                    strHSTM = "-";
                                    strHSDT = "-";
                                }
                            }
                            else
                            {
                                strHSTM = "-";
                                strHSDT = "-";
                            }

                            SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND = '', ";
                            if (strSTAT == "O")
                            {
                                SQL = SQL + ComNum.VBLF + " PTMISTAT = 'O',  ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " PTMISTAT = 'U',  ";
                            }
                            SQL = SQL + ComNum.VBLF + "     PTMIDCRT = '" + strTm + "', ";
                            SQL = SQL + ComNum.VBLF + "     PTMIDCDT = '" + Convert.ToDateTime(strOutDate).ToString("yyyyMMdd") + "', ";
                            SQL = SQL + ComNum.VBLF + "     PTMIDCTM = '" + strTTime + "', ";
                            if (string.Compare(strPTMIINDT, "20160222") >= 0)
                            {
                                SQL = SQL + ComNum.VBLF + "    PTMIHSDT = '" + VB.Left(strHSDT, 8) + "', ";
                                SQL = SQL + ComNum.VBLF + "    PTMIHSTM = '" + strHSTM + "', ";
                            }
                            SQL = SQL + ComNum.VBLF + "    PTMIDCTP = '" + strDCTP + "', ";
                            SQL = SQL + ComNum.VBLF + "    PTMIDCCD = '" + strTHCD + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) SEQNO FROM NUR_ER_EMIHPTMI ";
                            SQL = SQL + ComNum.VBLF + "                                        WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "                                          AND PTMIINTM = '" + strPTMIINTM + "'";
                            SQL = SQL + ComNum.VBLF + "                                          AND PTMIIDNO = '" + strPano + "' ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);

                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;


                        SQL = " UPDATE MID_SUMMARY ";
                        SQL = SQL + ComNum.VBLF + " SET GBPTMI = 'Y' ";
                        //'SQL = SQL + vbCr + " PTMIDCDT = '" + Replace(strOutDate, " - ", "") + "', "
                        //'SQL = SQL + vbCr + " PTMIDCTM = '" + Str
                        SQL = SQL + ComNum.VBLF + " WHERE OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBPTMI IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND IPKYNG = '1' "; //'응급실

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);

                            ComFunc.MsgBox("에러 발생함(MID_SUMMARY)");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    } //for i
                }
                dt.Dispose();
                dt = null;


                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        //기록실_퇴원자_SELECT_NOW
        private void SELECT_EMRJOB_DSCH_NOW()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            //진단_수술_inesrt
            INSERT_DX_OPERATION_NOW();
        }

        //진단_수술_INSERT_NOW
        private bool INSERT_DX_OPERATION_NOW()
        {
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            int k = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strInDate = "";
            string strOutDate = "";
            string strPano = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";
            string[] strSanCode = new string[10];
            string striLLCode = "";
            string strDGGB = "";

            string strDCTP = "";
            string strTHCD = "";
            //string strDCDT = "";
            //string strDCTM = "";
            string strTm = "";
            string strHSDT = "";
            string strHSTM = "";
            string strTTime = "";
            //string strNURTime = "";
            string strORDERNO = "";
            string strBDATE = "";
            string strOPDT = "";
            string strOPTM = "";
            string strCode = "";
            string strSTAT = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            //clsDB.setBeginTran(pEdisAgentDb);

            try
            {

                //'/-----------------------------퇴원자 중 전송대상자 중 미전송자 조회
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,                                                   ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, PANO, TMODEL, PTMIDCTP, PTMIDCDT, PTMIDCTM, THCD  ";
                SQL = SQL + ComNum.VBLF + "  FROM MID_SUMMARY                                                                          ";
                SQL = SQL + ComNum.VBLF + " WHERE OUTDATE >= TO_DATE('2012-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('2013-07-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (GBPTMI IS NULL OR GBPTMI = ' ' or GBPTMI = '' )                                        ";
                SQL = SQL + ComNum.VBLF + "   AND IPKYNG = '1'                                                            "; //'응급실
                SQL = SQL + ComNum.VBLF + "   AND PANO NOT IN (" + NOTPANO + ")";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pEdisAgentDb);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strInDate = dt.Rows[i]["INDATE"].ToString().Trim();
                        strOutDate = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDCTP = dt.Rows[i]["PTMIDCTP"].ToString().Trim();
                        strTHCD = dt.Rows[i]["THCD"].ToString().Trim();
                        strTTime = dt.Rows[i]["PTMIDCTM"].ToString().Trim();

                        strSTAT = "";


                        SQL = " SELECT PTMISTAT, PTMIINDT, PTMIINTM  FROM NUR_ER_EMIHPTMI A";
                        SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + Convert.ToDateTime(strInDate).ToString("yyyyMMdd") + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "'                       ";
                        SQL = SQL + ComNum.VBLF + "   AND PTMIEMRT IN ('31','32','33','34')                  ";
                        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS ";
                        SQL = SQL + ComNum.VBLF + " ( SELECT * FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI_NOTSEND B ";
                        SQL = SQL + ComNum.VBLF + "   WHERE A.PTMIIDNO = B.PTMIIDNO";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINDT = B.PTMIINDT";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTMIINTM = B.PTMIINTM)";
                        SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO DESC"; ;

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strPTMIINDT = dt1.Rows[0]["PTMIINDT"].ToString().Trim();
                            strPTMIINTM = dt1.Rows[0]["PTMIINTM"].ToString().Trim();
                            strSTAT = dt1.Rows[0]["PTMISTAT"].ToString().Trim();

                            SQL = " DELETE FROM EMIHDGDC@EDISAGENT WHERE DGDCEMCD = 'C24C0083' ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCIDNO  = '" + strPano + "'  ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCINDT  = '" + strPTMIINDT + "'  ";
                            SQL = SQL + ComNum.VBLF + "              AND DGDCINTM  = '" + strPTMIINTM + "'  ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("진단내역(EMIHDGDC)초기화시 에러 발생함");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }


                            SQL = " DELETE FROM NUR_ER_EMIHDGDC WHERE DGDCEMCD = 'C24C0083' ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCIDNO  = '" + strPano + "'  ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCINDT  = '" + strPTMIINDT + "'  ";
                            SQL = SQL + ComNum.VBLF + "           AND DGDCINTM  = '" + strPTMIINTM + "'  ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("진단내역(EMIHDGDC)초기화시 에러 발생함");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }



                            SQL = " SELECT DISTINCT B.ILLCODED DIAGNOSIS1, DECODE(SEQNO, 1, 1, 2) SEQNO ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.MID_DIAGNOSIS A, KOSMOS_PMPA.BAS_ILLS B ";
                            SQL = SQL + ComNum.VBLF + " WHERE A.PANO  = '" + strPano + "'                               ";
                            SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD')    ";
                            SQL = SQL + ComNum.VBLF + "   AND A.DIAGNOSIS1 = B.ILLCODE ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO    ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    striLLCode = dt2.Rows[j]["DIAGNOSIS1"].ToString().Trim();
                                    for (k = 0; k < striLLCode.Length; k++)
                                    {
                                        if (VB.Mid(striLLCode, k, 1) == "+")
                                        {
                                            striLLCode = VB.Pstr(striLLCode, "+", 1);
                                            break;
                                        }
                                        else if (VB.Mid(striLLCode, k, 1) == "*")
                                        {
                                            striLLCode = VB.Pstr(striLLCode, "*", 1);
                                            break;
                                        }
                                    } //for k

                                    strDGGB = dt2.Rows[j]["SEQNO"].ToString().Trim();


                                    SQL = " INSERT INTO EMIHDGDC@EDISAGENT (DGDCEMCD,DGDCIDNO,DGDCINDT,                ";
                                    SQL = SQL + ComNum.VBLF + "       DGDCINTM,DGDCDIAG,DGDCSERL, DGDCDGGB) VALUES (  ";
                                    SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',                                ";
                                    SQL = SQL + ComNum.VBLF + " '" + striLLCode + "',0,'" + strDGGB + "' )            ";

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("진단내역(EMIHDGDC) 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }

                                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHDGDC(DGDCEMCD,DGDCIDNO,DGDCINDT,       ";
                                    SQL = SQL + ComNum.VBLF + "       DGDCINTM,DGDCDIAG,DGDCSERL, DGDCDGGB) VALUES (  ";
                                    SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                    SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "',                                ";
                                    SQL = SQL + ComNum.VBLF + " '" + striLLCode + "',0, '" + strDGGB + "' )           ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("진단내역(EMIHDGDC) 추가시 에러 발생함");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }


                                    SQL = " UPDATE NUR_ER_EMIHDGDC SET GBSEND  = 'Y'                                ";
                                    SQL = SQL + ComNum.VBLF + " WHERE DGDCIDNO = '" + strPano + "'                  ";
                                    SQL = SQL + ComNum.VBLF + "   AND DGDCINDT = '" + strPTMIINDT + "'              ";
                                    SQL = SQL + ComNum.VBLF + "   AND DGDCINTM = '" + strPTMIINTM + "'              ";
                                    SQL = SQL + ComNum.VBLF + "   AND (GBSEND IS NULL OR GBSEND = ' ')              ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("에러발생함(EMIHDGDC)");
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            //'/----------------------수술내역 보내기
                            SQL = " SELECT OPERATION FROM MID_OP      ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "'                               ";
                            SQL = SQL + ComNum.VBLF + "   AND OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD')    ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO                                                ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT ";
                                    SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'       ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'   ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'   ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + dt.Rows[j]["OPERATION"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD = 'C24C0083' ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count == 0)
                                    {
                                        SQL = " INSERT INTO EMIHOPPT@EDISAGENT ( ";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD,OPPTIDNO,OPPTINDT,OPPTINTM,OPPTOPCD ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', ";
                                        SQL = SQL + ComNum.VBLF + " '" + strPTMIINDT + "', '" + strPTMIINTM + "',      ";
                                        SQL = SQL + ComNum.VBLF + " '" + dt2.Rows[j]["OPERATION"].ToString().Trim() + "' )   ";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            
                                            ComFunc.MsgBox("수술내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }

                                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT ( ";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD,OPPTIDNO,OPPTINDT,OPPTINTM,OPPTOPCD ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', ";
                                        SQL = SQL + ComNum.VBLF + " '" + strPTMIINDT + "', '" + strPTMIINTM + "',      ";
                                        SQL = SQL + ComNum.VBLF + " '" + dt2.Rows[j]["OPERATION"].ToString().Trim() + "' )   ";

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            
                                            ComFunc.MsgBox("수술내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                    }
                                    dt3.Dispose();
                                    dt3 = null;
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            //'==================================================
                            //'2014년도 부터 모든 처치검사전송
                            //'/--------------------입원시 처치내역 읽어오기

                            string strConvPTMIINDT = ComFunc.FormatStrToDateEx(strPTMIINDT.Replace("-", ""), "D", "-");

                            if (Convert.ToDateTime(strOutDate) > Convert.ToDateTime(strConvPTMIINDT).AddDays(1))
                            {
                                SQL = " SELECT A.SUCODE, B.BCODE, SUM(A.NAL) NAL, A.GBIOE ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.BAS_SUN B";
                                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BUN >= '23' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BUN <= '73' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT).AddDays(2), "D");
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE IS NULL OR A.GBIOE = 'I' OR GBIOE = '')";
                                SQL = SQL + ComNum.VBLF + "   AND B.BCODE IS NOT NULL ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.BCODE, A.GBIOE ";
                                SQL = SQL + ComNum.VBLF + " HAVING SUM(A.NAL) > 0 ";

                                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    //clsDB.setRollbackTran(pEdisAgentDb);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return rtnVal;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strCode = dt2.Rows[j]["BCODE"].ToString().Trim();

                                        SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT ";
                                        SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + strCode + "'";
                                        SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD = 'C24C0083' ";

                                        SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            return rtnVal;
                                        }

                                        if (dt3.Rows.Count == 0)
                                        {
                                            SQL = " INSERT INTO EMIHOPPT@EDISAGENT (";
                                            SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, OPPTOPCD ";
                                            SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                            SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                            SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "', '" + strCode + "')";

                                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                            if (SqlErr != "")
                                            {
                                                //clsDB.setRollbackTran(pEdisAgentDb);
                                                clsDB.setRollbackTran(clsDB.DbCon);
                                                
                                                ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                                Cursor.Current = Cursors.Default;
                                                return rtnVal;
                                            }

                                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT (";
                                            SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, OPPTOPCD ";
                                            SQL = SQL + ComNum.VBLF + " ) VALUES (";
                                            SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', ";
                                            SQL = SQL + ComNum.VBLF + " '" + strPTMIINTM + "', '" + strCode + "')";

                                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                            if (SqlErr != "")
                                            {
                                                //clsDB.setRollbackTran(pEdisAgentDb);
                                                clsDB.setRollbackTran(clsDB.DbCon);
                                                
                                                ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                                Cursor.Current = Cursors.Default;
                                                return rtnVal;
                                            }
                                        }
                                        dt3.Dispose();
                                        dt3 = null;
                                    } //for j
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }


                            SQL = " SELECT A.SUCODE, B.BCODE, SUM(A.NAL) NAL, A.GBIOE, A.ORDERNO, A.BDATE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.BAS_SUN B";
                            SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BUN >= '23' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BUN <= '73' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT), "D");
                            if (Convert.ToDateTime(strOutDate) <= Convert.ToDateTime(strConvPTMIINDT).AddDays(1))
                            {
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= " + ComFunc.ConvOraToDate(Convert.ToDateTime(strConvPTMIINDT).AddDays(1), "D");
                            }
                            SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE IS NULL OR A.GBIOE = 'I' OR GBIOE = '')";
                            SQL = SQL + ComNum.VBLF + "   AND B.BCODE IS NOT NULL ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY A.SUCODE, B.BCODE, A.GBIOE, A.ORDERNO, A.BDATE ";
                            SQL = SQL + ComNum.VBLF + " HAVING SUM(A.NAL) > 0 ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    strORDERNO = dt2.Rows[j]["ORDERNO"].ToString().Trim();
                                    strBDATE = dt2.Rows[j]["BDATE"].ToString().Trim();
                                    strCode = dt2.Rows[j]["BCODE"].ToString().Trim();

                                    strOPDT = "-";
                                    strOPTM = "-";

                                    SQL = "SELECT TO_CHAR(ACTTIME,'YYYYMMDD') ACTDATE, TO_CHAR(ACTTIME,'HH24MI') ACTTIME";
                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER_ACT_ER A, KOSMOS_PMPA.BAS_SUN B";
                                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPano + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + strPTMIINDT + "','YYYY-MM-DD')";
                                    SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT";
                                    SQL = SQL + ComNum.VBLF + "   AND B.BCODE = '" + strCode + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND (A.ORDERNO = " + strORDERNO + " OR A.ORDERNO IS NOT NULL)";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count > 0)
                                    {
                                        strOPDT = dt3.Rows[0]["ACTDATE"].ToString().Trim();
                                        strOPTM = dt3.Rows[0]["ACTTIME"].ToString().Trim();
                                    }

                                    dt3.Dispose();
                                    dt3 = null;


                                    if ((strOPDT == "-" || strOPDT == "") && strORDERNO != "")
                                    {
                                        strOPDT = READ_STARTTIME(strPano, strBDATE, strORDERNO, strPTMIINDT + " " + strPTMIINTM);
                                        if (strOPDT != "")
                                        {
                                            strOPTM = VB.Right(strOPDT, 4);
                                            strOPDT = VB.Left(strOPDT, 8);
                                        }
                                    }

                                    if (strOPDT == "") strOPDT = "-";

                                    if (strOPTM == "") strOPTM = "-";


                                    SQL = " SELECT OPPTIDNO FROM EMIHOPPT@EDISAGENT ";
                                    SQL = SQL + ComNum.VBLF + " WHERE OPPTIDNO = '" + strPano + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINDT = '" + strPTMIINDT + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTINTM = '" + strPTMIINTM + "'";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTOPCD = '" + strCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND OPPTEMCD = 'C24C0083' ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        //clsDB.setRollbackTran(pEdisAgentDb);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return rtnVal;
                                    }

                                    if (dt3.Rows.Count == 0)
                                    {
                                        SQL = " INSERT INTO EMIHOPPT@EDISAGENT (";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM, ";
                                        SQL = SQL + ComNum.VBLF + " OPPTOPCD, OPPTOPDT, OPPTOPTM ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (  ";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', '" + strPTMIINTM + "',";
                                        SQL = SQL + ComNum.VBLF + " '" + strCode + "','" + strOPDT + "','" + strOPTM + "')";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            
                                            ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }


                                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_EMIHOPPT (";
                                        SQL = SQL + ComNum.VBLF + " OPPTEMCD, OPPTIDNO, OPPTINDT, OPPTINTM,";
                                        SQL = SQL + ComNum.VBLF + " OPPTOPCD, OPPTOPDT, OPPTOPTM ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES (";
                                        SQL = SQL + ComNum.VBLF + " 'C24C0083', '" + strPano + "', '" + strPTMIINDT + "', '" + strPTMIINTM + "',";
                                        SQL = SQL + ComNum.VBLF + " '" + strCode + "','" + strOPDT + "','" + strOPTM + "' )";

                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            //clsDB.setRollbackTran(pEdisAgentDb);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            
                                            ComFunc.MsgBox("처치/검사내역(EMIHOPPT) 추가시 에러 발생함");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }
                                    }
                                    dt3.Dispose();
                                    dt3 = null;
                                } //for j
                            }
                            dt2.Dispose();
                            dt2 = null;


                            switch (dt.Rows[i]["TMODEL"].ToString().Trim())
                            {
                                case "1":
                                    strTm = "1";
                                    break;
                                case "2":
                                    strTm = "2";
                                    break;
                                case "3":
                                    strTm = "3";
                                    break;
                                case "4":
                                    strTm = "5";
                                    break;
                                case "5":
                                    strTm = "4";
                                    break;
                                default:
                                    strTm = "8";
                                    break;
                            }


                            strHSDT = "";
                            strHSTM = "";
                            strHSDT = READ_IPWONTIME(strPano, strPTMIINDT);
                            if (strHSDT != "")
                            {
                                if (strHSDT.IndexOf("월") != -1)
                                {
                                    strHSDT = VB.Replace(strHSDT, "년", "-");
                                    strHSDT = VB.Replace(strHSDT, "월", "-");
                                    strHSDT = VB.Replace(strHSDT, "일", "");
                                    strHSDT = VB.Replace(strHSDT, " ", "");
                                }
                                strHSTM = VB.Right(strHSDT, 5);
                                strHSDT = VB.Left(strHSDT, 10);

                                if (VB.IsDate(strHSTM) && VB.IsDate(strHSDT))
                                {
                                    strHSTM = VB.Replace(strHSTM, ":", "");
                                    strHSTM = VB.Replace(strHSTM, "-", "");
                                    if (VB.Len(strHSTM) < 4)
                                    {
                                        strHSTM = ComFunc.SetAutoZero(strHSTM, 4);
                                    }
                                    strHSDT = VB.Replace(strHSDT, "-", "");
                                    strHSDT = VB.Replace(strHSDT, "/", "");
                                }
                                else
                                {
                                    strHSTM = "-";
                                    strHSDT = "-";
                                }
                            }
                            else
                            {
                                strHSTM = "-";
                                strHSDT = "-";
                            }

                            SQL = " UPDATE NUR_ER_EMIHPTMI SET GBSEND = '', ";
                            if (strSTAT == "O")
                            {
                                SQL = SQL + ComNum.VBLF + " PTMISTAT = 'O',  ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " PTMISTAT = 'U',  ";
                            }
                            SQL = SQL + ComNum.VBLF + "     PTMIDCRT = '" + strTm + "', ";
                            SQL = SQL + ComNum.VBLF + "     PTMIDCDT = '" + Convert.ToDateTime(strOutDate).ToString("yyyyMMdd") + "', ";
                            SQL = SQL + ComNum.VBLF + "     PTMIDCTM = '" + strTTime + "', ";
                            if (string.Compare(strPTMIINDT, "20160222") >= 0)
                            {
                                SQL = SQL + ComNum.VBLF + "    PTMIHSDT = '" + VB.Left(strHSDT, 8) + "', ";
                                SQL = SQL + ComNum.VBLF + "    PTMIHSTM = '" + strHSTM + "', ";
                            }
                            SQL = SQL + ComNum.VBLF + "    PTMIDCTP = '" + strDCTP + "', ";
                            SQL = SQL + ComNum.VBLF + "    PTMIDCCD = '" + strTHCD + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND SEQNO IN (SELECT MAX(SEQNO) SEQNO FROM NUR_ER_EMIHPTMI ";
                            SQL = SQL + ComNum.VBLF + "                                        WHERE PTMIINDT = '" + strPTMIINDT + "' ";
                            SQL = SQL + ComNum.VBLF + "                                          AND PTMIINTM = '" + strPTMIINTM + "'";
                            SQL = SQL + ComNum.VBLF + "                                          AND PTMIIDNO = '" + strPano + "' ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //clsDB.setRollbackTran(pEdisAgentDb);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;


                        SQL = " UPDATE MID_SUMMARY ";
                        SQL = SQL + ComNum.VBLF + " SET GBPTMI = 'Y' ";
                        //'SQL = SQL + vbCr + " PTMIDCDT = '" + Replace(strOutDate, " - ", "") + "', "
                        //'SQL = SQL + vbCr + " PTMIDCTM = '" + Str
                        SQL = SQL + ComNum.VBLF + " WHERE OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBPTMI IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND IPKYNG = '1' "; //'응급실

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pEdisAgentDb);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            
                            ComFunc.MsgBox("에러 발생함(MID_SUMMARY)");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    } //for i
                }
                dt.Dispose();
                dt = null;


                //clsDB.setCommitTran(pEdisAgentDb);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pEdisAgentDb);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string READ_IPWONTIME(string strPtno, string strInDate)
        {
            string rtnVal = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //string strFormNo = "";
            //string strTEMP = "";

            if (string.Compare(strInDate, "20160222") < 0) return rtnVal;

            try
            {
                SQL = " SELECT extractValue(chartxml, '//dt1') indate, extractValue(chartxml, '//it4') time";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO = '2311'";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + VB.Replace(strInDate, "-", "") + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["indate"].ToString().Trim() + " " + dt.Rows[0]["time"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;


                SQL = " SELECT DECODE(FORMNO, '2285', extractValue(chartxml, '//dt1'), extractValue(chartxml, '//dt2')) indate, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it3') time";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN ('2285','2356')";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + VB.Replace(strInDate, "-", "") + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["indate"].ToString().Trim() + " " + dt.Rows[0]["time"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;


                SQL = " SELECT DECODE(FORMNO, '2295', extractValue(chartxml, '//dt3'), extractValue(chartxml, '//dt2')) indate, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it4') time";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN ('2294','2295','2305')";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + VB.Replace(strInDate, "-", "") + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["indate"].ToString().Trim() + " " + dt.Rows[0]["time"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
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

        private string READ_STARTTIME(string ArgPano, string ArgBDate, string argORDERNO, string ArgInDate, string argOutDate = "")
        {
            string rtnVal = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSDATE = "";
            string strInDate = "";
            string strSECOND = "";

            strInDate = VB.Left(ArgInDate, 8);
            strInDate = VB.Left(strInDate, 4) + "-" + VB.Mid(strInDate, 5, 2) + "-" + VB.Right(strInDate, 2);

            if (VB.IsDate(ArgBDate) == true)
            {
                ArgBDate = Convert.ToDateTime(ArgBDate).ToShortDateString();
            }

            if (Convert.ToDateTime(strInDate).AddDays(1) < Convert.ToDateTime(ArgBDate)) return rtnVal;

            try
            {
                SQL = " SELECT TO_CHAR(B.RECEIVEDATE,'YYYYMMDD HH24MI') RDATE, 'EXAM' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.EXAM_ORDER A, KOSMOS_OCS.EXAM_SPECMST B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(A.SPECNO,1, 10) = B.SPECNO";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE = B.BDATE";
                SQL = SQL + ComNum.VBLF + "    AND B.ORDERNO = " + argORDERNO;
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RDATE,'YYYYMMDD HH24MI') RDATE, 'ETC' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.ETC_JUPMST";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + argORDERNO;
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RDATE,'YYYYMMDD HH24MI') RDATE, 'ENDO' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.ENDO_JUPMST";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + argORDERNO;
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(SEEKDATE,'YYYYMMDD HH24MI') RDATE, 'XRAY' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + argORDERNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strSDATE = dt.Rows[0]["RDATE"].ToString().Trim();
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

            if (strSDATE == "") return rtnVal;

            string strFrDate = "";
            string strEndDate = "";

            strFrDate = ComFunc.FormatStrToDateEx(VB.Left(ArgInDate, 8), "D", "-") + " " + ComFunc.FormatStrToDateEx(VB.Right(ArgInDate, 4), "M", ":");
            strEndDate = ComFunc.FormatStrToDateEx(VB.Left(strSDATE, 8), "D", "-") + " " + ComFunc.FormatStrToDateEx(VB.Right(strSDATE, 4), "M", ":");

            strSECOND = ComFunc.TimeDiffMin(strFrDate, strEndDate);

            #region // 사용안함
            //try
            //{
            //    SQL = " SELECT (TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI') - TO_DATE('" + ArgInDate + "','YYYY-MM-DD HH24:MI')) * 24 * 60 SECOND";
            //    SQL = SQL + ComNum.VBLF + " FROM DUAL";

            //    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {

            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        return rtnVal;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        strSECOND = dt.Rows[0]["SECOND"].ToString().Trim();
            //    }
            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    dt.Dispose();
            //    dt = null;

            //    ComFunc.MsgBox(ex.Message);
            //}
            #endregion
            
            if (VB.Val(strSECOND) > 1440) return rtnVal;

            if (VB.Val(strSECOND) < 0) return rtnVal;

            if (argOutDate != "")
            {
                strFrDate = ComFunc.FormatStrToDateEx(VB.Left(argOutDate, 8), "D", "-") + " " + ComFunc.FormatStrToDateEx(VB.Right(argOutDate, 4), "M", ":");
                strEndDate = ComFunc.FormatStrToDateEx(VB.Left(strSDATE, 8), "D", "-") + " " + ComFunc.FormatStrToDateEx(VB.Right(strSDATE, 4), "M", ":");

                strSECOND = ComFunc.TimeDiffMin(strFrDate, strEndDate);
            }

            if (VB.Val(strSECOND) > 0) return rtnVal;

            #region // 사용안함
            //try
            //{
            //    if (argOutDate != "")
            //    {
            //        strSECOND = "";
            //        SQL = " SELECT (TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI') - TO_DATE('" + argOutDate + "','YYYY-MM-DD HH24:MI')) * 24 * 60 SECOND";
            //        SQL = SQL + ComNum.VBLF + " FROM DUAL";
            //        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            //        if (SqlErr != "")
            //        {
            //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //            return rtnVal;
            //        }
            //        if (dt.Rows.Count > 0)
            //        {
            //            strSECOND = dt.Rows[0]["SECOND"].ToString().Trim();
            //        }
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    if (VB.Val(strSECOND) > 0) return rtnVal;

            //}
            //catch (Exception ex)
            //{
            //    dt.Dispose();
            //    dt = null;

            //    ComFunc.MsgBox(ex.Message);
            //}
            #endregion

            rtnVal = strSDATE;
            return rtnVal;
        }
    }
}
