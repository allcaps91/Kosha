using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcExResultSend.cs
/// Description     : 종검검사 결과 전송 프로그램
/// Author          : 김민철
/// Create Date     : 2019-11-01
/// Update History  : 
/// </summary>
/// <history>  
/// 종검 FrmResultSend(임상) / FrmResultSend2(해부) 결과전송 프로그램 통합
/// </history>
/// <seealso cref= "ExHea02.frm(FrmResultSend) / ExHea04.frm(FrmResultSend2)" />
namespace HC_Act
{
    public partial class frmHcExResultSend : Form
    {
        clsSpread cSpd = null;
        ExamSpecmstService examSpecmstService = null;
        ExamResultcService examResultcService = null;
        HicExcodeService hicExcodeService = null;
        HeaJepsuService heaJepsuService = null;
        ExamAnatmstService examAnatmstService = null;

        int FnSS2Row = 0;
        long FnOldPano = 0;
        string FstrWrtOK = "";
        string FstrWRTNO = "";
        string FstrSName = "";
        string FstrSpecNo = "";
        string FstrBDate = "";
        string FstrH813 = "";
        string FstrAnatNO = "";
        string FstrLifeFlag = "N";
        string FstrJobFlag = "S";

        public frmHcExResultSend()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eFormClose);
            this.btnView.Click += new EventHandler(eSearchData);
            this.btnSave_Send.Click += new EventHandler(eSendData);
        }

        private void eSendData(object sender, EventArgs e)
        {
            string strChk          = "";
            string strJobSTS       = "";
            string strSendOK       = "";
            string strOldData      = "";
            string strNewData      = "";
            string strWorkSTS      = "";
            string strTemp         = "";

            FnSS2Row = 0;

            cSpd.Spread_Clear_Simple(SS2, 0);

            Cursor.Current = Cursors.WaitCursor;

            //Sheet에서 전송할 검체번호를 Check
            strOldData = ""; FstrWrtOK = "";
            FnOldPano = 0; 

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strChk = SS1.ActiveSheet.Cells[i, 0].Value.ToString();
                strJobSTS = SS1.ActiveSheet.Cells[i, 9].Text.Trim();
                strSendOK = SS1.ActiveSheet.Cells[i, 11].Text.Trim();
                strWorkSTS = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                strTemp = "";

                if (FstrJobFlag == "S")
                {
                    //if (VB.L(strWorkSTS, "C") > 1 || VB.L(strWorkSTS, "H") > 1 || VB.L(strWorkSTS, "B") > 1) { strTemp = "OK"; }
                    if (strChk.Equals("True") && strSendOK != "OK")
                    {
                        strTemp = "OK";
                    }
                }
                else
                {
                    if (strJobSTS.Equals("검사완료") && strChk.Equals("True") && strSendOK != "OK")
                    {
                        strTemp = "OK";
                    }
                }
                

                if (strTemp.Equals("OK"))
                {
                    FstrWRTNO  = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    FstrSName  = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                    FstrSpecNo = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                    FstrBDate  = SS1.ActiveSheet.Cells[i, 14].Text.Trim();

                    FstrAnatNO = SS1.ActiveSheet.Cells[i, 16].Text.Trim();

                    if (FstrAnatNO == "")
                    {
                        CmdSend_One_SpecNo(strSendOK, i);   //1개의 검체번호를 종합건진 결과에 SEND
                    }
                    else
                    {
                        CmdSend_One_SpecNo_Anat(i);   //1개의 검체번호를 종합건진 결과에 SEND
                    }
                    
                    strNewData = FstrWRTNO;
                    if (strNewData != strOldData)
                    { 
                        ABO_Type_SEND();  //혈액형을 SEND
                        strOldData = strNewData;
                    }
                }
            }

            MessageBox.Show("처방전송 완료", "확인");
            SS2.ActiveSheet.RowCount = FnSS2Row;
            Cursor.Current = Cursors.Default;
        }

        private void CmdSend_One_SpecNo_Anat(int nRow)
        {
            string strResult = "";
            string strExamCode = "";

            //1개의 검체번호를 일반건진 결과에 SEND
            FstrWrtOK = "";

            clsDB.setBeginTran(clsDB.DbCon);

            EXAM_ANATMST item = examAnatmstService.GetItemBySpecnoAnatno(FstrSpecNo, FstrAnatNO);
            if (!item.IsNullOrEmpty())
            {
                strResult = item.RESULT1.Trim();
                strExamCode = item.MASTERCODE.Trim();

                //검사코드 1개의 결과를 일반건진 결과에 전송
                Data_Send_Anat(strExamCode, strResult, FstrAnatNO);
            }

            if (FstrWrtOK == "NO")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
            }
            else
            {
                int result = examSpecmstService.UpDateSendFlag(FstrSpecNo);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("검체마스타에 전송완료 SET 오류", "RollBack");
                }
                else
                {
                    SS1.ActiveSheet.Cells[nRow, 11].Text = "OK";
                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
        }

        /// <summary>
        /// 해부병리는 결과자체가  테이블의 한컬럼에 값이 순서대로 입력이 되어있어서
        /// 하드코딩 하여야합니다.
        /// 추후 일정한 로직이 성립이 되면 자동으로 db화 해서 작업요망됩니다
        /// </summary>
        /// <param name="strExamCode"></param>
        /// <param name="strResult"></param>
        /// <param name="fstrAnatNO"></param>
        private void Data_Send_Anat(string ArgExamCode, string ArgResult, string ArgAnatNo)
        {
            string strHicCode = "";
            string strMedResult1 = "";

            //검사코드가 공란이면 전송 않함
            if (ArgExamCode == "") { return; }
            //검사결과가 공란이면 전송 않함
            if (ArgResult == "") { return; }

            string strWrtno = "";

            FstrWrtOK = "OK";
            //하드코딩 함.

            if (VB.Left(ArgAnatNo, 2) == "PS")      //객담 --------------------------------------------------------------------------
            {
                //LM10 객담세포학적검사    01  음성(Ⅰ)
                //LM10 객담세포학적검사    02  음성(Ⅱ)
                //LM10 객담세포학적검사    03  의양성(Ⅲ)
                //LM10 객담세포학적검사    04  양성(Ⅳ)
                //LM10 객담세포학적검사    05  양성(Ⅴ)
                //LM10 객담세포학적검사    06  검체불량
                //LM10 객담세포학적검사    99  미실시
                strHicCode = "LM10";

                if (VB.Mid(ArgResult, 3, 1) == "1")  //검체불량
                    strMedResult1 = "06";
                else if (VB.Mid(ArgResult, 4, 1) == "1")   //음성(I)
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 5, 1) == "1")  //음성(II)
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 6, 1) == "1")  //음성(III)                
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 7, 1) == "1")  //음성(IV)
                    strMedResult1 = "04";                
                else if (VB.Mid(ArgResult, 8, 1) == "1")  //음성(V)
                    strMedResult1 = "05";

                Data_Send_UPDATE(strMedResult1, strHicCode);

                return;
            }
            else if (VB.Left(ArgAnatNo, 2) == "PU")  // 소변----------------------------------------------------------------------------------
            {
                //LM11 요세포검사(파파니콜라우)    01  음성(Ⅰ)
                //LM11 요세포검사(파파니콜라우)    02  음성(Ⅱ)
                //LM11 요세포검사(파파니콜라우)    03  의양성(Ⅲ)
                //LM11 요세포검사(파파니콜라우)    04  양성(Ⅳ)
                //LM11 요세포검사(파파니콜라우)    06  양성(Ⅴ)
                //LM11 요세포검사(파파니콜라우)    07  검체불량

                strHicCode = "LM11";

                if (VB.Mid(ArgResult, 3, 1) == "1") //검체불량
                    strMedResult1 = "07";
                else if (VB.Mid(ArgResult, 4, 1) == "1") //음성(I)
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 5, 1) == "1") //음성(II)
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 6, 1) == "1") //음성(III)
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 7, 1) == "1") //음성(IV)
                    strMedResult1 = "04";
                else if (VB.Mid(ArgResult, 8, 1) == "1") //음성(V)
                    strMedResult1 = "06";                

                Data_Send_UPDATE(strMedResult1, strHicCode);

                return;
            }
            else if (VB.Left(ArgAnatNo, 1) == "P") //부인과암
            {
                //7항목으로 적용됨
                //A163 부인과 검체상태 01  적정----------------------------------------------1
                //A163 부인과 검체상태 02  부적절
                strHicCode = "A163";
                if (VB.Mid(ArgResult, 1, 1) == "1") //적정
                    strMedResult1 = "01"; //적정
                else if (VB.Mid(ArgResult, 2, 1) == "1") //부적정
                    strMedResult1 = "02"; //부적정
                else
                    strMedResult1 = "";

                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A164 부인과 자궁경부선상피   01  유------------------------------------------2
                //A164 부인과 자궁경부선상피   02  무

                strHicCode = "A164";
                if (VB.Mid(ArgResult, 3, 1) == "1") //유
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 4, 1) == "1") //무
                    strMedResult1 = "02";
                else
                    strMedResult1 = "";

                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A165 부인과 유형별진단   01  음성-------------------------------------------- - 3
                //A165 부인과 유형별진단   03  기타(자궁내막세포출현)
                //A165 부인과 유형별진단   02  상피세포이상

                strHicCode = "A165";
                if (VB.Mid(ArgResult, 5, 1) == "1") // 음성
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 6, 1) == "1") //상피세포이상
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 7, 1) == "1") //기타(자궁내막세포출현)
                    strMedResult1 = "03";
                else
                    strMedResult1 = "";

                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A166 부인과 편평상피세포이상 01  비정형 편평상피세포---------------------------4
                //A166 부인과 편평상피세포이상 03  고등급 편평상피내 병변
                //A166 부인과 편평상피세포이상 04  침윤성 편평세표암종
                //A166 부인과 편평상피세포이상 02  저등급 편평상피내 병변
                strHicCode = "A166";
                if (VB.Mid(ArgResult, 8, 1) == "1")      //비정형 편평상피세포
                    strMedResult1 = "01";
                //else if (VB.Mid(ArgResult, 9, 1) == "1")  //일반
                //   strMedResult1 = "02"
                //else if (VB.Mid(ArgResult, 10, 1) == "1") //고위험
                //   strMedResult1 = "03"
                else if (VB.Mid(ArgResult, 11, 1) == "1") //저등급 편평상피내 병변
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 12, 1) == "1") //고등급 편평상피내 병변
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 13, 1) == "1") //침윤성 편평세표암종
                    strMedResult1 = "04";
                else
                    strMedResult1 = "";
                

                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A167 부인과 선상피세포이상   01  비정형 선상피세포----------------------------------------5
                //A167 부인과 선상피세포이상   03  침윤성 선암종
                //A167 부인과 선상피세포이상   04  직접기입
                //A167 부인과 선상피세포이상   02  상피내 선압종

                strHicCode = "A167";
                if (VB.Mid(ArgResult, 14, 1) == "1")      //비정형 선상피세포
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 15, 1) == "1") //상피내 선압종
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 16, 1) == "1") //침윤성 선암종
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 17, 1) == "1") //직접기입
                    strMedResult1 = "04";
                else
                    strMedResult1 = "";
                

                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A168 부인과 추가소견 01  반응성 세포변화 -------------------------------------------6
                //A168 부인과 추가소견 03  캔디다
                //A168 부인과 추가소견 05  헤르페스 바이러스
                //A168 부인과 추가소견 06  직접기입
                //A168 부인과 추가소견 04  방선균
                //A168 부인과 추가소견 02  트리코모나스

                strHicCode = "A168";
                if (VB.Mid(ArgResult, 18, 1) == "1")      //반응성 세포변화
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 19, 1) == "1")  //트리코모나스
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 20, 1) == "1") //캔디다
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 21, 1) == "1") //방선균
                    strMedResult1 = "04";
                else if (VB.Mid(ArgResult, 22, 1) == "1") //헤르페스 바이러스
                    strMedResult1 = "05";
                else if (VB.Mid(ArgResult, 23, 1) == "1") //기타
                    strMedResult1 = "";
                else
                    strMedResult1 = "";
                
                Data_Send_UPDATE(strMedResult1, strHicCode);

                //A171 부인과종합판정  01  1.정상----------------------------------------------------7
                //A171 부인과종합판정  03  3.상피세포 이상
                //A171 부인과종합판정  05  5.기타
                //A171 부인과종합판정  04  4.자궁경부암 의심
                //A171 부인과종합판정  02  2.염증성 또는 감염성질환

                strHicCode = "A171";
                if (VB.Mid(ArgResult, 24, 1) == "1")      //정상
                    strMedResult1 = "01";
                else if (VB.Mid(ArgResult, 25, 1) == "1")  //염증성 또는 감염성질환
                    strMedResult1 = "02";
                else if (VB.Mid(ArgResult, 26, 1) == "1") //상피세포 이상
                    strMedResult1 = "03";
                else if (VB.Mid(ArgResult, 27, 1) == "1") //자궁경부암 의심
                    strMedResult1 = "04";
                else if (VB.Mid(ArgResult, 28, 1) == "1") //기타
                    strMedResult1 = "05";
                else if (VB.Mid(ArgResult, 29, 1) == "1") //기존 자궁경부암환자
                    strMedResult1 = "";
                else
                    strMedResult1 = "";

                Data_Send_UPDATE(strMedResult1, strHicCode);

            }
            else if (VB.Left(ArgAnatNo, 1) == "C")   //부인과세포검사
            {
                if (ArgExamCode == "YY01")
                    strHicCode = "A161";
                else
                    strHicCode = "A299";

                if (VB.InStr(ArgResult, "DIAGNOSIS") > 0 && VB.InStr(ArgResult, "(상기") > 0)
                    strMedResult1 = VB.Pstr(VB.Pstr(ArgResult, "DIAGNOSIS", 2), "(상기", 1).Trim();
                else
                    strMedResult1 = ArgResult.Trim();

                strMedResult1 = VB.TR(strMedResult1, ComNum.VBLF, "").Trim();

                Data_Send_UPDATE(strMedResult1, strHicCode);
            }
        }

        private void Data_Send_UPDATE(string strMedResult1, string strHicCode)
        {
            if (strMedResult1 == "") { return; }

            string strFlag = "";
            string strGCode = hicExcodeService.GetResCodebyCode(strHicCode);

            if (!strGCode.IsNullOrEmpty())
            {
                strFlag = "OK";
            }
            else
            {
                strFlag = "NO"; //오류처리
            }

            if (strFlag == "OK")
            {
                //TODO : HIC_RESULT_NEW 통합결과 테이블 작업 필요

                //SQL = "UPDATE KOSMOS_PMPA.HEA_RESULT SET result = "
                //SQL = SQL & " '" & strMedResult1 & "', "
                //If strGcode<> "" Then SQL = SQL & " RESCODE ='" & strGcode & "', "
                //SQL = SQL & " EntSabun=222,EntTime=SYSDATE "
                //If strWrtno<> "" Then
                //  SQL = SQL & "WHERE WRTNO IN (" & Val(strWrtno) & ") "
                //Else
                //   SQL = SQL & "WHERE WRTNO=" & Val(FstrWRTNO) & " "
                //End If
                //SQL = SQL & "  AND EXCODE= '" & strHicCode & "' "
                //result = AdoExecute(SQL)
                //If result<> 0 Then
                //   Data_Send_Display(strTYPE, nHicResult, argExamName, strMedResult, argResult, argExamCode, strHicCode);
                //   Exit Sub
                //End If
            }
        }

        private void ABO_Type_SEND()
        {
            string ArgABO = string.Empty;
            string ArgRh  = string.Empty;

            List<EXAM_RESULTC> list = examResultcService.GetABODataBySpecNo(FstrWRTNO.To<long>(), FstrBDate);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].SUBCODE.Trim() == "BB01")
                    {
                        ArgABO = list[i].RESULT.Trim();
                    }
                    else if (list[i].SUBCODE.Trim() == "BB05")
                    {
                        ArgRh = VB.Left(list[i].RESULT.Trim(), 1);
                    }
                }
            }

            //혈액형을 DB에 UPDATE
            if (ArgABO != "" && ArgRh != "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //TODO: HEA_RESULT -> HIC_RESULT_NEW 으로 통합필요

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        private void CmdSend_One_SpecNo(string argSendOK, int nRow)
        {
            string strExamCode = string.Empty;
            string strExamName = string.Empty;
            string strResult = string.Empty;

            //1개의 검체번호를 일반건진 결과에 SEND
            List<EXAM_RESULTC> list = examResultcService.GetListBySpecno(FstrSpecNo);

            FstrWrtOK = ""; FstrH813 = "";

            clsDB.setBeginTran(clsDB.DbCon);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strExamCode = list[i].SUBCODE.Trim();
                    strResult = list[i].RESULT.Trim();
                    strExamName = list[i].EXAMNAME.Trim();

                    //2014-10-20 혈액형검사 바코드 2개 인쇄로 수정
                    if (strExamCode == "BB01") { strExamCode = "BB001"; }

                    //검사코드 1개의 결과를 종합건진 결과에 전송
                    Data_Send_Main(strExamCode, strResult, strExamName);

                    if (FstrWrtOK == "NO") { break; }
                }
            }
                        
           
            if (FstrWrtOK == "NO")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
            }
            //2017-08-17 당일 상담으로 부분완료도 검사실에서 확인한것은 전송요청(최종숙과장)
            else if (argSendOK == "검사완료")
            {
                int result = examSpecmstService.UpDateSendFlag(FstrSpecNo);
                if (result <= 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("검체마스타에 전송완료 SET 오류", "RollBack");
                }
                else
                {
                    SS1.ActiveSheet.Cells[nRow, 11].Text = "OK";
                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
            else
            {
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        private void Data_Send_Main(string argExamCode, string argResult, string argExamName)
        {
            string strHicCode = string.Empty;
            string strTYPE = string.Empty;
            string strMedResult = string.Empty;

            long nHicResult = 0;

            //검사코드가 공란이면 전송 않함
            if (argExamCode == "") { return; }
            //검사결과가 공란이면 전송 않함
            if (argResult == "") { return; }

            //2017-05-25 유전자메틸암검사 자동전송 안함
            if (argExamCode == "TM30") { return; }

            List<HIC_EXCODE> list = hicExcodeService.GetListByExCode(argExamCode);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strHicCode = list[i].CODE.Trim();
                    strTYPE = list[i].RESULTTYPE.Trim();

                    if (strTYPE == "1")     //숫자
                    {
                        //소변검사등 일부검사 결과입력시 ".  결과"형태로 입력되는것 처리
                        if (VB.Left(argResult, 1).Trim() == ".") { argResult = VB.Right(argResult, argResult.Length - 1).Trim(); }

                        ////2019-10-02(종합검진 요청으로 주석처리)
                        //If Left(ArgResult$, 1) = ">" Or Left(ArgResult$, 1) = "<" Then ArgResult$ = Trim(Right(ArgResult$, Len(ArgResult$) - 1))
                        ////결과입력시 >= 1.03형태의 결과 >= 를 제거
                        //If Left(ArgResult$, 2) = ">=" Then ArgResult$ = Trim(Right(ArgResult$, Len(ArgResult$) - 2))

                        nHicResult = 0;
                        if (strHicCode == "A281")
                        {
                            strMedResult = argResult.To<int>().To<string>();
                        }
                        else if (strHicCode == "A282")
                        {
                            strMedResult = (argResult.To<double>() * 1000).To<string>();
                        }
                        else if (strHicCode == "A283")
                        {
                            strMedResult = (argResult.To<double>() * 100).To<int>().To<string>();
                        }
                        else if (VB.Left(argResult, 1).Trim() == "-" || VB.Left(argResult, 1).Trim() == "+")
                        {
                            nHicResult = 0;
                            if (VB.Left(argResult, 2).Trim() == "-")
                            {
                                strMedResult = "-";
                            }
                            else if (VB.Left(argResult, 2).Trim() == "+")
                            {
                                strMedResult = "+";
                            }
                            else
                            {
                                strMedResult = "";
                                for (int k = 0; k < argResult.Length; k++)
                                {
                                    if (VB.Mid(argResult, k, 1).Trim() == "+" || VB.Mid(argResult, k, 1).Trim() == "-")
                                    {
                                        strMedResult = strMedResult + VB.Mid(argResult, k, 1).Trim();
                                    }
                                }
                            }
                        }
                        else
                        {
                            strMedResult = argResult;
                        }
                    }
                    else if (VB.Left(argResult, 1).Trim() == "-" || VB.Left(argResult, 1).Trim() == "+")
                    {
                        nHicResult = 0;
                        strMedResult = VB.Left(argResult, 2).Trim();
                    }
                    else      //문자
                    {
                        nHicResult = 0;
                        strMedResult = argResult;
                    }

                    if ((argExamCode == "CR54A" || argExamCode == "CR54") && strHicCode == "H813")
                    {
                        FstrH813 = strMedResult;
                    }

                    #region Data_Send_UPDATE();
                    string strTemp2 = string.Empty;
                    //변환결과가 오류이면 오류메세지 표시함
                    if (strHicCode == "") { Data_Send_Display(strTYPE, nHicResult, argExamName, strMedResult, argResult, argExamCode, strHicCode); return; }
                    if (strTYPE == "") { Data_Send_Display(strTYPE, nHicResult, argExamName, strMedResult, argResult, argExamCode, strHicCode); return; }
                    if (strTYPE != "1" && strMedResult == "") { Data_Send_Display(strTYPE, nHicResult, argExamName, strMedResult, argResult, argExamCode, strHicCode); return; }

                    //DB에 결과를 UPDATE함
                    if (strHicCode != "H840" && strHicCode != "H841")
                    {
                        if (strHicCode == "TX26")
                        { 
                            if (VB.Left(strMedResult.Trim(), 3).ToUpper() == "REA" || VB.Left(strMedResult.Trim(), 3).ToUpper() == "POS")
                            { 
                               strMedResult = "양성";
                            }
                            else if (VB.Left(strMedResult.Trim(), 3).ToUpper() == "NEG" || VB.Left(strMedResult.Trim(), 3).ToUpper() == "NON")
                            {
                               strMedResult = "음성";
                            }
                        }
                        else
                        {
                            if (VB.Left(strMedResult.Trim(), 3).ToUpper() == "REA" || VB.Left(strMedResult.Trim(), 3).ToUpper() == "POS")
                            {
                                strMedResult = "+";
                            }
                            else if (VB.Left(strMedResult.Trim(), 3).ToUpper() == "NEG" || VB.Left(strMedResult.Trim(), 3).ToUpper() == "NON")
                            { 
                                strMedResult = "-";
                            }
                        }
                        //2008-11-27 종검검사실 전송에서 H813 + 검사실(CR56) = ZE03 자동값계산
                        if (strHicCode == "LH43" && argExamCode == "CR56")
                        {
                            strHicCode = "ZE03";
                            strMedResult = (VB.Val(strMedResult) + VB.Val(FstrH813)).To<string>();
                        }

                        if (strHicCode == "ZD05" || strHicCode == "ZD08")
                        {
                            strTemp2 = VB.Pstr(strMedResult, " ", (int)VB.L(strMedResult, " ") - 1);
                            strTemp2 = strTemp2 + VB.Pstr(strMedResult, " ", (int)VB.L(strMedResult, " "));
                            if (strTemp2.ToLower() == "notfound")
                            {
                                strMedResult = "음성";
                            }
                            else if (strTemp2.ToLower() == "not found")
                            {
                                strMedResult = "음성";
                            }
                        }

                        //TODO : 검진결과 통합으로 HIC_RESULT_NEW 로 치환할것
                       // SQL = "UPDATE KOSMOS_PMPA.HEA_RESULT SET result = "
                       // SQL = SQL & " '" & Trim(strMedResult) & "', "
                       // SQL = SQL & " EntSabun=222,EntTime=SYSDATE "
                       // SQL = SQL & "WHERE WRTNO=" & Val(FstrWRTNO) & " "
                       // SQL = SQL & "  AND EXCODE= '" & ArgHicCode & "' "
                       // result = AdoExecute(SQL)
                       // If result<> 0 Then
                       //    GoSub Data_Send_Display
                       //    Exit Sub
                       //End If
                    }
                    #endregion
                }
            }
        }

        private void Data_Send_Display(string ArgTYPE, double argHicResult, string ArgExamName, string strMedResult, string ArgResult, string ArgExamCode, string strHicCode)
        {
            FstrWrtOK = "NO";

            FnSS2Row = FnSS2Row + 1;
            if (SS2.ActiveSheet.RowCount < FnSS2Row) { SS2.ActiveSheet.RowCount = FnSS2Row; }

            SS2.ActiveSheet.Cells[FnSS2Row, 0].Text = FstrWRTNO;
            SS2.ActiveSheet.Cells[FnSS2Row, 1].Text = FstrSName;
            SS2.ActiveSheet.Cells[FnSS2Row, 2].Text = "";

            if (ArgTYPE == "1")
            {
                SS2.ActiveSheet.Cells[FnSS2Row, 3].Text = "숫자";
                SS2.ActiveSheet.Cells[FnSS2Row, 6].Text = argHicResult.To<string>();
            }
            else
            {
                SS2.ActiveSheet.Cells[FnSS2Row, 3].Text = "문자열";
                SS2.ActiveSheet.Cells[FnSS2Row, 6].Text = strMedResult;
            }

            SS2.ActiveSheet.Cells[FnSS2Row, 4].Text = ArgExamName;
            SS2.ActiveSheet.Cells[FnSS2Row, 5].Text = ArgResult;
            SS2.ActiveSheet.Cells[FnSS2Row, 7].Text = "오류";
            SS2.ActiveSheet.Cells[FnSS2Row, 8].Text = strHicCode;
            SS2.ActiveSheet.Cells[FnSS2Row, 9].Text = ArgExamCode;
            SS2.ActiveSheet.Cells[FnSS2Row, 10].Text = FstrBDate;
        }

        private void eFormClose(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void eSearchData(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(SS1, 0);

            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strSendFlag = rdoJob1.Checked == true ? "NO" : rdoJob2.Checked == true ? "OK" : "";

            FstrJobFlag = rdoGbn1.Checked == true ? "S" : "A";

            long nWRTNO = 0;


            SS1.DataSource = examSpecmstService.GetListByHicList(strFDate, strTDate, "61", txtSName.Text, strSendFlag, FstrJobFlag);
           
            if (SS1.ActiveSheet.RowCount > 0)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    nWRTNO = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();

                    HEA_JEPSU item = heaJepsuService.GetHeaJepsubyWrtNo(nWRTNO);

                    if (!item.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 15].Text = item.GJJONG;
                        SS1.ActiveSheet.Cells[i, 18].Text = item.JEPDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 19].Text = item.PANO.To<string>();
                    }
                }
            }
            
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpFDate.Text = DateTime.Now.AddDays(1).ToShortDateString();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            examSpecmstService = new ExamSpecmstService();
            examResultcService = new ExamResultcService();
            hicExcodeService = new HicExcodeService();
            heaJepsuService = new HeaJepsuService();
            examAnatmstService = new ExamAnatmstService();

            SS1.Initialize();
            SS1.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SS1.AddColumn("S",        "",                               40, FpSpreadCellType.CheckBoxCellType);
            SS1.AddColumn("병실번호", nameof(EXAM_SPECMST.ROOM),        44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("접수번호", nameof(EXAM_SPECMST.HICNO),       64, FpSpreadCellType.TextCellType);
            SS1.AddColumn("수검자명", nameof(EXAM_SPECMST.SNAME),       74, FpSpreadCellType.TextCellType);
            SS1.AddColumn("나이",     nameof(EXAM_SPECMST.AGESEX),      44, FpSpreadCellType.TextCellType);
            SS1.AddColumn("진료과",   nameof(EXAM_SPECMST.DEPTCODE),    46, FpSpreadCellType.TextCellType);
            SS1.AddColumn("의사명",   nameof(EXAM_SPECMST.DRNAME),      64, FpSpreadCellType.TextCellType);
            SS1.AddColumn("검체번호", nameof(EXAM_SPECMST.SPECNO),      75, FpSpreadCellType.TextCellType);
            SS1.AddColumn("결과일시", nameof(EXAM_SPECMST.RESULTDATE), 114, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SS1.AddColumn("상태",     nameof(EXAM_SPECMST.STATUS),      64, FpSpreadCellType.TextCellType);
            SS1.AddColumn("검사종류", nameof(EXAM_SPECMST.EXNAME),     180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SS1.AddColumn("전송",     nameof(EXAM_SPECMST.SENDFLAG),    44, FpSpreadCellType.TextCellType);
            SS1.AddColumn("W/S",      nameof(EXAM_SPECMST.WORKSTS),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("검체약어", nameof(EXAM_SPECMST.SPECCODE),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("진료일자", nameof(EXAM_SPECMST.BDATE),       74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });

            SS1.AddColumn("검진종류", "",                               74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("검진일자", "",                               74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("검진번호", "",                               74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("해부병리번호", nameof(EXAM_SPECMST.ANATNO),  74, FpSpreadCellType.TextCellType);
            SS1.AddColumn("등록번호", nameof(EXAM_SPECMST.PANO),        74, FpSpreadCellType.TextCellType);
        }
    }
}
