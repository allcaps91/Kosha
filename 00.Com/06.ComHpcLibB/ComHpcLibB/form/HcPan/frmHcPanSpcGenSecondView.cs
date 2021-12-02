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
/// File Name       : frmHcPanSpcGenSecondView.cs
/// Description     : 특수 및 일특2차 조회
/// Author          : 이상훈
/// Create Date     : 2019-12-18
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSecondView.frm(HcPan07)" />

namespace ComHpcLibB
{
    public partial class frmHcPanSpcGenSecondView : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResSpecialService hicResSpecialService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicPatientService hicPatientService = null;
        BasPatientService basPatientService = null;
        HicCodeService hicCodeService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicMcodeService hicMcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcCombo Combo = new clsHcCombo();

        long FnWRTNO;
        long FnWrtno1;  //1차
        long FnWrtno2;  //2차
        long FnPano;
        string FstrSex;
        string FstrGjJong;
        string FstrGjChasu;
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrJumin;
        string FstrPano;
        string FstrSaveGbn;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrGbSPC;

        string FstrROWID;
        string FstrYROWID;  //약속판정 ROWID
        string FstrPROWID;

        int nREAD = 0;
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

        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        string strOldJepDate = "";

        public frmHcPanSpcGenSecondView(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            comHpcLibBService = new ComHpcLibBService();
            hicResSpecialService = new HicResSpecialService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicPatientService = new HicPatientService();
            basPatientService = new BasPatientService();
            hicCodeService = new HicCodeService();
            hicResBohum2Service = new HicResBohum2Service();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicResultExCodeService = new HicResultExCodeService();
            hicMcodeService = new HicMcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SSPan.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SSPan_Sheet1.Columns.Get(9).Visible = false;  //검사코드
            SSPan_Sheet1.Columns.Get(10).Visible = false;  //ROWID

            SS2_Sheet1.Columns.Get(7).Visible = false;  //검사코드
            SS2_Sheet1.Columns.Get(8).Visible = false;  //결과1
            SS2_Sheet1.Columns.Get(9).Visible = false;  //결과2
            SS2_Sheet1.Columns.Get(10).Visible = false; //결과3

            txtSogen.Text = "";

            Combo.ComboPanjeng2_SET(cboPanjeng);//특수종합판정
            Combo.ComboPanjeng2_SET(cboPanjeng1);  //1차종합판정

            ComboLiver_SET(cboLiver);        //간염
            Combo.ComboEtcJil_SET(cboEtcJil);      //기타질환명
            fn_Screen_Clear();

            //---------( 일반2차판정 Combo SET )---------------
            //일반2차종합판정
            cboPanjeng2.Items.Clear();
            cboPanjeng2.Items.Add(" ");
            cboPanjeng2.Items.Add("1.정상A");
            cboPanjeng2.Items.Add("2.정상B");
            cboPanjeng2.Items.Add("3.건강주의");
            cboPanjeng2.Items.Add("4.유질환");
            cboPanjeng2.Items.Add("5.유질환(직업병D1)");
            cboPanjeng2.SelectedIndex = 0;

            //사후관리            
            cboSaHu1.Items.Clear();
            cboSaHu1.Items.Add(" ");
            cboSaHu1.Items.Add("1.근로금지 및 제한");
            cboSaHu1.Items.Add("2.작업전환");
            cboSaHu1.Items.Add("3.근로시간단축");
            cboSaHu1.Items.Add("4.근무중치료");
            cboSaHu1.Items.Add("5.추적검사");
            cboSaHu1.Items.Add("6.보호구착용");
            cboSaHu1.Items.Add("7.기타");            

            cboSaHu2.Items.Clear();
            cboSaHu2.Items.Add(" ");
            cboSaHu2.Items.Add("1.근로금지 및 제한");
            cboSaHu2.Items.Add("2.작업전환");
            cboSaHu2.Items.Add("3.근로시간단축");
            cboSaHu2.Items.Add("4.근무중치료");
            cboSaHu2.Items.Add("5.추적검사");
            cboSaHu2.Items.Add("6.보호구착용");
            cboSaHu2.Items.Add("7.기타");

            cboSaHu3.Items.Clear();
            cboSaHu3.Items.Add(" ");
            cboSaHu3.Items.Add("1.근로금지 및 제한");
            cboSaHu3.Items.Add("2.작업전환");
            cboSaHu3.Items.Add("3.근로시간단축");
            cboSaHu3.Items.Add("4.근무중치료");
            cboSaHu3.Items.Add("5.추적검사");
            cboSaHu3.Items.Add("6.보호구착용");
            cboSaHu3.Items.Add("7.기타");

            //판정값
            for (int i = 1; i <= 9; i++)
            {
                ComboBox cboSoGyun = (Controls.Find("cboSoGyun" + i.ToString(), true)[0] as ComboBox);
                cboSoGyun.Items.Clear();
                cboSoGyun.Items.Add(" ");
                cboSoGyun.Items.Add("1.정상A");
                cboSoGyun.Items.Add("2.정상B");
                cboSoGyun.Items.Add("3.건강주의");
                cboSoGyun.Items.Add("4.유질환");
            }

            //기타질환명
            Combo.ComboEtcJil_SET(cboEtcJil);
            ComboLiver_SET(cboResult);  //간염검사
            txtSogen2.Text = "";
            txtEtc_Exam.Text = "";

            //--------------( 일반2차판정 설정 끝 )--------------------
            if (!FnWRTNO.IsNullOrEmpty() && FnWRTNO != 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>();
                fn_Screen_Display();
                eSpdDClick(SSPan, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
                clsHcVariable.GnWRTNO = 0;
            }
        }

        void ComboLiver_SET(ComboBox argCbo)
        {
            argCbo.Items.Clear();
            argCbo.Items.Add(" ");
            argCbo.Items.Add("1.항체 있음");
            argCbo.Items.Add("2.항체 없음");
            argCbo.Items.Add("3.B형간염보유자의심");
            argCbo.Items.Add("4.판정보류");
            argCbo.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSPan)
            {
                string strCODE = "";

                if (SSPan.ActiveSheet.NonEmptyRowCount > 0)
                {
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
                                if (VB.Left(cboPanjeng.Items[i].ToString(), 1) == strCODE)
                                {
                                    cboPanjeng.SelectedIndex = i;
                                    return;
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
                            txtPanDate.Text = clsPublic.GstrSysDate;
                            txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                        }
                        else
                        {
                            txtPanDate.Text = list.PANJENGDATE;
                            if (!list.PANJENGDRNO.IsNullOrEmpty() && list.PANJENGDRNO != 0)
                            {
                                txtPanDrNo.Text = list.PANJENGDRNO.To<string>();
                            }
                            else
                            {
                                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                            }
                        }
                        lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    }
                }
                pnlPanjeng.Enabled = true;
            }
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(ssPatInfo);
            sp.Spread_All_Clear(SSJik);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SSMun);
            sp.Spread_All_Clear(SSPan);

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = " ";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = " ";

            //FnWRTNO = 0;            
            FstrSex = "";               FstrROWID = "";
            FstrUCodes = "";        FstrCOMMIT = "";            FnPano = 0;
            FnWrtno1 = 0;           FnWrtno2 = 0;
            FstrSaveGbn = "";       FstrPano = "";              FstrJumin = "";
            FstrGbOHMS = "";        FstrGbSPC = "";             FstrGjJong = "";


            txtWrtNo.Text = "";     txtIpDate.Text = "";        txtBuse.Text = "";
            txtJenipDate.Text = "";
            txtPTime.Text = "";     txtEtcMsym.Text = "";
            txtGajok.Text = "";     txtGiinsung.Text = "";
            txtNeuro.Text = "";     txtHead.Text = "";          txtSkin.Text = "";
            txtChest.Text = "";     txtJengSang.Text = "";      txtDent.Text = "";
            txtDentDoct.Text = "";  lblDentDrName.Text = "";
            txtPanDrNo1.Text = "";  lblDrName1.Text = "";
            txtJobYN.Text = "";     lblJobYN.Text = "";

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
            rdoHuyu1.Checked = false;
            rdoSang1.Checked = false;
            rdoSang1.Checked = false;
            rdoSang1.Checked = false;

            //1차판정 결과 Clear
            txtPanDate1.Text = "";      cboLiver.Text = "";
            cboEtcJil.Text = "";      cboLiver.Text = "";
            cboPanjeng1.Text = "";    txtSogen1.Text = "";

            chkPanjengB1.Checked = false;
            chkPanjengB2.Checked = false;
            chkPanjengB3.Checked = false;
            chkPanjengB4.Checked = false;
            chkPanjengB5.Checked = false;
            chkPanjengB6.Checked = false;
            chkPanjengB7.Checked = false;
            chkPanjengB8.Checked = false;

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
            cboSoGyun2.SelectedIndex = -1;
            cboSoGyun3.SelectedIndex = -1;
            cboSoGyun4.SelectedIndex = -1;
            cboSoGyun5.SelectedIndex = -1;
            cboSoGyun6.SelectedIndex = -1;
            cboSoGyun7.SelectedIndex = -1;
            cboSoGyun8.SelectedIndex = -1;
            cboSoGyun9.SelectedIndex = -1;

            lblSogyun1.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun2.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun3.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun4.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun5.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun6.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun7.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun8.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));
            lblSogyun9.ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H808080"));

            cboSoGyun1.Enabled = false;
            cboSoGyun2.Enabled = false;
            cboSoGyun3.Enabled = false;
            cboSoGyun4.Enabled = false;
            cboSoGyun5.Enabled = false;
            cboSoGyun6.Enabled = false;
            cboSoGyun7.Enabled = false;
            cboSoGyun8.Enabled = false;
            cboSoGyun9.Enabled = false;

            cboPanjeng2.SelectedIndex = -1;
            cboSaHu1.SelectedIndex = -1;
            cboSaHu2.SelectedIndex = -1;
            cboSaHu3.SelectedIndex = -1;

            txtYujil1.Text = "";
            txtYujil2.Text = "";
            txtYujil1.Enabled = false;
            txtYujil2.Enabled = false;
            lblUName1.Text = "";
            lblUName2.Text = "";
            lblUName1.Enabled = false;
            lblUName2.Enabled = false;
        }

        void fn_Screen_Clear_SUB()
        {
            cboPanjeng.SelectedIndex = -1;
            lblMName.Text = "";
            txtSogen.Text = "";         txtSogenRemark.Text = "";
            txtJochi.Text = "";         txtJochiRemark.Text = "";
            txtSahu.Text = "";          lblSahuName.Text = ""; 
            txtYuhe.Text = "";          lblYuheName.Text = "";
            txtPanDate.Text = "";
            txtReExam.Text = "";        lblReExamName.Text = "";
            txtPanDrNo.Text = "";       lblDrName.Text = "";
            txtJobYN.Text = "";         lblJobYN.Text = "";
            lblSogenName.Text = "";
            lblJochiName.Text = "";

            FstrYROWID = "";
            FstrPROWID = "";
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
            int nHeight = 0;
            int nWeight = 0;
            int nResult = 0;
            string strRemark = "";
            string strExPan = "";   //검사1건의 판정결과
            long nGan1 = 0;
            long nGan2 = 0;

            FnWRTNO = txtWrtNo.Text.To<long>();
            nGan1 = -1;
            nGan2 = -1;

            ssPatInfo.ActiveSheet.RowCount = 1;

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrSex = list.SEX;
            FnPano = list.PANO;
            strGjYear = list.GJYEAR;
            strGjChasu = list.GJCHASU;
            strGjBangi = list.GJBANGI;
            strJepDate = list.JEPDATE;
            FstrGjJong = list.GJJONG;
            FstrGjChasu = strGjChasu;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            FstrUCodes = list.UCODES;
            lblUCodeName.Text = hm.UCode_Names_Display(FstrUCodes);

            //안정공단 전송여부를 읽음
            FstrGbOHMS = hicResSpecialService.GetGbOhmsbyWrtNo(FnWRTNO);

            if (FstrGjChasu == "2")
            {
                //검사결과 1,2차를 모두 표시하기 위하여 특수1차,2차를 읽음
                List<HIC_JEPSU> list2 = hicJepsuService.GetWrtNobyPanoGjYearBangiJepDate(FnPano, strGjYear, strGjBangi, strJepDate, FstrGjJong);

                FnWrtno1 = 0;
                FnWrtno2 = 0;
                if (list2.Count >= 1) FnWrtno1 = list2[0].WRTNO;    //1차
                if (list2.Count >= 2) FnWrtno1 = list2[1].WRTNO;    //2차
            }

            fn_Screen_Display_Panjeng();
            fn_Screen_Munjin1_Display();
            fn_Screen_Munjin2_Display();
            fn_Screen_Clear_SUB();

            hm.ExamResult_RePanjeng(FnWRTNO, FstrSex, strJepDate, strGjYear);   //검사결과를 재판정
            //Screen_Exam_Items_display //검사항목을 Display
            List<HIC_RESULT_EXCODE_JEPSU> list3 = hicResultExcodeJepsuService.GetItembyWrtNo(FstrGjChasu, FnPano, FnWRTNO, FnWrtno1, FnWrtno2);

            nREAD = list3.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;         //검사코드
                strResult = list3[i].RESULT;         //검사실 결과값
                strResCode = list3[i].RESCODE;       //결과값 코드
                strResultType = list3[i].RESULTTYPE; //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE;   //결과값코드 사용여부

                //SS2에 검사실 결과값을 DISPLAY
                SS2.ActiveSheet.Cells[i, 0].Text = " " + list3[i].HNAME;
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
                strNomal = hm.EXAM_NomalValue_SET(strExCode, strJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 8].Text = strResult;   //정상값 점검용
                strExPan = list3[i].PANJENG;
                SS2.ActiveSheet.Cells[i, 2].Text = strExPan;
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                        break;
                }

                if (FstrGjChasu == "2")
                {
                    if (FnWrtno1 == list3[i].WRTNO)
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "1";  //1차
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "2";  //2차
                    }
                }

                //간염검사
                if (strExCode == "A131")        //감염항체
                {
                    nGan1 = long.Parse(strResult);
                }
                else if (strExCode == "A132")   //간염항원
                {
                    nGan2 = long.Parse(strResult);
                }
            }

            //종전결과 2개를 Display
            fn_OLD_Result_Display(FnPano, strJepDate, FstrSex);

            if (FstrGjChasu == "2")
            {
                fn_First_Panjeng_Display(FnWrtno1);
            }

            //일반+특수 함계 판정
            if (string.Compare(FstrGjJong, "16") >= 0 || string.Compare(FstrGjJong, "19") <= 0)
            {
                fn_Screen_Display_Second(); //2차판정
            }

            //주민등록번호로 원무행정의 등록번호를 찾음
            HIC_PATIENT list4 = hicPatientService.GetJumin2byPano(FnPano);

            FstrJumin = "";
            if (!list4.IsNullOrEmpty())
            {
                FstrJumin = clsAES.DeAES(list4.JUMIN2);
                FstrPano = clsAES.DeAES(list4.PTNO);
            }

            //주민등록번호로 환자 등록번호 찾기
            if (FstrPano == "")
            {
                FstrPano = basPatientService.GetPaNobyJumin1Jumin2Jumin3(VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), clsAES.AES(VB.Right(FstrJumin, 7)));
            }

            pnlPanjengMain.Enabled = true;

            //간염 판정
            if (nGan1 > 0 && nGan2 > 0)
            {
                if (nGan1 == 1 && nGan2 == 1)
                {
                    cboResult.SelectedIndex = 3; //접종대상자
                }
                else if (nGan1 == 1 && nGan2 == 2)
                {
                    cboResult.SelectedIndex = 2; //면역자
                }
                else if (nGan1 == 2 && nGan2 == 1)
                {
                    cboResult.SelectedIndex = 1; //보균자
                }
            }
        }

        /// <summary>
        /// 판정항목(취급물질)을 Display
        /// </summary>
        void fn_Screen_Display_Panjeng()
        {
            int nRead = 0;

            //판정항목(취급물질)을 Display
            List<HIC_SPC_PANJENG> list = hicSpcPanjengService.GetItembyWrtNo1WrtNo2(FnWRTNO, FnWrtno1, FnWrtno2);

            nRead = list.Count;
            SSPan.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSPan.ActiveSheet.Cells[i, 0].Text = "";
                SSPan.ActiveSheet.Cells[i, 1].Text = " " + hb.READ_MCode_Name(list[i].MCODE);
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

        void fn_OLD_Result_Display(long ArgPano, string ArgJepDate, string ArgSex)
        {
            // 검사항목을 Setting
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
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;

            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int ii = 0; ii < nREAD; ii++)
            {
                strExCode = list[ii].EXCODE;             //검사코드
                strResult = list[ii].RESULT;             //검사실 결과값
                strResCode = list[ii].RESCODE;           //결과값 코드
                strResultType = list[ii].RESULTTYPE;     //결과값 TYPE
                strGbCodeUse = list[ii].GBCODEUSE;       //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                for (int j = 0; j < SS2.ActiveSheet.NonEmptyRowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[j, 7].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow >= 0)
                {
                    SS2.ActiveSheet.Cells[nRow, index + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, index + 5].Text = strResName;                            
                        }
                        
                        SS2.ActiveSheet.Cells[nRow, index + 9].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        switch (strExPan)
                        {
                            case "B":
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                                break;
                            case "R":
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                                break;
                            default:
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                                break;
                        }
                    }
                }
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
            txtIpDate.Text = list.IPSADATE.ToString();
            txtBuse.Text = list.BUSE;
            //현작업공정,현직전입일자,현직근무기간
            lblGongjeng.Text = hb.READ_HIC_CODE("06", list.GONGJENG);
            txtJenipDate.Text = list.JENIPDATE.ToString();
            if (list.PGIGAN_YY > 0)
            {
                lblHGigan.Text = list.PGIGAN_YY + "년" + list.PGIGAN_MM + "개월";
            }
            else
            {
                lblHGigan.Text = "";
            }
            lblJikjong.Text = hb.READ_HIC_CODE("05", list.JIKJONG);
            txtPTime.Text = list.PTIME.ToString();

            //과거직력 1,2,3(작업공정,취급화학물코드,물질명,근무년수,1일노출
            //작업공정명
            SSJik.ActiveSheet.Cells[0, 0].Text = list.OLDGONG1;
            SSJik.ActiveSheet.Cells[1, 0].Text = list.OLDGONG2;
            SSJik.ActiveSheet.Cells[2, 0].Text = list.OLDGONG3;
            //취급물질명칭            
            SSJik.ActiveSheet.Cells[0, 1].Text = hb.READ_MCode_Name(list.OLDMCODE1);
            SSJik.ActiveSheet.Cells[1, 1].Text = hb.READ_MCode_Name(list.OLDMCODE2);
            SSJik.ActiveSheet.Cells[2, 1].Text = hb.READ_MCode_Name(list.OLDMCODE3);
            //근무년수
            SSJik.ActiveSheet.Cells[0, 2].Text = list.OLDYEAR1.ToString("##");
            SSJik.ActiveSheet.Cells[1, 2].Text = list.OLDYEAR2.ToString("##");
            SSJik.ActiveSheet.Cells[2, 2].Text = list.OLDYEAR3.ToString("##");
            //일일노출시간
            SSJik.ActiveSheet.Cells[0, 3].Text = list.OLDDAYTIME1.ToString("##");
            SSJik.ActiveSheet.Cells[1, 3].Text = list.OLDDAYTIME2.ToString("##");
            SSJik.ActiveSheet.Cells[2, 3].Text = list.OLDDAYTIME3.ToString("##");
            //생활습관,외상및휴유증,과거병력,일반상태
            if (list.HABIT1 == "Y") chkSang1.Checked = true;
            if (list.HABIT2 == "Y") chkSang2.Checked = true;
            if (list.HABIT3 == "Y") chkSang3.Checked = true;
            if (list.HABIT4 == "Y") chkSang4.Checked = true;
            if (list.HABIT5 == "Y") chkSang5.Checked = true;
            if (list.GBHUYU == "Y") rdoHuyu1.Checked = true;
            txtYear1.Text = list.OLDMYEAR1.ToString();
            txtYear2.Text = list.OLDMYEAR2.ToString();
            txtYear3.Text = list.OLDMYEAR3.ToString();
            txtYear4.Text = list.OLDMYEAR4.ToString();
            txtYear5.Text = list.OLDMYEAR5.ToString();
            txtEtcMsym.Text = list.OLDETCMSYM;
            //문진(가족력,업무기인성,임상진찰(신경계,두경부,피부,흉복부)
            txtGajok.Text = list.MUN_GAJOK;
            txtGiinsung.Text = list.MUN_GIINSUNG;          //업무기인성
            txtNeuro.Text = list.JIN_NEURO;
            txtHead.Text = list.JIN_HEAD;
            txtSkin.Text = list.JIN_SKIN;
            txtChest.Text = list.JIN_CHEST;
            //일반상태,현재증상및자타각증상
            if (list.GBSANGTAE == "1") rdoSang1.Checked = true;
            if (list.GBSANGTAE == "2") rdoSang2.Checked = true;
            if (list.GBSANGTAE == "3") rdoSang3.Checked = true;
            txtJengSang.Text = list.JENGSANG;
            lblJengSang.Text = hb.READ_HIC_CODE("11", txtJengSang.Text.Trim());
            //치과소견,치과의사
            txtDent.Text = list.DENTSOGEN;
            txtDentDoct.Text = list.DENTDOCT.ToString();
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

        void fn_First_Panjeng_Display(long ArgWrtNo)
        {
            int nAscii = 0;
            long nLicense = 0;

            //1차 판정결과
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(ArgWrtNo);

            if (list.IsNullOrEmpty())
            {
                return;
            }

            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            txtPanDate1.Text = list.PANJENGDATE;     //판정일자
            nLicense = list.PANJENGDRNO;                    //의사면허번호
            txtPanDrNo1.Text = string.Format("{0:#######0}", nLicense);
            lblDrName1.Text = hb.READ_License_DrName(nLicense);
            if (list.PANJENG != "")
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
                chkPanjengB5.Checked = true;
            }
            else
            {
                chkPanjengB5.Checked = false;
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
                chkPanjengB8.Checked = true;
            }
            else
            {
                chkPanjengB8.Checked = false;
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

            if (list.PANJENGR2 == "1")
            {
                chkPanjengR2.Checked = true;
                cboSoGyun2.Enabled = true;  //흉부검사
                lblSogyun2.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR2.Checked = false;
            }

            if (list.PANJENGR3 == "1")
            {
                chkPanjengR3.Checked = true;
                cboSoGyun3.Enabled = true;  //순환기계
                lblSogyun3.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR3.Checked = false;
            }

            if (list.PANJENGR4 == "1")
            {
                chkPanjengR4.Checked = true;
                cboSoGyun4.Enabled = true;  //고지혈증
                lblSogyun4.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR4.Checked = false;
            }

            if (list.PANJENGR5 == "1")
            {
                chkPanjengR5.Checked = true;
                cboSoGyun5.Enabled = true;  //간장질환
                lblSogyun5.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR5.Checked = false;
            }

            if (list.PANJENGR6 == "1")
            {
                chkPanjengR8.Checked = true;
                cboSoGyun8.Enabled = true;  //당뇨질환
                lblSogyun8.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR8.Checked = false;
            }

            if (list.PANJENGR7 == "1")
            {
                chkPanjengR6.Checked = true;
                cboSoGyun6.Enabled = true;  //신장질환
                lblSogyun6.ForeColor = Color.Black;
            }
            else
            {
                chkPanjengR6.Checked = false;
            }

            if (list.PANJENGR8 == "1")
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
                if (list.PANJENGETC != "")
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
            lblUCodeName.Text = hm.UCode_Names_Display(hicJepsuService.GetUcodesbyWrtNo(ArgWrtNo));

            tabControl1.SelectedTab = tab4;
        }

        void fn_Screen_Display_Second()
        {
            long nLicense = 0;
            string[] strMunjin = new string[3];
            string[] strChiRyo = new string[3];
            string[] strBalYY  = new string[3];
            string strGaJokJil = "";

            //일반건진2차 판정결과 Display
            HIC_RES_BOHUM2 list = hicResBohum2Service.GetItemByWrtno(FnWRTNO);
            if (list.IsNullOrEmpty())
            {
                if (clsHcVariable.GnHicLicense > 0)
                {
                    txtDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                    lblDrName2.Text = hb.READ_License_DrName(long.Parse(txtDrNo.Text));
                }
                return;
            }

            //-------------------- 판정결과 DISPLAY ------------------------------------

            cboResult.SelectedIndex = int.Parse(list.LIVER20);  //간염검사
            cboSoGyun1.SelectedIndex = int.Parse(list.CHEST3);           //폐결핵검사소견
            cboSoGyun1.SelectedIndex = int.Parse(list.CHEST_RES);        //흉부검사소견
            cboSoGyun1.SelectedIndex = int.Parse(list.CYCLE_RES);        //순환기계검사소견
            cboSoGyun1.SelectedIndex = int.Parse(list.GOJI_RES);         //고지혈증소견
            cboSoGyun1.SelectedIndex = int.Parse(list.LIVER_RES);        //간장질환소견
            cboSoGyun1.SelectedIndex = int.Parse(list.KIDNEY_RES);       //신장질환소견
            cboSoGyun1.SelectedIndex = int.Parse(list.AMEMIA_RES);       //빈혈증소견
            cboSoGyun1.SelectedIndex = int.Parse(list.DIABETES_RES);     //당뇨질환소견
            cboSoGyun1.SelectedIndex = int.Parse(list.ETC_RES);          //기타질환소견

            if (list.PANJENGDATE != "") //판정일자
            {
                txtPanDate2.Text = list.PANJENGDATE;
            }

            nLicense = list.PANJENGDRNO;               //의사면허번호
            if (nLicense != 0)
            {
                txtDrNo.Text = nLicense.ToString("#######");
            }
            else
            {
                txtDrNo.Text = clsHcVariable.GnHicLicense.ToString("#######");
            }
            lblDrName2.Text = hb.READ_License_DrName(nLicense);

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
                txtYujil1.Enabled = true; txtYujil1.Enabled = true;
                lblUName1.Enabled = true; lblUName2.Enabled = true;
                txtYujil1.Text = list.PANJENG_D11;
                txtYujil2.Text = list.PANJENG_D12;
                lblUName1.Text = hb.READ_HIC_CODE("31", txtYujil1.Text.Trim());
                lblUName2.Text = hb.READ_HIC_CODE("31", txtYujil2.Text.Trim());
            }

            cboSaHu1.SelectedIndex = int.Parse(list.PANJENG_SO1);
            cboSaHu2.SelectedIndex = int.Parse(list.PANJENG_SO2);
            cboSaHu3.SelectedIndex = int.Parse(list.PANJENG_SO3);
            txtSogen2.Text = list.SOGEN;
            txtEtc_Exam.Text = list.ETC_EXAM;
            txtJobYN2.Text = list.WORKYN;
            lblJobYN2.Text = "";
            if (txtJobYN2.Text.Trim() != "")
            {
                lblJobYN2.Text = hb.READ_HIC_CODE("13", txtJobYN2.Text.Trim()); //업무접합성
            }
        }
    }
}
