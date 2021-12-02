using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillResultNotiSendList.cs
/// Description     : 건강검진 결과통보서 발송대장
/// Author          : 이상훈
/// Create Date     : 2021-01-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcBill104.frm(Frm발송대장)" />

namespace HC_Bill
{
    public partial class frmHcBillResultNotiSendList : Form
    {
        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuService hicJepsuService = null;


        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHcBillResultNotiSendList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.Rows[-1].Height = 20;

            dtpFDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpTDate.Text = clsPublic.GstrSysDate;

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            cboYear.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            txtLtdCode.Text = "";

            cboJohap.Items.Clear();
            cboJohap.Items.Add("사업장");
            cboJohap.Items.Add("공무원");
            cboJohap.Items.Add("성인병");
            cboJohap.Items.Add("암검진");
            cboJohap.SelectedIndex = 0;
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
                string strJumin = "";
                string strFDate = "";
                string strTDate = "";
                string strChasu = "";
                string strJohap = "";
                long nLtdCode = 0;

                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;

                strChasu = "";
                if (rdoChasu1.Checked == true)
                {
                    strChasu = "1";
                }
                else if (rdoChasu2.Checked == true)
                {
                    strChasu = "2";
                }

                strJohap = cboJohap.Text;

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.Rows[-1].Height = 20;

                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetSendListItembyJepDate(strFDate, strTDate, strChasu, strJohap, nLtdCode);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    if (cboJohap.Text.Trim() != "암검진")
                    {
                        switch (list[i].GJJONG)
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "23":
                                SS1.ActiveSheet.Cells[i, 0].Text = "1";
                                break;
                            case "16":
                            case "17":
                            case "18":
                            case "19":
                            case "28":
                                SS1.ActiveSheet.Cells[i, 0].Text = "2";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "암검사";
                    }

                    switch (cboJohap.Text)
                    {
                        case "사업장":
                            SS1.ActiveSheet.Cells[i, 1].Text = "직장가입자";
                            break;
                        case "공무원":
                            SS1.ActiveSheet.Cells[i, 1].Text = "공교";
                            break;
                        case "성인병":
                            switch (list[i].GBINWON)
                            {
                                case "12":
                                case "13":
                                    SS1.ActiveSheet.Cells[i, 1].Text = "직장가입자";
                                    break;
                                case "11":
                                    SS1.ActiveSheet.Cells[i, 1].Text = "지역가입자";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "암검진":
                            SS1.ActiveSheet.Cells[i, 1].Text = "";
                            break;
                        default:
                            break;
                    }

                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    strJumin = clsAES.DeAES(list[i].JUMIN2);
                    SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].TONGBODATE;

                    if (cboJohap.Text.Trim() == "사업장")
                    {
                        hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                        SS1.ActiveSheet.Cells[i, 6].Text = clsHcVariable.GstrLtdJuso;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].JUSO1 + " " + list[i].JUSO2;
                    }
                    progressBar1.Value = i + 1;
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                if (txtLtdCode.Text.Trim() == "")
                {
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = " 건강검진 결과통보서 발송대장";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진 기관명 : " + VB.Pstr(txtLtdCode.Text, ".", 2) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strName = "";

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        if (txtLtdCode.Text.Trim().IndexOf(".") > 0)
                        {
                            strName = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                        }
                        else
                        {
                            strName = hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                        }

                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                        else
                        {
                            txtLtdCode.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
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
    }
}
