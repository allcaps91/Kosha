using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET12.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 유방촬영 기본판독문
    /// Author          : 윤조연
    /// Create Date     : 2018-02-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\Frm유방촬영.frm(Frm유방촬영) >> frmComSupXraySET12.cs 폼이름 재정의" />
    public partial class frmComSupXraySET12 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();
        
        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        //string gJob = "";
        string gSabun = "";

        #endregion

        public frmComSupXraySET12(string argSabun)
        {
            InitializeComponent();
            gSabun = argSabun;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
            
            screen_clear();
        }

        //권한체크
        void setAuth()
        {
            
        }
                
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);            
            this.btnOK.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);


            this.ssList1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList2.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssOrdList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);            
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);                        
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtMCode.KeyDown += new KeyEventHandler(eTxtEvent);
            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                cxraySpd.sSpd_enmXraySET12A(ssList1, cxraySpd.sSpdenmXraySET12A, cxraySpd.nSpdenmXraySET12A, 3, 0);
                cxraySpd.sSpd_enmXraySET12B(ssList2, cxraySpd.sSpdenmXraySET12B, cxraySpd.nSpdenmXraySET12B, 3, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth();

                //
                screen_display();
                screen_display2();

            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList1 ) 
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.chk].Text = "";
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }
                o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET12A.chk].Text = "1";
                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;
            }
            else if ( sender == this.ssList2)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.chk].Text = "";
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }
                o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET12B.chk].Text = "1";
                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;
            }

        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtMCode)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        //GetData2(txtMCode.Text.Trim().ToUpper());
            //    }
            //}

        }
                
        void eBtnClick(object sender, EventArgs e)
        {            
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnOK)
            {
                setDelegate("01");
            }
            
        }

        void eBtnSearch(object sender, EventArgs e)
        {

        }

        void eBtnSave(object sender, EventArgs e)
        {
            //if (sender == this.btnSave1)
            //{
            //    //
            //    eSave(clsDB.DbCon, "저장");
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

        }

        void eSave(PsmhDb pDbCon, string Job)
        {
        }

        void setDelegate(string argJob)
        {
            if (rSendMsg == null)
            {
                return;
            }

            string s = string.Empty;
            string s2 = string.Empty;

            #region //상용구세팅
            if (optGubunA1.Checked == true)
            {
                s += "1.Indication: screening" + "\r\n" + "\r\n";
            }
            else if (optGubunA2.Checked == true)
            {
                s += "1.Indication: diagnostic" + "\r\n" + "\r\n";
            }

            s += "2.Parenchymal composition: " + "\r\n" ;

            if (optGubunB1.Checked == true)
            {
                s += optGubunB1.Text + "\r\n" + "\r\n";
            }
            else if (optGubunB2.Checked == true)
            {
                s += optGubunB2.Text + "\r\n" + "\r\n";
            }
            else if (optGubunB3.Checked == true)
            {
                s += optGubunB3.Text  + "\r\n" + "\r\n";
            }
            else if (optGubunB4.Checked == true)
            {
                s += optGubunB4.Text + "\r\n" + "\r\n";
            }

            if (optGubunC1.Checked == true)
            {
                s += "3.Finding:" + "\r\n" + "\r\n";
            }
            else if (optGubunC2.Checked == true)
            {
                s += "3.Finding and Comparison with old films:" + "\r\n" + "\r\n";
            }

            s2 = "";
            if (ssList1.ActiveSheet.RowCount > 0)
            {
                 s2 = sheet_sel_chk(ssList1, "12A");
            }
            if (s2 !="")
            {
                s += s2;
            }

            s +=  "\r\n" + "\r\n";

            s += "4.Conclusion:" + "\r\n" + "\r\n";

            s2 = "";
            if (ssList2.ActiveSheet.RowCount > 0)
            {
                s2 = sheet_sel_chk(ssList2, "12B");
            }                

            if (s2 != "")
            {
                s += s2;
            }
            else
            {
                s += "BI-RADS Category ";
            }
            s += "\r\n" ;

            #endregion

            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;            
            cXray_Read_Delegate.Sogen = s;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
        }

        string sheet_sel_chk(FpSpread Spd,string argJob)
        {
            string s = string.Empty;

            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if (argJob=="12A")
                {
                    if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.chk].Text == "True")
                    {
                        s = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.Result1].Text;
                        if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.Result2].Text.Trim() !="")
                        {
                            s += Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.Result2].Text;
                        }
                    }
                }
                else if (argJob == "12B")
                {
                    if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.chk].Text == "True")
                    {
                        s = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.Result1].Text;
                        if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.Result2].Text.Trim() != "")
                        {
                            s += Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.Result2].Text;
                        }
                    }
                }
                
            }

            return s;
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");


            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    //((RadioButton)ctl).Checked = false;
                }


            }


        }

        void screen_display()
        {
            GetData(clsDB.DbCon,ssList1);
        }

        void screen_display2()
        {
            GetData2(clsDB.DbCon, ssList2);
        }

        void GetData(PsmhDb pDbCon, FpSpread Spd)
        {
            int i = 0;
            
            DataTable dt = null;
            
            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
                        
            dt = cRead.sel_XRAY_RESULTSET(pDbCon,"01",Convert.ToInt32(gSabun) );

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count +1;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                //Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12A.chk].Text = "1";
                for (i = 0; i < dt.Rows.Count; i++)
                {                   
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12A.Code].Text = dt.Rows[i]["SetName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12A.Result1].Text = dt.Rows[i]["Result1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12A.Result2].Text = dt.Rows[i]["Result2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;

        }

        void GetData2(PsmhDb pDbCon, FpSpread Spd)
        {
            int i = 0;

            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            dt = cRead.sel_XRAY_RESULTSET(pDbCon, "02", Convert.ToInt32(gSabun));

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count+1;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                //Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET12B.chk].Text = "1";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12B.Code].Text = dt.Rows[i]["SetName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12B.Result1].Text = dt.Rows[i]["Result1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12B.Result2].Text = dt.Rows[i]["Result2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i + 1, (int)clsComSupXraySpd.enmXraySET12B.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;

        }

    }
}
