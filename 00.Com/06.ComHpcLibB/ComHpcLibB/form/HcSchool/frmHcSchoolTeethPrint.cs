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
/// File Name       : frmHcSchoolTeethPrint.cs
/// Description     : 구강문진표 인쇄
/// Author          : 이상훈
/// Create Date     : 2020-01-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool2.frm(HcSchool02)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolTeethPrint : Form
    {
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;
        HicJepsuSchoolNewService hicJepsuSchoolNewService = null;
        HicSchoolNewService hicSchoolNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long FnDrNo;

        public frmHcSchoolTeethPrint()
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
            hicJepsuSchoolNewService = new HicJepsuSchoolNewService();
            hicSchoolNewService = new HicSchoolNewService();

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
            else if (sender == btnPrint)
            {
                long nWRTNO = 0;
                string strChk = "";

                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    nWRTNO = SS1.ActiveSheet.Cells[i, 8].Text.Trim().To<long>();

                    if (rdoPrint1.Checked == true)
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_DMun(nWRTNO);
                        }
                        if (chkPrtGbn2.Checked == true)
                        {
                            fn_Result_Print_DPan(nWRTNO);
                        }

                        fn_PrintDntOK(nWRTNO); //인쇄완료세팅
                    }
                    else if (rdoPrint2.Checked == true && strChk == "True")
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_DMun(nWRTNO);
                        }
                    }

                    if (chkPrtGbn2.Checked == true)
                    {
                        fn_Result_Print_DPan(nWRTNO);
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                        fn_PrintDntOK(nWRTNO);  //인쇄완료세팅
                    }
                    else if (rdoPrint3.Checked == true && strChk != "True")
                    {
                        fn_Sheet_Clear();
                        if (chkPrtGbn1.Checked == true)
                        {
                            fn_Result_Print_DMun(nWRTNO);
                        }
                        if (chkPrtGbn2.Checked == true)
                        {
                            fn_Result_Print_DPan(nWRTNO);
                        }
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                        fn_PrintDntOK(nWRTNO);  //인쇄완료세팅
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                int nRow = 0;
                string strJumin = "";
                string strFrDate = "";
                string strToDate = "";
                string strChkRePrint = "";
                string strSName = "";
                long nLtdCode = 0;
                string strClass = "";
                string strBan = "";

                sp.Spread_All_Clear(SS1);

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (chkRePrint.Checked == true)
                {
                    strChkRePrint = "1";
                }
                else
                {
                    strChkRePrint = "0";
                }

                strSName = txtSName.Text.Trim();
                nLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1).To<long>();
                strClass = cboClass.Text;
                strBan = cboBan.Text;
                
                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDate(strFrDate, strToDate, strChkRePrint, strSName, nLtdCode, strClass, strBan);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].CLASS.To<string>();
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].BAN.To<string>();
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].BUN.To<string>();
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].SEX + "/" + list[i].AGE;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].WRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 9].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                }

                btnPrint.Enabled = true;
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
            //구강문진
            SS2.ActiveSheet.Cells[4, 5].Text = "";
            SS2.ActiveSheet.Cells[5, 5].Text = "";
            SS2.ActiveSheet.Cells[6, 5].Text = "";
            SS2.ActiveSheet.Cells[7, 5].Text = "";
            SS2.ActiveSheet.Cells[7, 7].Text = "";
            for (int i = 11; i <= 21; i++)
            {
                SS2.ActiveSheet.Cells[i, 2].Text = "";
                SS2.ActiveSheet.Cells[i, 3].Text = "";
            }
            SS2.ActiveSheet.Cells[11, 4].Text = "";
            SS2.ActiveSheet.Cells[14, 4].Text = "";
            SS2.ActiveSheet.Cells[15, 4].Text = "";
            SS2.ActiveSheet.Cells[19, 4].Text = "";
            SS2.ActiveSheet.Cells[21, 4].Text = "";
            SS2.ActiveSheet.Cells[23, 1].Text = "";

            //구강판정
            SS3.ActiveSheet.Cells[3, 3].Text = "";
            SS3.ActiveSheet.Cells[3, 11].Text = "";
            SS3.ActiveSheet.Cells[4, 3].Text = "";
            SS3.ActiveSheet.Cells[4, 7].Text = "";
            SS3.ActiveSheet.Cells[4, 11].Text = "";

            SS3.ActiveSheet.Cells[8, 2].Text = "①";
            SS3.ActiveSheet.Cells[8, 2].Text = "②";
            SS3.ActiveSheet.Cells[8, 6].Text = "";
            SS3.ActiveSheet.Cells[8, 9].Text = "①";
            SS3.ActiveSheet.Cells[8, 11].Text = "②";
            SS3.ActiveSheet.Cells[8, 13].Text = "";
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



        void fn_Result_Print_DMun(long nWrtNo)
        {
            string strTEMP = "";
            string StrJumin = "";
            string strFrDate = "";
            string strToDate = "";

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            //자료를 SELECT
            HIC_JEPSU_PATIENT_SCHOOL list = hicJepsuPatientSchoolService.GetItembyJepDateWrtNo(strFrDate, strToDate, nWrtNo);

            if (!list.IsNullOrEmpty())
            {
                StrJumin = clsAES.DeAES(list.JUMIN2);

                SS2.ActiveSheet.Cells[4, 5].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                SS2.ActiveSheet.Cells[5, 5].Text = list.CLASS + "학년 " + list.BAN + "학년 " + list.BUN + "번";
                SS2.ActiveSheet.Cells[6, 5].Text = list.SNAME;
                
                if (list.SEX == "M")
                {
                    SS2.ActiveSheet.Cells[7, 5].Text = "남";
                }
                else if (list.SEX == "F")
                {
                    SS2.ActiveSheet.Cells[7, 5].Text = "여";
                }
                SS2.ActiveSheet.Cells[7, 7].Text = VB.Left(StrJumin, 2) + "년" + VB.Mid(StrJumin, 3, 2) + "월" + VB.Mid(StrJumin, 5, 2) + "일";
                if (list.DMUN1 == "1")
                {
                    SS2.ActiveSheet.Cells[11, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[11, 3].Text = "√";
                }
                if (list.DMUN2 == "1")
                {
                    SS2.ActiveSheet.Cells[13, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[13, 3].Text = "√";
                }
                if (list.DMUN3 == "1")
                {
                    SS2.ActiveSheet.Cells[15, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[15, 3].Text = "√";
                }
                if (list.DMUN4 == "1")
                {
                    SS2.ActiveSheet.Cells[17, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[17, 3].Text = "√";
                }
                if (list.DMUN5 == "1")
                {
                    SS2.ActiveSheet.Cells[19, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[19, 3].Text = "√";
                }
                if (list.DMUN6 == "1")
                {
                    SS2.ActiveSheet.Cells[21, 2].Text = "√";
                }
                else
                {
                    SS2.ActiveSheet.Cells[21, 3].Text = "√";
                }

                switch (list.DMUN7)
                {
                    case "1":
                        SS2.ActiveSheet.Cells[11, 4].Text = "●있다. ②없다. ③모르겠다.";
                        break;
                    case "2":
                        SS2.ActiveSheet.Cells[11, 4].Text = "①있다. ●없다. ③모르겠다.";
                        break;
                    case "3":
                        SS2.ActiveSheet.Cells[11, 4].Text = "①있다. ②없다. ●모르겠다.";
                        break;
                    default:
                        break;
                }

                strTEMP = "";
                if (VB.Pstr(list.DMUN8, "^^", 1) == "1")
                {
                    strTEMP += "●아침식사전 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 1) == "0")
                {
                    strTEMP += "①아침식사전 ";
                }
                if (VB.Pstr(list.DMUN8, "^^", 2) == "1")
                {
                    strTEMP += "●아침식사후 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 2) == "0")
                {
                    strTEMP += "②아침식사후 ";
                }
                SS2.ActiveSheet.Cells[14, 4].Text = strTEMP;

                strTEMP = "";
                if (VB.Pstr(list.DMUN8, "^^", 3) == "1")
                {
                    strTEMP += "●점심식사후 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 3) == "0")
                {
                    strTEMP += "③점심식사후 ";
                }
                if (VB.Pstr(list.DMUN8, "^^", 4) == "1")
                {
                    strTEMP += "●저녁식사후 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 4) == "0")
                {
                    strTEMP += "④저녁식사후 ";
                }
                SS2.ActiveSheet.Cells[15, 4].Text = strTEMP;

                strTEMP = "";
                if (VB.Pstr(list.DMUN8, "^^", 5) == "1")
                {
                    strTEMP += "●잠자기직전 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 5) == "0")
                {
                    strTEMP += "⑤잠자기직전 ";
                }
                if (VB.Pstr(list.DMUN8, "^^", 6) == "1")
                {
                    strTEMP += "●간식섭취후 ";
                }
                else if (VB.Pstr(list.DMUN8, "^^", 6) == "0")
                {
                    strTEMP += "⑥간식섭취후 ";
                }
                SS2.ActiveSheet.Cells[16, 4].Text = strTEMP;

                switch (list.DMUN9)
                {
                    case "1":
                        SS2.ActiveSheet.Cells[19, 4].Text = "●그렇다. ②보통이다. ③아니다.";
                        break;
                    case "2":
                        SS2.ActiveSheet.Cells[19, 4].Text = "①그렇다. ●보통이다. ③아니다.";
                        break;
                    case "3":
                        SS2.ActiveSheet.Cells[19, 4].Text = "①그렇다. ②보통이다. ●아니다.";
                        break;
                    default:
                        break;
                }
                switch (list.DMUN10)
                {
                    case "1":
                        SS2.ActiveSheet.Cells[20, 4].Text = "●예. ②아니오. ③불소치약이 무엇인지모름.";
                        break;
                    case "2":
                        SS2.ActiveSheet.Cells[20, 4].Text = "①예. ●아니오. ③불소치약이 무엇인지모름.";
                        break;
                    case "3":
                        SS2.ActiveSheet.Cells[20, 4].Text = "①예. ②아니오. ●불소치약이 무엇인지모름.";
                        break;
                    default:
                        break;
                }
                SS2.ActiveSheet.Cells[23, 4].Text = list.DMUNREMARK;
            }

            //인쇄
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

            sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void fn_Result_Print_DPan(long nWrtNo)
        {
            string strTEMP = "";
            string strJumin = "";
            string strGuBun = "";
            string strPan1 = "";
            string strPan2 = "";
            string strFrDate = "";
            string strToDate = "";

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            HIC_JEPSU_PATIENT_SCHOOL list = hicJepsuPatientSchoolService.GetItembyJepDateWrtNo(strFrDate, strToDate, nWrtNo);

            if (!list.IsNullOrEmpty())
            {
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
                if (VB.Pstr(list.DPAN1, "^^", 1).To<long>() > 0 || VB.Pstr(list.DPAN1, "^^", 2).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[8, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[8, 2].Text = "●";
                }

                if (VB.Pstr(list.DPAN1, "^^", 1).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[8, 6].Text = "상 (" + VB.Pstr(list.DPAN1, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[8, 6].Text = "상 (  ) 개";
                }

                if (VB.Pstr(list.DPAN1, "^^", 2).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[9, 6].Text = "하 (" + VB.Pstr(list.DPAN1, "^^", 2) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[9, 6].Text = "하 (  ) 개";
                }

                //우식발생
                if (VB.Pstr(list.DPAN2, "^^", 1).To<long>() > 0 || VB.Pstr(list.DPAN2, "^^", 2).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[10, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 2].Text = "●";
                }

                if (VB.Pstr(list.DPAN2, "^^", 1).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[10, 6].Text = "상 (" + VB.Pstr(list.DPAN2, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 6].Text = "상 (  ) 개";
                }
                if (VB.Pstr(list.DPAN2, "^^", 2).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (" + VB.Pstr(list.DPAN2, "^^", 2) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[11, 6].Text = "하 (  ) 개";
                }

                //결손치아
                if (VB.Pstr(list.DPAN3, "^^", 1).To<long>() > 0 || VB.Pstr(list.DPAN3, "^^", 2).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[12, 4].Text = "●";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 2].Text = "●";
                }

                if (VB.Pstr(list.DPAN3, "^^", 1).To<long>() > 0)
                {
                    SS3.ActiveSheet.Cells[12, 6].Text = "상 (" + VB.Pstr(list.DPAN3, "^^", 1) + ") 개";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 6].Text = "상 (  ) 개";
                }
                if (VB.Pstr(list.DPAN3, "^^", 2).To<long>() > 0)
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

                SS3.ActiveSheet.Cells[25, 4].Text = VB.Space(3) + hb.READ_License_DrName(list.DPANDRNO);
                FnDrNo = list.DPANDRNO;
            }

            //인쇄
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

            sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void fn_PrintDntOK(long nWrtNo)
        {
            int result = 0;

            //인쇄완료 SET
            clsDB.setBeginTran(clsDB.DbCon);

            result = hicSchoolNewService.UpdateGbDntPrtbyWrtNo(clsType.User.IdNumber, nWrtNo, "DNT");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("인쇄 완료 Setting 중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}
