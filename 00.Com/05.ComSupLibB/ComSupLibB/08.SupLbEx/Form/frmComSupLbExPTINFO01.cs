using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP01.cs 
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록 
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exinfect\EXMAIN23.frm" />
    public partial class frmComSupLbExPTINFO01 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        DateTime sysdate;

        string gStrACTDATE = "";
        bool gIsClosse;

        string gStrPANO     = "";

        public delegate void PSMH_RETURN_VALUE(object sender, DataSet ds);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupLbExPTINFO01(bool isClose, string strPANO, string strACTDATE)
        {
            InitializeComponent();
            this.gIsClosse = isClose;

            this.gStrPANO       = strPANO;
            this.gStrACTDATE    = strACTDATE;

            setEvent();
        }

        void setCtrlUcSupComPt()
        {
            Point p = new Point();

            p.X = this.Location.X + this.panel5.Location.X;
            p.Y = this.Location.Y + this.panTitle.Height + panTitleSub0.Height + panel1.Height + 20;

            this.ucPTINFO.pPSMH_LPoint = p;

        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlSpread();
        }

        void setEvent()
        {
            this.Load                                   += new EventHandler(eFormLoad);

            this.Move                                   += new EventHandler(eFormMove);
            this.Resize                                 += new EventHandler(eFormResize);

            this.btnExit.Click                          += new EventHandler(eBtnClick);

            this.ssMain.CellDoubleClick                 += new CellClickEventHandler(eSpreadDClick);
            this.btnSearch.Click                        += new EventHandler(eBtnSearch);

            this.rdo_I.Click                            += new EventHandler(eRdoClick);
            this.rdo_O.Click                            += new EventHandler(eRdoClick);
            this.rdo_MASTER.Click                       += new EventHandler(eRdoClick);

            this.ucPTINFO.ePSMH_UcSupComPtSearch_VALUE  += new UcSupComPtSearch.PSMH_RETURN_VALUE(eUC);

        }

        void eFormMove(object sender, EventArgs e)
        {
            setCtrlUcSupComPt();
        }

        void eFormResize(object sender, EventArgs e)
        {
            setCtrlUcSupComPt();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == this.rdo_MASTER)
            {
                setSpdStyle(this.ssMain, null, comSql.sSel_OPD_MASTER_PTINFO, comSql.nSel_OPD_MASTER_PTINFO);

                this.dtpACDATE_FR.Enabled = true;
                this.dtpACDATE_TO.Enabled = false;
            }
            else
            {
                if (this.rdo_I.Checked == true)
                {
                    this.dtpACDATE_FR.Enabled = true;
                    this.dtpACDATE_TO.Enabled = false;
                }
                else if (this.rdo_O.Checked == true)
                {
                    this.dtpACDATE_FR.Enabled = true;
                    this.dtpACDATE_TO.Enabled = true;
                }                
            }

            setCtrlSpread();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, (int)e.Column);
                return;
            }

            DataSet ds = new DataSet();
            DataSet ds_ss = (DataSet)this.ssMain.DataSource;

            ds = ds_ss.Clone();

            DataRow dr = ds_ss.Tables[0].Rows[e.Row];

            ds.Tables[0].ImportRow(dr);

            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_VALUE);
     
            this.ePsmhReturnValue(this,ds);

            if (this.gIsClosse == true)
            {
                this.Close();
            }
        }

        private void ePSMH_VALUE(object sender, DataSet ds)
        {
            
        }

        void eUC(object sender, string pano, string sname)
        {
            DataSet ds = null;

            ds = comSql.sel_OPD_MASTER_MENUAL(clsDB.DbCon, clsComSQL.enmPT_GBIO.MASTER, this.dtpACDATE_FR.Value.ToString("yyyy-MM-dd"), this.dtpACDATE_TO.Value.ToString("yyyy-MM-dd"), this.ucPTINFO.txtSearch_PtInfo.Text);
            setSpdStyle(this.ssMain, ds, comSql.sSel_OPD_MASTER_PTINFO, comSql.nSel_OPD_MASTER_PTINFO);

            this.ucPTINFO.txtSearch_PtInfo.Text = "";
            this.ucPTINFO.txtSearch_SName.Text = "";
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            if (string.IsNullOrEmpty(this.gStrACTDATE.Trim()) == false)
            {
                this.dtpACDATE_FR.Value = Convert.ToDateTime(this.gStrACTDATE).AddDays(-3);
                this.dtpACDATE_TO.Value = Convert.ToDateTime(this.gStrACTDATE);                
            }
            else
            {
                this.dtpACDATE_FR.Value = sysdate.AddDays(-3);
                this.dtpACDATE_TO.Value = sysdate;
            }
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            this.ucPTINFO.ePSMH_UcSupComPtSearch_VALUE += new UcSupComPtSearch.PSMH_RETURN_VALUE(ePTINFO);
            this.ucPTINFO.txtSearch_PtInfo.Text = this.gStrPANO;
            this.ucPTINFO.setSname();

            if (string.IsNullOrEmpty(this.ucPTINFO.txtSearch_SName.Text.Trim()) == true)
            {
                this.gStrPANO = "";
            }         

            DataSet ds = null;

            clsComSQL.enmPT_GBIO eGBIO = clsComSQL.enmPT_GBIO.I;

            if (this.rdo_O.Checked == true || this.rdo_I.Checked == true )
            {
                if (this.rdo_O.Checked == true)
                {
                    eGBIO = clsComSQL.enmPT_GBIO.O;
                }
               
                ds = comSql.sel_OPD_MASTER_MENUAL(clsDB.DbCon, eGBIO, this.dtpACDATE_FR.Value.ToString("yyyy-MM-dd"), this.dtpACDATE_TO.Value.ToString("yyyy-MM-dd"), this.gStrPANO);

                setSpdStyle(this.ssMain, ds, comSql.sSel_OPD_MASTER_PTINFO, comSql.nSel_OPD_MASTER_PTINFO);
            }
            else if (this.rdo_MASTER.Checked == true)
            {
                setSpdStyle(this.ssMain, null, comSql.sSel_OPD_MASTER_PTINFO, comSql.nSel_OPD_MASTER_PTINFO);

                if (string.IsNullOrEmpty(this.gStrPANO) == false)
                {
                    eUC(this, this.gStrPANO, this.ucPTINFO.txtSearch_SName.Text.Trim());
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void ePTINFO(object sender, string pano, string sname)
        {
            this.ucPTINFO.txtSearch_PtInfo.Text = this.gStrPANO;
            this.ucPTINFO.txtSearch_SName.Text = sname;

        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.Rows.Count = 0;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);


            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 30;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            spd.ActiveSheet.Columns[-1].HorizontalAlignment = CellHorizontalAlignment.Center;
            spd.ActiveSheet.Columns[-1].VerticalAlignment = CellVerticalAlignment.Center;

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            // 5. sort, filter
            spread.setSpdFilter(spd, (int)clsComSQL.enmSel_OPD_MASTER_PTINFO.PANO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSQL.enmSel_OPD_MASTER_PTINFO.SNAME, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSQL.enmSel_OPD_MASTER_PTINFO.WARDCODE, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSQL.enmSel_OPD_MASTER_PTINFO.DEPTCODE, AutoFilterMode.EnhancedContextMenu, true);
        }

    }
}
