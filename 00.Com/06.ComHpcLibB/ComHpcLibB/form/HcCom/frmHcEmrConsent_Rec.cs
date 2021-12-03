using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComEmrBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcEmrConset_Rec.cs
/// Description     : 검진센터 동의서 작성 NEW (EMR 연동)
/// Author          : 김민철
/// Create Date     : 2021-02-18
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm동의서(Frm동의서.frm)" />
/// 
namespace ComHpcLibB
{
    public partial class frmHcEmrConset_Rec :Form
    {
        HicDoctorService hicDoctorService = null;
        HicBcodeService hicBcodeService = null;
        HicConsentService hicConsentService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;
        HeaResvExamService heaResvExamService = null;
        HicPatientService hicPatientService = null;
        HicResSpecialService hicResSpecialService = null;
        HeaResultService heaResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        OcsDoctorService ocsDoctorService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;

        clsHcFunc cHF = null;
        clsHcMain cHM = null;
        clsHaBase cHB = null;

        string FstrFDate = "";
        string FstrTDate = "";
        string FstrSDate = "";
        string FstrDoct = "";
        string FstrDrNo = "";
        string FstrSName = "";
        string FstrPtno = "";
        string FstrDept = "";
        string FstrJob = "";
        string FstrFileName = "";
        string FstrASA = "";
        string FstrGUBUN = "";
        string FstrForm = "";

        string[] FstrFilePath = new string[20];
        int FnFileCnt = 0;

        int FnRow = 0;
        long FnWRTNO = 0;
        bool bolSort = false;

        List<HIC_BCODE> lsthBCD = null;
        List<string> lstHbCode = new List<string>();        //HEA조영제대상검사코드
        
        public frmHcEmrConset_Rec()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcEmrConset_Rec(long argWRTNO, string argGUBUN, string argForm = "")
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FnWRTNO = argWRTNO;
            FstrGUBUN = argGUBUN;
            FstrForm = argForm;
        }

        private void SetControl()
        {
            hicDoctorService = new HicDoctorService();
            hicBcodeService = new HicBcodeService();
            hicConsentService = new HicConsentService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuService = new HicJepsuService();
            heaJepsuService = new HeaJepsuService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
            heaResvExamService = new HeaResvExamService();
            hicPatientService = new HicPatientService();
            hicResSpecialService = new HicResSpecialService();
            heaResultService = new HeaResultService();
            comHpcLibBService = new ComHpcLibBService();
            ocsDoctorService = new OcsDoctorService();
            hicSangdamNewService = new HicSangdamNewService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();

            cHF = new clsHcFunc();
            cHM = new clsHcMain();
            cHB = new clsHaBase();

            lsthBCD = new List<HIC_BCODE>();

            List<HIC_DOCTOR> list = hicDoctorService.Read_Combo_HisDoctor("");
            cboDoct.Items.Clear();
            cboDoct.Items.Add("");
            cboDoct.SetItems(list, "DRNAME", "SABUN");
            cboDoct.SelectedIndex = 0;

            lsthBCD = hicBcodeService.GetCodeNamebyBcode("HEA_조영제대상검사코드");

            SS1.Initialize(new SpreadOption { RowHeaderVisible = true, RowHeight = 28 });
            SS1.AddColumnCheckBox("선택",     "",                              44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = true });
            SS1.AddColumn("등록번호",       nameof(HIC_CONSENT.PTNO),          84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("수검자명",       nameof(HIC_CONSENT.SNAME),         86, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("검진과목",       nameof(HIC_CONSENT.DEPTCODE),      44, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("서식코드",       nameof(HIC_CONSENT.FORMCODE),      40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("서식명",         nameof(HIC_CONSENT.FORMNAME),     240, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("등록일자",       nameof(HIC_CONSENT.BDATE),         84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("수검일자",       nameof(HIC_CONSENT.SDATE),         84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("접수번호",       nameof(HIC_CONSENT.WRTNO),         40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("동의일자",       nameof(HIC_CONSENT.CONSENT_TIME), 120, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("의사명",         nameof(HIC_CONSENT.DRSABUN),       40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("의사명",         nameof(HIC_CONSENT.DRNAME),        92, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID",          nameof(HIC_CONSENT.RID),           40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("ASA",            nameof(HIC_CONSENT.ASA),           44, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("EMRNO",          nameof(HIC_CONSENT.EMRNO),         44, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            //this.timer1.Tick += new EventHandler(eTimer_Timer);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnConfirm.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);

        }

        private void eSetASA_Score(string argVal)
        {
            if (!argVal.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[FnRow, 13].Text = argVal;
                FstrASA = argVal;
            }
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(SS1, e.Column, ref bolSort, true);
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader) { return; }

            if (e.Column == 5)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }

                SS1.ActiveSheet.Cells[e.Row, 0].Text = "Y";

                Data_Save();

                FnWRTNO = 0;
            }
            else if (e.Column == 13)
            {
                FnRow = e.Row;
                //내시경(진정) 검사전 기록지
                HIC_CONSENT item = SS1.GetCurrentRowData() as HIC_CONSENT;

                frmHaASA frmASA = new frmHaASA(item.PTNO, item.SNAME, item.SDATE, item.DRSABUN);
                frmASA.rSetGstrValue += new frmHaASA.SetGstrValue(eSetASA_Score);
                frmASA.ShowDialog();
                frmASA.rSetGstrValue -= new frmHaASA.SetGstrValue(eSetASA_Score);
                cHF.fn_ClearMemory(frmASA);
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string strOK = "";

            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtSName.Text = "";
            //timer1.Enabled = false;

            for (int i = 0; i < cboDoct.Items.Count; i++)
            {
                cboDoct.SelectedIndex = i;

                if (VB.Pstr(cboDoct.Text, ".", 1).Trim() == clsType.User.IdNumber)
                {
                    strOK = "OK";
                    break;
                }
            }

            if (strOK == "") { cboDoct.SelectedIndex = 0; }

            //HEA_조영제대상검사코드
            for (int i = 0; i < lsthBCD.Count; i++)
            {
                lstHbCode.Add(lsthBCD[i].CODE);
            }

            string strHEA = hicDoctorService.Chk_Hea_Doct(clsType.User.IdNumber.To<long>(0));

            if (!strHEA.IsNullOrEmpty())
            {
                rdoDept1.Checked = true;
            }

            if (FnWRTNO > 0)
            {
                rdoDept0.Checked = true;
                Screen_Display();
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnConfirm)
            {
                //frmHcConsentformView frm = new frmHcConsentformView();
                frmHcEmrConsentView frm = new frmHcEmrConsentView();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == btnDelete)
            {
                Data_Delete();
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
        }

        private void Data_Save()
        {
            string strOK = "", strPtno = "", strSName = "", strDept = "", strForm = "";
            string strJuso1 = "", strJuso2 = "", strUcodesName = "";
            long nWRTNO = 0;
            int nCNT = 0;
            long nEmrno = 0;
            bool bASA = false;

            string strDrName = "";
            string strDrGubun = "";

            cHB.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            strDrName = VB.Trim(clsHcVariable.GstrHicDrName);    //판정의 성명
            strDrGubun = clsHcVariable.GstrDrGbDent;    //판정의사 구분

            if(strDrGubun =="1")
            {
                strDrGubun = "doctor_signature_1614061594814";  //치과
            }
            else
            {
                
                strDrGubun = "doctor_signature_1609144282446";  //일반
                //strDrGubun = "doctor_signature_1614061594814";  //치과
            }
            
            if (cboDoct.Text.Trim() == "" && FstrGUBUN == "DORTOR")
            {
                MessageBox.Show("설명의사가 공란입니다.", "확인");
                return;
            }

            FstrDrNo = VB.Pstr(cboDoct.Text, ".", 1).Trim();
            long nDoct = FstrDrNo.To<long>(0);

            //timer1.Enabled = false;

            FstrFileName = "";
            FnFileCnt = 0;

            for (int i = 0; i < 20; i++) { FstrFilePath[i] = ""; }

            //-------------------------------
            //    신체등급(ASA) 등록
            //-------------------------------
            strOK = ""; nCNT = 0; nEmrno = 0;
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "Y")
                {
                    nCNT += 1;

                    if (strPtno == "")
                    {
                        strPtno = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strSName = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        strDept = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        strForm = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                        FstrASA = SS1.ActiveSheet.Cells[i, 13].Text.Trim();
                        FstrSDate = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                        nWRTNO = SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0);
                    }
                    else
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text != strPtno) { strOK = "NO"; break; }
                        if (SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0) != nWRTNO) { strOK = "NO"; break; }
                    }
                }
            }

            if (strOK == "NO")
            {
                clsPublic.GstrMsgList = "다른 사람과 함께 동의서 요청이 되어" + ComNum.VBLF;
                clsPublic.GstrMsgList += "동의서 작성이 불가능합니다.";
                MessageBox.Show(clsPublic.GstrMsgList, "확인");
                return;
            }

            bASA = false; nCNT = 0;

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0) == nWRTNO)
                {
                    if (SS1.ActiveSheet.Cells[i, 4].Text.Trim() == "D20" || SS1.ActiveSheet.Cells[i, 4].Text.Trim() == "D30")
                    {
                        nCNT += 1;
                    }

                    //기존 입력된 ASA가 있으면 등록화면 표시 안함
                    if (SS1.ActiveSheet.Cells[i, 13].Text.Trim() != "") { bASA = true; }
                }
            }

            if (nCNT == 0)
            {
                List<HIC_RESULT_EXCODE> lst1 = hicResultExCodeService.GetHeaEndoExListByWrtno(nWRTNO);

                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        if (!lst1[i].ENDOGUBUN3.IsNullOrEmpty() && lst1[i].ENDOGUBUN3.Trim() == "Y") { nCNT += 1; }
                    }
                }
            }

            //처음이고 수면내시경이면 ASA 입력화면 표시
            if (bASA == false && nCNT > 0)
            {
                //clsPublic.GstrHelpCode = strPtno + "{}" + strSName + "{}" + FstrDate + "{}" + FstrDrNo + "{}"; //등록번호,성명,검사일자,의사사번
                FnRow = SS1.ActiveSheet.ActiveRowIndex;

                //내시경(진정) 검사전 기록지
                HIC_CONSENT item = SS1.GetCurrentRowData() as HIC_CONSENT;

                frmHaASA frmASA = new frmHaASA(strPtno, strSName, FstrSDate, nDoct);
                frmASA.rSetGstrValue += new frmHaASA.SetGstrValue(eSetASA_Score);
                frmASA.ShowDialog();
                frmASA.rSetGstrValue -= new frmHaASA.SetGstrValue(eSetASA_Score);
                cHF.fn_ClearMemory(frmASA);
            }

            //-------------------------------
            //    동의서 받기
            //-------------------------------
            nCNT = 0;
            strOK = "";
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "Y")
                {
                    nCNT += 1;

                    if (strPtno == "")
                    {
                        strPtno = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strSName = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        strDept = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        strForm = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                        FstrSDate = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                        nWRTNO = SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0);
                        nEmrno = SS1.ActiveSheet.Cells[i, 14].Text.To<long>(0);
                    }
                    else
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text != strPtno) { strOK = "NO"; break; }
                        if (SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0) != nWRTNO) { strOK = "NO"; break; }
                        nEmrno = SS1.ActiveSheet.Cells[i, 14].Text.To<long>(0);
                    }
                }
            }

            long nAGE = 0;
            string strSex = "";
            string strJepDate = "";
            string strPANO = "";

            //성별,나이를 읽음
            if (strDept == "TO")
            {
                HEA_JEPSU haJP = heaJepsuService.GetItemByWrtno(nWRTNO);
                if (!haJP.IsNullOrEmpty())
                {
                    strSex = haJP.SEX;
                    strJepDate = haJP.SDATE;
                    strPANO = haJP.PANO.To<string>("0");
                    nAGE = haJP.AGE;
                    strJuso1 = haJP.JUSO1;
                    strJuso2 = haJP.JUSO2;
                }
                
            }
            else
            {
                HIC_JEPSU hcJP = hicJepsuService.GetItemByWRTNO(nWRTNO);
                if (!hcJP.IsNullOrEmpty())
                {
                    strSex = hcJP.SEX;
                    strJepDate = hcJP.JEPDATE;
                    strPANO = hcJP.PANO.To<string>("0");
                    nAGE = hcJP.AGE;
                    strJuso1 = hcJP.JUSO1;
                    strJuso2 = hcJP.JUSO2;
                    strUcodesName = cHM.UCode_Names_Display(hcJP.UCODES);
                }
                if (!strUcodesName.IsNullOrEmpty())
                {
                    strUcodesName =VB.Replace(strUcodesName, ",", " / ");
                }

            }

            string strMunjinRes = "";
            string strOMR = "";
            StringBuilder strMUN = new StringBuilder();
            string strEndo1 = "", strEndo2 = "", strEndo3 = "", strEndo4 = "", strENDO = "";

            if (strDept == "HR")
            {
                //문진표내용읽기
                HIC_IE_MUNJIN_NEW hIEMUN = hicIeMunjinNewService.GetMunjinResbyWrtNo2(nWRTNO);

                if (!hIEMUN.IsNullOrEmpty())
                {
                    strMunjinRes = CONV_Munjin_Res(hIEMUN.MUNJINRES);
                    strOMR = VB.Pstr(VB.Pstr(VB.Pstr(strMunjinRes, "{<*>}tbl_cancer_etc{*}", 2), "{<*>}", 1), "{*}", 2);

                    //기왕력
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }    //1
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }    //2
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 3) + ";"); } else { strMUN.Append(" ;"); } //3

                    //알레르기
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //당뇨병
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //저고혈압
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //호흡기질환
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //심장질환
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //신장질환
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //출혈소인
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //약물중독
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 3) + ";"); } else { strMUN.Append(" ;"); }

                    //치아상태
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2) == "1") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2) == "2") { strMUN.Append("1;"); } else { strMUN.Append("0;"); }

                    //기타
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 3) + ";"); } else { strMUN.Append(" ;"); }
                }

                List<HIC_RESULT_EXCODE_JEPSU> list = hicResultExcodeJepsuService.GetItembyOnlyWrtNo(nWRTNO);

                if (!list.IsNullOrEmpty() && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ENDOGUBUN3.To<string>("").Trim() == "Y") { strEndo2 = "■"; strENDO = "OK"; }    //위수면내시경
                        if (list[i].ENDOGUBUN5.To<string>("").Trim() == "Y") { strEndo4 = "■"; strENDO = "OK"; }    //대장수면내시경
                    }
                }

                if (strEndo2 == "") { strEndo1 = "■"; }
                if (strEndo4 == "") { strEndo4 = "■"; }
            }

            //2020-01-29(내시경 시행일자 확인)
            string strStomachEndoDate = "";
            string strColonEndoDate = "";

            if (!strPANO.IsNullOrEmpty())
            {
                List<HEA_RESV_EXAM> haRESV = heaResvExamService.GetEndoResvListByPanoSDate(strPANO.To<long>(0), DateTime.Now.ToShortDateString());

                if (!haRESV.IsNullOrEmpty() && haRESV.Count > 0)
                {
                    for (int i = 0; i < haRESV.Count; i++)
                    {
                        if (haRESV[i].GBEXAM == "01")
                        {
                            strStomachEndoDate = VB.Left(haRESV[i].SDATE, 4) + VB.Mid(haRESV[i].SDATE, 6, 2) + VB.Right(haRESV[i].SDATE, 2);
                        }
                        else
                        {
                            strColonEndoDate = VB.Left(haRESV[i].SDATE, 4) + VB.Mid(haRESV[i].SDATE, 6, 2) + VB.Right(haRESV[i].SDATE, 2);
                        }
                    }
                }
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            string strCurDate = DateTime.Now.ToShortDateString();
            string strCurTime = clsPublic.GstrSysTime;

            if (strStomachEndoDate.IsNullOrEmpty())
            {
                strStomachEndoDate = VB.Left(strCurDate, 4) + VB.Mid(strCurDate, 6, 2) + VB.Right(strCurDate, 2);
            }

            if (strColonEndoDate.IsNullOrEmpty())
            {
                strColonEndoDate = VB.Left(strCurDate, 4) + VB.Mid(strCurDate, 6, 2) + VB.Right(strCurDate, 2);
            }

            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin = hicPatientService.GetJumin2byPtno(strPtno);
            if (!strJumin.IsNullOrEmpty())
            {
                strJumin = clsAES.DeAES(strJumin);
                strJumin = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);

                strJumin1 = VB.Left(strJumin, 6);
                strJumin2 = VB.Right(strJumin, 7);
            }

            string strRel = "";
            string strDate = VB.Replace(strCurDate, "-", "");
            string strAgreeDate = VB.Left(strCurDate, 4) + "-" + VB.Mid(strCurDate, 6, 2) + "-" + VB.Right(strCurDate, 2) + " " + VB.Left(strCurTime, 2) + ":" + VB.Right(strCurTime, 2);
            string strExamDate = VB.Left(strJepDate, 4) + "-" + VB.Mid(strJepDate, 6, 2) + "-" + VB.Right(strJepDate, 2);

            FstrPtno = strPtno;
            FstrDept = strDept;
            FnWRTNO = nWRTNO;
            
            FstrFileName = strDate + "_" + string.Format("{0:#0}", nWRTNO) + "_" + strSName + "_" + string.Format("{0:#0}", nAGE) + strSex + "O" + strDept + ".jpg";

            string strDeptName = "종합건진";

            if (strDept == "HR") { strDeptName = "일반건진"; }

            //같은 수검자 동의서 건수를 읽음
            if (nCNT == 1)
            {
                nCNT = 0;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 8].Text.To<long>(0) == nWRTNO)
                    {
                        nCNT += 1;
                    }
                }
            }

            string strExName = "";
            string strExName2 = "";

            if (strForm == "D40")
            {
                //검사부위를 찾음
                List<HEA_RESULT> lstHaRES = heaResultService.GetListByWrtnoExCodeIN(FnWRTNO, lstHbCode);

                if (!lstHaRES.IsNullOrEmpty() && lstHaRES.Count > 0)
                {
                    for (int i = 0; i < lstHaRES.Count; i++)
                    {
                        switch (lstHaRES[i].EXCODE.To<string>(""))
                        {
                            case "TX57": strExName2 = strExName2 + "Brain" + "{}"; break;
                            case "TX58": strExName2 = strExName2 + "Chest" + "{}"; break;
                            case "TX59": strExName2 = strExName2 + "abdomen" + "{}"; break;
                            case "TX67": strExName2 = strExName2 + "관상동맥" + "{}"; break;
                            default: break;
                        }
                    }

                    strExName = VB.Pstr(strExName2, "{}", 1);
                    strExName2 = VB.Pstr(strExName2, strExName + "{}", 2);
                    if (!strExName2.IsNullOrEmpty()) { strExName2 = VB.Replace(strExName2, "{}", ","); }

                }
            }


            int j = 0;
            int nCNT1 = 0;
            int[] nGbSangDam = new int[21];
            string[] strPjSnagDam = new string[21];
            string strData = "";
            string strPYOJANGGI = "";
            string strJINSOGEN = "";
            string strFdate = "";
            string strTdate = "";

            string strMunRes = "";
            string strTemp = "";
            string strData1 = ""; //P_GIGAN_1DAY (1일폭로시간)
            string strData2 = ""; //WORK_GONGJENG1 (작업공정1)
            string strData3 = ""; //WORK_YEAR (근무년수1)
            string strData4 = ""; //WORK_DYAS1 (근무기간1)
            string strData5 = ""; //WORK_GONGJENG2 (작업공정1)
            string strData6 = ""; //WORK_YEAR2 (근무년수1)
            string strData7 = ""; // WORK_DYAS2 (근무기간1)

            //D54(건강진단개인표)
            if (strForm == "D54")
            {
                
                HIC_SANGDAM_NEW item1 = hicSangdamNewService.GetItembyWrtNo(FnWRTNO);
                if (!item1.IsNullOrEmpty())
                {
                    if (!item1.PJSANGDAM.IsNullOrEmpty())
                    {
                        nCNT1 = Convert.ToInt32(VB.L(item1.PJSANGDAM, "{$}") - 1);
                        for (int i = 1; i <= nCNT1; i++)
                        {
                            strData = VB.Pstr(item1.PJSANGDAM, "{$}", i);
                            j = Convert.ToInt32(VB.Val(VB.Pstr(strData, "{}", 1)));
                            strPjSnagDam[j] = VB.Pstr(strData, "{}", 2);
                        }
                    }
                }

                //상담해야할 표적장기 찾기
                List<HIC_SUNAPDTL_GROUPCODE> list1 = hicSunapdtlGroupcodeService.GetCodeGbSangdambyWrtNo(FnWRTNO);
                if (!list1.IsNullOrEmpty())
                {
                    for (int i = 1; i <= list1.Count; i++)
                    {
                        strData = list1[i-1].GBSANGDAM.Trim();
                        for (j = 1; j <= VB.Len(strData); j++)
                        {
                            if (VB.Mid(strData, j, 1) == "1") { nGbSangDam[j] = 1; }
                        }
                    }
                }

                //표적장기명칭 및 상담결과 표시
                for (int i = 1; i <= 20; i++)
                {
                    if (nGbSangDam[i] == 1)
                    {
                        strPYOJANGGI += hicBcodeService.GetCodeNamebyGubunCode("HIC_표적장기별특수상담", string.Format("{0:00}", i)) + "<br>";
                        strJINSOGEN += strPjSnagDam[i] + "<br>";
                    }
                }
                //strPYOJANGGI = "호흡기계<br>간담도<br>심혈관<br>비뇨기계<br>이비인후<br>신경계<br>악구강<br>눈;피부;비강;인두<br>위장관계<br>근골격계<br>";
                //strJINSOGEN = "호흡기계<br>간담도<br>심혈관<br>비뇨기계<br>이비인후<br>신경계<br>악구강<br>눈;피부;비강;인두<br>위장관계<br>근골격계<br>";

                strPYOJANGGI = VB.Replace(strPYOJANGGI, ",", ";");
                strJINSOGEN = VB.Replace(strJINSOGEN, ",", ";");


                //인터넷문진 데이터 조회
                strFdate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
                strTdate = Convert.ToDateTime(strJepDate).AddDays(2).ToShortDateString();
                HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetCountbyPtNoMunDate(FstrPtno, strFdate, strTdate);
                if (!list.IsNullOrEmpty())
                {
                    strMunRes = list.MUNJINRES;
                    strTemp = VB.Pstr(VB.Pstr(VB.Pstr(strMunRes, "{<*>}tbl_common{*}", 2), "{*}", 2), "{<*>}", 1);


                    //VB.Pstr(VB.Pstr(strTemp, "{}", 28), ",", 2)

                }
            }

            string strHPONE = "";
         
            HIC_PATIENT hPT = hicPatientService.GetPatInfoByPtno(strPtno);
            if (!hPT.IsNullOrEmpty())
            {
                strHPONE = hPT.HPHONE.To<string>("");
            }

            //전자동의서 기본정보
            EMR_CONSENT eCon = new EMR_CONSENT
            {
                SNAME = strSName,
                PTNO = strPtno,
                HPHONE = strHPONE,
                DEPT = strDept,
                DEPTNAME = strDeptName,
                EXNAME = strExName,
                DRNO = FstrDrNo,
                WRTNO = nWRTNO,
                JUMINNO = strJumin,                 // - 포함
                JUMINNO1 = strJumin1,               //주민번호 앞자리
                JUMINNO2 = strJumin2,               //주민번호 뒷자리
                EXDATE = strExamDate,               //검사일자
                AGREEDATE = strDate,                //동의서작성일자
                STOMACHDATE = strStomachEndoDate,   //위내시경 예약일자
                COLONDATE = strColonEndoDate,       //대장내시경 예약일자
                ASA = FstrASA,
                MUNDATA = strMUN,
                FORMCODE = strForm,
                GJJONG11_YN = "0",
                GJJONG31_YN = "0",
                LTDNAME = hPT.LTDNAME,

                //건강진단개인표
                P_GIGAN_1DAY = "",
                WORK_GONGJENG1 = "",
                WORK_YEAR1 = "",
                WORK_DYAS1 = "",
                WORK_GONGJENG2 = "",
                WORK_YEAR2 = "",
                WORK_DYAS2 = "",
                PYOJANGGI = strPYOJANGGI,
                JINSOGEN = strJINSOGEN,
                JUSO = strJuso1 + " " + strJuso2,
                UCODENAMES = strUcodesName,
                ENDO = strENDO,
                EMRNO = nEmrno,

                //의사구분
                DRNAME = strDrName,
                DRGUBUN = strDrGubun

            };

            //구서식지번호를 신규서식지번호로 치환
            eCon.FORMNO = comHpcLibBService.GetFormNoByFormCD(eCon.FORMCODE);
            eCon.UPDATENO = comHpcLibBService.GetFormUpDateNoByFormCD(eCon.FORMNO);

            //검진종류 가져오기
            List<HIC_JEPSU> lstJep = hicJepsuService.GetListByPtnoJepDate(strPtno, strJepDate);

            if (!lstJep.IsNullOrEmpty() && lstJep.Count > 0)
            {
                for (int i = 0; i < lstJep.Count; i++)
                {
                    if (lstJep[i].GJJONG.To<string>("").Trim() == "11")
                    {
                        eCon.GJJONG11_YN = "1";
                    }
                    else if (lstJep[i].GJJONG.To<string>("").Trim() == "31")
                    {
                        eCon.GJJONG31_YN = "1";
                    }
                }
            }

            //전자동의서 작성
            fn_Emr_Consent(eCon);

        }

        private void Data_Delete()
        {
            string strRowid = "";
            string strEmrNo = "";

            try
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strRowid = "";

                    if (SS1.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        strRowid = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                        strEmrNo = SS1.ActiveSheet.Cells[i, 14].Text.Trim();

                        if (!strRowid.IsNullOrEmpty())
                        {
                            if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                            {
                                MessageBox.Show("동의서 Data 삭제시 오류 발생!", "오류");
                                return;
                            }
                        }

                        if (!strEmrNo.IsNullOrEmpty())
                        {
                            if (!clsEmrQuery.AeasFormDelete(strEmrNo))
                            {
                                MessageBox.Show("동의서 Data 삭제시 오류 발생!", "오류");
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("삭제완료!", "확인");

                Screen_Display();

                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Screen_Display()
        {
            SS1.ActiveSheet.RowCount = 0;
            
            Cursor.Current = Cursors.WaitCursor;

            FstrFDate = dtpFDate.Value.ToShortDateString();
            FstrTDate = dtpTDate.Value.ToShortDateString();
            //FstrDoct  = VB.Pstr(cboDoct.Text, ".", 2).Trim();
            FstrSName = txtSName.Text.Trim();
            FstrDept = rdoDept0.Checked ? "HIC" : "HEA";
            FstrJob = rdoJob0.Checked ? "NEW" : "MOD";

            List<string> lstFrmCD = new List<string>
            {
                "D10",
                "D20",
                "D40",
                "D54"
            };

            List<string> lstFrmCD1 = new List<string>
            {
                "D50",
                "D52",
                "D53",
                "D54",
                "D55",
                "D56",
                "D57"
            };


            try
            {
                if ( FstrGUBUN == "DOCTOR")
                {
                    SS1.DataSource = hicConsentService.GetListByItems(FstrFDate, FstrTDate, FstrSName, FstrDept, FstrJob, FnWRTNO, lstFrmCD, FstrForm);
                }

                else if(FstrGUBUN == "NUR")
                {
                    SS1.DataSource = hicConsentService.GetListByItems(FstrFDate, FstrTDate, FstrSName, FstrDept, FstrJob, FnWRTNO, lstFrmCD1, FstrForm);
                }

                if (SS1.ActiveSheet.RowCount == 1)
                {
                    SS1.ActiveSheet.Cells[0, 0].Text = "Y";

                    Data_Save();
                }

                Cursor.Current = Cursors.Default;

                this.btnExit.Enabled = true;
                this.SS1.Enabled = true;

                FnWRTNO = 0;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string CONV_Munjin_Res(string ArgMunRes)
        {
            string rtnVal = string.Empty;

            string[] strCol = new string[101];
            string strTable = string.Empty;
            string strTemp1 = string.Empty;
            string strTemp2 = string.Empty;
            string strTemp3 = string.Empty;
            int k = 0;
            int nTblCnt = VB.L(ArgMunRes, "{<*>}").To<int>();
            int nDCnt = 0;
            int nColCnt = 0;

            for (int i = 1; i < nTblCnt; i++)
            {
                strTemp1 = VB.Pstr(ArgMunRes, "{<*>}", i + 1);
                strTable = VB.Pstr(strTemp1, "{*}", 1);
                strTemp2 = VB.Pstr(strTemp1, "{*}", 3);
                nDCnt = VB.L(strTemp2, "{}").To<int>();

                for (int j = 1; j <= nDCnt; j++)
                {
                    strTemp3 = VB.Pstr(strTemp2, "{}", j);
                    k = VB.Val(VB.Pstr(strTemp3, ",", 1)).To<int>();
                    if (k > 0)
                    {
                        nColCnt = k;
                        strCol[nColCnt] = strTemp3;
                    }
                }

                rtnVal = rtnVal + "{<*>}" + strTable + "{*}" + VB.Format(nColCnt, "#0") + "{*}";
                for (int j = 1; j <= nColCnt; j++)
                {
                    rtnVal = rtnVal + strCol[j] + "{}";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 전자동의서 작성 Main Function
        /// </summary>
        /// <param name="eCon"></param>
        /// <param name="eParam"></param>
        private void fn_Emr_Consent(EMR_CONSENT eCon)
        {
            //Form ActiveFormWrite = null;

            frmEasViewer ActiveFormWrite = null;

            EmrForm fWrite = null;
            EmrPatient AcpEmr = null;

            //EMR 환자정보 세팅
            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, eCon.PTNO, "O", eCon.EXDATE.Replace("-", ""), eCon.DEPT);

            if (AcpEmr == null)
            {
                AcpEmr = new EmrPatient()
                {
                    ptNo = eCon.PTNO,
                    medFrDate = eCon.EXDATE.Replace("-", ""),
                    medDeptCd = eCon.DEPT,
                    medDrCd = ocsDoctorService.GetDrCodebySabun(eCon.DRNO),
                    medDrName = ocsDoctorService.GetDrNamebySabun(eCon.DRNO),
                    inOutCls = "O"
                };
            }
            else
            {
                //SetEmrPatInfoOcs에서는 외래, 입원, EMR_TREATT 테이블을 참조하여 의사세팅 함.
                //검진의사는 7101, 7102로 세팅되어있으므로 동의서 작성의사로 재설정함.
                AcpEmr.medDrCd = ocsDoctorService.GetDrCodebySabun(eCon.DRNO);
                AcpEmr.medDrName = ocsDoctorService.GetDrNamebySabun(eCon.DRNO);
            }

            //서식지 정보 Form에 세팅
            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, eCon.FORMNO.To<string>(""), eCon.UPDATENO.To<string>(""));

            if (fWrite == null)
            {
                MessageBox.Show("등록된 기록지 폼이 없습니다.");
                fWrite = clsEmrChart.ClearEmrForm();
                return;
            }

            //서식지작성용 파라미터 매칭
            EasParam eParam= EasParamByFormCD(eCon);

            EasManager easManager = EasManager.Instance;

            ActiveFormWrite = easManager.GetEasFormViewer();

            long formDataId = easManager.GetFormDataId(fWrite, AcpEmr, eCon.EMRNO);

            if (formDataId > 0)
            {
                easManager.Update(fWrite, AcpEmr, formDataId, eParam);
            }
            else
            {
                easManager.Write(fWrite, AcpEmr, eParam);
            }

            ActiveFormWrite.IsAutoCloseBySave = true;
            ActiveFormWrite.exitDelegate += EasFormViewr_exitDelegate;
            ActiveFormWrite.Show();                             //의사 설명용 화면
            ActiveFormWrite.LocationToRight();

            easManager.ShowTabletMoniror(eParam, formDataId);   //수검자 작성용 화면

        }

        /// <summary>
        /// 동의서 서식지 별 argument 매칭
        /// </summary>
        /// <param name="eCon"></param>
        /// <returns></returns>
        private EasParam EasParamByFormCD(EMR_CONSENT eCon)
        {
            string strTag1 = "", strTag2 = "", strTag3 = "", strTag4 = "", strTag5 = "", strTag6 = "", strTag7 = "", strTag8 = "", strTag9 = "", strTag10 = "", strTag11 = "";

            EasParam eParam = new EasParam();

            if (eCon.FORMCODE == "D10")         //위내시경
            {
                if (eCon.MUNDATA.ToString().Trim() != "")
                {

                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 1) == "1") { strTag1 = clsHcType.MUNJIN11; } else { strTag1 = clsHcType.MUNJIN12; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 4) == "1") { strTag2 = clsHcType.MUNJIN21; } else { strTag2 = clsHcType.MUNJIN22; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 7) == "1") { strTag3 = clsHcType.MUNJIN31; } else { strTag3 = clsHcType.MUNJIN32; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 10) == "1") { strTag4 = clsHcType.MUNJIN41; } else { strTag4 = clsHcType.MUNJIN42; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 13) == "1") { strTag5 = clsHcType.MUNJIN51; } else { strTag5 = clsHcType.MUNJIN52; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 16) == "1") { strTag6 = clsHcType.MUNJIN61; } else { strTag6 = clsHcType.MUNJIN62; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 19) == "1") { strTag7 = clsHcType.MUNJIN71; } else { strTag7 = clsHcType.MUNJIN72; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 22) == "1") { strTag8 = clsHcType.MUNJIN81; } else { strTag8 = clsHcType.MUNJIN82; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 25) == "1") { strTag9 = clsHcType.MUNJIN91; } else { strTag9 = clsHcType.MUNJIN92; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 28) == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }
                    if(eCon.ENDO == "OK") { strTag11 = clsHcType.ENDO11; } else  { strTag11 = clsHcType.ENDO12; }


                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    + strTag1 + ",inputbox_1581409383124*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 3)     //기왕력
                    + strTag2 + ",inputbox_1581409387268*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 6)       //알레르기
                    + strTag3 + ",inputbox_1581409444395*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 9)       //당뇨병
                    + strTag4 + ",inputbox_1581409549349*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 12)       //저고혈압
                    + strTag5 + ",inputbox_1581409690676*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 15)       //호흡기질환
                    + strTag6 + ",inputbox_1581409703699*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 18)       //심장질환
                    + strTag7 + ",inputbox_1581409695805*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 21)       //신장질환
                    + strTag8 + ",inputbox_1581409706890*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 24)       //출혈소인
                    + strTag9 + ",inputbox_1581409699216*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 27)       //약물중독
                    + strTag10                                                   //치아상태
                    + ",inputbox_1581409682854*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 30)             //기타
                    + strTag11 //내시경수면여부                                                                              
                    + ",radiobox_1581415166630*true" + ",radiobox_1581415166631*true" + ",radiobox_1581415166632*true"
                    ;


                    //if (eCon.MUNDATA[0].ToString() == "1") { strTag1 = clsHcType.MUNJIN11; } else { strTag1 = clsHcType.MUNJIN12; }
                    //if (eCon.MUNDATA[3].ToString() == "1") { strTag2 = clsHcType.MUNJIN21; } else { strTag2 = clsHcType.MUNJIN22; }
                    //if (eCon.MUNDATA[6].ToString() == "1") { strTag3 = clsHcType.MUNJIN31; } else { strTag3 = clsHcType.MUNJIN32; }
                    //if (eCon.MUNDATA[9].ToString() == "1") { strTag4 = clsHcType.MUNJIN41; } else { strTag4 = clsHcType.MUNJIN42; }
                    //if (eCon.MUNDATA[12].ToString() == "1") { strTag5 = clsHcType.MUNJIN51; } else { strTag5 = clsHcType.MUNJIN52; }
                    //if (eCon.MUNDATA[15].ToString() == "1") { strTag6 = clsHcType.MUNJIN61; } else { strTag6 = clsHcType.MUNJIN62; }
                    //if (eCon.MUNDATA[18].ToString() == "1") { strTag7 = clsHcType.MUNJIN71; } else { strTag7 = clsHcType.MUNJIN72; }
                    //if (eCon.MUNDATA[21].ToString() == "1") { strTag8 = clsHcType.MUNJIN81; } else { strTag8 = clsHcType.MUNJIN82; }
                    //if (eCon.MUNDATA[24].ToString() == "1") { strTag9 = clsHcType.MUNJIN91; } else { strTag9 = clsHcType.MUNJIN92; }
                    //if (eCon.MUNDATA[27].ToString() == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }

                    //eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    //+ strTag1 + ",inputbox_1581409383124*" + eCon.MUNDATA[2]     //기왕력
                    //+ strTag2 + ",inputbox_1581409387268*" + eCon.MUNDATA[5]     //알레르기
                    //+ strTag3 + ",inputbox_1581409444395*" + eCon.MUNDATA[8]     //당뇨병
                    //+ strTag4 + ",inputbox_1581409549349*" + eCon.MUNDATA[11]    //저고혈압
                    //+ strTag5 + ",inputbox_1581409690676*" + eCon.MUNDATA[14]    //호흡기질환
                    //+ strTag6 + ",inputbox_1581409703699*" + eCon.MUNDATA[17]    //심장질환
                    //+ strTag7 + ",inputbox_1581409695805*" + eCon.MUNDATA[20]    //신장질환
                    //+ strTag8 + ",inputbox_1581409706890*" + eCon.MUNDATA[23]    //출혈소인
                    //+ strTag9 + ",inputbox_1581409699216*" + eCon.MUNDATA[26]    //약물중독
                    //+ strTag10                                                   //치아상태
                    //+ ",inputbox_1581409682854*" + eCon.MUNDATA[29];             //기타
                }
                else
                {
                    strTag1 = clsHcType.MUNJIN11;
                    strTag2 = clsHcType.MUNJIN21;
                    strTag3 = clsHcType.MUNJIN31;
                    strTag4 = clsHcType.MUNJIN41;
                    strTag5 = clsHcType.MUNJIN51;
                    strTag6 = clsHcType.MUNJIN61;
                    strTag7 = clsHcType.MUNJIN71;
                    strTag8 = clsHcType.MUNJIN81;
                    strTag9 = clsHcType.MUNJIN91;
                    strTag10 = clsHcType.MUNJIN01;
                    if (eCon.ENDO == "OK") { strTag11 = clsHcType.ENDO11; } else { strTag11 = clsHcType.ENDO12; }

                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    + strTag1+ strTag2+ strTag3 + strTag4 + strTag5 + strTag6 + strTag7 + strTag8 + strTag9 + strTag10 + strTag11
                    + ",radiobox_1581415166630*true" + ",radiobox_1581415166631*true" + ",radiobox_1581415166632*true"
                    ;

                }
            }
            else if (eCon.FORMCODE == "D20")    //대장내시경
            {
                if (eCon.MUNDATA.ToString().Trim() != "")
                {



                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 1) == "1") { strTag1 = clsHcType.MUNJIN11; } else { strTag1 = clsHcType.MUNJIN12; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 4) == "1") { strTag2 = clsHcType.MUNJIN21; } else { strTag2 = clsHcType.MUNJIN22; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 7) == "1") { strTag3 = clsHcType.MUNJIN31; } else { strTag3 = clsHcType.MUNJIN32; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 10) == "1") { strTag4 = clsHcType.MUNJIN41; } else { strTag4 = clsHcType.MUNJIN42; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 13) == "1") { strTag5 = clsHcType.MUNJIN51; } else { strTag5 = clsHcType.MUNJIN52; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 16) == "1") { strTag6 = clsHcType.MUNJIN61; } else { strTag6 = clsHcType.MUNJIN62; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 19) == "1") { strTag7 = clsHcType.MUNJIN71; } else { strTag7 = clsHcType.MUNJIN72; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 22) == "1") { strTag8 = clsHcType.MUNJIN81; } else { strTag8 = clsHcType.MUNJIN82; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 25) == "1") { strTag9 = clsHcType.MUNJIN91; } else { strTag9 = clsHcType.MUNJIN92; }
                    if (VB.Pstr(eCon.MUNDATA.ToString(), ";", 28) == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }

                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    + strTag1 + ",inputbox_1581409383124*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 3)     //기왕력
                    + strTag2 + ",inputbox_1581409387268*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 6)       //알레르기
                    + strTag3 + ",inputbox_1581409444395*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 9)       //당뇨병
                    + strTag4 + ",inputbox_1581409549349*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 12)       //저고혈압
                    + strTag5 + ",inputbox_1581409690676*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 15)       //호흡기질환
                    + strTag6 + ",inputbox_1581409703699*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 18)       //심장질환
                    + strTag7 + ",inputbox_1581409695805*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 21)       //신장질환
                    + strTag8 + ",inputbox_1581409706890*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 24)       //출혈소인
                    + strTag9 + ",inputbox_1581409699216*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 27)       //약물중독
                    + strTag10                                                   //치아상태
                    + ",inputbox_1581409682854*" + VB.Pstr(eCon.MUNDATA.ToString(), ";", 30);             //기타

                    //if (eCon.MUNDATA[0].ToString() == "1") { strTag1 = clsHcType.MUNJIN11; } else { strTag1 = clsHcType.MUNJIN12; }
                    //if (eCon.MUNDATA[3].ToString() == "1") { strTag2 = clsHcType.MUNJIN21; } else { strTag2 = clsHcType.MUNJIN22; }
                    //if (eCon.MUNDATA[6].ToString() == "1") { strTag3 = clsHcType.MUNJIN31; } else { strTag3 = clsHcType.MUNJIN32; }
                    //if (eCon.MUNDATA[9].ToString() == "1") { strTag4 = clsHcType.MUNJIN41; } else { strTag4 = clsHcType.MUNJIN42; }
                    //if (eCon.MUNDATA[12].ToString() == "1") { strTag5 = clsHcType.MUNJIN51; } else { strTag5 = clsHcType.MUNJIN52; }
                    //if (eCon.MUNDATA[15].ToString() == "1") { strTag6 = clsHcType.MUNJIN61; } else { strTag6 = clsHcType.MUNJIN62; }
                    //if (eCon.MUNDATA[18].ToString() == "1") { strTag7 = clsHcType.MUNJIN71; } else { strTag7 = clsHcType.MUNJIN72; }
                    //if (eCon.MUNDATA[21].ToString() == "1") { strTag8 = clsHcType.MUNJIN81; } else { strTag8 = clsHcType.MUNJIN82; }
                    //if (eCon.MUNDATA[24].ToString() == "1") { strTag9 = clsHcType.MUNJIN91; } else { strTag9 = clsHcType.MUNJIN92; }
                    //if (eCon.MUNDATA[27].ToString() == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }

                    //eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    //+ strTag1 + ",inputbox_1581409383124*" + eCon.MUNDATA[2]     //기왕력
                    //+ strTag2 + ",inputbox_1581409387268*" + eCon.MUNDATA[5]     //알레르기
                    //+ strTag3 + ",inputbox_1581409444395*" + eCon.MUNDATA[8]     //당뇨병
                    //+ strTag4 + ",inputbox_1581409549349*" + eCon.MUNDATA[11]    //저고혈압
                    //+ strTag5 + ",inputbox_1581409690676*" + eCon.MUNDATA[14]    //호흡기질환
                    //+ strTag6 + ",inputbox_1581409703699*" + eCon.MUNDATA[17]    //심장질환
                    //+ strTag7 + ",inputbox_1581409695805*" + eCon.MUNDATA[20]    //신장질환
                    //+ strTag8 + ",inputbox_1581409706890*" + eCon.MUNDATA[23]    //출혈소인
                    //+ strTag9 + ",inputbox_1581409699216*" + eCon.MUNDATA[26]    //약물중독
                    //+ ",inputbox_1581409682854*" + eCon.MUNDATA[29];             //기타
                }
                else
                {
                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME;
                }
            }
            else if (eCon.FORMCODE == "D40")    //조영제동의서
            {
                eParam.ControlInit = "inputbox_1580450335113*" + eCon.EXNAME + ",inputbox_1581408418699*" + eCon.DEPTNAME + ",inputbox_1580450282051*" + eCon.EXDATE + ",inputbox_1580450339113*" + eCon.DEPTNAME;
            }
            else if (eCon.FORMCODE == "D50")    //개인정보동의서
            {
                //자동입력
            }
            else if (eCon.FORMCODE == "D52")    //정보활용동의서
            {
                eParam.ControlInit = "checkbox_1609139700135*" + eCon.GJJONG11_YN + ",checkbox_1609139704868*" + eCon.GJJONG31_YN + ",inputboxtel_1609142266850*" + eCon.JUMINNO1 + ",inputboxtel_1609142271875*" + eCon.JUMINNO2;
            }
            else if (eCon.FORMCODE == "D53")    //건강검진 동시진행 동의서
            {
                eParam.ControlInit = "checkbox_1609143063765*1";
            }
            else if (eCon.FORMCODE == "D54")    //건강진단 개인표
            {
                eParam.ControlInit = "inputbox_1609143862721*" + eCon.SNAME + ",inputbox_1614060310363*" + eCon.HPHONE + ",inputbox_1609143602400*" + eCon.LTDNAME + ",inputbox_1614060080803*" + eCon.BUSENAME;
                eParam.ControlInit += ",inputbox_1614060057805*" + eCon.IPSADATE + ",inputboxtel_1609144339097*" + eCon.JUMINNO1 + ",inputboxtel_1609144342351*" + eCon.JUMINNO2 + ",textarea_1614062040863*" + eCon.JUSO;
                eParam.ControlInit += ",textarea_1614060377936*" + eCon.UCODENAMES + ",inputbox_1614060595152*" + eCon.BUSE + ",inputbox_1614060664082*" + eCon.GONGJENG + ",inputbox_1614060592760*" + eCon.BUSEIPSA;
                eParam.ControlInit += ",inputbox_1614060653303*" + eCon.P_GIGAN + ",inputbox_1614060738588*" + eCon.P_GIGAN_1DAY + ",inputbox_1614061055265*" + eCon.WORK_GONGJENG1 + ",inputbox_1614061051172*" + eCon.WORK_YEAR1;
                eParam.ControlInit += ",eas-date_1614061758204*" + eCon.WORK_YEAR1 + ",inputbox_1614061057357*" + eCon.WORK_GONGJENG2 + ",inputbox_1614061063262*" + eCon.WORK_YEAR2 + ",eas-date_1614061758204*" + eCon.WORK_DYAS2;
                eParam.ControlInit += ",textarea_1614061498419*" + eCon.PYOJANGGI + ",textarea_1614062202647*" + eCon.JINSOGEN;
                eParam.ControlInit += ",signid*" + eCon.DRGUBUN + ",signname*" + eCon.DRNAME;
            }
            else if (eCon.FORMCODE == "D55")    //자궁세포암 검사 시술 설명 및 동의서
            {
                //자동입력
            }
            else if (eCon.FORMCODE == "D56")    //생물학적 노출지표검사 (소변)
            {
                eParam.ControlInit = "inputbox_1609222448999*" + eCon.AGREEDATE + ",inputbox_1609145859223*" + eCon.LTDNAME + ",inputboxtel_1609145875161*" + eCon.JUMINNO1 + ",inputboxtel_1609145877930*" + eCon.JUMINNO2;
            }
            else if (eCon.FORMCODE == "D57")    //생물학적 노출지표검사 (혈액)
            {
                eParam.ControlInit = "inputbox_1609146923572*" + eCon.AGREEDATE + ",inputbox_1609146908451*" + eCon.LTDNAME + ",inputboxtel_1609146939430*" + eCon.JUMINNO1 + ",inputboxtel_1609146944827*" + eCon.JUMINNO2;
            }

            return eParam;
        }

        /// <summary>
        /// 개인정보 동의서 종료 이벤트
        /// </summary>
        public void EasFormViewr_exitDelegate(EmrForm emrForm, EmrPatient ePAT, string formDataId)
        {
            //var t = Task.Delay(1000);
            //t.Wait();

            string strFrmCD = "";
            string strDrSabun = "";

            EasManager easManager = EasManager.Instance;

            long formId = easManager.GetFormDataId(emrForm, ePAT, 0);

            //작성이 완료되었으면
            if (formId > 0)
            {
                strFrmCD = hicConsentService.GetFormCDByFormNo(emrForm.FmFORMNO);
                strDrSabun = ocsDoctorService.GetSabunByDrCode(ePAT.medDrCd);

                long nConCnt = hicConsentService.GetListByFormNoPtnoDate(emrForm.FmFORMNO, ePAT.ptNo, ePAT.medFrDate, ePAT.medDeptCd);

                if (nConCnt > 0)
                {
                    HIC_CONSENT item = new HIC_CONSENT
                    {
                        PTNO = ePAT.ptNo,
                        DRSABUN = strDrSabun.To<long>(0),
                        FORMCODE = strFrmCD,
                        SDATE = ePAT.medFrDate,
                        DEPTCODE = ePAT.medDeptCd,
                        EMRNO = formId
                    };

                    int result = hicConsentService.UpdateItem2(item);

                    if (result < 0)
                    {
                        MessageBox.Show("동의서 작성기록 UPDATE 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }

            Screen_Display();

        }
    }
}
