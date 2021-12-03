using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_BalDate : Form
    {

        clsHaBase hb = new clsHaBase();
        clsSpread sp = new clsSpread();
        clsHcFunc cHF = new clsHcFunc();

        HicJepsuResBohum2Service hicJepsuResBohum2Service = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResSpecialService hicResSpecialService = null;
        HicCancerNewService hicCancerNewService = null;



        string strFDate = "";
        string strTDate = "";


        public frmHcPrint_BalDate()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicJepsuResBohum2Service = new HicJepsuResBohum2Service();
            comHpcLibBService = new ComHpcLibBService();
            hicResSpecialService = new HicResSpecialService();
            hicCancerNewService = new HicCancerNewService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnPrint2.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.AddDays(-2).ToShortDateString();

            cboJong.Items.Clear();
            cboJong.Items.Add("*.전체");
            cboJong.Items.Add("1.1차검진");
            cboJong.Items.Add("2.2차검진");
            cboJong.Items.Add("3.특수검진");
            cboJong.Items.Add("4.구강검진");
            cboJong.Items.Add("5.암검진");
            cboJong.Items.Add("6.학생검진");
            cboJong.Items.Add("7.기타검진");
            cboJong.SelectedIndex = 1;

            cboJob.Items.Clear();
            cboJob.Items.Add("*.전체");
            cboJob.Items.Add("1.7일이내 미발송");
            cboJob.Items.Add("2.미판정");
            cboJob.Items.Add("3.미발송");
            cboJob.SelectedIndex = 1;

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);

            }
            else if (sender == btnPrint2)
            {
                
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);
                sp.setSpdPrint(SSList2, PrePrint, setMargin, setOption, strHeader, strFooter);


            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }

        }

        private void Screen_Display(FpSpread Spd)
        {

            ComFunc CF = null;
            CF = new ComFunc();


            double[] nTotCnt = new double[10];

            int nRow = 0;
            int nRow2 = 0;
            long nRate = 0;
            long nIlsu1 = 0;
            long nIlsu2 = 0;
            long nIlsu3 = 0; 
            long nDrNo = 0;
            long nPrtSabun = 0;

            string strNEW = "";
            string strOLD = "";
            string strJong = "";
            string strJob = "";
            string strOK = "";
            string strJepDate = "";
            string strPanDate = "";
            string strPrtDate = "";


            string strFDate = dtpFDate.Value.ToShortDateString();
            string strTDate = dtpTDate.Value.ToShortDateString();

            strJong = VB.Left(cboJong.Text, 1);
            strJob = VB.Left(cboJob.Text, 1);
            
            if (strJob != "*")
            {
                if (Convert.ToInt32(strJob) >= 2 && chkTongView.Checked == true)
                {
                    MessageBox.Show("월병 통계만 조회 기능은 전체 또는 7일경과만 가능합니다.!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            for (int i = 1; i<= 9; i++)
            {
                nTotCnt[i] = 0;
            }

            SSList.ActiveSheet.RowCount = 0;
            List<COMHPC> list = comHpcLibBService.GetItembyBalDate(strFDate, strTDate, strJong, strJob);
            if(!list.IsNullOrEmpty())
            {

                for (int i = 0; i < list.Count; i++)
                {
                    strJepDate = list[i].JEPDATE;
                    strPanDate = list[i].PANJENGDATE;
                    strPrtDate = Convert.ToDateTime(list[i].TONGBODATE.ToString()).ToShortDateString();

                    nDrNo = list[i].PANJENGDRNO;
                    nPrtSabun = list[i].PRTSABUN;

                    //일특 판정일을 읽음
                    if (list[i].GJBUN == "1")
                    {
                        if(!list[i].UCODES.IsNullOrEmpty())
                        {
                            HIC_RES_SPECIAL item = hicResSpecialService.GetPanTongDateByWrtno(list[i].WRTNO);
                            if (!item.IsNullOrEmpty())
                            {
                                strPanDate = item.PANJENGDATE;
                                strPrtDate = item.TONGBODATE;
                                nDrNo = item.PANJENGDRNO;
                                nDrNo = item.PRTSABUN;
                            }
                        }
                    }
                    //특수2차만 있는 경우

                    if (list[i].GJBUN == "2")
                    {
                        if (!list[i].UCODES.IsNullOrEmpty())
                        {
                            HIC_RES_SPECIAL item = hicResSpecialService.GetPanTongDateByWrtno(list[i].WRTNO);
                            if (!item.IsNullOrEmpty())
                            {
                                strPanDate = item.PANJENGDATE;
                                strPrtDate = item.TONGBODATE;
                                nDrNo = item.PANJENGDRNO;
                                nDrNo = item.PRTSABUN;
                            }
                        }
                    }

                    if (list[i].GJBUN == "5")
                    {
                        HIC_CANCER_NEW item = hicCancerNewService.GetPanjengDateByWrtno(list[i].WRTNO);
                        if(!item.IsNullOrEmpty())
                        {
                            if (item.S_PANJENGDATE.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.S_PANJENGDATE; }
                            if (item.C_PANJENGDATE.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.C_PANJENGDATE; }
                            if (item.L_PANJENGDATE.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.L_PANJENGDATE; }
                            if (item.B_PANJENGDATE.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.B_PANJENGDATE; }
                            if (item.W_PANJENGDATE.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.W_PANJENGDATE; }
                            if (item.L_PANJENGDATE1.To<DateTime>() > strPanDate.To<DateTime>()) { strPanDate = item.L_PANJENGDATE1; }
                        }
                    }

                    nIlsu1 = 0; nIlsu2 = 0; nIlsu3 = 0;
                    if (!strJepDate.IsNullOrEmpty() && !strPanDate.IsNullOrEmpty())
                    {
                        //nIlsu1 = CF.DATE_ILSU(clsDB.DbCon, strPanDate, strJepDate) + 1;
                        nIlsu1 = cHF.DATE_ILSU_YOIL(strJepDate, strPanDate);
                    }
                    if (!strJepDate.IsNullOrEmpty() && !strPrtDate.IsNullOrEmpty())
                    {
                        //nIlsu2 = CF.DATE_ILSU(clsDB.DbCon, strPrtDate, strJepDate) + 1;
                        nIlsu2 = cHF.DATE_ILSU_YOIL(strJepDate, strPrtDate);
                    }

                    if (!strPanDate.IsNullOrEmpty() && !strPrtDate.IsNullOrEmpty())
                    {
                        nIlsu3 = cHF.DATE_ILSU_YOIL(strPanDate, strPrtDate);
                    }


                    strOK = "OK";

                    if (strJob == "1" && nIlsu2 <= 7) { strOK = ""; }
                    if (strJob == "2" && !strPanDate.IsNullOrEmpty()) { strOK = ""; }   //미판정
                    if (strJob == "3" && !strPrtDate.IsNullOrEmpty()) { strOK = ""; }   //미인쇄

                    if(strJob== "*" || strJob == "1")
                    {

                        if (chkTongView.Checked) { strOK = ""; }

                        strNEW = VB.Left(strJepDate, 7);
                        if (strOLD.IsNullOrEmpty()) { strOLD = strNEW; }
                        if (strOLD != strNEW)
                        {
                            nRow2 += 1;
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 0].Text = strOLD;
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 1].Text = nTotCnt[1].ToString();
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 2].Text = nTotCnt[2].ToString();
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 3].Text = "";
                            if (nTotCnt[1] > 0 && nTotCnt[2] > 0)
                            {
                                nRate = CF.FIX_N(nTotCnt[2] / nTotCnt[1] * 100);
                                SSList2.ActiveSheet.Cells[nRow2 - 1, 3].Text = nRate.ToString();
                            }

                            SSList2.ActiveSheet.Cells[nRow2 - 1, 4].Text = nTotCnt[3].ToString();
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 5].Text = "";
                            if (nTotCnt[1] > 0 && nTotCnt[3] > 0)
                            {
                                nRate = CF.FIX_N(nTotCnt[3] / nTotCnt[1] * 100);
                                SSList2.ActiveSheet.Cells[nRow2 - 1, 5].Text = nRate.ToString();
                            }

                            SSList2.ActiveSheet.Cells[nRow2 - 1, 6].Text = nTotCnt[4].ToString();
                            SSList2.ActiveSheet.Cells[nRow2 - 1, 7].Text = "";
                            if (nTotCnt[1] > 0 && nTotCnt[4] > 0)
                            {
                                nRate = CF.FIX_N(nTotCnt[4] / nTotCnt[1] * 100);
                                SSList2.ActiveSheet.Cells[nRow2 - 1, 7].Text = nRate.ToString();
                            }

                            SSList2.ActiveSheet.Cells[nRow2 - 1, 8].Text = "";
                            if (nTotCnt[5] > 0 && nTotCnt[6] > 0)
                            {
                                SSList2.ActiveSheet.Cells[nRow2 - 1, 8].Text = VB.Format(nTotCnt[6] / nTotCnt[5], "#0.0") + "일";
                            }

                            SSList2.ActiveSheet.Cells[nRow2 - 1, 9].Text = "";
                            if (nTotCnt[1] > 0 && nTotCnt[2] > 0)
                            {
                                SSList2.ActiveSheet.Cells[nRow2 - 1, 9].Text = VB.Format(nTotCnt[8] / nTotCnt[7], "#0.0") + "일";
                            }


                            strOLD = strNEW;
                            for (int j = 1; j < 10; j++) 
                            {
                                nTotCnt[j] = 0;
                            }

                        }

                        //인원집계
                        nTotCnt[1] += 1;                    //검진인원
                        if (strPrtDate.IsNullOrEmpty())
                        {
                            nTotCnt[4] += 1;                //미발송
                        }
                        else
                        {
                            if(nIlsu2<= 7)
                            {
                                nTotCnt[2] += 1;            //정상발송
                            }
                            else
                            {
                                nTotCnt[3] += 1;            //기간초과

                            }
                        }

                        //평균판독일
                        if(!strPanDate.IsNullOrEmpty())
                        {
                            nTotCnt[5] += 1;                //판독전수
                            nTotCnt[6] += nIlsu1;
                        }
                        //평균통보일
                        if (!strPrtDate.IsNullOrEmpty())
                        {
                            nTotCnt[7] += 1;                //통보건수
                            nTotCnt[8] += nIlsu2;
                            nTotCnt[9] += nIlsu3;           //
                        }



                    }

                    if (strOK == "OK")
                    {
                        SSList.ActiveSheet.RowCount = SSList.ActiveSheet.RowCount + 1;
                        
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 0].Text = strJepDate;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 1].Text = list[i].WRTNO.ToString();
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 2].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 3].Text = list[i].AGE + "/" + list[i].SEX;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 4].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 5].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 6].Text = list[i].GBSTS;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 7].Text = VB.Replace(hb.READ_License_DrName(list[i].PANJENGDRNO)," ","");
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 8].Text = strPanDate;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 9].Text = nIlsu1.ToString();
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 10].Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list[i].PRTSABUN.ToString());
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount-  1, 11].Text = list[i].TONGBODATE.ToString() ;
                        SSList.ActiveSheet.Cells[SSList.ActiveSheet.RowCount - 1, 12].Text = nIlsu2.ToString();
                    }
                }

                //월통계를 표시함
                if (strJob == "*" || strJob == "1")
                {

                    nRow2 += 1;
                    SSList2.ActiveSheet.RowCount = nRow2;
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 0].Text = strOLD;
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 1].Text = nTotCnt[1].ToString();
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 2].Text = nTotCnt[2].ToString();
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 3].Text = "";
                    if (nTotCnt[1] > 0 && nTotCnt[2] > 0)
                    {
                        nRate = CF.FIX_N(nTotCnt[2] / nTotCnt[1] * 100);
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 3].Text = nRate.ToString();
                    }

                    SSList2.ActiveSheet.Cells[nRow2 - 1, 4].Text = nTotCnt[3].ToString();
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 5].Text = "";
                    if (nTotCnt[1] > 0 && nTotCnt[3] > 0)
                    {
                        nRate = CF.FIX_N(nTotCnt[3] / nTotCnt[1] * 100);
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 5].Text = nRate.ToString();
                    }

                    SSList2.ActiveSheet.Cells[nRow2 - 1, 6].Text = nTotCnt[4].ToString();
                    SSList2.ActiveSheet.Cells[nRow2 - 1, 7].Text = "";
                    if (nTotCnt[1] > 0 && nTotCnt[4] > 0)
                    {
                        nRate = CF.FIX_N(nTotCnt[4] / nTotCnt[1] * 100);
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 7].Text = nRate.ToString();
                    }

                    SSList2.ActiveSheet.Cells[nRow2 - 1, 8].Text = "";
                    if (nTotCnt[5] > 0 && nTotCnt[6] > 0)
                    {
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 8].Text = VB.Format(nTotCnt[6] / nTotCnt[5], "#0.0") + "일";
                    }

                    SSList2.ActiveSheet.Cells[nRow2 - 1, 9].Text = "";
                    if (nTotCnt[7] > 0 && nTotCnt[8] > 0)
                    {
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 9].Text = VB.Format(nTotCnt[8] / nTotCnt[7], "#0.0") + "일";
                    }

                    SSList2.ActiveSheet.Cells[nRow2 - 1, 10].Text = "";
                    if (nTotCnt[7] > 0 && nTotCnt[9] > 0)
                    {
                        SSList2.ActiveSheet.Cells[nRow2 - 1, 10].Text = VB.Format(nTotCnt[9] / nTotCnt[7], "#0.0") + "일";
                    }


                }
            }
        }
    }
}
