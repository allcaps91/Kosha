using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComHpcLibB.UseCon;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaExamResultReg.cs
/// Description     : 검사결과 등록
/// Author          : 이상훈
/// Create Date     : 2019-09-24
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain03.frm(FrmResult)" />

namespace ComHpcLibB
{
    public partial class frmHaExamResultReg : Form
    {
        HeaJepsuService heaJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaWomenService heaWomenService = null;
        HicRescodeService hicRescodeService = null;
        HeaJepsuMemoService heaJepsuMemoService = null;
        HicResultHisService hicResultHisService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaCodeService heaCodeService = null;
        HeaResultService heaResultService = null;
        HicMemoService hicMemoService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HicJepsuService hicJepsuService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        HIC_LTD LtdHelpItem = null;
        
        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcFunc hc = new clsHcFunc();

        conHaExamRes cHER = null;

        long FnWRTNO;
        int FnClickRow;                        //Help를 Click한 Row
        string[] FstrResult = new string[300];  //문장 결과를 보관할 배열
        long FnResultRow;
        long FnPano;
        string FstrPtno;                        //외래번호
        string FstrSName;
        string FstrSdate;                       //수진일자
        string FstrRowid;
        string FstrGjJong;
        long FnIEMunNo;

        List<conHaExamRes> lstCtrls = null;

        public frmHaExamResultReg()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHaExamResultReg(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            lstCtrls = new List<conHaExamRes>();

            LtdHelpItem = new HIC_LTD();
            heaJepsuService = new HeaJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            heaWomenService = new HeaWomenService();
            hicRescodeService = new HicRescodeService();
            heaJepsuMemoService = new HeaJepsuMemoService();
            hicResultHisService = new HicResultHisService();
            comHpcLibBService = new ComHpcLibBService();
            heaCodeService = new HeaCodeService();
            heaResultService = new HeaResultService();
            hicMemoService = new HicMemoService();
            heaSunapdtlService = new HeaSunapdtlService();
            hicJepsuService = new HicJepsuService();

            SheetView shv = SS2.ActiveSheet;
            InputMap im = new InputMap();
            Keystroke k = new Keystroke(Keys.Enter, Keys.None);
            im = SS2.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(k, SpreadActions.MoveToNextRow);
            im = SS2.GetInputMap(InputMapMode.WhenFocused);
            im.Put(k, SpreadActions.MoveToNextRow);

            SSList.Initialize(new SpreadOption { RowHeaderVisible = true, ColumnHeaderHeight = 35, RowHeight = 24 });
            SSList.AddColumn("종류",      nameof(HEA_JEPSU.GJJONG),     44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("접수번호",  nameof(HEA_JEPSU.WRTNO),      44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("성명",      nameof(HEA_JEPSU.SNAME),      82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진일자",  nameof(HEA_JEPSU.SDATE),      78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("상담",      nameof(HEA_JEPSU.SANGDAMGBN), 100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("입력",      "",                           82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.VisibleChanged += new EventHandler(eFormActivated);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnMenuHistory.Click += new EventHandler(eBtnClick);
            this.btnIEMunjin.Click += new EventHandler(eBtnClick);            
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);

            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.EditModeOff += new EventHandler(eSpdEditOff);
            this.SS2.KeyDown += new KeyEventHandler(eSpdKeyDown);

            this.SSList.CellClick += new CellClickEventHandler(eSpdClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.SS_ETC.EditModeOff += new EventHandler(eSpdEditOff);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);            
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.dtpFrDate.TextChanged += new EventHandler(eDtpChanged);

            this.chkEKG.Click += new EventHandler(echkClick);
            this.pnlResult.MouseWheel += new MouseEventHandler(eMouseWheel);

        }

        private void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == SS2)
            {
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
                {
                    int nRow = SS2.ActiveSheet.ActiveRowIndex;
                    int nCol = SS2.ActiveSheet.ActiveColumnIndex;

                    if (nRow >= 0 && nCol == 2)
                    {
                        SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";
                    }
                }
            }
        }

        private void eFormActivated(object sender, EventArgs e)
        {
            if (!clsHcVariable.GstrPanWRTNO.IsNullOrEmpty())
            {
                if (clsHcVariable.GstrPanWRTNO == txtWrtNo.Text)
                {
                    return;
                }
                txtWrtNo.Text = clsHcVariable.GstrPanWRTNO.ToString();
                
                Cursor.Current = Cursors.WaitCursor;

                fn_Screen_Display();
                fn_Hea_Memo_Screen();

                Cursor.Current = Cursors.Default;
            }
        }

        private void eMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                //마우스 휠 내림
                Point current = pnlResult.AutoScrollPosition;
                Point scrolled = new Point(current.X, -current.Y + 50);
                pnlResult.AutoScrollPosition = scrolled;
            }
            else
            {
                //마우스 휠 올림
                Point current = pnlResult.AutoScrollPosition;
                Point scrolled = new Point(current.X, -current.Y - 50);
                pnlResult.AutoScrollPosition = scrolled;
            }
            
        }

        private void eDtpChanged(object sender, EventArgs e)
        {
            dtpToDate.Text = dtpFrDate.Text;
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            if (sender == SS2)
            {
                int nRow = SS2.ActiveSheet.ActiveRowIndex;
                int nCol = SS2.ActiveSheet.ActiveColumnIndex;

                if (nRow >= 0 && nCol == 2)
                {
                    SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";
                }
            }
            else if (sender == SS_ETC)
            {
                int nRow = SS_ETC.ActiveSheet.ActiveRowIndex;
                int nCol = SS_ETC.ActiveSheet.ActiveColumnIndex;

                Size size = SS_ETC.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                SS_ETC.ActiveSheet.Rows[nRow].Height = size.Height;

            }
        }        

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            pnlResult.Dock = DockStyle.Fill;

            SS2_Sheet1.Columns.Get(7).Visible = false;   //결과값코드
            SS2_Sheet1.Columns.Get(8).Visible = false;   //변경
            SS2_Sheet1.Columns.Get(9).Visible = false;   //ROWID
            SS2_Sheet1.Columns.Get(10).Visible = false;  //결과형태
            SS2_Sheet1.Columns.Get(11).Visible = false;  //상담여부

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //대분류 설정
            cboHeaSort.Items.Clear();            

            //종합검진 조회순서,인쇄순서 그룹명을 READ
            List<HEA_CODE> list = heaCodeService.FindOne("07");

            if (list.Count > 0)
            {
                cboHeaSort.Items.Add("*.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboHeaSort.Items.Add(list[i].CODE + "." + list[i].NAME);
                }
            }
            cboHeaSort.SelectedIndex = 0;

            cboView.Items.Clear();
            cboView.Items.Add("1.성명순");
            cboView.Items.Add("2.접수번호");
            cboView.Items.Add("3.접수일자");
            cboView.Items.Add("4.검진종류");
            cboView.SelectedIndex = 0;

            fn_Screen_Clear();
            txtLtdCode.Text = "";

            sp.Spread_All_Clear(SSList);
            SSList.ActiveSheet.RowCount = 5;

            if (FnWRTNO != 0 && !FnWRTNO.IsNullOrEmpty())
            {
                txtWrtNo.Text = FnWRTNO.ToString();
                fn_Screen_Display();
                fn_Hea_Memo_Screen();
            }
        }

        void fn_Screen_Clear()
        {
            btnPacs.Enabled = false;
            btnEMR.Enabled = false;
            btnIEMunjin.Enabled = false;
            FstrPtno = "";
            FstrRowid = "";
            FstrGjJong = "";
            FstrSdate = "";
            FnIEMunNo = 0;
            txtWrtNo.Text = "";
            FnClickRow = 0;
            FnPano = 0;

            for (int i = 0; i < 300; i++)
            {
                FstrResult[i] = "";
            }

            FnResultRow = 0;
            txtWrtNo.Enabled = true;
            chkEKG.Checked = false;

            pnlResult.Controls.Clear();
            sp.Spread_Clear_Simple(SS1, 1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS_ETC);

            SS2.ActiveSheet.RowCount = 5;

            btnMenuSave.Enabled = false;
            pnlResult.Visible = false;
            lblGjJong.Text = "";

            //clsHcVariable.GstrPanWRTNO = "";
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            long nMaxLine = 0;
            string strSEX = "";
            string strHeaSORT = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strTemp = "";
            string strFlag = "";
            string strExcode = "";
            string strExName = "";
            string strYYYY = "";
            string strSDate = "";
            string strMaxLine = "";

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            txtWrtNo.Enabled = false;
            FnWRTNO = txtWrtNo.Text.To<long>();
            for (int i = 0; i < 300; i++)
            {
                FstrResult[i] = "";
            }

            btnMenuSave.Enabled = true;
            pnlResult.Visible = true;

            //검사결과 인쇄라인 초과 점검용 자료 설정(SET_PrintMaxLine)
            List<HEA_CODE> listCode = heaCodeService.FindOne("16");

            nREAD = listCode.Count;

            strMaxLine = "{}";
            for (int i = 0; i < nREAD; i++)
            {
                strMaxLine += "[" + listCode[i].CODE + "]" + listCode[i].GUBUN1 + "{}";
            }

            //인적사항을 Display(Screen_Injek_display)
            HEA_JEPSU list = heaJepsuService.GetItembyWrtNoGbSts(FnWRTNO, "0");

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnMenuSave.Enabled = false;
                return;
            }

            FstrPtno = list.PTNO;
            FstrSName = list.SNAME;
            FnIEMunNo = list.IEMUNNO;
            if (!FstrPtno.IsNullOrEmpty())
            {
                btnPacs.Enabled = true;
                btnEMR.Enabled = true;
            }

            strSEX = list.SEX;
            FnPano = list.PANO;
            strSDate = list.SDATE;
            FstrSdate = list.SDATE;
            FstrGjJong = list.GJJONG;
            FstrRowid = list.RID;

            strYYYY = VB.Left(list.SDATE, 4);

            if (FnIEMunNo > 0)
            {
                btnIEMunjin.Enabled = true;
            }

            SS1.ActiveSheet.RowCount = 1;

            SS1.ActiveSheet.Cells[0, 0].Text = list.PTNO.To<string>("0");
            SS1.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            SS1.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + list.SEX;
            SS1.ActiveSheet.Cells[0, 3].Text = list.BIRTHDAY.To<string>("").Substring(0, 8);
            SS1.ActiveSheet.Cells[0, 4].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            SS1.ActiveSheet.Cells[0, 5].Text = list.SDATE;
            //SS1.ActiveSheet.Cells[0, 6].Text = hb.READ_GjJong_HeaName(list.GJJONG);
            SS1.ActiveSheet.Cells[0, 6].Text = heaSunapdtlService.GetMainSunapDtlCodeNameByWrtno(FnWRTNO);
            SS1.ActiveSheet.Cells[0, 7].Text = !list.WEBPRINTREQ.IsNullOrEmpty() ? "알림톡" : list.GBCHK3 == "Y" ? "방문수령" : list.GBJUSO == "Y" ? "우편(집)" : list.GBJUSO == "N" ? "우편(회사)" : list.GBJUSO == "E" ? "우편(별도)" : "";


            //일반검진 접수 내역 Display
            List<HIC_JEPSU> lstHJ = hicJepsuService.GetListGjNameByPtnoJepDate(strSDate, FstrPtno);
            if (lstHJ.Count > 0)
            {
                lblGjJong.Text = "일반검진 내역 ▶ ";

                for (int i = 0; i < lstHJ.Count; i++)
                {
                    lblGjJong.Text += lstHJ[i].NAME + ", ";
                }
            }


            chkEKG.Checked = list.GBEKG == "*" ? true : false;

            btnMenuSave.Enabled = true;
            if (list.GBSTS == "9")
            {
                btnMenuSave.Enabled = false;
            }

            //검사결과를 재판정
            hm.ExamResult_RePanjeng_Hea(FnWRTNO, strSEX, strSDate);

            //검사항목을 Display(Screen_Exam_Items_display)
            strHeaSORT = VB.Left(cboHeaSort.Text, 1);

            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoNoActing(FnWRTNO, strHeaSORT);

            SS2.ActiveSheet.RowCount = 0;
            SS2.ActiveSheet.RowCount = list2.Count;

            nREAD = list2.Count;
            
            lstCtrls.Clear();
            nRow = 0;
            strFlag = "N";
            for (int i = 0; i < nREAD; i++)
            {
                strResult = list2[i].RESULT;
                strResCode = list2[i].RESCODE;
                strResultType = list2[i].RESULTTYPE;
                strGbCodeUse = list2[i].GBCODEUSE;
                strExcode = list2[i].EXCODE;
                strExName = list2[i].HNAME;

                if (strExcode == "E909")
                {
                    strExcode = "E909";
                }

                if (strResultType == "3")
                {
                    //기존 스프레드에서 Panel 방식으로 변경 
                    cHER = new conHaExamRes();
                    cHER.Dock = DockStyle.Top;
                    cHER.lblExTitle.Text = "◈" + strExName + "◈";
                    cHER.lblExCD.Text = "【" + strExcode + "】";
                    cHER.eMouseWheelEvnt += new conHaExamRes.rMouseWheelEvnt(eMouseWheel);

                    //지정한 글자수를 초과하면 **를 표시함
                    if (VB.InStr(strMaxLine, "{}[" + strExcode + "]") > 0)
                    {
                        nMaxLine = long.Parse(VB.STRCUT(strMaxLine, "{}[" + strExcode + "]", "{}"));
                        if (nMaxLine != 0)
                        {
                            strTemp = cf.TextBox_2_MultiLine(" " + strResult, 88);
                            if (VB.L(strTemp, "{{@}}") > nMaxLine)
                            {
                                cHER.lblExTitle.BackColor = Color.NavajoWhite;
                                cHER.lblExTitle.Text += " - 글자초과";
                            }
                        }
                    }

                    //내시경조직검사 결과 누락 점검
                    if (fn_ENDO_Biophy_Result_Check(strExcode, strResult) == true)
                    {
                        cHER.lblExTitle.BackColor = Color.NavajoWhite;
                        cHER.lblExTitle.Text += " - 조직결과 누락";
                    }

                    if (!strResult.IsNullOrEmpty())
                    {
                        strResult = strResult.Replace("\r\n", "##");
                        strResult = strResult.Replace("\n", "\r\n");
                        strResult = strResult.Replace("##", "\r\n");
                    }

                    cHER.txtRes.Text = strResult;

                    //SetControlSize(cHER);

                    lstCtrls.Add(cHER);
                }

                nRow += 1;
                if (nRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = nRow;
                }

                SS2.ActiveSheet.Cells[nRow - 1, 0].Text = list2[i].EXCODE;
                //여성정밀 항목 참고값 임의 수정 가능
                if (strExcode == "E908" || strExcode == "E909")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 6].CellType = txt;
                    SS2.ActiveSheet.Cells[nRow - 1, 6].Locked = false;
                }

                SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].HNAME;
                SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strResult;
                //결과형태가 문장이면 아래의 TextBox에서 입력을 해야됨
                if (strResultType == "3")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 2].CellType = txt;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Locked = true;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(255, 255, 192);
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strResult;
                    FstrResult[nRow - 1] = strResult;
                }

                //A103(비만도)는 자동계산(입력금지)
                if (strExcode == "A103")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Locked = true;
                }

                if (strResCode.IsNullOrEmpty() || strResultType == "3")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 3].CellType = txt;
                    SS2.ActiveSheet.Cells[nRow - 1, 3].Locked = true;
                    SS2.ActiveSheet.Cells[nRow - 1, 3].BackColor = Color.FromArgb(255, 255, 192);
                    SS2.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                }

                if (!strResCode.IsNullOrEmpty())
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (list2[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 5].Text = "*";
                }

                if (list2[i].EXCODE == "A103")
                {
                    strTemp = hm.Biman_Gesan(FnWRTNO, "HEA");
                    if (strResult.To<long>() <= 89)
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                    else if (strResult.To<long>() > 110)
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = VB.Pstr(strTemp, ".", 1);
                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = strTemp;
                }

                //혈액인자(RH -)
                if (list2[i].EXCODE == "H841")
                {
                    if (strResult == "-")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }

                //참고치를 Dispaly
                if (strExcode != "E908" && strExcode != "E909")
                {
                    if (hf.Check_ReferValue_ChangeCode(strExcode) == true)
                    {
                        strNomal = hf.GET_Refer_Value(strExcode, strSEX, strSDate, "N");
                    }
                    else
                    {
                        if (strSEX == "M")
                        {
                            strNomal = list2[i].MIN_M + "~" + list2[i].MAX_M;
                        }
                        else
                        {
                            strNomal = list2[i].MIN_F + "~" + list2[i].MAX_F;
                        }
                    }
                }
                else
                {
                    HEA_WOMEN list3 = heaWomenService.Read_Women_Reference(FnWRTNO);

                    if (!list3.IsNullOrEmpty())
                    {
                        if (strExcode == "E909")
                        {
                            strNomal = list3.MIN_ONE + "~" + list3.MAX_ONE;
                        }
                        else
                        {
                            strNomal = list3.MIN_TWO + "~" + list3.MAX_TWO;
                        }
                    }
                    else
                    {
                        if (strSEX == "M")
                        {
                            strNomal = list2[i].MIN_M + "~" + list2[i].MAX_M;
                        }
                        else
                        {
                            strNomal = list2[i].MIN_F + "~" + list2[i].MAX_F;
                        }
                    }
                }

                if (strNomal == "~")
                {
                    strNomal = "";
                }
                
                SS2.ActiveSheet.Cells[nRow - 1, 6].Text = strNomal;
                SS2.ActiveSheet.Cells[nRow - 1, 7].Text = strResCode;

                if (list2[i].EXCODE == "A151")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "007";
                }
                if (list2[i].EXCODE == "TH01" || list2[i].EXCODE == "TH02")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "022";
                }
                if (strFlag == "N")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                }

                SS2.ActiveSheet.Cells[nRow - 1, 9].Text = list2[i].RID;
                SS2.ActiveSheet.Cells[nRow - 1, 10].Text = strResultType;

                if (list2[i].SANGDAM == "Y")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 11].Text = "Y";
                    SS2.ActiveSheet.Cells[nRow - 1, 0].BackColor = Color.NavajoWhite;
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 11].Text = "N";
                    SS2.ActiveSheet.Cells[nRow - 1, 0].BackColor = Color.White;
                }


                if (list2[i].EXCODE == "TU42")
                {
                    list2[i].EXCODE = "TU42";
                }


                //판정결과를 다시 Check함(L=Low,H=High)
                SS2.ActiveSheet.Cells[nRow - 1, 5].Text = hb.Result_Panjeng(list2[i].EXCODE, strResult, strNomal);
                switch (SS2.ActiveSheet.Cells[nRow - 1, 5].Text)
                {
                    case "L":
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                        break;
                    case "H":
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                        break;
                    default:
                        break;
                }

                if (strExcode == "A271" || strExcode == "A272")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                }

                if (SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor != Color.FromArgb(250, 210, 222))
                {
                    if (!list2[i].PANJENG.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }

                //

            }

            if (lstCtrls.Count > 0)
            {
                DrawControls();

                pnlResult.PerformLayout();
            }

            SS2.ActiveSheet.RowCount = nRow;
        }

        private void DrawControls()
        {
            Point CurrentPoint;
            CurrentPoint = pnlResult.AutoScrollPosition;

            int i = 0;
            pnlResult.Controls.Clear();
            pnlResult.SuspendLayout();

            for (int j = lstCtrls.Count - 1; j >= 0; j--)
            {
                pnlResult.Controls.Add(lstCtrls[j]);
                lstCtrls[j].Width = pnlResult.ClientRectangle.Width;
                lstCtrls[j].Top = i; i += lstCtrls[j].Height;
            }

            pnlResult.ResumeLayout();
            pnlResult.AutoScrollPosition = new Point(Math.Abs(pnlResult.AutoScrollPosition.X), Math.Abs(CurrentPoint.Y));
            pnlResult.AutoScrollPosition = new Point(0, 0);
        }

        /// <summary>
        /// 위내시경,대장내시경 조직검사 결과 누락 점검(True:결과누락)
        /// </summary>
        /// <param name="argExcode"></param>
        /// <param name="argResult"></param>
        /// <returns></returns>
        bool fn_ENDO_Biophy_Result_Check(string argExcode, string argResult)
        {
            bool rtnVal = false;
            string strBiophy = "";

            //위내시경,대장내시경이 아니면 점검대상이 아님
            if (VB.InStr(clsHcVariable.B01_ENDO_EXCODE, argExcode) == 0)
            {
                rtnVal = false;
                return rtnVal;
            }

            //조직검사 결과가 있으면
            if (VB.InStr(argResult, "▶Biopsy Diagnosis:") > 0)
            {
                rtnVal = false;
                return rtnVal;
            }

            if (VB.InStr(argResult, "▶EndoScopic Biopsy:") > 0)
            {
                rtnVal = false;
                return rtnVal;
            }
            strBiophy = VB.STRCUT(argResult, "▶EndoScopic Biopsy:", "▶");
            strBiophy = strBiophy.Replace("\r\n", "");
            strBiophy = strBiophy.Replace("\r", "");
            strBiophy = strBiophy.Replace("\n", "");
            if (strBiophy == "")
            {
                rtnVal = false;
                return rtnVal;
            }

            //조직검사 결과 누락
            rtnVal = true;

            return rtnVal;
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
                int nREAD = 0;
                int nRead2 = 0;
                int nRow = 0;
                bool bOK = false;
                string strLtdCode = "";
                string strMaxLine = "";
                //string strCodeList = "";
                List<string> strCodeList = new List<string>();
                string strExcode = "";
                string strResult = "";
                string strTemp = "";
                long nMaxLine = 0;

                string strFrDate = "";
                string strToDate = "";
                string strGubun = "";
                string strSangTime = "";

                pnlResult.Visible = false;
                strLtdCode = "";
                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                if (chkWordOver.Checked == true)
                {
                    List<HEA_CODE> list = heaCodeService.FindOne("16");

                    nREAD = list.Count;
                    strMaxLine = "{}";
                    strCodeList.Clear();
                    for (int i = 0; i < nREAD; i++)
                    {
                        strMaxLine += "[" + list[i].CODE + "[" + list[i].GUBUN1 + "{}";
                        strCodeList.Add(list[i].CODE);
                    }
                }

                strLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1);
                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                switch (cboView.Text)
                {
                    case "1":
                        strGubun = "1";
                        break;
                    case "2":
                        strGubun = "2";
                        break;
                    case "3":
                        strGubun = "3";
                        break;
                    case "4":
                        strGubun = "4";
                        break;
                    default:
                        strGubun = "";
                        break;
                }

                // 자료를 SELECT
                List<HEA_JEPSU> list2 = heaJepsuService.GetItembyLtdCode(strFrDate, strToDate, strLtdCode, strGubun, txtSName.Text.Trim());

                nREAD = list2.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    strSangTime = "";
                    bOK = true;
                    if (chkWordOver.Checked == true)    //Check결과글자초과
                    {
                        List<HEA_RESULT> list3 = heaResultService.GetExCodebyWrtNo_All(list2[i].WRTNO, strCodeList);

                        nRead2 = list3.Count;
                        bOK = false;
                        for (int j = 0; j < nRead2; j++)
                        {
                            strExcode = list3[j].EXCODE;
                            strResult = list3[j].RESULT;

                            nMaxLine = VB.STRCUT(strMaxLine, "{}[" + strExcode + "]", "{}").To<long>();
                            if (nMaxLine != 0)
                            {
                                strTemp = cf.TextBox_2_MultiLine(" " + strResult, 88);
                                if (VB.L(strTemp, "{{@}}") > nMaxLine)
                                {
                                    bOK = true;
                                    break;
                                }
                            }
                            //위내시경,대장내시경 조직검사가 없으면 표시함
                            if (fn_ENDO_Biophy_Result_Check(strExcode, strResult) == true)
                            {
                                bOK = true;
                                break;
                            }
                        }
                    }

                    if (bOK == true)
                    {
                        nRow += 1;

                        if (SSList.ActiveSheet.RowCount < nRow)
                        {
                            SSList.ActiveSheet.RowCount = nRow;
                        }

                        SSList.ActiveSheet.Cells[i, 0].Text = list2[i].GJJONG;
                        //결과지 발송한 사람은 음영으로 표시
                        if (list2[i].PRTDATE.To<string>("").Trim() != "")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 2].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(180, 180, 180);
                        }

                        //알림톡 전송대상자 색깔표시
                        if (!list2[i].WEBPRINTSEND.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.Orange;
                        }

                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list2[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list2[i].SDATE.To<string>();

                        if (list2[i].SANGDAMGBN != "" || list2[i].SANGDAM_ONE == "Y")
                        {
                            if (list2[i].SANGSABUN > 0)
                            {

                                if (!list2[i].SANGDAMGBN.IsNullOrEmpty())
                                {
                                    if (VB.Mid(list2[i].SANGDAMGBN,3,8) == list2[i].SDATE.To<string>())
                                    {
                                        strSangTime = VB.Right(VB.Pstr(list2[i].SANGDAMGBN.Trim(), "^^", 1),5);
                                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_JikwonName(list2[i].SANGSABUN.To<string>()) + " " +strSangTime;
                                    }
                                    else
                                    {
                                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_JikwonName(list2[i].SANGSABUN.To<string>());
                                    }
                                }

                                //SSList.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_JikwonName(list2[i].SANGSABUN.To<string>())+" (" + strSangTime + ")";       
                            }
                        }
                        else
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                        }

                        if (list2[i].GBSTS.To<int>() > 2)
                        {
                            HEA_RESULT list4 = heaResultService.GetEntSabunbyWrtNo(list2[i].WRTNO);

                            if (!list4.IsNullOrEmpty())
                            {
                                SSList.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_JikwonName(list4.ENTSABUN.To<string>());
                            }

                            SSList.ActiveSheet.Cells[nRow - 1, 5].BackColor = Color.Pink;
                        }

                        if (list2[i].SANGDAM_ONE == "Y")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }
                }
                SSList.ActiveSheet.RowCount = nRow;
            }
            else if (sender == btnPacs)
            {
                frmViewResult f = new frmViewResult(FstrPtno);
                f.ShowDialog(this);

            }
            else if (sender == btnEMR)
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);
                //CallEmrViewNew();
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
            else if (sender == btnMenuSave)
            {
                string strResult       ="";
                string strCODE         ="";
                string strROWID        ="";
                string strPanjeng      ="";
                string strChange       ="";
                string strResCode      ="";
                string strResType = "";
                double nHEIGHT = 0;
                double nWeight = 0;
                long nDataCNT = 0;
                long nResultCNT = 0;
                string strGbSTS = "";
                string strEndoSo = "";
                string strSEX = "";

                string strYYYY = "";
                long nAge = 0;

                if (txtWrtNo.Text.To<long>() == 0) { return; }

                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //인적사항을 READ
                HEA_JEPSU list = heaJepsuService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

                if (!list.IsNullOrEmpty())
                {
                    strYYYY = VB.Left(list.SDATE.ToString(), 4);
                    strSEX = list.SEX;
                    nAge = list.AGE;
                    if (list.GBSTS == "9")
                    {
                        MessageBox.Show("판정이 완료된 자료는 Data를 수정할 수 없습니다!!", "수정불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;

                nDataCNT = 0;
                nResultCNT = 0;
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strPanjeng = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    strChange = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                    strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();
                    strResCode = SS2.ActiveSheet.Cells[i, 7].Text.Trim();
                    strResType = SS2.ActiveSheet.Cells[i, 10].Text.Trim();

                    //문장이면 텍스트박스에서 입력한 결과를 저장(사유:Sheet에서는 글자가 100Byte만 가능)
                    if (strResType == "3")
                    {
                        strResult = FstrResult[i];
                    }

                    //비만도
                    if (VB.UCase(strCODE.Trim()) == "A101")
                    {
                        nHEIGHT = SS2.ActiveSheet.Cells[i, 2].Text.To<double>();
                    }
                    else if (VB.UCase(strCODE.Trim()) == "A102")
                    {
                        nWeight = SS2.ActiveSheet.Cells[i, 2].Text.To<double>();
                    }

                    if (nHEIGHT > 0 && nWeight > 0 && VB.UCase(strCODE) == "A103")
                    {
                        strResult = hm.Biman_Gesan(FnWRTNO, "HEA");

                        if (string.Compare(strResult, "03") >= 0)   //비만
                        {
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170);
                        }

                        strResult = VB.Pstr(strResult, ".", 1);
                    }

                    if (strResult == "본인 제외") { strResult = "본인제외"; }

                    if (strChange == "Y" || strCODE == "A103")
                    {
                        //History에 INSERT
                        int result1 = hicResultHisService.Result_History_Insert_Hea(clsType.User.IdNumber, strResult, strROWID);

                        if (result1 < 0)
                        {
                            MessageBox.Show(i + "번줄 검사결과 History를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        int result2 = hicResultHisService.UpdatebyRowId_Hea(strResult, strPanjeng, strResCode,  clsType.User.IdNumber, strROWID);

                        if (result2 < 0)
                        {
                            MessageBox.Show(i + "번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                //2021-03-18 검사결과 저장 SS_Res -> conHaExamRes 컨트롤로 변경
                if (lstCtrls.Count > 0)
                {
                    for (int i = 0; i < lstCtrls.Count; i++)
                    {
                        strCODE = lstCtrls[i].lblExCD.Text.Replace("【", "").Replace("】", "").Trim();

                        if (!strCODE.IsNullOrEmpty())
                        {
                            strResult = lstCtrls[i].txtRes.Text.Trim();

                            if (strResult == "본인 제외") { strResult = "본인제외"; }

                            strResult = strResult.Replace("\r\n", "\n");

                            //History에 INSERT
                            int result2 = hicResultHisService.Result_History_Insert_Hea(clsType.User.IdNumber, strResult, strROWID, strCODE);

                            if (result2 < 0)
                            {
                                MessageBox.Show(i + "번줄 검사결과 History를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            int result3 = hicResultHisService.UpdatebyWrtNoExCode_Hea(strResult, clsType.User.IdNumber, long.Parse(txtWrtNo.Text), strCODE);

                            if (result3 < 0)
                            {
                                MessageBox.Show("판독문 결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }
                
                //청력검사 결과 자동입력
                hb.Update_Audio_Result(long.Parse(txtWrtNo.Text), strSEX);
                //폐활량검사 결과 자동입력
                hb.Update_Lung_Capacity(long.Parse(txtWrtNo.Text), strSEX);
                //MDRD-GFR 자동계산 2012년부터
                //hm.MDRD_GFR_Gesan(FnWRTNO, strSEX, nAge, "HEA");

                //해당접수번호의 결과입력대상건수, 결과입력건수를 READ
                List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetResultbyWrtNo(long.Parse(txtWrtNo.Text));

                nDataCNT = 0;
                nResultCNT = 0;
                for (int i = 0; i < list2.Count; i++)
                {
                    nDataCNT += 1;
                    if (list2[i].RESULT.To<string>("").Trim() != "") { nResultCNT += 1; }
                    if (list2[i].EXCODE.To<string>("").Trim() == "TX20" && list2[i].RESULT.To<string>("").Trim() != "")
                    {
                        strEndoSo = list2[i].RESULT.To<string>("").Trim();
                    }
                    if (list2[i].EXCODE.To<string>("").Trim() == "TX22" && list2[i].RESULT.To<string>("").Trim() != "")
                    {
                        strEndoSo = list2[i].RESULT.To<string>("").Trim();
                    }
                    if (list2[i].EXCODE.To<string>("").Trim() == "TX23" && list2[i].RESULT.To<string>("").Trim() != "")
                    {
                        strEndoSo = list2[i].RESULT.To<string>("").Trim();
                    }
                }

                //접수마스타에 상태를 UPDATE
                strGbSTS = "1"; //수진자등록
                if (nResultCNT == nDataCNT) { strGbSTS = "3"; } //입력완료
                if (nResultCNT > 0 && nResultCNT < nDataCNT) { strGbSTS = "2"; }    //입력중

                int result = heaJepsuService.Update_Hea_Jepsu_GbSts(strGbSTS, "", long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("접수마스타에 입력상태 변경중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                Cursor.Current = Cursors.Default;

                eBtnClick(btnMemoSave, new EventArgs());
                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnMenuHistory)
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("frmHaExamResultReg", FstrPtno, FstrSName);
                frm.ShowDialog();
            }
            else if (sender == btnIEMunjin)
            {
                //clsPublic.GstrRetValue = "MUNNO=" + FnIEMunNo;
                //clsHcVariable.GstrIEMunjin = "";
                //clsHcVariable.GstrProgram = "종합검진";

                //frmHcIEMunjin frm = new frmHcIEMunjin("", FstrSName);
                //frm.ShowDialog(this);

                Form frmMunJinView = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (frmMunJinView != null)
                {
                    frmMunJinView.Close();
                    frmMunJinView.Dispose();
                    frmMunJinView = null;
                }

                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrSdate, FstrPtno, "11", FstrRowid);
                FrmHcSangInternetMunjinView.Show();
                FrmHcSangInternetMunjinView.WindowState = FormWindowState.Minimized;
            }
            else if (sender == btnCancel)
            {
                clsHcVariable.GstrPanWRTNO = "";
                fn_Screen_Clear();
            }
            else if (sender == btnMemoSave)
            {
                string strMemo = "";
                string strROWID = "";
                string strOK = "";
                string strTime = "";

                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (FnWRTNO > 0)
                {
                    for (int i = 0; i < SS_ETC.ActiveSheet.RowCount; i++)
                    {
                        strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text.Trim();
                        strTime = SS_ETC.ActiveSheet.Cells[i, 1].Text.Trim();
                        strMemo = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                        strROWID = SS_ETC.ActiveSheet.Cells[i, 4].Text.Trim();
                        //신규작성일경우
                        if (strTime == "" && strMemo != "")
                        {
                            int result = comHpcLibBService.InsertHeaMemo(FnWRTNO, strMemo, clsType.User.IdNumber, FstrPtno);

                            if (result < 0)
                            {
                                MessageBox.Show("메모 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        //삭제할경우
                        if (strOK == "True")
                        {
                            int result = hicMemoService.DeleteData(strROWID, "종검");
                            if (result < 0)
                            {
                                MessageBox.Show("메모 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    fn_Hea_Memo_Screen();
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strExam = "";
                string strResult = "";
                string strSangDam = "";

                if (e.RowHeader == true)
                {
                    return;
                }

                if (e.Column != 0)
                {
                    return;
                }

                strExam = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();

                if (SS2.ActiveSheet.Cells[e.Row, 11].Text.Trim() == "Y")
                {
                    strSangDam = "N";
                }
                else
                {
                    strSangDam = "Y";
                }

                int result = heaResultService.UpdateSangdambyWrtnoExCode(FnWRTNO, strExam, strSangDam);

                if (SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() == "Y")
                {
                    SS2.ActiveSheet.Cells[e.Row, 0].Text = "N";
                    SS2.ActiveSheet.Cells[e.Row, 0].BackColor = Color.White;
                }
                else
                {
                    SS2.ActiveSheet.Cells[e.Row, 0].Text = "Y";
                    SS2.ActiveSheet.Cells[e.Row, 0].BackColor = Color.Orange;
                }
            }
            else if (sender == SSList)
            {
                long nWrtNo = 0;

                nWrtNo = SSList.ActiveSheet.Cells[e.Row, 1].Text.To<long>();

                fn_Screen_Clear();
                txtWrtNo.Text = nWrtNo.To<string>("");

                clsHcVariable.GstrPanWRTNO = txtWrtNo.Text;

                Cursor.Current = Cursors.WaitCursor;

                fn_Screen_Display();
                fn_Hea_Memo_Screen();

                Cursor.Current = Cursors.Default;
            }
        }

        void fn_Hea_Memo_Screen()
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS_ETC);
            SS_ETC.ActiveSheet.RowCount = 5;

            //참고사항 Display
            if (FnWRTNO > 0)
            {
                //ist<HEA_JEPSU_MEMO> list = heaJepsuMemoService.GetItembyPaNo(FnPano);
                //List<HEA_JEPSU_MEMO> list = heaJepsuMemoService.GetItembyPaNo(FstrPtno);
                List<HIC_MEMO> list = hicMemoService.GetHeaItembyPaNo(FstrPtno);

                if (!list.IsNullOrEmpty())
                {
                    nRead = list.Count;

                    SS_ETC.ActiveSheet.RowCount = nRead + 5;
                    if (nRead > 0)
                    {
                        for (int i = 0; i < nRead; i++)
                        {
                            SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].ENTTIME.To<string>();
                            SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].MEMO.To<string>();
                            SS_ETC.ActiveSheet.Cells[i, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, list[i].JOBSABUN.To<string>());
                            SS_ETC.ActiveSheet.Cells[i, 4].Text = list[i].RID.To<string>();

                            FarPoint.Win.Spread.Row row;
                            row = SS_ETC.ActiveSheet.Rows[i];
                            float rowSize = row.GetPreferredHeight();
                            row.Height = rowSize;
                        }
                    }
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strResCode = "";
                string strResType = "";

                if (e.Column != 2 && e.Column != 3)
                {
                    return;
                }

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                strResType = SS2.ActiveSheet.Cells[e.Row, 10].Text.Trim();

                //판독문등 문장 입력항목이면
                if (e.Column == 2)
                {
                    if (strResType == "3")
                    {
                        FnResultRow = e.Row;
                        //txtResult.Enabled = true;
                        //btnResult.Enabled = true;
                        //btnResultCancel.Enabled = true;
                        //txtResult.Text = FstrResult[e.Row];
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                FnClickRow = e.Row;

            }
            else if (sender == SSList)
            {
                if (e.ColumnHeader)
                {
                    sp.setSpdSort(SSList, e.Column, true);
                    return;
                }
                else if (e.Column == 2)
                {
                    long nWrtNo = SSList.ActiveSheet.Cells[e.Row, 1].Text.To<long>(0);

                    HEA_JEPSU list = heaJepsuService.GetPrtDatebyWrtNo(nWrtNo);

                    if (list.PRTDATE.To<string>("").Trim() != "")
                    {
                        this.toolTip1.SetToolTip(SSList, "출력일자 : " + list.PRTDATE.To<string>(""));
                    }

                    if (list.WEBPRINTSEND.To<string>("").Trim() != "")
                    {
                        this.toolTip1.SetToolTip(SSList, "전송일자 : " + list.WEBPRINTSEND.To<string>(""));
                    }
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
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
            else if (sender == txtWrtNo)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtWrtNo.Text.Trim() == "")
                    {
                        fn_Screen_Display();
                        fn_Hea_Memo_Screen();
                    }
                }
            }
        }

        void eSpreadLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (sender == SS2)
            {
                string strExcode  ="";
                string strGubun   ="";
                string strCODE    ="";
                string strResType ="";
                string strNomal   ="";
                string strResult = "";

                if (e.Column != 2)
                {
                    return;
                }

                strCODE = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();

                strExcode = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strGubun = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                strResType = SS2.ActiveSheet.Cells[e.Row, 10].Text.Trim();
                if (strResType == "3")
                {
                    return;
                }

                if (strCODE == "")
                {
                    SS2.ActiveSheet.Cells[e.Row, 4].Text = "";
                }
                else
                {
                    SS2.ActiveSheet.Cells[e.Row, 4].Text = hb.READ_ResultName(strGubun, strCODE);
                    if (strExcode == "A103" && strCODE == "본인제외")
                    {
                    }
                    else
                    {
                        if (SS2.ActiveSheet.Cells[e.Row, 2].Text == "")
                        {
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                            MessageBox.Show(strCODE + "가 결과코드값에 등록이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                strResult = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                strNomal = SS2.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                SS2.ActiveSheet.Cells[e.Row, 5].Text = hb.Result_Panjeng(strCODE, strResult, strNomal);
            }
            else if (sender == SS_ETC)
            {
                if (SS_ETC.ActiveSheet.Cells[e.Row, 2].Text.Trim().Length > 500)
                {
                    SS_ETC.ActiveSheet.Cells[e.Row, 2].Text = VB.Left(SS_ETC.ActiveSheet.Cells[e.Row, 2].Text, 500);
                    MessageBox.Show("저장할수 있는 최대글자 길이는 500자를 넘을수 없습니다!!", "확인창", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            //else if (sender == SS_Res)
            //{
            //    if (SS_Res.ActiveSheet.Cells[e.Row, e.Column].Text.Length > 2000)
            //    {
            //        SS_Res.ActiveSheet.Cells[e.Row, e.Column].Text = VB.Left(SS_Res.ActiveSheet.Cells[e.Row, e.Column].Text, 2000);
            //        MessageBox.Show("결과가 2000자를 초과함: " + SS_Res.ActiveSheet.Cells[e.Row, e.Column].Text.Length, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        SS_Res.ActiveSheet.SetActiveCell(e.Row, e.Column);
            //        return;
            //    }
            //}
        }

        void echkClick(object sender, EventArgs e)
        {
            if (sender == chkEKG)
            {
                string strGbEkg = "";

                if (txtWrtNo.Text == "")
                {
                    return;
                }

                if (chkEKG.Checked == true)
                {
                    strGbEkg = "*";
                }
                else
                {
                    strGbEkg = "";
                }

                int result = heaJepsuService.UpdateGbKg(strGbEkg, FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("등록오류!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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

        string fn_BlankLine_Delete(string argData)
        {
            string rtnVal = "";
            string strResult = "";
            int nPos1 = 0;
            int nPos2 = 0;
            int nPos3 = 0;
            int nPos4 = 0;
            int nPos5 = 0;

            strResult = argData;

            do
            {
                nPos1 = VB.InStr(strResult, Keys.Tab.ToString());
                if (nPos1 > 0) strResult = strResult.Replace(Keys.Tab.ToString(), " ");
                nPos2 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos2 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos3 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos3 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos4 = VB.InStr(strResult, Environment.NewLine + " " + Environment.NewLine);
                if (nPos4 > 0) strResult = strResult.Replace(Environment.NewLine + " " + Environment.NewLine, Environment.NewLine);
                nPos5 = VB.InStr(strResult, "  ");
                if (nPos5 > 0) strResult = strResult.Replace("  ", " ");
            }
            while (nPos1 != 0 && nPos2 != 0 && nPos3 != 0 && nPos4 != 0 && nPos5 != 0);

            rtnVal = strResult;

            return rtnVal;
        }
    }
}
