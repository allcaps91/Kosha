using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPermList_Rec.cs
/// Description     : 검진센터 동의서 작성
/// Author          : 김민철
/// Create Date     : 2020-11-19
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm동의서(Frm동의서.frm)" />
/// 
namespace ComHpcLibB
{
    public partial class frmHcPermList_Rec :Form
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
        HeaResultService heaResultService = null;

        clsHcFunc cHF = null;

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
        
        string[] FstrFilePath = new string[20];
        int FnFileCnt = 0;

        int FnRow = 0;
        long FnWRTNO = 0;
        bool bolSort = false;

        List<HIC_BCODE> lsthBCD = null;
        List<string> lstHbCode = new List<string>();        //HEA조영제대상검사코드
        
        public frmHcPermList_Rec()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPermList_Rec(long argWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FnWRTNO = argWRTNO;
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
            heaResultService = new HeaResultService();

            cHF = new clsHcFunc();

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
            SS1.AddColumn("매수",           nameof(HIC_CONSENT.PAGECNT),       42, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("등록일자",       nameof(HIC_CONSENT.BDATE),         84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("수검일자",       nameof(HIC_CONSENT.SDATE),         84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("접수번호",       nameof(HIC_CONSENT.WRTNO),         40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("동의일자",       nameof(HIC_CONSENT.CONSENT_TIME), 120, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("의사명",         nameof(HIC_CONSENT.DRSABUN),       40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("의사명",         nameof(HIC_CONSENT.DRNAME),        92, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID",          nameof(HIC_CONSENT.RID),           40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("ASA",            nameof(HIC_CONSENT.ASA),           44, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.timer1.Tick += new EventHandler(eTimer_Timer);
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
                SS1.ActiveSheet.Cells[FnRow, 14].Text = argVal;
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
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "Y";

                Data_Save();
            }
            else if (e.Column == 14)
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

        private void eTimer_Timer(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            long nDrSabun = VB.Pstr(cboDoct.Text, ".", 1).To<long>(0);

            FileInfo Dir = null;

            //동의서 파일이 모두 저장되었는지 점검
            string strOK = "OK";
            string strFile = "";

            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D11" || FstrFilePath[i] == "D12" || FstrFilePath[i] == "D21" || FstrFilePath[i] == "D22" || FstrFilePath[i] == "D31" || FstrFilePath[i] == "D41")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrNo + "\\" + FstrFileName;
                }

                Dir = new FileInfo(strFile);

                if (Dir.Exists == false || strFile == "")
                {
                    timer1.Enabled = false;
                    strOK = "";
                    break;
                }
            }

            if (strOK == "") { return; }

            //------------------------------
            //  파일을 서버에 저장함
            //------------------------------
            int nSeq = 0;
            string strOLD = "";

            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D11" || FstrFilePath[i] == "D12" || FstrFilePath[i] == "D21" || FstrFilePath[i] == "D22" || FstrFilePath[i] == "D31" || FstrFilePath[i] == "D41")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrNo + "\\" + FstrFileName;
                }

                string strNew = FstrFilePath[i];

                if (strNew != strOLD)
                {
                    strOLD = strNew;
                    nSeq = 1;
                }
                else
                {
                    nSeq = nSeq + 1;
                }

                string strServer = VB.Format(FnWRTNO, "#0") + "_" + FstrDept + "_" + FstrFilePath[i] + "_" + VB.Format(nSeq, "#0") + ".jpg";

                Ftpedt FtpedtX = new Ftpedt();

                //서버에 FTP로 파일을 전송
                if (!FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strServer, "/data/hic_result/consent_temp/"))
                {
                    int result = hicConsentService.UpdateItemByWrtno(FstrPtno, FnWRTNO, "");

                    if (result < 0)
                    {
                        clsPublic.GstrMsgList = "동의서를 서버로 전송을 못하였습니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList += "다시 동의서를 받으십시오.";

                        MessageBox.Show(clsPublic.GstrMsgList, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            //------------------------------
            //  PC의 파일을 삭제함
            //------------------------------

            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D11" || FstrFilePath[i] == "D12" || FstrFilePath[i] == "D21" || FstrFilePath[i] == "D22" || FstrFilePath[i] == "D31" || FstrFilePath[i] == "D41")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrNo + "\\" + FstrFileName;
                }

                Dir = new FileInfo(strFile);

                if (Dir.Exists == true)
                {
                    Dir.Delete();
                }
            }

            if (!FstrPtno.IsNullOrEmpty())
            {
                for (int i = 1; i <= FnFileCnt; i++)
                {
                    HIC_CONSENT item = new HIC_CONSENT
                    {
                        PTNO = FstrPtno,
                        DRSABUN = nDrSabun,
                        WRTNO = FnWRTNO,
                        FORMCODE = FstrFilePath[i]
                    };

                    int result = hicConsentService.UpdateItem(item);

                    if (result < 0)
                    {
                        MessageBox.Show("동의서 작성기록 UPDATE 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int result2 = hicConsentService.UpDateD10PageSetByWrtno(item);

                    if (result2 < 0)
                    {
                        MessageBox.Show("동의서 작성기록 UPDATE 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            Screen_Display(SS1);

            timer1.Enabled = false;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string strOK = "";

            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtSName.Text = "";
            timer1.Enabled = false;

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
                Screen_Display(SS1);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SS1);
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnConfirm)
            {
                frmHcConsentformView frm = new frmHcConsentformView();
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
            long nWRTNO = 0;
            int nCNT = 0;
            bool bASA = false;

            if (cboDoct.Text.Trim() == "")
            {
                MessageBox.Show("설명의사가 공란입니다.", "확인");
                return;
            }

            FstrDrNo = VB.Pstr(cboDoct.Text, ".", 1).Trim();
            long nDoct = FstrDrNo.To<long>();

            timer1.Enabled = false;

            FstrFileName = "";
            FnFileCnt = 0;

            for (int i = 0; i < 20; i++) { FstrFilePath[i] = ""; }

            //-------------------------------
            //    신체등급(ASA) 등록
            //-------------------------------
            strOK = ""; nCNT = 0;
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
                        FstrSDate = SS1.ActiveSheet.Cells[i, 8].Text.Trim();
                        nWRTNO = SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0);
                    }
                    else
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text != strPtno) { strOK = "NO"; break; }
                        if (SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0) != nWRTNO) { strOK = "NO"; break; }
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
                if (SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0) == nWRTNO)
                {
                    if (SS1.ActiveSheet.Cells[i, 4].Text.Trim() == "D20" || SS1.ActiveSheet.Cells[i, 4].Text.Trim() == "D30")
                    {
                        nCNT += 1;
                    }

                    //기존 입력된 ASA가 있으면 등록화면 표시 안함
                    if (SS1.ActiveSheet.Cells[i, 14].Text.Trim() != "") { bASA = true; }
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
                        FstrSDate = SS1.ActiveSheet.Cells[i, 8].Text.Trim();
                        nWRTNO = SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0);
                    }
                    else
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text != strPtno) { strOK = "NO"; break; }
                        if (SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0) != nWRTNO) { strOK = "NO"; break; }
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
                }
                
            }

            string strMunjinRes = "";
            string strOMR = "";
            StringBuilder strMUN = new StringBuilder();
            string strEndo1 = "", strEndo2 = "", strEndo3 = "", strEndo4 = "";

            if (strDept == "HR")
            {
                //문진표내용읽기
                HIC_IE_MUNJIN_NEW hIEMUN = hicIeMunjinNewService.GetMunjinResbyWrtNo2(nWRTNO);

                if (!hIEMUN.IsNullOrEmpty())
                {
                    strMunjinRes = CONV_Munjin_Res(hIEMUN.MUNJINRES);
                    strOMR = VB.Pstr(VB.Pstr(VB.Pstr(strMunjinRes, "{<*>}tbl_cancer_etc{*}", 2), "{<*>}", 1), "{*}", 2);

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 1), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 2), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 3), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 4), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 5), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 6), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 7), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 8), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 9), ",", 3)); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2) == "1") { strMUN.Append("■;"); } else { strMUN.Append(";"); }
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 10), ",", 2) == "2") { strMUN.Append("■;"); } else { strMUN.Append(";"); }

                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 3) != "") { strMUN.Append(VB.Pstr(VB.Pstr(strOMR, "{}", 11), ",", 3)); } else { strMUN.Append(";"); }
                }

                List<HIC_RESULT_EXCODE_JEPSU> list = hicResultExcodeJepsuService.GetItembyOnlyWrtNo(nWRTNO);

                if (!list.IsNullOrEmpty() && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ENDOGUBUN3.To<string>("").Trim() == "Y") { strEndo2 = "■"; }    //위수면내시경
                        if (list[i].ENDOGUBUN5.To<string>("").Trim() == "Y") { strEndo4 = "■"; }    //대장수면내시경
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
                            strStomachEndoDate = VB.Left(haRESV[i].SDATE, 4) + "년 " + VB.Mid(haRESV[i].SDATE, 6, 2) + "월 " + VB.Right(haRESV[i].SDATE, 2) + "일";
                        }
                        else
                        {
                            strColonEndoDate = VB.Left(haRESV[i].SDATE, 4) + "년 " + VB.Mid(haRESV[i].SDATE, 6, 2) + "월 " + VB.Right(haRESV[i].SDATE, 2) + "일";
                        }
                    }
                }
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            string strCurDate = DateTime.Now.ToShortDateString();
            string strCurTime = clsPublic.GstrSysTime;

            if (strStomachEndoDate.IsNullOrEmpty())
            {
                strStomachEndoDate = VB.Left(strCurDate, 4) + "년 " + VB.Mid(strCurDate, 6, 2) + "월 " + VB.Right(strCurDate, 2) + "일";
            }

            if (strColonEndoDate.IsNullOrEmpty())
            {
                strColonEndoDate = VB.Left(strCurDate, 4) + "년 " + VB.Mid(strCurDate, 6, 2) + "월 " + VB.Right(strCurDate, 2) + "일";
            }

            string strJumin = hicPatientService.GetJumin2byPtno(strPtno);
            if (!strJumin.IsNullOrEmpty())
            {
                strJumin = clsAES.DeAES(strJumin);
                strJumin = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
            }

            string strRel = "";
            string strDate = VB.Replace(strCurDate, "-", "");
            string strAgreeDate = VB.Left(strCurDate, 4) + "년 " + VB.Mid(strCurDate, 6, 2) + "월 " + VB.Right(strCurDate, 2) + "일 " + VB.Left(strCurTime, 2) + "시 " + VB.Right(strCurTime, 2) + "분";
            string strExamDate = VB.Left(strJepDate, 4) + "년 " + VB.Mid(strJepDate, 6, 2) + "월 " + VB.Right(strJepDate, 2) + "일";
            string strExamDate1 = VB.Left(strJepDate, 4) + "-" + VB.Mid(strJepDate, 6, 2) + "-" + VB.Right(strJepDate, 2);

            string strCmd = @"C:\_spool\PenToolController.exe ";
            strCmd += strDate + ";" + strPtno + ";_" + string.Format("{0:#0}", nWRTNO) + "_" + strSName + "_;" + strSex + ";" + nAGE + ";" + strDept + ";";

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
                    if (SS1.ActiveSheet.Cells[i, 9].Text.To<long>(0) == nWRTNO)
                    {
                        nCNT += 1;
                    }
                }
            }

            if (nCNT == 1 && strForm =="D50")
            {
                fn_SET_NeoPenDesk(strForm, FnFileCnt, ref strCmd, FstrDrNo, strPtno, strSName, strSex, nAGE, 
                    strAgreeDate, strExamDate, strExamDate1, strDeptName, strJumin, strStomachEndoDate, strColonEndoDate, strEndo1, strEndo2, strMUN);
            }
            else
            {
                clsPublic.GstrMsgList = "동의서가 " + nCNT.To<string>("0") + "건이 있습니다.";
                clsPublic.GstrMsgList += "전체 동의서를 승인요청을 하시겠습니까?";

                if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    fn_SET_NeoPenDesk(strForm, FnFileCnt, ref strCmd, FstrDrNo, strPtno, strSName, strSex, nAGE,
                    strAgreeDate, strExamDate, strExamDate1, strDeptName, strJumin, strStomachEndoDate, strColonEndoDate, strEndo1, strEndo2, strMUN);
                }
                else
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text.Trim() == strPtno)
                        {
                            strForm = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                            fn_SET_NeoPenDesk(strForm, FnFileCnt, ref strCmd, FstrDrNo, strPtno, strSName, strSex, nAGE,
                            strAgreeDate, strExamDate, strExamDate1, strDeptName, strJumin, strStomachEndoDate, strColonEndoDate, strEndo1, strEndo2, strMUN);
                        }
                    }
                }
            }

            DirectoryInfo Dir = new DirectoryInfo("C:\\_spool");

            if (Dir.Exists == false)
            {
                MessageBox.Show("동의서 실행파일이 없습니다!(C:\\_spool\\PenToolController.exe)" + "\r\n" + "전산팀으로 문의 바랍니다!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                timer1.Enabled = true;
                VB.Shell(strCmd, "NormalFocus");
            }

            //건강검진 동시진행 동의서 프로그램을 기다린다.
            Process[] ProcessEx = Process.GetProcessesByName("PenToolController");

            if (ProcessEx.Length > 0)
            {
                Process[] Pro1 = Process.GetProcessesByName("PenToolController");
                Process CurPro = Process.GetCurrentProcess();
                foreach (Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        //동의서 프로그램이 종료될때까지 기다림.
                        Proc.WaitForExit();
                    }
                }
            }

            Application.DoEvents();

            this.Close();
            return;

            //this.WindowState = FormWindowState.Minimized;

        }

        private void fn_SET_NeoPenDesk(string strForm, int fnFileCnt, ref string strCmd, string fstrDoct, string strPtno, string strSName, string strSex,
            long nAGE, string strAgreeDate, string strExamDate, string strExamDate1, string strDeptName, string strJumin, string strStomachEndoDate, string strColonEndoDate, string strEndo1, string strEndo2, StringBuilder strMUN)
        {
            string strMunToStr = "";
            string strExName = "";
            string strExName2 = "";

            //신규
            if (strForm == "D10")
            {
                strMunToStr = String.Join(";", strMUN);

                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D10";
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D11";
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D12";
                strCmd = strCmd + "P;@C@;D10_" + fstrDoct + ".ini;D10_" + fstrDoct + ";" + strPtno + ";" + strSName + "(" + strSex + "/" + nAGE.To<string>("0") + ")" + ";" + strJumin + ";";
                strCmd = strCmd + strAgreeDate + ";" + strStomachEndoDate + ";" + strDeptName + ";";
                strCmd = strCmd + strMunToStr;
                strCmd = strCmd + "P;@C@;D11.ini" + ";" + ";" + strEndo2 + ";" + strEndo1 + ";" + "D11; ;";
                strCmd = strCmd + "P;@C@;D12.ini;D12; ;";
            }
            else if (strForm == "D20")  
            {
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D20";
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D21";
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D22";
                strCmd = strCmd + "P;@C@;D20_" + fstrDoct + ".ini;D20_" + fstrDoct + ";" + strPtno + ";" + strSName + "(" + strSex + "/" + nAGE.To<string>("0") + ")" + ";" + strJumin + ";";
                strCmd = strCmd + strAgreeDate + ";" + strColonEndoDate + ";" + strDeptName + ";";
                strCmd = strCmd + "P;@C@;D21.ini;D21; ;";
                strCmd = strCmd + "P;@C@;D22.ini;D22; ;";
            }
            else if (strForm == "D30")
            {
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D30";
                FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D31";

                strCmd = strCmd + "P;@C@;D30_" + fstrDoct + ".ini;D30_" + fstrDoct + ";" + strPtno + ";" + strSName + ";" + strSex + "/" + nAGE.To<string>("0") + ";";
                strCmd = strCmd + strAgreeDate + ";" + strExamDate + ";" + strDeptName + ";";
                strCmd = strCmd + "P;@C@;D31.ini;D31; ;";
            }
            else if (strForm == "D40")
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

                    if (!strExName2.IsNullOrEmpty()) { strExName2 = VB.Left(strExName2, strExName2.Length - 2); }

                    strExName = VB.Pstr(strExName2, "{}", 1);
                    strExName2 = VB.Pstr(strExName2, strExName + "{}", 2);
                    if (!strExName2.IsNullOrEmpty()) { strExName2 = VB.Replace(strExName2, "{}", ","); }

                    FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D40";
                    FnFileCnt = FnFileCnt + 1; FstrFilePath[FnFileCnt] = "D41";

                    //strCT = "CT검사";

                    strCmd = strCmd + "P;@C@;D40_" + fstrDoct + ".ini;D40_" + fstrDoct + ";" + strPtno + ";" + strSName + "(" + strSex + "/" + nAGE.To<string>("0") + ")" + ";" + strJumin + ";";
                    strCmd = strCmd + strAgreeDate + ";" + strExName + ";" + strExamDate1 + ";" + strDeptName + ";";
                    strCmd = strCmd + "P;@C@;D41.ini;D41; ;";
                }
            }
        }

        private void Data_Delete()
        {
            string strRowid = "";

            try
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strRowid = "";

                    if (SS1.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        strRowid = SS1.ActiveSheet.Cells[i, 13].Text;

                        if (!strRowid.IsNullOrEmpty())
                        {
                            if (!hicConsentService.UpDateDelDateByRowid(strRowid))
                            {
                                MessageBox.Show("동의서 Data 삭제시 오류 발생!", "오류");
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("삭제완료!", "확인");
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Screen_Display(FpSpread argSpd)
        {
            argSpd.ActiveSheet.RowCount = 0;

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
                "D40"
            };

            try
            {
                //스레드 시작
                //thread = new Thread(tProcess);
                //thread.Start();

                SS1.DataSource = hicConsentService.GetListByItems(FstrFDate, FstrTDate, FstrSName, FstrDept, FstrJob, FnWRTNO, lstFrmCD);
                
                if (SS1.ActiveSheet.RowCount == 1)
                {
                    SS1.ActiveSheet.Cells[0, 0].Text = "Y";

                    Data_Save();
                }

                Cursor.Current = Cursors.Default;

                this.btnExit.Enabled = true;
                this.SS1.Enabled = true;

                if (SS1.ActiveSheet.RowCount == 1)
                {
                    rdoDept0.Checked = true;
                    Data_Save();
                }
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
    }
}
