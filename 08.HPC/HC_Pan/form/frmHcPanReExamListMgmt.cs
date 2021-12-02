using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanReExamListMgmt.cs
/// Description     : 2차검진 대상자 명단 관리
/// Author          : 이상훈
/// Create Date     : 2019-12-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm재검대상자관리.frm(Frm재검대상자관리)" />

namespace HC_Pan
{
    public partial class frmHcPanReExamListMgmt : Form
    {
        HicJepsuExjongPatientService hicJepsuExjongPatientService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        PrintDocument pd;

        public frmHcPanReExamListMgmt()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuExjongPatientService = new HicJepsuExjongPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrt1.Click += new EventHandler(eBtnClick);
            this.btnPrt2.Click += new EventHandler(eBtnClick);
            this.btnPrt3.Click += new EventHandler(eBtnClick);            
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1.ActiveSheet.Rows[-1].Height = 20;
            txtLtdCode.Text = "";

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-90).ToShortDateString();
            dtpToDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();

            cboJob.Items.Add("1.사업장");
            cboJob.Items.Add("2.공무원");
            cboJob.Items.Add("3.성인병");
            cboJob.Items.Add("4.채용및배치전");
            cboJob.Items.Add("5.기타");
            cboJob.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnPrt1)
            {
                string strPrintName = "";
                string strPrtSetOK = "NO";

                //PrintDocument pd;

                strPrintName = CP.getPrinter_Chk("봉투인쇄");

                if (strPrintName.IsNullOrEmpty())
                {
                    strPrtSetOK = "OK";
                }

                if (strPrtSetOK == "NO")
                {
                    return;
                }

                pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "봉투인쇄";
                pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("A3", 50, 30);

                pd.PrintPage += new PrintPageEventHandler(ePrint);
            }
            else if (sender == btnPrt2)
            {
                string strSname = "";
                string strSayu = "";
                int nCNT = 0;
                int nMaxRow = 0;
                int nPrint = 0;
                string strPrtSetOK = "";
                string strOK = "";

                string strFont1 = "";
                string strFont2 = "";
                string strHead1 = "";

                //마지막 row찾기
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strOK = "";
                    if (rdoPrt1.Checked == true)
                    {
                        strOK = "OK";
                        nMaxRow = i + 1;
                    }
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True" && rdoPrt2.Checked == true)
                    {
                        strOK = "OK";
                        nMaxRow = i + 1;
                    }
                    if (SS1.ActiveSheet.Cells[i, 0].Text.IsNullOrEmpty() && rdoPrt3.Checked == true)
                    {
                        strOK = "OK";
                        nMaxRow = i + 1;
                    }
                }

                SS2_Sheet_Clear();

                nCNT = 0;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strOK = "";
                    if (rdoPrt1.Checked == true)
                    {
                        strOK = "OK";                        
                    }

                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True" && rdoPrt2.Checked == true)
                    {
                        strOK = "OK";
                    }

                    if (SS1.ActiveSheet.Cells[i, 0].Text.IsNullOrEmpty() && rdoPrt3.Checked == true)
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        nCNT += 1;

                        strSname = "대상자명 : " + SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        strSname = "검진사유 : " + SS1.ActiveSheet.Cells[i, 9].Text.Trim();

                        switch (nCNT)
                        {
                            case 1:
                                SS2.ActiveSheet.Cells[3, 2].Text = strSname;
                                SS2.ActiveSheet.Cells[4, 2].Text = strSayu;
                                break;
                            case 2:
                                SS2.ActiveSheet.Cells[17, 2].Text = strSname;
                                SS2.ActiveSheet.Cells[18, 2].Text = strSayu;
                                break;
                            default:
                                break;
                        }

                        if (nCNT >= 2 || i == nMaxRow)
                        {
                            if ( i == nMaxRow)
                            {
                                switch (nCNT)
                                {
                                    case 1:
                                        for (int j = 14; j < 28; j++)
                                        {
                                            SS2_Sheet1.Rows.Get(j).Visible = false;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            SS2_Sheet1.PrintInfo.Header = "";
                            SS2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                            SS2_Sheet1.PrintInfo.Margin.Top = 50;
                            SS2_Sheet1.PrintInfo.Margin.Bottom = 50;
                            SS2_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
                            SS2_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
                            SS2_Sheet1.PrintInfo.ShowBorder = true;
                            SS2_Sheet1.PrintInfo.ShowColor = false;
                            SS2_Sheet1.PrintInfo.ShowGrid = true;
                            SS2_Sheet1.PrintInfo.ShowShadows = false;
                            SS2_Sheet1.PrintInfo.UseMax = false;
                            SS2_Sheet1.PrintInfo.PrintType = PrintType.All;
                            SS2.PrintSheet(0);

                            if (i == nMaxRow)
                            {
                                switch (nCNT)
                                {
                                    case 1:
                                        for (int j = 14; j < 28; j++)
                                        {
                                            SS2_Sheet1.Rows.Get(j).Visible = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            nCNT = 0;
                            SS2_Sheet_Clear();
                        }
                    }
                }
            }
            else if (sender == btnPrt3)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                SS1_Sheet1.Rows.Get(0).Visible = false;
                SS1_Sheet1.Rows.Get(4).Visible = false;
                SS1_Sheet1.Rows.Get(8).Visible = false;

                for (int i = 10; i <= 16; i++)
                {
                    SS1_Sheet1.Rows.Get(i).Visible = false;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "2차검진 대상자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간: " + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("맑은 고딕", 9), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진종류: " + cboJob.Text, new Font("맑은 고딕", 9), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진회사: " + VB.Pstr(txtLtdCode.Text, ".", 2), new Font("맑은 고딕", 9), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("맑은 고딕", 9), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                SS1_Sheet1.Rows.Get(0).Visible = true;
                SS1_Sheet1.Rows.Get(4).Visible = true;
                SS1_Sheet1.Rows.Get(8).Visible = true;

                for (int i = 10; i <= 16; i++)
                {
                    SS1_Sheet1.Rows.Get(i).Visible = true;
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                int nCNT = 0;
                string strGbSTS = "";
                string strGjJong = "";
                string StrJumin = "";
                string strOK = "";

                bool bGbAm = false;         //암검사 여부
                bool bGbAmMunjin = false;   //암검사 문진입력 여부

                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                string strTong = "";
                string strJob = "";

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                if (rdoTombo1.Checked == true)
                {
                    strTong = "1";
                }
                else if (rdoTombo2.Checked == true)
                {
                    strTong = "2";
                }
                else if (rdoTombo3.Checked == true)
                {
                    strTong = "3";
                }

                switch (VB.Left(cboJob.Text, 1))
                {
                    case "1":
                        strJob = "1";
                        break;
                    case "2":
                        strJob = "2";
                        break;
                    case "3":
                        strJob = "3";
                        break;
                    case "4":
                        strJob = "4";
                        break;
                    case "5":
                        strJob = "5";
                        break;
                    default:
                        break;
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                List<HIC_JEPSU_EXJONG_PATIENT> list = hicJepsuExjongPatientService.GetItembyJepDateLtdCode(strFrDate, strToDate, nLtdCode, strTong, strJob);

                nREAD = list.Count;
                nRow = 0;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strGbSTS = list[i].GBSTS.Trim();    //검사상태
                    strGjJong = list[i].GJJONG.Trim();  //검사종류
                    StrJumin = clsAES.DeAES(list[i].JUMIN2.Trim());

                    nRow += 1;
                    if (nRow > 0)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                        SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(StrJumin, 6) + "-" + VB.Right(StrJumin, 7);
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].GJJONG;
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].MAILCODE;
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].JUSO1;
                        SS1.ActiveSheet.Cells[i, 7].Text = list[i].JUSO2;
                        SS1.ActiveSheet.Cells[i, 8].Text = list[i].SECOND_EXAMS;
                        SS1.ActiveSheet.Cells[i, 9].Text = list[i].SECOND_SAYU;
                        SS1.ActiveSheet.Cells[i, 10].Text = list[i].SECOND_MISAYU;
                        SS1.ActiveSheet.Cells[i, 11].Text = list[i].TEL;
                        SS1.ActiveSheet.Cells[i, 12].Text = "";
                        SS1.ActiveSheet.Cells[i, 13].Text = "";
                        SS1.ActiveSheet.Cells[i, 14].Text = "";
                        if (!list[i].SECOND_TONGBO.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = VB.Right(list[i].SECOND_TONGBO, 5);
                        }
                        if (!list[i].SECOND_DATE.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 13].Text = VB.Right(list[i].SECOND_DATE, 5);
                        }
                        SS1.ActiveSheet.Cells[i, 14].Text = VB.Right(list[i].JEPDATE, 5);
                        SS1.ActiveSheet.Cells[i, 15].Text = list[i].WRTNO.To<string>();
                        SS1.ActiveSheet.Cells[i, 16].Text = "";
                        //SS1.ActiveSheet.Cells[i, 17].Text = "";
                        if (list[i].JONGGUMYN.Trim() == "1")
                        {
                            //SS1.ActiveSheet.Cells[i, 17].Text = "◎";
                            SS1.ActiveSheet.Cells[i, 16].Text = "◎";
                        }
                    }
                    progressBar1.Value = i + 1;
                }

                //RowHeight Resize
                for (int i = 0; i < nREAD; i++)
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 6);
                    Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 9);

                    if (size.Height >= size1.Height)
                    {
                        SS1.ActiveSheet.Rows[i].Height = size.Height;
                    }
                    else if (size.Height < size1.Height)
                    {
                        SS1.ActiveSheet.Rows[i].Height = size1.Height;
                    }
                }

                Cursor.Current = Cursors.Default;

                SS1.Enabled = true;
            }
        }

        void SS2_Sheet_Clear()
        {
            SS2.ActiveSheet.Cells[3, 2].Text = "";
            SS2.ActiveSheet.Cells[4, 2].Text = "";

            SS2.ActiveSheet.Cells[17, 2].Text = "";
            SS2.ActiveSheet.Cells[18, 2].Text = "";
        }

        void ePrint(object sender, PrintPageEventArgs ev)
        {
            string strOK = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;
            string strJuso1 = "";
            string strJuso2 = "";
            string strMail = "";
            string strSname = "";

            int nX = 0;
            int nY = 0;
            int nCY = 18;

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strOK = "";
                if (rdoPrt1.Checked == true)
                {
                    strOK = "OK";
                }

                if (SS1.ActiveSheet.Cells[i, 0].Text == "True" && rdoPrt2.Checked == true)
                {
                    strOK = "OK";
                }

                if (SS1.ActiveSheet.Cells[i, 0].Text.IsNullOrEmpty() && rdoPrt3.Checked == true)
                {
                    strOK = "OK";
                }

                if (strOK == "OK")
                {
                    strSname = SS1.ActiveSheet.Cells[i, 2].Text.Trim();

                    strJuso1 = SS1.ActiveSheet.Cells[i, 6].Text.Trim(); //우편번호의 주소
                                                                        //2번째줄의 주소
                    if (!SS1.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty())
                    {
                        strJuso2 = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                        strJuso2 += VB.Space(2) + SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    }
                    else
                    {
                        strJuso2 = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                    }
                    strMail = SS1.ActiveSheet.Cells[i, 5].Text.Trim();

                    ///TODO : 이상훈(2019.12.11) 봉투 출력 좌표 확인 필요
                    ev.Graphics.DrawString(strJuso1, new Font("굴림체", 15f), Brushes.Black, nX + 5, nY + (nCY * 0), new StringFormat());
                    ev.Graphics.DrawString(strJuso2, new Font("굴림체", 13f), Brushes.Black, nX + 5, nY + (nCY * 1), new StringFormat());
                    ev.Graphics.DrawString(strSname, new Font("굴림체", 15f), Brushes.Black, nX + 5, nY + (nCY * 2), new StringFormat());

                    ev.Graphics.DrawString(VB.Left(strMail, 1) + "  " + VB.Mid(strMail, 2, 1) + "  " + VB.Mid(strMail, 3, 1), new Font("굴림체", 18f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 3), new StringFormat());
                    ev.Graphics.DrawString(VB.Mid(strMail, 4, 1) + "  " + VB.Mid(strMail, 5, 1) + "  " + VB.Mid(strMail, 6, 1), new Font("굴림체", 18f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 4), new StringFormat());

                    pd.Print();    //프린트
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            rdoPrt2.Checked = true;
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
