using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanView.cs
/// Description     : 일반 및 특수 판정 조회
/// Author          : 이상훈
/// Create Date     : 2019-11-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanView.frm(HcPanView)" />

namespace ComHpcLibB
{
    public partial class frmHcPanView : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        BasIllsService basIllsService = null;
        HicJepsuXMunjinExjongLtdService hicJepsuXMunjinExjongLtdService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicResSpecialService hicResSpecialService = null;
        HicCodeService hicCodeService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicMcodeService hicMcodeService = null;
        BasPatientService basPatientService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO = 0;
        long FnWrtno1 = 0; //1차
        long FnWrtno2 = 0; //2차
        long FnPano = 0;
        string FstrSex = "";
        string FstrGjJong = "";
        string FstrGjChasu = "";
        string FstrCOMMIT = "";
        string FstrUCodes = "";
        string FstrJumin = "";
        string FstrPano = "";
        string FstrSaveGbn = "";
        string FstrGbOHMS  = "";
        string FstrMCode = "";
        string FstrGbSPC = "";
        string FstrROWID = "";
        string FstrYROWID = "";  //약속판정 ROWID
        string FstrPROWID = "";

        int nRow = 0;
        int nOldCNT = 0;
        //string strExamCode = "";
        List<string> strExamCode = new List<string>();
        string strAllWRTNO = "";
        string strJepDate = "";
        string strExPan = "";
        int nHyelH = 0;
        int nHyelL = 0;
        int nHeight = 0;
        int nWeight = 0;
        int nResult = 0;

        int nREAD = 0;
        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        long FnWrtNo;
        string FsJong;

        public frmHcPanView(long nWrtNo, string sJong = "")
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            FsJong = sJong;
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            basIllsService = new BasIllsService();
            hicJepsuXMunjinExjongLtdService = new HicJepsuXMunjinExjongLtdService();
            hicResBohum1Service = new HicResBohum1Service();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicResSpecialService = new HicResSpecialService();
            hicCodeService = new HicCodeService();
            hicResBohum2Service = new HicResBohum2Service();
            hicMcodeService = new HicMcodeService();
            basPatientService = new BasPatientService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SSPan.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SSPan_Sheet1.Columns.Get(9).Visible = false;  //코드
            SSPan_Sheet1.Columns.Get(10).Visible = false;  //ROWID

            SS2_Sheet1.Columns.Get(7).Visible = true;
            SS2_Sheet1.Columns.Get(8).Visible = true;
            SS2_Sheet1.Columns.Get(9).Visible = true;
            SS2_Sheet1.Columns.Get(10).Visible = true;

            txtSogen.Text = "";
            cf.Combo_BCode_SET(clsDB.DbCon, cboPanjeng, "특수종합판정", true, 1, "");    //특수종합판정
            cf.Combo_BCode_SET(clsDB.DbCon, cboPanjeng1, "1차종합판정", true, 1, "");    //1차종합판정
            cf.Combo_BCode_SET(clsDB.DbCon, cboLiver, "간염판정", true, 1, "");          //간염판정
            cf.Combo_BCode_SET(clsDB.DbCon, cboEtcJil, "기타질환명", true, 1, "");       //기타질환명
            fn_Screen_Clear();

            //---------( 일반2차판정 Combo SET )---------------
            cf.Combo_BCode_SET(clsDB.DbCon, cboPanjeng2, "일반2차판정", true, 1, "");    //일반2차판정

            //사후관리
            for (int i = 1; i <= 3; i++)
            {   
                ComboBox cboSaHu = (Controls.Find("cboSaHu" + i.To<string>(), true)[0] as ComboBox);

                cboSaHu.Items.Add(" ");
                cboSaHu.Items.Add("1.근로금지 및 제한");
                cboSaHu.Items.Add("2.작업전환");
                cboSaHu.Items.Add("3.근로시간단축");
                cboSaHu.Items.Add("4.근무중치료");
                cboSaHu.Items.Add("5.추적검사");
                cboSaHu.Items.Add("6.보호구착용");
                cboSaHu.Items.Add("7.기타");
                cboSaHu.SelectedIndex = 0;
            }

            //판정값
            for (int i = 1; i <= 9; i++)
            {
                ComboBox cboSoGyun = (Controls.Find("cboSoGyun" + i.To<string>(), true)[0] as ComboBox);

                cboSoGyun.Items.Add(" ");
                cboSoGyun.Items.Add("1.정상A");
                cboSoGyun.Items.Add("2.정상B");
                cboSoGyun.Items.Add("3.건강주의");
                cboSoGyun.Items.Add("4.유질환");
                cboSoGyun.SelectedIndex = 0;
            }

            //기타질환명
            cf.Combo_BCode_SET(clsDB.DbCon, cboResult, "HIC_간염검사", true, 1, "");       //간염검사

            txtSogen2.Text = "";
            txtEtc_Exam.Text = "";

            //--------------( 일반2차판정 설정 끝 )--------------------
            //if (clsPublic.GstrRetValue != "")
            if (FnWrtNo != 0)
            {
                //txtWrtNo.Text = clsPublic.GstrRetValue;
                txtWrtNo.Text = FnWrtNo.To<string>();
                fn_Screen_Display();
                eSpdDClick(SSPan, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
                clsHcVariable.GnWRTNO = 0;
            }

            tabControl1.SelectedTab = tab11;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SSJik);
            sp.Spread_All_Clear(SS2);

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = " ";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = " ";

            SSMun.ActiveSheet.RowCount = 0;
            SSPan.ActiveSheet.RowCount = 0;

            FnWRTNO = 0;
            FstrSex = "";
            FstrROWID = "";
            FstrUCodes = "";
            FstrCOMMIT = "";
            FnPano = 0;
            FnWrtno1 = 0;
            FnWrtno2 = 0;
            FstrSaveGbn = "";
            FstrPano = "";
            FstrJumin = "";
            FstrGbOHMS = "";
            FstrGbSPC = "";
            FstrGjJong = "";

            txtWrtNo.Text = "";
            txtIpDate.Text = "";
            txtBuse.Text = "";
            txtJenipDate.Text = "";
            txtPTime.Text = "";
            txtEtcMsym.Text = "";
            txtGajok.Text = "";
            txtGiinsung.Text = "";
            txtNeuro.Text = "";
            txtHead.Text = "";
            txtSkin.Text = "";
            txtChest.Text = "";
            txtJengSang.Text = "";
            txtDent.Text = "";
            txtDentDoct.Text = "";
            lblDentDrName.Text = "";
            txtPanDrNo1.Text = "";
            lblDrName1.Text = "";
            txtJobYN.Text = "";
            lblJobYN.Text = "";

            chkSang1.Checked = false;
            chkSang2.Checked = false;
            chkSang3.Checked = false;
            chkSang4.Checked = false;
            chkSang5.Checked = false;

            txtYear1.Text = "";
            txtYear2.Text = "";
            txtYear3.Text = "";
            txtYear4.Text = "";
            txtYear5.Text = "";

            lblGongjeng.Text = "";
            lblHGigan.Text = "";
            lblJikjong.Text = "";
            lblUCodeName.Text = "";
            lblJengSang.Text = "";

            rdoHuyu1.Checked = false;
            rdoHuyu2.Checked = false;
            rdoSang1.Checked = false;
            rdoSang2.Checked = false;
            rdoSang3.Checked = false;

            //1차판정 결과 Clear
            txtPanDate1.Text = "";
            cboLiver.Text = "";
            cboEtcJil.Text = "";
            cboLiver.Text = "";
            cboPanjeng1.Text = "";
            txtSogen1.Text = "";

            //chkPanjengB1.Checked 
            chkPanjengB1.Checked = false;
            chkPanjengB2.Checked = false;
            chkPanjengB3.Checked = false;
            chkPanjengB4.Checked = false;
            chkPanjengB8.Checked = false;
            chkPanjengB6.Checked = false;
            chkPanjengB7.Checked = false;
            chkPanjengB5.Checked = false;

            chkPanjengR1.Checked = false;
            chkPanjengR2.Checked = false;
            chkPanjengR3.Checked = false;
            chkPanjengR4.Checked = false;
            chkPanjengR5.Checked = false;
            chkPanjengR6.Checked = false;
            chkPanjengR7.Checked = false;
            chkPanjengR8.Checked = false;
            chkPanjengR9.Checked = false;
            chkPanjengR10.Checked = false;
            chkPanjengR11.Checked = false;

            //특검 판정화면 Clear
            fn_Screen_Clear_SUB();

            pnlPanjengMain.Enabled = false;

            //--------------(일반2차판정 Clear)-----------------
            txtPanDate2.Text = "";
            txtEtc_Exam.Text = "";
            txtSogen2.Text = "";
            txtDrNo.Text = "";
            lblDrName2.Text = "";
            txtJobYN2.Text = "";
            lblJobYN2.Text = "";

            cboResult.SelectedIndex = -1;

            cboSoGyun1.SelectedIndex = -1;

            for (int i = 0; i <= 8; i++)
            {
                ComboBox cboSoGyun = (Controls.Find("cboSoGyun" + (i + 1).To<string>(), true)[0] as ComboBox);
                Label lblSogyun = (Controls.Find("lblSogyun" + (i + 1).To<string>(), true)[0] as Label);

                cboSoGyun.SelectedIndex = -1;
                lblSogyun.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
                cboSoGyun.Enabled = false;
            }
            cboPanjeng2.SelectedIndex = -1;

            for (int i = 0; i <= 2; i++)
            {
                ComboBox cboSaHu = (Controls.Find("cboSaHu" + (i + 1).To<string>(), true)[0] as ComboBox);
                cboSaHu.SelectedIndex = -1;
            }

            for (int i = 0; i <= 1; i++)
            {
                TextBox txtYujil = (Controls.Find("txtYujil" + (i + 1).To<string>(), true)[0] as TextBox);
                Label lblUName = (Controls.Find("lblUName" + (i + 1).To<string>(), true)[0] as Label);

                txtYujil.Text = "";
                lblUName.Text = "";
                txtYujil.Enabled = false;
                lblUName.Enabled = false;
            }
        }

        void fn_Screen_Clear_SUB()
        {
            cboPanjeng.SelectedIndex = -1;
            lblMName.Text = "";
            txtSogen.Text = "";
            txtSogenRemark.Text = "";
            txtJochi.Text = "";
            txtJochiRemark.Text = "";
            txtSahu.Text = "";
            lblSahuName.Text = "";
            txtYuhe.Text = "";
            lblYuheName.Text = "";
            txtPanDate.Text = "";
            txtReExam.Text = "";
            lblReExamName.Text = "";
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            txtJobYN.Text = "";
            lblJobYN.Text = "";
            lblSogenName.Text = "";
            lblJochiName.Text = "";
            FstrYROWID = ""; FstrPROWID = "";
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            string strGjYear = "";
            string strGjChasu = "";
            string strGjBangi = "";
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType  = "";
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
            int nHeight = 0;
            int nWeight = 0;
            int nResult = 0;
            string strRemark = "";
            string strExPan = "";    //검사1건의 판정결과
            long nGan1 = 0;
            long nGan2 = 0;

            double nMaxData = 0; //정상 참고치 (High)
            double nMinData = 0; //정상 참고치 (Low)

            FnWRTNO = txtWrtNo.Text.To<long>();
            nGan1 = -1;
            nGan2 = -1;

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrSex = list.SEX;
            FnPano = list.PANO;
            strGjYear = list.GJYEAR;
            strGjChasu =list.GJCHASU;
            strGjBangi =list.GJBANGI;
            strJepDate =list.JEPDATE;
            FstrGjJong = list.GJJONG;
            FstrGjChasu = strGjChasu;

            switch (FstrGjJong)
            {
                case "21":
                case "22":
                case "23":
                case "24":
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                case "19":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                    break;
                default:
                    MessageBox.Show("건진 History 조회가 불가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            if (FstrGjJong == "21" || FstrGjJong == "22" || FstrGjJong == "23" || FstrGjJong == "24")            
            {
                FnWrtno1 = FnWRTNO;
                //특수2차 접수번호를 찾음
                FnWrtno2 = hicJepsuService.GetItembyPaNoGjYearGjBangiJepdateGjJong(FnPano, strGjYear, strGjBangi, strJepDate, FstrGjJong, "1");
            }
            else if (FstrGjJong == "27" || FstrGjJong == "29" || FstrGjJong == "28" || FstrGjJong == "33")
            {
                FnWrtno1 = FnWRTNO;
                //특수1차 접수번호를 찾음
                FnWrtno1 = hicJepsuService.GetItembyPaNoGjYearGjBangiJepdateGjJong(FnPano, strGjYear, strGjBangi, strJepDate, FstrGjJong, "2");
            }
            else
            {
                //1차,2차 접수번호를 찾음
                List<HIC_JEPSU> list2 = hicJepsuService.GetItembyPanoGjYear(FnPano, strGjYear);

                FnWrtno1 = 0;
                FnWrtno2 = 0;

                for (int i = 0; i < list2.Count; i++)
                {
                    //1차 접수번호를 찾음
                    if (FnWrtno1 == 0)
                    {
                        switch (list2[i].GJJONG)
                        {
                            case "16":
                            case "17":
                            case "18":
                            case "19":
                            case "27":
                            case "28":
                            case "29":
                            case "33":
                            case "44":
                            case "45":
                            case "46":
                                break;
                            default:
                                FnWrtno1 = list2[i].WRTNO;
                                break;
                        }
                    }
                    else if (FnWrtno2 == 0)
                    {
                        switch (list2[i].GJJONG)
                        {
                            case "16":
                            case "17":
                            case "18":
                            case "19":
                            case "27":
                            case "28":
                            case "29":
                            case "33":
                            case "44":
                            case "45":
                            case "46":
                                FnWrtno2 = list2[i].WRTNO;
                                break;
                            default:                                
                                break;
                        }
                    }
                }
            }

            if (FnWrtno1 == 0) return;

            //1차 인적사항을 Display
            HIC_JEPSU list3 = hicJepsuService.GetItembyWrtNo1(FnWrtno1);
            //HIC_JEPSU list3 = hicJepsuService.GetItembyPanoGjJong(FnPano, FsJong);

            if (list3.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWrtno1 + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //ssPatInfo.ActiveSheet.Cells[0, 0].Text = list3.PANO.To<string>();
            //ssPatInfo.ActiveSheet.Cells[0, 1].Text = list3.SNAME;
            //ssPatInfo.ActiveSheet.Cells[0, 2].Text = list3.AGE.To<string>() + "/" + FstrSex;
            //ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list3.LTDCODE.To<string>());
            //ssPatInfo.ActiveSheet.Cells[0, 4].Text = list3.JEPDATE.To<string>();
            //ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list3.GJJONG);

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = FnPano.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + FstrSex;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            FstrUCodes = list3.UCODES;
            lblUCodeName.Text = hm.UCode_Names_Display(FstrUCodes);

            //2020.11.26
            //특수검진2차 결과는 1차 결과에 update 되므로 1차 검사결과를 읽어 온다.
            //User 들의 니즈에 따라 각각 보여 줄 수 있다
            //단, 취급물질은 기존 로직을 따라야 한다.

            fn_Screen_Display_Panjeng();   //판정항목(취급물질)을 Display
            fn_Screen_Munjin1_Display();
            fn_Screen_Munjin2_Display();
            fn_Screen_Clear_SUB();

            //검사항목을 Display
            //Screen_Exam_Items_display
            List<HIC_RESULT_EXCODE> list4 = hicResultExCodeService.GetItembyWrtNoPaNo(FnWrtno1, FnWrtno2, FnWRTNO, FnPano, FstrGjChasu);

            nREAD = list4.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            strRemark = "";

            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list4[i].EXCODE;                 //검사코드
                strResult = list4[i].RESULT;                 //검사실 결과값
                strResCode = list4[i].RESCODE;               //결과값 코드
                strResultType = list4[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list4[i].GBCODEUSE;           //결과값코드 사용여부

                //SS2에 검사실 결과값을 DISPLAY
                SS2.ActiveSheet.Cells[i, 0].Text = " " + list4[i].HNAME;
                SS2.ActiveSheet.Cells[i, 1].Text = strResult;
                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        SS2.ActiveSheet.Cells[i, 1].Text = strResName;
                    }
                }
                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, strJepDate, FstrSex, list4[i].MIN_M, list4[i].MAX_M, list4[i].MIN_F, list4[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 8].Text = strResult;    //정상값 점검용;
                strExPan = list4[i].PANJENG;
                SS2.ActiveSheet.Cells[i, 2].Text = list4[i].PANJENG;
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222); //정상B
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170); //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220); //정상A 또는 기타
                        break;
                }
                if (FstrGjChasu == "2")
                {
                    if (FnWrtno1 == list4[i].WRTNO)
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "1";  //1차
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "2";  //2차
                    }
                }
                //간염검사
                if (strExCode == "A131")    //간염항체
                {
                    nGan1 = strResult.To<long>();  
                }
                else if (strExCode == "A132") //간염항원
                {
                    nGan2 = strResult.To<long>();
                }
            }

            OLD_Result_Display(FnPano, strJepDate, FstrSex);    //종전결과 2개를 Display

            if (FnWrtno1 > 0)
            {
                fn_First_Panjeng_Display(FnWrtno1);
            }

            //일반+특수 함계 판정
            if (FnWrtno2 > 0)
            {
                fn_Screen_Display_Second(); //2차판정
            }

            //주민등록번호로 원무행정의 등록번호를 찾음
            HIC_PATIENT listPaNo = hicPatientService.GetJumin2PtNobyPaNo(FnPano);
            
            if (!listPaNo.IsNullOrEmpty())
            {
                FstrJumin = clsAES.DeAES(listPaNo.JUMIN2);
                FstrPano = listPaNo.PTNO;
            }

            //주민등록번호로 환자 등록번호 찾기
            if (FstrPano == "")
            {
                FstrPano = basPatientService.GetPaNobyJumin1(VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), clsAES.AES(VB.Right(FstrJumin, 7)));
            }

            pnlPanjengMain.Enabled = true;

            //간염 판정
            if (nGan1 > 0 && nGan2 > 0)
            {
                if (nGan1 == 1 && nGan2 == 1)
                {
                    cboResult.SelectedIndex = 3;    //접종대상자
                }
                else if (nGan1 == 1 && nGan2 == 2)
                {
                    cboResult.SelectedIndex = 2;    //면역자
                }
                else if (nGan1 == 2 && nGan2 == 1)
                {
                    cboResult.SelectedIndex = 1;    //보균자
                }
            }

            //#region 강제 Sort
            ////검사결과의 판정값이 R,B인것을 위에 표시 
            //FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[] {
            //   new FarPoint.Win.Spread.SortInfo(2,true)
            //,  new FarPoint.Win.Spread.SortInfo(7,true)};

            //SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);
            //#endregion
        }

        void fn_Screen_Display_Panjeng()
        {
            int nRead = 0;

            //판정항목(취급물질)을 Display
            List<HIC_SPC_PANJENG> list = hicSpcPanjengService.GetItembyWrtno1Wrtno2(FnWrtno1, FnWrtno2);

            nRead = list.Count;
            SSPan.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSPan.ActiveSheet.Cells[i, 0].Text = "";
                SSPan.ActiveSheet.Cells[i, 1].Text = hb.READ_MCode_Name(list[i].MCODE);
                switch (list[i].PANJENG)
                {
                    case "1":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "A";
                        break;
                    case "2":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "B";
                        break;
                    case "3":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "C1";
                        break;
                    case "4":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "C2";
                        break;
                    case "5":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "D1";
                        break;
                    case "6":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "D2";
                        break;
                    case "7":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "R";
                        break;
                    case "8":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "U";
                        break;
                    case "9":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "CN";
                        break;
                    case "A":
                        SSPan.ActiveSheet.Cells[i, 2].Text = "DN";
                        break;
                    default:
                        SSPan.ActiveSheet.Cells[i, 2].Text = "";
                        break;
                }
                SSPan.ActiveSheet.Cells[i, 3].Text = list[i].SOGENCODE;
                SSPan.ActiveSheet.Cells[i, 4].Text = list[i].JOCHICODE;
                SSPan.ActiveSheet.Cells[i, 5].Text = list[i].SAHUCODE;
                SSPan.ActiveSheet.Cells[i, 6].Text = list[i].WORKYN;
                SSPan.ActiveSheet.Cells[i, 7].Text = list[i].SOGENREMARK;
                SSPan.ActiveSheet.Cells[i, 8].Text = list[i].JOCHIREMARK;
                SSPan.ActiveSheet.Cells[i, 9].Text = list[i].MCODE;
                SSPan.ActiveSheet.Cells[i, 10].Text = list[i].RID;
            }
        }

        void fn_Screen_Munjin1_Display()
        {
            string strGunGbn = "";

            //문진내역을 READ
            HIC_RES_SPECIAL list = hicResSpecialService.GetItemByWrtno(FnWrtno1);
            
            if (list.IsNullOrEmpty())
            {
                FstrROWID = "";
                return;
            }

            FstrROWID = list.RID;
            //사번,입사일자,현작업부서,수첩소지여부
            txtSabun.Text = list.SABUN;
            txtIpDate.Text = list.IPSADATE;
            txtBuse.Text = list.BUSE;
            //현작업공정,현직전입일자,현직근무기간
            lblGongjeng.Text = hb.READ_HIC_CODE("06", list.GONGJENG);
            txtJenipDate.Text = list.JENIPDATE.To<string>();
            if (list.PGIGAN_YY > 0)
            {
                lblHGigan.Text = list.PGIGAN_YY + "년" + list.PGIGAN_MM + "개월";
            }
            else
            {
                lblHGigan.Text = "";
            }

            lblJikjong.Text = hb.READ_HIC_CODE("05", list.JIKJONG);
            txtPTime.Text = list.PTIME.To<string>();

            //과거직력 1,2,3(작업공정,취급화학물코드,물질명,근무년수,1일노출
            //작업공정명      
            SSJik.ActiveSheet.RowCount = 3;
            SSJik.ActiveSheet.Cells[0, 0].Text = list.OLDGONG1;
            SSJik.ActiveSheet.Cells[1, 0].Text = list.OLDGONG2;
            SSJik.ActiveSheet.Cells[2, 0].Text = list.OLDGONG3;

            //취급물질명칭
            SSJik.ActiveSheet.Cells[0, 1].Text = list.OLDMCODE1;
            SSJik.ActiveSheet.Cells[1, 1].Text = list.OLDMCODE2;
            SSJik.ActiveSheet.Cells[2, 1].Text = list.OLDMCODE3;

            //근무년수
            SSJik.ActiveSheet.Cells[0, 2].Text = list.OLDYEAR1.To<string>();
            SSJik.ActiveSheet.Cells[1, 2].Text = list.OLDGONG2.To<string>();
            SSJik.ActiveSheet.Cells[2, 2].Text = list.OLDGONG3.To<string>();

            //일일노출시간
            SSJik.ActiveSheet.Cells[0, 3].Text = list.OLDDAYTIME1.To<string>();
            SSJik.ActiveSheet.Cells[1, 3].Text = list.OLDDAYTIME2.To<string>();
            SSJik.ActiveSheet.Cells[2, 3].Text = list.OLDDAYTIME3.To<string>();

            //생활습관,외상및휴유증,과거병력,일반상태
            if (list.HABIT1 == "Y") chkSang1.Checked = true;
            if (list.HABIT2 == "Y") chkSang2.Checked = true;
            if (list.HABIT3 == "Y") chkSang3.Checked = true;
            if (list.HABIT4 == "Y") chkSang4.Checked = true;
            if (list.HABIT5 == "Y") chkSang5.Checked = true;
            if (list.GBHUYU == "Y") rdoHuyu1.Checked = true;
            txtYear1.Text = list.OLDMYEAR1;
            txtYear2.Text = list.OLDMYEAR2;
            txtYear3.Text = list.OLDMYEAR3;
            txtYear4.Text = list.OLDMYEAR4;
            txtYear5.Text = list.OLDMYEAR5;
            txtEtcMsym.Text = list.OLDETCMSYM;
            //문진(가족력,업무기인성,임상진찰(신경계,두경부,피부,흉복부)
            txtGajok.Text = list.MUN_GAJOK;
            txtGiinsung.Text = list.MUN_GIINSUNG; //업무기인성
            txtNeuro.Text = list.JIN_NEURO;
            txtHead.Text = list.JIN_HEAD;
            txtSkin.Text = list.JIN_SKIN;
            txtChest.Text = list.JIN_CHEST;
            //일반상태,현재증상및자타각증상
            if (list.GBSANGTAE == "1") rdoSang1.Checked = true;
            if (list.GBSANGTAE == "2") rdoSang2.Checked = true;
            if (list.GBSANGTAE == "3") rdoSang3.Checked = true;
            txtJengSang.Text = list.JENGSANG;
            lblJengSang.Text = hb.READ_HIC_CODE("11", txtJengSang.Text);
            //치과소견,치과의사
            txtDent.Text = list.DENTSOGEN;
            txtDentDoct.Text = list.DENTDOCT.To<string>();
            lblDentDrName.Text = hb.READ_License_DrName(long.Parse(txtDentDoct.Text));
        }

        void fn_Screen_Munjin2_Display()
        {
            int nRow = 0;
            string[] strCodes = new string[14];
            string strCODE = "";
            string strData = "";
            string strRes = "";
            const string strMGubun = "ABCDEFGHIJKLMN";

            SSMun.ActiveSheet.RowCount = 0;

            //문진내역을 읽음
            HIC_RES_SPECIAL list = hicResSpecialService.GetItemByWrtno(FnWrtno1);

            if (list.IsNullOrEmpty()) return;

            strCodes[0] = list.MUNJIN_A + VB.Space(100);
            strCodes[1] = list.MUNJIN_B + VB.Space(100);
            strCodes[2] = list.MUNJIN_C + VB.Space(100);
            strCodes[3] = list.MUNJIN_D + VB.Space(100);
            strCodes[4] = list.MUNJIN_E + VB.Space(100);
            strCodes[5] = list.MUNJIN_F + VB.Space(100);
            strCodes[6] = list.MUNJIN_G + VB.Space(100);
            strCodes[7] = list.MUNJIN_H + VB.Space(100);
            strCodes[8] = list.MUNJIN_I + VB.Space(100);
            strCodes[9] = list.MUNJIN_J + VB.Space(100);
            strCodes[10] = list.MUNJIN_K + VB.Space(100);
            strCodes[11] = list.MUNJIN_L + VB.Space(100);
            strCodes[12] = list.MUNJIN_M + VB.Space(100);
            strCodes[13] = list.MUNJIN_N + VB.Space(100);

            for (int i = 0; i < 14; i++)
            {
                if (strCodes[i].Trim() != "")
                {
                    if (nRow > SSMun.ActiveSheet.RowCount)
                    {
                        SSMun.ActiveSheet.RowCount = nRow + 1;
                        switch (i)
                        {
                            case 0:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶소음";
                                break;
                            case 1:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶분진";
                                break;
                            case 2:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶진동";
                                break;
                            case 3:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶유기용제";
                                break;
                            case 4:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶연";
                                break;
                            case 5:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶수은";
                                break;
                            case 6:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶비전리방사선";
                                break;
                            case 7:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶크롬";
                                break;
                            case 8:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶특정화학물질1류";
                                break;
                            case 9:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶▶카드뮴";
                                break;
                            case 10:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶산,알카리,가스";
                                break;
                            case 11:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶망간";
                                break;
                            case 12:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶이상기압";
                                break;
                            case 13:
                                SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = " ▶전리방사선";
                                break;
                            default:
                                break;
                        }

                        //항목별 증상이 있는것만 표시함
                        for (int j = 1; j <= 21; j++)
                        {
                            strData = VB.Mid(strCodes[i], j * 3 - 2, 3).Trim();
                            if (strData != "")
                            {
                                //문진항목명을 읽음
                                strRes = hicCodeService.GetNamebyCode(VB.Mid(strMGubun, i, 1));
                                strRes += " ◈";
                                if (VB.Left(strData, 1) == "M")
                                {
                                    strRes += "월" + long.Parse(VB.Right(strData, 2)) + "회";
                                }
                                else
                                {
                                    strRes += "주" + long.Parse(VB.Right(strData, 2)) + "회";
                                }

                                nRow += 1;
                                if (nRow > SSMun.ActiveSheet.RowCount)
                                {
                                    SSMun.ActiveSheet.RowCount = nRow;
                                    SSMun.ActiveSheet.Cells[nRow - 1, 0].Text = strRes;
                                }
                            }
                        }
                    }
                }
            }
            SSMun.ActiveSheet.RowCount = nRow;
        }

        void OLD_Result_Display(long ArgPano, string ArgJepDate, string ArgSex)
        {
            //검사항목을 Setting
            strExamCode.Clear();

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() != "")
                {
                    strExamCode.Add(SS2.ActiveSheet.Cells[i, 7].Text.Trim());
                }
            }

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyPaNoJepDateWrtNo(ArgPano, ArgJepDate, FnWrtno1, FnWrtno2);

            nOldCNT = list.Count;
            strAllWRTNO = "";

            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE.To<string>();
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, ArgSex, i);
                if (i >= 1) break;
            }

            return;
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int argCol)
        {
            string strCode = "";

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

                //if (strExCode == "A999")
                //{
                //    strExCode = "A999";
                //}

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                strCode = "";
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {   
                    if (SS2.ActiveSheet.Cells[j, 7].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        strCode = "Y";
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                //if (nRow > 0)
                if (strCode == "Y")
                {
                    SS2.ActiveSheet.Cells[nRow, argCol + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = VB.Left(strResult, 2) + "." + hb.READ_ResultName(strResCode, VB.Left(strResult, 2));
                            //strResName = hb.READ_ResultName(strResCode,strResult);
                            SS2.ActiveSheet.Cells[nRow, argCol + 5].Text = strResName;
                        }
                    }

                    //SS2.ActiveSheet.Cells[nRow, argCol + 10].Text = strResult;   //정상값 점검용
                    SS2.ActiveSheet.Cells[nRow, argCol + 9].Text = strResult;   //정상값 점검용
                    strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepsuDate, "");
                    //판정결과별 바탕색상을 다르게 표시함
                    switch (strExPan)
                    {
                        case "B":
                            SS2.ActiveSheet.Cells[nRow, argCol + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                            break;
                        case "R":
                            SS2.ActiveSheet.Cells[nRow, argCol + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                            break;
                        default:
                            SS2.ActiveSheet.Cells[nRow, argCol + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                            break;
                    }
                    Application.DoEvents();
                    
                }
            }
        }

        void fn_Screen_Display_Second()
        {
            long nLicense = 0;
            string[] strMunjin = new string[3];
            string[] strChiRyo = new string[3];
            string[] strBalYY  = new string[3];
            string strGaJokJil = "";

            //일반건진2차 판정결과 Display
            HIC_RES_BOHUM2 list = hicResBohum2Service.GetItemByWrtno(FnWrtno2);

            if (list.IsNullOrEmpty())
            {
                if (clsHcVariable.GnHicLicense > 0)
                {
                    txtDrNo.Text = "";
                    lblDrName2.Text = "";
                }
                return;
            }

            //-------------------- 판정결과 DISPLAY ------------------------------------
            cboResult.SelectedIndex = list.LIVER20.To<int>();      //간염검사
            cboSoGyun1.SelectedIndex = list.CHEST3.To<int>();      //폐결핵검사소견
            cboSoGyun2.SelectedIndex = list.CHEST_RES.To<int>();   //흉부검사소견
            cboSoGyun3.SelectedIndex = list.CYCLE_RES.To<int>();   //순환기계검사소견
            cboSoGyun4.SelectedIndex = list.GOJI_RES.To<int>();    //고지혈증소견
            cboSoGyun5.SelectedIndex = list.LIVER_RES.To<int>();   //간장질환소견
            cboSoGyun6.SelectedIndex = list.KIDNEY_RES.To<int>();  //신장질환소견
            cboSoGyun7.SelectedIndex = list.AMEMIA_RES.To<int>();  //빈혈증소견
            cboSoGyun8.SelectedIndex = list.DIABETES_RES.To<int>();//당뇨질환소견
            cboSoGyun9.SelectedIndex = list.ETC_RES.To<int>();     //기타질환소견

            if (list.PANJENGDATE != "") //판정일자
            {
                txtPanDate2.Text = list.PANJENGDATE;
            }

            nLicense = list.PANJENGDRNO;    //의사면허번호
            if (nLicense != 0)
            {
                txtDrNo.Text = string.Format("{0:#######}", nLicense);
                lblDrName2.Text = hb.READ_License_DrName(nLicense);
            }
            else
            {
                txtDrNo.Text = "";
                lblDrName2.Text = "";
            }

            if (list.PANJENG == "1")
            {
                cboPanjeng2.SelectedIndex = 1;
            }
            else if (list.PANJENG == "2")
            {
                cboPanjeng2.SelectedIndex = 2;
            }
            else if (list.PANJENG == "3")
            {
                cboPanjeng2.SelectedIndex = 3;
            }
            else if (list.PANJENG == "4")
            {
                cboPanjeng2.SelectedIndex = 4;
            }
            else if (list.PANJENG == "5")
            {
                cboPanjeng2.SelectedIndex = 5;
                txtYujil1.Enabled = true;
                txtYujil2.Enabled = true;
                lblUName1.Enabled = true;
                lblUName2.Enabled = true;
                txtYujil1.Text = list.PANJENG_D11;
                txtYujil2.Text = list.PANJENG_D12;
                lblUName1.Text = hb.READ_HIC_CODE("31", txtYujil1.Text.Trim());
                lblUName2.Text = hb.READ_HIC_CODE("31", txtYujil2.Text.Trim());
            }

            cboSaHu1.SelectedIndex = list.PANJENG_SO1.To<int>();
            cboSaHu2.SelectedIndex = list.PANJENG_SO2.To<int>();
            cboSaHu3.SelectedIndex = list.PANJENG_SO3.To<int>();
            txtSogen2.Text = list.SOGEN;
            txtEtc_Exam.Text = list.ETC_EXAM;
            txtJobYN2.Text = list.WORKYN;
            lblJobYN2.Text = "";
            if (txtJobYN2.Text != "")
            {
                lblJobYN2.Text = hb.READ_HIC_CODE("13", txtJobYN2.Text.Trim()); //업무적합성
            }
        }

        void fn_First_Panjeng_Display(long ArgWRTNO)
        {
            int nAscii = 0;
            long nLicense = 0;

            //1차 판정결과
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(ArgWRTNO);

            if (list.IsNullOrEmpty())
            {
                return;
            }

            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            txtPanDate1.Text = list.PANJENGDATE;     //판정일자
            nLicense = list.PANJENGDRNO;                    //의사면허번호
            txtPanDrNo1.Text = string.Format("{0:#######0}", nLicense);
            lblDrName1.Text = hb.READ_License_DrName(nLicense);
            if (!list.PANJENG.IsNullOrEmpty())
            {
                if (list.PANJENG == "5")
                {
                    //cboPanjeng1.SelectedIndex = 4;
                }
                else
                {
                    //cboPanjeng1.SelectedIndex = int.Parse(list.PANJENG);
                }
            }
            else
            {
                //cboPanjeng1.Text = "";
            }

            //판정
            if (list.PANJENGB1 == "1")
            {
                chkPanjengB1.Checked = true;
            }
            else
            {
                chkPanjengB2.Checked = false;
            }
            if (list.PANJENGB2 == "1")
            {
                chkPanjengB2.Checked = true;
            }
            else
            {
                chkPanjengB2.Checked = false;
            }
            if (list.PANJENGB3 == "1")
            {
                chkPanjengB3.Checked = true;
            }
            else
            {
                chkPanjengB3.Checked = false;
            }
            if (list.PANJENGB4 == "1")
            {
                chkPanjengB4.Checked = true;
            }
            else
            {
                chkPanjengB4.Checked = false;
            }
            if (list.PANJENGB5 == "1")
            {
                chkPanjengB8.Checked = true;
            }
            else
            {
                chkPanjengB8.Checked = false;
            }
            if (list.PANJENGB6 == "1")
            {
                chkPanjengB6.Checked = true;
            }
            else
            {
                chkPanjengB6.Checked = false;
            }
            if (list.PANJENGB7 == "1")
            {
                chkPanjengB7.Checked = true;
            }
            else
            {
                chkPanjengB7.Checked = false;
            }
            if (list.PANJENGB8 == "1")
            {
                chkPanjengB5.Checked = true;
            }
            else
            {
                chkPanjengB5.Checked = false;
            }

            if (list.PANJENGR1 == "1")
            {
                chkPanjengR1.Checked = true;
                cboSoGyun1.Enabled = true;  //폐결핵
                lblSogyun1.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR1.Checked = false;
            }

            if (list.PANJENGR1 == "2")
            {
                chkPanjengR2.Checked = true;
                cboSoGyun2.Enabled = true;  //흉부검사
                lblSogyun2.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR2.Checked = false;
            }

            if (list.PANJENGR3 == "3")
            {
                chkPanjengR3.Checked = true;
                cboSoGyun3.Enabled = true;  //순환기계
                lblSogyun3.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR3.Checked = false;
            }

            if (list.PANJENGR4 == "4")
            {
                chkPanjengR4.Checked = true;
                cboSoGyun4.Enabled = true;  //고지혈증
                lblSogyun4.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR4.Checked = false;
            }

            if (list.PANJENGR5 == "5")
            {
                chkPanjengR5.Checked = true;
                cboSoGyun5.Enabled = true;  //간장질환
                lblSogyun5.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR5.Checked = false;
            }

            if (list.PANJENGR6 == "6")
            {
                chkPanjengR8.Checked = true;
                cboSoGyun8.Enabled = true;  //당뇨질환
                lblSogyun8.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR8.Checked = false;
            }

            if (list.PANJENGR7 == "7")
            {
                chkPanjengR6.Checked = true;
                cboSoGyun6.Enabled = true;  //신장질환
                lblSogyun6.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR6.Checked = false;
            }

            if (list.PANJENGR8 == "8")
            {
                chkPanjengR7.Checked = true;
                cboSoGyun7.Enabled = true;  //빈혈증
                lblSogyun7.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR7.Checked = false;
            }

            //if (list.PANJENGR9 == "9")
            //{
            //    chkPanjengR9.Checked = true;
            //    cboSoGyun9.Enabled = true;  //폐결핵
            //    lblSogyun9.ForeColor = Color.Black;
            //}
            //else
            //{
            //    chkPanjengR9.Checked = false;
            //}

            //if (list.PANJENGR10 == "10")
            //{
            //    chkPanjengR10.Checked = true;
            //    cboSoGyun10.Enabled = true;  //폐결핵
            //    lblSogyun10.ForeColor = Color.Black;
            //}
            //else
            //{
            //    chkPanjengR10.Checked = false;
            //}

            //원본
            //For i = 1 To 10
            //    If AdoGetString(RsPan, "PANJENGR" & Format(i, "#0"), 0) = "1" Then
            //       CheckPanjengR(i - 1).Value = "1"
            //       CheckPanjengR(i - 1).Value = 1
            //       Select Case i
            //            Case 1:  ComboSoGyun(0).Enabled = True '폐결핵
            //                     LabelSogyun(0).ForeColor = &H800000
            //            Case 2:  ComboSoGyun(1).Enabled = True '흉부검사
            //                     LabelSogyun(1).ForeColor = &H800000
            //            Case 3:  ComboSoGyun(2).Enabled = True '순환기계
            //                     LabelSogyun(2).ForeColor = &H800000
            //            Case 4:  ComboSoGyun(3).Enabled = True '고지혈증
            //                     LabelSogyun(3).ForeColor = &H800000
            //            Case 5:  ComboSoGyun(4).Enabled = True '간장질환
            //                     LabelSogyun(4).ForeColor = &H800000
            //            Case 6:  ComboSoGyun(7).Enabled = True '당뇨질환
            //                     LabelSogyun(7).ForeColor = &H800000
            //            Case 7:  ComboSoGyun(5).Enabled = True '신장질환
            //                     LabelSogyun(5).ForeColor = &H800000
            //            Case 8:  ComboSoGyun(6).Enabled = True '빈혈증
            //                     LabelSogyun(6).ForeColor = &H800000
            //            Case 11: ComboSoGyun(8).Enabled = True '기타질환
            //                     LabelSogyun(8).ForeColor = &H800000
            //        End Select
            //    Else
            //       CheckPanjengR(i - 1).Value = "0"
            //    End If
            //Next i

            if (list.PANJENGR11 == "1")
            {
                chkPanjengR11.Checked = true;
                if (!list.PANJENGETC.IsNullOrEmpty())
                {
                    nAscii = (VB.Asc(list.PANJENGETC) - 64);
                    cboEtcJil.SelectedIndex = nAscii;
                }
            }
            else
            {
                chkPanjengR11.Checked = false;
                cboEtcJil.Enabled = false;
            }

            if (list.LIVER3.IsNullOrEmpty())
            {
                cboLiver.Text = "";
            }
            else
            {
                cboLiver.SelectedIndex = list.LIVER3.To<int>(); //간염검사
            }

            txtSogen1.Text = list.SOGEN;    //조치 및 소견

            //1차 접수시 취급물질을 읽음
            lblUCodeName.Text = hm.UCode_Names_Display(hicJepsuService.GetUcodesbyWrtNo(ArgWRTNO));

            tabControl1.SelectedTab = tab13;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSPan)
            {
                string strCODE = "";

                if (SSPan.ActiveSheet.RowCount == 0) return;
                
                FstrROWID = SSPan.ActiveSheet.Cells[e.Row, 10].Text.Trim();

                fn_Screen_Clear_SUB();

                //더블클릭시 기존에 선택한것을 지움
                for (int i = 0; i < SSPan.ActiveSheet.RowCount; i++)
                {
                    SSPan.ActiveSheet.Cells[i, 0].Text = "";
                }

                SSPan.ActiveSheet.Cells[e.Row, 0].Text = "True";
                FstrPROWID = SSPan.ActiveSheet.Cells[e.Row, 10].Text.Trim();

                //자료를 읽어 Display
                HIC_SPC_PANJENG list = hicSpcPanjengService.GetItembyRowId(FstrPROWID);

                if (!list.IsNullOrEmpty())
                {
                    strCODE = list.PANJENG;
                    cboPanjeng.SelectedIndex = -1;
                    if (strCODE != "")
                    {
                        for (int i = 0; i < cboPanjeng.Items.Count; i++)
                        {
                            cboPanjeng.SelectedIndex = i;
                            if (VB.Left(cboPanjeng.Text, 1) == strCODE)
                            {
                                cboPanjeng.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    FstrMCode = list.MCODE;
                    lblMName.Text = hb.READ_MCode_Name(FstrMCode);

                    txtSogen.Text = list.SOGENCODE;
                    txtJochi.Text = list.JOCHICODE;
                    txtSogenRemark.Text = list.SOGENREMARK;
                    txtJochiRemark.Text = list.JOCHIREMARK;
                    txtJobYN.Text = list.WORKYN;
                    txtSahu.Text = list.SAHUCODE;
                    txtYuhe.Text = list.UCODE;
                    txtReExam.Text = list.REEXAM;
                    //유해물질 코드를 Display
                    if (txtYuhe.Text == "")
                    {
                        txtYuhe.Text = hicMcodeService.Read_UCode(FstrMCode);
                    }
                    lblSogenName.Text = hb.READ_SpcSCode_Name(txtSogen.Text.Trim());     //질병소견
                    lblJochiName.Text = hb.READ_HIC_CODE("14", txtJochi.Text.Trim());    //조치코드
                    lblJobYN.Text = hb.READ_HIC_CODE("13", txtJobYN.Text.Trim());        //업무접합성
                    lblSahuName.Text = hm.Sahu_Names_Display(txtSahu.Text.Trim());       //사후관리
                    lblReExamName.Text = hb.READ_HIC_CODE("53", txtReExam.Text.Trim());  //재검항목
                    lblYuheName.Text = hb.READ_HIC_CODE("09", txtYuhe.Text.Trim());      //유해인자

                    if (list.PANJENGDATE.IsNullOrEmpty())
                    {
                        txtPanDate.Text = "";
                        txtPanDrNo.Text = "";
                    }
                    else
                    {
                        txtPanDate.Text = list.PANJENGDATE.To<string>();
                        if (list.PANJENGDRNO.To<string>() != "")
                        {
                            txtPanDrNo.Text = list.PANJENGDRNO.To<string>();
                            lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                        }
                        else
                        {
                            txtPanDrNo.Text = "";
                            lblDrName.Text = "";
                        }
                    }
                }
                pnlPanjeng.Enabled = true;
            }
        }
    }
}
