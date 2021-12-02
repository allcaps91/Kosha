using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHa_HcAmtHalin.cs
/// Description     : 종검수검자 일반검진 비용 할인 적용
/// Author          : 김민철
/// Create Date     : 2020-04-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm일반검진수가적용(Frm일반검진수가적용.frm)" />
namespace HC_Main
{
    public partial class frmHa_HcAmtHalin : Form
    {
        frmHcNhicView frmHcNhicView = null;     //자격조회 정보 
        WorkNhicService workNhicService = null;
        BasBcodeService basBcodeService = null;
        HicGroupcodeService hicGroupcodeService = null;
        HicGroupexamService hicGroupexamService = null;
        HicGroupexamGroupcodeExcodeService hicGroupexamGroupcodeExcodeService = null;
        HeaCodeService heaCodeService = null;

        clsHaBase cHB   = null;
        clsSpread cSpd  = null;
        FpSpread ssExam = null;

        int[] FnAm = new int[7];
        long FnPano = 0;
        string FstrSDate = string.Empty;
        string FstrSName = string.Empty;

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        public frmHa_HcAmtHalin()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHa_HcAmtHalin(FpSpread ss, string argSDate, string argSName, long argPano)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            ssExam = ss;
            FstrSDate = argSDate;
            FstrSName = argSName;
            FnPano = argPano;
        }

        private void SetControl()
        {
            frmHcNhicView = new frmHcNhicView();
            workNhicService = new WorkNhicService();
            basBcodeService = new BasBcodeService();
            hicGroupcodeService = new HicGroupcodeService();
            hicGroupexamService = new HicGroupexamService();
            hicGroupexamGroupcodeExcodeService = new HicGroupexamGroupcodeExcodeService();

            cSpd = new clsSpread();
            cHB = new clsHaBase();

            ssExam  = new FpSpread();
            
            cboBuRate.Items.Clear();
            cboBuRate.Items.Add("01.감액100%");
            cboBuRate.Items.Add("02.감액 90%");

            SS1.Initialize();
            SS1.AddColumn("제외", "",                                42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("코드", nameof(HIC_GROUPCODE.CODE),       160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("묶음코드명", nameof(HIC_GROUPCODE.NAME),  82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("부담율", "",                              42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("금액", "0",                               42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("", nameof(HIC_GROUPCODE.GBSUGA),          42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = true });
            SS1.AddColumn("", nameof(HIC_GROUPCODE.GBAM),            42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = true });
            SS1.AddColumn("", nameof(HIC_GROUPCODE.GBSELECT),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = true });

            ss3.Initialize();
            ss3.AddColumn("", "", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", nameof(HIC_GROUPCODE.CODE), 160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", nameof(HIC_GROUPCODE.NAME), 82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", "", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", "0", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", nameof(HIC_GROUPCODE.GBSUGA), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", nameof(HIC_GROUPCODE.GBAM), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            ss3.AddColumn("", nameof(HIC_GROUPCODE.GBSELECT), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
        }

        private void SetEvent()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnExit.Click  += new EventHandler(eFormClose);
            this.btnSave.Click  += new EventHandler(eBtnClick);
            this.btnSel.Click   += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Accept_HalinAmt();
            }
            else if (sender == btnSel)
            {
                Select_GroupCode();
            }
            else if (sender == btnMatch)
            {
                Run_Code_Matching();
            }
        }

        private void Run_Code_Matching()
        {
            string strCODE = string.Empty;
            string strHcGroupCode = string.Empty;
            string strHaCode = string.Empty;
            string strOK = string.Empty;
            string strBuRate = string.Empty;

            int nSelf = 0;
            int nJoRate = 0;

            long nTotAmt = 0;
            long nAmt = 0;
            long nAmAmt = 0;

            List<string> lstCODE = new List<string>();

            SS2.ActiveSheet.Cells[0, 2, SS2.ActiveSheet.RowCount - 1, 2].BackColor = System.Drawing.Color.White;

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                SS2.ActiveSheet.Cells[i, 2].Text = "";
            }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                lstCODE.Add(SS1.ActiveSheet.Cells[i, 1].Text.Trim());
            }

            if (lstCODE.Count > 0)
            {
                for (int k = 0; k < lstCODE.Count; k++)
                {
                    if (SS1.ActiveSheet.Cells[k, 0].Text != "True")
                    {
                        SS1.ActiveSheet.Cells[k, 0].Text = "True";
                        strHcGroupCode = lstCODE[k];

                        if (strHcGroupCode != "")
                        {
                            strOK = "";
                            //TODO : List String 으로 받지 못함
                            List<string> strExCode = hicGroupexamService.GetListExcodeByGrpCode(strHcGroupCode);
                            if (strExCode.Count > 0)
                            {
                                for (int i = 0; i < strExCode.Count; i++)
                                {
                                    strHaCode = heaCodeService.GetItemByGubun1(strExCode[i]);

                                    if (!strHaCode.IsNullOrEmpty())
                                    {
                                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                                        {
                                            if (strHaCode == SS2.ActiveSheet.Cells[j, 0].Text.Trim())
                                            {
                                                SS2.ActiveSheet.Cells[j, 2].Text = "○";
                                                SS2.ActiveSheet.Cells[j, 2].BackColor = System.Drawing.Color.FromArgb(255, 187, 187);
                                                strOK = "OK";
                                            }

                                            //구강은 강제로 매칭
                                            if (strHaCode == "ZD00")
                                            {
                                                SS2.ActiveSheet.Cells[j, 2].Text = "○";
                                                SS2.ActiveSheet.Cells[j, 2].BackColor = System.Drawing.Color.FromArgb(255, 187, 187);
                                                strOK = "OK";
                                            }
                                        }
                                    }
                                }

                                if (strOK == "OK")
                                {
                                    SS1.ActiveSheet.Cells[k, 0].Text = "";
                                }
                            }
                        }
                    }
                }
            }

            //수납내역 확인
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i,0].Text != "True")
                {
                    nSelf = SS1.ActiveSheet.Cells[i, 3].Text.To<int>();
                    nAmt = SS1.ActiveSheet.Cells[i, 4].Text.To<int>();

                    if (nSelf == 0) { nSelf = strBuRate.To<int>(); }

                    switch (nSelf)
                    {
                        case 1: nJoRate = 0;  break;
                        case 2: nJoRate = 0;  break;
                        default: nJoRate = 0; break;
                    }

                    if (nJoRate > 0)
                    {
                        nAmAmt = (long)Math.Truncate((nAmt * nJoRate / 100.0) / 10.0) * 10;
                        nTotAmt += nAmAmt;
                    }

                    SS1.ActiveSheet.Cells[i, 8].Text = nAmAmt.To<string>();
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 8].Text = "0";
                }
            }

            lblAmt.Text = VB.Format(nTotAmt, "###,###,##0");
        }


        private void Select_GroupCode()
        {
            List<string> strJong = new List<string>();

            switch (clsHcType.THNV.hGKiho.Substring(0, 1))
            {
                case "1":
                case "2":
                case "3": strJong.Add("13"); strJong.Add("31"); break;
                case "5":
                case "6":
                    if (clsHcType.THNV.hJaGubun == "직장가입자")
                    {
                        strJong.Add("12"); strJong.Add("31");
                    }
                    else
                    {
                        strJong.Add("13"); strJong.Add("31");
                    }
                    break;
                case "7":
                case "8":
                    if (clsHcType.THNV.hJaGubun == "직장가입자")
                    {
                        strJong.Add("11"); strJong.Add("31");
                    }
                    else
                    {
                        strJong.Add("11"); strJong.Add("31");
                    }
                    break;
                default:
                    strJong.Add("11"); strJong.Add("12"); strJong.Add("13"); strJong.Add("31");
                    break;
            }

            frmHcGroupCode frm = new frmHcGroupCode(strJong);
            frm.ShowDialog();

            Read_GroupCode2(strJong);
        }

        private void Accept_HalinAmt()
        {
            int ii = -1;
            string strBuRate = string.Empty;

            //할인적용사항 저장
            if (lblAmt.Text.Replace(",", "").To<long>() > 0)
            {
                clsHcType.HaGam = new clsHcType.HaMain_HEA_GAMCODE_INFO[1];
                Array.Clear(clsHcType.HaGam, 0, clsHcType.HaGam.Length);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        ii = ii + 1;
                        if (clsHcType.HaGam.Length < ii + 1) { Array.Resize(ref clsHcType.HaGam, ii + 1); }

                        clsHcType.HaGam[ii].sName       = FstrSName;
                        clsHcType.HaGam[ii].sDate       = FstrSDate;
                        clsHcType.HaGam[ii].ActDate     = DateTime.Now.ToShortDateString();
                        clsHcType.HaGam[ii].Pano        = FnPano;

                        clsHcType.HaGam[ii].GCode       = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        clsHcType.HaGam[ii].GCodeName   = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        clsHcType.HaGam[ii].BuRate      = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        clsHcType.HaGam[ii].AMT         = SS1.ActiveSheet.Cells[i, 4].Text.Replace(",", "").To<long>();
                        clsHcType.HaGam[ii].GamAmt      = SS1.ActiveSheet.Cells[i, 8].Text.Replace(",", "").To<long>();
                        //clsHcType.HaGam[ii].flag        = true;
                        clsHcType.HaGamFlag = true;
                    }
                }
            }

            rSetGstrValue(lblAmt.Text.Replace(",", "").Trim());
            this.Close();

        }

        private void eFormClose(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            themTabForm(frmHcNhicView, this.panNhic);

            //자격조회 정보 Spread Display
            frmHcNhicView.SetDisPlayByVariable();

            if (clsPublic.GstrRetValue == "Manual")
            {
                chkNhic.Checked = true;
                clsPublic.GstrRetValue = "";
            }

            btnSel.Enabled = false;

            for (int i = 0; i < FnAm.Length; i++) { FnAm[i] = 0; }

            if (chkNhic.Checked == true)
            {
                btnSel.Enabled = true;
            }
            else
            {
                //암자격 읽기
                if (!clsHcType.THNV.hCan1.Contains("대상아님")) { FnAm[1] = 3; }
                if (!clsHcType.THNV.hCan2.Contains("대상아님")) { FnAm[2] = 9; }
                if (!clsHcType.THNV.hCan3.Contains("대상아님")) { FnAm[3] = 5; }
                if (!clsHcType.THNV.hCan4.Contains("대상아님")) { FnAm[4] = 7; }
                if (!clsHcType.THNV.hCan5.Contains("대상아님")) { FnAm[5] = 11; }
                if (!clsHcType.THNV.hCan6.Contains("대상아님")) { FnAm[6] = 13; }

                //암검진을 당해 수검했던 검진은 제외함
                if (clsHcType.THNV.h위Date != ""   || clsHcType.THNV.h위HName != "")   { FnAm[1] = 0; }
                if (clsHcType.THNV.h대장Date != "" || clsHcType.THNV.h대장HName != "") { FnAm[3] = 0; }
                if (clsHcType.THNV.h간Date != ""   || clsHcType.THNV.h간HName != "")   { FnAm[4] = 0; }
                if (clsHcType.THNV.h유방Date != "" || clsHcType.THNV.h유방HName != "") { FnAm[2] = 0; }
                if (clsHcType.THNV.h자궁Date != "" || clsHcType.THNV.h자궁HName != "") { FnAm[5] = 0; }
                if (clsHcType.THNV.h폐Date != ""   || clsHcType.THNV.h폐HName != "")   { FnAm[6] = 0; }

                Read_GroupCode("11");
                Read_GroupCode("35");
            }

            int nRow = 0;
            //종검검사항목을 복사
            for (int i = 0; i < ssExam.ActiveSheet.RowCount; i++)
            {
                if (ssExam.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    nRow = nRow + 1;
                    if (SS2.ActiveSheet.RowCount < nRow)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }

                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = ssExam.ActiveSheet.Cells[i, 1].Text.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = ssExam.ActiveSheet.Cells[i, 2].Text.Trim();
                }
            }

            lblAmt.Text = "0";
        }

        private void Read_GroupCode(string argJong)
        {
            int nRow = 0;
            int nAmtNo = 0;
            string strOK = "OK";
            string strAmTemp = string.Empty;
            string strFlag = string.Empty;
            string strCODE = string.Empty;
            string strGroupGbSuga = string.Empty;
            string strGbSuga = string.Empty;
            string strCodeName = string.Empty;
            string strGbAm = string.Empty;
            string strGbSelect = string.Empty;
            string strChkCode = string.Empty;

            long nAmt = 0;  //묶음코드별 합계금액
            long nPrice = 0;

            List<string> lstCode = new List<string>();
            List<string> lstExList = new List<string>();

            List<BAS_BCODE> CodeList = basBcodeService.GetListHicExamMstByGubun(clsHcType.TEC.GUBUN, clsHcType.TEC.SEX, clsHcType.TEC.AGE, clsHcType.TEC.JEPDATE);

            if (CodeList.Count > 0)
            {
                for (int i = 0; i < CodeList.Count; i++)
                {
                    lstCode.Add(CodeList[i].GUBUN2.Trim());
                }
            }

            cSpd.Spread_All_Clear(ss3);
            cSpd.Spread_All_Clear(ss5);
            
            if (argJong == "31")
            {
                for (int i = 0; i < FnAm.Length; i++)
                {
                    if (FnAm[i] > 0)
                    {
                        strAmTemp = "OK"; break;
                    }
                }
            }
            else
            {
                if (clsHcType.THNV.h1Cha.Contains("비대상") || clsHcType.THNV.h1ChaDate != "")
                {
                    strOK = "";
                }
            }

            if (strOK == "OK")
            {
                //수납 항목을 SELECT
                ss3.DataSource = hicGroupcodeService.GetHalinListByItems(FstrSDate, argJong, lstCode, clsHcType.TEC.GUBUN, FnAm, strAmTemp, clsHcType.THNV.hCan4);
            }

            for (int i = 0; i < ss3.ActiveSheet.RowCount; i++)
            {
                strFlag = "Y";

                if (ss3.ActiveSheet.Cells[i, 0].Text == "True") { strFlag = "N";}
                strCODE = ss3.ActiveSheet.Cells[i, 1].Text;
                strGroupGbSuga = ss3.ActiveSheet.Cells[i, 5].Text;

                //중복검사는 제거함
                for (int j = 0; j < ss5.ActiveSheet.RowCount; j++)
                {
                    if (ss5.ActiveSheet.Cells[j, 2].Text.Trim() != "")
                    {
                        lstExList.Add(ss5.ActiveSheet.Cells[j, 2].Text.Trim());
                    }
                }

                //자료를 READ
                List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetListByCode(strCODE, lstExList);

                if (lst.Count > 0)
                {
                    for (int j = 0; j < lst.Count; j++)
                    {
                        strGroupGbSuga = lst[j].GBSUGA.Trim(); //그룹
                        strGbSuga = lst[j].SUGAGBN.To<string>("").Trim();     //검사항목
                        //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                        if (strGbSuga == "") { strGbSuga = strGroupGbSuga; }

                        // Amt1 = 보험수가의 80%
                        // Amt2 = 보험수가의 100%
                        // Amt3 = 보험수가의 125%
                        // Amt4 = 일반+특검 차액
                        // Amt5 = 임의수가
                        nAmtNo = strGbSuga.To<int>();

                        //전년도 건진사업이면 Old수가를 적용함
                        if (string.Compare(VB.Left(FstrSDate, 4), VB.Left(lst[j].SUDATE, 4)) <= 0)
                        {
                            nPrice = lst[j].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }
                        else
                        {
                            nPrice = lst[j].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }

                        //접수일자를 기준으로 현재수가,Old수가를 적용함
                        if (string.Compare(FstrSDate, lst[j].SUDATE) >= 0)
                        {                            
                            nPrice = lst[j].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }
                        else
                        {
                            nPrice = lst[j].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }

                        //선택한것 제외됨
                        if (strFlag == "Y")
                        {
                            nRow = nRow + 1;
                            if (nRow > ss5.ActiveSheet.RowCount)
                            {
                                ss5.ActiveSheet.RowCount = nRow;
                            }

                            ss5.ActiveSheet.Cells[nRow - 1, 0].Text = lst[j].GROUPCODE.Trim();
                            ss5.ActiveSheet.Cells[nRow - 1, 1].Text = lst[j].GROUPNAME.Trim();
                            ss5.ActiveSheet.Cells[nRow - 1, 2].Text = lst[j].EXCODE.Trim();
                            ss5.ActiveSheet.Cells[nRow - 1, 3].Text = VB.Format(nPrice, "###,###,##0");
                            ss5.ActiveSheet.Cells[nRow - 1, 4].Text = lst[j].HNAME.Trim();
                        }

                        nAmt = nAmt + nPrice;
                    }
                }
                else
                {
                    nRow = nRow + 1;
                    if (nRow > ss5.ActiveSheet.RowCount)
                    {
                        ss5.ActiveSheet.RowCount = nRow;
                    }
                    ss5.ActiveSheet.Cells[nRow - 1, 0].Text = strCODE;
                    ss5.ActiveSheet.Cells[nRow - 1, 1].Text = cHB.READ_Group_Name(strCODE);
                    ss5.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                }

                ss3.ActiveSheet.Cells[i, 4].Text = VB.Format(nAmt, "###,###,##0");
            }

            for (int i = 0; i < ss3.ActiveSheet.RowCount; i++)
            {
                strOK = "";
                strCODE         = ss3.ActiveSheet.Cells[i, 1].Text.Trim();
                strCodeName     = ss3.ActiveSheet.Cells[i, 2].Text.Trim();
                nPrice          = ss3.ActiveSheet.Cells[i, 4].Text.To<long>();
                strGroupGbSuga  = ss3.ActiveSheet.Cells[i, 5].Text.Trim();
                strGbAm         = ss3.ActiveSheet.Cells[i, 6].Text.Trim();
                strGbSelect     = ss3.ActiveSheet.Cells[i, 7].Text.Trim();

                for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                {
                    if (SS1.ActiveSheet.Cells[j, 1].Text.Trim() == strCODE)
                    {
                        strOK = "OK";
                    }
                }

                if (strOK == "")
                {
                    nRow = SS1.ActiveSheet.RowCount;
                    nRow = nRow + 1;
                    SS1.ActiveSheet.RowCount = nRow;

                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = strCODE;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = strCodeName;
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Format(nPrice, "###,###,##0");
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = strGroupGbSuga;
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = strGbAm;
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strGbSelect;
                }
            }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strChkCode = SS1.ActiveSheet.Cells[i, 1].Text;
                //TODO : 하드코딩 없애기
                if (strChkCode == "3166" || strChkCode == "3166")
                {
                    SS1.ActiveSheet.Cells[nRow, 0].Text = "True";
                }
            }

        }

        private void Read_GroupCode2(List<string> lstJong)
        {
            int nRow = 0;
            int nAmtNo = 0;
            long nAmt = 0;
            long nPrice = 0;

            string strOK = string.Empty;
            string strGroupGbSuga = string.Empty;
            string strGbSuga = string.Empty;

            if (lstJong.Count > 0)
            {
                for (int i = 0; i < lstJong.Count; i++)
                {
                    strOK = "";

                    if (lstJong[i] != "")
                    {
                        for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                        {
                            if (SS1.ActiveSheet.Cells[i, 1].Text.Trim() == lstJong[i])
                            {
                                strOK = "OK";
                                break;
                            }
                        }

                        if (strOK == "")
                        {
                            nRow = SS1.ActiveSheet.RowCount;
                            nRow += 1;
                            SS1.ActiveSheet.RowCount = nRow;

                            //수납 항목을 SELECT
                            SS1.DataSource = hicGroupcodeService.GetListByCode(lstJong[i]);

                            //자료를 READ
                            List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetListByCode(lstJong[i], null);

                            nAmt = 0; //묶음코드별 합계금액
                            if (lst.Count > 0)
                            {
                                for (int j = 0; j < lst.Count; j++)
                                {
                                    strGroupGbSuga = lst[i].GBSUGA.Trim(); //그룹
                                    strGbSuga = lst[i].SUGAGBN.Trim();     //검사항목

                                    //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                                    if (strGbSuga == "") { strGbSuga = strGroupGbSuga; }

                                    nAmtNo = strGbSuga.To<int>();

                                    //전년도 건진사업이면 Old수가를 적용함
                                    if (string.Compare(VB.Left(FstrSDate, 4), VB.Left(lst[i].SUDATE, 4)) <= 0)
                                    {
                                        nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                                    }
                                    else
                                    {
                                        nPrice = lst[i].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                                    }

                                    //접수일자를 기준으로 현재수가,Old수가를 적용함
                                    if (string.Compare(FstrSDate, lst[i].SUDATE) >= 0)
                                    {
                                        nPrice = lst[i].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                                    }
                                    else
                                    {
                                        nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                                    }

                                    nAmt = nAmt + nPrice;
                                }
                            }

                            SS1.ActiveSheet.Cells[i, 4].Text = VB.Format(nAmt, "###,###,##0");
                        }
                    }
                }
            }
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }
    }
}
