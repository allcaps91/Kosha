using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmPanjeng_Spc.cs
/// Description     : 특수검진 판정
/// Author          : 이상훈
/// Create Date     : 2020-05-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm1차일반특수판정_2019.frm(Frm1차일반특수판정_2019) " />


namespace HC_Pan
{
    /// <summary>
    /// TODO: panSpc 특수물질 선택시 Enable = true  / 아닐시 Enable = false
    /// </summary>
    public partial class frmPanjeng_Spc : Form
    {
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
        HicSpcScodeService hicSpcScodeService = null;

        frmHcPanPanDrExamResultInput FrmHcPanPanDrExamResultInput = null;
        frmHcCodeHelp FrmHcCodeHelp = null;
        frmHcPanPanjengHelp FrmHcPanPanjengHelp = null;
        frmHcPanJochiHelp FrmHcPanJochiHelp = null;
        frmHcPanSpcSahuCode FrmHcPanSpcSahuCode = null;
        frmHcActPFTMunjin FrmHcActPFTMunjin = null;
        frmHcPanOpinionCounselReg FrmHcPanOpinionCounselReg = null;

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
        string FstrJong;    // 건진종류
        
        string FstrPtNo;
        string FstrGjChasu;
        string FstrJob;

        long FnWrtno1;      //1차
        long FnWrtno2;      //2차        
        string FstrSaveGbn;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;

        long FnRow;
        bool bCboPanChange = true;

        string FstrPROWID;
        string FstrOK;
        string FstrOldSpcPan;           //특수판정코드 변경 여부
        string FstrSelTab;
        string FstrJepDate;             //2차 접수일자
        string FstrSpcTable;            //취급물질테이블(P.HIC_SPC_PANJENG H:HIC_SPC_PANHIS)

        List<string> FstrNotAddPanList = new List<string>();       //2015-09-01일부 추가판정 제외 그룹코드 목록
        
        string[] FstrHabit = new string[18];

        List<HC_PANJENG_PATLIST> FPatListItem;

        string FstrGbHea;   //검진당일 종검수검 여부
        FpSpread FspdNm;

        string FstrCode;
        string FstrName;
        public frmPanjeng_Spc()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmPanjeng_Spc(long nWrtNo, HC_PANJENG_PATLIST patListItem, List<HC_PANJENG_PATLIST> lstPatListItem, FpSpread SpdNm)
        {
            InitializeComponent();

            FnWrtNo      = nWrtNo;
            FstrPtNo     = patListItem.PTNO;
            FstrJumin    = patListItem.JUMIN2;
            FnPano       = patListItem.PANO;
            FstrJepDate  = patListItem.JEPDATE;
            FstrSex      = patListItem.SEX;
            FstrSelTab   = patListItem.SELECTTAB;
            FPatListItem = lstPatListItem;
            FstrJob      = patListItem.JOB;
            FspdNm       = SpdNm;
            FstrGbHea    = patListItem.JONGGUMYN;
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
            hicSpcScodeService = new HicSpcScodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnJochi2.Click += new EventHandler(eBtnClick);
            this.btnReExam.Click += new EventHandler(eBtnClick);
            this.btnSpcAll.Click += new EventHandler(eBtnClick);
            this.btnCap.Click += new EventHandler(eBtnClick);
            this.btnPft.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSogen2.Click += new EventHandler(eBtnClick);
            
            this.btnSahu.Click += new EventHandler(eBtnClick);
            this.btnSahuSangdam.Click += new EventHandler(eBtnClick);
            this.ssPan.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.cboPanjeng2.Click += new EventHandler(eCboClick);
            this.cboPanjeng2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.txtSogen2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        private void eCboSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == cboPanjeng2)
            {
                if (FstrMCode == "") { return; }
                if (cboPanjeng2.Text.Trim() == "") { return; }
                if (!chkAutoPan.Checked) { return; }
                if (bCboPanChange == false)
                {
                    return;
                }

                string strPan = VB.Pstr(cboPanjeng2.Text, ".", 1);

                HIC_SPC_SCODE item = hicSpcScodeService.GetItemByMCodePanjeng(strPan, FstrMCode);

                if (!item.IsNullOrEmpty())
                {
                    txtSogen2.Text = item.CODE.To<string>("").Trim() + "." + item.NAME.To<string>("").Trim();
                    txtSogenRemark.Text = item.SOGENREMARK.To<string>("").Trim();
                    txtJochiRemark.Text = item.JOCHIREMARK.To<string>("").Trim();
                    for (int i = 0; i < cboUpmu.Items.Count; i++)
                    {
                        if (VB.Pstr(cboUpmu.Items[i].ToString(), ".", 1) == item.WORKYN.To<string>("").Trim())
                        {
                            cboUpmu.SelectedIndex = i;
                            break;
                        }
                    }

                    if (!item.SAHUCODE.IsNullOrEmpty())
                    {
                        txtSahu.Text = item.SAHUCODE.To<string>("");
                        txtSahu.Text += "." + hm.Sahu_Names_Display(txtSahu.Text); //사후관리
                    }
                    if(!item.REEXAM.IsNullOrEmpty())
                    {
                        txtReExam.Text = item.REEXAM;
                    }
                    else
                    {
                        txtReExam.Text = "";
                    }
                }
            }
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtSahuRemark.Text = "";
            txtSogenRemark.Text = "";
            txtJochiRemark.Text = "";
            lblNightMunjinView.Text = "";

            tabNightMunjin.Visible = false;

            tabNgtMun1.Visible = false;
            tabNgtMun2.Visible = false;
            tabNgtMun3.Visible = false;
            tabNgtMun4.Visible = false;
            tabNgtMun5.Visible = false;

            fn_ComboSet(cboUpmu, "13");             //업무적합성
            //fn_ComboSet(cboSahu, "32");             //사후관리

            hc.ComboOrgan_SET(cboOrgan);            //표적장기코드세팅
            fn_ComboOrganSayuSet(cboOrganSayu);     //장기선정사유

            hc.ComboSPanjeng_SET(cboPanjeng2);      //특수판정
            

            //생애 추가판정 제외 그룹코드 목록
            List<HIC_BCODE> list = hicBcodeService.GetItembyGubun("HIC_추가판정제외코드");

            FstrNotAddPanList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                FstrNotAddPanList.Add(list[i].CODE);
            }

            if (FnWrtNo > 0)
            {
                fn_Screen_Display();                
                //hf.fn_His_Screen_Display(ssHistory, FstrJumin, FnPano);
            }
            else
            {
                if (FstrJob == "N" && clsHcVariable.GnHicLicense > 0)   //미판정
                {
                    fn_Screen_Clear();
                }
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboPanjeng2)
            {
                #region 기존 판정 기준
                ////R에서 다른 판정으로 변경 시 소견내역,조치내역 Clear
                //if (FstrOldSpcPan == "7" && VB.Left(cboPanjeng2.Text, 1) != FstrOldSpcPan)
                //{
                //    txtSogen2.Text = "";
                //    txtSogenRemark.Text = "";
                //    txtJochiRemark.Text = "";
                //}

                //if (VB.Left(cboPanjeng2.Text, 1) == "1")
                //{
                //    txtSogen2.Text = "001" + "." + hb.READ_SpcPanjeng_Name(txtSogen2.Text); //소견
                //    txtSogenRemark.Text = hb.READ_SpcPanjeng_Name(txtSogen2.Text);
                //    txtJochiRemark.Text = "";

                //    cboUpmu.SelectedIndex = 1;
                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "3")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "4")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "5")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "6")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "7")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "8")
                //{

                //}
                //else if (VB.Left(cboPanjeng2.Text, 1) == "9" || VB.Left(cboPanjeng2.Text, 1) == "A")
                //{

                //}
                #endregion

            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtSogen2)
                {
                    FrmHcCodeHelp = new frmHcCodeHelp("54"); //특수판정소견코드
                    FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                    FrmHcCodeHelp.ShowDialog();
                    FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                    //if (!CodeHelpItem.IsNullOrEmpty())
                    if (!FstrCode.IsNullOrEmpty()) 
                    {
                        //txtSogen2.Text = CodeHelpItem.CODE + "." + CodeHelpItem.NAME;
                        txtSogen2.Text = FstrCode + "." + FstrName;
                    }
                    else
                    {
                        txtSogen2.Text = "";
                    }
                    SendKeys.Send("{TAB}");
                    if (VB.Pstr(txtSogen2.Text, ".", 1) == "1101" || VB.Pstr(txtSogen2.Text, ".", 1) == "1102" || 
                        VB.Pstr(txtSogen2.Text, ".", 1) == "1103" || VB.Pstr(txtSogen2.Text, ".", 1) == "1104" ||
                        VB.Pstr(txtSogen2.Text, ".", 1) == "1105" || VB.Pstr(txtSogen2.Text, ".", 1) == "1106")
                    {
                        //txtJochi.Text = "C00C" + "." + hb.READ_HIC_CODE("14", txtJochi.Text);   // 조치;
                    }
                    else
                    {
                        fn_Panjeng_Auto_Jochi_SET();
                    }
                }
            }
        }

        /// <summary>
        /// 김중구소장님 요청으로 자동 조치내역 설정
        /// </summary>
        void fn_Panjeng_Auto_Jochi_SET()
        {
            string strResult = "";
            string strUpmu = "";

            //조치내역이 공란이 아니면 자동설정 안함
            if (!txtSogenRemark.Text.IsNullOrEmpty())
            {
                return;
            }

            strResult = hm.Pajeng_Auto_JochiRemark(VB.Left(cboPanjeng2.Text, 1), VB.Pstr(txtSogen2.Text, ".", 1), VB.Pstr(txtSogen2.Text, ".", 2));
            if (strResult.IsNullOrEmpty())
            {
                txtSogenRemark.Text = VB.Pstr(txtSogen2.Text, ".", 2);
                return;
            }

            //결과값:(1)소견내역 (2)조치내역 (3)업무적합성 (4)사후관리 (5)추가검사
            txtSogenRemark.Text = VB.Pstr(strResult, "{$}", 1);
            txtJochiRemark.Text = VB.Pstr(strResult, "{$}", 2);
            strUpmu = VB.Pstr(strResult, "{$}", 3);
            for (int i = 0; i < cboUpmu.Items.Count; i++)
            {
                if (VB.Left(cboUpmu.Items[i].To<string>(), 3) == strUpmu)
                {
                    cboUpmu.SelectedIndex = i;
                    break;
                }
            }
            txtSahu.Text = VB.Pstr(strResult, "{$}", 4);
            txtReExam.Text = VB.Pstr(strResult, "{$}", 5);
            for (int i = 0; i < cboUpmu.Items.Count; i++)
            {
                if (VB.Left(cboUpmu.Items[i].To<string>(), 3) == strUpmu)
                {
                    cboUpmu.SelectedIndex = i;
                    break;
                }
            }
            //PanelJobYN.Caption = hb.READ_HIC_CODE("13", VB.Pstr(txtJobYN.Text, ".", 1));   //업무접합성
            if (txtSahu.Text.Trim() != "")
            {
                txtSahuRemark.Text = txtSahu.Text + "." + hm.Sahu_Names_Display(txtSahu.Text);  //사후관리
            }
        }

        void fn_Screen_Display()
        {
            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWrtNo) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWrtNo + " 는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(FnWrtNo);

            FstrJong     = item.GJJONG;
            FstrGjChasu  = item.GJCHASU;

            fn_Screen_Munjin_Night();   //야간문진표 작성 Data
            fn_Screen_Spc_Display();    //특검판정 Data
        }

        /// <summary>
        /// 야간작업 문진표
        /// </summary>
        void fn_Screen_Munjin_Night()
        {
            string strChar = "";
            string strDAT1 = "", strDAT2 = "", strDAT3 = "", strDAT4 = "", strDAT5 = "";
            long nJemsu1 = 0, nJemsu2 = 0, nJemsu3 = 0, nJemsu4 = 0, nJemsu5 = 0;
            string strPAN1 = "", strPAN2 = "", strPAN3 = "", strPAN4 = "", strPAN5 = "";            

            //야간작업 문진표를 읽음
            HIC_MUNJIN_NIGHT list = hicMunjinNightService.GetAllbyWrtNo(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                tabNightMunjin.Visible = false;
                return;
            }
            else
            {
                tabNightMunjin.Visible = true;
            }

            if (!list.ITEM1_DATA.IsNullOrEmpty())
            {
                //불면증지수
                if (!list.ITEM1_DATA.IsNullOrEmpty())
                {
                    tabNgtMun1.Visible = true;

                    strDAT1 = list.ITEM1_DATA;
                    nJemsu1 = list.ITEM1_JEMSU;
                    strPAN1 = list.ITEM1_PANJENG;
                    for (int i = 1; i <= 7; i++)
                    {
                        strChar = VB.Mid(strDAT1, i, 1);
                        SS1.ActiveSheet.Cells[i - 1, 1].Text = strChar;
                        if (string.Compare(strChar, "0") > 0)
                        {
                            SS1.ActiveSheet.Cells[i - 1, 2].Text = (strChar.To<long>() - 1).To<string>();
                            SS1.ActiveSheet.Cells[i - 1, 3].Text = hf.Munjin_Night_Value1(i, strChar);
                        }
                    }
                    lblSum1.Text = "총점 : " + nJemsu1 + " 점";
                    lblNgtSogen.Text = hf.Munjin_Night_Panjeng("1", strPAN1);
                }
            }

            //수면의질(2차)
            if (!list.ITEM2_DATA.IsNullOrEmpty())
            {
                tabNgtMun5.Visible = true;

                strDAT2 = list.ITEM2_DATA;
                nJemsu2 = list.ITEM2_JEMSU;
                strPAN2 = list.ITEM2_PANJENG;

                for (int i = 1; i <= 18; i++)
                {
                    strChar = VB.Pstr(strDAT2, ",", i);
                    SS5.ActiveSheet.Cells[i - 1, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS5.ActiveSheet.Cells[i - 1, 2].Text = (strChar.To<long>() - 1).To<string>();
                        SS5.ActiveSheet.Cells[i - 1, 3].Text = hf.Munjin_Night_Value2(i, strChar);
                    }
                }
            }

            //주간졸림(2차)
            if (!list.ITEM3_DATA.IsNullOrEmpty())
            {
                tabNgtMun4.Visible = true;

                strDAT3 = list.ITEM3_DATA;
                nJemsu3 = list.ITEM3_JEMSU;
                strPAN3 = list.ITEM3_PANJENG;

                for (int i = 1; i <= 8; i++)
                {
                    strChar = VB.Mid(strDAT3, i, 1);
                    SS4.ActiveSheet.Cells[i - 1, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS4.ActiveSheet.Cells[i - 1, 2].Text = (strChar.To<long>() - 1).To<string>();
                        SS4.ActiveSheet.Cells[i - 1, 3].Text = hf.Munjin_Night_Value3(i, strChar);
                    }
                }
            }

            //위장관질환(1차)
            if (!list.ITEM4_DATA.IsNullOrEmpty())
            {
                tabNgtMun2.Visible = true;

                strDAT4 = list.ITEM4_DATA;
                nJemsu4 = list.ITEM4_JEMSU;
                strPAN4 = list.ITEM4_PANJENG;

                for (int i = 1; i <= 6; i++)
                {
                    strChar = VB.Mid(strDAT4, i, 1);
                    SS2.ActiveSheet.Cells[i - 1, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS2.ActiveSheet.Cells[i - 1, 1].Text = (strChar.To<long>()).To<string>();
                        SS2.ActiveSheet.Cells[i - 1, 2].Text = hf.Munjin_Night_Value4(i, strChar);
                    }
                }
            }

            //유방암(여성) (1차)
            if (!list.ITEM5_DATA.IsNullOrEmpty())
            {
                tabNgtMun3.Visible = true;

                strDAT5 = list.ITEM5_DATA;
                nJemsu5 = list.ITEM5_JEMSU;
                strPAN5 = list.ITEM5_PANJENG;

                for (int i = 1; i <= 6; i++)
                {
                    strChar = VB.Mid(strDAT5, i, 1);
                    SS3.ActiveSheet.Cells[i - 1, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS3.ActiveSheet.Cells[i - 1, 1].Text = (strChar.To<long>()).To<string>();
                        SS3.ActiveSheet.Cells[i - 1, 2].Text = hf.Munjin_Night_Value5(i, strChar);
                    }
                }
            }
            
        }

        void fn_Screen_Spc_Display()
        {
            string strGjYear = "";
            string[] strGjjong = null;

            //안정공단 전송여부를 읽음
            FstrGbOHMS = hicResSpecialService.GetGbOhmsbyWrtNo(FnWrtNo);

            strGjYear = hicJepsuService.GetGjYearbyWrtNo(FnWrtNo);

            switch (FstrJong)
            { 
                case "16": strGjjong = new string[] { "11", "14", "16" }; break;
                case "17": strGjjong = new string[] { "12", "17" };            break;
                case "18": strGjjong = new string[] { "13", "18" };            break;
                case "19": strGjjong = new string[] { "14", "19" };            break;
                case "21": strGjjong = new string[] { "21" };                 break;
                case "22": strGjjong = new string[] { "22" };                 break;
                case "23": strGjjong = new string[] { "23" };                 break;
                case "24": strGjjong = new string[] { "24", "33" };            break;
                case "27": strGjjong = new string[] { "21", "27", "49"};       break;
                case "28": strGjjong = new string[] { "23", "28" };            break;
                case "29": strGjjong = new string[] { "22", "24", "29" };       break;
                case "33": strGjjong = new string[] { "24", "33" };            break;
                case "44":       
                case "45": strGjjong = new string[] { "41", "42", "44", "45" }; break;
                case "50": strGjjong = new string[] { "50", "11", "51" }; break;
                case "69": strGjjong = new string[] { "69" }; break;
                default : break;
            }

            FnWrtno1 = 0; FnWrtno2 = 0;

            if (!strGjjong.IsNullOrEmpty())
            {
                //검사결과 1,2차를 모두 표시하기 위하여 특수1차,2차를 읽음
                List<HIC_JEPSU> lstHJP =  hicJepsuService.GetListByPanoGjYearJepDateGjJonIN(FnPano, FstrJepDate, strGjYear, strGjjong, FstrJong);

                if (!lstHJP.IsNullOrEmpty() && lstHJP.Count > 0)
                {
                    for (int i = 0; i < lstHJP.Count; i++)
                    {
                        if (lstHJP[i].GJCHASU == "2")
                        {
                            FnWrtno1 = FnWrtNo;
                            FnWrtno2 = lstHJP[i].WRTNO;
                            FstrJepDate2 = lstHJP[i].JEPDATE;
                        }
                        else
                        {
                            FnWrtno1 = lstHJP[i].WRTNO;
                            FnWrtno2 = FnWrtNo;
                            FnWrtNo = FnWrtno1;
                            //FstrJepDate2 = FstrJepDate;
                            FstrJepDate2 = "";
                            FstrJepDate = lstHJP[i].JEPDATE;
                        }

                        if (FnWrtno1 > 0) { break; }
                    }
                }
            }
            else
            {
                //2차 접수일자를 찾음
                FstrJepDate2 = hicJepsuService.GetJepDAtebyPaNoJepDateGjYear(FnPano, FstrJepDate, strGjYear);
            }
            
            fn_Screen_Display_SpcPanjeng();
            fn_Screen_Clear_SpcSUB();

            eSpdDClick(ssPan, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));

        }

        void fn_ComboSet(ComboBox argCbo, string argGubun)
        {
            cboUpmu.Items.Add("");
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun(argGubun);
            argCbo.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
        }

        void fn_ComboOrganSayuSet(ComboBox argCbo)
        {
            List<HIC_ORGANCODE> list = hicOrgancodeService.GetSayuCodeNamebyGubunCode("2", VB.Left(cboOrgan.Text, 2));
            argCbo.SetItems(list, "SAYUNAME", "SAYUCODE", "", "", AddComboBoxPosition.Top);
        }
            
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnJochi2)
            {
                FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
                FrmHcPanJochiHelp.ShowDialog(this);
                FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
            }
            else if (sender == btnReExam)
            {   
                FrmHcPanPanjengHelp = new frmHcPanPanjengHelp("53"); //특수2차추가검사
                FrmHcPanPanjengHelp.rSetGstrValue += new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
                FrmHcPanPanjengHelp.ShowDialog();
                FrmHcPanPanjengHelp.rSetGstrValue -= new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
            }
            else if (sender == btnSpcAll)
            {
                ssPan.ActiveSheet.Cells[0, 0, ssPan.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "True";
            }
            else if (sender == btnSahu)
            {
                FrmHcPanSpcSahuCode = new frmHcPanSpcSahuCode();
                FrmHcPanSpcSahuCode.rSetGstrValue += new frmHcPanSpcSahuCode.SetGstrValue(Sahu_value);
                FrmHcPanSpcSahuCode.ShowDialog();
                FrmHcPanSpcSahuCode.rSetGstrValue -= new frmHcPanSpcSahuCode.SetGstrValue(Sahu_value);
            }
            else if (sender == btnSahuSangdam)
            {
                FrmHcPanOpinionCounselReg = new frmHcPanOpinionCounselReg(FnWrtNo);
                FrmHcPanOpinionCounselReg.ShowDialog(this);
                
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear_SpcSUB();
                fn_Screen_Display_SpcPanjeng();
                ssPan.Focus();
            }
            else if (sender == btnSave)
            {
                FstrOK = "Y";
                FstrSaveGbn = "저장";

                fn_DB_Update_SpcPanjeng(); //특수판정
                if (FstrOK == "Y")
                {
                    string strOK = fn_Panjeng_End_Check(); //판정
                    fn_Screen_Display_SpcPanjeng();

                    if (strOK == "OK")
                    {
                        rSaveEventClosed("SPC");
                    }
                }
            }
            else if (sender == btnPft)
            {
                FrmHcActPFTMunjin = new frmHcActPFTMunjin("HCPan", "SANG", FnWrtNo, FstrPtNo, FnWrtNo);
                FrmHcActPFTMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcActPFTMunjin.ShowDialog(this);
            }
            else if (sender == btnCap)
            {
                FrmHcPanPanDrExamResultInput = new frmHcPanPanDrExamResultInput(FnWrtNo);
                FrmHcPanPanDrExamResultInput.ShowDialog(this);
            }
            else if (sender == btnSogen2)
            {
                string strRetValue = "";

                strRetValue = VB.Left(cboPanjeng2.Text, 1);

                FrmHcCodeHelp = new frmHcCodeHelp("54", "", strRetValue); //특수판정소견코드
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtSogen2.Text = FstrCode.Trim() + "." + FstrName.Trim();

                    HIC_SPC_SCODE item = hicSpcScodeService.GetItemByMCodeSogen(FstrCode, FstrMCode);

                    if (!item.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboPanjeng2.Items.Count; i++)
                        {
                            if (item.PANJENG.To<string>("").Trim() == VB.Pstr(cboPanjeng2.Items[i].ToString(), ".", 1).Trim())
                            {
                                cboPanjeng2.SelectedIndex = i;
                                bCboPanChange = false;
                                break;
                            }
                        }

                        txtSogen2.Text = item.CODE.To<string>("").Trim() + "." + item.NAME.To<string>("").Trim();
                        txtSogenRemark.Text = item.SOGENREMARK.To<string>("").Trim();
                        txtJochiRemark.Text = item.JOCHIREMARK.To<string>("").Trim();
                        for (int i = 0; i < cboUpmu.Items.Count; i++)
                        {
                            if (VB.Pstr(cboUpmu.Items[i].ToString(), ".", 1) == item.WORKYN.To<string>("").Trim())
                            {
                                cboUpmu.SelectedIndex = i;
                                break;
                            }
                        }

                        if (!item.SAHUCODE.IsNullOrEmpty())
                        {
                            txtSahu.Text = item.SAHUCODE.To<string>("");
                            txtSahu.Text += "." + hm.Sahu_Names_Display(txtSahu.Text); //사후관리
                        }

                        bCboPanChange = true;
                    }

                    SendKeys.Send("{TAB}");
                    if (VB.Pstr(txtSogen2.Text, ".", 1) == "1101" || VB.Pstr(txtSogen2.Text, ".", 1) == "1102" ||
                        VB.Pstr(txtSogen2.Text, ".", 1) == "1103" || VB.Pstr(txtSogen2.Text, ".", 1) == "1104" ||
                        VB.Pstr(txtSogen2.Text, ".", 1) == "1105" || VB.Pstr(txtSogen2.Text, ".", 1) == "1106")
                    {
                        //txtJochi.Text = "C00C" + "." + hb.READ_HIC_CODE("14", txtJochi.Text);   // 조치;
                    }
                    else
                    {
                        fn_Panjeng_Auto_Jochi_SET();
                    }
                }
                else
                {
                    txtSogen2.Text = "";
                }
            }
        }

        private void Sahu_value(string argValue, string argRemark)
        {
            txtSahu.Text = argValue + "." + argRemark;
        }

        void frmHcPanJochiHelp_SpreadDoubleClick(string strRemark)
        {
            txtJochiRemark.Text += strRemark;
        }

        //private void Code_value(HIC_CODE item)
        private void Code_value(string strCode, string strName)
        {
            //CodeHelpItem = item;
            FstrCode = strCode;
            FstrName = strName;
        }

        private void ReExam_value(string argValue, string argName)
        {
            if (!argValue.IsNullOrEmpty())
            {
                //txtReExam.Text = argValue + "." + argName;
                txtReExam.Text = argValue;
                txtReExam.Text = VB.Left(txtReExam.Text, txtReExam.Text.Length - 1);
                lblReExamName.Text = "☞추가검사: " + argName;
            }
            else
            {
                txtReExam.Text = "";
                lblReExamName.Text = "";
            }
        }

        void fn_DB_Update_SpcPanjeng()
        {
            string strPanjeng = "";
            string strROWID = "";
            string strMsg = "";
            string strMCode = "";
            string strPyoJanggi = "";
            long nLicense = 0;
            int result = 0;

            if (ssPan.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("물질별 판정 내역이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //의사면허번호 조회
            nLicense = clsHcVariable.GnHicLicense;
            if (clsHcVariable.GbHicAdminSabun == true)
            {
                nLicense = txtPanDrNo2.Text.To<long>();
            }

            //퇴사일 이후 판정금지
            if (!clsHcVariable.GstrReDay.IsNullOrEmpty())
            {
                if (string.Compare(dtpPanDate.Text, clsHcVariable.GstrReDay) > 0)
                {
                    MessageBox.Show("판정일자가 " + clsHcVariable.GstrReDay + "일보다 큼" + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrOK = "N";
                    return;
                }
            }

            strMsg = "";
            //접수마스타의 상태를 변경
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoPartNot9(FnWrtNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strMsg += list[i].EXCODE + ":" + list[i].HNAME + "\r\n";
                }
                MessageBox.Show("결과값 누락항목이 있습니다. 검진 담당자에게 연락요망" + "\r\n" + "\r\n" + strMsg, "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                FstrOK = "N";
                return;
            }

            //자료에 오류가 있는지 점검
            strPanjeng = lblDrName2.Text;
            if (cboPanjeng2.Text.IsNullOrEmpty())
            {
                MessageBox.Show("판정이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (hf.GetLength(txtSogenRemark.Text) > 160)
            {
                MessageBox.Show("소견내용이 너무 깁니다.(160자까지가능)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lblDrName2.Text.IsNullOrEmpty())
            {
                lblDrName2.Text = hb.READ_License_DrName(txtPanDrNo2.Text.To<long>());
                if (lblDrName2.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사가 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (FstrSaveGbn == "저장" && FstrGbOHMS == "Y")
            {
                if (string.Compare(strPanjeng, "3") >= 0 && string.Compare(strPanjeng, "6") <= 0)
                {
                    if (strPanjeng == "5" || strPanjeng == "6")
                    {
                        if (cboUpmu.Text.IsNullOrEmpty())
                        {
                            MessageBox.Show("업무적합성 코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }

            if (string.Compare(FstrJepDate, dtpPanDate.Text) > 0)
            {
                MessageBox.Show("판정일자가 검진일(접수일)보다 빠릅니다." + "\r\n" + "날짜를 수정하여 주십시오", "확인창", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrOK = "N";
                return;
            }

            //소음 R판정 대상자인데 다른 판정이면 메세지 표시
            if (fn_Soum_R_Check() == true)
            {
                if (VB.Left(cboPanjeng2.Text, 1) == "1")
                {
                    if (MessageBox.Show("소음 R판정 대상자입니다." + "\r\n" + "소음을 다른 판정으로 저장을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        FstrOK = "N";
                        return;
                    }
                }
            }

            //접수마스타의 상태를 변경
            if (hicResultExCodeService.GetCountbyWrtNoNotPart9(FnWrtNo) > 0)
            {
                MessageBox.Show("결과값 누락항목이 있습니다. 검진 담당자에게 연락요망", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (FstrMCode == "L01" || FstrMCode == "L02" || FstrMCode == "L03")
            {
                if (VB.Left(cboPanjeng2.Text, 1) == "7")
                {
                    if (txtReExam.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("추가검사 누락!! 확인요망", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //DB에 자료를 UPDATE
            for (int i = 0; i < ssPan.ActiveSheet.RowCount; i++)
            {
                if (ssPan.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strROWID = ssPan.ActiveSheet.Cells[i, 12].Text;
                    strMCode = ssPan.ActiveSheet.Cells[i, 9].Text;
                    strPyoJanggi = "9";
                    
                    HIC_SPC_PANJENG item = new HIC_SPC_PANJENG();

                    item.PANJENGDATE = dtpPanDate.Value.ToString("yyyy-MM-dd");
                    item.PANJENGDRNO = nLicense;
                    //item.PANJENGDATE = null;
                    item.PANJENG = VB.Left(cboPanjeng2.Text, 1);
                    item.SOGENCODE = VB.Pstr(txtSogen2.Text, ".", 1);
                    item.JOCHICODE = "";
                    //item.JOCHICODE = txtJochi.Text;
                    item.WORKYN = VB.Left(cboUpmu.Text, 3);
                    item.SAHUCODE = VB.Pstr(txtSahu.Text, ".", 1);
                    item.SAHUREMARK = txtSahuRemark.Text.Replace("'", "`");
                    item.UCODE = FstrYuhe;
                    item.SOGENREMARK = txtSogenRemark.Text.Replace("'", "`");
                    item.JOCHIREMARK = txtJochiRemark.Text.Replace("'", "`");
                    item.REEXAM = txtReExam.Text;
                    item.ORCODE = VB.Left(cboOrgan.Text, 2);
                    item.ORSAYUCODE = VB.Left(cboOrganSayu.Text, 2);
                    item.PYOJANGGI = strPyoJanggi;
                    item.ENTSABUN = clsType.User.IdNumber.To<long>();
                    item.WRTNO = FnWrtNo;
                    item.MCODE = FstrMCode;
                    item.PYOJANGGi = strPyoJanggi;
                    item.RID = strROWID;

                    //1차판정 R을 다른 판정으로 고친 경우
                    if (FstrSpcTable == "H" && VB.Left(cboPanjeng2.Text, 1) != "7")
                    {
                        result = hicSpcPanjengService.UpdateAllbyWrtNoMCodePyojanggi(FstrSaveGbn, item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("특수 판정결과 DB에 저장시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }

                        //1차 R판정을 삭제함
                        result = hicSpcPanhisService.DeleteByRowId(strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("특수 판정결과 삭제시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //1차판정 다른 판정을 R판정으로 고친 경우
                    else if (!strROWID.IsNullOrEmpty() && FstrJepDate2.IsNullOrEmpty() && FstrSpcTable == "P" && VB.Left(cboPanjeng2.Text, 1) == "7")
                    {
                        result = hicSpcPanjengService.UpdateAllbyWrtNoMCodePyojanggi1(FstrSaveGbn, item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("특수 판정결과 DB에 저장시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //1차 R판정을 HISTORY에 복사
                        if (hicSpcPanhisService.GetCountbyWrtNoMCodePyoJanggi(FnWrtNo, strMCode, strPyoJanggi) == 0)
                        {
                            result = hicSpcPanhisService.Insert(strROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("특수 판정결과 저장시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {   
                        COMHPC item2 = new COMHPC();

                        if (FstrSaveGbn == "저장")
                        {
                            item2.PANJENGDATE = dtpPanDate.Text;
                            item2.PANJENGDRNO = nLicense;
                        }
                        else
                        {
                            item2.PANJENGDATE = null;
                            item2.PANJENGDRNO = 0;
                        }
                        item2.PANJENG = VB.Left(cboPanjeng2.Text, 1);
                        item2.SOGENCODE = VB.Pstr(txtSogen2.Text, ".", 1);
                        item2.JOCHICODE = "";
                        //item2.JOCHICODE = txtJochi.Text;
                        item2.WORKYN = VB.Left(cboUpmu.Text, 3);
                        item2.SAHUCODE = VB.Pstr(txtSahu.Text, ".", 1);
                        item2.SAHUREMARK = txtSahuRemark.Text.Replace("'", "`");
                        item2.UCODE = FstrYuhe;
                        item2.SOGENREMARK = txtSogenRemark.Text.Replace("'", "`");
                        item2.JOCHIREMARK = txtJochiRemark.Text.Replace("'", "`");
                        item2.REEXAM = txtReExam.Text;
                        item2.ORCODE = VB.Left(cboOrgan.Text, 2);
                        item2.ORSAYUCODE = VB.Left(cboOrganSayu.Text, 2);
                        item2.PYOJANGGI = strPyoJanggi;
                        item2.GJCHASU = "1"; //1차판정
                        item2.ENTSABUN = clsType.User.IdNumber;
                        item2.ROWID = strROWID;

                        result = comHpcLibBService.UpdateHicSpcPanjengPanHis(FstrSpcTable, item2);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("특수 판정결과 저장시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            fn_Screen_Clear_SpcSUB();
            ssPan.Focus();
        }

        /// <summary>
        /// 소음 R대상자 여부 점검
        /// </summary>
        /// <returns></returns>
        bool fn_Soum_R_Check()
        {
            bool rtnVal = false;
            string strM1 = "";
            string strM2 = "";
            string strR1 = "";
            string strR2 = "";
            string strSel = "";
            string strCode = "";
            string strResult = "";

            for (int i = 0; i < ssPan.ActiveSheet.RowCount; i++)
            {
                strSel = ssPan.ActiveSheet.Cells[i, 0].Text;
                strCode = ssPan.ActiveSheet.Cells[i, 9].Text;
                if (strSel == "True")
                {
                    switch (strCode)
                    {
                        case "L05":
                        case "L07":
                        case "L09":
                            strM1 = "Y";
                            break;
                        case "L04":
                        case "L06":
                        case "L08":
                            strM2 = "Y";
                            break;
                        default:
                            break;
                    }
                }
            }

            //소음 판정이 아니면
            if (strM1.IsNullOrEmpty() && strM2.IsNullOrEmpty())
            {
                rtnVal = false;
                return rtnVal;
            }

            //소음 R판정 대상 결과값
            //2000Hz(30db),3000Hz(40db),4000Hz(40db)이상일 경우
            strR1 = "";
            strR2 = "";
            for (int i = 0; i < FspdNm.ActiveSheet.RowCount; i++)
            {
                strCode = FspdNm.ActiveSheet.Cells[i, 7].Text;
                strResult = FspdNm.ActiveSheet.Cells[i, 1].Text;
                if (strM1 == "Y")
                {
                    if (strCode == "TH23" && strResult.To<int>() >= 30) { strR1 = "Y"; }
                    if (strCode == "TH24" && strResult.To<int>() >= 40) { strR1 = "Y"; }
                    if (strCode == "TH25" && strResult.To<int>() >= 40) { strR1 = "Y"; }
                }
                if (strM2 == "Y")
                {
                    if (strCode == "TH13" && strResult.To<int>() >= 30) { strR2 = "Y"; }
                    if (strCode == "TH14" && strResult.To<int>() >= 40) { strR2 = "Y"; }
                    if (strCode == "TH15" && strResult.To<int>() >= 40) { strR2 = "Y"; }
                }
            }

            if (strR1 == "Y" || strR2 == "Y")
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssPan)
            {
                string strCode = "";
                string strPan = "";
                string strUCode = "";
                bool bRePanjeng = false;
                string sMsg = "";
                int result = 0;

                if (e.Row >= ssPan.ActiveSheet.NonEmptyRowCount) return;

                fn_Screen_Clear_SpcSUB();

                //이미 판정을 하였으면 다른 판정의가 변경이 불가능함(본인만 가능함)
                btnSave.Enabled = true;    //저장
                bRePanjeng = hm.RePanjeng_WRTNO_Check(FnWrtNo);

                if (clsHcVariable.GnHicLicense == 0)
                {
                    if (clsHcVariable.GbHicAdminSabun == true)
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                }
                else
                {
                    //청구자 재판정을 위해 추가함
                    if (bRePanjeng == true)
                    {
                        btnSave.Enabled = true;
                    }
                    else if (clsHcVariable.GbHicAdminSabun == true)
                    {
                        btnSave.Enabled = true;
                    }
                }

                //2차재검 접수를 하였으면 수정이 불가능함
                if (!FstrJepDate2.IsNullOrEmpty())
                {
                    if (bRePanjeng == false)
                    {
                        if (clsHcVariable.GbHicAdminSabun == false)
                        {
                            btnSave.Enabled = false;
                        }
                    }
                }

                if (e.RowHeader == false)
                {
                    //for (int k = 0; k < ssPan.ActiveSheet.NonEmptyRowCount; k++)
                    //{
                    //    for (int i = 0; i < ssPan.ActiveSheet.ColumnCount; i++)
                    //    {
                    //        if (i != 2)
                    //        {
                    //            ssPan.ActiveSheet.Cells[k, i].ForeColor = Color.FromArgb(0, 0, 0);
                    //            ssPan.ActiveSheet.Cells[k, i].BackColor = Color.FromArgb(255, 255, 255);
                    //        }
                    //    }
                    //}
                    
                    //선택된 물질 색깔 표시
                    //for (int i = 0; i < ssPan.ActiveSheet.ColumnCount; i++)
                    //{
                    //    if (i != 2)
                    //    {
                    //        ssPan.ActiveSheet.Cells[e.Row, i].ForeColor = Color.FromArgb(0, 0, 0);
                    //        ssPan.ActiveSheet.Cells[e.Row, i].BackColor = Color.FromArgb(255, 255, 179);
                    //    }
                    //}
                }

                FnRow = e.Row;

                FstrPROWID = ssPan.ActiveSheet.Cells[e.Row, 12].Text.Trim();
                FstrSpcTable = ssPan.ActiveSheet.Cells[e.Row, 13].Text.Trim();
                strPan = ssPan.ActiveSheet.Cells[e.Row, 2].Text.Trim();

                //소견명 더블클릭
                if (e.Column == 7)
                {
                    if (btnSave.Enabled == true)
                    {
                        sMsg = "물질명 : " + ssPan.ActiveSheet.Cells[e.Row, 1].Text;
                        sMsg += "판정용 취급물질을 추가 하시겠습니까?";
                        if (MessageBox.Show(sMsg, "추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }

                        strCode = ssPan.ActiveSheet.Cells[e.Row, 10].Text;
                        strUCode = hicMcodeService.GetUCodebyCode(strCode);
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicSpcPanjengService.InsertPanjeng(FnWrtNo, strCode, strUCode);

                        if (result < 0)
                        {
                            MessageBox.Show("판정결과 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        fn_Screen_Clear_SpcSUB();
                        fn_Screen_Display_SpcPanjeng();
                        return;
                    }
                }
                else if (e.Column == 8)
                {
                    if (btnSave.Enabled == true)
                    {
                        sMsg = "물질명 : " + ssPan.ActiveSheet.Cells[e.Row, 1].Text;
                        sMsg += "판정용 취급물질을 삭제 하시겠습니까?";
                        if (MessageBox.Show(sMsg, "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }

                        strCode = ssPan.ActiveSheet.Cells[e.Row, 9].Text;

                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicSpcPanjengService.UpdateDelDatebyRowId(FstrPROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("판정결과 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);

                        fn_Screen_Clear_SpcSUB();
                        fn_Screen_Display_SpcPanjeng();
                        hm.UPDATE_SPC_PanHis(FnWrtNo);
                        return;
                    }
                }

                //더블클릭시 기존에 선택한것을 지움
                ssPan.ActiveSheet.Cells[0, 0, ssPan.ActiveSheet.RowCount - 1, 0].Text = "";

                ssPan.ActiveSheet.Cells[e.Row, 0].Text = "True";
                FstrPROWID = ssPan.ActiveSheet.Cells[e.Row, 12].Text.Trim();
                FstrSpcTable = ssPan.ActiveSheet.Cells[e.Row, 13].Text.Trim();

                //자료를 읽어 Display
                HIC_SPC_PANJENG list = comHpcLibBService.GetItembyRowId(FstrPROWID, FstrSpcTable);

                if (!list.IsNullOrEmpty())
                {
                    strCode = list.PANJENG;
                    cboPanjeng2.SelectedIndex = -1;
                    FstrOldSpcPan = strCode;    //판정 변경여부를 알기 위해 변수에 보관
                    if (!strCode.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboPanjeng2.Items.Count; i++)
                        {
                            if (VB.Left(cboPanjeng2.Items[i].To<string>(), 1) == strCode)
                            {
                                cboPanjeng2.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    FstrMCode = list.MCODE;
                    lblSpcName.Text = hb.READ_MCode_Name(FstrMCode);
                    txtSogen2.Text = list.SOGENCODE + "." + hb.READ_SpcSCode_Name(list.SOGENCODE);

                    //txtJochi.Text = list.JOCHICODE;
                    txtSogenRemark.Text = list.SOGENREMARK;
                    txtJochiRemark.Text = list.JOCHIREMARK;
                    for (int i = 0; i < cboUpmu.Items.Count; i++)
                    {
                        if (VB.Pstr(cboUpmu.Items[i].ToString(), ".", 1) == list.WORKYN)
                        {
                            cboUpmu.SelectedIndex = i;
                            break;
                        }
                    }

                    if (!list.SAHUCODE.IsNullOrEmpty())
                    {
                        txtSahu.Text = list.SAHUCODE + "." + hm.Sahu_Names_Display(list.SAHUCODE);
                    }
                    txtSahuRemark.Text = list.SAHUREMARK;
                    FstrYuhe = list.UCODE;
                    txtReExam.Text = list.REEXAM;
                    for (int i = 0; i < cboOrgan.Items.Count; i++)
                    {
                        if (VB.Pstr(cboOrgan.Text, ".", 1) == list.ORCODE)
                        {
                            cboOrgan.SelectedIndex = i;
                        }
                    }
                    for (int i = 0; i < cboOrganSayu.Items.Count; i++)
                    {
                        if (VB.Pstr(cboOrganSayu.Text, ".", 1) == list.ORSAYUCODE)
                        {
                            cboOrganSayu.SelectedIndex = i;
                        }
                    }

                    //유해물질 코드를 Display
                    if (FstrYuhe.IsNullOrEmpty())
                    {
                        FstrYuhe = hicMcodeService.GetUCodebyCode(FstrMCode);
                    }
                    //lblSogenName2.Text = READ_SpcSCode_Name(Trim(txtSogen2.Text)); //질병소견
                    //lblJochiName.Text = READ_HIC_CODE("14", Trim(txtJochi.Text));  //조치코드
                    //lblSahuName.Text = Sahu_Names_Display(Trim(txtSahu.Text));     //사후관리

                    if (list.PANJENGDATE.IsNullOrEmpty())
                    {
                        dtpPanDate.Text = clsPublic.GstrSysDate;
                        txtPanDrNo2.Text = clsHcVariable.GnHicLicense.To<string>();
                    }
                    else
                    {
                        dtpPanDate.Text = list.PANJENGDATE.To<string>();
                        txtPanDrNo2.Text = list.PANJENGDRNO.To<string>();
                    }
                    if (txtPanDrNo2.Text.IsNullOrEmpty())
                    {
                        txtPanDrNo2.Text = clsHcVariable.GnHicLicense.To<string>();
                    }

                    lblDrName2.Text = hb.READ_License_DrName(txtPanDrNo2.Text.To<long>());

                    if (bRePanjeng == false)
                    {
                        if (btnSave.Enabled == true)
                        {
                            if (list.PANJENGDRNO > 0)
                            {
                                if (list.PANJENGDRNO != clsHcVariable.GnHicLicense)
                                {
                                    btnSave.Enabled = false;
                                }
                                //btnYSave.Enabled = true;
                            }
                        }
                    }

                    if ((string.Compare(strPan, "R") >= 0 || strPan == "C2" || strPan == "") && clsHcVariable.GnHicLicense > 0)
                    {
                        btnSave.Enabled = true;
                    }

                }

                Application.DoEvents();
            }
        }

        /// <summary>
        /// 야간작업 점수를 표시함
        /// </summary>
        /// <param name="argWrtNo"></param>
        void fn_Display_NightMunjin(long argWrtNo)
        {
            long nPano = 0;
            string strJepDate = "";
            string strGjYear = "";
            string strGjChasu = "";
            long nJemsu1 = 0;
            long nJemsu2 = 0;
            long nJemsu3 = 0;
            string strPAN1 = "";
            string strPAN2 = "";
            string strMsg = "";

            //접수마스타를 읽어 건진년도,차수,건진번호를 찾음
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(argWrtNo);

            if (list.IsNullOrEmpty())
            {
                lblNightMunjinView.Text = "";
                return;
            }

            nPano = list.PANO;
            strJepDate = list.JEPDATE;
            strGjYear = list.GJYEAR;
            strGjChasu = list.GJCHASU;
            nJemsu1 = 0;
            nJemsu2 = 0;
            nJemsu3 = 0;
            strPAN1 = "";
            strPAN2 = "";

            //금년도 야간작업 문진표 점수를 읽음
            List<HIC_JEPSU_MUNJIN_NIGHT> list2 = hicJepsuMunjinNightService.GetItembyPaNoGjYear(nPano, strGjYear);

            if (list2.Count == 0)
            {
                lblNightMunjinView.Text = "";
                return;
            }

            #region 기존 문진 점수 표시
            //for (int i = 0; i < list2.Count; i++)
            //{
            //    if (strPAN1 == "" && !list2[i].ITEM1_PANJENG.IsNullOrEmpty())
            //    {
            //        nJemsu1 = list2[i].ITEM1_JEMSU;
            //        strPAN1 = "OK";
            //    }

            //    if (strPAN2 == "" && !list2[i].ITEM1_PANJENG.IsNullOrEmpty())
            //    {
            //        nJemsu2 = list2[i].ITEM2_JEMSU;
            //        nJemsu3 = list2[i].ITEM3_JEMSU;
            //        strPAN2 = "OK";
            //    }

            //    if (strPAN1 != "" && strPAN2 != "") { break; }
            //}

            //if (strPAN1 != "" && strPAN2 != "")
            //{
            //    strMsg = "불면증:" + nJemsu1 + ",수면질:" + nJemsu2 + ",주간졸림:" + nJemsu3;
            //}
            //else if (strPAN1 != "")
            //{
            //    strMsg = "불면증:" + nJemsu1 + "(2차안함)";
            //}
            //else
            //{
            //    strMsg = "수면질:" + nJemsu2 + ",주간졸림:" + nJemsu3 + "(1차안함)";
            //}
            #endregion

            if (!list2[0].ITEM1_PANJENG.IsNullOrEmpty())
            {
                strMsg += "불면증:" + list2[0].ITEM1_JEMSU + " ";
            }

            if (!list2[0].ITEM4_PANJENG.IsNullOrEmpty())
            {
                strMsg += "위장관:";
                strMsg += list2[0].ITEM4_PANJENG == "1" ? "정상" : "비정상" + " ";
            }

            if (!list2[0].ITEM5_PANJENG.IsNullOrEmpty())
            {
                strMsg += "유방암:";
                strMsg += list2[0].ITEM5_PANJENG == "1" ? "정상" : "비정상" + " ";
            }

            if (!list2[0].ITEM2_PANJENG.IsNullOrEmpty())
            {
                strMsg += "수면질:";
                strMsg += list2[0].ITEM2_JEMSU + " ";
            }

            if (!list2[0].ITEM3_PANJENG.IsNullOrEmpty())
            {
                strMsg += "주간졸림:";
                strMsg += list2[0].ITEM3_JEMSU + " ";
            }

            lblNightMunjinView.Text = strMsg;
        }

        void fn_Screen_Clear_SpcSUB()
        {
            cboPanjeng2.SelectedIndex = -1;
            lblSpcName.Text = "";
            txtSogenRemark.Text = "";
            txtJochiRemark.Text = "";
            txtSahu.Text = "";
            dtpPanDate.Checked = false;
            txtReExam.Text = "";
            //lblNightMunjinView.Text = "";
            txtPanDrNo2.Text = "";
            lblDrName2.Text = "";
            txtSahuRemark.Text = "";
            txtSogen2.Text = ""; ;
            lblReExamName.Text = "";
            cboOrgan.Text = "";
            cboOrganSayu.Text = "";
            cboUpmu.SelectedIndex = -1;
            btnSave.Enabled = false;   //저장버튼
        }

        void fn_Screen_Clear()
        {
            clsHcVariable.GnPanB_Etc = 0;
            clsHcVariable.Gstr_PanB_Etc = "";
            clsHcType.TFA.Sogen = "";
            
            FstrSex = "";
            FnPano = 0;
            FnWrtno1 = 0;           FnWrtno2 = 0;
            FstrSaveGbn = "";       FstrJumin = "";
            FstrGbOHMS = "";        
            FstrJepDate2 = "";          FstrSpcTable = "";
            FstrGjChasu = "";

            dtpPanDate.Checked = false;
            txtSogenRemark.Text = "";

            cboUpmu.SelectedIndex = -1;
            txtSahu.Text = "";
            txtSahuRemark.Text = "";
            txtReExam.Text = "";
            cboOrgan.SelectedIndex = -1;
            cboOrganSayu.SelectedIndex = -1;
            cboPanjeng2.SelectedIndex = -1;
            txtSogen2.Text = "";
            txtSogenRemark.Text = "";
            txtJochiRemark.Text = "";
            lblReExamName.Text = "";
            lblDrName2.Text = "";
            txtPanDrNo2.Text = "";
            lblNightMunjinView.Text = "";

            
        }

        void fn_Screen_Display_SpcPanjeng()
        {
            int nREAD = 0;
            int nRow = 0;
            string strUCodes = "";
            string strOK = "";
            string strMCode = "";
            List<string> strList = new List<string>();
            string strSogenCode = "";
            string strSogenRemark = "";
            string strPyoJangi = "";
            string[] strHisROWID = new string[50];
            long nHisCnt = 0;
            long nHisV01 = 0;
            string strRowId = "";
            string strPanRowId = "";
            int result = 0;

            //접수된 물질코드 읽음
            strUCodes = hicJepsuService.GetUcodesbyWrtNo(FnWrtNo);

            if (VB.InStr(strUCodes, ",") > 0)
            {
                strUCodes = VB.Left(strUCodes, strUCodes.Length - 1);
            }

            //특수문진표에 판정물질 업데이트
            if (!strUCodes.IsNullOrEmpty())
            {
                clsDB.setBeginTran(clsDB.DbCon);

                strRowId = hicResSpecialService.GetRowIdbyWrtNo(FnWrtNo);

                if (!strRowId.IsNullOrEmpty())
                {
                    result = hicResSpecialService.UpdateUcodesbyRowId(strRowId, strUCodes);

                    if (result < 0)
                    {
                        MessageBox.Show("특수검진 문진 및 판정결과 판정의사 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }

            //판정물질이 한개도 없을경우 특수판정의 물질을 삭제(단, 판정이 안된것만)
            if (FstrSelTab != "SPC" && FstrSelTab != "REC")
            {
                if (strUCodes.IsNullOrEmpty())
                {
                    if (hicSpcPanjengService.GetCountbyWrtNo(FnWrtNo) > 0)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicSpcPanjengService.UpdateDelDatebyWrtNo(FnWrtNo);

                        if (result < 0)
                        {
                            MessageBox.Show("특수검진 문진 및 판정결과 삭제일자 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }
                else
                {
                    //판정테이블 물질코드와 비교후 제외된것은 삭제 (단, 판정이 안된것만)
                    List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GetMCodebyWrtNo(FnWrtNo);

                    nREAD = list2.Count;
                    if (nREAD > 0)
                    {
                        strOK = "";
                        for (int i = 0; i < nREAD; i++)
                        {
                            strPanRowId = list2[i].RID;
                            if (list2[i].MCODE == VB.Pstr(strUCodes, ",", i))
                            {
                                strOK = "OK";
                                break;
                            }
                        }

                        if (strOK.IsNullOrEmpty())
                        {
                            clsDB.setBeginTran(clsDB.DbCon);
                            result = hicSpcPanjengService.UpdateDelDatebyRowId(strPanRowId);

                            if (result < 0)
                            {
                                MessageBox.Show("특수검진 문진 및 판정결과 삭제일자 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }
            }
            

            //판정항목(취급물질)을 Display
            HIC_SPC_PANJENG list = hicSpcPanjengService.GetAllbyWrtNo(FnWrtNo);
           
            if (!list.IsNullOrEmpty())
            {   
                strMCode = list.MCODE;             
            }

            //HIC_SPC_PANHIS에 보관된 판정 ROWID를 읽음

            nHisCnt = 0;
            nHisV01 = 0;
            for (int i = 0; i < 50; i++)
            {
                strHisROWID[i] = "";
            }

            //2차재검 접수가 되었으면
            if (!FstrJepDate2.IsNullOrEmpty() && FstrGjChasu != "2")
            {
                List<HIC_SPC_PANHIS> list3 = hicSpcPanhisService.GetMCodeRowIdbyWrtNo(FnWrtNo);

                for (int i = 0; i < list3.Count; i++)
                {
                    nHisCnt += 1;
                    strHisROWID[nHisCnt] = list3[i].RID;
                    //유해인자 야간작업 건수를 누적함
                    strMCode = list3[i].MCODE;
                    if (strMCode == "V01" || strMCode == "V02")
                    {
                        nHisV01 += 1;
                    }
                }
            }

            sp.Spread_All_Clear(ssPan);

            ssPan_Sheet1.Rows[-1].Height = 24;
            //판정항목(취급물질)을 Display
            List<HIC_SPC_PANJENG> list4 = hicSpcPanjengService.GetAllbyWrtNoList(FnWrtNo);

            nREAD = list4.Count;
            ssPan.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strMCode = list4[i].MCODE;
                strPyoJangi = "9";          //기타

                //야간작업만 표적장기 구분코드를 읽음
                if (strMCode == "V01" || strMCode == "V02")
                {
                    strSogenCode = list4[i].SOGENCODE.To<string>("").Trim();
                    strSogenRemark = list4[i].SOGENREMARK.To<string>("").Trim();
                    if (!strSogenCode.IsNullOrEmpty())
                    {
                        strPyoJangi = hm.GET_Night_OrganTarget(strSogenCode, strSogenRemark);
                    }
                    else
                    {
                        strPyoJangi = "";
                    }
                }

                strOK = "OK";

                //2차에 야간작업이 있으면 야간작업 정상은 2차 판정임으로 표시 안함
                if (nHisV01 > 0 && (strMCode == "V01" || strMCode == "V02") && strSogenCode == "001")
                {
                    strOK = "";
                }

                if (strOK == "OK")
                {
                    nRow += 1;
                    if (nRow > ssPan.ActiveSheet.RowCount)
                    {
                        ssPan.ActiveSheet.RowCount = nRow;
                    }
                    ssPan.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    ssPan.ActiveSheet.Cells[nRow - 1, 1].Text = " " + hb.READ_MCode_Name(list4[i].MCODE);
                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                    switch (list4[i].PANJENG)
                    {
                        case "1":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "A";
                            break;
                        case "2":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "B";
                            break;
                        case "3":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C1";
                            break;
                        case "4":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C2";
                            break;
                        case "5":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D1";
                            break;
                        case "6":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D2";
                            break;
                        case "7":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "R";
                            break;
                        case "8":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "U";
                            break;
                        case "9":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "CN";
                            break;
                        case "A":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "DN";
                            break;
                        default:
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                            break;
                    }
                    ssPan.ActiveSheet.Cells[nRow - 1, 3].Text = list4[i].SOGENCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 4].Text = list4[i].JOCHICODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 5].Text = list4[i].SAHUCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 6].Text = list4[i].WORKYN;
                    ssPan.ActiveSheet.Cells[nRow - 1, 7].Text = list4[i].SOGENREMARK;
                    ssPan.ActiveSheet.Cells[nRow - 1, 8].Text = list4[i].JOCHIREMARK;
                    ssPan.ActiveSheet.Cells[nRow - 1, 10].Text = list4[i].MCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 11].Text = list4[i].WRTNO.To<string>();
                    ssPan.ActiveSheet.Cells[nRow - 1, 12].Text = list4[i].RID;
                    ssPan.ActiveSheet.Cells[nRow - 1, 13].Text = "P";
                    ssPan.ActiveSheet.Cells[nRow - 1, 14].Text = strPyoJangi;

                    //2차 검진을 접수하였으면 1차 판정내역을 표시함
                    //TODO : 검진종류 및 차수를 읽어 PANHIS, PANJENG 테이블 보여줘야함
                    if (nHisCnt > 0)
                    {
                        HIC_SPC_PANHIS list5 = hicSpcPanhisService.GetItembyWrtNoMCodePyojanggi(FnWrtNo, strMCode, strPyoJangi);

                        if (!list5.IsNullOrEmpty())
                        {
                            ssPan.ActiveSheet.Cells[nRow - 1, 1].Text = " " + hb.READ_MCode_Name(list5.MCODE);
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                            switch (list5.PANJENG)
                            {
                                case "1":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "A";
                                    break;
                                case "2":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "B";
                                    break;
                                case "3":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C1";
                                    break;
                                case "4":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C2";
                                    break;
                                case "5":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D1";
                                    break;
                                case "6":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D2";
                                    break;
                                case "7":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "R";
                                    break;
                                case "8":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "U";
                                    break;
                                case "9":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "CN";
                                    break;
                                case "A":
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "DN";
                                    break;
                                default:
                                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                                    break;
                            }

                            ssPan.ActiveSheet.Cells[nRow - 1, 3].Text  = list5.SOGENCODE;
                            ssPan.ActiveSheet.Cells[nRow - 1, 4].Text  = list5.JOCHICODE;
                            ssPan.ActiveSheet.Cells[nRow - 1, 5].Text  = list5.SAHUCODE;
                            ssPan.ActiveSheet.Cells[nRow - 1, 6].Text  = list5.WORKYN;
                            ssPan.ActiveSheet.Cells[nRow - 1, 7].Text  = list5.SOGENREMARK;
                            ssPan.ActiveSheet.Cells[nRow - 1, 8].Text  = list5.JOCHIREMARK;
                            ssPan.ActiveSheet.Cells[nRow - 1, 10].Text = list5.MCODE;
                            ssPan.ActiveSheet.Cells[nRow - 1, 11].Text = list5.WRTNO.To<string>();
                            ssPan.ActiveSheet.Cells[nRow - 1, 12].Text = list5.RID;
                            ssPan.ActiveSheet.Cells[nRow - 1, 13].Text = "H";

                            //읽은 History을 제외함
                            for (int j = 0; j < 50; j++)
                            {
                                if (strHisROWID[j] == list5.RID)
                                {
                                    strHisROWID[j] = "";
                                    nHisCnt -= 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                //야간작업
                if (strMCode == "V01" || strMCode == "V02")
                {
                    fn_Display_NightMunjin(FnWrtNo);
                }
            }

            //HIS_SPC_PANHIS를 모두 표시하지 안았으면 나머지를 표시함
            if (nHisCnt > 0)
            {
                strList.Clear();
                for (int i = 1; i < 50; i++)
                {
                    if (!strHisROWID[i].IsNullOrEmpty())
                    {
                        strList.Add(strHisROWID[i]); 
                    }
                }

                List<HIC_SPC_PANHIS> list6 = hicSpcPanhisService.GetItembyWrtNoRowId(FnWrtNo, strList);

                nREAD = list6.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > ssPan.ActiveSheet.RowCount)
                    {
                        ssPan.ActiveSheet.RowCount = nRow;
                    }
                    ssPan.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    ssPan.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_MCode_Name(list6[i].MCODE);
                    ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                    switch (list6[i].PANJENG)
                    {
                        case "1":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "A";
                            break;
                        case "2":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "B";
                            break;
                        case "3":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C1";
                            break;
                        case "4":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "C2";
                            break;
                        case "5":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D1";
                            break;
                        case "6":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "D2";
                            break;
                        case "7":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "R";
                            break;
                        case "8":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "U";
                            break;
                        case "9":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "CN";
                            break;
                        case "A":
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "DN";
                            break;
                        default:
                            ssPan.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                            break;
                    }

                    ssPan.ActiveSheet.Cells[nRow - 1, 3].Text  = list6[i].SOGENCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 4].Text  = list6[i].JOCHICODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 5].Text  = list6[i].SAHUCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 6].Text  = list6[i].WORKYN;
                    ssPan.ActiveSheet.Cells[nRow - 1, 7].Text  = list6[i].SOGENREMARK;
                    ssPan.ActiveSheet.Cells[nRow - 1, 8].Text  = list6[i].JOCHIREMARK;
                    ssPan.ActiveSheet.Cells[nRow - 1, 10].Text = list6[i].MCODE;
                    ssPan.ActiveSheet.Cells[nRow - 1, 11].Text = list6[i].WRTNO.To<string>();
                    ssPan.ActiveSheet.Cells[nRow - 1, 12].Text = list6[i].RID;
                    ssPan.ActiveSheet.Cells[nRow - 1, 13].Text = "H";
                }
            }

            for (int i = 0; i < ssPan.ActiveSheet.RowCount; i++)
            {
                if (ssPan.ActiveSheet.Cells[i, 2].Text.Trim() != "" && ssPan.ActiveSheet.Cells[i, 2].Text != "R")
                {
                    //2차검진일때 1차검진시 R판정 표시
                    if (FstrGjChasu == "2")
                    {
                        strMCode = ssPan.ActiveSheet.Cells[i, 10].Text;
                        strPyoJangi = ssPan.ActiveSheet.Cells[i, 14].Text;

                        HIC_SPC_PANHIS list5 = hicSpcPanhisService.GetItembyWrtNoMCodePyojanggi(FnWrtNo, strMCode, strPyoJangi);

                        if (!list5.IsNullOrEmpty())
                        {
                            ssPan.ActiveSheet.Cells[i, 1, i, ssPan.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                            ssPan.ActiveSheet.Cells[i, 2].BackColor = Color.Aqua;
                            ssPan.ActiveSheet.Cells[i, 11].Text = FnWrtno2.ToString();
                        }
                        else
                        {
                            ssPan.ActiveSheet.Cells[i, 1, i, ssPan.ActiveSheet.ColumnCount - 1].BackColor = Color.LightGoldenrodYellow;
                        }
                    }
                    else
                    {
                        ssPan.ActiveSheet.Cells[i, 1, i, ssPan.ActiveSheet.ColumnCount - 1].BackColor = Color.LightGoldenrodYellow;
                    }
                }
                else
                {
                    ssPan.ActiveSheet.Cells[i, 1, i, ssPan.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                    ssPan.ActiveSheet.Cells[i, 2].BackColor = Color.Aqua;
                }
            }


            if (hicSpcPanhisService.GetCountbyWrtNoMCodePyoJanggi(FnWrtNo, strMCode, strPyoJangi) > 0 && !strPanRowId.IsNullOrEmpty())
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSpcPanhisService.Insert(strPanRowId);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("특수 판정결과 DB에 저장시 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        
        string fn_Panjeng_End_Check()
        {
            string rtnVal = "NO";
            string strOK = "";
            long nPanDrNo1 = 0;
            long nPanDrNo2 = 0;
            string strPanDate_RES = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;

            strOK = "OK";
            nPanDrno = 0;
            strPanDate = "";

            //특수검진의 미판정 건수를 읽음
            if (hicSpcPanjengService.GetCountbyWrtNo(FnWrtNo) > 0)
            {
                strOK = "NO";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if (strOK == "OK")
            {
                //판정의사를 읽음
                nPanDrNo1 = hicSpcPanjengService.GetPanjengDrNobyWrtNo(FnWrtNo);

                //이미 판정완료가 되었는지 점검
                HIC_RES_SPECIAL list = hicResSpecialService.GetPanjengDrNoDatebyWrtNo(FnWrtNo);

                if (!list.IsNullOrEmpty())
                {
                    nPanDrNo2 = list.PANJENGDRNO;
                    strPanDate_RES = list.PANJENGDATE.To<string>();
                    if (nPanDrno == 0)
                    {
                        nPanDrno = list.PANJENGDRNO;
                        strPanDate = list.PANJENGDATE.To<string>();
                    }
                }

                if (nPanDrNo1 != nPanDrNo2)
                {
                    result = hicResSpecialService.UpdatePanjengDrNoDatebyWrtNo(FnWrtNo, nPanDrNo1);

                    if (result < 0)
                    {
                        MessageBox.Show("특수검진 문진 및 판정결과 판정의사 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return rtnVal;
                    }
                }
                else
                {
                    result = hicResSpecialService.UpdatePanjengDatebyWrtNo(FnWrtNo);

                    if (result < 0)
                    {
                        MessageBox.Show("특수검진 문진 및 판정결과 판정의사 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return rtnVal;
                    }
                }
            }

            //판정완료/미완료 SET
            result = hicResBohum1Service.UpdateGbPanjengbyWrtNo(FnWrtNo, strOK);

            if (result < 0)
            {
                MessageBox.Show("건강검진1차 문진 및 판정결과 판정완료/미완료 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return rtnVal;
            }

            //접수마스타에 판정완료/미완료 SET
            if (FstrSelTab == "SPC" || FstrSelTab == "REC") //특수판정의 경우
            {
                if (strOK == "OK")  //판정이 완료되었으면
                {
                    //판정의사를 읽음
                    nPanDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo(FnWrtNo);
                    strPanDate = hicSpcPanjengService.GetPanjengDatebyWrtNo(FnWrtNo);
                }
            }

            if (nPanDrno == 0)
            {
                nPanDrno = clsHcVariable.GnHicLicense;
            }
            if (strPanDate.IsNullOrEmpty())
            {
                strPanDate = clsPublic.GstrSysDate;
            }
            result = hicJepsuService.UpdatePanjengbyWrtNo(nPanDrno, strPanDate, FnWrtNo, strOK);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 판정완료/미완료 Update중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return rtnVal;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            if (strOK == "OK")
            {
                //2차대상 판정내역 별도 보관(2차 접수를 하면 보관 안함)
                if (FstrJepDate2.IsNullOrEmpty())
                {
                    hm.UPDATE_SPC_PanHis(FnWrtNo);
                    if (MessageBox.Show("판정이 완료되었습니다." + "\r\n" + "화면을 Clear후 다음 환자를 판정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fn_Screen_Clear();
                        strOK = "OK";
                    }
                    else
                    {
                        strOK = "NO";
                    }   
                }
            }
            else if (strOK == "NO")
            {
                MessageBox.Show("판정이 완료되지 않았습니다...", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = strOK;

            return rtnVal;
        }
    }
}
