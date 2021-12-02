using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcMisuNoHelp2.cs
/// Description     : 지사별 미수번호 찾기
/// Author          : 심명섭
/// Create Date     : 2021-06-21
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > FrmMisunoHelp2 (HcMisu03.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcMisuNoHelp2 :BaseForm
    {
        public delegate void SetGstrValue(HIC_MISU_MST_SLIP GstrValue);
        public static event SetGstrValue rSetGstrValue;
        
        clsSpread cSpd                                  = null;
        ComFunc CF                                      = null;
        HicMisuMstSlipService hicMisuMstSlipService     = null;
        clsHaBase cHB                                   = null;
        frmHcCodeHelp FrmHcCodeHelp                     = null;

        string FstrCode = "";
        string FstrName = "";


        public frmHcMisuNoHelp2()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            CF = new ComFunc();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            cHB = new clsHaBase();

            SS1.Initialize(new SpreadOption { RowHeight = 20 });
            SS1.AddColumn("미수번호",       nameof(HIC_MISU_MST_SLIP.WRTNO),        100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("청구일자",       nameof(HIC_MISU_MST_SLIP.BDATE),        100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            
            // 회사코드 번호
            SS1.AddColumn("지사명",         nameof(HIC_MISU_MST_SLIP.JISACODE),     100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = true });
            // 쿼리에서 함수 호출 / 프로퍼티 생성(LTDNAME) /  KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME                   
            SS1.AddColumn("회사명",         nameof(HIC_MISU_MST_SLIP.KIHOCODE),     100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });

            SS1.AddColumn("검진종류",       nameof(HIC_MISU_MST_SLIP.GJNAME),       100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("검진종류",       nameof(HIC_MISU_MST_SLIP.GJNAME),       100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("미수금액",       nameof(HIC_MISU_MST_SLIP.MISUAMT),      100, FpSpreadCellType.NumberCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("입금금액",       nameof(HIC_MISU_MST_SLIP.IPGUMAMT),     100, FpSpreadCellType.NumberCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });

            SS1.AddColumn("삭감반송",       nameof(HIC_MISU_MST_SLIP.DAMT),         100, FpSpreadCellType.NumberCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });

            SS1.AddColumn("미수잔액",       nameof(HIC_MISU_MST_SLIP.JANAMT),       100, FpSpreadCellType.NumberCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("청구번호",       nameof(HIC_MISU_MST_SLIP.GIRONO),       100, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("적요",           nameof(HIC_MISU_MST_SLIP.REMARK),       350, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnJisaHelp.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            HIC_MISU_MST_SLIP item = SS1.GetCurrentRowData() as HIC_MISU_MST_SLIP;
            rSetGstrValue(item);
            this.Close();
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
                DisPlay_Screen();
            }

            else if (sender == btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Spread_Print();
            }

            else if (sender == btnJisaHelp)
            {
                JisaHelp();
            }

        }

        private void JisaHelp()
        {
            clsPublic.GstrRetValue = "21";      //건강보험 지사
            FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
            FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
            FrmHcCodeHelp.ShowDialog();

            if (!FstrCode.IsNullOrEmpty())
            {
                TxtJisaCode.Text = FstrCode.Trim();
                PanelJisaName.Text = FstrName.Trim();
            }
            else
            {
                TxtJisaCode.Text = "";
                PanelJisaName.Text = "";
            }

            FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
        }

        void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }
        private void DisPlay_Screen()
        {
            clsPublic.GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");
            
            string      strJisaCode;
            string      strYYMM;
            string      strFdate;
            string      strTdate;
            string      strDtlJong;
            string      strJong;
            string      strView;

            strJisaCode     =   TxtJisaCode.Text.Trim();
            strJong         =   VB.Left(CboJong.Text, 1);
            strDtlJong      =   VB.Left(CboDtlJong.Text, 1);
            strView         =   TxtView.Text.Trim();

            SS1.ActiveSheet.RowCount = 20;
            SS_Clear(SS1_Sheet1);

            if (CboYYMM.Text == "전체")
            {
                strYYMM = "";
                strFdate = "";
                strTdate = clsPublic.GstrSysDate;
            }
            else if (CboYYMM.Text == "최근6개월")
            {
                strYYMM = "";
                strFdate = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -190);
                strTdate = clsPublic.GstrSysDate;
            }
            else if (CboYYMM.Text == "최근3개월")
            {
                strYYMM = "";
                strFdate = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -93);
                strTdate = clsPublic.GstrSysDate;
            }
            else
            {
                strYYMM = VB.Left(CboYYMM.Text, 4) + VB.Mid(CboYYMM.Text, 6, 2);
                strFdate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
                strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFdate);
            }

            List<HIC_MISU_MST_SLIP> item = hicMisuMstSlipService.GetJisaName(strFdate, strTdate, strJong, strDtlJong, strJisaCode, rdoJob1.Checked, rdoJob2.Checked, strView, rdoSort1.Checked);

            SS1.ActiveSheet.RowCount = item.Count();

            for (int i = 0; i < item.Count(); i++)
            {
                //nBanAmt = item[i].GAMAMT;
                //nBanAmt += item[i].SAKAMT;
                //nBanAmt += item[i].BANAMT;

                //SS1.ActiveSheet.Cells[i, 0].Text = item[i].WRTNO.To<string>("0");
                //SS1.ActiveSheet.Cells[i, 1].Text = item[i].BDATE.Trim();

                //SS1.ActiveSheet.Cells[i, 2].Text = cHB.READ_HIC_CODE("21", item[i].JISA);
                //SS1.ActiveSheet.Cells[i, 3].Text = cHB.READ_HIC_CODE("18", item[i].KIHO);

                //SS1.ActiveSheet.Cells[i, 4].Text = cHB.READ_GjJong_Name(item[i].GJONG.Trim());
                //SS1.ActiveSheet.Cells[i, 5].Text = VB.Format(item[i].MISUAMT, "###,###,###,##0");
                
                //SS1.ActiveSheet.Cells[i, 6].Text = VB.Format(item[i].IPGUMAMT, "###,###,###,##0");
                //SS1.ActiveSheet.Cells[i, 7].Text = VB.Format(nBanAmt, "###,###,###,##0");

                //SS1.ActiveSheet.Cells[i, 8].Text = VB.Format(item[i].JANAMT, "###,###,###,##0");
                //SS1.ActiveSheet.Cells[i, 9].Text = item[i].GIRONO;

                //SS1.ActiveSheet.Cells[i, 10].Text = item[i].REMARK;

                SS1.DataSource = item;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            clsPublic.GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");
            int nYY;
            int nMM;

            CboJong.Clear();
            CboJong.Items.Add("*. 전체          ");
            CboJong.Items.Add("1. 회사미수      ");
            CboJong.Items.Add("2. 건강보험      ");
            CboJong.Items.Add("3. 국고          ");
            CboJong.Items.Add("4. 개인미수      ");
            CboJong.SelectedIndex = 2;

            CboDtlJong.Clear();
            CboDtlJong.Items.Add("*. 전체       ");
            CboDtlJong.Items.Add("1. 성인병     ");
            CboDtlJong.Items.Add("2. 공무원     ");
            CboDtlJong.Items.Add("3. 사업장     ");
            CboDtlJong.Items.Add("4. 기타검진   ");
            CboDtlJong.Items.Add("5. 작업측정   ");
            CboDtlJong.Items.Add("6. 보건대행   ");
            CboDtlJong.Items.Add("7. 종합건진   ");
            CboDtlJong.SelectedIndex = 0;
            
            TxtJisaCode.Text = "";
            TxtView.Text = "";
            PanelJisaName.Text = "전체";

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>(0);
            nMM = VB.Mid(clsPublic.GstrSysDate, 6, 2).To<int>(0);

            CboYYMM.Clear();
            CboYYMM.Items.Add("전체");
            CboYYMM.Items.Add("최근6개월");
            CboYYMM.Items.Add("최근3개월");

            for (int i = 0; i < 24; i++)
            {
                CboYYMM.Items.Add(VB.Format(nYY, "0000") + "년" + VB.Format(nMM, "00") + "월");
                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }
            CboYYMM.SelectedIndex = 1;
        }

        private void Spread_Print()
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "사  업  장   코  드  집";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("지사명칭 : " + PanelJisaName.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("청구년월 : " + VB.Left(CboYYMM.Text + VB.Space(60), 60) + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for (int i = 0; i < Spd.RowCount; i++)
            {
                for (int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }
    }
}
