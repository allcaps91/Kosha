using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcExamHelp.cs
/// Description     : 검사실 코드 찾기 / 매칭
/// Author          : 김민철
/// Create Date     : 2019-08-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcCode32.frm(FrmExamHelp)" />
namespace ComHpcLibB
{
    public partial class frmHcExamHelp : BaseForm
    {
        OcsOrdercodeService ocsOrdercodeService = null;
        OcsSubcodeService ocsSubcodeService = null;

        public frmHcExamHelp()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetEvents()
        {
            this.Load                += new EventHandler(eFormload);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSlip1.Click      += new EventHandler(eBtnClick);
            this.btnSlip2.Click      += new EventHandler(eBtnClick);
            this.btnSlip3.Click      += new EventHandler(eBtnClick);
            this.btnSlip4.Click      += new EventHandler(eBtnClick);
            this.btnSlip5.Click      += new EventHandler(eBtnClick);
            this.btnSlip6.Click      += new EventHandler(eBtnClick);
            this.btnSlip7.Click      += new EventHandler(eBtnClick);
            this.btnSlip8.Click      += new EventHandler(eBtnClick);
            this.btnSlip9.Click      += new EventHandler(eBtnClick);
            this.btnSlip10.Click     += new EventHandler(eBtnClick);
            this.btnSlip11.Click     += new EventHandler(eBtnClick);
            this.btnSlip12.Click     += new EventHandler(eBtnClick);
            this.btnSlip13.Click     += new EventHandler(eBtnClick);
            this.btnSlip14.Click     += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eFormload(object sender, EventArgs e)
        {
            clsPublic.GstrRetValue = "";
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strItemCd    = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                string strOrderCode = SS1.ActiveSheet.Cells[e.Row, 2].Text;
                string strSpecCode  = SS1.ActiveSheet.Cells[e.Row, 5].Text;
                
                if (strSpecCode == "2") //검체추가입력
                {
                    clsPublic.GstrRetValue = strItemCd;
                    this.Close();
                    return;
                }
                else if (strSpecCode != "1")
                {
                    clsPublic.GstrRetValue = strItemCd;
                    this.Close();
                    return;
                }
                else
                {
                    Display_Sub_List(strOrderCode);
                    return;
                }
            }
            else if (sender == SS2)
            {
                clsPublic.GstrRetValue = SS2.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
                return;
            }
        }

        private void Display_Sub_List(string strOrderCode)
        {
            int nRow = 0;
            string strNewData = string.Empty;
            string strOldData = string.Empty;
            string strItemCd = string.Empty;

            clsSpread cSpd = new clsSpread();

            cSpd.Spread_All_Clear(SS2);

            List<OCS_SUBCODE> list = ocsSubcodeService.FindSubCode(strOrderCode);

            for (int i = 0; i < list.Count; i++)
            {
                strNewData = list[i].SUBNAME;

                if (string.Compare(strOldData, strNewData) != 0)
                {
                    if (strOldData != "") { SS2.ActiveSheet.Cells[i, 1].Text = strItemCd; }

                    nRow = nRow + 1;
                    if (SS2.ActiveSheet.RowCount < nRow)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }
                    SS2.ActiveSheet.Cells[i, 0].Text = list[i].SUBNAME;

                    strItemCd = VB.Left(list[i].ITEMCD + VB.Space(10), 10);
                    strItemCd = strItemCd + VB.Left(list[i].CSUCODE + VB.Space(10), 10);
                    SS2.ActiveSheet.Cells[i, 1].Text = strItemCd;
                    strOldData = strNewData;
                }
                else
                {
                    strItemCd = strItemCd + VB.Left(list[i].ITEMCD + VB.Space(10), 10);
                    strItemCd = strItemCd + VB.Left(list[i].CSUCODE + VB.Space(10), 10);
                }
            }

            cSpd.Dispose();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (VB.Left(((Button)sender).Name.ToString(), 7) == "btnSlip")
            {
                string strSlipNo = string.Empty;
                string strHeaderName = string.Empty;

                switch (((Button)sender).Name.ToString())
                {
                    case "btnSlip1":  strSlipNo = "0010"; strHeaderName = "일 반 생 화 학";   break; 
                    case "btnSlip2":  strSlipNo = "0014"; strHeaderName = "혈액/골수/지혈";   break; 
                    case "btnSlip3":  strSlipNo = "0016"; strHeaderName = "소  변  검  사";   break; 
                    case "btnSlip4":  strSlipNo = "0018"; strHeaderName = "기생충.대변검사";  break; 
                    case "btnSlip5":  strSlipNo = "0022"; strHeaderName = "감염,면역혈청";    break; 
                    case "btnSlip6":  strSlipNo = "0024"; strHeaderName = "미 생 물 검 사";   break; 
                    case "btnSlip7":  strSlipNo = "0026"; strHeaderName = "체  액  검  사";   break; 
                    case "btnSlip8":  strSlipNo = "0028"; strHeaderName = "혈  액  은  행";   break; 
                    case "btnSlip9":  strSlipNo = "0030"; strHeaderName = "약물 특수 생화학"; break; 
                    case "btnSlip10": strSlipNo = "0032"; strHeaderName = "호르몬,암표지자";  break; 
                    case "btnSlip11": strSlipNo = "0034"; strHeaderName = "혈중 Allergen";    break; 
                    case "btnSlip12": strSlipNo = "0040"; strHeaderName = "병리 조직 검사";   break; 
                    case "btnSlip13": strSlipNo = "0042"; strHeaderName = "C Y T O L O G Y";  break; 
                    case "btnSlip14": strSlipNo = "0050"; strHeaderName = "외부 의뢰 검사";   break; 
                    default:
                        break;
                }
                SS1.ActiveSheet.Columns.Get(0).Font = new Font("굴림체", 10);
                SS1.ActiveSheet.Columns.Get(0).Label = strHeaderName;
                
                Display_List(strSlipNo);
            }
        }

        private void Display_List(string ArgSlipNo)
        {
            int nRow = 0;
            string strOrderName = string.Empty;
            string strGbInfo = string.Empty;

            clsSpread cSpd = new clsSpread();

            cSpd.Spread_All_Clear(SS1);
            
            List<OCS_ORDERCODE> list = ocsOrdercodeService.FindSlipNo(ArgSlipNo);

            for (int i = 0; i < list.Count; i++)
            {
                nRow = nRow + 1;
                if (SS1.ActiveSheet.RowCount < nRow)
                {
                    SS1.ActiveSheet.RowCount = nRow;
                }

                if (list[i].DISPSPACE > 0)
                {
                    strOrderName = Strings.Space((int)list[i].DISPSPACE) + list[i].ORDERNAME;
                }
                else
                {
                    strOrderName = list[i].ORDERNAME;
                }
                
                strGbInfo = list[i].GBINFO == "1" ? "▲ " : "   ";

                strOrderName = strGbInfo + strOrderName;

                SS1.ActiveSheet.Cells[i, 0].Text = strOrderName;
                SS1.ActiveSheet.Cells[i, 0].BackColor = list[i].GBINPUT == "0" ? Color.LightGray : Color.White;
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].ITEMCD;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].CORDERCODE;
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].DISPSPACE.ToString();
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].GBINFO;
                SS1.ActiveSheet.Cells[i, 5].Text = list[i].GBDOSAGE;
            }

            cSpd.Dispose();
        }

        private void SetControl()
        {
            
            ocsOrdercodeService = new OcsOrdercodeService();
            ocsSubcodeService = new OcsSubcodeService();

            SpreadCellTypeOption CTypeOot1 = null;
            SpreadCellTypeOption CTypeOot2 = null;

            CTypeOot1 = new SpreadCellTypeOption();
            CTypeOot1.IsEditble = true;
            CTypeOot1.Aligen = CellHorizontalAlignment.Left;

            CTypeOot2 = new SpreadCellTypeOption();
            CTypeOot2.IsVisivle = false;

            SS1.Initialize();
            SS1.AddColumn("검사명칭", nameof(OCS_ORDERCODE.ORDERNAME),  320, FpSpreadCellType.TextCellType, CTypeOot1);
            SS1.AddColumn("검사코드", nameof(OCS_ORDERCODE.ITEMCD),      64, FpSpreadCellType.TextCellType, CTypeOot2);
            SS1.AddColumn("오더코드", nameof(OCS_ORDERCODE.CORDERCODE),  64, FpSpreadCellType.TextCellType, CTypeOot2);
            SS1.AddColumn("공백여부", nameof(OCS_ORDERCODE.DISPSPACE),   32, FpSpreadCellType.TextCellType, CTypeOot2);
            SS1.AddColumn("항목구분", nameof(OCS_ORDERCODE.GBINFO),      32, FpSpreadCellType.TextCellType, CTypeOot2);
            SS1.AddColumn("추가입력", nameof(OCS_ORDERCODE.GBDOSAGE),    32, FpSpreadCellType.TextCellType, CTypeOot2);

            SS2.Initialize();
            SS2.AddColumn("선택항목", nameof(OCS_SUBCODE.SUBNAME), 320, FpSpreadCellType.TextCellType, CTypeOot1);
            SS2.AddColumn("검사코드", nameof(OCS_SUBCODE.ITEMCD),   64, FpSpreadCellType.TextCellType, CTypeOot2);
            SS2.AddColumn("수가코드", nameof(OCS_SUBCODE.CSUCODE),  64, FpSpreadCellType.TextCellType, CTypeOot2);

        }
    }
}
