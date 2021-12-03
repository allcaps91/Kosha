using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSchoolChargeExpenses_New.cs
/// Description     : 학생검진 비용 청구서
/// Author          : 이상훈
/// Create Date     : 2020-01-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool5_New.frm(HcSchool05_New)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolChargeExpenses_New : Form
    {
        HicSchoolNewService hicSchoolNewService = null;
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;
        HicBcodeService hicBcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        //string FstrWRTNO;
        List<string> FstrWRTNO = new List<string>();

        public frmHcSchoolChargeExpenses_New()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void SetEvent()
        {
            hicSchoolNewService = new HicSchoolNewService();
            hicJepsuPatientSchoolService = new HicJepsuPatientSchoolService();
            hicBcodeService = new HicBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtRDate.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtRDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtRDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            btnPrint.Enabled = false;

            SS1_Sheet1.Columns.Get(0).Visible = false;
            SS1_Sheet1.Columns.Get(6).Visible = false;
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
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
                int nRead = 0;
                string strOK = "";
                string strOK2 = "";
                string strFrDate = "";
                string strToDate = "";
                string strPrt = "";
                long nLtdCode = 0;
                long nLtdCode1 = 0;
                string strClass = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                if (rdoPrint1.Checked == true)
                {
                    strPrt = "1";
                }
                else if (rdoPrint2.Checked == true)
                {
                    strPrt = "2";
                }
                else
                {
                    strPrt = "";
                }

                sp.Spread_All_Clear(SS1);
                fn_SS2_Clear();

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDatePrtCnt(strFrDate, strToDate, strPrt, nLtdCode);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;

                for (int i = 0; i < nRead; i++)
                {
                    strOK = "";
                    switch (list[i].CLASS.To<string>())
                    {
                        case "2":
                        case "3":
                        case "5":
                        case "6":
                            strOK = "OK";
                            break;
                        case "1":
                        case "4":
                            strOK2 = "OK";
                            break;
                        default:
                            break;
                    }

                    nLtdCode1 = list[i].LTDCODE;
                    strClass = list[i].CLASS.To<string>();

                    HIC_SCHOOL_NEW list2 = hicSchoolNewService.GetDPanDrNobySDate(strFrDate, strToDate, nLtdCode1, strClass);

                    if (strOK == "OK")
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].MINDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].MAXDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].CNT.ToString();
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].CLASS.ToString();
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].LTDCODE.ToString();
                    }
                    else if (strOK2 == "OK")
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].MINDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].MAXDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].CNT.ToString();
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].CLASS.ToString();
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].LTDCODE.ToString();
                    }
                }
                btnPrint.Enabled = true;

                 FstrWRTNO.Clear();
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;
                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (FstrWRTNO == null)
                {
                    MessageBox.Show("다시 선택하여 인쇄하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

                //인쇄처리
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSchoolNewService.UpdateGbMirPrintbyWrtNo(FstrWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("DB에 자료를 등록시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FstrWRTNO.Clear();
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtRDate)
            {
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                clsPublic.GstrCalDate = "";
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                txtRDate.Text = clsPublic.GstrCalDate;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strDate1 = "";
                string strDate2 = "";
                string strClass = "";
                string strBan = "";
                string strLtdCode = "";

                fn_SS2_Clear();
                strDate1 = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                strDate2 = SS1.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                strClass = SS1.ActiveSheet.Cells[e.Row, 5].Text.Trim();
                strLtdCode = SS1.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                fn_Screen_Display(strDate1, strDate2, strClass, strBan, strLtdCode);
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtRDate)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtLtdCode)
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

        void fn_Screen_Display(string argDate1, string argDate2, string argClass, string argBan, string argLtdCode)
        {
            int nRead = 0;
            string strData = "";
            string strGBn = "";
            string strSex = "";
            string strLtdName = "";
            string strBiman = "";
            string strWEIGHT = "";
            long[] nPrice = new long[9];
            int[] nMan = new int[9];
            int[] nWoMan = new int[9];
            long[] nSum = new long[6];
            string strCode = "";

            FstrWRTNO.Clear();

            for (int j = 0; j <= 8; j++)
            {
                nPrice[j] = 0;
                nMan[j] = 0;
                nWoMan[j] = 0;
                if (j <= 5)
                {
                    nSum[j] = 0;
                }
            }

            //----------------------------------------------------------
            //   검진시작일자를 기준으로 검진단가 SET
            //----------------------------------------------------------
            //(1)건강검진 상담료 및 행정비용 (2)구강검사 (3)요검사 (4)혈액검사
            //(5)혈색소 (6)혈액형 (7)흉부방사선 (8)간염검사
            strCode = VB.Left(dtpFrDate.Text, 4);
            strData = hicBcodeService.GetCodeNamebyGubunCode("HIC_학생검진단가", strCode);

            if (strData.IsNullOrEmpty())
            {
                MessageBox.Show(VB.Left(dtpFrDate.Text, 4) + "년도 학생검진 단가가 자료사전에 누락됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 1; i <= 8; i++)
            {
                nPrice[i] = long.Parse(VB.Pstr(strData, ",", i));
                if (nPrice[i] == 0)
                {
                    MessageBox.Show(VB.Left(dtpFrDate.Text, 4) + "년도 학생검진 단가 " + i + 1 +"번 항목 금액이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //계
            for (int i = 1; i <= 8; i++)
            {
                nSum[0] += nPrice[i];
            }

            //초기세팅
            strLtdName = hb.READ_Ltd_Name(argLtdCode);
            if (VB.L(strLtdName, "초등") > 1)
            {
                strGBn = "1";
            }
            else if (VB.L(strLtdName, "중학") > 1)
            {
                strGBn = "2";
            }
            else if (VB.L(strLtdName, "고등") > 1)
            {
                strGBn = "3";
            }

            for (int k = 15; k <= 22; k++)
            {
                SS2.ActiveSheet.Cells[k - 1, 4].Text = string.Format("{0:N0}", nPrice[k - 14]);
            }

            List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDate(argDate1, argDate2, argLtdCode, argClass);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strSex = list[i].SEX;
                strBiman = list[i].PPANA3;
                strWEIGHT = list[i].PPANA4;
                FstrWRTNO.Add(list[i].WRTNO.To<string>());
                switch (strSex)
                {
                    case "M":
                        if (list[i].PPANDRNO > 0) 
                        {
                            nMan[1] += 1;   //상담료
                        }
                        if (!list[i].DPANSOGEN.IsNullOrEmpty())
                        {
                            nMan[2] += 1;   //구강
                        }

                        if (!list[i].PPANE1.IsNullOrEmpty())
                        {
                            nMan[3] += 1;   //요검사
                        }
                        if (string.Compare(strWEIGHT, "1") > 0)
                        {
                            if ((strGBn == "2" || strGBn == "3") || (strGBn == "1" && argClass == "4"))
                            {
                                if (!list[i].PPANF1.IsNullOrEmpty())
                                {
                                    nMan[4] += 1;   //혈액검사
                                }
                            }
                        }
                        if (strGBn == "1" && argClass == "1")
                        {
                            switch (VB.Pstr(list[i].PPANF6, "^^", 1))
                            {
                                case "미실시":
                                case ".":
                                case "":
                                    break;
                                default:
                                    nMan[6] += 1;   //혈액형
                                    break;
                            }
                        }
                        if (strGBn != "1")
                        {
                            switch (VB.Pstr(list[i].PPANG1, "^^", 1))
                            {
                                case "":
                                case "11":
                                    break;
                                default:
                                    nMan[7] += 1;   //흉부
                                    break;
                            }
                        }
                        if (strGBn == "2")
                        {
                            if (list[i].PPANH1 == "1" || list[i].PPANH1 == "2")
                            {
                                nMan[8] += 1;   //간염
                            }
                        }
                        break;
                    case "F":
                        if (list[i].PPANDRNO > 0)
                        {
                            nWoMan[1] += 1; //상담료
                        }
                        if (!list[i].DPANSOGEN.IsNullOrEmpty())
                        {
                            nWoMan[2] += 1; //구강
                        }
                        if (!list[i].PPANE1.IsNullOrEmpty())
                        {
                            nWoMan[3] += 1; //요검사
                        }
                        if (string.Compare(strWEIGHT, "1") > 0)
                        {
                            if ((strGBn == "2" || strGBn == "3") || (strGBn == "1" && argClass == "4"))
                            {
                                if (!list[i].PPANF1.IsNullOrEmpty())
                                {
                                    nWoMan[4] += 1; //혈액검사
                                }
                            }
                        }
                        if (strGBn == "3")
                        {
                            if (!list[i].PPANF6.IsNullOrEmpty())
                            {
                                nWoMan[5] += 1; // 혈색소
                            }
                        }
                        if (strGBn == "1" && argClass == "1")
                        {
                            switch (VB.Pstr(list[i].PPANF6, "^^", 1))
                            {
                                case "미실시":
                                case ".":
                                case "":
                                    break;
                                default:
                                    nMan[6] += 1;   //혈액형
                                    break;
                            }
                        }
                        if (strGBn != "1")
                        {
                            switch (VB.Pstr(list[i].PPANG1, "^^", 1))
                            {
                                case "":
                                case "11":
                                    break;
                                default:
                                    nMan[7] += 1;   //흉부
                                    break;
                            }
                        }
                        if (strGBn == "2")
                        {
                            if (list[i].PPANH1 == "1" || list[i].PPANH1 == "2")
                            {
                                nWoMan[8] += 1; //간염
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            //학교명
            SS2.ActiveSheet.Cells[3, 3].Text = strLtdName + " ( " + argClass + " 학년  )";

            for (int i = 15; i <= 22; i++)
            {
                SS2.ActiveSheet.Cells[i - 1, 5].Text = nMan[i - 14].ToString();
                nSum[2] += nMan[i - 14];
                SS2.ActiveSheet.Cells[i - 1, 6].Text = nWoMan[i - 14].ToString();
                nSum[3] += nWoMan[i - 14];
                SS2.ActiveSheet.Cells[i - 1, 7].Text = (nMan[i - 14] + nWoMan[i - 14]).ToString();
                nSum[4] += nMan[i - 14] + nWoMan[i - 14];
                SS2.ActiveSheet.Cells[i - 1, 8].Text = string.Format("{0:N0}", (nMan[i - 14] + nWoMan[i - 14]) * nPrice[i - 14]) + " ";
                nSum[5] += (nMan[i - 14] + nWoMan[i - 14]) * nPrice[i - 14];
            }

            for (int i = 5; i <= 9; i++)
            {
                if (i == 9)
                {
                    SS2.ActiveSheet.Cells[22, i - 1].Text = string.Format("{0:N0}", nSum[i - 4]) + " ";
                }
                else
                {
                    SS2.ActiveSheet.Cells[22, i - 1].Text = "-";
                }
            }

            //전체계
            SS2.ActiveSheet.Cells[9, 4].Text = string.Format("{0:N0}", nSum[5]);
            SS2.ActiveSheet.Cells[24, 2].Text = "우리 기관에서 " + argDate1 + " ~ " + argDate2 + " 까지 실시한 학생 건강검사 실시자의";
            SS2.ActiveSheet.Cells[27, 8].Text = VB.Left(txtRDate.Text, 4) + "년 " + VB.Mid(txtRDate.Text, 6, 2) + "월 " + VB.Right(txtRDate.Text, 2) + "일";
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            SS2.ActiveSheet.Cells[2, 0].CellType = txt;
            SS2.ActiveSheet.SetActiveCell(2, 0);
        }

        void fn_SS2_Clear()
        {
            SS2.ActiveSheet.Cells[3, 3].Text = "";  //학교명
            SS2.ActiveSheet.Cells[9, 4].Text = "";  //총청구금액
            for (int i = 14; i <= 22; i++)
            {
                for (int j = 4; j <= 10; j++)
                {
                    SS2.ActiveSheet.Cells[i, j].Text = "";
                }
            }
            SS2.ActiveSheet.Cells[24, 2].Text = "";
            SS2.ActiveSheet.Cells[27, 8].Text = "";
            SS2.ActiveSheet.Cells[31, 4].Text = "";
            SS2.ActiveSheet.Cells[32, 4].Text = "";
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
