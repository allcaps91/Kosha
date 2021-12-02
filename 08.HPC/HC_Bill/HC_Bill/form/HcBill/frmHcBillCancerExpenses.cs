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
/// Class Name      : HC_Bill
/// File Name       : frmHcBillCancerExpenses.cs
/// Description     : 암검진 비용청구서 [2020]
/// Author          : 이상훈
/// Create Date     : 2020-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm암청구서_2020.frm(Frm암청구서_2020)" />

namespace HC_Bill
{
    public partial class frmHcBillCancerExpenses : Form
    {
        HicCodeService hicCodeService = null;
        HicMirCancerBoService hicMirCancerBoService = null;
        HicMirCancerService hicMirCancerService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        PrintDocument pd;

        string FstrMirNo;

        public frmHcBillCancerExpenses()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmHcBillCancerExpenses(string strMirNo)
        {
            InitializeComponent();
            FstrMirNo = strMirNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            hicMirCancerBoService = new HicMirCancerBoService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtMirno.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            int nYY = 0;

            txtMirno.Text = "";

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            cboYear.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;
            //cboYear.Text = "2020";

            if (!FstrMirNo.IsNullOrEmpty())
            {
                txtMirno.Text = FstrMirNo;
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnAmt)
            {   
                fn_Amt_Gesan();
                btnAmt.Enabled = false;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;


                strTitle = "";
                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                if (rdoCnt1.Checked == true)
                {
                    for (int i = 0; i < 2; i++) //인쇄매수
                    {
                        sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                    }
                }
                else if (rdoCnt0.Checked == true)
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
            }
            else if (sender == btnSearch)
            {
                string strYear = "";
                string strkiho = "";
                string strLtdCode = "";
                string strLtdName = "";
                string strSDate = "";
                long nREAD = 0;
                long nAmt = 0;

                long[] nOneInwon = new long[51];    //41.위, 42.대장 , 43.간, 44.유방, 45.자궁 46.폐
                long nTot1 = 0;                 //절사된것
                long nTot2 = 0;                 //절사안된것
                long nCnt = 0;

                int nCol암 = 0;                 //암,의료급여암 Col 건수
                int nCol암2 = 0;                //암,의료급여암 Col 금액

                string strBogunso = "";

                for (int i = 0; i <= 50; i++)
                {
                    nOneInwon[i] = 0;
                }

                nTot1 = 0;
                nTot2 = 0;

                fn_Spread_Clear();

                strYear = cboYear.Text;
                if (txtMirno.Text.Trim() == "")
                {
                    return;
                }

                //암청구읽음
                chb.READ_HIC_MIR_CANCER(txtMirno.Text.To<long>());
                chb.READ_HIC_MIR_CANCER_Bo(txtMirno.Text.To<long>());
                if (clsHcType.TMC.Life_Gbn == "Y")
                {
                    lbl_Life.Visible = true;
                }
                else
                {
                    lbl_Life.Visible = false;
                }

                if (string.Compare(clsHcType.TMC.FrDate, "2015.09.01") >= 0)
                {
                    //위조직검사
                    for (int i = 14; i <= 18; i++)
                    {
                        nAmt = SS1.ActiveSheet.Cells[i, 5].Text.To<long>() + 22000;
                        SS1.ActiveSheet.Cells[i, 5].Text = string.Format("{0:#,##0}", nAmt);
                        nAmt = SS1.ActiveSheet.Cells[i, 6].Text.To<long>() + 19800;
                        SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:#,##0}", nAmt);
                        nAmt = SS1.ActiveSheet.Cells[i, 7].Text.To<long>() + 2200;
                        SS1.ActiveSheet.Cells[i, 7].Text = string.Format("{0:#,##0}", nAmt);
                    }
                    //대장직검사
                    for (int i = 33; i <= 37; i++)
                    {
                        nAmt = SS1.ActiveSheet.Cells[i, 5].Text.To<long>() + 22000;
                        SS1.ActiveSheet.Cells[i, 5].Text = string.Format("{0:#,##0}", nAmt);
                        nAmt = SS1.ActiveSheet.Cells[i, 6].Text.To<long>() + 19800;
                        SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:#,##0}", nAmt);
                        nAmt = SS1.ActiveSheet.Cells[i, 7].Text.To<long>() + 2200;
                        SS1.ActiveSheet.Cells[i, 7].Text = string.Format("{0:#,##0}", nAmt);
                    }
                }

                //청구암구분
                if (!clsHcType.TMCB.ROWID.IsNullOrEmpty() && !clsHcType.TMC.ROWID.IsNullOrEmpty())
                {
                    if (clsHcType.TMC.Johap == "X")
                    {
                        nCol암 = 9;
                        nCol암2 = 5;
                        SS1.ActiveSheet.Cells[2, 8].Text = "[의료급여+보건소]";
                        MessageBox.Show("의료급여+보건소 입니다. 확인바랍니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        strBogunso = "";
                        strBogunso = chb.READ_HIC_BOGUNSO(txtMirno.Text.To<long>());
                        if (!strBogunso.IsNullOrEmpty())
                        {
                            nCol암 = 9;
                            nCol암2 = 7;
                            SS1.ActiveSheet.Cells[2, 8].Text = "[암검진+보건소(" + strBogunso + ")]";
                        }
                        else
                        {
                            nCol암 = 9;
                            nCol암2 = 7;
                            SS1.ActiveSheet.Cells[2, 8].Text = "[암검진+보건소]";
                        }
                    }
                }
                else if (!clsHcType.TMC.ROWID.IsNullOrEmpty())
                {
                    if (clsHcType.TMC.Johap == "X")
                    {
                        nCol암 = 9;
                        nCol암2 = 5;
                        SS1.ActiveSheet.Cells[2, 8].Text = "[의료급여]";
                    }
                    else
                    {
                        nCol암 = 9;
                        nCol암2 = 5;
                        SS1.ActiveSheet.Cells[2, 8].Text = "[암검진]";
                    }
                }

                if (!clsHcType.TMC.ROWID.IsNullOrEmpty())
                {
                    //청구건수
                    nOneInwon[1] = clsHcType.TMC.Inwon[1];        //위암-위장조영검사
                    nOneInwon[2] = clsHcType.TMC.Inwon[2];        //위암-위내시경검사
                    nOneInwon[3] = clsHcType.TMC.Inwon[3];        //위암-조직검사 1-3
                    nOneInwon[4] = clsHcType.TMC.Inwon[4];        //위암-조직검사 4-6
                    nOneInwon[5] = clsHcType.TMC.Inwon[5];        //위암-조직검사 7-9
                    nOneInwon[6] = clsHcType.TMC.Inwon[6];        //위암-조직검사 10-12
                    nOneInwon[7] = clsHcType.TMC.Inwon[7];        //위암-조직검사 13이상

                    nOneInwon[8] = clsHcType.TMC.Inwon[8];        //간암-의료급여-ALT
                    nOneInwon[9] = clsHcType.TMC.Inwon[9];        //간암-의료급여-B형간염항원-정밀
                    nOneInwon[10] = clsHcType.TMC.Inwon[10];      //간암-의료급여-C형간염항체-정밀
                    nOneInwon[11] = clsHcType.TMC.Inwon[11];      //간암-간초음파검사
                    nOneInwon[12] = clsHcType.TMC.Inwon[12];      //간암-혈청알파태아단백(RPHA)
                    nOneInwon[13] = clsHcType.TMC.Inwon[13];      //간암-                (EIA)

                    nOneInwon[14] = clsHcType.TMC.Inwon[14];      //대장암-분변잠혈방응(RPHA)
                    nOneInwon[15] = clsHcType.TMC.Inwon[15];      //대장암-            (혈색소정량)
                    nOneInwon[16] = clsHcType.TMC.Inwon[16];      //대장암-대장이중조영촬영
                    nOneInwon[17] = clsHcType.TMC.Inwon[17];      //대장암-대장내시경검사
                    nOneInwon[18] = clsHcType.TMC.Inwon[18];      //대장암-조직검사 1-3
                    nOneInwon[19] = clsHcType.TMC.Inwon[19];      //대장암-조직검사 4-6
                    nOneInwon[20] = clsHcType.TMC.Inwon[20];      //대장암-조직검사 7-9
                    nOneInwon[21] = clsHcType.TMC.Inwon[21];      //대장암-조직검사 10-12
                    nOneInwon[22] = clsHcType.TMC.Inwon[22];      //대장암-조직검사 13이상

                    nOneInwon[23] = clsHcType.TMC.Inwon[23];      //유방암-유방촬영
                    nOneInwon[24] = clsHcType.TMC.Inwon[24];      //자궁경부암-자궁경부세포검사

                    nOneInwon[26] = clsHcType.TMC.Inwon[26];      //토요일,공휴일가산30%

                    nOneInwon[27] = clsHcType.TMC.Inwon[27];      //폐암-저선량CT
                    nOneInwon[28] = clsHcType.TMC.Inwon[28];      //폐암-사후상담

                    nOneInwon[41] = clsHcType.TMC.Inwon[41];      //위암-상담료
                    nOneInwon[42] = clsHcType.TMC.Inwon[42];      //대장암-상담료
                    nOneInwon[43] = clsHcType.TMC.Inwon[43];      //간암-상담료
                    nOneInwon[44] = clsHcType.TMC.Inwon[44];      //유방암-상담료
                    nOneInwon[45] = clsHcType.TMC.Inwon[45];      //자궁경부암-상담료
                    nOneInwon[46] = clsHcType.TMC.Inwon[46];      //폐암-상담료

                    strkiho = clsHcType.TMC.Kiho;
                    strLtdCode = "";
                    strLtdName = "";
                    strSDate = clsHcType.TMC.FrDate + " ~ " + clsHcType.TMC.ToDate;

                    //sheet dlsplay
                    SS1.ActiveSheet.Cells[11, nCol암].Text = nOneInwon[41].To<string>();
                    SS1.ActiveSheet.Cells[12, nCol암].Text = nOneInwon[1].To<string>();
                    SS1.ActiveSheet.Cells[13, nCol암].Text = nOneInwon[2].To<string>();
                    SS1.ActiveSheet.Cells[14, nCol암].Text = nOneInwon[3].To<string>();
                    SS1.ActiveSheet.Cells[15, nCol암].Text = nOneInwon[4].To<string>();
                    SS1.ActiveSheet.Cells[16, nCol암].Text = nOneInwon[5].To<string>();
                    SS1.ActiveSheet.Cells[17, nCol암].Text = nOneInwon[6].To<string>();
                    SS1.ActiveSheet.Cells[18, nCol암].Text = nOneInwon[7].To<string>();

                    SS1.ActiveSheet.Cells[19, nCol암].Text = nOneInwon[43].To<string>();
                    SS1.ActiveSheet.Cells[20, nCol암].Text = nOneInwon[8].To<string>();
                    SS1.ActiveSheet.Cells[22, nCol암].Text = nOneInwon[9].To<string>();
                    SS1.ActiveSheet.Cells[24, nCol암].Text = nOneInwon[10].To<string>();
                    SS1.ActiveSheet.Cells[25, nCol암].Text = nOneInwon[11].To<string>();
                    SS1.ActiveSheet.Cells[26, nCol암].Text = nOneInwon[12].To<string>();
                    SS1.ActiveSheet.Cells[27, nCol암].Text = nOneInwon[13].To<string>();

                    SS1.ActiveSheet.Cells[28, nCol암].Text = nOneInwon[42].To<string>();
                    SS1.ActiveSheet.Cells[29, nCol암].Text = nOneInwon[14].To<string>();
                    SS1.ActiveSheet.Cells[30, nCol암].Text = nOneInwon[15].To<string>();
                    SS1.ActiveSheet.Cells[31, nCol암].Text = nOneInwon[16].To<string>();
                    SS1.ActiveSheet.Cells[32, nCol암].Text = nOneInwon[17].To<string>();
                    SS1.ActiveSheet.Cells[33, nCol암].Text = nOneInwon[18].To<string>();
                    SS1.ActiveSheet.Cells[34, nCol암].Text = nOneInwon[19].To<string>();
                    SS1.ActiveSheet.Cells[35, nCol암].Text = nOneInwon[20].To<string>();
                    SS1.ActiveSheet.Cells[36, nCol암].Text = nOneInwon[21].To<string>();
                    SS1.ActiveSheet.Cells[37, nCol암].Text = nOneInwon[22].To<string>();

                    SS1.ActiveSheet.Cells[38, nCol암].Text = nOneInwon[44].To<string>();
                    SS1.ActiveSheet.Cells[39, nCol암].Text = nOneInwon[23].To<string>();

                    SS1.ActiveSheet.Cells[40, nCol암].Text = nOneInwon[45].To<string>();
                    SS1.ActiveSheet.Cells[41, nCol암].Text = nOneInwon[24].To<string>();

                    SS1.ActiveSheet.Cells[42, nCol암].Text = nOneInwon[26].To<string>();

                    SS1.ActiveSheet.Cells[43, nCol암].Text = nOneInwon[46].To<string>();
                    SS1.ActiveSheet.Cells[44, nCol암].Text = nOneInwon[27].To<string>();
                    SS1.ActiveSheet.Cells[45, nCol암].Text = nOneInwon[28].To<string>();

                    //인원*금액 -> 공단
                    nCnt = clsHcType.TMC.JepQty;
                    nOneInwon[41] = SS1.ActiveSheet.Cells[11, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[41];
                    SS1.ActiveSheet.Cells[11, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[41]);

                    nOneInwon[1] = SS1.ActiveSheet.Cells[12, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[1];
                    SS1.ActiveSheet.Cells[12, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[1]);

                    nOneInwon[2] = SS1.ActiveSheet.Cells[13, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[2];
                    SS1.ActiveSheet.Cells[13, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[2]);

                    nOneInwon[3] = SS1.ActiveSheet.Cells[14, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[3];
                    SS1.ActiveSheet.Cells[14, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[3]);

                    nOneInwon[4] = SS1.ActiveSheet.Cells[15, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[4];
                    SS1.ActiveSheet.Cells[15, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[4]);

                    nOneInwon[5] = SS1.ActiveSheet.Cells[16, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[5];
                    SS1.ActiveSheet.Cells[16, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[5]);

                    nOneInwon[6] = SS1.ActiveSheet.Cells[17, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[6];
                    SS1.ActiveSheet.Cells[17, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[6]);

                    nOneInwon[7] = SS1.ActiveSheet.Cells[18, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[7];
                    SS1.ActiveSheet.Cells[18, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[7]);

                    nOneInwon[43] = SS1.ActiveSheet.Cells[19, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[43];
                    SS1.ActiveSheet.Cells[19, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[43]);

                    nOneInwon[8] = SS1.ActiveSheet.Cells[20, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[8];
                    SS1.ActiveSheet.Cells[20, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[8]);

                    nOneInwon[9] = SS1.ActiveSheet.Cells[22, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[9];
                    SS1.ActiveSheet.Cells[22, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[9]);

                    nOneInwon[10] = SS1.ActiveSheet.Cells[24, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[10];
                    SS1.ActiveSheet.Cells[24, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[10]);

                    nOneInwon[11] = SS1.ActiveSheet.Cells[25, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[11];
                    SS1.ActiveSheet.Cells[25, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[11]);

                    nOneInwon[12] = SS1.ActiveSheet.Cells[26, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[12];
                    SS1.ActiveSheet.Cells[26, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[12]);

                    nOneInwon[13] = SS1.ActiveSheet.Cells[27, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[13];
                    SS1.ActiveSheet.Cells[27, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[13]);

                    nOneInwon[42] = SS1.ActiveSheet.Cells[28, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[42];
                    SS1.ActiveSheet.Cells[28, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[42]);

                    nOneInwon[14] = SS1.ActiveSheet.Cells[29, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[14];
                    SS1.ActiveSheet.Cells[29, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[14]);

                    nOneInwon[15] = SS1.ActiveSheet.Cells[30, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[15];
                    SS1.ActiveSheet.Cells[30, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[15]);

                    nOneInwon[16] = SS1.ActiveSheet.Cells[31, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[16];
                    SS1.ActiveSheet.Cells[31, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[16]);

                    nOneInwon[17] = SS1.ActiveSheet.Cells[32, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[17];
                    SS1.ActiveSheet.Cells[32, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[17]);

                    nOneInwon[18] = SS1.ActiveSheet.Cells[33, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[18];
                    SS1.ActiveSheet.Cells[33, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[18]);

                    nOneInwon[19] = SS1.ActiveSheet.Cells[34, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[19];
                    SS1.ActiveSheet.Cells[34, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[19]);

                    nOneInwon[20] = SS1.ActiveSheet.Cells[35, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[20];
                    SS1.ActiveSheet.Cells[35, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[20]);

                    nOneInwon[21] = SS1.ActiveSheet.Cells[36, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[21];
                    SS1.ActiveSheet.Cells[36, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[21]);

                    nOneInwon[22] = SS1.ActiveSheet.Cells[37, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[22];
                    SS1.ActiveSheet.Cells[37, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[22]);

                    nOneInwon[44] = SS1.ActiveSheet.Cells[38, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[44];
                    SS1.ActiveSheet.Cells[38, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[44]);

                    nOneInwon[23] = SS1.ActiveSheet.Cells[39, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[23];
                    SS1.ActiveSheet.Cells[39, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[23]);

                    nOneInwon[26] = SS1.ActiveSheet.Cells[42, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[26];
                    SS1.ActiveSheet.Cells[42, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[26]);

                    nOneInwon[46] = SS1.ActiveSheet.Cells[43, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[46];
                    SS1.ActiveSheet.Cells[43, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[46]);

                    nOneInwon[27] = SS1.ActiveSheet.Cells[44, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[27];
                    SS1.ActiveSheet.Cells[44, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[27]);

                    nOneInwon[28] = SS1.ActiveSheet.Cells[45, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[28];
                    SS1.ActiveSheet.Cells[45, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[28]);

                    if (clsHcType.TMC.Johap == "X")   //의료급여
                    {
                        nOneInwon[45] = SS1.ActiveSheet.Cells[40, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[45];
                        SS1.ActiveSheet.Cells[40, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[45]);

                        nOneInwon[24] = SS1.ActiveSheet.Cells[41, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[24];
                        SS1.ActiveSheet.Cells[41, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[24]);
                    }
                    else
                    {
                        if (clsHcType.TMC.Life_Gbn == "Y")
                        {
                            if (!strBogunso.IsNullOrEmpty())
                            {
                                nOneInwon[45] = SS1.ActiveSheet.Cells[40, 5].Text.Replace(",", "").To<long>() * nOneInwon[45];
                                SS1.ActiveSheet.Cells[40, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[45]);

                                nOneInwon[24] = SS1.ActiveSheet.Cells[41, 5].Text.Replace(",", "").To<long>() * nOneInwon[24];
                                SS1.ActiveSheet.Cells[41, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[24]);
                            }
                            else
                            {
                                nOneInwon[45] = SS1.ActiveSheet.Cells[40, nCol암2].Text.Replace(",", "").To<long>() * nOneInwon[45];
                                SS1.ActiveSheet.Cells[40, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[45]);

                                nOneInwon[24] = SS1.ActiveSheet.Cells[41, 5].Text.Replace(",", "").To<long>() * nOneInwon[24];
                                SS1.ActiveSheet.Cells[41, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[24]);
                            }
                        }
                        else
                        {
                            nOneInwon[45] = SS1.ActiveSheet.Cells[40, 5].Text.Replace(",", "").To<long>() * nOneInwon[45];
                            SS1.ActiveSheet.Cells[40, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[45]);

                            nOneInwon[24] = SS1.ActiveSheet.Cells[41, 5].Text.Replace(",", "").To<long>() * nOneInwon[24];
                            SS1.ActiveSheet.Cells[41, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nOneInwon[24]);
                        }
                    }

                    for (int j = 11; j <= 45; j++)
                    {
                        nTot1 += SS1.ActiveSheet.Cells[j, nCol암 + 2].Text.Replace(",", "").To<long>();
                    }

                    SS1.ActiveSheet.Cells[2, 1].Text = "  출력일자 : " + clsPublic.GstrSysDate;
                    nTot2 = nTot1;
                    nTot1 = (long)Math.Truncate((decimal)nTot1 / 10) * 10;  //원단위 절사

                    SS1.ActiveSheet.Cells[7, 9].Text = string.Format("{0:###,###,###,##0}", nTot1);
                    SS1.ActiveSheet.Cells[10, nCol암].Text = clsHcType.TMC.JepQty.To<string>();
                    SS1.ActiveSheet.Cells[10, nCol암 + 2].Text = string.Format("{0:###,###,###,##0}", nTot2);
                    SS1.ActiveSheet.Cells[46, 1].Text = "검진기간 : " + strSDate + "  파일명 : " + clsHcType.TMC.FileName + "  [ 청구번호 : " + clsHcType.TMC.MIRNO + " ]";
                }

                //보건소암
                if (!clsHcType.TMCB.ROWID.IsNullOrEmpty())
                {
                    for (int i = 0; i <= 50; i++)
                    {
                        nOneInwon[i] = 0;
                    }
                    nTot1 = 0;

                    //청구건수
                    nOneInwon[1] = clsHcType.TMCB.Inwon[1];           //위암-위장조영검사
                    nOneInwon[2] = clsHcType.TMCB.Inwon[2];           //위암-위내시경검사
                    nOneInwon[3] = clsHcType.TMCB.Inwon[3];           //위암-조직검사 1-3
                    nOneInwon[4] = clsHcType.TMCB.Inwon[4];           //위암-조직검사 4-6
                    nOneInwon[5] = clsHcType.TMCB.Inwon[5];           //위암-조직검사 7-9
                    nOneInwon[6] = clsHcType.TMCB.Inwon[6];           //위암-조직검사 10-12
                    nOneInwon[7] = clsHcType.TMCB.Inwon[7];           //위암-조직검사 13이상
                                                                
                    nOneInwon[8] = clsHcType.TMCB.Inwon[8];           //간암-의료급여-ALT
                    nOneInwon[9] = clsHcType.TMCB.Inwon[9];           //간암-의료급여-B형간염항원-정밀
                    nOneInwon[10] = clsHcType.TMCB.Inwon[10];         //간암-의료급여-C형간염항체-정밀
                    nOneInwon[11] = clsHcType.TMCB.Inwon[11];         //간암-간초음파검사
                    nOneInwon[12] = clsHcType.TMCB.Inwon[12];         //간암-혈청알파태아단백(RPHA)
                    nOneInwon[13] = clsHcType.TMCB.Inwon[13];         //간암-                (EIA)
                                                                
                    nOneInwon[14] = clsHcType.TMCB.Inwon[14];         //대장암-분변잠혈방응(RPHA)
                    nOneInwon[15] = clsHcType.TMCB.Inwon[15];         //대장암-            (혈색소정량)
                    nOneInwon[16] = clsHcType.TMCB.Inwon[16];         //대장암-대장이중조영촬영
                    nOneInwon[17] = clsHcType.TMCB.Inwon[17];         //대장암-대장내시경검사
                    nOneInwon[18] = clsHcType.TMCB.Inwon[18];         //대장암-조직검사 1-3
                    nOneInwon[19] = clsHcType.TMCB.Inwon[19];         //대장암-조직검사 4-6
                    nOneInwon[20] = clsHcType.TMCB.Inwon[20];         //대장암-조직검사 7-9
                    nOneInwon[21] = clsHcType.TMCB.Inwon[21];         //대장암-조직검사 10-12
                    nOneInwon[22] = clsHcType.TMCB.Inwon[22];         //대장암-조직검사 13이상
                                                                
                    nOneInwon[23] = clsHcType.TMCB.Inwon[23];         //유방암-유방촬영
                                                                
                    nOneInwon[24] = clsHcType.TMCB.Inwon[24];         //자궁경부암-자궁경부세포검사
                    nOneInwon[26] = clsHcType.TMCB.Inwon[26];         //토요일,공휴일가산30%
                                                                //
                    nOneInwon[27] = clsHcType.TMCB.Inwon[27];         //폐암 - 저선량CT
                    nOneInwon[28] = clsHcType.TMCB.Inwon[28];         //폐암 - 사후상담
                                                                
                    nOneInwon[41] = clsHcType.TMCB.Inwon[41];         //위암-상담료
                    nOneInwon[42] = clsHcType.TMCB.Inwon[42];         //대장암-상담료
                    nOneInwon[43] = clsHcType.TMCB.Inwon[43];         //간암-상담료
                    nOneInwon[44] = clsHcType.TMCB.Inwon[44];         //유방암-상담료
                    nOneInwon[45] = clsHcType.TMCB.Inwon[45];         //자궁경부암-상담료

                    //sheet dlsplay
                    SS1.ActiveSheet.Cells[11, 9].Text = nOneInwon[41].To<string>();              //위암-상담료
                    SS1.ActiveSheet.Cells[12, 9].Text = nOneInwon[1].To<string>();               //위암-위장조영검사
                    SS1.ActiveSheet.Cells[13, 9].Text = nOneInwon[2].To<string>();               //위암-위내시경검사
                    SS1.ActiveSheet.Cells[14, 9].Text = nOneInwon[3].To<string>();               //위암-조직검사 1-3
                    SS1.ActiveSheet.Cells[15, 9].Text = nOneInwon[4].To<string>();               //위암-조직검사
                    SS1.ActiveSheet.Cells[16, 9].Text = nOneInwon[5].To<string>();               //위암-조직검사
                    SS1.ActiveSheet.Cells[17, 9].Text = nOneInwon[6].To<string>();               //위암-조직검사
                    SS1.ActiveSheet.Cells[18, 9].Text = nOneInwon[7].To<string>();               //위암-조직검사


                    SS1.ActiveSheet.Cells[19, 9].Text = nOneInwon[43].To<string>();              //간암-상담료
                    SS1.ActiveSheet.Cells[25, 9].Text = nOneInwon[11].To<string>();              //간암-간초음파검사
                    SS1.ActiveSheet.Cells[26, 9].Text = nOneInwon[12].To<string>();              //간암-혈청알파태아단백(RPHA)
                    SS1.ActiveSheet.Cells[27, 9].Text = nOneInwon[13].To<string>();              //간암-                (EIA)

                    SS1.ActiveSheet.Cells[28, 9].Text = nOneInwon[42].To<string>();              //대장암-상담료
                    SS1.ActiveSheet.Cells[29, 9].Text = nOneInwon[14].To<string>();              //대장암-분변잠혈방응(RPHA)
                    SS1.ActiveSheet.Cells[30, 9].Text = nOneInwon[15].To<string>();              //대장암-            (혈색소정량)
                    SS1.ActiveSheet.Cells[31, 9].Text = nOneInwon[16].To<string>();              //대장암-대장이중조영촬영
                    SS1.ActiveSheet.Cells[32, 9].Text = nOneInwon[17].To<string>();              //대장암-대장내시경검사
                    SS1.ActiveSheet.Cells[33, 9].Text = nOneInwon[18].To<string>();              //대장암-조직검사 1-3
                    SS1.ActiveSheet.Cells[34, 9].Text = nOneInwon[19].To<string>();              //대장암-조직검사
                    SS1.ActiveSheet.Cells[35, 9].Text = nOneInwon[20].To<string>();              //대장암-조직검사
                    SS1.ActiveSheet.Cells[36, 9].Text = nOneInwon[21].To<string>();              //대장암-조직검사
                    SS1.ActiveSheet.Cells[37, 9].Text = nOneInwon[22].To<string>();              //대장암-조직검사

                    SS1.ActiveSheet.Cells[38, 9].Text = nOneInwon[44].To<string>();              //유방암-상담료
                    SS1.ActiveSheet.Cells[39, 9].Text = nOneInwon[23].To<string>();              //유방암-유방촬영

                    SS1.ActiveSheet.Cells[42, 9].Text = nOneInwon[26].To<string>();              //토요일,공휴일 30%가산

                    SS1.ActiveSheet.Cells[43, 9].Text = nOneInwon[44].To<string>();              //유방암-상담료
                    SS1.ActiveSheet.Cells[44, 9].Text = nOneInwon[27].To<string>();              //폐암-저선량CT
                    SS1.ActiveSheet.Cells[45, 9].Text = nOneInwon[28].To<string>();              //폐암-사후상담

                    //인원*금액 -> 보건소
                    nCnt = clsHcType.TMCB.JepQty;
                    nOneInwon[41] = SS1.ActiveSheet.Cells[11, 7].Text.Replace(",", "").To<long>() * nOneInwon[41];
                    SS1.ActiveSheet.Cells[11, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[41]);

                    nOneInwon[1] = SS1.ActiveSheet.Cells[12, 7].Text.Replace(",", "").To<long>() * nOneInwon[1];
                    SS1.ActiveSheet.Cells[12, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[1]);

                    nOneInwon[2] = SS1.ActiveSheet.Cells[13, 7].Text.Replace(",", "").To<long>() * nOneInwon[2];
                    SS1.ActiveSheet.Cells[13, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[2]);

                    nOneInwon[3] = SS1.ActiveSheet.Cells[14, 7].Text.Replace(",", "").To<long>() * nOneInwon[3];
                    SS1.ActiveSheet.Cells[14, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[3]);

                    nOneInwon[4] = SS1.ActiveSheet.Cells[15, 7].Text.Replace(",", "").To<long>() * nOneInwon[4];
                    SS1.ActiveSheet.Cells[15, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[4]);

                    nOneInwon[5] = SS1.ActiveSheet.Cells[16, 7].Text.Replace(",", "").To<long>() * nOneInwon[5];
                    SS1.ActiveSheet.Cells[16, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[5]);

                    nOneInwon[6] = SS1.ActiveSheet.Cells[17, 7].Text.Replace(",", "").To<long>() * nOneInwon[6];
                    SS1.ActiveSheet.Cells[17, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[6]);

                    nOneInwon[7] = SS1.ActiveSheet.Cells[18, 7].Text.Replace(",", "").To<long>() * nOneInwon[7];
                    SS1.ActiveSheet.Cells[18, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[7]);

                    nOneInwon[43] = SS1.ActiveSheet.Cells[19, 7].Text.Replace(",", "").To<long>() * nOneInwon[43];
                    SS1.ActiveSheet.Cells[19, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[43]);

                    nOneInwon[11] = SS1.ActiveSheet.Cells[25, 7].Text.Replace(",", "").To<long>() * nOneInwon[11];
                    SS1.ActiveSheet.Cells[25, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[11]);

                    nOneInwon[12] = SS1.ActiveSheet.Cells[26, 7].Text.Replace(",", "").To<long>() * nOneInwon[12];
                    SS1.ActiveSheet.Cells[26, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[12]);

                    nOneInwon[13] = SS1.ActiveSheet.Cells[27, 7].Text.Replace(",", "").To<long>() * nOneInwon[13];
                    SS1.ActiveSheet.Cells[27, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[13]);

                    nOneInwon[42] = SS1.ActiveSheet.Cells[28, 7].Text.Replace(",", "").To<long>() * nOneInwon[42];
                    SS1.ActiveSheet.Cells[28, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[42]);

                    nOneInwon[14] = SS1.ActiveSheet.Cells[29, 7].Text.Replace(",", "").To<long>() * nOneInwon[14];
                    SS1.ActiveSheet.Cells[29, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[14]);

                    nOneInwon[15] = SS1.ActiveSheet.Cells[30, 7].Text.Replace(",", "").To<long>() * nOneInwon[15];
                    SS1.ActiveSheet.Cells[30, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[15]);

                    nOneInwon[16] = SS1.ActiveSheet.Cells[31, 7].Text.Replace(",", "").To<long>() * nOneInwon[16];
                    SS1.ActiveSheet.Cells[31, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[16]);

                    nOneInwon[17] = SS1.ActiveSheet.Cells[32, 7].Text.Replace(",", "").To<long>() * nOneInwon[17];
                    SS1.ActiveSheet.Cells[32, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[17]);

                    nOneInwon[18] = SS1.ActiveSheet.Cells[33, 7].Text.Replace(",", "").To<long>() * nOneInwon[18];
                    SS1.ActiveSheet.Cells[33, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[18]);

                    nOneInwon[19] = SS1.ActiveSheet.Cells[34, 7].Text.Replace(",", "").To<long>() * nOneInwon[19];
                    SS1.ActiveSheet.Cells[34, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[19]);

                    nOneInwon[20] = SS1.ActiveSheet.Cells[35, 7].Text.Replace(",", "").To<long>() * nOneInwon[20];
                    SS1.ActiveSheet.Cells[35, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[20]);

                    nOneInwon[21] = SS1.ActiveSheet.Cells[36, 7].Text.Replace(",", "").To<long>() * nOneInwon[21];
                    SS1.ActiveSheet.Cells[36, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[21]);

                    nOneInwon[22] = SS1.ActiveSheet.Cells[37, 7].Text.Replace(",", "").To<long>() * nOneInwon[22];
                    SS1.ActiveSheet.Cells[37, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[22]);

                    nOneInwon[44] = SS1.ActiveSheet.Cells[38, 7].Text.Replace(",", "").To<long>() * nOneInwon[44];
                    SS1.ActiveSheet.Cells[38, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[44]);

                    nOneInwon[23] = SS1.ActiveSheet.Cells[39, 7].Text.Replace(",", "").To<long>() * nOneInwon[23];
                    SS1.ActiveSheet.Cells[39, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[23]);

                    nOneInwon[26] = SS1.ActiveSheet.Cells[42, 7].Text.Replace(",", "").To<long>() * nOneInwon[26];
                    SS1.ActiveSheet.Cells[42, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[26]);

                    nOneInwon[46] = SS1.ActiveSheet.Cells[43, 7].Text.Replace(",", "").To<long>() * nOneInwon[46];
                    SS1.ActiveSheet.Cells[43, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[46]);

                    nOneInwon[27] = SS1.ActiveSheet.Cells[44, 7].Text.Replace(",", "").To<long>() * nOneInwon[27];
                    SS1.ActiveSheet.Cells[44, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[27]);

                    nOneInwon[28] = SS1.ActiveSheet.Cells[45, 7].Text.Replace(",", "").To<long>() * nOneInwon[28];
                    SS1.ActiveSheet.Cells[45, 11].Text = string.Format("{0:###,###,###,##0}", nOneInwon[28]);

                    for (int j = 11; j <= 45; j++)
                    {
                        nTot1 += SS1.ActiveSheet.Cells[j, 11].Text.Replace(",", "").To<long>();
                    }

                    SS1.ActiveSheet.Cells[10, 9].Text = clsHcType.TMCB.JepQty.To<string>();
                    SS1.ActiveSheet.Cells[10, 11].Text = string.Format("{0:###,###,###,##0}", nTot1);

                    nTot1 = (long)Math.Truncate((decimal)nTot1 / 10) * 10;    //원단위 절사
                    nTot2 = (long)Math.Truncate((decimal)nTot2 / 10) * 10;    //암 절사

                    SS1.ActiveSheet.Cells[7, 9].Text = string.Format("{0:###,###,###,##0}", nTot1 + nTot2);
                }

                //총금액 업데이트
                fn_Amt_Gesan();
                btnAmt.Enabled = true;

                MessageBox.Show("검진비용금액을 자동으로 저장하였습니다." + "\r\n\r\n" + "금액이 틀릴경우 반드시 금액저장을 하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtMirno)
            {
                if (e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                }
            }
        }

        void eSpdEditModeOff(object sender, EventArgs e)
        {
            
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            int[] nOneInwon = new int[51];
            int result = 0;

            if (sender == SS1)
            {
                
                if (e.Column == 9 && e.Row >= 11 && e.Row <= 39)
                {
                    //수정못하는항목 멘트
                    switch (e.Row)
                    {
                        case 11:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 26:
                        case 28:
                        case 38:
                            MessageBox.Show("이항목은 수정불가합니다..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            eBtnClick(btnSearch, new EventArgs());
                            break;
                        default:
                            break;
                    }

                    //보건소암
                    chb.READ_HIC_MIR_CANCER_Bo(txtMirno.Text.To<long>());
                    if (!clsHcType.TMCB.ROWID.IsNullOrEmpty())
                    {
                        for (int i = 0; i < 51; i++)
                        {
                            nOneInwon[i] = 0;
                        }

                        if (MessageBox.Show("보건소암 항목수정건이 있습니다..정말로 수정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            eBtnClick(btnSearch, new EventArgs());
                            return;
                        }

                        //sheet dlsplay
                        nOneInwon[1] = SS1.ActiveSheet.Cells[12, 9].Text.To<int>(); //위암-위장조영검사
                        nOneInwon[2] = SS1.ActiveSheet.Cells[13, 9].Text.To<int>(); //위암-위내시경검사
                        nOneInwon[3] = SS1.ActiveSheet.Cells[14, 9].Text.To<int>(); //위암-조직검사 1-3
                        nOneInwon[4] = SS1.ActiveSheet.Cells[15, 9].Text.To<int>(); //위암-조직검사
                        nOneInwon[5] = SS1.ActiveSheet.Cells[16, 9].Text.To<int>(); //위암-조직검사
                        nOneInwon[6] = SS1.ActiveSheet.Cells[17, 9].Text.To<int>(); //위암-조직검사
                        nOneInwon[7] = SS1.ActiveSheet.Cells[18, 9].Text.To<int>(); //위암-조직검사

                        nOneInwon[11] = SS1.ActiveSheet.Cells[25, 9].Text.To<int>();//간암-간초음파검사
                        nOneInwon[13] = SS1.ActiveSheet.Cells[27, 9].Text.To<int>();//간암-                (EIA)

                        nOneInwon[14] = SS1.ActiveSheet.Cells[29, 9].Text.To<int>();//대장암-분변잠혈방응(RPHA)
                        nOneInwon[15] = SS1.ActiveSheet.Cells[30, 9].Text.To<int>();//대장암-            (혈색소정량)
                        nOneInwon[16] = SS1.ActiveSheet.Cells[31, 9].Text.To<int>();//대장암-대장이중조영촬영
                        nOneInwon[17] = SS1.ActiveSheet.Cells[32, 9].Text.To<int>();//대장암-대장내시경검사
                        nOneInwon[18] = SS1.ActiveSheet.Cells[33, 9].Text.To<int>();//대장암-조직검사 1-3
                        nOneInwon[19] = SS1.ActiveSheet.Cells[34, 9].Text.To<int>();//대장암-조직검사
                        nOneInwon[20] = SS1.ActiveSheet.Cells[35, 9].Text.To<int>();//대장암-조직검사
                        nOneInwon[21] = SS1.ActiveSheet.Cells[36, 9].Text.To<int>();//대장암-조직검사
                        nOneInwon[22] = SS1.ActiveSheet.Cells[37, 9].Text.To<int>();//대장암-조직검사

                        nOneInwon[23] = SS1.ActiveSheet.Cells[39, 9].Text.To<int>();//유방암-유방촬영
                        nOneInwon[26] = SS1.ActiveSheet.Cells[42, 9].Text.To<int>();//토요일,공휴30%가산

                        clsDB.setBeginTran(clsDB.DbCon);

                        HIC_MIR_CANCER_BO item = new HIC_MIR_CANCER_BO();

                        item.INWON01 = nOneInwon[1];
                        item.INWON02 = nOneInwon[2];
                        item.INWON03 = nOneInwon[3];
                        item.INWON04 = nOneInwon[4];
                        item.INWON05 = nOneInwon[5];
                        item.INWON06 = nOneInwon[6];
                        item.INWON07 = nOneInwon[7];
                        item.INWON11 = nOneInwon[11];
                        item.INWON12 = nOneInwon[12];
                        item.INWON14 = nOneInwon[14];
                        item.INWON15 = nOneInwon[15];
                        item.INWON16 = nOneInwon[16];
                        item.INWON17 = nOneInwon[17];
                        item.INWON18 = nOneInwon[18];
                        item.INWON19 = nOneInwon[19];
                        item.INWON20 = nOneInwon[20];
                        item.INWON21 = nOneInwon[21];
                        item.INWON22 = nOneInwon[22];
                        item.INWON23 = nOneInwon[23];
                        item.INWON26 = nOneInwon[26];
                        item.MIRNO = clsHcType.TMCB.MIRNO;

                        result = hicMirCancerBoService.UpdateAll(item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void fn_Spread_Clear()
        {
            SS1.ActiveSheet.Cells[2, 1].Text = "";  //출력일자
            SS1.ActiveSheet.Cells[2, 8].Text = "";  //암구분
            SS1.ActiveSheet.Cells[6, 7].Text = ""; //사업장명칭                                                   
            SS1.ActiveSheet.Cells[7, 9].Text = "";  //총청구금액

            //실시인원,청구금액
            for (int i = 10; i <= 42; i++)
            {
                SS1.ActiveSheet.Cells[i, 8].Text = "";  
                SS1.ActiveSheet.Cells[i, 9].Text = "";  
                SS1.ActiveSheet.Cells[i, 10].Text = ""; 
                SS1.ActiveSheet.Cells[i, 11].Text = ""; 
            }

            //실시기간
            SS1.ActiveSheet.Cells[46, 1].Text = "";
        }

        string  fn_Bogenso_Name(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.GetNameByGubunCode("18", argCode);

            return rtnVal;
        }

        void fn_Amt_Gesan()
        {
            long nTotAmt = 0;
            int nCol암 = 0;
            int nCol암2 = 0;
            int result = 0;

            nTotAmt = 0;
            if (txtMirno.Text.Trim() == "")
            {
                return;
            }

            if (!clsHcType.TMCB.ROWID.IsNullOrEmpty() && !clsHcType.TMC.ROWID.IsNullOrEmpty())
            {
                if (clsHcType.TMC.Johap == "X")
                {
                    nCol암 = 9;
                    nCol암2 = 5;
                    SS1.ActiveSheet.Cells[2, 10].Text = "[의료급여+보건소]";
                    MessageBox.Show("의료급여+보건소 입니다. 확인바랍니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    nCol암 = 8;
                    nCol암2 = 6;
                }
            }

            nTotAmt = SS1.ActiveSheet.Cells[10, nCol암 + 2].Text.Replace(",", "").To<long>();
            nTotAmt = (long)Math.Truncate((decimal)nTotAmt / 10) * 10;  //원단위 절사

            clsDB.setBeginTran(clsDB.DbCon);

            result = hicMirCancerService.UpdateTamtbyMirNo(nTotAmt, txtMirno.Text);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_CANCER UPDATE시 오류가 발생함", "확인");
                return;
            }

            //보건소암 금액저장
            if (VB.L(SS1.ActiveSheet.Cells[2, 8].Text, "보건소") > 1)
            {
                nTotAmt = 0;
                nTotAmt = SS1.ActiveSheet.Cells[10, 11].Text.Replace(",", "").To<long>();
                nTotAmt = (long)Math.Truncate((decimal)nTotAmt / 10) * 10;  //원단위 절사
                if (nTotAmt > 0)
                {
                    result = hicMirCancerBoService.UpdateTamtbyMirNo(nTotAmt, txtMirno.Text);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_MIR_CANCER_BO UPDATE시 오류가 발생함", "확인");
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}
