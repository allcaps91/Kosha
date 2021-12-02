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
/// Class Name      : HC_Pan
/// File Name       : frmPanjeng_LifeStyle.cs
/// Description     : 생활습관처방전
/// Author          : 이상훈
/// Create Date     : 2020-05-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm1차일반특수판정_2019.frm(Frm1차일반특수판정_2019) " />

namespace HC_Pan
{
    public partial class frmPanjeng_LifeStyle : Form
    {
        frmPanjeng_Hic frmHcPanHic = new frmPanjeng_Hic();
        HicSpcPanjengService hicSpcPanjengService = null;
        HicResSpecialService hicResSpecialService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanhisService hicSpcPanhisService = null;
        HicCodeService hicCodeService = null;
        HicOrgancodeService hicOrgancodeService = null;
        HicBcodeService hicBcodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        ComHpcLibBService comHpcLibBService = null;
        HicMcodeService hicMcodeService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaJepsuService heaJepsuService = null;
        HicJepsuMunjinNightService hicJepsuMunjinNightService = null;
        HicMunjinNightService hicMunjinNightService = null;
        HicResBohum1JepsuService hicResBohum1JepsuService = null;
        HicResultService hicResultService = null;
        BasDoctorService basDoctorService = null;
        HicTitemService hicTitemService = null;
        HicSangdamNewService hicSangdamNewService = null;

        public delegate void SaveEventClosed(string strReturn);
        public event SaveEventClosed rSaveEventClosed;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcType ht = new clsHcType();
        clsHcCombo hc = new clsHcCombo();

        long FnWrtNo;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrJepDate2;
        string FstrPano;    // 원무행정의 등록번호
        string FstrJong;    // 건진종류
        string FstrPtno;
        long FnAge;
        string FstrPtNo;

        long FnWrtno1;      //1차
        long FnWrtno2;      //2차
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrSaveGbn;

        string FstrTFlag;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;

        long FnRow;

        string FstrROWID;
        string FstrYROWID;              //약속판정 ROWID
        string FstrPROWID;
        string FstrGbSPC;               //일반+특수여부(Y/N)
        string FstrPanOk1;
        string FstrPanOk2;
        long FnPan2Row;
        string FstrOK;
        string FstrXrayRead;
        string FstrWrtnoDisplay;
        string FstrOldSpcPan;           //특수판정코드 변경 여부

        string FstrJepDate;             //2차 접수일자
        string FstrSpcTable;            //취급물질테이블(P.HIC_SPC_PANJENG H:HIC_SPC_PANHIS)

        string FnA118;                  //'하지기능(0.정상 1.정상B 2,의심R)
        string FnA119;                  //'보행장애(1.무 2.유 3.검사불가)
        string FnA120;                  //'평형성의심(0.정상 1.정상B 2.의심R)

        List<string> FstrNotAddPanList = new List<string>();       //2015-09-01일부 추가판정 제외 그룹코드 목록
        bool FbAutoPanjeng;             //자동판정 저장 여부(True/False)
        string[] FstrHabit = new string[19];

        List<string> FstrKind = new List<string>();

        List<HC_PANJENG_PATLIST> FPatListItem;

        string FstrGbHea;   //검진당일 종검수검 여부

        public frmPanjeng_LifeStyle()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmPanjeng_LifeStyle(long nWrtNo, frmPanjeng_Hic frmHcPanjengHic = null)
        {
            InitializeComponent();

            FnWrtNo = nWrtNo;
            frmHcPanHic = frmHcPanjengHic;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicSpcPanjengService = new HicSpcPanjengService();
            hicResSpecialService = new HicResSpecialService();
            hicResBohum1Service = new HicResBohum1Service();
            hicJepsuService = new HicJepsuService();
            hicSpcPanhisService = new HicSpcPanhisService();
            hicCodeService = new HicCodeService();
            hicOrgancodeService = new HicOrgancodeService();
            hicBcodeService = new HicBcodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            comHpcLibBService = new ComHpcLibBService();
            hicMcodeService = new HicMcodeService();
            hicResultExCodeService = new HicResultExCodeService();
            heaJepsuService = new HeaJepsuService();
            hicJepsuMunjinNightService = new HicJepsuMunjinNightService();
            hicMunjinNightService = new HicMunjinNightService();
            hicResBohum1JepsuService = new HicResBohum1JepsuService();
            hicResultService = new HicResultService();
            basDoctorService = new BasDoctorService();
            hicTitemService = new HicTitemService();
            hicSangdamNewService = new HicSangdamNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.tabSmoke.Click += new EventHandler(eTabClick);
            this.tabDrink.Click += new EventHandler(eTabClick);
            this.tabExercise.Click += new EventHandler(eTabClick);
            this.tabDiet.Click += new EventHandler(eTabClick);
            this.tabBiman.Click += new EventHandler(eTabClick);
        }

        void SetControl()
        {
            Control[] ctrls = ComFunc.GetAllControlsUsingRecursive(this);
            List<CheckBox> lstChk = new List<CheckBox>();

            foreach (Control ctl in ctrls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;
                    lstChk.Add(chk);
                }
            }

            for (int i = 0; i < lstChk.Count; i++)
            {
                lstChk[i].CheckedChanged += eChkBoxChanged;
            }
        }

        private void eChkBoxChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                ((CheckBox)sender).Font = new System.Drawing.Font("맑은 고딕", 10, System.Drawing.FontStyle.Bold);
                ((CheckBox)sender).BackColor = System.Drawing.Color.Orange;
            }
            else
            {
                ((CheckBox)sender).Font = new System.Drawing.Font("맑은 고딕", 10, System.Drawing.FontStyle.Regular);
                ((CheckBox)sender).BackColor = System.Drawing.Color.Transparent;
            }
        }

        bool fn_First_Panjeng_Check()
        {
            bool rtnVal = false;

            if (hicResBohum1Service.GetPanjengDrNobyWrtNo(FnWrtNo) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_Combo_Add();
            fn_Screen_Clear();

            HIC_JEPSU item = hicJepsuService.GetSexJepDatebyWrtNo(FnWrtNo);

            FstrSex = item.SEX;

            if (fn_First_Panjeng_Check() == false)
            {
                fn_Auto_Life_panjeng();
            }

            if(clsHcVariable.GnHicLicense > 0)
            {
                fn_LifeHabitReport_Read(FnWrtNo);
            }
            //fn_Auto_LifeHabit_Update(); //문진표를 기준으로 생활습관 자동 업데이트

            fn_LifeTabEnble();
            fn_Screen_Display();
        }

        void fn_LifeTabEnble()
        {
            string strTab = "";

            tabSmoke.Enabled = false;
            tabDrink.Enabled = false;
            tabExercise.Enabled = false;
            tabDiet.Enabled = false;
            tabBiman.Enabled = false;

            if (clsHcVariable.GstrPanLifeTab0 == "Y")
            {
                tabSmoke.Enabled = true;
                strTab = "0";
            }
            if (clsHcVariable.GstrPanLifeTab1 == "Y")
            {
                tabDrink.Enabled = true;
                strTab += "1";
            }
            if (clsHcVariable.GstrPanLifeTab2 == "Y")
            {
                tabExercise.Enabled = true;
                strTab += "2";
            }
            if (clsHcVariable.GstrPanLifeTab3 == "Y")
            {
                tabDiet.Enabled = true;
                strTab += "3";
            }
            if (clsHcVariable.GstrPanLifeTab4 == "Y")
            {
                tabBiman.Enabled = true;
                strTab += "4";
            }

            if (!strTab.IsNullOrEmpty())
            {
                strTab = VB.Left(strTab, 1);
            }

            switch (strTab)
            {
                case "0":
                    tabLife.SelectedTab = tabSmoke;
                    eTabClick(tabSmoke, new EventArgs());
                    break;
                case "1":
                    tabLife.SelectedTab = tabDrink;
                    eTabClick(tabDrink, new EventArgs());
                    break;
                case "2":
                    tabLife.SelectedTab = tabExercise;
                    eTabClick(tabExercise, new EventArgs());
                    break;
                case "3":
                    tabLife.SelectedTab = tabDiet;
                    eTabClick(tabDiet, new EventArgs());
                    break;
                case "4":
                    tabLife.SelectedTab = tabBiman;
                    eTabClick(tabBiman, new EventArgs());
                    break;
                default:
                    break;
            }
        }

        void eTabClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 생활습관도구표 Read
        /// </summary>
        /// <param name="argWrtNo"></param>
        void fn_LifeHabitReport_Read(long argWrtNo)
        {
            int nREAD = 0;
            string strDAT = "";
            long[] nJumsu = new long[20];
            long nWRTNO = 0;
            double nBiman1 = 0;
            long nBiman2 = 0;
            string strResult = "";
            string strBiman = "";
            string strPHQ = "";
            string strOK = "";

            int result = 0;

            nWRTNO = argWrtNo;
            FstrHabit[11] = "N";
            FstrHabit[12] = "N";
            FstrHabit[13] = "N";
            FstrHabit[14] = "N";
            FstrHabit[15] = "N";
            FstrHabit[16] = "N";
            FstrHabit[17] = "N";
            FstrHabit[18] = "N";

            //접수내역을 보고 항목을 세팅
            List<HIC_RESULT> list = hicResultService.GetOnlyExCodebyWrtNo(nWRTNO);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i].EXCODE.Trim())
                    {
                        case "A143":
                            FstrHabit[11] = "Y";   //흡연
                            break;
                        case "A144":
                            FstrHabit[12] = "Y";   //음주
                            break;
                        case "A145":
                            FstrHabit[13] = "Y";   //운동
                            break;
                        case "A146":
                            FstrHabit[14] = "Y";   //영양
                            break;
                        case "A147":
                            FstrHabit[15] = "Y";   //비만
                            break;
                        case "A127":
                            FstrHabit[16] = "Y";   //우울증
                            break;
                        case "A128":
                            FstrHabit[16] = "Y";   //우울증
                            break;
                        case "A130":
                            FstrHabit[16] = "Y";   //우울증
                            break;
                        case "A129":
                            FstrHabit[18] = "Y";   //인지
                            break;
                        case "A118":
                            FstrHabit[18] = "Y";   //노인신체기능
                            break;
                        case "A119":
                            FstrHabit[18] = "Y";   //노인신체기능
                            break;
                        case "A120":
                            FstrHabit[11] = "Y";   //노인신체기능
                            break;
                        default:
                            break;
                    }
                }
            }

            if (FstrHabit[11] == "N" && FstrHabit[12] == "N" && FstrHabit[13] == "N" && FstrHabit[14] == "N" && 
                FstrHabit[15] == "N" && FstrHabit[16] == "N" && FstrHabit[17] == "N" && FstrHabit[18] == "N")
            {
                FstrTFlag = "";
                return;
            }

            FstrTFlag = "Y";

            //점수표 계산 및 DB업데이트
            for (int i = 10; i <= 16; i++)
            {
                nJumsu[i] = 0;
                if (i >= 15)
                {
                    i += 2;
                }

                HIC_TITEM list2 = hicTitemService.GetJumsubyGubunWrtNo(i + 1, nWRTNO);

                if (!list2.IsNullOrEmpty())
                {
                    nJumsu[i] = list2.TOTAL;
                }

                if (i == 17)
                {
                    i -= 2;
                }
                
            }

            //우울증평가
            if (hicTitemService.GetCountbyWrtNo(nWRTNO) > 0)
            {
                strPHQ = "1";
            }

            //검사결과를 읽어 공란이면 점수 UPDATE .
            List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyOnlyWrtNo(nWRTNO);

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < list3.Count; i++)
            {
                strResult = list3[i].RESULT;
                switch (list3[i].EXCODE)
                {
                    case "A143":    //흡연
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode(nJumsu[10].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    case "A144":    //음주
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode(nJumsu[11].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    case "A145":    //운동
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode("0", nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    case "A146":    //영양
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode(nJumsu[13].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    case "A130":    //우울증
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode(nJumsu[17].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    case "A129":    //인지기능
                        result = hicResultService.UpdateResultEntSabunbyWrtNoExCode(nJumsu[18].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list3[i].EXCODE);
                        break;
                    default:
                        break;
                }

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("검사결과 저장중 오류발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            nJumsu[12] = hm.Read_Auto_WORK(nWRTNO);

            nBiman1 = 0;
            nBiman2 = 0;

            List<string> sExCode = new List<string>();

            sExCode.Clear();
            sExCode.Add("A115");
            sExCode.Add("A117");

            List<HIC_RESULT> list4 = hicResultService.GetResultbyWrtNoExCodeList(nWRTNO, sExCode);

            nBiman1 = list4[0].RESULT.To<double>();
            nBiman2 = list4[1].RESULT.To<long>();

            ////비만도체크
            //if (nBiman1 < 18.5)
            //{
            //    strBiman = "저체중";
            //}
            //else if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
            //{
            //    strBiman = "정상체중";
            //}
            //else if (nBiman1 >= 25 && nBiman1 <= 29.9)
            //{
            //    strBiman = "비만";
            //}
            //else if (nBiman1 >= 30)
            //{
            //    strBiman = "고도비만";
            //}

            //비만도체크(2021-05-04 비만기준 변경)
            if (nBiman1 <= 24.9)
            {
                strBiman = "정상체중";
            }
            else if (nBiman1 >= 25 && nBiman1 <= 29.9)
            {
                strBiman = "과체중";
            }
            else if (nBiman1 >= 30)
            {
                strBiman = "비만";
            }

            //청구관련 비만처방 검사코드에 결과입력
            clsDB.setBeginTran(clsDB.DbCon);

            result = hicResultService.UpdateBimanbyWrtNoExCode(strBiman, clsType.User.IdNumber, nWRTNO, "A147");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("비만처방 검사코드에 결과입력시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Auto_LifeHabit_Update()
        {
            string strHabit1 = "";
            string strHabit2 = "";
            string strHabit3 = "";
            string strHabit4 = "";
            string strTemp = "";
            int nTime1 = 0;
            int nTime2 = 0;
            int nDrink1 = 0;
            int nDrink2 = 0;
            string strAge = "";
            int result = 0;

            //건강검진 문진표 및 결과를  READ
            HIC_RES_BOHUM1_JEPSU list = hicResBohum1JepsuService.GetItembyWrtNo(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWrtNo + " 는 문진표 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strHabit1 = "0";
            strHabit2 = "0";
            strHabit3 = "0";
            strHabit4 = "0";

            strAge = list.AGE;
            //절주
            //1주일 음주량
            if (list.TMUN0003 != "4")
            {
                nDrink1 = 0;
                strTemp = list.TMUN0005;
                nDrink1 = fn_Drink_Set(strTemp);
                strTemp = list.TMUN0006;
                nDrink1 += fn_Drink_Set(strTemp);
                strTemp = list.TMUN0007;
                nDrink1 += fn_Drink_Set(strTemp);

                nDrink1 *= list.TMUN0004.To<int>();
                switch (list.TMUN0003)
                {
                    case "1":
                        nDrink2 = nDrink1 * 1;      //일주일
                        break;
                    case "2":
                        nDrink2 = nDrink1 / 4;     //한달
                        break;
                    case "3":
                        nDrink2 = nDrink1 / 48;     //1년
                        break;
                    default:
                        break;
                }

                if (FstrSex == "M" && string.Compare(strAge, "65") < 0)
                {
                    if (nDrink2 > 14)
                    {
                        strHabit1 = "1";    //절주
                    }
                }
                else if (FstrSex == "M" && string.Compare(strAge, "65") >= 0)
                {
                    if (nDrink2 > 7)
                    {
                        strHabit1 = "1";    //절주
                    }
                }
                else if (FstrSex == "F" && string.Compare(strAge, "65") < 0)
                {
                    if (nDrink2 > 7)
                    {
                        strHabit1 = "1";    //절주
                    }
                }
                else if (FstrSex == "F" && string.Compare(strAge, "65") >= 0)
                {
                    if (nDrink2 > 3)
                    {
                        strHabit1 = "1";    //절주
                    }
                }

                //최대음주량
                nDrink1 = 0;
                strTemp = list.TMUN0008;
                nDrink1 = fn_Drink_Set(strTemp);
                strTemp = list.TMUN0009;
                nDrink1 = fn_Drink_Set(strTemp);
                strTemp = list.TMUN0010;
                nDrink1 = fn_Drink_Set(strTemp);

                if (FstrSex == "M")
                {
                    if (nDrink1 > 4)
                    {
                        strHabit1 = "1";    //절주
                    }
                }
                else
                {
                    if (nDrink1 > 3)
                    {
                        strHabit1 = "1";    //절주
                    }
                }
            }

            //금연
            if (list.T_SMOKE1 == "1")
            {
                strHabit2 = "1";
            }
            //신체활동
            nTime1 = 0;
            nTime2 = 0;
            if (!list.TMUN0011.IsNullOrEmpty())
            {
                nTime1 = VB.Pstr(list.TMUN0011, ";", 1).To<int>() * 60;
                nTime1 += VB.Pstr(list.TMUN0011, ";", 2).To<int>();
                nTime1 += list.T_ACTIVE1.To<int>();
            }
            if (!list.TMUN0012.IsNullOrEmpty())
            {
                nTime1 = VB.Pstr(list.TMUN0012, ";", 1).To<int>() * 60;
                nTime1 += VB.Pstr(list.TMUN0012, ";", 2).To<int>();
                nTime1 += list.T_ACTIVE2.To<int>();
            }
            if ((nTime1 * 2) + nTime2 < 150)
            {
                strHabit3 = "1";
            }
            //근력운동
            if (list.T_ACTIVE3.To<int>() < 2)
            {
                strHabit4 = "1";
            }

            //상담에 결과를 저장함
            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.HABIT1 = strHabit1;
            item.HABIT2 = strHabit2;
            item.HABIT3 = strHabit3;
            item.HABIT4 = strHabit4;
            item.GBCHK = "Y";
            item.WRTNO = FnWrtNo;

            result = hicSangdamNewService.UpdateHabitGbChkbyWrtNo(item);

            if (result < 0)
            {
                MessageBox.Show("상담결과 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //판정에 결과를 저장함
            HIC_RES_BOHUM1 item1 = new HIC_RES_BOHUM1();

            item1.HABIT1 = strHabit1;
            item1.HABIT2 = strHabit2;
            item1.HABIT3 = strHabit3;
            item1.HABIT4 = strHabit4;
            item1.WRTNO = FnWrtNo;

            result = hicResBohum1Service.UpdateOnlyuHabitbyWrtNo(item1);

            if (result < 0)
            {
                MessageBox.Show("판정결과 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        int fn_Drink_Set(string strTemp)
        {
            int rtnVal = 0;
            int nDrink1 = 0;

            if (!strTemp.IsNullOrEmpty())
            {
                //소주
                if (VB.Pstr(strTemp, ";", 1) == "1")
                {
                    switch (VB.Pstr(strTemp, ";", 3))
                    {
                        case "잔":
                            nDrink1 = VB.Pstr(strTemp, ";", 2).To<int>() * (4 / 7);
                            break;
                        case "병":
                            nDrink1 = VB.Pstr(strTemp, ";", 2).To<int>() * 4;
                            break;
                        case "CC":
                            nDrink1 = VB.Pstr(strTemp, ";", 2).To<int>() / 90;
                            break;
                        default:
                            break;
                    }
                }
                //맥주
                else if (VB.Pstr(strTemp, ";", 1) == "2")
                {
                    switch (VB.Pstr(strTemp, ";", 3))
                    {
                        case "잔":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() * (200 / 350);
                            break;
                        case "병":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() * (500 / 350);
                            break;
                        case "CC":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() / 350;
                            break;
                        case "캔":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>();
                            break;
                        default:
                            break;
                    }
                }
                //양주
                else if (VB.Pstr(strTemp, ";", 1) == "3")
                {
                    switch (VB.Pstr(strTemp, ";", 3))
                    {
                        case "잔":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>();
                            break;
                        case "병":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() * (500 / 45);
                            break;
                        case "CC":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() / 45;
                            break;
                        default:
                            break;
                    }
                }
                //막걸리
                else if (VB.Pstr(strTemp, ";", 1) == "4")
                {
                    switch (VB.Pstr(strTemp, ";", 3))
                    {
                        case "잔":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>();
                            break;
                        case "병":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() * (750 / 300);
                            break;
                        case "CC":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() / 300;
                            break;
                        default:
                            break;
                    }
                }
                //와인
                else if (VB.Pstr(strTemp, ";", 1) == "5")
                {
                    switch (VB.Pstr(strTemp, ";", 3))
                    {
                        case "잔":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>();
                            break;
                        case "병":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() * (750 / 150);
                            break;
                        case "CC":
                            nDrink1 += VB.Pstr(strTemp, ";", 2).To<int>() / 150;
                            break;
                        default:
                            break;
                    }
                }
                rtnVal = nDrink1;
            }
            return rtnVal;
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            string strDAT = "";
            string strTSmoke1 = "";
            string strTSmoke2 = "";
            string strDrink = "";
            long nTOT = 0;
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            long[] nJemsu = new long[6];
            double nBiman1 = 0;
            double nBiman2 = 0;
            long nIlsu1 = 0;
            long nTime11 = 0;
            long nTime12 = 0;
            long nIlsu2 = 0;
            long nTime21 = 0;
            long nTime22 = 0;
            long nTime1 = 0;
            long nTime2 = 0;

            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp = "";
            double nDrink1 = 0;
            double nDrink2 = 0;
            string str음주 = "";
            string str음주1 = "";
            string str음주2 = "";

            //2019년기준
            string str보통음주 = "";
            string str보통음주1 = "";
            string str최대음주 = "";
            string str최대음주1 = "";

            strSLIP1 = "";
            strSLIP2 = "";
            strSLIP3 = "";
            strSLIP4 = "";
            strSLIP5 = "";
            strTSmoke1 = "";

            List<string> strCodeList = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                nJemsu[i] = 0;
            }
            
            //생활습관,신체활동 등
            HIC_RES_BOHUM1_JEPSU list = hicResBohum1JepsuService.GetItembyWrtNo(FnWrtNo);

            str음주 = "";
            str보통음주 = ""; str최대음주 = "";
            str보통음주1 = ""; str최대음주1 = "";

            //보통음주
            nDrink1 = 0;
            nDrink2 = 0;

            if (!list.IsNullOrEmpty())
            {
                if (list.TMUN0003 == "4")
                {
                    str음주 = "0";    //비음주자
                }
                else
                {
                    //보통음주
                    strTemp = list.TMUN0005;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    strTemp = list.TMUN0006;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    strTemp = list.TMUN0007;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    nDrink1 += list.TMUN0004.To<double>();

                    switch (list.TMUN0003)
                    {
                        case "1":
                            str보통음주 = (nDrink1 * 1).To<string>();  //일주일
                            break;
                        case "2":
                            str보통음주 = (nDrink1 / 4).To<string>();  //한달
                            break;
                        case "3":
                            str보통음주 = (nDrink1 / 48).To<string>();  //1년
                            break;
                        default:
                            break;
                    }

                    nDrink1 = 0;
                    //최대음주
                    strTemp = list.TMUN0008;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    strTemp = list.TMUN0009;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    strTemp = list.TMUN0010;
                    if (!strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (4 / 7);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 4;
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 90;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //맥주
                        else if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (200 / 350);
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 350);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "캔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 350;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //양주
                        else if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (500 / 45);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 45;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //막걸리
                        else if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 300);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 300;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //와인
                        else if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * 1;
                                    break;
                                case "병":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() * (750 / 150);
                                    break;
                                case "CC":
                                    nDrink1 += VB.Pstr(strTemp, ";", 2).To<double>() / 150;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    nDrink1 = nDrink1 * list.TMUN0004.To<double>();
                    switch (list.TMUN0003)
                    {
                        case "1":
                            str최대음주 = (nDrink1 * 1).To<string>();  //일주일
                            break;
                        case "2":
                            str최대음주 = (nDrink1 / 4).To<string>();  //한달
                            break;
                        case "3":
                            str최대음주 = (nDrink1 / 48).To<string>();  //1년
                            break;
                        default:
                            break;
                    }

                    str최대음주1 = nDrink1.To<string>();
                    str보통음주1 = (VB.Fix(str보통음주.To<int>() / 7)).To<string>();

                    if (FstrSex == "M" && FnAge < 65)
                    {
                        if (string.Compare(str보통음주, "14") > 0)
                        {
                            str음주1 = "OK";                      //절주
                        }
                    }
                    else if (FstrSex == "M" && FnAge >= 65)
                    {
                        if (string.Compare(str보통음주, "7") > 0)
                        {
                            str음주1 = "OK";                      //절주
                        }
                    }
                    else if (FstrSex == "F" && FnAge < 65)
                    {
                        if (string.Compare(str보통음주, "7") > 0)
                        {
                            str음주1 = "OK";                      //절주
                        }
                    }
                    else if (FstrSex == "F" && FnAge >= 65)
                    {
                        if (string.Compare(str보통음주, "3") > 0)
                        {
                            str음주1 = "OK";                      //절주
                        }
                    }

                    if (FstrSex == "M")
                    {
                        if (string.Compare(str최대음주1, "4") > 4)
                        {
                            str음주2 = "OK"; //절주
                        }
                    }
                    else if (FstrSex == "F")
                    {
                        if (string.Compare(str최대음주1, "3") > 4)
                        {
                            str음주2 = "OK"; //절주
                        }
                    }
                }
            }

            //생활습관처방전
            HIC_RES_BOHUM1 list2 = hicResBohum1Service.GetItemByWrtno(FnWrtNo);

            if (!list2.IsNullOrEmpty())
            {
                strSLIP1 = list2.SLIP_SMOKE;
                strSLIP2 = list2.SLIP_DRINK;
                strSLIP3 = list2.SLIP_ACTIVE;
                strSLIP4 = list2.SLIP_FOOD;
                strSLIP5 = list2.SLIP_BIMAN;
                strTSmoke1 = list2.T_SMOKE1;
                strTSmoke2 = list2.TMUN0097;
                strDrink = list2.TMUN0003;
            }

            //생활습관평가도구표 결과값을 읽음

            strCodeList.Clear();
            strCodeList.Add("A143");
            strCodeList.Add("A144");
            strCodeList.Add("A145");
            strCodeList.Add("A146");
            strCodeList.Add("A147");
            strCodeList.Add("A130");
            strCodeList.Add("A129");

            List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strCodeList);

            nREAD = list3.Count;
            for (int i = 0; i < nREAD; i++)
            {
                switch (list3[i].EXCODE.Trim())
                {
                    case "A143":
                        nJemsu[0] = list3[i].RESULT.To<long>();    //흡연
                        break;
                    case "A144":
                        nJemsu[1] = list3[i].RESULT.To<long>();    //음주
                        break;
                    case "A145":
                        nJemsu[2] = list3[i].RESULT.To<long>();    //운동
                        break;
                    case "A146":
                        nJemsu[3] = list3[i].RESULT.To<long>();    //영양
                        break;
                    case "A147":
                        nJemsu[4] = list3[i].RESULT.To<long>();    //비만
                        break;
                    default:
                        break;
                }
            }

            //금연처방전
            if (tabSmoke.Visible == true)
            {
                txtSmoke0.Text = nJemsu[0].To<string>();
                if (strSLIP1.IsNullOrEmpty())
                {
                    //흡연상태
                    cboSmoke1.SelectedIndex = 0;

                    if (strTSmoke1.IsNullOrEmpty() && strTSmoke2.IsNullOrEmpty())
                    {
                        cboSmoke1.SelectedIndex = 1;
                        cboSmoke2.SelectedIndex = 0;
                    }
                    else if (strTSmoke1 == "1")
                    {
                        cboSmoke1.SelectedIndex = 3;
                    }
                    else if (strTSmoke1 == "2")
                    {
                        cboSmoke1.SelectedIndex = 2;
                        cboSmoke2.SelectedIndex = 1;
                    }
                    else if (strTSmoke1 == "1")
                    {
                        cboSmoke1.SelectedIndex = 4;
                    }

                    //니코틴의존도
                    if (txtSmoke0.Text.To<int>() <= 3)
                    {
                        cboSmoke2.SelectedIndex = 1;
                    }
                    else if (txtSmoke0.Text.To<int>() >= 4 && txtSmoke0.Text.To<int>() <= 6)
                    {
                        cboSmoke2.SelectedIndex = 2;
                    }
                    else if (txtSmoke0.Text.To<int>() >= 7)
                    {
                        cboSmoke2.SelectedIndex = 3;
                    }
                }
                else
                {
                    //흡연상태
                    strDAT = VB.Pstr(strSLIP1, ";", 1);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke1.Items.Count; i++)
                        {
                            cboSmoke1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke1.Text, ".", 1))
                            {
                                cboSmoke1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //니코틴의존도
                    strDAT = VB.Pstr(strSLIP1, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke2.Items.Count; i++)
                        {
                            cboSmoke2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke2.Text, ".", 1))
                            {
                                cboSmoke2.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금연계획단계
                    strDAT = VB.Pstr(strSLIP1, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke3.Items.Count; i++)
                        {
                            cboSmoke3.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke3.Text, ".", 1))
                            {
                                cboSmoke3.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금연처방
                    for (int i = 4; i <= 9; i++)
                    {
                        CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i - 4).To<string>(), true)[0] as CheckBox);
                        chkSmoking4.Checked = VB.Pstr(strSLIP1, ";", i) == "1" ? true : false;
                    }

                    //금연처방-기타
                    txtSmoke4.Text = VB.Pstr(strSLIP1, ";", 10);
                    //금단증상
                    for (int i = 11; i <= 16; i++)
                    {
                        CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + (i - 11).To<string>(), true)[0] as CheckBox);
                        chkSmoking5.Checked = VB.Pstr(strSLIP1, ";", i) == "1" ? true : false;
                    }
                    txtSmoke6.Text = VB.Pstr(strSLIP1, ";", 17);
                    txtSmoke7.Text = VB.Pstr(strSLIP1, ";", 18);
                }
            }

            //음주처방전
            if (tabDrink.Visible == true)
            {
                txtDrink1.Text = nJemsu[1].To<string>();
                if (strSLIP2.IsNullOrEmpty())
                {
                    //음주
                    if (txtDrink1.Text.To<int>() >= 16)
                    {
                        cboDrink1.SelectedIndex = 4;
                    }
                    else
                    {
                        cboDrink1.SelectedIndex = 3;
                    }
                }
                else
                {
                    //음주관련 질환
                    for (int i = 2; i <= 9; i++)
                    {
                        CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i - 2).To<string>(), true)[0] as CheckBox);
                        chkDrink0.Checked = VB.Pstr(strSLIP2, ";", i) == "1" ? true : false;
                    }

                    //음주상태
                    strDAT = VB.Pstr(strSLIP2, ";", 10);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboDrink1.Items.Count; i++)
                        {
                            cboDrink1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboDrink1.Text, ".", 1))
                            {
                                cboDrink1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금주처방
                    for (int i = 11; i <= 16; i++)
                    {
                        CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i - 11).To<string>(), true)[0] as CheckBox);
                        chkDrink2.Checked = VB.Pstr(strSLIP2, ";", i) == "1" ? true : false;
                    }
                    txtDrink3.Text = VB.Pstr(strSLIP2, ";", 17);
                    txtDrink4.Text = VB.Pstr(strSLIP2, ";", 18);
                }
            }

            //운동처방전
            if (tabExercise.Visible == true)
            {
                HIC_RES_BOHUM1 list4 = hicResBohum1Service.GetItemByWrtno(FnWrtNo);

                //신체활동
                if (!list4.IsNullOrEmpty())
                {
                    nTime1 = 0;
                    nTime2 = 0;
                    if (!list4.TMUN0011.IsNullOrEmpty())
                    {
                        nTime1 = VB.Pstr(list4.TMUN0011, ":", 1).To<long>() * 60;
                        nTime1 += VB.Pstr(list4.TMUN0011, ":", 2).To<long>();
                        nTime1 *= list4.T_ACTIVE1.To<long>() * 2;
                    }
                    if (!list4.TMUN0012.IsNullOrEmpty())
                    {
                        nTime2 = VB.Pstr(list4.TMUN0012, ":", 1).To<long>() * 60;
                        nTime2 += VB.Pstr(list4.TMUN0012, ":", 2).To<long>();
                        nTime2 *= list4.T_ACTIVE2.To<long>();
                    }
                    txtHealth0.Text = (nTime1 + nTime2).To<string>();

                    //운동상태
                    if (txtHealth0.Text.To<int>() < 150)
                    {
                        cboHealth1.SelectedIndex = 1;
                    }
                    else if (txtHealth0.Text.To<int>() >= 150 && txtHealth0.Text.To<int>() < 300)
                    {
                        cboHealth1.SelectedIndex = 2;
                    }
                    else if (txtHealth0.Text.To<int>() >= 300)
                    {
                        cboHealth1.SelectedIndex = 3;
                    }

                    //근력운동
                    cboHealth4.SelectedIndex = 2;
                    if (list4.T_ACTIVE3.To<int>() < 2)
                    {
                        cboHealth4.SelectedIndex = 1;
                        chkHealth9.Checked = true;
                    }
                }

                if (!strSLIP3.IsNullOrEmpty())
                {
                    //운동종류
                    for (int i = 2; i <= 11; i++)
                    {
                        CheckBox chkHealth = (Controls.Find("chkHealth" + (i - 2).To<string>(), true)[0] as CheckBox);
                        chkHealth.Checked = VB.Pstr(strSLIP3, ";", i) == "1" ? true : false;
                    }
                    if (cboHealth4.SelectedIndex == 1)
                    {
                        chkHealth9.Checked = true;
                    }
                    txtHealth_Etc1.Text = VB.Pstr(strSLIP3, ";", 12);
                    //운동시간
                    strDAT = VB.Pstr(strSLIP3, ";", 13);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboHealth2.Items.Count; i++)
                        {
                            cboHealth2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboHealth2.Text, ".", 1))
                            {
                                cboHealth2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    txtHealth_Etc2.Text = VB.Pstr(strSLIP3, ";", 14);
                    //운동횟수
                    strDAT = VB.Pstr(strSLIP3, ";", 15);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboHealth3.Items.Count; i++)
                        {
                            cboHealth3.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboHealth3.Text, ".", 1))
                            {
                                cboHealth3.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //호전가능
                    for (int i = 16; i <= 26; i++)
                    {
                        CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (i - 16).To<string>(), true)[0] as CheckBox);
                        chkHealth2.Checked = VB.Pstr(strSLIP3, ";", i) == "1" ? true : false;
                    }
                    txtHealth_Etc3.Text = VB.Pstr(strSLIP3, ";", 27);
                    txtHealth_Etc4.Text = VB.Pstr(strSLIP3, ";", 28);
                }
            }

            //영양처방전
            if (tabDiet.Visible == true)
            {
                txtDiet0.Text = string.Format("{0:0}", nJemsu[3]);
                if (strSLIP4.IsNullOrEmpty())
                {
                    //영양
                    if (txtDiet0.Text.To<int>() <= 28)
                    {
                        cboDiet.SelectedIndex = 3;
                    }
                    else if (txtDiet0.Text.To<int>() >= 28 && txtDiet0.Text.To<int>() <= 38)
                    {
                        cboDiet.SelectedIndex = 2;
                    }
                    else if (txtDiet0.Text.To<int>() >= 39)
                    {
                        cboDiet.SelectedIndex = 1;
                    }
                }
                else
                {
                    //식생활습관
                    strDAT = VB.Pstr(strSLIP4, ";", 1);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboDiet.Items.Count; i++)
                        {
                            cboDiet.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboDiet.Text, ".", 1))
                            {
                                cboDiet.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //개선처방
                    for (int i = 2; i <= 11; i++)
                    {
                        CheckBox chkDiet = (Controls.Find("chkDiet" + (i - 2).To<string>(), true)[0] as CheckBox);
                        chkDiet.Checked = VB.Pstr(strSLIP4, ";", i) == "1" ? true : false;
                    }
                    //호전가능
                    for (int i = 12; i <= 20; i++)
                    {
                        CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (i - 12).To<string>(), true)[0] as CheckBox);
                        chkDiet2.Checked = VB.Pstr(strSLIP4, ";", i) == "1" ? true : false;
                    }
                    txtDiet_ETC1.Text = VB.Pstr(strSLIP4, ";", 21);
                    txtDiet_ETC2.Text = VB.Pstr(strSLIP4, ";", 22);
                }
            }

            //비만처방전
            if (tabBiman.Visible == true)
            {
                txtBiman0.Text = string.Format("{0:0}", nJemsu[4]);
                if (strSLIP5.IsNullOrEmpty())
                {
                    nBiman1 = 0;
                    nBiman2 = 0;

                    List<string> strExCode = new List<string>();

                    strExCode.Clear();
                    strExCode.Add("A115");
                    strExCode.Add("A117");

                    List<HIC_RESULT> list5 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCode);

                    if (list5.Count > 0)
                    {
                        for (int i = 0; i < list5.Count; i++)
                        {
                            if (list5[i].EXCODE == "A117")
                            {
                                nBiman1 = list5[i].RESULT.To<double>();
                            }
                            if (list5[i].EXCODE == "A115")
                            {
                                nBiman2 = Math.Round(list5[i].RESULT.To<double>(0), 0);
                            }
                        }
                    }

                    //비만도체크
                    if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
                    {
                        cboBiman0.SelectedIndex = 1;
                    }
                    else if (nBiman1 >= 25 && nBiman1 <= 29.9)
                    {
                        cboBiman0.SelectedIndex = 2;
                    }
                    else if (nBiman1 >= 30)
                    {
                        cboBiman0.SelectedIndex = 3;
                    }

                    //복부비만
                    switch (FstrSex)
                    {
                        case "M":
                            if (nBiman2 >= 90)
                            {
                                cboBiman1.SelectedIndex = 1;
                            }
                            else if (nBiman2 < 90)
                            {
                                cboBiman1.SelectedIndex = 2;
                            }
                            break;
                        case "F":
                            if (nBiman2 >= 85)
                            {
                                cboBiman1.SelectedIndex = 1;
                            }
                            else if (nBiman2 < 85)
                            {
                                cboBiman1.SelectedIndex = 2;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //체중
                    strDAT = VB.Pstr(strSLIP5, ";", 1);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman0.Items.Count; i++)
                        {
                            cboBiman0.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman0.Text, ".", 1))
                            {
                                cboBiman0.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //비만
                    strDAT = VB.Pstr(strSLIP5, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman1.Items.Count; i++)
                        {
                            cboBiman1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman1.Text, ".", 1))
                            {
                                cboBiman1.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //질환발생위험
                    strDAT = VB.Pstr(strSLIP5, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman2.Items.Count; i++)
                        {
                            cboBiman2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman2.Text, ".", 1))
                            {
                                cboBiman2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    txtBiman00.Text = VB.Pstr(strSLIP5, ";", 4);
                    txtBiman01.Text = VB.Pstr(strSLIP5, ";", 5);
                    txtBiman02.Text = VB.Pstr(strSLIP5, ";", 6);
                    txtBiman03.Text = VB.Pstr(strSLIP5, ";", 7);
                    //비만처방
                    for (int i = 8; i <= 14; i++)
                    {
                        CheckBox chkBiman = (Controls.Find("chkBiman" + (i - 8).To<string>(), true)[0] as CheckBox);
                        chkBiman.Checked = VB.Pstr(strSLIP5, ";", i) == "1" ? true : false;
                    }

                    txtBiman20.Text = VB.Pstr(strSLIP5, ";", 15);
                    txtBiman21.Text = VB.Pstr(strSLIP5, ";", 16);
                    //호잔가능
                    for (int i = 17; i < 26; i++)
                    {
                        CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i - 17).To<string>(), true)[0] as CheckBox);
                        chkBiman2.Checked = VB.Pstr(strSLIP5, ";", i) == "1" ? true : false;
                    }
                    txtBiman3.Text = VB.Pstr(strSLIP5, ";", 27);
                    txtBiman4.Text = VB.Pstr(strSLIP5, ";", 28);
                }
            }

       
        }

        void fn_Combo_Add()
        {
            //금연처방전
            cboSmoke1.Items.Clear();
            cboSmoke1.Items.Add(" ");
            cboSmoke1.Items.Add("0.비흡연자");
            cboSmoke1.Items.Add("1.과거 흡연자");
            cboSmoke1.Items.Add("2.현재 흡연자");
            cboSmoke1.Items.Add("3.전자담배 단독 사용자");

            cboSmoke2.Items.Clear();
            cboSmoke2.Items.Add(" ");
            cboSmoke2.Items.Add("1.낮음(0~3점)");
            cboSmoke2.Items.Add("2.중간정도(4~6점)");
            cboSmoke2.Items.Add("3.높은정도(7~10점)");

            cboSmoke3.Items.Clear();
            cboSmoke3.Items.Add(" ");
            cboSmoke3.Items.Add("1.금연 계획 이전단계");
            cboSmoke3.Items.Add("2.금연 계획 단계");
            cboSmoke3.Items.Add("3.금연 준비 단계");
            cboSmoke3.Items.Add("4.금연 시도");
            cboSmoke3.Items.Add("5.금연 유지");

            cboDrink1.Items.Clear();
            cboDrink1.Items.Add(" ");
            cboDrink1.Items.Add("1.비음주자");
            cboDrink1.Items.Add("2.적정 음주자");
            cboDrink1.Items.Add("3.위험 음주자");
            cboDrink1.Items.Add("4.알코올 사용장애 의심");

            cboHealth1.Items.Clear();
            cboHealth1.Items.Add(" ");
            cboHealth1.Items.Add("1.신체활동 부족");
            cboHealth1.Items.Add("2.기본 신체활동");
            cboHealth1.Items.Add("3.건강증진 신체활동");

            cboHealth2.Items.Clear();
            cboHealth2.Items.Add(" ");
            cboHealth2.Items.Add("1.10분");
            cboHealth2.Items.Add("2.15~30분");
            cboHealth2.Items.Add("3.30분 이상");
            cboHealth2.Items.Add("4.기타");

            cboHealth3.Items.Clear();
            cboHealth3.Items.Add(" ");
            cboHealth3.Items.Add("1.1주일에 1~2회");
            cboHealth3.Items.Add("2.1주일에 3~4회");
            cboHealth3.Items.Add("3.1주일에 5회이상");

            cboHealth4.Items.Clear();
            cboHealth4.Items.Add(" ");
            cboHealth4.Items.Add("1.근력운동 부족");
            cboHealth4.Items.Add("2.근력운동 적절");

            //식생활습관
            cboDiet.Items.Clear();
            cboDiet.Items.Add(" ");
            cboDiet.Items.Add("1.양호");
            cboDiet.Items.Add("2.보통");
            cboDiet.Items.Add("3.불량");
            cboDiet.SelectedIndex = 0;

            //체중
            cboBiman0.Items.Clear();
            cboBiman0.Items.Add(" ");
            cboBiman0.Items.Add("1.정상체중");
            cboBiman0.Items.Add("2.과체중");
            cboBiman0.Items.Add("3.비만");
            cboBiman0.SelectedIndex = 0;

            //비만
            cboBiman1.Items.Clear();
            cboBiman1.Items.Add(" ");
            cboBiman1.Items.Add("1.복부비만");
            cboBiman1.Items.Add("2.정상");
            cboBiman1.SelectedIndex = 0;

            //질환발생위험
            cboBiman2.Items.Clear();
            cboBiman2.Items.Add(" ");
            cboBiman2.Items.Add("1.낮음");
            cboBiman2.Items.Add("2.보통");
            cboBiman2.Items.Add("3.다소증가");
            cboBiman2.Items.Add("4.어느정도증가");
            cboBiman2.Items.Add("5.상당히증가");
            cboBiman2.Items.Add("6.매우증가");
            cboBiman2.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save_LifePanjeng();
            }
        }

        public bool Data_Save_LifePanjeng()
        {
            bool rtnVal = false;
            string strChk = "";
            string[] strHabit = new string[5];
            string strFood = "";
            string strDrink = "";
            long nLicense = 0;

            if (tabSmoke.Enabled == true)
            {
                if (VB.Left(cboSmoke1.Text, 1) == "2")  //현재 흡연자
                {
                    if (cboSmoke3.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("생활습관 금연처방전 결과가 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrOK = "N";
                        return rtnVal;
                    }
                    //금연처방
                    strChk = "";
                    for (int i = 0; i <= 5; i++)
                    {
                        CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i).To<string>(), true)[0] as CheckBox);
                        if (chkSmoking4.Checked == true)
                        {
                            strChk = "Y";
                            break;
                        }
                    }
                    if (strChk.IsNullOrEmpty())
                    {
                        MessageBox.Show("금연처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }
                }
            }
            if (tabDrink.Enabled == true)
            {
                if (cboDrink1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("생활습관 음주상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return rtnVal;
                }

                if (VB.Left(cboDrink1.Text, 1) == "3" || VB.Left(cboDrink1.Text, 1) == "4")
                {
                    //금주/절주 처방
                    strChk = "";
                    for (int i = 0; i <= 5; i++)
                    {
                        CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).To<string>(), true)[0] as CheckBox);
                        if (chkDrink2.Checked == true)
                        {
                            strChk = "Y";
                            break;
                        }
                    }
                    if (strChk.IsNullOrEmpty())
                    {
                        MessageBox.Show("금주/절주 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }
                }
            }
            if (tabExercise.Enabled == true)
            {
                if (cboHealth1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("운동상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return rtnVal;
                }

                if (cboHealth4.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("근력운동이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return rtnVal;
                }

                if (VB.Left(cboHealth1.Text, 1) == "1" || VB.Left(cboHealth4.Text, 1) == "1")
                {
                    //운동종류
                    for (int i = 0; i <= 9; i++)
                    {
                        strChk = "";
                        CheckBox chkHealth = (Controls.Find("chkHealth" + (i).To<string>(), true)[0] as CheckBox);
                        if (chkHealth.Checked == true)
                        {
                            strChk = "Y";
                            break;
                        }
                    }
                    if (strChk.IsNullOrEmpty())
                    {
                        MessageBox.Show("운동종류 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }

                    if (cboHealth2.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("운동시간이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }

                    if (cboHealth3.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("운동횟수가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }
                }

                if (tabDiet.Enabled == true)
                {
                    if (cboDiet.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("식생활습관 결과가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal;
                    }
                    if (VB.Left(cboDiet.Text, 1) == "3")
                    {
                        //식생활 개선처방
                        strChk = "";
                        for (int i = 0; i <= 9; i++)
                        {
                            CheckBox chkDiet = (Controls.Find("chkDiet" + (i).To<string>(), true)[0] as CheckBox);
                            if (chkDiet.Checked == true)
                            {
                                strChk = "Y";
                                break;
                            }
                        }
                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("식생활개선 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return rtnVal;
                        }
                    }
                }

                if (tabBiman.Enabled == true)
                {
                    if (VB.Left(cboBiman1.Text, 1) == "3" || VB.Left(cboBiman1.Text, 1) == "1")
                    {
                        strChk = "";
                        for (int i = 0; i <= 6; i++)
                        {
                            CheckBox chkBiman = (Controls.Find("chkBiman" + (i).To<string>(), true)[0] as CheckBox);
                            if (chkBiman.Checked == true)
                            {
                                strChk = "Y";
                                break;
                            }
                        }

                        if (!txtBiman20.Text.IsNullOrEmpty())
                        {
                            strChk = "Y";
                        }
                        if (!txtBiman21.Text.IsNullOrEmpty())
                        {
                            strChk = "Y";
                        }
                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("비만 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return rtnVal;
                        }
                    }

                    if (txtBiman02.Text.To<int>() >= 13)
                    {
                        MessageBox.Show("비만처방 개월수는 12개월 이하여야합니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return rtnVal; 
                    }

                    for (int i = 1; i <= txtBiman01.Text.Length; i++)
                    {
                        if (VB.Mid(txtBiman01.Text, i, 1) == ".")
                        {
                            MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return rtnVal;
                        }
                    }

                    for (int i = 1; i <= txtBiman03.Text.Length; i++)
                    {
                        if (VB.Mid(txtBiman03.Text, i, 1) == ".")
                        {
                            MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return rtnVal;
                        }
                    }
                }
            }

            //의사면허번호 조회
            nLicense = clsHcVariable.GnHicLicense;
            if (clsHcVariable.GbHicAdminSabun == true)
            {
                nLicense = txtPanDrNo.Text.To<long>();
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = clsType.User.UserName;
            }

            //퇴사일 이후 판정금지
            if (!clsHcVariable.GstrReDay.IsNullOrEmpty())
            {
                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();
                date1 = Convert.ToDateTime(dtpPanDate.Text);
                date2 = Convert.ToDateTime(clsHcVariable.GstrReDay);
                if (date1 > date2)
                {
                    MessageBox.Show("판정일자가 " + clsHcVariable.GstrReDay + "일보다 큼" + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return rtnVal;
                }
            }

            hb.READ_HIC_DrSabun(txtPanDrNo.Text);
            //의사당직 휴무 여부확인
            if (hb.READ_DOCTOR_SCH2(clsType.User.DrCode, dtpPanDate.Text) == "NO")
            {
                MessageBox.Show("해당 판정일자는 과장님의 진료가 없거나 휴일입니다." + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrOK = "N";
                return rtnVal;
            }

            strFood = "";
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkDiet = (Controls.Find("chkDiet" + (i).To<string>(), true)[0] as CheckBox);
                if (chkDiet.Checked == true)
                {
                    strFood = "OK";
                }
            }

            if (strFood.IsNullOrEmpty())
            {
                chkDiet8.Checked = true;
            }

            strDrink = "";
            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).To<string>(), true)[0] as CheckBox);
                if (chkDrink2.Checked == true)
                {
                    strDrink = "OK";
                }
            }

            if (strDrink.IsNullOrEmpty())
            {
                chkDrink20.Checked = true;
            }

            //저장
            rtnVal = fn_DB_Update_LifeSlip();

            return rtnVal;

            //rSaveEventClosed("LifeStyle");
        }

        bool fn_DB_Update_LifeSlip()
        {
            bool rtnVal = true;
            int result = 0;
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            string strSLIP6 = "";
            string strSLIP7 = "";

            //금연처방전
            if (tabSmoke.Enabled == true)
            {
                txtSmoke4.Text = txtSmoke4.Text.Replace(";", ",");
                txtSmoke6.Text = txtSmoke6.Text.Replace(";", ",");
                txtSmoke7.Text = txtSmoke7.Text.Replace(";", ",");
                strSLIP1 = VB.Pstr(cboSmoke1.Text, ".", 1) + ";";      //흡연상태(1)
                strSLIP1 += VB.Pstr(cboSmoke2.Text, ".", 1) + ";";     //니코틴의존도(2)
                strSLIP1 += VB.Pstr(cboSmoke3.Text, ".", 1) + ";";     //금연계획단계(3)
                //금연처방(4-9)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking4.Checked == true ? "1" : "0";
                    strSLIP1 += ";";
                }

                strSLIP1 += txtSmoke4.Text + ";"; //금연계획단계(10)
                //금단증상극복(11-16)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking5.Checked == true ? "1" : "0";
                    strSLIP1 += ";";
                }
                strSLIP1 += txtSmoke6.Text + ";"; //추가조치사항(17)
                strSLIP1 += txtSmoke7.Text + ";";
            }

            //음주처방전
            if (tabDrink.Enabled == true)
            {
                txtDrink3.Text = txtDrink3.Text.Replace(";", ",");
                txtDrink4.Text = txtDrink4.Text.Replace(";", ",");
                strSLIP2 = txtDrink1.Text + ";";                  //평가점수(1)
                //영향질환(2-9)
                for (int i = 0; i <= 7; i++)
                {
                    CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink0.Checked == true ? "1" : "0";
                    strSLIP2 += ";";
                }
                strSLIP2 += VB.Pstr(cboDrink1.Text, ".", 1) + ";"; //음주상태(10)
                //금주/절주처방(11-16)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink2.Checked == true ? "1" : "0";
                    strSLIP2 += ";";
                }
                strSLIP2 += txtDrink3.Text + ";"; //추가조치사항(17)
                strSLIP2 += txtDrink4.Text + ";"; //기타질환의심
            }

            //운동처방전
            if (tabExercise.Enabled == true)
            {
                txtHealth_Etc1.Text = txtHealth_Etc1.Text.Replace(";", ",");
                txtHealth_Etc2.Text = txtHealth_Etc2.Text.Replace(";", ",");
                txtHealth_Etc3.Text = txtHealth_Etc3.Text.Replace(";", ",");
                txtHealth_Etc4.Text = txtHealth_Etc4.Text.Replace(";", ",");

                strSLIP3 = VB.Pstr(cboHealth1.Text, ".", 1) + ";";            //운동상태(1)
                //운동종류(2-11)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth.Checked == true ? "1" : "0";
                    strSLIP3 += ";";
                }
                strSLIP3 += txtHealth_Etc1.Text + ";";              //기타(12)
                strSLIP3 += VB.Pstr(cboHealth2.Text, ".", 1) + ";"; //운동시간(13)
                strSLIP3 += txtHealth_Etc2.Text + ";";              //기타(14)
                strSLIP3 += VB.Pstr(cboHealth3.Text, ".", 1) + ";"; //운동횟수(15)
                //호전가능(16-26)
                for (int i = 0; i <= 10; i++)
                {
                    CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth2.Checked == true ? "1" : "0";
                    strSLIP3 += ";";
                }
                strSLIP3 += txtHealth_Etc3.Text + ";";              //기타(27)
                strSLIP3 += txtHealth_Etc4.Text + ";";              //추가조치사항(28)
                strSLIP3 += VB.Pstr(cboHealth4.Text, ".", 1) + ";"; //근력운동(29)
            }

            //영양처방전
            if (tabDiet.Enabled == true)
            {
                txtDiet_ETC1.Text = txtDiet_ETC1.Text.Replace(";", ",");
                txtDiet_ETC2.Text = txtDiet_ETC2.Text.Replace(";", ",");
                strSLIP4 = VB.Pstr(cboDiet.Text, ".", 1) + ";";            //식생활습관(1)

                //식생활개선(2-11)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet.Checked == true ? "1" : "0";
                    strSLIP4 += ";";
                }
                //호전가능(12-20)
                for (int i = 0; i <= 8; i++)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet2.Checked == true ? "1" : "0";
                    strSLIP4 += ";";
                }
                strSLIP4 += txtDiet_ETC1.Text + ";";  //기타(21)
                strSLIP4 += txtDiet_ETC2.Text + ";";  //추가조치사항(22)
            }

            //비만처방전
            if (tabBiman.Enabled == true)
            {
                txtBiman20.Text = txtBiman20.Text.Replace(";", ",");
                txtBiman21.Text = txtBiman21.Text.Replace(";", ",");
                txtBiman3.Text = txtBiman3.Text.Replace(";", ",");
                txtBiman4.Text = txtBiman4.Text.Replace(";", ",");

                strSLIP5 = VB.Pstr(cboBiman0.Text, ".", 1) + ";";  //체중(1)
                strSLIP5 += VB.Pstr(cboBiman1.Text, ".", 1) + ";"; //비만(2)
                strSLIP5 += VB.Pstr(cboBiman2.Text, ".", 1) + ";"; //질환발생위험(3)
                strSLIP5 += txtBiman00.Text + ";";        //목표체중(4)
                strSLIP5 += txtBiman01.Text + ";";        //목표체중(5)
                strSLIP5 += txtBiman02.Text + ";";        //목표체중(6)
                strSLIP5 += txtBiman03.Text + ";";        //목표체중(7)
                //비만처방(8-14)
                for (int i = 0; i <= 6; i++)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman.Checked == true ? "1" : "0";
                    strSLIP5 += ";";
                }
                strSLIP5 += txtBiman20.Text + ";";       //약물치료(15)
                strSLIP5 += txtBiman21.Text + ";";       //기타(16)
                //호전가능(17-26)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i).To<string>(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman2.Checked == true ? "1" : "0";
                    strSLIP5 += ";";
                }
                strSLIP5 += txtBiman3.Text + ";";      //기타(27)
                strSLIP5 += txtBiman4.Text + ";";       //기타의견(28)
            }

            strSLIP6 = "";
            strSLIP7 = "";

            //clsDB.setBeginTran(clsDB.DbCon);

            //판정결과를 DB에 UPDATE
            HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

            item.SLIP_SMOKE = strSLIP1;
            item.SLIP_DRINK = strSLIP2;
            item.SLIP_ACTIVE = strSLIP3;
            item.SLIP_FOOD = strSLIP4;
            item.SLIP_BIMAN = strSLIP5;
            item.SLIP_LIFESOGEN1 = strSLIP6;
            item.SLIP_LIFESOGEN2 = strSLIP7;
            item.WRTNO = FnWrtNo;

            result = hicResBohum1Service.UpdateLifebyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("생활습관처방전 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return rtnVal;
            //clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Screen_Clear()
        {
            clsHcVariable.GnPanB_Etc = 0;
            clsHcVariable.Gstr_PanB_Etc = "";
            clsHcType.TFA.Sogen = "";
            FbAutoPanjeng = false;

            //FnPano = 0;             FstrJumin = "";             FnWrtNo = 0;
            FstrSex = ""; FstrROWID = "";
            FstrUCodes = ""; FstrCOMMIT = ""; FnPano = 0;
            FnWrtno1 = 0; FnWrtno2 = 0;
            FstrSaveGbn = ""; FstrPano = ""; FstrJumin = "";
            FstrGbOHMS = ""; FstrGbSPC = "";
            FstrPanOk1 = ""; FstrPanOk2 = "";
            FstrTFlag = ""; FstrJepDate2 = ""; FstrSpcTable = "";

            //금연처방전
            txtSmoke0.Text = "";
            cboSmoke1.SelectedIndex = -1;
            cboSmoke2.SelectedIndex = -1;
            cboSmoke3.SelectedIndex = -1;

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + i.To<string>(), true)[0] as CheckBox);
                CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + i.To<string>(), true)[0] as CheckBox);
                chkSmoking4.Checked = false;
                chkSmoking5.Checked = false;
            }

            txtSmoke4.Text = "";
            txtSmoke6.Text = "";
            txtSmoke7.Text = "";

            //음주/절주 처방전
            txtDrink1.Text = "";
            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i).To<string>(), true)[0] as CheckBox);
                chkDrink0.Checked = false;
            }
            cboDrink1.SelectedIndex = -1;

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkDrink2 = (Controls.Find("chkDrink2" + i.To<string>(), true)[0] as CheckBox);
                chkDrink2.Checked = false;
            }
            txtDrink3.Text = "";
            txtDrink4.Text = "";

            //운동처방전
            txtHealth0.Text = "";
            cboHealth1.SelectedIndex = -1; cboHealth2.SelectedIndex = -1; cboHealth3.SelectedIndex = -1;
            cboHealth1.SelectedIndex = -1; cboHealth2.SelectedIndex = -1; cboHealth3.SelectedIndex = -1;
            txtHealth_Etc1.Text = "";
            txtHealth_Etc2.Text = "";
            txtHealth_Etc3.Text = "";
            txtHealth_Etc4.Text = "";

            for (int i = 0; i <= 10; i++)
            {
                if (i < 10)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + (i).To<string>(), true)[0] as CheckBox);
                    chkHealth.Checked = false;
                }
            }

            //영양
            txtDiet0.Text = "";
            cboDiet.Text = "";
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkDiet = (Controls.Find("chkDiet" + (i).To<string>(), true)[0] as CheckBox);
                chkDiet.Checked = false;
                if (i < 9)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + i.To<string>(), true)[0] as CheckBox);
                    chkDiet2.Checked = false;
                }
            }
            txtDiet_ETC1.Text = "";
            txtDiet_ETC2.Text = "";

            //비만
            txtBiman0.Text = "";
            for (int i = 0; i <= 3; i++)
            {
                if (i < 3)
                {
                    ComboBox cboBiman = (Controls.Find("cboBiman" + (i).To<string>(), true)[0] as ComboBox);
                    cboBiman.SelectedIndex = -1;
                }
                if (i < 2)
                {
                    TextBox txtBiman2 = (Controls.Find("txtBiman2" + (i).To<string>(), true)[0] as TextBox);
                    txtBiman2.Text = "";
                }
                TextBox txtBiman0 = (Controls.Find("txtBiman0" + i.To<string>(), true)[0] as TextBox);
                txtBiman0.Text = "";
            }

            for (int i = 0; i <= 9; i++)
            {
                if (i < 7)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + (i).To<string>(), true)[0] as CheckBox);
                    chkBiman.Checked = false;
                }
                CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i).To<string>(), true)[0] as CheckBox);
                chkBiman2.Checked = false;
            }
            txtBiman3.Text = "";
            txtBiman4.Text = "";
        }

        /// <summary>
        /// 생활습관처방전 흡연,음주 자동판정
        /// </summary>
        void fn_Auto_Life_panjeng()
        {
            //흡연
            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubunCode(FnWrtNo, "11");

            if (list.Count == 1)
            {
                if (VB.Right(VB.Trim(list[0].CODE), 1) == "1") { cboSmoke3.SelectedIndex = 3; }
                if (VB.Right(VB.Trim(list[0].CODE), 1) == "2") { cboSmoke3.SelectedIndex = 2; }
                if (VB.Right(VB.Trim(list[0].CODE), 1) == "3") { cboSmoke3.SelectedIndex = 1; }
                if (VB.Right(VB.Trim(list[0].CODE), 1) == "4") { cboSmoke3.SelectedIndex = 1; }
            }

            //영양
            List<HIC_TITEM> list2 = hicTitemService.GetItembyWrtNoGubunCode(FnWrtNo, "14");

            if (list2.Count > 0)
            {
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (i).To<string>(), true)[0] as CheckBox);

                    if (i < 2)
                    {   
                        if (list2[i].JUMSU != 5) { chkDiet.Checked = true; }
                    }
                    else if ( i == 2 || i == 3)
                    {
                        if (list2[i].JUMSU != 5) { chkDiet2.Checked = true; }
                    }
                    else if (i >= 4)
                    {
                        if (list2[i].JUMSU != 5) { chkDiet.Checked = true; }
                    }
                }
            }

            //일반판정에따른 생활습관 병명체크
            //비만(운동,영양)
            if (frmHcPanHic.chkPanjengB0.Checked == true || frmHcPanHic.chkPanjengR10.Checked == true)
            {
                chkHealth20.Checked = true;
                chkDiet27.Checked = true;
            }

            //'고혈압(음주,운동,영양,비만)
            if (frmHcPanHic.chkPanjengB1.Checked == true || frmHcPanHic.chkPanjengR2.Checked == true || frmHcPanHic.chkPanjengU0.Checked == true)
            {
                chkDrink02.Checked = true;
                chkHealth22.Checked = true;
                chkDiet20.Checked = true;
                chkBiman23.Checked = true;
            }

            //'이상지질혈증(음주,운동,영양,비만)
            if (frmHcPanHic.chkPanjengB2.Checked == true || frmHcPanHic.chkPanjengR3.Checked == true || frmHcPanHic.chkPanjengU2.Checked == true)
            { 
                chkDrink06.Checked = true;
                chkHealth26.Checked = true;
                chkDiet23.Checked = true;
                chkBiman24.Checked = true;
            }

            //'당뇨(음주,운동,영양,비만)
            if (frmHcPanHic.chkPanjengB4.Checked == true || frmHcPanHic.chkPanjengR5.Checked == true || frmHcPanHic.chkPanjengU1.Checked == true)
            { 
                chkDrink04.Checked = true;
                chkHealth23.Checked = true;
                chkDiet21.Checked = true;
                chkBiman21.Checked = true;
            }

            //'우울증(음주,운동)
            if (string.Compare(frmHcPanHic.txtPHQScr.Text.Trim(), "5") >= 0 || VB.Left(frmHcPanHic.cboDepression.Text, 1) == "5")
            { 
                chkDrink00.Checked = true;
                chkHealth210.Checked = true;
            }
        }
    }
}
