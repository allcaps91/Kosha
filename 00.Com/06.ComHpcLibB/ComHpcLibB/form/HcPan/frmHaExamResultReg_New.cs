using ComBase;
using ComBase.Controls;
using ComEmrBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaExamResultReg_New.cs
/// Description     : 종합검진판정
/// Author          : 이상훈
/// Create Date     : 2019-10-08
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanjeng_New.frm(HaMain05_New)" />

namespace ComHpcLibB
{
    public partial class frmHaExamResultReg_New : Form
    {
        /// <summary>
        /// The history of the contents of the TextBox.
        /// </summary>
        private Stack<string> _editingHistory = new Stack<string>();

        /// <summary>
        /// The history of TextBox contents that have been undone and can be redone.
        /// </summary>
        private Stack<string> _undoHistory = new Stack<string>();


        [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        HeaJepsuService heaJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaWomenService heaWomenService = null;
        HicCodeService hicCodeService = null;
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;
        HicRescodeService hicRescodeService = null;
        HeaJepsuMemoService heaJepsuMemoService = null;
        BasBcodeService basBcodeService = null;
        HicResultHisService hicResultHisService = null;
        ComHpcLibBService comHpcLibBService = null;
        XrayResultnewService xrayResultnewService = null;
        EtcJupmstService etcJupmstService = null;
        XrayResultnewDrService xrayResultnewDrService = null;
        ExamAnatmstService examAnatmstService = null;
        HeaResultwardService heaResultwardService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HeaJepsuHicLtdPatientSangdamService heaJepsuHicLtdPatientSangdamService = null;
        HeaManageService heaManageService = null;
        HeaMemoService heaMemoService = null;
        HeaResultService heaResultService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HicMemoService hicMemoService = null;

        frmHaPanjeng_New fHaPanjeng_New = null;
        frmHcLtdHelp FrmHcLtdHelp = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO = 0;
        long FnPano = 0;
        string FstrSex = "";
        string FstrGjJong = "";
        string FstrSDate = "";
        string FstrRowid = "";
        long FnAge = 0;
        
        string FstrPANO ="";
        long FnIEMunNo;
        string FstrSName;
        int FnSelLength = 0;
        int currentPosition = 0;

        public frmHaExamResultReg_New()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
            heaJepsuService = new HeaJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            heaWomenService = new HeaWomenService();
            hicCodeService = new HicCodeService();
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            hicRescodeService = new HicRescodeService();
            heaJepsuMemoService = new HeaJepsuMemoService();
            basBcodeService = new BasBcodeService();
            hicResultHisService = new HicResultHisService();
            comHpcLibBService = new ComHpcLibBService();
            xrayResultnewService = new XrayResultnewService();
            etcJupmstService = new EtcJupmstService();
            xrayResultnewDrService = new XrayResultnewDrService();
            examAnatmstService = new ExamAnatmstService();
            heaResultwardService = new HeaResultwardService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            heaJepsuHicLtdPatientSangdamService = new HeaJepsuHicLtdPatientSangdamService();
            heaManageService = new HeaManageService();
            heaMemoService = new HeaMemoService();
            heaResultService = new HeaResultService();
            heaSunapdtlService = new HeaSunapdtlService();
            hicMemoService = new HicMemoService();

            SSList.Initialize(new SpreadOption { RowHeaderVisible = true, ColumnHeaderHeight = 35, RowHeight = 24 });
            SSList.AddColumn("종류",      nameof(HEA_JEPSU.GJJONG),   44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("성명",      nameof(HEA_JEPSU.SNAME),    62, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진일자",  nameof(HEA_JEPSU.SDATE),    78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("회사명",    nameof(HEA_JEPSU.LTDNAME),  82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("상태",      nameof(HEA_JEPSU.GBSTS_NM), 62, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("접수번호",  nameof(HEA_JEPSU.WRTNO),    60, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("가판정",    nameof(HEA_JEPSU.NRNAME),   56, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("판정",      nameof(HEA_JEPSU.DRNAME),   56, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            _editingHistory.Push(txtPanjeng.Text);
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.VisibleChanged += new EventHandler(eFormActivated);
            //this.Activated += new EventHandler(eFormActivated);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnAmSave.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);

            this.chkAm13.Click += new EventHandler(eChkClick);

            this.btnGaSave.Click += new EventHandler(eMenuClick);
            this.btnDelete.Click += new EventHandler(eMenuClick);
            this.btnSangdam.Click += new EventHandler(eMenuClick);
            this.btnTel.Click += new EventHandler(eMenuClick);
            this.btnHang.Click += new EventHandler(eMenuClick);
            this.btnAddTel.Click += new EventHandler(eMenuClick);
            this.btnNoCounsel.Click += new EventHandler(eMenuClick);
            this.btnHistory.Click += new EventHandler(eMenuClick);
            this.btnIEMunjin.Click += new EventHandler(eMenuClick);
            this.btnCancerReg.Click += new EventHandler(eMenuClick);

            this.dtpFrDate.TextChanged += new EventHandler(eDtpChanged);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellClick += new CellClickEventHandler(eSpdClick);
            this.txtPanjeng.Click += new EventHandler(eTxtClick);
            this.txtPanjeng.Leave += new EventHandler(eTxtLeave);
            this.txtPanjeng.TextChanged += new EventHandler(eTxtChanged);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtPanjeng.KeyDown += new KeyEventHandler(eTxtKeyDown);
            
            this.txtResult1.DoubleClick += new EventHandler(eTxtDoubleClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoJob3.Click += new EventHandler(eRdoClick);
            this.rdoJob4.Click += new EventHandler(eRdoClick);

            this.btnRedo.Click += new EventHandler(eBtnClick);
            this.btnUndo.Click += new EventHandler(eBtnClick);
        }

        private void eTxtLeave(object sender, EventArgs e)
        {
            currentPosition = txtPanjeng.SelectionStart;
        }

        private void eTxtChanged(object sender, EventArgs e)
        {
            if (txtPanjeng.Modified)
            {
                RecordEdit();
            }
        }

        /// <summary>
        /// Records an edit made by the user.
        /// </summary>
        private void RecordEdit()
        {
            _editingHistory.Push(txtPanjeng.Text);
            btnUndo.Enabled = true;
            _undoHistory.Clear();
            btnRedo.Enabled = false;
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtPanjeng)
            {
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
                {
                    string strText = Clipboard.GetText();
                    //strText = VB.TR(strText, "\r\n", "");

                    if (strText.Length > 2 && strText.Substring(strText.Length - 2) == "\r\n")
                    {
                        strText = strText.Left(strText.Length-2);
                    }

                    //txtPanjeng.Text += strText;

                    Clipboard.Clear();

                    //txtPanjeng.SelectionStart = txtPanjeng.Text.Length;
                    txtPanjeng.SelectionStart = FnSelLength;
                    txtPanjeng.SelectedText += strText;
                    txtPanjeng.ScrollToCaret();
                }
                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
                {
                    eBtnClick(btnUndo, new EventArgs());
                    return;
                }
            }
        }


        private void eRdoClick(object sender, EventArgs e)
        {
            if (clsHcVariable.GnHicLicense > 0)
            {
                if (sender == rdoJob1)
                {
                    tabControl2.Height = 450;
                }
                else
                {
                    tabControl2.Height = 280;
                }
            }
            
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                sp.setSpdSort(SSList, e.Column, true);
                return;
            }
        }

        private void eDtpChanged(object sender, EventArgs e)
        {
            dtpToDate.Text = dtpFrDate.Text;
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (!clsHcVariable.GstrPanWRTNO.IsNullOrEmpty())
            {
                if (clsHcVariable.GstrPanWRTNO == txtWrtNo.Text)
                {
                    return;
                }

                txtWrtNo.Text = clsHcVariable.GstrPanWRTNO.ToString();
                btnPacs.Enabled = false;
                btnEMR.Enabled = false;
                FstrSName = heaJepsuService.GetSnameByWrtno(clsHcVariable.GstrPanWRTNO.To<long>(0));
                fn_Screen_Display();
                fn_Hea_Memo_Screen();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            pnlResult.Dock = DockStyle.Fill;
            btnPacs.Enabled = false;
            btnEMR.Enabled = false;

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            fn_Screen_Clear();
            sp.Spread_All_Clear(SSList);
            sp.Spread_Clear_Simple(SSPano, 1);

            //fn_HeaResult_Word_SET();  //예전 방식이라 주석처리함

            btnAddTel.Enabled = false;
            btnAddTel.Text = "최종상담";

            txtLtdCode.Text = "";

            btnGaSave.Visible = false;
            btnSangdam.Visible = false;
            btnDelete.Visible = false;

            //2021-04-05
            btnTel.Visible = false;
            btnHang.Visible = false;
            btnNoCounsel.Visible = false;
            btnCancerReg.Visible = false;
            btnSangdam.Visible = false;




            if (clsHcVariable.GnHicLicense > 0)
            {
                label3.Text = "판정일자";
                lblFormTitle.Text = "종합검진 판정";
                btnDelete.Text = "판정취소";
                btnSangdam.Visible = false;
                btnAddTel.Visible = false;
                rdoJob1.Enabled = true;
                rdoJob1.Visible = true;
                this.Text = "종합검진판정 【판정의사】" + cf.Read_SabunName(clsDB.DbCon, clsType.User.IdNumber);
                lblJob.Text = "판정의사 : ";
                lblDate.Text = "";
                chkYN.Text = "판정완료";
                tabControl2.Height = 280;
            }
            else
            {
                label3.Text = "가판정일자";
                lblFormTitle.Text = "종합검진 가판정";
                btnDelete.Text = "가판정취소";
                rdoJob1.Enabled = false;
                rdoJob1.Visible = false;
                this.Text = "종합검진 가판정【작업자】" + cf.Read_SabunName(clsDB.DbCon, clsType.User.IdNumber);
                lblJob.Text = "가판정간호사 : ";
                lblDate.Text = "";
                chkYN.Text = "가판정완료";
                tabControl2.Height = 450;

                btnTel.Visible = true;
                btnHang.Visible = true;
                btnNoCounsel.Visible = true;
                btnCancerReg.Visible = true;

            }
        }

        void fn_HeaResult_Word_SET()
        {
            int inx = 0;
            int nRead = 0;

            sp.Spread_All_Clear(SSHelp);

            //DB에서 자료를 SELECT
            List<HEA_RESULTWARD> list = heaResultwardService.GetCodeNameBySabunGubun(clsType.User.IdNumber, "01");

            nRead = list.Count;
            SSHelp.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                SSHelp.ActiveSheet.Cells[i, 1].Text = list[i].CODE.Trim();
                SSHelp.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME.Trim();
                if (SSHelp.ActiveSheet.ActiveRowIndex < nRead)
                {
                    SSHelp.ActiveSheet.Cells[i * 2, 1, i * 2, 2].BackColor = Color.FromArgb(232, 255, 232);
                }
            }
        }

        void fn_Screen_Clear()
        {
            FnWRTNO = 0;
            txtWrtNo.Text = "";
            FnPano = 0;

            pnlResult.Visible = false;

            txtPanjeng.ReadOnly = false;
            txtResult1.ReadOnly = false;

            txtPanjeng.Text = "";
            txtSangdam.Text = "";

            txtResult1.Text = "";
            txtResult2.Text = "";
            txtResult3.Text = "";
            txtResult4.Text = "";

            txtSangdam2.Text = "";
            txtSangdam3.Text = "";
            txtSangdam4.Text = "";

            txtDrSabun.Text = "";
            lblDrName.Text = "";
            lblSangdam.Text = "";
            lblGjJong.Text = "";

            sp.Spread_Clear_Simple(SSPano, 1);
            sp.Spread_Clear_Simple(SS2);
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, 5).Value = "";
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, 6).Value = "";
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, 7).Value = "";

            tabControl2.SelectedIndex = 0;
            chkYN.Checked = false;
            lblDate.Text = "";
            btnSave.Enabled = true;
            btnSave.BackColor = Color.White;

            for (int i = 1; i <= 13; i++)
            {
                CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                chkAm.Checked = false;
            }

            txtAmETC.Text = "";
            txtAmETC.Enabled = false;
            FnIEMunNo = 0;
            pnlCancer.Visible = false;
            btnIEMunjin.Visible = false;

            //clsHcVariable.GstrPanWRTNO = "";
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (chkAm13.Checked == true)
            {
                txtAmETC.Enabled = true;
            }
            else
            {
                txtAmETC.Text = "";
                txtAmETC.Enabled = false;
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
            string strSEX = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strDrname = "";
            string strAllResult = "";
            string strAllResult2 = "";

            double nMaxData = 0;  //정상 참고치 (High)
            double nMinData = 0;  //정상 참고치 (Low)
            string strExcode = "";  //검사실코드
            string strTemp = "";
            int nSubREAD = 0;
            long nWRTNO = 0;
            string strGbUse = "";
            long nDrSabun = 0;
            long nNrSabun = 0;
            string strBMI = "";
            string strOK = "";
            //string strGbCall = "";

            FnWRTNO = long.Parse(txtWrtNo.Text);
            sp.Spread_Clear_Simple(SS2);

            txtResult1.Text = "";
            txtResult2.Text = "";
            txtResult3.Text = "";
            txtResult4.Text = "";

            txtSangdam2.Text = "";
            txtSangdam3.Text = "";
            txtSangdam4.Text = "";

            lblSangdam.Text = "";

            tabControl1.TabPages[1].Text = "";
            tabControl1.TabPages[2].Text = "";
            tabControl1.TabPages[3].Text = "";
            
            tabPage2.Text = "";
            tabPage3.Text = "";
            tabPage4.Text = "";

            //SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "";
            //SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "";
            //SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "";

            //인적사항을 READ  Screen_Injek_display
            strBMI = "";

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            txtWrtNo.Enabled = false;
            FnWRTNO = long.Parse(txtWrtNo.Text);

            pnlResult.Visible = true;

            //인적사항을 Display(Screen_Injek_display)
            HEA_JEPSU list = heaJepsuService.GetItembyWrtNoGbSts(FnWRTNO, "0");

            if (list == null)
            {
                MessageBox.Show(FnWRTNO + " 접수번호가 등록 안됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            strSEX = list.SEX.Trim();
            FstrSex = strSEX;
            FstrSDate = list.SDATE.To<string>("");
            FstrRowid = list.RID;
            FstrGjJong = list.GJJONG;
            FnAge = list.AGE;
            FnPano = list.PANO;
            FnIEMunNo = list.IEMUNNO;
            nPano = list.PANO; ;
            FstrPANO = list.PTNO;

            if (string.Compare(FstrPANO, "0") > 0)
            {
                btnPacs.Enabled = true;
                btnEMR.Enabled = true;
            }

            strSDate = list.SDATE.ToString();
            btnIEMunjin.Visible = false;
            if (FnIEMunNo > 0)
            {
                btnIEMunjin.Visible = true;
            }

            SSPano.ActiveSheet.Cells[0, 0].Text = list.PTNO.To<string>("0");
            SSPano.ActiveSheet.Cells[0, 1].Text = list.SNAME.Trim();
            SSPano.ActiveSheet.Cells[0, 2].Text = FnAge + "/" + FstrSex;
            SSPano.ActiveSheet.Cells[0, 3].Text = list.BIRTHDAY.To<string>("").Substring(0, 8);
            SSPano.ActiveSheet.Cells[0, 4].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
            SSPano.ActiveSheet.Cells[0, 5].Text = list.SDATE.ToString();
            //SSPano.ActiveSheet.Cells[0, 6].Text = hb.READ_GjJong_HeaName(list.GJJONG.Trim());
            SSPano.ActiveSheet.Cells[0, 6].Text = heaSunapdtlService.GetMainSunapDtlCodeNameByWrtno(FnWRTNO);
            SSPano.ActiveSheet.Cells[0, 7].Text = !list.WEBPRINTREQ.IsNullOrEmpty() ? "알림톡" : list.GBCHK3 == "Y" ? "방문수령" : list.GBJUSO == "Y" ? "우편(집)" : list.GBJUSO == "N" ? "우편(회사)" : list.GBJUSO == "E" ? "우편(별도)" : "";

            //일반검진 접수 내역 Display
            List<HIC_JEPSU> lstHJ = hicJepsuService.GetListGjNameByPtnoJepDate(strSDate, FstrPANO);
            if (lstHJ.Count > 0)
            {
                lblGjJong.Text = "일반검진 내역 ▶ ";

                for (int i = 0; i < lstHJ.Count; i++)
                {
                    lblGjJong.Text += lstHJ[i].NAME + ", ";
                }
            }

            //BMI검사
            if (string.Compare(hm.Biman_Gesan(FnWRTNO, "HEA"), "0") > 0)
            {
                strBMI = "\r\n" + "  - 비만도 (BMI " + hm.Biman_Gesan(FnWRTNO, "HAPAN", FstrSex) + "):  " + VB.Pstr(hm.Biman_Gesan(FnWRTNO, "HEA"), ".", 2) + "\r\n";
            }

            strAllResult = list.PANREMARK;
            //strAllResult = VB.TR(strAllResult, "\r\n", "\n");
            //strAllResult = VB.TR(strAllResult, "\n", "\r\n");

            if (VB.Left(VB.Pstr(strAllResult, "비만도 (", 2), 3) != "BMI")
            {
                if (VB.Pstr(hm.Biman_Gesan(FnWRTNO, "HEA"), ".", 1) != "02")
                {
                    strAllResult += strBMI;
                }
                else
                {
                    strAllResult += "\r\n" + VB.Pstr(strBMI, "\r\n", 2) + "입니다."; 
                }
            }

            //심전도 ABNORNAML 인경우 결과 판독한경우
            txtPanjeng.Text = strAllResult;

            if (list.PANREMARK2.To<string>("").Trim() != "")
            {
                strAllResult2 = list.PANREMARK2;
                //strAllResult2 = VB.TR(strAllResult2, "\r\n", "\n");
                //strAllResult2 = VB.TR(strAllResult2, "\n", "\r\n");
                txtPanjeng.Text += strAllResult2;
            }

            if (txtPanjeng.Text != "")
            {
                txtPanjeng.SelectionStart = txtPanjeng.MaxLength - 1;
                txtPanjeng.SelectionLength = 1;
            }

            txtSangdam.Text = list.SANGDAM.To<string>("");

            nDrSabun = list.DRSABUN;
            nNrSabun = list.NRSABUN;
            strDrname = list.DRNAME;

            if (clsHcVariable.GnHicLicense == 0)
            {
                if (nNrSabun > 0)
                {
                    txtDrSabun.Text = nNrSabun.ToString();
                    chkYN.Checked = true;
                }
                else
                {
                    txtDrSabun.Text = "";
                }
                lblDrName.Text = cf.Read_SabunName(clsDB.DbCon, txtDrSabun.Text.Trim());
                lblDate.Text = list.GAPANDATE.To<string>("").Trim();
            }
            else
            {
                if (nDrSabun > 0)
                {
                    chkYN.Checked = true;
                    txtDrSabun.Text = nDrSabun.ToString();
                }
                else
                {
                    txtDrSabun.Text = "";
                }
                lblDrName.Text = cf.Read_SabunName(clsDB.DbCon, txtDrSabun.Text.Trim());
                lblDate.Text = list.PANDATE;
            }

            if (nDrSabun > 0 && strDrname != "")
            {
                //판정완료의 경우
                if (nDrSabun != long.Parse(clsType.User.IdNumber))
                {
                    btnSave.Enabled = false;
                    btnSave.BackColor = Color.Pink;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnSave.Enabled = true;
            }

            if (list.SANGDAMGBN.To<string>("").Trim() != "")
            {
                if (VB.Pstr(list.SANGDAMGBN.To<string>().Trim(), "^^", 2) == "1")
                {
                    lblSangdam.Text = "상담일시 " + VB.Pstr(list.SANGDAMGBN.Trim(), "^^", 1);
                    if (list.SANGSABUN > 0)
                    {
                        lblSangdam.Text += " " + cf.Read_SabunName(clsDB.DbCon, list.SANGSABUN.To<string>("0").Trim());
                    }
                }
            }

            if (list.SANGDAMTEL.To<string>("").Trim() != "")
            {
                lblSangdam.Text = "☎전화상담 " + VB.Pstr(list.SANGDAMTEL, "^^", 1);
                lblSangdam.Text += " " + cf.Read_SabunName(clsDB.DbCon, VB.Pstr(list.SANGDAMTEL.Trim(), "^^", 2));
            }

            if (list.SANGDAMOUT.To<string>("").Trim() != "")
            {
                lblSangdam.Text = "☎연락부재 " +  VB.Pstr(list.SANGDAMOUT.Trim(), "^^", 1);
                lblSangdam.Text += " " + cf.Read_SabunName(clsDB.DbCon, VB.Pstr(list.SANGDAMOUT.Trim(), "^^", 2));
            }

            if (list.SANGDAMNOT.To<string>("").Trim() != "")
            {
                lblSangdam.Text = "상담거절 " + VB.Pstr(list.SANGDAMNOT.Trim(), "^^", 1);
                lblSangdam.Text += " " + cf.Read_SabunName(clsDB.DbCon, VB.Pstr(list.SANGDAMNOT.Trim(), "^^", 2));
            }

            if (list.SANGDAMYN.To<string>("").Trim() != "")
            {
                lblSangdam.Text = "◈최종상담 " + VB.Pstr(list.SANGDAMYN, "^^", 1);
                lblSangdam.Text += " " + cf.Read_SabunName(clsDB.DbCon, VB.Pstr(list.SANGDAMYN, "^^", 2));
                btnAddTel.Text = "최종상담 해제";
            }
            else
            {
                btnAddTel.Text = "최종상담";
            }

            btnAddTel.Enabled = true;

            //if (list.PANMEMO..Trim() != "")
            //{
            //    //txtMemo.Text = list.PANMEMO.Trim();
            //}

            if (list.GBAM.To<string>("").Trim() != "")
            {
                for (int i = 1; i <= 13; i++)
                {
                    if (VB.Pstr(list.GBAM.To<string>("").Trim(), ",", i) == "1")
                    {
                        CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                        chkAm.Checked = true;
                    }
                }
            }

            if (chkAm13.Checked == true)
            {
                txtAmETC.Text = list.ETCAM.To<string>("").Trim();
            }

            if (clsType.User.IdNumber == "23515")
            {
                btnSave.Enabled = true;
            }

            //검사결과를 재판정
            hm.ExamResult_RePanjeng_Hea(FnWRTNO, FstrSex, strSDate);    //검사결과를 재판정

            //검사항목을 Display  Screen_Exam_Items_display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItemNoActingbyWrtNo(FnWRTNO, "Y");

            nREAD = list3.Count;
            nRow = 0;

            strAllResult = "";
            
            for (int i = 0; i < nREAD; i++)
            {

                strCODE = list3[i].EXCODE.To<string>("").Trim();
                strResult = list3[i].RESULT.To<string>("").Trim();
                strResCode = list3[i].RESCODE.To<string>("").Trim();
                strResultType = list3[i].RESULTTYPE.To<string>("").Trim();
                strGbCodeUse = list3[i].GBCODEUSE.To<string>("").Trim();
                strExcode = list3[i].EXCODE.To<string>("").Trim();


                if (strCODE == "E911")
                {
                    strCODE = "E911";
                }


                //심전도, 치아검사 내역 강제
                if (strCODE == "ZD00")
                {
                    strResultType = "2";
                    strAllResult += "◈" + list3[i].HNAME.To<string>("").Trim() + "◈ 【" + strCODE + "】" + "\r\n";
                    strAllResult += string.Concat(Enumerable.Repeat("-", 50)) + "\r\n";
                    strAllResult += list3[i].RESULT.To<string>("") + "\r\n" + "\r\n";
                }

                if (strCODE == "A151")
                {
                    strAllResult += "◈" + list3[i].HNAME.To<string>("").Trim() + "◈ 【" + strCODE + "】" + "\r\n";
                    strAllResult += string.Concat(Enumerable.Repeat("-", 50)) + "\r\n";
                    strAllResult += list3[i].RESULT.To<string>("").Replace("\n", "\r\n") + "\r\n" + "\r\n";
                    strResultType = "2";
                }

                if (strResultType == "3")
                {
                    strAllResult += "◈" + list3[i].HNAME.To<string>("").Trim() + "◈ 【" + strCODE + "】" + "\r\n";
                    strAllResult += string.Concat(Enumerable.Repeat("-", 50)) + "\r\n";
                    strAllResult += list3[i].RESULT.To<string>("") + "\r\n" + "\r\n";
                }

                nRow += 1;
                if (nRow > SS2.ActiveSheet.RowCount) SS2.ActiveSheet.RowCount = nRow;

                SS2.ActiveSheet.Cells[i, 0].Text = strCODE.To<string>("");
                SS2.ActiveSheet.Cells[i, 1].Text = list3[i].HNAME.To<string>("").Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220);

                if (list3[i].PANJENG.To<string>("").Trim() == "2")
                {
                    SS2.ActiveSheet.Cells[i, 3].Text = "*";
                }

                //참고치를 Dispaly
                if (FstrSex == "M")
                {
                    strNomal = list3[i].MIN_M.To<string>("") + "~" + list3[i].MAX_M.To<string>("").Trim();
                    nMinData = list3[i].MIN_M.To<double>();
                    nMaxData = list3[i].MAX_M.To<double>();
                }
                else
                {
                    strNomal = list3[i].MIN_F.To<string>("").Trim() + "~" + list3[i].MAX_F.To<string>("").Trim();
                    nMinData = list3[i].MIN_F.To<double>();
                    nMaxData = list3[i].MAX_F.To<double>();
                }

                if (nMinData != 0 || nMaxData != 0)
                {
                    if (strResult.Trim() != "" && strResult != ".")
                    {
                        switch (hb.Result_Panjeng_New(list3[i].EXCODE.To<string>(""), strResult, strNomal))
                        {
                            case "B":
                                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                                break;
                            case "H":
                                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if ((VB.Left(strResult, 2) == "양성" || VB.Left(strResult.Trim(), 1) == "+") && strCODE.To<string>("").Trim() != "H841" && strCODE.To<string>("").Trim() != "A132")
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
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                            break;
                        case "+-":
                            if (strCODE.To<string>("").Trim() != "LU46" && strCODE.To<string>("").Trim() != "A259")
                            {
                                SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
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

                if (strNomal == "음성(-)")
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
                if (strNomal.Trim() == "정상" && strResult.Trim() != "" && strResult.Trim() != ".")
                {
                    if (strNomal.Trim() != strResult.Trim())
                    {
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }

                //강제색상변경
                if (strCODE == "A289" && VB.UCase(strResult) == "TRACE")
                {
                    SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                }

                if (strCODE == "LU39" && strResult != "-" && strResult != "음성")
                {
                    SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                }
                if (strCODE == "LU42" && strResult != "yellow" && strResult != ".") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU44" && strResult != "-" && strResult != "음성") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU45" && strResult != "-" && strResult != "음성") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU46" && strResult != "-" && strResult != "+-" && strResult != "음성") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU47" && strResult != "-" && strResult != "음성")  SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU48" && strResult != "-" && strResult != "음성")  SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU49" && strResult != "-" && strResult != "음성")  SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "LU50" && strResult != "-" && strResult != "음성") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);

                if (strCODE == "TE05" && strResult != "정상") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "H841" && strResult == "-") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "TH28" && VB.Left(strResult, 8) == "정밀검사") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "ZE13" && VB.Left(strResult, 4) != "정상") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "A212" || strCODE == "TZ46") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);

                if (strCODE == "A241" && strResult == ">1000") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);

                switch (strCODE)
                {
                    case "TE05":
                    case "TE13":
                    case "TE14":
                    case "TE15":
                    case "TE16":
                    case "TE17":
                    case "TE18":
                    case "TE19":
                    case "TE20":
                    case "TE21":
                    case "TE22":
                    case "TE24":
                    case "TE31":
                    case "TE32":
                    case "TE42":
                        if (strResult != "정상" && strResult != ".")
                        {
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                        }
                        break;
                    default:
                        break;
                }

                if (strCODE == "A424") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(255, 210, 222);
                if (strCODE == "A151" && strResult != "정상" && strResult != ".") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "ZD00" && VB.L(strResult, "정상") == 1) SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "A258" && strResult != "-") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "A259") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "A456") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "ZD06" && strResult != "음성") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "ZD08" && (strResult != "-" && strResult != "음성")) SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "H837") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);

                if (strCODE == "TH15" && strResult == "측정불가") SS2.BackColor = SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strCODE == "TH25" && strResult == "측정불가") SS2.BackColor = SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strResult == "측정불가") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                if (strResult == "본인제외") SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);

                if (strCODE == "A103")
                {   
                    strTemp = hm.Biman_Gesan(FnWRTNO, "HEA");
                    switch (VB.Pstr(strResult, ".", 1))
                    {
                        case "02":
                            break;
                        case "01":
                        case "03":
                        case "04":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                            break;
                        case "05":
                        case "06":
                        case "07":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                            break;
                        default:
                            break;
                    }
                    SS2.ActiveSheet.Cells[i, 2].Text = VB.Pstr(strTemp, ".", 1);
                }

                if (strCODE == "E911")
                {
                    if(strResult !="-")
                    {
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;

                if (list3[i].SANGDAM.To<string>("").Trim() == "Y")
                {
                    SS2.ActiveSheet.Cells[i, 0].BackColor = Color.Orange;
                    SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                }
                else
                {
                    SS2.ActiveSheet.Cells[i, 0].BackColor = Color.White;
                    SS2.ActiveSheet.Cells[i, 8].Text = "N";
                }
            }

            tabControl1.Visible = true;

            if (strAllResult != "")
            {
                strAllResult = VB.TR(strAllResult, "\r", "\r\n");
                strAllResult = strAllResult.Replace("\n", "\r\n");
                txtResult1.Text = strAllResult;
            }
            else
            {
                txtResult1.Text = "";
            }

            SS2.ActiveSheet.RowCount = nRow;

            //종전결과 3개를 Display
            List<HEA_JEPSU> list4 = heaJepsuService.GetItembyPaNoSDateGbSts(nPano, strSDate);

            nSubREAD = list4.Count;
            for (int i = 0; i < nSubREAD; i++)
            {
                if (i > 2)
                {
                    break;
                }

                //tabControl1.TabIndex = i + 1;
                tabControl1.TabPages[i + 1].Text = list4[i].SDATE.To<string>("").ToString();
                strAllResult = "";
                if (list4[i].PANREMARK.To<string>("") != "")
                {
                    strAllResult = "◈ 판정결과(" + list4[i].DRNAME.To<string>("") + ") ◈" + "\r\n";
                    strAllResult += string.Concat(Enumerable.Repeat("-", 50)) + "\r\n";
                    strAllResult += list4[i].PANREMARK.To<string>("") + "\r\n" + "\r\n";
                }
                nCol = i + 5;
                //SS2.ActiveSheet.Cells[0, nCol].Text = list4[i].SDATE.To<string>("").ToString();
                SS2.ActiveSheet.ColumnHeader.Cells.Get(0, nCol).Value = list4[i].SDATE.To<string>("").ToString();
                nWRTNO = list4[i].WRTNO.To<long>(0);

                //검사항목을 Display
                //Screen_Exam_Items_OLD();  //종전결과
                List<HIC_RESULT_EXCODE> list5 = hicResultExCodeService.GetItembyWrtNoResult(nWRTNO, "HEA");

                nREAD = list5.Count;
                nRow = 0;
                for (int j = 0; j < nREAD; j++)
                {
                    strCODE = list5[j].EXCODE.To<string>("").Trim();
                    strResult = list5[j].RESULT.To<string>("").Trim();
                    strResCode = list5[j].RESCODE.To<string>("").Trim();
                    strResultType = list5[j].RESULTTYPE.To<string>("").Trim();
                    strGbCodeUse = list5[j].GBCODEUSE.To<string>("").Trim();
                    strGbUse = list5[j].GBUSE.To<string>("").Trim();

                    if (strResultType == "3")
                    {
                        strAllResult += "◈" + list5[j].HNAME.To<string>("") + "◈" + "\r\n";
                        strAllResult += string.Concat(Enumerable.Repeat("-", 50)) + "\r\n";
                        strAllResult += list5[j].RESULT.To<string>("") + "\r\n" + "\r\n";
                    }
                    else
                    {

                        nRow = 0; strOK = "OK";
                        for (int k = 0; k < SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k, 0].Text == strCODE)
                            {
                                nRow = k;
                                strOK = "";
                                break;
                            }
                        }

                        //strOK = "";
                        //if (nRow == 0 && !SS2.ActiveSheet.Cells[nRow, nCol].Text.IsNullOrEmpty())
                        //{
                        //    strOK = "OK";
                        //}

                        if (nRow >= 0 && strOK =="")
                        {
                            SS2.ActiveSheet.Cells[nRow, nCol].Text = strResult;
                            SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(190, 250, 220);
                            if (strResult != "" && strResCode != "")
                            {
                                SS2.ActiveSheet.Cells[nRow, nCol].Text = hb.READ_ResultName(strResCode, strResult);
                            }

                            //참고치를 Display
                            if (FstrSex == "M")
                            {
                                strNomal = list5[j].MIN_M.To<string>("") + "~" + list5[j].MAX_M.To<string>("");
                                nMinData = list5[j].MIN_M.To<double>();
                                nMaxData = list5[j].MAX_M.To<double>();
                            }
                            else
                            {
                                strNomal = list5[j].MIN_F.To<string>("") + "~" + list5[j].MAX_F.To<string>("");
                                nMinData = list5[j].MIN_F.To<double>(); 
                                nMaxData = list5[j].MAX_F.To<double>();
                            }

                            if (nMinData != 0 || nMaxData != 0)
                            {
                                switch (hb.Result_Panjeng(list5[j].EXCODE.To<string>(""), strResult, strNomal))
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
                            else if ((VB.Left(strResult, 2) == "양성" || VB.Left(strResult.Trim(), 1) == "+") && strCODE.To<string>("").Trim() != "H841" && strCODE.To<string>("").Trim() != "A132" && strCODE.To<string>("").Trim() != "A259")
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
                                        SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        break;
                                    case "+-":
                                        if (strCODE.To<string>("").Trim() != "LU46" && strCODE.To<string>("").Trim() != "A259")
                                        {
                                            SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (strCODE == "A103")
                            {
                                strTemp = hm.Biman_Gesan(FnWRTNO, "HEA");
                                if (!strTemp.IsNullOrEmpty())
                                {
                                    if (long.Parse(VB.Left(strTemp, 2)) > 3)
                                    {
                                        SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.FromArgb(250, 210, 222);
                                    }
                                }
                            }
                        }
                    }
                }

                if (strAllResult != "")
                {
                    strAllResult = strAllResult.Replace("\n", "\r\n");
                    //strAllResult = strAllResult.Replace("\r", "\r\n");
                    TextBox txtResult = (Controls.Find("txtResult" + (i + 2).ToString(), true)[0] as TextBox);
                    txtResult.Text = strAllResult;
                }
                else
                {
                    TextBox txtResult = (Controls.Find("txtResult" + (i + 2).ToString(), true)[0] as TextBox);
                    txtResult.Text = "";
                }

                if (!list4[i].SANGDAM.IsNullOrEmpty())
                {
                    TextBox txtSangdam = (Controls.Find("txtSangdam" + (i + 2).ToString(), true)[0] as TextBox);
                    txtSangdam.Text = list4[i].SANGDAM.To<string>("");
                }
                else
                {
                    TextBox txtSangdam = (Controls.Find("txtSangdam" + (i + 2).ToString(), true)[0] as TextBox);
                    txtSangdam.Text = "";
                }
            }

            fn_Hea_Manage_Dispaly(FnWRTNO);

            tabControl1.SelectedIndex = 0;
        }

        void fn_Hea_Manage_Dispaly(long argWrtNo)
        {
            //long M = 0;

            //전화상담 구분
            if (heaJepsuService.GetSangdamTelbyWrtno(argWrtNo).To<string>("").Trim() == "")
            {
                btnTel.Text = "전화상담";
            }
            else
            {
                btnTel.Text = "전화상담해제";
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                clsHcVariable.GstrPanWRTNO = "";
                fn_Screen_Clear();
            }
            else if (sender == btnAmSave)
            {
                string strAm = "";

                for (int i = 1; i <= 13; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                    if (chkAm.Checked == true)
                    {
                        strAm += "1,";
                    }
                    else
                    {
                        strAm += "0,";
                    }
                }

                int result = heaJepsuService.UpdateGbAmETCAmbyWrtNo(strAm, txtAmETC.Text.Trim(), long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                pnlCancer.Visible = false;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                long nWRTNO = 0;
                string strLtdCode = "";
                string strLtd = "";
                
                string sJob = "";
                string strFrDate = "";
                string strToDate = "";
                string strSort = "";

                string[] strGubun = { "12", "13", "14" };

                //fn_Hea_Manage_Insert();

                pnlResult.Visible = false;                
                strLtdCode = "";
                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 5;

                if (rdoJob1.Checked == true)
                {
                    sJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    sJob = "2";
                }
                else if (rdoJob3.Checked == true)
                {
                    sJob = "3";
                }
                else if (rdoJob4.Checked == true)
                {
                    sJob = "4";
                }

                btnAddTel.Enabled = false;
                btnAddTel.Text = "최종상담";

                if (txtLtdCode.Text.Trim() != "")
                {
                    if (txtLtdCode.Text.Trim().IndexOf(".") < 0)
                    {
                        txtLtdCode.Text += "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    }                    
                }
                else
                {
                    txtLtdCode.Text = "";
                }

                if (txtLtdCode.Text.Trim().IndexOf(".") < 0)
                {
                    strLtdCode = txtLtdCode.Text.Trim();
                }
                else
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1);
                }

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                Cursor.Current = Cursors.WaitCursor;

                string strWToDate = cf.DATE_ADD(clsDB.DbCon, strToDate, 1);
                // 신규접수 및 접수수정 자료를 SELECT
                List<HEA_JEPSU_HIC_LTD_PATIENT_SANGDAM> list = heaJepsuHicLtdPatientSangdamService.GetItembyEntTimeGubun(clsType.User.IdNumber, sJob, strFrDate, strToDate, strWToDate, strLtdCode, strSort, clsHcVariable.GnHicLicense, txtSName.Text.Trim());

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    if (list[i].MAILDATE.To<string>("") != "")
                    {
                        SSList.ActiveSheet.Cells[i, 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                    }
                    else if (list[i].RECVDATE.To<string>("") != "")
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80FFFF"));
                    }
                    else if (list[i].WEBPRINTREQ.To<string>("") != "")
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = Color.Orange;
                    }
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].GJJONG.Trim();        //건진종류
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();         //성명
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].SDATE.ToString();     //검진일자
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].LTDNAME.To<string>("").Trim();       //회사명

                    if (rdoJob1.Checked == true)
                    {
                        if (list[i].GBCALL.Trim() == "1")
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text = "";
                        }
                        else
                        {
                            if (list[i].SDATE.ToString() == clsPublic.GstrSysDate)
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "당일상담";
                                SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Khaki;
                            }
                            else
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "내원상담";
                            }
                        }
                    }
                    else
                    {
                        SSList.ActiveSheet.Cells[i, 4].Text = "";

                        if (!list[i].SANGDAMGBN.IsNullOrEmpty() && list[i].SANGDAMGBN.To<string>("") != "")
                        {
                            if (VB.Pstr(list[i].SANGDAMGBN.Trim(), "^^", 2) == "1" || VB.Pstr(list[i].SANGDAMGBN.Trim(), "^^", 2) == "2")
                            {
                                if (list[i].IDATE.To<string>("") != "")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "내원상담";
                                }
                                else
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "당일상담";
                                    SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Khaki;
                                }
                            }
                        }
                        

                        HEA_SANGDAM_WAIT list2 = heaSangdamWaitService.GetGBCallbyWrtNoInGubun(nWRTNO, strGubun);

                        if (!list2.IsNullOrEmpty())
                        {
                            if (list[i].SDATE.To<string>("") != list2.ENTTIME.To<string>(""))
                            {
                                if (list2.GBCALL == "Y")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "내원상담";
                                }
                            }
                        }
                        
                        if (!list[i].SANGDAMOUT.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text = "연락부재";
                            SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Orange;
                        }

                        if (!list[i].SANGDAMTEL.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text = "전화상담";
                            SSList.ActiveSheet.Cells[i, 4].BackColor = Color.PeachPuff;
                        }

                        if (!list[i].SANGDAMYN.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text = "최종상담";
                            SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Violet;
                        }

                        if (!list[i].SANGDAMNOT.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text = "상담거절";
                            SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Plum;
                        }
                    }

                    //개인정보보안상담
                    if (list[i].SANGDAM_ONE.To<string>("").Trim() == "Y")
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }

                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].WRTNO.ToString(); //접수번호

                    if (long.Parse(list[i].NRSABUN) > 0)
                    {
                        SSList.ActiveSheet.Cells[i, 6].Text = hb.READ_HIC_InsaName(list[i].NRSABUN); 
                    }

                    if (!list[i].DRSABUN.IsNullOrEmpty() && list[i].DRSABUN.To<string>("") != "0")
                    {
                        SSList.ActiveSheet.Cells[i, 7].Text = hb.READ_HIC_InsaName(list[i].DRSABUN);
                    }

                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPacs)
            {
                frmViewResult f = new frmViewResult(FstrPANO);
                f.ShowDialog(this);

            }
            else if (sender == btnEMR)
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPANO);
            }
            else if (sender == btnLtdCode)
            {
                FrmHcLtdHelp = new frmHcLtdHelp();
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
            else if (sender == btnSave)
            {
                long nDrSabun = 0;
                string strPrtDate = "";
                string strWebPrtDate = "";
                string strDrname = "";

                HEA_JEPSU list = heaJepsuService.GetDrSabunDrNmaebyWrtNo(FnWRTNO);

                nDrSabun = list.DRSABUN;
                strPrtDate = list.PRTDATE.To<string>("").Trim();
                strWebPrtDate = list.WEBPRINTSEND.To<string>("").Trim();
                strDrname = list.DRNAME.To<string>("").Trim();
                
                if (clsHcVariable.GnHicLicense > 0)
                {
                    if (rdoJob1.Checked == true)
                    {
                        eMenuClick(btnSangdam, new EventArgs());   //상담저장
                    }
                    else
                    {
                        if (strPrtDate != "" || strWebPrtDate != "")
                        {
                            MessageBox.Show("결과지 출력처리 완료되었습니다." + "\r\n\r\n" + "(결과지출력 작업취소 후 판정취소 가능)", "판정취소불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            chkYN.Checked = true;
                            return;
                        }
                        else
                        {
                            Data_Save(); //판정저장
                        }
                    }
                }
                else
                {
                    if (nDrSabun > 0 && strDrname != "")
                    {
                        MessageBox.Show("이미 판정이 완료되었습니다. 취소할 수 없습니다.", "취소불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        chkYN.Checked = true;
                        return;
                    }
                    else
                    {
                        eMenuClick(btnGaSave, new EventArgs());   //가판정저장
                    }
                }
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
                        strTime = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                        strMemo = SS_ETC.ActiveSheet.Cells[i, 3].Text.Trim();
                        strROWID = SS_ETC.ActiveSheet.Cells[i, 5].Text.Trim();
                        //신규작성일경우
                        if (strTime == "" && strMemo != "")
                        {
                            int result = comHpcLibBService.InsertHeaMemo(FnWRTNO, strMemo, clsType.User.IdNumber, FstrPANO);

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
            else if (sender == btnRedo)
            {
                _editingHistory.Push(_undoHistory.Pop());
                btnRedo.Enabled = _undoHistory.Count > 0;
                txtPanjeng.Text = _editingHistory.Peek();
                btnUndo.Enabled = true;
            }
            else if (sender == btnUndo)
            {
                if (_editingHistory.Count < 1) { return; }

                _undoHistory.Push(_editingHistory.Pop());
                btnRedo.Enabled = true;

                if (_editingHistory.Count < 1)
                {
                    btnUndo.Enabled = false;
                    return;
                }

                txtPanjeng.Text = _editingHistory.Peek();
                btnUndo.Enabled = _editingHistory.Count > 1;
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

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == btnGaSave)
            {
                string strSangDam_One = "";
                string strPanRemark = "";
                string strPanRemark2 = "";
                string strDate = "";
                string strAm = "";
                string strChk = "";
                string strSysDate = "";

                List<string> PanResult = new List<string>();

                if (FnWRTNO == 0)
                {
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strPanRemark = txtPanjeng.Text.TrimEnd();

                if (strPanRemark == "")
                {
                    MessageBox.Show("판정결과가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strPanRemark2 = "";
                if (ComFunc.LenH(txtPanjeng.Text) > 4000)
                {
                    //strPanRemark = ComFunc.LeftH(strPanRemark, 4000);
                    //strPanRemark2 = ComFunc.MidH(txtPanjeng.Text, 4001, txtPanjeng.Text.Length - 4000);
                    PanResult = ComFunc.GetByteString(txtPanjeng.Text.TrimEnd(), 4000);
                    strPanRemark = PanResult[0];
                    if (PanResult.Count>1)
                    {
                        strPanRemark2 = PanResult[1];
                    }
                }

                strDate = "";
                //기존상담 시각 체크
                strDate = heaJepsuService.GetSangdamGbnbyWrtNo(FnWRTNO);

                //if (!strDate.IsNullOrEmpty())
                //{
                //    strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^1";
                //}

                strSangDam_One = "N";
                
                strAm = "";
                for (int i = 0; i <= 12; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + (i + 1).ToString(), true)[0] as CheckBox);
                    if (chkAm.Checked == true)
                    {
                        strAm += "1,";
                    }
                    else
                    {
                        strAm += "0,";
                    }
                }

                if (chkYN.Checked == true)
                {
                    strChk = "Y";
                }
                else
                {
                    strChk = "";
                }

                strSysDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

                int result = heaJepsuService.UpdateSangdamGbnbyWrtNo(strDate, strSangDam_One, strPanRemark.Replace("'", "`"), strPanRemark2.Replace("'", "`"), strAm, txtAmETC.Text.Trim(), clsType.User.IdNumber, clsHcVariable.GstrRefValue1, long.Parse(txtWrtNo.Text), strChk, strSysDate);

                if (result < 0)
                {
                    MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //종검통계 관리등록
                fn_MemoSave();
                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());

                pnlResult.Visible = false;
                SSList.ActiveSheet.SetActiveCell(0, 0);
            }
            else if (sender == btnDelete)
            {
                if (long.Parse(txtWrtNo.Text) == 0)
                {
                    return;
                }

                if (clsHcVariable.GnHicLicense > 0)
                {
                    if (MessageBox.Show("판정 내용을 취소하시겠습니까?", "판정 취소", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    if (heaJepsuService.GetPrtDatebyWrtNo(long.Parse(txtWrtNo.Text)).ToString() != "")
                    {
                        MessageBox.Show("결과지 출력처리 완료되었습니다." + "\r\n\r\n" + "(결과지출력 작업취소 후 판정취소 가능)", "판정취소불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }

                    int result = heaJepsuService.UpdateDrSabunbyWrtNo(long.Parse(txtWrtNo.Text));

                    if (result < 0)
                    {
                        MessageBox.Show("결과지 출력처리중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("가판정 내용을 취소하시겠습니까?", "가판정 취소", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    if (heaJepsuService.GetDrSabunbyWrtNo(long.Parse(txtWrtNo.Text)) > 0)
                    {
                        MessageBox.Show("이미 판정이 완료되었습니다. 취소할 수 없습니다.", "판정취소불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }

                    int result = heaJepsuService.UpdateNrSabunbyWrtNo(long.Parse(txtWrtNo.Text));

                    if (result < 0)
                    {
                        MessageBox.Show("가판정 취소중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSangdam)
            {
                string strDate = "";
                string strSangDam_One = "";
                string strAm = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("먼저 환자를 선택해 주세요", "환자선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strSangDam_One = "N";
                strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^1";

                strAm = "";
                for (int i = 1; i <= 13; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                    if (chkAm.Checked == true)
                    {
                        strAm += "1,";
                    }
                    else
                    {
                        strAm += "0,";
                    }
                }

                int result = heaJepsuService.UpdateSangdamGbnGbAmbyWrtNo(strDate, strAm, txtAmETC.Text.Trim(), clsType.User.IdNumber, strSangDam_One, long.Parse(txtWrtNo.Text), txtSangdam.Text.Trim());

                if (result < 0)
                {
                    MessageBox.Show("상담여부 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (clsHcVariable.GnHicLicense > 0)
                {
                    //상담대기순번 완료 표시

                    string[] strJongSQL;

                    strJongSQL = new string[] { "12", "13", "14" };

                    int result2 = heaSangdamWaitService.Update_Sangdam_GbCall(long.Parse(txtWrtNo.Text), strJongSQL);

                    if (result2 < 0)
                    {
                        MessageBox.Show("상담대기순번 완료 갱신시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //결과가 안나온것 체크표시(보완)
                List<HEA_RESULT> list = heaResultService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RESULT.To<string>("").Trim() == "")
                    {
                        int result2 = heaResultService.UpdateSangdambyWrtnoExCode(long.Parse(txtWrtNo.Text), list[i].EXCODE.Trim(), "Y");

                        if (result2 < 0)
                        {
                            MessageBox.Show("결과 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                fn_MemoSave();

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnTel)
            {
                string strDate = "";
                string strSabun = "";
                string strWorkTemp = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("먼저 환자를 선택해 주세요", "환자선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^";
                strSabun = clsType.User.IdNumber + "^^";
                strWorkTemp = strDate + strSabun;

                if (heaJepsuService.GetSangdamTelbyWrtno(long.Parse(txtWrtNo.Text)).To<string>("").Trim() != "")
                {
                    strWorkTemp = "";
                }

                int result = heaJepsuService.UpdateSangdamTelbyWrtNo(strWorkTemp, long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("상담여부 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_MemoSave();

                if (strWorkTemp == "")
                {
                    btnTel.Text = "전화상담";
                    MessageBox.Show("전화상담체크 해제 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnTel.Text = "전화상담해제";
                    MessageBox.Show("전화상담체크 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                eBtnClick(btnSearch, new EventArgs());

            }
            else if (sender == btnHang)
            {
                string strDate = "";
                string strSabun = "";
                string strWorkTemp = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("먼저 환자를 선택해 주세요", "환자선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^";
                strSabun = clsType.User.IdNumber + "^^";
                strWorkTemp = strDate + strSabun;

                if (heaJepsuService.GetSangdamOutbyWrtno(long.Parse(txtWrtNo.Text)).To<string>("").Trim() != "")
                {
                    strWorkTemp = "";
                }

                int result = heaJepsuService.UpdateSangdamOutbyWrtNo(strWorkTemp, long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("연락부재 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_MemoSave();

                if (strWorkTemp == "")
                {
                    btnHang.Text = "연락부재(&H)";
                    MessageBox.Show("연락부재등록 해제 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    btnHang.Text = "연락부재해제(&H)";
                    MessageBox.Show("연락부재등록 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnAddTel)
            {
                string strDate = "";
                string strSabun = "";
                string strWorkTemp = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("먼저 환자를 선택해 주세요", "환자선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^";
                strSabun = clsType.User.IdNumber + "^^";
                strWorkTemp = strDate + strSabun;

                if (heaJepsuService.GetSangdamYNbyWrtno(txtWrtNo.Text.To<long>(0)).To<string>("").Trim() != "")
                {
                    strWorkTemp = "";
                }

                if (strWorkTemp == "")
                {
                    if (MessageBox.Show("최종상담을 해제하시겠습니까?", "최종상담삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    btnAddTel.Text = "최종상담";
                }
                else
                {
                    btnAddTel.Text = "최종상담 해제";
                }

                int result = heaJepsuService.UpdateSangdamYNbyWrtNo(strWorkTemp, long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("상담여부 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_MemoSave();
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnNoCounsel)
            {
                string strDate = "";
                string strSabun = "";
                string strWorkTemp = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("먼저 환자를 선택해 주세요", "환자선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                strDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "^^";
                strSabun = clsType.User.IdNumber.Trim() + "^^";
                strWorkTemp = strDate + strSabun;

                if (heaJepsuService.GetSangdamNotbyWrtno(long.Parse(txtWrtNo.Text)).To<string>("").Trim() != "")
                {
                    strWorkTemp = "";
                }

                int result = heaJepsuService.UpdateSangdamNotbyWrtNo(strWorkTemp, long.Parse(txtWrtNo.Text));

                if (result < 0)
                {
                    MessageBox.Show("상담여부 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_MemoSave();

                if (strWorkTemp == "")
                {
                    btnNoCounsel.Text = "상담거절";
                    MessageBox.Show("상담거절 해제 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnNoCounsel.Text = "상담거절해제";
                    MessageBox.Show("상담거절 체크 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnCancerReg)
            {
                if (pnlCancer.Visible == true)
                {
                    pnlCancer.Visible = false;
                }
                else
                {
                    pnlCancer.Visible = true;
                }
            }
            else if (sender == btnHistory)
            {
                frmHcPanPersonResult f = new frmHcPanPersonResult("", FstrPANO, "");
                f.ShowDialog();
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

                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrSDate, FstrPANO, "11", FstrRowid);
                FrmHcSangInternetMunjinView.Show();
                FrmHcSangInternetMunjinView.WindowState = FormWindowState.Minimized;
            }
        }

        private void Data_Save()
        {
            string strPanRemark = "";
            string strPanRemark2 = "";
            string strAm = "";
            string strChkYN = "";

            List<string> PanResult = new List<string>();

            if (FnWRTNO == 0)
            {
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            strPanRemark = txtPanjeng.Text.TrimEnd();
            
            if (strPanRemark == "")
            {
                MessageBox.Show("판정결과가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //strPanRemark2 = "";
            //if (ComFunc.LenH(txtPanjeng.Text) > 4000)
            //{
            //    strPanRemark = ComFunc.LeftH(strPanRemark, 4000);
            //    strPanRemark2 = ComFunc.MidH(txtPanjeng.Text, 4001, txtPanjeng.Text.Length - 4000);
            //}

            strPanRemark2 = "";
            if (ComFunc.LenH(txtPanjeng.Text) > 4000)
            {
                //strPanRemark = ComFunc.LeftH(strPanRemark, 4000);
                //strPanRemark2 = ComFunc.MidH(txtPanjeng.Text, 4001, txtPanjeng.Text.Length - 4000);
                PanResult = ComFunc.GetByteString(txtPanjeng.Text.TrimEnd(), 4000);
                strPanRemark = PanResult[0];
                if (PanResult.Count > 1)
                {
                    strPanRemark2 = PanResult[1];
                }
            }


            strAm = "";
            for (int i = 1; i <= 13; i++)
            {
                CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                if (chkAm.Checked == true)
                {
                    strAm += "1,";
                }
                else
                {
                    strAm += "0,";
                }
            }

            if (chkYN.Checked == true)
            {
                strChkYN = "Y";
            }
            else
            {
                strChkYN = "";
            }

            int result = heaJepsuService.UpdatepanRecode(clsHcVariable.GstrRefValue1, strPanRemark.Replace("'", "`"), strPanRemark2.Replace("'", "`"), strAm, txtAmETC.Text.Trim(), clsType.User.IdNumber, clsType.User.UserName, clsPublic.GstrSysDate, clsPublic.GstrSysTime, long.Parse(txtWrtNo.Text), strChkYN);

            if (result < 0)
            {
                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fn_Screen_Clear();
            eBtnClick(btnSearch, new EventArgs());
        }

        void fn_MemoSave()
        {
            string strCODE = "";
            string strMemo = "";
            string strROWID = "";
            string strOK = "";
            string strTime = "";

            if (FnWRTNO > 0)
            {
                for (int i = 0; i < SS_ETC.ActiveSheet.RowCount; i++)
                {
                    strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text.Trim();
                    strTime = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                    strMemo = SS_ETC.ActiveSheet.Cells[i, 3].Text.Trim();
                    strROWID = SS_ETC.ActiveSheet.Cells[i, 5].Text.Trim();
                    //신규작성일경우
                    if (strTime == "" && strMemo != "")
                    {
                        strCODE = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

                        HEA_MEMO item = new HEA_MEMO();

                        item.WRTNO = FnWRTNO;
                        item.MEMO = strMemo;
                        item.JOBSABUN = long.Parse(clsType.User.IdNumber);

                        int result = heaMemoService.Insert(item);

                        if (result < 0)
                        {
                            MessageBox.Show("메모 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //삭제할경우
                    if (strOK == "1")
                    {
                        int result = heaMemoService.Delete(strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("메모 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
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

                if (e.Column != 0 && e.Column != 1 )
                {
                    return;
                }

                if (SS2.ActiveSheet.Cells[e.Row, 8].Text == "Y")
                {
                    strSangDam = "N";
                }
                else
                {
                    strSangDam = "Y";
                }

                if (rdoJob1.Checked == true)
                {
                    strExam = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();

                    int result = hicResultService.UpdateSangdambyWrtno(FnWRTNO, strExam, strSangDam);

                    if (SS2.ActiveSheet.Cells[e.Row, 8].Text.Trim() == "Y")
                    {
                        SS2.ActiveSheet.Cells[e.Row, 8].Text = "N";
                        SS2.ActiveSheet.Cells[e.Row, 0].BackColor = Color.White;
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";
                        SS2.ActiveSheet.Cells[e.Row, 0].BackColor = Color.Orange;
                    }
                }
                else
                {
                    clsHcVariable.GstrRefValue = "";    //약속처방
                    clsHcVariable.GstrValue = "";       //결과값
                    if (e.Column == 0 || e.Column == 1)
                    {
                        clsHcVariable.GstrRefValue = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() + "^^";
                        clsHcVariable.GstrRefValue += SS2.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                        clsHcVariable.GstrValue = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                        clsHcVariable.GstrValue += "【" + SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim() + " 】";
                        clsHcVariable.GstrValue += "결과값:" + SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                        clsHcVariable.GstrValue += "  【레벨 " + SS2.ActiveSheet.Cells[e.Row, 3].Text.Trim() + " 】";

                        fHaPanjeng_New = new frmHaPanjeng_New(clsHcVariable.GstrValue, clsHcVariable.GstrRefValue);
                        fHaPanjeng_New.rEventClosed += new frmHaPanjeng_New.EventClosed(frmHaPanjeng_New_EventClosed);
                        fHaPanjeng_New.StartPosition = FormStartPosition.CenterScreen;
                        fHaPanjeng_New.ShowDialog();
                        fHaPanjeng_New.rEventClosed -= new frmHaPanjeng_New.EventClosed(frmHaPanjeng_New_EventClosed);
                        //txtPanjeng.SelectionStart = txtPanjeng.Text.Length;
                        //txtPanjeng.ScrollToCaret();

                        //2021.11.30 KMC 주석처리
                        //txtPanjeng.Select(FnSelLength, 0);
                        //txtPanjeng.ScrollToCaret();
                        //this.ActiveControl = txtPanjeng;

                        RecordEdit();
                        //txtPanjeng.Focus();
                    }

                    clsHcVariable.GstrValue = "";
                }
            }
            else if (sender == SSList)
            {
                if (e.RowHeader || e.ColumnHeader) { return; }

                long nWrtNo = 0;
                string strWRTNO = "";

                FstrSName = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                nWrtNo = long.Parse(SSList.ActiveSheet.Cells[e.Row, 5].Text);

                if (nWrtNo == 0)
                {
                    return;
                }

                fn_Screen_Clear();

                txtWrtNo.Text = nWrtNo.ToString();

                clsHcVariable.GstrPanWRTNO = txtWrtNo.Text;

                btnPacs.Enabled = false;
                btnEMR.Enabled = false;
                fn_Screen_Display();
                fn_Hea_Memo_Screen();

                if (clsHcVariable.GnHicLicense > 0)
                {   
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    //상담대기순번에 상담중 표시
                    //호출은 했으나 상담이 완료안된 접수번호 찾음
                    List<HEA_SANGDAM_WAIT> list = heaSangdamWaitService.GetWrtNobyWrtNo(long.Parse(txtWrtNo.Text));

                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            strWRTNO += list[i].WRTNO + ",";
                        }

                        if (VB.Right(strWRTNO, 1) == ",")
                        {
                            strWRTNO = VB.Left(strWRTNO, strWRTNO.Length - 1);
                        }

                        if (strWRTNO != "")
                        {
                            int result = heaSangdamWaitService.UpdateCallTimebyWrtNo(strWRTNO);

                            if (result < 0)
                            {
                                MessageBox.Show("호출시간 update 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    int result2 = heaSangdamWaitService.UpdateCallTimebyWrtNo(txtWrtNo.Text);

                    if (result2 < 0)
                    {
                        MessageBox.Show("호출시간 update 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (rdoJob1.Checked)
                    {
                        tabControl2.SelectedIndex = 1;
                    }
                    else
                    {
                        tabControl2.SelectedIndex = 0;
                    }
                    
                }
            }     
        }

        private void frmHaPanjeng_New_EventClosed(string sRtn)
        {
            if (!sRtn.IsNullOrEmpty())
            {
                //int y = (int)SendMessage(txtPanjeng.Handle, 201, -1, 0);
                //int x = (int)SendMessage(txtPanjeng.Handle, 176, 0, 0);

                //x = x & 0xffff;

                //string oldStr = txtPanjeng.Text;
                //string newStr = oldStr.Substring(0, x);

                //txtPanjeng.Text = newStr + sRtn + oldStr.Substring(x);

                //FnSelLength = (newStr + sRtn).Length;

                txtPanjeng.Text = txtPanjeng.Text.Insert(txtPanjeng.SelectionStart, sRtn);
                txtPanjeng.Focus();
                txtPanjeng.Select(currentPosition + sRtn.Length, 0);
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
                //List<HEA_JEPSU_MEMO> list = heaJepsuMemoService.GetItembyPaNo(FnPano);
                //List<HIC_MEMO> list = hicMemoService.GetHeaItembyPaNo(FstrPANO);
                List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(FstrPANO, ""); //통합

                if (list.Count > 0)
                {
                    nRead = list.Count;

                    SS_ETC.ActiveSheet.RowCount = nRead + 5;
                    for (int i = 0; i < nRead; i++)
                    {
                        SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].JOBGBN;
                        SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].ENTTIME.ToString();
                        SS_ETC.ActiveSheet.Cells[i, 3].Text = list[i].MEMO.ToString();
                        SS_ETC.ActiveSheet.Cells[i, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, list[i].JOBSABUN.ToString());
                        SS_ETC.ActiveSheet.Cells[i, 5].Text = list[i].RID.ToString();

                        FarPoint.Win.Spread.Row row;
                        row = SS_ETC.ActiveSheet.Rows[i];
                        float rowSize = row.GetPreferredHeight();
                        row.Height = rowSize;
                    }
                }   
            }
        }

        
        void eTxtClick(object sender, EventArgs e)
        {
            FnSelLength = txtPanjeng.SelectionStart;
        }

        void eTxtDoubleClick(object sender, EventArgs e)
        {
            if (sender == txtResult1)
            {
                string strCode = "";

                strCode = txtResult1.SelectedText.Trim();
                strCode = strCode.Replace("【", "");
                strCode = strCode.Replace("】", "");

                clsHcVariable.GstrRefValue = "";
                if (strCode.Length == 4)
                {
                    clsHcVariable.GstrRefValue = strCode.Trim();
                    if (strCode.Length == 4)
                    {
                        fHaPanjeng_New = new frmHaPanjeng_New(clsHcVariable.GstrRefValue, "");
                        fHaPanjeng_New.rEventClosed += new frmHaPanjeng_New.EventClosed(frmHaPanjeng_New_EventClosed);
                        fHaPanjeng_New.StartPosition = FormStartPosition.CenterScreen;
                        fHaPanjeng_New.ShowDialog(this);
                        fHaPanjeng_New.rEventClosed -= new frmHaPanjeng_New.EventClosed(frmHaPanjeng_New_EventClosed);

                        //2021.11.30 KMC 주석처리
                        //txtPanjeng.Select(FnSelLength, 0);
                        //txtPanjeng.ScrollToCaret();
                        //txtPanjeng.Focus();

                        RecordEdit();

                    }
                }
            }
        }


    }
}
