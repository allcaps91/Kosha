using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    /// <summary>
    /// Class Name      : HC_Print
    /// File Name       : frmHcPrint_Dental.cs
    /// Description     : 혈액종합판정 결과지출력
    /// Author          : 김경동
    /// Create Date     : 2021-07-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " Frm혈액종합판정결과지.frm(Frm혈액종합판정)" />
    /// 
    public partial class frmHcBloodExam : Form
    {

        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();
        clsHcMain hm = new clsHcMain();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;



        public frmHcBloodExam()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);


        }

        private void SetControl()
        {
            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtName.Text = "";
            txtLtdCode.Text = "";
            lblLtdName.Text = "";
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
                btnPrint.Enabled = true;
                
            }

            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
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
            #endregion
            else if (sender == btnPrint)
            {

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {

                        HIC_JEPSU_RES_ETC_PATIENT nHJREP = new HIC_JEPSU_RES_ETC_PATIENT
                        {
                                    
                            SNAME = SSList.ActiveSheet.Cells[i, 1].Text,
                            GJJONG = SSList.ActiveSheet.Cells[i, 2].Text,
                            LTDCODE = SSList.ActiveSheet.Cells[i, 3].Text.To<long>(0),
                            JEPDATE = SSList.ActiveSheet.Cells[i, 4].Text,
                            JUMIN = SSList.ActiveSheet.Cells[i, 5].Text,
                            TONGBODATE = SSList.ActiveSheet.Cells[i, 6].Text,
                            WRTNO = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 7].Text),
                            UCODES = SSList.ActiveSheet.Cells[i, 8].Text
                        };

                        Spread_Print(nHJREP);
                    }
                }
                
            }

        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
        private void Screen_Display(FpSpread Spd)
        {

            long nLtdcode = 0;
            long nWrtno = 0;
            string strRePrt = "";
            string strCHK3 = "";


            txtName.Text = txtName.Text.Trim();
            if (ChkGbRe.Checked) { strRePrt = "Y"; }



            List<HIC_JEPSU_RES_ETC_PATIENT> list = hicJepsuResEtcPatientService.GetItembyJepDate(dtpFDate.Text, dtpTDate.Text, nLtdcode, txtName.Text, strRePrt, "62", nWrtno, "");
            if( !list.IsNullOrEmpty())
            {
                for (int i = 0; i < list.Count; i++)
                {

                    SSList.ActiveSheet.Cells[i, 0].Text = "";
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE.Trim();
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN.Trim();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].TONGBODATE.To<string>("");
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 8].Text = hm.UCode_Names_Display(list[i].UCODES);

                    strCHK3 = "";
                    strCHK3 = hm.READ_JEPSU_GBCHK3(list[i].WRTNO);
                    if (strCHK3 == "OK")
                    {

                    }
                }
            }
        }
        private void Spread_Print(HIC_JEPSU_RES_ETC_PATIENT nHJREP)
        {
            string strRePrt = "";

            if (ChkGbRe.Checked) { strRePrt = "Y"; }

            frmHcBloodExam_Sub fHP = new frmHcBloodExam_Sub(nHJREP, strRePrt);
            fHP.ShowDialog();
        }
    }
}
