using ComBase;
using ComBase.Controls;
using ComEmrBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmPanjeng_Etc.cs
/// Description     : 혈액종합검진, 방사선종사자1차, 방사선종사자2차, 회사추가검진(69종) 판정 통합
/// Author          : 이상훈
/// Create Date     : 2020-05-14
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm혈액종합검진판정.frm(Frm혈액종합검진판정), Frm방사선종사자1차판정.frm(Frm방사선종사자1차판정),
///                 Frm방사선종사자1차판정.frm(Frm방사선종사자1차판정), Frm1차일반특수판정_2019.frm(Frm1차일반특수판정_2019) " />

namespace HC_Pan
{
    public partial class frmPanjeng_Etc : Form
    {
        public delegate void SetEventResSave(long argWRTNO);
        public static event SetEventResSave rSetEventResSave;

        //public delegate void SaveEventClosed(string strReturn);
        //public event SaveEventClosed rSaveEventClosed;

        HicResEtcService hicResEtcService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        BasBcodeService basBcodeService = null;
        BasPatientService basPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicResBohum1Service hicResBohum1Service = null;
        BasIllsService basIllsService = null;
        HicJepsuResEtcSangdamNewLtdService hicJepsuResEtcSangdamNewLtdService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicBcodeService hicBcodeService = null;
        HicResEtcBohum1Service hicResEtcBohum1Service = null;
        XrayResultnewService xrayResultnewService = null;
        HicXrayResultService hicXrayResultService = null;
        HicResSpecialService hicResSpecialService = null;

        frmHcPanJochiHelp FrmHcPanJochiHelp = null;
        frmHcPanXHelp FrmHcPanXHelp = null;
        frmHcPanPanjengHelp FrmHcPanPanjengHelp = null;
        frmHcPanSpcSahuCode FrmHcPanSpcSahuCode = null;
        frmHcCodeHelp FrmHcCodeHelp = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcCombo Combo = new clsHcCombo();

        //혈액종양 판정
        long FnWRTNO;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrJepDate;
        string FstrPano;    // 원무행정의 등록번호
        string FstrJong;    // 건진종류
        string FstrPtno;

        int nOldCNT = 0;

        string strAllWRTNO = "";
        string strJepDate = "";
        string strExPan = "";
        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        string strOldJepDate = "";
        string FstrJob = "";
        string strGaJokJil = "";
        string FstrCode;
        string FstrName;
        string FstrXRayCodeList;

        List<string> strExamCode = new List<string>();
        List<string> strTemp = new List<string>();
        List<string> strNotCode = new List<string>();
        List<string> FstrKind = new List<string>();
        List<long> lstWrtno = new List<long>();

        List<HC_PANJENG_PATLIST> FPatListItem;

        public frmPanjeng_Etc()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmPanjeng_Etc(List<long> lstWrtNo, string strPtNo, string strJumin, long nPano, string strJepDate, string strSex, string strJong, List<string> strKind, List<HC_PANJENG_PATLIST> PatListItem, string strJob)
        {
            InitializeComponent();

            lstWrtno = lstWrtNo;
            //FnWrtno = nWrtNo;
            FstrPano = strPtNo;
            FstrJumin = strJumin;
            FstrKind = strKind;
            FnPano = nPano;
            FstrJepDate = strJepDate;
            FstrSex = strSex;
            FstrJong = strJong;            
            FPatListItem = PatListItem;
            FstrJob = strJob;

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResEtcService = new HicResEtcService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            basBcodeService = new BasBcodeService();
            basPatientService = new BasPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicResBohum1Service = new HicResBohum1Service();
            basIllsService = new BasIllsService();
            hicJepsuResEtcSangdamNewLtdService = new HicJepsuResEtcSangdamNewLtdService();
            comHpcLibBService = new ComHpcLibBService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicBcodeService = new HicBcodeService();
            hicResEtcBohum1Service = new HicResEtcBohum1Service();
            xrayResultnewService = new XrayResultnewService();
            hicXrayResultService = new HicXrayResultService();
            hicResSpecialService = new HicResSpecialService();

            this.Load += new EventHandler(eFormLoad);
            this.tabEtc1.Click += new EventHandler(eTabClick);
            this.tabEtc2.Click += new EventHandler(eTabClick);
            this.tabEtc3.Click += new EventHandler(eTabClick);
            this.tabEtc4.Click += new EventHandler(eTabClick);
            this.tabEtc5.Click += new EventHandler(eTabClick);

            //혈액종양검사
            this.btnBldAutoPan.Click += new EventHandler(eBtnClick);
            this.btnBldJochi2.Click += new EventHandler(eBtnClick);
            this.btnBldAutoPan.Click += new EventHandler(eBtnClick);
            this.btnBldOK.Click += new EventHandler(eBtnClick);
            this.btnBldCancel.Click += new EventHandler(eBtnClick);
            this.dtpBldPanDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBldPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBldSogen.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtBldSogen.LostFocus += new EventHandler(eTxtLostFocus);

            //방사선종사자1차
            this.btnXray1MSave.Click += new EventHandler(eBtnClick);
            this.btnXray1MCancel.Click += new EventHandler(eBtnClick);
            this.btnXray1Sogen.Click += new EventHandler(eBtnClick);
            this.btnXray1ReExam.Click += new EventHandler(eBtnClick);
            this.btnXray1Jochi1.Click += new EventHandler(eBtnClick);
            this.btnXray1OK.Click += new EventHandler(eBtnClick);
            this.btnXray1Cancel.Click += new EventHandler(eBtnClick);
            this.btnXray1Default.Click += new EventHandler(eBtnClick);
            this.cboXray1Panjeng.KeyPress += new KeyPressEventHandler(eComboBoxKeyPress);
            this.cboXray1Panjeng.Click += new EventHandler(eComboBoxClick);
            this.chkXray1X1_11.Click += new EventHandler(eChkBoxClick);
            this.chkXray1X1_23.Click += new EventHandler(eChkBoxClick);
            this.chkXray1X1_42.Click += new EventHandler(eChkBoxClick);
            this.chkXray1X43.Click += new EventHandler(eChkBoxClick);
            this.txtXray1Etc.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtXray1PanDrNo1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtXray2PanDrNo1.KeyPress += new KeyPressEventHandler(eTxtKeyPress); 
            this.txtXray1JochiRemark.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtXray1JochiRemark.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtXray1PanDrNo1.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.txtXray2PanDrNo1.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.btnXray1AutoPan.Click += new EventHandler(eBtnClick);

            //방사선종사자2차
            this.btnXray2MSave.Click += new EventHandler(eBtnClick);
            this.btnXray2MCancel.Click += new EventHandler(eBtnClick);
            this.btnXray2Sogen.Click += new EventHandler(eBtnClick);
            this.btnXray2JobYN.Click += new EventHandler(eBtnClick);
            this.btnXray2Sahu.Click += new EventHandler(eBtnClick);
            this.btnXray2Jochi1.Click += new EventHandler(eBtnClick);
            this.btnXray2OK.Click += new EventHandler(eBtnClick);
            this.btnXray2Cancel.Click += new EventHandler(eBtnClick);
            this.btnXray2Default.Click += new EventHandler(eBtnClick);

            //회사추가검진(69종)
            this.btnAddSave.Click += new EventHandler(eBtnClick);
            this.btnAddCancel.Click += new EventHandler(eBtnClick);
            this.btnAddJochi1.Click += new EventHandler(eBtnClick);
            this.dtpAddPanDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtAddPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtAddSogen.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtAddSogen.LostFocus += new EventHandler(eTxtLostFocus);

            //위생검진(54종)
            this.btnWeSeangSave.Click += new EventHandler(eBtnClick);
            this.btnWeSeangCancel.Click += new EventHandler(eBtnClick);
            this.btnWeSeangJochi1.Click += new EventHandler(eBtnClick);
            this.dtpWeSeangPanDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWeSeangPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWeSeangSogen.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWeSeangSogen.LostFocus += new EventHandler(eTxtLostFocus);
        }

        private void eTabClick(object sender, EventArgs e)
        {
            if (sender == tabEtc1)
            {
                for (int i = 0; i < FstrKind.Count; i++)
                {
                    if (FstrKind[i] == "62")
                    {
                        clsHcVariable.GnWRTNO = lstWrtno[i];
                        break;
                    }
                }
            }
            else if (sender == tabEtc2)
            {
                for (int i = 0; i < FstrKind.Count; i++)
                {
                    if (FstrKind[i] == "51")
                    {
                        clsHcVariable.GnWRTNO = lstWrtno[i];
                        break;
                    }
                }
            }
            else if (sender == tabEtc3)
            {
                for (int i = 0; i < FstrKind.Count; i++)
                {
                    if (FstrKind[i] == "50")
                    {
                        clsHcVariable.GnWRTNO = lstWrtno[i];
                        break;
                    }
                }
            }
            else if (sender == tabEtc4)
            {
                for (int i = 0; i < FstrKind.Count; i++)
                {
                    if (FstrKind[i] == "69")
                    {
                        clsHcVariable.GnWRTNO = lstWrtno[i];
                        break;
                    }
                }
            }
            else if (sender == tabEtc5)
            {
                for (int i = 0; i < FstrKind.Count; i++)
                {
                    if (FstrKind[i] == "54")
                    {
                        clsHcVariable.GnWRTNO = lstWrtno[i];
                        break;
                    }
                }
            }
        }

        void SetControl()
        {
            tabEtc1.Enabled = false;
            tabEtc2.Enabled = false;
            tabEtc3.Enabled = false;
            tabEtc4.Enabled = false;
            tabEtc5.Enabled = false;

            for (int i = 0; i < FstrKind.Count; i++)
            {
                switch (FstrKind[i])
                {
                    case "62":  //혈액종양검사
                        tabEtc1.Enabled = true;
                        break;
                    case "51":  //방사선종사자1차
                        tabEtc2.Enabled = true;
                        break;
                    case "50":  //방사선종사자2차
                        tabEtc3.Enabled = true;
                        break;
                    case "69":  //회사추가검진
                        tabEtc4.Enabled = true;
                        break;
                    case "54":  //위생
                        tabEtc5.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strJong = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_Var_Clear();
            fn_Bld_Screen_Clear();  
            fn_Xray1_Screen_Clear();
            fn_Xray2_Screen_Clear();
            fn_LtdAdd_Screen_Clear();
            fn_WeSeang_Screen_Clear();
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());
            //방사선과 코드를 건진코드로 변환 테이블 설정
            XRayCode_2_HicCode_SET();

            Combo.ComboPanjeng2_SET(cboXray1Panjeng);  //특수종합판정
            Combo.ComboPanjeng2_SET(cboXray2Panjeng);  //특수종합판정

            //생애 추가판정 제외 그룹코드 목록
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyGubun("HIC_추가판정제외코드");

            if (list.Count > 0)
            {
                strNotCode.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    strNotCode.Add(list[i].CODE);
                }
            }

            for (int i = 0; i < lstWrtno.Count; i++)
            {
                //삭제된것 체크
                if (hb.READ_JepsuSTS(lstWrtno[i]) == "D")
                {
                    MessageBox.Show(lstWrtno[i] + "접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (lstWrtno.Count > 0)
            {
                for (int i = 0; i < lstWrtno.Count; i++)
                {
                    switch (FstrKind[i])
                    {
                        case "62":  //혈액종양검사
                            tabEtc1.Enabled = true;
                            strJong = "BLOOD";
                            fn_Screen_Display(lstWrtno[i], strJong);
                            break;
                        case "51":  //방사선종사자1차
                            tabEtc2.Enabled = true;
                            strJong = "XRAY1";
                            fn_Screen_Display(lstWrtno[i], strJong);
                            break;
                        case "50":  //방사선종사자2차
                            tabEtc3.Enabled = true;
                            strJong = "XRAY2";
                            fn_Screen_Display(lstWrtno[i], strJong);
                            break;
                        case "69":  //회사추가검진
                            tabEtc4.Enabled = true;
                            strJong = "COMPANYADD";
                            fn_Screen_Display(lstWrtno[i], strJong);
                            break;
                        case "54":  //위생
                            tabEtc5.Enabled = true;
                            strJong = "WESEANG";
                            fn_Screen_Display(lstWrtno[i], strJong);
                            break;
                        default:
                            break;
                    }
                }
                //fn_Screen_Display(strJong);
            }
        }

        void fn_Screen_Display(long nWRTNO, string strId)
        {
            switch (strId)
            {
                case "BLOOD":
                    fn_Bld_Screen_Display(nWRTNO);
                    break;
                case "XRAY1":
                    fn_Xray_Screen_Display(nWRTNO, "1");
                    break;
                case "XRAY2":
                    fn_Xray_Screen_Display(nWRTNO, "2");
                    break;
                case "COMPANYADD":
                    fn_CompanyAdd_Screen_Display(nWRTNO);
                    break;
                case "WESEANG":
                    fn_WeSeang_Screen_Display(nWRTNO);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 혈액종양검사
        /// </summary>
        void fn_Bld_Screen_Display(long nWRTNO)
        {
            long nLicense = 0;
            int nGan1 = 0;
            int nGan2 = 0;
            int nREAD = 0;

            txtBldResult1.Text = "";
            txtBldResult2.Text = "";
            txtBldResult3.Text = "";

            tabBld2.Text = "";
            tabBld3.Text = "";

            //주민등록번호로 원무행정의 등록번호를 찾음
            HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

            if (!list2.IsNullOrEmpty())
            {
                FstrJumin = clsAES.DeAES(list2.JUMIN2);
                FstrPano = list2.PTNO.Trim();
            }

            //주민등록번호로 환자 등록번호 찾기
            if (FstrPano.IsNullOrEmpty())
            {
                FstrPano = basPatientService.GetPaNOByJuminNo(VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), clsAES.AES(VB.Right(FstrJumin, 7)));
            }

            //Screen_Exam_Items_display  '검사항목을 Display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(nWRTNO);

            nREAD = list3.Count;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;                //검사코드
                strResult = list3[i].RESULT.Trim();         //검사실 결과값
                strResCode = list3[i].RESCODE;              //결과값 코드
                strResultType = list3[i].RESULTTYPE;        //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE;          //결과값코드 사용여부

                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "061";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        //SS2.ActiveSheet.Cells[i, 1].Text = hb.READ_ResultName(strResCode, strResult);
                        if (!strResName.IsNullOrEmpty())
                        {
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                }
                else if (hf.GetLength(strResult) > 7)
                {
                    strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                    strRemark += strResName + "\r\n";
                }

                //if (list3[i].PANJENG.Trim() == "2")
                //{
                //    SS2.ActiveSheet.Cells[i, 2].Text = "*";
                //}

                //참고치를 Dispaly
                //strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F)

                //SS2.ActiveSheet.Cells[i, 3].Text = strNomal;
                //SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                //SS2.ActiveSheet.Cells[i, 8].Text = strResult; //정상값 점검용
                //strExPan = list3[i].PANJENG;
                //판정결과별 바탕색상을 다르게 표시함
                //switch (strExPan)
                //{
                //    case "B":
                //        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                //        break;
                //    case "R":
                //        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                //        break;
                //    default:
                //        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                //        break;
                //}

                //간염검사
                if (strExCode == "A131")     //간염항원
                {
                    nGan1 = strResult.To<int>();
                }
                else if (strExCode == "A132")     //간염항체
                {
                    nGan2 = strResult.To<int>();
                }
            }

            if (!strRemark.IsNullOrEmpty())
            {
                txtBldResult1.Text = strRemark;
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_ETC list4 = hicResEtcService.GetItembyWrtNo(nWRTNO, "1");

            if (list4 == null)
            {
                MessageBox.Show("접수번호 " + nWRTNO + " 는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            dtpBldPanDate.Text = "";
            if (!list4.PANJENGDATE.To<string>().IsNullOrEmpty())    //판정일자
            {
                dtpBldPanDate.Text = Convert.ToDateTime(list4.PANJENGDATE).ToString("yyyy-MM-dd");
            }

            //성인병일경우 이홍주, 공무원일경우 배삼덕 강제세팅
            switch (FstrJong)
            {
                case "성인병1차":
                case "성인병2차":
                    nLicense = 1809;
                    break;
                case "공무원1차":
                case "공무원2차":
                    nLicense = 10936;
                    break;
                default:
                    nLicense = list4.PANJENGDRNO;
                    break;
            }
            nLicense = list4.PANJENGDRNO;  //의사면허번호
            txtBldPanDrNo.Text = "";
            lblBldDrName.Text = "";
            if (nLicense > 0)
            {
                txtBldPanDrNo.Text = nLicense.To<string>();
                lblBldDrName.Text = hb.READ_License_DrName(txtBldPanDrNo.Text.To<long>());
            }
            else
            {
                txtBldPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblBldDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
            }

            if (list4.PANJENGDRNO > 0)
            {
                dtpBldPanDate.Text = list4.PANJENGDATE.To<string>();
            }
            else
            {
                dtpBldPanDate.Text = clsPublic.GstrSysDate;
            }

            //혈액종합판정 및 소견
            txtBldSogen.Text = list4.SOGEN;

            fn_Bld_OLD_Result_Display(FnPano, FstrJepDate, FstrSex);  //종전결과 3개를 Display

            tabBldControl.SelectedTab = tabBld1;
            //의사 이외 저장 금지
            if (clsHcVariable.GnHicLicense == 0) btnBldOK.Enabled = false;
        }

        /// <summary>
        /// 방사선종사자 1, 2차
        /// </summary>
        /// <param name="argChasu"></param>
        void fn_Xray_Screen_Display(long nWRTNO, string argChasu)
        {
            int nREAD = 0;

            string strSex = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
            string strRemark = "";
            long nLicense = 0;
            
            int nGan1 = 0;
            int nGan2 = 0;

            //방사선종사자1차
            if (argChasu == "1")
            {
                //Screen_Injek_display  
                //인적사항을 Display
                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(nWRTNO);

                if (list == null)
                {
                    MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strIpsadate = list.IPSADATE;
                FnPano = list.PANO;
                strSex = list.SEX;
                strJepDate = list.JEPDATE;
                clsHcVariable.GstrGjYear = list.GJYEAR;
                FstrSex = strSex;

                //주민등록번호로 원무행정의 등록번호를 찾음
                HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

                FstrJumin = "";
                FstrPano = "";

                if (list2 != null)
                {
                    FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
                    FstrPano = list2.PTNO;
                }

                //hm.ExamResult_RePanjeng(nWRTNO, FstrSex, strJepDate, ""); //검사결과를 재판정

                //Screen_Exam_Items_display  
                //검사항목을 Display
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(nWRTNO);

                nREAD = list3.Count;
                strRemark = "";
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list3[i].EXCODE;                    //검사코드
                    strResult = list3[i].RESULT;                    //검사실 결과값
                    strResCode = list3[i].RESCODE;                  //결과값 코드
                    strResultType = list3[i].RESULTTYPE;            //결과값 TYPE
                    strGbCodeUse = list3[i].GBCODEUSE;              //결과값코드 사용여부

                    //비만도
                    if (strExCode == "A103")
                    {
                        strResCode = "061";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            if (!strResName.IsNullOrEmpty())
                            {
                                if (hf.GetLength(strResName) > 7)
                                {
                                    strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                                    strRemark += strResName + "\r\n";
                                }
                            }
                        }
                    }
                    else if (hf.GetLength(strResult) > 7)
                    {
                        strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                        strRemark += strResName + "\r\n";
                    }

                    //간염검사
                    if (strExCode == "A131")     //간염항원
                    {
                        nGan1 = strResult.To<int>();
                    }
                    else if (strExCode == "A132")     //간염항체
                    {
                        nGan2 = strResult.To<int>();
                    }
                }

                //Screen_Munjin_Display
                //문진표를 Display
                int nJil = 0;
                int nGajok = 0;

                HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(nWRTNO);
                //HIC_RES_SPECIAL
                if (!item.IsNullOrEmpty()) 
                {
                    txtWorkDay1.Text = item.IPSADATE;
                }

                //건강검진 문진표 및 결과를  READ
                COMHPC list4 = comHpcLibBService.GetHic_X_MunjinbyWrtNo(nWRTNO);

                if (list4 == null)
                {
                    MessageBox.Show("접수번호 " + nWRTNO + " 는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //문진
                if (list4.XP1 == "Y")
                {
                    rdoXray11.Checked = true;
                }
                else if (list4.XP1 == "N")
                {
                    rdoXray12.Checked = true;
                }

                if (list4.JINGBN == "Y")
                {
                    rdoXray1Gubun1.Checked = true;
                }
                else if (list4.JINGBN == "N")
                {
                    rdoXray1Gubun2.Checked = true;
                }

                if (!list4.PAN .IsNullOrEmpty())
                {
                    cboXray1Panjeng.SelectedIndex = VB.Left(list4.PAN, 1).To<int>();
                }
                else
                {
                    cboXray1Panjeng.SelectedIndex = 0;
                }
                txtXray1SogenRemark.Text = list4.SOGEN;

                txtXray1XJong.Text = list4.XPJONG;
                txtXray1Remark.Text = list4.XREMARK;
                txtXray1Place.Text = list4.XPLACE;
                txtXray1Term.Text = list4.XTERM;
                txtXray1XTerm.Text = list4.XTERM1;
                txtXray1Much.Text = list4.XMUCH;
                txtXray1Jung.Text = list4.XJUNGSAN;
                txtXray1Mun1.Text = list4.MUN1;
                txtXray1Eye.Text = list4.JUNGSAN1;
                txtXray1Skin.Text = list4.JUNGSAN2;
                txtXray1Etc.Text = list4.JUNGSAN3;
                txtXray1PanDrNo1.Text = list4.MUNDRNO;
                lblXray1DrName1.Text = hb.READ_License_DrName(txtXray1PanDrNo1.Text.To<long>());

                //판정
                txtXray1JochiRemark.Text = list4.PANJENG;
                if (!list4.PANJENGDATE.To<string>().IsNullOrEmpty())
                {
                    dtpXray1PanDate.Text = list4.PANJENGDATE.To<string>();
                }
                else
                {
                    dtpXray1PanDate.Text = clsPublic.GstrSysDate;
                }
                nLicense = list4.PANJENGDRNO;               //의사면허번호
                txtXray1PanDrNo.Text = "";
                lblXray1DrName.Text = "";
                if (nLicense > 0)
                {
                    txtXray1PanDrNo.Text = nLicense.To<string>();
                    lblXray1DrName.Text = hb.READ_License_DrName(txtXray1PanDrNo.Text.To<long>());
                }
                else
                {
                    txtXray1PanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                    lblXray1DrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
                }

                //2013-11-25 방사선종사자 컬럼 추가
                nJil = list4.JILBYUNG.To<int>();             //과거 질병력
                if (nJil == 0)
                {
                    rdoXray1X11.Checked = true;
                }
                else
                {
                    rdoXray1X12.Checked = true;
                }

                chkXray1X1_11.Checked = list4.BLOOD1 == "1" ? true : false; //혈액관련질환(빈혈)
                chkXray1X1_12.Checked = list4.BLOOD2 == "1" ? true : false; //혈액관련질환(백혈병)
                if (!list4.BLOOD3.IsNullOrEmpty())       //혈액관련질환(기타)
                {
                    chkXray1X1_13.Checked = true;
                    txtXray1X1_1.Text = list4.BLOOD3;
                }
                chkXray1X1_21.Checked = list4.SKIN1 == "1" ? true : false;  //피부질환(아토피)
                chkXray1X1_22.Checked = list4.SKIN2 == "1" ? true : false;  //피부질환(습진)
                if (!list4.SKIN3.IsNullOrEmpty())        //피부질환(기타)
                {
                    chkXray1X1_23.Checked = true;
                    txtXray1X1_2.Text = list4.SKIN3;
                }
                txtXray1X1_3.Text = list4.NERVOUS1;                   //신경계질환명
                chkXray1X1_41.Checked = list4.EYE1 == "1" ? true : false;    //눈 질환(백내장)
                if (!list4.EYE2.IsNullOrEmpty())         //눈 질환(기타)
                {
                    chkXray1X1_42.Checked = true;
                    txtXray1X1_4.Text = list4.EYE2;
                }
                txtXray1X1_5.Text = list4.CANCER1;     //암 질환명

                nGajok = list4.GAJOK.To<int>();              //가족력

                if (nGajok == 0)
                {
                    rdoXray1X21.Checked = true;
                }
                else
                {
                    rdoXray1X22.Checked = true;
                }

                txtXray1X2_1.Text = list4.BLOOD;          //혈액관련질환명
                txtXray1X2_2.Text = list4.NERVOUS2;       //신경계질환명
                txtXray1X2_3.Text = list4.CANCER2;        //암 질환명
                txtXray1XSympton.Text = list4.SYMPTON;    //최근 특이증상

                chkXray1X41.Checked = list4.JIKJONG1 == "1" ? true : false;  //현재 직종(비파괴검사)
                chkXray1X42.Checked = list4.JIKJONG2 == "1" ? true : false;  //현재 직종(방사선사)
                if (!list4.JIKJONG3.IsNullOrEmpty())         //눈 질환(기타)
                {
                    chkXray1X43.Checked = true;
                    txtXray1Jikjong.Text = list4.JIKJONG3;
                }

                txtXray1Remark2.Text = list4.SANGDAM;
                txtXray1ReExam.Text = list4.REEXAM;

                //종전결과 3개를 Display
                fn_Xray1_OLD_Result_Display(FnPano, strJepDate, strSex);

                tabXray1MunjinControl.SelectedTab = tabXray1Mun1;
                tabXray1PanjengControl.SelectedTab = tabXray1Pan1;
            }
            else if (argChasu == "2") //방사선종사자2차
            {
                tabXray2MunjinControl.SelectedTab = tabXray2Mun2;
                tabXray2MunjinControl.Text = "";

                //Screen_Injek_display  
                //인적사항을 Display
                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(nWRTNO);

                if (list == null)
                {
                    MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strIpsadate = list.IPSADATE;
                FnPano = list.PANO;
                strSex = list.SEX;
                strJepDate = list.JEPDATE;
                clsHcVariable.GstrGjYear = list.GJYEAR;
                FstrSex = strSex;

                strGjJong = list.GJJONG;
                strIpsadate = list.IPSADATE;

                //주민등록번호로 원무행정의 등록번호를 찾음
                HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

                FstrJumin = "";
                FstrPano = "";

                if (!list2.IsNullOrEmpty())
                {
                    FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
                    FstrPano = list2.PTNO;
                }

                //hm.ExamResult_RePanjeng(FnWRTNO, FstrSex, strJepDate, ""); //검사결과를 재판정

                //Screen_Exam_Items_display  
                //검사항목을 Display
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(nWRTNO);

                nREAD = list3.Count;
                strRemark = "";
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list3[i].EXCODE;                    //검사코드
                    strResult = list3[i].RESULT;                    //검사실 결과값
                    strResCode = list3[i].RESCODE;                  //결과값 코드
                    strResultType = list3[i].RESULTTYPE;            //결과값 TYPE
                    strGbCodeUse = list3[i].GBCODEUSE;              //결과값코드 사용여부

                    //비만도
                    if (strExCode == "A103")
                    {
                        strResCode = "061";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                    else if (hf.GetLength(strResult) > 7)
                    {
                        strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                        strRemark += strResName + "\r\n";
                    }

                    //간염검사
                    if (strExCode == "A131")     //간염항원
                    {
                        nGan1 = strResult.To<int>();
                    }
                    else if (strExCode == "A132")     //간염항체
                    {
                        nGan2 = strResult.To<int>();
                    }
                }

                //Screen_Munjin_Display
                //문진표를 Display
                int nJil = 0;
                int nGajok = 0;

                //HIC_RES_SPECIAL
                HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(nWRTNO);
                //HIC_RES_SPECIAL
                if (!item.IsNullOrEmpty())
                {
                    txtWorkDay2.Text = item.IPSADATE;
                }

                //건강검진 문진표 및 결과를  READ
                COMHPC list4 = comHpcLibBService.GetHic_X_MunjinbyWrtNo(nWRTNO);

                if (list4 == null)
                {
                    MessageBox.Show("접수번호 " + nWRTNO + " 는 문진 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //문진
                if (list4.XP1 == "Y")
                {
                    rdoXray21.Checked = true;
                }
                else if (list4.XP1 == "N")
                {
                    rdoXray22.Checked = true;
                }

                if (list4.JINGBN == "Y")
                {
                    rdoXray2Gubun1.Checked = true;
                }
                else if (list4.JINGBN == "N")
                {
                    rdoXray2Gubun2.Checked = true;
                }

                if (!list4.PAN.IsNullOrEmpty())
                {
                    cboXray2Panjeng.SelectedIndex = VB.Left(list4.PAN.Trim(), 1).To<int>();
                }
                else
                {
                    cboXray2Panjeng.SelectedIndex = 0;
                }
                txtXray2SogenRemark.Text = list4.SOGEN.Trim();

                txtXray2XJong.Text = list4.XPJONG.Trim();
                txtXray2Remark.Text = list4.XREMARK.Trim();
                txtXray2Place.Text = list4.XPLACE.Trim();
                txtXray2Term.Text = list4.XTERM.Trim();
                txtXray2XTerm.Text = list4.XTERM1.Trim();
                txtXray2Much.Text = list4.XMUCH.Trim();
                txtXray2Jung.Text = list4.XJUNGSAN.Trim();
                txtXray2Mun1.Text = list4.MUN1.Trim();
                txtXray2Eye.Text = list4.JUNGSAN1.Trim();
                txtXray2Skin.Text = list4.JUNGSAN2.Trim();
                txtXray2Etc.Text = list4.JUNGSAN3.Trim();
                txtXray2PanDrNo1.Text = list4.MUNDRNO.Trim();
                lblXray2DrName1.Text = hb.READ_License_DrName(txtXray2PanDrNo1.Text.To<long>());

                txtXray2JobYN.Text = list4.WORKYN.Trim();
                txtXray2Sahu.Text = list4.SAHUCODE.Trim();
                lblXray2JobYN.Text = hb.READ_HIC_CODE("13", txtXray2JobYN.Text.Trim());     //업무접합성
                lblXray2SahuName.Text = hm.Sahu_Names_Display(txtXray2Sahu.Text.Trim());    //사후관리

                //판정
                txtXray2JochiRemark.Text = list4.PANJENG.Trim();
                if (!list4.PANJENGDATE.To<string>().IsNullOrEmpty())
                {
                    dtpXray2PanDate.Text = list4.PANJENGDATE.To<string>();
                }
                else
                {
                    dtpXray2PanDate.Text = clsPublic.GstrSysDate;
                }
                nLicense = list4.PANJENGDRNO;               //의사면허번호
                txtXray2PanDrNo.Text = "";
                lblXray2DrName.Text = "";
                if (nLicense > 0)
                {
                    txtXray2PanDrNo.Text = nLicense.To<string>();
                    lblXray2DrName.Text = hb.READ_License_DrName(txtXray2PanDrNo.Text.To<long>());
                }
                else
                {
                    txtXray2PanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                    lblXray2DrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
                }

                //2013-11-25 방사선종사자 컬럼 추가
                nJil = list4.JILBYUNG.To<int>();             //과거 질병력
                rdoXray2X11.Checked = true;

                chkXray2X1_11.Checked = list4.BLOOD1 == "1" ? true : false; //혈액관련질환(빈혈)
                chkXray2X1_12.Checked = list4.BLOOD2 == "1" ? true : false; //혈액관련질환(백혈병)
                if (!list4.BLOOD3.Trim().IsNullOrEmpty())       //혈액관련질환(기타)
                {
                    chkXray2X1_13.Checked = true;
                    txtXray2X1_1.Text = list4.BLOOD3.Trim();
                }
                chkXray2X1_21.Checked = list4.SKIN1 == "1" ? true : false;  //피부질환(아토피)
                chkXray2X1_22.Checked = list4.SKIN2 == "1" ? true : false;  //피부질환(습진)
                if (!list4.SKIN3.IsNullOrEmpty())        //피부질환(기타)
                {
                    chkXray2X1_23.Checked = true;
                    txtXray2X1_2.Text = list4.SKIN3.Trim();
                }
                txtXray2X1_3.Text = list4.NERVOUS1.Trim();                   //신경계질환명
                chkXray2X1_41.Checked = list4.EYE1 == "1" ? true : false;    //눈 질환(백내장)
                if (!list4.EYE2.IsNullOrEmpty())          //눈 질환(기타)
                {
                    chkXray2X1_42.Checked = true;
                    txtXray2X1_4.Text = list4.EYE2;
                }
                txtXray2X1_5.Text = list4.CANCER1;        //암 질환명

                nGajok = list4.GAJOK.To<int>();           //가족력
                rdoXray2X21.Checked = true;

                txtXray2X2_1.Text = list4.BLOOD;          //혈액관련질환명
                txtXray2X2_2.Text = list4.NERVOUS2;       //신경계질환명
                txtXray2X2_3.Text = list4.CANCER2;        //암 질환명
                txtXray2XSympton.Text = list4.SYMPTON;    //최근 특이증상

                chkXray2X41.Checked = list4.JIKJONG1 == "1" ? true : false;  //현재 직종(비파괴검사)
                chkXray2X42.Checked = list4.JIKJONG2 == "1" ? true : false;  //현재 직종(방사선사)
                if (!list4.JIKJONG3.IsNullOrEmpty())         //눈 질환(기타)
                {
                    chkXray2X43.Checked = true;
                    txtXray2Jikjong.Text = list4.JIKJONG3;
                }

                txtXray2Remark2.Text = list4.SANGDAM;

                //종전결과 3개를 Display
                fn_Xray2_OLD_Result_Display(FnPano, strJepDate, strSex);

                tabXray2MunjinControl.SelectedTab = tabXray2Mun1;
                tabXray2PanjengControl.SelectedTab = tabXray2Pan1;
            }
        }

        void fn_Xray1_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            //for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            //{
            //    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() != "")
            //    {
            //        strExamCode.Add(SS2.ActiveSheet.Cells[i, 7].Text.Trim());
            //    }
            //}

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDateGjJong(argPano, argJepDate);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE;
                
                fn_Xray1_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_Xray1_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;
            int nREAD = 0;
            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (hf.GetLength(strResult) > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                    }
                }
            }

        }

        void fn_Xray2_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDateGjJong(argPano, argJepDate);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE;
                
                fn_Xray2_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_Xray2_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;
            int nREAD = 0;
            //판정결과를 strRemark에 보관
            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                //for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                //{
                //    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() == strExCode)
                //    {
                //        nRow = j;
                //        break;
                //    }
                //}

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResName;
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (hf.GetLength(strResult) > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        //SS2.ActiveSheet.Cells[nRow, i + 10].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        //switch (strExPan)
                        //{
                        //    case "B":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                        //        break;
                        //    case "R":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                        //        break;
                        //    default:
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                        //        break;
                        //}
                    }
                }
            }

        }

        /// <summary>
        /// 회사추가검진(추가검사항목 및 판정 표시)
        /// </summary>
        void fn_CompanyAdd_Screen_Display(long nWRTNO)
        {
            string strIpsadate = "";
            string strSex = "";
            string strXrayCode = "";
            string strXrayRead = "";
            string strEndoGbn = "";

            int nGan1 = 0;
            int nGan2 = 0;
            int nREAD = 0;
            long nLicense = 0;

            txtAddResult1.Text = "";
            txtAddResult2.Text = "";
            txtAddResult3.Text = "";

            tabAddRslt.SelectedTab = tabAdd2;
            tabAdd2.Text = "";
            tabAddRslt.SelectedTab = tabAdd3;
            tabAdd3.Text = "";

            //Screen_Injek_display       //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(nWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strIpsadate = Convert.ToDateTime(list.IPSADATE).ToString("yyyy-MM-dd");
            FnPano = list.PANO;
            strSex = list.SEX;
            clsHcVariable.GstrGjYear = list.GJYEAR;
            FstrSex = strSex;
            strJepDate = list.JEPDATE;
            FstrPtno = list.PTNO.Trim();

            //주민등록번호로 환자 등록번호 찾기
            HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

            FstrJumin = "";
            FstrPano = "";

            if (list2 != null)
            {
                FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
                FstrPano = list2.PTNO;
            }

            //Screen_Exam_Items_display //검사항목을 Display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(nWRTNO);

            nREAD = list3.Count;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;                 //검사코드
                strResult = list3[i].RESULT;                 //검사실 결과값
                strResCode = list3[i].RESCODE;               //결과값 코드
                strResultType = list3[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE;           //결과값코드 사용여부
                strXrayCode = list3[i].XRAYCODE;             //방사선코드

                if (strEndoGbn != "OK")
                {
                    if (list3[i].ENDOGUBUN2 == "Y" || list3[i].ENDOGUBUN3 == "Y" || list3[i].ENDOGUBUN4 == "Y" || list3[i].ENDOGUBUN5 == "Y")
                    {
                        strEndoGbn = "Y";
                    }
                }
                
                strXrayRead = Read_XRay_Result(FstrPtno, FstrJepDate, strXrayCode, strExCode, ref strEndoGbn);

                if (!strXrayRead.IsNullOrEmpty())
                {
                    strRemark += "▷" + list3[i].HNAME.Trim() + ": ";
                    strRemark += strXrayRead;
                }
                else
                {
                    //비만도
                    if (strExCode == "A103")
                    {
                        strResCode = "061";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            //SS2.ActiveSheet.Cells[i, 1].Text = hb.READ_ResultName(strResCode, strResult);
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                    else if (hf.GetLength(strResult) > 7)
                    {
                        strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                        strRemark += strResult + "\r\n";
                    }
                }

                strExPan = list3[i].PANJENG;
                
                //간염검사
                if (strExCode == "A131")     //간염항원
                {
                    nGan1 = strResult.To<int>();
                }
                else if (strExCode == "A132")     //간염항체
                {
                    nGan2 = strResult.To<int>();
                }
            }

            if (!strRemark.IsNullOrEmpty())
            {
                txtAddResult1.Text = strRemark;
            }

            //건강검진 문진표 및 결과를  READ
            if (list.GBADDPAN == "Y")
            {
                HIC_RES_ETC_BOHUM1 list4 = hicResEtcBohum1Service.GetItembyWrtNo(nWRTNO);

                if (list4.IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호 " + nWRTNO + " 는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //----------( 판정일자,판정의사,판정결과 )---------------------------------
                dtpAddPanDate.Text = "";
                if (!list4.PANJENGDATE.IsNullOrEmpty())    //판정일자
                {
                    dtpAddPanDate.Text = list4.PANJENGDATE;
                }

                //성인병일경우 이홍주, 공무원일경우 배삼덕 강제세팅
                switch (FstrJong)
                {
                    case "성인병1차":
                    case "성인병2차":
                        nLicense = 1809;
                        break;
                    case "공무원1차":
                    case "공무원2차":
                        nLicense = 10936;
                        break;
                    default:
                        nLicense = list4.PANJENGDRNO;
                        break;
                }
                nLicense = list4.PANJENGDRNO1;  //의사면허번호
                txtAddPanDrNo.Text = "";
                lblAddDrName.Text = "";
                if (nLicense > 0)
                {
                    txtAddPanDrNo.Text = nLicense.To<string>();
                    lblAddDrName.Text = hb.READ_License_DrName(txtAddPanDrNo.Text.To<long>());
                }
                else
                {
                    txtAddPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                    lblAddDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
                }

                if (list4.PANJENGDRNO > 0)
                {
                    dtpAddPanDate.Text = list4.PANJENGDATE;
                }
                else
                {
                    dtpAddPanDate.Text = clsPublic.GstrSysDate;
                }

                //판정완료자는 판정한 의사만 변경이 가능함
                pnlPanjeng.Enabled = false;
                if (nLicense == 0 || (nLicense == clsHcVariable.GnHicLicense && clsHcVariable.GnHicLicense > 0))
                {
                    pnlPanjeng.Enabled = true;
                }

                //혈액종합판정 및 소견
                txtAddSogen.Text = list4.ADDSO;
            }
            
            //종전결과 3개를 Display
            fn_OLD_Result_Display(FnPano, strJepDate, strSex);

            tabAddRslt.SelectedTab = tabAdd1;
        }
        void fn_WeSeang_Screen_Display(long nWRTNO)
        {
            long nLicense = 0;

            //Screen_Injek_display       //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(nWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            hb.HIC_RES_ETC_INSERT(nWRTNO, "4", "54");

           
            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            dtpWeSeangPanDate.Text = "";
            txtWeSeangPanDrNo.Text = "";
            lblWeSeangDrName.Text = "";

            HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(nWRTNO, "4");

            if (item.PANJENGDATE.IsNullOrEmpty())
            {
                dtpWeSeangPanDate.Text = clsPublic.GstrSysDate;
            }
            else
            {
                dtpWeSeangPanDate.Text = item.PANJENGDATE;
            }

            if (item.PANJENGDRNO > 0)
            {
                txtWeSeangPanDrNo.Text = item.PANJENGDRNO.ToString();
                lblWeSeangDrName.Text = hb.READ_License_DrName(item.PANJENGDRNO);
            }
            else
            {
                txtWeSeangPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblWeSeangDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
            }

            //판정완료자는 판정한 의사만 변경이 가능함
            pnlPanjeng.Enabled = false;
            if (nLicense == 0 || (nLicense == clsHcVariable.GnHicLicense && clsHcVariable.GnHicLicense > 0))
            {
                pnlPanjeng.Enabled = true;
            }

            txtWeSeangSogen.Text = item.SOGEN;

        }

        private string Read_XRay_Result(string fstrPtno, string fstrJepDate, string strXrayCode, string strExCode, ref string strEndoGbn)
        {
            string rtnVal = "";
            int nREAD = 0;
            string strGbJob = "";
            string strResult1 = "";
            string strResult2 = "";
            string strResult3 = "";
            string strResult4 = "";
            string strResult5 = "";
            string strResult6 = "";
            string strResult6_2 = "";
            string strResult6_3 = "";
            string strRemark = "";
            string strNew = "";

            string strInfo = "";
            string strRESULTDATE = "";
            string strResultDrCode = "";
            string strKorName = "";

            List<XRAY_RESULTNEW> list1 = xrayResultnewService.GetItembyPaNoSeekDateXCode(fstrPtno, fstrJepDate, strXrayCode);

            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    rtnVal += list1[i].RESULT.To<string>("").Trim() + "\r\n";
                }
            }

            //분진(Chest-Dust) 판독 결과
            List<HIC_XRAY_RESULT> list4 = hicXrayResultService.GetListItemByPtnoJepDate(fstrPtno, fstrJepDate, strExCode);
            nREAD = list4.Count;

            for (int i = 0; i < nREAD; i++)
            {
                if (list4[i].RESULT1.To<string>("").Trim() == "")
                {
                    strResult = "";
                }
                else
                {
                    strResult = "[" + cf.Read_SabunName(clsDB.DbCon, list4[i].READDOCT1.To<string>("")) + "] ☞ ";
                    strResult += "판독분류: " + list4[i].RESULT2.To<string>("") + "\r\n";
                    strResult += "판독분류명: " + list4[i].RESULT3.To<string>("") + "\r\n";
                    strResult += "판독소견: " + list4[i].RESULT4.To<string>("") + "\r\n";

                    rtnVal += strResult;
                }
            }

            //내시경코드 
            if (strEndoGbn == "Y")
            {
                List<ENDO_RESULT_JUPMST> list = comHpcLibBService.GetItembyJDateDeptPtno(fstrJepDate, fstrPtno);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strGbJob = list[i].GBJOB;
                    strNew = list[i].GBNEW.To<string>("").Trim();
                    strResultDrCode = list[i].RESULTDRCODE.To<string>("").Trim();

                    //결과입력의사명
                    if (!strResultDrCode.IsNullOrEmpty())
                    {
                        strKorName = cf.Read_SabunName(clsDB.DbCon, strResultDrCode);
                    }

                    strResult1 = list[i].REMARK1.To<string>("").Trim();
                    strResult2 = list[i].REMARK2.To<string>("").Trim();
                    strResult3 = list[i].REMARK3.To<string>("").Trim();
                    strResult4 = list[i].REMARK4.To<string>("").Trim();
                    strResult5 = list[i].REMARK5.To<string>("").Trim();
                    strResult6 = list[i].REMARK6.To<string>("").Trim(); //Biposy

                    if (strNew == "Y")
                    {
                        strResult6_2 = list[i].REMARK6_2.To<string>("").Trim();
                        strResult6_3 = list[i].REMARK6_3.To<string>("").Trim();
                        strRemark = list[i].REMARK.To<string>("").Trim();
                    }

                    //Premedication--------------------------------------------------------------------------
                    strInfo = "▶Premedication:" + "\r\n";
                    strInfo += list[i].GBPRE_1.To<string>("").Trim() == "Y" ? "None" : "";
                    strInfo += list[i].GBPRE_2.To<string>("").Trim() == "Y" ? "Aigiron " : "";

                    if (list[i].GBPRE_21.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBPRE_21 + "mg " + list[i].GBPRE_22 + ", ";
                    }

                    //Conscious Sedation---------------------------------------------------------------------
                    strInfo += "\r\n" + "▶Conscious Sedation:" + "\r\n";
                    strInfo += list[i].GBCON_1.To<string>("").Trim() == "Y" ? "None" : "";
                    strInfo += list[i].GBCON_2.To<string>("").Trim() == "Y" ? "Mediazolam " : "";

                    if (list[i].GBCON_21.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_21 + "mg " + list[i].GBCON_22 + ", ";
                    }

                    strInfo += list[i].GBCON_3.To<string>("").Trim() == "Y" ? "Anepol " : "";

                    if (list[i].GBCON_31.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_31 + "mg " + list[i].GBCON_32 + ", ";
                    }

                    strInfo += list[i].GBCON_4.To<string>("").Trim() == "Y" ? "Pathidine " : "";

                    if (list[i].GBCON_41.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_41 + "mg " + list[i].GBCON_42 + ", ";
                    }

                    switch (strGbJob)
                    {
                        case "1":   //기관지
                            strResult = "▶Vocal Cord:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            strResult += "▶Carina:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            strResult += "▶Bronchi:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            strResult += "▶EndoScopic Procedure:" + Environment.NewLine + strResult4;
                            strResult += "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6;
                            if (strNew == "Y")
                            {
                                strResult = strResult + Environment.NewLine + strResult6_2;
                                strResult = strResult + Environment.NewLine + strResult6_3 + Environment.NewLine;
                                strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "2":   //위
                            if (strNew == "Y")
                            {
                                if (list[i].REMARK6.To<string>("").Trim() != "")
                                {
                                    strResult6 = "Esophagus:" + list[i].REMARK6;
                                }
                                if (list[i].REMARK6_2.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "Stomach:" + list[i].REMARK6_2;
                                }
                                if (list[i].REMARK6_3.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "Duodenum:" + list[i].REMARK6_3;
                                }
                            }

                            if (strResult1 != "")
                            {
                                strResult = "▶Esophagus:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = "▶Esophagus:" + Environment.NewLine + strResult1;
                            }
                            if (strResult2 != "")
                            {
                                strResult = strResult + "▶Stomach:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Stomach:" + Environment.NewLine + strResult2;
                            }
                            if (strResult3 != "")
                            {
                                strResult = strResult + "▶Duodenum:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Duodenum:" + Environment.NewLine + strResult3;
                            }
                            if (strResult4 != "")
                            {
                                strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult4 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult4;
                            }
                            strResult = strResult + strInfo + Environment.NewLine; //add


                            if (list[i].PRO_RUT.To<string>("").Trim() == "Y")
                            {
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + "Rapid Urease Test, " + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + "Rapid Urease Test, " + Environment.NewLine + strResult5;
                                }
                            }
                            else
                            {
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult5;
                                }
                            }
                            strResult = strResult + "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6 + Environment.NewLine;


                            if (strNew == "Y")
                            {
                                //참고사항
                                if (strRemark != "")
                                {
                                    strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                }
                                strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "3":   //장
                            if (strNew == "Y")
                            {
                                strResult6 = "";
                                if (list[i].REMARK6.To<string>("").Trim() != "")
                                {
                                    strResult6 = "small Intestinal:" + list[i].REMARK6;
                                }
                                if (list[i].REMARK6_2.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "large Intestinal:" + list[i].REMARK6_2;
                                }
                                if (list[i].REMARK6_3.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "rectum:" + list[i].REMARK6_3;
                                }

                                if (strResult1 != "")
                                {
                                    strResult = strResult + "▶small Intestinal:" + Environment.NewLine + strResult1 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶small Intestinal:" + Environment.NewLine + strResult1;
                                }
                                if (strResult4 != "")
                                {
                                    strResult = strResult + "▶large Intestinal:" + Environment.NewLine + strResult4 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶large Intestinal:" + Environment.NewLine + strResult4;
                                }
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶rectum:" + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶rectum:" + Environment.NewLine + strResult5;
                                }
                                if (strResult2 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult2 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult2;
                                }
                                if (list[i].GB_CLEAN.To<string>("").Trim() != "")
                                {
                                    strResult = strResult + "▶장정결도:" + Environment.NewLine + list[i].GB_CLEAN + Environment.NewLine;   //2013-06-17
                                }
                                else
                                {
                                    strResult = strResult + "▶장정결도:" + Environment.NewLine + list[i].GB_CLEAN;    //2013-06-17
                                }
                                strResult = strResult + strInfo + Environment.NewLine;   //add
                                if (strResult3 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult3 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult3;
                                }
                                if (strResult6 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Biopsy:" + Environment.NewLine + strResult6 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Biopsy:" + Environment.NewLine + strResult6;
                                }
                                if (strNew == "Y")
                                {
                                    //참고사항
                                    if (strRemark != "")
                                    {
                                        strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                    }
                                    else
                                    {
                                        strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                    }
                                    strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                                }
                            }
                            break;
                        case "4":   // ERCP
                            strResult = "▶ERCP Finding:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            strResult += "▶Diagnosis:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            strResult += "▶Plan + Tx:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            strResult += "▶EndoScopic Procedure:" + Environment.NewLine + strResult4;
                            strResult += "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6;
                            if (strNew == "Y")
                            {
                                strResult += Environment.NewLine + strResult6_2;
                                strResult += Environment.NewLine + strResult6_3 + Environment.NewLine;
                                strResult += "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        default:
                            break;
                    }

                    strResult = fn_BlankLine_Delete(strResult);
                }

                rtnVal += strResult;

                strEndoGbn = "OK";
            }

            return rtnVal;
        }

        string fn_BlankLine_Delete(string argData)
        {
            string rtnVal = "";
            string strResult = "";
            int nPos1 = 0;
            int nPos2 = 0;
            int nPos3 = 0;
            int nPos4 = 0;
            int nPos5 = 0;

            strResult = argData;

            do
            {
                nPos1 = VB.InStr(strResult, Keys.Tab.ToString());
                if (nPos1 > 0) strResult = strResult.Replace(Keys.Tab.ToString(), " ");
                nPos2 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos2 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos3 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos3 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos4 = VB.InStr(strResult, Environment.NewLine + " " + Environment.NewLine);
                if (nPos4 > 0) strResult = strResult.Replace(Environment.NewLine + " " + Environment.NewLine, Environment.NewLine);
                nPos5 = VB.InStr(strResult, "  ");
                if (nPos5 > 0) strResult = strResult.Replace("  ", " ");
            }
            while (nPos1 != 0 && nPos2 != 0 && nPos3 != 0 && nPos4 != 0 && nPos5 != 0);

            rtnVal = strResult;

            return rtnVal;
        }

        void fn_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDate(argPano, argJepDate);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE;
                tabAddRslt.SelectedTabIndex = i + 1;
                tabAddRslt.SelectedTab.Text = strJepDate;
                fn_Add_OLD_Panjeng_Display(i, list[i].WRTNO);
                fn_Add_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }
        }

        void fn_Add_OLD_Panjeng_Display(int argNo, long argWrtNo)
        {
            string strPan = "";
            string strPAN1 = "";

            if (argNo == 0)
            {
                txtAddResult2.Text = "";
            }
            else
            {
                txtAddResult3.Text = "";
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(argWrtNo);

            if (list == null)
            {
                return;
            }

            //판정결과,판정일자,판정의사
            strPan = "▶판정결과:";
            switch (list.PANJENG)
            {
                case "1":
                    strPan += "정상A";
                    break;
                case "2":
                    strPan += "정상B";
                    break;
                case "3":
                    strPan += "질환의심(R)";
                    break;
                case "5":
                    strPan += "정상B+질환의심";
                    break;
                default:
                    strPan += "<오류>";
                    break;
            }
            strPan += " ○판정일자:" + list.PANJENGDATE;
            strPan += " ○판정의사:" + hb.READ_License_DrName(list.PANJENGDRNO) + "\r\n";

            //판정(B)
            strPAN1 = "";

            if (list.PANJENGB1 == "1")
            {
                strPAN1 += "◎비만관리 ";
            }
            if (list.PANJENGB2 == "1")
            {
                strPAN1 += "◎혈압관리 ";
            }
            if (list.PANJENGB3 == "1")
            {
                strPAN1 += "◎콜레스테롤관리 ";
            }
            if (list.PANJENGB4 == "1")
            {
                strPAN1 += "◎간기능관리 ";
            }
            if (list.PANJENGB5 == "1")
            {
                strPAN1 += "◎당뇨관리 ";
            }
            if (list.PANJENGB6 == "1")
            {
                strPAN1 += "◎신장기능관리 ";
            }
            if (list.PANJENGB7 == "1")
            {
                strPAN1 += "◎빈혈관리 ";
            }
            if (list.PANJENGB8 == "1")
            {
                strPAN1 += "◎부인과질환관리 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(정상B): " + strPAN1 + "\r\n";
            }

            //판정(R)
            strPAN1 = "";
            if (list.PANJENGR1 == "1")
            {
                strPAN1 += "◎폐결핵의심 ";
            }
            if (list.PANJENGR2 == "1")
            {
                strPAN1 += "◎기타흉부질환의심 ";
            }
            if (list.PANJENGR3 == "1")
            {
                strPAN1 += "◎고혈압의심 ";
            }
            if (list.PANJENGR4 == "1")
            {
                strPAN1 += "◎고지혈증의심 ";
            }
            if (list.PANJENGR5 == "1")
            {
                strPAN1 += "◎간장질환의심 ";
            }
            if (list.PANJENGR6 == "1")
            {
                strPAN1 += "◎당뇨질환의심 ";
            }
            if (list.PANJENGR7 == "1")
            {
                strPAN1 += "◎신장질환의심 ";
            }
            if (list.PANJENGR8 == "1")
            {
                strPAN1 += "◎빈혈증의심 ";
            }
            if (list.PANJENGR9 == "1")
            {
                strPAN1 += "◎부인과질환의심 ";
            }
            if (list.PANJENGR10 == "1")
            {
                strPAN1 += "◎자궁경부암의심 ";
            }
            if (list.PANJENGR11 == "1")
            {
                strPAN1 += "◎기타질환의심 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(질환의심): " + strPAN1 + "\r\n";
            }

            //소견 및 조치사항
            if (!list.SOGEN.IsNullOrEmpty())
            {
                strPan += "▶소견및조치사항: " + list.SOGEN.Trim() + "\r\n";
            }
            //간염검사
            if (!list.LIVER3.IsNullOrEmpty())
            {
                switch (list.LIVER3.Trim())
                {
                    case "1":
                        strPan += "▶간염검사: 보균자" + "\r\n";
                        break;
                    case "2":
                        strPan += "▶간염검사: 면역자" + "\r\n";
                        break;
                    case "3":
                        strPan += "▶간염검사: 접종대상자" + "\r\n";
                        break;
                    default:
                        break;
                }
            }
            //자궁경부 선상피세포 유/무
            if (list.WOMB02 == "1")
            {
                strPan += "▶자궁경부 선상피세표 있음" + "\r\n";
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_Add_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;
            int nREAD = 0;

            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);
            //판정결과를 strRemark에 보관            
            if (index == 0)
            {
                strRemark = txtAddResult2.Text + "\r\n";
            }
            else if (index == 1)
            {
                strRemark = txtAddResult3.Text + "\r\n";
            }

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                //for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                //{
                //    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() == strExCode)
                //    {
                //        nRow = j;
                //        break;
                //    }
                //}

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResName;
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (hf.GetLength(strResult) > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        //SS2.ActiveSheet.Cells[nRow, i + 10].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        //switch (strExPan)
                        //{
                        //    case "B":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                        //        break;
                        //    case "R":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                        //        break;
                        //    default:
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                        //        break;
                        //}
                    }
                }
            }

            if (index == 0)
            {
                txtAddResult2.Text = strRemark;
            }
            else
            {
                txtAddResult3.Text = strRemark;
            }
        }

        void fn_Bld_OLD_Result_Display(long ArgPano, string ArgJepDate, string ArgSex)
        {
            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDate(ArgPano, ArgJepDate, "Y");

            nOldCNT = list.Count;
            strAllWRTNO = "";

            for (int i = 0; i < nOldCNT; i++)
            {
                if (i >= 2) break;
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE;
                tabBldControl.SelectedTabIndex = i + 1;
                tabBldControl.SelectedTab.Text = strJepDate;
                fn_Bld_OLD_Panjeng_Display(i, list[i].WRTNO);
                fn_Bld_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, ArgSex, i);
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_Bld_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;
            int nREAD = 0;

            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);
            //판정결과를 strRemark에 보관            
            if (index == 0)
            {
                strRemark = txtBldResult2.Text + "\r\n";
            }
            else if (index == 1)
            {
                strRemark = txtBldResult3.Text + "\r\n";
            }

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                //for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                //{
                //    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() == strExCode)
                //    {
                //        nRow = j;
                //        break;
                //    }
                //}

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            //SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResName;
                            if (hf.GetLength(strResName) > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (hf.GetLength(strResult) > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        //SS2.ActiveSheet.Cells[nRow, i + 10].Text = strResult;   //정상값 점검용
                        //strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        //switch (strExPan)
                        //{
                        //    case "B":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                        //        break;
                        //    case "R":
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                        //        break;
                        //    default:
                        //        SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                        //        break;
                        //}
                    }
                }
            }

            if (index == 0)
            {
                txtBldResult2.Text = strRemark;
            }
            else
            {
                txtBldResult3.Text = strRemark;
            }
        }

        void fn_Bld_OLD_Panjeng_Display(int argNo, long argWrtNo)
        {
            string strPan = "";
            string strPAN1 = "";

            if (argNo == 0)
            {
                txtBldResult2.Text = "";
            }
            else
            {
                txtBldResult3.Text = "";
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(argWrtNo);

            if (list == null)
            {
                return;
            }

            //판정결과,판정일자,판정의사
            strPan = "▶판정결과:";
            switch (list.PANJENG)
            {
                case "1":
                    strPan += "정상A";
                    break;
                case "2":
                    strPan += "정상B";
                    break;
                case "3":
                    strPan += "질환의심(R)";
                    break;
                case "5":
                    strPan += "정상B+질환의심";
                    break;
                default:
                    strPan += "<오류>";
                    break;
            }
            strPan += " ○판정일자:" + list.PANJENGDATE;
            strPan += "  ○판정의사:" + hb.READ_License_DrName(list.PANJENGDRNO) + "\r\n";

            //판정(B)
            strPAN1 = "";

            if (list.PANJENGB1 == "1")
            {
                strPAN1 += "◎비만관리 ";
            }
            if (list.PANJENGB2 == "1")
            {
                strPAN1 += "◎혈압관리 ";
            }
            if (list.PANJENGB3 == "1")
            {
                strPAN1 += "◎콜레스테롤관리 ";
            }
            if (list.PANJENGB4 == "1")
            {
                strPAN1 += "◎간기능관리 ";
            }
            if (list.PANJENGB5 == "1")
            {
                strPAN1 += "◎당뇨관리 ";
            }
            if (list.PANJENGB6 == "1")
            {
                strPAN1 += "◎신장기능관리 ";
            }
            if (list.PANJENGB7 == "1")
            {
                strPAN1 += "◎빈혈관리 ";
            }
            if (list.PANJENGB8 == "1")
            {
                strPAN1 += "◎부인과질환관리 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(정상B): " + strPAN1 + "\r\n";
            }

            //판정(R)
            strPAN1 = "";
            if (list.PANJENGR1 == "1")
            {
                strPAN1 += "◎폐결핵의심 ";
            }
            if (list.PANJENGR2 == "1")
            {
                strPAN1 += "◎기타흉부질환의심 ";
            }
            if (list.PANJENGR3 == "1")
            {
                strPAN1 += "◎고혈압의심 ";
            }
            if (list.PANJENGR4 == "1")
            {
                strPAN1 += "◎고지혈증의심 ";
            }
            if (list.PANJENGR5 == "1")
            {
                strPAN1 += "◎간장질환의심 ";
            }
            if (list.PANJENGR6 == "1")
            {
                strPAN1 += "◎당뇨질환의심 ";
            }
            if (list.PANJENGR7 == "1")
            {
                strPAN1 += "◎신장질환의심 ";
            }
            if (list.PANJENGR8 == "1")
            {
                strPAN1 += "◎빈혈증의심 ";
            }
            if (list.PANJENGR9 == "1")
            {
                strPAN1 += "◎부인과질환의심 ";
            }
            if (list.PANJENGR10 == "1")
            {
                strPAN1 += "◎자궁경부암의심 ";
            }
            if (list.PANJENGR11 == "1")
            {
                strPAN1 += "◎기타질환의심 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(질환의심): " + strPAN1 + "\r\n";
            }

            //소견 및 조치사항
            if (!list.SOGEN.IsNullOrEmpty())
            {
                strPan += "▶소견및조치사항: " + list.SOGEN.Trim() + "\r\n";
            }
            //간염검사
            if (!list.LIVER3.IsNullOrEmpty())
            {
                switch (list.LIVER3.Trim())
                {
                    case "1":
                        strPan += "▶간염검사: 보균자" + "\r\n";
                        break;
                    case "2":
                        strPan += "▶간염검사: 면역자" + "\r\n";
                        break;
                    case "3":
                        strPan += "▶간염검사: 접종대상자" + "\r\n";
                        break;
                    default:
                        break;
                }
            }
            //자궁경부 선상피세포 유/무
            if (list.WOMB02 == "1")
            {
                strPan += "▶자궁경부 선상피세표 있음" + "\r\n";
            }

            if (argNo == 0)
            {
                txtBldResult2.Text = strPan;
            }
            else
            {
                txtBldResult3.Text = strPan;
            }
        }

        void fn_Var_Clear()
        {
            FstrJumin = "";
            FnPano = 0;
        }

        /// <summary>
        /// 혈액종양 판정 Clear
        /// </summary>
        void fn_Bld_Screen_Clear()
        {
            dtpBldPanDate.Text = "";
            txtBldResult1.Text = "";
            txtBldResult2.Text = "";
            txtBldResult3.Text = "";
            txtBldSogen.Text = "";

            txtBldPanDrNo.Text = "";
            lblBldDrName.Text = "";
        }

        /// <summary>
        /// 방사선종사자 1차 Clear
        /// </summary>
        void fn_Xray1_Screen_Clear()
        {
            dtpXray1PanDate.Text = "";
            
            //문진
            txtXray1Place.Text = "";
            txtXray1Remark.Text = "";
            txtXray1Much.Text = "";
            txtXray1Jung.Text = "";
            txtXray1Eye.Text = "";
            txtXray1Skin.Text = "";
            txtXray1Etc.Text = "";
            txtXray1Term.Text = "";
            rdoXray11.Checked = false;
            rdoXray12.Checked = false;
            txtXray1XJong.Text = "";
            txtXray1XTerm.Text = "";
            txtWorkDay1.Text = "";
            txtXray1Mun1.Text = "";
            txtXray1XJong.Text = "";
            rdoXray1Gubun1.Checked = false;
            rdoXray1Gubun2.Checked = false;
            txtXray1SogenRemark.Text = "";
            txtXray1JochiRemark.Text = "";

            txtXray1PanDrNo.Text = "";
            lblXray1DrName.Text = "";

            //방사선종사자 상담부분 추가항목
            tabXray1MunjinControl.SelectedTab = tabXray1Mun1;
            tabXray1PanjengControl.SelectedTab = tabXray1Pan1;

            rdoXray1X11.Checked = true;

            chkXray1X1_11.Checked = false;
            chkXray1X1_12.Checked = false;
            chkXray1X1_13.Checked = false;

            chkXray1X1_21.Checked = false;
            chkXray1X1_22.Checked = false;
            chkXray1X1_23.Checked = false;

            chkXray1X1_41.Checked = false;
            chkXray1X1_42.Checked = false;

            txtXray1X1_1.Text = "";
            txtXray1X1_2.Text = "";
            txtXray1X1_3.Text = "";
            txtXray1X1_4.Text = "";
            txtXray1X1_5.Text = "";

            txtXray1X1_1.Enabled = false;
            txtXray1X1_2.Enabled = false;
            txtXray1X1_4.Enabled = false;
            txtXray1Jikjong.Enabled = false;

            txtXray1X2_1.Text = "";
            txtXray1X2_2.Text = "";
            txtXray1X2_3.Text = "";

            rdoXray1X21.Checked = true;
            txtXray1XSympton.Text = "";

            chkXray1X41.Checked = false;
            chkXray1X42.Checked = false;
            chkXray1X43.Checked = false;
            txtXray1Jikjong.Text = "";
            txtXray1Remark2.Text = "";

            txtXray1ReExam.Text = "";
            lblXray1ReExamName.Text = "";
        }

        /// <summary>
        /// 방사선종사자 2차 Clear
        /// </summary>
        void fn_Xray2_Screen_Clear()
        {
            dtpXray2PanDate.Text = "";

            txtXray2JobYN.Text = "";
            txtXray2Sahu.Text = "";
            lblXray2JobYN.Text = "";
            lblXray2SahuName.Text = "";

            //문진
            txtXray2Place.Text = "";
            txtXray2Remark.Text = "";
            txtXray2Much.Text = "";
            txtXray2Jung.Text = "";
            txtXray2Eye.Text = "";
            txtXray2Skin.Text = "";
            txtXray2Etc.Text = "";
            txtXray2Term.Text = "";
            rdoXray21.Checked = false;
            rdoXray22.Checked = false;
            txtXray2XJong.Text = "";
            txtXray2XTerm.Text = "";
            txtWorkDay2.Text = "";
            txtXray2Mun1.Text = "";
            txtXray2XJong.Text = "";
            rdoXray2Gubun1.Checked = false;
            rdoXray2Gubun2.Checked = false;
            txtXray2SogenRemark.Text = "";
            txtXray2JochiRemark.Text = "";

            txtXray2PanDrNo.Text = "";
            lblXray2DrName.Text = "";

            //방사선종사자 상담부분 추가항목
            tabXray2PanjengControl.SelectedTab = tabXray2Pan1;
            tabXray2MunjinControl.SelectedTab = tabXray2Mun1;

            rdoXray2X11.Checked = true;

            chkXray2X1_11.Checked = false;
            chkXray2X1_12.Checked = false;
            chkXray2X1_13.Checked = false;

            chkXray2X1_21.Checked = false;
            chkXray2X1_22.Checked = false;
            chkXray2X1_23.Checked = false;

            chkXray2X1_41.Checked = false;
            chkXray2X1_42.Checked = false;

            txtXray2X1_1.Text = "";
            txtXray2X1_2.Text = "";
            txtXray2X1_3.Text = "";
            txtXray2X1_4.Text = "";
            txtXray2X1_5.Text = "";

            txtXray2X1_1.Enabled = false;
            txtXray2X1_2.Enabled = false;
            txtXray2X1_4.Enabled = false;
            txtXray2Jikjong.Enabled = false;

            txtXray2X2_1.Text = "";
            txtXray2X2_2.Text = "";
            txtXray2X2_3.Text = "";

            rdoXray2X21.Checked = true;
            txtXray2XSympton.Text = "";

            chkXray2X41.Checked = false;
            chkXray2X42.Checked = false;
            chkXray2X43.Checked = false;
            txtXray2Jikjong.Text = "";
            txtXray2Remark2.Text = "";
        }

        void fn_LtdAdd_Screen_Clear()
        {
            dtpAddPanDate.Text = "";
            txtAddSogen.Text = "";
            txtAddResult1.Text = "";
            txtAddResult2.Text = "";
            txtAddResult3.Text = "";

            txtAddPanDrNo.Text = "";
            lblAddDrName.Text = "";
        }

        void fn_WeSeang_Screen_Clear()
        {
            dtpWeSeangPanDate.Text = "";
            txtWeSeangSogen.Text = "";

            txtWeSeangPanDrNo.Text = "";
            lblWeSeangDrName.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnBldAutoPan)
            {
                if (clsHcVariable.GnWRTNO == 0) return;

                txtBldSogen.Text += "\r\n" + "\r\n" + hm.ReadAutoPan(clsHcVariable.GnWRTNO.To<string>(""));
            }
            else if (sender == btnBldJochi2)
            {
                FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
                FrmHcPanJochiHelp.ShowDialog(this);
                FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
                //FrmHcPanJochiHelp = null;
                //FrmHcPanJochiHelp.Dispose();
            }
            else if (sender == btnAddJochi1)
            {
                FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick2);
                FrmHcPanJochiHelp.ShowDialog(this);
                FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick2);
                //FrmHcPanJochiHelp = null;
                //FrmHcPanJochiHelp.Dispose();
            }
            else if (sender == btnWeSeangJochi1)
            {
                //FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                //FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick2);
                //FrmHcPanJochiHelp.ShowDialog(this);
                //FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick2);
                //FrmHcPanJochiHelp = null;
                //FrmHcPanJochiHelp.Dispose();
            }
            else if (sender == btnBldOK)
            {
                fn_DB_Update_Panjeng("판정", "BLOOD");
                fn_Panjeng_End_Check("BLOOD");
            }
            else if (sender == btnBldCancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Var_Clear();
                    fn_Bld_Screen_Clear();
                    return;
                }
            }
            else if (sender == btnXray1Cancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Var_Clear();
                    fn_Xray1_Screen_Clear();
                    return;
                }
            }
            else if (sender == btnXray2Cancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Var_Clear();
                    fn_Xray2_Screen_Clear();
                    return;
                }
            }
            else if (sender == btnAddCancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Var_Clear();
                    fn_LtdAdd_Screen_Clear();
                    return;
                }
            }
            else if (sender == btnAddSave)
            {
                if (clsHcVariable.GnHicLicense == 0)
                {
                    MessageBox.Show("판정의사만 판정을 할수 있습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                rSetEventResSave(clsHcVariable.GnWRTNO);
                fn_DB_Update_Panjeng("판정", "ADD");
                fn_Panjeng_End_Check("ADD");
            }
            else if (sender == btnAddCancel)
            {
                if (MessageBox.Show("변경한 내용을 저장하지 않습니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_LtdAdd_Screen_Clear();
                    return;
                }
            }
            else if (sender == btnWeSeangSave)
            {
                if (clsHcVariable.GnHicLicense == 0)
                {
                    MessageBox.Show("판정의사만 판정을 할수 있습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                rSetEventResSave(clsHcVariable.GnWRTNO);
                fn_DB_Update_Panjeng("판정", "WESEANG");
                fn_Panjeng_End_Check("WESEANG");
            }
            else if (sender == btnWeSeangCancel)
            {
                if (MessageBox.Show("변경한 내용을 저장하지 않습니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_WeSeang_Screen_Clear();
                    return;
                }
            }

            else if (sender == btnXray1OK)
            {
                fn_DB_Update_Panjeng("판정", "XRAY1");
                fn_Panjeng_End_Check("XRAY1");
            }
            else if (sender == btnXray2OK)
            {
                fn_DB_Update_Panjeng("판정", "XRAY2");
                fn_Panjeng_End_Check("XRAY2");
            }
            else if (sender == btnXray1MSave)
            {
                string strYN = "";
                string strGbn = "";
                int result = 0;

                if (txtXray1PanDrNo1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("진찰의사면호번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (rdoXray11.Checked == true)
                    {
                        strYN = "Y";
                    }
                    else
                    {
                        strYN = "N";
                    }

                    if (rdoXray1Gubun1.Checked == true)
                    {
                        strGbn = "Y";
                    }
                    else
                    {
                        strGbn = "N";
                    }

                    COMHPC item = new COMHPC();

                    item.JINGBN = strGbn;
                    item.XP1 = strYN;
                    item.XPJONG = txtXray1XJong.Text.Trim();
                    item.XPLACE = txtXray1Place.Text.Trim();
                    item.XREMARK = txtXray1Remark.Text.Trim();
                    item.XMUCH = txtXray1Much.Text.Trim();
                    item.XTERM = txtXray1Term.Text.Trim();
                    item.XTERM1 = txtXray1Term.Text.Trim();
                    item.XJUNGSAN = txtXray1Jung.Text.Trim();
                    item.MUN1 = txtXray1Mun1.Text.Trim();
                    item.JUNGSAN1 = txtXray1Eye.Text.Trim();
                    item.JUNGSAN2 = txtXray1Skin.Text.Trim();
                    item.JUNGSAN3 = txtXray1Etc.Text.Trim();
                    item.MUNDRNO = txtXray1PanDrNo1.Text;
                    item.WRTNO = clsHcVariable.GnWRTNO;

                    result = comHpcLibBService.UpdateHic_X_MunjinbyWrtNo(item);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnXray1MCancel, new EventArgs());
            }
            else if (sender == btnXray1MCancel)
            {
                txtXray1Place.Text = "";
                txtXray1Remark.Text = "";
                txtXray1Much.Text = "";
                txtXray1Jung.Text = "";
                txtXray1Eye.Text = "";
                txtXray1Skin.Text = "";
                txtXray1Etc.Text = "";
                txtXray1Term.Text = "";
                txtXray1XTerm.Text = "";
                txtWorkDay1.Text = "";
                txtXray1Mun1.Text = "";
                rdoXray11.Checked = false;
                rdoXray12.Checked = false;
                txtXray1XJong.Text = "";
                txtXray1PanDrNo1.Text = "";
                lblXray1DrName1.Text = "";
                rdoXray1Gubun1.Checked = false;
                rdoXray1Gubun2.Checked = false;
            }
            else if (sender == btnXray2MSave)
            {
                string strYN = "";
                string strGbn = "";
                int result = 0;

                if (txtXray2PanDrNo1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("진찰의사면호번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (rdoXray21.Checked == true)
                    {
                        strYN = "Y";
                    }
                    else
                    {
                        strYN = "N";
                    }

                    if (rdoXray2Gubun1.Checked == true)
                    {
                        strGbn = "Y";
                    }
                    else
                    {
                        strGbn = "N";
                    }

                    COMHPC item = new COMHPC();

                    item.JINGBN = strGbn;
                    item.XP1 = strYN;
                    item.XPJONG = txtXray2XJong.Text.Trim();
                    item.XPLACE = txtXray2Place.Text.Trim();
                    item.XREMARK = txtXray2Remark.Text.Trim();
                    item.XMUCH = txtXray2Much.Text.Trim();
                    item.XTERM = txtXray2Term.Text.Trim();
                    item.XTERM1 = txtXray2Term.Text.Trim();
                    item.XJUNGSAN = txtXray2Jung.Text.Trim();
                    item.MUN1 = txtXray2Mun1.Text.Trim();
                    item.JUNGSAN1 = txtXray2Eye.Text.Trim();
                    item.JUNGSAN2 = txtXray2Skin.Text.Trim();
                    item.JUNGSAN3 = txtXray2Etc.Text.Trim();
                    item.MUNDRNO = txtXray2PanDrNo1.Text;
                    item.WRTNO = clsHcVariable.GnWRTNO;

                    result = comHpcLibBService.UpdateHic_X_MunjinbyWrtNo(item);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnXray2MCancel, new EventArgs());
            }
            else if (sender == btnXray2MCancel)
            {
                txtXray2Place.Text = "";
                txtXray2Remark.Text = "";
                txtXray2Much.Text = "";
                txtXray2Jung.Text = "";
                txtXray2Eye.Text = "";
                txtXray2Skin.Text = "";
                txtXray2Etc.Text = "";
                txtXray2Term.Text = "";
                txtXray2XTerm.Text = "";
                txtWorkDay2.Text = "";
                txtXray2Mun1.Text = "";
                rdoXray21.Checked = false;
                rdoXray22.Checked = false;
                txtXray2XJong.Text = "";
                txtXray2PanDrNo1.Text = "";
                lblXray2DrName1.Text = "";
                rdoXray2Gubun1.Checked = false;
                rdoXray2Gubun2.Checked = false;
            }
            else if (sender == btnXray1Sogen)
            {
                FrmHcPanXHelp = new frmHcPanXHelp("70");
                FrmHcPanXHelp.rSetGstrValue += new frmHcPanXHelp.SetGstrValue(Xray1_Sogen_value);
                FrmHcPanXHelp.ShowDialog();
                FrmHcPanXHelp.rSetGstrValue -= new frmHcPanXHelp.SetGstrValue(Xray1_Sogen_value);
            }
            else if (sender == btnXray1ReExam)
            {
                FrmHcPanPanjengHelp = new frmHcPanPanjengHelp("53");
                FrmHcPanPanjengHelp.rSetGstrValue += new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
                FrmHcPanPanjengHelp.ShowDialog();
                FrmHcPanPanjengHelp.rSetGstrValue -= new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
            }
            else if (sender == btnXray1Default)
            {
                txtXray1SogenRemark.Text = "정상";
            }
            else if (sender == btnXray1Jochi1)
            {
                FrmHcPanXHelp = new frmHcPanXHelp("71");
                FrmHcPanXHelp.rSetGstrValue += new frmHcPanXHelp.SetGstrValue(Xray1_Jochi_value);
                FrmHcPanXHelp.ShowDialog();
                FrmHcPanXHelp.rSetGstrValue -= new frmHcPanXHelp.SetGstrValue(Xray1_Jochi_value);
            }
            else if (sender == btnXray2Sogen)
            {
                FrmHcPanXHelp = new frmHcPanXHelp("70");
                FrmHcPanXHelp.rSetGstrValue += new frmHcPanXHelp.SetGstrValue(Xray2_Sogen_value);
                FrmHcPanXHelp.ShowDialog();
                FrmHcPanXHelp.rSetGstrValue -= new frmHcPanXHelp.SetGstrValue(Xray2_Sogen_value);
            }
            else if (sender == btnXray2Default)
            {
                txtXray1SogenRemark.Text = "정상";
            }
            else if (sender == btnXray2Sahu)
            {
                FrmHcPanSpcSahuCode = new frmHcPanSpcSahuCode("70");
                FrmHcPanSpcSahuCode.rSetGstrValue += new frmHcPanSpcSahuCode.SetGstrValue(Xray2_Sahu_value);
                FrmHcPanSpcSahuCode.ShowDialog();
                FrmHcPanSpcSahuCode.rSetGstrValue -= new frmHcPanSpcSahuCode.SetGstrValue(Xray2_Sahu_value);

            }
            else if (sender == btnXray2Jochi1)
            {
                FrmHcPanXHelp = new frmHcPanXHelp("71");
                FrmHcPanXHelp.rSetGstrValue += new frmHcPanXHelp.SetGstrValue(Xray2_Jochi_value);
                FrmHcPanXHelp.ShowDialog();
                FrmHcPanXHelp.rSetGstrValue -= new frmHcPanXHelp.SetGstrValue(Xray2_Jochi_value);
            }
            else if (sender == btnXray2JobYN)
            {
                FrmHcCodeHelp = new frmHcCodeHelp("13");
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtXray2JobYN.Text = FstrCode.Trim();
                    lblXray2JobYN.Text = FstrName.Trim();
                }
                else
                {
                    txtXray2JobYN.Text = "";
                    lblXray2JobYN.Text = "";
                }
            }
            else if (sender == btnXray1AutoPan)
            {
                long nWRTNO = 0;
                int nREAD = 0;
                string strJong = "";
                bool bOK = true;
                string strExCode = "";
                string strResult = "";
                string strResCode = "";
                string strResultType = "";
                string strGbCodeUse = "";
                string strExPan = "";
                
                int result = 0;

                if (MessageBox.Show("검사결과가 정상인 경우 자동판정을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < FPatListItem.Count; i++)
                {
                    strJong = FPatListItem[i].GJJONG;
                    nWRTNO = FPatListItem[i].WRTNO;

                    bOK = true;

                    //건강검진 문진표 및 결과를  READ
                    COMHPC list = comHpcLibBService.GetXP1byWrtNo(nWRTNO);

                    if (list != null)
                    {
                        //방사선 피폭증상이 있을 경우 자동판정 대상이 아님
                        if (list.XP1.Trim() == "Y")
                        {
                            bOK = false;
                        }
                    }

                    if (bOK == true)
                    {
                        List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoExCodeNotIn(nWRTNO);

                        nREAD = list2.Count;
                        for (int j = 0; j < nREAD; j++)
                        {
                            strExCode = list2[j].EXCODE.Trim();                 //검사코드
                            strResult = list2[j].RESULT.Trim();                 //검사실 결과값
                            strResCode = list2[j].RESCODE.Trim();               //결과값 코드
                            strResultType = list2[j].RESULTTYPE.Trim();         //결과값 TYPE
                            strGbCodeUse = list2[j].GBCODEUSE.Trim();           //결과값코드 사용여부
                            strExPan = list2[j].PANJENG.Trim();
                            if (strExPan == "R" || strExPan == "B")
                            {
                                bOK = false;
                            }
                        }
                        if (bOK == true)
                        {
                            clsDB.setBeginTran(clsDB.DbCon);
                            //판정결과를 DB에 UPDATE

                            COMHPC item = new COMHPC();

                            item.STS = "Y";
                            item.PAN = "";
                            item.SOGEN = "정상";
                            item.WORKYN = "";
                            item.SAHUCODE = "";
                            item.PANJENG = "";
                            item.PANJENGDATE = clsPublic.GstrSysDate;
                            item.REEXAM = "";
                            item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                            item.WRTNO = nWRTNO;

                            result = comHpcLibBService.UpdateHic_X_MunjinResult(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show(i + " 번줄 판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }

                MessageBox.Show("자동판정 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Xray1_Sogen_value(string argValue)
        {
            txtXray1SogenRemark.Text = argValue;
        }

        private void Xray2_Sogen_value(string argValue)
        {
            txtXray2SogenRemark.Text = argValue;
        }

        private void ReExam_value(string argValue, string argName)
        {
            txtXray1ReExam.Text = argValue;
            txtXray1ReExam.Text = VB.Left(txtXray1ReExam.Text, txtXray1ReExam.Text.Length - 1);
            lblXray1ReExamName.Text = "☞추가검사: " + argName;
        }
        
        private void Xray2_Sahu_value(string argValue, string argRemark)
        {
            txtXray2Sahu.Text = argValue;
            lblXray2SahuName.Text = argRemark;
        }

        private void Xray1_Jochi_value(string argValue)
        {
            txtXray1JochiRemark.Text = argValue;
        }

        private void Xray2_Jochi_value(string argValue)
        {
            txtXray2JochiRemark.Text = argValue;
        }

        private void Code_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == dtpBldPanDate)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtBldPanDrNo)
                {
                    lblBldDrName.Text = hb.READ_License_DrName(txtBldPanDrNo.Text.To<long>());
                }
                else if (sender == txtXray1PanDrNo1)
                {
                    lblXray1DrName1.Text = hb.READ_License_DrName(txtXray1PanDrNo1.Text.To<long>());
                }
                else if (sender == txtXray2PanDrNo1)
                {
                    lblXray2DrName1.Text = hb.READ_License_DrName(txtXray2PanDrNo1.Text.To<long>());
                }
            }
        }

        void eComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtXray1Etc)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtXray1Etc.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else if (sender == txtXray1Eye)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtXray1Eye.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else if (sender == txtXray1Skin)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtXray1Skin.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eComboBoxClick(object sender, EventArgs e)
        {
            if (VB.Left(cboXray1Panjeng.Text, 1) == "1")
            {
                txtXray1SogenRemark.Text = "정상";
                SendKeys.Send("{TAB}");
            }
            else
            {
                txtXray1SogenRemark.Text = "";
                dtpXray1PanDate.Text = clsPublic.GstrSysDate;
            }
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            if (sender == chkXray1X1_13)
            {
                if (chkXray1X1_11.Checked == true)
                {
                    txtXray1X1_1.Enabled = true;
                }
                else
                {
                    txtXray1X1_1.Enabled = false;
                }
            }
            else if (sender == chkXray1X1_23)
            {
                if (chkXray1X1_23.Checked == true)
                {
                    txtXray1X1_2.Enabled = true;
                }
                else
                {
                    txtXray1X1_2.Enabled = false;
                }
            }
            else if (sender == chkXray1X1_42)
            {
                if (chkXray1X1_42.Checked == true)
                {
                    txtXray1X1_4.Enabled = true;
                }
                else
                {
                    txtXray1X1_4.Enabled = false;
                }
            }
            else if (sender == chkXray1X43)
            {
                if (sender == chkXray1X1_41)
                {
                    txtXray1Jikjong.Enabled = true;
                }
                else
                {
                    txtXray1Jikjong.Enabled = false;
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtBldSogen)
            {
                txtBldSogen.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtBldSogen)
            {
                txtBldSogen.ImeMode = ImeMode.Hangul;
            }
        }

        void frmHcPanJochiHelp_SpreadDoubleClick(string strRemark)
        {
            txtBldSogen.Text += strRemark;
        }

        void frmHcPanJochiHelp_SpreadDoubleClick2(string strRemark)
        {
            txtAddSogen.Text += "\r\n" + strRemark;
        }

        void fn_DB_Update_Panjeng(string argGbn, string argJong)
        {
            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            if (argJong == "BLOOD")
            {
                //판정결과를 DB에 UPDATE
                HIC_RES_ETC item = new HIC_RES_ETC();

                if (txtBldPanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    clsHcVariable.GnHicLicense = txtBldPanDrNo.Text.To<long>();
                }
                txtBldSogen.Text = txtBldSogen.Text.Trim();

                if (dtpBldPanDate.Text.IsNullOrEmpty())
                {
                    dtpBldPanDate.Text = clsPublic.GstrSysDate;
                }

                item.GBPANJENG = "Y";
                item.SOGEN = txtBldSogen.Text.Trim().Replace("'", "`");
                item.PANJENGDATE = dtpBldPanDate.Text;
                item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                item.WRTNO = clsHcVariable.GnWRTNO;
                item.GUBUN = "1";

                result = hicResEtcService.UpdatebyWrtNo(item);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            else if (argJong == "ADD")
            {
                //판정결과를 DB에 UPDATE
                HIC_RES_ETC item = new HIC_RES_ETC();

                if (txtAddPanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    clsHcVariable.GnHicLicense = txtAddPanDrNo.Text.To<long>();
                }
                txtAddSogen.Text = txtAddSogen.Text.Trim();

                if (dtpAddPanDate.Text.IsNullOrEmpty())
                {
                    dtpAddPanDate.Text = clsPublic.GstrSysDate;
                }

                item.GBPANJENG = "Y";
                item.SOGEN = txtAddSogen.Text.Trim().Replace("'", "`");
                item.PANJENGDATE = dtpAddPanDate.Text;
                item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                item.WRTNO = clsHcVariable.GnWRTNO;
                item.GUBUN = "2";

                result = hicResEtcService.UpdatebyWrtNo(item);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            else if (argJong == "WESEANG")
            {
                //위생
                //판정결과를 DB에 UPDATE
                HIC_RES_ETC item = new HIC_RES_ETC();

                if (txtWeSeangPanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    clsHcVariable.GnHicLicense = txtWeSeangPanDrNo.Text.To<long>();
                }
                txtWeSeangSogen.Text = txtWeSeangSogen.Text.Trim();

                if (dtpWeSeangPanDate.Text.IsNullOrEmpty())
                {
                    dtpWeSeangPanDate.Text = clsPublic.GstrSysDate;
                }

                item.GBPANJENG = "Y";
                item.SOGEN = txtWeSeangSogen.Text.Trim().Replace("'", "`");
                item.PANJENGDATE = dtpWeSeangPanDate.Text;
                item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                item.WRTNO = clsHcVariable.GnWRTNO;
                item.GUBUN = "4";

                result = hicResEtcService.UpdatebyWrtNo(item);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            else if (argJong == "XRAY1")
            {
                if (txtXray1PanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    clsHcVariable.GnHicLicense = txtXray1PanDrNo.Text.To<long>();
                }

                txtXray1JochiRemark.Text = txtXray1JochiRemark.Text.Trim();
                if (dtpXray1PanDate.Text.IsNullOrEmpty())
                {
                    dtpXray1PanDate.Text = clsPublic.GstrSysDate;
                }

                //판정결과를 DB에 UPDATE
                COMHPC item = new COMHPC();

                item.PAN = VB.Left(cboXray1Panjeng.Text, 1);
                item.STS = "Y";
                item.SOGEN = txtXray1SogenRemark.Text.Trim().Replace("'", "`");
                item.PANJENG = txtXray1JochiRemark.Text.Trim().Replace("'", "`");
                item.PANJENGDATE = dtpXray1PanDate.Text.Trim();
                item.REEXAM = txtXray1ReExam.Text.Trim();
                item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                item.WRTNO = clsHcVariable.GnWRTNO;

                result = comHpcLibBService.UpdateHic_X_MunjinResult(item);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                result = hicJepsuService.UpdatePanjengDatePanjenghDrNobyWrtNo(dtpXray1PanDate.Text, clsHcVariable.GnHicLicense, clsHcVariable.GnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            else if (argJong == "XRAY2")
            {
                if (txtXray2PanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    clsHcVariable.GnHicLicense = txtXray2PanDrNo.Text.To<long>();
                }

                txtXray2JochiRemark.Text = txtXray2JochiRemark.Text.Trim();
                if (dtpXray2PanDate.Text.IsNullOrEmpty())
                {
                    dtpXray2PanDate.Text = clsPublic.GstrSysDate;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //판정결과를 DB에 UPDATE
                COMHPC item = new COMHPC();

                item.PAN = VB.Left(cboXray2Panjeng.Text, 1);
                item.STS = "Y";
                item.SOGEN = txtXray2SogenRemark.Text.Trim().Replace("'", "`");
                item.WORKYN = txtXray2JobYN.Text.Trim();
                item.SAHUCODE = txtXray2Sahu.Text.Trim();
                item.PANJENG = txtXray2JochiRemark.Text.Trim().Replace("'", "`");
                item.PANJENGDATE = dtpXray2PanDate.Text.Trim();
                item.PANJENGDRNO = clsHcVariable.GnHicLicense;
                item.WRTNO = clsHcVariable.GnWRTNO;

                result = comHpcLibBService.UpdateHic_X_MunjinResult(item);

                if (result < 0)
                {
                    MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }




            clsDB.setCommitTran(clsDB.DbCon);
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            long nDrNO = 0;

            if (sender == txtXray1PanDrNo1)
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        nDrNO = 48054;  //F5(전혜리)
                        break;
                    case Keys.F6:
                        nDrNO = 19516;  //F6(김중구)
                        break;
                    case Keys.F7:
                        nDrNO = 18685;  //F7(황성일)
                        break;
                    case Keys.F8:
                        nDrNO = 22977;  //F8(김주호)
                        break;
                    case Keys.F9:
                        nDrNO = 0;
                        break;
                    default:
                        break;
                }

                txtXray1PanDrNo1.Text = nDrNO.To<string>();
                lblXray1DrName1.Text = hb.READ_License_DrName(txtXray1PanDrNo1.Text.To<long>());
            }
            else if (sender == txtXray2PanDrNo1)
            {
                lblXray2DrName1.Text = hb.READ_License_DrName(txtXray2PanDrNo1.Text.To<long>());
            }
            SendKeys.Send("{TAB}");
        }

        void fn_Panjeng_End_Check(string argJong)
        {
            string strOK = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;
            string strGbPanjeng = "";
            string strGubun = "";

            strOK = "OK";
            nPanDrno = 0;
            strPanDate = "";

            clsDB.setBeginTran(clsDB.DbCon);

            if (argJong == "BLOOD" || argJong == "ADD")
            {
                if (argJong == "BLOOD")
                {
                    strGubun = "1";
                }
                else if (argJong == "ADD")
                {
                    strGubun = "2";
                }

                //건강보험1차 판정완료 Check
                HIC_RES_ETC list = hicResEtcService.GetItembyWrtNo(clsHcVariable.GnWRTNO, strGubun);

                if (list.IsNullOrEmpty())
                {
                    strOK = "NO";
                }
                else
                {
                    if (list.PANJENGDRNO == 0)
                    {
                        strOK = "NO";
                    }
                    nPanDrno = list.PANJENGDRNO;
                    strPanDate = list.PANJENGDATE.To<string>();
                }

                if (strOK == "OK")
                {
                    strGbPanjeng = "Y";
                }
                else
                {
                    strGbPanjeng = "";
                }

                //판정완료/미완료 SET
                result = hicResEtcService.UpdatebyWrtNoGubun(strGbPanjeng, clsHcVariable.GnWRTNO, strGubun);

                if (result < 0)
                {
                    MessageBox.Show("판정완료/미완료 설정 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //접수마스타에 판정일자 Update
                result = hicJepsuService.UpdatePanjengDatebyWrtNo(strPanDate, nPanDrno, clsHcVariable.GnWRTNO, strOK);

                if (result < 0)
                {
                    MessageBox.Show("판정완료/미완료 설정 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strOK == "OK")
                {
                    //if (MessageBox.Show("판정이 완료 되었습니다." + "\r\n\r\n" + "화면을 Clear 한 후 다음 환자를 판정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    MessageBox.Show("판정이 완료 되었습니다." + "\r\n\r\n" + "화면을 Clear 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    {
                        fn_Bld_Screen_Clear();
                        fn_LtdAdd_Screen_Clear();
                        return;
                    }
                }
            }
            else if (argJong == "WESEANG")
            {
                strOK = "OK";
                strPanDate = dtpWeSeangPanDate.Text;
                nPanDrno = txtWeSeangPanDrNo.Text.To<long>();
                //접수마스타에 판정일자 Update
                result = hicJepsuService.UpdatePanjengDatebyWrtNo(strPanDate, nPanDrno, clsHcVariable.GnWRTNO, strOK);

                if (result < 0)
                {
                    MessageBox.Show("판정완료/미완료 설정 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strOK == "OK")
                {
                    //if (MessageBox.Show("판정이 완료 되었습니다." + "\r\n\r\n" + "화면을 Clear 한 후 다음 환자를 판정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    MessageBox.Show("판정이 완료 되었습니다." + "\r\n\r\n" + "화면을 Clear 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    {
                        fn_WeSeang_Screen_Clear();
                        return;
                    }
                }
            }
            else if (argJong == "XRAY1" || argJong == "XRAY2")
            {
                string sMsg = "";

                clsDB.setBeginTran(clsDB.DbCon);
                //방사선종사자 1차 판정완료 Check
                COMHPC list = comHpcLibBService.GetHic_X_MunjinbyWrtNo(clsHcVariable.GnWRTNO);

                if (list.IsNullOrEmpty())
                {
                    strOK = "NO";
                }
                else
                {
                    if (list.PANJENGDRNO == 0)
                    {
                        strOK = "NO";
                    }
                    nPanDrno = list.PANJENGDRNO;
                    strPanDate = list.PANJENGDATE.To<string>();
                }

                //판정완료/미완료 SET
                result = comHpcLibBService.UpdateHic_X_Munjin(clsHcVariable.GnWRTNO, strOK);

                if (result < 0)
                {
                    MessageBox.Show("방사선 작업종사자 판정 여부 Setting 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //접수마스타에 판정일자 Update
                result = hicJepsuService.UpdatePanjengDatebyWrtNo(clsHcVariable.GnWRTNO, strOK, strPanDate, nPanDrno);

                if (result < 0)
                {
                    MessageBox.Show("방사선 작업종사자 판정 여부 Setting 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strOK == "OK")
                {
                    sMsg = "판정이 완료되었습니다." + "\r\n";
                    sMsg += "화면을 Clear후 다음 환자를 판정하시겠습니까?";
                    if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (argJong == "XRAY1")
                        {
                            fn_Xray1_Screen_Clear();
                        }
                        else if (argJong == "XRAY2")
                        {
                            fn_Xray2_Screen_Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 방사선과 코드와 건강증진센타 코드 변환테이블 설정
        /// </summary>
        void XRayCode_2_HicCode_SET()
        {
            int nRead = 0;
            long nCnt = 0;
            string strCode = "";
            string strName = "";
            string strResult = "";

            List<BAS_BCODE> list = basBcodeService.GetItembyGubun("XRAY_건진코드_변환테이블");

            nRead = list.Count;
            FstrXRayCodeList = "{@}";
            for (int i = 0; i < nRead; i++)
            {
                strCode = list[i].CODE;
                strName = list[i].NAME;
                nCnt = VB.L(strName, ",");
                strResult = "";
                for (int j = 1; j <= nCnt; j++)
                {
                    if (VB.Pstr(strName, ",", j).Trim() != "")
                    {
                        strResult += VB.Pstr(strName, ",", j).Trim() + ",";
                    }
                }
                if (VB.Right(strResult, 1) == ",")
                {
                    strResult = VB.Left(strResult, strResult.Length - 1);
                    if (strResult != "")
                    {
                        FstrXRayCodeList += strCode + "{}" + strResult + "{@}";
                    }
                }
            }
        }
    }
}
