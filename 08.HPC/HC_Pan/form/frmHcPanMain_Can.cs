using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ComHpcLibB.clsHcType;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanMain_Can.cs
/// Description     : 암검진 문진 및 판정
/// Author          : 김민철
/// Create Date     : 2021-02-23
/// Update History  : 
/// </summary>
/// <history>  
/// 2021-02-23 암판정 프로그램 분리요청으로 개별작성함
/// </history>
/// <seealso cref= "Frm암검진판정_2020.frm(Frm암검진판정_2020)" />

namespace HC_Pan
{
    public partial class frmHcPanMain_Can :Form
    {

        public delegate void SetPanExamResultReg(long argWRTNO);
        public static event SetPanExamResultReg rSetPanExamResultReg;

        HicJepsuService hicJepsuService = null;
        HicCancerNewService hicCancerNewService = null;
        EndoJupmstService endoJupmstService = null;
        XrayResultnewService xrayResultnewService = null;
        HicResultService hicResultService = null;
        BasScheduleService basScheduleService = null;
        HicXrayResultService hicXrayResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicRescodeService hicRescodeService = null;
        HicSangdamNewService hicSangdamNewService = null;
        ExamAnatmstService examAnatmstService = null;
        BasBcodeService basBcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcAmCommonSetting FrmHcAmCommonSetting = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;
        frmViewResult FrmViewResult = null;

        bool bolSort = false;
        long FnWrtNo;
        long FnPano;
        string FstrSex;
        string FstrPano;
        string FstrPtno;
        string FstrJepDate;
        string FstrJob;
        string FItem;
   
        //자궁경부암 콤보 선택시 자동 결과 자동 update
        string FstrWomBo = "";
        string FstrWomBoCode = "";
        string FstrLiverRes = "";
        string FstrEndoRes = "";
        string FstrEndoRes1 = "";
        string FstrAnatRes = "";
        string FstrColonRes = "";
        string FstrColonRes1 = "";

        public frmHcPanMain_Can()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            this.KeyPreview = true;

            #region 판정항목 체크시 불들어오게
            Control[] ctrls = ComFunc.GetAllControlsUsingRecursive(this);
            List<CheckBox> lstChk = new List<CheckBox>();
            List<RadioButton> lstRdo = new List<RadioButton>();
            List<ComboBox> lstCbo = new List<ComboBox>();

            foreach (Control ctl in ctrls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;
                    if (chk.Name != "chkMunjin")
                    {
                        lstChk.Add(chk);
                    }
                }
                else if (ctl.GetType().Name.ToLower() == "radiobutton")
                {
                    RadioButton rdo = ctl as RadioButton;

                    if (rdo.Name != "rdoJob1" && rdo.Name != "rdoJob2" && rdo.Name != "rdoJob3"
                        && rdo.Name != "rdoSort1" && rdo.Name != "rdoSort2" && rdo.Name != "rdoSort3")
                    {
                        lstRdo.Add(rdo);
                    }
                }
                else if (ctl.GetType().Name.ToLower() == "combobox")
                {
                    ComboBox cbo = ctl as ComboBox;

                    lstCbo.Add(cbo);
                }
            }

            for (int i = 0; i < lstChk.Count; i++)
            {
                lstChk[i].CheckedChanged += eChkBoxChanged;
            }

            for (int i = 0; i < lstRdo.Count; i++)
            {
                lstRdo[i].CheckedChanged += eRdoChkChanged;
            }

            for (int i = 0; i < lstCbo.Count; i++)
            {
                lstCbo[i].SelectedIndexChanged += eCboSelectedIndexChanged2;
            }
            #endregion

            ssList.Initialize(new SpreadOption { RowHeaderVisible = true, RowHeight = 28, ColumnHeaderHeight = 38 });
            ssList.AddColumn("접수번호", nameof(HIC_JEPSU.WRTNO), 64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("수검자명", nameof(HIC_JEPSU.SNAME), 68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            ssList.AddColumn("성별", nameof(HIC_JEPSU.SEX), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("검진일자", nameof(HIC_JEPSU.JEPDATE), 78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("등록번호", nameof(HIC_JEPSU.PTNO), 78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            ssList.AddColumn("검진번호", nameof(HIC_JEPSU.PANO), 68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            ssList.AddColumn("위",   nameof(HIC_JEPSU.GBAM1),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("대장", nameof(HIC_JEPSU.GBAM2),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("간",   nameof(HIC_JEPSU.GBAM3),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("유방", nameof(HIC_JEPSU.GBAM4),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("자궁", nameof(HIC_JEPSU.GBAM5),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("폐",   nameof(HIC_JEPSU.GBAM6),      32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("문진", nameof(HIC_JEPSU.GBIEMUNJIN), 32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
        }

        private void eCboSelectedIndexChanged2(object sender, EventArgs e)
        {
            eInputStomachRes(null, null);
            eInputColonRes(null, null);
            eInputLiverRes(null, null);
            eInputBreastRes(null, null);
            eInputLungRes(null, null);
        }

        private void eRdoChkChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                ((RadioButton)sender).Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                ((RadioButton)sender).BackColor = Color.FromArgb(255, 192, 128);

                if (sender == rdoStomachTissue1)    //미시행
                {
                    Control[] ctrls = ComFunc.GetAllControlsUsingRecursive(panel16);
                    
                    foreach (Control ctl in ctrls)
                    {
                        if (ctl.GetType().Name.ToLower() == "checkbox")
                        {
                            CheckBox chk = ctl as CheckBox;
                            chk.Checked = false;
                        }
                    }

                    txtStomachTissueEtcCancer.Text = "";

                    Control[] ctrls2= ComFunc.GetAllControlsUsingRecursive(panel17);

                    foreach (Control ctl in ctrls2)
                    {
                        if (ctl.GetType().Name.ToLower() == "checkbox")
                        {
                            CheckBox chk = ctl as CheckBox;
                            chk.Checked = false;
                        }
                    }

                    txtStomachTissueEtc.Text = "";

                    cboResult0001.SelectedIndex = 0;

                    panel16.Enabled = false; panel17.Enabled = false;
                    
                }
                else if (sender == rdoStomachTissue0)   //시행
                {
                    //cboResult0001.Enabled = true;
                    
                }
            }
            else
            {
                ((RadioButton)sender).Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                ((RadioButton)sender).BackColor = Color.Transparent;

                if (sender == rdoStomachTissue0)    //시행
                {
                    cboResult0001.SelectedIndex = 0;
                    //cboResult0001.Enabled = false;
                }
            }

            eInputLiverRes(null, null);
            eInputStomachRes(null, null);
            eInputColonRes(null, null);
            eInputBreastRes(null, null);
            eInputLungRes(null, null);
        }

        private void eChkBoxChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                ((CheckBox)sender).Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                ((CheckBox)sender).BackColor = Color.FromArgb(255, 192, 128);
            }
            else
            {
                ((CheckBox)sender).Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                ((CheckBox)sender).BackColor = Color.Transparent;
            }

            eInputLiverRes(null, null);
            eInputStomachRes(null, null);
            eInputColonRes(null, null);
            eInputBreastRes(null, null);
            eInputLungRes(null, null);
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicCancerNewService = new HicCancerNewService();
            endoJupmstService = new EndoJupmstService();
            xrayResultnewService = new XrayResultnewService();
            hicResultService = new HicResultService();
            basScheduleService = new BasScheduleService();
            hicXrayResultService = new HicXrayResultService();
            comHpcLibBService = new ComHpcLibBService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicResultExCodeService = new HicResultExCodeService();
            hicRescodeService = new HicRescodeService();
            hicSangdamNewService = new HicSangdamNewService();
            examAnatmstService = new ExamAnatmstService();
            basBcodeService = new BasBcodeService();

            this.Load += new EventHandler(eFormLoad);
            //this.KeyDown += new KeyEventHandler(eKeyDown);
            this.btnResultSave.Click += new EventHandler(eBtnClick);
            this.btnF1.Click += new EventHandler(eBtnClick);
            this.btnF2.Click += new EventHandler(eBtnClick);
            this.btnF3.Click += new EventHandler(eBtnClick);
            this.btnF4.Click += new EventHandler(eBtnClick);
            this.btnF5.Click += new EventHandler(eBtnClick);
            this.btnF6.Click += new EventHandler(eBtnClick);
            this.btnF7.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnText.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnCommon0.Click += new EventHandler(eBtnClick);
            this.btnCommon1.Click += new EventHandler(eBtnClick);
            this.btnCommon2.Click += new EventHandler(eBtnClick);
            this.btnCommon3.Click += new EventHandler(eBtnClick);
            this.btnCommon4.Click += new EventHandler(eBtnClick);
            this.btnCommon5.Click += new EventHandler(eBtnClick);
            this.btnCommon6.Click += new EventHandler(eBtnClick);
            this.btnCommon7.Click += new EventHandler(eBtnClick);
            this.btnCommon8.Click += new EventHandler(eBtnClick);
            this.btnGubDaesang.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnLungHELP.Click += new EventHandler(eBtnClick);
            this.btnHelpLungExit.Click += new EventHandler(eBtnClick);

            this.lblAutoPanInfo.Click += new EventHandler(eLabelClick);

            this.rdoWomBo11.Click += new EventHandler(eRdoClick);
            this.rdoWomBo12.Click += new EventHandler(eRdoClick);
            this.rdoWomBo21.Click += new EventHandler(eRdoClick);
            this.rdoWomBo22.Click += new EventHandler(eRdoClick);
            this.rdoWomBo31.Click += new EventHandler(eRdoClick);
            this.rdoWomBo32.Click += new EventHandler(eRdoClick);
            this.rdoWomBo33.Click += new EventHandler(eRdoClick);
            this.rdoWomBo41.Click += new EventHandler(eRdoClick);
            this.rdoWomBo42.Click += new EventHandler(eRdoClick);
            this.rdoWomBo43.Click += new EventHandler(eRdoClick);
            this.rdoWomBo44.Click += new EventHandler(eRdoClick);
            this.rdoWomBo61.Click += new EventHandler(eRdoClick);
            this.rdoWomBo62.Click += new EventHandler(eRdoClick);
            this.rdoWomBo63.Click += new EventHandler(eRdoClick);
            this.rdoWomBo64.Click += new EventHandler(eRdoClick);
            this.rdoWomBo71.Click += new EventHandler(eRdoClick);
            this.rdoWomBo72.Click += new EventHandler(eRdoClick);
            this.rdoWomBo73.Click += new EventHandler(eRdoClick);
            this.rdoWomBo74.Click += new EventHandler(eRdoClick);
            this.rdoWomBo75.Click += new EventHandler(eRdoClick);
            this.rdoWomBo76.Click += new EventHandler(eRdoClick);
            this.rdoWomBo77.Click += new EventHandler(eRdoClick);

            this.rdoColonTissue0.Click += new EventHandler(eRdoClick);
            this.rdoColonTissue1.Click += new EventHandler(eRdoClick);
            this.rdoStomachTissue0.Click += new EventHandler(eRdoClick);
            this.rdoStomachTissue0.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoStomachTissue1.Click += new EventHandler(eRdoClick);
            this.rdoStomachTissue1.CheckedChanged += new EventHandler(eRdoChkChanged);

            this.cboResult0001.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboSPan.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboCPan.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboLPan.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboWPan.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboLPan1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboEndo0.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboEndo1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboEndo2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboForgedYoung0.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboForgedYoung1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboForgedYoung2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonizationAassistant0.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonizationAassistant1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonizationAassistant2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonoScope0.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonoScope1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboColonoScope2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboBreast0.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboBreast1.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);
            this.cboBreast2.SelectedIndexChanged += new EventHandler(eCboSelectedIndexChanged);


            this.rdoColonTissue0.Click += new EventHandler(eRdoClick);
            this.rdoColonTissue1.Click += new EventHandler(eRdoClick);

            this.txtResult0007.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtResult0008.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtResult0009.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.txtForgedYoungEtc.TextChanged             += new EventHandler(eTxtChanged);
            this.txtEndoEtc.TextChanged                    += new EventHandler(eTxtChanged);
            this.txtStomachTissueEtcCancer.TextChanged     += new EventHandler(eTxtChanged);
            this.txtStomachTissueEtc.TextChanged           += new EventHandler(eTxtChanged);
            this.txtColonizationAassistantEtc.TextChanged  += new EventHandler(eTxtChanged);
            this.txtColonScopeEtc.TextChanged              += new EventHandler(eTxtChanged);
            this.txtColonEtc.TextChanged                   += new EventHandler(eTxtChanged);
            this.txtColonCancerEtc.TextChanged             += new EventHandler(eTxtChanged);
            this.txtResult0012.TextChanged                 += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc10.TextChanged             += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc11.TextChanged             += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc20.TextChanged             += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc21.TextChanged             += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc30.TextChanged             += new EventHandler(eTxtChanged);
            this.txtBreastPosEtc31.TextChanged             += new EventHandler(eTxtChanged);
            this.txtWombo6_Etc.TextChanged                 += new EventHandler(eTxtChanged);
            this.txtWombo3_Etc.TextChanged                 += new EventHandler(eTxtChanged);
            this.txtWombo7_Etc.TextChanged                 += new EventHandler(eTxtChanged);
            this.txtResult11.TextChanged                   += new EventHandler(eTxtChanged);
            this.txtResult4.TextChanged                    += new EventHandler(eTxtChanged);
            this.txtResult5.TextChanged                    += new EventHandler(eTxtChanged);

            this.chkResult00040.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult00041.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult00042.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult00043.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult00150.CheckedChanged += new EventHandler(eChkChanged2);
                 
            this.chkResult200.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult201.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult202.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult203.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult204.CheckedChanged += new EventHandler(eChkChanged2);

            this.chkResult210.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult211.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult212.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult213.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult214.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult215.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult216.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult217.CheckedChanged += new EventHandler(eChkChanged2);
            this.chkResult218.CheckedChanged += new EventHandler(eChkChanged2);

            for (int i = 0; i < 8; i++)
            {
                CheckBox chkResult0005 = (Controls.Find("chkResult0005" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkResult0006 = (Controls.Find("chkResult0006" + i.ToString(), true)[0] as CheckBox);
                chkResult0005.CheckedChanged += new EventHandler(eChkChanged2);
                chkResult0006.CheckedChanged += new EventHandler(eChkChanged2);
            }

            this.chkWomBo4.Click += new EventHandler(eChkChanged2);
            this.chkWomBo6.Click += new EventHandler(eChkChanged2);

            this.chkResult010.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult011.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult012.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult013.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult014.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult015.CheckedChanged += new EventHandler(eChkChanged3);

            this.chkResult020.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult021.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult022.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult023.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult024.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult025.CheckedChanged += new EventHandler(eChkChanged3);

            this.chkResult030.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult031.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult032.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult033.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult034.CheckedChanged += new EventHandler(eChkChanged3);
            this.chkResult035.CheckedChanged += new EventHandler(eChkChanged3);

            this.chkCancer0.CheckedChanged += new EventHandler(eChkChanged);
            this.chkCancer1.CheckedChanged += new EventHandler(eChkChanged);
            this.chkCancer2.CheckedChanged += new EventHandler(eChkChanged);
            this.chkCancer3.CheckedChanged += new EventHandler(eChkChanged);
            this.chkCancer4.CheckedChanged += new EventHandler(eChkChanged);
            this.chkCancer5.CheckedChanged += new EventHandler(eChkChanged);

            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLiverRes.Click      += new EventHandler(eInputLiverRes);
            this.btnStomachRes.Click    += new EventHandler(eInputStomachRes);
            this.btnColonRes.Click      += new EventHandler(eInputColonRes);
            this.btnBreastRes.Click     += new EventHandler(eInputBreastRes);
            this.btnLungRes.Click       += new EventHandler(eInputLungRes);

            this.rdoCut0.Click += new EventHandler(eRdoClick);
            this.rdoCut1.Click += new EventHandler(eRdoClick);

            this.tab1.Click += new EventHandler(eTabClick);
            this.tab2.Click += new EventHandler(eTabClick);
            this.tab3.Click += new EventHandler(eTabClick);
            this.tab4.Click += new EventHandler(eTabClick);
            this.tab5.Click += new EventHandler(eTabClick);
            this.tab6.Click += new EventHandler(eTabClick);

            this.rTab_List.Click += new EventHandler(eTabClick);
            this.rTab_Info.Click += new EventHandler(eTabClick);
            
            this.dtpFrDate.TextChanged += new EventHandler(eDtpChanged);
            this.txtWrtno.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.ssList.CellClick += new CellClickEventHandler(eSpdClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.btnExamResultReg.Click += new EventHandler(eBtnClick);
        }

        
        private void eKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F1) { btnF1.PerformClick(); }
            //else if (e.KeyCode == Keys.F2) { btnF2.PerformClick(); }
            //else if (e.KeyCode == Keys.F3) { btnF3.PerformClick(); }
            //else if (e.KeyCode == Keys.F4) { btnF4.PerformClick(); }
            //else if (e.KeyCode == Keys.F5) { btnF5.PerformClick(); }
            //else if (e.KeyCode == Keys.F6) { btnF6.PerformClick(); }
            //else if (e.KeyCode == Keys.F7) { btnF7.PerformClick(); }
        }

        private void eTxtChanged(object sender, EventArgs e)
        {
            if (sender == txtForgedYoungEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkForgedYoungEtc7.Checked = true; }
            }
            else if (sender == txtEndoEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkEndoEtc7.Checked = true; }
            }
            else if (sender == txtStomachTissueEtcCancer)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkStomachTissue13.Checked = true; }
            }
            else if (sender == txtStomachTissueEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkStomachTissueEtc7.Checked = true; }
            }
            else if (sender == txtColonizationAassistantEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkColonizationAassistantEtc9.Checked = true; }
            }
            else if (sender == txtColonScopeEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkColonoScopeEtc9.Checked = true; }
            }
            else if (sender == txtColonEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkColonCancer12.Checked = true; }
            }
            else if (sender == txtColonCancerEtc)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkColonCancerEtc4.Checked = true; }
            }
            else if (sender == txtResult0012)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkResult00107.Checked = true; }
            }
            else if (sender == txtBreastPosEtc10)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastR16.Checked = true; }
            }
            else if (sender == txtBreastPosEtc11)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastL16.Checked = true; }
            }
            else if (sender == txtBreastPosEtc20)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastR26.Checked = true; }
            }
            else if (sender == txtBreastPosEtc21)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastL26.Checked = true; }
            }
            else if (sender == txtBreastPosEtc30)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastR36.Checked = true; }
            }
            else if (sender == txtBreastPosEtc31)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkBreastL36.Checked = true; }
            }
            else if (sender == txtWombo6_Etc)
            {

                if (((TextBox)sender).Text.Trim() != "")
                {
                    rdoWomBo64.Checked = true;
                    rResultChange("A169", txtWombo6_Etc.Text);
                }
            }
            else if (sender == txtWombo3_Etc)
            {
                if (((TextBox)sender).Text.Trim() != "")
                {
                    rdoWomBo33.Checked = true;
                }
            }
            else if (sender == txtWombo7_Etc)
            {
                if (((TextBox)sender).Text.Trim() != "")
                {
                    rdoWomBo77.Checked = true;
                    rResultChange("A170", txtWombo7_Etc.Text);
                }
            }
            else if (sender == txtResult11)
            {
                if (((TextBox)sender).Text.Trim() != "") { rdoResult61.Checked = true; }
            }
            else if (sender == txtResult4)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkResult204.Checked = true; }
            }
            else if (sender == txtResult5)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkResult218.Checked = true; }
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtResult0007 || sender == txtResult0008 || sender == txtResult0009)
            {
                if (((TextBox)sender).Text.Trim() != "") { chkResult00040.Checked = false; }
            }
        }

        private void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtno && e.KeyChar == (char)13)
            {
                if (txtWrtno.Text.Trim() == "") { return; }

                FnWrtNo = txtWrtno.Text.To<long>(0);
                clsHcVariable.GnWRTNO = FnWrtNo;
                
                fn_Screen_Clear();

                SS2.Parent = panList01;
                SS2.Dock = DockStyle.Fill;
                SS2.Visible = true;

                ssList.Dock = DockStyle.None;
                ssList.Visible = false;

                fn_Injek_Display(FnWrtNo);
                fn_Exam_Result_Display(FnWrtNo, FstrJepDate, FstrPtno, FstrSex);
                fn_OLD_Result_Display(FstrSex);
                fn_Screen_Display();
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (!e.RowHeader && !e.ColumnHeader)
            {
                if (sender == ssList)
                {
                    FnWrtNo = ssList.ActiveSheet.Cells[e.Row, 0].Text.To<long>(0);
                    clsHcVariable.GnWRTNO = FnWrtNo;

                    if (FnWrtNo == 0) { return; }

                    txtWrtno.Text = FnWrtNo.To<string>("");

                    fn_Screen_Clear();

                    SS2.Parent = panList01;
                    SS2.Dock = DockStyle.Fill;
                    SS2.Visible = true;

                    ssList.Dock = DockStyle.None;
                    ssList.Visible = false;

                    fn_Injek_Display(FnWrtNo);
                    fn_Exam_Result_Display(FnWrtNo, FstrJepDate, FstrPtno, FstrSex);
                    fn_OLD_Result_Display(FstrSex);
                    fn_Screen_Display();
                }
            }
        }

        private string GetCytologyResult(string strROWID)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strAnatNo = "";
            string strHRRemark1 = "";
            string strHRRemark2 = "";
            string strHRRemark3 = "";
            string strHRRemark4 = "";
            string strHRRemark5 = "";

            string strResultSabun = "";

            string strResult = "";
            string strRDate = "";
            string strRDate2 = "";
            string strReDate = "";


            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT A.ANATNO, A.GBJOB, A.RESULT1, A.RESULT2, A.HRREMARK1, A.HRREMARK2,";
                SQL = SQL + ComNum.VBLF + "  A.HRREMARK3, A.HRREMARK4, A.HRREMARK5,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.RESULTDATE ,'YYYY-MM-DD') RDATE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(B.RECEIVEDATE, 'YYYY-MM-DD') REDATE, ";
                SQL = SQL + ComNum.VBLF + "  A.RESULTDATE, A.RESULTSABUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ANATMST A , KOSMOS_OCS.EXAM_SPECMST B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ROWID = '" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.SPECNO = B.SPECNO(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GBJOB"].ToString().Trim() != "V")
                    {
                        strVal = ComNum.VBLF + " ▶병리과에서 검사 결과 확인중";
                        dt.Dispose();
                        dt = null;
                        return strVal;
                    }
                    else
                    {
                        strRDate = dt.Rows[0]["RDATE"].ToString().Trim();
                        strReDate = dt.Rows[0]["REDATE"].ToString().Trim();

                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병리번호: " + VB.Space(20), 20) + dt.Rows[0]["ANATNO"].ToString().Trim() + ComNum.VBLF;
                        strResult = strResult + ComNum.VBLF + ComFunc.LeftH("▶ 검사일: " + VB.Space(20), 20) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보고일: " + VB.Space(20), 20) + strRDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + "---------------------------------------------------------------";
                        strResult = strResult + ComNum.VBLF + dt.Rows[0]["RESULT1"].ToString().Trim() + dt.Rows[0]["RESULT2"].ToString().Trim();

                        strAnatNo = dt.Rows[0]["ANATNO"].ToString().Trim();
                        strHRRemark1 = dt.Rows[0]["HRREMARK1"].ToString().Trim();
                        strHRRemark2 = dt.Rows[0]["HRREMARK2"].ToString().Trim();
                        strHRRemark3 = dt.Rows[0]["HRREMARK3"].ToString().Trim();
                        strHRRemark4 = dt.Rows[0]["HRREMARK4"].ToString().Trim();
                        strHRRemark5 = dt.Rows[0]["HRREMARK5"].ToString().Trim();

                        strRDate2 = dt.Rows[0]["RESULTDATE"].ToString().Trim();
                        strResultSabun = dt.Rows[0]["RESULTSABUN"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (VB.Left(strAnatNo, 2) == "PS") //객담 --------------------------------------------------------------------------
                {
                    //'검체상태
                    strResult += ComNum.VBLF + ComFunc.LeftH("▶ 병리번호: " + VB.Space(20), 20) + strAnatNo + ComNum.VBLF;

                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검체상태 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 1, 1) == "1") //적정
                    {
                        strResult = strResult + "적정 ";
                    }
                    else if (VB.Mid(strHRRemark1, 2, 1) == "1") //제한적
                    {
                        strResult = strResult + "제한적 ";
                    }
                    else if (VB.Mid(strHRRemark1, 3, 1) == "1") //불량
                    {
                        strResult = strResult + "불량 ";
                    }

                    //'결과
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 결    과 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 4, 1) == "1") //'음성(I)
                    {
                        strResult = strResult + "음성(I)";
                    }
                    else if (VB.Mid(strHRRemark1, 5, 1) == "1") //음성(II)
                    {
                        strResult = strResult + "음성(II)";
                    }
                    else if (VB.Mid(strHRRemark1, 6, 1) == "1") //'의양성(III)
                    {
                        strResult = strResult + "의양성(III)";
                    }
                    else if (VB.Mid(strHRRemark1, 7, 1) == "1") //'양성(IV)
                    {
                        strResult = strResult + "양성(IV)";
                    }
                    else if (VB.Mid(strHRRemark1, 8, 1) == "1") //'양성(V)
                    {
                        strResult = strResult + "양성(V)";
                    }

                    //'대책
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 대    책 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 9, 1) == "1") //'소변 채취 방법 설명 후 재검사
                    {
                        strResult = strResult + "소변 채취 방법 설명 후 재검사";
                    }
                    else if (VB.Mid(strHRRemark1, 10, 1) == "1") // '정밀검사필요
                    {
                        strResult = strResult + "정밀검사필요";
                    }

                    strResult = strResult + ComNum.VBLF + strHRRemark5;
                }
                else if (VB.Left(strAnatNo, 1) == "P") //'부인과암(자궁도말)
                {
                    if (VB.Val((strAnatNo).Replace("P", "")) >= VB.Val("0900134"))  //if (strAnatNo >= "P0900134")
                    {
                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병    리   번   호: " + VB.Space(25), 25) + strAnatNo + ComNum.VBLF;
                        strResult += ComNum.VBLF + ComFunc.LeftH("▶ 검      사      일 : " + VB.Space(25), 25) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보      고      일 : " + VB.Space(25), 25) + strRDate;

                        //'검체상태
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검    체   상   태 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 1, 1) == "1") //'적정
                        {
                            strResult = strResult + "1.적정";
                        }
                        else if (VB.Mid(strHRRemark1, 2, 1) == "1") //'부적절
                        {
                            strResult = strResult + "2.부적절";
                        }

                        //'자궁경부 선상피세포(유,무)
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 자궁경부선상피세포 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 3, 1) == "1") //'유
                        {
                            strResult = strResult + "1.유 ";
                        }
                        else if (VB.Mid(strHRRemark1, 4, 1) == "1") //'무
                        {
                            strResult = strResult + "2.무 ";
                        }

                        //'유형별 진단
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 유  형  별  진  단 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 5, 1) == "1") //1.음성
                        {
                            strResult = strResult + "1.음성 ";
                        }
                        else if (VB.Mid(strHRRemark1, 6, 1) == "1") //2.상피세포이상
                        {
                            strResult = strResult + "2.상피세포이상";
                        }
                        else if (VB.Mid(strHRRemark1, 7, 1) == "1") //3.기타(자궁내막세포출현)
                        {
                            strResult = strResult + "3.기타(자궁내막세포출현)";
                        }

                        //'편평상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 편평 상피세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 8, 1) == "1") // '비정형 편평상피세포
                        {
                            strResult = strResult + "1.비정형 편평상피세포";
                            if (VB.Mid(strHRRemark1, 9, 1) == "1") //'일반
                            {
                                strResult = strResult + ":일반";
                            }
                            else if (VB.Mid(strHRRemark1, 10, 1) == "1") //'고위험
                            {
                                strResult = strResult + ":고위험";
                            }
                        }
                        else if (VB.Mid(strHRRemark1, 11, 1) == "1") //2.저등급 편평상피내 병변
                        {
                            strResult = strResult + "2.저등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 12, 1) == "1") //3.고등급 편평상피내 병변
                        {
                            strResult = strResult + "3.고등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 13, 1) == "1") //4.침윤성 편평세표암종
                        {
                            strResult = strResult + "4.침윤성 편평세표암종";
                        }

                        //'선상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 선 상 피 세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 14, 1) == "1") //'비정형 선상피세포
                        {
                            strResult = strResult + "1.비정형 선상피세포";

                            #region 2018-07-10 안정수, 병리과요청으로 일반, 종양성 항목 추가 

                            if (VB.Mid(strHRRemark1, 31, 1) == "1") //'일반
                            {
                                strResult = strResult + ":일반";
                            }
                            else if (VB.Mid(strHRRemark1, 32, 1) == "1") //'종양성
                            {
                                strResult = strResult + ":종양성";
                            }

                            #endregion
                        }
                        else if (VB.Mid(strHRRemark1, 15, 1) == "1") //'상피내 선압종
                        {
                            strResult = strResult + "2.상피내 선압종";
                        }
                        else if (VB.Mid(strHRRemark1, 16, 1) == "1") //'침윤성 선암종
                        {
                            strResult = strResult + "3.침윤성 선암종";
                        }
                        else if (VB.Mid(strHRRemark1, 17, 1) == "1") //'기타
                        {
                            strResult = strResult + "4.기타: ";
                            strResult = strResult + strHRRemark2;
                        }

                        //'추가소견
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 추   가    소   견 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 18, 1) == "1") //'반응성 세포변화
                        {
                            strResult = strResult + "1.반응성 세포변화" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 19, 1) == "1") //'트리코모나스
                        {
                            strResult = strResult + "2.트리코모나스 " + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 20, 1) == "1") //캔디다
                        {
                            strResult = strResult + "3.캔디다" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 21, 1) == "1") //'방선균
                        {
                            strResult = strResult + "4.방선균" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 22, 1) == "1") //'헤르페스 바이러스
                        {
                            strResult = strResult + "5.헤르페스 바이러스" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 23, 1) == "1") //'기타
                        {
                            strResult = strResult + ComNum.VBLF + VB.Space(25) + "6.기타 : ";
                            strResult = strResult + strHRRemark3;
                        }

                        //'종합판정
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 종   합    판   정 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 24, 1) == "1")
                        {
                            strResult = strResult + "1.이상소견 없음 ";
                        }
                        else if (VB.Mid(strHRRemark1, 25, 1) == "1")
                        {
                            strResult = strResult + "2.염증성 또는 감염성질환";
                        }
                        else if (VB.Mid(strHRRemark1, 26, 1) == "1")
                        {
                            strResult = strResult + "3.상피세포 이상";
                        }
                        else if (VB.Mid(strHRRemark1, 30, 1) == "1")
                        {
                            strResult = strResult + "4.자궁경부암 전구단계";
                        }
                        else if (VB.Mid(strHRRemark1, 27, 1) == "1")
                        {
                            strResult = strResult + "5.자궁경부암 의심:";
                        }
                        else if (VB.Mid(strHRRemark1, 28, 1) == "1")
                        {
                            strResult = strResult + "6.기타:";
                            strResult = strResult + strHRRemark4;
                        }
                        else if (VB.Mid(strHRRemark1, 29, 1) == "1")
                        {
                            strResult = strResult + "기존 자궁경부암 환자";
                        }

                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + strHRRemark5;
                    }
                    else
                    {
                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병    리   번    호: " + VB.Space(25), 25) + strAnatNo + ComNum.VBLF;
                        strResult += ComNum.VBLF + ComFunc.LeftH("▶ 검      사      일 : " + VB.Space(25), 25) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보      고      일 : " + VB.Space(25), 25) + strRDate;

                        // '검체상태
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검    체   상   태 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 1, 1) == "1") //'적정
                        {
                            strResult = strResult + "1.적정 ";
                        }
                        else if (VB.Mid(strHRRemark1, 2, 1) == "1") //'부적절
                        {
                            strResult = strResult + "2.부적절 ";
                        }

                        //'자궁경부 선상피세포(유,무)
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 자궁경부선상피세포 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 3, 1) == "1") //1.유
                        {
                            strResult = strResult + "1.유 ";
                        }
                        else if (VB.Mid(strHRRemark1, 4, 1) == "1") //2.무
                        {
                            strResult = strResult + "2.무 ";
                        }

                        //'유형별 진단
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 유  형  별  진  단 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 5, 1) == "1") //음성
                        {
                            strResult = strResult + "1.음성 ";
                        }
                        else if (VB.Mid(strHRRemark1, 6, 1) == "1") //상피세포이상
                        {
                            strResult = strResult + "2.상피세포이상";
                        }
                        else if (VB.Mid(strHRRemark1, 7, 1) == "1") //기타(자궁내막세포출현)
                        {
                            strResult = strResult + "3.기타(자궁내막세포출현)";
                        }

                        //'편평상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 편평 상피세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 8, 1) == "1") //'비정형 편평상피세포
                        {
                            strResult = strResult + "1.비정형 편평상피세포";
                        }
                        else if (VB.Mid(strHRRemark1, 9, 1) == "1") //'저등급 편평상피내 병변
                        {
                            strResult = strResult + "2.저등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 10, 1) == "1") //'고등급 편평상피내 병변
                        {
                            strResult = strResult + "3.고등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 11, 1) == "1") //침윤성 편평세표암종
                        {
                            strResult = strResult + "4.침윤성 편평세표암종";
                        }

                        //'선상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 선 상 피 세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 12, 1) == "1") //'비정형 선상피세포
                        {
                            strResult = strResult + "1.비정형 선상피세포";
                        }
                        else if (VB.Mid(strHRRemark1, 13, 1) == "1")//'상피내 선압종
                        {
                            strResult = strResult + "2.상피내 선압종";
                        }
                        else if (VB.Mid(strHRRemark1, 14, 1) == "1")//'침윤성 선암종
                        {
                            strResult = strResult + "3.침윤성 선암종";
                        }
                        else if (VB.Mid(strHRRemark1, 15, 1) == "1") //'기타
                        {
                            strResult = strResult + "4.기타: ";
                        }

                        //'추가소견
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 추   가    소   견 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 16, 1) == "1")  //'반응성 세포변화
                        {
                            strResult = strResult + "1.반응성 세포변화" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 17, 1) == "1") //'트리코모나스
                        {
                            strResult = strResult + "2.트리코모나스 " + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 18, 1) == "1") //'캔디다
                        {
                            strResult = strResult + "3.캔디다" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 19, 1) == "1")//'방선균
                        {
                            strResult = strResult + "4.방선균" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 20, 1) == "1") //'헤르페스 바이러스
                        {
                            strResult = strResult + "5.헤르페스 바이러스" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 21, 1) == "1") //'기타
                        {
                            strResult = strResult + ComNum.VBLF + VB.Space(25) + "6.기타 : ";
                            strResult = strResult + strHRRemark3;
                        }

                        //'종합판정
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 종   합    판   정 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 22, 1) == "1") //'정상
                        {
                            strResult = strResult + "1.정상";
                        }
                        else if (VB.Mid(strHRRemark1, 23, 1) == "1") //'염증성 또는 감염성질환
                        {
                            strResult = strResult + "2.추적검사필요";
                        }
                        else if (VB.Mid(strHRRemark1, 24, 1) == "1") //'상피세포 이상
                        {
                            strResult = strResult + "3.암의심정밀검사필요";
                        }
                        else if (VB.Mid(strHRRemark1, 25, 1) == "1") //'자궁경부암 의심
                        {
                            strResult = strResult + "4.기타질환:";
                        }
                        else if (VB.Mid(strHRRemark1, 26, 1) == "1") //'기타
                        {
                            strResult = strResult + "5.기 암환자";
                        }
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + strHRRemark5;
                    }
                }

                strResult = strResult + ComNum.VBLF + ComNum.VBLF + "판정일자 : " + strRDate2 + "    판정의사 : " + cf.Read_SabunName(clsDB.DbCon, strResultSabun);
                strVal = strResult;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }


            return strVal;
        }

        private void fn_Injek_Display(long fnWrtNo)
        {
            //인적사항을 READ
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(fnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrSex = list.SEX;
            FstrJepDate = list.JEPDATE;
            FstrPtno = list.PTNO;
            FstrPano = list.PTNO;
            FnPano = list.PANO;

            SSPano.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            SSPano.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            SSPano.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>("0") + "/" + FstrSex;
            SSPano.ActiveSheet.Cells[0, 3].Text = cf.Read_Ltd_Name(clsDB.DbCon, list.LTDCODE.ToString());
            SSPano.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>("");
            SSPano.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);  //건진유형
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssList, e.Column, ref bolSort, true);
            }
        }

        private void eDtpChanged(object sender, EventArgs e)
        {
            dtpToDate.Text = dtpFrDate.Text;
        }

        
        private void eInputStomachRes(object sender, EventArgs e)
        {
            string strCodeEndo = "TX23";
            string strCodeEndo1 = "TX22";
            string strCodeAnat1 = "TX45";
            string strCodeAnat2 = "TX46";
            string strCodeAnat3 = "TX47";
            string strCodeAnat4 = "TX48";
            string strCodeAnat5 = "TX49";

            FstrEndoRes = "";
            FstrEndoRes1 = "";
            FstrAnatRes = "";

            if (cboEndo0.Text.Trim() != "" && VB.Left(cboEndo0.Text.Trim(), 2) != "09") { FstrEndoRes += VB.Pstr(cboEndo0.Text, ".", 2) + ","; }    //관찰소견1
            if (cboEndo1.Text.Trim() != "" && VB.Left(cboEndo1.Text.Trim(), 2) != "09") { FstrEndoRes += VB.Pstr(cboEndo1.Text, ".", 2) + ","; }    //관찰소견2
            if (cboEndo2.Text.Trim() != "" && VB.Left(cboEndo2.Text.Trim(), 2) != "09") { FstrEndoRes += VB.Pstr(cboEndo2.Text, ".", 2) + ","; }    //관찰소견3

            //기타소견
            foreach (Control ctl in panel9.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkEndoEtc7")
                        {
                            FstrEndoRes += txtEndoEtc.Text + ",";
                        }
                        else
                        {
                            FstrEndoRes += chk.Text + ",";
                        }
                    }
                }
            }

            if (FstrEndoRes != "" && VB.Right(FstrEndoRes, 1) == ",")
            {
                FstrEndoRes = VB.Left(FstrEndoRes, FstrEndoRes.Length - 1);
            }

            rResultChange(strCodeEndo, FstrEndoRes);

            //위장조영검사
            if (cboForgedYoung0.Text.Trim() != "" && VB.Left(cboForgedYoung0.Text.Trim(), 1) != "9") { FstrEndoRes1 += VB.Pstr(cboForgedYoung0.Text, ".", 2) + ","; }    //관찰소견1
            if (cboForgedYoung1.Text.Trim() != "" && VB.Left(cboForgedYoung1.Text.Trim(), 1) != "9") { FstrEndoRes1 += VB.Pstr(cboForgedYoung1.Text, ".", 2) + ","; }    //관찰소견2
            if (cboForgedYoung2.Text.Trim() != "" && VB.Left(cboForgedYoung2.Text.Trim(), 1) != "9") { FstrEndoRes1 += VB.Pstr(cboForgedYoung2.Text, ".", 2) + ","; }    //관찰소견3
            //기타소견
            foreach (Control ctl in panel11.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkForgedYoungEtc7")
                        {
                            FstrEndoRes1 += txtForgedYoungEtc.Text + ",";
                        }
                        else
                        {
                            FstrEndoRes1 += chk.Text + ",";
                        }
                    }
                }
            }

            if (FstrEndoRes1 != "" && VB.Right(FstrEndoRes1, 1) == ",")
            {
                FstrEndoRes1 = VB.Left(FstrEndoRes1, FstrEndoRes1.Length - 1);
            }

            rResultChange(strCodeEndo1, FstrEndoRes1);


            //
            if (cboResult0001.Text.Trim() != "" && VB.Pstr(cboResult0001.Text, ".", 1) != "07" && VB.Pstr(cboResult0001.Text, ".", 1) != "08")
            {
                FstrAnatRes += VB.Pstr(cboResult0001.Text, ".", 2).Trim() + ",";
            }

            //조직진단
            foreach (Control ctl in panel16.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkStomachTissue13")
                        {
                            FstrAnatRes += txtStomachTissueEtcCancer.Text + ",";
                        }
                        else
                        {
                            FstrAnatRes += chk.Text + ",";
                        }
                    }
                }
            }

            //조직진단 기타
            foreach (Control ctl in panel17.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkStomachTissueEtc7")
                        {
                            FstrAnatRes += txtStomachTissueEtc.Text + ",";
                        }
                        else
                        {
                            FstrAnatRes += chk.Text + ",";
                        }
                    }
                }
            }

            if (FstrAnatRes != "" && VB.Right(FstrAnatRes, 1) == ",")
            {
                FstrAnatRes = VB.Left(FstrAnatRes, FstrAnatRes.Length - 1);
            }

            rResultChange(strCodeAnat1, FstrAnatRes);
            rResultChange(strCodeAnat2, FstrAnatRes);
            rResultChange(strCodeAnat3, FstrAnatRes);
            rResultChange(strCodeAnat4, FstrAnatRes);
            rResultChange(strCodeAnat5, FstrAnatRes);
        }

        //대장암
        private void eInputColonRes(object sender, EventArgs e)
        {
            string strCode = "TX32";
            string strcode1 = "TX70";

            FstrColonRes = "";
            FstrColonRes1 = "";

            //2021-08-06
            if (cboColonoScope0.Text.Trim() != "" && VB.Left(cboColonoScope0.Text.Trim(), 1) != "5") { FstrColonRes += VB.Pstr(cboColonoScope0.Text, ".", 2) + ","; }    //관찰소견1
            if (cboColonoScope1.Text.Trim() != "" && VB.Left(cboColonoScope1.Text.Trim(), 1) != "5") { FstrColonRes += VB.Pstr(cboColonoScope1.Text, ".", 2) + ","; }    //관찰소견2
            if (cboColonoScope2.Text.Trim() != "" && VB.Left(cboColonoScope2.Text.Trim(), 1) != "5") { FstrColonRes += VB.Pstr(cboColonoScope2.Text, ".", 2) + ","; }    //관찰소견3

            //기타소견
            foreach (Control ctl in panel29.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkColonoScopeEtc9")
                        {
                            FstrColonRes += txtColonScopeEtc.Text + ",";
                        }
                        else
                        {
                            FstrColonRes += chk.Text + ",";
                        }
                    }
                }
            }

            if (FstrColonRes != "" && VB.Right(FstrColonRes, 1) == ",")
            {
                FstrColonRes = VB.Left(FstrColonRes, FstrColonRes.Length - 1);
            }

            //조직
            if (cboColonTissue.Text.Trim() != "" && VB.Pstr(cboColonTissue.Text, ".", 1) != "07" && VB.Pstr(cboColonTissue.Text, ".", 1) != "08")
            {
                FstrColonRes1 += VB.Pstr(cboColonTissue.Text, ".", 2).Trim() + ",";
            }

            //조직진단
            foreach (Control ctl in panel94.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkColonCancer12")
                        {
                            FstrColonRes1 += txtColonEtc.Text + ",";
                        }
                        else
                        {
                            FstrColonRes1 += chk.Text + ",";
                        }
                    }
                }
            }

            //조직진단 기타
            foreach (Control ctl in panel95.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked)
                    {
                        if (chk.Name == "chkColonCancerEtc4")
                        {
                            FstrColonRes1 += txtColonCancerEtc.Text + ",";
                        }
                        else
                        {
                            FstrColonRes1 += chk.Text + ",";
                        }
                    }
                }
            }

            if (FstrColonRes1 != "" && VB.Right(FstrColonRes1, 1) == ",")
            {
                FstrColonRes1 = VB.Left(FstrColonRes1, FstrColonRes1.Length - 1);
            }

            rResultChange(strCode, FstrColonRes);
            rResultChange(strcode1, FstrColonRes1);
            
        }
        private void eInputLiverRes(object sender, EventArgs e)
        {
            bool bLiver = false;
            string strCode = "TX10";
            string strCode1 = "TX09";

            FstrLiverRes = "";

            //간실질
            if (chkResult00040.Checked) { FstrLiverRes += chkResult00040.Text + ","; }

            if (chkResult00041.Checked) { FstrLiverRes += chkResult00041.Text + ","; }
            if (chkResult00042.Checked) { FstrLiverRes += "거친에코상,"; }
            if (chkResult00043.Checked) { FstrLiverRes += chkResult00043.Text + ","; }

            //간종괴
            if (chkResult00150.Checked && bLiver == false)
            {
                FstrLiverRes += "간낭종,";
            }

            //1cm미만 종괴 병변위치
            foreach (Control ctl in grpBox1cmLowPos.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked && bLiver == false)
                    {
                        FstrLiverRes += "간종괴,";
                        bLiver = true;
                    }
                }
            }

            //1cm이상 종괴 병변위치
            foreach (Control ctl in grpBox1cmHighPos.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "checkbox")
                {
                    CheckBox chk = ctl as CheckBox;

                    if (chk.Checked && bLiver == false)
                    {
                        FstrLiverRes += "간종괴,";
                        bLiver = true;
                    }
                }
            }

            //간암기타소견
            if (chkResult00100.Checked) { FstrLiverRes += chkResult00100.Text + ","; }
            if (chkResult00101.Checked) { FstrLiverRes += chkResult00101.Text + ","; }
            if (chkResult00102.Checked) { FstrLiverRes += chkResult00102.Text + ","; }
            if (chkResult00103.Checked) { FstrLiverRes += chkResult00103.Text + ","; }
            if (chkResult00104.Checked) { FstrLiverRes += chkResult00104.Text + ","; }
            if (chkResult00105.Checked) { FstrLiverRes += chkResult00105.Text + ","; }
            if (chkResult00106.Checked) { FstrLiverRes += chkResult00106.Text + ","; }
            if (chkResult00107.Checked) { FstrLiverRes += txtResult0012.Text + ","; }
            if (chkResult00108.Checked) { FstrLiverRes += chkResult00108.Text + ","; }

            if (FstrLiverRes != "" && VB.Right(FstrLiverRes, 1) == ",")
            {
                FstrLiverRes = VB.Left(FstrLiverRes, FstrLiverRes.Length - 1);
            }

            if (VB.L(FstrLiverRes, "정상") > 1 && FstrLiverRes.Length > 5)
            {
                FstrLiverRes = VB.Replace(FstrLiverRes, "정상,", "");
            }

            rResultChange(strCode, FstrLiverRes);
            rResultChange(strCode1, FstrLiverRes);
        }

        private void eInputBreastRes(object sender, EventArgs e)
        {
            string strExCode = "TX29";
            string strExCode1 = "TY16";
            string strExCode2 = "TY17";
            string strExResult = "";

            if (cboBreast.SelectedIndex == 3 || cboBreast.SelectedIndex == 4)
            {
                strExResult = "치밀유방,";
            }
            else if (cboBreast.SelectedIndex == 5)
            {
                strExResult = VB.Pstr(cboBreast.Text, ".", 2).Trim() + ",";
            }

            if (cboBreast0.SelectedIndex == 1)
            {
                if (strExResult.IsNullOrEmpty()) { strExResult = "이상소견없음"; }
            }
            else if (cboBreast0.SelectedIndex > 1 && cboBreast0.SelectedIndex < 10)
            {
                foreach (Control ctl in panel58.Controls)
                {
                    if (ctl.GetType().Name.ToLower() == "checkbox")
                    {
                        CheckBox chk = ctl as CheckBox;

                        if (chk.Checked)
                        {
                            strExResult += "우측유방 " + VB.Pstr(cboBreast0.Text, ".", 2).Trim() + ",";
                            break;
                        }
                    }
                }

                foreach (Control ctl in panel73.Controls)
                {
                    if (ctl.GetType().Name.ToLower() == "checkbox")
                    {
                        CheckBox chk = ctl as CheckBox;

                        if (chk.Checked)
                        {
                            strExResult += "좌측유방 " + VB.Pstr(cboBreast0.Text, ".", 2).Trim() + ",";
                            break;
                        }
                    }
                }
            }
            else if (cboBreast0.SelectedIndex == 10)
            {
                foreach (Control ctl in panel58.Controls)
                {
                    if (ctl.GetType().Name.ToLower() == "checkbox")
                    {
                        CheckBox chk = ctl as CheckBox;

                        if (chk.Checked)
                        {
                            if (cboBreast0.SelectedIndex != 10)
                            {
                                strExResult += "우측유방 ";
                            }
                            break;
                        }
                    }
                }

                foreach (Control ctl in panel73.Controls)
                {
                    if (ctl.GetType().Name.ToLower() == "checkbox")
                    {
                        CheckBox chk = ctl as CheckBox;

                        if (chk.Checked)
                        {
                            if (cboBreast0.SelectedIndex != 10)
                            {
                                if (strExResult.IsNullOrEmpty())
                                {
                                    strExResult += "좌측유방 ";
                                }
                                else
                                {
                                    strExResult = "양측유방 ";
                                }
                            }

                            break;
                        }
                    }
                }

                strExResult += txtBreastReadOpinionEtc.Text;
            }

            rResultChange(strExCode, strExResult);
            rResultChange(strExCode1, strExResult);
            rResultChange(strExCode2, strExResult);
        }

        private void eInputLungRes(object sender, EventArgs e)
        {
            string strExCode = "TY10";
            string strExResult = "";
            
            if (cboLPan1.SelectedIndex > 0)
            {
                strExResult = VB.Pstr(cboLPan1.Text, ".", 2).Trim() + ",";
            }

            if (rdoResult61.Checked)
            {
                if (cboLPan1.SelectedIndex > 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }

                strExResult += "기관지내 병변 위치: ";
                strExResult += txtResult11.Text + ",";
            }

            for (int i = 1; i <= 3; i++)
            {
                CheckBox chkRes = (Controls.Find("chkResult20" + i.ToString(), true)[0] as CheckBox);

                if (chkRes.Checked)
                {
                    if (cboLPan1.SelectedIndex > 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }
                    strExResult += chkRes.Text + ",";
                }
            }

            if (chkResult204.Checked)
            {
                if (cboLPan1.SelectedIndex > 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }
                strExResult += txtResult4.Text + ",";
            }

            for (int i = 1; i <= 7; i++)
            {
                CheckBox chkRes = (Controls.Find("chkResult21" + i.ToString(), true)[0] as CheckBox);

                if (chkRes.Checked)
                {
                    if (cboLPan1.SelectedIndex > 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }
                    strExResult += VB.Pstr(chkRes.Text, "(", 1).Trim() + ",";
                }
            }

            if (chkResult218.Checked)
            {
                if (cboLPan1.SelectedIndex > 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }
                strExResult += txtResult5.Text + ",";
            }

            //이상소견없음 삭제(2021-04-22)
            if ((chkResult200.Checked == false) && (chkResult201.Checked == false) && (chkResult202.Checked == false) && (chkResult203.Checked == false) &&
                (chkResult210.Checked == false) && (chkResult211.Checked == false) && (chkResult212.Checked == false) && (chkResult213.Checked == false) &&
                (chkResult214.Checked == false) && (chkResult215.Checked == false) && (chkResult216.Checked == false) && (chkResult217.Checked == false))
            {
                if (cboLPan1.SelectedIndex == 1) { strExResult = VB.Replace(strExResult, "이상소견없음,", ""); }
            }


            rResultChange(strExCode, strExResult);
        }

        void eTabClick(object sender, EventArgs e)
        {
            fn_FunctionKey_Enabled();

            if (sender == tab1)
            {
                panList01_Control_Set("2");
            }
            else if (sender == tab2)
            {
                btnF1.Visible = false;
                btnF2.Visible = false;
                btnF3.Visible = false;
                btnF4.Visible = false;
                btnF5.Visible = false;
                btnF6.Visible = false;
                btnF7.Visible = false;
                panList01_Control_Set("2");
            }
            else if (sender == tab3)
            {
                btnF5.Visible = false;
                btnF6.Visible = false;
                btnF7.Visible = false;
                panList01_Control_Set("2");
            }
            else if (sender == tab4)
            {
                btnF5.Visible = false;
                btnF6.Visible = false;
                btnF7.Visible = false;
                panList01_Control_Set("2");
            }
            else if (sender == tab5)
            {
                btnF5.Visible = false;
                btnF6.Visible = false;
                btnF7.Visible = false;
                panList01_Control_Set("2");
            }
            else if (sender == tab6)
            {
                btnF1.Visible = false;
                btnF2.Visible = false;
                btnF3.Visible = false;
                btnF4.Visible = false;
                btnF5.Visible = false;
                btnF6.Visible = false;
                btnF7.Visible = false;
                panList01_Control_Set("2");
            }
            else if (sender == rTab_List)
            {
                panList01_Control_Set("1");
            }
            else if (sender == rTab_Info)
            {
                panList01_Control_Set("2");
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strJepDate = "";
            string strSex = "";

            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());
            hb.Read_DrCode(clsHcVariable.GnHicLicense1);

            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_Control_Set_Stomach();   //위암 화면구성
            fn_Control_Set_Colon();     //대장암 화면구성
            fn_Control_Set_Liver();     //간암 화면구성
            fn_Control_Set_Breast();    //유방암 화면구성
            fn_Control_Set_Woman();     //자궁경부암 화면구성

            fn_Screen_Clear();

            if (clsHcVariable.GnHicLicense > 0)
            {
                btnSave.Enabled = true;
                btnSave.Visible = true;
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                    chkMirGbn.Visible = false;
                }

                ssList.ActiveSheet.Columns.Get(6).Visible = false;
                ssList.ActiveSheet.Columns.Get(7).Visible = false;
                ssList.ActiveSheet.Columns.Get(8).Visible = false;
                ssList.ActiveSheet.Columns.Get(9).Visible = false;
                ssList.ActiveSheet.Columns.Get(10).Visible = false;
                //ssList.ActiveSheet.Columns.Get(11).Visible = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.Visible = false;
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                    chkMirGbn.Visible = false;
                }
            }

            //strJepDate = list.JEPDATE;
            //strSex = list.SEX;

            FstrJepDate = strJepDate;
            FstrSex = strSex;

            panList01_Control_Set("1");

            //fn_Screen_Display();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argJob"></param>
        private void panList01_Control_Set(string argJob)
        {
            switch (argJob)
            {
                case "1":   //명단조회
                    ssList.Parent = panList01;
                    ssList.Dock = DockStyle.Fill;
                    ssList.Visible = true;

                    SS2.Dock = DockStyle.None;
                    SS2.Visible = false;
                    break;
                case "2":   //검사항목 조회
                    ssList.Dock = DockStyle.None;
                    ssList.Visible = false;

                    SS2.Parent = panList01;
                    SS2.Dock = DockStyle.Fill;
                    SS2.Visible = true;
                    break;
                default:
                    break;
            }
        }

        void eChkChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked) { return; }

            if (sender == chkCancer0)
            {
                tab1.Enabled = true;
                eTabClick(tab1, new EventArgs());
            }
            else if(sender == chkCancer1)
            {
                tab2.Enabled = true;
                eTabClick(tab2, new EventArgs());
            }
            else if(sender == chkCancer2)
            {
                tab3.Enabled = true;
                eTabClick(tab3, new EventArgs());
            }
            else if (sender == chkCancer3)
            {
                tab4.Enabled = true;
                eTabClick(tab4, new EventArgs());
            }
            else if(sender == chkCancer4)
            {
                tab5.Enabled = true;
                eTabClick(tab5, new EventArgs());
            }
            else if(sender == chkCancer5)
            {
                tab6.Enabled = true;
                eTabClick(tab6, new EventArgs());
            }
        }

        private void eChkChanged2(object sender, EventArgs e)
        {
            if (sender == chkWomBo4)
            {
                if (((CheckBox)sender).Checked == false)
                {
                    rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                    chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                }
            }
            else if (sender == chkWomBo6)
            {
                if (((CheckBox)sender).Checked == false)
                {
                    rdoWomBo61.Checked = false; rdoWomBo62.Checked = false; rdoWomBo63.Checked = false;
                    chkResult00141.Checked = false; chkResult00142.Checked = false;
                }
            }
            else if (sender == chkResult00040)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult00041.Checked = false; chkResult00042.Checked = false; chkResult00043.Checked = false;
                    chkResult00150.Checked = false;

                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkResult0005 = (Controls.Find("chkResult0005" + i.ToString(), true)[0] as CheckBox);
                        CheckBox chkResult0006 = (Controls.Find("chkResult0006" + i.ToString(), true)[0] as CheckBox);
                        chkResult0005.Checked = false;
                        chkResult0006.Checked = false;
                    }

                    txtResult0007.Text = ""; txtResult0008.Text = ""; txtResult0009.Text = "";
                }
            }
            else if (sender == chkResult00041 || sender == chkResult00042 || sender == chkResult00043 || sender == chkResult00150)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult00040.Checked = false;
                }
            }
            else if (VB.Left(((CheckBox)sender).Name, 13) == "chkResult0005" || VB.Left(((CheckBox)sender).Name, 13) == "chkResult0006")
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult00040.Checked = false;
                }
            }
            else if (VB.Left(((CheckBox)sender).Name, 11) == "chkResult01")
            {
                if (((CheckBox)sender).Checked == true)
                {
                    string strInx = VB.Right(((CheckBox)sender).Name, 1);

                    Panel panResult01 = (Controls.Find("panResult01" + strInx, true)[0] as Panel);

                    Control[] ctrls = ComFunc.GetAllControlsUsingRecursive(panResult01);

                    foreach (Control ctl in ctrls)
                    {
                        if (ctl.GetType().Name.ToLower() == "checkbox")
                        {
                            CheckBox chk = ctl as CheckBox;
                            chk.Checked = false;
                        }
                        else if (ctl.GetType().Name.ToLower() == "textbox")
                        {
                            TextBox txt = ctl as TextBox;
                            ctl.Text = "";
                        }
                    }
                }
            }
            else if (sender == chkResult200)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult201.Checked = false; chkResult202.Checked = false;
                    chkResult203.Checked = false; chkResult204.Checked = false;

                    txtResult4.Text = "";
                }
            }
            else if (sender == chkResult201 || sender == chkResult202 || sender == chkResult203 || sender == chkResult204)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult200.Checked = false;
                }
            }
            else if (sender == chkResult210)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult211.Checked = false; chkResult212.Checked = false;
                    chkResult213.Checked = false; chkResult214.Checked = false;
                    chkResult215.Checked = false; chkResult216.Checked = false;
                    chkResult217.Checked = false; chkResult218.Checked = false;

                    txtResult5.Text = "";
                }
            }
            else if (sender == chkResult211 || sender == chkResult212 || sender == chkResult213 || sender == chkResult214
                || sender == chkResult215 || sender == chkResult216 || sender == chkResult217 || sender == chkResult218)
            {
                if (((CheckBox)sender).Checked == true)
                {
                    chkResult210.Checked = false;
                }
            }
        }

        private void eChkChanged3(object sender, EventArgs e)
        {
            if (VB.Left(((RadioButton)sender).Name, 11) == "chkResult01")
            {
                if (((RadioButton)sender).Checked == true)
                {
                    string strInx = VB.Right(((RadioButton)sender).Name, 1);

                    Panel panResult01 = (Controls.Find("panResult01" + strInx, true)[0] as Panel);

                    Control[] ctrls = ComFunc.GetAllControlsUsingRecursive(panResult01);

                    foreach (Control ctl in ctrls)
                    {
                        if (ctl.GetType().Name.ToLower() == "checkbox")
                        {
                            CheckBox chk = ctl as CheckBox;
                            chk.Checked = false;
                        }
                        else if (ctl.GetType().Name.ToLower() == "textbox")
                        {
                            TextBox txt = ctl as TextBox;
                            ctl.Text = "";
                        }
                    }

                    panResult01.Enabled = false;
                }
            }
            else if (VB.Left(((RadioButton)sender).Name, 11) == "chkResult02" || VB.Left(((RadioButton)sender).Name, 11) == "chkResult03")
            {
                if (((RadioButton)sender).Checked == true)
                {
                    string strInx = VB.Right(((RadioButton)sender).Name, 1);

                    Panel panResult01 = (Controls.Find("panResult01" + strInx, true)[0] as Panel);

                    panResult01.Enabled = true;

                    CheckBox chkResult14 = (Controls.Find("chkResult14" + strInx, true)[0] as CheckBox);
                    chkResult14.Checked = true;
                    CheckBox chkResult17 = (Controls.Find("chkResult17" + strInx, true)[0] as CheckBox);
                    chkResult17.Checked = true;
                }
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {

            List<string> strMcodes = new List<string>();

            if (sender == rdoColonTissue0)          //대장조직
            {
                //lblDrName6.Text = "김미진";
                //SSDR.ActiveSheet.Cells[5, 0].Text = "30846";

                strMcodes.Clear();
                strMcodes.Add("XR14");
                strMcodes.Add("XR27");
                strMcodes.Add("XR31");

                EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                if (!item1.IsNullOrEmpty())
                {
                    clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                    hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                    lblDrName6.Text = clsHcVariable.GstrHicDrName;
                    SSDR.ActiveSheet.Cells[5, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                }

                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;
            }
            else if (sender == rdoColonTissue1)
            {
                lblDrName6.Text = "";
                SSDR.ActiveSheet.Cells[5, 0].Text = "";
                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;
            }
            else if (sender == rdoStomachTissue0)   //위조직
            {
                //lblDrName3.Text = "김미진";
                //SSDR.ActiveSheet.Cells[2, 0].Text = "30846";

                strMcodes.Clear();
                strMcodes.Add("XR20");
                strMcodes.Add("XR31");
                strMcodes.Add("XR34");

                EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                if (!item1.IsNullOrEmpty())
                {
                    clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                    hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                    lblDrName3.Text = clsHcVariable.GstrHicDrName;
                    SSDR.ActiveSheet.Cells[2, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                }


                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;
            }
            else if (sender == rdoStomachTissue1)
            {
                lblDrName3.Text = "";
                SSDR.ActiveSheet.Cells[2, 0].Text = "";
                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;
            }
            else if (sender == rdoWomBo11)
            {
                //lblDrName10.Text = "김미진";
                //SSDR.ActiveSheet.Cells[9, 0].Text = "30846";

                strMcodes.Clear();
                strMcodes.Add("XR20");
                strMcodes.Add("XR31");
                strMcodes.Add("XR34");

                EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                if (!item1.IsNullOrEmpty())
                {
                    clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                    hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                    lblDrName10.Text = clsHcVariable.GstrHicDrName;
                    SSDR.ActiveSheet.Cells[9, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                }

                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;

                FstrWomBoCode = "A163";
                FstrWomBo = "01";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo12)
            {
                lblDrName10.Text = "";
                SSDR.ActiveSheet.Cells[9, 0].Text = "";
                clsHcVariable.GstrHicDrName = "";
                clsHcVariable.GnHicLicense1 = 0;

                FstrWomBoCode = "A163";
                FstrWomBo = "02";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo21)
            {
                FstrWomBoCode = "A164";
                FstrWomBo = "01";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo22)
            {
                FstrWomBoCode = "A164";
                FstrWomBo = "02";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo31)
            {
                chkWomBo4.Checked = false;
                rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                chkWomBo6.Checked = false;
                rdoWomBo61.Checked = false; rdoWomBo62.Checked = false; rdoWomBo63.Checked = false;
                chkResult00141.Checked = false; chkResult00142.Checked = false;
                rdoWomBo64.Checked = false; txtWombo6_Etc.Text = "";
                txtWombo3_Etc.Text = "";
                panWomBo3.Enabled = false;

                FstrWomBoCode = "A165";
                FstrWomBo = "01";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo32)
            {
                panWomBo3.Enabled = true; 
                FstrWomBoCode = "A165";
                FstrWomBo = "02";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo33)
            {
                chkWomBo4.Checked = false;
                rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                chkWomBo6.Checked = false;
                rdoWomBo61.Checked = false; rdoWomBo62.Checked = false; rdoWomBo63.Checked = false;
                chkResult00141.Checked = false; chkResult00142.Checked = false;
                rdoWomBo64.Checked = false; txtWombo6_Etc.Text = "";
                txtWombo3_Etc.Text = "";
                panWomBo3.Enabled = false;

                txtWombo3_Etc.Enabled = true;

                FstrWomBoCode = "A165";
                FstrWomBo = "03";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo41)
            {
                FstrWomBoCode = "A166";
                FstrWomBo = "01";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo42)
            {
                FstrWomBoCode = "A166";
                FstrWomBo = "02";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo43)
            {
                FstrWomBoCode = "A166";
                FstrWomBo = "03";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo44)
            {
                FstrWomBoCode = "A166";
                FstrWomBo = "04";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo61)
            {
                FstrWomBoCode = "A167";
                FstrWomBo = "01";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo62)
            {
                FstrWomBoCode = "A167";
                FstrWomBo = "02";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo63)
            {
                FstrWomBoCode = "A167";
                FstrWomBo = "03";

                rResultChange(FstrWomBoCode, FstrWomBo);
            }
            else if (sender == rdoWomBo64)
            {
                chkResult00141.Checked = false;
                chkResult00142.Checked = false;

                txtWombo6_Etc.Enabled = true;
                FstrWomBoCode = "A167"; FstrWomBo = "04";
                rResultChange(FstrWomBoCode, FstrWomBo);

            }
            else if (sender == rdoWomBo71)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "01";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo72)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "02";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo73)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "03";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo74)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "04";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo75)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "05";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo76)
            {
                FstrWomBoCode = "A168"; FstrWomBo = "06";
                rResultChange(FstrWomBoCode, FstrWomBo);
                rResultChange("A170", "");
            }
            else if (sender == rdoWomBo77)
            {
                txtWombo7_Etc.Enabled = true;

                FstrWomBoCode = "A168"; FstrWomBo = "07";
                rResultChange(FstrWomBoCode, FstrWomBo);
            }
        }

        void fn_Screen_Display()
        {
            //삭제된 것 체크
            if (hb.READ_JepsuSTS(FnWrtNo) == "D" || hb.READ_JepsuSTS(FnWrtNo).IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 : " + FnWrtNo + " 는 삭제되었거나 접수되지 않은 번호입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrJob = rdoJob1.Checked ? "1" : rdoJob2.Checked ? "2" : "";

            tabDefalut.Visible = false;

            fn_Screen_Cancer_Display();
            fn_Screen_Munjin_Display();   //암검진 문진 Display(기존 암환자 표시, 청구제외 표시)
            
            //대장암 검사결과(정량검사) txtResult 값 Read
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNotExCode(FnWrtNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].EXCODE == "TX26")
                    {
                        txtResult.Text = "검사결과: " + list[i].RESULT + "ng/ml [참고치:(0~100 ng/ml 이하)]";
                        txtResult.Enabled = false;
                    }

                    if (list[i].EXCODE == "A264")
                    {
                        txtEIA.Text = list[i].RESULT;
                        txtEIA1.Text = "검사결과: " + list[i].RESULT + " ng/ml";
                        txtEIA1.Enabled = false;
                    }
                }
            }

            fn_Screen_Panjeng_Display();    //암판정 Display

            //fn_Result_Save();
        }
        
        void fn_Screen_Munjin_Display()
        {
            string strROWID = "";

            HIC_CANCER_NEW list = hicCancerNewService.GetItemByWrtno(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("판정 및 문진내역이 없습니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //암검진 청구제외구분
            if (!list.CAN_MIRGBN.IsNullOrEmpty())
            {
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                    if (VB.Mid(list.CAN_MIRGBN, i + 1, 1) == "1")
                    {
                        chkMirGbn.Checked = true;
                    }
                    else
                    {
                        chkMirGbn.Checked = false;
                    }
                }
            }

            //출력이 완료되면 판정수정 및 저장불가
            if (list.GBPRINT.To<string>("").Trim() == "Y")
            {
                btnSave.Enabled = false;
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.Visible = true;
            }

            if (VB.Mid(list.NEW_SICK03, 1, 1) == "2") { chkAm0.Checked = true; }
            else { chkAm0.Checked = false; }

            if (VB.Mid(list.NEW_SICK08, 1, 1) == "2") { chkAm3.Checked = true; }
            else { chkAm3.Checked = false; }

            if (VB.Mid(list.NEW_SICK13, 1, 1) == "2") { chkAm1.Checked = true; }
            else { chkAm1.Checked = false; }

            if (VB.Mid(list.NEW_SICK18, 1, 1) == "2") { chkAm4.Checked = true; }
            else { chkAm4.Checked = false; }

            if (VB.Mid(list.NEW_SICK23, 1, 1) == "2") { chkAm2.Checked = true; }
            else { chkAm2.Checked = false; }

            if (VB.Mid(list.NEW_SICK76, 1, 1) == "2") { chkAm5.Checked = true; }
            else { chkAm5.Checked = false; }

            //문진뷰어 실행
            if (chkMunjin.Checked)
            {
                //상담내역이 있는지 점검
                HIC_SANGDAM_NEW list2 = hicSangdamNewService.GetItembyWrtNo(FnWrtNo);

                if (!list2.IsNullOrEmpty())
                {
                    strROWID = list2.RID;
                }
                else
                {
                    strROWID = "";
                }

                //검진문진뷰어
                DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                if (dir.Exists == true)
                {
                    VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH + " " + FstrPtno + "/", "NormalFocus");
                }
                else
                {
                    DirectoryInfo dir1 = new DirectoryInfo(@"C:\Program Files (x86)\SamOmr\");
                    if (dir1.Exists == true)
                    {
                        VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH_64 + " " + FstrPtno + "/", "NormalFocus");
                    }
                }

                Form frmMunJinView = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (frmMunJinView != null)
                {
                    frmMunJinView.Close();
                    frmMunJinView.Dispose();
                    frmMunJinView = null;
                }

                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWrtNo, FstrJepDate, FstrPtno, "31", strROWID, 0);
                FrmHcSangInternetMunjinView.Show();
                FrmHcSangInternetMunjinView.WindowState = FormWindowState.Minimized;
            }
        }

        void fn_Screen_Cancer_Display()
        {
            int k = 0;
            int kk = 0;
            int nRead = 0;
            string strOK = "";

            strOK = "";

            //암검사 종류 체크
            List<HIC_SUNAPDTL_GROUPCODE> list = hicSunapdtlGroupcodeService.GetGbSelfGbAmbyWrtNo(FnWrtNo);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                if (!list[i].GBAM.IsNullOrEmpty())
                {
                    for (int j = 1; j <= 7; j++)
                    {
                        if (VB.Pstr(list[i].GBAM, ",", j) == "1")
                        {
                            if (j == 1 || j == 2)
                            {
                                chkCancer0.Checked = true;
                                k = 0;
                                if (strOK == "")
                                {
                                    kk = 0;
                                }
                                if (superTabControl1.SelectedTabIndex == 0)
                                {
                                    superTabControl1.SelectedTabIndex = k + 1;

                                    if (list[i].GBSELF.To<long>() == 2 || list[i].GBSELF.To<long>() == 3)
                                    {
                                        chkMirGbn0.Checked = true;
                                    }
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                CheckBox chkCancer = (Controls.Find("chkCancer" + (j - 2).ToString(), true)[0] as CheckBox);
                                chkCancer.Checked = true;
                                k = j - 2;
                                if (strOK == "") kk = j - 2;
                                if (superTabControl1.SelectedTabIndex == 0)
                                {
                                    superTabControl1.SelectedTabIndex = k + 1;
                                }
                                if (list[i].GBSELF.To<long>() == 2 || list[i].GBSELF.To<long>() == 3)
                                {
                                    CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + (j - 2).ToString(), true)[0] as CheckBox);
                                    chkMirGbn.Checked = true;
                                }
                                strOK = "OK";
                            }
                        }
                    }
                }
            }

            //세부암 종류
            //위조영

            string[] strExCode = { "TX22" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode) > 0)
            {
                tabStomach1.Visible = true;
                TabStomach.SelectedTab = tabStomach1;
            }

            //위내시경
            string[] strExCode1 = { "TX20", "TX23", "TX41" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode1) > 0)
            {
                tabStomach2.Visible = true;
                TabStomach.SelectedTab = tabStomach2;
            }

            //분변잠혈
            string[] strExCode2 = { "TX26" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode2) > 0)
            {
                tabColon1.Visible = true;
                TabColono.SelectedTabIndex = 0;

                cboCPan.Items.Clear();
                cboCPan.Items.Add("");
                cboCPan.Items.Add("6.잠혈반응없음");
                cboCPan.Items.Add("7.잠혈반응있음");
                cboCPan.SelectedIndex = 0;
            }

            //대장조영
            string[] strExCode3 = { "TX31" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode3) > 0)
            {
                tabColon2.Visible = true;
                TabColono.SelectedTabIndex = 1;

                cboCPan.Items.Clear();
                cboCPan.Items.Add("");
                cboCPan.Items.Add("1.이상소견없음");
                cboCPan.Items.Add("2.대장용종");
                cboCPan.Items.Add("3.대장암 의심");
                cboCPan.Items.Add("4.대장암");
                cboCPan.Items.Add("5.기타");
                cboCPan.SelectedIndex = 0;
            }

            //대장내시경
            string[] strExCode4 = { "TX32", "TX64", "TX41" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode4) > 0)
            {
                tabColon3.Visible = true;
                TabColono.SelectedTabIndex = 2;

                cboCPan.Items.Clear();
                cboCPan.Items.Add("");
                cboCPan.Items.Add("1.이상소견없음");
                cboCPan.Items.Add("2.대장용종");
                cboCPan.Items.Add("3.대장암 의심");
                cboCPan.Items.Add("4.대장암");
                cboCPan.Items.Add("5.기타");
                cboCPan.SelectedIndex = 0;
            }

            //간염검사
            string[] strExCode5 = { "TX27", "TX10", "TX09" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode5) > 0)
            {
                grpLiverUltrasonography1.Enabled = true;
                grpLiverUltrasonography2.Enabled = true;
            }

            //간초음파검사
            string[] strExCode6 = { "A258" };
            if (hicResultService.GetCntbyWrtNoExCode(FnWrtNo, strExCode6) > 0)
            {
                grpHepatitisExam.Enabled = true;
            }

            //상담구분 체크
            //chkSangDam[kk].Value = true;

            switch (kk)
            {
                case 0:
                    chkSangdam0.Checked = true;
                    break;
                case 1:
                    chkSangdam1.Checked = true;
                    break;
                case 2:
                    chkSangdam2.Checked = true;
                    break;
                case 3:
                    chkSangdam3.Checked = true;
                    break;
                case 4:
                    chkSangdam4.Checked = true;
                    break;
                case 5:
                    chkSangdam5.Checked = true;
                    break;
                default:
                    break;
            }

            //문진표 DISPLAY ==========================================================================================================
            HIC_CANCER_NEW list2 = hicCancerNewService.GetItemByWrtno(FnWrtNo);

            if (list2.IsNullOrEmpty())
            {
                MessageBox.Show("판정 및 문진내역이 없습니다!", "수검상태 확인요망!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //tWrtNo.Focus();
                return;
            }

            //암검진 청구제외구분
            if (!list2.CAN_MIRGBN.IsNullOrEmpty())
            {
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                    if (VB.Mid(list2.CAN_MIRGBN, i + 1, 1) == "1")
                    {
                        chkMirGbn.Checked = true;
                    }
                    else
                    {
                        chkMirGbn.Checked = false;
                    }
                }
            }

            //출력이 완료되면 판정수정 및 저장불가
            if (list2.GBPRINT.To<string>("").Trim() == "Y")
            {
                btnSave.Enabled = false;
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.Visible = true;
            }

            //2021-04-08
            //for (int i = 0; i < superTabControl1.Tabs.Count; i++)
            //{
            //    if (superTabControl1.Tabs[i].Visible == true)
            //    {
            //        eTabClick(superTabControl1.Tabs[i], new EventArgs());
            //        break;
            //    }
            //}
        }

        /// <summary>
        /// 암판정 Display
        /// </summary>
        void fn_Screen_Panjeng_Display()
        {
            List<string> strMcodes = new List<string>();

            int nx = 0;
            string str위조영 = "";
            string str위내시경 = "";
            string str위조영기타 = "";
            string str위내시경기타 = "";
            string strPositS1 = "";
            string strPositS2 = "";
            string str대장조영 = "";
            string str대장내시경 = "";
            string str결장조영기타 = "";
            string str대장내시경기타 = "";
            string strPositC1 = "";
            string strPositC2 = "";
            
            string str유방판독소견 = "";
            string strPositB = "";
            string strCODE = "";
            bool bOK = false;
            string strTemp = "";
            string strEntTime = "";
            string strDrCode = "";
            string strGBJIN = "";
            string strGBJIN2 = "";
            long nHicLicense1 = 0;

            string strResult0004 = "";
            string strResult0005 = "";
            string strResult0006 = "";
            string strResult0010 = "";
            string strResult0015 = "";
            string strResult0016 = "";
            string strXrayno = "";

            string[] strPanjengDrNo = new string[11];

            for (int i = 0; i < 11; i++)
            {
                strPanjengDrNo[i] = "";
            }

            strResult0004 = "";
            strResult0005 = "";
            strResult0006 = "";
            strResult0010 = "";
            strResult0016 = "";
            strEntTime = "";

            HIC_CANCER_NEW list = hicCancerNewService.GetItemByWrtno(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("판정 및 문진내역이 없습니다!!", "수검상태 확인요망!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //암판정 권고사항
            txtS_Sogen.Text = list.S_SOGEN.IsNullOrEmpty() == false ? list.S_SOGEN.Replace("\r\n", "") : "";
            txtS_Sogen2.Text = list.S_SOGEN2.IsNullOrEmpty() == false ? list.S_SOGEN2.Replace("\r\n", "") : "";
            txtC_Sogen.Text = list.C_SOGEN.IsNullOrEmpty() == false ? list.C_SOGEN.Replace("\r\n", "") : "";
            txtC_Sogen2.Text = list.C_SOGEN2.IsNullOrEmpty() == false ? list.C_SOGEN2.Replace("\r\n", "") : "";
            txtC_Sogen3.Text = list.C_SOGEN3.IsNullOrEmpty() == false ? list.C_SOGEN3.Replace("\r\n", "") : "";
            txtL_Sogen.Text = list.L_SOGEN.IsNullOrEmpty() == false ? list.L_SOGEN.Replace("\r\n", "") : "";
            txtB_Sogen.Text = list.B_SOGEN.IsNullOrEmpty() == false ? list.B_SOGEN.Replace("\r\n", "") : "";
            txtW_Sogen.Text = list.W_SOGEN.IsNullOrEmpty() == false ? list.W_SOGEN.Replace("\r\n", "") : "";

            //암판정 일자 세팅
            //if (chkCancer0.Checked == true) { dtpSPanDate.Text = list.S_PANJENGDATE.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.S_PANJENGDATE; }
            //if (chkCancer1.Checked == true) { dtpCPanDate.Text = list.C_PANJENGDATE.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.C_PANJENGDATE; }
            //if (chkCancer2.Checked == true) { dtpLPanDate.Text = list.L_PANJENGDATE.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.L_PANJENGDATE; }
            //if (chkCancer3.Checked == true) { dtpBPanDate.Text = list.B_PANJENGDATE.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.B_PANJENGDATE; }
            //if (chkCancer4.Checked == true) { dtpWPanDate.Text = list.W_PANJENGDATE.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.W_PANJENGDATE; }
            //if (chkCancer5.Checked == true) { dtpLPanDate1.Text = list.L_PANJENGDATE1.IsNullOrEmpty() ? clsPublic.GstrSysDate : list.L_PANJENGDATE1; }

            if (chkCancer0.Checked == true)
            {
                if (list.S_PANJENGDATE.IsNullOrEmpty())
                {
                    dtpSPanDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpSPanDate.Text = list.S_PANJENGDATE;
                }
            }

            if (chkCancer1.Checked == true)
            {
                if (list.C_PANJENGDATE.IsNullOrEmpty())
                {
                    dtpCPanDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpCPanDate.Value = Convert.ToDateTime(list.C_PANJENGDATE);
                }
            }

            if (chkCancer2.Checked == true)
            {
                if (list.L_PANJENGDATE.IsNullOrEmpty())
                {
                    dtpLPanDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpLPanDate.Text = list.L_PANJENGDATE;
                }
            }

            if (chkCancer3.Checked == true)
            {
                if (list.B_PANJENGDATE.IsNullOrEmpty())
                {
                    dtpBPanDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpBPanDate.Text = list.B_PANJENGDATE;
                }
            }

            if (chkCancer4.Checked == true)
            {
                if (list.W_PANJENGDATE.IsNullOrEmpty())
                {
                    dtpWPanDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpWPanDate.Text = list.W_PANJENGDATE;
                }
            }

            if (chkCancer5.Checked == true)
            {
                if (list.L_PANJENGDATE1.IsNullOrEmpty())
                {
                    dtpLPanDate1.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    dtpLPanDate1.Text = list.L_PANJENGDATE1;
                }
            }

            //암검진 DISPLAY=================================================================================
            //위
            //조직진단
            strCODE = list.RESULT0001.To<string>(""); ;

            if (strCODE.IsNullOrEmpty())
            {
                cboResult0001.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < cboResult0001.Items.Count; i++)
                {
                    if (VB.Left(cboResult0001.Items[i].To<string>(), 2) == strCODE)
                    {
                        cboResult0001.SelectedIndex = i;
                        break;
                    }
                }
            }

            strResult0016 = list.RESULT0016.To<string>(""); ;
            if (strResult0016 == "1")
            {
                chkResult0016.Checked = true;
            }
            else if (strResult0016 == "0")
            {
                chkResult0016.Checked = false;
            }

            //대장
            //맹장삽입여부
            if (list.RESULT0002 == "1")
            {
                rdoResult00020.Checked = true;
            }
            else if (list.RESULT0002 == "2")
            {
                rdoResult00021.Checked = true;
            }

            //장정결도
            rdoResult00031.Checked = true;
            if (list.RESULT0003 == "1")
            {
                rdoResult00030.Checked = true;
            }
            else if (list.RESULT0003 == "2")
            {
                rdoResult00031.Checked = true;
            }

            //간암
            strResult0004 = list.RESULT0004.To<string>(""); ;
            for (int i = 0; i < 4; i++)
            {
                CheckBox chkResult0004 = (Controls.Find("chkResult0004" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strResult0004, i + 1, 1) == "1")
                {
                    chkResult0004.Checked = true;
                }
                else if (VB.Mid(strResult0004, i + 1, 1) == "0")
                {
                    chkResult0004.Checked = false;
                }
            }

            strResult0015 = list.RESULT0015.To<string>("");

            for (int i = 0; i < 2; i++)
            {
                CheckBox chkResult0015 = (Controls.Find("chkResult0015" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strResult0015, i + 1, 1) == "1")
                {
                    chkResult0015.Checked = true;
                }
                else if (VB.Mid(strResult0015, i + 1, 1) == "0")
                {
                    chkResult0015.Checked = false;
                }
            }

            //간암초음파검사 병변부위
            strResult0005 = list.RESULT0005.To<string>(""); ;
            for (int i = 0; i < 8; i++)
            {
                CheckBox chkResult0005 = (Controls.Find("chkResult0005" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strResult0005, i + 1, 1) == "1")
                {
                    chkResult0005.Checked = true;
                }
                else if (VB.Mid(strResult0005, i + 1, 1) == "0")
                {
                    chkResult0005.Checked = false;
                }
            }

            strResult0006 = list.RESULT0006.To<string>(""); ;
            for (int i = 0; i < 8; i++)
            {
                CheckBox chkResult0006 = (Controls.Find("chkResult0006" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strResult0006, i + 1, 1) == "1")
                {
                    chkResult0006.Checked = true;
                }
                else if (VB.Mid(strResult0006, i + 1, 1) == "0")
                {
                    chkResult0006.Checked = false;
                }
            }
            txtResult0007.Text = list.RESULT0007;
            txtResult0008.Text = list.RESULT0008;
            txtResult0009.Text = list.RESULT0009;
            strResult0010 = list.RESULT0010.To<string>("").Trim();
            for (int i = 0; i <= 8; i++)
            {
                CheckBox chkResult0010 = (Controls.Find("chkResult0010" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strResult0010, i + 1, 1) == "1")
                {
                    chkResult0010.Checked = true;
                }
                else if (VB.Mid(strResult0010, i + 1, 1) == "0")
                {
                    chkResult0010.Checked = false;
                }
            }
            txtResult0011.Text = list.RESULT0011;
            txtResult0012.Text = list.RESULT0012;

            //유방
            //촬영부위
            if (list.RESULT0013 == "1" || list.RESULT0013.IsNullOrEmpty()) { cboResult0013.SelectedIndex = 1; }
            else if (list.RESULT0013 == "2") { cboResult0013.SelectedIndex = 2; }
            else if (list.RESULT0013 == "3") { cboResult0013.SelectedIndex = 3; }

            //자궁
            //비정상 선상피
            if (list.RESULT0014 == "1") { chkResult00141.Checked = true; }
            else if (list.RESULT0014 == "2") { chkResult00142.Checked = true; }

            #region 위암 ===========================================================================================================
            str위조영 = list.STOMACH_S;
            if (!str위조영.IsNullOrEmpty())
            {
                for (int i = 0; i < str위조영.Length; i++)
                {
                    ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + i.ToString(), true)[0] as ComboBox);
                    cboForgedYoung.SelectedIndex = VB.Mid(str위조영, i + 1, 1).To<int>();
                }
            }

            str위내시경 = list.STOMACH_SENDO;
            for (int i = 0; i < 3; i++)
            {
                ComboBox cboEndo = (Controls.Find("cboEndo" + i.ToString(), true)[0] as ComboBox);
                strCODE = VB.Mid(str위내시경, ((i + 1) * 2) - 1, 2);
                if (strCODE.IsNullOrEmpty())
                {
                    cboEndo.SelectedIndex = 0;
                }
                else
                {
                    for (int j = 0; j < cboEndo.Items.Count; j++)
                    {
                        if (VB.Left(cboEndo.Items[j].To<string>(), 2) == strCODE)
                        {
                            cboEndo.SelectedIndex = j;
                            break;
                        }
                    }
                }
            }

            txtForgedYoungEtc.Text = list.STOMACH_PETC; //위장조영촬영 병형기타
            txtEndoEtc.Text = list.STOMACH_ENDOETC;     //위장내시경 병형기타

            if (list.S_ENDOGBN == "1")
            {
                rdoStomachEndo0.Checked = true;
            }
            else if (list.S_ENDOGBN.IsNullOrEmpty() || list.S_ENDOGBN == "2")
            {
                rdoStomachEndo1.Checked = true;
            }

            //2019요청
            if (list.S_ANATGBN == "1")
            {
                rdoStomachTissue0.Checked = true;
                eRdoClick(rdoStomachTissue0, new EventArgs());
            }
            else if (list.S_ANATGBN.IsNullOrEmpty() || list.S_ANATGBN == "2")
            {
                rdoStomachTissue1.Checked = true;
            }

            if (!list.S_PANJENG.IsNullOrEmpty())
            {
                cboSPan.SelectedIndex = list.S_PANJENG.To<int>(); //위암종합판정
            }

            txtJilEtcS.Text = list.S_JILETC;    //기타질환()치료대상

            //위장조영촬영병형_기타
            str위조영기타 = list.STOMACH_B;
            for (int i = 0; i < 8; i++)
            {
                CheckBox chkForgedYoungEtc = (Controls.Find("chkForgedYoungEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(str위조영기타, i + 1, 1) == "1")
                {
                    chkForgedYoungEtc.Checked = true;
                }
                else if (VB.Mid(str위조영기타, i + 1, 1) == "0")
                {
                    chkForgedYoungEtc.Checked = false;
                }
            }

            //위내시경검사병형_기타
            str위내시경기타 = list.STOMACH_BENDO;
            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(str위내시경기타, i + 1, 1) == "1")
                {
                    chkEndoEtc.Checked = true;
                }
                else if (VB.Mid(str위내시경기타, i + 1, 1) == "0")
                {
                    chkEndoEtc.Checked = false;
                }
            }

            //위장조영촬영부위(1-8)
            strPositS1 = list.STOMACH_P;
            nx = 0;
            for (int i = 0; i < 24; i += 3)
            {
                strTemp = VB.Mid(strPositS1, i + 1, 3);
                for (int j = 0; j <= 2; j++)
                {
                    ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + j.ToString(), true)[0] as ComboBox);
                    if (cboForgedYoung.SelectedIndex > 0)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            if (VB.Mid(strTemp, k + 1, 1).To<int>() > 0)
                            {
                                if (cboForgedYoung.SelectedIndex == VB.Mid(strTemp, k + 1, 1).To<int>())
                                {
                                    CheckBox chkForgedYoung = (Controls.Find("chkForgedYoung" + ((j + 1) * 10 + nx).ToString(), true)[0] as CheckBox);
                                    chkForgedYoung.Checked = true;
                                }
                            }
                        }
                    }
                }
                nx += 1;
            }

            //위장내시경촬영부위(1-8)
            strPositS2 = list.STOMACH_PENDO;
            nx = 0;
            for (int i = 0; i < 24; i += 3)
            {
                strTemp = VB.Mid(strPositS2, i + 1, 3);
                for (int j = 0; j <= 2; j++)
                {
                    ComboBox cboEndo = (Controls.Find("cboEndo" + j.ToString(), true)[0] as ComboBox);
                    if (cboEndo.SelectedIndex > 0)
                    {
                        if (VB.Mid(strTemp, j + 1, 1).To<int>() > 0)
                        {
                            CheckBox chkEndo = (Controls.Find("chkEndo" + ((j + 1) * 10 + nx).ToString(), true)[0] as CheckBox);
                            chkEndo.Checked = true;
                        }
                    }
                }
                nx += 1;
            }

            //위암 조직진단
            if (!list.NEW_SICK54.IsNullOrEmpty())
            {
                cboStomachTissueCnt.SelectedIndex = list.NEW_SICK54.To<int>();
            }

            //위용종 개수
            if (hb.Smt_Cnt_Chek(FnWrtNo) > 0)
            {
                if (!list.NEW_SICK54.IsNullOrEmpty())
                {
                    cboStomachTissueCnt.SelectedIndex = list.NEW_SICK54.To<int>();
                }
                else
                {
                    cboStomachTissueCnt.SelectedIndex = hb.Smt_Cnt_Chek(FnWrtNo);
                }
            }

            //개수에서 조직진단으로 기준변경
            if (cboResult0001.SelectedIndex > 0)
            {
                rdoStomachEndo0.Checked = true;
            }

            //위암조직진단시-암
            strTemp = list.NEW_SICK63;
            for (int i = 0; i <= 13; i++)
            {
                CheckBox chkStomachTissue = (Controls.Find("chkStomachTissue" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strTemp, i + 1, 1) == "1")
                {
                    chkStomachTissue.Checked = true;
                }
                else
                {
                    chkStomachTissue.Checked = false;
                }
            }
            //위암조직진단시-암기타
            txtStomachTissueEtcCancer.Text = list.NEW_SICK66;

            //위암조직진단시-기타
            strTemp = list.NEW_SICK67;
            for (int i = 0; i < 8; i++)
            {
                CheckBox chkStomachTissueEtc = (Controls.Find("chkStomachTissueEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strTemp, i + 1, 1) == "1")
                {
                    chkStomachTissueEtc.Checked = true;
                }
                else if (VB.Mid(strTemp, i + 1, 1) == "0")
                {
                    chkStomachTissueEtc.Checked = false;
                }
            }
            //위암조직진단시-기타기타
            txtStomachTissueEtc.Text = list.NEW_SICK68;

            if (tab1.Enabled == true)
            {
                if ((list.PANJENGDRNO2.IsNullOrEmpty() || list.PANJENGDRNO2 == 0) && (list.PANJENGDRNO3.IsNullOrEmpty() || list.PANJENGDRNO3 == 0))
                {
                    ENDO_JUPMST list2 = endoJupmstService.GetResultDrCodebyPtNoBDate(FstrPano, FstrJepDate);

                    if (!list2.IsNullOrEmpty())
                    {
                        hb.Read_DrCode(clsType.User.IdNumber.To<long>());

                        clsHcVariable.GnHicLicense1 = list2.RESULTDRCODE.To<long>();
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                        lblDrName2.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[1] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[1, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;

                        if (cboResult0001.Text.Trim() != "")
                        {
                            //lblDrName3.Text = "김미진";
                            //strPanjengDrNo[2] = "30846";
                            //SSDR.ActiveSheet.Cells[2, 0].Text = "30846";

                            strMcodes.Clear();
                            strMcodes.Add("XR20");
                            strMcodes.Add("XR31");
                            strMcodes.Add("XR34");

                            EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                            if (!item1.IsNullOrEmpty())
                            {
                                clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                                hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                                lblDrName3.Text = clsHcVariable.GstrHicDrName;
                                strPanjengDrNo[2] = clsHcVariable.GnHicLicense1.To<string>();
                                SSDR.ActiveSheet.Cells[2, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                            }


                            clsHcVariable.GstrHicDrName = "";
                            clsHcVariable.GnHicLicense1 = 0;
                        }
                    }
                }
                else
                {
                    lblDrName2.Text = hb.READ_HIC_DRCODE3(list.PANJENGDRNO2);
                    SSDR.ActiveSheet.Cells[1, 0].Text = list.PANJENGDRNO2.To<string>();
                    if (!list.PANJENGDRNO3.IsNullOrEmpty() && list.PANJENGDRNO3 != 0)
                    {
                        //lblDrName3.Text = "김미진";
                        //SSDR.ActiveSheet.Cells[2, 0].Text = list.PANJENGDRNO3.To<string>();

                        lblDrName3.Text = hb.Read_DrNamebyInsaDrBunho(list.PANJENGDRNO3).To<string>();
                        SSDR.ActiveSheet.Cells[2, 0].Text = list.PANJENGDRNO3.To<string>();
                    }
                }

                if (list.PANJENGDRNO1.IsNullOrEmpty() || list.PANJENGDRNO1 == 0)
                {
                    string[] strXCode1 = { "HA010" };

                    XRAY_RESULTNEW list4 = xrayResultnewService.GetXDrCode1byPaNoSeekDate(FstrPano, FstrJepDate, strXCode1);

                    if (!list4.IsNullOrEmpty())
                    {
                        clsHcVariable.GnHicLicense1 = list4.XDRCODE1;
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);
                        lblDrName1.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[0] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[0, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;

                        if (cboResult0001.Text.Trim() != "")
                        {
                            //lblDrName3.Text = "김미진";
                            //strPanjengDrNo[2] = "30846";
                            //SSDR.ActiveSheet.Cells[2, 0].Text = "30846";

                            strMcodes.Clear();
                            strMcodes.Add("XR20");
                            strMcodes.Add("XR31");
                            strMcodes.Add("XR34");

                            EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                            if (!item1.IsNullOrEmpty())
                            {
                                clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                                hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                                lblDrName3.Text = clsHcVariable.GstrHicDrName;
                                strPanjengDrNo[2] = clsHcVariable.GnHicLicense1.To<string>();
                                SSDR.ActiveSheet.Cells[2, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                            }

                            clsHcVariable.GstrHicDrName = "";
                            clsHcVariable.GnHicLicense1 = 0;
                        }
                    }
                }
                else
                {
                    lblDrName1.Text = hb.READ_HIC_DRCODE3(list.PANJENGDRNO1);
                    SSDR.ActiveSheet.Cells[0, 0].Text = list.PANJENGDRNO3.To<string>();
                    if (!list.PANJENGDRNO3.IsNullOrEmpty() && list.PANJENGDRNO3.To<long>(0) > 0)
                    {
                        //lblDrName3.Text = "김미진";
                        //SSDR.ActiveSheet.Cells[2, 0].Text = list.PANJENGDRNO3.To<string>();

                        lblDrName3.Text = hb.Read_DrNamebyInsaDrBunho(list.PANJENGDRNO3).To<string>();
                        SSDR.ActiveSheet.Cells[2, 0].Text = list.PANJENGDRNO3.To<string>();
                    }
                }

                //윗부분 재정의
                if (rdoStomachTissue0.Checked == true)
                {
                    if (!list.PANJENGDRNO3.IsNullOrEmpty() && list.PANJENGDRNO3.To<long>(0) > 0)
                    {
                        SSDR.ActiveSheet.Cells[2, 0].Text = list.PANJENGDRNO3.To<string>();
                        lblDrName3.Text = hb.READ_HIC_BCODE_NamebyCode("HIC_암판정_의사매칭", list.PANJENGDRNO3.To<string>());
                    }
                }
            }
            #endregion

            #region 대장암============================================================================================================

            //분변잠혈검사결과
            if (!list.COLON_RESULT.IsNullOrEmpty())
            {
                cboDenotationSubcutaneousBlood.SelectedIndex = list.COLON_RESULT.To<int>();
            }

            txtCSize0.Text = string.Format("{0:00#}", list.NEW_SICK33); //대장이중조영용종

            str대장조영 = list.COLON_S;     //결장단수조영촬영 병형
            if (!str대장조영.IsNullOrEmpty())
            {
                for (int i = 0; i < str대장조영.Length; i++)
                {
                    ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + i.ToString(), true)[0] as ComboBox);
                    cboColonizationAassistant.SelectedIndex = VB.Mid(str대장조영, i + 1, 1).To<int>();
                }
            }

            strPositC1 = list.COLON_P;  //결장조영 병변위치
            nx = 0;
            for (int i = 0; i <= 30; i += 3)
            {
                strTemp = VB.Mid(strPositC1, i + 1, 3);
                for (int j = 0; j <= 2; j++)
                {
                    ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + j.ToString(), true)[0] as ComboBox);
                    if (cboColonizationAassistant.SelectedIndex > 0)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            if (VB.Mid(strTemp, k + 1, 1).To<int>() > 0)
                            {
                                CheckBox chkColonizationAassistant = (Controls.Find("chkColonizationAassistant" + ((j + 1) * 10 + nx).ToString(), true)[0] as CheckBox);
                                if (cboColonizationAassistant.SelectedIndex == VB.Mid(strTemp, k + 1, 1).To<int>())
                                {
                                    chkColonizationAassistant.Checked = true;
                                }
                            }
                        }
                    }
                }
                nx += 1;
            }

            str결장조영기타 = list.COLON_B;   //결장조영 기타
            for (int i = 0; i < 10; i++)
            {
                CheckBox chkColonizationAassistantEtc = (Controls.Find("chkColonizationAassistantEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(str결장조영기타, i + 1, 1) == "1")
                {
                    chkColonizationAassistantEtc.Checked = true;
                }
                else if (VB.Mid(str결장조영기타, i + 1, 1) == "0")
                {
                    chkColonizationAassistantEtc.Checked = false;
                }
            }

            txtColonizationAassistantEtc.Text = list.COLON_PETC;        //대장조영기타의기타
            txtCSize1.Text = string.Format("{0:00#}", list.NEW_SICK38); //대장내시경용종

            //조직검사실시여부
            if (!list.C_ANATGBN.IsNullOrEmpty())
            {
                if (list.C_ANATGBN.To<long>() > 0)
                {
                    RadioButton rdoColonTissue = (Controls.Find("rdoColonTissue" + "0", true)[0] as RadioButton);
                    rdoColonTissue.Checked = true;

                }
            }

            str대장내시경 = list.COLON_SENDO;    //결장경.직장경.S상결장경검사 병형
            if (!str대장내시경.IsNullOrEmpty())
            {
                for (int i = 0; i < str대장내시경.Length; i++)
                {
                    ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + i.ToString(), true)[0] as ComboBox);
                    cboColonoScope.SelectedIndex = VB.Mid(str대장내시경, i + 1, 1).To<int>();
                }
            }

            strPositC2 = list.COLON_PENDO;
            nx = 0;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    nx += 1;
                    CheckBox chkColonoScope = (Controls.Find("chkColonoScope" + (10 * j + i).ToString(), true)[0] as CheckBox);
                    if (VB.Mid(strPositC2, nx, 1) == "1")
                    {
                        chkColonoScope.Checked = true;
                    }
                }
            }

            txtColonScopeEtc.Text = list.COLON_ENDOETC; //대장내시경 기타
            str대장내시경기타 = list.COLON_BENDO;

            for (int i = 0; i < 10; i++)
            {
                CheckBox chkColonoScopeEtc = (Controls.Find("chkColonoScopeEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(str대장내시경기타, i + 1, 1) == "1")
                {
                    chkColonoScopeEtc.Checked = true;
                }
                else if (VB.Mid(str대장내시경기타, i + 1, 1) == "0")
                {
                    chkColonoScopeEtc.Checked = false;
                }
            }

            if (!list.NEW_SICK71.IsNullOrEmpty())
            {
                cboColonTissue.SelectedIndex = list.NEW_SICK71.To<int>();
            }

            if (list.NEW_SICK34 == "0" || list.NEW_SICK34.IsNullOrEmpty())
            {
                rdoCut1.Checked = true;
            }
            else
            {
                rdoCut0.Checked = true;
            }

            cboColonTissueCnt.SelectedIndex = list.NEW_SICK59.To<int>();

            if (hb.Col_Cnt_Check(FnWrtNo) > 0)
            {
                if (!list.NEW_SICK59.IsNullOrEmpty())
                {
                    cboColonTissueCnt.SelectedIndex = list.NEW_SICK59.To<int>();
                }
                else
                {
                    cboColonTissueCnt.SelectedIndex = hb.Col_Cnt_Check(FnWrtNo);
                }
            }

            if (cboColonTissueCnt.SelectedIndex > 0)
            {
                rdoColonTissue0.Checked = true;
            }

            //대장암조직진단시-암
            strTemp = list.NEW_SICK69;
            for (int i = 0; i < 13; i++)
            {
                CheckBox chkColonCancer = (Controls.Find("chkColonCancer" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strTemp, i + 1, 1) == "1")
                {
                    chkColonCancer.Checked = true;
                }
                else
                {
                    chkColonCancer.Checked = false;
                }
            }

            //대장암조직진단시-암기타
            txtColonEtc.Text = list.NEW_SICK72;

            //대장암조직진단시-기타
            strTemp = list.NEW_SICK73;
            for (int i = 0; i < 5; i++)
            {
                CheckBox chkColonCancerEtc = (Controls.Find("chkColonCancerEtc" + i.ToString(), true)[0] as CheckBox);
                if (VB.Mid(strTemp, i + 1, 1) == "1")
                {
                    chkColonCancerEtc.Checked = true;
                }
                else if (VB.Mid(strTemp, i + 1, 1) == "0")
                {
                    chkColonCancerEtc.Checked = false;
                }
            }

            //대장암조직진단시-기타기타
            txtColonCancerEtc.Text = list.NEW_SICK74;

            //대장암종합판정
            if (!list.C_PANJENG.IsNullOrEmpty())
            {
                string strCPan = "";
               
                switch (list.C_PANJENG.To<string>())
                {
                    case "1": strCPan = "1.이상소견없음"; break;
                    case "2": strCPan = "2.대장용종"; break;
                    case "3": strCPan = "3.대장암 의심"; break;
                    case "4": strCPan = "4.대장암"; break;
                    case "5": strCPan = "5.기타"; break;
                    case "6": strCPan = "6.잠혈반응없음"; break;
                    case "7": strCPan = "7.잠혈반응있음"; break;
                    default: break;
                }

                //cboCPan.SelectedIndex = list.C_PANJENG.To<int>();
                cboCPan.SelectedIndex = cboCPan.FindStringExact(strCPan);
            }

            txtJilEtcC.Text = list.C_JILETC;

            if (tab2.Enabled == true)
            {
                if (TabColono.SelectedTab == tabColon3)
                {
                    if (list.PANJENGDRNO4.IsNullOrEmpty() || list.PANJENGDRNO4 == 0)
                    {
                        clsHcVariable.GnHicLicense1 = endoJupmstService.GetResultDrCodebyPtNoBDateGroup(FstrPano, FstrJepDate);
                        //clsHcVariable.GstrHicDrName = hb.READ_License_DrName(clsHcVariable.GnHicLicense1); //판정의 성명
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                        lblDrName5.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[3] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[3, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;
                    }
                    else
                    {
                        lblDrName5.Text = hb.READ_HIC_DRCODE3(list.PANJENGDRNO4);
                        SSDR.ActiveSheet.Cells[3, 0].Text = list.PANJENGDRNO4.To<string>();
                    }

                    if ((list.PANJENGDRNO5.IsNullOrEmpty() || list.PANJENGDRNO5 == 0) && (list.PANJENGDRNO6.IsNullOrEmpty() || list.PANJENGDRNO6 == 0))
                    {
                        clsHcVariable.GnHicLicense1 = endoJupmstService.GetResultDrCodebyPtNoBDateGroup(FstrPano, FstrJepDate);
                        //clsHcVariable.GstrHicDrName = hb.READ_License_DrName(clsHcVariable.GnHicLicense1); //판정의 성명
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);
                        lblDrName4.Text = clsHcVariable.GstrHicDrName;
                        SSDR.ActiveSheet.Cells[4, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        strPanjengDrNo[4] = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;

                        if (!cboColonTissue.Text.IsNullOrEmpty())
                        {
                            //lblDrName6.Text = "김미진";
                            //strPanjengDrNo[5] = "30846";
                            //SSDR.ActiveSheet.Cells[5, 0].Text = "30846";

                            strMcodes.Clear();
                            strMcodes.Add("XR14");
                            strMcodes.Add("XR27");
                            strMcodes.Add("XR31");

                            EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                            if (!item1.IsNullOrEmpty())
                            {
                                clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                                hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                                lblDrName6.Text = clsHcVariable.GstrHicDrName;
                                strPanjengDrNo[5] = clsHcVariable.GnHicLicense1.To<string>();
                                SSDR.ActiveSheet.Cells[5, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                            }
                        }
                    }
                    else
                    {
                        lblDrName4.Text = hb.READ_HIC_DRCODE2(list.PANJENGDRNO5);
                        SSDR.ActiveSheet.Cells[4, 0].Text = list.PANJENGDRNO5.To<string>();
                        lblDrName6.Text = hb.Read_DrNamebyInsaDrBunho(list.PANJENGDRNO6).To<string>();
                        SSDR.ActiveSheet.Cells[5, 0].Text = list.PANJENGDRNO6.To<string>();
                        //if (!list.PANJENGDRNO6.IsNullOrEmpty() && list.PANJENGDRNO6.To<long>(0) > 0)
                        //{
                        //    lblDrName6.Text = "김미진";
                        //    SSDR.ActiveSheet.Cells[5, 0].Text = list.PANJENGDRNO6.To<string>();
                        //}
                    }
                }

                //윗부분 재정의
                if (rdoColonTissue0.Checked == true)
                {
                    if (!list.PANJENGDRNO6.IsNullOrEmpty() && list.PANJENGDRNO6.To<long>(0) > 0)
                    {
                        SSDR.ActiveSheet.Cells[5, 0].Text = list.PANJENGDRNO6.To<string>();
                        lblDrName6.Text = hb.READ_HIC_BCODE_NamebyCode("HIC_암판정_의사매칭", list.PANJENGDRNO6.To<string>());
                    }
                }
            }
            #endregion

            #region 간암=================================================================================================

            //간암 RPHA, EIA
            if (!list.LIVER_RPHA.IsNullOrEmpty())
            {
                cboRPHA.SelectedIndex = list.LIVER_RPHA.To<int>();
            }

            txtEIA2.Text = "[참고치:(0~9 ng/ml 이하)]";
            txtEIA1.Enabled = false;
            txtEIA2.Enabled = false;

            //간암 의료급여대상자
            txtALT.Text = list.LIVER_NEW_ALT;
            cboNew_B.SelectedIndex = (list.LIVER_NEW_B.IsNullOrEmpty() == true ? "-1" : list.LIVER_NEW_B).To<int>();
            cboBRsult.SelectedIndex = (list.LIVER_NEW_BRESULT.IsNullOrEmpty() == true ? "-1" : list.LIVER_NEW_BRESULT).To<int>();
            cboNew_C.SelectedIndex = (list.LIVER_NEW_C.IsNullOrEmpty() == true ? "-1" : list.LIVER_NEW_C).To<int>();
            cboCResult.SelectedIndex = (list.LIVER_NEW_CRESULT.IsNullOrEmpty() == true ? "-1" : list.LIVER_NEW_CRESULT).To<int>();

            //간암대상자 검사결과 조회
            if (FstrJob == "1")    //신규대상자만
            {
                if (chkCancer2.Checked == true)
                {
                    List<HIC_RESULT> list3 = hicResultService.GetResultExCodebyWrtNo(FnWrtNo);

                    if (!list3.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list3.Count; i++)
                        {
                            if (!list3[i].RESULT.IsNullOrEmpty())
                            {
                                switch (list3[i].EXCODE)
                                {
                                    case "A125":
                                        txtALT.Text = list3[i].RESULT;
                                        break;
                                    case "A131":
                                        cboNew_B.SelectedIndex = list3[i].RESULT.To<int>();
                                        break;
                                    case "A264":
                                        txtEIA1.Text = "검사결과 : " + list3[i].RESULT + " ng/ml";
                                        ///TODO : 이상훈(2020.04.13 확인필요
                                        txtEIA.Text = list3[i].RESULT;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                eBtnClick(btnGubDaesang, new EventArgs());
            }

            //간암종합판정
            if (!list.LIVER_PANJENG.IsNullOrEmpty())
            {
                cboLPan.SelectedIndex = list.LIVER_PANJENG.To<int>();
            }

            txtJilEtcL.Text = list.LIVER_JILETC;    //기타질환

            string[] strXCode = { "US01B", "US01" };

            if (tab3.Enabled == true)
            {
                if (list.PANJENGDRNO7.IsNullOrEmpty() || list.PANJENGDRNO7.To<long>(0) == 0)
                {
                    XRAY_RESULTNEW list4 = xrayResultnewService.GetXDrCode1byPaNoSeekDate(FstrPano, FstrJepDate, strXCode);

                    if (!list4.IsNullOrEmpty())
                    {
                        clsHcVariable.GnHicLicense1 = list4.XDRCODE1;
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                        lbllDrName7.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[6] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[6, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;
                    }
                }
                else
                {
                    //TODO: Spread 의사하드코딩 방식 개선할것
                    SSDR.ActiveSheet.Cells[6, 0].Text = list.PANJENGDRNO7.To<string>();
                    lbllDrName7.Text = hb.READ_HIC_DRCODE3(list.PANJENGDRNO7);
                }
            }
            #endregion

            #region 유방암==============================================================================================
            cboBreast.SelectedIndex = (list.B_ANATGBN.IsNullOrEmpty() == true ? "-1" : list.B_ANATGBN).To<int>();    //유방실질분포량
            str유방판독소견 = list.BREAST_S;
            nx = 0;
            if (!str유방판독소견.IsNullOrEmpty())
            {
                for (int i = 0; i < str유방판독소견.Length; i += 2)
                {
                    ComboBox cboBreast = (Controls.Find("cboBreast" + nx.ToString(), true)[0] as ComboBox);
                    cboBreast.SelectedIndex = VB.Mid(str유방판독소견, i + 1, 2).To<int>();
                    nx += 1;
                }
            }

            txtBreastReadOpinionEtc.Text = list.NEW_WOMAN17;        //유방판독소견기타

            //유방단순촬영부위(우)
            nx = 0;
            strPositB = VB.Left(list.BREAST_P, 36);
            for (int i = 0; i < 36; i += 6)
            {
                strTemp = VB.Mid(strPositB, i + 1, 6);
                for (int j = 0; j <= 2; j++)
                {
                    ComboBox cboBreast = (Controls.Find("cboBreast" + j.ToString(), true)[0] as ComboBox);
                    CheckBox chkbreastR = (Controls.Find("chkbreastR" + ((j + 1) * 10 + nx).ToString(), true)[0] as CheckBox);
                    if (cboBreast.SelectedIndex > 0)
                    {
                        for (int k = 1; k <= 6; k += 2)
                        {
                            if (cboBreast.SelectedIndex == VB.Mid(strTemp, k, 2).To<int>())
                            {
                                chkbreastR.Checked = true;
                            }
                        }
                    }
                }
                nx += 1;
            }

            //유방단순촬영부위(좌)
            nx = 0;
            strPositB = VB.Mid(list.BREAST_P, 37, 36);
            for (int i = 0; i < 36; i += 6)
            {
                strTemp = VB.Mid(strPositB, i + 1, 6);
                for (int j = 0; j <= 2; j++)
                {
                    ComboBox cboBreast = (Controls.Find("cboBreast" + j.ToString(), true)[0] as ComboBox);
                    CheckBox chkBreastL = (Controls.Find("chkBreastL" + ((j + 1) * 10 + nx).ToString(), true)[0] as CheckBox);
                    if (cboBreast.SelectedIndex > 0)
                    {
                        for (int k = 1; k <= 6; k += 2)
                        {
                            if (cboBreast.SelectedIndex == VB.Mid(strTemp, k, 2).To<int>())
                            {
                                chkBreastL.Checked = true;
                            }
                        }
                    }
                }
                nx += 1;
            }

            if (!list.BREAST_ETC.IsNullOrEmpty()) { chkBreastR16.Checked = true; }
            if (!list.B_ANATETC.IsNullOrEmpty()) { chkBreastL16.Checked = true; }
            if (!list.BREAST_ETC2.IsNullOrEmpty()) { chkBreastR26.Checked = true; }
            if (!list.B_ANATETC2.IsNullOrEmpty()) { chkBreastL26.Checked = true; }
            if (!list.BREAST_ETC3.IsNullOrEmpty()) { chkBreastR36.Checked = true; }
            if (!list.B_ANATETC3.IsNullOrEmpty()) { chkBreastL36.Checked = true; }

            txtBreastPosEtc10.Text = list.BREAST_ETC;       //유방부위 기타1Set(우)
            txtBreastPosEtc11.Text = list.B_ANATETC;        //유방부위 기타1Set(좌)
            txtBreastPosEtc20.Text = list.BREAST_ETC2;      //유방부위 기타1Set(우)
            txtBreastPosEtc21.Text = list.B_ANATETC2;       //유방부위 기타1Set(좌)
            txtBreastPosEtc30.Text = list.BREAST_ETC3;      //유방부위 기타1Set(우)
            txtBreastPosEtc31.Text = list.B_ANATETC3;       //유방부위 기타1Set(좌)

            if (!list.B_PANJENG.IsNullOrEmpty())
            {
                cboBPan.SelectedIndex = list.B_PANJENG.To<int>();
            }

            txtJilEtcB.Text = list.B_JILETC;

            if (tab4.Enabled == true)
            {
                if (list.PANJENGDRNO8.IsNullOrEmpty() || list.PANJENGDRNO8 == 0)
                {
                    string[] strXCode2 = { "G2702", "G2702B" };

                    XRAY_RESULTNEW list5 = xrayResultnewService.GetXDrCode1byPaNoSeekDateGroup(FstrPano, FstrJepDate, strXCode2);

                    if (!list5.IsNullOrEmpty())
                    {
                        clsHcVariable.GnHicLicense1 = list5.XDRCODE1;
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                        lblDrName8.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[7] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[7, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;
                    }
                }
                else
                {
                    lblDrName8.Text = hb.Read_DrNamebyDrBunho(list.PANJENGDRNO8).To<string>();
                    if (lblDrName8.Text.IsNullOrEmpty())
                    {
                        if(list.PANJENGDRNO8 == 79507)
                        {
                            lblDrName8.Text = "임효진";
                        }
                        else if ( list.PANJENGDRNO8 == 72532 )
                        {
                            lblDrName8.Text ="최선형";
                        }

                        BAS_BCODE item2 = basBcodeService.GetAllByGubunCode1("XRAY_외주판독의사", list.PANJENGDRNO8.ToString());
                        if (!item2.IsNullOrEmpty())
                        {
                            lblDrName8.Text = item2.NAME; //판정의 성명
                        }


                    }
                    SSDR.ActiveSheet.Cells[7, 0].Text = list.PANJENGDRNO8.To<string>();
                }
            }
            #endregion

            #region 자궁경부암====================================================================================================  
            string[] strWomBArr = null;
            string strFDate = FstrJepDate;
            string strTDate = cf.DATE_ADD(clsDB.DbCon, FstrJepDate, 3);
            string strRowid = "";
            COMHPC item = comHpcLibBService.GetListWombAnatMstByPtno(FstrPtno, strFDate, strTDate);
            PanWomB panWomB = new PanWomB();
            
            if (!item.IsNullOrEmpty())
            {
                strRowid = item.RID.To<string>("");
                txtResult1.Text = GetCytologyResult(strRowid);
                //판독결과를 문자열로 변환
                string[] separator = new string[1]{"▶"};
                strWomBArr = txtResult1.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                //판독결과를 잘라서 저장함
                GetCytologyResult_Separator_Words(panWomB, strWomBArr);    
            }
            else
            {
                txtResult1.Text = "검체검사 진행중 ...";
            }

            //검체상태
            if (list.WOMB01.IsNullOrEmpty())
            {
                if (panWomB.WOMB01 == "1") { rdoWomBo11.Checked = true; }
                else if (panWomB.WOMB01 == "2") { rdoWomBo12.Checked = true; }
            }
            else
            {
                if (list.WOMB01.To<string>("").Trim() == "1") { rdoWomBo11.Checked = true; }
                else { rdoWomBo12.Checked = true; }
            }

            //자궁경부 선상피세포 유무
            if (list.WOMB02.IsNullOrEmpty())
            {
                if (panWomB.WOMB02 == "1") { rdoWomBo21.Checked = true; }
                else if (panWomB.WOMB02 == "2") { rdoWomBo22.Checked = true; }
            }
            else
            {
                if (list.WOMB02.To<string>("").Trim() == "1") { rdoWomBo21.Checked = true; }
                else { rdoWomBo22.Checked = true; }
            }

            //유형별진단
            if (list.WOMB03.IsNullOrEmpty())
            {
                if (panWomB.WOMB03 == "1") { rdoWomBo31.Checked = true; }
                else if (panWomB.WOMB03 == "2") { rdoWomBo32.Checked = true; }
                else if (panWomB.WOMB03 == "3") { rdoWomBo33.Checked = true; }
            }
            else
            {
                switch (list.WOMB03.To<string>("").Trim())
                {
                    case "1": rdoWomBo31.Checked = true; break;
                    case "2": rdoWomBo32.Checked = true; break;
                    case "3": rdoWomBo33.Checked = true; break;
                    default: break;
                }
            }

            txtWombo3_Etc.Text = list.WOMB11;                                                           //유형별진단 기타(자궁내막세포 출현등)

            //편평상피세포이상
            if (list.WOMB04.IsNullOrEmpty())
            {
                if (panWomB.WOMB04 == "1") { chkWomBo4.Checked = true; rdoWomBo41.Checked = true; }
                else if (panWomB.WOMB04 == "2") { chkWomBo4.Checked = true; rdoWomBo42.Checked = true; }
                else if (panWomB.WOMB04 == "3") { chkWomBo4.Checked = true; rdoWomBo43.Checked = true; }
                else if (panWomB.WOMB04 == "4") { chkWomBo4.Checked = true; rdoWomBo44.Checked = true; }
            }
            else
            {
                if (string.Compare(list.WOMB04.To<string>("").Trim(), "0") > 0)
                {
                    chkWomBo4.Checked = true;

                    switch (list.WOMB04.To<string>("").Trim())
                    {
                        case "1": rdoWomBo41.Checked = true; break;
                        case "2": rdoWomBo42.Checked = true; break;
                        case "3": rdoWomBo43.Checked = true; break;
                        case "4": rdoWomBo44.Checked = true; break;
                        default: break;
                    }
                }
            }

            //비정형 편평상피세포 위험구분
            if (list.WOMAN12.IsNullOrEmpty())
            {
                if (panWomB.WOMAN12.To<string>("").Trim() == "일반") { chkWomBo51.Checked = true; }
                else if (panWomB.WOMAN12.To<string>("").Trim() == "고위험") { chkWomBo52.Checked = true; }
            }
            else
            {
                if (list.WOMAN12.To<string>("").Trim() == "1")
                {
                    chkWomBo51.Checked = true;
                }
                else if (list.WOMAN12.To<string>("").Trim() == "2")
                {
                    chkWomBo52.Checked = true;
                }
            }

            //FstrJob => "1" : 미판정 / "2" : 판정 
            if (FstrJob == "1" && (list.WOMAN12.IsNullOrEmpty() || list.WOMAN12.To<int>() == 0))        
            {
                //병리과 검체상태 조회
                chkWomBo51.Checked = false; chkWomBo52.Checked = false;

                int nDn = (int)hb.READ_Womb_Check_New(FnWrtNo);

                if (nDn > 0)
                {
                    if (nDn == 1) { chkWomBo51.Checked = true; }
                    else if (nDn == 2) { chkWomBo52.Checked = true; }
                }
            }

            //선상피세포이상
            if (list.WOMB05.IsNullOrEmpty())
            {
                if (panWomB.WOMB05 == "1") { chkWomBo6.Checked = true; rdoWomBo61.Checked = true; }
                else if (panWomB.WOMB05 == "2") { chkWomBo6.Checked = false; rdoWomBo62.Checked = true; }
                else if (panWomB.WOMB05 == "3") { chkWomBo6.Checked = false; rdoWomBo63.Checked = true; }
                else if (panWomB.WOMB05 == "4") { chkWomBo6.Checked = false; rdoWomBo64.Checked = true; }
            }
            else
            {
                if (string.Compare(list.WOMB05.To<string>("").Trim(), "0") > 0)
                {
                    chkWomBo6.Checked = true;

                    switch (list.WOMB05.To<string>("").Trim())
                    {
                        case "1": rdoWomBo61.Checked = true; break;
                        case "2": rdoWomBo62.Checked = true; break;
                        case "3": rdoWomBo63.Checked = true; break;
                        default: break;
                    }
                }
            }

            txtWombo6_Etc.Text = list.WOMB06;                                                           //선상피세포이상 기타

            //추가소견
            if (list.WOMB07.IsNullOrEmpty())
            {
                switch (panWomB.WOMB06.To<string>("0").Trim())
                {
                    case "1": rdoWomBo71.Checked = true; break;
                    case "2": rdoWomBo72.Checked = true; break;
                    case "3": rdoWomBo73.Checked = true; break;
                    case "4": rdoWomBo74.Checked = true; break;
                    case "5": rdoWomBo75.Checked = true; break;
                    case "6": rdoWomBo76.Checked = true; break;
                    case "7": rdoWomBo77.Checked = true; break;
                    default: rdoWomBo70.Checked = true; break;
                }
            }
            else
            {
                switch (list.WOMB07.To<string>("0").Trim())
                {
                    case "0": rdoWomBo70.Checked = true; break;
                    case "1": rdoWomBo71.Checked = true; break;
                    case "2": rdoWomBo72.Checked = true; break;
                    case "3": rdoWomBo73.Checked = true; break;
                    case "4": rdoWomBo74.Checked = true; break;
                    case "5": rdoWomBo75.Checked = true; break;
                    case "6": rdoWomBo76.Checked = true; break;
                    case "7": rdoWomBo77.Checked = true; break;
                    default: rdoWomBo71.Checked = true; break;
                }
            }
            
            txtWombo7_Etc.Text = list.WOMB08;                                                           //추가소견 직접기입

            if (list.WOMB09.IsNullOrEmpty())
            {
                cboWPan.SelectedIndex = panWomB.WOMB07.To<int>();
            }
            else
            {
                cboWPan.SelectedIndex = list.WOMB09.To<int>();
            }

            //if (cboWPan.SelectedIndex > 0)
            //{
            //    if (list.WOMB07.IsNullOrEmpty())
            //    {
            //        rdoWomBo70.Checked = true;
            //    }
            //}

            //중복자궁
            rdoCervixDuplex1.Checked = true;
            if (!list.WOMB12.IsNullOrEmpty())
            {
                if (list.WOMB12.To<int>() == 2) rdoCervixDuplex2.Checked = true;
            }
            txtJilEtcM.Text = list.WOMB10;

            if (tab5.Enabled == true)
            {
                if (list.PANJENGDRNO9.IsNullOrEmpty() || list.PANJENGDRNO9 == 0 
                    || list.PANJENGDRNO10.IsNullOrEmpty() || list.PANJENGDRNO10 == 0)
                {
                    HIC_JEPSU list5 = hicJepsuService.GetEntTimebyWrtNoJepDate(FnWrtNo, FstrJepDate);

                    if (!list5.IsNullOrEmpty())
                    {
                        strEntTime = VB.Left(list5.ENTTIME.To<string>(), 2);
                    }

                    if (string.Compare(strEntTime, "13") < 0)
                    {
                        strEntTime = "오전";
                    }
                    else
                    {
                        strEntTime = "오후";
                    }

                    List<BAS_SCHEDULE> list6 = basScheduleService.GetItembySchDate(FstrJepDate);

                    for (int i = 0; i < list6.Count; i++)
                    {
                        strGBJIN = list6[i].GBJIN;
                        strGBJIN2 = list6[i].GBJIN2;

                        if (strEntTime == "오전" && strGBJIN == "1")
                        {
                            strDrCode = list6[i].DRCODE;
                            break;
                        }
                        else if (strEntTime == "오후" && strGBJIN2 == "1")
                        {
                            strDrCode = list6[i].DRCODE;
                            break;
                        }
                    }

                    if (!strDrCode.IsNullOrEmpty())
                    {
                        clsHcVariable.GnHicLicense1 = strDrCode.To<long>();
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1.To<string>(""));
                        lblDrName9.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[8] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[8, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;

                        strMcodes.Clear();
                        strMcodes.Add("YY01");

                        EXAM_ANATMST item1 = examAnatmstService.GetItembyPtNoJepDateOrder(FstrPtno, FstrJepDate, strMcodes);
                        if (!item1.IsNullOrEmpty())
                        {
                            clsHcVariable.GnHicLicense1 = item1.RESULTSABUN.To<long>();
                            hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                            lblDrName10.Text = clsHcVariable.GstrHicDrName;
                            strPanjengDrNo[9] = clsHcVariable.GnHicLicense1.To<string>();
                            SSDR.ActiveSheet.Cells[9, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                        }

                        //lblDrName10.Text = "김영금";
                        //strPanjengDrNo[9] = "1041";
                        //SSDR.ActiveSheet.Cells[9, 0].Text = "1041";

                        clsHcVariable.GstrHicDrName = "";
                        clsHcVariable.GnHicLicense1 = 0;
                    }
                    else
                    {
                        MessageBox.Show("진료과장 스케줄을 체크해주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    lblDrName9.Text = hb.Read_DrNamebyDrBunho(list.PANJENGDRNO9).To<string>();
                    SSDR.ActiveSheet.Cells[8, 0].Text = list.PANJENGDRNO9.To<string>();


                    lblDrName10.Text = hb.Read_DrNamebyInsaDrBunho(list.PANJENGDRNO10).To<string>();
                    SSDR.ActiveSheet.Cells[9, 0].Text = list.PANJENGDRNO10.To<string>();

                    //lblDrName10.Text = "김미진";
                    //SSDR.ActiveSheet.Cells[9, 0].Text = list.PANJENGDRNO10.To<string>();
                }

                //윗부분 재정의
                if (rdoWomBo11.Checked)
                {
                    if (!list.PANJENGDRNO10.IsNullOrEmpty() && list.PANJENGDRNO10.To<long>(0) > 0)
                    {
                        SSDR.ActiveSheet.Cells[9, 0].Text = list.PANJENGDRNO10.To<string>();
                        lblDrName10.Text = hb.READ_HIC_BCODE_NamebyCode("HIC_암판정_의사매칭", list.PANJENGDRNO10.To<string>());
                    }
                }
            }
            #endregion

            #region 폐암========================================================================================================
            if (tab6.Enabled == true)
            {
                pnlPanLungResult.Visible = true;
                if (list.NEW_WOMAN37.IsNullOrEmpty())
                {
                    HIC_XRAY_RESULT list7 = hicXrayResultService.GetAllbyPtNoJepDate(FstrPtno, FstrJepDate, "TY10");

                    if (!list7.IsNullOrEmpty())
                    {
                        //디폴트로 체크함
                        chkResult010.Checked = true;

                        if (tabLung1.Visible == true)
                        {
                            if (list7.NUDOYN_1 == "1") { chkResult010.Checked = true; }
                            else if (list7.NUDOYN_1 == "2") { chkResult020.Checked = true; }
                            else if (list7.NUDOYN_1 == "3") { chkResult030.Checked = true; }

                            if (list7.NUDOSITE_1 == "1") { chkResult040.Checked = true; }
                            else if (list7.NUDOSITE_1 == "2") { chkResult050.Checked = true; }
                            else if (list7.NUDOSITE_1 == "3") { chkResult060.Checked = true; }
                            else if (list7.NUDOSITE_1 == "4") { chkResult070.Checked = true; }
                            else if (list7.NUDOSITE_1 == "5") { chkResult080.Checked = true; }

                            if (list7.NUDOICON_1 == "1") { chkResult090.Checked = true; }
                            else if (list7.NUDOICON_1 == "2") { chkResult100.Checked = true; }
                            else if (list7.NUDOICON_1 == "3") { chkResult110.Checked = true; }

                            if (list7.NUDOPOSITIVE_1 == "1") { chkResult120.Checked = true; }
                            else if (list7.NUDOPOSITIVE_1 == "2") { chkResult130.Checked = true; }
                            else if (list7.NUDOPOSITIVE_1 == "3") { chkResult140.Checked = true; }

                            if (list7.NUDOTRACECHK_1 == "1") { chkResult150.Checked = true; }
                            else if (list7.NUDOTRACECHK_1 == "2") { chkResult160.Checked = true; }
                            else if (list7.NUDOTRACECHK_1 == "3") { chkResult170.Checked = true; }

                            if (list7.NUDOTRACECHK2_1 == "1") { chkResult180.Checked = true; }
                            else if (list7.NUDOTRACECHK2_1 == "2") { chkResult190.Checked = true; }

                            txtLungResult10.Text = "";
                            txtLungResult20.Text = "";
                            txtLungResult30.Text = "";
                        }

                        if (tabLung2.Visible == true)
                        {
                            if (list7.NUDOYN_2 == "1") { chkResult011.Checked = true; }
                            else if (list7.NUDOYN_2 == "2") { chkResult021.Checked = true; }
                            else if (list7.NUDOYN_2 == "3") { chkResult031.Checked = true; }
                             
                            if (list7.NUDOSITE_2 == "1") { chkResult041.Checked = true; }
                            else if (list7.NUDOSITE_2 == "2") { chkResult051.Checked = true; }
                            else if (list7.NUDOSITE_2 == "3") { chkResult061.Checked = true; }
                            else if (list7.NUDOSITE_2 == "4") { chkResult070.Checked = true; }
                            else if (list7.NUDOSITE_2 == "5") { chkResult081.Checked = true; }

                            if (list7.NUDOICON_2 == "1") { chkResult091.Checked = true; }
                            else if (list7.NUDOICON_2 == "2") { chkResult101.Checked = true; }
                            else if (list7.NUDOICON_2 == "3") { chkResult111.Checked = true; }

                            if (list7.NUDOPOSITIVE_2 == "1") { chkResult121.Checked = true; }
                            else if (list7.NUDOPOSITIVE_2 == "2") { chkResult131.Checked = true; }
                            else if (list7.NUDOPOSITIVE_2 == "3") { chkResult141.Checked = true; }

                            if (list7.NUDOTRACECHK_2 == "1") { chkResult151.Checked = true; }
                            else if (list7.NUDOTRACECHK_2 == "2") { chkResult161.Checked = true; }
                            else if (list7.NUDOTRACECHK_2 == "3") { chkResult171.Checked = true; }

                            if (list7.NUDOTRACECHK2_2 == "1") { chkResult181.Checked = true; }
                            else if (list7.NUDOTRACECHK2_2 == "2") { chkResult191.Checked = true; }

                            txtLungResult11.Text = "";
                            txtLungResult21.Text = "";
                            txtLungResult31.Text = "";
                        }

                        if (tabLung3.Visible == true)
                        {
                            if (list7.NUDOYN_3 == "1") { chkResult012.Checked = true; }
                            else if (list7.NUDOYN_3 == "2") { chkResult022.Checked = true; }
                            else if (list7.NUDOYN_3 == "3") { chkResult032.Checked = true; }

                            if (list7.NUDOSITE_3 == "1") { chkResult042.Checked = true; }
                            else if (list7.NUDOSITE_3 == "2") { chkResult052.Checked = true; }
                            else if (list7.NUDOSITE_3 == "3") { chkResult062.Checked = true; }
                            else if (list7.NUDOSITE_3 == "4") { chkResult072.Checked = true; }
                            else if (list7.NUDOSITE_3 == "5") { chkResult082.Checked = true; }

                            if (list7.NUDOICON_3 == "1") { chkResult092.Checked = true; }
                            else if (list7.NUDOICON_3 == "2") { chkResult102.Checked = true; }
                            else if (list7.NUDOICON_3 == "3") { chkResult112.Checked = true; }

                            if (list7.NUDOPOSITIVE_3 == "1") { chkResult122.Checked = true; }
                            else if (list7.NUDOPOSITIVE_3 == "2") { chkResult132.Checked = true; }
                            else if (list7.NUDOPOSITIVE_3 == "3") { chkResult142.Checked = true; }

                            if (list7.NUDOTRACECHK_3 == "1") { chkResult152.Checked = true; }
                            else if (list7.NUDOTRACECHK_3 == "2") { chkResult162.Checked = true; }
                            else if (list7.NUDOTRACECHK_3 == "3") { chkResult172.Checked = true; }

                            if (list7.NUDOTRACECHK2_3 == "1") { chkResult182.Checked = true; }
                            else if (list7.NUDOTRACECHK2_3 == "2") { chkResult192.Checked = true; }

                            txtLungResult12.Text = "";
                            txtLungResult22.Text = "";
                            txtLungResult32.Text = "";
                        }

                        if (tabLung4.Visible == true)
                        {
                            if (list7.NUDOYN_4 == "1") { chkResult013.Checked = true; }
                            else if (list7.NUDOYN_4 == "2") { chkResult023.Checked = true; }
                            else if (list7.NUDOYN_4 == "3") { chkResult033.Checked = true; }

                            if (list7.NUDOSITE_4 == "1") { chkResult043.Checked = true; }
                            else if (list7.NUDOSITE_4 == "2") { chkResult053.Checked = true; }
                            else if (list7.NUDOSITE_4 == "3") { chkResult063.Checked = true; }
                            else if (list7.NUDOSITE_4 == "4") { chkResult073.Checked = true; }
                            else if (list7.NUDOSITE_4 == "5") { chkResult083.Checked = true; }

                            if (list7.NUDOICON_4 == "1") { chkResult093.Checked = true; }
                            else if (list7.NUDOICON_4 == "2") { chkResult103.Checked = true; }
                            else if (list7.NUDOICON_4 == "3") { chkResult113.Checked = true; }

                            if (list7.NUDOPOSITIVE_4 == "1") { chkResult123.Checked = true; }
                            else if (list7.NUDOPOSITIVE_4 == "2") { chkResult133.Checked = true; }
                            else if (list7.NUDOPOSITIVE_4 == "3") { chkResult143.Checked = true; }

                            if (list7.NUDOTRACECHK_4 == "1") { chkResult153.Checked = true; }
                            else if (list7.NUDOTRACECHK_4 == "2") { chkResult163.Checked = true; }
                            else if (list7.NUDOTRACECHK_4 == "3") { chkResult173.Checked = true; }

                            if (list7.NUDOTRACECHK2_4 == "1") { chkResult183.Checked = true; }
                            else if (list7.NUDOTRACECHK2_4 == "2") { chkResult193.Checked = true; }

                            txtLungResult13.Text = "";
                            txtLungResult23.Text = "";
                            txtLungResult33.Text = "";
                        }

                        if (tabLung5.Visible == true)
                        {
                            if (list7.NUDOYN_5 == "1") { chkResult014.Checked = true; }
                            else if (list7.NUDOYN_5 == "2") { chkResult024.Checked = true; }
                            else if (list7.NUDOYN_5 == "3") { chkResult034.Checked = true; }

                            if (list7.NUDOSITE_5 == "1") { chkResult044.Checked = true; }
                            else if (list7.NUDOSITE_5 == "2") { chkResult054.Checked = true; }
                            else if (list7.NUDOSITE_5 == "3") { chkResult064.Checked = true; }
                            else if (list7.NUDOSITE_5 == "4") { chkResult074.Checked = true; }
                            else if (list7.NUDOSITE_5 == "5") { chkResult084.Checked = true; }

                            if (list7.NUDOICON_5 == "1") { chkResult094.Checked = true; }
                            else if (list7.NUDOICON_5 == "2") { chkResult104.Checked = true; }
                            else if (list7.NUDOICON_5 == "3") { chkResult114.Checked = true; }

                            if (list7.NUDOPOSITIVE_5 == "1") { chkResult124.Checked = true; }
                            else if (list7.NUDOPOSITIVE_5 == "2") { chkResult134.Checked = true; }
                            else if (list7.NUDOPOSITIVE_5 == "3") { chkResult144.Checked = true; }

                            if (list7.NUDOTRACECHK_5 == "1") { chkResult154.Checked = true; }
                            else if (list7.NUDOTRACECHK_5 == "2") { chkResult164.Checked = true; }
                            else if (list7.NUDOTRACECHK_5 == "3") { chkResult174.Checked = true; }
                             
                            if (list7.NUDOTRACECHK2_1 == "1") { chkResult184.Checked = true; }
                            else if (list7.NUDOTRACECHK2_1 == "2") { chkResult194.Checked = true; }

                            txtLungResult14.Text = "";
                            txtLungResult24.Text = "";
                            txtLungResult34.Text = "";
                        }

                        if (tabLung6.Visible == true)
                        {
                            if (list7.NUDOYN_6 == "1") { chkResult015.Checked = true; }
                            else if (list7.NUDOYN_6 == "2") { chkResult025.Checked = true; }
                            else if (list7.NUDOYN_6 == "3") { chkResult035.Checked = true; }

                            if (list7.NUDOSITE_6 == "1") { chkResult045.Checked = true; }
                            else if (list7.NUDOSITE_6 == "2") { chkResult055.Checked = true; }
                            else if (list7.NUDOSITE_6 == "3") { chkResult065.Checked = true; }
                            else if (list7.NUDOSITE_6 == "4") { chkResult075.Checked = true; }
                            else if (list7.NUDOSITE_6 == "5") { chkResult085.Checked = true; }

                            if (list7.NUDOICON_6 == "1") { chkResult095.Checked = true; }
                            else if (list7.NUDOICON_6 == "2") { chkResult105.Checked = true; }
                            else if (list7.NUDOICON_6 == "3") { chkResult115.Checked = true; }

                            if (list7.NUDOPOSITIVE_6 == "1") { chkResult125.Checked = true; }
                            else if (list7.NUDOPOSITIVE_6 == "2") { chkResult135.Checked = true; }
                            else if (list7.NUDOPOSITIVE_6 == "3") { chkResult145.Checked = true; }

                            if (list7.NUDOTRACECHK_6 == "1") { chkResult155.Checked = true; }
                            else if (list7.NUDOTRACECHK_6 == "2") { chkResult165.Checked = true; }
                            else if (list7.NUDOTRACECHK_6 == "3") { chkResult175.Checked = true; }

                            if (list7.NUDOTRACECHK2_6 == "1") { chkResult185.Checked = true; }
                            else if (list7.NUDOTRACECHK2_6 == "2") { chkResult195.Checked = true; }

                            txtLungResult15.Text = "";
                            txtLungResult25.Text = "";
                            txtLungResult35.Text = "";
                        }
                        //폐암공동결과
                        //이전CT유무

                        if (list7.PASTCT == "1")
                        {
                            rdoCTYN0.Checked = true;
                            txtResult6.Text = "";
                        }
                        else if (list7.PASTCT == "2")
                        {
                            rdoCTYN1.Checked = true;
                            txtResult6.Text = list7.PASTCTYYYY + list7.PASTCTMM;
                        }
                        else
                        {
                            rdoCTYN0.Checked = true;
                            txtResult6.Text = "";
                        }

                        //이전 CT 유무
                        if (list7.PASTCT.IsNullOrEmpty())
                        {
                            string strSeekDate = comHpcLibBService.GetSeekDatebyPaNoSeekDate(FstrPano, FstrJepDate);

                            if (!strSeekDate.IsNullOrEmpty())
                            {
                                rdoCTYN1.Checked = true;
                                txtResult6.Text = strSeekDate;
                            }
                        }

                        //선량
                        txtResult7.Text = list7.CTDOSE;

                        //기관지내 병변
                        if (list7.INDICATIOCHK == "1")
                        {
                            rdoResult61.Checked = true;
                            txtResult11.Text = list7.INDICATIOETC;
                        }
                        else
                        {
                            rdoResult60.Checked = true;
                        }

                        //폐결절 외 폐암시사 소견
                        if (list7.SISACHK == "1" || list7.SISACHK.IsNullOrEmpty()) { chkResult200.Checked = true; }
                        else if (list7.SISACHK == "2") { chkResult201.Checked = true; }
                        else if (list7.SISACHK == "3") { chkResult202.Checked = true; }
                        else if (list7.SISACHK == "4") { chkResult203.Checked = true; }
                        else if (list7.SISACHK == "5") { chkResult204.Checked = true; }

                        txtResult4.Text = list7.SISAETC;

                        //폐결절 외 의미있는 소견
                        if (list7.NUDOMEAN1 == "1" || list7.NUDOMEAN1.IsNullOrEmpty()) { chkResult210.Checked = true; }
                        if (list7.NUDOMEAN2 == "1") { chkResult211.Checked = true; }
                        if (list7.NUDOMEAN3 == "1") { chkResult212.Checked = true; }
                        if (list7.NUDOMEAN4 == "1") { chkResult213.Checked = true; }
                        if (list7.NUDOMEAN5 == "1") { chkResult214.Checked = true; }
                        if (list7.NUDOMEAN6 == "1") { chkResult215.Checked = true; }
                        if (list7.NUDOMEAN7 == "1") { chkResult216.Checked = true; }
                        if (list7.NUDOMEAN8 == "1") { chkResult217.Checked = true; }
                        if (list7.NUDOMEAN9 == "1") { chkResult218.Checked = true; }
                        txtResult5.Text = list7.NUDOMEAN9_9;

                        //비활동성폐결핵
                        if (list7.NUDOUNACTIVE.IsNullOrEmpty() || list7.NUDOUNACTIVE == "1")
                        {
                            rdoResult90.Checked = true;
                        }
                        else if (list7.NUDOUNACTIVE == "2")
                        {
                            rdoResult90.Checked = false;
                        }

                        //종합판독문
                        txtResult9.Text = list7.RESULT4;
                        //종합판독범주
                        txtResult10.Text = list7.NUDOMAXRESULT;
                    }
                }
                else
                {
                    //판정완료시

                    if (tabLung1.Visible == true)
                    {
                        if (!list.LUNG_RESULT005.IsNullOrEmpty())
                        {
                            tabLung1.BackColor = Color.LightPink;
                        
                            if (list.LUNG_RESULT005 == "1") { chkResult010.Checked = true; }
                            else if (list.LUNG_RESULT005 == "2") { chkResult020.Checked = true; }
                            else if (list.LUNG_RESULT005 == "3") { chkResult030.Checked = true; }
                        
                            if (list.LUNG_RESULT007 == "1") { chkResult040.Checked = true; }
                            else if (list.LUNG_RESULT007 == "2") { chkResult050.Checked = true; }
                            else if (list.LUNG_RESULT007 == "3") { chkResult060.Checked = true; }
                            else if (list.LUNG_RESULT007 == "4") { chkResult070.Checked = true; }
                            else if (list.LUNG_RESULT007 == "5") { chkResult080.Checked = true; }

                            if (list.LUNG_RESULT006 == "1") { chkResult090.Checked = true; }
                            else if (list.LUNG_RESULT006 == "2") { chkResult100.Checked = true; }
                            else if (list.LUNG_RESULT006 == "3") { chkResult110.Checked = true; }

                            if (list.LUNG_RESULT010 == "1") { chkResult120.Checked = true; chkResult140.Checked = false; }
                            else if (list.LUNG_RESULT010 == "2") { chkResult130.Checked = true; chkResult140.Checked = false; }
                            else if (list.LUNG_RESULT010 == "3") { chkResult140.Checked = true; }
                       

                            if (list.LUNG_RESULT011 == "1") { chkResult150.Checked = true; chkResult170.Checked = false; }
                            else if (list.LUNG_RESULT011 == "2") { chkResult160.Checked = true; chkResult170.Checked = false; }
                            else if (list.LUNG_RESULT011 == "3") { chkResult170.Checked = true; }
                        
                            if (list.LUNG_RESULT012 == "1") { chkResult180.Checked = true; chkResult170.Checked = false; }
                            else if (list.LUNG_RESULT012 == "2") { chkResult190.Checked = true; chkResult170.Checked = false; }

                            if (list.LUNG_RESULT008.IsNullOrEmpty()) { txtLungResult10.Text = ""; }
                            else { txtLungResult10.Text = list.LUNG_RESULT008; }

                            if (list.LUNG_RESULT009.IsNullOrEmpty()) { txtLungResult20.Text = ""; }
                            else { txtLungResult20.Text = list.LUNG_RESULT009; }

                            txtLungResult30.Text = list.LUNG_RESULT072;
                        }
                    }

                    if (tabLung2.Visible == true)
                    {
                        if (!list.LUNG_RESULT013.IsNullOrEmpty())
                        {
                            tabLung2.BackColor = Color.LightPink;
                        
                            if (list.LUNG_RESULT013 == "1") { chkResult011.Checked = true; }
                            else if (list.LUNG_RESULT013 == "2") { chkResult021.Checked = true; }
                            else if (list.LUNG_RESULT013 == "3") { chkResult031.Checked = true; }
                         
                            if (list.LUNG_RESULT015 == "1") { chkResult041.Checked = true; }
                            else if (list.LUNG_RESULT015 == "2") { chkResult051.Checked = true; }
                            else if (list.LUNG_RESULT015 == "3") { chkResult061.Checked = true; }
                            else if (list.LUNG_RESULT015 == "4") { chkResult071.Checked = true; }
                            else if (list.LUNG_RESULT015 == "5") { chkResult081.Checked = true; }

                            if (list.LUNG_RESULT014 == "1") { chkResult091.Checked = true; }
                            else if (list.LUNG_RESULT014 == "2") { chkResult101.Checked = true; }
                            else if (list.LUNG_RESULT014 == "3") { chkResult111.Checked = true; }

                            if (list.LUNG_RESULT018 == "1") { chkResult121.Checked = true; chkResult141.Checked = false; }
                            else if (list.LUNG_RESULT018 == "2") { chkResult131.Checked = true; chkResult141.Checked = false; }
                            else if (list.LUNG_RESULT018 == "3") { chkResult141.Checked = true; }

                            if (list.LUNG_RESULT019 == "1") { chkResult151.Checked = true; chkResult171.Checked = false; }
                            else if (list.LUNG_RESULT019 == "2") { chkResult161.Checked = true; chkResult171.Checked = false; }
                            else if (list.LUNG_RESULT019 == "3") { chkResult171.Checked = true; }
                       
                            if (list.LUNG_RESULT020 == "1") { chkResult181.Checked = true; chkResult171.Checked = false; }
                            else if (list.LUNG_RESULT020 == "2") { chkResult191.Checked = true; chkResult171.Checked = false; }

                            if (list.LUNG_RESULT016.IsNullOrEmpty())
                            {
                                txtLungResult11.Text = "";
                            }
                            else
                            {
                                txtLungResult11.Text = list.LUNG_RESULT016;
                            }

                            if (list.LUNG_RESULT017.IsNullOrEmpty())
                            {
                                txtLungResult21.Text = "";
                            }
                            else
                            {
                                txtLungResult21.Text = list.LUNG_RESULT017;
                            }

                            txtLungResult31.Text = list.LUNG_RESULT073;
                        }
                    }

                    if (tabLung3.Visible == true)
                    {
                        if (!list.LUNG_RESULT021.IsNullOrEmpty())
                        {
                            tabLung3.BackColor = Color.LightPink;
                        
                            if (list.LUNG_RESULT021 == "1") { chkResult012.Checked = true; }
                            else if (list.LUNG_RESULT021 == "2") { chkResult022.Checked = true; }
                            else if (list.LUNG_RESULT021 == "3") { chkResult032.Checked = true; }

                            if (list.LUNG_RESULT023 == "1") { chkResult042.Checked = true; }
                            else if (list.LUNG_RESULT023 == "2") { chkResult052.Checked = true; }
                            else if (list.LUNG_RESULT023 == "3") { chkResult062.Checked = true; }
                            else if (list.LUNG_RESULT023 == "4") { chkResult072.Checked = true; }
                            else if (list.LUNG_RESULT023 == "5") { chkResult082.Checked = true; }

                            if (list.LUNG_RESULT022 == "1") { chkResult092.Checked = true; }
                            else if (list.LUNG_RESULT022 == "2") { chkResult102.Checked = true; }
                            else if (list.LUNG_RESULT022 == "3") { chkResult112.Checked = true; }

                            if (list.LUNG_RESULT026 == "1") { chkResult122.Checked = true; chkResult142.Checked = false; }
                            else if (list.LUNG_RESULT026 == "2") { chkResult132.Checked = true; chkResult142.Checked = false; }
                            else if (list.LUNG_RESULT026 == "3") { chkResult142.Checked = true; }

                            if (list.LUNG_RESULT027 == "1") { chkResult152.Checked = true; chkResult172.Checked = false; }
                            else if (list.LUNG_RESULT027 == "2") { chkResult162.Checked = true; chkResult172.Checked = false; }
                            else if (list.LUNG_RESULT027 == "3") { chkResult172.Checked = true; }

                            if (list.LUNG_RESULT028 == "1") { chkResult182.Checked = true; chkResult172.Checked = false; }
                            else if (list.LUNG_RESULT028 == "2") { chkResult192.Checked = true; chkResult172.Checked = false; }

                            if (list.LUNG_RESULT024.IsNullOrEmpty())
                            {
                                txtLungResult12.Text = "";
                            }
                            else
                            {
                                txtLungResult12.Text = list.LUNG_RESULT024;
                            }

                            if (list.LUNG_RESULT025.IsNullOrEmpty())
                            {
                                txtLungResult22.Text = "";
                            }
                            else
                            {
                                txtLungResult22.Text = list.LUNG_RESULT025;
                            }

                            txtLungResult32.Text = list.LUNG_RESULT074;
                        }
                    }

                    if (tabLung4.Visible == true)
                    {
                        if (!list.LUNG_RESULT029.IsNullOrEmpty())
                        {
                            tabLung4.BackColor = Color.LightPink;

                            if (list.LUNG_RESULT029 == "1") { chkResult013.Checked = true; }
                            else if (list.LUNG_RESULT029 == "2") { chkResult023.Checked = true; }
                            else if (list.LUNG_RESULT029 == "3") { chkResult033.Checked = true; }

                            if (list.LUNG_RESULT031 == "1") { chkResult043.Checked = true; }
                            else if (list.LUNG_RESULT031 == "2") { chkResult053.Checked = true; }
                            else if (list.LUNG_RESULT031 == "3") { chkResult063.Checked = true; }
                            else if (list.LUNG_RESULT031 == "4") { chkResult073.Checked = true; }
                            else if (list.LUNG_RESULT031 == "5") { chkResult083.Checked = true; }

                            if (list.LUNG_RESULT030 == "1") { chkResult093.Checked = true; }
                            else if (list.LUNG_RESULT030 == "2") { chkResult103.Checked = true; }
                            else if (list.LUNG_RESULT030 == "3") { chkResult113.Checked = true; }

                            if (list.LUNG_RESULT034 == "1") { chkResult123.Checked = true; chkResult143.Checked = false; }
                            else if (list.LUNG_RESULT034 == "2") { chkResult133.Checked = true; chkResult143.Checked = false; }
                            else if (list.LUNG_RESULT034 == "3") { chkResult143.Checked = true; }

                            if (list.LUNG_RESULT035 == "1") { chkResult153.Checked = true; chkResult173.Checked = false; }
                            else if (list.LUNG_RESULT035 == "2") { chkResult163.Checked = true; chkResult173.Checked = false; }
                            else if (list.LUNG_RESULT035 == "3") { chkResult173.Checked = true; }

                            if (list.LUNG_RESULT036 == "1") { chkResult183.Checked = true; chkResult173.Checked = false; }
                            else if (list.LUNG_RESULT036 == "2") { chkResult193.Checked = true; chkResult173.Checked = false; }

                            if (list.LUNG_RESULT032.IsNullOrEmpty())
                            {
                                txtLungResult13.Text = "";
                            }
                            else
                            {
                                txtLungResult13.Text = list.LUNG_RESULT032;
                            }
                            if (list.LUNG_RESULT033.IsNullOrEmpty())
                            {
                                txtLungResult23.Text = "";
                            }
                            else
                            {
                                txtLungResult23.Text = list.LUNG_RESULT033;
                            }

                            txtLungResult33.Text = list.LUNG_RESULT075;
                        }
                    }

                    if (tabLung5.Visible == true)
                    {
                        if (!list.LUNG_RESULT037.IsNullOrEmpty())
                        {
                            tabLung5.BackColor = Color.LightPink;
                        
                            if (list.LUNG_RESULT037 == "1") { chkResult014.Checked = true; }
                            else if (list.LUNG_RESULT037 == "2") { chkResult024.Checked = true; }
                            else if (list.LUNG_RESULT037 == "3") { chkResult034.Checked = true; }

                            if (list.LUNG_RESULT039 == "1") { chkResult044.Checked = true; } 
                            else if (list.LUNG_RESULT039 == "2") { chkResult054.Checked = true; }
                            else if (list.LUNG_RESULT039 == "3") { chkResult064.Checked = true; }
                            else if (list.LUNG_RESULT039 == "4") { chkResult074.Checked = true; }
                            else if (list.LUNG_RESULT039 == "5") { chkResult084.Checked = true; }

                            if (list.LUNG_RESULT038 == "1") { chkResult094.Checked = true; }
                            else if (list.LUNG_RESULT038 == "2") { chkResult104.Checked = true; }
                            else if (list.LUNG_RESULT038 == "3") { chkResult114.Checked = true; }

                            if (list.LUNG_RESULT042 == "1") { chkResult124.Checked = true; chkResult144.Checked = false; }
                            else if (list.LUNG_RESULT042 == "2") { chkResult134.Checked = true; chkResult144.Checked = false; }
                            else if (list.LUNG_RESULT042 == "3") { chkResult144.Checked = true; }

                            if (list.LUNG_RESULT043 == "1") { chkResult154.Checked = true; chkResult174.Checked = false; }
                            else if (list.LUNG_RESULT043 == "2") { chkResult164.Checked = true; chkResult174.Checked = false; }
                            else if (list.LUNG_RESULT043 == "3") { chkResult174.Checked = true; }

                            if (list.LUNG_RESULT044 == "1") { chkResult184.Checked = true; chkResult174.Checked = false; }
                            else if (list.LUNG_RESULT044 == "2") { chkResult194.Checked = true; chkResult174.Checked = false; }

                            if (list.LUNG_RESULT040.IsNullOrEmpty())
                            {
                                txtLungResult14.Text = "";
                            }
                            else
                            {
                                txtLungResult14.Text = list.LUNG_RESULT040;
                            }
                            if (list.LUNG_RESULT041.IsNullOrEmpty())
                            {
                                txtLungResult24.Text = "";
                            }
                            else
                            {
                                txtLungResult24.Text = list.LUNG_RESULT041;
                            }

                            txtLungResult34.Text = list.LUNG_RESULT076;
                        }
                    }

                    if (tabLung6.Visible == true)
                    {
                        if (!list.LUNG_RESULT045.IsNullOrEmpty())
                        {
                            tabLung6.BackColor = Color.LightPink;
                        
                            if (list.LUNG_RESULT045 == "1") { chkResult015.Checked = true; }
                            else if (list.LUNG_RESULT045 == "2") { chkResult025.Checked = true; }
                            else if (list.LUNG_RESULT045 == "3") { chkResult035.Checked = true; }

                            if (list.LUNG_RESULT047 == "1") { chkResult045.Checked = true; }
                            else if (list.LUNG_RESULT047 == "2") { chkResult055.Checked = true; }
                            else if (list.LUNG_RESULT047 == "3") { chkResult065.Checked = true; }
                            else if (list.LUNG_RESULT047 == "4") { chkResult075.Checked = true; }
                            else if (list.LUNG_RESULT047 == "5") { chkResult085.Checked = true; }

                            if (list.LUNG_RESULT046 == "1") { chkResult095.Checked = true; }
                            else if (list.LUNG_RESULT046 == "2") { chkResult105.Checked = true; }
                            else if (list.LUNG_RESULT046 == "3") { chkResult115.Checked = true; }

                            if (list.LUNG_RESULT050 == "1") { chkResult125.Checked = true; chkResult145.Checked = false; }
                            else if (list.LUNG_RESULT050 == "2") { chkResult135.Checked = true; chkResult145.Checked = false; }
                            else if (list.LUNG_RESULT050 == "3") { chkResult145.Checked = true; }

                            if (list.LUNG_RESULT051 == "1") { chkResult155.Checked = true; chkResult175.Checked = false; }
                            else if (list.LUNG_RESULT051 == "2") { chkResult165.Checked = true; chkResult175.Checked = false; }
                            else if (list.LUNG_RESULT051 == "3") { chkResult175.Checked = true; }

                            if (list.LUNG_RESULT052 == "1") { chkResult185.Checked = true; chkResult175.Checked = false; }
                            else if (list.LUNG_RESULT052 == "2") { chkResult195.Checked = true; chkResult175.Checked = false; }

                            if (list.LUNG_RESULT048.IsNullOrEmpty())
                            {
                                txtLungResult15.Text = "";
                            }
                            else
                            {
                                txtLungResult15.Text = list.LUNG_RESULT048;
                            }

                            if (list.LUNG_RESULT049.IsNullOrEmpty())
                            {
                                txtLungResult25.Text = "";
                            }
                            else
                            {
                                txtLungResult25.Text = list.LUNG_RESULT049;
                            }

                            txtLungResult35.Text = list.LUNG_RESULT077;
                        }
                    }

                    //폐암공동결과
                    //이전CT유무
                    if (list.LUNG_RESULT002 == "1")
                    {
                        rdoCTYN0.Checked = true;
                        txtResult6.Text = "";
                    }
                    else
                    {
                        rdoCTYN1.Checked = true;
                        txtResult6.Text = list.LUNG_RESULT003 + list.LUNG_RESULT004;
                    }

                    //선량
                    txtResult7.Text = list.LUNG_RESULT001.To<string>();

                    //기관지내 병변
                    if (list.LUNG_RESULT053 == "1")
                    {
                        rdoResult61.Checked = true;
                        txtResult11.Text = list.LUNG_RESULT054;
                    }
                    else if (list.LUNG_RESULT053 == "2" || list.LUNG_RESULT053.IsNullOrEmpty())
                    {
                        rdoResult60.Checked = true;
                    }

                    //폐결절 외 폐암시사 소견
                    if (list.LUNG_RESULT055 == "1") { chkResult200.Checked = true; }
                    else if (list.LUNG_RESULT055 == "2") { chkResult201.Checked = true; }
                    else if (list.LUNG_RESULT055 == "3") { chkResult202.Checked = true; }
                    else if (list.LUNG_RESULT055 == "4") { chkResult203.Checked = true; }
                    else if (list.LUNG_RESULT055 == "5") { chkResult204.Checked = true; }

                    txtResult4.Text = list.LUNG_RESULT056;

                    //폐결절 외 의미있는 소견
                    if (list.LUNG_RESULT057 == "1") chkResult210.Checked = true;
                    if (list.LUNG_RESULT058 == "1") chkResult211.Checked = true;
                    if (list.LUNG_RESULT059 == "1") chkResult212.Checked = true;
                    if (list.LUNG_RESULT060 == "1") chkResult213.Checked = true;
                    if (list.LUNG_RESULT061 == "1") chkResult214.Checked = true;
                    if (list.LUNG_RESULT062 == "1") chkResult215.Checked = true;
                    if (list.LUNG_RESULT063 == "1") chkResult216.Checked = true;
                    if (list.LUNG_RESULT064 == "1") chkResult217.Checked = true;
                    if (list.LUNG_RESULT065 == "1") chkResult218.Checked = true;
                    txtResult5.Text = list.LUNG_RESULT066;

                    //비활동성폐결핵
                    if (list.LUNG_RESULT067.IsNullOrEmpty() || list.LUNG_RESULT067 == "1")
                    {
                        rdoResult90.Checked = true;
                    }
                    else if (list.LUNG_RESULT067 == "2")
                    {
                        rdoResult91.Checked = true;
                    }

                    //권고사항
                    txtL_Sogen1.Text = list.LUNG_RESULT070;
                    txtL_Sogen2.Text = list.LUNG_RESULT071;

                    //종합판정
                    cboLPan1.SelectedIndex = list.LUNG_RESULT068.To<int>();
                    cboLPan2.SelectedIndex = list.LUNG_RESULT069.To<int>();
                    txtResult8.Text = list.LUNG_RESULT078;
                    txtResult9.Text = list.LUNG_RESULT079;
                    txtResult10.Text = list.LUNG_RESULT080;
                }

                long nReadDoct1 = 0;
                HIC_XRAY_RESULT list9 = hicXrayResultService.GetAllbyPtNoJepDateXCode(FstrPano, FstrJepDate, "TY10");

                if (!list9.IsNullOrEmpty())
                {
                    strXrayno = list9.XRAYNO;
                    nReadDoct1 = list9.READDOCT1;
                    //종합판독문 팍스
                    COMHPC list10 = comHpcLibBService.GetConclusionConFDr1byHisOrderId(strXrayno.Trim());

                    if (!list10.IsNullOrEmpty())
                    {
                        txtResult12.Text = list10.CONCLUSION;
                        nHicLicense1 = list10.CONFDR1.To<long>();
                    }
                }

                if (list.PANJENGDRNO11.IsNullOrEmpty() || list.PANJENGDRNO11 == 0)
                {
                    clsHcVariable.GnHicLicense1 = nReadDoct1;
                    hb.Read_DrCode(clsHcVariable.GnHicLicense1);

                    lblDrName11.Text = clsHcVariable.GstrHicDrName;
                    strPanjengDrNo[10] = clsHcVariable.GnHicLicense1.To<string>();
                    SSDR.ActiveSheet.Cells[10, 0].Text = clsHcVariable.GnHicLicense1.To<string>();

                    //팍스부분
                    if (clsHcVariable.GnHicLicense1 == 0)
                    {
                        clsHcVariable.GnHicLicense1 = nHicLicense1;
                        hb.Read_DrCode(clsHcVariable.GnHicLicense1);
                        lblDrName11.Text = clsHcVariable.GstrHicDrName;
                        strPanjengDrNo[10] = clsHcVariable.GnHicLicense1.To<string>();
                        SSDR.ActiveSheet.Cells[10, 0].Text = clsHcVariable.GnHicLicense1.To<string>();
                    }

                    clsHcVariable.GstrHicDrName = "";
                    clsHcVariable.GnHicLicense1 = 0;
                }
                else
                {
                    SSDR.ActiveSheet.Cells[10, 0].Text = list.PANJENGDRNO11.To<string>();

                    lblDrName11.Text = hb.READ_HIC_DRCODE3(list.PANJENGDRNO11);
                    lblDrName11.Text = lblDrName11.Text.Trim();
                }
            }
            #endregion

            //의사세팅========================================================================================================

            if (tab1.Enabled == true)
            {
                if (list.NEW_WOMAN32.IsNullOrEmpty())
                {
                    txtDrNSabun0.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun0.Text = list.NEW_WOMAN32;   //의사면허번호
                }

                if (txtDrNSabun0.Text.IsNullOrEmpty() || txtDrNSabun0.Text.To<long>() == 0)
                {
                    txtDrNSabun0.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName00.Text = hb.READ_License_DrName(txtDrNSabun0.Text.To<long>());
            }

            if (tab2.Enabled == true)
            {
                if (list.NEW_WOMAN33.IsNullOrEmpty())
                {
                    txtDrNSabun1.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun1.Text = list.NEW_WOMAN33;   //의사면허번호
                }

                if (txtDrNSabun1.Text.IsNullOrEmpty() || txtDrNSabun1.Text.To<long>() == 0)
                {
                    txtDrNSabun1.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName01.Text = hb.READ_License_DrName(txtDrNSabun1.Text.To<long>());
            }

            if (tab3.Enabled == true)
            {
                if (list.NEW_WOMAN34.IsNullOrEmpty())
                {
                    txtDrNSabun2.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun2.Text = list.NEW_WOMAN34;   //의사면허번호
                }

                if (txtDrNSabun2.Text.IsNullOrEmpty() || txtDrNSabun2.Text.To<long>() == 0)
                {
                    txtDrNSabun2.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName02.Text = hb.READ_License_DrName(txtDrNSabun2.Text.To<long>());
            }

            if (tab4.Enabled == true)
            {
                if (list.NEW_WOMAN35.IsNullOrEmpty())
                {
                    txtDrNSabun3.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun3.Text = list.NEW_WOMAN35;   //의사면허번호
                }

                if (txtDrNSabun3.Text.IsNullOrEmpty() || txtDrNSabun3.Text.To<long>() == 0)
                {
                    txtDrNSabun3.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName03.Text = hb.READ_License_DrName(txtDrNSabun3.Text.To<long>());
            }

            if (tab5.Enabled == true)
            {
                if (list.NEW_WOMAN36.IsNullOrEmpty())
                {
                    txtDrNSabun4.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun4.Text = list.NEW_WOMAN36;   //의사면허번호
                }

                if (txtDrNSabun4.Text.IsNullOrEmpty() || txtDrNSabun4.Text.To<long>() == 0)
                {
                    txtDrNSabun4.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName04.Text = hb.READ_License_DrName(txtDrNSabun4.Text.To<long>());
            }

            if (tab6.Enabled == true)
            {
                if (list.NEW_WOMAN37.IsNullOrEmpty())
                {
                    txtDrNSabun5.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                else
                {
                    txtDrNSabun5.Text = list.NEW_WOMAN37;   //의사면허번호
                }

                if (txtDrNSabun5.Text.IsNullOrEmpty() || txtDrNSabun5.Text.To<long>() == 0)
                {
                    txtDrNSabun5.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                lblDrName05.Text = hb.READ_License_DrName(txtDrNSabun5.Text.To<long>());
            }

            //판정완료여부
            for (int i = 0; i <= 5; i++)
            {
                strTemp = "OK";
                CheckBox chkCancer = (Controls.Find("chkCancer" + i.To<string>(), true)[0] as CheckBox);
                if (chkCancer.Checked == true)
                {
                    if (i == 0)
                    {
                        if (txtS_Sogen.Text.Trim() == "" && txtS_Sogen2.Text.Trim() == "") { strTemp = ""; }
                        if (cboSPan.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab1.Text += "\r\n" + "판정 미완료";
                            tab1.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab1.Text += "\r\n" + "판정 완료";
                            tab1.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }
                    else if (i == 1)
                    {
                        if (txtC_Sogen.Text.Trim() == "" && txtC_Sogen2.Text.Trim() == "" && txtC_Sogen3.Text.Trim() == "") { strTemp = ""; }
                        if (cboCPan.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab2.Text += "\r\n" + "판정 미완료";
                            tab2.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab2.Text += "\r\n" + "판정 완료";
                            tab2.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }
                    else if (i == 2)
                    {
                        if (txtL_Sogen.Text.Trim() == "") { strTemp = ""; }
                        if (cboLPan.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab3.Text += "\r\n" + "판정 미완료";
                            tab3.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab3.Text += "\r\n" + "판정 완료";
                            tab3.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }

                    else if (i == 3)
                    {
                        if (txtB_Sogen.Text.Trim() == "") { strTemp = ""; }
                        if (cboBPan.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab4.Text += "\r\n" + "판정 미완료";
                            tab4.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab4.Text += "\r\n" + "판정 완료";
                            tab4.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }

                    else if (i == 4)
                    {
                        if (txtW_Sogen.Text.Trim() == "") { strTemp = ""; }
                        if (cboWPan.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab5.Text += "\r\n" + "판정 미완료";
                            tab5.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab5.Text += "\r\n" + "판정 완료";
                            tab5.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }

                    else if (i == 5)
                    {
                        if (txtL_Sogen1.Text.Trim() == "") { strTemp = ""; }
                        if (cboLPan1.SelectedIndex == 0) { strTemp = ""; }
                        if (strTemp.IsNullOrEmpty())
                        {
                            tab6.Text += "\r\n" + "판정 미완료";
                            tab6.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Red;
                        }
                        else
                        {
                            tab6.Text += "\r\n" + "판정 완료";
                            tab6.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Green;
                        }
                    }
                }
            }

            //분변잠혈 결과가 음성인 경우 자동판정
            if (FstrJob == "1") //미판정
            {
                List<string> sExCode = new List<string>();
                sExCode.Clear();
                sExCode.Add("TX26");

                List<HIC_RESULT> list2 = hicResultService.GetResultbyWrtNoExCode(FnWrtNo, sExCode);

                if (list2.Count > 0)
                {
                    bOK = false;
                    if (!list2[0].RESULT.IsNullOrEmpty())
                    {
                        if (VB.Left(list2[0].RESULT, 1) == ">")
                        {
                            bOK = false;
                        }
                        else if (!list2[0].RESULT.IsNumeric())
                        {
                            bOK = false;
                        }
                        else if (list2[0].RESULT.To<int>() <= 100)
                        {
                            bOK = true;
                        }
                    }

                    if (bOK == true)
                    {
                        cboDenotationSubcutaneousBlood.SelectedIndex = 1;
                        txtC_Sogen.Text = "분변잠혈반응 검사 결과 음성(대변에서 혈액이 검출되지 않음)입니다. 이러한 경우 대장에 아무런 이상이 없을 가능성이 높으나 경우에 따라 대장에 병변(염증, 용종, 암 등)이 있는 경우에도 정상으로 나올 수 있습니다. 그러므로 최근 의심되는 증상(체중감소, 대변 굵기의 변화, 혈변 등)이 있으면 의료기관을 방문하시어 진료상담을 받으시기 바랍니다.";
                        if (cboCPan.Items.Contains("6.잠혈반응없음"))
                        {
                            cboCPan.SelectedItem = "6.잠혈반응없음";
                        }
                        //cboCPan.SelectedIndex = 6;
                        dtpCPanDate.Text = clsPublic.GstrSysDate;
                        lblDrName01.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
                    }
                }
            }
        }

        private void GetCytologyResult_Separator_Words(PanWomB panWomB, string[] strWomBArr)
        {
            string strWomB6 = string.Empty;
            string[] separator = new string[1]{"\r\n"};
            string[] strWords = null;

            try
            {
                if (strWomBArr == null || strWomBArr.Length < 3)
                {
                    panWomB = null;
                    return;
                }

                for (int i = 0; i < strWomBArr.Length; i++)
                {
                    switch (i)
                    {
                        case 4: panWomB.WOMB01 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim(); break;
                        case 5: panWomB.WOMB02 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim(); break;
                        case 6: panWomB.WOMB03 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim(); break;
                        case 7: panWomB.WOMB04 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim(); 
                            panWomB.WOMAN12 = VB.Pstr(strWomBArr[i], ":", 3).Trim(); break;
                        case 8: panWomB.WOMB05 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim();
                            panWomB.RESULT0014 = VB.Pstr(strWomBArr[i], ":", 3).Trim(); break;
                        case 10: panWomB.WOMB07 = VB.Pstr(VB.Pstr(strWomBArr[i], ":", 2), ".", 1).Trim(); break;
                        default: break;
                    }
                }

                //기타소견 정렬
                strWords = strWomBArr[9].Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (FindWord(strWords, "2")) { strWomB6 = "2"; }
                else if (FindWord(strWords, "3")) { strWomB6 = "3"; }
                else if (FindWord(strWords, "4")) { strWomB6 = "4"; }
                else if (FindWord(strWords, "5")) { strWomB6 = "5"; }
                else
                {
                    for (int i = 0; i < strWords.Length; i++)
                    {
                        if (VB.Right(VB.Pstr(strWords[i], ".", 1), 1).Trim() != "")
                        {
                            strWomB6 = VB.Right(VB.Pstr(strWords[i], ".", 1), 1).Trim();
                            break;
                        }
                    }
                }

                panWomB.WOMB06 = strWomB6;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                panWomB = null;
            }
        }

        bool FindWord(string[] arr, string argT)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].IndexOf(argT) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        void eCboSelectedIndexChanged(object sender, EventArgs e)
        {
            string strCtrl = "";
            string strCtrl2 = "";

            if (sender == cboResult0001)
            {
                if (cboResult0001.Text.Trim() != "" && VB.Left(cboResult0001.Text, 2) == "07")
                {
                    panel16.Enabled = true;
                    panel17.Enabled = false;
                    rdoStomachTissue0.Checked = true;

                    eRdoClick(rdoStomachTissue0, new EventArgs());
                }
                else if (cboResult0001.Text.Trim() != "" && VB.Left(cboResult0001.Text, 2) == "08")
                {
                    panel16.Enabled = false;
                    panel17.Enabled = true;
                    rdoStomachTissue0.Checked = true;

                    eRdoClick(rdoStomachTissue0, new EventArgs());
                }
                else
                {
                    if (cboResult0001.Text.Trim() != "")
                    {
                        rdoStomachTissue0.Checked = true;
                        eRdoClick(rdoStomachTissue0, new EventArgs());
                    }
                    else
                    {
                        rdoStomachTissue1.Checked = true;
                    }

                    for (int i = 0; i <= 13; i++)
                    {
                        CheckBox chkbreastR = (Controls.Find("chkStomachTissue" + i.ToString(), true)[0] as CheckBox);
                        chkbreastR.Checked = false;
                    }

                    panel16.Enabled = false;
                    panel17.Enabled = false;
                }
            }
            else if (sender == cboSPan)
            {
                if (VB.Pstr(cboSPan.Text, ".", 1).Trim() == "5")
                {
                    txtJilEtcS.Enabled = true;
                }
                else
                {
                    txtJilEtcS.Enabled = false;
                }
            }
            else if (sender == cboCPan)
            {
                if (TabColono.SelectedTab == tabColon1)
                {
                    if (VB.Left(cboCPan.Text, 1).To<long>() > 0 && VB.Left(cboCPan.Text, 1).To<long>() < 6)
                    {
                        MessageBox.Show("분변잠혈반응검사는 음성or양성 으로만 판정가능.", "종합판정오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cboCPan.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (VB.Left(cboCPan.Text, 1).To<long>() > 0 && VB.Left(cboCPan.Text, 1).To<long>() > 5)
                    {
                        MessageBox.Show("대장암검사는 음성or양성 으로 판정불가.", "종합판정오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cboCPan.SelectedIndex = 0;
                    }
                }

                if (VB.Pstr(cboCPan.Text, ".", 1).Trim() == "5")
                {
                    txtJilEtcC.Enabled = true;
                }
                else
                {
                    txtJilEtcC.Enabled = false;
                }
            }
            else if (sender == cboLPan)
            {
                if (grpHepatitisExam.Enabled == true)
                {
                    if (VB.Left(cboLPan.Text, 1).To<long>() > 0 && VB.Left(cboLPan.Text, 1).To<long>() < 5)
                    {
                        MessageBox.Show("간염검사는 종합판정구분 5번부터 판정가능.", "종합판정오류(간염)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cboLPan.SelectedIndex = 0;
                    }
                }

                if (grpLiverUltrasonography1.Enabled == true || grpLiverUltrasonography2.Enabled == true)
                {
                    if (VB.Left(cboLPan.Text, 1).To<long>() > 0 && VB.Left(cboLPan.Text, 1).To<long>() > 4)
                    {
                        MessageBox.Show("간초음파검사는 종합판정구분 1번부터 4번까지 판정가능.", "종합판정오류(간초음파)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cboLPan.SelectedIndex = 0;
                    }
                }

                if (VB.Pstr(cboLPan.Text, ".", 1).Trim() == "4")
                {
                    txtJilEtcL.Enabled = true;
                }
                else
                {
                    txtJilEtcL.Enabled = false;
                }
            }
            else if (sender == cboWPan)
            {
                if (VB.Pstr(cboWPan.Text, ".", 1).Trim() == "6")
                {
                    txtJilEtcM.Enabled = true;
                }
                else
                {
                    txtJilEtcM.Enabled = false;
                }

                FstrWomBoCode = "A171";
                FstrWomBo = VB.Format(VB.Val(VB.Pstr(cboWPan.Text, ".", 1)), "00");

                rResultChange(FstrWomBoCode, FstrWomBo);

            }
            else if (sender == cboLPan1)
            {
                if (VB.Left(cboLPan1.Text, 1) == "4")
                {
                    cboLPan2.Enabled = true;
                }
                else
                {
                    cboLPan2.Enabled = false;
                }
            }
            //위장조영 판정시 병변위치 선택가능 여부 Set
            else if (sender == cboForgedYoung0 || sender == cboForgedYoung1 || sender == cboForgedYoung2)
            {
                if (sender == cboForgedYoung0) { strCtrl = "chkForgedYoung1"; }
                else if (sender == cboForgedYoung1) { strCtrl = "chkForgedYoung2"; }
                else if (sender == cboForgedYoung2) { strCtrl = "chkForgedYoung3"; }

                //1.이상소견없음,  9.기타 의 경우 병변위치 선택불가
                if (((ComboBox)sender).Text.Trim() == "" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "1" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "9")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkForgedYoung = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkForgedYoung.Checked = false; chkForgedYoung.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkForgedYoung = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkForgedYoung.Enabled = true;
                    }
                }

                if (VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "9")
                {
                    panel11.Enabled = true;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkForgedYoungEtc = (Controls.Find("chkForgedYoungEtc" + i.To<string>(), true)[0] as CheckBox);
                        chkForgedYoungEtc.Checked = false;
                    }
                    txtForgedYoungEtc.Text = "";
                    panel11.Enabled = false;
                }

            }
            //위내시경 판정시 병변위치 선택가능 여부 Set
            else if (sender == cboEndo0 || sender == cboEndo1 || sender == cboEndo2)
            {
                if (sender == cboEndo0) { strCtrl = "chkEndo1"; }
                else if (sender == cboEndo1) { strCtrl = "chkEndo2"; }
                else if (sender == cboEndo2) { strCtrl = "chkEndo3"; }

                string strGbEtc = "";
                if (VB.Pstr(cboEndo0.Text,".",1) == "09" || VB.Pstr(cboEndo1.Text, ".", 1) == "09" || VB.Pstr(cboEndo2.Text, ".", 1) == "09")
                {
                    strGbEtc = "OK";
                }

                //01.이상소견없음,  09.기타 의 경우 병변위치 선택불가
                if (((ComboBox)sender).Text.Trim() == "" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "01" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "09")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkEndo = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkEndo.Checked = false; chkEndo.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkEndo = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkEndo.Enabled = true;
                    }
                }

                if (VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "09")
                {
                    panel9.Enabled = true;
                }
                else
                {
                    if (strGbEtc == "")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + i.To<string>(), true)[0] as CheckBox);
                            chkEndoEtc.Checked = false;
                        }
                        txtEndoEtc.Text = "";
                        panel9.Enabled = false;
                    }

                }
            }
            //결장이중조영 판정시 병변위치 선택가능 여부 Set
            else if (sender == cboColonizationAassistant0 || sender == cboColonizationAassistant1 || sender == cboColonizationAassistant2)
            {
                if (sender == cboColonizationAassistant0) { strCtrl = "chkColonizationAassistant1"; }
                else if (sender == cboColonizationAassistant1) { strCtrl = "chkColonizationAassistant2"; }
                else if (sender == cboColonizationAassistant2) { strCtrl = "chkColonizationAassistant3"; }

                //1.이상소견없음 의 경우 병변위치 선택불가
                if (((ComboBox)sender).Text.Trim() == "" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "1")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkColonizationAassistant = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkColonizationAassistant.Checked = false; chkColonizationAassistant.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkColonizationAassistant = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkColonizationAassistant.Enabled = true;
                    }
                }
            }
            //대장내시경 판정시 병변위치 선택가능 여부 Set
            else if (sender == cboColonoScope0 || sender == cboColonoScope1 || sender == cboColonoScope2)
            {
                if (sender == cboColonoScope0) { strCtrl = "chkColonoScope1"; }
                else if (sender == cboColonoScope1) { strCtrl = "chkColonoScope2"; }
                else if (sender == cboColonoScope2) { strCtrl = "chkColonoScope3"; }

                //1.이상소견없음의 경우 병변위치 선택불가
                if (((ComboBox)sender).Text.Trim() == "" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "1")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkColonoScope = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkColonoScope.Checked = false; chkColonoScope.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        CheckBox chkColonoScope = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkColonoScope.Enabled = true;
                    }
                }
            }
            //유방암 판정시 병변위치 선택가능 여부 Set
            else if (sender == cboBreast0 || sender == cboBreast1 || sender == cboBreast2)
            {
                if (sender == cboBreast0) { strCtrl = "chkBreastR1"; strCtrl2 = "chkBreastL1"; }
                else if (sender == cboBreast1) { strCtrl = "chkBreastR2"; strCtrl2 = "chkBreastL2"; }
                else if (sender == cboBreast2) { strCtrl = "chkBreastR3"; strCtrl2 = "chkBreastL3"; }

                //01.이상소견없음,  10.직접기입 의 경우 병변위치 선택불가
                if (((ComboBox)sender).Text.Trim() == "" || VB.Pstr(((ComboBox)sender).Text.Trim(), ".", 1) == "01")
                {
                    for (int i = 0; i < 7; i++)
                    {
                        CheckBox chkBreastR = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkBreastR.Checked = false; chkBreastR.Enabled = false;

                        CheckBox chkBreastL = (Controls.Find(strCtrl2 + i.To<string>(), true)[0] as CheckBox);
                        chkBreastL.Checked = false; chkBreastL.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        CheckBox chkBreastR = (Controls.Find(strCtrl + i.To<string>(), true)[0] as CheckBox);
                        chkBreastR.Enabled = true;

                        CheckBox chkBreastL = (Controls.Find(strCtrl2 + i.To<string>(), true)[0] as CheckBox);
                        chkBreastL.Enabled = true;
                    }
                }
            }
        }

        void fn_Screen_Clear()
        {
            tabDefalut.Visible = true;
            tab1.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            tab2.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            tab3.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            tab4.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            tab5.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            tab6.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Default;
            superTabControl1.SelectedTab = tabDefalut;
            superTabControl1.SelectedTab = rTab_List;

            FstrJepDate = "";
            FstrSex = "";
            FstrPtno = "";
            FstrPano = "";
            FstrJob = "";
            FnPano = 0;

            tab1.Enabled = false;
            tab2.Enabled = false;
            tab3.Enabled = false;
            tab4.Enabled = false;
            tab5.Enabled = false;
            tab6.Enabled = false;

            tab1.Text = "위암";
            tab2.Text = "대장암";
            tab3.Text = "간암";
            tab4.Text = "유방암";
            tab5.Text = "자궁경부암";
            tab6.Text = "폐암";

            TabStomach.SelectedTabIndex = 0;
            TabColono.SelectedTabIndex = 1;

            tabStomach1.Visible = false;        //위암 - 위장조영 검사
            tabStomach2.Visible = false;        //위암 - 위내시경 검사 / 조직진단

            tabColon1.Visible = false;          //대장암 - 분변잠혈반응 검사
            tabColon2.Visible = false;          //대장암 - 결장이중조영 검사
            tabColon3.Visible = false;          //대장암 - 대장내시경 검사

            tabLung1.Visible = true; tabLung1.BackColor = Color.Transparent;          //폐암 - 1
            tabLung2.Visible = true; tabLung2.BackColor = Color.Transparent;          //폐암 - 2
            tabLung3.Visible = true; tabLung3.BackColor = Color.Transparent;          //폐암 - 3
            tabLung4.Visible = true; tabLung4.BackColor = Color.Transparent;          //폐암 - 4
            tabLung5.Visible = true; tabLung5.BackColor = Color.Transparent;          //폐암 - 5
            tabLung6.Visible = true; tabLung6.BackColor = Color.Transparent;          //폐암 - 6

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.To<string>(), true)[0] as CheckBox);
                chkMirGbn.Checked = false;
                TextBox txtDoctor = (Controls.Find("txtDrNSabun" + i.To<string>(), true)[0] as TextBox);
                txtDoctor.Text = "";
            }

            txtResult1.Text = "";

            btnSave.Enabled = false;
            btnSave.Visible = false; //판정저장

            //위암판정 Clear========================================================================
            rdoStomachEndo1.Checked = true;
            rdoStomachTissue1.Checked = true;

            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkForgedYoung1 = (Controls.Find("chkForgedYoung1" + i.ToString(), true)[0] as CheckBox);      //위장조영 관찰소견 1
                CheckBox chkForgedYoung2 = (Controls.Find("chkForgedYoung2" + i.ToString(), true)[0] as CheckBox);      //위장조영 관찰소견 2
                CheckBox chkForgedYoung3 = (Controls.Find("chkForgedYoung3" + i.ToString(), true)[0] as CheckBox);      //위장조영 관찰소견 3
                CheckBox chkForgedYoungEtc = (Controls.Find("chkForgedYoungEtc" + i.ToString(), true)[0] as CheckBox);  //위장조영 관찰소견 기타

                CheckBox chkEndo1 = (Controls.Find("chkEndo1" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkEndo2 = (Controls.Find("chkEndo2" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkEndo3 = (Controls.Find("chkEndo3" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + i.ToString(), true)[0] as CheckBox);

                CheckBox chkStomachTissueEtc = (Controls.Find("chkStomachTissueEtc" + i.ToString(), true)[0] as CheckBox);

                chkForgedYoung1.Checked = false;    
                chkForgedYoung2.Checked = false;    
                chkForgedYoung3.Checked = false;    
                chkForgedYoungEtc.Checked = false;  

                chkEndo1.Checked = false;     
                chkEndo2.Checked = false;     
                chkEndo3.Checked = false;     
                chkEndoEtc.Checked = false;   

                chkStomachTissueEtc.Checked = false; 
            }

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkCancer = (Controls.Find("chkCancer" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSangdam = (Controls.Find("chkSangdam" + i.ToString(), true)[0] as CheckBox);
                chkCancer.Checked = false;
                chkAm.Checked = false;
                chkMirGbn.Checked = false;
                chkSangdam.Checked = false;

                Label lblDoctName = (Controls.Find("lblDrName0" + i.ToString(), true)[0] as Label);
                lblDoctName.Text = "";
            }

            for (int i = 0; i <= 13; i++)
            {
                CheckBox chkStomachTissue = (Controls.Find("chkStomachTissue" + i.ToString(), true)[0] as CheckBox);
                chkStomachTissue.Checked = false;
            }

            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + i.ToString(), true)[0] as ComboBox);
                ComboBox cboEndo = (Controls.Find("cboEndo" + i.ToString(), true)[0] as ComboBox);
                cboForgedYoung.SelectedIndex = 0;
                cboEndo.SelectedIndex = 0;
            }

            cboStomachTissueCnt.SelectedIndex = 0;
            cboResult0001.SelectedIndex = 0;
            //cboResult0001.Enabled = false;
            cboSPan.SelectedIndex = 0;

            txtForgedYoungEtc.Text = "";
            txtStomachTissueEtc.Text = "";
            txtStomachTissueEtcCancer.Text = "";
            txtEndoEtc.Text = "";

            txtJilEtcS.Text = "";
            txtS_Sogen.Text = "";
            txtS_Sogen2.Text = "";
            dtpSPanDate.Checked = false;
            //dtpSPanDate.Text = "";
            lblDrName1.Text = "";
            lblDrName2.Text = "";
            lblDrName3.Text = "";

            //대장암판정 Clear========================================================================
            txtC_Sogen.Text = "";
            txtC_Sogen2.Text = "";
            txtC_Sogen3.Text = "";

            txtCSize0.Text = "";
            txtCSize1.Text = "";

            rdoColonTissue1.Checked = true;
            rdoCut1.Checked = true;
            rdoResult00021.Checked = true;
            rdoResult00031.Checked = true;

            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkColonizationAassistant1 = (Controls.Find("chkColonizationAassistant1" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkColonizationAassistant2 = (Controls.Find("chkColonizationAassistant2" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkColonizationAassistant3 = (Controls.Find("chkColonizationAassistant3" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkColonizationAassistantEtc = (Controls.Find("chkColonizationAassistantEtc" + i.ToString(), true)[0] as CheckBox);


                chkColonizationAassistant1.Checked = false;
                chkColonizationAassistant2.Checked = false;
                chkColonizationAassistant3.Checked = false;
                chkColonizationAassistantEtc.Checked = false;
            }

            for (int i = 0; i <= 12; i++)
            {
                CheckBox chkColonCancer = (Controls.Find("chkColonCancer" + i.ToString(), true)[0] as CheckBox);
                chkColonCancer.Checked = false;
            }

            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkColonCancer = (Controls.Find("chkColonCancer" + i.ToString(), true)[0] as CheckBox);
                chkColonCancer.Checked = false;
            }

            cboDenotationSubcutaneousBlood.SelectedIndex = 0;   //검사결과(정량검사) : 분별잠혈
            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + i.ToString(), true)[0] as ComboBox);
                ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + i.ToString(), true)[0] as ComboBox);
                cboColonizationAassistant.SelectedIndex = 0;
                cboColonoScope.SelectedIndex = 0;
            }

            cboColonTissue.SelectedIndex = 0;
            cboColonTissueCnt.SelectedIndex = 0;
            cboCPan.SelectedIndex = 0;

            txtColonizationAassistantEtc.Text = "";
            txtColonScopeEtc.Text = "";
            txtColonEtc.Text = "";
            txtColonCancerEtc.Text = "";
            dtpCPanDate.Checked = false;
            //dtpCPanDate.Text = "";
            txtJilEtcC.Text = "";
            lblDrName4.Text = "";
            lblDrName5.Text = "";
            lblDrName6.Text = "";

            txtResult.Text = "";

            //간암판정 Clear========================================================================
            for (int i = 0; i < 4; i++)
            {
                CheckBox chkResult0004 = (Controls.Find("chkResult0004" + i.ToString(), true)[0] as CheckBox);
                chkResult0004.Checked = false;
            }

            txtALT.Text = "";
            txtEIA1.Text = "";
            txtEIA2.Text = "";

            grpHepatitisExam.Enabled = false;
            grpLiverUltrasonography1.Enabled = false;
            grpLiverUltrasonography2.Enabled = false;

            cboNew_B.SelectedIndex = 0;
            cboBRsult.SelectedIndex = 0;
            cboNew_C.SelectedIndex = 0;
            cboCResult.SelectedIndex = 0;
            cboLPan.SelectedIndex = 0;

            txtL_Sogen.Text = "";
            dtpLPanDate.Checked = false;
            //dtpLPanDate.Text = "";
            txtJilEtcL.Text = "";

            lbllDrName7.Text = "";
            txtEIA1.Text = "";

            //유방암판정 Clear========================================================================
            cboBreast.SelectedIndex = 0;
            cboBreast0.SelectedIndex = 0;
            cboBreast1.SelectedIndex = 0;
            cboBreast2.SelectedIndex = 0;
            cboBPan.SelectedIndex = 0;

            for (int i = 0; i <= 6; i++)
            {
                CheckBox chkBreastR1 = (Controls.Find("chkBreastR1" + i.ToString(), true)[0] as CheckBox);
                chkBreastR1.Checked = false;
                CheckBox chkBreastL1 = (Controls.Find("chkBreastL1" + i.ToString(), true)[0] as CheckBox);
                chkBreastL1.Checked = false;
                CheckBox chkBreastR2 = (Controls.Find("chkBreastR2" + i.ToString(), true)[0] as CheckBox);
                chkBreastR2.Checked = false;
                CheckBox chkBreastL2 = (Controls.Find("chkBreastL2" + i.ToString(), true)[0] as CheckBox);
                chkBreastL2.Checked = false;
                CheckBox chkBreastR3 = (Controls.Find("chkBreastR3" + i.ToString(), true)[0] as CheckBox);
                chkBreastR3.Checked = false;
                CheckBox chkBreastL3 = (Controls.Find("chkBreastL3" + i.ToString(), true)[0] as CheckBox);
                chkBreastL3.Checked = false;
            }

            txtBreastPosEtc10.Text = "";
            txtBreastPosEtc11.Text = "";
            txtBreastPosEtc20.Text = "";
            txtBreastPosEtc21.Text = "";
            txtBreastPosEtc30.Text = "";
            txtBreastPosEtc31.Text = "";
            txtBreastReadOpinionEtc.Text = "";

            txtB_Sogen.Text = "";
            dtpBPanDate.Checked = false;
            //dtpBPanDate.Text = "";
            txtJilEtcB.Text = "";
            lblDrName8.Text = "";

            //자궁경부암판정 Clear========================================================================
            rdoWomBo11.Checked = true;
            rdoWomBo22.Checked = true;
            rdoWomBo31.Checked = true;
            chkWomBo4.Checked = false;
            rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
            chkWomBo51.Checked = false; chkWomBo52.Checked = false;
            panWomBo3.Enabled = false;

            chkWomBo6.Checked = false;
            rdoWomBo61.Checked = false; rdoWomBo62.Checked = false; rdoWomBo63.Checked = false;
            chkResult00141.Checked = false; chkResult00142.Checked = false;

            rdoWomBo71.Checked = true;
            cboWPan.SelectedIndex = 0;

            rdoCervixDuplex1.Checked = true;
            txtWombo3_Etc.Text = "";
            txtWombo3_Etc.Enabled = false;
            txtWombo6_Etc.Text = "";
            txtWombo6_Etc.Enabled = false;
            txtWombo7_Etc.Text = "";
            txtWombo7_Etc.Enabled = false;
            txtW_Sogen.Text = "";
            dtpWPanDate.Checked = false;
            //dtpWPanDate.Text = "";
            txtJilEtcM.Text = "";
            lblDrName9.Text = "";
            lblDrName10.Text = "";

            //폐암판정 Clear========================================================================
            pnlPanLungResult.Visible = false;
            rdoCTYN0.Checked = true;
            for (int i = 0; i <= 5; i++)
            {
                TextBox txtLungResult1 = (Controls.Find("txtLungResult1" + i.ToString(), true)[0] as TextBox);
                txtLungResult1.Text = "";
                TextBox txtLungResult2 = (Controls.Find("txtLungResult2" + i.ToString(), true)[0] as TextBox);
                txtLungResult2.Text = "";
                TextBox txtLungResult3 = (Controls.Find("txtLungResult3" + i.ToString(), true)[0] as TextBox);
                txtLungResult3.Text = "";

                Panel panResult01 = (Controls.Find("panResult01" + i.ToString(), true)[0] as Panel);
                panResult01.Enabled = false;

            }

            txtResult4.Text = "";
            txtResult5.Text = "";
            txtResult6.Text = "";
            txtResult7.Text = "";
            txtResult8.Text = "";
            txtResult9.Text = "";
            txtResult10.Text = "";
            txtResult11.Text = "";
            txtResult12.Text = "";

            for (int i = 0; i <= 5; i++)
            {
                RadioButton chkResult01 = (Controls.Find("chkResult01" + i.ToString(), true)[0] as RadioButton);
                chkResult01.Checked = false;
                RadioButton chkResult02 = (Controls.Find("chkResult02" + i.ToString(), true)[0] as RadioButton);
                chkResult02.Checked = false;
                RadioButton chkResult03 = (Controls.Find("chkResult03" + i.ToString(), true)[0] as RadioButton);
                chkResult03.Checked = false;
                CheckBox chkResult04 = (Controls.Find("chkResult04" + i.ToString(), true)[0] as CheckBox);
                chkResult04.Checked = false;
                CheckBox chkResult05 = (Controls.Find("chkResult05" + i.ToString(), true)[0] as CheckBox);
                chkResult05.Checked = false;

                CheckBox chkResult06 = (Controls.Find("chkResult06" + i.ToString(), true)[0] as CheckBox);
                chkResult06.Checked = false;
                CheckBox chkResult07 = (Controls.Find("chkResult07" + i.ToString(), true)[0] as CheckBox);
                chkResult07.Checked = false;
                CheckBox chkResult08 = (Controls.Find("chkResult08" + i.ToString(), true)[0] as CheckBox);
                chkResult08.Checked = false;
                CheckBox chkResult09 = (Controls.Find("chkResult09" + i.ToString(), true)[0] as CheckBox);
                chkResult09.Checked = false;
                CheckBox chkResult10 = (Controls.Find("chkResult10" + i.ToString(), true)[0] as CheckBox);
                chkResult10.Checked = false;
                CheckBox chkResult11 = (Controls.Find("chkResult11" + i.ToString(), true)[0] as CheckBox);
                chkResult11.Checked = false;
                CheckBox chkResult12 = (Controls.Find("chkResult12" + i.ToString(), true)[0] as CheckBox);
                chkResult12.Checked = false;
                CheckBox chkResult13 = (Controls.Find("chkResult13" + i.ToString(), true)[0] as CheckBox);
                chkResult13.Checked = false;
                CheckBox chkResult14 = (Controls.Find("chkResult14" + i.ToString(), true)[0] as CheckBox);
                chkResult14.Checked = false;
                CheckBox chkResult15 = (Controls.Find("chkResult15" + i.ToString(), true)[0] as CheckBox);
                chkResult15.Checked = false;
                CheckBox chkResult16 = (Controls.Find("chkResult16" + i.ToString(), true)[0] as CheckBox);
                chkResult16.Checked = false;
                CheckBox chkResult17 = (Controls.Find("chkResult17" + i.ToString(), true)[0] as CheckBox);
                chkResult17.Checked = false;
                CheckBox chkResult18 = (Controls.Find("chkResult18" + i.ToString(), true)[0] as CheckBox);
                chkResult18.Checked = false;
                CheckBox chkResult19 = (Controls.Find("chkResult19" + i.ToString(), true)[0] as CheckBox);
                chkResult19.Checked = false;
            }

            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkResult20 = (Controls.Find("chkResult20" + i.ToString(), true)[0] as CheckBox);
                chkResult20.Checked = false;
            }

            for (int i = 0; i <= 8; i++)
            {
                CheckBox chkResult21 = (Controls.Find("chkResult21" + i.ToString(), true)[0] as CheckBox);
                chkResult21.Checked = false;
            }

            txtL_Sogen1.Text = "";
            txtL_Sogen2.Text = "";

            cboLPan1.SelectedIndex = 0;
            cboLPan2.SelectedIndex = 0;

            SSDR.ActiveSheet.Cells[0, 0, 10, 0].Text = "";
            lblDrName11.Text = "";
            dtpWPanDate.Text = "";
            dtpLPanDate1.Checked = false;
            //dtpLPanDate1.Text = "";

            clsHcVariable.GnHicLicense1 = 0;

            panList01_Control_Set("1");
        }

        /// <summary>
        /// 위암 화면구성
        /// </summary>
        void fn_Control_Set_Stomach()
        {
            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + i.ToString(), true)[0] as ComboBox);
                cboForgedYoung.Items.Clear();
                cboForgedYoung.Items.Add("");
                cboForgedYoung.Items.Add("1.이상소견없음");
                cboForgedYoung.Items.Add("2.위염");
                cboForgedYoung.Items.Add("3.위암의심");
                cboForgedYoung.Items.Add("4.조기위암");
                cboForgedYoung.Items.Add("5.진행위암");
                cboForgedYoung.Items.Add("6.양성위궤양");
                cboForgedYoung.Items.Add("7.위용종");
                cboForgedYoung.Items.Add("8.위 점막하종양");
                cboForgedYoung.Items.Add("9.기타");
                cboForgedYoung.SelectedIndex = 0;


                ComboBox cboEndo = (Controls.Find("cboEndo" + i.ToString(), true)[0] as ComboBox);
                cboEndo.Items.Clear();
                cboEndo.Items.Add("");
                cboEndo.Items.Add("01.이상소견없음");
                cboEndo.Items.Add("21.위염");
                cboEndo.Items.Add("22.위축성위염");
                cboEndo.Items.Add("23.장상피화생");
                cboEndo.Items.Add("03.위암의심");
                cboEndo.Items.Add("04.조기위암");
                cboEndo.Items.Add("05.진행위암");
                cboEndo.Items.Add("06.양성위궤양");
                cboEndo.Items.Add("71.위용종");
                cboEndo.Items.Add("72.위선종");
                cboEndo.Items.Add("08.위 점막하종양");
                cboEndo.Items.Add("09.기타");
                cboEndo.SelectedIndex = 0;
            }

            //위조직소견
            cboResult0001.Items.Clear();
            cboResult0001.Items.Add("");
            cboResult0001.Items.Add("01.이상소견없음");
            cboResult0001.Items.Add("21.위염");
            //2020년 변경사항      
            cboResult0001.Items.Add("22.위축성위염");
            cboResult0001.Items.Add("23.장상피화생");
            //----------------------------------
            cboResult0001.Items.Add("03.염증성 또는 증식성 병변");
            cboResult0001.Items.Add("04.저도샘종 또는 이형성");
            cboResult0001.Items.Add("05.고도샘종 또는 이형성");
            cboResult0001.Items.Add("06.암의심");
            cboResult0001.Items.Add("07.암");
            cboResult0001.Items.Add("08.기타");
            cboResult0001.SelectedIndex = 0;

            //위조직개수
            cboStomachTissueCnt.Items.Clear();
            cboStomachTissueCnt.Items.Add("");
            cboStomachTissueCnt.Items.Add("1. [1-3개]");
            cboStomachTissueCnt.Items.Add("2. [4-6개]");
            cboStomachTissueCnt.Items.Add("3. [7-9개]");
            cboStomachTissueCnt.Items.Add("4. [10-12개]");
            cboStomachTissueCnt.Items.Add("5. [13개이상]");

            //위암종합판정
            cboSPan.Items.Clear();
            cboSPan.Items.Add("");
            cboSPan.Items.Add("1.이상소견없음 또는 위염");
            cboSPan.Items.Add("2.양성질환");
            cboSPan.Items.Add("3.위암의심");
            cboSPan.Items.Add("4.위암");
            cboSPan.Items.Add("5.기타()");
            cboSPan.SelectedIndex = 0;

            //폐암종합판정
            cboLPan1.Items.Clear();
            cboLPan1.Items.Add("");
            cboLPan1.Items.Add("1.이상소견없음");
            cboLPan1.Items.Add("2.양성결절");
            cboLPan1.Items.Add("3.경계선 결절");
            cboLPan1.Items.Add("4.폐암의심");
            cboLPan1.SelectedIndex = 0;

            cboLPan2.Items.Clear();
            cboLPan2.Items.Add("");
            cboLPan2.Items.Add("1.4A");
            cboLPan2.Items.Add("2.4B");
            cboLPan2.Items.Add("3.4X");
            cboLPan2.SelectedIndex = 0;
        }

        /// <summary>
        /// 대장암 화면구성
        /// </summary>
        void fn_Control_Set_Colon()
        {
            cboDenotationSubcutaneousBlood.Items.Clear();
            cboDenotationSubcutaneousBlood.Items.Add("");
            cboDenotationSubcutaneousBlood.Items.Add("1.음성");
            cboDenotationSubcutaneousBlood.Items.Add("2.양성");
            cboDenotationSubcutaneousBlood.SelectedIndex = 0;

            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + i.ToString(), true)[0] as ComboBox);
                ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + i.ToString(), true)[0] as ComboBox);

                cboColonizationAassistant.Items.Clear();
                cboColonizationAassistant.Items.Add("");
                cboColonizationAassistant.Items.Add("1.이상소견없음");
                cboColonizationAassistant.Items.Add("2.대장용종");
                cboColonizationAassistant.Items.Add("3.대장암의심");
                cboColonizationAassistant.Items.Add("4.대장암");
                cboColonizationAassistant.Items.Add("5.기타");
                cboColonizationAassistant.SelectedIndex = 0;

                //대장조직소견
                cboColonoScope.Items.Clear();
                cboColonoScope.Items.Add("");
                cboColonoScope.Items.Add("1.이상소견없음");
                cboColonoScope.Items.Add("2.대장용종");
                cboColonoScope.Items.Add("3.대장암의심");
                cboColonoScope.Items.Add("4.대장암");
                cboColonoScope.Items.Add("5.기타");
                cboColonoScope.SelectedIndex = 0;

                cboCPan.Items.Clear();
                cboCPan.Items.Add("");
                //cboCPan.Items.Add("1.이상소견없음");
                //cboCPan.Items.Add("2.대장용종");
                //cboCPan.Items.Add("3.대장암 의심");
                //cboCPan.Items.Add("4.대장암");
                //cboCPan.Items.Add("5.기타");
                cboCPan.Items.Add("6.잠혈반응없음");
                cboCPan.Items.Add("7.잠혈반응있음");
                cboCPan.SelectedIndex = 0;
            }

            //대장조직진단
            cboColonTissue.Items.Add("");
            cboColonTissue.Items.Add("1.이상소견없음");
            cboColonTissue.Items.Add("2.염증성 또는 증식성 병변");
            cboColonTissue.Items.Add("3.저도샘종 또는 이형성");
            cboColonTissue.Items.Add("4.고도샘종 또는 이형성");
            cboColonTissue.Items.Add("5.암의심");
            cboColonTissue.Items.Add("6.암");
            cboColonTissue.Items.Add("7.기타");
            cboColonTissue.SelectedIndex = 0;

            //대장조직개수            
            cboColonTissueCnt.Items.Clear();
            cboColonTissueCnt.Items.Add("");
            cboColonTissueCnt.Items.Add("1. [1-3개]");
            cboColonTissueCnt.Items.Add("2. [4-6개]");
            cboColonTissueCnt.Items.Add("3. [7-9개]");
            cboColonTissueCnt.Items.Add("4. [10-12개]");
            cboColonTissueCnt.Items.Add("5. [13개이상]");
            cboColonTissueCnt.SelectedIndex = 0;
        }

        /// <summary>
        /// 간암 화면구성
        /// </summary>
        void fn_Control_Set_Liver()
        {
            //B형간염항원
            cboNew_B.Items.Clear();
            cboNew_B.Items.Add("");
            cboNew_B.Items.Add("1.음성");
            cboNew_B.Items.Add("2.양성");
            cboNew_B.SelectedIndex = 0;

            //ALT및B형간염항원결과
            cboBRsult.Items.Clear();
            cboBRsult.Items.Add("");
            cboBRsult.Items.Add("1.이상없음");
            cboBRsult.Items.Add("2.이상있음");
            cboBRsult.Items.Add("3.간암고위험간장질환");
            cboBRsult.Items.Add("4.기타");
            cboBRsult.SelectedIndex = 0;

            //C형간염항체
            cboNew_C.Items.Clear();
            cboNew_C.Items.Add("");
            cboNew_C.Items.Add("1.음성");
            cboNew_C.Items.Add("2.양성");
            cboNew_C.SelectedIndex = 0;

            //C형간염항체결과
            cboCResult.Items.Clear();
            cboCResult.Items.Add("");
            cboCResult.Items.Add("1.이상없음");
            cboCResult.Items.Add("3.간암고위험간장질환");
            cboCResult.Items.Add("4.기타");
            cboCResult.SelectedIndex = 0;

            cboLPan.Items.Clear();
            cboLPan.Items.Add("");
            cboLPan.Items.Add("1.간암 의심소견 없음");
            cboLPan.Items.Add("2.추적검사 요망(3개월 이내)");
            cboLPan.Items.Add("3.간암 의심(정밀 검사 요망)");
            cboLPan.Items.Add("4.기타");
            cboLPan.SelectedIndex = 0;

            cboRPHA.Items.Clear();
            cboRPHA.Items.Add(" ");
            cboRPHA.Items.Add("1.음성");
            cboRPHA.Items.Add("2.양성");
            cboRPHA.SelectedIndex = 0;

            cboLPan.Items.Clear();
            cboLPan.Items.Add("");
            cboLPan.Items.Add("1.간암 의심소견 없음");
            cboLPan.Items.Add("2.추적검사 요망(3개월 이내)");
            cboLPan.Items.Add("3.간암 의심(정밀 검사 요망)");
            cboLPan.Items.Add("4.기타");
            cboLPan.SelectedIndex = 0;
        }

        /// <summary>
        /// 유방암 화면구성
        /// </summary>
        void fn_Control_Set_Breast()
        {
            //유방실질분포량
            cboBreast.Items.Clear();
            cboBreast.Items.Add("");
            cboBreast.Items.Add("1. [25%미만]");
            cboBreast.Items.Add("2. [25~50%]");
            cboBreast.Items.Add("3. [51~75%]");
            cboBreast.Items.Add("4. [76~100%]");
            cboBreast.Items.Add("5.유방실질내 인공물질 주입");
            cboBreast.SelectedIndex = 0;

            //유방
            cboResult0013.Items.Clear();
            cboResult0013.Items.Add("");
            cboResult0013.Items.Add("1.양측");
            cboResult0013.Items.Add("2.편측(오른쪽)");
            cboResult0013.Items.Add("3.편측(왼쪽)");
            cboResult0013.SelectedIndex = 0;

            //유방판독소견
            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboBreast = (Controls.Find("cboBreast" + i.ToString(), true)[0] as ComboBox);
                cboBreast.Items.Clear();
                cboBreast.Items.Add("");
                cboBreast.Items.Add("01.이상소견없음");
                cboBreast.Items.Add("02.종괴");
                cboBreast.Items.Add("03.양성석회화");
                cboBreast.Items.Add("04.미세석회화");
                cboBreast.Items.Add("05.구조왜곡");
                cboBreast.Items.Add("06.비대칭");
                cboBreast.Items.Add("07.피부이상");
                cboBreast.Items.Add("08.임파선 비후");
                cboBreast.Items.Add("09.판정곤란");
                cboBreast.Items.Add("10.직접기입");
                cboBreast.SelectedIndex = 0;
            }

            //종합판정(유방)
            cboBPan.Items.Clear();
            cboBPan.Items.Add("");
            cboBPan.Items.Add("1.이상소견없음");
            cboBPan.Items.Add("2.양성질환");
            cboBPan.Items.Add("3.유방암 의심");
            cboBPan.Items.Add("4.판정유보");
            cboBPan.SelectedIndex = 0;
        }

        /// <summary>
        /// 자궁경부암 화면구성
        /// </summary>
        void fn_Control_Set_Woman()
        {
            cboWPan.Items.Clear();
            cboWPan.Items.Add("");
            cboWPan.Items.Add("1.이상소견없음");
            cboWPan.Items.Add("2.반응성 소견 및 감염성질환");
            cboWPan.Items.Add("3.비정형 세포 이상");
            cboWPan.Items.Add("4.자궁경부암 전구단계 의심");
            cboWPan.Items.Add("5.자궁경부암의심");
            cboWPan.Items.Add("6.기타");
            cboWPan.SelectedIndex = 0;

            lblTissueCnt.Visible = false;
            cboStomachTissueCnt.Visible = false;
            lblColonoTissueCnt.Visible = false;
            cboColonTissueCnt.Visible = false;
        }

        void eLabelClick(object sender, EventArgs e)
        {
            if (sender == lblAutoPanInfo)
            {
                //pnlPanHelp.Visible = true;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnText)
            {
                chkResult00107.Checked = true;
                txtResult0012.Text = btnText.Text;
            }
            else if (sender == btnHelpLungExit)
            {
                pnlPanLungHelp.Visible = false;
            }
            else if (sender == btnGubDaesang)
            {
                long nAlt = 0;
                string strOK = "OK";

                if (txtALT.Text.IsNullOrEmpty()) { nAlt = 0; }
                else { nAlt = txtALT.Text.To<long>(); }

                if (nAlt >= 46) { strOK = "NO"; }

                //B형
                if (cboNew_B.SelectedIndex > 0 && !txtALT.Text.IsNullOrEmpty())
                {
                    if (strOK == "OK" && cboNew_B.SelectedIndex == 1)
                    {
                        cboBRsult.SelectedIndex = 1;
                    }
                    else if (strOK == "NO" && cboNew_B.SelectedIndex == 1)
                    {
                        cboBRsult.SelectedIndex = 2;
                    }
                    else
                    {
                        cboBRsult.SelectedIndex = 3;
                    }
                }

                //C형
                if (cboNew_C.SelectedIndex > 0)
                {
                    if (cboNew_C.SelectedIndex == 1)
                    {
                        cboCResult.SelectedIndex = 1;
                    }
                    else
                    {
                        cboCResult.SelectedIndex = 3;
                    }
                }
            }
            else if (sender == btnF1 || sender == btnF2 || sender == btnF3 || sender == btnF4 || sender == btnF5 || sender == btnF6 || sender == btnF7)
            {
                if (superTabControl1.SelectedTab == tab1)   //위암
                {
                    if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                    {
                        chkEndo10.Checked = false;
                        chkEndo11.Checked = false;
                        chkEndo12.Checked = false;
                        chkEndo13.Checked = false;
                        chkEndo14.Checked = false;
                        chkEndo15.Checked = false;
                        chkEndo16.Checked = false;
                        chkEndo17.Checked = false;

                        chkEndo20.Checked = false;
                        chkEndo21.Checked = false;
                        chkEndo22.Checked = false;
                        chkEndo23.Checked = false;
                        chkEndo24.Checked = false;
                        chkEndo25.Checked = false;
                        chkEndo26.Checked = false;
                        chkEndo27.Checked = false;

                        cboEndo2.SelectedIndex = 0;
                        chkEndo30.Checked = false;
                        chkEndo31.Checked = false;
                        chkEndo32.Checked = false;
                        chkEndo33.Checked = false;
                        chkEndo34.Checked = false;
                        chkEndo35.Checked = false;
                        chkEndo36.Checked = false;
                        chkEndo37.Checked = false;

                        cboResult0001.SelectedIndex = 0;
                        cboStomachTissueCnt.SelectedIndex = 0;

                        chkEndoEtc0.Checked = false;
                        chkEndoEtc1.Checked = false;
                        chkEndoEtc2.Checked = false;
                        chkEndoEtc3.Checked = false;
                        chkEndoEtc4.Checked = false;
                        chkEndoEtc5.Checked = false;
                        chkEndoEtc6.Checked = false;
                        chkEndoEtc7.Checked = false;
                    }
                    else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                    {
                        chkEndo10.Checked = false;
                        chkEndo11.Checked = false;
                        chkEndo12.Checked = false;
                        chkEndo13.Checked = false;
                        chkEndo14.Checked = false;
                        chkEndo15.Checked = false;
                        chkEndo16.Checked = false;
                        chkEndo17.Checked = false;

                        chkEndo20.Checked = false;
                        chkEndo21.Checked = false;
                        chkEndo22.Checked = false;
                        chkEndo23.Checked = false;
                        chkEndo24.Checked = false;
                        chkEndo25.Checked = false;
                        chkEndo26.Checked = false;
                        chkEndo27.Checked = false;

                        cboEndo2.SelectedIndex = 0;
                        chkEndo30.Checked = false;
                        chkEndo31.Checked = false;
                        chkEndo32.Checked = false;
                        chkEndo33.Checked = false;
                        chkEndo34.Checked = false;
                        chkEndo35.Checked = false;
                        chkEndo36.Checked = false;
                        chkEndo37.Checked = false;

                        cboResult0001.SelectedIndex = 0;
                        cboStomachTissueCnt.SelectedIndex = 0;

                        chkEndoEtc0.Checked = false;
                        chkEndoEtc1.Checked = false;
                        chkEndoEtc2.Checked = false;
                        chkEndoEtc3.Checked = false;
                        chkEndoEtc4.Checked = false;
                        chkEndoEtc5.Checked = false;
                        chkEndoEtc6.Checked = false;
                        chkEndoEtc7.Checked = false;
                    }

                    if (sender == btnF1)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                        {
                            cboForgedYoung0.SelectedIndex = 1;  //1.이상소견없음
                            cboSPan.SelectedIndex = 1;          //1.이상소견없음
                            chkForgedYoung10.Checked = false;
                            chkForgedYoung11.Checked = false;
                            chkForgedYoung12.Checked = false;
                            chkForgedYoung13.Checked = false;
                            chkForgedYoung14.Checked = false;
                            chkForgedYoung15.Checked = false;
                            chkForgedYoung16.Checked = false;
                            chkForgedYoung17.Checked = false;
                            chkForgedYoungEtc7.Checked = false;
                            txtForgedYoungEtc.Text = "";
                            txtS_Sogen.Text = "위장조영검사 결과 이상 소견이 없습니다. 속쓰림, 소화불량 등의 증상이 없으시면 1-2년 후 정기검사 받으시기 바랍니다. ";
                            txtS_Sogen.Text += "다만 위장조영검사는 다소 불완전한 검사이므로, ";
                            txtS_Sogen.Text += "위 십이지장 등에 대한 확실한 검사를 위해서는 내시경 검사를 받으시는 게 더 좋습니다.";
                            txtJilEtcS.Text = "";
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 2;
                            cboEndo1.SelectedIndex = 12;
                            chkEndoEtc1.Checked = true; //2.식도염
                            chkEndoEtc7.Checked = false; //직접기입
                            txtEndoEtc.Text = "";
                            cboResult0001.SelectedIndex = 0;
                            cboStomachTissueCnt.SelectedIndex = 0;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissueEtc7.Checked = false;
                            txtStomachTissueEtc.Text = "";
                            txtS_Sogen2.Text = "위내시경검사에서 위염,역류성식도염이 관찰됩니다. 위염은 여러 가지 원인에 의해 위점막에 염증이 발생한 것으로 속쓰림, 소화 불량 등 증상이 있으시면 진료를 받으십시오. 역류성식도염은 위속 물질이 식도로 역류해 식도점막에 염증을 일으킨 것으로 흡연,음주,커피, 야식 등이 원인이 되므로 주의하시기 바랍니다";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF2)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)        //위장조영검사
                        {
                            cboForgedYoung0.SelectedIndex = 9;  //9.기타
                            cboSPan.SelectedIndex = 5;          //5.기타
                            chkForgedYoung10.Checked = false;
                            chkForgedYoung11.Checked = false;
                            chkForgedYoung12.Checked = false;
                            chkForgedYoung13.Checked = false;
                            chkForgedYoung14.Checked = false;
                            chkForgedYoung15.Checked = false;
                            chkForgedYoung16.Checked = false;
                            chkForgedYoung17.Checked = false;
                            chkForgedYoungEtc7.Checked = true;
                            txtForgedYoungEtc.Text = "위하수";
                            txtS_Sogen.Text = "위장조영검사 결과 위하수가 있으며 그 외 이상소견은 없습니다. ";
                            txtS_Sogen.Text += "속쓰림, 소화불량 등의 증상이 없으시면 1-2년 후 정기검사 받으시기 바랍니다.";
                            txtJilEtcS.Text = "위하수";
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 12;
                            chkEndoEtc1.Checked = true; //2.식도염.
                            chkEndoEtc7.Checked = false; //2.직접기입
                            txtStomachTissueEtc.Text = "";
                            cboResult0001.SelectedIndex = 0;
                            cboStomachTissueCnt.SelectedIndex = 0;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Checked = false;
                            txtStomachTissueEtc.Text = "";
                            txtS_Sogen2.Text = "위내시경검사에서 만성위축성위염,역류성식도염이 관찰됩니다. 위축성위염은 만성적인 염증과 노화에 의하여 위점막이 얇아진 상태입니다. 역류성 식도염은 위속물질이 식도로 역류해 식도 점막에 염증을 일으킨 것으로 흡연,음주,커피,야식 등이 원인이므로 주의하시기 바랍니다. 1년 후 반드시 내시경검사를 받으시기 바랍니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF3)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                        {
                            cboForgedYoung0.SelectedIndex = 9;  //9.기타
                            cboSPan.SelectedIndex = 5;          //5.기타
                            chkForgedYoung10.Checked = false;
                            chkForgedYoung11.Checked = false;
                            chkForgedYoung12.Checked = false;
                            chkForgedYoung13.Checked = false;
                            chkForgedYoung14.Checked = false;
                            chkForgedYoung15.Checked = false;
                            chkForgedYoung16.Checked = false;
                            chkForgedYoung17.Checked = false;
                            chkForgedYoungEtc7.Checked = true;
                            txtForgedYoungEtc.Text = "위식도 역류";
                            txtS_Sogen.Text = "위장조영검사에서 위식도 역류가 관찰됩니다. ";
                            txtS_Sogen.Text += "위식도 역류란 위산과 같은 위속 물질이 식도로 역류하는 것으로 식도 점막에 반복적인 염증을 일으킬 수 있습니다. ";
                            txtS_Sogen.Text += "신물 올라옴, 속 쓰림, 소화불량 등의 증상이 있으면 진료를 받으시기 바랍니다.";
                            txtJilEtcS.Text = "위식도 역류";
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 3;
                            chkEndoEtc1.Checked = false; //2.식도염.
                            cboResult0001.SelectedIndex = 0;
                            cboStomachTissueCnt.SelectedIndex = 0;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Checked = false;

                            cboEndo1.SelectedIndex = 4;
                            chkEndo21.Checked = true;
                            chkEndo22.Checked = true;
                            chkEndo24.Checked = true;
                            txtStomachTissueEtc.Text = "";
                            txtS_Sogen2.Text = "위내시경검사에서 만성위축성위염, 장상피화생이 관찰됩니다. 위축성위염은 만성적인 염증과 노화에 의하여 위점막이 얇아진 상태이고, 장상피화생은 위점막이 울퉁불퉁 장점막화된 것으로 , 이런경우 위암발병율이 증가할 수 있으므로 향후 1년마다 정기적인 내시경 검사를 권합니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF4)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                        {
                            cboForgedYoung0.SelectedIndex = 2;  //9.기타
                            cboSPan.SelectedIndex = 2;          //5.기타
                            chkForgedYoung10.Checked = false;
                            chkForgedYoung11.Checked = false;
                            chkForgedYoung12.Checked = true;
                            chkForgedYoung13.Checked = false;
                            chkForgedYoung14.Checked = true;
                            chkForgedYoung15.Checked = false;
                            chkForgedYoung16.Checked = false;
                            chkForgedYoung17.Checked = false;
                            chkForgedYoungEtc7.Checked = true;
                            txtForgedYoungEtc.Text = "위식도 역류";
                            txtS_Sogen.Text = "위장조영검사 결과 위염이 의심됩니다. 위염과 관련해 속쓰림, ";
                            txtS_Sogen.Text += "소화 불량 등 증상이 있으시면 진료를 받으시고, 보다 정확한 진단을 위해서는 ";
                            txtS_Sogen.Text += "위내시경 검사를 받으시는 게 좋습니다.";
                            txtJilEtcS.Text = "";
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 4;
                            chkEndo21.Checked = true;
                            chkEndo22.Checked = true;
                            chkEndo24.Checked = true;
                            cboEndo2.SelectedIndex = 12;

                            chkEndoEtc1.Checked = true; //2.식도염.
                            cboResult0001.SelectedIndex = 0;
                            cboStomachTissueCnt.SelectedIndex = 0;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Checked = false;
                            txtStomachTissueEtc.Text = "";
                            txtS_Sogen2.Text = "위내시경검사에서 만성위축성위염,역류성식도염,장상피화생이 관찰됩니다. 위축성위염은 만성적인 염증과 노화에 의하여 위점막이 얇아진 상태이고, 장상피화생은 위점막이 울퉁불퉁 장점막화된 것으로 , 이런경우 위암발병율이 증가할 수 있으므로 향후 정기적인 내시경 검사를 권합니다. 과식,야식,음주, 카페인 등은 역류성 식도염을 악화시키므로 주의하시기 바랍니다. 1년 후 반드시 내시경검사를 받으시기 바랍니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF5)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                        {
                            rdoStomachTissue0.Checked = true;
                            //lblDrName3.Text = "김미진";
                            //SSDR.ActiveSheet.Cells[2, 0].Text = "30846";

                            eRdoClick(rdoStomachTissue0, new EventArgs());

                            clsHcVariable.GstrHicDrName = "";
                            clsHcVariable.GnHicLicense1 = 0;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 0;
                            chkEndoEtc1.Checked = false;    //2.식도염
                            chkEndoEtc7.Checked = false;    //직접기입
                            txtEndoEtc.Text = "";
                            cboResult0001.SelectedIndex = 0;
                            cboStomachTissueCnt.SelectedIndex = 0;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Text = "";
                            txtStomachTissueEtc.Text = "위염,장상피화생,헬리코박터균";
                            txtS_Sogen2.Text = "위내시경검사에서 만성위축성위염이 관찰됩니다. 조직검사에서 장상피화생을 동반한 위염과 헬리코박터균이 확인되었습니다. 위축성위염이란 만성적인 염증으로 인해 위점막이 얇아진 것이고,장상피화생은 위염이 진행되어 위점막이 울퉁불퉁하게 장점막화되는 상태로, 이런 경우 위암발생 가능성이 증가할 수 있으므로 향후 정기적인 위내시경검사를 권합니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 0;
                            chkEndoEtc1.Checked = false; //2.식도염.
                            chkEndoEtc7.Checked = false; //2.직접기입
                            txtStomachTissueEtc.Text = "";
                            cboResult0001.SelectedIndex = 10;
                            cboStomachTissueCnt.SelectedIndex = 1;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Checked = false;
                            txtStomachTissueEtc.Text = "위염,장상피화생,헬리코박터균";
                            txtS_Sogen2.Text = "위내시경검사에서 만성위축성위염이 관찰됩니다. 조직검사에서 장상피화생을 동반한 위염과 헬리코박터균이 확인되었습니다. 위축성위염이란 만성적인 염증으로 인해 위점막이 얇아진 것이고,장상피화생은 위염이 진행되어 위점막이 울퉁불퉁하게 장점막화되는 상태로, 이런 경우 위암발생 가능성이 증가할 수 있으므로 1년 후 반드시 내시경검사를 받으시기 바랍니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF6)
                    {
                        if (TabStomach.SelectedTab == tabStomach1)    //위장조영검사
                        {
                            rdoStomachTissue0.Checked = true;
                            //lblDrName3.Text = "김미진";
                            //SSDR.ActiveSheet.Cells[2, 0].Text = "30846";

                            eRdoClick(rdoStomachTissue0, new EventArgs());

                            clsHcVariable.GstrHicDrName = "";
                            clsHcVariable.GnHicLicense1 = 0;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 12;
                            chkEndoEtc1.Checked = true; //2.식도염
                            cboResult0001.SelectedIndex = 10;
                            cboStomachTissueCnt.SelectedIndex = 1;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Text = "";
                            txtStomachTissueEtc.Text = "위염,장상피화생,헬리코박터균";
                            txtS_Sogen2.Text = "위내시경검사에서 위축성위염,역류성식도염이 있고, 조직검사에서 장상피화생을 동반한 위염과 헬리코박터균이 확인되었습니다.위축성위염이란 만성적인 염증으로 인해 위점막이 얇아진 것을 말하고, 장상피화생은 위염이 진행되어 위점막이 울퉁불퉁하게 장점막화되는 상태로, 이런 경우 위암발생 가능성이 증가할 수 있으므로 향후 정기적인 위내시경검사를 권합니다. 과식,야식,음주,카페인 등은 역류성식도염을 악화시키므로 삼가하시기 바랍니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                        else if (TabStomach.SelectedTab == tabStomach2)   //위내시경 검사/조직진단
                        {
                            rdoStomachTissue1.Checked = true;
                            cboEndo0.SelectedIndex = 3;
                            cboEndo1.SelectedIndex = 12;
                            chkEndoEtc1.Checked = true; //2.식도염.
                            txtStomachTissueEtc.Text = "";
                            cboResult0001.SelectedIndex = 10;
                            cboStomachTissueCnt.SelectedIndex = 1;
                            chkEndo11.Checked = true;
                            chkEndo12.Checked = true;
                            chkEndo14.Checked = true;
                            chkStomachTissue8.Checked = false;
                            txtStomachTissueEtc.Text = "위염,장상피화생,헬리코박터균";
                            //txtS_Sogen2.Text = "위내시경검사에서 위축성위염,역류성식도염이 있고, 조직검사에서 장상피화생을 동반한 위염과 헬리코박터균이 확인되었습니다.위축성위염이란 만성적인 염증으로 인해 위점막이 얇아진 것을 말하고, 장상피화생은 위염이 진행되어 위점막이 울퉁불퉁하게 장점막화되는 상태로, 이런 경우 위암발생 가능성이 증가할 수 있으므로 향후 정기적인 위내시경검사를 권합니다. 과식,야식,음주,카페인 등은 역류성식도염을 악화시키므로 삼가하시기 바랍니다.";
                            txtS_Sogen2.Text = "위내시경검사에서 위축성위염,역류성식도염이 있고, 조직검사에서 장상피화생을 동반한 위염과 헬리코박터균이 확인되었습니다.위축성위염이란 만성적인 염증으로 인해 위점막이 얇아진 것을 말하고, 장상피화생은 위염이 진행되어 위점막이 울퉁불퉁하게 장점막화되는 상태로, 이런 경우 위암발생 가능성이 증가할 수 있으므로 1년 뒤 추적 위내시경검사를 권합니다. 과식,야식,음주,카페인 등은 역류성식도염을 악화시키므로 삼가하시기 바랍니다.";
                            cboSPan.SelectedIndex = 2;
                        }
                    }
                    else if (sender == btnF7)
                    {
                        for (int i = 0; i <= 7; i++)
                        {
                            CheckBox chkForgedYoung1 = (Controls.Find("chkForgedYoung1" + i.ToString(), true)[0] as CheckBox);
                            CheckBox chkForgedYoung2 = (Controls.Find("chkForgedYoung2" + i.ToString(), true)[0] as CheckBox);
                            CheckBox chkForgedYoung3 = (Controls.Find("chkForgedYoung3" + i.ToString(), true)[0] as CheckBox);
                            CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + i.ToString(), true)[0] as CheckBox);

                            chkForgedYoung1.Checked = false;
                            chkForgedYoung2.Checked = false;
                            chkForgedYoung3.Checked = false;
                            chkEndoEtc.Checked = false;
                        }

                        cboResult0001.SelectedIndex = 0;        //조직진단
                        cboStomachTissueCnt.SelectedIndex = 0;  //위조직개수

                        txtS_Sogen2.Text = "";
                        cboEndo0.SelectedIndex = 0;
                        cboEndo1.SelectedIndex = 0;
                        cboEndo2.SelectedIndex = 0;
                        //cboSPan.SelectedIndex = 0;

                        cboEndo0.SelectedIndex = 3;
                        chkEndo11.Checked = true;
                        chkEndo12.Checked = true;
                        chkEndo14.Checked = true;
                        cboSPan.SelectedIndex = 1;

                        txtS_Sogen2.Text = " 위내시경검사에서 위축성위염이 있습니다. 위축성위염은 만성적인 염증,노화에 의해 위점막이 얇아진 상태입니다. 소화 불량 등 증상이 있으시면 진료를 받으시고, 1년 후 정기 위 내시경검사를 권합니다.";

                    }
                }
                else if (superTabControl1.SelectedTab == tab2)   //대장암
                {

                }
                else if (superTabControl1.SelectedTab == tab3)   //간암
                {
                    cboStomachTissueCnt.SelectedIndex = 0;
                    chkResult00040.Checked = false;
                    chkResult00041.Checked = false;
                    chkResult00042.Checked = false;
                    chkResult00043.Checked = false;

                    chkResult00060.Checked = false;
                    chkResult00061.Checked = false;
                    chkResult00062.Checked = false;
                    chkResult00063.Checked = false;
                    chkResult00064.Checked = false;
                    chkResult00065.Checked = false;
                    chkResult00066.Checked = false;
                    chkResult00067.Checked = false;

                    chkResult00050.Checked = false;
                    chkResult00051.Checked = false;
                    chkResult00052.Checked = false;
                    chkResult00053.Checked = false;
                    chkResult00054.Checked = false;
                    chkResult00055.Checked = false;
                    chkResult00056.Checked = false;
                    chkResult00057.Checked = false;

                    chkResult00100.Checked = false;
                    chkResult00101.Checked = false;
                    chkResult00102.Checked = false;
                    chkResult00103.Checked = false;
                    chkResult00104.Checked = false;
                    chkResult00105.Checked = false;
                    chkResult00106.Checked = false;
                    chkResult00107.Checked = false;
                    chkResult00108.Checked = false;

                    chkResult00150.Checked = false;
                    chkResult00151.Checked = false;

                    txtResult0007.Text = "";
                    txtResult0008.Text = "";
                    txtResult0009.Text = "";
                    txtResult0012.Text = "";

                    if (sender == btnF1)
                    {
                        chkResult00040.Checked = true;
                        cboLPan.SelectedIndex = 1;
                        txtL_Sogen.Text = "간초음파 검사결과 및 간암 수치가 정상입니다. 향후 정기적인 간암검진을 받으시기 바랍니다.";
                        txtJilEtcL.Text = "";
                    }
                    else if (sender == btnF2)
                    {
                        chkResult00041.Checked = true;
                        cboLPan.SelectedIndex = 4;
                        txtL_Sogen.Text = "간초음파 검사상 지방간이 있으며 간암수치는 정상입니다. 지방간은 간에 지방이 침착되어 발생하는 것으로 음주와 비만 등이 주요 원인입니다. 적절한 운동과 체중 조절 및 절주가 필요합니다. 정기적인 간암검진을 받으시기 바랍니다.";
                        txtJilEtcL.Text = "지방간";
                    }
                    else if (sender == btnF3)
                    {
                        chkResult00042.Checked = true;
                        cboLPan.SelectedIndex = 4;
                        txtL_Sogen.Text = "간초음파 검사상 간이 거칠게 보이며, 간암수치는 정상입니다. 만성간질환에서 보일 수 있는 소견으로, 향후 정기적인 간암 검진을 받으시기 바랍니다.";
                        txtJilEtcL.Text = "거친에코상";
                    }
                    else if (sender == btnF4)
                    {
                        chkResult00043.Checked = true;
                        cboLPan.SelectedIndex = 4;
                        txtL_Sogen.Text = "간초음파 검사상 간경변증의 소견이 있으며, 간암 수치는 정상입니다. 간경변증이란 만성 염증에 의해 간이 점점 작아지고 단단해지는 상태를 말합니다. 향후 이에 대한 치료 및 경과 관찰이 필요하오니, 의료기관을 방문하시어 진료를 받으시기 바랍니다.";
                        txtJilEtcL.Text = "간경변";
                    }
                }
                else if (superTabControl1.SelectedTab == tab4)   //유방암
                {
                    if (sender == btnF1)
                    {
                        cboBreast.SelectedIndex = 1;
                        cboBreast0.SelectedIndex = 1;
                        txtBreastReadOpinionEtc.Text = "";
                        cboBPan.SelectedIndex = 1;
                        txtB_Sogen.Text = "유방촬영 검사상 이상소견이 발견되지 않습니다. 1-2년 후 정기검진을 받으시길 바랍니다. 하지만 만져지는 병변이 있거나 유두의 습진, 혈성 분비물과 같은 임상증상이 있는 경우 의료기관을 방문하시어 추가검사 여부에 대한 진료상담 바랍니다.";

                    }
                    else if (sender == btnF2)
                    {
                        cboBreast.SelectedIndex = 3;
                        cboBreast0.SelectedIndex = 1;
                        txtBreastReadOpinionEtc.Text = "";
                        cboBPan.SelectedIndex = 1;
                        txtB_Sogen.Text = "유방촬영 검사상 치밀 유방 소견을 보입니다. 하지만 치밀유방의 경우 병변의 발견이 어려울 수 있으므로 추가적으로 초음파검사를 권유합니다.  만져지는 병변이 있거나 유두의 습진, 혈성 분비물과 같은 임상증상이 있는 경우 의료기관을 방문하시어  진료상담하십시요. 정기적 검진이 필요합니다(1년)";
                    }
                    else if (sender == btnF3)
                    {
                        cboBreast.SelectedIndex = 4;
                        cboBreast0.SelectedIndex = 1;
                        txtBreastReadOpinionEtc.Text = "";
                        cboBPan.SelectedIndex = 1;
                        txtB_Sogen.Text = "유방촬영 검사상 치밀 유방 소견을 보입니다. 하지만 치밀유방의 경우 병변의 발견이 어려울 수 있으므로 추가적으로 초음파검사를 권유합니다.  만져지는 병변이 있거나 유두의 습진, 혈성 분비물과 같은 임상증상이 있는 경우 의료기관을 방문하시어  진료상담하십시요. 정기적 검진이 필요합니다(1년)";
                    }
                    else if (sender == btnF4)
                    {
                        cboBreast.SelectedIndex = 4;
                        cboBreast0.SelectedIndex = 9;
                        txtBreastReadOpinionEtc.Text = "";
                        cboBPan.SelectedIndex = 4;
                        txtB_Sogen.Text = "유방촬영 검사상 치밀 유방 소견을 보입니다. 하지만 치밀유방의 경우 병변의 발견이 어려울 수 있으므로 추가적으로 초음파검사를 권유합니다.  만져지는 병변이 있거나 유두의 습진, 혈성 분비물과 같은 임상증상이 있는 경우 의료기관을 방문하시어  진료상담하십시요. 정기적 검진이 필요합니다(1년)";
                    }
                }
                else if (superTabControl1.SelectedTab == tab5)   //자궁경부암
                {
                    if (sender == btnF1)
                    {
                        rdoWomBo11.Checked = true;
                        rdoWomBo21.Checked = true;
                        rdoWomBo31.Checked = true;
                        chkWomBo4.Checked = false;
                        rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                        chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                        chkWomBo6.Checked = false;
                        rdoWomBo71.Checked = true;
                        cboWPan.SelectedIndex = 2;
                        //txtW_Sogen.Text = "자궁경부세포검사상 반응성세포 변화가 있습니다. 반응성세포 변화란 여러 가지 자극에 의한 세포 모양의 변화가 생긴 것입니다. 질 분비물의 증가와 가려움 등의 증상이 동반되었을 경우는 의료기관을 방문하여 진료를 받으시기 바랍니다. 특별한 증상이 동반되지 않은 경우 정기검진 받으시길 바랍니다.";
                        txtW_Sogen.Text = "자궁경부세포검사상 반응성세포 변화가 있습니다. 반응성세포 변화란 여러 가지 자극에 의한 세포 모양의 변화가 생긴 것입니다.검사상 염증소견이 있으니, 산부인과 진료 및 치료 후에 재검 받으시기 바랍니다."; 
                        txtWombo3_Etc.Text = "";
                    }
                    else if (sender == btnF2)
                    {
                        rdoWomBo11.Checked = true;
                        rdoWomBo21.Checked = true;
                        rdoWomBo31.Checked = true;
                        chkWomBo4.Checked = false;
                        rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                        chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                        chkWomBo6.Checked = false;
                        rdoWomBo70.Checked = true;
                        cboWPan.SelectedIndex = 1;
                        txtW_Sogen.Text = "자궁경부 세포검사상 이상이 없습니다. 이전 이상소견이 있었던 경우가 아니면 1년후 정기검진 받으시길 바랍니다.";
                        txtWombo3_Etc.Text = "";
                    }
                    else if (sender == btnF3)
                    {
                        rdoWomBo11.Checked = true;
                        rdoWomBo21.Checked = true;
                        rdoWomBo32.Checked = true;
                        chkWomBo4.Checked = true;
                        rdoWomBo41.Checked = true; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                        chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                        chkWomBo6.Checked = false;
                        rdoWomBo70.Checked = true;
                        cboWPan.SelectedIndex = 3;
                        txtW_Sogen.Text = "자궁경부세포검사에서 암세포는 아니지만 이상 세포가 발견되었습니다. 추적 관찰 혹은 추가 검사를 통해서만 정확히 암의 위험성을 평가할 수 있는 상태이므로 빠른 시일 내에 의료기관을 방문하시어 진료상담 및 추가 검사를 받으시기 바랍니다. (3개월 내 재검)";
                        txtWombo3_Etc.Text = "";
                    }
                    else if (sender == btnF4)
                    {
                        rdoWomBo11.Checked = true;
                        rdoWomBo21.Checked = true;
                        rdoWomBo31.Checked = true;
                        chkWomBo4.Checked = false;
                        rdoWomBo41.Checked = false; rdoWomBo42.Checked = false; rdoWomBo43.Checked = false; rdoWomBo44.Checked = false;
                        chkWomBo51.Checked = false; chkWomBo52.Checked = false;
                        chkWomBo6.Checked = false;
                        rdoWomBo71.Checked = true;
                        cboWPan.SelectedIndex = 2;
                        txtW_Sogen.Text = "자궁경부세포검사상 반응성세포 변화가 있습니다. 반응성세포 변화란 여러 가지 자극에 의한 세포 모양의 변화가 생긴 것입니다. 질 분비물의 증가와 가려움 등의 증상이 동반되었을 경우는 의료기관을 방문하여 진료를 받으시기 바랍니다. 특별한 증상이 동반되지 않은 경우 정기검진 받으시길 바랍니다.";
                        txtWombo3_Etc.Text = "";
                    }
                }
                else if (superTabControl1.SelectedTab == tab6)   //폐암
                {

                }
            }
            else if (sender == btnCommon0) { fn_Sogen_Common_Disp(txtS_Sogen, "11", "1"); }
            else if (sender == btnCommon1) { fn_Sogen_Common_Disp(txtS_Sogen2, "11", "2"); }
            else if (sender == btnCommon2) { fn_Sogen_Common_Disp(txtC_Sogen, "21", "1"); }
            else if (sender == btnCommon3) { fn_Sogen_Common_Disp(txtC_Sogen2, "21", "2"); }
            else if (sender == btnCommon4) { fn_Sogen_Common_Disp(txtC_Sogen3, "21", "3"); }
            else if (sender == btnCommon5) { fn_Sogen_Common_Disp(txtL_Sogen, "31", "1"); }
            else if (sender == btnCommon6) { fn_Sogen_Common_Disp(txtB_Sogen, "41", "1"); }
            else if (sender == btnCommon7) { fn_Sogen_Common_Disp(txtW_Sogen, "51", "1"); }
            else if (sender == btnCommon8)
            {
                fn_Sogen_Common_Disp(txtL_Sogen1, "61", "1");
            }
            else if (sender == btnSave)
            {
                #region Variable Define
                string[] strPanDrNo = new string[6];
                string[] strNewSogen = new string[10];
                string str위조영소견 = "";
                string str위내시경소견 = "";
                string strGbEndo = "";
                string strGbAnat = "";
                string str위조영기타 = "";
                string str위내시경기타 = "";
                string strPositS1 = "";
                string strPositS2 = "";
                string strJinS2 = "";
                string strJinSB1 = "";
                string strJinSB2 = "";
                string str분변잠혈 = "";
                string str대장조영 = "";
                string str대장내시경 = "";
                string strByungC1 = "";
                string strByungC2 = "";
                string strGbCAnat = "";
                string strPositC1 = "";
                string strPositC2 = "";
                string strSizeC1 = "";
                string strSizeC2 = "";
                string strSizeCut = "";
                string strJinC1 = "";
                string strJinC2 = "";
                string strJinCB1 = "";
                string strJinCB2 = "";
                string str간암B형항원 = "";
                string str간암B형결과 = "";
                string str간암C형항체 = "";
                string str간암C형결과 = "";
                string strRPHA = "";
                string str유방분포량 = "";
                string strBSogen = "";
                string strPositB = "";
                string[] strWomb = new string[11];
                string str위암판정 = "";
                string str대장암판정 = "";
                string str간암판정 = "";
                string str유방암판정 = "";
                string str자궁암판정 = "";
                string str폐암판정 = "";
                string strSangDam = "";
                string strROWID = "";
                string strTemp = "";
                string strPanjengDrNo = "";
                string strPanDate = "";
                string strOK = "";
                string strOK1 = "";
                string strOK2 = "";
                string str청구제외 = "";
                string str중복자궁 = "";
                string strJepDate = "";

                string strResult0001 = "";   //조직진단
                string strResult0002 = "";   //장삽입여부
                string strResult0003 = "";   //정결도
                string strResult0004 = "";   //사소견
                string strResult0005 = "";   //암1cm미만 종괴 병변위치
                string strResult0006 = "";   //암1cm이상 종괴 병변위치
                string strResult0010 = "";   //변크기1
                string strResult0011 = "";   //변크기2
                string strResult0012 = "";   //변크기3
                string strResult0013 = "";   //방촬영검사(1.양측 2.편측(우) 3.편측(좌)
                string strResult0014 = "";   //궁-비정상 선상피세표 (1.일반 2.종양성)
                string strResult0015 = "";   //낭종
                string strResult0016 = "";   //이물제거술


                string strPANJENGDRNO1 = "";     //암-판독의사
                string strPANJENGDRNO2 = "";     //암-검사의사
                string strPANJENGDRNO3 = "";     //암-병리진단의사
                string strPANJENGDRNO4 = "";     //장암-판독의사
                string strPANJENGDRNO5 = "";     //장암-검사의사
                string strPANJENGDRNO6 = "";     //장암=병리진단의사
                string strPANJENGDRNO7 = "";     //암-검사의사
                string strPANJENGDRNO8 = "";     //방암-검사의사
                string strPANJENGDRNO9 = "";     //자궁경부암-검사의사
                string strPANJENGDRNO10 = "";    //궁경부암-병리진단의사
                string strPANJENGDRNO11 = "";    //폐암-판독의사

                //폐암추가
                string strLUNG002 = "";
                string strLUNG003 = "";
                string strLUNG004 = "";
                string strLUNG005 = "";
                string strLUNG006 = "";
                string strLUNG007 = "";
                string strLUNG008 = "";
                string strLUNG009 = "";
                string strLUNG010 = "";
                string strLUNG011 = "";
                string strLUNG012 = "";
                string strLUNG013 = "";
                string strLUNG014 = "";
                string strLUNG015 = "";
                string strLUNG016 = "";
                string strLUNG017 = "";
                string strLUNG018 = "";
                string strLUNG019 = "";
                string strLUNG020 = "";
                string strLUNG021 = "";
                string strLUNG022 = "";
                string strLUNG023 = "";
                string strLUNG024 = "";
                string strLUNG025 = "";
                string strLUNG026 = "";
                string strLUNG027 = "";
                string strLUNG028 = "";
                string strLUNG029 = "";
                string strLUNG030 = "";
                string strLUNG031 = "";
                string strLUNG032 = "";
                string strLUNG033 = "";
                string strLUNG034 = "";
                string strLUNG035 = "";
                string strLUNG036 = "";
                string strLUNG037 = "";
                string strLUNG038 = "";
                string strLUNG039 = "";
                string strLUNG040 = "";
                string strLUNG041 = "";
                string strLUNG042 = "";
                string strLUNG043 = "";
                string strLUNG044 = "";
                string strLUNG045 = "";
                string strLUNG046 = "";
                string strLUNG047 = "";
                string strLUNG048 = "";
                string strLUNG049 = "";
                string strLUNG050 = "";
                string strLUNG051 = "";
                string strLUNG052 = "";
                string strLUNG053 = "";
                string strLUNG054 = "";
                string strLUNG055 = "";
                string strLUNG056 = "";
                string strLUNG057 = "";
                string strLUNG058 = "";
                string strLUNG059 = "";
                string strLUNG060 = "";
                string strLUNG061 = "";
                string strLUNG062 = "";
                string strLUNG063 = "";
                string strLUNG064 = "";
                string strLUNG065 = "";
                string strLUNG066 = "";
                string strLUNG067 = "";
                string strLUNG068 = "";
                string strLUNG069 = "";
                string strLUNG070 = "";
                string strLUNG071 = "";
                string strLUNG072 = "";
                string strLUNG073 = "";
                string strLUNG074 = "";
                string strLUNG075 = "";
                string strLUNG076 = "";
                string strLUNG077 = "";
                string strLUNG078 = "";
                string strLUNG079 = "";
                string strLUNG080 = "";
                string strLUNGOK = "";

                string sGbCancer1 = "";  //암종류(위)
                string sGbCancer2 = "";  //암종류(대장)
                string sGbCancer3 = "";  //암종류(간)
                string sGbCancer4 = "";  //암종류(유방)
                string sGbCancer5 = "";  //암종류(자궁경부)
                string sGbCancer6 = "";  //암종류(폐)

                string sStomach1 = "";  //위장조영 Visible 여부
                string sStomach2 = "";  //위내시경 Visible 여부

                string sColon1 = "";    //분변잠혈 Visible 여부
                string sColon2 = "";    //결장이중조영 Visible 여부
                string sColon3 = "";    //대장내시경 Visible 여부

                string sLiver1 = "";    //초음파검사 Enabled 여부
                string sLiver2 = "";    //혈청알파태아단백검사 Enabled 여부
                string sLiver3 = "";    //고위험군선별검사 Enabled 여부

                string sLung1 = "";     //폐결절소견 1 Visible 여부
                string sLung2 = "";     //폐결절소견 2 Visible 여부
                string sLung3 = "";     //폐결절소견 3 Visible 여부
                string sLung4 = "";     //폐결절소견 4 Visible 여부
                string sLung5 = "";     //폐결절소견 5 Visible 여부
                string sLung6 = "";     //폐결절소견 6 Visible 여부

                string sPanjengDate1 = "";   //위암       판정일자, 판정의사 1
                string sPanjengDate2 = "";   //대장암     판정일자, 판정의사 2
                string sPanjengDate3 = "";   //간암       판정일자, 판정의사 3
                string sPanjengDate4 = "";   //유방암     판정일자, 판정의사 4
                string sPanjengDate5 = "";   //자궁경부암 판정일자, 판정의사 5
                string sPanjengDate6 = "";   //폐암       판정일자, 판정의사 6
                #endregion

                //상담미완료시 판정불가
                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWrtNo);

                if (!list.IsNullOrEmpty())
                {
                    strJepDate = list.JEPDATE;
                    if (list.SANGDAMDRNO.IsNullOrEmpty() || list.SANGDAMDRNO == 0)
                    {
                        MessageBox.Show("상담이 미완료 상태입니다.", "판정불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                strLUNGOK = "";
                HIC_JEPSU list2 = hicJepsuService.GetPanjengDatebyWrtNo(FnWrtNo);

                if (!list2.IsNullOrEmpty())
                {
                    if (!list2.PANJENGDATE.IsNullOrEmpty())
                    {
                        strLUNGOK = "OK";
                    }
                }


                //TODO : 스프레드 하드코딩방식 개선 요망
                strPANJENGDRNO1 = SSDR.ActiveSheet.Cells[0, 0].Text;
                strPANJENGDRNO2 = SSDR.ActiveSheet.Cells[1, 0].Text;
                strPANJENGDRNO3 = SSDR.ActiveSheet.Cells[2, 0].Text;
                strPANJENGDRNO4 = SSDR.ActiveSheet.Cells[3, 0].Text;
                strPANJENGDRNO5 = SSDR.ActiveSheet.Cells[4, 0].Text;
                strPANJENGDRNO6 = SSDR.ActiveSheet.Cells[5, 0].Text;
                strPANJENGDRNO7 = SSDR.ActiveSheet.Cells[6, 0].Text;
                strPANJENGDRNO8 = SSDR.ActiveSheet.Cells[7, 0].Text;
                strPANJENGDRNO9 = SSDR.ActiveSheet.Cells[8, 0].Text;
                strPANJENGDRNO10 = SSDR.ActiveSheet.Cells[9, 0].Text;
                strPANJENGDRNO11 = SSDR.ActiveSheet.Cells[10, 0].Text;

                eInputStomachRes(null, null);
                eInputColonRes(null, null);
                eInputLiverRes(null, null);
                eInputBreastRes(null, null);
                eInputLungRes(null, null);


            if (cboResult0001.Text.Trim() != "")
                {
                    //strPANJENGDRNO3 = "30846";
                }

                //전산신연습 예외처리
                if (FstrPtno.Substring(0, 7) != "8100000")
                {
                    if ((tab1.Enabled == true && tabStomach2.Visible == true) && (strPANJENGDRNO2.IsNullOrEmpty() || strPANJENGDRNO2 == "0"))
                    {
                        MessageBox.Show("위암 검사의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //if ((tab1.Enabled == true && tabStomach2.Visible == true) && (!cboResult0001.Text.IsNullOrEmpty() && strPANJENGDRNO3 != "30846"))
                    if ((tab1.Enabled == true && tabStomach2.Visible == true) && (!cboResult0001.Text.IsNullOrEmpty()) && strPANJENGDRNO3.IsNullOrEmpty())
                    {
                        MessageBox.Show("위암 병리진단의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (tab3.Enabled == true && (strPANJENGDRNO7.IsNullOrEmpty() || strPANJENGDRNO7 == "0"))
                    {
                        MessageBox.Show("간암 검사의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (tab4.Enabled == true && (strPANJENGDRNO8.IsNullOrEmpty() || strPANJENGDRNO8 == "0"))
                    {
                        MessageBox.Show("유방암 판독의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (tab5.Enabled == true && (strPANJENGDRNO9.IsNullOrEmpty() || strPANJENGDRNO10.IsNullOrEmpty() || strPANJENGDRNO9 == "0" || strPANJENGDRNO10 == "0"))
                    {
                        MessageBox.Show("자궁경부암 검체채취의사, 병리진단의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (tab6.Enabled == true && (strPANJENGDRNO11.IsNullOrEmpty() || strPANJENGDRNO11 == "0"))
                    {
                        MessageBox.Show("폐암판독의사를 확인해 주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                

                //암판정 화면에서는 판정만 저장가능!!

                if (hb.READ_JepsuSTS(FnWrtNo) == "D")
                {
                    MessageBox.Show("접수번호는 삭제된 번호입니다.", "접수상태확인요망", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                //문진입력완료 여부
                HIC_CANCER_NEW list3 = hicCancerNewService.GetRowIdbyWrtNo(FnWrtNo);

                if (list3.IsNullOrEmpty())
                {
                    MessageBox.Show("문진입력이 완료되지 않았습니다", "판정불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                else
                {
                    strROWID = list3.RID;
                }

                //출력했으면 판정 입력 및 수정불가
                if (hicCancerNewService.GetGbPrintbyWrtNo(FnWrtNo) == "Y")
                {
                    MessageBox.Show(FnWrtNo + " 결과지가 출력되었습니다." + "\r\n" + "판정입력이 불가능합니다.", "입력불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                //사전오류점검    GoSub DATA_ERROR_CHECK
                //판정일자
                if (chkCancer0.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpSPanDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpSPanDate.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[위암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkCancer1.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpCPanDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpCPanDate.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[대장암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkCancer2.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpLPanDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpLPanDate.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[간암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkCancer3.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpBPanDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpBPanDate.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[유방암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkCancer4.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpWPanDate.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpWPanDate.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[자궁경부암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkCancer5.Checked == true)
                {
                    DateTime DATE1 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                    DateTime DATE2 = dtpLPanDate1.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(dtpLPanDate1.Text);

                    if (DATE1 < DATE2)
                    {
                        MessageBox.Show(FnWrtNo + "[폐암]" + "\r\n" + "판정일은 접수일로 부터 14일을 경과할 수 없습니다.", "판정일자 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //위조영촬영 병변위치 체크
                strOK = "OK";
                for (int i = 0; i <= 2; i++)
                {
                    ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + i.ToString(), true)[0] as ComboBox);
                    if (string.Compare(VB.Left(cboForgedYoung.Text, 1), "1") > 0 && string.Compare(VB.Left(cboForgedYoung.Text, 1), "9") < 0)
                    {
                        //이상소견 선택시 병변위치 체크여부 확인
                        strOK = "";
                        for (int j = 0; j <= 7; j++)
                        {
                            CheckBox chkForgedYoung = (Controls.Find("chkForgedYoung" + ((i + 1) * 10 + j).ToString(), true)[0] as CheckBox);
                            if (chkForgedYoung.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }

                        if (strOK == "")
                        {
                            MessageBox.Show("위조영 관찰소견 병변위치가 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else if (VB.Left(cboForgedYoung.Text, 1) == "9")
                    {
                        //기타소견 선택시 기타 병변위치 체크여부 확인
                        strOK = "";
                        for (int j = 0; j <= 7; j++)
                        {
                            CheckBox chkForgedYoungEtc = (Controls.Find("chkForgedYoungEtc" + j.ToString(), true)[0] as CheckBox);
                            if (chkForgedYoungEtc.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }

                        if (strOK == "")
                        {
                            MessageBox.Show("위조영 기타 관찰소견이 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                //위내시경촬영
                for (int i = 0; i <= 2; i++)
                {
                    strOK = "OK"; strOK1 = ""; strOK2 = "";

                    ComboBox cboEndo = (Controls.Find("cboEndo" + i.ToString(), true)[0] as ComboBox);
                    if (string.Compare(VB.Left(cboEndo.Text, 2), "01") > 0 && VB.Left(cboEndo.Text, 2) != "09")
                    {
                        strOK = "";
                        for (int j = 0; j <= 7; j++)
                        {
                            CheckBox chkEndo = (Controls.Find("chkEndo" + ((i + 1) * 10 + j).ToString(), true)[0] as CheckBox);
                            if (chkEndo.Checked == true)
                            {
                                strOK = "OK";
                                if (j < 4) { strOK1 = "OK"; }
                                else { strOK2 = "OK"; }
                            }
                        }

                        if (strOK == "")
                        {
                            MessageBox.Show("위내시경 관찰소견 병변위치가 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }

                        if (strOK1 != strOK2)
                        {
                            MessageBox.Show("위내시경 관찰소견 병변위치 선택오류." + ComNum.VBLF + "병변위치 앞, 뒤 1개 이상 선택요망.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else if (VB.Left(cboEndo.Text, 2) == "09")
                    {
                        strOK = "";
                        for (int j = 0; j <= 7; j++)
                        {
                            CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + j.ToString(), true)[0] as CheckBox);
                            if (chkEndoEtc.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }

                        if (strOK == "")
                        {
                            MessageBox.Show("위내시경 기타 관찰소견이 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                //조직검사 시행인 경우
                if (rdoStomachTissue0.Checked)
                {
                    if (cboResult0001.Text.Trim() == "")
                    {
                        MessageBox.Show("조직진단이 공란입니다.", "조직검사 시행대상", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //위암 조직진단 암 체크
                strOK = "";
                if (VB.Pstr(cboResult0001.Text, ".", 1) == "07")
                {
                    for (int i = 0; i <= 13; i++)
                    {
                        CheckBox chkStomachTissue = (Controls.Find("chkStomachTissue" + i.ToString(), true)[0] as CheckBox);

                        if (chkStomachTissue.Checked)
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "")
                    {
                        MessageBox.Show("조직검사 암진단 구분 선택요망.", "조직검사 암진단 대상", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                else if (VB.Pstr(cboResult0001.Text, ".", 1) == "08")
                {
                    for (int i = 0; i <= 7; i++)
                    {
                        CheckBox chkStomachTissueEtc = (Controls.Find("chkStomachTissueEtc" + i.ToString(), true)[0] as CheckBox);

                        if (chkStomachTissueEtc.Checked)
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "")
                    {
                        MessageBox.Show("조직검사 조직진단 기타 구분 선택요망.", "조직검사 조직진단 기타 대상", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //위암판정시 직접기입란 체크
                if (chkForgedYoungEtc7.Checked)
                {
                    if (txtForgedYoungEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("위장조영 판정 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkEndoEtc7.Checked)
                {
                    if (txtEndoEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("위내시경 판정 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkStomachTissue13.Checked)
                {
                    if (txtStomachTissueEtcCancer.Text.Trim() == "")
                    {
                        MessageBox.Show("위내시경 조직검사 암진단 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkStomachTissueEtc7.Checked)
                {
                    if (txtStomachTissueEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("위내시경 조직검사 조직진단 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //위암판정 기타시 기타소견란 공백 체크
                if (VB.Pstr(cboSPan.Text, ".", 1) == "5")
                {
                    if (txtJilEtcS.Text.Trim() == "")
                    {
                        MessageBox.Show("위암 종합판정 기타 소견란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //대장내시경
                strOK = "OK";
                for (int i = 0; i <= 2; i++)
                {
                    ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + i.ToString(), true)[0] as ComboBox);
                    
                    if (VB.Left(cboColonoScope.Text, 1) == "2")
                    {
                        if (txtCSize1.Text.Trim() == "" || txtCSize1.Text.Trim().To<int>(0) == 0)
                        {
                            MessageBox.Show("대장내시경 판독소견 대장용종의 경우 용종의 크기를 입력바랍니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else if (VB.Left(cboColonoScope.Text, 1) == "5")
                    {
                        strOK = "";
                        for (int j = 0; j <= 9; j++)
                        {
                            CheckBox chkColonoScopeEtc = (Controls.Find("chkColonoScopeEtc" + j.ToString(), true)[0] as CheckBox);
                            if (chkColonoScopeEtc.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }
                        if (strOK == "")
                        {
                            MessageBox.Show("대장내시경 판독소견이 5.기타 일경우 기타소견 항목을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else if (string.Compare(VB.Left(cboColonoScope.Text, 1), "1") > 0 && string.Compare(VB.Left(cboColonoScope.Text, 1), "5") < 0)
                    {
                        strOK = "";
                        for (int j = 0; j <= 9; j++)
                        {
                            CheckBox chkColonoScope = (Controls.Find("chkColonoScope" + ((i + 1) * 10 + j).ToString(), true)[0] as CheckBox);
                            if (chkColonoScope.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }
                        if (strOK == "")
                        {
                            MessageBox.Show("대장내시경 판독소견 병변위치가 선택되지 않았습니다", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                if (chkColonizationAassistantEtc9.Checked)
                {
                    if (txtColonizationAassistantEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("결장이중조영 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkColonoScopeEtc9.Checked)
                {
                    if (txtColonScopeEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("대장내시경 판정 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkColonCancer12.Checked)
                {
                    if (txtColonEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("대장내시경 조직검사 암진단 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkColonCancerEtc4.Checked)
                {
                    if (txtColonCancerEtc.Text.Trim() == "")
                    {
                        MessageBox.Show("대장내시경 조직검사 암진단 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //대장암판정 기타시 기타소견란 공백 체크
                if (VB.Pstr(cboCPan.Text, ".", 1) == "5")
                {
                    if (txtJilEtcC.Text.Trim() == "")
                    {
                        MessageBox.Show("대장암 종합판정 기타 소견란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //간암판정
                if (chkResult00107.Checked)
                {
                    if (txtResult0012.Text.Trim() == "")
                    {
                        MessageBox.Show("간암판정 기타 직접기입란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //간암판정 1cm이상 고형종괴 선택시 병변크기 입력 체크
                for (int i = 0; i < 8; i++)
                {
                    CheckBox chkResult0006 = (Controls.Find("chkResult0006" + i.ToString(), true)[0] as CheckBox);
                    if (chkResult0006.Checked)
                    {
                        if (txtResult0007.Text.Trim() == "" && txtResult0008.Text.Trim() == "" && txtResult0009.Text.Trim() == "")
                        {
                            MessageBox.Show("간암 1cm이상 고형종괴 병변크기를 입력해주세요.", "1cm이상 고형 종괴 병변크기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }

                        if (txtResult0007.Text.To<double>(0.0) < 1.0 && txtResult0008.Text.To<double>(0.0) < 1.0 && txtResult0009.Text.To<double>(0.0) < 1.0)
                        {
                            MessageBox.Show("간암의 병변크기는 1.0cm 이상 이여야 합니다.", "1cm이상 고형 종괴 병변위치", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                //간암판정 기타시 기타소견란 공백 체크
                if (VB.Pstr(cboLPan.Text, ".", 1) == "4")
                {
                    if (txtJilEtcL.Text.Trim() == "")
                    {
                        MessageBox.Show("간암 종합판정 기타 소견란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //유방암촬영                
                for (int i = 0; i <= 2; i++)
                {
                    ComboBox cboBreast = (Controls.Find("cboBreast" + i.ToString(), true)[0] as ComboBox);
                    if (cboBreast.SelectedIndex > 1 && cboBreast.SelectedIndex < 9)
                    {
                        strOK = "";
                        for (int j = 0; j <= 6; j++)
                        {
                            CheckBox chkBreastR = (Controls.Find("chkBreastR" + ((i + 1) * 10 + j).ToString(), true)[0] as CheckBox);
                            CheckBox chkBreastL = (Controls.Find("chkBreastL" + ((i + 1) * 10 + j).ToString(), true)[0] as CheckBox);
                            if (chkBreastR.Checked == true)
                            {
                                strOK = "OK";
                            }
                            if (chkBreastL.Checked == true)
                            {
                                strOK = "OK";
                            }
                        }
                        if (strOK == "")
                        {
                            MessageBox.Show("유방암 판독소견 병변위치가 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else if (VB.Left(cboBreast.Text, 2) == "10")    //직접기입
                    {
                        if (txtBreastReadOpinionEtc.Text.Trim() == "")
                        {
                            MessageBox.Show("유방암 판독소견(직접기입) 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                //유방암 판독소견 직접기입 공란 체크
                if (chkBreastR16.Checked)
                {
                    if (txtBreastPosEtc10.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 오른쪽 판독소견1 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkBreastL16.Checked)
                {
                    if (txtBreastPosEtc11.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 왼쪽 판독소견1 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkBreastR26.Checked)
                {
                    if (txtBreastPosEtc20.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 오른쪽 판독소견2 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkBreastL26.Checked)
                {
                    if (txtBreastPosEtc21.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 왼쪽 판독소견2 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkBreastR36.Checked)
                {
                    if (txtBreastPosEtc30.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 오른쪽 판독소견3 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (chkBreastL36.Checked)
                {
                    if (txtBreastPosEtc31.Text.Trim() == "")
                    {
                        MessageBox.Show("유방암 왼쪽 판독소견3 직접기입 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //자궁경부암판정 기타시 기타소견란 공백 체크
                if (VB.Pstr(cboWPan.Text, ".", 1) == "6")
                {
                    if (txtJilEtcM.Text.Trim() == "")
                    {
                        MessageBox.Show("자궁경부암 종합판정 기타 소견란이 공란입니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                if (tab6.Enabled == true)
                {
                    if (txtResult7.Text.Trim() == "")
                    {
                        MessageBox.Show("선량값을 입력하세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (chkResult010.Checked == true && txtLungResult30.Text != "")
                    {
                        MessageBox.Show("폐결절소견1 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult011.Checked == true && txtLungResult31.Text != "")
                    {
                        MessageBox.Show("폐결절소견2 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult012.Checked == true && txtLungResult32.Text != "")
                    {
                        MessageBox.Show("폐결절소견3 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult013.Checked == true && txtLungResult33.Text != "")
                    {
                        MessageBox.Show("폐결절소견4 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult014.Checked == true && txtLungResult34.Text != "")
                    {
                        MessageBox.Show("폐결절소견5 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult015.Checked == true && txtLungResult35.Text != "")
                    {
                        MessageBox.Show("폐결절소견6 폐결절유무-무 인경우 결절범주가 없어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    if ((chkResult020.Checked == true || chkResult030.Checked == true) && txtLungResult30.Text == "")
                    {
                        MessageBox.Show("폐결절소견1 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if ((chkResult021.Checked == true || chkResult031.Checked == true) && txtLungResult31.Text == "")
                    {
                        MessageBox.Show("폐결절소견2 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if ((chkResult022.Checked == true || chkResult032.Checked == true) && txtLungResult32.Text == "")
                    {
                        MessageBox.Show("폐결절소견3 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if ((chkResult023.Checked == true || chkResult033.Checked == true) && txtLungResult33.Text == "")
                    {
                        MessageBox.Show("폐결절소견4 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if ((chkResult024.Checked == true || chkResult034.Checked == true) && txtLungResult34.Text == "")
                    {
                        MessageBox.Show("폐결절소견5 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if ((chkResult025.Checked == true || chkResult035.Checked == true) && txtLungResult35.Text == "")
                    {
                        MessageBox.Show("폐결절소견6 폐결절유무-유/석회화 또는 지방 포함 결절 인경우  결절범주가 있어야합니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    if (chkResult160.Checked == true && (chkResult180.Checked == false && chkResult190.Checked == false))
                    {
                        MessageBox.Show("폐결절소견1 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult161.Checked == true && (chkResult181.Checked == false && chkResult191.Checked == false))
                    {
                        MessageBox.Show("폐결절소견2 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult162.Checked == true && (chkResult182.Checked == false && chkResult192.Checked == false))
                    {
                        MessageBox.Show("폐결절소견3 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult163.Checked == true && (chkResult183.Checked == false && chkResult193.Checked == false))
                    {
                        MessageBox.Show("폐결절소견4 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult164.Checked == true && (chkResult184.Checked == false && chkResult194.Checked == false))
                    {
                        MessageBox.Show("폐결절소견5 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult165.Checked == true && (chkResult185.Checked == false && chkResult195.Checked == false))
                    {
                        MessageBox.Show("폐결절소견6 추적검사소견 변화있음값을 선택해주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (rdoResult61.Checked == true && txtResult11.Text == "")
                    {
                        MessageBox.Show("기관지내 병변 있음시 내용을 적어주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult204.Checked == true && txtResult4.Text == "")
                    {
                        MessageBox.Show("폐결절 외 폐암시사 소견 기타 내용을 적어주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (chkResult218.Checked == true && txtResult5.Text == "")
                    {
                        MessageBox.Show("폐결절 외 의미있는 소견 기타 내용을 적어주세요.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (VB.Left(cboLPan1.Text, 1) == "4" && cboLPan2.Text == "")
                    {
                        MessageBox.Show("폐암 판정에대한 구분이 선택되지 않았습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (VB.Left(cboLPan1.Text, 1) == "5" && txtResult8.Text == "")
                    {
                        MessageBox.Show("폐암 판정에대한 기타소견이 없습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        CheckBox chkResult09 = (Controls.Find("chkResult09" + i.ToString(), true)[0] as CheckBox);
                        TextBox txtLungRes1 = (Controls.Find("txtLungResult1" + i.ToString(), true)[0] as TextBox);

                        if (chkResult09.Checked && txtLungRes1.Text.Trim() == "")
                        {
                            MessageBox.Show("폐암 결절선상 고형 결절크기가 없습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        CheckBox chkResult10 = (Controls.Find("chkResult10" + i.ToString(), true)[0] as CheckBox);
                        TextBox txtLungRes2 = (Controls.Find("txtLungResult2" + i.ToString(), true)[0] as TextBox);

                        if (chkResult10.Checked && txtLungRes2.Text.Trim() == "")
                        {
                            MessageBox.Show("폐암 결절선상 부분고형 결절크기가 없습니다.", "판정내용 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }


                }

                //판정의 clear
                strPanjengDrNo = "";
                strPanDate = "";
                for (int i = 0; i <= 5; i++)
                {
                    strPanDrNo[i] = "0";
                }

                //개별소견 세팅
                for (int i = 0; i <= 9; i++)
                {
                    strNewSogen[i] = "";
                }

                strNewSogen[1] = txtS_Sogen.Text.Replace("\r\n", "");
                strNewSogen[2] = txtS_Sogen2.Text.Replace("\r\n", "");
                strNewSogen[3] = txtC_Sogen.Text.Replace("\r\n", "");
                strNewSogen[4] = txtC_Sogen2.Text.Replace("\r\n", "");
                strNewSogen[5] = txtC_Sogen3.Text.Replace("\r\n", "");
                strNewSogen[6] = txtL_Sogen.Text.Replace("\r\n", "");
                strNewSogen[7] = txtB_Sogen.Text.Replace("\r\n", "");
                strNewSogen[8] = txtW_Sogen.Text.Replace("\r\n", "");
                strNewSogen[9] = txtL_Sogen.Text.Replace("\r\n", "");

                for (int i = 0; i < 10; i++)
                {
                    strWomb[i] = "";
                }

                //위
                //위조직진단
                if (cboResult0001.SelectedIndex > 0)
                {
                    strResult0001 = VB.Left(cboResult0001.Text, 2);
                }
                strResult0016 = chkResult0016.Checked == true ? "1" : "0";

                //대장
                //맹장삽입여부
                strResult0002 = "2";
                if (rdoResult00020.Checked == true)
                {
                    strResult0002 = "1";
                }
                //장정결도
                strResult0003 = "2";
                if (rdoResult00030.Checked == true)
                {
                    strResult0003 = "1";
                }

                //간암
                //관찰소견
                for (int i = 0; i <= 3; i++)
                {
                    CheckBox chkResult0004 = (Controls.Find("chkResult0004" + i.ToString(), true)[0] as CheckBox);
                    strResult0004 += chkResult0004.Checked == true ? "1" : "0";
                }
                for (int i = 0; i <= 1; i++)
                {
                    CheckBox chkResult0015 = (Controls.Find("chkResult0015" + i.ToString(), true)[0] as CheckBox);
                    strResult0015 += chkResult0015.Checked == true ? "1" : "0";
                }

                //병변위치
                for (int i = 0; i <= 7; i++)
                {
                    CheckBox chkResult0005 = (Controls.Find("chkResult0005" + i.ToString(), true)[0] as CheckBox);
                    strResult0005 += chkResult0005.Checked == true ? "1" : "0";
                }
                for (int i = 0; i <= 7; i++)
                {
                    CheckBox chkResult0006 = (Controls.Find("chkResult0006" + i.ToString(), true)[0] as CheckBox);
                    strResult0006 += chkResult0006.Checked == true ? "1" : "0";
                }
                for (int i = 0; i <= 8; i++)
                {
                    CheckBox chkResult0010 = (Controls.Find("chkResult0010" + i.ToString(), true)[0] as CheckBox);
                    strResult0010 += chkResult0010.Checked == true ? "1" : "0";
                }
                strResult0011 = txtResult0011.Text.Trim();
                strResult0012 = txtResult0012.Text.Trim();

                //유방
                //촬영부위
                if (cboResult0013.SelectedIndex > 0)
                {
                    strResult0013 = cboResult0013.SelectedIndex.To<string>();
                }

                //자궁
                //비정상 선상피세포
                if (chkResult00141.Checked) { strResult0014 = "1"; }
                else if (chkResult00142.Checked) { strResult0014 = "2"; }
                else { strResult0014 = ""; }

                //폐암
                strLUNG002 = rdoCTYN0.Checked == true ? "1" : "2";
                if (!txtResult6.Text.IsNullOrEmpty())
                {
                    strLUNG003 = VB.Left(txtResult6.Text, 4);
                    strLUNG004 = VB.Right(txtResult6.Text, 2);
                }

                //폐결절 소견1
                if (tabLung1.Visible == true)
                {
                    if (chkResult010.Checked == true) { strLUNG005 = "1"; }
                    if (chkResult020.Checked == true) { strLUNG005 = "2"; }
                    if (chkResult030.Checked == true) { strLUNG005 = "3"; }

                    if (chkResult040.Checked == true) { strLUNG007 = "1"; }
                    if (chkResult050.Checked == true) { strLUNG007 = "2"; }
                    if (chkResult060.Checked == true) { strLUNG007 = "3"; }
                    if (chkResult070.Checked == true) { strLUNG007 = "4"; }
                    if (chkResult080.Checked == true) { strLUNG007 = "5"; }

                    if (chkResult090.Checked == true) { strLUNG006 = "1"; }
                    if (chkResult100.Checked == true) { strLUNG006 = "2"; }
                    if (chkResult110.Checked == true) { strLUNG006 = "3"; }

                    if (chkResult120.Checked == true) { strLUNG010 = "1"; }
                    if (chkResult130.Checked == true) { strLUNG010 = "2"; }
                    if (chkResult140.Checked == true) { strLUNG010 = "3"; }

                    if (chkResult150.Checked == true) { strLUNG011 = "1"; }
                    if (chkResult160.Checked == true) { strLUNG011 = "2"; }
                    if (chkResult170.Checked == true) { strLUNG011 = "3"; }

                    if (chkResult180.Checked == true) { strLUNG012 = "1"; }
                    if (chkResult190.Checked == true) { strLUNG012 = "2"; }

                    strLUNG008 = txtLungResult10.Text;
                    strLUNG009 = txtLungResult20.Text;
                    strLUNG072 = txtLungResult30.Text;
                }

                //폐결절 소견2
                if (tabLung2.Visible == true)
                {
                    if (chkResult011.Checked == true) { strLUNG013 = "1"; }
                    if (chkResult021.Checked == true) { strLUNG013 = "2"; }
                    if (chkResult031.Checked == true) { strLUNG013 = "3"; }

                    if (chkResult041.Checked == true) { strLUNG015 = "1"; }
                    if (chkResult051.Checked == true) { strLUNG015 = "2"; }
                    if (chkResult061.Checked == true) { strLUNG015 = "3"; }
                    if (chkResult071.Checked == true) { strLUNG015 = "4"; }
                    if (chkResult081.Checked == true) { strLUNG015 = "5"; }

                    if (chkResult091.Checked == true) { strLUNG014 = "1"; }
                    if (chkResult101.Checked == true) { strLUNG014 = "2"; }
                    if (chkResult111.Checked == true) { strLUNG014 = "3"; }

                    if (chkResult121.Checked == true) { strLUNG018 = "1"; }
                    if (chkResult131.Checked == true) { strLUNG018 = "2"; }
                    if (chkResult141.Checked == true) { strLUNG018 = "3"; }

                    if (chkResult151.Checked == true) { strLUNG019 = "1"; }
                    if (chkResult161.Checked == true) { strLUNG019 = "2"; }
                    if (chkResult171.Checked == true) { strLUNG019 = "3"; }

                    if (chkResult181.Checked == true) { strLUNG020 = "1"; }
                    if (chkResult191.Checked == true) { strLUNG020 = "2"; }

                    strLUNG016 = txtLungResult11.Text;
                    strLUNG017 = txtLungResult21.Text;
                    strLUNG073 = txtLungResult31.Text;
                }

                //폐결절 소견3
                if (tabLung3.Visible == true)
                {
                    if (chkResult012.Checked == true) { strLUNG021 = "1"; }
                    if (chkResult022.Checked == true) { strLUNG021 = "2"; }
                    if (chkResult032.Checked == true) { strLUNG021 = "3"; }

                    if (chkResult042.Checked == true) { strLUNG023 = "1"; }
                    if (chkResult052.Checked == true) { strLUNG023 = "2"; }
                    if (chkResult062.Checked == true) { strLUNG023 = "3"; }
                    if (chkResult072.Checked == true) { strLUNG023 = "4"; }
                    if (chkResult082.Checked == true) { strLUNG023 = "5"; }

                    if (chkResult092.Checked == true) { strLUNG022 = "1"; }
                    if (chkResult102.Checked == true) { strLUNG022 = "2"; }
                    if (chkResult112.Checked == true) { strLUNG022 = "3"; }

                    if (chkResult122.Checked == true) { strLUNG026 = "1"; }
                    if (chkResult132.Checked == true) { strLUNG026 = "2"; }
                    if (chkResult142.Checked == true) { strLUNG026 = "3"; }

                    if (chkResult152.Checked == true) { strLUNG027 = "1"; }
                    if (chkResult162.Checked == true) { strLUNG027 = "2"; }
                    if (chkResult172.Checked == true) { strLUNG027 = "3"; }

                    if (chkResult182.Checked == true) { strLUNG028 = "1"; }
                    if (chkResult192.Checked == true) { strLUNG028 = "2"; }

                    strLUNG024 = txtLungResult12.Text;
                    strLUNG025 = txtLungResult22.Text;
                    strLUNG074 = txtLungResult32.Text;
                }

                //폐결절 소견4
                if (tabLung4.Visible == true)
                {
                    if (chkResult013.Checked == true) { strLUNG029 = "1"; }
                    if (chkResult023.Checked == true) { strLUNG029 = "2"; }
                    if (chkResult033.Checked == true) { strLUNG029 = "3"; }

                    if (chkResult043.Checked == true) { strLUNG031 = "1"; }
                    if (chkResult053.Checked == true) { strLUNG031 = "2"; }
                    if (chkResult063.Checked == true) { strLUNG031 = "3"; }
                    if (chkResult073.Checked == true) { strLUNG031 = "4"; }
                    if (chkResult083.Checked == true) { strLUNG031 = "5"; }

                    if (chkResult093.Checked == true) { strLUNG030 = "1"; }
                    if (chkResult103.Checked == true) { strLUNG030 = "2"; }
                    if (chkResult113.Checked == true) { strLUNG030 = "3"; }

                    if (chkResult123.Checked == true) { strLUNG034 = "1"; }
                    if (chkResult133.Checked == true) { strLUNG034 = "2"; }
                    if (chkResult143.Checked == true) { strLUNG034 = "3"; }

                    if (chkResult153.Checked == true) { strLUNG035 = "1"; }
                    if (chkResult163.Checked == true) { strLUNG035 = "2"; }
                    if (chkResult173.Checked == true) { strLUNG035 = "3"; }

                    if (chkResult183.Checked == true) { strLUNG036 = "1"; }
                    if (chkResult193.Checked == true) { strLUNG036 = "2"; }

                    strLUNG032 = txtLungResult13.Text;
                    strLUNG033 = txtLungResult23.Text;
                    strLUNG075 = txtLungResult33.Text;
                }

                //폐결절 소견5
                if (tabLung5.Visible == true)
                {
                    if (chkResult014.Checked == true) { strLUNG037 = "1"; }
                    if (chkResult024.Checked == true) { strLUNG037 = "2"; }
                    if (chkResult034.Checked == true) { strLUNG037 = "3"; }

                    if (chkResult044.Checked == true) { strLUNG039 = "1"; }
                    if (chkResult054.Checked == true) { strLUNG039 = "2"; }
                    if (chkResult064.Checked == true) { strLUNG039 = "3"; }
                    if (chkResult074.Checked == true) { strLUNG039 = "4"; }
                    if (chkResult084.Checked == true) { strLUNG039 = "5"; }

                    if (chkResult094.Checked == true) { strLUNG038 = "1"; }
                    if (chkResult104.Checked == true) { strLUNG038 = "2"; }
                    if (chkResult114.Checked == true) { strLUNG038 = "3"; }

                    if (chkResult124.Checked == true) { strLUNG042 = "1"; }
                    if (chkResult134.Checked == true) { strLUNG042 = "2"; }
                    if (chkResult144.Checked == true) { strLUNG042 = "3"; }

                    if (chkResult154.Checked == true) { strLUNG043 = "1"; }
                    if (chkResult164.Checked == true) { strLUNG043 = "2"; }
                    if (chkResult174.Checked == true) { strLUNG043 = "3"; }

                    if (chkResult184.Checked == true) { strLUNG044 = "1"; }
                    if (chkResult194.Checked == true) { strLUNG044 = "2"; }

                    strLUNG040 = txtLungResult14.Text;
                    strLUNG041 = txtLungResult24.Text;
                    strLUNG076 = txtLungResult34.Text;
                }

                //폐결절 소견6
                if (tabLung6.Visible == true)
                {
                    if (chkResult015.Checked == true) { strLUNG045 = "1"; }
                    if (chkResult025.Checked == true) { strLUNG045 = "2"; }
                    if (chkResult035.Checked == true) { strLUNG045 = "3"; }

                    if (chkResult045.Checked == true) { strLUNG047 = "1"; }
                    if (chkResult055.Checked == true) { strLUNG047 = "2"; }
                    if (chkResult065.Checked == true) { strLUNG047 = "3"; }
                    if (chkResult075.Checked == true) { strLUNG047 = "4"; }
                    if (chkResult085.Checked == true) { strLUNG047 = "5"; }

                    if (chkResult095.Checked == true) { strLUNG046 = "1"; }
                    if (chkResult105.Checked == true) { strLUNG046 = "2"; }
                    if (chkResult115.Checked == true) { strLUNG046 = "3"; }

                    if (chkResult125.Checked == true) { strLUNG050 = "1"; }
                    if (chkResult135.Checked == true) { strLUNG050 = "2"; }
                    if (chkResult145.Checked == true) { strLUNG050 = "3"; }

                    if (chkResult155.Checked == true) { strLUNG051 = "1"; }
                    if (chkResult165.Checked == true) { strLUNG051 = "2"; }
                    if (chkResult175.Checked == true) { strLUNG051 = "3"; }

                    if (chkResult185.Checked == true) { strLUNG052 = "1"; }
                    if (chkResult195.Checked == true) { strLUNG052 = "2"; }

                    strLUNG048 = txtLungResult15.Text;
                    strLUNG049 = txtLungResult25.Text;
                    strLUNG077 = txtLungResult35.Text;
                }

                //기관지내 병변
                strLUNG053 = rdoResult60.Checked == true ? "2" : "1";
                strLUNG054 = txtResult11.Text;

                //for (int i = 0; i <= 4; i++)
                //{
                //    if (chkResult200.Checked == true)
                //    {
                //        strLUNG055 = (i + 1).To<string>();
                //        break;
                //    }
                //}


                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkResult20 = (Controls.Find("chkResult20" + i.ToString(), true)[0] as CheckBox);
                    if (chkResult20.Checked == true)
                    {
                        strLUNG055 = (i + 1).To<string>();
                        break;
                    }

                }


                strLUNG056 = txtResult4.Text;

                //폐결절 외 의미있는 소견
                strLUNG057 = chkResult210.Checked == true ? "1" : "0";
                strLUNG058 = chkResult211.Checked == true ? "1" : "0";
                strLUNG059 = chkResult212.Checked == true ? "1" : "0";
                strLUNG060 = chkResult213.Checked == true ? "1" : "0";
                strLUNG061 = chkResult214.Checked == true ? "1" : "0";
                strLUNG062 = chkResult215.Checked == true ? "1" : "0";
                strLUNG063 = chkResult216.Checked == true ? "1" : "0";
                strLUNG064 = chkResult217.Checked == true ? "1" : "0";
                strLUNG065 = chkResult218.Checked == true ? "1" : "0";
                strLUNG066 = txtResult5.Text;

                //비활동성 폐결핵
                strLUNG067 = rdoResult90.Checked == true ? "1" : "2";

                strLUNG068 = VB.Left(cboLPan1.Text, 1);
                strLUNG069 = VB.Left(cboLPan2.Text, 1);
                strLUNG070 = txtL_Sogen1.Text;
                strLUNG071 = txtL_Sogen2.Text;

                strLUNG078 = txtResult8.Text;
                //2020-04-16 (txtResult9 -> TxtResult12 변경)
                strLUNG079 = txtResult12.Text;
                strLUNG080 = txtResult10.Text;

                //위암판정==========================================================================================
                if (chkCancer0.Checked == true)
                {
                    strGbEndo = rdoStomachEndo0.Checked == true ? "1" : "2";        //위내시경 필요유무
                    strGbAnat = rdoStomachTissue0.Checked == true ? "1" : "2";      //위조직검사 필요유무
                    //위조영소견/위내시경소견
                    str위조영소견 = "";
                    str위내시경소견 = "";
                    for (int i = 0; i <= 2; i++)
                    {
                        ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + i.ToString(), true)[0] as ComboBox);
                        str위조영소견 += cboForgedYoung.SelectedIndex;
                    }
                    for (int i = 0; i <= 2; i++)
                    {
                        ComboBox cboEndo = (Controls.Find("cboEndo" + i.ToString(), true)[0] as ComboBox);
                        str위내시경소견 += VB.Left(cboEndo.Text, 2);
                    }
                    //위조영소견기타/위내시경소견기타
                    str위조영기타 = "";
                    str위내시경기타 = "";
                    for (int i = 0; i <= 7; i++)
                    {
                        CheckBox chkForgedYoungEtc = (Controls.Find("chkForgedYoungEtc" + i.ToString(), true)[0] as CheckBox);
                        str위조영기타 += chkForgedYoungEtc.Checked == true ? "1" : "0";
                    }
                    for (int i = 0; i <= 7; i++)
                    {
                        CheckBox chkEndoEtc = (Controls.Find("chkEndoEtc" + i.ToString(), true)[0] as CheckBox);
                        str위내시경기타 += chkEndoEtc.Checked == true ? "1" : "0";
                    }

                    //위장조영촬영병변부위
                    strPositS1 = "";
                    for (int i = 0; i <= 7; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkForgedYoung = (Controls.Find("chkForgedYoung" + (10 * j + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboForgedYoung = (Controls.Find("cboForgedYoung" + (j - 1).ToString(), true)[0] as ComboBox);

                            strPositS1 += chkForgedYoung.Checked == true ? cboForgedYoung.SelectedIndex.To<string>() : "0";
                        }
                    }

                    //위내시경검사병변부위
                    strPositS2 = "";
                    for (int i = 0; i <= 7; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkEndo = (Controls.Find("chkEndo" + (10 * j + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboEndo = (Controls.Find("cboEndo" + (j - 1).ToString(), true)[0] as ComboBox);

                            if (chkEndo.Checked == true)
                            {
                                strPositS2 += cboEndo.SelectedIndex > 0 ? "1" : "0";
                            }
                            else
                            {
                                strPositS2 += "0";
                            }
                        }
                    }

                    //위조직진단
                    strJinS2 = cboStomachTissueCnt.SelectedIndex.To<string>();

                    //위조직진단시-암/ 위조직진단시-기타
                    strJinSB1 = "";
                    strJinSB2 = "";
                    for (int i = 0; i <= 13; i++)
                    {
                        CheckBox chkStomachTissue = (Controls.Find("chkStomachTissue" + i.ToString(), true)[0] as CheckBox);
                        strJinSB1 += chkStomachTissue.Checked == true ? "1" : "0";
                    }
                    for (int i = 0; i <= 7; i++)
                    {
                        CheckBox chkStomachTissueEtc = (Controls.Find("chkStomachTissueEtc" + i.ToString(), true)[0] as CheckBox);
                        strJinSB2 += chkStomachTissueEtc.Checked == true ? "1" : "0";
                    }
                    str위암판정 = VB.Left(cboSPan.Text, 1);
                }

                //대장암판정==========================================================================================
                if (chkCancer1.Checked == true)
                {
                    //대장분변잠혈
                    if (cboDenotationSubcutaneousBlood.SelectedIndex > 0)
                    {
                        str분변잠혈 = cboDenotationSubcutaneousBlood.SelectedIndex.To<string>();
                    }

                    //대장조영/대장내시경
                    str대장조영 = "";
                    str대장내시경 = "";

                    for (int i = 0; i <= 2; i++)
                    {
                        ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + i.ToString(), true)[0] as ComboBox);
                        str대장조영 += cboColonizationAassistant.SelectedIndex.To<string>();
                    }

                    for (int i = 0; i <= 2; i++)
                    {
                        ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + i.ToString(), true)[0] as ComboBox);
                        str대장내시경 += cboColonoScope.SelectedIndex.To<string>();
                    }

                    //대장이중조영검사기타/ 대장내시경검사기타
                    strByungC1 = "";
                    strByungC2 = "";
                    for (int i = 0; i <= 9; i++)
                    {
                        CheckBox chkColonizationAassistantEtc = (Controls.Find("chkColonizationAassistantEtc" + i.ToString(), true)[0] as CheckBox);
                        strByungC1 += chkColonizationAassistantEtc.Checked == true ? "1" : "0";
                    }

                    for (int i = 0; i <= 9; i++)
                    {
                        CheckBox chkColonoScopeEtc = (Controls.Find("chkColonoScopeEtc" + i.ToString(), true)[0] as CheckBox);
                        strByungC2 += chkColonoScopeEtc.Checked == true ? "1" : "0";
                    }

                    //대장이중조영검사 병변위치
                    strPositC1 = "";
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkColonizationAassistant = (Controls.Find("chkColonizationAassistant" + (10 * j + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboColonizationAassistant = (Controls.Find("cboColonizationAassistant" + (j - 1).ToString(), true)[0] as ComboBox);

                            strPositC1 += chkColonizationAassistant.Checked == true ? cboColonizationAassistant.SelectedIndex.To<string>() : "0";
                        }
                    }

                    //대장내시경검사 병변위치
                    strPositC2 = "";
                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkColonoScope = (Controls.Find("chkColonoScope" + (10 * j + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboColonoScope = (Controls.Find("cboColonoScope" + (j - 1).ToString(), true)[0] as ComboBox);
                            if (chkColonoScope.Checked == true)
                            {
                                strPositC2 += cboColonoScope.SelectedIndex > 0 ? "1" : "0";
                            }
                            else
                            {
                                strPositC2 += "0";
                            }
                        }
                    }

                    //대장용종크기 및 절제여부                    
                    strSizeC1 = "";
                    strSizeC2 = "";
                    strSizeC1 = string.Format("{0:000}", txtCSize0.Text);
                    strSizeC2 = string.Format("{0:000}", txtCSize1.Text);
                    strSizeCut = rdoCut1.Checked == true ? "0" : "1";   //용종절제여부

                    //조직진단 필요여부
                    strGbCAnat = rdoColonTissue1.Checked == true ? "0" : "1";
                    //대장암 조직진단
                    if (cboColonTissue.SelectedIndex > 0)
                    {
                        strJinC1 = cboColonTissue.SelectedIndex.To<string>();
                    }
                    //대장용종개수
                    strJinC2 = cboColonTissueCnt.SelectedIndex.To<string>();
                    //대장조직진단시-암/ 대장암 조직진단시-기타
                    strJinCB1 = "";
                    strJinCB2 = "";
                    for (int i = 0; i <= 12; i++)
                    {
                        CheckBox chkColonCancer = (Controls.Find("chkColonCancer" + i.ToString(), true)[0] as CheckBox);
                        strJinCB1 += chkColonCancer.Checked == true ? "1" : "0";
                    }
                    for (int i = 0; i <= 4; i++)
                    {
                        CheckBox chkColonCancerEtc = (Controls.Find("chkColonCancerEtc" + i.ToString(), true)[0] as CheckBox);
                        strJinCB2 += chkColonCancerEtc.Checked == true ? "1" : "0";
                    }
                    str대장암판정 = VB.Left(cboCPan.Text, 1);
                }

                //간암판정==========================================================================================
                if (chkCancer2.Checked == true)
                {
                    if (cboNew_B.SelectedIndex > 0) { str간암B형항원 = cboNew_B.SelectedIndex.To<string>(); }
                    if (cboBRsult.SelectedIndex > 0) { str간암B형결과 = cboBRsult.SelectedIndex.To<string>(); }
                    if (cboNew_C.SelectedIndex > 0) { str간암C형항체 = cboNew_C.SelectedIndex.To<string>(); }
                    if (cboCResult.SelectedIndex > 0) { str간암C형결과 = cboCResult.SelectedIndex.To<string>(); }
                    if (cboRPHA.SelectedIndex > 0) { strRPHA = cboRPHA.SelectedIndex.To<string>(); }

                    str간암판정 = VB.Left(cboLPan.Text, 1);
                }

                //유방암판정==========================================================================================
                if (chkCancer3.Checked == true)
                {
                    if (cboBreast.SelectedIndex > 0)
                    {
                        str유방분포량 = cboBreast.SelectedIndex.To<string>();
                    }
                    //유방촬영소견
                    for (int i = 0; i <= 2; i++)
                    {
                        ComboBox cboBreast = (Controls.Find("cboBreast" + i.ToString(), true)[0] as ComboBox);
                        strBSogen += string.Format("{0:00}", cboBreast.SelectedIndex);
                    }
                    //유방판독소견별 병변위치(좌/우)
                    strPositB = "";
                    for (int i = 0; i <= 5; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkBreastR = (Controls.Find("chkBreastR" + (j * 10 + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboBreast = (Controls.Find("cboBreast" + (j - 1).ToString(), true)[0] as ComboBox);

                            strPositB += chkBreastR.Checked == true ? string.Format("{0:00}", cboBreast.SelectedIndex) : "00";
                        }
                    }

                    for (int i = 0; i <= 5; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            CheckBox chkBreastL = (Controls.Find("chkBreastL" + (j * 10 + i).ToString(), true)[0] as CheckBox);
                            ComboBox cboBreast = (Controls.Find("cboBreast" + (j - 1).ToString(), true)[0] as ComboBox);

                            strPositB += chkBreastL.Checked == true ? string.Format("{0:00}", cboBreast.SelectedIndex) : "00";
                        }
                    }

                    str유방암판정 = VB.Left(cboBPan.Text, 1);
                }

                //자궁암판정 ==========================================================================================
                if (chkCancer4.Checked == true)
                {
                    strWomb[1] = rdoWomBo11.Checked ? "1" : "2";                                            //검체상태
                    strWomb[2] = rdoWomBo21.Checked ? "1" : "2";                                            //선상피세포
                    strWomb[3] = rdoWomBo31.Checked ? "1" : rdoWomBo32.Checked ? "2" : "3";                 //유형별진단
                    //편평상피세포이상
                    if (chkWomBo4.Checked == false)                                                         
                    {
                        strWomb[4] = "";
                    }
                    else
                    {
                        if (rdoWomBo41.Checked) { strWomb[4] = "1"; }
                        else if (rdoWomBo42.Checked) { strWomb[4] = "2"; }
                        else if (rdoWomBo43.Checked) { strWomb[4] = "3"; }
                        else { strWomb[4] = "4"; }
                    }
                    //비정형 편평상피세포 위험구분
                    if (chkWomBo51.Checked) { strWomb[9] = "1"; }       
                    else if (chkWomBo52.Checked) { strWomb[9] = "2"; }
                    else { strWomb[9] = ""; }
                    //선상피세포이상
                    if (chkWomBo6.Checked == false)
                    {
                        strWomb[5] = "";
                    }
                    else
                    {
                        if (rdoWomBo61.Checked) { strWomb[5] = "1"; }
                        else if (rdoWomBo62.Checked) { strWomb[5] = "2"; }
                        else { strWomb[5] = "3"; }
                    }

                    strWomb[6] = txtWombo6_Etc.Text.Trim();             //선상피세포이상기타
                    strWomb[7] = "";             //추가소견
                    for (int i = 0; i < 8; i++)
                    {
                        RadioButton rdoWomBo = (Controls.Find("rdoWomBo7" + i.ToString(), true)[0] as RadioButton);
                        if (rdoWomBo.Checked)
                        {
                            //if (i > 0) { strWomb[7] = i.ToString(); }
                            strWomb[7] = i.ToString();
                            break;
                        }
                    }
                    strWomb[8] = txtWombo7_Etc.Text.Trim();             //추가소견기타
                    strWomb[10] = txtWombo3_Etc.Text.Trim();            //유형별진단 기타(자궁내막세포출현등)
                    str중복자궁 = "";
                    if (rdoCervixDuplex2.Checked == true)
                    {
                        str중복자궁 = "2";
                    }
                    str자궁암판정 = VB.Left(cboWPan.Text, 1);
                }

                //판정의사성명
                strPanjengDrNo = "";
                for (int i = 0; i <= 5; i++)
                {
                    TextBox txtDrNSabun = (Controls.Find("txtDrNSabun" + i.ToString(), true)[0] as TextBox);
                    if (!txtDrNSabun.Text.IsNullOrEmpty())
                    {
                        strPanDrNo[i] = txtDrNSabun.Text.Trim();
                        //접수마스타에 판정의사 업데이트
                        if (strPanjengDrNo.IsNullOrEmpty() && !strPanDrNo[i].IsNullOrEmpty())
                        {
                            strPanjengDrNo = strPanDrNo[i].Trim();
                        }
                        //청구제외 구분 Can_MirGbn

                        CheckBox chkMirGbn = (Controls.Find("chkMirGbn" + i.ToString(), true)[0] as CheckBox);
                        if (chkMirGbn.Checked == true)
                        {
                            str청구제외 += "1";
                        }
                        else
                        {
                            str청구제외 += "0";
                        }
                        //상담구분 체크
                        CheckBox chkSangDam = (Controls.Find("chkSangDam" + i.ToString(), true)[0] as CheckBox);
                        if (chkSangDam.Checked == true)
                        {
                            strSangDam += "1^^";
                        }
                        else
                        {
                            strSangDam += "0^^";
                        }
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_CANCER_NEW item = new HIC_CANCER_NEW();

                //위암판정 저장======================================================================================================
                if (chkCancer0.Checked == true)
                {
                    sGbCancer1 = "Y";
                    if (tabStomach1.Visible == true)
                    {
                        sStomach1 = "Y";

                        item.STOMACH_S = str위조영소견;
                        item.STOMACH_B = str위조영기타;
                        item.STOMACH_P = strPositS1;
                        item.STOMACH_PETC = txtForgedYoungEtc.Text.Trim();
                        item.S_ENDOGBN = strGbEndo; //위내시경 필요유무                        
                    }
                    if (tabStomach2.Visible == true)
                    {
                        sStomach2 = "Y";

                        //위조직검사가 있고 조직검사필요없음이면 강제로 "조직검사필요"로 변경
                        if (strJinS2.To<long>() >= 1 && strGbAnat == "2")
                        {
                            strGbAnat = "1";
                        }

                        item.STOMACH_SENDO = str위내시경소견;
                        item.STOMACH_BENDO = str위내시경기타;
                        item.STOMACH_PENDO = strPositS2;
                        item.STOMACH_ENDOETC = txtEndoEtc.Text.Trim();
                        item.S_ANATGBN = strGbAnat;
                        item.RESULT0001 = strResult0001;    //위조직검사
                        item.RESULT0016 = strResult0016;    //이물제거술

                        item.NEW_SICK54 = strJinS2;
                        item.NEW_SICK63 = strJinSB1;
                        item.NEW_SICK66 = txtStomachTissueEtcCancer.Text.Trim();
                        item.NEW_SICK67 = strJinSB2;
                        item.NEW_SICK68 = txtStomachTissueEtc.Text.Trim();
                    }
                    item.S_PANJENG = str위암판정;
                    item.S_JILETC = txtJilEtcS.Text.Trim();
                    item.PANJENGDRNO1 = strPANJENGDRNO1.To<long>();
                    item.PANJENGDRNO2 = strPANJENGDRNO2.To<long>();
                    item.PANJENGDRNO3 = strPANJENGDRNO3.To<long>();
                }

                //대장암 판정 저장======================================================================================================
                if (chkCancer1.Checked == true)
                {
                    sGbCancer2 = "Y";

                    if (tabColon1.Visible == true)
                    {
                        sColon1 = "Y";

                        item.COLON_RESULT = str분변잠혈;
                    }

                    if (tabColon2.Visible == true)
                    {
                        sColon2 = "Y";

                        item.COLON_S = str대장조영;
                        item.COLON_B = strByungC1;
                        item.COLON_P = strPositC1;
                        item.COLON_PETC = txtColonizationAassistantEtc.Text.Trim();
                        item.NEW_SICK33 = strSizeC1;

                        item.RESULT0002 = strResult0002;
                        item.RESULT0003 = strResult0003;
                    }

                    if (tabColon3.Visible == true)
                    {
                        sColon3 = "Y";

                        item.COLON_SENDO = str대장내시경;
                        item.COLON_BENDO = strByungC2;
                        item.COLON_PENDO = strPositC2;
                        item.COLON_ENDOETC = txtColonScopeEtc.Text.Trim();
                        item.C_ANATGBN = strGbCAnat;
                        item.NEW_SICK71 = strJinC1;
                        item.NEW_SICK69 = strJinCB1;
                        item.NEW_SICK72 = txtColonEtc.Text.Trim();
                        item.NEW_SICK73 = strJinCB2;
                        item.NEW_SICK74 = txtColonCancerEtc.Text.Trim();
                        item.NEW_SICK34 = strSizeCut;
                        item.NEW_SICK38 = strSizeC2;
                        item.NEW_SICK59 = strJinC2;
                        item.PANJENGDRNO4 = strPANJENGDRNO4.To<long>();
                        item.PANJENGDRNO5 = strPANJENGDRNO5.To<long>();
                        item.PANJENGDRNO6 = strPANJENGDRNO6.To<long>();
                    }
                    item.C_PANJENG = str대장암판정;
                    item.C_JILETC = txtJilEtcC.Text.Trim();
                }

                //간암판정 저장======================================================================================================
                if (chkCancer2.Checked == true)
                {
                    sGbCancer3 = "Y";

                    if (grpLiverUltrasonography2.Enabled == true)
                    {
                        sLiver1 = "Y";

                        item.RESULT0004 = strResult0004;
                        item.RESULT0005 = strResult0005;
                        item.RESULT0006 = strResult0006;
                        item.RESULT0007 = txtResult0007.Text.Trim();
                        item.RESULT0008 = txtResult0008.Text.Trim();
                        item.RESULT0009 = txtResult0009.Text.Trim();
                        item.RESULT0010 = strResult0010;    //간초음파소견-기타
                        item.RESULT0011 = strResult0011;    //간초음파소견-기타
                        item.RESULT0012 = strResult0012;    //간초음파소견-기타
                        item.RESULT0015 = strResult0015;    //간낭종
                    }

                    if (grpLiverUltrasonography1.Enabled == true)
                    {
                        sLiver2 = "Y";

                        item.LIVER_RPHA = strRPHA;
                        item.LIVER_EIA = txtEIA.Text.Trim();
                    }

                    if (grpHepatitisExam.Enabled == true)
                    {
                        sLiver3 = "Y";

                        item.LIVER_NEW_ALT = txtALT.Text.Trim();
                        item.LIVER_NEW_B = str간암B형항원;
                        item.LIVER_NEW_BRESULT = str간암B형결과;
                        item.LIVER_NEW_C = str간암C형항체;
                        item.LIVER_NEW_CRESULT = str간암C형결과;
                    }
                    item.LIVER_PANJENG = str간암판정;
                    item.LIVER_JILETC = txtJilEtcL.Text.Trim();
                    item.PANJENGDRNO7 = strPANJENGDRNO7.To<long>();
                }

                //유방암판정 저장======================================================================================================
                if (chkCancer3.Checked == true)
                {
                    sGbCancer4 = "Y";

                    item.RESULT0013 = strResult0013;                        //유방촬영검사(1.양측, 2.편측(우), 3.편측(좌)
                    item.B_ANATGBN = str유방분포량;                          //유방실질분포량
                    item.BREAST_S = strBSogen;                               //유방소견
                    item.NEW_WOMAN17 = txtBreastReadOpinionEtc.Text.Trim();  //유방소견기타
                    item.BREAST_P = strPositB;                               //유방단순촬영부위 우,좌
                    item.BREAST_ETC = txtBreastPosEtc10.Text.Trim();         //유방부위기타1(우)
                    item.B_ANATETC = txtBreastPosEtc11.Text.Trim();          //유방부위기타1(좌)
                    item.BREAST_ETC2 = txtBreastPosEtc20.Text.Trim();        //유방부위기타2(우)
                    item.B_ANATETC2 = txtBreastPosEtc21.Text.Trim();         //유방부위기타2(좌)
                    item.BREAST_ETC3 = txtBreastPosEtc30.Text.Trim();        //유방부위기타3(우)
                    item.B_ANATETC3 = txtBreastPosEtc30.Text.Trim();         //유방부위기타3(좌)
                    item.B_PANJENG = str유방암판정;
                    item.B_JILETC = txtJilEtcB.Text.Trim();
                    item.PANJENGDRNO8 = strPANJENGDRNO8.To<long>();
                }

                //자궁암판정======================================================================================================
                if (chkCancer4.Checked == true)
                {
                    sGbCancer5 = "Y";

                    item.RESULT0014 = strResult0014;    //자궁-비정상 선상피세표 (1.일반 2.종양성)
                    item.WOMB01 = strWomb[1];
                    item.WOMB02 = strWomb[2];
                    item.WOMB03 = strWomb[3];
                    item.WOMB04 = strWomb[4];
                    item.WOMB05 = strWomb[5];
                    item.WOMB06 = strWomb[6];
                    item.WOMB07 = strWomb[7];
                    item.WOMB08 = strWomb[8];
                    item.WOMB12 = str중복자궁;
                    item.WOMAN12 = strWomb[9];          //위험구분
                    item.WOMB11 = strWomb[10];          //기타- 자궁내막세포출현
                    item.WOMB09 = str자궁암판정;
                    item.WOMB10 = txtJilEtcM.Text.Trim();
                    item.PANJENGDRNO9 = strPANJENGDRNO9.To<long>();
                    item.PANJENGDRNO10 = strPANJENGDRNO10.To<long>();
                }

                //폐암판정 저장======================================================================================================
                if (chkCancer5.Checked == true)
                {
                    sGbCancer6 = "Y";

                    str폐암판정 = VB.Left(cboLPan1.Text, 1);

                    item.LUNG_RESULT001 = txtResult7.Text.To<double>();
                    item.LUNG_RESULT002 = strLUNG002;
                    item.LUNG_RESULT003 = strLUNG003;
                    item.LUNG_RESULT004 = strLUNG004;

                    if (tabLung1.Visible == true)
                    {
                        sLung1 = "Y";

                        item.LUNG_RESULT005 = strLUNG005;
                        item.LUNG_RESULT006 = strLUNG006;
                        item.LUNG_RESULT007 = strLUNG007;
                        item.LUNG_RESULT008 = strLUNG008;
                        item.LUNG_RESULT009 = strLUNG009;
                        item.LUNG_RESULT010 = strLUNG010;
                        item.LUNG_RESULT011 = strLUNG011;
                        item.LUNG_RESULT012 = strLUNG012;
                    }

                    if (tabLung2.Visible == true)
                    {
                        sLung2 = "Y";

                        item.LUNG_RESULT013 = strLUNG013;
                        item.LUNG_RESULT014 = strLUNG014;
                        item.LUNG_RESULT015 = strLUNG015;
                        item.LUNG_RESULT016 = strLUNG016;
                        item.LUNG_RESULT017 = strLUNG017;
                        item.LUNG_RESULT018 = strLUNG018;
                        item.LUNG_RESULT019 = strLUNG019;
                        item.LUNG_RESULT020 = strLUNG020;
                    }

                    if (tabLung3.Visible == true)
                    {
                        sLung3 = "Y";

                        item.LUNG_RESULT021 = strLUNG021;
                        item.LUNG_RESULT022 = strLUNG022;
                        item.LUNG_RESULT023 = strLUNG023;
                        item.LUNG_RESULT024 = strLUNG024;
                        item.LUNG_RESULT025 = strLUNG025;
                        item.LUNG_RESULT026 = strLUNG026;
                        item.LUNG_RESULT027 = strLUNG027;
                        item.LUNG_RESULT028 = strLUNG028;
                    }

                    if (tabLung4.Visible == true)
                    {
                        sLung4 = "Y";

                        item.LUNG_RESULT029 = strLUNG029;
                        item.LUNG_RESULT030 = strLUNG030;
                        item.LUNG_RESULT031 = strLUNG031;
                        item.LUNG_RESULT032 = strLUNG032;
                        item.LUNG_RESULT033 = strLUNG033;
                        item.LUNG_RESULT034 = strLUNG034;
                        item.LUNG_RESULT035 = strLUNG035;
                        item.LUNG_RESULT036 = strLUNG036;
                    }

                    if (tabLung5.Visible == true)
                    {
                        sLung5 = "Y";

                        item.LUNG_RESULT037 = strLUNG037;
                        item.LUNG_RESULT038 = strLUNG038;
                        item.LUNG_RESULT039 = strLUNG039;
                        item.LUNG_RESULT040 = strLUNG040;
                        item.LUNG_RESULT041 = strLUNG041;
                        item.LUNG_RESULT042 = strLUNG042;
                        item.LUNG_RESULT043 = strLUNG043;
                        item.LUNG_RESULT044 = strLUNG044;
                    }

                    if (tabLung6.Visible == true)
                    {
                        sLung6 = "Y";

                        item.LUNG_RESULT045 = strLUNG045;
                        item.LUNG_RESULT046 = strLUNG046;
                        item.LUNG_RESULT047 = strLUNG047;
                        item.LUNG_RESULT048 = strLUNG048;
                        item.LUNG_RESULT049 = strLUNG049;
                        item.LUNG_RESULT050 = strLUNG050;
                        item.LUNG_RESULT051 = strLUNG051;
                        item.LUNG_RESULT052 = strLUNG052;
                    }

                    item.LUNG_RESULT053 = strLUNG053;
                    item.LUNG_RESULT054 = strLUNG054;
                    item.LUNG_RESULT055 = strLUNG055;
                    item.LUNG_RESULT056 = strLUNG056;
                    item.LUNG_RESULT057 = strLUNG057;
                    item.LUNG_RESULT058 = strLUNG058;
                    item.LUNG_RESULT059 = strLUNG059;
                    item.LUNG_RESULT060 = strLUNG060;
                    item.LUNG_RESULT061 = strLUNG061;
                    item.LUNG_RESULT062 = strLUNG062;
                    item.LUNG_RESULT063 = strLUNG063;
                    item.LUNG_RESULT064 = strLUNG064;
                    item.LUNG_RESULT065 = strLUNG065;
                    item.LUNG_RESULT066 = strLUNG066;
                    item.LUNG_RESULT067 = strLUNG067;
                    item.LUNG_RESULT068 = strLUNG068;
                    item.LUNG_RESULT069 = strLUNG069;
                    item.LUNG_RESULT070 = strLUNG070;
                    item.LUNG_RESULT071 = strLUNG071;
                    item.LUNG_RESULT072 = strLUNG072;
                    item.LUNG_RESULT073 = strLUNG073;
                    item.LUNG_RESULT074 = strLUNG074;
                    item.LUNG_RESULT075 = strLUNG075;
                    item.LUNG_RESULT076 = strLUNG076;
                    item.LUNG_RESULT077 = strLUNG077;
                    item.LUNG_RESULT078 = strLUNG078;

                    if (strLUNGOK.IsNullOrEmpty())
                    {
                        item.LUNG_RESULT079 = strLUNG079;
                    }
                    item.LUNG_RESULT080 = strLUNG080;
                    item.PANJENGDRNO11 = strPANJENGDRNO11.To<long>();
                }

                //판정소견
                item.S_SOGEN = strNewSogen[1];
                item.S_SOGEN2 = strNewSogen[2];
                item.C_SOGEN = strNewSogen[3];
                item.C_SOGEN2 = strNewSogen[4];
                item.C_SOGEN3 = strNewSogen[5];
                item.L_SOGEN = strNewSogen[6];
                item.B_SOGEN = strNewSogen[7];
                item.W_SOGEN = strNewSogen[8];

                //판정일자 / 판정의사면허번호
                for (int i = 1; i <= 6; i++)
                {
                    if (tab1.Enabled == true && i == 1)
                    {
                        if (!str위암판정.IsNullOrEmpty() && (!strNewSogen[1].IsNullOrEmpty() || !strNewSogen[2].IsNullOrEmpty()))
                        {
                            sPanjengDate1 = "Y";

                            //item.S_PANJENGDATE = dtpSPanDate.Text;
                            item.S_PANJENGDATE = dtpSPanDate.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN32 = strPanDrNo[0];
                            //접수마스타에 판정일자 업데이트
                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpSPanDate.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpSPanDate.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpSPanDate.Text);

                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpSPanDate.Text;
                                }
                            }
                        }
                    }
                    else if (tab2.Enabled == true && i == 2)
                    {
                        if (!str대장암판정.IsNullOrEmpty() && !strNewSogen[3].IsNullOrEmpty() || !strNewSogen[4].IsNullOrEmpty() || !strNewSogen[5].IsNullOrEmpty())
                        {
                            sPanjengDate2 = "Y";

                            //item.C_PANJENGDATE = dtpCPanDate.Text;
                            item.C_PANJENGDATE = dtpCPanDate.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN33 = strPanDrNo[1];
                            //접수마스타에 판정일자 업데이트

                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpCPanDate.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpCPanDate.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpCPanDate.Text);



                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpCPanDate.Text;
                                }
                            }
                        }
                    }
                    else if (tab3.Enabled == true && i == 3)
                    {
                        if (!str간암판정.IsNullOrEmpty() && !strNewSogen[6].IsNullOrEmpty())
                        {
                            sPanjengDate3 = "Y";

                            //item.L_PANJENGDATE = dtpLPanDate.Text;
                            item.L_PANJENGDATE = dtpLPanDate.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN34 = strPanDrNo[2];
                            //접수마스타에 판정일자 업데이트

                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpLPanDate.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpLPanDate.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpLPanDate.Text);

                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpLPanDate.Text;
                                }
                            }
                        }
                    }
                    else if (tab4.Enabled == true && i == 4)
                    {
                        if (!str유방암판정.IsNullOrEmpty() && !strNewSogen[7].IsNullOrEmpty())
                        {
                            sPanjengDate4 = "Y";

                            //item.B_PANJENGDATE = dtpBPanDate.Text;
                            item.B_PANJENGDATE = dtpBPanDate.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN35 = strPanDrNo[3];
                            //접수마스타에 판정일자 업데이트

                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpBPanDate.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpBPanDate.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpBPanDate.Text);

                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpBPanDate.Text;
                                }
                            }
                        }
                    }
                    else if (tab5.Enabled == true && i == 5)
                    {
                        if (!str자궁암판정.IsNullOrEmpty() && !strNewSogen[8].IsNullOrEmpty())
                        {
                            sPanjengDate5 = "Y";

                            //item.W_PANJENGDATE = dtpWPanDate.Text;
                            item.W_PANJENGDATE = dtpWPanDate.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN36 = strPanDrNo[4];
                            //접수마스타에 판정일자 업데이트

                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpWPanDate.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpWPanDate.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpWPanDate.Text);

                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpWPanDate.Text;
                                }
                            }
                        }
                    }
                    else if (tab6.Enabled == true && i == 6)
                    {
                        if (!str폐암판정.IsNullOrEmpty())
                        {
                            sPanjengDate6 = "Y";

                            //item.L_PANJENGDATE1 = dtpLPanDate1.Text;
                            item.L_PANJENGDATE1 = dtpLPanDate1.Value.ToString("yyyy-MM-dd");
                            item.NEW_WOMAN37 = strPanDrNo[5];
                            //접수마스타에 판정일자 업데이트

                            if (strPanDate.IsNullOrEmpty())
                            {
                                strPanDate = dtpLPanDate1.Text;
                            }
                            else
                            {
                                DateTime DATE1 = Convert.ToDateTime(strPanDate);
                                DateTime DATE2 = dtpLPanDate1.Value;
                                //DateTime DATE2 = Convert.ToDateTime(dtpLPanDate.Text);

                                if (DATE1 < DATE2)
                                {
                                    strPanDate = dtpLPanDate1.Text;
                                }
                            }
                        }
                    }
                }

                //검진실시장소 2.내원 으로 일괄세팅
                item.S_PLACE = "2";
                item.C_PLACE = "2";
                item.LIVER_PLACE = "2";
                item.B_PLACE = "2";
                item.WOMB_PLACE = "2";
                item.LUNG_PLACE = "2";
                item.GBSTOMACH = chkCancer0.Checked == true ? "1" : "0";
                item.GBLIVER = chkCancer2.Checked == true ? "1" : "0";
                item.GBRECTUM = chkCancer1.Checked == true ? "1" : "0";
                item.GBBREAST = chkCancer3.Checked == true ? "1" : "0";
                item.GBWOMB = chkCancer4.Checked == true ? "1" : "0";
                item.GBLUNG = chkCancer5.Checked == true ? "1" : "0";
                item.NEW_WOMAN03 = strSangDam;
                item.TONGBOGBN = "2";
                item.CAN_MIRGBN = str청구제외;
                item.GUNDATE = FstrJepDate;
                item.RID = strROWID;

                COMHPC item2 = new COMHPC();

                //============= item2 초기화 Start ==============
                item2.GBCANCER1 = "";
                item2.GBCANCER2 = "";
                item2.GBCANCER3 = "";
                item2.GBCANCER4 = "";
                item2.GBCANCER5 = "";
                item2.GBCANCER6 = "";
                item2.STOMACH1 = "";
                item2.STOMACH2 = "";
                item2.COLON1 = "";
                item2.COLON2 = "";
                item2.COLON3 = "";
                item2.LUNG1 = "";
                item2.LUNG2 = "";
                item2.LUNG3 = "";
                item2.LUNG4 = "";
                item2.LUNG5 = "";
                item2.LUNG6 = "";
                item2.LUNGOK = "";
                item2.PANJENGDATE1 = "";
                item2.PANJENGDATE2 = "";
                item2.PANJENGDATE3 = "";
                item2.PANJENGDATE4 = "";
                item2.PANJENGDATE5 = "";
                item2.PANJENGDATE6 = "";
                item.S_ANATGBN = "";
                item2.CANCERKIND1 = "";
                item2.CANCERKIND2 = "";
                item2.CANCERKIND3 = "";
                item2.CANCERKIND4 = "";
                item2.CANCERKIND5 = "";
                item2.CANCERKIND6 = "";
                //============= item2 초기화 End ==============

                item2.GBCANCER1 = sGbCancer1;
                item2.GBCANCER2 = sGbCancer2;
                item2.GBCANCER3 = sGbCancer3;
                item2.GBCANCER4 = sGbCancer4;
                item2.GBCANCER5 = sGbCancer5;
                item2.GBCANCER6 = sGbCancer6;

                item2.LIVER1 = sLiver1;
                item2.LIVER2 = sLiver2;
                item2.LIVER3 = sLiver3;

                item2.STOMACH1 = sStomach1;
                item2.STOMACH2 = sStomach2;

                item2.COLON1 = sColon1;
                item2.COLON2 = sColon2;
                item2.COLON3 = sColon3;

                item2.LUNG1 = sLung1;
                item2.LUNG2 = sLung2;
                item2.LUNG3 = sLung3;
                item2.LUNG4 = sLung4;
                item2.LUNG5 = sLung5;
                item2.LUNG6 = sLung6;

                item2.LUNGOK = strLUNGOK;

                item2.PANJENGDATE1 = sPanjengDate1;
                item2.PANJENGDATE2 = sPanjengDate2;
                item2.PANJENGDATE3 = sPanjengDate3;
                item2.PANJENGDATE4 = sPanjengDate4;
                item2.PANJENGDATE5 = sPanjengDate5;
                item2.PANJENGDATE6 = sPanjengDate6;

                //위조직검사가 있고 조직검사필요없음이면 강제로 "조직검사필요"로 변경
                if (strJinS2.To<long>() > 1 && strGbAnat == "2") { strGbAnat = "1"; }

                item.S_ANATGBN = strGbAnat;

                if (tab1.Enabled == true) { item2.CANCERKIND1 = "Y"; }
                if (tab2.Enabled == true) { item2.CANCERKIND2 = "Y"; }
                if (tab3.Enabled == true) { item2.CANCERKIND3 = "Y"; }
                if (tab4.Enabled == true) { item2.CANCERKIND4 = "Y"; }
                if (tab5.Enabled == true) { item2.CANCERKIND5 = "Y"; }
                if (tab6.Enabled == true) { item2.CANCERKIND6 = "Y"; }

                result = hicCancerNewService.UpdateCancerPanjengbyRowId(item, item2);

                if (result < 0)
                {
                    MessageBox.Show("암판정 저장중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                result = hicJepsuService.UpdateGbMunjin1byWrtNo(FnWrtNo);

                if (result < 0)
                {
                    MessageBox.Show("접수마스타에 문진완료 등록중 오류가 발생함!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //판정완료여부 점검
                strTemp = "Y";

                for (int i = 0; i <= 5; i++)
                {
                    if (i == 0)
                    {
                        if (tab1.Enabled == true)
                        {
                            if (strNewSogen[1].IsNullOrEmpty() && strNewSogen[2].IsNullOrEmpty()) { strTemp = ""; }
                            if (str위암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }
                    else if (i == 1)
                    {
                        if (tab2.Enabled == true)
                        {
                            if (strNewSogen[3].IsNullOrEmpty() && strNewSogen[3].IsNullOrEmpty() && strNewSogen[5].IsNullOrEmpty()) { strTemp = ""; }
                            if (str대장암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }
                    else if (i == 2)
                    {
                        if (tab3.Enabled == true)
                        {
                            if (strNewSogen[6].IsNullOrEmpty()) { strTemp = ""; }
                            if (str간암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }
                    else if (i == 3)
                    {
                        if (tab4.Enabled == true)
                        {
                            if (strNewSogen[7].IsNullOrEmpty()) { strTemp = ""; }
                            if (str유방암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }
                    else if (i == 4)
                    {
                        if (tab5.Enabled == true)
                        {
                            if (strNewSogen[8].IsNullOrEmpty()) { strTemp = ""; }
                            if (str자궁암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }
                    else if (i == 5)
                    {
                        if (tab6.Enabled == true)
                        {
                            if (str폐암판정.IsNullOrEmpty()) { strTemp = ""; }
                        }
                    }

                    if (strTemp.IsNullOrEmpty()) { break; }
                }

                result = hicCancerNewService.UpdatePanjengYNbyRowId(strTemp, strROWID);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("판정완료 여부(Y/N) Data등록 중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //접수마스타에 판정완료 SET
                result = hicJepsuService.UpdatePanjengDateDrNobyWrtNo(strPanjengDrNo, strPanDate, FnWrtNo, strTemp);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show(" 접수마스타에 판정완료 등록 중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                #region 아래 연동부분은 실시간 결과입력 반영으로 미리 세팅됨으로 구현하지 않음
                //위암 위내시경 검사결과 연동
                //if (tab1.Enabled == true && tabStomach2.Visible == true)
                //{
                //    eInputStomachRes(null, null);
                //}

                ////간암판정내용 검사결과연동
                //if (tab3.Enabled == true)
                //{
                //    eInputLiverRes(null, null);
                //}

                ////유방판정내용 검사결과연동
                //if (tab4.Enabled == true)
                //{
                //    eInputBreastRes(null, null);
                //}

                ////폐암판정내용 검사결과연동
                //if (tab6.Enabled == true)
                //{
                //    eInputLungRes(null, null);
                //}
                #endregion

                //판정저장시 결과변경사항 저장
                fn_Result_Save();
                fn_Screen_Clear();

            }
            else if (sender == btnLungHELP)
            {
                pnlPanLungHelp.Location = new Point(1, 0);
                pnlPanLungHelp.BringToFront();
                pnlPanLungHelp.Visible = true;
            }
            else if (sender == btnSearch)
            {
                Display_ssList();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnPacs)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnEMR)
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);
            }
            else if(sender == btnResultSave)
            {
                fn_Result_Save();
            }
            else if (sender == btnExamResultReg)
            {
                frmHcPanExamResultRegChg f = new frmHcPanExamResultRegChg(FnWrtNo, "Y", "Y");           //검사결과
                f.ShowDialog();
            }
        }

        private void Display_ssList()
        {
            long nLicense = clsHcVariable.GnHicLicense;

            List<string> strJobSabun = new List<string>();
            string strFDate = dtpFrDate.Text;
            string strTDate = dtpToDate.Text;
            string strSName = txtSName.Text.Trim();
            string strJob = rdoJob1.Checked ? "NEW" : rdoJob2.Checked ? "OLD" : "";
            string strSort = rdoSort1.Checked ? "1" : rdoSort2.Checked ? "2" : "3";
            string strHea = "";
            panList01_Control_Set("1");

            sp.Spread_Clear_Simple(ssList);
            ssList.ActiveSheet.RowCount = 0;

            strJobSabun.Add("32158");   //이주령
            strJobSabun.Add("39444");   //주철효
            strJobSabun.Add("54075");   //이지은
            if (clsType.User.IdNumber =="32158" || clsType.User.IdNumber == "39444" || clsType.User.IdNumber == "54075") { strHea = "Y"; }
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                List<HIC_JEPSU> lstCan = hicJepsuService.GetCancerListByDate(strFDate, strTDate, strJob, strSort, nLicense, strJobSabun, strHea, strSName);

                ssList.DataSource = lstCan;

                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// menuResSave_Click() //결과 변경 내용 저장
        /// </summary>
        void fn_Result_Save()
        {
            string strCODE = "";
            string strResult = "";
            string strPanjeng = "";
            string strResCode = "";
            string strChange = "";
            string strNewPan = "";
            string strROWID = "";
            int result = 0;

            if (FnWrtNo == 0) { return; }

            if (SS2.ActiveSheet.RowCount == 0)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Value.Trim();
                strPanjeng = SS2.ActiveSheet.Cells[i, 4].Text.Trim();
                strResCode = SS2.ActiveSheet.Cells[i, 7].Text.Trim();
                strChange = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();
                strNewPan = hm.ExCode_Result_Panjeng(strCODE, strResult, FstrSex, FstrJepDate, VB.Left(FstrJepDate, 4));

                if (!strResCode.IsNullOrEmpty())
                {
                    strResult = VB.Pstr(strResult, ".", 1);
                }
                
                //결과입력란 공백시 . 으로 자동세팅 처리
                if (strResult.IsNullOrEmpty() && strResCode.IsNullOrEmpty())
                {
                    strResult = ".";
                    strChange = "Y";
                }

                //if (strChange == "Y" && (!strResult.IsNullOrEmpty() && strResCode.IsNullOrEmpty() && clsHcVariable.GnHicLicense > 0))
                if (strChange == "Y" && (!strResult.IsNullOrEmpty() && clsHcVariable.GnHicLicense > 0))
                {
                    //결과를 저장
                    result = hicResultService.UpdateResultPanjengResCodeActivebyRowId(strResult, strNewPan, strResCode, clsType.User.IdNumber, clsPublic.GstrSysDate + ' ' + clsPublic.GstrSysTime, strROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(i + 1 + "번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            //접수마스타의 상태를 변경
            hm.Result_EntryEnd_Check(FnWrtNo);

            clsDB.setCommitTran(clsDB.DbCon);

            fn_Exam_Result_Display(FnWrtNo, FstrJepDate, FstrPtno, FstrSex);
            fn_OLD_Result_Display(FstrSex);
        }

        void Common_Value(string sItem)
        {
            FItem = sItem;
        }

        void fn_Sogen_Common_Disp(TextBox argtxtBox, string argGubun, string argExam)
        {
            FItem = "";

            FrmHcAmCommonSetting = new frmHcAmCommonSetting(argGubun, argExam);
            FrmHcAmCommonSetting.rSetGstrValue += new frmHcAmCommonSetting.SetGstrValue(Common_Value);
            FrmHcAmCommonSetting.ShowDialog();
            FrmHcAmCommonSetting.rSetGstrValue -= new frmHcAmCommonSetting.SetGstrValue(Common_Value);

            if (argGubun == "61")
            {
                string[] strLungArr = null;

                if (!FItem.IsNullOrEmpty())
                {
                    string[] separator = new string[1]{"\r\n"};
                    strLungArr = FItem.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    if (strLungArr.Length > 0)
                    {
                        for (int i = 0; i < strLungArr.Length; i++)
                        {
                            if (VB.Pstr(strLungArr[i], "{}", 2).To<int>(0) < 7)
                            {
                                txtL_Sogen1.Text += VB.Pstr(strLungArr[i], "{}", 1).Trim() + " ";
                            }
                            else
                            {
                                txtL_Sogen2.Text += VB.Pstr(strLungArr[i], "{}", 1).Trim() + " ";
                            }
                        }
                    }
                }
            }
            else
            {
                argtxtBox.Text = VB.Replace(FItem, "\r\n", " ");
            }
        }

        void fn_FunctionKey_Enabled()
        {
            btnF1.Visible = true;
            btnF2.Visible = true;
            btnF3.Visible = true;
            btnF4.Visible = true;
            btnF5.Visible = true;
            btnF6.Visible = true;
            btnF7.Visible = true;
        }

        private void rResultChange(string sCode, string sValue)
        {
            if (!sCode.IsNullOrEmpty())
            {
                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == sCode)
                    {
                        FarPoint.Win.Spread.CellType.ICellType cmbocell = SS2.ActiveSheet.GetCellType(i, 2);

                        if (cmbocell is FarPoint.Win.Spread.CellType.ComboBoxCellType)
                        {
                            clsSpread.gSdCboItemFindLeft(SS2, i, 2, 2, sValue);
                            SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i, 2].Text = sValue;
                            SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                        }

                        Application.DoEvents();
                    }
                }
            }
        }

        void fn_Exam_Result_Display(long argWrtNo, string argJepDate, string argPtNo, string argSex)
        {
            long nRead = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExPan = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            List<string> strInExCode = new List<string>();    //위수면내시경
            string[] strList;
            string strResName = "";
            string strRemark = "";

            superTabControl2.SelectedTab = rTab_Info;
            panList01_Control_Set("2");

            List<HIC_RESULT_EXCODE> list = new List<HIC_RESULT_EXCODE>();

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전결과1";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이전결과2";

            strInExCode.Clear();
            strInExCode.Add("ZE47");
            strInExCode.Add("TX20");

            sp.Spread_All_Clear(SS2);
            Application.DoEvents();

            list = hicResultExCodeService.GetItemCounselCanbyWrtNo(argWrtNo);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < nRead; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                strResCode = list[i].RESCODE;
                strResultType = list[i].RESULTTYPE;
                strGbCodeUse = list[i].GBCODEUSE;

                SS2.ActiveSheet.Cells[i, 0].Text = list[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].HNAME;
                if (!strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult.Replace("\n", "");
                }

                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "065";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        if (strResName.IsNullOrEmpty())
                        {
                            strResName = strResult;
                        }

                        if (!strResName.IsNullOrEmpty())
                        {
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                }
                else if (!strResult.IsNullOrEmpty())
                {
                    if (strResult.Length > 7)
                    {
                        strRemark += "▷" + list[i].HNAME + ":";
                        strRemark += strResName + "\r\n";
                    }
                }

                if (list[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "*";
                }

                //Combo_set
                //자료를 READ
                List<HIC_RESCODE> list2 = hicRescodeService.GetCodeNamebyBindGubun(strResCode);
                int nCnt = list2.Count;

                //Array.Resize(ref strList, nCnt);
                strList = new string[nCnt];

                //for (int k = 0; k < list2.Count; k++) { strList[k] = ""; }

                if (list2.Count > 0)
                {
                    FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                    combo.Clear();

                    for (int j = 0; j < list2.Count; j++)
                    {
                        strList[j] = list2[j].CODE + "." + list2[j].NAME;
                    }

                    SS2.ActiveSheet.Cells[i, 2].Text = "";
                    combo.Items = strList;
                    //combo.ItemData = strList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    combo.MaxDrop = list2.Count;
                    combo.MaxLength = 100;
                    combo.ListWidth = 200;
                    combo.Editable = false;
                    SS2.ActiveSheet.Cells[i, 2].CellType = combo;
                    SS2.ActiveSheet.Cells[i, 2].Locked = false;
                    //SS2.ActiveSheet.Cells[i, 2].Text = "";

                    if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                    {
                        HIC_RESCODE list3 = hicRescodeService.GetCodeNamebyGubunCode(strResCode, strResult);

                        if (!list3.IsNullOrEmpty())
                        {
                            for (int k = 0; k < combo.Items.Length; k++)
                            {
                                combo.ListOffset = k;
                                if (combo.Items[k].ToString() == (list3.CODE.Trim() + "." + list3.NAME.Trim()).ToString())
                                {
                                    SS2.ActiveSheet.Cells[i, 2].Text = combo.Items[k].ToString();
                                    //break;
                                }
                            }
                        }
                    }
                }

                if (strResCode.IsNullOrEmpty() || strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                }

                Application.DoEvents();

                SS2.ActiveSheet.Cells[i, 2].ForeColor = Color.FromArgb(0, 0, 0);

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 5].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                strExPan = list[i].PANJENG;

                strNomal = hm.EXAM_NomalValue_SET(strExCode, argJepDate, argSex, list[i].MIN_M, list[i].MAX_M, list[i].MIN_F, list[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 5].Text = strExPan;
                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;

                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;

                if (list[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";

                    string strResult1 = strResult;
                }

                if (list[i].EXCODE.Trim() == "TH01" || list[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }

                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list[i].RESULTTYPE;

                strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, argJepDate, "");

                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222); //정상B
                        break;
                    case "C":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));  //주의C
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170); //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220); //정상A 또는 기타
                        break;
                }

                if (list[i].GROUPCODE != "1160" && (strExCode == "A123" || strExCode == "A242" || strExCode == "A241" || strExCode == "C404"))
                {
                    SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222); //정상
                }
            }

        }

        void fn_OLD_Result_Display(string argSex)
        {
            int nOldCNT = 0;
            List<string> strExamCode = new List<string>();
            string strAllWRTNO = "";
            string strJepDate = "";

            string[] strGjJong = { "31", "35" };

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전결과1";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이전결과2";

            List<HIC_JEPSU> list = new List<HIC_JEPSU>();

            //검사항목을 Setting
            strExamCode.Clear();
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (!SS2.ActiveSheet.Cells[i, 0].Text.Trim().IsNullOrEmpty())
                {
                    strExamCode.Add(SS2.ActiveSheet.Cells[i, 0].Text.Trim());
                }
            }

            string strPtNo = FstrPtno;

            //1차검사 종전 접수번호를 읽음
            //List<HIC_JEPSU> list = hicJepsuService.GetWrtNoJepDatebyPanoJepDateGjJong(FnPano, FstrJepDate, strGjJong);
            list = hicJepsuService.GetWrtNoJepDatePanjeng(FnPano, FstrJepDate, "CAN");

            nOldCNT = list.Count;
            strAllWRTNO = "";
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE;
                if (SS2.ActiveSheet.ColumnCount < i + 11)
                {
                    SS2.ActiveSheet.ColumnCount += 1;
                }
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 11).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                if (strExamCode.Count > 0)
                {
                    fn_OLD_Result_Display_SUB(i, list[i].WRTNO, strExamCode, argSex);
                }
                if (i >= 1)
                {
                    break;
                }
            }
        }

        void fn_OLD_Result_Display_SUB(int argCol, long argWrtNo, List<string> argExamCode, string argSex)
        {
            int nREAD = 0;
            int nRow = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strResName = "";
            string strRemark = "";
            string strSearch = "";
            string strGbUse = "";


            List<HIC_RESULT_EXCODE> list = new List<HIC_RESULT_EXCODE>();

            //검사항목 및 결과를 READ
            list = hicResultExCodeService.GetItembyWrtNoInExCodes(argWrtNo, argExamCode, "HIC");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE.Trim();          //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부
                strGbUse = list[i].GBUSE;

                //해당검사코드가 있는 Row를 찾음
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[j, 0].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        strSearch = "OK";
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                //if (nRow > 0)
                if (strSearch == "OK")
                {
                    SS2.ActiveSheet.Cells[nRow, argCol + 11].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, argCol + 11].Text = strResName;
                            if (!strResName.IsNullOrEmpty())
                            {
                                if (strResName.Length > 7)
                                {
                                    strRemark += "▷" + list[i].HNAME + ": ";
                                    strRemark += strResName + "\r\n";
                                }
                            }
                        }
                    }
                    else if (!strResult.IsNullOrEmpty())
                    {
                        if (strResult.Length > 7)
                        {
                            strRemark += "▷" + list[i].HNAME + ":";
                            strRemark += strResult + "\r\n";
                        }
                    }


                }
            }
        }
    }
}
