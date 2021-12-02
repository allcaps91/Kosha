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
/// File Name       : frmHcBillSunapList.cs
/// Description     : 청구작업용 수납집계표
/// Author          : 이상훈
/// Create Date     : 2021-01-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSunapList.frm(HcBill077)" />

namespace HC_Bill
{
    public partial class frmHcBillSunapList : Form
    {
        HicCodeService hicCodeService = null;
        HicResultService hicResultService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicPatientService hicPatientService = null;

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


        public frmHcBillSunapList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            hicResultService = new HicResultService();
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuService = new HicJepsuService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicPatientService = new HicPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnKiho.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtKiho.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns[7].Visible = false;
            SS1_Sheet1.Columns[17].Visible = false;

            dtpFDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpTDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";

            //검진종류 SET
            cboJong.Items.Clear();
            cboJong.Items.Add("1.건강검진");
            cboJong.Items.Add("3.구강검진");
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;
            txtKiho.Text = "";

            //성인병구분
            cboGKiho.Items.Clear();
            cboGKiho.Items.Add(" ");
            cboGKiho.Items.Add("K.지역");
            cboGKiho.Items.Add("G.공교");
            cboGKiho.Items.Add("J.직장");
            cboGKiho.SelectedIndex = 0;

            chkBo.Visible = false;
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
                int nREAD = 0;
                int nRow = 0;
                long nPano = 0;
                string strJumin = "";
                string strUCodes = "";
                string strGjYear = "";
                string strOldData = "";
                string strNewData = "";
                string strJob = "";
                int nDntCnt1 = 0;
                int nDntCnt2 = 0;
                string strChasu = "";
                long nDntAmt = 0;// 구강금액
                double[,] nTotAmt = new double[3, 9];
                string strJong = "";
                string strFDate = "";
                string strTDate = "";
                long nLtdCode = 0;
                string strJonggum = "";
                long nMirNo = 0;
                string strBo = "";
                string strSunap = "";
                string strDentAmt = "";
                string strDentAmt1 = "";

                List<HIC_JEPSU_SUNAP> jsList = new List<HIC_JEPSU_SUNAP>();
                
                if (string.Compare(dtpTDate.Text, dtpFDate.Text) < 0)
                {
                    MessageBox.Show("종료일자가 시작일보다 적음", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strJong = VB.Left(cboJong.Text, 1);

                //해당년도 구강청구단가를 설정
                hb.SET_Dental_Amt(VB.Left(dtpFDate.Text, 4));

                sp.Spread_All_Clear(SS1);
                Application.DoEvents();
                //SS1.ActiveSheet.RowCount = 30;

                //누적할 배열을 Clear
                for (int i = 0; i <= 8; i++)
                {
                    nTotAmt[1, i] = 0;
                    nTotAmt[2, i] = 0;
                }

                nDntCnt1 = 0;
                nDntCnt2 = 0;

                if (txtMirtNo.Text.Trim() == "")
                {
                    MessageBox.Show("청구번호를 입력하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMirtNo.Focus();
                    return;
                }

                HIC_RESULT list = hicResultService.GetCount(strJong, txtMirtNo.Text.To<long>());

                if (!list.IsNullOrEmpty())
                {
                    nDntCnt1 = (int)list.CNT;
                }

                //구강 토.공휴일가산
                HIC_SUNAPDTL list2 = hicSunapdtlService.GetCount(strJong, txtMirtNo.Text.To<long>());

                if (!list2.IsNullOrEmpty())
                {
                    nDntCnt2 = (int)list2.CNT;
                }

                HIC_SUNAPDTL list3 = hicSunapdtlService.GetCountbyCodeMirNo("1118", strJong, txtMirtNo.Text.To<long>());

                if (list3.IsNullOrEmpty())
                {
                    nDntCnt2 = (int)list3.CNT;
                }

                List<HIC_JEPSU> list4 = hicJepsuService.GetGjJongCntbyMirNo(strJong, txtMirtNo.Text.To<long>());

                if (list4.Count == 1)
                {
                    strChasu = "1";
                }

                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;

                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                if (chkJongGum.Checked == true)
                {
                    strJonggum = "Y";
                }
                else
                {
                    strJonggum = "";
                }

                nMirNo = txtMirtNo.Text.To<long>();

                if (chkBo.Checked == true)
                {
                    strBo = "Y";
                }
                else
                {
                    strBo = "";
                }

                if (chkSunap.Checked == true)
                {
                    strSunap = "Y";
                }
                else
                {
                    strSunap = "";
                }

                //자료를 SELECT
                if (chkHistory.Checked == false)
                {
                    jsList = hicJepsuSunapService.GetItembySuDateLtdCodeMirNo(strFDate, strTDate, nLtdCode, strJong, strJonggum, nMirNo, strSunap, strBo);
                }
                else
                {
                    jsList = hicJepsuSunapService.GetItembySuDateMirNo(strFDate, strTDate, strJong, strJonggum, nMirNo, strSunap, strBo);
                }

                nREAD = jsList.Count;
                if (nREAD == 0)
                {
                    MessageBox.Show("수납 자료가 1건도 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                nRow = 0;
                strOldData = "";
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nPano = jsList[i].PANO;
                    strNewData = VB.Left(jsList[i].GJJONG + VB.Space(2) , 2);
                    strNewData += string.Format("{0:00000000}", nPano);
                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    if (strOldData.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_GjJong_Name(VB.Left(strNewData, 2));
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = nPano.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = jsList[i].SNAME;
                    }
                    else if (VB.Left(strOldData, 2) != VB.Left(strNewData, 2))
                    {
                        //GoSub SubTotal_Display  ===================================================================================================================================================
                        strDentAmt = string.Format("{0:###,###,###,##0}", nTotAmt[1, 3] - (nDntCnt1 * clsHcVariable.GnDentAmt) - (nDntCnt2 * clsHcVariable.GnDentAddAmt));
                        strDentAmt1 = string.Format("{0:###,###,###,##0}", nTotAmt[1, 4] - (nTotAmt[1, 4] - (nDntCnt1 * clsHcVariable.GnDentAmt * 0.1) - (nDntCnt2 * clsHcVariable.GnDentAddAmt * 0.1)));

                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "구강" + nDntCnt1 + "명";
                        if (VB.Left(cboJong.Text, 1) != "5")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strDentAmt;
                        }
                        else//보건소암
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strDentAmt1;
                        }
                        for (int j = 0; j <= 8; j++)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, j + 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt[1, j + 1]);
                            nTotAmt[1, j + 1] = 0;
                        }
                        //===========================================================================================================================================================================

                        strOldData = strNewData;
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_GjJong_Name(VB.Left(strNewData, 2));
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = nPano.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = jsList[i].SNAME;
                    }
                    else if (strOldData != strNewData)
                    {
                        strOldData = strNewData;
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = nPano.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = jsList[i].SNAME;
                    }

                    //주민번호 조회
                    HIC_PATIENT list5 = hicPatientService.GetJumin2byPano(nPano);

                    if (!list5.IsNullOrEmpty())
                    {
                        strJumin = clsAES.DeAES(list5.JUMIN2);
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = jsList[i].SEQNO.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = jsList[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = jsList[i].JEPDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = jsList[i].JONGGUMYN;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = hb.READ_Ltd_Name(jsList[i].LTDCODE.To<string>());
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = hb.READ_HIC_InsaName(jsList[i].JOBSABUN.To<string>());

                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = string.Format("{0:###,###,###,##0}", jsList[i].SUNAPAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = string.Format("{0:###,###,###,##0}", jsList[i].TOTAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 12].Text = string.Format("{0:###,###,###,##0}", jsList[i].JOHAPAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 13].Text = string.Format("{0:###,###,###,##0}", jsList[i].LTDAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 14].Text = string.Format("{0:###,###,###,##0}", jsList[i].BOGENAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 15].Text = string.Format("{0:###,###,###,##0}", jsList[i].BONINAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 16].Text = string.Format("{0:###,###,###,##0}", jsList[i].HALINAMT);
                    SS1.ActiveSheet.Cells[nRow - 1, 17].Text = string.Format("{0:###,###,###,##0}", jsList[i].MISUAMT);

                    strGjYear = VB.Left(dtpFDate.Text, 4);
                    //1차검진의 유해인자 2차에 출력
                    HIC_JEPSU list6 = hicJepsuService.GetUCodesbyPaNoGjYear(nPano, strGjYear);

                    strUCodes = "";
                    if (!list6.IsNullOrEmpty())
                    {
                        strUCodes = list6.UCODES;
                    }

                    if (jsList[i].GJCHASU == "2" || jsList[i].GJJONG == "19")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 18].Text = hm.UCode_Names_Display(strUCodes);
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 18].Text = hm.UCode_Names_Display(jsList[i].UCODES);
                    }

                    //소계에 금액을 Add
                    nTotAmt[1, 1] += jsList[i].SUNAPAMT;
                    nTotAmt[1, 2] += jsList[i].TOTAMT;
                    nTotAmt[1, 3] += jsList[i].JOHAPAMT;
                    nTotAmt[1, 4] += jsList[i].LTDAMT;
                    nTotAmt[1, 5] += jsList[i].BOGENAMT;
                    nTotAmt[1, 6] += jsList[i].BONINAMT;
                    nTotAmt[1, 7] += jsList[i].HALINAMT;
                    nTotAmt[1, 8] += jsList[i].MISUAMT;

                    //합계에 금액을 Add
                    nTotAmt[2, 1] += jsList[i].SUNAPAMT;
                    nTotAmt[2, 2] += jsList[i].TOTAMT;
                    nTotAmt[2, 3] += jsList[i].JOHAPAMT;
                    nTotAmt[2, 4] += jsList[i].LTDAMT;
                    nTotAmt[2, 5] += jsList[i].BOGENAMT;
                    nTotAmt[2, 6] += jsList[i].BONINAMT;
                    nTotAmt[2, 7] += jsList[i].HALINAMT;
                    nTotAmt[2, 8] += jsList[i].MISUAMT;

                    progressBar1.Value = i + 1;
                }

                nRow += 2;
                SS1.ActiveSheet.RowCount = nRow;
                //건진종류별 소계를 Display
                //GoSub SubTotal_Display  ===================================================================================================================================================
                strDentAmt = string.Format("{0:###,###,###,##0}", nTotAmt[1, 3] - (nDntCnt1 * clsHcVariable.GnDentAmt) - (nDntCnt2 * clsHcVariable.GnDentAddAmt));
                strDentAmt1 = string.Format("{0:###,###,###,##0}", nTotAmt[1, 4] - (nTotAmt[1, 4] - (nDntCnt1 * clsHcVariable.GnDentAmt * 0.1) - (nDntCnt2 * clsHcVariable.GnDentAddAmt * 0.1)));

                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "구강" + nDntCnt1 + "명";
                if (VB.Left(cboJong.Text, 1) != "5")
                {
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strDentAmt;
                }
                else//보건소암
                {
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strDentAmt1;
                }
                for (int j = 0; j <= 8; j++)
                {
                    SS1.ActiveSheet.Cells[nRow - 1, j + 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt[1, j]);
                    nTotAmt[1, j] = 0;
                }
                //===========================================================================================================================================================================
                //합계를 Display
                //GoSub Total_Display
                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "합계";
                for (int j = 0; j < 8; j++)
                {
                    SS1.ActiveSheet.Cells[nRow - 1, j + 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt[2, j]);
                    nTotAmt[2, j] = 0;
                }
                //===========================================================================================================================================================================
                if (strChasu != "1")
                {
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 5].Text = "";
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 8].Text = "";
                }
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

                strTitle = "검 진 비  수 납 자  명 단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간:" + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("검진종류:" + cboJong.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  Page: /p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnKiho)
            {
                Hic_Code_Help("18", txtKiho);
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
