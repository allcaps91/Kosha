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



/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Job.cs
/// Description     : 채용(배치전) 결과 통보서
/// Author          : 김경동
/// Create Date     : 2021-07-13
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
/// 

namespace HC_Print
{
    public partial class frmHcPrint_Job : Form
    {

        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuPatientService hicJepsuPatientService = null;

        public frmHcPrint_Job()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetEvents()
        {

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

        }

        private void SetControl()
        {
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuPatientService = new HicJepsuPatientService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtName.Text = "";
            txtLtdCode.Text = "";
            lblLtdName.Text = "";

            SSList.ActiveSheet.Columns[10].Visible = false;

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
                        nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 8].Text);
                        Spread_Print(nWrtno);

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

            long nWrtno = 0;
            string strLtdCode = "";
            string strCHK3 = "";
            string strRePrt = "";
            string strSort = "";

            strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
            txtName.Text = txtName.Text.Trim();
            txtWrtno.Text = txtWrtno.Text.Trim();

            if (ChkGbRe.Checked) { strRePrt = "OK"; }
            
            SSList.ActiveSheet.RowCount = 0;
            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetListByItems(dtpFDate.Text, dtpTDate.Text, txtName.Text, txtWrtno.Text, strLtdCode, strRePrt, strSort, "54");

            if (!list.IsNullOrEmpty())
            {
                SSList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    nWrtno = list[i].WRTNO;

                    SSList.ActiveSheet.Cells[i, 0].Text = "";
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.Trim());
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN;
                    SSList.ActiveSheet.Cells[i, 6].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[i, 8].Text = nWrtno.ToString();
                    SSList.ActiveSheet.Cells[i, 9].Text = cHcMain.UCode_Names_Display(list[i].UCODES);

                    if (hicSunapdtlService.GetCountbyWrtNoCode(nWrtno, "5107") > 0)
                    {
                        SSList.ActiveSheet.Cells[i, 10].Text = "Y";
                    }

                    strCHK3 = "";
                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);
                }
            }
        }

        private void Spread_Print(long argWrtno)
        {
            frmHcPrint_Job_Sub fHP = new frmHcPrint_Job_Sub(argWrtno);
            fHP.ShowDialog();
        }
    }
}
