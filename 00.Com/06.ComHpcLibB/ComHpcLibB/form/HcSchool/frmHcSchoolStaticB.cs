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
/// File Name       : frmHcSchoolStaticB.cs
/// Description     : 학생건강검진 통계표(B)
/// Author          : 이상훈
/// Create Date     : 2020-02-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool11.frm(HcSchool41)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolStaticB : Form
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

        public frmHcSchoolStaticB()
        {
            InitializeComponent(); SetEvent();
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
                int x = 0;
                int y = 0;
                int nTotal = 0;
                string strClass = "";
                string strClass1 = "";
                string strBan = "";
                string strSchName = "";
                string strSex = "";
                string strGuBun = "";
                int nCol = 0;
                int[,] nCnt1 = new int[5, 3];
                int[,] nCnt2 = new int[5, 3];
                int[,] nCnt3 = new int[5, 3];
                int[,] nCnt4 = new int[5, 3];
                string strNewData = "";
                string strOldData = "";
                long nTemp = 0;

                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                for (int i = 0; i <= 4; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        nCnt1[i, j] = 0;
                        nCnt2[i, j] = 0;
                        nCnt3[i, j] = 0;
                        nCnt4[i, j] = 0;
                    }
                }

                fn_Sheet_Clear();

                if (txtLtdCode.Text.IsNullOrEmpty())
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
                    x = 1;
                    y = 0;
                    if (list[i].SEX == "F")
                    {
                        x = 2;
                    }
                    strClass1 = list[i].CLASS.To<string>(); 
                    strNewData = strClass1;

                    switch (strClass1)
                    {
                        case "2":
                            y = 1;
                            break;
                        case "3":
                            y = 2;
                            break;
                        case "5":
                            y = 3;
                            break;
                        case "6":
                            y = 4;
                            break;
                        default:
                            break;
                    }

                    if (!VB.Pstr(list[i].DPAN1, "^^", 1).IsNullOrEmpty() || !VB.Pstr(list[i].DPAN1, "^^", 2).IsNullOrEmpty())
                    {
                        nCnt1[y, x] += 1;   //치아우식증    
                    }
                    if (VB.Pstr(list[i].DPAN8, "^^", 1) == "1" || VB.Pstr(list[i].DPAN8, "^^", 2) == "1" || VB.Pstr(list[i].DPAN8, "^^", 3) == "1" || !VB.Pstr(list[i].DPAN8, "^^", 4).IsNullOrEmpty())
                    {
                        nCnt2[y, x] += 1;   //치주질환
                    }
                    if (!list[i].DPAN5.IsNullOrEmpty() && list[i].DPAN5 != "1")
                    {
                        nCnt3[y, x] += 1;   //부정교합
                    }

                    nCnt4[y, x] += 1;       //검사인원

                    strOldData = list[i].CLASS.To<string>();
                }

                nTotal = 0;                
                for (int i = 5; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 5:
                        case 6:
                            y = 1;
                            break;
                        case 7:
                        case 8:
                            y = 2;
                            break;
                        case 9:
                        case 10:
                            y = 3;
                            break;
                        case 11:
                        case 12:
                            y = 4;
                            break;
                        default:
                            break;
                    }

                    x = (i % 2) > 0 ? 1 : 2;
                    SS1.ActiveSheet.Cells[18, i - 1].Text = nCnt1[y, x].To<string>();
                    nTotal += nCnt1[y, x];
                }
                SS1.ActiveSheet.Cells[18, 12].Text = nTotal.To<string>();

                nTotal = 0;
                for (int i = 5; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 5:
                        case 6:
                            y = 1;
                            break;
                        case 7:
                        case 8:
                            y = 2;
                            break;
                        case 9:
                        case 10:
                            y = 3;
                            break;
                        case 11:
                        case 12:
                            y = 4;
                            break;
                        default:
                            break;
                    }
                    x = (i % 2) > 0 ? 1 : 2;
                    SS1.ActiveSheet.Cells[19, i - 1].Text = nCnt2[y, x].To<string>();
                    nTotal += nCnt2[y, x];
                }
                SS1.ActiveSheet.Cells[19, 12].Text = nTotal.To<string>();

                nTotal = 0;
                for (int i = 5; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 5:
                        case 6:
                            y = 1;
                            break;
                        case 7:
                        case 8:
                            y = 2;
                            break;
                        case 9:
                        case 10:
                            y = 3;
                            break;
                        case 11:
                        case 12:
                            y = 4;
                            break;
                        default:
                            break;
                    }
                    x = (i % 2) > 0 ? 1 : 2;
                    SS1.ActiveSheet.Cells[20, i - 1].Text = nCnt3[y, x].To<string>();
                    nTotal += nCnt3[y, x];
                }
                SS1.ActiveSheet.Cells[20, 12].Text = nTotal.To<string>();

                nTotal = 0;
                for (int i = 5; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 5:
                        case 6:
                            y = 1;
                            break;
                        case 7:
                        case 8:
                            y = 2;
                            break;
                        case 9:
                        case 10:
                            y = 3;
                            break;
                        case 11:
                        case 12:
                            y = 4;
                            break;
                        default:
                            break;
                    }
                    x = (i % 2) > 0 ? 1 : 2;
                    SS1.ActiveSheet.Cells[31, i - 1].Text = nCnt4[y, x].To<string>();
                    nTotal += nCnt4[y, x];
                }
                SS1.ActiveSheet.Cells[31, 12].Text = nTotal.ToString();

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
                if (rdoCnt2.Checked == true)
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }

                fn_Sheet_Clear();
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

                string strFileName = "";
                string strPath = "C:\\ExcelFileDown\\";
                Cursor.Current = Cursors.WaitCursor;

                SS1.ActiveSheet.Protect = false;

                strDate = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1)) + "(" + string.Format("{0:0000}", VB.Pstr(txtLtdCode.Text, ".", 1)) + "_" + VB.Left(dtpFrDate.Text, 4) + VB.Mid(dtpFrDate.Text, 6, 2) + VB.Mid(dtpFrDate.Text, 9, 2) + "~" + VB.Left(dtpToDate.Text, 4) + VB.Mid(dtpToDate.Text, 6, 2) + VB.Mid(dtpToDate.Text, 9, 2) + "[통계표]";

                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strFileName = strDate + "_01";

                hf.Excel_File_Create(strPath, strFileName, SS1, SS1_Sheet1);

                Cursor.Current = Cursors.Default;
            }
        }

        void fn_Sheet_Clear()
        {   
            for (int i = 18; i <= 20; i++)
            {
                for (int j = 4; j <= 12; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            for (int j = 4; j <= 12; j++)
            {
                SS1.ActiveSheet.Cells[31, j].Text = "-";
            }
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
