using ComBase;
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
/// File Name       : frmHcPanYearlungcapacityExamCnt.cs
/// Description     : 연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황
/// Author          : 이상훈
/// Create Date     : 2019-11-15
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm연도별폐활량검사대상자수.frm(Frm연도별폐활량검사대상자수)" />
namespace HC_Pan
{
    public partial class frmHcPanYearlungcapacityExamCnt : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HicResultService hicResultService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanYearlungcapacityExamCnt()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            hicResultService = new HicResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYear = 0;

            progressBar1.Value = 0;
            lblRate.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYear = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            cboYear.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                cboYear.Items.Add(nYear + "년도");
                nYear -= 1;
            }
            cboYear.SelectedIndex = 0;
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

                strTitle = "연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnSearch)
            {
                int j = 0;
                double nREAD = 0;
                long nYEAR = 0;
                string[] strYear = new string[3];
                long nWRTNO = 0;
                string strJepDate = "";
                double nRate = 0;
                double[,] nTotCnt = new double[3, 13];

                string strFrDate = "";
                string strToDate = "";

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 3;
                SS1_Sheet1.Rows.Get(-1).Height = 29;

                Application.DoEvents();

                btnSearch.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                nYEAR = int.Parse(VB.Left(cboYear.Text, 4));

                for (int i = 0; i < 3; i++)
                {
                    strYear[i] = string.Format("{0:0000}", nYEAR);
                    nYEAR -= 1;
                    for (int k = 0; k < 13; k++)
                    {
                        nTotCnt[i, k] = 0;
                    }
                }

                strFrDate = strYear[2] + "-01-01";
                strToDate = strYear[0] + "-12-31";

                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItembyJepDateExCode(strFrDate, strToDate, "TR11");
                
                nREAD = list.Count;
                progressBar1.Maximum = (int)nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strJepDate = list[i].JEPDATE;
                    if (VB.Left(strJepDate, 4) == strYear[0])
                    {
                        nYEAR = 0;
                    }
                    else if (VB.Left(strJepDate, 4) == strYear[1])
                    {
                        nYEAR = 1;
                    }
                    else
                    {
                        nYEAR = 2;
                    }

                    switch (list[i].RESULT.Trim())
                    {
                        case "01":
                            j = 0;
                            break;
                        case "02":
                        case "03":
                        case "04":
                        case "12":
                            j = 3;  //제한성
                            break;
                        case "05":
                        case "06":
                        case "07":
                        case "13":
                            j = 5;  //폐쇄성
                            break;
                        default:
                            j = 7;  //혼합성
                            break;
                    }

                    nTotCnt[nYEAR, 0] += 1;  //대상자수
                    if (j > 0)
                    {
                        nTotCnt[nYEAR, 1] += 1;  //이상자수
                        nTotCnt[nYEAR, j] += 1;  //이상자수
                    }

                    //BMI 결과
                    string[] strExCode = { "A117" };
                    HIC_RESULT list2 = hicResultService.GetResultByWrtNoExCode(nWRTNO, strExCode);

                    if (list2 != null)
                    {
                        if (string.Compare(list2.RESULT, "30") >= 0)
                        {
                            nTotCnt[nYEAR, 9] += 1;          //BMI 30이상
                            if (j > 0)
                            {
                                nTotCnt[nYEAR, 10] += 1;    //이상자중 BMI30이상
                            }
                        }
                    }
                    nRate = Math.Round(cf.FIX_N(i + 1) / nREAD * 100);
                    lblRate.Text = nRate.ToString() + " %";
                    progressBar1.Value = i + 1;
                }

                for (int i = 0; i < 3; i++)
                {
                    if (nTotCnt[i, 0] > 0) nTotCnt[i, 2] = nTotCnt[i, 1] / nTotCnt[i, 0] * 100;
                    if (nTotCnt[i, 1] > 0) nTotCnt[i, 4] = nTotCnt[i, 3] / nTotCnt[i, 1] * 100;
                    if (nTotCnt[i, 1] > 0) nTotCnt[i, 6] = nTotCnt[i, 5] / nTotCnt[i, 1] * 100;
                    if (nTotCnt[i, 1] > 0) nTotCnt[i, 8] = nTotCnt[i, 7] / nTotCnt[i, 1] * 100;
                    if (nTotCnt[i, 9] > 0) nTotCnt[i, 11] = nTotCnt[i, 10] / nTotCnt[i, 9] * 100;
                    if (nTotCnt[i, 1] > 0) nTotCnt[i, 12] = nTotCnt[i, 9] / nTotCnt[i, 1] * 100;

                    SS1.ActiveSheet.Cells[i, 0].Text = strYear[i];
                    for (int k = 0; k < 13; k++)
                    {
                        SS1.ActiveSheet.Cells[i, k + 1].Text = string.Format("{0:N0}", nTotCnt[i, k]);
                        Application.DoEvents();
                    }
                }
                btnSearch.Enabled = false;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
