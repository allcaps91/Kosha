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
/// Description     :  
/// /// Author          : 김경동
/// Create Date     : 2021-06-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Hcbill106.frm(FrmAddPrint)" />
/// 
namespace HC_Print
{
    public partial class frmHicXrayPrint : Form
    {

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();

        HIC_LTD LtdHelpItem = null;
        frmHcLtdHelp FrmHcLtdHelp = null;

        HicResBohum1JepsuPatientService hicResBohum1JepsuPatientService = null;
        HicXrayResultService hicXrayResultService = null;

        public frmHicXrayPrint()
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

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.SSList2.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);

        }

        private void SetControl()
        {
            hicXrayResultService = new HicXrayResultService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-10).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtSname.Text = "";
            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            
            btnSearch.Enabled = true;
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
                string strFDate = "";
                string strTDate = "";
                string strGubun = "";

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {

                        HIC_XRAY_RESULT HXR = new HIC_XRAY_RESULT
                        {

                            GJJONG = SSList.ActiveSheet.Cells[i, 2].Text,
                            READDATE = Convert.ToDateTime(SSList.ActiveSheet.Cells[i, 3].Text),
                            MINDATE = SSList.ActiveSheet.Cells[i, 7].Text,
                            MAXDATE = SSList.ActiveSheet.Cells[i, 8].Text,
                            LTDCODE = SSList.ActiveSheet.Cells[i, 1].Text.To<long>(0),
                            READDOCT1 = SSList.ActiveSheet.Cells[i, 10].Text.To<long>(0),
                            READDOCT2 = SSList.ActiveSheet.Cells[i, 11].Text.To<long>(0),
                            GBREAD = SSList.ActiveSheet.Cells[i, 12].Text
                        };

                        strFDate = dtpFDate.Text;
                        strFDate = dtpFDate.Text;
                        if (ChkGbRe.Checked)
                        {
                            strGubun = "Y";
                        }

                        Spread_Print(HXR, strFDate, strTDate, strGubun);
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
            string strSname = "";
            string strGubun = "";

            strSname = txtSname.Text.Trim();


            if (ChkGbRe.Checked)
            {
                strGubun = "Y";
            }

            List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByJepDateLtdCodeGubun(dtpFDate.Text, dtpTDate.Text, txtLtdCode.Text.To<long>(0), strGubun, strSname);

            if (!list.IsNullOrEmpty())
            {

                SSList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                    SSList.ActiveSheet.Cells[i, 3].Text = VB.Format(list[i].READDATE, "yyyy-MM-dd");
                    SSList.ActiveSheet.Cells[i, 4].Text = cf.READ_PassName(clsDB.DbCon, list[i].READDOCT1.ToString());
                    SSList.ActiveSheet.Cells[i, 5].Text = cf.READ_PassName(clsDB.DbCon, list[i].READDOCT2.ToString());
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].CNT.ToString();
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].MINDATE;
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].MAXDATE;
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].LTDCODE.ToString();
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].READDOCT1.ToString();
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].READDOCT2.ToString();
                    SSList.ActiveSheet.Cells[i, 12].Text = list[i].GBREAD;
                    if (list[i].GBREAD == "2")
                    {
                        SSList.ActiveSheet.Cells[i, 13].Text = "◎";
                    }
                    else
                    {
                        SSList.ActiveSheet.Cells[i, 13].Text = "";
                    }
                }

                btnExit.Enabled = true;
            }

        }


        private void Spread_Print(HIC_XRAY_RESULT HXR, string strFDate, string strTDate, string strGubun)
        {
            frmHicXrayPrint_Sub fHP = new frmHicXrayPrint_Sub(HXR, strFDate, strTDate, strGubun);
            fHP.ShowDialog();
        }
        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {

            if (sender == SSList)
            {
                long nLtdCode = 0;
                long nDoct1 = 0;
                long nDoct2 = 0;

                string strReadDate1 = "";
                string strReadDate2 = "";
                string strJepDate1 = "";
                string strJepDate2 = "";
                string strGbRead = "";
                
                string strJong = "";
               
                string strGubun = "";

                if (ChkGbRe.Checked)
                {
                    strGubun = "Y";
                }

                strJong = SSList.ActiveSheet.Cells[e.Row,2].Text;
                strReadDate1 = SSList.ActiveSheet.Cells[e.Row, 3].Text;
                strJepDate1 = SSList.ActiveSheet.Cells[e.Row, 7].Text;
                strJepDate2 = SSList.ActiveSheet.Cells[e.Row, 8].Text;
                nLtdCode = SSList.ActiveSheet.Cells[e.Row, 9].Text.To<long>(0);
                nDoct1 = SSList.ActiveSheet.Cells[e.Row, 10].Text.To<long>(0);
                nDoct2 = SSList.ActiveSheet.Cells[e.Row, 11].Text.To<long>(0);
                strGbRead = SSList.ActiveSheet.Cells[e.Row, 12].Text;

                List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByJepDateLtdCodeDocJongGubun(strJepDate1, strJepDate2, nLtdCode, strGbRead, strJong, nDoct1, nDoct2, strGubun, strReadDate1);
                if (!list.IsNullOrEmpty())
                {
                    SSList2.ActiveSheet.RowCount = list.Count;
                    for ( int i = 0; i< list.Count; i++)
                    {
                        SSList2.ActiveSheet.Cells[i, 0].Text = list[i].SNAME.Trim();
                        SSList2.ActiveSheet.Cells[i, 1].Text = list[i].PANO.ToString().Trim();
                        SSList2.ActiveSheet.Cells[i, 2].Text = list[i].XRAYNO.Trim() ;
                        SSList2.ActiveSheet.Cells[i, 3].Text = list[i].SEX.Trim()+ "/" + list[i].AGE.Trim();
                        SSList2.ActiveSheet.Cells[i, 4].Text = hb.READ_GjJong_Name(strJong);
                        SSList2.ActiveSheet.Cells[i, 5].Text = cf.Read_Ltd_Name(clsDB.DbCon, nLtdCode.ToString());
                        SSList2.ActiveSheet.Cells[i, 6].Text = list[i].GBCHUL;
                        SSList2.ActiveSheet.Cells[i, 7].Text = list[i].PTNO;

                    }
                }
            }
            else if (sender ==SSList2)
            {
                string strXrayno = "";

                strXrayno = SSList2.ActiveSheet.Cells[e.Row, 2].Text;
                if(ChkGbRe.Checked)
                {
                    if (MessageBox.Show("인쇄한것을 취소하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int result = hicXrayResultService.UpdateGbPrintByXrayNo(strXrayno, "");

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }
            }
        }
    }

}
