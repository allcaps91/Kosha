using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcPreChkPrint :CommonForm
    {
        HIC_CHUKMST_NEW hCMN = null;
        HicChkMcodeService hicChkMcodeService = null;
        HicChukWorkerService hicChukWorkerService = null;
        HicChukDtlSubltdService hicChukDtlSubltdService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HicChukDtlChemicalService hicChukDtlChemicalService = null;
        HcCodeService hcCodeService = null;

        clsSpread sp = new clsSpread();

        public frmHcPreChkPrint()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPreChkPrint(HIC_CHUKMST_NEW aHCMN)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            hCMN = aHCMN;
        }

        private void SetControl()
        {
            hCMN = new HIC_CHUKMST_NEW();
            hicChkMcodeService = new HicChkMcodeService();
            hicChukWorkerService = new HicChukWorkerService();
            hicChukDtlSubltdService = new HicChukDtlSubltdService();
            hicChukDtlPlanService = new HicChukDtlPlanService();
            hicChukDtlChemicalService = new HicChukDtlChemicalService();
            hcCodeService = new HcCodeService();

            #region 측정사업장 공정 및 유해인자 측정계획 내역
            SpreadComboBoxData scbChkWay = hcCodeService.GetSpreadComboBoxData("WEM_MTH_CD", "WEM");
            SpreadComboBoxData scbChkJugi = hcCodeService.GetSpreadComboBoxData("OCCRRNC_CYCLE_CD", "WEM");

            ssGONG.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 44 });
            ssGONG.AddColumn("일련번호",  nameof(HIC_CHUKDTL_PLAN.WRTNO),       74, new SpreadCellTypeOption { IsVisivle = false });
            ssGONG.AddColumnNumber("순번", nameof(HIC_CHUKDTL_PLAN.SEQNO),       48, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssGONG.AddColumn("공정명",    nameof(HIC_CHUKDTL_PLAN.PROCESS_NM), 130, new SpreadCellTypeOption { });
            ssGONG.AddColumn("유해인자명",nameof(HIC_CHUKDTL_PLAN.MCODE_NM),  174,  new SpreadCellTypeOption { });
            ssGONG.AddColumnComboBox("유해인자 발생주기", nameof(HIC_CHUKDTL_PLAN.JUGI), 62, IsReadOnly.N, scbChkJugi, new SpreadCellTypeOption { });
            ssGONG.AddColumn("근로자수", nameof(HIC_CHUKDTL_PLAN.INWON), 54, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("작업시간", nameof(HIC_CHUKDTL_PLAN.JTIME), 54, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("폭로시간", nameof(HIC_CHUKDTL_PLAN.PTIME), 54, new SpreadCellTypeOption { });
            ssGONG.AddColumnComboBox("측정방법", nameof(HIC_CHUKDTL_PLAN.CHKWAY), 84, IsReadOnly.N, scbChkWay, new SpreadCellTypeOption { IsSort = false });
            ssGONG.AddColumnNumber("예상 시료채취 또는 측정건수", nameof(HIC_CHUKDTL_PLAN.CHKCOUNT), 92, new SpreadCellTypeOption { TextMaxLength = 3 });
            ssGONG.AddColumn("ROWID", nameof(HIC_CHUKDTL_PLAN.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 측정사업장 공정별 유해화학물질 사용 실태
            SpreadComboBoxData TREATMENT_UNIT = hiccodeService.GetSpreadComboBoxData("C2");

            ssCHEMICAL.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssCHEMICAL.AddColumn("일련번호",                    nameof(HIC_CHUKDTL_CHEMICAL.WRTNO),       74, new SpreadCellTypeOption { IsVisivle = false });
            ssCHEMICAL.AddColumnNumber("순번",                  nameof(HIC_CHUKDTL_CHEMICAL.SEQNO),       48, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssCHEMICAL.AddColumn("부서 또는 공정명",            nameof(HIC_CHUKDTL_CHEMICAL.PROCESS_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssCHEMICAL.AddColumn("화학물질명(상품명)",          nameof(HIC_CHUKDTL_CHEMICAL.PRODUCT_NM), 194, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("제조 또는 사용 여부",         nameof(HIC_CHUKDTL_CHEMICAL.GBUSE),       72, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("사용 용도",                   nameof(HIC_CHUKDTL_CHEMICAL.USAGE),      130, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnNumber("월 취급량",             nameof(HIC_CHUKDTL_CHEMICAL.TREATMENT),   48, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnComboBox("단위",                nameof(HIC_CHUKDTL_CHEMICAL.UNIT),        52, IsReadOnly.N, TREATMENT_UNIT, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("비고",                        nameof(HIC_CHUKDTL_CHEMICAL.REMARK),      84, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("ROWID",                       nameof(HIC_CHUKDTL_CHEMICAL.RID),         74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 측정사업장 협력업체 List
            ssSUBLTD.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            //ssSUBLTD.AddColumnCheckBox("삭제", nameof(HIC_CHUKDTL_SUBLTD.IsDelete), 44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false });
            ssSUBLTD.AddColumn("일련번호",                   nameof(HIC_CHUKDTL_SUBLTD.WRTNO),        74, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("측정사업장",           nameof(HIC_CHUKDTL_SUBLTD.LTDCODE),      48, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("협력업체사업장코드",   nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDCODE),  88, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumn("협력업체명",                 nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDNAME), 308, new SpreadCellTypeOption { IsEditble = false });
            ssSUBLTD.AddColumn("비고",                       nameof(HIC_CHUKDTL_SUBLTD.REMARK),      264, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, TextMaxLength = 25 });
            ssSUBLTD.AddColumn("ROWID",                      nameof(HIC_CHUKDTL_SUBLTD.RID),          74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            if (!hCMN.IsNullOrEmpty())
            {
                Screen_Display(hCMN, ssPrt.ActiveSheet);
                Print_Page(ssPrt);
                Print_Page(ssGONG, "나. 작업환경측정대상 공정 및 유해인자별 측정계획", true);
                Print_Page(ssCHEMICAL, "다. 공정별 유해화학물질 사용 실태", true);
                Print_Page(ssSUBLTD, "라. 협력업체 현황", true);

                Application.DoEvents();
                this.Close();
                return;
            }
        }

        private void Print_Page(FpSpread ssPrt, string strTitle = "", bool ColVis = false)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (strTitle != "")
            {
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strHeader = sp.setSpdPrint_String("", null, clsSpread.enmSpdHAlign.Center, false, true);
            }
            
            strFooter = sp.setSpdPrint_String("", null, clsSpread.enmSpdHAlign.Center, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 40, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, ColVis, false, true, false, false, false, true);
            sp.setSpdPrint(ssPrt, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void Screen_Display(HIC_CHUKMST_NEW hCMN, SheetView aSht)
        {
            int nCol = 0;
            int nRow = 0;

            //측정년도
            aSht.Cells[4, 1].Text = hCMN.CHKYEAR;
            //측정반기
            if (hCMN.BANGI == "1") { aSht.Cells[4, 6].Value = true; }
            else { aSht.Cells[5, 6].Value = true; }
            //측정구분 (신규/기존)
            if (hCMN.GBNEW == "1") { aSht.Cells[4, 10].Value = true; }
            else { aSht.Cells[5, 10].Value = true; }
            //측정방법 (유선/방문)
            if (hCMN.GBWAY == "1") { aSht.Cells[4, 15].Value = true; }
            else { aSht.Cells[5, 15].Value = true; }
            //예비조사일자
            aSht.Cells[8, 10].Text = VB.Left(hCMN.BDATE, 4); aSht.Cells[8, 12].Text = VB.Mid(hCMN.BDATE, 6, 2); aSht.Cells[8, 15].Text = VB.Right(hCMN.BDATE, 2);
            //예비조사자
            List<HIC_CHUK_WORKER> lsthCW = hicChukWorkerService.GetListByWrtno(hCMN.WRTNO);
            if (lsthCW.Count > 0)
            {
                nRow = 8; nCol = 22;
                for (int i = 0; i < lsthCW.Count; i++)
                {
                    if (i > 2)
                    {
                        nRow += 1;
                        nCol = 22;
                    }

                    if (((FarPoint.Win.Spread.CellType.CheckBoxCellType)aSht.Cells[nRow, nCol].CellType).Caption.Trim() == lsthCW[i].WORKER_NAME.To<string>("").Trim())
                    {
                        aSht.Cells[nRow, nCol].Value = true;
                    }

                    nCol += 3;
                }
            }
            //사업장명
            aSht.Cells[11, 4].Text = hCMN.SANGHO;
            //대표자
            aSht.Cells[11, 22].Text = hCMN.DAEPYO;
            //우편번호
            aSht.Cells[12, 4].Text = hCMN.MAILCODE;
            //사업장주소
            aSht.Cells[12, 7].Text = hCMN.JUSO;
            //전화번호
            aSht.Cells[12, 22].Text = hCMN.TEL;
            //담당자
            aSht.Cells[13, 4].Text = hCMN.LTD_MANAGER + " " + hCMN.LTD_GRADE;
            //담당자 연락처
            aSht.Cells[13, 8].Text = hCMN.LTD_HPHONE;
            //FAX
            aSht.Cells[13, 22].Text = hCMN.LTD_FAX;
            //예상 작업환경측정일자
            aSht.Cells[15, 10].Text = hCMN.SDATE; aSht.Cells[15, 16].Text = hCMN.EDATE; aSht.Cells[15, 23].Text = hCMN.ILSU.To<string>("0");
            //근로자수
            aSht.Cells[18, 4].Text = "총 " + hCMN.INWON.To<string>("0") + " 명";
            aSht.Cells[18, 4].Text += "  (사무직 : " + hCMN.INWON_S.To<string>("0") + " 명,";
            aSht.Cells[18, 4].Text += " 현장직 : " + hCMN.INWON_H.To<string>("0") + " 명)";
            //근로형태
            if (hCMN.GBDAY == "1") { aSht.Cells[18, 21].Value = true; }
            aSht.Cells[18, 24].Text = hCMN.DAYTIME.To<string>("");
            if (hCMN.GBSHIFT == "1") { aSht.Cells[19, 21].Value = true; }
            aSht.Cells[19, 24].Text = hCMN.SHIFTGRPCNT.To<string>("") + "조 ";
            aSht.Cells[19, 24].Text += hCMN.SHIFTQUARTER.To<string>("") + "교대 ";
            aSht.Cells[19, 24].Text += hCMN.SHIFTTIME.To<string>("") + "시간";
            //근로시간
            aSht.Cells[20, 4].Text = hCMN.WORKTIME1 + "-" + hCMN.WORKTIME11;
            aSht.Cells[20, 11].Text = hCMN.WORKTIME2 + "-" + hCMN.WORKTIME22; 
            aSht.Cells[21, 4].Text = hCMN.WORKTIME3 + "-" + hCMN.WORKTIME33;
            aSht.Cells[21, 11].Text = hCMN.WORKTIME4 + "-" + hCMN.WORKTIME44;
            //식사시간, 휴식시간
            aSht.Cells[20, 21].Text = hCMN.MEALTIME1 + "-" + hCMN.MEALTIME11;
            aSht.Cells[21, 21].Text = hCMN.MEALTIME2 + "-" + hCMN.MEALTIME22; ;
            //협력업체 현황
            List<HIC_CHUKDTL_SUBLTD> lstHCS = hicChukDtlSubltdService.GetListByWrtno(hCMN.WRTNO);
            if (lstHCS.Count > 0)
            {
                nRow = 22; nCol = 4;
                for (int i = 0; i < lsthCW.Count; i++)
                {
                    if (i > 4)
                    {
                        nRow += 1;
                        nCol = 4;
                    }
                    aSht.Cells[nRow, nCol].Text = lstHCS[i].SUB_LTDNAME;
                    nCol += 4;
                }
            }
            //잔업유무
            if (hCMN.GBOVERTIME == "Y")
            {
                aSht.Cells[22, 21].Value = true;
                aSht.Cells[22, 23].Text = hCMN.OVERTIME;
            }
            else
            {
                aSht.Cells[23, 21].Value = true;
            }
            //공정흐름도
            aSht.Cells[25, 1].Text = hCMN.REMARK;
            //견적서요청
            if (hCMN.GBESTIMATE == "Y") { aSht.Cells[34, 4].Value = true; }
            else { aSht.Cells[34, 6].Value = true; }
            //노출기준보정
            if (hCMN.GBCORRECT == "Y") { aSht.Cells[34, 12].Value = true; }
            else { aSht.Cells[34, 14].Value = true; }
            //지역시료
            if (hCMN.GBSAMPLE == "Y") { aSht.Cells[34, 18].Value = true; }
            else { aSht.Cells[34, 20].Value = true; }
            //6가크롬(불)
            if (hCMN.GBCHROMIUM == "Y") { aSht.Cells[34, 26].Value = true; }
            else { aSht.Cells[34, 28].Value = true; }
            //과산화수소
            if (hCMN.GBUCODE1 == "1") { aSht.Cells[35, 1].Value = true; }
            else { aSht.Cells[35, 1].Value = false; }
            //시안화수소
            if (hCMN.GBUCODE2 == "1") { aSht.Cells[35, 6].Value = true; }
            else { aSht.Cells[35, 6].Value = false; }
            //무수프탈산
            if (hCMN.GBUCODE3 == "1") { aSht.Cells[35, 11].Value = true; }
            else { aSht.Cells[35, 11].Value = false; }
            //무수말레인
            if (hCMN.GBUCODE4 == "1") { aSht.Cells[35, 15].Value = true; }
            else { aSht.Cells[35, 15].Value = false; }
            //TDI, MDI
            if (hCMN.GBUCODE5 == "1") { aSht.Cells[35, 19].Value = true; }
            else { aSht.Cells[35, 19].Value = false; }
            //플루오라이드
            if (hCMN.GBUCODE6 == "1") { aSht.Cells[35, 24].Value = true; }
            else { aSht.Cells[35, 24].Value = false; }
            //예비조사 담당자
            aSht.Cells[41, 1].Text = " ";
            //사업주
            aSht.Cells[41, 12].Text = " ";

            //유해인자별 측정계획
            List<HIC_CHUKDTL_PLAN> lstHCU = hicChukDtlPlanService.GetListByWrtno(hCMN.WRTNO);
            ssGONG.DataSource = lstHCU;

            //화학물질 사용실태
            List<HIC_CHUKDTL_CHEMICAL> lstHCC = hicChukDtlChemicalService.GetListByWrtno(hCMN.WRTNO);
            ssCHEMICAL.DataSource = lstHCC;

            //협력업체 현황
            List<HIC_CHUKDTL_SUBLTD> lstHCSUB = hicChukDtlSubltdService.GetListByWrtno(hCMN.WRTNO);
            ssSUBLTD.DataSource = lstHCSUB;
        }

        private void Screen_Clear()
        {
            ssPrt_Clear(ssPrt.ActiveSheet);
        }

        private void ssPrt_Clear(SheetView aSht)
        {
            int nRow;

            nRow = 4;
            aSht.Cells[nRow, 1].Text = ""; aSht.Cells[nRow, 6].Value = false; aSht.Cells[nRow, 10].Value = false; aSht.Cells[nRow, 15].Value = false;
            nRow = 5;
            aSht.Cells[nRow, 6].Value = false; aSht.Cells[nRow, 10].Value = false; aSht.Cells[nRow, 15].Value = false;
            nRow = 8;
            aSht.Cells[nRow, 8].Text = ""; aSht.Cells[nRow, 12].Text = ""; aSht.Cells[nRow, 15].Text = ""; aSht.Cells[nRow, 22].Text = ""; aSht.Cells[nRow, 25].Text = ""; aSht.Cells[nRow, 28].Text = "";
            nRow = 9;
            aSht.Cells[nRow, 22].Text = ""; aSht.Cells[nRow, 25].Text = ""; aSht.Cells[nRow, 28].Text = "";
            nRow = 11;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 22].Text = "";
            nRow = 12;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 7].Text = ""; aSht.Cells[nRow, 22].Text = "";
            nRow = 13;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 8].Text = ""; aSht.Cells[nRow, 22].Text = "";
            nRow = 15;
            aSht.Cells[nRow, 10].Text = ""; aSht.Cells[nRow, 16].Text = ""; aSht.Cells[nRow, 23].Text = "";
            nRow = 18;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 21].Value = false; aSht.Cells[nRow, 24].Text = "";
            nRow = 19;
            aSht.Cells[nRow, 21].Value = false; aSht.Cells[nRow, 24].Text = "";
            nRow = 20;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 11].Text = ""; aSht.Cells[nRow, 21].Text = "";
            nRow = 21;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 11].Text = ""; aSht.Cells[nRow, 21].Text = "";
            nRow = 22;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 8].Text = ""; aSht.Cells[nRow, 12].Text = ""; aSht.Cells[nRow, 21].Value = false; aSht.Cells[nRow, 23].Text = "";
            nRow = 23;
            aSht.Cells[nRow, 4].Text = ""; aSht.Cells[nRow, 8].Text = ""; aSht.Cells[nRow, 12].Text = ""; aSht.Cells[nRow, 21].Value = false;
            nRow = 25;
            aSht.Cells[nRow, 1].Text = "";
            nRow = 34;
            aSht.Cells[nRow, 4].Value = false; aSht.Cells[nRow, 6].Value = false; aSht.Cells[nRow, 12].Value = false; aSht.Cells[nRow, 14].Value = false; aSht.Cells[nRow, 18].Value = false; aSht.Cells[nRow, 20].Value = false; aSht.Cells[nRow, 26].Value = false; aSht.Cells[nRow, 28].Value = false;
            nRow = 35;
            aSht.Cells[nRow, 1].Value = false; aSht.Cells[nRow, 6].Value = false; aSht.Cells[nRow, 11].Value = false; aSht.Cells[nRow, 15].Value = false; aSht.Cells[nRow, 19].Value = false; aSht.Cells[nRow, 24].Value = false;
            nRow = 41;
            aSht.Cells[nRow, 1].Text = ""; aSht.Cells[nRow, 4].CellType = null; aSht.Cells[nRow, 12].Text = ""; aSht.Cells[nRow, 16].CellType = null;
        }
    }
}
