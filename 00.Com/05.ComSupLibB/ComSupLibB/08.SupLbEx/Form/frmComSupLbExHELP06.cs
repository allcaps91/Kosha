using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP04.cs
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록
    /// Create Date     : 2018-04-06
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "신규" />
    public partial class frmComSupLbExHELP06: Form
    {
        clsComSupLbExRsltSQL rsltSQL = new clsComSupLbExRsltSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        string gStrPANO;
        string gStrSUBCODE;
        string gStrSNAME;
        string gStrEXAMNM;

        public frmComSupLbExHELP06(string strPANO, string strSUBCODE, string strSNAME, string strEXAMNM)
        {
            InitializeComponent();

            this.gStrPANO       = strPANO;
            this.gStrSUBCODE    = strSUBCODE;
            this.gStrSNAME      = strSNAME;
            this.gStrEXAMNM     = strEXAMNM;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.ss_EXAM_RESULT.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow((FpSpread)sender, e.Column);
                return;
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
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                this.label1.Text = this.gStrPANO + "/" + this.gStrSNAME + "/" + this.gStrSUBCODE + "/" + this.gStrEXAMNM;

                setCtrlSpread();
            }
        }

        void setCtrlSpread()
        {
            DataSet ds = rsltSQL.sel_EXAM_RESULTC_HIS(clsDB.DbCon, this.gStrPANO, this.gStrSUBCODE);

            if (ComFunc.isDataSetNull(ds) == true)
            {
                ComFunc.MsgBox("전결과가 존재 하지 않습니다.");
                this.Close();
            }
            else
            {
                setSpdStyle(this.ss_EXAM_RESULT, ds, rsltSQL.sSel_EXAM_RESULTC_HIS, rsltSQL.nSel_EXAM_RESULTC_HIS);
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 40;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            spd.ActiveSheet.ColumnCount = 0;

            spd.ActiveSheet.ColumnCount = colName.Length;
            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            spd.DataSource = ds;

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spread.setSpdFilter(spd, (int)clsComSupLbExRsltSQL.enmSel_EXAM_RESULTC_HIS.SPECNO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSupLbExRsltSQL.enmSel_EXAM_RESULTC_HIS.SPEC_NM, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSupLbExRsltSQL.enmSel_EXAM_RESULTC_HIS.RESULT, AutoFilterMode.EnhancedContextMenu, true);


        }
    }
}
