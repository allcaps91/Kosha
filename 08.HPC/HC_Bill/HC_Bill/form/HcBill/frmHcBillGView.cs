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
/// File Name       : frmHcBillGView.cs
/// Description     : 공무원 인원 조회
/// Author          : 이상훈
/// Create Date     : 2021-01-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcBill081.frm(FrmGView)" />

namespace HC_Bill
{
    public partial class frmHcBillGView : Form
    {
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

        public frmHcBillGView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
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

            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-60).ToShortDateString();
            dtpTDate.Text = clsPublic.GstrSysDate;

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            cboYear.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            txtLtdCode.Text = "";
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
                string strFrDate = "";
                string strToDate = "";
                string strYear = "";
                string[] strGjJong = { "12", "17" };
                string strLtdName = "";
                string strKiho = "";
                string strChung = "";
                string strchk1 = "";
                string strchk2 = "";

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.Rows[-1].Height = 20;

                if (txtLtdCode.Text.Trim() == "")
                {
                    return;
                }

                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;

                strYear = cboYear.Text;
                strLtdName = VB.Pstr(txtLtdCode.Text, ".", 2);

                strKiho = txtKiho.Text.Trim();

                if (rdoChung1.Checked == true)
                {
                    strChung = "1";
                }
                else
                {
                    strChung = "0";
                }

                strchk1 = "";
                strchk2 = "";
                if (chk1.Checked == true && chk2.Checked == true)
                {
                }
                else
                {
                    if (chk1.Checked == true)
                    {
                        strchk1 = "Y";
                    }
                    if (chk2.Checked == true)
                    {
                        strchk2 = "Y";
                    }
                }

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateGjYearGjJong(strFrDate, strToDate, strYear, strLtdName, strKiho, strChung, strchk1, strchk2);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                progressBar1.Maximum = nRead;
                if (list.Count > 0)
                {
                    for (int i = 0; i < nRead; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].AGE.To<string>();
                        SS1.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].LTDCODE.To<string>();
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].KIHO;
                        progressBar1.Value = i + 1;
                    }
                }
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
