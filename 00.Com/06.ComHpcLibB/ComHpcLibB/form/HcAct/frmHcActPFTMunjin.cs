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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcActPFTMunjin.cs
/// Description     : 폐활량 검사 문진표 & 검사표
/// Author          : 이상훈
/// Create Date     : 2020-02-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcActPFT.frm(FrmHcActPFT)" />

namespace ComHpcLibB
{
    public partial class frmHcActPFTMunjin : Form
    {
        HicResultService hicResultService = null;
        HicResSpecialService hicResSpecialService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicPatientService hicPatientService = null;
        HeaResultService heaResultService = null;
        HicJepsuService hicJepsuService = null;

        frmHcActPFTMunjin_Print FrmHcActPFTMunjin_Print = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        string FstrPtNo;
        long FnWrtNo;
        bool FbPrt;
        long FnHcWRTNO;
        string FsFormID;
        string FsGubun;

        public frmHcActPFTMunjin(string sFormID, string sGubun, long nHcWRTNO, string strPtno, long nWrtNo)
        {
            InitializeComponent();

            FsFormID = sFormID;
            FsGubun = sGubun;
            FstrPtNo = strPtno;
            FnHcWRTNO = nHcWRTNO;            
            FnWrtNo = nWrtNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            hicResSpecialService = new HicResSpecialService();
            comHpcLibBService = new ComHpcLibBService();
            hicPatientService = new HicPatientService();
            heaResultService = new HeaResultService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.chkDenT0.Click += new EventHandler(eCheckBoxClick);
            this.chkDenT1.Click += new EventHandler(eCheckBoxClick);
            this.chkDenT2.Click += new EventHandler(eCheckBoxClick);
            this.chkDrug0.Click += new EventHandler(eCheckBoxClick);
            this.chkDrug1.Click += new EventHandler(eCheckBoxClick);
            this.chkDrug2.Click += new EventHandler(eCheckBoxClick);
            this.chkDrug3.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi0.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi1.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi2.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi3.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi4.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi5.Click += new EventHandler(eCheckBoxClick);
            this.chkGeumGi6.Click += new EventHandler(eCheckBoxClick);
            this.chkGrade0.Click += new EventHandler(eCheckBoxClick);
            this.chkGrade1.Click += new EventHandler(eCheckBoxClick);
            this.chkGrade2.Click += new EventHandler(eCheckBoxClick);
            this.chkGrade3.Click += new EventHandler(eCheckBoxClick);
            this.chkGrade4.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan0.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan1.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan2.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan3.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan4.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan5.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan6.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan7.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan8.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan9.Click += new EventHandler(eCheckBoxClick);
            this.chkJilhwan10.Click += new EventHandler(eCheckBoxClick);
            this.chkSmk0.Click += new EventHandler(eCheckBoxClick);
            this.chkSmk1.Click += new EventHandler(eCheckBoxClick);
            this.chkSmk2.Click += new EventHandler(eCheckBoxClick);
            this.chkSmoke.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen0.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen1.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen2.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen3.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen4.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen5.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen6.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen7.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen8.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen9.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen10.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen11.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen12.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen13.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen14.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen15.Click += new EventHandler(eCheckBoxClick);
            this.chkSogen16.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul0.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul1.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul2.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul3.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul4.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul5.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul6.Click += new EventHandler(eCheckBoxClick);
            this.chkSusul7.Click += new EventHandler(eCheckBoxClick);
            this.chkYN0.Click += new EventHandler(eCheckBoxClick);
            this.chkYN1.Click +=  new EventHandler(eCheckBoxClick);
            this.txtDayCnt.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtSyymm0.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtSyymm1.LostFocus += new EventHandler(eTxtLostFocus);            
            this.txtDrug.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.txtJilHwan.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.txtSusul.KeyUp += new KeyEventHandler(eTxtKeyUp); 
            this.txtPftDate.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtPftSabun.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSyymm0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSyymm1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strGubun = "";

            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);

            cboSN.Items.Clear();

            if (!FsFormID.IsNullOrEmpty() && FsFormID == "HCACT")
            {
                cboSN.Items.Add(" ");
                cboSN.Items.Add("14811079");
                cboSN.Items.Add("14810632");
                cboSN.Items.Add("14810633");
                cboSN.Items.Add("14813567");
            }
            else
            {
                cboSN.Items.Add(" ");
                cboSN.Items.Add("14811403");
                cboSN.Items.Add("14812589");
            }

            fn_Screen_Clear();

            chkYN1.Checked = true;

            //strGubun = "HIC";
            strGubun = FsGubun;

            //인적사항을 Read
            HIC_PATIENT list = hicPatientService.GetItembyPtNo(FstrPtNo);

            if (!list.IsNullOrEmpty())
            {
                lblSName.Text = list.SNAME;
                lblJumin.Text = VB.Left(clsAES.DeAES(list.JUMIN2), 6) + "-" + VB.Right(clsAES.DeAES(list.JUMIN2), 7);
            }

            cboSN.SelectedIndex = cboSN.FindString(clsHcVariable.GstrPFTSN);

            if (cboSN.Text.IsNullOrEmpty())
            {
                cboSN.Text = clsHcVariable.GstrPFTSN;
            }

            if (FnHcWRTNO > 0)
            {
                fn_PftMunjin(FnHcWRTNO, strGubun, FnWrtNo);
            }
            else
            {
                MessageBox.Show("접수번호가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblJobSabun.Text += hb.READ_HIC_InsaName(clsType.User.IdNumber);

            this.Text += " 접수번호 : " + FnWrtNo;

            //의사사번구분
            if (!clsHcVariable.GnHicLicense.IsNullOrEmpty() && clsHcVariable.GnHicLicense != 0)
            {
                btnSave.Visible = false;
                lblJobSabun.Visible = false;
            }
        }

        void fn_PftMunjin(long argHcWRTNO, string argGubun, long argWrtNo)
        {
            int n = 0;
            string strResult = "";
            string strTemp = "";
            string strIEMUNNO = "";

            HIC_RES_SPECIAL list = hicResSpecialService.GetItemByWrtno(argHcWRTNO);

            if (!list.IsNullOrEmpty())
            {
                if (list.WEIGHT == 0)
                {
                    txtWeight.Text = "";
                }
                else
                {
                    txtWeight.Text = list.WEIGHT.To<string>();
                }

                if (list.HEIGHT == 0)
                {
                    txtHeight.Text = "";
                }
                else
                {
                    txtHeight.Text = list.HEIGHT.To<string>();
                }
            

                //키/몸무게 Data 없으면 계측정보 불러옴
                if (txtWeight.Text.IsNullOrEmpty() || txtHeight.Text.IsNullOrEmpty())
                {
                    if (argGubun == "HIC")
                    {
                        List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyWrtNoExCode(argHcWRTNO);

                        if (list2.Count > 0)
                        {
                            for (int i = 0; i < list2.Count; i++)
                            {
                                if (list2[i].EXCODE.Trim() == "A101")
                                {
                                    if (txtWeight.Text.Trim() == "")
                                    {
                                        txtWeight.Text = list2[i].RESULT;
                                    }
                                }
                                else if (list2[i].EXCODE == "A102")
                                {
                                    if (txtHeight.Text.Trim() == "")
                                    {
                                        txtHeight.Text = list2[i].RESULT;
                                    }
                                }
                            }
                        }
                    }
                    else if (argGubun == "HEA")
                    {
                        List<HEA_RESULT> list2 = heaResultService.GetExCodeResultbyWrtNoExCode(argWrtNo);

                        if (list2.Count > 0)
                        {
                            for (int i = 0; i < list2.Count; i++)
                            {
                                if (list2[i].EXCODE == "A101")
                                {
                                    if (txtWeight.Text.Trim() == "")
                                    {
                                        txtWeight.Text = list2[i].RESULT;
                                    }
                                }
                                else if (list2[i].EXCODE == "A102")
                                {
                                    if (txtHeight.Text.Trim() == "")
                                    {
                                        txtHeight.Text = list2[i].RESULT;
                                    }
                                }
                            }
                        }
                    }
                }

                //폐활량검사 경험
                if (!list.GBCAPACITY.IsNullOrEmpty())
                {
                    strResult = list.GBCAPACITY;
                    switch (strResult)
                    {
                        case "0":
                            chkYN0.Checked = true;
                            break;
                        case "1":
                            chkYN1.Checked = true;
                            break;
                        //case "2":
                        //    chkYN2.Checked = true;
                        //    break;
                        default:
                            break;
                    }
                }

                //2019추가사항
                if (!list.GBGEUMGI.IsNullOrEmpty())
                {
                    strResult = list.GBGEUMGI;
                    for (int i = 1; i <= 7; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + (i - 1).ToString(), true)[0] as CheckBox);
                            chkGeumGi.Checked = true;

                        }
                    }
                }
                else
                {
                    chkGeumGi0.Checked = true;
                }

                //과거또는현재의 질병
                if (!list.OLDJILHWAN.IsNullOrEmpty())
                {
                    strResult = list.OLDJILHWAN;
                    for (int i = 1; i <= 11; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + (i - 1).ToString(), true)[0] as CheckBox);
                            chkJilhwan.Checked = true;
                        }
                    }
                }
                else
                {
                    chkJilhwan0.Checked = true;
                }

                //과거또는현재의 질병기타사항
                if (!list.OLDJILHWAN_ETC.IsNullOrEmpty())
                {
                    txtJilHwan.Text = list.OLDJILHWAN_ETC;
                }

                //수술경험
                if (!list.GBSUSUL.IsNullOrEmpty())
                {
                    strResult = list.GBSUSUL;
                    for (int i = 1; i <= 8; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            CheckBox chkSusul = (Controls.Find("chkSusul" + (i - 1).ToString(), true)[0] as CheckBox);
                            chkSusul.Checked = true;
                        }
                    }
                }
                else
                {
                    chkSusul0.Checked = true;
                }

                //수술경험기타사항
                if (!list.GBSUSUL_ETC.IsNullOrEmpty())
                {
                    txtSusul.Text = list.GBSUSUL_ETC;
                }

                //현재복용약물
                if (!list.GBDRUG.IsNullOrEmpty())
                {
                    strResult = list.GBDRUG;
                    for (int i = 1; i <= 4; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            CheckBox chkDrug = (Controls.Find("chkDrug" + (i - 1).ToString(), true)[0] as CheckBox);
                            chkDrug.Checked = true;
                        }
                    }
                }
                else
                {
                    chkDrug0.Checked = true;
                }

                //현재복용약물기타사항
                if (!list.GBDRUG_ETC.IsNullOrEmpty())
                {
                    txtDrug.Text = list.GBDRUG_ETC;
                }

                //흡연력
                if (list.GBSMOKE1 == "1" || list.SMOKEYEAR.IsNullOrEmpty())
                {
                    chkSmoke.Checked = true;
                }

                strIEMUNNO = "";
                strIEMUNNO = hicJepsuService.GetIEMUNNObyWrtNo(argWrtNo);

                if (list.SMOKEYEAR.IsNullOrEmpty())
                {
                    n = 0;
                }
                else
                {
                    n = list.SMOKEYEAR.Length - list.SMOKEYEAR.Replace(",", "").Length;
                }

                if (n != 3)
                {
                    if (!list.SMOKEYEAR.IsNullOrEmpty())
                    {
                        strResult = list.SMOKEYEAR.Trim();
                        for (int i = 1; i <= 2; i++)
                        {
                            if (i == 2)
                            {
                                txtDayCnt.Text = VB.Pstr(strResult, ",", i);
                            }
                            else
                            {
                                txtSyymm0.Text = VB.Pstr(strResult, ",", i);
                            }
                        }
                    }
                }
                else
                {
                    if (!list.SMOKEYEAR.IsNullOrEmpty())
                    {
                        strResult = list.SMOKEYEAR.Trim();
                        for (int i = 1; i <= 3; i++)
                        {
                            if (i == 3)
                            {
                                txtDayCnt.Text = VB.Pstr(strResult, ",", i);
                            }
                            else
                            {
                                TextBox txtSyymm = (Controls.Find("txtSyymm" + (i - 1).ToString(), true)[0] as TextBox);
                                txtSyymm.Text = VB.Pstr(strResult, ",", i);
                            }
                        }
                    }
                }

                //금일흡연여부
                if (!list.GBSMOKE.IsNullOrEmpty())
                {
                    strResult = list.GBSMOKE;
                    switch (strResult)
                    {
                        case "0":
                            chkSmk0.Checked = true;
                            break;
                        case "1":
                            chkSmk1.Checked = true;
                            break;
                        case "2":
                            chkSmk2.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    chkSmk0.Checked = true;
                }

                //의치착용여부
                if (!list.GBDENTY.IsNullOrEmpty())
                {
                    strResult = list.GBDENTY;
                    switch (strResult)
                    {
                        case "0":
                            chkDenT0.Checked = true;
                            break;
                        case "1":
                            chkDenT1.Checked = true;
                            break;
                        case "2":
                            chkDenT2.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    chkDenT0.Checked = true;
                }

                //호흡곤란정도
                if (!list.DYSPNEA.IsNullOrEmpty())
                {
                    strResult = list.DYSPNEA;
                    switch (strResult)
                    {
                        case "0":
                            chkGrade0.Checked = true;
                            break;
                        case "1":
                            chkGrade1.Checked = true;
                            break;
                        case "2":
                            chkGrade2.Checked = true;
                            break;
                        case "3":
                            chkGrade3.Checked = true;
                            break;
                        case "4":
                            chkGrade4.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    chkGrade0.Checked = true;
                }

                //검사자세
                if (list.POSTURE.IsNullOrEmpty())
                {
                    rdoStand1.Checked = true;
                }
                else if (list.POSTURE == "0")
                {
                    rdoStand0.Checked = true;
                }
                else
                {
                    rdoStand1.Checked = true;
                }

                //장비 S/N
                if (!list.SERIALNO.IsNullOrEmpty())
                {
                    cboSN.FindString(list.SERIALNO, -1);
                    if (cboSN.Text.IsNullOrEmpty())
                    {
                        cboSN.Text = list.SERIALNO;
                    }
                }

                //검사협조
                if (list.COOPERATE == "1")
                {
                    rdoCooper1.Checked = true;
                }
                else if (list.COOPERATE == "2")
                {
                    rdoCooper2.Checked = true;
                }
                else
                {
                    rdoCooper0.Checked = true;
                }

                //검사자의견
                if (!list.PSOGEN.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(list.PSOGEN, "{}") - 1; i++)
                    {
                        strTemp = VB.Pstr(list.PSOGEN, "{}", i + 1);
                        CheckBox chkSogen = (Controls.Find("chkSogen" + i.ToString(), true)[0] as CheckBox);
                        chkSogen.Checked = strTemp == "1" ? true : false;
                    }
                }

                //검사기타의견
                txtEtcSogen.Text = list.PETCSOGEN;

                txtPftDate.Text = list.PFTDATE;
                txtPftSabun.Text = list.PFTSABUN.To<string>();

                if (txtPftDate.Text.IsNullOrEmpty())
                {
                    txtPftDate.Text = clsPublic.GstrSysDate;
                    if (txtPftSabun.Text.IsNullOrEmpty() || txtPftSabun.Text.To<int>() == 0)
                    {
                        txtPftSabun.Text = clsType.User.IdNumber;
                    }
                }

                lblJobName.Text = hb.READ_HIC_InsaName(txtPftSabun.Text);
                lblJobName.Text = lblJobName.Text.Trim();

                if (list.PFTSABUN == 0)
                {
                    lblSTS.Text = "신규 ";
                    lblSTS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0FFC0"));
                    FbPrt = false;
                }
                else
                {
                    lblSTS.Text = "수정 ";
                    lblSTS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0C0FF"));
                    FbPrt = true;
                }

                if (list.BMI.IsNullOrEmpty())
                {
                    txtBMI.Text = "";
                }
                else
                {
                    txtBMI.Text = list.BMI;
                }
            }
        }

        void fn_Screen_Clear()
        {
            //폐활량 문진표
            lblSName.Text = ""; lblJumin.Text = "";
            txtWeight.Text = ""; txtHeight.Text = ""; txtJilHwan.Text = "";
            txtSusul.Text = ""; txtDrug.Text = ""; txtDayCnt.Text = "";

            for (int i = 0; i <= 8; i++)
            {   
                CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                
                if (i < 2)
                {
                    CheckBox chkYN = (Controls.Find("chkYN" + i.ToString(), true)[0] as CheckBox);
                    TextBox txtSyymm = (Controls.Find("txtSyymm" + i.ToString(), true)[0] as TextBox);
                    chkYN.Checked = false;
                    txtSyymm.Text = "";
                }                
                chkJilhwan.Checked = false;
                if (i < 7)
                {
                    CheckBox chkSusul = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                    CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + i.ToString(), true)[0] as CheckBox);
                    chkSusul.Checked = false;
                    chkGeumGi.Checked = false;
                }

                if (i < 2)
                {
                    CheckBox chkDrug = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                    CheckBox chkSmk = (Controls.Find("chkSmk" + i.ToString(), true)[0] as CheckBox);
                    CheckBox chkDenT = (Controls.Find("chkDenT" + i.ToString(), true)[0] as CheckBox);
                    chkDrug.Checked = false;
                    chkSmk.Checked = false;
                    chkDenT.Checked = false;
                }
                if (i < 5)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    chkGrade.Checked = false;
                }
            }

            chkSmoke.Checked = false;
            rdoStand1.Checked = false;
            chkSogen0.Checked = true;

            for (int i = 1; i <= 16; i++)
            {
                CheckBox chkSogen = (Controls.Find("chkSogen" + i.ToString(), true)[0] as CheckBox);
                chkSogen.Checked = false;
            }

            txtEtcSogen.Text = "";
            txtPftDate.Text = clsPublic.GstrSysDate;
            txtPftSabun.Text = "";
            lblJobName.Text = "";
            lblSTS.Text = "";
            lblSTS.BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0C0C0"));
            FbPrt = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnPrint)
            {
                if (FnWrtNo > 0)
                {
                    //검사자 사번이 등록된 경우만 출력되도록 수정해야함
                    if (FbPrt == true)
                    {
                        FrmHcActPFTMunjin_Print = new frmHcActPFTMunjin_Print(FnWrtNo, lblSName.Text, lblJumin.Text);
                        FrmHcActPFTMunjin_Print.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show("폐활량 검사표가 작성되지 않았습니다.", "출력불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (sender == btnSave)
            {
                string strCapacity = "";
                string strOldJilHwan = "";
                string strOldJilHwan_ETC = "";
                string strSusul = "";
                string strSusul_ETC = "";
                string strDrug = "";
                string strDrug_Etc = "";
                string strSmoke = "";
                string strGbSmoke = "";
                string strDenty = "";
                string strDyspnea = "";
                string strGeumGi = "";
                string strGbSmoke1 = "";
                string strPosture = "";
                string strSerialNo = "";
                string strCooper = "";
                string strPSogen = "";
                string strPEtcSogen = "";
                string strBMI = "";
                int result = 0;

                if (FnWrtNo == 0) return;

                if (hicResSpecialService.GetCountbyWrtNo(FnWrtNo) == 0)
                {
                    MessageBox.Show("폐활량 검사 대상자가 아닙니다.", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cboSN.Text.Trim() == "")
                {
                    MessageBox.Show("검사장비 S/N (Serial No) 입력요망", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (rdoStand0.Checked == true)
                {
                    if (txtEtcSogen.Text.Trim() == "")
                    {
                        MessageBox.Show("선 자세인 경우 기타의견을 기재해주십시오.", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (lblJobName.Text == "")
                {
                    MessageBox.Show("검사자가 공란입니다.", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //인적사항을 READ
                //txtPftSabun.Text = "35660";
                if (comHpcLibBService.GetSignImagebyHicDojangSabun(txtPftSabun.Text.Trim()) == 0)
                {
                    MessageBox.Show("폐활량 검사자 싸인 이미지가 없습니다.", "사번 등록 불가!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    return;
                }

                //폐활량검사 경험 유무(무:0, 유:1)
                strCapacity = "";
                for (int i = 0; i < 1; i++)
                {
                    CheckBox chkYN = (Controls.Find("chkYN" + i.ToString(), true)[0] as CheckBox);
                    if (chkYN.Checked == true)
                    {
                        strCapacity = i.ToString();
                    }
                }

                //금기사항확인
                strGeumGi = "";
                for (int i = 0; i <= 6; i++)
                {
                    CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + i.ToString(), true)[0] as CheckBox);
                    if (chkGeumGi.Checked == true)
                    {
                        strGeumGi += "1" + ",";
                    }
                    else
                    {
                        strGeumGi += "0" + ",";
                    }
                }

                //과거또는현재질병
                strOldJilHwan = "";
                for (int i = 0; i <= 8; i++)
                {
                    
                    CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                    if (chkJilhwan.Checked == true)
                    {
                        strOldJilHwan += "1" + ",";
                    }
                    else
                    {
                        strOldJilHwan += "0" + ",";
                    }
                }

                //과거또는현재질병기타사항
                strOldJilHwan_ETC = "";
                if (!txtJilHwan.Text.IsNullOrEmpty())
                {
                    strOldJilHwan_ETC = txtJilHwan.Text.Trim();
                }

                //수술경험
                strSusul = "";
                for (int i = 0; i <= 6; i++)
                {
                    CheckBox chkSusul = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                    if (chkSusul.Checked == true)
                    {
                        strSusul += "1" + ",";
                    }
                    else
                    {
                        strSusul += "0" + ",";
                    }
                }

                //수술경험기타사항
                strDrug = "";
                for (int i = 0; i <= 2; i++)
                {
                    CheckBox chkDrug = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                    if (chkDrug.Checked == true)
                    {
                        strDrug += "1" + ",";
                    }
                    else
                    {
                        strDrug += "0" + ",";
                    }
                }

                //현재약물복용여부기타사항
                strDrug_Etc = "";
                if (txtDrug.Text.Trim() != "")
                {
                    strDrug_Etc = txtDrug.Text.Trim();
                }

                //흡연관련문진
                strSmoke = "";
                strSmoke = txtSyymm0.Text.Trim() + "," + txtSyymm1.Text.Trim() + ",";
                strSmoke += txtDayCnt.Text.Trim() + ",";

                if (chkSmoke.Checked == true)
                {
                    strGbSmoke1 = "1";
                }

                //금일흡연여부
                strGbSmoke = "";
                for (int i = 0; i <= 2; i++)
                {
                    CheckBox chkSmk = (Controls.Find("chkSmk" + i.ToString(), true)[0] as CheckBox);
                    if (chkSmk.Checked == true)
                    {
                        strGbSmoke = i.ToString();
                    }
                }

                //의치착용여부
                strDenty = "";
                for (int i = 0; i <= 2; i++)
                {
                    CheckBox chkDenT = (Controls.Find("chkDenT" + i.ToString(), true)[0] as CheckBox);
                    if (chkDenT.Checked == true)
                    {
                        strDenty = i.ToString();
                    }
                }

                //호흡곤란정도
                strDyspnea = "";
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (chkGrade.Checked == true)
                    {
                        strDyspnea = i.ToString();
                    }
                }

                strPosture = "";
                //검사자세
                if (rdoStand0.Checked == true)
                {
                    strPosture = "0";
                }
                else
                {
                    strPosture = "1";
                }

                //검사장비 S/N
                strSerialNo = "";
                if (cboSN.Text.Trim() != "")
                {
                    strSerialNo = cboSN.Text.Trim();
                }

                strCooper = "";
                //검사협조
                if (rdoCooper0.Checked == true)
                {
                    strCooper = "0";
                }
                else if (rdoCooper1.Checked == true)
                {
                    strCooper = "1";
                }
                else
                {
                    strCooper = "2";
                }

                //검사자 의견
                strPSogen = "";
                for (int i = 0; i <= 16; i++)
                {
                    CheckBox chkSogen = (Controls.Find("chkSogen" + i.ToString(), true)[0] as CheckBox);
                    if (chkSogen.Checked == true)
                    {
                        strPSogen += "1" + "{}";
                    }
                    else
                    {
                        strPSogen += "0" + "{}";
                    }
                }

                //기타소견
                strPEtcSogen = "";
                if (txtEtcSogen.Text.Trim() != "")
                {
                    strPEtcSogen = txtEtcSogen.Text.Trim();
                }

                strBMI = txtBMI.Text;

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_RES_SPECIAL item = new HIC_RES_SPECIAL();

                item.WEIGHT = txtWeight.Text.To<decimal>();
                item.HEIGHT = txtHeight.Text.To<decimal>();
                item.GBCAPACITY = strCapacity;
                item.OLDJILHWAN = strOldJilHwan;
                item.OLDJILHWAN_ETC = strOldJilHwan_ETC;
                item.GBSUSUL = strSusul;
                item.GBSUSUL_ETC = strSusul_ETC;
                item.GBDRUG = strDrug;
                item.GBDRUG_ETC = strDrug_Etc;
                item.SMOKEYEAR = strSmoke;
                item.GBSMOKE = strGbSmoke;
                item.GBDENTY = strDenty;
                item.DYSPNEA = strDyspnea;
                item.GBGEUMGI = strGeumGi;
                item.GBSMOKE1 = strGbSmoke1;
                item.POSTURE = strPosture;
                item.SERIALNO = strSerialNo;
                item.COOPERATE = strCooper;
                item.PSOGEN = strPSogen;
                item.PETCSOGEN = strPEtcSogen;
                item.PFTDATE = txtPftDate.Text;
                item.PFTSABUN = txtPftSabun.Text.To<long>();
                item.BMI = txtBMI.Text;
                item.WRTNO = FnWrtNo;

                result = hicResSpecialService.UPdateSpecialbyWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("특수검진 문진 및 판정결과 저장 중 오류 발생!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                clsPublic.GstrRetValue = "OK";

                FbPrt = true;
                //출력기능은 필요시만 사용하므로 저장할때 폼 닫힘
                this.Close();
            }
        }
        
        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (sender == chkDenT0 || sender == chkDenT1 || sender == chkDenT2)
            {

                if(sender == chkDenT0)
                {
                    chkDenT1.Checked = false;
                    chkDenT2.Checked = false;
                }
 
                else if (sender == chkDenT1)
                {
                    chkDenT0.Checked = false;
                    chkDenT2.Checked = false;
                }
                else if (sender == chkDenT2)
                {
                    chkDenT0.Checked = false;
                    chkDenT1.Checked = false;
                }


                //for (int i = 0; i <= 2; i++)
                //{
                //    CheckBox chkDenT = (Controls.Find("chkDenT" + i.ToString(), true)[0] as CheckBox);
                //    if (i == 0)
                //    {
                //        if (chkDenT.Checked == true)
                //        {
                //            chkDenT1.Checked = false;
                //            chkDenT2.Checked = false;
                //        }
                //    }
                //    else if (i == 1)
                //    {
                //        if (chkDenT.Checked == true)
                //        {
                //            chkDenT0.Checked = false;
                //            chkDenT2.Checked = false;
                //        }
                //    }
                //    else if (i == 2)
                //    {
                //        if (chkDenT.Checked == true)
                //        {
                //            chkDenT0.Checked = false;
                //            chkDenT1.Checked = false;
                //        }
                //    }
                //}
            }
            else if (sender == chkDrug0 || sender == chkDrug1 || sender == chkDrug2 || sender == chkDrug3)
            {

                if (sender == chkDrug0)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        CheckBox chkDrugi = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                        chkDrugi.Checked = false;
                    }
                }
                else
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        CheckBox chkDrugi = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                        if (chkDrugi.Checked == true)
                        {
                            chkDrug0.Checked = false;
                        }
                    }
                }





                //for (int i = 0; i <= 3; i++)
                //{
                //    CheckBox chkDrug = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                //    if (i == 0)
                //    {
                //        if (chkDrug.Checked == true)
                //        {
                //            for (int j = 1; j <= 3; j++)
                //            {
                //                CheckBox chkDrugTemp = (Controls.Find("chkDrug" + i.ToString(), true)[0] as CheckBox);
                //                chkDrugTemp.Checked = false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (chkDrug.Checked == true)
                //        {
                //            chkDrug0.Checked = false;
                //        }
                //    }
                //}
            }
            else if (sender == chkGeumGi0 || sender == chkGeumGi1 || sender == chkGeumGi2 || sender == chkGeumGi3 || sender == chkGeumGi4 || sender == chkGeumGi5 || sender == chkGeumGi6)
            {

                if (sender == chkGeumGi0)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + i.ToString(), true)[0] as CheckBox);
                        chkGeumGi.Checked = false;
                    }
                }

                else
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + i.ToString(), true)[0] as CheckBox);
                        if (chkGeumGi.Checked == true)
                        {
                            chkGeumGi0.Checked = false;
                        }
                    }
                }

                //for (int i = 0; i <= 6; i++)
                //{
                //    CheckBox chkGeumGi = (Controls.Find("chkGeumGi" + i.ToString(), true)[0] as CheckBox);
                //    if (i == 0)
                //    {
                //        for (int j = 1; j <= 6; j++)
                //        {
                //            chkGeumGi.Checked = false;
                //        }
                //    }
                //    else
                //    {
                //        if (chkGeumGi.Checked == true)
                //        {
                //            chkGeumGi0.Checked = false;
                //        }
                //    }
                //}
            }
            else if (sender == chkGrade0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (i != 0)
                    {
                        chkGrade.Checked = false;
                    }
                }
            }
            else if (sender == chkGrade1)
            {
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (i != 1)
                    {
                        chkGrade.Checked = false;
                    }
                }
            }
            else if (sender == chkGrade2)
            {
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (i != 2)
                    {
                        chkGrade.Checked = false;
                    }
                }
            }
            else if (sender == chkGrade3)
            {
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (i != 3)
                    {
                        chkGrade.Checked = false;
                    }
                }
            }
            else if (sender == chkGrade4)
            {
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkGrade = (Controls.Find("chkGrade" + i.ToString(), true)[0] as CheckBox);
                    if (i != 4)
                    {
                        chkGrade.Checked = false;
                    }
                }
            }
            else if (sender == chkJilhwan0 || sender == chkJilhwan1 || sender == chkJilhwan2 || sender == chkJilhwan3 ||
                     sender == chkJilhwan4 || sender == chkJilhwan5 || sender == chkJilhwan6 || sender == chkJilhwan7 ||
                     sender == chkJilhwan8 || sender == chkJilhwan9 || sender == chkJilhwan10)
            {

                if (sender == chkJilhwan0)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                        chkJilhwan.Checked = false;
                        txtJilHwan.Text = "";
                    }

                }
                else
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                        if (chkJilhwan.Checked == true)
                        {
                            chkJilhwan0.Checked = false;
                        }
                    }
                }


                //for (int i = 0; i <= 3; i++)
                //{
                //    CheckBox chkJilhwan = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                //    if (i == 0)
                //    {
                //        if (chkJilhwan.Checked == true)
                //        {
                //            for (int j = 1; j <= 10; j++)
                //            {
                //                CheckBox cchkJilhwanTemp = (Controls.Find("chkJilhwan" + i.ToString(), true)[0] as CheckBox);
                //                cchkJilhwanTemp.Checked = false;
                //            }
                //            txtJilHwan.Text = "";
                //        }
                //    }
                //    else
                //    {
                //        if (chkJilhwan.Checked == true)
                //        {
                //            chkJilhwan0.Checked = false;
                //        }
                //    }
                //}
            }
            else if (sender == chkSmk0)
            {
                if (chkSmk0.Checked == true)
                {
                    chkSmk1.Checked = false;
                    chkSmk2.Checked = false;
                }
            }
            else if (sender == chkSmk1)
            {
                if (chkSmk1.Checked == true)
                {
                    chkSmk0.Checked = false;
                    chkSmk2.Checked = false;
                }
            }
            else if (sender == chkSmk2)
            {
                if (chkSmk2.Checked == true)
                {
                    chkSmk0.Checked = false;
                    chkSmk1.Checked = false;
                }
            }
            else if (sender == chkSmoke)
            {
                if (chkSmoke.Checked == true)
                {
                    txtSyymm0.Text = "";
                    txtDayCnt.Text = "";
                }
            }
            else if (sender == chkSogen0 || sender == chkSogen1 || sender == chkSogen2 || sender == chkSogen3 ||
                     sender == chkSogen4 || sender == chkSogen5 || sender == chkSogen6 || sender == chkSogen7 ||
                     sender == chkSogen8 || sender == chkSogen9 || sender == chkSogen10 || sender == chkSogen11 ||
                     sender == chkSogen12 || sender == chkSogen13 || sender == chkSogen14 || sender == chkSogen15 || 
                     sender == chkSogen16)
            {
                 List<string> strExCode = new List<string>();

                 rdoCooper0.Checked = true;
                 for (int i = 1; i <= 16; i++)
                 {
                     CheckBox chkSogen = (Controls.Find("chkSogen" + i.ToString(), true)[0] as CheckBox);
                     if (chkSogen.Checked == true)
                     {
                         if (i >= 1 && i <= 3)
                         {
                             rdoCooper1.Checked = true;
                         }
                         else
                         {
                             rdoCooper2.Checked = true;
                             break;
                         }
                     }

                     if (sender == chkSogen)
                     {
                         if (chkSogen.Checked == false)
                         {
                             return;
                         }
                     }
                 }

                 if (sender == chkSogen0)
                 {
                     for (int i = 1; i <= 16; i++)
                     {
                         CheckBox chkSogen = (Controls.Find("chkSogen" + i.ToString(), true)[0] as CheckBox);
                         if (chkSogen.Checked == true)
                         {
                            chkSogen.Checked = false;
                         }
                         rdoCooper0.Checked = true;
                     }
                    txtBMI.Text = "";

                }
                else
                 {
                     chkSogen0.Checked = false;
                 }

                 if (chkSogen3.Checked == true)
                 {
                     strExCode.Clear();
                     strExCode.Add("A115");
                     
                     HIC_RESULT list = hicResultService.GetResultbyWrtNo(FnWrtNo, strExCode);
                     
                     if (!list.IsNullOrEmpty())
                     {
                         txtBMI.Text = list.RESULT.Trim();
                     }
                     else
                     {
                         txtBMI.Text = "";
                     }
                 }
            } 
            else if (sender == chkSusul0 || sender == chkSusul1 || sender == chkSusul2 || sender == chkSusul3 ||
                     sender == chkSusul4 || sender == chkSusul5 || sender == chkSusul6 || sender == chkSusul7)
            {
                 if (sender == chkSusul0)
                 {

                    for (int i = 1; i <= 7; i++)
                    {
                        CheckBox chkSusuli = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                        chkSusuli.Checked = false;
                    }
                    txtSusul.Text = "";

                    //if (chkSusul0.Checked == true)
                    //{
                    //    for (int i = 1; i <= 7; i++)
                    //    {
                    //        CheckBox chkSusul = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                    //        chkSusul.Checked = false;
                    //    }
                    //    txtSusul.Text = "";
                    //}
                }
                else
                 {

                    for (int i = 0; i <= 7; i++)
                    {
                        CheckBox chkSusuli = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                        if (chkSusuli.Checked == true)
                        {
                            chkSusul0.Checked = false;
                        }
                    }

                    //for (int i = 0; i <= 7; i++)
                    //{
                    //    CheckBox chkSusulTemp = (Controls.Find("chkSusul" + i.ToString(), true)[0] as CheckBox);
                    //    if (sender == chkSusulTemp)
                    //    {
                    //        if (chkSusulTemp.Checked == true)
                    //        {
                    //            chkSusulTemp.Checked = false;
                    //        }
                    //    }
                    //}                    
                }
            }
            else if (sender == chkYN0 || sender == chkYN1)
            {
                if (sender == chkYN0)
                { 
                    if (chkYN0.Checked == true)
                    {
                        chkYN1.Checked = false;
                    }
                }
                else
                {
                    if (chkYN1.Checked == true)
                    {
                        chkYN0.Checked = false;
                    }
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPftSabun)
            {
                if (e.KeyChar == 13)
                {
                    if (txtPftSabun.Text.Trim() != "")
                    {
                        btnSave.Enabled = true;
                        //인적사항을 READ
                        if (comHpcLibBService.GetCoutnbyDojangSabun(txtPftSabun.Text.Trim()) == 0)
                        {
                            MessageBox.Show("폐활량 검사자 싸인 이미지가 없습니다.", "사번 등록 불가!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnSave.Enabled = false;
                        }
                        lblJobName.Text = hb.READ_HIC_InsaName(txtPftSabun.Text.Trim());
                    }
                }
            }
            else if (sender == txtSyymm0 || sender == txtSyymm1)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtPftDate)
            {
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                clsPublic.GstrCalDate = "";
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                txtPftDate.Text = clsPublic.GstrCalDate;
            }
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            if (sender == txtDrug)
            {
                if (txtDrug.Text.Trim() == "")
                {
                    return;
                }

                if (chkDrug3.Checked == false)
                {
                    chkDrug3.Checked = true;
                }
            }
            else if (sender == txtJilHwan)
            {
                if (txtJilHwan.Text.Trim() == "")
                {
                    return;
                }
                if (chkJilhwan10.Checked == false)
                {
                    chkJilhwan10.Checked = true;
                }
            }
            else if (sender == txtSusul)
            {
                if (txtSusul.Text.Trim() == "")
                {
                    return;
                }
                if (chkSusul7.Checked == false)
                {
                    chkSusul7.Checked = true;
                }
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (txtDayCnt.Text != "")
            {
                if (chkSmoke.Checked == true)
                {
                    chkSmoke.Checked = false;
                }

                if (txtSyymm0.Text.Trim() == "")
                {
                    MessageBox.Show("흡연력 년수가 없습니다.", "흡연년수 입력요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSyymm0.Focus();
                }
            }
            else if (sender == txtSyymm0 || sender == txtSyymm1)
            {
                if (txtSyymm0.Text.Trim().To<long>() > 0)
                {
                    if (chkSmoke.Checked == true)
                    {
                        chkSmoke.Checked = false;
                    }
                }
            }
        }
    }
}
