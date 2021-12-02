using ComBase;
using ComDbB;
using ComSupLibB.Com;
using ComSupLibB.SupFnEx;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayLIST04.cs
    /// Description     : 영상의학과 근전도 검사 명단
    /// Author          : 윤조연
    /// Create Date     : 2018-02-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\Frm근전도검사리스트.frm(Frm근전도검사리스트) >> frmComSupXrayLIST04.cs 폼이름 재정의" />
    public partial class frmComSupXrayLIST04 : Form,MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupFnEx cFnEx = new clsComSupFnEx();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSupXrayRead.cXray_ResultNew cXray_ResultNew = null;

        string gJob = "";

        System.Windows.Forms.Timer timer1 = null;
        int gnTimer = 0;

        Thread thread;
        FpSpread spd;

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

        public frmComSupXrayLIST04()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupXrayLIST04(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;           
            setEvent();       
        }

        //기본값 세팅
        void setCtrlData()
        {

            screen_clear();

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);            
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            //this.chkAuto.Click += new EventHandler(eChkClick);

        }

        void setAuth(string argJob)
        {
            if (argJob == "00")
            {
                if (timer1 == null)
                {
                    timer1 = new System.Windows.Forms.Timer();
                }
                timer1.Enabled = true;
                timer1.Interval = 5000;
                timer1.Tick += new EventHandler(eTimer);

                panTitleSub0.Visible = false;
                panel6.Visible = true;
                btnExit.Visible = false;


            }
            else if (argJob == "01")
            {
                if (timer1 != null)
                {
                    timer1.Enabled = false;
                    timer1.Tick -= new EventHandler(eTimer);
                }
                panTitleSub0.Visible = true;
                panel6.Visible = false;
                btnExit.Visible = true;
            }
            else
            {

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
                //            
                cxraySpd.sSpd_XrayList04(ssList, cxraySpd.sSpdXrayList04, cxraySpd.nSpdXrayList04, 5, 0);                

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                                
                screen_clear();

                setCtrlData();

                setAuth(gJob);

                //
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
            timer_close();

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
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                //
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            DataTable dt = null;
            FpSpread o = (FpSpread)sender;

            txtResult.Text = "";

            if (e.Row < 0 || e.Column < 0) return;

            if (e.ColumnHeader == true)
            {
                return;
            }

            //마우스 우클릭 메뉴팝업
            if (e.Button == MouseButtons.Right)
            {
                #region //우클릭시 팝업메뉴 생성
                                   
                
                #endregion
            }
            else
            {
                if (sender == this.ssList)
                {

                    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                    {
                        o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                    }

                    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;

                    if (e.Column == (int)clsComSupXraySpd.enmXrayList04.Remark2 && o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.Remark].Text.Trim() != "")
                    {
                        txtResult.Text = "<의사참고사항>" +  "\r\n"  + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.Remark].Text.Trim();

                    }
                    else if (e.Column == (int)clsComSupXraySpd.enmXrayList04.STS00 && o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.STS00].Text.Trim() == "◎")
                    {
                        long nWRTNO = Convert.ToInt32(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.WRTNO].Text.Trim());
                        dt = cRead.sel_XRAY_RESULTNEW(clsDB.DbCon, "03", "", "", nWRTNO);
                        if (ComFunc.isDataTableNull(dt) == false)
                        {
                            if (dt.Rows[0]["Approve"].ToString().Trim() == "N")
                            {
                                txtResult.Text = "\r\n" + "\r\n" + "\r\n" + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                            }
                            else
                            {
                                txtResult.Text = dt.Rows[0]["Result"].ToString().Trim() + dt.Rows[0]["Result1"].ToString().Trim();
                            }
                            
                        }

                    }
                    else if (e.Column == (int)clsComSupXraySpd.enmXrayList04.STS01 && o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.STS01].Text.Trim()!="")
                    {
                        long nEmgWRTNO = Convert.ToInt32(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList04.EmgWRTNO].Text.Trim());

                        dt = sup.sel_Etc_Result(clsDB.DbCon, nEmgWRTNO);
                        if (ComFunc.isDataTableNull(dt) == false)
                        {
                            cFnEx.FnEx_display_EMG_file(dt, "1");
                        }

                    }
                }

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
                return;
            }

            if (sender == this.ssList)
            {
                //gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.ROWID].Text.Trim();
            }

        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtPano)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {

            //    }
            //}

        }

        void eChkClick(object sender, EventArgs e)
        {
            CheckBox o = (CheckBox)sender;

            //if (sender == this.chkAuto)
            //{
            //    if (o.Checked == true)
            //    {
            //        timer1.Enabled = true;
            //    }
            //    else
            //    {
            //        timer1.Enabled = false;
            //    }
            //}
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            string strIO = "";

            if (optIO0.Checked == true)
            {
                strIO = "(전체)";
            }
            else if (optIO1.Checked == true)
            {
                strIO = "(입원)";
            }
            else if (optIO2.Checked == true)
            {
                strIO = "(외래)";
            }
            else if (optIO3.Checked == true)
            {
                strIO = "(응급)";
            }

            strTitle = "근전도검사 리스트 " + strIO +  " (" + dtpFDate.Text.Trim()  + "~" + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void eTimer(object sender, EventArgs e)
        {
            gnTimer++;
            if (gnTimer == 1)
            {
                gnTimer = 0;
                screen_display();
            }
        }

        public void timer_close()
        {
            if (timer1 != null && timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
        }

        void screen_display()
        {
            read_sysdate();
            GetData_th(clsDB.DbCon);
        }

        void screen_clear()
        {
            //
            read_sysdate();

            ////콘트롤 값 clear
            //Control[] controls = ComFunc.GetAllControls(this);

            //foreach (Control ctl in controls)
            //{

            //    if (ctl is TextBox)
            //    {
            //        ctl.Text = "";
            //    }
            //    else if (ctl is CheckBox)
            //    {
            //        ((CheckBox)ctl).Checked = false;
            //    }
            //    else if (ctl is RadioButton)
            //    {
            //        ((RadioButton)ctl).Checked = false;
            //    }
            //    else if (ctl is DateTimePicker)
            //    {
            //        if (((DateTimePicker)ctl).Name == "dtpDate")
            //        {
            //            ((DateTimePicker)ctl).Text = "";
            //        }

            //    }
            //}

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData_th(PsmhDb pDbCon)
        {
            Cursor.Current = Cursors.WaitCursor;

            cXray_ResultNew = new clsComSupXrayRead.cXray_ResultNew();
            cXray_ResultNew.Job = "00";
            cXray_ResultNew.SDate = dtpFDate.Text.Trim();
            cXray_ResultNew.TDate = dtpTDate.Text.Trim();
            cXray_ResultNew.Gubun1 = "";
            if (optIO1.Checked == true)
            {
                cXray_ResultNew.Gubun1 = "IPD";
            }
            else if (optIO2.Checked == true)
            {
                cXray_ResultNew.Gubun1 = "OPD";
            }
            else if (optIO3.Checked == true)
            {
                cXray_ResultNew.Gubun1 = "ER";
            }


            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {
            spd = ssList;
            DataTable dt = null;
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            dt = cRead.sel_XRAY_RESULTNEW_detail(clsDB.DbCon, cXray_ResultNew);
            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            int i = 0;
            string strXName = string.Empty;

            spd.ActiveSheet.RowCount = 0;

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                spd.ActiveSheet.RowCount = dt.Rows.Count+1;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 5);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strXName = dt.Rows[i]["OrderName"].ToString().Trim();
                    if (strXName == "")
                    {
                        strXName = dt.Rows[i]["XName"].ToString().Trim();
                    }
                    strXName +=  " " + dt.Rows[i]["Remark"].ToString().Trim();

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Ward].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Room].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.DrCode].Text = dt.Rows[i]["FC_DrName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.XName].Text = strXName;
                    if (dt.Rows[i]["WRTNO"].ToString().Trim().CompareTo("1000") > 0)
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.STS00].Text ="◎";
                    }
                    if (dt.Rows[i]["EMGWRTNO"].ToString().Trim().CompareTo("0") > 0)
                    {                        
                        if (dt.Rows[i]["FC_EMGCNT"].ToString().Trim().CompareTo("0")> 0)
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.STS01].Text = "▦" + dt.Rows[i]["FC_EMGCNT"].ToString().Trim() +"장";
                        }
                    }
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.SeekDate].Text = dt.Rows[i]["Seekdate"].ToString().Trim();

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.WRTNO].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.EmgWRTNO].Text = dt.Rows[i]["EMGWRTNO"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Remark].Text = dt.Rows[i]["DrRemark"].ToString().Trim();
                    if (dt.Rows[i]["DrRemark"].ToString().Trim() !="")
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Remark2].Text = "※";
                    }
                    

                    if (dt.Rows[i]["BDate"].ToString().Trim() != dt.Rows[i]["SeekDate"].ToString().Trim())
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.RDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    }                    

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Time1].Text = (Convert.ToDateTime(dt.Rows[i]["SeekDate"].ToString().Trim()) - Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim())).TotalDays.ToString();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.Time2].Text = (Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString().Trim()) - Convert.ToDateTime(dt.Rows[i]["SeekDate"].ToString().Trim())).TotalDays.ToString();


                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList04.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    spd.ActiveSheet.Rows.Get(i).Font = new System.Drawing.Font("맑은 고딕", 9.75F);
                }

                spd.ActiveSheet.Cells[dt.Rows.Count, (int)clsComSupXraySpd.enmXrayList04.XName].Text = "# 이상 총 " +  dt.Rows.Count + "명 #";

                // 화면상의 정렬표시 Clear
                spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
            
            dt.Dispose();
            dt = null;

        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }


        #endregion


    }
}
