using System;
using System.Data;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using DevComponents.DotNetBar;
using FarPoint.Win.Spread;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupDrst
{
    /// <summary>
    /// Class Name      : ComSupLibB.Drst
    /// File Name       : frmComSupDrstSEND01.cs
    /// Description     : 조제 DUR 전송 메인
    /// Author          : 김경동
    /// Create Date     : 2018-11-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\drug\dratc\FrmDUR_ATC.frm(FrmDUR_ATC) >> frmComSupDrstSEND01.cs 폼이름 재정의" />
    public partial class frmComSupDrstSEND01 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic =null; //공용함수
        conPatInfo pinfo = null; //공통정보        
        ComFunc fun = null;
        clsComSup sup = null;
        clsComSup.SupPInfo cinfo = null;
        clsMethod method = null;
        clsSpread methodSpd = null;

        clsComSupDrstSpd cSpd = null;
        clsComSupDrstSQL cSQL = null;
        clsComSupDrstSQL.cDurSend cDurSend = null;

        FarPoint.Win.Spread.FpSpread spd;
        Thread thread;

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
        #endregion //MainFormMessage

        public frmComSupDrstSEND01()
        {
            InitializeComponent();

            setEvent();
            setAuth();
        }

        public frmComSupDrstSEND01(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            
            setEvent();
            setAuth();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
                      
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            
            //
            setCombo("");
                        

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "영상판독_통합환자정보표시");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //파트설정로드
            //    if (dt.Rows[0]["VALUEV"].ToString() == "N")
            //    {
            //        exSpliter1.Expanded = true;
            //        pan_pinfo.Size = new System.Drawing.Size(1180, 25);
            //    }
            //}
            //setPart();            
        }

        //권한체크
        void setAuth()
        {

            
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancle.Click += new EventHandler(eBtnClick);
            //this.btnSort.Click += new EventHandler(eBtnPopUp);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSend.Click += new EventHandler(eBtnSave);

            this.exSpliter1.Click += new EventHandler(eSpliterClick);

            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClick);
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);            
            this.ssList.AutoFilteredColumn += new AutoFilteredColumnEventHandler(eSpreadAutoFilerColumn);

            this.optJob_ALL.Click += new EventHandler(eOptEvent);
            this.optJob_Tewon.Click += new EventHandler(eOptEvent);
            this.optJob_OPD.Click += new EventHandler(eOptEvent);
            this.optJob_ER.Click += new EventHandler(eOptEvent);
            this.optSEND_ALL.Click += new EventHandler(eOptEvent);
            this.optSEND_Y.Click += new EventHandler(eOptEvent);
            this.optSEND_N.Click += new EventHandler(eOptEvent);
            
            //this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.dtpFDate.ValueChanged += new EventHandler(eDtpValueChanged);

        }

        void eSpliterClick(object sender, EventArgs e)
        {
            ExpandableSplitter ex = (ExpandableSplitter)sender;

            if (ex.Expanded == true)
            {
                panPtInfo.Size = new System.Drawing.Size(1180, 15);
            }
            else
            {
                panPtInfo.Size = new System.Drawing.Size(1180, 90);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {

                cpublic = new clsPublic(); //공용함수
                pinfo = new conPatInfo(); //공통정보        
                fun = new ComFunc();
                sup = new clsComSup();
                cinfo = new clsComSup.SupPInfo();
                method = new clsMethod();
                methodSpd = new clsSpread();

                cSpd = new clsComSupDrstSpd();
                cSQL = new clsComSupDrstSQL();

                cDurSend = new clsComSupDrstSQL.cDurSend();

                //            
                cSpd.sSpd_DurSend01(ssList, cSpd.sSpdDurSend01, cSpd.nSpdDurSend01, 5, 0);
                cSpd.sSpd_DurSend02(ssDetail, cSpd.sSpdDurSend02, cSpd.nSpdDurSend02, 5, 0);
                
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData(clsDB.DbCon);

                setCtrlInit();

                screen_display();

            }

        }

        void eFormResize(object sender, EventArgs e)
        {

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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnCancle)
            {
                screen_clear();
                return;
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
                screen_clear("A1");
            }
            
        }

        void eBtnSave(object sender, EventArgs e)
        {
            //if (sender == this.btnSave1)
            //{
            //    eSave(clsDB.DbCon, "취소");
            //}
            //else if (sender == this.btnSave2)
            //{
            //    eSave(clsDB.DbCon, "UR취소");
            //}
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            //if (sender == this.btnPrint)
            //{
            //    ePrint();
            //}
        }

        void eBtnPopUp(object sender, EventArgs e)
        {
            //if (sender == this.btnSort)
            //{
            //    frmComSupXraySET18 f = new frmComSupXraySET18();
            //    f.ShowDialog();
            //    sup.setClearMemory(f);
            //    XSort = cxray.read_xrayList_Sort(clsDB.DbCon, clsType.User.IdNumber);
            //    btnSort.Text = setXSort(XSort.ToString());
            //}
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

            if (e.ColumnHeader == true)
            {
                if (sender == this.ssList)
                {
                    if (e.Column == (int)clsComSupDrstSpd.enmDurSend01.chk01)
                    {
                        e.Cancel = true;
                        if (o.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Value.Equals(false))
                        {
                            o.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Value = true;
                        }
                        else
                        {
                            o.ActiveSheet.ColumnHeader.Cells[0, e.Column].Value = false;
                        }

                        for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                        {
                            o.ActiveSheet.Cells[i, e.Column].Value = o.ActiveSheet.ColumnHeader.Cells[e.Row, (int)clsComSupDrstSpd.enmDurSend01.chk01].Value;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (sender == this.ssList)
            {
                string strGbn = o.ActiveSheet.Cells[e.Row, (int)clsComSupDrstSpd.enmDurSend01.Gubun].Text.Trim();
                string strPano = o.ActiveSheet.Cells[e.Row, (int)clsComSupDrstSpd.enmDurSend01.Pano].Text.Trim();
                string strBDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupDrstSpd.enmDurSend01.BDate].Text.Trim();
                string strTuyak = o.ActiveSheet.Cells[e.Row, (int)clsComSupDrstSpd.enmDurSend01.TuyakNo].Text.Trim();

                screen_display2(clsDB.DbCon,ssDetail, strGbn, strPano,strBDate,strTuyak);

            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(o, e.Column);
                return;
            }
            else if (sender == this.ssList)
            {
               

            }
        }
               
        void eSpreadSelChanged(object sender, SelectionChangedEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (sender == this.ssList)
            {
                //기본정보 설정
                read_cinfo(o, e.Range.Row);

                //환자공통 정보
                if (cinfo.strPano != "")
                {
                    conPatInfo1.SetDisPlay(cinfo.strEmrNo, cinfo.strIO, cinfo.strBDate, cinfo.strPano, cinfo.strDept);
                    
                }

                

                o.Select();
                o.Focus();

            }
        }

        void eSpreadBtnClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //if (sender == this.ssList)
            //{
            //    if (e.Column == (int)clsSupXraySpd.enmSupXrayLIST01.chk)
            //    {
            //        if (o.ActiveSheet.Cells[e.Row, (int)clsSupXraySpd.enmSupXrayLIST01.chk].Text == "True")
            //        {
            //            o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
            //        }
            //        else
            //        {
            //            o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
            //        }
            //    }
            //}

        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (o.ActiveSheet.RowCount <= 0)
            {
                return;
            }

            if (e.RowHeader == true || e.Column < 1)
            {
                return;
            }
            else
            {
                e.TipText = o.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                e.ShowTip = true;
            }

        }

        void eSpreadAutoFilerColumn(object sender, AutoFilteredColumnEventArgs e)
        {
            if (sender == this.ssList)
            {
                //lblCnt.Text = "";
                //lblCnt.Text = methodSpd.SpdFilter_DataRowCount(ssList).ToString() + "건";
            }
        }

        void eOptEvent(object sender, EventArgs e)
        {
            RadioButton o = (RadioButton)sender;
   
            if (VB.Left(o.Name, 7) == "optSEND" || VB.Left(o.Name, 6) == "optJob")
            {
                ssList.ActiveSheet.RowCount = 0;
                screen_display();
                screen_clear();
            }

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtSearch)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        lblSName.Text = "";
            //        string s = string.Empty;
            //        s = txtSearch.Text.Trim();
            //        if (s != "" && (cXray_Read.Gubun1 == "1" || cXray_Read.Gubun1 == "2"))
            //        {
            //            s = ComFunc.SetAutoZero(s, ComNum.LENPTNO);
            //            txtSearch.Text = s;
            //            lblSName.Text = fun.Read_Patient(clsDB.DbCon, s, "2");
            //            screen_display();
            //        }
            //    }
            //}
        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text.Trim();
        }              
        
        void read_cinfo(FpSpread Spd, int row)
        {
            //환자공통정보 표시                   
            cinfo = new clsComSup.SupPInfo();

            cinfo.selRow = row;
            cinfo.strIO = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.STS].Text.Trim() == "입원" ? "I" : "O";
            cinfo.strBDate = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.BDate].Text.Trim();            
            cinfo.strPano = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.Pano].Text.Trim();
            cinfo.strSName = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.SName].Text.Trim();
            cinfo.strDept = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.DeptCode].Text.Trim();
            cinfo.strDrCode = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.DrCode].Text.Trim();
            cinfo.strDrName = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.DrName].Text.Trim();            
            cinfo.strROWID = Spd.ActiveSheet.Cells[row, (int)clsComSupDrstSpd.enmDurSend01.ROWID].Text.Trim();

        }            
                
        void setCombo(string argJob)
        {
            //과
            method.setCombo_View(this.cboDept, sup.sel_BAS_CLINICDEPT(clsDB.DbCon, "", " 'II','RD','MD' "), clsParam.enmComParamComboType.ALL);

            //작업자       
            method.setCombo_View(this.cboWork, sup.sel_BasBCode(clsDB.DbCon, " 'DRUG_DUR_JOJAE_SABUN' ", "", " Code || '.' || Name Codes ", "", " SORT,Code "), clsParam.enmComParamComboType.NULL);
           
        }

        void setTxtTip()
        {
            //툴팁
            //ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            //ssList.TextTipDelay = 500;

        }                
        
        string setXSort(string argSort)
        {
            if (argSort == "PANO")
            {
                return "등록번호순";
            }
            else if (argSort == "SNAME")
            {
                return "성명순";
            }
            else if (argSort == "DEPT")
            {
                return "과별순";
            }
            else if (argSort == "XNAME")
            {
                return "촬영명칭순";
            }
            else if (argSort == "XCODE")
            {
                return "검사코드순";
            }
            else
            {
                return "정렬 설정없음";
            }


        }                

        void screen_clear(string argJob = "")
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            if (argJob == "")
            {
                
            }
            else if (argJob == "A1")
            {
                
            }

            //btnSelect.Enabled = false;

            //환자공통정보 표시                   
            cinfo = new clsComSup.SupPInfo();
            conPatInfo1.SetItemClear();

            

        }

        void screen_display()
        {            
            GetData_th(ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());            
        }

        void screen_display2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argGbn, string argPano, string argBDate, string argTuyak)
        {
            GetData2(pDbCon,  Spd,  argGbn, argPano,argBDate, argTuyak);
        }

        void GetData_th(FpSpread Spd, string argSDate, string argTDate)
        {
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            cDurSend.STS = "전체";
            if (optSEND_Y.Checked == true)
            {
                cDurSend.STS = "전송";
            }
            else if (optSEND_N.Checked == true)
            {
                cDurSend.STS = "미전송";
            }            

            cDurSend.Gubun = "전체";
            if (optJob_OPD.Checked == true)
            {
                cDurSend.Gubun = "외래약";
            }
            else if (optJob_Tewon.Checked == true)
            {
                cDurSend.Gubun = "퇴원약";
            }
            else if (optJob_ER.Checked == true)
            {
                cDurSend.Gubun = "응급실";
            }
            
            cDurSend.Dept = "**";
            cDurSend.Dept =  VB.Left(cboDept.Text.Trim(),2);
            cDurSend.fDate = dtpFDate.Text.Trim();
            cDurSend.tDate = dtpTDate.Text.Trim();

            spd = Spd;

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;
            dt = cSQL.sel_DurSend(clsDB.DbCon, cDurSend);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            string strOK = "";
            string strTemp = "";
            int nRow = -1;
            //DataTable dt2 = null;   

            spd.ActiveSheet.RowCount = 0;


            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 5);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    if (dt.Rows[i]["FC_DUR_MASER_CHK"].ToString().Trim() == "Y")
                    {
                        nRow++;

                        if (spd.ActiveSheet.RowCount <= nRow) spd.ActiveSheet.RowCount = nRow + 1;

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.chk01].Text = "";

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.AtcDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.AtcTime].Text = dt.Rows[i]["EntTime"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.BDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.Gubun].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.TuyakNo].Text = dt.Rows[i]["TuyakNo"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.Bi].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupDrstSpd.enmDurSend01.Send].Text = dt.Rows[i]["FC_DUR_MASER_CHK2"].ToString().Trim();

                        spd.ActiveSheet.Rows.Get(nRow).Font = new System.Drawing.Font("맑은 고딕", 9.75F);

                        //spd.ActiveSheet.Rows.Get(nRow).Height = spd.ActiveSheet.Rows[nRow].GetPreferredHeight();
                    }
                    

                }
                // 화면상의 정렬표시 Clear
                spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;

            spd.ActiveSheet.RowCount = nRow + 1;
            
        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }


        #endregion

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argGbn, string argPano, string argBDate, string argTuyak)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;                      

            dt = cSQL.sel_DurSend_Detail(pDbCon, argGbn,argPano,argBDate,argTuyak);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupDrstSpd.enmDurSend02.DosCode].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();

                                       
                }

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion

        }

    }
}
