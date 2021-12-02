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
/// File Name       : frmHcPrint_Cancer.cs
/// Description     : 암검진 결과지출력
/// Author          : 김경동
/// Create Date     : 2021-01-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Hcbill106.frm(FrmAddPrint)" />
/// 

namespace HC_Print
{
    public partial class frmHcPrint_Cancer : Form
    {
        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();

        HIC_LTD LtdHelpItem = null;

        HicCancerNewJepsuPatientService hicCancerNewJepsuPatientService = null;


        public frmHcPrint_Cancer()
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
            this.btnWebSearch.Click += new EventHandler(eBtnClick);
            this.btnWebPrint.Click += new EventHandler(eBtnClick);
            //this.btnExit.Click += new EventHandler(eBtnClick);

            this.ChkAll.CheckedChanged += new EventHandler(chkChanged);
        }


        private void SetControl()
        {

            hicCancerNewJepsuPatientService = new HicCancerNewJepsuPatientService();


        }




        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            //txtName.Text = "";
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
            else if (sender == btnWebSearch)
            {
                Screen_Display_Web(SSList);
                btnWebPrint.Enabled = true;
            }
            else if (sender == btnWebPrint)
            {
                //Spread_Print();
            }

        }

        private void Spread_Print(long argWrtno)
        {
            frmHcPrint_Cancer_Sub fHP = new frmHcPrint_Cancer_Sub(argWrtno);
            fHP.ShowDialog();
        }


        private void Screen_Display(FpSpread Spd)
        {

            long nWrtno = 0;
            string strName = "";
            string strLtdcode = "";
            string strCHK3 = "";
            string strGjjong = "";
            string strSort = "";
            string strGbRe = "";

            if (dtpFDate.Text.IsNullOrEmpty()) { MessageBox.Show("시작일자가 공란입니다."); return; }
            if (dtpTDate.Text.IsNullOrEmpty()) { MessageBox.Show("종료일자가 공란입니다."); return; }

            strLtdcode = txtLtdCode.Text.Trim();
            strName = txtName.Text.Trim();
            nWrtno = txtName.Text.To<long>(0);

            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }

            List<HIC_CANCER_NEW_JEPSU_PATIENT> list = hicCancerNewJepsuPatientService.GetItembyJepdateGubun(dtpFDate.Text, dtpTDate.Text, strName, strLtdcode, strSort, strGbRe, nWrtno, "");

            SSList.ActiveSheet.RowCount = 0;
            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                SSList.ActiveSheet.RowCount = list.Count; 

                for (int i = 0; i < list.Count; i++)
                {
                    nWrtno = list[i].WRTNO;

                    SSList.ActiveSheet.Cells[i, 0].Text = "";
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;
                    //SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString() + "/" + list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN;
                    if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 6].Text = "Y"; }
                    SSList.ActiveSheet.Cells[i, 7].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                    SSList.ActiveSheet.Cells[i, 8].Text = cHcMain.UCode_Names_Display(list[i].UCODES);
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].TONGBODATE2;
                    SSList.ActiveSheet.Cells[i, 12].Text = list[i].BALDATE;
                    SSList.ActiveSheet.Cells[i, 13].Text = list[i].HPHONE;
                    SSList.ActiveSheet.Cells[i, 14].Text = list[i].EMAIL;

                    strCHK3 = "";
                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);

                }
            }
        }

        private void Screen_Display_Web(FpSpread Spd)
        {

            long nWrtno = 0;
            string strName = "";
            string strLtdcode = "";
            string strCHK3 = "";
            string strGjjong = "";
            string strSort = "";
            string strGbRe = "";

            if (dtpFDate.Text.IsNullOrEmpty()) { MessageBox.Show("시작일자가 공란입니다."); return; }
            if (dtpTDate.Text.IsNullOrEmpty()) { MessageBox.Show("종료일자가 공란입니다."); return; }

            strLtdcode = txtLtdCode.Text.Trim();
            strName = txtName.Text.Trim();
            nWrtno = txtName.Text.To<long>(0);

            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }

            List<HIC_CANCER_NEW_JEPSU_PATIENT> list = hicCancerNewJepsuPatientService.GetItembyJepdateGubun(dtpFDate.Text, dtpTDate.Text, strName, strLtdcode, strSort, strGbRe, nWrtno, "CERT");
            SSList.ActiveSheet.RowCount = 0;

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                SSList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    nWrtno = list[i].WRTNO;

                    SSList.ActiveSheet.Cells[i, 0].Text = "";
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                    SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;
                    //SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString() + "/" + list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN;
                    if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 6].Text = "Y"; }
                    SSList.ActiveSheet.Cells[i, 7].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                    SSList.ActiveSheet.Cells[i, 8].Text = cHcMain.UCode_Names_Display(list[i].UCODES);
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].TONGBODATE2;
                    SSList.ActiveSheet.Cells[i, 12].Text = list[i].BALDATE;
                    SSList.ActiveSheet.Cells[i, 13].Text = list[i].HPHONE;
                    SSList.ActiveSheet.Cells[i, 14].Text = list[i].EMAIL;

                    strCHK3 = "";
                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);

                }
            }
        }


        private void chkChanged(object sender, EventArgs e)
        {
            int nCount = 0;

            if (sender == ChkAll)
            {
                nCount = SSList.ActiveSheet.RowCount;
                if (nCount > 0)
                {
                    SSList.ActiveSheet.Cells[0, 0, nCount - 1, 0].Text = ChkAll.Checked.ToString();
                }
            }
        }

    }
}
