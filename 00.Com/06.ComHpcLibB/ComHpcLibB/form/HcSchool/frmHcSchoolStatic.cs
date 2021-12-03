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
/// File Name       : frmHcSchoolStatic.cs
/// Description     : 학생건강검진 통계표(A)
/// Author          : 이상훈
/// Create Date     : 2020-01-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool4.frm(HcSchool04)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolStatic : Form
    {
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long FnDrNo;

        public frmHcSchoolStatic()
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
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
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
                int[] nCNT = new int[35];
                string strNewData  = "";
                string strOldData = "";
                long nTemp = 0;
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                for (int i = 0; i <= 34; i++)
                {
                    nCNT[i] = 0;
                }

                fn_Sheet_Clear();

                strNewData = "";
                strOldData = "";
                strClass = "";
                strClass1 = "";
                strBan = "";
                strSchName = "";
                strSex = "";
                strGuBun = "";

                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드를 입력하셔야 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLtdCode.Focus();
                    return;
                }

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDateLtdCode(strFrDate, strToDate, nLtdCode);

                nRead = list.Count;

                if (nRead == 0)
                {
                    MessageBox.Show("자료가 없습니다. 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //-------------------------기초자료읽기-----------------------------------
                lblSTS.Text = nRead + " 명";
                strSchName = hb.READ_Ltd_Name(list[0].LTDCODE.To<string>());
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

                //성별점검
                List<HIC_JEPSU_PATIENT_SCHOOL> list2 = hicJepsuPatientSchoolService.GetSexbyJepDate(strFrDate, strToDate, nLtdCode);

                if (list2.Count == 1)
                {
                    switch (list2[0].SEX)
                    {
                        case "M":
                            strSex = "■남  □여";
                            break;
                        case "F":
                            strSex = "□남  ■여";
                            break;
                        default:
                            break;
                    }
                }
                else if (list2.Count > 1)
                {
                    strSex = "■남  ■여";
                }

                SS1.ActiveSheet.Cells[3, 1].Text = VB.Left(dtpFrDate.Text, 4) + " 년도";
                SS1.ActiveSheet.Cells[3, 6].Text = "(" + VB.Pstr(txtLtdCode.Text, ".", 2) + ")";
                switch (strClass)
                {
                    case "1":
                        SS1.ActiveSheet.Cells[4, 1].Text = "■초  □중  □고" + VB.Space(5) + strSex;
                        break;
                    case "2":
                        SS1.ActiveSheet.Cells[4, 1].Text = "□초  ■중  □고" + VB.Space(5) + strSex;
                        break;
                    case "3":
                        SS1.ActiveSheet.Cells[4, 1].Text = "□초  □중  ■고" + VB.Space(5) + strSex;
                        break;
                    default:
                        break;
                }

                //--------------------------------------------------------------------
                //이상건수 조회
                for (int i = 0; i < nRead; i++)
                {
                    strClass1 = list[i].CLASS.To<string>();
                    strNewData = strClass1;
                    if (i != 0 && strNewData != strOldData)
                    {
                        //학년선택 Col
                        switch (strClass)
                        {
                            case "1":
                                if (strOldData == "1")
                                {
                                    nCol = 4;
                                    nCNT[4] = 0;    //색각이상 =0
                                }
                                else if (strOldData == "4")
                                {
                                    nCol = 5;
                                }
                                break;
                            case "2":
                                nCol = 6;
                                break;
                            case "3":
                                nCol = 7;
                                nCNT[4] = 0;    //색각이상 =0
                                break;
                            default:
                                break;
                        }

                        for (int k = 9; k <= 42; k++)
                        {
                            SS1.ActiveSheet.Cells[k - 1, nCol].Text = nCNT[k - 8].ToString();
                        }

                        //변수초기화
                        for (int k = 0; k <= 34; k++)
                        {
                            nCNT[k] = 0;
                        }
                    }

                    if (!VB.Pstr(list[i].PPANB1, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANB1, "^^", 1) != "1")
                        {
                            nCNT[1] += 1;   //근골격
                        }
                    }

                    if (!VB.Pstr(list[i].PPANC1, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list[i].PPANC1, "^^", 2).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC1, "^^", 1).To<double>() <= 0.7 || VB.Pstr(list[i].PPANC1, "^^", 2).To<double>() <= 0.7)
                        {
                            nCNT[2] += 1;   //교정대상
                        }
                    }

                    if (!VB.Pstr(list[i].PPANC2, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list[i].PPANC2, "^^", 2).IsNullOrEmpty())
                    {
                        nCNT[3] += 1;   //교정한학생
                    }
                    if (!list[i].PPANC3.IsNullOrEmpty() && !list[i].PPANC3.IsNullOrEmpty())
                    {
                        nCNT[4] += 1;   //색각이상
                    }
                    if (!VB.Pstr(list[i].PPANC4, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list[i].PPANC4, "^^", 2).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC4, "^^", 1) != "1" || VB.Pstr(list[i].PPANC4, "^^", 2) != "1" || 
                            VB.Pstr(list[i].PPANC4, "^^", 3) != "")
                        {
                            nCNT[5] += 1;   //눈병
                        }
                    }
                    if (!VB.Pstr(list[i].PPANC5, "^^", 1).IsNullOrEmpty() && !VB.Pstr(list[i].PPANC5, "^^", 2).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC5, "^^", 1) != "1" || VB.Pstr(list[i].PPANC5, "^^", 2) != "1")
                        {
                            nCNT[6] += 1;   //청력장애
                        }
                    }
                    if (!VB.Pstr(list[i].PPANC6, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC6, "^^", 1) != "1" || VB.Pstr(list[i].PPANC6, "^^", 2) != "")
                        {
                            nCNT[7] += 1;   //귓병
                        }
                    }
                    if (!VB.Pstr(list[i].PPANC7, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC7, "^^", 1) != "1" || VB.Pstr(list[i].PPANC7, "^^", 2) != "")
                        {
                            nCNT[8] += 1;   //콧병
                        }
                    }
                    if (!VB.Pstr(list[i].PPANC8, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC8, "^^", 1) != "1" || VB.Pstr(list[i].PPANC8, "^^", 2) != "")
                        {
                            nCNT[9] += 1;   //목병
                        }
                    }
                    if (!VB.Pstr(list[i].PPANC9, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANC9, "^^", 1) != "1" || VB.Pstr(list[i].PPANC9, "^^", 2) != "")
                        {
                            nCNT[10] += 1;   //피부병
                        }
                    }
                    if (!VB.Pstr(list[i].DPAN1, "^^", 1).IsNullOrEmpty() || !VB.Pstr(list[i].DPAN1, "^^", 2).IsNullOrEmpty())
                    {
                        nCNT[11] += 1;   //치아우식증
                    }
                    if (VB.Pstr(list[i].DPAN8, "^^", 1) == "1" || VB.Pstr(list[i].DPAN8, "^^", 2) == "1" ||
                        VB.Pstr(list[i].DPAN8, "^^", 3) == "1" || !VB.Pstr(list[i].DPAN8, "^^", 4).IsNullOrEmpty())
                    {
                        nCNT[12] += 1;   //치주질환
                    }
                    if (!list[i].DPAN5.IsNullOrEmpty() && list[i].DPAN5 != "1")
                    {
                        nCNT[13] += 1;   //부정교합
                    }
                    if (!VB.Pstr(list[i].PPAND1, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND1, "^^", 1) != "1")
                        {
                            nCNT[14] += 1;   //호흡기질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPAND2, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND2, "^^", 1) != "1")
                        {
                            nCNT[15] += 1;   //순환기질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPAND3, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND3, "^^", 1) != "1")
                        {
                            nCNT[16] += 1;   //비뇨기질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPAND4, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND4, "^^", 1) != "1")
                        {
                            nCNT[17] += 1;   //소화기질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPAND5, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND5, "^^", 1) != "1")
                        {
                            nCNT[18] += 1;   //신경계질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPAND6, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPAND6, "^^", 1) != "1")
                        {
                            nCNT[19] += 1;   //그밖의질환
                        }
                    }
                    if (!VB.Pstr(list[i].PPANJ1, "^^", 1).IsNullOrEmpty())
                    {
                        if (VB.Pstr(list[i].PPANJ1, "^^", 1).To<double>() >= 140 || VB.Pstr(list[i].PPANJ1, "^^", 2).To<double>() >= 90)
                        {
                            nCNT[20] += 1;   //혈압 고
                        }
                    }
                    if (!VB.Pstr(list[i].PPANJ1, "^^", 2).IsNullOrEmpty())
                    {
                        //if (double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 1)) >= 100 || double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 2)) <= 70)
                        //{
                        //    nCNT[20] += 1;   //혈압 저
                        //}
                    }
                    if (!list[i].PPANE1.IsNullOrEmpty())
                    {
                        if (list[i].PPANE1 != "1" && list[i].PPANE1 != "2")
                        {
                            nCNT[22] += 1;   //요단백
                        }
                    }
                    if (!list[i].PPANE2.IsNullOrEmpty())
                    {
                        if (list[i].PPANE2 != "1" && list[i].PPANE2 != "2")
                        {
                            nCNT[23] += 1;   //요잠혈
                        }
                    }
                    nCNT[24] += 1;                 //검사인원(전체)
                    //각종검사
                    if (!list[i].PPANF1.IsNullOrEmpty())
                    {
                        if (list[i].PPANF1.To<double>() >= 121)
                        {
                            nCNT[25] += 1;   //혈당(식전)
                        }
                    }
                    if (!list[i].PPANF2.IsNullOrEmpty())
                    {
                        if (list[i].PPANF2.To<double>() >= 251)
                        {
                            nCNT[26] += 1;   //총콜레스테롤
                        }
                    }
                    if (!list[i].PPANF3.IsNullOrEmpty())
                    {
                        if (list[i].PPANF3.To<double>() >= 51)
                        {
                            nCNT[27] += 1;   //AST GOT
                        }
                    }
                    if (!list[i].PPANF4.IsNullOrEmpty())
                    {
                        if (list[i].PPANF4.To<double>() >= 46)
                        {
                            nCNT[27] += 1;   //AST GPT
                        }
                    }
                    if (!list[i].PPANF1.IsNullOrEmpty() || !list[i].PPANF2.IsNullOrEmpty() || 
                        !list[i].PPANF3.IsNullOrEmpty() || !list[i].PPANF4.IsNullOrEmpty())
                    {
                        nCNT[28] += 1;      //검사인원(혈액1)
                    }
                    if (!list[i].PPANF5.IsNullOrEmpty())
                    {
                        nCNT[30] += 1;      //검사인원(혈액2)
                        if (list[i].PPANF5.To<double>() < 10)
                        {
                            nCNT[29] += 1;  //혈색소
                        }
                    }
                    if (list[i].PPANH1 == "1" || list[i].PPANH1 == "2")
                    {
                        nCNT[32] += 1;      //검사인원(B형간염)

                        if (list[i].PPANH1 == "2")
                        {
                            nCNT[31] += 1;  //B형간염
                        }
                    }
                    if (!VB.Pstr(list[i].PPANG1, "^^", 1).IsNullOrEmpty() && VB.Pstr(list[i].PPANG1, "^^", 1) != "11")
                    {
                        nCNT[34] += 1;
                        if (list[i].PPANG1.To<double>() >= 4 && list[i].PPANG1.To<double>() <= 7)
                        {
                            nCNT[33] += 1;  //흉부방사선
                        }
                    }
                    strOldData = list[i].CLASS.To<string>();
                }

                //학년선택 Col
                switch (strClass)
                {
                    case "1":
                        if (strOldData == "1")
                        {
                            nCol = 4;
                            nCNT[4] = 0;    //색각이상 =0
                        }
                        else if (strOldData == "4")
                        {
                            nCol = 5;
                        }
                        break;
                    case "2":
                        nCol = 6;
                        break;
                    case "3":
                        nCol = 7;
                        nCNT[4] = 0; //색각이상 =0
                        break;
                    default:
                        break;
                }

                for (int i = 9; i <= 42; i++)
                {
                    SS1.ActiveSheet.Cells[i - 1, nCol].Text = nCNT[i - 8].ToString();
                }

                if (strClass == "1")
                {
                    for (int i = 8; i <= 41; i++)
                    {
                        nTemp = 0;
                        nTemp = SS1.ActiveSheet.Cells[i, 4].Text.To<long>();
                        nTemp += SS1.ActiveSheet.Cells[i, 5].Text.To<long>();
                        SS1.ActiveSheet.Cells[i, 8].Text = nTemp.To<string>();
                    }
                }
                btnPrint.Enabled = true;
                fn_Sheet_Clear_Etc();
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

                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, false);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
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

                strDate = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1)) + "(" + string.Format("{0:0000}", VB.Pstr(txtLtdCode.Text, ".", 1)) + "_" + VB.Left(dtpFrDate.Text, 4) + VB.Mid(dtpFrDate.Text, 6, 2) + VB.Mid(dtpFrDate.Text, 9, 2) + "[통계표]";

                if (txtLtdCode.Text.IsNullOrEmpty())
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

        void fn_Sheet_Clear()
        {
            SS1.ActiveSheet.Cells[3, 1].Text = "";
            SS1.ActiveSheet.Cells[3, 6].Text = "";
            SS1.ActiveSheet.Cells[4, 1].Text = "";
            for (int i = 8; i <= 41; i++)
            {
                for (int j = 4; j <= 8; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }
            SS1.ActiveSheet.Cells[11, 4].Text = "-";
            SS1.ActiveSheet.Cells[11, 7].Text = "-";
        }

        void fn_Sheet_Clear_Etc()
        {
            SS1.ActiveSheet.Cells[11, 4].Text = "-";
            SS1.ActiveSheet.Cells[11, 7].Text = "-";
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
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
