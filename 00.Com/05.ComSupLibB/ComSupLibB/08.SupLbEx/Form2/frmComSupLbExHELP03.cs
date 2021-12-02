using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

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
    /// <seealso cref= "d:\psmh\exam\exinfect\ExInfect90.frm" />
    public partial class frmComSupLbExHELP03 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        public enum enmMasterType { EXAM, PTHL };

        bool gIsAutoClose;
        string gStrMasterCode;
        enmMasterType gEnmType = enmMasterType.EXAM;

        Thread thread;

        public delegate void PSMH_RETURN_VALUE(object sender, string code, string name, string Yname);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupLbExHELP03(enmMasterType enmType, string strMasterCode, bool isAutoClose = false)
        {
            InitializeComponent();

            this.gIsAutoClose = isAutoClose;
            this.gStrMasterCode = strMasterCode;
            this.gEnmType = enmType;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

        }

        void setCtrl()
        {
            setCtrlTitle();
            setCtrlSpread();
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
            string strCode;
            string strName;
            string strYName;

            strCode = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSupLbExSQL.enmSel_EXAM_MASTER_SEARCH.MASTERCODE].Text;
            strName = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSupLbExSQL.enmSel_EXAM_MASTER_SEARCH.EXAMNAME].Text;
            strYName = this.ssMain.ActiveSheet.Cells[nRow, (int)clsComSupLbExSQL.enmSel_EXAM_MASTER_SEARCH.WSCODE1].Text;

            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);

            this.ePsmhReturnValue(this, strCode, strName, strYName);

            if (this.gIsAutoClose == true)
            {
                this.Close();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string header = string.Empty;
            string foot = string.Empty;

            string headerSubTitle = string.Empty;
            

            if (this.gEnmType == enmMasterType.EXAM)
            {
                headerSubTitle = "진단검사의학과 검사코드";
            }
            else if (this.gEnmType == enmMasterType.PTHL)
            {
                headerSubTitle = "해부병리 검사코드";
            }

            string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            header = spread.setSpdPrint_String(headerSubTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            header += spread.setSpdPrint_String("출력시간:" + s + " Page : /p", new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(5, 5, 5, 5, 5, 5);
            clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape
                                            , PrintType.All, 0, 0, true  , true, true, false, false, false, false);

            spread.setSpdPrint(this.ssMain, true, margin, option, header, foot);
        }

        void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname)
        {

        }

        void setCtrlTitle()
        {
            string strTitle = "";

            if (this.gEnmType == enmMasterType.EXAM)
            {
                strTitle = "진단검사의학과 검사코드";
            }
            else if (this.gEnmType == enmMasterType.PTHL)
            {
                strTitle = "해부병리 검사코드";
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
            //this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            ds = lbExSQL.sel_EXAM_MASTER_SEARCH(clsDB.DbCon, this.gEnmType);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, lbExSQL.sSel_EXAM_MASTER_SEARCH, lbExSQL.nSel_EXAM_MASTER_SEARCH });
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

            //this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
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
        }

    }
}
