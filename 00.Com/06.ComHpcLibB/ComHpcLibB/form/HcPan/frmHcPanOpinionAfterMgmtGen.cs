using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanOpinionAfterMgmtGen.cs
/// Description     : 일반-질병유소견자 사후관리(생애포함)
/// Author          : 이상훈
/// Create Date     : 2019-10-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm일반사후관리_2010_NEW.frm(Frm질병유소견자사후관리(일반)_2010_NEW)" />

namespace ComHpcLibB
{
    public partial class frmHcPanOpinionAfterMgmtGen : Form
    {
        HicResultExCodeService hicResultExCodeService = null;
        HicCodeService hicCodeService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicJepsuGundateService hicJepsuGundateService = null;
        HicResultService hicResultService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType ht = new clsHcType();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        FarPoint.Win.Spread.CellType.ImageCellType Img = new FarPoint.Win.Spread.CellType.ImageCellType();

        string FstrLtdCode = "";
        string FstrFDate = "";
        string FstrTDate = "";

        long FnWRTNO = 0;
        long FnWrtno2 = 0;
        long FnPano = 0;
        int FnRow = 0;
        int FnDoctCnt = 0;
        long[] FnPanDrNo = new long[10];
        string FstrChk = "";
        string Fstr생애구분 = "";
        string FstrNameFlag = "";
        string Fstr생애Flag = "";

        string FSTRWRTNO = "";

        string strSname = "";
        string strSex = "";
        long nAge = 0;
        string strIpsadate = "";
        string strJepDate = "";
        int nGunsokYY = 0;
        int nGunsokMM = 0;
        string strSogen = "";
        string strPanName = "";
        string strGbPanBun2 = "";
        string strResult = "";
        string strOK = "";
        string strPanjeng = "";
        string strGongjeng = "";

        string[] strPanjengC = new string[5];
        string[] strPanjengD1 = new string[3];
        string[] strPanjengD2 = new string[3];

        string[] strPanjengU = new string[4];
        string strFlag = "";                    //생애한줄 작업
        string strD2Flag = "";                  //D2 대상자

        public frmHcPanOpinionAfterMgmtGen()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResultExCodeService = new HicResultExCodeService();
            hicCodeService = new HicCodeService();
            hicJepsuService = new HicJepsuService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicJepsuGundateService = new HicJepsuGundateService();
            hicResultService = new HicResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboLostFocus);
        }

        void eCboLostFocus(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYear = 0;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);
            nYear = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            cboYear.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYear));
                nYear -= 1;
            }
            cboYear.SelectedIndex = 0;

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

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            
            SS1_Sheet1.Columns.Get(9).Visible = false;   //의사도장

            fn_Screen_Clear();
            //vbHicDojang.SignImage_Download(); //의사도장 다운로드 => hf.SetDojangImage 로 대체
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
                int nRead = 0;
                int nRead1 = 0;
                int nRow = 0;
                string strFrDate = "";
                string strToDate = "";
                string strGjYear = "";
                string strGjBangi = "";
                string strLtdCode = "";

                int nRead1Row = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strGjYear = cboYear.Text;
                strGjBangi = cboBangi.Text;
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 50;
                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;
                ssList.Enabled = false;

                lblLtdName.Text = "";
                lblGigan.Text = "";

                List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyJepDateGjYearGjBangi(strFrDate, strToDate, strGjYear, strGjBangi, strLtdCode);
                nRead = list.Count;
                ssList.ActiveSheet.RowCount = nRead;
                //ssList_Sheet1.Rows.Get(-1).Height = 24;

                List<HIC_JEPSU_GUNDATE> list2 = hicJepsuGundateService.GetItembyGjYear(strGjYear);
                nRead1 = list2.Count;

                for (int i = 0; i < nRead; i++)
                {
                    if (list[i].SUM > 0)
                    {
                        if (list[i].NAME != null)
                        {
                            ssList.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                        }
                        ssList.ActiveSheet.Cells[i, 1].Text = string.Format("{0:N0}", list[i].SUM);
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].LTDCODE.ToString();
                        if (nRead1 == 1)
                        {
                            ssList.ActiveSheet.Cells[i, 7].Text = "Y";
                        }
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].MINDATE;
                        ssList.ActiveSheet.Cells[i, 4].Text = list[i].MAXDATE;
                        // 로직 이상함. 아래로 변경
                        //If nRead1 > 0 Then
                        //    SSList.Col = 6:  SSList.Text = Trim(AdoGetString(Rs, "MinDate1", i))
                        //    SSList.Col = 7:  SSList.Text = Trim(AdoGetString(Rs, "MaxDate1", i))
                        //End If
                        //if (nRead1> 0)
                        if (i <= nRead1 - 1)
                        {
                            //ssList.ActiveSheet.Cells[i, 5].Text = list2[nRead1Row].MINDATE1;
                            //ssList.ActiveSheet.Cells[i, 6].Text = list2[nRead1Row].MAXDATE1;
                            ssList.ActiveSheet.Cells[i, 5].Text = list2[i].MINDATE1;
                            ssList.ActiveSheet.Cells[i, 6].Text = list2[i].MAXDATE1;
                            nRead1Row += 1;
                        }
                    }
                }
                ssList.Enabled = true;
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
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "근로자 건강진단 사후관리 소견서(일반)";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("사업장명: " + lblLtdName.Text + VB.Space(20) + "건강진단실시기간: " + lblGigan.Text + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                for (int i = 0; i < int.Parse(cboPrtCnt.Text); i++) //인쇄매수
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
            }
            else if (sender == btnExcel)
            {
                string strPath = "C:\\ExcelFileDown\\";
                string strFileName = "C:\\ExcelFileDown\\근로자 건강진단 사후관리 소견서(일반)_" + lblLtdName.Text + "(" + lblGigan.Text + ")";

                hf.Excel_File_Create(strPath, strFileName, SS1, SS1_Sheet1);
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

        void eKeyPress(object sender, KeyPressEventArgs e)
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true && e.RowHeader != true)
                {
                    if (MessageBox.Show("선택한 " + e.Row + "번째 Row를 제거 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SS1_Sheet1.RemoveRows(SS1_Sheet1.ActiveRowIndex, 1);
                    }
                }
            }
            else if (sender == ssList)
            {
                string strFDate = "";
                string strTDate = "";
                string strJepDate2 = "";
                string strGjYear = "";

                strGjYear = cboYear.Text;
                
                lblLtdName.Text = " " + ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                FstrLtdCode = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                FstrTDate = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                List<HIC_JEPSU_GUNDATE> list = hicJepsuGundateService.GetItembyJepDateGjYear(FstrFDate, FstrTDate, strGjYear, FstrLtdCode);

                //ssList_Sheet1.Rows.Get(-1).Height = 24;

                if (list.Count == 0)
                {
                    HIC_RESULT list2 = hicResultService.GetJepDate(FstrFDate, FstrTDate, FstrLtdCode, strGjYear);
                    strFDate = list2.MINDATE.ToString();
                    strTDate = list2.MAXDATE.ToString();
                }
                else
                {
                    if (list.Count == 1)
                    {
                        strFDate = list[0].JEPDATE.ToString();
                        strTDate = list[0].JEPDATE.ToString();
                    }
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].GUNDATE != null)
                            {
                                strJepDate2 = list[i].GUNDATE.ToString();
                                if (strFDate == "") strFDate = strJepDate2;
                                if (string.Compare(strFDate, strJepDate2) > 0) strFDate = strJepDate2;
                                if (strTDate == "") strTDate = strJepDate2;
                                if (string.Compare(strTDate, strJepDate2) < 0) strTDate = strJepDate2;
                            }
                            else
                            {
                                strJepDate2 = list[i].JEPDATE;
                                if (strFDate == "") strFDate = strJepDate2;
                                if (string.Compare(strFDate, strJepDate2) > 0) strFDate = strJepDate2;
                                if (strTDate == "") strTDate = strJepDate2;
                                if (string.Compare(strTDate, strJepDate2) < 0) strTDate = strJepDate2;
                            }
                        }
                    }
                }

                lblGigan.Text = strFDate + " ~ " + strTDate;

                fn_Screen_Display();
            }
        }

        string READ_Exam_Result(string argGbn)
        {
            string rtnVal="";
            string strRETURN = "";
            int nREAD = 0;

            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoGbn(FnWRTNO, argGbn);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list[i].YNAME;
                if (strExName.IsNullOrEmpty())
                {
                    strExName = list[i].HNAME;
                }
                if (strExName == "HDL콜레스테롤")
                {
                    strExName = "HDL";
                }

                if (strExName == "LDL-콜레스테롤")
                {
                    strExName = "LDL";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                {
                    strResult = "";
                }

                if (!strResult.IsNullOrEmpty())
                {
                    strRETURN += strExName + ":" + strResult + ","; 
                }
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoGbn(FnWrtno2, argGbn);

                nREAD = list2.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list2[i].EXCODE;                 //검사코드
                    strResult = list2[i].RESULT;                 //검사실 결과값
                    strResCode = list2[i].RESCODE;               //결과값 코드
                    strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                    strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                    strExName = list2[i].YNAME;
                    if (strExName == "")
                    {
                        strExName = list2[i].HNAME;
                    }
                    if (strExName == "HDL콜레스테롤")
                    {
                        strExName = "HDL";
                    }

                    if (strExName == "LDL-콜레스테롤")
                    {
                        strExName = "LDL";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                    {
                        strResult = "";
                    }
                    //식전공복혈당 2차
                    if (strExCode == "A148")
                    {
                        strExName = "2차:" + strExName;
                    }

                    if (!strResult.IsNullOrEmpty())
                    {
                        strRETURN += strExName + ":" + strResult + ",";
                    }
                }
            }

            //마지막 컴마을 제거
            if (!strRETURN.IsNullOrEmpty())
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            rtnVal = strRETURN;
            return rtnVal;
        }

        string READ_D1D2_Result(string argGbn, string argCode)
        {
            string rtnVal = "";
            string strRETURN = "";
            int nRead = 0;

            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strExCodes = "";
            List<string> strNewExCode = new List<string>();

            HIC_CODE list = hicCodeService.GetGCode2byGubunCode(argGbn, argCode);

            if (list != null)
            {
                strExCodes = list.GCODE2;
            }
            else
            {
                return rtnVal;
            }

            if (strExCodes == "")
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.I(strExCodes, ","); i++)
            {
                strNewExCode.Add(VB.Pstr(strExCodes, ",", i));
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWRTNO, strNewExCode, "");

            nRead = list2.Count;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName == "")
                {
                    strExName = list2[i].HNAME;
                }
                
                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                {                    
                }
                else
                {
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWrtno2, strNewExCode, "");

                nRead = list2.Count;
                for (int i = 0; i < nRead; i++)
                {
                    strExCode = list3[i].EXCODE;                 //검사코드
                    strResult = list3[i].RESULT;                 //검사실 결과값
                    strResCode = list3[i].RESCODE;               //결과값 코드
                    strResultType = list3[i].RESULTTYPE;         //결과값 TYPE
                    strGbCodeUse = list3[i].GBCODEUSE;           //결과값코드 사용여부
                    strExName = list3[i].YNAME;
                    if (strExName == "")
                    {
                        strExName = list3[i].HNAME;
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                    {
                    }
                    else
                    {
                        strRETURN += strExName + ":" + strResult + ",";
                    }
                }
            }

            //마지막 컴마을 제거
            if (!strRETURN.IsNullOrEmpty())
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            //검사결과를 53자 단위로 분리함
            rtnVal = strRETURN;

            return rtnVal;
        }

        

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            lblGigan.Text = "";
            btnPrint.Enabled = false;
            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            //의사도장 복사 때문에 수동으로 클리어
            SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 8].Text = "";
            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nRead = 0;
            string strJepDate = "";
            string strBangi = "";
            string strGjYear = "";

            FSTRWRTNO = "";
            FstrChk = "";

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            Application.DoEvents();

            //의사도장 복사 때문에 수동으로 클리어
            btnPrint.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;

            //변수를 Clear
            FnRow = 0;
            FnDoctCnt = 0;
            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            for (int i = 0; i < 3; i++)
            {
                strPanjengD1[i] = "";
                strPanjengD2[i] = "";
            }

            for (int i = 0; i < 4; i++)
            {
                strPanjengU[i] = "";
            }

            FstrNameFlag = "";
            Fstr생애Flag = "";

            strGjYear = cboYear.Text;

            if (cboBangi.Text.Trim() == "상반기")
            {
                strBangi = "1";
            }
            else if (cboBangi.Text.Trim() == "하반기")
            {
                strBangi = "2";
            }
            else
            {
                strBangi = "";
            }

            //자료를 SELECT
            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeGjYear(FstrFDate, FstrTDate, FstrLtdCode, strGjYear, strBangi);

            nRead = list.Count;
            progressBar1.Maximum = nRead;
            for (int i = 0; i < nRead; i++)
            {
                FnWRTNO = list[i].WRTNO;
                FnPano = list[i].PANO;
                //strJepDate = list[i].JEPDATE.ToString();
                strJepDate = list[i].JEPDATE;
                switch (list[i].GJJONG)
                {
                    case "41":
                    case "42":
                        Fstr생애구분 = "생애";
                        break;
                    default:
                        Fstr생애구분 = "일반";
                        break;
                }
                
                FstrChk = "";
                FnWrtno2 = 0;
                //2차 접수번호를 읽음
                FnWrtno2 = hicJepsuService.GetWrtnoByPanoJepDateGjYear(FnPano, strJepDate, strGjYear, strBangi);

                //2차 검진결과를 읽음
                if (FnWrtno2 > 0)
                {
                    chb.READ_HIC_RES_BOHUM2(FnWrtno2);
                    if (!clsHcType.B2.ROWID.IsNullOrEmpty())
                    {
                        //2010-일반1,2차 있으면 1,2차 표시(단,2차 고혈압,당뇨가 있을경우 1차의 고혈압,당뇨는 제외함)
                        fn_Screen_First_Result(FnWRTNO);
                        fn_Screen_Second_Result(FnWrtno2);
                    }
                    else
                    {
                        fn_Screen_First_Result(FnWRTNO);
                    }
                }
                else
                {
                    chb.READ_HIC_RES_BOHUM2(FnWrtno2);
                    fn_Screen_First_Result(FnWRTNO);
                }
                FstrNameFlag = "";
                Fstr생애Flag = "";

                progressBar1.Value = i + 1;
            }

            if (FnRow == 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                    SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = "  ** 해 당 없 음 **";
                }
            }

            for (int i = 0; i < FnRow; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 6);
                Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 7);
                if (size.Height >= size1.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
                else
                {
                    SS1.ActiveSheet.Rows[i].Height = size1.Height;
                }
            }

            //판정의사를 Display
            FnRow += 3;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            SS1_Sheet1.Rows.Get(FnRow - 3).Border = new LineBorder(Color.Black, 1, false, true, false, false);

            SS1_Sheet1.Cells.Get(FnRow - 3, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //SS1_Sheet1.Cells.Get(FnRow - 3, 0).RowSpan = 1;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).Value = "비고";

            SS1_Sheet1.Cells.Get(FnRow - 2, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //SS1_Sheet1.Cells.Get(FnRow - 2, 0).RowSpan = 1;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).Value = "1)이 법에 해당되는 건강진단 항목만 기재";
            //SS1_Sheet1.Cells.Get(FnRow - 2, 7).ColumnSpan = 2;
            SS1_Sheet1.Cells.Get(FnRow - 2, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 2, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 7).Value = "  ▶작성일자: " + clsPublic.GstrSysDate;

            SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //SS1_Sheet1.Cells.Get(FnRow - 1, 0).RowSpan = 1;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).Value = "2)검진소견,사후관리 소견, 업무수행 적합여부는";
            //SS1_Sheet1.Cells.Get(FnRow - 1, 7).ColumnSpan = 2;
            SS1_Sheet1.Cells.Get(FnRow - 1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 7).Value = "  ▶검진기관: 대한보건환경연구소";
            for (int i = 0; i < FnDoctCnt; i++)
            {
                FnRow += 1;
                if (FnPanDrNo[i] > 0)
                {   
                    if (FnRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = FnRow;
                    }

                    if (i == 0)
                    {
                        SS1_Sheet1.Cells.Get(FnRow - 1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                        SS1_Sheet1.Cells.Get(FnRow - 1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "  ▶판정의사: " + hb.READ_License_DrName(FnPanDrNo[i]);
                        SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                        SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "(인)";
                        hf.SetDojangImage(SS1, FnRow - 1, 8, FnPanDrNo[i].ToString());
                        SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                
                        SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = "요관찰자,유소견자 등 이상 소견이 있는 검진자만 기재";
                        SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
                    }
                    else
                    {
                        SS1_Sheet1.Cells.Get(FnRow - 1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                        SS1_Sheet1.Cells.Get(FnRow - 1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "                 " + hb.READ_License_DrName(FnPanDrNo[i]);
                        SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                        SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "(인)";
                        hf.SetDojangImage(SS1, FnRow - 1, 8, FnPanDrNo[i].ToString());
                        SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                    }
                }
                SS1.ActiveSheet.RowCount = FnRow;
            }

            //SS1_Sheet1.Columns.Get(7).Width = SS1.ActiveSheet.Cells[FnRow - 1, 7].Text.Length + 20;
            Cursor.Current = Cursors.Default;
        }

        void fn_Screen_First_Result(long argWrtNo)
        {
            int nRow = 0;

            for (int i = 0; i < 3; i++)
            {
                strPanjengD1[i] = "";
            }

            for (int i = 0; i < 3; i++)
            {
                strPanjengD2[i] = "";
            }

            for (int i = 0; i < 4; i++)
            {
                strPanjengU[i] = "";
            }
            

            chb.READ_HIC_RES_BOHUM1(argWrtNo);
            if (clsHcType.B1.ROWID.IsNullOrEmpty() || clsHcType.B1.PanDrNo == 0)
            {
                return;
            }
            if (FnDoctCnt == 0)
            {
                FnDoctCnt = 1;
                FnPanDrNo[FnDoctCnt - 1] = clsHcType.B1.PanDrNo;
            }
            else
            {
                strOK = "";
                for (int i = 0; i < FnDoctCnt; i++)
                {
                    if (FnPanDrNo[i] == clsHcType.B1.PanDrNo)
                    {
                        strOK = "OK";
                    }
                }

                if (strOK != "OK")
                {
                    FnDoctCnt += 1;
                    FnPanDrNo[FnDoctCnt - 1] = clsHcType.B1.PanDrNo;
                }
            }

            strOK = "";
            strD2Flag = "";

            //정상인경우 표시함
            if (clsHcType.B1.Panjeng == "1" || clsHcType.B1.Panjeng == "2")
            {
                strOK = "OK";
            }

            //질환의심(R)이 있으면 표시함
            for (int i = 0; i < 12; i++)
            {
                if (clsHcType.B1.PanjengR[i] == "1")
                {
                    strOK = "OK";
                    break;
                }
            }

            //직업병(D1,D2)이 있으면 표시함
            if (!clsHcType.B1.PANJENGD11.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[0] = clsHcType.B1.PANJENGD11; }
            if (!clsHcType.B1.PANJENGD12.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[1] = clsHcType.B1.PANJENGD12; }
            if (!clsHcType.B1.PANJENGD13.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[2] = clsHcType.B1.PANJENGD13; }
            if (!clsHcType.B1.PANJENGD21.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[0] = clsHcType.B1.PANJENGD21; strD2Flag = "OK"; }
            if (!clsHcType.B1.PANJENGD22.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[1] = clsHcType.B1.PANJENGD22; strD2Flag = "OK"; }
            if (!clsHcType.B1.PANJENGD23.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[2] = clsHcType.B1.PANJENGD23; strD2Flag = "OK"; }

            //유질환(D)이 있으면 표시함
            if (clsHcType.B1.PANJENGU1 == "1") { strOK = "OK"; strPanjengU[0] = clsHcType.B1.PANJENGU1; }
            if (clsHcType.B1.PANJENGU2 == "1") { strOK = "OK"; strPanjengU[1] = clsHcType.B1.PANJENGU2; }
            if (clsHcType.B1.PANJENGU3 == "1") { strOK = "OK"; strPanjengU[2] = clsHcType.B1.PANJENGU3; }
            if (clsHcType.B1.PANJENGU4 == "1") { strOK = "OK"; strPanjengU[3] = clsHcType.B1.PANJENGU4; }

            //2010 생애일경우 2차 안했으면 strok ="ok" 달아줌
            if (Fstr생애구분 == "생애" && clsHcType.B2.ROWID.IsNullOrEmpty() == true && strOK == "")
            {
                strOK = "OK";
            }

            if (strOK != "OK")
            {
                return;
            }

            //인적사항을 읽음
            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            strSname = list.SNAME;
            strSex = list.SEX;
            if (strSex == "M")
            {
                strSex = "남";
            }
            else
            {
                strSex = "여";
            }

            strGongjeng = list.BUSENAME;
            strIpsadate = list.IPSADATE;
            strJepDate = list.JEPDATE;

            nGunsokYY = int.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
            nGunsokMM = int.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
            nAge = list.AGE;

            //2010 생애부분추가
            //생애1차만 했을경우
            strFlag = "";
            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && clsHcType.B2.ROWID.IsNullOrEmpty() == true && Fstr생애구분 == "생애")
            {
                strPanName = "2차생애미수검";
                strResult = "2차생애상담필요";
                strFlag = "OK";
                fn_Screen_First_Sub(strPanName, strResult);
                strFlag = "";
            }

            //정상소견
            if (clsHcType.B1.Panjeng == "1" || clsHcType.B1.Panjeng == "2")
            {
                strPanName = "정상";
                strResult = "";
                strPanjeng = "1";
                fn_Screen_First_Sub(strPanName, strResult);
            }

            //D2 일반질병 판정이 있을경우 동일한 질환으로 질환의심R1 판정이 있어도 D2 질환만 표시함
            //질환의심(R1,R2)
            for (int i = 0; i < 12; i++)
            {
                if (clsHcType.B1.PanjengR[i] == "1")
                {
                    if (!strD2Flag.IsNullOrEmpty())
                    {
                        if (fn_Read_GenPanjengD2(i + 1) == "OK")
                        {
                            clsHcType.B1.PanjengR[i] = "";
                        }
                    }

                    if (clsHcType.B1.PanjengR[i] == "1")
                    {
                        switch (i + 1)
                        {
                            case 1:
                                strPanName = "폐결핵의심";
                                strPanjeng = "4";
                                break;
                            case 2:
                                strPanName = "기타흉부질환의심";
                                strPanjeng = "4";
                                break;
                            case 3:
                                strPanName = "고혈압";
                                strPanjeng = "5";
                                break;
                            case 4:
                                strPanName = "이상지질혈증의심";
                                strPanjeng = "4";
                                break;
                            case 5:
                                strPanName = "간장질환의심";
                                strPanjeng = "4";
                                break;
                            case 6:
                                strPanName = "당뇨병";
                                strPanjeng = "5";
                                break;
                            case 7:
                                strPanName = "신장질환의심";
                                strPanjeng = "4";
                                break;
                            case 8:
                                strPanName = "빈혈증의심";
                                strPanjeng = "4";
                                break;
                            case 9:
                                strPanName = "골다공증의심";
                                strPanjeng = "4";
                                break;
                            case 10:
                                strPanName = "기타질환의심";
                                strPanjeng = "4";
                                break;
                            case 11:
                                strPanName = "비만";
                                strPanjeng = "4";
                                break;
                            case 12:
                                strPanName = "난청";
                                strPanjeng = "4";
                                break;
                            default:
                                break;
                        }

                        strGbPanBun2 = string.Format("{0:00}", i + 1);
                        strResult = "";
                        strResult += READ_Exam_Result(strGbPanBun2);

                        fn_Screen_First_Sub(strPanName, strResult);
                    }
                }
            }

            //직업병(D1)
            for (int i = 0; i < 3; i++)
            {
                if (!strPanjengD1[i].IsNullOrEmpty())
                {
                    strPanName = fn_Read_GenPanjengD1D2("31", strPanjengD1[i]);
                    strResult = "";
                    strResult += fn_Read_D1D2_Result("31", strPanjengD1[i]);
                    if (!strPanName.IsNullOrEmpty())
                    {
                        strPanjeng = "6";
                    }
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }

            //일반질환(D2)
            strResult = "";
            for (int i = 0; i < 3; i++)
            {
                if (!strPanjengD2[i].IsNullOrEmpty())
                {
                    strPanName = fn_Read_GenPanjengD1D2("33", strPanjengD2[i]);
                    strResult = "";
                    strResult += fn_Read_D1D2_Result("33", strPanjengD2[i]);
                    if (!strPanName.IsNullOrEmpty())
                    {
                        strPanjeng = "7";
                    }
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }

            //유질환(D)
            for (int i = 0; i < 4; i++)
            {
                if (strPanjengU[i] == "1")
                {
                    switch (i)
                    {
                        case 0:
                            strPanName = "유질환(고혈압)";
                            strPanjeng = "8";
                            break;
                        case 1:
                            strPanName = "유질환(당뇨병)";
                            strPanjeng = "8";
                            break;
                        case 2:
                            strPanName = "유질환(이상지질혈증)";
                            strPanjeng = "8";
                            break;
                        case 3:
                            strPanName = "유질환(폐결핵)";
                            strPanjeng = "8";
                            break;
                        default:
                            break;
                    }

                    if (i == 0)
                    {
                        strGbPanBun2 = string.Format("{0:00}", 3);
                        strResult = "";
                        strResult += READ_Exam_Result(strGbPanBun2);
                    }
                    else if (i == 1)
                    {
                        strGbPanBun2 = string.Format("{0:00}", 6);
                        strResult = "";
                        strResult += READ_Exam_Result(strGbPanBun2);
                    }
                    else if (i == 2)
                    {
                        strGbPanBun2 = string.Format("{0:00}", 4);
                        strResult = "";
                        strResult += READ_Exam_Result(strGbPanBun2);
                    }
                    else if (i == 3)
                    {
                        strGbPanBun2 = string.Format("{0:00}", 1);
                        strResult = "";
                        strResult += READ_Exam_Result(strGbPanBun2);
                    }
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }

        }

        void fn_Screen_Second_Result(long nWrtNo)
        {
            if (clsHcType.B2.PanjengDrNo == 0)
            {
                return;
            }

            //판정의사 명단 등록
            if (FnDoctCnt == 0)
            {
                FnDoctCnt = 1;
                FnPanDrNo[FnDoctCnt - 1] = clsHcType.B2.PanjengDrNo;
            }
            else
            {
                strOK = "";
                for (int i = 0; i < FnDoctCnt; i++)
                {
                    if (FnPanDrNo[i] == clsHcType.B2.PanjengDrNo)
                    {
                        strOK = "OK";
                        return;
                    }
                }
                if (strOK == "OK")
                {
                    FnDoctCnt += 1;
                    FnPanDrNo[FnDoctCnt - 1] = clsHcType.B2.PanjengDrNo;
                }
            }

            //판정결과가 건강주의,유질환자만 표시함

            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            strSname = list.SNAME;
            strSex = list.SEX;
            if (strSex == "M")
            {
                strSex = "남";
            }
            else
            {
                strSex = "여";
            }

            strGongjeng = list.BUSENAME;
            strIpsadate = list.IPSADATE.ToString();
            strJepDate = list.JEPDATE.ToString();
            nGunsokYY = int.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
            nGunsokMM = int.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
            nAge = list.AGE;

            //2010 생애부분추가
            //생애2차했을경우
            strFlag = "";
            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && !clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애")
            {
                strPanName = "2차생애수검";
                strResult = "";
                strFlag = "OK";
                fn_Screen_Second_Sub();
                strFlag = "";
            }

            if (clsHcType.B2.Cycle_RES == "1" || clsHcType.B2.Cycle_RES == "2" || clsHcType.B2.Cycle_RES == "3")
            {
                if (clsHcType.B2.Cycle_RES == "1") strPanName = "고혈압-정상";
                if (clsHcType.B2.Cycle_RES == "2") strPanName = "고혈압전단계";
                if (clsHcType.B2.Cycle_RES == "3") strPanName = "고혈압";
                strPanjeng = clsHcType.B2.Cycle_RES;
                strGbPanBun2 = "03";
                strResult = READ_Exam_Result(strGbPanBun2);
                fn_Screen_Second_Sub();
            }

            if (clsHcType.B2.Diabetes_Res == "1" || clsHcType.B2.Diabetes_Res == "2" || clsHcType.B2.Diabetes_Res == "3")
            {
                if (clsHcType.B2.Diabetes_Res == "1") strPanName = "당뇨병-정상";
                if (clsHcType.B2.Diabetes_Res == "2") strPanName = "공복혈당장애";
                if (clsHcType.B2.Diabetes_Res == "3") strPanName = "당뇨병";
                strPanjeng = clsHcType.B2.Diabetes_Res;
                strGbPanBun2 = "06";
                strResult = READ_Exam_Result(strGbPanBun2);
                fn_Screen_Second_Sub();
            }
        }

        string fn_Read_D1D2_Result(string argGbn, string argCode)
        {
            string rtnVal = "";
            string strRETURN = "";
            int nRead = 0;

            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strExCodes = "";
            List<string> strNewExCode = new List<string>();

            HIC_CODE list = hicCodeService.GetGCode2byGubunCode(argGbn, argCode);

            if (!list.IsNullOrEmpty())
            {
                strExCodes = list.GCODE2;
            }
            else
            {
                return rtnVal;
            }

            if (strExCodes.IsNullOrEmpty())
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(strExCodes, ","); i++)
            {
                if (VB.L(strExCodes, ",") == 1)
                {
                    strNewExCode.Add(strExCodes);
                }
                else
                {
                    strNewExCode.Add(VB.Pstr(strExCodes, ",", i).Trim());
                }
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWRTNO, strNewExCode, "");

            nRead = list2.Count;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName == "") strExName = list2[i].HNAME;

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }
                if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                {
                    //strExName = strExName;
                }
                else
                {
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //2차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWrtno2, strNewExCode, "");

            nRead = list3.Count;
            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    strExCode = list3[i].EXCODE;                 //검사코드
                    strResult = list3[i].RESULT;                 //검사실 결과값
                    strResCode = list3[i].RESCODE;               //결과값 코드
                    strResultType = list3[i].RESULTTYPE;         //결과값 TYPE
                    strGbCodeUse = list3[i].GBCODEUSE;           //결과값코드 사용여부
                    strExName = list3[i].YNAME;
                    if (strExName == "") strExName = list3[i].HNAME;

                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //마지막 컴마을 제거
            if (strRETURN != "")
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        /// <summary>
        /// 내용을 Display
        /// </summary>
        void fn_Screen_First_Sub(string argPanName, string argResult)
        {
            long nSogenCnt = 0;
            long nResultCNT = 0;
            long ii = 0;
            long jj = 0;

            FstrChk = "Y";
            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            if (!strSname.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = strGongjeng.IsNullOrEmpty() ? "기타" : strGongjeng;  //부서표기
                SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
                SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
                SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
                SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
                FstrNameFlag = "OK";
                strSogen = "";
                
                if (strPanjeng == "1" || strPanjeng == "2")
                {
                    strSogen = "";
                    strSogen = "필요없음";
                }
                else
                {
                    strSogen += clsHcType.B1.Sogen;
                }

                SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = strSogen;
                SS1_Sheet1.Rows.Get(FnRow - 1).Border = new LineBorder(Color.Black, 1, false, true, false, false);
            }

            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애" && strFlag == "OK")
            {
                strSogen = "";
                strPanjeng = "X";
                strSogen = "★2차생애상담필요합니다. ";
            }

            switch (strPanjeng)
            {
                case "1":
                case "2":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "A";
                    break;
                case "3":
                case "4":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "C";
                    break;
                case "5":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "확진검사대상";
                    break;
                case "6":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "D1";
                    break;
                case "7":
                case "8":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "D2";
                    break;
                case "X":
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "생애2차상담";    //생애미수검
                    break;
                default:
                    break;
            }

            //검사결과를 53자 단위로 분리함(RowHeight 조정으로 대체)
            if (argResult == "")
            {
                argResult = argPanName;
            }
            else
            {
                argResult = argPanName + "/" + argResult;
            }
            SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = argResult;
            //SS1_Sheet1.Columns.Get(6).Width = 270;

            SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "가";
            if (string.Compare(strPanjeng, "2") > 0)
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "나";
            }

            if (strPanjeng == "X")
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "";
            }

            strSname = "";
            nSogenCnt = 0;
            strSogen = "";
            strResult = "";
            strPanName = "";
            strPanjeng = "";
        }

        void fn_Screen_Second_Sub()
        {
            long nSogenCnt = 0;
            long nResultCNT = 0;
            long ii = 0;
            long jj = 0;

            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            if (!strSname.IsNullOrEmpty() && FstrNameFlag.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = strGongjeng.IsNullOrEmpty() ? "기타" : strGongjeng;  //부서표기
                SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
                SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
                SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
                SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "";

                SS1_Sheet1.Rows.Get(FnRow - 1).Border = new LineBorder(Color.Black, 1, false, true, false, false);
            }

            strSogen = "";
            if (!clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 != "일반" && strFlag == "OK")
            {
                strSogen = "★2차생애수검 ";
                SS1.ActiveSheet.Cells[FnRow, 5].Text = "2차생애상담";
            }
            else
            {
                strSogen += clsHcType.B2.Sogen;
                if (strGbPanBun2 == "03")
                {
                    //고혈압관련
                    if (clsHcType.B2.Cycle_RES == "1") SS1.ActiveSheet.Cells[FnRow, 5].Text = "A";
                    if (clsHcType.B2.Cycle_RES == "2") SS1.ActiveSheet.Cells[FnRow, 5].Text = "C";
                    if (clsHcType.B2.Cycle_RES == "3") SS1.ActiveSheet.Cells[FnRow, 5].Text = "D2";
                }
                else
                {
                    if (clsHcType.B2.Diabetes_Res == "1") SS1.ActiveSheet.Cells[FnRow, 5].Text = "A";
                    if (clsHcType.B2.Diabetes_Res == "2") SS1.ActiveSheet.Cells[FnRow, 5].Text = "C";
                    if (clsHcType.B2.Diabetes_Res == "3") SS1.ActiveSheet.Cells[FnRow, 5].Text = "D2";
                }
            }

            SS1.ActiveSheet.Cells[FnRow, 7].Text = strSogen;

            // 검사결과를 53자 단위로 분리함
            if (strResult == "")
            {
                strResult = strPanName;
            }
            else
            {
                strResult = strPanName + "/" + strResult;
            }

            SS1.ActiveSheet.Cells[FnRow, 6].Text = strResult;
            //SS1_Sheet1.Columns.Get(7).Width = 270;

            SS1.ActiveSheet.Cells[FnRow, 8].Text = "가";
            switch (SS1.ActiveSheet.Cells[FnRow, 5].Text.Trim())
            {
                case "C":
                case "D1":
                case "D2":
                    SS1.ActiveSheet.Cells[FnRow, 8].Text = "나";
                    break;
                case "2차생애상담":
                    SS1.ActiveSheet.Cells[FnRow, 8].Text = "";
                    break;
                default:
                    break;
            }

            jj = 0;
            if (nSogenCnt > 1)
            {
                jj = nSogenCnt;
            }
            if (nResultCNT > 1 && jj < nResultCNT)
            {
                jj = nResultCNT;
            }

            strSname = "";
            nSogenCnt = 0;
            strSogen = "";
            strResult = "";
            strPanName = "";
        }

        /// <summary>
        /// READ_일반판정D2()
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD2(int ArgGbn)
        {
            string rtnval = "NO";
            string strDPan = "";

            strDPan = clsHcType.B1.PANJENGD21 + "@" + clsHcType.B1.PANJENGD22 + "@" + clsHcType.B1.PANJENGD23;

            switch (ArgGbn)
            {
                case 1:
                case 2:
                    rtnval = VB.L(strDPan, "J") > 1 ? "OK" : "NO";
                    break;
                case 4:
                    rtnval = VB.L(strDPan, "E") > 1 ? "OK" : "NO";
                    break;
                case 5:
                    rtnval = VB.L(strDPan, "K") > 1 ? "OK" : "NO";
                    break;
                case 8:
                    rtnval = VB.L(strDPan, "D") > 1 ? "OK" : "NO";
                    break;
                default:
                    break;
            }

            return rtnval;
        }

        /// <summary>
        /// READ_일반판정D1D2
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD1D2(string argGbn, string argCode)
        {
            string rtnVal = "";

            HIC_CODE list = hicCodeService.GetItembyGubunCode2(argGbn, argCode);

            if (list != null)
            {
                if (list.NAME == list.NEWNAME)
                {
                    rtnVal = list.NEWNAME;
                }
                else
                {
                    rtnVal = list.NEWNAME + "(" + list.NAME + ")";
                }
            }

            return rtnVal;
        }
    }
}
