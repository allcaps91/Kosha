using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP01.cs
    /// Description     : 검사코드HELP
    /// Author          : 김홍록
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\Ocs\ptocs\iupent12.frm" />
    public partial class frmComSupPMPA01 : Form
    {
        //clsLbExSQL lbExSQL = new clsLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        bool gIsClose;

        public enum enmSel_BAS_SUN_TYPE { PT,EXAM };

        string gStrPANO;
        DataSet gDs;

        Thread thread;

        public delegate void PSMH_RETURN_VALUE(object sender, string PANO, string INOUTCLS, string MEDFRDATE, string MEDFRTIME, string MEDENDDATE, string MEDENDTIME, string MEDDEPTCD, string MEDDRCD, string PTNAME, string AGE, string SEX);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupPMPA01(string strPANO)
        {
            InitializeComponent();
            this.gStrPANO = strPANO;
            setEvent();
        }

        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.btnOK.Click += new EventHandler(eBtnOK);
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

        void eBtnOK(object sender, EventArgs e)
        {
            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(eReturnValue);

            this.ePsmhReturnValue(this
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNO].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.INOUTCLS].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDFRDATE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDTIME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDDATE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDTIME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDDEPTCD].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDDRCD].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNAME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.AGE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[this.ssMain.ActiveSheet.ActiveRow.Index, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.SEX].Text.Trim()
                                      );
            if (this.gIsClose == true)
            {
                this.Close();
            }

        }

        void setCtrl()
        { 
            setCtrlSpread();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, (int)e.Column);
                return;
            }

            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(eReturnValue);

            this.ePsmhReturnValue(this
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNO].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.INOUTCLS].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDFRDATE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDTIME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDDATE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDTIME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDDEPTCD].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDDRCD].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNAME].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.AGE].Text.Trim()
                                      , this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.SEX].Text.Trim()
                                      );
            if (this.gIsClose == true)
            {
                this.Close();
            }
        }

        void eReturnValue(object sender, string PANO, string INOUTCLS, string MEDFRDATE, string MEDFRTIME, string MEDENDDATE, string MEDENDTIME, string MEDDEPTCD, string MEDDRCD, string PTNAME, string AGE, string SEX)
        {
        
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataSet ds = comSql.sel_IPD_NEW_MASTER_EMR(clsDB.DbCon, this.gStrPANO);

            if (ComFunc.isDataSetNull(ds) == true)
            {
                this.Close();
            }
            else
            {
                this.txt_PANO.Text  = ds.Tables[0].Rows[0][(int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNO].ToString();
                this.txt_SNAME.Text = ds.Tables[0].Rows[0][(int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNAME].ToString();
                this.txt_SEX.Text   = ds.Tables[0].Rows[0][(int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.SEX].ToString();

                setSpdStyle(this.ssMain, ds, comSql.sSel_IPD_NEW_MASTER_EMR, comSql.nSel_IPD_NEW_MASTER_EMR);
            }
        }
        
        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {

            spd.ActiveSheet.Rows.Count = 0;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }
            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            // 3. 컬럼 스타일 설정.
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDFRTIME  , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDENDTIME , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNO       , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.PTNAME     , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.SEX        , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.AGE        , clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSQL.enmSel_IPD_NEW_MASTER_EMR.MEDDEPTCD  , clsSpread.enmSpdType.Hide);

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            // 5. sort, filter
            spread.setSpdFilter(spd, -1, AutoFilterMode.EnhancedContextMenu, true);
        }

    }
}
