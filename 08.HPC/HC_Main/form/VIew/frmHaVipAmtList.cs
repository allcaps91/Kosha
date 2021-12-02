using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaVipAmtList.cs
/// Description     : 종검 고액수검자 명단
/// Author          : 김민철
/// Create Date     : 2020-03-24
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm고액종검명단(Frm고액종검명단.frm)" />
namespace HC_Main
{
    public partial class frmHaVipAmtList : Form
    {
        clsSpread cSpd = null;
        clsHaBase cHB = null;
        HeaJepsuService heaJepsuService = null;
        HeaSunapService heaSunapService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;

        public frmHaVipAmtList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cHB = new clsHaBase();
            heaJepsuService = new HeaJepsuService();
            heaSunapService = new HeaSunapService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();

            cboAmt.Items.Clear();
            cboAmt.Items.Add("*.100만원이상");
            cboAmt.Items.Add("1.100~149만원");
            cboAmt.Items.Add("2.150~199만원");
            cboAmt.Items.Add("3.200만원이상");
            cboAmt.SelectedIndex = 1;

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong2_AddItem(cboJong);
            cboJong.SelectedIndex = 1;
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                Screen_Display();
            }
        }

        private void Screen_Display()
        {
            string strJong = VB.Pstr(cboJong.Text, ".", 1).Trim();
            string strAmt = VB.Left(cboAmt.Text, 1).Trim();
            string strFDate = dtpFDate.Text.Trim();
            string strTDate = dtpTDate.Text.Trim();
            string strOK = string.Empty;
            string strName = string.Empty;
            string strYName = string.Empty;

            long nWRTNO = 0;
            long nAMT = 0;

            int nRow = 0;

            cSpd.Spread_Clear_Simple(SSList);

            IList<HEA_JEPSU> list = heaJepsuService.GetListByItems(strFDate, strTDate, "접수", strJong, "", 0);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    nWRTNO = list[i].WRTNO;

                    strOK = "";
                    //최종 총진료비를 읽음
                    nAMT = heaSunapService.GetSunapAmtByWrtno(nWRTNO);
                    if (nAMT >= 1000000)
                    {
                        switch (VB.Left(cboAmt.Text, 1))
                        {
                            case "*": strOK = "OK"; break;
                            case "1": if (nAMT >= 1000000 && nAMT <= 1499999) { strOK = "OK"; } break;
                            case "2": if (nAMT >= 1500000 && nAMT <= 1999999) { strOK = "OK"; } break;
                            case "3": if (nAMT >= 2000000) { strOK = "OK"; } break;
                            default: break;
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        if (nRow > SSList.ActiveSheet.RowCount)
                        {
                            SSList.ActiveSheet.RowCount = nRow;

                            SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SDATE.Trim();
                            SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME.Trim();
                            SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].AGE.To<string>("") + "/" + list[i].SEX.Trim();
                            SSList.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                            SSList.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Format(nAMT, "#,##0");
                            SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].LTDNAME.To<string>("").Trim();

                            //2016-07-20 명품검진,골드검진,VIP검진 표시
                            strName = "표준형";
                            if (list[i].GJJONG.Trim() == "11" || list[i].GJJONG.Trim() == "12")
                            {
                                HIC_SUNAPDTL_GROUPCODE item = hicSunapdtlGroupcodeService.GetYNameByWrtno(nWRTNO);
                                if (!item.IsNullOrEmpty())
                                {
                                    strName = item.YNAME.Trim();
                                    if (strName.IndexOf("플러스검진") >= 0) { strName = "플러스검진"; }
                                    if (strName.IndexOf("골드검진") >= 0) { strName = "골드검진"; }
                                    if (strName.IndexOf("VIP검진") >= 0) { strName = "VIP검진"; }
                                    if (strName.IndexOf("명품검진") >= 0) { strName = "명품검진"; }
                                }
                            }

                            if (list[i].LTDCODE > 0 && strName == "표준형") { strName = "회사검진"; }

                            SSList.ActiveSheet.Cells[nRow - 1, 3].Text = strName;
                        }
                    }
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpFDate.Text = DateTime.Now.ToShortDateString();

            cSpd.Spread_Clear_Simple(SSList);
        }
    }
}
