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
/// Class Name      : Hc_School
/// File Name       : frmHcSchoolStudentPhysicalDevStatic.cs
/// Description     : 학생신체발달상황 통계표
/// Author          : 이상훈
/// Create Date     : 2020-01-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool7.frm(HcSchool07)" />

namespace HC_School
{
    public partial class frmHcSchoolStudentPhysicalDevStatic : Form
    {
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHcSchoolStudentPhysicalDevStatic()
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
            hicJepsuPatientSchoolService = new HicJepsuPatientSchoolService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuExcel.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            btnPrint.Enabled = false;

            fn_Sheet_Clear();
            lblSTS.Text = "";
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
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

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

                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                fn_Sheet_Clear();
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                string strClass = "";
                string strClass1 = "";
                string strBan = "";
                string strSchName = "";
                string strSex = "";
                string strGuBun = "";
                int nCol = 0;
                long[] nCNT = new long[20];
                long[] nSum = new long[20];
                string strNewData = "";
                string strOldData = "";
                long nTemp = 0;
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));

                for (int i = 0; i < 20; i++)
                {
                    nCNT[i] = 0;
                    nSum[i] = 0;
                }

                fn_Sheet_Clear();

                if (txtLtdCode.Text.Trim() == "")
                {
                    MessageBox.Show("회사코드를 입력하셔야 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDateLtdCode(strFrDate, strToDate, nLtdCode);

                nRead = list.Count;
                if (nRead == 0)
                {
                    MessageBox.Show("자료가 없습니다. 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lblSTS.Text = nRead + " 명";
                strSchName = hb.READ_Ltd_Name(list[0].LTDCODE.ToString()).Trim();
                if (VB.L(strSchName, "초등") > 1)
                {
                    strClass = "1";
                }
                else if (VB.L(strSchName, "중학") > 1)
                {
                    strClass = "2";
                }
                else if (VB.L(strSchName, "고등") > 1)
                {
                    strClass = "3";
                }

                SS1.ActiveSheet.Cells[3, 1].Text = VB.Left(dtpFrDate.Text, 4) + " 년도";
                SS1.ActiveSheet.Cells[3, 1].Text = "학교명 : " + VB.Pstr(txtLtdCode.Text, ".", 2).Trim();

                switch (strClass)
                {
                    case "1":
                        SS1.ActiveSheet.Cells[4, 1].Text = "■초  □중  □고";
                        break;
                    case "2":
                        SS1.ActiveSheet.Cells[4, 1].Text = "□초  ■중  □고";
                        break;
                    case "3":
                        SS1.ActiveSheet.Cells[4, 1].Text = "□초  □중  ■고";
                        break;
                    default:
                        break;
                }

                //--------------------------------------------------------------------
                //이상건수 조회
                for (int i = 0; i < nRead; i++)
                {
                    strClass1 = list[i].CLASS.Trim();
                    strSex = list[i].SEX.Trim();
                    strNewData = strClass1;
                    if (i != 0 && strNewData != strOldData)
                    {
                        //학년선택 Col
                        switch (strClass)
                        {
                            case "1":
                                if (strOldData == "1")
                                {
                                    nCol = 5;
                                }
                                else if (strOldData == "4")
                                {
                                    nCol = 6;
                                }
                                break;
                            case "2":
                                nCol = 7;
                                break;
                            case "3":
                                nCol = 8;
                                break;
                            default:
                                break;
                        }

                        for (int k = 8; k <= 25; k++)
                        {
                            if (nSum[k - 8] > 0 && nCNT[k - 8] > 0)
                            {
                                switch (k)
                                {
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                        SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", nSum[k - 8] / nCNT[k - 8]);
                                        break;
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                    case 16:
                                    case 17:
                                        switch (k)
                                        {
                                            case 12:
                                            case 14:
                                            case 16:
                                                SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[4] + nSum[6] + nSum[8])) * 100);
                                                break;
                                            case 13:
                                            case 15:
                                            case 17:
                                                SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[5] + nSum[7] + nSum[9])) * 100);
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case 18:
                                    case 19:
                                    case 20:
                                    case 21:
                                    case 22:
                                    case 23:
                                    case 24:
                                    case 25:
                                        switch (k)
                                        {
                                            case 18:
                                            case 20:
                                            case 22:
                                            case 24:
                                                SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[10] + nSum[12] + nSum[14] + nSum[16])) * 100);
                                                break;
                                            case 19:
                                            case 21:
                                            case 23:
                                            case 25:
                                                SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[11] + nSum[13] + nSum[15] + nSum[17])) * 100);
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        SS1.ActiveSheet.Cells[26, nCol].Text = nCNT[18].ToString();
                        SS1.ActiveSheet.Cells[27, nCol].Text = nCNT[19].ToString();
                        SS1.ActiveSheet.Cells[28, nCol].Text = (nCNT[18] + nCNT[19]).ToString();

                        //변수초기화
                        for (int k = 0; k <= 33; k++)
                        {
                            nCNT[k] = 0;
                            nSum[k] = 0;
                        }
                    }

                    //키
                    if (list[i].PPANA1.Trim() != "")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[0] += 1;
                                nSum[0] += long.Parse(list[i].PPANA1);
                                break;
                            case "F":
                                nCNT[1] += 1;
                                nSum[1] += long.Parse(list[i].PPANA1);
                                break;
                            default:
                                break;
                        }
                    }
                    //몸무게
                    if (list[i].PPANA2.Trim() != "")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[2] += 1;
                                nSum[2] += long.Parse(list[i].PPANA2);
                                break;
                            case "F":
                                nCNT[3] += 1;
                                nSum[3] += long.Parse(list[i].PPANA2);
                                break;
                            default:
                                break;
                        }
                    }
                    //체질량지수 정상
                    if (list[i].PPANA3.Trim() == "1")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[4] += 1;
                                nSum[4] += 1;
                                break;
                            case "F":
                                nCNT[5] += 1;
                                nSum[5] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //체질량지수 비만위험군
                    if (list[i].PPANA3.Trim() == "2")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[6] += 1;
                                nSum[6] += 1;
                                break;
                            case "F":
                                nCNT[7] += 1;
                                nSum[7] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //체질량지수 비만
                    if (list[i].PPANA3.Trim() == "3")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[8] += 1;
                                nSum[8] += 1;
                                break;
                            case "F":
                                nCNT[9] += 1;
                                nSum[9] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //상대체중 정상
                    if (list[i].PPANA4.Trim() == "1")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[10] += 1;
                                nSum[10] += 1;
                                break;
                            case "F":
                                nCNT[11] += 1;
                                nSum[11] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //상대체중 경도
                    if (list[i].PPANA4.Trim() == "2")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[12] += 1;
                                nSum[12] += 1;
                                break;
                            case "F":
                                nCNT[13] += 1;
                                nSum[13] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //상대체중 중등도
                    if (list[i].PPANA4.Trim() == "3")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[14] += 1;
                                nSum[14] += 1;
                                break;
                            case "F":
                                nCNT[15] += 1;
                                nSum[15] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //상대체중 고도
                    if (list[i].PPANA4.Trim() == "4")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[16] += 1;
                                nSum[16] += 1;
                                break;
                            case "F":
                                nCNT[17] += 1;
                                nSum[17] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    //검사인원(키)
                    if (list[i].PPANA1.Trim() != "")
                    {
                        switch (strSex)
                        {
                            case "M":
                                nCNT[18] += 1;
                                break;
                            case "F":
                                nCNT[19] += 1;
                                break;
                            default:
                                break;
                        }
                    }
                    strOldData = list[i].CLASS.Trim();
                }

                //학년선택 Col
                switch (strClass)
                {
                    case "1":
                        if (strOldData == "1")
                        {
                            nCol = 5;
                        }
                        else if (strOldData == "4")
                        {
                            nCol = 6;
                        }                        
                        break;
                    case "2":
                        nCol = 7;
                        break;
                    case "3":
                        nCol = 8;
                        break;
                    default:
                        break;
                }

                for (int k = 8; k <= 25; k++)
                {
                    if (nSum[k - 8] > 0 && nCNT[k - 8] > 0)
                    {
                        switch (k)
                        {
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", nSum[k - 8] / nCNT[k - 8]);
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                            case 17:
                                switch (k)
                                {
                                    case 12:
                                    case 14:
                                    case 16:
                                        SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[4] + nSum[6] + nSum[8])) * 100);
                                        break;
                                    case 13:
                                    case 15:
                                    case 17:
                                        SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[5] + nSum[7] + nSum[9])) * 100);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 18:
                            case 19:
                            case 20:
                            case 21:
                            case 22:
                            case 23:
                            case 24:
                            case 25:
                                switch (k)
                                {
                                    case 18:
                                    case 20:
                                    case 22:
                                    case 24:
                                        SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[10] + nSum[12] + nSum[14] + nSum[16])) * 100);
                                        break;
                                    case 19:
                                    case 21:
                                    case 23:
                                    case 25:
                                        SS1.ActiveSheet.Cells[k, nCol].Text = string.Format("{0:##0.0}", (nSum[k - 8] / (nSum[11] + nSum[13] + nSum[15] + nSum[17])) * 100);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                SS1.ActiveSheet.Cells[26, nCol].Text = nCNT[18].ToString();
                SS1.ActiveSheet.Cells[27, nCol].Text = nCNT[19].ToString();
                SS1.ActiveSheet.Cells[28, nCol].Text = (nCNT[18] + nCNT[19]).ToString();

                btnPrint.Enabled = true;
            }
            else if (sender == btnMenuExcel)
            {
                bool x;
                bool y;
                string strDate = "";
                string strDir = "";
                string strMyName = "";
                string strMyPath1 = "";
                string strPathName = "";

                strMyPath1 = @"C:\";
                Cursor.Current = Cursors.WaitCursor;

                SS1.ActiveSheet.Protect = false;

                strDate = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1)) + "(" + string.Format("{0:0000}", VB.Pstr(txtLtdCode.Text, ".", 1)) + "_" + VB.Left(dtpFrDate.Text, 4) + VB.Mid(dtpFrDate.Text, 6, 2) + VB.Mid(dtpFrDate.Text, 9, 2) + "~" + VB.Left(dtpToDate.Text, 4) + VB.Mid(dtpToDate.Text, 6, 2) + VB.Mid(dtpToDate.Text, 9, 2) + "[신체발달상황]";

                if (txtLtdCode.Text.Trim() == "")
                {
                    MessageBox.Show("회사코드 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                x = SS1.SaveExcel(strMyPath1 + strDate + "_01.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
                {
                    if (x == true)
                    {
                        MessageBox.Show("C:\\" + strDate + " 파일생성 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("파일생성에 실패하였습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                SS1.ActiveSheet.Protect = true;

                Cursor.Current = Cursors.Default;
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
        }

        void fn_Sheet_Clear()
        {
            SS1.ActiveSheet.Cells[3, 1].Text = "";
            SS1.ActiveSheet.Cells[3, 7].Text = "";
            SS1.ActiveSheet.Cells[4, 1].Text = "";
            for (int i = 8; i <= 28; i++)
            {
                for (int j = 5; j <= 9; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
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
