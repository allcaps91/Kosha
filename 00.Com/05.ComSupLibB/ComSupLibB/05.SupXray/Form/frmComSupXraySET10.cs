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
    /// File Name       : frmComSupXraySET10.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 개인별 판독 내역
    /// Author          : 윤조연
    /// Create Date     : 2018-02-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\xuread06.frm(FrmPanoHistory) >> frmComSupXraySET10.cs 폼이름 재정의" />
    public partial class frmComSupXraySET10 : Form
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
                
        clsComSupXrayRead.cXray_ResultNew cXray_ResultNew = null;

        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        string gJob = "";
        string gPano = "";        
                
        #endregion

        public frmComSupXraySET10(string argJob,string argPano)
        {
            InitializeComponent();
            gJob = argJob;
            gPano = argPano;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;


            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");


            screen_clear();
                        
        }

        //권한체크
        void setAuth()
        {
            lblDrName.Text = fun.Read_Patient(clsDB.DbCon, gPano, "2") + "( " +gPano + " )";            
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSet2.Click += new EventHandler(eBtnClick);
            this.btnSet3.Click += new EventHandler(eBtnClick);
            this.btnSet1.Click += new EventHandler(eBtnClick);
            //this.btnCancel.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);


            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssOrdList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);            
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);                        
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtMCode.KeyDown += new KeyEventHandler(eTxtEvent);
            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);


            this.exSpliter1.Click += ExpandableSplitter1_Click;

           
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
                cxraySpd.sSpd_enmXraySET10(ssList, cxraySpd.sSpdenmXraySET10, cxraySpd.nSpdenmXraySET10, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth();

                //
                GetData(clsDB.DbCon, ssList);
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;
            if (e.Row < 0 || e.ColumnHeader == true) return;
            if (e.Button != MouseButtons.Left) return;

            if (sender == this.ssList)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }

                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;

                txtXCode.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET10.XCode].Text.Trim();
                txtXName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET10.XName].Text.Trim();
                txtResult.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET10.Result].Text.Trim() + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET10.Result1].Text.Trim();

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
            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }

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

        void ExpandableSplitter1_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            //if (ex.Expanded == true)
            //{
            //    panel3.Size = new System.Drawing.Size(882, 20);
            //}
            //else
            //{
            //    panel3.Size = new System.Drawing.Size(882, 280);
            //}
        }              

        void eBtnClick(object sender, EventArgs e)
        {
            string sPos = "";
            if (optPos1.Checked == true)
            {
                sPos = "01";
            }
            else if (optPos2.Checked == true)
            {
                sPos = "02";
            }
            else if (optPos3.Checked == true)
            {
                sPos = "03";
            }


            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSet1)
            {
                setDelegate("01", sPos, txtResult.Text);
            }
            else if (sender == this.btnSet2)
            {
                setDelegate("02", sPos, txtResult.Text);
            }
            else if (sender == this.btnSet3)
            {
                if (txtResult.SelectedText.Trim() != "")
                {
                    setDelegate("03", sPos, txtResult.SelectedText);
                }
                else
                {
                    ComFunc.MsgBox("필요한 문구를 선택후 작업하세요!!");
                    txtResult.Focus();
                }
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

        void setDelegate(string argJob, string argPos, string argRemark)
        {
            if (rSendMsg == null)
            {
                return;
            }

            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;
            cXray_Read_Delegate.sPos = argPos; //01.현재커서 02.처음 03.마지막               
            cXray_Read_Delegate.Sogen = argRemark;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
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


            //lblJName.Text = "";
            
            lblDrName.Text = "";

        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList);
        }

        void GetData(PsmhDb pDbCon, FpSpread Spd)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            #region //변수세팅
            cXray_ResultNew = new clsComSupXrayRead.cXray_ResultNew();
            cXray_ResultNew.Job = "03";
            cXray_ResultNew.Pano = gPano;
            #endregion

            dt = cRead.sel_XRAY_RESULTNEW(pDbCon, cXray_ResultNew,false);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.ReadDate].Text = dt.Rows[i]["ReadDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.XJong].Text = dt.Rows[i]["FC_XJong2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.XName].Text = dt.Rows[i]["XName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.Result].Text = dt.Rows[i]["Result"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.Result1].Text = dt.Rows[i]["Result1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET10.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

            }

            #endregion

            Cursor.Current = Cursors.Default;

        }


    }
}
