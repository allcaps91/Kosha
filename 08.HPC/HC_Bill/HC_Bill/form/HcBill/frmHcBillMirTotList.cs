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
/// File Name       : frmHcBillMirTotList.cs
/// Description     : 종검 및 일반건진 청구금액 조회
/// Author          : 이상훈
/// Create Date     : 2021-02-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmMirLtdList.frm(HcBill101)" />

namespace HC_Bill
{
    public partial class frmHcBillMirTotList : Form
    {
        HicJepsuPatientService hicJepsuPatientService = null;
        HeaSunapService heaSunapService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicSunapService hicSunapService = null;

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

        public frmHcBillMirTotList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            heaSunapService = new HeaSunapService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicSunapService = new HicSunapService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";
            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpTDate.Text = clsPublic.GstrSysDate;
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
                long nTotAmt = 0;
                long nTotAmt1 = 0;
                long nAmt1 = 0;
                long nAmt2 = 0;
                long nAmt3 = 0;
                long[] nTot = new long[5];
                int nRow = 0;

                string strJumin = "";
                string strNewData = "";
                string strOldData = "";

                string strFrDate = "";
                string strToDate = "";
                string strToDate1 = "";
                string strJong = "";
                long nLtdCode = 0;

                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;
                strToDate1 = DateTime.Parse(dtpTDate.Text).AddDays(3).ToShortDateString();

                if (rdoJong0.Checked == true)
                {
                    strJong = "0";
                }
                else if (rdoJong1.Checked == true)
                {
                    strJong = "1";
                }
                else if (rdoJong2.Checked == true)
                {
                    strJong = "2";
                }

                if (txtLtdCode.Text.Trim() != "")
                {
                    if (txtLtdCode.Text.IndexOf(".") > 0)
                    {
                        nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                    }
                    else
                    {
                        nLtdCode = 0;
                    }
                }
                else
                {
                    nLtdCode = 0;
                }

                nAmt1 = 0; nAmt2 = 0; nAmt3 = 0; nTotAmt = 0; nTot[0] = 0; nTot[1] = 0; nTot[2] = 0; nTot[3] = 0; nTot[4] = 0;

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateSDateLtdCode(strJong, strFrDate, strToDate, nLtdCode, strToDate1, "2");

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = VB.Left(clsAES.DeAES(list[i].JUMIN2), 6) + "-" + VB.Right(clsAES.DeAES(list[i].JUMIN2), 6);

                    //종검일경우
                    if (list[i].GUBUN == "1")
                    {
                        strNewData = list[i].PANO2.To<string>();
                        if (strNewData != strOldData)
                        {
                            nRow += 1;
                            SS1.ActiveSheet.RowCount = nRow;
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "종검";
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].PANO1.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(strJumin, 7) + "******";
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_GjJong_HeaName(list[i].GJJONG);
                            //종검비
                            nAmt1 = 0;
                            nAmt2 = 0;
                            nAmt3 = 0;
                            nTotAmt = heaSunapService.GetSumTotAmtByWrtno(list[i].WRTNO1);
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nTotAmt.To<string>();
                            //종검 본인부담금
                            nAmt1 = heaSunapService.GetSumBoninAmtByWrtno(list[i].WRTNO1);
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nAmt1.To<string>();
                            if (list[i].GJJONG == "11")
                            {
                                HIC_JEPSU_SUNAP list2 = hicJepsuSunapService.GetSumTotAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "1");

                                if (!list2.IsNullOrEmpty())
                                {
                                    if (list2.MIRNO1 > 0)
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 8].BackColor = Color.FromArgb(236, 236, 255);
                                    }
                                    else
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 8].BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                    nAmt2 = list2.TOTAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = string.Format("{0:###,###,###,##0}", nAmt2);
                                }
                            }
                            else
                            {
                                //암검진
                                HIC_JEPSU_SUNAP list3 = hicJepsuSunapService.GetSumTotAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "3");

                                if (!list3.IsNullOrEmpty())
                                {
                                    if (list3.MIRNO3 > 0)
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(236, 236, 255);
                                    }
                                    else
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                    nAmt3 = list3.TOTAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = string.Format("{0:###,###,###,##0}", nAmt3);
                                }
                            }
                            //등록번호가 같으면= 암검진일경우
                        }
                        else
                        {
                            //암검진
                            HIC_JEPSU_SUNAP list4 = hicJepsuSunapService.GetSumTotAmtMirNo1byWrtNo2(list[i].WRTNO2, "31", "3");

                            if (!list4.IsNullOrEmpty())
                            {
                                if (list4.MIRNO3 > 0)
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(236, 236, 255);
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                                }
                                nAmt3 = list4.JOHAPAMT;
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = string.Format("{0:###,###,###,##0}", nAmt3);
                            }
                        }
                        strOldData = list[i].PANO2.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt - (nAmt1 + nAmt2 + nAmt3));
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = string.Format("{0:###,###,###,##0}", nAmt2 + nAmt3);
                    }
                    //건진일경우
                    else if (list[i].GUBUN == "2")
                    {
                        strNewData = list[i].PANO2.To<string>();
                        if (strNewData != strOldData)
                        {
                            nRow += 1;
                            SS1.ActiveSheet.RowCount = nRow;
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "건진";
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].PANO2.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(strJumin, 7) + "******";
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                            //검진비
                            nAmt1 = 0;
                            nAmt2 = 0;
                            nAmt3 = 0;
                            nTotAmt = 0;
                            nTotAmt1 = 0;

                            nTotAmt = hicSunapService.GetTotAmtbyWrtNo2(list[i].WRTNO2);
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nTotAmt.To<string>();
                            if (list[i].GJJONG == "11")
                            {
                                //건진 사업장
                                HIC_JEPSU_SUNAP listBoninAmt = hicJepsuSunapService.GetBoninAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "1");
                                if (!listBoninAmt.IsNullOrEmpty())
                                {
                                    nAmt1 = listBoninAmt.BONINAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = string.Format("{0:###,###,###,##0}", nAmt1);
                                }

                                HIC_JEPSU_SUNAP list5 = hicJepsuSunapService.GetJohapAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "1");

                                if (!list5.IsNullOrEmpty())
                                {
                                    if (list5.MIRNO1 > 0)
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 8].BackColor = Color.FromArgb(236, 236, 255);
                                    }
                                    else
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 8].BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                    nAmt2 = list5.JOHAPAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = string.Format("{0:###,###,###,##0}", nAmt2);
                                }

                                //회사부담금
                                HIC_JEPSU_SUNAP listJohap = hicJepsuSunapService.GetLtdAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "1");
                                if (!listJohap.IsNullOrEmpty())
                                {
                                    nTotAmt1 = listJohap.LTDAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt1);
                                }
                            }                            
                            else if (list[i].GJJONG == "31")
                            {
                                HIC_JEPSU_SUNAP listBoninAmt3 = hicJepsuSunapService.GetBoninAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "3");
                                if (!listBoninAmt3.IsNullOrEmpty())
                                {
                                    nAmt1 = listBoninAmt3.BONINAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = string.Format("{0:###,###,###,##0}", nAmt1);
                                }

                                //암검진
                                HIC_JEPSU_SUNAP list12 = hicJepsuSunapService.GetJohapAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "3");

                                if (!list12.IsNullOrEmpty())
                                {
                                    if (list12.MIRNO3 > 0)
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(236, 236, 255);
                                    }
                                    else
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                    nAmt3 = list12.JOHAPAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = string.Format("{0:###,###,###,##0}", nAmt3);
                                }

                                //회사부담금
                                HIC_JEPSU_SUNAP list11 = hicJepsuSunapService.GetLtdAmtMirNo1byWrtNo2(list[i].WRTNO2, list[i].GJJONG, "3");
                                if (!list11.IsNullOrEmpty())
                                {
                                    nTotAmt1 = list11.LTDAMT;
                                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt1);
                                }
                            }
                        }
                        //두개 동시에 할경우
                        else
                        {
                            if (list[i].GJJONG == "31")
                            {
                                HIC_JEPSU_SUNAP listBoninAmt3 = hicJepsuSunapService.GetBoninAmtMirNo1byWrtNo2(list[i].WRTNO1, list[i].GJJONG, "3");
                                if (!listBoninAmt3.IsNullOrEmpty())
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nAmt1 + string.Format("{0:###,###,###,##0}", listBoninAmt3.BONINAMT);
                                    nAmt1 = listBoninAmt3.BONINAMT;
                                }

                                //암검진
                                HIC_JEPSU_SUNAP list12 = hicJepsuSunapService.GetJohapAmtMirNo1byWrtNo2(list[i].WRTNO1, list[i].GJJONG, "3");

                                if (!list12.IsNullOrEmpty())
                                {
                                    if (list12.MIRNO3 > 0)
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(236, 236, 255);
                                    }
                                    else
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                                    }
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = string.Format("{0:###,###,###,##0}", list12.JOHAPAMT);
                                    nAmt3 = list12.JOHAPAMT;
                                }

                                //회사부담금
                                HIC_JEPSU_SUNAP list11 = hicJepsuSunapService.GetLtdAmtMirNo1byWrtNo2(list[i].WRTNO1, list[i].GJJONG, "3");
                                if (!list11.IsNullOrEmpty())
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = nTotAmt1 + string.Format("{0:###,###,###,##0}", list11.LTDAMT);
                                }

                                //건진종류
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "1차+암검진";

                                //토탈금액
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = string.Format("{0:###,###,###,##0}", hicSunapService.GetTotAmtbyWrtNo2(list[i].WRTNO2));
                            }
                        }
                        strOldData = list[i].PANO2.To<string>();
                    }
                    progressBar1.Value = i + 1;
                }

                //for (int i = 0; i < nREAD; i++)
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 6].Text.Trim() != "")
                    {
                        nTot[0] += SS1.ActiveSheet.Cells[i, 6].Text.Trim().Replace(",", "").To<long>();
                    }
                    if (SS1.ActiveSheet.Cells[i, 6].Text.Trim() != "")
                    {
                        nTot[1] += SS1.ActiveSheet.Cells[i, 7].Text.Trim().Replace(",", "").To<long>();
                    }
                    if (SS1.ActiveSheet.Cells[i, 6].Text.Trim() != "")
                    {
                        nTot[2] += SS1.ActiveSheet.Cells[i, 8].Text.Trim().Replace(",", "").To<long>();
                    }
                    if (SS1.ActiveSheet.Cells[i, 6].Text.Trim() != "")
                    {
                        nTot[3] += SS1.ActiveSheet.Cells[i, 9].Text.Trim().Replace(",", "").To<long>();
                    }
                    if (SS1.ActiveSheet.Cells[i, 6].Text.Trim() != "")
                    {
                        nTot[4] += SS1.ActiveSheet.Cells[i, 10].Text.Trim().Replace(",", "").To<long>();
                    }
                }

                SS1.ActiveSheet.RowCount = nRow + 1;
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 4].Text = "합계";
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 6].Text = string.Format("{0:###,###,###,##0}", nTot[0]);
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 7].Text = string.Format("{0:###,###,###,##0}", nTot[1]);
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 8].Text = string.Format("{0:###,###,###,##0}", nTot[2]);
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 9].Text = string.Format("{0:###,###,###,##0}", nTot[3]);
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 10].Text = string.Format("{0:###,###,###,##0}", nTot[4]);
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

                strTitle = "종검 및 일반건진 청구금액 조회";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간:" + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  Page: /p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
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
