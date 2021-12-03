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
/// Class Name      : 
/// File Name       : 
/// Description     : 
/// Author          : 김경동
/// Create Date     : 2021-03-23
/// Update History  : 
/// </summary>
namespace ComHpcLibB
{
    public partial class frmPftSomoTong : Form
    {
        clsSpread sp = new clsSpread();

        ComHpcLibBService comHpcLibBService = null;
        HicJepsuResultService hicJepsuResultService = null;

        public frmPftSomoTong()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        private void SetControl()
        {

            comHpcLibBService = new ComHpcLibBService();
            hicJepsuResultService = new HicJepsuResultService();

        }
        private void eFormLoad(object sender, EventArgs e)
        {
            long nYear = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);
            nYear = Convert.ToInt32(VB.Left(DateTime.Now.ToShortDateString(), 4));

            for (int i = 0; i <= 5; i++)
            {
                nYear = Convert.ToInt32(VB.Left(DateTime.Now.ToShortDateString(), 4));
                cboYear.Items.Add(nYear-i);
            }
            cboYear.SelectedIndex = 0;


            //dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            //dtpTDate.Text = DateTime.Now.ToShortDateString();


        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Clear();
                Screen_Display(SSList);
            }
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

        private void Screen_Clear()
        {
            for (int i = 0; i<=12; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    SSList.ActiveSheet.Cells[i, j].Text= "";
                }
            }
        }
        private void Screen_Display(FpSpread Spd)
        {

            int nRow = 0;

            long nCount1 = 0;
            long nCount2 = 0;
            long nCount3 = 0;
            long nCount4 = 0;
            long nCount5 = 0;

            string strFdate = "";
            string strTdate = "";

            strFdate = cboYear.Text + "-01-01";
            strTdate = cboYear.Text + "-12-31";

            //부서코드 101774:신체계측, 044510:종합검진
            List<string> strDEPTCODE = new List<string> { "101774", "044510" };
            //물품코드 PFTFILT1:필터 NOSECLIP:코집계, 
            List<string> strJEPCODE = new List<string> { "PFTFILT1", "NOSECLIP" };

            List < COMHPC> list = comHpcLibBService.GetCountOrdReq(strFdate, strTdate, strDEPTCODE, strJEPCODE);
            if (list.Count >0 )
            {
                for (int i = 0; i <= list.Count-1; i++)
                {
                    nRow = Convert.ToInt32(VB.Right(list[i].REQDATE, 2))-1;

                    if (list[i].DEPTCODE.Trim() == "101774" && list[i].JEPCODE.Trim() == "PFTFILT1")
                    {
                        SSList.ActiveSheet.Cells[nRow, 0].Text = list[i].OQTY.ToString();
                        nCount1 = nCount1 + list[i].OQTY;
                    }
                    else if (list[i].DEPTCODE.Trim() == "044510" && list[i].JEPCODE.Trim() == "PFTFILT1")
                    {
                        SSList.ActiveSheet.Cells[nRow, 1].Text = list[i].OQTY.ToString();
                        nCount2 = nCount2 + list[i].OQTY;
                    }
                    else if (list[i].DEPTCODE.Trim() == "101774" && list[i].JEPCODE.Trim() == "NOSECLIP")
                    {
                        SSList.ActiveSheet.Cells[nRow, 2].Text = list[i].OQTY.ToString();
                        nCount3 = nCount3 + list[i].OQTY;
                    }
                    else if (list[i].DEPTCODE.Trim() == "044510" && list[i].JEPCODE.Trim() == "NOSECLIP")
                    {
                        SSList.ActiveSheet.Cells[nRow, 3].Text = list[i].OQTY.ToString();
                        nCount4 = nCount4 + list[i].OQTY;
                    }
                }

                SSList.ActiveSheet.Cells[12, 0].Text = nCount1.ToString();
                SSList.ActiveSheet.Cells[12, 1].Text = nCount2.ToString();
                SSList.ActiveSheet.Cells[12, 2].Text = nCount3.ToString();
                SSList.ActiveSheet.Cells[12, 3].Text = nCount4.ToString();
            }

            List<HIC_JEPSU_RESULT> list1 = hicJepsuResultService.GetCountJepdateExcodeResult(strFdate, strTdate, "A899", "01");
            if (list1.Count > 0)
            {
                for (int i = 0; i <= list1.Count - 1; i++)
                {
                    nRow = Convert.ToInt32(VB.Right(list1[i].JEPDATE, 2)) - 1;
                    SSList.ActiveSheet.Cells[nRow, 4].Text = list1[i].COUNT.ToString();
                    nCount5 = nCount5 + list1[i].COUNT;
                }
                SSList.ActiveSheet.Cells[12, 4].Text = nCount5.ToString(); 
            }

            MessageBox.Show(cboYear.Text +"년 조회완료!!!", "작업완료");
        }

        private void Spread_Print()
        {

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            strTitle = cboYear.Text + "년 폐활량 물품 청구 및 폐활량 대상자";

            strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.7f);
            sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

    }
}
