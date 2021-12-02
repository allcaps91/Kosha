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
/// File Name       : frmHcSchoolOpinionAfterMgmtPrint.cs
/// Description     : 질병 유소견자 사후관리 소견서 인쇄
/// Author          : 이상훈
/// Create Date     : 2020-01-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool8.frm(HcSchool08)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolOpinionAfterMgmtPrint : Form
    {
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string FstrJepDate;
        string FstrHName;
        string[] FstrDrName = new string[2];

        public frmHcSchoolOpinionAfterMgmtPrint()
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
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtPrtDate.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtPrtDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtPrtDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            btnPrint.Enabled = false;

            SS1_Sheet1.Columns.Get(0).Visible = false;
            SS1_Sheet1.Columns.Get(6).Visible = false;
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
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                long nLtdCode1 = 0;
                string strClass = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS2);

                FstrJepDate = "";
                FstrHName = "";

                List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItemCntbyJepDate(strFrDate, strToDate, nLtdCode);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    nLtdCode1 = list[i].LTDCODE;
                    strClass = list[i].CLASS.To<string>();

                    HIC_JEPSU_PATIENT_SCHOOL list2 = hicJepsuPatientSchoolService.GetItembyJepDateSingle(strFrDate, strToDate, nLtdCode1, strClass);

                    SS1.ActiveSheet.Cells[i, 2].Text = list2.MINDATE;
                    SS1.ActiveSheet.Cells[i, 3].Text = list2.MAXDATE;
                    SS1.ActiveSheet.Cells[i, 4].Text = list2.CNT.To<string>();
                    SS1.ActiveSheet.Cells[i, 5].Text = list2.CLASS.To<string>();
                    SS1.ActiveSheet.Cells[i, 6].Text = list2.LTDCODE.To<string>();
                }

                btnPrint.Enabled = true;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (SS2.ActiveSheet.NonEmptyRowCount == 0)
                {
                    MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (FstrJepDate == "")
                {
                    MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "질병 유소견자 사후관리 소견서";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                //strHeader += sp.setSpdPrint_String(VB.Space(2) + FstrHName + VB.Space(2) + txtPrtDate.Text + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, true, true);
                strHeader += sp.setSpdPrint_String(VB.Space(2) + FstrHName + VB.Space(2) + txtPrtDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

                sp.Spread_All_Clear(SS2);
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

                SS2.ActiveSheet.Protect = false;

                strDate = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1)) + "(" + string.Format("{0:0000}", VB.Pstr(txtLtdCode.Text, ".", 1)) + "_" + VB.Left(dtpFrDate.Text, 4) + VB.Mid(dtpFrDate.Text, 6, 2) + VB.Mid(dtpFrDate.Text, 9, 2) + "~" + VB.Left(dtpToDate.Text, 4) + VB.Mid(dtpToDate.Text, 6, 2) + VB.Mid(dtpToDate.Text, 9, 2) + "[신체발달상황]";

                if (txtLtdCode.Text.Trim() == "")
                {
                    MessageBox.Show("회사코드 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                x = SS2.SaveExcel(strMyPath1 + strDate + "_01.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
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

                SS2.ActiveSheet.Protect = true;

                Cursor.Current = Cursors.Default;
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtPrtDate)
            {
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                clsPublic.GstrCalDate = "";
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                txtPrtDate.Text = clsPublic.GstrCalDate;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPrtDate)
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strDate1 = "";
                string strDate2 = "";
                string strClass = "";
                string strBan = "";
                string strLtdCode = "";

                sp.Spread_All_Clear(SS2);

                FstrHName = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                strDate1 = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                strDate2 = SS1.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                strClass = SS1.ActiveSheet.Cells[e.Row, 5].Text.Trim();
                strLtdCode = SS1.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                FstrHName += " ( " + strClass + " 학년 )";
                FstrJepDate = "실시기간 : " + VB.Left(strDate1, 4) + "년 " + VB.Mid(strDate1, 6, 2) + "월 " + VB.Right(strDate1, 2) + "일 ~ " + VB.Left(strDate2, 4) + "년 " + VB.Mid(strDate2, 6, 2) + "월 " + VB.Right(strDate2, 2) + "일";

                fn_Screen_Display(strDate1, strDate2, strClass, strBan, strLtdCode);
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

            FstrDrName[0] = "";
            FstrDrName[1] = "";

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

            List<HIC_JEPSU_PATIENT_SCHOOL> list = hicJepsuPatientSchoolService.GetItembyJepDateGroup(argDate1, argDate2, argLtdCode, argClass);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strSex = list[i].SEX;
                strBiman = list[i].PPANA3;
                strWEIGHT = list[i].PPANA4;
                if (FstrDrName[0] == "")
                {
                    FstrDrName[0] = hb.READ_License_DrName(list[i].PPANDRNO);
                }
                else
                {
                    if (FstrDrName[0] != hb.READ_License_DrName(list[i].PPANDRNO))
                    {
                        FstrDrName[1] = hb.READ_License_DrName(list[i].PPANDRNO);
                    }
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list[i].CLASS.To<string>();
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].BAN.To<string>();
                SS2.ActiveSheet.Cells[i, 2].Text = list[i].BUN.To<string>();
                SS2.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                SS2.ActiveSheet.Cells[i, 4].Text = list[i].SEX1;
                SS2.ActiveSheet.Cells[i, 5].Text = "[ 종합판정 : 질환의심 ]" + "\r\n" + list[i].PPANREMARK1;
                SS2.ActiveSheet.Cells[i, 6].Text = list[i].PPANREMARK2;
            }

            //ROW 높이 설정
            for (int i = 0; i < nRead; i++)
            {   
                Size size = SS2.ActiveSheet.GetPreferredCellSize(i, 5);
                SS2.ActiveSheet.Rows[i].Height = size.Height;
            }

            //SS2.AddCellSpan 1, .MaxRows, 7, 3

            //SS2.Col = 1
            //SS2.Row = .MaxRows
            //SS2.CellType = SS_CELL_TYPE_STATIC_TEXT
            //SS2.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
            //SS2.TypeEllipses = False
            //SS2.TypeHAlign = SS_CELL_H_ALIGN_RIGHT
            //SS2.TypeVAlign = 2       'center
            //SS2.TypeTextShadow = False
            //SS2.TypeTextShadowIn = False
            //SS2.TypeTextWordWrap = True
            //SS2.TypeTextPrefix = False
            //SS2.Text = ""
            //SS2.FontName = "굴림체"
            //SS2.FontSize = 10#

            if (FstrDrName[1] != "")
            {
                SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 0].Text = "검진 의사 : " + FstrDrName[0] + "  (인)  검진 의사 : " + FstrDrName[1] + "  (인)" + VB.Space(10) + "\r\n" + "\r\n" + "\r\n" + "\r\n";
            }
            else if (FstrDrName[0] != "")
            {
                SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 0].Text = "검진 의사 : " + FstrDrName[0] + "  (인)" + VB.Space(10) + "\r\n" + "\r\n" + "\r\n" + "\r\n";
            }
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 0].Text += "작성일자 : " + VB.Left(txtPrtDate.Text, 4) + "년 " + VB.Mid(txtPrtDate.Text, 6, 2) + "월 " + VB.Right(txtPrtDate.Text, 2) + "일" + VB.Space(10) + "\r\n" + "\r\n" + "검진 기관명 : (재)포항성모병원" + VB.Space(10);
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
