using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaSetTime_Count.cs
/// Description     : 종검 시간별 예약정원 세팅
/// Author          : 김경동
/// Create Date     : 2021-08-04
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm검사별정원세팅(Frm검사별정원세팅.frm)" />

namespace ComHpcLibB
{
    public partial class frmHaSetTime_Count : Form
    {

        ComFunc CF = null;
        clsSpread cSpd = null;

        HeaResvSetTimeService heaResvSetTimeService = null;
        HeaCodeService heaCodeService = null;


        public frmHaSetTime_Count()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();

            heaResvSetTimeService = new HeaResvSetTimeService();
            heaCodeService = new HeaCodeService();

            string strYYMM = DateTime.Now.ToShortDateString();

            Set_Spread(strYYMM);

            CF.ComboMonth_Set3(cboYYMM, 13, DateTime.Now.Year, DateTime.Now.Month);

            cboExam.Items.Clear();
            cboExam.Items.Add("전체");
            cboExam.SetItems(heaCodeService.GetItemByGubunGroupBy("13"), "NAME", "", "", "", AddComboBoxPosition.None, false);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSet.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
        }

        private void Set_Spread(string argYYMM)
        {
            Set_Spread_Control1(SS2, argYYMM.Replace("-", ""));
            Set_Spread_Control2(SS1, argYYMM.Replace("-", ""));
        }


        private void Set_Spread_Control1(FpSpread sS2, string argYYMM)
        {
            //sS2.Initialize(new SpreadOption { RowHeaderVisible = true });
            sS2.Initialize();
            sS2.AddColumn("제 목", "", 188, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS2.AddColumn("구분", "", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(4, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                sS2.AddColumn(i.To<string>(), "", 28, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }
        }

        private void Set_Spread_Control2(FpSpread sS1, string argYYMM)
        {
            //sS1.Initialize(new SpreadOption { RowHeaderVisible = true });
            sS1.Initialize();
            sS1.AddColumn("검사구분", "", 0, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS1.AddColumn("검사항목", "", 188, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS1.AddColumn("구분", "", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(4, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                sS1.AddColumn(i.To<string>(), "", 28, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }

        }

        private void eFormload(object sender, EventArgs e)
        {
            Screen_Clear();

            cboExam.SelectedIndex = 0;
        }

        private void Screen_Clear()
        {
            cSpd.Spread_Clear_Simple(SS1, 25);
            cSpd.Spread_Clear_Simple(SS2, 3);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSet)
            {
                //frmHaSetExam_Daily frm = new frmHaSetExam_Daily();
                //frm.ShowDialog();
            }
            else if (sender == btnSearch)
            {
                Set_Spread(cboYYMM.Text.Trim());
                Screen_Clear();

                Screen_Display(cboYYMM.Text.Trim(), cboExam.Text.Trim());
            }
            else if (sender == btnCancel)
            {
                Set_Spread(cboYYMM.Text.Trim());
                Screen_Clear();
            }
            else if (sender == btnDelete)
            {
                Delete_Data_Set();
            }
            else if (sender == btnNew)
            {
                Set_Spread_New();
            }
        }

        private void Set_Yoil(string argYYMM)
        {
            string strDate = string.Empty;
            string strYoil = string.Empty;
            string strLastDay = CF.READ_LASTDAY(clsDB.DbCon, argYYMM + "-01");
            int nLastDay = VB.Right(strLastDay, 2).To<int>();

            UnaryComparisonConditionalFormattingRule unary;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.LightSteelBlue;

            //일자별 요일을 SET
            for (int i = 1; i <= nLastDay; i++)
            {
                strDate = argYYMM + "-" + VB.Format(i, "00");
                strYoil = CF.READ_YOIL(clsDB.DbCon, strDate);

                SS1.ActiveSheet.Cells[0, i + 2].Text = VB.Left(strYoil, 1);
                SS2.ActiveSheet.Cells[0, i + 1].Text = VB.Left(strYoil, 1);

                if (VB.Left(strYoil, 1) == "토")
                {
                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.LightSteelBlue;
                    SS1.ActiveSheet.SetConditionalFormatting(-1, i + 2, unary);

                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.LightSteelBlue;
                    SS2.ActiveSheet.SetConditionalFormatting(-1, i + 1, unary);
                }
                else if (VB.Left(strYoil, 1) == "일")
                {
                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.MistyRose;
                    SS1.ActiveSheet.SetConditionalFormatting(-1, i + 2, unary);

                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.MistyRose;
                    SS2.ActiveSheet.SetConditionalFormatting(-1, i + 1, unary);
                }
            }
        }


        private void Delete_Data_Set()
        {
            string strYYMM = VB.Left(cboYYMM.Text.Trim(), 4) + "-" + VB.Mid(cboYYMM.Text, 4, 2);

            if (strYYMM.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                if (MessageBox.Show(strYYMM + " 인원설정을 삭제하시겠습니까?", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string strGetSDate = strYYMM + "-01";
                    int nLastDD = DateTime.DaysInMonth(VB.Left(strGetSDate, 4).To<int>(), VB.Mid(strGetSDate, 6, 2).To<int>());
                    string strGetLDate = VB.Mid(strGetSDate, 1, 8) + VB.Format(nLastDD, "00");

                    if(!heaResvSetTimeService.DeleteBySDate(strGetSDate, strGetLDate))

                    {
                        MessageBox.Show("초기인원 삭제 중 에러발생", "에러");
                        return;
                    }

                    MessageBox.Show("삭제완료.");

                    eBtnClick(btnCancel, null);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Screen_Display(string argYYMM, string argGbName)
        {
            int nRow = 1;
            string strExName = string.Empty;
            string strDate = string.Empty;
            string strStime = "";
            SS2.ActiveSheet.RowCount = 1;

            Set_Yoil(argYYMM);

            if (argGbName == "전체")
            {
                //검사항목별 설정인원 Display
                for (int j = 0; j < SS2.ActiveSheet.ColumnCount - 2; j++)
                {
                    strDate = argYYMM + "-" + VB.Format(j + 1, "");

                    List<HEA_RESV_SET_TIME> list = heaResvSetTimeService.GetSumInwonAMPMByGubun(strDate, "00");

                    if(SS2.ActiveSheet.RowCount < list.Count + 1) { SS2.ActiveSheet.RowCount = list.Count + 1; }
                    if (list.Count>0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            SS2.ActiveSheet.Cells[i + 1, 1].Text = list[i].STIME;
                            SS2.ActiveSheet.Cells[i + 1, j + 2].Text = list[i].INWON.To<string>();
                        }             
                    }
                }
                
                
            }

        }

        private void Set_Spread_New()
        {

            string strGbn = string.Empty;
            string strYoil = "";

            //종합검진 인원 기본세팅

            for (int j = 2; j < SS2.ActiveSheet.ColumnCount; j++)
            {

                switch (SS2.ActiveSheet.Cells[0, j].Text.Trim())
                {
                    case "월": strYoil = "1"; break;
                    case "화": strYoil = "2"; break;
                    case "수": strYoil = "3"; break;
                    case "목": strYoil = "4"; break;
                    case "금": strYoil = "5"; break;
                    case "토": strYoil = "6"; break;
                    case "일": strYoil = "7"; break;
                    default: break;
                }

                List <HEA_RESV_SET_TIME> list = heaResvSetTimeService.GetAmPmInwonByGubun("1990-01-01", "00");
                if(SS2.ActiveSheet.RowCount < list.Count + 1) { SS2.ActiveSheet.RowCount = list.Count + 1; }

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (strYoil != "6" && strYoil != "7")
                        {
                            SS2.ActiveSheet.Cells[i + 1, 1].Text = list[i].STIME;
                            SS2.ActiveSheet.Cells[i + 1, j].Text = list[i].INWON.To<string>("0");
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i + 1, j].Text = "0";
                        }
                            
                    }
                }
            }


            MessageBox.Show("초기세팅 완료!", "작업완료");

        }

        private void Data_Save()
        {
            string strExGubun = string.Empty;
            string strExCode = string.Empty;
            string strDate = string.Empty;
            string strYoil = string.Empty;
            string strSTime = string.Empty;

            int nInwon = 0;
            int nGaInwon = 0;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                for (int j = 2; j < SS2.ActiveSheet.ColumnCount; j++)
                {
                    strDate = cboYYMM.Text.Trim() + "-" + VB.Format(j - 1, "00");
                    switch (SS2.ActiveSheet.Cells[0, j].Text)
                    {
                        case "월": strYoil = "1"; break;
                        case "화": strYoil = "2"; break;
                        case "수": strYoil = "3"; break;
                        case "목": strYoil = "4"; break;
                        case "금": strYoil = "5"; break;
                        case "토": strYoil = "6"; break;
                        case "일": strYoil = "7"; break;
                        default: break;
                    }


                    for (int i = 1; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        strSTime = SS2.ActiveSheet.Cells[i, 1].Text;
                        nInwon = Convert.ToInt32(SS2.ActiveSheet.Cells[i, j].Text);

                        HEA_RESV_SET_TIME item1 = new HEA_RESV_SET_TIME
                        {
                            SDATE = strDate,
                            STIME = strSTime,
                            INWON = nInwon,
                            GAINWON = nGaInwon,
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            GUBUN = "00"

                        };

                        HEA_RESV_SET_TIME item2 = heaResvSetTimeService.GetItemBySTimeGubun(strDate, strSTime, "00");
                        if (!item2.IsNullOrEmpty())
                        {
                            item1.RID = item2.RID;
                            heaResvSetTimeService.UpDate(item1);
                        }
                        else
                        {
                            heaResvSetTimeService.Insert(item1);
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장완료!", "작업완료");
                return;
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

    }
}
