using ComBase;
using ComLibB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;
using System.IO;
using System.Linq;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHeaResult.cs
/// Description     : 종합검진 검사결과 조회
/// Author          : 이상훈
/// Create Date     : 2020-02-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHeaResult.frm(FrmHeaResult)" />

namespace ComHpcLibB
{
    public partial class frmHeaResult : Form
    {
        HicPatientService hicPatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnPano;
        long FnAge;
        string FstrSex;
        string FstrPano;
        string FstrJumin;

        public frmHeaResult(string sJumin)
        {
            InitializeComponent();
            FstrJumin = sJumin;
            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();
            heaJepsuService = new HeaJepsuService();
            hicResultExCodeService = new HicResultExCodeService(); 

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS2_Sheet1.Columns[7].Visible = false;  //검사코드

            if (!FstrJumin.IsNullOrEmpty())
            {
                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            int nCol = 0;
            long nPano = 0;
            string strSDate = "";
            string strCODE = "";
            string strSex = "";
            string strPart = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
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
            string strNormal = "";  //참고치(음성/양성)
            string strExCode = "";  //검사실코드
            int ii = 0;
            int nSubREAD = 0;
            long nWRTNO = 0;
            long nDrSabun = 0;
            string strExcode = "";
            string strSexNew = "";

            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;
            txtResult1.Text = "";
            txtResult20.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            tab2.Text = "";
            tab3.Text = "";
            tab4.Text = "";

            SS2_Sheet1.Columns.Get(5).Label = " ";
            SS2_Sheet1.Columns.Get(6).Label = " ";
            SS2_Sheet1.Columns.Get(7).Label = " ";

            //주민등록 번호로 최근의 접수일자 및 검진등록번호 찾기
            nPano = hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)).To<long>();

            //GoSub Screen_Injek_display       '인적사항을 Display
            List<HEA_JEPSU> list = heaJepsuService.GetItembyPaNo(nPano);

            if (list.Count == 0)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FnWRTNO = list[0].WRTNO;
            FstrSex = list[0].SEX;
            FnAge = list[0].AGE;
            FnPano = nPano;
            strSDate = list[0].SDATE.To<string>();

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list[0].PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list[0].SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list[0].AGE + "/" + list[0].SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list[0].LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list[0].SDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_HeaName(list[0].GJJONG);
            ssPatInfo.ActiveSheet.Cells[0, 6].Text = list[0].PTNO;
            if (!list[0].PANREMARK.IsNullOrEmpty())
            {
                strAllResult = list[0].PANREMARK;
                strAllResult = strAllResult.Replace("\r\n", "\r");
                strAllResult = strAllResult.Replace("\r", "\r\n");
            }
            txtPanjeng.Text = strAllResult;
            nDrSabun = list[0].DRSABUN;

            //GoSub Screen_Exam_Items_display  '검사항목을 Display
            strSexNew = "";
            tabControl1.SelectedTab = tab1;
            tab1.Text = VB.Left(strSDate, 4) + "/" + VB.Mid(strSDate, 6, 2);
            SS2_Sheet1.Columns.Get(1).Label = VB.Left(strSDate, 4) + "/" + VB.Mid(strSDate, 6, 2);

            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItemHeaNoActingbyWrtNo(FnWRTNO);

            nREAD = list3.Count;
            nRow = 0;
            strAllResult = "";
            //SS2.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strCODE = list3[i].EXCODE;
                strResult = list3[i].RESULT;
                strResCode = list3[i].RESCODE;
                strResultType = list3[i].RESULTTYPE;
                strGbCodeUse = list3[i].GBCODEUSE;
                strExcode = list3[i].CODE;

                if (strResultType == "3")
                {
                    strAllResult += "▷" + list3[i].HNAME + ": ";
                    strAllResult += list3[i].RESULT + "\r\n";
                }
                else
                {
                    nRow += 1;
                    if (nRow > SS2.ActiveSheet.RowCount) SS2.ActiveSheet.RowCount = nRow;

                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = list3[i].HNAME;
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strResult;
                    SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(190, 250, 220);

                    if (!strResCode.IsNullOrEmpty() && !strResCode.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_ResultName(strResCode, strResult);
                    }

                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = strCODE;

                    //참고치를 Dispaly
                    if (FstrSex == "M")
                    {
                        strNomal = list3[i].MIN_M + "~" + list3[i].MAX_M;
                        nMinData = list3[i].MIN_M.To<double>();
                        nMaxData = list3[i].MAX_M.To<double>();
                    }
                    else
                    {
                        strNomal = list3[i].MIN_F + "~" + list3[i].MAX_F;
                        nMinData = list3[i].MIN_F.To<double>();
                        nMaxData = list3[i].MAX_F.To<double>();
                    }

                    if (nMinData != 0 || nMaxData != 0)
                    {
                        if (!strResult.IsNullOrEmpty() && strResult != ".")
                        {
                            //switch (hb.Result_Panjeng_New(list3[i].EXCODE, strResult, strNomal))
                            switch (fn_Result_Panjeng(list3[i].EXCODE, strResult, strNomal))
                            {
                                case "L":
                                    SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(250, 210, 222);
                                    break;
                                case "H":
                                    SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(250, 210, 222);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else if ((VB.Left(strResult, 2) == "양성" || VB.Left(strResult, 1) == "+") && strCODE.Trim() != "H841" && strCODE.Trim() != "A132" && strCODE.Trim() != "A259")
                    {
                        //소변 및 대변 검사
                        switch (strResult.Trim())
                        {
                            case "양성":
                            case "+":
                            case "++":
                            case "+++":
                            case "++++":
                            case "+++++":
                                SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(250, 210, 222);
                                break;
                            case "+-":
                                if (strCODE.Trim() != "LU46" && strCODE.Trim() != "A259")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(250, 210, 222);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (strNomal == "~")
                    {
                        strNomal = "";
                    }

                    if (strNomal == "음성(-)~")
                    {
                        strNomal = "음성(-)";
                    }

                    if (strNomal == "음성~")
                    {
                        strNomal = "음성";
                    }

                    if (strNomal == "정상~")
                    {
                        strNomal = "정상";
                    }

                    //정상,이상으로 결과값이 나올때
                    if (strNomal == "정상" && !strResult.IsNullOrEmpty() && strResult.Trim() != ".")
                    {
                        if (strNomal.Trim() != strResult.Trim())
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                        }
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strNomal;
                }
            }

            if (!strAllResult.IsNullOrEmpty())
            {
                strAllResult = strAllResult.Replace("\r", "\r\n");
                strAllResult = strAllResult.Replace("\n", "\r\n");                
                txtResult1.Text = strAllResult;
            }
            else
            {
                txtResult1.Text = "";
            }
            SS2.ActiveSheet.RowCount = nRow;

            //종전결과 3개를 Display
            List<HEA_JEPSU> list4 = heaJepsuService.GetItembyPaNoSDate(nPano, strSDate, "Y");

            nSubREAD = list4.Count;
            for (int i = 0; i < nSubREAD; i++)
            {
                if (i > 2)
                {
                    break;
                }

                strJepDate = list4[i].SDATE;
                tabControl1.SelectedTabIndex = i + 1;
                tabControl1.SelectedTab.Text = VB.Left(list4[i].SDATE, 7);

                strAllResult = "";
                if (!list4[i].PANREMARK.IsNullOrEmpty())
                {
                    strAllResult = "◈ 판정결과(" + list4[i].DRNAME + ") ◈" + "\r\n";
                    strAllResult += string.Concat(Enumerable.Repeat("-", 60)) + "\r\n";
                    strAllResult += list4[i].PANREMARK + "\r\n\r\n";
                }
                nCol = i + 3;
                if (!strJepDate.IsNullOrEmpty())
                {
                    SS2_Sheet1.ColumnHeader.Cells.Get(0, nCol).Text = strJepDate.Replace("-", "");
                }
                nWRTNO = list4[i].WRTNO;

                //검사항목을 Display
                //Screen_Exam_Items_OLD();  //종전결과
                List<HIC_RESULT_EXCODE> list5 = hicResultExCodeService.GetItemHeabyWrtNo(nWRTNO);

                nREAD = list5.Count;
                nRow = 0;
                for (int j = 0; j < nREAD; j++)
                {
                    strCODE = list5[j].EXCODE;
                    strResult = list5[j].RESULT;
                    strResCode = list5[j].RESCODE;
                    strResultType = list5[j].RESULTTYPE;
                    strGbCodeUse = list5[j].GBCODEUSE;

                    if (strResultType == "3")
                    {
                        strAllResult += "▷" + list5[j].HNAME + ": ";
                        strAllResult += list5[j].RESULT + "\r\n";
                    }
                    else
                    {
                        nRow = 0;
                        for (int k = 1; k <= SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k - 1, 7].Text == strCODE)
                            {
                                nRow = k;
                                break;
                            }
                        }

                        if (nRow > 0)
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, nCol].Text = strResult;
                            SS2.ActiveSheet.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(190, 250, 220);
                            if (!strResult.IsNullOrEmpty() && !strResCode.IsNullOrEmpty())
                            {
                                SS2.ActiveSheet.Cells[nRow - 1, nCol].Text = hb.READ_ResultName(strResCode, strResult);
                            }

                            //참고치를 Dispaly
                            strNomal = fn_HEA_NomalValue_SET(strCODE, strJepDate, FstrSex, list5[j].MIN_M.To<double>(), list5[j].MAX_M.To<double>(), list5[j].MIN_F.To<double>(), list5[j].MAX_F.To<double>());
                            nMinData = VB.Pstr(strNomal, "~", 1).To<double>();
                            nMaxData = VB.Pstr(strNomal, "~", 2).To<double>();

                            if (nMinData != 0 || nMaxData != 0)
                            {
                                //switch (hb.Result_Panjeng_New(list5[j].EXCODE, strResult, strNomal))
                                switch (fn_Result_Panjeng(list5[j].EXCODE, strResult, strNomal))
                                {
                                    case "L":
                                        SS2.ActiveSheet.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    case "H":
                                        SS2.ActiveSheet.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if ((VB.Left(strResult, 2) == "양성" || VB.Left(strResult, 1) == "+") && strCODE.Trim() != "H841" && strCODE.Trim() != "A132" && strCODE.Trim() != "A259")
                            {
                                //소변 및 대변 검사
                                switch (strResult.Trim())
                                {
                                    case "양성":
                                    case "+":
                                    case "++":
                                    case "+++":
                                    case "++++":
                                    case "+++++":
                                        SS2.ActiveSheet.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    case "+-":
                                        if (strCODE.Trim() != "LU46" && strCODE.Trim() != "A259")
                                        {
                                            SS2.ActiveSheet.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                TextBox txtResult2 = (Controls.Find("txtResult2" + i.ToString(), true)[0] as TextBox);
                if (!strAllResult.IsNullOrEmpty())
                {
                    strAllResult = strAllResult.Replace("\n", "\r\n");
                    strAllResult = strAllResult.Replace("\r", "\r\n");
                    txtResult2.Text = strAllResult;
                }
                else
                {   
                    txtResult2.Text = "";
                }
            }

            tabControl1.SelectedTab = tab1;
        }

        /// <summary>
        /// 종합검진 검사결과 판정(L=Low,H=High,"":Nomal)
        /// </summary>
        /// <param name="argExCode"></param>
        /// <param name="argResult"></param>
        /// <param name="argNomal"></param>
        /// <returns></returns>
        string fn_Result_Panjeng(string argExCode, string argResult, string argNomal)
        {
            string rtnVal = "";
            double nMinValue = 0;
            double nMaxValue = 0;
            double nResult = 0;

            if (argResult.IsNullOrEmpty() || argNomal.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            if (VB.L(argNomal, "~") < 2)
            {
                rtnVal = "";
                return rtnVal;
            }

            nMinValue = VB.Pstr(argNomal, "~", 1).To<double>();
            nMaxValue = VB.Pstr(argNomal, "~", 2).To<double>();
            if (nMinValue == 0 && nMaxValue == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nResult = argResult.To<double>();

            //요침사 현미경검사
            switch (argExCode.Trim())
            {
                case "A271":
                    if (argResult.Trim() != "0-2")
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                case "A272":
                    if (argResult.Trim() != "0-5")
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                default:
                    break;
            }

            //소변 및 대변 검사
            switch (argResult)
            {
                case "음성":
                case "-":
                    rtnVal = "";
                    return rtnVal;
                case "양성":
                    rtnVal = "L";
                    return rtnVal;
                case "+-":
                    if (argExCode.Trim() == "LU46" && argExCode.Trim() == "A259")
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                default:
                    break;
            }

            rtnVal = "";
            if (nResult < nMinValue)
            {
                rtnVal = "L";   //Low
            }
            else if (nResult > nMaxValue)
            {
                rtnVal = "H";   //High
            }
            else
            {
                rtnVal = "";    //Nomal 또는 점검불능
            }

            return rtnVal;
        }

        string fn_HEA_NomalValue_SET(string argExCode, string argJepDate, string argSex, double argMinM, double argMaxM, double argMinF, double argMaxF)
        {
            string rtnVal = "";
            string strNomal = "";

            if (argExCode.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("EXAM_NomalValue_SET:ArgExCode 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return rtnVal;
            }
            if (argJepDate.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("EXAM_NomalValue_SET:ArgJepDate 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return rtnVal;
            }
            if (argSex.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("EXAM_NomalValue_SET:ArgSex 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return rtnVal;
            }

            //특정 검사코드 참고치 강제설정
            strNomal = "";

            switch (argExCode)
            {
                case "A124":
                    strNomal = "7~38";
                    break;
                case "A125":
                    strNomal = "7~43";
                    break;
                case "A126":
                    strNomal = argSex == "M" ? "12~73" : "8~48";
                    break;
                case "A274":
                    strNomal = "0.6~1.2";
                    break;
                default:
                    break;
            }

            if (!strNomal.IsNullOrEmpty())
            {
                rtnVal = strNomal;
                return rtnVal;
            }

            //검사코드의 참고치를 Return
            if (argSex == "M")
            {
                strNomal = argMinM + "~" + argMaxM;
            }
            else
            {
                strNomal = argMinF + "~" + argMaxF;
            }

            if (strNomal == "~")
            {
                strNomal = "";
            }

            if (!strNomal.IsNullOrEmpty())
            {
                if (VB.Right(strNomal, 1) == "~")
                {
                    strNomal = VB.Left(strNomal, strNomal.Length - 1);
                }
            }

            rtnVal = strNomal;
            return rtnVal;
        }
    }
}
