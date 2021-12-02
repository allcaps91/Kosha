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
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSchoolExamResult.cs
/// Description     : 학생건강검사결과 인쇄(통합)
/// Author          : 이상훈
/// Create Date     : 2020-02-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool10.frm(HcSchool10)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolExamResult : Form
    {
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;
        HicSchoolNewService hicSchoolNewService = null;
        HicSunapdtlService hicSunapdtlService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();        
        clsHcPrint hp = new clsHcPrint();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long FnDrNo;

        public frmHcSchoolExamResult()
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
            hicJepsuPatientSchoolService = new HicJepsuPatientSchoolService();
            hicSchoolNewService = new HicSchoolNewService();
            hicSunapdtlService = new HicSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
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

            SS1_Sheet1.Rows.Get(33).Visible = false;

            fn_Sheet_Clear();

            cboClass.Items.Clear();
            cboClass.Items.Add("*전체");
            cboClass.Items.Add("1");
            cboClass.Items.Add("2");
            cboClass.Items.Add("3");
            cboClass.Items.Add("4");
            cboClass.Items.Add("5");
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
                int nRow = 0;
                string strSex = "";
                string strJumin = "";
                string strOK = "";

                string strFrDate = "";
                string strToDate = "";
                string strChkRePrint = "";
                string strSName = "";
                long nLtdCode = 0;
                string strClass = "";
                string strBan = "";
                string strGbn = "";
                string strRePrint = "";

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
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strClass = cboClass.Text;
                strBan = cboBan.Text;

                if (rdoGbn2.Checked == true)
                {
                    strGbn = "2";
                }
                else if (rdoGbn3.Checked == true)
                {
                    strGbn = "3";
                }

                if (chkRePrint.Checked == true)
                {
                    strRePrint = "1";
                }

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDate(strFrDate, strToDate, strChkRePrint, strSName, nLtdCode, strClass, strBan);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "";
                    if (rdoGbn1.Checked == false)
                    {
                        HIC_SUNAPDTL list2 = hicSunapdtlService.GetCodebyWrtNo(list[i].WRTNO, strGbn);

                        if (!list2.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                    }
                    else
                    {
                        strOK = "OK";
                    }

                    if (rdoGbn1.Checked == true)
                    {
                        if (list[i].PPANDRNO == 0)
                        {
                            strOK = "";
                        }
                    }
                    else if (rdoGbn3.Checked == true)
                    {
                        if (list[i].DPANDRNO == 0)
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "OK")
                    {
                        strJumin = clsAES.DeAES(list[i].JUMIN2);
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].CLASS.To<string>();
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].BAN.To<string>();
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].BUN.To<string>();
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].SEX + "/" + list[i].AGE.ToString();
                        SS1.ActiveSheet.Cells[i, 7].Text = list[i].JEPDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 8].Text = list[i].WRTNO.ToString();
                        SS1.ActiveSheet.Cells[i, 9].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                        SS1.ActiveSheet.Cells[i, 10].Text = list[i].GBN;
                        if (list[i].SEX == "M")
                        {
                            strSex = "남";
                        }
                        else
                        {
                            strSex = "여";
                        }
                        SS1.ActiveSheet.Cells[i, 11].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString()) + "^^" + list[i].CLASS + "학년 " + list[i].BAN + "반 " + list[i].BUN + "번";

                        if (list[i].PPANDRNO == 0)
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = "X";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = "○";
                        }

                        if (list[i].DPANDRNO == 0)
                        {
                            SS1.ActiveSheet.Cells[i, 13].Text = "X";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 13].Text = "○";
                        }
                    }
                }
                btnPrint.Enabled = true;
            }
            else if (sender == btnPrint)
            {
                long nWRTNO = 0;
                string strChk = "";
                string strGBn = "";
                string strTitle = "";

                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nWRTNO = long.Parse(SS1.ActiveSheet.Cells[i, 8].Text);
                    strGBn = SS1.ActiveSheet.Cells[i, 10].Text;
                    strTitle = SS1.ActiveSheet.Cells[i, 11].Text;

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
                            fn_Result_Print_DPan(nWRTNO);
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
                            fn_Result_Print_DPan(nWRTNO);
                        }
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                        fn_PrintPanOK(nWRTNO);  //인쇄완료세팅
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
                            fn_Result_Print_DPan(nWRTNO);
                        }
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                        fn_PrintPanOK(nWRTNO);  //인쇄완료세팅
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
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
                SS2.ActiveSheet.Cells[3, 2].Text = VB.Space(3) + hb.READ_Ltd_Name(list.JEPLTDCODE);
                SS2.ActiveSheet.Cells[3, 6].Text = VB.Space(3) + list.CLASS + "학년 " + list.BAN + "반 " + list.BUN + "번";
                SS2.ActiveSheet.Cells[4, 2].Text = VB.Space(3) + list.SNAME;
                if (list.SEX == "M")
                {
                    SS2.ActiveSheet.Cells[4, 6].Text = "남";
                }
                else
                {
                    SS2.ActiveSheet.Cells[4, 6].Text = "여";
                }
                SS2.ActiveSheet.Cells[4, 8].Text = VB.Space(3) + VB.Left(clsAES.DeAES(list.JUMIN2), 2) + "년 " + VB.Mid(clsAES.DeAES(list.JUMIN2), 3, 2) + "월 " + VB.Mid(clsAES.DeAES(list.JUMIN2), 5, 2) + "일";
                SS2.ActiveSheet.Cells[7, 4].Text = list.PPANA1 + " Cm";              //키
                SS2.ActiveSheet.Cells[7, 9].Text = hf.READ_URO("2", list.PPANE1);    //요단백
                SS2.ActiveSheet.Cells[8, 4].Text = list.PPANA2 + " Kg";              //몸무게
                SS2.ActiveSheet.Cells[8, 9].Text = hf.READ_URO("2", list.PPANE2);    //요잠혈
                SS2.ActiveSheet.Cells[9, 4].Text = hf.READ_Biman("2", list.PPANA3);  //체질량지수
                if (!list.PPANF1.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[9, 9].Text = list.PPANF1 + " mg/dL";       //혈당(식전)
                }
                SS2.ActiveSheet.Cells[10, 4].Text = hf.READ_Biman("4", list.PPANA4); //상대체중
                if (!list.PPANF2.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[9, 9].Text = list.PPANF2 + " mg/dL";       //총콜레스테롤
                }
                SS2.ActiveSheet.Cells[11, 4].Text = hf.READ_Mush("2", list.PPANB1); //근골격
                if (!list.PPANF3.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[11, 9].Text = list.PPANF3 + " mg/dL";       //AST
                }
                if (!VB.Pstr(list.PPANC1, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list.PPANC1, "^^", 2).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "좌:" + VB.Pstr(list.PPANC1, "^^", 1) + " 우:" + VB.Pstr(list.PPANC1, "^^", 2);   //나안시력
                }
                else if (!VB.Pstr(list.PPANC1, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "좌:" + VB.Pstr(list.PPANC1, "^^", 1); //나안시력(좌)
                }
                else if (!VB.Pstr(list.PPANC1, "^^", 2).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[12, 4].Text = "우:" + VB.Pstr(list.PPANC1, "^^", 2); //나안시력(우)
                }
                if (!list.PPANF4.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[12, 9].Text = list.PPANF4 + " mg/dL";      //ALT
                }
                if (!VB.Pstr(list.PPANC2, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list.PPANC2, "^^", 2).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "좌:" + VB.Pstr(list.PPANC2, "^^", 1) + " 우:" + VB.Pstr(list.PPANC2, "^^", 2);        //교정시력
                }
                else if (!VB.Pstr(list.PPANC2, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "좌:" + VB.Pstr(list.PPANC2, "^^", 1);   //교정시력(좌)
                }
                else if (!VB.Pstr(list.PPANC2, "^^", 2).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[13, 4].Text = "우:" + VB.Pstr(list.PPANC2, "^^", 2);   //교정시력(우)
                }
                SS2.ActiveSheet.Cells[13, 9].Text = list.PPANF5;                                 //혈색소

                SS2.ActiveSheet.Cells[14, 9].Text = hf.READ_YN2("2", list.PPANC3);               //색각
                if (!VB.Pstr(list.PPANF6, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list.PPANF6, "^^", 1) + "형, " + VB.Right(VB.Pstr(list.PPANF6, "^^", 2), 3);   //혈액형
                }
                if (!VB.Pstr(list.PPANC4, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[15, 4].Text = "좌:" + hf.READ_Eye("2", VB.Pstr(list.PPANC4, "^^", 1)) + " 우:" + hf.READ_Eye("2", VB.Pstr(list.PPANC4, "^^", 2));   //안질환
                }
                if (!VB.Pstr(list.PPANG1, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[15, 9].Text = hf.READ_Xray("2", VB.Pstr(list.PPANG1, "^^", 1));    //흉부방사선
                    if (!VB.Pstr(list.PPANG1, "^^", 2).IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list.PPANG1, "^^", 2);
                    }
                }
                if (!VB.Pstr(list.PPANC5, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[16, 4].Text = "좌:" + hf.READ_YN2("2", VB.Pstr(list.PPANC5, "^^", 1)) + " 우:" + hf.READ_YN2("2", VB.Pstr(list.PPANC5, "^^", 2));        //청력
                }
                SS2.ActiveSheet.Cells[16, 9].Text = hf.READ_PM("2", list.PPANH1);      //간염
                if (!VB.Pstr(list.PPANC6, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[17, 4].Text = hf.READ_Hear("2", VB.Pstr(list.PPANC6, "^^", 1));     //귓병
                    if (VB.Pstr(list.PPANC6, "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[17, 4].Text = VB.Pstr(list.PPANC6, "^^", 2);
                    }
                }
                SS2.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list.PPANJ1, "^^", 1) + " mmHg";    //혈압고
                if (!VB.Pstr(list.PPANC7, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[18, 4].Text = hf.READ_Nose("2", VB.Pstr(list.PPANC7, "^^", 1));     //콧병
                    if (VB.Pstr(list.PPANC7, "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[18, 4].Text = VB.Pstr(list.PPANC7, "^^", 2);
                    }
                }
                SS2.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list.PPANJ1, "^^", 2) + " mmHg";    //혈압저
                if (!VB.Pstr(list.PPANC8, " ^^ ", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[19, 4].Text = hf.READ_Neck("2", VB.Pstr(list.PPANC8, "^^", 1));     //목병
                    if (VB.Pstr(list.PPANC8, " ^^ ", 1) == "5")
                    {
                        SS2.ActiveSheet.Cells[19, 4].Text = VB.Pstr(list.PPANC8, " ^^ ", 2);
                    }
                }
                if (!VB.Pstr(list.PPANC9, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[20, 4].Text = hf.READ_Skin("2", VB.Pstr(list.PPANC9, "^^", 1));    //피부병
                    if (VB.Pstr(list.PPANC9, "^^", 1) == "4")
                    {
                        SS2.ActiveSheet.Cells[20, 4].Text = VB.Pstr(list.PPANC9, "^^", 2);
                    }
                }
                if (!VB.Pstr(list.PPAND1, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[21, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND1, "^^", 1));      //호흡기
                }
                if (!VB.Pstr(list.PPAND2, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[22, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND2, "^^", 1));      //순환기
                }
                if (!VB.Pstr(list.PPAND3, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[23, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND3, "^^", 1));      //비뇨기
                }
                if (!VB.Pstr(list.PPANK1, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[23, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK1, "^^", 1));        //과거병력
                    if (!VB.Pstr(list.PPANK1, "^^", 2).IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list.PPANK1, "^^", 2);
                    }
                }
                if (!VB.Pstr(list.PPAND4, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[24, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND4, "^^", 1));      //소화기
                }
                if (!VB.Pstr(list.PPANK2, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[24, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK2, "^^", 1));        //생활습관
                    if (!VB.Pstr(list.PPANK2, "^^", 2).IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list.PPANK2, "^^", 2);
                    }
                }
                if (!VB.Pstr(list.PPAND5, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[25, 4].Text = hf.READ_YN_NEW("2", VB.Pstr(list.PPAND5, "^^", 1));      //신경계
                }
                if (!VB.Pstr(list.PPANK2, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[25, 9].Text = hf.READ_YN_1("2", VB.Pstr(list.PPANK3, "^^", 1));        //외상후유증
                    if (!@VB.Pstr(list.PPANK3, "^^", 2).IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list.PPANK3, "^^", 2);
                    }
                }
                if (!VB.Pstr(list.PPAND6, "^^", 2).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[26, 4].Text = VB.Pstr(list.PPAND6, "^^", 2);      //기타
                }
                if (!VB.Pstr(list.PPANK2, "^^", 1).IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[26, 9].Text = hf.READ_Old2("2", VB.Pstr(list.PPANK4, "^^", 1));        //일반상태
                    if (!VB.Pstr(list.PPANK4, "^^", 2).IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list.PPANK4, "^^", 2);
                    }
                }
                if (!list.GBPAN.IsNullOrEmpty())
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
                    SS2.ActiveSheet.Cells[28, 4].Text = list.PPANREMARK1;    //종합소견
                    SS2.ActiveSheet.Cells[29, 4].Text = list.PPANREMARK2;    //조치사항
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

            result = hicSchoolNewService.UpdateGbDntPrtbyWrtNo(clsType.User.IdNumber, argWrtNo, "PAN");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("인쇄 완료 Setting 중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Result_Print_DPan(long argWrtNo)
        {
            string strTEMP = "";
            int nRead = 0;
            string strJumin = "";
            string strGuBun = "";
            string strPan1 = "";
            string strPan2 = "";
            string strFrDate = "";
            string strToDate = "";

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            HIC_JEPSU_PATIENT_SCHOOL list = hicJepsuPatientSchoolService.GetItembyJepDateWrtNo(strFrDate, strToDate, argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                nRead = 1;
                strJumin = clsAES.DeAES(list.JUMIN2);
                strTEMP = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                if (VB.L(strTEMP, "초등") > 1)
                {
                    strGuBun = "1";
                }
                else if (VB.L(strTEMP, "중학") > 1)
                {
                    strGuBun = "2";
                }
                else if (VB.L(strTEMP, "고등") > 1)
                {
                    strGuBun = "3";
                }
                strTEMP = "";
                SS3.ActiveSheet.Cells[3, 3].Text = VB.Space(3) + hb.READ_Ltd_Name(list.LTDCODE.ToString());
                SS3.ActiveSheet.Cells[3, 11].Text = VB.Space(3) + list.CLASS + "학년 " + list.BAN + "반 " + list.BUN + "번";
                SS3.ActiveSheet.Cells[4, 3].Text = list.SNAME;
                if (list.SEX == "M")
                {
                    SS3.ActiveSheet.Cells[4, 6].Text = "남";
                }
                else if (list.SEX == "F")
                {
                    SS3.ActiveSheet.Cells[4, 6].Text = "여";
                }
                SS3.ActiveSheet.Cells[4, 11].Text = VB.Space(3) + VB.Left(strJumin, 2) + "년 " + VB.Mid(strJumin, 3, 2) + "월 " + VB.Mid(strJumin, 5, 2) + "일";

                //초중고
                if (long.Parse(VB.Pstr(list.DPAN1, "^^", 1)) > 0 || long.Parse(VB.Pstr(list.DPAN1, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[8, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[8, 2].Text = "●";
                }

                if (long.Parse(VB.Pstr(list.DPAN1, "^^", 1)) > 0)
                {
                    SS3.ActiveSheet.Cells[8, 6].Text = "상 (" + VB.Pstr(list.DPAN1, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[8, 6].Text = "상 (  ) 개";
                }

                if (long.Parse(VB.Pstr(list.DPAN1, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[9, 6].Text = "하 (" + VB.Pstr(list.DPAN1, "^^", 2) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[9, 6].Text = "하 (  ) 개";
                }

                //우식발생
                if (long.Parse(VB.Pstr(list.DPAN2, "^^", 1)) > 0 || long.Parse(VB.Pstr(list.DPAN2, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[10, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 2].Text = "●";
                }

                if (long.Parse(VB.Pstr(list.DPAN2, "^^", 1)) > 0)
                {
                    SS3.ActiveSheet.Cells[10, 6].Text = "상 (" + VB.Pstr(list.DPAN2, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 6].Text = "상 (  ) 개";
                }
                if (long.Parse(VB.Pstr(list.DPAN2, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (" + VB.Pstr(list.DPAN2, "^^", 2) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (  ) 개";
                }

                //결손치아
                if (long.Parse(VB.Pstr(list.DPAN3, "^^", 1)) > 0 || long.Parse(VB.Pstr(list.DPAN3, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[12, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 2].Text = "●";
                }

                if (long.Parse(VB.Pstr(list.DPAN3, "^^", 1)) > 0)
                {
                    SS3.ActiveSheet.Cells[12, 6].Text = "상 (" + VB.Pstr(list.DPAN3, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 6].Text = "상 (  ) 개";
                }
                if (long.Parse(VB.Pstr(list.DPAN3, "^^", 2)) > 0)
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (" + VB.Pstr(list.DPAN3, "^^", 2) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (  ) 개";
                }

                if (VB.Pstr(list.DPAN4, "^^", 1) == "2")
                {
                    SS3.ActiveSheet.Cells[14, 4].Text = "●";
                }
                else if (VB.Pstr(list.DPAN4, "^^", 1) == "1")
                {
                    SS3.ActiveSheet.Cells[14, 2].Text = "●";
                }
                SS3.ActiveSheet.Cells[14, 6].Text = VB.Pstr(list.DPAN4, "^^", 2);

                if (list.DPAN5 == "1")
                {
                    SS3.ActiveSheet.Cells[15, 2].Text = "●";
                }
                else if (list.DPAN5 == "2")
                {
                    SS3.ActiveSheet.Cells[15, 4].Text = "●";
                }
                else if (list.DPAN5 == "3")
                {
                    SS3.ActiveSheet.Cells[15, 6].Text = "●";
                }

                if (list.DPAN6 == "1")
                {
                    SS3.ActiveSheet.Cells[15, 2].Text = "●";
                }
                else if (list.DPAN6 == "2")
                {
                    SS3.ActiveSheet.Cells[15, 4].Text = "●";
                }
                else if (list.DPAN6 == "3")
                {
                    SS3.ActiveSheet.Cells[15, 6].Text = "●";
                }
                SS3.ActiveSheet.Cells[15, 6].Text = "③그밖의치아상태";

                if (VB.Pstr(list.DPAN7, "^^", 1) == "1")
                {
                    SS3.ActiveSheet.Cells[17, 6].Text = "●";
                }
                else if (VB.Pstr(list.DPAN7, "^^", 1) == "2")
                {
                    SS3.ActiveSheet.Cells[17, 6].Text = "●";
                }
                else if (VB.Pstr(list.DPAN7, "^^", 1) == "3")
                {
                    SS3.ActiveSheet.Cells[17, 6].Text = "●그밖의치아상태";
                    SS3.ActiveSheet.Cells[18, 6].Text = VB.Pstr(list.DPAN7, "^^", 2);
                }

                //치주질환
                if (strGuBun == "2" || strGuBun == "3")
                {
                    strTEMP = "";
                    if (VB.Pstr(list.DPAN8, "^^", 1) == "1")
                    {
                        SS3.ActiveSheet.Cells[8, 13].Text = "치은출혈/비대(●)";
                        strTEMP = "OK";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[8, 13].Text = "치은출혈/비대(  )";
                    }

                    if (VB.Pstr(list.DPAN8, "^^", 2) == "1")
                    {
                        SS3.ActiveSheet.Cells[9, 13].Text = "치석형성(●)";
                        strTEMP = "OK";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[9, 13].Text = "치석형성(  )";
                    }
                    if (VB.Pstr(list.DPAN8, "^^", 3) == "1")
                    {
                        SS3.ActiveSheet.Cells[10, 13].Text = "치주낭형성(●)";
                        strTEMP = "OK";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[10, 13].Text = "치주낭형성(  )";
                    }
                    if (!VB.Pstr(list.DPAN8, "^^", 4).IsNullOrEmpty())
                    {
                        SS3.ActiveSheet.Cells[12, 13].Text = VB.Pstr(list.DPAN8, "^^", 4);
                        strTEMP = "OK";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[12, 13].Text = "그밖증상(  )";
                    }
                    if (strTEMP == "OK")
                    {
                        SS3.ActiveSheet.Cells[8, 11].Text = "●";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[8, 9].Text = "●";
                    }
                    if (list.DPAN9 == "2")
                    {
                        SS3.ActiveSheet.Cells[12, 11].Text = "●";
                    }
                    else if (list.DPAN9 == "1")
                    {
                        SS3.ActiveSheet.Cells[12, 9].Text = "●";
                    }
                }

                if (strGuBun == "3")
                {
                    if (list.DPAN10 == "2")
                    {
                        SS3.ActiveSheet.Cells[15, 11].Text = "●";
                    }
                    else if (list.DPAN10 == "1")
                    {
                        SS3.ActiveSheet.Cells[15, 9].Text = "●";
                    }
                    if (VB.Pstr(list.DPAN11, "^^", 1) == "1")
                    {
                        SS3.ActiveSheet.Cells[16, 9].Text = "●";
                        SS3.ActiveSheet.Cells[16, 12].Text = "이상( )";
                    }
                    else if (VB.Pstr(list.DPAN11, "^^", 1) == "2")
                    {
                        SS3.ActiveSheet.Cells[16, 9].Text = "●";
                        SS3.ActiveSheet.Cells[16, 12].Text = "이상(" + VB.Pstr(list.DPAN11, "^^", 2) + ")";
                    }
                }
                strPan1 = "";
                strPan2 = "";
                switch (list.DPAN12)
                {
                    case "1":
                        strPan1 = "정상";
                        break;
                    case "2":
                        strPan1 = "정상(경계)";
                        break;
                    case "3":
                        strPan1 = "정밀검사요함";
                        break;
                    default:
                        break;
                }

                switch (list.DPAN13)
                {
                    case "1":
                        strPan1 = "정상";
                        break;
                    case "2":
                        strPan1 = "정상(경계)";
                        break;
                    case "3":
                        strPan1 = "정밀검사요함";
                        break;
                    default:
                        break;
                }

                SS3.ActiveSheet.Cells[19, 3].Text = strPan1;
                SS3.ActiveSheet.Cells[19, 10].Text = strPan2;

                SS3.ActiveSheet.Cells[22, 1].Text = VB.Space(3) + list.DPANSOGEN; ;
                SS3.ActiveSheet.Cells[22, 8].Text = VB.Space(3) + list.DPANJOCHI; ;

                SS3.ActiveSheet.Cells[24, 3].Text = VB.Space(3) + list.DPANDRNO.ToString();
                SS3.ActiveSheet.Cells[24, 12].Text = VB.Space(3) + list.SDATE.ToString();

                SS3.ActiveSheet.Cells[25, 4].Text = VB.Space(3) + hb.READ_License_DrName(long.Parse(list.DPANDRNO.ToString()));
                FnDrNo = long.Parse(list.DPANDRNO.ToString());

                if (nRead > 0)
                {
                    string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = true;

                    ComFunc.ReadSysDate(clsDB.DbCon);

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    strTitle = "";
                    strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                    strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                    setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                    ///TODO : 이상훈(2020.02.03) HcPrint.bas (김경동) 개발완료 후 주석 해제....
                    //hp.HIC_CERT_INSERT(SS3, argWrtNo, "6A", FnDrNo);

                    sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
                    if (rdoCnt2.Checked == true)
                    {
                        sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
                    }
                }
            }
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
                SSMun1.ActiveSheet.Cells[9, 3].Text = VB.Pstr(list[0].PMUNA1, "^^", 1);
                SSMun1.ActiveSheet.Cells[9, 4].Text = VB.Pstr(list[0].PMUNA1, "^^", 2);
                SSMun1.ActiveSheet.Cells[9, 5].Text = VB.Pstr(list[0].PMUNA1, "^^", 3);

                SSMun1.ActiveSheet.Cells[10, 3].Text = VB.Pstr(list[0].PMUNA2, "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 4].Text = VB.Pstr(list[0].PMUNA2, "^^", 2);
                SSMun1.ActiveSheet.Cells[10, 5].Text = VB.Pstr(list[0].PMUNA2, "^^", 3);

                SSMun1.ActiveSheet.Cells[11, 3].Text = VB.Pstr(list[0].PMUNA3, "^^", 1);
                SSMun1.ActiveSheet.Cells[11, 4].Text = VB.Pstr(list[0].PMUNA3, "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 5].Text = VB.Pstr(list[0].PMUNA3, "^^", 3);

                SSMun1.ActiveSheet.Cells[12, 3].Text = VB.Pstr(list[0].PMUNA4, "^^", 1);
                SSMun1.ActiveSheet.Cells[12, 4].Text = VB.Pstr(list[0].PMUNA4, "^^", 2);
                SSMun1.ActiveSheet.Cells[12, 5].Text = VB.Pstr(list[0].PMUNA4, "^^", 3);

                SSMun1.ActiveSheet.Cells[13, 3].Text = VB.Pstr(list[0].PMUNA5, "^^", 1);
                SSMun1.ActiveSheet.Cells[13, 4].Text = VB.Pstr(list[0].PMUNA5, "^^", 2);
                SSMun1.ActiveSheet.Cells[13, 5].Text = VB.Pstr(list[0].PMUNA5, "^^", 3);

                SSMun1.ActiveSheet.Cells[14, 3].Text = VB.Pstr(list[0].PMUNA6, "^^", 1);
                SSMun1.ActiveSheet.Cells[14, 4].Text = VB.Pstr(list[0].PMUNA6, "^^", 2);
                SSMun1.ActiveSheet.Cells[14, 5].Text = VB.Pstr(list[0].PMUNA6, "^^", 3);

                SSMun1.ActiveSheet.Cells[15, 3].Text = VB.Pstr(list[0].PMUNA7, "^^", 1);
                SSMun1.ActiveSheet.Cells[15, 4].Text = VB.Pstr(list[0].PMUNA7, "^^", 2);
                SSMun1.ActiveSheet.Cells[15, 5].Text = VB.Pstr(list[0].PMUNA7, "^^", 3);

                //소화기계
                SSMun1.ActiveSheet.Cells[18, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[18, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[19, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[19, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[20, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[20, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 3), "^^", 2);

                //호흡기계
                SSMun1.ActiveSheet.Cells[21, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[21, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[22, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[22, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[23, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[23, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[24, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[24, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 4), "^^", 2);

                SSMun1.ActiveSheet.Cells[25, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 5), "^^", 1);
                SSMun1.ActiveSheet.Cells[25, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 5), "^^", 2);

                //눈.귀
                SSMun1.ActiveSheet.Cells[26, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[26, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[27, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[27, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[28, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[28, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[29, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 4), "^^", 2);

                //피부
                SSMun1.ActiveSheet.Cells[30, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[30, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[31, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 2), "^^", 2);

                //순환기계
                SSMun1.ActiveSheet.Cells[32, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[32, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[33, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[33, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 2), "^^", 2);

                //근골격계
                SSMun1.ActiveSheet.Cells[34, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[34, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[35, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[35, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[36, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[36, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[37, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[37, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 4), "^^", 2);

                //그밖의
                SSMun1.ActiveSheet.Cells[38, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 1), "^^", 1);
                SSMun1.ActiveSheet.Cells[38, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 1), "^^", 2);

                SSMun1.ActiveSheet.Cells[39, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 2), "^^", 1);
                SSMun1.ActiveSheet.Cells[39, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 2), "^^", 2);

                SSMun1.ActiveSheet.Cells[40, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 3), "^^", 1);
                SSMun1.ActiveSheet.Cells[40, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 3), "^^", 2);

                SSMun1.ActiveSheet.Cells[41, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 4), "^^", 1);
                SSMun1.ActiveSheet.Cells[41, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 4), "^^", 2);

                SSMun1.ActiveSheet.Cells[42, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 5), "^^", 1);
                SSMun1.ActiveSheet.Cells[42, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 5), "^^", 2);

                SSMun1.ActiveSheet.Cells[43, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 6), "^^", 1);
                SSMun1.ActiveSheet.Cells[43, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 6), "^^", 2);

                SSMun1.ActiveSheet.Cells[44, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 7), "^^", 1);
                SSMun1.ActiveSheet.Cells[44, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 7), "^^", 2);

                SSMun1.ActiveSheet.Cells[45, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 8), "^^", 1);
                SSMun1.ActiveSheet.Cells[45, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 8), "^^", 2);

                SSMun1.ActiveSheet.Cells[46, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 9), "^^", 1);
                SSMun1.ActiveSheet.Cells[46, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 9), "^^", 2);

                SSMun1.ActiveSheet.Cells[47, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 10), "^^", 1);
                SSMun1.ActiveSheet.Cells[47, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 10), "^^", 2);

                SSMun1.ActiveSheet.Cells[9, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 3);

                SSMun1.ActiveSheet.Cells[12, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 1);
                SSMun1.ActiveSheet.Cells[13, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 2);
                SSMun1.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 3);
                SSMun1.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 4);
                SSMun1.ActiveSheet.Cells[16, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 5);

                SSMun1.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list[0].PMUNC3, "^^", 1);
                SSMun1.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list[0].PMUNC3, "^^", 2);

                SSMun1.ActiveSheet.Cells[19, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 1);
                SSMun1.ActiveSheet.Cells[20, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 2);
                SSMun1.ActiveSheet.Cells[21, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 3);
                SSMun1.ActiveSheet.Cells[22, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 4);

                SSMun1.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 1);
                SSMun1.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 2);
                SSMun1.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 3);
                SSMun1.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 4);
                SSMun1.ActiveSheet.Cells[27, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 5);

                SSMun1.ActiveSheet.Cells[28, 9].Text = VB.Pstr(list[0].PMUNC6, "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 9].Text = VB.Pstr(list[0].PMUNC6, "^^", 2);

                SSMun1.ActiveSheet.Cells[30, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 2);
                SSMun1.ActiveSheet.Cells[32, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 3);
                SSMun1.ActiveSheet.Cells[33, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 4);
                SSMun1.ActiveSheet.Cells[34, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 5);
                SSMun1.ActiveSheet.Cells[35, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 6);

                //식생활
                SSMun1.ActiveSheet.Cells[9, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 1);
                SSMun1.ActiveSheet.Cells[10, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 2);
                SSMun1.ActiveSheet.Cells[11, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 3);
                SSMun1.ActiveSheet.Cells[12, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 4);
                SSMun1.ActiveSheet.Cells[13, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 5);

                SSMun1.ActiveSheet.Cells[14, 16].Text = VB.Pstr(list[0].PMUND2, "^^", 1);
                SSMun1.ActiveSheet.Cells[15, 16].Text = VB.Pstr(list[0].PMUND2, "^^", 2);

                SSMun1.ActiveSheet.Cells[16, 16].Text = VB.Pstr(list[0].PMUND3, "^^", 1);
                SSMun1.ActiveSheet.Cells[17, 16].Text = VB.Pstr(list[0].PMUND3, "^^", 2);

                SSMun1.ActiveSheet.Cells[18, 16].Text = VB.Pstr(list[0].PMUND4, "^^", 1);
                SSMun1.ActiveSheet.Cells[19, 16].Text = VB.Pstr(list[0].PMUND4, "^^", 2);

                SSMun1.ActiveSheet.Cells[20, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 1);
                SSMun1.ActiveSheet.Cells[21, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 2);
                SSMun1.ActiveSheet.Cells[22, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 3);
                SSMun1.ActiveSheet.Cells[23, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 4);
                SSMun1.ActiveSheet.Cells[24, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 5);
                SSMun1.ActiveSheet.Cells[25, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 6);

                SSMun1.ActiveSheet.Cells[26, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 1);
                SSMun1.ActiveSheet.Cells[27, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 2);

                SSMun1.ActiveSheet.Cells[28, 16].Text = VB.Pstr(list[0].PMUND7, "^^", 1);
                SSMun1.ActiveSheet.Cells[29, 16].Text = VB.Pstr(list[0].PMUND7, "^^", 2);

                SSMun1.ActiveSheet.Cells[30, 16].Text = VB.Pstr(list[0].PMUND8, "^^", 1);
                SSMun1.ActiveSheet.Cells[31, 16].Text = VB.Pstr(list[0].PMUND8, "^^", 2);

                SSMun1.ActiveSheet.Cells[32, 16].Text = list[0].PMUND9;

                SSMun1.ActiveSheet.Cells[42, 7].Text = list[0].PMUNREMARK1;
                SSMun1.ActiveSheet.Cells[42, 11].Text = list[0].PMUNREMARK2;
            }
            else
            {
                //중고등
                SSMun2.ActiveSheet.Cells[9, 3].Text = VB.Pstr(list[0].PMUNA1, "^^", 1);
                SSMun2.ActiveSheet.Cells[9, 4].Text = VB.Pstr(list[0].PMUNA1, "^^", 2);
                SSMun2.ActiveSheet.Cells[9, 5].Text = VB.Pstr(list[0].PMUNA1, "^^", 3);

                SSMun2.ActiveSheet.Cells[10, 3].Text = VB.Pstr(list[0].PMUNA2, "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 4].Text = VB.Pstr(list[0].PMUNA2, "^^", 2);
                SSMun2.ActiveSheet.Cells[10, 5].Text = VB.Pstr(list[0].PMUNA2, "^^", 3);

                SSMun2.ActiveSheet.Cells[11, 3].Text = VB.Pstr(list[0].PMUNA3, "^^", 1);
                SSMun2.ActiveSheet.Cells[11, 4].Text = VB.Pstr(list[0].PMUNA3, "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 5].Text = VB.Pstr(list[0].PMUNA3, "^^", 3);

                SSMun2.ActiveSheet.Cells[12, 3].Text = VB.Pstr(list[0].PMUNA4, "^^", 1);
                SSMun2.ActiveSheet.Cells[12, 4].Text = VB.Pstr(list[0].PMUNA4, "^^", 2);
                SSMun2.ActiveSheet.Cells[12, 5].Text = VB.Pstr(list[0].PMUNA4, "^^", 3);

                SSMun2.ActiveSheet.Cells[13, 3].Text = VB.Pstr(list[0].PMUNA5, "^^", 1);
                SSMun2.ActiveSheet.Cells[13, 4].Text = VB.Pstr(list[0].PMUNA5, "^^", 2);
                SSMun2.ActiveSheet.Cells[13, 5].Text = VB.Pstr(list[0].PMUNA5, "^^", 3);

                SSMun2.ActiveSheet.Cells[14, 3].Text = VB.Pstr(list[0].PMUNA6, "^^", 1);
                SSMun2.ActiveSheet.Cells[14, 4].Text = VB.Pstr(list[0].PMUNA6, "^^", 2);
                SSMun2.ActiveSheet.Cells[14, 5].Text = VB.Pstr(list[0].PMUNA6, "^^", 3);

                SSMun2.ActiveSheet.Cells[15, 3].Text = VB.Pstr(list[0].PMUNA7, "^^", 1);
                SSMun2.ActiveSheet.Cells[15, 4].Text = VB.Pstr(list[0].PMUNA7, "^^", 2);
                SSMun2.ActiveSheet.Cells[15, 5].Text = VB.Pstr(list[0].PMUNA7, "^^", 3);

                //소화기계
                SSMun2.ActiveSheet.Cells[18, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[18, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[19, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[19, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[20, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB1, "@@", 3), "^^", 2);

                //호흡기계
                SSMun2.ActiveSheet.Cells[21, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[21, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[22, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[23, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[23, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[24, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[24, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB2, "@@", 4), "^^", 2);

                //눈.귀
                SSMun2.ActiveSheet.Cells[25, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[25, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[26, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[26, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[27, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[27, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[28, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[28, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB3, "@@", 4), "^^", 2);

                //피부
                SSMun2.ActiveSheet.Cells[29, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[29, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[30, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[30, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB4, "@@", 2), "^^", 2);

                //순환기계
                SSMun2.ActiveSheet.Cells[31, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[32, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[32, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB5, "@@", 2), "^^", 2);

                //근골격계
                SSMun2.ActiveSheet.Cells[33, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[34, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[34, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[35, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[35, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[36, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[36, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB6, "@@", 4), "^^", 2);

                //그밖의
                SSMun2.ActiveSheet.Cells[37, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 1), "^^", 1);
                SSMun2.ActiveSheet.Cells[37, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 1), "^^", 2);

                SSMun2.ActiveSheet.Cells[38, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 2), "^^", 1);
                SSMun2.ActiveSheet.Cells[38, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 2), "^^", 2);

                SSMun2.ActiveSheet.Cells[39, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 3), "^^", 1);
                SSMun2.ActiveSheet.Cells[39, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 3), "^^", 2);

                SSMun2.ActiveSheet.Cells[40, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 4), "^^", 1);
                SSMun2.ActiveSheet.Cells[40, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 4), "^^", 2);

                SSMun2.ActiveSheet.Cells[41, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 5), "^^", 1);
                SSMun2.ActiveSheet.Cells[41, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 5), "^^", 2);

                SSMun2.ActiveSheet.Cells[42, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 6), "^^", 1);
                SSMun2.ActiveSheet.Cells[42, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 6), "^^", 2);

                SSMun2.ActiveSheet.Cells[43, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 7), "^^", 1);
                SSMun2.ActiveSheet.Cells[43, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 7), "^^", 2);

                SSMun2.ActiveSheet.Cells[44, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 8), "^^", 1);
                SSMun2.ActiveSheet.Cells[44, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 8), "^^", 2);

                SSMun2.ActiveSheet.Cells[45, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 9), "^^", 1);
                SSMun2.ActiveSheet.Cells[45, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 9), "^^", 2);

                SSMun2.ActiveSheet.Cells[46, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 10), "^^", 1);
                SSMun2.ActiveSheet.Cells[46, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 10), "^^", 2);

                SSMun2.ActiveSheet.Cells[47, 4].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 11), "^^", 1);
                SSMun2.ActiveSheet.Cells[47, 5].Text = VB.Pstr(VB.Pstr(list[0].PMUNB7, "@@", 11), "^^", 2);

                SSMun2.ActiveSheet.Cells[9, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 9].Text = VB.Pstr(list[0].PMUNC1, "^^", 3);

                SSMun2.ActiveSheet.Cells[12, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 1);
                SSMun2.ActiveSheet.Cells[13, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 2);
                SSMun2.ActiveSheet.Cells[14, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 3);
                SSMun2.ActiveSheet.Cells[15, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 4);
                SSMun2.ActiveSheet.Cells[16, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 5);
                SSMun2.ActiveSheet.Cells[17, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 6);
                SSMun2.ActiveSheet.Cells[18, 9].Text = VB.Pstr(list[0].PMUNC2, "^^", 7);

                SSMun2.ActiveSheet.Cells[19, 9].Text = VB.Pstr(list[0].PMUNC3, "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 9].Text = VB.Pstr(list[0].PMUNC3, "^^", 2);

                SSMun2.ActiveSheet.Cells[21, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 2);
                SSMun2.ActiveSheet.Cells[23, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 3);
                SSMun2.ActiveSheet.Cells[24, 9].Text = VB.Pstr(list[0].PMUNC4, "^^", 4);

                SSMun2.ActiveSheet.Cells[25, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 1);
                SSMun2.ActiveSheet.Cells[26, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 2);
                SSMun2.ActiveSheet.Cells[27, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 3);
                SSMun2.ActiveSheet.Cells[28, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 4);
                SSMun2.ActiveSheet.Cells[29, 9].Text = VB.Pstr(list[0].PMUNC5, "^^", 5);

                SSMun2.ActiveSheet.Cells[30, 9].Text = VB.Pstr(list[0].PMUNC6, "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 9].Text = VB.Pstr(list[0].PMUNC6, "^^", 2);

                SSMun2.ActiveSheet.Cells[32, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 2);
                SSMun2.ActiveSheet.Cells[34, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 3);
                SSMun2.ActiveSheet.Cells[35, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 4);
                SSMun2.ActiveSheet.Cells[36, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 5);
                SSMun2.ActiveSheet.Cells[37, 9].Text = VB.Pstr(list[0].PMUNC7, "^^", 6);

                //식생활
                SSMun2.ActiveSheet.Cells[9, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 1);
                SSMun2.ActiveSheet.Cells[10, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 2);
                SSMun2.ActiveSheet.Cells[11, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 3);
                SSMun2.ActiveSheet.Cells[12, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 4);
                SSMun2.ActiveSheet.Cells[13, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 5);
                SSMun2.ActiveSheet.Cells[14, 16].Text = VB.Pstr(list[0].PMUND1, "^^", 6);

                SSMun2.ActiveSheet.Cells[15, 16].Text = VB.Pstr(list[0].PMUND2, "^^", 1);
                SSMun2.ActiveSheet.Cells[16, 16].Text = VB.Pstr(list[0].PMUND2, "^^", 2);

                SSMun2.ActiveSheet.Cells[17, 16].Text = VB.Pstr(list[0].PMUND3, "^^", 1);
                SSMun2.ActiveSheet.Cells[18, 16].Text = VB.Pstr(list[0].PMUND3, "^^", 2);

                SSMun2.ActiveSheet.Cells[19, 16].Text = VB.Pstr(list[0].PMUND4, "^^", 1);
                SSMun2.ActiveSheet.Cells[20, 16].Text = VB.Pstr(list[0].PMUND4, "^^", 2);

                SSMun2.ActiveSheet.Cells[21, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 1);
                SSMun2.ActiveSheet.Cells[22, 16].Text = VB.Pstr(list[0].PMUND5, "^^", 2);

                SSMun2.ActiveSheet.Cells[23, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 1);
                SSMun2.ActiveSheet.Cells[24, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 2);
                SSMun2.ActiveSheet.Cells[25, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 3);
                SSMun2.ActiveSheet.Cells[26, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 4);
                SSMun2.ActiveSheet.Cells[27, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 5);
                SSMun2.ActiveSheet.Cells[28, 16].Text = VB.Pstr(list[0].PMUND6, "^^", 6);

                SSMun2.ActiveSheet.Cells[29, 16].Text = VB.Pstr(list[0].PMUND7, "^^", 1);
                SSMun2.ActiveSheet.Cells[30, 16].Text = VB.Pstr(list[0].PMUND7, "^^", 1);
                SSMun2.ActiveSheet.Cells[31, 16].Text = VB.Pstr(list[0].PMUND7, "^^", 1);

                SSMun2.ActiveSheet.Cells[32, 16].Text = VB.Pstr(list[0].PMUND8, "^^", 1);
                SSMun2.ActiveSheet.Cells[33, 16].Text = VB.Pstr(list[0].PMUND8, "^^", 2);

                SSMun2.ActiveSheet.Cells[34, 16].Text = list[0].PMUND9;
                SSMun2.ActiveSheet.Cells[42, 7].Text = list[0].PMUNREMARK1;
                SSMun2.ActiveSheet.Cells[42, 11].Text = list[0].PMUNREMARK2;
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
            SS2.ActiveSheet.Cells[30, 3].Text = "";
            SS2.ActiveSheet.Cells[30, 9].Text = "";
            SS2.ActiveSheet.Cells[31, 3].Text = "";

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
            }
            //최근질병?
            for (int i = 18; i <= 47; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 4].Text = "1.무";
                SSMun1.ActiveSheet.Cells[i, 5].Text = "";
            }
            //의사에게하고픈말,상담 ?
            SSMun1.ActiveSheet.Cells[42, 7].Text = "";
            SSMun1.ActiveSheet.Cells[42, 11].Text = "";

            SSMun2.ActiveSheet.Cells[1, 13].Text = "";
            SSMun2.ActiveSheet.Cells[2, 13].Text = "";
            SSMun2.ActiveSheet.Cells[3, 13].Text = "";
            SSMun2.ActiveSheet.Cells[4, 13].Text = "";
            SSMun2.ActiveSheet.Cells[4, 15].Text = "";
            //가족의학병력
            for (int i = 9; i <= 15; i++)
            {
                SSMun2.ActiveSheet.Cells[i, 3].Text = "1.무";
                SSMun2.ActiveSheet.Cells[i, 4].Text = "";
                SSMun2.ActiveSheet.Cells[i, 5].Text = "";
            }
            //최근질병?
            for (int i = 18; i <= 47; i++)
            {
                SSMun2.ActiveSheet.Cells[i, 4].Text = "1.무";
                SSMun2.ActiveSheet.Cells[i, 5].Text = "";
            }
            //의사에게하고픈말,상담 ?
            SSMun2.ActiveSheet.Cells[42, 7].Text = "";
            SSMun2.ActiveSheet.Cells[42, 11].Text = "";

            //초등문진
            for (int i = 9; i <= 35; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 9].Text = "1.N";
            }
            //건강생활
            for (int i = 9; i <= 32; i++)
            {
                SSMun1.ActiveSheet.Cells[i, 16].Text = "1.N";
            }
            //중고등문진
            //경험증상
            for (int i = 9; i <= 38; i++)
            {
                SSMun2.ActiveSheet.Cells[i, 9].Text = "1.N";
            }
            //건강생활
            for (int i = 9; i <= 35; i++)
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
                    else if (SSMun1.ActiveSheet.Cells[i, j].Text == "2.무" || SSMun1.ActiveSheet.Cells[i, j].Text == "2.N")
                    {
                        SSMun1.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun1.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 255, 255);
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
                    else if (SSMun2.ActiveSheet.Cells[i, j].Text == "2.무" || SSMun2.ActiveSheet.Cells[i, j].Text == "2.N")
                    {
                        SSMun2.ActiveSheet.Cells[i, j].ForeColor = Color.FromArgb(0, 0, 0);
                        SSMun2.ActiveSheet.Cells[i, j].BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }

            //구강판정
            SS3.ActiveSheet.Cells[3, 3].Text = "";
            SS3.ActiveSheet.Cells[3, 11].Text = "";
            SS3.ActiveSheet.Cells[4, 3].Text = "";
            SS3.ActiveSheet.Cells[4, 7].Text = "";
            SS3.ActiveSheet.Cells[4, 11].Text = "";
            SS3.ActiveSheet.Cells[8, 2].Text = "①";
            SS3.ActiveSheet.Cells[8, 4].Text = "②";
            SS3.ActiveSheet.Cells[8, 6].Text = "";
            SS3.ActiveSheet.Cells[8, 9].Text = "①";
            SS3.ActiveSheet.Cells[8, 11].Text = "②";
            SS3.ActiveSheet.Cells[8, 13].Text = "";
            SS3.ActiveSheet.Cells[9, 2].Text = "①";
            SS3.ActiveSheet.Cells[9, 4].Text = "②";
            SS3.ActiveSheet.Cells[9, 6].Text = "";
            SS3.ActiveSheet.Cells[9, 13].Text = "";
            SS3.ActiveSheet.Cells[10, 2].Text = "①";
            SS3.ActiveSheet.Cells[10, 4].Text = "②";
            SS3.ActiveSheet.Cells[10, 6].Text = "";
            SS3.ActiveSheet.Cells[10, 13].Text = "";
            SS3.ActiveSheet.Cells[11, 13].Text = "";
            SS3.ActiveSheet.Cells[12, 2].Text = "①";
            SS3.ActiveSheet.Cells[12, 4].Text = "②";
            SS3.ActiveSheet.Cells[12, 6].Text = "";
            SS3.ActiveSheet.Cells[12, 9].Text = "①";
            SS3.ActiveSheet.Cells[12, 11].Text = "②";
            SS3.ActiveSheet.Cells[13, 6].Text = "";
            SS3.ActiveSheet.Cells[14, 2].Text = "①";
            SS3.ActiveSheet.Cells[14, 4].Text = "②";
            SS3.ActiveSheet.Cells[15, 2].Text = "①";
            SS3.ActiveSheet.Cells[15, 4].Text = "②";
            SS3.ActiveSheet.Cells[15, 6].Text = "③";
            SS3.ActiveSheet.Cells[15, 9].Text = "①";
            SS3.ActiveSheet.Cells[15, 11].Text = "②";
            SS3.ActiveSheet.Cells[16, 2].Text = "①";
            SS3.ActiveSheet.Cells[16, 4].Text = "②";
            SS3.ActiveSheet.Cells[16, 6].Text = "③";
            SS3.ActiveSheet.Cells[16, 9].Text = "①";
            SS3.ActiveSheet.Cells[16, 11].Text = "②";
            SS3.ActiveSheet.Cells[17, 2].Text = "①";
            SS3.ActiveSheet.Cells[17, 4].Text = "②";
            SS3.ActiveSheet.Cells[18, 6].Text = "";
            SS3.ActiveSheet.Cells[19, 3].Text = "";
            SS3.ActiveSheet.Cells[19, 10].Text = "";
            SS3.ActiveSheet.Cells[22, 1].Text = "";
            SS3.ActiveSheet.Cells[22, 8].Text = "";
            SS3.ActiveSheet.Cells[24, 4].Text = "";
            SS3.ActiveSheet.Cells[24, 12].Text = "";
            SS3.ActiveSheet.Cells[25, 4].Text = "";
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
