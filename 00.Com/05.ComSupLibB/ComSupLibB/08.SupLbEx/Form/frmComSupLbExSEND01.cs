using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupInfc;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcLIST01.cs
    /// Description     : Influenza 검사 (유행성검사) LIST
    /// Author          : 김홍록
    /// Create Date     : 2017-06-19
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\ExInfluenza.frm" />
    public partial class frmComSupLbExSEND01 : Form, MainFormMessage
    {

        clsInFcSQL inFcSql = new clsInFcSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsComSupLbExSendSQL clsLbExSend = new clsComSupLbExSendSQL();

        DateTime sysdate;

        Thread thread;

        clsComSupLbExSendSQL.cLbExSend cLbExSend = null; 

        string SQL = "";
        string SqlErr = "";
        int intRowAffected = 0;

        private frmComSupLbExSEND03 frmComSupLbExSEND03X = null;
        string pStrResult = ""; // 호출폼에서 처리여부를 True or False로 받을때 사용

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        #endregion

        /// <summary>생성자</summary>
        public frmComSupLbExSEND01()
        {
            InitializeComponent();

            setEvent();
        }

        public frmComSupLbExSEND01(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();

        }

        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnSearch.Click        += new EventHandler(eBtnSearch);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnView_Send.Click     += new EventHandler(eBtnClick);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.btnSave_Send.Click     += new EventHandler(eBtnSave) ;
            
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {
                for (int i = 0; i < this.ssMain.ActiveSheet.Rows.Count; i++)
                {
                    if (this.ssMain.ActiveSheet.Cells[i,(int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.CHK].Text.ToString().Equals("True") == true)
                    {
                        DATA_SEND(i);                        
                    }
                }

                setCtrlSpread();
            }
        }

        void DATA_SEND(int nRow)
        {
            
            //string strSend01 = "";
            //string strSend02 = "";
            //string strSend = "";
            ////string WinHttp As Object
            //string strreqestinstt_charger_nm = "";
            //string strinspctinstt_charger_nm = "";
            //string strpatnt_nm = "";
            //string strpatnt_sexdstn_cd = "";
            //string strpatnt_lifyea_md = "";
            //string strpatnt_regist_no = "";
            //string strkwa_ward_nm = "";
            //string strspm_ty_list = "";
            //string strinspct_mth_ty_list = "";
            //string strspm_ty_etc = "";
            //string strinspct_mth_ty_etc = "";
            //string strpthgogan_cd = "";
            //string strreqest_de = "";
            //string strdgnss_de = "";
            //string strSpecNo = "";
            //string strResult = "";
            //string strMasterCode = "";
            //string strSubCode = "";
            //string strOrderNo = "";
            //string strBDate = "";
            //string strResultDate = "";
            //string strResultSabun = "";
            //string strRETURN = "";
            string strUri = "";

            cLbExSend = new clsComSupLbExSendSQL.cLbExSend();

            cLbExSend.strCert = "cn=(재)포항성모병원,ou=건강보험,ou=MOHW RA센터,ou=등록기관,ou=licensedCA,o=KICA,c=KR";

            cLbExSend.strdgnss_de = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RDATE_YYYY].Text.Trim().Replace("-", "");   //SS1.Col = 2  : 
            cLbExSend.strreqest_de = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.JDATE].Text.Trim().Replace("-", "");        //SS1.Col = 3  : 
            cLbExSend.strpatnt_regist_no = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.PANO].Text.Trim();                          //SS1.Col = 4  : 
            cLbExSend.strpatnt_nm = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SNAME].Text.Trim();                         //SS1.Col = 5  : 

            cLbExSend.strpatnt_sexdstn_cd = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SEX].Text.Trim();                           //SS1.Col = 6  : 
            cLbExSend.strpatnt_lifyea_md = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.BIRTH].Text.Trim();                         //SS1.Col = 7  : 
            cLbExSend.strreqestinstt_charger_nm = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.DR_NM].Text.Trim();                         //SS1.Col = 9  : 
            cLbExSend.strSpecNo = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SPECNO].Text.Trim();                        //SS1.Col = 11 : 
            cLbExSend.strResult = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RESULT].Text.Trim();                        //SS1.Col = 13 : 
            cLbExSend.strinspctinstt_charger_nm = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RESULT_NM].Text.Trim();                     //SS1.Col = 17 : 
            cLbExSend.strpthgogan_cd = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.ACODE].Text.Trim();                         //SS1.Col = 20 : 

            cLbExSend.strspm_ty_list = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE].Text.Trim();                      //SS1.Col = 21 : 
            cLbExSend.strinspct_mth_ty_list = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY].Text.Trim();                //SS1.Col = 22 :              
            cLbExSend.strspm_ty_etc = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPEETC].Text.Trim();                    // SS1.Col = 23 : 


            cLbExSend.strinspct_mth_ty_etc = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAYETC].Text.Trim();                     // SS1.Col = 24 : 

            cLbExSend.strMasterCode = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.MCODE].Text.Trim();                          // SS1.Col = 25 : 
            cLbExSend.strSubCode = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SCODE].Text.Trim();                          // SS1.Col = 26 : 
            cLbExSend.strOrderNo = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.ORDERNO].Text.Trim();                        // SS1.Col = 27 : 
            cLbExSend.strBDate = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.BDATE].Text.Trim();                          // SS1.Col = 28 : 
            cLbExSend.strResultDate = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RDATE].Text.Trim();                          // SS1.Col = 29 : 
            cLbExSend.strResultSabun = this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RESULTSABUN].Text.Trim();                    // SS1.Col = 30 : 
            cLbExSend.strinspctinstt_charger_nm = "양선문";

            cLbExSend.User = clsType.User.IdNumber;
            
            #region 2018-06-11 안정수, 병원체자동신고 웹호출 방식 로직 추가       
            
            try
            {
                WebBrowser wb = new WebBrowser();

                //strUri = "https://is.cdc.go.kr/tids/anids/pthgogan/reportAutoForm.vp?";
                strUri = "https://is.kdca.go.kr/tids/anids/pthgogan/reportAutoForm.vp?";

                string strPostData = string.Format("dplct_at={0}&rspns_mssage_ty={1}&ogcr={2}&reqestinstt_charger_nm={3}&inspctinstt_charger_nm={4}&patnt_nm={5}&patnt_sexdstn_cd={6}&patnt_lifyea_md={7}&patnt_regist_no={8}&spm_ty_list={9}&spm_ty_etc={10}&inspct_mth_ty_list={11}&inspct_mth_ty_etc={12}&pthgogan_cd={13}&reqest_de={14}&dgnss_de={15}&hsptl_swbser={16}&hsptl_swknd={17}",
                                                "1",
                                                "0",
                                                cLbExSend.strCert,
                                                cLbExSend.strreqestinstt_charger_nm,
                                                cLbExSend.strinspctinstt_charger_nm,
                                                cLbExSend.strpatnt_nm,
                                                cLbExSend.strpatnt_sexdstn_cd,
                                                cLbExSend.strpatnt_lifyea_md,
                                                cLbExSend.strpatnt_regist_no,
                                                cLbExSend.strspm_ty_list, //
                                                cLbExSend.strspm_ty_etc,
                                                cLbExSend.strinspct_mth_ty_list,  //
                                                cLbExSend.strinspct_mth_ty_etc,
                                                cLbExSend.strpthgogan_cd,
                                                cLbExSend.strreqest_de,
                                                cLbExSend.strdgnss_de,
                                                "자체개발",
                                                "1"
                                                );
                byte[] postData = UTF8Encoding.UTF8.GetBytes(strPostData);

                frmComSupLbExSEND03X = new frmComSupLbExSEND03(strUri, postData);
                frmComSupLbExSEND03X.rSendText += new frmComSupLbExSEND03.SendText(GetText);
                frmComSupLbExSEND03X.rEventExit += new frmComSupLbExSEND03.EventExit(frmComSupLbExSEND03X_rEventExit);
                frmComSupLbExSEND03X.ShowDialog();

                if(pStrResult == "True")
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    //전송에 성공했을 시, EXAM_AUTOSEND_LOG에 INSERT
                    SqlErr = clsLbExSend.ins_EXAM_AUTOSEND_LOG(clsDB.DbCon, cLbExSend, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                        return;
                    }

                    if (SqlErr == "")
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }

                else
                {
                    //
                    READ_ERROR(pStrResult, cLbExSend);
                }
                postData = null;
                pStrResult = ""; 
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }

            #endregion 웹 호출 방식

            #region 메세지 전송 방식

            ////연계서버주소
            ////strUri = "https://is.cdc.go.kr/tids/anids/pthgogan/reportAutoForm.vp?";
            //strUri = "https://152.99.73.139:8443/indigo/InfctnRgstr";

            ////전송항목설정
            //StringBuilder dataParams = new StringBuilder();
            //dataParams.Append("dplct_at = 0");
            //dataParams.Append("&rspns_mssage_ty = 0");
            //dataParams.Append("&ogcr = " + strCert);
            //dataParams.Append("&reqestinstt_charger_nm = " + strreqestinstt_charger_nm);
            //dataParams.Append("&inspctinstt_charger_nm = " + strinspctinstt_charger_nm);
            //dataParams.Append("&patnt_nm = " + strpatnt_nm);

            //dataParams.Append("&patnt_sexdstn_cd = " + strpatnt_sexdstn_cd);
            //dataParams.Append("&patnt_lifyea_md = " + strpatnt_lifyea_md);
            //dataParams.Append("&patnt_regist_no = " + strpatnt_regist_no);
            //dataParams.Append("&spm_ty_list = " + strspm_ty_list);
            //dataParams.Append("&spm_ty_etc = " + strspm_ty_etc);

            //dataParams.Append("&inspct_mth_ty_list = " + strinspct_mth_ty_list);
            //dataParams.Append("&inspct_mth_ty_etc = " + strinspct_mth_ty_etc);
            //dataParams.Append("&pthgogan_cd = " + strpthgogan_cd);
            //dataParams.Append("&reqest_de = " + strreqest_de);
            //dataParams.Append("&dgnss_de = " + strdgnss_de);
            //dataParams.Append("&hsptl_swbser = 자체개발");
            //dataParams.Append("&hsptl_swknd = 1");

            //// 요청 String -> 요청 Byte 변환
            //byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(dataParams.ToString());

            //// HttpWebRequest 객체 생성, 설정
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://152.99.73.139:8443/indigo/PthgnRgstr?ogcr=cn%3D%28%EC%9E%AC%29%ED%8F%AC%ED%95%AD%EC%84%B1%EB%AA%A8%EB%B3%91%EC%9B%90%2Cou%3D%EA%B1%B4%EA%B0%95%EB%B3%B4%ED%97%98%2Cou%3DMOHW+RA%EC%84%BC%ED%84%B0%2Cou%3D%EB%93%B1%EB%A1%9D%EA%B8%B0%EA%B4%80%2Cou%3DlicensedCA%2Co%3DKICA%2Cc%3DKR&dplct_at=0&rspns_mssage_ty=0&icd_cd=A0001&pthgogan_cd=A000100&reqestinstt_charger_nm=%EA%B9%80%EC%9A%A9%EA%B5%AD&patnt_nm=%ED%95%9C%EC%98%88%EC%A3%BC&patnt_sexdstn_cd=2&patnt_lifyea_md=20151102&patnt_regist_no=10631264&kwa_ward_nm=&spm_ty_list=03&spm_ty_etc=&inspct_mth_ty_list=02&inspct_mth_ty_etc=&icdgrp_cd=01&reqest_de=20180610&dgnss_de=20180612&rm_info=&inspctinstt_charger_nm=%EC%96%91%EC%84%A0%EB%AC%B8");
            //request.Method = "POST";    // 기본값 "GET"
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = byteDataParams.Length;

            //// 요청 Byte -> 요청 Stream 변환
            //Stream stDataParams = request.GetRequestStream();
            //stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
            //stDataParams.Close();

            //// 요청, 응답 받기
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //// 응답 Stream 읽기
            //Stream stReadData = response.GetResponseStream();
            //StreamReader srReadData = new StreamReader(stReadData, Encoding.Default);

            //// 응답 Stream -> 응답 String 변환            
            //strResult = srReadData.ReadToEnd();

            //string Test = "";
            // 전송 결과값 처리
            //strRETURN = StrConv(WinHttp.responseBody, vbUnicode)

            //If Mid(strRETURN, InStr(strRETURN, "<code_dt>") +9, 4) = "2001" Then
            //   SQL = " INSERT INTO KOSMOS_OCS.EXAM_AUTOSEND_LOG("
            //    SQL = SQL & vbCr & " JDATE,RDATE,Pano,sName,Sex,"
            //    SQL = SQL & vbCr & " BIRTH,DRNAME,SpecNo,MASTERCODE,SUBCODE,"
            //    SQL = SQL & vbCr & " Result,RESULTDATE,RESULTSABUN,ACODE,EXAMTYPE,"
            //    SQL = SQL & vbCr & " EXAMWAY,EXAMTYPEETC,EXAMWAYETC,ORDERNO,"
            //    SQL = SQL & vbCr & " BDATE,SENDDATE,SENDSABUN) VALUES ("
            //    SQL = SQL & vbCr & " TO_DATE('" & strdgnss_de & "','YYYY-MM-DD'), TO_DATE('" & strreqest_de & "','YYYY-MM-DD'), '" & strpatnt_regist_no & "','" & strpatnt_nm & "','" & strpatnt_sexdstn_cd & "', "
            //    SQL = SQL & vbCr & " '" & strpatnt_lifyea_md & "','" & strreqestinstt_charger_nm & "','" & strSpecNo & "','" & strMasterCode & "','" & strSubCode & "', "
            //    SQL = SQL & vbCr & " '" & strResult & "',TO_DATE('" & strResultDate & "','YYYY-MM-DD HH24:MI'), " & strResultSabun & ",'" & strpthgogan_cd & "','" & strspm_ty_list & "',"
            //    SQL = SQL & vbCr & " '" & strinspct_mth_ty_list & "','" & strspm_ty_etc & "','" & strinspct_mth_ty_etc & "', " & strOrderNo & ", "
            //    SQL = SQL & vbCr & " TO_DATE('" & strBDate & "','YYYY-MM-DD'), SYSDATE, " & GnJobSabun & ") "
            //    result = AdoExecute(SQL)
            //    If result <> 0 Then DATA_SEND = "Log Error"
            //End If

            //Call READ_ERROR(strRETURN, strpatnt_regist_no, strpatnt_nm, strreqest_de, strdgnss_de, strSpecNo)

            #endregion
        }

        void READ_ERROR(string arg, clsComSupLbExSendSQL.cLbExSend argCls)
        {            
            string strMsg = "";

            switch (arg)
            {                
                case "3001":    strMsg = "인증정보" + "\r\n" + "사용자(기관)  인증정보는 필수 입력사항입니다.";
                    break;
              
                case "3003":    strMsg = "담당의사명" + "\r\n" + "담당의사명은 필수 입력사항입니다.";
                    break;

                case "3004":    strMsg = "담당의사 면허번호" + "\r\n" + "담당의사 면허번호는 필수 입력사항입니다.";
                    break;

                case "3005":    strMsg = "환자성명" + "\r\n" + "환자성명은  필수 입력사항입니다.";
                    break;

                case "3006":    strMsg = "환자성별" + "\r\n" + "환자성별은  필수 입력사항입니다.";
                    break;

                case "3007":    strMsg = "환자생년월일" + "\r\n" + "환자생년월인(YYYYMMDD)은  필수 입력사항입니다.";
                    break;

                case "3008":    strMsg = "발병일자" + "\r\n" + "발병일자(YYYYMMDD)는  필수 입력사항입니다.";
                    break;
                
                case "3010":    strMsg = "검사법" + "\r\n" + "감염병의 검사법은 필수 입력사항입니다.";
                    break;

                case "3011":    strMsg = "검체코드" + "\r\n" + "감염병의 검체코드는 필수 입력사항입니다.";
                    break;
             
                case "3013":    strMsg = "검체채취일자" + "\r\n" + "감염병의 검체채취일자는 필수 입력사항입니다.";
                    break;

                case "3014":    strMsg = "감염병_코드" + "\r\n" + "감염병코드는 필수 입력사항입니다.";
                    break;

                case "3015":    strMsg = "검사기관코드" + "\r\n" + "검사기관코드는 필수 입력사항입니다.";
                    break;

                case "4001":    strMsg = "검체코드 중복입력 여부" + "\r\n" + "감염병의 검사법과 검체명을 중복하여 등록할 수 없습니다.";
                    break;

                case "4002":    strMsg = "담당의사명" + "\r\n" + "담당의사명과 면허번호가 일치하지 않습니다. 질병보건통합관리시스템을 통해 확인해 주세요.";
                    break;

                case "4003":    strMsg = "면허번호 숫자체크" + "\r\n" + "담당의사 면허번호는 숫자로 입력하세요.";
                    break;

                case "4005":    //strMsg = "검체의뢰정보 일치 여부" + "\r\n" + "검사기관에 대한 감염병코드, 검사법, 검체코드가 유효하지 않습니다.";                    
                                strMsg = "검체코드 중복입력 여부" + "\r\n" + "중복 전송된 병원체 검사결과 신고입니다.";
                    break;

                case "4007":    strMsg = "환자성명" + "\r\n" + "환자성명은  최대 30자리로 입력하세요.";
                    break;

                case "4008":    strMsg = "환자성별" + "\r\n" + "환자성별코드는 1 또는 2 여야합니다.";
                    break;

                case "4009":    strMsg = "환자생년월일" + "\r\n" + "환자생년월일은 8자리(YYYYMMDD)로 입력하세요.";
                    break;

                case "4010":    strMsg = "환자생년월일" + "\r\n" + "환자생년월일은 숫자로 입력하세요.";
                    break;

                case "4011":    strMsg = "발병일자" + "\r\n" + "발병일자는 8자리(YYYYMMDD)로 입력하세요";
                    break;

                case "4012":    strMsg = "발병일자" + "\r\n" + "발병일자 숫자로 입력하세요.";
                    break;
                
                case "4014":    strMsg = "검사법" + "\r\n" + "감염병의 검사법 값이 유효하지 않습니다. 배포가이드 문서를 참조하세요.";
                    break;

                case "4015":    strMsg = "검체코드" + "\r\n" + "감염병의 검체코드 값이 유효하지 않습니다. 배포가이드 문서를 참조하세요.";
                    break;
                
                case "4017":    strMsg = "채취일자" + "\r\n" + "감염병의 채취일자는 8자리(YYYYMMDD) 입니다.";
                    break;

                case "4020":    strMsg = "감염병_코드" + "\r\n" + "감염병코드 값이 유효하지 않습니다. 배포가이드 문서를 참조하세요.";
                    break;

                case "4021":    strMsg = "발병일자(발병일자<=채취일자)" + "\r\n" + "감염병의 발병일은 채취일과 같거나 빨라야합니다.";
                    break;

                case "4022":    strMsg = "예방접종여부" + "\r\n" + "예방접종여부는 1,2중 하나여야 합니다.";
                    break;

                case "4023":    strMsg = "생년월일(생년월일<=발병일)" + "\r\n" + "생년월일은 발병일보다 같거나 빨라야합니다.";
                    break;

                case "4024":    strMsg = "검사기관코드" + "\r\n" + "검사기관 코드가 유효하지 않습니다. 배포가이드 문서를 참조하세요.";
                    break;

                case "4025":    strMsg = "감별진단여부" + "\r\n" + "감염병의 감별진단여부는 Y,N 중 하나여야합니다";
                    break;

                case "4026":    strMsg = "검체운송기관" + "\r\n" + "검체운송기관이 유효하지 않습니다. 배포가이드 문서를 참조하세요.";
                    break;

                case "5001":    strMsg = "인증정보" + "\r\n" + "사용자(기관)  인증정보가 웹신고시스템에 등록된 인증정보와 같지 않습니다.";
                    break;

                default:        strMsg = "전송 에러 코드 : " + arg + "\r\n" + "전산정보팀으로 연락 바랍니다.";
                    break;
            }

            if(strMsg != "")
            {
                ComFunc.MsgBox(strMsg + "\r\n" + "▶ 등록번호 : " + argCls.strpatnt_regist_no + "(" + argCls.strpatnt_nm + ")" + "\r\n"
                                               + "▶ 검사일/결과일 : " + argCls.strreqest_de + "/" + argCls.strdgnss_de + "\r\n"
                                               + "▶ 검체번호 : " + argCls.strSpecNo, "전송에러");
            }
        }


        void frmComSupLbExSEND03X_rEventExit()
        {
            frmComSupLbExSEND03X.Dispose();
            frmComSupLbExSEND03X = null;
        }

        void GetText(string argResult)
        {
            pStrResult = argResult;
        }

        void eViewSendData()
        {
            try
            {
                WebBrowser wb = new WebBrowser();
                //string strUri = "https://is.cdc.go.kr/tids/anids/pthgogan/pthgoganList.vp?";
                string strUri = "https://is.kdca.go.kr/tids/anids/pthgogan/pthgoganList.vp?";
                string strCert = "cn=(재)포항성모병원,ou=건강보험,ou=MOHW RA센터,ou=등록기관,ou=licensedCA,o=KICA,c=KR";

                string strPostData = string.Format("ogcr={0}",
                                                strCert
                                                );
                byte[] postData = UTF8Encoding.UTF8.GetBytes(strPostData);                

                wb.Navigate(strUri, "감염발생신고조회", postData, "Content-Type: application/x-www-form-urlencoded");                
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }       

        void setCtrl()
        {
            setCtrlDate();
            setCtrlSpread();

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnPrintClick(object sender, EventArgs e)
        {
        }

        void eRdoClick(object sender, EventArgs e)
        {
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView_Send)
            {
                eViewSendData();
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE_NM || e.Column == (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY_NM)
            {
                string strEXAMTYPE = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE].Text.Trim();
                string strEXAMTYPEETC = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPEETC].Text.Trim();

                string strEXAMWAY = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY].Text.Trim();
                string strEXAMWAYETC = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAYETC].Text.Trim();

                frmComSupLbExSEND02 f = new frmComSupLbExSEND02(strEXAMTYPE, strEXAMWAY, strEXAMTYPEETC, strEXAMWAYETC);
                f.ePsmhReturnValue += new frmComSupLbExSEND02.PSMH_RETURN_VALUE(ePSMH_VALUE);
                f.ShowDialog();
            }

        }

        void ePSMH_VALUE(string strEXAMTYPE, string strEXAMTYPE_NM, string strEXAMWAY, string strEXAMWAY_NM, string strEXAMTYPEETC, string strEXAMWAYETC)
        {

            int nRow = this.ssMain.ActiveSheet.ActiveRow.Index;

            if (nRow > -1)
            {
                if (string.IsNullOrEmpty(strEXAMTYPE) == false)
                {
                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE].Text = strEXAMTYPE.Trim();
                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE_NM].Text = strEXAMTYPE_NM.Trim();
                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPEETC].Text = strEXAMTYPEETC.Trim();

                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY].Text = strEXAMWAY.Trim();
                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY_NM].Text = strEXAMWAY_NM.Trim();

                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAYETC].Text = strEXAMWAYETC.Trim();

                    this.ssMain.ActiveSheet.Cells[nRow, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.CHK].Text = "True";
                }
            }


        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate;
            this.dtpTDate.Value = sysdate;
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
           
            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");


            if (method.getDate_Gap(Convert.ToDateTime(strFDate), Convert.ToDateTime(strTDate)) > 60)
            {
                ComFunc.MsgBox("일자는 3일을 넘을 수 없습니다.");
                this.dtpFDate.Focus();
                return;
            }

            string strOptA = "";

            if (rdo_OptA1.Checked == true)
            {
                strOptA = "A1";
            }
            else if (rdo_OptA2.Checked == true)
            {
                strOptA = "A2";
            }
            else if (rdo_OptA3.Checked == true)
            {
                strOptA = "A3";
            }


            thread = new Thread(() => threadSetCtrlSpread(this.rdo_Opt1.Checked, strOptA, strFDate, strTDate));
            thread.Start();
          
        }

        void threadSetCtrlSpread( bool isOpt1, string strOptA, string strFDate, string strTDate)
        {
            DataSet ds = null;

            string strPart = "";

            if(chkPart.Checked == true)
            {
                strPart = "Y";
            }
            else
            {
                strPart = "";
            }

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = inFcSql.sel_EXAM_AUTOSEND_LOG_SEND(clsDB.DbCon, isOpt1, strOptA, strFDate, strTDate, strPart);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, inFcSql.sEXAM_AUTOSEND_LOG_SEND, inFcSql.nEXAM_AUTOSEND_LOG_SEND });
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void delegateSetCtrlCircular(bool b);
        void setCtrlCircular(bool b)
        {
            if (b == true)
            {
                this.ssMain.Enabled = true;
            }
            else
            {
                this.ssMain.Enabled = true;
            }

            this.circProgress.Visible = b;
            this.circProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size);
        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {

            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 40;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            spd.ActiveSheet.ColumnCount = 0;

            spd.ActiveSheet.ColumnCount = colName.Length;
            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            spd.DataSource = ds;

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size, 10);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.CHK, clsSpread.enmSpdType.CheckBox);

            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SEX           , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.BIRTH         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.CHG           , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.DR_NM         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.IPDOPD        , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.ROWID_R       , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.ACODE         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPE      , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAY       , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMTYPEETC   , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAMWAYETC    , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.MCODE         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SCODE         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.ORDERNO       , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.BDATE         , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RDATE         , clsSpread.enmSpdType.Hide);  
            spread.setColStyle(spd, -1, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RESULTSABUN   , clsSpread.enmSpdType.Hide);


            //// 4. 정렬
            //spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            //// 5. sort, filter
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.PANO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SNAME, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.SPECNO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.EXAM_NM, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmEXAM_AUTOSEND_LOG_SEND.RESULT, AutoFilterMode.EnhancedContextMenu, true);
            //spread.setSpdSort(spd, (int)clsInFcSQL.enmSel_EXAM_RESULTC_infec.PANO, true);
        }      
    }
}
