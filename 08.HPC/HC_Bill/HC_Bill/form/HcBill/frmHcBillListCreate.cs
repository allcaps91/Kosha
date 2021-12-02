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
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillListCreate.cs
/// Description     : 건강검진 청구대상자 명단 작성[2020/생애포함]
/// Author          : 이상훈
/// Create Date     : 2020-12-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm청구명단작성_2020.frm(Frm청구명단작성_2020)" />

namespace HC_Bill
{
    public partial class frmHcBillListCreate : Form
    {
        HicJepsuService hicJepsuService = null;
        HicSunapService hicSunapService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicCodeService hicCodeService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResDentalService hicResDentalService = null;
        HicCancerNewService hicCancerNewService = null;
        HicMirBohumService hicMirBohumService = null;
        HicResultService hicResultService = null;
        HicMirDentalService hicMirDentalService = null;
        HicMirCancerService hicMirCancerService = null;
        HicMirCancerBoService hicMirCancerBoService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHaCodeHelp FrmHaCodeHelp = null;
        HEA_CODE CodeHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        PrintDocument pd;

        string FstrLtdName;
        bool boolSort = false;
        int FnRow;

        string FstrOldData = "";
        string FstrNewData = "";
        int[] nFCnt = new int[8];
        string FstrMinDate = "";
        string FstrMaxDate = "";

        int FnTemp;

        int FnTotCNT = 0;
        int FnCnt1 = 0;
        int FnCnt2 = 0; //전체건수,1차건수,2차건수
        int FnHuCnt = 0;

        string FstrInsFlag;

        public frmHcBillListCreate()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicSunapService = new HicSunapService();
            comHpcLibBService = new ComHpcLibBService();
            hicCodeService = new HicCodeService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResDentalService = new HicResDentalService();
            hicCancerNewService = new HicCancerNewService();
            hicMirBohumService = new HicMirBohumService();
            hicResultService = new HicResultService();
            hicMirDentalService = new HicMirDentalService();
            hicMirCancerService = new HicMirCancerService();
            hicMirCancerBoService = new HicMirCancerBoService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnBuild.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnPrintDtl.Click += new EventHandler(eBtnClick);
            this.btnKiho.Click += new EventHandler(eBtnClick);
            this.btnBogen.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBogen.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtKiho.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            int nYY = 0;
            string strSDate = "";

            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.SetRowHeight(-1, 24);
            sp.Spread_All_Clear(SS2);
            SS2_Sheet1.SetRowHeight(-1, 24);

            txtLtdCode.Text = "";
            txtKiho.Text = "";
            txtBogen.Text = "";

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();

            cboYear.Items.Clear();
            for (int i = 1; i <= 3; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;

            cboJohap.Items.Clear();
            cboJohap.Items.Add("사업장");
            cboJohap.Items.Add("공무원");
            cboJohap.Items.Add("성인병");
            cboJohap.Items.Add("통합");
            cboJohap.SelectedIndex = 0;

            cboJong.Items.Clear();
            cboJong.Items.Add("1.건강검진");
            cboJong.Items.Add("3.구강검진");
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;

            dtpFDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-50).ToShortDateString();
            dtpTDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-3).ToShortDateString();

            strSDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";

            DateTime DATE1 = Convert.ToDateTime(dtpFDate.Text);
            DateTime DATE2 = Convert.ToDateTime(strSDate);

            if (DATE1 < DATE2)
            {
                dtpFDate.Text = strSDate;
            }

            txtWrtNo.Text = "";
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
            else if (sender == btnSearch)
            {
                switch (VB.Left(cboJong.Text, 1))
                {
                    case "1":
                        fn_List_View_Bohum();   //건강검진
                        break;
                    case "2":
                        fn_List_View_Dental();  //구강검진
                        break;
                    case "3":
                        fn_List_View_Cancer();  //암검진,의료급여암
                        break;
                    default:
                        break;
                }

                fn_SS_Sort();
            }            
            else if (sender == btnBuild)
            {
                long nCnt = 0;
                long nCnt1 = 0;
                long nCnt2 = 0;
                long nCnt3 = 0;
                long nCNT4 = 0;
                long nCnt5 = 0;
                string strJong = "";
                string strChk = "";
                string strLtdCode = "";
                string strFDate = "";
                string strTDate = "";
                string sMsg = "";

                strJong = VB.Left(cboJong.Text, 1);

                //청구명단 대상 회사를 선택하였는지 점검
                nCnt = 0;
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text.Trim() == "True")
                    {
                        nCnt += 1;
                        nCnt1 = SS1.ActiveSheet.Cells[i, 5].Text.To<long>();
                        nCnt1 = SS1.ActiveSheet.Cells[i, 6].Text.To<long>();
                        nCnt1 = SS1.ActiveSheet.Cells[i, 7].Text.To<long>();
                        nCnt1 = SS1.ActiveSheet.Cells[i, 8].Text.To<long>();
                        if (strJong == "1" && nCnt2 == 0)
                        {
                            MessageBox.Show(i + 1 + "번줄 1차, 2차 청구대상 건수가 없는 회사를 선택함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            SS1_Sheet1.SetActiveCell(i, 1);
                            return;
                        }
                        if (nCnt3 > 0 && nCNT4 > 0)
                        {
                            MessageBox.Show(i + 1 + "번줄 미판정 검진자가 있는 회사를 선택함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            SS1_Sheet1.SetActiveCell(i, 1);
                            return;
                        }
                    }
                }

                if (nCnt == 0)
                {
                    MessageBox.Show("회사를 선택하지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                sMsg = "건강검진 청구 작업을 정말로 하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //청구대상 회사별로 자료 형성
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strLtdCode = SS1.ActiveSheet.Cells[i, 1].Text;
                    strFDate = SS1.ActiveSheet.Cells[i, 3].Text;
                    strTDate = SS1.ActiveSheet.Cells[i, 4].Text;
                    nCnt5 = SS1.ActiveSheet.Cells[i, 10].Text.To<long>();
                    if (strChk == "True")
                    {
                        switch (strJong)
                        {
                            case "1":
                                fn_Build_Bohum(strLtdCode, strFDate, strTDate, nCnt5);      //건강검진
                                break;
                            case "3":
                                fn_Build_Dental(strLtdCode, strFDate, strTDate, nCnt5);     //구강검사
                                break;
                            case "4":
                            case "E":
                                fn_Build_Cancer(strLtdCode, strFDate, strTDate, nCnt5);     //암검진,의료보험암
                                break;
                            default:
                                break;
                        }
                    }
                }

                MessageBox.Show("정상적으로 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                eBtnClick(btnSearch, new EventArgs());

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

                strTitle = "건강보험 청구대상자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrintDtl)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강보험 청구대상자 상세내역";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("회 사 명:" + FstrLtdName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnKiho)
            {
                Hic_Code_Help("18", txtKiho);
            }
            else if (sender == btnBogen)
            {
                Hic_Code_Help("25", txtBogen);
            }
        }

        private void Hic_Code_Help(string strGB, TextBox tx)
        {
            string strFind = "";

            if (tx.Text.Contains("."))
            {
                strFind = VB.Pstr(tx.Text, ".", 2).Trim();
            }
            else
            {
                strFind = tx.Text.Trim();
            }

            FrmHaCodeHelp = new frmHaCodeHelp(strGB, strFind);
            FrmHaCodeHelp.rSetGstrValue += new frmHaCodeHelp.SetGstrValue(ePost_value_CODE);
            FrmHaCodeHelp.ShowDialog();

            if (!CodeHelpItem.CODE.IsNullOrEmpty() && !CodeHelpItem.IsNullOrEmpty())
            {
                tx.Text = CodeHelpItem.CODE.Trim() + "." + CodeHelpItem.NAME.Trim();
            }
            else
            {
                if (VB.Pstr(tx.Text, ".", 1).Trim() == "") { tx.Text = ""; }
            }
        }

        private void ePost_value_CODE(HEA_CODE item)
        {
            CodeHelpItem = item;
        }

        /// <summary>
        /// ---------( 사업장,공무원,성인병 1차,2차검진대상자 검색)-----------------
        /// </summary>
        void fn_List_View_Bohum()
        {
            int nREAD = 0;
            int nRow = 0;
            string strJohap = "";            
            string strDate = "";
            string strDate1 = "";
            string strFlag = "";
            string strOK = "";
            //string str검진종류세팅 = "";
            List<string> str검진종류세팅 = new List<string>();

            //검진종류 세팅 모듈에 사용 ----------------
            string strJohap2 = "";
            string strChasu2 = "";
            string strLife2 = "";
            string strLtdCode = "";
            string strBogunso = "";
            string strLife66 = "";
            string strLife2018 = "";

            strJohap2 = cboJohap.Text.Trim();

            if (rdoChasu0.Checked == true)
            {
                strChasu2 = "0";
            }
            else if (rdoChasu1.Checked == true)
            {
                strChasu2 = "1";
            }
            else if (rdoChasu2.Checked == true)
            {
                strChasu2 = "2";
            }

            strLife2 = "0";

            if (chkLife.Checked == true)
            {
                strLife2 = "1";
            }

            strFlag = "Y";

            for (int i = 0; i <= 7; i++)
            {
                nFCnt[i] = 0;
            }

            if (txtLtdCode.Text.Trim() != "")
            {
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
            }
            else
            {
                strLtdCode = "";
            }

            if (txtBogen.Text.Trim() != "")
            {
                strBogunso = VB.Pstr(txtBogen.Text, ".", 1);
            }
            else
            {
                strBogunso = "";
            }

            if (chkLife66.Checked == true)
            {
                strLife66 = "1";
            }
            else
            {
                strLife66 = "";
            }

            if (chkLife2018.Checked == true)
            {
                strLife2018 = "1";
            }
            else
            {
                strLife2018 = "";

            }
            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);

            strJohap = cboJohap.Text.Trim();

            str검진종류세팅 = chb.READ_HIC_HcKindSetting_List(strJohap2, strChasu2, strLife2);

            List<HIC_JEPSU> list = hicJepsuService.GetExpenseItembyJepDateGjYear(dtpFDate.Text, dtpTDate.Text, cboYear.Text, txtWrtNo.Text.To<long>(), strLtdCode, strBogunso, str검진종류세팅, strLife66, strLife2018, strJohap2);

            nREAD = list.Count;
            FstrOldData = "";
            for (int j = 0; j <= 7; j++)
            {
                nFCnt[j] = 0;
            }
            FstrMinDate = "";
            FstrMaxDate = "";
            FnRow = 0;
            for (int i = 0; i < nREAD; i++)
            {
                //2012년도 자격조회부터 11자리로 바뀜
                FnRow += 1;
                if (FnRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = FnRow;
                }

                SS2.ActiveSheet.Cells[FnRow - 1, 0].Text = list[i].JEPDATE;
                SS2.ActiveSheet.Cells[FnRow - 1, 1].Text = list[i].WRTNO.To<string>();
                SS2.ActiveSheet.Cells[FnRow - 1, 2].Text = list[i].SNAME;
                SS2.ActiveSheet.Cells[FnRow - 1, 3].Text = list[i].AGE + "/" + list[i].SEX;

                SS2.ActiveSheet.Cells[FnRow - 1, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                SS2.ActiveSheet.Cells[FnRow - 1, 5].Text = list[i].GJCHASU;
                SS2.ActiveSheet.Cells[FnRow - 1, 9].Text = hm.SExam_Names_Display(list[i].SEXAMS);
                SS2.ActiveSheet.Cells[FnRow - 1, 10].Text = hm.UCode_Names_Display(list[i].UCODES);
                SS2.ActiveSheet.Cells[FnRow - 1, 11].Text = list[i].KIHO;
                SS2.ActiveSheet.Cells[FnRow - 1, 12].Text = list[i].BOGUNSO;
            }

            if (strFlag == "N")
            {
                MessageBox.Show("증번호 오류건 발생하였습니다. 수정후 작업하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < nREAD; i++)
            {
                //조합부담액이 있으면 청구함
                HIC_SUNAP list2 = hicSunapService.GetJohapAmtBogenAmtbyWrtNo(list[i].WRTNO);

                strOK = "OK";
                if (list2.JOHAPAMT == 0)
                {
                    strOK = "";
                }

                if (strOK == "OK")
                {
                    if (strJohap2 != "성인병")
                    {
                        FstrNewData = strJohap2;
                    }
                    else
                    {
                        FstrNewData = list[i].JOHAP.To<string>();
                    }

                    if (FstrOldData.IsNullOrEmpty())
                    {
                        FstrOldData = FstrNewData;
                    }
                    if (FstrOldData != FstrNewData)
                    {
                        //GoSub List_View_Bohum_SUB
                        fn_List_View_Bohum_SUB(strJohap2, nFCnt[1], nFCnt[2], nFCnt[3], nFCnt[4], nFCnt[5], nFCnt[7]);
                    }

                    //판정여부(결과미입력도 미판정으로 처리)
                    HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetPanjengDatebyWrtno(list[i].WRTNO, list[i].GJCHASU);

                    if (!list3.IsNullOrEmpty())
                    {
                        if (!list3.PANJENGDATE.IsNullOrEmpty() && list[i].GBSTS == "2")
                        {
                            //회사별 인원 및 금액을 누적(판정)
                            if (list[i].GJCHASU == "1")
                            {
                                nFCnt[1] += 1;  //1차인원
                            }
                            else
                            {
                                nFCnt[2] += 1;  //2차인원
                            }
                        }
                        else
                        {
                            //회사별 인원 및 금액을 누적(미판정)
                            if (list[i].GJCHASU == "1")
                            {
                                nFCnt[3] += 1;  //1차인원
                            }
                            else
                            {
                                nFCnt[4] += 1;  //2차인원
                            }
                        }
                    }
                    else
                    {
                        //회사별 인원 및 금액을 누적(미판정)
                        if (list[i].GJCHASU == "1")
                        {
                            nFCnt[3] += 1;  //1차인원
                        }
                        else
                        {
                            nFCnt[4] += 1;  //2차인원
                        }
                    }

                    //2차미실시
                    if (list[i].SECOND_FLAG == "Y")
                    {
                        if (list[i].SECOND_DATE.IsNullOrEmpty())
                        {
                            nFCnt[5] += 1;
                        }
                    }

                    //생애체크
                    switch (list[i].GJJONG)
                    {
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "35":
                            nFCnt[7] += 1;
                            break;
                        default:
                            break;
                    }

                    //검진시작일,종료일
                    strDate = list[i].JEPDATE;
                    if (FstrMinDate.IsNullOrEmpty())
                    {
                        FstrMinDate = strDate;
                    }
                    if (string.Compare(strDate, FstrMinDate) < 0)
                    {
                        FstrMinDate = strDate;
                    }
                    if (FstrMaxDate.IsNullOrEmpty())
                    {
                        FstrMaxDate = strDate;
                    }
                    if (string.Compare(strDate, FstrMaxDate) > 0)
                    {
                        FstrMaxDate = strDate;
                    }
                }
            }

            //GoSub List_View_Bohum_SUB
            fn_List_View_Bohum_SUB(strJohap2, nFCnt[1], nFCnt[2], nFCnt[3], nFCnt[4], nFCnt[5], nFCnt[7]);
            SS1.ActiveSheet.RowCount = FnRow;
        }

        /// <summary>
        ///  //List_View_Bohum_SUB
        /// </summary>
        /// <param name="strJohap2"></param>
        /// <param name="nCnt1"></param>
        /// <param name="nCnt2"></param>
        /// <param name="nCnt3"></param>
        /// <param name="nCnt4"></param>
        /// <param name="nCnt5"></param>
        /// <param name="nCnt7"></param>
        void fn_List_View_Bohum_SUB(string strJohap2, long nCnt1, long nCnt2, long nCnt3, long nCnt4, long nCnt5, long nCnt7)
        {
            if (nCnt1 > 0 || nCnt2 > 0 || nCnt3 > 0 || nCnt4 > 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = FnRow;
                    if (strJohap2 == "사업장" || strJohap2 == "공무원")
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = "0000";
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = " " + strJohap2;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = FstrOldData;
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = " " + hb.READ_Ltd_Name(FstrOldData);
                    }
                    SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = FstrMinDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = FstrMaxDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = nCnt1.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = nCnt2.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = nCnt3.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = nCnt4.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = nCnt5.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = nCnt7.To<string>();
                }

                FstrOldData = FstrNewData;
                FstrMinDate = "";
                FstrMaxDate = "";
                for (int k = 0; k <= 7; k++)
                {
                    nFCnt[k] = 0;
                }
            }
        }

        /// <summary>
        /// ---------( 사업장,공무원,성인병 구강검사 검색)-----------------
        /// </summary>
        void fn_List_View_Dental()
        {
            int nREAD = 0;
            string strJohap = "";
            string strDate = "";
            string strFlag = "";
            //string str검진종류세팅 = "";
            List<string> str검진종류세팅 = new List<string>();

            //검진종류 세팅 모듈에 사용 ----------------
            string strJohap2 = "";
            string strChasu2 = "";
            string strLife2 = "";
            string strLtdCode = "";
            string strBogunso = "";

            strJohap2 = cboJohap.Text.Trim();

            if (rdoChasu0.Checked == true)
            {
                strChasu2 = "0";
            }
            else if (rdoChasu1.Checked == true)
            {
                strChasu2 = "1";
            }
            else if (rdoChasu2.Checked == true)
            {
                strChasu2 = "2";
            }

            strLife2 = "0";

            if (chkLife.Checked == true)
            {
                strLife2 = "1";
            }

            strFlag = "Y";

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);

            strJohap = cboJohap.Text;

            if (strJohap == "통합")
            {
                MessageBox.Show("구강은 지역별로 명단 작성하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i <= 7; i++)
            {
                nFCnt[i] = 0;
            }

            if (txtLtdCode.Text.Trim() != "")
            {
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
            }
            else
            {
                strLtdCode = "";
            }

            if (txtBogen.Text.Trim() != "")
            {
                strBogunso = VB.Pstr(txtBogen.Text, ".", 1);
            }
            else
            {
                strBogunso = "";
            }

            str검진종류세팅 = chb.READ_HIC_HcKindSetting_List(strJohap2, strChasu2, strLife2);

            List<HIC_JEPSU> list = hicJepsuService.GetExpenseItembyJepDateGjYear_Dental(dtpFDate.Text, dtpTDate.Text, cboYear.Text, txtWrtNo.Text.To<long>(), strLtdCode, strBogunso, str검진종류세팅, strJohap2);

            nREAD = list.Count;
            FstrOldData = "";
            for (int j = 0; j <= 7; j++)
            {
                nFCnt[j] = 0;
            }
            FstrMinDate = "";
            FstrMaxDate = "";
            FnRow = 0;
            for (int i = 0; i < nREAD; i++)
            {
                FnRow += 1;
                if (FnRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = FnRow;
                }

                SS2.ActiveSheet.Cells[FnRow - 1, 0].Text = list[i].JEPDATE;
                SS2.ActiveSheet.Cells[FnRow - 1, 1].Text = list[i].WRTNO.To<string>();
                SS2.ActiveSheet.Cells[FnRow - 1, 2].Text = list[i].SNAME;
                SS2.ActiveSheet.Cells[FnRow - 1, 3].Text = list[i].AGE + "/" + list[i].SEX;

                SS2.ActiveSheet.Cells[FnRow - 1, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                SS2.ActiveSheet.Cells[FnRow - 1, 5].Text = list[i].GJCHASU;
                SS2.ActiveSheet.Cells[FnRow - 1, 9].Text = "";
                if (!list[i].SEXAMS.IsNullOrEmpty()) { SS2.ActiveSheet.Cells[FnRow - 1, 9].Text = hm.SExam_Names_Display(list[i].SEXAMS); }
                SS2.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
                if (!list[i].UCODES.IsNullOrEmpty()) { SS2.ActiveSheet.Cells[FnRow - 1, 10].Text = hm.UCode_Names_Display(list[i].UCODES); }
                
                SS2.ActiveSheet.Cells[FnRow - 1, 11].Text = list[i].KIHO;
                SS2.ActiveSheet.Cells[FnRow - 1, 12].Text = list[i].BOGUNSO;
            }

            if (strFlag == "N")
            {
                MessageBox.Show("증번호 오류건 발생하였습니다. 수정후 작업하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < nREAD; i++)
            {   
                if (strJohap2 != "성인병")
                {
                    FstrNewData = strJohap2;
                }
                else
                {
                    FstrNewData = list[i].JOHAP.To<string>();
                }

                if (FstrOldData.IsNullOrEmpty())
                {
                    FstrOldData = FstrNewData;
                }
                if (FstrOldData != FstrNewData)
                {
                    //GoSub List_View_Dental_SUB
                    fn_List_View_Dental_SUB(strJohap2, nFCnt[1], nFCnt[2], nFCnt[3], nFCnt[4], nFCnt[5], nFCnt[7]);
                }

                //판정여부(결과미입력도 미판정으로 처리)
                //구강문진이 없으면 미판정으로 처리
                HIC_RES_DENTAL list3 = hicResDentalService.GetItemByWrtno(list[i].WRTNO);

                if (!list3.IsNullOrEmpty())
                {
                    if (list3.OPDDNT.IsNullOrEmpty() || list3.DNTSTATUS.IsNullOrEmpty())
                    {
                        //회사별 인원 누적(미판정)
                        nFCnt[3] += 1;  //1차인원
                    }
                    else if (!list3.PANJENGDATE.IsNullOrEmpty() && !list3.TPANJENGCHK.IsNullOrEmpty())
                    {
                        //회사별 인원 누적(판정)
                        nFCnt[1] += 1;  //1차인원                        
                    }
                    else 
                    {
                        //회사별 인원 누적(미판정)
                        nFCnt[3] += 1;  //1차인원                        
                    }
                }                

                //생애체크
                switch (list[i].GJJONG)
                {
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "35":
                        nFCnt[7] += 1;
                        break;
                    default:
                        break;
                }

                //검진시작일,종료일
                strDate = list[i].JEPDATE;
                if (FstrMinDate.IsNullOrEmpty())
                {
                    FstrMinDate = strDate;
                }
                if (string.Compare(strDate, FstrMinDate) < 0)
                {
                    FstrMinDate = strDate;
                }
                if (FstrMaxDate.IsNullOrEmpty())
                {
                    FstrMaxDate = strDate;
                }
                if (string.Compare(strDate, FstrMaxDate) > 0)
                {
                    FstrMaxDate = strDate;
                }
            }

            //GoSub List_View_Dental_SUB
            fn_List_View_Dental_SUB(strJohap2, nFCnt[1], nFCnt[2], nFCnt[3], nFCnt[4], nFCnt[5], nFCnt[7]);
            SS1.ActiveSheet.RowCount = FnRow;
        }

        void fn_List_View_Dental_SUB(string strJohap2, long nCnt1, long nCnt2, long nCnt3, long nCnt4, long nCnt5, long nCnt7)
        {
            if (nCnt1 > 0 || nCnt2 > 0 || nCnt3 > 0 || nCnt4 > 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    if (FnRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = FnRow;
                    }

                    if (strJohap2 != "성인병")
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = "0000";
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = " " + strJohap2;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = FstrOldData;
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = " " + hb.READ_Ltd_Name(FstrOldData);
                    }
                    SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = FstrMinDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = FstrMaxDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = nCnt1.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = nCnt2.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = nCnt3.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = nCnt4.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = nCnt5.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = nCnt7.To<string>();
                }

                FstrOldData = FstrNewData;
                FstrMinDate = "";
                FstrMaxDate = "";
                for (int k = 0; k <= 7; k++)
                {
                    nFCnt[k] = 0;
                }
            }
        }

        /// <summary>
        /// ---------( 암검진 청구 대상자 검색)-----------------
        /// </summary>
        void fn_List_View_Cancer()
        {
            int nREAD = 0;
            string strJohap = "";
            string strDate = "";
            string strFlag = "";
            string strFDate = "";
            string strTDate = "";

            //검진종류 세팅 모듈에 사용 ----------------
            string strJohap2 = "";
            string strChasu2 = "";
            string strLife2 = "";
            string strLtdCode = "";
            string strBogunso = "";

            string strLife = "";
            string strGubun = "";
            string strYear = "";
            string strJong = "";
            string strKiho = "";
            string strW_Am = "";
            long nWrtNo = 0;

            strFlag = "Y";

            strJohap = cboJohap.Text.Trim();

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);

            if (VB.Left(cboJong.Text, 1) == "4")
            {
                MessageBox.Show("공단암일 경우 보건소기호를 확인후 청구명단을 작성하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (chkLife.Checked == true)
            {
                strLife = "1";
            }
            else
            {
                strLife = "";
            }

            if (chkGubun1.Checked == true)
            {
                strGubun = "1";

            }
            else
            {
                strGubun = "";
            }

            for (int i = 0; i <= 7; i++)
            {
                nFCnt[i] = 0;
            }

            if (txtLtdCode.Text.Trim() != "")
            {
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
            }
            else
            {
                strLtdCode = "";
            }

            strYear = cboYear.Text;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            if (txtBogen.Text.Trim() != "")
            {
                strBogunso = VB.Pstr(txtBogen.Text, ".", 1);
            }
            else
            {
                strBogunso = "";
            }

            if (chkW_Am.Checked == true)
            {
                strW_Am = "1";
            }
            else
            {
                strW_Am = "";
            }

            nWrtNo = txtWrtNo.Text.To<long>();

            strKiho = txtKiho.Text.Trim();

            strJong = VB.Left(cboJong.Text, 1);

            List<HIC_JEPSU> list = hicJepsuService.GetCancerItembyJepDateGjYear(strGubun, strFDate, strTDate, strLife, strKiho, strW_Am, strYear, nWrtNo, strLtdCode, strBogunso, strJohap, strJong);

            nREAD = list.Count;
            FstrOldData = "";
            for (int j = 0; j <= 7; j++)
            {
                nFCnt[j] = 0;
            }
            FstrMinDate = "";
            FstrMaxDate = "";
            
            FnRow = 0;
            //증번호 체크
            for (int i = 0; i < nREAD; i++)
            {
                FnRow += 1;
                if (FnRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = FnRow;
                }

                SS2.ActiveSheet.Cells[FnRow - 1, 0].Text = list[i].JEPDATE;
                SS2.ActiveSheet.Cells[FnRow - 1, 1].Text = list[i].WRTNO.To<string>();
                SS2.ActiveSheet.Cells[FnRow - 1, 2].Text = list[i].SNAME;
                SS2.ActiveSheet.Cells[FnRow - 1, 3].Text = list[i].AGE + "/" + list[i].SEX;
                SS2.ActiveSheet.Cells[FnRow - 1, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                SS2.ActiveSheet.Cells[FnRow - 1, 5].Text = list[i].GJCHASU;
                SS2.ActiveSheet.Cells[FnRow - 1, 9].Text = "";
                if (!list[i].SEXAMS.IsNullOrEmpty()) { SS2.ActiveSheet.Cells[FnRow - 1, 9].Text = hm.SExam_Names_Display(list[i].SEXAMS); }
                SS2.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
                if (!list[i].UCODES.IsNullOrEmpty()) { SS2.ActiveSheet.Cells[FnRow - 1, 10].Text = hm.UCode_Names_Display(list[i].UCODES); }
                SS2.ActiveSheet.Cells[FnRow - 1, 11].Text = list[i].KIHO;
                SS2.ActiveSheet.Cells[FnRow - 1, 12].Text = list[i].BOGUNSO;
            }

            if (VB.Left(cboJohap.Text, 1) != "E")
            {
                if (strFlag == "N")
                {
                    MessageBox.Show("증번호 오류건 발생하였습니다. 수정후 작업하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                FnRow = 0;
            }

            for (int i = 0; i < nREAD; i++)
            {
                //판정여부(결과미입력도 미판정으로 처리) 건진일자를 판정일자로
                HIC_CANCER_NEW list3 = hicCancerNewService.GetGunDatebyWrtNo(list[i].WRTNO);

                if (!list3.IsNullOrEmpty())
                {
                    if (!list3.GUNDATE.IsNullOrEmpty() || list[i].GBSTS == "2")
                    {
                        //인원 및 금액을 누적(판정)
                        nFCnt[1] += 1;  //1차인원
                    }
                    else
                    {
                        //인원 및 금액을 누적(미판정)
                        nFCnt[3] += 1;  //1차인원미실시                   
                    }
                }
                else
                {
                    nFCnt[3] += 1;  //1차인원미실시                  
                }

                //생애체크
                switch (list[i].GJJONG)
                {
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "35":
                        nFCnt[7] += 1;
                        break;
                    default:
                        break;
                }

                //검진시작일,종료일
                strDate = list[i].JEPDATE;
                if (FstrMinDate.IsNullOrEmpty())
                {
                    FstrMinDate = strDate;
                }
                if (string.Compare(strDate, FstrMinDate) < 0)
                {
                    FstrMinDate = strDate;
                }
                if (FstrMaxDate.IsNullOrEmpty())
                {
                    FstrMaxDate = strDate;
                }
                if (string.Compare(strDate, FstrMaxDate) > 0)
                {
                    FstrMaxDate = strDate;
                }
            }

            //GoSub List_View_Cancer_SUB
            fn_List_View_Cancer_SUB(strJohap2, nFCnt[1], nFCnt[2], nFCnt[3], nFCnt[4], nFCnt[5], nFCnt[7]);
            SS1.ActiveSheet.RowCount = FnRow;
        }

        void fn_List_View_Cancer_SUB(string strJohap2, long nCnt1, long nCnt2, long nCnt3, long nCnt4, long nCnt5, long nCnt7)
        {
            if (nCnt1 > 0 || nCnt2 > 0 || nCnt3 > 0 || nCnt4 > 0)
            {
                FnRow += 1;
                if (FnRow > SS1.ActiveSheet.RowCount)
                {
                    if (FnRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = FnRow;
                    }

                    if (txtLtdCode.Text.Trim() != "")
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = VB.Pstr(txtLtdCode.Text, ".", 1);
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = "";
                    }

                    if (VB.Left(cboJong.Text, 1) != "E")
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = "암검진";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = "의료급여암";
                    }
                    SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = FstrMinDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = FstrMaxDate;
                    SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = nCnt1.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 6].Text = nCnt2.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = nCnt3.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = nCnt4.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = nCnt5.To<string>();
                    SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = nCnt7.To<string>();
                }

                FstrOldData = FstrNewData;
                FstrMinDate = "";
                FstrMaxDate = "";
                for (int k = 0; k <= 7; k++)
                {
                    nFCnt[k] = 0;
                }
            }
        }

        void fn_SS_Sort()
        {
            #region 강제 Sort
            FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[] {
               new FarPoint.Win.Spread.SortInfo(2,true)};

            SS1_Sheet1.SortRows(0, SS1_Sheet1.RowCount, si);
            #endregion
        }

        /// <summary>
        /// 회사별 건강보험 청구대상자 확정
        /// </summary>
        /// <param name="strLtdCode"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="nCnt5"></param>
        void fn_Build_Bohum(string argLtdCode, string argFDate, string argTDate, long argCnt)
        {
            int nREAD = 0;
            int nChasu = 0;
            //int nTotCNT = 0;
            //int nCnt1 = 0;
            //int nCnt2 = 0; //전체건수,1차건수,2차건수
            string strPandate = "";
            string strkiho = "";
            long nMirno = 0;
            long nWRTNO = 0;
            string strChasu = "";           //2차 단독청구시 Y
            string strPanoNew = "";
            string strPanoOld = "";
            string strFalg = "";
            string strMirGbn = "";          //추가청구구분
            string strOK = "";
            string strLife_Gbn = "";    //생애구분
            List<string> str검진종류세팅 = new List<string>();
            string strGbJohap = "";         //조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
            //검진종류 세팅 모듈에 사용 ----------------
            string strJohap2 = "";
            string strChasu2 = "";
            string strLife2 = "";
            string strBogenso = "";
            string strYear = "";
            string strKiho = "";

            int result = 0;

            FnTotCNT = 0;
            FnCnt1 = 0;
            FnCnt2 = 0;

            FnTemp = 0;

            strJohap2 = cboJohap.Text;

            if (rdoChasu0.Checked == true)
            {
                strChasu2 = "0";
            }
            else if (rdoChasu1.Checked == true)
            {
                strChasu2 = "1";
            }
            else if (rdoChasu2.Checked == true)
            {
                strChasu2 = "2";
            }

            strLife2 = "0";

            if (chkLife.Checked == true)
            {
                strLife2 = "1";
            }

            strLife_Gbn = "";
            if (argCnt > 0)
            {
                strLife_Gbn = "Y";
            }

            strMirGbn = "N";
            if (rdoMirGbn1.Checked == true)
            {
                strMirGbn = "Y";
            }
            strChasu = "N";
            FnTemp = 0;

            nWRTNO = txtWrtNo.Text.To<long>();
            strBogenso = txtBogen.Text.Trim();
            strYear = cboYear.Text;
            strKiho = VB.Pstr(txtKiho.Text, ".", 1);

            clsDB.setBeginTran(clsDB.DbCon);

            //조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
            switch (argLtdCode)
            {
                case "지역":
                    strGbJohap = "1";
                    break;
                case "공교":
                    strGbJohap = "2";
                    break;
                case "직장":
                    strGbJohap = "3";
                    break;
                case "급여":
                    strGbJohap = "4";
                    break;
                default:
                    strGbJohap = "0";
                    break;
            }

            nMirno = fn_READ_New_HicMirNo();
            str검진종류세팅 = chb.READ_HIC_HcKindSetting_List(strJohap2, strChasu2, strLife2);

            switch (strJohap2)
            {
                case "사업장":
                case "공무원":
                case "성인병":
                case "통합":
                    if (strChasu2 == "2")
                    {
                        strChasu = "Y";
                    }
                    break;
                default:
                    break;
            }

            //해당 회사의 청구 대상자를 읽음
            List<HIC_JEPSU> list = hicJepsuService.GetBohumItembyJepDateGjYear(argFDate, argTDate, strYear, str검진종류세팅, nWRTNO, argLtdCode, strBogenso, strJohap2, strKiho);

            nREAD = list.Count;
            if (argLtdCode == "급여" && nREAD > 1)
            {
                if (MessageBox.Show("생애 성인병 급여명단 작성시 - 보건소별로 청구 명단을 작성해야합니다.." + "\r\n\r\n" + "보건소 별로 명단을 만들었다면 그대로 진행해도 됩니다" + "\r\n\r\n" + "이대로 진행하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            FnTotCNT = 0;
            FnCnt1 = 0;
            FnCnt2 = 0;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                //조합부담액이 있으면 청구함
                HIC_SUNAP list2 = hicSunapService.GetJohapAmtBogenAmtbyWrtNo(list[i].WRTNO);

                strOK = "OK";
                if (!list2.IsNullOrEmpty())
                {
                    if (list2.JOHAPAMT == 0)
                    {
                        strOK = "";
                    }
                }

                if (strOK == "OK")
                {
                    //판정여부(결과미입력도 미판정으로 처리)
                    HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetPanjengDatebyWrtNo(list[i].WRTNO);

                    if (list3.IsNullOrEmpty())
                    {
                        strPandate = "비대상";
                    }
                    else
                    {
                        strPandate = list3.PANJENGDATE;
                    }

                    if (list[i].GJCHASU == "2" && strPandate.IsNullOrEmpty())
                    {
                        FnTemp += 1;
                    }

                    //---------------------------------------------
                    //      판정을 완료하였으면 청구대상임
                    //---------------------------------------------
                    if (!strPandate.IsNullOrEmpty())
                    {
                        nWRTNO = list[i].WRTNO;
                        nChasu = list[i].GJCHASU.To<int>();
                        strkiho = list[i].KIHO;

                        //1차,2차건수를 누적
                        if (nChasu == 1)
                        {
                            FnCnt1 += 1; //1차건수
                        }
                        else
                        {
                            FnCnt2 += 1;//2차건수
                        }

                        FnTotCNT += 1;

                        //접수마스타에 청구형성 완료 SET
                        result = hicJepsuService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "1");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_JEPSU에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //수납마스타에 청구형성 완료 SET
                        result = hicSunapService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "1");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_SUNAP에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (FnTotCNT > 0 && FnTotCNT >= 100)
                        {
                            if (nChasu == 1)
                            {
                                if (list[i].PANO == list[i + 1].PANO)
                                {
                                    FnTotCNT -= 1;
                                    FnCnt1 -= 1;
                                    //GoSub Build_Bohum_INSERT
                                    fn_Build_Bohum_INSERT(nMirno, cboYear.Text, cboJohap.Text, strGbJohap, argFDate, argTDate, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, strkiho);
                                    if (FstrInsFlag == "NO")
                                    {
                                        return;
                                    }
                                    nMirno = fn_READ_New_HicMirNo(); //새로운 미수번호
                                    i -= 1;
                                    FnTotCNT = 0;
                                    FnCnt1 = 0;
                                    FnCnt2 = 0;
                                }
                                else if (list[i].PANO != list[i + 1].PANO)
                                {
                                    //새로운 건강검진 비용청구서 INSERT(최대 100건씩 청구 가능)
                                    if (!list[i].PANO.IsNullOrEmpty() && list[i].PANO != 0)
                                    {
                                        fn_Build_Bohum_INSERT(nMirno, cboYear.Text, cboJohap.Text, strGbJohap, argFDate, argTDate, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, strkiho);
                                        if (FstrInsFlag == "NO")
                                        {
                                            return;
                                        }
                                        nMirno = fn_READ_New_HicMirNo(); //새로운 미수번호
                                        FnTotCNT = 0;
                                        FnCnt1 = 0;
                                        FnCnt2 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //새로운 건강검진 비용청구서 INSERT(최대 100건씩 청구 가능)
                                fn_Build_Bohum_INSERT(nMirno, cboYear.Text, cboJohap.Text, strGbJohap, argFDate, argTDate, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, strkiho);
                                if (FstrInsFlag == "NO")
                                {
                                    return;
                                }
                                nMirno = fn_READ_New_HicMirNo(); //새로운 미수번호
                                FnTotCNT = 0;
                                FnCnt1 = 0;
                                FnCnt2 = 0;
                            }
                        }
                    }
                }
            }

            //새로운 건강검진 비용청구서 INSERT
            fn_Build_Bohum_INSERT(nMirno, cboYear.Text, cboJohap.Text, strGbJohap, argFDate, argTDate, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, strkiho);

            if (FstrInsFlag == "NO")
            {
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 새로운 건강검진 비용청구서 INSERT
        /// </summary>
        /// <param name="nMirno"></param>
        /// <param name="argYear"></param>
        /// <param name="strJohap"></param>
        /// <param name="strGbJohap"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="nTotCNT"></param>
        /// <param name="nCnt1"></param>
        /// <param name="nCnt2"></param>
        /// <param name="strChasu"></param>
        /// <param name="argSabun"></param>
        /// <param name="strMirGbn"></param>
        /// <param name="strLife_Gbn"></param>
        /// <param name="strKiho"></param>
        void fn_Build_Bohum_INSERT(long nMirno, string argYear, string strJohap, string strGbJohap, string argFDate, string argTDate, string strChasu, string argSabun, string strMirGbn, string strLife_Gbn, string strKiho)
        {
            int result = 0;

            FstrInsFlag = "";

            //2차시 판정이없으면
            FnCnt2 -= FnTemp;
            FnTotCNT -= FnTemp;

            result = hicMirBohumService.InsertAll(nMirno, argYear, strJohap, strGbJohap, argFDate, argTDate, FnTotCNT, FnCnt1, FnCnt2, strChasu, argSabun, strMirGbn, strLife_Gbn, strKiho);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_BOHUM에 INSERT시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrInsFlag = "NO";
                return;
            }
        }

        long fn_READ_New_HicMirNo()
        {
            long rtnVal = 0;

            rtnVal = comHpcLibBService.GetHicMIrNobySeq();

            return rtnVal;
        }

        /// <summary>
        /// 회사별 구강검사 청구대상자 확정
        /// </summary>
        /// <param name="strLtdCode"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="nCnt5"></param>
        void fn_Build_Dental(string argLtdCode, string argFDate, string argTDate, long argCnt)
        {
            int nREAD = 0;
            int nChasu = 0;
            //int nTotCNT = 0;
            //int nCnt1 = 0;
            //int nCnt2 = 0; //전체건수,1차건수,2차건수
            string strPandate = "";
            string strkiho = "";
            long nMirno = 0;
            long nWRTNO = 0;
            string strChasu = "";           //2차 단독청구시 Y
            string strPanoNew = "";
            string strPanoOld = "";
            string strFalg = "";
            string strMirGbn = "";          //추가청구구분
            string strOK = "";
            string strLife_Gbn = "";    //생애구분
            List<string> str검진종류세팅 = new List<string>();
            string strGbJohap = "";         //조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
            //검진종류 세팅 모듈에 사용 ----------------
            string strJohap2 = "";
            string strChasu2 = "";
            string strLife2 = "";
            string strBogenso = "";
            string strYear = "";
            string strKiho = "";
            string strJohap = "";

            int result = 0;

            FnTotCNT = 0;
            FnCnt1 = 0;
            FnCnt2 = 0;

            FnTemp = 0;

            strJohap2 = cboJohap.Text;

            if(rdoChasu0.Checked == true)
            {
                strChasu2 = "0";
            }
            else if (rdoChasu1.Checked == true)
            {
                strChasu2 = "1";
            }
            else if (rdoChasu2.Checked == true)
            {
                strChasu2 = "2";
            }

            strYear = cboYear.Text;
            nWRTNO = txtWrtNo.Text.To<long>();
            strBogenso = VB.Pstr(txtBogen.Text, ".", 1);
            strKiho = txtKiho.Text.Trim();
            strJohap = VB.Pstr(cboJohap.Text, ".", 1);
            strLife2 = "0";

            if (chkLife.Checked == true)
            {
                strLife2 = "1";
            }

            strLife_Gbn = "";
            if (argCnt > 0)
            {
                strLife_Gbn = "Y";
            }

            strMirGbn = "N";
            if (rdoMirGbn1.Checked == true)
            {
                strMirGbn = "Y";
            }
            strChasu = "N";

            //조합구분(1.지역 2.공교 3.직장 4.급여 0.공통)
            switch (argLtdCode)
            {
                case "지역":
                    strGbJohap = "1";
                    break;
                case "공교":
                    strGbJohap = "2";
                    break;
                case "직장":
                    strGbJohap = "3";
                    break;
                case "급여 ":
                    strGbJohap = "4";
                    break;
                default:
                    strGbJohap = "0";
                    break;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            nMirno = fn_READ_New_HicMirNo();    //새로운 청구번호
            str검진종류세팅 = chb.READ_HIC_HcKindSetting_List(strJohap2, strChasu2, strLife2);

            //해당 회사의 청구 대상자를 읽음
            //문진표가 입력된것만 명단을 작성함
            List<HIC_JEPSU> list = hicJepsuService.GetDentalItembyJepDateGjYear(argFDate, argTDate, strYear, str검진종류세팅, nWRTNO, argLtdCode, strBogenso, strJohap2, strKiho);

            nREAD = list.Count;

            FnTotCNT = 0;
            FnHuCnt = 0;
            for (int i = 0; i < nREAD; i++)
            {
                //판정여부(결과미입력도 미판정으로 처리)
                //구강문진표 누락 수검자는 명단작성 제외
                HIC_RES_DENTAL list2 = hicResDentalService.GetPanjengDateOpdDntDntStatusbyWrtNo(list[i].WRTNO);

                if (!list2.IsNullOrEmpty())
                {
                    strPandate = list2.PANJENGDATE;
                    if (list2.OPDDNT.IsNullOrEmpty())
                    {
                        strPandate = "";
                    }
                    if (list2.DNTSTATUS.IsNullOrEmpty())
                    {
                        strPandate = "";
                    }
                }

                if (!strPandate.IsNullOrEmpty())
                {
                    nWRTNO = list[i].WRTNO;
                    strkiho = list[i].KIHO;

                    //접수마스타에 청구형성 완료 SET
                    result = hicJepsuService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "2");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_JEPSU에 청구번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //수납마스타에 청구형성 완료 SET
                    result = hicSunapService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "2");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_SUNAP에 청구번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FnTotCNT += 1;   //전체건수

                    //휴일가산 건수 누적
                    if (hicResultService.GetCountbyWrtNoExCode(nWRTNO.To<string>(), "AA43") > 0) //구강 토.공휴일 가산
                    {
                        FnHuCnt += 1;
                    }

                    //If nTotCNT > 0 And nTotCNT >= 100 Then  //왜???
                    if (FnTotCNT >= 100)
                    {
                        //새로운 구강검사 비용청구서 INSERT(최대 100건씩 청구 가능)
                        fn_Build_Dental_INSERT(nMirno, strYear, strJohap, FnTotCNT, strGbJohap, argFDate, argTDate, strkiho, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, FnHuCnt);
                        if (FstrInsFlag == "NO")
                        {
                            return;
                        }
                        nMirno = fn_READ_New_HicMirNo(); //새로운 미수번호
                        FnTotCNT = 0;
                        FnHuCnt = 0;
                    }
                }
            }

            //새로운 구강검사 비용청구서 INSERT
            if (FnTotCNT > 0)
            {
                fn_Build_Dental_INSERT(nMirno, strYear, strJohap, FnTotCNT, strGbJohap, argFDate, argTDate, strkiho, strChasu, clsType.User.IdNumber, strMirGbn, strLife_Gbn, FnHuCnt);
            }

            if (FstrInsFlag == "NO")
            {
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 새로운 구강검진 비용청구서 INSERT
        /// </summary>
        /// <param name="nMirno"></param>
        /// <param name="strYear"></param>
        /// <param name="strJohap"></param>
        /// <param name="FnTotCNT"></param>
        /// <param name="strGbJohap"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="strkiho"></param>
        /// <param name="strChasu"></param>
        /// <param name="IdNumber"></param>
        /// <param name="strMirGbn"></param>
        /// <param name="strLife_Gbn"></param>
        /// <param name="FnHuCnt"></param>
        void fn_Build_Dental_INSERT(long nMirno, string strYear, string strJohap, long FnTotCNT, string strGbJohap, string argFDate, string argTDate, string strkiho, string strChasu, string IdNumber, string strMirGbn, string strLife_Gbn,long  FnHuCnt)
        {
            int result = 0;
            FstrInsFlag = "";

            result = hicMirDentalService.InsertAll(nMirno, strYear, strJohap, FnTotCNT, strGbJohap, argFDate, argTDate, strkiho, strChasu, IdNumber, strMirGbn, strLife_Gbn, FnHuCnt);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_DENTAL에 INSERT시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrInsFlag = "NO";
            }
        }

        /// <summary>
        /// 암검진 청구대상자 확정
        /// </summary>
        /// <param name="argLtdCode"></param>
        /// <param name="argFDate"></param>
        /// <param name="argTDate"></param>
        /// <param name="argCnt"></param>
        void fn_Build_Cancer(string argLtdCode, string argFDate, string argTDate, long argCnt)
        {
            int nREAD = 0;
            int nChasu = 0;
            //int nTotCNT = 0;
            //int nCnt1 = 0;
            //int nCnt2 = 0; //전체건수,1차건수,2차건수
            string strPandate = "";
            string strkiho = "";
            string strPano = "";
            long nMirno = 0;
            long nWRTNO = 0;
            string strJik = "";
            List<string> strWRTNO = new List<string>();
            string strMirGbn = ""; //추가청구구분
            string strLife_Gbn = ""; //생애구분
            string strTemp = "";
            string strPanoList = ""; //동일 청구번호에 암검진이 2개 이상이면 청구 오류로 추가함

            string strLife = "";
            string strYear = "";
            string strJong = "";
            string strJohap = "";
            string strW_Am = "";
            string strBogenso = "";

            int result = 0;

            strLife_Gbn = "";

            FnTotCNT = 0;
            FnCnt1 = 0;
            FnCnt2 = 0;

            if (argCnt > 0)
            {
                strLife_Gbn = "Y";
            }

            strMirGbn = "N";
            if (rdoMirGbn1.Checked == true)
            {
                strMirGbn = "Y";
            }

            strLife = "";
            if (chkLife.Checked == true)
            {
                strLife = "Y";
            }

            strYear = cboYear.Text;
            strJohap = cboJohap.Text.Trim();
            strJong = VB.Left(cboJong.Text, 4);
            strW_Am = "";
            if (chkW_Am.Checked == true)
            {
                strW_Am = "1";
            }
            strBogenso = VB.Pstr(txtBogen.Text.Trim(), ".", 1);

            clsDB.setBeginTran(clsDB.DbCon);

            nMirno = fn_READ_New_HicMirNo();    //새로운 미수번호

            //암검진 청구 대상자를 읽음

            List<HIC_JEPSU> list = hicJepsuService.GetCancerJepDateWrtNoKinoPanobyJepDate(argFDate, argTDate, strYear, strLife, argLtdCode, strJohap, strJong, strkiho, txtWrtNo.Text, strW_Am, strBogenso);

            nREAD = list.Count;

            if (cboJohap.Text.Trim() == "사업장")
            {
                strJik = "K";
            }
            else if (cboJohap.Text.Trim() == "공무원")
            {
                strJik = "G";
            }
            else if (cboJohap.Text.Trim() == "성인병")
            {
                strJik = "J";
            }
            else if (cboJohap.Text.Trim() == "통합")
            {
                strJik = "T";
            }

            strPanoList = ",";
            strWRTNO.Clear();
            for (int i = 0; i < nREAD; i++)
            {
                //1개의 청구번호에 동일 수검자가 2건이상이면 오류로 분리 청구함
                strPano = string.Format("{0:######0}", list[i].PANO);
                if (strPanoList.IndexOf("," + strPano + ",") == 0)
                {
                    strPanoList += strPano + ",";
                     
                    strTemp = "";
                    //판정여부(결과미입력도 미판정으로 처리)
                    HIC_CANCER_NEW list2 = hicCancerNewService.GetGunDatebyWrtNo(list[i].WRTNO);

                    if (!list2.IsNullOrEmpty())
                    {
                        strPandate = list2.GUNDATE;
                    }

                    nWRTNO = list[i].WRTNO;
                    strWRTNO.Add(nWRTNO.To<string>());
                    strkiho = list[i].KIHO;

                    if (VB.Left(cboJong.Text, 1) == "4")    //공단암
                    {
                        //접수마스타에 청구형성 완료 SET
                        result = hicJepsuService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "3");
                    }
                    else if (VB.Left(cboJong.Text, 1) == "E")    //의료급여암
                    {
                        result = hicJepsuService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "5");
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_JEPSU에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //수납마스타에 청구형성 완료 SET
                    if (VB.Left(cboJong.Text, 1) == "4")    //공단암
                    {
                        result = hicSunapService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "3");
                    }
                    else if (VB.Left(cboJong.Text, 1) == "E")    //의료급여암
                    {
                        result = hicSunapService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "5");
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_SUNAP에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FnTotCNT += 1;  // 전체건수

                    if (FnTotCNT >= 100)
                    {
                        break;
                    }
                }
            }

            //새로운 암검진 비용청구서 INSERT
            if (strTemp.IsNullOrEmpty())
            {
                fn_Build_CANCER_INSERT(nMirno, strYear, strJik, argFDate, argTDate, strkiho, clsType.User.IdNumber, strMirGbn, strLife_Gbn, strJong);                
            }

            if (FstrInsFlag == "NO")
            {
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //보건소암 조회및 청구명단 작성 (특정암일경우만 작성)
            if (VB.Left(cboJong.Text, 1) == "4")
            {
                List<HIC_JEPSU> list2 = hicJepsuService.GetExpenseItembyJepDAteGjYearWrtNo(argFDate, argTDate, strYear, strLife, strWRTNO);

                nREAD = list2.Count;

                FnTotCNT = 0;
                FnCnt1 = 0;
                FnCnt2 = 0;
                if (cboJohap.Text.Trim() == "사업장")
                {
                    strJik = "K";
                }
                else if (cboJohap.Text.Trim() == "공무원")
                {
                    strJik = "G";
                }
                else if (cboJohap.Text.Trim() == "성인병")
                {
                    strJik = "J";
                }

                //보건소암 건수가 있어야 청구명단 만듬
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        //판정여부(결과미입력도 미판정으로 처리)
                        HIC_CANCER_NEW list3 = hicCancerNewService.GetGunDatebyWrtNo(list[i].WRTNO);

                        if (!list3.IsNullOrEmpty())
                        {
                            strPandate = list3.GUNDATE;
                        }

                        nWRTNO = list[i].WRTNO;
                        strkiho = list[i].KIHO;

                        //접수마스타에 청구형성 완료 SET
                        result = hicJepsuService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "4");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_JEPSU에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //수납마스타에 청구형성 완료 SET
                        result = hicSunapService.UpdateMirNobyWrtNo(nWRTNO, nMirno, "4");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_SUNAP에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        FnTotCNT += 1;  //전체건수
                    }

                    //새로운 보건소암 비용청구서 INSERT
                    fn_Build_CANCER_BO_INSERT(nMirno, strYear, strJik, argFDate, argTDate, strkiho, clsType.User.IdNumber, strMirGbn, strLife_Gbn);
                    if (FstrInsFlag == "NO")
                    {
                        return;
                    }
                }
            }

            if (FstrInsFlag == "")
            {
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        /// <summary>
        /// 새로운 암검진 비용청구서 INSERT
        /// </summary>
        /// <param name="nMirNo"></param>
        /// <param name="strYear"></param>
        /// <param name="strjik"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="strkiho"></param>
        /// <param name="IdNumber"></param>
        /// <param name="strMirGbn"></param>
        /// <param name="strLife_Gbn"></param>
        /// <param name="strJong"></param>
        void fn_Build_CANCER_INSERT(long nMirNo, string strYear, string strjik, string strFDate, string strTDate, string strkiho, string IdNumber, string strMirGbn, string strLife_Gbn, string strJong)
        {
            int result = 0;

            result = hicMirCancerService.InsertAll(nMirNo, strYear, strjik, strFDate, strTDate, strkiho, FnTotCNT, IdNumber, strMirGbn, strLife_Gbn, VB.Left(cboJong.Text, 1));

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_CANCER에 INSERT시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrInsFlag = "NO";
                return;
            }
        }

        /// <summary>
        /// 새로운 보건소암 비용청구서 INSERT
        /// </summary>
        /// <param name="nMirNo"></param>
        /// <param name="strYear"></param>
        /// <param name="strjik"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="strkiho"></param>
        /// <param name="IdNumber"></param>
        /// <param name="strMirGbn"></param>
        /// <param name="strLife_Gbn"></param>
        void fn_Build_CANCER_BO_INSERT(long nMirNo, string strYear, string strjik, string strFDate, string strTDate, string strkiho, string IdNumber, string strMirGbn, string strLife_Gbn)
        {
            int result = 0;

            result = hicMirCancerBoService.InsertAll(nMirNo, strYear, strjik, strFDate, strTDate, strkiho, FnTotCNT, IdNumber, strMirGbn, strLife_Gbn);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_CANCER_BO에 INSERT시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrInsFlag = "NO";
                return;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strName = "";

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        strName = hb.READ_Ltd_Name(txtLtdCode.Text.Trim());

                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                        else
                        {
                            txtLtdCode.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtBogen)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtBogen.Text.Length >= 2)
                    {
                        strName = hicCodeService.GetNameByGubunCode("25", txtBogen.Text.Trim());
                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(txtBogen, new EventArgs());
                        }
                        else
                        {
                            txtBogen.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtKiho)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtKiho.Text.Length >= 2)
                    {
                        strName = hicCodeService.GetNameByGubunCode("18", txtKiho.Text.Trim());
                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(txtKiho, new EventArgs());
                        }
                        else
                        {
                            txtKiho.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS1, e.Column, ref boolSort, true);
                    return;
                }
            }
            else if (sender == SS2)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS2, e.Column, ref boolSort, true);
                    return;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                int nREAD = 0;
                int nRow = 0;
                string strLtdCode = "";
                string strFDate = "";
                string strTDate = "";
                long nTotAmt = 0;
                string strTemp = "";
                string strOK = "";

                //검진종류 세팅 모듈에 사용 ========================
                string strJohap2 = "";
                string strChasu2 = "";
                string strLife2 = "";
                //string str검진종류세팅 = "";
                List<string> str검진종류세팅 = new List<string>();
                string strTempSQL = "";
                string strJong = "";
                string strLife = "";
                string strGubun1 = "";
                string strGubun2 = "";
                string strW_Am = "";

                strJohap2 = cboJohap.Text;

                if (rdoChasu0.Checked == true)
                {
                    strChasu2 = "0";
                }
                else if (rdoChasu1.Checked == true)
                {
                    strChasu2 = "1";
                }
                else if (rdoChasu2.Checked == true)
                {
                    strChasu2 = "2";
                }

                strLife2 = "0";

                if (chkLife.Checked == true)
                {
                    strLife2 = "1";
                }

                strGubun1 = "";
                if (chkGubun1.Checked == true)
                {
                    strGubun1 = "1";
                }

                strGubun2 = "";
                if (chkGubun2.Checked == true)
                {
                    strGubun2 = "1";
                }

                strW_Am = "";
                if (chkW_Am.Checked == true)
                {
                    strW_Am = "1";
                }

                strJong = cboJong.Text;

                //검진종류 세팅 모듈에 사용 ========================

                strLtdCode = SS1.ActiveSheet.Cells[1, 1].Text.Trim();
                FstrLtdName = SS1.ActiveSheet.Cells[1, 2].Text.Trim();
                strFDate = SS1.ActiveSheet.Cells[1, 3].Text.Trim();
                strTDate = SS1.ActiveSheet.Cells[1, 4].Text.Trim();

                //해당 회사의 청구명단을 표시함
                sp.Spread_All_Clear(SS2);

                //명단을 SELECT
                if (VB.Left(cboJohap.Text, 1) == "1")   //건강검진
                {
                    str검진종류세팅 = chb.READ_HIC_HcKindSetting_List(strJohap2, strChasu2, strLife2);

                    HIC_JEPSU item = new HIC_JEPSU();

                    item.GJYEAR = cboYear.Text;
                    item.WRTNO = txtWrtNo.Text.To<long>();
                    item.BOGUNSO = VB.Pstr(txtBogen.Text, ".", 1);
                    item.LTDCODE = strLtdCode.To<long>();
                    item.KIHO = VB.Pstr(txtKiho.Text, ".", 1);
                    item.GJJONG = VB.Pstr(cboJong.Text, ".", 1);

                    List<HIC_JEPSU> list = hicJepsuService.GetItembyAll(strFDate, strTDate, strJong, strJohap2, str검진종류세팅, strLtdCode, strLife2, strGubun1, strGubun2, strW_Am, item);

                    nREAD = list.Count;
                    nRow = 0;
                    if (nREAD > 0)
                    {
                        for (int i = 0; i < nREAD; i++)
                        {
                            //조합부담액이 있으면 청구함
                            HIC_SUNAP list2 = hicSunapService.GetJohapAmtBogenAmtbyWrtNo(list[i].WRTNO);

                            strOK = "OK";

                            if (list2.JOHAPAMT == 0 && list2.BOGENAMT == 0)
                            {
                                strOK = "";
                            }

                            if (strOK == "OK")
                            {
                                nRow += 1;
                                if (nRow > SS2.ActiveSheet.RowCount)
                                {
                                    SS2.ActiveSheet.RowCount = nRow;
                                }
                                SS2.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                                if (list[i].MURYOAM == "Y")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 202, 255);
                                }

                                SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].WRTNO.To<string>();
                                SS2.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                                SS2.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].AGE + "/" + list[i].SEX;

                                if (VB.Left(cboJong.Text, 1) == "1" || VB.Left(cboJong.Text, 1) == "3")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                                }
                                else if (VB.Left(cboJong.Text, 1) == "4")
                                {
                                    if (list[i].AGE < 40)
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].LTDCODE + "  만40↓";
                                    }
                                    else
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].LTDCODE.To<string>();
                                    }
                                }
                                else if (VB.Left(cboJong.Text, 1) == "E")
                                {
                                    if (list[i].AGE < 40)
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].KIHO + "  만40↓";
                                    }
                                    else
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].KIHO.To<string>();
                                    }
                                }

                                //생애전환기 여부
                                if (fn_Read_TurningPointLife(list[i].GJJONG) == "Y")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 4].BackColor = Color.FromArgb(128, 255, 128);
                                }
                                SS2.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].GJCHASU;

                                //판정여부(결과미입력도 미판정으로 처리)
                                SS2.ActiveSheet.Cells[nRow - 1, 6].Text = "";

                                strJong = VB.Left(cboJong.Text, 1);    //건강검진
                                List<COMHPC> list3 = comHpcLibBService.GetPanjengDatebyWrtNo(strJong, list[i].GJCHASU, list[i].WRTNO);
                                
                                if (list3.Count > 0)
                                {
                                    if (list[i].GBSTS != "2")
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 6].Text = "Ⅹ";
                                    }
                                    if (list3[0].PANJENGDATE.IsNullOrEmpty())
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "Ⅹ";
                                    }
                                    //구강종합판정 체크
                                    if (VB.Left(cboJong.Text, 1) == "3")    //구강검사
                                    {
                                        strTemp = "";
                                        if (!list3[0].PANJENGDATE.IsNullOrEmpty())
                                        {
                                            if (!list3[0].T_PANJENG1.IsNullOrEmpty())
                                            {
                                                strTemp = "OK";
                                            }

                                            if (!list3[0].RES_RESULT.IsNullOrEmpty())
                                            {
                                                strTemp = "OK";
                                            }

                                            if (!list3[0].RES_JOCHI.IsNullOrEmpty())
                                            {
                                                strTemp = "OK";
                                            }
                                        }
                                        if (strTemp != "OK")
                                        {
                                            SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "Ⅹ";
                                        }
                                    }
                                }
                                else
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 6].Text = "Ⅹ";
                                }
                                //2차미실시
                                SS2.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                                if (list[i].SECOND_FLAG == "Y")
                                {
                                    if (list[i].SECOND_DATE.IsNullOrEmpty())
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, 8].Text = "Ⅹ";
                                    }
                                }
                                SS2.ActiveSheet.Cells[nRow - 1, 9].Text = hm.SExam_Names_Display(list[i].SEXAMS);
                                SS2.ActiveSheet.Cells[nRow - 1, 10].Text = hm.UCode_Names_Display(list[i].UCODES);
                                SS2.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].KIHO;
                                SS2.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].BOGUNSO;
                            }
                        }
                    }

                    SS2.ActiveSheet.RowCount = nRow;
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

        /// <summary>
        /// READ_생애여부(string argJong)
        /// </summary>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_Read_TurningPointLife(string argJong)
        {
            string rtnVal = "N";

            switch (argJong)
            {
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "35":
                    rtnVal = "Y";
                    break;
                default:
                    rtnVal = "N";
                    break;
            }

            return rtnVal;
        }
    }
}
