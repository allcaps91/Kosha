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
/// File Name       : frmHCPrint_Bohum1.cs
/// Description     : 공단검진 결과지출력
/// Author          : 김경동
/// Create Date     : 2021-01-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Hcbill106.frm(FrmAddPrint)" />
/// 

namespace HC_Print.form
{
    public partial class frmHCPrint_Bohum1 : Form
    {


        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();

        HIC_LTD LtdHelpItem = null;

        HicResBohum1JepsuPatientService hicResBohum1JepsuPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResDentalService hicResDentalService = null;

        public frmHCPrint_Bohum1()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnWebSearch.Click += new EventHandler(eBtnClick);
            this.btnWebPrint.Click += new EventHandler(eBtnClick);
            //this.btnExit.Click += new EventHandler(eBtnClick);

            this.menuExit.Click += new EventHandler(eMenuClick);
            this.menuTongbo.Click += new EventHandler(eMenuClick);

            this.ChkAll.CheckedChanged += new EventHandler(chkChanged);


        }

        private void SetControl()
        {
            hicResBohum1JepsuPatientService = new HicResBohum1JepsuPatientService();
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResDentalService = new HicResDentalService();


        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtName.Text = "";
            txtLtdCode.Text = "";
            lblLtdName.Text = "";

            SSList.ActiveSheet.Columns[11].Visible = false; //취급물질
            SSList.ActiveSheet.Columns[12].Visible = false; //사번
            SSList.ActiveSheet.Columns[16].Visible = false; //메일
            SSList.ActiveSheet.Columns[18].Visible = false; //건진번호
            SSList.ActiveSheet.Columns[19].Visible = false; //
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
            }

            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                //Ltd_Code_Help();
                return;
            }
            #endregion
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
            else if (sender == menuTongbo)
            {

                long nWrtno = 0;
                string strBDate = "";

                strBDate = VB.InputBox("세팅할 통보일자는? (YYYY-MM-DD):", "통보일자강제세팅", clsPublic.GstrSysDate);


                if (!strBDate.IsNullOrEmpty())
                {

                    for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                    {
                        if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 10].Text);

                            //접수
                            hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(nWrtno, clsType.User.IdNumber.To<long>());

                            //1차
                            hicResBohum1Service.UpdateTongBoInfobyWrtNo(nWrtno, strBDate, "1", clsType.User.IdNumber);

                            //구강
                            hicResDentalService.UpdateTongBoInfobyWrtNo(nWrtno, strBDate, "1");

                        }

                        SSList.ActiveSheet.Cells[i, 0].Text = "False";
                    }

                    btnSearch.PerformClick();
                }
            }
        }

        private void Screen_Display(FpSpread Spd)
        {
            long nWrtno = 0;
            string strName = "";
            string strSort = "";
            string strLtdcode = "";
            string strHphone = "";
            string strEmail = "";
            string strDntSts = "";
            string strCHK3 = "";
            string strGjjong = "";
            string strGbRe = "";

            bool bOK = false;



            if (dtpFDate.Text.IsNullOrEmpty()) { MessageBox.Show("시작일자가 공란입니다."); return; }
            if (dtpTDate.Text.IsNullOrEmpty()) { MessageBox.Show("종료일자가 공란입니다."); return; }

            strLtdcode = txtLtdCode.Text.Trim();
            strName = txtName.Text.Trim();
            nWrtno = txtWrtno.Text.To<long>(0);


            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }


            strSort = "1";
            List<HIC_RES_BOHUM1_JEPSU_PATIENT> list = hicResBohum1JepsuPatientService.GetItembyJepdateGubun(dtpFDate.Text, dtpTDate.Text, strName, strLtdcode, strSort, strGbRe, nWrtno, "");

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
                    SSList.ActiveSheet.Cells[i, 5].Text = cHcMain.Dental_Status_Check(nWrtno);
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].JUMIN;
                    if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 7].Text = "Y"; }
                    SSList.ActiveSheet.Cells[i, 8].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[i, 10].Text = nWrtno.ToString();
                    SSList.ActiveSheet.Cells[i, 11].Text = cHcMain.UCode_Names_Display(list[i].UCODES);
                    SSList.ActiveSheet.Cells[i, 12].Text = VB.Pstr(list[i].BUSENAME, "/", 2);
                    SSList.ActiveSheet.Cells[i, 13].Text = list[i].TONGBODATE2;
                    SSList.ActiveSheet.Cells[i, 14].Text = list[i].PANJENGDATE;
                    SSList.ActiveSheet.Cells[i, 15].Text = list[i].HPHONE;
                    SSList.ActiveSheet.Cells[i, 16].Text = list[i].EMAIL;
                    SSList.ActiveSheet.Cells[i, 17].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[i, 18].Text = "";

                    strCHK3 = "";
                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);
                }
            }
        }
        private void Screen_Display_Web(FpSpread Spd)
        {
            long nWrtno = 0;
            string strName = "";
            string strSort = "";
            string strLtdcode = "";
            string strHphone = "";
            string strEmail = "";
            string strDntSts = "";
            string strCHK3 = "";
            string strGjjong = "";
            string strGbRe = "";

            bool bOK = false;



            if (dtpFDate.Text.IsNullOrEmpty()) { MessageBox.Show("시작일자가 공란입니다."); return; }
            if (dtpTDate.Text.IsNullOrEmpty()) { MessageBox.Show("종료일자가 공란입니다."); return; }

            strLtdcode = txtLtdCode.Text.Trim();
            strName = txtName.Text.Trim();



            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }


            strSort = "1";
            List<HIC_RES_BOHUM1_JEPSU_PATIENT> list = hicResBohum1JepsuPatientService.GetItembyJepdateGubun(dtpFDate.Text, dtpTDate.Text, strName, strLtdcode, strSort, strGbRe, nWrtno, "CERT");

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
                    SSList.ActiveSheet.Cells[i, 5].Text = cHcMain.Dental_Status_Check(nWrtno);
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].JUMIN;
                    if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 7].Text = "Y"; }
                    SSList.ActiveSheet.Cells[i, 8].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[i, 10].Text = nWrtno.ToString();
                    SSList.ActiveSheet.Cells[i, 11].Text = cHcMain.UCode_Names_Display(list[i].UCODES);
                    SSList.ActiveSheet.Cells[i, 12].Text = VB.Pstr(list[i].BUSENAME, "/", 2);
                    SSList.ActiveSheet.Cells[i, 13].Text = list[i].TONGBODATE2;
                    SSList.ActiveSheet.Cells[i, 14].Text = list[i].PANJENGDATE;
                    SSList.ActiveSheet.Cells[i, 15].Text = list[i].HPHONE;
                    SSList.ActiveSheet.Cells[i, 16].Text = list[i].EMAIL;
                    SSList.ActiveSheet.Cells[i, 17].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[i, 18].Text = "";

                    strCHK3 = "";
                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);
                }
            }
        }

        private void Spread_Print(long argWrtno)
        {
            frmHcPrint_Bohum1_Sub fHP = new frmHcPrint_Bohum1_Sub(argWrtno);
            fHP.ShowDialog();
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
