using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayLIST01.cs
    /// Description     : 영상의학과 영상누락자 리스트
    /// Author          : 윤조연
    /// Create Date     : 2017-11-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 워크리스트 폼에 포함되어있는것을 폼 분리하여 신규폼 생성
    /// </history>
    /// <seealso cref= " >> frmComSupXrayLIST01.cs 폼이름 재정의" />
    public partial class frmComSupXrayLIST01 : Form
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

        clsComSupXraySQL.cXrayDetail cXrayDetail = null;    
                       
        string gJob = "";

        #endregion

        public frmComSupXrayLIST01(string argJob)
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


            this.btnExit.Click += new EventHandler(eBtnClick);
                        
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            //this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void setAuth(string argJob)
        {
            if (argJob == "00")
            {              
                panTitleSub0.Visible = false;
                panel6.Visible = true;
                btnExit.Visible = false;

            }
            else if (argJob == "01")
            {               
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
                cxraySpd.sSpd_XrayList01(ssList, cxraySpd.sSpdXrayList01, cxraySpd.nSpdXrayList01, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth(gJob);

                //
                screen_display();
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
                
        void screen_display()
        {
            GetData(clsDB.DbCon, ssList);
        }

        void screen_clear()
        {
            //
            read_sysdate();                       
           
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
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name == "dtpDate")
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }
                }
            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            #region // class 초기화 , 변수 설정
            cXrayDetail = new clsComSupXraySQL.cXrayDetail();
            cXrayDetail.Job = "05";

            #endregion

            dt = cxraySql.sel_XrayDetail(pDbCon, cXrayDetail);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.GbIO].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.XJong].Text = dt.Rows[i]["XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.WardCode].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.XName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.JepDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayList01.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            
        }

    }
}
