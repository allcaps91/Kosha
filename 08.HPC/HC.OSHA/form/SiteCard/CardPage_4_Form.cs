using ComBase;
using ComBase.Controls;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Repository.Schedule;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.Model.Visit;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class CardPage_4_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {

        InOutEmployeeForm inOutEmployeeForm;
        IndustrialAccidentForm accForm;

        HcOshaCard5Service hcOshaCard5Service;
        OshaVisitPriceRepository oshaVisitPriceRepository;

        public CardPage_4_Form()
        {
            InitializeComponent();
            hcOshaCard5Service= new HcOshaCard5Service();
            oshaVisitPriceRepository = new OshaVisitPriceRepository();
            inOutEmployeeForm = new InOutEmployeeForm();
            accForm = new IndustrialAccidentForm();

            SSCard.ActiveSheet.ActiveRowIndex = 0;
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }
        public void Search()
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            string SqlErr = "";
            string strVisit = "";
            DataTable dt = null;
            double dblRate;
            
            Cursor.Current = Cursors.WaitCursor;

            Clear();
            if (base.SelectedSite == null) return;

            string year = this.SelectedEstimate.CONTRACTSTARTDATE.Left(4);//  SelectedEstimate GetCurrentYear();
            string year_1 = (year.To<int>() - 1).ToString();// 1년전
            string year_2 = (year.To<int>() - 2).ToString();// 2년전

            SSCard.ActiveSheet.Cells[3, 0].Text = year_1.Substring(2, 2);
            SSCard.ActiveSheet.Cells[5, 0].Text = year.Substring(2, 2);

            List<Dictionary<string, int>> workerInfo = new List<Dictionary<string, int>>();
            List<HC_OSHA_CARD5> list = null;

            list = hcOshaCard5Service.hcOshaCard5Repository.FindYear(base.SelectedSite.ID, year_1);
            Darw(list, 3, true);
            Darw(list, 4, false);

            list = hcOshaCard5Service.hcOshaCard5Repository.FindYear(base.SelectedSite.ID, year);
            Darw(list, 5, true);
            Darw(list, 6, false);

            //6.산업재해발생현황
            //작년 월별 근로자수

            long[,] array = new long[26, 5];
            for (i = 0; i < 26; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    array[i, j] = 0;
                }
            }

            SSCard.ActiveSheet.Cells[15, 0].Value = year_1.Substring(2, 2);

            try
            {
                SQL = "SELECT VISITDATE,CURRENTWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL = SQL + ComNum.VBLF + "      INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT ";
                SQL = SQL + ComNum.VBLF + "  FROM HIC_OSHA_REPORT_NURSE ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITE_ID=" + base.SelectedSite.ID + " ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE>='" + year_2 + "0101' ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE<='" + year + "1231' ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strVisit = dt.Rows[i]["VISITDATE"].ToString().Trim();
                        long cnt1 = long.Parse(dt.Rows[i]["CURRENTWORKERCOUNT"].ToString());
                        long cnt2 = long.Parse(dt.Rows[i]["ACCIDENTWORKERCOUNT"].ToString());
                        long cnt3 = long.Parse(dt.Rows[i]["DEADWORKERCOUNT"].ToString());
                        long cnt4 = long.Parse(dt.Rows[i]["INJURYWORKERCOUNT"].ToString());
                        long cnt5 = long.Parse(dt.Rows[i]["BIZDISEASEWORKERCOUNT"].ToString());

                        if (VB.Left(strVisit,4)== year_2)
                        {
                            //2년전
                            array[0, 0] += cnt1; //근로자수
                            array[0, 1] += cnt2; //재해자수계
                            array[0, 2] += cnt3; //사망
                            array[0, 3] += cnt4; //부상
                            array[0, 4] += cnt5; //직업병
                        }

                        //전년도
                        if (VB.Left(strVisit,4)==year_1)
                        {
                            //1년전
                            array[1, 0] += cnt1; //근로자수
                            array[1, 1] += cnt2; //재해자수계
                            array[1, 2] += cnt3; //사망
                            array[1, 3] += cnt4; //부상
                            array[1, 4] += cnt5; //직업병
                            //전년도 월별
                            j = Int32.Parse(VB.Mid(strVisit, 5, 2))+1;
                            array[j, 0] += cnt1; //근로자수
                            array[j, 1] += cnt2; //재해자수계
                            array[j, 2] += cnt3; //사망
                            array[j, 3] += cnt4; //부상
                            array[j, 4] += cnt5; //직업병
                        }
                        //금년도
                        if (VB.Left(strVisit, 4) == year)
                        {
                            //월별
                            j = Int32.Parse(VB.Mid(strVisit, 5, 2)) + 13;
                            array[j, 0] += cnt1; //근로자수
                            array[j, 1] += cnt2; //재해자수계
                            array[j, 2] += cnt3; //사망
                            array[j, 3] += cnt4; //부상
                            array[j, 4] += cnt5; //직업병
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //2년전
                if (array[0, 0]>0)
                {
                    SSCard.ActiveSheet.Cells[14, 4].Value = array[0, 0] + " 명";
                    if (array[0, 1] > 0) SSCard.ActiveSheet.Cells[14, 8].Value = array[0, 1];
                    if (array[0, 2] > 0) SSCard.ActiveSheet.Cells[14, 11].Value = array[0, 2];
                    if (array[0, 3] > 0) SSCard.ActiveSheet.Cells[14, 14].Value = array[0, 3];
                    if (array[0, 4] > 0) SSCard.ActiveSheet.Cells[14, 17].Value = array[0, 4];
                    if (array[0, 0] > 0)
                    {
                        if (array[0, 1] == 0)
                        {
                            SSCard.ActiveSheet.Cells[14, 20].Value = "0.00 %";
                        }
                        else
                        {
                            dblRate = array[0, 1] / array[0, 0] * 100;
                            SSCard.ActiveSheet.Cells[14, 20].Value = VB.Format(dblRate, "#0.00") + " %";
                        }
                    }
                }

                //1년전
                if (array[1, 0] > 0)
                {
                    SSCard.ActiveSheet.Cells[15, 4].Value = array[1, 0] + " 명";
                    if (array[1, 1] > 0) SSCard.ActiveSheet.Cells[15, 8].Value = array[1, 1];
                    if (array[1, 2] > 0) SSCard.ActiveSheet.Cells[15, 11].Value = array[1, 2];
                    if (array[1, 3] > 0) SSCard.ActiveSheet.Cells[15, 14].Value = array[1, 3];
                    if (array[1, 4] > 0) SSCard.ActiveSheet.Cells[15, 17].Value = array[1, 4];
                    if (array[1, 0] > 0)
                    {
                        if (array[1, 1] == 0)
                        {
                            SSCard.ActiveSheet.Cells[15, 20].Value = "0.00 %";
                        }
                        else
                        {
                            dblRate = array[1, 1] / array[1, 0] * 100;
                            SSCard.ActiveSheet.Cells[15, 20].Value = VB.Format(dblRate, "#0.00") + " %";
                        }
                    }
                }

                //전년도 월별
                SSCard.ActiveSheet.Cells[16, 0].Value = year_1.Substring(2, 2);
                for (i = 0; i < 12; i++)
                {
                    if (array[i+2, 0] > 0)
                    {
                        SSCard.ActiveSheet.Cells[i+16, 4].Value = array[i + 2, 0] + " 명";
                        if (array[i + 2, 1] > 0) SSCard.ActiveSheet.Cells[i + 16, 8].Value = array[i + 2, 1];
                        if (array[i + 2, 2] > 0) SSCard.ActiveSheet.Cells[i + 16, 11].Value = array[i + 2, 2];
                        if (array[i + 2, 3] > 0) SSCard.ActiveSheet.Cells[i + 16, 14].Value = array[i + 2, 3];
                        if (array[i + 2, 4] > 0) SSCard.ActiveSheet.Cells[i + 16, 17].Value = array[i + 2, 4];
                        if (array[i + 2, 0] > 0)
                        {
                            if (array[i + 2, 1] == 0)
                            {
                                SSCard.ActiveSheet.Cells[i + 16, 20].Value = "0.00 %";
                            }
                            else
                            {
                                dblRate = array[i + 2, 1] / array[i + 2, 0] * 100;
                                SSCard.ActiveSheet.Cells[i + 16, 20].Value = VB.Format(dblRate, "#0.00") + " %";
                            }
                        }
                    }
                }

                //금년도 월별
                SSCard.ActiveSheet.Cells[27, 0].Value = year.Substring(2, 2);
                for (i = 0; i < 12; i++)
                {
                    if (array[i + 14, 0] > 0)
                    {
                        SSCard.ActiveSheet.Cells[i + 27, 4].Value = array[i + 14, 0] + " 명";
                        if (array[i + 14, 1] > 0) SSCard.ActiveSheet.Cells[i + 27, 8].Value = array[i + 14, 1];
                        if (array[i + 14, 2] > 0) SSCard.ActiveSheet.Cells[i + 27, 11].Value = array[i + 14, 2];
                        if (array[i + 14, 3] > 0) SSCard.ActiveSheet.Cells[i + 27, 14].Value = array[i + 14, 3];
                        if (array[i + 14, 4] > 0) SSCard.ActiveSheet.Cells[i + 27, 17].Value = array[i + 14, 4];
                        if (array[i + 14, 0] > 0)
                        {
                            if (array[i + 14, 1] == 0)
                            {
                                SSCard.ActiveSheet.Cells[i + 27, 20].Value = "0.00 %";
                            }
                            else
                            {
                                dblRate = array[i + 14, 1] / array[i + 14, 0] * 100;
                                SSCard.ActiveSheet.Cells[i + 27, 20].Value = VB.Format(dblRate,"#0.00") + " %";
                            }
                        }
                    }
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private int GetColmunIndex(int IND_ACC_TYPE)
        {
            if (IND_ACC_TYPE == 1)
            {
                return 11;
            }
            else if (IND_ACC_TYPE == 2)
            {
                return 14;
            }
            else if (IND_ACC_TYPE == 3)
            {
                return 17;
            }
            else
            {
                throw new Exception("산업재해 구분이 없습니다");
            }
        }
        private void Darw(List<HC_OSHA_CARD5> list, int rowIndex, bool isJoin)
        {
            string month = string.Empty;
            int count = 0;
            foreach (HC_OSHA_CARD5 dto in list)
            {
                month = dto.REGISTERDATE.Substring(5, 2);
                if (isJoin)
                {
                    count = (int)dto.JOINCOUNT;
                }else
                {
                    count = (int)dto.QUITCOUNT;
                }
                switch (month)
                {
                    case "01":
                        SSCard.ActiveSheet.Cells[rowIndex, 4].Value = count;
                        break;
                    case "02":
                        SSCard.ActiveSheet.Cells[rowIndex, 6].Value = count;
                        break;
                    case "03":
                        SSCard.ActiveSheet.Cells[rowIndex, 8].Value = count;
                        break;
                    case "04":
                        SSCard.ActiveSheet.Cells[rowIndex, 10].Value = count;
                        break;
                    case "05":
                        SSCard.ActiveSheet.Cells[rowIndex, 12].Value = count;
                        break;
                    case "06":
                        SSCard.ActiveSheet.Cells[rowIndex, 14].Value = count;
                        break;
                    case "07":
                        SSCard.ActiveSheet.Cells[rowIndex, 16].Value = count;
                        break;
                    case "08":
                        SSCard.ActiveSheet.Cells[rowIndex, 18].Value =  count;;
                        break;
                    case "09":
                        SSCard.ActiveSheet.Cells[rowIndex, 20].Value =  count;;
                        break;
                    case "10":
                        SSCard.ActiveSheet.Cells[rowIndex, 22].Value =  count;;
                        break;
                    case "11":
                        SSCard.ActiveSheet.Cells[rowIndex, 24].Value =  count;;
                        break;
                    case "12":
                        SSCard.ActiveSheet.Cells[rowIndex, 26].Value =  count;;
                        break;
                }
            }
        }
        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();


            inOutEmployeeForm.SelectedEstimate = base.SelectedEstimate;
            accForm.SelectedEstimate = base.SelectedEstimate;

            inOutEmployeeForm.Select();
            accForm.Select();
        }

      

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();

            inOutEmployeeForm.SelectedSite = base.SelectedSite;
            accForm.SelectedSite = base.SelectedSite;

            inOutEmployeeForm.Select();
            accForm.Select();
        }

        private void Clear()
        {
            SSCard.ActiveSheet.Cells[3, 0].Text = string.Empty;
            SSCard.ActiveSheet.Cells[5, 0].Text = string.Empty;

            for (int i=4; i< 27; i++)
            {
                SSCard.ActiveSheet.Cells[3, i].Text = "0";
                SSCard.ActiveSheet.Cells[4, i].Text = "0";
                SSCard.ActiveSheet.Cells[5, i].Text = "0";
                SSCard.ActiveSheet.Cells[6, i].Text = "0";
            }


            //재해자수
            for(int i=1; i<4; i++)
            {
                SSCard.ActiveSheet.Cells[15, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[16, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[17, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[18, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[19, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[20, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[21, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[22, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[23, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[24, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[25, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[26, GetColmunIndex(i)].Value = 0;

                SSCard.ActiveSheet.Cells[27, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[28, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[29, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[30, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[31, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[32, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[33, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[34, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[35, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[36, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[37, GetColmunIndex(i)].Value = 0;
                SSCard.ActiveSheet.Cells[38, GetColmunIndex(i)].Value = 0;
            }
            for (int i = 15; i < 39; i++)
            {
                SSCard.ActiveSheet.Cells[i, 4].Value = "";

                SSCard.ActiveSheet.Cells[i, 11].Value = 0;
                SSCard.ActiveSheet.Cells[i, 14].Value = 0;
                SSCard.ActiveSheet.Cells[i, 17].Value = 0;
                SSCard.ActiveSheet.Cells[i, 8].Value =  0;
                SSCard.ActiveSheet.Cells[i, 5].Value = 0;
                SSCard.ActiveSheet.Cells[i, 21].Value = "";
                
            }
        }

        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            //Task.WaitAny(print.Execute());
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void SSCard_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void CardPage_4_Form_Load(object sender, EventArgs e)
        {
            
            inOutEmployeeForm.SelectedSite = base.SelectedSite;
            AddForm(inOutEmployeeForm, tabPage2);
       
            accForm.SelectedSite = base.SelectedSite;
            AddForm(accForm, tabPage3);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }
    }
}
