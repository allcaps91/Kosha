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
    /// Description     : 구강검진 결과지출력
    /// Author          : 김경동
    /// Create Date     : 2021-06-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " Frm검진구강결과지.frm(Frm검진구강결과지)" />
    /// 
    public partial class frmHcPrint_Dental : Form
    {

        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();
        clsHcMain hm = new clsHcMain();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        HicResDentalJepsuPatientService hicResDentalJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicResBohum1Service hicResBohum1Service = null;


        public frmHcPrint_Dental()
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
            //this.btnExit.Click += new EventHandler(eBtnClick);


            this.menuExit.Click += new EventHandler(eMenuClick);

        }

        private void SetControl()
        {
            hicResDentalJepsuPatientService = new HicResDentalJepsuPatientService();
            hicResultService = new HicResultService();
            hicResBohum1Service = new HicResBohum1Service();


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
                long nWrtno = 0;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 10].Text);
                        Spread_Print(nWrtno);
                    }
                }
            }
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {

            int nRow = 0;

            bool bOK = false;

            long nWrtno = 0;

            string strLtdCode = "";
            string strHphone = "";
            string strEMail = "";
            string strCHK3 = "";
            string strRePrint = "";
            string strSort = "";

            strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
            txtName.Text = txtName.Text.Trim();

            if (ChkGbRe.Checked) { strRePrint = "OK"; }
            strSort = "2";

            List<HIC_RES_DENTAL_JEPSU_PATEINT> list = hicResDentalJepsuPatientService.GetItemByPandateSnameLtdGubun(dtpFDate.Text, dtpTDate.Text, txtName.Text, strLtdCode, strRePrint, strSort);

            if(!list.IsNullOrEmpty())
            {
                //SSList.ActiveSheet.RowCount = list.Count;
                nRow = 0;
                for ( int i = 0; i < list.Count; i++)
                {
                    nWrtno = list[i].WRTNO;

                    bOK = true;
                    string[] strExCodes = { "ZD00", "ZD01", "ZD99" };

                    if (hicResultService.GetCountbyWrtNoNotIn(nWrtno, strExCodes) > 0)
                    {
                        HIC_RES_BOHUM1 item = hicResBohum1Service.GetTongBoPanjengDateByWrtno(nWrtno);
                        bOK = false;
                        if (item.IsNullOrEmpty())
                        {
                            if(item.TONGBODATE.IsNullOrEmpty()) { bOK = true; }
                        }
                    }

                    if(bOK == true)
                    {
                        nRow += 1;
                        SSList.ActiveSheet.RowCount = nRow;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE);
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPDATE.Trim();
                        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].JUMIN.Trim();
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].TONGBODATE.To<string>("");
                        SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].WRTNO.ToString();
                        SSList.ActiveSheet.Cells[nRow - 1, 8].Text = hm.UCode_Names_Display(list[i].UCODES);
                        SSList.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Pstr(list[i].BUSENAME, "/", 2);
                        SSList.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].TONGBODATE2.To<string>("");
                        SSList.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].BALDATE.To<string>("");
                        SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].HPHONE.To<string>("");
                        SSList.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].EMAIL.To<string>("");
                        SSList.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].GJYEAR.Trim();

                        strCHK3 = "";
                        strCHK3 = hm.READ_JEPSU_GBCHK3(nWrtno);
                        if(strCHK3 == "OK")
                        {
                           
                        }
                    }
                }
            }
        }

        private void Spread_Print(long argWrtno)
        {
            frmHcPrint_Dental_Sub fHP = new frmHcPrint_Dental_Sub(argWrtno);
            fHP.ShowDialog();
        }
    }
}
