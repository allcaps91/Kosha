using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanFirstListConditionalsearch.cs
/// Description     : 1차 검진자 조건검색
/// Author          : 이상훈
/// Create Date     : 2019-12-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmFirstList.frm(HcPan41)" />

namespace HC_Pan
{
    public partial class frmHcPanFirstListConditionalsearch : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicPatientService hicPatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicExcodeService hicExcodeService = null;
        HicResultService hicResultService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResDentalService hicResDentalService = null;
        HicXrayResultService hicXrayResultService = null;
        XrayResultnewService xrayResultnewService = null;
        EndoJupmstResultService endoJupmstResultService = null;
        HicResultHisService hicResultHisService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicRescodeService hicRescodeService = null;
        EtcJupmstService etcJupmstService = null;
        XrayResultnewDrService xrayResultnewDrService = null;
        ExamAnatmstService examAnatmstService = null;
        HicMemoService hicMemoService = null;
        HicJepsuResBohum2Service hicJepsuResBohum2Service = null;

        HicExjongService hicExjongService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        //string FstrFirstGjJong;
        List<string> FstrFirstGjJong = new List<string>();
        bool FboolBreak;

        public frmHcPanFirstListConditionalsearch()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            comHpcLibBService = new ComHpcLibBService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicPatientService = new HicPatientService();
            heaJepsuService = new HeaJepsuService();
            hicExcodeService = new HicExcodeService();
            hicResultService = new HicResultService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResDentalService = new HicResDentalService();
            hicXrayResultService = new HicXrayResultService();
            xrayResultnewService = new XrayResultnewService();
            endoJupmstResultService = new EndoJupmstResultService();
            hicResultHisService = new HicResultHisService();
            hicJepsuResultService = new HicJepsuResultService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicRescodeService = new HicRescodeService();
            etcJupmstService = new EtcJupmstService();
            xrayResultnewDrService = new XrayResultnewDrService();
            examAnatmstService = new ExamAnatmstService();
            hicMemoService = new HicMemoService();
            hicJepsuResBohum2Service = new HicJepsuResBohum2Service();

            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.chkFirst11.Click += new EventHandler(eChkClick);
            this.chkFirst12.Click += new EventHandler(eChkClick);
            this.chkFirst13.Click += new EventHandler(eChkClick);
            this.chkFirst14.Click += new EventHandler(eChkClick);
            this.chkFirst15.Click += new EventHandler(eChkClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.cboFirst1.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboFirst2.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboJob.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboYear.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboJob.Click += new EventHandler(eCboClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";

            fn_Combo_Add();
        }

        void fn_Combo_Add()
        {
            long nYear = 0;
            List<string> strBun = new List<string>();
            string strChasu = "1";

            //검진년도
            cboYear.Items.Clear();
            nYear = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYear));
                nYear -= 1;
            }
            cboYear.SelectedIndex = 0;

            //검진방법
            cboJob.Items.Add("1.1차 실시자 명단");
            cboJob.Items.Add("2.1차 결과 미입력");
            cboJob.Items.Add("3.1차 미판정");
            cboJob.Items.Add("4.2차 대상자 명단");
            cboJob.Items.Add("5.2차 미실시자 명단");
            cboJob.Items.Add("6.2차 미판정 명단");
            cboJob.SelectedIndex = 0;

            //1차검진 종류
            FstrFirstGjJong.Clear();
            strBun.Clear();
            strBun.Add("1");
            strBun.Add("2");

            List<HIC_EXJONG> list2 = hicExjongService.GetCodeName(strBun, strChasu);

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            for (int i = 0; i < list2.Count; i++)
            {
                cboJong.Items.Add(list2[i].CODE + "." + list2[i].NAME.Trim());
                FstrFirstGjJong.Add(list2[i].CODE);
            }
            cboJong.SelectedIndex = 0;

            //정상B
            cboFirst1.Items.Clear();
            cboFirst1.Items.Add("*");
            cboFirst1.Items.Add("1.비만관리");
            cboFirst1.Items.Add("2.혈압관리");
            cboFirst1.Items.Add("3.콜레스트롤관리");
            cboFirst1.Items.Add("4.간기능관리");
            cboFirst1.Items.Add("5.당뇨관리");
            cboFirst1.Items.Add("6.신장기능관리");
            cboFirst1.Items.Add("7.빈혈관리");
            cboFirst1.Items.Add("8.부인과질환관리");
            cboFirst1.SelectedIndex = 0;

            //질환의심
            cboFirst2.Items.Clear();
            cboFirst2.Items.Add("*");
            cboFirst2.Items.Add("01.폐결핵의심");
            cboFirst2.Items.Add("02.기타흉부질환의심");
            cboFirst2.Items.Add("03.고혈압의심");
            cboFirst2.Items.Add("04.고지혈증의심");
            cboFirst2.Items.Add("05.간장질환의심");
            cboFirst2.Items.Add("06.당뇨질환의심");
            cboFirst2.Items.Add("07.신장질환의심");
            cboFirst2.Items.Add("08.빈혈증의심");
            cboFirst2.Items.Add("09.부인과질환의심");
            cboFirst2.Items.Add("10.자궁경부암의심");
            cboFirst2.Items.Add("11.기타질환의심");
            cboFirst2.SelectedIndex = 0;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
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
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;

                //bool boolFirstRead = false;
                //bool boolSecondRead  = false;

                int nRow = 0;
                int nCol = 0;
                //string strDAT ="";
                List<string> strDAT = new List<string>();
                string strPANO ="";
                string strYear ="";
                string strBangi ="";
                string strJob  ="";
                
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                string strGjYear = "";
                string strGjJong = "";
                string strSort = "";
                string strChkFirst1 = "";
                string strChkFirst2 = "";

                strJob = VB.Left(cboJob.Text, 1);

                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;
                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }

                strGjYear = cboYear.Text;
                strGjJong = VB.Left(cboJong.Text, 2);
                if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else
                {
                    strSort = "2";
                }

                if (chkFirst11.Checked == false)
                {
                    strDAT.Clear();
                    if (chkFirst12.Checked == true) strDAT.Add("1");
                    if (chkFirst13.Checked == true) strDAT.Add("2");
                    if (chkFirst14.Checked == true) strDAT.Add("3");
                    if (chkFirst15.Checked == true) strDAT.Add("5");                    
                }

                //정상B 항목 선택
                switch (VB.Left(cboFirst1.Text, 1))
                {
                    case "1":
                        strChkFirst1 = "1";
                        break;
                    case "2":
                        strChkFirst1 = "2";
                        break;
                    case "3":
                        strChkFirst1 = "3";
                        break;
                    case "4":
                        strChkFirst1 = "4";
                        break;
                    case "5":
                        strChkFirst1 = "5";
                        break;
                    case "6":
                        strChkFirst1 = "6";
                        break;
                    case "7":
                        strChkFirst1 = "7";
                        break;
                    case "8":
                        strChkFirst1 = "8";
                        break;
                    default:
                        break;
                }

                //질환의심 항목 선택
                switch (VB.Left(cboFirst2.Text, 2))
                {
                    case "01":
                        strChkFirst2 = "01";
                        break;
                    case "02":
                        strChkFirst2 = "02";
                        break;
                    case "03":
                        strChkFirst2 = "03";
                        break;
                    case "04":
                        strChkFirst2 = "04";
                        break;
                    case "05":
                        strChkFirst2 = "05";
                        break;
                    case "06":
                        strChkFirst2 = "06";
                        break;
                    case "07":
                        strChkFirst2 = "07";
                        break;
                    case "08":
                        strChkFirst2 = "08";
                        break;
                    case "09":
                        strChkFirst2 = "09";
                        break;
                    case "10":
                        strChkFirst2 = "10";
                        break;
                    case "11":
                        strChkFirst2 = "11";
                        break;
                    default:
                        break;
                }

                sp.Spread_All_Clear(SS1);
                //SS1.ActiveSheet.RowCount = 37;

                //건진 접수마스타에서 대상자를 읽음
                if (strJob == "1" || strJob == "2" || strJob == "3")    //1.1차전체,2.결과미입력,3.1차미판정
                {
                    List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeGjJong(strFrDate, strToDate, strGjYear, strGjJong, FstrFirstGjJong, nLtdCode, strSort);
                    nRead = list.Count;

                    for (int i = 0; i < nRead; i++)
                    {
                        fn_Spread_Display(nRead, i, strJob, list[i].WRTNO, strDAT, strChkFirst1, strChkFirst2, list[i].GJBANGI, list[i].JEPDATE.ToString(),
                                          list[i].SNAME, list[i].AGE, list[i].SEX, list[i].GJJONG, list[i].LTDCODE, list[i].CHUNGGUYN, list[i].DENTCHUNGGUYN,
                                          "", "", "", "", "", "", "", "", "", "", "", "",
                                          "", "", "", "", "", "",
                                          "", list[i].PANO.ToString(), "", "", list[i].GBSTS, strGjYear);
                        if (FboolBreak == true)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyGjYearGjjong(strGjYear, strGjJong, nLtdCode, strJob, FstrFirstGjJong, strDAT, strChkFirst1, strChkFirst2, strSort);
                    nRead = list.Count;

                    for (int i = 0; i < nRead; i++)
                    {
                        fn_Spread_Display(nRead, i, strJob, list[i].WRTNO, strDAT, strChkFirst1, strChkFirst2, list[i].GJBANGI, list[i].JEPDATE.ToString(),
                                          list[i].SNAME, list[i].AGE, list[i].SEX, list[i].GJJONG, list[i].LTDCODE, list[i].CHUNGGUYN, list[i].DENTCHUNGGUYN,
                                          list[i].PANJENGB1, list[i].PANJENGB2, list[i].PANJENGB3, list[i].PANJENGB4, list[i].PANJENGB5, list[i].PANJENGB6,
                                          list[i].PANJENGB7, list[i].PANJENGB8, list[i].PANJENGR1, list[i].PANJENGR2, list[i].PANJENGR3, list[i].PANJENGR4,
                                          list[i].PANJENGR5, list[i].PANJENGR6, list[i].PANJENGR7, list[i].PANJENGR8, list[i].PANJENGR9, list[i].PANJENGR10,
                                          list[i].PANJENGR11, list[i].PANO.ToString(), list[i].PANJENG, list[i].PANJENGDATE.ToString(), list[i].GBSTS, strGjYear);

                        if (FboolBreak == true)
                        {
                            break;
                        }
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "1차검진자 조회";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void fn_Spread_Display(int nRead, int i, string strJob, long nWrtNo, List<string> strDAT, string strChkFirst1, string strChkFirst2,
                               string strGJBANGI, string strJEPDATE, string strSNAME, long nAGE, string strSEX, string  strGJJONG, long nLTDCODE,
                               string strCHUNGGUYN, string strDENTCHUNGGUYN, string strPanjengB1, string strPanjengB2, string strPanjengB3, 
                               string strPanjengB4, string strPanjengB5, string strPanjengB6, string strPanjengB7, string strPanjengB8, 
                               string strPanjengR1, string strPanjengR2, string strPanjengR3, string strPanjengR4, string strPanjengR5,
                               string strPanjengR6, string strPanjengR7, string strPanjengR8, string strPanjengR9, string strPanjengR10, 
                               string strPanjengR11, string strPANO, string strPanjeng, string strPanjengDate, string strGbSTS, string strGjYear)
        {
            int j = 0;
            int nRow = 0;
            int nCol = 0;
            bool boolFirstRead = false;
            bool boolSecondRead = false;
            string strOK = "";
            string strSPaNo = "";
            string strYear = "";

            string SinglestrPanjengB1 = "";
            string SinglestrPanjengB2 = "";
            string SinglestrPanjengB3 = "";
            string SinglestrPanjengB4 = "";
            string SinglestrPanjengB5 = "";
            string SinglestrPanjengB6 = "";
            string SinglestrPanjengB7 = "";
            string SinglestrPanjengB8 = "";

            string SinglestrPanjengR1 = "";
            string SinglestrPanjengR2 = "";
            string SinglestrPanjengR3 = "";
            string SinglestrPanjengR4 = "";
            string SinglestrPanjengR5 = "";
            string SinglestrPanjengR6 = "";
            string SinglestrPanjengR7 = "";
            string SinglestrPanjengR8 = "";
            string SinglestrPanjengR9 = "";
            string SinglestrPanjengR10 = "";
            string SinglestrPanjengR11 = "";

            strOK = "OK";
            boolFirstRead = true;

            //1차전체,결과미입력,1차미판정
            if (strJob == "1" || strJob == "2" || strJob == "3")
            {
                HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtnoPanjeng(nWrtNo, strDAT, strChkFirst1, strChkFirst2);

                if (list.IsNullOrEmpty())
                {
                    boolFirstRead = false;
                }
                if (boolFirstRead == true)
                {
                    //1차 미판정
                    if (strJob == "3" && list.PANJENGDATE.IsNullOrEmpty())
                    {
                        strOK = "NO";
                    }
                    //판정결과 조건검색을 설정하였고 1차판정을 하지 않았으면 표시 않함
                    if (chkFirst11.Checked == false && list.PANJENGDATE.IsNullOrEmpty())
                    {
                        strOK = "NO";
                    }
                
                    SinglestrPanjengB1 = list.PANJENGB1;
                    SinglestrPanjengB2 = list.PANJENGB2;
                    SinglestrPanjengB3 = list.PANJENGB3;
                    SinglestrPanjengB4 = list.PANJENGB4;
                    SinglestrPanjengB5 = list.PANJENGB5;
                    SinglestrPanjengB6 = list.PANJENGB6;
                    SinglestrPanjengB7 = list.PANJENGB7;
                    SinglestrPanjengB8 = list.PANJENGB8;

                    SinglestrPanjengR1 = list.PANJENGR1;
                    SinglestrPanjengR2 = list.PANJENGR2;
                    SinglestrPanjengR3 = list.PANJENGR3;
                    SinglestrPanjengR4 = list.PANJENGR4;
                    SinglestrPanjengR5 = list.PANJENGR5;
                    SinglestrPanjengR6 = list.PANJENGR6;
                    SinglestrPanjengR7 = list.PANJENGR7;
                    SinglestrPanjengR8 = list.PANJENGR8;
                    SinglestrPanjengR9 = list.PANJENGR9;
                    SinglestrPanjengR10 = list.PANJENGR10;
                    SinglestrPanjengR11 = list.PANJENGR11;
                }
            }

            //2차판정 결과를 READ
            strYear = strGjYear;
            strSPaNo = strPANO;

            //2차 접수 및 판정결과를 읽음
            HIC_JEPSU_RES_BOHUM2 list2 = hicJepsuResBohum2Service.GetItembyPanoGjYearGjBangiJepsuDate(strSPaNo, strGjYear, strGJBANGI, strJEPDATE);

            boolSecondRead = true;
            if (list2.IsNullOrEmpty())
            {
                boolSecondRead = false;
            }

            //----------( 검색조건에 맞는지 점검 )--------------------
            if (strJob == "4")  //2차 대상자 명단
            {
                switch (strPanjeng)
                {
                    case "3":
                    case "5":
                        strOK = "OK";
                        break;
                    default:
                        strOK = "NO";
                        break;
                }
            }
            else if (strJob == "5")
            {
                strOK = "NO";
                if (boolSecondRead == false)
                {
                    strOK = "OK";
                }
            }
            else if (strJob == "6")
            {
                strOK = "NO";
                if (boolSecondRead == true && list2.PANJENGDATE.IsNullOrEmpty())
                {
                    strOK = "OK";
                }
            }

            //----------------( 검색조건에 맞으면 쉬트에 표시함 )---------------
            FboolBreak = false;
            if (strOK == "OK")
            {
                if (nRow >= 2000)
                {
                    MessageBox.Show("조회할 자료가 2000건을 초과하여 2000건만 표시함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FboolBreak = true;
                    return;
                }
                nRow += 1;
                if (nRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = nRow;
                }

                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = strSNAME; //성명
                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = nAGE.ToString() + "/" + strSEX;
                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = VB.Mid(strJEPDATE, 6, 2) + "/" + VB.Mid(strJEPDATE, 9, 2);   //검진일자
                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = strGJJONG;    //종류

                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_Ltd_Name(nLTDCODE.ToString());   //상호
                }

                switch (strGbSTS)   //처리상태
                {
                    case "":
                    case "1":
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "접";
                        break;
                    case "2":
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "입";
                        break;
                    case "3":
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "판";
                        break;
                    case "D":
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "삭";
                        break;
                    default:
                        break;
                }
                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = strCHUNGGUYN;        //청구구분
                SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strDENTCHUNGGUYN;    //구강청구

                if (boolSecondRead == true)
                {
                    //1차전체,결과미입력,1차미판정
                    if (strJob == "1" || strJob == "2" || strJob == "3")
                    {
                        nCol = 10;

                        switch (list2.PANJENG)
                        {
                            case "1":
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "A";
                                break;
                            case "2":
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "A";
                                break;
                            case "3":
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "A";
                                break;
                            case "5":
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "A";
                                break;
                            default:
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "-";
                                break;
                        }

                        if (SinglestrPanjengB1 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "◎";     //비만
                        if (SinglestrPanjengB2 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "◎";     //혈압
                        if (SinglestrPanjengB3 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "◎";     //콜레스트롤
                        if (SinglestrPanjengB4 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "◎";     //간기능
                        if (SinglestrPanjengB5 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "◎";     //당뇨
                        if (SinglestrPanjengB6 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "◎";     //신장
                        if (SinglestrPanjengB7 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "◎";     //빈혈
                        if (SinglestrPanjengB8 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "◎";     //부인과

                        nCol = 20;
                        if (SinglestrPanjengR1 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "◎";         //폐결핵
                        if (SinglestrPanjengR2 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "◎";     //기타흉부
                        if (SinglestrPanjengR3 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "◎";     //고혈압
                        if (SinglestrPanjengR4 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "◎";     //고지혈
                        if (SinglestrPanjengR5 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "◎";     //간장질환
                        if (SinglestrPanjengR6 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "◎";     //당뇨질환
                        if (SinglestrPanjengR7 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "◎";     //신장질환
                        if (SinglestrPanjengR8 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "◎";     //빈혈증
                        if (SinglestrPanjengR9 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "◎";     //부인과
                        if (SinglestrPanjengR10 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 9].Text = "◎";    //자궁경부
                        if (SinglestrPanjengR11 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 10].Text = "◎";   //기타질환
                    }
                    else
                    {
                        nCol = 10;
                        switch (strPanjeng)
                        {
                            case "1":
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "A";
                                break;
                            case "2":
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "B";
                                break;
                            case "3":
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "R";
                                break;
                            case "5":
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "BR";
                                break;
                            default:
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "-";
                                break;
                        }
                        if (strPanjengB1 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "◎";     //비만
                        if (strPanjengB2 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "◎";     //혈압
                        if (strPanjengB3 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "◎";     //콜레스트롤
                        if (strPanjengB4 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "◎";     //간기능
                        if (strPanjengB5 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "◎";     //당뇨
                        if (strPanjengB6 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "◎";     //신장
                        if (strPanjengB7 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "◎";     //빈혈
                        if (strPanjengB8 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "◎";     //부인과

                        nCol = 20;
                        if (strPanjengR1 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = "◎";         //폐결핵
                        if (strPanjengR2 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "◎";     //기타흉부
                        if (strPanjengR3 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "◎";     //고혈압
                        if (strPanjengR4 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "◎";     //고지혈
                        if (strPanjengR5 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "◎";     //간장질환
                        if (strPanjengR6 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "◎";     //당뇨질환
                        if (strPanjengR7 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "◎";     //신장질환
                        if (strPanjengR8 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "◎";     //빈혈증
                        if (strPanjengR9 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "◎";     //부인과
                        if (strPanjengR10 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 9].Text = "◎";    //자궁경부
                        if (strPanjengR11 == "1") SS1.ActiveSheet.Cells[nRow - 1, nCol + 10].Text = "◎";   //기타질환
                    }
                }

                if (boolSecondRead == true)
                {
                    j -= 1;
                    nCol = 32;
                    SS1.ActiveSheet.Cells[i, nCol].Text = list2.PANJENGDATE;
                    switch (list2.PANJENG) //2차 판정
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 1].Text = "-";
                            break;
                    }

                    switch (list2.CHEST_RES) 
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 2].Text = "-";
                            break;
                    }

                    switch (list2.CYCLE_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 3].Text = "-";
                            break;
                    }

                    switch (list2.GOJI_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 4].Text = "-";
                            break;
                    }

                    switch (list2.LIVER_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 5].Text = "-";
                            break;
                    }

                    switch (list2.KIDNEY_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 6].Text = "-";
                            break;
                    }

                    switch (list2.AMEMIA_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 7].Text = "-";
                            break;
                    }

                    switch (list2.DIABETES_RES)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "A";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "B";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "C";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "D";
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow - 1, nCol + 8].Text = "-";
                            break;
                    }
                }
                SS1.ActiveSheet.Cells[nRow - 1, 42].Text = nWrtNo.ToString();
                SS1.ActiveSheet.Cells[nRow - 1, 43].Text = strPANO;
            }
        }

        void eChkClick(object sender, EventArgs e)
        {
            sp.Spread_All_Clear(SS1);
            if (sender != chkFirst11 && chkFirst11.Checked == false)
            {
                chkFirst11.Checked = false;
                cboFirst1.Enabled = true;
                cboFirst2.Enabled = true;
            }
            else
            {
                chkFirst12.Checked = false;
                chkFirst13.Checked = false;
                chkFirst14.Checked = false;
                chkFirst15.Checked = false;
                cboFirst1.SelectedIndex = 0;
                cboFirst2.SelectedIndex = 0;
                cboFirst1.Enabled = false;
                cboFirst2.Enabled = false;
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            dtpFDate.Text = cboYear.Text + "-01-01";
            dtpTDate.Text = clsPublic.GstrSysDate;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            //clsHcVariable.GnWRTNO = long.Parse(SS1.ActiveSheet.Cells[e.Row, 42].Text);  //접수번호
            //clsHcVariable.GnPano = long.Parse(SS1.ActiveSheet.Cells[e.Row, 43].Text);   //등록번호
            //SS1.ActiveSheet.Cells[e.Row, e.Column]
        //    If Col< 11 Then
        //'        FrmJuso.Show 1
        //   ElseIf Col > 11 Then
        //        'FrmResFirst.Show 1
        //   ElseIf Col< 25 Then
        //        'FrmResFirst.Show 1
        //   ElseIf SS1.Col > 26 Then
        //        If SS1.Text <> "" Then
        //            'FrmResSecond.Show 1
        //        End If
        //   End If
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
