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

namespace HC_Print
{


    /// <summary>
    /// Class Name      : HC_Print
    /// File Name       : frmHcPrint_Spc.cs
    /// Description     : 특수검진 결과지출력
    /// Author          : 김경동
    /// Create Date     : 2021-02-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " Frm검진결과지특수_2019.frm(Frm검진결과지특수_2019)" />



    public partial class frmHcPrint_Spc : Form
    {

        clsHaBase hb = null;
        clsHcMain cHcMain = null;

        HIC_LTD LtdHelpItem = null;

        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicResSpecialService hicResSpecialService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResBohum2Service hicResBohum2Service = null;

        public frmHcPrint_Spc()
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
        }

        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuService = new HicJepsuService();
            hicResSpecialService = new HicResSpecialService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();

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
            else if (sender == btnPrint)
            {
                long nWrtno = 0;
                string strGjjong = "";

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 10].Text);
                        strGjjong = SSList.ActiveSheet.Cells[i, 17].Text;

                        Spread_Print(nWrtno, strGjjong);
                    }
                }

            }




            

        }

        private void Screen_Display(FpSpread Spd)
        {

            bool bOK = false;
            bool bPanRead = false;
            bool bDangnyo = false;

            int nRow = 0;
            long nWrtno = 0;
            long nWrtno2 = 0;
            long nFWRTNO = 0;
            long nPanDrno = 0;

            string strLtdCode = "";
            string strJepDate = "";
            string strPANO = "";
            string strGjjong = "";
            string strGJJONG1 = "";
            string strTongDate = "";
            string strPandate = "";
            string strOk = "";
            string strREC = "";
            string str30Y = "";
            string strSex = "";
            string strSExam = "";
            string strUCODES = "";
            string strSecond_Sayu = "";
            string strDangnyoExam = "";
            string strData = "";
            string strJong = "";
            string str채용구분 = "";
            string strDntSts = "";
            string strCHK3 = "";
            string strSname = "";
            string strRePrint = "";
            string strGubun1 = "";
            string strSort = "";

            strDangnyoExam = hb.READ_HIC_BCODE_Data("HIC_혈압당뇨그룹코드", "");



            if (dtpFDate.Text.IsNullOrEmpty()) { MessageBox.Show("시작일자가 공란입니다."); return; }
            if (dtpTDate.Text.IsNullOrEmpty()) { MessageBox.Show("종료일자가 공란입니다."); return; }

            strLtdCode = txtLtdCode.Text.Trim();
            txtName.Text = txtName.Text.Trim();
            strSname = txtName.Text;

            strGjjong = "";

            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetListByJepdateJong(dtpFDate.Text, dtpTDate.Text, "**", strSname, 0, strRePrint, strGubun1, strSort);



            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {

                    strREC = "";
                    str30Y = "";
                    bDangnyo = false;

                    strPANO = list[i].PANO.ToString();
                    strJepDate = list[i].JEPDATE;
                    nWrtno = list[i].WRTNO;
                    strJong = list[i].GJJONG;
                    strSExam = list[i].SEXAMS;
                    strUCODES = list[i].UCODES;
                    strSex = list[i].SEX;

                    bOK = true;

                    if (strUCODES.IsNullOrEmpty())
                    {
                        switch (strJong)
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "41":
                            case "42":
                            case "43":
                                bOK = true; break;
                            default: break;
                        }
                    }

                    if (bOK = true)
                    {
                        switch (strJong)
                        {
                            case "16":
                            case "17":
                            case "19":
                            case "44":
                                HIC_JEPSU item1 = hicJepsuService.GetItemByPanoGjyearJepdateGjjong(list[i].PANO.ToString(), list[i].GJYEAR, list[i].JEPDATE, "1");

                                bOK = false;
                                if (!item1.UCODES.IsNullOrEmpty()) { bOK = true; }
                                //30년보관 차트를 표시하기 위해 추가함(특수 2차는 취급물질이 등록 안됨)
                                if (strUCODES == "") { strUCODES = item1.UCODES; }
                                break;
                            case "27":
                            case "28":
                            case "29":
                                HIC_JEPSU item2 = hicJepsuService.GetItemByPanoGjyearJepdateGjjong(list[i].PANO.ToString(), list[i].GJYEAR, list[i].JEPDATE, "2");
                                bOK = false;
                                if (!item2.UCODES.IsNullOrEmpty()) { bOK = true; }
                                //30년보관 차트를 표시하기 위해 추가함(특수 2차는 취급물질이 등록 안됨)
                                if (strUCODES == "") { strUCODES = item2.UCODES; }
                                break;
                            default: break;
                        }
                    }

                    //구강검진이 완료 안되도 명단에 표시하며 오류는 인쇄 안함
                    if (bOK = true)
                    {
                        if (list[i].GBDENTAL != "Y")
                        {
                            bOK = true;
                            strDntSts = "X";
                        }
                        else if (Convert.ToInt32(strJong) >= 16 && Convert.ToInt32(strJong) <= 40)
                        {
                            bOK = true;
                            strDntSts = "X";
                        }
                        else if (Convert.ToInt32(strJong) >= 44)
                        {
                            bOK = true;
                            strDntSts = "X";
                        }
                        else
                        {
                            strDntSts = cHcMain.Dental_Status_Check(nWrtno);
                        }
                    }

                    //공무원채용검진(별도 인쇄함)
                    if (bOK = true)
                    {

                        HIC_RES_SPECIAL item3 = hicResSpecialService.GetItemByWrtno(nWrtno);
                        if (!item3.IsNullOrEmpty())
                        {
                            strREC = "OK";
                            if (item3.GBSPC == "10") { bOK = false; }
                        }
                    }

                    //스프레드표시
                    if (bOK = true)
                    {

                        nRow = nRow + 1;
                        if (nRow > SSList.ActiveSheet.RowCount + 1)
                        {
                            SSList.ActiveSheet.RowCount = SSList.ActiveSheet.RowCount + 1;
                        }

                        strTongDate = list[i].TONGBODATE;
                        strPandate = list[i].PANJENGDATE.ToString();
                        nPanDrno = list[i].PANJENGDRNO;

                        //2014-06-10 일반과 특수가 함께 있을 경우 일반판정일을 다시 읽음
                        HIC_RES_BOHUM1 item4 = hicResBohum1Service.GetItemByWrtno(nWrtno);
                        if (!item4.IsNullOrEmpty())
                        {
                            strPandate = item4.PANJENGDATE;
                            strTongDate = item4.TONGBODATE;
                            nPanDrno = item4.PANJENGDRNO;
                        }

                        if (list[i].GJCHASU != "1")
                        {
                            HIC_RES_BOHUM2 item5 = hicResBohum2Service.GetItemByWrtno(nWrtno);
                            if (!item4.IsNullOrEmpty())
                            {
                                strPandate = item5.PANJENGDATE;
                                strTongDate = item5.TONGBODATE;
                                nPanDrno = item5.PANJENGDRNO;
                            }
                        }

                        SSList.ActiveSheet.Cells[nRow, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                        SSList.ActiveSheet.Cells[nRow, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.Trim()); ;
                        SSList.ActiveSheet.Cells[nRow, 4].Text = list[i].JEPDATE.Trim();
                        SSList.ActiveSheet.Cells[nRow, 5].Text = strDntSts;
                        SSList.ActiveSheet.Cells[nRow, 6].Text = list[i].JUMIN.Trim();
                        if (!list[i].WEBPRINTREQ.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[nRow, 7].Text = "Y";
                        }

                        SSList.ActiveSheet.Cells[nRow, 8].Text = VB.Replace(list[i].JUSO, ComNum.VBLF, "");
                        SSList.ActiveSheet.Cells[nRow, 9].Text = cHcMain.UCode_Names_Display(strUCODES);
                        SSList.ActiveSheet.Cells[nRow, 10].Text = nWrtno.ToString();
                        SSList.ActiveSheet.Cells[nRow, 11].Text = strTongDate;
                        SSList.ActiveSheet.Cells[nRow, 12].Text = list[i].SABUN.Trim();
                        SSList.ActiveSheet.Cells[nRow, 13].Text = strTongDate;
                        SSList.ActiveSheet.Cells[nRow, 14].Text = strPandate;
                        SSList.ActiveSheet.Cells[nRow, 17].Text = strJong;

                        strCHK3 = "";

                        if (strCHK3 == "OK")
                        {
                            strGJJONG1 = cHcMain.READ_JEPSU_COUNT(list[i].PTNO, list[i].JEPDATE);
                            if (!strGJJONG1.IsNullOrEmpty())
                            {

                            }
                        }

                        //유해물질_30년보관대상
                        if (cHcMain.READ_UCODES_30Y(strUCODES) == "OK")
                        {
                            //색상표시
                            SSList.ActiveSheet.Cells[nRow, 1].BackColor = Color.Aqua;
                            str30Y = "OK";
                        }

                        SSList.ActiveSheet.Cells[nRow, 20].Text = list[i].MAILCODE.Trim();

                        if (bDangnyo = true)
                        {
                            SSList.ActiveSheet.Cells[nRow, 2].BackColor = Color.Yellow;
                        }
                        else if (list[i].GJCHASU == "2")
                        {
                            SSList.ActiveSheet.Cells[nRow, 2].BackColor = Color.LightSalmon;
                        }
                    }

                    btnPrint.Enabled = true;

                }
            }
        }
        private void Spread_Print(long argWrtno, string argGjjong)
        {
            if ( argGjjong == "11" || argGjjong =="14" || argGjjong =="41")
            {
                frmHcPrint_Bohum1_Sub fHP = new frmHcPrint_Bohum1_Sub(argWrtno);
                fHP.ShowDialog();
            }

            frmHcPrint_Spc_Sub fHP1 = new frmHcPrint_Spc_Sub(argWrtno);
            fHP1.ShowDialog();
        }
    }
}
