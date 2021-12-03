using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComPmpaLibB.form.PmpaView;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : PmpaMagam
    /// File Name       : frmIpdMirCheckupdate2.cs
    /// Description     : 퇴원,중간청구액 차액 점검 및 변경(개인별)[응급실6시간,낮병동 명칭 >>응급실 입원으로 변경]
    /// Author          : 김해수
    /// Create Date     : 2018-09-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs39.frm(FrmIpdMirCheckUpdate2) >> frmPmpaMagamIpdMirCheckUpdate2.cs 폼이름 재정의" />
    public partial class frmPmpaMagamIpdMirCheckUpdate2 : Form 
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsComPmpaSpd MagamSpd = new clsComPmpaSpd();
        clsPmpaMisu cPM = new clsPmpaMisu();
        clsPmpaFunc cPF = new clsPmpaFunc();
        frmPmpaViewSlip2MirCheck frmPmpaViewSlip2MirCheckS = null;

        string RemarkBackUp = "";
        int RowBackUp = 0;
        #endregion

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion
        public frmPmpaMagamIpdMirCheckUpdate2()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMagamIpdMirCheckUpdate2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            ////탭버튼클릭 이벤트
            
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnReView.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);
           

            ////명단 더블클릭 이벤트
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpdChange);
            //this.ssList.EditModeOff += new EventHandler(eSpdChange);
            //this.ssList.Change += new ChangeEventHandler(eSpreadChange);
            //this.ssList.Change += new ChangeEventHandler(eSpdChange);
            //this.ssList.EditChange += new EditorNotifyEventHandler(eSpdChange);
            this.ssList.LeaveCell += new LeaveCellEventHandler(eSpdChange);
            this.ssList.KeyDown += new KeyEventHandler(eTxtKeyDown);

        }

        void setCombo()
        {

            CF.ComboMonth_Set(cboYYMM, 24);
            CF.ComboMonth_Set(cboYYMM2, 24);

            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.SelectedIndex = 1;

            cboJob.Items.Add("*.전체");
            cboJob.Items.Add("0.누락");
            cboJob.Items.Add("1.차이금액 5% 이상");
            cboJob.Items.Add("2.차이금액 10% 이상");
            cboJob.Items.Add("3.+5만원 이상");
            cboJob.Items.Add("4.-5만원 이하");
            cboJob.Items.Add("5.+2만원 이상");
            cboJob.Items.Add("6.-2만원 이하");
            cboJob.Items.Add("7.+10만원 이상");
            cboJob.Items.Add("8.-10만원 이하");
            cboJob.Items.Add("9.+1만원 이상");
            cboJob.Items.Add("A.-1만원 이하");
            cboJob.Items.Add("B.(+ -) 1만원");
            cboJob.Items.Add("C.(+ -) 2만원");
            cboJob.Items.Add("D.(+ -) 5만원");
            cboJob.Items.Add("E.(+ -) 10만원");
            cboJob.SelectedIndex = 0;

        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //설정세팅
            //}
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {

                MagamSpd.sSpd_enmIpdMirCheckUpdate(ssList, MagamSpd.senmIpdMirCheckUpdate, MagamSpd.nenmIpdMirCheckUpdate, 1, 0);

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }
        }

        void eFormResize(object sender, EventArgs e)
        {
            //setCtrlProgress();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eSpdChange(object sender, EventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            int nRowIndex = 0;
            int nCollndex = 0;

            nRowIndex = ssList.ActiveSheet.ActiveRowIndex;
            nCollndex = ssList.ActiveSheet.ActiveColumnIndex;

            if (sender == this.ssList)
            {
                eSave(clsDB.DbCon, nRowIndex, nCollndex);
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;            

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
                //if ((e.Column).ToString() == "6")
                //{
                //    LblMsg.Text = "주별구분 (1-5: 1주차~5주차 주별청구,6.외래및퇴원청구, 7.중간청구)";
                //}
                //else if ((e.Column).ToString() == "7")
                //{
                //    LblMsg.Text = "청구구분 (0.일반청구 1.보완(재)청구 2.추가청구 4.NP정액)";
                //}
                //else
                //{
                //    LblMsg.Text = "";
                //}

               // string strJob = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsSupEndsSpd.enmSupEndsSCH01A.STS02].Text.Trim();
            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            string strPano = "", strSname = "", strBi = "", strInDate = "";
            string strActdate = "", strBDate = "", strYYMM = "", nWRTNO = "";
            string strIO = "I", strSu = "0",GstrRetValue ="";


            if (e.Row < 0 || e.Column < 0) return;
            if (ssList.ActiveSheet.RowCount == e.Row)
            {
                return;
            }

            if(e.Column == 2)
            {
                return;
            }

            strPano = ssList.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Pano].Text;
            strSname = ssList.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.SName].Text;
            strBi = ssList.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Bi].Text;
            strBDate = ssList.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.BDate].Text;
            strInDate = ssList.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.InDate].Text;
            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2);
            
            if(e.Column == 3)
            {
                if(opt4.Checked == true)
                {
                    strIO = "O";
                }
                frmPmpaViewPanoMir f = new frmPmpaViewPanoMir(strPano + strBi + strIO);
                f.ShowDialog();
                cPF.fn_ClearMemory(f);
            }else if(e.Column == 0 || e.Column == 1)
            {
                if(opt3.Checked == true)
                {
                    strSu = "1";
                }
                frmPmpaViewPanoMir2 f = new frmPmpaViewPanoMir2(strPano + strBi + strSu);
                f.ShowDialog();
                cPF.fn_ClearMemory(f);
            }
            else if(e.Column == 6){
                GstrRetValue = VB.Format(VB.Val(nWRTNO), "#########0")+",";
                GstrRetValue = GstrRetValue + strActdate + ",";
                GstrRetValue = GstrRetValue + strPano + ",";
                GstrRetValue = GstrRetValue + strSname + ",";
                GstrRetValue = GstrRetValue + strBi + ",";
                GstrRetValue = GstrRetValue + strInDate + ",";
                GstrRetValue = GstrRetValue + strYYMM + ",";
                if(opt4.Checked == true)
                {
                    switch (VB.Left(cboJong.Text, 1))
                    {
                        case "4":
                        case "3":
                            GstrRetValue = GstrRetValue + "O,1"; //자보
                            break;
                        default:
                            GstrRetValue = GstrRetValue + "O,0";
                            break;
                    }
                }
                else if(opt1.Checked == true)
                {
                    GstrRetValue = GstrRetValue + "I,1"; //퇴원
                }
                else if(opt2.Checked == true)
                {
                    GstrRetValue = GstrRetValue + "I,2"; //중간
                }else if(opt3.Checked == true)
                {
                    GstrRetValue = GstrRetValue + "I,3"; //응급실6시간
                }
                GstrRetValue = GstrRetValue + "," + strBDate;

                foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                {
                    if (frm2.Name == "frmPmpaViewSlip2MirCheck")
                    {
                        frm2.Visible = true;
                        frm2.Activate();
                        frmPmpaViewSlip2MirCheckS.reScreen_Display2(GstrRetValue);
                        return;
                    }
                }

                frmPmpaViewSlip2MirCheckS = new frmPmpaViewSlip2MirCheck(GstrRetValue);
                frmPmpaViewSlip2MirCheckS.StartPosition = FormStartPosition.Manual;
                frmPmpaViewSlip2MirCheckS.Location = new Point(0, 50);
                frmPmpaViewSlip2MirCheckS.Show();

            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.ssList)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    FpSpread o = (FpSpread)sender;

                    int nRowIndex = 0;

                    nRowIndex = ssList.ActiveSheet.ActiveRowIndex;

                    ssList.ActiveSheet.Cells[nRowIndex, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Remark].Text = "";   
                }
            }
        }

        void eOptClick(object sender, KeyEventArgs e)
        {
            if(this.opt1.Checked == true)
            {
                gbYYMM.Width = 258;
            }else if (this.opt2.Checked == true)
            {
                gbYYMM.Width = 125;
            }else if (this.opt3.Checked == true)
            {
                gbYYMM.Width = 125;
            }else if (this.opt4.Checked == true)
            {
                gbYYMM.Width = 258;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                //this.Close();
                this.Visible = false;
                return;
            }
            else if (sender == this.btnReView)
            {
                screen_clear("Clear");

            }
            else if (sender == this.btnView)
            {
                screen_display();
            }
            else if(sender == this.btnHelp)
            {
                if(panelHelp.Visible == false)
                {
                    panelHelp.Visible = true;
                }else
                {
                    panelHelp.Visible = false;
                }
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {
                cboYYMM.Focus();
                panelHelp.Visible = false;
            }
            else if (Job == "Clear")
            {
                cboYYMM.Focus();
                btnView.Enabled = true;
                //gbIO.Enabled = true;
                gbYYMM.Enabled = true;
                gbJong.Enabled = true;
                ssList.ActiveSheet.RowCount = 0;
                ssList.ActiveSheet.RowCount = 1;
                //// 화면상의 정렬표시 Clear ( Sort 정렬 아이콘 클리어)
                ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            Cursor.Current = Cursors.WaitCursor;
            GetData(clsDB.DbCon, ssList);
            Cursor.Current = Cursors.Default;
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            #region 변수 선언 부분 및 부분선언
            DataTable dt = null;

            int i = 0;
            //int j = 0;
            int nRow = 0;
            string strYYMM = "", strFDate = "", strTDate = "", strJong = "";
            //string strOldData = "", strNewData = "";
            string strBi = "";
            //string strDeptCode = "";
            string strOK = "", GuBun = "", strYYMM2 ="";
            string ADDTDate = "";
            double nToiAmt = 0, nMirAmt = 0, nChaAmt = 0, nChaRate = 0;
            double nTotToiAmt = 0, nTotMirAmt = 0, nTotChaAmt = 0;

            btnView.Enabled = false;
            gbYYMM.Enabled = false;
            gbJong.Enabled = false;

            //Sheet Clear
            Spd.ActiveSheet.RowCount = 1;

            //누적할 변수를 Clear 
            nTotToiAmt = 0;
            nTotMirAmt = 0;
            nTotChaAmt = 0;

            if (opt2.Checked == true)
            {
                strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2);
                strYYMM = cPF.DATE_YYMM_ADD(strYYMM, 1);
            }
            else
            {
                strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2);
                strYYMM2 = ComFunc.LeftH(cboYYMM2.Text, 4) + ComFunc.MidH(cboYYMM2.Text, 8, 2);

                if (strYYMM.CompareTo(strYYMM2) > 0)
                {
                    ComFunc.MsgBox("작업년월이 잘못 되었습니다.");
                    return;
                }
            }

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strJong = ComFunc.LeftH(cboJong.Text, 1);

            if (opt1.Checked == true)
            {
                GuBun = "1";
            } else if (opt2.Checked == true)
            {
                GuBun = "2";
            } else if (opt3.Checked == true)
            {
                GuBun = "3";
            } else if (opt4.Checked == true)
            {
                GuBun = "4";
            }

            ADDTDate = CF.DATE_ADD(clsDB.DbCon, strTDate, 1);
            #endregion


            #region 퇴원자 명단을 조회 

            dt = cSQL.sel_IpdMirCheckUpdate2_Check1(pDbCon, strYYMM, strYYMM2, strJong, GuBun, strFDate, ADDTDate);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBi = dt.Rows[i]["Bi"].ToString().Trim() + "";
                nToiAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["JOHAP"].ToString().Trim() + ""));
                nMirAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim() + ""));
                nChaAmt = nMirAmt - nToiAmt;
                nChaRate = 0;
                if(nChaAmt != 0 && nMirAmt != 0)
                {
                    nChaRate = nChaAmt / nMirAmt * 100;
                }

                strOK = "NO";

                switch (VB.Left(cboJob.Text, 1).Trim())
                {
                    case "*" :
                        strOK = "OK";
                        break;
                    case "1" :
                        if(nToiAmt != 0 && nMirAmt == 0)strOK = "OK"; 
                        break;
                    case "2" :
                        if (nToiAmt != 0 && nMirAmt == 0) strOK = "OK"; //
                        if (nChaRate > 5 || nChaRate < -5) strOK = "OK";
                        break;
                    case "3" :
                        if (nChaAmt >= 50000) strOK = "OK";
                        break;
                    case "4" :
                        if (nChaAmt <= -50000) strOK = "OK";
                        break;
                    case "5" :
                        if (nChaAmt >= 20000) strOK = "OK";
                        break;
                    case "6" :
                        if (nChaAmt <= -20000) strOK = "OK";
                        break;
                    case "7" :
                        if (nChaAmt >= 100000) strOK = "OK";
                        break;
                    case "8" :
                        if (nChaAmt <= -100000) strOK = "OK";
                        break;
                    case "9":
                        if (nChaAmt >= 10000) strOK = "OK";
                        break;
                    case "A":
                        if (nChaAmt <= -10000) strOK = "OK";
                        break;
                    case "B":
                        if (nChaAmt <= -10000 || nChaAmt >= 10000) strOK = "OK";
                        break;
                    case "C":
                        if (nChaAmt <= -20000 || nChaAmt >= 20000) strOK = "OK";
                        break;
                    case "D":
                        if (nChaAmt <= -50000 || nChaAmt >= 50000) strOK = "OK";
                        break;
                    case "E":
                        if (nChaAmt <= -100000 || nChaAmt >= 100000) strOK = "OK";
                        break;
                    default:
                        if (nToiAmt != 0 && nMirAmt == 0) strOK = "OK";
                        break;
                }

                if(strOK == "OK")
                {
                    nTotToiAmt = nTotToiAmt + nToiAmt;
                    nTotMirAmt = nTotMirAmt + nMirAmt;
                    nTotChaAmt = nTotChaAmt + nChaAmt;

                    nRow += 1;
                    if (nRow > Spd.ActiveSheet.RowCount)
                    {
                        Spd.ActiveSheet.RowCount = nRow;
                    }

                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.SName].Text = dt.Rows[i]["SName"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Bi].Text = dt.Rows[i]["Bi"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.nToiAmt].Text = VB.Format(nToiAmt, "###,###,###,##0");
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.MirAmt].Text = VB.Format(nMirAmt, "###,###,###,##0");
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.ChaAmt].Text = VB.Format(nChaAmt, "###,###,###,##0");
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Remark].Text = dt.Rows[i]["remark"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.OldMirAmt].Text = VB.Format(nMirAmt, "###,###,###,##0");
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.OldReMark].Text = dt.Rows[i]["remark"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.YYMM].Text = dt.Rows[i]["YYMM"].ToString().Trim() + "";
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.InDate].Text = dt.Rows[i]["INDATE"].ToString().Trim() + "";
                }
            }

            #endregion

            Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;//Sort 소트 정렬 아이콘 클리어
            dt.Dispose();
            dt = null;

            nRow += 1;
            Spd.ActiveSheet.RowCount = nRow;
            Spd.ActiveSheet.Cells[nRow -1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.SName].Text = "** 합계 **";
            Spd.ActiveSheet.Cells[nRow -1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.nToiAmt].Text = VB.Format(nTotToiAmt, "###,###,###,##0");
            Spd.ActiveSheet.Cells[nRow -1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.MirAmt].Text = VB.Format(nTotMirAmt, "###,###,###,##0");
            Spd.ActiveSheet.Cells[nRow -1, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.ChaAmt].Text = VB.Format(nTotChaAmt, "###,###,###,##0");

            //Set The height of a selected row
            switch (VB.Left(cboJob.Text, 1))
            {
                case "*":
                case "1":
                case "2":
                    break;
                default:
                    Spd.ActiveSheet.Rows[0,nRow - 1].Height = (float)30.5;
                    break;
            }

            btnPrint.Enabled = true;
            btnReView.Enabled = true;

        }

        void eSave(PsmhDb pDbCon, int argRow, int argCol)
        {
            int intRowAffected = 0;

            string strROWID = "", strRemark = "", strOldRemark = "", strGubun = "0";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if(argCol == 7)
            {
                strRemark = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Remark].Text;
                strROWID = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.ROWID].Text;
                strOldRemark = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.OldReMark].Text;

                if(strRemark != strOldRemark)
                {
                    clsDB.setBeginTran(pDbCon);

                    if(opt2.Checked == true)
                    {
                        strGubun = "2";
                    }

                    try
                    {
                        SqlErr = cSQL.udt_IpdMirCheckUpdate2_update1(pDbCon, strRemark, strROWID, clsType.User.IdNumber, strGubun, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("MISU_BALCHECK_PANO에 자료를 등록중 오류발생", "RollBack");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        if (SqlErr == "")
                        {
                            clsDB.setCommitTran(pDbCon);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                    }
                }
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            // strTitle = "내시경 예약자 명단 " + "(" + dtpFDate.Text.Trim() + ")";
            if(opt1.Checked == true)
            {
                strTitle = "퇴원자 차액점검(개인별) LIST";
            }
            else if(opt2.Checked == true)
            {
                strTitle = "중간청구 차액점검(개인별) LIST";
            }
            else if(opt3.Checked == true)
            {
                strTitle = "응급실 입원 차액점검(개인별) LIST";
            }
            else if(opt4.Checked == true)
            {
                strTitle = "외래 차액점검(개인별) LIST";
            }
            

            //strSubTitle += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            
            strSubTitle += SPR.setSpdPrint_String("작업월:" + cboYYMM.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            strSubTitle += SPR.setSpdPrint_String("       인쇄 일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            //strFooter = "";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 0, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }

        private void opt1_CheckedChanged(object sender, EventArgs e)
        {
            gbYYMM.Width = 260;
            lbl.Visible = true;
        }

        private void opt2_CheckedChanged(object sender, EventArgs e)
        {
            gbYYMM.Width = 127;
            lbl.Visible = false;
        }

        private void opt3_CheckedChanged(object sender, EventArgs e)
        {
            gbYYMM.Width = 260;
            lbl.Visible = true;
        }

        private void opt4_CheckedChanged(object sender, EventArgs e)
        {
            gbYYMM.Width = 260;
            lbl.Visible = true;
        }
    }
}
