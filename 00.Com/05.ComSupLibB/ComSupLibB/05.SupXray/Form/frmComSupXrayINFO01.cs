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
    /// File Name       : frmComSupXrayINFO01.cs
    /// Description     : 영상의학과 판독프로그램에 개인환자정보
    /// Author          : 윤조연
    /// Create Date     : 2018-02-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\xuread13.frm(FrmPanoInfo) >> frmComSupXrayINFO01.cs 폼이름 재정의" />
    public partial class frmComSupXrayINFO01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cxreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSup.cBasPatient cBasPatient = null;



        string gIO = "";
        string gPano = "";
        string gDept = "";

        #endregion

        public frmComSupXrayINFO01(string argIO,string argPano,string argDept)
        {
            InitializeComponent();
            gIO = argIO;
            gPano = argPano;
            gDept = argDept;
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

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);            
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
                //cxraySpd.sSpd_enmXraySET10(ssList, cxraySpd.sSpdenmXraySET10, cxraySpd.nSpdenmXraySET10, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                screen_clear();

                setCtrlData();

                setAuth();

                //
                GetData(clsDB.DbCon, ssList);
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;
                       

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
            GetData(clsDB.DbCon, ssList);
        }

        void GetData(PsmhDb pDbCon, FpSpread Spd)
        {
            int i = 0;
            string strTemp = "";
            DataTable dt = null;
            
            Cursor.Current = Cursors.WaitCursor;

            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, gPano);

            if (cBasPatient !=null)
            {
                Spd.ActiveSheet.Cells[0, 1].Text = cBasPatient.SName;
                Spd.ActiveSheet.Cells[1, 1].Text = cBasPatient.Jumin1 + "-" + cBasPatient.Jumin3;
                Spd.ActiveSheet.Cells[8, 1].Text = cBasPatient.Tel;
                Spd.ActiveSheet.Cells[9, 1].Text = fun.READ_BAS_Mail(clsDB.DbCon, cBasPatient.ZipCode1 + cBasPatient.ZipCode2) + cBasPatient.Juso;
                Spd.ActiveSheet.Cells[11, 1].Text = fun.Read_Bi_Name(clsDB.DbCon, cBasPatient.Bi, "2");                          

            }
            if (gIO == "외래")
            {
                dt = cxraySql.sel_OPD_ILLS(pDbCon, "00", gPano, gDept);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    strTemp = dt.Rows[0]["YYMM"].ToString().Trim();

                    dt = cxraySql.sel_OPD_ILLS(pDbCon, "00", gPano, strTemp, gDept);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i < 4)
                            {
                                Spd.ActiveSheet.Cells[i + 2, 1].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                            }

                        }
                    }
                }
                
            }
            else
            {
                dt = cxraySql.sel_IPD_NEW_MASTER(pDbCon, "00", gPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    strTemp = dt.Rows[0]["InDate"].ToString().Trim();

                    dt = cxraySql.sel_OCS_IILLS(pDbCon, "00", gPano, strTemp);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i < 4)
                            {
                                Spd.ActiveSheet.Cells[i + 2, 1].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                            }
                        }
                    }
                }

                dt = cxraySql.sel_NUR_JINDAN(pDbCon, "00", gPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    Spd.ActiveSheet.Cells[10, 1].Text = dt.Rows[0]["Diagnosys"].ToString().Trim();
                }


            }

            Cursor.Current = Cursors.Default;

        }


    }
}
