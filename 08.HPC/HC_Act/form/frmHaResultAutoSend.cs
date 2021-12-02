using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaResultAutoSend.cs
/// Description     : 종검결과자동전송
/// Author          : 이상훈
/// Create Date     : 2019-08-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검결과자동전송.frm(Frm종검결과자동전송)" />

namespace HC_Act
{
    public partial class frmHaResultAutoSend : Form
    {
        XrayResultnewService xrayResultnewService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HicResultService hicResultService = null;
        HeaResultService heaResultService = null;
        HicJepsuService hicJepsuService = null;
        HicMunjinNightService hicMunjinNightService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;

        clsHcAct ha = new clsHcAct();
        clsHcMain hm = new clsHcMain();

        long FnWRTNO;
        long FnHeaWRTNO;
        string FstrPtno;
        string FstrGjYear;
        string FstrSex;
        long FnAge;
        string FstrJepDate;
        string FstrGjChasu;
        string FstrGjJong;
        string FstrUCodes;

        public frmHaResultAutoSend()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            xrayResultnewService = new XrayResultnewService();
            hicJepsuResultService = new HicJepsuResultService();
            hicResultService = new HicResultService();
            heaResultService = new HeaResultService();
            hicJepsuService = new HicJepsuService();
            hicMunjinNightService = new HicMunjinNightService();
            comHpcLibBService = new ComHpcLibBService();
            heaJepsuService = new HeaJepsuService();
            hicResultExCodeService = new HicResultExCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            progressBar1.Value = 0;
            lblMsg.Text = "";
            lblRate.Text = "";

            timer1.Enabled = false;
            fn_Hea_Send_Main();
        }

        void fn_Hea_Send_Main()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            //종검 흉부촬영 결과를 일반건진에 전송함
            fn_SEND_HEA_XRay_Result();
            //종검결과를 일반건진에 전송함
            fn_HEA_Send_Process();
            //야간작업문진료 전송
            fn_NightMunjin_SEND_Process();
            //프로그램 종료
            timer1.Enabled = true;
        }

        /// <summary>
        /// 종검 흉부촬영 결과를 일반건진에 전송함
        /// </summary>
        void fn_SEND_HEA_XRay_Result()
        {
            int nREAD = 0;
            int nREAD2 = 0;
            int nRate = 0;
            long nHeaPano = 0;
            string strRes = "";
            string strResName = "";
            long nWRTNO = 0;
            string strJumin = "";
            string strPtNo = "";
            string strJepDate = "";
            string strSex = "";
            string strResult = "";
            string strNewPan = "";
            string strXCode = "";
            string strDeptCode = "";

            strDeptCode = "TO";
            strXCode = "GR2101";

            progressBar1.Value = 0;
            lblMsg.Text = "";

            List<XRAY_RESULTNEW> list = xrayResultnewService.GetItembyXCode(strXCode, strDeptCode);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strJepDate = list[i].SEEKDATE.To<string>("").Trim();
                strPtNo = list[i].PANO;
                strResult = list[i].PANHIC;
                nRate = VB.Fix((i + 1) / nREAD * 100);
                progressBar1.Value = nRate;
                lblRate.Text = nRate + "%";
                lblMsg.Text = (i + 1) + "/" + nREAD + " 방사선 판독결과 전송";
                Application.DoEvents();

                nWRTNO = 0;
                if (!strResult.IsNullOrEmpty())
                {
                    HIC_JEPSU_RESULT list2 = hicJepsuResultService.GetItembyPtNo(strJepDate, strPtNo);

                    if (!list2.IsNullOrEmpty())
                    {
                        nWRTNO = list2.WRTNO;
                        strJepDate = list2.JEPDATE;
                        strSex = list2.SEX;

                        switch (strResult)
                        {
                            //case "01":
                            //    strRes = "01";
                            //    strResName = "정상(A)";
                            //    break;
                            //case "02":
                            //    strRes = "01";
                            //    strResName = "정상(A)";
                            //    break;
                            //case "03":
                            //    strRes = "02";
                            //    strResName = "사진불량(B)";
                            //    break;
                            //case "04":
                            //    strRes = "03";
                            //    strResName = "비활동성(C)";
                            //    break;
                            //case "08":
                            //    strRes = "07";
                            //    strResName = "폐결핵의증(E)";
                            //    break;
                            //case "09":
                            //    strRes = "08";
                            //    strResName = "비폐결핵성질환(F)#";
                            //    break;
                            //case "10":
                            //    strRes = "09";
                            //    strResName = "순환기계질환(F)";
                            //    break;
                            case "01":
                                strRes = "01";
                                strResName = "정상";
                                break;
                            case "02":
                                strRes = "02";
                                strResName = "사진불량";
                                break;
                            case "03":
                                strRes = "03";
                                strResName = "비활동성(정상)";
                                break;
                            case "07":
                                strRes = "07";
                                strResName = "폐결핵 의증";
                                break;
                            case "08":
                                strRes = "08";
                                strResName = "비결핵성 질환";
                                break;
                            case "09":
                                strRes = "09";
                                strResName = "순환기계 질환";
                                break;
                            case "10":
                                strRes = "10";
                                strResName = "진단미정";
                                break;
                            case "11":
                                strRes = "11";
                                strResName = "미촬영";
                                break;
                            case "12":
                                strRes = "12";
                                strResName = "유질환자";
                                break;
                            case "13":
                                strRes = "13";
                                strResName = "비활동성 폐결핵";
                                break;
                            default:
                                strResName = "";
                                break;
                        }

                        if (!strResName.IsNullOrEmpty())
                        {
                            strNewPan = "";
                            if (strRes != "01")
                            {
                                strNewPan = "R";
                            }

                            HIC_RESULT item = new HIC_RESULT();

                            item.PANJENG = strNewPan;
                            item.ENTSABUN = 111;
                            //item.RESULT = strResult;
                            item.RESULT = strResName;
                            item.WRTNO = nWRTNO;
                            item.EXCODE = "A154";

                            int result = hicResultService.Update_Auto_Result_Hea(item, "1");

                            if (result < 0)
                            {
                                MessageBox.Show("종검결과 자동 전송 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            item.PANJENG = strNewPan;
                            item.ENTSABUN = 111;
                            item.RESULT = strRes;
                            item.WRTNO = nWRTNO;
                            item.EXCODE = "A142";

                            int result1 = hicResultService.Update_Auto_Result_Hea(item, "2");

                            if (result1 < 0)
                            {
                                MessageBox.Show("종검결과 자동 전송 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 종검결과를 일반건진에 전송함
        /// </summary>
        void fn_HEA_Send_Process()
        {
            int nREAD = 0;
            long nWRTNO = 0;
            string strPtNo = "";
            string strJepDate = "";
            string strXrayno = "";
            string strYoil = "";
            int nRate = 0;
            List<string> strExcode = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);

            progressBar1.Value = 0;
            lblMsg.Text = "";

            //종검 결과전송은 익일 전송됨
            List<HIC_JEPSU> list = hicJepsuService.GetItembyGbSts("1");
            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nRate = VB.Fix(i + 1) / nREAD * 100;
                progressBar1.Value = nRate;
                lblRate.Text = nRate + "%";

                FnWRTNO = list[i].WRTNO;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                FstrJepDate = list[i].JEPDATE;
                strPtNo = list[i].PTNO;
                strXrayno = list[i].XRAYNO;

                if (strXrayno.IsNullOrEmpty())
                {
                    strXrayno = string.Format("{0:#0}", FnWRTNO);
                }

                lblMsg.Text = (i + 1) + "/" + nREAD + " 종검결과 전송";
                //Application.DoEvents();

                if (FnWRTNO == 1167031)
                {
                    i = i;
                }

                if (!strXrayno.IsNullOrEmpty())
                {
                    string[] strExCodes = { "A215" };
                    if (hicResultService.GetCountbyWrtNo(FnWRTNO, strExCodes) > 0)
                    {
                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = strXrayno;
                        item.ENTSABUN = 111;
                        item.WRTNO = FnWRTNO;
                        item.EXCODE = "A215";

                        int result = hicResultService.Update_Auto_Result_Hea(item, "3");

                        if (result < 0)
                        {
                            MessageBox.Show("종검결과 전송중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // 종검접수에서 종검번호를 찾음
                FnHeaWRTNO = 0;
                FnHeaWRTNO = heaJepsuService.GetWrtNobyJepDate(strPtNo, FstrJepDate);

                if (FnHeaWRTNO > 0)
                {
                    fn_Hea_Send_Sub();

                    HIC_RESULT item = new HIC_RESULT();

                    item.RESULT = ".";
                    item.ENTSABUN = 111;
                    item.WRTNO = FnWRTNO;
                    strExcode.Clear();
                    strExcode.Add("TX24");

                    //TX24는 결과가 없으면 자동으로 점을 찍음
                    int result = hicResultService.Update_ResultbyWrtNo(item, strExcode, "");

                    if (result < 0)
                    {
                        MessageBox.Show("종검결과 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //비만도 계산 및 Update
                    hm.Biman_Gesan(FnWRTNO, "HIC");                        //체질량 자동계산 'A117
                    hm.Update_Audiometry(FnWRTNO);                  //기도청력 시 기본청력 정상입력

                    //hm.MDRD_GFR_Gesan(FnWRTNO, FstrSex, FnAge,"HIC");     //MDRD-GFR 자동계산 2012년부터
                    hm.AIR3_AUTO(FnWRTNO, "HIC");                          //AIR 3분법 자동계산
                    hm.AIR6_AUTO(FnWRTNO, "HIC");                          //AIR 6분법 자동계산
                    hm.LDL_Gesan(FnWRTNO);                          //LDL콜레스테롤 계산
                    hm.TIBC_Gesan(FnWRTNO);                         //TIBC총철결합능 계산     

                    //string[] strExCodes = { "ZD00", "LU38", "LU54", "A291" };
                    string[] strExCodes = { "LU38", "LU54", "A291" };


                    //구강검사(ZD00) 누락자는 "." 찍기
                    if (hicResultService.GetCountbyWrtNo(FnWRTNO, strExCodes) > 0)
                    {
                        HIC_RESULT item1 = new HIC_RESULT();

                        item1.RESULT = ".";
                        item1.ENTSABUN = 111;
                        item1.WRTNO = FnWRTNO;
                        item1.EXCODE = "'ZD00','LU38','LU54','A291'";

                        int result1 = hicResultService.Update_Auto_Result_Hea(item1, "3");
                        
                        if (result1 < 0)
                        {
                            MessageBox.Show("검사결과 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //접수마스타의 상태를 변경
                    hm.Result_EntryEnd_Check(FnWRTNO);
                }
            }
        }

        void fn_Hea_Send_Sub()
        {
            int nREAD = 0;
            string strExCode = "";
            string strNewCode = "";
            string strResult = "";
            string strNewPan = "";
            string strResult_New = "";

            if (FnHeaWRTNO == 0)
            {
                return;
            }

            //종검의 결과를 읽음
            //List<HIC_RESULT> list = hicResultService.GetExCodeResultbyWrtNo(FnWRTNO);

            List<HEA_RESULT> list = heaResultService.GetExCodeResultbyWrtNo1(FnHeaWRTNO);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;

                switch (strExCode)
                {
                    case "C203":
                        if (strResult == ".")
                        {
                            strNewCode = strExCode; //교정시력(좌)
                        }
                        else
                        {
                            strNewCode = "A104"; //교정시력(좌)
                        }
                        break;
                    case "C204":
                        if (strResult == ".")
                        {
                            strNewCode = strExCode; //교정시력(우)
                        }
                        else
                        {
                            strNewCode = "A105"; //교정시력(우)
                        }
                        break;
                    case "A151":
                        strNewCode = "A153";    //심전도결과
                        break;
                    case "TH12":
                        strNewCode = "A106";    //청력(좌)
                        break;
                    case "TH22":
                        strNewCode = "A107";    //청력(우)
                        break;
                    case "A258":
                        strNewCode = "A131";    //HBs-Ag
                        break;
                    case "A259":
                        strNewCode = "A132";    //HBs-Ab
                        break;
                    case "A264":
                        strNewCode = "A257";    //알파피토단백
                        break;
                    case "A272":
                        strNewCode = "LU53";    //요침사
                        break;
                    case "LU48":
                        strNewCode = "A112";    //요단백
                        break;
                    case "LU44":
                        strNewCode = "LU44";    //요잠혈
                        break;
                    case "LU51":
                        strNewCode = "LU51";    //요산도 PH
                        break;
                    case "TZ46":
                        strNewCode = "TZ46";    //소변니코틴검사
                        break;
                    default:
                        strNewCode = strExCode;
                        break;
                }

                strResult = list[i].RESULT.To<string>("").Trim();
                if (strExCode == "TH12" || strExCode == "TH22")
                {
                    strResult_New = list[i].RESULT;
                }

                //청력일경우 숫자를 -> 정상 , 비정상으로
                if ((strNewCode == "A106" || strNewCode == "A107") && strResult != "본인제외")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        //if (VB.Val(strResult) >= VB.Val("39"))
                        //{
                        //    strResult = "정상";
                        //}
                        //else if (VB.Val(strResult) >= VB.Val("40"))
                        //{
                        //    strResult = "비정상";
                        //}
                        if (VB.Val(strResult).To<int>(0) <= 39)
                        {
                            strResult = "정상";
                        }
                        else if (VB.Val(strResult).To<int>(0) >= 40)
                        {
                            strResult = "비정상";
                        }
                    }
                }
                else if ((strNewCode == "A106" || strNewCode == "A107") && strResult == "본인제외")
                {
                    strResult = "미실시";
                }

                //치아검사는 . 으로
                if (strNewCode == "ZD00")
                {
                    strResult = ".";
                }

                if (strNewCode == "A111" || strNewCode == "A112" || strNewCode == "A113")
                {
                    switch (strResult)
                    {
                        case "음성":
                            strResult = "01";
                            break;
                        case "-":
                            strResult = "01";
                            break;
                        case "+-":
                            strResult = "02";
                            break;
                        case "양성":
                            strResult = "03";
                            break;
                        case "+":
                            strResult = "03";
                            break;
                        case "++":
                            strResult = "04";
                            break;
                        case "+++":
                            strResult = "05";
                            break;
                        case "++++":
                            strResult = "06";
                            break;
                        default:
                            strResult = "";
                            break;
                    }
                }

                //교정시력 결과에 괄호를 추가함
                if (strExCode == "C203" || strExCode == "C204")
                {
                    strResult = "(" + strResult + ")";
                }
                strNewPan = hm.ExCode_Result_Panjeng(strNewCode, strResult, FstrSex, FstrJepDate, "").Trim();

                //일반건진 검사결과를 읽음
                HIC_RESULT lst = hicResultService.GetResultRowIdbyWrtNo(FnWRTNO, strNewCode, strExCode);

                if (lst != null)
                {
                    if (strResult.Length > 200)
                    {
                        strResult = "종검결과 길이초과";
                    }

                    HIC_RESULT item = new HIC_RESULT();

                    item.RESULT = strResult;
                    item.PANJENG = strNewPan;
                    item.ENTSABUN = 111;
                    item.RID = lst.RID;

                    if (strResult != "")
                    {
                        int result = hicResultService.Update_Auto_Result_Hic(item);
                    }
                }

                //검사결과 수치 추가 업데이트
                if (strExCode == "TH12" || strExCode == "TH22")
                {
                    HIC_RESULT lst1 = hicResultService.GetResultRowIdbyExCode(FnWRTNO, strExCode);

                    if (lst1 != null)
                    {
                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = strResult_New;
                        item.PANJENG = strNewPan;
                        item.ENTSABUN = 111;
                        item.RID = lst1.RID;

                        int result = hicResultService.Update_Auto_Result_Hic(item);

                        if (result < 0)
                        {
                            MessageBox.Show("검사결과 수치 추가 update시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            //액팅코드에 자동으로 01 달아주기
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNo(FnWRTNO);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    if (list2[i].EXCODE != "ZD99")
                    {
                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = "01";
                        item.ENTSABUN = 111;
                        item.WRTNO = list2[i].WRTNO;
                        item.EXCODE = list2[i].EXCODE;

                        int result = hicResultService.Update_ResultbyWrtNo_Auto(item);

                        if (result < 0)
                        {
                            MessageBox.Show("액팅코드 자동 01 update 시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 야간작업문진료 전송
        /// </summary>
        void fn_NightMunjin_SEND_Process()
        {
            int nREAD = 0;
            int nREAD2 = 0;
            int nRate = 0;
            bool bOK = false;
            long nWRTNO = 0;
            string strMunRes = "";
            string strTemp = "";
            string strRes = "";
            string strPanjeng = "";
            long nJemsu = 0;

            progressBar1.Value = 0;
            lblMsg.Text = "";

            List<HIC_JEPSU> list = hicJepsuService.GetItembyUCodes("V01", "V02");

            nREAD = list.Count;

            for (int i = 0; i < nREAD; i++)
            {
                nRate = VB.Fix((i + 1) / nREAD * 100);
                progressBar1.Value = nRate;
                lblRate.Text = nRate + "%";
                lblMsg.Text = (i + 1) + "/" + nREAD + " 야간작업문진표 전송";
                //Application.DoEvents();

                nWRTNO = list[i].WRTNO;

                bOK = true;
                if (list[i].IEMUNNO == 0)
                {
                    bOK = false;
                }

                if (bOK == true)
                {
                    if (!hicMunjinNightService.GetCountMunjinbyIemunNo(nWRTNO).IsNullOrEmpty())
                    {
                        bOK = false;
                    }
                }

                if (bOK == true)
                {
                    strMunRes = comHpcLibBService.GetMunjinResByIEMunNo(list[i].IEMUNNO);

                    strRes = "";
                    nJemsu = 0;

                    if (!strMunRes.IsNullOrEmpty() && strMunRes.To<int>() != 0)
                    {
                        strTemp = VB.Pstr(VB.Pstr(VB.Pstr(strMunRes, "{<*>}tbl_night{*}", 2), "{*}", 2), "{<*>}", 1);
                        if (VB.L(strTemp, "{}") > 1)
                        {
                            for (int j = 15; j <= 21; j++)
                            {
                                nJemsu += long.Parse(VB.Pstr(VB.Pstr(strTemp, "{}", j), ",", 2)) - 1;
                                strRes += string.Format(VB.Pstr(VB.Pstr(strTemp, "{}", j), ",", 2), "0");
                            }
                        }
                    }
                    if (strRes.IsNullOrEmpty())
                    {
                        bOK = false;
                    }
                }

                if (bOK == true)
                {
                    if (nJemsu <= 7)
                    {
                        strPanjeng = "1"; //정상
                    }
                    else if (nJemsu <= 14)
                    {
                        strPanjeng = "2"; //경미한 불면증"
                    }
                    else if (nJemsu <= 21)
                    {
                        strPanjeng = "3"; //중증도 불면증"
                    }
                    else
                    { 
                        strPanjeng = "4"; //심한 불면증"
                    }

                    HIC_MUNJIN_NIGHT item = new HIC_MUNJIN_NIGHT();

                    item.WRTNO = nWRTNO;
                    item.ITEM1_DATA = strRes;
                    item.ITEM1_JEMSU = nJemsu;
                    item.ITEM1_PANJENG = strPanjeng;
                    item.ENTSABUN = 111;

                    int result = hicMunjinNightService.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("야간작업 문진표 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void eTimerTick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
