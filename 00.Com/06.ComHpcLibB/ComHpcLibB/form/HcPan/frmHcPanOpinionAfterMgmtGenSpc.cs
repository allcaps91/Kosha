using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanOpinionAfterMgmtGenSpc.cs
/// Description     : 질별유소견자사후관리(대행)
/// Author          : 이상훈
/// Create Date     : 2019-10-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm질별유소견자사후관리_일반특수.frm(Frm질별유소견자사후관리_일반특수)" />

namespace ComHpcLibB
{
    public partial class frmHcPanOpinionAfterMgmtGenSpc : Form                                                  
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

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType ht = new clsHcType();

        //보건 근로자상담관리
        FpSpread SSCard;
        int SSCardRow;
        Dictionary<string, OSHA_HEALTH_CHECK_MODEL> workerHealthCheckList;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;
        string strGjYear;
        string pano;

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
        string FstrChk;
        string FstrNameFlag;
        string Fstr생애Flag;

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

        string strGongjeng;

        string[] strPanjengC = new string[5];
        string[] strPanjengD1 = new string[3];
        string[] strPanjengD2 = new string[3];
        string[] strPanjengU = new string[4];
        string strFlag = ""; //생애한줄 작업
        string strD2Flag = "";  //D2 대상자

        public frmHcPanOpinionAfterMgmtGenSpc()
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
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
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
            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            sp.Spread_All_Clear(SS1);
            txtLtdCode.Text = "";
            cboYear.Items.Clear();

            //건진년도 Combo SET
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYEAR));
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
            //vbHicDojang.SignImage_Download(); => hf.SetDojangImage 로 대체
        }
        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
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
                long nLtdCode = 0;
                string strJob = "";
                string strBangi = "";

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 50;

                lblLtdName.Text = "";
                txtJepDate.Text = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strGjYear = cboYear.Text;
                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }

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
                    strBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strBangi = "2";
                }

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;
                ssList.Enabled = false;

                List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyJepDateGjYearGjBangi_GenSpc(strFrDate, strToDate, strGjYear, strBangi, strJob, nLtdCode);
                nRead = list.Count;
                nRow = 0;
                ssList_Sheet1.Rows.Get(-1).Height = 24;
                ssList.ActiveSheet.RowCount = nRead;

                for (int i = 0; i < nRead; i++)
                {   
                    nRow += 1;
                    if (nRow > ssList.ActiveSheet.RowCount)
                    {
                        ssList.ActiveSheet.RowCount = nRow;
                    }
                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:N0}", list[i].CNT);
                    ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].LTDCODE.ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].MINDATE.ToString();
                    FstrDate1 = list[i].MINDATE.ToString(); 
                    ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].MAXDATE.ToString();
                    FstrDate2 = list[i].MAXDATE.ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].CNT.ToString();
                    
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

                strTitle = "근로자 건강진단 사후관리 소견서(대행)";
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
                string strFileName = "C:\\ExcelFileDown\\근로자 건강진단 사후관리 소견서(대행)_" + lblLtdName.Text + "(" + txtJepDate.Text + ")";

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

        public void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true && e.RowHeader == false)
                {
                    if (MessageBox.Show("선택한 " + e.Row + "번째 Row를 제거 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SS1_Sheet1.RemoveRows(e.Row, 1);
                    }
                }
            }
            else if (sender == ssList)
            {
                lblLtdName.Text = " " + ssList.ActiveSheet.Cells[e.Row, 0].Text;
                FstrLtdCode = ssList.ActiveSheet.Cells[e.Row, 2].Text;
                FstrFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text;
                FstrTDate = ssList.ActiveSheet.Cells[e.Row, 4].Text;
                txtJepDate.Text = FstrDate1 + " ~ " + FstrDate2;


                fn_Screen_Display();
                MessageBox.Show("작업완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public FpSpread GetPanjeongBySite(string FstrFDate, string FstrTDate, string FstrLtdCode, string strGjYear)
        {
            sp.Spread_All_Clear(SS1);

            this.FstrFDate = FstrFDate;
            this.FstrTDate = FstrTDate;
            this.FstrLtdCode = FstrLtdCode;
            this.strGjYear = strGjYear;
            
            fn_Screen_Display();

            return SS1;
        }
        public void SetSpread(FpSpread ssCard, FpSpread fpSpread, string FstrFDate, string FstrTDate, string FstrLtdCode, string strGjYear, string pano)
        {
            //this.FstrFDate = "2018-08-13";
            //this.FstrTDate = "2018-08-28";
            //this.FstrLtdCode = "43025";
            //this.strGjYear = "2018";
            workerHealthCheckList = new Dictionary<string, OSHA_HEALTH_CHECK_MODEL>();
            this.FstrFDate = FstrFDate;
            this.FstrTDate = FstrTDate;
            this.FstrLtdCode = FstrLtdCode;
            this.strGjYear = strGjYear;
            this.pano = pano;
            this.SS1 = fpSpread;
            this.SSCard = ssCard; //근로자 건강상담 및 사후관리
            this.SSCardRow = 12;

            try
            {
                fn_Screen_Display();
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert("질병유소견자(대행) 정보를 가져오는중 오류가 발생하였습니다");
            }
            

            //for (int i = 0; i < 6; i++)
            //{
            //    SSCard.ActiveSheet.Cells[12 + i, 2].Value = ""; //년도
            //    SSCard.ActiveSheet.Cells[12 + i, 3].Value = ""; 
            //    SSCard.ActiveSheet.Cells[12 + i, 38].Value = ""; 
            //}

            int con = workerHealthCheckList.Count;
            foreach (KeyValuePair<string, OSHA_HEALTH_CHECK_MODEL> pair in workerHealthCheckList.OrderByDescending(i => i.Key))
            {
                SSCard.ActiveSheet.Cells[SSCardRow, 2].Value = pair.Value.YEAR;
                SSCard.ActiveSheet.Cells[SSCardRow, 3].Value = pair.Value.SOGEN;
                //      SSCard.ActiveSheet.Cells[SSCardRow, 28].Value = pair.Value.SOGEN;
                
                SSCard.ActiveSheet.Cells[SSCardRow, 38].Value = pair.Value.JOB;
                SSCard.ActiveSheet.Rows[SSCardRow].Height = SSCard.ActiveSheet.Rows[SSCardRow].GetPreferredHeight();

                SSCardRow++;
            }

            for(int i=0; i< SS1.ActiveSheet.RowCount; i++)
            {
                if(i< SS1.ActiveSheet.RowCount - 3)
                {
                    SS1.ActiveSheet.Rows[i].Height = SS1.ActiveSheet.Rows[i].GetPreferredHeight();
                }
                
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
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoSogenCode(FnWRTNO, argGbn, strNewExCode, argJob);

            nREAD = list2.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName.IsNullOrEmpty()) strExName = list2[i].HNAME;
                if (VB.Left(strExName, 3) == "HDL") strExName = "HDL";
                if (VB.Left(strExName, 3) == "LDL") strExName = "LDL";

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

                if (strResult == ".")
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
                    if (strExName.IsNullOrEmpty())
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
                        if (!strResult.IsNullOrEmpty())
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

                    if (!strResult.IsNullOrEmpty())
                    {
                        strRETURN += strExName + ":" + strResult + ",";
                    }
                }
            }

            //if (VB.InStr(strRet1, "청력") > 0) strRet1 = "1차:" + strRet1;
            //if (VB.InStr(strRet2, "청력") > 0) strRet2 = "2차:" + strRet2;
            //strRETURN = strRet1 + strRet2;

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
                    if (!strPtNo.IsNullOrEmpty() && !strJepDate.IsNullOrEmpty())
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
            string strExCodes = "";
            //string strNewExCode = "";
            List<string> strNewExCode = new List<string>();
            string strNomal = "";

            HIC_CODE list = hicCodeService.GetItembyGubunGCode(argGbn, argCode);

            if (!list.IsNullOrEmpty())
            {
                strExCodes = list.CODE;
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
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoNewExCode(FnWRTNO, strNewExCode, "N");

            nRead = list2.Count;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName.IsNullOrEmpty())
                {
                    strExName = list2[i].HNAME;
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (!strResult.IsNullOrEmpty())
                {
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
                    if (strExName.IsNullOrEmpty())
                    {
                        strExName = list3[i].HNAME;
                    }

                    //참고치를 Dispaly
                    strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (!strResult.IsNullOrEmpty())
                    {
                        strRETURN += "(" + strNomal + ")";
                    }
                    strRETURN += strExName + ":" + strResult + ",";
                }
            }

            //마지막 컴마 제거
            if (!strRETURN.IsNullOrEmpty())
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
            FnCNT1_1 = hicSpcPanjengService.GetCountbyWrtNo(FnWRTNO);

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

            //보건대행은 정상A,정상B도 인쇄 요청
            chb.READ_HIC_RES_BOHUM1(FnWRTNO);
            FnCNT2_1 = 0;
            if (!clsHcType.B1.ROWID.IsNullOrEmpty())
            {
                FnCNT2_1 = 1;
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
            nGunsokYY = VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3).To<long>();
            nGunsokMM = VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2).To<long>();
            nAge = list.AGE;
            strUNames = hm.UCode_Names_Display(list.UCODES);
            strMCode = list.UCODES;
            //strUNames_EDIT = cf.TextBox_2_MultiLine(strUNames, 13);
            strUNames_EDIT = strUNames;

            //특수테이블
            chb.READ_HIC_RES_SPECIAL(FnWRTNO);

            FnRow += 1;
            //공정명
            if (!clsHcType.B5.GONGJENG.IsNullOrEmpty())
            {
                strTemp1 = hb.READ_HIC_CODE("A2", clsHcType.B5.GONGJENG);    //공정명
                if (!strTemp1.IsNullOrEmpty())
                {
                    strGong = strTemp1;
                }
            }

            //취급물질명 및 인적사항을 Display
            //nSpcRow = VB.L(strUNames_EDIT, "{{@}}");
            //FnRow += 1;
            //if (FnRow + nSpcRow > SS1.ActiveSheet.RowCount)
            //{
            //    SS1.ActiveSheet.RowCount = (int)(FnRow + nSpcRow);
            //}

            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

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
            SS1.ActiveSheet.Cells[FnRow - 1, 11].Text = FnPano.ToString();
            if (SSCard != null)
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 12].Text = FstrGjYear;
                SS1.ActiveSheet.Cells[FnRow - 1, 13].Text = VB.Right(FstrJepDate,5);
            }

            //SS1.ActiveSheet.Cells[FnRow, 5].Text = VB.Pstr(strUNames_EDIT, "{{@}}", 1);

            //if (nSpcRow >= 2)
            //{
            //    for (int i = 1; i < nSpcRow; i++)
            //    {
            //        SS1.ActiveSheet.Cells[FnRow + i - 1, 5].Text = VB.Pstr(strUNames_EDIT, "{{@}}", i);
            //    }
            //}

            //생물학적 노출지표(참고치)
            if (!strMCode.IsNullOrEmpty())
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


                if(FnRow < SS1_Sheet1.RowCount)
                {
                    SS1_Sheet1.Cells.Get(FnRow - 1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    SS1_Sheet1.Cells.Get(FnRow - 1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                    SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = strResult2;

                }

                nSpcRowStart = FnRow;
                FnStartRow = FnRow;
                nSpcResCnt = 0;

                //특수 유소견판정 유무 확인(유소견이 있는 사람은 정상(A)를 보여주지 않음. 손대영 요청 - 10.10.15
                strTemp2 = "";
                List<HIC_SPC_PANJENG> list3 = hicSpcPanjengService.GetItembyWrtNoWrtNo2(FnWRTNO, FnWrtno2, "");
                if (list3.Count > 0)
                {
                    strTemp2 = "OK";
                }

                //특수 1,2차 판정 결과를 읽음
                List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GeResulttItembyWrtNoWrtNo2(FnWRTNO, FnWrtno2, "");

                nREAD = list2.Count;
                strOldData = "";
                strMCode = "";
                strResult2 = "";
                nRowCnt2 = 0;
                nRow = FnRow;

                for (int i = 0; i < nREAD; i++)
                {
                    //소견코드
                    if (list2[i].SOGENCODE != null)
                    {
                        strSogen = list2[i].SOGENCODE;
                    }
                    
                    strNewData = strSogen + "@" + list2[i].PANJENG;
                    strNewData = strNewData + "@" + list2[i].WORKYN;
                    strMCode = "'" + list2[i].MCODE + "',";

                    if (!strMCode.IsNullOrEmpty())
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
                        if (!strSogen.IsNullOrEmpty())
                        {
                            if (strSogen == "A04")
                            {
                                strResult = Read_Exam_Result("특수", "1102", "");
                            }
                            else if (strSogen == "A03")
                            {
                                strResult = Read_Exam_Result("특수", "1103", "");
                            }
                            else
                            {
                                strResult = Read_Exam_Result("특수", strSogen, "");
                            }
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
                //특수검진 1차,2차 모두 정상이면
                if (nSpcResCnt == 0)
                {
                    FnRow -= 1;
                }
                else if (FnRow <= (nSpcRowStart + nSpcRow - 1))
                {
                    FnRow = (int)(nSpcRowStart + nSpcRow - 1);
                }
            }
        }

        //가져와야함
        void fn_Screen_Special_Sub(string strPanjeng, string strSogenCode, string strJochiRemark, string strSahuCode, string strSogenRemark, string strWorkYn)
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
            switch (strPanjeng)
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
            FstrSogen = strSogenCode;
            FstrJochi = strJochiRemark;
            strSahu = hm.Sahu_Names_Display(strSahuCode);
            strJochi = strJochiRemark;
            //strJochi = cf.TextBox_2_MultiLine(strJochi, 38);
            strSogen = strSogenRemark;

            if (VB.InStr(strSogen, "야간작업") > 0)
            {
                if (VB.InStr(strSogen, "고혈압") > 0) strResult = strResult + Read_Exam_Result("일반", "03", "");
                if (VB.InStr(strSogen, "이상지질") > 0) strResult = strResult + Read_Exam_Result("일반", "04", "");
                if (VB.InStr(strSogen, "당뇨") > 0) strResult = strResult + Read_Exam_Result("일반", "06", "");
            }
            else if (VB.InStr(strSogen, "폐기능 이상") > 0)
            {
                if (strResult.IsNullOrEmpty())
                {
                    strResult += Read_Exam_Result("특수", "J", "");
                }
            }

            //마지막 컴마 제거
            if (!strResult.IsNullOrEmpty())
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            //if (strResult != "")
            //{
            //    //검사결과를 49 단위로 분리함
            //    strResult = cf.TextBox_2_MultiLine(strResult, 49);
            //}

            if (!strSogen.IsNullOrEmpty())
            {
                //strResult = strSogen + "{{@}}" + strResult;
                strResult = strSogen + "/" + strResult;
            }

            if (string.Compare(strPanjeng, "2") > 0)
            {
                //SS1.ActiveSheet.Cells[nRow, 8].Text = VB.Pstr(strResult, "{{@}}", 1);
                //SS1.ActiveSheet.Cells[nRow, 9].Text = VB.Pstr(strJochi, "{{@}}", 1);
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
                switch (strWorkYn)
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

            //-----( 결과,조치사항을 여러줄인 경우 분리하여 인쇄 )-----------
            //nJochiCnt = VB.L(strJochi, "{{@}}");
            //nResultCNT = VB.L(strResult, "{{@}}");

            //jj = 0;
            //if (nJochiCnt > 1)
            //{
            //    jj = (int)nJochiCnt;
            //}

            //if (nResultCNT > 1 && jj < nResultCNT)
            //{
            //    jj = (int)nResultCNT;
            //}

            //for (ii = 2; ii <= jj; ii++)
            //{
            //    FnRow += 1;
            //    if (FnRow > SS1.ActiveSheet.RowCount)
            //    {
            //        SS1.ActiveSheet.RowCount = FnRow;
            //    }

            //    if (ii <= nResultCNT)
            //    {
            //        SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = VB.Pstr(strResult, "{{@}}", ii);
            //    }

            //    if (ii <= nJochiCnt)
            //    {
            //        SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = VB.Pstr(strJochi, "{{@}}", ii);
            //    }
            //}
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

        void fn_Screen_First_Result(long nWrtNo)
        {
            //int nRow = 0;
            //string strSogen = "";
            //string strPanName = "";
            //string strGbPanBun2 = "";
            //string strResult = "";
            //string strOK = "";
            ////string strPanjeng = "";
            //string[] strPanjengC = new string[5];
            //string[] strPanjengD1 = new string[3];
            //string[] strPanjengD2 = new string[3];
            //string[] strPanjengU = new string[4];
            //string strFlag = ""; //생애한줄 작업
            //string strD2Flag = "";  //D2 대상자

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
            if (clsHcType.B1.ROWID.IsNullOrEmpty() || clsHcType.B1.PanDrNo == 0) return;

            //판정의사 명단 등록
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
                        break;
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
            if (clsHcType.B1.PANJENGU1 == "1") { strOK = "OK"; strPanjengU[0] = clsHcType.B1.PANJENGU1; } //고혈압
            if (clsHcType.B1.PANJENGU2 == "1") { strOK = "OK"; strPanjengU[1] = clsHcType.B1.PANJENGU2; } //당뇨
            if (clsHcType.B1.PANJENGU3 == "1") { strOK = "OK"; strPanjengU[2] = clsHcType.B1.PANJENGU3; } //이상지질
            if (clsHcType.B1.PANJENGU4 == "1") { strOK = "OK"; strPanjengU[3] = clsHcType.B1.PANJENGU4; } //폐결핵

            //2010 생애일경우 2차 안했으면 strok ="OK" 달아줌
            if (Fstr생애구분 == "생애" && clsHcType.B2.ROWID.IsNullOrEmpty() && strOK.IsNullOrEmpty())
            {
                strOK = "OK";
            }

            if (strOK != "OK")
            {
                return;
            }

            //인적사항을 읽음
            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                strSname = list.SNAME;
                if (list.SEX == "M")
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
                nGunsokYY = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
                nGunsokMM = long.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
                nAge = list.AGE;
            }

            //2010 생애부분추가
            //생애1차만 했을경우
            strFlag = "";
            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애")
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

            //2011-11-01 건진센터 한흥렬계장 요청사항(의뢰서)
            //D2 일반질병 판정이 있을경우 동일한 질환으로 질환의심R1 판정이 있어도 D2 질환만 표시함
            //질환의심(R1,R2)
            for (int i = 0; i <= 11; i++)
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
                        switch (i)
                        {
                            case 0:
                                strPanName = "폐결핵의심";
                                strPanjeng = "4";
                                break;
                            case 1:
                                strPanName = "기타흉부질환의심";
                                strPanjeng = "4";
                                break;
                            case 2:
                                strPanName = "고혈압";
                                strPanjeng = "5";
                                break;
                            case 3:
                                strPanName = "이상지질혈증의심";
                                strPanjeng = "4";
                                break;
                            case 4:
                                strPanName = "간장질환의심";
                                strPanjeng = "4";
                                break;
                            case 5:
                                strPanName = "당뇨병";
                                strPanjeng = "5";
                                break;
                            case 6:
                                strPanName = "신장질환의심";
                                strPanjeng = "4";
                                break;
                            case 7:
                                strPanName = "빈혈증의심";
                                strPanjeng = "4";
                                break;
                            case 8:
                                strPanName = "골다공증의심";
                                strPanjeng = "4";
                                break;
                            case 9:
                                strPanName = "기타질환의심";
                                strPanjeng = "4";
                                break;
                            case 10:
                                strPanName = "비만";
                                strPanjeng = "4";
                                break;
                            case 11:
                                strPanName = "난청";
                                strPanjeng = "4";
                                break;
                            default:
                                break;
                        }
                        strGbPanBun2 = string.Format("{0:00}", i + 1);
                        strResult += Read_Exam_Result("일반", strGbPanBun2, "");
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
                    strResult += Read_Exam_Result("D1D2", "31", strPanjengD1[i]);
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
                    strResult += Read_Exam_Result("D1D2", "33", strPanjengD2[i]);
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
                            strGbPanBun2 = "03";
                            break;
                        case 1:
                            strPanName = "유질환(당뇨병)";
                            strPanjeng = "8";
                            strGbPanBun2 = "06";
                            break;
                        case 2:
                            strPanName = "유질환(이상지질혈증)";
                            strPanjeng = "8";
                            strGbPanBun2 = "04";
                            break;
                        case 3:
                            strPanName = "유질환(폐결핵)";
                            strPanjeng = "8";
                            strGbPanBun2 = "01";
                            break;
                        default:
                            break;
                    }
                    strResult = "";
                    strResult += Read_Exam_Result("일반", strGbPanBun2, "");
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }            
        }

        /// <summary>
        /// 내용을 Display
        /// </summary>
        void fn_Screen_First_Sub(string argPanName, string argResult)
        {
            try
            {
                long nSogenCnt = 0;
                long nResultCNT = 0;
                long jj = 0;

                argPanName = argPanName + "\r\n";

                FstrChk = "Y";
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                }

                if (!strSname.IsNullOrEmpty())
                {
                    
                    SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = strGongjeng.IsNullOrEmpty() ? "기타" : strGongjeng;
                    SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
                    SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
                    SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
                    SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "▶일반건진";
                    SS1.ActiveSheet.Cells[FnRow - 1, 11].Text = FnPano.ToString();
                    if (SSCard != null)
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 12].Text = FstrGjYear;
                    }

                    FstrNameFlag = "OK";

                    strSogen = "";
                    if (Fstr생애구분 == "일반")
                    {
                        if (strPanjeng == "1" || strPanjeng == "2")
                        {
                            strSogen = "";
                            strSogen = "필요없음";
                        }
                        else
                        {
                            strSogen += clsHcType.B1.Sogen;
                        }
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
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A";
                        break;
                    case "3":
                    case "4":
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C";
                        break;
                    case "5":
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "확진검사대상";
                        break;
                    case "6":
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D1";
                        break;
                    case "7":
                    case "8":
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
                        break;
                    case "X":
                        SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "생애2차상담";
                        break;
                    default:
                        break;
                }

                //[기타질환의심]은 상세내용을 표시함(소장님)
                if (argPanName == "기타질환의심" && !clsHcType.B1.PanjengR_Etc.IsNullOrEmpty())
                {
                    argPanName += ":" + clsHcType.B1.PanjengR_Etc.Trim();
                }

                if (argResult.IsNullOrEmpty())
                {
                    argResult = argPanName;
                }
                else
                {
                    argResult = argPanName + "/" + argResult;
                }

                if (VB.Right(argResult, 1) == ",")
                {
                    argResult = VB.Mid(argResult, 1, argResult.Length - 1);
                }

                if (VB.Right(argResult, 2) == "\r\n")
                {
                    argResult = VB.Mid(argResult, 1, argResult.Length - 2);
                }

                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = argResult;
                SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strSogen;

                SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "가";
                if (string.Compare(strPanjeng, "2") > 0)
                {
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "나";
                }
                if (strPanjeng == "X")
                {
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
                }

                SetCard(FstrGjYear, argResult, SS1.ActiveSheet.Cells[FnRow - 1, 7].Text, SS1.ActiveSheet.Cells[FnRow - 1, 10].Text);

                strSname = "";
                nSogenCnt = 0;
                strSogen = "";
                strResult = "";
                strPanName = "";
                strPanjeng = "";

            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        void fn_Screen_Second_Sub()
        {
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
                SS1.ActiveSheet.Cells[FnRow - 1, 11].Text = FnPano.ToString();
            }

            SS1_Sheet1.Rows.Get(FnRow - 3).Border = new LineBorder(Color.Black, 1, false, true, false, false);

            strSogen = "";
            if (!clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 != "일반" && strFlag == "OK")
            {
                strSogen = "★2차생애수검 ";
                SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = "2차생애상담";
            }
            else
            {
                strSogen += clsHcType.B2.Sogen;
                if (strGbPanBun2 == "03")
                {
                    if (clsHcType.B2.Cycle_RES == "1") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A"; }
                    if (clsHcType.B2.Cycle_RES == "2") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C"; }
                    if (clsHcType.B2.Cycle_RES == "3") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2"; }
                }
                else
                {
                    if (clsHcType.B2.Diabetes_Res == "1") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A"; }
                    if (clsHcType.B2.Diabetes_Res == "2") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C"; }
                    if (clsHcType.B2.Diabetes_Res == "3") { SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2"; }
                }
            }

            switch (SS1.ActiveSheet.Cells[FnRow - 1, 7].Text.Trim())
            {
                case "C":
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "나";
                    break;
                case "2차생애상담":
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
                    break;
                default:
                    break;
            }

            strSname = "";
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
                if (strOK != "OK")
                {
                    FnDoctCnt += 1;
                    FnPanDrNo[FnDoctCnt - 1] = clsHcType.B2.PanjengDrNo;
                }
            }

            //인적사항을 읽음
            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                strSname = list.SNAME;
                if (list.SEX == "M")
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
                nGunsokYY = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
                nGunsokMM = long.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
                nAge = list.AGE;
            }

            //2010 생애2차만 했을경우
            strFlag = "";
            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && !clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애")
            {
                //strPanjeng = "X";
                strPanName = "2차생애수검";
                strResult = "";
                //strSogen = "★2차생애수검";
                //strResult = fn_Read_Living_Habit_Order(FnWRTNO);
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
           
            string strBangi = "";
            string strJob = "";
            long nBohumWRTNO = 0;

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 50;
            SS1_Sheet1.SetRowHeight(-1, 20);
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
            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeGjYearBangiJob(FstrFDate, FstrTDate, FstrLtdCode, "", strBangi, strJob, pano);

            nREAD = list.Count;
            nRow = 0;
            progressBar1.Maximum = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    clsHcType.B1.PanjengR[j] = "";
                }

                strResult = "";
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

                //--------------------------------------
                //  일반건진 결과를 표시함
                //--------------------------------------

                FstrChk = "";
                FnWrtno2 = 0;
                //2차 접수번호를 읽음
                if(FnPano == 242004)
                {
                    string x = "";
                }
                if(strGjYear == "")
                {
                    FnWrtno2 = hicJepsuService.GetWrtnoByPanoJepDateGjYear(FnPano, FstrJepDate, FstrGjYear, strBangi);
                }
                else
                {
                    FnWrtno2 = hicJepsuService.GetWrtnoByPanoJepDateGjYear(FnPano, FstrJepDate, strGjYear, strBangi);
                }
                

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
                //요기 부터 분석~~~~~~~~~~~
                //--------------------------------------
                //  특수 사후관리 유소견자가 있으면
                //--------------------------------------
                if (Special_Result_Exist() == "OK")
                {
                    nPrtCNT += 1;
                    fn_Screen_Special_Result();
                }
                
                FstrNameFlag = "";
                Fstr생애Flag = "";

                nPanjengDrno = 0;
                //특수판정 판정의사를 읽음(순서: 2차,1차)
                nPanjengDrno = hicJepsuService.GetPanjengDrNobyWrtNo(FnWrtno2);

                if (nPanjengDrno == 0)
                {
                    nPanjengDrno = hicJepsuService.GetPanjengDrNobyWrtNo(FnWRTNO);
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
            if(FnRow <  SS1_Sheet1.RowCount)
            {
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
                    if (FnRow >=SS1.ActiveSheet.RowCount)
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
            }
  

            //SS1_Sheet1.Columns.Get(8).Width = SS1.ActiveSheet.Cells[FnRow - 1, 8].Text.Length + 20;
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

                if (!rtnVal.IsNullOrEmpty())
                {
                    rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 1);
                    rtnVal += rtnVal + "에 관한 처방";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// READ_일반판정D2()
        /// </summary>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD2(int argGbn)
        {
            string strDPan = "";
            string rtnVal = "NO";

            strDPan = clsHcType.B1.PANJENGD21 + "@" + clsHcType.B1.PANJENGD22 + "@" + clsHcType.B1.PANJENGD23;

            switch (argGbn)
            {
                case 1:
                case 2:
                    rtnVal = VB.L(strDPan, "J") > 1 ? "OK" : "NO";
                    break;
                case 4:
                    rtnVal = VB.L(strDPan, "E") > 1 ? "OK" : "NO";
                    break;
                case 5:
                    rtnVal = VB.L(strDPan, "K") > 1 ? "OK" : "NO";
                    break;
                case 8:
                    rtnVal = VB.L(strDPan, "D") > 1 ? "OK" : "NO";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        private void SetCard(string year, string result, string pan, string job )
        {
            if (SSCard != null)
            {
                if (FstrGjYear.NotEmpty() && workerHealthCheckList.Count<=6)
                {
                    
                    //SSCard.ActiveSheet.Cells[SSCardRow, 3].Value = year;
                    //SSCard.ActiveSheet.Cells[SSCardRow, 4].Value = result;
                    //SSCard.ActiveSheet.Cells[SSCardRow, 28].Value = result;
                    //SSCard.ActiveSheet.Cells[SSCardRow, 37].Value = result;
                    //SSCard.ActiveSheet.Cells[SSCardRow, 4].Text = FstrGjYear;

                    if (workerHealthCheckList.ContainsKey(year))
                    {
                        workerHealthCheckList[year].SOGEN += pan + " " + result + "\n";

                    }
                    else
                    {
                        OSHA_HEALTH_CHECK_MODEL model = new OSHA_HEALTH_CHECK_MODEL();
                        model.YEAR = year;
                        model.SOGEN = pan +" "+result +"\n";
                        model.PAN = pan;
                        model.JOB = job;
                        workerHealthCheckList.Add(model.YEAR, model);
           //             SS1.ActiveSheet.Cells[FnRow - 1, 12].Text = year; // 검진년도
                    }

                   // SSCardRow++;

                }

            }
        }
    }
}
