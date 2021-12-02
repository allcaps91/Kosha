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


namespace HC_Tong
{
    public partial class frmHaHanWol : Form
    {

        clsHaBase hb = new clsHaBase();

        HeaJepsuPatientService heaJepsuPatientService = null;
        HeaResultService heaResultService = null;


        public frmHaHanWol()
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
            heaJepsuPatientService = new HeaJepsuPatientService();
            heaResultService = new HeaResultService();
        }


        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-10).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

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

        }

        private void Screen_Display(FpSpread Spd)
        {

            string strOK1 = "";
            string strOK2 = "";
            string strOK3 = "";
            string strOK4 = "";
            string strOK5 = "";

            long nCount1 = 0;   //골밀도
            long nCount2 = 0;   //혈압
            long nCount3 = 0;   //당뇨
            long nCount4 = 0;   //간
            long nCount5 = 0;   //갑상선

            List<string> strCodeList = new List<string>();

            strCodeList.Clear();
            
            strCodeList.Add("A108");    //혈압-최고
            strCodeList.Add("A109");    //혈압-최저
            strCodeList.Add("A122");    //공복혈당(당뇨)
            strCodeList.Add("A124");    //간
            strCodeList.Add("A125");    //간
            strCodeList.Add("A126");    //간
            strCodeList.Add("LH42");    //갑상선
            strCodeList.Add("LH43");    //갑상선
            strCodeList.Add("LH44");    //갑상선
            strCodeList.Add("TX82");    //골밀도

            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItemByHanwol(dtpFDate.Text, dtpTDate.Text, 2199);
            
            if (!list.IsNullOrEmpty())
            {

                SSList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    //SSList.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].JUMIN;
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].SDATE;
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].AGE + "/" + list[i].SEX;

                    strOK1 = ""; strOK2 = ""; strOK3 = ""; strOK4 = ""; strOK5 = "";

                    //검사결과 카운트
                    List<HEA_RESULT> list1 = heaResultService.GetExCodebyWrtNo_All(list[i].WRTNO, strCodeList);
                    if(!list1.IsNullOrEmpty())
                    {
                        for (int j = 0; j < list1.Count; j++)
                        {
                            if(list1[j].EXCODE == "TX82" && strOK1 == "")
                            {
                                if( list1[j].RESULT.To<double>() >= 2.51) { strOK1 = "OK"; nCount1 += 1; }
                            }
                            else if (list1[j].EXCODE == "A108" && strOK2 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 140) { strOK2 = "OK"; nCount2 += 1; }
                            }
                            else if (list1[j].EXCODE == "A109" && strOK2 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 90) { strOK2 = "OK"; nCount2 += 1; }
                            }
                            else if (list1[j].EXCODE == "A122" && strOK3 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 126) { strOK3 = "OK"; nCount3 += 1; }
                            }
                            else if (list1[j].EXCODE == "A124" && strOK4 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 51) { strOK4 = "OK"; nCount4 += 1; }
                            }
                            else if (list1[j].EXCODE == "A125" && strOK4 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 51) { strOK4 = "OK"; nCount4 += 1; }
                            }
                            else if (list1[j].EXCODE == "A126" && strOK4 == "")
                            {
                                if(list[i].SEX == "M")
                                {
                                    if (list1[j].RESULT.To<double>() >= 118) { strOK4 = "OK"; nCount4 += 1; }
                                }
                                else if (list[i].SEX =="F")
                                {
                                    if (list1[j].RESULT.To<double>() >= 48) { strOK4 = "OK"; nCount4 += 1; }
                                }

                            }
                            else if (list1[j].EXCODE == "LH42" && strOK5 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 1.71) { strOK5 = "OK"; nCount5 += 1; }
                            }
                            else if (list1[j].EXCODE == "LH43" && strOK5 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 356) { strOK5 = "OK"; nCount5 += 1; }
                            }
                            else if (list1[j].EXCODE == "LH44" && strOK5 == "")
                            {
                                if (list1[j].RESULT.To<double>() >= 1.64) { strOK5 = "OK"; nCount5 += 1; }
                            }
                        }
                    }
                }

                //카운트표시
                SSList2.ActiveSheet.Cells[0, 0].Text = nCount1.ToString();
                SSList2.ActiveSheet.Cells[0, 1].Text = nCount2.ToString();
                SSList2.ActiveSheet.Cells[0, 2].Text = nCount3.ToString();
                SSList2.ActiveSheet.Cells[0, 3].Text = nCount4.ToString();
                SSList2.ActiveSheet.Cells[0, 4].Text = nCount5.ToString();

                btnExit.Enabled = true;
            }
            else
            {
                MessageBox.Show("대상건수가 0건입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.None);
            }

        }


    }
}
