using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using FarPoint.Win.Spread;
using System.Windows.Forms;
using ComBase;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaDrugList.cs
/// Description     : 종검 비상마약류 관리대장
/// Author          : 김민철
/// Create Date     : 2019-09-29
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmDrugListTO(FrmDrugListTO.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaDrugList : BaseForm
    {

        clsSpread sp = new clsSpread();

        OcsMayakJegoService ocsMayakJegoService = null;

        private double FnUnit = 0.0;

        public frmHaDrugList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {


            ocsMayakJegoService = new OcsMayakJegoService();

            SS1.Initialize();
            SS1.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SS1.AddColumn("코드",         nameof(OCS_MAYAK_JEGO.SUCODE),    84, FpSpreadCellType.TextCellType);
            SS1.AddColumn("비치수량",     nameof(OCS_MAYAK_JEGO.strQTY),    44, FpSpreadCellType.TextCellType);
            SS1.AddColumn("처방량",       "",                               44, FpSpreadCellType.TextCellType);
            SS1.AddColumn("약품명",       nameof(OCS_MAYAK_JEGO.HNAME),    240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("단위",         nameof(OCS_MAYAK_JEGO.UNIT),      74, FpSpreadCellType.TextCellType);
            SS1.AddColumn("ROWID",        nameof(OCS_MAYAK_JEGO.RID),       44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle  = false });

            SS2.Initialize();
            SS2.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SS2.AddColumn("순번",         "",                               40, FpSpreadCellType.TextCellType);
            SS2.AddColumn("구분",         "",                               100, FpSpreadCellType.TextCellType);
            SS2.AddColumn("처방일자",     nameof(OCS_MAYAK_JEGO.BDATE),     100, FpSpreadCellType.TextCellType);
            SS2.AddColumn("반환량",       nameof(OCS_MAYAK_JEGO.HNAME),    55, FpSpreadCellType.TextCellType);
            SS2.AddColumn("진료과",       nameof(OCS_MAYAK_JEGO.DEPTCODE),  55, FpSpreadCellType.TextCellType);
            SS2.AddColumn("병실",         nameof(OCS_MAYAK_JEGO.ROOMCODE),  44, FpSpreadCellType.TextCellType);
            SS2.AddColumn("환자명",       nameof(OCS_MAYAK_JEGO.SNAME),     74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("등록번호",     nameof(OCS_MAYAK_JEGO.PTNO),      74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("수가코드",     nameof(OCS_MAYAK_JEGO.SUCODE),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("처방량",       nameof(OCS_MAYAK_JEGO.REALQTY),   55, FpSpreadCellType.TextCellType);
            SS2.AddColumn("불출량",       nameof(OCS_MAYAK_JEGO.strQTY),      55, FpSpreadCellType.TextCellType);
            SS2.AddColumn("재고량",       nameof(OCS_MAYAK_JEGO.JQTY),      55, FpSpreadCellType.TextCellType);
            SS2.AddColumn("",   "",                               3, FpSpreadCellType.TextCellType);
            SS2.AddColumn("잔여량발생", "", 100, FpSpreadCellType.TextCellType);
            SS2.AddColumn("ORDERNO", "", 100, FpSpreadCellType.TextCellType);
            SS2.AddColumn("약제과확인", "", 44, FpSpreadCellType.TextCellType);
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormload);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnSet.Click           += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdClick);
            this.SS2.CellDoubleClick    += new CellClickEventHandler(eSpdClick);
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strBQty = "";

                if (string.Compare(dtpDate.Text, "2014-12-10") < 0)
                {
                    MessageBox.Show("비상마약류 관리대장은 2014-12-10일부터 조회가 가능합니다.", "오류");
                    return;
                }

                string strDcode = SS1.ActiveSheet.Cells[e.Row, 0].Text;
                string strDQty = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                string strDName = SS1.ActiveSheet.Cells[e.Row, 3].Text;
                string strUnit = SS1.ActiveSheet.Cells[e.Row, 4].Text;

                OCS_MAYAK_JEGO item = ocsMayakJegoService.GetCodeInfoItemBySucode(strDcode, dtpDate.Text);

                if (!item.IsNullOrEmpty())
                {
                    strBQty = item.BQTY;
                    FnUnit = item.UNITNEW1.To<double>();
                }

                txtDCode.Text = strDcode;
                txtDName.Text = strDName;
                txtDQty.Text = strDQty;
                txtBQty.Text = VB.Pstr(strBQty, "A", 1);
                txtUnit.Text = strUnit;

                READ_OCS_DURG(strDcode);
            }
            else if (sender == SS2)
            {
                //약제팀에서 해당 마약 삭제기능 추가 필요 (과거 이민영 데레사 수녀님이 하셨음)
                //약제팀 업무분장에 맞춰서 작업필요함
            }
            
        }
        private void SS2_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strGel1 = "";
            string strGel2 = "";
            string strGel3 = "";
            string strGel4 = "";
            string strGel5 = "";
            string strGel6 = "";
            string strGel7 = "";
            string strGel8 = "";
            string strGel9 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strGel1 = "┌─┬────────┬────────┐";
            strGel2 = "│  │마약류관리보조자│마약류관리보조자│";
            strGel3 = "│확│정 ( 진료과장 ) │부 ( 간호팀장 ) │";
            strGel4 = "│  ├────────┼────────┤";
            strGel5 = "│  │                │                │";
            strGel6 = "│인│                │                │";
            strGel7 = "│  │                │                │";
            strGel8 = "│  │                │                │";
            strGel9 = "└─┴────────┴────────┘";

            strTitle = "비상마약류 관리대장 (종검)";

            strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += sp.setSpdPrint_String(VB.Space(58) + strGel1, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(VB.Space(58) + strGel2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("일    자:" + dtpDate.Text + VB.Space(58), 58) + strGel3, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("약 품 명:" + txtDName.Text + VB.Space(58), 58) + strGel4, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("구    분:" + VB.IIf(rdoGB1.Checked = true, "마약", "향정신성의약품") + VB.Space(58), 58) + strGel5, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("약품코드:" + txtDCode.Text +VB.Space(58), 58) + strGel6, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("단    위:" + txtUnit.Text +VB.Space(58), 58) + strGel7, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("비치수량:" + txtBQty.Text + VB.Space(58), 58) + strGel8, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(ComFunc.LeftH("차    수: 1차수 " + clsPublic.GstrSysDate + clsPublic.GstrSysTime + VB.Space(58), 58) + strGel9, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += sp.setSpdPrint_String(VB.Space(8) + "확    인: 종합검진               약제팀", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            
            strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
            sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);


        }

            private void WORK_OCS_DRUG_SET()
        {
            int result = 0;
            string strSuCode = txtDCode.Text;
            int nQty = VB.Pstr(txtBQty.Text, "A", 1).To<int>();

            if (strSuCode.IsNullOrEmpty())
            {
                return;
            }

            string strGBn = rdoGB1.Checked ? "1" : "2";

            string strRowid = ocsMayakJegoService.GetOcsDrugSetRowidBySucode(strSuCode, dtpDate.Text);

            if (!strRowid.IsNullOrEmpty())
            {
                result = ocsMayakJegoService.UpdateOcsDrugSetQtyByRowid(nQty.To<string>(), nQty.To<string>() + "A", strRowid);
            }
            else
            {
                OCS_MAYAK_JEGO item = new OCS_MAYAK_JEGO
                {
                    JDATE = dtpDate.Text
                   ,GBN = strGBn
                   ,WARDCODE = "TO"
                   ,SUCODE = strSuCode
                   ,BQTY = nQty.To<string>() + "A"
                   ,QTY = nQty
                   ,UNIT = "A"
                };

                result = ocsMayakJegoService.InsertOcsDrugSet(item);
            }

            //당일재고 정보 갱신
            result = ocsMayakJegoService.UpdateOcsDrugSetQty(nQty.To<string>(), dtpDate.Text, strSuCode);

            eBtnClick(btnCancel, new EventArgs());
            eBtnClick(btnSearch, new EventArgs());

        }

        private void READ_OCS_DURG(string strDcode)
        {
            int nRow = 0;     
            double nQty  = 0;     
            double nJQty = 0;    
            double niQty = 0;    
            double nJanQty  = 0.0;
            double nJegoQty = 0.0; 
            int nChasu = 0;  
            double nJQtyT = 0;
            double nSQtyT = 0;  
            double nEntQty2 = 0.0;
            double nRealQty = 0.0;
            double nRealJeagoQty = 0.0;

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 5;

            nChasu = 1;
            nEntQty2 = 0; nRealQty = 0;
            nJQtyT = 0; nSQtyT = 0;

            nRow = 0;
            nQty = 0;
            nJQty = 0;
            nRealJeagoQty = 0;

            string strGbn = rdoGB1.Checked == true ? "1" : "2";

            //당일재고 -------------------------------------------------------------------------------------------
            List<OCS_MAYAK_JEGO> items1 = ocsMayakJegoService.GetDayJegoBySucodeDate(strDcode, dtpDate.Text, strGbn);

            SS2.DataSource = items1;
            
            if (items1.Count == 0)
            {
                SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + 1;
            }
            else
            {
                SS2.ActiveSheet.Cells[0, 11].Text = items1[0].strQTY + " ";
                nJQty = nJQty + items1[0].strQTY.To<int>();
            }
            
            SS2.ActiveSheet.Cells[0, 1].Text = "당일비치량";
            
            nJegoQty = nJQty;
            nRealJeagoQty = nJQty;

            //소모 ----------------------------------------------------------------------------------------------
            List<OCS_MAYAK_JEGO> items2 = ocsMayakJegoService.GetDayUsedBySucodeDate(strDcode, dtpDate.Text, strGbn, nChasu, "2");

            if (items2.Count > 0)
            {
                nRow = SS2.ActiveSheet.RowCount;
                SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + items2.Count;


                for (int i = 0; i < items2.Count; i++)
                {
                    SS2.ActiveSheet.Rows[nRow].Height = 30;
                    SS2.ActiveSheet.Cells[nRow, 0].Text = (i + 1).To<string>();
                    SS2.ActiveSheet.Cells[nRow, 1].Text = items2[i].strQTY.To<int>() > 0 ? "소모" : "반환";
                    SS2.ActiveSheet.Cells[nRow, 2].Text = items2[i].BDATE;
                    SS2.ActiveSheet.Cells[nRow, 4].Text = items2[i].DEPTCODE;
                    SS2.ActiveSheet.Cells[nRow, 5].Text = items2[i].IO == "I" ? items2[i].ROOMCODE : (items2[i].DEPTCODE == "HR" || items2[i].DEPTCODE == "TO") ? "종검" : "외래";
                    SS2.ActiveSheet.Cells[nRow, 6].Text = items2[i].SNAME;
                    SS2.ActiveSheet.Cells[nRow, 7].Text = items2[i].PTNO;
                    SS2.ActiveSheet.Cells[nRow, 8].Text = items2[i].SUCODER;
                    SS2.ActiveSheet.Cells[nRow, 9].Text = items2[i].REALQTY.To<string>();
                    SS2.ActiveSheet.Cells[nRow, 10].Text = items2[i].strQTY.To<string>();

                    nQty = nQty + items2[i].strQTY.To<int>();

                    if (items2[i].strQTY.To<int>() < 0)
                    {
                        nJegoQty = nJegoQty - VB.Fix((items2[i].strQTY.To<int>() * 10 - 9) / 10);
                    }
                    else
                    {
                        nJegoQty = nJegoQty - VB.Fix((items2[i].strQTY.To<int>() * 10 + 9) / 10);
                    }

                    SS2.ActiveSheet.Cells[nRow, 11].Text = nJegoQty.To<string>() + " ";
                    nJanQty = items2[i].JQTY;

                    if (nJanQty != 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 13].Text = nJanQty.To<string>() + " ";
                    }

                    SS2.ActiveSheet.Cells[nRow, 14].Text = " ";

                    nRow = nRow + 1;
                }
            }

            SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + 1;
            SS2.ActiveSheet.Rows[nRow].Height = 30;
            SS2.ActiveSheet.Cells[nRow, 1].Text = "소 모 합 계";
            SS2.ActiveSheet.Cells[nRow, 10].Text = nQty.To<string>() + " ";

            //입고 -------------------------------------------------------------------------------------------------------
            List<OCS_MAYAK_JEGO> items3 = ocsMayakJegoService.GetDayUsedBySucodeDate(strDcode, dtpDate.Text, strGbn, 0 , "1");

            nRow = SS2.ActiveSheet.RowCount;
            SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + 1;

            if (items3.Count > 0)
            {
                SS2.ActiveSheet.Cells[nRow, 2].Text = dtpDate.Text;
                nJQty = nJQty + items1[0].QTY.To<int>();
            }

            SS2.ActiveSheet.Rows[nRow].Height = 30;
            SS2.ActiveSheet.Cells[nRow, 1].Text = "당일반환량";
            SS2.ActiveSheet.Cells[nRow, 3].Text = (nRealJeagoQty - nQty).To<string>();
            SS2.ActiveSheet.Cells[nRow, 11].Text = "0";

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Display_List();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSet)
            {
                WORK_OCS_DRUG_SET();
            }
            else if (sender == btnPrint)
            {
                SS2_Print();
            }

            
        }

        private void Screen_Clear()
        {
            SS1.ActiveSheet.ClearRange(0, 0, SS1.ActiveSheet.Rows.Count, SS1.ActiveSheet.ColumnCount, true);
            SS1.ActiveSheet.Rows.Count = 0;

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 0;

            txtDCode.Text = "";
            txtDName.Text = "";
            txtDQty.Text = "";
            txtBQty.Text = "";
            txtUnit.Text = "";
        }

        private void eFormload(object sender, EventArgs e)
        {
            dtpDate.Text = DateTime.Now.ToShortDateString();
        }

        private void Display_List()
        {
            string strGbn = "";
            string strSucde = "";
            strGbn = rdoGB1.Checked == true ? "1" : "2";

            SS1.ActiveSheet.ClearRange(0, 0, SS1.ActiveSheet.Rows.Count, SS1.ActiveSheet.ColumnCount, true);
            SS1.ActiveSheet.Rows.Count = 5;

            SS1.DataSource = ocsMayakJegoService.GetListByDate(dtpDate.Text, strGbn);

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strSucde = SS1.ActiveSheet.Cells[i, 0].Text.Trim();

                string sQty = ocsMayakJegoService.GetDrugQtyBySucode(strSucde, dtpDate.Text);

                if (!sQty.IsNullOrEmpty())
                {
                    if (sQty.To<int>() > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 2].Text = sQty;
                        SS1.ActiveSheet.Cells[i, 2].BackColor = System.Drawing.Color.LightSalmon;
                    }
                }
                
            }

            

        }
    }
}
