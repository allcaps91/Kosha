using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcNhic_New : Form, MainFormMessage
    {
        string mPara1 = "";

        string FstrROWID = "";
        string FstrRequest1 = "";
        string FstrRequest2 = "";
        string FstrRequest3 = "";

        clsSpread cSpd = null;

        WorkNhicService workNhicService = null;
        HicResultService hicResultService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuService hicJepsuService = null; 

        public frmHcNhic_New()
        {
            InitializeComponent();
            setEvent();
        }


        #region //MainFormMessage

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
        #endregion //MainFormMessage

        void setEvent()
        {
            cSpd = new clsSpread();

            workNhicService = new WorkNhicService();
            hicResultService = new HicResultService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuService = new HicJepsuService();

            this.Load += new System.EventHandler(eFormLoad);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnStart.Click += new EventHandler(eBtnClick);
            this.btnStop.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick1);
            this.timer2.Tick += new EventHandler(eTimerTick2);

            this.rdoJob1.CheckedChanged += new EventHandler(eRdoChanged);
            this.rdoJob2.CheckedChanged += new EventHandler(eRdoChanged);

            int nYear = VB.Left(DateTime.Now.ToShortDateString(), 4).To<int>();

            cboYear.Items.Clear();

            for (int i = 0; i < 2; i++)
            {
                cboYear.Items.Add(nYear);
                nYear = nYear - 1;
            }
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
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

        public void SetControl()
        {
        }

        public frmHcNhic_New(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetControl();
            setEvent();
        }

        public frmHcNhic_New(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetControl();
            setEvent();
        }


        void eFormLoad(object sender, EventArgs e)
        {
            cboYear.SelectedIndex = 0;
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            cSpd.Spread_Clear_Simple(SS1);
            cSpd.Spread_Clear_Simple(SSList);
            SSList.ActiveSheet.RowCount = 1;
            SSList.ActiveSheet.Rows[0].Height = 25;
        }

        private void Read_Work()
        {
            FstrROWID = "";
            FstrRequest1 = "";
            FstrRequest2 = "";
            FstrRequest3 = "";


            //구분 =H 
            WORK_NHIC item = workNhicService.GetOneItemByNewData("H");

            //일반-인적정보
            if (!item.IsNullOrEmpty())
            {
                //이름, 주민번호
                SSList.ActiveSheet.Cells[0, 0].Text = item.SNAME;
                SSList.ActiveSheet.Cells[0, 1].Text = clsAES.DeAES(item.JUMIN2);

                FstrROWID = item.RID;
                lblMsg.Text = "자격조회(인적정보) 진행중... ";
                #region 메세지 전송 방식 검진정보

                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "1");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest1 = response.Content;

                #endregion 메세지 전송 방식
            }
            
            //일반-검진정보
            if (!item.IsNullOrEmpty())
            {
                lblMsg.Text = "자격조회(검진정보) 진행중... ";
                #region 메세지 전송 방식 검진정보

                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "2");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest2 = response.Content;

                #endregion 메세지 전송 방식
            }
            
            //일반-수검정보
            if (!item.IsNullOrEmpty())
            {
                lblMsg.Text = "자격조회(수진정보) 진행중... ";
                #region 메세지 전송 방식 검진정보
                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "3");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest3 = response.Content;

                #endregion 메세지 전송 방식
            }
        }

        private void Read1()
        {
            FstrROWID = "";
            FstrRequest1 = "";

            WORK_NHIC item = workNhicService.GetOneItemByNewData("H");

            if (!item.IsNullOrEmpty())
            {
                FstrROWID = item.RID;
                #region 메세지 전송 방식 검진정보

                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "1");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)


                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest1 = response.Content;

                #endregion 메세지 전송 방식
            }
        }

        private void Read2()
        {

            FstrRequest2 = "";
            WORK_NHIC item = workNhicService.GetOneItemByNewData("H");

            if (!item.IsNullOrEmpty())
            {
                #region 메세지 전송 방식 검진정보

                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded"); 
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "2");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)


                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest2 = response.Content;

                #endregion 메세지 전송 방식
            }
        }

        private void Read3()
        {

            FstrRequest3 = "";
            WORK_NHIC item = workNhicService.GetOneItemByNewData("H");

            if (!item.IsNullOrEmpty())
            {
                #region 메세지 전송 방식 검진정보

                var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/provideHccInfo.do");
                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=jILE7HgtWXjM2eklzcn3C4owITvVUbtQo-SWBg4972EEHOWnZ5bL!-2128738377!2125512421");

                request.AddParameter("SVC_TKN_KEY", "a03e81e7-138f-43d5-b476-acfaea1d8a8f");    //검진대상자 API 신청시 발행되는 인증키
                request.AddParameter("HCC_NO", "37100068");                                     //검진기관기호
                request.AddParameter("RRNO", clsAES.DeAES(item.JUMIN2));                                  //수검자 주민등록번호
                request.AddParameter("BZ_YYYY", item.YEAR);                                     //검진년도
                request.AddParameter("OBJT_FNM", item.SNAME);                                   //수검자 성명
                request.AddParameter("SVC_TYPE", "1");                                          //요청구분 (1:일반, 2:영유아, 3:학교밖청소년)
                request.AddParameter("REQUEST_GB", "3");                                        //서비스정보 (1:인적정보, 2:검진정보, 3:수검정보)


                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                FstrRequest3 = response.Content;

                #endregion 메세지 전송 방식
            }
        }

        //자격조회
        private void WORK_NHIC_API()
        {

            #region Define Variable
            string strPANO = string.Empty; string strJumin = string.Empty; string strSName = string.Empty; string strROWID = string.Empty; string strRel = string.Empty;
            string strBI = string.Empty; string strkiho = string.Empty; string strGkiho = string.Empty; string strPName = string.Empty; string strBDate = string.Empty;
            string strJisa = string.Empty; string strLiver2 = string.Empty;
            string strYear = string.Empty; string strTrans = string.Empty; string strFirst = string.Empty; string strSecond = string.Empty; string strEKG = string.Empty;
            string strLiver = string.Empty; string strLiverC = string.Empty;
            string strDental = string.Empty; string str1차Add = string.Empty; string str2차Add = string.Empty;
            string strCancer11 = string.Empty; string strCancer12 = string.Empty;
            string strCancer21 = string.Empty; string strCancer22 = string.Empty;
            string strCancer31 = string.Empty; string strCancer32 = string.Empty;
            string strCancer41 = string.Empty; string strCancer42 = string.Empty;
            string strCancer51 = string.Empty; string strCancer52 = string.Empty;
            string strCancer53 = string.Empty;
            string strCancer61 = string.Empty; string strCancer62 = string.Empty;
            string strCancer71 = string.Empty; string strCancer72 = string.Empty;

            //1차검진 일자,출장,기관
            string strChk01_1 = string.Empty;
            string strChk01_2 = string.Empty;
            string strChk01_3 = string.Empty;
            //2차검진
            string strChk02_1 = string.Empty;
            string strChk02_2 = string.Empty;
            string strChk02_3 = string.Empty;
            //구강
            string strChk03_1 = string.Empty;
            string strChk03_2 = string.Empty;
            string strChk03_3 = string.Empty;

            //위암(조영)
            string strChk04_1 = string.Empty;
            string strChk04_2 = string.Empty;
            string strChk04_3 = string.Empty;
            //위암(내시경)
            string strChk15_1 = string.Empty;
            string strChk15_2 = string.Empty;
            string strChk15_3 = string.Empty;

            //대장암(잠혈)
            string strChk05_1 = string.Empty;
            string strChk05_2 = string.Empty;
            string strChk05_3 = string.Empty;
            //대장암(조영)
            string strChk16_1 = string.Empty;
            string strChk16_2 = string.Empty;
            string strChk16_3 = string.Empty;
            //대장(내시경)
            string strChk17_1 = string.Empty;
            string strChk17_2 = string.Empty;
            string strChk17_3 = string.Empty;

            //유방암
            string strChk06_1 = string.Empty;
            string strChk06_2 = string.Empty;
            string strChk06_3 = string.Empty;
            //자궁경부
            string strChk07_1 = string.Empty;
            string strChk07_2 = string.Empty;
            string strChk07_3 = string.Empty;
            //간암(상반기)
            string strChk08_1 = string.Empty;
            string strChk08_2 = string.Empty;
            string strChk08_3 = string.Empty;
            //간암(하반기)
            string strChk09_1 = string.Empty;
            string strChk09_2 = string.Empty;
            string strChk09_3 = string.Empty;
            //폐암
            string strChk10_1 = string.Empty;
            string strChk10_2 = string.Empty;
            string strChk10_3 = string.Empty;

            string strOK = string.Empty;
            string strGubun = string.Empty;
            string strLifeGubun = string.Empty;

            string strExamA = string.Empty; //이상지질혈증
            string strExamD = string.Empty; //골밀도
            string strExamE = string.Empty; //인지기능장애
            string strExamF = string.Empty; //정신건강검사
            string strExamG = string.Empty; //생활습관평가
            string strExamH = string.Empty; //노인신체기능
            string strExamI = string.Empty; //치면세균막
            string strBIGO = string.Empty; //연장대상구분
            
            int nKIND = 0;
            int nKIND1 = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            #endregion

            //
            if (!FstrRequest1.IsNullOrEmpty())
            {
                strOK = "OK";

                //이름
                strSName = VB.Pstr(FstrRequest1, "OBJT_FNM", 2);
                strSName = VB.Pstr(strSName, ":", 2);
                strSName = VB.Pstr(strSName, "\"", 2);

                //사업년도            
                strYear = VB.Pstr(FstrRequest1, "BZ_YYYY", 2);
                strYear = VB.Pstr(strYear, ":", 2);
                strYear = VB.Pstr(strYear, "\"", 2);

                //증번호
                //strGkiho = VB.Pstr(FstrRequest1, "<th scope=row class=first>증번호</th>", 2);
                strGkiho = VB.Pstr(FstrRequest1, "HIC_NO", 2);
                strGkiho = VB.Pstr(strGkiho, ":", 2);
                strGkiho = VB.Pstr(strGkiho, "\"", 2);

                //소속지사
                strJisa = VB.Pstr(FstrRequest1, "BRCH_NM", 2);
                strJisa = VB.Pstr(strJisa, ":", 2);
                strJisa = VB.Pstr(strJisa, "\"", 2);

                //지역구분(지역, 직장, 의료급여, 공교, 학교밖청소년)
                strRel = VB.Pstr(FstrRequest1, "HC_CLCD_NM", 2);
                strRel = VB.Pstr(strRel, ":", 2);
                strRel = VB.Pstr(strRel, "\"", 2);

                //사업장관리번호
                //strkiho = VB.Pstr(FstrRequest1, "<th scope=row class=first>사업장번호</th>", 2);
                //strkiho = VB.Pstr(strkiho, "<td class=left>", 2);
                //strkiho = VB.Pstr(strkiho, "</td>", 1);

                //국가암 통보처
                strCancer53 = VB.Pstr(FstrRequest1, "HSYM", 3);
                strCancer53 = VB.Pstr(strCancer53, ":", 2);
                strCancer53 = VB.Pstr(strCancer53, "\"", 2);
                if (strCancer53 == "MESSAGE") { strCancer53 = ""; }

                //연장구분
                strBIGO = VB.Pstr(FstrRequest1, "MESSAGE", 2);
                strBIGO = VB.Pstr(strBIGO, ":", 2);
                strBIGO = VB.Pstr(strBIGO, "\"", 2);
                //strBIGO = VB.Pstr(strBIGO, "")
            }

            if (!FstrRequest2.IsNullOrEmpty())
            {

                //1차진단
                if (VB.InStr(FstrRequest2, "A0") > 0)
                {
                    strFirst = "본인부담없음";
                }
                else
                {
                    strFirst = "비대상";
                }

                if (strRel == "의료급여" && strFirst == "비대상")
                {
                    if (VB.InStr(FstrRequest2, "A5") > 0)
                    {
                        strFirst = "본인부담없음";
                    }
                    else
                    {
                        strFirst = "비대상";
                    }
                }

                //구강검진

                if (VB.InStr(FstrRequest2, "C1") > 0)
                {
                    strDental = "본인부담없음";
                }
                else
                {
                    strDental = "비대상";
                }

                //연령별 세부검사항목-----------------------------------start---------------

                if (!VB.Pstr(FstrRequest2, "연령별세부검사항목", 2).IsNullOrEmpty())
                {
                    //이상지질혈증
                    strExamA = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamA = VB.Pstr(strExamA, "A101", 2);
                    strExamA = VB.Pstr(strExamA, ":", 2);
                    strExamA = VB.Pstr(strExamA, "\"", 2);
                    if (strExamA == "0")
                    {
                        strExamA = "비대상";
                    }
                    else if (strExamA == "1")
                    {
                        strExamA = "대상";
                    }

                    //B형간염
                    strLiver = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strLiver = VB.Pstr(strLiver, "A102", 2);
                    strLiver = VB.Pstr(strLiver, ":", 2);
                    strLiver = VB.Pstr(strLiver, "\"", 2);
                    if (strLiver == "0")
                    {
                        strLiver = "비대상";
                    }
                    else if (strLiver == "1")
                    {
                        strLiver = "대상";
                    }

                    //골밀도검사
                    strExamD = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamD = VB.Pstr(strExamD, "A103", 2);
                    strExamD = VB.Pstr(strExamD, ":", 2);
                    strExamD = VB.Pstr(strExamD, "\"", 2);
                    if (strExamD == "0")
                    {
                        strExamD = "비대상";
                    }
                    else if (strExamD == "1")
                    {
                        strExamD = "대상";
                    }

                    //인지기능장애
                    strExamE = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamE = VB.Pstr(strExamE, "A104", 2);
                    strExamE = VB.Pstr(strExamE, ":", 2);
                    strExamE = VB.Pstr(strExamE, "\"", 2);
                    if (strExamE == "0")
                    {
                        strExamE = "비대상";
                    }
                    else if (strExamE == "1")
                    {
                        strExamE = "대상";
                    }


                    //정신건강검사
                    strExamF = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamF = VB.Pstr(strExamF, "A105", 2);
                    strExamF = VB.Pstr(strExamF, ":", 2);
                    strExamF = VB.Pstr(strExamF, "\"", 2);
                    if (strExamF == "0")
                    {
                        strExamF = "비대상";
                    }
                    else if (strExamF == "1")
                    {
                        strExamF = "대상";
                    }

                    //생활습관평가
                    strExamG = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamG = VB.Pstr(strExamG, "A106", 2);
                    strExamG = VB.Pstr(strExamG, ":", 2);
                    strExamG = VB.Pstr(strExamG, "\"", 2);
                    if (strExamG == "0")
                    {
                        strExamG = "비대상";
                    }
                    else if (strExamG == "1")
                    {
                        strExamG = "대상";
                    }

                    //노인신체기능
                    strExamH = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamH = VB.Pstr(strExamH, "A107", 2);
                    strExamH = VB.Pstr(strExamH, ":", 2);
                    strExamH = VB.Pstr(strExamH, "\"", 2);
                    if (strExamH == "0")
                    {
                        strExamH = "비대상";
                    }
                    else if (strExamH == "1")
                    {
                        strExamH = "대상";
                    }


                    //치면세균막
                    strExamI = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strExamI = VB.Pstr(strExamI, "A108", 2);
                    strExamI = VB.Pstr(strExamI, ":", 2);
                    strExamI = VB.Pstr(strExamI, "\"", 2);
                    if (strExamI == "0")
                    {
                        strExamI = "비대상";
                    }
                    else if (strExamI == "1")
                    {
                        strExamI = "대상";
                    }

                    //C형간염
                    strLiverC = VB.Pstr(FstrRequest2, "연령별세부검사항목", 2);
                    strLiverC = VB.Pstr(strLiverC, "A109", 2);
                    strLiverC = VB.Pstr(strLiverC, ":", 2);
                    strLiverC = VB.Pstr(strLiverC, "\"", 2);
                    if (strLiverC == "0")
                    {
                        strLiverC = "비대상";
                    }
                    else if (strLiverC == "1")
                    {
                        strLiverC = "대상";
                    }
                }
                else
                {
                    strExamA ="비대상";
                    strExamD ="비대상";
                    strExamE ="비대상";
                    strExamF ="비대상";
                    strExamG ="비대상";
                    strExamH ="비대상";
                    strExamI ="비대상";
                    strLiver ="비대상";
                    strLiverC = "비대상";
                }
                // 연령별 세부검사 항목 -------------------------End

                // 암검진 항목 -------------------------Start

                //위암 부담율, 치료비지원
                if (VB.InStr(FstrRequest2, "D1") > 0)
                {
                    strCancer11 = VB.Pstr(FstrRequest2, "위암", 2);
                    strCancer11 = VB.Pstr(strCancer11, "SBA_RAT_CD", 2);
                    strCancer11 = VB.Pstr(strCancer11, ":", 2);
                    strCancer11 = VB.Pstr(strCancer11, "\"", 2);

                    if (strCancer11 == "00")
                    {
                        strCancer11 = "본인부담없음";
                        strCancer12 = "본인부담없음";
                    }
                    else
                    {
                        strCancer11 = "10%본인부담";
                        strCancer12 = "10%본인부담";
                    }
                    
                }
                else
                {
                    strCancer11 = "대상아님";
                    strCancer12 = "대상아님";
                }


                //대장암 부담율, 치료비지원
                if (VB.InStr(FstrRequest2, "D2") > 0)
                {
                    strCancer31 = VB.Pstr(FstrRequest2, "대장암", 2);
                    strCancer31 = VB.Pstr(strCancer31, "SBA_RAT_CD", 2);
                    strCancer31 = VB.Pstr(strCancer31, ":", 2);
                    strCancer31 = VB.Pstr(strCancer31, "\"", 2);

                    if (strCancer31 == "00")
                    {
                        strCancer31 = "본인부담없음";
                        strCancer32 = "본인부담없음";
                    }
                    else
                    {
                        strCancer31 = "10%본인부담";
                        strCancer32 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer31 = "대상아님";
                    strCancer32 = "대상아님";
                }


                //간암(상반기), 치료비지원
                if (VB.InStr(FstrRequest2, "D4") > 0)
                {
                    strCancer41 = VB.Pstr(FstrRequest2, "간암상반기", 2);
                    strCancer41 = VB.Pstr(strCancer41, "SBA_RAT_CD", 2);
                    strCancer41 = VB.Pstr(strCancer41, ":", 2);
                    strCancer41 = VB.Pstr(strCancer41, "\"", 2);

                    if (strCancer41 == "00")
                    {
                        strCancer41 = "본인부담없음";
                        strCancer42 = "본인부담없음";
                    }
                    else
                    {
                        strCancer41 = "10%본인부담";
                        strCancer42 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer41 = "대상아님";
                    strCancer42 = "대상아님";
                }


                //간암(하반기), 치료비지원
                if (VB.InStr(FstrRequest2, "D6") > 0)
                {
                    strCancer61 = VB.Pstr(FstrRequest2, "간암하반기", 2);
                    strCancer61 = VB.Pstr(strCancer61, "SBA_RAT_CD", 2);
                    strCancer61 = VB.Pstr(strCancer61, ":", 2);
                    strCancer61 = VB.Pstr(strCancer61, "\"", 2);

                    if (strCancer61 == "00")
                    {
                        strCancer61 = "본인부담없음";
                        strCancer62 = "본인부담없음";
                    }
                    else
                    {
                        strCancer61 = "10%본인부담";
                        strCancer62 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer61 = "대상아님";
                    strCancer62 = "대상아님";
                }

                //유방암, 유방암 치료비지원
                if (VB.InStr(FstrRequest2, "D3") > 0)
                {
                    strCancer21 = VB.Pstr(FstrRequest2, "유방암", 2);
                    strCancer21 = VB.Pstr(strCancer21, "SBA_RAT_CD", 2);
                    strCancer21 = VB.Pstr(strCancer21, ":", 2);
                    strCancer21 = VB.Pstr(strCancer21, "\"", 2);

                    if (strCancer21 == "00")
                    {
                        strCancer21 = "본인부담없음";
                        strCancer22 = "본인부담없음";
                    }
                    else
                    {
                        strCancer21 = "10%본인부담";
                        strCancer22 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer21 = "대상아님";
                    strCancer22 = "대상아님";
                }

                //자궁경부암, 치료비지원
                if (VB.InStr(FstrRequest2, "D5") > 0)
                {
                    strCancer51 = VB.Pstr(FstrRequest2, "자궁경부암", 2);
                    strCancer51 = VB.Pstr(strCancer51, "SBA_RAT_CD", 2);
                    strCancer51 = VB.Pstr(strCancer51, ":", 2);
                    strCancer51 = VB.Pstr(strCancer51, "\"", 2);

                    if (strCancer51 == "00")
                    {
                        strCancer51 = "본인부담없음";
                        strCancer52 = "본인부담없음";
                    }
                    else
                    {
                        strCancer51 = "10%본인부담";
                        strCancer52 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer51 = "대상아님";
                    strCancer52 = "대상아님";
                }

                //폐암, 치료비지원
                if (VB.InStr(FstrRequest2, "D7") > 0)
                {
                    strCancer71 = VB.Pstr(FstrRequest2, "폐암", 2);
                    strCancer71 = VB.Pstr(strCancer71, "SBA_RAT_CD", 2);
                    strCancer71 = VB.Pstr(strCancer71, ":", 2);
                    strCancer71 = VB.Pstr(strCancer71, "\"", 2);

                    if (strCancer71 == "00")
                    {
                        strCancer71 = "본인부담없음";
                        strCancer72 = "본인부담없음";
                    }
                    else
                    {
                        strCancer71 = "10%본인부담";
                        strCancer72 = "10%본인부담";
                    }

                }
                else
                {
                    strCancer71 = "대상아님";
                    strCancer72 = "대상아님";
                }
                // 암검진 항목 -------------------------End

                //검진 정보 --------------------------------------------------------------------------------------

                if (!FstrRequest2.IsNullOrEmpty())
                {

                    MatchCollection matches = Regex.Matches(FstrRequest3, "HC_KIND_CD");
                    nKIND = matches.Count;
                    nKIND1 = 0;

                    //if (nKIND > 0)
                    //{
                        //1차진단
                        if (VB.Pstr(FstrRequest3, "01\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk01_1 = VB.Pstr(FstrRequest3, "01\"}", 1);
                            strChk01_1 = VB.Pstr(strChk01_1, "WB_HC_DT", nKIND1 + 1);
                            strChk01_1 = VB.Pstr(strChk01_1, ":", 2);
                            strChk01_1 = VB.Pstr(strChk01_1, "\"", 2);


                            //1차진단 출장여부(WB_PLC_TYPE: 출장:"Y")
                            strChk01_2 = VB.Pstr(FstrRequest3, "01\"}", 1);
                            strChk01_2 = VB.Pstr(strChk01_2, "WB_PLC_TYPE", 2);
                            strChk01_2 = VB.Pstr(strChk01_2, ":", 2);
                            strChk01_2 = VB.Pstr(strChk01_2, "\"", 2);
                            if (strChk01_2 == "1")
                            {
                                strChk01_2 = "Y";
                            }
                            else
                            {
                                strChk01_2 = "";
                            }

                            //1차 검진기관(WB_HCC_NM)
                            strChk01_3 = VB.Pstr(FstrRequest3, "01\"}", 1);
                            strChk01_3 = VB.Pstr(strChk01_3, "WB_HCC_NM", 2);
                            strChk01_3 = VB.Pstr(strChk01_3, ":", 2);
                            strChk01_3 = VB.Pstr(strChk01_3, "\"", 2);
                        }
                        if (VB.Pstr(FstrRequest3, "02\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                        }

                        //구강검사
                        if (VB.Pstr(FstrRequest3, "03\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk03_1 = VB.Pstr(FstrRequest3, "03\"}", 1);
                            strChk03_1 = VB.Pstr(strChk03_1, "WB_HC_DT", nKIND1 + 1);
                            strChk03_1 = VB.Pstr(strChk03_1, ":", 2);
                            strChk03_1 = VB.Pstr(strChk03_1, "\"", 2);


                            //구강검사 출장여부(WB_PLC_TYPE: 출장:"Y")
                            strChk03_2 = VB.Pstr(FstrRequest3, "03\"}", 1);
                            strChk03_2 = VB.Pstr(strChk03_2, "WB_PLC_TYPE", 2);
                            strChk03_2 = VB.Pstr(strChk03_2, ":", 2);
                            strChk03_2 = VB.Pstr(strChk03_2, "\"", 2);
                            if (strChk03_2 == "1")
                            {
                                strChk03_2 = "Y";
                            }
                            else
                            {
                                strChk03_2 = "";
                            }

                            //구강검사 검진기관(WB_HCC_NM)
                            strChk03_3 = VB.Pstr(FstrRequest3, "03\"}", 1);
                            strChk03_3 = VB.Pstr(strChk03_3, "WB_HCC_NM", 2);
                            strChk03_3 = VB.Pstr(strChk03_3, ":", 2);
                            strChk03_3 = VB.Pstr(strChk03_3, "\"", 2);
                        }

                        //위암검사(조영)
                        if (VB.Pstr(FstrRequest3, "04\"}", 1) != FstrRequest3)
                        {
                            //위암검사
                            nKIND1 = nKIND1 + 1;
                            strChk04_1 = VB.Pstr(FstrRequest3, "04\"}", 1);
                            strChk04_1 = VB.Pstr(strChk04_1, "WB_HC_DT", nKIND1 + 1);
                            strChk04_1 = VB.Pstr(strChk04_1, ":", 2);
                            strChk04_1 = VB.Pstr(strChk04_1, "\"", 2);
                            //if (!strChk04_1.IsNullOrEmpty()) { strCancer11 = "수검완료" + strCancer11; }
                            if (!strChk04_1.IsNullOrEmpty()) { strCancer11 = "수검완료"; }

                            //위암검사 검진기관
                            strChk04_3 = VB.Pstr(FstrRequest3, "04\"}", 1);
                            strChk04_3 = VB.Pstr(strChk04_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk04_3 = VB.Pstr(strChk04_3, ":", 2);
                            strChk04_3 = VB.Pstr(strChk04_3, "\"", 2);
                        }

                        //대장검사(잠혈)
                        if (VB.Pstr(FstrRequest3, "05\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk05_1 = VB.Pstr(FstrRequest3, "05\"}", 1);
                            strChk05_1 = VB.Pstr(strChk05_1, "WB_HC_DT", nKIND1 + 1);
                            strChk05_1 = VB.Pstr(strChk05_1, ":", 2);
                            strChk05_1 = VB.Pstr(strChk05_1, "\"", 2);
                            //if (!strChk05_1.IsNullOrEmpty()) { strCancer31 = "수검완료" + strCancer31; }
                            if (!strChk05_1.IsNullOrEmpty()) { strCancer31 = "수검완료"; }

                            strChk05_3 = VB.Pstr(FstrRequest3, "05\"}", 1);
                            strChk05_3 = VB.Pstr(strChk05_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk05_3 = VB.Pstr(strChk05_3, ":", 2);
                            strChk05_3 = VB.Pstr(strChk05_3, "\"", 2);
                        }

                        //유방암
                        if (VB.Pstr(FstrRequest3, "06\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk06_1 = VB.Pstr(FstrRequest3, "06\"}", 1);
                            strChk06_1 = VB.Pstr(strChk06_1, "WB_HC_DT", nKIND1 + 1);
                            strChk06_1 = VB.Pstr(strChk06_1, ":", 2);
                            strChk06_1 = VB.Pstr(strChk06_1, "\"", 2);
                            //if (!strChk06_1.IsNullOrEmpty()) { strCancer21 = "수검완료" + strCancer21; }
                            if (!strChk06_1.IsNullOrEmpty()) { strCancer21 = "수검완료"; }

                            strChk06_3 = VB.Pstr(FstrRequest3, "06\"}", 1);
                            strChk06_3 = VB.Pstr(strChk06_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk06_3 = VB.Pstr(strChk06_3, ":", 2);
                            strChk06_3 = VB.Pstr(strChk06_3, "\"", 2);
                        }

                        //자궁경부암검사
                        if (VB.Pstr(FstrRequest3, "07\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk07_1 = VB.Pstr(FstrRequest3, "07\"}", 1);
                            strChk07_1 = VB.Pstr(strChk07_1, "WB_HC_DT", nKIND1 + 1);
                            strChk07_1 = VB.Pstr(strChk07_1, ":", 2);
                            strChk07_1 = VB.Pstr(strChk07_1, "\"", 2);
                            //if (!strChk07_1.IsNullOrEmpty()) { strCancer51 = "수검완료" + strCancer51; }
                            if (!strChk07_1.IsNullOrEmpty()) { strCancer51 = "수검완료"; }

                            strChk07_3 = VB.Pstr(FstrRequest3, "07\"}", 1);
                            strChk07_3 = VB.Pstr(strChk07_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk07_3 = VB.Pstr(strChk07_3, ":", 2);
                            strChk07_3 = VB.Pstr(strChk07_3, "\"", 2);
                        }

                        //간암검사(상반기)
                        if (VB.Pstr(FstrRequest3, "08\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk08_1 = VB.Pstr(FstrRequest3, "08\"}", 1);
                            strChk08_1 = VB.Pstr(strChk08_1, "WB_HC_DT", nKIND1 + 1);
                            strChk08_1 = VB.Pstr(strChk08_1, ":", 2);
                            strChk08_1 = VB.Pstr(strChk08_1, "\"", 2);
                            //if (!strChk08_1.IsNullOrEmpty()) { strCancer41 = "수검완료" + strCancer41 ; }
                            if (!strChk08_1.IsNullOrEmpty()) { strCancer41 = "수검완료" ; }

                            strChk08_3 = VB.Pstr(FstrRequest3, "08\"}", 1);
                            strChk08_3 = VB.Pstr(strChk08_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk08_3 = VB.Pstr(strChk08_3, ":", 2);
                            strChk08_3 = VB.Pstr(strChk08_3, "\"", 2);
                        }

                        //폐암
                        if (VB.Pstr(FstrRequest3, "09\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk10_1 = VB.Pstr(FstrRequest3, "09\"}", 1);
                            strChk10_1 = VB.Pstr(strChk10_1, "WB_HC_DT", nKIND1 + 1);
                            strChk10_1 = VB.Pstr(strChk10_1, ":", 2);
                            strChk10_1 = VB.Pstr(strChk10_1, "\"", 2);
                            //if (!strChk10_1.IsNullOrEmpty()) { strCancer71 = "수검완료" + strCancer71 ; }
                            if (!strChk10_1.IsNullOrEmpty()) { strCancer71 = "수검완료"; }

                            strChk10_3 = VB.Pstr(FstrRequest3, "09\"}", 1);
                            strChk10_3 = VB.Pstr(strChk10_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk10_3 = VB.Pstr(strChk10_3, ":", 2);
                            strChk10_3 = VB.Pstr(strChk10_3, "\"", 2);
                        }

                        //간암검사(하반기)
                        if (VB.Pstr(FstrRequest3, "11\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk09_1 = VB.Pstr(FstrRequest3, "11\"}", 1);
                            strChk09_1 = VB.Pstr(strChk09_1, "WB_HC_DT", nKIND1 + 1);
                            strChk09_1 = VB.Pstr(strChk09_1, ":", 2);
                            strChk09_1 = VB.Pstr(strChk09_1, "\"", 2);
                            //if (!strChk09_1.IsNullOrEmpty()) { strCancer61 = "수검완료" + strCancer61 ; }
                            if (!strChk09_1.IsNullOrEmpty()) { strCancer61 = "수검완료"; }

                            strChk09_3 = VB.Pstr(FstrRequest3, "11\"}", 1);
                            strChk09_3 = VB.Pstr(strChk09_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk09_3 = VB.Pstr(strChk09_3, ":", 2);
                            strChk09_3 = VB.Pstr(strChk09_3, "\"", 2);
                        }

                        //2020-04-01
                        //위암 (내시경)
                        if (VB.Pstr(FstrRequest3, "15\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk15_1 = VB.Pstr(FstrRequest3, "15\"}", 1);
                            strChk15_1 = VB.Pstr(strChk15_1, "WB_HC_DT", nKIND1 + 1);
                            strChk15_1 = VB.Pstr(strChk15_1, ":", 2);
                            strChk15_1 = VB.Pstr(strChk15_1, "\"", 2);
                            //if (!strChk15_1.IsNullOrEmpty()) { strCancer11 = "수검완료" + strCancer11 ; }
                            if (!strChk15_1.IsNullOrEmpty()) { strCancer11 = "수검완료"; }

                            strChk15_3 = VB.Pstr(FstrRequest3, "15\"}", 1);
                            strChk15_3 = VB.Pstr(strChk15_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk15_3 = VB.Pstr(strChk15_3, ":", 2);
                            strChk15_3 = VB.Pstr(strChk15_3, "\"", 2);
                        }

                        //대장검사(조영)
                        if (VB.Pstr(FstrRequest3, "16\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk16_1 = VB.Pstr(FstrRequest3, "16\"}", 1);
                            strChk16_1 = VB.Pstr(strChk16_1, "WB_HC_DT", nKIND1 + 1);
                            strChk16_1 = VB.Pstr(strChk16_1, ":", 2);
                            strChk16_1 = VB.Pstr(strChk16_1, "\"", 2);
                            //if (!strChk16_1.IsNullOrEmpty()) { strCancer31 = "수검완료" + strCancer31 ; }
                            if (!strChk16_1.IsNullOrEmpty()) { strCancer31 = "수검완료"; }

                            strChk16_3 = VB.Pstr(FstrRequest3, "16\"}", 1);
                            strChk16_3 = VB.Pstr(strChk16_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk16_3 = VB.Pstr(strChk16_3, ":", 2);
                            strChk16_3 = VB.Pstr(strChk16_3, "\"", 2);
                        }

                        //대장검사(내시경)
                        if (VB.Pstr(FstrRequest3, "17\"}", 1) != FstrRequest3)
                        {
                            nKIND1 = nKIND1 + 1;
                            strChk17_1 = VB.Pstr(FstrRequest3, "17\"}", 1);
                            strChk17_1 = VB.Pstr(strChk17_1, "WB_HC_DT", nKIND1 + 1);
                            strChk17_1 = VB.Pstr(strChk17_1, ":", 2);
                            strChk17_1 = VB.Pstr(strChk17_1, "\"", 2);
                            //if (!strChk17_1.IsNullOrEmpty()) { strCancer31 = "수검완료" + strCancer31 ; }
                            if (!strChk17_1.IsNullOrEmpty()) { strCancer31 = "수검완료"; }

                            strChk17_3 = VB.Pstr(FstrRequest3, "17\"}", 1);
                            strChk17_3 = VB.Pstr(strChk17_3, "WB_HCC_NM", nKIND1 + 1);
                            strChk17_3 = VB.Pstr(strChk17_3, ":", 2);
                            strChk17_3 = VB.Pstr(strChk17_3, "\"", 2);
                        }

                        //검진정보에 수검여부체크
                        //일반
                        if (strChk01_1.IsNullOrEmpty())
                        {
                            strChk01_1 = VB.Pstr(FstrRequest2, "A0", 2);
                            strChk01_1 = VB.Pstr(strChk01_1, "SUGUMYN", 2);
                            strChk01_1 = VB.Pstr(strChk01_1, ":", 2);
                            strChk01_1 = VB.Pstr(strChk01_1, "\"", 2);
                            if (strChk01_1 == "Y")
                                { strChk01_1 = "수검완료"; strChk01_3 = "타기관수검"; }
                            else
                                { strChk01_1 = ""; }
                        }

                        //구강
                        if (strChk03_1.IsNullOrEmpty())
                        {
                            strChk03_1 = VB.Pstr(FstrRequest2, "C1", 2);
                            strChk03_1 = VB.Pstr(strChk03_1, "SUGUMYN", 2);
                            strChk03_1 = VB.Pstr(strChk03_1, ":", 2);
                            strChk03_1 = VB.Pstr(strChk03_1, "\"", 2);
                            if (strChk03_1 == "Y")
                                { strChk03_1 = "수검완료"; strChk03_3 = "타기관수검"; }
                            else
                                { strChk03_1 = ""; }
                        }

                        //위
                        if (strChk04_1.IsNullOrEmpty())
                        {
                            strChk04_1 = VB.Pstr(FstrRequest2, "D1", 2);
                            strChk04_1 = VB.Pstr(strChk04_1, "SUGUMYN", 2);
                            strChk04_1 = VB.Pstr(strChk04_1, ":", 2);
                            strChk04_1 = VB.Pstr(strChk04_1, "\"", 2);
                            if (strChk04_1 == "Y")
                                { strChk04_1 = "수검완료"; strChk04_3 = "타기관수검"; }
                            else
                                { strChk04_1 = ""; }
                        }

                        //대장
                        if (strChk05_1.IsNullOrEmpty())
                        {
                            strChk05_1 = VB.Pstr(FstrRequest2, "D2", 2);
                            strChk05_1 = VB.Pstr(strChk05_1, "SUGUMYN", 2);
                            strChk05_1 = VB.Pstr(strChk05_1, ":", 2);
                            strChk05_1 = VB.Pstr(strChk05_1, "\"", 2);
                            if (strChk05_1 == "Y")
                                { strChk05_1 = "수검완료"; strChk05_3 = "타기관수검"; }
                            else
                                { strChk05_1 = ""; }
                        }

                        //유방암
                        if (strChk06_1.IsNullOrEmpty())
                        {
                            strChk06_1 = VB.Pstr(FstrRequest2, "D3", 2);
                            strChk06_1 = VB.Pstr(strChk06_1, "SUGUMYN", 2);
                            strChk06_1 = VB.Pstr(strChk06_1, ":", 2);
                            strChk06_1 = VB.Pstr(strChk06_1, "\"", 2);
                            if (strChk06_1 == "Y")
                                { strChk06_1 = "수검완료"; strChk04_3 = "타기관수검"; }
                            else
                                { strChk06_1 = ""; }
                        }

                        //자궁경부암
                        if (strChk07_1.IsNullOrEmpty())
                        {
                            strChk07_1 = VB.Pstr(FstrRequest2, "D5", 2);
                            strChk07_1 = VB.Pstr(strChk07_1, "SUGUMYN", 2);
                            strChk07_1 = VB.Pstr(strChk07_1, ":", 2);
                            strChk07_1 = VB.Pstr(strChk07_1, "\"", 2);
                            if (strChk07_1 == "Y")
                                { strChk07_1 = "수검완료"; strChk07_3 = "타기관수검"; }
                            else
                                { strChk07_1 = ""; }
                        }

                        //간암(상반기)
                        if (strChk08_1.IsNullOrEmpty())
                        {
                            strChk08_1 = VB.Pstr(FstrRequest2, "D4", 2);
                            strChk08_1 = VB.Pstr(strChk08_1, "SUGUMYN", 2);
                            strChk08_1 = VB.Pstr(strChk08_1, ":", 2);
                            strChk08_1 = VB.Pstr(strChk08_1, "\"", 2);
                            if (strChk08_1 == "Y")
                                { strChk08_1 = "수검완료"; strChk08_3 = "타기관수검"; }
                            else
                                { strChk08_1 = ""; }
                        }

                        //폐암
                        if (strChk10_1.IsNullOrEmpty())
                        {
                            strChk10_1 = VB.Pstr(FstrRequest2, "D7", 2);
                            strChk10_1 = VB.Pstr(strChk10_1, "SUGUMYN", 2);
                            strChk10_1 = VB.Pstr(strChk10_1, ":", 2);
                            strChk10_1 = VB.Pstr(strChk10_1, "\"", 2);
                            if (strChk10_1 == "Y")
                                { strChk10_1 = "수검완료"; strChk10_3 = "타기관수검"; }
                            else
                                { strChk10_1 = ""; }
                        }

                        //간암(하반기)
                        if (strChk09_1.IsNullOrEmpty())
                        {
                            strChk09_1 = VB.Pstr(FstrRequest2, "D6", 2);
                            strChk09_1 = VB.Pstr(strChk09_1, "SUGUMYN", 2);
                            strChk09_1 = VB.Pstr(strChk09_1, ":", 2);
                            strChk09_1 = VB.Pstr(strChk09_1, "\"", 2);
                            if (strChk09_1 == "Y")
                                { strChk09_1 = "수검완료"; strChk09_3 = "타기관수검"; }
                            else
                                { strChk09_1 = ""; }
                        }

                    //} 
                }
            }

            //자격조회 완료 및 정보 저장
            if (strOK == "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "WORK_NHIC ";
                SQL += ComNum.VBLF + "    SET PNAME         ='" + strPName.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,BI            ='" + strBI.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,KIHO          ='" + strkiho.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GKIHO         ='" + strGkiho.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,BDATE         ='" + strBDate.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,REL           ='" + strRel.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,JISA          ='" + strJisa.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,YEAR          ='" + strYear.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,TRANS         ='" + strTrans.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EKG           ='" + strEKG.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,FIRST         ='" + strFirst.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,DENTAL        ='" + strDental.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVER         ='" + strLiver.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVER2        ='" + strLiver2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVERC        ='" + strLiverC.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,SECOND        ='" + strSecond.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,FIRSTADD      ='" + str1차Add.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,SECONDADD     ='" + str2차Add.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER11      ='" + strCancer11.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER12      ='" + strCancer12.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER21      ='" + strCancer21.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER22      ='" + strCancer22.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER31      ='" + strCancer31.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER32      ='" + strCancer32.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER41      ='" + strCancer41.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER42      ='" + strCancer42.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER51      ='" + strCancer51.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER52      ='" + strCancer52.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER53      ='" + strCancer53.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER61      ='" + strCancer61.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER62      ='" + strCancer62.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER71      ='" + strCancer71.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER72      ='" + strCancer72.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01       ='" + strChk01_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01_CHUL  ='" + strChk01_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01_NAME  ='" + strChk01_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02       ='" + strChk02_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02_CHUL  ='" + strChk02_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02_NAME  ='" + strChk02_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03       ='" + strChk03_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03_CHUL  ='" + strChk03_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03_NAME  ='" + strChk03_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04       ='" + strChk04_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04_CHUL  ='" + strChk04_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04_NAME  ='" + strChk04_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05       ='" + strChk05_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05_CHUL  ='" + strChk05_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05_NAME  ='" + strChk05_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06       ='" + strChk06_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06_CHUL  ='" + strChk06_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06_NAME  ='" + strChk06_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07       ='" + strChk07_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07_CHUL  ='" + strChk07_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07_NAME  ='" + strChk07_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08       ='" + strChk08_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08_CHUL  ='" + strChk08_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08_NAME  ='" + strChk08_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09       ='" + strChk09_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09_CHUL  ='" + strChk09_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09_NAME  ='" + strChk09_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10       ='" + strChk10_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10_CHUL  ='" + strChk10_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10_NAME  ='" + strChk10_3.Trim() + "' ";
                //2020-04-01(위, 대장 구분)
                SQL += ComNum.VBLF + "       ,GBCHK15       ='" + strChk15_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK15_CHUL  ='" + strChk15_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK15_NAME  ='" + strChk15_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16       ='" + strChk16_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16_CHUL  ='" + strChk16_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16_NAME  ='" + strChk16_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17       ='" + strChk17_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17_CHUL  ='" + strChk17_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17_NAME  ='" + strChk17_3.Trim() + "' ";
                //2018-01-01(추가항목)
                SQL += ComNum.VBLF + "       ,EXAMA         ='" + strExamA.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMD         ='" + strExamD.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAME         ='" + strExamE.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMF         ='" + strExamF.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMG         ='" + strExamG.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMH         ='" + strExamH.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMI         ='" + strExamI.Trim() + "' ";
                //2021-01-29(연장검사)
                SQL += ComNum.VBLF + "       ,REMARK        ='" + strBIGO.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBSTS         ='1' ";
                SQL += ComNum.VBLF + "       ,GBERROR       ='N' ";
                SQL += ComNum.VBLF + "       ,CTIME         =SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE ROWID         = '" + FstrROWID + "'";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "WORK_NHIC          ";
                SQL += ComNum.VBLF + "    SET CTIME=SYSDATE, GBERROR='Y', GBSTS ='2'    ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "'                 ";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

        }

        //자격조회 수진등록
        private void WORK_NHIC_API_INSERT()
        {
            string strJumin = string.Empty;
            string strDate = string.Empty;
            string strChul = string.Empty;
            string strYear = string.Empty;
            string strGubun = string.Empty;
            string strOK = "";

            long nWrtno = 0;

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if(SS1.ActiveSheet.Cells[i,0].Text == "True")
                {
                    nWrtno = Convert.ToInt32(SS1.ActiveSheet.Cells[i, 1].Text);
                    strDate = SS1.ActiveSheet.Cells[i, 2].Text.Replace("-","");
                    strJumin = SS1.ActiveSheet.Cells[i, 5].Text;
                    if (SS1.ActiveSheet.Cells[i, 6].Text == "Y")
                    {
                        strChul = "1";
                    }
                    else
                    {
                        strChul = "2";
                    }
                    strYear = SS1.ActiveSheet.Cells[i, 20].Text;

                    
                    for (int j = 7; j < 20; j++)
                    {
                        if(SS1.ActiveSheet.Cells[i,j].Text =="Y")
                        {
                            //수진등록 대상코드
                            switch (j)
                            {   
                                case 7: strGubun = "01"; break;
                                case 8: strGubun = "03"; break;
                                case 9: strGubun = ""; break;
                                case 10: strGubun = "04"; break;
                                case 11: strGubun = "15"; break;
                                case 12: strGubun = "05"; break;
                                case 13: strGubun = "16"; break;
                                case 14: strGubun = "17"; break;
                                case 15: strGubun = "06"; break;
                                case 16: strGubun = "07"; break;
                                case 17: strGubun = "08"; break;
                                case 18: strGubun = "11"; break;
                                case 19: strGubun = "09"; break;

                                default: break;
                            }


                            var client = new RestClient("https://sis.nhis.or.kr/openapi/hpte100/insertDoneHccInfo.do");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            request.AddHeader("Cookie", "WMONID=QBUDNiF6Uh0; HPSESSIONID=YPnUnyf-GBp9PP7C_TitwA3cSk-k2cAvd9uPd6rTlNGKV_6vN_kj!1069267609!-1256378506");

                            request.AddParameter("SVC_TKN_KEY", "47034b93-7b98-4276-bfa6-05b2810d7cdb");
                            request.AddParameter("HCC_NO", "37100068");

                            request.AddParameter("RRNO", strJumin);
                            request.AddParameter("BZ_YYYY", strYear);
                            request.AddParameter("HC_KIND_CD", strGubun);
                            request.AddParameter("HC_DT", strDate);
                            request.AddParameter("HC_PLC_SPCD", strChul);
                            IRestResponse response = client.Execute(request);
                            Console.WriteLine(response.Content);

                            if(VB.InStr(response.Content, "수검정보를 저장했습니다.") > 0)
                            {
                                strOK = "OK";
                            }
                            else
                            {
                                strOK = "";
                                break;
                            }

                        }

                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = "False";
                    if (strOK == "OK")
                    {
                        int result = hicJepsuService.UpdateGbSujinbyWrtNo(nWrtno);

                        if (result <= 0)
                        {
                            lblMsg.Text = "수진완료 여부 등록에러";
                            SS1.ActiveSheet.Cells[i, 21].Text = "에러";
                        }
                    }
                }
            }


            Display_Pat_List();
            Work_Stop();
            lblMsg.Text = "수진완료 등록완료";

        }

        private void eRdoChanged(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                if (rdoJob1.Checked)
                {
                    Work_Stop();
                    btnSearch.Visible = false;
                    panDate.Visible = false;
                    btnStart.Text = "조회시작";
                    btnStop.Text = "조회중지";

                    tab1.SelectedIndex = 0;
                }
            }
            else if (sender == rdoJob2)
            {
                if (rdoJob2.Checked)
                {
                    Work_Stop();
                    btnSearch.Visible = true;
                    panDate.Visible = true;
                    btnStart.Text = "작업시작";
                    btnStop.Text = "작업중지";

                    tab1.SelectedIndex = 1;
                }
            }
        }

        private void Work_Start()
        {
            if (rdoJob1.Checked)
            {
                lblMsg.Text = "자격조회 시작.. ";
                timer1.Enabled = true;
                timer2.Enabled = false;
            }
            else
            {
                lblMsg.Text = "수검등록 시작.. ";
                timer2.Enabled = true;
                timer1.Enabled = false;
            }

        }

        private void Work_Stop()
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            lblMsg.Text = "작업종료.. ";
        }


        private void eTimerTick1(object sender, EventArgs e)
        {
            Read_Work();
            if (!FstrROWID.IsNullOrEmpty())
            {
                WORK_NHIC_API();
            }
            
        }

        private void eTimerTick2(object sender, EventArgs e)
        {
            if (SS1.ActiveSheet.RowCount > 0)
            {
                WORK_NHIC_API_INSERT();
            }
            
        }

        private void Display_Pat_List()
        {
            int nRow = 0;

            cSpd.Spread_Clear_Simple(SS1);

            IList<HIC_JEPSU_PATIENT> lists = hicJepsuPatientService.GetNhicListByDate(dtpFDate.Text, dtpTDate.Text);

            if (lists.Count > 0)
            {
                SS1.ActiveSheet.RowCount = lists.Count;

                for (int i = 0; i < lists.Count; i++)
                {
                    if (lists[i].WRTNO == 1013018)
                    {
                        i = i;
                    }
                    SS1.ActiveSheet.Rows[nRow].Height = 24;

                    SS1.ActiveSheet.Cells[nRow, 0].Text = "True";
                    SS1.ActiveSheet.Cells[nRow, 1].Text = lists[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[nRow, 2].Text = lists[i].JEPDATE_STR;
                    SS1.ActiveSheet.Cells[nRow, 3].Text = lists[i].GJJONG;
                    SS1.ActiveSheet.Cells[nRow, 4].Text = lists[i].SNAME;
                    SS1.ActiveSheet.Cells[nRow, 5].Text = clsAES.DeAES(lists[i].JUMIN2);
                    if (lists[i].GBCHUL == "Y")
                    {
                        if (lists[i].GBCHUL2 != "Y")
                        {
                            SS1.ActiveSheet.Cells[nRow, 6].Text = "Y";
                        }
                    }
                    SS1.ActiveSheet.Cells[nRow, 8].Text = lists[i].GBDENTAL == "Y" ? "Y" : "";
                    switch (lists[i].GJJONG)
                    {
                        case "11": SS1.ActiveSheet.Cells[nRow, 7].Text = "Y"; break;
                        case "16": SS1.ActiveSheet.Cells[nRow, 9].Text = "Y"; break;
                        default: break;
                    }
                    SS1.ActiveSheet.Cells[nRow, 20].Text = lists[i].GJYEAR;

                    SS1.ActiveSheet.Cells[nRow, 10].Text = hicResultService.GetRowidByOneExcodeWrtno("TX22", lists[i].WRTNO) != null ? "Y" : "";  //위암(조영)
                    SS1.ActiveSheet.Cells[nRow, 11].Text = hicResultService.GetRowidStomachByWrtno(lists[i].WRTNO) != null ? "Y" : "";              //위암(내시경)
                    SS1.ActiveSheet.Cells[nRow, 12].Text = hicResultService.GetRowidByOneExcodeWrtno("TX26", lists[i].WRTNO) != null ? "Y" : "";  //대장암(잠혈)
                    SS1.ActiveSheet.Cells[nRow, 13].Text = hicResultService.GetRowidByOneExcodeWrtno("TX31", lists[i].WRTNO) != null ? "Y" : "";  //대장암(조영)
                    SS1.ActiveSheet.Cells[nRow, 14].Text = hicResultService.GetRowidColonByWrtno(lists[i].WRTNO) != null ? "Y" : "";              //대장암(내시경)
                    SS1.ActiveSheet.Cells[nRow, 15].Text = VB.Mid(lists[i].GBAM, 9, 1) == "1" ? "Y" : "";                                           //유방
                    SS1.ActiveSheet.Cells[nRow, 16].Text = VB.Mid(lists[i].GBAM, 11, 1) == "1" ? "Y" : "";                                          //자궁경부
                    if (VB.Mid(lists[i].GBAM, 7, 1) == "1")
                    {
                        if (string.Compare(VB.Mid(lists[i].JEPDATE_STR, 6, 2), "07") < 0)
                        {
                            SS1.ActiveSheet.Cells[nRow, 17].Text = "Y";      //간암(상반기)
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow, 18].Text = "Y";      //간암(하반기)
                        }
                    }
                    SS1.ActiveSheet.Cells[nRow, 19].Text = VB.Mid(lists[i].GBAM, 13, 1) == "1" ? "Y" : "";      //폐암

                    nRow = nRow + 1;
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Display_Pat_List();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnStart)
            {
                Work_Start();
            }
            else if(sender ==btnStop)
            {
                Work_Stop();
            }
        }


    }
}
