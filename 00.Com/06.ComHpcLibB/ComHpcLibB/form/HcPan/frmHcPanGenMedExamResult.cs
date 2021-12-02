using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanGenMedExamResult.cs
/// Description     : 일반건강진단결과표(2010년+생애포함)
/// Author          : 이상훈
/// Create Date     : 2019-12-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm일반건강진단표_2010.frm(Frm일반건강진단표_2010)" />

namespace ComHpcLibB
{
    public partial class frmHcPanGenMedExamResult : Form
    {
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicResBohum2JepsuService hicResBohum2JepsuService = null;
        HicLtdService hicLtdService = null;
        HicJepsuLtdService hicJepsuLtdService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();

        string FstrFDate;
        string FstrTDate;
        string FstrLtdCode;
        string FstrYear;
        string FstrGjBangi;

        long FnWRTNO;   //1차검진번호
        long FnWrtno2;   //2차검진번호
        long FnPano;     //등록번호
        string FstrSex;
        long FnAge;
        string FstrJikGbn;
        string FstrJepDate;
        string FstrIpsaDate;
        string FstrPanjeng;
        long FnGunsok; //직력(근속년수)

        long FnRow;
        int FnDoctCnt;
        long[] FnPanDrNo = new long[10];
        string FstrFDate1;
        string FstrTDate1;
        string FstrFDate2;
        string FstrTDate2;

        string[] FstrPanjengR = new string[121]; //질병의심판정

        long[,] FnCNT1 = new long[3, 22];    //건강진단현황
        long[,] FnCNT2 = new long[7, 19];    //질병유소견자현황
        long[,] FnCNT3 = new long[12, 8];    //사후관리

        string Fstr생애구분;
        string FstrGjYear;

        public frmHcPanGenMedExamResult()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicResBohum2JepsuService = new HicResBohum2JepsuService();
            hicLtdService = new HicLtdService();
            hicJepsuLtdService = new HicJepsuLtdService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.chkR1.Click += new EventHandler(eChkClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.cboPrtCnt.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYear = 0;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);

            SSList_Sheet1.Rows.Get(-1).Height = 20;
            SSList_Sheet1.Columns.Get(3).Visible = false;   //회사코드
            SSList_Sheet1.Columns.Get(4).Visible = false;   //시작일자
            SSList_Sheet1.Columns.Get(5).Visible = false;   //종료일자

            txtLtdCode.Text = "";
            lblLtdName2.Text = "";
            FstrGjYear = cboYear.Text;
            switch (cboBangi.Text.Trim())
            {
                case "상반기":
                    FstrGjBangi = "1";
                    break;
                case "하반기":
                    FstrGjBangi = "2";
                    break;
                default:
                    FstrGjBangi = "*";
                    break;
            }
            FstrGjBangi = "*";
            nYear = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            cboYear.Items.Clear();
            for (int i = 1; i <= 2; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYear));
                nYear -= 1;
            }
            cboYear.SelectedIndex = 1;

            dtpFrDate.Text = cboYear.Text + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;

            //인쇄매수
            cboPrtCnt.Items.Clear();
            cboPrtCnt.Items.Add("1");
            cboPrtCnt.Items.Add("2");
            cboPrtCnt.Items.Add("3");
            cboPrtCnt.SelectedIndex = 0;

            //반기
            cboBangi.Items.Clear();
            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;

            fn_Screen_Clear();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnListView)
            {
                int nREAD = 0;
                int nRow = 0;
                long nCNT = 0;
                long nCNT1 = 0;
                long nCNT2 = 0;
                string strOldData = "";
                string strNewData = "";
                string strOK = "";
                string FstrLtdCode = "";
                string strFDate = "";
                string strTDate = "";
                string strBangi = "";
                string strYear = "";
                long nLtdCode = 0;

                strFDate = dtpFrDate.Text;
                strTDate = dtpToDate.Text;

                strYear = cboYear.Text;
                if (cboBangi.Text == "상반기")
                {
                    strBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strBangi = "2";
                }

                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));


                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 30;

                //해당기간에 검진한 업체를 READ
                List<HIC_JEPSU_LTD> list = hicJepsuLtdService.GetItembyJepDateGjYearGjBangiLtdCode(strFDate, strTDate, strYear, strBangi, nLtdCode);

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                    SSList.ActiveSheet.Cells[i, 1].Text = string.Format("{0:N0}", list[i].CNT.To<string>());
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].LTDCODE.To<string>();
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].MINDATE.To<string>();
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].MAXDATE.To<string>();

                    nCNT1 = list[i].CNT;
                    FstrLtdCode = list[i].LTDCODE.To<string>();
                    strFDate = list[i].MINDATE.To<string>();
                    strTDate = list[i].MAXDATE.To<string>();
                    nCNT2 = 0;

                    //회사별 1차판정건수를 READ
                    nCNT2 += hicJepsuLtdResBohum1Service.GetCountbyJepDateLtdCodeGjYearGjBangi(strFDate, strTDate, FstrLtdCode, strYear, strBangi);

                    //미판정+결과미입력 건수
                    SSList.ActiveSheet.Cells[i, 2].Text = string.Format("{0:N0}", (nCNT1 - nCNT2).To<string>());
                }
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
                    txtLtdCode.Text = LtdHelpItem.CODE.To<string>() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
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

                //strTitle = "회사명: " + VB.Pstr(txtLtdCode.Text, ".", 2);
                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == cboPrtCnt)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                fn_Screen_Clear();

                txtLtdCode.Text = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                FstrFDate = SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                FstrTDate = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();
                if (txtLtdCode.Text.IndexOf(".") != -1)
                {
                    lblLtdName2.Text = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1));
                }
                else
                {
                    lblLtdName2.Text = hb.READ_Ltd_Name(txtLtdCode.Text);
                }

                fn_Screen_Display_Main();
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtLtdCode)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(txtLtdCode, new EventArgs());
                        //SendKeys.Send("{TAB}");
                    }
                }
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            //if (sender == txtLtdCode)
            //{
            //    eBtnClick(btnLtdCode, new EventArgs());
            //}
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (sender == chkR1)
            {
                if (chkR1.Checked == true)
                {
                    SS1.ActiveSheet.Cells[10, 21].Text = "2차건강진단" + "\r\n" + "미수검자"; //(R1,R2)
                }
                else
                {
                    SS1.ActiveSheet.Cells[10, 21].Text = "2차건강진단" + "\r\n" + "미수검자";   //(R2)
                }
            }
        }

        /// <summary>
        /// 1개 회사의 일반건강진단 결과표를 Display
        /// </summary>
        void fn_Screen_Display_Main()
        {
            int nREAD = 0;
            int nRow = 0;
            long nPanjengDrno = 0;
            string strJepDate2 = "";
            string strOK = "";
            string strFrDate = "";
            string strToDate = "";
            string strYear = "";
            string strBangi = "";

            int nCol = 0;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;
            strYear = cboYear.Text;
            if (cboBangi.Text == "상반기")
            {
                strBangi = "1";
            }
            else if (cboBangi.Text == "하반기")
            {
                strBangi = "2";
            }

            Cursor.Current = Cursors.WaitCursor;
            //판정의사 Clear
            FnDoctCnt = 0;
            for (int i = 1; i <= 10; i++)
            {
                FnPanDrNo[i] = 0;
            }
            //1,2차 검진기간
            FstrFDate1 = ""; FstrTDate1 = "";  //1차 검진기간
            FstrFDate2 = ""; FstrTDate2 = "";  //2차 검진기간

            //건강진단현황
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 22; j++)
                {
                    FnCNT1[i, j] = 0;
                }
            }
            //질병유소견자현황
            for (int i = 1; i <= 7; i++)
            {
                for (int j = 1; j <= 19; j++)
                {
                    FnCNT2[i, j] = 0;
                }
            }
            //사후관리
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    FnCNT3[i, j] = 0;
                }
            }
            //질병의심판정
            for (int i = 1; i <= 11; i++)
            {
                FstrPanjengR[i] = "";
            }

            ComFunc.ReadSysDate(clsDB.DbCon);
            FstrLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1);
            fn_Ltd_Info_Display();  //회사주소등 정보를 Sheet에 표시

            //일반건진 1차 명단을 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyJepDateGjYearLtdCode(FstrFDate, FstrTDate, strYear, strBangi, FstrLtdCode);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                FnWRTNO = list[i].WRTNO1;
                FnPano = list[i].PANO;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                FstrJikGbn = list[i].JIKGBN;
                if (list[i].GUNDATE.To<string>() == "")
                {
                    FstrJepDate = list[i].JEPDATE.To<string>();
                }
                else
                {
                    FstrJepDate = list[i].GUNDATE.To<string>();
                }
                FstrIpsaDate = list[i].IPSADATE2.To<string>();
                FstrPanjeng = list[i].PANJENG; //1차 판정
                FnGunsok = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(FstrIpsaDate, FstrJepDate), 3)); //근속년수

                switch (list[i].GJJONG)
                {
                    case "41":
                        Fstr생애구분 = "생애";
                        break;
                    default:
                        Fstr생애구분 = "일반";
                        break;
                }

                //판정상태 Check (R1,R2 판정)
                if (list[i].PANJENGR1 == "1") FstrPanjengR[0] = "1";
                if (list[i].PANJENGR2 == "1") FstrPanjengR[1] = "1";
                if (list[i].PANJENGR3 == "1") FstrPanjengR[2] = "1";
                if (list[i].PANJENGR4 == "1") FstrPanjengR[3] = "1";
                if (list[i].PANJENGR5 == "1") FstrPanjengR[4] = "1";
                if (list[i].PANJENGR6 == "1") FstrPanjengR[5] = "1";
                if (list[i].PANJENGR7 == "1") FstrPanjengR[6] = "1";
                if (list[i].PANJENGR8 == "1") FstrPanjengR[7] = "1";
                if (list[i].PANJENGR9 == "1") FstrPanjengR[8] = "1";

                //1차 검진기간
                if (FstrFDate1 == "") FstrFDate1 = FstrJepDate;
                if (string.Compare(FstrFDate1, FstrJepDate) > 0) FstrFDate1 = FstrJepDate;
                if (FstrTDate1 == "") FstrTDate1 = FstrJepDate;
                if (string.Compare(FstrTDate1, FstrJepDate) < 0) FstrTDate1 = FstrJepDate;

                //2차의 접수번호를 찾음
                HIC_RES_BOHUM2_JEPSU list2 = hicResBohum2JepsuService.GetItembyLtdCodeJepDate(FnPano, FstrJepDate, strYear, strBangi);

                FnWrtno2 = 0;
                if (list2.IsNullOrEmpty())
                {
                    FnWrtno2 = list2.WRTNO;
                    strJepDate2 = list2.JEPDATE.To<string>();
                }

                //1차검진결과를 읽음
                chb.READ_HIC_RES_BOHUM1(FnWRTNO);
                //2차 검진기간
                clsHcType.B2.ROWID = "";
                chb.READ_HIC_RES_BOHUM2(FnWrtno2);
                if (FnWrtno2 > 0)
                {
                    if (FstrFDate2 == "") FstrFDate2 = strJepDate2;
                    if (string.Compare(FstrFDate2, strJepDate2) > 0) FstrFDate2 = strJepDate2;
                    if (FstrTDate2 == "") FstrTDate2 = strJepDate2;
                    if (string.Compare(FstrTDate2, strJepDate2) < 0) FstrTDate2 = strJepDate2;
                    //2차검진결과를 읽음
                    chb.READ_HIC_RES_BOHUM2(FnWrtno2);
                }
                //--------( 판정의사 명단 등록 )---------------
                nPanjengDrno = list[i].PANJENGDRNO;
                if (nPanjengDrno > 0)
                {
                    if (FnDoctCnt == 0)
                    {
                        FnDoctCnt = 1;
                        FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                    }
                    else
                    {
                        strOK = "";
                        for (int j = 0; j < FnDoctCnt; j++)
                        {
                            if (FnPanDrNo[j] == nPanjengDrno)
                            {
                                strOK = "OK";
                                return;
                            }
                        }

                        if (strOK != "OK")
                        {
                            FnDoctCnt += 1;
                            FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                        }
                    }
                }
                //2차판정의사
                if (FnWrtno2 > 0)
                {
                    nPanjengDrno = clsHcType.B2.PanjengDrNo;
                    if (nPanjengDrno > 0)
                    {
                        if (FnDoctCnt == 0)
                        {
                            FnDoctCnt = 1;
                            FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                        }
                        else
                        {
                            strOK = "";
                            for (int j = 0; j < FnDoctCnt; j++)
                            {
                                if (FnPanDrNo[j] == nPanjengDrno)
                                {
                                    strOK = "OK";
                                    return;
                                }
                            }

                            if (strOK != "OK")
                            {
                                FnDoctCnt += 1;
                                FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                            }
                        }
                    }
                }

                fn_Screen_Display_JindanTong(); //건강진단현황 누적
                fn_Screen_Display_YuSogen();   //질병유소견자
            }

            //검진기간
            SS1.ActiveSheet.Cells[4, 10].Text = FstrFDate1 + "~" + FstrTDate1;
            SS1.ActiveSheet.Cells[5, 10].Text = "";

            if (FstrFDate2 != "")
            {
                SS1.ActiveSheet.Cells[5, 10].Text = FstrFDate2 + "~" + FstrTDate2;
            }

            //수진근로자를 대상근로자로 기본 같이 설정
            FnCNT1[0, 1] = FnCNT1[0, 4];
            FnCNT1[0, 2] = FnCNT1[0, 5];
            FnCNT1[1, 1] = FnCNT1[1, 4];
            FnCNT1[1, 2] = FnCNT1[1, 5];
            FnCNT1[2, 1] = FnCNT1[2, 4];
            FnCNT1[2, 2] = FnCNT1[2, 5];

            //건강진단현황 Display
            for (int i = 0; i < 3; i++)
            {
                FnCNT1[i, 0] = FnCNT1[i, 1] + FnCNT1[i, 2];    //대상근로자수계
                FnCNT1[i, 3] = FnCNT1[i, 4] + FnCNT1[i, 5];   //수진근로자수계
                FnCNT1[i, 6] = FnCNT1[i, 7] + FnCNT1[i, 8];   //질병건수
                FnCNT1[i, 10] = FnCNT1[i, 12] + FnCNT1[i, 14]; //질병유소견자계(남)
                FnCNT1[i, 11] = FnCNT1[i, 13] + FnCNT1[i, 15]; //질병유소견자계(여)
                FnCNT1[i, 9] = FnCNT1[i, 10] + FnCNT1[i, 11]; //질병유소견자계(전체)
                FnCNT1[i, 16] = FnCNT1[i, 17] + FnCNT1[i, 18]; //요관찰자
                FnCNT1[i, 19] = FnCNT1[i, 20] + FnCNT1[i, 21]; //미수검자
                for (int j = 0; j < 22; j++)
                {
                    SS1.ActiveSheet.Cells[i + 13, j + 2].Text = string.Format("{0:#####0}", FnCNT1[i, j]);
                }
            }

            //대상자수
            SS1.ActiveSheet.Cells[3, 5].Text = "";
            SS1.ActiveSheet.Cells[4, 5].Text = "";
            SS1.ActiveSheet.Cells[5, 5].Text = "";

            //질병유소견자 현황 Display
            for (int i = 1; i < 7; i++)
            {
                FnCNT2[i, 0] = FnCNT2[i, 1] + FnCNT2[i, 2];
                for (int j = 0; j < 19; j++)
                {
                    FnCNT2[0, j] = FnCNT2[0, j] + FnCNT2[i, j];
                }
            }

            nRow = 19;
            for (int i = 0; i < 7; i++)
            {
                if (FnCNT2[i, 0] > 0)
                {
                    nRow += 1;
                    switch (i)
                    {
                        case 1:
                            SS1.ActiveSheet.Cells[i, 3].Text = "J"; //흉부질환
                            break;
                        case 2:
                            SS1.ActiveSheet.Cells[i, 3].Text = "I"; //순환기계질환
                            break;
                        case 3:
                            SS1.ActiveSheet.Cells[i, 3].Text = "K"; //간장질환
                            break;
                        case 4:
                            SS1.ActiveSheet.Cells[i, 3].Text = "N"; //신장질환
                            break;
                        case 5:
                            SS1.ActiveSheet.Cells[i, 3].Text = "D"; //빈혈증
                            break;
                        case 6:
                            SS1.ActiveSheet.Cells[i, 3].Text = "E"; //당뇨질환
                            break;
                        default:
                            break;
                    }

                    for (int j = 0; j < 19; j++)
                    {
                        SS1.ActiveSheet.Cells[i, j + 5].Text = string.Format("{0:#####0}", FnCNT2[i, j]);
                    }
                }
            }

            //----------( 사후관리 )----------------
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (j)
                    {
                        case 0:
                            nCol = 5;
                            break;
                        case 1:
                            nCol = 7;
                            break;
                        case 2:
                            nCol = 9;
                            break;
                        case 3:
                            nCol = 10;
                            break;
                        case 4:
                            nCol = 12;
                            break;
                        case 5:
                            nCol = 14;
                            break;
                        case 6:
                            nCol = 15;
                            break;
                        case 7:
                            nCol = 17;
                            break;
                        default:
                            break;
                    }
                    SS1.ActiveSheet.Cells[i + 36, nCol].Text = string.Format("{0:#####}", FnCNT3[i, j]);
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 건강진단현황 누적
        /// </summary>
        void fn_Screen_Display_JindanTong()
        {
            long nSahu1 = 0;
            long nSahu2 = 0;
            long nSahu3 = 0;
            string strOK = "";
            string strC = "";
            string strD1 = "";
            string strD2 = "";

            //----------( 수진근로자수 ADD )--------------
            if (FstrSex == "M")
            {
                FnCNT1[0, 4] = FnCNT1[0, 4] + 1;
                if (FstrJikGbn == "3")
                {
                    FnCNT1[1, 4] = FnCNT1[1, 4] + 1; //사무직(남)
                }
                else
                {
                    FnCNT1[2, 4] = FnCNT1[2, 4] + 1; //생산직(남)
                }
            }
            else
            {
                FnCNT1[0, 5] = FnCNT1[0, 5] + 1;
                if (FstrJikGbn == "3")
                {
                    FnCNT1[1, 5] = FnCNT1[1, 5] + 1; //사무직(여)
                }
                else
                {
                    FnCNT1[2, 5] = FnCNT1[2, 5] + 1; //생산직(여)
                }
            }

            //--------------------( 2차건강진단 미수검자 )-------------------
            strOK = "";
            if (chkR1.Checked == true)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (FstrPanjengR[j] == "1")
                    {
                        strOK = "OK";
                        return;
                    }
                }
            }
            else
            {
                if (FstrPanjengR[2] == "1") strOK = "OK";  //고혈압
                if (FstrPanjengR[5] == "1") strOK = "OK";  //당뇨
            }

            //생애이면 1차하고 2차 없으면 미수검자포함
            if (Fstr생애구분 == "생애")
            {
                if (clsHcType.B1.PanDrNo > 0 && clsHcType.B2.PanjengDrNo == 0)
                {
                    strOK = "OK";
                }
                else if (clsHcType.B1.PanDrNo > 0 && clsHcType.B2.ROWID == "")
                {
                    strOK = "OK";
                }
                else
                {
                    strOK = "";
                }
            }

            if (strOK == "OK" && FnWrtno2 == 0)
            {
                if (FstrSex == "M")
                {
                    FnCNT1[0, 20] = FnCNT1[0, 20] + 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 20] = FnCNT1[1, 20] + 1; //사무직(남)
                    }
                    else
                    {
                        FnCNT1[2, 20] = FnCNT1[2, 20] + 1; //생산직(남)
                    }
                }
                else
                {
                    FnCNT1[0, 21] = FnCNT1[0, 21] + 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 21] = FnCNT1[1, 21] + 1; //사무직(여)
                    }
                    else
                    {
                        FnCNT1[2, 21] = FnCNT1[2, 21] + 1; //생산직(여)
                    }
                }
            }

            //판정상태 Check (D1,D2)
            strOK = ""; strD1 = ""; strD2 = "";
            if (clsHcType.B1.PANJENGD11 != "") strD1 = "OK"; strOK = "OK";
            if (clsHcType.B1.PANJENGD12 != "") strD1 = "OK"; strOK = "OK";
            if (clsHcType.B1.PANJENGD13 != "") strD1 = "OK"; strOK = "OK";

            if (clsHcType.B1.PANJENGD21 != "") strD2 = "OK"; strOK = "OK";
            if (clsHcType.B1.PANJENGD22 != "") strD2 = "OK"; strOK = "OK";
            if (clsHcType.B1.PANJENGD23 != "") strD2 = "OK"; strOK = "OK";

            //1차 질병 유소견자
            if (clsHcType.B1.ROWID != "" && strOK == "OK")
            {
                if (strD2 != "")    //일반질병
                {
                    FnCNT3[0, 0] += 1;
                    FnCNT3[3, 0] += 1;
                    if (FstrSex == "M")
                    {
                        FnCNT1[0, 12] = FnCNT1[0, 12] + 1;
                        if (FstrJikGbn == "3")
                        {
                            FnCNT1[1, 12] += 1;  // 사무직(남)
                        }
                        else
                        {
                            FnCNT1[2, 12] += 1;  //기타(남)
                        }
                        FnCNT3[1, 0] += 1;
                        FnCNT3[4, 0] += 1;
                    }
                    else
                    {
                        FnCNT1[0, 13] += 1;
                        if (FstrJikGbn == "3")
                        {
                            FnCNT1[1, 13] += 1;  //사무직(여)
                        }
                        else
                        {
                            FnCNT1[2, 13] += 1;  //기타(여)
                        }
                        FnCNT3[2, 0] += 1;
                        FnCNT3[5, 0] += 1;
                    }
                }
                else if (strD1 != "")   //직업병
                {
                    FnCNT3[0, 0] += 1;
                    FnCNT3[6, 0] += 1;
                    if (FstrSex == "M")
                    {
                        FnCNT1[0, 14] += 1;
                        if (FstrJikGbn == "3")
                        {
                            FnCNT1[1, 14] += 1;  //사무직(남)
                        }
                        else
                        {
                            FnCNT1[2, 14] += 1;  //기타(남)
                        }
                        FnCNT3[1, 0] += 1;
                        FnCNT3[7, 0] += 1;
                    }
                    else
                    {
                        FnCNT1[0, 15] += 1;
                        if (FstrJikGbn == "3")
                        {
                            FnCNT1[1, 15] += 1; //사무직(여)
                        }
                        else
                        {
                            FnCNT1[2, 15] += 1; //기타(여)
                        }
                        FnCNT3[2, 0] += 1;
                        FnCNT3[8, 0] += 1;
                    }
                }
            }

            //D2항목 임의2차판정으로 추가됨
            //2차판정읽어 D2강제표시
            if (clsHcType.B2.ROWID != "" && (clsHcType.B2.Cycle_RES == "3" || clsHcType.B2.Diabetes_Res == "3"))
            {
                strD2 = "OK";
                //일반질병 D2
                FnCNT3[0, 0] += 1;
                FnCNT3[3, 0] += 1;
                if (FstrSex == "M")
                {
                    FnCNT1[0, 12] += 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 12] += 1; //사무직(남)
                    }
                    else
                    {
                        FnCNT1[2, 12] += 1; //기타(남)
                    }
                    FnCNT3[1, 0] += 1;
                    FnCNT3[4, 0] += 1;
                }
                else
                {
                    FnCNT1[0, 13] += 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 13] += 1; //사무직(여)
                    }
                    else
                    {
                        FnCNT1[2, 13] += 1; //기타(여)
                    }
                    FnCNT3[2, 0] += 1;
                    FnCNT3[5, 0] += 1;
                }
            }

            strC = "";
            //C2항목 임의2차판정으로 추가됨
            if (clsHcType.B2.ROWID != "" && clsHcType.B2.Cycle_RES == "2") strC = "OK";
            if (clsHcType.B2.ROWID != "" && clsHcType.B2.Diabetes_Res == "2") strC = "OK";

            //2차 요관찰자
            if (clsHcType.B1.ROWID != "" && strC == "OK")
            {
                FnCNT3[9, 0] += 1;
                if (FstrSex == "M")
                {
                    FnCNT1[0, 17] += 1;
                    FnCNT3[10, 0] += 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 17] += 1; //사무직(남)
                    }
                    else
                    {
                        FnCNT1[2, 17] += 1; //생산직(남)
                    }
                }
                else
                {
                    FnCNT1[0, 18] += 1;
                    FnCNT3[11, 0] += 1;
                    if (FstrJikGbn == "3")
                    {
                        FnCNT1[1, 18] += 1; //사무직(여)
                    }
                    else
                    {
                        FnCNT1[2, 18] += 1; //생산직(여)
                    }
                }
            }

            //사후관리 통계
            if (clsHcType.B1.ROWID != "")
            {
                if (string.Compare(clsHcType.B1.PANJENGSAHU, "1") >= 0)
                {
                    if (strD2 != "")  //일반질병
                    {
                        nSahu1 = long.Parse(clsHcType.B1.PANJENGSAHU);  //일반질병
                        FnCNT3[0, nSahu1 + 1] += 1;
                        FnCNT3[3, nSahu1 + 1] += 1;
                        if (FstrSex == "M")
                        {
                            FnCNT3[1, nSahu1 + 1] += 1;
                            FnCNT3[4, nSahu1 + 1] += 1;
                        }
                        else
                        {
                            FnCNT3[2, nSahu1 + 1] += 1;
                            FnCNT3[5, nSahu1 + 1] += 1;
                        }
                    }
                    if (strD1 != "")  //직업병
                    {
                        nSahu2 = long.Parse(clsHcType.B1.PANJENGSAHU);  //직업병
                        FnCNT3[0, nSahu2 + 1] += 1;
                        FnCNT3[6, nSahu2 + 1] += 1;
                        if (FstrSex == "M")
                        {
                            FnCNT3[1, nSahu2 + 1] += 1;
                            FnCNT3[7, nSahu2 + 1] += 1;
                        }
                        else
                        {
                            FnCNT3[2, nSahu2 + 1] += 1;
                            FnCNT3[8, nSahu2 + 1] += 1;
                        }
                    }
                }
                else
                {
                    //2010 수정
                    if (strD2 != "")  //일반질병
                    {
                        nSahu1 = 4; //근무중치료
                        FnCNT3[0, nSahu1 + 1] += 1;
                        FnCNT3[3, nSahu1 + 1] += 1;
                        if (FstrSex == "M")
                        {
                            FnCNT3[1, nSahu1 + 1] += 1;
                            FnCNT3[4, nSahu1 + 1] += 1;
                        }
                        else
                        {
                            FnCNT3[2, nSahu1 + 1] += 1;
                            FnCNT3[5, nSahu1 + 1] += 1;
                        }
                    }
                    if (strD1 != "")  //직업병
                    {
                        nSahu2 = 4; //근무중치료
                        FnCNT3[0, nSahu2 + 1] += 1;
                        FnCNT3[6, nSahu2 + 1] += 1;
                        if (FstrSex == "M")
                        {
                            FnCNT3[1, nSahu2 + 1] += 1;
                            FnCNT3[7, nSahu2 + 1] += 1;
                        }
                        else
                        {
                            FnCNT3[2, nSahu2 + 1] += 1;
                            FnCNT3[8, nSahu2 + 1] += 1;
                        }
                    }
                }

                //2010 수정
                if (strC != "")  //요관찰자
                {
                    nSahu3 = 0;  //건강주의

                    switch (clsHcType.B2.DIABETES_RES_CARE)
                    {
                        case "1":
                            nSahu3 = 7;   //기타
                            break;
                        case "2":
                            nSahu3 = 5;   //추적검사
                            break;
                        case "3":
                            nSahu3 = 4;   //근무중치료
                            break;
                        default:
                            break;
                    }

                    if (nSahu3 == 0)
                    {
                        switch (clsHcType.B2.CYCLE_RES_CARE)
                        {
                            case "1":
                                nSahu3 = 7;   //기타
                                break;
                            case "2":
                                nSahu3 = 5;   //추적검사
                                break;
                            case "3":
                                nSahu3 = 4;   //근무중치료
                                break;
                            default:
                                nSahu3 = 7;   //기타
                                break;
                        }
                    }

                    FnCNT3[9, nSahu3 + 1] += 1;
                    if (FstrSex == "M")
                    {
                        FnCNT3[10, nSahu3 + 1] += 1;
                    }
                    else
                    {
                        FnCNT3[11, nSahu3 + 1] += 1;
                    }
                }
            }

            //질병의심판정 Clear
            for (int i = 0; i < 9; i++)
            {
                FstrPanjengR[i] = "";
            }
        }

        /// <summary>
        /// 질병유소견자
        /// </summary>
        void fn_Screen_Display_YuSogen()
        {
            int n = 0;
            int nYoCnt = 0;

            if (clsHcType.B1.ROWID == "") return;
            if (string.Compare(clsHcType.B1.Panjeng, "1") < 0) return;

            if (clsHcType.B1.PANJENGD21 == "J") n = 1; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD21 == "I") n = 2; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD21 == "E") n = 6; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD21 == "K") n = 3; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD21 == "N") n = 4; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD21 == "D") n = 5; fn_YuSogen_ADD_SUB(n);

            if (clsHcType.B1.PANJENGD22 == "J") n = 1; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD22 == "I") n = 2; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD22 == "E") n = 6; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD22 == "K") n = 3; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD22 == "N") n = 4; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD22 == "D") n = 5; fn_YuSogen_ADD_SUB(n);

            if (clsHcType.B1.PANJENGD23 == "J") n = 1; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD23 == "I") n = 2; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD23 == "E") n = 6; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD23 == "K") n = 3; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD23 == "N") n = 4; fn_YuSogen_ADD_SUB(n);
            if (clsHcType.B1.PANJENGD23 == "D") n = 5; fn_YuSogen_ADD_SUB(n);

            //---------------(직력별 누적 )------------------

            //2010 2차판정으로 추가됨
            if (clsHcType.B2.ROWID != "" && clsHcType.B2.Cycle_RES == "3") n = 3; fn_YuSogen_ADD_SUB(n);        //2차고혈압  I 강제세팅
            if (clsHcType.B2.ROWID != "" && clsHcType.B2.Diabetes_Res == "3") n = 7; fn_YuSogen_ADD_SUB(n);    //2차당뇨병  E 강제세팅
        }

        void fn_YuSogen_ADD_SUB(int n)
        {
            int M = 0;

            if (FnGunsok < 1)
            {
                M = 3;
            }
            else if (FnGunsok >= 1 && FnGunsok <= 4)
            {
                M = 5;
            }
            else if (FnGunsok >= 5 && FnGunsok <= 9)
            {
                M = 7;
            }
            else
            {
                M = 9;
            }

            if (FstrSex == "M")
            {
                FnCNT2[n, M] += 1;
                FnCNT2[n, 0] += 1;
                FnCNT2[n, 1] += 1;
                //질병건수 누적
                if (FstrJikGbn == "3")
                {
                    FnCNT1[0, 7] += 1;
                    FnCNT1[1, 7] += 1;
                }
                else
                {
                    FnCNT1[0, 7] += 1;
                    FnCNT1[2, 7] += 1;
                }
            }
            else
            {
                FnCNT2[n, M + 1] += 1;
                FnCNT2[n, 0] += 1;
                FnCNT2[n, 2] += 1;
                //질병건수 누적
                if (FstrJikGbn == "3")
                {
                    FnCNT1[0, 8] += 1;
                    FnCNT1[1, 8] += 1;
                }
                else
                {
                    FnCNT1[0, 8] += 1;
                    FnCNT1[2, 8] += 1;
                }
            }

            //------------( 연령별 누적 )-------------------
            if (FnAge < 30)
            {
                M = 11;
            }
            else if (FnAge >= 30 && FnAge <= 39)
            {
                M = 13;
            }
            else if (FnAge >= 40 && FnAge <= 49)
            {
                M = 15;
            }
            else
            {
                M = 17;
            }

            if (FstrSex == "M")
            {
                FnCNT2[n, M] += 1;
            }
            else
            {
                FnCNT2[n, M + 1] += 1;
            }
        }

        /// <summary>
        /// 선택한 회사의 정보를 Sheet에 Display
        /// </summary>
        void fn_Ltd_Info_Display()
        {
            //사업장 기초사항
            HIC_LTD list = hicLtdService.GetAllbyCode(FstrLtdCode);

            if (!list.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[3, 20].Text = list.SANKIHO;
                if (list.SANKIHO.Length == 11)
                {
                    SS1.ActiveSheet.Cells[3, 20].Text = VB.Left(list.SANKIHO, 4) + "-" + VB.Right(list.SANKIHO, 7);
                }
                SS1.ActiveSheet.Cells[4, 20].Text = list.SAUPNO;
                SS1.ActiveSheet.Cells[7, 2].Text = list.SANGHO;
                SS1.ActiveSheet.Cells[5, 20].Text = list.UPJONG;
                SS1.ActiveSheet.Cells[7, 17].Text = "주요생산품 : " + list.JEPUMLIST;
                SS1.ActiveSheet.Cells[8, 2].Text = list.JUSO + " " + list.JUSODETAIL;
                SS1.ActiveSheet.Cells[8, 10].Text = "전화번호:" + VB.Space(1) + list.TEL;
                SS1.ActiveSheet.Cells[8, 20].Text = "";
                SS1.ActiveSheet.Cells[43, 18].Text = list.DAEPYO;
            }
            else
            {
                SS1.ActiveSheet.Cells[3, 20].Text = "";
                SS1.ActiveSheet.Cells[4, 20].Text = "";
                SS1.ActiveSheet.Cells[7, 2].Text = "";
                SS1.ActiveSheet.Cells[7, 12].Text = "";
                SS1.ActiveSheet.Cells[7, 17].Text = "";
                SS1.ActiveSheet.Cells[7, 20].Text = "";
                SS1.ActiveSheet.Cells[7, 20].Text = "";
                SS1.ActiveSheet.Cells[8, 2].Text = "";
                SS1.ActiveSheet.Cells[8, 10].Text = "";
                SS1.ActiveSheet.Cells[8, 20].Text = "";
                SS1.ActiveSheet.Cells[36, 18].Text = "";
                SS1.ActiveSheet.Cells[43, 18].Text = "";
            }
            SS1.ActiveSheet.Cells[38, 18].Text = "작성일자 : " + VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일";
            SS1.ActiveSheet.Cells[41, 18].Text = "건강진단기관명: 포항성모병원";
        }

        void fn_Screen_Clear()
        {
            int k = 0;
            int jj = 0;
            int nRow = 0;
            int nREA = 0;
            string FstrLtdCode="";
            string strSex = "";
            int nSujinM = 0;
            int nSujinF = 0;
            int nSuJinTot = 0;
            int nYSIlbanM = 0;
            int nYSJikupM = 0;
            int nJilbyengM = 0;
            int nYGanM = 0;
            int nYSIlbanF = 0;
            int nYSJikupF = 0;
            int nJilbyengF = 0;
            int nYGanF = 0;
            int nAge = 0;
            string strChest = "";
            int[] nChest = new int[16];
            string strCycle = "";
            int[] nCycle = new int[16];
            string strGoJi              = "";
            int[] nGoJi = new int[16];
            string strLiver             = "";
            int[] nLiver = new int[16];
            string strKidney            = "";
            int[] nKidney = new int[16];
            string strAnemia            = "";
            int[] nAnemia = new int[16];
            string strDiabetes          = "";
            int[] nDiabetes = new int[16];
            string  strSahu1 = "";
            string  strSahu2 = "";
            string  strSahu3 = "";
            string strSahu4 = "";
            int[] nSahu = new int[28];
            int[] nSahu2 = new int[14];
            int[] nData = new int[7];
            string[]  strData = new string[7];
            string  strData21 = "";
            string  strData22 = "";
            string  strData23 = "";
            string  strData24 = "";
            string  strData25 = "";
            string  strData26 = "";
            string  strData27 = "";
            string  strDataJ1 = "";
            string  strDataJ2 = "";
            string strDataJ3 = "";
            int[] nCount = new int[4];
            int[] nSahuTot = new int[8];
            long nLicense = 0;
            string strDrname = "";

            string strPanjeng = "";
            string strFDate = "";
            string strTDate = "";
            string strYY = "";
            string strYY1 = "";
            string strYY2 = "";
            string strYY3 = "";
            string strMM = "";
            string strSname = "";

            txtLtdCode.Text = "";
            FstrFDate = "";
            FstrTDate = "";

            SS1.ActiveSheet.Cells[7, 2].Text = "";
            SS1.ActiveSheet.Cells[7, 11].Text = "";
            SS1.ActiveSheet.Cells[7, 19].Text = "";
            SS1.ActiveSheet.Cells[8, 2].Text = "";
            SS1.ActiveSheet.Cells[8, 19].Text = "";
            SS1.ActiveSheet.Cells[36, 18].Text = "";
            SS1.ActiveSheet.Cells[41, 18].Text = "";
            //의사명 조회
            SS1.ActiveSheet.Cells[44, 18].Text = "";
            SS1.ActiveSheet.Cells[45, 18].Text = "";
            SS1.ActiveSheet.Cells[46, 18].Text = "";
            SS1.ActiveSheet.Cells[4, 20].Text = "";
            SS1.ActiveSheet.Cells[5, 20].Text = "";

            //질병건수,질병유소견자수,요관찰자수
            SS1.ActiveSheet.Cells[13, 2, 15, 23].Text = "";

            for (int i = 0; i < 7; i++)
            {
                nRow = 20 + k;
                SS1.ActiveSheet.Cells[nRow, 3].Text = "";
                for (int j = 0; j < 8; j++)
                {
                    switch (i)
                    {
                        case 0:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 1:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 2:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 3:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 4:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 5:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";
                            break;
                        case 6:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) + 7].Text = "";                            
                            break;
                        default:
                            break;
                    }
                }

                for (int j = 8; j < 16; j++)
                {
                    switch (i)
                    {
                        case 0:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 1:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 2:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 3:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 4:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 5:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        case 6:
                            SS1.ActiveSheet.Cells[nRow, (j * 2) - 8].Text = "";
                            break;
                        default:
                            break;
                    }
                }
                SS1.ActiveSheet.Cells[nRow, 5].Text = "";
                SS1.ActiveSheet.Cells[nRow, 6].Text = "";
                SS1.ActiveSheet.Cells[nRow, 7].Text = "";
                k += 1;
            }

            //합계 계산
            SS1.ActiveSheet.Cells[19, 5].Text = "";
            SS1.ActiveSheet.Cells[19, 6].Text = "";
            SS1.ActiveSheet.Cells[19, 7].Text = "";
            for (int i = 0; i < 8; i++)
            {
                SS1.ActiveSheet.Cells[19, (i * 2) + 7].Text = "";
                SS1.ActiveSheet.Cells[19, (i * 2) + 8].Text = "";
            }
            //질병건수
            SS1.ActiveSheet.Cells[13, 8].Text = "";
            SS1.ActiveSheet.Cells[13, 9].Text = "";
            SS1.ActiveSheet.Cells[13, 10].Text = "";
            SS1.ActiveSheet.Cells[14, 8].Text = "";
            SS1.ActiveSheet.Cells[14, 9].Text = "";
            SS1.ActiveSheet.Cells[14, 10].Text = "";
            SS1.ActiveSheet.Cells[15, 8].Text = "";
            SS1.ActiveSheet.Cells[15, 9].Text = "";
            SS1.ActiveSheet.Cells[15, 10].Text = "";

            //질병유소견자 사후관리 계
            SS1.ActiveSheet.Cells[36, 5].Text = "";
            SS1.ActiveSheet.Cells[36, 7].Text = "";
            SS1.ActiveSheet.Cells[36, 9].Text = "";
            SS1.ActiveSheet.Cells[36, 10].Text = "";
            SS1.ActiveSheet.Cells[36, 12].Text = "";
            SS1.ActiveSheet.Cells[36, 14].Text = "";
            SS1.ActiveSheet.Cells[36, 15].Text = "";
            SS1.ActiveSheet.Cells[36, 17].Text = "";
            for (int i = 0; i < 2; i++)
            {
                jj = 0;
                k = 0;
                SS1.ActiveSheet.Cells[37 + i, 5].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 7].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 9].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 10].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 12].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 14].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 15].Text = "";
                SS1.ActiveSheet.Cells[37 + i, 17].Text = "";
            }
            //일반질병 소계
            SS1.ActiveSheet.Cells[39, 5].Text = "";
            SS1.ActiveSheet.Cells[39, 7].Text = "";
            SS1.ActiveSheet.Cells[39, 9].Text = "";
            SS1.ActiveSheet.Cells[39, 10].Text = "";
            SS1.ActiveSheet.Cells[39, 12].Text = "";
            SS1.ActiveSheet.Cells[39, 14].Text = "";
            SS1.ActiveSheet.Cells[39, 15].Text = "";
            SS1.ActiveSheet.Cells[39, 17].Text = "";
            for (int i = 0; i < 2; i++)
            {
                //일반질병(남,여)
                SS1.ActiveSheet.Cells[40 + i, 5].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 7].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 9].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 10].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 12].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 14].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 15].Text = "";
                SS1.ActiveSheet.Cells[40 + i, 17].Text = "";
            }
            //직업병 소계
            SS1.ActiveSheet.Cells[42, 5].Text = "";
            SS1.ActiveSheet.Cells[42, 7].Text = "";
            SS1.ActiveSheet.Cells[42, 9].Text = "";
            SS1.ActiveSheet.Cells[42, 10].Text = "";
            SS1.ActiveSheet.Cells[42, 12].Text = "";
            SS1.ActiveSheet.Cells[42, 14].Text = "";
            SS1.ActiveSheet.Cells[42, 15].Text = "";
            SS1.ActiveSheet.Cells[42, 17].Text = "";
            for (int i = 0; i < 2; i++)
            {
                //직업병(남,여)
                SS1.ActiveSheet.Cells[43 + i, 5].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 7].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 9].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 10].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 12].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 14].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 15].Text = "";
                SS1.ActiveSheet.Cells[43 + i, 17].Text = "";
            }
            //요관찰자 소계
            SS1.ActiveSheet.Cells[45, 5].Text = "";
            SS1.ActiveSheet.Cells[45, 7].Text = "";
            SS1.ActiveSheet.Cells[45, 9].Text = "";
            SS1.ActiveSheet.Cells[45, 10].Text = "";
            SS1.ActiveSheet.Cells[45, 12].Text = "";
            SS1.ActiveSheet.Cells[45, 14].Text = "";
            SS1.ActiveSheet.Cells[45, 15].Text = "";
            SS1.ActiveSheet.Cells[45, 17].Text = "";
            for (int i = 0; i < 2; i++)
            {
                //직업병(남,여)
                SS1.ActiveSheet.Cells[46 + i, 5].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 7].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 9].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 10].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 12].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 14].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 15].Text = "";
                SS1.ActiveSheet.Cells[46 + i, 17].Text = "";
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
