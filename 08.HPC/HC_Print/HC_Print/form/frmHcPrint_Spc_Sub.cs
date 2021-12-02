using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Spc_Sub.cs
/// Description     : 특수검진 결과지 인쇄
/// Author          : 김경동
/// Create Date     : 2021-05-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm검진결과지특수_2019.frm(FrmIDateChange)" />

namespace HC_Print
{
    public partial class frmHcPrint_Spc_Sub : Form
    {
        bool FbSS2_Print = false;
        bool FbSpcNewReport = false;            //2015-04-01 특수검진 1차,2차 분리 대상 여부(True:대상)

        int FnFlag = 0;
        int FnCnt = 0;
        int FnJulCNT = 0;
        int FnRowCnt = 0;
        int FnAge = 0;
        int FnExamCnt = 0;                      //기타검사건수
        int FnPanCNT = 0;                       //판정건수
        int FnSpcPanCNT = 0;                    //특수판정건수
        int FnTimeCNT = 0;
        int FnP = 0;                            //스프레드 결정("0"->SS2(0),"1"-> SS2(1))
        int FnSogenCnt = 0;

        long fnWrtno = 0;
        long FnDrno = 0;
        long FnDentDrno = 0;
        long FnWRTNO1 = 0;                      //특수1차 접수번호
        long FnWrtno2 = 0;                      //특수2차 접수번호
        long FnPano = 0;                        //건진 등록번호
  
        private string[] FstrJule = new string[4];
        private string[] FstrOldYear = new string[2];   //종전 검사년도 1,2

        string FstrUCodes = "";
        string FUCodes = "";
        string FstrSExams = "";
        string FstrLtdName = "";
        string FstrName = "";
        string FstrJumin = "";
        string FstrWrtno = "";
        string FstrGjjong = "";
        string FstrGjChasu = "";
        string FstrPtno = "";
        string FstrBasicExam1 = "";             //기본검사항목,줄,칸
        string FstrBasicExam2 = "";             //순음청력검사
        string FstrSQL1 ="";                    //기본검사의 검사항목들('A101','A102',...) :   SQL의 IN 항목
        string FstrSQL2 = "";                   //순음청력검사 검사항목들('TH11','TH12',...) : SQL의 IN 항목
        string FstrGjYear = "";                 //건진년도
        string FstrJepdate = "";                //건진일자
        string FstrTongbodate = "";             //통보일자
        string FstrSex = "";                    //성별(검사결과 참고치)
        string FstrChasu = "";
        string FstrFlag = "";                   //채용일때 이전 검사 사용X
        string FstrFlag1 = "";                  //일+특 일때 1차에 통보일 업데이트
        string FstrFlag2 = "";                  //특수이고 2차 실시한경우
        string FstrFlag3 = "";                  //건강위험도구표 인쇄 플래그
        string FstrSpc = "";                    //일+특일때 인쇄구분
        string FstrDent = "";                   //치과
        string FstrLifeGbn = "";
        string FstrDntSts = "";
        string Fstr채용검진 = "";                //채용검진
        string FstrDaePanjeng = "";             //대표판정
        string FstrDaeSuhang = "";              //대표업무수행
        string FstrSogen = "";
        string FstrTongbo = "";
        string FstrJepDateOK = "";

        //옵션
        string fstrTxtTongbo = "";
        string fstrChkTongbo = "";

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();
        clsHcMain hm = new clsHcMain();

        HicJepsuService hicJepsuService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicResSpecialService hicResSpecialService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicJepsuResSpecialPatientService hicJepsuResSpecialPatientService = null;
        HicResDentalService hicResDentalService = null;

        public frmHcPrint_Spc_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }
        public frmHcPrint_Spc_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }

        private void SetControl()
        {
            hicJepsuService = new HicJepsuService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();
            hicResSpecialService = new HicResSpecialService();
            hicSangdamNewService = new HicSangdamNewService();
            hicJepsuResSpecialPatientService = new HicJepsuResSpecialPatientService();
            hicResDentalService = new HicResDentalService();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Sub();

            ComFunc.Delay(1500);
            this.Close();

        }

        private void Result_Print_Sub()
        {
            Result_Print_Clear();       //Sheet Clear
            Result_Print_Main_1();
            Result_Print_Sub1();        //검진년도, 반기, 등록번호, 특수1차,2차의 접수번호
            Result_Print_Sub2();        //인적사항,문진,임상진찰,사업주등...










        }

        private void Result_Print_Clear()
        {

            FstrOldYear[1] = ""; FstrOldYear[2] = "";
            FstrDent = "";
            FbSpcNewReport = false;


            SS21_CLEAR();
            SS22_CLEAR();


        }

        private void Result_Print_Main_1()
        {

            int i = 0;
            int nRead = 0;

            string strBangi = "'";
            string strGjChasu = "";
            string strJepdate = "";
            string strUcode = "";
            string strUcodes = "";
            string strGjjong = "";
            string strLtdCode = "";

            string strJuso = "";
            string strGbJuso = "";
            string strOpt1 = "";
            string strPANHIS = "";
            string strPanGDate = "";
            string strPandate = "";
            string strMsg = "";
            string strUpdateOK = "";

            //검진년도, 반기, 등록번호
            HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(fnWrtno);
            if (!item.IsNullOrEmpty())
            {
                FstrGjYear = item.GJYEAR.Trim();
                strGjChasu = item.GJCHASU.Trim();
                FstrGjChasu = item.GJCHASU.Trim();
                strBangi = item.GJBANGI.Trim();
                FnPano = item.PANO;
                FstrSex = item.SEX.Trim();
                strJepdate = item.JEPDATE.Trim();
                strUcodes = item.UCODES.Trim();
                FstrGjjong = item.GJJONG.Trim();
                strGjjong = item.GJJONG.Trim();
                strLtdCode = VB.Format(item.LTDCODE, "#");
                FstrPtno = item.PTNO.Trim();
            }

            nRead = 0;
            //당해년도,반기의 1차,2차 접수번호를 읽음
            List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyPanoLtdYearChasuGjjong(FnPano, strLtdCode, FstrGjYear, strGjChasu, strGjjong, strOpt1);
            nRead = list.Count;
            if (list.Count == 1)
            {
                FnWRTNO1 = list[0].WRTNO;
                FnWrtno2 = 0;

            }


            if (list.Count > 1)
            {
                FnWRTNO1 = list[0].WRTNO;

                if (strGjChasu == "2")
                {
                    FnWrtno2 = list[nRead - 1].WRTNO;

                    strPANHIS = "OK";
                    for (i = 0; i < nRead; i++)
                    {
                        FnWRTNO1 = list[i].WRTNO;
                        strPANHIS = hb.Read_SPC_PANJENG(FnWRTNO1);
                        if (strPANHIS == "OK")
                        {
                            break;
                        }
                    }
                }
            }

            // FstrTongbodate = VB.IIf(strSpecial == "Y", 1, 0).ToString();

            //2차가 아닌경우 지정한 접수번호로 출력
            if (strGjChasu != "2") { FnWRTNO1 = fnWrtno; }
            //2차인경우 
            if (strGjChasu == "2") { FnWrtno2 = fnWrtno; }

            if (!list[0].GBSPC.IsNullOrEmpty()) { FstrFlag2 = "Y"; }

            i = 0;
            if (FnWrtno2 > 0) { i = 1; }


            FstrJepdate = list[i].JEPDATE;
            strPanGDate = list[i].JEPDATE;
            strPandate = list[i].JEPDATE;

            //특수검진 1차,2차 결과지 분리 시행

            FbSpcNewReport = false;

            if (FnWrtno2 == 0)
            {
                //if( strJepdate >= "2015-04-01") { FbSpcNewReport = true; }
            }
            else
            {
                //if( hicJepsuService.GetJepDatebyWrtNo(FnWRTNO1) >= "2015-04-01") { FbSpcNewReport = true; }
            }

            if ((strGjjong == "11"|| strGjjong == "11" || strGjjong == "11" || strGjjong == "11" || strGjjong == "11" || strGjjong == "11") && !strUcodes.IsNullOrEmpty())
            {
                HIC_RES_BOHUM1 item1 = null;
                if (FnWrtno2 == 0)
                {
                    item1 = hicResBohum1Service.GetPanjengDatebyWrtno(FnWRTNO1, "1");
                }
                else
                {
                    item1 = hicResBohum1Service.GetPanjengDatebyWrtno(FnWrtno2, "2");
                }

                strPandate = item1.PANJENGDATE;

                if(Convert.ToDateTime(FstrTongbodate) > Convert.ToDateTime(strPanGDate) || Convert.ToDateTime(FstrTongbodate) < Convert.ToDateTime(strPandate))
                {
                    strMsg = "공단통보일이 검진일로 부터 14일 경과 하였거나" + ComNum.VBLF;
                    strMsg += "판정일이 통보일보다 큽니다. 확인요망!" + ComNum.VBLF;
                    strMsg += "판정일: " + strPandate + " " + ComNum.VBLF;
                    strMsg += "정상통보일: " + strPanGDate + "까지";

                    MessageBox.Show(strMsg, "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
            if (fstrChkTongbo == "False")
            {
                //인쇄완료처리(1차)
                HIC_RES_BOHUM1 item2 = hicResBohum1Service.GetItemByWrtno(FnWRTNO1);
                if(!item2.IsNullOrEmpty())
                {
                    strUpdateOK = "";
                    if (item2.GBPRINT != "Y") { strUpdateOK = "미출력"; }
                    if (item2.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                    if (item2.TONGBOGBN.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                    if (item2.PRTSABUN==0) { strUpdateOK = "미출력"; }

                    if(strUpdateOK =="미출력")
                    {
                        hicResBohum1Service.UpdateTongBoInfobyWrtNo(FnWRTNO1, fstrTxtTongbo, "1", clsType.User.IdNumber);
                    }
                }

                //인쇄완료처리(2차)
                HIC_RES_BOHUM2 item3 = hicResBohum2Service.GetItemByWrtno(FnWrtno2);
                if (!item3.IsNullOrEmpty())
                {
                    strUpdateOK = "";
                    if (item3.GBPRINT != "Y") { strUpdateOK = "미출력"; }
                    if (item3.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                    if (item3.TONGBOGBN.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                    if (item3.PRTSABUN == 0) { strUpdateOK = "미출력"; }

                    if (strUpdateOK == "미출력")
                    {
                        hicResBohum1Service.UpdateTongBoInfobyWrtNo(FnWrtno2, fstrTxtTongbo, "1", clsType.User.IdNumber);
                    }
                }

                HIC_RES_SPECIAL item4 = hicResSpecialService.GetItemByWrtno(FnWRTNO1);
                if(!item4.IsNullOrEmpty())
                {
                    strUpdateOK = "";
                    if (item4.GBPRINT != "Y") { strUpdateOK = "미출력"; }
                    if (item4.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                    if (item4.PRTSABUN == 0) { strUpdateOK = "미출력"; }

                    if (strUpdateOK == "미출력")
                    {
                        hicResSpecialService.UpdatePRINTbyWrtNo(FnWRTNO1, Convert.ToInt32(clsType.User.IdNumber));
                    }
                }

                if(FnWrtno2 > 0)
                {
                    HIC_RES_SPECIAL item5 = hicResSpecialService.GetItemByWrtno(FnWrtno2);
                    if (!item5.IsNullOrEmpty())
                    {
                        strUpdateOK = "";
                        if (item5.GBPRINT != "Y") { strUpdateOK = "미출력"; }
                        if (item5.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "미출력"; }
                        if (item5.PRTSABUN == 0) { strUpdateOK = "미출력"; }

                        if (strUpdateOK == "미출력")
                        {
                            hicResSpecialService.UpdatePRINTbyWrtNo(FnWrtno2, Convert.ToInt32(clsType.User.IdNumber));
                        }
                    }
                }

                //접수테이블에 통보일자 세팅최초값-갱신안됨
                HIC_JEPSU item6 = hicJepsuService.GetItemByWRTNO(fnWrtno);
                if(!item6.IsNullOrEmpty())
                {
                    hicJepsuService.UpdatePRINTbyWrtNo(fnWrtno, Convert.ToInt32(clsType.User.IdNumber), fstrTxtTongbo);
                }
            }
        }

        private void Result_Print_Sub1()
        {
            int i = 0;
            int K = 0;
            int nRead = 0;
            int nAge = 0;
            string strUcodes = "";
            string strUNames = "";
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strJepDate2 = "";
            string strNextDate = "";
            string strData = "";
            string strTongBo = "";
            string strTemp ="";
            string strTemp1="";
            string strJong = ""; 
            string strGb1Year = "";
            string strPjSangdam = "";
            string strTongBoDATA = "";
            string strJuso = "";             //(2차검진 접수테이블 주소)
            string strGbJuso = "";

            string strMailcode = "";
            string strJongName = "";

            strJuso = ""; strGbJuso="";

            //표적장기별 상담내역
            HIC_SANGDAM_NEW item1 = hicSangdamNewService.GetItembyWrtNo(FnWRTNO1);
            if(!item1.IsNullOrEmpty())
            {
                strPjSangdam = item1.PJSANGDAM.Trim();
            }

            //접수마스터를 읽음
            HIC_JEPSU_RES_SPECIAL_PATIENT item = hicJepsuResSpecialPatientService.GetItembyWrtno(FnWRTNO1);
            if(!item.IsNullOrEmpty())
            {
                strUcodes = item.UCODES;
                strJumin = item.JUMIN;
                strLtdCode = VB.Format(item.LTDCODE, "#");
                strJepDate = item.JEPDATE;
                strJong = item.GJJONG.Trim();
                FstrGjChasu = item.GJCHASU.Trim();
                nAge = Convert.ToInt32(item.AGE);
                strGbJuso = item.GBJUSO;
                FstrJepdate = strJepDate;
            }


            if (FnWrtno2 != 0)
            {
                HIC_JEPSU item2 = hicJepsuService.GetItemByWRTNO(FnWrtno2);
                if(!item2.IsNullOrEmpty())
                {
                    strJepDate = item2.JEPDATE;
                    strJuso = item2.JUSO1 + " " + VB.Replace(item2.JUSO2, ComNum.VBLF, "");
                    strGbJuso = item2.GBJUSO.Trim();
                }
            }

            if (FnWrtno2 != 0)
            {
                SS_ETC.ActiveSheet.Cells[8, 7].Text = strJuso;
            }
            else
            {
                SS_ETC.ActiveSheet.Cells[8,7].Text = item.JUSO1 + " " + VB.Replace(item.JUSO2, ComNum.VBLF, "");
            }

            if(strGbJuso.IsNullOrEmpty()|| strGbJuso =="N")
            {
                SS_ETC.ActiveSheet.Cells[10, 8].Text = " " + item.SNAME + " 귀하" + "(" + hb.READ_Ltd_Name(VB.Format(item.LTDCODE, "#")) + ")";
            }
            else
            {
                SS_ETC.ActiveSheet.Cells[10, 8].Text = " " + item.SNAME + " 귀하";
            }

            //2차 수검일 표시관련 보완
            SS_ETC.ActiveSheet.Cells[11, 12].Text = strJepDate;
            SS_ETC.ActiveSheet.Cells[12, 8].Text = "";

            for (i = 1; i < 4; i++)
            {
                strMailcode += VB.Mid(item.MAILCODE, i, 1) + " ";
            }
            strMailcode += "- ";
            for (i = 4; i < 7; i++)
            {
                strMailcode += VB.Mid(item.MAILCODE, i, 1) + " ";
            }

            SS_ETC.ActiveSheet.Cells[12, 8].Text = strMailcode;


            //현대제철의 경우 부서/사번입력(일반검진 요청)
            SS_ETC.ActiveSheet.Cells[12, 12].Text = "";
            if (!item.BUSENAME.IsNullOrEmpty() || !item.SABUN.IsNullOrEmpty())
            {
                if (strGbJuso.IsNullOrEmpty() || strGbJuso == "N")
                {
                    SS_ETC.ActiveSheet.Cells[12, 12].Text = "(" + item.BUSENAME + " " + item.SABUN + ")";
                }
            }

            FstrFlag = ""; FstrFlag1 = ""; FstrFlag3 = ""; FstrSpc = "";


            switch (VB.Val(item.GBSPC))
            {
                case 1:
                    strJongName = "특 수";
                    FstrFlag1 = "Y"; FstrSpc = "Y";
                    FstrFlag3 = VB.IIf(VB.Left(strJong, 1) == "4", "LIFE", "Y").ToString();
                    break;
                case 2:
                    strJongName = "특 수"; FstrFlag1 = "Y"; 
                    break;
                case 3:
                    strJongName = "배 치 전"; FstrFlag = "Y";
                    break;
                case 4:
                    strJongName = "채용+배치전"; FstrFlag = "Y";
                    break;
                case 5:
                    strJongName = "수 시";
                    break;
                case 6:
                    strJongName = "임 시";
                    break;
                case 7:
                    strJongName = "채 용"; FstrFlag = "Y";
                    break;
                case 8:
                    strJongName = "일 반"; FstrFlag3 = "Y";
                    break;
                case 9:
                    strJongName = "배 치 전"; FstrFlag = "Y";
                    FstrFlag3 = VB.IIf(VB.Left(strJong, 1) == "4", "LIFE", "Y").ToString();
                    break;
                case 10:
                    strJongName = "공무원채용";
                    break;
                case 11:
                    strJongName = "추적검사"; 
                    break;
                case 13:
                    strJongName = "신 체";
                    break;
                case 14:
                    strJongName = "일 반";
                    break;
                case 99:
                    strJongName = "추적검사"; 

                    break;
                default:
                    strJongName = "특 수";
                    break;
            }

            SS21.ActiveSheet.Cells[2, 10].Text = strJongName;

            if (strJong == "21") { SS21.ActiveSheet.Cells[2, 10].Text = "채 용"; FstrFlag = "Y"; }
            if (strJong == "24") { SS21.ActiveSheet.Cells[2, 10].Text = "배 치 전"; FstrFlag = "Y"; }

            if(FbSpcNewReport = true && FstrChasu == "2")
            {
                SS21.ActiveSheet.Cells[2, 10].Text = "2차";
                SS21.ActiveSheet.Cells[1, 7].Text = "2차";
            }

            if(strJong == "21")
            {
                SS31.ActiveSheet.Cells[2, 10].Text = "채 용";
                SS31.ActiveSheet.Cells[2, 10].Text = "채 용";
            }
            else
            {
                SS31.ActiveSheet.Cells[2, 10].Text = strJongName;
                SS31.ActiveSheet.Cells[2, 10].Text = strJongName;
            }

            //유해인자, 주민번호, 성명, 나이, 성별, 사원번호
            //SS5생략

            FUCodes = strUcodes;
            strUNames = hm.UCode_Names_Display(strUcodes);
            strUNames = VB.Replace(strUNames, "(일특)", "");

            SS21.ActiveSheet.Cells[4, 5].Text = strUNames;
            SS21.ActiveSheet.Cells[7, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
            SS21.ActiveSheet.Cells[7, 16].Text = " " + item.SNAME;
            SS21.ActiveSheet.Cells[7, 24].Text = " " + item.AGE;
            FnAge = Convert.ToInt32(item.AGE);
            if(item.SEX =="M")
            {
                SS21.ActiveSheet.Cells[7, 30].Text = "남";
            }
            else if ( item.SEX =="F")
            {
                SS21.ActiveSheet.Cells[7, 30].Text = "여";
            }

            SS21.ActiveSheet.Cells[15, 4].Text = strUNames;
            SS21.ActiveSheet.Cells[7, 36].Text = item.SABUN; //사원번호

            SS21.ActiveSheet.Cells[8, 6].Text = item.JUSO1 + " " + VB.Replace(item.JUSO2, ComNum.VBLF, "");
            SS21.ActiveSheet.Cells[8, 36].Text = item.TEL;
            if(!item.HPHONE.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[8, 36].Text = item.HPHONE.Trim();
            }

            //사업장명,업종명,현작업부서,현작업공정,현직전입일,현직근무기간
            SS21.ActiveSheet.Cells[9, 6].Text = hb.READ_Ltd_Name(VB.Format(item.LTDCODE, "#"));
            SS21.ActiveSheet.Cells[9, 26].Text = hb.READ_HIC_CODE("05", item.JIKJONG);
            SS21.ActiveSheet.Cells[10, 6].Text = item.BUSENAME;
            SS21.ActiveSheet.Cells[10, 18].Text = hb.READ_HIC_CODE("06", item.GONGJENG);
            SS21.ActiveSheet.Cells[11, 6].Text = item.IPSADATE;
            SS21.ActiveSheet.Cells[11, 15].Text = item.JENIPDATE;

            //과거직력1
            SS21.ActiveSheet.Cells[13, 3].Text = item.OLDGONG1;
            if(item.OLDMYEAR1 > 0 || item.OLDDAYTIME1 >0)
            {
                SS21.ActiveSheet.Cells[13, 20].Text = item.OLDMYEAR1.ToString() + "년 " + item.OLDDAYTIME1.ToString() + "시간";
            }
            //과거직력2
            SS21.ActiveSheet.Cells[14, 3].Text = item.OLDGONG2;
            if (item.OLDMYEAR2 > 0 || item.OLDDAYTIME2 > 0)
            {
                SS21.ActiveSheet.Cells[14, 14].Text = item.OLDMYEAR2.ToString() + "년 " + item.OLDDAYTIME2.ToString() + "시간";
            }

            //과거병력
            if(item.MUN_OLDMSYM.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[12, 27].Text = "무";
            }
            else
            {
                SS21.ActiveSheet.Cells[12, 27].Text = " " + item.MUN_OLDMSYM.Trim();
            }

            //가족력
            if (item.MUN_GAJOK.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[13, 27].Text = "무";
            }
            else
            {
                SS21.ActiveSheet.Cells[13, 27].Text = " " + item.MUN_GAJOK.Trim();
            }

            //업무기인성
            if (item.MUN_GIINSUNG.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[14, 27].Text = "무";
            }
            else
            {
                SS21.ActiveSheet.Cells[14, 27].Text = " " + item.MUN_GIINSUNG.Trim();
            }

            //임상진찰(안과,이비인후,피부,치아),휴유증상
            if(strPjSangdam.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[12, 33].Text = "안과";
                if(!item.JIN_NEURO.IsNullOrEmpty())
                {
                    SS21.ActiveSheet.Cells[12, 38].Text = " " + item.JIN_NEURO;
                }
                else
                {
                    SS21.ActiveSheet.Cells[12, 38].Text = " 무";
                }

                SS21.ActiveSheet.Cells[13, 33].Text = "이비인후";
                if (!item.JIN_HEAD.IsNullOrEmpty())
                {
                    SS21.ActiveSheet.Cells[13, 38].Text = " " + item.JIN_HEAD;
                }
                else
                {
                    SS21.ActiveSheet.Cells[13, 38].Text = " 무";
                }

                SS21.ActiveSheet.Cells[14, 33].Text = "피부";
                if (!item.JIN_SKIN.IsNullOrEmpty())
                {
                    SS21.ActiveSheet.Cells[14, 38].Text = " " + item.JIN_SKIN;
                }
                else
                {
                    SS21.ActiveSheet.Cells[14, 38].Text = " 무";
                }
            }

            if(!item.JENGSANG.IsNullOrEmpty())
            {
                if( FstrGjYear.To<long>(0) < 2007)
                {
                    SS21.ActiveSheet.Cells[18, 4].Text = hb.READ_HIC_CODE("11",item.JENGSANG.Trim());
                }
                else
                {
                    SS21.ActiveSheet.Cells[18, 4].Text = hb.READ_HIC_CODE("84",item.JENGSANG.Trim());
                }
            }
            else
            {
                SS21.ActiveSheet.Cells[18, 4].Text = "무 ";
            }

            //구강검진
            FnDentDrno = 0;
            
            if (!item.DENTSOGEN.IsNullOrEmpty())
            {
                SS21.ActiveSheet.Cells[24, 6].Text = item.DENTSOGEN.Trim();
                SS21.ActiveSheet.Cells[24, 36].Text = hb.READ_License_DrName(item.DENTDOCT);
                FnDentDrno = item.DENTDOCT;
                FstrDent = "Y";
            }

            //채용 구강검진
            if(FstrGjjong == "21")
            {
                HIC_RES_DENTAL item2 = hicResDentalService.GetItemByWrtno(FnWRTNO1);
                if(!item2.IsNullOrEmpty())
                {
                    SS21.ActiveSheet.Cells[24, 6].Text = item2.SANGDAM.Trim();
                    SS21.ActiveSheet.Cells[24, 36].Text = hb.READ_License_DrName(item2.PANJENGDRNO);



                }




            }





            //지방노동사무소,대표자,보건관리대행자








        }

        private void Result_Print_Sub2()
        {
        }
        private void SS21_CLEAR()
        {
            SS21.ActiveSheet.Cells[1, 42].Text = "";
            SS21.ActiveSheet.Cells[2, 10].Text = "";    //개인표 제목
            SS21.ActiveSheet.Cells[4, 5].Text = "";     //유해인자

            SS21.ActiveSheet.Cells[5, 6].Text = "";     //주민등록번호
            SS21.ActiveSheet.Cells[5, 16].Text = "";    //성명
            SS21.ActiveSheet.Cells[5, 24].Text = "";    //나이
            SS21.ActiveSheet.Cells[5, 30].Text = "";    //성별
            SS21.ActiveSheet.Cells[5, 36].Text = "";    //사원번호

            SS21.ActiveSheet.Cells[8, 6].Text = "";
            SS21.ActiveSheet.Cells[8, 36].Text = "";

            SS21.ActiveSheet.Cells[9, 6].Text = "";
            SS21.ActiveSheet.Cells[9, 26].Text = "";

            SS21.ActiveSheet.Cells[10, 6].Text = "";
            SS21.ActiveSheet.Cells[10, 18].Text = "";
            SS21.ActiveSheet.Cells[10, 36].Text = "";

            SS21.ActiveSheet.Cells[11, 6].Text = "";
            SS21.ActiveSheet.Cells[11, 15].Text = "";
            SS21.ActiveSheet.Cells[11, 27].Text = "";
            SS21.ActiveSheet.Cells[11, 36].Text = "";

            SS21.ActiveSheet.Cells[13, 3].Text = "";
            SS21.ActiveSheet.Cells[13, 11].Text = "";
            SS21.ActiveSheet.Cells[13, 14].Text = "";

            SS21.ActiveSheet.Cells[14, 3].Text = "";
            SS21.ActiveSheet.Cells[14, 11].Text = "";
            SS21.ActiveSheet.Cells[14, 14].Text = "";

            SS21.ActiveSheet.Cells[12, 27].Text = "";
            SS21.ActiveSheet.Cells[13, 27].Text = "";
            SS21.ActiveSheet.Cells[14, 27].Text = "";

            SS21.ActiveSheet.Cells[12, 33].Text = "";
            SS21.ActiveSheet.Cells[13, 33].Text = "";
            SS21.ActiveSheet.Cells[14, 33].Text = "";

            SS21.ActiveSheet.Cells[12, 38].Text = "";
            SS21.ActiveSheet.Cells[13, 38].Text = "";
            SS21.ActiveSheet.Cells[14, 38].Text = "";

            SS21.ActiveSheet.Cells[15, 4].Text = "";
            SS21.ActiveSheet.Cells[18, 4].Text = "";
            SS21.ActiveSheet.Cells[19, 18].Text = "";
            SS21.ActiveSheet.Cells[20, 18].Text = "";

            SS21.ActiveSheet.Cells[22, 1].Text = "";
            SS21.ActiveSheet.Cells[22, 6].Text = "";
            SS21.ActiveSheet.Cells[22, 9].Text = "";
            SS21.ActiveSheet.Cells[22, 12].Text = "";
            SS21.ActiveSheet.Cells[22, 15].Text = "";
            SS21.ActiveSheet.Cells[22, 18].Text = "";
            SS21.ActiveSheet.Cells[22, 32].Text = "";
            SS21.ActiveSheet.Cells[22, 42].Text = "";

            SS21.ActiveSheet.Cells[23, 1].Text = "";
            SS21.ActiveSheet.Cells[23, 6].Text = "";
            SS21.ActiveSheet.Cells[23, 9].Text = "";
            SS21.ActiveSheet.Cells[23, 12].Text = "";
            SS21.ActiveSheet.Cells[23, 15].Text = "";
            SS21.ActiveSheet.Cells[23, 18].Text = "";
            SS21.ActiveSheet.Cells[23, 32].Text = "";

            SS21.ActiveSheet.Cells[24, 6].Text = "";
            SS21.ActiveSheet.Cells[24, 9].Text = "";
            SS21.ActiveSheet.Cells[24, 36].Text = "";

            for (int i = 27; i<=50; i++)
            {
                SS21.ActiveSheet.Cells[i, 1].Text = "";
                SS21.ActiveSheet.Cells[i, 12].Text = "";
                SS21.ActiveSheet.Cells[i, 25].Text = "";
                SS21.ActiveSheet.Cells[i, 32].Text = "";
                SS21.ActiveSheet.Cells[i, 38].Text = "";
            }

            SS21.ActiveSheet.Cells[57, 1].Text = "";
            SS21.ActiveSheet.Cells[57, 30].Text = "";

            for (int i = 60; i <= 65; i++)
            {
                SS21.ActiveSheet.Cells[i, 3].Text = "";
                SS21.ActiveSheet.Cells[i, 16].Text = "";
                SS21.ActiveSheet.Cells[i, 32].Text = "";
                SS21.ActiveSheet.Cells[i, 42].Text = "";
            }

            SS21.ActiveSheet.Cells[63, 6].Text = "";
            SS21.ActiveSheet.Cells[63, 14].Text = "";
            SS21.ActiveSheet.Cells[63, 22].Text = "";
            SS21.ActiveSheet.Cells[63, 30].Text = "";
            SS21.ActiveSheet.Cells[63, 37].Text = "";
            SS21.ActiveSheet.Cells[64, 6].Text = "";
        }

        private void SS22_CLEAR()
        {
            SS22.ActiveSheet.Cells[1, 42].Text = "";
            SS22.ActiveSheet.Cells[2, 10].Text = "";
            SS22.ActiveSheet.Cells[4, 5].Text = "";

            SS22.ActiveSheet.Cells[7, 6].Text = "";
            SS22.ActiveSheet.Cells[7, 16].Text = "";
            SS22.ActiveSheet.Cells[7, 24].Text = "";
            SS22.ActiveSheet.Cells[7, 30].Text = "";
            SS22.ActiveSheet.Cells[7, 36].Text = "";

            SS22.ActiveSheet.Cells[8, 6].Text = "";
            SS22.ActiveSheet.Cells[8, 36].Text = "";

            SS22.ActiveSheet.Cells[9, 6].Text = "";
            SS22.ActiveSheet.Cells[9, 26].Text = "";

            SS22.ActiveSheet.Cells[10, 6].Text = "";
            SS22.ActiveSheet.Cells[10, 18].Text = "";
            SS22.ActiveSheet.Cells[10, 36].Text = "";

            SS22.ActiveSheet.Cells[11, 6].Text = "";
            SS22.ActiveSheet.Cells[11, 15].Text = "";
            SS22.ActiveSheet.Cells[11, 27].Text = "";
            SS22.ActiveSheet.Cells[11, 36].Text = "";

            SS22.ActiveSheet.Cells[13, 3].Text = "";
            SS22.ActiveSheet.Cells[13, 11].Text = "";
            SS22.ActiveSheet.Cells[13, 14].Text = "";

            SS22.ActiveSheet.Cells[14, 3].Text = "";
            SS22.ActiveSheet.Cells[14, 11].Text = "";
            SS22.ActiveSheet.Cells[14, 14].Text = "";

            SS22.ActiveSheet.Cells[12, 27].Text = "";
            SS22.ActiveSheet.Cells[13, 27].Text = "";
            SS22.ActiveSheet.Cells[14, 27].Text = "";

            SS22.ActiveSheet.Cells[12, 33].Text = "";
            SS22.ActiveSheet.Cells[13, 33].Text = "";
            SS22.ActiveSheet.Cells[14, 33].Text = "";

            SS22.ActiveSheet.Cells[12, 38].Text = "";
            SS22.ActiveSheet.Cells[13, 38].Text = "";
            SS22.ActiveSheet.Cells[14, 38].Text = "";

            SS22.ActiveSheet.Cells[15, 4].Text = "";
            SS22.ActiveSheet.Cells[18, 4].Text = "";
            SS22.ActiveSheet.Cells[19, 18].Text = "";
            SS22.ActiveSheet.Cells[20, 18].Text = "";

            SS22.ActiveSheet.Cells[22, 1].Text = "";
            SS22.ActiveSheet.Cells[22, 6].Text = "";
            SS22.ActiveSheet.Cells[22, 9].Text = "";
            SS22.ActiveSheet.Cells[22, 12].Text = "";
            SS22.ActiveSheet.Cells[22, 15].Text = "";
            SS22.ActiveSheet.Cells[22, 18].Text = "";
            SS22.ActiveSheet.Cells[22, 32].Text = "";
            SS22.ActiveSheet.Cells[22, 42].Text = "";

            SS22.ActiveSheet.Cells[23, 1].Text = "";
            SS22.ActiveSheet.Cells[23, 6].Text = "";
            SS22.ActiveSheet.Cells[23, 9].Text = "";
            SS22.ActiveSheet.Cells[23, 12].Text = "";
            SS22.ActiveSheet.Cells[23, 15].Text = "";
            SS22.ActiveSheet.Cells[23, 18].Text = "";
            SS22.ActiveSheet.Cells[23, 32].Text = "";

            SS22.ActiveSheet.Cells[24, 6].Text = "";
            SS22.ActiveSheet.Cells[24, 9].Text = "";
            SS22.ActiveSheet.Cells[24, 36].Text = "";

            for (int i = 27; i <= 64; i++)
            {
                SS22.ActiveSheet.Cells[i, 1].Text = "";
                SS22.ActiveSheet.Cells[i, 12].Text = "";
                SS22.ActiveSheet.Cells[i, 25].Text = "";
                SS22.ActiveSheet.Cells[i, 32].Text = "";
                SS22.ActiveSheet.Cells[i, 38].Text = "";
            }

            SS22.ActiveSheet.Cells[60, 6].Text = "";
            SS22.ActiveSheet.Cells[60, 14].Text = "";
            SS22.ActiveSheet.Cells[60, 22].Text = "";
            SS22.ActiveSheet.Cells[60, 30].Text = "";
            SS22.ActiveSheet.Cells[60, 37].Text = "";
            SS22.ActiveSheet.Cells[61, 6].Text = "";

            SS22.ActiveSheet.Cells[62, 1].Text = "";

            SS_ETC.ActiveSheet.Cells[8, 6].Text = "";
            SS_ETC.ActiveSheet.Cells[10, 6].Text = "";
            SS_ETC.ActiveSheet.Cells[10, 7].Text = "";
            SS_ETC.ActiveSheet.Cells[11, 12].Text = "";
            SS_ETC.ActiveSheet.Cells[12, 8].Text = "";
            SS_ETC.ActiveSheet.Cells[12, 12].Text = "";


        }

    }
}
