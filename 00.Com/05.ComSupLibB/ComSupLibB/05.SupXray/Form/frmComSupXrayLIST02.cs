using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayLIST02.cs
    /// Description     : 영상의학과 촬영대기순번 리스트
    /// Author          : 윤조연
    /// Create Date     : 2017-11-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 워크리스트 폼에 포함되어있는것을 폼 분리하여 신규폼 생성
    /// </history>
    /// <seealso cref= " >> frmComSupXrayLIST02.cs 폼이름 재정의" />
    public partial class frmComSupXrayLIST02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cxreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();
        
        clsComSupXraySQL.cXray_Wait cXray_Wait = null;
        
        string gJob = "";

        System.Windows.Forms.Timer timer1 = null;
        int gnTimer = 0;

        Thread thread;
        FpSpread spd;
               

        #endregion

        public frmComSupXrayLIST02(string argJob)
        {
            InitializeComponent();
            gJob = argJob;
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
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);


            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            //this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);

            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.chkAuto.Click += new EventHandler(eChkClick);
                        
        }

        void setAuth(string argJob)
        {
            if (argJob =="00")
            {
                if (timer1 == null)
                {
                    timer1 = new System.Windows.Forms.Timer();
                }                
                timer1.Enabled = true;
                timer1.Interval = 5000;
                timer1.Tick += new EventHandler(eTimer);

                chkAuto.Visible = true;
                chkAuto.Checked = true;

                panTitleSub0.Visible = false;
                panel6.Visible = true;
                btnExit.Visible = false;


            }
            else if(argJob == "01")
            {
                if (timer1 !=null)
                {
                    timer1.Enabled = false;
                    timer1.Tick -= new EventHandler(eTimer);
                }

                chkAuto.Visible = false;
                chkAuto.Checked = false;

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
                cxraySpd.sSpd_XrayList02(ssList1, cxraySpd.sSpdXrayList02, cxraySpd.nSpdXrayList02, 10, 0);
                cxraySpd.sSpd_XrayList02(ssList2, cxraySpd.sSpdXrayList02, cxraySpd.nSpdXrayList02, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth(gJob);

                //
                screen_display();
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            timer_close();
        }

        void eFormClosing(object sender, FormClosingEventArgs e)
        {
            
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
            //if (sender == this.btnPrint)
            //{
            //    ePrint();
            //}
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

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

            if (sender == this.ssList1 || sender == this.ssList2)
            {
                timer1.Enabled = false;

                string s = string.Empty;
                s += o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList02.Pano].Text.Trim()  ;
                s += "("+ o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList02.SName].Text.Trim() + ")";

                if (ComFunc.MsgBoxQ( s + " 선택하신 대기자를 대기자에서 제외하시겠습니까??", "제외선택", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    if (chkAuto.Checked == true && timer1.Enabled == false)
                    {
                        timer1.Enabled = true;
                    }
                    return;
                }
                
                try
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    string strROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayList02.ROWID].Text.Trim();

                    SqlErr = cxraySql.up_XRAY_WAIT(clsDB.DbCon, strROWID, " GbEnd ='*' ", "", ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                    
                        if (chkAuto.Checked == true && timer1.Enabled == false)
                        {
                            timer1.Enabled = true;
                        }
                        return;
                    }
                    else
                    {
                        clsDB.setCommitTran(clsDB.DbCon);

                        if (chkAuto.Checked == true && timer1.Enabled == false)
                        {
                            timer1.Enabled = true;
                        }
                    }
                }  
                catch(Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                    
                    if (chkAuto.Checked == true && timer1.Enabled == false)
                    {
                        timer1.Enabled = true;
                    }
                    return;
                }
   
                
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

            if (sender == this.chkAuto)
            {
                if (o.Checked == true)
                {
                    timer1.Enabled = true;
                }
                else
                {
                    timer1.Enabled = false;
                }
            }
        }

        void eTimer(object sender,EventArgs e)
        {
            gnTimer++;
            if (gnTimer ==1)
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

            cXray_Wait = new clsComSupXraySQL.cXray_Wait();
            cXray_Wait.Job = "TOTAL";            
            cXray_Wait.PART = "1";   //본관만            
            cXray_Wait.JDATE = cpublic.strSysDate;
            if (optGbn1.Checked ==true)
            {
                cXray_Wait.GBEND = "";
            }
            else if (optGbn2.Checked == true)
            {
                cXray_Wait.GBEND = "0";
            }
            else if (optGbn3.Checked == true)
            {
                cXray_Wait.GBEND = "*";
            }
                       
            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;

        }               

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {
            spd = ssList1;
                        
            DataTable dt = null;
            cXray_Wait.GBROOM = "1"; //1.DR촬영만 /2.일반             
            dt = cxraySql.sel_XRAY_WAIT(clsDB.DbCon, cXray_Wait,false);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);
            
            spd = ssList2;

            dt = null;
            cXray_Wait.GBROOM = "2"; //1.DR촬영만 /2.일반 
            dt = cxraySql.sel_XRAY_WAIT(clsDB.DbCon, cXray_Wait,false);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);
                        
        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            int i = 0;
           
            spd.ActiveSheet.RowCount = 0;

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();                    
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.Remark].Text = dt.Rows[i]["Gubun2"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.JepTime].Text = VB.Mid(dt.Rows[i]["SeqTime"].ToString().Trim(),1,2)+":" + VB.Mid(dt.Rows[i]["SeqTime"].ToString().Trim(), 3, 2) + ":" + VB.Mid(dt.Rows[i]["SeqTime"].ToString().Trim(), 5, 2);
                    
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.EndTime].Text = dt.Rows[i]["JepTime"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList02.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                // 화면상의 정렬표시 Clear
                spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;

        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            //this.Progress.Visible = b;
            //this.Progress.IsRunning = b;
        }


        #endregion

    }
}
