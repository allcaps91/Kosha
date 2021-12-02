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
using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_JinDan_Main.cs
/// Description     : 추가검진 결과지출력
/// Author          : 김경동
/// Create Date     : 2020-07-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " HcPrint10_1.frm(FrmJinDan_New)" />
/// 
namespace HC_Print
{
    public partial class frmHcPrint_JinDan_Main : Form
    {
        long fnWrtno = 0;
        string fstrJepdate = "";
        string fstrJinGbn = "";
        

        #region Declare Variable Area
        HIC_LTD LtdHelpItem = null;
        #endregion

        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();

        HicJepsuPatientJinGbdService hicJepsuPatientJinGbdService = null;
        HicJinGbnService hicJinGbnService = null;


        public frmHcPrint_JinDan_Main()
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
            this.btnExit.Click += new EventHandler(eBtnClick);

        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();

            hicJepsuPatientJinGbdService = new HicJepsuPatientJinGbdService();
            hicJinGbnService = new HicJinGbnService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            txtSname.Text = "";
            btnPrint.Enabled = false;
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
                Ltd_Code_Help();
                return;
            }
            #endregion
            else if (sender == btnPrint)
            {
                Spread_Print();
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {
            int nRead = 0;
            long nLtdCode = 0;
            string strCHK3 = "";
            string strSName = "";
            string strGbRe = "";



            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }


            //nLtdCode = txtLtdCode.Text.To<long>();
            nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

            List<HIC_JEPSU_PATIENT_JIN_GBD> list = hicJepsuPatientJinGbdService.GetItembyJepDate(dtpFDate.Text, dtpTDate.Text, nLtdCode, strSName, strGbRe);


            nRead = list.Count;
            SSList.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE.Trim();
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE + " / "+ list[i].SEX;
                SSList.ActiveSheet.Cells[i, 6].Text = list[i].WRTNO.ToString();
                SSList.ActiveSheet.Cells[i, 7].Text = list[i].GUBUN.Trim();
                SSList.ActiveSheet.Cells[i, 8].Text = hb.READ_HIC_CODE("J1", list[i].GUBUN.Trim());
                SSList.ActiveSheet.Cells[i, 9].Text = list[i].PANJENGDRNO.ToString();


                strCHK3 = hm.READ_JEPSU_GBCHK3(list[i].WRTNO);
                if (strCHK3 == "OK")
                {
                    SSList.ActiveSheet.Rows[i].BackColor = Color.Aqua;
                }
            }


        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdCode.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdCode.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdCode.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtLtdCode.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                if (VB.Pstr(txtLtdCode.Text, ",", 1).Trim() == "")
                {
                    txtLtdCode.Text = "";
                }
            }
        }

        private void Spread_Print()
        {
            long nPrtCNT = 0;
            string strChk = "";

            for (int i = 0; i <= SSList.ActiveSheet.RowCount; i++)
            {
                strChk = SSList.ActiveSheet.Cells[i, 0].Text;

                if (strChk == "True")
                {
                    fnWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 6].Text);
                    fstrJepdate = SSList.ActiveSheet.Cells[i, 4].Text;

                    if (rdoGubun1.Checked == true)
                    {
                        nPrtCNT = nPrtCNT + 1;
                        Result_Print_Main();

                    }
                    else if (rdoGubun2.Checked == true && strChk == "True")
                    {
                        nPrtCNT = nPrtCNT + 1;
                        Result_Print_Main();
                        SSList.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    else if (rdoGubun3.Checked == true && strChk == "")
                    {
                        nPrtCNT = nPrtCNT + 1;
                        Result_Print_Main();
                        SSList.ActiveSheet.Cells[i, 0].Text = "True";
                    }
                }
            }
        }

        private void Result_Print_Main()
        {

            string strOK = "";

            frmHcPrint_Jindan_Sub fHPP = new frmHcPrint_Jindan_Sub(fnWrtno);
            fHPP.ShowDialog();

            //인쇄여부확인(Result_Print_Main_1)
            HIC_JIN_GBN item = hicJinGbnService.GetItemByWrtno(fnWrtno);
            if (item.GBPRINT == "Y") { strOK = "OK"; }

            if (strOK == "OK")
            {
                HIC_JIN_GBN item1 = new HIC_JIN_GBN();
                item1.WRTNO = fnWrtno;

                hicJinGbnService.PrintUpdate(item1);
            }
        }


    }
}
