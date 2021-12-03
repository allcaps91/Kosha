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
/// Class Name      : HC_Pan
/// File Name       : frmHcPanDrSpcExamJudgmentCnt.cs
/// Description     : 의사별 특수검진 판정 인원수
/// Author          : 이상훈
/// Create Date     : 2019-11-15
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm의사별특수검진판정인원수.frm(Frm의사별특수검진판정인원수)" />

namespace HC_Pan
{
    public partial class frmHcPanDrSpcExamJudgmentCnt : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HicResultService hicResultService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanDrSpcExamJudgmentCnt()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            hicResultService = new HicResultService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFrDate.Text = long.Parse(VB.Left(clsPublic.GstrSysDate, 4)) - 1 + "-01-01";
            dtpToDate.Text = long.Parse(VB.Left(clsPublic.GstrSysDate, 4)) - 1 + "-12-31";
            btnPrint.Enabled = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strOLD = "";
                string strNew = "";
                int nCNT1 = 0;
                int nCNT2 = 0;
                string strFrDate = "";
                string strToDate = "";
                string strGjYear = "";

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strGjYear = VB.Left(dtpFrDate.Text, 4);

                //-------------------------------------
                //   특수검진 의사+회사별 인원수
                //-------------------------------------
                List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyJepDateGjYearPanDrNo(strFrDate, strToDate, strGjYear);

                nREAD = list.Count;
                //SS1.ActiveSheet.RowCount = nREAD;
                strOLD = "";
                nCNT1 = 0;
                nCNT2 = 0;
                nRow = 0;

                for (int i = 0; i < nREAD; i++)
                {
                    strNew = list[i].PANJENGDRNO.ToString();
                    if (strOLD != strNew)
                    {   
                        if (!strOLD.IsNullOrEmpty() && nCNT1 > 0)
                        {
                            nRow += 1;
                            if (nRow > SS1.ActiveSheet.RowCount)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_License_DrName(long.Parse(strOLD));
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:###,###,###}", nCNT1);
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:###,###,###}", nCNT2);
                        }
                        strOLD = strNew;
                        nCNT1 = 0;
                        nCNT2 = 0;
                    }
                    nCNT1 += 1;
                    nCNT2 += (int)list[i].CNT;
                }

                if (nCNT1 > 0)
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }
                    SS1.ActiveSheet.RowCount += 1;
                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_License_DrName(long.Parse(strOLD));
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:###,###,###}", nCNT1);
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:###,###,###}", nCNT2);
                }

                //사업장별 인원수를 계산
                List<HIC_JEPSU_RES_SPECIAL> list2 = hicJepsuResSpecialService.GetItembyJepDateGjYear(strFrDate, strToDate, strGjYear);

                nREAD = list2.Count;
                strOLD = "";
                nCNT1 = 0;
                nCNT2 = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    nCNT1 += 1;
                    nCNT2 += (int)list2[i].CNT;
                }

                nRow += 1;
                SS1.ActiveSheet.RowCount = nRow;
                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "** 합계 **";
                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:###,###,###}", nCNT1);
                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:###,###,###}", nCNT2);

                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;
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

                strTitle = "의사별 특수검진 판정 인원수";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진일자:" + dtpFrDate.Text + " 부터 " + dtpToDate.Text + " 일까지" + "\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
        }
    }
}
