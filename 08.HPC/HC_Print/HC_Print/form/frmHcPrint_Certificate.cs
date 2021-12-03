using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_Certificate : Form
    {
        clsHaBase cHB = null;
        clsSpread sp = new clsSpread();
        HicJepsuLtdService hicJepsuLtdService = null;
        BasBcodeService basBcodeService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultService hicResultService = null;

        string strFDate = "";
        string strTDate = "";
        long nWrtno = 0;


        public frmHcPrint_Certificate()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {

            hicJepsuLtdService = new HicJepsuLtdService();
            basBcodeService = new BasBcodeService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            SSList.AddColumn("접수번호", nameof(HIC_JEPSU_LTD.WRTNO), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("접수일자", nameof(HIC_JEPSU_LTD.JEPDATE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("이름", nameof(HIC_JEPSU_LTD.SNAME), 100, FpSpreadCellType.TextCellType);
            SSList.AddColumn("성별/나이", nameof(HIC_JEPSU_LTD.SEX), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("검진종류", nameof(HIC_JEPSU_LTD.GJJONG), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("업체명", nameof(HIC_JEPSU_LTD.LTDCODE), 88, FpSpreadCellType.TextCellType);
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnInput.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }


        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //이름표시
            if (sender == txtWrtno)
            {
                if (e.KeyCode != Keys.Enter) { return; }

                //long a = Convert.ToInt32(VB.Val(txtWrtno.Text));
                //lblSname.Text = HicJepsuService.Read_SName(a);
            }
        }


        void eFormLoad(object sender, EventArgs e)
        {

            cHB = new clsHaBase();

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            CLEAR_SCREEN();
            CLEAR_SCREEN_SS1();

            cboDR.Items.Clear();

            List<BAS_BCODE> list = basBcodeService.GetItembyGubun("HIC_진료과장_영문");
            cboDR.SetItems(list, "NAME", "NAME", "", "", AddComboBoxPosition.Top,false);
            cboDR.SelectedIndex = 0;

        }
        void CLEAR_SCREEN()
        {
            lblSname.Text = "";
            txtWrtno.Text = "";

            lblpNames.Text = "";

            txtENAME.Text = "";
            TxtAddress.Text = "";
            TxtDisease.Text = "";
            TxtSummary.Text = "";
        }

        void CLEAR_SCREEN_SS1()
        {

            SS1.ActiveSheet.Cells[4, 2].Text = "";           //Name
            SS1.ActiveSheet.Cells[4, 7].Text = "";           //Date of Examination

            SS1.ActiveSheet.Cells[5, 2].Text = "";           //Date Of Birth
            SS1.ActiveSheet.Cells[5, 6].Text = "";           //Age
            SS1.ActiveSheet.Cells[5, 8].Text = "";           //Sex

            SS1.ActiveSheet.Cells[6, 2].Text = "";           //Address

            SS1.ActiveSheet.Cells[9, 2].Text = "";           //Height
            SS1.ActiveSheet.Cells[9, 6].Text = "";           //Weight

            SS1.ActiveSheet.Cells[10, 5].Text = "";          //Blood Press(Systolic)
            SS1.ActiveSheet.Cells[11, 5].Text = "";          //- -Diastolic
            SS1.ActiveSheet.Cells[12, 5].Text = "";          //pulse sitting

            SS1.ActiveSheet.Cells[14, 5].Text = "";          //vision - left
            SS1.ActiveSheet.Cells[14, 7].Text = "";          //vision - right

            SS1.ActiveSheet.Cells[15, 5].Text = "";          //vision - left
            SS1.ActiveSheet.Cells[15, 7].Text = "";          //vision - right

            SS1.ActiveSheet.Cells[16, 3].Text = "";          //color vision

            SS1.ActiveSheet.Cells[18, 5].Text = "";          //hearing - 1000hz
            SS1.ActiveSheet.Cells[18, 7].Text = "";          //hearing - 4000hz

            SS1.ActiveSheet.Cells[19, 5].Text = "";          //hearing - 1000hz
            SS1.ActiveSheet.Cells[19, 7].Text = "";          //hearing - 4000hz

            SS1.ActiveSheet.Cells[20, 3].Text = "";          //Disease Other

            SS1.ActiveSheet.Cells[22, 3].Text = "";          //Film Number
            SS1.ActiveSheet.Cells[22, 7].Text = "";          //Date

            SS1.ActiveSheet.Cells[24, 3].Text = "";          //Chest
            SS1.ActiveSheet.Cells[24, 6].Text = "";          //CT-Spine

            //Laboratory Findings
            SS1.ActiveSheet.Cells[27, 3].Text = "";
            SS1.ActiveSheet.Cells[27, 4].Text = "";
            SS1.ActiveSheet.Cells[27, 5].Text = "";
            SS1.ActiveSheet.Cells[27, 6].Text = "";
            SS1.ActiveSheet.Cells[27, 7].Text = "";
            SS1.ActiveSheet.Cells[27, 8].Text = "";

            SS1.ActiveSheet.Cells[29, 3].Text = "";
            SS1.ActiveSheet.Cells[29, 4].Text = "";
            SS1.ActiveSheet.Cells[29, 5].Text = "";
            SS1.ActiveSheet.Cells[29, 6].Text = "";
            SS1.ActiveSheet.Cells[29, 7].Text = "";
            SS1.ActiveSheet.Cells[29, 8].Text = "";

            SS1.ActiveSheet.Cells[31, 3].Text = "";
            SS1.ActiveSheet.Cells[31, 4].Text = "";
            SS1.ActiveSheet.Cells[31, 5].Text = "";
            SS1.ActiveSheet.Cells[31, 6].Text = "";
            SS1.ActiveSheet.Cells[31, 7].Text = "";
            SS1.ActiveSheet.Cells[31, 8].Text = "";

            //Summary of the Examinning Physician
            SS1.ActiveSheet.Cells[34, 1].Text = "";


        }

        void READ_SS1(long nWrtno)
        {

            long nRow = 0;

            string strJUMIN = "";
            string strEXCODE = "";
            string strRESULT = "";

            string[] strTemp = new string[34];

            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(nWrtno);

            SS1.ActiveSheet.Cells[4, 2].Text = txtENAME.Text;
            SS1.ActiveSheet.Cells[4, 7].Text = list.JEPDATE;

            strJUMIN = VB.Mid(list.JUMIN, 7, 1);
            switch (strJUMIN)
            {
                case "1":
                case "2":
                case "5":
                case "6":
                    strJUMIN = "19";
                    break;
                case "3":
                case "4":
                case "7":
                case "8":
                    strJUMIN = "20";
                    break;
                default:
                    strJUMIN = "18";
                    break;
            }

            strJUMIN = strJUMIN + VB.Mid(list.JUMIN, 1, 2) + "." + VB.Mid(list.JUMIN, 3, 2) + "." + VB.Mid(list.JUMIN, 5, 2);

            SS1.ActiveSheet.Cells[5, 2].Text = strJUMIN;
            SS1.ActiveSheet.Cells[5, 6].Text = Convert.ToString(list.AGE);
            SS1.ActiveSheet.Cells[5, 8].Text = list.SEX;
            SS1.ActiveSheet.Cells[6, 2].Text = TxtAddress.Text;

            List<HIC_RESULT> list2 = hicResultService.Get_Results(nWrtno);

            nRow = list2.Count;

            for (int i = 0; i < nRow; i++)
            {
                if (VB.Trim(list2[i].RESULT) == null || VB.Trim(list2[i].RESULT) == "")
                {
                    strRESULT = "-";
                }
                else
                {
                    strRESULT = list2[i].RESULT;
                }

                if (list2[i].RESULT == "정상")
                {
                    strRESULT = "Normal";
                }
                if (list2[i].RESULT == "음성")
                {
                    strRESULT = "Negative";
                }
                if (list2[i].RESULT == "양성")
                {
                    strRESULT = "Positive";
                }

                strEXCODE = list2[i].EXCODE;
                switch (strEXCODE)
                {
                    case "A101":
                        strTemp[1] = list2[i].RESULT;
                        break;
                    case "A102":
                        strTemp[2] = strRESULT;
                        break;
                    case "A108":
                        strTemp[3] = strRESULT;
                        break;
                    case "A109":
                        strTemp[4] = strRESULT;
                        break;
                    case "TA07":
                        strTemp[5] = strRESULT;
                        break;
                    case "A104":
                        strTemp[6] = strRESULT;
                        break;
                    case "A105":
                        strTemp[7] = strRESULT;
                        break;
                    case "A301":
                        strTemp[8] = strRESULT;
                        break;
                    case "A302":
                        strTemp[9] = strRESULT;
                        break;
                    case "TE05":
                        strTemp[10] = strRESULT;
                        break;
                    case "A106":
                        strTemp[11] = strRESULT;
                        break;
                    case "A107":
                        strTemp[12] = strRESULT;
                        break;
                    case "TH15":
                        strTemp[13] = strRESULT;
                        break;
                    case "TH25":
                        strTemp[14] = strRESULT;
                        break;
                    case "A215":
                        strTemp[15] = strRESULT;
                        break;
                    case "A142":
                        strTemp[16] = strRESULT;
                        break;

                    case "A111":
                        strTemp[17] = strRESULT;
                        break;
                    case "A112":
                        strTemp[18] = strRESULT;
                        break;
                    case "A113":
                        strTemp[19] = strRESULT;
                        break;
                    case "A114":
                        strTemp[20] = strRESULT;
                        break;
                    case "A123":
                        strTemp[21] = strRESULT;
                        break;
                    case "A281":
                        strTemp[22] = strRESULT;
                        break;
                    case "A121":
                        strTemp[23] = strRESULT;
                        break;
                    case "A124":
                        strTemp[24] = strRESULT;
                        break;
                    case "A125":
                        strTemp[25] = strRESULT;
                        break;
                    case "H840":
                        strTemp[26] = strRESULT;
                        break;
                    case "H841":
                        strTemp[26] = strRESULT + "(" + strRESULT + ")";
                        break;
                    case "A131":
                        strTemp[27] = strRESULT;
                        break;
                    case "A132":
                        strTemp[28] = strRESULT;
                        break;
                    case "E921":
                        strTemp[29] = strRESULT;
                        break;
                    case "LU39":
                        strTemp[30] = strRESULT;
                        break;

                }
                if (strTemp[i] == "")
                {
                    strTemp[i] = "-";
                }

            }

            strTemp[31] = " " + VB.Trim(TxtDisease.Text);
            strTemp[32] = " " + VB.Trim(TxtSummary.Text);

            for (int i = 0; i < 31; i++)
            {
                if (strTemp[i] == "" || strTemp[i] == null)
                {
                    strTemp[i] = "-";
                }
            }

            SS1.ActiveSheet.Cells[9, 2].Text = strTemp[1] + " Cm";
            SS1.ActiveSheet.Cells[9, 6].Text = strTemp[2] + " Kg";

            SS1.ActiveSheet.Cells[10, 5].Text = strTemp[3] + " mmHg";
            SS1.ActiveSheet.Cells[11, 5].Text = strTemp[4] + " mmHg";
            SS1.ActiveSheet.Cells[12, 5].Text = strTemp[5];

            SS1.ActiveSheet.Cells[14, 5].Text = strTemp[6];
            SS1.ActiveSheet.Cells[14, 7].Text = strTemp[7];

            SS1.ActiveSheet.Cells[15, 5].Text = strTemp[8];
            SS1.ActiveSheet.Cells[15, 7].Text = strTemp[9];

            SS1.ActiveSheet.Cells[16, 3].Text = strTemp[10];                                //color vision

            SS1.ActiveSheet.Cells[18, 5].Text = strTemp[11];
            SS1.ActiveSheet.Cells[18, 7].Text = strTemp[12];

            SS1.ActiveSheet.Cells[19, 5].Text = strTemp[13];
            SS1.ActiveSheet.Cells[19, 7].Text = strTemp[14];

            SS1.ActiveSheet.Cells[20, 3].Text = strTemp[31];                            //Disease Other

            SS1.ActiveSheet.Cells[22, 3].Text = strTemp[15];
            SS1.ActiveSheet.Cells[22, 7].Text = "";

            SS1.ActiveSheet.Cells[24, 3].Text = strTemp[16];                             //CHEST
            SS1.ActiveSheet.Cells[24, 6].Text = "";                             //CT_SPINE

            SS1.ActiveSheet.Cells[27, 3].Text = strTemp[17];
            SS1.ActiveSheet.Cells[27, 4].Text = strTemp[18];
            SS1.ActiveSheet.Cells[27, 5].Text = strTemp[19];
            SS1.ActiveSheet.Cells[27, 6].Text = strTemp[20];
            SS1.ActiveSheet.Cells[27, 7].Text = "";
            SS1.ActiveSheet.Cells[27, 8].Text = "";

            SS1.ActiveSheet.Cells[29, 3].Text = strTemp[21];
            SS1.ActiveSheet.Cells[29, 4].Text = strTemp[22];
            SS1.ActiveSheet.Cells[29, 5].Text = strTemp[23];
            SS1.ActiveSheet.Cells[29, 6].Text = strTemp[24];
            SS1.ActiveSheet.Cells[29, 7].Text = strTemp[25];
            SS1.ActiveSheet.Cells[29, 8].Text = strTemp[26];

            SS1.ActiveSheet.Cells[30, 3].Text = "HBsAg";
            SS1.ActiveSheet.Cells[30, 4].Text = "HBsAb";
            SS1.ActiveSheet.Cells[30, 5].Text = "HIV(AIDS)";
            SS1.ActiveSheet.Cells[30, 6].Text = "VDRL";
            SS1.ActiveSheet.Cells[30, 7].Text = "-";
            SS1.ActiveSheet.Cells[30, 8].Text = "-";

            SS1.ActiveSheet.Cells[31, 3].Text = strTemp[27];
            SS1.ActiveSheet.Cells[31, 4].Text = strTemp[28];
            SS1.ActiveSheet.Cells[31, 5].Text = strTemp[29];
            SS1.ActiveSheet.Cells[31, 6].Text = strTemp[30];
            SS1.ActiveSheet.Cells[31, 7].Text = "";
            SS1.ActiveSheet.Cells[31, 8].Text = "";

            SS1.ActiveSheet.Cells[34, 1].Text = strTemp[32];

            SS1.ActiveSheet.Cells[35, 1].Text = "Signature :             " + VB.Trim(cboDR.Text) + "               ";
        }

        string Verity_Value( )
        {
            string strOK = "OK";

            if (txtENAME.Text == "")
            {
                MessageBox.Show("영문 이름을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                strOK = "";
                return strOK;
            }

            if (TxtAddress.Text == "")
            {
                MessageBox.Show("영문 주소를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                strOK = "";
                return strOK; 
            }

            if (TxtSummary.Text == "")
            {
                MessageBox.Show("Summary of the Examnning physician이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                strOK = "";
                return strOK;
            }

            return strOK;

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnPrint)
            {
                //string strTitle = "";
                //string strHeader = "";
                //string strFooter = "";
                //bool PrePrint = true;

                //ComFunc.ReadSysDate(clsDB.DbCon);

                //clsSpread.SpdPrint_Margin setMargin;
                //clsSpread.SpdPrint_Option setOption;

                //strTitle = "청구오류명단";
                //strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                //sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnInput)
            {

                string Chk = "";

                Chk =  Verity_Value();

                if (Chk == "OK")
                {

                    READ_SS1(nWrtno);

                    string strTitle = "";
                    string strHeader = "";
                    string strFooter = "";
                    bool PrePrint = true;

                    ComFunc.ReadSysDate(clsDB.DbCon);

                    clsSpread.SpdPrint_Margin setMargin;
                    clsSpread.SpdPrint_Option setOption;

                    strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                    setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }

            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }
        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";


            nWrtno = 0;
            CLEAR_SCREEN();
            CLEAR_SCREEN_SS1();

            nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[e.Row, 0].Text);
            strTemp1 = SSList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            strTemp2 = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

            lblpNames.Text = "접수번호: " + nWrtno + " " + strTemp1 + " " + strTemp2;

            READ_SS1(nWrtno);
        }

        private void Screen_Display(FpSpread Spd)
        {

            ComFunc CF = null;
            CF = new ComFunc();

            int nRow = 0;

            strFDate = dtpFDate.Value.ToShortDateString();
            strTDate = dtpTDate.Value.ToShortDateString();

            List<HIC_JEPSU_LTD> list = hicJepsuLtdService.GetListByItems(strFDate, strTDate); 

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList.ActiveSheet.RowCount = nRow;

            for (int i = 0; i < nRow; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE.ToString();
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].SEX;
                SSList.ActiveSheet.Cells[i, 4].Text = list[i].GJJONG;
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].SANGHO;

            }


        }
    }
}
