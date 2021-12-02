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
/// File Name       : frmHcBillExpenses.cs
/// Description     : 검진비용청구서 [2020]
/// Author          : 이상훈
/// Create Date     : 2020-12-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm검진청구서_2020.frm(Frm검진청구서_2020)" />

namespace HC_Bill
{
    public partial class frmHcBillExpenses : Form
    {
        HicMirBohumService hicMirBohumService = null;

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

        string FstrMirNo;

        public frmHcBillExpenses()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcBillExpenses(string strMirNo)
        {
            InitializeComponent();

            FstrMirNo = strMirNo;

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicMirBohumService = new HicMirBohumService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            int nYY = 0;

            txtLtdCode.Text = "";
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
            else if(sender == btnPrint)
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
                long nRead1 = 0;

                long[] nOneInwon = new long[31];
                long[] nTwoInwon = new long[31];
                long nTot1 = 0;
                long nTot2 = 0;

                long nBreAmt1 = 0;
                long nBreAmt2 = 0;
                string strBreFlag = "";
                string strFrDate = "";
                string strToDate = "";

                for (int i = 0; i < 31; i++)
                {
                    nOneInwon[i] = 0;
                    nTwoInwon[i] = 0;
                }

                nTot1 = 0;    nTot2 = 0;     nBreAmt1 = 0;  nBreAmt2 = 0;

                fn_Spread_Clear();

                strYear = cboYear.Text;
                if (txtMirno.Text == "")
                {
                    return;
                }

                chb.READ_HIC_MIR_BOHUM(txtMirno.Text.To<long>());
                    
                if (clsHcType.TMB.Life_Gbn == "Y")
                {
                    lbl_Life.Visible = true;
                }
                else
                {
                    lbl_Life.Visible = false;
                }

                List<HIC_MIR_BOHUM> list = hicMirBohumService.GetItembyMirNoLtdCodeYear(txtMirno.Text.To<long>(), strYear);

                nREAD = list.Count;

                if (nREAD == 0)
                {
                    MessageBox.Show("자료가없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        strkiho = list[i].KIHO;
                        strLtdCode = string.Format("{0:#####}", list[i].LTDCODE2);
                        strLtdName = hb.READ_Ltd_Name(string.Format("{0:#####}", list[i].LTDCODE2));
                        strSDate = list[i].FRDATE + " ~ " + list[i].TODATE;
                        strFrDate = list[i].FRDATE;
                        strToDate = list[i].TODATE;

                        //1차 인원
                        nOneInwon[1] =  list[i].ONE_INWON011;  //상담료
                        nOneInwon[2] =  list[i].ONE_INWON012;  //흉부
                        nOneInwon[3] =  list[i].ONE_INWON021;  //요검사
                        nOneInwon[4] =  list[i].ONE_INWON022;  //혈색소
                        nOneInwon[5] =  list[i].ONE_INWON031;  //식전혈당
                        nOneInwon[6] =  list[i].ONE_INWON032;  //총콜레스테롤
                        nOneInwon[7] =  list[i].ONE_INWON041;  //HDL콜레스테롤
                        nOneInwon[8] =  list[i].ONE_INWON042;  //트리글리세라이드
                        nOneInwon[9] =  list[i].ONE_INWON051;  //ALT
                        nOneInwon[10] = list[i].ONE_INWON052;  //AST
                        nOneInwon[11] = list[i].ONE_INWON061;  //감마지티피
                        nOneInwon[12] = list[i].ONE_INWON062;  //혈청크레아티닌
                        nOneInwon[13] = list[i].ONE_INWON071;  //간염항원-일반
                        nOneInwon[14] = list[i].ONE_INWON072;  //간염항원-정밀
                        nOneInwon[15] = list[i].ONE_INWON081;  //간염항체-일반
                        nOneInwon[16] = list[i].ONE_INWON082;  //간염항체-정밀
                        nOneInwon[17] = list[i].ONE_INWON091;  //양방사선골밀도검사
                        nOneInwon[18] = list[i].ONE_INWON092;  //양방사선말단골밀도검사
                        nOneInwon[19] = list[i].ONE_INWON101;  //정량적전산화단층골밀도검사
                        nOneInwon[20] = list[i].ONE_INWON102;  //초음파골밀도검사
                        nOneInwon[21] = list[i].ONE_INWON111;  //노인신체기능검사
                        nOneInwon[22] = list[i].ONE_INWON112;  //LDL콜레스테롤(추가분)
                        nOneInwon[23] = list[i].ONE_INWON013;  //토.공휴일 가산

                        //2차 인원
                        nTwoInwon[1] =  list[i].TWO_INWON01;   //상담료
                        nTwoInwon[2] =  list[i].TWO_INWON02;   //식전혈당(혈액화확분석기)
                        nTwoInwon[3] =  list[i].TWO_INWON03;   //식전혈당(자가혈당측정기)
                        nTwoInwon[4] =  list[i].TWO_INWON04;   //생활습관 상담료(기본료)
                        nTwoInwon[5] =  list[i].TWO_INWON05;   //흡연
                        nTwoInwon[6] =  list[i].TWO_INWON06;   //음주
                        nTwoInwon[7] =  list[i].TWO_INWON07;   //운동
                        nTwoInwon[8] =  list[i].TWO_INWON08;   //영양
                        nTwoInwon[9] =  list[i].TWO_INWON09;   //비만
                        nTwoInwon[10] = list[i].TWO_INWON10;   //우울증(CES-D)
                        nTwoInwon[11] = list[i].TWO_INWON11;   //우울증(GDS)
                        nTwoInwon[12] = list[i].TWO_INWON12;   //인지기능검사(치매)(KDSQ-C)
                        nTwoInwon[13] = list[i].TWO_INWON16;   //토.공휴일 가산
                    }
                }

                SS1.ActiveSheet.Cells[2, 1].Text = "  출력일자 : " + clsPublic.GstrSysDate;
                SS1.ActiveSheet.Cells[6, 7].Text = "  " + strLtdName;       //사업장명칭
                SS1.ActiveSheet.Cells[6, 10].Text = "    " + strkiho;        //사업장기호
                //===============================================================================

                SS1.ActiveSheet.Cells[10, 5].Text = nOneInwon[1].To<string>();      //상담1
                SS1.ActiveSheet.Cells[10, 10].Text = nTwoInwon[1].To<string>();     //상담2
                                                                                    
                SS1.ActiveSheet.Cells[11, 5].Text = nOneInwon[23].To<string>();     //토.공휴일 가산
                SS1.ActiveSheet.Cells[12, 10].Text = nTwoInwon[13].To<string>();    //토.공휴일 가산

                SS1.ActiveSheet.Cells[13, 10].Text = nTwoInwon[2].To<string>();     //혈액화학분석기
                SS1.ActiveSheet.Cells[14, 10].Text = nTwoInwon[3].To<string>();     //자가혈당측정기
                SS1.ActiveSheet.Cells[15, 10].Text = nTwoInwon[4].To<string>();     //생활습관 기본상담

                SS1.ActiveSheet.Cells[16, 5].Text = nOneInwon[2].To<string>();      //흉부 14*17
                SS1.ActiveSheet.Cells[16, 10].Text = nTwoInwon[5].To<string>();     //생활습관 흡연

                SS1.ActiveSheet.Cells[17, 5].Text = nOneInwon[3].To<string>();      //요검사
                SS1.ActiveSheet.Cells[17, 10].Text = nTwoInwon[6].To<string>();     //생활습관 음주

                SS1.ActiveSheet.Cells[18, 5].Text = nOneInwon[4].To<string>();      //혈색소
                SS1.ActiveSheet.Cells[18, 10].Text = nTwoInwon[7].To<string>();     //생활습관 운동

                SS1.ActiveSheet.Cells[19, 5].Text = nOneInwon[5].To<string>();      //식전혈당
                SS1.ActiveSheet.Cells[19, 10].Text = nTwoInwon[8].To<string>();     //생활습관 영양

                SS1.ActiveSheet.Cells[20, 5].Text = nOneInwon[6].To<string>();      //총콜레스테롤
                SS1.ActiveSheet.Cells[20, 10].Text = nTwoInwon[9].To<string>();     //생활습관 비만

                SS1.ActiveSheet.Cells[21, 5].Text = nOneInwon[7].To<string>();      //HDL콜레스테롤\
                SS1.ActiveSheet.Cells[21, 10].Text = nTwoInwon[10].To<string>();    //우울증(CES-D)

                SS1.ActiveSheet.Cells[22, 5].Text = nOneInwon[22].To<string>();     //LDL콜레스테롤
                SS1.ActiveSheet.Cells[22, 10].Text = nTwoInwon[11].To<string>();    //우울증(GDS)

                SS1.ActiveSheet.Cells[23, 5].Text = nOneInwon[8].To<string>();      //트리글리세라이드
                SS1.ActiveSheet.Cells[24, 5].Text = nOneInwon[9].To<string>();      //AST
                SS1.ActiveSheet.Cells[24, 10].Text = nTwoInwon[12].To<string>();    //인지기능검사

                SS1.ActiveSheet.Cells[25, 5].Text = nOneInwon[10].To<string>();     //ALT
                SS1.ActiveSheet.Cells[26, 5].Text = nOneInwon[11].To<string>();     //감마지티피
                SS1.ActiveSheet.Cells[27, 5].Text = nOneInwon[12].To<string>();     //혈청크레아티닌
                SS1.ActiveSheet.Cells[30, 5].Text = nOneInwon[13].To<string>();     //항원일반
                SS1.ActiveSheet.Cells[31, 5].Text = nOneInwon[14].To<string>();     //항원정밀
                SS1.ActiveSheet.Cells[32, 5].Text = nOneInwon[15].To<string>();     //항체일반
                SS1.ActiveSheet.Cells[33, 5].Text = nOneInwon[16].To<string>();     //항체정밀
                SS1.ActiveSheet.Cells[34, 5].Text = nOneInwon[17].To<string>();     //양방선골밀도
                SS1.ActiveSheet.Cells[35, 5].Text = nOneInwon[18].To<string>();     //양방사선말단골밀도
                SS1.ActiveSheet.Cells[36, 5].Text = nOneInwon[19].To<string>();     //적량적전산화단층
                SS1.ActiveSheet.Cells[37, 5].Text = nOneInwon[20].To<string>();     //초음파골밀도
                SS1.ActiveSheet.Cells[38, 5].Text = nOneInwon[21].To<string>();     //노인신체기능검사

                SS1.ActiveSheet.Cells[39, 5].Text = nOneInwon[21].To<string>();     //1차 인원수
                SS1.ActiveSheet.Cells[39, 10].Text = nOneInwon[21].To<string>();    //2차 인원수

                SS1.ActiveSheet.Cells[39, 1].Text = "검진기간 : " + strSDate + "  파일명 : " + list[0].FILENAME + "  [ 청구번호 : " + txtMirno.Text + " ]";

                //금액을 display
                SS1.ActiveSheet.Cells[10, 4].Text = list[0].ONE_QTY.To<string>();   //1차 인원수
                SS1.ActiveSheet.Cells[10, 10].Text = nOneInwon[1].To<string>();     //2차 인원수

                nOneInwon[1] = SS1.ActiveSheet.Cells[10, 4].Text.Replace(",", "").To<long>() * nOneInwon[1];      
                SS1.ActiveSheet.Cells[10, 4].Text = nOneInwon[1].To<string>();
                nTwoInwon[1] = SS1.ActiveSheet.Cells[10, 9].Text.Replace(",", "").To<long>() * nTwoInwon[1];      
                SS1.ActiveSheet.Cells[10, 11].Text = nTwoInwon[1].To<string>();

                nOneInwon[23] = SS1.ActiveSheet.Cells[11, 4].Text.Replace(",", "").To<long>() * nOneInwon[23];
                SS1.ActiveSheet.Cells[11, 6].Text = nOneInwon[23].To<string>();
                nTwoInwon[13] = SS1.ActiveSheet.Cells[12, 9].Text.Replace(",", "").To<long>() * nTwoInwon[13];
                SS1.ActiveSheet.Cells[12, 11].Text = nTwoInwon[13].To<string>();

                nTwoInwon[2] = SS1.ActiveSheet.Cells[13, 9].Text.Replace(",", "").To<long>() * nTwoInwon[2];
                SS1.ActiveSheet.Cells[13, 11].Text = nTwoInwon[2].To<string>();

                nTwoInwon[3] = SS1.ActiveSheet.Cells[14, 9].Text.Replace(",", "").To<long>() * nTwoInwon[3];
                SS1.ActiveSheet.Cells[14, 11].Text = nTwoInwon[3].To<string>();

                nTwoInwon[4] = SS1.ActiveSheet.Cells[15, 9].Text.Replace(",", "").To<long>() * nTwoInwon[4];
                SS1.ActiveSheet.Cells[15, 11].Text = nTwoInwon[4].To<string>();

                nOneInwon[2] = SS1.ActiveSheet.Cells[16, 4].Text.Replace(",", "").To<long>() * nOneInwon[2];
                SS1.ActiveSheet.Cells[16, 6].Text = nOneInwon[2].To<string>();

                nTwoInwon[5] = SS1.ActiveSheet.Cells[16, 9].Text.Replace(",", "").To<long>() * nTwoInwon[5];
                SS1.ActiveSheet.Cells[16, 11].Text = nTwoInwon[5].To<string>();

                nOneInwon[3] = SS1.ActiveSheet.Cells[17, 4].Text.Replace(",", "").To<long>() * nOneInwon[3];
                SS1.ActiveSheet.Cells[17, 6].Text = nOneInwon[3].To<string>();

                nTwoInwon[6] = SS1.ActiveSheet.Cells[17, 9].Text.Replace(",", "").To<long>() * nTwoInwon[6];
                SS1.ActiveSheet.Cells[17, 11].Text = nTwoInwon[6].To<string>();

                nOneInwon[4] = SS1.ActiveSheet.Cells[18, 4].Text.Replace(",", "").To<long>() * nOneInwon[4];
                SS1.ActiveSheet.Cells[18, 6].Text = nOneInwon[4].To<string>();

                nTwoInwon[7] = SS1.ActiveSheet.Cells[18, 9].Text.Replace(",", "").To<long>() * nTwoInwon[7];
                SS1.ActiveSheet.Cells[18, 11].Text = nTwoInwon[7].To<string>();

                nOneInwon[5] = SS1.ActiveSheet.Cells[19, 4].Text.Replace(",", "").To<long>() * nOneInwon[5];
                SS1.ActiveSheet.Cells[19, 6].Text = nOneInwon[5].To<string>();

                nTwoInwon[8] = SS1.ActiveSheet.Cells[19, 9].Text.Replace(",", "").To<long>() * nTwoInwon[8];
                SS1.ActiveSheet.Cells[19, 11].Text = nTwoInwon[8].To<string>();

                nOneInwon[6] = SS1.ActiveSheet.Cells[20, 4].Text.Replace(",", "").To<long>() * nOneInwon[6];
                SS1.ActiveSheet.Cells[20, 6].Text = nOneInwon[6].To<string>();

                nTwoInwon[9] = SS1.ActiveSheet.Cells[20, 9].Text.Replace(",", "").To<long>() * nTwoInwon[9];
                SS1.ActiveSheet.Cells[20, 11].Text = nTwoInwon[9].To<string>();

                nOneInwon[7] = SS1.ActiveSheet.Cells[21, 4].Text.Replace(",", "").To<long>() * nOneInwon[7];
                SS1.ActiveSheet.Cells[21, 6].Text = nOneInwon[7].To<string>();

                nTwoInwon[10] = SS1.ActiveSheet.Cells[21, 9].Text.Replace(",", "").To<long>() * nTwoInwon[10];
                SS1.ActiveSheet.Cells[21, 11].Text = nTwoInwon[10].To<string>();

                nOneInwon[22] = SS1.ActiveSheet.Cells[22, 4].Text.Replace(",", "").To<long>() * nOneInwon[22];
                SS1.ActiveSheet.Cells[22, 6].Text = nOneInwon[22].To<string>();

                nTwoInwon[11] = SS1.ActiveSheet.Cells[22, 9].Text.Replace(",", "").To<long>() * nTwoInwon[11];
                SS1.ActiveSheet.Cells[22, 11].Text = nTwoInwon[11].To<string>();

                nOneInwon[8] = SS1.ActiveSheet.Cells[23, 4].Text.Replace(",", "").To<long>() * nOneInwon[8];
                SS1.ActiveSheet.Cells[23, 6].Text = nOneInwon[8].To<string>();

                nTwoInwon[12] = SS1.ActiveSheet.Cells[23, 9].Text.Replace(",", "").To<long>() * nTwoInwon[12];
                SS1.ActiveSheet.Cells[23, 11].Text = nTwoInwon[12].To<string>();

                nOneInwon[9] = SS1.ActiveSheet.Cells[24, 4].Text.Replace(",", "").To<long>() * nOneInwon[9];
                SS1.ActiveSheet.Cells[24, 6].Text = nOneInwon[9].To<string>();

                nTwoInwon[13] = SS1.ActiveSheet.Cells[24, 9].Text.Replace(",", "").To<long>() * nTwoInwon[13];
                SS1.ActiveSheet.Cells[24, 11].Text = nTwoInwon[13].To<string>();

                nOneInwon[10] = SS1.ActiveSheet.Cells[25, 4].Text.Replace(",", "").To<long>() * nOneInwon[10];
                SS1.ActiveSheet.Cells[25, 6].Text = nOneInwon[10].To<string>();

                nOneInwon[11] = SS1.ActiveSheet.Cells[26, 4].Text.Replace(",", "").To<long>() * nOneInwon[11];
                SS1.ActiveSheet.Cells[26, 6].Text = nOneInwon[11].To<string>();

                nOneInwon[12] = SS1.ActiveSheet.Cells[27, 4].Text.Replace(",", "").To<long>() * nOneInwon[12];
                SS1.ActiveSheet.Cells[27, 6].Text = nOneInwon[12].To<string>();

                nOneInwon[13] = SS1.ActiveSheet.Cells[30, 4].Text.Replace(",", "").To<long>() * nOneInwon[13];
                SS1.ActiveSheet.Cells[30, 6].Text = nOneInwon[13].To<string>();

                nOneInwon[14] = SS1.ActiveSheet.Cells[31, 4].Text.Replace(",", "").To<long>() * nOneInwon[14];
                SS1.ActiveSheet.Cells[31, 6].Text = nOneInwon[14].To<string>();

                nOneInwon[15] = SS1.ActiveSheet.Cells[32, 4].Text.Replace(",", "").To<long>() * nOneInwon[15];
                SS1.ActiveSheet.Cells[32, 6].Text = nOneInwon[15].To<string>();

                nOneInwon[16] = SS1.ActiveSheet.Cells[33, 4].Text.Replace(",", "").To<long>() * nOneInwon[16];
                SS1.ActiveSheet.Cells[33, 6].Text = nOneInwon[16].To<string>();

                nOneInwon[17] = SS1.ActiveSheet.Cells[34, 4].Text.Replace(",", "").To<long>() * nOneInwon[17];
                SS1.ActiveSheet.Cells[33, 6].Text = nOneInwon[17].To<string>();

                nOneInwon[18] = SS1.ActiveSheet.Cells[35, 4].Text.Replace(",", "").To<long>() * nOneInwon[18];
                SS1.ActiveSheet.Cells[35, 6].Text = nOneInwon[18].To<string>();

                nOneInwon[19] = SS1.ActiveSheet.Cells[36, 4].Text.Replace(",", "").To<long>() * nOneInwon[19];
                SS1.ActiveSheet.Cells[36, 6].Text = nOneInwon[19].To<string>();

                nOneInwon[20] = SS1.ActiveSheet.Cells[37, 4].Text.Replace(",", "").To<long>() * nOneInwon[20];
                SS1.ActiveSheet.Cells[37, 6].Text = nOneInwon[20].To<string>();

                nOneInwon[21] = SS1.ActiveSheet.Cells[38, 4].Text.Replace(",", "").To<long>() * nOneInwon[21];
                SS1.ActiveSheet.Cells[38, 6].Text = nOneInwon[21].To<string>();

                //중간계산
                for (int j = 10; j <= 37; j++)
                {
                    nTot1 += SS1.ActiveSheet.Cells[j, 6].Text.To<long>();
                    if (j <= 23)
                    {
                        nTot2 += SS1.ActiveSheet.Cells[j, 11].Text.To<long>();
                    }
                }
                SS1.ActiveSheet.Cells[38, 6].Text = string.Format("{0:###,###,###,##0}", (nTot1 + nTot2)) + " )";
                SS1.ActiveSheet.Cells[38, 11].Text = string.Format("{0:###,###,###,##0}", (nTot1 + nTot2)) + " )";
                SS1.ActiveSheet.Cells[38, 9].Text = string.Format("{0:###,###,###,##0}", (nTot1 + nTot2)) + " )";

                //, 를 표시
                for (int i = 10; i <= 37; i++)
                {
                    SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:###,###,###,##0}", SS1.ActiveSheet.Cells[i, 6].Text);
                    if (i <= 23)
                    {
                        nTot2 += SS1.ActiveSheet.Cells[i, 11].Text.To<long>();
                    }
                }

                //총금액 업데이트
                fn_Amt_Gesan();
                btnAmt.Enabled = true;
                MessageBox.Show("검진비용금액을 자동으로 저장하였습니다.." + "\r\n" + "\r\n" + "금액이 틀릴경우 반드시 금액저장을 하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void fn_Spread_Clear()
        {
            SS1.ActiveSheet.Cells[2, 1].Text = "";  //출력일자
            SS1.ActiveSheet.Cells[6, 7].Text = "";  //사업장 명칭
            SS1.ActiveSheet.Cells[6, 10].Text = ""; //기호                                                    
            SS1.ActiveSheet.Cells[7, 9].Text = "";  //총청구금액

            //검사항목 및 검사료
            for (int i = 10; i <= 35; i++)
            {
                SS1.ActiveSheet.Cells[i, 5].Text = "";  //1차실시인원
                SS1.ActiveSheet.Cells[i, 6].Text = "";  //1차청구금액
                SS1.ActiveSheet.Cells[i, 10].Text = ""; //2차실시인원
                SS1.ActiveSheet.Cells[i, 11].Text = ""; //2차청구금액
            }
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

        void fn_Amt_Gesan()
        {
            long nTotAmt = 0;
            long nOneAmt = 0;
            long nTwoAmt = 0;
            long nAmt = 0;
            int result = 0;

            nTotAmt = 0;
            nOneAmt = 0;
            nTwoAmt = 0;
            nAmt = 0;

            HIC_MIR_BOHUM list = hicMirBohumService.GetTamtbyMirNo(txtMirno.Text);
            if (!list.IsNullOrEmpty())
            {
                nTotAmt = SS1.ActiveSheet.Cells[7, 9].Text.Replace(",", "").To<long>();
                nOneAmt = SS1.ActiveSheet.Cells[38, 6].Text.Replace(",", "").To<long>();
                nTwoAmt = SS1.ActiveSheet.Cells[38, 12].Text.Replace(",", "").To<long>();

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicMirBohumService.UpdateTamtOntTamtTowTamtbyMirNo(nTotAmt, nOneAmt, nTwoAmt, txtMirno.Text.Trim());

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("HIC_MIR_BOHUM UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
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
