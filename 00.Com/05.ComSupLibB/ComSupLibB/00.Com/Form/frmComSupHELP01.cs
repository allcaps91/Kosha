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
    public partial class frmComSupHELP01 : Form
    {
        //clsLbExSQL lbExSQL = new clsLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        bool gIsClose;

        public enum enmSel_BAS_SUN_TYPE { PT,EXAM };

        enmSel_BAS_SUN_TYPE gEnmType;

        Thread thread;

        public delegate void PSMH_RETURN_VALUE(object sender, string code, string name, string Yname,string BUN,string BUN_NAME);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupHELP01(enmSel_BAS_SUN_TYPE enmType,bool isClose )
        {
            InitializeComponent();

            this.gEnmType = enmType;
            this.gIsClose = isClose;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
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

        void setCtrl()
        {
            setCtrlTitle();
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
            int nRow;
            
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, (int)e.Column);
                return;
            }

            nRow = e.Row;

            string strSUNEXT;
            string strSUNAMEE;
            string strSUNAMEK;
            string strBUN;
            string strBUN_NAME;

            strSUNEXT = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSQL.enmSel_BAS_SUN.SUNEXT].Text;
            strSUNAMEE = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSQL.enmSel_BAS_SUN.SUNAMEE].Text;
            strSUNAMEK = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSQL.enmSel_BAS_SUN.SUNAMEK].Text;
            strBUN = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSQL.enmSel_BAS_SUN.BUN].Text;
            strBUN_NAME = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSQL.enmSel_BAS_SUN.BUN_NAME].Text;

            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);

            this.ePsmhReturnValue(this, strSUNEXT, strSUNAMEE, strSUNAMEK, strBUN, strBUN_NAME);

            if (this.gIsClose == true)
            {
                this.Close();
            }
        }

        private void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname, string BUN, string BUN_NAME)
        {
        }

        void setCtrlTitle()
        {
            string strTitle = "";

            if (this.gEnmType == enmSel_BAS_SUN_TYPE.PT)
            {
                strTitle = "물리치료수가 [세부적인 수가 문의는 심사팀에 문의하세요]";
            }
            else if (this.gEnmType == enmSel_BAS_SUN_TYPE.EXAM)
            {
                strTitle = "진단검사의학과 수가 매칭";
            }

            this.lblTitle.Text = strTitle + " HELP";
            this.lblTitleSub0.Text = strTitle + " 리스트";
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            thread = new Thread(() => threadSetCtrlSpread());
            thread.Start();

        }

        void threadSetCtrlSpread()
        {
            DataSet ds = null;            

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            ds = comSql.sel_BAS_SUN(clsDB.DbCon, this.gEnmType);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, comSql.sSel_BAS_SUN, comSql.nSel_BAS_SUN });
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

        }

        delegate void delegateSetCtrlCircular(bool b);
        void setCtrlCircular(bool b)
        {

            if (b == true)
            {
                this.ssMain.Enabled = false;
            }
            else
            {
                this.ssMain.Enabled = true;
            }

            this.circProgress.Visible = b;
            this.circProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size);
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
            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);
            // 5. sort, filter
            spread.setSpdFilter(spd, -1, AutoFilterMode.EnhancedContextMenu, true);

            spd.ActiveSheet.AddRows(0, 1);
        }

    }
}
