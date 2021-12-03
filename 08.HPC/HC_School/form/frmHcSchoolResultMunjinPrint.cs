using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : Hc_School
/// File Name       : frmHcSchoolResultMunjinPrint.cs
/// Description     : 학생건강검사결과 및 문진 인쇄
/// Author          : 이상훈
/// Create Date     : 2020-01-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool3.frm(HcSchool03)" />
/// 
namespace HC_School
{
    public partial class frmHcSchoolResultMunjinPrint : Form
    {
        HicSchoolNewService hicSchoolNewService = null;
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long FnDrNo;

        public frmHcSchoolResultMunjinPrint()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void SetEvent()
        {
            hicSchoolNewService = new HicSchoolNewService();
            hicJepsuPatientSchoolService = new HicJepsuPatientSchoolService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            txtSName.Text = "";

            SS1_Sheet1.Columns.Get(10).Visible = false;
            SS1_Sheet1.Columns.Get(11).Visible = false;

            fn_Sheet_Clear();

            cboClass.Items.Clear();
            cboClass.Items.Add("*전체");
            cboClass.Items.Add("1");
            cboClass.Items.Add("2");
            cboClass.Items.Add("3");
            cboClass.Items.Add("4");
            cboClass.Items.Add("6");
            cboClass.SelectedIndex = 0;

            cboBan.Items.Clear();
            cboBan.Items.Add("*전체");
            cboBan.Items.Add("1");
            cboBan.Items.Add("2");
            cboBan.Items.Add("3");
            cboBan.Items.Add("4");
            cboBan.Items.Add("5");
            cboBan.Items.Add("6");
            cboBan.Items.Add("7");
            cboBan.Items.Add("8");
            cboBan.Items.Add("9");
            cboBan.Items.Add("10");
            cboBan.Items.Add("11");
            cboBan.Items.Add("12");
            cboBan.Items.Add("13");
            cboBan.Items.Add("14");
            cboBan.Items.Add("15");
            cboBan.Items.Add("");
            cboBan.SelectedIndex = 0;

            btnPrint.Enabled = false;

        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
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
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnPrint)
            {
                long nWRTNO = 0;
                string strChk = "";
                string strGBn  = "";
                string strTitle = "";

                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    nWRTNO = long.Parse(SS1.ActiveSheet.Cells[i, 8].Text.Trim());
                    strGBn = SS1.ActiveSheet.Cells[i, 10].Text.Trim();
                    strTitle = SS1.ActiveSheet.Cells[i, 11].Text.Trim();

                    if (rdoPrint1.Checked == true)
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_PMun(nWRTNO, strGBn, strTitle);
                        }
                        if (chkPrtGbn2.Checked == true)
                        {
                            fn_Result_Print_PPan(nWRTNO, strGBn);
                        }

                        fn_PrintPanOK(nWRTNO); //인쇄완료세팅
                    }
                    else if (rdoPrint2.Checked == true && strChk == "True")
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_PMun(nWRTNO, strGBn, strTitle);
                        }
                        if (chkPrtGbn2.Checked == true)
                        {
                            fn_Result_Print_PPan(nWRTNO, strGBn);
                        }
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                        fn_PrintPanOK(nWRTNO); //인쇄완료세팅
                    }
                    else if (rdoPrint3.Checked == true && strChk != "True")
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_PMun(nWRTNO, strGBn, strTitle);
                        }
                        if (chkPrtGbn2.Checked == true)
                        {
                            fn_Result_Print_PPan(nWRTNO, strGBn);
                        }
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                        fn_PrintPanOK(nWRTNO); //인쇄완료세팅
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                int nRow = 0;
                string strSex = "";
                string strJumin = "";

                string strFrDate = "";
                string strToDate = "";
                string strChkRePrint = "";
                string strSName = "";
                long nLtdCode = 0;
                string strClass = "";
                string strBan = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (chkRePrint.Checked == true)
                {
                    strChkRePrint = "1";
                }
                else
                {
                    strChkRePrint = "";
                }
                strSName = txtSName.Text;
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));                
                strClass = cboClass.Text;
                strBan = cboBan.Text;

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDate(strFrDate, strToDate, strChkRePrint, strSName, nLtdCode, strClass, strBan);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2.Trim());
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                    SS1.ActiveSheet.Cells[i, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].CLASS.Trim();
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].BAN.Trim();
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].BUN.Trim();
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim() + "/" + list[i].AGE.ToString();
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].WRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 9].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].GBN.Trim();
                    if (list[i].SEX.Trim() == "M")
                    {
                        strSex = "남";
                    }
                    else
                    {
                        strSex = "여";
                    }
                    SS1.ActiveSheet.Cells[i, 11].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString()) + "^^" + list[i].CLASS.Trim() + "학년 " + list[i].BAN.Trim() + "반 " + list[i].BUN.Trim() + "번";
                }
            }
            btnPrint.Enabled = true;
        }

        void fn_Result_Print_PMun(long argWrtNo, string argGbn, string argTitle)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            //문진표에 표시

            SSMun1.ActiveSheet.Cells[1, 13].Text = VB.Pstr(argTitle, "^^", 1);
            SSMun1.ActiveSheet.Cells[2, 13].Text = VB.Pstr(argTitle, "^^", 2);
            SSMun1.ActiveSheet.Cells[3, 13].Text = VB.Pstr(argTitle, "^^", 3);
            SSMun1.ActiveSheet.Cells[4, 13].Text = VB.Pstr(argTitle, "^^", 4);
            SSMun1.ActiveSheet.Cells[4, 15].Text = VB.Pstr(argTitle, "^^", 5);

            SSMun2.ActiveSheet.Cells[1, 13].Text = VB.Pstr(argTitle, "^^", 1);
            SSMun2.ActiveSheet.Cells[2, 13].Text = VB.Pstr(argTitle, "^^", 2);
            SSMun2.ActiveSheet.Cells[3, 13].Text = VB.Pstr(argTitle, "^^", 3);
            SSMun2.ActiveSheet.Cells[4, 13].Text = VB.Pstr(argTitle, "^^", 4);
            SSMun2.ActiveSheet.Cells[4, 15].Text = VB.Pstr(argTitle, "^^", 5);

            fn_Munjin_Display(argWrtNo, argGbn);

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (argGbn == "1")
            {
                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SSMun1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SSMun2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void fn_Result_Print_PPan(long argWrtNo, string argGbn)
        {
            string strTemp = "";
            string strFrDate = "";
            string strToDate = "";

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            HIC_JEPSU_PATIENT_SCHOOL list = hicJepsuPatientSchoolService.GetItembyJepDateWrtNo(strFrDate, strToDate, argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[3, 2].Text = VB.Space(3) + hb.READ_Ltd_Name(list.JEPLTDCODE.Trim());
                SS2.ActiveSheet.Cells[3, 6].Text = VB.Space(3) + list.CLASS.Trim() + "학년 " + list.BAN.Trim() + "반 " + list.BUN.Trim() + "번";
                SS2.ActiveSheet.Cells[4, 2].Text = VB.Space(3) + list.SNAME.Trim();
                if (list.SEX.Trim() == "M")
                {
                    SS2.ActiveSheet.Cells[4, 6].Text = "남";
                }
                else
                {
                    SS2.ActiveSheet.Cells[4, 6].Text = "여";
                }
                SS2.ActiveSheet.Cells[4, 8].Text = VB.Space(3) + VB.Left(clsAES.DeAES(list.JUMIN2.Trim()), 2) + "년 " + VB.Mid(clsAES.DeAES(list.JUMIN2.Trim()), 3, 2) + "월 " + VB.Mid(clsAES.DeAES(list.JUMIN2.Trim()), 5, 2) + "일";
                SS2.ActiveSheet.Cells[7, 4].Text = list.PPANA1.Trim() + " Cm";              //키
                SS2.ActiveSheet.Cells[7, 9].Text = hf.READ_URO("2", list.PPANE1.Trim());    //요단백
                SS2.ActiveSheet.Cells[8, 4].Text = list.PPANA2.Trim() + " Kg";              //몸무게
                SS2.ActiveSheet.Cells[8, 9].Text = hf.READ_URO("2", list.PPANE2.Trim());    //요잠혈
                SS2.ActiveSheet.Cells[9, 4].Text = hf.READ_Biman("2", list.PPANA3.Trim());  //체질량지수
                if (list.PPANF1.Trim() != "")
                {
                    SS2.ActiveSheet.Cells[9, 9].Text = list.PPANF1.Trim() + " mg/dL";       //혈당(식전)
                }
                SS2.ActiveSheet.Cells[10, 4].Text = hf.READ_Biman("4", list.PPANA4.Trim()); //상대체중
                if (list.PPANF2.Trim() != "")
                {
                    SS2.ActiveSheet.Cells[9, 9].Text = list.PPANF2.Trim() + " mg/dL";       //총콜레스테롤
                }
                SS2.ActiveSheet.Cells[11, 4].Text = hf.READ_Mush("2", list.PPANB1.Trim()); //근골격
                if (list.PPANF3.Trim() != "")
                {
                    SS2.ActiveSheet.Cells[11, 9].Text = list.PPANF3.Trim() + " mg/dL";       //AST
                }
                if (VB.Pstr(list.PPANC1.Trim(), "^^", 1) != "" && VB.Pstr(list.PPANC1.Trim(), "^^", 2) != "")
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "좌:" + VB.Pstr(list.PPANC1.Trim(), "^^", 1) + " 우:" + VB.Pstr(list.PPANC1.Trim(), "^^", 2);   //나안시력
                }
                else if (VB.Pstr(list.PPANC1.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "좌:" + VB.Pstr(list.PPANC1.Trim(), "^^", 1); //나안시력(좌)
                }
                else if (VB.Pstr(list.PPANC1.Trim(), "^^", 2) != "")
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "우:" + VB.Pstr(list.PPANC1.Trim(), "^^", 2); //나안시력(우)
                }
                if (list.PPANF4.Trim() != "")
                {
                    SS2.ActiveSheet.Cells[12, 9].Text = list.PPANF4.Trim() + " mg/dL";      //ALT
                }
                if (VB.Pstr(list.PPANC2.Trim(), "^^", 1) != "" && VB.Pstr(list.PPANC2.Trim(), "^^", 2) != "")
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "좌:" + VB.Pstr(list.PPANC2.Trim(), "^^", 1) + " 우:" + VB.Pstr(list.PPANC2.Trim(), "^^", 2);        //교정시력
                }
                else if (VB.Pstr(list.PPANC2.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "좌:" + VB.Pstr(list.PPANC2.Trim(), "^^", 1);   //교정시력(좌)
                }
                else if (VB.Pstr(list.PPANC2.Trim(), "^^", 2) != "")
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "우:" + VB.Pstr(list.PPANC2.Trim(), "^^", 2);   //교정시력(우)
                }
                SS2.ActiveSheet.Cells[13, 9].Text = list.PPANF5.Trim();                                 //혈색소

                SS2.ActiveSheet.Cells[14, 9].Text = hf.READ_YN2("2", list.PPANC3.Trim());               //색각
                if (VB.Pstr(list.PPANF6.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list.PPANF6.Trim(), "^^", 1) + "형, " + VB.Right(VB.Pstr(list.PPANF6.Trim(), "^^", 2), 3);   //혈액형
                }
                if (VB.Pstr(list.PPANC4.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[15, 4].Text = "좌:" + hf.READ_Eye("2", VB.Pstr(list.PPANC4.Trim(), "^^", 1)) + " 우:" + hf.READ_Eye("2", VB.Pstr(list.PPANC4.Trim(), "^^", 2));   //안질환
                }
                if (VB.Pstr(list.PPANG1.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[15, 9].Text = hf.READ_Xray("2", VB.Pstr(list.PPANG1.Trim(), "^^", 1));    //흉부방사선
                    if (VB.Pstr(list.PPANG1.Trim(), "^^", 2) != "")
                    {
                        SS2.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list.PPANG1.Trim(), "^^", 2);
                    }
                }
                if (VB.Pstr(list.PPANC5.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[16, 4].Text = "좌:" + hf.READ_YN2("2", VB.Pstr(list.PPANC5.Trim(), "^^", 1)) + " 우:" + hf.READ_YN2("2", VB.Pstr(list.PPANC5.Trim(), "^^", 2));        //청력
                }
                SS2.ActiveSheet.Cells[16, 9].Text = hf.READ_PM("2", list.PPANH1.Trim());      //간염
                if (VB.Pstr(list.PPANC6.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[17, 4].Text = hf.READ_Hear("2", VB.Pstr(list.PPANC6.Trim(), "^^", 1));     //귓병
                    if (VB.Pstr(list.PPANC6.Trim(), "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[17, 4].Text = VB.Pstr(list.PPANC6.Trim(), "^^", 2);
                    }
                }
                SS2.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list.PPANJ1.Trim(), "^^", 1) + " mmHg";    //혈압고
                if (VB.Pstr(list.PPANC7.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[18, 4].Text = hf.READ_Nose("2", VB.Pstr(list.PPANC7.Trim(), "^^", 1));     //콧병
                    if (VB.Pstr(list.PPANC7.Trim(), "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[18, 4].Text = VB.Pstr(list.PPANC7.Trim(), "^^", 2);
                    }
                }
                SS2.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list.PPANJ1.Trim(), "^^", 2) + " mmHg";    //혈압저
                if (VB.Pstr(list.PPANC8.Trim(), " ^^ ", 1) != "")
                {
                    SS2.ActiveSheet.Cells[19, 4].Text = hf.READ_Neck("2", VB.Pstr(list.PPANC8.Trim(), "^^", 1));     //목병
                    if (VB.Pstr(list.PPANC8.Trim(), " ^^ ", 1) == "5")
                    {
                        SS2.ActiveSheet.Cells[19, 4].Text = VB.Pstr(list.PPANC8.Trim(), " ^^ ", 2);
                    }
                }
                if (VB.Pstr(list.PPANC9.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[20, 4].Text = hf.READ_Skin("2", VB.Pstr(list.PPANC9.Trim(), "^^", 1));    //피부병
                    if (VB.Pstr(list.PPANC9.Trim(), "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[20, 4].Text = VB.Pstr(list.PPANC9.Trim(), "^^", 2);
                    }
                }
                if (VB.Pstr(list.PPAND1.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[21, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND1.Trim(), "^^", 1));      //호흡기
                }
                if (VB.Pstr(list.PPAND2.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[22, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND2.Trim(), "^^", 1));      //순환기
                }
                if (VB.Pstr(list.PPAND3.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[23, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND3.Trim(), "^^", 1));      //비뇨기
                }
                if (VB.Pstr(list.PPANK1.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[23, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK1.Trim(), "^^", 1));        //과거병력
                    if (VB.Pstr(list.PPANK1.Trim(), "^^", 2) != "")
                    {
                        SS2.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list.PPANK1.Trim(), "^^", 2);
                    }
                }
                if (VB.Pstr(list.PPAND4.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[24, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND4.Trim(), "^^", 1));      //소화기
                }
                if (VB.Pstr(list.PPANK2.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[24, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK2.Trim(), "^^", 1));        //생활습관
                    if (VB.Pstr(list.PPANK2.Trim(), "^^", 2) != "")
                    {
                        SS2.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list.PPANK2.Trim(), "^^", 2);
                    }
                }
                if (VB.Pstr(list.PPAND5.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[25, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND5.Trim(), "^^", 1));      //신경계
                }
                if (VB.Pstr(list.PPANK2.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[25, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK3.Trim(), "^^", 1));        //외상후유증
                    if (VB.Pstr(list.PPANK3.Trim(), "^^", 2) != "")
                    {
                        SS2.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list.PPANK3.Trim(), "^^", 2);
                    }
                }
                if (VB.Pstr(list.PPAND6.Trim(), "^^", 2) != "")
                {
                    SS2.ActiveSheet.Cells[26, 4].Text = VB.Pstr(list.PPAND6.Trim(), "^^", 2);      //기타
                }
                if (VB.Pstr(list.PPANK2.Trim(), "^^", 1) != "")
                {
                    SS2.ActiveSheet.Cells[26, 9].Text = hf.READ_Old2("2", VB.Pstr(list.PPANK4.Trim(), "^^", 1));        //일반상태
                    if (VB.Pstr(list.PPANK4.Trim(), "^^", 2) != "")
                    {
                        SS2.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list.PPANK4.Trim(), "^^", 2);
                    }
                }
                if (list.GBPAN.Trim() != "")
                {
                    switch (list.GBPAN)
                    {
                        case "1":
                            SS2.ActiveSheet.Cells[27, 6].Text = VB.Space(3) + "정상"; //종합판정
                            break;
                        case "2":
                            SS2.ActiveSheet.Cells[27, 6].Text = VB.Space(3) + "정상(경계)";
                            break;
                        case "3":
                            SS2.ActiveSheet.Cells[27, 6].Text = VB.Space(3) + "정밀검사요함";
                            break;
                        default:
                            break;
                    }
                    SS2.ActiveSheet.Cells[28, 4].Text = list.PPANREMARK1.Trim();    //종합소견
                    SS2.ActiveSheet.Cells[29, 4].Text = list.PPANREMARK2.Trim();    //조치사항
                    SS2.ActiveSheet.Cells[30, 3].Text = list.PPANDRNO.ToString();
                    SS2.ActiveSheet.Cells[30, 9].Text = list.JEPDATE.ToString();
                    SS2.ActiveSheet.Cells[31, 3].Text = VB.Space(3) + hb.READ_License_DrName(list.PPANDRNO);
                    FnDrNo = list.PPANDRNO;
                }
            }

            strTitle = "";
            strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
            strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void fn_PrintPanOK(long argWrtNo)
        {
            int result = 0;

            //인쇄완료 SET
            clsDB.setBeginTran(clsDB.DbCon);

            result = hicSchoolNewService.UpdateGbDntPrtbyWrtNo(clsType.User.IdNumber, argWrtNo, "PANDNT");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("인쇄 완료 Setting 중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Munjin_Display(long argWrtNo, string argGbn)
        {
            int nRead = 0;
            int j = 0;

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(argWrtNo);

            if (list.Count > 1)
            {
                MessageBox.Show("자료가 2건 이상입니다.확인하세요", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //문진항목
            //초등
            if (argGbn == "1")
            {
                SSMun1.ActiveSheet.Cells[9, 3].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[9, 4].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[9, 5].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[10, 3].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 4].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[10, 5].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[11, 3].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[11, 4].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 5].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[12, 3].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[12, 4].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[12, 5].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[13, 3].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[13, 4].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[13, 5].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[14, 3].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[14, 4].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[14, 5].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[15, 3].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[15, 4].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[15, 5].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 3);

                //소화기계
                SSMun1.ActiveSheet.Cells[18, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[18, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[19, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[19, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[20, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[20, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 3), "^^", 2);

                //호흡기계
                SSMun1.ActiveSheet.Cells[21, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[21, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[22, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[22, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[23, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[23, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[24, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[24, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 4), "^^", 2);

                SSMun1.ActiveSheet.Cells[25, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 5), "^^", 1);
                SSMun1.ActiveSheet.Cells[25, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 5), "^^", 2);

                //눈.귀
                SSMun1.ActiveSheet.Cells[26, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[26, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[27, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[27, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[28, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[28, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[29, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 4), "^^", 2);

                //피부
                SSMun1.ActiveSheet.Cells[30, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[30, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[31, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 2), "^^", 2);

                //순환기계
                SSMun1.ActiveSheet.Cells[32, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[32, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[33, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[33, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 2), "^^", 2);

                //근골격계
                SSMun1.ActiveSheet.Cells[34, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[34, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[35, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[35, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[36, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[36, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[37, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[37, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 4), "^^", 2);

                //그밖의
                SSMun1.ActiveSheet.Cells[38, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[38, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[39, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[39, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[40, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[40, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[41, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[41, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 4), "^^", 2);

                SSMun1.ActiveSheet.Cells[42, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 5), "^^", 1);
                SSMun1.ActiveSheet.Cells[42, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 5), "^^", 2);

                SSMun1.ActiveSheet.Cells[43, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 6), "^^", 1);
                SSMun1.ActiveSheet.Cells[43, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 6), "^^", 2);

                SSMun1.ActiveSheet.Cells[44, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 7), "^^", 1);
                SSMun1.ActiveSheet.Cells[44, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 7), "^^", 2);

                SSMun1.ActiveSheet.Cells[45, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 8), "^^", 1);
                SSMun1.ActiveSheet.Cells[45, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 8), "^^", 2);

                SSMun1.ActiveSheet.Cells[46, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 9), "^^", 1);
                SSMun1.ActiveSheet.Cells[46, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 9), "^^", 2);

                SSMun1.ActiveSheet.Cells[47, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 10), "^^", 1);
                SSMun1.ActiveSheet.Cells[47, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 10), "^^", 2);

                SSMun1.ActiveSheet.Cells[9, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 3);

                SSMun1.ActiveSheet.Cells[12, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[13, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 4);
                SSMun1.ActiveSheet.Cells[16, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 5);

                SSMun1.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list[0].PMUNC3.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list[0].PMUNC3.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[19, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[20, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[21, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[22, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 4);

                SSMun1.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 4);
                SSMun1.ActiveSheet.Cells[27, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 5);

                SSMun1.ActiveSheet.Cells[28, 9].Text = VB.Pstr(list[0].PMUNC6.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 9].Text = VB.Pstr(list[0].PMUNC6.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[30, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[32, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[33, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 4);
                SSMun1.ActiveSheet.Cells[34, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 5);
                SSMun1.ActiveSheet.Cells[35, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 6);

                //식생활
                SSMun1.ActiveSheet.Cells[9, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[12, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 4);
                SSMun1.ActiveSheet.Cells[13, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 5);

                SSMun1.ActiveSheet.Cells[14, 16].Text = VB.Pstr(list[0].PMUND2.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[15, 16].Text = VB.Pstr(list[0].PMUND2.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[16, 16].Text = VB.Pstr(list[0].PMUND3.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[17, 16].Text = VB.Pstr(list[0].PMUND3.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[18, 16].Text = VB.Pstr(list[0].PMUND4.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[19, 16].Text = VB.Pstr(list[0].PMUND4.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[20, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[21, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 2);
                SSMun1.ActiveSheet.Cells[22, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 3);
                SSMun1.ActiveSheet.Cells[23, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 4);
                SSMun1.ActiveSheet.Cells[24, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 5);
                SSMun1.ActiveSheet.Cells[25, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 6);

                SSMun1.ActiveSheet.Cells[26, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[27, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[28, 16].Text = VB.Pstr(list[0].PMUND7.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 16].Text = VB.Pstr(list[0].PMUND7.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[30, 16].Text = VB.Pstr(list[0].PMUND8.Trim(), "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 16].Text = VB.Pstr(list[0].PMUND8.Trim(), "^^", 2);

                SSMun1.ActiveSheet.Cells[32, 16].Text = list[0].PMUND9.Trim();

                SSMun1.ActiveSheet.Cells[42, 7].Text = list[0].PMUNREMARK1.Trim();
                SSMun1.ActiveSheet.Cells[42, 11].Text = list[0].PMUNREMARK2.Trim();
            }
            else
            {
                //중고등
                SSMun2.ActiveSheet.Cells[9, 3].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[9, 4].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[9, 5].Text = VB.Pstr(list[0].PMUNA1.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[10, 3].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 4].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[10, 5].Text = VB.Pstr(list[0].PMUNA2.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[11, 3].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[11, 4].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 5].Text = VB.Pstr(list[0].PMUNA3.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[12, 3].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[12, 4].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[12, 5].Text = VB.Pstr(list[0].PMUNA4.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[13, 3].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[13, 4].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[13, 5].Text = VB.Pstr(list[0].PMUNA5.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[14, 3].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[14, 4].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[14, 5].Text = VB.Pstr(list[0].PMUNA6.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[15, 3].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[15, 4].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[15, 5].Text = VB.Pstr(list[0].PMUNA7.Trim(), "^^", 3);

                //소화기계
                SSMun2.ActiveSheet.Cells[18, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[18, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[19, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[19, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[20, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1.Trim(), "@@", 3), "^^", 2);

                //호흡기계
                SSMun2.ActiveSheet.Cells[21, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[21, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[22, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[23, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[23, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[24, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[24, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2.Trim(), "@@", 4), "^^", 2);

                //눈.귀
                SSMun2.ActiveSheet.Cells[25, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[25, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[26, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[26, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[27, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[27, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[28, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[28, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3.Trim(), "@@", 4), "^^", 2);

                //피부
                SSMun2.ActiveSheet.Cells[29, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[29, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[30, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[30, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4.Trim(), "@@", 2), "^^", 2);

                //순환기계
                SSMun2.ActiveSheet.Cells[31, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[32, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[32, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5.Trim(), "@@", 2), "^^", 2);

                //근골격계
                SSMun2.ActiveSheet.Cells[33, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[34, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[34, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[35, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[35, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[36, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[36, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6.Trim(), "@@", 4), "^^", 2);

                //그밖의
                SSMun2.ActiveSheet.Cells[37, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[37, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[38, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[38, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[39, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[39, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[40, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[40, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 4), "^^", 2);

                SSMun2.ActiveSheet.Cells[41, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 5), "^^", 1);
                SSMun2.ActiveSheet.Cells[41, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 5), "^^", 2);

                SSMun2.ActiveSheet.Cells[42, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 6), "^^", 1);
                SSMun2.ActiveSheet.Cells[42, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 6), "^^", 2);

                SSMun2.ActiveSheet.Cells[43, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 7), "^^", 1);
                SSMun2.ActiveSheet.Cells[43, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 7), "^^", 2);

                SSMun2.ActiveSheet.Cells[44, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 8), "^^", 1);
                SSMun2.ActiveSheet.Cells[44, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 8), "^^", 2);

                SSMun2.ActiveSheet.Cells[45, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 9), "^^", 1);
                SSMun2.ActiveSheet.Cells[45, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 9), "^^", 2);

                SSMun2.ActiveSheet.Cells[46, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 10), "^^", 1);
                SSMun2.ActiveSheet.Cells[46, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 10), "^^", 2);

                SSMun2.ActiveSheet.Cells[47, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 11), "^^", 1);
                SSMun2.ActiveSheet.Cells[47, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7.Trim(), "@@", 11), "^^", 2);

                SSMun2.ActiveSheet.Cells[9, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 9].Text = VB.Pstr(list[0].PMUNC1.Trim(), "^^", 3);

                SSMun2.ActiveSheet.Cells[12, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[13, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 4);
                SSMun2.ActiveSheet.Cells[16, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 5);
                SSMun2.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 6);
                SSMun2.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list[0].PMUNC2.Trim(), "^^", 7);

                SSMun2.ActiveSheet.Cells[19, 9].Text = VB.Pstr(list[0].PMUNC3.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 9].Text = VB.Pstr(list[0].PMUNC3.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[21, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list[0].PMUNC4.Trim(), "^^", 4);

                SSMun2.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[27, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[28, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 4);
                SSMun2.ActiveSheet.Cells[29, 9].Text = VB.Pstr(list[0].PMUNC5.Trim(), "^^", 5);

                SSMun2.ActiveSheet.Cells[30, 9].Text = VB.Pstr(list[0].PMUNC6.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 9].Text = VB.Pstr(list[0].PMUNC6.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[32, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[34, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[35, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 4);
                SSMun2.ActiveSheet.Cells[36, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 5);
                SSMun2.ActiveSheet.Cells[37, 9].Text = VB.Pstr(list[0].PMUNC7.Trim(), "^^", 6);

                //식생활
                SSMun2.ActiveSheet.Cells[9, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[12, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 4);
                SSMun2.ActiveSheet.Cells[13, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 5);
                SSMun2.ActiveSheet.Cells[14, 16].Text = VB.Pstr(list[0].PMUND1.Trim(), "^^", 6);

                SSMun2.ActiveSheet.Cells[15, 16].Text = VB.Pstr(list[0].PMUND2.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[16, 16].Text = VB.Pstr(list[0].PMUND2.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[17, 16].Text = VB.Pstr(list[0].PMUND3.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[18, 16].Text = VB.Pstr(list[0].PMUND3.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[19, 16].Text = VB.Pstr(list[0].PMUND4.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 16].Text = VB.Pstr(list[0].PMUND4.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[21, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 16].Text = VB.Pstr(list[0].PMUND5.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[23, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[24, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 2);
                SSMun2.ActiveSheet.Cells[25, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 3);
                SSMun2.ActiveSheet.Cells[26, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 4);
                SSMun2.ActiveSheet.Cells[27, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 5);
                SSMun2.ActiveSheet.Cells[28, 16].Text = VB.Pstr(list[0].PMUND6.Trim(), "^^", 6);

                SSMun2.ActiveSheet.Cells[29, 16].Text = VB.Pstr(list[0].PMUND7.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[30, 16].Text = VB.Pstr(list[0].PMUND7.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 16].Text = VB.Pstr(list[0].PMUND7.Trim(), "^^", 1);

                SSMun2.ActiveSheet.Cells[32, 16].Text = VB.Pstr(list[0].PMUND8.Trim(), "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 16].Text = VB.Pstr(list[0].PMUND8.Trim(), "^^", 2);

                SSMun2.ActiveSheet.Cells[34, 16].Text = list[0].PMUND9.Trim();
                SSMun2.ActiveSheet.Cells[42, 7].Text = list[0].PMUNREMARK1.Trim();
                SSMun2.ActiveSheet.Cells[42, 11].Text = list[0].PMUNREMARK2.Trim();
            }

            //색상
            if (argGbn == "1")
            {
                for (int i = 0; i <= 47; i++)
                {
                    for (int k = 0; k <= 16; k++)
                    {
                        if (SSMun1.ActiveSheet.Cells[i, k].Text == "2.유" || SSMun1.ActiveSheet.Cells[i, k].Text == "2.Y")
                        {
                            SSMun1.ActiveSheet.Cells[i, k].ForeColor = Color.FromArgb(0, 0, 0);
                            SSMun1.ActiveSheet.Cells[i, k].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        else if (SSMun1.ActiveSheet.Cells[i, k].Text == "1.무" || SSMun1.ActiveSheet.Cells[i, k].Text == "2.N")
                        {
                            SSMun1.ActiveSheet.Cells[i, k].ForeColor = Color.FromArgb(0, 0, 0);
                            SSMun1.ActiveSheet.Cells[i, k].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }                
            }
            else
            {
                for (int i = 0; i <= 47; i++)
                {
                    for (int k = 0; k <= 16; k++)
                    {
                        if (SSMun2.ActiveSheet.Cells[i, k].Text == "2.유" || SSMun2.ActiveSheet.Cells[i, k].Text == "2.Y")
                        {
                            SSMun2.ActiveSheet.Cells[i, k].ForeColor = Color.FromArgb(0, 0, 0);
                            SSMun2.ActiveSheet.Cells[i, k].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        else if (SSMun2.ActiveSheet.Cells[i, k].Text == "1.무" || SSMun2.ActiveSheet.Cells[i, k].Text == "2.N")
                        {
                            SSMun2.ActiveSheet.Cells[i, k].ForeColor = Color.FromArgb(0, 0, 0);
                            SSMun2.ActiveSheet.Cells[i, k].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void fn_Sheet_Clear()
        {
            //판정
            SS2.ActiveSheet.Cells[3, 2].Text = "";
            SS2.ActiveSheet.Cells[3, 6].Text = "";
            SS2.ActiveSheet.Cells[4, 2].Text = "";
            SS2.ActiveSheet.Cells[4, 6].Text = "";
            SS2.ActiveSheet.Cells[4, 8].Text = "";

            for (int i = 7; i <= 26; i++)
            {
                SS2.ActiveSheet.Cells[i, 4].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = "";
            }

            SS2.ActiveSheet.Cells[27, 6].Text = "";
            SS2.ActiveSheet.Cells[28, 4].Text = "";
            SS2.ActiveSheet.Cells[29, 4].Text = "";
            SS2.ActiveSheet.Cells[30, 5].Text = "";
            SS2.ActiveSheet.Cells[30, 9].Text = "";
            SS2.ActiveSheet.Cells[31, 5].Text = "";

            //문진 공통사항
            SSMun1.ActiveSheet.Cells[1, 13].Text = "";
            SSMun1.ActiveSheet.Cells[2, 13].Text = "";
            SSMun1.ActiveSheet.Cells[3, 13].Text = "";
            SSMun1.ActiveSheet.Cells[4, 13].Text = "";
            SSMun1.ActiveSheet.Cells[4, 15].Text = "";

            //가족의학병력
            for (int i = 9; i <= 15; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 3].Text = "1.무";
                SSMun1.ActiveSheet.Cells[i, 4].Text = "";
                SSMun1.ActiveSheet.Cells[i, 5].Text = "";

                SSMun2.ActiveSheet.Cells[i, 3].Text = "1.무";
                SSMun2.ActiveSheet.Cells[i, 4].Text = "";
                SSMun2.ActiveSheet.Cells[i, 5].Text = "";
            }

            //최근질병?
            for (int i = 18; i <= 47; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 4].Text = "1.무";
                SSMun1.ActiveSheet.Cells[i, 5].Text = "";

                SSMun2.ActiveSheet.Cells[i, 4].Text = "1.무";
                SSMun2.ActiveSheet.Cells[i, 5].Text = "";
            }

            //의사에게하고픈말,상담 ?
            SSMun1.ActiveSheet.Cells[42, 7].Text = "";
            SSMun1.ActiveSheet.Cells[42, 11].Text = "";

            SSMun2.ActiveSheet.Cells[42, 7].Text = "";
            SSMun2.ActiveSheet.Cells[42, 11].Text = "";

            //초등문진
            for (int i = 9; i <= 35; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 9].Text = "1.N";
                SSMun2.ActiveSheet.Cells[i, 9].Text = "1.N";
            }

            //건강생활
            for (int i = 9; i <= 32; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 16].Text = "1.N";
            }

            //중고등문진
            for (int i = 9; i <= 38; i++)
            {
                SSMun2.ActiveSheet.Cells[i, 9].Text = "1.N";
            }

            //건강생활
            for (int i = 9; i <= 34; i++)
            {
                SSMun2.ActiveSheet.Cells[i, 16].Text = "1.N";
            }

            //색상
            for (int i = 0; i <= 47; i++)
            {
                for (int j = 0; j <= 16; j++)
                {
                    if (SSMun1.ActiveSheet.Cells[i, j].Text == "2.유" || SSMun1.ActiveSheet.Cells[i, j].Text == "2.Y")
                    {
                        SSMun1.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun1.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (SSMun1.ActiveSheet.Cells[i, j].Text == "1.무" || SSMun1.ActiveSheet.Cells[i, j].Text == "1.N")
                    {
                        SSMun1.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun1.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 0, 0);
                    }
                }
            }

            for (int i = 0; i <= 47; i++)
            {
                for (int j = 0; j <= 16; j++)
                {
                    if (SSMun2.ActiveSheet.Cells[i, j].Text == "2.유" || SSMun2.ActiveSheet.Cells[i, j].Text == "2.Y")
                    {
                        SSMun2.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun2.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (SSMun2.ActiveSheet.Cells[i, j].Text == "1.무" || SSMun2.ActiveSheet.Cells[i, j].Text == "1.N")
                    {
                        SSMun2.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun2.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 0, 0);
                    }
                }
            }


        }
    }
}
