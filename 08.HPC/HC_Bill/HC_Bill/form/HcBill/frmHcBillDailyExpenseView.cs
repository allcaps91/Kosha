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
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillDailyExpenseView.cs
/// Description     : 일자별 청구금액 조회
/// Author          : 이상훈
/// Create Date     : 2021-01-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcBill24.frm(FrmChkList)" />

namespace HC_Bill
{
    public partial class frmHcBillDailyExpenseView : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicSunapdtlService hicSunapdtlService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        public frmHcBillDailyExpenseView()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicSunapdtlService = new HicSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtSabun.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            sp.Spread_All_Clear(SS1);

            SS1_Sheet1.Rows[-1].Height = 20;
            SS1_Sheet1.Columns[12].Visible = false;
            SS1_Sheet1.Columns[13].Visible = false;

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;

            cboJohap.Items.Clear();
            cboJohap.Items.Add("전체");
            cboJohap.Items.Add("사업장");
            cboJohap.Items.Add("공무원");
            cboJohap.Items.Add("성인병");
            cboJohap.Items.Add("통합");
            cboJohap.SelectedIndex = 0;

            cboJong.Items.Clear();
            cboJong.Items.Add("*.전체");
            cboJong.Items.Add("1.건강검진");
            cboJong.Items.Add("3.구강검진");
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("5.보건소암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strTDate = "";
                string strFDate = "";
                string strNewData = "";
                string strOldData = "";
                long[,] nTotAmt = new long[3, 6];
                int nRow = 0;
                long nMirno = 0;
                string strGubun = "";
                long nQty = 0; //1차건수
                long nTotAmt1 = 0; //수납액
                long nSunapAmt = 0;//수납금액
                long nChaAmt = 0;//차액
                long nDentCnt1 = 0;//구강건수
                long nDentCnt2 = 0;//구강건수(공휴)
                long nDntAmt = 0; //구강수가
                string str직역구분 = "";
                string strJong = "";
                string strSabun = "";
                string strLife = "";

                switch (cboJohap.Text)
                {
                    case "전체":
                        str직역구분 = "*";
                        break;
                    case "사업장":
                        str직역구분 = "K";
                        break;
                    case "공무원":
                        str직역구분 = "G";
                        break;
                    case "성인병":
                        str직역구분 = "J";
                        break;
                    case "통합":
                        str직역구분 = "T";
                        break;
                    default:
                        break;
                }

                for (int i = 0; i <= 2; i++)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        nTotAmt[i, j] = 0;
                    }
                }

                nQty = 0;
                sp.Spread_All_Clear(SS1);

                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;

                strJong = VB.Left(cboJong.Text, 1);
                strSabun = txtSabun.Text.Trim();

                if (chkLife.Checked == true)
                {
                    strLife = "Y";
                }
                else
                {
                    strLife = "";
                }

                List<COMHPC> list = comHpcLibBService.GetExpenseItembyJepDateJohap(strJong, strFDate, strTDate, strSabun, str직역구분, strLife);

                SS1.ActiveSheet.RowCount = list.Count;
                if (list.Count == 0)
                {
                    MessageBox.Show("자료가 1건도 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                nREAD = list.Count;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nMirno = list[i].MIRNO;
                    strNewData = list[i].GUBUN;
                    strNewData += nMirno;
                    strGubun = list[i].GUBUN;

                    //건진년도별 구강금액
                    hb.SET_Dental_Amt(VB.Left(list[i].FRDATE, 4));

                    //------------------------------------------
                    //    수납금액,건수,구강건수를 구함
                    //------------------------------------------
                    COMHPC list2 = comHpcLibBService.GetSunapAmtbyJepDateMirNo(strGubun, list[i].FRDATE, list[i].TODATE, list[i].MIRNO, strLife);

                    if (!list2.IsNullOrEmpty())
                    {
                        nQty = list2.CNT;
                        nSunapAmt = list2.AMT;
                        nDentCnt1 = list2.DENCNT;
                    }

                    //-------------------------------------
                    //    공휴 구강건수를 구함
                    //-------------------------------------
                    nDentCnt2 = hicSunapdtlService.GetCountbyInWrtNoJepDate(strGubun, list[i].FRDATE, list[i].TODATE, list[i].MIRNO);

                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    if (!strOldData.IsNullOrEmpty())
                    {
                        strOldData = strNewData;
                        switch (list[i].GUBUN)
                        {
                            case "1":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "건강검진";
                                break;
                            case "2":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "구강검진";
                                break;
                            case "3":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "공단암";
                                break;
                            case "4":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "보건소암";
                                break;
                            case "5":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "의료급여";
                                break;
                            default:
                                break;
                        }
                    }
                    else if (VB.Left(strOldData, 1) != VB.Left(strNewData, 1))
                    {
                        //GoSub SubTotal_Display
                        //=====================================================================================================
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "건수소계";
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = nTotAmt[1, 5].To<string>();
                        nTotAmt[1, 5] = 0;
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                        for (int j = 1; j <= 4; j++)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, j - 1 + 8].Text = string.Format("{0:###,###,###,##0}", nTotAmt[1, j]);
                            nTotAmt[1, j] = 0;
                        }
                        //=====================================================================================================

                        strOldData = strNewData;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }

                        switch (list[i].GUBUN)
                        {
                            case "1":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "건강검진";
                                break;
                            case "2":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "구강검진";
                                break;
                            case "3":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "공단암";
                                break;
                            case "4":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "보건소암";
                                break;
                            case "5":
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "의료급여";
                                break;
                            default:
                                break;
                        }
                    }
                    else if (strOldData != strNewData)
                    {
                        strOldData = strNewData;
                    }

                    //청구금액과 건수
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].MIRNO.To<string>();
                    switch (list[i].JOHAP)
                    {
                        case "K":
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "직장";
                            break;
                        case "G":
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "공교";
                            break;
                        case "J":
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "지역";
                            break;
                        case "X":
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "급여";
                            break;
                        default:
                            break;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPQTY.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].FRDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].TODATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = string.Format("{0:###,###,###,##0}", list[i].TAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = string.Format("{0:###,###,###,##0}", list[i].ONE_TAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = string.Format("{0:###,###,###,##0}", list[i].TWO_TAMT);

                    if (strGubun == "1")
                    {
                        //수납집계표, 1차 건수
                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nQty.To<string>();       //1차건수
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = string.Format("{0:###,###,###,##0}", nSunapAmt);  //수납액
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = string.Format("{0:###,###,###,##0}", list[i].TAMT - (nSunapAmt - (nDentCnt1 * clsHcVariable.GnDentAmt) - (nDentCnt2 * clsHcVariable.GnDentAddAmt))); //차액
                        nChaAmt = SS1.ActiveSheet.Cells[nRow - 1, 11].Text.Replace(",", "").To<long>();                        
                    }
                    else if (strGubun == "2")
                    {
                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nQty.To<string>();       //1차건수
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = (nDentCnt1 * clsHcVariable.GnDentAmt) + (nDentCnt2 * clsHcVariable.GnDentAddAmt).To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = string.Format("{0:###,###,###,##0}", list[i].TAMT - (nDentCnt1 * clsHcVariable.GnDentAmt) - (nDentCnt2 * clsHcVariable.GnDentAddAmt));  //차액
                        nChaAmt = SS1.ActiveSheet.Cells[nRow - 1, 11].Text.Replace(",", "").To<long>();
                    }
                    else if (strGubun == "4")
                    {
                        //수납집계표, 1차 건수
                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nQty.To<string>();       //1차건수
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = string.Format("{0:###,###,###,##0}", nTotAmt1);   //수납액
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = string.Format("{0:###,###,###,##0}", list[i].TAMT - (nSunapAmt - (nDentCnt1 * clsHcVariable.GnDentAmt) - (nDentCnt2 * clsHcVariable.GnDentAddAmt))); //차액
                        nChaAmt = SS1.ActiveSheet.Cells[nRow - 1, 11].Text.Replace(",", "").To<long>();
                    }
                    else 
                    {
                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = string.Format("{0:###,###,###,##0}", nSunapAmt);       //수납액
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = string.Format("{0:###,###,###,##0}", (list[i].TAMT - nSunapAmt));
                        nChaAmt = SS1.ActiveSheet.Cells[nRow - 1, 11].Text.Replace(",", "").To<long>();
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 17].Text = list[i].NHICNO;

                    //소계에 금액을 Add
                    nTotAmt[1, 1] += list[i].TAMT;
                    nTotAmt[1, 2] += list[i].ONE_TAMT;
                    nTotAmt[1, 3] += list[i].TWO_TAMT;
                    nTotAmt[1, 4] += nChaAmt;
                    nTotAmt[1, 5] += list[i].JEPQTY;

                    //합계에 금액을 Add
                    nTotAmt[2, 1] += list[i].TAMT;
                    nTotAmt[2, 2] += list[i].ONE_TAMT;
                    nTotAmt[2, 3] += list[i].TWO_TAMT;
                    nTotAmt[2, 4] += nChaAmt;
                    nTotAmt[2, 5] += list[i].JEPQTY;

                    progressBar1.Value = i + 1;
                }

                nRow += 2;
                SS1.ActiveSheet.RowCount = nRow;
                //GoSub Total_Display
                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "건수합계";
                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = string.Format("{0:###,###,###,##0}", nTotAmt[2, 5]);
                nTotAmt[2, 5] = 0;
                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "합계";
                for (int j= 1; j <= 4; j++)
                {
                    SS1.ActiveSheet.Cells[nRow - 1, j - 1 + 8].Text = string.Format("{0:###,###,###,##0}", nTotAmt[2, j]);
                    nTotAmt[2, j] = 0;
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                if (chkLife.Checked == false)
                {
                    strTitle = "건진 청구 금액 조회 (" + VB.Pstr(cboJong.Text, ".", 2) + ")";
                }
                else
                {
                    strTitle = "건진 청구 금액 조회 (" + VB.Pstr(cboJong.Text, ".", 2) + ") - 생애";
                }

                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strHeader += sp.setSpdPrint_String("┌─┬────┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("│  ├────┼────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("│재│        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("└─┴────┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("      작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("      인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  Page: /p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strMirDate = "";

                strMirDate = SS1.ActiveSheet.Cells[e.Row, 14].Text;

                if (e.Column == 14)
                {
                    for (int i = 1; i < SS1.ActiveSheet.RowCount - 2; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 14].Text = strMirDate;
                    }
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtSabun)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSabun.Text.Trim() != "")
                    {
                        lblName.Text = hb.READ_HIC_InsaName(txtSabun.Text);
                    }
                    else
                    {
                        lblName.Text = "";
                    }
                    SendKeys.Send("{Tab}");
                }
            }
        }
    }
}
