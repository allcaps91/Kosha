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
/// File Name       : frmHcPanOpinionAfterMgmtSpc.cs
/// Description     : 특수-질병유소견자 사후관리(생애포함)
/// Author          : 이상훈
/// Create Date     : 2019-10-24
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm특수사후관리_2010_NEW.frm(Frm질병유소견자사후관리(특수)_2010_NEW)" />

namespace ComHpcLibB
{
    public partial class frmHcPanOpinionAfterMgmtSpc : Form
    {
        HicCodeService hicCodeService = null;
        HicResultExCodeService hicResultExCodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicSpcSahuService hicSpcSahuService = null;
        HicJepsuGundateService hicJepsuGundateService = null;
        HicResultService hicResultService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;

        //FrmHaSpcExam frmHaSpcExam = null; //FrmSpcExam

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType ht = new clsHcType();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcPanSpcExam FrmHcPanSpcExam = null;

        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;

        long FnWRTNO;   //1차 접수번호
        long FnWrtno2;  //2차 접수번호

        string FstrSex;

        int FnCNT1_1;   //특수1차 건수
        int FnCNT1_2;   //특수2차 건수
        int FnCNT2_1;   //일반1차 건수
        int FnCNT2_2;   //일반2차 건수

        int FnCNT3_1;   //생애1차만건수 2010
        int FnCNT3_2;   // 생애2차했는건수 2010

        long FnPano;
        string FstrSName = "";
        string FstrUCodes = "";
        string FstrJepDate = "";
        string FstrGjYear = "";
        string FstrGjBangi = "";
        string FstrGjJong = "";

        int FnRow;
        int FnDoctCnt;
        long[] FnPanDrNo = new long[10];
        int FnStartRow;

        string FstrDate1;
        string FstrDate2;

        string Fstr생애구분;

        string FstrGubun;
        string FstrSogen;
        string FstrExams;
        string FstrJochi;

        int nREAD = 0;
        long nSpcRow = 0;
        int nSpcRowStart = 0;
        int nRow = 0;
        long nResult2CNT = 0;
        long nPanjengDrno = 0;
        int nSpcResCnt = 0; //특검 유질환 물질 건수

        string strSname = "";
        string strSex = "";
        long nAge = 0;
        string strIpsadate = "";
        string strJepDate = "";
        long nGunsok = 0;
        long nGunsokYY = 0;
        long nGunsokMM = 0;
        int nRowCnt2 = 0;
        string strPanName = "";
        string strPanjeng = "";
        string strGbPanBun2 = "";
        string strSogen = "";
        string strResult = "";
        string strOK = "";
        string strGong = "";
        string strUNames = "";
        string strUNames_EDIT = "";
        string strOldData = "";
        string strNewData = "";
        string strMCode = "";
        string strResult2 = "";
        List<string> strTemp = new List<string>();
        string strTemp2 = "";

        string[] strPanjengC = new string[5];
        string[] strPanjengD1 = new string[3];
        string[] strPanjengD2 = new string[3];
        string[] strPanjengU = new string[4];

        string strList_Chamgo = "";
        string strList_Gubun = "";
        string strList_Sogen = "";
        string strList_Sahu = "";
        string strList_Upmu = "";

        public frmHcPanOpinionAfterMgmtSpc()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            hicResultExCodeService = new HicResultExCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSpcSahuService = new HicSpcSahuService();
            hicJepsuGundateService = new HicJepsuGundateService();
            hicResultService = new HicResultService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.btnMenuSet.Click += new EventHandler(eBtnClick); 
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.chkExcel.Click += new EventHandler(eCheckBoxClick);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYEAR = 0;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);
            sp.Spread_All_Clear(SS1);
            txtLtdCode.Text = "";
            cboYear.Items.Clear();
            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            //건진년도 Combo SET
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYEAR);
                nYEAR -= 1;
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
            lblLtdName.Text = "";
            fn_Screen_Clear();

            //vbHicDojang.SignImage_Download(); => hf.SetDojangImage() 로 대체
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
                int nRow = 0;

                string strFrDate = "";
                string strToDate = "";
                string strGjYear = "";
                string strBangi = "";
                string strJob = "";
                string strLtdCode = "";

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 50;

                lblLtdName.Text = "";
                txtJepDate.Text = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strGjYear = cboYear.Text;
                strBangi = cboBangi.Text;
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }
                else if (rdoJob3.Checked == true)
                {
                    strJob = "3";
                }

                if (cboBangi.Text == "상반기")
                {
                    strBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strBangi = "2";
                }

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;
                ssList.Enabled = false;

                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItemCountbyJepDateGjYearGjBangi(strFrDate, strToDate, strGjYear, strBangi, strJob, strLtdCode);

                nREAD = list.Count;
                nRow = 0;
                ssList_Sheet1.Rows.Get(-1).Height = 24;
                ssList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > ssList.ActiveSheet.RowCount)
                    {
                        ssList.ActiveSheet.RowCount = nRow;
                    }
                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].CNT.ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].LTDCODE.ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].MINDATE;
                    FstrDate1 = list[i].MINDATE;
                    ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].MAXDATE;
                    FstrDate2 = list[i].MAXDATE;

                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = comHpcLibBService.GetCountSpcPanjengbyLtdCode(FstrDate1, FstrDate2, list[i].LTDCODE, strJob, strGjYear, strBangi);
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

                if (rdoJob1.Checked == true)
                {
                    strTitle = "근로자 건강진단 사후관리 소견서(일반)";
                }
                else if (rdoJob2.Checked == true)
                {
                    strTitle = "근로자 건강진단 사후관리 소견서(배치전)";
                }
                else
                {
                    strTitle = "근로자 건강진단 사후관리 소견서(기타)";
                }
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("사업장명: " + VB.Pstr(txtLtdCode.Text, ".", 2) + VB.Space(20) + "건강진단실시기간: " + txtJepDate.Text + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
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
                string strFileName = "";

                if (rdoJob1.Checked == true)
                {
                    strFileName = "C:\\ExcelFileDown\\근로자 건강진단 사후관리 소견서(일반)_" + lblLtdName.Text + "(" + txtJepDate.Text + ")";
                }
                else if (rdoJob2.Checked == true)
                {
                    strFileName = "C:\\ExcelFileDown\\근로자 건강진단 사후관리 소견서(배치전)_" + lblLtdName.Text + "(" + txtJepDate.Text + ")";
                }
                else
                {
                    strFileName = "C:\\ExcelFileDown\\근로자 건강진단 사후관리 소견서(기타)_" + lblLtdName.Text + "(" + txtJepDate.Text + ")";
                }

                hf.Excel_File_Create(strPath, strFileName, SS1, SS1_Sheet1);
            }
            else if (sender == btnMenuSet)
            {
                FrmHcPanSpcExam = new frmHcPanSpcExam();
                FrmHcPanSpcExam.ShowDialog(this);
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

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
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
                if (e.ColumnHeader == true && e.RowHeader == false)
                {
                    if (MessageBox.Show("선택한 " + e.Row + "번째 Row를 제거 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SS1_Sheet1.RemoveRows(SS1_Sheet1.ActiveRowIndex, 1);
                    }
                }
            }
            else if (sender == ssList)
            {
                string strJepDate2 = "";
                string strFDate = "";
                string strTDate = "";
                string strGjYear = "";
                string strJob = "";
                string strGjBangi = "";

                strGjYear = cboYear.Text;

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }
                else
                {
                    strJob = "3";
                }

                if (cboBangi.Text == "상반기")
                {
                    strGjBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strGjBangi = "2";
                }

                lblLtdName.Text = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                FstrLtdCode = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                FstrTDate = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                HIC_JEPSU_GUNDATE list = hicJepsuGundateService.GetItembyWrtNo(FstrFDate, FstrTDate, strGjYear, FstrLtdCode, strJob);

                if (!list.IsNullOrEmpty())
                {
                    strFDate = list.MINDATE;
                    strTDate = list.MAXDATE;
                }

                if (strFDate == "" || strTDate == "")
                {
                    txtJepDate.Text = FstrDate1 + " ~ " + FstrDate2;
                }
                else
                {
                    txtJepDate.Text = strFDate + " ~ " + strTDate;
                }

                if (chkExcel.Checked == true)
                {
                    fn_Screen_Display_Excel();
                }
                else
                {
                    fn_Screen_Display();
                }

                //1차,2차 검진기간을 읽음
                txtJepDate.Text = VB.Left(txtJepDate.Text, 10) + " ~ " + hicJepsuService.GetMaxDatebyJepDate(strFDate, strTDate, FstrLtdCode, strGjYear, strGjBangi, strJob);

                MessageBox.Show("작업완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (chkExcel.Checked == true)
            {
                btnPrint.Enabled = false;
                btnExcel.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = true;
                btnExcel.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argJob : 일반,특수,D1D2"></param>
        /// <param name="argGbn"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string Read_Exam_Result(string argJob, string argGbn, string argCode)
        {
            string rtnVal = "";

            string strRETURN = "";
            string strRet1 = "";
            string strRet2 = "";
            int nREAD = 0;
            int nFVC = 0;
            int nFvcMeas = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strExCodes = "";
            //string strNewExCode = "";
            List<string> strNewExCode = new List<string>();
            string strJepDate = "";
            string strPtNo = "";

            if (argJob == "D1D2")
            {
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

                for (int i = 0; i < VB.L(strExCodes, ","); i++)
                {
                    strNewExCode.Add(VB.Pstr(strExCodes, ",", i));
                }
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoSogenCode(FnWRTNO, argGbn, strNewExCode, argJob);

            nREAD = list2.Count;
            strRet1 = "";
            strRet2 = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName == "") strExName = list2[i].HNAME;
                if (VB.Left(strExName, 3) == "HDL") strExName = "HDL";
                if (VB.Left(strExName, 3) == "LDL") strExName = "LDL";

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }
                if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                {
                    strResult = "";
                }

                //혈압,당뇨 2차 표시
                switch (strExCode)
                {
                    case "A108":
                    case "A122":
                        strExName = "1차:" + strExName;
                        break;
                    default:
                        break;
                }

                if (strResult == ".") strResult = "";
                if (strResult != "") strRet1 += strExName + ":" + strResult + ",";
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoSogenCode(FnWrtno2, argGbn, strNewExCode, argJob);

                nREAD = list3.Count;
                for (int i = 0; i < nREAD; i++)
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
                    if (VB.Left(strExName, 3) == "HDL")
                    {
                        strExName = "HDL";
                    }

                    if (VB.Left(strExName, 3) == "LDL")
                    {
                        strExName = "LDL";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                    {
                        strResult = "";
                    }
                    //혈압,당뇨 2차 표시
                    switch (strExCode)
                    {
                        case "A148":
                        case "A231":
                        case "ZE22":
                            strExName = "2차:" + strExName;
                            break;
                        default:
                            break;
                    }

                    if (strResult != "")
                    {
                        strRet2 += strExName + ":" + strResult + ",";
                    }
                }
            }

            if (VB.InStr(strRet1, "청력") > 0) strRet1 = "1차:" + strRet1;
            if (VB.InStr(strRet2, "청력") > 0) strRet2 = "2차:" + strRet2;
            strRETURN = strRet1 + strRet2;

            //PFT 검사결과 표시
            nFVC = 0;
            nFvcMeas = 0;

            if (argJob == "특수" && VB.Left(argGbn, 1) == "J")
            {
                List<COMHPC> list4 = comHpcLibBService.GetHicResPftbyWrtNo(FnWRTNO, FnWrtno2);

                nREAD = list4.Count;
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        strRETURN += "[" + i + 1 + "] FVC(%):" + list4[i].FVC_PRED + ",";
                        strRETURN += "FVC1.0(%):" + list4[i].FEV10_PRED + ",";
                        strRETURN += "FEV1/FVC(%):" + list4[i].FEV1_FVC_MEAS + " ";
                    }
                }

                //종검의 PFT 결과를 읽음
                if (nREAD == 0)
                {
                    //등록번호 및 접수일자를 읽음
                    HIC_JEPSU list5 = hicJepsuService.GetItembyWrtNo(FnWRTNO);

                    strJepDate = list5.JEPDATE.ToString();
                    strPtNo = list5.PTNO;
                    if (strPtNo != "" && strJepDate != "")
                    {
                        List<COMHPC> list6 = comHpcLibBService.GetHeaResPftbyPtNoExDate(strPtNo, strJepDate);

                        nREAD = list6.Count;
                        if (nREAD > 0)
                        {
                            for (int i = 0; i < nREAD; i++)
                            {
                                strRETURN += "[" + i + 1 + "] FVC(%):" + list6[i].FVC_PRED + ",";
                                strRETURN += "FVC1.0(%):" + list6[i].FEV10_PRED + ",";
                                strRETURN += "FEV1/FVC(%):" + list6[i].FEV1_FVC_MEAS + " ";
                            }
                        }
                    }
                }
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        string READ_D1D2_Result(string argGbn, string argCode)
        {
            string rtnVal = "";
            string strRETURN = "";
            string strRet1 = "";
            string strRet2 = "";
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
            strRet1 = "";
            strRet2 = "";
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
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                strRet1 = strExName + ":" + strResult + ",";
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
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }
                    strRet2 += strExName + ":" + strResult + ",";
                }
            }

            if (VB.InStr(strRet1, "청력") > 0) strRet1 = "1차:" + strRet1;
            if (VB.InStr(strRet2, "청력") > 0) strRet2 = "2차:" + strRet2;
            strRETURN = strRet1 + strRet2;

            //마지막 컴마을 제거
            if (strRETURN != "")
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        /// <summary>
        /// READ_생물학적노출지표_Result()
        /// </summary>
        /// <returns></returns>
        string Read_Biological_Exposure_Index(string argGbn, List<string> argCode)
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
            List<string> strExCodes = new List<string>();
            List<string> strNewExCode = new List<string>();
            string strNomal = "";

            HIC_CODE list = hicCodeService.GetItembyGubunGCode(argGbn, argCode);

            if (list != null)
            {
                strExCodes.Add(list.CODE);
            }
            else
            {
                return rtnVal;
            }

            if (strExCodes.IsNullOrEmpty())
            {
                return rtnVal;
            }

            for (int i = 0; i < strExCodes.Count ; i++)
            {
                strNewExCode.Add(strExCodes[i]);
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoInExCodes(FnWRTNO, strNewExCode, "HIC");

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

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (strResult != "")
                {
                    strResult += "(" + strNomal + ")";
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWrtno2, strNewExCode, "Y");

                nRead = list3.Count;
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

                    //참고치를 Dispaly
                    strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (strResult != "")
                    {
                        strRETURN += "(" + strNomal + ")";
                    }
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //마지막 컴마 제거
            if (strRETURN != "")
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        /// <summary>
        /// READ_ExamSpc_Result()
        /// </summary>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        string fn_READ_ExamSpc_Result(string argGbn)
        {
            string rtnVal = "";
            string strRETURN = "";
            string strRet1 = "";
            string strRet2 = "";
            int nRead = 0;

            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNo_First(FnWRTNO, argGbn);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list[i].YNAME;
                if (strExName == "")
                {
                    strExName = list[i].HNAME;
                }

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                FstrExams += strExCode + ":" + strResult + ",";
                strRet2 += strExName + ":" + strResult + ",";
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNo_First(FnWrtno2, argGbn);

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
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    FstrExams += strExCode + ":" + strResult + ",";
                    strRet2 += strExName + ":" + strResult + ",";
                }
            }

            if (VB.InStr(strRet1, "청력") > 0) strRet1 = "1차:" + strRet1;
            if (VB.InStr(strRet2, "청력") > 0) strRet2 = "2차:" + strRet2;
            strRETURN = strRet1 + strRet2;

            //마지막 컴마 제거
            if (strRETURN != "")
            {
                strRETURN = VB.Left(strRETURN, strRETURN.Length - 1);
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        string Special_Result_Exist()
        {
            string rtnVal = "";
            string strJob = "";

            FnCNT1_1 = 0; FnCNT1_2 = 0;   //특수 1,2차 유소견건수
            FnCNT2_1 = 0; FnCNT2_2 = 0;   //일반 1,2차 유소견건수
            FnCNT3_1 = 0; FnCNT3_2 = 0;   //생애1차만, 생애2차실시

            //특수판정 1차의 C1,C2,D1,D2,R의 건수를 읽음
            FnCNT1_1 = hicSpcPanjengService.GetCountbyWrtNo2(FnWRTNO);

            if (rdoJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                strJob = "2";
            }
            else
            {
                strJob = "3";
            }
            //2차의 접수번호를 찾음
            HIC_JEPSU list = hicJepsuService.GetWrtnoByPanoJepDateGjJong(FnPano, FstrJepDate, FstrGjYear, strJob);

            if (!list.IsNullOrEmpty())
            {
                FnWrtno2 = list.WRTNO;
            }
            else
            {
                FnWrtno2 = 0;
            }

            //특수판정 2차의 C1,C2,D1,D2의 건수를 읽음
            if (FnWrtno2 > 0)
            {
                FnCNT1_2 = hicSpcPanjengService.GetCountbyWrtNo(FnWrtno2);
            }

            rtnVal = "";

            if (FnCNT1_1 > 0 || FnCNT1_2 > 0 || FnCNT2_1 > 0 || FnCNT2_2 > 0 || FnCNT3_1 > 0 || FnCNT3_2 > 0)
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        void fn_Screen_Special_Result()
        {
            string strTemp1 = "";

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

            strGong = list.BUSENAME;
            strIpsadate = list.IPSADATE.ToString();
            strJepDate = list.JEPDATE.ToString();
            nGunsokYY = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
            nGunsokMM = long.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
            nAge = list.AGE;
            strUNames = hm.UCode_Names_Display(list.UCODES);
            strMCode = list.UCODES;
            strUNames_EDIT = strUNames;

            //특수테이블
            chb.READ_HIC_RES_SPECIAL(FnWRTNO);

            FnRow += 1;
            SS1.ActiveSheet.RowCount = FnRow;

            if (strGong.IsNullOrEmpty())
            {
                strGong = "기타";
            }
            SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = strGong;
            SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
            SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
            SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
            SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
            SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = strUNames_EDIT;

            //생물학적 노출지표(참고치)
            if (strMCode != "")
            {
                if (VB.Right(strMCode, 1) == ",")
                {
                    strMCode = VB.Left(strMCode, strMCode.Length - 1);
                }
                strTemp.Clear();
                if (VB.L(strMCode, ",") == 1)
                {
                    strTemp.Add(strMCode);
                }
                else
                {
                    for (int j = 1; j <= VB.L(strMCode, ","); j++)
                    {
                        strTemp.Add(VB.Pstr(strMCode, ",", j));
                    }
                }

                strResult2 = Read_Biological_Exposure_Index("28", strTemp);
            }

            SS1_Sheet1.Cells.Get(FnRow - 1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = strResult2;

            nSpcRowStart = FnRow;
            FnStartRow = FnRow;
            nSpcResCnt = 0;

            SS1_Sheet1.Rows.Get(FnRow - 1).Border = new LineBorder(Color.Black, 1, false, true, false, false);

            //특수 유소견판정 유무 확인(유소견이 있는 사람은 정상(A)를 보여주지 않음. 손대영 요청 - 10.10.15
            strTemp2 = "";
            List<HIC_SPC_PANJENG> list3 = hicSpcPanjengService.GetItembyWrtNoWrtNo3(FnWRTNO, FnWrtno2);
            if (list3.Count > 0)
            {
                strTemp2 = "OK";
            }

            //특수 1,2차 판정 결과를 읽음
            List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GeResulttItembyWrtNoWrtNo2(FnWRTNO, FnWrtno2, strTemp2);

            nREAD = list2.Count;
            strOldData = "";
            strMCode = "";
            strResult2 = "";
            nRowCnt2 = 0;
            nRow = FnRow;

            for (int i = 0; i < nREAD; i++)
            {
                //소견코드
                strSogen = list2[i].SOGENCODE;
                strNewData = strSogen + "@" + list2[i].PANJENG;
                strNewData += "@" + list2[i].WORKYN;
                strMCode = "'" + list2[i].MCODE + "',";

                //소견코드,판정코드,업무수행적합여부가 동일하면 1개만 인쇄함
                if (strOldData != strNewData)
                {
                    strResult = "";
                    if (strSogen != "")
                    {
                        strResult = fn_READ_ExamSpc_Result(strSogen);
                    }
                    fn_Screen_Special_Sub(list2[i].PANJENG, list2[i].SOGENCODE, list2[i].JOCHIREMARK, list2[i].SAHUCODE, list2[i].SOGENREMARK, list2[i].WORKYN);

                    //사업장 수검현황 자료입력
                    if (FstrGubun == "C1" || FstrGubun == "D1" || FstrGubun == "C2" || FstrGubun == "D2")
                    {
                        fn_INSERT_SAHU_RESULT();
                    }

                    strOldData = strNewData;
                }
                strResult2 = "";
            }
        }

        void fn_Screen_Special_Result_Excel()
        {
            int nREAD = 0;
            int nSpcRow = 0;
            int nSpcRowStart = 0;
            int nRow = 0;
            int nResult2CNT = 0;
            int y = 0;
            int z = 0;
            long nPanjengDrno = 0;
            int nSpcResCnt = 0;  //특검 유질환 물질 건수

            string strSname = "";
            string strSex = "";
            long nAge = 0;
            string strIpsadate = "";
            string strJepDate = "";
            int nGunsok = 0;
            long nGunsokYY = 0;
            long nGunsokMM = 0;
            int nRowCnt2 = 0;
            string strPanName = "";
            string strPanjeng = "";
            string strGbPanBun2 = "";
            string strSogen = "";
            string strResult = "";
            string strOK = "";
            string strGong = "";

            string strUNames = "";
            string strUNames_EDIT = "";
            string strOldData = "";
            string strNewData = "";
            string strMCode = "";
            string strResult2 = "";
            List<string> strTemp = new List<string>();
            string strTemp2 = "";

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

            strGong = list.BUSENAME;
            strIpsadate = list.IPSADATE;
            strJepDate = list.JEPDATE;
            nGunsokYY = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
            nGunsokMM = long.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
            nAge = list.AGE;
            strUNames = hm.UCode_Names_Display(list.UCODES);

            //특수테이블
            chb.READ_HIC_RES_SPECIAL(FnWRTNO);

            //취급물질명 및 인적사항을 Display
            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = hb.READ_HIC_CODE("A2", clsHcType.B5.GONGJENG); //기타가공
            SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
            SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
            SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
            SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
            SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = strUNames;

            //특수 유소견판정 유무 확인(유소견이 있는 사람은 정상(A)를 보여주지 않음
            strTemp.Clear();
            strTemp2 = "";
            List<HIC_SPC_PANJENG> list3 = hicSpcPanjengService.GetItembyWrtNoUnionWrtNo2(FnWRTNO, FnWrtno2, "OK");

            if (list3.Count > 0)
            {
                strTemp2 = "OK";
            }

            //특수 1,2차 판정 결과를 읽음
            List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GetItembyWrtNoUnionWrtNo2(FnWRTNO, FnWrtno2, strTemp2);

            nREAD = list2.Count;
            strOldData = "";
            strMCode = "";
            strResult2 = "";

            strList_Chamgo = ""; strList_Gubun = "";
            strList_Sogen = ""; strList_Sahu = ""; strList_Upmu = "";

            for (int i = 0; i < nREAD; i++)
            {
                //소견코드
                strSogen = list2[i].SOGENCODE;
                strNewData = strSogen + "@" + list2[i].PANJENG;
                strNewData = strNewData + "@" + list2[i].WORKYN;
                strMCode = "'" + list2[i].MCODE + "',";

                if (strMCode != "")
                {
                    if (VB.Right(strMCode, 1) == ",")
                    {
                        strMCode = VB.Left(strMCode, strMCode.Length - 1);
                    }
                    strTemp.Clear();
                    if (VB.L(strMCode, ",") == 1)
                    {
                        strTemp.Add(strMCode);
                    }
                    else
                    {
                        for (int j = 1; j <= VB.L(strMCode, ","); j++)
                        {
                            strTemp.Add(VB.Pstr(strMCode, ",", j));
                        }
                    }

                    strResult2 = Read_Biological_Exposure_Index("28", strTemp);
                }

                //소견코드,판정코드,업무수행적합여부가 동일하면 1개만 인쇄함
                if (strOldData != strNewData)
                {
                    strResult = "";
                    if (strSogen != "")
                    {
                        strResult = fn_READ_ExamSpc_Result(strSogen);
                    }
                    if (strResult2 != "")
                    {
                        strList_Chamgo += strResult2 + "/";
                    }

                    fn_Screen_Special_Excel_Sub(list2[i].PANJENG, list2[i].SOGENCODE, list2[i].JOCHIREMARK, list2[i].SAHUCODE, list2[i].SOGENREMARK, list2[i].WORKYN);

                    //사업장 수검현황 자료입력
                    if (FstrGubun == "C1" || FstrGubun == "D1" || FstrGubun == "C2" || FstrGubun == "D2")
                    {
                        fn_INSERT_SAHU_RESULT();
                    }

                    strOldData = strNewData;
                }
                strResult2 = "";
            }

            if (VB.Right(strList_Chamgo, 1) == "/") strList_Chamgo = VB.Left(strList_Chamgo, strList_Chamgo.Length - 1);
            if (VB.Right(strList_Gubun, 1) == ",") strList_Gubun = VB.Left(strList_Gubun, strList_Gubun.Length - 1);
            if (VB.Right(strList_Sogen, 1) == "/") strList_Sogen = VB.Left(strList_Sogen, strList_Sogen.Length - 1);
            if (VB.Right(strList_Sahu, 1) == "/") strList_Sahu = VB.Left(strList_Sahu, strList_Sahu.Length - 1);
            if (VB.Right(strList_Upmu, 1) == ",") strList_Upmu = VB.Left(strList_Upmu, strList_Upmu.Length - 1);

            SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = strList_Chamgo;
            SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = strList_Gubun;
            SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = strList_Sogen;
            SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strList_Sahu;
            SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = strList_Upmu;
        }

        void fn_INSERT_SAHU_RESULT()
        {
            if (hicSpcSahuService.GetRowIdbyWrtNo(FnWRTNO, FstrGjYear, FstrLtdCode) > 0)
            {
                return;
            }
            else
            {
                HIC_SPC_SAHU item = new HIC_SPC_SAHU();
                item.GJYEAR = FstrGjYear;
                item.LTDCODE = long.Parse(FstrLtdCode);
                item.GUBUN = FstrGubun;
                item.SOGEN = FstrSogen;
                item.EXAM = FstrExams;
                item.JOCHI = FstrJochi;

                int result = hicSpcSahuService.Insert(item);
            }
        }

        void fn_Screen_Special_Excel_Sub(string argPanjeng, string argSogenCode, string argJochiRemark, string argSahuCode, string argSogenRemark, string argWorkYn)
        {
            string strJochi = "";
            string strSahu = "";
            int nMCnt = 0;
            long nJochiCnt = 0;
            long nResultCNT = 0;
            int ii = 0;
            int jj = 0;
            string strCFlag = "";

            nSpcResCnt += 1;

            strCFlag = "";
            switch (argPanjeng)
            {
                case "1":
                case "2":
                    strList_Gubun += "A,";
                    break;
                case "3":
                    strList_Gubun += "C1,";
                    break;
                case "4":
                    strList_Gubun += "C2,";
                    break;
                case "5":
                    strList_Gubun += "D1,";
                    break;
                case "6":
                    strList_Gubun += "D2,";
                    break;
                case "7":
                case "8":
                    strList_Gubun += "U,";
                    break;
                default:
                    strList_Gubun += "";
                    break;
            }

            FstrGubun = SS1.ActiveSheet.Cells[FnRow - 1, 7].Text;
            FstrSogen = argSogenCode;
            FstrJochi = argJochiRemark;
            strSahu = hm.Sahu_Names_Display(argSahuCode);
            strJochi = argJochiRemark;
            strSogen = hb.READ_SpcSCode_Name(argSogenCode); //소견

            if (string.Compare(argPanjeng, "2") > 0)
            {
                strList_Sogen += strSogen + "/";
                strList_Sahu += strJochi + "/";
            }
            else
            {
                strList_Sogen += "정상/";
                strList_Sahu += "필요없음/";
            }

            //업무수행적합여부
            switch (argWorkYn)
            {
                case "001":
                    strList_Upmu += "가";
                    break;
                case "002":
                    strList_Upmu += "나";
                    break;
                case "003":
                    strList_Upmu += "다";
                    break;
                case "004":
                    strList_Upmu += "라";
                    break;
                default:
                    break;
            }
        }

        void fn_Screen_Special_Sub(string argPanjeng, string argSogenCode, string argJochiRemark, string argSahuCode, string argSogenRemark, string argWorkYn)
        {
            string strJochi = "";
            string strSahu = "";
            int nMCnt = 0;
            long nJochiCnt = 0;
            long nResultCNT = 0;
            int ii = 0;
            int jj = 0;
            string strCFlag = "";

            nSpcResCnt += 1;

            if (nSpcResCnt > 1)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }
            }

            strCFlag = "";
            switch (argPanjeng)
            {
                case "1":
                case "2":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A";
                    break;
                case "3":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C1";
                    break;
                case "4":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C2";
                    break;
                case "5":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D1";
                    break;
                case "6":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
                    break;
                case "7":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "U";
                    break;
                case "8":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "U";
                    break;
                case "9":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "CN";
                    break;
                case "A":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "DN";
                    break;
                default:
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "";
                    break;
            }

            FstrGubun = SS1.ActiveSheet.Cells[FnRow - 1, 7].Text;
            FstrSogen = argSogenCode;
            FstrJochi = argJochiRemark;
            strSahu = hm.Sahu_Names_Display(argSahuCode);
            strJochi = argJochiRemark;
            strSogen = argSogenRemark;

            if (VB.InStr(strSogen, "야간작업") > 0)
            {
                if (VB.InStr(strSogen, "고혈압") > 0) strResult = strResult + Read_Exam_Result("일반", "03", "");
                if (VB.InStr(strSogen, "이상지질") > 0) strResult = strResult + Read_Exam_Result("일반", "04", "");
                if (VB.InStr(strSogen, "당뇨") > 0) strResult = strResult + Read_Exam_Result("일반", "06", "");
            }
            else if (VB.InStr(strSogen, "폐기능") > 0)
            {
                if (strResult == "")
                {
                    strResult += Read_Exam_Result("특수", "J", "");
                }
            }

            if (strSogen != "")
            {
                strResult = strSogen + "\r\n" + strResult;
            }

            if (string.Compare(argPanjeng, "2") > 0)
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = strResult;
                SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strJochi;
            }
            else
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "정상";
                SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = "필요없음";
            }

            //업무수행적합여부
            SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
            if (strCFlag != "OK")
            {
                switch (argWorkYn)
                {
                    case "001":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "가";
                        break;
                    case "002":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "나";
                        break;
                    case "003":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "다";
                        break;
                    case "004":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "라";
                        break;
                    default:
                        break;
                }
            }         
        }

        void fn_Screen_First_Result(long nWrtNo)
        {
            for (int i = 0; i < 5; i++)
            {
                strPanjengC[i] = "";
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

            chb.READ_HIC_RES_BOHUM1(FnWRTNO);
            chb.READ_HIC_RES_BOHUM2(FnWrtno2);

            if (clsHcType.B1.ROWID.IsNullOrEmpty())
            {
                strOK = "";
                //질병관리(C)가 있으면 표시함
                if (clsHcType.B1.PANJENGC1 == "1") { strOK = "OK"; strPanjengC[1] = "1"; }
                if (clsHcType.B1.PANJENGC2 == "1") { strOK = "OK"; strPanjengC[2] = "1"; }
                if (clsHcType.B1.PANJENGC3 == "1") { strOK = "OK"; strPanjengC[3] = "1"; }
                if (clsHcType.B1.PANJENGC4 == "1") { strOK = "OK"; strPanjengC[4] = "1"; }
                if (clsHcType.B1.PANJENGC5 == "1") { strOK = "OK"; strPanjengC[5] = "1"; }

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
                if (clsHcType.B1.PANJENGD11 != "") { strOK = "OK"; strPanjengD1[1] = clsHcType.B1.PANJENGD11; }
                if (clsHcType.B1.PANJENGD12 != "") { strOK = "OK"; strPanjengD1[2] = clsHcType.B1.PANJENGD12; }
                if (clsHcType.B1.PANJENGD13 != "") { strOK = "OK"; strPanjengD1[3] = clsHcType.B1.PANJENGD13; }
                if (clsHcType.B1.PANJENGD21 != "") { strOK = "OK"; strPanjengD2[1] = clsHcType.B1.PANJENGD21; }
                if (clsHcType.B1.PANJENGD22 != "") { strOK = "OK"; strPanjengD2[2] = clsHcType.B1.PANJENGD22; }
                if (clsHcType.B1.PANJENGD23 != "") { strOK = "OK"; strPanjengD2[3] = clsHcType.B1.PANJENGD23; }

                //유질환(D)이 있으면 표시함
                if (clsHcType.B1.PANJENGU1 == "1") { strOK = "OK"; strPanjengU[1] = clsHcType.B1.PANJENGU1; } //고혈압
                if (clsHcType.B1.PANJENGU2 == "1") { strOK = "OK"; strPanjengU[2] = clsHcType.B1.PANJENGU2; } //당뇨
                if (clsHcType.B1.PANJENGU3 == "1") { strOK = "OK"; strPanjengU[3] = clsHcType.B1.PANJENGU3; } //이상지질
                if (clsHcType.B1.PANJENGU4 == "1") { strOK = "OK"; strPanjengU[4] = clsHcType.B1.PANJENGU4; } //폐결핵

                //2010 생애일경우 2차 안했으면 strok ="OK" 달아줌
                if (Fstr생애구분 == "생애" && clsHcType.B2.ROWID == "")
                {
                    strPanjeng = "X";
                    strPanName = "2차생애미수검";
                    strSogen = "★2차생애상담필요합니다.";
                    strResult = "2차생애상담필요";
                    fn_Screen_First_Sub(strPanName, strResult);
                }

                if (strOK != "OK")
                {
                    return;
                }

                //건강주의(C)
                for (int i = 0; i < 5; i++)
                {
                    if (strPanjengC[i] == "1")
                    {
                        switch (i)
                        {
                            case 0:
                                strPanName = "폐결핵주의"; strPanjeng = "3"; strGbPanBun2 = "01";
                                break;
                            case 1:
                                strPanName = "간장질환주의"; strPanjeng = "3"; strGbPanBun2 = "05";
                                break;
                            case 2:
                                strPanName = "빈혈증주의"; strPanjeng = "3"; strGbPanBun2 = "08";
                                break;
                            case 3:
                                strPanName = "기타질환주의"; strPanjeng = "3"; strGbPanBun2 = "10";
                                break;
                            case 4:
                                strPanName = "비만주의"; strPanjeng = "3"; strGbPanBun2 = "12";
                                break;
                            default:
                                break;
                        }
                        strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                        fn_Screen_First_Sub(strPanName, strResult);
                    }
                }
                //만일 2차대상자이면 R2항목은 1차꺼 제외함
                //질환의심(R1,R2)
                for (int i = 0; i < 12; i++)
                {
                    if (clsHcType.B1.PanjengR[i] == "1")
                    {
                        switch (i)
                        {
                            case 0:
                                strPanName = "폐결핵의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 1:
                                strPanName = "기타흉부질환의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 2:
                                strPanName = "고혈압";
                                strPanjeng = "5";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                if (clsHcType.B2.Cycle_RES == "")
                                {
                                    strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                }
                                break;
                            case 3:
                                strPanName = "이상지질혈증의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 4:
                                strPanName = "간장질환의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 5:
                                strPanName = "당뇨병";
                                strPanjeng = "5";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                if (clsHcType.B2.Diabetes_Res == "")
                                {
                                    strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                }
                                break;
                            case 6:
                                strPanName = "신장질환의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 7:
                                strPanName = "빈혈증의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 8:
                                strPanName = "골다공증의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 9:
                                strPanName = "기타질환의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            case 10:
                                strPanName = "비만관리의심";
                                strPanjeng = "4";
                                strGbPanBun2 = string.Format("{0:00}", i + 1);
                                strResult += Read_Exam_Result("일반", strGbPanBun2, "");
                                break;
                            default:
                                break;
                        }
                        fn_Screen_First_Sub(strPanName, strResult);
                    }
                }

                //직업병(D1)
                for (int i = 1; i <= 3; i++)
                {
                    if (strPanjengD1[i] != "")
                    {
                        strPanName = fn_Read_GenPanjengD1D2("31", strPanjengD1[i]);
                        strResult = "";
                        strResult += READ_D1D2_Result("31", strPanjengD1[i]);
                        if (strPanName != "")
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
                    if (strPanjengD2[i] != "")
                    {
                        strPanName = fn_Read_GenPanjengD1D2("33", strPanjengD2[i]);
                        strResult = "";
                        strResult += READ_D1D2_Result("33", strPanjengD2[i]);
                        if (strPanName != "")
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
                            case 1:
                                strPanName = "고혈압";
                                strPanjeng = "8";
                                strGbPanBun2 = "03";
                                break;
                            case 2:
                                strPanName = "유질환(당뇨병)";
                                strPanjeng = "8";
                                strGbPanBun2 = "05";
                                break;
                            case 3:
                                strPanName = "유질환(이상지질혈증)";
                                strPanjeng = "8";
                                strGbPanBun2 = "04";
                                break;
                            case 4:
                                strPanName = "유질환(폐결핵)";
                                strPanjeng = "8";
                                strGbPanBun2 = "01";
                                break;
                            default:
                                break;
                        }

                        strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                        fn_Screen_First_Sub(strPanName, strResult);
                    }
                }
            }
            //마지막 컴마 제거
            if (strResult != "" && VB.Right(strResult, 1) == ",")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }
        }

        /// <summary>
        /// 내용을 Display
        /// </summary>
        void fn_Screen_First_Sub(string argPanName, string argResult)
        {
            long nSogenCnt = 0;
            long nResultCNT = 0;
            long jj = 0;

            //특수에서 발생된 소견이 2차의 소견과 동일한 것이 있으면 2차 결과는 생략
            for (int i = FnStartRow; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 6].Text == strPanName)
                {
                    nSogenCnt = 0;
                    strSogen = "";
                    strResult = "";
                    strPanName = "";
                    return;
                }
            }
            nSogenCnt = 0;
            nResultCNT = 0;

            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            switch (strPanjeng)
            {
                case "3":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C";
                    break;
                case "4":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "R1";
                    break;
                case "5":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "R2";
                    break;
                case "6":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D1";
                    break;
                case "7":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
                    break;
                case "8":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D";
                    break;
                case "X":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "R"; //생애1차했을경우 2010
                    break;
                default:
                    break;
            }

            if (strPanName != "")
            {
                strResult = strPanName + "\r\n" + strResult;
            }
            SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = strResult;
            SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strSogen;

            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            nSogenCnt = 0;
            strSogen = "";
            strResult = "";
            strPanName = "";
        }

        void fn_Screen_Second_Sub()
        {
            long nSogenCnt = 0;
            long nResultCNT = 0;
            int jj = 0;
            string strCFlag = "";

            //특수에서 발생된 소견이 2차의 소견과 동일한 것이 있으면 2차 결과는 생략
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 6].Text == strPanName)
                {
                    nSogenCnt = 0;
                    strResult = "";
                    strPanName = "";
                    return;
                }
            }

            nSogenCnt = 0;
            nResultCNT = 0;

            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            strCFlag = "";
            if (strGbPanBun2 == "03")
            {
                if (strPanjeng == "1") SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A";
                if (strPanjeng == "2")
                {
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C2";
                    strCFlag = "OK";
                }
                if (strPanjeng == "3") SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
            }
            else
            {
                if (strPanjeng == "1") SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A";
                if (strPanjeng == "2")
                {
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C2";
                    strCFlag = "OK";
                }
                if (strPanjeng == "3") SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
            }

            if (strPanjeng == "X")
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "2차생애상담";
            }

            if (strPanName != "")
            {
                strResult = strPanName + "\r\n" + strResult;
            }
            SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = strResult;
            SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strSogen;
            SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "";

            if (strCFlag != "OK")
            {
                switch (clsHcType.B2.WorkYN)
                {
                    case "001":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "가";
                        break;
                    case "002":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "나";
                        break;
                    case "003":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "다";
                        break;
                    case "004":
                        SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "라";
                        break;
                    default:
                        break;
                }
            }

            nSogenCnt = 0;
            strSogen = "";
            strResult = "";
            strPanName = "";
        }

        void fn_Screen_Second_Result(long nWrtNo)
        {
            string strSogen = "";
            string strPanName = "";
            string strPanjeng = "";
            string strGbPanBun2 = "";
            string strResult = "";
            string strOK = "";

            chb.READ_HIC_RES_BOHUM2(FnWrtno2);

            //2010 생애2차만 했을경우
            if (Fstr생애구분 == "생애" && clsHcType.B2.ROWID != "")
            {
                strPanjeng = "X";
                strPanName = "2차생애수검";
                strSogen = "★2차생애수검";
                strResult = fn_Read_Living_Habit_Order(FnWRTNO);
                fn_Screen_Second_Sub();
            }

            //판정결과가 건강주의,유질환자만 표시함
            if (clsHcType.B2.Panjeng == "1")
            {
                return;
            }

            if (clsHcType.B2.Cycle_RES == "1" || clsHcType.B2.Cycle_RES == "2" || clsHcType.B2.Cycle_RES == "3")
            {
                if (clsHcType.B2.Cycle_RES == "1") strPanName = "고혈압-정상";
                if (clsHcType.B2.Cycle_RES == "2") strPanName = "고혈압전단계";
                if (clsHcType.B2.Cycle_RES == "3") strPanName = "고혈압";
                strPanjeng = clsHcType.B2.Cycle_RES;
                strGbPanBun2 = "03";
                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                fn_Screen_Second_Sub();
            }

            if (clsHcType.B2.Diabetes_Res == "1" || clsHcType.B2.Diabetes_Res == "2" || clsHcType.B2.Diabetes_Res == "3")
            {
                if (clsHcType.B2.Diabetes_Res == "1") strPanName = "당뇨병-정상";
                if (clsHcType.B2.Diabetes_Res == "2") strPanName = "공복혈당장애";
                if (clsHcType.B2.Diabetes_Res == "3") strPanName = "당뇨병";
                strPanjeng = clsHcType.B2.Diabetes_Res;
                strGbPanBun2 = "06";
                strResult = Read_Exam_Result("일반", strGbPanBun2, "");
                fn_Screen_Second_Sub();
            }
        }

        /// <summary>
        /// READ_생활습관처방읽기()
        /// </summary>
        /// <param name="nRwtNo"></param>
        /// <returns></returns>
        string fn_Read_Living_Habit_Order(long argWrtNo)
        {
            string rtnVal = "";
            int TempX = 0;

            HIC_RES_BOHUM1 list = hicJepsuLtdResBohum1Service.GetItembyWrtNo(argWrtNo);

            if (list != null)
            {
                if (list.HABIT1 == "1")
                {
                    rtnVal += " '음주',";
                }

                if (list.HABIT2 == "1")
                {
                    rtnVal += " '흡연',";
                }

                if (list.HABIT3 == "1")
                {
                    rtnVal += " '운동',";
                }

                if (list.HABIT4 == "1")
                {
                    rtnVal += " '비만',";
                }

                if (list.HABIT5 == "1")
                {
                    rtnVal += " '영양',";
                }

                if (rtnVal != "")
                {
                    rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 1);
                    rtnVal += rtnVal + "에 관한 처방";
                }
            }

            return rtnVal;
        }

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
            txtJepDate.Text = "";
            btnPrint.Enabled = false;

            //의사도장 복사 때문에 수동으로 클리어
            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nREAD = 0;
            long nPanjengDrno = 0;
            string strOK = "";
            int nPrtCNT = 0;
            string strGjYear = "";
            string strBangi = "";
            string strJob = "";
            
            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            Application.DoEvents();

            clsHcType.B1_CLEAR();

            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);
            btnPrint.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;

            //변수를 Clear
            FnRow = 0;
            FnDoctCnt = 0;
            nPrtCNT = 0;

            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            strGjYear = cboYear.Text;
            if (cboBangi.Text.Trim() == "상반기")
            {
                strBangi = "1";
            }
            else if (cboBangi.Text.Trim() == "하반기")
            {
                strBangi = "2";
            }

            if (rdoJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                strJob = "2";
            }
            else if (rdoJob3.Checked == true)
            {
                strJob = "3";
            }

            //회사별 특수검진 명단을 읽음
            List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyJepDateLtdCodeGjYear(FstrFDate, FstrTDate, FstrLtdCode, strGjYear, strBangi, strJob);

            nREAD = list.Count;
            nRow = 0;
            progressBar1.Maximum = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    clsHcType.B1.PanjengR[j] = "";
                }

                FstrExams = "";

                FnWRTNO = list[i].WRTNO;
                FnPano = list[i].PANO;
                FstrSex = list[i].SEX;
                FstrSName = list[i].SNAME;
                FstrUCodes = list[i].UCODES;
                FstrJepDate = list[i].JEPDATE.ToString();
                FstrGjYear = list[i].GJYEAR;
                FstrGjBangi = list[i].GJBANGI;
                FstrGjJong = list[i].GJJONG;

                if (FstrSName == "정현윤")
                {
                    int ii = 0;
                }

                switch (FstrGjJong)
                {
                    case "41":
                        Fstr생애구분 = "생애";
                        break;
                    default:
                        Fstr생애구분 = "일반";
                        break;
                }

                //특수판정 판정의사를 읽음(순서: 2차,1차)
                nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWrtno2);

                if (nPanjengDrno == 0)
                {
                    nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWRTNO);
                }

                //판정의사 명단 등록
                if (nPanjengDrno > 0)
                {
                    if (FnDoctCnt == 0)
                    {
                        FnDoctCnt = 1;
                        FnPanDrNo[FnDoctCnt - 1] = nPanjengDrno;
                    }
                    else
                    {
                        strOK = "";
                        for (int j = 0; j < FnDoctCnt; j++)
                        {
                            if (FnPanDrNo[j] == nPanjengDrno)
                            {
                                strOK = "OK";
                                break;
                            }
                        }

                        if (strOK != "OK")
                        {
                            FnDoctCnt += 1;
                            FnPanDrNo[FnDoctCnt - 1] = nPanjengDrno;
                        }
                    }
                }

                //특수 사후관리 유소견자가 있으면
                if (Special_Result_Exist() == "OK")
                {
                    nPrtCNT += 1;
                    fn_Screen_Special_Result();
                }
                progressBar1.Value = i + 1;
            }

            if (nPrtCNT == 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }
                SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "**해당없음**";
            }

            for (int i = 0; i < FnRow; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 5);
                Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 8);
                Size size2 = SS1.ActiveSheet.GetPreferredCellSize(i, 9);

                if (size.Height >= size1.Height && size.Height >= size2.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
                else if (size.Height >= size1.Height && size.Height <= size2.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size2.Height;
                }
                else if (size1.Height >= size.Height && size1.Height >= size2.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size1.Height;
                }
                else if (size1.Height >= size.Height && size1.Height <= size2.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size2.Height;
                }
                else if (size2.Height >= size.Height && size2.Height >= size1.Height)
                {
                    SS1.ActiveSheet.Rows[i].Height = size2.Height;
                }
                else if (size2.Height >= size.Height && size2.Height <= size1.Height)
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

            //선긋기
            SS1_Sheet1.Rows.Get(FnRow - 3).Border = new LineBorder(Color.Black, 1, false, true, false, false);

            SS1_Sheet1.Cells.Get(FnRow - 3, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).Value = "비고";

            SS1_Sheet1.Cells.Get(FnRow - 2, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).Value = "1)이 법에 해당되는 건강진단 항목만 기재";
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).Value = "  ▶작성일자: " + clsPublic.GstrSysDate;

            SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).Value = "2)검진소견,사후관리 소견, 업무수행 적합여부는";
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).Value = "  ▶검진기관: 포 항 성 모 병 원";

            for (int i = 0; i < FnDoctCnt; i++)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }

                if (i == 0)
                {
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "  ▶판정의사: " + hb.READ_License_DrName(FnPanDrNo[i]);
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = "(인)";
                    hf.SetDojangImage(SS1, FnRow - 1, 9, FnPanDrNo[i].ToString());
                    SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                    SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = "요관찰자,유소견자 등 이상 소견이 있는 검진자만 기재";
                    SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
                }
                else
                {
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "                 " + hb.READ_License_DrName(FnPanDrNo[i]);
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = "(인)";
                    hf.SetDojangImage(SS1, FnRow - 1, 9, FnPanDrNo[i].ToString());
                    SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                }
                SS1.ActiveSheet.RowCount = FnRow;
            }

            Cursor.Current = Cursors.Default;
        }

        void fn_Screen_Display_Excel()
        {
            int nRow = 0;
            int nREAD = 0;
            long nPanjengDrno = 0;
            string strOK = "";
            int nPrtCNT = 0;
            string strGjYear = "";
            string strBangi = "";
            string strJob = "";

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            Application.DoEvents();
            btnPrint.Enabled = false;
            btnExcel.Enabled = true;

            clsHcType.B1_CLEAR();
            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);

            Cursor.Current = Cursors.WaitCursor;

            //변수를 Clear
            FnRow = 0;
            FnDoctCnt = 0;
            nPrtCNT = 0;

            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            strGjYear = cboYear.Text;
            if (cboBangi.Text.Trim() == "상반기")
            {
                strBangi = "1";
            }
            else if (cboBangi.Text.Trim() == "하반기")
            {
                strBangi = "2";
            }

            if (rdoJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                strJob = "2";
            }
            else if (rdoJob3.Checked == true)
            {
                strJob = "3";
            }

            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            //회사별 특수검진 명단을 읽음
            List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyJepDateLtdCodeGjYear(FstrFDate, FstrTDate, FstrLtdCode, strGjYear, strBangi, strJob);

            nREAD = list.Count;
            nRow = 0;
            progressBar1.Maximum = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    clsHcType.B1.PanjengR[j] = "";
                }

                FstrExams = "";

                FnWRTNO = list[i].WRTNO;
                FnPano = list[i].PANO;
                FstrSex = list[i].SEX;
                FstrSName = list[i].SNAME;
                FstrUCodes = list[i].UCODES;
                FstrJepDate = list[i].JEPDATE;
                FstrGjYear = list[i].GJYEAR;
                FstrGjBangi = list[i].GJBANGI;
                FstrGjJong = list[i].GJJONG;

                switch (FstrGjJong)
                {
                    case "41":
                        Fstr생애구분 = "생애";
                        break;
                    default:
                        Fstr생애구분 = "일반";
                        break;
                }

                //특수판정 판정의사를 읽음(순서: 2차,1차)
                nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWrtno2);

                if (nPanjengDrno == 0)
                {
                    nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWRTNO);
                }

                //판정의사 명단 등록
                if (nPanjengDrno > 0)
                {
                    if (FnDoctCnt == 0)
                    {
                        FnDoctCnt = 1;
                        FnPanDrNo[FnDoctCnt - 1] = nPanjengDrno;
                    }
                    else
                    {
                        strOK = "";
                        for (int j = 0; j < FnDoctCnt; j++)
                        {
                            if (FnPanDrNo[j] == nPanjengDrno)
                            {
                                strOK = "OK";
                                break;
                            }
                        }

                        if (strOK != "OK")
                        {
                            FnDoctCnt += 1;
                            FnPanDrNo[FnDoctCnt - 1] = nPanjengDrno;
                        }
                    }
                }

                //특수 사후관리 유소견자가 있으면
                if (Special_Result_Exist() == "OK")
                {
                    nPrtCNT += 1;
                    fn_Screen_Special_Result_Excel();
                }
                progressBar1.Value = i + 1;
            }

            if (nPrtCNT == 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }
                SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "  ** 해 당 없 음 **";
            }

            //판정의사를 Display
            FnRow += 3;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            //선긋기
            SS1_Sheet1.Rows.Get(FnRow - 3).Border = new LineBorder(Color.Black, 1, false, true, false, false);

            SS1_Sheet1.Cells.Get(FnRow - 3, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 3, 0).Value = "비고";

            SS1_Sheet1.Cells.Get(FnRow - 2, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 2, 0).Value = "1)이 법에 해당되는 건강진단 항목만 기재";
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 2, 8).Value = "  ▶작성일자: " + clsPublic.GstrSysDate;

            SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 1, 0).Value = "2)검진소견,사후관리 소견, 업무수행 적합여부는";
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            SS1_Sheet1.Cells.Get(FnRow - 1, 8).Value = "  ▶검진기관: 포 항 성 모 병 원";

            for (int i = 0; i < FnDoctCnt; i++)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }

                if (i == 0)
                {
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "  ▶판정의사: " + hb.READ_License_DrName(FnPanDrNo[i]);
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = "(인)";
                    hf.SetDojangImage(SS1, FnRow - 1, 9, FnPanDrNo[i].ToString());
                    SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                    SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = "요관찰자,유소견자 등 이상 소견이 있는 검진자만 기재";
                    SS1_Sheet1.Cells.Get(FnRow - 1, 0).ColumnSpan = 6;
                }
                else
                {
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "                 " + hb.READ_License_DrName(FnPanDrNo[i]);
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = "(인)";
                    hf.SetDojangImage(SS1, FnRow - 1, 9, FnPanDrNo[i].ToString());
                    SS1_Sheet1.SetRowHeight(FnRow - 1, 60);
                }
                SS1.ActiveSheet.RowCount = FnRow;
            }

            Cursor.Current = Cursors.Default;
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

            if (!list.IsNullOrEmpty())
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
