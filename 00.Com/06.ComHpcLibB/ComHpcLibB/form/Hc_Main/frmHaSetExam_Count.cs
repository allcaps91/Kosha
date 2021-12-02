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
/// File Name       : frmHaSetExam_Count.cs
/// Description     : 종검 검사별 예약정원 세팅
/// Author          : 김민철
/// Create Date     : 2020-03-19
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm검사별정원세팅(Frm검사별정원세팅.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaSetExam_Count : Form
    {
        ComFunc CF = null;
        clsSpread cSpd = null;

        HeaResvSetService heaResvSetService = null;
        HeaCodeService heaCodeService = null;

        public frmHaSetExam_Count()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();

            heaResvSetService = new HeaResvSetService();
            heaCodeService = new HeaCodeService();

            string strYYMM = DateTime.Now.ToShortDateString();

            Set_Spread(strYYMM);

            CF.ComboMonth_Set3(cboYYMM, 13, DateTime.Now.Year, DateTime.Now.Month);

            cboExam.Items.Clear();
            cboExam.Items.Add("전체");
            cboExam.SetItems(heaCodeService.GetItemByGubunGroupBy("13"), "NAME", "", "", "", AddComboBoxPosition.None, false);
        }

        private void Set_Spread(string argYYMM)
        {
            Set_Spread_Control1(SS2, argYYMM.Replace("-", ""));
            Set_Spread_Control2(SS1, argYYMM.Replace("-", ""));
        }

        private void Set_Spread_New()
        {
            string strYoil = "";

            //종합검진 인원 기본세팅
            for (int i = 2; i < SS2.ActiveSheet.ColumnCount; i++)
            {
                switch (SS2.ActiveSheet.Cells[0, i].Text.Trim())
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

                HEA_RESV_SET item = heaResvSetService.GetAmPmInwonByGubun("1999-01-01", "3", "SS", strYoil);

                if (!item.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[1, i].Text = item.AMINWON.To<string>("0");
                    SS2.ActiveSheet.Cells[2, i].Text = item.PMINWON.To<string>("0");
                }
            }

            string strGbn = string.Empty;

            //선택검사별 인원 기본세팅
            for (int i = 1; i < SS1.ActiveSheet.RowCount; i += 2)
            {
                strGbn = SS1.ActiveSheet.Cells[i, 0].Text.Trim();

                for (int j = 3; j < SS1.ActiveSheet.ColumnCount; j++)
                {
                    switch (SS1.ActiveSheet.Cells[0, j].Text.Trim())
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

                    HEA_RESV_SET item = heaResvSetService.GetAmPmInwonByGubun("1999-01-01", "3", strGbn, strYoil);

                    if (!item.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, j].Text = item.AMINWON.To<string>("0");
                        SS1.ActiveSheet.Cells[i + 1, j].Text = item.PMINWON.To<string>("0");
                    }
                }
            }

            MessageBox.Show("초기세팅 완료!", "작업완료");

        }

        private void Set_Spread_Control2(FpSpread sS1, string argYYMM)
        {
            //sS1.Initialize(new SpreadOption { RowHeaderVisible = true });
            sS1.Initialize();
            sS1.AddColumn("검사구분", "",   0, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS1.AddColumn("검사항목", "", 188, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS1.AddColumn("구분",     "",  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(4, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                sS1.AddColumn(i.To<string>(), "", 28, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }

        }

        private void Set_Spread_Control1(FpSpread sS2, string argYYMM)
        {
            //sS2.Initialize(new SpreadOption { RowHeaderVisible = true });
            sS2.Initialize();
            sS2.AddColumn("제 목",  "",  188, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            sS2.AddColumn("구분",   "",   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(4, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                sS2.AddColumn(i.To<string>(), "", 28, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormload);
            this.btnNew.Click       += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSet.Click       += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnCancel.Click    += new EventHandler(eBtnClick);
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
                frmHaSetExam_Daily frm = new frmHaSetExam_Daily();
                frm.ShowDialog();
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

                    if (!heaResvSetService.DeleteBySDate(strGetSDate, strGetLDate))
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

        private void Data_Save()
        {
            string strExGubun = string.Empty;
            string strExCode = string.Empty;
            string strDate = string.Empty;
            string strYoil = string.Empty;

            int nAmInwon = 0;
            int nPmInwon = 0;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 1; i < SS1.ActiveSheet.RowCount; i += 2)
                {
                    strExGubun = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strExCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();

                    for (int j = 3; j < SS1.ActiveSheet.ColumnCount; j++)
                    {
                        strDate = cboYYMM.Text.Trim() + "-" + VB.Format(j - 2, "00");
                        switch (SS1.ActiveSheet.Cells[0, j].Text)
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

                        nAmInwon = SS1.ActiveSheet.Cells[i, j].Text.To<int>();
                        nPmInwon = SS1.ActiveSheet.Cells[i + 1, j].Text.To<int>();

                        HEA_RESV_SET item1 = new HEA_RESV_SET
                        {
                            SDATE = strDate,
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            YOIL = strYoil,
                            GBRESV = "1",
                            GUBUN = strExGubun,
                            EXAMNAME = strExCode
                        };

                        HEA_RESV_SET item2 = heaResvSetService.GetAmPmInwonByGubun(strDate, "1", strExGubun);
                        if (!item2.IsNullOrEmpty())
                        {
                            item1.RID = item2.RID;
                            heaResvSetService.UpDate(item1);
                        }
                        else
                        {
                            heaResvSetService.Insert(item1);
                        }
                    }
                }

                for (int j = 2; j < SS2.ActiveSheet.ColumnCount; j++)
                {
                    strDate = cboYYMM.Text.Trim() + "-" + VB.Format(j - 1, "00");
                    switch(SS2.ActiveSheet.Cells[0, j].Text)
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

                    nAmInwon = SS2.ActiveSheet.Cells[1, j].Text.To<int>();
                    nPmInwon = SS2.ActiveSheet.Cells[2, j].Text.To<int>();

                    HEA_RESV_SET item1 = new HEA_RESV_SET
                    {
                        SDATE = strDate,
                        AMINWON = nAmInwon,
                        PMINWON = nPmInwon,
                        ENTSABUN = clsType.User.IdNumber.To<long>(),
                        YOIL = strYoil,
                        GBRESV = "",
                        GUBUN = "TT",
                        EXAMNAME = "종합검진정원"
                    };

                    HEA_RESV_SET item2 = heaResvSetService.GetItemByGubun(strDate, "00");
                    if (!item2.IsNullOrEmpty())
                    {
                        item1.RID = item2.RID;
                        heaResvSetService.UpDate(item1);
                    }
                    else
                    {
                        heaResvSetService.Insert(item1);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장완료!", "작업완료");
                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void Screen_Display(string argYYMM, string argGbName)
        {
            int nRow = 1;
            string strExName = string.Empty;
            string strDate = string.Empty;

            Set_Yoil(argYYMM);

            if (argGbName == "전체")
            {
                //검사항목 Display
                List<HEA_CODE> lst2 = heaCodeService.GetItemByGubunGroupBy("13");

                if (lst2.Count > 0)
                {
                    SS1.ActiveSheet.RowCount = lst2.Count * 2 + 1;

                    for (int i = 0; i < lst2.Count; i++)
                    {
                        SS1.ActiveSheet.Cells[nRow, 0].Text = lst2[i].GUBUN2.Trim();
                        SS1.ActiveSheet.Cells[nRow, 1].Text = lst2[i].NAME.Trim();
                        SS1.ActiveSheet.Cells[nRow, 2].Text = "AM";

                        SS1.ActiveSheet.AddSpanCell(nRow, 1, 2, 1);

                        nRow = nRow + 1;

                        SS1.ActiveSheet.Cells[nRow, 0].Text = lst2[i].GUBUN2.Trim();
                        SS1.ActiveSheet.Cells[nRow, 2].Text = "PM";

                        nRow = nRow + 1;
                    }
                }
            }
            else
            {
                //개별항목의 경우 요일 + 검사항목만 표시
                SS1.ActiveSheet.RowCount = 3;
                SS1.ActiveSheet.Cells[nRow, 0].Text = heaCodeService.GetGubun2ByNameGubun(argGbName, "13");
                SS1.ActiveSheet.Cells[nRow, 1].Text = argGbName;
                SS1.ActiveSheet.Cells[nRow, 2].Text = "AM";

                SS1.ActiveSheet.AddSpanCell(nRow, 1, 2, 1);

                nRow = nRow + 1;

                SS1.ActiveSheet.Cells[nRow, 0].Text = heaCodeService.GetGubun2ByNameGubun(argGbName, "13");
                SS1.ActiveSheet.Cells[nRow, 1].Text = argGbName;
                SS1.ActiveSheet.Cells[nRow, 2].Text = "PM";
            }

            

            //검사항목별 설정인원 Display
            for (int i = 1; i < SS1.ActiveSheet.RowCount; i += 2)
            {
                strExName = SS1.ActiveSheet.Cells[i, 0].Text;

                if (!strExName.IsNullOrEmpty())
                {
                    for (int j = 0; j < SS1.ActiveSheet.ColumnCount - 3; j++)
                    {
                        strDate = argYYMM + "-" + VB.Format(j + 1, "");

                        HEA_RESV_SET item1 = heaResvSetService.GetAmPmInwonByGubun(strDate, "1", strExName);

                        if (!item1.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, j + 3].Text = item1.AMINWON.To<string>();
                            SS1.ActiveSheet.Cells[i + 1, j + 3].Text = item1.PMINWON.To<string>();
                        }
                    }
                }
            }

            //종합검진 정원 표시
            SS2.ActiveSheet.Cells[0, 0].Text = "종합검진 정원";
            SS2.ActiveSheet.AddSpanCell(0, 0, 3, 1);

            SS2.ActiveSheet.Cells[1, 1].Text = "AM";
            SS2.ActiveSheet.Cells[2, 1].Text = "PM";

            for (int i = 2; i < SS2.ActiveSheet.ColumnCount; i++)
            {
                strDate = argYYMM + "-" + VB.Format(i - 1, "00");

                HEA_RESV_SET item = heaResvSetService.GetItemByGubun(strDate, "00");

                if (!item.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[1, i].Text = item.AMINWON.To<string>();
                    SS2.ActiveSheet.Cells[2, i].Text = item.PMINWON.To<string>();
                }
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
    }
}
