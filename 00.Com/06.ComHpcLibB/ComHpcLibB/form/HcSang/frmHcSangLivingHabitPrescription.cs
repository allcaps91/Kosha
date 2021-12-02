using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSangLivingHabitPrescription.cs
/// Description     : 생활습관 처방전
/// Author          : 이상훈
/// Create Date     : 2020-01-17
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm생활습관처방전.frm(Frm생활습관처방전)" />

namespace ComHpcLibB
{
    public partial class frmHcSangLivingHabitPrescription : Form
    {
        HicJepsuPatientService hicJepsuPatientService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResultService hicResultService = null;
        HicTitemService hicTitemService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrJong;
        string FstrChasu;
        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrYear;
        string FstrUCodes;
        string FstrChul;
        long FnPano;
        long FnAge;
        long FnWrtNo;


        public frmHcSangLivingHabitPrescription(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResultService = new HicResultService();
            hicTitemService = new HicTitemService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.chkSmoking41.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking42.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking43.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking44.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking45.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking46.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking51.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking52.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking53.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking54.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking55.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkSmoking56.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink01.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink02.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink03.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink04.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink05.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink06.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink07.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink08.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink21.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink22.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink23.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink24.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink25.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDrink26.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth4.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth5.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth6.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth7.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth8.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth9.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth10.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth21.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth22.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth23.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth24.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth25.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth26.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth27.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth28.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth29.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth210.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkHealth211.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet4.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet5.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet6.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet7.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet8.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet9.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet10.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet21.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet22.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet23.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet24.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet25.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet26.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet27.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet28.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkDiet29.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman4.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman5.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman7.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman21.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman23.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman24.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman25.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman26.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman27.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman28.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman29.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.chkBiman210.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);

            this.cboBiman1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboBiman2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboBiman3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);

            this.cboDiet.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboDrink1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboHealth1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboHealth2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboHealth3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboHealth4.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboSmoke1.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboSmoke2.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);
            this.cboSmoke3.KeyPress += new KeyPressEventHandler(eChkBoxKeyPress);

            this.txtBiman11.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman12.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman13.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman14.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman21.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman22.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBiman3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtDiet_ETC1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDiet_ETC2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDiet0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHealth_Etc1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHealth_Etc2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHealth_Etc3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHealth_Etc4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHealth0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSmoke0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSmoke4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSmoke6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_combo_Set();
            fn_Screen_Clear();

            txtWrtNo.Text = FnWrtNo.To<string>();
            FstrUCodes = "";

            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWrtNo + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrJong = list.GJJONG;          //검진종류
            FstrChasu = list.GJCHASU;        //검진차수
            FstrPtno = list.PTNO;            //등록번호
            FnPano = list.PANO;                     //검진번호
            FnAge = (long)hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list.JUMIN2)); //나이
            FstrSex = list.SEX;              //성별
            FstrJepDate = list.JEPDATE.To<string>();  //접수일자
            FstrJumin = clsAES.DeAES(list.JUMIN2);  //주민번호
            FstrYear = list.GJYEAR;          //검진년도
            FstrUCodes = list.UCODES;        //유해물질
            FstrChul = list.GBCHUL;          //출장여부

            //검진자 기본정보 표시---------------
            
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = FnAge + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            tab1.Visible = false;
            tab2.Visible = false;
            tab3.Visible = false;
            tab4.Visible = false;
            tab5.Visible = false;

            //생활습관도구 처방전 TabVisible
            List<HIC_SUNAPDTL> list2 = hicSunapdtlService.GetAllbyWrtNo(FnWrtNo);

            for (int i = 0; i < list2.Count; i++)
            {
                switch (list2[i].CODE.Trim())
                {
                    case "1164":
                        tabControl1.SelectedTab = tab3;
                        tab3.Visible = true;
                        tab4.Visible = true;
                        tab5.Visible = true;
                        break;
                    case "1165":
                        tabControl1.SelectedTab = tab1;
                        tab1.Visible = true;
                        break;
                    case "1166":
                        tabControl1.SelectedTab = tab2;
                        tab2.Visible = true;
                        break;
                    default:
                        break;
                }
            }

            fn_Screen_Life_Slip();
        }

        /// <summary>
        /// 생활습관처방전
        /// </summary>
        void fn_Screen_Life_Slip()
        {
            int nREAD = 0;
            string strDAT = "";
            string strTSmoke1 = "";
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
            int nIlsu1 = 0;
            int nTime11 = 0;
            int nTime12 = 0;
            int nIlsu2 = 0;
            int nTime21 = 0;
            int nTime22 = 0;
            List<string> strExCode = new List<string>();

            string strCode = "";

            strExCode.Clear();
            strExCode.Add("A143");
            strExCode.Add("A144");
            strExCode.Add("A145");
            strExCode.Add("A146");
            strExCode.Add("A147");
            strExCode.Add("A130");
            strExCode.Add("A129");

            for (int i = 0; i <= 5; i++)
            {
                nJemsu[i] = 0;
            }
            txtLifeSogen1.Text = "";
            txtLifeSogen2.Text = "";

            HIC_RES_BOHUM1 list = hicResBohum1Service.GetIetmbyWrtNo(FnWrtNo);

            if (!list.IsNullOrEmpty())
            {
                strSLIP1 = list.SLIP_SMOKE;
                strSLIP2 = list.SLIP_DRINK;
                strSLIP3 = list.SLIP_ACTIVE;
                strSLIP4 = list.SLIP_FOOD;
                strSLIP5 = list.SLIP_BIMAN;
                strTSmoke1 = list.T_SMOKE1;
                strDrink = list.TMUN0003;
                txtLifeSogen1.Text = list.SLIP_LIFESOGEN1;
                txtLifeSogen2.Text = list.SLIP_LIFESOGEN2;
            }

            //생활습관평가도구표 결과값을 읽음
            List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCode);

            nREAD = list2.Count;
            for (int i = 0; i < nREAD; i++)
            {
                switch (list2[i].EXCODE)
                {
                    case "A143":
                        nJemsu[0] = list2[i].RESULT.To<long>();    //흡연
                        break;
                    case "A144":
                        nJemsu[1] = list2[i].RESULT.To<long>();    //음주
                        break;
                    case "A145":
                        nJemsu[2] = list2[i].RESULT.To<long>();    //운동
                        break;
                    case "A146":
                        nJemsu[3] = list2[i].RESULT.To<long>();    //영양
                        break;
                    case "A147":
                        nJemsu[4] = list2[i].RESULT.To<long>();    //비만
                        break;
                    default:
                        break;
                }
            }

            //금연처방전
            if (tab1.Visible == true)
            {
                txtSmoke0.Text = string.Format("{0:0}", nJemsu[0]);
                if (strSLIP1.IsNullOrEmpty())
                {
                    //흡연상태
                    cboSmoke1.SelectedIndex = 0;
                    if (strTSmoke1 == "1")      //1차 문진표에 비흡연자
                    {
                        cboSmoke1.SelectedIndex = 0;
                        cboSmoke2.SelectedIndex = 0;
                    }
                    else if (strTSmoke1 == "2") //문진표에 과거흡연자
                    {
                        cboSmoke1.SelectedIndex = 1;
                        cboSmoke2.SelectedIndex = strTSmoke1.To<int>();    //니코틴의존도
                    }
                    else
                    {
                        cboSmoke1.SelectedIndex = 2;
                        txtLifeSogen1.Text += "금연필요/";
                    }

                    //니코틴의존도
                    if (string.Compare(txtSmoke0.Text, "3") <= 0)
                    {
                        cboSmoke2.SelectedIndex = 1;
                    }
                    else if (string.Compare(txtSmoke0.Text, "4") >= 0 && string.Compare(txtSmoke0.Text, "6") <= 0)
                    {
                        cboSmoke2.SelectedIndex = 2;
                    }
                    else if (string.Compare(txtSmoke0.Text, "7") >= 0)
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
                            if (strDAT == VB.Pstr(cboSmoke1.Text, ".", 1))
                            {
                                cboSmoke1.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                    //니코틴의존도
                    strDAT = VB.Pstr(strSLIP1, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke2.Items.Count; i++)
                        {
                            if (strDAT == VB.Pstr(cboSmoke2.Text, ".", 1))
                            {
                                cboSmoke2.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                    //금연계획단계
                    strDAT = VB.Pstr(strSLIP1, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke3.Items.Count; i++)
                        {
                            if (strDAT == VB.Pstr(cboSmoke3.Text, ".", 1))
                            {
                                cboSmoke3.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                    //금연처방
                    chkSmoking41.Checked = VB.Pstr(strSLIP1, ";", 4) == "1" ? true : false;
                    chkSmoking42.Checked = VB.Pstr(strSLIP1, ";", 5) == "1" ? true : false;
                    chkSmoking43.Checked = VB.Pstr(strSLIP1, ";", 6) == "1" ? true : false;
                    chkSmoking44.Checked = VB.Pstr(strSLIP1, ";", 7) == "1" ? true : false;
                    chkSmoking45.Checked = VB.Pstr(strSLIP1, ";", 8) == "1" ? true : false;
                    chkSmoking46.Checked = VB.Pstr(strSLIP1, ";", 9) == "1" ? true : false;

                    //금연처방-기타
                    txtSmoke4.Text = VB.Pstr(strSLIP1, ";", 10);
                    //금단증상
                    chkSmoking51.Checked = VB.Pstr(strSLIP1, ";", 11) == "1" ? true : false;
                    chkSmoking52.Checked = VB.Pstr(strSLIP1, ";", 12) == "1" ? true : false;
                    chkSmoking53.Checked = VB.Pstr(strSLIP1, ";", 13) == "1" ? true : false;
                    chkSmoking54.Checked = VB.Pstr(strSLIP1, ";", 14) == "1" ? true : false;
                    chkSmoking55.Checked = VB.Pstr(strSLIP1, ";", 15) == "1" ? true : false;
                    chkSmoking56.Checked = VB.Pstr(strSLIP1, ";", 16) == "1" ? true : false;
                    txtSmoke6.Text = VB.Pstr(strSLIP1, ";", 17);
                }
            }

            //음주처방전
            if (tab2.Visible == true)
            {
                txtDrink1.Text = string.Format("{0:0}", nJemsu[3]);
                if (strSLIP2.IsNullOrEmpty())
                {
                    //음주
                    if (strDrink == "4")
                    {
                        cboDrink1.SelectedIndex = 1;
                    }
                    else if (string.Compare(txtDrink1.Text, "14") <= 0)
                    {
                        cboDrink1.SelectedIndex = 2;
                    }
                    else if (string.Compare(txtDrink1.Text, "15") >= 0 && string.Compare(txtDrink1.Text, "25") <= 0)
                    {
                        cboDrink1.SelectedIndex = 3;
                        txtLifeSogen1.Text += "절주필요/";
                    }
                    else if (string.Compare(txtDrink1.Text, "26") >= 0)
                    {
                        cboDrink1.SelectedIndex = 4;
                        txtLifeSogen1.Text += "절주필요/";
                    }
                }
                else
                {
                    //음주관련 질환
                    chkDrink01.Checked = VB.Pstr(strSLIP2, ";", 2) == "1" ? true : false;
                    chkDrink02.Checked = VB.Pstr(strSLIP2, ";", 3) == "1" ? true : false;
                    chkDrink03.Checked = VB.Pstr(strSLIP2, ";", 4) == "1" ? true : false;
                    chkDrink04.Checked = VB.Pstr(strSLIP2, ";", 5) == "1" ? true : false;
                    chkDrink05.Checked = VB.Pstr(strSLIP2, ";", 6) == "1" ? true : false;
                    chkDrink06.Checked = VB.Pstr(strSLIP2, ";", 7) == "1" ? true : false;
                    chkDrink07.Checked = VB.Pstr(strSLIP2, ";", 8) == "1" ? true : false;
                    chkDrink08.Checked = VB.Pstr(strSLIP2, ";", 9) == "1" ? true : false;

                    //음주상태
                    strDAT = VB.Pstr(strSLIP2, ";", 10);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int j = 0; j < cboDrink1.SelectedIndex; j++)
                        {
                            if (strDAT == VB.Pstr(cboDrink1.Text, ".", 1))
                            {
                                cboDrink1.SelectedIndex = j;
                                return;
                            }
                        }
                    }
                    //금주처방
                    chkDrink21.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;
                    chkDrink22.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;
                    chkDrink23.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;
                    chkDrink24.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;
                    chkDrink25.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;
                    chkDrink26.Checked = VB.Pstr(strSLIP2, ";", 11) == "1" ? true : false;

                    txtDrink3.Text = VB.Pstr(strSLIP2, ";", 17);
                }
            }

            //운동처방전
            if (tab3.Visible == true)
            {
                List<HIC_TITEM> list3 = hicTitemService.GetJumsuCodebyWrtNo(FnWrtNo);

                nIlsu1 = 0;
                nTime11 = 0;
                nTime12 = 0;
                nIlsu2 = 0;
                nTime21 = 0;
                nTime22 = 0;
                for (int i = 0; i < list3.Count; i++)
                {
                    switch (list3[i].JUMSU.To<string>())
                    {
                        case "910":
                            nIlsu1 = list3[i].CODE.To<int>();
                            break;
                        case "911":
                            nTime11 = list3[i].CODE.To<int>();
                            break;
                        case "912":
                            nTime12 = list3[i].CODE.To<int>();
                            break;
                        case "913":
                            nIlsu2 = list3[i].CODE.To<int>();
                            break;
                        case "914":
                            nTime21 = list3[i].CODE.To<int>();
                            break;
                        case "915":
                            nTime22 = list3[i].CODE.To<int>();
                            break;
                        default:
                            break;
                    }
                }

                //주간운동시간
                nTOT = ((nTime11 * 60) + nTime12) * nIlsu1 * 2;
                nTOT += ((nTime21 * 60) + nTime22) * nIlsu2;

                txtHealth0.Text = string.Format("{0:0}", nTOT);
                //운동상태
                if (string.Compare(txtHealth0.Text, "150") < 0)
                {
                    cboHealth1.SelectedIndex = 1;
                    if (strSLIP3.IsNullOrEmpty())
                    {
                        txtLifeSogen2.Text += "신체활동 필요/";
                    }
                }
                else if (string.Compare(txtHealth0.Text, "150") >= 0 && string.Compare(txtHealth0.Text, "300") < 0)
                {
                    cboHealth1.SelectedIndex = 2;
                }
                else if (string.Compare(txtHealth0.Text, "300") >= 0)
                {
                    cboHealth1.SelectedIndex = 3;
                }

                //근력운동
                strCode = hicTitemService.GetCodebyWrtNo(FnWrtNo);
                cboHealth4.Text = "";
                switch (strCode)
                {
                    case "31801":
                    case "31802":
                        cboHealth4.SelectedIndex = 1;
                        if (strSLIP3.IsNullOrEmpty())
                        {
                            txtLifeSogen2.Text += "근력운동 필요/";
                        }
                        break;
                    case "31803":
                    case "31804":
                    case "31805":
                    case "31806":
                        cboHealth4.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }

                if (!strSLIP3.IsNullOrEmpty())
                {
                    //운동종류
                    for (int k = 2; k <= 11; k++)
                    {
                        CheckBox chkHealth = (Controls.Find("chkHealth" + (k - 1).To<string>(), true)[0] as CheckBox);
                        chkHealth.Checked = VB.Pstr(strSLIP3, ";", k) == "1" ? true : false;
                    }
                    txtHealth_Etc1.Text = VB.Pstr(strSLIP3, ";", 12);

                    //운동시간
                    strDAT = VB.Pstr(strSLIP3, ";", 13);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int k = 0; k < cboHealth2.Items.Count; k++)
                        {
                            if (strDAT == VB.Pstr(cboHealth2.Text, ".", 1))
                            {
                                cboHealth2.SelectedIndex = k;
                            }
                            return;
                        }
                    }
                    txtHealth_Etc2.Text = VB.Pstr(strSLIP3, ";", 14);
                    //운동횟수
                    strDAT = VB.Pstr(strSLIP3, ";", 15);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int k = 0; k < cboHealth3.Items.Count; k++)
                        {
                            if (strDAT == VB.Pstr(cboHealth3.Text, ".", 1))
                            {
                                cboHealth3.SelectedIndex = k;
                            }
                            return;
                        }
                    }
                    //호전가능
                    for (int k = 17; k <= 27; k++)
                    {   
                        CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (k - 16).To<string>(), true)[0] as CheckBox);
                        chkHealth2.Checked = VB.Pstr(strSLIP3, ";", k) == "1" ? true : false;
                    }
                    txtHealth_Etc3.Text = VB.Pstr(strSLIP3, ";", 27);
                    txtHealth_Etc4.Text = VB.Pstr(strSLIP3, ";", 28);
                }
            }

            //영양처방전
            if (tab4.Visible == true)
            {
                txtDiet0.Text = string.Format("{0:0}", nJemsu[5]);
                if (strSLIP4.IsNullOrEmpty())
                {
                    //영양
                    if (string.Compare(txtDiet0.Text, "28") <= 0)
                    {
                        cboDiet.SelectedIndex = 3;
                        txtLifeSogen2.Text += "식생활습관 개선/";
                    }
                    else if (string.Compare(txtDiet0.Text.Trim(), "28") >= 0 && string.Compare(txtDiet0.Text, "38") <= 0)
                    {
                        cboDiet.SelectedIndex = 2;
                    }
                    else if (string.Compare(txtDiet0.Text, "39") >= 0)
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
                        for (int j = 0; j < cboDiet.Items.Count; j++)
                        {
                            if (strDAT == VB.Pstr(cboDiet.Text, ".", 1))
                            {
                                cboDiet.SelectedIndex = j;
                                break;
                            }
                        }
                    }
                    //개선처방
                    for (int j = 2; j <= 11; j++)
                    {   
                        CheckBox chkDiet = (Controls.Find("chkDiet" + (j - 1).To<string>(), true)[0] as CheckBox);
                        chkDiet.Checked = VB.Pstr(strSLIP4, ";", j) == "1" ? true : false;
                    }
                    //호전가능
                    for (int j = 12; j <= 20; j++)
                    {
                        CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (j - 11).To<string>(), true)[0] as CheckBox);
                        chkDiet2.Checked = VB.Pstr(strSLIP4, ";", j) == "1" ? true : false;
                    }
                    txtDiet_ETC1.Text = VB.Pstr(strSLIP4, ";", 21);
                    txtDiet_ETC2.Text = VB.Pstr(strSLIP4, ";", 22);
                }
            }

            //비만처방전
            if (tab5.Visible == true)
            {
                txtBiman0.Text = string.Format("{0:0}", nJemsu[4]);
                if (strSLIP5.IsNullOrEmpty())
                {
                    nBiman1 = 0;
                    nBiman2 = 0;

                    strExCode.Clear();
                    strExCode.Add("A115");
                    strExCode.Add("A117");

                    List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCode);

                    for (int i = 0; i < list3.Count; i++)
                    {
                        if (list3[i].EXCODE == "A117")
                        {
                            nBiman1 = list3[i].RESULT.To<double>();
                        }
                        if (list3[i].EXCODE == "A115")
                        {
                            nBiman2 = list3[i].RESULT.To<double>();
                        }
                    }
                    //비만도체크
                    if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
                    {
                        cboBiman1.SelectedIndex = 1;
                    }
                    else if (nBiman1 >= 25 && nBiman1 <= 29.9)
                    {
                        cboBiman1.SelectedIndex = 2;
                        txtLifeSogen2.Text += "체중관리/";
                    }
                    else if (nBiman1 >= 30)
                    {
                        cboBiman1.SelectedIndex = 3;
                        txtLifeSogen2.Text += "체중관리/";
                    }

                    //복부비만
                    switch (FstrSex)
                    {
                        case "M":
                            if (nBiman2 >= 90)
                            {
                                cboBiman2.SelectedIndex = 1;
                                txtLifeSogen2.Text += "복부비만관리/";
                            }
                            else if (nBiman2 < 90)
                            {
                                cboBiman2.SelectedIndex = 2;
                            }
                            break;
                        case "F":
                            if (nBiman2 >= 85)
                            {
                                cboBiman2.SelectedIndex = 1;
                                txtLifeSogen2.Text += "복부비만관리/";
                            }
                            else if (nBiman2 < 85)
                            {
                                cboBiman2.SelectedIndex = 2;
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
                        for (int j = 0; j < cboBiman1.Items.Count; j++)
                        {
                            if (strDAT == VB.Pstr(cboBiman1.Text, ".", 1))
                            {
                                cboBiman1.SelectedIndex = j;
                                break;
                            }
                        }
                    }
                    //비만
                    strDAT = VB.Pstr(strSLIP5, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int j = 0; j < cboBiman2.Items.Count; j++)
                        {
                            if (strDAT == VB.Pstr(cboBiman2.Text, ".", 1))
                            {
                                cboBiman2.SelectedIndex = j;
                                break;
                            }
                        }
                    }
                    //질환발생위험
                    strDAT = VB.Pstr(strSLIP5, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int j = 0; j < cboBiman3.Items.Count; j++)
                        {
                            if (strDAT == VB.Pstr(cboBiman3.Text, ".", 1))
                            {
                                cboBiman3.SelectedIndex = j;
                                break;
                            }
                        }
                    }
                    txtBiman11.Text = VB.Pstr(strSLIP5, ";", 4);
                    txtBiman12.Text = VB.Pstr(strSLIP5, ";", 5);
                    txtBiman13.Text = VB.Pstr(strSLIP5, ";", 6);
                    txtBiman14.Text = VB.Pstr(strSLIP5, ";", 7);
                    //비만처방
                    for (int j = 8; j <= 14; j++)
                    {
                        CheckBox chkBiman = (Controls.Find("chkBiman" + (j - 7).To<string>(), true)[0] as CheckBox);
                        chkBiman.Checked = VB.Pstr(strSLIP5, ";", j) == "1" ? true : false;
                    }
                    txtBiman21.Text = VB.Pstr(strSLIP5, ";", 15);
                    txtBiman22.Text = VB.Pstr(strSLIP5, ";", 16);
                    //호전가능
                    for (int j = 17; j <= 26; j++)
                    {
                        CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (j - 16).To<string>(), true)[0] as CheckBox);
                        chkBiman2.Checked = VB.Pstr(strSLIP5, ";", j) == "1" ? true : false;
                    }
                    txtBiman3.Text = VB.Pstr(strSLIP5, ";", 27);
                }
            }

            if (txtLifeSogen1.Text.IsNullOrEmpty())
            {
                txtLifeSogen1.Text = "특이소견 없음/";
            }

            if (txtLifeSogen2.Text.IsNullOrEmpty())
            {
                txtLifeSogen2.Text = "특이소견 없음/";
            }

            //생활습관도구 판넬을 처음것을 열기
            if (tab1.Visible == true)
            {
                tabControl1.SelectedTab = tab1;
                Application.DoEvents();
            }

            if (tab2.Visible == true)
            {
                tabControl1.SelectedTab = tab2;
                Application.DoEvents();
            }

            if (tab3.Visible == true)
            {
                tabControl1.SelectedTab = tab3;
                Application.DoEvents();
            }

            if (tab4.Visible == true)
            {
                tabControl1.SelectedTab = tab4;
                Application.DoEvents();
            }

            if (tab5.Visible == true)
            {
                tabControl1.SelectedTab = tab5;
                Application.DoEvents();
            }

            //if (tab1.Visible == false && tab2.Visible == false && tab3.Visible == false && tab4.Visible == false && tab5.Visible == false)
            //{
            //    tab1.Visible = true;
            //    panel3.Enabled = false;
            //}
            //else
            //{
            //    panel3.Enabled = true;
            //}
        }

        void fn_combo_Set()
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
            cboBiman1.Items.Clear();
            cboBiman1.Items.Add(" ");
            cboBiman1.Items.Add("1.정상체중");
            cboBiman1.Items.Add("2.과체중");
            cboBiman1.Items.Add("3.비만");
            cboBiman1.SelectedIndex = 0;

            //비만
            cboBiman2.Items.Clear();
            cboBiman2.Items.Add(" ");
            cboBiman2.Items.Add("1.복부비만");
            cboBiman2.Items.Add("2.정상");
            cboBiman2.SelectedIndex = 0;

            //질환발생위험
            cboBiman3.Items.Clear();
            cboBiman3.Items.Add(" ");
            cboBiman3.Items.Add("1.낮음");
            cboBiman3.Items.Add("2.보통");
            cboBiman3.Items.Add("3.다소증가");
            cboBiman3.Items.Add("4.어느정도증가");
            cboBiman3.Items.Add("5.상당히증가");
            cboBiman3.Items.Add("6.매우증가");
            cboBiman3.SelectedIndex = 0;
        }

        void fn_Screen_Clear()
        {
            //금연처방전
            txtSmoke0.Text = "";
            cboSmoke1.Text = "";
            cboSmoke2.Text = "";
            cboSmoke3.Text = "";
            chkSmoking41.Checked = false;
            chkSmoking42.Checked = false;
            chkSmoking43.Checked = false;
            chkSmoking44.Checked = false;
            chkSmoking45.Checked = false;
            chkSmoking46.Checked = false;

            chkSmoking51.Checked = false;
            chkSmoking52.Checked = false;
            chkSmoking53.Checked = false;
            chkSmoking54.Checked = false;
            chkSmoking55.Checked = false;
            chkSmoking56.Checked = false;

            txtSmoke4.Text = "";
            txtSmoke6.Text = "";

            //음주/절주 처방전
            txtDrink1.Text = "";

            chkDrink01.Checked = false;
            chkDrink02.Checked = false;
            chkDrink03.Checked = false;
            chkDrink04.Checked = false;
            chkDrink05.Checked = false;
            chkDrink06.Checked = false;
            chkDrink07.Checked = false;
            chkDrink08.Checked = false;

            cboDrink1.Text = "";
            txtDrink3.Text = "";

            //운동처방전
            txtHealth0.Text = "";
            cboHealth1.Text = ""; cboHealth2.Text = ""; cboHealth3.Text = "";
            cboHealth1.Text = ""; cboHealth2.Text = ""; cboHealth3.Text = "";
            txtHealth_Etc1.Text = ""; txtHealth_Etc2.Text = ""; txtHealth_Etc3.Text = ""; txtHealth_Etc4.Text = "";

            for (int i = 1; i <= 11; i++)
            {   
                CheckBox chkHealth2 = (Controls.Find("chkHealth2" + i.To<string>(), true)[0] as CheckBox);
                chkHealth2.Checked = false;

                if (i < 11)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + i.To<string>(), true)[0] as CheckBox);
                    chkHealth.Checked = false;
                }
            }

            //영양
            txtDiet0.Text = "";
            cboDiet.Text = "";
            for (int i = 1; i <= 9; i++)
            {
                CheckBox chkDiet = (Controls.Find("chkDiet" + i.To<string>(), true)[0] as CheckBox);                
                chkDiet.Checked = false;
                if (i < 10)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + i.To<string>(), true)[0] as CheckBox);
                    chkDiet2.Checked = false;
                }
            }
            txtDiet_ETC1.Text = "";
            txtDiet_ETC2.Text = "";

            //비만
            txtBiman0.Text = "";
            for (int i = 1; i <= 4; i++)
            {   
                TextBox txtBiman1 = (Controls.Find("txtBiman1" + i.To<string>(), true)[0] as TextBox);
                txtBiman1.Text = "";

                if (i < 4)
                {
                    ComboBox cboBiman = (Controls.Find("cboBiman" + i.To<string>(), true)[0] as ComboBox);
                    cboBiman1.Text = "";
                }                
                if (i < 3)
                {
                    TextBox txtBiman2 = (Controls.Find("txtBiman2" + i.To<string>(), true)[0] as TextBox);
                    txtBiman2.Text = "";
                }
            }

            for (int i = 1; i <= 10; i++)
            {   
                CheckBox chkBiman2 = (Controls.Find("chkBiman2" + i.To<string>(), true)[0] as CheckBox);
                chkBiman2.Checked = false;
                if (i < 8)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + i.To<string>(), true)[0] as CheckBox);
                    chkBiman.Checked = false;
                }
            }
            txtBiman3.Text = "";

            tab1.Visible = false;
            tab2.Visible = false;
            tab3.Visible = false;
            tab4.Visible = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strChk = "";

                //생활습관처방전 누락
                if (tab1.Visible == true)
                {
                    if (VB.Left(cboSmoke1.Text, 1) == "2")  //현재 흡연자
                    {
                        if (cboSmoke3.Text.IsNullOrEmpty())
                        {
                            MessageBox.Show("생활습관 금연처방전 결과가 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //금연처방
                        if (chkSmoking41.Checked == true ||
                            chkSmoking42.Checked == true ||
                            chkSmoking43.Checked == true ||
                            chkSmoking44.Checked == true ||
                            chkSmoking45.Checked == true ||
                            chkSmoking46.Checked == true)
                        {
                            strChk = "Y";
                        }

                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("금연처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (tab2.Visible == true)
                {
                    if (cboDrink1.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("생활습관 음주상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (VB.Left(cboDrink1.Text, 1) == "3" || VB.Left(cboDrink1.Text, 1) == "4")
                    {
                        //금주/절주 처방
                        strChk = "";
                        if (chkDrink21.Checked == true ||
                            chkDrink22.Checked == true ||
                            chkDrink23.Checked == true ||
                            chkDrink24.Checked == true ||
                            chkDrink25.Checked == true ||
                            chkDrink26.Checked == true)
                        {
                            strChk = "Y";
                        }

                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("금주/절주 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (tab3.Visible == true)
                {
                    if (cboHealth1.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("운동상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (VB.Left(cboHealth1.Text.Trim(), 1) == "1" || VB.Left(cboHealth4.Text.Trim(), 1) == "1")
                    {
                        //운동종류
                        strChk = "";
                        if (chkHealth1.Checked == true ||
                            chkHealth2.Checked == true ||
                            chkHealth3.Checked == true ||
                            chkHealth4.Checked == true ||
                            chkHealth5.Checked == true ||
                            chkHealth6.Checked == true ||
                            chkHealth7.Checked == true ||
                            chkHealth8.Checked == true ||
                            chkHealth9.Checked == true ||
                            chkHealth10.Checked == true)
                        {
                            strChk = "Y";
                        }

                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("운동종류 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (cboHealth2.Text.IsNullOrEmpty())
                        {
                            MessageBox.Show("운동시간이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (cboHealth3.Text.IsNullOrEmpty())
                        {
                            MessageBox.Show("운동횟수가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (tab4.Visible == true)
                {
                    if (cboDiet.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("식생활습관 결과가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (VB.Left(cboDiet.Text, 1) == "3")
                    {
                        //식생활 개선처방
                        strChk = "";
                        if (chkDiet1.Checked == true ||
                            chkDiet2.Checked == true ||
                            chkDiet3.Checked == true ||
                            chkDiet4.Checked == true ||
                            chkDiet5.Checked == true ||
                            chkDiet6.Checked == true ||
                            chkDiet7.Checked == true ||
                            chkDiet8.Checked == true ||
                            chkDiet9.Checked == true ||
                            chkDiet10.Checked == true)
                        {
                            strChk = "Y";
                        }

                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("식생활개선 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (tab5.Visible == true)
                {
                    if (VB.Left(cboBiman1.Text, 1) == "3" || VB.Left(cboBiman2.Text, 1) == "1")
                    {
                        strChk = "";
                        if (chkBiman1.Checked == true ||
                            chkBiman2.Checked == true ||
                            chkBiman3.Checked == true ||
                            chkBiman4.Checked == true ||
                            chkBiman5.Checked == true ||
                            chkBiman6.Checked == true ||
                            chkBiman7.Checked == true)
                        {
                            strChk = "Y";
                        }

                        if (!txtBiman21.Text.IsNullOrEmpty()) strChk = "Y";
                        if (!txtBiman22.Text.IsNullOrEmpty()) strChk = "Y";
                        if (strChk.IsNullOrEmpty())
                        {
                            MessageBox.Show("비만 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    for (int i = 1; i <= txtBiman12.Text.Length; i++)
                    {
                        if (VB.Mid(txtBiman12.Text, i, 1) == ".")
                        {
                            MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    for (int i = 1; i <= txtBiman14.Text.Length; i++)
                    {
                        if (VB.Mid(txtBiman14.Text, i, 1) == ".")
                        {
                            MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                fn_DB_Update_LifeSlip();
                this.Close();
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void fn_DB_Update_LifeSlip()
        {
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            string strSLIP6 = "";
            string strSLIP7 = "";
            int result = 0;

            //금연처방전
            if (tab1.Visible == true)
            {
                txtSmoke4.Text = txtSmoke4.Text.Replace(";", ",");
                txtSmoke6.Text = txtSmoke6.Text.Replace(";", ",");
                strSLIP1 = VB.Pstr(cboSmoke1.Text, ".", 1) + ";";           //흡연상태(1)
                strSLIP1 = strSLIP1 + VB.Pstr(cboSmoke2.Text, ".", 1) + ";";   //니코틴의존도(2)
                strSLIP1 = strSLIP1 + VB.Pstr(cboSmoke3.Text, ".", 1) + ";";   //금연계획단계(3)
                //금연처방(4-9)
                for (int j = 1; j <= 6; j++)
                {
                    CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking4.Checked + ";";
                }
                strSLIP1 += txtSmoke4.Text.Trim() + ";";    //금연계획단계(10)
                //금단증상극복(11-16)
                for (int j = 1; j <= 6; j++)
                {
                    CheckBox chkSmoking5 = (Controls.Find("chkSmoking4" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking5.Checked + ";";
                }
                strSLIP1 += txtSmoke6.Text.Trim() + ";";    //추가조치사항(17)
            }

            //음주처방전
            if (tab2.Visible == true)
            {
                txtDrink3.Text = txtDrink3.Text.Replace(";", ",");
                strSLIP2 = txtDrink1.Text.Trim() + ";";                   //평가점수(1)
                //영향질환(2-9)
                for (int j = 1; j <= 8; j++)
                {
                    CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink0.Checked + ";";
                }
                strSLIP2 += VB.Pstr(cboDrink1.Text, ".", 1) + ";"; //음주상태(10)
                //금주/절주처방(11-16)
                for (int j = 1; j <= 6; j++)
                {
                    CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink2.Checked + ";";
                }
                strSLIP2 += txtDrink3.Text.Trim() + ";";  //추가조치사항(17)
            }

            //'운동처방전
            if (tab3.Visible == true)
            {
                txtHealth_Etc1.Text = txtHealth_Etc1.Text.Replace(";", ",");
                txtHealth_Etc2.Text = txtHealth_Etc2.Text.Replace(";", ",");
                txtHealth_Etc3.Text = txtHealth_Etc3.Text.Replace(";", ",");
                txtHealth_Etc4.Text = txtHealth_Etc4.Text.Replace(";", ",");

                strSLIP3 = VB.Pstr(cboHealth1.Text, ".", 1) + ";";           //운동상태(1)
                //운동종류(2-11)
                for (int j = 1; j <= 10; j++)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth.Checked + ";";
                }
                strSLIP3 += txtHealth_Etc1.Text.Trim() + ";";    //기타(12)
                strSLIP3 += VB.Pstr(cboHealth2.Text, ".", 1) + ";"; //운동시간(13)
                strSLIP3 += txtHealth_Etc2.Text.Trim() + ";";    //기타(14)
                strSLIP3 += VB.Pstr(cboHealth3.Text, ".", 1) + ";"; //운동횟수(15)
                //호전가능(16-26)
                for (int j = 1; j <= 11; j++)
                {
                    CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth2.Checked + ";";
                }
                strSLIP3 += txtHealth_Etc3.Text.Trim() + ";";    //기타(27)
                strSLIP3 += txtHealth_Etc4.Text.Trim() + ";";    //추가조치사항(28)
                strSLIP3 += VB.Pstr(cboHealth4.Text, ".", 1) + ";"; //근력운동(29)
            }

            //'영양처방전
            if (tab4.Visible == true)
            {
                txtDiet_ETC1.Text = txtDiet_ETC1.Text.Replace(";", ",");
                txtDiet_ETC2.Text = txtDiet_ETC2.Text.Replace(";", ",");

                strSLIP4 = VB.Pstr(cboDiet.Text, ".", 1) + ";";           //식생활습관(1)
                //식생활개선(2-11)
                for (int j = 1; j <= 10; j++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet.Checked + ";";
                }

                //호전가능(12-20)
                for (int j = 1; j <= 9; j++)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet2.Checked + ";";
                }
                strSLIP4 += txtDiet_ETC1.Text.Trim() + ";"; //기타(21)
                strSLIP4 += txtDiet_ETC2.Text.Trim() + ";"; //가조치사항(22)
            }

            //비만처방전
            if (tab5.Visible == true)
            {
                txtBiman21.Text = txtBiman21.Text.Replace(";", ",");
                txtBiman22.Text = txtBiman22.Text.Replace(";", ",");
                txtBiman3.Text = txtBiman3.Text.Replace(";", ",");

                strSLIP5 = VB.Pstr(cboBiman1.Text, ".", 1) + ";";  //체중(1)
                strSLIP5 += VB.Pstr(cboBiman2.Text, ".", 1) + ";"; //비만(2)
                strSLIP5 += VB.Pstr(cboBiman3.Text, ".", 1) + ";"; //질환발생위험(3)
                strSLIP5 += txtBiman11.Text.Trim() + ";";        //목표체중(4)
                strSLIP5 += txtBiman12.Text.Trim() + ";";        //목표체중(5)
                strSLIP5 += txtBiman13.Text.Trim() + ";";        //목표체중(6)
                strSLIP5 += txtBiman14.Text.Trim() + ";";        //목표체중(7)
                //비만처방(8-14)
                for (int j = 1; j <= 7; j++)
                {
                    CheckBox chkBiman1 = (Controls.Find("chkBiman1" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman1.Checked + ";";
                }
                strSLIP5 += txtBiman21.Text.Trim() + ";";       //약물치료(15)
                strSLIP5 += txtBiman22.Text.Trim() + ";";       //기타(16)
                //호전가능(17-26)
                for (int j = 1; j <= 10; j++)
                {
                    CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (j).To<string>(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman2.Checked + ";";
                }
                strSLIP5 += txtBiman3.Text.Trim() + ";";       //기타(27)
            }
            strSLIP6 = txtLifeSogen1.Text.Trim().Replace("'", "`");
            strSLIP7 = txtLifeSogen2.Text.Trim().Replace("'", "`");

            clsDB.setBeginTran(clsDB.DbCon);

            //판정결과를 DB에 UPDATE
            result = hicResBohum1Service.UpdateSlipbyWrtNo(strSLIP1, strSLIP2, strSLIP3, strSLIP4, strSLIP5, strSLIP6, strSLIP7, FnWrtNo);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("생활습관처방전 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        void eChkBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
