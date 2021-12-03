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
/// File Name       : frmPanjeng_Hic.cs
/// Description     : 일반검진 판정
/// Author          : 이상훈
/// Create Date     : 2020-05-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm1차일반특수판정_2019.frm(Frm1차일반특수판정_2019) " />

namespace HC_Pan
{
    public partial class frmPanjeng_Hic_Old : Form
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
        HicResultService hicResultService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicResBohum1JepsuService hicResBohum1JepsuService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicTitemService hicTitemService = null;
        EtcJupmstService etcJupmstService = null;
        HicMirBohumService hicMirBohumService = null;

        frmHcPanPanDrExamResultInput FrmHcPanPanDrExamResultInput = null;
        frmHcPanXrayResultInput FrmHcPanXrayResultInput = null;
        frmHcCodeHelp FrmHcCodeHelp = null;
        frmHcPanPanjengHelp FrmHcPanPanjengHelp = null;
        frmHcPanJochiHelp FrmHcPanJochiHelp = null;
        frmHcPanSpcSahuCode FrmHcPanSpcSahuCode = null;
        frmHcPanMunjin_2019 FrmHcPanMunjin_2019 = null;

        HIC_CODE CodeHelpItem = null;

        public delegate void SaveEventClosed(string sRtn);
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
        string FstrGjYear;

        string FstrJob;

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

        int FnA118;                     //'하지기능(0.정상 1.정상B 2,의심R)
        int FnA119;                     //'보행장애(1.무 2.유 3.검사불가)
        int FnA120;                     //'평형성의심(0.정상 1.정상B 2.의심R)

        List<string> FstrNotAddPanList = new List<string>();       //2015-09-01일부 추가판정 제외 그룹코드 목록
        bool FbAutoPanjeng;             //자동판정 저장 여부(True/False)
        string[] FstrHabit = new string[18];

        List<string> FstrKind = new List<string>();

        List<HC_PANJENG_PATLIST> FPatListItem;

        string FstrGubun;

        string FstrGbHea;   //검진당일 종검수검 여부
        string FstrSName;
        string FstrLtdCode;

        string strSExam;

        public frmPanjeng_Hic_Old()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmPanjeng_Hic_Old(long nWrtNo, string strSName, string strJob, string strLtdCode, string strGbHea)
        {
            InitializeComponent();

            FstrSName = strSName;
            FnWrtNo = nWrtNo;
            FstrJob = strJob;
            FstrLtdCode = strLtdCode;
            FstrGbHea = strGbHea;
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
            hicResultService = new HicResultService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicResBohum1JepsuService = new HicResBohum1JepsuService();
            hicSangdamNewService = new HicSangdamNewService();
            hicSunapdtlService = new HicSunapdtlService();
            hicTitemService = new HicTitemService();
            etcJupmstService = new EtcJupmstService();
            hicMirBohumService = new HicMirBohumService();
            
            this.Load += new EventHandler(eFormLoad);
            this.btnD1.Click += new EventHandler(eBtnClick);
            this.btnD2.Click += new EventHandler(eBtnClick);
            this.btnXrayResult.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnMunjin.Click += new EventHandler(eBtnClick);
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

        void eFormLoad(object sender, EventArgs e)
        {
            string strOK = "";
            long nMirNo = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_ComboSet();

            //생애 추가판정 제외 그룹코드 목록
            List<HIC_BCODE> list = hicBcodeService.GetCodebyGubun("HIC_추가판정제외코드");

            FstrNotAddPanList.Clear();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    FstrNotAddPanList.Add(list[i].CODE);
                }
            }

            //작업자 사번으로 저장 버튼 설정
            strOK = "OK";
            //판독의사가 아니면 저장버튼 금지
            if (clsHcVariable.GnHicLicense == 0) strOK = "";
            //2차 접수를 하였으면 저장버튼 금지
            if (!FstrJepDate2.IsNullOrEmpty()) strOK = "";
            //청구를 하였으면 저장버튼 금지
            if (strOK == "OK")
            {
                HIC_JEPSU list2 = hicJepsuService.GetMirNo1byWrtNo(FnWrtNo);

                if (!list2.IsNullOrEmpty())
                {
                    nMirNo = list2.MIRNO1;
                    if (nMirNo > 0)
                    {
                        if (hicMirBohumService.GetJepNobyMirNo(nMirNo) != "")
                        {
                            strOK = "";
                        }
                    }
                }
            }

            //재판정이 등록되었거나 관리자이면 등록버튼 무조건 활성화
            if (strOK == "")
            {
                if (hm.RePanjeng_WRTNO_Check(FnWrtNo) == true)
                {
                    strOK = "OK";
                }
                else if (clsHcVariable.GbHicAdminSabun == true)
                {
                    strOK = "OK";
                }
            }

            if (strOK == "OK")
            {
                txtPanDrNo.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                txtPanDrNo.Enabled = true;
                btnSave.Enabled = true;
            }

            fn_Screen_Clear();

            if (FnWrtNo > 0)
            {
                fn_Screen_Display();
            }
            else
            {
                if (FstrJob == "1" && clsHcVariable.GnHicLicense > 0)
                {
                    fn_Bohum_AutoPanjeng("[자동]");
                }
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

        void fn_ComboSet()
        {
            //판정B 기타질환관리세부
            cboPanBEtc.Enabled = true;
            cboPanBEtc.Items.Clear();
            cboPanBEtc.Items.Add(" ");
            cboPanBEtc.Items.Add("1.혈색소과다");
            cboPanBEtc.Items.Add("2.저체중");
            cboPanBEtc.Items.Add("3.시력저하");
            cboPanBEtc.Items.Add("4.기타");
            cboPanBEtc.SelectedIndex = 0;
            cboPanBEtc.Enabled = false;

            //우울증,인지기능,노인신체기능평가
            cboDepression.Items.Clear();
            cboDepression.Items.Add(" ");
            cboDepression.Items.Add("1.우울증상 없음(0-4점)");
            cboDepression.Items.Add("2.가벼운 우울증상(5-9점)");
            cboDepression.Items.Add("3.중간정도 우울증 의심(10-19점)");
            cboDepression.Items.Add("4.심한 우울증 의심(20-27점)");
            cboDepression.Items.Add("5.★자살생각");
            cboDepression.SelectedIndex = 0;

            cboCognitiveFunction.Items.Clear();
            cboCognitiveFunction.Items.Add(" ");
            cboCognitiveFunction.Items.Add("1.특이소견 없음(0-5점)");
            cboCognitiveFunction.Items.Add("2.인지기능저하의심(6점이상)");

            cboSenileBodyFunction.Items.Clear();
            cboSenileBodyFunction.Items.Add(" ");
            cboSenileBodyFunction.Items.Add("1.정상");
            cboSenileBodyFunction.Items.Add("2.신체기능 저하");

            cboLiver.Items.Clear();
            cboLiver.Items.Add(" ");
            cboLiver.Items.Add("1.항체 있음");
            cboLiver.Items.Add("2.항체 없음");
            cboLiver.Items.Add("3.B형간염보유자의심");
            cboLiver.Items.Add("4.판정보류");
            cboLiver.SelectedIndex = 0;

            fn_ComboSet(cboUpmu, "13");             //업무적합성

            ComboSahu_Set(cboSahu1);
            ComboSahu_Set(cboSahu2);
            ComboSahu_Set(cboSahu3);
        }

        void fn_Auto_Panjeng_Display()
        {
            int nRead = 0;
            string strRowId = "";
            string strSogen = "";    //의심질환 소견
            string strSogenB = "";  //유질환 소견
            string strSogenC = "";  //생활습관 소견
            string strSogenD = "";  //기타소견
            string strWSogen = "";
            string strXSogen = "";
            string strFlag = "";
            string strTemp = "";

            strSogen = clsHcType.TFA.Sogen;
            strSogenB = clsHcType.TFA.SogenB;
            strSogenC = clsHcType.TFA.SogenC;
            strSogenD = clsHcType.TFA.SogenD;

            clsHcType.TFA.Sogen = "";
            clsHcType.TFA.SogenB = "";
            clsHcType.TFA.SogenC = "";
            clsHcType.TFA.SogenD = "";

            strFlag = "";
            //판정결과
            chkPanjengA.Checked = false;
            if (clsHcType.TFA.Panjeng == "1")
            {
                chkPanjengA.Checked = true;
            }
            else
            {
                chkPanjengA.Checked = false;
            }

            //판정(정상B)
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkPanjengB = (Controls.Find("chkPanjengB" + (i).To<string>(), true)[0] as CheckBox);
                if (clsHcType.TFA.PanB[i] == true)
                {
                    chkPanjengB.Checked = true;
                }
                else
                {
                    chkPanjengB.Checked = false;
                }
            }

            //판정(질환의심)
            for (int i = 0; i <= 11; i++)
            {
                if (i < 4)
                {
                    CheckBox chkPanjengU = (Controls.Find("chkPanjengU" + (i).To<string>(), true)[0] as CheckBox);
                    CheckBox chkPanjengR = (Controls.Find("chkPanjengR" + (i).To<string>(), true)[0] as CheckBox);

                    chkPanjengU.Checked = false;
                    if (clsHcType.TFA.PanU[i] == true)
                    {
                        chkPanjengU.Checked = true;
                    }
                    else
                    {
                        chkPanjengU.Checked = false;
                    }
                    if (clsHcType.TFA.PanR[i] == true)
                    {
                        chkPanjengR.Checked = true;
                        chkPanjengA.Checked = false;
                    }
                    else
                    {
                        chkPanjengR.Checked = false;
                    }
                }
            }

            if (chkPanjengR7.Checked == true)
            {
                clsHcType.TFA.PanR[7] = true;
                strFlag = "OK";
            }
            else
            {
                clsHcType.TFA.PanR[7] = false;
            }

            //간염
            cboLiver.SelectedIndex = 0;
            if (clsHcType.TFA.Liver > 0)
            {
                cboLiver.SelectedIndex = clsHcType.TFA.Liver;
            }

            dtpPanDate.Text = clsPublic.GstrSysDate;
            txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
            //상담시 생활습관개선
            HIC_SANGDAM_NEW list = hicSangdamNewService.GetItembyWrtNo(FnWrtNo);

            if (!list.IsNullOrEmpty())
            {
                if (list.HABIT1 == "1")
                {
                    strTemp += "절주필요,";
                }
                if (list.HABIT2 == "1")
                {
                    strTemp += "금연필요,";
                }
                if (list.HABIT3 == "1")
                {
                    strTemp += "신체활동필요,";
                }
                if (list.HABIT4 == "1")
                {
                    strTemp += "근력운동필요,";
                }
            }

            if (!strTemp.IsNullOrEmpty())
            {
                strSogenC += "생활습관개선(" + VB.Left(strTemp, strTemp.Length - 1) + ")/";
            }

            //정상B(적극적인 관리)
            if (string.Compare(clsHcType.TFA.Panjeng, "1") > 0)
            {
                strSogenB = clsHcType.TFA.SogenB;
                if (clsHcType.TFA.PanB[0] == true) { strSogenD += "비만관리:체중조절요/ "; }
                //콜레스테롤관리
                if (clsHcType.TFA.PanB[2] == true) { strSogenD += "이상지질혈증관리:식이요법,운동,추적검사/ "; }
                //간기능
                if (clsHcType.TFA.PanB[3] == true) { strSogenD += "간기능관리:금주,체중관리,추적검사/ "; }
                //신장기능
                if (clsHcType.TFA.PanB[5] == true) { strSogenD += "신장기능:추적변화관찰/ "; }
                //빈혈관리
                if (clsHcType.TFA.PanB[6] == true) { strSogenD += "빈혈관리:추적재검사/ "; }
                //골다공증
                if (clsHcType.TFA.PanB[7] == true) { strSogenD += "골다공증관리:추적검사/ "; }
                //부인과질환 및 방사선소견
                if (strWSogen.IsNullOrEmpty()) strSogenD += strWSogen + "/";
                if (strXSogen.IsNullOrEmpty()) strSogenD += strXSogen + "/";
                if (!clsHcVariable.Gstr_PanB_Etc.IsNullOrEmpty()) strSogenD += clsHcVariable.Gstr_PanB_Etc;
            }

            //소견조치 자동 설정
            if (clsHcType.TFA.Panjeng == "1")   //정상A
            {
                if (cboSenileBodyFunction.SelectedIndex == 2) { strSogenD += "노인신체검사:낙상고위험군 낙상주의요/"; }
                if (cboCognitiveFunction.SelectedIndex == 2) { strSogenD += "추가인지기능평가필요(병.의원진료)/"; }
                if (cboDepression.SelectedIndex >= 3) { strSogenD += "[우울증상과 극복방법]참고 정신건강의학과 진료요/"; }

                strSogen = "정상";
                strSogenB = "";
                if (!strWSogen.IsNullOrEmpty()) { strSogenB += strWSogen; }
                if (!strXSogen.IsNullOrEmpty()) { strSogenB += strXSogen; }
                txtSogen.Text = "특이소견 없음";
                if (strSogenB.IsNullOrEmpty()) { strSogenB = "특이소견 없음"; }
                txtSogenB.Text = strSogenB;
                if (strSogenC.IsNullOrEmpty()) { strSogenC = "특이소견 없음"; }
                txtSogenC.Text = strSogenC;
                if (strSogenD.IsNullOrEmpty()) { strSogenD = "특이소견 없음"; }
                txtSogenD.Text = strSogenD;
                clsHcType.TFA.Sogen = "특이소견 없음";
                clsHcType.TFA.SogenB = strSogenB;
                clsHcType.TFA.SogenC = strSogenC;
                clsHcType.TFA.SogenD = strSogenD;

                return;
            }
            else if (clsHcType.TFA.Panjeng == "2")  //정상B 또는 정상B+의심
            {
                if (clsHcType.TFA.PanU[0] == true) { strSogenB += "유질환:지속적인 치료상담요(고혈압)/ "; }
                if (clsHcType.TFA.PanU[1] == true) { strSogenB += "유질환:지속적인 치료상담요(당뇨병)/ "; }
                if (clsHcType.TFA.PanU[2] == true) { strSogenB += "유질환:지속적인 치료상담요(이상지질혈증)/ "; }
                if (clsHcType.TFA.PanU[3] == true) { strSogenB += "폐결핵유질환: 완치시까지 꾸준한 약복용요/ "; }
            }
            else if (clsHcType.TFA.Panjeng == "3")
            {
                if (clsHcType.TFA.PanU[0] == true) { strSogenB += "유질환:지속적인 치료상담요(고혈압)/ "; }
                if (clsHcType.TFA.PanU[1] == true) { strSogenB += "유질환:지속적인 치료상담요(당뇨병)/ "; }
                if (clsHcType.TFA.PanU[2] == true) { strSogenB += "유질환:지속적인 치료상담요(이상지질혈증)/ "; }
                if (clsHcType.TFA.PanU[3] == true) { strSogenB += "폐결핵유질환: 완치시까지 꾸준한 약복용요/ "; }
            }
            else if (clsHcType.TFA.Panjeng == "4")
            {
                if (clsHcType.TFA.PanR[0] == true) { strSogen += "폐결핵의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[3] == true) { strSogen += "이상지질혈증의심:식이,운동,추적검사/ "; }
                if (clsHcType.TFA.PanR[4] == true) { strSogen += "간장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[6] == true) { strSogen += "신장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[7] == true) { strSogen += "빈혈증의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[8] == true) { strSogen += "골다공증의심:칼슘섭취,진료상담/ "; }
                if (clsHcType.TFA.PanR[10] == true) { strSogen += "비만도높음:내과상담요/ "; }
                if (clsHcType.TFA.PanR[11] == true) { strSogen += "난청:이비인후과 추적관찰/ "; }
                //--------------------------------------------------------------------------------------------------------------
                if (clsHcType.TFA.PanU[0] == true) { strSogenB += "유질환:지속적인 치료상담요(고혈압)/ "; }
                if (clsHcType.TFA.PanU[1] == true) { strSogenB += "유질환:지속적인 치료상담요(당뇨병)/ "; }
                if (clsHcType.TFA.PanU[2] == true) { strSogenB += "유질환:지속적인 치료상담요(이상지질혈증)/ "; }
                if (clsHcType.TFA.PanU[3] == true) { strSogenB += "폐결핵유질환: 완치시까지 꾸준한 약복용요/ "; }
                if (!clsHcVariable.Gstr_PanR_Etc.IsNullOrEmpty()) { txtPanjengR_Etc.Text = clsHcVariable.Gstr_PanR_Etc; }
            }
            else if (clsHcType.TFA.Panjeng == "5")  //2차
            {
                if (clsHcType.TFA.PanR[2] == true) { strSogen += "고혈압의심:확진검사요/ "; }
                if (clsHcType.TFA.PanR[5] == true) { strSogen += "당뇨병의심:확진검사요/ "; }
                //-----------------------------------------------------------------------------------------/
                if (clsHcType.TFA.PanR[0] == true) { strSogen += "폐결핵의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[3] == true) { strSogen += "이상지질혈증의심:식이,운동,추적검사/ "; }
                if (clsHcType.TFA.PanR[4] == true) { strSogen += "간장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[6] == true) { strSogen += "신장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[7] == true) { strSogen += "빈혈증의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[8] == true) { strSogen += "골다공증의심:칼슘섭취,진료상담/ "; }
                if (clsHcType.TFA.PanR[10] == true) { strSogen += "비만도높음:전문상담요/ "; }
                if (clsHcType.TFA.PanR[11] == true) { strSogen += "난청:이비인후과 추적관찰/ "; }
                //-----------------------------------------------------------------------------------------/
                if (clsHcType.TFA.PanU[0] == true) { strSogenB += "유질환:지속적인 치료상담요(고혈압)/ "; }
                if (clsHcType.TFA.PanU[1] == true) { strSogenB += "유질환:지속적인 치료상담요(당뇨병)/ "; }
                if (clsHcType.TFA.PanU[2] == true) { strSogenB += "유질환:지속적인 치료상담요(이상지질혈증)/ "; }
                if (clsHcType.TFA.PanU[3] == true)
                {
                    strSogenB += "폐결핵유질환: 완치시까지 꾸준한 약복용요/ ";

                    if (!clsHcVariable.Gstr_PanR_Etc.IsNullOrEmpty()) { txtPanjengR_Etc.Text = clsHcVariable.Gstr_PanR_Etc; }
                }
            }
            else if (clsHcType.TFA.Panjeng == "8")
            {
                if (clsHcType.TFA.PanR[2] == true) { strSogen += "고혈압의심:확진검사요/ "; }
                if (clsHcType.TFA.PanR[5] == true) { strSogen += "당뇨병의심:확진검사요/ "; }
                if (clsHcType.TFA.PanR[0] == true) { strSogen += "폐결핵의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[3] == true) { strSogen += "이상지질혈증의심:식이,운동,추적검사/ "; }
                if (clsHcType.TFA.PanR[4] == true) { strSogen += "간장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[6] == true) { strSogen += "신장질환의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[7] == true) { strSogen += "빈혈증의심:내과진료요/ "; }
                if (clsHcType.TFA.PanR[8] == true) { strSogen += "골다공증의심:칼슘섭취,진료상담/ "; }
                if (clsHcType.TFA.PanR[10] == true) { strSogen += "비만도높음:전문상담요/ "; }
                if (clsHcType.TFA.PanR[11] == true) { strSogen += "난청:이비인후과 추적관찰/ "; }
                //-----------------------------------------------------------------------------------------/
                if (clsHcType.TFA.PanU[0] == true) { strSogenB += "유질환:지속적인 치료상담요(고혈압)/ "; }
                if (clsHcType.TFA.PanU[1] == true) { strSogenB += "유질환:지속적인 치료상담요(당뇨병)/ "; }
                if (clsHcType.TFA.PanU[2] == true) { strSogenB += "유질환:지속적인 치료상담요(이상지질혈증)/ "; }
                if (clsHcType.TFA.PanU[3] == true) { strSogenB += "폐결핵유질환: 완치시까지 꾸준한 약복용요/ "; }
                if (!clsHcVariable.Gstr_PanR_Etc.IsNullOrEmpty()) { txtPanjengR_Etc.Text = clsHcVariable.Gstr_PanR_Etc; }
            }

            txtSogen.Text = strSogen;
            txtSogenB.Text = strSogenB;
            txtSogenC.Text = strSogenC;
            txtSogenD.Text = strSogenD;
            

            if (cboLiver.SelectedIndex > 0)
            {
                //보균자,접종대상자
                if (VB.Left(cboLiver.Text, 1) == "2")
                {
                    txtSogenD.Text += "B형간염검사(예방접종 상담필요)/";
                }
                else if (VB.Left(cboLiver.Text, 1) == "3")
                {
                    txtSogenD.Text += "B형간염검사(보균자):내과 정기적 검진요/";
                }
            }

            if (cboSenileBodyFunction.SelectedIndex == 2) { txtSogenD.Text += "노인신체검사:낙상고위험군 낙상주의요/"; }
            if (cboCognitiveFunction.SelectedIndex == 2) { txtSogenD.Text += "추가인지기능평가필요(병.의원진료)/"; }
            if (cboDepression.SelectedIndex >= 3) { txtSogenD.Text += "[우울증상과 극복방법]참고 정신건강의학과 진료요/"; }

            if (!clsHcType.TFA.Sogen.IsNullOrEmpty()) { txtSogen.Text += clsHcType.TFA.Sogen; }
            if (txtSogen.Text.Trim() == "정상/") { txtSogen.Text = "특이소견 없음"; }
            if (txtSogenB.Text.Trim() == "정상/") { txtSogenB.Text = "특이소견 없음"; }
            if (txtSogenC.Text.Trim() == "정상/") { txtSogenC.Text = "특이소견 없음"; }
            if (txtSogenD.Text.Trim() == "정상/") { txtSogenD.Text = "특이소견 없음"; }
            if (txtSogen.Text.Trim() == "") { txtSogen.Text = "특이소견 없음"; }
            if (txtSogenB.Text.Trim() == "") { txtSogenB.Text = "특이소견 없음"; }
            if (txtSogenC.Text.Trim() == "") { txtSogenC.Text = "특이소견 없음"; }
            if (txtSogenD.Text.Trim() == "") { txtSogenD.Text = "특이소견 없음"; }
        }

        /// <summary>
        /// 업무적합_사후관리_Display()
        /// </summary>
        void fn_Suitability_FollowUp_Display()
        {
            string strLtdName = "";

            //회사가 아닌경우 사후관리,업무적합성을 공란으로 처리(소장님)
            strLtdName = hb.READ_Ltd_Name(FstrLtdCode);

            if (strLtdName.IsNullOrEmpty())
            {
                cboSahu1.Text = "";
                cboSahu2.Text = "";
                cboSahu3.Text = "";
                return;
            }

            cboUpmu.SelectedIndex = 1;

            //R1 판정 사후관리 세팅
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkPanjengR = (Controls.Find("chkPanjengR" + (i).To<string>(), true)[0] as CheckBox);
                if (i != 2 && i != 5)
                {
                    if (chkPanjengR.Checked == true)
                    {
                        cboSahu1.Enabled = true;
                        cboSahu1.SelectedIndex = 7; //기타
                        cboSahu1.SelectedIndex = 2; //업무적합성
                    }
                }
            }

            //D 판정 사후관리 세팅
            for (int i = 0; i <= 3; i++)
            {
                CheckBox chkPanjengU = (Controls.Find("chkPanjengU" + (i).To<string>(), true)[0] as CheckBox);
                if (chkPanjengU.Checked == true)
                {
                    cboSahu2.Enabled = true;
                    cboSahu2.SelectedIndex = 4; //근무중 치료
                    cboUpmu.SelectedIndex = 2;  //업무적합성
                }
            }
        }

        /// <summary>
        /// 판정이 의심질환,유질환,생활습관,기타로 분리됨
        /// </summary>
        void fn_Screen_Display()
        {
            string[] strMunjin = new string[3];
            string[] strChiRyo = new string[3];
            string[] strBalYY = new string[3];
            string strGaJokJil = "";
            int[] n40Feel = new int[4];
            int[] n66Feel = new int[3];
            int[] n66Memory = new int[5];
            int nTotal = 0;
            string strOK = "";

            int nREAD = 0;
            int nRow = 0;
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
            long nLicense = 0;
            string strDrname = "";
            int nAscii = 0;
            int nHyelH = 0;
            int nHyelL = 0;
            int nHEIGHT = 0;
            int nWeight = 0;
            int nResult = 0;
            string strRemark = "";
            string strExPan = "";    //검사1건의 판정결과
            string strGbSPC = "";
            string strSExam = "";    //선택검사
            string strTemp = "";

            int nGan1 = 0;
            long nGan2 = 0;
            int nGan3 = 0;
            int nGanResult = 0;
            long nAge = 0;

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWrtNo) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWrtNo + " 는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrPanOk1 = "";
            FstrPanOk2 = "";
            FnA118 = 0;
            FnA119 = 0;
            FnA120 = 0;

            FstrGbSPC = "";

            //인적사항을 READ
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWrtNo);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FnPano = list.PANO;
            FstrJepDate = list.JEPDATE.To<string>();
            FstrJepDate = list.JEPDATE;
            FstrSex = list.SEX.Trim();
            FnAge = list.AGE;
            FstrGjYear = list.GJYEAR;

            //취급물질
            FstrUCodes = list.UCODES;
            lblUCodeName.Text = hm.UCode_Names_Display(FstrUCodes);
            strSExam = list.SEXAMS;
            lblSExamName.Text = hm.SExam_Names_Display(strSExam);

            //일반,특수 분리 판정 시 오류로 수정함
            if (fn_First_Panjeng_Check() == true)
            {
                fn_Auto_LifeHabit_Update(); //문진표를 기준으로 생활습관 자동 업데이트
                hm.Auto_NewPanjeng_First(FnWrtNo, FstrJepDate, VB.Left(FstrJepDate, 4), FstrSex);
                fn_Auto_Panjeng_Display();

                clsHcType.TFA.PanB[8] = false;
                if (clsHcType.TFA.PanB[8] == false)
                {
                    cboPanBEtc.Enabled = false;
                    chkPanjengB8.Checked = false;
                }
                if (clsHcType.TFA.PanB[8] == true)
                {
                    if (chkPanjengA.Checked == true)
                    {
                        chkPanjengA.Checked = false;
                    }
                    cboPanBEtc.Enabled = true;
                    chkPanjengB8.Checked = true;
                }
                cboPanBEtc.SelectedIndex = clsHcVariable.GnPanB_Etc;
                if (!clsHcVariable.Gstr_PanB_Etc.IsNullOrEmpty())
                {
                    if (string.Compare(clsHcVariable.GstrGjYear, "2014") <= 0)
                    {
                        if (txtSogen.Text.IsNullOrEmpty() || txtSogen.Text.Trim() == "정상")
                        {
                            txtSogen.Text = clsHcVariable.Gstr_PanB_Etc;
                        }
                        else
                        {
                            txtSogenC.Text += clsHcVariable.Gstr_PanB_Etc;
                        }
                    }
                    else
                    {
                        if (txtSogenC.Text.IsNullOrEmpty() || txtSogenC.Text.Trim() == "정상" || txtSogenC.Text.Trim() == "특이소견 없음")
                        {
                            if (txtSogenD.Text.Trim() == "특이소견 없음")
                            {
                                txtSogenD.Text = clsHcVariable.Gstr_PanB_Etc;
                            }
                            else
                            {
                                txtSogenD.Text += clsHcVariable.Gstr_PanB_Etc;
                            }
                        }
                        else
                        {
                            if (txtSogenD.Text.Trim() == "특이소견 없음")
                            {
                                txtSogenD.Text = clsHcVariable.Gstr_PanB_Etc;
                            }
                            else
                            {
                                txtSogenD.Text += clsHcVariable.Gstr_PanB_Etc;
                            }
                        }
                    }
                }

                if (!clsHcVariable.Gstr_PanR_Etc.IsNullOrEmpty())
                {
                    if (clsHcVariable.Gstr_PanR_Etc.IndexOf("시력장애") > 0)
                    {
                        if (txtSogen.Text.IsNullOrEmpty() || txtSogen.Text.Trim() == "정상" || txtSogen.Text.Trim() == "특이소견 없음")
                        {
                            txtSogen.Text = "시력장애:안과 추적관찰";
                        }
                        else
                        {
                            txtSogen.Text += "시력장애:안과 추적관찰";
                        }
                    }
                }
                txtSogen.Text = txtSogen.Text.Trim() + "/";

                btnSave.Enabled = true;

                //업무적합_사후관리_Display
                fn_Suitability_FollowUp_Display();
            }
            else
            {
                //판정한 의사만 수정이 가능함
                strOK = "";
                btnSave.Enabled = true;
            }

            //건강검진 문진표 및 결과를  READ
            List<HIC_RES_BOHUM1> list2 = hicResBohum1Service.GetAllByWrtno(FnWrtNo);

            if (list2.Count > 0)
            {
                txtPanjengB_etc.Text = list2[0].PANJENGB_ETC;
                txtPanjengR_Etc.Text = list2[0].PANJENGR_ETC;

                if (!list2[0].PANJENGDATE.IsNullOrEmpty())            //판정일자
                {
                    dtpPanDate.Text = list2[0].PANJENGDATE;
                }

                nLicense = list2[0].PANJENGDRNO;           //의사면허번호
                txtPanDrNo.Text = "";
                lblDrName.Text = "";

                if (nLicense > 0)
                {
                    txtPanDrNo.Text = nLicense.To<string>();
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                }

                //판정완료자는 판정한 의사만 변경이 가능함
                //lblPanjeng.Enabled = false;  //윤조연임시로 풀어줌
                //lblPanjeng.Enabled = true;
                //If nLicense = 0 Or(nLicense = GnHicLicense And GnHicLicense > 0) Then
                //    PanelPanjeng.Enabled = True
                //End If

                //임시저장 등록버튼 클릭 가능하게
                //if (txtPanDrNo.Text.To<long>() == 0)
                //{
                //    lblPanjeng.Enabled = true;
                //    TxtPanDrNo.Locked = False
                //}

                //생애검진 우울증 및 인지기능장애 검사
                if (FstrTFlag == "Y")
                {
                    //최근의 기분상태T40_Feel3
                    n40Feel[0] = list2[0].T40_FEEL1.To<int>();
                    n40Feel[1] = list2[0].T40_FEEL2.To<int>();
                    n40Feel[2] = list2[0].T40_FEEL3.To<int>();
                    n40Feel[3] = list2[0].T40_FEEL4.To<int>();

                    n66Feel[1] = list2[0].T66_FEEL1.To<int>();
                    n66Feel[2] = list2[0].T66_FEEL2.To<int>();
                    n66Feel[3] = list2[0].T66_FEEL3.To<int>();
                    n66Feel[0] = n66Feel[1] + n66Feel[2] + n66Feel[3];

                    n66Memory[0] = list2[0].T66_MEMORY1.To<int>();
                    n66Memory[1] = list2[0].T66_MEMORY2.To<int>();
                    n66Memory[2] = list2[0].T66_MEMORY3.To<int>();
                    n66Memory[3] = list2[0].T66_MEMORY4.To<int>();
                    n66Memory[4] = list2[0].T66_MEMORY5.To<int>();
                }

                nTotal = 0;
                n66Memory[0] = list2[0].T66_MEMORY1.To<int>();
                n66Memory[1] = list2[0].T66_MEMORY2.To<int>();
                n66Memory[2] = list2[0].T66_MEMORY3.To<int>();
                n66Memory[3] = list2[0].T66_MEMORY4.To<int>();
                n66Memory[4] = list2[0].T66_MEMORY5.To<int>();

                //-------------------- 판정결과 DISPLAY ---------------------------
                //판정B

                if (list2[0].PANJENGB1 == "1")
                {
                    chkPanjengB0.Checked = true;
                }
                else
                {
                    chkPanjengB0.Checked = false;
                }
                if (list2[0].PANJENGB2 == "1")
                {
                    chkPanjengB1.Checked = true;
                }
                else
                {
                    chkPanjengB1.Checked = false;
                }
                if (list2[0].PANJENGB3 == "1")
                {
                    chkPanjengB2.Checked = true;
                }
                else
                {
                    chkPanjengB2.Checked = false;
                }
                if (list2[0].PANJENGB4 == "1")
                {
                    chkPanjengB3.Checked = true;
                }
                else
                {
                    chkPanjengB3.Checked = false;
                }
                if (list2[0].PANJENGB5 == "1")
                {
                    chkPanjengB4.Checked = true;
                }
                else
                {
                    chkPanjengB4.Checked = false;
                }
                if (list2[0].PANJENGB6 == "1")
                {
                    chkPanjengB5.Checked = true;
                }
                else
                {
                    chkPanjengB5.Checked = false;
                }
                if (list2[0].PANJENGB7 == "1")
                {
                    chkPanjengB6.Checked = true;
                }
                else
                {
                    chkPanjengB6.Checked = false;
                }
                if (list2[0].PANJENGB8 == "1")
                {
                    chkPanjengB7.Checked = true;
                }
                else
                {
                    chkPanjengB7.Checked = false;
                }
                if (list2[0].PANJENGB9 == "1")
                {
                    chkPanjengB8.Checked = true;
                }
                else
                {
                    chkPanjengB8.Checked = false;
                }
                if (list2[0].PANJENGB10 == "1")
                {
                    chkPanjengB9.Checked = true;
                }
                else
                {
                    chkPanjengB9.Checked = false;
                }

                //판정b기타질환관리세부
                if (!list2[0].PANJENGB_ETC_DTL.IsNullOrEmpty())
                {
                    cboPanBEtc.Enabled = true;
                    cboPanBEtc.SelectedIndex = list2[0].PANJENGB_ETC_DTL.To<int>();
                    cboPanBEtc.Enabled = false;
                }
                //기타질환관리
                if (list2[0].PANJENGB9 == "1")
                {
                    cboPanBEtc.Enabled = true;
                }
                else if (cboPanBEtc.Enabled == false)
                {
                    cboPanBEtc.Enabled = false;
                }

                //판정R1,R2
                if (list2[0].PANJENGR1 == "1")
                {
                    chkPanjengR0.Checked = true;
                }
                else
                {
                    chkPanjengR0.Checked = false;
                }
                if (list2[0].PANJENGR2 == "1")
                {
                    chkPanjengR1.Checked = true;
                }
                else
                {
                    chkPanjengR1.Checked = false;
                }
                if (list2[0].PANJENGR3 == "1")
                {
                    chkPanjengR2.Checked = true;
                }
                else
                {
                    chkPanjengR2.Checked = false;
                }
                if (list2[0].PANJENGR4 == "1")
                {
                    chkPanjengR3.Checked = true;
                }
                else
                {
                    chkPanjengR3.Checked = false;
                }
                if (list2[0].PANJENGR5 == "1")
                {
                    chkPanjengR4.Checked = true;
                }
                else
                {
                    chkPanjengR4.Checked = false;
                }
                if (list2[0].PANJENGR6 == "1")
                {
                    chkPanjengR5.Checked = true;
                }
                else
                {
                    chkPanjengR5.Checked = false;
                }
                if (list2[0].PANJENGR7 == "1")
                {
                    chkPanjengR6.Checked = true;
                }
                else
                {
                    chkPanjengR6.Checked = false;
                }
                if (list2[0].PANJENGR8 == "1")
                {
                    chkPanjengR7.Checked = true;
                }
                else
                {
                    chkPanjengR7.Checked = false;
                }
                if (list2[0].PANJENGR9 == "1")
                {
                    chkPanjengR8.Checked = true;
                }
                else
                {
                    chkPanjengR8.Checked = false;
                }
                if (list2[0].PANJENGR10 == "1")
                {
                    chkPanjengR9.Checked = true;
                }
                else
                {
                    chkPanjengR9.Checked = false;
                }
                if (list2[0].PANJENGR11 == "1")
                {
                    chkPanjengR10.Checked = true;
                }
                else
                {
                    chkPanjengR10.Checked = false;
                }
                if (list2[0].PANJENGR12 == "1")
                {
                    chkPanjengR11.Checked = true;
                }
                else
                {
                    chkPanjengR11.Checked = false;
                }

                //판정U
                if (list2[0].PANJENGU1 == "1")
                {
                    chkPanjengU0.Checked = true;
                }
                else
                {
                    chkPanjengU0.Checked = false;
                }
                if (list2[0].PANJENGU2 == "1")
                {
                    chkPanjengU1.Checked = true;
                }
                else
                {
                    chkPanjengU1.Checked = false;
                }
                if (list2[0].PANJENGU3 == "1")
                {
                    chkPanjengU2.Checked = true;
                }
                else
                {
                    chkPanjengU2.Checked = false;
                }
                if (list2[0].PANJENGU4 == "1")
                {
                    chkPanjengU3.Checked = true;
                }
                else
                {
                    chkPanjengU3.Checked = false;
                }

                //직업병 D1
                if (!list2[0].PANJENGD11.IsNullOrEmpty())
                {
                    txtD10.Text = list2[0].PANJENGD11;
                }
                else
                {
                    txtD10.Text = "";
                }
                if (!list2[0].PANJENGD12.IsNullOrEmpty())
                {
                    txtD11.Text = list2[0].PANJENGD12;
                }
                else
                {
                    txtD11.Text = "";
                }
                if (!list2[0].PANJENGD13.IsNullOrEmpty())
                {
                    txtD12.Text = list2[0].PANJENGD13;
                }
                else
                {
                    txtD12.Text = "";
                }

                //직업병 D2
                if (!list2[0].PANJENGD21.IsNullOrEmpty())
                {
                    txtD20.Text = list2[0].PANJENGD21;
                }
                else
                {
                    txtD10.Text = "";
                }
                if (!list2[0].PANJENGD22.IsNullOrEmpty())
                {
                    txtD21.Text = list2[0].PANJENGD22;
                }
                else
                {
                    txtD21.Text = "";
                }
                if (!list2[0].PANJENGD23.IsNullOrEmpty())
                {
                    txtD22.Text = list2[0].PANJENGD23;
                }
                else
                {
                    txtD22.Text = "";
                }

                //판정(의심) 기타질환
                if (list2[0].PANJENGR10 == "1")
                {
                    chkPanjengR9.Checked = true;
                    if (!list2[0].PANJENGETC.IsNullOrEmpty())
                    {
                        nAscii = VB.Asc(list2[0].PANJENGETC) - 64;
                    }
                }
                else
                {
                    chkPanjengR9.Checked = false;
                }

                //간염여부
                if (!list2[0].LIVER3.IsNullOrEmpty())
                {
                    cboLiver.SelectedIndex = list2[0].LIVER3.To<int>();
                }
                //사후관리(R1)
                if (list2[0].PANJENGSAHU != "0")
                {
                    cboSahu1.SelectedIndex = list2[0].PANJENGSAHU.To<int>();
                }
                //사후관리(D2)
                if (list2[0].PANJENGSAHU2 != "0")
                {
                    cboSahu2.SelectedIndex = list2[0].PANJENGSAHU2.To<int>();
                }
                //사후관리(D1)
                if (list2[0].PANJENGSAHU3 != "0")
                {
                    cboSahu3.SelectedIndex = list2[0].PANJENGSAHU3.To<int>();
                }

                //업무적합성
                cboUpmu.SelectedIndex = 0;
                if (!list2[0].WORKYN.IsNullOrEmpty())
                {
                    cboUpmu.SelectedIndex = list2[0].WORKYN.To<int>();
                }

                //판정 A일때 만 판정란에 표시
                if (list2[0].PANJENG == "1")
                {
                    chkPanjengA.Checked = true;
                }
                txtSogen.Text = list2[0].SOGEN;  //의심질환소견
                txtSogenB.Text = list2[0].SOGENB; //유질환소견
                txtSogenC.Text = list2[0].SOGENC; //생활습관소견
                txtSogenD.Text = list2[0].SOGEND; //기타소견
                if (txtSogen.Text.IsNullOrEmpty()) txtSogen.Text = "특이소견 없음";
                if (txtSogenB.Text.IsNullOrEmpty()) txtSogenB.Text = "특이소견 없음";
                if (txtSogenC.Text.IsNullOrEmpty()) txtSogenC.Text = "특이소견 없음";
                if (txtSogenD.Text.IsNullOrEmpty()) txtSogenD.Text = "특이소견 없음";

                fn_Screen_Life_Slip2();
            }
        }

        /// <summary>
        /// 우울증,인지기능,노인신체기능평가
        /// </summary>
        void fn_Screen_Life_Slip2()
        {
            int nREAD = 0;
            string strDAT = "";
            string strTSmoke1 = "";
            long nTOT = 0;
            string[] strGbSlip = new string[3];
            long[] nJemsu = new long[3];
            bool blnSuicidalImpulse = false; //자살생각
            List<string> strExCodes = new List<string>();

            txtPHQScr.Text = "";
            txtKDSQScr.Text = "";
            cboDepression.SelectedIndex = -1;
            cboCognitiveFunction.SelectedIndex = -1;
            cboSenileBodyFunction.SelectedIndex = -1;
            cboDepression.Enabled = false;
            cboCognitiveFunction.Enabled = false;
            cboSenileBodyFunction.Enabled = false;

            for (int i = 0; i < 3; i++)
            {
                strGbSlip[i] = "";
            }
            blnSuicidalImpulse = false;

            //우울증,인지기능,노인신체기능평가 대상인지 확인
            List<HIC_SUNAPDTL> list = hicSunapdtlService.GetItembyWrtNo(FnWrtNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i].CODE.Trim())
                    {
                        case "1167":
                            strGbSlip[0] = "Y"; //우울증
                            break;
                        case "1163":
                            strGbSlip[1] = "Y"; //인지기능
                            break;
                        case "1168":
                            strGbSlip[2] = "Y"; //노인신체
                            break;
                        default:
                            break;
                    }
                }
            }

            strExCodes.Clear();
            strExCodes.Add("A143");
            strExCodes.Add("A144");
            strExCodes.Add("A145");
            strExCodes.Add("A146");
            strExCodes.Add("A147");
            strExCodes.Add("A130");
            strExCodes.Add("A129");

            //생활습관평가도구표 결과값을 읽음
            List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCodes);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    switch (list2[i].EXCODE)
                    {
                        case "A130":    //우울증
                            nJemsu[0] = list2[i].RESULT.To<long>();    
                            break;
                        case "A129":    //인지기능
                            nJemsu[1] = list2[i].RESULT.To<long>();
                            break;
                        default:
                            break;
                    }
                }
            }

            //우울증
            if (strGbSlip[0] == "Y")
            {
                string[] strCodes = { "80902", "80903", "80904" };

                cboDepression.Enabled = true;
                //자살생각 여부
                if (hicTitemService.GetCountbyWrtNoGubunCode(FnWrtNo, "18", strCodes) > 0)
                {
                    blnSuicidalImpulse = true;
                }

                if (blnSuicidalImpulse == true)
                {
                    txtPHQScr.Text = string.Format("{0:0}", nJemsu[0]);
                    cboDepression.SelectedIndex = 5;
                    MessageBox.Show("우울증 평가도구 9번문항에 ★자살생각으로 응답을 하였습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    txtPHQScr.Text = string.Format("{0:0}", nJemsu[0]);
                    if (nJemsu[0] <= 4)
                    {
                        cboDepression.SelectedIndex = 1;
                    }
                    else if (nJemsu[0] <= 9)
                    {
                        cboDepression.SelectedIndex = 2;
                    }
                    else if (nJemsu[0] <= 19)
                    {
                        cboDepression.SelectedIndex = 3;
                    }
                    else
                    {
                        cboDepression.SelectedIndex = 4;
                    }
                }
            }
            //인지기능
            if (strGbSlip[1] == "Y")
            {
                cboCognitiveFunction.Enabled = true;
                txtKDSQScr.Text = string.Format("{0:0}", nJemsu[1]);
                if (nJemsu[1] <= 5)
                {
                    cboCognitiveFunction.SelectedIndex = 1;
                }
                else
                {
                    cboCognitiveFunction.SelectedIndex = 2;
                }
            }

            //노인신체기능
            if (strGbSlip[2] == "Y")
            {
                cboSenileBodyFunction.Enabled = true;
                //노인신체검사 결과를 읽음
                strExCodes.Clear();
                strExCodes.Add("A118");
                strExCodes.Add("A119");
                strExCodes.Add("A120");

                List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCodes);

                nREAD = list3.Count;
                cboSenileBodyFunction.SelectedIndex = 1;
                for (int i = 0; i < nREAD; i++)
                {
                    //하지기능
                    if (list3[i].EXCODE == "A118")
                    {
                        if (list3[i].RESULT.To<long>() > 10)
                        {
                            cboSenileBodyFunction.SelectedIndex = 2;
                        }
                    }
                    //보행장애
                    if (list3[i].EXCODE == "A119")
                    {
                        if (string.Compare(list3[i].RESULT, "01") > 0)
                        {
                            cboSenileBodyFunction.SelectedIndex = 2;
                        }
                    }
                    //평형성
                    if (list3[i].EXCODE == "A120")
                    {
                        if (list3[i].RESULT.To<long>() < 20 || list3[i].RESULT.To<long>() >= 100)
                        {
                            cboSenileBodyFunction.SelectedIndex = 2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 보험1차 정상A 자동판정
        /// argJob : [수동]=메뉴를 클릭한 경우,[자동]=신규로 조회시 자동실행
        /// </summary>
        /// <param name="argJob"></param>
        void fn_Bohum_AutoPanjeng(string argJob)
        {
            int nREAD = 0;
            int nPanCnt = 0;
            string strPanjeng = "";
            //string strExPan = "";
            string strOK = "";
            string strSname = "";

            long nWRTNO = 0;
            string strPtNo = "";
            string strSex = "";
            string strJepDate = "";
            //int nPan_R = 0;
            //int nPan_C = 0;
            //int nPan_B = 0;
            string[] strT_STAT = new string[8];
            long nMunjinDrno = 0;
            string strPanjengDrNo = "";
            string strSExam = "";
            List<string> strTemp = new List<string>();
            long nMunjinDrNo = 0;
            string strExCode;

            Cursor.Current = Cursors.WaitCursor;

            if (VB.Pstr(cboPanBEtc.Text, ".", 1) != "**")
            {
                nMunjinDrNo = VB.Pstr(cboPanBEtc.Text, ".", 1).To<long>();
            }
            else
            {
                nMunjinDrNo = 0;
            }

            //자동가판정 대상 명단을 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyMunjinDrNo(clsHcVariable.GnHicLicense, clsHcVariable.B02_PANJENG_DRNO, nMunjinDrNo, clsType.User.IdNumber);

            nREAD = list.Count;
            if (nREAD > 0)
            {
                Cursor.Current = Cursors.Default;
                if (argJob == "[수동]")
                {
                    MessageBox.Show("자동 판정할 자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            nPanCnt = 0;
            for (int i = 0; i < nREAD; i++)
            {
                fn_Screen_Clear();
                FnWrtNo = list[i].WRTNO;
                strSname = list[i].SNAME;
                strSExam = list[i].SEXAMS;
                nWRTNO = list[i].WRTNO;
                strPtNo = list[i].PTNO;
                strJepDate = list[i].JEPDATE;
                strSex = list[i].SEX;
                FstrJong = list[i].GJJONG;
                nMunjinDrno = list[i].MUNJINDRNO;
                strPanjengDrNo = list[i].PANJENGDRNO.To<string>();
                strOK = "OK";
                strPanjeng = "";
                //흉부촬영 정상인 경우만 자동판정 대상
                strExCode = "A142";
                if (hicResultService.GetResultOnlybyWrtNoExCode(nWRTNO, strExCode) != "01")
                {
                    strOK = "";
                }

                if (strSExam == ",")
                {
                    strSExam = "";
                }

                strTemp.Clear();
                for (int j = 0; j < VB.L(strSExam, ","); j++)
                {
                    if (!VB.Pstr(strSex, ",", j).IsNullOrEmpty())
                    {
                        strTemp.Add(VB.Pstr(strSExam, ",", j));
                    }
                }

                if (!strTemp.IsNullOrEmpty())
                {
                    List<HIC_SUNAPDTL_GROUPCODE> list2 = hicSunapdtlGroupcodeService.GetWrtNoCodeNamebyWrtNoCode(nWRTNO, strTemp, FstrNotAddPanList);

                    nREAD = list2.Count;
                    if (nREAD > 0)
                    {
                        strOK = "";
                    }
                }

                if (strOK == "OK")
                {
                    //생활습관자동판정
                    if (strPanjengDrNo == "0" || strPanjengDrNo.IsNullOrEmpty())
                    {
                        fn_Auto_LifeHabit_Update();
                    }
                }
            }

            Cursor.Current = Cursors.Default;
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
                //2021년 보통음주 추가
                strTemp = list.TMUN0125;
                nDrink1 += fn_Drink_Set(strTemp);
                strTemp = list.TMUN0126;
                nDrink1 += fn_Drink_Set(strTemp);

                nDrink1 *= list.TMUN0004.To<int>();
                switch (list.TMUN0003)
                {
                    case "1":
                        nDrink2 = nDrink1 * 1;      //일주일
                        break;
                    case "2":
                        nDrink2 = nDrink1  / 4;     //한달
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
                //2021년 보통음주 추가
                strTemp = list.TMUN0127;
                nDrink1 = fn_Drink_Set(strTemp);
                strTemp = list.TMUN0128;
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

        void fn_ComboSet(ComboBox argCbo, string argGubun)
        {
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun(argGubun);
            argCbo.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
        }

        void ComboSahu_Set(ComboBox argCbo)
        {
            argCbo.Items.Clear();
            argCbo.Items.Add(" ");
            argCbo.Items.Add("1.근로금지 및 제한");
            argCbo.Items.Add("2.작업전환");
            argCbo.Items.Add("3.근로시간 단축");
            argCbo.Items.Add("4.근무중 치료");
            argCbo.Items.Add("5.추적검사");
            argCbo.Items.Add("6.보후구 착용");
            argCbo.Items.Add("7.기타");
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                if (MessageBox.Show("변경한 내용을 저장하지 않습니다." + "\r\n" + "정말 취소하시겠습니까?", "취소", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    return;
                }                
            }
            else if (sender == btnSave)
            {
                //고혈압 2차대상자 오류 점검
                if (chkPanjengR3.Checked == true && clsHcType.TFA.PanR[2] == false)
                {
                    if (MessageBox.Show("정말로 고혈압 확진대상자가 맞습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (chkPanjengR3.Checked == false && clsHcType.TFA.PanR[2] == true)
                {
                    if (MessageBox.Show("정말로 고혈압 확진대상자가 아닙니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                //당뇨병 2차대상자 오류 점검
                if (chkPanjengR7.Checked == true && clsHcType.TFA.PanR[5] == false)
                {
                    if (MessageBox.Show("정말로 당뇨병 확진대상자가 맞습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (chkPanjengR7.Checked == false && clsHcType.TFA.PanR[5] == true)
                {
                    if (MessageBox.Show("정말로 당뇨병 확진대상자가 아닙니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (chkPanjengU3.Checked == true)
                {
                    if (MessageBox.Show("정말로 유질환(D)-폐결핵이 맞습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                FstrOK = "Y";
                fn_DB_Update_Panjeng("판정");
                if (FstrOK == "Y" && FbAutoPanjeng == false)
                {
                    fn_Panjeng_End_Check();
                }

                if (FstrOK != "N")
                {
                    rSaveEventClosed("HIC");
                }
            }
            else if (sender == btnXrayResult)
            {
                FrmHcPanXrayResultInput = new frmHcPanXrayResultInput(FnWrtNo);
                FrmHcPanXrayResultInput.ShowDialog(this);
            }
            else if (sender == btnD1)
            {
                FstrGubun = "1";
                FrmHcPanPanjengHelp = new frmHcPanPanjengHelp("31");
                FrmHcPanPanjengHelp.rSetGstrValue += new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
                FrmHcPanPanjengHelp.ShowDialog();
                FrmHcPanPanjengHelp.rSetGstrValue -= new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
            }
            else if (sender == btnD2)
            {
                FstrGubun = "2";
                FrmHcPanPanjengHelp = new frmHcPanPanjengHelp("34");
                FrmHcPanPanjengHelp.rSetGstrValue += new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
                FrmHcPanPanjengHelp.ShowDialog();
                FrmHcPanPanjengHelp.rSetGstrValue -= new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
            }
            else if (sender == btnMunjin)
            {
                FrmHcPanMunjin_2019 = new frmHcPanMunjin_2019(FnWrtNo);
                FrmHcPanMunjin_2019.ShowDialog(this);
            }
        }

        void fn_Panjeng_End_Check()
        {
            string strOK = "";
            long nPanDrNo1 = 0;
            long nPanDrNo2 = 0;
            string strPanDate_SPC = "";
            string strPanDate_RES = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;

            strOK = "OK";

            clsDB.setBeginTran(clsDB.DbCon);

            //건강보험1차 판정완료 Check
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(FnWrtNo);

            if (!list.IsNullOrEmpty())
            {
                if (list.PANJENGDRNO == 0 || list.PANJENGDRNO.IsNullOrEmpty())
                {
                    strOK = "NO";
                }
                else
                {
                    nPanDrno = list.PANJENGDRNO;
                    strPanDate = list.PANJENGDATE;
                }
            }

            //판정완료/미완료 SET
            result = hicResBohum1Service.UpdateGbPanjengbyWrtNo(FnWrtNo, strOK);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("판정완료/미완료 저장 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //접수마스타에 판정완료/미완료 SET
            result = hicJepsuService.UpdatePanjengbyWrtNo(nPanDrno, strPanDate, FnWrtNo, strOK);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("접수마스타 판정완료/미완료 저장 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            strOK = "OK";
            //2차대상 판정내역 별도 보관(2차 접수를 하면 보관 안함)
            MessageBox.Show("판정이 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            fn_Screen_Clear();
            

        }

        void fn_DB_Update_Panjeng(string argGbn)
        {
            int nBCnt = 0;
            int nRCNT = 0;
            int nUCNT = 0;
            string strPanjeng = "";
            string strGbErFlag = "";
            string strTemp = "";
            string strChk = "";
            string strOK = "";
            string strT40Feel = "";
            string strT66Stat = "";
            string strTSogen = "";
            string strMsg = "";
            string strSave = "";
            string strFood = "";
            string strDrink = "";
            long nLicense = 0;
            int result = 0;

            //접수마스타의 상태를 변경
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNotInPartNoResult(FnWrtNo, "9");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strMsg += list[i].EXCODE + ":" + list[i].HNAME + "\r\n";
                }
                MessageBox.Show("결과값 누락항목이 있습니다. 검진 담당자에게 연락요망" + "\r\n\r\n" + strMsg, "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrOK = "N";
                return;
            }

            //의사면허번호 조회
            nLicense = clsHcVariable.GnHicLicense;
            if (clsHcVariable.GbHeaAdminSabun == true)
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
                    return;
                }
            }

            hb.READ_HIC_DrSabun(txtPanDrNo.Text);
            //의사당직 휴무 여부확인
            if (hb.READ_DOCTOR_SCH2(clsType.User.DrCode, dtpPanDate.Text) == "NO")
            {
                MessageBox.Show("해당 판정일자는 과장님의 진료가 없거나 휴일입니다." + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrOK = "N";
                return;
            }
            //strGbHabit = "1";
            //for (int i = 0; i <= 4; i++)
            //{
            //    CheckBox chkHabit = (Controls.Find("chkHabit" + (i).To<string>(), true)[0] as CheckBox);
            //    if (chkHabit.Checked == true)
            //    {
            //        strGbHabit = "2";
            //        strHabit[i] = "1";
            //    }
            //    else
            //    {
            //        strHabit[i] = "0";
            //    }
            //}

            txtSogen.Text = txtSogen.Text.Trim();
            nBCnt = 0;
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkPanjengB = (Controls.Find("chkPanjengB" + (i).To<string>(), true)[0] as CheckBox);
                if (chkPanjengB.Checked == true)
                {
                    nBCnt += 1;
                }
            }

            nRCNT = 0;
            for (int i = 0; i <= 11; i++)
            {
                if (i != 2 && i != 5)   //고혈압,당뇨병  제외
                {
                    CheckBox chkPanjengR = (Controls.Find("chkPanjengR" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkPanjengR.Checked == true)
                    {
                        nRCNT += 1;
                    }
                }
            }
            nUCNT = 0;
            for (int i = 0; i <= 3; i++)
            {
                CheckBox chkPanjengU = (Controls.Find("chkPanjengU" + (i).To<string>(), true)[0] as CheckBox);
                if (chkPanjengU.Checked == true)
                {
                    nUCNT += 1;
                }
            }
            //판정값 읽어오기
            if (chkPanjengA.Checked == true)
            {
                strPanjeng = "1";
            }
            if (nBCnt > 0)
            {
                strPanjeng = "2";
            }
            if (nRCNT > 0)
            {
                strPanjeng = "4";
            }
            if (nUCNT > 0)
            {
                strPanjeng = "8";
            }
            if (chkPanjengR3.Checked == true || chkPanjengR7.Checked == true)
            {
                strPanjeng = "5";
            }

            if (argGbn == "판정")
            {
                //dtpPanDate.Text = clsPublic.GstrSysDate;
                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();
                date1 = Convert.ToDateTime(FstrJepDate);
                date2 = Convert.ToDateTime(dtpPanDate.Text);
                if (date1 > date2)
                {
                    MessageBox.Show("판정일자가 검진일(접수일)보다 빠릅니다." + "\r\n" + "날짜를 수정하여 주십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }

                date1 = Convert.ToDateTime(dtpPanDate.Text);
                date2 = Convert.ToDateTime(FstrJepDate).AddDays(14);
                if (date1 > date2)
                {
                    MessageBox.Show("판정일자가 검진일로부터 14일이 경과되었습니다." + "\r\n" + "날짜를 수정하여 주십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }
                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                }
                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("판정의사 면허번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }
                //종합판정 오류 점검
                if (strPanjeng == "1")
                {
                    if (nBCnt > 0 || nRCNT > 0)
                    {
                        MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }
                }
                else if (strPanjeng == "2")
                {
                    if (nBCnt == 0 || nRCNT > 0 || chkPanjengA.Checked == true)
                    {
                        MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }
                }
                else if (strPanjeng == "4")
                {
                    if (chkPanjengR2.Checked == true || chkPanjengR5.Checked == true || chkPanjengA.Checked == true)
                    {
                        MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }
                }
                else if (strPanjeng == "5")
                {
                    if ((chkPanjengR2.Checked != true && chkPanjengR5.Checked != true) || chkPanjengA.Checked == true)
                    {
                        MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }
                }
                else if (strPanjeng == "8")
                {
                    if (chkPanjengU0.Checked == true)   //고혈압
                    {
                        if (chkPanjengR2.Checked == true || chkPanjengB1.Checked == true)
                        {
                            MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                    if (chkPanjengU1.Checked == true)   //당뇨병
                    {
                        if (chkPanjengR5.Checked == true || chkPanjengB4.Checked == true)
                        {
                            MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                    if (chkPanjengU2.Checked == true)   //이상지질혈증
                    {
                        if (chkPanjengR3.Checked == true || chkPanjengB2.Checked == true)
                        {
                            MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                    if (chkPanjengU3.Checked == true)   //폐결핵
                    {
                        if (chkPanjengR0.Checked == true)
                        {
                            MessageBox.Show("종합판정 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                }

                strOK = "Y";
                switch (strPanjeng)
                {
                    case "2":
                    case "4":
                        if (chkPanjengR3.Checked == true)
                        {
                            strOK = "R1판정인데 R2고혈압항목을 체크하셨습니다..";
                        }
                        if (chkPanjengR7.Checked == true)
                        {
                            strOK = "R1판정인데 R2당뇨질환항목을 체크하셨습니다..";
                        }
                        break;
                    default:
                        break;
                }

                if (strOK != "Y")
                {
                    if (MessageBox.Show(strOK + " 그래도 저장을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        FstrOK = "N";
                        return;
                    }
                }

                if (hf.GetLength(txtSogen.Text) > 500)
                {
                    MessageBox.Show("소견조치는 한글500자까지만 가능합니다.(청구에 제한됨)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }

                strGbErFlag = "N";

                if (txtSogen.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("의심질환종합소견이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }
                if (txtSogenB.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("유질환종합소견이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }
                if (txtSogenC.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("생활습관관리종합소견 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }
                if (txtSogenD.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("기타종합소견이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }

                strSave = "";
                if (chkPanjengA.Checked == true)
                {
                    strSave = "OK";
                }

                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkPanjengB = (Controls.Find("chkPanjengB" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkPanjengB.Checked == true)
                    {
                        strSave = "OK";
                    }
                }

                for (int i = 0; i <= 11; i++)
                {
                    CheckBox chkPanjengR = (Controls.Find("chkPanjengR" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkPanjengR.Checked == true)
                    {
                        strSave = "OK";
                    }
                }

                for (int i = 0; i <= 3; i++)
                {
                    CheckBox chkPanjengU = (Controls.Find("chkPanjengU" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkPanjengU.Checked == true)
                    {
                        strSave = "OK";
                    }
                }

                if (strSave.IsNullOrEmpty())
                {
                    MessageBox.Show("판정 값이 모두 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //판정결과를 DB에 UPDATE
                HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

                item.PANJENG = strPanjeng;
                //정상B
                if (chkPanjengB0.Checked == true)
                {
                    item.PANJENGB1 = "1";
                }
                else
                {
                    item.PANJENGB1 = "0";
                }
                if (chkPanjengB1.Checked == true)
                {
                    item.PANJENGB2 = "1";
                }
                else
                {
                    item.PANJENGB2 = "0";
                }
                if (chkPanjengB2.Checked == true)
                {
                    item.PANJENGB3 = "1";
                }
                else
                {
                    item.PANJENGB3 = "0";
                }
                if (chkPanjengB3.Checked == true)
                {
                    item.PANJENGB4 = "1";
                }
                else
                {
                    item.PANJENGB4 = "0";
                }
                if (chkPanjengB4.Checked == true)
                {
                    item.PANJENGB5 = "1";
                }
                else
                {
                    item.PANJENGB5 = "0";
                }
                if (chkPanjengB5.Checked == true)
                {
                    item.PANJENGB6 = "1";
                }
                else
                {
                    item.PANJENGB6 = "0";
                }
                if (chkPanjengB6.Checked == true)
                {
                    item.PANJENGB7 = "1";
                }
                else
                {
                    item.PANJENGB7 = "0";
                }
                if (chkPanjengB7.Checked == true)
                {
                    item.PANJENGB8 = "1";
                }
                else
                {
                    item.PANJENGB8 = "0";
                }
                if (chkPanjengB8.Checked == true)
                {
                    item.PANJENGB9 = "1";
                }
                else
                {
                    item.PANJENGB9 = "0";
                }
                if (chkPanjengB9.Checked == true)
                {
                    item.PANJENGB10 = "1";
                }
                else
                {
                    item.PANJENGB10 = "0";
                }
                //질환의심R1, R2
                if (chkPanjengR0.Checked == true)
                {
                    item.PANJENGR1 = "1";
                }
                else
                {
                    item.PANJENGR1 = "0";
                }
                if (chkPanjengR1.Checked == true)
                {
                    item.PANJENGR2 = "1";
                }
                else
                {
                    item.PANJENGR2 = "0";
                }
                if (chkPanjengR3.Checked == true)
                {
                    item.PANJENGR3 = "1";
                }
                else
                {
                    item.PANJENGR3 = "0";
                }
                if (chkPanjengR4.Checked == true)
                {
                    item.PANJENGR4 = "1";
                }
                else
                {
                    item.PANJENGR4 = "0";
                }
                if (chkPanjengR6.Checked == true)
                {
                    item.PANJENGR5 = "1";
                }
                else
                {
                    item.PANJENGR5 = "0";
                }
                if (chkPanjengR7.Checked == true)
                {
                    item.PANJENGR6 = "1";
                }
                else
                {
                    item.PANJENGR6 = "0";
                }
                if (chkPanjengR8.Checked == true)
                {
                    item.PANJENGR7 = "1";
                }
                else
                {
                    item.PANJENGR7 = "0";
                }
                if (chkPanjengR11.Checked == true)
                {
                    item.PANJENGR8 = "1";
                }
                else
                {
                    item.PANJENGR8 = "0";
                }
                if (chkPanjengR10.Checked == true)
                {
                    item.PANJENGR9 = "1";
                }
                else
                {
                    item.PANJENGR9 = "0";
                }
                if (chkPanjengR9.Checked == true)
                {
                    item.PANJENGR10 = "1";
                }
                else
                {
                    item.PANJENGR10 = "0";
                }
                if (chkPanjengR2.Checked == true)
                {
                    item.PANJENGR11 = "1";
                }
                else
                {
                    item.PANJENGR11 = "0";
                }
                if (chkPanjengR5.Checked == true)
                {
                    item.PANJENGR12 = "1";
                }
                else
                {
                    item.PANJENGR12 = "0";
                }
                //질환의심U
                if (chkPanjengU0.Checked == true)
                {
                    item.PANJENGU1 = "1";
                }
                else
                {
                    item.PANJENGU1 = "0";
                }
                if (chkPanjengU1.Checked == true)
                {
                    item.PANJENGU2 = "1";
                }
                else
                {
                    item.PANJENGU2 = "0";
                }
                if (chkPanjengU2.Checked == true)
                {
                    item.PANJENGU3 = "1";
                }
                else
                {
                    item.PANJENGU3 = "0";
                }
                //직업병D1
                if (!txtD10.Text.IsNullOrEmpty())
                {
                    item.PANJENGD11 = txtD10.Text;
                }
                else
                {
                    item.PANJENGD11 = " ";
                }
                if (!txtD11.Text.IsNullOrEmpty())
                {
                    item.PANJENGD12 = txtD11.Text;
                }
                else
                {
                    item.PANJENGD12 = " ";
                }
                if (!txtD12.Text.IsNullOrEmpty())
                {
                    item.PANJENGD13 = txtD12.Text;
                }
                else
                {
                    item.PANJENGD13 = " ";
                }
                //직업병D2
                if (!txtD20.Text.IsNullOrEmpty())
                {
                    item.PANJENGD21 = txtD20.Text;
                }
                else
                {
                    item.PANJENGD21 = " ";
                }
                if (!txtD21.Text.IsNullOrEmpty())
                {
                    item.PANJENGD22 = txtD21.Text;
                }
                else
                {
                    item.PANJENGD22 = " ";
                }
                if (!txtD22.Text.IsNullOrEmpty())
                {
                    item.PANJENGD23 = txtD22.Text;
                }
                else
                {
                    item.PANJENGD23 = " ";
                }
                //사후관리(R1)
                if (!VB.Left(cboSahu1.Text, 1).IsNullOrEmpty())
                {
                    item.PANJENGSAHU = VB.Left(cboSahu1.Text, 1).Trim();
                }
                else
                {
                    item.PANJENGSAHU = "0";
                }
                //사후관리(D2)
                if (!VB.Left(cboSahu2.Text, 1).IsNullOrEmpty())
                {
                    item.PANJENGSAHU2 = VB.Left(cboSahu2.Text, 1).Trim();
                }
                else
                {
                    item.PANJENGSAHU2 = "0";
                }
                //사후관리(D1)
                if (!VB.Left(cboSahu3.Text, 1).IsNullOrEmpty())
                {
                    item.PANJENGSAHU3 = VB.Left(cboSahu3.Text, 1).Trim();
                }
                else
                {
                    item.PANJENGSAHU3 = "0";
                }
                //업무적합성
                if (!cboUpmu.Text.IsNullOrEmpty())
                {
                    item.WORKYN = VB.Left(cboUpmu.Text, 3);
                }
                else
                {
                    item.WORKYN = "";
                }

                if (!txtPanjengB_etc.Text.IsNullOrEmpty())
                {
                    item.PANJENGB_ETC = txtPanjengB_etc.Text.Trim();
                }
                else
                {
                    item.PANJENGB_ETC = "";
                }
                item.PANJENGB_ETC_DTL = VB.Left(cboPanBEtc.Text, 1);
                if (!txtPanjengR_Etc.Text.IsNullOrEmpty())
                {
                    item.PANJENGR_ETC = txtPanjengR_Etc.Text;
                }
                else
                {
                    item.PANJENGR_ETC = "";
                }
                //item.HABIT = strGbHabit;
                //item.HABIT1 = strHabit[0];
                //item.HABIT2 = strHabit[1];
                //item.HABIT3 = strHabit[2];
                //item.HABIT4 = strHabit[3];
                //item.HABIT5 = strHabit[4];
                //item.JINCHAL1 = strJinchal1;
                //item.JINCHAL2 = strJinchal2;
                if (!cboLiver.Text.IsNullOrEmpty())
                {
                    item.LIVER3 = cboLiver.SelectedIndex.To<string>();
                }
                else
                {
                    item.LIVER3 = "";
                }

                //strTSogen = FstrSName + "님은 다음 항목에 대해 생활습관 개선 상담을 받으시기를 권고합니다." + "\r\n";
                strTemp = "";
                //for (int i = 0; i < 5; i++)
                //{
                //    if (strHabit[i] == "1")
                //    {
                //        switch (i)
                //        {
                //            case 0:
                //                strTemp += "음주,";
                //                break;
                //            case 1:
                //                strTemp += "흡연,";
                //                break;
                //            case 2:
                //                strTemp += "운동,";
                //                break;
                //            case 3:
                //                strTemp += "영양,";
                //                break;
                //            case 4:
                //                strTemp += "비만,";
                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //}

                //if (VB.Right(strTemp, 1) == ",")
                //{
                //    strTemp = VB.Left(strTemp, strTemp.Length - 1);
                //}

                strTSogen += strTemp;

                item.T40_FEEL = strT40Feel;
                item.T66_STAT = strT66Stat;
                item.LIFESOGEN = strTSogen; //생애권고사항
                item.PANJENGETC = "";
                item.SOGEN = txtSogen.Text.Replace("'", "`");
                item.SOGENB = txtSogenB.Text.Replace("'", "`");
                item.SOGENC = txtSogenC.Text.Replace("'", "`");
                item.SOGEND = txtSogenD.Text.Replace("'", "`");
                if (argGbn == "판정")
                {
                    item.PANJENGDATE = dtpPanDate.Text;
                    item.PANJENGDRNO = nLicense;
                }
                else
                {
                    item.PANJENGDATE = "";
                    item.PANJENGDRNO = 0;
                }
                item.WRTNO = FnWrtNo;

                result = hicResBohum1Service.UpdateHicPanjengbyWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인");
                    return;
                }

                //접수마스타에 응급2차 UPDATE
                HIC_JEPSU item2 = new HIC_JEPSU();

                item2.GBAUTOPAN = "N";
                item2.ERFLAG = strGbErFlag;
                item2.WRTNO = FnWrtNo;

                result = hicJepsuService.UpdateGbAutoPanbyWrtNo(item2);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("접수마스타 응급2차 저장시 오류 발생", "확인");
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

        }

        private void ReExam_value(string argValue, string argName)
        {
            if (!argValue.IsNullOrEmpty())
            {
                if (FstrGubun == "1")
                {
                    //for (int i = 0; i <= VB.L(argValue, ","); i++)
                    for (int i = 0; i < VB.L(argValue, ",") - 1; i++)
                    {
                        TextBox txtD1 = (Controls.Find("txtD1" + (i).To<string>(), true)[0] as TextBox);                        
                        txtD1.Text = VB.Pstr(argValue, ",", i + 1);
                        lblD1.Text += VB.Pstr(argName, "/", i + 1) + "/";
                    }

                    if (!lblD1.Text.IsNullOrEmpty())
                    {
                        lblD1.Text = VB.Mid(lblD1.Text, 1, lblD1.Text.Length - 1);
                    }
                    cboSahu3.Enabled = true;
                }
                else if (FstrGubun == "2")
                {
                    lblD2.Text = "";
                    //for (int i = 0; i <= VB.L(argValue, ","); i++)
                    for (int i = 0; i < VB.L(argValue, ",") - 1; i++)
                    {
                        TextBox txtD2 = (Controls.Find("txtD2" + (i).To<string>(), true)[0] as TextBox);
                        txtD2.Text = VB.Pstr(argValue, ",", i + 1);
                        lblD2.Text += VB.Pstr(argName, "/", i + 1) + "/";
                    }
                    if (!lblD1.Text.IsNullOrEmpty())
                    {
                        lblD1.Text = VB.Mid(lblD1.Text, 1, lblD1.Text.Length - 1);
                    }
                    cboSahu2.Enabled = true;
                }
            }
        }

        void fn_Screen_Clear()
        {
            cboDepression.SelectedIndex = -1;
            cboCognitiveFunction.SelectedIndex = -1;
            cboSenileBodyFunction.SelectedIndex = -1;

            cboPanBEtc.SelectedIndex = -1;
            cboDepression.SelectedIndex = -1;
            cboCognitiveFunction.SelectedIndex = -1;
            cboSenileBodyFunction.SelectedIndex = -1;

            cboLiver.SelectedIndex = -1;
            cboUpmu.SelectedIndex = -1;
            cboSahu1.SelectedIndex = -1;
            cboSahu2.SelectedIndex = -1;
            cboSahu3.SelectedIndex = -1;

            txtPanjengB_etc.Text = "";
            txtPanjengR_Etc.Text = "";
            txtPHQScr.Text = "";
            txtKDSQScr.Text = "";

            for (int i = 0; i <= 2; i++)
            {
                TextBox txtD1 = (Controls.Find("txtD1" + (i).To<string>(), true)[0] as TextBox);
                TextBox txtD2 = (Controls.Find("txtD2" + (i).To<string>(), true)[0] as TextBox);
                txtD1.Text = "";
                txtD2.Text = "";
            }

            lblD1.Text = "";
            lblD2.Text = "";

            txtPanDrNo.Text = "";
            lblDrName.Text = "";

            chkPanjengA.Checked = false;

            txtSogen.Text = "";
            txtSogenB.Text = "";
            txtSogenC.Text = "";
            txtSogenD.Text = "";

            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkPanjengB = (Controls.Find("chkPanjengB" + (i).To<string>(), true)[0] as CheckBox);
                chkPanjengB.Checked = false;
            }

            for (int i = 0; i <= 11; i++)
            {
                CheckBox chkPanjengR = (Controls.Find("chkPanjengR" + (i).To<string>(), true)[0] as CheckBox);
                chkPanjengR.Checked = false;
            }

            for (int i = 0; i <= 3; i++)
            {
                CheckBox chkPanjengU = (Controls.Find("chkPanjengU" + (i).To<string>(), true)[0] as CheckBox);
                chkPanjengU.Checked = false;
            }
        }
    }
}
