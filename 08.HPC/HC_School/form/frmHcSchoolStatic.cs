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
/// File Name       : frmHcSchoolStatic.cs
/// Description     : 학생건강검진 통계표(A)
/// Author          : 이상훈
/// Create Date     : 2020-01-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool4.frm(HcSchool04)" />

namespace HC_School
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
                int[] nCNT = new int[34];
                string strNewData  = "";
                string strOldData = "";
                long nTemp = 0;
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));

                for (int i = 0; i <= 33; i++)
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

                if (txtLtdCode.Text.Trim() == "")
                {
                    MessageBox.Show("회사코드를 입력하셔야 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                strSchName = hb.READ_Ltd_Name(list[0].LTDCODE.ToString());
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
                    switch (list2[0].SEX.Trim())
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
                SS1.ActiveSheet.Cells[3, 5].Text = "(" + VB.Pstr(txtLtdCode.Text, ".", 2) + ")";
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
                    strClass1 = list2[i].CLASS.Trim();
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
                                    nCNT[3] = 0;    //색각이상 =0
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
                                nCNT[3] = 0;    //색각이상 =0
                                break;
                            default:
                                break;
                        }

                        for (int k = 8; k <= 41; k++)
                        {
                            SS1.ActiveSheet.Cells[k, nCol].Text = nCNT[k - 8].ToString();
                        }

                        //변수초기화
                        for (int k = 0; k <= 33; k++)
                        {
                            nCNT[k] = 0;
                        }
                    }

                    if (VB.Pstr(list[i].PPANB1.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPANB1.Trim(), "^^", 1) != "1")
                        {
                            nCNT[0] += 1;   //근골격
                        }
                    }

                    if (VB.Pstr(list[i].PPANC1.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANC1.Trim(), "^^", 2) != "")
                    {
                        if (double.Parse(VB.Pstr(list[i].PPANC1, "^^", 1)) <= 0.7 || double.Parse(VB.Pstr(list[i].PPANC1, "^^", 2)) <= 0.7)
                        {
                            nCNT[1] += 1;   //교정대상
                        }
                    }

                    if (VB.Pstr(list[i].PPANC2.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANC2.Trim(), "^^", 2) != "")
                    {
                        nCNT[2] += 1;   //교정한학생
                    }
                    if (VB.Pstr(list[i].PPANC3.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANC3.Trim(), "^^", 2) != "")
                    {
                        nCNT[3] += 1;   //색각이상
                    }
                    if (VB.Pstr(list[i].PPANC4.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANC4.Trim(), "^^", 2) != "")
                    {
                        if (VB.Pstr(list[i].PPANC4.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC4.Trim(), "^^", 2) != "1" || 
                            VB.Pstr(list[i].PPANC4.Trim(), "^^", 3) != "1")
                        {
                            nCNT[4] += 1;   //눈병
                        }
                    }
                    if (VB.Pstr(list[i].PPANC5.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANC5.Trim(), "^^", 2) != "")
                    {
                        if (VB.Pstr(list[i].PPANC5.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC5.Trim(), "^^", 2) != "1")
                        {
                            nCNT[5] += 1;   //청력장애
                        }
                    }
                    if (VB.Pstr(list[i].PPANC6.Trim(), "^^", 1) != "" )
                    {
                        if (VB.Pstr(list[i].PPANC6.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC6.Trim(), "^^", 2) != "1")
                        {
                            nCNT[6] += 1;   //귓병
                        }
                    }
                    if (VB.Pstr(list[i].PPANC7.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPANC7.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC7.Trim(), "^^", 2) != "1")
                        {
                            nCNT[7] += 1;   //콧병
                        }
                    }
                    if (VB.Pstr(list[i].PPANC8.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPANC8.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC8.Trim(), "^^", 2) != "1")
                        {
                            nCNT[8] += 1;   //목병
                        }
                    }
                    if (VB.Pstr(list[i].PPANC9.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPANC9.Trim(), "^^", 1) != "1" || VB.Pstr(list[i].PPANC9.Trim(), "^^", 2) != "1")
                        {
                            nCNT[9] += 1;   //피부병
                        }
                    }
                    if (VB.Pstr(list[i].DPAN1.Trim(), "^^", 1) != "" || VB.Pstr(list[i].DPAN1.Trim(), "^^", 2) != "")
                    {
                        nCNT[10] += 1;   //치아우식증
                    }
                    if (VB.Pstr(list[i].DPAN8.Trim(), "^^", 1) == "1" || VB.Pstr(list[i].DPAN8.Trim(), "^^", 2) == "1" ||
                        VB.Pstr(list[i].DPAN8.Trim(), "^^", 3) == "1" || VB.Pstr(list[i].DPAN8.Trim(), "^^", 4) != "")
                    {
                        nCNT[11] += 1;   //치주질환
                    }
                    if (VB.Pstr(list[i].DPAN5.Trim(), "^^", 1) != "" && VB.Pstr(list[i].DPAN5.Trim(), "^^", 2) != "1")
                    {
                        nCNT[12] += 1;   //부정교합
                    }
                    if (VB.Pstr(list[i].PPAND1.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND1.Trim(), "^^", 1) != "1")
                        {
                            nCNT[13] += 1;   //호흡기질환
                        }
                    }
                    if (VB.Pstr(list[i].PPAND2.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND2.Trim(), "^^", 1) != "1")
                        {
                            nCNT[14] += 1;   //순환기질환
                        }
                    }
                    if (VB.Pstr(list[i].PPAND3.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND3.Trim(), "^^", 1) != "1")
                        {
                            nCNT[15] += 1;   //비뇨기질환
                        }
                    }
                    if (VB.Pstr(list[i].PPAND4.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND4.Trim(), "^^", 1) != "1")
                        {
                            nCNT[16] += 1;   //소화기질환
                        }
                    }
                    if (VB.Pstr(list[i].PPAND5.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND5.Trim(), "^^", 1) != "1")
                        {
                            nCNT[17] += 1;   //신경계질환
                        }
                    }
                    if (VB.Pstr(list[i].PPAND6.Trim(), "^^", 1) != "")
                    {
                        if (VB.Pstr(list[i].PPAND6.Trim(), "^^", 1) != "1")
                        {
                            nCNT[18] += 1;   //그밖의질환
                        }
                    }
                    if (VB.Pstr(list[i].PPANJ1.Trim(), "^^", 1) != "")
                    {
                        if (double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 1)) >= 140 || double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 2)) >= 90)
                        {
                            nCNT[19] += 1;   //혈압 고
                        }
                    }
                    if (VB.Pstr(list[i].PPANJ1.Trim(), "^^", 2) != "")
                    {
                        //if (double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 1)) >= 100 || double.Parse(VB.Pstr(list[i].PPANJ1, "^^", 2)) <= 70)
                        //{
                        //    nCNT[20] += 1;   //혈압 저
                        //}
                    }
                    if (list[i].PPANE1.Trim() != "")
                    {
                        if (list[i].PPANE1.Trim() != "1" && list[i].PPANE1.Trim() != "2")
                        {
                            nCNT[21] += 1;   //요단백
                        }
                    }
                    if (list[i].PPANE2.Trim() != "")
                    {
                        if (list[i].PPANE2.Trim() != "1" && list[i].PPANE2.Trim() != "2")
                        {
                            nCNT[22] += 1;   //요잠혈
                        }
                    }
                    nCNT[23] += 1;                 //검사인원(전체)
                    //각종검사
                    if (list[i].PPANF1 != "")
                    {
                        if (double.Parse(list[i].PPANF1) >= 121)
                        {
                            nCNT[24] += 1;   //혈당(식전)
                        }
                    }
                    if (list[i].PPANF2 != "")
                    {
                        if (double.Parse(list[i].PPANF2) >= 251)
                        {
                            nCNT[25] += 1;   //총콜레스테롤
                        }
                    }
                    if (list[i].PPANF3 != "")
                    {
                        if (double.Parse(list[i].PPANF3) >= 51)
                        {
                            nCNT[26] += 1;   //AST GOT
                        }
                    }
                    if (list[i].PPANF4 != "")
                    {
                        if (double.Parse(list[i].PPANF4) >= 46)
                        {
                            nCNT[26] += 1;   //AST GPT
                        }
                    }
                    if (list[i].PPANF1.Trim() != "" || list[i].PPANF2.Trim() != "" || 
                        list[i].PPANF3.Trim() != "" || list[i].PPANF4.Trim() != "")
                    {
                        nCNT[27] += 1;      //검사인원(혈액1)
                    }
                    if (list[i].PPANF5.Trim() != "")
                    {
                        nCNT[29] += 1;      //검사인원(혈액2)
                        if (double.Parse(list[i].PPANF5) < 10)
                        {
                            nCNT[28] += 1;  //혈색소
                        }
                    }
                    if (list[i].PPANH1.Trim() == "1" || list[i].PPANH1.Trim() == "2")
                    {
                        nCNT[31] += 1;      //검사인원(B형간염)

                        if (list[i].PPANH1.Trim() == "2")
                        {
                            nCNT[30] += 1;  //B형간염
                        }
                    }
                    if (VB.Pstr(list[i].PPANG1.Trim(), "^^", 1) != "" && VB.Pstr(list[i].PPANG1.Trim(), "^^", 1) != "11")
                    {
                        nCNT[33] += 1;
                        if (double.Parse(list[i].PPANG1.Trim()) >= 4 && double.Parse(list[i].PPANG1.Trim()) <= 7)
                        {
                            nCNT[32] += 1;  //흉부방사선
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
                            nCol = 4;
                            nCNT[3] = 0;    //색각이상 =0
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
                        nCNT[3] = 0; //색각이상 =0
                        break;
                    default:
                        break;
                }

                for (int i = 8; i <= 41; i++)
                {
                    SS1.ActiveSheet.Cells[i, nCol].Text = nCNT[i - 8].ToString();
                }

                if (strClass == "1")
                {
                    for (int i = 8; i <= 41; i++)
                    {
                        nTemp = 0;
                        nTemp = long.Parse(SS1.ActiveSheet.Cells[i, 4].Text);
                        nTemp += long.Parse(SS1.ActiveSheet.Cells[i, 5].Text);
                        SS1.ActiveSheet.Cells[i, 8].Text = nTemp.ToString();
                    }
                }
                btnPrint.Enabled = true;
                fn_Sheet_Clear_Etc();
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

        void fn_Sheet_Clear()
        {
            SS1.ActiveSheet.Cells[3, 1].Text = "";
            SS1.ActiveSheet.Cells[3, 61].Text = "";
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

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
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
