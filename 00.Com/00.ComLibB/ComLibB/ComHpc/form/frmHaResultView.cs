using ComBase;
using ComBase.Controls;
using ComLibB;
using ComLibB.Dto;
using ComLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaResultView.cs
/// Description     : 개인별 검사결과 조회
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain21.frm(FrmResultView)" />

namespace ComLibB
{
    public partial class frmHaResultView : Form
    {
        //HeaJepsuService heaJepsuService = null;
        //HicResultExCodeService hicResultExCodeService = null;

        ComHpcService comHpcService = null;


        clsSpread sp = new clsSpread();
        //clsHcMain hm = new clsHcMain();
        //clsHaBase hb = new clsHaBase();
        //clsHcFunc hc = new clsHcFunc();
        //clsHaBase ha = new clsHaBase();
        clsComHpc ch = new clsComHpc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;

        public frmHaResultView(long nWrtno)
        {
            InitializeComponent();

            FnWRTNO = nWrtno;

            SetEvent();
        }

        void SetEvent()
        {
            //heaJepsuService = new HeaJepsuService();
            //hicResultExCodeService = new HicResultExCodeService();

            comHpcService = new ComHpcService();


            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtWrtNo.Text = "";
            if (FnWRTNO != 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>();
                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (txtWrtNo.Text.Trim() == "") return;

                if (e.KeyChar == 13)
                {
                    fn_Screen_Display();
                }
            }
        }

        void fn_Screen_Clear()
        {
            FnWRTNO = 0;
            txtWrtNo.Text = "";
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 40;
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            int nCol = 0;

            long nPano = 0;
            string strSDate = "";
            string strCODE = "";

            string strSEX = "";
            string strPart = "";
            string strExcode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJEPDATE = "";
            string strExamDate = "";
            string strGJJONG = "";
            long nLicense = 0;
            string strDrname = "";
            int nAscii = 0;
            int nHyelH = 0;
            int nHyelL = 0;
            int nHEIGHT = 0;
            int nWeight = 0;

            string strAllResult = "";

            double nMaxData = 0; //정상 참고치 (High)
            double nMinData = 0; //정상 참고치 (Low)
            double nResult = 0; //검사결과
            int ii = 0;
            int nSubREAD = 0;
            long nWRTNO = 0;
            string strOK = "";

            FnWRTNO = txtWrtNo.Text.To<long>();
            SS2.ActiveSheet.RowCount = 50;
            txtResult1.Text = "";
            txtResult2.Text = "";
            txtResult3.Text = "";
            txtResult4.Text = "";

            //tabItem1.Text = "";
            tabItem2.Text = "";
            tabItem3.Text = "";
            tabItem4.Text = "";

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = " ";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = " ";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = " ";

            //인적사항을 Display  Screen_Injek_display
            int nDrSabun = 0;

            //HEA_JEPSU list = heaJepsuService.GetItembyWrtNo(FnWRTNO);

            COMHPC list = comHpcService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWrtNo.Focus();
                return;
            }

            strAllResult = "";
            string sHipen = new String('-', 60);


            if (!list.PANREMARK.IsNullOrEmpty())
            {
                strAllResult = "◈ 판정결과(" + list.DRNAME + ") ◈" + "\r\n";
                strAllResult += sHipen + "\r\n";
                strAllResult += list.PANREMARK + "\r\n\r\n";
            }

            strSEX = list.SEX;
            nPano = list.PANO;
            strSDate = list.SDATE.To<string>();

            ssPano.ActiveSheet.Cells[0, 0].Text = list.PTNO.To<string>();
            ssPano.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPano.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + strSEX;
            ssPano.ActiveSheet.Cells[0, 3].Text = ch.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPano.ActiveSheet.Cells[0, 4].Text = list.SDATE.To<string>();
            ssPano.ActiveSheet.Cells[0, 5].Text = ch.READ_GjJong_HeaName(list.GJJONG);

            //검사항목을 Display  Screen_Exam_Items_display
            //List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoResult(FnWRTNO);
            //List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItemHeaNoActingbyWrtNo(FnWRTNO);

            List<COMHPC> list2 = comHpcService.GetItemHeaNoActingbyWrtNo(FnWRTNO);
            
            nREAD = list2.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            for (int i = 0; i < nREAD; i++)
            {
                strExcode = list2[i].EXCODE;
                strResult = list2[i].RESULT;
                strResCode = list2[i].RESCODE;
                strResultType = list2[i].RESULTTYPE;
                strGbCodeUse = list2[i].GBCODEUSE;

                if (strResultType == "3")
                {
                    strAllResult += "◈" + list2[i].HNAME + "◈" + "\r\n";
                    strAllResult += sHipen + "\r\n";
                    strAllResult += list2[i].RESULT + "\r\n\r\n";
                }
                else
                {
                    nRow += 1;
                    if (nRow > SS2.ActiveSheet.RowCount)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = list2[i].EXCODE;
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].HNAME;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strResult;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(190, 250, 220);
                    if (!strResult.IsNullOrEmpty() && !strResCode.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 2].Text = ch.READ_ResultName(strResCode, strResult);
                    }
                    if (list2[i].PANJENG == "2")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 3].Text = "*";
                    }

                    //참고치를 Dispaly
                    if (ch.Check_ReferValue_ChangeCode(strExcode) == true)
                    {
                        strNomal = ch.GET_Refer_Value(strExcode, strSEX, strSDate, "N");
                        nMinData = VB.Pstr(strNomal, "~", 1).To<double>();
                        nMaxData = VB.Pstr(strNomal, "~", 2).To<double>();
                    }
                    else
                    {
                        if (strSEX == "M")
                        {
                            strNomal = list2[i].MIN_M + "~" + list2[i].MAX_M;
                            nMinData = list2[i].MIN_M.To<double>();
                            nMaxData = list2[i].MAX_M.To<double>();
                        }
                        else
                        {
                            strNomal = list2[i].MIN_F + "~" + list2[i].MAX_F;
                            nMinData = list2[i].MIN_F.To<double>();
                            nMaxData = list2[i].MAX_F.To<double>();
                        }
                    }

                    if (nMinData != 0 || nMaxData != 0)
                    {
                        switch (ch.Result_Panjeng(list2[i].EXCODE, strResult, strNomal))
                        {
                            case "L":
                                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                                break;
                            case "H":
                                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        strNomal = "";
                    }

                    if (strNomal == "~")
                    {
                        strNomal = "";
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = strNomal;
                }
            }

            if (!strAllResult.IsNullOrEmpty())
            {
                strAllResult = strAllResult.Replace("\n", "\r\n");
                txtResult1.Text = strAllResult;
            }
            else
            {
                txtResult1.Text = "";
            }
            SS2.ActiveSheet.RowCount = nRow;

            //종전결과 3개를 Display
            //List<HEA_JEPSU> list3 = heaJepsuService.GetItembyPaNoSDate(nPano, strSDate);
            List<COMHPC> list3 = comHpcService.GetItembyPaNoSDate(nPano, strSDate);

            nSubREAD = list3.Count;
            for (int i = 0; i < list3.Count; i++)
            {
                if (i > 2)
                {
                    break;
                }

                strExamDate = list3[i].SDATE;
                //tabControl1.SelectedTabIndex = i + 1;
                if (i == 0)
                {
                    tabItem2.Text = VB.Left(strExamDate, 7);
                }
                else if (i == 1)
                {
                    tabItem3.Text = VB.Left(strExamDate, 7);
                }
                else if (i == 2)
                {
                    tabItem4.Text = VB.Left(strExamDate, 7);
                }

                strAllResult = "";
                if (list3[i].PANREMARK != "")
                {
                    strAllResult = "◈ 판정결과(" + list3[i].DRNAME + ") ◈" + "\r\n";
                    strAllResult += sHipen + "\r\n";
                    strAllResult += list3[i].PANREMARK + "\r\n\r\n";
                }

                nCol = 5 + i;
                SS2_Sheet1.ColumnHeader.Cells.Get(0, nCol).Value = VB.Left(strExamDate, 7);
                nWRTNO = list3[i].WRTNO;

                //검사항목을 Display Screen_Exam_Items_OLD(종전결과)
                //List<HIC_RESULT_EXCODE> list4 = hicResultExCodeService.GetItembyWrtNoResultHea(nWRTNO);

                List<COMHPC> list4 = comHpcService.GetItembyWrtNoResultHea(nWRTNO);

                nREAD = list4.Count;
                nRow = 0;
                for (int k = 0; k < nREAD; k++)
                {
                    strCODE = list4[k].EXCODE;
                    strResult = list4[k].RESULT;
                    strResCode = list4[k].RESCODE;
                    strResultType = list4[k].RESULTTYPE;
                    strGbCodeUse = list4[k].GBCODEUSE;

                    if (strResultType == "3")
                    {
                        strAllResult += "◈" + list4[k].HNAME + "◈" + "\r\n";
                        strAllResult += sHipen + "\r\n";
                        strAllResult += list4[k].RESULT + "\r\n\r\n";
                    }
                    else
                    {
                        nRow = 0;
                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 0].Text == strCODE)
                            {
                                nRow = j;
                                break;
                            }
                        }

                        strOK = "";
                        if (nRow == 0 && !SS2.ActiveSheet.Cells[nRow, nCol].Text.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }


                        if (nRow >= 0 && strOK == "")
                        {
                            SS2.ActiveSheet.Cells[nRow, nCol].Text = strResult;
                            SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(190, 250, 220);
                            if (!strResult.IsNullOrEmpty() && !strResCode.IsNullOrEmpty())
                            {
                                SS2.ActiveSheet.Cells[nRow, nCol].Text = ch.READ_ResultName(strResCode, strResult);
                            }

                            //참고치를 Dispaly
                            if (ch.Check_ReferValue_ChangeCode(strCODE) == true)
                            {
                                strNomal = ch.GET_Refer_Value(strCODE, strSEX, strExamDate, "N");
                                nMinData = VB.Pstr(strNomal, ",", 1).To<double>();
                                nMaxData = VB.Pstr(strNomal, ",", 2).To<double>();
                            }
                            else
                            {
                                if (strSEX == "M")
                                {
                                    strNomal = list4[k].MIN_M + "~" + list4[k].MAX_M;
                                    nMinData = list4[k].MIN_M.To<double>();
                                    nMaxData = list4[k].MAX_M.To<double>();
                                }
                                else
                                {
                                    strNomal = list4[k].MIN_M + "~" + list4[k].MAX_M;
                                    nMinData = list4[k].MIN_M.To<double>();
                                    nMaxData = list4[k].MAX_M.To<double>();
                                }
                            }

                            if (nMinData != 0 || nMaxData != 0)
                            {
                                switch (ch.Result_Panjeng(list4[k].EXCODE, strResult, strNomal))
                                {
                                    case "L":
                                        SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    case "H":
                                        SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                strNomal = "";
                            }
                        }
                    }
                }

                if (!strAllResult.IsNullOrEmpty())
                {
                    strAllResult = strAllResult.Replace("\n", "\r\n");
                    if (i == 0)
                    {
                        txtResult2.Text = strAllResult;
                    }
                    else if (i == 1)
                    {
                        txtResult3.Text = strAllResult;
                    }
                    else if (i == 2)
                    {
                        txtResult4.Text = strAllResult;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        txtResult2.Text = "";
                    }
                    else if (i == 1)
                    {
                        txtResult3.Text = "";
                    }
                    else if (i == 2)
                    {
                        txtResult4.Text = "";
                    }
                }
            }
            tabControl1.SelectedTab = tabItem1;
        }
    }
}
